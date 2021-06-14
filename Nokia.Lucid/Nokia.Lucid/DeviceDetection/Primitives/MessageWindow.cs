using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using Nokia.Lucid.Diagnostics;
using Nokia.Lucid.Interop;
using Nokia.Lucid.Interop.Win32Types;
using Nokia.Lucid.Primitives;
using Nokia.Lucid.Properties;

namespace Nokia.Lucid.DeviceDetection.Primitives
{
	// Token: 0x02000007 RID: 7
	internal sealed class MessageWindow : IDisposable
	{
		// Token: 0x0600000B RID: 11 RVA: 0x0000236C File Offset: 0x0000056C
		private MessageWindow(IHandleDeviceChanged deviceChangeHandler, IHandleThreadException threadExceptionHandler)
		{
			this.deviceChangeHandler = deviceChangeHandler;
			this.threadExceptionHandler = threadExceptionHandler;
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600000C RID: 12 RVA: 0x000023A3 File Offset: 0x000005A3
		public AggregateException Exception
		{
			get
			{
				if (this.currentStatus == MessageWindowStatus.Faulted && this.exception == null)
				{
					this.exception = new AggregateException(this.deferredExceptions);
				}
				return this.exception;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000D RID: 13 RVA: 0x000023D5 File Offset: 0x000005D5
		public MessageWindowStatus Status
		{
			get
			{
				return this.currentStatus;
			}
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000023E0 File Offset: 0x000005E0
		public static void Create(IHandleDeviceChanged deviceChangeHandler, IHandleThreadException threadExceptionHandler, ref MessageWindow window)
		{
			bool flag = false;
			bool flag2 = false;
			string text = null;
			IntPtr intPtr = IntPtr.Zero;
			MessageWindow messageWindow = new MessageWindow(deviceChangeHandler, threadExceptionHandler);
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				MessageWindowClass.AcquireClass(ref text);
				ThreadAffinity.BeginThreadAffinity(ref flag);
				CriticalRegion.BeginCriticalRegion(ref flag2);
				messageWindow.unmanagedThreadId = Kernel32NativeMethods.GetCurrentThreadId();
				IntPtr moduleHandle = Kernel32NativeMethods.GetModuleHandle(IntPtr.Zero);
				if (moduleHandle == IntPtr.Zero)
				{
					throw new Win32Exception();
				}
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
				}
				finally
				{
					RobustTrace.Trace<string>(new Action<string>(MessageTraceSource.Instance.MessageWindowCreation_Start), text);
					intPtr = User32NativeMethods.CreateWindowEx(0, text, null, 0, 0, 0, 0, 0, new IntPtr(-3), IntPtr.Zero, moduleHandle, IntPtr.Zero);
					if (intPtr != IntPtr.Zero)
					{
						messageWindow.handle = intPtr;
						window = messageWindow;
					}
				}
				if (intPtr == IntPtr.Zero)
				{
					try
					{
						throw new Win32Exception();
					}
					catch (Win32Exception ex)
					{
						RobustTrace.Trace<string, int, string>(new Action<string, int, string>(MessageTraceSource.Instance.MessageWindowCreation_Error), text, ex.NativeErrorCode, ex.Message);
						throw;
					}
				}
				RobustTrace.Trace<string, IntPtr>(new Action<string, IntPtr>(MessageTraceSource.Instance.MessageWindowCreation_Stop), text, intPtr);
			}
			catch
			{
				if (intPtr == IntPtr.Zero)
				{
					if (flag2)
					{
						CriticalRegion.EndCriticalRegion();
					}
					if (flag)
					{
						ThreadAffinity.EndThreadAffinity();
					}
					if (text != null)
					{
						MessageWindowClass.ReleaseClass();
					}
				}
				throw;
			}
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002544 File Offset: 0x00000744
		public void RegisterDeviceNotification(IEnumerable<Guid> classGuids)
		{
			this.VerifyNotDisposed();
			this.VerifyAccess();
			foreach (Guid guid in classGuids)
			{
				DEV_BROADCAST_DEVICEINTERFACE dev_BROADCAST_DEVICEINTERFACE = new DEV_BROADCAST_DEVICEINTERFACE
				{
					dbcc_size = Marshal.SizeOf(typeof(DEV_BROADCAST_DEVICEINTERFACE)),
					dbcc_devicetype = 5,
					dbcc_classguid = guid
				};
				RobustTrace.Trace<IntPtr, Guid>(new Action<IntPtr, Guid>(MessageTraceSource.Instance.DeviceNotificationRegistration_Start), this.handle, guid);
				try
				{
					IntPtr intPtr = User32NativeMethods.RegisterDeviceNotification(new HandleRef(this, this.handle), ref dev_BROADCAST_DEVICEINTERFACE, 0);
					if (intPtr == IntPtr.Zero)
					{
						throw new Win32Exception();
					}
					RobustTrace.Trace<IntPtr, Guid, IntPtr>(new Action<IntPtr, Guid, IntPtr>(MessageTraceSource.Instance.DeviceNotificationRegistration_Stop), this.handle, guid, intPtr);
					this.devNotifyHandles.Push(intPtr);
				}
				catch (Win32Exception ex)
				{
					RobustTrace.Trace<IntPtr, Guid, int, string>(new Action<IntPtr, Guid, int, string>(MessageTraceSource.Instance.DeviceNotificationRegistration_Error), this.handle, guid, ex.NativeErrorCode, ex.Message);
					try
					{
						this.CloseAsync();
					}
					catch (Win32Exception ex2)
					{
						throw new AggregateException(new Exception[]
						{
							ex,
							ex2
						});
					}
					throw new AggregateException(new Exception[]
					{
						ex
					});
				}
			}
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000026DC File Offset: 0x000008DC
		public void AttachWindowProc()
		{
			this.VerifyNotDisposed();
			this.VerifyAccess();
			try
			{
				WNDPROC dwNewLong = new WNDPROC(this.WindowProc);
				RobustTrace.Trace<IntPtr>(new Action<IntPtr>(MessageTraceSource.Instance.MessageWindowProcAttach_Start), this.handle);
				if (((IntPtr.Size == 4) ? User32NativeMethods.SetWindowLong(this.handle, -4, dwNewLong) : User32NativeMethods.SetWindowLongPtr(this.handle, -4, dwNewLong)) == null)
				{
					throw new Win32Exception();
				}
				RobustTrace.Trace<IntPtr>(new Action<IntPtr>(MessageTraceSource.Instance.MessageWindowProcAttach_Stop), this.handle);
				this.cachedWindowProc = dwNewLong;
			}
			catch (Win32Exception ex)
			{
				RobustTrace.Trace<IntPtr, int, string>(new Action<IntPtr, int, string>(MessageTraceSource.Instance.MessageWindowProcAttach_Error), this.handle, ex.NativeErrorCode, ex.Message);
				try
				{
					this.CloseAsync();
				}
				catch (Win32Exception ex2)
				{
					throw new AggregateException(new Exception[]
					{
						ex,
						ex2
					});
				}
				throw new AggregateException(new Exception[]
				{
					ex
				});
			}
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000027F0 File Offset: 0x000009F0
		public void CloseAsync()
		{
			IntPtr intPtr;
			lock (this.syncRoot)
			{
				intPtr = this.handle;
			}
			if (intPtr == IntPtr.Zero)
			{
				return;
			}
			RobustTrace.Trace<IntPtr>(new Action<IntPtr>(MessageTraceSource.Instance.MessageWindowCloseRequest_Start), intPtr);
			if (!User32NativeMethods.PostMessage(new HandleRef(this, intPtr), 16, IntPtr.Zero, IntPtr.Zero))
			{
				try
				{
					throw new Win32Exception();
				}
				catch (Win32Exception ex)
				{
					RobustTrace.Trace<IntPtr, int, string>(new Action<IntPtr, int, string>(MessageTraceSource.Instance.MessageWindowCloseRequest_Error), intPtr, ex.NativeErrorCode, ex.Message);
					throw;
				}
			}
			RobustTrace.Trace<IntPtr>(new Action<IntPtr>(MessageTraceSource.Instance.MessageWindowCloseRequest_Stop), intPtr);
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000028BC File Offset: 0x00000ABC
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public void Dispose()
		{
			if (this.disposed)
			{
				return;
			}
			this.VerifyAccess();
			this.disposed = true;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
			}
			finally
			{
				CriticalRegion.EndCriticalRegion();
				ThreadAffinity.EndThreadAffinity();
				MessageWindowClass.ReleaseClass();
			}
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002908 File Offset: 0x00000B08
		private IntPtr WindowProc(IntPtr hwnd, int uMsg, IntPtr wParam, IntPtr lParam)
		{
			RobustTrace.Trace<IntPtr, int, IntPtr, IntPtr>(new Action<IntPtr, int, IntPtr, IntPtr>(MessageTraceSource.Instance.WindowMessage), hwnd, uMsg, wParam, lParam);
			IntPtr result = IntPtr.Zero;
			if (uMsg != 2)
			{
				if (uMsg != 537)
				{
					result = User32NativeMethods.DefWindowProc(hwnd, uMsg, wParam, lParam);
				}
				else
				{
					this.WmDeviceChange(wParam, lParam);
				}
			}
			else
			{
				this.WmDestroy();
			}
			return result;
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002964 File Offset: 0x00000B64
		private void WmDeviceChange(IntPtr wParam, IntPtr lParam)
		{
			DEV_BROADCAST_HDR dev_BROADCAST_HDR = (DEV_BROADCAST_HDR)Marshal.PtrToStructure(lParam, typeof(DEV_BROADCAST_HDR));
			int num = wParam.ToInt32();
			RobustTrace.Trace<IntPtr, int, int>(new Action<IntPtr, int, int>(MessageTraceSource.Instance.DeviceNotification), this.handle, num, dev_BROADCAST_HDR.dbch_devicetype);
			if (dev_BROADCAST_HDR.dbch_devicetype != 5)
			{
				return;
			}
			if (wParam != new IntPtr(32768) && wParam != new IntPtr(32772))
			{
				return;
			}
			DEV_BROADCAST_DEVICEINTERFACE dev_BROADCAST_DEVICEINTERFACE = (DEV_BROADCAST_DEVICEINTERFACE)Marshal.PtrToStructure(lParam, typeof(DEV_BROADCAST_DEVICEINTERFACE));
			try
			{
				RobustTrace.Trace<IntPtr, string, Guid, int>(new Action<IntPtr, string, Guid, int>(MessageTraceSource.Instance.DeviceNotificationProcessing_Start), this.handle, dev_BROADCAST_DEVICEINTERFACE.dbcc_name, dev_BROADCAST_DEVICEINTERFACE.dbcc_classguid, num);
				this.deviceChangeHandler.HandleDeviceChanged(num, ref dev_BROADCAST_DEVICEINTERFACE);
				RobustTrace.Trace<IntPtr, string, Guid, int>(new Action<IntPtr, string, Guid, int>(MessageTraceSource.Instance.DeviceNotificationProcessing_Stop), this.handle, dev_BROADCAST_DEVICEINTERFACE.dbcc_name, dev_BROADCAST_DEVICEINTERFACE.dbcc_classguid, num);
			}
			catch (Exception ex)
			{
				if (ExceptionServices.IsCriticalException(ex))
				{
					throw;
				}
				RobustTrace.Trace<IntPtr, string, Guid, int, Exception>(new Action<IntPtr, string, Guid, int, Exception>(MessageTraceSource.Instance.DeviceNotificationProcessing_Error), this.handle, dev_BROADCAST_DEVICEINTERFACE.dbcc_name, dev_BROADCAST_DEVICEINTERFACE.dbcc_classguid, num, ex);
				this.HandleThreadException(ex);
			}
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002AAC File Offset: 0x00000CAC
		private void HandleThreadException(Exception error)
		{
			bool flag = false;
			RobustTrace.Trace<IntPtr, Exception>(new Action<IntPtr, Exception>(MessageTraceSource.Instance.ThreadExceptionDelegation_Start), this.handle, error);
			try
			{
				flag = this.threadExceptionHandler.TryHandleThreadException(error);
			}
			catch (Exception ex)
			{
				if (ExceptionServices.IsCriticalException(ex))
				{
					throw;
				}
				RobustTrace.Trace<IntPtr, Exception, Exception>(new Action<IntPtr, Exception, Exception>(MessageTraceSource.Instance.ThreadExceptionDelegation_Error), this.handle, error, ex);
				this.deferredExceptions.Add(ex);
			}
			RobustTrace.Trace<IntPtr, bool, Exception>(new Action<IntPtr, bool, Exception>(MessageTraceSource.Instance.ThreadExceptionDelegation_Stop), this.handle, flag, error);
			if (flag)
			{
				return;
			}
			this.deferredExceptions.Add(error);
			this.CloseAsync();
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002B60 File Offset: 0x00000D60
		private void WmDestroy()
		{
			RobustTrace.Trace(new Action(MessageTraceSource.Instance.MessageLoopExitRequest_Start));
			User32NativeMethods.PostQuitMessage(0);
			RobustTrace.Trace(new Action(MessageTraceSource.Instance.MessageLoopExitRequest_Stop));
			IntPtr arg = this.handle;
			lock (this.syncRoot)
			{
				this.handle = new IntPtr(-1);
				goto IL_E3;
			}
			IL_67:
			IntPtr arg2 = this.devNotifyHandles.Pop();
			RobustTrace.Trace<IntPtr, IntPtr>(new Action<IntPtr, IntPtr>(MessageTraceSource.Instance.DeviceNotificationUnregistration_Start), arg, arg2);
			if (User32NativeMethods.UnregisterDeviceNotification(arg2))
			{
				RobustTrace.Trace<IntPtr, IntPtr>(new Action<IntPtr, IntPtr>(MessageTraceSource.Instance.DeviceNotificationUnregistration_Stop), arg, arg2);
			}
			else
			{
				try
				{
					throw new Win32Exception();
				}
				catch (Win32Exception ex)
				{
					RobustTrace.Trace<IntPtr, IntPtr, int, string>(new Action<IntPtr, IntPtr, int, string>(MessageTraceSource.Instance.DeviceNotificationUnregistration_Error), arg, arg2, ex.NativeErrorCode, ex.Message);
					this.deferredExceptions.Add(ex);
				}
			}
			IL_E3:
			if (this.devNotifyHandles.Count <= 0)
			{
				MessageWindowStatus arg3 = this.currentStatus;
				MessageWindowStatus arg4 = (this.deferredExceptions.Count == 0) ? MessageWindowStatus.Destroyed : MessageWindowStatus.Faulted;
				RobustTrace.Trace<IntPtr, MessageWindowStatus, MessageWindowStatus>(new Action<IntPtr, MessageWindowStatus, MessageWindowStatus>(MessageTraceSource.Instance.MessageWindowStatusChange_Start), arg, arg3, arg4);
				this.currentStatus = arg4;
				RobustTrace.Trace<IntPtr, MessageWindowStatus, MessageWindowStatus>(new Action<IntPtr, MessageWindowStatus, MessageWindowStatus>(MessageTraceSource.Instance.MessageWindowStatusChange_Stop), arg, arg3, arg4);
				return;
			}
			goto IL_67;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002CD8 File Offset: 0x00000ED8
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		private void VerifyAccess()
		{
			if (this.unmanagedThreadId != Kernel32NativeMethods.GetCurrentThreadId())
			{
				throw new InvalidOperationException(Resources.InvalidOperationException_MessageText_CallingThreadDoesNotHaveAccessToThisMessageWindowInstance);
			}
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002CF2 File Offset: 0x00000EF2
		private bool CheckNotDisposed()
		{
			return this.disposed;
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002CFA File Offset: 0x00000EFA
		private void VerifyNotDisposed()
		{
			if (this.CheckNotDisposed())
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
		}

		// Token: 0x04000007 RID: 7
		private readonly object syncRoot = new object();

		// Token: 0x04000008 RID: 8
		private readonly IHandleDeviceChanged deviceChangeHandler;

		// Token: 0x04000009 RID: 9
		private readonly IHandleThreadException threadExceptionHandler;

		// Token: 0x0400000A RID: 10
		private readonly Stack<IntPtr> devNotifyHandles = new Stack<IntPtr>();

		// Token: 0x0400000B RID: 11
		private readonly List<Exception> deferredExceptions = new List<Exception>();

		// Token: 0x0400000C RID: 12
		private volatile AggregateException exception;

		// Token: 0x0400000D RID: 13
		private volatile MessageWindowStatus currentStatus;

		// Token: 0x0400000E RID: 14
		private IntPtr handle;

		// Token: 0x0400000F RID: 15
		private bool disposed;

		// Token: 0x04000010 RID: 16
		private int unmanagedThreadId;

		// Token: 0x04000011 RID: 17
		private WNDPROC cachedWindowProc;
	}
}
