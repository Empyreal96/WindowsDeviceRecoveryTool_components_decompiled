using System;
using System.Runtime.Serialization;

namespace Microsoft.WindowsDeviceRecoveryTool.Model.Exceptions
{
	// Token: 0x02000044 RID: 68
	[Serializable]
	public class PlannedServiceBreakException : Exception
	{
		// Token: 0x06000198 RID: 408 RVA: 0x00004D88 File Offset: 0x00002F88
		public PlannedServiceBreakException()
		{
		}

		// Token: 0x06000199 RID: 409 RVA: 0x00004D93 File Offset: 0x00002F93
		public PlannedServiceBreakException(string message) : base(message)
		{
		}

		// Token: 0x0600019A RID: 410 RVA: 0x00004D9F File Offset: 0x00002F9F
		public PlannedServiceBreakException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x0600019B RID: 411 RVA: 0x00004DAC File Offset: 0x00002FAC
		protected PlannedServiceBreakException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x0600019C RID: 412 RVA: 0x00004DB9 File Offset: 0x00002FB9
		public PlannedServiceBreakException(DateTime start, DateTime end)
		{
			this.Start = start;
			this.End = end;
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x0600019D RID: 413 RVA: 0x00004DD4 File Offset: 0x00002FD4
		// (set) Token: 0x0600019E RID: 414 RVA: 0x00004DEB File Offset: 0x00002FEB
		public DateTime Start { get; private set; }

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x0600019F RID: 415 RVA: 0x00004DF4 File Offset: 0x00002FF4
		// (set) Token: 0x060001A0 RID: 416 RVA: 0x00004E0B File Offset: 0x0000300B
		public DateTime End { get; private set; }

		// Token: 0x060001A1 RID: 417 RVA: 0x00004E14 File Offset: 0x00003014
		public new virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info != null)
			{
				base.GetObjectData(info, context);
			}
		}
	}
}
