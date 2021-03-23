﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace GenerateFromSchema
{
    public class CSharpGenerator : Generator
    {
        private readonly string m_outputDirectory;
        private readonly Configuration m_configuration;
        [NotNull]
        private readonly HashSet<Schema> m_writtenSchemas = new HashSet<Schema>();

        public CSharpGenerator(string outputDirectory, string configurationFileName)
        {
            m_outputDirectory = outputDirectory ?? throw new ArgumentNullException(nameof(outputDirectory));

            string configuration = File.ReadAllText(configurationFileName ?? throw new ArgumentNullException(nameof(configurationFileName)));
            m_configuration = JsonConvert.DeserializeObject<Configuration>(configuration);
        }

        public override void Generate(Schema schema)
        {
            GeneratePacketWriter(schema);
        }

        private void GenerateWriterClass(Schema schema)
        {
            if (m_writtenSchemas.Contains(schema))
                return;

            m_writtenSchemas.Add(schema);

            using (var writer = new CodeWriter(Path.Combine(m_outputDirectory, schema.NameWithPascalCase + "CesiumWriter.cs")))
            {
                WriteGeneratedWarning(writer);
                writer.WriteLine();
                WriteUsingStatements(writer, schema);
                writer.WriteLine();

                writer.WriteLine("namespace {0}", m_configuration.Namespace);
                using (writer.OpenScope())
                {
                    WriteDescriptionAsClassSummary(writer, schema);

                    foreach (string attribute in m_configuration.Attributes ?? Array.Empty<string>())
                    {
                        writer.WriteLine("[{0}]", attribute);
                    }

                    string baseClass = schema.IsInterpolatable ? "CesiumInterpolatablePropertyWriter" : "CesiumPropertyWriter";
                    string interfaces = string.Join(", ", schema.Interfaces);
                    if (interfaces.Length > 0)
                        interfaces = $", {interfaces}";

                    writer.WriteLine("{0} class {1} : {2}<{1}>{3}", m_configuration.Access, GetWriterType(schema), baseClass, interfaces);

                    using (writer.OpenScope())
                    {
                        WritePropertyNameConstants(writer, schema);
                        WritePropertyLazyFields(writer, schema);
                        WriteConstructorsAndCloneMethod(writer, schema);
                        WriteProperties(writer, schema);
                        WriteAsTypeMethods(writer, schema);
                    }
                }
            }
        }

        private void GeneratePacketWriter(Schema packetSchema)
        {
            string packetWriterFilename = Path.Combine(m_outputDirectory, "PacketCesiumWriter.cs");
            using (var writer = new CodeWriter(packetWriterFilename))
            {
                WriteGeneratedWarning(writer);
                writer.WriteLine();
                WriteUsingStatements(writer, packetSchema);
                writer.WriteLine();

                writer.WriteLine("namespace {0}", m_configuration.Namespace);

                using (writer.OpenScope())
                {
                    WriteDescriptionAsClassSummary(writer, packetSchema);

                    foreach (string attribute in m_configuration.Attributes ?? Array.Empty<string>())
                    {
                        writer.WriteLine("[{0}]", attribute);
                    }

                    writer.WriteLine("{0} class PacketCesiumWriter : CesiumElementWriter", m_configuration.Access);

                    using (writer.OpenScope())
                    {
                        WritePropertyNameConstants(writer, packetSchema);
                        WritePropertyLazyFields(writer, packetSchema);
                        WritePacketOpenClose(writer);
                        WriteProperties(writer, packetSchema);
                    }
                }
            }
        }

        private static void WriteGeneratedWarning(CodeWriter writer)
        {
            writer.WriteLine("// <auto-generated>");
            writer.WriteLine("// This file was generated automatically by GenerateFromSchema. Do NOT edit it.");
            writer.WriteLine("// https://github.com/AnalyticalGraphicsInc/czml-writer");
            writer.WriteLine("// </auto-generated>");
        }

        private void WriteUsingStatements(CodeWriter writer, Schema schema)
        {
            var namespaces = new List<string>
            {
                m_configuration.Namespace + ".Advanced",
                "System",
                "JetBrains.Annotations",
            };

            namespaces.AddRange(m_configuration.UsingNamespaces ?? Enumerable.Empty<string>());

            namespaces.AddRange(schema.AllProperties
                                      .SelectMany(property => GetOverloadsForProperty(property).Concat(property.ValueType.Properties.SelectMany(GetOverloadsForProperty)))
                                      .SelectMany(overload => overload.UsingNamespaces ?? Enumerable.Empty<string>()));

            foreach (string ns in namespaces.Distinct())
            {
                writer.WriteLine("using {0};", ns);
            }
        }

        private static void WriteSummaryText(CodeWriter writer, string text)
        {
            writer.WriteLine("/// <summary>");
            writer.WriteLine("/// {0}", text);
            writer.WriteLine("/// </summary>");
        }

        [StringFormatMethod("format")]
        private static void WriteSummaryText(CodeWriter writer, string format, params object[] args)
        {
            WriteSummaryText(writer, string.Format(format, args));
        }

        private static void WriteInheritDoc(CodeWriter writer)
        {
            writer.WriteLine("/// <inheritdoc/>");
        }

        private static void WriteParameterText(CodeWriter writer, string parameterName, string description)
        {
            writer.WriteLine("/// <param name=\"{0}\">{1}</param>", parameterName, description);
        }

        private static void WriteTypeParameterText(CodeWriter writer, string typeName, string description)
        {
            writer.WriteLine("/// <typeparam name=\"{0}\">{1}</typeparam>", typeName, description);
        }

        private static void WriteReturnsText(CodeWriter writer, string description)
        {
            writer.WriteLine("/// <returns>{0}</returns>", description);
        }

        private static void WriteDescriptionAsClassSummary(CodeWriter writer, Schema schema)
        {
            string description = schema.Description.UncapitalizeFirstLetter();
            description = s_markdownRegex.Replace(description, match => $"<c>{match.Groups[1].Value}</c>");

            WriteSummaryText(writer, "Writes a <c>{0}</c> to a <see cref=\"CesiumOutputStream\"/>. A <c>{0}</c> is {1}", schema.Name, description);
        }

        private static void WritePropertyNameConstants(CodeWriter writer, Schema schema)
        {
            foreach (var property in schema.AllProperties)
            {
                WriteSummaryText(writer, "The name of the <c>{0}</c> property.", property.Name);
                writer.WriteLine("[NotNull]");
                writer.WriteLine("public const string {0}PropertyName = \"{1}\";", property.NameWithPascalCase, property.Name);
                writer.WriteLine();
            }
        }

        private void WritePropertyLazyFields(CodeWriter writer, Schema schema)
        {
            foreach (Property property in schema.AllProperties)
            {
                if (PropertyValueIsLeaf(property))
                {
                    if (property.IsValue)
                    {
                        // Does this property have an overload to write sampled data?
                        // If so, it's interpolatable.
                        OverloadInfo firstOverload = GetOverloadsForProperty(property).First();
                        if (firstOverload.Parameters.Length != 1)
                            continue;

                        string adaptorType = GetAdaptorType(schema, property);

                        writer.WriteLine("[NotNull]");
                        writer.WriteLine("[CSToJavaFinalField]");
                        writer.WriteLine("private readonly Lazy<{0}> m_as{1};", adaptorType, property.NameWithPascalCase);
                    }
                }
                else
                {
                    writer.WriteLine("private readonly Lazy<{0}> m_{1} = new Lazy<{0}>(() => new {0}({2}PropertyName), false);",
                                     GetWriterType(property.ValueType),
                                     property.Name,
                                     property.NameWithPascalCase);
                }
            }

            if (schema.AllProperties.Count > 0)
                writer.WriteLine();
        }

        private static void WritePacketOpenClose(CodeWriter writer)
        {
            WriteSummaryText(writer, "Writes the start of a new JSON object representing the packet.");
            writer.WriteLine("protected override void OnOpen()");
            using (writer.OpenScope())
            {
                writer.WriteLine("Output.WriteStartObject();");
            }

            writer.WriteLine();

            WriteSummaryText(writer, "Writes the end of the JSON object representing the packet.");
            writer.WriteLine("protected override void OnClose()");
            using (writer.OpenScope())
            {
                writer.WriteLine("Output.WriteEndObject();");
            }

            writer.WriteLine();
        }

        private static bool PropertyValueIsLeaf(Property property)
        {
            JsonSchemaType jsonTypes = property.ValueType.JsonTypes;
            return !jsonTypes.HasFlag(JsonSchemaType.Object);
        }

        private void WriteProperties(CodeWriter writer, Schema schema)
        {
            bool isFirstValueProperty = true;

            foreach (Property property in schema.AllProperties)
            {
                if (PropertyValueIsLeaf(property))
                {
                    WriteLeafProperty(writer, schema, property, property.IsValue && isFirstValueProperty);
                }
                else
                {
                    WriteIntervalsProperty(writer, schema, property);
                }

                if (property.IsValue)
                {
                    isFirstValueProperty = false;
                }
            }

            var additionalProperties = schema.AdditionalProperties;
            if (additionalProperties != null)
            {
                var additionalPropertiesValueType = additionalProperties.ValueType;
                GenerateWriterClass(additionalPropertiesValueType);

                string writerType = GetWriterType(additionalPropertiesValueType);

                WriteSummaryText(writer, "Gets a new writer for a <c>{0}</c> property. The returned instance must be opened by calling the <see cref=\"CesiumElementWriter.Open\"/> method before it can be used for writing. A <c>{0}</c> property defines {1}", additionalPropertiesValueType.Name, GetDescription(additionalProperties));
                writer.WriteLine("public {0} Get{1}Writer(string name)", writerType, additionalPropertiesValueType.NameWithPascalCase);
                using (writer.OpenScope())
                {
                    writer.WriteLine("return new {0}(name);", writerType);
                }

                writer.WriteLine();

                WriteSummaryText(writer, "Opens and returns a new writer for a <c>{0}</c> property. A <c>{0}</c> property defines {1}", additionalPropertiesValueType.Name, GetDescription(additionalProperties));
                WriteParameterText(writer, "name", "The name of the new property writer.");
                writer.WriteLine("public {0} Open{1}Property(string name)", writerType, additionalPropertiesValueType.NameWithPascalCase);
                using (writer.OpenScope())
                {
                    writer.WriteLine("OpenIntervalIfNecessary();");
                    writer.WriteLine("return OpenAndReturn(new {0}(name));", writerType);
                }

                writer.WriteLine();
            }
        }

        private void WriteIntervalsProperty(CodeWriter writer, Schema schema, Property property)
        {
            GenerateWriterClass(property.ValueType);

            string writerType = GetWriterType(property.ValueType);

            WriteSummaryText(writer, "Gets the writer for the <c>{0}</c> property. The returned instance must be opened by calling the <see cref=\"CesiumElementWriter.Open\"/> method before it can be used for writing. The <c>{0}</c> property defines {1}", property.Name, GetDescription(property));
            writer.WriteLine("[NotNull]");
            writer.WriteLine("public {0} {1}Writer", writerType, property.NameWithPascalCase);
            using (writer.OpenScope())
            {
                writer.WriteLine("get {{ return m_{0}.Value; }}", property.Name);
            }

            writer.WriteLine();

            WriteSummaryText(writer, "Opens and returns the writer for the <c>{0}</c> property. The <c>{0}</c> property defines {1}", property.Name, GetDescription(property));
            writer.WriteLine("[NotNull]");
            writer.WriteLine("public {0} Open{1}Property()", writerType, property.NameWithPascalCase);
            using (writer.OpenScope())
            {
                if (schema.Name != "Packet")
                    writer.WriteLine("OpenIntervalIfNecessary();");
                writer.WriteLine("return OpenAndReturn({0}Writer);", property.NameWithPascalCase);
            }

            writer.WriteLine();

            bool isFirstValueProperty = true;
            foreach (var nestedProperty in property.ValueType.AllProperties.Where(p => p.IsValue))
            {
                foreach (var overload in GetOverloadsForProperty(nestedProperty))
                {
                    WriteSummaryText(writer, "Writes a value for the <c>{0}</c> property as a <c>{1}</c> value. The <c>{0}</c> property specifies {2}", property.Name, nestedProperty.Name, GetDescription(property));
                    foreach (var parameter in overload.Parameters.Where(p => !string.IsNullOrEmpty(p.Description)))
                    {
                        WriteParameterText(writer, parameter.Name, parameter.Description);
                    }

                    string subPropertyName = nestedProperty.NameWithPascalCase;
                    if (subPropertyName == property.NameWithPascalCase || isFirstValueProperty)
                        subPropertyName = "";

                    writer.WriteLine("public void Write{0}Property{1}({2})", property.NameWithPascalCase, subPropertyName, overload.FormattedParameters);
                    using (writer.OpenScope())
                    {
                        writer.WriteLine("using (var writer = Open{0}Property())", property.NameWithPascalCase);
                        using (writer.OpenScope())
                        {
                            writer.WriteLine("writer.Write{0}({1});", nestedProperty.NameWithPascalCase, string.Join(", ", Array.ConvertAll(overload.Parameters, p => p.Name)));
                        }
                    }

                    writer.WriteLine();
                }

                isFirstValueProperty = false;
            }
        }

        private void WriteLeafProperty(CodeWriter writer, Schema schema, Property property, bool isFirstValueProperty)
        {
            foreach (var overload in GetOverloadsForProperty(property))
            {
                WriteSummaryText(writer, "Writes the value expressed as a <c>{0}</c>, which is {1}", property.Name, GetDescription(property));
                foreach (var parameter in overload.Parameters.Where(p => !string.IsNullOrEmpty(p.Description)))
                {
                    WriteParameterText(writer, parameter.Name, parameter.Description);
                }

                writer.WriteLine("public void Write{0}({1})", property.NameWithPascalCase, overload.FormattedParameters);
                using (writer.OpenScope())
                {
                    if (overload.CallOverload != null)
                    {
                        writer.WriteLine("Write{0}({1});", property.NameWithPascalCase, overload.CallOverload);
                    }
                    else
                    {
                        writer.WriteLine("const string PropertyName = {0}PropertyName;", property.NameWithPascalCase);

                        if (schema.Name == "Packet")
                        {
                            writer.WriteLine("Output.WritePropertyName(PropertyName);");
                        }
                        else if (isFirstValueProperty && !overload.NeedsInterval)
                        {
                            // For the first value property only, if an overload is marked 
                            // as not needing an interval, because it writes a simple JSON 
                            // type (string, number, boolean), we can skip opening an interval 
                            // and just write the property value directly.
                            // Unless ForceInterval has been set to true.
                            writer.WriteLine("if (ForceInterval)");
                            using (writer.OpenScope())
                            {
                                writer.WriteLine("OpenIntervalIfNecessary();");
                            }

                            if (overload.WritePropertyName)
                            {
                                writer.WriteLine("if (IsInterval)");
                                using (writer.OpenScope())
                                {
                                    writer.WriteLine("Output.WritePropertyName(PropertyName);");
                                }
                            }
                        }
                        else
                        {
                            writer.WriteLine("OpenIntervalIfNecessary();");

                            if (overload.WritePropertyName)
                            {
                                writer.WriteLine("Output.WritePropertyName(PropertyName);");
                            }
                        }

                        writer.WriteLine(overload.WriteValue);
                    }
                }

                writer.WriteLine();
            }
        }

        private void WriteConstructorsAndCloneMethod(CodeWriter writer, Schema schema)
        {
            string writerType = GetWriterType(schema);

            WriteSummaryText(writer, "Initializes a new instance.");
            WriteParameterText(writer, "propertyName", "The name of the property.");
            writer.WriteLine("public {0}([NotNull] string propertyName)", writerType);
            writer.WriteLine("    : base(propertyName)");
            using (writer.OpenScope())
            {
                WriteAsTypeLazyInitialization(writer, schema);
            }

            writer.WriteLine();

            WriteSummaryText(writer, "Initializes a new instance as a copy of an existing instance.");
            WriteParameterText(writer, "existingInstance", "The existing instance to copy.");
            writer.WriteLine("protected {0}([NotNull] {0} existingInstance)", writerType);
            writer.WriteLine("    : base(existingInstance)");
            using (writer.OpenScope())
            {
                WriteAsTypeLazyInitialization(writer, schema);
            }

            writer.WriteLine();

            WriteInheritDoc(writer);
            writer.WriteLine("public override {0} Clone()", writerType);
            using (writer.OpenScope())
            {
                writer.WriteLine("return new {0}(this);", writerType);
            }

            writer.WriteLine();
        }

        private void WriteAsTypeLazyInitialization(CodeWriter writer, Schema schema)
        {
            foreach (Property property in schema.AllProperties)
            {
                if (!property.IsValue)
                    continue;

                if (PropertyValueIsLeaf(property))
                {
                    OverloadInfo firstOverload = GetOverloadsForProperty(property).First();
                    if (firstOverload.Parameters.Length != 1)
                        continue;

                    writer.WriteLine("m_as{0} = CreateAs{0}();", property.NameWithPascalCase);
                }
            }
        }

        private void WriteAsTypeMethods(CodeWriter writer, Schema schema)
        {
            foreach (var property in schema.AllProperties.Where(p => p.IsValue).Where(PropertyValueIsLeaf))
            {
                OverloadInfo firstOverload = GetOverloadsForProperty(property).First();
                if (firstOverload.Parameters.Length != 1)
                    continue;

                WriteSummaryText(writer, "Returns a wrapper for this instance that implements <see cref=\"ICesium{0}ValuePropertyWriter\"/>. Because the returned instance is a wrapper for this instance, you may call <see cref=\"ICesiumElementWriter.Close\"/> on either this instance or the wrapper, but you must not call it on both.", property.ValueType.NameWithPascalCase);
                WriteReturnsText(writer, "The wrapper.");

                string adaptorType = GetAdaptorType(schema, property);

                writer.WriteLine("[NotNull]");
                writer.WriteLine("public {0} As{1}()", adaptorType, property.NameWithPascalCase);
                using (writer.OpenScope())
                {
                    writer.WriteLine("return m_as{0}.Value;", property.NameWithPascalCase);
                }
                writer.WriteLine();

                writer.WriteLine("[NotNull]");
                writer.WriteLine("private Lazy<{0}> CreateAs{1}()", adaptorType, property.NameWithPascalCase);
                using (writer.OpenScope())
                {
                    writer.WriteLine("return new Lazy<{0}>(Create{1}, false);", adaptorType, property.ValueType.NameWithPascalCase);
                }
                writer.WriteLine();

                writer.WriteLine("[NotNull]");
                writer.WriteLine("private {0} Create{1}()", adaptorType, property.ValueType.NameWithPascalCase);
                using (writer.OpenScope())
                {
                    writer.WriteLine("return CesiumValuePropertyAdaptors.Create{0}(this);", property.ValueType.NameWithPascalCase);
                }
                writer.WriteLine();
            }
        }

        private static string GetAdaptorType(Schema schema, Property property)
        {
            return $"Cesium{property.ValueType.NameWithPascalCase}ValuePropertyAdaptor<{schema.NameWithPascalCase}CesiumWriter>";
        }

        private static string GetWriterType(Schema schema)
        {
            return $"{schema.NameWithPascalCase}CesiumWriter";
        }

        private IEnumerable<OverloadInfo> GetOverloadsForProperty(Property property)
        {
            if (property.ValueType.IsSchemaFromType)
            {
                JsonSchemaType type = property.ValueType.JsonTypes;

                if (type.HasFlag(JsonSchemaType.Object) ||
                    type.HasFlag(JsonSchemaType.Array) ||
                    type.HasFlag(JsonSchemaType.Null) ||
                    type.HasFlag(JsonSchemaType.Any) ||
                    type == JsonSchemaType.None)
                {
                    throw new Exception($"Property '{property.Name}' does not specify a $ref to a schema, nor is it a simple JSON type.");
                }

                if (type.HasFlag(JsonSchemaType.String))
                    yield return s_defaultStringOverload;

                if (type.HasFlag(JsonSchemaType.Float))
                    yield return s_defaultDoubleOverload;

                if (type.HasFlag(JsonSchemaType.Integer))
                    yield return s_defaultIntegerOverload;

                if (type.HasFlag(JsonSchemaType.Boolean))
                    yield return s_defaultBooleanOverload;
            }
            else
            {
                if (m_configuration.Types.TryGetValue(property.ValueType.Name, out var overloads))
                {
                    foreach (var overload in overloads)
                        yield return overload;
                }
                else
                {
                    yield return OverloadInfo.CreateDefault(property.ValueType.NameWithPascalCase);
                }
            }
        }

        private static readonly Regex s_markdownRegex = new Regex("`([^`]+)`", RegexOptions.Compiled);

        private static string GetDescription(Property property)
        {
            string description = property.Description.UncapitalizeFirstLetter();
            description = s_markdownRegex.Replace(description, match => $"<c>{match.Groups[1].Value}</c>");

            JToken defaultToken = property.Default;
            if (defaultToken != null)
            {
                string defaultText;
                switch (defaultToken.Type)
                {
                    case JTokenType.Boolean:
                        defaultText = $"<see langword=\"{(defaultToken.Value<bool>() ? "true" : "false")}\"/>";
                        break;
                    case JTokenType.Float:
                        defaultText = defaultToken.Value<double>().ToString("0.0###############", CultureInfo.InvariantCulture);
                        break;
                    default:
                        defaultText = defaultToken.Value<string>();
                        break;
                }

                description += $" If not specified, the default value is {defaultText}.";
            }

            if (property.IsRequiredForDisplay)
            {
                description += " This value must be specified in order for the client to display graphics.";
            }

            return description;
        }

        [SuppressMessage("ReSharper", "RedundantDefaultMemberInitializer")]
        private class ParameterInfo
        {
            [JsonProperty("type")]
            public string Type;

            [JsonProperty("name")]
            public string Name;

            [JsonProperty("description")]
            public string Description;

            [JsonProperty("attributes")]
            public string[] Attributes = null;

            public static ParameterInfo SimpleValue(string type)
            {
                return new ParameterInfo
                {
                    Type = type,
                    Name = "value",
                    Description = "The value."
                };
            }

            public string Format()
            {
                string attributes = string.Join(" ", Array.ConvertAll(Attributes ?? Array.Empty<string>(), attribute => $"[{attribute}]"));
                if (attributes.Length > 0)
                    attributes += " ";

                return $"{attributes}{Type} {Name}";
            }
        }

        [SuppressMessage("ReSharper", "RedundantDefaultMemberInitializer")]
        private class OverloadInfo
        {
            [JsonProperty("usingNamespaces")]
            public string[] UsingNamespaces = null;

            [JsonProperty("parameters")]
            public ParameterInfo[] Parameters = null;

            [JsonProperty("writeValue")]
            public string WriteValue = null;

            [JsonProperty("callOverload")]
            public string CallOverload = null;

            [JsonProperty("writePropertyName")]
            public bool WritePropertyName = true;

            [JsonProperty("needsInterval")]
            public bool NeedsInterval = true;

            public string FormattedParameters => string.Join(", ", Array.ConvertAll(Parameters, parameter => parameter.Format()));

            public static OverloadInfo CreateDefault(string typeName)
            {
                return new OverloadInfo
                {
                    Parameters = new[] { ParameterInfo.SimpleValue(typeName) },
                    WriteValue = "Output.WriteValue(value);",
                };
            }
        }

        [SuppressMessage("ReSharper", "RedundantDefaultMemberInitializer")]
        private class Configuration
        {
            [JsonProperty("namespace")]
            public string Namespace = null;

            [JsonProperty("access")]
            public string Access = null;

            [JsonProperty("usingNamespaces")]
            public string[] UsingNamespaces = null;

            [JsonProperty("attributes")]
            public string[] Attributes = null;

            [JsonProperty("types")]
            public Dictionary<string, OverloadInfo[]> Types = null;
        }

        private static readonly OverloadInfo s_defaultStringOverload = OverloadInfo.CreateDefault("string");
        private static readonly OverloadInfo s_defaultDoubleOverload = OverloadInfo.CreateDefault("double");
        private static readonly OverloadInfo s_defaultIntegerOverload = OverloadInfo.CreateDefault("int");
        private static readonly OverloadInfo s_defaultBooleanOverload = OverloadInfo.CreateDefault("bool");
    }
}