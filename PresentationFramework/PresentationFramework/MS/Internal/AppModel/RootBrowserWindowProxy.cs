using System;

namespace MS.Internal.AppModel
{
	// Token: 0x0200079B RID: 1947
	internal class RootBrowserWindowProxy : MarshalByRefObject
	{
		// Token: 0x06007A1A RID: 31258 RVA: 0x002297DB File Offset: 0x002279DB
		internal RootBrowserWindowProxy(RootBrowserWindow rbw)
		{
			this._rbw = rbw;
		}

		// Token: 0x17001CC6 RID: 7366
		// (get) Token: 0x06007A1B RID: 31259 RVA: 0x002297EA File Offset: 0x002279EA
		internal RootBrowserWindow RootBrowserWindow
		{
			get
			{
				return this._rbw;
			}
		}

		// Token: 0x06007A1C RID: 31260 RVA: 0x002297F2 File Offset: 0x002279F2
		internal void TabInto(bool forward)
		{
			this._rbw.TabInto(forward);
		}

		// Token: 0x040039B6 RID: 14774
		private RootBrowserWindow _rbw;
	}
}
