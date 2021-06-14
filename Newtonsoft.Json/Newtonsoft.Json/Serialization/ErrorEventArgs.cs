using System;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x020000AC RID: 172
	public class ErrorEventArgs : EventArgs
	{
		// Token: 0x170001BE RID: 446
		// (get) Token: 0x0600086D RID: 2157 RVA: 0x0002080F File Offset: 0x0001EA0F
		// (set) Token: 0x0600086E RID: 2158 RVA: 0x00020817 File Offset: 0x0001EA17
		public object CurrentObject { get; private set; }

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x0600086F RID: 2159 RVA: 0x00020820 File Offset: 0x0001EA20
		// (set) Token: 0x06000870 RID: 2160 RVA: 0x00020828 File Offset: 0x0001EA28
		public ErrorContext ErrorContext { get; private set; }

		// Token: 0x06000871 RID: 2161 RVA: 0x00020831 File Offset: 0x0001EA31
		public ErrorEventArgs(object currentObject, ErrorContext errorContext)
		{
			this.CurrentObject = currentObject;
			this.ErrorContext = errorContext;
		}
	}
}
