using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.Serialization;

namespace System.Drawing.Printing
{
	/// <summary>Specifies the dimensions of the margins of a printed page.</summary>
	// Token: 0x02000055 RID: 85
	[TypeConverter(typeof(MarginsConverter))]
	[Serializable]
	public class Margins : ICloneable
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Printing.Margins" /> class with 1-inch wide margins.</summary>
		// Token: 0x0600070B RID: 1803 RVA: 0x0001CA84 File Offset: 0x0001AC84
		public Margins() : this(100, 100, 100, 100)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Printing.Margins" /> class with the specified left, right, top, and bottom margins.</summary>
		/// <param name="left">The left margin, in hundredths of an inch. </param>
		/// <param name="right">The right margin, in hundredths of an inch. </param>
		/// <param name="top">The top margin, in hundredths of an inch. </param>
		/// <param name="bottom">The bottom margin, in hundredths of an inch. </param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="left" /> parameter value is less than 0.-or- The <paramref name="right" /> parameter value is less than 0.-or- The <paramref name="top" /> parameter value is less than 0.-or- The <paramref name="bottom" /> parameter value is less than 0. </exception>
		// Token: 0x0600070C RID: 1804 RVA: 0x0001CA94 File Offset: 0x0001AC94
		public Margins(int left, int right, int top, int bottom)
		{
			this.CheckMargin(left, "left");
			this.CheckMargin(right, "right");
			this.CheckMargin(top, "top");
			this.CheckMargin(bottom, "bottom");
			this.left = left;
			this.right = right;
			this.top = top;
			this.bottom = bottom;
			this.doubleLeft = (double)left;
			this.doubleRight = (double)right;
			this.doubleTop = (double)top;
			this.doubleBottom = (double)bottom;
		}

		/// <summary>Gets or sets the left margin width, in hundredths of an inch.</summary>
		/// <returns>The left margin width, in hundredths of an inch.</returns>
		/// <exception cref="T:System.ArgumentException">The <see cref="P:System.Drawing.Printing.Margins.Left" /> property is set to a value that is less than 0. </exception>
		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x0600070D RID: 1805 RVA: 0x0001CB16 File Offset: 0x0001AD16
		// (set) Token: 0x0600070E RID: 1806 RVA: 0x0001CB1E File Offset: 0x0001AD1E
		public int Left
		{
			get
			{
				return this.left;
			}
			set
			{
				this.CheckMargin(value, "Left");
				this.left = value;
				this.doubleLeft = (double)value;
			}
		}

		/// <summary>Gets or sets the right margin width, in hundredths of an inch.</summary>
		/// <returns>The right margin width, in hundredths of an inch.</returns>
		/// <exception cref="T:System.ArgumentException">The <see cref="P:System.Drawing.Printing.Margins.Right" /> property is set to a value that is less than 0. </exception>
		// Token: 0x170002C7 RID: 711
		// (get) Token: 0x0600070F RID: 1807 RVA: 0x0001CB3B File Offset: 0x0001AD3B
		// (set) Token: 0x06000710 RID: 1808 RVA: 0x0001CB43 File Offset: 0x0001AD43
		public int Right
		{
			get
			{
				return this.right;
			}
			set
			{
				this.CheckMargin(value, "Right");
				this.right = value;
				this.doubleRight = (double)value;
			}
		}

		/// <summary>Gets or sets the top margin width, in hundredths of an inch.</summary>
		/// <returns>The top margin width, in hundredths of an inch.</returns>
		/// <exception cref="T:System.ArgumentException">The <see cref="P:System.Drawing.Printing.Margins.Top" /> property is set to a value that is less than 0. </exception>
		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x06000711 RID: 1809 RVA: 0x0001CB60 File Offset: 0x0001AD60
		// (set) Token: 0x06000712 RID: 1810 RVA: 0x0001CB68 File Offset: 0x0001AD68
		public int Top
		{
			get
			{
				return this.top;
			}
			set
			{
				this.CheckMargin(value, "Top");
				this.top = value;
				this.doubleTop = (double)value;
			}
		}

		/// <summary>Gets or sets the bottom margin, in hundredths of an inch.</summary>
		/// <returns>The bottom margin, in hundredths of an inch.</returns>
		/// <exception cref="T:System.ArgumentException">The <see cref="P:System.Drawing.Printing.Margins.Bottom" /> property is set to a value that is less than 0. </exception>
		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x06000713 RID: 1811 RVA: 0x0001CB85 File Offset: 0x0001AD85
		// (set) Token: 0x06000714 RID: 1812 RVA: 0x0001CB8D File Offset: 0x0001AD8D
		public int Bottom
		{
			get
			{
				return this.bottom;
			}
			set
			{
				this.CheckMargin(value, "Bottom");
				this.bottom = value;
				this.doubleBottom = (double)value;
			}
		}

