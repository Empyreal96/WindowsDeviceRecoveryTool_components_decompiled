using System;
using System.Runtime.InteropServices;

namespace System.Drawing.Imaging
{
	/// <summary>Used to pass a value, or an array of values, to an image encoder. </summary>
	// Token: 0x02000099 RID: 153
	[StructLayout(LayoutKind.Sequential)]
	public sealed class EncoderParameter : IDisposable
	{
		/// <summary>Allows an <see cref="T:System.Drawing.Imaging.EncoderParameter" /> object to attempt to free resources and perform other cleanup operations before the <see cref="T:System.Drawing.Imaging.EncoderParameter" /> object is reclaimed by garbage collection.</summary>
		// Token: 0x0600092A RID: 2346 RVA: 0x00022E28 File Offset: 0x00021028
		~EncoderParameter()
		{
			this.Dispose(false);
		}

		/// <summary>Gets or sets the <see cref="T:System.Drawing.Imaging.Encoder" /> object associated with this <see cref="T:System.Drawing.Imaging.EncoderParameter" /> object. The <see cref="T:System.Drawing.Imaging.Encoder" /> object encapsulates the globally unique identifier (GUID) that specifies the category (for example <see cref="F:System.Drawing.Imaging.Encoder.Quality" />, <see cref="F:System.Drawing.Imaging.Encoder.ColorDepth" />, or <see cref="F:System.Drawing.Imaging.Encoder.Compression" />) of the parameter stored in this <see cref="T:System.Drawing.Imaging.EncoderParameter" /> object.</summary>
		/// <returns>An <see cref="T:System.Drawing.Imaging.Encoder" /> object that encapsulates the GUID that specifies the category of the parameter stored in this <see cref="T:System.Drawing.Imaging.EncoderParameter" /> object.</returns>
		// Token: 0x1700035E RID: 862
		// (get) Token: 0x0600092B RID: 2347 RVA: 0x00022E58 File Offset: 0x00021058
		// (set) Token: 0x0600092C RID: 2348 RVA: 0x00022E65 File Offset: 0x00021065
		public Encoder Encoder
		{
			get
			{
				return new Encoder(this.parameterGuid);
			}
			set
			{
				this.parameterGuid = value.Guid;
			}
		}

		/// <summary>Gets the data type of the values stored in this <see cref="T:System.Drawing.Imaging.EncoderParameter" /> object.</summary>
		/// <returns>A member of the <see cref="T:System.Drawing.Imaging.EncoderParameterValueType" /> enumeration that indicates the data type of the values stored in this <see cref="T:System.Drawing.Imaging.EncoderParameter" /> object.</returns>
		// Token: 0x1700035F RID: 863
		// (get) Token: 0x0600092D RID: 2349 RVA: 0x00022E73 File Offset: 0x00021073
		public EncoderParameterValueType Type
		{
			get
			{
				return this.parameterValueType;
			}
		}

		/// <summary>Gets the data type of the values stored in this <see cref="T:System.Drawing.Imaging.EncoderParameter" /> object.</summary>
		/// <returns>A member of the <see cref="T:System.Drawing.Imaging.EncoderParameterValueType" /> enumeration that indicates the data type of the values stored in this <see cref="T:System.Drawing.Imaging.EncoderParameter" /> object.</returns>
		// Token: 0x17000360 RID: 864
		// (get) Token: 0x0600092E RID: 2350 RVA: 0x00022E73 File Offset: 0x00021073
		public EncoderParameterValueType ValueType
		{
			get
			{
				return this.parameterValueType;
			}
		}

		/// <summary>Gets the number of elements in the array of values stored in this <see cref="T:System.Drawing.Imaging.EncoderParameter" /> object.</summary>
		/// <returns>An integer that indicates the number of elements in the array of values stored in this <see cref="T:System.Drawing.Imaging.EncoderParameter" /> object.</returns>
		// Token: 0x17000361 RID: 865
		// (get) Token: 0x0600092F RID: 2351 RVA: 0x00022E7B File Offset: 0x0002107B
		public int NumberOfValues
		{
			get
			{
				return this.numberOfValues;
			}
		}

