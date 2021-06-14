using System;
using System.Windows;

namespace MS.Internal.Globalization
{
	// Token: 0x0200069F RID: 1695
	internal interface ILocalizabilityInheritable
	{
		// Token: 0x17001A2A RID: 6698
		// (get) Token: 0x06006E3F RID: 28223
		ILocalizabilityInheritable LocalizabilityAncestor { get; }

		// Token: 0x17001A2B RID: 6699
		// (get) Token: 0x06006E40 RID: 28224
		// (set) Token: 0x06006E41 RID: 28225
		LocalizabilityAttribute InheritableAttribute { get; set; }

		// Token: 0x17001A2C RID: 6700
		// (get) Token: 0x06006E42 RID: 28226
		// (set) Token: 0x06006E43 RID: 28227
		bool IsIgnored { get; set; }
	}
}
