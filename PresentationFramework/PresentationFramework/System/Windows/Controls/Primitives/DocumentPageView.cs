using System;
using System.Windows.Automation.Peers;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using MS.Internal;
using MS.Internal.Documents;
using MS.Internal.KnownBoxes;

namespace System.Windows.Controls.Primitives
{
	/// <summary>Represents a viewport for a paginated <see cref="T:System.Windows.Documents.DocumentPage" />.  </summary>
	// Token: 0x02000583 RID: 1411
	public class DocumentPageView : FrameworkElement, IServiceProvider, IDisposable
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.Primitives.DocumentPageView" /> class.</summary>
		// Token: 0x06005D6C RID: 23916 RVA: 0x001A4B28 File Offset: 0x001A2D28
		public DocumentPageView()
		{
			this._pageZoom = 1.0;
		}

		// Token: 0x06005D6D RID: 23917 RVA: 0x001A4B48 File Offset: 0x001A2D48
		static DocumentPageView()
		{
			UIElement.ClipToBoundsProperty.OverrideMetadata(typeof(DocumentPageView), new PropertyMetadata(BooleanBoxes.TrueBox));
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.Documents.DocumentPaginator" /> used to retrieve content pages for this view.</summary>
		/// <returns>The paginator that retrieves content pages for this view.</returns>
		// Token: 0x17001696 RID: 5782
		// (get) Token: 0x06005D6E RID: 23918 RVA: 0x001A4BF8 File Offset: 0x001A2DF8
		// (set) Token: 0x06005D6F RID: 23919 RVA: 0x001A4C00 File Offset: 0x001A2E00
		public DocumentPaginator DocumentPaginator
		{
			get
			{
				return this._documentPaginator;
			}
			set
			{
				this.CheckDisposed();
				if (this._documentPaginator != value)
				{
					if (this._documentPaginator != null)
					{
						this._documentPaginator.GetPageCompleted -= this.HandleGetPageCompleted;
						this._documentPaginator.PagesChanged -= this.HandlePagesChanged;
						this.DisposeCurrentPage();
						this.DisposeAsyncPage();
					}
					Invariant.Assert(this._documentPage == null);
					Invariant.Assert(this._documentPageAsync == null);
					this._documentPaginator = value;
					this._textView = null;
					if (this._documentPaginator != null)
					{
						this._documentPaginator.GetPageCompleted += this.HandleGetPageCompleted;
						this._documentPaginator.PagesChanged += this.HandlePagesChanged;
					}
					base.InvalidateMeasure();
				}
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Documents.DocumentPage" /> associated with this view.</summary>
		/// <returns>The document page associated with this view.</returns>
		// Token: 0x17001697 RID: 5783
		// (get) Token: 0x06005D70 RID: 23920 RVA: 0x001A4CC7 File Offset: 0x001A2EC7
		public DocumentPage DocumentPage
		{
			get
			{
				if (this._documentPage != null)
				{
					return this._documentPage;
				}
				return DocumentPage.Missing;
			}
		}

		/// <summary>Gets or sets the page number of the current page displayed. </summary>
		/// <returns>The zero-based page number of the current page displayed.  The default is 0.</returns>
		// Token: 0x17001698 RID: 5784
		// (get) Token: 0x06005D71 RID: 23921 RVA: 0x001A4CDD File Offset: 0x001A2EDD
		// (set) Token: 0x06005D72 RID: 23922 RVA: 0x001A4CEF File Offset: 0x001A2EEF
		public int PageNumber
		{
			get
			{
				return (int)base.GetValue(DocumentPageView.PageNumberProperty);
			}
			set
			{
				base.SetValue(DocumentPageView.PageNumberProperty, value);
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Windows.Media.Stretch" /> enumeration that specifies how content should be stretched to fill the display page. </summary>
		/// <returns>The enumeration value that specifies how content should be stretched to fill the display page.  The default is <see cref="F:System.Windows.Media.Stretch.Uniform" />.</returns>
		// Token: 0x17001699 RID: 5785
		// (get) Token: 0x06005D73 RID: 23923 RVA: 0x001A4D02 File Offset: 0x001A2F02
		// (set) Token: 0x06005D74 RID: 23924 RVA: 0x001A4D14 File Offset: 0x001A2F14
		public Stretch Stretch
		{
			get
			{
				return (Stretch)base.GetValue(DocumentPageView.StretchProperty);
			}
			set
			{
				base.SetValue(DocumentPageView.StretchProperty, value);
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Windows.Controls.StretchDirection" /> enumeration that specifies in what scaling directions <see cref="P:System.Windows.Controls.Primitives.DocumentPageView.Stretch" /> should be applied. </summary>
		/// <returns>The enumeration value that specifies in what scaling directions <see cref="P:System.Windows.Controls.Primitives.DocumentPageView.Stretch" /> should be applied.  The default is <see cref="F:System.Windows.Controls.StretchDirection.DownOnly" />.</returns>
		// Token: 0x1700169A RID: 5786
		// (get) Token: 0x06005D75 RID: 23925 RVA: 0x001A4D27 File Offset: 0x001A2F27
		// (set) Token: 0x06005D76 RID: 23926 RVA: 0x001A4D39 File Offset: 0x001A2F39
		public StretchDirection StretchDirection
		{
			get
			{
				return (StretchDirection)base.GetValue(DocumentPageView.StretchDirectionProperty);
			}
			set
			{
				base.SetValue(DocumentPageView.StretchDirectionProperty, value);
			}
		}

		/// <summary>Occurs when a <see cref="T:System.Windows.Media.Visual" /> element of the <see cref="P:System.Windows.Controls.Primitives.DocumentPageView.DocumentPage" /> is connected.</summary>
		// Token: 0x14000113 RID: 275
		// (add) Token: 0x06005D77 RID: 23927 RVA: 0x001A4D4C File Offset: 0x001A2F4C
		// (remove) Token: 0x06005D78 RID: 23928 RVA: 0x001A4D84 File Offset: 0x001A2F84
		public event EventHandler PageConnected;

		/// <summary>Occurs when a <see cref="T:System.Windows.Media.Visual" /> element of the <see cref="P:System.Windows.Controls.Primitives.DocumentPageView.DocumentPage" /> is disconnected.</summary>
		// Token: 0x14000114 RID: 276
		// (add) Token: 0x06005D79 RID: 23929 RVA: 0x001A4DBC File Offset: 0x001A2FBC
		// (remove) Token: 0x06005D7A RID: 23930 RVA: 0x001A4DF4 File Offset: 0x001A2FF4
		public event EventHandler PageDisconnected;

		/// <summary>Invoked when the DPI setting for the document page view is changed.</summary>
		/// <param name="oldDpiScaleInfo">The available size that the parent can give to the child. This is soft constraint.</param>
		/// <param name="newDpiScaleInfo">The DocumentPageView's desired size.</param>
		// Token: 0x06005D7B RID: 23931 RVA: 0x001A4E29 File Offset: 0x001A3029
		protected override void OnDpiChanged(DpiScale oldDpiScaleInfo, DpiScale newDpiScaleInfo)
		{
			this.DisposeCurrentPage();
			this.DisposeAsyncPage();
		}

		/// <summary>Returns the available viewport size that can be given to display the current <see cref="P:System.Windows.Controls.Primitives.DocumentPageView.DocumentPage" />.</summary>
		/// <param name="availableSize">The maximum available size.</param>
		/// <returns>The actual desired size.</returns>
		// Token: 0x06005D7C RID: 23932 RVA: 0x001A4E38 File Offset: 0x001A3038
		protected sealed override Size MeasureOverride(Size availableSize)
		{
			Size result = default(Size);
			this.CheckDisposed();
			if (this._suspendLayout)
			{
				result = base.DesiredSize;
			}
			else if (this._documentPaginator != null)
			{
				if (this.ShouldReflowContent() && (!double.IsInfinity(availableSize.Width) || !double.IsInfinity(availableSize.Height)))
				{
					Size pageSize = this._documentPaginator.PageSize;
					Size size;
					if (double.IsInfinity(availableSize.Width))
					{
						size = default(Size);
						size.Height = availableSize.Height / this._pageZoom;
						size.Width = size.Height * (pageSize.Width / pageSize.Height);
					}
					else if (double.IsInfinity(availableSize.Height))
					{
						size = default(Size);
						size.Width = availableSize.Width / this._pageZoom;
						size.Height = size.Width * (pageSize.Height / pageSize.Width);
					}
					else
					{
						size = new Size(availableSize.Width / this._pageZoom, availableSize.Height / this._pageZoom);
					}
					if (!DoubleUtil.AreClose(pageSize, size))
					{
						this._documentPaginator.PageSize = size;
					}
				}
				if (this._documentPage == null && this._documentPageAsync == null)
				{
					if (this.PageNumber >= 0)
					{
						if (this._useAsynchronous)
						{
							this._documentPaginator.GetPageAsync(this.PageNumber, this);
						}
						else
						{
							this._documentPageAsync = this._documentPaginator.GetPage(this.PageNumber);
							if (this._documentPageAsync == null)
							{
								this._documentPageAsync = DocumentPage.Missing;
							}
						}
					}
					else
					{
						this._documentPage = DocumentPage.Missing;
					}
				}
				if (this._documentPageAsync != null)
				{
					this.DisposeCurrentPage();
					if (this._documentPageAsync == null)
					{
						this._documentPageAsync = DocumentPage.Missing;
					}
					if (this._pageVisualClone != null)
					{
						this.RemoveDuplicateVisual();
					}
					this._documentPage = this._documentPageAsync;
					if (this._documentPage != DocumentPage.Missing)
					{
						this._documentPage.PageDestroyed += this.HandlePageDestroyed;
						this._documentPageAsync.PageDestroyed -= this.HandleAsyncPageDestroyed;
					}
					this._documentPageAsync = null;
					this._newPageConnected = true;
				}
				if (this._documentPage != null && this._documentPage != DocumentPage.Missing)
				{
					Size pageSize = new Size(this._documentPage.Size.Width * this._pageZoom, this._documentPage.Size.Height * this._pageZoom);
					Size size2 = Viewbox.ComputeScaleFactor(availableSize, pageSize, this.Stretch, this.StretchDirection);
					result = new Size(pageSize.Width * size2.Width, pageSize.Height * size2.Height);
				}
				if (this._pageVisualClone != null)
				{
					result = this._visualCloneSize;
				}
			}
			return result;
		}

		/// <summary>Arranges the content to fit a specified view size.</summary>
		/// <param name="finalSize">The maximum size that the page view should use to arrange itself and its children.</param>
		/// <returns>The actual size that the page view used to arrange itself and its children.</returns>
		// Token: 0x06005D7D RID: 23933 RVA: 0x001A5104 File Offset: 0x001A3304
		protected sealed override Size ArrangeOverride(Size finalSize)
		{
			this.CheckDisposed();
			if (this._pageVisualClone == null)
			{
				if (this._pageHost == null)
				{
					this._pageHost = new DocumentPageHost();
					base.AddVisualChild(this._pageHost);
				}
				Invariant.Assert(this._pageHost != null);
				Visual visual = (this._documentPage == null) ? null : this._documentPage.Visual;
				if (visual == null)
				{
					this._pageHost.PageVisual = null;
					this._pageHost.CachedOffset = default(Point);
					this._pageHost.RenderTransform = null;
					this._pageHost.Arrange(new Rect(this._pageHost.CachedOffset, finalSize));
				}
				else
				{
					if (this._pageHost.PageVisual != visual)
					{
						DocumentPageHost.DisconnectPageVisual(visual);
						this._pageHost.PageVisual = visual;
					}
					Size size = this._documentPage.Size;
					Transform transform = Transform.Identity;
					if (base.FlowDirection == FlowDirection.RightToLeft)
					{
						transform = new MatrixTransform(-1.0, 0.0, 0.0, 1.0, size.Width, 0.0);
					}
					if (!DoubleUtil.IsOne(this._pageZoom))
					{
						ScaleTransform scaleTransform = new ScaleTransform(this._pageZoom, this._pageZoom);
						if (transform == Transform.Identity)
						{
							transform = scaleTransform;
						}
						else
						{
							transform = new MatrixTransform(transform.Value * scaleTransform.Value);
						}
						size = new Size(size.Width * this._pageZoom, size.Height * this._pageZoom);
					}
					Size size2 = Viewbox.ComputeScaleFactor(finalSize, size, this.Stretch, this.StretchDirection);
					if (!DoubleUtil.IsOne(size2.Width) || !DoubleUtil.IsOne(size2.Height))
					{
						ScaleTransform scaleTransform = new ScaleTransform(size2.Width, size2.Height);
						if (transform == Transform.Identity)
						{
							transform = scaleTransform;
						}
						else
						{
							transform = new MatrixTransform(transform.Value * scaleTransform.Value);
						}
						size = new Size(size.Width * size2.Width, size.Height * size2.Height);
					}
					this._pageHost.CachedOffset = new Point((finalSize.Width - size.Width) / 2.0, (finalSize.Height - size.Height) / 2.0);
					this._pageHost.RenderTransform = transform;
					this._pageHost.Arrange(new Rect(this._pageHost.CachedOffset, this._documentPage.Size));
				}
				if (this._newPageConnected)
				{
					this.OnPageConnected();
				}
				this.OnTransformChangedAsync();
			}
			else if (this._pageHost.PageVisual != this._pageVisualClone)
			{
				this._pageHost.PageVisual = this._pageVisualClone;
				this._pageHost.Arrange(new Rect(this._pageHost.CachedOffset, finalSize));
			}
			return base.ArrangeOverride(finalSize);
		}

		/// <summary>Returns the <see cref="T:System.Windows.Media.Visual" /> child at a specified index.</summary>
		/// <param name="index">The index of the visual child to return.</param>
		/// <returns>The visual child at the specified index.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="index" /> is less than zero, or greater than or equal to <see cref="P:System.Windows.Controls.Primitives.DocumentPageView.VisualChildrenCount" />.</exception>
		// Token: 0x06005D7E RID: 23934 RVA: 0x001A53EB File Offset: 0x001A35EB
		protected override Visual GetVisualChild(int index)
		{
			if (index != 0 || this._pageHost == null)
			{
				throw new ArgumentOutOfRangeException("index", index, SR.Get("Visual_ArgumentOutOfRange"));
			}
			return this._pageHost;
		}

		/// <summary>Releases all resources used by the <see cref="T:System.Windows.Controls.Primitives.DocumentPageView" />.</summary>
		// Token: 0x06005D7F RID: 23935 RVA: 0x001A541C File Offset: 0x001A361C
		protected void Dispose()
		{
			if (!this._disposed)
			{
				this._disposed = true;
				if (this._documentPaginator != null)
				{
					this._documentPaginator.GetPageCompleted -= this.HandleGetPageCompleted;
					this._documentPaginator.PagesChanged -= this.HandlePagesChanged;
					this._documentPaginator.CancelAsync(this);
					this.DisposeCurrentPage();
					this.DisposeAsyncPage();
				}
				Invariant.Assert(this._documentPage == null);
				Invariant.Assert(this._documentPageAsync == null);
				this._documentPaginator = null;
				this._textView = null;
			}
		}

		/// <summary>Gets the service object of the specified type.</summary>
		/// <param name="serviceType">The type of service object to get. </param>
		/// <returns>A service object of type <paramref name="serviceType" />, or <see langword="null" /> if there is no service object of type <paramref name="serviceType" />.</returns>
		// Token: 0x06005D80 RID: 23936 RVA: 0x001A54B0 File Offset: 0x001A36B0
		protected object GetService(Type serviceType)
		{
			object result = null;
			if (serviceType == null)
			{
				throw new ArgumentNullException("serviceType");
			}
			this.CheckDisposed();
			if (this._documentPaginator != null && this._documentPaginator is IServiceProvider)
			{
				if (serviceType == typeof(ITextView))
				{
					if (this._textView == null)
					{
						ITextContainer textContainer = ((IServiceProvider)this._documentPaginator).GetService(typeof(ITextContainer)) as ITextContainer;
						if (textContainer != null)
						{
							this._textView = new DocumentPageTextView(this, textContainer);
						}
					}
					result = this._textView;
				}
				else if (serviceType == typeof(TextContainer) || serviceType == typeof(ITextContainer))
				{
					result = ((IServiceProvider)this._documentPaginator).GetService(serviceType);
				}
			}
			return result;
		}

		/// <summary>Creates and returns an <see cref="T:System.Windows.Automation.Peers.AutomationPeer" /> for this <see cref="T:System.Windows.Controls.Primitives.DocumentPageView" />.</summary>
		/// <returns>The new <see cref="T:System.Windows.Automation.Peers.DocumentPageViewAutomationPeer" />.</returns>
		// Token: 0x06005D81 RID: 23937 RVA: 0x001A557E File Offset: 0x001A377E
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new DocumentPageViewAutomationPeer(this);
		}

		/// <summary>Gets a value that indicates whether <see cref="M:System.Windows.Controls.Primitives.DocumentPageView.Dispose" /> has been called for this instance.</summary>
		/// <returns>
		///     <see langword="true" /> if <see cref="M:System.Windows.Controls.Primitives.DocumentPageView.Dispose" /> has been called for this <see cref="T:System.Windows.Controls.Primitives.DocumentPageView" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700169B RID: 5787
		// (get) Token: 0x06005D82 RID: 23938 RVA: 0x001A5586 File Offset: 0x001A3786
		protected bool IsDisposed
		{
			get
			{
				return this._disposed;
			}
		}

		/// <summary>Gets the number of visual children contained in this view.</summary>
		/// <returns>The number of visual children contained in this view.</returns>
		// Token: 0x1700169C RID: 5788
		// (get) Token: 0x06005D83 RID: 23939 RVA: 0x001A558E File Offset: 0x001A378E
		protected override int VisualChildrenCount
		{
			get
			{
				if (this._pageHost == null)
				{
					return 0;
				}
				return 1;
			}
		}

		// Token: 0x06005D84 RID: 23940 RVA: 0x001A559C File Offset: 0x001A379C
		internal void SetPageZoom(double pageZoom)
		{
			Invariant.Assert(!DoubleUtil.LessThanOrClose(pageZoom, 0.0) && !double.IsInfinity(pageZoom));
			Invariant.Assert(!this._disposed);
			if (!DoubleUtil.AreClose(this._pageZoom, pageZoom))
			{
				this._pageZoom = pageZoom;
				base.InvalidateMeasure();
			}
		}

		// Token: 0x06005D85 RID: 23941 RVA: 0x001A55F4 File Offset: 0x001A37F4
		internal void SuspendLayout()
		{
			this._suspendLayout = true;
			this._pageVisualClone = this.DuplicatePageVisual();
			this._visualCloneSize = base.DesiredSize;
		}

		// Token: 0x06005D86 RID: 23942 RVA: 0x001A5615 File Offset: 0x001A3815
		internal void ResumeLayout()
		{
			this._suspendLayout = false;
			this._pageVisualClone = null;
			base.InvalidateMeasure();
		}

		// Token: 0x06005D87 RID: 23943 RVA: 0x001A562B File Offset: 0x001A382B
		internal void DuplicateVisual()
		{
			if (this._documentPage != null && this._pageVisualClone == null)
			{
				this._pageVisualClone = this.DuplicatePageVisual();
				this._visualCloneSize = base.DesiredSize;
				base.InvalidateArrange();
			}
		}

		// Token: 0x06005D88 RID: 23944 RVA: 0x001A565B File Offset: 0x001A385B
		internal void RemoveDuplicateVisual()
		{
			if (this._pageVisualClone != null)
			{
				this._pageVisualClone = null;
				base.InvalidateArrange();
			}
		}

		// Token: 0x1700169D RID: 5789
		// (get) Token: 0x06005D89 RID: 23945 RVA: 0x001A5672 File Offset: 0x001A3872
		// (set) Token: 0x06005D8A RID: 23946 RVA: 0x001A567A File Offset: 0x001A387A
		internal bool UseAsynchronousGetPage
		{
			get
			{
				return this._useAsynchronous;
			}
			set
			{
				this._useAsynchronous = value;
			}
		}

		// Token: 0x1700169E RID: 5790
		// (get) Token: 0x06005D8B RID: 23947 RVA: 0x001A5683 File Offset: 0x001A3883
		internal DocumentPage DocumentPageInternal
		{
			get
			{
				return this._documentPage;
			}
		}

		// Token: 0x06005D8C RID: 23948 RVA: 0x001A568B File Offset: 0x001A388B
		private void HandlePageDestroyed(object sender, EventArgs e)
		{
			if (!this._disposed)
			{
				base.InvalidateMeasure();
				this.DisposeCurrentPage();
			}
		}

		// Token: 0x06005D8D RID: 23949 RVA: 0x001A56A1 File Offset: 0x001A38A1
		private void HandleAsyncPageDestroyed(object sender, EventArgs e)
		{
			if (!this._disposed)
			{
				this.DisposeAsyncPage();
			}
		}

		// Token: 0x06005D8E RID: 23950 RVA: 0x001A56B4 File Offset: 0x001A38B4
		private void HandleGetPageCompleted(object sender, GetPageCompletedEventArgs e)
		{
			if (!this._disposed && e != null && !e.Cancelled && e.Error == null && e.PageNumber == this.PageNumber && e.UserState == this)
			{
				if (this._documentPageAsync != null && this._documentPageAsync != DocumentPage.Missing)
				{
					this._documentPageAsync.PageDestroyed -= this.HandleAsyncPageDestroyed;
				}
				this._documentPageAsync = e.DocumentPage;
				if (this._documentPageAsync == null)
				{
					this._documentPageAsync = DocumentPage.Missing;
				}
				if (this._documentPageAsync != DocumentPage.Missing)
				{
					this._documentPageAsync.PageDestroyed += this.HandleAsyncPageDestroyed;
				}
				base.InvalidateMeasure();
			}
		}

		// Token: 0x06005D8F RID: 23951 RVA: 0x001A5774 File Offset: 0x001A3974
		private void HandlePagesChanged(object sender, PagesChangedEventArgs e)
		{
			if (!this._disposed && e != null && this.PageNumber >= e.Start && (e.Count == 2147483647 || this.PageNumber <= e.Start + e.Count))
			{
				this.OnPageContentChanged();
			}
		}

		// Token: 0x06005D90 RID: 23952 RVA: 0x001A57C2 File Offset: 0x001A39C2
		private void OnTransformChangedAsync()
		{
			base.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new DispatcherOperationCallback(this.OnTransformChanged), null);
		}

		// Token: 0x06005D91 RID: 23953 RVA: 0x001A57DF File Offset: 0x001A39DF
		private object OnTransformChanged(object arg)
		{
			if (this._textView != null && this._documentPage != null)
			{
				this._textView.OnTransformChanged();
			}
			return null;
		}

		// Token: 0x06005D92 RID: 23954 RVA: 0x001A57FD File Offset: 0x001A39FD
		private void OnPageConnected()
		{
			this._newPageConnected = false;
			if (this._textView != null)
			{
				this._textView.OnPageConnected();
			}
			if (this.PageConnected != null && this._documentPage != null)
			{
				this.PageConnected(this, EventArgs.Empty);
			}
		}

		// Token: 0x06005D93 RID: 23955 RVA: 0x001A583A File Offset: 0x001A3A3A
		private void OnPageDisconnected()
		{
			if (this._textView != null)
			{
				this._textView.OnPageDisconnected();
			}
			if (this.PageDisconnected != null)
			{
				this.PageDisconnected(this, EventArgs.Empty);
			}
		}

		// Token: 0x06005D94 RID: 23956 RVA: 0x001A5868 File Offset: 0x001A3A68
		private void OnPageContentChanged()
		{
			base.InvalidateMeasure();
			this.DisposeCurrentPage();
			this.DisposeAsyncPage();
		}

		// Token: 0x06005D95 RID: 23957 RVA: 0x001A587C File Offset: 0x001A3A7C
		private void DisposeCurrentPage()
		{
			if (this._documentPage != null)
			{
				if (this._pageHost != null)
				{
					this._pageHost.PageVisual = null;
				}
				if (this._documentPage != DocumentPage.Missing)
				{
					this._documentPage.PageDestroyed -= this.HandlePageDestroyed;
				}
				if (this._documentPage != null)
				{
					((IDisposable)this._documentPage).Dispose();
				}
				this._documentPage = null;
				this.OnPageDisconnected();
			}
		}

		// Token: 0x06005D96 RID: 23958 RVA: 0x001A58EC File Offset: 0x001A3AEC
		private void DisposeAsyncPage()
		{
			if (this._documentPageAsync != null)
			{
				if (this._documentPageAsync != DocumentPage.Missing)
				{
					this._documentPageAsync.PageDestroyed -= this.HandleAsyncPageDestroyed;
				}
				if (this._documentPageAsync != null)
				{
					((IDisposable)this._documentPageAsync).Dispose();
				}
				this._documentPageAsync = null;
			}
		}

		// Token: 0x06005D97 RID: 23959 RVA: 0x001A593F File Offset: 0x001A3B3F
		private void CheckDisposed()
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException(typeof(DocumentPageView).ToString());
			}
		}

		// Token: 0x06005D98 RID: 23960 RVA: 0x001A5960 File Offset: 0x001A3B60
		private bool ShouldReflowContent()
		{
			bool result = false;
			if (DocumentViewerBase.GetIsMasterPage(this))
			{
				DocumentViewerBase hostViewer = this.GetHostViewer();
				if (hostViewer != null)
				{
					result = hostViewer.IsMasterPageView(this);
				}
			}
			return result;
		}

		// Token: 0x06005D99 RID: 23961 RVA: 0x001A598C File Offset: 0x001A3B8C
		private DocumentViewerBase GetHostViewer()
		{
			DocumentViewerBase result = null;
			if (base.TemplatedParent is DocumentViewerBase)
			{
				result = (DocumentViewerBase)base.TemplatedParent;
			}
			else
			{
				for (Visual visual = VisualTreeHelper.GetParent(this) as Visual; visual != null; visual = (VisualTreeHelper.GetParent(visual) as Visual))
				{
					if (visual is DocumentViewerBase)
					{
						result = (DocumentViewerBase)visual;
						break;
					}
				}
			}
			return result;
		}

		// Token: 0x06005D9A RID: 23962 RVA: 0x001A59E5 File Offset: 0x001A3BE5
		private static void OnPageNumberChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Invariant.Assert(d != null && d is DocumentPageView);
			((DocumentPageView)d).OnPageContentChanged();
		}

		// Token: 0x06005D9B RID: 23963 RVA: 0x001A5A08 File Offset: 0x001A3C08
		private DrawingVisual DuplicatePageVisual()
		{
			DrawingVisual drawingVisual = null;
			if (this._pageHost != null && this._pageHost.PageVisual != null && this._documentPage.Size != Size.Empty)
			{
				Rect rectangle = new Rect(this._documentPage.Size);
				rectangle.Width = Math.Min(rectangle.Width, 4096.0);
				rectangle.Height = Math.Min(rectangle.Height, 4096.0);
				drawingVisual = new DrawingVisual();
				try
				{
					if (rectangle.Width > 1.0 && rectangle.Height > 1.0)
					{
						RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int)rectangle.Width, (int)rectangle.Height, 96.0, 96.0, PixelFormats.Pbgra32);
						renderTargetBitmap.Render(this._pageHost.PageVisual);
						ImageBrush brush = new ImageBrush(renderTargetBitmap);
						drawingVisual.Opacity = 0.5;
						using (DrawingContext drawingContext = drawingVisual.RenderOpen())
						{
							drawingContext.DrawRectangle(brush, null, rectangle);
						}
					}
				}
				catch (OverflowException)
				{
				}
			}
			return drawingVisual;
		}

