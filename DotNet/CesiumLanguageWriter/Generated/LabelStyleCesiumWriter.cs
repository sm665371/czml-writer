﻿// <auto-generated>
// This file was generated automatically by GenerateFromSchema. Do NOT edit it.
// https://github.com/AnalyticalGraphicsInc/czml-writer
// </auto-generated>

using CesiumLanguageWriter.Advanced;
using System;
using JetBrains.Annotations;

namespace CesiumLanguageWriter
{
    /// <summary>
    /// Writes a <c>LabelStyle</c> to a <see cref="CesiumOutputStream"/>. A <c>LabelStyle</c> is the style of a label.
    /// </summary>
    public class LabelStyleCesiumWriter : CesiumPropertyWriter<LabelStyleCesiumWriter>, ICesiumDeletablePropertyWriter, ICesiumLabelStyleValuePropertyWriter, ICesiumReferenceValuePropertyWriter
    {
        /// <summary>
        /// The name of the <c>labelStyle</c> property.
        /// </summary>
        [NotNull]
        public const string LabelStylePropertyName = "labelStyle";

        /// <summary>
        /// The name of the <c>reference</c> property.
        /// </summary>
        [NotNull]
        public const string ReferencePropertyName = "reference";

        /// <summary>
        /// The name of the <c>delete</c> property.
        /// </summary>
        [NotNull]
        public const string DeletePropertyName = "delete";

        [NotNull]
        [CSToJavaFinalField]
        private readonly Lazy<CesiumLabelStyleValuePropertyAdaptor<LabelStyleCesiumWriter>> m_asLabelStyle;
        [NotNull]
        [CSToJavaFinalField]
        private readonly Lazy<CesiumReferenceValuePropertyAdaptor<LabelStyleCesiumWriter>> m_asReference;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        public LabelStyleCesiumWriter([NotNull] string propertyName)
            : base(propertyName)
        {
            m_asLabelStyle = CreateAsLabelStyle();
            m_asReference = CreateAsReference();
        }

        /// <summary>
        /// Initializes a new instance as a copy of an existing instance.
        /// </summary>
        /// <param name="existingInstance">The existing instance to copy.</param>
        protected LabelStyleCesiumWriter([NotNull] LabelStyleCesiumWriter existingInstance)
            : base(existingInstance)
        {
            m_asLabelStyle = CreateAsLabelStyle();
            m_asReference = CreateAsReference();
        }

        /// <inheritdoc/>
        public override LabelStyleCesiumWriter Clone()
        {
            return new LabelStyleCesiumWriter(this);
        }

        /// <summary>
        /// Writes the value expressed as a <c>labelStyle</c>, which is the label style.
        /// </summary>
        /// <param name="value">The label style.</param>
        public void WriteLabelStyle(CesiumLabelStyle value)
        {
            const string PropertyName = LabelStylePropertyName;
            if (ForceInterval)
            {
                OpenIntervalIfNecessary();
            }
            if (IsInterval)
            {
                Output.WritePropertyName(PropertyName);
            }
            Output.WriteValue(CesiumFormattingHelper.LabelStyleToString(value));
        }

        /// <summary>
        /// Writes the value expressed as a <c>reference</c>, which is the label style specified as a reference to another property.
        /// </summary>
        /// <param name="value">The reference.</param>
        public void WriteReference(Reference value)
        {
            const string PropertyName = ReferencePropertyName;
            OpenIntervalIfNecessary();
            Output.WritePropertyName(PropertyName);
            CesiumWritingHelper.WriteReference(Output, value);
        }

        /// <summary>
        /// Writes the value expressed as a <c>reference</c>, which is the label style specified as a reference to another property.
        /// </summary>
        /// <param name="value">The reference.</param>
        public void WriteReference(string value)
        {
            const string PropertyName = ReferencePropertyName;
            OpenIntervalIfNecessary();
            Output.WritePropertyName(PropertyName);
            CesiumWritingHelper.WriteReference(Output, value);
        }

