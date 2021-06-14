using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Annotations;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml;
using MS.Internal.Annotations.Anchoring;
using MS.Utility;

namespace MS.Internal.Annotations.Component
{
	// Token: 0x020007E3 RID: 2019
	internal sealed class MarkedHighlightComponent : Canvas, IAnnotationComponent
	{
		// Token: 0x06007CD1 RID: 31953 RVA: 0x00231C48 File Offset: 0x0022FE48
		public MarkedHighlightComponent(XmlQualifiedName type, DependencyObject host)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			this._DPHost = ((host == null) ? this : host);
			base.ClipToBounds = false;
			this.HighlightAnchor = new HighlightComponent(1, true, type);
			base.Children.Add(this.HighlightAnchor);
			this._leftMarker = null;
			this._rightMarker = null;
			this._state = 0;
			this.SetState();
		}

		// Token: 0x17001D05 RID: 7429
		// (get) Token: 0x06007CD2 RID: 31954 RVA: 0x00231CC8 File Offset: 0x0022FEC8
		public IList AttachedAnnotations
		{
			get
			{
				ArrayList arrayList = new ArrayList();
				if (this._attachedAnnotation != null)
				{
					arrayList.Add(this._attachedAnnotation);
				}
				return arrayList;
			}
		}

		// Token: 0x17001D06 RID: 7430
		// (get) Token: 0x06007CD3 RID: 31955 RVA: 0x00231CF1 File Offset: 0x0022FEF1
		// (set) Token: 0x06007CD4 RID: 31956 RVA: 0x00231CF9 File Offset: 0x0022FEF9
		public PresentationContext PresentationContext
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

		// Token: 0x17001D07 RID: 7431
		// (get) Token: 0x06007CD5 RID: 31957 RVA: 0x0011BEC8 File Offset: 0x0011A0C8
		// (set) Token: 0x06007CD6 RID: 31958 RVA: 0x00002137 File Offset: 0x00000337
		public int ZOrder
		{
			get
			{
				return -1;
			}
			set
			{
			}
		}

		// Token: 0x17001D08 RID: 7432
		// (get) Token: 0x06007CD7 RID: 31959 RVA: 0x00231D02 File Offset: 0x0022FF02
		public UIElement AnnotatedElement
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

		// Token: 0x17001D09 RID: 7433
		// (get) Token: 0x06007CD8 RID: 31960 RVA: 0x00231D1E File Offset: 0x0022FF1E
		// (set) Token: 0x06007CD9 RID: 31961 RVA: 0x00231D26 File Offset: 0x0022FF26
		public bool IsDirty
		{
			get
			{
				return this._isDirty;
			}
			set
			{
				this._isDirty = value;
				if (value)
				{
					this.UpdateGeometry();
				}
			}
		}

		// Token: 0x06007CDA RID: 31962 RVA: 0x00231D38 File Offset: 0x0022FF38
		public GeneralTransform GetDesiredTransform(GeneralTransform transform)
		{
			if (this._attachedAnnotation == null)
			{
				throw new InvalidOperationException(SR.Get("InvalidAttachedAnnotation"));
			}
			this.HighlightAnchor.GetDesiredTransform(transform);
			return transform;
		}

