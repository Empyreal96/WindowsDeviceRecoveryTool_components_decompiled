using System;
using System.Collections.Generic;
using System.Security;
using System.Windows.Documents;
using System.Windows.Interop;
using System.Windows.Media;
using MS.Internal;
using MS.Internal.Automation;
using MS.Internal.Documents;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes <see cref="F:System.Windows.Automation.ControlType.Document" /> control types to UI Automation.</summary>
	// Token: 0x020002AB RID: 683
	public class DocumentAutomationPeer : ContentTextAutomationPeer
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Automation.Peers.DocumentAutomationPeer" /> class.</summary>
		/// <param name="owner">The <see cref="T:System.Windows.FrameworkContentElement" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.DocumentAutomationPeer" />.</param>
		// Token: 0x06002659 RID: 9817 RVA: 0x000B6CDC File Offset: 0x000B4EDC
		public DocumentAutomationPeer(FrameworkContentElement owner) : base(owner)
		{
			if (owner is IServiceProvider)
			{
				this._textContainer = (((IServiceProvider)owner).GetService(typeof(ITextContainer)) as ITextContainer);
				if (this._textContainer != null)
				{
					this._textPattern = new TextAdaptor(this, this._textContainer);
				}
			}
		}

		// Token: 0x0600265A RID: 9818 RVA: 0x000B6D32 File Offset: 0x000B4F32
		internal void OnDisconnected()
		{
			if (this._textPattern != null)
			{
				this._textPattern.Dispose();
				this._textPattern = null;
			}
		}

		/// <summary>Gets the collection of child elements for the <see cref="T:System.Windows.FrameworkContentElement" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.DocumentAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetChildren" />.</summary>
		/// <returns>Because <see cref="T:System.Windows.Automation.Peers.DocumentAutomationPeer" /> gives access to its content through <see cref="T:System.Windows.Automation.TextPattern" />, this method always returns <see langword="null" />.</returns>
		// Token: 0x0600265B RID: 9819 RVA: 0x000B6D50 File Offset: 0x000B4F50
		protected override List<AutomationPeer> GetChildrenCore()
		{
			if (this._childrenStart != null && this._childrenEnd != null)
			{
				ITextContainer textContainer = ((IServiceProvider)base.Owner).GetService(typeof(ITextContainer)) as ITextContainer;
				return TextContainerHelper.GetAutomationPeersFromRange(this._childrenStart, this._childrenEnd, textContainer.Start);
			}
			return null;
		}

		/// <summary>Gets the control pattern for the <see cref="T:System.Windows.FrameworkContentElement" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.DocumentAutomationPeer" />. </summary>
		/// <param name="patternInterface">One of the enumeration values.</param>
		/// <returns>If <paramref name="patternInterface" /> is <see cref="F:System.Windows.Automation.Peers.PatternInterface.Text" />, this method returns an <see cref="T:System.Windows.Automation.Provider.ITextProvider" />.</returns>
		// Token: 0x0600265C RID: 9820 RVA: 0x000B6DA8 File Offset: 0x000B4FA8
		public override object GetPattern(PatternInterface patternInterface)
		{
			object result;
			if (patternInterface == PatternInterface.Text)
			{
				if (this._textPattern == null && base.Owner is IServiceProvider)
				{
					ITextContainer textContainer = ((IServiceProvider)base.Owner).GetService(typeof(ITextContainer)) as ITextContainer;
					if (textContainer != null)
					{
						this._textPattern = new TextAdaptor(this, textContainer);
					}
				}
				result = this._textPattern;
			}
			else
			{
				result = base.GetPattern(patternInterface);
			}
			return result;
		}

		/// <summary>Gets the control type for the control that is associated with this <see cref="T:System.Windows.Automation.Peers.DocumentAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetAutomationControlType" />.</summary>
		/// <returns>
		///     <see cref="F:System.Windows.Automation.Peers.AutomationControlType.Text" /> in all cases.</returns>
		// Token: 0x0600265D RID: 9821 RVA: 0x000965D0 File Offset: 0x000947D0
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Document;
		}

		/// <summary>Gets the name of the <see cref="T:System.Windows.FrameworkContentElement" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.DocumentAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetClassName" />.</summary>
		/// <returns>A string that contains "Document".</returns>
		// Token: 0x0600265E RID: 9822 RVA: 0x000B6E13 File Offset: 0x000B5013
		protected override string GetClassNameCore()
		{
			return "Document";
		}

		/// <summary>Gets or sets a value that indicates whether the <see cref="T:System.Windows.FrameworkContentElement" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.DocumentAutomationPeer" /> is understood by the end user as interactive or the user might understand the <see cref="T:System.Windows.FrameworkContentElement" /> as contributing to the logical structure of the control in the GUI. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.IsControlElement" />.</summary>
		/// <returns>
		///     <see langword="true" /> in all cases.</returns>
		// Token: 0x0600265F RID: 9823 RVA: 0x000B6E1C File Offset: 0x000B501C
		protected override bool IsControlElementCore()
		{
			if (base.IncludeInvisibleElementsInControlView)
			{
				return true;
			}
			ITextContainer textContainer = this._textContainer;
			ITextView textView = (textContainer != null) ? textContainer.TextView : null;
			UIElement uielement = (textView != null) ? textView.RenderScope : null;
			return uielement != null && uielement.IsVisible;
		}

		/// <summary>Gets the <see cref="T:System.Windows.Rect" /> that represents the screen coordinates of the element that is associated with this <see cref="T:System.Windows.Automation.Peers.DocumentAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetBoundingRectangle" />.</summary>
		/// <returns>The bounding rectangle.</returns>
		// Token: 0x06002660 RID: 9824 RVA: 0x000B6E60 File Offset: 0x000B5060
		[SecurityCritical]
		[SecurityTreatAsSafe]
		protected override Rect GetBoundingRectangleCore()
		{
			UIElement uielement;
			Rect rect = this.CalculateBoundingRect(false, out uielement);
			if (rect != Rect.Empty && uielement != null)
			{
				HwndSource hwndSource = PresentationSource.CriticalFromVisual(uielement) as HwndSource;
				if (hwndSource != null)
				{
					rect = PointUtil.ElementToRoot(rect, uielement, hwndSource);
					rect = PointUtil.RootToClient(rect, hwndSource);
					rect = PointUtil.ClientToScreen(rect, hwndSource);
				}
			}
			return rect;
		}

		/// <summary>Gets a <see cref="T:System.Windows.Point" /> that represents the clickable space that is on the <see cref="T:System.Windows.FrameworkContentElement" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.ContentElementAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetClickablePoint" />.</summary>
		/// <returns>The point that represents the clickable space that is on the element.</returns>
		// Token: 0x06002661 RID: 9825 RVA: 0x000B6EB0 File Offset: 0x000B50B0
		[SecurityCritical]
		[SecurityTreatAsSafe]
		protected override Point GetClickablePointCore()
		{
			Point result = default(Point);
			UIElement uielement;
			Rect rect = this.CalculateBoundingRect(true, out uielement);
			if (rect != Rect.Empty && uielement != null)
			{
				HwndSource hwndSource = PresentationSource.CriticalFromVisual(uielement) as HwndSource;
				if (hwndSource != null)
				{
					rect = PointUtil.ElementToRoot(rect, uielement, hwndSource);
					rect = PointUtil.RootToClient(rect, hwndSource);
					rect = PointUtil.ClientToScreen(rect, hwndSource);
					result = new Point(rect.Left + rect.Width * 0.1, rect.Top + rect.Height * 0.1);
				}
			}
			return result;
		}

		/// <summary>Gets a value that indicates whether the <see cref="T:System.Windows.FrameworkContentElement" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.DocumentAutomationPeer" /> is off the screen. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.IsOffscreen" />.</summary>
		/// <returns>
		///     <see langword="true" /> if the element is not on the screen; otherwise, <see langword="false" />.</returns>
		// Token: 0x06002662 RID: 9826 RVA: 0x000B6F44 File Offset: 0x000B5144
		protected override bool IsOffscreenCore()
		{
			IsOffscreenBehavior isOffscreenBehavior = AutomationProperties.GetIsOffscreenBehavior(base.Owner);
			if (isOffscreenBehavior == IsOffscreenBehavior.Onscreen)
			{
				return false;
			}
			if (isOffscreenBehavior != IsOffscreenBehavior.Offscreen)
			{
				UIElement uielement;
				Rect rect = this.CalculateBoundingRect(true, out uielement);
				return DoubleUtil.AreClose(rect, Rect.Empty) || uielement == null;
			}
			return true;
		}

		// Token: 0x06002663 RID: 9827 RVA: 0x000B6F88 File Offset: 0x000B5188
		internal override List<AutomationPeer> GetAutomationPeersFromRange(ITextPointer start, ITextPointer end)
		{
			this._childrenStart = start.CreatePointer();
			this._childrenEnd = end.CreatePointer();
			base.ResetChildrenCache();
			return base.GetChildren();
		}

		// Token: 0x06002664 RID: 9828 RVA: 0x000B6FB0 File Offset: 0x000B51B0
		private Rect CalculateBoundingRect(bool clipToVisible, out UIElement uiScope)
		{
			uiScope = null;
			Rect empty = Rect.Empty;
			if (base.Owner is IServiceProvider)
			{
				ITextContainer textContainer = ((IServiceProvider)base.Owner).GetService(typeof(ITextContainer)) as ITextContainer;
				ITextView textView = (textContainer != null) ? textContainer.TextView : null;
				if (textView != null)
				{
					if (!textView.IsValid)
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
					if (textView != null)
					{
						empty = new Rect(textView.RenderScope.RenderSize);
						uiScope = textView.RenderScope;
						if (clipToVisible)
						{
							Visual visual = textView.RenderScope;
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
						}
					}
				}
			}
			return empty;
		}

		// Token: 0x04001B71 RID: 7025
		private ITextPointer _childrenStart;

		// Token: 0x04001B72 RID: 7026
		private ITextPointer _childrenEnd;

		// Token: 0x04001B73 RID: 7027
		private TextAdaptor _textPattern;

		// Token: 0x04001B74 RID: 7028
		private ITextContainer _textContainer;
	}
}
