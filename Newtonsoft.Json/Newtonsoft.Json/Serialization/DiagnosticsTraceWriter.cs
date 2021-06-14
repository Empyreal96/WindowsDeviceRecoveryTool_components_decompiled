using System;
using System.Diagnostics;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x020000A8 RID: 168
	public class DiagnosticsTraceWriter : ITraceWriter
	{
		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x06000856 RID: 2134 RVA: 0x00020565 File Offset: 0x0001E765
		// (set) Token: 0x06000857 RID: 2135 RVA: 0x0002056D File Offset: 0x0001E76D
		public TraceLevel LevelFilter { get; set; }

		// Token: 0x06000858 RID: 2136 RVA: 0x00020578 File Offset: 0x0001E778
		private TraceEventType GetTraceEventType(TraceLevel level)
		{
			switch (level)
			{
			case TraceLevel.Error:
				return TraceEventType.Error;
			case TraceLevel.Warning:
				return TraceEventType.Warning;
			case TraceLevel.Info:
				return TraceEventType.Information;
			case TraceLevel.Verbose:
				return TraceEventType.Verbose;
			default:
				throw new ArgumentOutOfRangeException("level");
			}
		}

		// Token: 0x06000859 RID: 2137 RVA: 0x000205B4 File Offset: 0x0001E7B4
		public void Trace(TraceLevel level, string message, Exception ex)
		{
			if (level == TraceLevel.Off)
			{
				return;
			}
			TraceEventCache eventCache = new TraceEventCache();
			TraceEventType traceEventType = this.GetTraceEventType(level);
			foreach (object obj in System.Diagnostics.Trace.Listeners)
			{
				TraceListener traceListener = (TraceListener)obj;
				if (!traceListener.IsThreadSafe)
				{
					lock (traceListener)
					{
						traceListener.TraceEvent(eventCache, "Newtonsoft.Json", traceEventType, 0, message);
						goto IL_6D;
					}
					goto IL_5E;
				}
				goto IL_5E;
				IL_6D:
				if (System.Diagnostics.Trace.AutoFlush)
				{
					traceListener.Flush();
					continue;
				}
				continue;
				IL_5E:
				traceListener.TraceEvent(eventCache, "Newtonsoft.Json", traceEventType, 0, message);
				goto IL_6D;
			}
		}
	}
}
