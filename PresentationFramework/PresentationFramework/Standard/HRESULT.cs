using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using MS.Internal.PresentationFramework;

namespace Standard
{
	// Token: 0x0200000A RID: 10
	[StructLayout(LayoutKind.Explicit)]
	internal struct HRESULT
	{
		// Token: 0x06000037 RID: 55 RVA: 0x00002776 File Offset: 0x00000976
		public HRESULT(uint i)
		{
			this._value = i;
		}

		// Token: 0x06000038 RID: 56 RVA: 0x0000277F File Offset: 0x0000097F
		public static HRESULT Make(bool severe, Facility facility, int code)
		{
			return new HRESULT((uint)((severe ? ((Facility)(-2147483648)) : Facility.Null) | (int)facility << 16 | (Facility)code));
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000039 RID: 57 RVA: 0x00002798 File Offset: 0x00000998
		public Facility Facility
		{
			get
			{
				return HRESULT.GetFacility((int)this._value);
			}
		}

		// Token: 0x0600003A RID: 58 RVA: 0x000027A5 File Offset: 0x000009A5
		public static Facility GetFacility(int errorCode)
		{
			return (Facility)(errorCode >> 16 & 8191);
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600003B RID: 59 RVA: 0x000027B1 File Offset: 0x000009B1
		public int Code
		{
			get
			{
				return HRESULT.GetCode((int)this._value);
			}
		}

		// Token: 0x0600003C RID: 60 RVA: 0x000027BE File Offset: 0x000009BE
		public static int GetCode(int error)
		{
			return error & 65535;
		}

		// Token: 0x0600003D RID: 61 RVA: 0x000027C8 File Offset: 0x000009C8
		public override string ToString()
		{
			foreach (FieldInfo fieldInfo in typeof(HRESULT).GetFields(BindingFlags.Static | BindingFlags.Public))
			{
				if (fieldInfo.FieldType == typeof(HRESULT))
				{
					HRESULT hrLeft = (HRESULT)fieldInfo.GetValue(null);
					if (hrLeft == this)
					{
						return fieldInfo.Name;
					}
				}
			}
			if (this.Facility == Facility.Win32)
			{
				foreach (FieldInfo fieldInfo2 in typeof(Win32Error).GetFields(BindingFlags.Static | BindingFlags.Public))
				{
					if (fieldInfo2.FieldType == typeof(Win32Error))
					{
						Win32Error error = (Win32Error)fieldInfo2.GetValue(null);
						if ((HRESULT)error == this)
						{
							return "HRESULT_FROM_WIN32(" + fieldInfo2.Name + ")";
						}
					}
				}
			}
			return string.Format(CultureInfo.InvariantCulture, "0x{0:X8}", new object[]
			{
				this._value
			});
		}

		// Token: 0x0600003E RID: 62 RVA: 0x000028E0 File Offset: 0x00000AE0
		public override bool Equals(object obj)
		{
			bool result;
			try
			{
				result = (((HRESULT)obj)._value == this._value);
			}
			catch (InvalidCastException)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x0600003F RID: 63 RVA: 0x0000291C File Offset: 0x00000B1C
		public override int GetHashCode()
		{
			return this._value.GetHashCode();
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00002937 File Offset: 0x00000B37
		public static bool operator ==(HRESULT hrLeft, HRESULT hrRight)
		{
			return hrLeft._value == hrRight._value;
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00002947 File Offset: 0x00000B47
		public static bool operator !=(HRESULT hrLeft, HRESULT hrRight)
		{
			return !(hrLeft == hrRight);
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000042 RID: 66 RVA: 0x00002953 File Offset: 0x00000B53
		public bool Succeeded
		{
			get
			{
				return this._value >= 0U;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000043 RID: 67 RVA: 0x00002961 File Offset: 0x00000B61
		public bool Failed
		{
			get
			{
				return this._value < 0U;
			}
		}

		// Token: 0x06000044 RID: 68 RVA: 0x0000296C File Offset: 0x00000B6C
		public void ThrowIfFailed()
		{
			this.ThrowIfFailed(null);
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00002978 File Offset: 0x00000B78
		public void ThrowIfFailed(string message)
		{
			if (this.Failed)
			{
				if (string.IsNullOrEmpty(message))
				{
					message = this.ToString();
				}
				Exception ex = SecurityHelper.GetExceptionForHR((int)this._value);
				if (ex.GetType() == typeof(COMException))
				{
					Facility facility = this.Facility;
					if (facility == Facility.Win32)
					{
						ex = HRESULT.CreateWin32Exception(this.Code, message);
					}
					else
					{
						ex = new COMException(message, (int)this._value);
					}
				}
				else
				{
					ConstructorInfo constructor = ex.GetType().GetConstructor(new Type[]
					{
						typeof(string)
					});
					if (null != constructor)
					{
						ex = (constructor.Invoke(new object[]
						{
							message
						}) as Exception);
					}
				}
				throw ex;
			}
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00002A34 File Offset: 0x00000C34
		[SecurityCritical]
		public static void ThrowLastError()
		{
			((HRESULT)Win32Error.GetLastError()).ThrowIfFailed();
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00002A53 File Offset: 0x00000C53
		[SecuritySafeCritical]
		private static Exception CreateWin32Exception(int code, string message)
		{
			return new Win32Exception(code, message);
		}

		// Token: 0x04000043 RID: 67
		[FieldOffset(0)]
		private readonly uint _value;

		// Token: 0x04000044 RID: 68
		public static readonly HRESULT S_OK = new HRESULT(0U);

		// Token: 0x04000045 RID: 69
		public static readonly HRESULT S_FALSE = new HRESULT(1U);

		// Token: 0x04000046 RID: 70
		public static readonly HRESULT E_PENDING = new HRESULT(2147483658U);

		// Token: 0x04000047 RID: 71
		public static readonly HRESULT E_NOTIMPL = new HRESULT(2147500033U);

		// Token: 0x04000048 RID: 72
		public static readonly HRESULT E_NOINTERFACE = new HRESULT(2147500034U);

		// Token: 0x04000049 RID: 73
		public static readonly HRESULT E_POINTER = new HRESULT(2147500035U);

		// Token: 0x0400004A RID: 74
		public static readonly HRESULT E_ABORT = new HRESULT(2147500036U);

		// Token: 0x0400004B RID: 75
		public static readonly HRESULT E_FAIL = new HRESULT(2147500037U);

		// Token: 0x0400004C RID: 76
		public static readonly HRESULT E_UNEXPECTED = new HRESULT(2147549183U);

		// Token: 0x0400004D RID: 77
		public static readonly HRESULT STG_E_INVALIDFUNCTION = new HRESULT(2147680257U);

		// Token: 0x0400004E RID: 78
		public static readonly HRESULT REGDB_E_CLASSNOTREG = new HRESULT(2147746132U);

		// Token: 0x0400004F RID: 79
		public static readonly HRESULT DESTS_E_NO_MATCHING_ASSOC_HANDLER = new HRESULT(2147749635U);

		// Token: 0x04000050 RID: 80
		public static readonly HRESULT DESTS_E_NORECDOCS = new HRESULT(2147749636U);

		// Token: 0x04000051 RID: 81
		public static readonly HRESULT DESTS_E_NOTALLCLEARED = new HRESULT(2147749637U);

		// Token: 0x04000052 RID: 82
		public static readonly HRESULT E_ACCESSDENIED = new HRESULT(2147942405U);

		// Token: 0x04000053 RID: 83
		public static readonly HRESULT E_OUTOFMEMORY = new HRESULT(2147942414U);

		// Token: 0x04000054 RID: 84
		public static readonly HRESULT E_INVALIDARG = new HRESULT(2147942487U);

		// Token: 0x04000055 RID: 85
		public static readonly HRESULT INTSAFE_E_ARITHMETIC_OVERFLOW = new HRESULT(2147942934U);

		// Token: 0x04000056 RID: 86
		public static readonly HRESULT COR_E_OBJECTDISPOSED = new HRESULT(2148734498U);

		// Token: 0x04000057 RID: 87
		public static readonly HRESULT WC_E_GREATERTHAN = new HRESULT(3222072867U);

		// Token: 0x04000058 RID: 88
		public static readonly HRESULT WC_E_SYNTAX = new HRESULT(3222072877U);
	}
}
