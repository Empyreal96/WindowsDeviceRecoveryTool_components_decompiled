using System;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace Microsoft.WindowsDeviceRecoveryTool.Common.Helpers
{
	// Token: 0x02000006 RID: 6
	public class IntervalResetAccessTimer
	{
		// Token: 0x0600000F RID: 15 RVA: 0x000022E8 File Offset: 0x000004E8
		public IntervalResetAccessTimer(int intervalMillis, bool isAccessAvailableInitialValue)
		{
			this.intervalMillis = intervalMillis;
			this.isAccessAvailable = isAccessAvailableInitialValue;
			this.modifyIsAccessAvailableValueSemaphore = new SemaphoreSlim(1, 1);
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002318 File Offset: 0x00000518
		public void StartTimer()
		{
			if (this.intervalMillis <= 0)
			{
				this.isAccessAvailable = true;
			}
			else
			{
				this.intervalTimer = new System.Timers.Timer((double)this.intervalMillis);
				this.intervalTimer.Elapsed += delegate(object sender, ElapsedEventArgs args)
				{
					this.isAccessAvailable = true;
				};
				this.intervalTimer.AutoReset = true;
				this.intervalTimer.Enabled = true;
				this.intervalTimer.Start();
			}
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002390 File Offset: 0x00000590
		public void StopTimer()
		{
			if (this.intervalTimer != null)
			{
				this.intervalTimer.Stop();
				this.intervalTimer = null;
			}
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000023C4 File Offset: 0x000005C4
		public bool RunIfAccessAvailable(Action actionToRun)
		{
			bool flag = this.TryAccessSectionAndSet();
			if (flag)
			{
				actionToRun();
			}
			return flag;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002590 File Offset: 0x00000790
		public async Task<bool> RunIfAccessAvailableAsync(Task taskToRun, CancellationToken cancellationToken)
		{
			bool result = await this.TryAccessSectionAndSetAsync(cancellationToken);
			if (result)
			{
				await taskToRun;
			}
			return result;
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000025EC File Offset: 0x000007EC
		public bool TryAccessSectionAndSet()
		{
			bool result;
			if (this.intervalMillis <= 0)
			{
				result = true;
			}
			else
			{
				try
				{
					this.modifyIsAccessAvailableValueSemaphore.Wait();
					bool flag = this.isAccessAvailable;
					if (flag)
					{
						this.isAccessAvailable = false;
					}
					result = flag;
				}
				finally
				{
					this.modifyIsAccessAvailableValueSemaphore.Release();
				}
			}
			return result;
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000027F8 File Offset: 0x000009F8
		public async Task<bool> TryAccessSectionAndSetAsync(CancellationToken cancellationToken)
		{
			bool result;
			if (this.intervalMillis <= 0)
			{
				result = true;
			}
			else
			{
				try
				{
					await this.modifyIsAccessAvailableValueSemaphore.WaitAsync(cancellationToken);
					bool value = this.isAccessAvailable;
					if (value)
					{
						this.isAccessAvailable = false;
					}
					result = value;
				}
				finally
				{
					this.modifyIsAccessAvailableValueSemaphore.Release();
				}
			}
			return result;
		}

		// Token: 0x04000006 RID: 6
		private readonly int intervalMillis;

		// Token: 0x04000007 RID: 7
		private bool isAccessAvailable;

		// Token: 0x04000008 RID: 8
		private System.Timers.Timer intervalTimer;

		// Token: 0x04000009 RID: 9
		private readonly SemaphoreSlim modifyIsAccessAvailableValueSemaphore;
	}
}
