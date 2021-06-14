using System;
using System.Collections.Generic;

namespace Microsoft.WindowsAzure.Storage.Queue.Protocol
{
	// Token: 0x020000FE RID: 254
	public sealed class QueueEntry
	{
		// Token: 0x0600127D RID: 4733 RVA: 0x00044E1B File Offset: 0x0004301B
		internal QueueEntry()
		{
		}

		// Token: 0x0600127E RID: 4734 RVA: 0x00044E23 File Offset: 0x00043023
		internal QueueEntry(string name, Uri uri, IDictionary<string, string> metadata)
		{
			this.Name = name;
			this.Uri = uri;
			this.Metadata = metadata;
		}

		// Token: 0x170002E2 RID: 738
		// (get) Token: 0x0600127F RID: 4735 RVA: 0x00044E40 File Offset: 0x00043040
		// (set) Token: 0x06001280 RID: 4736 RVA: 0x00044E48 File Offset: 0x00043048
		public IDictionary<string, string> Metadata { get; internal set; }

		// Token: 0x170002E3 RID: 739
		// (get) Token: 0x06001281 RID: 4737 RVA: 0x00044E51 File Offset: 0x00043051
		// (set) Token: 0x06001282 RID: 4738 RVA: 0x00044E59 File Offset: 0x00043059
		public string Name { get; internal set; }

		// Token: 0x170002E4 RID: 740
		// (get) Token: 0x06001283 RID: 4739 RVA: 0x00044E62 File Offset: 0x00043062
		// (set) Token: 0x06001284 RID: 4740 RVA: 0x00044E6A File Offset: 0x0004306A
		public Uri Uri { get; internal set; }
	}
}
