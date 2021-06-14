using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace PortableDeviceApiLib
{
	// Token: 0x02000030 RID: 48
	[Guid("6848F6F2-3155-4F86-B6F5-263EEEAB3143")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[CompilerGenerated]
	[TypeIdentifier]
	[ComImport]
	public interface IPortableDeviceValues
	{
		// Token: 0x0600015E RID: 350
		void _VtblGap1_3();

		// Token: 0x0600015F RID: 351
		void GetValue([In] ref _tagpropertykey key, out tag_inner_PROPVARIANT pValue);

		// Token: 0x06000160 RID: 352
		void SetStringValue([In] ref _tagpropertykey key, [MarshalAs(UnmanagedType.LPWStr)] [In] string Value);

		// Token: 0x06000161 RID: 353
		void GetStringValue([In] ref _tagpropertykey key, [MarshalAs(UnmanagedType.LPWStr)] out string pValue);

		// Token: 0x06000162 RID: 354
		void SetUnsignedIntegerValue([In] ref _tagpropertykey key, [In] uint Value);

		// Token: 0x06000163 RID: 355
		void GetUnsignedIntegerValue([In] ref _tagpropertykey key, out uint pValue);

		// Token: 0x06000164 RID: 356
		void _VtblGap2_9();

		// Token: 0x06000165 RID: 357
		void GetErrorValue([In] ref _tagpropertykey key, [MarshalAs(UnmanagedType.Error)] out int pValue);

		// Token: 0x06000166 RID: 358
		void _VtblGap3_6();

		// Token: 0x06000167 RID: 359
		void SetGuidValue([In] ref _tagpropertykey key, [In] ref Guid Value);

		// Token: 0x06000168 RID: 360
		void GetGuidValue([In] ref _tagpropertykey key, out Guid pValue);

		// Token: 0x06000169 RID: 361
		void SetBufferValue([In] ref _tagpropertykey key, [In] IntPtr pValue, [In] uint cbValue);

		// Token: 0x0600016A RID: 362
		void GetBufferValue([In] ref _tagpropertykey key, out IntPtr ppValue, out uint pcbValue);

		// Token: 0x0600016B RID: 363
		void _VtblGap4_2();

		// Token: 0x0600016C RID: 364
		void SetIPortableDevicePropVariantCollectionValue([In] ref _tagpropertykey key, [MarshalAs(UnmanagedType.Interface)] [In] IPortableDevicePropVariantCollection pValue);

		// Token: 0x0600016D RID: 365
		void GetIPortableDevicePropVariantCollectionValue([In] ref _tagpropertykey key, [MarshalAs(UnmanagedType.Interface)] out IPortableDevicePropVariantCollection ppValue);

		// Token: 0x0600016E RID: 366
		void _VtblGap5_7();

		// Token: 0x0600016F RID: 367
		void Clear();
	}
}
