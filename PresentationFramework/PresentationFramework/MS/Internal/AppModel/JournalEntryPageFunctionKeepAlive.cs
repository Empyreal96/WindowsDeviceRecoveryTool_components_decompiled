using System;
using System.Windows.Navigation;

namespace MS.Internal.AppModel
{
	// Token: 0x0200078B RID: 1931
	internal class JournalEntryPageFunctionKeepAlive : JournalEntryPageFunction
	{
		// Token: 0x06007967 RID: 31079 RVA: 0x00227356 File Offset: 0x00225556
		internal JournalEntryPageFunctionKeepAlive(JournalEntryGroupState jeGroupState, PageFunctionBase pageFunction) : base(jeGroupState, pageFunction)
		{
			this._keepAlivePageFunction = pageFunction;
		}

		// Token: 0x06007968 RID: 31080 RVA: 0x00016748 File Offset: 0x00014948
		internal override bool IsPageFunction()
		{
			return true;
		}

		// Token: 0x06007969 RID: 31081 RVA: 0x00227367 File Offset: 0x00225567
		internal override bool IsAlive()
		{
			return this.KeepAlivePageFunction != null;
		}

		// Token: 0x17001CAD RID: 7341
		// (get) Token: 0x0600796A RID: 31082 RVA: 0x00227372 File Offset: 0x00225572
		internal PageFunctionBase KeepAlivePageFunction
		{
			get
			{
				return this._keepAlivePageFunction;
			}
		}

		// Token: 0x0600796B RID: 31083 RVA: 0x0022737C File Offset: 0x0022557C
		internal override PageFunctionBase ResumePageFunction()
		{
			PageFunctionBase keepAlivePageFunction = this.KeepAlivePageFunction;
			keepAlivePageFunction._Resume = true;
			return keepAlivePageFunction;
		}

		// Token: 0x0600796C RID: 31084 RVA: 0x00227398 File Offset: 0x00225598
		internal override void SaveState(object contentObject)
		{
			Invariant.Assert(this._keepAlivePageFunction == contentObject);
		}

		// Token: 0x0600796D RID: 31085 RVA: 0x002273A8 File Offset: 0x002255A8
		internal override bool Navigate(INavigator navigator, NavigationMode navMode)
		{
			PageFunctionBase content = (navigator.Content == this._keepAlivePageFunction) ? this._keepAlivePageFunction : this.ResumePageFunction();
			return navigator.Navigate(content, new NavigateInfo(base.Source, navMode, this));
		}

		// Token: 0x04003983 RID: 14723
		private PageFunctionBase _keepAlivePageFunction;
	}
}
