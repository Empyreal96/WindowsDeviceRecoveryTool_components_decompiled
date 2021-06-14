using System;

namespace Microsoft.WindowsAzure.Storage.Auth
{
	// Token: 0x02000083 RID: 131
	internal struct StorageAccountKey
	{
		// Token: 0x06000F1C RID: 3868 RVA: 0x00039C63 File Offset: 0x00037E63
		public StorageAccountKey(string keyName, byte[] keyValue)
		{
			this.KeyName = keyName;
			this.KeyValue = keyValue;
		}

		// Token: 0x0400027A RID: 634
		internal string KeyName;

		// Token: 0x0400027B RID: 635
		internal byte[] KeyValue;
	}
}
