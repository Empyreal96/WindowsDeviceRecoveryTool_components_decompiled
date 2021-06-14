using System;
using System.Runtime.InteropServices;
using System.Security;
using MS.Internal.Interop;

namespace Standard
{
	// Token: 0x0200007D RID: 125
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("886d8eeb-8cf2-4446-8d02-cdba1dbdcf99")]
	[ComImport]
	internal interface IPropertyStore
	{
		// Token: 0x0600012D RID: 301
		uint GetCount();

		// Token: 0x0600012E RID: 302
		PKEY GetAt(uint iProp);

		// Token: 0x0600012F RID: 303
		[SecurityCritical]
		void GetValue([In] ref PKEY pkey, [In] [Out] PROPVARIANT pv);

		// Token: 0x06000130 RID: 304
		[SecurityCritical]
		void SetValue([In] ref PKEY pkey, PROPVARIANT pv);

		// Token: 0x06000131 RID: 305
		void Commit();
	}
}
