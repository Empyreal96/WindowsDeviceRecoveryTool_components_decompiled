using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Documents;
using MS.Internal.Automation;
using MS.Internal.Documents;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes <see cref="T:System.Windows.Controls.RichTextBox" /> types to UI Automation.</summary>
	// Token: 0x020002DA RID: 730
	public class RichTextBoxAutomationPeer : TextAutomationPeer
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Automation.Peers.RichTextBoxAutomationPeer" /> class.</summary>
		/// <param name="owner">The <see cref="T:System.Windows.Controls.RichTextBox" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.RichTextBoxAutomationPeer" />.</param>
		// Token: 0x060027C8 RID: 10184 RVA: 0x000BAAB5 File Offset: 0x000B8CB5
		public RichTextBoxAutomationPeer(RichTextBox owner) : base(owner)
		{
			this._textPattern = new TextAdaptor(this, owner.TextContainer);
		}

		/// <summary>Gets the name of the <see cref="T:System.Windows.Controls.RichTextBox" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.RichTextBoxAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetClassName" />.</summary>
		/// <returns>A string that contains "RichTextBox".</returns>
		// Token: 0x060027C9 RID: 10185 RVA: 0x000BAAD0 File Offset: 0x000B8CD0
		protected override string GetClassNameCore()
		{
			return "RichTextBox";
		}

		/// <summary>Gets the control type for the <see cref="T:System.Windows.Controls.RichTextBox" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.RichTextBoxAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetAutomationControlType" />.</summary>
		/// <returns>The <see cref="F:System.Windows.Automation.Peers.AutomationControlType.Document" /> enumeration value.</returns>
		// Token: 0x060027CA RID: 10186 RVA: 0x000965D0 File Offset: 0x000947D0
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Document;
		}

		/// <summary>Gets the control pattern for the <see cref="T:System.Windows.Controls.RichTextBox" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.RichTextBoxAutomationPeer" />.</summary>
		/// <param name="patternInterface">A value in the enumeration.</param>
		/// <returns>If <paramref name="patternInterface" /> is <see cref="F:System.Windows.Automation.Peers.PatternInterface.Text" />, this method returns an <see cref="T:System.Windows.Automation.Provider.ITextProvider" /> reference. If <paramref name="patternInterface" /> is <see cref="F:System.Windows.Automation.Peers.PatternInterface.Scroll" />, this method returns a new <see cref="T:System.Windows.Automation.Peers.ScrollViewerAutomationPeer" />.  </returns>
		// Token: 0x060027CB RID: 10187 RVA: 0x000BAAD8 File Offset: 0x000B8CD8
		public override object GetPattern(PatternInterface patternInterface)
		{
			object obj = null;
			RichTextBox richTextBox = (RichTextBox)base.Owner;
			if (patternInterface == PatternInterface.Text)
			{
				if (this._textPattern == null)
				{
					this._textPattern = new TextAdaptor(this, richTextBox.TextContainer);
				}
				return this._textPattern;
			}
			if (patternInterface == PatternInterface.Scroll)
			{
				if (richTextBox.ScrollViewer != null)
				{
					obj = richTextBox.ScrollViewer.CreateAutomationPeer();
					((AutomationPeer)obj).EventsSource = this;
				}
			}
			else
			{
				obj = base.GetPattern(patternInterface);
			}
			return obj;
		}

		/// <summary>Gets the collection of child elements for <see cref="T:System.Windows.Controls.RichTextBox" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.RichTextBoxAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetChildren" />.</summary>
		/// <returns>The collection of child elements.</returns>
		// Token: 0x060027CC RID: 10188 RVA: 0x000BAB48 File Offset: 0x000B8D48
		protected override List<AutomationPeer> GetChildrenCore()
		{
			RichTextBox richTextBox = (RichTextBox)base.Owner;
			return TextContainerHelper.GetAutomationPeersFromRange(richTextBox.TextContainer.Start, richTextBox.TextContainer.End, null);
		}

		// Token: 0x060027CD RID: 10189 RVA: 0x000BAB80 File Offset: 0x000B8D80
		internal override List<AutomationPeer> GetAutomationPeersFromRange(ITextPointer start, ITextPointer end)
		{
			base.GetChildren();
			RichTextBox richTextBox = (RichTextBox)base.Owner;
			return TextContainerHelper.GetAutomationPeersFromRange(start, end, richTextBox.TextContainer.Start);
		}

		// Token: 0x04001B94 RID: 7060
		private TextAdaptor _textPattern;
	}
}
