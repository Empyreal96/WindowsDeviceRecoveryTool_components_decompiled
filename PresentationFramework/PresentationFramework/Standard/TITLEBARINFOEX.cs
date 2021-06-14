using System;

namespace Standard
{
	// Token: 0x0200004B RID: 75
	internal struct TITLEBARINFOEX
	{
		// Token: 0x04000433 RID: 1075
		public int cbSize;

		// Token: 0x04000434 RID: 1076
		public RECT rcTitleBar;

		// Token: 0x04000435 RID: 1077
		public STATE_SYSTEM rgstate_TitleBar;

		// Token: 0x04000436 RID: 1078
		public STATE_SYSTEM rgstate_Reserved;

		// Token: 0x04000437 RID: 1079
		public STATE_SYSTEM rgstate_MinimizeButton;

		// Token: 0x04000438 RID: 1080
		public STATE_SYSTEM rgstate_MaximizeButton;

		// Token: 0x04000439 RID: 1081
		public STATE_SYSTEM rgstate_HelpButton;

		// Token: 0x0400043A RID: 1082
		public STATE_SYSTEM rgstate_CloseButton;

		// Token: 0x0400043B RID: 1083
		public RECT rgrect_TitleBar;

		// Token: 0x0400043C RID: 1084
		public RECT rgrect_Reserved;

		// Token: 0x0400043D RID: 1085
		public RECT rgrect_MinimizeButton;

		// Token: 0x0400043E RID: 1086
		public RECT rgrect_MaximizeButton;

		// Token: 0x0400043F RID: 1087
		public RECT rgrect_HelpButton;

		// Token: 0x04000440 RID: 1088
		public RECT rgrect_CloseButton;
	}
}
