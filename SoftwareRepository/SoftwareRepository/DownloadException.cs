using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace SoftwareRepository
{
	// Token: 0x02000007 RID: 7
	[Serializable]
	public class DownloadException : Exception
	{
		// Token: 0x0600000A RID: 10 RVA: 0x000021AC File Offset: 0x000003AC
		public DownloadException()
		{
			this.StatusCode = 0;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000021BB File Offset: 0x000003BB
		public DownloadException(int statusCode)
		{
			this.StatusCode = statusCode;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000021CA File Offset: 0x000003CA
		public DownloadException(string message) : base(message)
		{
			this.StatusCode = 0;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000021DA File Offset: 0x000003DA
		public DownloadException(int statusCode, string message) : base(message)
		{
			this.StatusCode = statusCode;
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000021EA File Offset: 0x000003EA
		public DownloadException(string message, Exception innerException) : base(message, innerException)
		{
			this.StatusCode = 0;
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000021FB File Offset: 0x000003FB
		public DownloadException(int statusCode, string message, Exception innerException) : base(message, innerException)
		{
			this.StatusCode = statusCode;
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000021A2 File Offset: 0x000003A2
		protected DownloadException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000011 RID: 17 RVA: 0x0000220C File Offset: 0x0000040C
		// (set) Token: 0x06000012 RID: 18 RVA: 0x00002214 File Offset: 0x00000414
		public int StatusCode { get; set; }

		// Token: 0x06000013 RID: 19 RVA: 0x0000221D File Offset: 0x0000041D
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
		}

		// Token: 0x0400000B RID: 11
		public const int Undefined = 0;

		// Token: 0x0400000C RID: 12
		public const int DownloadInterrupted = 507;

		// Token: 0x0400000D RID: 13
		public const int DownloadInterruptedDiskFull = 508;

		// Token: 0x0400000E RID: 14
		public const int FileIntegrityError = 417;

		// Token: 0x0400000F RID: 15
		public const int FileNotFound = 404;

		// Token: 0x04000010 RID: 16
		public const int IncorrectFileSize = 412;

		// Token: 0x04000011 RID: 17
		public const int RequestTimeout = 408;

		// Token: 0x04000012 RID: 18
		public const int UnknownError = 999;
	}
}
