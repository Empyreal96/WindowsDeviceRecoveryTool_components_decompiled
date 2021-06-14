using System;
using System.Diagnostics;

namespace Microsoft.WindowsDeviceRecoveryTool.Common.Tracing
{
	// Token: 0x0200000F RID: 15
	internal class TraceListenerAdapter : TraceListener
	{
		// Token: 0x0600005D RID: 93 RVA: 0x00003700 File Offset: 0x00001900
		public TraceListenerAdapter(ITraceWriter writer)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			this.writer = writer;
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00003734 File Offset: 0x00001934
		public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, params object[] data)
		{
			this.writer.TraceData(eventCache, source, eventType, id, data);
		}

		// Token: 0x0600005F RID: 95 RVA: 0x0000374A File Offset: 0x0000194A
		public override void Write(string message)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00003752 File Offset: 0x00001952
		public override void WriteLine(string message)
		{
			throw new NotImplementedException();
		}

		// Token: 0x04000013 RID: 19
		private readonly ITraceWriter writer;
	}
}
