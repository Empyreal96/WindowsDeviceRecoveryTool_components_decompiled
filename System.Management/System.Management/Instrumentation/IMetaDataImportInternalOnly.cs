using System;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Management.Instrumentation
{
	// Token: 0x020000C5 RID: 197
	[Guid("7DAC8207-D3AE-4c75-9B67-92801A497D44")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[TypeLibType(TypeLibTypeFlags.FRestricted)]
	[ComImport]
	internal interface IMetaDataImportInternalOnly
	{
		// Token: 0x06000570 RID: 1392
		void f1();

		// Token: 0x06000571 RID: 1393
		void f2();

		// Token: 0x06000572 RID: 1394
		void f3();

		// Token: 0x06000573 RID: 1395
		void f4();

		// Token: 0x06000574 RID: 1396
		void f5();

		// Token: 0x06000575 RID: 1397
		void f6();

		// Token: 0x06000576 RID: 1398
		void f7();

		// Token: 0x06000577 RID: 1399
		void GetScopeProps([MarshalAs(UnmanagedType.LPWStr)] [Out] StringBuilder szName, [In] uint cchName, out uint pchName, out Guid pmvid);
	}
}
