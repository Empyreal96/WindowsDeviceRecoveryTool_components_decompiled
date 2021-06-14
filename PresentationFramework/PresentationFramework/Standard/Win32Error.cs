using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Standard
{
	// Token: 0x02000008 RID: 8
	[StructLayout(LayoutKind.Explicit)]
	internal struct Win32Error
	{
		// Token: 0x0600002E RID: 46 RVA: 0x000025A8 File Offset: 0x000007A8
		public Win32Error(int i)
		{
			this._value = i;
		}

		// Token: 0x0600002F RID: 47 RVA: 0x000025B1 File Offset: 0x000007B1
		public static explicit operator HRESULT(Win32Error error)
		{
			if (error._value <= 0)
			{
				return new HRESULT((uint)error._value);
			}
			return HRESULT.Make(true, Facility.Win32, error._value & 65535);
		}

		// Token: 0x06000030 RID: 48 RVA: 0x000025DB File Offset: 0x000007DB
		public HRESULT ToHRESULT()
		{
			return (HRESULT)this;
		}

		// Token: 0x06000031 RID: 49 RVA: 0x000025E8 File Offset: 0x000007E8
		[SecurityCritical]
		public static Win32Error GetLastError()
		{
			return new Win32Error(Marshal.GetLastWin32Error());
		}

		// Token: 0x06000032 RID: 50 RVA: 0x000025F4 File Offset: 0x000007F4
		public override bool Equals(object obj)
		{
			bool result;
			try
			{
				result = (((Win32Error)obj)._value == this._value);
			}
			catch (InvalidCastException)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00002630 File Offset: 0x00000830
		public override int GetHashCode()
		{
			return this._value.GetHashCode();
		}

		// Token: 0x06000034 RID: 52 RVA: 0x0000264B File Offset: 0x0000084B
		public static bool operator ==(Win32Error errLeft, Win32Error errRight)
		{
			return errLeft._value == errRight._value;
		}

		// Token: 0x06000035 RID: 53 RVA: 0x0000265B File Offset: 0x0000085B
		public static bool operator !=(Win32Error errLeft, Win32Error errRight)
		{
			return !(errLeft == errRight);
		}

		// Token: 0x04000023 RID: 35
		[FieldOffset(0)]
		private readonly int _value;

		// Token: 0x04000024 RID: 36
		public static readonly Win32Error ERROR_SUCCESS = new Win32Error(0);

		// Token: 0x04000025 RID: 37
		public static readonly Win32Error ERROR_INVALID_FUNCTION = new Win32Error(1);

		// Token: 0x04000026 RID: 38
		public static readonly Win32Error ERROR_FILE_NOT_FOUND = new Win32Error(2);

		// Token: 0x04000027 RID: 39
		public static readonly Win32Error ERROR_PATH_NOT_FOUND = new Win32Error(3);

		// Token: 0x04000028 RID: 40
		public static readonly Win32Error ERROR_TOO_MANY_OPEN_FILES = new Win32Error(4);

		// Token: 0x04000029 RID: 41
		public static readonly Win32Error ERROR_ACCESS_DENIED = new Win32Error(5);

		// Token: 0x0400002A RID: 42
		public static readonly Win32Error ERROR_INVALID_HANDLE = new Win32Error(6);

		// Token: 0x0400002B RID: 43
		public static readonly Win32Error ERROR_OUTOFMEMORY = new Win32Error(14);

		// Token: 0x0400002C RID: 44
		public static readonly Win32Error ERROR_NO_MORE_FILES = new Win32Error(18);

		// Token: 0x0400002D RID: 45
		public static readonly Win32Error ERROR_SHARING_VIOLATION = new Win32Error(32);

		// Token: 0x0400002E RID: 46
		public static readonly Win32Error ERROR_INVALID_PARAMETER = new Win32Error(87);

		// Token: 0x0400002F RID: 47
		public static readonly Win32Error ERROR_INSUFFICIENT_BUFFER = new Win32Error(122);

		// Token: 0x04000030 RID: 48
		public static readonly Win32Error ERROR_NESTING_NOT_ALLOWED = new Win32Error(215);

		// Token: 0x04000031 RID: 49
		public static readonly Win32Error ERROR_KEY_DELETED = new Win32Error(1018);

		// Token: 0x04000032 RID: 50
		public static readonly Win32Error ERROR_NOT_FOUND = new Win32Error(1168);

		// Token: 0x04000033 RID: 51
		public static readonly Win32Error ERROR_NO_MATCH = new Win32Error(1169);

		// Token: 0x04000034 RID: 52
		public static readonly Win32Error ERROR_BAD_DEVICE = new Win32Error(1200);

		// Token: 0x04000035 RID: 53
		public static readonly Win32Error ERROR_CANCELLED = new Win32Error(1223);

		// Token: 0x04000036 RID: 54
		public static readonly Win32Error ERROR_CLASS_ALREADY_EXISTS = new Win32Error(1410);

		// Token: 0x04000037 RID: 55
		public static readonly Win32Error ERROR_INVALID_DATATYPE = new Win32Error(1804);
	}
}
