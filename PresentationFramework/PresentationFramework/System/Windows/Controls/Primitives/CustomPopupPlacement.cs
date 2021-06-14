using System;

namespace System.Windows.Controls.Primitives
{
	/// <summary>Defines custom placement parameters for a <see cref="T:System.Windows.Controls.Primitives.Popup" /> control.</summary>
	// Token: 0x0200057A RID: 1402
	public struct CustomPopupPlacement
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.Primitives.CustomPopupPlacement" /> structure.</summary>
		/// <param name="point">The <see cref="T:System.Windows.Point" /> that is relative to the <see cref="P:System.Windows.Controls.Primitives.Popup.PlacementTarget" /> where the upper-left corner of the <see cref="T:System.Windows.Controls.Primitives.Popup" /> is placed.</param>
		/// <param name="primaryAxis">The <see cref="T:System.Windows.Controls.Primitives.PopupPrimaryAxis" /> along which a <see cref="T:System.Windows.Controls.Primitives.Popup" /> control moves when it is obstructed by a screen edge.</param>
		// Token: 0x06005C86 RID: 23686 RVA: 0x001A0B00 File Offset: 0x0019ED00
		public CustomPopupPlacement(Point point, PopupPrimaryAxis primaryAxis)
		{
			this._point = point;
			this._primaryAxis = primaryAxis;
		}

		/// <summary>Gets or sets the point that is relative to the target object where the upper-left corner of the <see cref="T:System.Windows.Controls.Primitives.Popup" /> control is placedl. </summary>
		/// <returns>A <see cref="T:System.Windows.Point" /> that is used to position a <see cref="T:System.Windows.Controls.Primitives.Popup" /> control. The default value is (0,0).</returns>
		// Token: 0x17001668 RID: 5736
		// (get) Token: 0x06005C87 RID: 23687 RVA: 0x001A0B10 File Offset: 0x0019ED10
		// (set) Token: 0x06005C88 RID: 23688 RVA: 0x001A0B18 File Offset: 0x0019ED18
		public Point Point
		{
			get
			{
				return this._point;
			}
			set
			{
				this._point = value;
			}
		}

		/// <summary>Gets or sets the direction in which to move a <see cref="T:System.Windows.Controls.Primitives.Popup" /> control when the <see cref="T:System.Windows.Controls.Primitives.Popup" /> is obscured by screen boundaries.</summary>
		/// <returns>The direction in which to move a <see cref="T:System.Windows.Controls.Primitives.Popup" /> control when the <see cref="T:System.Windows.Controls.Primitives.Popup" /> is obscured by screen boundaries.</returns>
		// Token: 0x17001669 RID: 5737
		// (get) Token: 0x06005C89 RID: 23689 RVA: 0x001A0B21 File Offset: 0x0019ED21
		// (set) Token: 0x06005C8A RID: 23690 RVA: 0x001A0B29 File Offset: 0x0019ED29
		public PopupPrimaryAxis PrimaryAxis
		{
			get
			{
				return this._primaryAxis;
			}
			set
			{
				this._primaryAxis = value;
			}
		}

		/// <summary>Compares two <see cref="T:System.Windows.Controls.Primitives.CustomPopupPlacement" /> structures to determine whether they are equal.</summary>
		/// <param name="placement1">The first <see cref="T:System.Windows.Controls.Primitives.CustomPopupPlacement" /> structure to compare.</param>
		/// <param name="placement2">The second <see cref="T:System.Windows.Controls.Primitives.CustomPopupPlacement" /> structure to compare.</param>
		/// <returns>
		///     <see langword="true" /> if the structures have the same values; otherwise, <see langword="false" />.</returns>
		// Token: 0x06005C8B RID: 23691 RVA: 0x001A0B32 File Offset: 0x0019ED32
		public static bool operator ==(CustomPopupPlacement placement1, CustomPopupPlacement placement2)
		{
			return placement1.Equals(placement2);
		}

		/// <summary>Compares two <see cref="T:System.Windows.Controls.Primitives.CustomPopupPlacement" /> structures to determine whether they are not equal. </summary>
		/// <param name="placement1">The first <see cref="T:System.Windows.Controls.Primitives.CustomPopupPlacement" /> structure to compare.</param>
		/// <param name="placement2">The second <see cref="T:System.Windows.Controls.Primitives.CustomPopupPlacement" /> structure to compare.</param>
		/// <returns>
		///     <see langword="true" /> if the structures do not have the same values; otherwise, <see langword="false" />.</returns>
		// Token: 0x06005C8C RID: 23692 RVA: 0x001A0B47 File Offset: 0x0019ED47
		public static bool operator !=(CustomPopupPlacement placement1, CustomPopupPlacement placement2)
		{
			return !placement1.Equals(placement2);
		}

		/// <summary>Compares this structure with another <see cref="T:System.Windows.Controls.Primitives.CustomPopupPlacement" /> structure to determine whether they are equal.</summary>
		/// <param name="o">The <see cref="T:System.Windows.Controls.Primitives.CustomPopupPlacement" /> structure to compare.</param>
		/// <returns>
		///     <see langword="true" /> if the structures have the same values; otherwise, <see langword="false" />.</returns>
		// Token: 0x06005C8D RID: 23693 RVA: 0x001A0B60 File Offset: 0x0019ED60
		public override bool Equals(object o)
		{
			if (o is CustomPopupPlacement)
			{
				CustomPopupPlacement customPopupPlacement = (CustomPopupPlacement)o;
				return customPopupPlacement._primaryAxis == this._primaryAxis && customPopupPlacement._point == this._point;
			}
			return false;
		}

		/// <summary>Gets the hash code for this structure. </summary>
		/// <returns>The hash code for this structure.</returns>
		// Token: 0x06005C8E RID: 23694 RVA: 0x001A0B9F File Offset: 0x0019ED9F
		public override int GetHashCode()
		{
			return this._primaryAxis.GetHashCode() ^ this._point.GetHashCode();
		}

		// Token: 0x04002FD4 RID: 12244
		private Point _point;

		// Token: 0x04002FD5 RID: 12245
		private PopupPrimaryAxis _primaryAxis;
	}
}
