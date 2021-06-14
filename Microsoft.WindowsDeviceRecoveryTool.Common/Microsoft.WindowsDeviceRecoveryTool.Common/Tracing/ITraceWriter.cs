using System;
using System.Diagnostics;

namespace Microsoft.WindowsDeviceRecoveryTool.Common.Tracing
{
	// Token: 0x0200000B RID: 11
	public interface ITraceWriter
	{
		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000044 RID: 68
		string LogFilePath { get; }

		// Token: 0x06000045 RID: 69
		void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, params object[] data);

		// Token: 0x06000046 RID: 70
		void ChangeLogFolder(string newPath);

		// Token: 0x06000047 RID: 71
		void Close();
	}
}
