using System;

namespace ComponentAce.Compression.Interfaces
{
	// Token: 0x02000048 RID: 72
	internal interface IInternalSearchRec
	{
		// Token: 0x1700006C RID: 108
		// (get) Token: 0x060002FB RID: 763
		// (set) Token: 0x060002FC RID: 764
		int ItemNo { get; set; }

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x060002FD RID: 765
		// (set) Token: 0x060002FE RID: 766
		string CFindMask { get; set; }

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x060002FF RID: 767
		// (set) Token: 0x06000300 RID: 768
		bool FWildCards { get; set; }

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000301 RID: 769
		// (set) Token: 0x06000302 RID: 770
		int FFindAttr { get; set; }

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000303 RID: 771
		// (set) Token: 0x06000304 RID: 772
		string ExclusionMask { get; set; }

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000305 RID: 773
		// (set) Token: 0x06000306 RID: 774
		bool UseProperties { get; set; }
	}
}
