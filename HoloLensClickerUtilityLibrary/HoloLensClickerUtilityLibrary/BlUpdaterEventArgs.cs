using System;

namespace ClickerUtilityLibrary
{
	// Token: 0x02000003 RID: 3
	public class BlUpdaterEventArgs : EventArgs
	{
		// Token: 0x04000003 RID: 3
		public BlUpdaterEventArgs.EventType Type;

		// Token: 0x04000004 RID: 4
		public object Parameters;

		// Token: 0x0200003E RID: 62
		public enum EventType
		{
			// Token: 0x0400016B RID: 363
			UpdateCompleted,
			// Token: 0x0400016C RID: 364
			UpdateProgress,
			// Token: 0x0400016D RID: 365
			DeviceDisconnected,
			// Token: 0x0400016E RID: 366
			ConnectedToApplication,
			// Token: 0x0400016F RID: 367
			ConnectedToBootLoader,
			// Token: 0x04000170 RID: 368
			ConnectedToBootLoaderUpdater
		}
	}
}
