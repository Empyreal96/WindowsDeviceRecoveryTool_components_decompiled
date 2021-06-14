using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;
using MS.Internal.Documents;

namespace MS.Internal.Automation
{
	// Token: 0x020005FA RID: 1530
	internal class TextAdaptor : ITextProvider, IDisposable
	{
		// Token: 0x060065C7 RID: 26055 RVA: 0x001C83E8 File Offset: 0x001C65E8
		internal TextAdaptor(AutomationPeer textPeer, ITextContainer textContainer)
		{
			Invariant.Assert(textContainer != null, "Invalid ITextContainer");
			Invariant.Assert(textPeer is TextAutomationPeer || textPeer is ContentTextAutomationPeer, "Invalid AutomationPeer");
			this._textPeer = textPeer;
			this._textContainer = textContainer;
			this._textContainer.Changed += this.OnTextContainerChanged;
			if (this._textContainer.TextSelection != null)
			{
				this._textContainer.TextSelection.Changed += this.OnTextSelectionChanged;
			}
		}

		// Token: 0x060065C8 RID: 26056 RVA: 0x001C8475 File Offset: 0x001C6675
		public void Dispose()
		{
			if (this._textContainer != null && this._textContainer.TextSelection != null)
			{
				this._textContainer.TextSelection.Changed -= this.OnTextSelectionChanged;
			}
			GC.SuppressFinalize(this);
		}

		// Token: 0x060065C9 RID: 26057 RVA: 0x001C84B0 File Offset: 0x001C66B0
		internal Rect[] GetBoundingRectangles(ITextPointer start, ITextPointer end, bool clipToView, bool transformToScreen)
		{
			ITextView updatedTextView = this.GetUpdatedTextView();
			if (updatedTextView == null)
			{
				return new Rect[0];
			}
			ReadOnlyCollection<TextSegment> textSegments = updatedTextView.TextSegments;
			if (textSegments.Count > 0)
			{
				if (!updatedTextView.Contains(start) && start.CompareTo(textSegments[0].Start) < 0)
				{
					start = textSegments[0].Start.CreatePointer();
				}
				if (!updatedTextView.Contains(end) && end.CompareTo(textSegments[textSegments.Count - 1].End) > 0)
				{
					end = textSegments[textSegments.Count - 1].End.CreatePointer();
				}
			}
			if (!updatedTextView.Contains(start) || !updatedTextView.Contains(end))
			{
				return new Rect[0];
			}
			TextRangeAdaptor.MoveToInsertionPosition(start, LogicalDirection.Forward);
			TextRangeAdaptor.MoveToInsertionPosition(end, LogicalDirection.Backward);
			Rect rect = Rect.Empty;
			if (clipToView)
			{
				rect = this.GetVisibleRectangle(updatedTextView);
				if (rect.IsEmpty)
				{
					return new Rect[0];
				}
			}
			List<Rect> list = new List<Rect>();
			ITextPointer textPointer = start.CreatePointer();
			while (textPointer.CompareTo(end) < 0)
			{
				TextSegment lineRange = updatedTextView.GetLineRange(textPointer);
				if (!lineRange.IsNull)
				{
					ITextPointer startPosition = (lineRange.Start.CompareTo(start) <= 0) ? start : lineRange.Start;
					ITextPointer endPosition = (lineRange.End.CompareTo(end) >= 0) ? end : lineRange.End;
					Rect item = Rect.Empty;
					Geometry tightBoundingGeometryFromTextPositions = updatedTextView.GetTightBoundingGeometryFromTextPositions(startPosition, endPosition);
					if (tightBoundingGeometryFromTextPositions != null)
					{
						item = tightBoundingGeometryFromTextPositions.Bounds;
						if (clipToView)
						{
							item.Intersect(rect);
						}
						if (!item.IsEmpty)
						{
							if (transformToScreen)
							{
								item = new Rect(this.ClientToScreen(item.TopLeft, updatedTextView.RenderScope), this.ClientToScreen(item.BottomRight, updatedTextView.RenderScope));
							}
							list.Add(item);
						}
					}
				}
				if (textPointer.MoveToLineBoundary(1) == 0)
				{
					textPointer = end;
				}
			}
			return list.ToArray();
		}

		// Token: 0x060065CA RID: 26058 RVA: 0x001C869C File Offset: 0x001C689C
		internal ITextView GetUpdatedTextView()
		{
			ITextView textView = this._textContainer.TextView;
			if (textView != null && !textView.IsValid)
			{
				if (!textView.Validate())
				{
					textView = null;
				}
				if (textView != null && !textView.IsValid)
				{
					textView = null;
				}
			}
			return textView;
		}

