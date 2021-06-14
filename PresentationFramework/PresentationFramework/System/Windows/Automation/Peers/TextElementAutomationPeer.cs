using System;
using System.Collections.Generic;
using System.Security;
using System.Windows.Documents;
using System.Windows.Interop;
using System.Windows.Media;
using MS.Internal;
using MS.Internal.Documents;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes <see cref="T:System.Windows.Documents.TextElement" /> types to UI Automation.</summary>
	// Token: 0x020002EC RID: 748
	public class TextElementAutomationPeer : ContentTextAutomationPeer
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Automation.Peers.TextElementAutomationPeer" /> class.</summary>
		/// <param name="owner">The <see cref="T:System.Windows.Documents.TextElement" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.TextElementAutomationPeer" />.</param>
		// Token: 0x0600284A RID: 10314 RVA: 0x000BC02D File Offset: 0x000BA22D
		public TextElementAutomationPeer(TextElement owner) : base(owner)
		{
		}

		/// <summary>Gets the collection of child elements of the <see cref="T:System.Windows.Documents.TextElement" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.TextElementAutomationPeer" />. Called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetChildren" />.</summary>
		/// <returns>
		///     <see langword="null" />.</returns>
		// Token: 0x0600284B RID: 10315 RVA: 0x000BC038 File Offset: 0x000BA238
		protected override List<AutomationPeer> GetChildrenCore()
		{
			TextElement textElement = (TextElement)base.Owner;
			return TextContainerHelper.GetAutomationPeersFromRange(textElement.ContentStart, textElement.ContentEnd, null);
		}

		/// <summary>Gets the <see cref="T:System.Windows.Rect" /> representing the bounding rectangle of the <see cref="T:System.Windows.Documents.TextElement" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.TextElementAutomationPeer" />. Called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetBoundingRectangle" />.</summary>
		/// <returns>The <see cref="T:System.Windows.Rect" /> that contains the coordinates of the element, or <see cref="P:System.Windows.Rect.Empty" /> if the element is not a <see cref="T:System.Windows.Interop.HwndSource" /> and a <see cref="T:System.Windows.PresentationSource" />.</returns>
		// Token: 0x0600284C RID: 10316 RVA: 0x000BC064 File Offset: 0x000BA264
		[SecurityCritical]
		[SecurityTreatAsSafe]
		protected override Rect GetBoundingRectangleCore()
		{
			TextElement textElement = (TextElement)base.Owner;
			ITextView textView = textElement.TextContainer.TextView;
			if (textView == null || !textView.IsValid)
			{
				return Rect.Empty;
			}
			Geometry tightBoundingGeometryFromTextPositions = textView.GetTightBoundingGeometryFromTextPositions(textElement.ContentStart, textElement.ContentEnd);
			if (tightBoundingGeometryFromTextPositions == null)
			{
				return Rect.Empty;
			}
			PresentationSource presentationSource = PresentationSource.CriticalFromVisual(textView.RenderScope);
			if (presentationSource == null)
			{
				return Rect.Empty;
			}
			HwndSource hwndSource = presentationSource as HwndSource;
			if (hwndSource == null)
			{
				return Rect.Empty;
			}
			Rect bounds = tightBoundingGeometryFromTextPositions.Bounds;
			Rect rectRoot = PointUtil.ElementToRoot(bounds, textView.RenderScope, presentationSource);
			Rect rectClient = PointUtil.RootToClient(rectRoot, presentationSource);
			return PointUtil.ClientToScreen(rectClient, hwndSource);
		}

		/// <summary>Gets a <see cref="T:System.Windows.Point" /> that represents the clickable space that is on the <see cref="T:System.Windows.Documents.TextElement" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.TextElementAutomationPeer" />. Called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetClickablePoint" />.</summary>
		/// <returns>The <see cref="T:System.Windows.Point" /> on the element that allows a click. The point values are (<see cref="F:System.Double.NaN" />, <see cref="F:System.Double.NaN" />) if the element is not a <see cref="T:System.Windows.Interop.HwndSource" /> and a <see cref="T:System.Windows.PresentationSource" />.</returns>
		// Token: 0x0600284D RID: 10317 RVA: 0x000BC10C File Offset: 0x000BA30C
		[SecurityCritical]
		[SecurityTreatAsSafe]
		protected override Point GetClickablePointCore()
		{
			Point result = default(Point);
			TextElement textElement = (TextElement)base.Owner;
			ITextView textView = textElement.TextContainer.TextView;
			if (textView == null || !textView.IsValid || (!textView.Contains(textElement.ContentStart) && !textView.Contains(textElement.ContentEnd)))
			{
				return result;
			}
			PresentationSource presentationSource = PresentationSource.CriticalFromVisual(textView.RenderScope);
			if (presentationSource == null)
			{
				return result;
			}
			HwndSource hwndSource = presentationSource as HwndSource;
			if (hwndSource == null)
			{
				return result;
			}
			TextPointer textPointer = textElement.ContentStart.GetNextInsertionPosition(LogicalDirection.Forward);
			if (textPointer == null || textPointer.CompareTo(textElement.ContentEnd) > 0)
			{
				textPointer = textElement.ContentEnd;
			}
			Rect rectElement = this.CalculateVisibleRect(textView, textElement, textElement.ContentStart, textPointer);
			Rect rectRoot = PointUtil.ElementToRoot(rectElement, textView.RenderScope, presentationSource);
			Rect rectClient = PointUtil.RootToClient(rectRoot, presentationSource);
			Rect rect = PointUtil.ClientToScreen(rectClient, hwndSource);
			result = new Point(rect.Left + rect.Width * 0.5, rect.Top + rect.Height * 0.5);
			return result;
		}

		/// <summary>Gets a value that indicates whether <see cref="T:System.Windows.Documents.TextElement" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.TextElementAutomationPeer" /> is off of the screen. Called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.IsOffscreen" />.</summary>
		/// <returns>
		///     <see langword="true" /> if the element is not on the screen; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600284E RID: 10318 RVA: 0x000BC21C File Offset: 0x000BA41C
		protected override bool IsOffscreenCore()
		{
			IsOffscreenBehavior isOffscreenBehavior = AutomationProperties.GetIsOffscreenBehavior(base.Owner);
			if (isOffscreenBehavior == IsOffscreenBehavior.Onscreen)
			{
				return false;
			}
			if (isOffscreenBehavior != IsOffscreenBehavior.Offscreen)
			{
				TextElement textElement = (TextElement)base.Owner;
				ITextView textView = textElement.TextContainer.TextView;
				return textView == null || !textView.IsValid || (!textView.Contains(textElement.ContentStart) && !textView.Contains(textElement.ContentEnd)) || this.CalculateVisibleRect(textView, textElement, textElement.ContentStart, textElement.ContentEnd) == Rect.Empty;
			}
			return true;
		}

		// Token: 0x0600284F RID: 10319 RVA: 0x000BC2A8 File Offset: 0x000BA4A8
		private Rect CalculateVisibleRect(ITextView textView, TextElement textElement, TextPointer startPointer, TextPointer endPointer)
		{
			Geometry tightBoundingGeometryFromTextPositions = textView.GetTightBoundingGeometryFromTextPositions(startPointer, endPointer);
			Rect rect = (tightBoundingGeometryFromTextPositions != null) ? tightBoundingGeometryFromTextPositions.Bounds : Rect.Empty;
			Visual visual = textView.RenderScope;
			while (visual != null && rect != Rect.Empty)
			{
				if (VisualTreeHelper.GetClip(visual) != null)
				{
					GeneralTransform inverse = textView.RenderScope.TransformToAncestor(visual).Inverse;
					if (inverse == null)
					{
						return Rect.Empty;
					}
					Rect rect2 = VisualTreeHelper.GetClip(visual).Bounds;
					rect2 = inverse.TransformBounds(rect2);
					rect.Intersect(rect2);
				}
				visual = (VisualTreeHelper.GetParent(visual) as Visual);
			}
			return rect;
		}

		// Token: 0x06002850 RID: 10320 RVA: 0x000BC33C File Offset: 0x000BA53C
		internal override List<AutomationPeer> GetAutomationPeersFromRange(ITextPointer start, ITextPointer end)
		{
			base.GetChildren();
			TextElement textElement = (TextElement)base.Owner;
			return TextContainerHelper.GetAutomationPeersFromRange(start, end, textElement.ContentStart);
		}

		// Token: 0x170009C4 RID: 2500
		// (get) Token: 0x06002851 RID: 10321 RVA: 0x000BC36C File Offset: 0x000BA56C
		internal bool? IsTextViewVisible
		{
			get
			{
				TextElement textElement = (TextElement)base.Owner;
				ITextView textView;
				if (textElement == null)
				{
					textView = null;
				}
				else
				{
					TextContainer textContainer = textElement.TextContainer;
					textView = ((textContainer != null) ? textContainer.TextView : null);
				}
				ITextView textView2 = textView;
				UIElement uielement = (textView2 != null) ? textView2.RenderScope : null;
				if (uielement == null)
				{
					return null;
				}
				return new bool?(uielement.IsVisible);
			}
		}
	}
}
