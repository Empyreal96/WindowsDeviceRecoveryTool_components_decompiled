using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace System.Windows.Forms
{
	/// <summary>Provides a type converter to convert <see cref="T:System.Windows.Forms.ColumnHeader" /> objects from one type to another.</summary>
	// Token: 0x02000147 RID: 327
	public class ColumnHeaderConverter : ExpandableObjectConverter
	{
		/// <summary>Returns a value indicating whether the <see cref="T:System.Windows.Forms.ColumnHeaderConverter" /> can convert a <see cref="T:System.Windows.Forms.ColumnHeader" /> to the specified type, using the specified context.</summary>
		/// <param name="context">A <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context.</param>
		/// <param name="destinationType">A type representing the type to convert to.</param>
		/// <returns>
		///     <see langword="true" /> if this converter can perform the conversion; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000A75 RID: 2677 RVA: 0x0001F8F0 File Offset: 0x0001DAF0
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
		}

		/// <summary>Converts the specified object to the specified type, using the specified context and culture information.</summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context.</param>
		/// <param name="culture">A <see cref="T:System.Globalization.CultureInfo" /> that represents information about a culture, such as language and calendar system. Can be <see langword="null" />.</param>
		/// <param name="value">The <see cref="T:System.Object" /> to convert.</param>
		/// <param name="destinationType">The <see cref="T:System.Type" /> to convert to.</param>
		/// <returns>The <see cref="T:System.Object" /> that is the result of the conversion.</returns>
		/// <exception cref="T:System.NotSupportedException">The conversion cannot be performed<paramref name="." /></exception>
		// Token: 0x06000A76 RID: 2678 RVA: 0x0001F910 File Offset: 0x0001DB10
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (!(destinationType == typeof(InstanceDescriptor)) || !(value is ColumnHeader))
			{
				return base.ConvertTo(context, culture, value, destinationType);
			}
			ColumnHeader columnHeader = (ColumnHeader)value;
			Type reflectionType = TypeDescriptor.GetReflectionType(value);
			InstanceDescriptor instanceDescriptor = null;
			ConstructorInfo constructor;
			if (columnHeader.ImageIndex != -1)
			{
				constructor = reflectionType.GetConstructor(new Type[]
				{
					typeof(int)
				});
				if (constructor != null)
				{
					instanceDescriptor = new InstanceDescriptor(constructor, new object[]
					{
						columnHeader.ImageIndex
					}, false);
				}
			}
			if (instanceDescriptor == null && !string.IsNullOrEmpty(columnHeader.ImageKey))
			{
				constructor = reflectionType.GetConstructor(new Type[]
				{
					typeof(string)
				});
				if (constructor != null)
				{
					instanceDescriptor = new InstanceDescriptor(constructor, new object[]
					{
						columnHeader.ImageKey
					}, false);
				}
			}
			if (instanceDescriptor != null)
			{
				return instanceDescriptor;
			}
			constructor = reflectionType.GetConstructor(new Type[0]);
			if (constructor != null)
			{
				return new InstanceDescriptor(constructor, new object[0], false);
			}
			throw new ArgumentException(SR.GetString("NoDefaultConstructor", new object[]
			{
				reflectionType.FullName
			}));
		}
	}
}
