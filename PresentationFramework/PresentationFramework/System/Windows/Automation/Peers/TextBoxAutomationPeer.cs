using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using MS.Internal.Automation;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes <see cref="T:System.Windows.Controls.TextBox" /> types to UI Automation.</summary>
	// Token: 0x020002EA RID: 746
	public class TextBoxAutomationPeer : TextAutomationPeer, IValueProvider
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Automation.Peers.TextBoxAutomationPeer" /> class.</summary>
		/// <param name="owner">The <see cref="T:System.Windows.Controls.TextBox" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.TextBoxAutomationPeer" />.</param>
		// Token: 0x0600283C RID: 10300 RVA: 0x000BBEF3 File Offset: 0x000BA0F3
		public TextBoxAutomationPeer(TextBox owner) : base(owner)
		{
			this._textPattern = new TextAdaptor(this, owner.TextContainer);
		}

		/// <summary>Gets the name of the <see cref="T:System.Windows.Controls.TextBox" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.TextBoxAutomationPeer" />. Called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetClassName" />.</summary>
		/// <returns>A string that contains "TextBox".</returns>
		// Token: 0x0600283D RID: 10301 RVA: 0x000BBF0E File Offset: 0x000BA10E
		protected override string GetClassNameCore()
		{
			return "TextBox";
		}

		/// <summary>Gets the control type for the <see cref="T:System.Windows.Controls.TextBox" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.TextBoxAutomationPeer" />. Called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetAutomationControlType" />.</summary>
		/// <returns>The <see cref="F:System.Windows.Automation.Peers.AutomationControlType.Edit" /> enumeration value.</returns>
		// Token: 0x0600283E RID: 10302 RVA: 0x00094CFC File Offset: 0x00092EFC
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Edit;
		}

		/// <summary>Gets the control pattern for the <see cref="T:System.Windows.Controls.TextBox" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.TextBoxAutomationPeer" />.</summary>
		/// <param name="patternInterface">A value from the enumeration.</param>
		/// <returns>An object that supports the control pattern if <paramref name="patternInterface" /> is a supported value; otherwise, <see langword="null" />.</returns>
		// Token: 0x0600283F RID: 10303 RVA: 0x000BBF18 File Offset: 0x000BA118
		public override object GetPattern(PatternInterface patternInterface)
		{
			object obj = null;
			if (patternInterface == PatternInterface.Value)
			{
				obj = this;
			}
			if (patternInterface == PatternInterface.Text)
			{
				if (this._textPattern == null)
				{
					this._textPattern = new TextAdaptor(this, ((TextBoxBase)base.Owner).TextContainer);
				}
				return this._textPattern;
			}
			if (patternInterface == PatternInterface.Scroll)
			{
				TextBox textBox = (TextBox)base.Owner;
				if (textBox.ScrollViewer != null)
				{
					obj = textBox.ScrollViewer.CreateAutomationPeer();
					((AutomationPeer)obj).EventsSource = this;
				}
			}
			if (patternInterface == PatternInterface.SynchronizedInput)
			{
				obj = base.GetPattern(patternInterface);
			}
			return obj;
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <returns>
		///     <see langword="true" /> if the value is read-only; <see langword="false" /> if it can be modified.</returns>
		// Token: 0x170009C2 RID: 2498
		// (get) Token: 0x06002840 RID: 10304 RVA: 0x000BBF9C File Offset: 0x000BA19C
		bool IValueProvider.IsReadOnly
		{
			get
			{
				TextBox textBox = (TextBox)base.Owner;
				return textBox.IsReadOnly;
			}
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <returns>A string value of the control.</returns>
		// Token: 0x170009C3 RID: 2499
		// (get) Token: 0x06002841 RID: 10305 RVA: 0x000BBFBC File Offset: 0x000BA1BC
		string IValueProvider.Value
		{
			get
			{
				TextBox textBox = (TextBox)base.Owner;
				return textBox.Text;
			}
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <param name="value"> The value of a control.</param>
		// Token: 0x06002842 RID: 10306 RVA: 0x000BBFDC File Offset: 0x000BA1DC
		void IValueProvider.SetValue(string value)
		{
			if (!base.IsEnabled())
			{
				throw new ElementNotEnabledException();
			}
			TextBox textBox = (TextBox)base.Owner;
			if (textBox.IsReadOnly)
			{
				throw new ElementNotEnabledException();
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			textBox.Text = value;
		}

		// Token: 0x06002843 RID: 10307 RVA: 0x000B3FD9 File Offset: 0x000B21D9
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal void RaiseValuePropertyChangedEvent(string oldValue, string newValue)
		{
			if (oldValue != newValue)
			{
				base.RaisePropertyChangedEvent(ValuePatternIdentifiers.ValueProperty, oldValue, newValue);
			}
		}

		// Token: 0x06002844 RID: 10308 RVA: 0x000BA790 File Offset: 0x000B8990
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal void RaiseIsReadOnlyPropertyChangedEvent(bool oldValue, bool newValue)
		{
			if (oldValue != newValue)
			{
				base.RaisePropertyChangedEvent(ValuePatternIdentifiers.IsReadOnlyProperty, oldValue, newValue);
			}
		}

		// Token: 0x06002845 RID: 10309 RVA: 0x000BA7AD File Offset: 0x000B89AD
		internal override List<AutomationPeer> GetAutomationPeersFromRange(ITextPointer start, ITextPointer end)
		{
			return new List<AutomationPeer>();
		}

		// Token: 0x04001B97 RID: 7063
		private TextAdaptor _textPattern;
	}
}
