using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Printing;
using System.Security;
using System.Windows.Annotations;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Documents.Serialization;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;
using System.Windows.Xps;
using MS.Internal;
using MS.Internal.Annotations.Anchoring;
using MS.Internal.AppModel;
using MS.Internal.Commands;
using MS.Internal.Controls;
using MS.Internal.Documents;
using MS.Internal.KnownBoxes;

namespace System.Windows.Controls
{
	/// <summary>Provides a control for viewing flow content in a continuous scrolling mode.</summary>
	// Token: 0x020004D2 RID: 1234
	[TemplatePart(Name = "PART_ContentHost", Type = typeof(ScrollViewer))]
	[TemplatePart(Name = "PART_FindToolBarHost", Type = typeof(Decorator))]
	[TemplatePart(Name = "PART_ToolBarHost", Type = typeof(Decorator))]
	[ContentProperty("Document")]
	public class FlowDocumentScrollViewer : Control, IAddChild, IServiceProvider, IJournalState
	{
		// Token: 0x06004B8B RID: 19339 RVA: 0x001547A4 File Offset: 0x001529A4
		static FlowDocumentScrollViewer()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(FlowDocumentScrollViewer), new FrameworkPropertyMetadata(new ComponentResourceKey(typeof(PresentationUIStyleResources), "PUIFlowDocumentScrollViewer")));
			FlowDocumentScrollViewer._dType = DependencyObjectType.FromSystemTypeInternal(typeof(FlowDocumentScrollViewer));
			TextBoxBase.SelectionBrushProperty.OverrideMetadata(typeof(FlowDocumentScrollViewer), new FrameworkPropertyMetadata(new PropertyChangedCallback(FlowDocumentScrollViewer.UpdateCaretElement)));
			TextBoxBase.SelectionOpacityProperty.OverrideMetadata(typeof(FlowDocumentScrollViewer), new FrameworkPropertyMetadata(0.4, new PropertyChangedCallback(FlowDocumentScrollViewer.UpdateCaretElement)));
			FlowDocumentScrollViewer.CreateCommandBindings();
			EventManager.RegisterClassHandler(typeof(FlowDocumentScrollViewer), FrameworkElement.RequestBringIntoViewEvent, new RequestBringIntoViewEventHandler(FlowDocumentScrollViewer.HandleRequestBringIntoView));
			EventManager.RegisterClassHandler(typeof(FlowDocumentScrollViewer), Keyboard.KeyDownEvent, new KeyEventHandler(FlowDocumentScrollViewer.KeyDownHandler), true);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.FlowDocumentScrollViewer" /> class.</summary>
		// Token: 0x06004B8C RID: 19340 RVA: 0x00154B3A File Offset: 0x00152D3A
		public FlowDocumentScrollViewer()
		{
			AnnotationService.SetDataId(this, "FlowDocument");
		}

