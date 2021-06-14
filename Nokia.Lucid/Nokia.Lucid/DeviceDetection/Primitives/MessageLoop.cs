using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Nokia.Lucid.Diagnostics;
using Nokia.Lucid.Interop;
using Nokia.Lucid.Interop.Win32Types;
using Nokia.Lucid.Primitives;

namespace Nokia.Lucid.DeviceDetection.Primitives
{
	// Token: 0x02000005 RID: 5
	internal static class MessageLoop
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000008 RID: 8 RVA: 0x00002103 File Offset: 0x00000303
		public static bool IsRunning
		{
			get
			{
				return MessageLoop.level > 0;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000009 RID: 9 RVA: 0x0000210D File Offset: 0x0000030D
		internal static int Level
		{
			get
			{
				return MessageLoop.level;
			}
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002114 File Offset: 0x00000314
		public static int Run()
		{
			int result = 0;
			bool flag = true;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				ThreadAffinity.BeginThreadAffinity(ref flag2);
				CriticalRegion.BeginCriticalRegion(ref flag3);
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
				}
				finally
				{
					MessageLoop.level++;
					flag4 = true;
				}
				RobustTrace.Trace(new Action(MessageTraceSource.Instance.MessageLoopEnter_StartStop));
				while (flag)
				{
					if (User32NativeMethods.MsgWaitForMultipleObjectsEx(0, IntPtr.Zero, 100, 1279, 4) != 258)
					{
						MSG msg;
						while (User32NativeMethods.PeekMessage(out msg, default(HandleRef), 0, 0, 1))
						{
							if (msg.message == 18)
							{
								RobustTrace.Trace(new Action(MessageTraceSource.Instance.MessageLoopExit_Start));
								flag = false;
								result = msg.wParam.ToInt32();
								break;
							}
							if (msg.hwnd == IntPtr.Zero)
							{
								RobustTrace.Trace<int, IntPtr, IntPtr>(new Action<int, IntPtr, IntPtr>(MessageTraceSource.Instance.ThreadMessage), msg.message, msg.wParam, msg.lParam);
							}
							else
							{
								User32NativeMethods.TranslateMessage(ref msg);
								RobustTrace.Trace<IntPtr, int, IntPtr, IntPtr>(new Action<IntPtr, int, IntPtr, IntPtr>(MessageTraceSource.Instance.MessageDispatch_Start), msg.hwnd, msg.message, msg.wParam, msg.lParam);
								try
								{
									User32NativeMethods.DispatchMessage(ref msg);
									RobustTrace.Trace<IntPtr, int, IntPtr, IntPtr>(new Action<IntPtr, int, IntPtr, IntPtr>(MessageTraceSource.Instance.MessageDispatch_Stop), msg.hwnd, msg.message, msg.wParam, msg.lParam);
								}
								catch (Exception ex)
								{
									if (ExceptionServices.IsCriticalException(ex))
									{
										throw;
									}
									RobustTrace.Trace<IntPtr, int, IntPtr, IntPtr, Exception>(new Action<IntPtr, int, IntPtr, IntPtr, Exception>(MessageTraceSource.Instance.MessageDispatch_Error), msg.hwnd, msg.message, msg.wParam, msg.lParam, ex);
								}
							}
						}
					}
					else
					{
						Thread.Yield();
					}
				}
			}
			finally
			{
				if (flag3)
				{
					CriticalRegion.EndCriticalRegion();
				}
				if (flag2)
				{
					ThreadAffinity.EndThreadAffinity();
				}
				if (flag4)
				{
					MessageLoop.level--;
				}
			}
			RobustTrace.Trace(new Action(MessageTraceSource.Instance.MessageLoopExit_Stop));
			return result;
		}

		// Token: 0x04000002 RID: 2
		[ThreadStatic]
		private static int level;
	}
}
