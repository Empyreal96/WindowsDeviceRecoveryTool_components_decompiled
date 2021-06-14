using System;
using System.ComponentModel;
using System.Drawing.Text;
using System.Runtime.InteropServices;

namespace System.Drawing
{
	/// <summary>Encapsulates text layout information (such as alignment, orientation and tab stops) display manipulations (such as ellipsis insertion and national digit substitution) and OpenType features. This class cannot be inherited.</summary>
	// Token: 0x02000047 RID: 71
	public sealed class StringFormat : MarshalByRefObject, ICloneable, IDisposable
	{
		// Token: 0x060006B8 RID: 1720 RVA: 0x0001B77D File Offset: 0x0001997D
		private StringFormat(IntPtr format)
		{
			this.nativeFormat = format;
		}

		/// <summary>Initializes a new <see cref="T:System.Drawing.StringFormat" /> object.</summary>
		// Token: 0x060006B9 RID: 1721 RVA: 0x0001B78C File Offset: 0x0001998C
		public StringFormat() : this((StringFormatFlags)0, 0)
		{
		}

		/// <summary>Initializes a new <see cref="T:System.Drawing.StringFormat" /> object with the specified <see cref="T:System.Drawing.StringFormatFlags" /> enumeration.</summary>
		/// <param name="options">The <see cref="T:System.Drawing.StringFormatFlags" /> enumeration for the new <see cref="T:System.Drawing.StringFormat" /> object. </param>
		// Token: 0x060006BA RID: 1722 RVA: 0x0001B796 File Offset: 0x00019996
		public StringFormat(StringFormatFlags options) : this(options, 0)
		{
		}

