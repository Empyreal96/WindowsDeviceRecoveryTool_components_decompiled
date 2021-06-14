using System;

namespace System.Windows
{
	// Token: 0x020000C5 RID: 197
	internal enum InternalFlags : uint
	{
		// Token: 0x0400069F RID: 1695
		HasResourceReferences = 1U,
		// Token: 0x040006A0 RID: 1696
		HasNumberSubstitutionChanged,
		// Token: 0x040006A1 RID: 1697
		HasImplicitStyleFromResources = 4U,
		// Token: 0x040006A2 RID: 1698
		InheritanceBehavior0 = 8U,
		// Token: 0x040006A3 RID: 1699
		InheritanceBehavior1 = 16U,
		// Token: 0x040006A4 RID: 1700
		InheritanceBehavior2 = 32U,
		// Token: 0x040006A5 RID: 1701
		IsStyleUpdateInProgress = 64U,
		// Token: 0x040006A6 RID: 1702
		IsThemeStyleUpdateInProgress = 128U,
		// Token: 0x040006A7 RID: 1703
		StoresParentTemplateValues = 256U,
		// Token: 0x040006A8 RID: 1704
		NeedsClipBounds = 1024U,
		// Token: 0x040006A9 RID: 1705
		HasWidthEverChanged = 2048U,
		// Token: 0x040006AA RID: 1706
		HasHeightEverChanged = 4096U,
		// Token: 0x040006AB RID: 1707
		IsInitialized = 32768U,
		// Token: 0x040006AC RID: 1708
		InitPending = 65536U,
		// Token: 0x040006AD RID: 1709
		IsResourceParentValid = 131072U,
		// Token: 0x040006AE RID: 1710
		AncestorChangeInProgress = 524288U,
		// Token: 0x040006AF RID: 1711
		InVisibilityCollapsedTree = 1048576U,
		// Token: 0x040006B0 RID: 1712
		HasStyleEverBeenFetched = 2097152U,
		// Token: 0x040006B1 RID: 1713
		HasThemeStyleEverBeenFetched = 4194304U,
		// Token: 0x040006B2 RID: 1714
		HasLocalStyle = 8388608U,
		// Token: 0x040006B3 RID: 1715
		HasTemplateGeneratedSubTree = 16777216U,
		// Token: 0x040006B4 RID: 1716
		HasLogicalChildren = 67108864U,
		// Token: 0x040006B5 RID: 1717
		IsLogicalChildrenIterationInProgress = 134217728U,
		// Token: 0x040006B6 RID: 1718
		CreatingRoot = 268435456U,
		// Token: 0x040006B7 RID: 1719
		IsRightToLeft = 536870912U,
		// Token: 0x040006B8 RID: 1720
		ShouldLookupImplicitStyles = 1073741824U,
		// Token: 0x040006B9 RID: 1721
		PotentiallyHasMentees = 2147483648U
	}
}
