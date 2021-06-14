using System;
using System.Windows.Automation.Peers;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using MS.Internal.PresentationFramework;
using MS.Internal.Telemetry.PresentationFramework;

namespace System.Windows.Controls
{
	/// <summary>Represents a control that displays an image.</summary>
	// Token: 0x020004E7 RID: 1255
	[Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
	public class Image : FrameworkElement, IUriContext, IProvidePropertyFallback
	{
		/// <summary>Gets or sets the <see cref="T:System.Windows.Media.ImageSource" /> for the image.  </summary>
		/// <returns>The source of the drawn image. The default value is <see langword="null" />.</returns>
		// Token: 0x1700131A RID: 4890
		// (get) Token: 0x06004E69 RID: 20073 RVA: 0x00160F92 File Offset: 0x0015F192
		// (set) Token: 0x06004E6A RID: 20074 RVA: 0x00160FA4 File Offset: 0x0015F1A4
		public ImageSource Source
		{
			get
			{
				return (ImageSource)base.GetValue(Image.SourceProperty);
			}
			set
			{
				base.SetValue(Image.SourceProperty, value);
			}
		}

		/// <summary>Gets or sets a value that describes how an <see cref="T:System.Windows.Controls.Image" /> should be stretched to fill the destination rectangle.  </summary>
		/// <returns>One of the <see cref="T:System.Windows.Media.Stretch" /> values. The default is <see cref="F:System.Windows.Media.Stretch.Uniform" />.</returns>
		// Token: 0x1700131B RID: 4891
		// (get) Token: 0x06004E6B RID: 20075 RVA: 0x00160FB2 File Offset: 0x0015F1B2
		// (set) Token: 0x06004E6C RID: 20076 RVA: 0x00160FC4 File Offset: 0x0015F1C4
		public Stretch Stretch
		{
			get
			{
				return (Stretch)base.GetValue(Image.StretchProperty);
			}
			set
			{
				base.SetValue(Image.StretchProperty, value);
			}
		}

		/// <summary>Gets or sets a value that indicates how the image is scaled.  </summary>
		/// <returns>One of the <see cref="T:System.Windows.Controls.StretchDirection" /> values. The default is <see cref="F:System.Windows.Controls.StretchDirection.Both" />.</returns>
		// Token: 0x1700131C RID: 4892
		// (get) Token: 0x06004E6D RID: 20077 RVA: 0x00160FD7 File Offset: 0x0015F1D7
		// (set) Token: 0x06004E6E RID: 20078 RVA: 0x00160FE9 File Offset: 0x0015F1E9
		public StretchDirection StretchDirection
		{
			get
			{
				return (StretchDirection)base.GetValue(Image.StretchDirectionProperty);
			}
			set
			{
				base.SetValue(Image.StretchDirectionProperty, value);
			}
		}

		/// <summary>Occurs when there is a failure in the image.</summary>
		// Token: 0x140000DF RID: 223
		// (add) Token: 0x06004E6F RID: 20079 RVA: 0x00160FFC File Offset: 0x0015F1FC
		// (remove) Token: 0x06004E70 RID: 20080 RVA: 0x0016100A File Offset: 0x0015F20A
		public event EventHandler<ExceptionRoutedEventArgs> ImageFailed
		{
			add
			{
				base.AddHandler(Image.ImageFailedEvent, value);
			}
			remove
			{
				base.RemoveHandler(Image.ImageFailedEvent, value);
			}
		}

		/// <summary>Occurs after the DPI of the screen on which the Image is displayed changes.</summary>
		// Token: 0x140000E0 RID: 224
		// (add) Token: 0x06004E71 RID: 20081 RVA: 0x00161018 File Offset: 0x0015F218
		// (remove) Token: 0x06004E72 RID: 20082 RVA: 0x00161026 File Offset: 0x0015F226
		public event DpiChangedEventHandler DpiChanged
		{
			add
			{
				base.AddHandler(Image.DpiChangedEvent, value);
			}
			remove
			{
				base.RemoveHandler(Image.DpiChangedEvent, value);
			}
		}

		/// <summary>Creates and returns an <see cref="T:System.Windows.Automation.Peers.AutomationPeer" /> object for this <see cref="T:System.Windows.Controls.Image" />.</summary>
		/// <returns>An <see cref="T:System.Windows.Automation.Peers.AutomationPeer" /> object for this <see cref="T:System.Windows.Controls.Image" />.</returns>
		// Token: 0x06004E73 RID: 20083 RVA: 0x00161034 File Offset: 0x0015F234
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new ImageAutomationPeer(this);
		}

		/// <summary>Called when the DPI at which this Image is rendered changes.</summary>
		/// <param name="oldDpi">The previous DPI scale setting</param>
		/// <param name="newDpi">The new DPI scale setting</param>
		// Token: 0x06004E74 RID: 20084 RVA: 0x0016103C File Offset: 0x0015F23C
		protected override void OnDpiChanged(DpiScale oldDpi, DpiScale newDpi)
		{
			this._hasDpiChangedEverFired = true;
			base.RaiseEvent(new DpiChangedEventArgs(oldDpi, newDpi, Image.DpiChangedEvent, this));
		}

		/// <summary>Updates the <see cref="P:System.Windows.UIElement.DesiredSize" /> of the image. This method is called by the parent <see cref="T:System.Windows.UIElement" /> and is the first pass of layout. </summary>
		/// <param name="constraint">The size that the image should not exceed.</param>
		/// <returns>The image's desired size.</returns>
		// Token: 0x06004E75 RID: 20085 RVA: 0x00161058 File Offset: 0x0015F258
		protected override Size MeasureOverride(Size constraint)
		{
			if (!this._hasDpiChangedEverFired)
			{
				this._hasDpiChangedEverFired = true;
				DpiScale dpi = base.GetDpi();
				this.OnDpiChanged(dpi, dpi);
			}
			return this.MeasureArrangeHelper(constraint);
		}

		/// <summary>Arranges and sizes an image control.</summary>
		/// <param name="arrangeSize">The size used to arrange the control.</param>
		/// <returns>The size of the control.</returns>
		// Token: 0x06004E76 RID: 20086 RVA: 0x0016108A File Offset: 0x0015F28A
		protected override Size ArrangeOverride(Size arrangeSize)
		{
			return this.MeasureArrangeHelper(arrangeSize);
		}

		/// <summary>Renders the contents of an <see cref="T:System.Windows.Controls.Image" />.</summary>
		/// <param name="dc">An instance of <see cref="T:System.Windows.Media.DrawingContext" /> used to render the control.</param>
		// Token: 0x06004E77 RID: 20087 RVA: 0x00161094 File Offset: 0x0015F294
		protected override void OnRender(DrawingContext dc)
		{
			ImageSource source = this.Source;
			if (source == null)
			{
				return;
			}
			dc.DrawImage(source, new Rect(default(Point), base.RenderSize));
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <returns>The base URI of the current context.</returns>
		// Token: 0x1700131D RID: 4893
		// (get) Token: 0x06004E78 RID: 20088 RVA: 0x001610C7 File Offset: 0x0015F2C7
		// (set) Token: 0x06004E79 RID: 20089 RVA: 0x001610CF File Offset: 0x0015F2CF
		Uri IUriContext.BaseUri
		{
			get
			{
				return this.BaseUri;
			}
			set
			{
				this.BaseUri = value;
			}
		}

		/// <summary>Gets or sets the base uniform resource identifier (URI) for the <see cref="T:System.Windows.Controls.Image" />.</summary>
		/// <returns>A base URI for the <see cref="T:System.Windows.Controls.Image" />.</returns>
		// Token: 0x1700131E RID: 4894
		// (get) Token: 0x06004E7A RID: 20090 RVA: 0x000C216F File Offset: 0x000C036F
		// (set) Token: 0x06004E7B RID: 20091 RVA: 0x000C2181 File Offset: 0x000C0381
		protected virtual Uri BaseUri
		{
			get
			{
				return (Uri)base.GetValue(BaseUriHelper.BaseUriProperty);
			}
			set
			{
				base.SetValue(BaseUriHelper.BaseUriProperty, value);
			}
		}

		// Token: 0x06004E7C RID: 20092 RVA: 0x001610D8 File Offset: 0x0015F2D8
		private Size MeasureArrangeHelper(Size inputSize)
		{
			ImageSource source = this.Source;
			Size size = default(Size);
			if (source == null)
			{
				return size;
			}
			try
			{
				Image.UpdateBaseUri(this, source);
				size = source.Size;
			}
			catch (Exception errorException)
			{
				base.SetCurrentValue(Image.SourceProperty, null);
				base.RaiseEvent(new ExceptionRoutedEventArgs(Image.ImageFailedEvent, this, errorException));
			}
			Size size2 = Viewbox.ComputeScaleFactor(inputSize, size, this.Stretch, this.StretchDirection);
			return new Size(size.Width * size2.Width, size.Height * size2.Height);
		}

		// Token: 0x1700131F RID: 4895
		// (get) Token: 0x06004E7D RID: 20093 RVA: 0x0003BCFF File Offset: 0x00039EFF
		internal override int EffectiveValuesInitialSize
		{
			get
			{
				return 19;
			}
		}

		// Token: 0x06004E7E RID: 20094 RVA: 0x00161174 File Offset: 0x0015F374
		static Image()
		{
			Image.ImageFailedEvent = EventManager.RegisterRoutedEvent("ImageFailed", RoutingStrategy.Bubble, typeof(EventHandler<ExceptionRoutedEventArgs>), typeof(Image));
			Style defaultValue = Image.CreateDefaultStyles();
			FrameworkElement.StyleProperty.OverrideMetadata(typeof(Image), new FrameworkPropertyMetadata(defaultValue));
			Image.StretchProperty.OverrideMetadata(typeof(Image), new FrameworkPropertyMetadata(Stretch.Uniform, FrameworkPropertyMetadataOptions.AffectsMeasure));
			Image.StretchDirectionProperty.OverrideMetadata(typeof(Image), new FrameworkPropertyMetadata(StretchDirection.Both, FrameworkPropertyMetadataOptions.AffectsMeasure));
			Image.DpiChangedEvent = Window.DpiChangedEvent.AddOwner(typeof(Image));
			ControlsTraceLogger.AddControl(TelemetryControls.Image);
		}

		// Token: 0x06004E7F RID: 20095 RVA: 0x00161294 File Offset: 0x0015F494
		private static Style CreateDefaultStyles()
		{
			Style style = new Style(typeof(Image), null);
			style.Setters.Add(new Setter(FrameworkElement.FlowDirectionProperty, FlowDirection.LeftToRight));
			style.Seal();
			return style;
		}

		// Token: 0x06004E80 RID: 20096 RVA: 0x001612D4 File Offset: 0x0015F4D4
		private void OnSourceDownloaded(object sender, EventArgs e)
		{
			this.DetachBitmapSourceEvents();
			base.InvalidateMeasure();
			base.InvalidateVisual();
		}

		// Token: 0x06004E81 RID: 20097 RVA: 0x001612E8 File Offset: 0x0015F4E8
		private void OnSourceFailed(object sender, ExceptionEventArgs e)
		{
			this.DetachBitmapSourceEvents();
			base.SetCurrentValue(Image.SourceProperty, null);
			base.RaiseEvent(new ExceptionRoutedEventArgs(Image.ImageFailedEvent, this, e.ErrorException));
		}

		// Token: 0x06004E82 RID: 20098 RVA: 0x00161313 File Offset: 0x0015F513
		private void AttachBitmapSourceEvents(BitmapSource bitmapSource)
		{
			Image.DownloadCompletedEventManager.AddHandler(bitmapSource, new EventHandler<EventArgs>(this.OnSourceDownloaded));
			Image.DownloadFailedEventManager.AddHandler(bitmapSource, new EventHandler<ExceptionEventArgs>(this.OnSourceFailed));
			Image.DecodeFailedEventManager.AddHandler(bitmapSource, new EventHandler<ExceptionEventArgs>(this.OnSourceFailed));
			this._bitmapSource = bitmapSource;
		}

		// Token: 0x06004E83 RID: 20099 RVA: 0x00161354 File Offset: 0x0015F554
		private void DetachBitmapSourceEvents()
		{
			if (this._bitmapSource != null)
			{
				Image.DownloadCompletedEventManager.RemoveHandler(this._bitmapSource, new EventHandler<EventArgs>(this.OnSourceDownloaded));
				Image.DownloadFailedEventManager.RemoveHandler(this._bitmapSource, new EventHandler<ExceptionEventArgs>(this.OnSourceFailed));
				Image.DecodeFailedEventManager.RemoveHandler(this._bitmapSource, new EventHandler<ExceptionEventArgs>(this.OnSourceFailed));
				this._bitmapSource = null;
			}
		}

		// Token: 0x06004E84 RID: 20100 RVA: 0x001613B8 File Offset: 0x0015F5B8
		private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (!e.IsASubPropertyChange)
			{
				Image image = (Image)d;
				ImageSource imageSource = (ImageSource)e.OldValue;
				ImageSource imageSource2 = (ImageSource)e.NewValue;
				Image.UpdateBaseUri(d, imageSource2);
				image.DetachBitmapSourceEvents();
				BitmapSource bitmapSource = imageSource2 as BitmapSource;
				if (bitmapSource != null && bitmapSource.CheckAccess() && !bitmapSource.IsFrozen)
				{
					image.AttachBitmapSourceEvents(bitmapSource);
				}
			}
		}

		// Token: 0x06004E85 RID: 20101 RVA: 0x00161420 File Offset: 0x0015F620
		private static void UpdateBaseUri(DependencyObject d, ImageSource source)
		{
			if (source is IUriContext && !source.IsFrozen && ((IUriContext)source).BaseUri == null)
			{
				Uri baseUriCore = BaseUriHelper.GetBaseUriCore(d);
				if (baseUriCore != null)
				{
					((IUriContext)source).BaseUri = BaseUriHelper.GetBaseUriCore(d);
				}
			}
		}

		// Token: 0x06004E86 RID: 20102 RVA: 0x00161471 File Offset: 0x0015F671
		bool IProvidePropertyFallback.CanProvidePropertyFallback(string property)
		{
			return string.CompareOrdinal(property, "Source") == 0;
		}

		// Token: 0x06004E87 RID: 20103 RVA: 0x00161483 File Offset: 0x0015F683
		object IProvidePropertyFallback.ProvidePropertyFallback(string property, Exception cause)
		{
			if (string.CompareOrdinal(property, "Source") == 0)
			{
				base.RaiseEvent(new ExceptionRoutedEventArgs(Image.ImageFailedEvent, this, cause));
			}
			return null;
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Image.Source" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Image.Source" /> dependency property.</returns>
		// Token: 0x04002BC5 RID: 11205
		[CommonDependencyProperty]
		public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(ImageSource), typeof(Image), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(Image.OnSourceChanged), null), null);

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Image.Stretch" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Image.Stretch" /> dependency property.</returns>
		// Token: 0x04002BC7 RID: 11207
		[CommonDependencyProperty]
		public static readonly DependencyProperty StretchProperty = Viewbox.StretchProperty.AddOwner(typeof(Image));

