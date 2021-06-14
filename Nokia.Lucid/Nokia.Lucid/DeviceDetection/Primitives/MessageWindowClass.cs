using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Threading;
using Nokia.Lucid.Diagnostics;
using Nokia.Lucid.Interop;
using Nokia.Lucid.Interop.Win32Types;

namespace Nokia.Lucid.DeviceDetection.Primitives
{
	// Token: 0x0200000A RID: 10
	internal static class MessageWindowClass
	{
		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000022 RID: 34 RVA: 0x00002E64 File Offset: 0x00001064
		public static bool IsClassAcquired
		{
			get
			{
				return MessageWindowClass.referenceCount > 0;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000023 RID: 35 RVA: 0x00002E6E File Offset: 0x0000106E
		internal static int ReferenceCount
		{
			get
			{
				return MessageWindowClass.referenceCount;
			}
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002E78 File Offset: 0x00001078
		public static void AcquireClass(ref string className)
		{
			if (MessageWindowClass.referenceCount > 0)
			{
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
					return;
				}
				finally
				{
					MessageWindowClass.referenceCount++;
					className = MessageWindowClass.threadClassName;
				}
			}
			IntPtr moduleHandle = Kernel32NativeMethods.GetModuleHandle(IntPtr.Zero);
			if (moduleHandle == IntPtr.Zero)
			{
				throw new Win32Exception();
			}
			string text = typeof(MessageWindowClass).FullName + "." + Thread.CurrentThread.ManagedThreadId;
			WNDCLASSEX wndclassex = new WNDCLASSEX
			{
				cbSize = Marshal.SizeOf(typeof(WNDCLASSEX)),
				lpszClassName = text,
				hInstance = moduleHandle,
				lpfnWndProc = MessageWindowClass.CachedDefWindowProc
			};
			RuntimeHelpers.PrepareConstrainedRegions();
			int num;
			try
			{
			}
			finally
			{
				num = (int)User32NativeMethods.RegisterClassEx(ref wndclassex);
				if (num != 0)
				{
					MessageWindowClass.referenceCount++;
					MessageWindowClass.threadClassName = text;
					className = text;
				}
			}
			if (num == 0)
			{
				throw new Win32Exception();
			}
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002F7C File Offset: 0x0000117C
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public static void ReleaseClass()
		{
			if (MessageWindowClass.referenceCount <= 0)
			{
				throw new InvalidOperationException();
			}
			if (MessageWindowClass.referenceCount > 1)
			{
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
					return;
				}
				finally
				{
					MessageWindowClass.referenceCount--;
				}
			}
			IntPtr moduleHandle = Kernel32NativeMethods.GetModuleHandle(IntPtr.Zero);
			if (moduleHandle == IntPtr.Zero)
			{
				throw new Win32Exception();
			}
			RuntimeHelpers.PrepareConstrainedRegions();
			bool flag;
			try
			{
			}
			finally
			{
				flag = User32NativeMethods.UnregisterClass(MessageWindowClass.threadClassName, moduleHandle);
				if (flag)
				{
					MessageWindowClass.referenceCount--;
					MessageWindowClass.threadClassName = null;
				}
			}
			if (!flag)
			{
				throw new Win32Exception();
			}
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00003020 File Offset: 0x00001220
		private static IntPtr DefWindowProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam)
		{
			RobustTrace.Trace<IntPtr, int, IntPtr, IntPtr>(new Action<IntPtr, int, IntPtr, IntPtr>(MessageTraceSource.Instance.WindowMessage), hWnd, msg, wParam, lParam);
			if (msg == 2)
			{
				RobustTrace.Trace(new Action(MessageTraceSource.Instance.MessageLoopExitRequest_Start));
				User32NativeMethods.PostQuitMessage(0);
				RobustTrace.Trace(new Action(MessageTraceSource.Instance.MessageLoopExitRequest_Stop));
				return IntPtr.Zero;
			}
			return User32NativeMethods.DefWindowProc(hWnd, msg, wParam, lParam);
		}

		// Token: 0x04000014 RID: 20
		private static readonly WNDPROC CachedDefWindowProc = new WNDPROC(MessageWindowClass.DefWindowProc);

		// Token: 0x04000015 RID: 21
		[ThreadStatic]
		private static int referenceCount;

		// Token: 0x04000016 RID: 22
		[ThreadStatic]
		private static string threadClassName;
	}
}
