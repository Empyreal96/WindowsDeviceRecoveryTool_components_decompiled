using System;
using System.Collections.Generic;

namespace Microsoft.WindowsAzure.Storage.File
{
	// Token: 0x020000DB RID: 219
	public sealed class FileResultSegment
	{
		// Token: 0x060011B5 RID: 4533 RVA: 0x00042334 File Offset: 0x00040534
		internal FileResultSegment(IEnumerable<IListFileItem> files, FileContinuationToken continuationToken)
		{
			this.Results = files;
			this.ContinuationToken = continuationToken;
		}

		// Token: 0x17000296 RID: 662
		// (get) Token: 0x060011B6 RID: 4534 RVA: 0x0004234A File Offset: 0x0004054A
		// (set) Token: 0x060011B7 RID: 4535 RVA: 0x00042352 File Offset: 0x00040552
		public IEnumerable<IListFileItem> Results { get; private set; }

		// Token: 0x17000297 RID: 663
		// (get) Token: 0x060011B8 RID: 4536 RVA: 0x0004235B File Offset: 0x0004055B
		// (set) Token: 0x060011B9 RID: 4537 RVA: 0x00042363 File Offset: 0x00040563
		public FileContinuationToken ContinuationToken { get; private set; }
	}
}
