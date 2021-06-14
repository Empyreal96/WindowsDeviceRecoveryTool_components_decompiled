using System;
using System.Diagnostics.Tracing;

namespace MS.Internal.Telemetry.PresentationFramework
{
	// Token: 0x020007E8 RID: 2024
	internal class TelemetryEventSource : EventSource
	{
		// Token: 0x06007D20 RID: 32032 RVA: 0x00232E61 File Offset: 0x00231061
		internal TelemetryEventSource(string eventSourceName) : base(eventSourceName, EventSourceSettings.EtwSelfDescribingEventFormat, TelemetryEventSource.telemetryTraits)
		{
		}

		// Token: 0x06007D21 RID: 32033 RVA: 0x00232E70 File Offset: 0x00231070
		protected TelemetryEventSource() : base(EventSourceSettings.EtwSelfDescribingEventFormat, TelemetryEventSource.telemetryTraits)
		{
		}

		// Token: 0x06007D22 RID: 32034 RVA: 0x00232E80 File Offset: 0x00231080
		internal static EventSourceOptions TelemetryOptions()
		{
			return new EventSourceOptions
			{
				Keywords = (EventKeywords)35184372088832L
			};
		}

		// Token: 0x06007D23 RID: 32035 RVA: 0x00232EA8 File Offset: 0x002310A8
		internal static EventSourceOptions MeasuresOptions()
		{
			return new EventSourceOptions
			{
				Keywords = (EventKeywords)70368744177664L
			};
		}

		// Token: 0x06007D24 RID: 32036 RVA: 0x00232ED0 File Offset: 0x002310D0
		internal static EventSourceOptions CriticalDataOptions()
		{
			return new EventSourceOptions
			{
				Keywords = (EventKeywords)140737488355328L
			};
		}

		// Token: 0x04003AA2 RID: 15010
		internal const EventKeywords Reserved44Keyword = (EventKeywords)17592186044416L;

		// Token: 0x04003AA3 RID: 15011
		internal const EventKeywords TelemetryKeyword = (EventKeywords)35184372088832L;

		// Token: 0x04003AA4 RID: 15012
		internal const EventKeywords MeasuresKeyword = (EventKeywords)70368744177664L;

		// Token: 0x04003AA5 RID: 15013
		internal const EventKeywords CriticalDataKeyword = (EventKeywords)140737488355328L;

		// Token: 0x04003AA6 RID: 15014
		internal const EventTags CoreData = (EventTags)524288;

		// Token: 0x04003AA7 RID: 15015
		internal const EventTags InjectXToken = (EventTags)1048576;

		// Token: 0x04003AA8 RID: 15016
		internal const EventTags RealtimeLatency = (EventTags)2097152;

		// Token: 0x04003AA9 RID: 15017
		internal const EventTags NormalLatency = (EventTags)4194304;

		// Token: 0x04003AAA RID: 15018
		internal const EventTags CriticalPersistence = (EventTags)8388608;

		// Token: 0x04003AAB RID: 15019
		internal const EventTags NormalPersistence = (EventTags)16777216;

		// Token: 0x04003AAC RID: 15020
		internal const EventTags DropPii = (EventTags)33554432;

		// Token: 0x04003AAD RID: 15021
		internal const EventTags HashPii = (EventTags)67108864;

		// Token: 0x04003AAE RID: 15022
		internal const EventTags MarkPii = (EventTags)134217728;

		// Token: 0x04003AAF RID: 15023
		internal const EventFieldTags DropPiiField = (EventFieldTags)67108864;

		// Token: 0x04003AB0 RID: 15024
		internal const EventFieldTags HashPiiField = (EventFieldTags)134217728;

		// Token: 0x04003AB1 RID: 15025
		private static readonly string[] telemetryTraits = new string[]
		{
			"ETW_GROUP",
			"{4f50731a-89cf-4782-b3e0-dce8c90476ba}"
		};
	}
}