		/// <summary>This member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.  Use the type-safe <see cref="M:System.Windows.Controls.Primitives.DocumentPageView.GetService(System.Type)" /> method instead. </summary>
		/// <param name="serviceType"> An object that specifies the type of service object to get.</param>
		/// <returns>A service object of type <paramref name="serviceType" />.</returns>
		// Token: 0x06005D9C RID: 23964 RVA: 0x001A5B5C File Offset: 0x001A3D5C
		object IServiceProvider.GetService(Type serviceType)
		{
			return this.GetService(serviceType);
		}

		/// <summary>This member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.  Use the type-safe <see cref="M:System.Windows.Controls.Primitives.DocumentPageView.Dispose" /> method instead. </summary>
		// Token: 0x06005D9D RID: 23965 RVA: 0x001A5B65 File Offset: 0x001A3D65
		void IDisposable.Dispose()
		{
			this.Dispose();
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.DocumentPageView.PageNumber" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.DocumentPageView.PageNumber" /> dependency property.</returns>
		// Token: 0x04003018 RID: 12312
		public static readonly DependencyProperty PageNumberProperty = DependencyProperty.Register("PageNumber", typeof(int), typeof(DocumentPageView), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(DocumentPageView.OnPageNumberChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.DocumentPageView.Stretch" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.DocumentPageView.Stretch" /> dependency property.</returns>
		// Token: 0x04003019 RID: 12313
		public static readonly DependencyProperty StretchProperty = Viewbox.StretchProperty.AddOwner(typeof(DocumentPageView), new FrameworkPropertyMetadata(Stretch.Uniform, FrameworkPropertyMetadataOptions.AffectsMeasure));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.DocumentPageView.StretchDirection" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.DocumentPageView.StretchDirection" /> dependency property.</returns>
		// Token: 0x0400301A RID: 12314
		public static readonly DependencyProperty StretchDirectionProperty = Viewbox.StretchDirectionProperty.AddOwner(typeof(DocumentPageView), new FrameworkPropertyMetadata(StretchDirection.DownOnly, FrameworkPropertyMetadataOptions.AffectsMeasure));

		// Token: 0x0400301D RID: 12317
		private DocumentPaginator _documentPaginator;

		// Token: 0x0400301E RID: 12318
		private double _pageZoom;

		// Token: 0x0400301F RID: 12319
		private DocumentPage _documentPage;

		// Token: 0x04003020 RID: 12320
		private DocumentPage _documentPageAsync;

		// Token: 0x04003021 RID: 12321
		private DocumentPageTextView _textView;

		// Token: 0x04003022 RID: 12322
		private DocumentPageHost _pageHost;

		// Token: 0x04003023 RID: 12323
		private Visual _pageVisualClone;

		// Token: 0x04003024 RID: 12324
		private Size _visualCloneSize;

		// Token: 0x04003025 RID: 12325
		private bool _useAsynchronous = true;

		// Token: 0x04003026 RID: 12326
		private bool _suspendLayout;

		// Token: 0x04003027 RID: 12327
		private bool _disposed;

		// Token: 0x04003028 RID: 12328
		private bool _newPageConnected;
	}
}
