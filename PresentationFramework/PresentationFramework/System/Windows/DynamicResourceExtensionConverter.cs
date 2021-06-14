using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Security;

namespace System.Windows
{
	/// <summary>Converts from parsed XAML to <see cref="T:System.Windows.DynamicResourceExtension" /> and supports dynamic resource references made from XAML. </summary>
	// Token: 0x020000B7 RID: 183
	public class DynamicResourceExtensionConverter : TypeConverter
	{
		/// <summary>Returns a value indicating whether this converter can convert an object to the given destination type using the context. </summary>
		/// <param name="context">Context in which the provided type should be evaluated.</param>
		/// <param name="destinationType">The type of the destination/output of conversion.</param>
		/// <returns>
		///     <see langword="true" /> if <paramref name="destinationType" /> is type of <see cref="T:System.ComponentModel.Design.Serialization.InstanceDescriptor" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x060003D9 RID: 985 RVA: 0x0000B0C1 File Offset: 0x000092C1
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
		}

		/// <summary>Converts the specified object to another type.</summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> object that provides a format context.</param>
		/// <param name="culture">A <see cref="T:System.Globalization.CultureInfo" /> object that specifies the culture to represent the number. </param>
		/// <param name="value">Value to be converted. This is expected to be type <see cref="T:System.Windows.DynamicResourceExtension" />.</param>
		/// <param name="destinationType">Type that should be converted to. </param>
		/// <returns>The returned converted object. Cast this to the requested type. Ordinarily this should be cast to <see cref="T:System.ComponentModel.Design.Serialization.InstanceDescriptor" />.</returns>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="value" /> could not be assigned as <see cref="T:System.Windows.DynamicResourceExtension" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="value" /> is <see langword="null" />.</exception>
		// Token: 0x060003DA RID: 986 RVA: 0x0000B0E0 File Offset: 0x000092E0
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
			DynamicResourceExtension dynamicResourceExtension = value as DynamicResourceExtension;
			if (dynamicResourceExtension == null)
			{
				throw new ArgumentException(SR.Get("MustBeOfType", new object[]
				{
					"value",
					"DynamicResourceExtension"
				}), "value");
			}
			return new InstanceDescriptor(typeof(DynamicResourceExtension).GetConstructor(new Type[]
			{
				typeof(object)
			}), new object[]
			{
				dynamicResourceExtension.ResourceKey
			});
		}
	}
}
