using System;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;

namespace Microsoft.WindowsAzure.Storage.File.Protocol
{
	// Token: 0x020000E7 RID: 231
	public sealed class FileListingContext : ListingContext
	{
		// Token: 0x060011F3 RID: 4595 RVA: 0x0004291D File Offset: 0x00040B1D
		public FileListingContext(int? maxResults) : base(null, maxResults)
		{
		}
	}
}
