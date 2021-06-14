using System;

namespace Microsoft.WindowsDeviceRecoveryTool.Messages
{
	// Token: 0x0200006A RID: 106
	public class ThemeColorChangedMessage
	{
		// Token: 0x0600031E RID: 798 RVA: 0x000102DC File Offset: 0x0000E4DC
		public ThemeColorChangedMessage(string theme, string color)
		{
			this.Theme = theme;
			this.Color = color;
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x0600031F RID: 799 RVA: 0x000102F8 File Offset: 0x0000E4F8
		// (set) Token: 0x06000320 RID: 800 RVA: 0x0001030F File Offset: 0x0000E50F
		public string Theme { get; private set; }

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000321 RID: 801 RVA: 0x00010318 File Offset: 0x0000E518
		// (set) Token: 0x06000322 RID: 802 RVA: 0x0001032F File Offset: 0x0000E52F
		public string Color { get; private set; }
	}
}
