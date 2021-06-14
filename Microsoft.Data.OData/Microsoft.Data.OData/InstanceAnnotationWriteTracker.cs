using System;
using System.Collections.Generic;

namespace Microsoft.Data.OData
{
	// Token: 0x0200013A RID: 314
	internal sealed class InstanceAnnotationWriteTracker
	{
		// Token: 0x0600086B RID: 2155 RVA: 0x0001B720 File Offset: 0x00019920
		public InstanceAnnotationWriteTracker()
		{
			this.writeStatus = new HashSet<string>(StringComparer.Ordinal);
		}

		// Token: 0x0600086C RID: 2156 RVA: 0x0001B738 File Offset: 0x00019938
		public bool IsAnnotationWritten(string key)
		{
			return this.writeStatus.Contains(key);
		}

		// Token: 0x0600086D RID: 2157 RVA: 0x0001B746 File Offset: 0x00019946
		public bool MarkAnnotationWritten(string key)
		{
			return this.writeStatus.Add(key);
		}

		// Token: 0x04000331 RID: 817
		private readonly HashSet<string> writeStatus;
	}
}