		/// <summary>Identifies the <see cref="T:System.Windows.Controls.StretchDirection" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="T:System.Windows.Controls.StretchDirection" /> dependency property.</returns>
		// Token: 0x04002BC8 RID: 11208
		public static readonly DependencyProperty StretchDirectionProperty = Viewbox.StretchDirectionProperty.AddOwner(typeof(Image));

		// Token: 0x04002BCA RID: 11210
		private BitmapSource _bitmapSource;

		// Token: 0x04002BCB RID: 11211
		private bool _hasDpiChangedEverFired;

		// Token: 0x0200098F RID: 2447
		private class DownloadCompletedEventManager : WeakEventManager
		{
			// Token: 0x060087BB RID: 34747 RVA: 0x0001737C File Offset: 0x0001557C
			private DownloadCompletedEventManager()
			{
			}

			// Token: 0x060087BC RID: 34748 RVA: 0x00250D75 File Offset: 0x0024EF75
			public static void AddHandler(BitmapSource source, EventHandler<EventArgs> handler)
			{
				if (handler == null)
				{
					throw new ArgumentNullException("handler");
				}
				Image.DownloadCompletedEventManager.CurrentManager.ProtectedAddHandler(source, handler);
			}

			// Token: 0x060087BD RID: 34749 RVA: 0x00250D91 File Offset: 0x0024EF91
			public static void RemoveHandler(BitmapSource source, EventHandler<EventArgs> handler)
			{
				if (handler == null)
				{
					throw new ArgumentNullException("handler");
				}
				Image.DownloadCompletedEventManager.CurrentManager.ProtectedRemoveHandler(source, handler);
			}

