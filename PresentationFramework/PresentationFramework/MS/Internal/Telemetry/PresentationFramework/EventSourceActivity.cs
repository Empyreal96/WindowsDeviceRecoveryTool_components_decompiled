using System;
using System.Diagnostics.Contracts;
using System.Diagnostics.Tracing;

namespace MS.Internal.Telemetry.PresentationFramework
{
	// Token: 0x020007E7 RID: 2023
	internal sealed class EventSourceActivity : IDisposable
	{
		// Token: 0x06007D0D RID: 32013 RVA: 0x00232B8C File Offset: 0x00230D8C
		internal EventSourceActivity(EventSource eventSource) : this(eventSource, default(EventSourceOptions))
		{
		}

		// Token: 0x06007D0E RID: 32014 RVA: 0x00232BA9 File Offset: 0x00230DA9
		internal EventSourceActivity(EventSource eventSource, EventSourceOptions startStopOptions) : this(eventSource, startStopOptions, Guid.Empty)
		{
		}

		// Token: 0x06007D0F RID: 32015 RVA: 0x00232BB8 File Offset: 0x00230DB8
		internal EventSourceActivity(EventSource eventSource, EventSourceOptions startStopOptions, Guid parentActivityId)
		{
			this._id = Guid.NewGuid();
			base..ctor();
			Contract.Requires<ArgumentNullException>(eventSource != null, "eventSource");
			this._eventSource = eventSource;
			this._startStopOptions = startStopOptions;
			this._parentId = parentActivityId;
		}

		// Token: 0x06007D10 RID: 32016 RVA: 0x00232BF0 File Offset: 0x00230DF0
		internal EventSourceActivity(EventSourceActivity parentActivity) : this(parentActivity, default(EventSourceOptions))
		{
		}

		// Token: 0x06007D11 RID: 32017 RVA: 0x00232C0D File Offset: 0x00230E0D
		internal EventSourceActivity(EventSourceActivity parentActivity, EventSourceOptions startStopOptions)
		{
			this._id = Guid.NewGuid();
			base..ctor();
			Contract.Requires<ArgumentNullException>(parentActivity != null, "parentActivity");
			this._eventSource = parentActivity.EventSource;
			this._startStopOptions = startStopOptions;
			this._parentId = parentActivity.Id;
		}

		// Token: 0x17001D15 RID: 7445
		// (get) Token: 0x06007D12 RID: 32018 RVA: 0x00232C4D File Offset: 0x00230E4D
		internal EventSource EventSource
		{
			get
			{
				return this._eventSource;
			}
		}

		// Token: 0x17001D16 RID: 7446
		// (get) Token: 0x06007D13 RID: 32019 RVA: 0x00232C55 File Offset: 0x00230E55
		internal Guid Id
		{
			get
			{
				return this._id;
			}
		}

		// Token: 0x06007D14 RID: 32020 RVA: 0x00232C60 File Offset: 0x00230E60
		internal void Start(string eventName)
		{
			Contract.Requires<ArgumentNullException>(eventName != null, "eventName");
			EventSourceActivity.EmptyStruct instance = EventSourceActivity.EmptyStruct.Instance;
			this.Start<EventSourceActivity.EmptyStruct>(eventName, ref instance);
		}

		// Token: 0x06007D15 RID: 32021 RVA: 0x00232C8A File Offset: 0x00230E8A
		internal void Start<T>(string eventName, T data)
		{
			this.Start<T>(eventName, ref data);
		}

		// Token: 0x06007D16 RID: 32022 RVA: 0x00232C98 File Offset: 0x00230E98
		internal void Stop(string eventName)
		{
			Contract.Requires<ArgumentNullException>(eventName != null, "eventName");
			EventSourceActivity.EmptyStruct instance = EventSourceActivity.EmptyStruct.Instance;
			this.Stop<EventSourceActivity.EmptyStruct>(eventName, ref instance);
		}

		// Token: 0x06007D17 RID: 32023 RVA: 0x00232CC2 File Offset: 0x00230EC2
		internal void Stop<T>(string eventName, T data)
		{
			this.Stop<T>(eventName, ref data);
		}

		// Token: 0x06007D18 RID: 32024 RVA: 0x00232CD0 File Offset: 0x00230ED0
		internal void Write(string eventName)
		{
			Contract.Requires<ArgumentNullException>(eventName != null, "eventName");
			EventSourceOptions eventSourceOptions = default(EventSourceOptions);
			EventSourceActivity.EmptyStruct instance = EventSourceActivity.EmptyStruct.Instance;
			this.Write<EventSourceActivity.EmptyStruct>(eventName, ref eventSourceOptions, ref instance);
		}

