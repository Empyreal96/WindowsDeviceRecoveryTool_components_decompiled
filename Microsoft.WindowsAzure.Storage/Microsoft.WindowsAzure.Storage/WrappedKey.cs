using System;

namespace Microsoft.WindowsAzure.Storage
{
	// Token: 0x02000082 RID: 130
	internal class WrappedKey
	{
		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x06000F15 RID: 3861 RVA: 0x00039C13 File Offset: 0x00037E13
		// (set) Token: 0x06000F16 RID: 3862 RVA: 0x00039C1B File Offset: 0x00037E1B
		public string KeyId { get; set; }

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x06000F17 RID: 3863 RVA: 0x00039C24 File Offset: 0x00037E24
		// (set) Token: 0x06000F18 RID: 3864 RVA: 0x00039C2C File Offset: 0x00037E2C
		public byte[] EncryptedKey { get; set; }

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x06000F19 RID: 3865 RVA: 0x00039C35 File Offset: 0x00037E35
		// (set) Token: 0x06000F1A RID: 3866 RVA: 0x00039C3D File Offset: 0x00037E3D
		public string Algorithm { get; set; }

		// Token: 0x06000F1B RID: 3867 RVA: 0x00039C46 File Offset: 0x00037E46
		public WrappedKey(string keyId, byte[] encryptedKey, string algorithm)
		{
			this.KeyId = keyId;
			this.EncryptedKey = encryptedKey;
			this.Algorithm = algorithm;
		}
	}
}
