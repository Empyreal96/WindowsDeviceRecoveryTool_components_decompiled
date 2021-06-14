using System;
using System.IO;

namespace System.Data.Services.Client
{
	// Token: 0x02000033 RID: 51
	internal sealed class ContentStream
	{
		// Token: 0x06000185 RID: 389 RVA: 0x00008EBE File Offset: 0x000070BE
		public ContentStream(Stream stream, bool isKnownMemoryStream)
		{
			this.stream = stream;
			this.isKnownMemoryStream = isKnownMemoryStream;
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000186 RID: 390 RVA: 0x00008ED4 File Offset: 0x000070D4
		public Stream Stream
		{
			get
			{
				return this.stream;
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000187 RID: 391 RVA: 0x00008EDC File Offset: 0x000070DC
		public bool IsKnownMemoryStream
		{
			get
			{
				return this.isKnownMemoryStream;
			}
		}

		// Token: 0x040001F6 RID: 502
		private readonly Stream stream;

		// Token: 0x040001F7 RID: 503
		private readonly bool isKnownMemoryStream;
	}
}