			// Token: 0x060087BE RID: 34750 RVA: 0x0008090A File Offset: 0x0007EB0A
			protected override WeakEventManager.ListenerList NewListenerList()
			{
				return new WeakEventManager.ListenerList<EventArgs>();
			}

			// Token: 0x060087BF RID: 34751 RVA: 0x00250DB0 File Offset: 0x0024EFB0
			protected override void StartListening(object source)
			{
				BitmapSource bitmapSource = (BitmapSource)source;
				bitmapSource.DownloadCompleted += this.OnDownloadCompleted;
			}

			// Token: 0x060087C0 RID: 34752 RVA: 0x00250DD8 File Offset: 0x0024EFD8
			protected override void StopListening(object source)
			{
				BitmapSource bitmapSource = (BitmapSource)source;
				if (bitmapSource.CheckAccess() && !bitmapSource.IsFrozen)
				{
					bitmapSource.DownloadCompleted -= this.OnDownloadCompleted;
				}
			}

			// Token: 0x17001EA7 RID: 7847
			// (get) Token: 0x060087C1 RID: 34753 RVA: 0x00250E10 File Offset: 0x0024F010
			private static Image.DownloadCompletedEventManager CurrentManager
			{
				get
				{
					Type typeFromHandle = typeof(Image.DownloadCompletedEventManager);
					Image.DownloadCompletedEventManager downloadCompletedEventManager = (Image.DownloadCompletedEventManager)WeakEventManager.GetCurrentManager(typeFromHandle);
					if (downloadCompletedEventManager == null)
					{
						downloadCompletedEventManager = new Image.DownloadCompletedEventManager();
						WeakEventManager.SetCurrentManager(typeFromHandle, downloadCompletedEventManager);
					}
					return downloadCompletedEventManager;
				}
			}

