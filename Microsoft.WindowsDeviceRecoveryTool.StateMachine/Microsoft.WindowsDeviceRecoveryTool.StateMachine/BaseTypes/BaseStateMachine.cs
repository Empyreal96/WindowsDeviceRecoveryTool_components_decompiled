using System;
using System.Collections.Generic;
using Microsoft.WindowsDeviceRecoveryTool.Common.Tracing;
using Microsoft.WindowsDeviceRecoveryTool.StateMachine.DefaultTypes;
using Microsoft.WindowsDeviceRecoveryTool.StateMachine.Exceptions;

namespace Microsoft.WindowsDeviceRecoveryTool.StateMachine.BaseTypes
{
	// Token: 0x02000003 RID: 3
	public class BaseStateMachine
	{
		// Token: 0x06000025 RID: 37 RVA: 0x00002810 File Offset: 0x00000A10
		public BaseStateMachine()
		{
			this.CurrentState = BaseState.NullObject();
			this.states = new List<BaseState>();
			this.machineState = BaseStateMachine.StateMachineState.Stopped;
		}

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06000026 RID: 38 RVA: 0x00002844 File Offset: 0x00000A44
		// (remove) Token: 0x06000027 RID: 39 RVA: 0x00002880 File Offset: 0x00000A80
		public event EventHandler MachineStarted;

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x06000028 RID: 40 RVA: 0x000028BC File Offset: 0x00000ABC
		// (remove) Token: 0x06000029 RID: 41 RVA: 0x000028F8 File Offset: 0x00000AF8
		public event EventHandler<TransitionEventArgs> MachineEnded;

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x0600002A RID: 42 RVA: 0x00002934 File Offset: 0x00000B34
		// (remove) Token: 0x0600002B RID: 43 RVA: 0x00002970 File Offset: 0x00000B70
		public event EventHandler MachineStopped;

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x0600002C RID: 44 RVA: 0x000029AC File Offset: 0x00000BAC
		// (remove) Token: 0x0600002D RID: 45 RVA: 0x000029E8 File Offset: 0x00000BE8
		public event EventHandler<BaseStateMachineErrorEventArgs> MachineErrored;

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x0600002E RID: 46 RVA: 0x00002A24 File Offset: 0x00000C24
		// (remove) Token: 0x0600002F RID: 47 RVA: 0x00002A60 File Offset: 0x00000C60
		public event Action<BaseState, BaseState> CurrentStateChanged;

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000030 RID: 48 RVA: 0x00002A9C File Offset: 0x00000C9C
		// (set) Token: 0x06000031 RID: 49 RVA: 0x00002AB4 File Offset: 0x00000CB4
		public string MachineName
		{
			get
			{
				return this.machineName;
			}
			set
			{
				this.machineName = value.Substring(value.LastIndexOf('.') + 1);
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000032 RID: 50 RVA: 0x00002AD0 File Offset: 0x00000CD0
		// (set) Token: 0x06000033 RID: 51 RVA: 0x00002AE8 File Offset: 0x00000CE8
		public BaseState CurrentState
		{
			get
			{
				return this.currentState;
			}
			private set
			{
				BaseState previousState = this.currentState;
				this.currentState = value;
				this.RaiseCurrentStateChanged(previousState, this.currentState);
			}
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002B14 File Offset: 0x00000D14
		public void AddState(BaseState state)
		{
			if (!this.CheckIsRunning())
			{
				state.MachineName = this.MachineName;
				this.states.Add(state);
			}
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002B48 File Offset: 0x00000D48
		public void SetStartState(BaseState state)
		{
			this.states.Remove(state);
			this.states.Insert(0, state);
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002B68 File Offset: 0x00000D68
		public virtual void Start()
		{
			lock (this.sync)
			{
				if (!this.CheckIsRunning())
				{
					this.RaiseStateMachineEvent(this.MachineStarted);
					this.StartStateMachine();
				}
			}
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002BD0 File Offset: 0x00000DD0
		public virtual void Stop()
		{
			lock (this.sync)
			{
				if (this.CheckIsRunning())
				{
					this.StopStateMachine();
					this.RaiseStateMachineEvent(this.MachineStopped);
				}
			}
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002C38 File Offset: 0x00000E38
		public virtual void NextState()
		{
			this.NextState(TransitionEventArgs.Empty);
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002C48 File Offset: 0x00000E48
		public bool CheckIsRunning()
		{
			return this.machineState == BaseStateMachine.StateMachineState.Running;
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002C64 File Offset: 0x00000E64
		private void RaiseCurrentStateChanged(BaseState previousState, BaseState newState)
		{
			Action<BaseState, BaseState> currentStateChanged = this.CurrentStateChanged;
			if (currentStateChanged != null)
			{
				currentStateChanged(previousState, newState);
			}
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00002C8C File Offset: 0x00000E8C
		private void NextState(TransitionEventArgs eventArgs)
		{
			lock (this.sync)
			{
				if (this.CheckIsRunning())
				{
					this.SetNextState(eventArgs);
				}
			}
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002CE8 File Offset: 0x00000EE8
		private void ErroredState(Error error)
		{
			lock (this.sync)
			{
				if (this.CheckIsRunning())
				{
					this.SetErrorState(error);
				}
			}
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002D44 File Offset: 0x00000F44
		private void StartStateMachine()
		{
			if (this.states.Count == 0)
			{
				UnexpectedErrorException ex = new UnexpectedErrorException("No states to run state machine!");
				Tracer<BaseStateMachine>.WriteError(ex);
				throw ex;
			}
			this.machineState = BaseStateMachine.StateMachineState.Running;
			this.CurrentState = this.states[0];
			this.CurrentState.Finished += this.CurrentStateFinished;
			this.CurrentState.Errored += this.CurrentStateErrored;
			this.CurrentState.Start();
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002DD4 File Offset: 0x00000FD4
		private string StopStateMachine()
		{
			this.machineState = BaseStateMachine.StateMachineState.Stopped;
			string result = string.Empty;
			EndState endState = this.CurrentState as EndState;
			if (endState != null)
			{
				result = endState.Status;
			}
			this.CurrentState.Finished -= this.CurrentStateFinished;
			this.CurrentState.Errored -= this.CurrentStateErrored;
			this.CurrentState.Stop();
			this.CurrentState = BaseState.NullObject();
			return result;
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002E5C File Offset: 0x0000105C
		private void SetNextState(TransitionEventArgs eventArgs)
		{
			if (this.CheckIsRunning())
			{
				BaseState baseState = this.CurrentState.NextState(eventArgs);
				this.CurrentState.Stop();
				this.CurrentState.Finished -= this.CurrentStateFinished;
				this.CurrentState.Errored -= this.CurrentStateErrored;
				this.CurrentState = baseState;
				this.CurrentState.Finished += this.CurrentStateFinished;
				this.CurrentState.Errored += this.CurrentStateErrored;
				try
				{
					this.CurrentState.Start();
				}
				catch (OutOfMemoryException ex)
				{
					Tracer<BaseStateMachine>.WriteError(ex, "Cannot start a state", new object[0]);
					this.CurrentStateErrored(this.CurrentState, new Error(ex));
				}
				catch (Exception ex2)
				{
					Tracer<BaseStateMachine>.WriteError(ex2, "Cannot start a state", new object[0]);
					this.CurrentStateErrored(this.CurrentState, new Error(new InternalException(string.Empty, ex2)));
				}
				if (this.IsCurrentStateEndState())
				{
					string status = this.StopStateMachine();
					EventHandler<TransitionEventArgs> machineEnded = this.MachineEnded;
					if (machineEnded != null)
					{
						machineEnded(this, new TransitionEventArgs(status));
					}
				}
			}
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00002FC4 File Offset: 0x000011C4
		private void SetErrorState(Error error)
		{
			if (this.CheckIsRunning())
			{
				if (this.IsCurrentStateEndErrorState())
				{
					this.StopStateMachine();
					this.RaiseStateMachineErroredEvent(error);
				}
				else
				{
					BaseErrorState baseErrorState = this.CurrentState.NextErrorState(error);
					if (baseErrorState != this.CurrentState)
					{
						this.CurrentState.Stop();
						this.CurrentState.Finished -= this.CurrentStateFinished;
						this.CurrentState.Errored -= this.CurrentStateErrored;
						this.CurrentState = baseErrorState;
						baseErrorState.Finished += this.CurrentStateFinished;
						baseErrorState.Errored += this.CurrentStateErrored;
						baseErrorState.Start(error);
					}
				}
			}
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00003094 File Offset: 0x00001294
		private void CurrentStateFinished(object sender, TransitionEventArgs eventArgs)
		{
			BaseState baseState = sender as BaseState;
			lock (this.sync)
			{
				if (this.CurrentState == baseState)
				{
					this.NextState(eventArgs);
				}
				else
				{
					Tracer<BaseStateMachine>.WriteWarning(string.Concat(new object[]
					{
						"Blocked state change attempt from:",
						this.CurrentState,
						" to:",
						baseState
					}), new object[0]);
				}
			}
		}

		// Token: 0x06000042 RID: 66 RVA: 0x0000313C File Offset: 0x0000133C
		private void CurrentStateErrored(object sender, Error error)
		{
			BaseState baseState = sender as BaseState;
			lock (this.sync)
			{
				if (this.CurrentState == baseState)
				{
					this.ErroredState(error);
				}
				else
				{
					Tracer<BaseStateMachine>.WriteWarning(string.Concat(new object[]
					{
						"Blocked state change attempt from:",
						this.CurrentState,
						" to:",
						baseState
					}), new object[0]);
				}
			}
		}

		// Token: 0x06000043 RID: 67 RVA: 0x000031E4 File Offset: 0x000013E4
		private bool IsCurrentStateEndState()
		{
			return this.CurrentState.DefaultTransition == null && this.CurrentState.ConditionalTransitions.Count == 0;
		}

		// Token: 0x06000044 RID: 68 RVA: 0x0000321C File Offset: 0x0000141C
		private bool IsCurrentStateEndErrorState()
		{
			return this.CurrentState is ErrorEndState;
		}

		// Token: 0x06000045 RID: 69 RVA: 0x0000323C File Offset: 0x0000143C
		private void RaiseStateMachineEvent(EventHandler eventHandler)
		{
			if (eventHandler != null)
			{
				eventHandler(this, EventArgs.Empty);
			}
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00003264 File Offset: 0x00001464
		private void RaiseStateMachineErroredEvent(Error error)
		{
			EventHandler<BaseStateMachineErrorEventArgs> machineErrored = this.MachineErrored;
			if (machineErrored != null)
			{
				machineErrored(this, new BaseStateMachineErrorEventArgs(error));
			}
		}

		// Token: 0x0400000B RID: 11
		private readonly object sync = new object();

		// Token: 0x0400000C RID: 12
		private readonly List<BaseState> states;

		// Token: 0x0400000D RID: 13
		private BaseStateMachine.StateMachineState machineState;

		// Token: 0x0400000E RID: 14
		private BaseState currentState;

		// Token: 0x0400000F RID: 15
		private string machineName;

		// Token: 0x02000004 RID: 4
		private enum StateMachineState
		{
			// Token: 0x04000016 RID: 22
			Running,
			// Token: 0x04000017 RID: 23
			Stopped
		}
	}
}
