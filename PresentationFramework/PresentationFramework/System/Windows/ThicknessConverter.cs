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
	/// <summary>Converts instances of other types to and from instances of <see cref="T:System.Windows.Thickness" />.</summary>
	// Token: 0x0200012E RID: 302
	public class ThicknessConverter : TypeConverter
	{
		/// <summary>Determines whether the type converter can create an instance of <see cref="T:System.Windows.Thickness" /> from a specified type.</summary>
		/// <param name="typeDescriptorContext">The context information of a type.</param>
		/// <param name="sourceType">The source type that the type converter is evaluating for conversion.</param>
		/// <returns>
		///     <see langword="true" /> if the type converter can create an instance of <see cref="T:System.Windows.Thickness" /> from the specified type; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000C51 RID: 3153 RVA: 0x0002DC8C File Offset: 0x0002BE8C
		public override bool CanConvertFrom(ITypeDescriptorContext typeDescriptorContext, Type sourceType)
		{
			TypeCode typeCode = Type.GetTypeCode(sourceType);
			return typeCode - TypeCode.Int16 <= 8 || typeCode == TypeCode.String;
		}

		/// <summary>Determines whether the type converter can convert an instance of <see cref="T:System.Windows.Thickness" /> to a different type. </summary>
		/// <param name="typeDescriptorContext">The context information of a type.</param>
		/// <param name="destinationType">The type for which the type converter is evaluating this instance of <see cref="T:System.Windows.Thickness" /> for conversion.</param>
		/// <returns>
		///     <see langword="true" /> if the type converter can convert this instance of <see cref="T:System.Windows.Thickness" /> to the <paramref name="destinationType" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000C52 RID: 3154 RVA: 0x00009F0E File Offset: 0x0000810E
		public override bool CanConvertTo(ITypeDescriptorContext typeDescriptorContext, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || destinationType == typeof(string);
		}

		/// <summary>Attempts to create an instance of <see cref="T:System.Windows.Thickness" /> from a specified object. </summary>
		/// <param name="typeDescriptorContext">The context information for a type.</param>
		/// <param name="cultureInfo">The <see cref="T:System.Globalization.CultureInfo" /> of the type being converted.</param>
		/// <param name="source">The <paramref name="source" /><see cref="T:System.Object" /> being converted.</param>
		/// <returns>An instance of <see cref="T:System.Windows.Thickness" /> created from the converted <paramref name="source" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="source" /> object is a null reference (<see langword="Nothing" /> in Visual Basic).</exception>
		/// <exception cref="T:System.ArgumentException">The example object is not a null reference and is not a valid type that can be converted to a <see cref="T:System.Windows.Thickness" />.</exception>
		// Token: 0x06000C53 RID: 3155 RVA: 0x0002DCB0 File Offset: 0x0002BEB0
		public override object ConvertFrom(ITypeDescriptorContext typeDescriptorContext, CultureInfo cultureInfo, object source)
		{
			if (source == null)
			{
				throw base.GetConvertFromException(source);
			}
			if (source is string)
			{
				return ThicknessConverter.FromString((string)source, cultureInfo);
			}
			if (source is double)
			{
				return new Thickness((double)source);
			}
			return new Thickness(Convert.ToDouble(source, cultureInfo));
		}

		/// <summary>Attempts to convert an instance of <see cref="T:System.Windows.Thickness" /> to a specified type. </summary>
		/// <param name="typeDescriptorContext">The context information of a type.</param>
		/// <param name="cultureInfo">The <see cref="T:System.Globalization.CultureInfo" /> of the type being converted.</param>
		/// <param name="value">The instance of <see cref="T:System.Windows.Thickness" /> to convert.</param>
		/// <param name="destinationType">The type that this instance of <see cref="T:System.Windows.Thickness" /> is converted to.</param>
		/// <returns>The type that is created when the type converter converts an instance of <see cref="T:System.Windows.Thickness" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="value" /> object is not a null reference (<see langword="Nothing" />) and is not a Brush, or the <paramref name="destinationType" /> is not one of the valid types for conversion.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="value" /> object is a null reference.</exception>
		// Token: 0x06000C54 RID: 3156 RVA: 0x0002DD0C File Offset: 0x0002BF0C
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
			if (!(value is Thickness))
			{
				throw new ArgumentException(SR.Get("UnexpectedParameterType", new object[]
				{
					value.GetType(),
					typeof(Thickness)
				}), "value");
			}
			Thickness th = (Thickness)value;
			if (destinationType == typeof(string))
			{
				return ThicknessConverter.ToString(th, cultureInfo);
			}
			if (destinationType == typeof(InstanceDescriptor))
			{
				ConstructorInfo constructor = typeof(Thickness).GetConstructor(new Type[]
				{
					typeof(double),
					typeof(double),
					typeof(double),
					typeof(double)
				});
				return new InstanceDescriptor(constructor, new object[]
				{
					th.Left,
					th.Top,
					th.Right,
					th.Bottom
				});
			}
			throw new ArgumentException(SR.Get("CannotConvertType", new object[]
			{
				typeof(Thickness),
				destinationType.FullName
			}));
		}

		// Token: 0x06000C55 RID: 3157 RVA: 0x0002DE6C File Offset: 0x0002C06C
		internal static string ToString(Thickness th, CultureInfo cultureInfo)
		{
			char numericListSeparator = TokenizerHelper.GetNumericListSeparator(cultureInfo);
			StringBuilder stringBuilder = new StringBuilder(64);
			stringBuilder.Append(LengthConverter.ToString(th.Left, cultureInfo));
			stringBuilder.Append(numericListSeparator);
			stringBuilder.Append(LengthConverter.ToString(th.Top, cultureInfo));
			stringBuilder.Append(numericListSeparator);
			stringBuilder.Append(LengthConverter.ToString(th.Right, cultureInfo));
			stringBuilder.Append(numericListSeparator);
			stringBuilder.Append(LengthConverter.ToString(th.Bottom, cultureInfo));
			return stringBuilder.ToString();
		}

		// Token: 0x06000C56 RID: 3158 RVA: 0x0002DEF8 File Offset: 0x0002C0F8
		internal static Thickness FromString(string s, CultureInfo cultureInfo)
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
				array[num] = LengthConverter.FromString(tokenizerHelper.GetCurrentToken(), cultureInfo);
				num++;
			}
			switch (num)
			{
			case 1:
				return new Thickness(array[0]);
			case 2:
				return new Thickness(array[0], array[1], array[0], array[1]);
			case 4:
				return new Thickness(array[0], array[1], array[2], array[3]);
			}
			throw new FormatException(SR.Get("InvalidStringThickness", new object[]
			{
				s
			}));
		}
	}
}
