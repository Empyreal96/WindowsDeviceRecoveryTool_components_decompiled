using System;
using System.Runtime.InteropServices;
using System.Security;
using MS.Internal.Interop;

namespace MS.Internal.AppModel
{
	// Token: 0x020007A7 RID: 1959
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[SuppressUnmanagedCodeSecurity]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("000214F2-0000-0000-C000-000000000046")]
	[ComImport]
	internal interface IEnumIDList
	{
		// Token: 0x06007A6E RID: 31342
		[PreserveSig]
		HRESULT Next(uint celt, out IntPtr rgelt, out int pceltFetched);

		// Token: 0x06007A6F RID: 31343
		[PreserveSig]
		HRESULT Skip(uint celt);

		// Token: 0x06007A70 RID: 31344
		void Reset();

		// Token: 0x06007A71 RID: 31345
		[return: MarshalAs(UnmanagedType.Interface)]
		IEnumIDList Clone();
	}
}
