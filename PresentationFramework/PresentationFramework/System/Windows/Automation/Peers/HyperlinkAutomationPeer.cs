using System;
using System.Windows.Automation.Provider;
using System.Windows.Documents;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes <see cref="T:System.Windows.Documents.Hyperlink" /> types to UI Automation.</summary>
	// Token: 0x020002C0 RID: 704
	public class HyperlinkAutomationPeer : TextElementAutomationPeer, IInvokeProvider
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Automation.Peers.HyperlinkAutomationPeer" /> class.</summary>
		/// <param name="owner">The <see cref="T:System.Windows.Documents.Hyperlink" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.HyperlinkAutomationPeer" />.</param>
		// Token: 0x060026F3 RID: 9971 RVA: 0x000B8A79 File Offset: 0x000B6C79
		public HyperlinkAutomationPeer(Hyperlink owner) : base(owner)
		{
		}

		/// <summary>Gets the control pattern for the <see cref="T:System.Windows.Documents.Hyperlink" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.HyperlinkAutomationPeer" />.</summary>
		/// <param name="patternInterface">A value in the enumeration.</param>
		/// <returns>If <paramref name="patternInterface" /> is <see cref="F:System.Windows.Automation.Peers.PatternInterface.Invoke" />, this method returns a <see langword="this" /> pointer; otherwise, this method returns <see langword="null" />.</returns>
		// Token: 0x060026F4 RID: 9972 RVA: 0x000B8A82 File Offset: 0x000B6C82
		public override object GetPattern(PatternInterface patternInterface)
		{
			if (patternInterface == PatternInterface.Invoke)
			{
				return this;
			}
			return base.GetPattern(patternInterface);
		}

		/// <summary>Gets the control type for the <see cref="T:System.Windows.Documents.Hyperlink" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.HyperlinkAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetAutomationControlType" />.</summary>
		/// <returns>The <see cref="F:System.Windows.Automation.Peers.AutomationControlType.Hyperlink" /> enumeration value.</returns>
		// Token: 0x060026F5 RID: 9973 RVA: 0x00094E43 File Offset: 0x00093043
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Hyperlink;
		}

		/// <summary>Gets the text label of the <see cref="T:System.Windows.Documents.Hyperlink" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.HyperlinkAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetName" />.</summary>
		/// <returns>The string that contains the label.</returns>
		// Token: 0x060026F6 RID: 9974 RVA: 0x000B8A90 File Offset: 0x000B6C90
		protected override string GetNameCore()
		{
			string text = base.GetNameCore();
			if (text == string.Empty)
			{
				Hyperlink hyperlink = (Hyperlink)base.Owner;
				text = hyperlink.Text;
				if (text == null)
				{
					text = string.Empty;
				}
			}
			return text;
		}

		/// <summary>Gets the name of the <see cref="T:System.Windows.Documents.Hyperlink" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.HyperlinkAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetClassName" />.</summary>
		/// <returns>A string that contains "Hyperlink".</returns>
		// Token: 0x060026F7 RID: 9975 RVA: 0x000B8ACE File Offset: 0x000B6CCE
		protected override string GetClassNameCore()
		{
			return "Hyperlink";
		}

		/// <summary>Gets or sets a value that indicates whether the <see cref="T:System.Windows.Documents.Hyperlink" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.HyperlinkAutomationPeer" /> is understood by the end user as interactive the user might understand the <see cref="T:System.Windows.Documents.Hyperlink" /> as contributing to the logical structure of the control in the GUI. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.IsControlElement" />.</summary>
		/// <returns>
		///     <see langword="true" />.</returns>
		// Token: 0x060026F8 RID: 9976 RVA: 0x000B8AD8 File Offset: 0x000B6CD8
		protected override bool IsControlElementCore()
		{
			return base.IncludeInvisibleElementsInControlView || base.IsTextViewVisible == true;
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		// Token: 0x060026F9 RID: 9977 RVA: 0x000B8B0C File Offset: 0x000B6D0C
		void IInvokeProvider.Invoke()
		{
			if (!base.IsEnabled())
			{
				throw new ElementNotEnabledException();
			}
			Hyperlink hyperlink = (Hyperlink)base.Owner;
			hyperlink.DoClick();
		}
	}
}
