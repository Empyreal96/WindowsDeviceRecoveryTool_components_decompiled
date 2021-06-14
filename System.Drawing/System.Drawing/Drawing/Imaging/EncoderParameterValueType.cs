using System;

namespace System.Drawing.Imaging
{
	/// <summary>Used to specify the data type of the <see cref="T:System.Drawing.Imaging.EncoderParameter" /> used with the <see cref="Overload:System.Drawing.Image.Save" /> or <see cref="Overload:System.Drawing.Image.SaveAdd" /> method of an image. </summary>
	// Token: 0x0200009B RID: 155
	public enum EncoderParameterValueType
	{
		/// <summary>Specifies that each value in the array is an 8-bit unsigned integer.</summary>
		// Token: 0x04000884 RID: 2180
		ValueTypeByte = 1,
		/// <summary>Specifies that the array of values is a null-terminated ASCII character string. Note that the <see langword="NumberOfValues" /> data member of the <see cref="T:System.Drawing.Imaging.EncoderParameter" /> object indicates the length of the character string including the NULL terminator.</summary>
		// Token: 0x04000885 RID: 2181
		ValueTypeAscii,
		/// <summary>Specifies that each value in the array is a 16-bit, unsigned integer.</summary>
		// Token: 0x04000886 RID: 2182
		ValueTypeShort,
		/// <summary>Specifies that each value in the array is a 32-bit unsigned integer.</summary>
		// Token: 0x04000887 RID: 2183
		ValueTypeLong,
		/// <summary>Specifies that each value in the array is a pair of 32-bit unsigned integers. Each pair represents a fraction, the first integer being the numerator and the second integer being the denominator.</summary>
		// Token: 0x04000888 RID: 2184
		ValueTypeRational,
		/// <summary>Specifies that each value in the array is a pair of 32-bit unsigned integers. Each pair represents a range of numbers.</summary>
		// Token: 0x04000889 RID: 2185
		ValueTypeLongRange,
		/// <summary>Specifies that the array of values is an array of bytes that has no data type defined.</summary>
		// Token: 0x0400088A RID: 2186
		ValueTypeUndefined,
		/// <summary>Specifies that each value in the array is a set of four, 32-bit unsigned integers. The first two integers represent one fraction, and the second two integers represent a second fraction. The two fractions represent a range of rational numbers. The first fraction is the smallest rational number in the range, and the second fraction is the largest rational number in the range.</summary>
		// Token: 0x0400088B RID: 2187
		ValueTypeRationalRange
	}
}
