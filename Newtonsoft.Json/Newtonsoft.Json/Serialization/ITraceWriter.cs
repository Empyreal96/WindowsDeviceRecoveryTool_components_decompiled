using System;
using System.Diagnostics;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x020000A7 RID: 167
	public interface ITraceWriter
	{
		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x06000854 RID: 2132
		TraceLevel LevelFilter { get; }

		// Token: 0x06000855 RID: 2133
		void Trace(TraceLevel level, string message, Exception ex);
	}
}
