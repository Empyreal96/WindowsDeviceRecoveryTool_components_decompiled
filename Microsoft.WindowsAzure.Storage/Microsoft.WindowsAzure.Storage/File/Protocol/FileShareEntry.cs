using System;
using System.Collections.Generic;

namespace Microsoft.WindowsAzure.Storage.File.Protocol
{
	// Token: 0x020000EB RID: 235
	public sealed class FileShareEntry
	{
		// Token: 0x060011FC RID: 4604 RVA: 0x00042A4D File Offset: 0x00040C4D
		internal FileShareEntry()
		{
		}

		// Token: 0x170002AC RID: 684
		// (get) Token: 0x060011FD RID: 4605 RVA: 0x00042A55 File Offset: 0x00040C55
		// (set) Token: 0x060011FE RID: 4606 RVA: 0x00042A5D File Offset: 0x00040C5D
		public IDictionary<string, string> Metadata { get; internal set; }

		// Token: 0x170002AD RID: 685
		// (get) Token: 0x060011FF RID: 4607 RVA: 0x00042A66 File Offset: 0x00040C66
		// (set) Token: 0x06001200 RID: 4608 RVA: 0x00042A6E File Offset: 0x00040C6E
		public FileShareProperties Properties { get; internal set; }

		// Token: 0x170002AE RID: 686
		// (get) Token: 0x06001201 RID: 4609 RVA: 0x00042A77 File Offset: 0x00040C77
		// (set) Token: 0x06001202 RID: 4610 RVA: 0x00042A7F File Offset: 0x00040C7F
		public string Name { get; internal set; }

		// Token: 0x170002AF RID: 687
		// (get) Token: 0x06001203 RID: 4611 RVA: 0x00042A88 File Offset: 0x00040C88
		// (set) Token: 0x06001204 RID: 4612 RVA: 0x00042A90 File Offset: 0x00040C90
		public Uri Uri { get; internal set; }
	}
}
