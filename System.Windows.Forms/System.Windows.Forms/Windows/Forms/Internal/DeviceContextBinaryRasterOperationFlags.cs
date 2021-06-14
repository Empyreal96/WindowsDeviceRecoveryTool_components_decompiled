using System;

namespace System.Windows.Forms.Internal
{
	// Token: 0x020004EA RID: 1258
	[Flags]
	internal enum DeviceContextBinaryRasterOperationFlags
	{
		// Token: 0x04003531 RID: 13617
		Black = 1,
		// Token: 0x04003532 RID: 13618
		NotMergePen = 2,
		// Token: 0x04003533 RID: 13619
		MaskNotPen = 3,
		// Token: 0x04003534 RID: 13620
		NotCopyPen = 4,
		// Token: 0x04003535 RID: 13621
		MaskPenNot = 5,
		// Token: 0x04003536 RID: 13622
		Not = 6,
		// Token: 0x04003537 RID: 13623
		XorPen = 7,
		// Token: 0x04003538 RID: 13624
		NotMaskPen = 8,
		// Token: 0x04003539 RID: 13625
		MaskPen = 9,
		// Token: 0x0400353A RID: 13626
		NotXorPen = 10,
		// Token: 0x0400353B RID: 13627
		Nop = 11,
		// Token: 0x0400353C RID: 13628
		MergeNotPen = 12,
		// Token: 0x0400353D RID: 13629
		CopyPen = 13,
		// Token: 0x0400353E RID: 13630
		MergePenNot = 14,
		// Token: 0x0400353F RID: 13631
		MergePen = 15,
		// Token: 0x04003540 RID: 13632
		White = 16
	}
}
