using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Drawing
{
	/// <summary>Stores an ordered pair of floating-point numbers, typically the width and height of a rectangle.</summary>
	// Token: 0x02000042 RID: 66
	[ComVisible(true)]
	[TypeConverter(typeof(SizeFConverter))]
	[Serializable]
	public struct SizeF
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.SizeF" /> structure from the specified existing <see cref="T:System.Drawing.SizeF" /> structure.</summary>
		/// <param name="size">The <see cref="T:System.Drawing.SizeF" /> structure from which to create the new <see cref="T:System.Drawing.SizeF" /> structure. </param>
		// Token: 0x06000691 RID: 1681 RVA: 0x0001B209 File Offset: 0x00019409
		public SizeF(SizeF size)
		{
			this.width = size.width;
			this.height = size.height;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.SizeF" /> structure from the specified <see cref="T:System.Drawing.PointF" /> structure.</summary>
		/// <param name="pt">The <see cref="T:System.Drawing.PointF" /> structure from which to initialize this <see cref="T:System.Drawing.SizeF" /> structure. </param>
		// Token: 0x06000692 RID: 1682 RVA: 0x0001B223 File Offset: 0x00019423
		public SizeF(PointF pt)
		{
			this.width = pt.X;
			this.height = pt.Y;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.SizeF" /> structure from the specified dimensions.</summary>
		/// <param name="width">The width component of the new <see cref="T:System.Drawing.SizeF" /> structure. </param>
		/// <param name="height">The height component of the new <see cref="T:System.Drawing.SizeF" /> structure. </param>
		// Token: 0x06000693 RID: 1683 RVA: 0x0001B23F File Offset: 0x0001943F
		public SizeF(float width, float height)
		{
			this.width = width;
			this.height = height;
		}

		/// <summary>Adds the width and height of one <see cref="T:System.Drawing.SizeF" /> structure to the width and height of another <see cref="T:System.Drawing.SizeF" /> structure.</summary>
		/// <param name="sz1">The first <see cref="T:System.Drawing.SizeF" /> structure to add. </param>
		/// <param name="sz2">The second <see cref="T:System.Drawing.SizeF" /> structure to add. </param>
		/// <returns>A <see cref="T:System.Drawing.Size" /> structure that is the result of the addition operation.</returns>
		// Token: 0x06000694 RID: 1684 RVA: 0x0001B24F File Offset: 0x0001944F
		public static SizeF operator +(SizeF sz1, SizeF sz2)
		{
			return SizeF.Add(sz1, sz2);
		}

		/// <summary>Subtracts the width and height of one <see cref="T:System.Drawing.SizeF" /> structure from the width and height of another <see cref="T:System.Drawing.SizeF" /> structure.</summary>
		/// <param name="sz1">The <see cref="T:System.Drawing.SizeF" /> structure on the left side of the subtraction operator. </param>
		/// <param name="sz2">The <see cref="T:System.Drawing.SizeF" /> structure on the right side of the subtraction operator. </param>
		/// <returns>A <see cref="T:System.Drawing.SizeF" /> that is the result of the subtraction operation.</returns>
		// Token: 0x06000695 RID: 1685 RVA: 0x0001B258 File Offset: 0x00019458
		public static SizeF operator -(SizeF sz1, SizeF sz2)
		{
			return SizeF.Subtract(sz1, sz2);
		}

		/// <summary>Tests whether two <see cref="T:System.Drawing.SizeF" /> structures are equal.</summary>
		/// <param name="sz1">The <see cref="T:System.Drawing.SizeF" /> structure on the left side of the equality operator. </param>
		/// <param name="sz2">The <see cref="T:System.Drawing.SizeF" /> structure on the right of the equality operator. </param>
		/// <returns>This operator returns <see langword="true" /> if <paramref name="sz1" /> and <paramref name="sz2" /> have equal width and height; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000696 RID: 1686 RVA: 0x0001B261 File Offset: 0x00019461
		public static bool operator ==(SizeF sz1, SizeF sz2)
		{
			return sz1.Width == sz2.Width && sz1.Height == sz2.Height;
		}

		/// <summary>Tests whether two <see cref="T:System.Drawing.SizeF" /> structures are different.</summary>
		/// <param name="sz1">The <see cref="T:System.Drawing.SizeF" /> structure on the left of the inequality operator. </param>
		/// <param name="sz2">The <see cref="T:System.Drawing.SizeF" /> structure on the right of the inequality operator. </param>
		/// <returns>This operator returns <see langword="true" /> if <paramref name="sz1" /> and <paramref name="sz2" /> differ either in width or height; <see langword="false" /> if <paramref name="sz1" /> and <paramref name="sz2" /> are equal.</returns>
		// Token: 0x06000697 RID: 1687 RVA: 0x0001B285 File Offset: 0x00019485
		public static bool operator !=(SizeF sz1, SizeF sz2)
		{
			return !(sz1 == sz2);
		}

		/// <summary>Converts the specified <see cref="T:System.Drawing.SizeF" /> structure to a <see cref="T:System.Drawing.PointF" /> structure.</summary>
		/// <param name="size">The <see cref="T:System.Drawing.SizeF" /> structure to be converted</param>
		/// <returns>The <see cref="T:System.Drawing.PointF" /> structure to which this operator converts.</returns>
		// Token: 0x06000698 RID: 1688 RVA: 0x0001B291 File Offset: 0x00019491
		public static explicit operator PointF(SizeF size)
		{
			return new PointF(size.Width, size.Height);
		}

		/// <summary>Gets a value that indicates whether this <see cref="T:System.Drawing.SizeF" /> structure has zero width and height.</summary>
		/// <returns>This property returns <see langword="true" /> when this <see cref="T:System.Drawing.SizeF" /> structure has both a width and height of zero; otherwise, <see langword="false" />.</returns>
		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x06000699 RID: 1689 RVA: 0x0001B2A6 File Offset: 0x000194A6
		[Browsable(false)]
		public bool IsEmpty
		{
			get
			{
				return this.width == 0f && this.height == 0f;
			}
		}

		/// <summary>Gets or sets the horizontal component of this <see cref="T:System.Drawing.SizeF" /> structure.</summary>
		/// <returns>The horizontal component of this <see cref="T:System.Drawing.SizeF" /> structure, typically measured in pixels.</returns>
		// Token: 0x170002B2 RID: 690
		// (get) Token: 0x0600069A RID: 1690 RVA: 0x0001B2C4 File Offset: 0x000194C4
		// (set) Token: 0x0600069B RID: 1691 RVA: 0x0001B2CC File Offset: 0x000194CC
		public float Width
		{
			get
			{
				return this.width;
			}
			set
			{
				this.width = value;
			}
		}

		/// <summary>Gets or sets the vertical component of this <see cref="T:System.Drawing.SizeF" /> structure.</summary>
		/// <returns>The vertical component of this <see cref="T:System.Drawing.SizeF" /> structure, typically measured in pixels.</returns>
		// Token: 0x170002B3 RID: 691
		// (get) Token: 0x0600069C RID: 1692 RVA: 0x0001B2D5 File Offset: 0x000194D5
		// (set) Token: 0x0600069D RID: 1693 RVA: 0x0001B2DD File Offset: 0x000194DD
		public float Height
		{
			get
			{
				return this.height;
			}
			set
			{
				this.height = value;
			}
		}

		/// <summary>Adds the width and height of one <see cref="T:System.Drawing.SizeF" /> structure to the width and height of another <see cref="T:System.Drawing.SizeF" /> structure.</summary>
		/// <param name="sz1">The first <see cref="T:System.Drawing.SizeF" /> structure to add.</param>
		/// <param name="sz2">The second <see cref="T:System.Drawing.SizeF" /> structure to add.</param>
		/// <returns>A <see cref="T:System.Drawing.SizeF" /> structure that is the result of the addition operation.</returns>
		// Token: 0x0600069E RID: 1694 RVA: 0x0001B2E6 File Offset: 0x000194E6
		public static SizeF Add(SizeF sz1, SizeF sz2)
		{
			return new SizeF(sz1.Width + sz2.Width, sz1.Height + sz2.Height);
		}

		/// <summary>Subtracts the width and height of one <see cref="T:System.Drawing.SizeF" /> structure from the width and height of another <see cref="T:System.Drawing.SizeF" /> structure.</summary>
		/// <param name="sz1">The <see cref="T:System.Drawing.SizeF" /> structure on the left side of the subtraction operator. </param>
		/// <param name="sz2">The <see cref="T:System.Drawing.SizeF" /> structure on the right side of the subtraction operator. </param>
		/// <returns>A <see cref="T:System.Drawing.SizeF" /> structure that is a result of the subtraction operation.</returns>
		// Token: 0x0600069F RID: 1695 RVA: 0x0001B30B File Offset: 0x0001950B
		public static SizeF Subtract(SizeF sz1, SizeF sz2)
		{
			return new SizeF(sz1.Width - sz2.Width, sz1.Height - sz2.Height);
		}

		/// <summary>Tests to see whether the specified object is a <see cref="T:System.Drawing.SizeF" /> structure with the same dimensions as this <see cref="T:System.Drawing.SizeF" /> structure.</summary>
		/// <param name="obj">The <see cref="T:System.Object" /> to test. </param>
		/// <returns>This method returns <see langword="true" /> if <paramref name="obj" /> is a <see cref="T:System.Drawing.SizeF" /> and has the same width and height as this <see cref="T:System.Drawing.SizeF" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x060006A0 RID: 1696 RVA: 0x0001B330 File Offset: 0x00019530
		public override bool Equals(object obj)
		{
			if (!(obj is SizeF))
			{
				return false;
			}
			SizeF sizeF = (SizeF)obj;
			return sizeF.Width == this.Width && sizeF.Height == this.Height && sizeF.GetType().Equals(base.GetType());
		}

		/// <summary>Returns a hash code for this <see cref="T:System.Drawing.Size" /> structure.</summary>
		/// <returns>An integer value that specifies a hash value for this <see cref="T:System.Drawing.Size" /> structure.</returns>
		// Token: 0x060006A1 RID: 1697 RVA: 0x0001B38E File Offset: 0x0001958E
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		/// <summary>Converts a <see cref="T:System.Drawing.SizeF" /> structure to a <see cref="T:System.Drawing.PointF" /> structure.</summary>
		/// <returns>Returns a <see cref="T:System.Drawing.PointF" /> structure.</returns>
		// Token: 0x060006A2 RID: 1698 RVA: 0x0001B3A0 File Offset: 0x000195A0
		public PointF ToPointF()
		{
			return (PointF)this;
		}

		/// <summary>Converts a <see cref="T:System.Drawing.SizeF" /> structure to a <see cref="T:System.Drawing.Size" /> structure.</summary>
		/// <returns>Returns a <see cref="T:System.Drawing.Size" /> structure.</returns>
		// Token: 0x060006A3 RID: 1699 RVA: 0x0001B3AD File Offset: 0x000195AD
		public Size ToSize()
		{
			return Size.Truncate(this);
		}

		/// <summary>Creates a human-readable string that represents this <see cref="T:System.Drawing.SizeF" /> structure.</summary>
		/// <returns>A string that represents this <see cref="T:System.Drawing.SizeF" /> structure.</returns>
		// Token: 0x060006A4 RID: 1700 RVA: 0x0001B3BC File Offset: 0x000195BC
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"{Width=",
				this.width.ToString(CultureInfo.CurrentCulture),
				", Height=",
				this.height.ToString(CultureInfo.CurrentCulture),
				"}"
			});
		}

		/// <summary>Gets a <see cref="T:System.Drawing.SizeF" /> structure that has a <see cref="P:System.Drawing.SizeF.Height" /> and <see cref="P:System.Drawing.SizeF.Width" /> value of 0. </summary>
		/// <returns>A <see cref="T:System.Drawing.SizeF" /> structure that has a <see cref="P:System.Drawing.SizeF.Height" /> and <see cref="P:System.Drawing.SizeF.Width" /> value of 0.</returns>
		// Token: 0x04000574 RID: 1396
		public static readonly SizeF Empty;

		// Token: 0x04000575 RID: 1397
		private float width;

		// Token: 0x04000576 RID: 1398
		private float height;
	}
}
