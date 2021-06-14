using System;
using Microsoft.Data.OData;

namespace System.Data.Services.Client
{
	// Token: 0x02000055 RID: 85
	public sealed class ReadingEntryArgs
	{
		// Token: 0x060002B6 RID: 694 RVA: 0x0000CE4A File Offset: 0x0000B04A
		public ReadingEntryArgs(ODataEntry entry)
		{
			this.Entry = entry;
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x060002B7 RID: 695 RVA: 0x0000CE59 File Offset: 0x0000B059
		// (set) Token: 0x060002B8 RID: 696 RVA: 0x0000CE61 File Offset: 0x0000B061
		public ODataEntry Entry { get; private set; }
	}
}
