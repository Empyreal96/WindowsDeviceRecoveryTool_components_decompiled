using System;

namespace System.Windows
{
	/// <summary>Indicates the current mode of lookup for both property value inheritance, a resource lookup, and a RelativeSource FindAncestor lookup.</summary>
	// Token: 0x020000D1 RID: 209
	public enum InheritanceBehavior
	{
		/// <summary>Property value inheritance lookup will query the current element and continue walking up the element tree to the page root.</summary>
		// Token: 0x0400072E RID: 1838
		Default,
		/// <summary>Property value inheritance lookup will not query the current element or any further.</summary>
		// Token: 0x0400072F RID: 1839
		SkipToAppNow,
		/// <summary>Property value inheritance lookup will query the current element but not any further.</summary>
		// Token: 0x04000730 RID: 1840
		SkipToAppNext,
		/// <summary>Property value inheritance lookup will not query the current element or any further.</summary>
		// Token: 0x04000731 RID: 1841
		SkipToThemeNow,
		/// <summary>Property value inheritance lookup will query the current element but not any further.</summary>
		// Token: 0x04000732 RID: 1842
		SkipToThemeNext,
		/// <summary>Property value inheritance lookup will not query the current element or any further.</summary>
		// Token: 0x04000733 RID: 1843
		SkipAllNow,
		/// <summary>Property value inheritance lookup will query the current element but not any further.</summary>
		// Token: 0x04000734 RID: 1844
		SkipAllNext
	}
}
