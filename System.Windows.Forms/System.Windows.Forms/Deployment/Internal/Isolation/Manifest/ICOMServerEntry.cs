using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x0200009F RID: 159
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("3903B11B-FBE8-477c-825F-DB828B5FD174")]
	[ComImport]
	internal interface ICOMServerEntry
	{
		// Token: 0x17000098 RID: 152
		// (get) Token: 0x0600026F RID: 623
		COMServerEntry AllData { [SecurityCritical] get; }

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000270 RID: 624
		Guid Clsid { [SecurityCritical] get; }

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000271 RID: 625
		uint Flags { [SecurityCritical] get; }

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000272 RID: 626
		Guid ConfiguredGuid { [SecurityCritical] get; }

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000273 RID: 627
		Guid ImplementedClsid { [SecurityCritical] get; }

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000274 RID: 628
		Guid TypeLibrary { [SecurityCritical] get; }

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x06000275 RID: 629
		uint ThreadingModel { [SecurityCritical] get; }

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000276 RID: 630
		string RuntimeVersion { [SecurityCritical] [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x06000277 RID: 631
		string HostFile { [SecurityCritical] [return: MarshalAs(UnmanagedType.LPWStr)] get; }
	}
}
