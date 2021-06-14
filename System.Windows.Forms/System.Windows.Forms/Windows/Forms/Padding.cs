using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;

namespace System.Windows.Forms
{
	/// <summary>Represents padding or margin information associated with a user interface (UI) element.</summary>
	// Token: 0x02000305 RID: 773
	[TypeConverter(typeof(PaddingConverter))]
	[Serializable]
	public struct Padding
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.Padding" /> class using the supplied padding size for all edges.</summary>
		/// <param name="all">The number of pixels to be used for padding for all edges.</param>
		// Token: 0x06002ECC RID: 11980 RVA: 0x000D93F0 File Offset: 0x000D75F0
		public Padding(int all)
		{
			this._all = true;
			this._bottom = all;
			this._right = all;
			this._left = all;
			this._top = all;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.Padding" /> class using a separate padding size for each edge.</summary>
		/// <param name="left">The padding size, in pixels, for the left edge.</param>
		/// <param name="top">The padding size, in pixels, for the top edge.</param>
		/// <param name="right">The padding size, in pixels, for the right edge.</param>
		/// <param name="bottom">The padding size, in pixels, for the bottom edge.</param>
		// Token: 0x06002ECD RID: 11981 RVA: 0x000D9428 File Offset: 0x000D7628
		public Padding(int left, int top, int right, int bottom)
		{
			this._top = top;
			this._left = left;
			this._right = right;
			this._bottom = bottom;
			this._all = (this._top == this._left && this._top == this._right && this._top == this._bottom);
		}

		/// <summary>Gets or sets the padding value for all the edges.</summary>
		/// <returns>The padding, in pixels, for all edges if the same; otherwise, -1.</returns>
		// Token: 0x17000B4B RID: 2891
		// (get) Token: 0x06002ECE RID: 11982 RVA: 0x000D9485 File Offset: 0x000D7685
		// (set) Token: 0x06002ECF RID: 11983 RVA: 0x000D9498 File Offset: 0x000D7698
		[RefreshProperties(RefreshProperties.All)]
		public int All
		{
			get
			{
				if (!this._all)
				{
					return -1;
				}
				return this._top;
			}
			set
			{
				if (!this._all || this._top != value)
				{
					this._all = true;
					this._bottom = value;
					this._right = value;
					this._left = value;
					this._top = value;
				}
			}
		}

		/// <summary>Gets or sets the padding value for the bottom edge.</summary>
		/// <returns>The padding, in pixels, for the bottom edge.</returns>
		// Token: 0x17000B4C RID: 2892
		// (get) Token: 0x06002ED0 RID: 11984 RVA: 0x000D94DF File Offset: 0x000D76DF
		// (set) Token: 0x06002ED1 RID: 11985 RVA: 0x000D94F6 File Offset: 0x000D76F6
		[RefreshProperties(RefreshProperties.All)]
		public int Bottom
		{
			get
			{
				if (this._all)
				{
					return this._top;
				}
				return this._bottom;
			}
			set
			{
				if (this._all || this._bottom != value)
				{
					this._all = false;
					this._bottom = value;
				}
			}
		}

		/// <summary>Gets or sets the padding value for the left edge.</summary>
		/// <returns>The padding, in pixels, for the left edge.</returns>
		// Token: 0x17000B4D RID: 2893
		// (get) Token: 0x06002ED2 RID: 11986 RVA: 0x000D9517 File Offset: 0x000D7717
		// (set) Token: 0x06002ED3 RID: 11987 RVA: 0x000D952E File Offset: 0x000D772E
		[RefreshProperties(RefreshProperties.All)]
		public int Left
		{
			get
			{
				if (this._all)
				{
					return this._top;
				}
				return this._left;
			}
			set
			{
				if (this._all || this._left != value)
				{
					this._all = false;
					this._left = value;
				}
			}
		}

		/// <summary>Gets or sets the padding value for the right edge.</summary>
		/// <returns>The padding, in pixels, for the right edge.</returns>
		// Token: 0x17000B4E RID: 2894
		// (get) Token: 0x06002ED4 RID: 11988 RVA: 0x000D954F File Offset: 0x000D774F
		// (set) Token: 0x06002ED5 RID: 11989 RVA: 0x000D9566 File Offset: 0x000D7766
		[RefreshProperties(RefreshProperties.All)]
		public int Right
		{
			get
			{
				if (this._all)
				{
					return this._top;
				}
				return this._right;
			}
			set
			{
				if (this._all || this._right != value)
				{
					this._all = false;
					this._right = value;
				}
			}
		}

		/// <summary>Gets or sets the padding value for the top edge.</summary>
		/// <returns>The padding, in pixels, for the top edge.</returns>
		// Token: 0x17000B4F RID: 2895
		// (get) Token: 0x06002ED6 RID: 11990 RVA: 0x000D9587 File Offset: 0x000D7787
		// (set) Token: 0x06002ED7 RID: 11991 RVA: 0x000D958F File Offset: 0x000D778F
		[RefreshProperties(RefreshProperties.All)]
		public int Top
		{
			get
			{
				return this._top;
			}
			set
			{
				if (this._all || this._top != value)
				{
					this._all = false;
					this._top = value;
				}
			}
		}

		/// <summary>Gets the combined padding for the right and left edges.</summary>
		/// <returns>Gets the sum, in pixels, of the <see cref="P:System.Windows.Forms.Padding.Left" /> and <see cref="P:System.Windows.Forms.Padding.Right" /> padding values.</returns>
		// Token: 0x17000B50 RID: 2896
		// (get) Token: 0x06002ED8 RID: 11992 RVA: 0x000D95B0 File Offset: 0x000D77B0
		[Browsable(false)]
		public int Horizontal
		{
			get
			{
				return this.Left + this.Right;
			}
		}

		/// <summary>Gets the combined padding for the top and bottom edges.</summary>
		/// <returns>Gets the sum, in pixels, of the <see cref="P:System.Windows.Forms.Padding.Top" /> and <see cref="P:System.Windows.Forms.Padding.Bottom" /> padding values.</returns>
		// Token: 0x17000B51 RID: 2897
		// (get) Token: 0x06002ED9 RID: 11993 RVA: 0x000D95BF File Offset: 0x000D77BF
		[Browsable(false)]
		public int Vertical
		{
			get
			{
				return this.Top + this.Bottom;
			}
		}

		/// <summary>Gets the padding information in the form of a <see cref="T:System.Drawing.Size" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Size" /> containing the padding information.</returns>
		// Token: 0x17000B52 RID: 2898
		// (get) Token: 0x06002EDA RID: 11994 RVA: 0x000D95CE File Offset: 0x000D77CE
		[Browsable(false)]
		public Size Size
		{
			get
			{
				return new Size(this.Horizontal, this.Vertical);
			}
		}

		/// <summary>Computes the sum of the two specified <see cref="T:System.Windows.Forms.Padding" /> values.</summary>
		/// <param name="p1">A <see cref="T:System.Windows.Forms.Padding" />.</param>
		/// <param name="p2">A <see cref="T:System.Windows.Forms.Padding" />.</param>
		/// <returns>A <see cref="T:System.Windows.Forms.Padding" /> that contains the sum of the two specified <see cref="T:System.Windows.Forms.Padding" /> values.</returns>
		// Token: 0x06002EDB RID: 11995 RVA: 0x000D95E1 File Offset: 0x000D77E1
		public static Padding Add(Padding p1, Padding p2)
		{
			return p1 + p2;
		}

		/// <summary>Subtracts one specified <see cref="T:System.Windows.Forms.Padding" /> value from another.</summary>
		/// <param name="p1">A <see cref="T:System.Windows.Forms.Padding" />.</param>
		/// <param name="p2">A <see cref="T:System.Windows.Forms.Padding" />.</param>
		/// <returns>A <see cref="T:System.Windows.Forms.Padding" /> that contains the result of the subtraction of one specified <see cref="T:System.Windows.Forms.Padding" /> value from another.</returns>
		// Token: 0x06002EDC RID: 11996 RVA: 0x000D95EA File Offset: 0x000D77EA
		public static Padding Subtract(Padding p1, Padding p2)
		{
			return p1 - p2;
		}

		/// <summary>Determines whether the value of the specified object is equivalent to the current <see cref="T:System.Windows.Forms.Padding" />.</summary>
		/// <param name="other">The object to compare to the current <see cref="T:System.Windows.Forms.Padding" />.</param>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Forms.Padding" /> objects are equivalent; otherwise, <see langword="false" />.</returns>
		// Token: 0x06002EDD RID: 11997 RVA: 0x000D95F3 File Offset: 0x000D77F3
		public override bool Equals(object other)
		{
			return other is Padding && (Padding)other == this;
		}

		/// <summary>Performs vector addition on the two specified <see cref="T:System.Windows.Forms.Padding" /> objects, resulting in a new <see cref="T:System.Windows.Forms.Padding" />.</summary>
		/// <param name="p1">The first <see cref="T:System.Windows.Forms.Padding" /> to add.</param>
		/// <param name="p2">The second <see cref="T:System.Windows.Forms.Padding" /> to add.</param>
		/// <returns>A new <see cref="T:System.Windows.Forms.Padding" /> that results from adding <paramref name="p1" /> and <paramref name="p2" />.</returns>
		// Token: 0x06002EDE RID: 11998 RVA: 0x000D9610 File Offset: 0x000D7810
		public static Padding operator +(Padding p1, Padding p2)
		{
			return new Padding(p1.Left + p2.Left, p1.Top + p2.Top, p1.Right + p2.Right, p1.Bottom + p2.Bottom);
		}

		/// <summary>Performs vector subtraction on the two specified <see cref="T:System.Windows.Forms.Padding" /> objects, resulting in a new <see cref="T:System.Windows.Forms.Padding" />.</summary>
		/// <param name="p1">The <see cref="T:System.Windows.Forms.Padding" /> to subtract from (the minuend).</param>
		/// <param name="p2">The <see cref="T:System.Windows.Forms.Padding" /> to subtract from (the subtrahend).</param>
		/// <returns>The <see cref="T:System.Windows.Forms.Padding" /> result of subtracting <paramref name="p2" /> from <paramref name="p1" />.</returns>
		// Token: 0x06002EDF RID: 11999 RVA: 0x000D9660 File Offset: 0x000D7860
		public static Padding operator -(Padding p1, Padding p2)
		{
			return new Padding(p1.Left - p2.Left, p1.Top - p2.Top, p1.Right - p2.Right, p1.Bottom - p2.Bottom);
		}

		/// <summary>Tests whether two specified <see cref="T:System.Windows.Forms.Padding" /> objects are equivalent.</summary>
		/// <param name="p1">A <see cref="T:System.Windows.Forms.Padding" /> to test.</param>
		/// <param name="p2">A <see cref="T:System.Windows.Forms.Padding" /> to test.</param>
		/// <returns>
		///     <see langword="true" /> if the two <see cref="T:System.Windows.Forms.Padding" /> objects are equal; otherwise, <see langword="false" />.</returns>
		// Token: 0x06002EE0 RID: 12000 RVA: 0x000D96B0 File Offset: 0x000D78B0
		public static bool operator ==(Padding p1, Padding p2)
		{
			return p1.Left == p2.Left && p1.Top == p2.Top && p1.Right == p2.Right && p1.Bottom == p2.Bottom;
		}

		/// <summary>Tests whether two specified <see cref="T:System.Windows.Forms.Padding" /> objects are not equivalent.</summary>
		/// <param name="p1">A <see cref="T:System.Windows.Forms.Padding" /> to test.</param>
		/// <param name="p2">A <see cref="T:System.Windows.Forms.Padding" /> to test.</param>
		/// <returns>
		///     <see langword="true" /> if the two <see cref="T:System.Windows.Forms.Padding" /> objects are different; otherwise, <see langword="false" />.</returns>
		// Token: 0x06002EE1 RID: 12001 RVA: 0x000D96FF File Offset: 0x000D78FF
		public static bool operator !=(Padding p1, Padding p2)
		{
			return !(p1 == p2);
		}

		/// <summary>Generates a hash code for the current <see cref="T:System.Windows.Forms.Padding" />. </summary>
		/// <returns>A 32-bit signed integer hash code.</returns>
		// Token: 0x06002EE2 RID: 12002 RVA: 0x000D970B File Offset: 0x000D790B
		public override int GetHashCode()
		{
			return this.Left ^ WindowsFormsUtils.RotateLeft(this.Top, 8) ^ WindowsFormsUtils.RotateLeft(this.Right, 16) ^ WindowsFormsUtils.RotateLeft(this.Bottom, 24);
		}

		/// <summary>Returns a string that represents the current <see cref="T:System.Windows.Forms.Padding" />.</summary>
		/// <returns>A <see cref="T:System.String" /> that represents the current <see cref="T:System.Windows.Forms.Padding" />.</returns>
		// Token: 0x06002EE3 RID: 12003 RVA: 0x000D973C File Offset: 0x000D793C
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"{Left=",
				this.Left.ToString(CultureInfo.CurrentCulture),
				",Top=",
				this.Top.ToString(CultureInfo.CurrentCulture),
				",Right=",
				this.Right.ToString(CultureInfo.CurrentCulture),
				",Bottom=",
				this.Bottom.ToString(CultureInfo.CurrentCulture),
				"}"
			});
		}

		// Token: 0x06002EE4 RID: 12004 RVA: 0x000D97D5 File Offset: 0x000D79D5
		private void ResetAll()
		{
			this.All = 0;
		}

		// Token: 0x06002EE5 RID: 12005 RVA: 0x000D97DE File Offset: 0x000D79DE
		private void ResetBottom()
		{
			this.Bottom = 0;
		}

		// Token: 0x06002EE6 RID: 12006 RVA: 0x000D97E7 File Offset: 0x000D79E7
		private void ResetLeft()
		{
			this.Left = 0;
		}

		// Token: 0x06002EE7 RID: 12007 RVA: 0x000D97F0 File Offset: 0x000D79F0
		private void ResetRight()
		{
			this.Right = 0;
		}

		// Token: 0x06002EE8 RID: 12008 RVA: 0x000D97F9 File Offset: 0x000D79F9
		private void ResetTop()
		{
			this.Top = 0;
		}

		// Token: 0x06002EE9 RID: 12009 RVA: 0x000D9804 File Offset: 0x000D7A04
		internal void Scale(float dx, float dy)
		{
			this._top = (int)((float)this._top * dy);
			this._left = (int)((float)this._left * dx);
			this._right = (int)((float)this._right * dx);
			this._bottom = (int)((float)this._bottom * dy);
		}

		// Token: 0x06002EEA RID: 12010 RVA: 0x000D9851 File Offset: 0x000D7A51
		internal bool ShouldSerializeAll()
		{
			return this._all;
		}

		// Token: 0x06002EEB RID: 12011 RVA: 0x000D9859 File Offset: 0x000D7A59
		[Conditional("DEBUG")]
		private void Debug_SanityCheck()
		{
			bool all = this._all;
		}

		// Token: 0x04001D4A RID: 7498
		private bool _all;

		// Token: 0x04001D4B RID: 7499
		private int _top;

		// Token: 0x04001D4C RID: 7500
		private int _left;

		// Token: 0x04001D4D RID: 7501
		private int _right;

		// Token: 0x04001D4E RID: 7502
		private int _bottom;

		/// <summary>Provides a <see cref="T:System.Windows.Forms.Padding" /> object with no padding.</summary>
		// Token: 0x04001D4F RID: 7503
		public static readonly Padding Empty = new Padding(0);
	}
}
