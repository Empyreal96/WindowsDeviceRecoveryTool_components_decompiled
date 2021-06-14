using System;
using System.Threading;

namespace Microsoft.WindowsAzure.Storage.Core.Util
{
	// Token: 0x0200009C RID: 156
	internal class StreamDescriptor
	{
		// Token: 0x170001FC RID: 508
		// (get) Token: 0x06001027 RID: 4135 RVA: 0x0003D8A0 File Offset: 0x0003BAA0
		// (set) Token: 0x06001028 RID: 4136 RVA: 0x0003D8AD File Offset: 0x0003BAAD
		public long Length
		{
			get
			{
				return Interlocked.Read(ref this.length);
			}
			set
			{
				Interlocked.Exchange(ref this.length, value);
			}
		}

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x06001029 RID: 4137 RVA: 0x0003D8BC File Offset: 0x0003BABC
		// (set) Token: 0x0600102A RID: 4138 RVA: 0x0003D8C6 File Offset: 0x0003BAC6
		public string Md5
		{
			get
			{
				return this.md5;
			}
			set
			{
				this.md5 = value;
			}
		}

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x0600102B RID: 4139 RVA: 0x0003D8D1 File Offset: 0x0003BAD1
		// (set) Token: 0x0600102C RID: 4140 RVA: 0x0003D8DB File Offset: 0x0003BADB
		public MD5Wrapper Md5HashRef
		{
			get
			{
				return this.md5HashRef;
			}
			set
			{
				this.md5HashRef = value;
			}
		}

		// Token: 0x040003BF RID: 959
		private long length;

		// Token: 0x040003C0 RID: 960
		private volatile string md5;

		// Token: 0x040003C1 RID: 961
		private volatile MD5Wrapper md5HashRef;
	}
}
