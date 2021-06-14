using System;
using System.Windows;
using System.Windows.Media;

namespace MS.Internal.Documents
{
	// Token: 0x020006BF RID: 1727
	internal class DocumentPageHost : FrameworkElement
	{
		// Token: 0x06006FA0 RID: 28576 RVA: 0x000D7058 File Offset: 0x000D5258
		internal DocumentPageHost()
		{
		}

		// Token: 0x06006FA1 RID: 28577 RVA: 0x002017A8 File Offset: 0x001FF9A8
		internal static void DisconnectPageVisual(Visual pageVisual)
		{
			Visual visual = VisualTreeHelper.GetParent(pageVisual) as Visual;
			if (visual != null)
			{
				ContainerVisual containerVisual = visual as ContainerVisual;
				if (containerVisual == null)
				{
					throw new ArgumentException(SR.Get("DocumentPageView_ParentNotDocumentPageHost"), "pageVisual");
				}
				DocumentPageHost documentPageHost = VisualTreeHelper.GetParent(containerVisual) as DocumentPageHost;
				if (documentPageHost == null)
				{
					throw new ArgumentException(SR.Get("DocumentPageView_ParentNotDocumentPageHost"), "pageVisual");
				}
				documentPageHost.PageVisual = null;
			}
		}

		// Token: 0x17001A82 RID: 6786
		// (get) Token: 0x06006FA2 RID: 28578 RVA: 0x0020180E File Offset: 0x001FFA0E
		// (set) Token: 0x06006FA3 RID: 28579 RVA: 0x00201818 File Offset: 0x001FFA18
		internal Visual PageVisual
		{
			get
			{
				return this._pageVisual;
			}
			set
			{
				if (this._pageVisual != null)
				{
					ContainerVisual containerVisual = VisualTreeHelper.GetParent(this._pageVisual) as ContainerVisual;
					Invariant.Assert(containerVisual != null);
					containerVisual.Children.Clear();
					base.RemoveVisualChild(containerVisual);
				}
				this._pageVisual = value;
				if (this._pageVisual != null)
				{
					ContainerVisual containerVisual = new ContainerVisual();
					base.AddVisualChild(containerVisual);
					containerVisual.Children.Add(this._pageVisual);
					containerVisual.SetValue(FrameworkElement.FlowDirectionProperty, FlowDirection.LeftToRight);
				}
			}
		}

		// Token: 0x06006FA4 RID: 28580 RVA: 0x00201898 File Offset: 0x001FFA98
		protected override Visual GetVisualChild(int index)
		{
			if (index != 0 || this._pageVisual == null)
			{
				throw new ArgumentOutOfRangeException("index", index, SR.Get("Visual_ArgumentOutOfRange"));
			}
			return VisualTreeHelper.GetParent(this._pageVisual) as Visual;
		}

		// Token: 0x17001A83 RID: 6787
		// (get) Token: 0x06006FA5 RID: 28581 RVA: 0x002018D0 File Offset: 0x001FFAD0
		protected override int VisualChildrenCount
		{
			get
			{
				if (this._pageVisual == null)
				{
					return 0;
				}
				return 1;
			}
		}

		// Token: 0x040036CC RID: 14028
		internal Point CachedOffset;

		// Token: 0x040036CD RID: 14029
		private Visual _pageVisual;
	}
}
