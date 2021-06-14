using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace Standard
{
	// Token: 0x0200003E RID: 62
	[SecurityCritical]
	internal sealed class SafeDC : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x17000007 RID: 7
		// (set) Token: 0x06000055 RID: 85 RVA: 0x00002F0F File Offset: 0x0000110F
		public IntPtr Hwnd
		{
			[SecurityCritical]
			set
			{
				this._hwnd = new IntPtr?(value);
			}
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00002EF9 File Offset: 0x000010F9
		[SecurityCritical]
		private SafeDC() : base(true)
		{
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00002F20 File Offset: 0x00001120
		[SecurityCritical]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		protected override bool ReleaseHandle()
		{
			if (this._created)
			{
				return SafeDC.NativeMethods.DeleteDC(this.handle);
			}
			return this._hwnd == null || this._hwnd.Value == IntPtr.Zero || SafeDC.NativeMethods.ReleaseDC(this._hwnd.Value, this.handle) == 1;
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00002F80 File Offset: 0x00001180
		[SecurityCritical]
		public static SafeDC CreateDC(string deviceName)
		{
			SafeDC safeDC = null;
			try
			{
				safeDC = SafeDC.NativeMethods.CreateDC(deviceName, null, IntPtr.Zero, IntPtr.Zero);
			}
			finally
			{
				if (safeDC != null)
				{
					safeDC._created = true;
				}
			}
			if (safeDC.IsInvalid)
			{
				safeDC.Dispose();
				throw new SystemException("Unable to create a device context from the specified device information.");
			}
			return safeDC;
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00002FD8 File Offset: 0x000011D8
		[SecurityCritical]
		public static SafeDC CreateCompatibleDC(SafeDC hdc)
		{
			SafeDC safeDC = null;
			try
			{
				IntPtr hdc2 = IntPtr.Zero;
				if (hdc != null)
				{
					hdc2 = hdc.handle;
				}
				safeDC = SafeDC.NativeMethods.CreateCompatibleDC(hdc2);
				if (safeDC == null)
				{
					HRESULT.ThrowLastError();
				}
			}
			finally
			{
				if (safeDC != null)
				{
					safeDC._created = true;
				}
			}
			if (safeDC.IsInvalid)
			{
				safeDC.Dispose();
				throw new SystemException("Unable to create a device context from the specified device information.");
			}
			return safeDC;
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00003040 File Offset: 0x00001240
		[SecurityCritical]
		public static SafeDC GetDC(IntPtr hwnd)
		{
			SafeDC safeDC = null;
			try
			{
				safeDC = SafeDC.NativeMethods.GetDC(hwnd);
			}
			finally
			{
				if (safeDC != null)
				{
					safeDC.Hwnd = hwnd;
				}
			}
			if (safeDC.IsInvalid)
			{
				HRESULT.E_FAIL.ThrowIfFailed();
			}
			return safeDC;
		}

		// Token: 0x0600005B RID: 91 RVA: 0x0000308C File Offset: 0x0000128C
		[SecurityCritical]
		public static SafeDC GetDesktop()
		{
			return SafeDC.GetDC(IntPtr.Zero);
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00003098 File Offset: 0x00001298
		[SecurityCritical]
		public static SafeDC WrapDC(IntPtr hdc)
		{
			return new SafeDC
			{
				handle = hdc,
				_created = false,
				_hwnd = new IntPtr?(IntPtr.Zero)
			};
		}

		// Token: 0x040003FA RID: 1018
		[SecurityCritical]
		private IntPtr? _hwnd;

		// Token: 0x040003FB RID: 1019
		private bool _created;

		// Token: 0x0200080A RID: 2058
		private static class NativeMethods
		{
			// Token: 0x06007E1D RID: 32285
			[SecurityCritical]
			[DllImport("user32.dll")]
			public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

			// Token: 0x06007E1E RID: 32286
			[SecurityCritical]
			[DllImport("user32.dll")]
			public static extern SafeDC GetDC(IntPtr hwnd);

			// Token: 0x06007E1F RID: 32287
			[SecurityCritical]
			[DllImport("gdi32.dll", CharSet = CharSet.Unicode)]
			public static extern SafeDC CreateDC([MarshalAs(UnmanagedType.LPWStr)] string lpszDriver, [MarshalAs(UnmanagedType.LPWStr)] string lpszDevice, IntPtr lpszOutput, IntPtr lpInitData);

			// Token: 0x06007E20 RID: 32288
			[SecurityCritical]
			[DllImport("gdi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
			public static extern SafeDC CreateCompatibleDC(IntPtr hdc);

			// Token: 0x06007E21 RID: 32289
			[SecurityCritical]
			[DllImport("gdi32.dll")]
			[return: MarshalAs(UnmanagedType.Bool)]
			public static extern bool DeleteDC(IntPtr hdc);
		}
	}
}
