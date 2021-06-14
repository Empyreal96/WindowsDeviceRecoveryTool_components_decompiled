using System;
using System.Windows;

namespace MS.Internal
{
	// Token: 0x020005DB RID: 1499
	internal abstract class GlyphRunInfo
	{
		// Token: 0x170017E1 RID: 6113
		// (get) Token: 0x060063A1 RID: 25505
		internal abstract Point StartPosition { get; }

		// Token: 0x170017E2 RID: 6114
		// (get) Token: 0x060063A2 RID: 25506
		internal abstract Point EndPosition { get; }

		// Token: 0x170017E3 RID: 6115
		// (get) Token: 0x060063A3 RID: 25507
		internal abstract double WidthEmFontSize { get; }

		// Token: 0x170017E4 RID: 6116
		// (get) Token: 0x060063A4 RID: 25508
		internal abstract double HeightEmFontSize { get; }

		// Token: 0x170017E5 RID: 6117
		// (get) Token: 0x060063A5 RID: 25509
		internal abstract bool GlyphsHaveSidewaysOrientation { get; }

		// Token: 0x170017E6 RID: 6118
		// (get) Token: 0x060063A6 RID: 25510
		internal abstract int BidiLevel { get; }

		// Token: 0x170017E7 RID: 6119
		// (get) Token: 0x060063A7 RID: 25511
		internal abstract uint LanguageID { get; }

		// Token: 0x170017E8 RID: 6120
		// (get) Token: 0x060063A8 RID: 25512
		internal abstract string UnicodeString { get; }
	}
}
