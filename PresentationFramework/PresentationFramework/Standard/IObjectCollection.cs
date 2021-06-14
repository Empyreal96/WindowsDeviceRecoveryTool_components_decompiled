using System;
using System.Runtime.InteropServices;

namespace Standard
{
	// Token: 0x0200007C RID: 124
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("92CA9DCD-5622-4bba-A805-5E9F541BD8C9")]
	[ComImport]
	internal interface IObjectCollection : IObjectArray
	{
		// Token: 0x06000127 RID: 295
		uint GetCount();

		// Token: 0x06000128 RID: 296
		[return: MarshalAs(UnmanagedType.IUnknown)]
		object GetAt([In] uint uiIndex, [In] ref Guid riid);

		// Token: 0x06000129 RID: 297
		void AddObject([MarshalAs(UnmanagedType.IUnknown)] object punk);

		// Token: 0x0600012A RID: 298
		void AddFromArray(IObjectArray poaSource);

		// Token: 0x0600012B RID: 299
		void RemoveObjectAt(uint uiIndex);

		// Token: 0x0600012C RID: 300
		void Clear();
	}
}