		// Token: 0x060065CB RID: 26059 RVA: 0x001C86D8 File Offset: 0x001C68D8
		internal void Select(ITextPointer start, ITextPointer end)
		{
			if (this._textContainer.TextSelection != null)
			{
				this._textContainer.TextSelection.Select(start, end);
			}
		}

		// Token: 0x060065CC RID: 26060 RVA: 0x001C86FC File Offset: 0x001C68FC
		internal void ScrollIntoView(ITextPointer start, ITextPointer end, bool alignToTop)
		{
			Rect rect = Rect.Empty;
			Rect[] boundingRectangles = this.GetBoundingRectangles(start, end, false, false);
			foreach (Rect rect2 in boundingRectangles)
			{
				rect.Union(rect2);
			}
			ITextView updatedTextView = this.GetUpdatedTextView();
			if (updatedTextView != null && !rect.IsEmpty)
			{
				Rect visibleRectangle = this.GetVisibleRectangle(updatedTextView);
				Rect rect3 = Rect.Intersect(rect, visibleRectangle);
				if (rect3 == rect)
				{
					return;
				}
				UIElement renderScope = updatedTextView.RenderScope;
				Visual visual = renderScope;
				while (visual != null)
				{
					IScrollInfo scrollInfo = visual as IScrollInfo;
					if (scrollInfo != null)
					{
						if (visual != renderScope)
						{
							GeneralTransform generalTransform = renderScope.TransformToAncestor(visual);
							rect = generalTransform.TransformBounds(rect);
						}
						if (scrollInfo.CanHorizontallyScroll)
						{
							scrollInfo.SetHorizontalOffset(alignToTop ? rect.Left : (rect.Right - scrollInfo.ViewportWidth));
						}
						if (scrollInfo.CanVerticallyScroll)
						{
							scrollInfo.SetVerticalOffset(alignToTop ? rect.Top : (rect.Bottom - scrollInfo.ViewportHeight));
							break;
						}
						break;
					}
					else
					{
						visual = (VisualTreeHelper.GetParent(visual) as Visual);
					}
				}
				FrameworkElement frameworkElement = renderScope as FrameworkElement;
				if (frameworkElement != null)
				{
					frameworkElement.BringIntoView(rect3);
					return;
				}
			}
			else
			{
				ITextPointer textPointer = alignToTop ? start.CreatePointer() : end.CreatePointer();
				textPointer.MoveToElementEdge(alignToTop ? ElementEdge.AfterStart : ElementEdge.AfterEnd);
				FrameworkContentElement frameworkContentElement = textPointer.GetAdjacentElement(LogicalDirection.Backward) as FrameworkContentElement;
				if (frameworkContentElement != null)
				{
					frameworkContentElement.BringIntoView();
				}
			}
		}

		// Token: 0x060065CD RID: 26061 RVA: 0x001C886F File Offset: 0x001C6A6F
		private void OnTextContainerChanged(object sender, TextContainerChangedEventArgs e)
		{
			this._textPeer.RaiseAutomationEvent(AutomationEvents.TextPatternOnTextChanged);
		}

		// Token: 0x060065CE RID: 26062 RVA: 0x001C887E File Offset: 0x001C6A7E
		private void OnTextSelectionChanged(object sender, EventArgs e)
		{
			this._textPeer.RaiseAutomationEvent(AutomationEvents.TextPatternOnTextSelectionChanged);
		}

		// Token: 0x060065CF RID: 26063 RVA: 0x001C8890 File Offset: 0x001C6A90
		private Rect GetVisibleRectangle(ITextView textView)
		{
			Rect empty = new Rect(textView.RenderScope.RenderSize);
			Visual visual = VisualTreeHelper.GetParent(textView.RenderScope) as Visual;
			while (visual != null && empty != Rect.Empty)
			{
				if (VisualTreeHelper.GetClip(visual) != null)
				{
					GeneralTransform inverse = textView.RenderScope.TransformToAncestor(visual).Inverse;
					if (inverse != null)
					{
						Rect rect = VisualTreeHelper.GetClip(visual).Bounds;
						rect = inverse.TransformBounds(rect);
						empty.Intersect(rect);
					}
					else
					{
						empty = Rect.Empty;
					}
				}
				visual = (VisualTreeHelper.GetParent(visual) as Visual);
			}
			return empty;
		}

