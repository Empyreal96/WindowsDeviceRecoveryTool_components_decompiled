using System;
using System.Security.Cryptography;

namespace Microsoft.WindowsAzure.Storage.Core.Util
{
	// Token: 0x02000099 RID: 153
	internal class MD5Wrapper : IDisposable
	{
		// Token: 0x06000FFF RID: 4095 RVA: 0x0003CD39 File Offset: 0x0003AF39
		internal MD5Wrapper()
		{
			this.hash = (this.version1MD5 ? MD5.Create() : new NativeMD5());
		}

		// Token: 0x06001000 RID: 4096 RVA: 0x0003CD66 File Offset: 0x0003AF66
		internal void UpdateHash(byte[] input, int offset, int count)
		{
			if (count > 0)
			{
				this.hash.TransformBlock(input, offset, count, null, 0);
			}
		}

		// Token: 0x06001001 RID: 4097 RVA: 0x0003CD7D File Offset: 0x0003AF7D
		internal string ComputeHash()
		{
			this.hash.TransformFinalBlock(new byte[0], 0, 0);
			return Convert.ToBase64String(this.hash.Hash);
		}

		// Token: 0x06001002 RID: 4098 RVA: 0x0003CDA3 File Offset: 0x0003AFA3
		public void Dispose()
		{
			if (this.hash != null)
			{
				((IDisposable)this.hash).Dispose();
				this.hash = null;
			}
		}

		// Token: 0x040003B7 RID: 951
		private readonly bool version1MD5 = CloudStorageAccount.UseV1MD5;

		// Token: 0x040003B8 RID: 952
		private MD5 hash;
	}
}
