using System;

namespace System.Windows
{
	// Token: 0x020000D3 RID: 211
	internal interface IWindowService
	{
		// Token: 0x17000185 RID: 389
		// (get) Token: 0x0600074D RID: 1869
		// (set) Token: 0x0600074E RID: 1870
		string Title { get; set; }

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x0600074F RID: 1871
		// (set) Token: 0x06000750 RID: 1872
		double Height { get; set; }

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x06000751 RID: 1873
		// (set) Token: 0x06000752 RID: 1874
		double Width { get; set; }

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x06000753 RID: 1875
		bool UserResized { get; }
	}
}
