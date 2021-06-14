using System;
using System.Runtime.Serialization;

namespace Microsoft.WindowsDeviceRecoveryTool.Model.Exceptions
{
	// Token: 0x02000011 RID: 17
	[Serializable]
	public class AutoUpdateNotEnoughSpaceException : Exception
	{
		// Token: 0x0600008D RID: 141 RVA: 0x0000315D File Offset: 0x0000135D
		public AutoUpdateNotEnoughSpaceException()
		{
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00003168 File Offset: 0x00001368
		public AutoUpdateNotEnoughSpaceException(string message) : base(message)
		{
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00003174 File Offset: 0x00001374
		public AutoUpdateNotEnoughSpaceException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00003181 File Offset: 0x00001381
		protected AutoUpdateNotEnoughSpaceException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000091 RID: 145 RVA: 0x00003190 File Offset: 0x00001390
		// (set) Token: 0x06000092 RID: 146 RVA: 0x000031A7 File Offset: 0x000013A7
		public long Available { get; set; }

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000093 RID: 147 RVA: 0x000031B0 File Offset: 0x000013B0
		// (set) Token: 0x06000094 RID: 148 RVA: 0x000031C7 File Offset: 0x000013C7
		public long Needed { get; set; }

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000095 RID: 149 RVA: 0x000031D0 File Offset: 0x000013D0
		// (set) Token: 0x06000096 RID: 150 RVA: 0x000031E7 File Offset: 0x000013E7
		public string Disk { get; set; }
	}
}
