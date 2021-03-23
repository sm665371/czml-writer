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
    /// Writes a <c>LineOffset</c> to a <see cref="CesiumOutputStream"/>. A <c>LineOffset</c> is the offset of grid lines along each axis, as a percentage from 0 to 1.
    /// </summary>
    public class LineOffsetCesiumWriter : CesiumInterpolatablePropertyWriter<LineOffsetCesiumWriter>, ICesiumDeletablePropertyWriter, ICesiumCartesian2ValuePropertyWriter, ICesiumReferenceValuePropertyWriter
    {
        /// <summary>
        /// The name of the <c>cartesian2</c> property.
        /// </summary>
        [NotNull]
        public const string Cartesian2PropertyName = "cartesian2";

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
        private readonly Lazy<CesiumCartesian2ValuePropertyAdaptor<LineOffsetCesiumWriter>> m_asCartesian2;
        [NotNull]
        [CSToJavaFinalField]
        private readonly Lazy<CesiumReferenceValuePropertyAdaptor<LineOffsetCesiumWriter>> m_asReference;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        public LineOffsetCesiumWriter([NotNull] string propertyName)
            : base(propertyName)
        {
            m_asCartesian2 = CreateAsCartesian2();
            m_asReference = CreateAsReference();
        }

        /// <summary>
        /// Initializes a new instance as a copy of an existing instance.
        /// </summary>
        /// <param name="existingInstance">The existing instance to copy.</param>
        protected LineOffsetCesiumWriter([NotNull] LineOffsetCesiumWriter existingInstance)
            : base(existingInstance)
        {
            m_asCartesian2 = CreateAsCartesian2();
            m_asReference = CreateAsReference();
        }

        /// <inheritdoc/>
        public override LineOffsetCesiumWriter Clone()
        {
            return new LineOffsetCesiumWriter(this);
        }

        /// <summary>
        /// Writes the value expressed as a <c>cartesian2</c>, which is the offset of grid lines along each axis, specified as a percentage from 0 to 1.
        /// </summary>
        /// <param name="value">The value.</param>
        public void WriteCartesian2(Rectangular value)
        {
            const string PropertyName = Cartesian2PropertyName;
            OpenIntervalIfNecessary();
            Output.WritePropertyName(PropertyName);
            CesiumWritingHelper.WriteCartesian2(Output, value);
        }

        /// <summary>
        /// Writes the value expressed as a <c>cartesian2</c>, which is the offset of grid lines along each axis, specified as a percentage from 0 to 1.
        /// </summary>
        /// <param name="x">The X component.</param>
        /// <param name="y">The Y component.</param>
        public void WriteCartesian2(double x, double y)
        {
            WriteCartesian2(new Rectangular(x, y));
        }

        /// <summary>
        /// Writes the value expressed as a <c>cartesian2</c>, which is the offset of grid lines along each axis, specified as a percentage from 0 to 1.
        /// </summary>
        /// <param name="dates">The dates at which the value is specified.</param>
        /// <param name="values">The values corresponding to each date.</param>
        public void WriteCartesian2(IList<JulianDate> dates, IList<Rectangular> values)
        {
            WriteCartesian2(dates, values, 0, dates.Count);
        }

        /// <summary>
        /// Writes the value expressed as a <c>cartesian2</c>, which is the offset of grid lines along each axis, specified as a percentage from 0 to 1.
        /// </summary>
        /// <param name="dates">The dates at which the value is specified.</param>
        /// <param name="values">The values corresponding to each date.</param>
        /// <param name="startIndex">The index of the first element to write.</param>
        /// <param name="length">The number of elements to write.</param>
        public void WriteCartesian2(IList<JulianDate> dates, IList<Rectangular> values, int startIndex, int length)
        {
            const string PropertyName = Cartesian2PropertyName;
            OpenIntervalIfNecessary();
            CesiumWritingHelper.WriteCartesian2(Output, PropertyName, dates, values, startIndex, length);
        }

        /// <summary>
        /// Writes the value expressed as a <c>reference</c>, which is the offset of grid lines along each axis specified as a reference to another property.
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
        /// Writes the value expressed as a <c>reference</c>, which is the offset of grid lines along each axis specified as a reference to another property.
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
        /// Writes the value expressed as a <c>reference</c>, which is the offset of grid lines along each axis specified as a reference to another property.
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
        /// Writes the value expressed as a <c>reference</c>, which is the offset of grid lines along each axis specified as a reference to another property.
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
        /// Returns a wrapper for this instance that implements <see cref="ICesiumCartesian2ValuePropertyWriter"/>. Because the returned instance is a wrapper for this instance, you may call <see cref="ICesiumElementWriter.Close"/> on either this instance or the wrapper, but you must not call it on both.
        /// </summary>
        /// <returns>The wrapper.</returns>
        [NotNull]
        public CesiumCartesian2ValuePropertyAdaptor<LineOffsetCesiumWriter> AsCartesian2()
        {
            return m_asCartesian2.Value;
        }

        [NotNull]
        private Lazy<CesiumCartesian2ValuePropertyAdaptor<LineOffsetCesiumWriter>> CreateAsCartesian2()
        {
            return new Lazy<CesiumCartesian2ValuePropertyAdaptor<LineOffsetCesiumWriter>>(CreateCartesian2, false);
        }

        [NotNull]
        private CesiumCartesian2ValuePropertyAdaptor<LineOffsetCesiumWriter> CreateCartesian2()
        {
            return CesiumValuePropertyAdaptors.CreateCartesian2(this);
        }

        /// <summary>
        /// Returns a wrapper for this instance that implements <see cref="ICesiumReferenceValuePropertyWriter"/>. Because the returned instance is a wrapper for this instance, you may call <see cref="ICesiumElementWriter.Close"/> on either this instance or the wrapper, but you must not call it on both.
        /// </summary>
        /// <returns>The wrapper.</returns>
        [NotNull]
        public CesiumReferenceValuePropertyAdaptor<LineOffsetCesiumWriter> AsReference()
        {
            return m_asReference.Value;
        }

        [NotNull]
        private Lazy<CesiumReferenceValuePropertyAdaptor<LineOffsetCesiumWriter>> CreateAsReference()
        {
            return new Lazy<CesiumReferenceValuePropertyAdaptor<LineOffsetCesiumWriter>>(CreateReference, false);
        }

        [NotNull]
        private CesiumReferenceValuePropertyAdaptor<LineOffsetCesiumWriter> CreateReference()
        {
            return CesiumValuePropertyAdaptors.CreateReference(this);
        }

    }
}
