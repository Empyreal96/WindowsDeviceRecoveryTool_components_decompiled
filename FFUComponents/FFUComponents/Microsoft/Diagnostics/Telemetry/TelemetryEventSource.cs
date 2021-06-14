using System;
using System.Globalization;
using System.Threading;
using Microsoft.Diagnostics.Tracing;

namespace Microsoft.Diagnostics.Telemetry
{
	// Token: 0x02000062 RID: 98
	internal class TelemetryEventSource : EventSource
	{
		// Token: 0x06000247 RID: 583 RVA: 0x0000B764 File Offset: 0x00009964
		public TelemetryEventSource(string eventSourceName) : base(eventSourceName, EventSourceSettings.EtwSelfDescribingEventFormat, TelemetryEventSource.telemetryTraits)
		{
		}

		// Token: 0x06000248 RID: 584 RVA: 0x0000B773 File Offset: 0x00009973
		protected TelemetryEventSource() : base(EventSourceSettings.EtwSelfDescribingEventFormat, TelemetryEventSource.telemetryTraits)
		{
		}

		// Token: 0x06000249 RID: 585 RVA: 0x0000B784 File Offset: 0x00009984
		public static EventSourceOptions TelemetryOptions()
		{
			return new EventSourceOptions
			{
				Keywords = (EventKeywords)35184372088832L
			};
		}

		// Token: 0x0600024A RID: 586 RVA: 0x0000B7AC File Offset: 0x000099AC
		public static EventSourceOptions MeasuresOptions()
		{
			return new EventSourceOptions
			{
				Keywords = (EventKeywords)70368744177664L
			};
		}

		// Token: 0x0600024B RID: 587 RVA: 0x0000B7D4 File Offset: 0x000099D4
		public static EventSourceOptions CriticalDataOptions()
		{
			return new EventSourceOptions
			{
				Keywords = (EventKeywords)140737488355328L
			};
		}

		// Token: 0x0600024C RID: 588 RVA: 0x0000B7FA File Offset: 0x000099FA
		[NonEvent]
		public void WriteTelemetry<T>(T data)
		{
			if (base.IsEnabled())
			{
				base.Write<T>(null, ref TelemetryEventSource.EventDescriptionInfo<T>.GetInstance().Options, ref data);
			}
		}

		// Token: 0x0600024D RID: 589 RVA: 0x0000B817 File Offset: 0x00009A17
		[NonEvent]
		public void WriteTelemetry<T>(ref Guid activityId, ref Guid relatedActivityId, ref T data)
		{
			if (base.IsEnabled())
			{
				base.Write<T>(null, ref TelemetryEventSource.EventDescriptionInfo<T>.GetInstance().Options, ref activityId, ref relatedActivityId, ref data);
			}
		}

		// Token: 0x0600024E RID: 590 RVA: 0x0000B835 File Offset: 0x00009A35
		public static EventSourceOptions GetEventSourceOptionsForType<T>()
		{
			return TelemetryEventSource.EventDescriptionInfo<T>.GetInstance().Options;
		}

		// Token: 0x040001E4 RID: 484
		public const EventKeywords Reserved44Keyword = (EventKeywords)17592186044416L;

		// Token: 0x040001E5 RID: 485
		public const EventKeywords TelemetryKeyword = (EventKeywords)35184372088832L;

		// Token: 0x040001E6 RID: 486
		public const EventKeywords MeasuresKeyword = (EventKeywords)70368744177664L;

		// Token: 0x040001E7 RID: 487
		public const EventKeywords CriticalDataKeyword = (EventKeywords)140737488355328L;

		// Token: 0x040001E8 RID: 488
		public const EventTags CostDeferredLatency = (EventTags)262144;

		// Token: 0x040001E9 RID: 489
		public const EventTags CoreData = (EventTags)524288;

		// Token: 0x040001EA RID: 490
		public const EventTags InjectXToken = (EventTags)1048576;

		// Token: 0x040001EB RID: 491
		public const EventTags RealtimeLatency = (EventTags)2097152;

		// Token: 0x040001EC RID: 492
		public const EventTags NormalLatency = (EventTags)4194304;

		// Token: 0x040001ED RID: 493
		public const EventTags CriticalPersistence = (EventTags)8388608;

		// Token: 0x040001EE RID: 494
		public const EventTags NormalPersistence = (EventTags)16777216;

		// Token: 0x040001EF RID: 495
		public const EventTags DropPii = (EventTags)33554432;

		// Token: 0x040001F0 RID: 496
		public const EventTags HashPii = (EventTags)67108864;

		// Token: 0x040001F1 RID: 497
		public const EventTags MarkPii = (EventTags)134217728;

		// Token: 0x040001F2 RID: 498
		public const EventFieldTags DropPiiField = (EventFieldTags)67108864;

		// Token: 0x040001F3 RID: 499
		public const EventFieldTags HashPiiField = (EventFieldTags)134217728;

		// Token: 0x040001F4 RID: 500
		private static readonly string[] telemetryTraits = new string[]
		{
			"ETW_GROUP",
			"{4f50731a-89cf-4782-b3e0-dce8c90476ba}"
		};

		// Token: 0x02000063 RID: 99
		private class EventDescriptionInfo<T>
		{
			// Token: 0x06000250 RID: 592 RVA: 0x0000B870 File Offset: 0x00009A70
			private EventDescriptionInfo(Type type)
			{
				Type typeFromHandle = typeof(T);
				EventDescriptionAttribute eventDescriptionAttribute = null;
				object[] customAttributes = typeFromHandle.GetCustomAttributes(typeof(EventDescriptionAttribute), false);
				int num = 0;
				if (num < customAttributes.Length)
				{
					object obj = customAttributes[num];
					eventDescriptionAttribute = (obj as EventDescriptionAttribute);
				}
				if (eventDescriptionAttribute == null)
				{
					throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "WriteTelemetry requires the data type {0} to have an {1} attribute", new object[]
					{
						typeof(T).Name,
						typeof(EventDescriptionAttribute).Name
					}));
				}
				this.Options = new EventSourceOptions
				{
					Keywords = eventDescriptionAttribute.Keywords,
					Level = eventDescriptionAttribute.Level,
					Opcode = eventDescriptionAttribute.Opcode,
					Tags = eventDescriptionAttribute.Tags,
					ActivityOptions = eventDescriptionAttribute.ActivityOptions
				};
			}

			// Token: 0x06000251 RID: 593 RVA: 0x0000B958 File Offset: 0x00009B58
			public static TelemetryEventSource.EventDescriptionInfo<T> GetInstance()
			{
				if (TelemetryEventSource.EventDescriptionInfo<T>.instance == null)
				{
					TelemetryEventSource.EventDescriptionInfo<T> value = new TelemetryEventSource.EventDescriptionInfo<T>(typeof(T));
					Interlocked.CompareExchange<TelemetryEventSource.EventDescriptionInfo<T>>(ref TelemetryEventSource.EventDescriptionInfo<T>.instance, value, null);
				}
				return TelemetryEventSource.EventDescriptionInfo<T>.instance;
			}

			// Token: 0x040001F5 RID: 501
			private static TelemetryEventSource.EventDescriptionInfo<T> instance;

			// Token: 0x040001F6 RID: 502
			public EventSourceOptions Options;
		}
	}
}
