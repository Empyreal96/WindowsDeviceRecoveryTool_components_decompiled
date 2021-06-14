using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using System.Security;
using System.Windows.Markup;
using MS.Internal;

namespace System.Windows
{
	/// <summary>Converts instances of other types to and from <see cref="T:System.Windows.GridLength" /> instances. </summary>
	// Token: 0x020000CE RID: 206
	public class GridLengthConverter : TypeConverter
	{
		/// <summary>Determines whether a class can be converted from a given type to an instance of <see cref="T:System.Windows.GridLength" />.</summary>
		/// <param name="typeDescriptorContext">Describes the context information of a type.</param>
		/// <param name="sourceType">The type of the source that is being evaluated for conversion.</param>
		/// <returns>
		///     <see langword="true" /> if the converter can convert from the specified type to an instance of <see cref="T:System.Windows.GridLength" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000726 RID: 1830 RVA: 0x000169EC File Offset: 0x00014BEC
		public override bool CanConvertFrom(ITypeDescriptorContext typeDescriptorContext, Type sourceType)
		{
			TypeCode typeCode = Type.GetTypeCode(sourceType);
			return typeCode - TypeCode.Int16 <= 8 || typeCode == TypeCode.String;
		}

		/// <summary>Determines whether an instance of <see cref="T:System.Windows.GridLength" /> can be converted to a different type. </summary>
		/// <param name="typeDescriptorContext">Describes the context information of a type.</param>
		/// <param name="destinationType">The desired type that this instance of <see cref="T:System.Windows.GridLength" /> is being evaluated for conversion.</param>
		/// <returns>
		///     <see langword="true" /> if the converter can convert this instance of <see cref="T:System.Windows.GridLength" /> to the specified type; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000727 RID: 1831 RVA: 0x0000B8AE File Offset: 0x00009AAE
		public override bool CanConvertTo(ITypeDescriptorContext typeDescriptorContext, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || destinationType == typeof(string);
		}

		/// <summary>Attempts to convert a specified object to an instance of <see cref="T:System.Windows.GridLength" />. </summary>
		/// <param name="typeDescriptorContext">Describes the context information of a type.</param>
		/// <param name="cultureInfo">Cultural specific information that should be respected during conversion.</param>
		/// <param name="source">The object being converted.</param>
		/// <returns>The instance of <see cref="T:System.Windows.GridLength" /> that is created from the converted <paramref name="source" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="source" /> object is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="source" /> object is not <see langword="null" /> and is not a valid type that can be converted to a <see cref="T:System.Windows.GridLength" />.</exception>
		// Token: 0x06000728 RID: 1832 RVA: 0x00016A10 File Offset: 0x00014C10
		public override object ConvertFrom(ITypeDescriptorContext typeDescriptorContext, CultureInfo cultureInfo, object source)
		{
			if (source == null)
			{
				throw base.GetConvertFromException(source);
			}
			if (source is string)
			{
				return GridLengthConverter.FromString((string)source, cultureInfo);
			}
			double value = Convert.ToDouble(source, cultureInfo);
			GridUnitType type;
			if (DoubleUtil.IsNaN(value))
			{
				value = 1.0;
				type = GridUnitType.Auto;
			}
			else
			{
				type = GridUnitType.Pixel;
			}
			return new GridLength(value, type);
		}

		/// <summary>Attempts to convert an instance of <see cref="T:System.Windows.GridLength" /> to a specified type. </summary>
		/// <param name="typeDescriptorContext">Describes the context information of a type.</param>
		/// <param name="cultureInfo">Cultural specific information that should be respected during conversion.</param>
		/// <param name="value">The instance of <see cref="T:System.Windows.GridLength" /> to convert.</param>
		/// <param name="destinationType">The type that this instance of <see cref="T:System.Windows.GridLength" /> is converted to.</param>
		/// <returns>The object that is created from the converted instance of <see cref="T:System.Windows.GridLength" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="destinationType" /> is not one of the valid types for conversion.</exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="value" /> is <see langword="null" />.</exception>
		// Token: 0x06000729 RID: 1833 RVA: 0x00016A70 File Offset: 0x00014C70
		[SecurityCritical]
		public override object ConvertTo(ITypeDescriptorContext typeDescriptorContext, CultureInfo cultureInfo, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (value != null && value is GridLength)
			{
				GridLength gl = (GridLength)value;
				if (destinationType == typeof(string))
				{
					return GridLengthConverter.ToString(gl, cultureInfo);
				}
				if (destinationType == typeof(InstanceDescriptor))
				{
					ConstructorInfo constructor = typeof(GridLength).GetConstructor(new Type[]
					{
						typeof(double),
						typeof(GridUnitType)
					});
					return new InstanceDescriptor(constructor, new object[]
					{
						gl.Value,
						gl.GridUnitType
					});
				}
			}
			throw base.GetConvertToException(value, destinationType);
		}

		// Token: 0x0600072A RID: 1834 RVA: 0x00016B3C File Offset: 0x00014D3C
		internal static string ToString(GridLength gl, CultureInfo cultureInfo)
		{
			GridUnitType gridUnitType = gl.GridUnitType;
			if (gridUnitType == GridUnitType.Auto)
			{
				return "Auto";
			}
			if (gridUnitType != GridUnitType.Star)
			{
				return Convert.ToString(gl.Value, cultureInfo);
			}
			if (!DoubleUtil.IsOne(gl.Value))
			{
				return Convert.ToString(gl.Value, cultureInfo) + "*";
			}
			return "*";
		}

		// Token: 0x0600072B RID: 1835 RVA: 0x00016B9C File Offset: 0x00014D9C
		internal static GridLength FromString(string s, CultureInfo cultureInfo)
		{
			double value;
			GridUnitType type;
			XamlGridLengthSerializer.FromString(s, cultureInfo, out value, out type);
			return new GridLength(value, type);
		}
	}
}
