using System;
using System.Diagnostics;
using System.Globalization;
using Nokia.Lucid.DeviceDetection.Primitives;

namespace Nokia.Lucid.Diagnostics
{
	// Token: 0x02000024 RID: 36
	internal sealed class MessageTraceSource : TraceSource
	{
		// Token: 0x06000103 RID: 259 RVA: 0x0000A406 File Offset: 0x00008606
		private MessageTraceSource() : base("Nokia.Lucid.Messages")
		{
		}

		// Token: 0x06000104 RID: 260 RVA: 0x0000A413 File Offset: 0x00008613
		public void MessageLoopEnter_StartStop()
		{
			this.TraceEvent(TraceEventType.Start, MessageTraceEventId.MessageLoopEnter, "Entering message loop.");
			this.TraceEvent(TraceEventType.Stop, MessageTraceEventId.MessageLoopEnter, "Entered message loop.");
		}

		// Token: 0x06000105 RID: 261 RVA: 0x0000A438 File Offset: 0x00008638
		public void MessageDispatch_Start(IntPtr windowHandle, int message, IntPtr wParam, IntPtr lParam)
		{
			string messageName = KnownNames.GetMessageName(message);
			string messageText = string.Format(CultureInfo.InvariantCulture, "Dispatching message to window.\r\nHWND: 0x{0:x4}\r\nMessage: 0x{1:x4} ({2})\r\nWPARAM: 0x{3:x4}\r\nLPARAM: 0x{4:x4}", new object[]
			{
				windowHandle.ToInt64(),
				message,
				messageName,
				wParam.ToInt64(),
				lParam.ToInt64()
			});
			this.TraceEvent(TraceEventType.Start, MessageTraceEventId.MessageDispatch, messageText);
		}

		// Token: 0x06000106 RID: 262 RVA: 0x0000A4AC File Offset: 0x000086AC
		public void MessageDispatch_Stop(IntPtr windowHandle, int message, IntPtr wParam, IntPtr lParam)
		{
			string messageName = KnownNames.GetMessageName(message);
			string messageText = string.Format(CultureInfo.InvariantCulture, "Dispatched message to window.\r\nHWND: 0x{0:x4}\r\nMessage: 0x{1:x4} ({2})\r\nWPARAM: 0x{3:x4}\r\nLPARAM: 0x{4:x4}", new object[]
			{
				windowHandle.ToInt64(),
				message,
				messageName,
				wParam.ToInt64(),
				lParam.ToInt64()
			});
			this.TraceEvent(TraceEventType.Stop, MessageTraceEventId.MessageDispatch, messageText);
		}

		// Token: 0x06000107 RID: 263 RVA: 0x0000A520 File Offset: 0x00008720
		public void MessageDispatch_Error(IntPtr windowHandle, int message, IntPtr wParam, IntPtr lParam, Exception exception)
		{
			string messageName = KnownNames.GetMessageName(message);
			string messageText = string.Format(CultureInfo.InvariantCulture, "Error while dispatching message to window, suppressing exception.\r\nHWND: 0x{0:x4}\r\nMessage: 0x{1:x4} ({2})\r\nWPARAM: 0x{3:x4}\r\nLPARAM: 0x{4:x4}\r\nException:\r\n{5}", new object[]
			{
				windowHandle.ToInt64(),
				message,
				messageName,
				wParam.ToInt64(),
				lParam.ToInt64(),
				exception
			});
			this.TraceEvent(TraceEventType.Error, MessageTraceEventId.MessageDispatch, messageText);
		}

		// Token: 0x06000108 RID: 264 RVA: 0x0000A594 File Offset: 0x00008794
		public void MessageLoopExit_Start()
		{
			this.TraceEvent(TraceEventType.Start, MessageTraceEventId.MessageLoopExit, "Exiting message loop.");
		}

		// Token: 0x06000109 RID: 265 RVA: 0x0000A5A7 File Offset: 0x000087A7
		public void MessageLoopExit_Stop()
		{
			this.TraceEvent(TraceEventType.Stop, MessageTraceEventId.MessageLoopExit, "Exited message loop.");
		}

