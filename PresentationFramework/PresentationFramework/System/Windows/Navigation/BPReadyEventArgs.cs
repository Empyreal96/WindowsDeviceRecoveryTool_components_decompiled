using System;
using System.ComponentModel;

namespace System.Windows.Navigation
{
	// Token: 0x02000319 RID: 793
	internal class BPReadyEventArgs : CancelEventArgs
	{
		// Token: 0x060029F6 RID: 10742 RVA: 0x000C1AFF File Offset: 0x000BFCFF
		internal BPReadyEventArgs(object content, Uri uri)
		{
			this._content = content;
			this._uri = uri;
		}

		// Token: 0x17000A23 RID: 2595
		// (get) Token: 0x060029F7 RID: 10743 RVA: 0x000C1B15 File Offset: 0x000BFD15
		internal object Content
		{
			get
			{
				return this._content;
			}
		}

		// Token: 0x17000A24 RID: 2596
		// (get) Token: 0x060029F8 RID: 10744 RVA: 0x000C1B1D File Offset: 0x000BFD1D
		internal Uri Uri
		{
			get
			{
				return this._uri;
			}
		}

		// Token: 0x04001C1E RID: 7198
		private object _content;

		// Token: 0x04001C1F RID: 7199
		private Uri _uri;
	}
}
