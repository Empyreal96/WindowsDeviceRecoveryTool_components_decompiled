using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using System.Security;
using System.Windows.Markup;

namespace System.Windows
{
	/// <summary>Converts instances of other types to and from a <see cref="T:System.Windows.FigureLength" />.</summary>
	// Token: 0x020000BD RID: 189
	public class FigureLengthConverter : TypeConverter
	{
		/// <summary>Indicates whether an object can be converted from a given type to an instance of a <see cref="T:System.Windows.FigureLength" />.</summary>
		/// <param name="typeDescriptorContext">Describes the context information of a type.</param>
		/// <param name="sourceType">The source <see cref="T:System.Type" /> that is being queried for conversion support.</param>
		/// <returns>
		///     <see langword="true" /> if object of the specified type can be converted to a <see cref="T:System.Windows.FigureLength" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600040C RID: 1036 RVA: 0x0000B88C File Offset: 0x00009A8C
		public override bool CanConvertFrom(ITypeDescriptorContext typeDescriptorContext, Type sourceType)
		{
			TypeCode typeCode = Type.GetTypeCode(sourceType);
			return typeCode - TypeCode.Int16 <= 8 || typeCode == TypeCode.String;
		}

		/// <summary>Determines whether instances of <see cref="T:System.Windows.FigureLength" /> can be converted to the specified type.</summary>
		/// <param name="typeDescriptorContext">Describes the context information of a type.</param>
		/// <param name="destinationType">The desired type this <see cref="T:System.Windows.FigureLength" /> is being evaluated to be converted to.</param>
		/// <returns>
		///     <see langword="true" /> if instances of <see cref="T:System.Windows.FigureLength" /> can be converted to <paramref name="destinationType" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600040D RID: 1037 RVA: 0x0000B8AE File Offset: 0x00009AAE
		public override bool CanConvertTo(ITypeDescriptorContext typeDescriptorContext, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || destinationType == typeof(string);
		}

		/// <summary>Converts the specified object to a <see cref="T:System.Windows.FigureLength" />.</summary>
		/// <param name="typeDescriptorContext">Describes the context information of a type.</param>
		/// <param name="cultureInfo">Describes the <see cref="T:System.Globalization.CultureInfo" /> of the type being converted.</param>
		/// <param name="source">The object being converted.</param>
		/// <returns>The <see cref="T:System.Windows.FigureLength" /> created from converting <paramref name="source" />.</returns>
		// Token: 0x0600040E RID: 1038 RVA: 0x0000B8D4 File Offset: 0x00009AD4
		public override object ConvertFrom(ITypeDescriptorContext typeDescriptorContext, CultureInfo cultureInfo, object source)
		{
			if (source == null)
			{
				throw base.GetConvertFromException(source);
			}
			if (source is string)
			{
				return FigureLengthConverter.FromString((string)source, cultureInfo);
			}
			return new FigureLength(Convert.ToDouble(source, cultureInfo));
		}

		/// <summary>Converts the specified <see cref="T:System.Windows.FigureLength" /> to the specified type.</summary>
		/// <param name="typeDescriptorContext">Describes the context information of a type.</param>
		/// <param name="cultureInfo">Describes the <see cref="T:System.Globalization.CultureInfo" /> of the type being converted.</param>
		/// <param name="value">The <see cref="T:System.Windows.FigureLength" /> to convert.</param>
		/// <param name="destinationType">The type to convert the <see cref="T:System.Windows.FigureLength" /> to.</param>
		/// <returns>The object created from converting this <see cref="T:System.Windows.FigureLength" />.</returns>
		// Token: 0x0600040F RID: 1039 RVA: 0x0000B90C File Offset: 0x00009B0C
		[SecurityCritical]
		public override object ConvertTo(ITypeDescriptorContext typeDescriptorContext, CultureInfo cultureInfo, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (value != null && value is FigureLength)
			{
				FigureLength fl = (FigureLength)value;
				if (destinationType == typeof(string))
				{
					return FigureLengthConverter.ToString(fl, cultureInfo);
				}
				if (destinationType == typeof(InstanceDescriptor))
				{
					ConstructorInfo constructor = typeof(FigureLength).GetConstructor(new Type[]
					{
						typeof(double),
						typeof(FigureUnitType)
					});
					return new InstanceDescriptor(constructor, new object[]
					{
						fl.Value,
						fl.FigureUnitType
					});
				}
			}
			throw base.GetConvertToException(value, destinationType);
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x0000B9D8 File Offset: 0x00009BD8
		internal static string ToString(FigureLength fl, CultureInfo cultureInfo)
		{
			FigureUnitType figureUnitType = fl.FigureUnitType;
			if (figureUnitType == FigureUnitType.Auto)
			{
				return "Auto";
			}
			if (figureUnitType != FigureUnitType.Pixel)
			{
				return Convert.ToString(fl.Value, cultureInfo) + " " + fl.FigureUnitType.ToString();
			}
			return Convert.ToString(fl.Value, cultureInfo);
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x0000BA38 File Offset: 0x00009C38
		internal static FigureLength FromString(string s, CultureInfo cultureInfo)
		{
			double value;
			FigureUnitType type;
			XamlFigureLengthSerializer.FromString(s, cultureInfo, out value, out type);
			return new FigureLength(value, type);
		}
	}
}
