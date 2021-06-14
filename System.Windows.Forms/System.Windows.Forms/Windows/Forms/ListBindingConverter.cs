using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace System.Windows.Forms
{
	/// <summary>Provides a type converter to convert <see cref="T:System.Windows.Forms.Binding" /> objects to and from various other representations.</summary>
	// Token: 0x020002BA RID: 698
	public class ListBindingConverter : TypeConverter
	{
		// Token: 0x170009CD RID: 2509
		// (get) Token: 0x06002865 RID: 10341 RVA: 0x000BC84C File Offset: 0x000BAA4C
		private static Type[] ConstructorParamaterTypes
		{
			get
			{
				if (ListBindingConverter.ctorTypes == null)
				{
					ListBindingConverter.ctorTypes = new Type[]
					{
						typeof(string),
						typeof(object),
						typeof(string),
						typeof(bool),
						typeof(DataSourceUpdateMode),
						typeof(object),
						typeof(string),
						typeof(IFormatProvider)
					};
				}
				return ListBindingConverter.ctorTypes;
			}
		}

		// Token: 0x170009CE RID: 2510
		// (get) Token: 0x06002866 RID: 10342 RVA: 0x000BC8D8 File Offset: 0x000BAAD8
		private static string[] ConstructorParameterProperties
		{
			get
			{
				if (ListBindingConverter.ctorParamProps == null)
				{
					ListBindingConverter.ctorParamProps = new string[]
					{
						null,
						null,
						null,
						"FormattingEnabled",
						"DataSourceUpdateMode",
						"NullValue",
						"FormatString",
						"FormatInfo"
					};
				}
				return ListBindingConverter.ctorParamProps;
			}
		}

		/// <summary>Returns whether this converter can convert the object to the specified type, using the specified context.</summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
		/// <param name="destinationType">A <see cref="T:System.Type" /> that represents the type you want to convert to. </param>
		/// <returns>
		///     <see langword="true" /> if this converter can perform the conversion; otherwise, <see langword="false" />.</returns>
		// Token: 0x06002867 RID: 10343 RVA: 0x0001F8F0 File Offset: 0x0001DAF0
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
		}

		/// <summary>Converts the given value object to the specified type, using the specified context and culture information.</summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
		/// <param name="culture">A <see cref="T:System.Globalization.CultureInfo" />. If <see langword="null" /> is passed, the current culture is assumed. </param>
		/// <param name="value">The <see cref="T:System.Object" /> to convert. </param>
		/// <param name="destinationType">The <see cref="T:System.Type" /> to convert the value parameter to. </param>
		/// <returns>An <see cref="T:System.Object" /> that represents the converted value.</returns>
		// Token: 0x06002868 RID: 10344 RVA: 0x000BC924 File Offset: 0x000BAB24
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (destinationType == typeof(InstanceDescriptor) && value is Binding)
			{
				Binding b = (Binding)value;
				return this.GetInstanceDescriptorFromValues(b);
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		/// <summary>Creates an instance of the type that this <see cref="T:System.ComponentModel.TypeConverter" /> is associated with, using the specified context, given a set of property values for the object.</summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
		/// <param name="propertyValues">An <see cref="T:System.Collections.IDictionary" /> of new property values. </param>
		/// <returns>An <see cref="T:System.Object" /> representing the given <see cref="T:System.Collections.IDictionary" />, or <see langword="null" /> if the object cannot be created. This method always returns <see langword="null" />.</returns>
		// Token: 0x06002869 RID: 10345 RVA: 0x000BC97C File Offset: 0x000BAB7C
		public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
		{
			object result;
			try
			{
				result = new Binding((string)propertyValues["PropertyName"], propertyValues["DataSource"], (string)propertyValues["DataMember"]);
			}
			catch (InvalidCastException innerException)
			{
				throw new ArgumentException(SR.GetString("PropertyValueInvalidEntry"), innerException);
			}
			catch (NullReferenceException innerException2)
			{
				throw new ArgumentException(SR.GetString("PropertyValueInvalidEntry"), innerException2);
			}
			return result;
		}

		/// <summary>Returns whether changing a value on this object requires a call to <see cref="M:System.ComponentModel.TypeConverter.CreateInstance(System.Collections.IDictionary)" /> to create a new value, using the specified context.</summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context.</param>
		/// <returns>
		///     <see langword="true" /> if changing a property on this object requires a call to <see cref="M:System.ComponentModel.TypeConverter.CreateInstance(System.Collections.IDictionary)" /> to create a new value; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600286A RID: 10346 RVA: 0x0000E214 File Offset: 0x0000C414
		public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		// Token: 0x0600286B RID: 10347 RVA: 0x000BCA00 File Offset: 0x000BAC00
		private InstanceDescriptor GetInstanceDescriptorFromValues(Binding b)
		{
			b.FormattingEnabled = true;
			bool isComplete = true;
			int num = ListBindingConverter.ConstructorParameterProperties.Length - 1;
			while (num >= 0 && ListBindingConverter.ConstructorParameterProperties[num] != null)
			{
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(b)[ListBindingConverter.ConstructorParameterProperties[num]];
				if (propertyDescriptor != null && propertyDescriptor.ShouldSerializeValue(b))
				{
					break;
				}
				num--;
			}
			Type[] array = new Type[num + 1];
			Array.Copy(ListBindingConverter.ConstructorParamaterTypes, 0, array, 0, array.Length);
			ConstructorInfo constructor = typeof(Binding).GetConstructor(array);
			if (constructor == null)
			{
				isComplete = false;
				constructor = typeof(Binding).GetConstructor(new Type[]
				{
					typeof(string),
					typeof(object),
					typeof(string)
				});
			}
			object[] array2 = new object[array.Length];
			for (int i = 0; i < array2.Length; i++)
			{
				object obj;
				switch (i)
				{
				case 0:
					obj = b.PropertyName;
					break;
				case 1:
					obj = b.BindToObject.DataSource;
					break;
				case 2:
					obj = b.BindToObject.BindingMemberInfo.BindingMember;
					break;
				default:
					obj = TypeDescriptor.GetProperties(b)[ListBindingConverter.ConstructorParameterProperties[i]].GetValue(b);
					break;
				}
				array2[i] = obj;
			}
			return new InstanceDescriptor(constructor, array2, isComplete);
		}

		// Token: 0x0400119D RID: 4509
		private static Type[] ctorTypes;

		// Token: 0x0400119E RID: 4510
		private static string[] ctorParamProps;
	}
}
