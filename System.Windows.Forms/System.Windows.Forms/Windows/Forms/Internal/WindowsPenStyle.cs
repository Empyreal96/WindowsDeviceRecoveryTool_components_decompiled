using System;

namespace System.Windows.Forms.Internal
{
	// Token: 0x020004FD RID: 1277
	[Flags]
	internal enum WindowsPenStyle
	{
		// Token: 0x04003618 RID: 13848
		Solid = 0,
		// Token: 0x04003619 RID: 13849
		Dash = 1,
		// Token: 0x0400361A RID: 13850
		Dot = 2,
		// Token: 0x0400361B RID: 13851
		DashDot = 3,
		// Token: 0x0400361C RID: 13852
		DashDotDot = 4,
		// Token: 0x0400361D RID: 13853
		Null = 5,
		// Token: 0x0400361E RID: 13854
		InsideFrame = 6,
		// Token: 0x0400361F RID: 13855
		UserStyle = 7,
		// Token: 0x04003620 RID: 13856
		Alternate = 8,
		// Token: 0x04003621 RID: 13857
		EndcapRound = 0,
		// Token: 0x04003622 RID: 13858
		EndcapSquare = 256,
		// Token: 0x04003623 RID: 13859
		EndcapFlat = 512,
		// Token: 0x04003624 RID: 13860
		JoinRound = 0,
		// Token: 0x04003625 RID: 13861
		JoinBevel = 4096,
		// Token: 0x04003626 RID: 13862
		JoinMiter = 8192,
		// Token: 0x04003627 RID: 13863
		Cosmetic = 0,
		// Token: 0x04003628 RID: 13864
		Geometric = 65536,
		// Token: 0x04003629 RID: 13865
		Default = 0
	}
}
