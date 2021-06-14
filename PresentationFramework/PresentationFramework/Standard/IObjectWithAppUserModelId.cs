using System;
using System.Runtime.InteropServices;

namespace Standard
{
	// Token: 0x02000088 RID: 136
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("36db0196-9665-46d1-9ba7-d3709eecf9ed")]
	[ComImport]
	internal interface IObjectWithAppUserModelId
	{
		// Token: 0x06000185 RID: 389
		void SetAppID([MarshalAs(UnmanagedType.LPWStr)] string pszAppID);

		// Token: 0x06000186 RID: 390
		[return: MarshalAs(UnmanagedType.LPWStr)]
		string GetAppID();
	}
}
