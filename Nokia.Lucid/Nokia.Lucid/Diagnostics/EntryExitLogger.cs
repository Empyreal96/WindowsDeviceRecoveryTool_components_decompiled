using System;
using System.Diagnostics;

namespace Nokia.Lucid.Diagnostics
{
	// Token: 0x02000020 RID: 32
	public class EntryExitLogger : IDisposable
	{
		// Token: 0x060000F9 RID: 249 RVA: 0x0000A079 File Offset: 0x00008279
		private EntryExitLogger(string methodName, TraceSource source)
		{
			this.traceSource = source;
			this.methodName = methodName;
			this.traceSource.TraceEvent(TraceEventType.Start, 0, "Enter: " + this.methodName);
		}

		// Token: 0x060000FA RID: 250 RVA: 0x0000A0B0 File Offset: 0x000082B0
		public static IDisposable Log(string methodName, TraceSource source)
		{
			IDisposable result = null;
			if (source.Switch.Level > SourceLevels.Verbose)
			{
				result = new EntryExitLogger(methodName, source);
			}
			return result;
		}

		// Token: 0x060000FB RID: 251 RVA: 0x0000A0D7 File Offset: 0x000082D7
		public void Dispose()
		{
			this.traceSource.TraceEvent(TraceEventType.Stop, 0, "Exit: " + this.methodName);
		}

		// Token: 0x04000086 RID: 134
		private readonly string methodName;

		// Token: 0x04000087 RID: 135
		private readonly TraceSource traceSource;
	}
}