		/// <summary>Releases all resources used by this <see cref="T:System.Drawing.Imaging.EncoderParameter" /> object.</summary>
		// Token: 0x06000930 RID: 2352 RVA: 0x00022E83 File Offset: 0x00021083
		public void Dispose()
		{
			this.Dispose(true);
			GC.KeepAlive(this);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000931 RID: 2353 RVA: 0x00022E98 File Offset: 0x00021098
		private void Dispose(bool disposing)
		{
			if (this.parameterValue != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(this.parameterValue);
			}
			this.parameterValue = IntPtr.Zero;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.EncoderParameter" /> class with the specified <see cref="T:System.Drawing.Imaging.Encoder" /> object and one unsigned 8-bit integer. Sets the <see cref="P:System.Drawing.Imaging.EncoderParameter.ValueType" /> property to <see cref="F:System.Drawing.Imaging.EncoderParameterValueType.ValueTypeByte" />, and sets the <see cref="P:System.Drawing.Imaging.EncoderParameter.NumberOfValues" /> property to 1.</summary>
		/// <param name="encoder">An <see cref="T:System.Drawing.Imaging.Encoder" /> object that encapsulates the globally unique identifier of the parameter category. </param>
		/// <param name="value">An 8-bit unsigned integer that specifies the value stored in the <see cref="T:System.Drawing.Imaging.EncoderParameter" /> object. </param>
		// Token: 0x06000932 RID: 2354 RVA: 0x00022EC4 File Offset: 0x000210C4
		public EncoderParameter(Encoder encoder, byte value)
		{
			this.parameterGuid = encoder.Guid;
			this.parameterValueType = EncoderParameterValueType.ValueTypeByte;
			this.numberOfValues = 1;
			this.parameterValue = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(byte)));
			if (this.parameterValue == IntPtr.Zero)
			{
				throw SafeNativeMethods.Gdip.StatusException(3);
			}
			Marshal.WriteByte(this.parameterValue, value);
			GC.KeepAlive(this);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.EncoderParameter" /> class with the specified <see cref="T:System.Drawing.Imaging.Encoder" /> object and one 8-bit value. Sets the <see cref="P:System.Drawing.Imaging.EncoderParameter.ValueType" /> property to <see cref="F:System.Drawing.Imaging.EncoderParameterValueType.ValueTypeUndefined" /> or <see cref="F:System.Drawing.Imaging.EncoderParameterValueType.ValueTypeByte" />, and sets the <see cref="P:System.Drawing.Imaging.EncoderParameter.NumberOfValues" /> property to 1.</summary>
		/// <param name="encoder">An <see cref="T:System.Drawing.Imaging.Encoder" /> object that encapsulates the globally unique identifier of the parameter category. </param>
		/// <param name="value">A byte that specifies the value stored in the <see cref="T:System.Drawing.Imaging.EncoderParameter" /> object. </param>
		/// <param name="undefined">If <see langword="true" />, the <see cref="P:System.Drawing.Imaging.EncoderParameter.ValueType" /> property is set to <see cref="F:System.Drawing.Imaging.EncoderParameterValueType.ValueTypeUndefined" />; otherwise, the <see cref="P:System.Drawing.Imaging.EncoderParameter.ValueType" /> property is set to <see cref="F:System.Drawing.Imaging.EncoderParameterValueType.ValueTypeByte" />. </param>
		// Token: 0x06000933 RID: 2355 RVA: 0x00022F38 File Offset: 0x00021138
		public EncoderParameter(Encoder encoder, byte value, bool undefined)
		{
			this.parameterGuid = encoder.Guid;
			if (undefined)
			{
				this.parameterValueType = EncoderParameterValueType.ValueTypeUndefined;
			}
			else
			{
				this.parameterValueType = EncoderParameterValueType.ValueTypeByte;
			}
			this.numberOfValues = 1;
			this.parameterValue = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(byte)));
			if (this.parameterValue == IntPtr.Zero)
			{
				throw SafeNativeMethods.Gdip.StatusException(3);
			}
			Marshal.WriteByte(this.parameterValue, value);
			GC.KeepAlive(this);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.EncoderParameter" /> class with the specified <see cref="T:System.Drawing.Imaging.Encoder" /> object and one, 16-bit integer. Sets the <see cref="P:System.Drawing.Imaging.EncoderParameter.ValueType" /> property to <see cref="F:System.Drawing.Imaging.EncoderParameterValueType.ValueTypeShort" />, and sets the <see cref="P:System.Drawing.Imaging.EncoderParameter.NumberOfValues" /> property to 1.</summary>
		/// <param name="encoder">An <see cref="T:System.Drawing.Imaging.Encoder" /> object that encapsulates the globally unique identifier of the parameter category. </param>
		/// <param name="value">A 16-bit integer that specifies the value stored in the <see cref="T:System.Drawing.Imaging.EncoderParameter" /> object. Must be nonnegative. </param>
		// Token: 0x06000934 RID: 2356 RVA: 0x00022FB8 File Offset: 0x000211B8
		public EncoderParameter(Encoder encoder, short value)
		{
			this.parameterGuid = encoder.Guid;
			this.parameterValueType = EncoderParameterValueType.ValueTypeShort;
			this.numberOfValues = 1;
			this.parameterValue = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(short)));
			if (this.parameterValue == IntPtr.Zero)
			{
				throw SafeNativeMethods.Gdip.StatusException(3);
			}
			Marshal.WriteInt16(this.parameterValue, value);
			GC.KeepAlive(this);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.EncoderParameter" /> class with the specified <see cref="T:System.Drawing.Imaging.Encoder" /> object and one 64-bit integer. Sets the <see cref="P:System.Drawing.Imaging.EncoderParameter.ValueType" /> property to <see cref="F:System.Drawing.Imaging.EncoderParameterValueType.ValueTypeLong" /> (32 bits), and sets the <see cref="P:System.Drawing.Imaging.EncoderParameter.NumberOfValues" /> property to 1.</summary>
		/// <param name="encoder">An <see cref="T:System.Drawing.Imaging.Encoder" /> object that encapsulates the globally unique identifier of the parameter category. </param>
		/// <param name="value">A 64-bit integer that specifies the value stored in the <see cref="T:System.Drawing.Imaging.EncoderParameter" /> object. Must be nonnegative. This parameter is converted to a 32-bit integer before it is stored in the <see cref="T:System.Drawing.Imaging.EncoderParameter" /> object. </param>
		// Token: 0x06000935 RID: 2357 RVA: 0x0002302C File Offset: 0x0002122C
		public EncoderParameter(Encoder encoder, long value)
		{
			this.parameterGuid = encoder.Guid;
			this.parameterValueType = EncoderParameterValueType.ValueTypeLong;
			this.numberOfValues = 1;
			this.parameterValue = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(int)));
			if (this.parameterValue == IntPtr.Zero)
			{
				throw SafeNativeMethods.Gdip.StatusException(3);
			}
			Marshal.WriteInt32(this.parameterValue, (int)value);
			GC.KeepAlive(this);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.EncoderParameter" /> class with the specified <see cref="T:System.Drawing.Imaging.Encoder" /> object and a pair of 32-bit integers. The pair of integers represents a fraction, the first integer being the numerator, and the second integer being the denominator. Sets the <see cref="P:System.Drawing.Imaging.EncoderParameter.ValueType" /> property to <see cref="F:System.Drawing.Imaging.EncoderParameterValueType.ValueTypeRational" />, and sets the <see cref="P:System.Drawing.Imaging.EncoderParameter.NumberOfValues" /> property to 1.</summary>
		/// <param name="encoder">An <see cref="T:System.Drawing.Imaging.Encoder" /> object that encapsulates the globally unique identifier of the parameter category. </param>
		/// <param name="numerator">A 32-bit integer that represents the numerator of a fraction. Must be nonnegative. </param>
		/// <param name="denominator">A 32-bit integer that represents the denominator of a fraction. Must be nonnegative. </param>
		// Token: 0x06000936 RID: 2358 RVA: 0x000230A0 File Offset: 0x000212A0
		public EncoderParameter(Encoder encoder, int numerator, int denominator)
		{
			this.parameterGuid = encoder.Guid;
			this.parameterValueType = EncoderParameterValueType.ValueTypeRational;
			this.numberOfValues = 1;
			int num = Marshal.SizeOf(typeof(int));
			this.parameterValue = Marshal.AllocHGlobal(2 * num);
			if (this.parameterValue == IntPtr.Zero)
			{
				throw SafeNativeMethods.Gdip.StatusException(3);
			}
			Marshal.WriteInt32(this.parameterValue, numerator);
			Marshal.WriteInt32(EncoderParameter.Add(this.parameterValue, num), denominator);
			GC.KeepAlive(this);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.EncoderParameter" /> class with the specified <see cref="T:System.Drawing.Imaging.Encoder" /> object and a pair of 64-bit integers. The pair of integers represents a range of integers, the first integer being the smallest number in the range, and the second integer being the largest number in the range. Sets the <see cref="P:System.Drawing.Imaging.EncoderParameter.ValueType" /> property to <see cref="F:System.Drawing.Imaging.EncoderParameterValueType.ValueTypeLongRange" />, and sets the <see cref="P:System.Drawing.Imaging.EncoderParameter.NumberOfValues" /> property to 1.</summary>
		/// <param name="encoder">An <see cref="T:System.Drawing.Imaging.Encoder" /> object that encapsulates the globally unique identifier of the parameter category. </param>
		/// <param name="rangebegin">A 64-bit integer that represents the smallest number in a range of integers. Must be nonnegative. This parameter is converted to a 32-bit integer before it is stored in the <see cref="T:System.Drawing.Imaging.EncoderParameter" /> object. </param>
		/// <param name="rangeend">A 64-bit integer that represents the largest number in a range of integers. Must be nonnegative. This parameter is converted to a 32-bit integer before it is stored in the <see cref="T:System.Drawing.Imaging.EncoderParameter" /> object. </param>
		// Token: 0x06000937 RID: 2359 RVA: 0x00023128 File Offset: 0x00021328
		public EncoderParameter(Encoder encoder, long rangebegin, long rangeend)
		{
			this.parameterGuid = encoder.Guid;
			this.parameterValueType = EncoderParameterValueType.ValueTypeLongRange;
			this.numberOfValues = 1;
			int num = Marshal.SizeOf(typeof(int));
			this.parameterValue = Marshal.AllocHGlobal(2 * num);
			if (this.parameterValue == IntPtr.Zero)
			{
				throw SafeNativeMethods.Gdip.StatusException(3);
			}
			Marshal.WriteInt32(this.parameterValue, (int)rangebegin);
			Marshal.WriteInt32(EncoderParameter.Add(this.parameterValue, num), (int)rangeend);
			GC.KeepAlive(this);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.EncoderParameter" /> class with the specified <see cref="T:System.Drawing.Imaging.Encoder" /> object and four, 32-bit integers. The four integers represent a range of fractions. The first two integers represent the smallest fraction in the range, and the remaining two integers represent the largest fraction in the range. Sets the <see cref="P:System.Drawing.Imaging.EncoderParameter.ValueType" /> property to <see cref="F:System.Drawing.Imaging.EncoderParameterValueType.ValueTypeRationalRange" />, and sets the <see cref="P:System.Drawing.Imaging.EncoderParameter.NumberOfValues" /> property to 1.</summary>
		/// <param name="encoder">An <see cref="T:System.Drawing.Imaging.Encoder" /> object that encapsulates the globally unique identifier of the parameter category. </param>
		/// <param name="numerator1">A 32-bit integer that represents the numerator of the smallest fraction in the range. Must be nonnegative. </param>
		/// <param name="demoninator1">A 32-bit integer that represents the denominator of the smallest fraction in the range. Must be nonnegative. </param>
		/// <param name="numerator2">A 32-bit integer that represents the denominator of the smallest fraction in the range. Must be nonnegative. </param>
		/// <param name="demoninator2">A 32-bit integer that represents the numerator of the largest fraction in the range. Must be nonnegative. </param>
		// Token: 0x06000938 RID: 2360 RVA: 0x000231B4 File Offset: 0x000213B4
		public EncoderParameter(Encoder encoder, int numerator1, int demoninator1, int numerator2, int demoninator2)
		{
			this.parameterGuid = encoder.Guid;
			this.parameterValueType = EncoderParameterValueType.ValueTypeRationalRange;
			this.numberOfValues = 1;
			int num = Marshal.SizeOf(typeof(int));
			this.parameterValue = Marshal.AllocHGlobal(4 * num);
			if (this.parameterValue == IntPtr.Zero)
			{
				throw SafeNativeMethods.Gdip.StatusException(3);
			}
			Marshal.WriteInt32(this.parameterValue, numerator1);
			Marshal.WriteInt32(EncoderParameter.Add(this.parameterValue, num), demoninator1);
			Marshal.WriteInt32(EncoderParameter.Add(this.parameterValue, 2 * num), numerator2);
			Marshal.WriteInt32(EncoderParameter.Add(this.parameterValue, 3 * num), demoninator2);
			GC.KeepAlive(this);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.EncoderParameter" /> class with the specified <see cref="T:System.Drawing.Imaging.Encoder" /> object and a character string. The string is converted to a null-terminated ASCII string before it is stored in the <see cref="T:System.Drawing.Imaging.EncoderParameter" /> object. Sets the <see cref="P:System.Drawing.Imaging.EncoderParameter.ValueType" /> property to <see cref="F:System.Drawing.Imaging.EncoderParameterValueType.ValueTypeAscii" />, and sets the <see cref="P:System.Drawing.Imaging.EncoderParameter.NumberOfValues" /> property to the length of the ASCII string including the NULL terminator.</summary>
		/// <param name="encoder">An <see cref="T:System.Drawing.Imaging.Encoder" /> object that encapsulates the globally unique identifier of the parameter category. </param>
		/// <param name="value">A <see cref="T:System.String" /> that specifies the value stored in the <see cref="T:System.Drawing.Imaging.EncoderParameter" /> object. </param>
		// Token: 0x06000939 RID: 2361 RVA: 0x00023268 File Offset: 0x00021468
		public EncoderParameter(Encoder encoder, string value)
		{
			this.parameterGuid = encoder.Guid;
			this.parameterValueType = EncoderParameterValueType.ValueTypeAscii;
			this.numberOfValues = value.Length;
			this.parameterValue = Marshal.StringToHGlobalAnsi(value);
			GC.KeepAlive(this);
			if (this.parameterValue == IntPtr.Zero)
			{
				throw SafeNativeMethods.Gdip.StatusException(3);
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.EncoderParameter" /> class with the specified <see cref="T:System.Drawing.Imaging.Encoder" /> object and an array of unsigned 8-bit integers. Sets the <see cref="P:System.Drawing.Imaging.EncoderParameter.ValueType" /> property to <see cref="F:System.Drawing.Imaging.EncoderParameterValueType.ValueTypeByte" />, and sets the <see cref="P:System.Drawing.Imaging.EncoderParameter.NumberOfValues" /> property to the number of elements in the array.</summary>
		/// <param name="encoder">An <see cref="T:System.Drawing.Imaging.Encoder" /> object that encapsulates the globally unique identifier of the parameter category. </param>
		/// <param name="value">An array of 8-bit unsigned integers that specifies the values stored in the <see cref="T:System.Drawing.Imaging.EncoderParameter" /> object. </param>
		// Token: 0x0600093A RID: 2362 RVA: 0x000232C8 File Offset: 0x000214C8
		public EncoderParameter(Encoder encoder, byte[] value)
		{
			this.parameterGuid = encoder.Guid;
			this.parameterValueType = EncoderParameterValueType.ValueTypeByte;
			this.numberOfValues = value.Length;
			this.parameterValue = Marshal.AllocHGlobal(this.numberOfValues);
			if (this.parameterValue == IntPtr.Zero)
			{
				throw SafeNativeMethods.Gdip.StatusException(3);
			}
			Marshal.Copy(value, 0, this.parameterValue, this.numberOfValues);
			GC.KeepAlive(this);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.EncoderParameter" /> class with the specified <see cref="T:System.Drawing.Imaging.Encoder" /> object and an array of bytes. Sets the <see cref="P:System.Drawing.Imaging.EncoderParameter.ValueType" /> property to <see cref="F:System.Drawing.Imaging.EncoderParameterValueType.ValueTypeUndefined" /> or <see cref="F:System.Drawing.Imaging.EncoderParameterValueType.ValueTypeByte" />, and sets the <see cref="P:System.Drawing.Imaging.EncoderParameter.NumberOfValues" /> property to the number of elements in the array.</summary>
		/// <param name="encoder">An <see cref="T:System.Drawing.Imaging.Encoder" /> object that encapsulates the globally unique identifier of the parameter category. </param>
		/// <param name="value">An array of bytes that specifies the values stored in the <see cref="T:System.Drawing.Imaging.EncoderParameter" /> object. </param>
		/// <param name="undefined">If <see langword="true" />, the <see cref="P:System.Drawing.Imaging.EncoderParameter.ValueType" /> property is set to <see cref="F:System.Drawing.Imaging.EncoderParameterValueType.ValueTypeUndefined" />; otherwise, the <see cref="P:System.Drawing.Imaging.EncoderParameter.ValueType" /> property is set to <see cref="F:System.Drawing.Imaging.EncoderParameterValueType.ValueTypeByte" />. </param>
		// Token: 0x0600093B RID: 2363 RVA: 0x0002333C File Offset: 0x0002153C
		public EncoderParameter(Encoder encoder, byte[] value, bool undefined)
		{
			this.parameterGuid = encoder.Guid;
			if (undefined)
			{
				this.parameterValueType = EncoderParameterValueType.ValueTypeUndefined;
			}
			else
			{
				this.parameterValueType = EncoderParameterValueType.ValueTypeByte;
			}
			this.numberOfValues = value.Length;
			this.parameterValue = Marshal.AllocHGlobal(this.numberOfValues);
			if (this.parameterValue == IntPtr.Zero)
			{
				throw SafeNativeMethods.Gdip.StatusException(3);
			}
			Marshal.Copy(value, 0, this.parameterValue, this.numberOfValues);
			GC.KeepAlive(this);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.EncoderParameter" /> class with the specified <see cref="T:System.Drawing.Imaging.Encoder" /> object and an array of 16-bit integers. Sets the <see cref="P:System.Drawing.Imaging.EncoderParameter.ValueType" /> property to <see cref="F:System.Drawing.Imaging.EncoderParameterValueType.ValueTypeShort" />, and sets the <see cref="P:System.Drawing.Imaging.EncoderParameter.NumberOfValues" /> property to the number of elements in the array.</summary>
		/// <param name="encoder">An <see cref="T:System.Drawing.Imaging.Encoder" /> object that encapsulates the globally unique identifier of the parameter category. </param>
		/// <param name="value">An array of 16-bit integers that specifies the values stored in the <see cref="T:System.Drawing.Imaging.EncoderParameter" /> object. The integers in the array must be nonnegative. </param>
		// Token: 0x0600093C RID: 2364 RVA: 0x000233BC File Offset: 0x000215BC
		public EncoderParameter(Encoder encoder, short[] value)
		{
			this.parameterGuid = encoder.Guid;
			this.parameterValueType = EncoderParameterValueType.ValueTypeShort;
			this.numberOfValues = value.Length;
			int num = Marshal.SizeOf(typeof(short));
			this.parameterValue = Marshal.AllocHGlobal(checked(this.numberOfValues * num));
			if (this.parameterValue == IntPtr.Zero)
			{
				throw SafeNativeMethods.Gdip.StatusException(3);
			}
			Marshal.Copy(value, 0, this.parameterValue, this.numberOfValues);
			GC.KeepAlive(this);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.EncoderParameter" /> class with the specified <see cref="T:System.Drawing.Imaging.Encoder" /> object and an array of 64-bit integers. Sets the <see cref="P:System.Drawing.Imaging.EncoderParameter.ValueType" /> property to <see cref="F:System.Drawing.Imaging.EncoderParameterValueType.ValueTypeLong" /> (32-bit), and sets the <see cref="P:System.Drawing.Imaging.EncoderParameter.NumberOfValues" /> property to the number of elements in the array.</summary>
		/// <param name="encoder">An <see cref="T:System.Drawing.Imaging.Encoder" /> object that encapsulates the globally unique identifier of the parameter category. </param>
		/// <param name="value">An array of 64-bit integers that specifies the values stored in the <see cref="T:System.Drawing.Imaging.EncoderParameter" /> object. The integers in the array must be nonnegative. The 64-bit integers are converted to 32-bit integers before they are stored in the <see cref="T:System.Drawing.Imaging.EncoderParameter" /> object. </param>
		// Token: 0x0600093D RID: 2365 RVA: 0x00023440 File Offset: 0x00021640
		public unsafe EncoderParameter(Encoder encoder, long[] value)
		{
			this.parameterGuid = encoder.Guid;
			this.parameterValueType = EncoderParameterValueType.ValueTypeLong;
			this.numberOfValues = value.Length;
			int num = Marshal.SizeOf(typeof(int));
			this.parameterValue = Marshal.AllocHGlobal(checked(this.numberOfValues * num));
			if (this.parameterValue == IntPtr.Zero)
			{
				throw SafeNativeMethods.Gdip.StatusException(3);
			}
			int* ptr = (int*)((void*)this.parameterValue);
			fixed (long* ptr2 = value)
			{
				for (int i = 0; i < value.Length; i++)
				{
					ptr[i] = (int)ptr2[i];
				}
			}
			GC.KeepAlive(this);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.EncoderParameter" /> class with the specified <see cref="T:System.Drawing.Imaging.Encoder" /> object and two arrays of 32-bit integers. The two arrays represent an array of fractions. Sets the <see cref="P:System.Drawing.Imaging.EncoderParameter.ValueType" /> property to <see cref="F:System.Drawing.Imaging.EncoderParameterValueType.ValueTypeRational" />, and sets the <see cref="P:System.Drawing.Imaging.EncoderParameter.NumberOfValues" /> property to the number of elements in the <paramref name="numerator" /> array, which must be the same as the number of elements in the <paramref name="denominator" /> array.</summary>
		/// <param name="encoder">An <see cref="T:System.Drawing.Imaging.Encoder" /> object that encapsulates the globally unique identifier of the parameter category. </param>
		/// <param name="numerator">An array of 32-bit integers that specifies the numerators of the fractions. The integers in the array must be nonnegative. </param>
		/// <param name="denominator">An array of 32-bit integers that specifies the denominators of the fractions. The integers in the array must be nonnegative. A denominator of a given index is paired with the numerator of the same index. </param>
		// Token: 0x0600093E RID: 2366 RVA: 0x000234FC File Offset: 0x000216FC
		public EncoderParameter(Encoder encoder, int[] numerator, int[] denominator)
		{
			this.parameterGuid = encoder.Guid;
			if (numerator.Length != denominator.Length)
			{
				throw SafeNativeMethods.Gdip.StatusException(2);
			}
			this.parameterValueType = EncoderParameterValueType.ValueTypeRational;
			this.numberOfValues = numerator.Length;
			int num = Marshal.SizeOf(typeof(int));
			this.parameterValue = Marshal.AllocHGlobal(checked(this.numberOfValues * 2 * num));
			if (this.parameterValue == IntPtr.Zero)
			{
				throw SafeNativeMethods.Gdip.StatusException(3);
			}
			for (int i = 0; i < this.numberOfValues; i++)
			{
				Marshal.WriteInt32(EncoderParameter.Add(i * 2 * num, this.parameterValue), numerator[i]);
				Marshal.WriteInt32(EncoderParameter.Add((i * 2 + 1) * num, this.parameterValue), denominator[i]);
			}
			GC.KeepAlive(this);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.EncoderParameter" /> class with the specified <see cref="T:System.Drawing.Imaging.Encoder" /> object and two arrays of 64-bit integers. The two arrays represent an array integer ranges. Sets the <see cref="P:System.Drawing.Imaging.EncoderParameter.ValueType" /> property to <see cref="F:System.Drawing.Imaging.EncoderParameterValueType.ValueTypeLongRange" />, and sets the <see cref="P:System.Drawing.Imaging.EncoderParameter.NumberOfValues" /> property to the number of elements in the <paramref name="rangebegin" /> array, which must be the same as the number of elements in the <paramref name="rangeend" /> array.</summary>
		/// <param name="encoder">An <see cref="T:System.Drawing.Imaging.Encoder" /> object that encapsulates the globally unique identifier of the parameter category. </param>
		/// <param name="rangebegin">An array of 64-bit integers that specifies the minimum values for the integer ranges. The integers in the array must be nonnegative. The 64-bit integers are converted to 32-bit integers before they are stored in the <see cref="T:System.Drawing.Imaging.EncoderParameter" /> object. </param>
		/// <param name="rangeend">An array of 64-bit integers that specifies the maximum values for the integer ranges. The integers in the array must be nonnegative. The 64-bit integers are converted to 32-bit integers before they are stored in the <see cref="T:System.Drawing.Imaging.EncoderParameters" /> object. A maximum value of a given index is paired with the minimum value of the same index. </param>
		// Token: 0x0600093F RID: 2367 RVA: 0x000235C4 File Offset: 0x000217C4
		public EncoderParameter(Encoder encoder, long[] rangebegin, long[] rangeend)
		{
			this.parameterGuid = encoder.Guid;
			if (rangebegin.Length != rangeend.Length)
			{
				throw SafeNativeMethods.Gdip.StatusException(2);
			}
			this.parameterValueType = EncoderParameterValueType.ValueTypeLongRange;
			this.numberOfValues = rangebegin.Length;
			int num = Marshal.SizeOf(typeof(int));
			this.parameterValue = Marshal.AllocHGlobal(checked(this.numberOfValues * 2 * num));
			if (this.parameterValue == IntPtr.Zero)
			{
				throw SafeNativeMethods.Gdip.StatusException(3);
			}
			for (int i = 0; i < this.numberOfValues; i++)
			{
				Marshal.WriteInt32(EncoderParameter.Add(i * 2 * num, this.parameterValue), (int)rangebegin[i]);
				Marshal.WriteInt32(EncoderParameter.Add((i * 2 + 1) * num, this.parameterValue), (int)rangeend[i]);
			}
			GC.KeepAlive(this);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.EncoderParameter" /> class with the specified <see cref="T:System.Drawing.Imaging.Encoder" /> object and four arrays of 32-bit integers. The four arrays represent an array rational ranges. A rational range is the set of all fractions from a minimum fractional value through a maximum fractional value. Sets the <see cref="P:System.Drawing.Imaging.EncoderParameter.ValueType" /> property to <see cref="F:System.Drawing.Imaging.EncoderParameterValueType.ValueTypeRationalRange" />, and sets the <see cref="P:System.Drawing.Imaging.EncoderParameter.NumberOfValues" /> property to the number of elements in the <paramref name="numerator1" /> array, which must be the same as the number of elements in the other three arrays.</summary>
		/// <param name="encoder">An <see cref="T:System.Drawing.Imaging.Encoder" /> object that encapsulates the globally unique identifier of the parameter category. </param>
		/// <param name="numerator1">An array of 32-bit integers that specifies the numerators of the minimum values for the ranges. The integers in the array must be nonnegative. </param>
		/// <param name="denominator1">An array of 32-bit integers that specifies the denominators of the minimum values for the ranges. The integers in the array must be nonnegative. </param>
		/// <param name="numerator2">An array of 32-bit integers that specifies the numerators of the maximum values for the ranges. The integers in the array must be nonnegative. </param>
		/// <param name="denominator2">An array of 32-bit integers that specifies the denominators of the maximum values for the ranges. The integers in the array must be nonnegative. </param>
		// Token: 0x06000940 RID: 2368 RVA: 0x0002368C File Offset: 0x0002188C
		public EncoderParameter(Encoder encoder, int[] numerator1, int[] denominator1, int[] numerator2, int[] denominator2)
		{
			this.parameterGuid = encoder.Guid;
			if (numerator1.Length != denominator1.Length || numerator1.Length != denominator2.Length || denominator1.Length != denominator2.Length)
			{
				throw SafeNativeMethods.Gdip.StatusException(2);
			}
			this.parameterValueType = EncoderParameterValueType.ValueTypeRationalRange;
			this.numberOfValues = numerator1.Length;
			int num = Marshal.SizeOf(typeof(int));
			this.parameterValue = Marshal.AllocHGlobal(checked(this.numberOfValues * 4 * num));
			if (this.parameterValue == IntPtr.Zero)
			{
				throw SafeNativeMethods.Gdip.StatusException(3);
			}
			for (int i = 0; i < this.numberOfValues; i++)
			{
				Marshal.WriteInt32(EncoderParameter.Add(this.parameterValue, 4 * i * num), numerator1[i]);
				Marshal.WriteInt32(EncoderParameter.Add(this.parameterValue, (4 * i + 1) * num), denominator1[i]);
				Marshal.WriteInt32(EncoderParameter.Add(this.parameterValue, (4 * i + 2) * num), numerator2[i]);
				Marshal.WriteInt32(EncoderParameter.Add(this.parameterValue, (4 * i + 3) * num), denominator2[i]);
			}
			GC.KeepAlive(this);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.EncoderParameter" /> class with the specified <see cref="T:System.Drawing.Imaging.Encoder" /> object and three integers that specify the number of values, the data type of the values, and a pointer to the values stored in the <see cref="T:System.Drawing.Imaging.EncoderParameter" /> object.</summary>
		/// <param name="encoder">An <see cref="T:System.Drawing.Imaging.Encoder" /> object that encapsulates the globally unique identifier of the parameter category. </param>
		/// <param name="NumberOfValues">An integer that specifies the number of values stored in the <see cref="T:System.Drawing.Imaging.EncoderParameter" /> object. The <see cref="P:System.Drawing.Imaging.EncoderParameter.NumberOfValues" /> property is set to this value. </param>
		/// <param name="Type">A member of the <see cref="T:System.Drawing.Imaging.EncoderParameterValueType" /> enumeration that specifies the data type of the values stored in the <see cref="T:System.Drawing.Imaging.EncoderParameter" /> object. The <see cref="T:System.Type" /> and <see cref="P:System.Drawing.Imaging.EncoderParameter.ValueType" /> properties are set to this value. </param>
		/// <param name="Value">A pointer to an array of values of the type specified by the <paramref name="Type" /> parameter.</param>
		/// <exception cref="T:System.InvalidOperationException">Type is not a valid <see cref="T:System.Drawing.Imaging.EncoderParameterValueType" />.</exception>
		// Token: 0x06000941 RID: 2369 RVA: 0x0002379C File Offset: 0x0002199C
		[Obsolete("This constructor has been deprecated. Use EncoderParameter(Encoder encoder, int numberValues, EncoderParameterValueType type, IntPtr value) instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		public EncoderParameter(Encoder encoder, int NumberOfValues, int Type, int Value)
		{
			IntSecurity.UnmanagedCode.Demand();
			int num;
			switch (Type)
			{
			case 1:
			case 2:
				num = 1;
				break;
			case 3:
				num = 2;
				break;
			case 4:
				num = 4;
				break;
			case 5:
			case 6:
				num = 8;
				break;
			case 7:
				num = 1;
				break;
			case 8:
				num = 16;
				break;
			default:
				throw SafeNativeMethods.Gdip.StatusException(8);
			}
			int num2 = checked(num * NumberOfValues);
			this.parameterValue = Marshal.AllocHGlobal(num2);
			if (this.parameterValue == IntPtr.Zero)
			{
				throw SafeNativeMethods.Gdip.StatusException(3);
			}
			for (int i = 0; i < num2; i++)
			{
				Marshal.WriteByte(EncoderParameter.Add(this.parameterValue, i), Marshal.ReadByte((IntPtr)(Value + i)));
			}
			this.parameterValueType = (EncoderParameterValueType)Type;
			this.numberOfValues = NumberOfValues;
			this.parameterGuid = encoder.Guid;
			GC.KeepAlive(this);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.EncoderParameter" /> class with the specified <see cref="T:System.Drawing.Imaging.Encoder" /> object, number of values, data type of the values, and a pointer to the values stored in the <see cref="T:System.Drawing.Imaging.EncoderParameter" /> object.</summary>
		/// <param name="encoder">An <see cref="T:System.Drawing.Imaging.Encoder" /> object that encapsulates the globally unique identifier of the parameter category. </param>
		/// <param name="numberValues">An integer that specifies the number of values stored in the <see cref="T:System.Drawing.Imaging.EncoderParameter" /> object. The <see cref="P:System.Drawing.Imaging.EncoderParameter.NumberOfValues" /> property is set to this value. </param>
		/// <param name="type">A member of the <see cref="T:System.Drawing.Imaging.EncoderParameterValueType" /> enumeration that specifies the data type of the values stored in the <see cref="T:System.Drawing.Imaging.EncoderParameter" /> object. The <see cref="T:System.Type" /> and <see cref="P:System.Drawing.Imaging.EncoderParameter.ValueType" /> properties are set to this value. </param>
		/// <param name="value">A pointer to an array of values of the type specified by the <paramref name="Type" /> parameter.</param>
		// Token: 0x06000942 RID: 2370 RVA: 0x0002387C File Offset: 0x00021A7C
		public EncoderParameter(Encoder encoder, int numberValues, EncoderParameterValueType type, IntPtr value)
		{
			IntSecurity.UnmanagedCode.Demand();
			int num;
			switch (type)
			{
			case EncoderParameterValueType.ValueTypeByte:
			case EncoderParameterValueType.ValueTypeAscii:
				num = 1;
				break;
			case EncoderParameterValueType.ValueTypeShort:
				num = 2;
				break;
			case EncoderParameterValueType.ValueTypeLong:
				num = 4;
				break;
			case EncoderParameterValueType.ValueTypeRational:
			case EncoderParameterValueType.ValueTypeLongRange:
				num = 8;
				break;
			case EncoderParameterValueType.ValueTypeUndefined:
				num = 1;
				break;
			case EncoderParameterValueType.ValueTypeRationalRange:
				num = 16;
				break;
			default:
				throw SafeNativeMethods.Gdip.StatusException(8);
			}
			int num2 = checked(num * numberValues);
			this.parameterValue = Marshal.AllocHGlobal(num2);
			if (this.parameterValue == IntPtr.Zero)
			{
				throw SafeNativeMethods.Gdip.StatusException(3);
			}
			for (int i = 0; i < num2; i++)
			{
				Marshal.WriteByte(EncoderParameter.Add(this.parameterValue, i), Marshal.ReadByte(value + i));
			}
			this.parameterValueType = type;
			this.numberOfValues = numberValues;
			this.parameterGuid = encoder.Guid;
			GC.KeepAlive(this);
		}

		// Token: 0x06000943 RID: 2371 RVA: 0x00023956 File Offset: 0x00021B56
		private static IntPtr Add(IntPtr a, int b)
		{
			return (IntPtr)((long)a + (long)b);
		}

		// Token: 0x06000944 RID: 2372 RVA: 0x00023966 File Offset: 0x00021B66
		private static IntPtr Add(int a, IntPtr b)
		{
			return (IntPtr)((long)a + (long)b);
		}

		// Token: 0x0400087E RID: 2174
		[MarshalAs(UnmanagedType.Struct)]
		private Guid parameterGuid;

		// Token: 0x0400087F RID: 2175
		private int numberOfValues;

		// Token: 0x04000880 RID: 2176
		private EncoderParameterValueType parameterValueType;

		// Token: 0x04000881 RID: 2177
		private IntPtr parameterValue;
	}
}
