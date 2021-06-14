using System;

namespace System.Windows
{
	// Token: 0x020000EF RID: 239
	internal class ResourcesChangedEventArgs : EventArgs
	{
		// Token: 0x06000884 RID: 2180 RVA: 0x0001BCC9 File Offset: 0x00019EC9
		internal ResourcesChangedEventArgs(ResourcesChangeInfo info)
		{
			this._info = info;
		}

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x06000885 RID: 2181 RVA: 0x0001BCD8 File Offset: 0x00019ED8
		internal ResourcesChangeInfo Info
		{
			get
			{
				return this._info;
			}
		}

		// Token: 0x040007A6 RID: 1958
		private ResourcesChangeInfo _info;
	}
}
