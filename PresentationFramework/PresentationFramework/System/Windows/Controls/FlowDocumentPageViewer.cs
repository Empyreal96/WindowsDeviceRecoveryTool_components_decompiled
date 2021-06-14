using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Printing;
using System.Security;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Documents.Serialization;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using System.Windows.Xps;
using MS.Internal;
using MS.Internal.AppModel;
using MS.Internal.Commands;
using MS.Internal.Documents;
using MS.Internal.KnownBoxes;

namespace System.Windows.Controls
{
	/// <summary>Represents a control for viewing flow content in a fixed viewing mode that shows content one page at a time.</summary>
	// Token: 0x02000526 RID: 1318
	[TemplatePart(Name = "PART_FindToolBarHost", Type = typeof(Decorator))]
	public class FlowDocumentPageViewer : DocumentViewerBase, IJournalState
	{
		// Token: 0x0600556C RID: 21868 RVA: 0x0017ADA0 File Offset: 0x00178FA0
		static FlowDocumentPageViewer()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(FlowDocumentPageViewer), new FrameworkPropertyMetadata(new ComponentResourceKey(typeof(PresentationUIStyleResources), "PUIFlowDocumentPageViewer")));
			FlowDocumentPageViewer._dType = DependencyObjectType.FromSystemTypeInternal(typeof(FlowDocumentPageViewer));
			TextBoxBase.SelectionBrushProperty.OverrideMetadata(typeof(FlowDocumentPageViewer), new FrameworkPropertyMetadata(new PropertyChangedCallback(FlowDocumentPageViewer.UpdateCaretElement)));
			TextBoxBase.SelectionOpacityProperty.OverrideMetadata(typeof(FlowDocumentPageViewer), new FrameworkPropertyMetadata(0.4, new PropertyChangedCallback(FlowDocumentPageViewer.UpdateCaretElement)));
			FlowDocumentPageViewer.CreateCommandBindings();
			EventManager.RegisterClassHandler(typeof(FlowDocumentPageViewer), Keyboard.KeyDownEvent, new KeyEventHandler(FlowDocumentPageViewer.KeyDownHandler), true);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.FlowDocumentPageViewer" /> class.</summary>
		// Token: 0x0600556D RID: 21869 RVA: 0x0017B08E File Offset: 0x0017928E
		public FlowDocumentPageViewer()
		{
			base.LayoutUpdated += this.HandleLayoutUpdated;
		}

