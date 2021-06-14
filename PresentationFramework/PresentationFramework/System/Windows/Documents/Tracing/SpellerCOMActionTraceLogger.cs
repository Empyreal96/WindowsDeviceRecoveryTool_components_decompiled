using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using MS.Internal;
using MS.Internal.Telemetry.PresentationFramework;

namespace System.Windows.Documents.Tracing
{
	// Token: 0x0200043B RID: 1083
	internal class SpellerCOMActionTraceLogger : IDisposable
	{
		// Token: 0x06003F80 RID: 16256 RVA: 0x00124D9C File Offset: 0x00122F9C
		public SpellerCOMActionTraceLogger(WinRTSpellerInterop caller, SpellerCOMActionTraceLogger.Actions action)
		{
			this._action = action;
			SpellerCOMActionTraceLogger.InstanceInfo instanceInfo = null;
			object lockObject = SpellerCOMActionTraceLogger._lockObject;
			lock (lockObject)
			{
				if (!SpellerCOMActionTraceLogger._instanceInfos.TryGetValue(caller, out instanceInfo))
				{
					instanceInfo = new SpellerCOMActionTraceLogger.InstanceInfo
					{
						Id = Guid.NewGuid(),
						CumulativeCallTime100Ns = new Dictionary<SpellerCOMActionTraceLogger.Actions, long>(),
						NumCallsMeasured = new Dictionary<SpellerCOMActionTraceLogger.Actions, long>()
					};
					foreach (object obj in Enum.GetValues(typeof(SpellerCOMActionTraceLogger.Actions)))
					{
						SpellerCOMActionTraceLogger.Actions key = (SpellerCOMActionTraceLogger.Actions)obj;
						instanceInfo.CumulativeCallTime100Ns.Add(key, 0L);
						instanceInfo.NumCallsMeasured.Add(key, 0L);
					}
					SpellerCOMActionTraceLogger._instanceInfos.Add(caller, instanceInfo);
				}
			}
			this._instanceInfo = instanceInfo;
			this._beginTicks = DateTime.Now.Ticks;
		}