		// Token: 0x0600010A RID: 266 RVA: 0x0000A5BC File Offset: 0x000087BC
		public void ThreadMessage(int message, IntPtr wParam, IntPtr lParam)
		{
			string messageName = KnownNames.GetMessageName(message);
			string message2 = string.Format(CultureInfo.InvariantCulture, "Received thread message, discarding.\r\nMessage: 0x{0:x4} ({1})\r\nWPARAM: 0x{2:x4}\r\nLPARAM: 0x{3:x4}", new object[]
			{
				message,
				messageName,
				wParam.ToInt64(),
				lParam.ToInt64()
			});
			base.TraceEvent(TraceEventType.Warning, 3, message2);
		}

		// Token: 0x0600010B RID: 267 RVA: 0x0000A61C File Offset: 0x0000881C
		public void WindowMessage(IntPtr windowHandle, int message, IntPtr wParam, IntPtr lParam)
		{
			string messageName = KnownNames.GetMessageName(message);
			string message2 = string.Format(CultureInfo.InvariantCulture, "Received window message.\r\nHWND: 0x{0:x4}\r\nMessage: 0x{1:x4} ({2})\r\nWPARAM: 0x{3:x4}\r\nLPARAM: 0x{4:x4}", new object[]
			{
				windowHandle.ToInt64(),
				message,
				messageName,
				wParam.ToInt64(),
				lParam.ToInt64()
			});
			base.TraceEvent(TraceEventType.Verbose, 1, message2);
		}

		// Token: 0x0600010C RID: 268 RVA: 0x0000A68C File Offset: 0x0000888C
		public void DeviceNotification(IntPtr windowHandle, int eventType, int deviceType)
		{
			string str;
			string text = KnownNames.TryGetEventTypeName(eventType, out str) ? (" (" + str + ")") : string.Empty;
			string str2;
			string text2 = KnownNames.TryGetDeviceTypeName(deviceType, out str2) ? (" (" + str2 + ")") : string.Empty;
			string messageText = string.Format(CultureInfo.InvariantCulture, "Received device notification.\r\nHWND: 0x{0:x4}\r\nEvent: 0x{1:x4}{2}\r\nHeader: 0x{3:x4}{4}", new object[]
			{
				windowHandle.ToInt64(),
				eventType,
				text,
				deviceType,
				text2
			});
			this.TraceEvent(TraceEventType.Verbose, MessageTraceEventId.DeviceNotification, messageText);
		}

		// Token: 0x0600010D RID: 269 RVA: 0x0000A734 File Offset: 0x00008934
		public void MessageWindowCreation_Start(string windowClass)
		{
			string messageText = string.Format(CultureInfo.InvariantCulture, "Creating message window.\r\nClass: {0}", new object[]
			{
				windowClass
			});
			this.TraceEvent(TraceEventType.Start, MessageTraceEventId.MessageWindowCreation, messageText);
		}

		// Token: 0x0600010E RID: 270 RVA: 0x0000A76C File Offset: 0x0000896C
		public void MessageWindowCreation_Stop(string windowClass, IntPtr windowHandle)
		{
			string messageText = string.Format(CultureInfo.InvariantCulture, "Created message window and acquired HWND handle.\r\nClass: {0}\r\nHWND: 0x{1:x4}", new object[]
			{
				windowClass,
				windowHandle.ToInt64()
			});
			this.TraceEvent(TraceEventType.Stop, MessageTraceEventId.MessageWindowCreation, messageText);
		}

		// Token: 0x0600010F RID: 271 RVA: 0x0000A7B4 File Offset: 0x000089B4
		public void MessageWindowCreation_Error(string windowClass, int errorCode, string errorMessage)
		{
			string messageText = string.Format(CultureInfo.InvariantCulture, "Could not create message window.\r\nClass: {0}\r\nError: 0x{1:x4} ({2})", new object[]
			{
				windowClass,
				errorCode,
				errorMessage
			});
			this.TraceEvent(TraceEventType.Error, MessageTraceEventId.MessageWindowCreation, messageText);
		}

