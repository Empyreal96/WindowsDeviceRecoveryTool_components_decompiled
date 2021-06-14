using System;

namespace Microsoft.WindowsDeviceRecoveryTool.Messages
{
	// Token: 0x02000066 RID: 102
	public class NotificationMessage
	{
		// Token: 0x0600030E RID: 782 RVA: 0x000101BC File Offset: 0x0000E3BC
		public NotificationMessage(bool showNotification, string header, string text)
		{
			this.ShowNotification = showNotification;
			this.Header = header;
			this.Text = text;
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x0600030F RID: 783 RVA: 0x000101E0 File Offset: 0x0000E3E0
		// (set) Token: 0x06000310 RID: 784 RVA: 0x000101F7 File Offset: 0x0000E3F7
		public bool ShowNotification { get; private set; }

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000311 RID: 785 RVA: 0x00010200 File Offset: 0x0000E400
		// (set) Token: 0x06000312 RID: 786 RVA: 0x00010217 File Offset: 0x0000E417
		public string Header { get; private set; }

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000313 RID: 787 RVA: 0x00010220 File Offset: 0x0000E420
		// (set) Token: 0x06000314 RID: 788 RVA: 0x00010237 File Offset: 0x0000E437
		public string Text { get; private set; }
	}
}
