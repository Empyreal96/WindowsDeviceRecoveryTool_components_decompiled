using System;
using System.Diagnostics.Tracing;

namespace MS.Internal.Telemetry.PresentationFramework
{
	// Token: 0x020007EB RID: 2027
	internal static class ControlsTraceLogger
	{
		// Token: 0x06007D28 RID: 32040 RVA: 0x00232F98 File Offset: 0x00231198
		internal static void LogUsedControlsDetails()
		{
			EventSource provider = TraceLoggingProvider.GetProvider();
			if (provider != null)
			{
				provider.Write(ControlsTraceLogger.ControlsUsed, TelemetryEventSource.MeasuresOptions(), new
				{
					ControlsUsedInApp = ControlsTraceLogger._telemetryControls
				});
			}
		}

		// Token: 0x06007D29 RID: 32041 RVA: 0x00232FC8 File Offset: 0x002311C8
		internal static void AddControl(TelemetryControls control)
		{
			ControlsTraceLogger._telemetryControls |= control;
		}

		// Token: 0x04003AE2 RID: 15074
		private static readonly string ControlsUsed = "ControlsUsed";

		// Token: 0x04003AE3 RID: 15075
		private static TelemetryControls _telemetryControls = TelemetryControls.None;
	}
}