			// Token: 0x060087C2 RID: 34754 RVA: 0x000174E5 File Offset: 0x000156E5
			private void OnDownloadCompleted(object sender, EventArgs args)
			{
				base.DeliverEvent(sender, args);
			}
		}

		// Token: 0x02000990 RID: 2448
		private class DownloadFailedEventManager : WeakEventManager
		{
			// Token: 0x060087C3 RID: 34755 RVA: 0x0001737C File Offset: 0x0001557C
			private DownloadFailedEventManager()
			{
			}

			// Token: 0x060087C4 RID: 34756 RVA: 0x00250E45 File Offset: 0x0024F045
			public static void AddHandler(BitmapSource source, EventHandler<ExceptionEventArgs> handler)
			{
				if (handler == null)
				{
					throw new ArgumentNullException("handler");
				}
				Image.DownloadFailedEventManager.CurrentManager.ProtectedAddHandler(source, handler);
			}

			// Token: 0x060087C5 RID: 34757 RVA: 0x00250E61 File Offset: 0x0024F061
			public static void RemoveHandler(BitmapSource source, EventHandler<ExceptionEventArgs> handler)
			{
				if (handler == null)
				{
					throw new ArgumentNullException("handler");
				}
				Image.DownloadFailedEventManager.CurrentManager.ProtectedRemoveHandler(source, handler);
			}

