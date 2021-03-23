﻿// <auto-generated>
// This file was generated automatically by GenerateFromSchema. Do NOT edit it.
// https://github.com/AnalyticalGraphicsInc/czml-writer
// </auto-generated>

using CesiumLanguageWriter.Advanced;
using System;
using JetBrains.Annotations;
using System.Collections.Generic;

namespace CesiumLanguageWriter
{
    /// <summary>
    /// Writes a <c>BoxDimensions</c> to a <see cref="CesiumOutputStream"/>. A <c>BoxDimensions</c> is the width, depth, and height of a box.
    /// </summary>
    public class BoxDimensionsCesiumWriter : CesiumInterpolatablePropertyWriter<BoxDimensionsCesiumWriter>, ICesiumDeletablePropertyWriter, ICesiumCartesian3ValuePropertyWriter, ICesiumReferenceValuePropertyWriter
    {
        /// <summary>
        /// The name of the <c>cartesian</c> property.
        /// </summary>
        [NotNull]
        public const string CartesianPropertyName = "cartesian";

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
        private readonly Lazy<CesiumCartesian3ValuePropertyAdaptor<BoxDimensionsCesiumWriter>> m_asCartesian;
        [NotNull]
        [CSToJavaFinalField]
        private readonly Lazy<CesiumReferenceValuePropertyAdaptor<BoxDimensionsCesiumWriter>> m_asReference;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        public BoxDimensionsCesiumWriter([NotNull] string propertyName)
            : base(propertyName)
        {
            m_asCartesian = CreateAsCartesian();
            m_asReference = CreateAsReference();
        }

        /// <summary>
        /// Initializes a new instance as a copy of an existing instance.
        /// </summary>
        /// <param name="existingInstance">The existing instance to copy.</param>
        protected BoxDimensionsCesiumWriter([NotNull] BoxDimensionsCesiumWriter existingInstance)
            : base(existingInstance)
        {
            m_asCartesian = CreateAsCartesian();
            m_asReference = CreateAsReference();
        }

        /// <inheritdoc/>
        public override BoxDimensionsCesiumWriter Clone()
        {
            return new BoxDimensionsCesiumWriter(this);
        }

        /// <summary>
        /// Writes the value expressed as a <c>cartesian</c>, which is the dimensions specified as a three-dimensional Cartesian value <c>[X, Y, Z]</c>, with X representing width, Y representing depth, and Z representing height, in world coordinates in meters.
        /// </summary>
        /// <param name="value">The value.</param>
        public void WriteCartesian(Cartesian value)
        {
            const string PropertyName = CartesianPropertyName;
            OpenIntervalIfNecessary();
            Output.WritePropertyName(PropertyName);
            CesiumWritingHelper.WriteCartesian3(Output, value);
        }

        /// <summary>
        /// Writes the value expressed as a <c>cartesian</c>, which is the dimensions specified as a three-dimensional Cartesian value <c>[X, Y, Z]</c>, with X representing width, Y representing depth, and Z representing height, in world coordinates in meters.
        /// </summary>
        /// <param name="dates">The dates at which the value is specified.</param>
        /// <param name="values">The values corresponding to each date.</param>
        public void WriteCartesian(IList<JulianDate> dates, IList<Cartesian> values)
        {
            WriteCartesian(dates, values, 0, dates.Count);
        }

        /// <summary>
        /// Writes the value expressed as a <c>cartesian</c>, which is the dimensions specified as a three-dimensional Cartesian value <c>[X, Y, Z]</c>, with X representing width, Y representing depth, and Z representing height, in world coordinates in meters.
        /// </summary>
        /// <param name="dates">The dates at which the value is specified.</param>
        /// <param name="values">The values corresponding to each date.</param>
        /// <param name="startIndex">The index of the first element to write.</param>
        /// <param name="length">The number of elements to write.</param>
        public void WriteCartesian(IList<JulianDate> dates, IList<Cartesian> values, int startIndex, int length)
        {
            const string PropertyName = CartesianPropertyName;
            OpenIntervalIfNecessary();
            CesiumWritingHelper.WriteCartesian3(Output, PropertyName, dates, values, startIndex, length);
        }

        /// <summary>
        /// Writes the value expressed as a <c>reference</c>, which is the dimensions specified as a reference to another property.
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
        /// Writes the value expressed as a <c>reference</c>, which is the dimensions specified as a reference to another property.
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
        /// Writes the value expressed as a <c>reference</c>, which is the dimensions specified as a reference to another property.
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
        /// Writes the value expressed as a <c>reference</c>, which is the dimensions specified as a reference to another property.
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
        /// Returns a wrapper for this instance that implements <see cref="ICesiumCartesian3ValuePropertyWriter"/>. Because the returned instance is a wrapper for this instance, you may call <see cref="ICesiumElementWriter.Close"/> on either this instance or the wrapper, but you must not call it on both.
        /// </summary>
        /// <returns>The wrapper.</returns>
        [NotNull]
        public CesiumCartesian3ValuePropertyAdaptor<BoxDimensionsCesiumWriter> AsCartesian()
        {
            return m_asCartesian.Value;
        }

        [NotNull]
        private Lazy<CesiumCartesian3ValuePropertyAdaptor<BoxDimensionsCesiumWriter>> CreateAsCartesian()
        {
            return new Lazy<CesiumCartesian3ValuePropertyAdaptor<BoxDimensionsCesiumWriter>>(CreateCartesian3, false);
        }

        [NotNull]
        private CesiumCartesian3ValuePropertyAdaptor<BoxDimensionsCesiumWriter> CreateCartesian3()
        {
            return CesiumValuePropertyAdaptors.CreateCartesian3(this);
        }

        /// <summary>
        /// Returns a wrapper for this instance that implements <see cref="ICesiumReferenceValuePropertyWriter"/>. Because the returned instance is a wrapper for this instance, you may call <see cref="ICesiumElementWriter.Close"/> on either this instance or the wrapper, but you must not call it on both.
        /// </summary>
        /// <returns>The wrapper.</returns>
        [NotNull]
        public CesiumReferenceValuePropertyAdaptor<BoxDimensionsCesiumWriter> AsReference()
        {
            return m_asReference.Value;
        }

        [NotNull]
        private Lazy<CesiumReferenceValuePropertyAdaptor<BoxDimensionsCesiumWriter>> CreateAsReference()
        {
            return new Lazy<CesiumReferenceValuePropertyAdaptor<BoxDimensionsCesiumWriter>>(CreateReference, false);
        }

        [NotNull]
        private CesiumReferenceValuePropertyAdaptor<BoxDimensionsCesiumWriter> CreateReference()
        {
            return CesiumValuePropertyAdaptors.CreateReference(this);
        }

    }
}
