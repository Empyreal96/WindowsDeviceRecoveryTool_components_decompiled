using System;
using System.Collections.Generic;

namespace Microsoft.WindowsAzure.Storage.File.Protocol
{
	// Token: 0x020000EE RID: 238
	public sealed class ListFileEntry : IListFileEntry
	{
		// Token: 0x0600120D RID: 4621 RVA: 0x00042AE9 File Offset: 0x00040CE9
		internal ListFileEntry(string name, CloudFileAttributes attributes)
		{
			this.Name = name;
			this.Attributes = attributes;
		}

		// Token: 0x170002B4 RID: 692
		// (get) Token: 0x0600120E RID: 4622 RVA: 0x00042AFF File Offset: 0x00040CFF
		// (set) Token: 0x0600120F RID: 4623 RVA: 0x00042B07 File Offset: 0x00040D07
		internal CloudFileAttributes Attributes { get; private set; }

		// Token: 0x170002B5 RID: 693
		// (get) Token: 0x06001210 RID: 4624 RVA: 0x00042B10 File Offset: 0x00040D10
		// (set) Token: 0x06001211 RID: 4625 RVA: 0x00042B18 File Offset: 0x00040D18
		public string Name { get; private set; }

		// Token: 0x170002B6 RID: 694
		// (get) Token: 0x06001212 RID: 4626 RVA: 0x00042B21 File Offset: 0x00040D21
		public FileProperties Properties
		{
			get
			{
				return this.Attributes.Properties;
			}
		}

		// Token: 0x170002B7 RID: 695
		// (get) Token: 0x06001213 RID: 4627 RVA: 0x00042B2E File Offset: 0x00040D2E
		public IDictionary<string, string> Metadata
		{
			get
			{
				return this.Attributes.Metadata;
			}
		}

		// Token: 0x170002B8 RID: 696
		// (get) Token: 0x06001214 RID: 4628 RVA: 0x00042B3B File Offset: 0x00040D3B
		public Uri Uri
		{
			get
			{
				return this.Attributes.Uri;
			}
		}
	}
}
