using System;

namespace Standard
{
	// Token: 0x0200004A RID: 74
	internal struct TITLEBARINFO
	{
		// Token: 0x0400042B RID: 1067
		public int cbSize;

		// Token: 0x0400042C RID: 1068
		public RECT rcTitleBar;

		// Token: 0x0400042D RID: 1069
		public STATE_SYSTEM rgstate_TitleBar;

		// Token: 0x0400042E RID: 1070
		public STATE_SYSTEM rgstate_Reserved;

		// Token: 0x0400042F RID: 1071
		public STATE_SYSTEM rgstate_MinimizeButton;

		// Token: 0x04000430 RID: 1072
		public STATE_SYSTEM rgstate_MaximizeButton;

		// Token: 0x04000431 RID: 1073
		public STATE_SYSTEM rgstate_HelpButton;

		// Token: 0x04000432 RID: 1074
		public STATE_SYSTEM rgstate_CloseButton;
	}
}