		// Token: 0x06000110 RID: 272 RVA: 0x0000A7F4 File Offset: 0x000089F4
		public void MessageWindowProcAttach_Start(IntPtr windowHandle)
		{
			string messageText = string.Format(CultureInfo.InvariantCulture, "Attaching window proc to message window.\r\nHWND: 0x{0:x4}", new object[]
			{
				windowHandle.ToInt64()
			});
			this.TraceEvent(TraceEventType.Start, MessageTraceEventId.MessageWindowProcAttach, messageText);
		}

		// Token: 0x06000111 RID: 273 RVA: 0x0000A838 File Offset: 0x00008A38
		public void MessageWindowProcAttach_Stop(IntPtr windowHandle)
		{
			string messageText = string.Format(CultureInfo.InvariantCulture, "Attached window proc to message window.\r\nHWND: 0x{0:x4}", new object[]
			{
				windowHandle.ToInt64()
			});
			this.TraceEvent(TraceEventType.Stop, MessageTraceEventId.MessageWindowProcAttach, messageText);
		}

		// Token: 0x06000112 RID: 274 RVA: 0x0000A87C File Offset: 0x00008A7C
		public void MessageWindowProcAttach_Error(IntPtr windowHandle, int errorCode, string errorMessage)
		{
			string messageText = string.Format(CultureInfo.InvariantCulture, "Could not attach window proc to message window.\r\nHWND: {0}\r\nError: 0x{1:x4} ({2})", new object[]
			{
				windowHandle,
				errorCode,
				errorMessage
			});
			this.TraceEvent(TraceEventType.Error, MessageTraceEventId.MessageWindowProcAttach, messageText);
		}

		// Token: 0x06000113 RID: 275 RVA: 0x0000A8C1 File Offset: 0x00008AC1
		public void MessageLoopExitRequest_Start()
		{
			this.TraceEvent(TraceEventType.Start, MessageTraceEventId.MessageLoopExitRequest, "Posting WM_QUIT to the message queue.");
		}

		// Token: 0x06000114 RID: 276 RVA: 0x0000A8D4 File Offset: 0x00008AD4
		public void MessageLoopExitRequest_Stop()
		{
			this.TraceEvent(TraceEventType.Stop, MessageTraceEventId.MessageLoopExitRequest, "Posted WM_QUIT to the message queue.");
		}

		// Token: 0x06000115 RID: 277 RVA: 0x0000A8E8 File Offset: 0x00008AE8
		public void MessageWindowCloseRequest_Start(IntPtr windowHandle)
		{
			string messageText = string.Format(CultureInfo.InvariantCulture, "Posting WM_CLOSE to message queue.\r\nHWND: 0x{0:x4}", new object[]
			{
				windowHandle.ToInt64()
			});
			this.TraceEvent(TraceEventType.Start, MessageTraceEventId.MessageWindowCloseRequest, messageText);
		}

		// Token: 0x06000116 RID: 278 RVA: 0x0000A92C File Offset: 0x00008B2C
		public void MessageWindowCloseRequest_Stop(IntPtr windowHandle)
		{
			string messageText = string.Format(CultureInfo.InvariantCulture, "Posted WM_CLOSE to message queue.\r\nHWND: 0x{0:x4}", new object[]
			{
				windowHandle.ToInt64()
			});
			this.TraceEvent(TraceEventType.Stop, MessageTraceEventId.MessageWindowCloseRequest, messageText);
		}

		// Token: 0x06000117 RID: 279 RVA: 0x0000A970 File Offset: 0x00008B70
		public void MessageWindowCloseRequest_Error(IntPtr windowHandle, int errorCode, string errorText)
		{
			string messageText = string.Format(CultureInfo.InvariantCulture, "Could not post WM_CLOSE to message queue.\r\nHWND: 0x{0:x4}\r\nError: 0x{1:x4} ({2})", new object[]
			{
				windowHandle.ToInt64(),
				errorCode,
				errorText
			});
			this.TraceEvent(TraceEventType.Error, MessageTraceEventId.MessageWindowCloseRequest, messageText);
		}

