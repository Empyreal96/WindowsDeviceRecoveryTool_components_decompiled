using System;

namespace System.Windows.Forms
{
	/// <summary>Indicates current system power status information.</summary>
	// Token: 0x02000311 RID: 785
	public class PowerStatus
	{
		// Token: 0x06002F98 RID: 12184 RVA: 0x000027DB File Offset: 0x000009DB
		internal PowerStatus()
		{
		}

		/// <summary>Gets the current system power status.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.PowerLineStatus" /> values indicating the current system power status.</returns>
		// Token: 0x17000B75 RID: 2933
		// (get) Token: 0x06002F99 RID: 12185 RVA: 0x000DB1F4 File Offset: 0x000D93F4
		public PowerLineStatus PowerLineStatus
		{
			get
			{
				this.UpdateSystemPowerStatus();
				return (PowerLineStatus)this.systemPowerStatus.ACLineStatus;
			}
		}

		/// <summary>Gets the current battery charge status.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.BatteryChargeStatus" /> values indicating the current battery charge level or charging status.</returns>
		// Token: 0x17000B76 RID: 2934
		// (get) Token: 0x06002F9A RID: 12186 RVA: 0x000DB207 File Offset: 0x000D9407
		public BatteryChargeStatus BatteryChargeStatus
		{
			get
			{
				this.UpdateSystemPowerStatus();
				return (BatteryChargeStatus)this.systemPowerStatus.BatteryFlag;
			}
		}

		/// <summary>Gets the reported full charge lifetime of the primary battery power source in seconds.</summary>
		/// <returns>The reported number of seconds of battery life available when the battery is fully charged, or -1 if the battery life is unknown.</returns>
		// Token: 0x17000B77 RID: 2935
		// (get) Token: 0x06002F9B RID: 12187 RVA: 0x000DB21A File Offset: 0x000D941A
		public int BatteryFullLifetime
		{
			get
			{
				this.UpdateSystemPowerStatus();
				return this.systemPowerStatus.BatteryFullLifeTime;
			}
		}

		/// <summary>Gets the approximate amount of full battery charge remaining.</summary>
		/// <returns>The approximate amount, from 0.0 to 1.0, of full battery charge remaining.</returns>
		// Token: 0x17000B78 RID: 2936
		// (get) Token: 0x06002F9C RID: 12188 RVA: 0x000DB230 File Offset: 0x000D9430
		public float BatteryLifePercent
		{
			get
			{
				this.UpdateSystemPowerStatus();
				float num = (float)this.systemPowerStatus.BatteryLifePercent / 100f;
				if (num <= 1f)
				{
					return num;
				}
				return 1f;
			}
		}

		/// <summary>Gets the approximate number of seconds of battery time remaining.</summary>
		/// <returns>The approximate number of seconds of battery life remaining, or –1 if the approximate remaining battery life is unknown.</returns>
		// Token: 0x17000B79 RID: 2937
		// (get) Token: 0x06002F9D RID: 12189 RVA: 0x000DB265 File Offset: 0x000D9465
		public int BatteryLifeRemaining
		{
			get
			{
				this.UpdateSystemPowerStatus();
				return this.systemPowerStatus.BatteryLifeTime;
			}
		}

		// Token: 0x06002F9E RID: 12190 RVA: 0x000DB278 File Offset: 0x000D9478
		private void UpdateSystemPowerStatus()
		{
			UnsafeNativeMethods.GetSystemPowerStatus(ref this.systemPowerStatus);
		}

		// Token: 0x04001D94 RID: 7572
		private NativeMethods.SYSTEM_POWER_STATUS systemPowerStatus;
	}
}