			// Token: 0x060087C6 RID: 34758 RVA: 0x00250E7D File Offset: 0x0024F07D
			protected override WeakEventManager.ListenerList NewListenerList()
			{
				return new WeakEventManager.ListenerList<ExceptionEventArgs>();
			}

			// Token: 0x060087C7 RID: 34759 RVA: 0x00250E84 File Offset: 0x0024F084
			protected override void StartListening(object source)
			{
				BitmapSource bitmapSource = (BitmapSource)source;
				bitmapSource.DownloadFailed += this.OnDownloadFailed;
			}

			// Token: 0x060087C8 RID: 34760 RVA: 0x00250EAC File Offset: 0x0024F0AC
			protected override void StopListening(object source)
			{
				BitmapSource bitmapSource = (BitmapSource)source;
				if (bitmapSource.CheckAccess() && !bitmapSource.IsFrozen)
				{
					bitmapSource.DownloadFailed -= this.OnDownloadFailed;
				}
			}

			// Token: 0x17001EA8 RID: 7848
			// (get) Token: 0x060087C9 RID: 34761 RVA: 0x00250EE4 File Offset: 0x0024F0E4
			private static Image.DownloadFailedEventManager CurrentManager
			{
				get
				{
					Type typeFromHandle = typeof(Image.DownloadFailedEventManager);
					Image.DownloadFailedEventManager downloadFailedEventManager = (Image.DownloadFailedEventManager)WeakEventManager.GetCurrentManager(typeFromHandle);
					if (downloadFailedEventManager == null)
					{
						downloadFailedEventManager = new Image.DownloadFailedEventManager();
						WeakEventManager.SetCurrentManager(typeFromHandle, downloadFailedEventManager);
					}
					return downloadFailedEventManager;
				}
			}

