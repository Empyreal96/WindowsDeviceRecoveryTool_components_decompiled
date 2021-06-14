using System;

namespace System.Windows.Forms
{
	/// <summary>Defines identifiers that indicate the current battery charge level or charging state information.</summary>
	// Token: 0x0200030F RID: 783
	[Flags]
	public enum BatteryChargeStatus
	{
		/// <summary>Indicates a high level of battery charge.</summary>
		// Token: 0x04001D8B RID: 7563
		High = 1,
		/// <summary>Indicates a low level of battery charge.</summary>
		// Token: 0x04001D8C RID: 7564
		Low = 2,
		/// <summary>Indicates a critically low level of battery charge.</summary>
		// Token: 0x04001D8D RID: 7565
		Critical = 4,
		/// <summary>Indicates a battery is charging.</summary>
		// Token: 0x04001D8E RID: 7566
		Charging = 8,
		/// <summary>Indicates that no battery is present.</summary>
		// Token: 0x04001D8F RID: 7567
		NoSystemBattery = 128,
		/// <summary>Indicates an unknown battery condition.</summary>
		// Token: 0x04001D90 RID: 7568
		Unknown = 255
	}
}
