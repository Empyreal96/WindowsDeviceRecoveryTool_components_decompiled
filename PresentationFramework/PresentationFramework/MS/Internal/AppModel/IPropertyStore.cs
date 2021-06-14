using System;
using System.Runtime.InteropServices;
using System.Security;
using MS.Internal.Interop;

namespace MS.Internal.AppModel
{
	// Token: 0x020007AA RID: 1962
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[SuppressUnmanagedCodeSecurity]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("886d8eeb-8cf2-4446-8d02-cdba1dbdcf99")]
	[ComImport]
	internal interface IPropertyStore
	{
		// Token: 0x06007A7A RID: 31354
		uint GetCount();

		// Token: 0x06007A7B RID: 31355
		PKEY GetAt(uint iProp);

		// Token: 0x06007A7C RID: 31356
		[SecurityCritical]
		void GetValue([In] ref PKEY pkey, [In] [Out] PROPVARIANT pv);

		// Token: 0x06007A7D RID: 31357
		[SecurityCritical]
		void SetValue([In] ref PKEY pkey, PROPVARIANT pv);

		// Token: 0x06007A7E RID: 31358
		void Commit();
	}
}