		/// <summary>Initializes a new <see cref="T:System.Drawing.StringFormat" /> object with the specified <see cref="T:System.Drawing.StringFormatFlags" /> enumeration and language.</summary>
		/// <param name="options">The <see cref="T:System.Drawing.StringFormatFlags" /> enumeration for the new <see cref="T:System.Drawing.StringFormat" /> object. </param>
		/// <param name="language">A value that indicates the language of the text. </param>
		// Token: 0x060006BB RID: 1723 RVA: 0x0001B7A0 File Offset: 0x000199A0
		public StringFormat(StringFormatFlags options, int language)
		{
			int num = SafeNativeMethods.Gdip.GdipCreateStringFormat(options, language, out this.nativeFormat);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Initializes a new <see cref="T:System.Drawing.StringFormat" /> object from the specified existing <see cref="T:System.Drawing.StringFormat" /> object.</summary>
		/// <param name="format">The <see cref="T:System.Drawing.StringFormat" /> object from which to initialize the new <see cref="T:System.Drawing.StringFormat" /> object. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="format" /> is <see langword="null" />.</exception>
		// Token: 0x060006BC RID: 1724 RVA: 0x0001B7CC File Offset: 0x000199CC
		public StringFormat(StringFormat format)
		{
			if (format == null)
			{
				throw new ArgumentNullException("format");
			}
			int num = SafeNativeMethods.Gdip.GdipCloneStringFormat(new HandleRef(format, format.nativeFormat), out this.nativeFormat);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Releases all resources used by this <see cref="T:System.Drawing.StringFormat" /> object.</summary>
		// Token: 0x060006BD RID: 1725 RVA: 0x0001B80F File Offset: 0x00019A0F
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060006BE RID: 1726 RVA: 0x0001B820 File Offset: 0x00019A20
		private void Dispose(bool disposing)
		{
			if (this.nativeFormat != IntPtr.Zero)
			{
				try
				{
					SafeNativeMethods.Gdip.GdipDeleteStringFormat(new HandleRef(this, this.nativeFormat));
				}
				catch (Exception ex)
				{
					if (ClientUtils.IsCriticalException(ex))
					{
						throw;
					}
				}
				finally
				{
					this.nativeFormat = IntPtr.Zero;
				}
			}
		}

		/// <summary>Creates an exact copy of this <see cref="T:System.Drawing.StringFormat" /> object.</summary>
		/// <returns>The <see cref="T:System.Drawing.StringFormat" /> object this method creates.</returns>
		// Token: 0x060006BF RID: 1727 RVA: 0x0001B888 File Offset: 0x00019A88
		public object Clone()
		{
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCloneStringFormat(new HandleRef(this, this.nativeFormat), out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return new StringFormat(zero);
		}

		/// <summary>Gets or sets a <see cref="T:System.Drawing.StringFormatFlags" /> enumeration that contains formatting information.</summary>
		/// <returns>A <see cref="T:System.Drawing.StringFormatFlags" /> enumeration that contains formatting information.</returns>
		// Token: 0x170002B6 RID: 694
		// (get) Token: 0x060006C0 RID: 1728 RVA: 0x0001B8C4 File Offset: 0x00019AC4
		// (set) Token: 0x060006C1 RID: 1729 RVA: 0x0001B8F0 File Offset: 0x00019AF0
		public StringFormatFlags FormatFlags
		{
			get
			{
				StringFormatFlags result;
				int num = SafeNativeMethods.Gdip.GdipGetStringFormatFlags(new HandleRef(this, this.nativeFormat), out result);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return result;
			}
			set
			{
				int num = SafeNativeMethods.Gdip.GdipSetStringFormatFlags(new HandleRef(this, this.nativeFormat), value);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
		}

		/// <summary>Specifies an array of <see cref="T:System.Drawing.CharacterRange" /> structures that represent the ranges of characters measured by a call to the <see cref="M:System.Drawing.Graphics.MeasureCharacterRanges(System.String,System.Drawing.Font,System.Drawing.RectangleF,System.Drawing.StringFormat)" /> method.</summary>
		/// <param name="ranges">An array of <see cref="T:System.Drawing.CharacterRange" /> structures that specifies the ranges of characters measured by a call to the <see cref="M:System.Drawing.Graphics.MeasureCharacterRanges(System.String,System.Drawing.Font,System.Drawing.RectangleF,System.Drawing.StringFormat)" /> method. </param>
		/// <exception cref="T:System.OverflowException">More than 32 character ranges are set.</exception>
		// Token: 0x060006C2 RID: 1730 RVA: 0x0001B91C File Offset: 0x00019B1C
		public void SetMeasurableCharacterRanges(CharacterRange[] ranges)
		{
			int num = SafeNativeMethods.Gdip.GdipSetStringFormatMeasurableCharacterRanges(new HandleRef(this, this.nativeFormat), ranges.Length, ranges);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Gets or sets horizontal alignment of the string.</summary>
		/// <returns>A <see cref="T:System.Drawing.StringAlignment" /> enumeration that specifies the horizontal  alignment of the string.</returns>
		// Token: 0x170002B7 RID: 695
		// (get) Token: 0x060006C3 RID: 1731 RVA: 0x0001B94C File Offset: 0x00019B4C
		// (set) Token: 0x060006C4 RID: 1732 RVA: 0x0001B97C File Offset: 0x00019B7C
		public StringAlignment Alignment
		{
			get
			{
				StringAlignment result = StringAlignment.Near;
				int num = SafeNativeMethods.Gdip.GdipGetStringFormatAlign(new HandleRef(this, this.nativeFormat), out result);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return result;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(StringAlignment));
				}
				int num = SafeNativeMethods.Gdip.GdipSetStringFormatAlign(new HandleRef(this, this.nativeFormat), value);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
		}

		/// <summary>Gets or sets the vertical alignment of the string.</summary>
		/// <returns>A <see cref="T:System.Drawing.StringAlignment" /> enumeration that represents the vertical line alignment.</returns>
		// Token: 0x170002B8 RID: 696
		// (get) Token: 0x060006C5 RID: 1733 RVA: 0x0001B9CC File Offset: 0x00019BCC
		// (set) Token: 0x060006C6 RID: 1734 RVA: 0x0001B9FC File Offset: 0x00019BFC
		public StringAlignment LineAlignment
		{
			get
			{
				StringAlignment result = StringAlignment.Near;
				int num = SafeNativeMethods.Gdip.GdipGetStringFormatLineAlign(new HandleRef(this, this.nativeFormat), out result);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return result;
			}
			set
			{
				if (value < StringAlignment.Near || value > StringAlignment.Far)
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(StringAlignment));
				}
				int num = SafeNativeMethods.Gdip.GdipSetStringFormatLineAlign(new HandleRef(this, this.nativeFormat), value);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Drawing.Text.HotkeyPrefix" /> object for this <see cref="T:System.Drawing.StringFormat" /> object.</summary>
		/// <returns>The <see cref="T:System.Drawing.Text.HotkeyPrefix" /> object for this <see cref="T:System.Drawing.StringFormat" /> object, the default is <see cref="F:System.Drawing.Text.HotkeyPrefix.None" />.</returns>
		// Token: 0x170002B9 RID: 697
		// (get) Token: 0x060006C7 RID: 1735 RVA: 0x0001BA44 File Offset: 0x00019C44
		// (set) Token: 0x060006C8 RID: 1736 RVA: 0x0001BA70 File Offset: 0x00019C70
		public HotkeyPrefix HotkeyPrefix
		{
			get
			{
				HotkeyPrefix result;
				int num = SafeNativeMethods.Gdip.GdipGetStringFormatHotkeyPrefix(new HandleRef(this, this.nativeFormat), out result);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return result;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(HotkeyPrefix));
				}
				int num = SafeNativeMethods.Gdip.GdipSetStringFormatHotkeyPrefix(new HandleRef(this, this.nativeFormat), value);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
		}

		/// <summary>Sets tab stops for this <see cref="T:System.Drawing.StringFormat" /> object.</summary>
		/// <param name="firstTabOffset">The number of spaces between the beginning of a line of text and the first tab stop. </param>
		/// <param name="tabStops">An array of distances between tab stops in the units specified by the <see cref="P:System.Drawing.Graphics.PageUnit" /> property. </param>
		// Token: 0x060006C9 RID: 1737 RVA: 0x0001BAC0 File Offset: 0x00019CC0
		public void SetTabStops(float firstTabOffset, float[] tabStops)
		{
			if (firstTabOffset < 0f)
			{
				throw new ArgumentException(SR.GetString("InvalidArgument", new object[]
				{
					"firstTabOffset",
					firstTabOffset
				}));
			}
			int num = SafeNativeMethods.Gdip.GdipSetStringFormatTabStops(new HandleRef(this, this.nativeFormat), firstTabOffset, tabStops.Length, tabStops);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Gets the tab stops for this <see cref="T:System.Drawing.StringFormat" /> object.</summary>
		/// <param name="firstTabOffset">The number of spaces between the beginning of a text line and the first tab stop. </param>
		/// <returns>An array of distances (in number of spaces) between tab stops.</returns>
		// Token: 0x060006CA RID: 1738 RVA: 0x0001BB20 File Offset: 0x00019D20
		public float[] GetTabStops(out float firstTabOffset)
		{
			int num = 0;
			int num2 = SafeNativeMethods.Gdip.GdipGetStringFormatTabStopCount(new HandleRef(this, this.nativeFormat), out num);
			if (num2 != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num2);
			}
			float[] array = new float[num];
			num2 = SafeNativeMethods.Gdip.GdipGetStringFormatTabStops(new HandleRef(this, this.nativeFormat), num, out firstTabOffset, array);
			if (num2 != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num2);
			}
			return array;
		}

		/// <summary>Gets or sets the <see cref="T:System.Drawing.StringTrimming" /> enumeration for this <see cref="T:System.Drawing.StringFormat" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.StringTrimming" /> enumeration that indicates how text drawn with this <see cref="T:System.Drawing.StringFormat" /> object is trimmed when it exceeds the edges of the layout rectangle.</returns>
		// Token: 0x170002BA RID: 698
		// (get) Token: 0x060006CB RID: 1739 RVA: 0x0001BB74 File Offset: 0x00019D74
		// (set) Token: 0x060006CC RID: 1740 RVA: 0x0001BBA0 File Offset: 0x00019DA0
		public StringTrimming Trimming
		{
			get
			{
				StringTrimming result;
				int num = SafeNativeMethods.Gdip.GdipGetStringFormatTrimming(new HandleRef(this, this.nativeFormat), out result);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return result;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 5))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(StringTrimming));
				}
				int num = SafeNativeMethods.Gdip.GdipSetStringFormatTrimming(new HandleRef(this, this.nativeFormat), value);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
		}

