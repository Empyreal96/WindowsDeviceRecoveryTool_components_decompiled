using System;
using System.Runtime.Serialization;

namespace Microsoft.WindowsDeviceRecoveryTool.Model.Exceptions
{
	// Token: 0x02000042 RID: 66
	[Serializable]
	public class NotEnoughSpaceException : Exception
	{
		// Token: 0x0600018B RID: 395 RVA: 0x00004CCE File Offset: 0x00002ECE
		public NotEnoughSpaceException() : base("There is not enough space on disk")
		{
		}

		// Token: 0x0600018C RID: 396 RVA: 0x00004CDE File Offset: 0x00002EDE
		public NotEnoughSpaceException(string message) : base(message)
		{
		}

		// Token: 0x0600018D RID: 397 RVA: 0x00004CEA File Offset: 0x00002EEA
		public NotEnoughSpaceException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x0600018E RID: 398 RVA: 0x00004CF7 File Offset: 0x00002EF7
		protected NotEnoughSpaceException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x0600018F RID: 399 RVA: 0x00004D04 File Offset: 0x00002F04
		// (set) Token: 0x06000190 RID: 400 RVA: 0x00004D1B File Offset: 0x00002F1B
		public long Available { get; set; }

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000191 RID: 401 RVA: 0x00004D24 File Offset: 0x00002F24
		// (set) Token: 0x06000192 RID: 402 RVA: 0x00004D3B File Offset: 0x00002F3B
		public long Needed { get; set; }

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000193 RID: 403 RVA: 0x00004D44 File Offset: 0x00002F44
		// (set) Token: 0x06000194 RID: 404 RVA: 0x00004D5B File Offset: 0x00002F5B
		public string Disk { get; set; }
	}
}
