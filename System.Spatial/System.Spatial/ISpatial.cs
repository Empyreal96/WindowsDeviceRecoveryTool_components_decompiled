using System;

namespace System.Spatial
{
	// Token: 0x02000014 RID: 20
	public interface ISpatial
	{
		// Token: 0x1700000C RID: 12
		// (get) Token: 0x060000D3 RID: 211
		CoordinateSystem CoordinateSystem { get; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x060000D4 RID: 212
		bool IsEmpty { get; }
	}
}