        /// <summary>
        /// Writes the value expressed as a <c>reference</c>, which is the label style specified as a reference to another property.
        /// </summary>
        /// <param name="identifier">The identifier of the object which contains the referenced property.</param>
        /// <param name="propertyName">The property on the referenced object.</param>
        public void WriteReference(string identifier, string propertyName)
        {
            const string PropertyName = ReferencePropertyName;
            OpenIntervalIfNecessary();
            Output.WritePropertyName(PropertyName);
            CesiumWritingHelper.WriteReference(Output, identifier, propertyName);
        }

        /// <summary>
        /// Writes the value expressed as a <c>reference</c>, which is the label style specified as a reference to another property.
        /// </summary>
        /// <param name="identifier">The identifier of the object which contains the referenced property.</param>
        /// <param name="propertyNames">The hierarchy of properties to be indexed on the referenced object.</param>
        public void WriteReference(string identifier, string[] propertyNames)
        {
            const string PropertyName = ReferencePropertyName;
            OpenIntervalIfNecessary();
            Output.WritePropertyName(PropertyName);
            CesiumWritingHelper.WriteReference(Output, identifier, propertyNames);
        }

        /// <summary>
        /// Writes the value expressed as a <c>delete</c>, which is whether the client should delete existing samples or interval data for this property. Data will be deleted for the containing interval, or if there is no containing interval, then all data. If true, all other properties in this property will be ignored.
        /// </summary>
        /// <param name="value">The value.</param>
        public void WriteDelete(bool value)
        {
            const string PropertyName = DeletePropertyName;
            OpenIntervalIfNecessary();
            Output.WritePropertyName(PropertyName);
            Output.WriteValue(value);
        }

        /// <summary>
        /// Returns a wrapper for this instance that implements <see cref="ICesiumLabelStyleValuePropertyWriter"/>. Because the returned instance is a wrapper for this instance, you may call <see cref="ICesiumElementWriter.Close"/> on either this instance or the wrapper, but you must not call it on both.
        /// </summary>
        /// <returns>The wrapper.</returns>
        [NotNull]
        public CesiumLabelStyleValuePropertyAdaptor<LabelStyleCesiumWriter> AsLabelStyle()
        {
            return m_asLabelStyle.Value;
        }

        [NotNull]
        private Lazy<CesiumLabelStyleValuePropertyAdaptor<LabelStyleCesiumWriter>> CreateAsLabelStyle()
        {
            return new Lazy<CesiumLabelStyleValuePropertyAdaptor<LabelStyleCesiumWriter>>(CreateLabelStyle, false);
        }

        [NotNull]
        private CesiumLabelStyleValuePropertyAdaptor<LabelStyleCesiumWriter> CreateLabelStyle()
        {
            return CesiumValuePropertyAdaptors.CreateLabelStyle(this);
        }

        /// <summary>
        /// Returns a wrapper for this instance that implements <see cref="ICesiumReferenceValuePropertyWriter"/>. Because the returned instance is a wrapper for this instance, you may call <see cref="ICesiumElementWriter.Close"/> on either this instance or the wrapper, but you must not call it on both.
        /// </summary>
        /// <returns>The wrapper.</returns>
        [NotNull]
        public CesiumReferenceValuePropertyAdaptor<LabelStyleCesiumWriter> AsReference()
        {
            return m_asReference.Value;
        }

        [NotNull]
        private Lazy<CesiumReferenceValuePropertyAdaptor<LabelStyleCesiumWriter>> CreateAsReference()
        {
            return new Lazy<CesiumReferenceValuePropertyAdaptor<LabelStyleCesiumWriter>>(CreateReference, false);
        }

        [NotNull]
        private CesiumReferenceValuePropertyAdaptor<LabelStyleCesiumWriter> CreateReference()
        {
            return CesiumValuePropertyAdaptors.CreateReference(this);
        }

    }
}
