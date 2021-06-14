using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.WindowsDeviceRecoveryTool.Common.Tracing;
using Microsoft.WindowsDeviceRecoveryTool.StateMachine.DefaultTypes;

namespace Microsoft.WindowsDeviceRecoveryTool.StateMachine.BaseTypes
{
	// Token: 0x02000002 RID: 2
	public class BaseState
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		protected BaseState()
		{
			this.ConditionalTransitions = new Collection<BaseTransition>();
			this.ErrorTransitions = new Dictionary<Type, ErrorTransition>();
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000002 RID: 2 RVA: 0x00002074 File Offset: 0x00000274
		// (remove) Token: 0x06000003 RID: 3 RVA: 0x000020B0 File Offset: 0x000002B0
		public event EventHandler<EventArgs> StateStarted;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000004 RID: 4 RVA: 0x000020EC File Offset: 0x000002EC
		// (remove) Token: 0x06000005 RID: 5 RVA: 0x00002128 File Offset: 0x00000328
		public event EventHandler<TransitionEventArgs> Finished;

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000006 RID: 6 RVA: 0x00002164 File Offset: 0x00000364
		// (remove) Token: 0x06000007 RID: 7 RVA: 0x000021A0 File Offset: 0x000003A0
		public event EventHandler<Error> Errored;

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x06000008 RID: 8 RVA: 0x000021DC File Offset: 0x000003DC
		// (remove) Token: 0x06000009 RID: 9 RVA: 0x00002218 File Offset: 0x00000418
		public event EventHandler Closing;

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600000A RID: 10 RVA: 0x00002254 File Offset: 0x00000454
		// (set) Token: 0x0600000B RID: 11 RVA: 0x0000226B File Offset: 0x0000046B
		public DefaultTransition DefaultTransition { get; set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600000C RID: 12 RVA: 0x00002274 File Offset: 0x00000474
		// (set) Token: 0x0600000D RID: 13 RVA: 0x0000228B File Offset: 0x0000048B
		public Collection<BaseTransition> ConditionalTransitions { get; private set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000E RID: 14 RVA: 0x00002294 File Offset: 0x00000494
		// (set) Token: 0x0600000F RID: 15 RVA: 0x000022AB File Offset: 0x000004AB
		public ErrorTransition DefaultErrorTransition { get; set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000010 RID: 16 RVA: 0x000022B4 File Offset: 0x000004B4
		// (set) Token: 0x06000011 RID: 17 RVA: 0x000022CB File Offset: 0x000004CB
		public Dictionary<Type, ErrorTransition> ErrorTransitions { get; private set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000012 RID: 18 RVA: 0x000022D4 File Offset: 0x000004D4
		// (set) Token: 0x06000013 RID: 19 RVA: 0x000022EB File Offset: 0x000004EB
		public bool Started { get; private set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000014 RID: 20 RVA: 0x000022F4 File Offset: 0x000004F4
		// (set) Token: 0x06000015 RID: 21 RVA: 0x0000230B File Offset: 0x0000050B
		public string MachineName { get; set; }

		// Token: 0x06000016 RID: 22 RVA: 0x00002314 File Offset: 0x00000514
		public static BaseState NullObject()
		{
			return new BaseState();
		}

		// Token: 0x06000017 RID: 23 RVA: 0x0000232B File Offset: 0x0000052B
		public void AddConditionalTransition(BaseTransition transition)
		{
			this.ConditionalTransitions.Add(transition);
		}

		// Token: 0x06000018 RID: 24 RVA: 0x0000233B File Offset: 0x0000053B
		public void AddErrorTransition(ErrorTransition transition, Exception exception)
		{
			this.ErrorTransitions.Add(exception.GetType(), transition);
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002354 File Offset: 0x00000554
		public virtual void Start()
		{
			if (!this.Started)
			{
				Tracer<BaseState>.WriteInformation(string.Format("Started state: {0} ({1})", this.ToString(), this.MachineName));
				this.Started = true;
				this.RaiseStateStarted(EventArgs.Empty);
			}
			else
			{
				Tracer<BaseState>.WriteWarning("Trying to start state {0} ({1}) which is already started!", new object[]
				{
					this.ToString(),
					this.MachineName
				});
			}
		}

		// Token: 0x0600001A RID: 26 RVA: 0x000023C8 File Offset: 0x000005C8
		public virtual void Stop()
		{
			if (this.Started)
			{
				Tracer<BaseState>.WriteInformation(string.Format("Stopped state: {0} ({1})", this.ToString(), this.MachineName));
				this.Started = false;
			}
			else
			{
				Tracer<BaseState>.WriteWarning("Trying to stop state {0} ({1}) which is already stopped!", new object[]
				{
					this.ToString(),
					this.MachineName
				});
			}
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002434 File Offset: 0x00000634
		public virtual BaseState NextState(TransitionEventArgs eventArgs)
		{
			BaseState baseState = this;
			BaseTransition baseTransition = this.DefaultTransition;
			Tracer<BaseState>.WriteInformation(string.Format("Getting Next state of {0} ({1})", this.ToString(), this.MachineName));
			try
			{
				foreach (BaseTransition baseTransition2 in this.ConditionalTransitions)
				{
					if (baseTransition2.ConditionsAreMet(this, eventArgs))
					{
						baseTransition = baseTransition2;
						Tracer<BaseState>.WriteInformation("Conditions are met for {0}", new object[]
						{
							baseTransition2.ToString()
						});
						break;
					}
				}
			}
			catch (Exception ex)
			{
				Tracer<BaseState>.WriteError(ex, "Checking transitions is failed: unexpected error", new object[0]);
				return this.HandleTransitionException(baseState, ex);
			}
			if (baseTransition != null)
			{
				if (baseTransition == this.DefaultTransition)
				{
					Tracer<BaseState>.WriteInformation("Selecting Default transition {0}", new object[]
					{
						baseTransition.Next.ToString()
					});
				}
				Tracer<BaseState>.WriteInformation(string.Format("Next state of {0} is {1}", this.ToString(), baseTransition.Next));
				baseState = baseTransition.Next;
			}
			return baseState;
		}

		// Token: 0x0600001C RID: 28 RVA: 0x0000258C File Offset: 0x0000078C
		public void Finish(string status)
		{
			this.RaiseStateFinished(string.IsNullOrEmpty(status) ? TransitionEventArgs.Empty : new TransitionEventArgs(status));
		}

		// Token: 0x0600001D RID: 29 RVA: 0x000025AC File Offset: 0x000007AC
		public void Error(Exception exception)
		{
			this.RaiseStateErrored(new Error(exception));
		}

		// Token: 0x0600001E RID: 30 RVA: 0x000025BC File Offset: 0x000007BC
		private BaseState HandleTransitionException(BaseState state, Exception exception)
		{
			Error error = new Error(exception);
			BaseErrorState baseErrorState = this.NextErrorState(error);
			if (baseErrorState != null)
			{
				baseErrorState.Start(error);
				state = baseErrorState;
			}
			return state;
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000025F4 File Offset: 0x000007F4
		public virtual BaseErrorState NextErrorState(Error error)
		{
			Tracer<BaseState>.WriteInformation(string.Format("Getting Next Error state of {0}, error code: {1}", this.ToString(), error.Message));
			BaseErrorState next;
			if (this.ErrorTransitions.ContainsKey(error.ExceptionType))
			{
				next = this.ErrorTransitions[error.ExceptionType].Next;
			}
			else
			{
				if (this.DefaultErrorTransition == null)
				{
					throw new InvalidOperationException("There is no error transition for code: " + error.ExceptionType);
				}
				next = this.DefaultErrorTransition.Next;
				Tracer<BaseState>.WriteInformation("Selecting Default error state {0}", new object[]
				{
					next.ToString()
				});
			}
			if (next != null)
			{
				Tracer<BaseState>.WriteInformation(string.Format("Next Error state of {0} is {1}", this.ToString(), next));
			}
			return next;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x000026C8 File Offset: 0x000008C8
		protected virtual void RaiseStateStarted(EventArgs eventArgs)
		{
			EventHandler<EventArgs> stateStarted = this.StateStarted;
			if (stateStarted != null)
			{
				stateStarted(this, eventArgs);
			}
		}

		// Token: 0x06000021 RID: 33 RVA: 0x000026F0 File Offset: 0x000008F0
		protected virtual void RaiseStateFinished(TransitionEventArgs eventArgs)
		{
			EventHandler<TransitionEventArgs> finished = this.Finished;
			if (finished != null)
			{
				finished(this, eventArgs);
			}
			else
			{
				Tracer<BaseState>.WriteWarning("Finishing state without handler {0} ({1}), status: {2}", new object[]
				{
					this.ToString(),
					this.MachineName,
					(eventArgs == null || string.IsNullOrEmpty(eventArgs.Status)) ? "empty" : eventArgs.Status
				});
			}
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002764 File Offset: 0x00000964
		protected virtual void RaiseStateErrored(Error error)
		{
			EventHandler<Error> errored = this.Errored;
			if (errored != null)
			{
				errored(this, error);
			}
			else
			{
				Tracer<BaseState>.WriteWarning("Error in state without handler {0} ({1})", new object[]
				{
					this.ToString(),
					this.MachineName
				});
			}
		}

		// Token: 0x06000023 RID: 35 RVA: 0x000027B8 File Offset: 0x000009B8
		public override string ToString()
		{
			return base.ToString().Substring(base.ToString().LastIndexOf('.') + 1);
		}

		// Token: 0x06000024 RID: 36 RVA: 0x000027E4 File Offset: 0x000009E4
		protected void SendClosingEvent()
		{
			EventHandler closing = this.Closing;
			if (closing != null)
			{
				closing(this, EventArgs.Empty);
			}
		}
	}
}
