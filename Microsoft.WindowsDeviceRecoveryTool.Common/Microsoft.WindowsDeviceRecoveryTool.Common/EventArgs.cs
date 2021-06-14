using System;

namespace Microsoft.WindowsDeviceRecoveryTool.Common
{
	// Token: 0x02000005 RID: 5
	public class EventArgs<T> : EventArgs
	{
		// Token: 0x0600000B RID: 11 RVA: 0x000022A7 File Offset: 0x000004A7
		public EventArgs()
		{
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000022B2 File Offset: 0x000004B2
		public EventArgs(T value) : this()
		{
			this.Value = value;
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600000D RID: 13 RVA: 0x000022C8 File Offset: 0x000004C8
		// (set) Token: 0x0600000E RID: 14 RVA: 0x000022DF File Offset: 0x000004DF
		public T Value { get; set; }
	}
}
