using System;
using System.ComponentModel;
using System.Globalization;

namespace System.Windows.Markup
{
	/// <summary>Implements a type converter for <see cref="T:System.Windows.TemplateKey" /> objects, which deliberately have no type conversion pathways. The type converter enforces and reports that behavior.</summary>
	// Token: 0x02000230 RID: 560
	public sealed class TemplateKeyConverter : TypeConverter
	{
		/// <summary>Determines whether an object of the specified type can be converted to an instance of <see cref="T:System.Windows.TemplateKey" />.</summary>
		/// <param name="context">A format context that provides information about the environment from which this converter is being invoked.</param>
		/// <param name="sourceType">The type being evaluated for conversion.</param>
		/// <returns>Always returns <see langword="false" />.</returns>
		// Token: 0x0600225A RID: 8794 RVA: 0x0000B02A File Offset: 0x0000922A
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return false;
		}

		/// <summary>Determines whether an instance of <see cref="T:System.Windows.TemplateKey" /> can be converted to the specified type.</summary>
		/// <param name="context">A format context that provides information about the environment from which this converter is being invoked.</param>
		/// <param name="destinationType">The type being evaluated for conversion.</param>
		/// <returns>Always returns <see langword="false" />.</returns>
		// Token: 0x0600225B RID: 8795 RVA: 0x0000B02A File Offset: 0x0000922A
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return false;
		}

		/// <summary>Attempts to convert the specified object (string) to a <see cref="T:System.Windows.TemplateKey" />.</summary>
		/// <param name="context">A format context that provides information about the environment from which this converter is being invoked.</param>
		/// <param name="culture">Culture specific information.</param>
		/// <param name="source">The object to convert.</param>
		/// <returns>Always throws an exception.</returns>
		/// <exception cref="T:System.NotSupportedException">
		///         <paramref name="source" /> cannot be converted.</exception>
		// Token: 0x0600225C RID: 8796 RVA: 0x000AADEE File Offset: 0x000A8FEE
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object source)
		{
			throw base.GetConvertFromException(source);
		}

		/// <summary>Attempts to convert a <see cref="T:System.Windows.TemplateKey" /> to the specified type, using the specified context.</summary>
		/// <param name="context">A format context that provides information about the environment from which this converter is being invoked.</param>
		/// <param name="culture">Culture specific information.</param>
		/// <param name="value">The object to convert.</param>
		/// <param name="destinationType">The type to convert the object to.</param>
		/// <returns>Always throws an exception.</returns>
		/// <exception cref="T:System.NotSupportedException">
		///         <paramref name="value" /> cannot be converted.</exception>
		// Token: 0x0600225D RID: 8797 RVA: 0x0008795A File Offset: 0x00085B5A
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			throw base.GetConvertToException(value, destinationType);
		}
	}
}
