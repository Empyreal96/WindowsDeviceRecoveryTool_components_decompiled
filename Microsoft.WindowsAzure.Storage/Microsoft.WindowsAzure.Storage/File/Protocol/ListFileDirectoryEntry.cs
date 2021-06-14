using System;

namespace Microsoft.WindowsAzure.Storage.File.Protocol
{
	// Token: 0x020000ED RID: 237
	public sealed class ListFileDirectoryEntry : IListFileEntry
	{
		// Token: 0x06001206 RID: 4614 RVA: 0x00042A99 File Offset: 0x00040C99
		internal ListFileDirectoryEntry(string name, Uri uri, FileDirectoryProperties properties)
		{
			this.Name = name;
			this.Uri = uri;
			this.Properties = properties;
		}

		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x06001207 RID: 4615 RVA: 0x00042AB6 File Offset: 0x00040CB6
		// (set) Token: 0x06001208 RID: 4616 RVA: 0x00042ABE File Offset: 0x00040CBE
		public string Name { get; internal set; }

		// Token: 0x170002B2 RID: 690
		// (get) Token: 0x06001209 RID: 4617 RVA: 0x00042AC7 File Offset: 0x00040CC7
		// (set) Token: 0x0600120A RID: 4618 RVA: 0x00042ACF File Offset: 0x00040CCF
		public Uri Uri { get; internal set; }

		// Token: 0x170002B3 RID: 691
		// (get) Token: 0x0600120B RID: 4619 RVA: 0x00042AD8 File Offset: 0x00040CD8
		// (set) Token: 0x0600120C RID: 4620 RVA: 0x00042AE0 File Offset: 0x00040CE0
		public FileDirectoryProperties Properties { get; internal set; }
	}
}
