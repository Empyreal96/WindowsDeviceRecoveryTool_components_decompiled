using System;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000646 RID: 1606
	internal interface ISegment
	{
		// Token: 0x06006A57 RID: 27223
		void GetFirstPara(out int successful, out IntPtr firstParaName);

		// Token: 0x06006A58 RID: 27224
		void GetNextPara(BaseParagraph currentParagraph, out int found, out IntPtr nextParaName);

		// Token: 0x06006A59 RID: 27225
		void UpdGetFirstChangeInSegment(out int fFound, out int fChangeFirst, out IntPtr nmpBeforeChange);
	}
}
