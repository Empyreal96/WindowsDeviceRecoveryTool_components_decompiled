using System;

namespace System.Windows
{
	// Token: 0x020000C6 RID: 198
	[Flags]
	internal enum InternalFlags2 : uint
	{
		// Token: 0x040006BB RID: 1723
		R0 = 1U,
		// Token: 0x040006BC RID: 1724
		R1 = 2U,
		// Token: 0x040006BD RID: 1725
		R2 = 4U,
		// Token: 0x040006BE RID: 1726
		R3 = 8U,
		// Token: 0x040006BF RID: 1727
		R4 = 16U,
		// Token: 0x040006C0 RID: 1728
		R5 = 32U,
		// Token: 0x040006C1 RID: 1729
		R6 = 64U,
		// Token: 0x040006C2 RID: 1730
		R7 = 128U,
		// Token: 0x040006C3 RID: 1731
		R8 = 256U,
		// Token: 0x040006C4 RID: 1732
		R9 = 512U,
		// Token: 0x040006C5 RID: 1733
		RA = 1024U,
		// Token: 0x040006C6 RID: 1734
		RB = 2048U,
		// Token: 0x040006C7 RID: 1735
		RC = 4096U,
		// Token: 0x040006C8 RID: 1736
		RD = 8192U,
		// Token: 0x040006C9 RID: 1737
		RE = 16384U,
		// Token: 0x040006CA RID: 1738
		RF = 32768U,
		// Token: 0x040006CB RID: 1739
		TreeHasLoadedChangeHandler = 1048576U,
		// Token: 0x040006CC RID: 1740
		IsLoadedCache = 2097152U,
		// Token: 0x040006CD RID: 1741
		IsStyleSetFromGenerator = 4194304U,
		// Token: 0x040006CE RID: 1742
		IsParentAnFE = 8388608U,
		// Token: 0x040006CF RID: 1743
		IsTemplatedParentAnFE = 16777216U,
		// Token: 0x040006D0 RID: 1744
		HasStyleChanged = 33554432U,
		// Token: 0x040006D1 RID: 1745
		HasTemplateChanged = 67108864U,
		// Token: 0x040006D2 RID: 1746
		HasStyleInvalidated = 134217728U,
		// Token: 0x040006D3 RID: 1747
		IsRequestingExpression = 268435456U,
		// Token: 0x040006D4 RID: 1748
		HasMultipleInheritanceContexts = 536870912U,
		// Token: 0x040006D5 RID: 1749
		BypassLayoutPolicies = 2147483648U,
		// Token: 0x040006D6 RID: 1750
		Default = 65535U
	}
}
