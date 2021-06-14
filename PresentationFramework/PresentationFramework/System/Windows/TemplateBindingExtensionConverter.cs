using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Security;

namespace System.Windows
{
	/// <summary>A type converter that is used to construct a <see cref="T:System.Windows.TemplateBindingExtension" /> from an instance during serialization.</summary>
	// Token: 0x0200011E RID: 286
	public class TemplateBindingExtensionConverter : TypeConverter
	{
		/// <summary>Returns whether this converter can convert the object to the specified type, using the specified context. </summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> implementation that provides a format context. </param>
		/// <param name="destinationType">The desired type of the conversion's output.</param>
		/// <returns>
		///     <see langword="true" /> if this converter can perform the requested conversion; otherwise, <see langword="false" />. Only a <paramref name="destinationType" /> of <see cref="T:System.ComponentModel.Design.Serialization.InstanceDescriptor" /> will return <see langword="true" />.</returns>
		// Token: 0x06000BD4 RID: 3028 RVA: 0x0000B0C1 File Offset: 0x000092C1
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
		}

		/// <summary>Converts the given value object to the specified type. </summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> implementation that provides a format context. </param>
		/// <param name="culture">A <see cref="T:System.Globalization.CultureInfo" /> object. If a null reference is passed, the current culture is assumed. </param>
		/// <param name="value">The value to convert.</param>
		/// <param name="destinationType">The desired type to convert to.</param>
		/// <returns>The converted value. </returns>
		// Token: 0x06000BD5 RID: 3029 RVA: 0x0002B358 File Offset: 0x00029558
		[SecurityCritical]
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (!(destinationType == typeof(InstanceDescriptor)))
			{
				return base.ConvertTo(context, culture, value, destinationType);
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			TemplateBindingExtension templateBindingExtension = value as TemplateBindingExtension;
			if (templateBindingExtension == null)
			{
				throw new ArgumentException(SR.Get("MustBeOfType", new object[]
				{
					"value",
					"TemplateBindingExtension"
				}), "value");
			}
			return new InstanceDescriptor(typeof(TemplateBindingExtension).GetConstructor(new Type[]
			{
				typeof(DependencyProperty)
			}), new object[]
			{
				templateBindingExtension.Property
			});
		}
	}
}
