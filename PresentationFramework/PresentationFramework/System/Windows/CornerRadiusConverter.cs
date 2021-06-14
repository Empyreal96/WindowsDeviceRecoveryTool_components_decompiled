using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using System.Security;
using System.Text;
using MS.Internal;

namespace System.Windows
{
	/// <summary>Converts instances of other types to and from a <see cref="T:System.Windows.CornerRadius" />. </summary>
	// Token: 0x020000A8 RID: 168
	public class CornerRadiusConverter : TypeConverter
	{
		/// <summary>Indicates whether an object can be converted from a given type to a <see cref="T:System.Windows.CornerRadius" />. </summary>
		/// <param name="typeDescriptorContext">Describes the context information of a type.</param>
		/// <param name="sourceType">The source <see cref="T:System.Type" /> that is being queried for conversion support.</param>
		/// <returns>
		///     <see langword="true" /> if <paramref name="sourceType" /> is of type <see cref="T:System.String" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600037C RID: 892 RVA: 0x00009EEC File Offset: 0x000080EC
		public override bool CanConvertFrom(ITypeDescriptorContext typeDescriptorContext, Type sourceType)
		{
			TypeCode typeCode = Type.GetTypeCode(sourceType);
			return typeCode - TypeCode.Int16 <= 8 || typeCode == TypeCode.String;
		}

		/// <summary>Determines whether <see cref="T:System.Windows.CornerRadius" /> values can be converted to the specified type. </summary>
		/// <param name="typeDescriptorContext">Describes the context information of a type.</param>
		/// <param name="destinationType">The desired type this <see cref="T:System.Windows.CornerRadius" /> is being evaluated to be converted to.</param>
		/// <returns>
		///     <see langword="true" /> if <paramref name="destinationType" /> is of type <see cref="T:System.String" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600037D RID: 893 RVA: 0x00009F0E File Offset: 0x0000810E
		public override bool CanConvertTo(ITypeDescriptorContext typeDescriptorContext, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || destinationType == typeof(string);
		}

		/// <summary>Converts the specified object to a <see cref="T:System.Windows.CornerRadius" />.</summary>
		/// <param name="typeDescriptorContext">Describes the context information of a type.</param>
		/// <param name="cultureInfo">Describes the <see cref="T:System.Globalization.CultureInfo" /> of the type being converted.</param>
		/// <param name="source">The object being converted.</param>
		/// <returns>The <see cref="T:System.Windows.CornerRadius" /> created from converting <paramref name="source" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="source" /> is not <see langword="null" /> and is not a valid type which can be converted to a <see cref="T:System.Windows.CornerRadius" />.</exception>
		// Token: 0x0600037E RID: 894 RVA: 0x00009F37 File Offset: 0x00008137
		public override object ConvertFrom(ITypeDescriptorContext typeDescriptorContext, CultureInfo cultureInfo, object source)
		{
			if (source == null)
			{
				throw base.GetConvertFromException(source);
			}
			if (source is string)
			{
				return CornerRadiusConverter.FromString((string)source, cultureInfo);
			}
			return new CornerRadius(Convert.ToDouble(source, cultureInfo));
		}

		/// <summary>Converts the specified <see cref="T:System.Windows.CornerRadius" /> to the specified type.</summary>
		/// <param name="typeDescriptorContext">Describes the context information of a type.</param>
		/// <param name="cultureInfo">Describes the <see cref="T:System.Globalization.CultureInfo" /> of the type being converted.</param>
		/// <param name="value">The <see cref="T:System.Windows.CornerRadius" /> to convert.</param>
		/// <param name="destinationType">The type to convert the <see cref="T:System.Windows.CornerRadius" /> to.</param>
		/// <returns>The object created from converting this <see cref="T:System.Windows.CornerRadius" /> (a string).</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="value" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="value" /> is not <see langword="null" /> and is not a <see cref="T:System.Windows.Media.Brush" />, or if <paramref name="destinationType" /> is not one of the valid destination types.</exception>
		// Token: 0x0600037F RID: 895 RVA: 0x00009F70 File Offset: 0x00008170
		[SecurityCritical]
		public override object ConvertTo(ITypeDescriptorContext typeDescriptorContext, CultureInfo cultureInfo, object value, Type destinationType)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (null == destinationType)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (!(value is CornerRadius))
			{
				throw new ArgumentException(SR.Get("UnexpectedParameterType", new object[]
				{
					value.GetType(),
					typeof(CornerRadius)
				}), "value");
			}
			CornerRadius cr = (CornerRadius)value;
			if (destinationType == typeof(string))
			{
				return CornerRadiusConverter.ToString(cr, cultureInfo);
			}
			if (destinationType == typeof(InstanceDescriptor))
			{
				ConstructorInfo constructor = typeof(CornerRadius).GetConstructor(new Type[]
				{
					typeof(double),
					typeof(double),
					typeof(double),
					typeof(double)
				});
				return new InstanceDescriptor(constructor, new object[]
				{
					cr.TopLeft,
					cr.TopRight,
					cr.BottomRight,
					cr.BottomLeft
				});
			}
			throw new ArgumentException(SR.Get("CannotConvertType", new object[]
			{
				typeof(CornerRadius),
				destinationType.FullName
			}));
		}

		// Token: 0x06000380 RID: 896 RVA: 0x0000A0D0 File Offset: 0x000082D0
		internal static string ToString(CornerRadius cr, CultureInfo cultureInfo)
		{
			char numericListSeparator = TokenizerHelper.GetNumericListSeparator(cultureInfo);
			StringBuilder stringBuilder = new StringBuilder(64);
			stringBuilder.Append(cr.TopLeft.ToString(cultureInfo));
			stringBuilder.Append(numericListSeparator);
			stringBuilder.Append(cr.TopRight.ToString(cultureInfo));
			stringBuilder.Append(numericListSeparator);
			stringBuilder.Append(cr.BottomRight.ToString(cultureInfo));
			stringBuilder.Append(numericListSeparator);
			stringBuilder.Append(cr.BottomLeft.ToString(cultureInfo));
			return stringBuilder.ToString();
		}

		// Token: 0x06000381 RID: 897 RVA: 0x0000A168 File Offset: 0x00008368
		internal static CornerRadius FromString(string s, CultureInfo cultureInfo)
		{
			TokenizerHelper tokenizerHelper = new TokenizerHelper(s, cultureInfo);
			double[] array = new double[4];
			int num = 0;
			while (tokenizerHelper.NextToken())
			{
				if (num >= 4)
				{
					num = 5;
					break;
				}
				array[num] = double.Parse(tokenizerHelper.GetCurrentToken(), cultureInfo);
				num++;
			}
			if (num == 1)
			{
				return new CornerRadius(array[0]);
			}
			if (num != 4)
			{
				throw new FormatException(SR.Get("InvalidStringCornerRadius", new object[]
				{
					s
				}));
			}
			return new CornerRadius(array[0], array[1], array[2], array[3]);
		}
	}
}
