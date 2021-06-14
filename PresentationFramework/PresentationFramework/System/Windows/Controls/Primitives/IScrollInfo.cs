using System;
using System.Windows.Media;

namespace System.Windows.Controls.Primitives
{
	/// <summary>Represents the main scrollable region inside a <see cref="T:System.Windows.Controls.ScrollViewer" /> control.</summary>
	// Token: 0x02000593 RID: 1427
	public interface IScrollInfo
	{
		/// <summary>Scrolls up within content by one logical unit. </summary>
		// Token: 0x06005E39 RID: 24121
		void LineUp();

		/// <summary>Scrolls down within content by one logical unit. </summary>
		// Token: 0x06005E3A RID: 24122
		void LineDown();

		/// <summary>Scrolls left within content by one logical unit.</summary>
		// Token: 0x06005E3B RID: 24123
		void LineLeft();

		/// <summary>Scrolls right within content by one logical unit.</summary>
		// Token: 0x06005E3C RID: 24124
		void LineRight();

		/// <summary>Scrolls up within content by one page.</summary>
		// Token: 0x06005E3D RID: 24125
		void PageUp();

		/// <summary>Scrolls down within content by one page.</summary>
		// Token: 0x06005E3E RID: 24126
		void PageDown();

		/// <summary>Scrolls left within content by one page.</summary>
		// Token: 0x06005E3F RID: 24127
		void PageLeft();

		/// <summary>Scrolls right within content by one page.</summary>
		// Token: 0x06005E40 RID: 24128
		void PageRight();

		/// <summary>Scrolls up within content after a user clicks the wheel button on a mouse.</summary>
		// Token: 0x06005E41 RID: 24129
		void MouseWheelUp();

		/// <summary>Scrolls down within content after a user clicks the wheel button on a mouse.</summary>
		// Token: 0x06005E42 RID: 24130
		void MouseWheelDown();

		/// <summary>Scrolls left within content after a user clicks the wheel button on a mouse.</summary>
		// Token: 0x06005E43 RID: 24131
		void MouseWheelLeft();

		/// <summary>Scrolls right within content after a user clicks the wheel button on a mouse.</summary>
		// Token: 0x06005E44 RID: 24132
		void MouseWheelRight();

		/// <summary>Sets the amount of horizontal offset.</summary>
		/// <param name="offset">The degree to which content is horizontally offset from the containing viewport.</param>
		// Token: 0x06005E45 RID: 24133
		void SetHorizontalOffset(double offset);

		/// <summary>Sets the amount of vertical offset.</summary>
		/// <param name="offset">The degree to which content is vertically offset from the containing viewport.</param>
		// Token: 0x06005E46 RID: 24134
		void SetVerticalOffset(double offset);

		/// <summary>Forces content to scroll until the coordinate space of a <see cref="T:System.Windows.Media.Visual" /> object is visible. </summary>
		/// <param name="visual">A <see cref="T:System.Windows.Media.Visual" /> that becomes visible.</param>
		/// <param name="rectangle">A bounding rectangle that identifies the coordinate space to make visible.</param>
		/// <returns>A <see cref="T:System.Windows.Rect" /> that is visible.</returns>
		// Token: 0x06005E47 RID: 24135
		Rect MakeVisible(Visual visual, Rect rectangle);

		/// <summary>Gets or sets a value that indicates whether scrolling on the vertical axis is possible. </summary>
		/// <returns>
		///     <see langword="true" /> if scrolling is possible; otherwise, <see langword="false" />. This property has no default value.</returns>
		// Token: 0x170016BB RID: 5819
		// (get) Token: 0x06005E48 RID: 24136
		// (set) Token: 0x06005E49 RID: 24137
		bool CanVerticallyScroll { get; set; }

		/// <summary>Gets or sets a value that indicates whether scrolling on the horizontal axis is possible.</summary>
		/// <returns>
		///     <see langword="true" /> if scrolling is possible; otherwise, <see langword="false" />. This property has no default value.</returns>
		// Token: 0x170016BC RID: 5820
		// (get) Token: 0x06005E4A RID: 24138
		// (set) Token: 0x06005E4B RID: 24139
		bool CanHorizontallyScroll { get; set; }

		/// <summary>Gets the horizontal size of the extent.</summary>
		/// <returns>A <see cref="T:System.Double" /> that represents, in device independent pixels, the horizontal size of the extent. This property has no default value.</returns>
		// Token: 0x170016BD RID: 5821
		// (get) Token: 0x06005E4C RID: 24140
		double ExtentWidth { get; }

		/// <summary>Gets the vertical size of the extent.</summary>
		/// <returns>A <see cref="T:System.Double" /> that represents, in device independent pixels, the vertical size of the extent.This property has no default value.</returns>
		// Token: 0x170016BE RID: 5822
		// (get) Token: 0x06005E4D RID: 24141
		double ExtentHeight { get; }

		/// <summary>Gets the horizontal size of the viewport for this content.</summary>
		/// <returns>A <see cref="T:System.Double" /> that represents, in device independent pixels, the horizontal size of the viewport for this content. This property has no default value.</returns>
		// Token: 0x170016BF RID: 5823
		// (get) Token: 0x06005E4E RID: 24142
		double ViewportWidth { get; }

		/// <summary>Gets the vertical size of the viewport for this content.</summary>
		/// <returns>A <see cref="T:System.Double" /> that represents, in device independent pixels, the vertical size of the viewport for this content. This property has no default value.</returns>
		// Token: 0x170016C0 RID: 5824
		// (get) Token: 0x06005E4F RID: 24143
		double ViewportHeight { get; }

		/// <summary>Gets the horizontal offset of the scrolled content.</summary>
		/// <returns>A <see cref="T:System.Double" /> that represents, in device independent pixels, the horizontal offset. This property has no default value.</returns>
		// Token: 0x170016C1 RID: 5825
		// (get) Token: 0x06005E50 RID: 24144
		double HorizontalOffset { get; }

		/// <summary>Gets the vertical offset of the scrolled content.</summary>
		/// <returns>A <see cref="T:System.Double" /> that represents, in device independent pixels, the vertical offset of the scrolled content. Valid values are between zero and the <see cref="P:System.Windows.Controls.Primitives.IScrollInfo.ExtentHeight" /> minus the <see cref="P:System.Windows.Controls.Primitives.IScrollInfo.ViewportHeight" />. This property has no default value.</returns>
		// Token: 0x170016C2 RID: 5826
		// (get) Token: 0x06005E51 RID: 24145
		double VerticalOffset { get; }

		/// <summary>Gets or sets a <see cref="T:System.Windows.Controls.ScrollViewer" /> element that controls scrolling behavior.</summary>
		/// <returns>A <see cref="T:System.Windows.Controls.ScrollViewer" /> element that controls scrolling behavior. This property has no default value.</returns>
		// Token: 0x170016C3 RID: 5827
		// (get) Token: 0x06005E52 RID: 24146
		// (set) Token: 0x06005E53 RID: 24147
		ScrollViewer ScrollOwner { get; set; }
	}
}