		// Token: 0x06003F81 RID: 16257 RVA: 0x00124EB0 File Offset: 0x001230B0
		private void UpdateRunningAverageAndLogDebugInfo(long endTicks)
		{
			try
			{
				long num = endTicks - this._beginTicks;
				object lockObject = SpellerCOMActionTraceLogger._lockObject;
				lock (lockObject)
				{
					Dictionary<SpellerCOMActionTraceLogger.Actions, long> numCallsMeasured = this._instanceInfo.NumCallsMeasured;
					SpellerCOMActionTraceLogger.Actions action = this._action;
					long num2 = numCallsMeasured[action];
					numCallsMeasured[action] = num2 + 1L;
					Dictionary<SpellerCOMActionTraceLogger.Actions, long> cumulativeCallTime100Ns = this._instanceInfo.CumulativeCallTime100Ns;
					action = this._action;
					cumulativeCallTime100Ns[action] += num;
				}
				long num3 = (long)Math.Floor(1.0 * (double)this._instanceInfo.CumulativeCallTime100Ns[this._action] / (double)this._instanceInfo.NumCallsMeasured[this._action]);
				if (this._action == SpellerCOMActionTraceLogger.Actions.RegisterUserDictionary || this._action == SpellerCOMActionTraceLogger.Actions.UnregisterUserDictionary || num > SpellerCOMActionTraceLogger._timeLimits100Ns[this._action] || num3 > 2L * SpellerCOMActionTraceLogger._timeLimits100Ns[this._action])
				{
					EventSource provider = TraceLoggingProvider.GetProvider();
					EventSourceOptions options = TelemetryEventSource.MeasuresOptions();
					SpellerCOMActionTraceLogger.SpellerCOMTimingData data = new SpellerCOMActionTraceLogger.SpellerCOMTimingData
					{
						TextBoxBaseIdentifier = this._instanceInfo.Id.ToString(),
						SpellerCOMAction = this._action.ToString(),
						CallTimeForCOMCallMs = (long)Math.Floor((double)num * 1.0 / 10000.0),
						RunningAverageCallTimeForCOMCallsMs = (long)Math.Floor((double)num3 * 1.0 / 10000.0)
					};
					if (provider != null)
					{
						provider.Write<SpellerCOMActionTraceLogger.SpellerCOMTimingData>(SpellerCOMActionTraceLogger.SpellerCOMLatencyMeasurement, options, data);
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x06003F82 RID: 16258 RVA: 0x001250A4 File Offset: 0x001232A4
		protected virtual void Dispose(bool disposing)
		{
			if (!this._disposed)
			{
				if (disposing)
				{
					this.UpdateRunningAverageAndLogDebugInfo(DateTime.Now.Ticks);
				}
				this._disposed = true;
			}
		}

		// Token: 0x06003F83 RID: 16259 RVA: 0x001250D6 File Offset: 0x001232D6
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x04002738 RID: 10040
		private static readonly string SpellerCOMLatencyMeasurement = "SpellerCOMLatencyMeasurement";

		// Token: 0x04002739 RID: 10041
		private static readonly Dictionary<SpellerCOMActionTraceLogger.Actions, long> _timeLimits100Ns = new Dictionary<SpellerCOMActionTraceLogger.Actions, long>
		{
			{
				SpellerCOMActionTraceLogger.Actions.SpellCheckerCreation,
				2500000L
			},
			{
				SpellerCOMActionTraceLogger.Actions.ComprehensiveCheck,
				500000L
			},
			{
				SpellerCOMActionTraceLogger.Actions.RegisterUserDictionary,
				10000000L
			},
			{
				SpellerCOMActionTraceLogger.Actions.UnregisterUserDictionary,
				10000000L
			}
		};

		// Token: 0x0400273A RID: 10042
		private static WeakDictionary<WinRTSpellerInterop, SpellerCOMActionTraceLogger.InstanceInfo> _instanceInfos = new WeakDictionary<WinRTSpellerInterop, SpellerCOMActionTraceLogger.InstanceInfo>();

		// Token: 0x0400273B RID: 10043
		private static object _lockObject = new object();

		// Token: 0x0400273C RID: 10044
		private SpellerCOMActionTraceLogger.Actions _action;

		// Token: 0x0400273D RID: 10045
		private long _beginTicks;

		// Token: 0x0400273E RID: 10046
		private SpellerCOMActionTraceLogger.InstanceInfo _instanceInfo;

		// Token: 0x0400273F RID: 10047
		private bool _disposed;

		// Token: 0x02000928 RID: 2344
		public enum Actions
		{
			// Token: 0x04004389 RID: 17289
			SpellCheckerCreation,
			// Token: 0x0400438A RID: 17290
			RegisterUserDictionary,
			// Token: 0x0400438B RID: 17291
			UnregisterUserDictionary,
			// Token: 0x0400438C RID: 17292
			ComprehensiveCheck
		}

		// Token: 0x02000929 RID: 2345
		private class InstanceInfo
		{
			// Token: 0x17001E64 RID: 7780
			// (get) Token: 0x06008680 RID: 34432 RVA: 0x0024E87D File Offset: 0x0024CA7D
			// (set) Token: 0x06008681 RID: 34433 RVA: 0x0024E885 File Offset: 0x0024CA85
			public Guid Id { get; set; }

			// Token: 0x17001E65 RID: 7781
			// (get) Token: 0x06008682 RID: 34434 RVA: 0x0024E88E File Offset: 0x0024CA8E
			// (set) Token: 0x06008683 RID: 34435 RVA: 0x0024E896 File Offset: 0x0024CA96
			public Dictionary<SpellerCOMActionTraceLogger.Actions, long> CumulativeCallTime100Ns { get; set; }

			// Token: 0x17001E66 RID: 7782
			// (get) Token: 0x06008684 RID: 34436 RVA: 0x0024E89F File Offset: 0x0024CA9F
			// (set) Token: 0x06008685 RID: 34437 RVA: 0x0024E8A7 File Offset: 0x0024CAA7
			public Dictionary<SpellerCOMActionTraceLogger.Actions, long> NumCallsMeasured { get; set; }
		}

		// Token: 0x0200092A RID: 2346
		[EventData]
		private struct SpellerCOMTimingData
		{
			// Token: 0x17001E67 RID: 7783
			// (get) Token: 0x06008687 RID: 34439 RVA: 0x0024E8B0 File Offset: 0x0024CAB0
			// (set) Token: 0x06008688 RID: 34440 RVA: 0x0024E8B8 File Offset: 0x0024CAB8
			public string TextBoxBaseIdentifier { get; set; }

			// Token: 0x17001E68 RID: 7784
			// (get) Token: 0x06008689 RID: 34441 RVA: 0x0024E8C1 File Offset: 0x0024CAC1
			// (set) Token: 0x0600868A RID: 34442 RVA: 0x0024E8C9 File Offset: 0x0024CAC9
			public string SpellerCOMAction { get; set; }

			// Token: 0x17001E69 RID: 7785
			// (get) Token: 0x0600868B RID: 34443 RVA: 0x0024E8D2 File Offset: 0x0024CAD2
			// (set) Token: 0x0600868C RID: 34444 RVA: 0x0024E8DA File Offset: 0x0024CADA
			public long CallTimeForCOMCallMs { get; set; }

			// Token: 0x17001E6A RID: 7786
			// (get) Token: 0x0600868D RID: 34445 RVA: 0x0024E8E3 File Offset: 0x0024CAE3
			// (set) Token: 0x0600868E RID: 34446 RVA: 0x0024E8EB File Offset: 0x0024CAEB
			public long RunningAverageCallTimeForCOMCallsMs { get; set; }
		}
	}
}
