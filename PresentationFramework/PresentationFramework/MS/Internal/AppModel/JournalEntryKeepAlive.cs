using System;
using System.Windows.Navigation;

namespace MS.Internal.AppModel
{
	// Token: 0x02000789 RID: 1929
	internal class JournalEntryKeepAlive : JournalEntry
	{
		// Token: 0x06007957 RID: 31063 RVA: 0x002271BE File Offset: 0x002253BE
		internal JournalEntryKeepAlive(JournalEntryGroupState jeGroupState, Uri uri, object keepAliveRoot) : base(jeGroupState, uri)
		{
			Invariant.Assert(keepAliveRoot != null);
			this._keepAliveRoot = keepAliveRoot;
		}

		// Token: 0x17001CAA RID: 7338
		// (get) Token: 0x06007958 RID: 31064 RVA: 0x002271D8 File Offset: 0x002253D8
		internal object KeepAliveRoot
		{
			get
			{
				return this._keepAliveRoot;
			}
		}

		// Token: 0x06007959 RID: 31065 RVA: 0x002271E0 File Offset: 0x002253E0
		internal override bool IsAlive()
		{
			return this.KeepAliveRoot != null;
		}

		// Token: 0x0600795A RID: 31066 RVA: 0x002271EB File Offset: 0x002253EB
		internal override void SaveState(object contentObject)
		{
			this._keepAliveRoot = contentObject;
		}

		// Token: 0x0600795B RID: 31067 RVA: 0x002271F4 File Offset: 0x002253F4
		internal override bool Navigate(INavigator navigator, NavigationMode navMode)
		{
			return navigator.Navigate(this.KeepAliveRoot, new NavigateInfo(base.Source, navMode, this));
		}

		// Token: 0x0400397F RID: 14719
		private object _keepAliveRoot;
	}
}
