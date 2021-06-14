using System;
using System.Diagnostics.Tracing;
using MS.Internal.Telemetry.PresentationFramework;

namespace MS.Internal.Telemetry
{
	// Token: 0x020007E6 RID: 2022
	internal static class NetFxVersionTraceLogger
	{
		// Token: 0x06007D0B RID: 32011 RVA: 0x00232B38 File Offset: 0x00230D38
		internal static void LogVersionDetails()
		{
			EventSource provider = TraceLoggingProvider.GetProvider();
			NetFxVersionTraceLogger.VersionInfo data = default(NetFxVersionTraceLogger.VersionInfo);
			data.TargetFrameworkVersion = NetfxVersionHelper.GetTargetFrameworkVersion();
			data.NetfxReleaseVersion = NetfxVersionHelper.GetNetFXReleaseVersion();
			if (provider != null)
			{
				provider.Write<NetFxVersionTraceLogger.VersionInfo>(NetFxVersionTraceLogger.NetFxVersion, TelemetryEventSource.MeasuresOptions(), data);
			}
		}

		// Token: 0x04003A9B RID: 15003
		private static readonly string NetFxVersion = "NetFxVersion";

		// Token: 0x02000B87 RID: 2951
		[EventData]
		private struct VersionInfo
		{
			// Token: 0x17001FB4 RID: 8116
			// (get) Token: 0x06008E74 RID: 36468 RVA: 0x0025C6BD File Offset: 0x0025A8BD
			// (set) Token: 0x06008E75 RID: 36469 RVA: 0x0025C6C5 File Offset: 0x0025A8C5
			public string TargetFrameworkVersion
			{
				get
				{
					return this._targetFrameworkVersion;
				}
				set
				{
					this._targetFrameworkVersion = value;
				}
			}

			// Token: 0x17001FB5 RID: 8117
			// (get) Token: 0x06008E76 RID: 36470 RVA: 0x0025C6CE File Offset: 0x0025A8CE
			// (set) Token: 0x06008E77 RID: 36471 RVA: 0x0025C6D6 File Offset: 0x0025A8D6
			public int NetfxReleaseVersion
			{
				get
				{
					return this._netfxReleaseVersion;
				}
				set
				{
					this._netfxReleaseVersion = value;
				}
			}

			// Token: 0x04004B94 RID: 19348
			private string _targetFrameworkVersion;

			// Token: 0x04004B95 RID: 19349
			private int _netfxReleaseVersion;
		}
	}
}
