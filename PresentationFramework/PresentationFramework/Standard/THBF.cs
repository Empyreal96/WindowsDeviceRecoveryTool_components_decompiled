using System;

namespace Standard
{
	// Token: 0x0200006E RID: 110
	[Flags]
	internal enum THBF : uint
	{
		// Token: 0x04000506 RID: 1286
		ENABLED = 0U,
		// Token: 0x04000507 RID: 1287
		DISABLED = 1U,
		// Token: 0x04000508 RID: 1288
		DISMISSONCLICK = 2U,
		// Token: 0x04000509 RID: 1289
		NOBACKGROUND = 4U,
		// Token: 0x0400050A RID: 1290
		HIDDEN = 8U,
		// Token: 0x0400050B RID: 1291
		NONINTERACTIVE = 16U
	}
}
