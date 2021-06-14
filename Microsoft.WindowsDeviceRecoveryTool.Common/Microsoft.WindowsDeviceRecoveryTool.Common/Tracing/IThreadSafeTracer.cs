using System;
using System.Diagnostics;

namespace Microsoft.WindowsDeviceRecoveryTool.Common.Tracing
{
	// Token: 0x0200000D RID: 13
	public interface IThreadSafeTracer
	{
		// Token: 0x06000051 RID: 81
		void TraceData(TraceEventType eventType, params object[] data);

		// Token: 0x06000052 RID: 82
		void DisableTracing();

		// Token: 0x06000053 RID: 83
		void EnableTracing();

		// Token: 0x06000054 RID: 84
		void AddTraceListener(TraceListener traceListener);

		// Token: 0x06000055 RID: 85
		void Close();
	}
}
