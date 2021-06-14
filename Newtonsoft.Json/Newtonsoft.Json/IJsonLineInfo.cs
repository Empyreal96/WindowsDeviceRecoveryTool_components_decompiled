using System;

namespace Newtonsoft.Json
{
	// Token: 0x02000043 RID: 67
	public interface IJsonLineInfo
	{
		// Token: 0x06000255 RID: 597
		bool HasLineInfo();

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000256 RID: 598
		int LineNumber { get; }

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000257 RID: 599
		int LinePosition { get; }
	}
}