		// Token: 0x06007CDB RID: 31963 RVA: 0x00231D60 File Offset: 0x0022FF60
		public void AddAttachedAnnotation(IAttachedAnnotation attachedAnnotation)
		{
			if (this._attachedAnnotation != null)
			{
				throw new ArgumentException(SR.Get("MoreThanOneAttachedAnnotation"));
			}
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.AddAttachedMHBegin);
			this._attachedAnnotation = attachedAnnotation;
			if ((attachedAnnotation.AttachmentLevel & AttachmentLevel.StartPortion) != AttachmentLevel.Unresolved)
			{
				this._leftMarker = this.CreateMarker(this.GetMarkerGeometry());
			}
			if ((attachedAnnotation.AttachmentLevel & AttachmentLevel.EndPortion) != AttachmentLevel.Unresolved)
			{
				this._rightMarker = this.CreateMarker(this.GetMarkerGeometry());
			}
			this.RegisterAnchor();
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.AddAttachedMHEnd);
		}

		// Token: 0x06007CDC RID: 31964 RVA: 0x00231DE4 File Offset: 0x0022FFE4
		public void RemoveAttachedAnnotation(IAttachedAnnotation attachedAnnotation)
		{
			if (attachedAnnotation == null)
			{
				throw new ArgumentNullException("attachedAnnotation");
			}
			if (attachedAnnotation != this._attachedAnnotation)
			{
				throw new ArgumentException(SR.Get("InvalidAttachedAnnotation"), "attachedAnnotation");
			}
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.RemoveAttachedMHBegin);
			this.CleanUpAnchor();
			this._attachedAnnotation = null;
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.RemoveAttachedMHEnd);
		}

		// Token: 0x06007CDD RID: 31965 RVA: 0x00127C49 File Offset: 0x00125E49
		public void ModifyAttachedAnnotation(IAttachedAnnotation attachedAnnotation, object previousAttachedAnchor, AttachmentLevel previousAttachmentLevel)
		{
			throw new NotSupportedException(SR.Get("NotSupported"));
		}

		// Token: 0x17001D0A RID: 7434
		// (set) Token: 0x06007CDE RID: 31966 RVA: 0x00231E44 File Offset: 0x00230044
		public bool Focused
		{
			set
			{
				byte state = this._state;
				if (value)
				{
					this._state |= 1;
				}
				else
				{
					this._state &= 126;
				}
				if (this._state == 0 != (state == 0))
				{
					this.SetState();
				}
			}
		}

		// Token: 0x17001D0B RID: 7435
		// (set) Token: 0x06007CDF RID: 31967 RVA: 0x00231E91 File Offset: 0x00230091
		public Brush MarkerBrush
		{
			set
			{
				base.SetValue(MarkedHighlightComponent.MarkerBrushProperty, value);
			}
		}

		// Token: 0x17001D0C RID: 7436
		// (set) Token: 0x06007CE0 RID: 31968 RVA: 0x00231E9F File Offset: 0x0023009F
		public double StrokeThickness
		{
			set
			{
				base.SetValue(MarkedHighlightComponent.StrokeThicknessProperty, value);
			}
		}

		// Token: 0x06007CE1 RID: 31969 RVA: 0x00231EB2 File Offset: 0x002300B2
		internal void SetTabIndex(int index)
		{
			if (this._DPHost != null)
			{
				KeyboardNavigation.SetTabIndex(this._DPHost, index);
			}
		}

		// Token: 0x06007CE2 RID: 31970 RVA: 0x00231EC8 File Offset: 0x002300C8
		private void SetMarkerTransform(Path marker, ITextPointer anchor, ITextPointer baseAnchor, int xScaleFactor)
		{
			if (marker == null)
			{
				return;
			}
			GeometryGroup geometryGroup = marker.Data as GeometryGroup;
			Rect anchorRectangle = TextSelectionHelper.GetAnchorRectangle(anchor);
			if (anchorRectangle == Rect.Empty)
			{
				return;
			}
			double num = anchorRectangle.Height - MarkedHighlightComponent.MarkerVerticalSpace - this._bottomTailHeight - this._topTailHeight;
			double scaleY = 0.0;
			double scaleY2 = 0.0;
			if (num > 0.0)
			{
				scaleY = num / this._bodyHeight;
				scaleY2 = 1.0;
			}
			ScaleTransform value = new ScaleTransform(1.0, scaleY);
			TranslateTransform value2 = new TranslateTransform(anchorRectangle.X, anchorRectangle.Y + this._topTailHeight + MarkedHighlightComponent.MarkerVerticalSpace);
			TransformGroup transformGroup = new TransformGroup();
			transformGroup.Children.Add(value);
			transformGroup.Children.Add(value2);
			ScaleTransform value3 = new ScaleTransform((double)xScaleFactor, scaleY2);
			TranslateTransform value4 = new TranslateTransform(anchorRectangle.X, anchorRectangle.Bottom - this._bottomTailHeight);
			TranslateTransform value5 = new TranslateTransform(anchorRectangle.X, anchorRectangle.Top + MarkedHighlightComponent.MarkerVerticalSpace);
			TransformGroup transformGroup2 = new TransformGroup();
			transformGroup2.Children.Add(value3);
			transformGroup2.Children.Add(value4);
			TransformGroup transformGroup3 = new TransformGroup();
			transformGroup3.Children.Add(value3);
			transformGroup3.Children.Add(value5);
			if (geometryGroup.Children[0] != null)
			{
				geometryGroup.Children[0].Transform = transformGroup3;
			}
			if (geometryGroup.Children[1] != null)
			{
				geometryGroup.Children[1].Transform = transformGroup;
			}
			if (geometryGroup.Children[2] != null)
			{
				geometryGroup.Children[2].Transform = transformGroup2;
			}
			if (baseAnchor != null)
			{
				ITextView documentPageTextView = TextSelectionHelper.GetDocumentPageTextView(baseAnchor);
				ITextView documentPageTextView2 = TextSelectionHelper.GetDocumentPageTextView(anchor);
				if (documentPageTextView != documentPageTextView2 && documentPageTextView.RenderScope != null && documentPageTextView2.RenderScope != null)
				{
					geometryGroup.Transform = (Transform)documentPageTextView2.RenderScope.TransformToVisual(documentPageTextView.RenderScope);
				}
			}
		}

		// Token: 0x06007CE3 RID: 31971 RVA: 0x002320D8 File Offset: 0x002302D8
		private void SetSelected(bool selected)
		{
			byte state = this._state;
			if (selected && this._uiParent.IsFocused)
			{
				this._state |= 2;
			}
			else
			{
				this._state &= 125;
			}
			if (this._state == 0 != (state == 0))
			{
				this.SetState();
			}
		}

		// Token: 0x06007CE4 RID: 31972 RVA: 0x00232134 File Offset: 0x00230334
		private void RemoveHighlightMarkers()
		{
			if (this._leftMarker != null)
			{
				base.Children.Remove(this._leftMarker);
			}
			if (this._rightMarker != null)
			{
				base.Children.Remove(this._rightMarker);
			}
			this._leftMarker = null;
			this._rightMarker = null;
		}

		// Token: 0x06007CE5 RID: 31973 RVA: 0x00232184 File Offset: 0x00230384
		private void RegisterAnchor()
		{
			TextAnchor textAnchor = this._attachedAnnotation.AttachedAnchor as TextAnchor;
			if (textAnchor == null)
			{
				throw new ArgumentException(SR.Get("InvalidAttachedAnchor"));
			}
			ITextContainer textContainer = textAnchor.Start.TextContainer;
			this.HighlightAnchor.AddAttachedAnnotation(this._attachedAnnotation);
			this.UpdateGeometry();
			AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this.AnnotatedElement);
			if (adornerLayer == null)
			{
				throw new InvalidOperationException(SR.Get("NoPresentationContextForGivenElement", new object[]
				{
					this.AnnotatedElement
				}));
			}
			AdornerPresentationContext.HostComponent(adornerLayer, this, this.AnnotatedElement, false);
			this._selection = textContainer.TextSelection;
			if (this._selection != null)
			{
				this._uiParent = (PathNode.GetParent(textContainer.Parent) as UIElement);
				this.RegisterComponent();
				if (this._uiParent != null)
				{
					this._uiParent.GotKeyboardFocus += this.OnContainerGotFocus;
					this._uiParent.LostKeyboardFocus += this.OnContainerLostFocus;
					if (this.HighlightAnchor.IsSelected(this._selection))
					{
						this.SetSelected(true);
					}
				}
			}
		}

		// Token: 0x06007CE6 RID: 31974 RVA: 0x00232294 File Offset: 0x00230494
		private void CleanUpAnchor()
		{
			if (this._selection != null)
			{
				this.UnregisterComponent();
				if (this._uiParent != null)
				{
					this._uiParent.GotKeyboardFocus -= this.OnContainerGotFocus;
					this._uiParent.LostKeyboardFocus -= this.OnContainerLostFocus;
				}
			}
			this._presentationContext.RemoveFromHost(this, false);
			if (this.HighlightAnchor != null)
			{
				this.HighlightAnchor.RemoveAttachedAnnotation(this._attachedAnnotation);
				base.Children.Remove(this.HighlightAnchor);
				this.HighlightAnchor = null;
				this.RemoveHighlightMarkers();
			}
			this._attachedAnnotation = null;
		}

		// Token: 0x06007CE7 RID: 31975 RVA: 0x00232330 File Offset: 0x00230530
		private void SetState()
		{
			if (this._state == 0)
			{
				if (this._highlightAnchor != null)
				{
					this._highlightAnchor.Activate(false);
				}
				this.MarkerBrush = new SolidColorBrush(MarkedHighlightComponent.DefaultMarkerColor);
				this.StrokeThickness = MarkedHighlightComponent.MarkerStrokeThickness;
				this._DPHost.SetValue(StickyNoteControl.IsActiveProperty, false);
				return;
			}
			if (this._highlightAnchor != null)
			{
				this._highlightAnchor.Activate(true);
			}
			this.MarkerBrush = new SolidColorBrush(MarkedHighlightComponent.DefaultActiveMarkerColor);
			this.StrokeThickness = MarkedHighlightComponent.ActiveMarkerStrokeThickness;
			this._DPHost.SetValue(StickyNoteControl.IsActiveProperty, true);
		}

		// Token: 0x06007CE8 RID: 31976 RVA: 0x002323C8 File Offset: 0x002305C8
		private Path CreateMarker(Geometry geometry)
		{
			Path path = new Path();
			path.Data = geometry;
			Binding binding = new Binding("MarkerBrushProperty");
			binding.Source = this;
			path.SetBinding(Shape.StrokeProperty, binding);
			Binding binding2 = new Binding("StrokeThicknessProperty");
			binding2.Source = this;
			path.SetBinding(Shape.StrokeThicknessProperty, binding2);
			path.StrokeEndLineCap = PenLineCap.Round;
			path.StrokeStartLineCap = PenLineCap.Round;
			base.Children.Add(path);
			return path;
		}

		// Token: 0x06007CE9 RID: 31977 RVA: 0x0023243C File Offset: 0x0023063C
		private void RegisterComponent()
		{
			MarkedHighlightComponent.ComponentsRegister componentsRegister = (MarkedHighlightComponent.ComponentsRegister)MarkedHighlightComponent._documentHandlers[this._selection];
			if (componentsRegister == null)
			{
				componentsRegister = new MarkedHighlightComponent.ComponentsRegister(new EventHandler(MarkedHighlightComponent.OnSelectionChanged), new MouseEventHandler(MarkedHighlightComponent.OnMouseMove));
				MarkedHighlightComponent._documentHandlers.Add(this._selection, componentsRegister);
				this._selection.Changed += componentsRegister.SelectionHandler;
				if (this._uiParent != null)
				{
					this._uiParent.MouseMove += componentsRegister.MouseMoveHandler;
				}
			}
			componentsRegister.Add(this);
		}

		// Token: 0x06007CEA RID: 31978 RVA: 0x002324C4 File Offset: 0x002306C4
		private void UnregisterComponent()
		{
			MarkedHighlightComponent.ComponentsRegister componentsRegister = (MarkedHighlightComponent.ComponentsRegister)MarkedHighlightComponent._documentHandlers[this._selection];
			componentsRegister.Remove(this);
			if (componentsRegister.Components.Count == 0)
			{
				MarkedHighlightComponent._documentHandlers.Remove(this._selection);
				this._selection.Changed -= componentsRegister.SelectionHandler;
				if (this._uiParent != null)
				{
					this._uiParent.MouseMove -= componentsRegister.MouseMoveHandler;
				}
			}
		}

		// Token: 0x06007CEB RID: 31979 RVA: 0x00232538 File Offset: 0x00230738
		private void UpdateGeometry()
		{
			if (this.HighlightAnchor == null || this.HighlightAnchor == null)
			{
				throw new Exception(SR.Get("UndefinedHighlightAnchor"));
			}
			TextAnchor range = ((IHighlightRange)this.HighlightAnchor).Range;
			ITextPointer textPointer = range.Start.CreatePointer(LogicalDirection.Forward);
			ITextPointer textPointer2 = range.End.CreatePointer(LogicalDirection.Backward);
			FlowDirection textFlowDirection = MarkedHighlightComponent.GetTextFlowDirection(textPointer);
			FlowDirection textFlowDirection2 = MarkedHighlightComponent.GetTextFlowDirection(textPointer2);
			this.SetMarkerTransform(this._leftMarker, textPointer, null, (textFlowDirection == FlowDirection.LeftToRight) ? 1 : -1);
			this.SetMarkerTransform(this._rightMarker, textPointer2, textPointer, (textFlowDirection2 == FlowDirection.LeftToRight) ? -1 : 1);
			this.HighlightAnchor.IsDirty = true;
			this.IsDirty = false;
		}

		// Token: 0x06007CEC RID: 31980 RVA: 0x002325D8 File Offset: 0x002307D8
		private Geometry GetMarkerGeometry()
		{
			GeometryGroup geometryGroup = new GeometryGroup();
			geometryGroup.Children.Add(new LineGeometry(new Point(0.0, 1.0), new Point(1.0, 0.0)));
			geometryGroup.Children.Add(new LineGeometry(new Point(0.0, 0.0), new Point(0.0, 50.0)));
			geometryGroup.Children.Add(new LineGeometry(new Point(0.0, 0.0), new Point(1.0, 1.0)));
			this._bodyHeight = geometryGroup.Children[1].Bounds.Height;
			this._topTailHeight = geometryGroup.Children[0].Bounds.Height;
			this._bottomTailHeight = geometryGroup.Children[2].Bounds.Height;
			return geometryGroup;
		}

		// Token: 0x06007CED RID: 31981 RVA: 0x00232704 File Offset: 0x00230904
		private void CheckPosition(ITextPointer position)
		{
			IHighlightRange highlightAnchor = this._highlightAnchor;
			bool flag = highlightAnchor.Range.Contains(position);
			bool flag2 = (bool)this._DPHost.GetValue(StickyNoteControl.IsMouseOverAnchorProperty);
			if (flag != flag2)
			{
				this._DPHost.SetValue(StickyNoteControl.IsMouseOverAnchorPropertyKey, flag);
			}
		}

		// Token: 0x06007CEE RID: 31982 RVA: 0x00232750 File Offset: 0x00230950
		private void OnContainerGotFocus(object sender, KeyboardFocusChangedEventArgs args)
		{
			if (this.HighlightAnchor != null && this.HighlightAnchor.IsSelected(this._selection))
			{
				this.SetSelected(true);
			}
		}

		// Token: 0x06007CEF RID: 31983 RVA: 0x00232774 File Offset: 0x00230974
		private void OnContainerLostFocus(object sender, KeyboardFocusChangedEventArgs args)
		{
			this.SetSelected(false);
		}

		// Token: 0x17001D0D RID: 7437
		// (get) Token: 0x06007CF0 RID: 31984 RVA: 0x0023277D File Offset: 0x0023097D
		// (set) Token: 0x06007CF1 RID: 31985 RVA: 0x00232785 File Offset: 0x00230985
		internal HighlightComponent HighlightAnchor
		{
			get
			{
				return this._highlightAnchor;
			}
			set
			{
				this._highlightAnchor = value;
				if (this._highlightAnchor != null)
				{
					this._highlightAnchor.DefaultBackground = MarkedHighlightComponent.DefaultAnchorBackground;
					this._highlightAnchor.DefaultActiveBackground = MarkedHighlightComponent.DefaultActiveAnchorBackground;
				}
			}
		}

		// Token: 0x06007CF2 RID: 31986 RVA: 0x002327B8 File Offset: 0x002309B8
		private static FlowDirection GetTextFlowDirection(ITextPointer pointer)
		{
			Invariant.Assert(pointer != null, "Null pointer passed.");
			Invariant.Assert(pointer.IsAtInsertionPosition, "Pointer is not an insertion position");
			int num = 0;
			LogicalDirection logicalDirection = pointer.LogicalDirection;
			TextPointerContext pointerContext = pointer.GetPointerContext(logicalDirection);
			FlowDirection result;
			if ((pointerContext == TextPointerContext.ElementEnd || pointerContext == TextPointerContext.ElementStart) && !TextSchema.IsFormattingType(pointer.ParentType))
			{
				result = (FlowDirection)pointer.GetValue(FrameworkElement.FlowDirectionProperty);
			}
			else
			{
				Rect anchorRectangle = TextSelectionHelper.GetAnchorRectangle(pointer);
				ITextPointer textPointer = pointer.GetNextInsertionPosition(logicalDirection);
				if (textPointer != null)
				{
					textPointer = textPointer.CreatePointer((logicalDirection == LogicalDirection.Backward) ? LogicalDirection.Forward : LogicalDirection.Backward);
					if (logicalDirection == LogicalDirection.Forward)
					{
						if (pointerContext == TextPointerContext.ElementEnd && textPointer.GetPointerContext(textPointer.LogicalDirection) == TextPointerContext.ElementStart)
						{
							return (FlowDirection)pointer.GetValue(FrameworkElement.FlowDirectionProperty);
						}
					}
					else if (pointerContext == TextPointerContext.ElementStart && textPointer.GetPointerContext(textPointer.LogicalDirection) == TextPointerContext.ElementEnd)
					{
						return (FlowDirection)pointer.GetValue(FrameworkElement.FlowDirectionProperty);
					}
					Rect anchorRectangle2 = TextSelectionHelper.GetAnchorRectangle(textPointer);
					if (anchorRectangle2 != Rect.Empty && anchorRectangle != Rect.Empty)
					{
						num = Math.Sign(anchorRectangle2.Left - anchorRectangle.Left);
						if (logicalDirection == LogicalDirection.Backward)
						{
							num = -num;
						}
					}
				}
				if (num == 0)
				{
					result = (FlowDirection)pointer.GetValue(FrameworkElement.FlowDirectionProperty);
				}
				else
				{
					result = ((num > 0) ? FlowDirection.LeftToRight : FlowDirection.RightToLeft);
				}
			}
			return result;
		}

		// Token: 0x06007CF3 RID: 31987 RVA: 0x002328F8 File Offset: 0x00230AF8
		private static void OnSelectionChanged(object sender, EventArgs args)
		{
			ITextRange textRange = sender as ITextRange;
			MarkedHighlightComponent.ComponentsRegister componentsRegister = (MarkedHighlightComponent.ComponentsRegister)MarkedHighlightComponent._documentHandlers[textRange];
			if (componentsRegister == null)
			{
				return;
			}
			List<MarkedHighlightComponent> components = componentsRegister.Components;
			bool[] array = new bool[components.Count];
			for (int i = 0; i < components.Count; i++)
			{
				array[i] = components[i].HighlightAnchor.IsSelected(textRange);
				if (!array[i])
				{
					components[i].SetSelected(false);
				}
			}
			for (int j = 0; j < components.Count; j++)
			{
				if (array[j])
				{
					components[j].SetSelected(true);
				}
			}
		}

		// Token: 0x06007CF4 RID: 31988 RVA: 0x002329A0 File Offset: 0x00230BA0
		private static void OnMouseMove(object sender, MouseEventArgs args)
		{
			IServiceProvider serviceProvider = sender as IServiceProvider;
			if (serviceProvider == null)
			{
				return;
			}
			ITextView textView = (ITextView)serviceProvider.GetService(typeof(ITextView));
			if (textView == null || !textView.IsValid)
			{
				return;
			}
			Point position = Mouse.PrimaryDevice.GetPosition(textView.RenderScope);
			ITextPointer textPositionFromPoint = textView.GetTextPositionFromPoint(position, false);
			if (textPositionFromPoint != null)
			{
				MarkedHighlightComponent.CheckAllHighlightRanges(textPositionFromPoint);
			}
		}

		// Token: 0x06007CF5 RID: 31989 RVA: 0x00232A00 File Offset: 0x00230C00
		private static void CheckAllHighlightRanges(ITextPointer pos)
		{
			ITextContainer textContainer = pos.TextContainer;
			ITextRange textSelection = textContainer.TextSelection;
			if (textSelection == null)
			{
				return;
			}
			MarkedHighlightComponent.ComponentsRegister componentsRegister = (MarkedHighlightComponent.ComponentsRegister)MarkedHighlightComponent._documentHandlers[textSelection];
			if (componentsRegister == null)
			{
				return;
			}
			List<MarkedHighlightComponent> components = componentsRegister.Components;
			for (int i = 0; i < components.Count; i++)
			{
				components[i].CheckPosition(pos);
			}
		}

		// Token: 0x04003A80 RID: 14976
		public static DependencyProperty MarkerBrushProperty = DependencyProperty.Register("MarkerBrushProperty", typeof(Brush), typeof(MarkedHighlightComponent));

		// Token: 0x04003A81 RID: 14977
		public static DependencyProperty StrokeThicknessProperty = DependencyProperty.Register("StrokeThicknessProperty", typeof(double), typeof(MarkedHighlightComponent));

		// Token: 0x04003A82 RID: 14978
		internal static Color DefaultAnchorBackground = (Color)ColorConverter.ConvertFromString("#3380FF80");

		// Token: 0x04003A83 RID: 14979
		internal static Color DefaultMarkerColor = (Color)ColorConverter.ConvertFromString("#FF008000");

		// Token: 0x04003A84 RID: 14980
		internal static Color DefaultActiveAnchorBackground = (Color)ColorConverter.ConvertFromString("#3300FF00");

		// Token: 0x04003A85 RID: 14981
		internal static Color DefaultActiveMarkerColor = (Color)ColorConverter.ConvertFromString("#FF008000");

		// Token: 0x04003A86 RID: 14982
		internal static double MarkerStrokeThickness = 1.0;

		// Token: 0x04003A87 RID: 14983
		internal static double ActiveMarkerStrokeThickness = 2.0;

		// Token: 0x04003A88 RID: 14984
		internal static double MarkerVerticalSpace = 2.0;

		// Token: 0x04003A89 RID: 14985
		private static Hashtable _documentHandlers = new Hashtable();

		// Token: 0x04003A8A RID: 14986
		private byte _state;

		// Token: 0x04003A8B RID: 14987
		private HighlightComponent _highlightAnchor;

		// Token: 0x04003A8C RID: 14988
		private double _bodyHeight;

		// Token: 0x04003A8D RID: 14989
		private double _bottomTailHeight;

		// Token: 0x04003A8E RID: 14990
		private double _topTailHeight;

		// Token: 0x04003A8F RID: 14991
		private Path _leftMarker;

		// Token: 0x04003A90 RID: 14992
		private Path _rightMarker;

		// Token: 0x04003A91 RID: 14993
		private DependencyObject _DPHost;

		// Token: 0x04003A92 RID: 14994
		private const byte FocusFlag = 1;

		// Token: 0x04003A93 RID: 14995
		private const byte FocusFlagComplement = 126;

		// Token: 0x04003A94 RID: 14996
		private const byte SelectedFlag = 2;

		// Token: 0x04003A95 RID: 14997
		private const byte SelectedFlagComplement = 125;

		// Token: 0x04003A96 RID: 14998
		private IAttachedAnnotation _attachedAnnotation;

		// Token: 0x04003A97 RID: 14999
		private PresentationContext _presentationContext;

		// Token: 0x04003A98 RID: 15000
		private bool _isDirty = true;

		// Token: 0x04003A99 RID: 15001
		private ITextRange _selection;

		// Token: 0x04003A9A RID: 15002
		private UIElement _uiParent;

		// Token: 0x02000B86 RID: 2950
		private class ComponentsRegister
		{
			// Token: 0x06008E6D RID: 36461 RVA: 0x0025C50B File Offset: 0x0025A70B
			public ComponentsRegister(EventHandler selectionHandler, MouseEventHandler mouseMoveHandler)
			{
				this._components = new List<MarkedHighlightComponent>();
				this._selectionHandler = selectionHandler;
				this._mouseMoveHandler = mouseMoveHandler;
			}

			// Token: 0x06008E6E RID: 36462 RVA: 0x0025C52C File Offset: 0x0025A72C
			public void Add(MarkedHighlightComponent component)
			{
				if (this._components.Count == 0)
				{
					UIElement host = component.PresentationContext.Host;
					if (host != null)
					{
						KeyboardNavigation.SetTabNavigation(host, KeyboardNavigationMode.Local);
						KeyboardNavigation.SetControlTabNavigation(host, KeyboardNavigationMode.Local);
					}
				}
				int i = 0;
				while (i < this._components.Count && this.Compare(this._components[i], component) <= 0)
				{
					i++;
				}
				this._components.Insert(i, component);
				while (i < this._components.Count)
				{
					this._components[i].SetTabIndex(i);
					i++;
				}
			}

			// Token: 0x06008E6F RID: 36463 RVA: 0x0025C5C4 File Offset: 0x0025A7C4
			public void Remove(MarkedHighlightComponent component)
			{
				int i = 0;
				while (i < this._components.Count && this._components[i] != component)
				{
					i++;
				}
				if (i < this._components.Count)
				{
					this._components.RemoveAt(i);
					while (i < this._components.Count)
					{
						this._components[i].SetTabIndex(i);
						i++;
					}
				}
			}

			// Token: 0x17001FB1 RID: 8113
			// (get) Token: 0x06008E70 RID: 36464 RVA: 0x0025C636 File Offset: 0x0025A836
			public List<MarkedHighlightComponent> Components
			{
				get
				{
					return this._components;
				}
			}

			// Token: 0x17001FB2 RID: 8114
			// (get) Token: 0x06008E71 RID: 36465 RVA: 0x0025C63E File Offset: 0x0025A83E
			public EventHandler SelectionHandler
			{
				get
				{
					return this._selectionHandler;
				}
			}

			// Token: 0x17001FB3 RID: 8115
			// (get) Token: 0x06008E72 RID: 36466 RVA: 0x0025C646 File Offset: 0x0025A846
			public MouseEventHandler MouseMoveHandler
			{
				get
				{
					return this._mouseMoveHandler;
				}
			}

			// Token: 0x06008E73 RID: 36467 RVA: 0x0025C650 File Offset: 0x0025A850
			private int Compare(IAnnotationComponent first, IAnnotationComponent second)
			{
				TextAnchor textAnchor = ((IAttachedAnnotation)first.AttachedAnnotations[0]).FullyAttachedAnchor as TextAnchor;
				TextAnchor textAnchor2 = ((IAttachedAnnotation)second.AttachedAnnotations[0]).FullyAttachedAnchor as TextAnchor;
				int num = textAnchor.Start.CompareTo(textAnchor2.Start);
				if (num == 0)
				{
					num = textAnchor.End.CompareTo(textAnchor2.End);
				}
				return num;
			}

			// Token: 0x04004B91 RID: 19345
			private List<MarkedHighlightComponent> _components;

			// Token: 0x04004B92 RID: 19346
			private EventHandler _selectionHandler;

			// Token: 0x04004B93 RID: 19347
			private MouseEventHandler _mouseMoveHandler;
		}
	}
}