		// Token: 0x060065D0 RID: 26064 RVA: 0x001C8920 File Offset: 0x001C6B20
		private Point ClientToScreen(Point point, Visual visual)
		{
			if (AccessibilitySwitches.UseNetFx472CompatibleAccessibilityFeatures)
			{
				return this.ObsoleteClientToScreen(point, visual);
			}
			try
			{
				point = visual.PointToScreen(point);
			}
			catch (InvalidOperationException)
			{
			}
			return point;
		}

		// Token: 0x060065D1 RID: 26065 RVA: 0x001C8960 File Offset: 0x001C6B60
		[SecuritySafeCritical]
		private Point ObsoleteClientToScreen(Point point, Visual visual)
		{
			PresentationSource presentationSource = PresentationSource.CriticalFromVisual(visual);
			if (presentationSource != null)
			{
				GeneralTransform generalTransform = visual.TransformToAncestor(presentationSource.RootVisual);
				if (generalTransform != null)
				{
					point = generalTransform.Transform(point);
				}
			}
			return PointUtil.ClientToScreen(point, presentationSource);
		}

		// Token: 0x060065D2 RID: 26066 RVA: 0x001C8998 File Offset: 0x001C6B98
		private Point ScreenToClient(Point point, Visual visual)
		{
			if (AccessibilitySwitches.UseNetFx472CompatibleAccessibilityFeatures)
			{
				return this.ObsoleteScreenToClient(point, visual);
			}
			try
			{
				point = visual.PointFromScreen(point);
			}
			catch (InvalidOperationException)
			{
			}
			return point;
		}

		// Token: 0x060065D3 RID: 26067 RVA: 0x001C89D8 File Offset: 0x001C6BD8
		[SecuritySafeCritical]
		private Point ObsoleteScreenToClient(Point point, Visual visual)
		{
			PresentationSource presentationSource = PresentationSource.CriticalFromVisual(visual);
			point = PointUtil.ScreenToClient(point, presentationSource);
			if (presentationSource != null)
			{
				GeneralTransform generalTransform = visual.TransformToAncestor(presentationSource.RootVisual);
				if (generalTransform != null)
				{
					generalTransform = generalTransform.Inverse;
					if (generalTransform != null)
					{
						point = generalTransform.Transform(point);
					}
				}
			}
			return point;
		}

		// Token: 0x060065D4 RID: 26068 RVA: 0x001C8A1C File Offset: 0x001C6C1C
		ITextRangeProvider[] ITextProvider.GetSelection()
		{
			ITextRange textSelection = this._textContainer.TextSelection;
			if (textSelection == null)
			{
				throw new InvalidOperationException(SR.Get("TextProvider_TextSelectionNotSupported"));
			}
			return new ITextRangeProvider[]
			{
				new TextRangeAdaptor(this, textSelection.Start, textSelection.End, this._textPeer)
			};
		}

		// Token: 0x060065D5 RID: 26069 RVA: 0x001C8A6C File Offset: 0x001C6C6C
		ITextRangeProvider[] ITextProvider.GetVisibleRanges()
		{
			ITextRangeProvider[] array = null;
			ITextView updatedTextView = this.GetUpdatedTextView();
			if (updatedTextView != null)
			{
				List<TextSegment> list = new List<TextSegment>();
				if (updatedTextView is MultiPageTextView)
				{
					list.AddRange(updatedTextView.TextSegments);
				}
				else
				{
					Rect visibleRectangle = this.GetVisibleRectangle(updatedTextView);
					if (!visibleRectangle.IsEmpty)
					{
						ITextPointer textPositionFromPoint = updatedTextView.GetTextPositionFromPoint(visibleRectangle.TopLeft, true);
						ITextPointer textPositionFromPoint2 = updatedTextView.GetTextPositionFromPoint(visibleRectangle.BottomRight, true);
						list.Add(new TextSegment(textPositionFromPoint, textPositionFromPoint2, true));
					}
				}
				if (list.Count > 0)
				{
					array = new ITextRangeProvider[list.Count];
					for (int i = 0; i < list.Count; i++)
					{
						array[i] = new TextRangeAdaptor(this, list[i].Start, list[i].End, this._textPeer);
					}
				}
			}
			if (array == null)
			{
				array = new ITextRangeProvider[]
				{
					new TextRangeAdaptor(this, this._textContainer.Start, this._textContainer.Start, this._textPeer)
				};
			}
			return array;
		}