		/// <summary>Builds the visual tree for the <see cref="T:System.Windows.Controls.FlowDocumentScrollViewer" />.</summary>
		// Token: 0x06004B8D RID: 19341 RVA: 0x00154B50 File Offset: 0x00152D50
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			if (this.FindToolBar != null)
			{
				this.ToggleFindToolBar(false);
			}
			this._findToolBarHost = (base.GetTemplateChild("PART_FindToolBarHost") as Decorator);
			this._toolBarHost = (base.GetTemplateChild("PART_ToolBarHost") as Decorator);
			if (this._toolBarHost != null)
			{
				this._toolBarHost.Visibility = (this.IsToolBarVisible ? Visibility.Visible : Visibility.Collapsed);
			}
			if (this._contentHost != null)
			{
				BindingOperations.ClearBinding(this._contentHost, FlowDocumentScrollViewer.HorizontalScrollBarVisibilityProperty);
				BindingOperations.ClearBinding(this._contentHost, FlowDocumentScrollViewer.VerticalScrollBarVisibilityProperty);
				this._contentHost.ScrollChanged -= this.OnScrollChanged;
				this.RenderScope.Document = null;
				base.ClearValue(TextEditor.PageHeightProperty);
				this._contentHost.Content = null;
			}
			this._contentHost = (base.GetTemplateChild("PART_ContentHost") as ScrollViewer);
			if (this._contentHost != null)
			{
				if (this._contentHost.Content != null)
				{
					throw new NotSupportedException(SR.Get("FlowDocumentScrollViewerMarkedAsContentHostMustHaveNoContent"));
				}
				this._contentHost.ScrollChanged += this.OnScrollChanged;
				this.CreateTwoWayBinding(this._contentHost, FlowDocumentScrollViewer.HorizontalScrollBarVisibilityProperty, "HorizontalScrollBarVisibility");
				this.CreateTwoWayBinding(this._contentHost, FlowDocumentScrollViewer.VerticalScrollBarVisibilityProperty, "VerticalScrollBarVisibility");
				this._contentHost.Focusable = false;
				this._contentHost.Content = new FlowDocumentView();
				this.RenderScope.Document = this.Document;
			}
			this.AttachTextEditor();
			this.ApplyZoom();
		}

		/// <summary>Toggles the Find dialog.</summary>
		// Token: 0x06004B8E RID: 19342 RVA: 0x00154CD9 File Offset: 0x00152ED9
		public void Find()
		{
			this.OnFindCommand();
		}

		/// <summary>Invokes a standard Print dialog which can be used to print the contents of the <see cref="T:System.Windows.Controls.FlowDocumentScrollViewer" /> and configure printing preferences.</summary>
		// Token: 0x06004B8F RID: 19343 RVA: 0x00154CE1 File Offset: 0x00152EE1
		public void Print()
		{
			this.OnPrintCommand();
		}

		/// <summary>Cancels any current printing job.</summary>
		// Token: 0x06004B90 RID: 19344 RVA: 0x00154CE9 File Offset: 0x00152EE9
		public void CancelPrint()
		{
			this.OnCancelPrintCommand();
		}

		/// <summary>Executes the <see cref="P:System.Windows.Input.NavigationCommands.IncreaseZoom" /> routed command.</summary>
		// Token: 0x06004B91 RID: 19345 RVA: 0x00154CF1 File Offset: 0x00152EF1
		public void IncreaseZoom()
		{
			this.OnIncreaseZoomCommand();
		}

		/// <summary>Executes the <see cref="P:System.Windows.Input.NavigationCommands.DecreaseZoom" /> routed command.</summary>
		// Token: 0x06004B92 RID: 19346 RVA: 0x00154CF9 File Offset: 0x00152EF9
		public void DecreaseZoom()
		{
			this.OnDecreaseZoomCommand();
		}

		/// <summary>Gets or sets a <see cref="T:System.Windows.Documents.FlowDocument" /> that hosts the content to be displayed by the <see cref="T:System.Windows.Controls.FlowDocumentScrollViewer" />.  </summary>
		/// <returns>A <see cref="T:System.Windows.Documents.FlowDocument" /> that hosts the content to be displayed by the <see cref="T:System.Windows.Controls.FlowDocumentScrollViewer" />. The default is <see langword="null" />.</returns>
		// Token: 0x17001270 RID: 4720
		// (get) Token: 0x06004B93 RID: 19347 RVA: 0x00154D01 File Offset: 0x00152F01
		// (set) Token: 0x06004B94 RID: 19348 RVA: 0x00154D13 File Offset: 0x00152F13
		public FlowDocument Document
		{
			get
			{
				return (FlowDocument)base.GetValue(FlowDocumentScrollViewer.DocumentProperty);
			}
			set
			{
				base.SetValue(FlowDocumentScrollViewer.DocumentProperty, value);
			}
		}

		/// <summary>Gets the selected content of the <see cref="T:System.Windows.Controls.FlowDocumentScrollViewer" />.</summary>
		/// <returns>The selected content of the <see cref="T:System.Windows.Controls.FlowDocumentScrollViewer" />.</returns>
		// Token: 0x17001271 RID: 4721
		// (get) Token: 0x06004B95 RID: 19349 RVA: 0x00154D24 File Offset: 0x00152F24
		public TextSelection Selection
		{
			get
			{
				ITextSelection textSelection = null;
				FlowDocument document = this.Document;
				if (document != null)
				{
					textSelection = document.StructuralCache.TextContainer.TextSelection;
				}
				return textSelection as TextSelection;
			}
		}

		/// <summary>Gets or sets the current zoom level.  </summary>
		/// <returns>The current zoom level, interpreted as a percentage. The default is 100.0 (a zoom level of 100%).</returns>
		// Token: 0x17001272 RID: 4722
		// (get) Token: 0x06004B96 RID: 19350 RVA: 0x00154D54 File Offset: 0x00152F54
		// (set) Token: 0x06004B97 RID: 19351 RVA: 0x00154D66 File Offset: 0x00152F66
		public double Zoom
		{
			get
			{
				return (double)base.GetValue(FlowDocumentScrollViewer.ZoomProperty);
			}
			set
			{
				base.SetValue(FlowDocumentScrollViewer.ZoomProperty, value);
			}
		}

		/// <summary>Gets or sets the maximum allowable <see cref="P:System.Windows.Controls.FlowDocumentScrollViewer.Zoom" /> level for the <see cref="T:System.Windows.Controls.FlowDocumentScrollViewer" />.  </summary>
		/// <returns>The maximum allowable <see cref="P:System.Windows.Controls.FlowDocumentScrollViewer.Zoom" /> level for the <see cref="T:System.Windows.Controls.FlowDocumentScrollViewer" />, interpreted as a percentage. The default is 200.0 (a maximum zoom of 200%).</returns>
		// Token: 0x17001273 RID: 4723
		// (get) Token: 0x06004B98 RID: 19352 RVA: 0x00154D79 File Offset: 0x00152F79
		// (set) Token: 0x06004B99 RID: 19353 RVA: 0x00154D8B File Offset: 0x00152F8B
		public double MaxZoom
		{
			get
			{
				return (double)base.GetValue(FlowDocumentScrollViewer.MaxZoomProperty);
			}
			set
			{
				base.SetValue(FlowDocumentScrollViewer.MaxZoomProperty, value);
			}
		}

		/// <summary>Gets or sets the minimum allowable <see cref="P:System.Windows.Controls.FlowDocumentScrollViewer.Zoom" /> level for the <see cref="T:System.Windows.Controls.FlowDocumentScrollViewer" />.  </summary>
		/// <returns>The minimum allowable <see cref="P:System.Windows.Controls.FlowDocumentScrollViewer.Zoom" /> level for the <see cref="T:System.Windows.Controls.FlowDocumentScrollViewer" />, interpreted as a percentage. The default is 80.0 (a minimum zoom of 80%).</returns>
		// Token: 0x17001274 RID: 4724
		// (get) Token: 0x06004B9A RID: 19354 RVA: 0x00154D9E File Offset: 0x00152F9E
		// (set) Token: 0x06004B9B RID: 19355 RVA: 0x00154DB0 File Offset: 0x00152FB0
		public double MinZoom
		{
			get
			{
				return (double)base.GetValue(FlowDocumentScrollViewer.MinZoomProperty);
			}
			set
			{
				base.SetValue(FlowDocumentScrollViewer.MinZoomProperty, value);
			}
		}

		/// <summary>Gets or sets the zoom increment.  </summary>
		/// <returns>The current zoom increment, interpreted as a percentage. The default is 10.0 (zoom increments by 10%).</returns>
		// Token: 0x17001275 RID: 4725
		// (get) Token: 0x06004B9C RID: 19356 RVA: 0x00154DC3 File Offset: 0x00152FC3
		// (set) Token: 0x06004B9D RID: 19357 RVA: 0x00154DD5 File Offset: 0x00152FD5
		public double ZoomIncrement
		{
			get
			{
				return (double)base.GetValue(FlowDocumentScrollViewer.ZoomIncrementProperty);
			}
			set
			{
				base.SetValue(FlowDocumentScrollViewer.ZoomIncrementProperty, value);
			}
		}

		/// <summary>Gets a value that indicates whether the <see cref="P:System.Windows.Controls.FlowDocumentScrollViewer.Zoom" /> level can be increased.  </summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="P:System.Windows.Controls.FlowDocumentScrollViewer.Zoom" /> level can be increased; otherwise, <see langword="false" />.</returns>
		// Token: 0x17001276 RID: 4726
		// (get) Token: 0x06004B9E RID: 19358 RVA: 0x00154DE8 File Offset: 0x00152FE8
		public bool CanIncreaseZoom
		{
			get
			{
				return (bool)base.GetValue(FlowDocumentScrollViewer.CanIncreaseZoomProperty);
			}
		}

		/// <summary>Gets a value that indicates whether the <see cref="P:System.Windows.Controls.FlowDocumentScrollViewer.Zoom" /> level can be decreased.  </summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="P:System.Windows.Controls.FlowDocumentScrollViewer.Zoom" /> level can be decreased; otherwise, <see langword="false" />.</returns>
		// Token: 0x17001277 RID: 4727
		// (get) Token: 0x06004B9F RID: 19359 RVA: 0x00154DFA File Offset: 0x00152FFA
		public bool CanDecreaseZoom
		{
			get
			{
				return (bool)base.GetValue(FlowDocumentScrollViewer.CanDecreaseZoomProperty);
			}
		}

		/// <summary>Gets or sets a value that indicates whether selection of content within the <see cref="T:System.Windows.Controls.FlowDocumentScrollViewer" /> is enabled.  </summary>
		/// <returns>
		///     <see langword="true" /> to indicate that selection is enabled; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17001278 RID: 4728
		// (get) Token: 0x06004BA0 RID: 19360 RVA: 0x00154E0C File Offset: 0x0015300C
		// (set) Token: 0x06004BA1 RID: 19361 RVA: 0x00154E1E File Offset: 0x0015301E
		public bool IsSelectionEnabled
		{
			get
			{
				return (bool)base.GetValue(FlowDocumentScrollViewer.IsSelectionEnabledProperty);
			}
			set
			{
				base.SetValue(FlowDocumentScrollViewer.IsSelectionEnabledProperty, value);
			}
		}

		/// <summary>Gets or sets a value that indicates whether the <see cref="T:System.Windows.Controls.FlowDocumentScrollViewer" /> toolbar is visible.  </summary>
		/// <returns>
		///     <see langword="true" /> to indicate that the toolbar is visible; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17001279 RID: 4729
		// (get) Token: 0x06004BA2 RID: 19362 RVA: 0x00154E2C File Offset: 0x0015302C
		// (set) Token: 0x06004BA3 RID: 19363 RVA: 0x00154E3E File Offset: 0x0015303E
		public bool IsToolBarVisible
		{
			get
			{
				return (bool)base.GetValue(FlowDocumentScrollViewer.IsToolBarVisibleProperty);
			}
			set
			{
				base.SetValue(FlowDocumentScrollViewer.IsToolBarVisibleProperty, value);
			}
		}

		/// <summary>Gets or sets a value that indicates whether a horizontal scroll bar is shown.  </summary>
		/// <returns>One of the <see cref="T:System.Windows.Controls.ScrollBarVisibility" /> values. The default is <see cref="F:System.Windows.Controls.ScrollBarVisibility.Auto" /> .</returns>
		// Token: 0x1700127A RID: 4730
		// (get) Token: 0x06004BA4 RID: 19364 RVA: 0x00154E4C File Offset: 0x0015304C
		// (set) Token: 0x06004BA5 RID: 19365 RVA: 0x00154E5E File Offset: 0x0015305E
		public ScrollBarVisibility HorizontalScrollBarVisibility
		{
			get
			{
				return (ScrollBarVisibility)base.GetValue(FlowDocumentScrollViewer.HorizontalScrollBarVisibilityProperty);
			}
			set
			{
				base.SetValue(FlowDocumentScrollViewer.HorizontalScrollBarVisibilityProperty, value);
			}
		}

		/// <summary>Gets or sets a value that indicates whether a vertical scroll bar is shown.  </summary>
		/// <returns>One of the <see cref="T:System.Windows.Controls.ScrollBarVisibility" /> values. The default is <see cref="F:System.Windows.Controls.ScrollBarVisibility.Visible" />.</returns>
		// Token: 0x1700127B RID: 4731
		// (get) Token: 0x06004BA6 RID: 19366 RVA: 0x00154E71 File Offset: 0x00153071
		// (set) Token: 0x06004BA7 RID: 19367 RVA: 0x00154E83 File Offset: 0x00153083
		public ScrollBarVisibility VerticalScrollBarVisibility
		{
			get
			{
				return (ScrollBarVisibility)base.GetValue(FlowDocumentScrollViewer.VerticalScrollBarVisibilityProperty);
			}
			set
			{
				base.SetValue(FlowDocumentScrollViewer.VerticalScrollBarVisibilityProperty, value);
			}
		}

		/// <summary>Gets or sets the brush that highlights the selected text.</summary>
		/// <returns>A brush that highlights the selected text.</returns>
		// Token: 0x1700127C RID: 4732
		// (get) Token: 0x06004BA8 RID: 19368 RVA: 0x00154E96 File Offset: 0x00153096
		// (set) Token: 0x06004BA9 RID: 19369 RVA: 0x00154EA8 File Offset: 0x001530A8
		public Brush SelectionBrush
		{
			get
			{
				return (Brush)base.GetValue(FlowDocumentScrollViewer.SelectionBrushProperty);
			}
			set
			{
				base.SetValue(FlowDocumentScrollViewer.SelectionBrushProperty, value);
			}
		}

		/// <summary>Gets or sets the opacity of the <see cref="P:System.Windows.Controls.FlowDocumentScrollViewer.SelectionBrush" />.</summary>
		/// <returns>The opacity of the <see cref="P:System.Windows.Controls.FlowDocumentScrollViewer.SelectionBrush" />. The default is 0.4.</returns>
		// Token: 0x1700127D RID: 4733
		// (get) Token: 0x06004BAA RID: 19370 RVA: 0x00154EB6 File Offset: 0x001530B6
		// (set) Token: 0x06004BAB RID: 19371 RVA: 0x00154EC8 File Offset: 0x001530C8
		public double SelectionOpacity
		{
			get
			{
				return (double)base.GetValue(FlowDocumentScrollViewer.SelectionOpacityProperty);
			}
			set
			{
				base.SetValue(FlowDocumentScrollViewer.SelectionOpacityProperty, value);
			}
		}

		/// <summary>Gets a value that indicates whether the <see cref="T:System.Windows.Controls.FlowDocumentScrollViewer" /> has focus and selected text.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Controls.FlowDocumentScrollViewer" /> displays selected text when the text box does not have focus; otherwise, <see langword="false" />.The registered default is <see langword="false" />. For more information about what can influence the value, see Dependency Property Value Precedence.</returns>
		// Token: 0x1700127E RID: 4734
		// (get) Token: 0x06004BAC RID: 19372 RVA: 0x00154EDB File Offset: 0x001530DB
		public bool IsSelectionActive
		{
			get
			{
				return (bool)base.GetValue(FlowDocumentScrollViewer.IsSelectionActiveProperty);
			}
		}

		/// <summary>Gets or sets a value that indicates whether <see cref="T:System.Windows.Controls.FlowDocumentScrollViewer" /> displays selected text when the control does not have focus.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Controls.FlowDocumentScrollViewer" /> displays selected text when the <see cref="T:System.Windows.Controls.FlowDocumentScrollViewer" /> does not have focus; otherwise, <see langword="false" />.The registered default is <see langword="false" />. For more information about what can influence the value, see Dependency Property Value Precedence.</returns>
		// Token: 0x1700127F RID: 4735
		// (get) Token: 0x06004BAD RID: 19373 RVA: 0x00154EED File Offset: 0x001530ED
		// (set) Token: 0x06004BAE RID: 19374 RVA: 0x00154EFF File Offset: 0x001530FF
		public bool IsInactiveSelectionHighlightEnabled
		{
			get
			{
				return (bool)base.GetValue(FlowDocumentScrollViewer.IsInactiveSelectionHighlightEnabledProperty);
			}
			set
			{
				base.SetValue(FlowDocumentScrollViewer.IsInactiveSelectionHighlightEnabledProperty, value);
			}
		}

		/// <summary>Called when a printing job has completed.</summary>
		// Token: 0x06004BAF RID: 19375 RVA: 0x00154F0D File Offset: 0x0015310D
		protected virtual void OnPrintCompleted()
		{
			this.ClearPrintingState();
		}

		/// <summary>Handles the <see cref="P:System.Windows.Input.ApplicationCommands.Find" /> routed command.</summary>
		// Token: 0x06004BB0 RID: 19376 RVA: 0x00154F15 File Offset: 0x00153115
		protected virtual void OnFindCommand()
		{
			if (this.CanShowFindToolBar)
			{
				this.ToggleFindToolBar(this.FindToolBar == null);
			}
		}

		/// <summary>Handles the <see cref="P:System.Windows.Input.ApplicationCommands.Print" /> routed command.</summary>
		// Token: 0x06004BB1 RID: 19377 RVA: 0x00154F30 File Offset: 0x00153130
		protected virtual void OnPrintCommand()
		{
			PrintDocumentImageableArea printDocumentImageableArea = null;
			if (this._printingState != null)
			{
				return;
			}
			if (this.Document == null)
			{
				this.OnPrintCompleted();
				return;
			}
			XpsDocumentWriter xpsDocumentWriter = PrintQueue.CreateXpsDocumentWriter(ref printDocumentImageableArea);
			if (xpsDocumentWriter != null && printDocumentImageableArea != null)
			{
				if (this.RenderScope != null)
				{
					this.RenderScope.SuspendLayout();
				}
				FlowDocumentPaginator flowDocumentPaginator = ((IDocumentPaginatorSource)this.Document).DocumentPaginator as FlowDocumentPaginator;
				this._printingState = new FlowDocumentPrintingState();
				this._printingState.XpsDocumentWriter = xpsDocumentWriter;
				this._printingState.PageSize = flowDocumentPaginator.PageSize;
				this._printingState.PagePadding = this.Document.PagePadding;
				this._printingState.IsSelectionEnabled = this.IsSelectionEnabled;
				this._printingState.ColumnWidth = this.Document.ColumnWidth;
				CommandManager.InvalidateRequerySuggested();
				xpsDocumentWriter.WritingCompleted += this.HandlePrintCompleted;
				xpsDocumentWriter.WritingCancelled += this.HandlePrintCancelled;
				if (this._contentHost != null)
				{
					CommandManager.AddPreviewCanExecuteHandler(this._contentHost, new CanExecuteRoutedEventHandler(this.PreviewCanExecuteRoutedEventHandler));
				}
				if (this.IsSelectionEnabled)
				{
					base.SetCurrentValueInternal(FlowDocumentScrollViewer.IsSelectionEnabledProperty, BooleanBoxes.FalseBox);
				}
				flowDocumentPaginator.PageSize = new Size(printDocumentImageableArea.MediaSizeWidth, printDocumentImageableArea.MediaSizeHeight);
				Thickness thickness = this.Document.ComputePageMargin();
				this.Document.PagePadding = new Thickness(Math.Max(printDocumentImageableArea.OriginWidth, thickness.Left), Math.Max(printDocumentImageableArea.OriginHeight, thickness.Top), Math.Max(printDocumentImageableArea.MediaSizeWidth - (printDocumentImageableArea.OriginWidth + printDocumentImageableArea.ExtentWidth), thickness.Right), Math.Max(printDocumentImageableArea.MediaSizeHeight - (printDocumentImageableArea.OriginHeight + printDocumentImageableArea.ExtentHeight), thickness.Bottom));
				this.Document.ColumnWidth = double.PositiveInfinity;
				xpsDocumentWriter.WriteAsync(flowDocumentPaginator);
				return;
			}
			this.OnPrintCompleted();
		}

		/// <summary>Handles the <see cref="P:System.Windows.Input.ApplicationCommands.CancelPrint" /> routed command.</summary>
		// Token: 0x06004BB2 RID: 19378 RVA: 0x00155110 File Offset: 0x00153310
		protected virtual void OnCancelPrintCommand()
		{
			if (this._printingState != null)
			{
				this._printingState.XpsDocumentWriter.CancelAsync();
			}
		}

		/// <summary>Handles the <see cref="P:System.Windows.Input.NavigationCommands.IncreaseZoom" /> routed command.</summary>
		// Token: 0x06004BB3 RID: 19379 RVA: 0x0015512A File Offset: 0x0015332A
		protected virtual void OnIncreaseZoomCommand()
		{
			if (this.CanIncreaseZoom)
			{
				this.Zoom = Math.Min(this.Zoom + this.ZoomIncrement, this.MaxZoom);
			}
		}

		/// <summary>Handles the <see cref="P:System.Windows.Input.NavigationCommands.DecreaseZoom" /> routed command.</summary>
		// Token: 0x06004BB4 RID: 19380 RVA: 0x00155152 File Offset: 0x00153352
		protected virtual void OnDecreaseZoomCommand()
		{
			if (this.CanDecreaseZoom)
			{
				this.Zoom = Math.Max(this.Zoom - this.ZoomIncrement, this.MinZoom);
			}
		}

		/// <summary>Handles the <see cref="E:System.Windows.UIElement.KeyDown" />  routed event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Input.KeyEventArgs" /> object containing the arguments associated with the <see cref="E:System.Windows.UIElement.KeyDown" /> routed event.</param>
		// Token: 0x06004BB5 RID: 19381 RVA: 0x0015517C File Offset: 0x0015337C
		[SecurityCritical]
		[SecurityTreatAsSafe]
		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (e.Handled)
			{
				return;
			}
			Key key = e.Key;
			if (key != Key.Escape)
			{
				if (key == Key.F3)
				{
					if (this.CanShowFindToolBar)
					{
						if (this.FindToolBar != null)
						{
							this.FindToolBar.SearchUp = ((e.KeyboardDevice.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift);
							this.OnFindInvoked(this, EventArgs.Empty);
						}
						else
						{
							this.ToggleFindToolBar(true);
						}
						e.Handled = true;
					}
				}
			}
			else if (this.FindToolBar != null)
			{
				this.ToggleFindToolBar(false);
				e.Handled = true;
			}
			if (!e.Handled)
			{
				base.OnKeyDown(e);
			}
		}

		/// <summary>Handles the <see cref="E:System.Windows.UIElement.MouseWheel" />  routed event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Input.MouseWheelEventArgs" /> object containing arguments associated with the <see cref="E:System.Windows.UIElement.MouseWheel" /> routed event.</param>
		// Token: 0x06004BB6 RID: 19382 RVA: 0x00155214 File Offset: 0x00153414
		protected override void OnMouseWheel(MouseWheelEventArgs e)
		{
			if (e.Handled)
			{
				return;
			}
			if (this._contentHost != null)
			{
				if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
				{
					if (e.Delta > 0 && this.CanIncreaseZoom)
					{
						base.SetCurrentValueInternal(FlowDocumentScrollViewer.ZoomProperty, Math.Min(this.Zoom + this.ZoomIncrement, this.MaxZoom));
					}
					else if (e.Delta < 0 && this.CanDecreaseZoom)
					{
						base.SetCurrentValueInternal(FlowDocumentScrollViewer.ZoomProperty, Math.Max(this.Zoom - this.ZoomIncrement, this.MinZoom));
					}
				}
				else if (e.Delta < 0)
				{
					this._contentHost.LineDown();
				}
				else
				{
					this._contentHost.LineUp();
				}
				e.Handled = true;
			}
			if (!e.Handled)
			{
				base.OnMouseWheel(e);
			}
		}

		/// <summary>Invoked whenever an unhandled <see cref="E:System.Windows.FrameworkElement.ContextMenuOpening" /> routed event reaches this class in its route. Implement this method to add class handling for this event.</summary>
		/// <param name="e">Arguments of the event.</param>
		// Token: 0x06004BB7 RID: 19383 RVA: 0x001552EC File Offset: 0x001534EC
		protected override void OnContextMenuOpening(ContextMenuEventArgs e)
		{
			base.OnContextMenuOpening(e);
			DocumentViewerHelper.OnContextMenuOpening(this.Document, this, e);
		}

		/// <summary>Creates and returns an <see cref="T:System.Windows.Automation.Peers.AutomationPeer" /> object for this <see cref="T:System.Windows.Controls.FlowDocumentScrollViewer" />.</summary>
		/// <returns>An <see cref="T:System.Windows.Automation.Peers.AutomationPeer" /> object for this <see cref="T:System.Windows.Controls.FlowDocumentScrollViewer" />.</returns>
		// Token: 0x06004BB8 RID: 19384 RVA: 0x00155302 File Offset: 0x00153502
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new FlowDocumentScrollViewerAutomationPeer(this);
		}

		/// <summary>Gets an enumerator that can iterate the logical children of the <see cref="T:System.Windows.Controls.FlowDocumentScrollViewer" />.</summary>
		/// <returns>An enumerator for the logical children.</returns>
		// Token: 0x17001280 RID: 4736
		// (get) Token: 0x06004BB9 RID: 19385 RVA: 0x0015530A File Offset: 0x0015350A
		protected internal override IEnumerator LogicalChildren
		{
			get
			{
				if (base.HasLogicalChildren && this.Document != null)
				{
					return new SingleChildEnumerator(this.Document);
				}
				return EmptyEnumerator.Instance;
			}
		}

		// Token: 0x06004BBA RID: 19386 RVA: 0x00155330 File Offset: 0x00153530
		internal override bool BuildRouteCore(EventRoute route, RoutedEventArgs args)
		{
			DependencyObject document = this.Document;
			if (document != null && LogicalTreeHelper.GetParent(document) != this)
			{
				DependencyObject dependencyObject = route.PeekBranchNode() as DependencyObject;
				if (dependencyObject != null && DocumentViewerHelper.IsLogicalDescendent(dependencyObject, document))
				{
					FrameworkElement.AddIntermediateElementsToRoute(LogicalTreeHelper.GetParent(document), route, args, LogicalTreeHelper.GetParent(dependencyObject));
				}
			}
			return base.BuildRouteCore(route, args);
		}

		// Token: 0x06004BBB RID: 19387 RVA: 0x00155384 File Offset: 0x00153584
		internal override bool InvalidateAutomationAncestorsCore(Stack<DependencyObject> branchNodeStack, out bool continuePastCoreTree)
		{
			bool flag = true;
			DependencyObject document = this.Document;
			if (document != null && LogicalTreeHelper.GetParent(document) != this)
			{
				DependencyObject dependencyObject = (branchNodeStack.Count > 0) ? branchNodeStack.Peek() : null;
				if (dependencyObject != null && DocumentViewerHelper.IsLogicalDescendent(dependencyObject, document))
				{
					flag = FrameworkElement.InvalidateAutomationIntermediateElements(LogicalTreeHelper.GetParent(document), LogicalTreeHelper.GetParent(dependencyObject));
				}
			}
			return flag & base.InvalidateAutomationAncestorsCore(branchNodeStack, out continuePastCoreTree);
		}

		// Token: 0x06004BBC RID: 19388 RVA: 0x001553E4 File Offset: 0x001535E4
		internal object BringContentPositionIntoView(object arg)
		{
			ITextPointer textPointer = arg as ITextPointer;
			if (textPointer != null)
			{
				ITextView textView = this.GetTextView();
				if (textView != null && textView.IsValid && textView.RenderScope is IScrollInfo && textPointer.TextContainer == textView.TextContainer)
				{
					if (textView.Contains(textPointer))
					{
						Rect rectangleFromTextPosition = textView.GetRectangleFromTextPosition(textPointer);
						if (rectangleFromTextPosition != Rect.Empty)
						{
							IScrollInfo scrollInfo = (IScrollInfo)textView.RenderScope;
							scrollInfo.SetVerticalOffset(rectangleFromTextPosition.Top + scrollInfo.VerticalOffset);
						}
					}
					else
					{
						base.Dispatcher.BeginInvoke(DispatcherPriority.Background, new DispatcherOperationCallback(this.BringContentPositionIntoView), textPointer);
					}
				}
			}
			return null;
		}

		// Token: 0x17001281 RID: 4737
		// (get) Token: 0x06004BBD RID: 19389 RVA: 0x00155486 File Offset: 0x00153686
		internal ScrollViewer ScrollViewer
		{
			get
			{
				return this._contentHost;
			}
		}

		// Token: 0x17001282 RID: 4738
		// (get) Token: 0x06004BBE RID: 19390 RVA: 0x0015548E File Offset: 0x0015368E
		internal bool CanShowFindToolBar
		{
			get
			{
				return this._findToolBarHost != null && this.Document != null && this._textEditor != null;
			}
		}

		// Token: 0x17001283 RID: 4739
		// (get) Token: 0x06004BBF RID: 19391 RVA: 0x001554AB File Offset: 0x001536AB
		internal bool IsPrinting
		{
			get
			{
				return this._printingState != null;
			}
		}

		// Token: 0x17001284 RID: 4740
		// (get) Token: 0x06004BC0 RID: 19392 RVA: 0x001554B8 File Offset: 0x001536B8
		internal TextPointer ContentPosition
		{
			get
			{
				TextPointer result = null;
				ITextView textView = this.GetTextView();
				if (textView != null && textView.IsValid && textView.RenderScope is IScrollInfo)
				{
					result = (textView.GetTextPositionFromPoint(default(Point), true) as TextPointer);
				}
				return result;
			}
		}

		// Token: 0x06004BC1 RID: 19393 RVA: 0x00155500 File Offset: 0x00153700
		private void ToggleFindToolBar(bool enable)
		{
			Invariant.Assert(enable == (this.FindToolBar == null));
			DocumentViewerHelper.ToggleFindToolBar(this._findToolBarHost, new EventHandler(this.OnFindInvoked), enable);
			if (!this.IsToolBarVisible && this._toolBarHost != null)
			{
				this._toolBarHost.Visibility = (enable ? Visibility.Visible : Visibility.Collapsed);
			}
		}

		// Token: 0x06004BC2 RID: 19394 RVA: 0x00155558 File Offset: 0x00153758
		private void ApplyZoom()
		{
			if (this.RenderScope != null)
			{
				this.RenderScope.LayoutTransform = new ScaleTransform(this.Zoom / 100.0, this.Zoom / 100.0);
			}
		}

		// Token: 0x06004BC3 RID: 19395 RVA: 0x00155594 File Offset: 0x00153794
		private void AttachTextEditor()
		{
			AnnotationService service = AnnotationService.GetService(this);
			bool flag = false;
			if (service != null && service.IsEnabled)
			{
				flag = true;
				service.Disable();
			}
			if (this._textEditor != null)
			{
				this._textEditor.TextContainer.TextView = null;
				this._textEditor.OnDetach();
				this._textEditor = null;
			}
			ITextView textView = null;
			if (this.Document != null)
			{
				textView = this.GetTextView();
				this.Document.StructuralCache.TextContainer.TextView = textView;
			}
			if (this.IsSelectionEnabled && this.Document != null && this.RenderScope != null && this.Document.StructuralCache.TextContainer.TextSelection == null)
			{
				this._textEditor = new TextEditor(this.Document.StructuralCache.TextContainer, this, false);
				this._textEditor.IsReadOnly = !FlowDocumentScrollViewer.IsEditingEnabled;
				this._textEditor.TextView = textView;
			}
			if (service != null && flag)
			{
				service.Enable(service.Store);
			}
			if (this._textEditor == null && this.FindToolBar != null)
			{
				this.ToggleFindToolBar(false);
			}
		}

		// Token: 0x06004BC4 RID: 19396 RVA: 0x001556A5 File Offset: 0x001538A5
		private void OnScrollChanged(object sender, ScrollChangedEventArgs e)
		{
			if (e.OriginalSource == this.ScrollViewer && !DoubleUtil.IsZero(e.ViewportHeightChange))
			{
				base.SetValue(TextEditor.PageHeightProperty, e.ViewportHeight);
			}
		}

		// Token: 0x06004BC5 RID: 19397 RVA: 0x001556D8 File Offset: 0x001538D8
		private void HandlePrintCompleted(object sender, WritingCompletedEventArgs e)
		{
			this.OnPrintCompleted();
		}

		// Token: 0x06004BC6 RID: 19398 RVA: 0x00154F0D File Offset: 0x0015310D
		private void HandlePrintCancelled(object sender, WritingCancelledEventArgs e)
		{
			this.ClearPrintingState();
		}

		// Token: 0x06004BC7 RID: 19399 RVA: 0x001556E0 File Offset: 0x001538E0
		private void ClearPrintingState()
		{
			if (this._printingState != null)
			{
				if (this.RenderScope != null)
				{
					this.RenderScope.ResumeLayout();
				}
				if (this._printingState.IsSelectionEnabled)
				{
					base.SetCurrentValueInternal(FlowDocumentScrollViewer.IsSelectionEnabledProperty, BooleanBoxes.TrueBox);
				}
				if (this._contentHost != null)
				{
					CommandManager.RemovePreviewCanExecuteHandler(this._contentHost, new CanExecuteRoutedEventHandler(this.PreviewCanExecuteRoutedEventHandler));
				}
				this._printingState.XpsDocumentWriter.WritingCompleted -= this.HandlePrintCompleted;
				this._printingState.XpsDocumentWriter.WritingCancelled -= this.HandlePrintCancelled;
				this.Document.PagePadding = this._printingState.PagePadding;
				this.Document.ColumnWidth = this._printingState.ColumnWidth;
				((IDocumentPaginatorSource)this.Document).DocumentPaginator.PageSize = this._printingState.PageSize;
				this._printingState = null;
				CommandManager.InvalidateRequerySuggested();
			}
		}

		// Token: 0x06004BC8 RID: 19400 RVA: 0x001557D4 File Offset: 0x001539D4
		private void HandleRequestBringIntoView(RequestBringIntoViewEventArgs args)
		{
			Rect rect = Rect.Empty;
			if (args != null && args.TargetObject != null && this.Document != null)
			{
				DependencyObject document = this.Document;
				if (args.TargetObject == document)
				{
					if (this._contentHost != null)
					{
						this._contentHost.ScrollToHome();
					}
					args.Handled = true;
				}
				else if (args.TargetObject is UIElement)
				{
					UIElement uielement = (UIElement)args.TargetObject;
					if (this.RenderScope != null && this.RenderScope.IsAncestorOf(uielement))
					{
						rect = args.TargetRect;
						if (rect.IsEmpty)
						{
							rect = new Rect(uielement.RenderSize);
						}
						rect = FlowDocumentScrollViewer.MakeVisible(this.RenderScope, uielement, rect);
						if (!rect.IsEmpty)
						{
							GeneralTransform generalTransform = this.RenderScope.TransformToAncestor(this);
							rect = generalTransform.TransformBounds(rect);
						}
						args.Handled = true;
					}
				}
				else if (args.TargetObject is ContentElement)
				{
					DependencyObject dependencyObject = args.TargetObject;
					while (dependencyObject != null && dependencyObject != document)
					{
						dependencyObject = LogicalTreeHelper.GetParent(dependencyObject);
					}
					if (dependencyObject != null)
					{
						IContentHost icontentHost = this.GetIContentHost();
						if (icontentHost != null)
						{
							ReadOnlyCollection<Rect> rectangles = icontentHost.GetRectangles((ContentElement)args.TargetObject);
							if (rectangles.Count > 0)
							{
								rect = FlowDocumentScrollViewer.MakeVisible(this.RenderScope, (Visual)icontentHost, rectangles[0]);
								if (!rect.IsEmpty)
								{
									GeneralTransform generalTransform2 = this.RenderScope.TransformToAncestor(this);
									rect = generalTransform2.TransformBounds(rect);
								}
							}
						}
						args.Handled = true;
					}
				}
				if (args.Handled)
				{
					if (rect.IsEmpty)
					{
						base.BringIntoView();
						return;
					}
					base.BringIntoView(rect);
				}
			}
		}

		// Token: 0x06004BC9 RID: 19401 RVA: 0x00155980 File Offset: 0x00153B80
		private void DocumentChanged(FlowDocument oldDocument, FlowDocument newDocument)
		{
			if (newDocument != null && newDocument.StructuralCache.TextContainer != null && newDocument.StructuralCache.TextContainer.TextSelection != null)
			{
				throw new ArgumentException(SR.Get("FlowDocumentScrollViewerDocumentBelongsToAnotherFlowDocumentScrollViewerAlready"));
			}
			if (oldDocument != null)
			{
				if (this._documentAsLogicalChild)
				{
					base.RemoveLogicalChild(oldDocument);
				}
				if (this.RenderScope != null)
				{
					this.RenderScope.Document = null;
				}
				oldDocument.ClearValue(PathNode.HiddenParentProperty);
				oldDocument.StructuralCache.ClearUpdateInfo(true);
			}
			if (newDocument != null && LogicalTreeHelper.GetParent(newDocument) != null)
			{
				ContentOperations.SetParent(newDocument, this);
				this._documentAsLogicalChild = false;
			}
			else
			{
				this._documentAsLogicalChild = true;
			}
			if (newDocument != null)
			{
				if (this.RenderScope != null)
				{
					this.RenderScope.Document = newDocument;
				}
				if (this._documentAsLogicalChild)
				{
					base.AddLogicalChild(newDocument);
				}
				newDocument.SetValue(PathNode.HiddenParentProperty, this);
				newDocument.StructuralCache.ClearUpdateInfo(true);
			}
			this.AttachTextEditor();
			if (!this.CanShowFindToolBar && this.FindToolBar != null)
			{
				this.ToggleFindToolBar(false);
			}
			FlowDocumentScrollViewerAutomationPeer flowDocumentScrollViewerAutomationPeer = UIElementAutomationPeer.FromElement(this) as FlowDocumentScrollViewerAutomationPeer;
			if (flowDocumentScrollViewerAutomationPeer != null)
			{
				flowDocumentScrollViewerAutomationPeer.InvalidatePeer();
			}
		}

		// Token: 0x06004BCA RID: 19402 RVA: 0x00155A90 File Offset: 0x00153C90
		private ITextView GetTextView()
		{
			ITextView result = null;
			if (this.RenderScope != null)
			{
				result = (ITextView)((IServiceProvider)this.RenderScope).GetService(typeof(ITextView));
			}
			return result;
		}

		// Token: 0x06004BCB RID: 19403 RVA: 0x00155AC4 File Offset: 0x00153CC4
		private IContentHost GetIContentHost()
		{
			IContentHost result = null;
			if (this.RenderScope != null && VisualTreeHelper.GetChildrenCount(this.RenderScope) > 0)
			{
				result = (VisualTreeHelper.GetChild(this.RenderScope, 0) as IContentHost);
			}
			return result;
		}

		// Token: 0x06004BCC RID: 19404 RVA: 0x00155AFC File Offset: 0x00153CFC
		private void CreateTwoWayBinding(FrameworkElement fe, DependencyProperty dp, string propertyPath)
		{
			fe.SetBinding(dp, new Binding(propertyPath)
			{
				Mode = BindingMode.TwoWay,
				Source = this
			});
		}

		// Token: 0x06004BCD RID: 19405 RVA: 0x00155B28 File Offset: 0x00153D28
		private static void CreateCommandBindings()
		{
			ExecutedRoutedEventHandler executedRoutedEventHandler = new ExecutedRoutedEventHandler(FlowDocumentScrollViewer.ExecutedRoutedEventHandler);
			CanExecuteRoutedEventHandler canExecuteRoutedEventHandler = new CanExecuteRoutedEventHandler(FlowDocumentScrollViewer.CanExecuteRoutedEventHandler);
			FlowDocumentScrollViewer._commandLineDown = new RoutedUICommand(string.Empty, "FDSV_LineDown", typeof(FlowDocumentScrollViewer));
			FlowDocumentScrollViewer._commandLineUp = new RoutedUICommand(string.Empty, "FDSV_LineUp", typeof(FlowDocumentScrollViewer));
			FlowDocumentScrollViewer._commandLineLeft = new RoutedUICommand(string.Empty, "FDSV_LineLeft", typeof(FlowDocumentScrollViewer));
			FlowDocumentScrollViewer._commandLineRight = new RoutedUICommand(string.Empty, "FDSV_LineRight", typeof(FlowDocumentScrollViewer));
			TextEditor.RegisterCommandHandlers(typeof(FlowDocumentScrollViewer), true, !FlowDocumentScrollViewer.IsEditingEnabled, true);
			CommandHelpers.RegisterCommandHandler(typeof(FlowDocumentScrollViewer), ApplicationCommands.Find, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(FlowDocumentScrollViewer), ApplicationCommands.Print, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(FlowDocumentScrollViewer), ApplicationCommands.CancelPrint, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(FlowDocumentScrollViewer), NavigationCommands.PreviousPage, executedRoutedEventHandler, canExecuteRoutedEventHandler, Key.Prior);
			CommandHelpers.RegisterCommandHandler(typeof(FlowDocumentScrollViewer), NavigationCommands.NextPage, executedRoutedEventHandler, canExecuteRoutedEventHandler, Key.Next);
			CommandHelpers.RegisterCommandHandler(typeof(FlowDocumentScrollViewer), NavigationCommands.FirstPage, executedRoutedEventHandler, canExecuteRoutedEventHandler, new KeyGesture(Key.Home), new KeyGesture(Key.Home, ModifierKeys.Control));
			CommandHelpers.RegisterCommandHandler(typeof(FlowDocumentScrollViewer), NavigationCommands.LastPage, executedRoutedEventHandler, canExecuteRoutedEventHandler, new KeyGesture(Key.End), new KeyGesture(Key.End, ModifierKeys.Control));
			CommandHelpers.RegisterCommandHandler(typeof(FlowDocumentScrollViewer), NavigationCommands.IncreaseZoom, executedRoutedEventHandler, canExecuteRoutedEventHandler, new KeyGesture(Key.OemPlus, ModifierKeys.Control));
			CommandHelpers.RegisterCommandHandler(typeof(FlowDocumentScrollViewer), NavigationCommands.DecreaseZoom, executedRoutedEventHandler, canExecuteRoutedEventHandler, new KeyGesture(Key.OemMinus, ModifierKeys.Control));
			CommandHelpers.RegisterCommandHandler(typeof(FlowDocumentScrollViewer), FlowDocumentScrollViewer._commandLineDown, executedRoutedEventHandler, canExecuteRoutedEventHandler, Key.Down);
			CommandHelpers.RegisterCommandHandler(typeof(FlowDocumentScrollViewer), FlowDocumentScrollViewer._commandLineUp, executedRoutedEventHandler, canExecuteRoutedEventHandler, Key.Up);
			CommandHelpers.RegisterCommandHandler(typeof(FlowDocumentScrollViewer), FlowDocumentScrollViewer._commandLineLeft, executedRoutedEventHandler, canExecuteRoutedEventHandler, Key.Left);
			CommandHelpers.RegisterCommandHandler(typeof(FlowDocumentScrollViewer), FlowDocumentScrollViewer._commandLineRight, executedRoutedEventHandler, canExecuteRoutedEventHandler, Key.Right);
		}

		// Token: 0x06004BCE RID: 19406 RVA: 0x00155D40 File Offset: 0x00153F40
		private static void CanExecuteRoutedEventHandler(object target, CanExecuteRoutedEventArgs args)
		{
			FlowDocumentScrollViewer flowDocumentScrollViewer = target as FlowDocumentScrollViewer;
			Invariant.Assert(flowDocumentScrollViewer != null, "Target of QueryEnabledEvent must be FlowDocumentScrollViewer.");
			Invariant.Assert(args != null, "args cannot be null.");
			if (flowDocumentScrollViewer._printingState != null)
			{
				args.CanExecute = (args.Command == ApplicationCommands.CancelPrint);
				return;
			}
			if (args.Command == ApplicationCommands.Find)
			{
				args.CanExecute = flowDocumentScrollViewer.CanShowFindToolBar;
				return;
			}
			if (args.Command == ApplicationCommands.Print)
			{
				args.CanExecute = (flowDocumentScrollViewer.Document != null);
				return;
			}
			if (args.Command == ApplicationCommands.CancelPrint)
			{
				args.CanExecute = false;
				return;
			}
			args.CanExecute = true;
		}

		// Token: 0x06004BCF RID: 19407 RVA: 0x00155DE0 File Offset: 0x00153FE0
		private static void ExecutedRoutedEventHandler(object target, ExecutedRoutedEventArgs args)
		{
			FlowDocumentScrollViewer flowDocumentScrollViewer = target as FlowDocumentScrollViewer;
			Invariant.Assert(flowDocumentScrollViewer != null, "Target of ExecuteEvent must be FlowDocumentScrollViewer.");
			Invariant.Assert(args != null, "args cannot be null.");
			if (args.Command == ApplicationCommands.Find)
			{
				flowDocumentScrollViewer.OnFindCommand();
				return;
			}
			if (args.Command == ApplicationCommands.Print)
			{
				flowDocumentScrollViewer.OnPrintCommand();
				return;
			}
			if (args.Command == ApplicationCommands.CancelPrint)
			{
				flowDocumentScrollViewer.OnCancelPrintCommand();
				return;
			}
			if (args.Command == NavigationCommands.IncreaseZoom)
			{
				flowDocumentScrollViewer.OnIncreaseZoomCommand();
				return;
			}
			if (args.Command == NavigationCommands.DecreaseZoom)
			{
				flowDocumentScrollViewer.OnDecreaseZoomCommand();
				return;
			}
			if (args.Command == FlowDocumentScrollViewer._commandLineDown)
			{
				if (flowDocumentScrollViewer._contentHost != null)
				{
					flowDocumentScrollViewer._contentHost.LineDown();
					return;
				}
			}
			else if (args.Command == FlowDocumentScrollViewer._commandLineUp)
			{
				if (flowDocumentScrollViewer._contentHost != null)
				{
					flowDocumentScrollViewer._contentHost.LineUp();
					return;
				}
			}
			else if (args.Command == FlowDocumentScrollViewer._commandLineLeft)
			{
				if (flowDocumentScrollViewer._contentHost != null)
				{
					flowDocumentScrollViewer._contentHost.LineLeft();
					return;
				}
			}
			else if (args.Command == FlowDocumentScrollViewer._commandLineRight)
			{
				if (flowDocumentScrollViewer._contentHost != null)
				{
					flowDocumentScrollViewer._contentHost.LineRight();
					return;
				}
			}
			else if (args.Command == NavigationCommands.NextPage)
			{
				if (flowDocumentScrollViewer._contentHost != null)
				{
					flowDocumentScrollViewer._contentHost.PageDown();
					return;
				}
			}
			else if (args.Command == NavigationCommands.PreviousPage)
			{
				if (flowDocumentScrollViewer._contentHost != null)
				{
					flowDocumentScrollViewer._contentHost.PageUp();
					return;
				}
			}
			else if (args.Command == NavigationCommands.FirstPage)
			{
				if (flowDocumentScrollViewer._contentHost != null)
				{
					flowDocumentScrollViewer._contentHost.ScrollToHome();
					return;
				}
			}
			else if (args.Command == NavigationCommands.LastPage)
			{
				if (flowDocumentScrollViewer._contentHost != null)
				{
					flowDocumentScrollViewer._contentHost.ScrollToEnd();
					return;
				}
			}
			else
			{
				Invariant.Assert(false, "Command not handled in ExecutedRoutedEventHandler.");
			}
		}

		// Token: 0x06004BD0 RID: 19408 RVA: 0x00155F94 File Offset: 0x00154194
		private void OnFindInvoked(object sender, EventArgs e)
		{
			FindToolBar findToolBar = this.FindToolBar;
			if (findToolBar != null && this._textEditor != null)
			{
				base.Focus();
				ITextRange textRange = DocumentViewerHelper.Find(findToolBar, this._textEditor, this._textEditor.TextView, this._textEditor.TextView);
				if (textRange == null || textRange.IsEmpty)
				{
					DocumentViewerHelper.ShowFindUnsuccessfulMessage(findToolBar);
				}
			}
		}

		// Token: 0x06004BD1 RID: 19409 RVA: 0x00155FF0 File Offset: 0x001541F0
		private void PreviewCanExecuteRoutedEventHandler(object target, CanExecuteRoutedEventArgs args)
		{
			ScrollViewer scrollViewer = target as ScrollViewer;
			Invariant.Assert(scrollViewer != null, "Target of PreviewCanExecuteRoutedEventHandler must be ScrollViewer.");
			Invariant.Assert(args != null, "args cannot be null.");
			if (this._printingState != null)
			{
				args.CanExecute = false;
				args.Handled = true;
			}
		}

		// Token: 0x06004BD2 RID: 19410 RVA: 0x00156036 File Offset: 0x00154236
		private static void KeyDownHandler(object sender, KeyEventArgs e)
		{
			DocumentViewerHelper.KeyDownHelper(e, ((FlowDocumentScrollViewer)sender)._findToolBarHost);
		}

		// Token: 0x06004BD3 RID: 19411 RVA: 0x0015604C File Offset: 0x0015424C
		private static Rect MakeVisible(IScrollInfo scrollInfo, Visual visual, Rect rectangle)
		{
			Rect result;
			if (scrollInfo.GetType() == typeof(ScrollContentPresenter))
			{
				result = ((ScrollContentPresenter)scrollInfo).MakeVisible(visual, rectangle, false);
			}
			else
			{
				result = scrollInfo.MakeVisible(visual, rectangle);
			}
			return result;
		}

		// Token: 0x06004BD4 RID: 19412 RVA: 0x0015608B File Offset: 0x0015428B
		private static void DocumentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Invariant.Assert(d != null && d is FlowDocumentScrollViewer);
			((FlowDocumentScrollViewer)d).DocumentChanged((FlowDocument)e.OldValue, (FlowDocument)e.NewValue);
			CommandManager.InvalidateRequerySuggested();
		}

		// Token: 0x06004BD5 RID: 19413 RVA: 0x001560CC File Offset: 0x001542CC
		private static void ZoomChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Invariant.Assert(d != null && d is FlowDocumentScrollViewer);
			FlowDocumentScrollViewer flowDocumentScrollViewer = (FlowDocumentScrollViewer)d;
			if (!DoubleUtil.AreClose((double)e.OldValue, (double)e.NewValue))
			{
				flowDocumentScrollViewer.SetValue(FlowDocumentScrollViewer.CanIncreaseZoomPropertyKey, BooleanBoxes.Box(DoubleUtil.GreaterThan(flowDocumentScrollViewer.MaxZoom, flowDocumentScrollViewer.Zoom)));
				flowDocumentScrollViewer.SetValue(FlowDocumentScrollViewer.CanDecreaseZoomPropertyKey, BooleanBoxes.Box(DoubleUtil.LessThan(flowDocumentScrollViewer.MinZoom, flowDocumentScrollViewer.Zoom)));
				flowDocumentScrollViewer.ApplyZoom();
			}
		}

		// Token: 0x06004BD6 RID: 19414 RVA: 0x0015615C File Offset: 0x0015435C
		private static object CoerceZoom(DependencyObject d, object value)
		{
			Invariant.Assert(d != null && d is FlowDocumentScrollViewer);
			FlowDocumentScrollViewer flowDocumentScrollViewer = (FlowDocumentScrollViewer)d;
			double value2 = (double)value;
			double maxZoom = flowDocumentScrollViewer.MaxZoom;
			if (DoubleUtil.LessThan(maxZoom, value2))
			{
				return maxZoom;
			}
			double minZoom = flowDocumentScrollViewer.MinZoom;
			if (DoubleUtil.GreaterThan(minZoom, value2))
			{
				return minZoom;
			}
			return value;
		}

		// Token: 0x06004BD7 RID: 19415 RVA: 0x001561BC File Offset: 0x001543BC
		private static void MaxZoomChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Invariant.Assert(d != null && d is FlowDocumentScrollViewer);
			FlowDocumentScrollViewer flowDocumentScrollViewer = (FlowDocumentScrollViewer)d;
			flowDocumentScrollViewer.CoerceValue(FlowDocumentScrollViewer.ZoomProperty);
			flowDocumentScrollViewer.SetValue(FlowDocumentScrollViewer.CanIncreaseZoomPropertyKey, BooleanBoxes.Box(DoubleUtil.GreaterThan(flowDocumentScrollViewer.MaxZoom, flowDocumentScrollViewer.Zoom)));
		}

		// Token: 0x06004BD8 RID: 19416 RVA: 0x00156210 File Offset: 0x00154410
		private static object CoerceMaxZoom(DependencyObject d, object value)
		{
			Invariant.Assert(d != null && d is FlowDocumentScrollViewer);
			FlowDocumentScrollViewer flowDocumentScrollViewer = (FlowDocumentScrollViewer)d;
			double minZoom = flowDocumentScrollViewer.MinZoom;
			if ((double)value >= minZoom)
			{
				return value;
			}
			return minZoom;
		}

		// Token: 0x06004BD9 RID: 19417 RVA: 0x00156250 File Offset: 0x00154450
		private static void MinZoomChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Invariant.Assert(d != null && d is FlowDocumentScrollViewer);
			FlowDocumentScrollViewer flowDocumentScrollViewer = (FlowDocumentScrollViewer)d;
			flowDocumentScrollViewer.CoerceValue(FlowDocumentScrollViewer.MaxZoomProperty);
			flowDocumentScrollViewer.CoerceValue(FlowDocumentScrollViewer.ZoomProperty);
			flowDocumentScrollViewer.SetValue(FlowDocumentScrollViewer.CanDecreaseZoomPropertyKey, BooleanBoxes.Box(DoubleUtil.LessThan(flowDocumentScrollViewer.MinZoom, flowDocumentScrollViewer.Zoom)));
		}

		// Token: 0x06004BDA RID: 19418 RVA: 0x001562B0 File Offset: 0x001544B0
		private static bool ZoomValidateValue(object o)
		{
			double num = (double)o;
			return !double.IsNaN(num) && !double.IsInfinity(num) && DoubleUtil.GreaterThan(num, 0.0);
		}

		// Token: 0x06004BDB RID: 19419 RVA: 0x001562E5 File Offset: 0x001544E5
		private static void HandleRequestBringIntoView(object sender, RequestBringIntoViewEventArgs args)
		{
			if (sender != null && sender is FlowDocumentScrollViewer)
			{
				((FlowDocumentScrollViewer)sender).HandleRequestBringIntoView(args);
			}
		}

		// Token: 0x06004BDC RID: 19420 RVA: 0x00156300 File Offset: 0x00154500
		private static void IsSelectionEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Invariant.Assert(d != null && d is FlowDocumentScrollViewer);
			FlowDocumentScrollViewer flowDocumentScrollViewer = (FlowDocumentScrollViewer)d;
			flowDocumentScrollViewer.AttachTextEditor();
		}

		// Token: 0x06004BDD RID: 19421 RVA: 0x00156330 File Offset: 0x00154530
		private static void IsToolBarVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Invariant.Assert(d != null && d is FlowDocumentScrollViewer);
			FlowDocumentScrollViewer flowDocumentScrollViewer = (FlowDocumentScrollViewer)d;
			if (flowDocumentScrollViewer._toolBarHost != null)
			{
				flowDocumentScrollViewer._toolBarHost.Visibility = (((bool)e.NewValue) ? Visibility.Visible : Visibility.Collapsed);
			}
		}

		// Token: 0x06004BDE RID: 19422 RVA: 0x00156380 File Offset: 0x00154580
		private static void UpdateCaretElement(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			FlowDocumentScrollViewer flowDocumentScrollViewer = (FlowDocumentScrollViewer)d;
			if (flowDocumentScrollViewer.Selection != null)
			{
				CaretElement caretElement = flowDocumentScrollViewer.Selection.CaretElement;
				if (caretElement != null)
				{
					caretElement.InvalidateVisual();
				}
			}
		}

		// Token: 0x17001285 RID: 4741
		// (get) Token: 0x06004BDF RID: 19423 RVA: 0x001563B1 File Offset: 0x001545B1
		private FindToolBar FindToolBar
		{
			get
			{
				if (this._findToolBarHost == null)
				{
					return null;
				}
				return this._findToolBarHost.Child as FindToolBar;
			}
		}

		// Token: 0x17001286 RID: 4742
		// (get) Token: 0x06004BE0 RID: 19424 RVA: 0x001563CD File Offset: 0x001545CD
		private FlowDocumentView RenderScope
		{
			get
			{
				if (this._contentHost == null)
				{
					return null;
				}
				return this._contentHost.Content as FlowDocumentView;
			}
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code. Use the <see cref="P:System.Windows.Controls.FlowDocumentScrollViewer.Document" /> property to add a <see cref="T:System.Windows.Documents.FlowDocument" /> as the content child for the <see cref="T:System.Windows.Controls.FlowDocumentScrollViewer" />.</summary>
		/// <param name="value">An object to add as a child.</param>
		// Token: 0x06004BE1 RID: 19425 RVA: 0x001563EC File Offset: 0x001545EC
		void IAddChild.AddChild(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (this.Document != null)
			{
				throw new ArgumentException(SR.Get("FlowDocumentScrollViewerCanHaveOnlyOneChild"));
			}
			if (!(value is FlowDocument))
			{
				throw new ArgumentException(SR.Get("UnexpectedParameterType", new object[]
				{
					value.GetType(),
					typeof(FlowDocument)
				}), "value");
			}
			this.Document = (value as FlowDocument);
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <param name="text">A string to add to the object.</param>
		// Token: 0x06004BE2 RID: 19426 RVA: 0x0000B31C File Offset: 0x0000951C
		void IAddChild.AddText(string text)
		{
			XamlSerializerUtil.ThrowIfNonWhiteSpaceInAddText(text, this);
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <param name="serviceType">An object that specifies the type of service object to get.</param>
		/// <returns>A service object of type <paramref name="serviceType" />, or <see langword="null" /> if there is no service object of type <paramref name="serviceType" />.</returns>
		// Token: 0x06004BE3 RID: 19427 RVA: 0x00156464 File Offset: 0x00154664
		object IServiceProvider.GetService(Type serviceType)
		{
			object result = null;
			if (serviceType == null)
			{
				throw new ArgumentNullException("serviceType");
			}
			if (serviceType == typeof(ITextView))
			{
				result = this.GetTextView();
			}
			else if ((serviceType == typeof(TextContainer) || serviceType == typeof(ITextContainer)) && this.Document != null)
			{
				result = ((IServiceProvider)this.Document).GetService(serviceType);
			}
			return result;
		}

		// Token: 0x06004BE4 RID: 19428 RVA: 0x001564DC File Offset: 0x001546DC
		CustomJournalStateInternal IJournalState.GetJournalState(JournalReason journalReason)
		{
			int contentPosition = -1;
			LogicalDirection contentPositionDirection = LogicalDirection.Forward;
			TextPointer contentPosition2 = this.ContentPosition;
			if (contentPosition2 != null)
			{
				contentPosition = contentPosition2.Offset;
				contentPositionDirection = contentPosition2.LogicalDirection;
			}
			return new FlowDocumentScrollViewer.JournalState(contentPosition, contentPositionDirection, this.Zoom);
		}

		// Token: 0x06004BE5 RID: 19429 RVA: 0x00156514 File Offset: 0x00154714
		void IJournalState.RestoreJournalState(CustomJournalStateInternal state)
		{
			FlowDocumentScrollViewer.JournalState journalState = state as FlowDocumentScrollViewer.JournalState;
			if (state != null)
			{
				this.Zoom = journalState.Zoom;
				if (journalState.ContentPosition != -1)
				{
					FlowDocument document = this.Document;
					if (document != null)
					{
						TextContainer textContainer = document.StructuralCache.TextContainer;
						if (journalState.ContentPosition <= textContainer.SymbolCount)
						{
							TextPointer arg = textContainer.CreatePointerAtOffset(journalState.ContentPosition, journalState.ContentPositionDirection);
							base.Dispatcher.BeginInvoke(DispatcherPriority.Input, new DispatcherOperationCallback(this.BringContentPositionIntoView), arg);
						}
					}
				}
			}
		}

		// Token: 0x17001287 RID: 4743
		// (get) Token: 0x06004BE6 RID: 19430 RVA: 0x00156591 File Offset: 0x00154791
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return FlowDocumentScrollViewer._dType;
			}
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.FlowDocumentScrollViewer.Document" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.FlowDocumentScrollViewer.Document" /> dependency property.</returns>
		// Token: 0x04002AE3 RID: 10979
		public static readonly DependencyProperty DocumentProperty = DependencyProperty.Register("Document", typeof(FlowDocument), typeof(FlowDocumentScrollViewer), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(FlowDocumentScrollViewer.DocumentChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.FlowDocumentScrollViewer.Zoom" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.FlowDocumentScrollViewer.Zoom" /> dependency property.</returns>
		// Token: 0x04002AE4 RID: 10980
		public static readonly DependencyProperty ZoomProperty = FlowDocumentPageViewer.ZoomProperty.AddOwner(typeof(FlowDocumentScrollViewer), new FrameworkPropertyMetadata(100.0, new PropertyChangedCallback(FlowDocumentScrollViewer.ZoomChanged), new CoerceValueCallback(FlowDocumentScrollViewer.CoerceZoom)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.FlowDocumentScrollViewer.MaxZoom" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.FlowDocumentScrollViewer.MaxZoom" /> dependency property.</returns>
		// Token: 0x04002AE5 RID: 10981
		public static readonly DependencyProperty MaxZoomProperty = FlowDocumentPageViewer.MaxZoomProperty.AddOwner(typeof(FlowDocumentScrollViewer), new FrameworkPropertyMetadata(200.0, new PropertyChangedCallback(FlowDocumentScrollViewer.MaxZoomChanged), new CoerceValueCallback(FlowDocumentScrollViewer.CoerceMaxZoom)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.FlowDocumentScrollViewer.MinZoom" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.FlowDocumentScrollViewer.MinZoom" /> dependency property.</returns>
		// Token: 0x04002AE6 RID: 10982
		public static readonly DependencyProperty MinZoomProperty = FlowDocumentPageViewer.MinZoomProperty.AddOwner(typeof(FlowDocumentScrollViewer), new FrameworkPropertyMetadata(80.0, new PropertyChangedCallback(FlowDocumentScrollViewer.MinZoomChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.FlowDocumentScrollViewer.ZoomIncrement" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.FlowDocumentScrollViewer.ZoomIncrement" /> dependency property.</returns>
		// Token: 0x04002AE7 RID: 10983
		public static readonly DependencyProperty ZoomIncrementProperty = FlowDocumentPageViewer.ZoomIncrementProperty.AddOwner(typeof(FlowDocumentScrollViewer));

		// Token: 0x04002AE8 RID: 10984
		private static readonly DependencyPropertyKey CanIncreaseZoomPropertyKey = DependencyProperty.RegisterReadOnly("CanIncreaseZoom", typeof(bool), typeof(FlowDocumentScrollViewer), new FrameworkPropertyMetadata(BooleanBoxes.TrueBox));

		/// <summary>Identifies the <see cref="F:System.Windows.Controls.FlowDocumentScrollViewer.CanDecreaseZoomProperty" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.FlowDocumentScrollViewer.CanIncreaseZoom" /> dependency property.</returns>
		// Token: 0x04002AE9 RID: 10985
		public static readonly DependencyProperty CanIncreaseZoomProperty = FlowDocumentScrollViewer.CanIncreaseZoomPropertyKey.DependencyProperty;

		// Token: 0x04002AEA RID: 10986
		private static readonly DependencyPropertyKey CanDecreaseZoomPropertyKey = DependencyProperty.RegisterReadOnly("CanDecreaseZoom", typeof(bool), typeof(FlowDocumentScrollViewer), new FrameworkPropertyMetadata(BooleanBoxes.TrueBox));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.FlowDocumentScrollViewer.CanDecreaseZoom" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.FlowDocumentScrollViewer.CanDecreaseZoom" /> dependency property.</returns>
		// Token: 0x04002AEB RID: 10987
		public static readonly DependencyProperty CanDecreaseZoomProperty = FlowDocumentScrollViewer.CanDecreaseZoomPropertyKey.DependencyProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.FlowDocumentScrollViewer.IsSelectionEnabled" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.FlowDocumentScrollViewer.IsSelectionEnabled" /> dependency property.</returns>
		// Token: 0x04002AEC RID: 10988
		public static readonly DependencyProperty IsSelectionEnabledProperty = DependencyProperty.Register("IsSelectionEnabled", typeof(bool), typeof(FlowDocumentScrollViewer), new FrameworkPropertyMetadata(BooleanBoxes.TrueBox, new PropertyChangedCallback(FlowDocumentScrollViewer.IsSelectionEnabledChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.FlowDocumentScrollViewer.IsToolBarVisible" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.FlowDocumentScrollViewer.IsToolBarVisible" /> dependency property.</returns>
		// Token: 0x04002AED RID: 10989
		public static readonly DependencyProperty IsToolBarVisibleProperty = DependencyProperty.Register("IsToolBarVisible", typeof(bool), typeof(FlowDocumentScrollViewer), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, new PropertyChangedCallback(FlowDocumentScrollViewer.IsToolBarVisibleChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.FlowDocumentScrollViewer.HorizontalScrollBarVisibility" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.FlowDocumentScrollViewer.HorizontalScrollBarVisibility" /> dependency property.</returns>
		// Token: 0x04002AEE RID: 10990
		public static readonly DependencyProperty HorizontalScrollBarVisibilityProperty = ScrollViewer.HorizontalScrollBarVisibilityProperty.AddOwner(typeof(FlowDocumentScrollViewer), new FrameworkPropertyMetadata(ScrollBarVisibility.Auto));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.FlowDocumentScrollViewer.VerticalScrollBarVisibility" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.FlowDocumentScrollViewer.VerticalScrollBarVisibility" /> dependency property.</returns>
		// Token: 0x04002AEF RID: 10991
		public static readonly DependencyProperty VerticalScrollBarVisibilityProperty = ScrollViewer.VerticalScrollBarVisibilityProperty.AddOwner(typeof(FlowDocumentScrollViewer), new FrameworkPropertyMetadata(ScrollBarVisibility.Visible));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.FlowDocumentScrollViewer.SelectionBrush" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.FlowDocumentScrollViewer.SelectionBrush" /> dependency property.</returns>
		// Token: 0x04002AF0 RID: 10992
		public static readonly DependencyProperty SelectionBrushProperty = TextBoxBase.SelectionBrushProperty.AddOwner(typeof(FlowDocumentScrollViewer));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.FlowDocumentScrollViewer.SelectionOpacity" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.FlowDocumentScrollViewer.SelectionOpacity" /> dependency property.</returns>
		// Token: 0x04002AF1 RID: 10993
		public static readonly DependencyProperty SelectionOpacityProperty = TextBoxBase.SelectionOpacityProperty.AddOwner(typeof(FlowDocumentScrollViewer));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.FlowDocumentScrollViewer.IsSelectionActive" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.FlowDocumentScrollViewer.IsSelectionActive" /> dependency property.</returns>
		// Token: 0x04002AF2 RID: 10994
		public static readonly DependencyProperty IsSelectionActiveProperty = TextBoxBase.IsSelectionActiveProperty.AddOwner(typeof(FlowDocumentScrollViewer));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.FlowDocumentScrollViewer.IsInactiveSelectionHighlightEnabled" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.FlowDocumentScrollViewer.IsInactiveSelectionHighlightEnabled" /> dependency property.</returns>
		// Token: 0x04002AF3 RID: 10995
		public static readonly DependencyProperty IsInactiveSelectionHighlightEnabledProperty = TextBoxBase.IsInactiveSelectionHighlightEnabledProperty.AddOwner(typeof(FlowDocumentScrollViewer));

		// Token: 0x04002AF4 RID: 10996
		private TextEditor _textEditor;

		// Token: 0x04002AF5 RID: 10997
		private Decorator _findToolBarHost;

		// Token: 0x04002AF6 RID: 10998
		private Decorator _toolBarHost;

		// Token: 0x04002AF7 RID: 10999
		private ScrollViewer _contentHost;

		// Token: 0x04002AF8 RID: 11000
		private bool _documentAsLogicalChild;

		// Token: 0x04002AF9 RID: 11001
		private FlowDocumentPrintingState _printingState;

		// Token: 0x04002AFA RID: 11002
		private const string _contentHostTemplateName = "PART_ContentHost";

		// Token: 0x04002AFB RID: 11003
		private const string _findToolBarHostTemplateName = "PART_FindToolBarHost";

		// Token: 0x04002AFC RID: 11004
		private const string _toolBarHostTemplateName = "PART_ToolBarHost";

		// Token: 0x04002AFD RID: 11005
		private static bool IsEditingEnabled = false;

		// Token: 0x04002AFE RID: 11006
		private static RoutedUICommand _commandLineDown;

		// Token: 0x04002AFF RID: 11007
		private static RoutedUICommand _commandLineUp;

		// Token: 0x04002B00 RID: 11008
		private static RoutedUICommand _commandLineLeft;

		// Token: 0x04002B01 RID: 11009
		private static RoutedUICommand _commandLineRight;

		// Token: 0x04002B02 RID: 11010
		private static DependencyObjectType _dType;

		// Token: 0x02000972 RID: 2418
		[Serializable]
		private class JournalState : CustomJournalStateInternal
		{
			// Token: 0x0600877E RID: 34686 RVA: 0x0025010B File Offset: 0x0024E30B
			public JournalState(int contentPosition, LogicalDirection contentPositionDirection, double zoom)
			{
				this.ContentPosition = contentPosition;
				this.ContentPositionDirection = contentPositionDirection;
				this.Zoom = zoom;
			}

			// Token: 0x0400444D RID: 17485
			public int ContentPosition;

			// Token: 0x0400444E RID: 17486
			public LogicalDirection ContentPositionDirection;

			// Token: 0x0400444F RID: 17487
			public double Zoom;
		}
	}
}
