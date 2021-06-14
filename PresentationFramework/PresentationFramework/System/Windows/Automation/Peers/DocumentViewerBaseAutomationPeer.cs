using System;
using System.Collections.Generic;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes <see cref="T:System.Windows.Controls.Primitives.DocumentViewerBase" /> types to UI Automation.</summary>
	// Token: 0x020002AE RID: 686
	public class DocumentViewerBaseAutomationPeer : FrameworkElementAutomationPeer
	{
		/// <summary>Creates a new instance of the <see cref="T:System.Windows.Automation.Peers.DocumentViewerBaseAutomationPeer" /> class.</summary>
		/// <param name="owner">The <see cref="T:System.Windows.Controls.Primitives.DocumentViewerBase" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.DocumentViewerBaseAutomationPeer" />.</param>
		// Token: 0x0600266B RID: 9835 RVA: 0x000B30F9 File Offset: 0x000B12F9
		public DocumentViewerBaseAutomationPeer(DocumentViewerBase owner) : base(owner)
		{
		}

		/// <summary>Gets the control pattern for the <see cref="T:System.Windows.Controls.Primitives.DocumentViewerBase" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.DocumentViewerBaseAutomationPeer" />. </summary>
		/// <param name="patternInterface">One of the enumeration values.</param>
		/// <returns>If <paramref name="patternInterface" /> is <see cref="F:System.Windows.Automation.Peers.PatternInterface.Text" />, this method returns an <see cref="T:System.Windows.Automation.Provider.ITextProvider" /> reference. </returns>
		// Token: 0x0600266C RID: 9836 RVA: 0x000B7194 File Offset: 0x000B5394
		public override object GetPattern(PatternInterface patternInterface)
		{
			object result = null;
			if (patternInterface == PatternInterface.Text)
			{
				base.GetChildren();
				if (this._documentPeer != null)
				{
					this._documentPeer.EventsSource = this;
					result = this._documentPeer.GetPattern(patternInterface);
				}
			}
			else
			{
				result = base.GetPattern(patternInterface);
			}
			return result;
		}

		/// <summary>Gets the collection of child elements of the <see cref="T:System.Windows.Controls.Primitives.DocumentViewerBase" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.DocumentViewerBaseAutomationPeer" />. Called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetChildren" />.</summary>
		/// <returns>The collection of child elements.</returns>
		// Token: 0x0600266D RID: 9837 RVA: 0x000B71DC File Offset: 0x000B53DC
		protected override List<AutomationPeer> GetChildrenCore()
		{
			List<AutomationPeer> list = base.GetChildrenCore();
			AutomationPeer documentAutomationPeer = this.GetDocumentAutomationPeer();
			if (this._documentPeer != documentAutomationPeer)
			{
				if (this._documentPeer != null)
				{
					this._documentPeer.OnDisconnected();
				}
				this._documentPeer = (documentAutomationPeer as DocumentAutomationPeer);
			}
			if (documentAutomationPeer != null)
			{
				if (list == null)
				{
					list = new List<AutomationPeer>();
				}
				list.Add(documentAutomationPeer);
			}
			return list;
		}

		/// <summary>Gets the control type for the <see cref="T:System.Windows.Controls.Primitives.DocumentViewerBase" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.DocumentViewerBaseAutomationPeer" />. Called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetAutomationControlType" />.</summary>
		/// <returns>The <see cref="F:System.Windows.Automation.Peers.AutomationControlType.Document" /> enumeration value.</returns>
		// Token: 0x0600266E RID: 9838 RVA: 0x000965D0 File Offset: 0x000947D0
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Document;
		}

		/// <summary>Gets the name of the <see cref="T:System.Windows.Controls.Primitives.DocumentViewerBase" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.DocumentViewerBaseAutomationPeer" />. Called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetClassName" />.</summary>
		/// <returns>A string that contains "DocumentViewer".</returns>
		// Token: 0x0600266F RID: 9839 RVA: 0x000B7137 File Offset: 0x000B5337
		protected override string GetClassNameCore()
		{
			return "DocumentViewer";
		}

		// Token: 0x06002670 RID: 9840 RVA: 0x000B7234 File Offset: 0x000B5434
		private AutomationPeer GetDocumentAutomationPeer()
		{
			AutomationPeer result = null;
			IDocumentPaginatorSource document = ((DocumentViewerBase)base.Owner).Document;
			if (document != null)
			{
				if (document is UIElement)
				{
					result = UIElementAutomationPeer.CreatePeerForElement((UIElement)document);
				}
				else if (document is ContentElement)
				{
					result = ContentElementAutomationPeer.CreatePeerForElement((ContentElement)document);
				}
			}
			return result;
		}

		// Token: 0x04001B75 RID: 7029
		private DocumentAutomationPeer _documentPeer;
	}
}