			// Token: 0x060087CA RID: 34762 RVA: 0x000174E5 File Offset: 0x000156E5
			private void OnDownloadFailed(object sender, ExceptionEventArgs args)
			{
				base.DeliverEvent(sender, args);
			}
		}

		// Token: 0x02000991 RID: 2449
		private class DecodeFailedEventManager : WeakEventManager
		{
			// Token: 0x060087CB RID: 34763 RVA: 0x0001737C File Offset: 0x0001557C
			private DecodeFailedEventManager()
			{
			}

			// Token: 0x060087CC RID: 34764 RVA: 0x00250F19 File Offset: 0x0024F119
			public static void AddHandler(BitmapSource source, EventHandler<ExceptionEventArgs> handler)
			{
				if (handler == null)
				{
					throw new ArgumentNullException("handler");
				}
				Image.DecodeFailedEventManager.CurrentManager.ProtectedAddHandler(source, handler);
			}

			// Token: 0x060087CD RID: 34765 RVA: 0x00250F35 File Offset: 0x0024F135
			public static void RemoveHandler(BitmapSource source, EventHandler<ExceptionEventArgs> handler)
			{
				if (handler == null)
				{
					throw new ArgumentNullException("handler");
				}
				Image.DecodeFailedEventManager.CurrentManager.ProtectedRemoveHandler(source, handler);
			}

			// Token: 0x060087CE RID: 34766 RVA: 0x00250E7D File Offset: 0x0024F07D
			protected override WeakEventManager.ListenerList NewListenerList()
			{
				return new WeakEventManager.ListenerList<ExceptionEventArgs>();
			}

			// Token: 0x060087CF RID: 34767 RVA: 0x00250F54 File Offset: 0x0024F154
			protected override void StartListening(object source)
			{
				BitmapSource bitmapSource = (BitmapSource)source;
				bitmapSource.DecodeFailed += this.OnDecodeFailed;
			}

			// Token: 0x060087D0 RID: 34768 RVA: 0x00250F7C File Offset: 0x0024F17C
			protected override void StopListening(object source)
			{
				BitmapSource bitmapSource = (BitmapSource)source;
				if (bitmapSource.CheckAccess() && !bitmapSource.IsFrozen)
				{
					bitmapSource.DecodeFailed -= this.OnDecodeFailed;
				}
			}

			// Token: 0x17001EA9 RID: 7849
			// (get) Token: 0x060087D1 RID: 34769 RVA: 0x00250FB4 File Offset: 0x0024F1B4
			private static Image.DecodeFailedEventManager CurrentManager
			{
				get
				{
					Type typeFromHandle = typeof(Image.DecodeFailedEventManager);
					Image.DecodeFailedEventManager decodeFailedEventManager = (Image.DecodeFailedEventManager)WeakEventManager.GetCurrentManager(typeFromHandle);
					if (decodeFailedEventManager == null)
					{
						decodeFailedEventManager = new Image.DecodeFailedEventManager();
						WeakEventManager.SetCurrentManager(typeFromHandle, decodeFailedEventManager);
					}
					return decodeFailedEventManager;
				}
			}

			// Token: 0x060087D2 RID: 34770 RVA: 0x000174E5 File Offset: 0x000156E5
			private void OnDecodeFailed(object sender, ExceptionEventArgs args)
			{
				base.DeliverEvent(sender, args);
			}
		}
	}
}
