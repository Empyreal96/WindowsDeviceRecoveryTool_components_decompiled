using System;
using System.Xml.Linq;

namespace System.Data.Services.Client
{
	// Token: 0x02000122 RID: 290
	internal sealed class WritingEntityInfo
	{
		// Token: 0x0600099B RID: 2459 RVA: 0x0002775C File Offset: 0x0002595C
		internal WritingEntityInfo(object entity, RequestInfo requestInfo)
		{
			this.Entity = entity;
			this.EntryPayload = new XDocument();
			this.RequestInfo = requestInfo;
		}

		// Token: 0x04000594 RID: 1428
		internal readonly object Entity;

		// Token: 0x04000595 RID: 1429
		internal readonly XDocument EntryPayload;

		// Token: 0x04000596 RID: 1430
		internal readonly RequestInfo RequestInfo;
	}
}
