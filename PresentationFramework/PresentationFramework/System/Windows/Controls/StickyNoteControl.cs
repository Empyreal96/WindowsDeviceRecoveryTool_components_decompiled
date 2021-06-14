using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Annotations;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using System.Xml;
using MS.Internal;
using MS.Internal.Annotations;
using MS.Internal.Annotations.Component;
using MS.Internal.Commands;
using MS.Internal.Controls.StickyNote;
using MS.Internal.Documents;
using MS.Internal.KnownBoxes;
using MS.Utility;

namespace System.Windows.Controls
{
	/// <summary>Represents a control that lets users attach typed text or handwritten annotations to documents.</summary>
	// Token: 0x02000463 RID: 1123
	[TemplatePart(Name = "PART_CloseButton", Type = typeof(Button))]
	[TemplatePart(Name = "PART_TitleThumb", Type = typeof(Thumb))]
	[TemplatePart(Name = "PART_ResizeBottomRightThumb", Type = typeof(Thumb))]
	[TemplatePart(Name = "PART_ContentControl", Type = typeof(ContentControl))]
	[TemplatePart(Name = "PART_IconButton", Type = typeof(Button))]
	[TemplatePart(Name = "PART_CopyMenuItem", Type = typeof(MenuItem))]
	[TemplatePart(Name = "PART_PasteMenuItem", Type = typeof(MenuItem))]
	[TemplatePart(Name = "PART_InkMenuItem", Type = typeof(MenuItem))]
	[TemplatePart(Name = "PART_SelectMenuItem", Type = typeof(MenuItem))]
	[TemplatePart(Name = "PART_EraseMenuItem", Type = typeof(MenuItem))]
	public sealed class StickyNoteControl : Control, IAnnotationComponent
	{
		// Token: 0x060040BE RID: 16574 RVA: 0x00127B98 File Offset: 0x00125D98
		void IAnnotationComponent.AddAttachedAnnotation(IAttachedAnnotation attachedAnnotation)
		{
			if (attachedAnnotation == null)
			{
				throw new ArgumentNullException("attachedAnnotation");
			}
			if (this._attachedAnnotation == null)
			{
				EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.AddAttachedSNBegin);
				this.SetAnnotation(attachedAnnotation);
				EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.AddAttachedSNEnd);
				return;
			}
			throw new InvalidOperationException(SR.Get("AddAnnotationsNotImplemented"));
		}

		// Token: 0x060040BF RID: 16575 RVA: 0x00127BEC File Offset: 0x00125DEC
		void IAnnotationComponent.RemoveAttachedAnnotation(IAttachedAnnotation attachedAnnotation)
		{
			if (attachedAnnotation == null)
			{
				throw new ArgumentNullException("attachedAnnotation");
			}
			if (attachedAnnotation == this._attachedAnnotation)
			{
				EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.RemoveAttachedSNBegin);
				this.GiveUpFocus();
				this.ClearAnnotation();
				EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.RemoveAttachedSNEnd);
				return;
			}
			throw new ArgumentException(SR.Get("InvalidValueSpecified"), "attachedAnnotation");
		}

		// Token: 0x060040C0 RID: 16576 RVA: 0x00127C49 File Offset: 0x00125E49
		void IAnnotationComponent.ModifyAttachedAnnotation(IAttachedAnnotation attachedAnnotation, object previousAttachedAnchor, AttachmentLevel previousAttachmentLevel)
		{
			throw new NotSupportedException(SR.Get("NotSupported"));
		}

		// Token: 0x17000FE7 RID: 4071
		// (get) Token: 0x060040C1 RID: 16577 RVA: 0x00127C5C File Offset: 0x00125E5C
		IList IAnnotationComponent.AttachedAnnotations
		{
			get
			{
				ArrayList arrayList = new ArrayList(1);
				if (this._attachedAnnotation != null)
				{
					arrayList.Add(this._attachedAnnotation);
				}
				return arrayList;
			}
		}

		// Token: 0x060040C2 RID: 16578 RVA: 0x00127C88 File Offset: 0x00125E88
		GeneralTransform IAnnotationComponent.GetDesiredTransform(GeneralTransform transform)
		{
			if (this._attachedAnnotation == null)
			{
				return null;
			}
			if (this.IsExpanded && base.FlowDirection == FlowDirection.RightToLeft && this._attachedAnnotation.Parent is DocumentPageHost)
			{
				this._selfMirroring = true;
			}
			else
			{
				this._selfMirroring = false;
			}
			Point anchorPoint = this._attachedAnnotation.AnchorPoint;
			if (double.IsInfinity(anchorPoint.X) || double.IsInfinity(anchorPoint.Y))
			{
				throw new InvalidOperationException(SR.Get("InvalidAnchorPosition"));
			}
			if (double.IsNaN(anchorPoint.X) || double.IsNaN(anchorPoint.Y))
			{
				return null;
			}
			GeneralTransformGroup generalTransformGroup = new GeneralTransformGroup();
			if (this._selfMirroring)
			{
				generalTransformGroup.Children.Add(new MatrixTransform(-1.0, 0.0, 0.0, 1.0, base.Width, 0.0));
			}
			generalTransformGroup.Children.Add(new TranslateTransform(anchorPoint.X, anchorPoint.Y));
			TranslateTransform translateTransform = new TranslateTransform(0.0, 0.0);
			if (this.IsExpanded)
			{
				translateTransform = this.PositionTransform.Clone();
				this._deltaX = (this._deltaY = 0.0);
				Rect pageBounds = this.PageBounds;
				Rect stickyNoteBounds = this.StickyNoteBounds;
				double num;
				double num2;
				StickyNoteControl.GetOffsets(pageBounds, stickyNoteBounds, out num, out num2);
				if (DoubleUtil.GreaterThan(Math.Abs(num), Math.Abs(this._offsetX)))
				{
					double num3 = this._offsetX - num;
					if (DoubleUtil.LessThan(num3, 0.0))
					{
						num3 = Math.Max(num3, -(stickyNoteBounds.Left - pageBounds.Left));
					}
					translateTransform.X += num3;
					this._deltaX = num3;
				}
				if (DoubleUtil.GreaterThan(Math.Abs(num2), Math.Abs(this._offsetY)))
				{
					double num4 = this._offsetY - num2;
					if (DoubleUtil.LessThan(num4, 0.0))
					{
						num4 = Math.Max(num4, -(stickyNoteBounds.Top - pageBounds.Top));
					}
					translateTransform.Y += num4;
					this._deltaY = num4;
				}
			}
			if (translateTransform != null)
			{
				generalTransformGroup.Children.Add(translateTransform);
			}
			generalTransformGroup.Children.Add(transform);
			return generalTransformGroup;
		}

		// Token: 0x17000FE8 RID: 4072
		// (get) Token: 0x060040C3 RID: 16579 RVA: 0x00127EE6 File Offset: 0x001260E6
		UIElement IAnnotationComponent.AnnotatedElement
		{
			get
			{
				if (this._attachedAnnotation == null)
				{
					return null;
				}
				return this._attachedAnnotation.Parent as UIElement;
			}
		}

		// Token: 0x17000FE9 RID: 4073
		// (get) Token: 0x060040C4 RID: 16580 RVA: 0x00127F02 File Offset: 0x00126102
		// (set) Token: 0x060040C5 RID: 16581 RVA: 0x00127F0A File Offset: 0x0012610A
		PresentationContext IAnnotationComponent.PresentationContext
		{
			get
			{
				return this._presentationContext;
			}
			set
			{
				this._presentationContext = value;
			}
		}

		// Token: 0x17000FEA RID: 4074
		// (get) Token: 0x060040C6 RID: 16582 RVA: 0x00127F13 File Offset: 0x00126113
		// (set) Token: 0x060040C7 RID: 16583 RVA: 0x00127F1B File Offset: 0x0012611B
		int IAnnotationComponent.ZOrder
		{
			get
			{
				return this._zOrder;
			}
			set
			{
				this._zOrder = value;
				this.UpdateAnnotationWithSNC(XmlToken.ZOrder);
			}
		}

		// Token: 0x17000FEB RID: 4075
		// (get) Token: 0x060040C8 RID: 16584 RVA: 0x00127F2F File Offset: 0x0012612F
		// (set) Token: 0x060040C9 RID: 16585 RVA: 0x00127F46 File Offset: 0x00126146
		bool IAnnotationComponent.IsDirty
		{
			get
			{
				return this._anchor != null && this._anchor.IsDirty;
			}
			set
			{
				if (this._anchor != null)
				{
					this._anchor.IsDirty = value;
				}
				if (value)
				{
					base.InvalidateVisual();
				}
			}
		}

		// Token: 0x17000FEC RID: 4076
		// (get) Token: 0x060040CA RID: 16586 RVA: 0x00127F65 File Offset: 0x00126165
		// (set) Token: 0x060040CB RID: 16587 RVA: 0x00127F6D File Offset: 0x0012616D
		internal TranslateTransform PositionTransform
		{
			get
			{
				return this._positionTransform;
			}
			set
			{
				Invariant.Assert(value != null, "PositionTransform cannot be null.");
				this._positionTransform = value;
				this.InvalidateTransform();
			}
		}

		// Token: 0x17000FED RID: 4077
		// (get) Token: 0x060040CC RID: 16588 RVA: 0x00127F8A File Offset: 0x0012618A
		// (set) Token: 0x060040CD RID: 16589 RVA: 0x00127F92 File Offset: 0x00126192
		internal double XOffset
		{
			get
			{
				return this._offsetX;
			}
			set
			{
				this._offsetX = value;
			}
		}

		// Token: 0x17000FEE RID: 4078
		// (get) Token: 0x060040CE RID: 16590 RVA: 0x00127F9B File Offset: 0x0012619B
		// (set) Token: 0x060040CF RID: 16591 RVA: 0x00127FA3 File Offset: 0x001261A3
		internal double YOffset
		{
			get
			{
				return this._offsetY;
			}
			set
			{
				this._offsetY = value;
			}
		}

		// Token: 0x17000FEF RID: 4079
		// (get) Token: 0x060040D0 RID: 16592 RVA: 0x00127FAC File Offset: 0x001261AC
		internal bool FlipBothOrigins
		{
			get
			{
				return this.IsExpanded && base.FlowDirection == FlowDirection.RightToLeft && this._attachedAnnotation != null && this._attachedAnnotation.Parent is DocumentPageHost;
			}
		}

		// Token: 0x060040D1 RID: 16593 RVA: 0x00127FDC File Offset: 0x001261DC
		private void OnAuthorUpdated(object obj, AnnotationAuthorChangedEventArgs args)
		{
			if (!this.InternalLocker.IsLocked(LockHelper.LockFlag.AnnotationChanged))
			{
				this.UpdateSNCWithAnnotation(XmlToken.Author);
				this.IsDirty = true;
			}
		}

		// Token: 0x060040D2 RID: 16594 RVA: 0x00128000 File Offset: 0x00126200
		private void OnAnnotationUpdated(object obj, AnnotationResourceChangedEventArgs args)
		{
			if (!this.InternalLocker.IsLocked(LockHelper.LockFlag.AnnotationChanged))
			{
				SNCAnnotation sncAnnotation = new SNCAnnotation(args.Annotation);
				this._sncAnnotation = sncAnnotation;
				this.UpdateSNCWithAnnotation(XmlToken.Left | XmlToken.Top | XmlToken.XOffset | XmlToken.YOffset | XmlToken.Width | XmlToken.Height | XmlToken.IsExpanded | XmlToken.Author | XmlToken.Text | XmlToken.Ink | XmlToken.ZOrder);
				this.IsDirty = true;
			}
		}

		// Token: 0x060040D3 RID: 16595 RVA: 0x00128040 File Offset: 0x00126240
		private void SetAnnotation(IAttachedAnnotation attachedAnnotation)
		{
			SNCAnnotation sncannotation = new SNCAnnotation(attachedAnnotation.Annotation);
			bool hasInkData = sncannotation.HasInkData;
			bool hasTextData = sncannotation.HasTextData;
			if (hasInkData && hasTextData)
			{
				throw new ArgumentException(SR.Get("InvalidStickyNoteAnnotation"), "attachedAnnotation");
			}
			if (hasInkData)
			{
				this._stickyNoteType = StickyNoteType.Ink;
			}
			else if (hasTextData)
			{
				this._stickyNoteType = StickyNoteType.Text;
			}
			if (this.Content != null)
			{
				this.EnsureStickyNoteType();
			}
			if (sncannotation.IsNewAnnotation)
			{
				AnnotationResource item = new AnnotationResource("Meta Data");
				attachedAnnotation.Annotation.Cargos.Add(item);
			}
			this._attachedAnnotation = attachedAnnotation;
			this._attachedAnnotation.Annotation.CargoChanged += this.OnAnnotationUpdated;
			this._attachedAnnotation.Annotation.AuthorChanged += this.OnAuthorUpdated;
			this._sncAnnotation = sncannotation;
			this._anchor.AddAttachedAnnotation(attachedAnnotation);
			this.UpdateSNCWithAnnotation(XmlToken.Left | XmlToken.Top | XmlToken.XOffset | XmlToken.YOffset | XmlToken.Width | XmlToken.Height | XmlToken.IsExpanded | XmlToken.Author | XmlToken.Text | XmlToken.Ink | XmlToken.ZOrder);
			this.IsDirty = false;
			if ((this._attachedAnnotation.AttachmentLevel & AttachmentLevel.StartPortion) == AttachmentLevel.Unresolved)
			{
				base.SetValue(UIElement.VisibilityProperty, Visibility.Collapsed);
				return;
			}
			base.RequestBringIntoView += this.OnRequestBringIntoView;
		}

		// Token: 0x060040D4 RID: 16596 RVA: 0x00128160 File Offset: 0x00126360
		private void ClearAnnotation()
		{
			this._attachedAnnotation.Annotation.CargoChanged -= this.OnAnnotationUpdated;
			this._attachedAnnotation.Annotation.AuthorChanged -= this.OnAuthorUpdated;
			this._anchor.RemoveAttachedAnnotation(this._attachedAnnotation);
			this._sncAnnotation = null;
			this._attachedAnnotation = null;
			base.RequestBringIntoView -= this.OnRequestBringIntoView;
		}

		// Token: 0x060040D5 RID: 16597 RVA: 0x001281D8 File Offset: 0x001263D8
		private void OnRequestBringIntoView(object sender, RequestBringIntoViewEventArgs e)
		{
			FrameworkElement frameworkElement = ((IAnnotationComponent)this).AnnotatedElement as FrameworkElement;
			DocumentPageHost documentPageHost = frameworkElement as DocumentPageHost;
			if (documentPageHost != null)
			{
				frameworkElement = (documentPageHost.PageVisual as FrameworkElement);
			}
			if (frameworkElement == null)
			{
				return;
			}
			IScrollInfo scrollInfo = frameworkElement as IScrollInfo;
			if (scrollInfo != null)
			{
				Rect stickyNoteBounds = this.StickyNoteBounds;
				Rect rect = new Rect(0.0, 0.0, scrollInfo.ViewportWidth, scrollInfo.ViewportHeight);
				if (stickyNoteBounds.IntersectsWith(rect))
				{
					return;
				}
			}
			Transform transform = (Transform)base.TransformToVisual(frameworkElement);
			Rect rect2 = new Rect(0.0, 0.0, base.Width, base.Height);
			rect2.Transform(transform.Value);
			base.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new DispatcherOperationCallback(this.DispatchBringIntoView), new object[]
			{
				frameworkElement,
				rect2
			});
		}

		// Token: 0x060040D6 RID: 16598 RVA: 0x001282BC File Offset: 0x001264BC
		private object DispatchBringIntoView(object arg)
		{
			object[] array = (object[])arg;
			FrameworkElement frameworkElement = (FrameworkElement)array[0];
			Rect targetRectangle = (Rect)array[1];
			frameworkElement.BringIntoView(targetRectangle);
			return null;
		}

		// Token: 0x060040D7 RID: 16599 RVA: 0x001282EC File Offset: 0x001264EC
		private void UpdateSNCWithAnnotation(XmlToken tokens)
		{
			if (this._sncAnnotation != null)
			{
				EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.UpdateSNCWithAnnotationBegin);
				using (new LockHelper.AutoLocker(this.InternalLocker, LockHelper.LockFlag.DataChanged))
				{
					SNCAnnotation.UpdateStickyNoteControl(tokens, this, this._sncAnnotation);
				}
				EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.UpdateSNCWithAnnotationEnd);
			}
		}

		// Token: 0x060040D8 RID: 16600 RVA: 0x00128350 File Offset: 0x00126550
		private void UpdateAnnotationWithSNC(XmlToken tokens)
		{
			if (this._sncAnnotation != null && !this.InternalLocker.IsLocked(LockHelper.LockFlag.DataChanged))
			{
				EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.UpdateAnnotationWithSNCBegin);
				using (new LockHelper.AutoLocker(this.InternalLocker, LockHelper.LockFlag.AnnotationChanged))
				{
					SNCAnnotation.UpdateAnnotation(tokens, this, this._sncAnnotation);
				}
				EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.UpdateAnnotationWithSNCEnd);
			}
		}

		// Token: 0x060040D9 RID: 16601 RVA: 0x001283C4 File Offset: 0x001265C4
		private void UpdateOffsets()
		{
			if (this._attachedAnnotation != null)
			{
				Rect pageBounds = this.PageBounds;
				Rect stickyNoteBounds = this.StickyNoteBounds;
				if (!pageBounds.IsEmpty && !stickyNoteBounds.IsEmpty)
				{
					Invariant.Assert(DoubleUtil.GreaterThan(stickyNoteBounds.Right, pageBounds.Left), "Note's right is off left of page.");
					Invariant.Assert(DoubleUtil.LessThan(stickyNoteBounds.Left, pageBounds.Right), "Note's left is off right of page.");
					Invariant.Assert(DoubleUtil.GreaterThan(stickyNoteBounds.Bottom, pageBounds.Top), "Note's bottom is off top of page.");
					Invariant.Assert(DoubleUtil.LessThan(stickyNoteBounds.Top, pageBounds.Bottom), "Note's top is off bottom of page.");
					double num;
					double num2;
					StickyNoteControl.GetOffsets(pageBounds, stickyNoteBounds, out num, out num2);
					if (!DoubleUtil.AreClose(this.XOffset, num))
					{
						this.XOffset = num;
					}
					if (!DoubleUtil.AreClose(this.YOffset, num2))
					{
						this.YOffset = num2;
					}
				}
			}
		}

		// Token: 0x060040DA RID: 16602 RVA: 0x001284AC File Offset: 0x001266AC
		private static void GetOffsets(Rect rectPage, Rect rectStickyNote, out double offsetX, out double offsetY)
		{
			offsetX = 0.0;
			if (DoubleUtil.LessThan(rectStickyNote.Left, rectPage.Left))
			{
				offsetX = rectStickyNote.Left - rectPage.Left;
			}
			else if (DoubleUtil.GreaterThan(rectStickyNote.Right, rectPage.Right))
			{
				offsetX = rectStickyNote.Right - rectPage.Right;
			}
			offsetY = 0.0;
			if (DoubleUtil.LessThan(rectStickyNote.Top, rectPage.Top))
			{
				offsetY = rectStickyNote.Top - rectPage.Top;
				return;
			}
			if (DoubleUtil.GreaterThan(rectStickyNote.Bottom, rectPage.Bottom))
			{
				offsetY = rectStickyNote.Bottom - rectPage.Bottom;
			}
		}

		// Token: 0x17000FF0 RID: 4080
		// (get) Token: 0x060040DB RID: 16603 RVA: 0x0012856C File Offset: 0x0012676C
		private Rect StickyNoteBounds
		{
			get
			{
				Rect empty = Rect.Empty;
				Point anchorPoint = this._attachedAnnotation.AnchorPoint;
				if (!double.IsNaN(anchorPoint.X) && !double.IsNaN(anchorPoint.Y) && this.PositionTransform != null)
				{
					empty = new Rect(anchorPoint.X + this.PositionTransform.X + this._deltaX, anchorPoint.Y + this.PositionTransform.Y + this._deltaY, base.Width, base.Height);
				}
				return empty;
			}
		}

		// Token: 0x17000FF1 RID: 4081
		// (get) Token: 0x060040DC RID: 16604 RVA: 0x001285F8 File Offset: 0x001267F8
		private Rect PageBounds
		{
			get
			{
				Rect empty = Rect.Empty;
				IScrollInfo scrollInfo = ((IAnnotationComponent)this).AnnotatedElement as IScrollInfo;
				if (scrollInfo != null)
				{
					empty = new Rect(-scrollInfo.HorizontalOffset, -scrollInfo.VerticalOffset, scrollInfo.ExtentWidth, scrollInfo.ExtentHeight);
				}
				else
				{
					UIElement annotatedElement = ((IAnnotationComponent)this).AnnotatedElement;
					if (annotatedElement != null)
					{
						Size renderSize = annotatedElement.RenderSize;
						empty = new Rect(0.0, 0.0, renderSize.Width, renderSize.Height);
					}
				}
				return empty;
			}
		}

		// Token: 0x060040DD RID: 16605 RVA: 0x0012867C File Offset: 0x0012687C
		static StickyNoteControl()
		{
			Type typeFromHandle = typeof(StickyNoteControl);
			EventManager.RegisterClassHandler(typeFromHandle, Stylus.PreviewStylusDownEvent, new StylusDownEventHandler(StickyNoteControl._OnPreviewDeviceDown<StylusDownEventArgs>));
			EventManager.RegisterClassHandler(typeFromHandle, Mouse.PreviewMouseDownEvent, new MouseButtonEventHandler(StickyNoteControl._OnPreviewDeviceDown<MouseButtonEventArgs>));
			EventManager.RegisterClassHandler(typeFromHandle, Mouse.MouseDownEvent, new MouseButtonEventHandler(StickyNoteControl._OnDeviceDown<MouseButtonEventArgs>));
			EventManager.RegisterClassHandler(typeFromHandle, ContextMenuService.ContextMenuOpeningEvent, new ContextMenuEventHandler(StickyNoteControl._OnContextMenuOpening));
			CommandHelpers.RegisterCommandHandler(typeof(StickyNoteControl), StickyNoteControl.DeleteNoteCommand, new ExecutedRoutedEventHandler(StickyNoteControl._OnCommandExecuted), new CanExecuteRoutedEventHandler(StickyNoteControl._OnQueryCommandEnabled));
			CommandHelpers.RegisterCommandHandler(typeof(StickyNoteControl), StickyNoteControl.InkCommand, new ExecutedRoutedEventHandler(StickyNoteControl._OnCommandExecuted), new CanExecuteRoutedEventHandler(StickyNoteControl._OnQueryCommandEnabled));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeFromHandle, new FrameworkPropertyMetadata(new ComponentResourceKey(typeof(PresentationUIStyleResources), "StickyNoteControlStyleKey")));
			KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeFromHandle, new FrameworkPropertyMetadata(KeyboardNavigationMode.Local));
			Control.IsTabStopProperty.OverrideMetadata(typeFromHandle, new FrameworkPropertyMetadata(false));
			Control.ForegroundProperty.OverrideMetadata(typeFromHandle, new FrameworkPropertyMetadata(new PropertyChangedCallback(StickyNoteControl._UpdateInkDrawingAttributes)));
			Control.FontFamilyProperty.OverrideMetadata(typeFromHandle, new FrameworkPropertyMetadata(new PropertyChangedCallback(StickyNoteControl.OnFontPropertyChanged)));
			Control.FontSizeProperty.OverrideMetadata(typeFromHandle, new FrameworkPropertyMetadata(new PropertyChangedCallback(StickyNoteControl.OnFontPropertyChanged)));
			Control.FontStretchProperty.OverrideMetadata(typeFromHandle, new FrameworkPropertyMetadata(new PropertyChangedCallback(StickyNoteControl.OnFontPropertyChanged)));
			Control.FontStyleProperty.OverrideMetadata(typeFromHandle, new FrameworkPropertyMetadata(new PropertyChangedCallback(StickyNoteControl.OnFontPropertyChanged)));
			Control.FontWeightProperty.OverrideMetadata(typeFromHandle, new FrameworkPropertyMetadata(new PropertyChangedCallback(StickyNoteControl.OnFontPropertyChanged)));
		}

		// Token: 0x060040DE RID: 16606 RVA: 0x00128B24 File Offset: 0x00126D24
		private StickyNoteControl() : this(StickyNoteType.Text)
		{
		}

		// Token: 0x060040DF RID: 16607 RVA: 0x00128B30 File Offset: 0x00126D30
		internal StickyNoteControl(StickyNoteType type)
		{
			this._stickyNoteType = type;
			base.SetValue(StickyNoteControl.StickyNoteTypePropertyKey, type);
			this.InitStickyNoteControl();
		}

		/// <summary>Registers event handlers for all the children of a template.</summary>
		// Token: 0x060040E0 RID: 16608 RVA: 0x00128B80 File Offset: 0x00126D80
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			if (this.IsExpanded)
			{
				this.EnsureStickyNoteType();
			}
			this.UpdateSNCWithAnnotation(XmlToken.Left | XmlToken.Top | XmlToken.XOffset | XmlToken.YOffset | XmlToken.Width | XmlToken.Height | XmlToken.IsExpanded | XmlToken.Author | XmlToken.Text | XmlToken.Ink | XmlToken.ZOrder);
			if (!this.IsExpanded)
			{
				Button iconButton = this.GetIconButton();
				if (iconButton != null)
				{
					iconButton.AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(this.OnButtonClick));
					return;
				}
			}
			else
			{
				Button closeButton = this.GetCloseButton();
				if (closeButton != null)
				{
					closeButton.AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(this.OnButtonClick));
				}
				Thumb titleThumb = this.GetTitleThumb();
				if (titleThumb != null)
				{
					titleThumb.AddHandler(Thumb.DragDeltaEvent, new DragDeltaEventHandler(this.OnDragDelta));
					titleThumb.AddHandler(Thumb.DragCompletedEvent, new DragCompletedEventHandler(this.OnDragCompleted));
				}
				Thumb resizeThumb = this.GetResizeThumb();
				if (resizeThumb != null)
				{
					resizeThumb.AddHandler(Thumb.DragDeltaEvent, new DragDeltaEventHandler(this.OnDragDelta));
					resizeThumb.AddHandler(Thumb.DragCompletedEvent, new DragCompletedEventHandler(this.OnDragCompleted));
				}
				this.SetupMenu();
			}
		}

		/// <summary>Gets the name of the author who created the sticky note.  </summary>
		/// <returns>The name of the author who created the sticky note.</returns>
		// Token: 0x17000FF2 RID: 4082
		// (get) Token: 0x060040E1 RID: 16609 RVA: 0x00128C70 File Offset: 0x00126E70
		public string Author
		{
			get
			{
				return (string)base.GetValue(StickyNoteControl.AuthorProperty);
			}
		}

		/// <summary>Gets or sets a value indicating whether the <see cref="T:System.Windows.Controls.StickyNoteControl" /> is expanded.  </summary>
		/// <returns>
		///     <see langword="true" /> if the control is expanded; otherwise, <see langword="false" />. The default is <see langword="true" />. </returns>
		// Token: 0x17000FF3 RID: 4083
		// (get) Token: 0x060040E2 RID: 16610 RVA: 0x00128C82 File Offset: 0x00126E82
		// (set) Token: 0x060040E3 RID: 16611 RVA: 0x00128C94 File Offset: 0x00126E94
		public bool IsExpanded
		{
			get
			{
				return (bool)base.GetValue(StickyNoteControl.IsExpandedProperty);
			}
			set
			{
				base.SetValue(StickyNoteControl.IsExpandedProperty, value);
			}
		}

		/// <summary>Gets a value indicating whether the <see cref="T:System.Windows.Controls.StickyNoteControl" /> is active.  </summary>
		/// <returns>
		///     <see langword="true" /> if the control is active; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000FF4 RID: 4084
		// (get) Token: 0x060040E4 RID: 16612 RVA: 0x00128CA2 File Offset: 0x00126EA2
		public bool IsActive
		{
			get
			{
				return (bool)base.GetValue(StickyNoteControl.IsActiveProperty);
			}
		}

		/// <summary>Gets a value indicating whether the mouse cursor is over the anchor of the <see cref="T:System.Windows.Controls.StickyNoteControl" />.  </summary>
		/// <returns>
		///     <see langword="true" /> if the mouse cursor is over the anchor; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000FF5 RID: 4085
		// (get) Token: 0x060040E5 RID: 16613 RVA: 0x00128CB4 File Offset: 0x00126EB4
		public bool IsMouseOverAnchor
		{
			get
			{
				return (bool)base.GetValue(StickyNoteControl.IsMouseOverAnchorProperty);
			}
		}

		/// <summary>Gets or sets the font family of the caption for the <see cref="T:System.Windows.Controls.StickyNoteControl" />.  </summary>
		/// <returns>A <see cref="T:System.Drawing.FontFamily" /> for the control's caption. The default is the value of <see cref="P:System.Windows.SystemFonts.MessageFontFamily" />. </returns>
		// Token: 0x17000FF6 RID: 4086
		// (get) Token: 0x060040E6 RID: 16614 RVA: 0x00128CC6 File Offset: 0x00126EC6
		// (set) Token: 0x060040E7 RID: 16615 RVA: 0x00128CD8 File Offset: 0x00126ED8
		public FontFamily CaptionFontFamily
		{
			get
			{
				return (FontFamily)base.GetValue(StickyNoteControl.CaptionFontFamilyProperty);
			}
			set
			{
				base.SetValue(StickyNoteControl.CaptionFontFamilyProperty, value);
			}
		}

		/// <summary>Gets or sets the size of the font used for the caption of the <see cref="T:System.Windows.Controls.StickyNoteControl" />.  </summary>
		/// <returns>A <see cref="T:System.Double" /> representing the font size. The default is the value of <see cref="P:System.Windows.SystemFonts.MessageFontSize" />.</returns>
		// Token: 0x17000FF7 RID: 4087
		// (get) Token: 0x060040E8 RID: 16616 RVA: 0x00128CE6 File Offset: 0x00126EE6
		// (set) Token: 0x060040E9 RID: 16617 RVA: 0x00128CF8 File Offset: 0x00126EF8
		public double CaptionFontSize
		{
			get
			{
				return (double)base.GetValue(StickyNoteControl.CaptionFontSizeProperty);
			}
			set
			{
				base.SetValue(StickyNoteControl.CaptionFontSizeProperty, value);
			}
		}

		/// <summary>Gets or sets the degree to which the font used for the caption of the <see cref="T:System.Windows.Controls.StickyNoteControl" /> is stretched.  </summary>
		/// <returns>A <see cref="T:System.Windows.FontStretch" /> structure representing the degree of stretching compared to a font's normal aspect ratio. The default is <see cref="P:System.Windows.FontStretches.Normal" />. </returns>
		// Token: 0x17000FF8 RID: 4088
		// (get) Token: 0x060040EA RID: 16618 RVA: 0x00128D0B File Offset: 0x00126F0B
		// (set) Token: 0x060040EB RID: 16619 RVA: 0x00128D1D File Offset: 0x00126F1D
		public FontStretch CaptionFontStretch
		{
			get
			{
				return (FontStretch)base.GetValue(StickyNoteControl.CaptionFontStretchProperty);
			}
			set
			{
				base.SetValue(StickyNoteControl.CaptionFontStretchProperty, value);
			}
		}

		/// <summary>Gets or sets the style of the font used for the caption of the <see cref="T:System.Windows.Controls.StickyNoteControl" />.  </summary>
		/// <returns>A <see cref="T:System.Windows.FontStyle" /> structure representing the style of the caption as normal, italic, or oblique. The default is the value of <see cref="P:System.Windows.SystemFonts.MessageFontStyle" />.</returns>
		// Token: 0x17000FF9 RID: 4089
		// (get) Token: 0x060040EC RID: 16620 RVA: 0x00128D30 File Offset: 0x00126F30
		// (set) Token: 0x060040ED RID: 16621 RVA: 0x00128D42 File Offset: 0x00126F42
		public FontStyle CaptionFontStyle
		{
			get
			{
				return (FontStyle)base.GetValue(StickyNoteControl.CaptionFontStyleProperty);
			}
			set
			{
				base.SetValue(StickyNoteControl.CaptionFontStyleProperty, value);
			}
		}

		/// <summary>Gets or sets the weight of the font used for the caption of the <see cref="T:System.Windows.Controls.StickyNoteControl" />.  </summary>
		/// <returns>A <see cref="T:System.Windows.FontWeight" /> structure representing the weight of the font, for example, bold, ultrabold, or extralight. The default is the value of <see cref="P:System.Windows.SystemFonts.MessageFontWeight" />. </returns>
		// Token: 0x17000FFA RID: 4090
		// (get) Token: 0x060040EE RID: 16622 RVA: 0x00128D55 File Offset: 0x00126F55
		// (set) Token: 0x060040EF RID: 16623 RVA: 0x00128D67 File Offset: 0x00126F67
		public FontWeight CaptionFontWeight
		{
			get
			{
				return (FontWeight)base.GetValue(StickyNoteControl.CaptionFontWeightProperty);
			}
			set
			{
				base.SetValue(StickyNoteControl.CaptionFontWeightProperty, value);
			}
		}

		/// <summary>Gets or sets the width of the pen for an ink <see cref="T:System.Windows.Controls.StickyNoteControl" />.  </summary>
		/// <returns>A <see cref="T:System.Double" /> representing the width of the pen. The default is the value of <see cref="P:System.Windows.Ink.DrawingAttributes.Width" />.</returns>
		// Token: 0x17000FFB RID: 4091
		// (get) Token: 0x060040F0 RID: 16624 RVA: 0x00128D7A File Offset: 0x00126F7A
		// (set) Token: 0x060040F1 RID: 16625 RVA: 0x00128D8C File Offset: 0x00126F8C
		public double PenWidth
		{
			get
			{
				return (double)base.GetValue(StickyNoteControl.PenWidthProperty);
			}
			set
			{
				base.SetValue(StickyNoteControl.PenWidthProperty, value);
			}
		}

		/// <summary>Gets a value that indicates whether the sticky note is text or ink.  </summary>
		/// <returns>A <see cref="T:System.Windows.Controls.StickyNoteType" /> value indicating the type of note. The default is <see cref="F:System.Windows.Controls.StickyNoteType.Text" />. </returns>
		// Token: 0x17000FFC RID: 4092
		// (get) Token: 0x060040F2 RID: 16626 RVA: 0x00128D9F File Offset: 0x00126F9F
		public StickyNoteType StickyNoteType
		{
			get
			{
				return (StickyNoteType)base.GetValue(StickyNoteControl.StickyNoteTypeProperty);
			}
		}

		/// <summary>Gets an object that provides information about the annotated object.</summary>
		/// <returns>An <see cref="T:System.Windows.Annotations.IAnchorInfo" /> object that provides information about the annotated object.</returns>
		// Token: 0x17000FFD RID: 4093
		// (get) Token: 0x060040F3 RID: 16627 RVA: 0x00128DB1 File Offset: 0x00126FB1
		public IAnchorInfo AnchorInfo
		{
			get
			{
				if (this._attachedAnnotation != null)
				{
					return this._attachedAnnotation;
				}
				return null;
			}
		}

		// Token: 0x060040F4 RID: 16628 RVA: 0x00128DC3 File Offset: 0x00126FC3
		protected override void OnTemplateChanged(ControlTemplate oldTemplate, ControlTemplate newTemplate)
		{
			base.OnTemplateChanged(oldTemplate, newTemplate);
			this.ClearCachedControls();
		}

		// Token: 0x060040F5 RID: 16629 RVA: 0x00128DD4 File Offset: 0x00126FD4
		protected override void OnIsKeyboardFocusWithinChanged(DependencyPropertyChangedEventArgs args)
		{
			base.OnIsKeyboardFocusWithinChanged(args);
			ContextMenu contextMenu = Keyboard.FocusedElement as ContextMenu;
			if (contextMenu != null && contextMenu.PlacementTarget != null && contextMenu.PlacementTarget.IsDescendantOf(this))
			{
				return;
			}
			this._anchor.Focused = base.IsKeyboardFocusWithin;
		}

		// Token: 0x060040F6 RID: 16630 RVA: 0x00128E20 File Offset: 0x00127020
		protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs args)
		{
			base.OnGotKeyboardFocus(args);
			base.ApplyTemplate();
			if (this.IsExpanded)
			{
				Invariant.Assert(this.Content != null);
				this.BringToFront();
				if (args.NewFocus == this)
				{
					UIElement innerControl = this.Content.InnerControl;
					Invariant.Assert(innerControl != null, "InnerControl is null or not a UIElement.");
					if (!innerControl.IsKeyboardFocused)
					{
						innerControl.Focus();
					}
				}
			}
		}

		// Token: 0x060040F7 RID: 16631 RVA: 0x00128E8C File Offset: 0x0012708C
		private void EnsureStickyNoteType()
		{
			UIElement contentContainer = this.GetContentContainer();
			if (this._contentControl != null)
			{
				if (this._contentControl.Type != this._stickyNoteType)
				{
					this.DisconnectContent();
					this._contentControl = StickyNoteContentControlFactory.CreateContentControl(this._stickyNoteType, contentContainer);
					this.ConnectContent();
					return;
				}
			}
			else
			{
				this._contentControl = StickyNoteContentControlFactory.CreateContentControl(this._stickyNoteType, contentContainer);
				this.ConnectContent();
			}
		}

		// Token: 0x060040F8 RID: 16632 RVA: 0x00128EF2 File Offset: 0x001270F2
		private void DisconnectContent()
		{
			Invariant.Assert(this.Content != null, "Content is null.");
			this.StopListenToContentControlEvent();
			this.UnbindContentControlProperties();
			this._contentControl = null;
		}

		// Token: 0x060040F9 RID: 16633 RVA: 0x00128F1C File Offset: 0x0012711C
		private void ConnectContent()
		{
			Invariant.Assert(this.Content != null);
			InkCanvas inkCanvas = this.Content.InnerControl as InkCanvas;
			if (inkCanvas != null)
			{
				this.InitializeEventHandlers();
				base.SetValue(StickyNoteControl.InkEditingModeProperty, InkCanvasEditingMode.Ink);
				this.UpdateInkDrawingAttributes();
			}
			this.StartListenToContentControlEvent();
			this.BindContentControlProperties();
		}

		// Token: 0x17000FFE RID: 4094
		// (get) Token: 0x060040FA RID: 16634 RVA: 0x00128F74 File Offset: 0x00127174
		internal StickyNoteContentControl Content
		{
			get
			{
				return this._contentControl;
			}
		}

		// Token: 0x060040FB RID: 16635 RVA: 0x00128F7C File Offset: 0x0012717C
		private Button GetCloseButton()
		{
			return base.GetTemplateChild("PART_CloseButton") as Button;
		}

		// Token: 0x060040FC RID: 16636 RVA: 0x00128F8E File Offset: 0x0012718E
		private Button GetIconButton()
		{
			return base.GetTemplateChild("PART_IconButton") as Button;
		}

		// Token: 0x060040FD RID: 16637 RVA: 0x00128FA0 File Offset: 0x001271A0
		private Thumb GetTitleThumb()
		{
			return base.GetTemplateChild("PART_TitleThumb") as Thumb;
		}

		// Token: 0x060040FE RID: 16638 RVA: 0x00128FB2 File Offset: 0x001271B2
		private UIElement GetContentContainer()
		{
			return base.GetTemplateChild("PART_ContentControl") as UIElement;
		}

		// Token: 0x060040FF RID: 16639 RVA: 0x00128FC4 File Offset: 0x001271C4
		private Thumb GetResizeThumb()
		{
			return base.GetTemplateChild("PART_ResizeBottomRightThumb") as Thumb;
		}

		// Token: 0x17000FFF RID: 4095
		// (get) Token: 0x06004100 RID: 16640 RVA: 0x00128FD6 File Offset: 0x001271D6
		// (set) Token: 0x06004101 RID: 16641 RVA: 0x00128FDE File Offset: 0x001271DE
		private bool IsDirty
		{
			get
			{
				return this._dirty;
			}
			set
			{
				this._dirty = value;
			}
		}

		// Token: 0x06004102 RID: 16642 RVA: 0x00128FE8 File Offset: 0x001271E8
		private static void _OnIsExpandedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			StickyNoteControl stickyNoteControl = (StickyNoteControl)d;
			stickyNoteControl.OnIsExpandedChanged();
		}

		// Token: 0x06004103 RID: 16643 RVA: 0x00129004 File Offset: 0x00127204
		private static void OnFontPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			StickyNoteControl stickyNoteControl = (StickyNoteControl)d;
			if (stickyNoteControl.Content != null && stickyNoteControl.Content.Type != StickyNoteType.Ink)
			{
				FrameworkElement innerControl = stickyNoteControl.Content.InnerControl;
				if (innerControl != null)
				{
					innerControl.SetValue(e.Property, e.NewValue);
				}
			}
		}

		// Token: 0x06004104 RID: 16644 RVA: 0x00129054 File Offset: 0x00127254
		private static void _UpdateInkDrawingAttributes(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			StickyNoteControl stickyNoteControl = (StickyNoteControl)d;
			stickyNoteControl.UpdateInkDrawingAttributes();
			if (e.Property == Control.ForegroundProperty && stickyNoteControl.Content != null && stickyNoteControl.Content.Type != StickyNoteType.Ink)
			{
				FrameworkElement innerControl = stickyNoteControl.Content.InnerControl;
				if (innerControl != null)
				{
					innerControl.SetValue(Control.ForegroundProperty, e.NewValue);
				}
			}
		}

		// Token: 0x06004105 RID: 16645 RVA: 0x001290B4 File Offset: 0x001272B4
		private void OnTextChanged(object obj, TextChangedEventArgs args)
		{
			if (!this.InternalLocker.IsLocked(LockHelper.LockFlag.DataChanged))
			{
				EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.AnnotationTextChangedBegin);
				try
				{
					this.AsyncUpdateAnnotation(XmlToken.Text);
					this.IsDirty = true;
				}
				finally
				{
					EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.AnnotationTextChangedEnd);
				}
			}
		}

		// Token: 0x06004106 RID: 16646 RVA: 0x00129114 File Offset: 0x00127314
		private static void _OnDeviceDown<TEventArgs>(object sender, TEventArgs args) where TEventArgs : InputEventArgs
		{
			args.Handled = true;
		}

		// Token: 0x06004107 RID: 16647 RVA: 0x00129122 File Offset: 0x00127322
		private static void _OnContextMenuOpening(object sender, ContextMenuEventArgs args)
		{
			if (!(args.TargetElement is ScrollBar))
			{
				args.Handled = true;
			}
		}

		// Token: 0x06004108 RID: 16648 RVA: 0x00129138 File Offset: 0x00127338
		private static void _OnPreviewDeviceDown<TEventArgs>(object sender, TEventArgs args) where TEventArgs : InputEventArgs
		{
			StickyNoteControl stickyNoteControl = sender as StickyNoteControl;
			IInputElement inputElement = null;
			StylusDevice stylusDevice = args.Device as StylusDevice;
			if (stylusDevice != null)
			{
				inputElement = stylusDevice.Captured;
			}
			else
			{
				MouseDevice mouseDevice = args.Device as MouseDevice;
				if (mouseDevice != null)
				{
					inputElement = mouseDevice.Captured;
				}
			}
			if (stickyNoteControl != null && (inputElement == stickyNoteControl || inputElement == null))
			{
				stickyNoteControl.OnPreviewDeviceDown(sender, args);
			}
		}

		// Token: 0x06004109 RID: 16649 RVA: 0x001291A1 File Offset: 0x001273A1
		private void OnInkCanvasStrokesReplacedEventHandler(object sender, InkCanvasStrokesReplacedEventArgs e)
		{
			this.StopListenToStrokesEvent(e.PreviousStrokes);
			this.StartListenToStrokesEvent(e.NewStrokes);
		}

		// Token: 0x0600410A RID: 16650 RVA: 0x001291BC File Offset: 0x001273BC
		private void OnInkCanvasSelectionMovingEventHandler(object sender, InkCanvasSelectionEditingEventArgs e)
		{
			Rect newRectangle = e.NewRectangle;
			if (newRectangle.X < 0.0 || newRectangle.Y < 0.0)
			{
				newRectangle.X = ((newRectangle.X < 0.0) ? 0.0 : newRectangle.X);
				newRectangle.Y = ((newRectangle.Y < 0.0) ? 0.0 : newRectangle.Y);
				e.NewRectangle = newRectangle;
			}
		}

		// Token: 0x0600410B RID: 16651 RVA: 0x00129254 File Offset: 0x00127454
		private void OnInkCanvasSelectionResizingEventHandler(object sender, InkCanvasSelectionEditingEventArgs e)
		{
			Rect newRectangle = e.NewRectangle;
			if (newRectangle.X < 0.0 || newRectangle.Y < 0.0)
			{
				if (newRectangle.X < 0.0)
				{
					newRectangle.Width += newRectangle.X;
					newRectangle.X = 0.0;
				}
				if (newRectangle.Y < 0.0)
				{
					newRectangle.Height += newRectangle.Y;
					newRectangle.Y = 0.0;
				}
				e.NewRectangle = newRectangle;
			}
		}

		// Token: 0x0600410C RID: 16652 RVA: 0x00129304 File Offset: 0x00127504
		private void OnInkStrokesChanged(object sender, StrokeCollectionChangedEventArgs args)
		{
			this.StopListenToStrokeEvent(args.Removed);
			this.StartListenToStrokeEvent(args.Added);
			if (args.Removed.Count > 0 || args.Added.Count > 0)
			{
				Invariant.Assert(this.Content != null && this.Content.InnerControl is InkCanvas);
				FrameworkElement frameworkElement = VisualTreeHelper.GetParent(this.Content.InnerControl) as FrameworkElement;
				if (frameworkElement != null)
				{
					frameworkElement.InvalidateMeasure();
				}
			}
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.AnnotationInkChangedBegin);
			try
			{
				this.UpdateAnnotationWithSNC(XmlToken.Ink);
				this.IsDirty = true;
			}
			finally
			{
				EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.AnnotationInkChangedEnd);
			}
		}

		// Token: 0x0600410D RID: 16653 RVA: 0x001293C4 File Offset: 0x001275C4
		private void InitStickyNoteControl()
		{
			XmlQualifiedName type = (this._stickyNoteType == StickyNoteType.Text) ? StickyNoteControl.TextSchemaName : StickyNoteControl.InkSchemaName;
			this._anchor = new MarkedHighlightComponent(type, this);
			this.IsDirty = false;
			base.Loaded += this.OnLoadedEventHandler;
		}

		// Token: 0x0600410E RID: 16654 RVA: 0x0012940C File Offset: 0x0012760C
		private void InitializeEventHandlers()
		{
			this._propertyDataChangedHandler = new StickyNoteControl.StrokeChangedHandler<PropertyDataChangedEventArgs>(this);
			this._strokeDrawingAttributesReplacedHandler = new StickyNoteControl.StrokeChangedHandler<DrawingAttributesReplacedEventArgs>(this);
			this._strokePacketDataChangedHandler = new StickyNoteControl.StrokeChangedHandler<EventArgs>(this);
		}

		// Token: 0x0600410F RID: 16655 RVA: 0x00129434 File Offset: 0x00127634
		private void OnButtonClick(object sender, RoutedEventArgs e)
		{
			bool isExpanded = this.IsExpanded;
			base.SetCurrentValueInternal(StickyNoteControl.IsExpandedProperty, BooleanBoxes.Box(!isExpanded));
		}

		// Token: 0x06004110 RID: 16656 RVA: 0x0012945C File Offset: 0x0012765C
		private void DeleteStickyNote()
		{
			Invariant.Assert(this._attachedAnnotation != null, "AttachedAnnotation is null.");
			Invariant.Assert(this._attachedAnnotation.Store != null, "AttachedAnnotation's Store is null.");
			this._attachedAnnotation.Store.DeleteAnnotation(this._attachedAnnotation.Annotation.Id);
		}

		// Token: 0x06004111 RID: 16657 RVA: 0x001294B8 File Offset: 0x001276B8
		private void OnDragCompleted(object sender, DragCompletedEventArgs args)
		{
			Thumb thumb = args.Source as Thumb;
			if (thumb == this.GetTitleThumb())
			{
				this.UpdateAnnotationWithSNC(XmlToken.Left | XmlToken.Top | XmlToken.XOffset | XmlToken.YOffset);
				return;
			}
			if (thumb == this.GetResizeThumb())
			{
				this.UpdateAnnotationWithSNC(XmlToken.Left | XmlToken.Top | XmlToken.XOffset | XmlToken.YOffset | XmlToken.Width | XmlToken.Height);
			}
		}

		// Token: 0x06004112 RID: 16658 RVA: 0x001294F8 File Offset: 0x001276F8
		private void OnDragDelta(object sender, DragDeltaEventArgs args)
		{
			Invariant.Assert(this.IsExpanded, "Dragging occurred when the StickyNoteControl was not expanded.");
			Thumb thumb = args.Source as Thumb;
			double num = args.HorizontalChange;
			if (this._selfMirroring)
			{
				num = -num;
			}
			if (thumb == this.GetTitleThumb())
			{
				this.OnTitleDragDelta(num, args.VerticalChange);
			}
			else if (thumb == this.GetResizeThumb())
			{
				this.OnResizeDragDelta(args.HorizontalChange, args.VerticalChange);
			}
			this.UpdateOffsets();
		}

		// Token: 0x06004113 RID: 16659 RVA: 0x0012956C File Offset: 0x0012776C
		private void OnTitleDragDelta(double horizontalChange, double verticalChange)
		{
			Invariant.Assert(this.IsExpanded);
			Rect stickyNoteBounds = this.StickyNoteBounds;
			Rect pageBounds = this.PageBounds;
			double num = 45.0;
			double num2 = 20.0;
			if (this._selfMirroring)
			{
				double num3 = num2;
				num2 = num;
				num = num3;
			}
			Point point = new Point(-(stickyNoteBounds.X + stickyNoteBounds.Width - num), -stickyNoteBounds.Y);
			Point point2 = new Point(pageBounds.Width - stickyNoteBounds.X - num2, pageBounds.Height - stickyNoteBounds.Y - 20.0);
			horizontalChange = Math.Min(Math.Max(point.X, horizontalChange), point2.X);
			verticalChange = Math.Min(Math.Max(point.Y, verticalChange), point2.Y);
			TranslateTransform positionTransform = this.PositionTransform;
			this.PositionTransform = new TranslateTransform(positionTransform.X + horizontalChange + this._deltaX, positionTransform.Y + verticalChange + this._deltaY);
			this._deltaX = (this._deltaY = 0.0);
			this.IsDirty = true;
		}

		// Token: 0x06004114 RID: 16660 RVA: 0x00129694 File Offset: 0x00127894
		private void OnResizeDragDelta(double horizontalChange, double verticalChange)
		{
			Invariant.Assert(this.IsExpanded);
			Rect stickyNoteBounds = this.StickyNoteBounds;
			double num = stickyNoteBounds.Width + horizontalChange;
			double num2 = stickyNoteBounds.Height + verticalChange;
			if (!this._selfMirroring && stickyNoteBounds.X + num < 45.0)
			{
				num = stickyNoteBounds.Width;
			}
			double minWidth = base.MinWidth;
			double minHeight = base.MinHeight;
			if (num < minWidth)
			{
				num = minWidth;
				horizontalChange = num - base.Width;
			}
			if (num2 < minHeight)
			{
				num2 = minHeight;
			}
			base.SetCurrentValueInternal(FrameworkElement.WidthProperty, num);
			base.SetCurrentValueInternal(FrameworkElement.HeightProperty, num2);
			if (this._selfMirroring)
			{
				this.OnTitleDragDelta(-horizontalChange, 0.0);
			}
			else
			{
				this.OnTitleDragDelta(0.0, 0.0);
			}
			this.IsDirty = true;
		}

		// Token: 0x06004115 RID: 16661 RVA: 0x00129770 File Offset: 0x00127970
		private void OnPreviewDeviceDown(object dc, InputEventArgs args)
		{
			if (this.IsExpanded)
			{
				bool flag = false;
				if (!base.IsKeyboardFocusWithin && this.StickyNoteType == StickyNoteType.Ink)
				{
					Visual visual = args.OriginalSource as Visual;
					if (visual != null)
					{
						Invariant.Assert(this.Content.InnerControl != null, "InnerControl is null.");
						flag = visual.IsDescendantOf(this.Content.InnerControl);
					}
				}
				this.BringToFront();
				if (!this.IsActive || !base.IsKeyboardFocusWithin)
				{
					base.Focus();
				}
				if (flag)
				{
					args.Handled = true;
				}
			}
		}

		// Token: 0x06004116 RID: 16662 RVA: 0x001297F8 File Offset: 0x001279F8
		private void OnLoadedEventHandler(object sender, RoutedEventArgs e)
		{
			if (this.IsExpanded)
			{
				this.UpdateSNCWithAnnotation(XmlToken.Left | XmlToken.Top | XmlToken.XOffset | XmlToken.YOffset | XmlToken.Width | XmlToken.Height);
				if (this._sncAnnotation.IsNewAnnotation)
				{
					base.Focus();
				}
			}
			base.Loaded -= this.OnLoadedEventHandler;
		}

		// Token: 0x06004117 RID: 16663 RVA: 0x00129834 File Offset: 0x00127A34
		private void ClearCachedControls()
		{
			if (this.Content != null)
			{
				this.DisconnectContent();
			}
			Button closeButton = this.GetCloseButton();
			if (closeButton != null)
			{
				closeButton.RemoveHandler(ButtonBase.ClickEvent, new RoutedEventHandler(this.OnButtonClick));
			}
			Button iconButton = this.GetIconButton();
			if (iconButton != null)
			{
				iconButton.RemoveHandler(ButtonBase.ClickEvent, new RoutedEventHandler(this.OnButtonClick));
			}
			Thumb titleThumb = this.GetTitleThumb();
			if (titleThumb != null)
			{
				titleThumb.RemoveHandler(Thumb.DragDeltaEvent, new DragDeltaEventHandler(this.OnDragDelta));
				titleThumb.RemoveHandler(Thumb.DragCompletedEvent, new DragCompletedEventHandler(this.OnDragCompleted));
			}
			Thumb resizeThumb = this.GetResizeThumb();
			if (resizeThumb != null)
			{
				resizeThumb.RemoveHandler(Thumb.DragDeltaEvent, new DragDeltaEventHandler(this.OnDragDelta));
				resizeThumb.RemoveHandler(Thumb.DragCompletedEvent, new DragCompletedEventHandler(this.OnDragCompleted));
			}
		}

		// Token: 0x06004118 RID: 16664 RVA: 0x00129904 File Offset: 0x00127B04
		private void OnIsExpandedChanged()
		{
			this.InvalidateTransform();
			this.UpdateAnnotationWithSNC(XmlToken.IsExpanded);
			this.IsDirty = true;
			if (this.IsExpanded)
			{
				this.BringToFront();
				base.Dispatcher.BeginInvoke(DispatcherPriority.Background, new DispatcherOperationCallback(this.AsyncTakeFocus), null);
				return;
			}
			this.GiveUpFocus();
			this.SendToBack();
		}

		// Token: 0x06004119 RID: 16665 RVA: 0x0012995E File Offset: 0x00127B5E
		private object AsyncTakeFocus(object notUsed)
		{
			base.Focus();
			return null;
		}

		// Token: 0x0600411A RID: 16666 RVA: 0x00129968 File Offset: 0x00127B68
		private void GiveUpFocus()
		{
			if (base.IsKeyboardFocusWithin)
			{
				bool flag = false;
				DependencyObject dependencyObject = this._attachedAnnotation.Parent;
				while (dependencyObject != null && !flag)
				{
					IInputElement inputElement = dependencyObject as IInputElement;
					if (inputElement != null)
					{
						flag = inputElement.Focus();
					}
					if (!flag)
					{
						dependencyObject = FrameworkElement.GetFrameworkParent(dependencyObject);
					}
				}
				if (!flag)
				{
					Keyboard.Focus(null);
				}
			}
		}

		// Token: 0x0600411B RID: 16667 RVA: 0x001299BC File Offset: 0x00127BBC
		private void BringToFront()
		{
			PresentationContext presentationContext = ((IAnnotationComponent)this).PresentationContext;
			if (presentationContext != null)
			{
				presentationContext.BringToFront(this);
			}
		}

		// Token: 0x0600411C RID: 16668 RVA: 0x001299DC File Offset: 0x00127BDC
		private void SendToBack()
		{
			PresentationContext presentationContext = ((IAnnotationComponent)this).PresentationContext;
			if (presentationContext != null)
			{
				presentationContext.SendToBack(this);
			}
		}

		// Token: 0x0600411D RID: 16669 RVA: 0x001299FC File Offset: 0x00127BFC
		private void InvalidateTransform()
		{
			PresentationContext presentationContext = ((IAnnotationComponent)this).PresentationContext;
			if (presentationContext != null)
			{
				presentationContext.InvalidateTransform(this);
			}
		}

		// Token: 0x0600411E RID: 16670 RVA: 0x00129A1A File Offset: 0x00127C1A
		private object AsyncUpdateAnnotation(object arg)
		{
			this.UpdateAnnotationWithSNC((XmlToken)arg);
			return null;
		}

		// Token: 0x0600411F RID: 16671 RVA: 0x00129A2C File Offset: 0x00127C2C
		private void BindContentControlProperties()
		{
			Invariant.Assert(this.Content != null);
			if (this.Content.Type != StickyNoteType.Ink)
			{
				FrameworkElement innerControl = this.Content.InnerControl;
				innerControl.SetValue(Control.FontFamilyProperty, base.GetValue(Control.FontFamilyProperty));
				innerControl.SetValue(Control.FontSizeProperty, base.GetValue(Control.FontSizeProperty));
				innerControl.SetValue(Control.FontStretchProperty, base.GetValue(Control.FontStretchProperty));
				innerControl.SetValue(Control.FontStyleProperty, base.GetValue(Control.FontStyleProperty));
				innerControl.SetValue(Control.FontWeightProperty, base.GetValue(Control.FontWeightProperty));
				innerControl.SetValue(Control.ForegroundProperty, base.GetValue(Control.ForegroundProperty));
				return;
			}
			MultiBinding multiBinding = new MultiBinding();
			multiBinding.Mode = BindingMode.TwoWay;
			multiBinding.Converter = new StickyNoteControl.InkEditingModeIsKeyboardFocusWithin2EditingMode();
			Binding binding = new Binding();
			binding.Mode = BindingMode.TwoWay;
			binding.Path = new PropertyPath(StickyNoteControl.InkEditingModeProperty);
			binding.Source = this;
			multiBinding.Bindings.Add(binding);
			Binding binding2 = new Binding();
			binding2.Path = new PropertyPath(UIElement.IsKeyboardFocusWithinProperty);
			binding2.Source = this;
			multiBinding.Bindings.Add(binding2);
			this.Content.InnerControl.SetBinding(InkCanvas.EditingModeProperty, multiBinding);
		}

		// Token: 0x06004120 RID: 16672 RVA: 0x00129B74 File Offset: 0x00127D74
		private void UnbindContentControlProperties()
		{
			Invariant.Assert(this.Content != null);
			FrameworkElement innerControl = this.Content.InnerControl;
			if (this.Content.Type != StickyNoteType.Ink)
			{
				innerControl.ClearValue(Control.FontFamilyProperty);
				innerControl.ClearValue(Control.FontSizeProperty);
				innerControl.ClearValue(Control.FontStretchProperty);
				innerControl.ClearValue(Control.FontStyleProperty);
				innerControl.ClearValue(Control.FontWeightProperty);
				innerControl.ClearValue(Control.ForegroundProperty);
				return;
			}
			BindingOperations.ClearBinding(innerControl, InkCanvas.EditingModeProperty);
		}

		// Token: 0x06004121 RID: 16673 RVA: 0x00129BF8 File Offset: 0x00127DF8
		private void StartListenToContentControlEvent()
		{
			Invariant.Assert(this.Content != null);
			if (this.Content.Type == StickyNoteType.Ink)
			{
				InkCanvas inkCanvas = this.Content.InnerControl as InkCanvas;
				Invariant.Assert(inkCanvas != null, "InnerControl is not an InkCanvas for note of type Ink.");
				inkCanvas.StrokesReplaced += this.OnInkCanvasStrokesReplacedEventHandler;
				inkCanvas.SelectionMoving += this.OnInkCanvasSelectionMovingEventHandler;
				inkCanvas.SelectionResizing += this.OnInkCanvasSelectionResizingEventHandler;
				this.StartListenToStrokesEvent(inkCanvas.Strokes);
				return;
			}
			TextBoxBase textBoxBase = this.Content.InnerControl as TextBoxBase;
			Invariant.Assert(textBoxBase != null, "InnerControl is not a TextBoxBase for note of type Text.");
			textBoxBase.TextChanged += this.OnTextChanged;
		}

		// Token: 0x06004122 RID: 16674 RVA: 0x00129CB4 File Offset: 0x00127EB4
		private void StopListenToContentControlEvent()
		{
			Invariant.Assert(this.Content != null);
			if (this.Content.Type == StickyNoteType.Ink)
			{
				InkCanvas inkCanvas = this.Content.InnerControl as InkCanvas;
				Invariant.Assert(inkCanvas != null, "InnerControl is not an InkCanvas for note of type Ink.");
				inkCanvas.StrokesReplaced -= this.OnInkCanvasStrokesReplacedEventHandler;
				inkCanvas.SelectionMoving -= this.OnInkCanvasSelectionMovingEventHandler;
				inkCanvas.SelectionResizing -= this.OnInkCanvasSelectionResizingEventHandler;
				this.StopListenToStrokesEvent(inkCanvas.Strokes);
				return;
			}
			TextBoxBase textBoxBase = this.Content.InnerControl as TextBoxBase;
			Invariant.Assert(textBoxBase != null, "InnerControl is not a TextBoxBase for note of type Text.");
			textBoxBase.TextChanged -= this.OnTextChanged;
		}

		// Token: 0x06004123 RID: 16675 RVA: 0x00129D70 File Offset: 0x00127F70
		private void StartListenToStrokesEvent(StrokeCollection strokes)
		{
			strokes.StrokesChanged += this.OnInkStrokesChanged;
			strokes.PropertyDataChanged += this._propertyDataChangedHandler.OnStrokeChanged;
			this.StartListenToStrokeEvent(strokes);
		}

		// Token: 0x06004124 RID: 16676 RVA: 0x00129DA2 File Offset: 0x00127FA2
		private void StopListenToStrokesEvent(StrokeCollection strokes)
		{
			strokes.StrokesChanged -= this.OnInkStrokesChanged;
			strokes.PropertyDataChanged -= this._propertyDataChangedHandler.OnStrokeChanged;
			this.StopListenToStrokeEvent(strokes);
		}

		// Token: 0x06004125 RID: 16677 RVA: 0x00129DD4 File Offset: 0x00127FD4
		private void StartListenToStrokeEvent(IEnumerable<Stroke> strokes)
		{
			foreach (Stroke stroke in strokes)
			{
				stroke.DrawingAttributes.AttributeChanged += this._propertyDataChangedHandler.OnStrokeChanged;
				stroke.DrawingAttributesReplaced += this._strokeDrawingAttributesReplacedHandler.OnStrokeChanged;
				stroke.StylusPointsReplaced += new StylusPointsReplacedEventHandler(this._strokePacketDataChangedHandler.OnStrokeChanged);
				stroke.StylusPoints.Changed += this._strokePacketDataChangedHandler.OnStrokeChanged;
				stroke.PropertyDataChanged += this._propertyDataChangedHandler.OnStrokeChanged;
			}
		}

		// Token: 0x06004126 RID: 16678 RVA: 0x00129E98 File Offset: 0x00128098
		private void StopListenToStrokeEvent(IEnumerable<Stroke> strokes)
		{
			foreach (Stroke stroke in strokes)
			{
				stroke.DrawingAttributes.AttributeChanged -= this._propertyDataChangedHandler.OnStrokeChanged;
				stroke.DrawingAttributesReplaced -= this._strokeDrawingAttributesReplacedHandler.OnStrokeChanged;
				stroke.StylusPointsReplaced -= new StylusPointsReplacedEventHandler(this._strokePacketDataChangedHandler.OnStrokeChanged);
				stroke.StylusPoints.Changed -= this._strokePacketDataChangedHandler.OnStrokeChanged;
				stroke.PropertyDataChanged -= this._propertyDataChangedHandler.OnStrokeChanged;
			}
		}

		// Token: 0x06004127 RID: 16679 RVA: 0x00129F5C File Offset: 0x0012815C
		private void SetupMenu()
		{
			MenuItem inkMenuItem = this.GetInkMenuItem();
			if (inkMenuItem != null)
			{
				Binding binding = new Binding("InkEditingMode");
				binding.Mode = BindingMode.OneWay;
				binding.RelativeSource = RelativeSource.TemplatedParent;
				binding.Converter = new StickyNoteControl.InkEditingModeConverter();
				binding.ConverterParameter = InkCanvasEditingMode.Ink;
				inkMenuItem.SetBinding(MenuItem.IsCheckedProperty, binding);
			}
			MenuItem selectMenuItem = this.GetSelectMenuItem();
			if (selectMenuItem != null)
			{
				Binding binding2 = new Binding("InkEditingMode");
				binding2.Mode = BindingMode.OneWay;
				binding2.RelativeSource = RelativeSource.TemplatedParent;
				binding2.Converter = new StickyNoteControl.InkEditingModeConverter();
				binding2.ConverterParameter = InkCanvasEditingMode.Select;
				selectMenuItem.SetBinding(MenuItem.IsCheckedProperty, binding2);
			}
			MenuItem eraseMenuItem = this.GetEraseMenuItem();
			if (eraseMenuItem != null)
			{
				Binding binding3 = new Binding("InkEditingMode");
				binding3.Mode = BindingMode.OneWay;
				binding3.RelativeSource = RelativeSource.TemplatedParent;
				binding3.Converter = new StickyNoteControl.InkEditingModeConverter();
				binding3.ConverterParameter = InkCanvasEditingMode.EraseByStroke;
				eraseMenuItem.SetBinding(MenuItem.IsCheckedProperty, binding3);
			}
			bool flag = SecurityHelper.CallerHasAllClipboardPermission();
			MenuItem copyMenuItem = this.GetCopyMenuItem();
			if (copyMenuItem != null)
			{
				if (flag)
				{
					copyMenuItem.CommandTarget = this.Content.InnerControl;
				}
				else
				{
					copyMenuItem.Visibility = Visibility.Collapsed;
				}
			}
			MenuItem pasteMenuItem = this.GetPasteMenuItem();
			if (pasteMenuItem != null)
			{
				if (flag)
				{
					pasteMenuItem.CommandTarget = this.Content.InnerControl;
				}
				else
				{
					pasteMenuItem.Visibility = Visibility.Collapsed;
				}
			}
			Separator clipboardSeparator = this.GetClipboardSeparator();
			if (clipboardSeparator != null && !flag)
			{
				clipboardSeparator.Visibility = Visibility.Collapsed;
			}
		}

		// Token: 0x06004128 RID: 16680 RVA: 0x0012A0D0 File Offset: 0x001282D0
		private static void _OnCommandExecuted(object sender, ExecutedRoutedEventArgs args)
		{
			RoutedCommand routedCommand = (RoutedCommand)args.Command;
			StickyNoteControl stickyNoteControl = sender as StickyNoteControl;
			Invariant.Assert(stickyNoteControl != null, "Unexpected Commands");
			Invariant.Assert(routedCommand == StickyNoteControl.DeleteNoteCommand || routedCommand == StickyNoteControl.InkCommand, "Unknown Commands");
			if (routedCommand == StickyNoteControl.DeleteNoteCommand)
			{
				stickyNoteControl.DeleteStickyNote();
				return;
			}
			if (routedCommand == StickyNoteControl.InkCommand)
			{
				StickyNoteContentControl content = stickyNoteControl.Content;
				if (content == null || content.Type != StickyNoteType.Ink)
				{
					throw new InvalidOperationException(SR.Get("CannotProcessInkCommand"));
				}
				InkCanvasEditingMode inkCanvasEditingMode = (InkCanvasEditingMode)args.Parameter;
				stickyNoteControl.SetValue(StickyNoteControl.InkEditingModeProperty, inkCanvasEditingMode);
			}
		}

		// Token: 0x06004129 RID: 16681 RVA: 0x0012A174 File Offset: 0x00128374
		private static void _OnQueryCommandEnabled(object sender, CanExecuteRoutedEventArgs args)
		{
			RoutedCommand routedCommand = (RoutedCommand)args.Command;
			StickyNoteControl stickyNoteControl = sender as StickyNoteControl;
			Invariant.Assert(stickyNoteControl != null, "Unexpected Commands");
			Invariant.Assert(routedCommand == StickyNoteControl.DeleteNoteCommand || routedCommand == StickyNoteControl.InkCommand, "Unknown Commands");
			if (routedCommand == StickyNoteControl.DeleteNoteCommand)
			{
				args.CanExecute = (stickyNoteControl._attachedAnnotation != null);
				return;
			}
			if (routedCommand == StickyNoteControl.InkCommand)
			{
				StickyNoteContentControl content = stickyNoteControl.Content;
				args.CanExecute = (content != null && content.Type == StickyNoteType.Ink);
				return;
			}
			Invariant.Assert(false, "Unknown command.");
		}

		// Token: 0x0600412A RID: 16682 RVA: 0x0012A208 File Offset: 0x00128408
		private void UpdateInkDrawingAttributes()
		{
			if (this.Content == null || this.Content.Type != StickyNoteType.Ink)
			{
				return;
			}
			DrawingAttributes drawingAttributes = new DrawingAttributes();
			SolidColorBrush solidColorBrush = base.Foreground as SolidColorBrush;
			if (solidColorBrush == null)
			{
				throw new ArgumentException(SR.Get("InvalidInkForeground"));
			}
			drawingAttributes.StylusTip = StylusTip.Ellipse;
			drawingAttributes.Width = this.PenWidth;
			drawingAttributes.Height = this.PenWidth;
			drawingAttributes.Color = solidColorBrush.Color;
			((InkCanvas)this.Content.InnerControl).DefaultDrawingAttributes = drawingAttributes;
		}

		// Token: 0x0600412B RID: 16683 RVA: 0x0012A292 File Offset: 0x00128492
		private MenuItem GetInkMenuItem()
		{
			return base.GetTemplateChild("PART_InkMenuItem") as MenuItem;
		}

		// Token: 0x0600412C RID: 16684 RVA: 0x0012A2A4 File Offset: 0x001284A4
		private MenuItem GetSelectMenuItem()
		{
			return base.GetTemplateChild("PART_SelectMenuItem") as MenuItem;
		}

		// Token: 0x0600412D RID: 16685 RVA: 0x0012A2B6 File Offset: 0x001284B6
		private MenuItem GetEraseMenuItem()
		{
			return base.GetTemplateChild("PART_EraseMenuItem") as MenuItem;
		}

		// Token: 0x0600412E RID: 16686 RVA: 0x0012A2C8 File Offset: 0x001284C8
		private MenuItem GetCopyMenuItem()
		{
			return base.GetTemplateChild("PART_CopyMenuItem") as MenuItem;
		}

		// Token: 0x0600412F RID: 16687 RVA: 0x0012A2DA File Offset: 0x001284DA
		private MenuItem GetPasteMenuItem()
		{
			return base.GetTemplateChild("PART_PasteMenuItem") as MenuItem;
		}

		// Token: 0x06004130 RID: 16688 RVA: 0x0012A2EC File Offset: 0x001284EC
		private Separator GetClipboardSeparator()
		{
			return base.GetTemplateChild("PART_ClipboardSeparator") as Separator;
		}

		// Token: 0x17001000 RID: 4096
		// (get) Token: 0x06004131 RID: 16689 RVA: 0x0012A2FE File Offset: 0x001284FE
		private LockHelper InternalLocker
		{
			get
			{
				if (this._lockHelper == null)
				{
					this._lockHelper = new LockHelper();
				}
				return this._lockHelper;
			}
		}

		/// <summary>Gets the Xml type of the text sticky note annotation. </summary>
		/// <returns>An <see cref="T:System.Xml.XmlQualifiedName" /> of the type that a text <see cref="T:System.Windows.Controls.StickyNoteControl" /> instantiates. </returns>
		// Token: 0x04002778 RID: 10104
		public static readonly XmlQualifiedName TextSchemaName = new XmlQualifiedName("TextStickyNote", "http://schemas.microsoft.com/windows/annotations/2003/11/base");

		/// <summary>Gets the Xml type of the ink sticky note annotation. </summary>
		/// <returns>An <see cref="T:System.Xml.XmlQualifiedName" /> of the XML type that an ink <see cref="T:System.Windows.Controls.StickyNoteControl" /> instantiates. </returns>
		// Token: 0x04002779 RID: 10105
		public static readonly XmlQualifiedName InkSchemaName = new XmlQualifiedName("InkStickyNote", "http://schemas.microsoft.com/windows/annotations/2003/11/base");

		// Token: 0x0400277A RID: 10106
		private PresentationContext _presentationContext;

		// Token: 0x0400277B RID: 10107
		private TranslateTransform _positionTransform = new TranslateTransform(0.0, 0.0);

		// Token: 0x0400277C RID: 10108
		private IAttachedAnnotation _attachedAnnotation;

		// Token: 0x0400277D RID: 10109
		private SNCAnnotation _sncAnnotation;

		// Token: 0x0400277E RID: 10110
		private double _offsetX;

		// Token: 0x0400277F RID: 10111
		private double _offsetY;

		// Token: 0x04002780 RID: 10112
		private double _deltaX;

		// Token: 0x04002781 RID: 10113
		private double _deltaY;

		// Token: 0x04002782 RID: 10114
		private int _zOrder;

		// Token: 0x04002783 RID: 10115
		private bool _selfMirroring;

		// Token: 0x04002784 RID: 10116
		internal static readonly DependencyPropertyKey AuthorPropertyKey = DependencyProperty.RegisterReadOnly("Author", typeof(string), typeof(StickyNoteControl), new FrameworkPropertyMetadata(string.Empty));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.StickyNoteControl.Author" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.StickyNoteControl.Author" /> dependency property. </returns>
		// Token: 0x04002785 RID: 10117
		public static readonly DependencyProperty AuthorProperty = StickyNoteControl.AuthorPropertyKey.DependencyProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.StickyNoteControl.IsExpanded" /> dependency property. </summary>
		// Token: 0x04002786 RID: 10118
		public static readonly DependencyProperty IsExpandedProperty = DependencyProperty.Register("IsExpanded", typeof(bool), typeof(StickyNoteControl), new FrameworkPropertyMetadata(BooleanBoxes.TrueBox, new PropertyChangedCallback(StickyNoteControl._OnIsExpandedChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.StickyNoteControl.IsActive" /> dependency property. </summary>
		// Token: 0x04002787 RID: 10119
		public static readonly DependencyProperty IsActiveProperty = DependencyProperty.RegisterAttached("IsActive", typeof(bool), typeof(StickyNoteControl), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.Inherits));

		// Token: 0x04002788 RID: 10120
		internal static readonly DependencyPropertyKey IsMouseOverAnchorPropertyKey = DependencyProperty.RegisterReadOnly("IsMouseOverAnchor", typeof(bool), typeof(StickyNoteControl), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.StickyNoteControl.IsMouseOverAnchor" /> dependency property. </summary>
		// Token: 0x04002789 RID: 10121
		public static readonly DependencyProperty IsMouseOverAnchorProperty = StickyNoteControl.IsMouseOverAnchorPropertyKey.DependencyProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.StickyNoteControl.CaptionFontFamily" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.StickyNoteControl.CaptionFontFamily" /> dependency property.</returns>
		// Token: 0x0400278A RID: 10122
		public static readonly DependencyProperty CaptionFontFamilyProperty = DependencyProperty.Register("CaptionFontFamily", typeof(FontFamily), typeof(StickyNoteControl), new FrameworkPropertyMetadata(SystemFonts.MessageFontFamily, FrameworkPropertyMetadataOptions.AffectsMeasure));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.StickyNoteControl.CaptionFontSize" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.StickyNoteControl.CaptionFontSize" /> dependency property.</returns>
		// Token: 0x0400278B RID: 10123
		public static readonly DependencyProperty CaptionFontSizeProperty = DependencyProperty.Register("CaptionFontSize", typeof(double), typeof(StickyNoteControl), new FrameworkPropertyMetadata(SystemFonts.MessageFontSize, FrameworkPropertyMetadataOptions.AffectsMeasure));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.StickyNoteControl.CaptionFontStretch" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.StickyNoteControl.CaptionFontStretch" /> dependency property.</returns>
		// Token: 0x0400278C RID: 10124
		public static readonly DependencyProperty CaptionFontStretchProperty = DependencyProperty.Register("CaptionFontStretch", typeof(FontStretch), typeof(StickyNoteControl), new FrameworkPropertyMetadata(FontStretches.Normal, FrameworkPropertyMetadataOptions.AffectsMeasure));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.StickyNoteControl.CaptionFontStyle" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.StickyNoteControl.CaptionFontStyle" /> dependency property.</returns>
		// Token: 0x0400278D RID: 10125
		public static readonly DependencyProperty CaptionFontStyleProperty = DependencyProperty.Register("CaptionFontStyle", typeof(FontStyle), typeof(StickyNoteControl), new FrameworkPropertyMetadata(SystemFonts.MessageFontStyle, FrameworkPropertyMetadataOptions.AffectsMeasure));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.StickyNoteControl.CaptionFontWeight" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.StickyNoteControl.CaptionFontWeight" /> dependency property.</returns>
		// Token: 0x0400278E RID: 10126
		public static readonly DependencyProperty CaptionFontWeightProperty = DependencyProperty.Register("CaptionFontWeight", typeof(FontWeight), typeof(StickyNoteControl), new FrameworkPropertyMetadata(SystemFonts.MessageFontWeight, FrameworkPropertyMetadataOptions.AffectsMeasure));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.StickyNoteControl.PenWidth" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.StickyNoteControl.PenWidth" /> dependency property.</returns>
		// Token: 0x0400278F RID: 10127
		public static readonly DependencyProperty PenWidthProperty = DependencyProperty.Register("PenWidth", typeof(double), typeof(StickyNoteControl), new FrameworkPropertyMetadata(new DrawingAttributes().Width, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(StickyNoteControl._UpdateInkDrawingAttributes)));

		// Token: 0x04002790 RID: 10128
		private static readonly DependencyPropertyKey StickyNoteTypePropertyKey = DependencyProperty.RegisterReadOnly("StickyNoteType", typeof(StickyNoteType), typeof(StickyNoteControl), new FrameworkPropertyMetadata(StickyNoteType.Text));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.StickyNoteControl.StickyNoteType" /> dependency property. </summary>
		// Token: 0x04002791 RID: 10129
		public static readonly DependencyProperty StickyNoteTypeProperty = StickyNoteControl.StickyNoteTypePropertyKey.DependencyProperty;

		/// <summary>Represents a command whose <see cref="M:System.Windows.Input.RoutedCommand.Execute(System.Object,System.Windows.IInputElement)" /> method deletes a <see cref="T:System.Windows.Controls.StickyNoteControl" />. </summary>
		/// <returns>A <see cref="T:System.Windows.Input.RoutedCommand" /> that can be used to remove a <see cref="T:System.Windows.Controls.StickyNoteControl" />.</returns>
		// Token: 0x04002792 RID: 10130
		public static readonly RoutedCommand DeleteNoteCommand = new RoutedCommand("DeleteNote", typeof(StickyNoteControl));

		/// <summary>Represents a command whose <see cref="M:System.Windows.Input.RoutedCommand.Execute(System.Object,System.Windows.IInputElement)" /> method will switch the cursor in an ink sticky note to one of several possible modes, including draw and erase. </summary>
		// Token: 0x04002793 RID: 10131
		public static readonly RoutedCommand InkCommand = new RoutedCommand("Ink", typeof(StickyNoteControl));

		// Token: 0x04002794 RID: 10132
		private static readonly DependencyProperty InkEditingModeProperty = DependencyProperty.Register("InkEditingMode", typeof(InkCanvasEditingMode), typeof(StickyNoteControl), new FrameworkPropertyMetadata(InkCanvasEditingMode.None));

		// Token: 0x04002795 RID: 10133
		private LockHelper _lockHelper;

		// Token: 0x04002796 RID: 10134
		private MarkedHighlightComponent _anchor;

		// Token: 0x04002797 RID: 10135
		private bool _dirty;

		// Token: 0x04002798 RID: 10136
		private StickyNoteType _stickyNoteType;

		// Token: 0x04002799 RID: 10137
		private StickyNoteContentControl _contentControl;

		// Token: 0x0400279A RID: 10138
		private StickyNoteControl.StrokeChangedHandler<PropertyDataChangedEventArgs> _propertyDataChangedHandler;

		// Token: 0x0400279B RID: 10139
		private StickyNoteControl.StrokeChangedHandler<DrawingAttributesReplacedEventArgs> _strokeDrawingAttributesReplacedHandler;

		// Token: 0x0400279C RID: 10140
		private StickyNoteControl.StrokeChangedHandler<EventArgs> _strokePacketDataChangedHandler;

		// Token: 0x02000957 RID: 2391
		private class InkEditingModeConverter : IValueConverter
		{
			// Token: 0x0600871D RID: 34589 RVA: 0x0024EEEC File Offset: 0x0024D0EC
			public object Convert(object o, Type type, object parameter, CultureInfo culture)
			{
				InkCanvasEditingMode inkCanvasEditingMode = (InkCanvasEditingMode)parameter;
				InkCanvasEditingMode inkCanvasEditingMode2 = (InkCanvasEditingMode)o;
				if (inkCanvasEditingMode2 == inkCanvasEditingMode)
				{
					return true;
				}
				return DependencyProperty.UnsetValue;
			}

			// Token: 0x0600871E RID: 34590 RVA: 0x0000C238 File Offset: 0x0000A438
			public object ConvertBack(object o, Type type, object parameter, CultureInfo culture)
			{
				return null;
			}
		}

		// Token: 0x02000958 RID: 2392
		private class InkEditingModeIsKeyboardFocusWithin2EditingMode : IMultiValueConverter
		{
			// Token: 0x06008720 RID: 34592 RVA: 0x0024EF18 File Offset: 0x0024D118
			public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
			{
				InkCanvasEditingMode inkCanvasEditingMode = (InkCanvasEditingMode)values[0];
				bool flag = (bool)values[1];
				if (flag)
				{
					return inkCanvasEditingMode;
				}
				return InkCanvasEditingMode.None;
			}

			// Token: 0x06008721 RID: 34593 RVA: 0x0024EF47 File Offset: 0x0024D147
			public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
			{
				return new object[]
				{
					value,
					Binding.DoNothing
				};
			}
		}

		// Token: 0x02000959 RID: 2393
		private class StrokeChangedHandler<TEventArgs>
		{
			// Token: 0x06008723 RID: 34595 RVA: 0x0024EF5B File Offset: 0x0024D15B
			public StrokeChangedHandler(StickyNoteControl snc)
			{
				Invariant.Assert(snc != null);
				this._snc = snc;
			}

			// Token: 0x06008724 RID: 34596 RVA: 0x0024EF73 File Offset: 0x0024D173
			public void OnStrokeChanged(object sender, TEventArgs t)
			{
				this._snc.UpdateAnnotationWithSNC(XmlToken.Ink);
				this._snc._dirty = true;
			}

			// Token: 0x040043F2 RID: 17394
			private StickyNoteControl _snc;
		}
	}
}
