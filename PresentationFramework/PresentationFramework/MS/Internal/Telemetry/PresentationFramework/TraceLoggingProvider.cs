using System;
using System.Diagnostics.Tracing;

namespace MS.Internal.Telemetry.PresentationFramework
{
	// Token: 0x020007E9 RID: 2025
	internal static class TraceLoggingProvider
	{
		// Token: 0x06007D26 RID: 32038 RVA: 0x00232F14 File Offset: 0x00231114
		internal static EventSource GetProvider()
		{
			if (TraceLoggingProvider._logger == null)
			{
				object lockObject = TraceLoggingProvider._lockObject;
				lock (lockObject)
				{
					if (TraceLoggingProvider._logger == null)
					{
						try
						{
							TraceLoggingProvider._logger = new TelemetryEventSource(TraceLoggingProvider.ProviderName);
						}
						catch (ArgumentException)
						{
						}
					}
				}
			}
			return TraceLoggingProvider._logger;
		}

		// Token: 0x04003AB2 RID: 15026
		private static EventSource _logger;

		// Token: 0x04003AB3 RID: 15027
		private static object _lockObject = new object();

		// Token: 0x04003AB4 RID: 15028
		private static readonly string ProviderName = "Microsoft.DOTNET.WPF.PresentationFramework";
	}
}
