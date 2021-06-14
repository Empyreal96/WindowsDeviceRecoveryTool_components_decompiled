using System;

namespace ClickerUtilityLibrary
{
	// Token: 0x02000005 RID: 5
	public class FwUpdaterEventArgs : EventArgs
	{
		// Token: 0x0400001E RID: 30
		public FwUpdaterEventArgs.EventType Type;

		// Token: 0x0400001F RID: 31
		public object Parameters;

		// Token: 0x02000041 RID: 65
		public enum EventType
		{
			// Token: 0x0400017C RID: 380
			UpdateCompleted,
			// Token: 0x0400017D RID: 381
			UpdateProgress,
			// Token: 0x0400017E RID: 382
			DeviceDisconnected,
			// Token: 0x0400017F RID: 383
			ConnectedToApplication,
			// Token: 0x04000180 RID: 384
			ConnectedToBootLoader
		}
	}
}