		// Token: 0x06000118 RID: 280 RVA: 0x0000A9BC File Offset: 0x00008BBC
		public void DeviceNotificationRegistration_Start(IntPtr windowHandle, Guid interfaceClass)
		{
			string str;
			string text = KnownNames.TryGetInterfaceClassName(interfaceClass, out str) ? (" (" + str + ")") : string.Empty;
			string messageText = string.Format(CultureInfo.InvariantCulture, "Registering device notification.\r\nHWND: 0x{0:x4}\r\nClass: {1}{2}", new object[]
			{
				windowHandle.ToInt64(),
				interfaceClass,
				text
			});
			this.TraceEvent(TraceEventType.Start, MessageTraceEventId.DeviceNotificationRegistration, messageText);
		}

		// Token: 0x06000119 RID: 281 RVA: 0x0000AA30 File Offset: 0x00008C30
		public void DeviceNotificationRegistration_Stop(IntPtr windowHandle, Guid interfaceClass, IntPtr devNotifyHandle)
		{
			string str;
			string text = KnownNames.TryGetInterfaceClassName(interfaceClass, out str) ? (" (" + str + ")") : string.Empty;
			string messageText = string.Format(CultureInfo.InvariantCulture, "Registered device notification and acquired HDEVNOTIFY handle.\r\nHWND: 0x{0:x4}\r\nClass: {1}{2}\r\nHDEVNOTIFY: 0x{3:x4}", new object[]
			{
				windowHandle.ToInt64(),
				interfaceClass,
				text,
				devNotifyHandle.ToInt64()
			});
			this.TraceEvent(TraceEventType.Stop, MessageTraceEventId.DeviceNotificationRegistration, messageText);
		}

		// Token: 0x0600011A RID: 282 RVA: 0x0000AAB0 File Offset: 0x00008CB0
		public void DeviceNotificationRegistration_Error(IntPtr windowHandle, Guid interfaceClass, int errorCode, string errorMessage)
		{
			string str;
			string text = KnownNames.TryGetInterfaceClassName(interfaceClass, out str) ? (" (" + str + ")") : string.Empty;
			string messageText = string.Format(CultureInfo.InvariantCulture, "Could not register device notification, deferring exception.\r\nHWND: 0x{0:x4}\r\nClass: {1}{2}\r\nError: 0x{3:x4} ({4})", new object[]
			{
				windowHandle.ToInt64(),
				interfaceClass,
				text,
				errorCode,
				errorMessage
			});
			this.TraceEvent(TraceEventType.Error, MessageTraceEventId.DeviceNotificationRegistration, messageText);
		}

		// Token: 0x0600011B RID: 283 RVA: 0x0000AB2C File Offset: 0x00008D2C
		public void DeviceNotificationUnregistration_Start(IntPtr windowHandle, IntPtr devNotifyHandle)
		{
			string messageText = string.Format(CultureInfo.InvariantCulture, "Unregistering device notification.\r\nHWND: 0x{0:x4}\r\nHDEVNOTIFY: 0x{1:x4}", new object[]
			{
				windowHandle.ToInt64(),
				devNotifyHandle.ToInt64()
			});
			this.TraceEvent(TraceEventType.Start, MessageTraceEventId.DeviceNotificationUnregistration, messageText);
		}

		// Token: 0x0600011C RID: 284 RVA: 0x0000AB80 File Offset: 0x00008D80
		public void DeviceNotificationUnregistration_Stop(IntPtr windowHandle, IntPtr devNotifyHandle)
		{
			string messageText = string.Format(CultureInfo.InvariantCulture, "Unregistered device notification.\r\nHWND: 0x{0:x4}\r\nHDEVNOTIFY: 0x{1:x4}", new object[]
			{
				windowHandle.ToInt64(),
				devNotifyHandle.ToInt64()
			});
			this.TraceEvent(TraceEventType.Stop, MessageTraceEventId.DeviceNotificationUnregistration, messageText);
		}

