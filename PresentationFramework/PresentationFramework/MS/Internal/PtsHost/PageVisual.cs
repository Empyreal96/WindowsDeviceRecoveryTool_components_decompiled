using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000635 RID: 1589
	internal class PageVisual : DrawingVisual, IContentHost
	{
		// Token: 0x060068F6 RID: 26870 RVA: 0x001D9CD2 File Offset: 0x001D7ED2
		internal PageVisual(FlowDocumentPage owner)
		{
			this._owner = new WeakReference(owner);
		}

		// Token: 0x060068F7 RID: 26871 RVA: 0x001D9CE8 File Offset: 0x001D7EE8
		internal void DrawBackground(Brush backgroundBrush, Rect renderBounds)
		{
			if (this._backgroundBrush != backgroundBrush || this._renderBounds != renderBounds)
			{
				this._backgroundBrush = backgroundBrush;
				this._renderBounds = renderBounds;
				using (DrawingContext drawingContext = base.RenderOpen())
				{
					if (this._backgroundBrush != null)
					{
						drawingContext.DrawRectangle(this._backgroundBrush, null, this._renderBounds);
					}
					else
					{
						drawingContext.DrawRectangle(Brushes.Transparent, null, this._renderBounds);
					}
				}
			}
		}

		// Token: 0x17001966 RID: 6502
		// (get) Token: 0x060068F8 RID: 26872 RVA: 0x001D9D6C File Offset: 0x001D7F6C
		// (set) Token: 0x060068F9 RID: 26873 RVA: 0x001D9D94 File Offset: 0x001D7F94
		internal Visual Child
		{
			get
			{
				VisualCollection children = base.Children;
				if (children.Count != 0)
				{
					return children[0];
				}
				return null;
			}
			set
			{
				VisualCollection children = base.Children;
				if (children.Count == 0)
				{
					children.Add(value);
					return;
				}
				if (children[0] != value)
				{
					children[0] = value;
				}
			}
		}

		// Token: 0x060068FA RID: 26874 RVA: 0x001D9DCC File Offset: 0x001D7FCC
		internal void ClearDrawingContext()
		{
			DrawingContext drawingContext = base.RenderOpen();
			if (drawingContext != null)
			{
				drawingContext.Close();
			}
		}

		// Token: 0x060068FB RID: 26875 RVA: 0x001D9DEC File Offset: 0x001D7FEC
		IInputElement IContentHost.InputHitTest(Point point)
		{
			IContentHost contentHost = this._owner.Target as IContentHost;
			if (contentHost != null)
			{
				return contentHost.InputHitTest(point);
			}
			return null;
		}

		// Token: 0x060068FC RID: 26876 RVA: 0x001D9E18 File Offset: 0x001D8018
		ReadOnlyCollection<Rect> IContentHost.GetRectangles(ContentElement child)
		{
			IContentHost contentHost = this._owner.Target as IContentHost;
			if (contentHost != null)
			{
				return contentHost.GetRectangles(child);
			}
			return new ReadOnlyCollection<Rect>(new List<Rect>(0));
		}

		// Token: 0x17001967 RID: 6503
		// (get) Token: 0x060068FD RID: 26877 RVA: 0x001D9E4C File Offset: 0x001D804C
		IEnumerator<IInputElement> IContentHost.HostedElements
		{
			get
			{
				IContentHost contentHost = this._owner.Target as IContentHost;
				if (contentHost != null)
				{
					return contentHost.HostedElements;
				}
				return null;
			}
		}

		// Token: 0x060068FE RID: 26878 RVA: 0x001D9E78 File Offset: 0x001D8078
		void IContentHost.OnChildDesiredSizeChanged(UIElement child)
		{
			IContentHost contentHost = this._owner.Target as IContentHost;
			if (contentHost != null)
			{
				contentHost.OnChildDesiredSizeChanged(child);
			}
		}

		// Token: 0x04003402 RID: 13314
		private readonly WeakReference _owner;

		// Token: 0x04003403 RID: 13315
		private Brush _backgroundBrush;

		// Token: 0x04003404 RID: 13316
		private Rect _renderBounds;
	}
}