		// Token: 0x170002CA RID: 714
		// (get) Token: 0x06000715 RID: 1813 RVA: 0x0001CBAA File Offset: 0x0001ADAA
		// (set) Token: 0x06000716 RID: 1814 RVA: 0x0001CBB2 File Offset: 0x0001ADB2
		internal double DoubleLeft
		{
			get
			{
				return this.doubleLeft;
			}
			set
			{
				this.Left = (int)Math.Round(value);
				this.doubleLeft = value;
			}
		}

		// Token: 0x170002CB RID: 715
		// (get) Token: 0x06000717 RID: 1815 RVA: 0x0001CBC8 File Offset: 0x0001ADC8
		// (set) Token: 0x06000718 RID: 1816 RVA: 0x0001CBD0 File Offset: 0x0001ADD0
		internal double DoubleRight
		{
			get
			{
				return this.doubleRight;
			}
			set
			{
				this.Right = (int)Math.Round(value);
				this.doubleRight = value;
			}
		}

		// Token: 0x170002CC RID: 716
		// (get) Token: 0x06000719 RID: 1817 RVA: 0x0001CBE6 File Offset: 0x0001ADE6
		// (set) Token: 0x0600071A RID: 1818 RVA: 0x0001CBEE File Offset: 0x0001ADEE
		internal double DoubleTop
		{
			get
			{
				return this.doubleTop;
			}
			set
			{
				this.Top = (int)Math.Round(value);
				this.doubleTop = value;
			}
		}

		// Token: 0x170002CD RID: 717
		// (get) Token: 0x0600071B RID: 1819 RVA: 0x0001CC04 File Offset: 0x0001AE04
		// (set) Token: 0x0600071C RID: 1820 RVA: 0x0001CC0C File Offset: 0x0001AE0C
		internal double DoubleBottom
		{
			get
			{
				return this.doubleBottom;
			}
			set
			{
				this.Bottom = (int)Math.Round(value);
				this.doubleBottom = value;
			}
		}

		// Token: 0x0600071D RID: 1821 RVA: 0x0001CC24 File Offset: 0x0001AE24
		[OnDeserialized]
		private void OnDeserializedMethod(StreamingContext context)
		{
			if (this.doubleLeft == 0.0 && this.left != 0)
			{
				this.doubleLeft = (double)this.left;
			}
			if (this.doubleRight == 0.0 && this.right != 0)
			{
				this.doubleRight = (double)this.right;
			}
			if (this.doubleTop == 0.0 && this.top != 0)
			{
				this.doubleTop = (double)this.top;
			}
			if (this.doubleBottom == 0.0 && this.bottom != 0)
			{
				this.doubleBottom = (double)this.bottom;
			}
		}

		// Token: 0x0600071E RID: 1822 RVA: 0x0001CCC9 File Offset: 0x0001AEC9
		private void CheckMargin(int margin, string name)
		{
			if (margin < 0)
			{
				throw new ArgumentException(SR.GetString("InvalidLowBoundArgumentEx", new object[]
				{
					name,
					margin,
					"0"
				}));
			}
		}

		/// <summary>Retrieves a duplicate of this object, member by member.</summary>
		/// <returns>A duplicate of this object.</returns>
		// Token: 0x0600071F RID: 1823 RVA: 0x0001CCFA File Offset: 0x0001AEFA
		public object Clone()
		{
			return base.MemberwiseClone();
		}

		/// <summary>Compares this <see cref="T:System.Drawing.Printing.Margins" /> to the specified <see cref="T:System.Object" /> to determine whether they have the same dimensions.</summary>
		/// <param name="obj">The object to which to compare this <see cref="T:System.Drawing.Printing.Margins" />. </param>
		/// <returns>
		///     <see langword="true" /> if the specified object is a <see cref="T:System.Drawing.Printing.Margins" /> and has the same <see cref="P:System.Drawing.Printing.Margins.Top" />, <see cref="P:System.Drawing.Printing.Margins.Bottom" />, <see cref="P:System.Drawing.Printing.Margins.Right" /> and <see cref="P:System.Drawing.Printing.Margins.Left" /> values as this <see cref="T:System.Drawing.Printing.Margins" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000720 RID: 1824 RVA: 0x0001CD04 File Offset: 0x0001AF04
		public override bool Equals(object obj)
		{
			Margins margins = obj as Margins;
			return margins == this || (!(margins == null) && (margins.Left == this.Left && margins.Right == this.Right && margins.Top == this.Top) && margins.Bottom == this.Bottom);
		}

