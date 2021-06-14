using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using System.Security;
using System.Text;
using MS.Internal;

namespace System.Windows.Controls
{
	/// <summary>Converts objects to and from a <see cref="T:System.Windows.Controls.VirtualizationCacheLength" />.</summary>
	// Token: 0x02000515 RID: 1301
	public class VirtualizationCacheLengthConverter : TypeConverter
	{
		/// <summary>Determines whether the <see cref="T:System.Windows.Controls.VirtualizationCacheLengthConverter" /> can convert an object of the specified type to a <see cref="T:System.Windows.Controls.VirtualizationCacheLength" />, using the specified context.</summary>
		/// <param name="typeDescriptorContext">An object that provides a format context.</param>
		/// <param name="sourceType">The type to convert from.</param>
		/// <returns>true if the <see cref="T:System.Windows.Controls.VirtualizationCacheLengthConverter" /> can convert the specified type to a <see cref="T:System.Windows.Controls.VirtualizationCacheLength" />; otherwise, false.</returns>
		// Token: 0x0600540F RID: 21519 RVA: 0x00174D88 File Offset: 0x00172F88
		public override bool CanConvertFrom(ITypeDescriptorContext typeDescriptorContext, Type sourceType)
		{
			TypeCode typeCode = Type.GetTypeCode(sourceType);
			return typeCode - TypeCode.Int16 <= 8 || typeCode == TypeCode.String;
		}

		/// <summary>Determines whether the <see cref="T:System.Windows.Controls.VirtualizationCacheLengthConverter" /> can convert a <see cref="T:System.Windows.Controls.VirtualizationCacheLength" /> to the specified type.</summary>
		/// <param name="typeDescriptorContext">An object that provides a format context.</param>
		/// <param name="destinationType">The type to convert to.</param>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Controls.VirtualizationCacheLengthConverter" /> can convert a <see cref="T:System.Windows.Controls.VirtualizationCacheLength" /> to the specified type; otherwise, <see langword="false" />.</returns>
		// Token: 0x06005410 RID: 21520 RVA: 0x00009F0E File Offset: 0x0000810E
		public override bool CanConvertTo(ITypeDescriptorContext typeDescriptorContext, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || destinationType == typeof(string);
		}

		/// <summary>Converts the specified object to a <see cref="T:System.Windows.Controls.VirtualizationCacheLength" />.</summary>
		/// <param name="typeDescriptorContext">An object that provides a format context.</param>
		/// <param name="cultureInfo">An object that provides the culture information that is used during conversion.</param>
		/// <param name="source">The object to convert to a <see cref="T:System.Windows.Controls.VirtualizationCacheLength" />.</param>
		/// <returns>The converted object.</returns>
		/// <exception cref="T:System.NotSupportedException">
		///         <paramref name="source" /> is <see langword="null" />.--or--
		///         <paramref name="source" /> cannot be converted to a <see cref="T:System.Windows.Controls.VirtualizationCacheLength" />.</exception>
		// Token: 0x06005411 RID: 21521 RVA: 0x00174DAC File Offset: 0x00172FAC
		public override object ConvertFrom(ITypeDescriptorContext typeDescriptorContext, CultureInfo cultureInfo, object source)
		{
			if (source == null)
			{
				throw base.GetConvertFromException(source);
			}
			if (source is string)
			{
				return VirtualizationCacheLengthConverter.FromString((string)source, cultureInfo);
			}
			double cacheBeforeAndAfterViewport = Convert.ToDouble(source, cultureInfo);
			return new VirtualizationCacheLength(cacheBeforeAndAfterViewport);
		}

		/// <summary>Converts the specified <see cref="T:System.Windows.Controls.VirtualizationCacheLength" /> to an object of the specified type.</summary>
		/// <param name="typeDescriptorContext">An object that provides a format context.</param>
		/// <param name="cultureInfo">An object that provides the culture information that is used during conversion.</param>
		/// <param name="value">A <see cref="T:System.Windows.Controls.VirtualizationCacheLength" /> to convert to another type.</param>
		/// <param name="destinationType">The type to convert to.</param>
		/// <returns>The converted object.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="destinationType" /> is <paramref name="null" />.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///         <paramref name="value" /> is <see langword="null" />.--or--
		///         <paramref name="value" /> is not a <see cref="T:System.Windows.Controls.VirtualizationCacheLength" />.</exception>
		// Token: 0x06005412 RID: 21522 RVA: 0x00174DF4 File Offset: 0x00172FF4
		[SecurityCritical]
		public override object ConvertTo(ITypeDescriptorContext typeDescriptorContext, CultureInfo cultureInfo, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (value != null && value is VirtualizationCacheLength)
			{
				VirtualizationCacheLength cacheLength = (VirtualizationCacheLength)value;
				if (destinationType == typeof(string))
				{
					return VirtualizationCacheLengthConverter.ToString(cacheLength, cultureInfo);
				}
				if (destinationType == typeof(InstanceDescriptor))
				{
					ConstructorInfo constructor = typeof(VirtualizationCacheLength).GetConstructor(new Type[]
					{
						typeof(double),
						typeof(VirtualizationCacheLengthUnit)
					});
					return new InstanceDescriptor(constructor, new object[]
					{
						cacheLength.CacheBeforeViewport,
						cacheLength.CacheAfterViewport
					});
				}
			}
			throw base.GetConvertToException(value, destinationType);
		}

		// Token: 0x06005413 RID: 21523 RVA: 0x00174EC0 File Offset: 0x001730C0
		internal static string ToString(VirtualizationCacheLength cacheLength, CultureInfo cultureInfo)
		{
			char numericListSeparator = TokenizerHelper.GetNumericListSeparator(cultureInfo);
			StringBuilder stringBuilder = new StringBuilder(26);
			stringBuilder.Append(cacheLength.CacheBeforeViewport.ToString(cultureInfo));
			stringBuilder.Append(numericListSeparator);
			stringBuilder.Append(cacheLength.CacheAfterViewport.ToString(cultureInfo));
			return stringBuilder.ToString();
		}

		// Token: 0x06005414 RID: 21524 RVA: 0x00174F18 File Offset: 0x00173118
		internal static VirtualizationCacheLength FromString(string s, CultureInfo cultureInfo)
		{
			TokenizerHelper tokenizerHelper = new TokenizerHelper(s, cultureInfo);
			double[] array = new double[2];
			int num = 0;
			while (tokenizerHelper.NextToken())
			{
				if (num >= 2)
				{
					num = 3;
					break;
				}
				array[num] = double.Parse(tokenizerHelper.GetCurrentToken(), cultureInfo);
				num++;
			}
			if (num == 1)
			{
				return new VirtualizationCacheLength(array[0]);
			}
			if (num != 2)
			{
				throw new FormatException(SR.Get("InvalidStringVirtualizationCacheLength", new object[]
				{
					s
				}));
			}
			return new VirtualizationCacheLength(array[0], array[1]);
		}
	}
}