		// Token: 0x060065D6 RID: 26070 RVA: 0x001C8B74 File Offset: 0x001C6D74
		ITextRangeProvider ITextProvider.RangeFromChild(IRawElementProviderSimple childElementProvider)
		{
			if (childElementProvider == null)
			{
				throw new ArgumentNullException("childElementProvider");
			}
			DependencyObject dependencyObject;
			if (this._textPeer is TextAutomationPeer)
			{
				dependencyObject = ((TextAutomationPeer)this._textPeer).ElementFromProvider(childElementProvider);
			}
			else
			{
				dependencyObject = ((ContentTextAutomationPeer)this._textPeer).ElementFromProvider(childElementProvider);
			}
			TextRangeAdaptor textRangeAdaptor = null;
			if (dependencyObject != null)
			{
				ITextPointer textPointer = null;
				ITextPointer textPointer2 = null;
				if (dependencyObject is TextElement)
				{
					textPointer = ((TextElement)dependencyObject).ElementStart;
					textPointer2 = ((TextElement)dependencyObject).ElementEnd;
				}
				else
				{
					DependencyObject parent = LogicalTreeHelper.GetParent(dependencyObject);
					if (parent is InlineUIContainer || parent is BlockUIContainer)
					{
						textPointer = ((TextElement)parent).ContentStart;
						textPointer2 = ((TextElement)parent).ContentEnd;
					}
					else
					{
						ITextPointer textPointer3 = this._textContainer.Start.CreatePointer();
						while (textPointer3.CompareTo(this._textContainer.End) < 0)
						{
							TextPointerContext pointerContext = textPointer3.GetPointerContext(LogicalDirection.Forward);
							if (pointerContext == TextPointerContext.ElementStart)
							{
								if (dependencyObject == textPointer3.GetAdjacentElement(LogicalDirection.Forward))
								{
									textPointer = textPointer3.CreatePointer(LogicalDirection.Forward);
									textPointer3.MoveToElementEdge(ElementEdge.AfterEnd);
									textPointer2 = textPointer3.CreatePointer(LogicalDirection.Backward);
									break;
								}
							}
							else if (pointerContext == TextPointerContext.EmbeddedElement && dependencyObject == textPointer3.GetAdjacentElement(LogicalDirection.Forward))
							{
								textPointer = textPointer3.CreatePointer(LogicalDirection.Forward);
								textPointer3.MoveToNextContextPosition(LogicalDirection.Forward);
								textPointer2 = textPointer3.CreatePointer(LogicalDirection.Backward);
								break;
							}
							textPointer3.MoveToNextContextPosition(LogicalDirection.Forward);
						}
					}
				}
				if (textPointer != null && textPointer2 != null)
				{
					textRangeAdaptor = new TextRangeAdaptor(this, textPointer, textPointer2, this._textPeer);
				}
			}
			if (textRangeAdaptor == null)
			{
				throw new InvalidOperationException(SR.Get("TextProvider_InvalidChildElement"));
			}
			return textRangeAdaptor;
		}

		// Token: 0x060065D7 RID: 26071 RVA: 0x001C8CF0 File Offset: 0x001C6EF0
		ITextRangeProvider ITextProvider.RangeFromPoint(Point location)
		{
			TextRangeAdaptor textRangeAdaptor = null;
			ITextView updatedTextView = this.GetUpdatedTextView();
			if (updatedTextView != null)
			{
				location = this.ScreenToClient(location, updatedTextView.RenderScope);
				ITextPointer textPositionFromPoint = updatedTextView.GetTextPositionFromPoint(location, true);
				if (textPositionFromPoint != null)
				{
					textRangeAdaptor = new TextRangeAdaptor(this, textPositionFromPoint, textPositionFromPoint, this._textPeer);
				}
			}
			if (textRangeAdaptor == null)
			{
				throw new ArgumentException(SR.Get("TextProvider_InvalidPoint"));
			}
			return textRangeAdaptor;
		}

		// Token: 0x17001872 RID: 6258
		// (get) Token: 0x060065D8 RID: 26072 RVA: 0x001C8D47 File Offset: 0x001C6F47
		ITextRangeProvider ITextProvider.DocumentRange
		{
			get
			{
				return new TextRangeAdaptor(this, this._textContainer.Start, this._textContainer.End, this._textPeer);
			}
		}

		// Token: 0x17001873 RID: 6259
		// (get) Token: 0x060065D9 RID: 26073 RVA: 0x001C8D6B File Offset: 0x001C6F6B
		SupportedTextSelection ITextProvider.SupportedTextSelection
		{
			get
			{
				if (this._textContainer.TextSelection != null)
				{
					return SupportedTextSelection.Single;
				}
				return SupportedTextSelection.None;
			}
		}

		// Token: 0x040032EB RID: 13035
		private AutomationPeer _textPeer;

		// Token: 0x040032EC RID: 13036
		private ITextContainer _textContainer;
	}
}