		/// <summary>Gets a generic default <see cref="T:System.Drawing.StringFormat" /> object.</summary>
		/// <returns>The generic default <see cref="T:System.Drawing.StringFormat" /> object.</returns>
		// Token: 0x170002BB RID: 699
		// (get) Token: 0x060006CD RID: 1741 RVA: 0x0001BBF0 File Offset: 0x00019DF0
		public static StringFormat GenericDefault
		{
			get
			{
				IntPtr format;
				int num = SafeNativeMethods.Gdip.GdipStringFormatGetGenericDefault(out format);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return new StringFormat(format);
			}
		}

		/// <summary>Gets a generic typographic <see cref="T:System.Drawing.StringFormat" /> object.</summary>
		/// <returns>A generic typographic <see cref="T:System.Drawing.StringFormat" /> object.</returns>
		// Token: 0x170002BC RID: 700
		// (get) Token: 0x060006CE RID: 1742 RVA: 0x0001BC18 File Offset: 0x00019E18
		public static StringFormat GenericTypographic
		{
			get
			{
				IntPtr format;
				int num = SafeNativeMethods.Gdip.GdipStringFormatGetGenericTypographic(out format);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return new StringFormat(format);
			}
		}

		/// <summary>Specifies the language and method to be used when local digits are substituted for western digits.</summary>
		/// <param name="language">A National Language Support (NLS) language identifier that identifies the language that will be used when local digits are substituted for western digits. You can pass the <see cref="P:System.Globalization.CultureInfo.LCID" /> property of a <see cref="T:System.Globalization.CultureInfo" /> object as the NLS language identifier. For example, suppose you create a <see cref="T:System.Globalization.CultureInfo" /> object by passing the string "ar-EG" to a <see cref="T:System.Globalization.CultureInfo" /> constructor. If you pass the <see cref="P:System.Globalization.CultureInfo.LCID" /> property of that <see cref="T:System.Globalization.CultureInfo" /> object along with <see cref="F:System.Drawing.StringDigitSubstitute.Traditional" /> to the <see cref="M:System.Drawing.StringFormat.SetDigitSubstitution(System.Int32,System.Drawing.StringDigitSubstitute)" /> method, then Arabic-Indic digits will be substituted for western digits at display time. </param>
		/// <param name="substitute">An element of the <see cref="T:System.Drawing.StringDigitSubstitute" /> enumeration that specifies how digits are displayed. </param>
		// Token: 0x060006CF RID: 1743 RVA: 0x0001BC40 File Offset: 0x00019E40
		public void SetDigitSubstitution(int language, StringDigitSubstitute substitute)
		{
			int num = SafeNativeMethods.Gdip.GdipSetStringFormatDigitSubstitution(new HandleRef(this, this.nativeFormat), language, substitute);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Gets the method to be used for digit substitution.</summary>
		/// <returns>A <see cref="T:System.Drawing.StringDigitSubstitute" /> enumeration value that specifies how to substitute characters in a string that cannot be displayed because they are not supported by the current font.</returns>
		// Token: 0x170002BD RID: 701
		// (get) Token: 0x060006D0 RID: 1744 RVA: 0x0001BC6C File Offset: 0x00019E6C
		public StringDigitSubstitute DigitSubstitutionMethod
		{
			get
			{
				int num = 0;
				StringDigitSubstitute result;
				int num2 = SafeNativeMethods.Gdip.GdipGetStringFormatDigitSubstitution(new HandleRef(this, this.nativeFormat), out num, out result);
				if (num2 != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num2);
				}
				return result;
			}
		}

