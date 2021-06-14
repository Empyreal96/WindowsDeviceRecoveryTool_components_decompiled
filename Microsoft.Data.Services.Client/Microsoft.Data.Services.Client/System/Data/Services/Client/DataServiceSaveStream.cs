using System;
using System.IO;

namespace System.Data.Services.Client
{
	// Token: 0x0200005D RID: 93
	internal class DataServiceSaveStream
	{
		// Token: 0x0600030F RID: 783 RVA: 0x0000DEFE File Offset: 0x0000C0FE
		internal DataServiceSaveStream(Stream stream, bool close, DataServiceRequestArgs args)
		{
			this.stream = stream;
			this.close = close;
			this.args = args;
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000310 RID: 784 RVA: 0x0000DF1B File Offset: 0x0000C11B
		internal Stream Stream
		{
			get
			{
				return this.stream;
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000311 RID: 785 RVA: 0x0000DF23 File Offset: 0x0000C123
		// (set) Token: 0x06000312 RID: 786 RVA: 0x0000DF2B File Offset: 0x0000C12B
		internal DataServiceRequestArgs Args
		{
			get
			{
				return this.args;
			}
			set
			{
				this.args = value;
			}
		}

		// Token: 0x06000313 RID: 787 RVA: 0x0000DF34 File Offset: 0x0000C134
		internal void Close()
		{
			if (this.stream != null && this.close)
			{
				this.stream.Close();
			}
		}

		// Token: 0x0400027E RID: 638
		private DataServiceRequestArgs args;

		// Token: 0x0400027F RID: 639
		private Stream stream;

		// Token: 0x04000280 RID: 640
		private bool close;
	}
}
