using System;
using System.Runtime.InteropServices;
using System.Security;

namespace MS.Internal.AppModel
{
	// Token: 0x020007A9 RID: 1961
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[SuppressUnmanagedCodeSecurity]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("92CA9DCD-5622-4bba-A805-5E9F541BD8C9")]
	[ComImport]
	internal interface IObjectCollection : IObjectArray
	{
		// Token: 0x06007A74 RID: 31348
		uint GetCount();

		// Token: 0x06007A75 RID: 31349
		[return: MarshalAs(UnmanagedType.IUnknown)]
		object GetAt([In] uint uiIndex, [In] ref Guid riid);

		// Token: 0x06007A76 RID: 31350
		void AddObject([MarshalAs(UnmanagedType.IUnknown)] object punk);

		// Token: 0x06007A77 RID: 31351
		void AddFromArray(IObjectArray poaSource);

		// Token: 0x06007A78 RID: 31352
		void RemoveObjectAt(uint uiIndex);

		// Token: 0x06007A79 RID: 31353
		void Clear();
	}
}