		// Token: 0x0600011D RID: 285 RVA: 0x0000ABD4 File Offset: 0x00008DD4
		public void DeviceNotificationUnregistration_Error(IntPtr windowHandle, IntPtr devNotifyHandle, int errorCode, string errorMessage)
		{
			string messageText = string.Format(CultureInfo.InvariantCulture, "Could not unregister device notification, deferring exception.\r\nHWND: 0x{0:x4}\r\nHDEVNOTIFY: 0x{1:x4}\r\nError: 0x{2:x4} ({3})", new object[]
			{
				windowHandle.ToInt64(),
				devNotifyHandle.ToInt64(),
				errorCode,
				errorMessage
			});
			this.TraceEvent(TraceEventType.Error, MessageTraceEventId.DeviceNotificationUnregistration, messageText);
		}

		// Token: 0x0600011E RID: 286 RVA: 0x0000AC30 File Offset: 0x00008E30
		public void DeviceNotificationProcessing_Start(IntPtr windowHandle, string devicePath, Guid interfaceClass, int eventType)
		{
			string str;
			string text = KnownNames.TryGetInterfaceClassName(interfaceClass, out str) ? (" (" + str + ")") : string.Empty;
			string str2;
			string text2 = KnownNames.TryGetEventTypeName(eventType, out str2) ? (" (" + str2 + ")") : string.Empty;
			string messageText = string.Format(CultureInfo.InvariantCulture, "Processing device notification.\r\nHWND: 0x{0:x4}\r\nPath: {1}\r\nClass: {2}{3}\r\nEvent: 0x{4:x4}{5}", new object[]
			{
				windowHandle.ToInt64(),
				devicePath,
				interfaceClass,
				text,
				eventType,
				text2
			});
			this.TraceEvent(TraceEventType.Start, MessageTraceEventId.DeviceNotificationProcessing, messageText);
		}

		// Token: 0x0600011F RID: 287 RVA: 0x0000ACE0 File Offset: 0x00008EE0
		public void DeviceNotificationProcessing_Stop(IntPtr windowHandle, string devicePath, Guid interfaceClass, int eventType)
		{
			string str;
			string text = KnownNames.TryGetInterfaceClassName(interfaceClass, out str) ? (" (" + str + ")") : string.Empty;
			string str2;
			string text2 = KnownNames.TryGetEventTypeName(eventType, out str2) ? (" (" + str2 + ")") : string.Empty;
			string messageText = string.Format(CultureInfo.InvariantCulture, "Processed device notification.\r\nHWND: 0x{0:x4}\r\nPath: {1}\r\nClass: {2}{3}\r\nEvent: 0x{4:x4}{5}", new object[]
			{
				windowHandle.ToInt64(),
				devicePath,
				interfaceClass,
				text,
				eventType,
				text2
			});
			this.TraceEvent(TraceEventType.Stop, MessageTraceEventId.DeviceNotificationProcessing, messageText);
		}

		// Token: 0x06000120 RID: 288 RVA: 0x0000AD90 File Offset: 0x00008F90
		public void DeviceNotificationProcessing_Error(IntPtr windowHandle, string devicePath, Guid interfaceClass, int eventType, Exception exception)
		{
			string str;
			string text = KnownNames.TryGetInterfaceClassName(interfaceClass, out str) ? (" (" + str + ")") : string.Empty;
			string str2;
			string text2 = KnownNames.TryGetEventTypeName(eventType, out str2) ? (" (" + str2 + ")") : string.Empty;
			string messageText = string.Format(CultureInfo.InvariantCulture, "Error while processing device notification.\r\nHWND: 0x{0:x4}\r\nPath: {1}\r\nClass: {2}{3}\r\nEvent: 0x{4:x4}{5}\r\nException:\r\n{6}", new object[]
			{
				windowHandle.ToInt64(),
				devicePath,
				interfaceClass,
				text,
				eventType,
				text2,
				exception
			});
			this.TraceEvent(TraceEventType.Error, MessageTraceEventId.DeviceNotificationProcessing, messageText);
		}

