using System;
using System.Collections.Generic;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Documents;
using MS.Internal.Documents;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes <see cref="T:System.Windows.Controls.FlowDocumentScrollViewer" /> types to UI Automation.</summary>
	// Token: 0x020002B3 RID: 691
	public class FlowDocumentScrollViewerAutomationPeer : FrameworkElementAutomationPeer
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Automation.Peers.FlowDocumentScrollViewerAutomationPeer" /> class.</summary>
		/// <param name="owner">The <see cref="T:System.Windows.Controls.FlowDocumentScrollViewer" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.FlowDocumentScrollViewerAutomationPeer" />.</param>
		// Token: 0x0600268F RID: 9871 RVA: 0x000B30F9 File Offset: 0x000B12F9
		public FlowDocumentScrollViewerAutomationPeer(FlowDocumentScrollViewer owner) : base(owner)
		{
		}

		/// <summary>Gets the control pattern for the <see cref="T:System.Windows.Controls.FlowDocumentScrollViewer" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.FlowDocumentScrollViewerAutomationPeer" />. </summary>
		/// <param name="patternInterface">One of the enumeration values.</param>
		/// <returns>An object that supports the control pattern if <paramref name="patternInterface" /> is a supported value; otherwise, <see langword="null" />. </returns>
		// Token: 0x06002690 RID: 9872 RVA: 0x000B7764 File Offset: 0x000B5964
		public override object GetPattern(PatternInterface patternInterface)
		{
			object result = null;
			if (patternInterface == PatternInterface.Scroll)
			{
				FlowDocumentScrollViewer flowDocumentScrollViewer = (FlowDocumentScrollViewer)base.Owner;
				if (flowDocumentScrollViewer.ScrollViewer != null)
				{
					AutomationPeer automationPeer = UIElementAutomationPeer.CreatePeerForElement(flowDocumentScrollViewer.ScrollViewer);
					if (automationPeer != null && automationPeer is IScrollProvider)
					{
						automationPeer.EventsSource = this;
						result = automationPeer;
					}
				}
			}
			else if (patternInterface == PatternInterface.Text)
			{
				base.GetChildren();
				if (this._documentPeer != null)
				{
					this._documentPeer.EventsSource = this;
					result = this._documentPeer.GetPattern(patternInterface);
				}
			}
			else if (patternInterface == PatternInterface.SynchronizedInput)
			{
				result = base.GetPattern(patternInterface);
			}
			return result;
		}

		/// <summary>Gets the collection of child elements of the <see cref="T:System.Windows.Controls.FlowDocumentScrollViewer" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.FlowDocumentScrollViewerAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetChildren" />.</summary>
		/// <returns>The collection of child elements.</returns>
		// Token: 0x06002691 RID: 9873 RVA: 0x000B77EC File Offset: 0x000B59EC
		protected override List<AutomationPeer> GetChildrenCore()
		{
			List<AutomationPeer> list = base.GetChildrenCore();
			if (!(base.Owner is IFlowDocumentViewer))
			{
				FlowDocument document = ((FlowDocumentScrollViewer)base.Owner).Document;
				if (document != null)
				{
					AutomationPeer automationPeer = ContentElementAutomationPeer.CreatePeerForElement(document);
					if (this._documentPeer != automationPeer)
					{
						if (this._documentPeer != null)
						{
							this._documentPeer.OnDisconnected();
						}
						this._documentPeer = (automationPeer as DocumentAutomationPeer);
					}
					if (automationPeer != null)
					{
						if (list == null)
						{
							list = new List<AutomationPeer>();
						}
						list.Add(automationPeer);
					}
				}
			}
			return list;
		}

		/// <summary>Gets the control type for the <see cref="T:System.Windows.Controls.FlowDocumentScrollViewer" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.FlowDocumentScrollViewerAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetAutomationControlType" />.</summary>
		/// <returns>The <see cref="F:System.Windows.Automation.Peers.AutomationControlType.Document" /> enumeration value.</returns>
		// Token: 0x06002692 RID: 9874 RVA: 0x000965D0 File Offset: 0x000947D0
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Document;
		}

		/// <summary>Gets the name of the <see cref="T:System.Windows.Controls.FlowDocumentScrollViewer" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.FlowDocumentScrollViewerAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetClassName" />.</summary>
		/// <returns>A string that contains "FlowDocumentScrollViewer".</returns>
		// Token: 0x06002693 RID: 9875 RVA: 0x000B7864 File Offset: 0x000B5A64
		protected override string GetClassNameCore()
		{
			return "FlowDocumentScrollViewer";
		}

		// Token: 0x04001B77 RID: 7031
		private DocumentAutomationPeer _documentPeer;
	}
}