		/// <summary>Builds the visual tree for the <see cref="T:System.Windows.Controls.FlowDocumentPageViewer" />.</summary>
		// Token: 0x0600556E RID: 21870 RVA: 0x0017B0B3 File Offset: 0x001792B3
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			if (this.FindToolBar != null)
			{
				this.ToggleFindToolBar(false);
			}
			this._findToolBarHost = (base.GetTemplateChild("PART_FindToolBarHost") as Decorator);
		}

		/// <summary>Executes the <see cref="P:System.Windows.Input.NavigationCommands.IncreaseZoom" /> routed command.</summary>
		// Token: 0x0600556F RID: 21871 RVA: 0x0017B0E0 File Offset: 0x001792E0
		public void IncreaseZoom()
		{
			this.OnIncreaseZoomCommand();
		}

		/// <summary>Executes the <see cref="P:System.Windows.Input.NavigationCommands.DecreaseZoom" /> routed command.</summary>
		// Token: 0x06005570 RID: 21872 RVA: 0x0017B0E8 File Offset: 0x001792E8
		public void DecreaseZoom()
		{
			this.OnDecreaseZoomCommand();
		}

		/// <summary>Toggles the Find dialog.</summary>
		// Token: 0x06005571 RID: 21873 RVA: 0x0017B0F0 File Offset: 0x001792F0
		public void Find()
		{
			this.OnFindCommand();
		}

		/// <summary>Gets the selected content of the <see cref="T:System.Windows.Controls.FlowDocumentPageViewer" />.</summary>
		/// <returns>The selected content of the <see cref="T:System.Windows.Controls.FlowDocumentPageViewer" />.</returns>
		// Token: 0x170014CA RID: 5322
		// (get) Token: 0x06005572 RID: 21874 RVA: 0x0017B0F8 File Offset: 0x001792F8
		public TextSelection Selection
		{
			get
			{
				ITextSelection textSelection = null;
				FlowDocument flowDocument = base.Document as FlowDocument;
				if (flowDocument != null)
				{
					textSelection = flowDocument.StructuralCache.TextContainer.TextSelection;
				}
				return textSelection as TextSelection;
			}
		}

		/// <summary>Gets or sets the current zoom level for the <see cref="T:System.Windows.Controls.FlowDocumentPageViewer" />. </summary>
		/// <returns>The current zoom level, interpreted as a percentage. The default is 100.0 (a zoom level of 100%).</returns>
		// Token: 0x170014CB RID: 5323
		// (get) Token: 0x06005573 RID: 21875 RVA: 0x0017B12D File Offset: 0x0017932D
		// (set) Token: 0x06005574 RID: 21876 RVA: 0x0017B13F File Offset: 0x0017933F
		public double Zoom
		{
			get
			{
				return (double)base.GetValue(FlowDocumentPageViewer.ZoomProperty);
			}
			set
			{
				base.SetValue(FlowDocumentPageViewer.ZoomProperty, value);
			}
		}

		/// <summary>Gets or sets the maximum allowable <see cref="P:System.Windows.Controls.FlowDocumentPageViewer.Zoom" /> level for the <see cref="T:System.Windows.Controls.FlowDocumentPageViewer" />. </summary>
		/// <returns>The maximum allowable zoom level for the <see cref="T:System.Windows.Controls.FlowDocumentPageViewer" />, interpreted as a percentage. The default is 200.0 (a maximum zoom of 200%).</returns>
		// Token: 0x170014CC RID: 5324
		// (get) Token: 0x06005575 RID: 21877 RVA: 0x0017B152 File Offset: 0x00179352
		// (set) Token: 0x06005576 RID: 21878 RVA: 0x0017B164 File Offset: 0x00179364
		public double MaxZoom
		{
			get
			{
				return (double)base.GetValue(FlowDocumentPageViewer.MaxZoomProperty);
			}
			set
			{
				base.SetValue(FlowDocumentPageViewer.MaxZoomProperty, value);
			}
		}

		/// <summary>Gets or sets the minimum allowable <see cref="P:System.Windows.Controls.FlowDocumentPageViewer.Zoom" /> level for the <see cref="T:System.Windows.Controls.FlowDocumentPageViewer" />. </summary>
		/// <returns>The minimum allowable zoom level for the <see cref="T:System.Windows.Controls.FlowDocumentPageViewer" />, interpreted as a percentage. The default is 80.0 (a minimum zoom of 80%).</returns>
		// Token: 0x170014CD RID: 5325
		// (get) Token: 0x06005577 RID: 21879 RVA: 0x0017B177 File Offset: 0x00179377
		// (set) Token: 0x06005578 RID: 21880 RVA: 0x0017B189 File Offset: 0x00179389
		public double MinZoom
		{
			get
			{
				return (double)base.GetValue(FlowDocumentPageViewer.MinZoomProperty);
			}
			set
			{
				base.SetValue(FlowDocumentPageViewer.MinZoomProperty, value);
			}
		}

		/// <summary>Gets or sets the zoom increment. </summary>
		/// <returns>The current zoom increment, interpreted as a percentage. The default is 10.0 (zoom increments by 10%).</returns>
		// Token: 0x170014CE RID: 5326
		// (get) Token: 0x06005579 RID: 21881 RVA: 0x0017B19C File Offset: 0x0017939C
		// (set) Token: 0x0600557A RID: 21882 RVA: 0x0017B1AE File Offset: 0x001793AE
		public double ZoomIncrement
		{
			get
			{
				return (double)base.GetValue(FlowDocumentPageViewer.ZoomIncrementProperty);
			}
			set
			{
				base.SetValue(FlowDocumentPageViewer.ZoomIncrementProperty, value);
			}
		}

		/// <summary>Gets a value that indicates whether the <see cref="P:System.Windows.Controls.FlowDocumentPageViewer.Zoom" /> level can be increased. </summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="P:System.Windows.Controls.FlowDocumentPageViewer.Zoom" /> level can be increased; otherwise, <see langword="false" />.</returns>
		// Token: 0x170014CF RID: 5327
		// (get) Token: 0x0600557B RID: 21883 RVA: 0x0017B1C1 File Offset: 0x001793C1
		public virtual bool CanIncreaseZoom
		{
			get
			{
				return (bool)base.GetValue(FlowDocumentPageViewer.CanIncreaseZoomProperty);
			}
		}

		/// <summary>Gets a value that indicates whether the <see cref="P:System.Windows.Controls.FlowDocumentPageViewer.Zoom" /> level can be decreased. </summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="P:System.Windows.Controls.FlowDocumentPageViewer.Zoom" /> level can be decreased; otherwise, <see langword="false" />.</returns>
		// Token: 0x170014D0 RID: 5328
		// (get) Token: 0x0600557C RID: 21884 RVA: 0x0017B1D3 File Offset: 0x001793D3
		public virtual bool CanDecreaseZoom
		{
			get
			{
				return (bool)base.GetValue(FlowDocumentPageViewer.CanDecreaseZoomProperty);
			}
		}

		/// <summary>Gets or sets the brush that highlights the selected text.</summary>
		/// <returns>A brush that highlights the selected text.</returns>
		// Token: 0x170014D1 RID: 5329
		// (get) Token: 0x0600557D RID: 21885 RVA: 0x0017B1E5 File Offset: 0x001793E5
		// (set) Token: 0x0600557E RID: 21886 RVA: 0x0017B1F7 File Offset: 0x001793F7
		public Brush SelectionBrush
		{
			get
			{
				return (Brush)base.GetValue(FlowDocumentPageViewer.SelectionBrushProperty);
			}
			set
			{
				base.SetValue(FlowDocumentPageViewer.SelectionBrushProperty, value);
			}
		}

		/// <summary>Gets or sets the opacity of the <see cref="P:System.Windows.Controls.FlowDocumentPageViewer.SelectionBrush" />.</summary>
		/// <returns>The opacity of the <see cref="P:System.Windows.Controls.FlowDocumentPageViewer.SelectionBrush" />. The default is 0.4.</returns>
		// Token: 0x170014D2 RID: 5330
		// (get) Token: 0x0600557F RID: 21887 RVA: 0x0017B205 File Offset: 0x00179405
		// (set) Token: 0x06005580 RID: 21888 RVA: 0x0017B217 File Offset: 0x00179417
		public double SelectionOpacity
		{
			get
			{
				return (double)base.GetValue(FlowDocumentPageViewer.SelectionOpacityProperty);
			}
			set
			{
				base.SetValue(FlowDocumentPageViewer.SelectionOpacityProperty, value);
			}
		}

		/// <summary>Gets a value that indicates whether the <see cref="T:System.Windows.Controls.FlowDocumentPageViewer" /> has focus and selected text.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Controls.FlowDocumentPageViewer" /> displays selected text when the text box does not have focus; otherwise, <see langword="false" />.The registered default is <see langword="false" />. For more information about what can influence the value, see Dependency Property Value Precedence.</returns>
		// Token: 0x170014D3 RID: 5331
		// (get) Token: 0x06005581 RID: 21889 RVA: 0x0017B22A File Offset: 0x0017942A
		public bool IsSelectionActive
		{
			get
			{
				return (bool)base.GetValue(FlowDocumentPageViewer.IsSelectionActiveProperty);
			}
		}

		/// <summary>Gets or sets a value that indicates whether <see cref="T:System.Windows.Controls.FlowDocumentPageViewer" /> displays selected text when the control does not have focus.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Controls.FlowDocumentPageViewer" /> displays selected text when the <see cref="T:System.Windows.Controls.FlowDocumentPageViewer" /> does not have focus; otherwise, <see langword="false" />.The registered default is <see langword="false" />. For more information about what can influence the value, see Dependency Property Value Precedence.</returns>
		// Token: 0x170014D4 RID: 5332
		// (get) Token: 0x06005582 RID: 21890 RVA: 0x0017B23C File Offset: 0x0017943C
		// (set) Token: 0x06005583 RID: 21891 RVA: 0x0017B24E File Offset: 0x0017944E
		public bool IsInactiveSelectionHighlightEnabled
		{
			get
			{
				return (bool)base.GetValue(FlowDocumentPageViewer.IsInactiveSelectionHighlightEnabledProperty);
			}
			set
			{
				base.SetValue(FlowDocumentPageViewer.IsInactiveSelectionHighlightEnabledProperty, value);
			}
		}

		/// <summary>Provides an appropriate <see cref="T:System.Windows.Automation.Peers.AutomationPeer" /> implementation for this control, as part of the WPF automation infrastructure.</summary>
		/// <returns>The appropriate <see cref="T:System.Windows.Automation.Peers.AutomationPeer" /> implementation for this control.</returns>
		// Token: 0x06005584 RID: 21892 RVA: 0x0017B25C File Offset: 0x0017945C
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new FlowDocumentPageViewerAutomationPeer(this);
		}

		/// <summary>Handles the <see cref="E:System.Windows.UIElement.KeyDown" />  routed event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Input.KeyEventArgs" /> object that contains the arguments associated with the <see cref="E:System.Windows.UIElement.KeyDown" /> routed event.</param>
		// Token: 0x06005585 RID: 21893 RVA: 0x0017B264 File Offset: 0x00179464
		[SecurityCritical]
		[SecurityTreatAsSafe]
		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (e.Key == Key.Escape && this.FindToolBar != null)
			{
				this.ToggleFindToolBar(false);
				e.Handled = true;
			}
			if (e.Key == Key.F3 && this.CanShowFindToolBar)
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
			if (!e.Handled)
			{
				base.OnKeyDown(e);
			}
		}

		/// <summary>Handles the <see cref="E:System.Windows.UIElement.MouseWheel" /> routed event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Input.MouseWheelEventArgs" /> object containing arguments associated with the <see cref="E:System.Windows.UIElement.MouseWheel" /> routed event.</param>
		// Token: 0x06005586 RID: 21894 RVA: 0x0017B2F4 File Offset: 0x001794F4
		protected override void OnMouseWheel(MouseWheelEventArgs e)
		{
			if (e.Delta != 0)
			{
				if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
				{
					if (e.Delta > 0)
					{
						this.IncreaseZoom();
					}
					else
					{
						this.DecreaseZoom();
					}
				}
				else if (e.Delta > 0)
				{
					base.PreviousPage();
				}
				else
				{
					base.NextPage();
				}
				e.Handled = false;
			}
			if (!e.Handled)
			{
				base.OnMouseWheel(e);
			}
		}

		/// <summary>Called whenever an unhandled <see cref="E:System.Windows.FrameworkElement.ContextMenuOpening" /> routed event reaches this class in its route. Implement this method to add class handling for this event.</summary>
		/// <param name="e">Arguments of the event.</param>
		// Token: 0x06005587 RID: 21895 RVA: 0x0017B359 File Offset: 0x00179559
		protected override void OnContextMenuOpening(ContextMenuEventArgs e)
		{
			base.OnContextMenuOpening(e);
			DocumentViewerHelper.OnContextMenuOpening(base.Document as FlowDocument, this, e);
		}

		/// <summary>Handles the <see cref="E:System.Windows.Controls.Primitives.DocumentViewerBase.PageViewsChanged" />  routed event.</summary>
		// Token: 0x06005588 RID: 21896 RVA: 0x0017B374 File Offset: 0x00179574
		protected override void OnPageViewsChanged()
		{
			this._contentPosition = null;
			this.ApplyZoom();
			base.OnPageViewsChanged();
		}

		/// <summary>Called when the <see cref="P:System.Windows.Controls.Primitives.DocumentViewerBase.Document" /> property is changed.</summary>
		// Token: 0x06005589 RID: 21897 RVA: 0x0017B38C File Offset: 0x0017958C
		protected override void OnDocumentChanged()
		{
			this._contentPosition = null;
			if (this._oldDocument != null)
			{
				DynamicDocumentPaginator dynamicDocumentPaginator = this._oldDocument.DocumentPaginator as DynamicDocumentPaginator;
				if (dynamicDocumentPaginator != null)
				{
					dynamicDocumentPaginator.GetPageNumberCompleted -= this.HandleGetPageNumberCompleted;
				}
				FlowDocumentPaginator flowDocumentPaginator = this._oldDocument.DocumentPaginator as FlowDocumentPaginator;
				if (flowDocumentPaginator != null)
				{
					flowDocumentPaginator.BreakRecordTableInvalidated -= this.HandleAllBreakRecordsInvalidated;
				}
			}
			base.OnDocumentChanged();
			this._oldDocument = base.Document;
			if (base.Document != null && !(base.Document is FlowDocument))
			{
				base.Document = null;
				throw new NotSupportedException(SR.Get("FlowDocumentPageViewerOnlySupportsFlowDocument"));
			}
			if (base.Document != null)
			{
				DynamicDocumentPaginator dynamicDocumentPaginator2 = base.Document.DocumentPaginator as DynamicDocumentPaginator;
				if (dynamicDocumentPaginator2 != null)
				{
					dynamicDocumentPaginator2.GetPageNumberCompleted += this.HandleGetPageNumberCompleted;
				}
				FlowDocumentPaginator flowDocumentPaginator2 = base.Document.DocumentPaginator as FlowDocumentPaginator;
				if (flowDocumentPaginator2 != null)
				{
					flowDocumentPaginator2.BreakRecordTableInvalidated += this.HandleAllBreakRecordsInvalidated;
				}
			}
			if (!this.CanShowFindToolBar && this.FindToolBar != null)
			{
				this.ToggleFindToolBar(false);
			}
			this.OnGoToPageCommand(1);
		}

		/// <summary>Called when a printing job has completed.</summary>
		// Token: 0x0600558A RID: 21898 RVA: 0x0017B4A4 File Offset: 0x001796A4
		protected virtual void OnPrintCompleted()
		{
			this.ClearPrintingState();
		}

		/// <summary>Handles the <see cref="P:System.Windows.Input.NavigationCommands.PreviousPage" /> routed command.</summary>
		// Token: 0x0600558B RID: 21899 RVA: 0x0017B4AC File Offset: 0x001796AC
		protected override void OnPreviousPageCommand()
		{
			if (this.CanGoToPreviousPage)
			{
				this._contentPosition = null;
				base.OnPreviousPageCommand();
			}
		}

		/// <summary>Handles the <see cref="P:System.Windows.Input.NavigationCommands.NextPage" /> routed command.</summary>
		// Token: 0x0600558C RID: 21900 RVA: 0x0017B4C3 File Offset: 0x001796C3
		protected override void OnNextPageCommand()
		{
			if (this.CanGoToNextPage)
			{
				this._contentPosition = null;
				base.OnNextPageCommand();
			}
		}

		/// <summary>Handles the <see cref="P:System.Windows.Input.NavigationCommands.FirstPage" /> routed command.</summary>
		// Token: 0x0600558D RID: 21901 RVA: 0x0017B4DA File Offset: 0x001796DA
		protected override void OnFirstPageCommand()
		{
			if (this.CanGoToPreviousPage)
			{
				this._contentPosition = null;
				base.OnFirstPageCommand();
			}
		}

		/// <summary>Handles the <see cref="P:System.Windows.Input.NavigationCommands.LastPage" /> routed command.</summary>
		// Token: 0x0600558E RID: 21902 RVA: 0x0017B4F1 File Offset: 0x001796F1
		protected override void OnLastPageCommand()
		{
			if (this.CanGoToNextPage)
			{
				this._contentPosition = null;
				base.OnLastPageCommand();
			}
		}

		/// <summary>Handles the <see cref="P:System.Windows.Input.NavigationCommands.GoToPage" /> routed command.</summary>
		/// <param name="pageNumber">The page number to go to.</param>
		// Token: 0x0600558F RID: 21903 RVA: 0x0017B508 File Offset: 0x00179708
		protected override void OnGoToPageCommand(int pageNumber)
		{
			if (this.CanGoToPage(pageNumber) && this.MasterPageNumber != pageNumber)
			{
				this._contentPosition = null;
				base.OnGoToPageCommand(pageNumber);
			}
		}

		/// <summary>Handles the <see cref="P:System.Windows.Input.ApplicationCommands.Find" /> routed command.</summary>
		// Token: 0x06005590 RID: 21904 RVA: 0x0017B52A File Offset: 0x0017972A
		protected virtual void OnFindCommand()
		{
			if (this.CanShowFindToolBar)
			{
				this.ToggleFindToolBar(this.FindToolBar == null);
			}
		}

		/// <summary>Handles the <see cref="P:System.Windows.Input.ApplicationCommands.Print" /> routed command.</summary>
		// Token: 0x06005591 RID: 21905 RVA: 0x0017B544 File Offset: 0x00179744
		protected override void OnPrintCommand()
		{
			PrintDocumentImageableArea printDocumentImageableArea = null;
			FlowDocument flowDocument = base.Document as FlowDocument;
			if (this._printingState != null)
			{
				return;
			}
			if (flowDocument != null)
			{
				XpsDocumentWriter xpsDocumentWriter = PrintQueue.CreateXpsDocumentWriter(ref printDocumentImageableArea);
				if (xpsDocumentWriter != null && printDocumentImageableArea != null)
				{
					FlowDocumentPaginator flowDocumentPaginator = ((IDocumentPaginatorSource)flowDocument).DocumentPaginator as FlowDocumentPaginator;
					this._printingState = new FlowDocumentPrintingState();
					this._printingState.XpsDocumentWriter = xpsDocumentWriter;
					this._printingState.PageSize = flowDocumentPaginator.PageSize;
					this._printingState.PagePadding = flowDocument.PagePadding;
					this._printingState.IsSelectionEnabled = base.IsSelectionEnabled;
					CommandManager.InvalidateRequerySuggested();
					xpsDocumentWriter.WritingCompleted += this.HandlePrintCompleted;
					xpsDocumentWriter.WritingCancelled += this.HandlePrintCancelled;
					CommandManager.AddPreviewCanExecuteHandler(this, new CanExecuteRoutedEventHandler(this.PreviewCanExecuteRoutedEventHandler));
					ReadOnlyCollection<DocumentPageView> pageViews = base.PageViews;
					for (int i = 0; i < pageViews.Count; i++)
					{
						pageViews[i].SuspendLayout();
					}
					if (base.IsSelectionEnabled)
					{
						base.IsSelectionEnabled = false;
					}
					flowDocumentPaginator.PageSize = new Size(printDocumentImageableArea.MediaSizeWidth, printDocumentImageableArea.MediaSizeHeight);
					Thickness thickness = flowDocument.ComputePageMargin();
					flowDocument.PagePadding = new Thickness(Math.Max(printDocumentImageableArea.OriginWidth, thickness.Left), Math.Max(printDocumentImageableArea.OriginHeight, thickness.Top), Math.Max(printDocumentImageableArea.MediaSizeWidth - (printDocumentImageableArea.OriginWidth + printDocumentImageableArea.ExtentWidth), thickness.Right), Math.Max(printDocumentImageableArea.MediaSizeHeight - (printDocumentImageableArea.OriginHeight + printDocumentImageableArea.ExtentHeight), thickness.Bottom));
					xpsDocumentWriter.WriteAsync(flowDocumentPaginator);
					return;
				}
			}
			else
			{
				base.OnPrintCommand();
			}
		}

		/// <summary>Handles the <see cref="P:System.Windows.Input.ApplicationCommands.CancelPrint" /> routed command.</summary>
		// Token: 0x06005592 RID: 21906 RVA: 0x0017B6EA File Offset: 0x001798EA
		protected override void OnCancelPrintCommand()
		{
			if (this._printingState != null)
			{
				this._printingState.XpsDocumentWriter.CancelAsync();
				return;
			}
			base.OnCancelPrintCommand();
		}

		/// <summary>Handles the <see cref="P:System.Windows.Input.NavigationCommands.IncreaseZoom" /> routed command.</summary>
		// Token: 0x06005593 RID: 21907 RVA: 0x0017B70B File Offset: 0x0017990B
		protected virtual void OnIncreaseZoomCommand()
		{
			if (this.CanIncreaseZoom)
			{
				base.SetCurrentValueInternal(FlowDocumentPageViewer.ZoomProperty, Math.Min(this.Zoom + this.ZoomIncrement, this.MaxZoom));
			}
		}

		/// <summary>Handles the <see cref="P:System.Windows.Input.NavigationCommands.DecreaseZoom" /> routed command.</summary>
		// Token: 0x06005594 RID: 21908 RVA: 0x0017B73D File Offset: 0x0017993D
		protected virtual void OnDecreaseZoomCommand()
		{
			if (this.CanDecreaseZoom)
			{
				base.SetCurrentValueInternal(FlowDocumentPageViewer.ZoomProperty, Math.Max(this.Zoom - this.ZoomIncrement, this.MinZoom));
			}
		}

		// Token: 0x06005595 RID: 21909 RVA: 0x0017B770 File Offset: 0x00179970
		internal override bool BuildRouteCore(EventRoute route, RoutedEventArgs args)
		{
			DependencyObject dependencyObject = base.Document as DependencyObject;
			if (dependencyObject != null && LogicalTreeHelper.GetParent(dependencyObject) != this)
			{
				DependencyObject dependencyObject2 = route.PeekBranchNode() as DependencyObject;
				if (dependencyObject2 != null && DocumentViewerHelper.IsLogicalDescendent(dependencyObject2, dependencyObject))
				{
					FrameworkElement.AddIntermediateElementsToRoute(LogicalTreeHelper.GetParent(dependencyObject), route, args, LogicalTreeHelper.GetParent(dependencyObject2));
				}
			}
			return base.BuildRouteCore(route, args);
		}

		// Token: 0x06005596 RID: 21910 RVA: 0x0017B7C8 File Offset: 0x001799C8
		internal override bool InvalidateAutomationAncestorsCore(Stack<DependencyObject> branchNodeStack, out bool continuePastCoreTree)
		{
			bool flag = true;
			DependencyObject dependencyObject = base.Document as DependencyObject;
			if (dependencyObject != null && LogicalTreeHelper.GetParent(dependencyObject) != this)
			{
				DependencyObject dependencyObject2 = (branchNodeStack.Count > 0) ? branchNodeStack.Peek() : null;
				if (dependencyObject2 != null && DocumentViewerHelper.IsLogicalDescendent(dependencyObject2, dependencyObject))
				{
					flag = FrameworkElement.InvalidateAutomationIntermediateElements(LogicalTreeHelper.GetParent(dependencyObject), LogicalTreeHelper.GetParent(dependencyObject2));
				}
			}
			return flag & base.InvalidateAutomationAncestorsCore(branchNodeStack, out continuePastCoreTree);
		}

		// Token: 0x06005597 RID: 21911 RVA: 0x0017B82C File Offset: 0x00179A2C
		internal bool BringPointIntoView(Point point)
		{
			ReadOnlyCollection<DocumentPageView> pageViews = base.PageViews;
			bool result = false;
			if (pageViews.Count > 0)
			{
				Rect[] array = new Rect[pageViews.Count];
				int i;
				for (i = 0; i < pageViews.Count; i++)
				{
					Rect rect = new Rect(pageViews[i].RenderSize);
					rect = pageViews[i].TransformToAncestor(this).TransformBounds(rect);
					array[i] = rect;
				}
				i = 0;
				while (i < array.Length && !array[i].Contains(point))
				{
					i++;
				}
				if (i >= array.Length)
				{
					Rect rect = array[0];
					for (i = 1; i < array.Length; i++)
					{
						rect.Union(array[i]);
					}
					if (DoubleUtil.LessThan(point.X, rect.Left))
					{
						if (this.CanGoToPreviousPage)
						{
							this.OnPreviousPageCommand();
							result = true;
						}
					}
					else if (DoubleUtil.GreaterThan(point.X, rect.Right))
					{
						if (this.CanGoToNextPage)
						{
							this.OnNextPageCommand();
							result = true;
						}
					}
					else if (DoubleUtil.LessThan(point.Y, rect.Top))
					{
						if (this.CanGoToPreviousPage)
						{
							this.OnPreviousPageCommand();
							result = true;
						}
					}
					else if (DoubleUtil.GreaterThan(point.Y, rect.Bottom) && this.CanGoToNextPage)
					{
						this.OnNextPageCommand();
						result = true;
					}
				}
			}
			return result;
		}

		// Token: 0x06005598 RID: 21912 RVA: 0x0017B988 File Offset: 0x00179B88
		internal object BringContentPositionIntoView(object arg)
		{
			this.PrivateBringContentPositionIntoView(arg, false);
			return null;
		}

		// Token: 0x170014D5 RID: 5333
		// (get) Token: 0x06005599 RID: 21913 RVA: 0x0017B993 File Offset: 0x00179B93
		internal ContentPosition ContentPosition
		{
			get
			{
				return this._contentPosition;
			}
		}

		// Token: 0x170014D6 RID: 5334
		// (get) Token: 0x0600559A RID: 21914 RVA: 0x0017B99B File Offset: 0x00179B9B
		internal bool CanShowFindToolBar
		{
			get
			{
				return this._findToolBarHost != null && base.Document != null && base.TextEditor != null;
			}
		}

		// Token: 0x170014D7 RID: 5335
		// (get) Token: 0x0600559B RID: 21915 RVA: 0x0017B9B8 File Offset: 0x00179BB8
		internal bool IsPrinting
		{
			get
			{
				return this._printingState != null;
			}
		}

		// Token: 0x0600559C RID: 21916 RVA: 0x0017B9C4 File Offset: 0x00179BC4
		private void HandleLayoutUpdated(object sender, EventArgs e)
		{
			if (base.Document != null && this._printingState == null)
			{
				DynamicDocumentPaginator dynamicDocumentPaginator = base.Document.DocumentPaginator as DynamicDocumentPaginator;
				if (dynamicDocumentPaginator != null)
				{
					if (this._contentPosition == null)
					{
						DocumentPageView masterPageView = base.GetMasterPageView();
						if (masterPageView != null && masterPageView.DocumentPage != null)
						{
							this._contentPosition = dynamicDocumentPaginator.GetPagePosition(masterPageView.DocumentPage);
						}
						if (this._contentPosition == ContentPosition.Missing)
						{
							this._contentPosition = null;
							return;
						}
					}
					else
					{
						this.PrivateBringContentPositionIntoView(this._contentPosition, true);
					}
				}
			}
		}

		// Token: 0x0600559D RID: 21917 RVA: 0x0017BA44 File Offset: 0x00179C44
		private void HandleGetPageNumberCompleted(object sender, GetPageNumberCompletedEventArgs e)
		{
			if (base.Document != null && sender == base.Document.DocumentPaginator && e != null && !e.Cancelled && e.Error == null && e.UserState == this._bringContentPositionIntoViewToken)
			{
				int pageNumber = e.PageNumber + 1;
				this.OnGoToPageCommand(pageNumber);
			}
		}

		// Token: 0x0600559E RID: 21918 RVA: 0x0017BA98 File Offset: 0x00179C98
		private void HandleAllBreakRecordsInvalidated(object sender, EventArgs e)
		{
			ReadOnlyCollection<DocumentPageView> pageViews = base.PageViews;
			for (int i = 0; i < pageViews.Count; i++)
			{
				pageViews[i].DuplicateVisual();
			}
		}

		// Token: 0x0600559F RID: 21919 RVA: 0x0017BACC File Offset: 0x00179CCC
		private bool IsValidContentPositionForDocument(IDocumentPaginatorSource document, ContentPosition contentPosition)
		{
			FlowDocument flowDocument = document as FlowDocument;
			TextPointer textPointer = contentPosition as TextPointer;
			return flowDocument == null || textPointer == null || flowDocument.ContentStart.TextContainer == textPointer.TextContainer;
		}

		// Token: 0x060055A0 RID: 21920 RVA: 0x0017BB04 File Offset: 0x00179D04
		private void PrivateBringContentPositionIntoView(object arg, bool isAsyncRequest)
		{
			ContentPosition contentPosition = arg as ContentPosition;
			if (contentPosition != null && base.Document != null)
			{
				DynamicDocumentPaginator dynamicDocumentPaginator = base.Document.DocumentPaginator as DynamicDocumentPaginator;
				if (dynamicDocumentPaginator != null && this.IsValidContentPositionForDocument(base.Document, contentPosition))
				{
					dynamicDocumentPaginator.CancelAsync(this._bringContentPositionIntoViewToken);
					if (isAsyncRequest)
					{
						dynamicDocumentPaginator.GetPageNumberAsync(contentPosition, this._bringContentPositionIntoViewToken);
					}
					else
					{
						int pageNumber = dynamicDocumentPaginator.GetPageNumber(contentPosition) + 1;
						this.OnGoToPageCommand(pageNumber);
					}
					this._contentPosition = contentPosition;
				}
			}
		}

		// Token: 0x060055A1 RID: 21921 RVA: 0x0017BB7C File Offset: 0x00179D7C
		private void HandlePrintCompleted(object sender, WritingCompletedEventArgs e)
		{
			this.OnPrintCompleted();
		}

		// Token: 0x060055A2 RID: 21922 RVA: 0x0017B4A4 File Offset: 0x001796A4
		private void HandlePrintCancelled(object sender, WritingCancelledEventArgs e)
		{
			this.ClearPrintingState();
		}

		// Token: 0x060055A3 RID: 21923 RVA: 0x0017BB84 File Offset: 0x00179D84
		private void ClearPrintingState()
		{
			if (this._printingState != null)
			{
				ReadOnlyCollection<DocumentPageView> pageViews = base.PageViews;
				for (int i = 0; i < pageViews.Count; i++)
				{
					pageViews[i].ResumeLayout();
				}
				if (this._printingState.IsSelectionEnabled)
				{
					base.IsSelectionEnabled = true;
				}
				CommandManager.RemovePreviewCanExecuteHandler(this, new CanExecuteRoutedEventHandler(this.PreviewCanExecuteRoutedEventHandler));
				this._printingState.XpsDocumentWriter.WritingCompleted -= this.HandlePrintCompleted;
				this._printingState.XpsDocumentWriter.WritingCancelled -= this.HandlePrintCancelled;
				((FlowDocument)base.Document).PagePadding = this._printingState.PagePadding;
				base.Document.DocumentPaginator.PageSize = this._printingState.PageSize;
				this._printingState = null;
				CommandManager.InvalidateRequerySuggested();
			}
		}

		// Token: 0x060055A4 RID: 21924 RVA: 0x0017BC60 File Offset: 0x00179E60
		private void ApplyZoom()
		{
			ReadOnlyCollection<DocumentPageView> pageViews = base.PageViews;
			for (int i = 0; i < pageViews.Count; i++)
			{
				pageViews[i].SetPageZoom(this.Zoom / 100.0);
			}
		}

		// Token: 0x060055A5 RID: 21925 RVA: 0x0017BCA1 File Offset: 0x00179EA1
		private void ToggleFindToolBar(bool enable)
		{
			Invariant.Assert(enable == (this.FindToolBar == null));
			DocumentViewerHelper.ToggleFindToolBar(this._findToolBarHost, new EventHandler(this.OnFindInvoked), enable);
		}

		// Token: 0x060055A6 RID: 21926 RVA: 0x0017BCCC File Offset: 0x00179ECC
		private void OnFindInvoked(object sender, EventArgs e)
		{
			FindToolBar findToolBar = this.FindToolBar;
			if (findToolBar != null && base.TextEditor != null)
			{
				base.Focus();
				ITextRange textRange = base.Find(findToolBar);
				if (textRange != null && !textRange.IsEmpty)
				{
					if (textRange.Start is ContentPosition)
					{
						this._contentPosition = (ContentPosition)textRange.Start;
						int pageNumber = ((DynamicDocumentPaginator)base.Document.DocumentPaginator).GetPageNumber(this._contentPosition) + 1;
						this.OnBringIntoView(this, Rect.Empty, pageNumber);
						return;
					}
				}
				else
				{
					DocumentViewerHelper.ShowFindUnsuccessfulMessage(findToolBar);
				}
			}
		}

		// Token: 0x060055A7 RID: 21927 RVA: 0x0017BD55 File Offset: 0x00179F55
		private void ZoomChanged(double oldValue, double newValue)
		{
			if (!DoubleUtil.AreClose(oldValue, newValue))
			{
				this.UpdateCanIncreaseZoom();
				this.UpdateCanDecreaseZoom();
				this.ApplyZoom();
			}
		}

		// Token: 0x060055A8 RID: 21928 RVA: 0x0017BD72 File Offset: 0x00179F72
		private void UpdateCanIncreaseZoom()
		{
			base.SetValue(FlowDocumentPageViewer.CanIncreaseZoomPropertyKey, BooleanBoxes.Box(DoubleUtil.GreaterThan(this.MaxZoom, this.Zoom)));
		}

		// Token: 0x060055A9 RID: 21929 RVA: 0x0017BD95 File Offset: 0x00179F95
		private void UpdateCanDecreaseZoom()
		{
			base.SetValue(FlowDocumentPageViewer.CanDecreaseZoomPropertyKey, BooleanBoxes.Box(DoubleUtil.LessThan(this.MinZoom, this.Zoom)));
		}

		// Token: 0x060055AA RID: 21930 RVA: 0x0017BDB8 File Offset: 0x00179FB8
		private void MaxZoomChanged(double oldValue, double newValue)
		{
			base.CoerceValue(FlowDocumentPageViewer.ZoomProperty);
			this.UpdateCanIncreaseZoom();
		}

		// Token: 0x060055AB RID: 21931 RVA: 0x0017BDCB File Offset: 0x00179FCB
		private void MinZoomChanged(double oldValue, double newValue)
		{
			base.CoerceValue(FlowDocumentPageViewer.MaxZoomProperty);
			base.CoerceValue(FlowDocumentPageViewer.ZoomProperty);
			this.UpdateCanIncreaseZoom();
			this.UpdateCanDecreaseZoom();
		}

		// Token: 0x060055AC RID: 21932 RVA: 0x0017BDF0 File Offset: 0x00179FF0
		private static void CreateCommandBindings()
		{
			ExecutedRoutedEventHandler executedRoutedEventHandler = new ExecutedRoutedEventHandler(FlowDocumentPageViewer.ExecutedRoutedEventHandler);
			CanExecuteRoutedEventHandler canExecuteRoutedEventHandler = new CanExecuteRoutedEventHandler(FlowDocumentPageViewer.CanExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(FlowDocumentPageViewer), ApplicationCommands.Find, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(FlowDocumentPageViewer), NavigationCommands.IncreaseZoom, executedRoutedEventHandler, canExecuteRoutedEventHandler, new KeyGesture(Key.OemPlus, ModifierKeys.Control));
			CommandHelpers.RegisterCommandHandler(typeof(FlowDocumentPageViewer), NavigationCommands.DecreaseZoom, executedRoutedEventHandler, canExecuteRoutedEventHandler, new KeyGesture(Key.OemMinus, ModifierKeys.Control));
			CommandManager.RegisterClassInputBinding(typeof(FlowDocumentPageViewer), new InputBinding(NavigationCommands.PreviousPage, new KeyGesture(Key.Left)));
			CommandManager.RegisterClassInputBinding(typeof(FlowDocumentPageViewer), new InputBinding(NavigationCommands.PreviousPage, new KeyGesture(Key.Up)));
			CommandManager.RegisterClassInputBinding(typeof(FlowDocumentPageViewer), new InputBinding(NavigationCommands.PreviousPage, new KeyGesture(Key.Prior)));
			CommandManager.RegisterClassInputBinding(typeof(FlowDocumentPageViewer), new InputBinding(NavigationCommands.NextPage, new KeyGesture(Key.Right)));
			CommandManager.RegisterClassInputBinding(typeof(FlowDocumentPageViewer), new InputBinding(NavigationCommands.NextPage, new KeyGesture(Key.Down)));
			CommandManager.RegisterClassInputBinding(typeof(FlowDocumentPageViewer), new InputBinding(NavigationCommands.NextPage, new KeyGesture(Key.Next)));
			CommandManager.RegisterClassInputBinding(typeof(FlowDocumentPageViewer), new InputBinding(NavigationCommands.FirstPage, new KeyGesture(Key.Home)));
			CommandManager.RegisterClassInputBinding(typeof(FlowDocumentPageViewer), new InputBinding(NavigationCommands.FirstPage, new KeyGesture(Key.Home, ModifierKeys.Control)));
			CommandManager.RegisterClassInputBinding(typeof(FlowDocumentPageViewer), new InputBinding(NavigationCommands.LastPage, new KeyGesture(Key.End)));
			CommandManager.RegisterClassInputBinding(typeof(FlowDocumentPageViewer), new InputBinding(NavigationCommands.LastPage, new KeyGesture(Key.End, ModifierKeys.Control)));
		}

		// Token: 0x060055AD RID: 21933 RVA: 0x0017BFB4 File Offset: 0x0017A1B4
		private static void CanExecuteRoutedEventHandler(object target, CanExecuteRoutedEventArgs args)
		{
			FlowDocumentPageViewer flowDocumentPageViewer = target as FlowDocumentPageViewer;
			Invariant.Assert(flowDocumentPageViewer != null, "Target of QueryEnabledEvent must be FlowDocumentPageViewer.");
			Invariant.Assert(args != null, "args cannot be null.");
			if (args.Command == ApplicationCommands.Find)
			{
				args.CanExecute = flowDocumentPageViewer.CanShowFindToolBar;
				return;
			}
			args.CanExecute = true;
		}

		// Token: 0x060055AE RID: 21934 RVA: 0x0017C008 File Offset: 0x0017A208
		private static void ExecutedRoutedEventHandler(object target, ExecutedRoutedEventArgs args)
		{
			FlowDocumentPageViewer flowDocumentPageViewer = target as FlowDocumentPageViewer;
			Invariant.Assert(flowDocumentPageViewer != null, "Target of ExecuteEvent must be FlowDocumentPageViewer.");
			Invariant.Assert(args != null, "args cannot be null.");
			if (args.Command == NavigationCommands.IncreaseZoom)
			{
				flowDocumentPageViewer.OnIncreaseZoomCommand();
				return;
			}
			if (args.Command == NavigationCommands.DecreaseZoom)
			{
				flowDocumentPageViewer.OnDecreaseZoomCommand();
				return;
			}
			if (args.Command == ApplicationCommands.Find)
			{
				flowDocumentPageViewer.OnFindCommand();
				return;
			}
			Invariant.Assert(false, "Command not handled in ExecutedRoutedEventHandler.");
		}

		// Token: 0x060055AF RID: 21935 RVA: 0x0017C080 File Offset: 0x0017A280
		private void PreviewCanExecuteRoutedEventHandler(object target, CanExecuteRoutedEventArgs args)
		{
			FlowDocumentPageViewer flowDocumentPageViewer = target as FlowDocumentPageViewer;
			Invariant.Assert(flowDocumentPageViewer != null, "Target of PreviewCanExecuteRoutedEventHandler must be FlowDocumentPageViewer.");
			Invariant.Assert(args != null, "args cannot be null.");
			if (flowDocumentPageViewer._printingState != null && args.Command != ApplicationCommands.CancelPrint)
			{
				args.CanExecute = false;
				args.Handled = true;
			}
		}

		// Token: 0x060055B0 RID: 21936 RVA: 0x0017C0D3 File Offset: 0x0017A2D3
		private static void KeyDownHandler(object sender, KeyEventArgs e)
		{
			DocumentViewerHelper.KeyDownHelper(e, ((FlowDocumentPageViewer)sender)._findToolBarHost);
		}

		// Token: 0x060055B1 RID: 21937 RVA: 0x0017C0E8 File Offset: 0x0017A2E8
		private static object CoerceZoom(DependencyObject d, object value)
		{
			Invariant.Assert(d != null && d is FlowDocumentPageViewer);
			FlowDocumentPageViewer flowDocumentPageViewer = (FlowDocumentPageViewer)d;
			double value2 = (double)value;
			double maxZoom = flowDocumentPageViewer.MaxZoom;
			if (DoubleUtil.LessThan(maxZoom, value2))
			{
				return maxZoom;
			}
			double minZoom = flowDocumentPageViewer.MinZoom;
			if (DoubleUtil.GreaterThan(minZoom, value2))
			{
				return minZoom;
			}
			return value;
		}

		// Token: 0x060055B2 RID: 21938 RVA: 0x0017C148 File Offset: 0x0017A348
		private static object CoerceMaxZoom(DependencyObject d, object value)
		{
			Invariant.Assert(d != null && d is FlowDocumentPageViewer);
			FlowDocumentPageViewer flowDocumentPageViewer = (FlowDocumentPageViewer)d;
			double minZoom = flowDocumentPageViewer.MinZoom;
			if (DoubleUtil.LessThan((double)value, minZoom))
			{
				return minZoom;
			}
			return value;
		}

		// Token: 0x060055B3 RID: 21939 RVA: 0x0017C18D File Offset: 0x0017A38D
		private static void ZoomChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Invariant.Assert(d != null && d is FlowDocumentPageViewer);
			((FlowDocumentPageViewer)d).ZoomChanged((double)e.OldValue, (double)e.NewValue);
		}

		// Token: 0x060055B4 RID: 21940 RVA: 0x0017C1C6 File Offset: 0x0017A3C6
		private static void MaxZoomChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Invariant.Assert(d != null && d is FlowDocumentPageViewer);
			((FlowDocumentPageViewer)d).MaxZoomChanged((double)e.OldValue, (double)e.NewValue);
		}

		// Token: 0x060055B5 RID: 21941 RVA: 0x0017C1FF File Offset: 0x0017A3FF
		private static void MinZoomChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Invariant.Assert(d != null && d is FlowDocumentPageViewer);
			((FlowDocumentPageViewer)d).MinZoomChanged((double)e.OldValue, (double)e.NewValue);
		}

		// Token: 0x060055B6 RID: 21942 RVA: 0x0017C238 File Offset: 0x0017A438
		private static bool ZoomValidateValue(object o)
		{
			double num = (double)o;
			return !double.IsNaN(num) && !double.IsInfinity(num) && DoubleUtil.GreaterThan(num, 0.0);
		}

		// Token: 0x060055B7 RID: 21943 RVA: 0x0017C270 File Offset: 0x0017A470
		private static void UpdateCaretElement(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			FlowDocumentPageViewer flowDocumentPageViewer = (FlowDocumentPageViewer)d;
			if (flowDocumentPageViewer.Selection != null)
			{
				CaretElement caretElement = flowDocumentPageViewer.Selection.CaretElement;
				if (caretElement != null)
				{
					caretElement.InvalidateVisual();
				}
			}
		}

		// Token: 0x170014D8 RID: 5336
		// (get) Token: 0x060055B8 RID: 21944 RVA: 0x0017C2A1 File Offset: 0x0017A4A1
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

		// Token: 0x060055B9 RID: 21945 RVA: 0x0017C2C0 File Offset: 0x0017A4C0
		CustomJournalStateInternal IJournalState.GetJournalState(JournalReason journalReason)
		{
			int contentPosition = -1;
			LogicalDirection contentPositionDirection = LogicalDirection.Forward;
			TextPointer textPointer = this.ContentPosition as TextPointer;
			if (textPointer != null)
			{
				contentPosition = textPointer.Offset;
				contentPositionDirection = textPointer.LogicalDirection;
			}
			return new FlowDocumentPageViewer.JournalState(contentPosition, contentPositionDirection, this.Zoom);
		}

		// Token: 0x060055BA RID: 21946 RVA: 0x0017C2FC File Offset: 0x0017A4FC
		void IJournalState.RestoreJournalState(CustomJournalStateInternal state)
		{
			FlowDocumentPageViewer.JournalState journalState = state as FlowDocumentPageViewer.JournalState;
			if (state != null)
			{
				base.SetCurrentValueInternal(FlowDocumentPageViewer.ZoomProperty, journalState.Zoom);
				if (journalState.ContentPosition != -1)
				{
					FlowDocument flowDocument = base.Document as FlowDocument;
					if (flowDocument != null)
					{
						TextContainer textContainer = flowDocument.StructuralCache.TextContainer;
						if (journalState.ContentPosition <= textContainer.SymbolCount)
						{
							TextPointer arg = textContainer.CreatePointerAtOffset(journalState.ContentPosition, journalState.ContentPositionDirection);
							base.Dispatcher.BeginInvoke(DispatcherPriority.Input, new DispatcherOperationCallback(this.BringContentPositionIntoView), arg);
						}
					}
				}
			}
		}

		// Token: 0x170014D9 RID: 5337
		// (get) Token: 0x060055BB RID: 21947 RVA: 0x0017C388 File Offset: 0x0017A588
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return FlowDocumentPageViewer._dType;
			}
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.FlowDocumentPageViewer.Zoom" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.FlowDocumentPageViewer.Zoom" /> dependency property.</returns>
		// Token: 0x04002DF6 RID: 11766
		public static readonly DependencyProperty ZoomProperty = DependencyProperty.Register("Zoom", typeof(double), typeof(FlowDocumentPageViewer), new FrameworkPropertyMetadata(100.0, new PropertyChangedCallback(FlowDocumentPageViewer.ZoomChanged), new CoerceValueCallback(FlowDocumentPageViewer.CoerceZoom)), new ValidateValueCallback(FlowDocumentPageViewer.ZoomValidateValue));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.FlowDocumentPageViewer.MaxZoom" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.FlowDocumentPageViewer.MaxZoom" /> dependency property.</returns>
		// Token: 0x04002DF7 RID: 11767
		public static readonly DependencyProperty MaxZoomProperty = DependencyProperty.Register("MaxZoom", typeof(double), typeof(FlowDocumentPageViewer), new FrameworkPropertyMetadata(200.0, new PropertyChangedCallback(FlowDocumentPageViewer.MaxZoomChanged), new CoerceValueCallback(FlowDocumentPageViewer.CoerceMaxZoom)), new ValidateValueCallback(FlowDocumentPageViewer.ZoomValidateValue));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.FlowDocumentPageViewer.MinZoom" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.FlowDocumentPageViewer.MinZoom" /> dependency property.</returns>
		// Token: 0x04002DF8 RID: 11768
		public static readonly DependencyProperty MinZoomProperty = DependencyProperty.Register("MinZoom", typeof(double), typeof(FlowDocumentPageViewer), new FrameworkPropertyMetadata(80.0, new PropertyChangedCallback(FlowDocumentPageViewer.MinZoomChanged)), new ValidateValueCallback(FlowDocumentPageViewer.ZoomValidateValue));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.FlowDocumentPageViewer.ZoomIncrement" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.FlowDocumentPageViewer.ZoomIncrement" /> dependency property.</returns>
		// Token: 0x04002DF9 RID: 11769
		public static readonly DependencyProperty ZoomIncrementProperty = DependencyProperty.Register("ZoomIncrement", typeof(double), typeof(FlowDocumentPageViewer), new FrameworkPropertyMetadata(10.0), new ValidateValueCallback(FlowDocumentPageViewer.ZoomValidateValue));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.FlowDocumentPageViewer.CanIncreaseZoom" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.FlowDocumentPageViewer.CanIncreaseZoom" /> dependency property.</returns>
		// Token: 0x04002DFA RID: 11770
		protected static readonly DependencyPropertyKey CanIncreaseZoomPropertyKey = DependencyProperty.RegisterReadOnly("CanIncreaseZoom", typeof(bool), typeof(FlowDocumentPageViewer), new FrameworkPropertyMetadata(true));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.FlowDocumentPageViewer.CanIncreaseZoom" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.FlowDocumentPageViewer.CanIncreaseZoom" /> dependency property.</returns>
		// Token: 0x04002DFB RID: 11771
		public static readonly DependencyProperty CanIncreaseZoomProperty = FlowDocumentPageViewer.CanIncreaseZoomPropertyKey.DependencyProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.FlowDocumentPageViewer.CanDecreaseZoom" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.FlowDocumentPageViewer.CanDecreaseZoom" /> dependency property.</returns>
		// Token: 0x04002DFC RID: 11772
		protected static readonly DependencyPropertyKey CanDecreaseZoomPropertyKey = DependencyProperty.RegisterReadOnly("CanDecreaseZoom", typeof(bool), typeof(FlowDocumentPageViewer), new FrameworkPropertyMetadata(true));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.FlowDocumentPageViewer.CanDecreaseZoom" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.FlowDocumentPageViewer.CanDecreaseZoom" /> dependency property.</returns>
		// Token: 0x04002DFD RID: 11773
		public static readonly DependencyProperty CanDecreaseZoomProperty = FlowDocumentPageViewer.CanDecreaseZoomPropertyKey.DependencyProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.FlowDocumentPageViewer.SelectionBrush" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.FlowDocumentPageViewer.SelectionBrush" /> dependency property.</returns>
		// Token: 0x04002DFE RID: 11774
		public static readonly DependencyProperty SelectionBrushProperty = TextBoxBase.SelectionBrushProperty.AddOwner(typeof(FlowDocumentPageViewer));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.FlowDocumentPageViewer.SelectionOpacity" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.FlowDocumentPageViewer.SelectionOpacity" /> dependency property.</returns>
		// Token: 0x04002DFF RID: 11775
		public static readonly DependencyProperty SelectionOpacityProperty = TextBoxBase.SelectionOpacityProperty.AddOwner(typeof(FlowDocumentPageViewer));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.FlowDocumentPageViewer.IsSelectionActive" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.FlowDocumentPageViewer.IsSelectionActive" /> dependency property.</returns>
		// Token: 0x04002E00 RID: 11776
		public static readonly DependencyProperty IsSelectionActiveProperty = TextBoxBase.IsSelectionActiveProperty.AddOwner(typeof(FlowDocumentPageViewer));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.FlowDocumentPageViewer.IsInactiveSelectionHighlightEnabled" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.FlowDocumentPageViewer.IsInactiveSelectionHighlightEnabled" /> dependency property.</returns>
		// Token: 0x04002E01 RID: 11777
		public static readonly DependencyProperty IsInactiveSelectionHighlightEnabledProperty = TextBoxBase.IsInactiveSelectionHighlightEnabledProperty.AddOwner(typeof(FlowDocumentPageViewer));

		// Token: 0x04002E02 RID: 11778
		private Decorator _findToolBarHost;

		// Token: 0x04002E03 RID: 11779
		private ContentPosition _contentPosition;

		// Token: 0x04002E04 RID: 11780
		private FlowDocumentPrintingState _printingState;

		// Token: 0x04002E05 RID: 11781
		private IDocumentPaginatorSource _oldDocument;

		// Token: 0x04002E06 RID: 11782
		private object _bringContentPositionIntoViewToken = new object();

		// Token: 0x04002E07 RID: 11783
		private const string _findToolBarHostTemplateName = "PART_FindToolBarHost";

		// Token: 0x04002E08 RID: 11784
		private static DependencyObjectType _dType;

		// Token: 0x020009B8 RID: 2488
		[Serializable]
		private class JournalState : CustomJournalStateInternal
		{
			// Token: 0x0600886B RID: 34923 RVA: 0x002522C0 File Offset: 0x002504C0
			public JournalState(int contentPosition, LogicalDirection contentPositionDirection, double zoom)
			{
				this.ContentPosition = contentPosition;
				this.ContentPositionDirection = contentPositionDirection;
				this.Zoom = zoom;
			}

			// Token: 0x04004554 RID: 17748
			public int ContentPosition;

			// Token: 0x04004555 RID: 17749
			public LogicalDirection ContentPositionDirection;

			// Token: 0x04004556 RID: 17750
			public double Zoom;
		}
	}
}
