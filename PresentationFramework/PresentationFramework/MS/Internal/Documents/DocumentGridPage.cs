using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MS.Internal.Documents
{
	// Token: 0x020006BE RID: 1726
	internal class DocumentGridPage : FrameworkElement, IDisposable
	{
		// Token: 0x06006F8D RID: 28557 RVA: 0x00201180 File Offset: 0x001FF380
		public DocumentGridPage(DocumentPaginator paginator)
		{
			this._paginator = paginator;
			this._paginator.GetPageCompleted += this.OnGetPageCompleted;
			this.Init();
		}

		// Token: 0x17001A7C RID: 6780
		// (get) Token: 0x06006F8E RID: 28558 RVA: 0x00201215 File Offset: 0x001FF415
		public DocumentPage DocumentPage
		{
			get
			{
				this.CheckDisposed();
				return this._documentPageView.DocumentPage;
			}
		}

		// Token: 0x17001A7D RID: 6781
		// (get) Token: 0x06006F8F RID: 28559 RVA: 0x00201228 File Offset: 0x001FF428
		// (set) Token: 0x06006F90 RID: 28560 RVA: 0x0020123B File Offset: 0x001FF43B
		public int PageNumber
		{
			get
			{
				this.CheckDisposed();
				return this._documentPageView.PageNumber;
			}
			set
			{
				this.CheckDisposed();
				if (this._documentPageView.PageNumber != value)
				{
					this._documentPageView.PageNumber = value;
				}
			}
		}

		// Token: 0x17001A7E RID: 6782
		// (get) Token: 0x06006F91 RID: 28561 RVA: 0x0020125D File Offset: 0x001FF45D
		public DocumentPageView DocumentPageView
		{
			get
			{
				this.CheckDisposed();
				return this._documentPageView;
			}
		}

		// Token: 0x17001A7F RID: 6783
		// (get) Token: 0x06006F92 RID: 28562 RVA: 0x0020126B File Offset: 0x001FF46B
		// (set) Token: 0x06006F93 RID: 28563 RVA: 0x00201279 File Offset: 0x001FF479
		public bool ShowPageBorders
		{
			get
			{
				this.CheckDisposed();
				return this._showPageBorders;
			}
			set
			{
				this.CheckDisposed();
				if (this._showPageBorders != value)
				{
					this._showPageBorders = value;
					base.InvalidateMeasure();
				}
			}
		}

		// Token: 0x17001A80 RID: 6784
		// (get) Token: 0x06006F94 RID: 28564 RVA: 0x00201297 File Offset: 0x001FF497
		public bool IsPageLoaded
		{
			get
			{
				this.CheckDisposed();
				return this._loaded;
			}
		}

		// Token: 0x14000138 RID: 312
		// (add) Token: 0x06006F95 RID: 28565 RVA: 0x002012A8 File Offset: 0x001FF4A8
		// (remove) Token: 0x06006F96 RID: 28566 RVA: 0x002012E0 File Offset: 0x001FF4E0
		public event EventHandler PageLoaded;

		// Token: 0x06006F97 RID: 28567 RVA: 0x00201318 File Offset: 0x001FF518
		protected override Visual GetVisualChild(int index)
		{
			this.CheckDisposed();
			if (this.VisualChildrenCount == 0)
			{
				throw new ArgumentOutOfRangeException("index", index, SR.Get("Visual_ArgumentOutOfRange"));
			}
			switch (index)
			{
			case 0:
				return this._dropShadowRight;
			case 1:
				return this._dropShadowBottom;
			case 2:
				return this._pageBorder;
			default:
				throw new ArgumentOutOfRangeException("index", index, SR.Get("Visual_ArgumentOutOfRange"));
			}
		}

		// Token: 0x17001A81 RID: 6785
		// (get) Token: 0x06006F98 RID: 28568 RVA: 0x00201391 File Offset: 0x001FF591
		protected override int VisualChildrenCount
		{
			get
			{
				if (!this._disposed && this.hasAddedChildren)
				{
					return 3;
				}
				return 0;
			}
		}

		// Token: 0x06006F99 RID: 28569 RVA: 0x002013A8 File Offset: 0x001FF5A8
		protected sealed override Size MeasureOverride(Size availableSize)
		{
			this.CheckDisposed();
			if (!this.hasAddedChildren)
			{
				base.AddVisualChild(this._dropShadowRight);
				base.AddVisualChild(this._dropShadowBottom);
				base.AddVisualChild(this._pageBorder);
				this.hasAddedChildren = true;
			}
			if (this.ShowPageBorders)
			{
				this._pageBorder.BorderThickness = this._pageBorderVisibleThickness;
				this._pageBorder.Background = Brushes.White;
				this._dropShadowRight.Opacity = 0.35;
				this._dropShadowBottom.Opacity = 0.35;
			}
			else
			{
				this._pageBorder.BorderThickness = this._pageBorderInvisibleThickness;
				this._pageBorder.Background = Brushes.Transparent;
				this._dropShadowRight.Opacity = 0.0;
				this._dropShadowBottom.Opacity = 0.0;
			}
			this._dropShadowRight.Measure(availableSize);
			this._dropShadowBottom.Measure(availableSize);
			this._pageBorder.Measure(availableSize);
			if (this.DocumentPage.Size != Size.Empty && this.DocumentPage.Size.Width != 0.0)
			{
				this._documentPageView.SetPageZoom(availableSize.Width / this.DocumentPage.Size.Width);
			}
			return availableSize;
		}

		// Token: 0x06006F9A RID: 28570 RVA: 0x0020150C File Offset: 0x001FF70C
		protected sealed override Size ArrangeOverride(Size arrangeSize)
		{
			this.CheckDisposed();
			this._pageBorder.Arrange(new Rect(new Point(0.0, 0.0), arrangeSize));
			this._dropShadowRight.Arrange(new Rect(new Point(arrangeSize.Width, 5.0), new Size(5.0, Math.Max(0.0, arrangeSize.Height - 5.0))));
			this._dropShadowBottom.Arrange(new Rect(new Point(5.0, arrangeSize.Height), new Size(arrangeSize.Width, 5.0)));
			base.ArrangeOverride(arrangeSize);
			return arrangeSize;
		}

		// Token: 0x06006F9B RID: 28571 RVA: 0x002015DC File Offset: 0x001FF7DC
		private void Init()
		{
			this._documentPageView = new DocumentPageView();
			this._documentPageView.ClipToBounds = true;
			this._documentPageView.StretchDirection = StretchDirection.Both;
			this._documentPageView.PageNumber = int.MaxValue;
			this._pageBorder = new Border();
			this._pageBorder.BorderBrush = Brushes.Black;
			this._pageBorder.Child = this._documentPageView;
			this._dropShadowRight = new Rectangle();
			this._dropShadowRight.Fill = Brushes.Black;
			this._dropShadowRight.Opacity = 0.35;
			this._dropShadowBottom = new Rectangle();
			this._dropShadowBottom.Fill = Brushes.Black;
			this._dropShadowBottom.Opacity = 0.35;
			this._loaded = false;
		}

		// Token: 0x06006F9C RID: 28572 RVA: 0x002016B0 File Offset: 0x001FF8B0
		private void OnGetPageCompleted(object sender, GetPageCompletedEventArgs e)
		{
			if (!this._disposed && e != null && !e.Cancelled && e.Error == null && e.PageNumber != 2147483647 && e.PageNumber == this.PageNumber && e.DocumentPage != null && e.DocumentPage != DocumentPage.Missing)
			{
				this._loaded = true;
				if (this.PageLoaded != null)
				{
					this.PageLoaded(this, EventArgs.Empty);
				}
			}
		}

		// Token: 0x06006F9D RID: 28573 RVA: 0x00201728 File Offset: 0x001FF928
		protected void Dispose()
		{
			if (!this._disposed)
			{
				this._disposed = true;
				if (this._paginator != null)
				{
					this._paginator.GetPageCompleted -= this.OnGetPageCompleted;
					this._paginator = null;
				}
				IDisposable documentPageView = this._documentPageView;
				if (documentPageView != null)
				{
					documentPageView.Dispose();
				}
			}
		}

		// Token: 0x06006F9E RID: 28574 RVA: 0x0020177A File Offset: 0x001FF97A
		private void CheckDisposed()
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException(typeof(DocumentPageView).ToString());
			}
		}

		// Token: 0x06006F9F RID: 28575 RVA: 0x00201799 File Offset: 0x001FF999
		void IDisposable.Dispose()
		{
			GC.SuppressFinalize(this);
			this.Dispose();
		}

		// Token: 0x040036BF RID: 14015
		private bool hasAddedChildren;

		// Token: 0x040036C0 RID: 14016
		private DocumentPaginator _paginator;

		// Token: 0x040036C1 RID: 14017
		private DocumentPageView _documentPageView;

		// Token: 0x040036C2 RID: 14018
		private Rectangle _dropShadowRight;

		// Token: 0x040036C3 RID: 14019
		private Rectangle _dropShadowBottom;

		// Token: 0x040036C4 RID: 14020
		private Border _pageBorder;

		// Token: 0x040036C5 RID: 14021
		private bool _showPageBorders;

		// Token: 0x040036C6 RID: 14022
		private bool _loaded;

		// Token: 0x040036C7 RID: 14023
		private const double _dropShadowOpacity = 0.35;

		// Token: 0x040036C8 RID: 14024
		private const double _dropShadowWidth = 5.0;

		// Token: 0x040036C9 RID: 14025
		private readonly Thickness _pageBorderVisibleThickness = new Thickness(1.0, 1.0, 1.0, 1.0);

		// Token: 0x040036CA RID: 14026
		private readonly Thickness _pageBorderInvisibleThickness = new Thickness(0.0, 0.0, 0.0, 0.0);

		// Token: 0x040036CB RID: 14027
		private bool _disposed;
	}
}