		// Token: 0x06007D19 RID: 32025 RVA: 0x00232D04 File Offset: 0x00230F04
		internal void Write(string eventName, EventSourceOptions options)
		{
			Contract.Requires<ArgumentNullException>(eventName != null, "eventName");
			EventSourceActivity.EmptyStruct instance = EventSourceActivity.EmptyStruct.Instance;
			this.Write<EventSourceActivity.EmptyStruct>(eventName, ref options, ref instance);
		}

		// Token: 0x06007D1A RID: 32026 RVA: 0x00232D30 File Offset: 0x00230F30
		internal void Write<T>(string eventName, T data)
		{
			EventSourceOptions eventSourceOptions = default(EventSourceOptions);
			this.Write<T>(eventName, ref eventSourceOptions, ref data);
		}

		// Token: 0x06007D1B RID: 32027 RVA: 0x00232D50 File Offset: 0x00230F50
		internal void Write<T>(string eventName, EventSourceOptions options, T data)
		{
			this.Write<T>(eventName, ref options, ref data);
		}

		// Token: 0x06007D1C RID: 32028 RVA: 0x00232D60 File Offset: 0x00230F60
		public void Dispose()
		{
			if (this._state == EventSourceActivity.State.Started)
			{
				this._state = EventSourceActivity.State.Stopped;
				EventSourceActivity.EmptyStruct instance = EventSourceActivity.EmptyStruct.Instance;
				this._eventSource.Write<EventSourceActivity.EmptyStruct>("Dispose", ref this._startStopOptions, ref this._id, ref EventSourceActivity._emptyGuid, ref instance);
			}
		}

		// Token: 0x06007D1D RID: 32029 RVA: 0x00232DA8 File Offset: 0x00230FA8
		private void Start<T>(string eventName, ref T data)
		{
			if (this._state != EventSourceActivity.State.Initialized)
			{
				throw new InvalidOperationException();
			}
			this._state = EventSourceActivity.State.Started;
			this._startStopOptions.Opcode = EventOpcode.Start;
			this._eventSource.Write<T>(eventName, ref this._startStopOptions, ref this._id, ref this._parentId, ref data);
			this._startStopOptions.Opcode = EventOpcode.Stop;
		}

		// Token: 0x06007D1E RID: 32030 RVA: 0x00232E01 File Offset: 0x00231001
		private void Write<T>(string eventName, ref EventSourceOptions options, ref T data)
		{
			if (this._state != EventSourceActivity.State.Started)
			{
				throw new InvalidOperationException();
			}
			this._eventSource.Write<T>(eventName, ref options, ref this._id, ref EventSourceActivity._emptyGuid, ref data);
		}

		// Token: 0x06007D1F RID: 32031 RVA: 0x00232E2B File Offset: 0x0023102B
		private void Stop<T>(string eventName, ref T data)
		{
			if (this._state != EventSourceActivity.State.Started)
			{
				throw new InvalidOperationException();
			}
			this._state = EventSourceActivity.State.Stopped;
			this._eventSource.Write<T>(eventName, ref this._startStopOptions, ref this._id, ref EventSourceActivity._emptyGuid, ref data);
		}

		// Token: 0x04003A9C RID: 15004
		private static Guid _emptyGuid;

		// Token: 0x04003A9D RID: 15005
		private readonly EventSource _eventSource;

		// Token: 0x04003A9E RID: 15006
		private EventSourceOptions _startStopOptions;

		// Token: 0x04003A9F RID: 15007
		private Guid _parentId;

		// Token: 0x04003AA0 RID: 15008
		private Guid _id;

		// Token: 0x04003AA1 RID: 15009
		private EventSourceActivity.State _state;

		// Token: 0x02000B88 RID: 2952
		private enum State
		{
			// Token: 0x04004B97 RID: 19351
			Initialized,
			// Token: 0x04004B98 RID: 19352
			Started,
			// Token: 0x04004B99 RID: 19353
			Stopped
		}

		// Token: 0x02000B89 RID: 2953
		[EventData]
		private class EmptyStruct
		{
			// Token: 0x06008E78 RID: 36472 RVA: 0x0000326D File Offset: 0x0000146D
			private EmptyStruct()
			{
			}

			// Token: 0x17001FB6 RID: 8118
			// (get) Token: 0x06008E79 RID: 36473 RVA: 0x0025C6DF File Offset: 0x0025A8DF
			internal static EventSourceActivity.EmptyStruct Instance
			{
				get
				{
					if (EventSourceActivity.EmptyStruct._instance == null)
					{
						EventSourceActivity.EmptyStruct._instance = new EventSourceActivity.EmptyStruct();
					}
					return EventSourceActivity.EmptyStruct._instance;
				}
			}

			// Token: 0x04004B9A RID: 19354
			private static EventSourceActivity.EmptyStruct _instance;
		}
	}
}
