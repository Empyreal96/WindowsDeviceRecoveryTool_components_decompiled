using System;
using System.ComponentModel;
using System.Drawing.Internal;
using System.Globalization;

namespace System.Drawing
{
	/// <summary>Stores a set of four floating-point numbers that represent the location and size of a rectangle. For more advanced region functions, use a <see cref="T:System.Drawing.Region" /> object.</summary>
	// Token: 0x02000041 RID: 65
	[Serializable]
	public struct RectangleF
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.RectangleF" /> class with the specified location and size.</summary>
		/// <param name="x">The x-coordinate of the upper-left corner of the rectangle. </param>
		/// <param name="y">The y-coordinate of the upper-left corner of the rectangle. </param>
		/// <param name="width">The width of the rectangle. </param>
		/// <param name="height">The height of the rectangle. </param>
		// Token: 0x06000669 RID: 1641 RVA: 0x0001AB7B File Offset: 0x00018D7B
		public RectangleF(float x, float y, float width, float height)
		{
			this.x = x;
			this.y = y;
			this.width = width;
			this.height = height;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.RectangleF" /> class with the specified location and size.</summary>
		/// <param name="location">A <see cref="T:System.Drawing.PointF" /> that represents the upper-left corner of the rectangular region. </param>
		/// <param name="size">A <see cref="T:System.Drawing.SizeF" /> that represents the width and height of the rectangular region. </param>
		// Token: 0x0600066A RID: 1642 RVA: 0x0001AB9A File Offset: 0x00018D9A
		public RectangleF(PointF location, SizeF size)
		{
			this.x = location.X;
			this.y = location.Y;
			this.width = size.Width;
			this.height = size.Height;
		}

		/// <summary>Creates a <see cref="T:System.Drawing.RectangleF" /> structure with upper-left corner and lower-right corner at the specified locations.</summary>
		/// <param name="left">The x-coordinate of the upper-left corner of the rectangular region. </param>
		/// <param name="top">The y-coordinate of the upper-left corner of the rectangular region. </param>
		/// <param name="right">The x-coordinate of the lower-right corner of the rectangular region. </param>
		/// <param name="bottom">The y-coordinate of the lower-right corner of the rectangular region. </param>
		/// <returns>The new <see cref="T:System.Drawing.RectangleF" /> that this method creates.</returns>
		// Token: 0x0600066B RID: 1643 RVA: 0x0001ABD0 File Offset: 0x00018DD0
		public static RectangleF FromLTRB(float left, float top, float right, float bottom)
		{
			return new RectangleF(left, top, right - left, bottom - top);
		}

		/// <summary>Gets or sets the coordinates of the upper-left corner of this <see cref="T:System.Drawing.RectangleF" /> structure.</summary>
		/// <returns>A <see cref="T:System.Drawing.PointF" /> that represents the upper-left corner of this <see cref="T:System.Drawing.RectangleF" /> structure.</returns>
		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x0600066C RID: 1644 RVA: 0x0001ABDF File Offset: 0x00018DDF
		// (set) Token: 0x0600066D RID: 1645 RVA: 0x0001ABF2 File Offset: 0x00018DF2
		[Browsable(false)]
		public PointF Location
		{
			get
			{
				return new PointF(this.X, this.Y);
			}
			set
			{
				this.X = value.X;
				this.Y = value.Y;
			}
		}

		/// <summary>Gets or sets the size of this <see cref="T:System.Drawing.RectangleF" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.SizeF" /> that represents the width and height of this <see cref="T:System.Drawing.RectangleF" /> structure.</returns>
		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x0600066E RID: 1646 RVA: 0x0001AC0E File Offset: 0x00018E0E
		// (set) Token: 0x0600066F RID: 1647 RVA: 0x0001AC21 File Offset: 0x00018E21
		[Browsable(false)]
		public SizeF Size
		{
			get
			{
				return new SizeF(this.Width, this.Height);
			}
			set
			{
				this.Width = value.Width;
				this.Height = value.Height;
			}
		}

		/// <summary>Gets or sets the x-coordinate of the upper-left corner of this <see cref="T:System.Drawing.RectangleF" /> structure.</summary>
		/// <returns>The x-coordinate of the upper-left corner of this <see cref="T:System.Drawing.RectangleF" /> structure. The default is 0.</returns>
		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x06000670 RID: 1648 RVA: 0x0001AC3D File Offset: 0x00018E3D
		// (set) Token: 0x06000671 RID: 1649 RVA: 0x0001AC45 File Offset: 0x00018E45
		public float X
		{
			get
			{
				return this.x;
			}
			set
			{
				this.x = value;
			}
		}

		/// <summary>Gets or sets the y-coordinate of the upper-left corner of this <see cref="T:System.Drawing.RectangleF" /> structure.</summary>
		/// <returns>The y-coordinate of the upper-left corner of this <see cref="T:System.Drawing.RectangleF" /> structure. The default is 0.</returns>
		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x06000672 RID: 1650 RVA: 0x0001AC4E File Offset: 0x00018E4E
		// (set) Token: 0x06000673 RID: 1651 RVA: 0x0001AC56 File Offset: 0x00018E56
		public float Y
		{
			get
			{
				return this.y;
			}
			set
			{
				this.y = value;
			}
		}

		/// <summary>Gets or sets the width of this <see cref="T:System.Drawing.RectangleF" /> structure.</summary>
		/// <returns>The width of this <see cref="T:System.Drawing.RectangleF" /> structure. The default is 0.</returns>
		// Token: 0x170002AA RID: 682
		// (get) Token: 0x06000674 RID: 1652 RVA: 0x0001AC5F File Offset: 0x00018E5F
		// (set) Token: 0x06000675 RID: 1653 RVA: 0x0001AC67 File Offset: 0x00018E67
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

		/// <summary>Gets or sets the height of this <see cref="T:System.Drawing.RectangleF" /> structure.</summary>
		/// <returns>The height of this <see cref="T:System.Drawing.RectangleF" /> structure. The default is 0.</returns>
		// Token: 0x170002AB RID: 683
		// (get) Token: 0x06000676 RID: 1654 RVA: 0x0001AC70 File Offset: 0x00018E70
		// (set) Token: 0x06000677 RID: 1655 RVA: 0x0001AC78 File Offset: 0x00018E78
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

		/// <summary>Gets the x-coordinate of the left edge of this <see cref="T:System.Drawing.RectangleF" /> structure.</summary>
		/// <returns>The x-coordinate of the left edge of this <see cref="T:System.Drawing.RectangleF" /> structure. </returns>
		// Token: 0x170002AC RID: 684
		// (get) Token: 0x06000678 RID: 1656 RVA: 0x0001AC81 File Offset: 0x00018E81
		[Browsable(false)]
		public float Left
		{
			get
			{
				return this.X;
			}
		}

		/// <summary>Gets the y-coordinate of the top edge of this <see cref="T:System.Drawing.RectangleF" /> structure.</summary>
		/// <returns>The y-coordinate of the top edge of this <see cref="T:System.Drawing.RectangleF" /> structure.</returns>
		// Token: 0x170002AD RID: 685
		// (get) Token: 0x06000679 RID: 1657 RVA: 0x0001AC89 File Offset: 0x00018E89
		[Browsable(false)]
		public float Top
		{
			get
			{
				return this.Y;
			}
		}

		/// <summary>Gets the x-coordinate that is the sum of <see cref="P:System.Drawing.RectangleF.X" /> and <see cref="P:System.Drawing.RectangleF.Width" /> of this <see cref="T:System.Drawing.RectangleF" /> structure.</summary>
		/// <returns>The x-coordinate that is the sum of <see cref="P:System.Drawing.RectangleF.X" /> and <see cref="P:System.Drawing.RectangleF.Width" /> of this <see cref="T:System.Drawing.RectangleF" /> structure. </returns>
		// Token: 0x170002AE RID: 686
		// (get) Token: 0x0600067A RID: 1658 RVA: 0x0001AC91 File Offset: 0x00018E91
		[Browsable(false)]
		public float Right
		{
			get
			{
				return this.X + this.Width;
			}
		}

		/// <summary>Gets the y-coordinate that is the sum of <see cref="P:System.Drawing.RectangleF.Y" /> and <see cref="P:System.Drawing.RectangleF.Height" /> of this <see cref="T:System.Drawing.RectangleF" /> structure.</summary>
		/// <returns>The y-coordinate that is the sum of <see cref="P:System.Drawing.RectangleF.Y" /> and <see cref="P:System.Drawing.RectangleF.Height" /> of this <see cref="T:System.Drawing.RectangleF" /> structure.</returns>
		// Token: 0x170002AF RID: 687
		// (get) Token: 0x0600067B RID: 1659 RVA: 0x0001ACA0 File Offset: 0x00018EA0
		[Browsable(false)]
		public float Bottom
		{
			get
			{
				return this.Y + this.Height;
			}
		}

		/// <summary>Tests whether the <see cref="P:System.Drawing.RectangleF.Width" /> or <see cref="P:System.Drawing.RectangleF.Height" /> property of this <see cref="T:System.Drawing.RectangleF" /> has a value of zero.</summary>
		/// <returns>This property returns <see langword="true" /> if the <see cref="P:System.Drawing.RectangleF.Width" /> or <see cref="P:System.Drawing.RectangleF.Height" /> property of this <see cref="T:System.Drawing.RectangleF" /> has a value of zero; otherwise, <see langword="false" />.</returns>
		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x0600067C RID: 1660 RVA: 0x0001ACAF File Offset: 0x00018EAF
		[Browsable(false)]
		public bool IsEmpty
		{
			get
			{
				return this.Width <= 0f || this.Height <= 0f;
			}
		}

		/// <summary>Tests whether <paramref name="obj" /> is a <see cref="T:System.Drawing.RectangleF" /> with the same location and size of this <see cref="T:System.Drawing.RectangleF" />.</summary>
		/// <param name="obj">The <see cref="T:System.Object" /> to test. </param>
		/// <returns>This method returns <see langword="true" /> if <paramref name="obj" /> is a <see cref="T:System.Drawing.RectangleF" /> and its <see langword="X" />, <see langword="Y" />, <see langword="Width" />, and <see langword="Height" /> properties are equal to the corresponding properties of this <see cref="T:System.Drawing.RectangleF" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600067D RID: 1661 RVA: 0x0001ACD0 File Offset: 0x00018ED0
		public override bool Equals(object obj)
		{
			if (!(obj is RectangleF))
			{
				return false;
			}
			RectangleF rectangleF = (RectangleF)obj;
			return rectangleF.X == this.X && rectangleF.Y == this.Y && rectangleF.Width == this.Width && rectangleF.Height == this.Height;
		}

		/// <summary>Tests whether two <see cref="T:System.Drawing.RectangleF" /> structures have equal location and size.</summary>
		/// <param name="left">The <see cref="T:System.Drawing.RectangleF" /> structure that is to the left of the equality operator. </param>
		/// <param name="right">The <see cref="T:System.Drawing.RectangleF" /> structure that is to the right of the equality operator. </param>
		/// <returns>This operator returns <see langword="true" /> if the two specified <see cref="T:System.Drawing.RectangleF" /> structures have equal <see cref="P:System.Drawing.RectangleF.X" />, <see cref="P:System.Drawing.RectangleF.Y" />, <see cref="P:System.Drawing.RectangleF.Width" />, and <see cref="P:System.Drawing.RectangleF.Height" /> properties.</returns>
		// Token: 0x0600067E RID: 1662 RVA: 0x0001AD2C File Offset: 0x00018F2C
		public static bool operator ==(RectangleF left, RectangleF right)
		{
			return left.X == right.X && left.Y == right.Y && left.Width == right.Width && left.Height == right.Height;
		}

		/// <summary>Tests whether two <see cref="T:System.Drawing.RectangleF" /> structures differ in location or size.</summary>
		/// <param name="left">The <see cref="T:System.Drawing.RectangleF" /> structure that is to the left of the inequality operator. </param>
		/// <param name="right">The <see cref="T:System.Drawing.RectangleF" /> structure that is to the right of the inequality operator. </param>
		/// <returns>This operator returns <see langword="true" /> if any of the <see cref="P:System.Drawing.RectangleF.X" /> , <see cref="P:System.Drawing.RectangleF.Y" />, <see cref="P:System.Drawing.RectangleF.Width" />, or <see cref="P:System.Drawing.RectangleF.Height" /> properties of the two <see cref="T:System.Drawing.Rectangle" /> structures are unequal; otherwise <see langword="false" />.</returns>
		// Token: 0x0600067F RID: 1663 RVA: 0x0001AD7B File Offset: 0x00018F7B
		public static bool operator !=(RectangleF left, RectangleF right)
		{
			return !(left == right);
		}

		/// <summary>Determines if the specified point is contained within this <see cref="T:System.Drawing.RectangleF" /> structure.</summary>
		/// <param name="x">The x-coordinate of the point to test. </param>
		/// <param name="y">The y-coordinate of the point to test. </param>
		/// <returns>This method returns <see langword="true" /> if the point defined by <paramref name="x" /> and <paramref name="y" /> is contained within this <see cref="T:System.Drawing.RectangleF" /> structure; otherwise <see langword="false" />.</returns>
		// Token: 0x06000680 RID: 1664 RVA: 0x0001AD87 File Offset: 0x00018F87
		public bool Contains(float x, float y)
		{
			return this.X <= x && x < this.X + this.Width && this.Y <= y && y < this.Y + this.Height;
		}

		/// <summary>Determines if the specified point is contained within this <see cref="T:System.Drawing.RectangleF" /> structure.</summary>
		/// <param name="pt">The <see cref="T:System.Drawing.PointF" /> to test. </param>
		/// <returns>This method returns <see langword="true" /> if the point represented by the <paramref name="pt" /> parameter is contained within this <see cref="T:System.Drawing.RectangleF" /> structure; otherwise <see langword="false" />.</returns>
		// Token: 0x06000681 RID: 1665 RVA: 0x0001ADBD File Offset: 0x00018FBD
		public bool Contains(PointF pt)
		{
			return this.Contains(pt.X, pt.Y);
		}

		/// <summary>Determines if the rectangular region represented by <paramref name="rect" /> is entirely contained within this <see cref="T:System.Drawing.RectangleF" /> structure.</summary>
		/// <param name="rect">The <see cref="T:System.Drawing.RectangleF" /> to test. </param>
		/// <returns>This method returns <see langword="true" /> if the rectangular region represented by <paramref name="rect" /> is entirely contained within the rectangular region represented by this <see cref="T:System.Drawing.RectangleF" />; otherwise <see langword="false" />.</returns>
		// Token: 0x06000682 RID: 1666 RVA: 0x0001ADD4 File Offset: 0x00018FD4
		public bool Contains(RectangleF rect)
		{
			return this.X <= rect.X && rect.X + rect.Width <= this.X + this.Width && this.Y <= rect.Y && rect.Y + rect.Height <= this.Y + this.Height;
		}

		/// <summary>Gets the hash code for this <see cref="T:System.Drawing.RectangleF" /> structure. For information about the use of hash codes, see <see langword="Object.GetHashCode" />.</summary>
		/// <returns>The hash code for this <see cref="T:System.Drawing.RectangleF" />.</returns>
		// Token: 0x06000683 RID: 1667 RVA: 0x0001AE40 File Offset: 0x00019040
		public override int GetHashCode()
		{
			return (int)((uint)this.X ^ ((uint)this.Y << 13 | (uint)this.Y >> 19) ^ ((uint)this.Width << 26 | (uint)this.Width >> 6) ^ ((uint)this.Height << 7 | (uint)this.Height >> 25));
		}

		/// <summary>Enlarges this <see cref="T:System.Drawing.RectangleF" /> structure by the specified amount.</summary>
		/// <param name="x">The amount to inflate this <see cref="T:System.Drawing.RectangleF" /> structure horizontally. </param>
		/// <param name="y">The amount to inflate this <see cref="T:System.Drawing.RectangleF" /> structure vertically. </param>
		// Token: 0x06000684 RID: 1668 RVA: 0x0001AE94 File Offset: 0x00019094
		public void Inflate(float x, float y)
		{
			this.X -= x;
			this.Y -= y;
			this.Width += 2f * x;
			this.Height += 2f * y;
		}

		/// <summary>Enlarges this <see cref="T:System.Drawing.RectangleF" /> by the specified amount.</summary>
		/// <param name="size">The amount to inflate this rectangle. </param>
		// Token: 0x06000685 RID: 1669 RVA: 0x0001AEE5 File Offset: 0x000190E5
		public void Inflate(SizeF size)
		{
			this.Inflate(size.Width, size.Height);
		}

		/// <summary>Creates and returns an enlarged copy of the specified <see cref="T:System.Drawing.RectangleF" /> structure. The copy is enlarged by the specified amount and the original rectangle remains unmodified.</summary>
		/// <param name="rect">The <see cref="T:System.Drawing.RectangleF" /> to be copied. This rectangle is not modified. </param>
		/// <param name="x">The amount to enlarge the copy of the rectangle horizontally. </param>
		/// <param name="y">The amount to enlarge the copy of the rectangle vertically. </param>
		/// <returns>The enlarged <see cref="T:System.Drawing.RectangleF" />.</returns>
		// Token: 0x06000686 RID: 1670 RVA: 0x0001AEFC File Offset: 0x000190FC
		public static RectangleF Inflate(RectangleF rect, float x, float y)
		{
			RectangleF result = rect;
			result.Inflate(x, y);
			return result;
		}

		/// <summary>Replaces this <see cref="T:System.Drawing.RectangleF" /> structure with the intersection of itself and the specified <see cref="T:System.Drawing.RectangleF" /> structure.</summary>
		/// <param name="rect">The rectangle to intersect. </param>
		// Token: 0x06000687 RID: 1671 RVA: 0x0001AF18 File Offset: 0x00019118
		public void Intersect(RectangleF rect)
		{
			RectangleF rectangleF = RectangleF.Intersect(rect, this);
			this.X = rectangleF.X;
			this.Y = rectangleF.Y;
			this.Width = rectangleF.Width;
			this.Height = rectangleF.Height;
		}

		/// <summary>Returns a <see cref="T:System.Drawing.RectangleF" /> structure that represents the intersection of two rectangles. If there is no intersection, and empty <see cref="T:System.Drawing.RectangleF" /> is returned.</summary>
		/// <param name="a">A rectangle to intersect. </param>
		/// <param name="b">A rectangle to intersect. </param>
		/// <returns>A third <see cref="T:System.Drawing.RectangleF" /> structure the size of which represents the overlapped area of the two specified rectangles.</returns>
		// Token: 0x06000688 RID: 1672 RVA: 0x0001AF68 File Offset: 0x00019168
		public static RectangleF Intersect(RectangleF a, RectangleF b)
		{
			float num = Math.Max(a.X, b.X);
			float num2 = Math.Min(a.X + a.Width, b.X + b.Width);
			float num3 = Math.Max(a.Y, b.Y);
			float num4 = Math.Min(a.Y + a.Height, b.Y + b.Height);
			if (num2 >= num && num4 >= num3)
			{
				return new RectangleF(num, num3, num2 - num, num4 - num3);
			}
			return RectangleF.Empty;
		}

		/// <summary>Determines if this rectangle intersects with <paramref name="rect" />.</summary>
		/// <param name="rect">The rectangle to test. </param>
		/// <returns>This method returns <see langword="true" /> if there is any intersection.</returns>
		// Token: 0x06000689 RID: 1673 RVA: 0x0001B000 File Offset: 0x00019200
		public bool IntersectsWith(RectangleF rect)
		{
			return rect.X < this.X + this.Width && this.X < rect.X + rect.Width && rect.Y < this.Y + this.Height && this.Y < rect.Y + rect.Height;
		}

		/// <summary>Creates the smallest possible third rectangle that can contain both of two rectangles that form a union.</summary>
		/// <param name="a">A rectangle to union. </param>
		/// <param name="b">A rectangle to union. </param>
		/// <returns>A third <see cref="T:System.Drawing.RectangleF" /> structure that contains both of the two rectangles that form the union.</returns>
		// Token: 0x0600068A RID: 1674 RVA: 0x0001B06C File Offset: 0x0001926C
		public static RectangleF Union(RectangleF a, RectangleF b)
		{
			float num = Math.Min(a.X, b.X);
			float num2 = Math.Max(a.X + a.Width, b.X + b.Width);
			float num3 = Math.Min(a.Y, b.Y);
			float num4 = Math.Max(a.Y + a.Height, b.Y + b.Height);
			return new RectangleF(num, num3, num2 - num, num4 - num3);
		}

		/// <summary>Adjusts the location of this rectangle by the specified amount.</summary>
		/// <param name="pos">The amount to offset the location. </param>
		// Token: 0x0600068B RID: 1675 RVA: 0x0001B0F6 File Offset: 0x000192F6
		public void Offset(PointF pos)
		{
			this.Offset(pos.X, pos.Y);
		}

		/// <summary>Adjusts the location of this rectangle by the specified amount.</summary>
		/// <param name="x">The amount to offset the location horizontally. </param>
		/// <param name="y">The amount to offset the location vertically. </param>
		// Token: 0x0600068C RID: 1676 RVA: 0x0001B10C File Offset: 0x0001930C
		public void Offset(float x, float y)
		{
			this.X += x;
			this.Y += y;
		}

		// Token: 0x0600068D RID: 1677 RVA: 0x0001B12A File Offset: 0x0001932A
		internal GPRECTF ToGPRECTF()
		{
			return new GPRECTF(this.X, this.Y, this.Width, this.Height);
		}

		/// <summary>Converts the specified <see cref="T:System.Drawing.Rectangle" /> structure to a <see cref="T:System.Drawing.RectangleF" /> structure.</summary>
		/// <param name="r">The <see cref="T:System.Drawing.Rectangle" /> structure to convert. </param>
		/// <returns>The <see cref="T:System.Drawing.RectangleF" /> structure that is converted from the specified <see cref="T:System.Drawing.Rectangle" /> structure.</returns>
		// Token: 0x0600068E RID: 1678 RVA: 0x0001B149 File Offset: 0x00019349
		public static implicit operator RectangleF(Rectangle r)
		{
			return new RectangleF((float)r.X, (float)r.Y, (float)r.Width, (float)r.Height);
		}

		/// <summary>Converts the <see langword="Location" /> and <see cref="T:System.Drawing.Size" /> of this <see cref="T:System.Drawing.RectangleF" /> to a human-readable string.</summary>
		/// <returns>A string that contains the position, width, and height of this <see cref="T:System.Drawing.RectangleF" /> structure. For example, "{X=20, Y=20, Width=100, Height=50}".</returns>
		// Token: 0x0600068F RID: 1679 RVA: 0x0001B170 File Offset: 0x00019370
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"{X=",
				this.X.ToString(CultureInfo.CurrentCulture),
				",Y=",
				this.Y.ToString(CultureInfo.CurrentCulture),
				",Width=",
				this.Width.ToString(CultureInfo.CurrentCulture),
				",Height=",
				this.Height.ToString(CultureInfo.CurrentCulture),
				"}"
			});
		}

		/// <summary>Represents an instance of the <see cref="T:System.Drawing.RectangleF" /> class with its members uninitialized.</summary>
		// Token: 0x0400056F RID: 1391
		public static readonly RectangleF Empty;

		// Token: 0x04000570 RID: 1392
		private float x;

		// Token: 0x04000571 RID: 1393
		private float y;

		// Token: 0x04000572 RID: 1394
		private float width;

		// Token: 0x04000573 RID: 1395
		private float height;
	}
}
