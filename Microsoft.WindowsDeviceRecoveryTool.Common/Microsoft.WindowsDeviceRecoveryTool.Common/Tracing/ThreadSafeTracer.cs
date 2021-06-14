using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Microsoft.WindowsDeviceRecoveryTool.Common.Tracing
{
	// Token: 0x0200000E RID: 14
	[CompilerGenerated]
	internal sealed class ThreadSafeTracer : IThreadSafeTracer
	{
		// Token: 0x06000056 RID: 86 RVA: 0x000034F8 File Offset: 0x000016F8
		public ThreadSafeTracer(string name, SourceLevels tracingLevel)
		{
			this.tracer = new TraceSource(name)
			{
				Switch = new SourceSwitch("Main switch")
				{
					Level = tracingLevel
				}
			};
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00003542 File Offset: 0x00001742
		public ThreadSafeTracer(string name) : this(name, SourceLevels.All)
		{
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00003550 File Offset: 0x00001750
		public void TraceData(TraceEventType eventType, params object[] data)
		{
			lock (this.syncRoot)
			{
				this.tracer.TraceData(eventType, 0, data);
			}
		}

		// Token: 0x06000059 RID: 89 RVA: 0x000035A4 File Offset: 0x000017A4
		public void DisableTracing()
		{
			lock (this.syncRoot)
			{
				this.tracer.Switch.Level = SourceLevels.Off;
			}
		}

		// Token: 0x0600005A RID: 90 RVA: 0x000035FC File Offset: 0x000017FC
		public void EnableTracing()
		{
			lock (this.syncRoot)
			{
				this.tracer.Switch.Level = SourceLevels.All;
			}
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00003654 File Offset: 0x00001854
		public void AddTraceListener(TraceListener traceListener)
		{
			lock (this.syncRoot)
			{
				this.tracer.Listeners.Add(traceListener);
			}
		}

		// Token: 0x0600005C RID: 92 RVA: 0x000036AC File Offset: 0x000018AC
		public void Close()
		{
			lock (this.syncRoot)
			{
				this.tracer.Close();
			}
		}

		// Token: 0x04000011 RID: 17
		private readonly object syncRoot = new object();

		// Token: 0x04000012 RID: 18
		private readonly TraceSource tracer;
	}
}