		// Token: 0x06000121 RID: 289 RVA: 0x0000AE44 File Offset: 0x00009044
		public void ThreadExceptionDelegation_Start(IntPtr windowHandle, Exception threadException)
		{
			string messageText = string.Format(CultureInfo.InvariantCulture, "Delegating thread exception to thread exception handler.\r\nHWND: 0x{0:x4}\r\nThread exception:\r\n{1}", new object[]
			{
				windowHandle.ToInt64(),
				threadException
			});
			this.TraceEvent(TraceEventType.Start, MessageTraceEventId.ThreadExceptionDelegation, messageText);
		}

		// Token: 0x06000122 RID: 290 RVA: 0x0000AE8C File Offset: 0x0000908C
		public void ThreadExceptionDelegation_Error(IntPtr windowHandle, Exception threadException, Exception exception)
		{
			string messageText = string.Format(CultureInfo.InvariantCulture, "Error while delegating thread exception to thread exception handler, deferring thread exception.\r\nHWND: 0x{0:x4}\r\nThread exception:\r\n{1}\r\nException:\r\n{2}", new object[]
			{
				windowHandle.ToInt64(),
				threadException,
				exception
			});
			this.TraceEvent(TraceEventType.Error, MessageTraceEventId.ThreadExceptionDelegation, messageText);
		}

		// Token: 0x06000123 RID: 291 RVA: 0x0000AED4 File Offset: 0x000090D4
		public void ThreadExceptionDelegation_Stop(IntPtr windowHandle, bool handled, Exception threadException)
		{
			string messageText;
			if (handled)
			{
				messageText = string.Format(CultureInfo.InvariantCulture, "Thread exception handled by thread exception handler, suppressing thread exception.\r\nHWND: 0x{0:x4}\r\nThread exception:\r\n{1}", new object[]
				{
					windowHandle.ToInt64(),
					threadException
				});
			}
			else
			{
				messageText = string.Format(CultureInfo.InvariantCulture, "Thread exception not handled by thread exception handler, deferring thread exception.\r\nHWND: 0x{0:x4}\r\nThread exception:\r\n{1}", new object[]
				{
					windowHandle.ToInt64(),
					threadException
				});
			}
			this.TraceEvent(TraceEventType.Stop, MessageTraceEventId.ThreadExceptionDelegation, messageText);
		}

		// Token: 0x06000124 RID: 292 RVA: 0x0000AF4C File Offset: 0x0000914C
		public void MessageWindowStatusChange_Start(IntPtr windowHandle, MessageWindowStatus oldStatus, MessageWindowStatus newStatus)
		{
			string messageText = string.Format(CultureInfo.InvariantCulture, "Changing message window status.\r\nHWND: 0x{0:x4}\r\nOld status: {1}\r\nNew status: {2}", new object[]
			{
				windowHandle.ToInt64(),
				oldStatus,
				newStatus
			});
			this.TraceEvent(TraceEventType.Start, MessageTraceEventId.MessageWindowStatusChange, messageText);
		}

		// Token: 0x06000125 RID: 293 RVA: 0x0000AFA0 File Offset: 0x000091A0
		public void MessageWindowStatusChange_Stop(IntPtr windowHandle, MessageWindowStatus oldStatus, MessageWindowStatus newStatus)
		{
			string messageText = string.Format(CultureInfo.InvariantCulture, "Changed message window status.\r\nHWND: 0x{0:x4}\r\nOld status: {1}\r\nNew status: {2}", new object[]
			{
				windowHandle.ToInt64(),
				oldStatus,
				newStatus
			});
			this.TraceEvent(TraceEventType.Stop, MessageTraceEventId.MessageWindowStatusChange, messageText);
		}

		// Token: 0x06000126 RID: 294 RVA: 0x0000AFF4 File Offset: 0x000091F4
		private void TraceEvent(TraceEventType eventType, MessageTraceEventId id, string messageText)
		{
			base.TraceEvent(eventType, (int)id, messageText);
		}

		// Token: 0x04000098 RID: 152
		private const string TraceSourceName = "Nokia.Lucid.Messages";

		// Token: 0x04000099 RID: 153
		public static readonly MessageTraceSource Instance = new MessageTraceSource();
	}
}
