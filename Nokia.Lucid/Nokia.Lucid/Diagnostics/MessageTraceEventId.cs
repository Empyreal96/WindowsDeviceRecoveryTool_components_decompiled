using System;

namespace Nokia.Lucid.Diagnostics
{
	// Token: 0x02000023 RID: 35
	internal enum MessageTraceEventId
	{
		// Token: 0x04000089 RID: 137
		WindowMessage = 1,
		// Token: 0x0400008A RID: 138
		DeviceNotification,
		// Token: 0x0400008B RID: 139
		ThreadMessage,
		// Token: 0x0400008C RID: 140
		MessageLoopEnter,
		// Token: 0x0400008D RID: 141
		MessageLoopExit,
		// Token: 0x0400008E RID: 142
		MessageDispatch,
		// Token: 0x0400008F RID: 143
		MessageLoopExitRequest,
		// Token: 0x04000090 RID: 144
		MessageWindowCreation,
		// Token: 0x04000091 RID: 145
		MessageWindowProcAttach,
		// Token: 0x04000092 RID: 146
		MessageWindowCloseRequest,
		// Token: 0x04000093 RID: 147
		DeviceNotificationRegistration,
		// Token: 0x04000094 RID: 148
		DeviceNotificationUnregistration,
		// Token: 0x04000095 RID: 149
		DeviceNotificationProcessing,
		// Token: 0x04000096 RID: 150
		MessageWindowStatusChange,
		// Token: 0x04000097 RID: 151
		ThreadExceptionDelegation
	}
}
