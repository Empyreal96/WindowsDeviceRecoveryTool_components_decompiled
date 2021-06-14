using System;
using Microsoft.WindowsDeviceRecoveryTool.Common.Tracing;
using Microsoft.WindowsDeviceRecoveryTool.StateMachine.BaseTypes;

namespace Microsoft.WindowsDeviceRecoveryTool.StateMachine.DefaultTypes
{
	// Token: 0x02000011 RID: 17
	public abstract class StateMachineState : BaseState
	{
		// Token: 0x06000076 RID: 118 RVA: 0x00003700 File Offset: 0x00001900
		protected StateMachineState()
		{
			this.Machine = new BaseStateMachine
			{
				MachineName = this.ToString()
			};
			this.Machine.CurrentStateChanged += this.OnCurrentStateChanged;
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000077 RID: 119 RVA: 0x0000374C File Offset: 0x0000194C
		// (set) Token: 0x06000078 RID: 120 RVA: 0x00003763 File Offset: 0x00001963
		public new bool Started { get; private set; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000079 RID: 121 RVA: 0x0000376C File Offset: 0x0000196C
		public BaseState CurrentState
		{
			get
			{
				return (this.Machine != null) ? this.Machine.CurrentState : null;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600007A RID: 122 RVA: 0x00003798 File Offset: 0x00001998
		// (set) Token: 0x0600007B RID: 123 RVA: 0x000037AF File Offset: 0x000019AF
		protected BaseStateMachine Machine { get; set; }

		// Token: 0x0600007C RID: 124 RVA: 0x000037B8 File Offset: 0x000019B8
		public sealed override string ToString()
		{
			return base.ToString();
		}

		// Token: 0x0600007D RID: 125 RVA: 0x000037D0 File Offset: 0x000019D0
		public override void Start()
		{
			if (!this.Started)
			{
				Tracer<BaseState>.WriteInformation("Started state: {0} ({1})", new object[]
				{
					this.ToString(),
					base.MachineName
				});
				this.Started = true;
				this.Machine.MachineStarted += this.MachineStarted;
				this.Machine.MachineStopped += this.MachineStopped;
				this.Machine.MachineEnded += this.MachineEnded;
				this.Machine.MachineErrored += this.MachineErrored;
				this.Machine.Start();
			}
			else
			{
				Tracer<BaseState>.WriteWarning("Trying to start state {0} which is already started!", new object[]
				{
					this.ToString()
				});
			}
		}

		// Token: 0x0600007E RID: 126 RVA: 0x000038A8 File Offset: 0x00001AA8
		public override void Stop()
		{
			if (this.Started)
			{
				Tracer<BaseState>.WriteInformation("Stopped state: {0} ({1})", new object[]
				{
					this.ToString(),
					base.MachineName
				});
				this.Started = false;
				this.Machine.Stop();
				this.Machine.MachineStarted -= this.MachineStarted;
				this.Machine.MachineStopped -= this.MachineStopped;
				this.Machine.MachineEnded -= this.MachineEnded;
				this.Machine.MachineErrored -= this.MachineErrored;
			}
			else
			{
				Tracer<BaseState>.WriteWarning("Trying to stop state {0} which is already stopped!", new object[]
				{
					this.ToString()
				});
			}
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00003983 File Offset: 0x00001B83
		public void ClearStateMachine()
		{
			this.Machine = new BaseStateMachine();
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00003992 File Offset: 0x00001B92
		protected virtual void OnCurrentStateChanged(BaseState oldValue, BaseState newValue)
		{
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00003998 File Offset: 0x00001B98
		protected virtual void MachineEnded(object sender, TransitionEventArgs args)
		{
			Tracer<StateMachineState>.WriteInformation("Machine Ended {0} ({1})", new object[]
			{
				this.ToString(),
				base.MachineName
			});
			this.RaiseStateFinished(args);
		}

		// Token: 0x06000082 RID: 130 RVA: 0x000039D4 File Offset: 0x00001BD4
		protected virtual void MachineErrored(object sender, BaseStateMachineErrorEventArgs eventArgs)
		{
			Tracer<StateMachineState>.WriteInformation("Machine Errored {0} ({1})", new object[]
			{
				this.ToString(),
				base.MachineName
			});
			this.RaiseStateErrored(eventArgs.Error);
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00003A14 File Offset: 0x00001C14
		protected virtual void MachineStopped(object sender, EventArgs args)
		{
			Tracer<StateMachineState>.WriteInformation("Machine Stopped {0} ({1})", new object[]
			{
				this.ToString(),
				base.MachineName
			});
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00003A48 File Offset: 0x00001C48
		protected virtual void MachineStarted(object sender, EventArgs args)
		{
			Tracer<StateMachineState>.WriteInformation("Machine Started {0} ({1})", new object[]
			{
				this.ToString(),
				base.MachineName
			});
			this.RaiseStateStarted(args);
		}
	}
}
