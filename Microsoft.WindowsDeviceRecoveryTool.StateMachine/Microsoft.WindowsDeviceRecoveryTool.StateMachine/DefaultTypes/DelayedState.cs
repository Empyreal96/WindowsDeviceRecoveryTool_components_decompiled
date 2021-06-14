using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using Microsoft.WindowsDeviceRecoveryTool.Common.Tracing;
using Microsoft.WindowsDeviceRecoveryTool.StateMachine.BaseTypes;

namespace Microsoft.WindowsDeviceRecoveryTool.StateMachine.DefaultTypes
{
	// Token: 0x0200000B RID: 11
	public class DelayedState : BaseState
	{
		// Token: 0x0600005C RID: 92 RVA: 0x00003413 File Offset: 0x00001613
		public DelayedState(int minimumStateDuration)
		{
			this.minimumStateDuration = ((minimumStateDuration >= 0) ? minimumStateDuration : 0);
			this.stopwatch = new Stopwatch();
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00003438 File Offset: 0x00001638
		public override void Start()
		{
			base.Start();
			this.error = null;
			this.stopwatch.Restart();
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00003458 File Offset: 0x00001658
		protected override void RaiseStateFinished(TransitionEventArgs eventArgs)
		{
			this.stopwatch.Stop();
			this.transitionEventArgs = eventArgs;
			if (this.stopwatch.ElapsedMilliseconds < (long)this.minimumStateDuration)
			{
				this.ExtendStateVisibility();
			}
			else
			{
				base.RaiseStateFinished(this.transitionEventArgs);
			}
		}

		// Token: 0x0600005F RID: 95 RVA: 0x000034B0 File Offset: 0x000016B0
		protected override void RaiseStateErrored(Error e)
		{
			this.stopwatch.Stop();
			this.error = e;
			if (this.stopwatch.ElapsedMilliseconds < (long)this.minimumStateDuration)
			{
				this.ExtendStateVisibility();
			}
			else
			{
				base.RaiseStateErrored(this.error);
			}
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00003508 File Offset: 0x00001708
		private void ExtendStateVisibility()
		{
			BackgroundWorker backgroundWorker = new BackgroundWorker();
			backgroundWorker.DoWork += this.ExtendStateVisibilityDoWork;
			backgroundWorker.RunWorkerCompleted += this.ExtendStateVisibilityCompleted;
			backgroundWorker.RunWorkerAsync();
		}

		// Token: 0x06000061 RID: 97 RVA: 0x0000354C File Offset: 0x0000174C
		private void ExtendStateVisibilityDoWork(object sender, DoWorkEventArgs e)
		{
			int millisecondsTimeout = this.minimumStateDuration - (int)this.stopwatch.ElapsedMilliseconds;
			Thread.Sleep(millisecondsTimeout);
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00003578 File Offset: 0x00001778
		private void ExtendStateVisibilityCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			if (e.Error != null)
			{
				Tracer<DelayedState>.WriteError("Exception while waiting for delayed state to end!", new object[0]);
				Tracer<DelayedState>.WriteError(e.Error, e.Error.Message, new object[0]);
			}
			if (this.error != null)
			{
				base.RaiseStateErrored(this.error);
			}
			else
			{
				base.RaiseStateFinished(this.transitionEventArgs);
			}
		}

		// Token: 0x0400001D RID: 29
		private readonly int minimumStateDuration;

		// Token: 0x0400001E RID: 30
		private readonly Stopwatch stopwatch;

		// Token: 0x0400001F RID: 31
		private Error error;

		// Token: 0x04000020 RID: 32
		private TransitionEventArgs transitionEventArgs;
	}
}
