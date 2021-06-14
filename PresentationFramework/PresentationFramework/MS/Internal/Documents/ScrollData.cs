using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MS.Internal.Documents
{
	// Token: 0x020006F0 RID: 1776
	internal class ScrollData
	{
		// Token: 0x06007200 RID: 29184 RVA: 0x002095D5 File Offset: 0x002077D5
		internal void LineUp(UIElement owner)
		{
			this.SetVerticalOffset(owner, this._offset.Y - 16.0);
		}

		// Token: 0x06007201 RID: 29185 RVA: 0x002095F3 File Offset: 0x002077F3
		internal void LineDown(UIElement owner)
		{
			this.SetVerticalOffset(owner, this._offset.Y + 16.0);
		}

		// Token: 0x06007202 RID: 29186 RVA: 0x00209611 File Offset: 0x00207811
		internal void LineLeft(UIElement owner)
		{
			this.SetHorizontalOffset(owner, this._offset.X - 16.0);
		}

		// Token: 0x06007203 RID: 29187 RVA: 0x0020962F File Offset: 0x0020782F
		internal void LineRight(UIElement owner)
		{
			this.SetHorizontalOffset(owner, this._offset.X + 16.0);
		}

		// Token: 0x06007204 RID: 29188 RVA: 0x0020964D File Offset: 0x0020784D
		internal void PageUp(UIElement owner)
		{
			this.SetVerticalOffset(owner, this._offset.Y - this._viewport.Height);
		}

		// Token: 0x06007205 RID: 29189 RVA: 0x0020966D File Offset: 0x0020786D
		internal void PageDown(UIElement owner)
		{
			this.SetVerticalOffset(owner, this._offset.Y + this._viewport.Height);
		}

		// Token: 0x06007206 RID: 29190 RVA: 0x0020968D File Offset: 0x0020788D
		internal void PageLeft(UIElement owner)
		{
			this.SetHorizontalOffset(owner, this._offset.X - this._viewport.Width);
		}

		// Token: 0x06007207 RID: 29191 RVA: 0x002096AD File Offset: 0x002078AD
		internal void PageRight(UIElement owner)
		{
			this.SetHorizontalOffset(owner, this._offset.X + this._viewport.Width);
		}

		// Token: 0x06007208 RID: 29192 RVA: 0x002096CD File Offset: 0x002078CD
		internal void MouseWheelUp(UIElement owner)
		{
			this.SetVerticalOffset(owner, this._offset.Y - 48.0);
		}

		// Token: 0x06007209 RID: 29193 RVA: 0x002096EB File Offset: 0x002078EB
		internal void MouseWheelDown(UIElement owner)
		{
			this.SetVerticalOffset(owner, this._offset.Y + 48.0);
		}

		// Token: 0x0600720A RID: 29194 RVA: 0x00209709 File Offset: 0x00207909
		internal void MouseWheelLeft(UIElement owner)
		{
			this.SetHorizontalOffset(owner, this._offset.X - 48.0);
		}

		// Token: 0x0600720B RID: 29195 RVA: 0x00209727 File Offset: 0x00207927
		internal void MouseWheelRight(UIElement owner)
		{
			this.SetHorizontalOffset(owner, this._offset.X + 48.0);
		}

		// Token: 0x0600720C RID: 29196 RVA: 0x00209748 File Offset: 0x00207948
		internal void SetHorizontalOffset(UIElement owner, double offset)
		{
			if (!this.CanHorizontallyScroll)
			{
				return;
			}
			offset = Math.Max(0.0, Math.Min(this._extent.Width - this._viewport.Width, offset));
			if (!DoubleUtil.AreClose(offset, this._offset.X))
			{
				this._offset.X = offset;
				owner.InvalidateArrange();
				if (this._scrollOwner != null)
				{
					this._scrollOwner.InvalidateScrollInfo();
				}
			}
		}

		// Token: 0x0600720D RID: 29197 RVA: 0x002097C4 File Offset: 0x002079C4
		internal void SetVerticalOffset(UIElement owner, double offset)
		{
			if (!this.CanVerticallyScroll)
			{
				return;
			}
			offset = Math.Max(0.0, Math.Min(this._extent.Height - this._viewport.Height, offset));
			if (!DoubleUtil.AreClose(offset, this._offset.Y))
			{
				this._offset.Y = offset;
				owner.InvalidateArrange();
				if (this._scrollOwner != null)
				{
					this._scrollOwner.InvalidateScrollInfo();
				}
			}
		}

		// Token: 0x0600720E RID: 29198 RVA: 0x00209840 File Offset: 0x00207A40
		internal Rect MakeVisible(UIElement owner, Visual visual, Rect rectangle)
		{
			if (rectangle.IsEmpty || visual == null || (visual != owner && !owner.IsAncestorOf(visual)))
			{
				return Rect.Empty;
			}
			GeneralTransform generalTransform = visual.TransformToAncestor(owner);
			rectangle = generalTransform.TransformBounds(rectangle);
			Rect rect = new Rect(this._offset.X, this._offset.Y, this._viewport.Width, this._viewport.Height);
			rectangle.X += rect.X;
			rectangle.Y += rect.Y;
			double num = this.ComputeScrollOffset(rect.Left, rect.Right, rectangle.Left, rectangle.Right);
			double num2 = this.ComputeScrollOffset(rect.Top, rect.Bottom, rectangle.Top, rectangle.Bottom);
			this.SetHorizontalOffset(owner, num);
			this.SetVerticalOffset(owner, num2);
			if (this.CanHorizontallyScroll)
			{
				rect.X = num;
			}
			else
			{
				rectangle.X = rect.X;
			}
			if (this.CanVerticallyScroll)
			{
				rect.Y = num2;
			}
			else
			{
				rectangle.Y = rect.Y;
			}
			rectangle.Intersect(rect);
			if (!rectangle.IsEmpty)
			{
				rectangle.X -= rect.X;
				rectangle.Y -= rect.Y;
			}
			return rectangle;
		}

		// Token: 0x0600720F RID: 29199 RVA: 0x002099AC File Offset: 0x00207BAC
		internal void SetScrollOwner(UIElement owner, ScrollViewer value)
		{
			if (value != this._scrollOwner)
			{
				this._disableHorizonalScroll = false;
				this._disableVerticalScroll = false;
				this._offset = default(Vector);
				this._viewport = default(Size);
				this._extent = default(Size);
				this._scrollOwner = value;
				owner.InvalidateArrange();
			}
		}

		// Token: 0x17001B20 RID: 6944
		// (get) Token: 0x06007210 RID: 29200 RVA: 0x00209A01 File Offset: 0x00207C01
		// (set) Token: 0x06007211 RID: 29201 RVA: 0x00209A0C File Offset: 0x00207C0C
		internal bool CanVerticallyScroll
		{
			get
			{
				return !this._disableVerticalScroll;
			}
			set
			{
				this._disableVerticalScroll = !value;
			}
		}

		// Token: 0x17001B21 RID: 6945
		// (get) Token: 0x06007212 RID: 29202 RVA: 0x00209A18 File Offset: 0x00207C18
		// (set) Token: 0x06007213 RID: 29203 RVA: 0x00209A23 File Offset: 0x00207C23
		internal bool CanHorizontallyScroll
		{
			get
			{
				return !this._disableHorizonalScroll;
			}
			set
			{
				this._disableHorizonalScroll = !value;
			}
		}

		// Token: 0x17001B22 RID: 6946
		// (get) Token: 0x06007214 RID: 29204 RVA: 0x00209A2F File Offset: 0x00207C2F
		// (set) Token: 0x06007215 RID: 29205 RVA: 0x00209A3C File Offset: 0x00207C3C
		internal double ExtentWidth
		{
			get
			{
				return this._extent.Width;
			}
			set
			{
				this._extent.Width = value;
			}
		}

		// Token: 0x17001B23 RID: 6947
		// (get) Token: 0x06007216 RID: 29206 RVA: 0x00209A4A File Offset: 0x00207C4A
		// (set) Token: 0x06007217 RID: 29207 RVA: 0x00209A57 File Offset: 0x00207C57
		internal double ExtentHeight
		{
			get
			{
				return this._extent.Height;
			}
			set
			{
				this._extent.Height = value;
			}
		}

		// Token: 0x17001B24 RID: 6948
		// (get) Token: 0x06007218 RID: 29208 RVA: 0x00209A65 File Offset: 0x00207C65
		internal double ViewportWidth
		{
			get
			{
				return this._viewport.Width;
			}
		}

		// Token: 0x17001B25 RID: 6949
		// (get) Token: 0x06007219 RID: 29209 RVA: 0x00209A72 File Offset: 0x00207C72
		internal double ViewportHeight
		{
			get
			{
				return this._viewport.Height;
			}
		}

		// Token: 0x17001B26 RID: 6950
		// (get) Token: 0x0600721A RID: 29210 RVA: 0x00209A7F File Offset: 0x00207C7F
		internal double HorizontalOffset
		{
			get
			{
				return this._offset.X;
			}
		}

		// Token: 0x17001B27 RID: 6951
		// (get) Token: 0x0600721B RID: 29211 RVA: 0x00209A8C File Offset: 0x00207C8C
		internal double VerticalOffset
		{
			get
			{
				return this._offset.Y;
			}
		}

		// Token: 0x17001B28 RID: 6952
		// (get) Token: 0x0600721C RID: 29212 RVA: 0x00209A99 File Offset: 0x00207C99
		internal ScrollViewer ScrollOwner
		{
			get
			{
				return this._scrollOwner;
			}
		}

		// Token: 0x17001B29 RID: 6953
		// (get) Token: 0x0600721D RID: 29213 RVA: 0x00209AA1 File Offset: 0x00207CA1
		// (set) Token: 0x0600721E RID: 29214 RVA: 0x00209AA9 File Offset: 0x00207CA9
		internal Vector Offset
		{
			get
			{
				return this._offset;
			}
			set
			{
				this._offset = value;
			}
		}

		// Token: 0x17001B2A RID: 6954
		// (get) Token: 0x0600721F RID: 29215 RVA: 0x00209AB2 File Offset: 0x00207CB2
		// (set) Token: 0x06007220 RID: 29216 RVA: 0x00209ABA File Offset: 0x00207CBA
		internal Size Extent
		{
			get
			{
				return this._extent;
			}
			set
			{
				this._extent = value;
			}
		}

		// Token: 0x17001B2B RID: 6955
		// (get) Token: 0x06007221 RID: 29217 RVA: 0x00209AC3 File Offset: 0x00207CC3
		// (set) Token: 0x06007222 RID: 29218 RVA: 0x00209ACB File Offset: 0x00207CCB
		internal Size Viewport
		{
			get
			{
				return this._viewport;
			}
			set
			{
				this._viewport = value;
			}
		}

		// Token: 0x06007223 RID: 29219 RVA: 0x00209AD4 File Offset: 0x00207CD4
		private double ComputeScrollOffset(double topView, double bottomView, double topChild, double bottomChild)
		{
			bool flag = DoubleUtil.GreaterThanOrClose(topChild, topView) && DoubleUtil.LessThan(topChild, bottomView);
			bool flag2 = DoubleUtil.LessThanOrClose(bottomChild, bottomView) && DoubleUtil.GreaterThan(bottomChild, topView);
			double result;
			if (flag && flag2)
			{
				result = topView;
			}
			else
			{
				result = topChild;
			}
			return result;
		}

		// Token: 0x04003753 RID: 14163
		private bool _disableHorizonalScroll;

		// Token: 0x04003754 RID: 14164
		private bool _disableVerticalScroll;

		// Token: 0x04003755 RID: 14165
		private Vector _offset;

		// Token: 0x04003756 RID: 14166
		private Size _viewport;

		// Token: 0x04003757 RID: 14167
		private Size _extent;

		// Token: 0x04003758 RID: 14168
		private ScrollViewer _scrollOwner;
	}
}
