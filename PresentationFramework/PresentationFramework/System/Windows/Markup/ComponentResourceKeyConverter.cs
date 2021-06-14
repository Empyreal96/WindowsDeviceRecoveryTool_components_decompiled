using System;
using System.ComponentModel;
using System.Globalization;

namespace System.Windows.Markup
{
	/// <summary>Implements a type converter for <see cref="T:System.Windows.ComponentResourceKey" /> objects, which deliberately have no type conversion pathways. The type converter enforces and reports that behavior.</summary>
	// Token: 0x020001C1 RID: 449
	public class ComponentResourceKeyConverter : ExpressionConverter
	{
		/// <summary>Determines whether an object of the specified type can be converted to an instance of <see cref="T:System.Windows.ComponentResourceKey" />, using the specified context. Always returns <see langword="false" />.</summary>
		/// <param name="context">A format context that provides information about the environment from which this converter is being invoked.</param>
		/// <param name="sourceType">The type being evaluated for conversion.</param>
		/// <returns>
		///     <see langword="false" /> in all cases.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="sourceType" /> is <see langword="null" />.</exception>
		// Token: 0x06001CFD RID: 7421 RVA: 0x000877AE File Offset: 0x000859AE
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType == null)
			{
				throw new ArgumentNullException("sourceType");
			}
			return base.CanConvertFrom(context, sourceType);
		}

		/// <summary>Determines whether an instance of <see cref="T:System.Windows.ComponentResourceKey" /> can be converted to the specified type, using the specified context. Always returns <see langword="false" />.</summary>
		/// <param name="context">A format context that provides information about the environment from which this converter is being invoked.</param>
		/// <param name="destinationType">The type being evaluated for conversion.</param>
		/// <returns>
		///     <see langword="false" /> in all cases.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="destinationType" /> is <see langword="null" />.</exception>
		// Token: 0x06001CFE RID: 7422 RVA: 0x000877CC File Offset: 0x000859CC
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			return base.CanConvertTo(context, destinationType);
		}

		/// <summary>Attempts to convert the specified object to a <see cref="T:System.Windows.ComponentResourceKey" />, using the specified context. Throws an exception in all cases.</summary>
		/// <param name="context">A format context that provides information about the environment from which this converter is being invoked.</param>
		/// <param name="culture">Culture specific information.</param>
		/// <param name="value">The object to convert.</param>
		/// <returns>Throws an exception in all cases.</returns>
		/// <exception cref="T:System.NotSupportedException">Cannot perform the conversion.</exception>
		// Token: 0x06001CFF RID: 7423 RVA: 0x000877EA File Offset: 0x000859EA
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			return base.ConvertFrom(context, culture, value);
		}

		/// <summary>Attempts to convert a <see cref="T:System.Windows.ComponentResourceKey" /> to the specified type, using the specified context. Throws an exception in all cases.</summary>
		/// <param name="context">A format context that provides information about the environment from which this converter is being invoked.</param>
		/// <param name="culture">Culture specific information.</param>
		/// <param name="value">The object to convert.</param>
		/// <param name="destinationType">The type to convert the object to.</param>
		/// <returns>Throws an exception in all cases.</returns>
		/// <exception cref="T:System.NotSupportedException">Cannot perform the conversion.</exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="value" /> is not type of <see cref="T:System.Windows.ComponentResourceKey" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="destinationType" /> is <see langword="null" />.</exception>
		// Token: 0x06001D00 RID: 7424 RVA: 0x000877F8 File Offset: 0x000859F8
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (!(value is ComponentResourceKey))
			{
				throw new ArgumentException(SR.Get("MustBeOfType", new object[]
				{
					"value",
					"ComponentResourceKey"
				}));
			}
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			return base.CanConvertTo(context, destinationType);
		}
	}
}