		/// <summary>Calculates and retrieves a hash code based on the width of the left, right, top, and bottom margins.</summary>
		/// <returns>A hash code based on the left, right, top, and bottom margins.</returns>
		// Token: 0x06000721 RID: 1825 RVA: 0x0001CD68 File Offset: 0x0001AF68
		public override int GetHashCode()
		{
			uint num = (uint)this.Left;
			uint num2 = (uint)this.Right;
			uint num3 = (uint)this.Top;
			uint num4 = (uint)this.Bottom;
			return (int)(num ^ (num2 << 13 | num2 >> 19) ^ (num3 << 26 | num3 >> 6) ^ (num4 << 7 | num4 >> 25));
		}

		/// <summary>Compares two <see cref="T:System.Drawing.Printing.Margins" /> to determine if they have the same dimensions.</summary>
		/// <param name="m1">The first <see cref="T:System.Drawing.Printing.Margins" /> to compare for equality.</param>
		/// <param name="m2">The second <see cref="T:System.Drawing.Printing.Margins" /> to compare for equality.</param>
		/// <returns>
		///     <see langword="true" /> to indicate the <see cref="P:System.Drawing.Printing.Margins.Left" />, <see cref="P:System.Drawing.Printing.Margins.Right" />, <see cref="P:System.Drawing.Printing.Margins.Top" />, and <see cref="P:System.Drawing.Printing.Margins.Bottom" /> properties of both margins have the same value; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000722 RID: 1826 RVA: 0x0001CDB4 File Offset: 0x0001AFB4
		public static bool operator ==(Margins m1, Margins m2)
		{
			return m1 == null == (m2 == null) && (m1 == null || (m1.Left == m2.Left && m1.Top == m2.Top && m1.Right == m2.Right && m1.Bottom == m2.Bottom));
		}

		/// <summary>Compares two <see cref="T:System.Drawing.Printing.Margins" /> to determine whether they are of unequal width.</summary>
		/// <param name="m1">The first <see cref="T:System.Drawing.Printing.Margins" /> to compare for inequality.</param>
		/// <param name="m2">The second <see cref="T:System.Drawing.Printing.Margins" /> to compare for inequality.</param>
		/// <returns>
		///     <see langword="true" /> to indicate if the <see cref="P:System.Drawing.Printing.Margins.Left" />, <see cref="P:System.Drawing.Printing.Margins.Right" />, <see cref="P:System.Drawing.Printing.Margins.Top" />, or <see cref="P:System.Drawing.Printing.Margins.Bottom" /> properties of both margins are not equal; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000723 RID: 1827 RVA: 0x0001CE0C File Offset: 0x0001B00C
		public static bool operator !=(Margins m1, Margins m2)
		{
			return !(m1 == m2);
		}

		/// <summary>Converts the <see cref="T:System.Drawing.Printing.Margins" /> to a string.</summary>
		/// <returns>A <see cref="T:System.String" /> representation of the <see cref="T:System.Drawing.Printing.Margins" />. </returns>
		// Token: 0x06000724 RID: 1828 RVA: 0x0001CE18 File Offset: 0x0001B018
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"[Margins Left=",
				this.Left.ToString(CultureInfo.InvariantCulture),
				" Right=",
				this.Right.ToString(CultureInfo.InvariantCulture),
				" Top=",
				this.Top.ToString(CultureInfo.InvariantCulture),
				" Bottom=",
				this.Bottom.ToString(CultureInfo.InvariantCulture),
				"]"
			});
		}

		// Token: 0x0400060B RID: 1547
		private int left;

		// Token: 0x0400060C RID: 1548
		private int right;

		// Token: 0x0400060D RID: 1549
		private int top;

		// Token: 0x0400060E RID: 1550
		private int bottom;

		// Token: 0x0400060F RID: 1551
		[OptionalField]
		private double doubleLeft;

		// Token: 0x04000610 RID: 1552
		[OptionalField]
		private double doubleRight;

		// Token: 0x04000611 RID: 1553
		[OptionalField]
		private double doubleTop;

		// Token: 0x04000612 RID: 1554
		[OptionalField]
		private double doubleBottom;
	}
}