		/// <summary>Gets the language that is used when local digits are substituted for western digits.</summary>
		/// <returns>A National Language Support (NLS) language identifier that identifies the language that will be used when local digits are substituted for western digits. You can pass the <see cref="P:System.Globalization.CultureInfo.LCID" /> property of a <see cref="T:System.Globalization.CultureInfo" /> object as the NLS language identifier. For example, suppose you create a <see cref="T:System.Globalization.CultureInfo" /> object by passing the string "ar-EG" to a <see cref="T:System.Globalization.CultureInfo" /> constructor. If you pass the <see cref="P:System.Globalization.CultureInfo.LCID" /> property of that <see cref="T:System.Globalization.CultureInfo" /> object along with.<see cref="F:System.Drawing.StringDigitSubstitute.Traditional" /> to the <see cref="M:System.Drawing.StringFormat.SetDigitSubstitution(System.Int32,System.Drawing.StringDigitSubstitute)" /> method, then Arabic-Indic digits will be substituted for western digits at display time.</returns>
		// Token: 0x170002BE RID: 702
		// (get) Token: 0x060006D1 RID: 1745 RVA: 0x0001BC9C File Offset: 0x00019E9C
		public int DigitSubstitutionLanguage
		{
			get
			{
				int result = 0;
				StringDigitSubstitute stringDigitSubstitute;
				int num = SafeNativeMethods.Gdip.GdipGetStringFormatDigitSubstitution(new HandleRef(this, this.nativeFormat), out result, out stringDigitSubstitute);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return result;
			}
		}

		/// <summary>Allows an object to try to free resources and perform other cleanup operations before it is reclaimed by garbage collection.</summary>
		// Token: 0x060006D2 RID: 1746 RVA: 0x0001BCCC File Offset: 0x00019ECC
		~StringFormat()
		{
			this.Dispose(false);
		}

		/// <summary>Converts this <see cref="T:System.Drawing.StringFormat" /> object to a human-readable string.</summary>
		/// <returns>A string representation of this <see cref="T:System.Drawing.StringFormat" /> object.</returns>
		// Token: 0x060006D3 RID: 1747 RVA: 0x0001BCFC File Offset: 0x00019EFC
		public override string ToString()
		{
			return "[StringFormat, FormatFlags=" + this.FormatFlags.ToString() + "]";
		}

		// Token: 0x04000582 RID: 1410
		internal IntPtr nativeFormat;
	}
}
