using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Threading;
using <CppImplementationDetails>;
using <CrtImplementationDetails>;
using ATL;
using FlashingPlatform;
using Microsoft.Windows.Flashing.Platform;
using RAII;
using std;

// Token: 0x02000001 RID: 1
internal class <Module>
{
	// Token: 0x06000001 RID: 1 RVA: 0x00012424 File Offset: 0x00011824
	internal unsafe static void Term(CAtlComModule* A_0)
	{
		if (*A_0 != 0)
		{
			_ATL_OBJMAP_ENTRY30** ptr = *(A_0 + 8);
			if (ptr < *(A_0 + 12))
			{
				do
				{
					uint num = (uint)(*(int*)ptr);
					if (num != 0U)
					{
						_ATL_OBJMAP_ENTRY30* ptr2 = num;
						uint num2 = (uint)(*(int*)(ptr2 + 16 / sizeof(_ATL_OBJMAP_ENTRY30)));
						if (num2 != 0U)
						{
							uint num3 = num2;
							object obj = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), num3, *(*num3 + 8));
						}
						*(int*)(ptr2 + 16 / sizeof(_ATL_OBJMAP_ENTRY30)) = 0;
					}
					ptr += 4 / sizeof(_ATL_OBJMAP_ENTRY30*);
				}
				while (ptr < *(A_0 + 12));
			}
			<Module>.DeleteCriticalSection(A_0 + 16);
			*A_0 = 0;
		}
	}

	// Token: 0x06000002 RID: 2 RVA: 0x00012320 File Offset: 0x00011720
	internal static void ??__E_AtlReleaseManagedClassFactories@ATL@@YMXXZ()
	{
		<Module>._atexit_m(ldftn(?A0xea60aed3.??__F_AtlReleaseManagedClassFactories@ATL@@YMXXZ));
	}

	// Token: 0x06000003 RID: 3 RVA: 0x00019A24 File Offset: 0x00018E24
	internal static void ??__F_AtlReleaseManagedClassFactories@ATL@@YMXXZ()
	{
		<Module>.ATL.CAtlComModule.Term(ref <Module>.ATL._AtlComModule);
	}

	// Token: 0x06000004 RID: 4 RVA: 0x00012B10 File Offset: 0x00011F10
	internal unsafe static void Forget(CAutoCleanupBase<unsigned\u0020short\u0020const\u0020*>* A_0)
	{
		*(A_0 + 4) = 0;
	}

	// Token: 0x06000005 RID: 5 RVA: 0x00012AE8 File Offset: 0x00011EE8
	internal unsafe static ushort* Steal(CAutoCleanupBase<unsigned\u0020short\u0020const\u0020*>* A_0)
	{
		ushort* result = *(A_0 + 4);
		*(A_0 + 4) = 0;
		return result;
	}

	// Token: 0x06000006 RID: 6 RVA: 0x00012AB8 File Offset: 0x00011EB8
	[return: MarshalAs(UnmanagedType.U1)]
	internal unsafe static bool !(CAutoCleanupBase<unsigned\u0020short\u0020const\u0020*>* A_0)
	{
		ushort* ptr = 0;
		return calli(System.Byte modopt(System.Runtime.CompilerServices.CompilerMarshalOverride) modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr,System.UInt16 modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.IsConst) modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.IsImplicitlyDereferenced)), A_0, ref ptr, *(*A_0 + 28));
	}

	// Token: 0x06000007 RID: 7 RVA: 0x00012A88 File Offset: 0x00011E88
	[return: MarshalAs(UnmanagedType.U1)]
	internal unsafe static bool !=(CAutoCleanupBase<unsigned\u0020short\u0020const\u0020*>* A_0, ushort** p)
	{
		return (calli(System.Byte modopt(System.Runtime.CompilerServices.CompilerMarshalOverride) modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr,System.UInt16 modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.IsConst) modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.IsImplicitlyDereferenced)), A_0, p, *(*A_0 + 28)) == 0) ? 1 : 0;
	}

	// Token: 0x06000008 RID: 8 RVA: 0x00012A60 File Offset: 0x00011E60
	[return: MarshalAs(UnmanagedType.U1)]
	internal unsafe static bool ==(CAutoCleanupBase<unsigned\u0020short\u0020const\u0020*>* A_0, ushort** p)
	{
		return (*(A_0 + 4) == *p) ? 1 : 0;
	}

	// Token: 0x06000009 RID: 9 RVA: 0x00012A0C File Offset: 0x00011E0C
	internal unsafe static ushort* QBG(CAutoCleanupBase<unsigned\u0020short\u0020const\u0020*>* A_0)
	{
		return *(A_0 + 4);
	}

	// Token: 0x0600000A RID: 10 RVA: 0x000129E8 File Offset: 0x00011DE8
	internal unsafe static ushort* PBG(CAutoCleanupBase<unsigned\u0020short\u0020const\u0020*>* A_0)
	{
		return *(A_0 + 4);
	}

	// Token: 0x0600000B RID: 11 RVA: 0x000129A0 File Offset: 0x00011DA0
	internal unsafe static ushort* ->(CAutoCleanupBase<unsigned\u0020short\u0020const\u0020*>* A_0)
	{
		return *(A_0 + 4);
	}

	// Token: 0x0600000C RID: 12 RVA: 0x000129C4 File Offset: 0x00011DC4
	internal unsafe static ushort* ->(CAutoCleanupBase<unsigned\u0020short\u0020const\u0020*>* A_0)
	{
		return *(A_0 + 4);
	}

	// Token: 0x0600000D RID: 13 RVA: 0x00012960 File Offset: 0x00011D60
	internal unsafe static ushort** &(CAutoCleanupBase<unsigned\u0020short\u0020const\u0020*>* A_0)
	{
		return A_0 + 4;
	}

	// Token: 0x0600000E RID: 14 RVA: 0x00012980 File Offset: 0x00011D80
	internal unsafe static ushort** &(CAutoCleanupBase<unsigned\u0020short\u0020const\u0020*>* A_0)
	{
		return A_0 + 4;
	}

	// Token: 0x0600000F RID: 15 RVA: 0x00012A30 File Offset: 0x00011E30
	internal unsafe static void =(CAutoComFree<unsigned\u0020short\u0020const\u0020*>* A_0, ushort* p)
	{
		calli(System.Void modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), A_0, *(*A_0 + 48));
		*(A_0 + 4) = p;
	}

	// Token: 0x06000010 RID: 16 RVA: 0x00012784 File Offset: 0x00011B84
	internal unsafe static void Release(CAutoComFree<unsigned\u0020short\u0020const\u0020*>* A_0)
	{
		uint num = (uint)(*(A_0 + 4));
		if (num != 0U)
		{
			<Module>.CoTaskMemFree(num);
			*(A_0 + 4) = 0;
		}
	}

	// Token: 0x06000011 RID: 17 RVA: 0x000127AC File Offset: 0x00011BAC
	internal unsafe static void {dtor}(CAutoComFree<unsigned\u0020short\u0020const\u0020*>* A_0)
	{
		*A_0 = ref <Module>.??_7?$CAutoComFree@PBG@RAII@@6B@;
		uint num = (uint)(*(A_0 + 4));
		if (num != 0U)
		{
			<Module>.CoTaskMemFree(num);
			*(A_0 + 4) = 0;
		}
	}

	// Token: 0x06000012 RID: 18 RVA: 0x00012B34 File Offset: 0x00011F34
	internal unsafe static void* __vecDelDtor(CAutoComFree<unsigned\u0020short\u0020const\u0020*>* A_0, uint A_0)
	{
		if ((A_0 & 2U) != 0U)
		{
			CAutoComFree<unsigned\u0020short\u0020const\u0020*>* ptr = A_0 - 4;
			<Module>.__ehvec_dtor(A_0, 8U, (uint)(*ptr), ldftn(RAII.CAutoComFree<unsigned\u0020short\u0020const\u0020*>.{dtor}));
			if ((A_0 & 1U) != 0U)
			{
				<Module>.delete[](ptr);
			}
			return ptr;
		}
		*A_0 = ref <Module>.??_7?$CAutoComFree@PBG@RAII@@6B@;
		uint num = (uint)(*(A_0 + 4));
		if (num != 0U)
		{
			<Module>.CoTaskMemFree(num);
			*(A_0 + 4) = 0;
		}
		if ((A_0 & 1U) != 0U)
		{
			<Module>.delete(A_0);
		}
		return A_0;
	}

	// Token: 0x06000013 RID: 19 RVA: 0x00012F24 File Offset: 0x00012324
	internal unsafe static CDeviceNotificationCallbackShim* {ctor}(CDeviceNotificationCallbackShim* A_0, DeviceNotificationCallback Callback)
	{
		*A_0 = ref <Module>.??_7CDeviceNotificationCallbackShim@@6B@;
		*(A_0 + 4) = ((IntPtr)GCHandle.Alloc(Callback)).ToPointer();
		return A_0;
	}

	// Token: 0x06000014 RID: 20 RVA: 0x00012F58 File Offset: 0x00012358
	internal unsafe static void Connected(CDeviceNotificationCallbackShim* A_0, ushort* DevicePath)
	{
		IntPtr ptr = (IntPtr)((void*)DevicePath);
		IntPtr value = new IntPtr(*(A_0 + 4));
		((GCHandle)value).Target.Connected(Marshal.PtrToStringUni(ptr));
	}

	// Token: 0x06000015 RID: 21 RVA: 0x00012FA0 File Offset: 0x000123A0
	internal unsafe static void Disconnected(CDeviceNotificationCallbackShim* A_0, ushort* DevicePath)
	{
		IntPtr ptr = (IntPtr)((void*)DevicePath);
		IntPtr value = new IntPtr(*(A_0 + 4));
		((GCHandle)value).Target.Disconnected(Marshal.PtrToStringUni(ptr));
	}

	// Token: 0x06000016 RID: 22 RVA: 0x00012FE8 File Offset: 0x000123E8
	internal unsafe static void RegisterProgress(CGenericProgressShim* A_0, uint Progress)
	{
		IntPtr value = new IntPtr(*(A_0 + 4));
		((GCHandle)value).Target.RegisterProgress(Progress);
	}

	// Token: 0x06000017 RID: 23 RVA: 0x00015C78 File Offset: 0x00015078
	internal unsafe static void Forget(CAutoCleanupBase<CGenericProgressShim\u0020*>* A_0)
	{
		*(A_0 + 4) = 0;
	}

	// Token: 0x06000018 RID: 24 RVA: 0x00014944 File Offset: 0x00013D44
	internal unsafe static CGenericProgressShim* Steal(CAutoCleanupBase<CGenericProgressShim\u0020*>* A_0)
	{
		CGenericProgressShim* result = *(A_0 + 4);
		*(A_0 + 4) = 0;
		return result;
	}

	// Token: 0x06000019 RID: 25 RVA: 0x00014E20 File Offset: 0x00014220
	[return: MarshalAs(UnmanagedType.U1)]
	internal unsafe static bool !(CAutoCleanupBase<CGenericProgressShim\u0020*>* A_0)
	{
		CGenericProgressShim* ptr = 0;
		return calli(System.Byte modopt(System.Runtime.CompilerServices.CompilerMarshalOverride) modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr,CGenericProgressShim* modopt(System.Runtime.CompilerServices.IsConst) modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.IsImplicitlyDereferenced)), A_0, ref ptr, *(*A_0 + 28));
	}

	// Token: 0x0600001A RID: 26 RVA: 0x00014DF0 File Offset: 0x000141F0
	[return: MarshalAs(UnmanagedType.U1)]
	internal unsafe static bool !=(CAutoCleanupBase<CGenericProgressShim\u0020*>* A_0, CGenericProgressShim** p)
	{
		return (calli(System.Byte modopt(System.Runtime.CompilerServices.CompilerMarshalOverride) modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr,CGenericProgressShim* modopt(System.Runtime.CompilerServices.IsConst) modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.IsImplicitlyDereferenced)), A_0, p, *(*A_0 + 28)) == 0) ? 1 : 0;
	}

	// Token: 0x0600001B RID: 27 RVA: 0x00014DC8 File Offset: 0x000141C8
	[return: MarshalAs(UnmanagedType.U1)]
	internal unsafe static bool ==(CAutoCleanupBase<CGenericProgressShim\u0020*>* A_0, CGenericProgressShim** p)
	{
		return (*(A_0 + 4) == *p) ? 1 : 0;
	}

	// Token: 0x0600001C RID: 28 RVA: 0x00014D9C File Offset: 0x0001419C
	internal unsafe static CGenericProgressShim* QAVCGenericProgressShim@@(CAutoCleanupBase<CGenericProgressShim\u0020*>* A_0)
	{
		return *(A_0 + 4);
	}

	// Token: 0x0600001D RID: 29 RVA: 0x00014D9C File Offset: 0x0001419C
	internal unsafe static CGenericProgressShim* PAVCGenericProgressShim@@(CAutoCleanupBase<CGenericProgressShim\u0020*>* A_0)
	{
		return *(A_0 + 4);
	}

	// Token: 0x0600001E RID: 30 RVA: 0x00014D9C File Offset: 0x0001419C
	internal unsafe static CGenericProgressShim* ->(CAutoCleanupBase<CGenericProgressShim\u0020*>* A_0)
	{
		return *(A_0 + 4);
	}

	// Token: 0x0600001F RID: 31 RVA: 0x00014D9C File Offset: 0x0001419C
	internal unsafe static CGenericProgressShim* ->(CAutoCleanupBase<CGenericProgressShim\u0020*>* A_0)
	{
		return *(A_0 + 4);
	}

	// Token: 0x06000020 RID: 32 RVA: 0x00015BA0 File Offset: 0x00014FA0
	internal unsafe static CGenericProgressShim** &(CAutoCleanupBase<CGenericProgressShim\u0020*>* A_0)
	{
		return A_0 + 4;
	}

	// Token: 0x06000021 RID: 33 RVA: 0x00015BA0 File Offset: 0x00014FA0
	internal unsafe static CGenericProgressShim** &(CAutoCleanupBase<CGenericProgressShim\u0020*>* A_0)
	{
		return A_0 + 4;
	}

	// Token: 0x06000022 RID: 34 RVA: 0x00014E64 File Offset: 0x00014264
	internal unsafe static void =(CAutoDelete<CGenericProgressShim\u0020*>* A_0, CGenericProgressShim* p)
	{
		calli(System.Void modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), A_0, *(*A_0 + 48));
		*(A_0 + 4) = p;
	}

	// Token: 0x06000023 RID: 35 RVA: 0x00013F10 File Offset: 0x00013310
	internal unsafe static void Release(CAutoDelete<CGenericProgressShim\u0020*>* A_0)
	{
		uint num = (uint)(*(A_0 + 4));
		if (num != 0U)
		{
			CGenericProgressShim* ptr = num;
			<Module>.gcroot<Microsoft::Windows::Flashing::Platform::GenericProgress\u0020^>.{dtor}(ptr + 4 / sizeof(CGenericProgressShim));
			<Module>.delete((void*)ptr);
			*(A_0 + 4) = 0;
		}
	}

	// Token: 0x06000024 RID: 36 RVA: 0x00013F44 File Offset: 0x00013344
	internal unsafe static void {dtor}(CAutoDelete<CGenericProgressShim\u0020*>* A_0)
	{
		*A_0 = ref <Module>.??_7?$CAutoDelete@PAVCGenericProgressShim@@@RAII@@6B@;
		<Module>.RAII.CAutoDelete<CGenericProgressShim\u0020*>.Release(A_0);
	}

	// Token: 0x06000025 RID: 37 RVA: 0x00014800 File Offset: 0x00013C00
	internal unsafe static void Forget(CAutoCleanupBase<FlashingPlatform::IFlashingDevice\u0020*>* A_0)
	{
		*(A_0 + 4) = 0;
	}

	// Token: 0x06000026 RID: 38 RVA: 0x000147D8 File Offset: 0x00013BD8
	internal unsafe static IFlashingDevice* Steal(CAutoCleanupBase<FlashingPlatform::IFlashingDevice\u0020*>* A_0)
	{
		IFlashingDevice* result = *(A_0 + 4);
		*(A_0 + 4) = 0;
		return result;
	}

	// Token: 0x06000027 RID: 39 RVA: 0x000147A8 File Offset: 0x00013BA8
	[return: MarshalAs(UnmanagedType.U1)]
	internal unsafe static bool !(CAutoCleanupBase<FlashingPlatform::IFlashingDevice\u0020*>* A_0)
	{
		IFlashingDevice* ptr = 0;
		return calli(System.Byte modopt(System.Runtime.CompilerServices.CompilerMarshalOverride) modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr,FlashingPlatform.IFlashingDevice* modopt(System.Runtime.CompilerServices.IsConst) modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.IsImplicitlyDereferenced)), A_0, ref ptr, *(*A_0 + 28));
	}

	// Token: 0x06000028 RID: 40 RVA: 0x00014778 File Offset: 0x00013B78
	[return: MarshalAs(UnmanagedType.U1)]
	internal unsafe static bool !=(CAutoCleanupBase<FlashingPlatform::IFlashingDevice\u0020*>* A_0, IFlashingDevice** p)
	{
		return (calli(System.Byte modopt(System.Runtime.CompilerServices.CompilerMarshalOverride) modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr,FlashingPlatform.IFlashingDevice* modopt(System.Runtime.CompilerServices.IsConst) modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.IsImplicitlyDereferenced)), A_0, p, *(*A_0 + 28)) == 0) ? 1 : 0;
	}

	// Token: 0x06000029 RID: 41 RVA: 0x00014750 File Offset: 0x00013B50
	[return: MarshalAs(UnmanagedType.U1)]
	internal unsafe static bool ==(CAutoCleanupBase<FlashingPlatform::IFlashingDevice\u0020*>* A_0, IFlashingDevice** p)
	{
		return (*(A_0 + 4) == *p) ? 1 : 0;
	}

	// Token: 0x0600002A RID: 42 RVA: 0x000146FC File Offset: 0x00013AFC
	internal unsafe static IFlashingDevice* QAUIFlashingDevice@FlashingPlatform@@(CAutoCleanupBase<FlashingPlatform::IFlashingDevice\u0020*>* A_0)
	{
		return *(A_0 + 4);
	}

	// Token: 0x0600002B RID: 43 RVA: 0x000146D8 File Offset: 0x00013AD8
	internal unsafe static IFlashingDevice* PAUIFlashingDevice@FlashingPlatform@@(CAutoCleanupBase<FlashingPlatform::IFlashingDevice\u0020*>* A_0)
	{
		return *(A_0 + 4);
	}

	// Token: 0x0600002C RID: 44 RVA: 0x00014690 File Offset: 0x00013A90
	internal unsafe static IFlashingDevice* ->(CAutoCleanupBase<FlashingPlatform::IFlashingDevice\u0020*>* A_0)
	{
		return *(A_0 + 4);
	}

	// Token: 0x0600002D RID: 45 RVA: 0x000146B4 File Offset: 0x00013AB4
	internal unsafe static IFlashingDevice* ->(CAutoCleanupBase<FlashingPlatform::IFlashingDevice\u0020*>* A_0)
	{
		return *(A_0 + 4);
	}

	// Token: 0x0600002E RID: 46 RVA: 0x00014650 File Offset: 0x00013A50
	internal unsafe static IFlashingDevice** &(CAutoCleanupBase<FlashingPlatform::IFlashingDevice\u0020*>* A_0)
	{
		return A_0 + 4;
	}

	// Token: 0x0600002F RID: 47 RVA: 0x00014670 File Offset: 0x00013A70
	internal unsafe static IFlashingDevice** &(CAutoCleanupBase<FlashingPlatform::IFlashingDevice\u0020*>* A_0)
	{
		return A_0 + 4;
	}

	// Token: 0x06000030 RID: 48 RVA: 0x00014720 File Offset: 0x00013B20
	internal unsafe static void =(CAutoRelease<FlashingPlatform::IFlashingDevice\u0020*>* A_0, IFlashingDevice* p)
	{
		calli(System.Void modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), A_0, *(*A_0 + 48));
		*(A_0 + 4) = p;
	}

	// Token: 0x06000031 RID: 49 RVA: 0x00012DB8 File Offset: 0x000121B8
	internal unsafe static void Release(CAutoRelease<FlashingPlatform::IFlashingDevice\u0020*>* A_0)
	{
		uint num = (uint)(*(A_0 + 4));
		if (num != 0U)
		{
			uint num2 = num;
			object obj = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), num2, *(*num2 + 20));
			*(A_0 + 4) = 0;
		}
	}

	// Token: 0x06000032 RID: 50 RVA: 0x00012DE4 File Offset: 0x000121E4
	internal unsafe static void {dtor}(CAutoRelease<FlashingPlatform::IFlashingDevice\u0020*>* A_0)
	{
		*A_0 = ref <Module>.??_7?$CAutoRelease@PAUIFlashingDevice@FlashingPlatform@@@RAII@@6B@;
		<Module>.RAII.CAutoRelease<FlashingPlatform::IFlashingDevice\u0020*>.Release(A_0);
	}

	// Token: 0x06000033 RID: 51 RVA: 0x00015C78 File Offset: 0x00015078
	internal unsafe static void Forget(CAutoCleanupBase<unsigned\u0020char\u0020*>* A_0)
	{
		*(A_0 + 4) = 0;
	}

	// Token: 0x06000034 RID: 52 RVA: 0x00014944 File Offset: 0x00013D44
	internal unsafe static byte* Steal(CAutoCleanupBase<unsigned\u0020char\u0020*>* A_0)
	{
		byte* result = *(A_0 + 4);
		*(A_0 + 4) = 0;
		return result;
	}

	// Token: 0x06000035 RID: 53 RVA: 0x00014914 File Offset: 0x00013D14
	[return: MarshalAs(UnmanagedType.U1)]
	internal unsafe static bool !(CAutoCleanupBase<unsigned\u0020char\u0020*>* A_0)
	{
		byte* ptr = 0;
		return calli(System.Byte modopt(System.Runtime.CompilerServices.CompilerMarshalOverride) modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr,System.Byte* modopt(System.Runtime.CompilerServices.IsConst) modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.IsImplicitlyDereferenced)), A_0, ref ptr, *(*A_0 + 28));
	}

	// Token: 0x06000036 RID: 54 RVA: 0x000148E4 File Offset: 0x00013CE4
	[return: MarshalAs(UnmanagedType.U1)]
	internal unsafe static bool !=(CAutoCleanupBase<unsigned\u0020char\u0020*>* A_0, byte** p)
	{
		return (calli(System.Byte modopt(System.Runtime.CompilerServices.CompilerMarshalOverride) modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr,System.Byte* modopt(System.Runtime.CompilerServices.IsConst) modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.IsImplicitlyDereferenced)), A_0, p, *(*A_0 + 28)) == 0) ? 1 : 0;
	}

	// Token: 0x06000037 RID: 55 RVA: 0x00014DC8 File Offset: 0x000141C8
	[return: MarshalAs(UnmanagedType.U1)]
	internal unsafe static bool ==(CAutoCleanupBase<unsigned\u0020char\u0020*>* A_0, byte** p)
	{
		return (*(A_0 + 4) == *p) ? 1 : 0;
	}

	// Token: 0x06000038 RID: 56 RVA: 0x00014D9C File Offset: 0x0001419C
	internal unsafe static byte* QAE(CAutoCleanupBase<unsigned\u0020char\u0020*>* A_0)
	{
		return *(A_0 + 4);
	}

	// Token: 0x06000039 RID: 57 RVA: 0x00014D9C File Offset: 0x0001419C
	internal unsafe static byte* PAE(CAutoCleanupBase<unsigned\u0020char\u0020*>* A_0)
	{
		return *(A_0 + 4);
	}

	// Token: 0x0600003A RID: 58 RVA: 0x00014D9C File Offset: 0x0001419C
	internal unsafe static byte* ->(CAutoCleanupBase<unsigned\u0020char\u0020*>* A_0)
	{
		return *(A_0 + 4);
	}

	// Token: 0x0600003B RID: 59 RVA: 0x00014D9C File Offset: 0x0001419C
	internal unsafe static byte* ->(CAutoCleanupBase<unsigned\u0020char\u0020*>* A_0)
	{
		return *(A_0 + 4);
	}

	// Token: 0x0600003C RID: 60 RVA: 0x00015BA0 File Offset: 0x00014FA0
	internal unsafe static byte** &(CAutoCleanupBase<unsigned\u0020char\u0020*>* A_0)
	{
		return A_0 + 4;
	}

	// Token: 0x0600003D RID: 61 RVA: 0x00015BA0 File Offset: 0x00014FA0
	internal unsafe static byte** &(CAutoCleanupBase<unsigned\u0020char\u0020*>* A_0)
	{
		return A_0 + 4;
	}

	// Token: 0x0600003E RID: 62 RVA: 0x00014E64 File Offset: 0x00014264
	internal unsafe static void =(CAutoDeleteArray<unsigned\u0020char>* A_0, byte* p)
	{
		calli(System.Void modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), A_0, *(*A_0 + 48));
		*(A_0 + 4) = p;
	}

	// Token: 0x0600003F RID: 63 RVA: 0x00012E04 File Offset: 0x00012204
	internal unsafe static void Release(CAutoDeleteArray<unsigned\u0020char>* A_0)
	{
		uint num = (uint)(*(A_0 + 4));
		if (num != 0U)
		{
			<Module>.delete[](num);
			*(A_0 + 4) = 0;
		}
	}

	// Token: 0x06000040 RID: 64 RVA: 0x00014D9C File Offset: 0x0001419C
	internal unsafe static byte* P(CAutoDeleteArray<unsigned\u0020char>* A_0)
	{
		return *(A_0 + 4);
	}

	// Token: 0x06000041 RID: 65 RVA: 0x00014D9C File Offset: 0x0001419C
	internal unsafe static byte* P(CAutoDeleteArray<unsigned\u0020char>* A_0)
	{
		return *(A_0 + 4);
	}

	// Token: 0x06000042 RID: 66 RVA: 0x000149D8 File Offset: 0x00013DD8
	internal unsafe static byte* [](CAutoDeleteArray<unsigned\u0020char>* A_0, uint i)
	{
		return *(A_0 + 4) + (int)i;
	}

	// Token: 0x06000043 RID: 67 RVA: 0x000149D8 File Offset: 0x00013DD8
	internal unsafe static byte* [](CAutoDeleteArray<unsigned\u0020char>* A_0, uint i)
	{
		return *(A_0 + 4) + (int)i;
	}

	// Token: 0x06000044 RID: 68 RVA: 0x00012E2C File Offset: 0x0001222C
	internal unsafe static void {dtor}(CAutoDeleteArray<unsigned\u0020char>* A_0)
	{
		*A_0 = ref <Module>.??_7?$CAutoDeleteArray@E@RAII@@6B@;
		uint num = (uint)(*(A_0 + 4));
		if (num != 0U)
		{
			<Module>.delete[](num);
			*(A_0 + 4) = 0;
		}
	}

	// Token: 0x06000045 RID: 69 RVA: 0x00014A1C File Offset: 0x00013E1C
	internal unsafe static void =(CAutoDeleteArray<unsigned\u0020short\u0020const\u0020>* A_0, ushort* p)
	{
		calli(System.Void modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), A_0, *(*A_0 + 48));
		*(A_0 + 4) = p;
	}

	// Token: 0x06000046 RID: 70 RVA: 0x00012E58 File Offset: 0x00012258
	internal unsafe static void Release(CAutoDeleteArray<unsigned\u0020short\u0020const\u0020>* A_0)
	{
		uint num = (uint)(*(A_0 + 4));
		if (num != 0U)
		{
			<Module>.delete[](num);
			*(A_0 + 4) = 0;
		}
	}

	// Token: 0x06000047 RID: 71 RVA: 0x00014AF0 File Offset: 0x00013EF0
	internal unsafe static ushort* P(CAutoDeleteArray<unsigned\u0020short\u0020const\u0020>* A_0)
	{
		return *(A_0 + 4);
	}

	// Token: 0x06000048 RID: 72 RVA: 0x00014B14 File Offset: 0x00013F14
	internal unsafe static ushort* P(CAutoDeleteArray<unsigned\u0020short\u0020const\u0020>* A_0)
	{
		return *(A_0 + 4);
	}

	// Token: 0x06000049 RID: 73 RVA: 0x00014AA0 File Offset: 0x00013EA0
	internal unsafe static ushort* [](CAutoDeleteArray<unsigned\u0020short\u0020const\u0020>* A_0, uint i)
	{
		return i * 2U + (uint)(*(A_0 + 4));
	}

	// Token: 0x0600004A RID: 74 RVA: 0x00014AC8 File Offset: 0x00013EC8
	internal unsafe static ushort* [](CAutoDeleteArray<unsigned\u0020short\u0020const\u0020>* A_0, uint i)
	{
		return i * 2U + (uint)(*(A_0 + 4));
	}

	// Token: 0x0600004B RID: 75 RVA: 0x00012E80 File Offset: 0x00012280
	internal unsafe static void {dtor}(CAutoDeleteArray<unsigned\u0020short\u0020const\u0020>* A_0)
	{
		*A_0 = ref <Module>.??_7?$CAutoDeleteArray@$$CBG@RAII@@6B@;
		uint num = (uint)(*(A_0 + 4));
		if (num != 0U)
		{
			<Module>.delete[](num);
			*(A_0 + 4) = 0;
		}
	}

	// Token: 0x0600004C RID: 76 RVA: 0x00014CF0 File Offset: 0x000140F0
	internal unsafe static void Forget(CAutoCleanupBase<FlashingPlatform::IConnectedDevice\u0020*>* A_0)
	{
		*(A_0 + 4) = 0;
	}

	// Token: 0x0600004D RID: 77 RVA: 0x00014CC8 File Offset: 0x000140C8
	internal unsafe static IConnectedDevice* Steal(CAutoCleanupBase<FlashingPlatform::IConnectedDevice\u0020*>* A_0)
	{
		IConnectedDevice* result = *(A_0 + 4);
		*(A_0 + 4) = 0;
		return result;
	}

	// Token: 0x0600004E RID: 78 RVA: 0x00014C98 File Offset: 0x00014098
	[return: MarshalAs(UnmanagedType.U1)]
	internal unsafe static bool !(CAutoCleanupBase<FlashingPlatform::IConnectedDevice\u0020*>* A_0)
	{
		IConnectedDevice* ptr = 0;
		return calli(System.Byte modopt(System.Runtime.CompilerServices.CompilerMarshalOverride) modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr,FlashingPlatform.IConnectedDevice* modopt(System.Runtime.CompilerServices.IsConst) modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.IsImplicitlyDereferenced)), A_0, ref ptr, *(*A_0 + 28));
	}

	// Token: 0x0600004F RID: 79 RVA: 0x00014C68 File Offset: 0x00014068
	[return: MarshalAs(UnmanagedType.U1)]
	internal unsafe static bool !=(CAutoCleanupBase<FlashingPlatform::IConnectedDevice\u0020*>* A_0, IConnectedDevice** p)
	{
		return (calli(System.Byte modopt(System.Runtime.CompilerServices.CompilerMarshalOverride) modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr,FlashingPlatform.IConnectedDevice* modopt(System.Runtime.CompilerServices.IsConst) modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.IsImplicitlyDereferenced)), A_0, p, *(*A_0 + 28)) == 0) ? 1 : 0;
	}

	// Token: 0x06000050 RID: 80 RVA: 0x00014C40 File Offset: 0x00014040
	[return: MarshalAs(UnmanagedType.U1)]
	internal unsafe static bool ==(CAutoCleanupBase<FlashingPlatform::IConnectedDevice\u0020*>* A_0, IConnectedDevice** p)
	{
		return (*(A_0 + 4) == *p) ? 1 : 0;
	}

	// Token: 0x06000051 RID: 81 RVA: 0x00014BEC File Offset: 0x00013FEC
	internal unsafe static IConnectedDevice* QAUIConnectedDevice@FlashingPlatform@@(CAutoCleanupBase<FlashingPlatform::IConnectedDevice\u0020*>* A_0)
	{
		return *(A_0 + 4);
	}

	// Token: 0x06000052 RID: 82 RVA: 0x00014BC8 File Offset: 0x00013FC8
	internal unsafe static IConnectedDevice* PAUIConnectedDevice@FlashingPlatform@@(CAutoCleanupBase<FlashingPlatform::IConnectedDevice\u0020*>* A_0)
	{
		return *(A_0 + 4);
	}

	// Token: 0x06000053 RID: 83 RVA: 0x00014B80 File Offset: 0x00013F80
	internal unsafe static IConnectedDevice* ->(CAutoCleanupBase<FlashingPlatform::IConnectedDevice\u0020*>* A_0)
	{
		return *(A_0 + 4);
	}

	// Token: 0x06000054 RID: 84 RVA: 0x00014BA4 File Offset: 0x00013FA4
	internal unsafe static IConnectedDevice* ->(CAutoCleanupBase<FlashingPlatform::IConnectedDevice\u0020*>* A_0)
	{
		return *(A_0 + 4);
	}

	// Token: 0x06000055 RID: 85 RVA: 0x00014B40 File Offset: 0x00013F40
	internal unsafe static IConnectedDevice** &(CAutoCleanupBase<FlashingPlatform::IConnectedDevice\u0020*>* A_0)
	{
		return A_0 + 4;
	}

	// Token: 0x06000056 RID: 86 RVA: 0x00014B60 File Offset: 0x00013F60
	internal unsafe static IConnectedDevice** &(CAutoCleanupBase<FlashingPlatform::IConnectedDevice\u0020*>* A_0)
	{
		return A_0 + 4;
	}

	// Token: 0x06000057 RID: 87 RVA: 0x00014C10 File Offset: 0x00014010
	internal unsafe static void =(CAutoRelease<FlashingPlatform::IConnectedDevice\u0020*>* A_0, IConnectedDevice* p)
	{
		calli(System.Void modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), A_0, *(*A_0 + 48));
		*(A_0 + 4) = p;
	}

	// Token: 0x06000058 RID: 88 RVA: 0x00012EAC File Offset: 0x000122AC
	internal unsafe static void Release(CAutoRelease<FlashingPlatform::IConnectedDevice\u0020*>* A_0)
	{
		uint num = (uint)(*(A_0 + 4));
		if (num != 0U)
		{
			uint num2 = num;
			object obj = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), num2, *(*num2 + 20));
			*(A_0 + 4) = 0;
		}
	}

	// Token: 0x06000059 RID: 89 RVA: 0x00012ED8 File Offset: 0x000122D8
	internal unsafe static void {dtor}(CAutoRelease<FlashingPlatform::IConnectedDevice\u0020*>* A_0)
	{
		*A_0 = ref <Module>.??_7?$CAutoRelease@PAUIConnectedDevice@FlashingPlatform@@@RAII@@6B@;
		<Module>.RAII.CAutoRelease<FlashingPlatform::IConnectedDevice\u0020*>.Release(A_0);
	}

	// Token: 0x0600005A RID: 90 RVA: 0x000162A8 File Offset: 0x000156A8
	internal unsafe static void {dtor}(gcroot<Microsoft::Windows::Flashing::Platform::GenericProgress\u0020^>* A_0)
	{
		IntPtr value = new IntPtr(*A_0);
		((GCHandle)value).Free();
		*A_0 = 0;
	}

	// Token: 0x0600005B RID: 91 RVA: 0x00012EF8 File Offset: 0x000122F8
	internal unsafe static void {dtor}(gcroot<Microsoft::Windows::Flashing::Platform::DeviceNotificationCallback\u0020^>* A_0)
	{
		IntPtr value = new IntPtr(*A_0);
		((GCHandle)value).Free();
		*A_0 = 0;
	}

	// Token: 0x0600005C RID: 92 RVA: 0x00014EA0 File Offset: 0x000142A0
	internal unsafe static void* __vecDelDtor(CAutoDelete<CGenericProgressShim\u0020*>* A_0, uint A_0)
	{
		if ((A_0 & 2U) != 0U)
		{
			CAutoDelete<CGenericProgressShim\u0020*>* ptr = A_0 - 4;
			<Module>.__ehvec_dtor(A_0, 8U, (uint)(*ptr), ldftn(RAII.CAutoDelete<CGenericProgressShim\u0020*>.{dtor}));
			if ((A_0 & 1U) != 0U)
			{
				<Module>.delete[](ptr);
			}
			return ptr;
		}
		*A_0 = ref <Module>.??_7?$CAutoDelete@PAVCGenericProgressShim@@@RAII@@6B@;
		<Module>.RAII.CAutoDelete<CGenericProgressShim\u0020*>.Release(A_0);
		if ((A_0 & 1U) != 0U)
		{
			<Module>.delete(A_0);
		}
		return A_0;
	}

	// Token: 0x0600005D RID: 93 RVA: 0x00014824 File Offset: 0x00013C24
	internal unsafe static void* __vecDelDtor(CAutoRelease<FlashingPlatform::IFlashingDevice\u0020*>* A_0, uint A_0)
	{
		if ((A_0 & 2U) != 0U)
		{
			CAutoRelease<FlashingPlatform::IFlashingDevice\u0020*>* ptr = A_0 - 4;
			<Module>.__ehvec_dtor(A_0, 8U, (uint)(*ptr), ldftn(RAII.CAutoRelease<FlashingPlatform::IFlashingDevice\u0020*>.{dtor}));
			if ((A_0 & 1U) != 0U)
			{
				<Module>.delete[](ptr);
			}
			return ptr;
		}
		*A_0 = ref <Module>.??_7?$CAutoRelease@PAUIFlashingDevice@FlashingPlatform@@@RAII@@6B@;
		<Module>.RAII.CAutoRelease<FlashingPlatform::IFlashingDevice\u0020*>.Release(A_0);
		if ((A_0 & 1U) != 0U)
		{
			<Module>.delete(A_0);
		}
		return A_0;
	}

	// Token: 0x0600005E RID: 94 RVA: 0x00014978 File Offset: 0x00013D78
	internal unsafe static void* __vecDelDtor(CAutoDeleteArray<unsigned\u0020char>* A_0, uint A_0)
	{
		if ((A_0 & 2U) != 0U)
		{
			CAutoDeleteArray<unsigned\u0020char>* ptr = A_0 - 4;
			<Module>.__ehvec_dtor(A_0, 8U, (uint)(*ptr), ldftn(RAII.CAutoDeleteArray<unsigned\u0020char>.{dtor}));
			if ((A_0 & 1U) != 0U)
			{
				<Module>.delete[](ptr);
			}
			return ptr;
		}
		<Module>.RAII.CAutoDeleteArray<unsigned\u0020char>.{dtor}(A_0);
		if ((A_0 & 1U) != 0U)
		{
			<Module>.delete(A_0);
		}
		return A_0;
	}

	// Token: 0x0600005F RID: 95 RVA: 0x00014A4C File Offset: 0x00013E4C
	internal unsafe static void* __vecDelDtor(CAutoDeleteArray<unsigned\u0020short\u0020const\u0020>* A_0, uint A_0)
	{
		if ((A_0 & 2U) != 0U)
		{
			CAutoDeleteArray<unsigned\u0020short\u0020const\u0020>* ptr = A_0 - 4;
			<Module>.__ehvec_dtor(A_0, 8U, (uint)(*ptr), ldftn(RAII.CAutoDeleteArray<unsigned\u0020short\u0020const\u0020>.{dtor}));
			if ((A_0 & 1U) != 0U)
			{
				<Module>.delete[](ptr);
			}
			return ptr;
		}
		<Module>.RAII.CAutoDeleteArray<unsigned\u0020short\u0020const\u0020>.{dtor}(A_0);
		if ((A_0 & 1U) != 0U)
		{
			<Module>.delete(A_0);
		}
		return A_0;
	}

	// Token: 0x06000060 RID: 96 RVA: 0x00014D14 File Offset: 0x00014114
	internal unsafe static void* __vecDelDtor(CAutoRelease<FlashingPlatform::IConnectedDevice\u0020*>* A_0, uint A_0)
	{
		if ((A_0 & 2U) != 0U)
		{
			CAutoRelease<FlashingPlatform::IConnectedDevice\u0020*>* ptr = A_0 - 4;
			<Module>.__ehvec_dtor(A_0, 8U, (uint)(*ptr), ldftn(RAII.CAutoRelease<FlashingPlatform::IConnectedDevice\u0020*>.{dtor}));
			if ((A_0 & 1U) != 0U)
			{
				<Module>.delete[](ptr);
			}
			return ptr;
		}
		*A_0 = ref <Module>.??_7?$CAutoRelease@PAUIConnectedDevice@FlashingPlatform@@@RAII@@6B@;
		<Module>.RAII.CAutoRelease<FlashingPlatform::IConnectedDevice\u0020*>.Release(A_0);
		if ((A_0 & 1U) != 0U)
		{
			<Module>.delete(A_0);
		}
		return A_0;
	}

	// Token: 0x06000061 RID: 97 RVA: 0x00015C78 File Offset: 0x00015078
	internal unsafe static void Forget(CAutoCleanupBase<CDeviceNotificationCallbackShim\u0020*>* A_0)
	{
		*(A_0 + 4) = 0;
	}

	// Token: 0x06000062 RID: 98 RVA: 0x00014944 File Offset: 0x00013D44
	internal unsafe static CDeviceNotificationCallbackShim* Steal(CAutoCleanupBase<CDeviceNotificationCallbackShim\u0020*>* A_0)
	{
		CDeviceNotificationCallbackShim* result = *(A_0 + 4);
		*(A_0 + 4) = 0;
		return result;
	}

	// Token: 0x06000063 RID: 99 RVA: 0x00015EA8 File Offset: 0x000152A8
	[return: MarshalAs(UnmanagedType.U1)]
	internal unsafe static bool !(CAutoCleanupBase<CDeviceNotificationCallbackShim\u0020*>* A_0)
	{
		CDeviceNotificationCallbackShim* ptr = 0;
		return calli(System.Byte modopt(System.Runtime.CompilerServices.CompilerMarshalOverride) modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr,CDeviceNotificationCallbackShim* modopt(System.Runtime.CompilerServices.IsConst) modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.IsImplicitlyDereferenced)), A_0, ref ptr, *(*A_0 + 28));
	}

	// Token: 0x06000064 RID: 100 RVA: 0x00015E78 File Offset: 0x00015278
	[return: MarshalAs(UnmanagedType.U1)]
	internal unsafe static bool !=(CAutoCleanupBase<CDeviceNotificationCallbackShim\u0020*>* A_0, CDeviceNotificationCallbackShim** p)
	{
		return (calli(System.Byte modopt(System.Runtime.CompilerServices.CompilerMarshalOverride) modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr,CDeviceNotificationCallbackShim* modopt(System.Runtime.CompilerServices.IsConst) modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.IsImplicitlyDereferenced)), A_0, p, *(*A_0 + 28)) == 0) ? 1 : 0;
	}

	// Token: 0x06000065 RID: 101 RVA: 0x00014DC8 File Offset: 0x000141C8
	[return: MarshalAs(UnmanagedType.U1)]
	internal unsafe static bool ==(CAutoCleanupBase<CDeviceNotificationCallbackShim\u0020*>* A_0, CDeviceNotificationCallbackShim** p)
	{
		return (*(A_0 + 4) == *p) ? 1 : 0;
	}

	// Token: 0x06000066 RID: 102 RVA: 0x00014D9C File Offset: 0x0001419C
	internal unsafe static CDeviceNotificationCallbackShim* QAVCDeviceNotificationCallbackShim@@(CAutoCleanupBase<CDeviceNotificationCallbackShim\u0020*>* A_0)
	{
		return *(A_0 + 4);
	}

	// Token: 0x06000067 RID: 103 RVA: 0x00014D9C File Offset: 0x0001419C
	internal unsafe static CDeviceNotificationCallbackShim* PAVCDeviceNotificationCallbackShim@@(CAutoCleanupBase<CDeviceNotificationCallbackShim\u0020*>* A_0)
	{
		return *(A_0 + 4);
	}

	// Token: 0x06000068 RID: 104 RVA: 0x00014D9C File Offset: 0x0001419C
	internal unsafe static CDeviceNotificationCallbackShim* ->(CAutoCleanupBase<CDeviceNotificationCallbackShim\u0020*>* A_0)
	{
		return *(A_0 + 4);
	}

	// Token: 0x06000069 RID: 105 RVA: 0x00014D9C File Offset: 0x0001419C
	internal unsafe static CDeviceNotificationCallbackShim* ->(CAutoCleanupBase<CDeviceNotificationCallbackShim\u0020*>* A_0)
	{
		return *(A_0 + 4);
	}

	// Token: 0x0600006A RID: 106 RVA: 0x00015BA0 File Offset: 0x00014FA0
	internal unsafe static CDeviceNotificationCallbackShim** &(CAutoCleanupBase<CDeviceNotificationCallbackShim\u0020*>* A_0)
	{
		return A_0 + 4;
	}

	// Token: 0x0600006B RID: 107 RVA: 0x00015BA0 File Offset: 0x00014FA0
	internal unsafe static CDeviceNotificationCallbackShim** &(CAutoCleanupBase<CDeviceNotificationCallbackShim\u0020*>* A_0)
	{
		return A_0 + 4;
	}

	// Token: 0x0600006C RID: 108 RVA: 0x00014E64 File Offset: 0x00014264
	internal unsafe static void =(CAutoDelete<CDeviceNotificationCallbackShim\u0020*>* A_0, CDeviceNotificationCallbackShim* p)
	{
		calli(System.Void modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), A_0, *(*A_0 + 48));
		*(A_0 + 4) = p;
	}

	// Token: 0x0600006D RID: 109 RVA: 0x000158E4 File Offset: 0x00014CE4
	internal unsafe static void Release(CAutoDelete<CDeviceNotificationCallbackShim\u0020*>* A_0)
	{
		uint num = (uint)(*(A_0 + 4));
		if (num != 0U)
		{
			CDeviceNotificationCallbackShim* ptr = num;
			<Module>.gcroot<Microsoft::Windows::Flashing::Platform::DeviceNotificationCallback\u0020^>.{dtor}(ptr + 4 / sizeof(CDeviceNotificationCallbackShim));
			<Module>.delete((void*)ptr);
			*(A_0 + 4) = 0;
		}
	}

	// Token: 0x0600006E RID: 110 RVA: 0x00015918 File Offset: 0x00014D18
	internal unsafe static void {dtor}(CAutoDelete<CDeviceNotificationCallbackShim\u0020*>* A_0)
	{
		*A_0 = ref <Module>.??_7?$CAutoDelete@PAVCDeviceNotificationCallbackShim@@@RAII@@6B@;
		<Module>.RAII.CAutoDelete<CDeviceNotificationCallbackShim\u0020*>.Release(A_0);
	}

	// Token: 0x0600006F RID: 111 RVA: 0x00015C78 File Offset: 0x00015078
	internal unsafe static void Forget(CAutoCleanupBase<FlashingPlatform::IConnectedDeviceCollection\u0020*>* A_0)
	{
		*(A_0 + 4) = 0;
	}

	// Token: 0x06000070 RID: 112 RVA: 0x00014944 File Offset: 0x00013D44
	internal unsafe static IConnectedDeviceCollection* Steal(CAutoCleanupBase<FlashingPlatform::IConnectedDeviceCollection\u0020*>* A_0)
	{
		IConnectedDeviceCollection* result = *(A_0 + 4);
		*(A_0 + 4) = 0;
		return result;
	}

	// Token: 0x06000071 RID: 113 RVA: 0x00015C40 File Offset: 0x00015040
	[return: MarshalAs(UnmanagedType.U1)]
	internal unsafe static bool !(CAutoCleanupBase<FlashingPlatform::IConnectedDeviceCollection\u0020*>* A_0)
	{
		IConnectedDeviceCollection* ptr = 0;
		return calli(System.Byte modopt(System.Runtime.CompilerServices.CompilerMarshalOverride) modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr,FlashingPlatform.IConnectedDeviceCollection* modopt(System.Runtime.CompilerServices.IsConst) modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.IsImplicitlyDereferenced)), A_0, ref ptr, *(*A_0 + 28));
	}

	// Token: 0x06000072 RID: 114 RVA: 0x00015C10 File Offset: 0x00015010
	[return: MarshalAs(UnmanagedType.U1)]
	internal unsafe static bool !=(CAutoCleanupBase<FlashingPlatform::IConnectedDeviceCollection\u0020*>* A_0, IConnectedDeviceCollection** p)
	{
		return (calli(System.Byte modopt(System.Runtime.CompilerServices.CompilerMarshalOverride) modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr,FlashingPlatform.IConnectedDeviceCollection* modopt(System.Runtime.CompilerServices.IsConst) modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.IsImplicitlyDereferenced)), A_0, p, *(*A_0 + 28)) == 0) ? 1 : 0;
	}

	// Token: 0x06000073 RID: 115 RVA: 0x00014DC8 File Offset: 0x000141C8
	[return: MarshalAs(UnmanagedType.U1)]
	internal unsafe static bool ==(CAutoCleanupBase<FlashingPlatform::IConnectedDeviceCollection\u0020*>* A_0, IConnectedDeviceCollection** p)
	{
		return (*(A_0 + 4) == *p) ? 1 : 0;
	}

	// Token: 0x06000074 RID: 116 RVA: 0x00014D9C File Offset: 0x0001419C
	internal unsafe static IConnectedDeviceCollection* QAUIConnectedDeviceCollection@FlashingPlatform@@(CAutoCleanupBase<FlashingPlatform::IConnectedDeviceCollection\u0020*>* A_0)
	{
		return *(A_0 + 4);
	}

	// Token: 0x06000075 RID: 117 RVA: 0x00014D9C File Offset: 0x0001419C
	internal unsafe static IConnectedDeviceCollection* PAUIConnectedDeviceCollection@FlashingPlatform@@(CAutoCleanupBase<FlashingPlatform::IConnectedDeviceCollection\u0020*>* A_0)
	{
		return *(A_0 + 4);
	}

	// Token: 0x06000076 RID: 118 RVA: 0x00014D9C File Offset: 0x0001419C
	internal unsafe static IConnectedDeviceCollection* ->(CAutoCleanupBase<FlashingPlatform::IConnectedDeviceCollection\u0020*>* A_0)
	{
		return *(A_0 + 4);
	}

	// Token: 0x06000077 RID: 119 RVA: 0x00014D9C File Offset: 0x0001419C
	internal unsafe static IConnectedDeviceCollection* ->(CAutoCleanupBase<FlashingPlatform::IConnectedDeviceCollection\u0020*>* A_0)
	{
		return *(A_0 + 4);
	}

	// Token: 0x06000078 RID: 120 RVA: 0x00015BA0 File Offset: 0x00014FA0
	internal unsafe static IConnectedDeviceCollection** &(CAutoCleanupBase<FlashingPlatform::IConnectedDeviceCollection\u0020*>* A_0)
	{
		return A_0 + 4;
	}

	// Token: 0x06000079 RID: 121 RVA: 0x00015BA0 File Offset: 0x00014FA0
	internal unsafe static IConnectedDeviceCollection** &(CAutoCleanupBase<FlashingPlatform::IConnectedDeviceCollection\u0020*>* A_0)
	{
		return A_0 + 4;
	}

	// Token: 0x0600007A RID: 122 RVA: 0x00014E64 File Offset: 0x00014264
	internal unsafe static void =(CAutoRelease<FlashingPlatform::IConnectedDeviceCollection\u0020*>* A_0, IConnectedDeviceCollection* p)
	{
		calli(System.Void modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), A_0, *(*A_0 + 48));
		*(A_0 + 4) = p;
	}

	// Token: 0x0600007B RID: 123 RVA: 0x00014FAC File Offset: 0x000143AC
	internal unsafe static void Release(CAutoRelease<FlashingPlatform::IConnectedDeviceCollection\u0020*>* A_0)
	{
		uint num = (uint)(*(A_0 + 4));
		if (num != 0U)
		{
			uint num2 = num;
			object obj = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), num2, *(*num2 + 8));
			*(A_0 + 4) = 0;
		}
	}

	// Token: 0x0600007C RID: 124 RVA: 0x00014FD8 File Offset: 0x000143D8
	internal unsafe static void {dtor}(CAutoRelease<FlashingPlatform::IConnectedDeviceCollection\u0020*>* A_0)
	{
		*A_0 = ref <Module>.??_7?$CAutoRelease@PAUIConnectedDeviceCollection@FlashingPlatform@@@RAII@@6B@;
		<Module>.RAII.CAutoRelease<FlashingPlatform::IConnectedDeviceCollection\u0020*>.Release(A_0);
	}

	// Token: 0x0600007D RID: 125 RVA: 0x00015C78 File Offset: 0x00015078
	internal unsafe static void Forget(CAutoCleanupBase<FlashingPlatform::IFlashingPlatform\u0020*>* A_0)
	{
		*(A_0 + 4) = 0;
	}

	// Token: 0x0600007E RID: 126 RVA: 0x00014944 File Offset: 0x00013D44
	internal unsafe static IFlashingPlatform* Steal(CAutoCleanupBase<FlashingPlatform::IFlashingPlatform\u0020*>* A_0)
	{
		IFlashingPlatform* result = *(A_0 + 4);
		*(A_0 + 4) = 0;
		return result;
	}

	// Token: 0x0600007F RID: 127 RVA: 0x00015D8C File Offset: 0x0001518C
	[return: MarshalAs(UnmanagedType.U1)]
	internal unsafe static bool !(CAutoCleanupBase<FlashingPlatform::IFlashingPlatform\u0020*>* A_0)
	{
		IFlashingPlatform* ptr = 0;
		return calli(System.Byte modopt(System.Runtime.CompilerServices.CompilerMarshalOverride) modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr,FlashingPlatform.IFlashingPlatform* modopt(System.Runtime.CompilerServices.IsConst) modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.IsImplicitlyDereferenced)), A_0, ref ptr, *(*A_0 + 28));
	}

	// Token: 0x06000080 RID: 128 RVA: 0x00015D5C File Offset: 0x0001515C
	[return: MarshalAs(UnmanagedType.U1)]
	internal unsafe static bool !=(CAutoCleanupBase<FlashingPlatform::IFlashingPlatform\u0020*>* A_0, IFlashingPlatform** p)
	{
		return (calli(System.Byte modopt(System.Runtime.CompilerServices.CompilerMarshalOverride) modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr,FlashingPlatform.IFlashingPlatform* modopt(System.Runtime.CompilerServices.IsConst) modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.IsImplicitlyDereferenced)), A_0, p, *(*A_0 + 28)) == 0) ? 1 : 0;
	}

	// Token: 0x06000081 RID: 129 RVA: 0x00014DC8 File Offset: 0x000141C8
	[return: MarshalAs(UnmanagedType.U1)]
	internal unsafe static bool ==(CAutoCleanupBase<FlashingPlatform::IFlashingPlatform\u0020*>* A_0, IFlashingPlatform** p)
	{
		return (*(A_0 + 4) == *p) ? 1 : 0;
	}

	// Token: 0x06000082 RID: 130 RVA: 0x00014D9C File Offset: 0x0001419C
	internal unsafe static IFlashingPlatform* QAUIFlashingPlatform@FlashingPlatform@@(CAutoCleanupBase<FlashingPlatform::IFlashingPlatform\u0020*>* A_0)
	{
		return *(A_0 + 4);
	}

	// Token: 0x06000083 RID: 131 RVA: 0x00014D9C File Offset: 0x0001419C
	internal unsafe static IFlashingPlatform* PAUIFlashingPlatform@FlashingPlatform@@(CAutoCleanupBase<FlashingPlatform::IFlashingPlatform\u0020*>* A_0)
	{
		return *(A_0 + 4);
	}

	// Token: 0x06000084 RID: 132 RVA: 0x00014D9C File Offset: 0x0001419C
	internal unsafe static IFlashingPlatform* ->(CAutoCleanupBase<FlashingPlatform::IFlashingPlatform\u0020*>* A_0)
	{
		return *(A_0 + 4);
	}

	// Token: 0x06000085 RID: 133 RVA: 0x00014D9C File Offset: 0x0001419C
	internal unsafe static IFlashingPlatform* ->(CAutoCleanupBase<FlashingPlatform::IFlashingPlatform\u0020*>* A_0)
	{
		return *(A_0 + 4);
	}

	// Token: 0x06000086 RID: 134 RVA: 0x00015BA0 File Offset: 0x00014FA0
	internal unsafe static IFlashingPlatform** &(CAutoCleanupBase<FlashingPlatform::IFlashingPlatform\u0020*>* A_0)
	{
		return A_0 + 4;
	}

	// Token: 0x06000087 RID: 135 RVA: 0x00015BA0 File Offset: 0x00014FA0
	internal unsafe static IFlashingPlatform** &(CAutoCleanupBase<FlashingPlatform::IFlashingPlatform\u0020*>* A_0)
	{
		return A_0 + 4;
	}

	// Token: 0x06000088 RID: 136 RVA: 0x00014E64 File Offset: 0x00014264
	internal unsafe static void =(CAutoRelease<FlashingPlatform::IFlashingPlatform\u0020*>* A_0, IFlashingPlatform* p)
	{
		calli(System.Void modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), A_0, *(*A_0 + 48));
		*(A_0 + 4) = p;
	}

	// Token: 0x06000089 RID: 137 RVA: 0x00014FF8 File Offset: 0x000143F8
	internal unsafe static void Release(CAutoRelease<FlashingPlatform::IFlashingPlatform\u0020*>* A_0)
	{
		uint num = (uint)(*(A_0 + 4));
		if (num != 0U)
		{
			uint num2 = num;
			object obj = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), num2, *(*num2 + 28));
			*(A_0 + 4) = 0;
		}
	}

	// Token: 0x0600008A RID: 138 RVA: 0x00015024 File Offset: 0x00014424
	internal unsafe static void {dtor}(CAutoRelease<FlashingPlatform::IFlashingPlatform\u0020*>* A_0)
	{
		*A_0 = ref <Module>.??_7?$CAutoRelease@PAUIFlashingPlatform@FlashingPlatform@@@RAII@@6B@;
		<Module>.RAII.CAutoRelease<FlashingPlatform::IFlashingPlatform\u0020*>.Release(A_0);
	}

	// Token: 0x0600008B RID: 139 RVA: 0x00015F04 File Offset: 0x00015304
	internal unsafe static void* __vecDelDtor(CAutoDelete<CDeviceNotificationCallbackShim\u0020*>* A_0, uint A_0)
	{
		if ((A_0 & 2U) != 0U)
		{
			CAutoDelete<CDeviceNotificationCallbackShim\u0020*>* ptr = A_0 - 4;
			<Module>.__ehvec_dtor(A_0, 8U, (uint)(*ptr), ldftn(RAII.CAutoDelete<CDeviceNotificationCallbackShim\u0020*>.{dtor}));
			if ((A_0 & 1U) != 0U)
			{
				<Module>.delete[](ptr);
			}
			return ptr;
		}
		*A_0 = ref <Module>.??_7?$CAutoDelete@PAVCDeviceNotificationCallbackShim@@@RAII@@6B@;
		<Module>.RAII.CAutoDelete<CDeviceNotificationCallbackShim\u0020*>.Release(A_0);
		if ((A_0 & 1U) != 0U)
		{
			<Module>.delete(A_0);
		}
		return A_0;
	}

	// Token: 0x0600008C RID: 140 RVA: 0x00015C9C File Offset: 0x0001509C
	internal unsafe static void* __vecDelDtor(CAutoRelease<FlashingPlatform::IConnectedDeviceCollection\u0020*>* A_0, uint A_0)
	{
		if ((A_0 & 2U) != 0U)
		{
			CAutoRelease<FlashingPlatform::IConnectedDeviceCollection\u0020*>* ptr = A_0 - 4;
			<Module>.__ehvec_dtor(A_0, 8U, (uint)(*ptr), ldftn(RAII.CAutoRelease<FlashingPlatform::IConnectedDeviceCollection\u0020*>.{dtor}));
			if ((A_0 & 1U) != 0U)
			{
				<Module>.delete[](ptr);
			}
			return ptr;
		}
		*A_0 = ref <Module>.??_7?$CAutoRelease@PAUIConnectedDeviceCollection@FlashingPlatform@@@RAII@@6B@;
		<Module>.RAII.CAutoRelease<FlashingPlatform::IConnectedDeviceCollection\u0020*>.Release(A_0);
		if ((A_0 & 1U) != 0U)
		{
			<Module>.delete(A_0);
		}
		return A_0;
	}

	// Token: 0x0600008D RID: 141 RVA: 0x00015DD0 File Offset: 0x000151D0
	internal unsafe static void* __vecDelDtor(CAutoRelease<FlashingPlatform::IFlashingPlatform\u0020*>* A_0, uint A_0)
	{
		if ((A_0 & 2U) != 0U)
		{
			CAutoRelease<FlashingPlatform::IFlashingPlatform\u0020*>* ptr = A_0 - 4;
			<Module>.__ehvec_dtor(A_0, 8U, (uint)(*ptr), ldftn(RAII.CAutoRelease<FlashingPlatform::IFlashingPlatform\u0020*>.{dtor}));
			if ((A_0 & 1U) != 0U)
			{
				<Module>.delete[](ptr);
			}
			return ptr;
		}
		*A_0 = ref <Module>.??_7?$CAutoRelease@PAUIFlashingPlatform@FlashingPlatform@@@RAII@@6B@;
		<Module>.RAII.CAutoRelease<FlashingPlatform::IFlashingPlatform\u0020*>.Release(A_0);
		if ((A_0 & 1U) != 0U)
		{
			<Module>.delete(A_0);
		}
		return A_0;
	}

	// Token: 0x0600008E RID: 142 RVA: 0x00015F60 File Offset: 0x00015360
	[return: MarshalAs(UnmanagedType.U1)]
	internal static bool <CrtImplementationDetails>.NativeDll.IsSafeForManagedCode()
	{
		if (((<Module>.__native_dllmain_reason != 4294967295U) ? 1 : 0) == 0)
		{
			return 1;
		}
		if (((<Module>.__native_vcclrit_reason != 4294967295U) ? 1 : 0) != 0)
		{
			return 1;
		}
		int num;
		if (((<Module>.__native_dllmain_reason == 1U) ? 1 : 0) == 0 && ((<Module>.__native_dllmain_reason == 0U) ? 1 : 0) == 0)
		{
			num = 1;
		}
		else
		{
			num = 0;
		}
		return (byte)num;
	}

	// Token: 0x0600008F RID: 143 RVA: 0x00015FCC File Offset: 0x000153CC
	internal unsafe static int <CrtImplementationDetails>.DefaultDomain.DoNothing(void* cookie)
	{
		GC.KeepAlive(int.MaxValue);
		return 0;
	}

	// Token: 0x06000090 RID: 144 RVA: 0x00015FF0 File Offset: 0x000153F0
	[return: MarshalAs(UnmanagedType.U1)]
	internal unsafe static bool <CrtImplementationDetails>.DefaultDomain.HasPerProcess()
	{
		if (<Module>.?hasPerProcess@DefaultDomain@<CrtImplementationDetails>@@0W4State@TriBool@2@A == (TriBool.State)2)
		{
			void** ptr = (void**)(&<Module>.?A0x2442659e.__xc_mp_a);
			if (ref <Module>.?A0x2442659e.__xc_mp_a < ref <Module>.?A0x2442659e.__xc_mp_z)
			{
				while (*(int*)ptr == 0)
				{
					ptr += 4 / sizeof(void*);
					if (ptr >= (void**)(&<Module>.?A0x2442659e.__xc_mp_z))
					{
						goto IL_34;
					}
				}
				<Module>.?hasPerProcess@DefaultDomain@<CrtImplementationDetails>@@0W4State@TriBool@2@A = (TriBool.State)(-1);
				return 1;
			}
			IL_34:
			<Module>.?hasPerProcess@DefaultDomain@<CrtImplementationDetails>@@0W4State@TriBool@2@A = (TriBool.State)0;
			return 0;
		}
		return (<Module>.?hasPerProcess@DefaultDomain@<CrtImplementationDetails>@@0W4State@TriBool@2@A == (TriBool.State)(-1)) ? 1 : 0;
	}

	// Token: 0x06000091 RID: 145 RVA: 0x00016048 File Offset: 0x00015448
	[return: MarshalAs(UnmanagedType.U1)]
	internal unsafe static bool <CrtImplementationDetails>.DefaultDomain.HasNative()
	{
		if (<Module>.?hasNative@DefaultDomain@<CrtImplementationDetails>@@0W4State@TriBool@2@A == (TriBool.State)2)
		{
			void** ptr = (void**)(&<Module>.__xi_a);
			if (ref <Module>.__xi_a < ref <Module>.__xi_z)
			{
				while (*(int*)ptr == 0)
				{
					ptr += 4 / sizeof(void*);
					if (ptr >= (void**)(&<Module>.__xi_z))
					{
						goto IL_34;
					}
				}
				<Module>.?hasNative@DefaultDomain@<CrtImplementationDetails>@@0W4State@TriBool@2@A = (TriBool.State)(-1);
				return 1;
			}
			IL_34:
			void** ptr2 = (void**)(&<Module>.__xc_a);
			if (ref <Module>.__xc_a < ref <Module>.__xc_z)
			{
				while (*(int*)ptr2 == 0)
				{
					ptr2 += 4 / sizeof(void*);
					if (ptr2 >= (void**)(&<Module>.__xc_z))
					{
						goto IL_60;
					}
				}
				<Module>.?hasNative@DefaultDomain@<CrtImplementationDetails>@@0W4State@TriBool@2@A = (TriBool.State)(-1);
				return 1;
			}
			IL_60:
			<Module>.?hasNative@DefaultDomain@<CrtImplementationDetails>@@0W4State@TriBool@2@A = (TriBool.State)0;
			return 0;
		}
		return (<Module>.?hasNative@DefaultDomain@<CrtImplementationDetails>@@0W4State@TriBool@2@A == (TriBool.State)(-1)) ? 1 : 0;
	}

	// Token: 0x06000092 RID: 146 RVA: 0x000160CC File Offset: 0x000154CC
	[return: MarshalAs(UnmanagedType.U1)]
	internal static bool <CrtImplementationDetails>.DefaultDomain.NeedsInitialization()
	{
		int num;
		if ((<Module>.<CrtImplementationDetails>.DefaultDomain.HasPerProcess() != null && !<Module>.?InitializedPerProcess@DefaultDomain@<CrtImplementationDetails>@@2_NA) || (<Module>.<CrtImplementationDetails>.DefaultDomain.HasNative() != null && !<Module>.?InitializedNative@DefaultDomain@<CrtImplementationDetails>@@2_NA && <Module>.__native_startup_state == (__enative_startup_state)0))
		{
			num = 1;
		}
		else
		{
			num = 0;
		}
		return (byte)num;
	}

	// Token: 0x06000093 RID: 147 RVA: 0x0001610C File Offset: 0x0001550C
	internal static void <CrtImplementationDetails>.DefaultDomain.Initialize()
	{
		<Module>.<CrtImplementationDetails>.DoCallBackInDefaultDomain(<Module>.__unep@?DoNothing@DefaultDomain@<CrtImplementationDetails>@@$$FCGJPAX@Z, null);
	}

	// Token: 0x06000094 RID: 148 RVA: 0x0001233C File Offset: 0x0001173C
	internal static void ??__E?Initialized@CurrentDomain@<CrtImplementationDetails>@@$$Q2HA@@YMXXZ()
	{
		<Module>.?Initialized@CurrentDomain@<CrtImplementationDetails>@@$$Q2HA = 0;
	}

	// Token: 0x06000095 RID: 149 RVA: 0x00012350 File Offset: 0x00011750
	internal static void ??__E?Uninitialized@CurrentDomain@<CrtImplementationDetails>@@$$Q2HA@@YMXXZ()
	{
		<Module>.?Uninitialized@CurrentDomain@<CrtImplementationDetails>@@$$Q2HA = 0;
	}

	// Token: 0x06000096 RID: 150 RVA: 0x00012364 File Offset: 0x00011764
	internal static void ??__E?IsDefaultDomain@CurrentDomain@<CrtImplementationDetails>@@$$Q2_NA@@YMXXZ()
	{
		<Module>.?IsDefaultDomain@CurrentDomain@<CrtImplementationDetails>@@$$Q2_NA = false;
	}

	// Token: 0x06000097 RID: 151 RVA: 0x00012378 File Offset: 0x00011778
	internal static void ??__E?InitializedVtables@CurrentDomain@<CrtImplementationDetails>@@$$Q2W4State@Progress@2@A@@YMXXZ()
	{
		<Module>.?InitializedVtables@CurrentDomain@<CrtImplementationDetails>@@$$Q2W4State@Progress@2@A = (Progress.State)0;
	}

	// Token: 0x06000098 RID: 152 RVA: 0x0001238C File Offset: 0x0001178C
	internal static void ??__E?InitializedNative@CurrentDomain@<CrtImplementationDetails>@@$$Q2W4State@Progress@2@A@@YMXXZ()
	{
		<Module>.?InitializedNative@CurrentDomain@<CrtImplementationDetails>@@$$Q2W4State@Progress@2@A = (Progress.State)0;
	}

	// Token: 0x06000099 RID: 153 RVA: 0x000123A0 File Offset: 0x000117A0
	internal static void ??__E?InitializedPerProcess@CurrentDomain@<CrtImplementationDetails>@@$$Q2W4State@Progress@2@A@@YMXXZ()
	{
		<Module>.?InitializedPerProcess@CurrentDomain@<CrtImplementationDetails>@@$$Q2W4State@Progress@2@A = (Progress.State)0;
	}

	// Token: 0x0600009A RID: 154 RVA: 0x000123B4 File Offset: 0x000117B4
	internal static void ??__E?InitializedPerAppDomain@CurrentDomain@<CrtImplementationDetails>@@$$Q2W4State@Progress@2@A@@YMXXZ()
	{
		<Module>.?InitializedPerAppDomain@CurrentDomain@<CrtImplementationDetails>@@$$Q2W4State@Progress@2@A = (Progress.State)0;
	}

	// Token: 0x0600009B RID: 155 RVA: 0x000162D4 File Offset: 0x000156D4
	[DebuggerStepThrough]
	internal unsafe static void <CrtImplementationDetails>.LanguageSupport.InitializeVtables(LanguageSupport* A_0)
	{
		string target = "The C++ module failed to load during vtable initialization.\n";
		IntPtr value = new IntPtr(*A_0);
		((GCHandle)value).Target = target;
		<Module>.?InitializedVtables@CurrentDomain@<CrtImplementationDetails>@@$$Q2W4State@Progress@2@A = (Progress.State)1;
		<Module>._initterm_m((method*)(&<Module>.?A0x2442659e.__xi_vt_a), (method*)(&<Module>.?A0x2442659e.__xi_vt_z));
		<Module>.?InitializedVtables@CurrentDomain@<CrtImplementationDetails>@@$$Q2W4State@Progress@2@A = (Progress.State)2;
	}

	// Token: 0x0600009C RID: 156 RVA: 0x00016320 File Offset: 0x00015720
	internal unsafe static void <CrtImplementationDetails>.LanguageSupport.InitializeDefaultAppDomain(LanguageSupport* A_0)
	{
		string target = "The C++ module failed to load while attempting to initialize the default appdomain.\n";
		IntPtr value = new IntPtr(*A_0);
		((GCHandle)value).Target = target;
		<Module>.<CrtImplementationDetails>.DefaultDomain.Initialize();
	}

	// Token: 0x0600009D RID: 157 RVA: 0x00016358 File Offset: 0x00015758
	[DebuggerStepThrough]
	internal unsafe static void <CrtImplementationDetails>.LanguageSupport.InitializeNative(LanguageSupport* A_0)
	{
		string target = "The C++ module failed to load during native initialization.\n";
		IntPtr value = new IntPtr(*A_0);
		((GCHandle)value).Target = target;
		<Module>.?InitializedNative@DefaultDomain@<CrtImplementationDetails>@@2_NA = true;
		if (<Module>.<CrtImplementationDetails>.NativeDll.IsSafeForManagedCode() == null)
		{
			<Module>._amsg_exit(33);
		}
		if (<Module>.__native_startup_state == (__enative_startup_state)1)
		{
			<Module>._amsg_exit(33);
		}
		else if (<Module>.__native_startup_state == (__enative_startup_state)0)
		{
			<Module>.?InitializedNative@CurrentDomain@<CrtImplementationDetails>@@$$Q2W4State@Progress@2@A = (Progress.State)1;
			<Module>.__native_startup_state = (__enative_startup_state)1;
			if (<Module>._initterm_e((method*)(&<Module>.__xi_a), (method*)(&<Module>.__xi_z)) != 0)
			{
				IntPtr value2 = new IntPtr(*A_0);
				<Module>.<CrtImplementationDetails>.ThrowModuleLoadException(((GCHandle)value2).Target);
			}
			<Module>._initterm((method*)(&<Module>.__xc_a), (method*)(&<Module>.__xc_z));
			<Module>.__native_startup_state = (__enative_startup_state)2;
			<Module>.?InitializedNativeFromCCTOR@DefaultDomain@<CrtImplementationDetails>@@2_NA = true;
			<Module>.?InitializedNative@CurrentDomain@<CrtImplementationDetails>@@$$Q2W4State@Progress@2@A = (Progress.State)2;
		}
	}

	// Token: 0x0600009E RID: 158 RVA: 0x00016418 File Offset: 0x00015818
	[DebuggerStepThrough]
	internal unsafe static void <CrtImplementationDetails>.LanguageSupport.InitializePerProcess(LanguageSupport* A_0)
	{
		string target = "The C++ module failed to load during process initialization.\n";
		IntPtr value = new IntPtr(*A_0);
		((GCHandle)value).Target = target;
		<Module>.?InitializedPerProcess@CurrentDomain@<CrtImplementationDetails>@@$$Q2W4State@Progress@2@A = (Progress.State)1;
		<Module>._initatexit_m();
		<Module>._initterm_m((method*)(&<Module>.?A0x2442659e.__xc_mp_a), (method*)(&<Module>.?A0x2442659e.__xc_mp_z));
		<Module>.?InitializedPerProcess@CurrentDomain@<CrtImplementationDetails>@@$$Q2W4State@Progress@2@A = (Progress.State)2;
		<Module>.?InitializedPerProcess@DefaultDomain@<CrtImplementationDetails>@@2_NA = true;
	}

	// Token: 0x0600009F RID: 159 RVA: 0x00016470 File Offset: 0x00015870
	[DebuggerStepThrough]
	internal unsafe static void <CrtImplementationDetails>.LanguageSupport.InitializePerAppDomain(LanguageSupport* A_0)
	{
		string target = "The C++ module failed to load during appdomain initialization.\n";
		IntPtr value = new IntPtr(*A_0);
		((GCHandle)value).Target = target;
		<Module>.?InitializedPerAppDomain@CurrentDomain@<CrtImplementationDetails>@@$$Q2W4State@Progress@2@A = (Progress.State)1;
		<Module>._initatexit_app_domain();
		<Module>._initterm_m((method*)(&<Module>.?A0x2442659e.__xc_ma_a), (method*)(&<Module>.?A0x2442659e.__xc_ma_z));
		<Module>.?InitializedPerAppDomain@CurrentDomain@<CrtImplementationDetails>@@$$Q2W4State@Progress@2@A = (Progress.State)2;
	}

	// Token: 0x060000A0 RID: 160 RVA: 0x000164C4 File Offset: 0x000158C4
	[DebuggerStepThrough]
	internal unsafe static void <CrtImplementationDetails>.LanguageSupport.InitializeUninitializer(LanguageSupport* A_0)
	{
		string target = "The C++ module failed to load during registration for the unload events.\n";
		IntPtr value = new IntPtr(*A_0);
		((GCHandle)value).Target = target;
		<Module>.<CrtImplementationDetails>.RegisterModuleUninitializer(new EventHandler(<Module>.<CrtImplementationDetails>.LanguageSupport.DomainUnload));
	}

	// Token: 0x060000A1 RID: 161 RVA: 0x00016508 File Offset: 0x00015908
	[DebuggerStepThrough]
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	internal unsafe static void <CrtImplementationDetails>.LanguageSupport._Initialize(LanguageSupport* A_0)
	{
		Interlocked.Increment(ref <Module>.?Count@AllDomains@<CrtImplementationDetails>@@2HA);
		<Module>.?IsDefaultDomain@CurrentDomain@<CrtImplementationDetails>@@$$Q2_NA = AppDomain.CurrentDomain.IsDefaultAppDomain();
		if (<Module>.?IsDefaultDomain@CurrentDomain@<CrtImplementationDetails>@@$$Q2_NA)
		{
			<Module>.?Entered@DefaultDomain@<CrtImplementationDetails>@@2_NA = true;
		}
		void* ptr = <Module>._getFiberPtrId();
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
			while (num2 == 0)
			{
				try
				{
				}
				finally
				{
					IntPtr comparand = (IntPtr)0;
					IntPtr value = (IntPtr)ptr;
					IntPtr value2 = Interlocked.CompareExchange(ref <Module>.__native_startup_lock, value, comparand);
					void* ptr2 = (void*)value2;
					if (ptr2 == null)
					{
						num2 = 1;
					}
					else if (ptr2 == ptr)
					{
						num = 1;
						num2 = 1;
					}
				}
				if (num2 == 0)
				{
					<Module>.Sleep(1000);
				}
			}
			<Module>.<CrtImplementationDetails>.LanguageSupport.InitializeVtables(A_0);
			if (<Module>.?IsDefaultDomain@CurrentDomain@<CrtImplementationDetails>@@$$Q2_NA)
			{
				<Module>.<CrtImplementationDetails>.LanguageSupport.InitializeNative(A_0);
				<Module>.<CrtImplementationDetails>.LanguageSupport.InitializePerProcess(A_0);
			}
			else if (<Module>.<CrtImplementationDetails>.DefaultDomain.NeedsInitialization() != null)
			{
				num3 = 1;
			}
		}
		finally
		{
			if (num == 0)
			{
				IntPtr value3 = (IntPtr)0;
				Interlocked.Exchange(ref <Module>.__native_startup_lock, value3);
			}
		}
		if (num3 != 0)
		{
			<Module>.<CrtImplementationDetails>.LanguageSupport.InitializeDefaultAppDomain(A_0);
		}
		<Module>.<CrtImplementationDetails>.LanguageSupport.InitializePerAppDomain(A_0);
		<Module>.?Initialized@CurrentDomain@<CrtImplementationDetails>@@$$Q2HA = 1;
		<Module>.<CrtImplementationDetails>.LanguageSupport.InitializeUninitializer(A_0);
	}

	// Token: 0x060000A2 RID: 162 RVA: 0x0001612C File Offset: 0x0001552C
	internal static void <CrtImplementationDetails>.LanguageSupport.UninitializeAppDomain()
	{
		<Module>._app_exit_callback();
	}

	// Token: 0x060000A3 RID: 163 RVA: 0x00016144 File Offset: 0x00015544
	internal unsafe static int <CrtImplementationDetails>.LanguageSupport._UninitializeDefaultDomain(void* cookie)
	{
		<Module>._exit_callback();
		<Module>.?InitializedPerProcess@DefaultDomain@<CrtImplementationDetails>@@2_NA = false;
		if (<Module>.?InitializedNativeFromCCTOR@DefaultDomain@<CrtImplementationDetails>@@2_NA)
		{
			<Module>._cexit();
			<Module>.__native_startup_state = (__enative_startup_state)0;
			<Module>.?InitializedNativeFromCCTOR@DefaultDomain@<CrtImplementationDetails>@@2_NA = false;
		}
		<Module>.?InitializedNative@DefaultDomain@<CrtImplementationDetails>@@2_NA = false;
		return 0;
	}

	// Token: 0x060000A4 RID: 164 RVA: 0x00016184 File Offset: 0x00015584
	internal static void <CrtImplementationDetails>.LanguageSupport.UninitializeDefaultDomain()
	{
		if (<Module>.?Entered@DefaultDomain@<CrtImplementationDetails>@@2_NA)
		{
			if (AppDomain.CurrentDomain.IsDefaultAppDomain())
			{
				<Module>.<CrtImplementationDetails>.LanguageSupport._UninitializeDefaultDomain(null);
			}
			else
			{
				<Module>.<CrtImplementationDetails>.DoCallBackInDefaultDomain(<Module>.__unep@?_UninitializeDefaultDomain@LanguageSupport@<CrtImplementationDetails>@@$$FCGJPAX@Z, null);
			}
		}
	}

	// Token: 0x060000A5 RID: 165 RVA: 0x000161C0 File Offset: 0x000155C0
	[PrePrepareMethod]
	internal static void <CrtImplementationDetails>.LanguageSupport.DomainUnload(object source, EventArgs arguments)
	{
		if (<Module>.?Initialized@CurrentDomain@<CrtImplementationDetails>@@$$Q2HA != 0 && Interlocked.Exchange(ref <Module>.?Uninitialized@CurrentDomain@<CrtImplementationDetails>@@$$Q2HA, 1) == 0)
		{
			byte b = (Interlocked.Decrement(ref <Module>.?Count@AllDomains@<CrtImplementationDetails>@@2HA) == 0) ? 1 : 0;
			<Module>._app_exit_callback();
			if (b != 0)
			{
				<Module>.<CrtImplementationDetails>.LanguageSupport.UninitializeDefaultDomain();
			}
		}
	}

	// Token: 0x060000A6 RID: 166 RVA: 0x00016200 File Offset: 0x00015600
	[DebuggerStepThrough]
	internal unsafe static void <CrtImplementationDetails>.LanguageSupport.Cleanup(LanguageSupport* A_0, Exception innerException)
	{
		try
		{
			bool flag = ((Interlocked.Decrement(ref <Module>.?Count@AllDomains@<CrtImplementationDetails>@@2HA) == 0) ? 1 : 0) != 0;
			<Module>.<CrtImplementationDetails>.LanguageSupport.UninitializeAppDomain();
			if (flag)
			{
				<Module>.<CrtImplementationDetails>.LanguageSupport.UninitializeDefaultDomain();
			}
		}
		catch (Exception nestedException)
		{
			<Module>.<CrtImplementationDetails>.ThrowNestedModuleLoadException(innerException, nestedException);
		}
		catch (object obj)
		{
			<Module>.<CrtImplementationDetails>.ThrowNestedModuleLoadException(innerException, null);
		}
	}

	// Token: 0x060000A7 RID: 167 RVA: 0x00016640 File Offset: 0x00015A40
	[DebuggerStepThrough]
	internal unsafe static void <CrtImplementationDetails>.LanguageSupport.Initialize(LanguageSupport* A_0)
	{
		try
		{
			<Module>.gcroot<System::String\u0020^>.=(A_0, "The C++ module failed to load.\n");
			<Module>.<CrtImplementationDetails>.LanguageSupport._Initialize(A_0);
		}
		catch (Exception innerException)
		{
			<Module>.<CrtImplementationDetails>.LanguageSupport.Cleanup(A_0, innerException);
			IntPtr value = new IntPtr(*A_0);
			<Module>.<CrtImplementationDetails>.ThrowModuleLoadException(((GCHandle)value).Target, innerException);
		}
		catch (object obj)
		{
			<Module>.<CrtImplementationDetails>.LanguageSupport.Cleanup(A_0, null);
			IntPtr value2 = new IntPtr(*A_0);
			<Module>.<CrtImplementationDetails>.ThrowModuleLoadException(((GCHandle)value2).Target, null);
		}
	}

	// Token: 0x060000A8 RID: 168 RVA: 0x00016738 File Offset: 0x00015B38
	[DebuggerStepThrough]
	static unsafe <Module>()
	{
		LanguageSupport languageSupport;
		<Module>.<CrtImplementationDetails>.LanguageSupport.{ctor}(ref languageSupport);
		try
		{
			<Module>.<CrtImplementationDetails>.LanguageSupport.Initialize(ref languageSupport);
		}
		catch
		{
			<Module>.___CxxCallUnwindDtor(ldftn(<CrtImplementationDetails>.LanguageSupport.{dtor}), (void*)(&languageSupport));
			throw;
		}
		<Module>.gcroot<System::String\u0020^>.{dtor}(ref languageSupport);
	}

	// Token: 0x060000A9 RID: 169 RVA: 0x000166F0 File Offset: 0x00015AF0
	internal unsafe static LanguageSupport* <CrtImplementationDetails>.LanguageSupport.{ctor}(LanguageSupport* A_0)
	{
		*A_0 = ((IntPtr)GCHandle.Alloc(null)).ToPointer();
		return A_0;
	}

	// Token: 0x060000AA RID: 170 RVA: 0x0001671C File Offset: 0x00015B1C
	internal unsafe static void <CrtImplementationDetails>.LanguageSupport.{dtor}(LanguageSupport* A_0)
	{
		<Module>.gcroot<System::String\u0020^>.{dtor}(A_0);
	}

	// Token: 0x060000AB RID: 171 RVA: 0x0001627C File Offset: 0x0001567C
	internal unsafe static gcroot<System::String\u0020^>* =(gcroot<System::String\u0020^>* A_0, string t)
	{
		IntPtr value = new IntPtr(*A_0);
		((GCHandle)value).Target = t;
		return A_0;
	}

	// Token: 0x060000AC RID: 172 RVA: 0x000162A8 File Offset: 0x000156A8
	internal unsafe static void {dtor}(gcroot<System::String\u0020^>* A_0)
	{
		IntPtr value = new IntPtr(*A_0);
		((GCHandle)value).Free();
		*A_0 = 0;
	}

	// Token: 0x060000AD RID: 173 RVA: 0x000167A4 File Offset: 0x00015BA4
	[HandleProcessCorruptedStateExceptions]
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	internal unsafe static void ___CxxCallUnwindDtor(method pDtor, void* pThis)
	{
		try
		{
			calli(System.Void(System.Void*), pThis, pDtor);
		}
		catch when (endfilter(<Module>.__FrameUnwindFilter(Marshal.GetExceptionPointers()) != null))
		{
		}
	}

	// Token: 0x060000AE RID: 174 RVA: 0x00016814 File Offset: 0x00015C14
	[PrePrepareMethod]
	internal unsafe static _exception_handling_state_pointers_t* _get_exception_handling_state_uplevel(_exception_handling_state_pointers_t* p)
	{
		if (*(int*)p == 0)
		{
			_tiddata_managed* ptr = <Module>._getptd();
			*(int*)p = ptr + 144 / sizeof(_tiddata_managed);
			*(int*)(p + 4 / sizeof(_exception_handling_state_pointers_t)) = ptr + 524 / sizeof(_tiddata_managed);
			*(int*)(p + 8 / sizeof(_exception_handling_state_pointers_t)) = ptr + 136 / sizeof(_tiddata_managed);
			*(int*)(p + 12 / sizeof(_exception_handling_state_pointers_t)) = ptr + 140 / sizeof(_tiddata_managed);
			*(int*)(p + 16 / sizeof(_exception_handling_state_pointers_t)) = ptr + 148 / sizeof(_tiddata_managed);
			*(int*)(p + 20 / sizeof(_exception_handling_state_pointers_t)) = ptr + 152 / sizeof(_tiddata_managed);
		}
		return p;
	}

	// Token: 0x060000AF RID: 175 RVA: 0x000167F0 File Offset: 0x00015BF0
	[PrePrepareMethod]
	[SecurityPermission(SecurityAction.Assert, UnmanagedCode = true)]
	internal unsafe static _tiddata_managed* _getptd()
	{
		_tiddata_managed* ptr = <Module>._getptd_noexit();
		if (ptr == null)
		{
			<Module>._amsg_exit(16);
		}
		return ptr;
	}

	// Token: 0x060000B0 RID: 176 RVA: 0x00016874 File Offset: 0x00015C74
	[DebuggerStepThrough]
	internal static ValueType <CrtImplementationDetails>.AtExitLock._handle()
	{
		if (<Module>.?_lock@AtExitLock@<CrtImplementationDetails>@@$$Q0PAXA != null)
		{
			IntPtr value = new IntPtr(<Module>.?_lock@AtExitLock@<CrtImplementationDetails>@@$$Q0PAXA);
			return GCHandle.FromIntPtr(value);
		}
		return null;
	}

	// Token: 0x060000B1 RID: 177 RVA: 0x000168A8 File Offset: 0x00015CA8
	[DebuggerStepThrough]
	internal static void <CrtImplementationDetails>.AtExitLock._lock_Set(object value)
	{
		ValueType valueType = <Module>.<CrtImplementationDetails>.AtExitLock._handle();
		if (valueType == null)
		{
			valueType = GCHandle.Alloc(value);
			<Module>.?_lock@AtExitLock@<CrtImplementationDetails>@@$$Q0PAXA = GCHandle.ToIntPtr((GCHandle)valueType).ToPointer();
		}
		else
		{
			((GCHandle)valueType).Target = value;
		}
	}

	// Token: 0x060000B2 RID: 178 RVA: 0x000168FC File Offset: 0x00015CFC
	[DebuggerStepThrough]
	internal static object <CrtImplementationDetails>.AtExitLock._lock_Get()
	{
		ValueType valueType = <Module>.<CrtImplementationDetails>.AtExitLock._handle();
		if (valueType != null)
		{
			return ((GCHandle)valueType).Target;
		}
		return null;
	}

	// Token: 0x060000B3 RID: 179 RVA: 0x00016924 File Offset: 0x00015D24
	[DebuggerStepThrough]
	internal static void <CrtImplementationDetails>.AtExitLock._lock_Destruct()
	{
		ValueType valueType = <Module>.<CrtImplementationDetails>.AtExitLock._handle();
		if (valueType != null)
		{
			((GCHandle)valueType).Free();
			<Module>.?_lock@AtExitLock@<CrtImplementationDetails>@@$$Q0PAXA = null;
		}
	}

	// Token: 0x060000B4 RID: 180 RVA: 0x00016D0C File Offset: 0x0001610C
	[DebuggerStepThrough]
	internal static void <CrtImplementationDetails>.AtExitLock.AddRef()
	{
		if (((<Module>.<CrtImplementationDetails>.AtExitLock._lock_Get() != null) ? 1 : 0) == 0)
		{
			<Module>.?_lock@AtExitLock@<CrtImplementationDetails>@@$$Q0PAXA = null;
			<Module>.<CrtImplementationDetails>.AtExitLock._lock_Set(new object());
			<Module>.?_ref_count@AtExitLock@<CrtImplementationDetails>@@$$Q0HA = 0;
		}
		<Module>.?_ref_count@AtExitLock@<CrtImplementationDetails>@@$$Q0HA++;
	}

	// Token: 0x060000B5 RID: 181 RVA: 0x00016950 File Offset: 0x00015D50
	[DebuggerStepThrough]
	[return: MarshalAs(UnmanagedType.U1)]
	internal static bool __global_lock()
	{
		bool result = false;
		if (((<Module>.<CrtImplementationDetails>.AtExitLock._lock_Get() != null) ? 1 : 0) != 0)
		{
			Monitor.Enter(<Module>.<CrtImplementationDetails>.AtExitLock._lock_Get());
			result = true;
		}
		return result;
	}

	// Token: 0x060000B6 RID: 182 RVA: 0x00016980 File Offset: 0x00015D80
	[DebuggerStepThrough]
	[return: MarshalAs(UnmanagedType.U1)]
	internal static bool __global_unlock()
	{
		bool result = false;
		if (((<Module>.<CrtImplementationDetails>.AtExitLock._lock_Get() != null) ? 1 : 0) != 0)
		{
			Monitor.Exit(<Module>.<CrtImplementationDetails>.AtExitLock._lock_Get());
			result = true;
		}
		return result;
	}

	// Token: 0x060000B7 RID: 183 RVA: 0x00016D50 File Offset: 0x00016150
	[DebuggerStepThrough]
	[return: MarshalAs(UnmanagedType.U1)]
	internal static bool __alloc_global_lock()
	{
		<Module>.<CrtImplementationDetails>.AtExitLock.AddRef();
		return (<Module>.<CrtImplementationDetails>.AtExitLock._lock_Get() != null) ? 1 : 0;
	}

	// Token: 0x060000B8 RID: 184 RVA: 0x000169B0 File Offset: 0x00015DB0
	internal unsafe static int _atexit_helper(method func, uint* __pexit_list_size, method** __ponexitend_e, method** __ponexitbegin_e)
	{
		method system.Void_u0020() = 0;
		if (func == null)
		{
			return -1;
		}
		if (<Module>.?A0x103fe9b9.__global_lock() == 1)
		{
			try
			{
				method* ptr = <Module>._decode_pointer(*(int*)__ponexitbegin_e);
				method* ptr2 = <Module>._decode_pointer(*(int*)__ponexitend_e);
				int num = (int)(ptr2 - ptr);
				if (*__pexit_list_size - 1U < (uint)num >> 2)
				{
					try
					{
						uint num2 = *__pexit_list_size * 4U;
						uint num3;
						if (num2 < 2048U)
						{
							num3 = num2;
						}
						else
						{
							num3 = 2048U;
						}
						IntPtr cb = new IntPtr((int)(num2 + num3));
						IntPtr pv = new IntPtr((void*)ptr);
						IntPtr intPtr = Marshal.ReAllocHGlobal(pv, cb);
						IntPtr intPtr2 = intPtr;
						ptr2 = (method*)((byte*)intPtr2.ToPointer() + num);
						ptr = (method*)intPtr2.ToPointer();
						uint num4 = *__pexit_list_size;
						uint num5;
						if (512U < num4)
						{
							num5 = 512U;
						}
						else
						{
							num5 = num4;
						}
						*__pexit_list_size = num4 + num5;
					}
					catch (OutOfMemoryException)
					{
						IntPtr cb2 = new IntPtr((int)(*__pexit_list_size * 4U + 8U));
						IntPtr pv2 = new IntPtr((void*)ptr);
						IntPtr intPtr3 = Marshal.ReAllocHGlobal(pv2, cb2);
						IntPtr intPtr4 = intPtr3;
						ptr2 = (intPtr4.ToPointer() - ptr) / (IntPtr)sizeof(method) + ptr2;
						ptr = (method*)intPtr4.ToPointer();
						*__pexit_list_size += 4U;
					}
				}
				*(int*)ptr2 = func;
				ptr2 += 4 / sizeof(method);
				system.Void_u0020() = func;
				*(int*)__ponexitbegin_e = <Module>._encode_pointer((void*)ptr);
				*(int*)__ponexitend_e = <Module>._encode_pointer((void*)ptr2);
			}
			catch (OutOfMemoryException)
			{
			}
			finally
			{
				<Module>.?A0x103fe9b9.__global_unlock();
			}
			if (system.Void_u0020() != null)
			{
				return 0;
			}
		}
		return -1;
	}

	// Token: 0x060000B9 RID: 185 RVA: 0x00016B34 File Offset: 0x00015F34
	internal unsafe static void _exit_callback()
	{
		if (<Module>.?A0x103fe9b9.__exit_list_size != 0U)
		{
			method* ptr = <Module>._decode_pointer((void*)<Module>.?A0x103fe9b9.__onexitbegin_m);
			method* ptr2 = <Module>._decode_pointer((void*)<Module>.?A0x103fe9b9.__onexitend_m);
			if (ptr != -1 && ptr != null && ptr2 != null)
			{
				method* ptr3 = ptr;
				method* ptr4 = ptr2;
				for (;;)
				{
					ptr2 -= 4 / sizeof(method);
					if (ptr2 < ptr)
					{
						break;
					}
					if (*(int*)ptr2 != <Module>._encoded_null())
					{
						void* ptr5 = <Module>._decode_pointer(*(int*)ptr2);
						*(int*)ptr2 = <Module>._encoded_null();
						calli(System.Void(), ptr5);
						method* ptr6 = <Module>._decode_pointer((void*)<Module>.?A0x103fe9b9.__onexitbegin_m);
						method* ptr7 = <Module>._decode_pointer((void*)<Module>.?A0x103fe9b9.__onexitend_m);
						if (ptr3 != ptr6 || ptr4 != ptr7)
						{
							ptr3 = ptr6;
							ptr = ptr6;
							ptr4 = ptr7;
							ptr2 = ptr7;
						}
					}
				}
				IntPtr hglobal = new IntPtr((void*)ptr);
				Marshal.FreeHGlobal(hglobal);
			}
			<Module>.?_ref_count@AtExitLock@<CrtImplementationDetails>@@$$Q0HA--;
			if (<Module>.?_ref_count@AtExitLock@<CrtImplementationDetails>@@$$Q0HA == 0)
			{
				<Module>.<CrtImplementationDetails>.AtExitLock._lock_Destruct();
			}
		}
	}

	// Token: 0x060000BA RID: 186 RVA: 0x00016D74 File Offset: 0x00016174
	[DebuggerStepThrough]
	internal static int _initatexit_m()
	{
		int result = 0;
		if (<Module>.?A0x103fe9b9.__alloc_global_lock() == 1)
		{
			<Module>.?A0x103fe9b9.__onexitbegin_m = <Module>._encode_pointer(Marshal.AllocHGlobal(128).ToPointer());
			<Module>.?A0x103fe9b9.__onexitend_m = <Module>.?A0x103fe9b9.__onexitbegin_m;
			<Module>.?A0x103fe9b9.__exit_list_size = 32U;
			result = 1;
		}
		return result;
	}

	// Token: 0x060000BB RID: 187 RVA: 0x00016BF0 File Offset: 0x00015FF0
	internal unsafe static int _atexit_m(method func)
	{
		return <Module>._atexit_helper(<Module>._encode_pointer(func), &<Module>.?A0x103fe9b9.__exit_list_size, &<Module>.?A0x103fe9b9.__onexitend_m, &<Module>.?A0x103fe9b9.__onexitbegin_m);
	}

	// Token: 0x060000BC RID: 188 RVA: 0x00016DC0 File Offset: 0x000161C0
	[DebuggerStepThrough]
	internal static int _initatexit_app_domain()
	{
		if (<Module>.?A0x103fe9b9.__alloc_global_lock() == 1)
		{
			<Module>.__onexitbegin_app_domain = <Module>._encode_pointer(Marshal.AllocHGlobal(128).ToPointer());
			<Module>.__onexitend_app_domain = <Module>.__onexitbegin_app_domain;
			<Module>.__exit_list_size_app_domain = 32U;
		}
		return 1;
	}

	// Token: 0x060000BD RID: 189 RVA: 0x00016C1C File Offset: 0x0001601C
	[HandleProcessCorruptedStateExceptions]
	internal unsafe static void _app_exit_callback()
	{
		if (<Module>.__exit_list_size_app_domain != 0U)
		{
			method* ptr = <Module>._decode_pointer((void*)<Module>.__onexitbegin_app_domain);
			method* ptr2 = <Module>._decode_pointer((void*)<Module>.__onexitend_app_domain);
			try
			{
				if (ptr != -1 && ptr != null && ptr2 != null)
				{
					method* ptr3 = ptr;
					method* ptr4 = ptr2;
					for (;;)
					{
						do
						{
							ptr2 -= 4 / sizeof(method);
						}
						while (ptr2 >= ptr && *(int*)ptr2 == <Module>._encoded_null());
						if (ptr2 < ptr)
						{
							break;
						}
						method system.Void_u0020() = <Module>._decode_pointer(*(int*)ptr2);
						*(int*)ptr2 = <Module>._encoded_null();
						calli(System.Void(), system.Void_u0020());
						method* ptr5 = <Module>._decode_pointer((void*)<Module>.__onexitbegin_app_domain);
						method* ptr6 = <Module>._decode_pointer((void*)<Module>.__onexitend_app_domain);
						if (ptr3 != ptr5 || ptr4 != ptr6)
						{
							ptr3 = ptr5;
							ptr = ptr5;
							ptr4 = ptr6;
							ptr2 = ptr6;
						}
					}
				}
			}
			finally
			{
				IntPtr hglobal = new IntPtr((void*)ptr);
				Marshal.FreeHGlobal(hglobal);
				<Module>.?_ref_count@AtExitLock@<CrtImplementationDetails>@@$$Q0HA--;
				if (<Module>.?_ref_count@AtExitLock@<CrtImplementationDetails>@@$$Q0HA == 0)
				{
					<Module>.<CrtImplementationDetails>.AtExitLock._lock_Destruct();
				}
			}
		}
	}

	// Token: 0x060000BE RID: 190 RVA: 0x00016E08 File Offset: 0x00016208
	internal unsafe static bad_alloc* {ctor}(bad_alloc* A_0)
	{
		<Module>.exception.{ctor}(A_0);
		try
		{
			*A_0 = ref <Module>.??_7bad_alloc@std@@6B@;
			bad_alloc* ptr = A_0 + 4;
			if (*ptr == 0)
			{
				if (*(A_0 + 8) == 0)
				{
					*ptr = <Module>.std._bad_alloc_Message;
				}
			}
		}
		catch
		{
			<Module>.___CxxCallUnwindDtor(ldftn(exception.{dtor}), A_0);
			throw;
		}
		return A_0;
	}

	// Token: 0x060000BF RID: 191 RVA: 0x00016E70 File Offset: 0x00016270
	internal unsafe static void {dtor}(bad_alloc* A_0)
	{
		*A_0 = ref <Module>.??_7bad_alloc@std@@6B@;
		<Module>.exception.{dtor}(A_0);
	}

	// Token: 0x060000C0 RID: 192 RVA: 0x00016FE4 File Offset: 0x000163E4
	internal unsafe static void* __vecDelDtor(bad_alloc* A_0, uint A_0)
	{
		if ((A_0 & 2U) != 0U)
		{
			bad_alloc* ptr = A_0 - 4;
			<Module>.__ehvec_dtor(A_0, 12U, (uint)(*ptr), ldftn(std.bad_alloc.{dtor}));
			if ((A_0 & 1U) != 0U)
			{
				<Module>.delete(ptr);
			}
			return ptr;
		}
		*A_0 = ref <Module>.??_7bad_alloc@std@@6B@;
		<Module>.exception.{dtor}(A_0);
		if ((A_0 & 1U) != 0U)
		{
			<Module>.delete(A_0);
		}
		return A_0;
	}

	// Token: 0x060000C1 RID: 193 RVA: 0x00016F04 File Offset: 0x00016304
	internal unsafe static void* new[](uint size)
	{
		void* ptr = <Module>.malloc(size);
		if (ptr == null)
		{
			while (<Module>._callnewh(size) != null)
			{
				ptr = <Module>.malloc(size);
				if (ptr != null)
				{
					return ptr;
				}
			}
			if ((<Module>.?A0xba0a35f5.?$S1@?6???_U@YAPAXI@Z@4IA & 1U) == 0U)
			{
				<Module>.?A0xba0a35f5.?$S1@?6???_U@YAPAXI@Z@4IA |= 1U;
				try
				{
					<Module>.std.bad_alloc.{ctor}(ref <Module>.?A0xba0a35f5.?nomem@?6???_U@YAPAXI@Z@4Vbad_alloc@std@@B);
					<Module>._atexit_m(ldftn(?A0xba0a35f5.??__Fnomem@?6???_U@YAPAXI@Z@YMXXZ));
				}
				catch
				{
					<Module>.?A0xba0a35f5.?$S1@?6???_U@YAPAXI@Z@4IA &= 4294967294U;
					throw;
				}
			}
			bad_alloc bad_alloc;
			<Module>.exception.{ctor}(ref bad_alloc, ref <Module>.?A0xba0a35f5.?nomem@?6???_U@YAPAXI@Z@4Vbad_alloc@std@@B);
			try
			{
				bad_alloc = ref <Module>.??_7bad_alloc@std@@6B@;
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(exception.{dtor}), (void*)(&bad_alloc));
				throw;
			}
			<Module>._CxxThrowException((void*)(&bad_alloc), (_s__ThrowInfo*)(&<Module>._TI2?AVbad_alloc@std@@));
		}
		return ptr;
	}

	// Token: 0x060000C2 RID: 194 RVA: 0x00019A40 File Offset: 0x00018E40
	internal static void ??__Fnomem@?6???_U@YAPAXI@Z@YMXXZ()
	{
		<Module>.std.bad_alloc.{dtor}(ref <Module>.?A0xba0a35f5.?nomem@?6???_U@YAPAXI@Z@4Vbad_alloc@std@@B);
	}

	// Token: 0x060000C3 RID: 195 RVA: 0x00016E94 File Offset: 0x00016294
	internal unsafe static bad_alloc* {ctor}(bad_alloc* A_0, bad_alloc* A_0)
	{
		<Module>.exception.{ctor}(A_0, A_0);
		try
		{
			*A_0 = ref <Module>.??_7bad_alloc@std@@6B@;
		}
		catch
		{
			<Module>.___CxxCallUnwindDtor(ldftn(exception.{dtor}), A_0);
			throw;
		}
		return A_0;
	}

	// Token: 0x060000C4 RID: 196 RVA: 0x00016EEC File Offset: 0x000162EC
	internal unsafe static void delete[](void* p)
	{
		<Module>.free(p);
	}

	// Token: 0x060000C5 RID: 197 RVA: 0x0001704C File Offset: 0x0001644C
	internal unsafe static int _vsnwprintf_s(char* _Dst, uint _SizeInWords, uint _Count, char* _Format, sbyte* _ArgList)
	{
		if (_Format == null)
		{
			*<Module>._errno() = 22;
			<Module>._invalid_parameter(null, null, null, 0U, 0U);
			return -1;
		}
		if (_Count == 0U)
		{
			if (_Dst == null)
			{
				if (_SizeInWords == 0U)
				{
					return 0;
				}
				goto IL_29;
			}
		}
		else if (_Dst == null)
		{
			goto IL_29;
		}
		if (_SizeInWords > 0U)
		{
			int num;
			if (_SizeInWords > _Count)
			{
				num = <Module>._swoutput_s(_Dst, _Count + 1U, _Format, _ArgList);
				if (num == -2)
				{
					return -1;
				}
			}
			else
			{
				num = <Module>._swoutput_s(_Dst, _SizeInWords, _Format, _ArgList);
				if (num == -2)
				{
					if (_Count == 4294967295U)
					{
						return -1;
					}
					goto IL_6F;
				}
			}
			if (num >= 0)
			{
				return num;
			}
			IL_6F:
			*_Dst = '\0';
			if (num == -2)
			{
				*<Module>._errno() = 34;
				<Module>._invalid_parameter(null, null, null, 0U, 0U);
				return -1;
			}
			return -1;
		}
		IL_29:
		*<Module>._errno() = 22;
		<Module>._invalid_parameter(null, null, null, 0U, 0U);
		return -1;
	}

	// Token: 0x060000C6 RID: 198 RVA: 0x00017164 File Offset: 0x00016564
	[HandleProcessCorruptedStateExceptions]
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	internal unsafe static void __ehvec_dtor(void* ptr, uint size, int count, method pDtor)
	{
		int num = 0;
		ptr = (void*)(size * (uint)count + (byte*)ptr);
		try
		{
			for (;;)
			{
				count--;
				if (count < 0)
				{
					break;
				}
				ptr = (void*)((byte*)ptr - size);
				calli(System.Void(System.Void*), ptr, pDtor);
			}
			num = 1;
		}
		finally
		{
			if (num == 0)
			{
				<Module>.__ArrayUnwind(ptr, size, count, pDtor);
			}
		}
	}

	// Token: 0x060000C7 RID: 199 RVA: 0x000171C8 File Offset: 0x000165C8
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	internal unsafe static void __ehvec_dtor(void* ptr, uint size, uint count, method pDtor)
	{
		<Module>.__ehvec_dtor(ptr, size, (int)count, pDtor);
	}

	// Token: 0x060000C8 RID: 200 RVA: 0x000170F8 File Offset: 0x000164F8
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	[HandleProcessCorruptedStateExceptions]
	internal unsafe static void __ArrayUnwind(void* ptr, uint size, int count, method pDtor)
	{
		_EXCEPTION_POINTERS* exceptionPointers;
		EHExceptionRecord* ptr2;
		int num;
		try
		{
			for (;;)
			{
				count--;
				if (count < 0)
				{
					break;
				}
				ptr = (void*)((byte*)ptr - size);
				calli(System.Void(System.Void*), ptr, pDtor);
			}
		}
		catch when (delegate
		{
			// Failed to create a 'catch-when' expression
			exceptionPointers = Marshal.GetExceptionPointers();
			ptr2 = *(int*)exceptionPointers;
			if (*(int*)ptr2 == -529697949)
			{
				<Module>.terminate();
			}
			num = 0;
			endfilter(num != 0);
		})
		{
		}
	}

	// Token: 0x060000C9 RID: 201 RVA: 0x000176D8 File Offset: 0x00016AD8
	internal static void <CrtImplementationDetails>.ThrowNestedModuleLoadException(Exception innerException, Exception nestedException)
	{
		throw new ModuleLoadExceptionHandlerException("A nested exception occurred after the primary exception that caused the C++ module to fail to load.\n", innerException, nestedException);
	}

	// Token: 0x060000CA RID: 202 RVA: 0x00017388 File Offset: 0x00016788
	internal static void <CrtImplementationDetails>.ThrowModuleLoadException(string errorMessage)
	{
		throw new ModuleLoadException(errorMessage);
	}

	// Token: 0x060000CB RID: 203 RVA: 0x000173A4 File Offset: 0x000167A4
	internal static void <CrtImplementationDetails>.ThrowModuleLoadException(string errorMessage, Exception innerException)
	{
		throw new ModuleLoadException(errorMessage, innerException);
	}

	// Token: 0x060000CC RID: 204 RVA: 0x00017508 File Offset: 0x00016908
	internal static void <CrtImplementationDetails>.RegisterModuleUninitializer(EventHandler handler)
	{
		ModuleUninitializer._ModuleUninitializer.AddHandler(handler);
	}

	// Token: 0x060000CD RID: 205 RVA: 0x00017528 File Offset: 0x00016928
	[HandleProcessCorruptedStateExceptions]
	internal unsafe static int __get_default_appdomain(IUnknown** ppUnk)
	{
		int num = 0;
		IUnknown* ptr = null;
		ICorRuntimeHost* ptr2 = null;
		try
		{
			num = <Module>.CoCreateInstance(ref <Module>._GUID_cb2f6723_ab3a_11d2_9c40_00c04fa30a3e, null, 1, ref <Module>._GUID_00000000_0000_0000_c000_000000000046, (void**)(&ptr));
			if (num >= 0)
			{
				num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,_GUID modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.IsImplicitlyDereferenced),System.Void**), ptr, ref <Module>._GUID_cb2f6722_ab3a_11d2_9c40_00c04fa30a3e, ref ptr2, *(*(int*)ptr));
				if (num >= 0)
				{
					num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,IUnknown**), ptr2, ppUnk, *(*(int*)ptr2 + 52));
				}
			}
		}
		finally
		{
			if (ptr != null)
			{
				IUnknown* ptr3 = ptr;
				object obj = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr3, *(*(int*)ptr3 + 8));
			}
			if (ptr2 != null)
			{
				ICorRuntimeHost* ptr4 = ptr2;
				object obj2 = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr4, *(*(int*)ptr4 + 8));
			}
		}
		return num;
	}

	// Token: 0x060000CE RID: 206 RVA: 0x000175BC File Offset: 0x000169BC
	internal unsafe static AppDomain <CrtImplementationDetails>.GetDefaultDomain()
	{
		IUnknown* ptr = null;
		int num = <Module>.__get_default_appdomain(&ptr);
		if (num >= 0)
		{
			try
			{
				IntPtr pUnk = new IntPtr((void*)ptr);
				return (AppDomain)Marshal.GetObjectForIUnknown(pUnk);
			}
			finally
			{
				IUnknown* ptr2 = ptr;
				object obj = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr2, *(*(int*)ptr2 + 8));
			}
		}
		Marshal.ThrowExceptionForHR(num);
		return null;
	}

	// Token: 0x060000CF RID: 207 RVA: 0x00017628 File Offset: 0x00016A28
	internal unsafe static void <CrtImplementationDetails>.DoCallBackInDefaultDomain(method function, void* cookie)
	{
		ICLRRuntimeHost* ptr = null;
		try
		{
			int num = <Module>.CorBindToRuntimeEx(null, null, 0, ref <Module>._GUID_90f1a06e_7712_4762_86b5_7a5eba6bdb02, ref <Module>._GUID_90f1a06c_7712_4762_86b5_7a5eba6bdb02, (void**)(&ptr));
			if (num < 0)
			{
				Marshal.ThrowExceptionForHR(num);
			}
			AppDomain appDomain = <Module>.<CrtImplementationDetails>.GetDefaultDomain();
			int num2 = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,System.UInt32 modopt(System.Runtime.CompilerServices.IsLong),System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall) (System.Void*),System.Void*), ptr, appDomain.Id, function, cookie, *(*(int*)ptr + 32));
			if (num2 < 0)
			{
				Marshal.ThrowExceptionForHR(num2);
			}
		}
		finally
		{
			if (ptr != null)
			{
				ICLRRuntimeHost* ptr2 = ptr;
				object obj = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr2, *(*(int*)ptr2 + 8));
			}
		}
	}

	// Token: 0x060000D0 RID: 208 RVA: 0x0001775C File Offset: 0x00016B5C
	[DebuggerStepThrough]
	internal unsafe static int _initterm_e(method* pfbegin, method* pfend)
	{
		int num = 0;
		if (pfbegin < pfend)
		{
			while (num == 0)
			{
				uint num2 = (uint)(*(int*)pfbegin);
				if (num2 != 0U)
				{
					num = calli(System.Int32 modopt(System.Runtime.CompilerServices.CallConvCdecl)(), num2);
				}
				pfbegin += 4 / sizeof(method);
				if (pfbegin >= pfend)
				{
					break;
				}
			}
		}
		return num;
	}

	// Token: 0x060000D1 RID: 209 RVA: 0x00017790 File Offset: 0x00016B90
	[DebuggerStepThrough]
	internal unsafe static void _initterm(method* pfbegin, method* pfend)
	{
		if (pfbegin < pfend)
		{
			do
			{
				uint num = (uint)(*(int*)pfbegin);
				if (num != 0U)
				{
					calli(System.Void modopt(System.Runtime.CompilerServices.CallConvCdecl)(), num);
				}
				pfbegin += 4 / sizeof(method);
			}
			while (pfbegin < pfend);
		}
	}

	// Token: 0x060000D2 RID: 210 RVA: 0x000177BC File Offset: 0x00016BBC
	[DebuggerStepThrough]
	internal static ModuleHandle <CrtImplementationDetails>.ThisModule.Handle()
	{
		return typeof(ThisModule).Module.ModuleHandle;
	}

	// Token: 0x060000D3 RID: 211 RVA: 0x00017814 File Offset: 0x00016C14
	[DebuggerStepThrough]
	internal unsafe static void _initterm_m(method* pfbegin, method* pfend)
	{
		if (pfbegin < pfend)
		{
			do
			{
				uint num = (uint)(*(int*)pfbegin);
				if (num != 0U)
				{
					object obj = calli(System.Void modopt(System.Runtime.CompilerServices.IsConst)*(), <Module>.<CrtImplementationDetails>.ThisModule.ResolveMethod<void\u0020const\u0020*\u0020__clrcall(void)>(num));
				}
				pfbegin += 4 / sizeof(method);
			}
			while (pfbegin < pfend);
		}
	}

	// Token: 0x060000D4 RID: 212 RVA: 0x000177E4 File Offset: 0x00016BE4
	[DebuggerStepThrough]
	internal static method <CrtImplementationDetails>.ThisModule.ResolveMethod<void\u0020const\u0020*\u0020__clrcall(void)>(method methodToken)
	{
		return <Module>.<CrtImplementationDetails>.ThisModule.Handle().ResolveMethodHandle(methodToken).GetFunctionPointer().ToPointer();
	}

	// Token: 0x060000D5 RID: 213 RVA: 0x00017848 File Offset: 0x00016C48
	internal unsafe static int __FrameUnwindFilter(_EXCEPTION_POINTERS* pExPtrs)
	{
		_exception_handling_state_pointers_t exception_handling_state_pointers_t = 0;
		initblk(ref exception_handling_state_pointers_t + 4, 0, 20);
		uint num = (uint)(*(*(int*)pExPtrs));
		if (num != 3762504530U && num != 3762507597U)
		{
			if (num != 3765269347U)
			{
				return 0;
			}
			*(*<Module>._get_exception_handling_state(&exception_handling_state_pointers_t)) = 0;
			<Module>.terminate();
		}
		if (*(*<Module>._get_exception_handling_state(&exception_handling_state_pointers_t)) > 0)
		{
			*(*<Module>._get_exception_handling_state(&exception_handling_state_pointers_t)) += -1;
		}
		return 0;
	}

	// Token: 0x060000D6 RID: 214 RVA: 0x000178B0 File Offset: 0x00016CB0
	[PrePrepareMethod]
	[SecurityPermission(SecurityAction.Assert, UnmanagedCode = true)]
	internal unsafe static int GetDllBaseAndSizeFromAddress(void* Address, void** OutBase, uint* OutSize)
	{
		_MEMORY_BASIC_INFORMATION memory_BASIC_INFORMATION = 0;
		initblk(ref memory_BASIC_INFORMATION + 4, 0, 24);
		int result = 0;
		if (OutBase != null)
		{
			*(int*)OutBase = 0;
		}
		if (OutSize != null)
		{
			*(int*)OutSize = 0;
		}
		if (Address != null && OutBase != null && OutSize != null && <Module>.VirtualQuery((void*)Address, &memory_BASIC_INFORMATION, 28) == 28)
		{
			void* ptr = *(ref memory_BASIC_INFORMATION + 4);
			do
			{
				Address = *(ref memory_BASIC_INFORMATION + 12) + memory_BASIC_INFORMATION;
				if (<Module>.VirtualQuery((void*)Address, &memory_BASIC_INFORMATION, 28) != 28)
				{
					return result;
				}
			}
			while (ptr == *(ref memory_BASIC_INFORMATION + 4));
			*(int*)OutBase = ptr;
			*(int*)OutSize = (int)(Address - ptr);
			result = 1;
		}
		return result;
	}

	// Token: 0x060000D7 RID: 215 RVA: 0x00017928 File Offset: 0x00016D28
	[PrePrepareMethod]
	internal unsafe static int IsPointerInMsvcrtDll(void* p, int* Result)
	{
		int result = 0;
		if (Result != null)
		{
			*Result = 0;
		}
		if (p != null && Result != null)
		{
			if (<Module>.?A0x47ab7201.?Begin@?1??IsPointerInMsvcrtDll@?A0x47ab7201@@YAHPAXPAH@Z@4PAXA == null || <Module>.?A0x47ab7201.?End@?1??IsPointerInMsvcrtDll@?A0x47ab7201@@YAHPAXPAH@Z@4PAXA == null)
			{
				uint num;
				if (<Module>.?A0x47ab7201.GetDllBaseAndSizeFromAddress((void*)<Module>.__unep@?_errno@@$$J0YAPAHXZ, &<Module>.?A0x47ab7201.?Begin@?1??IsPointerInMsvcrtDll@?A0x47ab7201@@YAHPAXPAH@Z@4PAXA, &num) == null)
				{
					return result;
				}
				<Module>.?A0x47ab7201.?End@?1??IsPointerInMsvcrtDll@?A0x47ab7201@@YAHPAXPAH@Z@4PAXA = (void*)((byte*)<Module>.?A0x47ab7201.?Begin@?1??IsPointerInMsvcrtDll@?A0x47ab7201@@YAHPAXPAH@Z@4PAXA + num);
			}
			int num2;
			if (p >= <Module>.?A0x47ab7201.?Begin@?1??IsPointerInMsvcrtDll@?A0x47ab7201@@YAHPAXPAH@Z@4PAXA && p < <Module>.?A0x47ab7201.?End@?1??IsPointerInMsvcrtDll@?A0x47ab7201@@YAHPAXPAH@Z@4PAXA)
			{
				num2 = 1;
			}
			else
			{
				num2 = 0;
			}
			*Result = num2;
			result = 1;
		}
		return result;
	}

	// Token: 0x060000D8 RID: 216 RVA: 0x00017994 File Offset: 0x00016D94
	[PrePrepareMethod]
	[SecurityPermission(SecurityAction.Assert, UnmanagedCode = true)]
	internal unsafe static _tiddata_managed* _getptd_noexit()
	{
		uint lastError = <Module>.GetLastError();
		_tiddata_managed* result = null;
		if ((<Module>.GetVersion() & 255) >= 6)
		{
			int* ptr = <Module>._errno();
			int num = 0;
			if (<Module>.?A0x47ab7201.IsPointerInMsvcrtDll(ptr, &num) != null && num == 0)
			{
				result = ptr - 8;
			}
		}
		<Module>.SetLastError(lastError);
		return result;
	}

	// Token: 0x060000D9 RID: 217 RVA: 0x000179DC File Offset: 0x00016DDC
	internal unsafe static void* GetKernelProc(sbyte* pszFunction)
	{
		void* result = null;
		HINSTANCE__* moduleHandleA = <Module>.GetModuleHandleA((sbyte*)(&<Module>.??_C@_0P@OEFGOMJK@kernelbase?4dll?$AA@));
		if (moduleHandleA == null)
		{
			moduleHandleA = <Module>.GetModuleHandleA((sbyte*)(&<Module>.??_C@_0N@MDJJJHMB@kernel32?4dll?$AA@));
			if (moduleHandleA == null)
			{
				return result;
			}
		}
		result = <Module>.GetProcAddress(moduleHandleA, pszFunction);
		return result;
	}

	// Token: 0x060000DA RID: 218 RVA: 0x00017A18 File Offset: 0x00016E18
	internal unsafe static void* _encode_pointer(void* ptr)
	{
		if (!<Module>.?A0x00a411d4.?fInitialized@?1??_encode_pointer@@YAPAXPAX@Z@4_NA)
		{
			<Module>.?A0x00a411d4.?pfnEncodePointer@?1??_encode_pointer@@YAPAXPAX@Z@4P6GPAX0@ZA = <Module>.GetKernelProc((sbyte*)(&<Module>.??_C@_0O@NLDKAIKN@EncodePointer?$AA@));
			<Module>.?A0x00a411d4.?fInitialized@?1??_encode_pointer@@YAPAXPAX@Z@4_NA = true;
		}
		if (<Module>.?A0x00a411d4.?pfnEncodePointer@?1??_encode_pointer@@YAPAXPAX@Z@4P6GPAX0@ZA != null)
		{
			ptr = calli(System.Void* modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Void*), ptr, <Module>.?A0x00a411d4.?pfnEncodePointer@?1??_encode_pointer@@YAPAXPAX@Z@4P6GPAX0@ZA);
		}
		return ptr;
	}

	// Token: 0x060000DB RID: 219 RVA: 0x00017A5C File Offset: 0x00016E5C
	internal unsafe static void* _encoded_null()
	{
		return <Module>._encode_pointer(null);
	}

	// Token: 0x060000DC RID: 220 RVA: 0x00017A74 File Offset: 0x00016E74
	internal unsafe static void* _decode_pointer(void* codedptr)
	{
		if (!<Module>.?A0x00a411d4.?fInitialized@?1??_decode_pointer@@YAPAXPAX@Z@4_NA)
		{
			<Module>.?A0x00a411d4.?pfnDecodePointer@?1??_decode_pointer@@YAPAXPAX@Z@4P6GPAX0@ZA = <Module>.GetKernelProc((sbyte*)(&<Module>.??_C@_0O@KBPMFGHI@DecodePointer?$AA@));
			<Module>.?A0x00a411d4.?fInitialized@?1??_decode_pointer@@YAPAXPAX@Z@4_NA = true;
		}
		if (<Module>.?A0x00a411d4.?pfnDecodePointer@?1??_decode_pointer@@YAPAXPAX@Z@4P6GPAX0@ZA != null)
		{
			codedptr = calli(System.Void* modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Void*), codedptr, <Module>.?A0x00a411d4.?pfnDecodePointer@?1??_decode_pointer@@YAPAXPAX@Z@4P6GPAX0@ZA);
		}
		return codedptr;
	}

	// Token: 0x060000DD RID: 221 RVA: 0x00017AB8 File Offset: 0x00016EB8
	internal unsafe static ushort _putwc_nolock([MarshalAs(UnmanagedType.U2)] char _c, _iobuf* _stream)
	{
		*(int*)(_stream + 4 / sizeof(_iobuf)) = *(int*)(_stream + 4 / sizeof(_iobuf)) + -2;
		if (*(int*)(_stream + 4 / sizeof(_iobuf)) >= 0)
		{
			*(*(int*)_stream) = (short)_c;
			*(int*)_stream = *(int*)_stream + 2;
			return _c;
		}
		*(int*)(_stream + 12 / sizeof(_iobuf)) = (*(int*)(_stream + 12 / sizeof(_iobuf)) | 32);
		return 65535;
	}

	// Token: 0x060000DE RID: 222 RVA: 0x00017AFC File Offset: 0x00016EFC
	internal unsafe static int _swoutput_s(char* _Dst, uint _Size, char* _Format, sbyte* _ArgList)
	{
		if (_Size == 0U)
		{
			*<Module>._errno() = 22;
			<Module>._invalid_parameter(null, null, null, 0U, 0U);
			return -1;
		}
		_iobuf iobuf;
		if (_Size == 4294967295U)
		{
			*(ref iobuf + 4) = int.MaxValue;
		}
		else
		{
			if (_Size > 1073741823U)
			{
				*<Module>._errno() = 22;
				<Module>._invalid_parameter(null, null, null, 0U, 0U);
				return -1;
			}
			*(ref iobuf + 4) = (int)((int)_Size << 1);
		}
		*(ref iobuf + 8) = _Dst;
		iobuf = _Dst;
		*(ref iobuf + 12) = 66;
		int num = <Module>._woutput_s(&iobuf, _Format, _ArgList);
		*(_Size + _Dst - 1) = '\0';
		if (num >= 0)
		{
			*(ref iobuf + 4) = *(ref iobuf + 4) - 1;
			int num2 = *(ref iobuf + 4);
			if (num2 >= 0)
			{
				*iobuf = 0;
				iobuf++;
			}
			else if (<Module>._flsbuf_s(0, &iobuf) == -1)
			{
				return -2;
			}
			*(ref iobuf + 4) = *(ref iobuf + 4) - 1;
			num2 = *(ref iobuf + 4);
			if (num2 >= 0)
			{
				*iobuf = 0;
			}
			else if (<Module>._flsbuf_s(0, &iobuf) == -1)
			{
				return -2;
			}
			return num;
		}
		if (*(ref iobuf + 4) < 0)
		{
			return -2;
		}
		if (_Dst != null && _Size > 0U)
		{
			*_Dst = '\0';
		}
		return num;
	}

	// Token: 0x060000DF RID: 223 RVA: 0x00017BF8 File Offset: 0x00016FF8
	internal unsafe static void write_char([MarshalAs(UnmanagedType.U2)] char ch, _iobuf* f, int* pnumwritten)
	{
		if ((*(int*)(f + 12 / sizeof(_iobuf)) & 64) != 0 && *(int*)(f + 8 / sizeof(_iobuf)) == 0)
		{
			(*pnumwritten)++;
		}
		else if (<Module>.?A0xf690dbd3._putwc_nolock(ch, f) == 65535 && <Module>.ferror(f) != null)
		{
			*pnumwritten = -1;
		}
		else
		{
			(*pnumwritten)++;
		}
	}

	// Token: 0x060000E0 RID: 224 RVA: 0x00017C44 File Offset: 0x00017044
	internal unsafe static void write_multi_char([MarshalAs(UnmanagedType.U2)] char ch, int num, _iobuf* f, int* pnumwritten)
	{
		if (num > 0)
		{
			do
			{
				num--;
				<Module>.?A0xf690dbd3.write_char(ch, f, pnumwritten);
			}
			while (*pnumwritten != -1 && num > 0);
		}
	}

	// Token: 0x060000E1 RID: 225 RVA: 0x00017C70 File Offset: 0x00017070
	internal unsafe static void write_string(char* @string, int len, _iobuf* f, int* pnumwritten)
	{
		if ((*(int*)(f + 12 / sizeof(_iobuf)) & 64) != 0 && *(int*)(f + 8 / sizeof(_iobuf)) == 0)
		{
			*pnumwritten += len;
		}
		else if (len > 0)
		{
			do
			{
				len--;
				<Module>.?A0xf690dbd3.write_char(*@string, f, pnumwritten);
				@string++;
				if (*pnumwritten == -1)
				{
					if (*<Module>._errno() != 42)
					{
						break;
					}
					<Module>.?A0xf690dbd3.write_char('?', f, pnumwritten);
				}
			}
			while (len > 0);
		}
	}

	// Token: 0x060000E2 RID: 226 RVA: 0x0001884C File Offset: 0x00017C4C
	[PrePrepareMethod]
	internal unsafe static _exception_handling_state_pointers_t* _get_exception_handling_state(_exception_handling_state_pointers_t* p)
	{
		return <Module>._get_exception_handling_state_uplevel(p);
	}

	// Token: 0x060000E3 RID: 227 RVA: 0x00018864 File Offset: 0x00017C64
	internal unsafe static int _flsbuf_s(int ch, _iobuf* str)
	{
		int num = <Module>._fileno(str);
		int num2 = *(int*)(str + 12 / sizeof(_iobuf));
		if ((num2 & 130) == 0)
		{
			*<Module>._errno() = 9;
			*(int*)(str + 12 / sizeof(_iobuf)) = (*(int*)(str + 12 / sizeof(_iobuf)) | 32);
			return -1;
		}
		if ((num2 & 64) != 0)
		{
			*<Module>._errno() = 34;
			*(int*)(str + 12 / sizeof(_iobuf)) = (*(int*)(str + 12 / sizeof(_iobuf)) | 32);
			return -1;
		}
		if ((num2 & 1) != 0)
		{
			*(int*)(str + 4 / sizeof(_iobuf)) = 0;
			if ((num2 & 16) == 0)
			{
				*(int*)(str + 12 / sizeof(_iobuf)) = (num2 | 32);
				return -1;
			}
			*(int*)str = *(int*)(str + 8 / sizeof(_iobuf));
			*(int*)(str + 12 / sizeof(_iobuf)) = (num2 & -2);
		}
		int num3 = (*(int*)(str + 12 / sizeof(_iobuf)) & -17) | 2;
		*(int*)(str + 12 / sizeof(_iobuf)) = num3;
		*(int*)(str + 4 / sizeof(_iobuf)) = 0;
		int num4 = 0;
		if ((num3 & 268) == 0 && ((str != (_iobuf*)(<Module>.__imp__iob + 32) && str != (_iobuf*)(<Module>.__imp__iob + 64)) || <Module>._isatty(num) == null))
		{
			*<Module>._errno() = 22;
			<Module>._invalid_parameter(null, null, null, 0U, 0U);
			return -1;
		}
		int num6;
		if ((*(int*)(str + 12 / sizeof(_iobuf)) & 264) != 0)
		{
			int num5 = *(int*)(str + 8 / sizeof(_iobuf));
			num6 = *(int*)str - num5;
			*(int*)str = num5 + 1;
			*(int*)(str + 4 / sizeof(_iobuf)) = *(int*)(str + 24 / sizeof(_iobuf)) - 1;
			if (num6 > 0)
			{
				num4 = <Module>._write(num, num5, (uint)num6);
			}
			else
			{
				ioinfo* ptr;
				if (num != -1 && num != -2)
				{
					ptr = (num & 31) * 36 + *(int*)((num >> 5) * 4 + <Module>.__imp___pioinfo);
				}
				else
				{
					ptr = <Module>.__imp___badioinfo;
				}
				if ((*(sbyte*)(ptr + 4 / sizeof(ioinfo)) & 32) != 0 && <Module>._lseeki64(num, 0L, 2) == -1L)
				{
					*(int*)(str + 12 / sizeof(_iobuf)) = (*(int*)(str + 12 / sizeof(_iobuf)) | 32);
					return -1;
				}
			}
			*(*(int*)(str + 8 / sizeof(_iobuf))) = (byte)ch;
		}
		else
		{
			num6 = 1;
			num4 = <Module>._write(num, (void*)(&ch), 1U);
		}
		if (num4 != num6)
		{
			*(int*)(str + 12 / sizeof(_iobuf)) = (*(int*)(str + 12 / sizeof(_iobuf)) | 32);
			return -1;
		}
		return ch & 255;
	}

	// Token: 0x060000E4 RID: 228 RVA: 0x00018A08 File Offset: 0x00017E08
	internal unsafe static int _safecrt_cfltcvt(_CRT_DOUBLE* arg, sbyte* buffer, uint _SizeInBytes, int type, int precision, int flags)
	{
		if (_SizeInBytes == 0U)
		{
			*(byte*)buffer = 0;
			return 22;
		}
		if ((flags & 1) != 0)
		{
			type -= 32;
		}
		$ArrayType$$$BY0BO@D $ArrayType$$$BY0BO@D = 37;
		uint num = 1U;
		if ((flags & 128) != 0)
		{
			*(ref $ArrayType$$$BY0BO@D + 1) = 35;
			num = 2U;
		}
		*(num + ref $ArrayType$$$BY0BO@D) = 46;
		<Module>._itoa(precision, num + (ref $ArrayType$$$BY0BO@D + 1), 10);
		sbyte* ptr = ref $ArrayType$$$BY0BO@D;
		if ($ArrayType$$$BY0BO@D != null)
		{
			do
			{
				ptr++;
			}
			while (*ptr != 0);
		}
		num = ptr - ref $ArrayType$$$BY0BO@D;
		*(num + ref $ArrayType$$$BY0BO@D) = (byte)type;
		*(num + (ref $ArrayType$$$BY0BO@D + 1)) = 0;
		*(byte*)(_SizeInBytes / (uint)sizeof(sbyte) + buffer - 1 / sizeof(sbyte)) = 0;
		int num2 = <Module>._snprintf(buffer, _SizeInBytes, (sbyte*)(&$ArrayType$$$BY0BO@D), *(double*)arg);
		if (*(sbyte*)(_SizeInBytes / (uint)sizeof(sbyte) + buffer - 1 / sizeof(sbyte)) == 0 && num2 > 0)
		{
			return 0;
		}
		*(byte*)buffer = 0;
		return 22;
	}

	// Token: 0x060000E5 RID: 229 RVA: 0x0001905E File Offset: 0x0001845E
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern void* malloc(uint);

	// Token: 0x060000E6 RID: 230 RVA: 0x00019762 File Offset: 0x00018B62
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern void DeleteCriticalSection(_RTL_CRITICAL_SECTION*);

	// Token: 0x060000E7 RID: 231 RVA: 0x00019052 File Offset: 0x00018452
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern void free(void*);

	// Token: 0x060000E8 RID: 232 RVA: 0x000197F2 File Offset: 0x00018BF2
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern void _CxxThrowException(void*, _s__ThrowInfo*);

	// Token: 0x060000E9 RID: 233 RVA: 0x00019756 File Offset: 0x00018B56
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal static extern uint GetLastError();

	// Token: 0x060000EA RID: 234 RVA: 0x0001974A File Offset: 0x00018B4A
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal static extern uint GetVersion();

	// Token: 0x060000EB RID: 235 RVA: 0x00019032 File Offset: 0x00018432
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern void delete(void*);

	// Token: 0x060000EC RID: 236 RVA: 0x000197DA File Offset: 0x00018BDA
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern void CoTaskMemFree(void*);

	// Token: 0x060000ED RID: 237 RVA: 0x000198B4 File Offset: 0x00018CB4
	[SuppressUnmanagedCodeSecurity]
	[MethodImpl(MethodImplOptions.Unmanaged | MethodImplOptions.PreserveSig)]
	internal unsafe static extern void* @new(uint);

	// Token: 0x060000EE RID: 238 RVA: 0x0001269D File Offset: 0x00011A9D
	[SuppressUnmanagedCodeSecurity]
	[MethodImpl(MethodImplOptions.Unmanaged | MethodImplOptions.PreserveSig)]
	internal unsafe static extern ushort* UfphNativeStrFormat(ushort*, __arglist);

	// Token: 0x060000EF RID: 239 RVA: 0x00015FBA File Offset: 0x000153BA
	[SuppressUnmanagedCodeSecurity]
	[MethodImpl(MethodImplOptions.Unmanaged | MethodImplOptions.PreserveSig)]
	internal unsafe static extern void* _getFiberPtrId();

	// Token: 0x060000F0 RID: 240 RVA: 0x000197FE File Offset: 0x00018BFE
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal static extern void _cexit();

	// Token: 0x060000F1 RID: 241 RVA: 0x000192D2 File Offset: 0x000186D2
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal static extern void _amsg_exit(int);

	// Token: 0x060000F2 RID: 242 RVA: 0x0001976E File Offset: 0x00018B6E
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal static extern void Sleep(uint);

	// Token: 0x060000F3 RID: 243 RVA: 0x00019822 File Offset: 0x00018C22
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal static extern int _callnewh(uint);

	// Token: 0x060000F4 RID: 244 RVA: 0x00019130 File Offset: 0x00018530
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.ThisCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern exception* {ctor}(exception*, exception*);

	// Token: 0x060000F5 RID: 245 RVA: 0x0001980A File Offset: 0x00018C0A
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.ThisCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern exception* {ctor}(exception*);

	// Token: 0x060000F6 RID: 246 RVA: 0x00019816 File Offset: 0x00018C16
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.ThisCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern void {dtor}(exception*);

	// Token: 0x060000F7 RID: 247 RVA: 0x0001913C File Offset: 0x0001853C
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern int* _errno();

	// Token: 0x060000F8 RID: 248 RVA: 0x000191EA File Offset: 0x000185EA
	[SuppressUnmanagedCodeSecurity]
	[MethodImpl(MethodImplOptions.Unmanaged | MethodImplOptions.PreserveSig)]
	internal unsafe static extern void _invalid_parameter(char*, char*, char*, uint, uint);

	// Token: 0x060000F9 RID: 249 RVA: 0x0001982E File Offset: 0x00018C2E
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal static extern void terminate();

	// Token: 0x060000FA RID: 250 RVA: 0x000197E6 File Offset: 0x00018BE6
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern int CoCreateInstance(_GUID*, IUnknown*, uint, _GUID*, void**);

	// Token: 0x060000FB RID: 251 RVA: 0x000197CE File Offset: 0x00018BCE
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern int CorBindToRuntimeEx(char*, char*, uint, _GUID*, _GUID*, void**);

	// Token: 0x060000FC RID: 252 RVA: 0x0001977C File Offset: 0x00018B7C
	[SuppressUnmanagedCodeSecurity]
	[MethodImpl(MethodImplOptions.Unmanaged | MethodImplOptions.PreserveSig)]
	internal unsafe static extern void* _CallSettingFrame(void*, EHRegistrationNode*, uint);

	// Token: 0x060000FD RID: 253 RVA: 0x000198E2 File Offset: 0x00018CE2
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern uint VirtualQuery(void*, _MEMORY_BASIC_INFORMATION*, uint);

	// Token: 0x060000FE RID: 254 RVA: 0x000198EE File Offset: 0x00018CEE
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal static extern void SetLastError(uint);

	// Token: 0x060000FF RID: 255 RVA: 0x00019906 File Offset: 0x00018D06
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern method GetProcAddress(HINSTANCE__*, sbyte*);

	// Token: 0x06000100 RID: 256 RVA: 0x000198FA File Offset: 0x00018CFA
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern HINSTANCE__* GetModuleHandleA(sbyte*);

	// Token: 0x06000101 RID: 257 RVA: 0x00017CCD File Offset: 0x000170CD
	[SuppressUnmanagedCodeSecurity]
	[MethodImpl(MethodImplOptions.Unmanaged | MethodImplOptions.PreserveSig)]
	internal unsafe static extern int _woutput_s(_iobuf*, char*, sbyte*);

	// Token: 0x06000102 RID: 258 RVA: 0x0001983A File Offset: 0x00018C3A
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern int ferror(_iobuf*);

	// Token: 0x06000103 RID: 259 RVA: 0x000192BA File Offset: 0x000186BA
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern int _fileno(_iobuf*);

	// Token: 0x06000104 RID: 260 RVA: 0x00019852 File Offset: 0x00018C52
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern int _write(int, void*, uint);

	// Token: 0x06000105 RID: 261 RVA: 0x00019846 File Offset: 0x00018C46
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal static extern int _isatty(int);

	// Token: 0x06000106 RID: 262 RVA: 0x0001985E File Offset: 0x00018C5E
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal static extern long _lseeki64(int, long, int);

	// Token: 0x06000107 RID: 263 RVA: 0x0001986A File Offset: 0x00018C6A
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern sbyte* _itoa(int, sbyte*, int);

	// Token: 0x06000108 RID: 264 RVA: 0x00019876 File Offset: 0x00018C76
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern int _snprintf(sbyte*, uint, sbyte*, __arglist);

	// Token: 0x04000001 RID: 1 RVA: 0x000017F4 File Offset: 0x00000BF4
	internal static $ArrayType$$$BY01$$CBD ??_C@_01CPLAODJH@S?$AA@;

	// Token: 0x04000002 RID: 2 RVA: 0x000017F8 File Offset: 0x00000BF8
	internal static $ArrayType$$$BY01$$CBD ??_C@_01PLPBNMEI@M?$AA@;

	// Token: 0x04000003 RID: 3 RVA: 0x000017FC File Offset: 0x00000BFC
	internal static $ArrayType$$$BY01$$CBD ??_C@_01CKDDGHAB@D?$AA@;

	// Token: 0x04000004 RID: 4 RVA: 0x00001800 File Offset: 0x00000C00
	internal static $ArrayType$$$BY01$$CBD ??_C@_01HMGJMAIH@B?$AA@;

	// Token: 0x04000005 RID: 5 RVA: 0x00001804 File Offset: 0x00000C04
	internal static $ArrayType$$$BY03$$CBD ??_C@_03LAIAPFCB@Val?$AA@;

	// Token: 0x04000006 RID: 6 RVA: 0x00001808 File Offset: 0x00000C08
	internal static $ArrayType$$$BY0M@$$CBD ??_C@_0M@DBGDLGLL@ForceRemove?$AA@;

	// Token: 0x04000007 RID: 7 RVA: 0x00001814 File Offset: 0x00000C14
	internal static $ArrayType$$$BY08$$CBD ??_C@_08KAAPDIAN@NoRemove?$AA@;

	// Token: 0x04000008 RID: 8 RVA: 0x00001820 File Offset: 0x00000C20
	internal static $ArrayType$$$BY06$$CBD ??_C@_06JBKGCNBB@Delete?$AA@;

	// Token: 0x04000009 RID: 9 RVA: 0x00001864 File Offset: 0x00000C64
	internal static $PTMType$QQtagVARIANT@@J ?pmField@?$CVarTypeInfo@J@ATL@@2QQtagVARIANT@@JQ3@;

	// Token: 0x0400000A RID: 10 RVA: 0x00001860 File Offset: 0x00000C60
	internal static $PTMType$QQtagVARIANT@@PAPAG ?pmField@?$CVarTypeInfo@PAPAG@ATL@@2QQtagVARIANT@@PAPAGQ3@;

	// Token: 0x0400000B RID: 11 RVA: 0x0000186C File Offset: 0x00000C6C
	internal static $PTMType$QQtagVARIANT@@PAG ?pmField@?$CVarTypeInfo@PAG@ATL@@2QQtagVARIANT@@PAGQ3@;

	// Token: 0x0400000C RID: 12 RVA: 0x00001830 File Offset: 0x00000C30
	internal static $PTMType$QQtagVARIANT@@PAM ?pmField@?$CVarTypeInfo@PAM@ATL@@2QQtagVARIANT@@PAMQ3@;

	// Token: 0x0400000D RID: 13 RVA: 0x0001AA34 File Offset: 0x00019A34
	internal static CAtlReleaseManagedClassFactories _AtlReleaseManagedClassFactories;

	// Token: 0x0400000E RID: 14 RVA: 0x00001040 File Offset: 0x00000440
	internal static method _AtlReleaseManagedClassFactories$initializer$;

	// Token: 0x0400000F RID: 15 RVA: 0x0000182C File Offset: 0x00000C2C
	internal static $PTMType$QQtagVARIANT@@PAN ?pmField@?$CVarTypeInfo@PAN@ATL@@2QQtagVARIANT@@PANQ3@;

	// Token: 0x04000010 RID: 16 RVA: 0x00001848 File Offset: 0x00000C48
	internal static $PTMType$QQtagVARIANT@@_K ?pmField@?$CVarTypeInfo@_K@ATL@@2QQtagVARIANT@@_KQ3@;

	// Token: 0x04000011 RID: 17 RVA: 0x00001868 File Offset: 0x00000C68
	internal static $PTMType$QQtagVARIANT@@E ?pmField@?$CVarTypeInfo@E@ATL@@2QQtagVARIANT@@EQ3@;

	// Token: 0x04000012 RID: 18 RVA: 0x00001870 File Offset: 0x00000C70
	internal static $PTMType$QQtagVARIANT@@PATtagCY@@ ?pmField@?$CVarTypeInfo@PATtagCY@@@ATL@@2QQtagVARIANT@@PATtagCY@@Q3@;

	// Token: 0x04000013 RID: 19 RVA: 0x00001854 File Offset: 0x00000C54
	internal static $PTMType$QQtagVARIANT@@G ?pmField@?$CVarTypeInfo@G@ATL@@2QQtagVARIANT@@GQ3@;

	// Token: 0x04000014 RID: 20 RVA: 0x00001828 File Offset: 0x00000C28
	internal static $PTMType$QQtagVARIANT@@K ?pmField@?$CVarTypeInfo@K@ATL@@2QQtagVARIANT@@KQ3@;

	// Token: 0x04000015 RID: 21 RVA: 0x00001858 File Offset: 0x00000C58
	internal static $PTMType$QQtagVARIANT@@PAF ?pmField@?$CVarTypeInfo@PAF@ATL@@2QQtagVARIANT@@PAFQ3@;

	// Token: 0x04000016 RID: 22 RVA: 0x00001890 File Offset: 0x00000C90
	internal static $PTMType$QQtagVARIANT@@PAI ?pmField@?$CVarTypeInfo@PAI@ATL@@2QQtagVARIANT@@PAIQ3@;

	// Token: 0x04000017 RID: 23 RVA: 0x0000188C File Offset: 0x00000C8C
	internal static $PTMType$QQtagVARIANT@@PAJ ?pmField@?$CVarTypeInfo@PAJ@ATL@@2QQtagVARIANT@@PAJQ3@;

	// Token: 0x04000018 RID: 24 RVA: 0x00001840 File Offset: 0x00000C40
	internal static $PTMType$QQtagVARIANT@@PAD ?pmField@?$CVarTypeInfo@PAD@ATL@@2QQtagVARIANT@@PADQ3@;

	// Token: 0x04000019 RID: 25 RVA: 0x00001850 File Offset: 0x00000C50
	internal static $PTMType$QQtagVARIANT@@PAUIUnknown@@ ?pmField@?$CVarTypeInfo@PAUIUnknown@@@ATL@@2QQtagVARIANT@@PAUIUnknown@@Q3@;

	// Token: 0x0400001A RID: 26 RVA: 0x000018A0 File Offset: 0x00000CA0
	internal static $PTMType$QQtagVARIANT@@PA_K ?pmField@?$CVarTypeInfo@PA_K@ATL@@2QQtagVARIANT@@PA_KQ3@;

	// Token: 0x0400001B RID: 27 RVA: 0x00001888 File Offset: 0x00000C88
	internal static $PTMType$QQtagVARIANT@@PAPAUIUnknown@@ ?pmField@?$CVarTypeInfo@PAPAUIUnknown@@@ATL@@2QQtagVARIANT@@PAPAUIUnknown@@Q3@;

	// Token: 0x0400001C RID: 28 RVA: 0x00001844 File Offset: 0x00000C44
	internal static $PTMType$QQtagVARIANT@@TtagCY@@ ?pmField@?$CVarTypeInfo@TtagCY@@@ATL@@2QQtagVARIANT@@TtagCY@@Q3@;

	// Token: 0x0400001D RID: 29 RVA: 0x0000183C File Offset: 0x00000C3C
	internal static $PTMType$QQtagVARIANT@@M ?pmField@?$CVarTypeInfo@M@ATL@@2QQtagVARIANT@@MQ3@;

	// Token: 0x0400001E RID: 30 RVA: 0x0000184C File Offset: 0x00000C4C
	internal static $PTMType$QQtagVARIANT@@PAPAUIDispatch@@ ?pmField@?$CVarTypeInfo@PAPAUIDispatch@@@ATL@@2QQtagVARIANT@@PAPAUIDispatch@@Q3@;

	// Token: 0x0400001F RID: 31 RVA: 0x00001874 File Offset: 0x00000C74
	internal static $PTMType$QQtagVARIANT@@PAH ?pmField@?$CVarTypeInfo@PAH@ATL@@2QQtagVARIANT@@PAHQ3@;

	// Token: 0x04000020 RID: 32 RVA: 0x00001878 File Offset: 0x00000C78
	internal static $PTMType$QQtagVARIANT@@PAUIDispatch@@ ?pmField@?$CVarTypeInfo@PAUIDispatch@@@ATL@@2QQtagVARIANT@@PAUIDispatch@@Q3@;

	// Token: 0x04000021 RID: 33 RVA: 0x0001AA30 File Offset: 0x00019A30
	internal static int __@@_PchSym_@00@hwpgllohUuozhsrmtUfukslhgUnzmztvwUvcgvimzoUlyquivUrDIGUkxsOlyq@ufphostm;

	// Token: 0x04000022 RID: 34 RVA: 0x00001838 File Offset: 0x00000C38
	internal static $PTMType$QQtagVARIANT@@PAK ?pmField@?$CVarTypeInfo@PAK@ATL@@2QQtagVARIANT@@PAKQ3@;

	// Token: 0x04000023 RID: 35 RVA: 0x00001060 File Offset: 0x00000460
	internal unsafe static sbyte* szValToken;

	// Token: 0x04000024 RID: 36 RVA: 0x00001834 File Offset: 0x00000C34
	internal static $PTMType$QQtagVARIANT@@I ?pmField@?$CVarTypeInfo@I@ATL@@2QQtagVARIANT@@IQ3@;

	// Token: 0x04000025 RID: 37 RVA: 0x0000106C File Offset: 0x0000046C
	internal unsafe static sbyte* szDelete;

	// Token: 0x04000026 RID: 38 RVA: 0x0001AE50 File Offset: 0x00019E50
	internal static bool ?m_bInitFailed@CAtlBaseModule@ATL@@2_NA;

	// Token: 0x04000027 RID: 39 RVA: 0x00019B88 File Offset: 0x00018F88
	internal unsafe static _ATL_OBJMAP_ENTRY30* __pobjMapEntryLast;

	// Token: 0x04000028 RID: 40 RVA: 0x0000105C File Offset: 0x0000045C
	internal unsafe static sbyte* szBinaryVal;

	// Token: 0x04000029 RID: 41 RVA: 0x0000185C File Offset: 0x00000C5C
	internal static $PTMType$QQtagVARIANT@@F ?pmField@?$CVarTypeInfo@F@ATL@@2QQtagVARIANT@@FQ3@;

	// Token: 0x0400002A RID: 42 RVA: 0x00019B84 File Offset: 0x00018F84
	internal unsafe static _ATL_OBJMAP_ENTRY30* __pobjMapEntryFirst;

	// Token: 0x0400002B RID: 43 RVA: 0x00001064 File Offset: 0x00000464
	internal unsafe static sbyte* szForceRemove;

	// Token: 0x0400002C RID: 44 RVA: 0x00001050 File Offset: 0x00000450
	internal unsafe static sbyte* szStringVal;

	// Token: 0x0400002D RID: 45 RVA: 0x00001054 File Offset: 0x00000454
	internal unsafe static sbyte* multiszStringVal;

	// Token: 0x0400002E RID: 46 RVA: 0x0000187C File Offset: 0x00000C7C
	internal static $PTMType$QQtagVARIANT@@_J ?pmField@?$CVarTypeInfo@_J@ATL@@2QQtagVARIANT@@_JQ3@;

	// Token: 0x0400002F RID: 47 RVA: 0x00001068 File Offset: 0x00000468
	internal unsafe static sbyte* szNoRemove;

	// Token: 0x04000030 RID: 48 RVA: 0x00001880 File Offset: 0x00000C80
	internal static $PTMType$QQtagVARIANT@@PA_J ?pmField@?$CVarTypeInfo@PA_J@ATL@@2QQtagVARIANT@@PA_JQ3@;

	// Token: 0x04000031 RID: 49 RVA: 0x00001884 File Offset: 0x00000C84
	internal static $PTMType$QQtagVARIANT@@H ?pmField@?$CVarTypeInfo@H@ATL@@2QQtagVARIANT@@HQ3@;

	// Token: 0x04000032 RID: 50 RVA: 0x00001894 File Offset: 0x00000C94
	internal static $PTMType$QQtagVARIANT@@D ?pmField@?$CVarTypeInfo@D@ATL@@2QQtagVARIANT@@DQ3@;

	// Token: 0x04000033 RID: 51 RVA: 0x00001058 File Offset: 0x00000458
	internal unsafe static sbyte* szDwordVal;

	// Token: 0x04000034 RID: 52 RVA: 0x0001A000 File Offset: 0x00019000
	internal unsafe static void* _pAtlReleaseManagedClassFactories;

	// Token: 0x04000035 RID: 53 RVA: 0x00001898 File Offset: 0x00000C98
	internal static $PTMType$QQtagVARIANT@@PAE ?pmField@?$CVarTypeInfo@PAE@ATL@@2QQtagVARIANT@@PAEQ3@;

	// Token: 0x04000036 RID: 54 RVA: 0x0000189C File Offset: 0x00000C9C
	internal static $PTMType$QQtagVARIANT@@N ?pmField@?$CVarTypeInfo@N@ATL@@2QQtagVARIANT@@NQ3@;

	// Token: 0x04000037 RID: 55 RVA: 0x00011A0C File Offset: 0x00010E0C
	internal static $_s__RTTIBaseClassArray$_extraBytes_8 ??_R2?$CAutoComFree@PBG@RAII@@8;

	// Token: 0x04000038 RID: 56 RVA: 0x000119D8 File Offset: 0x00010DD8
	internal static $_s__RTTIBaseClassArray$_extraBytes_4 ??_R2?$CAutoCleanupBase@PBG@RAII@@8;

	// Token: 0x04000039 RID: 57 RVA: 0x000119BC File Offset: 0x00010DBC
	internal static _s__RTTIBaseClassDescriptor2 ??_R1A@?0A@EA@?$CAutoComFree@PBG@RAII@@8;

	// Token: 0x0400003A RID: 58 RVA: 0x000119F0 File Offset: 0x00010DF0
	internal static _s__RTTIBaseClassDescriptor2 ??_R1A@?0A@EA@?$CAutoCleanupBase@PBG@RAII@@8;

	// Token: 0x0400003B RID: 59 RVA: 0x000119E0 File Offset: 0x00010DE0
	internal static _s__RTTIClassHierarchyDescriptor ??_R3?$CAutoCleanupBase@PBG@RAII@@8;

	// Token: 0x0400003C RID: 60 RVA: 0x0001A4E8 File Offset: 0x000194E8
	internal static $_TypeDescriptor$_extraBytes_34 ??_R0?AV?$CAutoCleanupBase@PBG@RAII@@@8;

	// Token: 0x0400003D RID: 61 RVA: 0x00011A18 File Offset: 0x00010E18
	internal static _s__RTTIClassHierarchyDescriptor ??_R3?$CAutoComFree@PBG@RAII@@8;

	// Token: 0x0400003E RID: 62 RVA: 0x0001A4C0 File Offset: 0x000194C0
	internal static $_TypeDescriptor$_extraBytes_30 ??_R0?AV?$CAutoComFree@PBG@RAII@@@8;

	// Token: 0x0400003F RID: 63 RVA: 0x00011A28 File Offset: 0x00010E28
	internal static _s__RTTICompleteObjectLocator ??_R4?$CAutoComFree@PBG@RAII@@6B@;

	// Token: 0x04000040 RID: 64 RVA: 0x0001A008 File Offset: 0x00019008
	internal static $ArrayType$$$BY0P@Q6GXXZ ??_7?$CAutoComFree@PBG@RAII@@6B@;

	// Token: 0x04000041 RID: 65 RVA: 0x00001304 File Offset: 0x00000704
	internal static $ArrayType$$$BY0BJ@$$CBG ??_C@_1DC@CMEHKPEC@?$AAY?$AAo?$AAu?$AA?5?$AAm?$AAu?$AAs?$AAt?$AA?5?$AAc?$AAa?$AAl?$AAl?$AA?5?$AAM?$AAo?$AAv?$AAe?$AAN?$AAe?$AAx?$AAt?$AA?$CI?$AA?$CJ?$AA?$AA@;

	// Token: 0x04000042 RID: 66 RVA: 0x00001338 File Offset: 0x00000738
	internal static $ArrayType$$$BY0BG@$$CBG ??_C@_1CM@MEAAIPAK@?$AAE?$AAn?$AAu?$AAm?$AAe?$AAr?$AAa?$AAt?$AAi?$AAo?$AAn?$AA?5?$AAh?$AAa?$AAs?$AA?5?$AAe?$AAn?$AAd?$AAe?$AAd?$AA?$AA@;

	// Token: 0x04000043 RID: 67 RVA: 0x00001070 File Offset: 0x00000470
	internal static $ArrayType$$$BY0CL@$$CBG ??_C@_1FG@NPJPCCED@?$AAF?$AAa?$AAi?$AAl?$AAe?$AAd?$AA?5?$AAt?$AAo?$AA?5?$AAg?$AAe?$AAt?$AA?5?$AAc?$AAo?$AAn?$AAn?$AAe?$AAc?$AAt?$AAe?$AAd?$AA?5?$AAd?$AAe?$AAv?$AAi?$AAc?$AAe?$AA?5?$AAa@;

	// Token: 0x04000044 RID: 68 RVA: 0x000010C8 File Offset: 0x000004C8
	internal static $ArrayType$$$BY0BK@$$CBG ??_C@_1DE@GCEKIMOP@?$AAF?$AAa?$AAi?$AAl?$AAe?$AAd?$AA?5?$AAt?$AAo?$AA?5?$AAg?$AAe?$AAt?$AA?5?$AAd?$AAe?$AAv?$AAi?$AAc?$AAe?$AA?5?$AAp?$AAa?$AAt?$AAh?$AA?$AA@;

	// Token: 0x04000045 RID: 69 RVA: 0x000010FC File Offset: 0x000004FC
	internal static $ArrayType$$$BY0BI@$$CBG ??_C@_1DA@DIJJDJIP@?$AAF?$AAa?$AAi?$AAl?$AAe?$AAd?$AA?5?$AAt?$AAo?$AA?5?$AAs?$AAe?$AAn?$AAd?$AA?5?$AAr?$AAa?$AAw?$AA?5?$AAd?$AAa?$AAt?$AAa?$AA?$AA@;

	// Token: 0x04000046 RID: 70 RVA: 0x0000112C File Offset: 0x0000052C
	internal static $ArrayType$$$BY0BL@$$CBG ??_C@_1DG@KNJJMPJN@?$AAF?$AAa?$AAi?$AAl?$AAe?$AAd?$AA?5?$AAt?$AAo?$AA?5?$AAr?$AAe?$AAc?$AAe?$AAi?$AAv?$AAe?$AA?5?$AAr?$AAa?$AAw?$AA?5?$AAd?$AAa?$AAt?$AAa?$AA?$AA@;

	// Token: 0x04000047 RID: 71 RVA: 0x00001368 File Offset: 0x00000768
	internal static $ArrayType$$$BY0CB@$$CBG ??_C@_1EC@LEPNEJDA@?$AAF?$AAa?$AAi?$AAl?$AAe?$AAd?$AA?5?$AAt?$AAo?$AA?5?$AAc?$AAr?$AAe?$AAa?$AAt?$AAe?$AA?5?$AAf?$AAl?$AAa?$AAs?$AAh?$AAi?$AAn?$AAg?$AA?5?$AAd?$AAe?$AAv?$AAi?$AAc?$AAe@;

	// Token: 0x04000048 RID: 72 RVA: 0x00001168 File Offset: 0x00000568
	internal static $ArrayType$$$BY0CD@$$CBG ??_C@_1EG@CCFOHDEM@?$AAF?$AAa?$AAi?$AAl?$AAe?$AAd?$AA?5?$AAt?$AAo?$AA?5?$AAg?$AAe?$AAt?$AA?5?$AAd?$AAe?$AAv?$AAi?$AAc?$AAe?$AA?5?$AAf?$AAr?$AAi?$AAe?$AAn?$AAd?$AAl?$AAy?$AA?5?$AAn?$AAa@;

	// Token: 0x04000049 RID: 73 RVA: 0x000011B0 File Offset: 0x000005B0
	internal static $ArrayType$$$BY0BP@$$CBG ??_C@_1DO@FGPACCJK@?$AAF?$AAa?$AAi?$AAl?$AAe?$AAd?$AA?5?$AAt?$AAo?$AA?5?$AAg?$AAe?$AAt?$AA?5?$AAd?$AAe?$AAv?$AAi?$AAc?$AAe?$AA?5?$AAu?$AAn?$AAi?$AAq?$AAu?$AAe?$AA?5?$AAI?$AAD?$AA?$AA@;

	// Token: 0x0400004A RID: 74 RVA: 0x000011F0 File Offset: 0x000005F0
	internal static $ArrayType$$$BY0CD@$$CBG ??_C@_1EG@FNHIMFAE@?$AAF?$AAa?$AAi?$AAl?$AAe?$AAd?$AA?5?$AAt?$AAo?$AA?5?$AAg?$AAe?$AAt?$AA?5?$AAd?$AAe?$AAv?$AAi?$AAc?$AAe?$AA?5?$AAs?$AAe?$AAr?$AAi?$AAa?$AAl?$AA?5?$AAn?$AAu?$AAm?$AAb@;

	// Token: 0x0400004B RID: 75 RVA: 0x000013AC File Offset: 0x000007AC
	internal static $ArrayType$$$BY0BE@$$CBG ??_C@_1CI@ECIEBNIH@?$AAF?$AAa?$AAi?$AAl?$AAe?$AAd?$AA?5?$AAt?$AAo?$AA?5?$AAw?$AAr?$AAi?$AAt?$AAe?$AA?5?$AAW?$AAI?$AAM?$AA?$AA@;

	// Token: 0x0400004C RID: 76 RVA: 0x00001238 File Offset: 0x00000638
	internal static $ArrayType$$$BY0BI@$$CBG ??_C@_1DA@FFAALIKO@?$AAF?$AAa?$AAi?$AAl?$AAe?$AAd?$AA?5?$AAt?$AAo?$AA?5?$AAs?$AAk?$AAi?$AAp?$AA?5?$AAt?$AAr?$AAa?$AAn?$AAs?$AAf?$AAe?$AAr?$AA?$AA@;

	// Token: 0x0400004D RID: 77 RVA: 0x00001268 File Offset: 0x00000668
	internal static $ArrayType$$$BY0BB@$$CBG ??_C@_1CC@KCIGDDEL@?$AAF?$AAa?$AAi?$AAl?$AAe?$AAd?$AA?5?$AAt?$AAo?$AA?5?$AAr?$AAe?$AAb?$AAo?$AAo?$AAt?$AA?$AA@;

	// Token: 0x0400004E RID: 78 RVA: 0x00001290 File Offset: 0x00000690
	internal static $ArrayType$$$BY0CC@$$CBG ??_C@_1EE@BNENPODH@?$AAF?$AAa?$AAi?$AAl?$AAe?$AAd?$AA?5?$AAt?$AAo?$AA?5?$AAe?$AAn?$AAt?$AAe?$AAr?$AA?5?$AAm?$AAa?$AAs?$AAs?$AA?5?$AAs?$AAt?$AAo?$AAr?$AAa?$AAg?$AAe?$AA?5?$AAm?$AAo?$AAd@;

	// Token: 0x0400004F RID: 79 RVA: 0x000012D4 File Offset: 0x000006D4
	internal static $ArrayType$$$BY0BI@$$CBG ??_C@_1DA@MBNNDMOJ@?$AAF?$AAa?$AAi?$AAl?$AAe?$AAd?$AA?5?$AAt?$AAo?$AA?5?$AAs?$AAe?$AAt?$AA?5?$AAb?$AAo?$AAo?$AAt?$AA?5?$AAm?$AAo?$AAd?$AAe?$AA?$AA@;

	// Token: 0x04000050 RID: 80 RVA: 0x000013D4 File Offset: 0x000007D4
	internal static $ArrayType$$$BY0BJ@$$CBG ??_C@_1DC@PGNKAIIP@?$AAF?$AAa?$AAi?$AAl?$AAe?$AAd?$AA?5?$AAt?$AAo?$AA?5?$AAf?$AAl?$AAa?$AAs?$AAh?$AA?5?$AAF?$AAF?$AAU?$AA?5?$AAf?$AAi?$AAl?$AAe?$AA?$AA@;

	// Token: 0x04000051 RID: 81 RVA: 0x00011D58 File Offset: 0x00011158
	internal static $_s__RTTIBaseClassArray$_extraBytes_8 ??_R2?$CAutoDelete@PAVCGenericProgressShim@@@RAII@@8;

	// Token: 0x04000052 RID: 82 RVA: 0x00011C24 File Offset: 0x00011024
	internal static $_s__RTTIBaseClassArray$_extraBytes_4 ??_R2?$CAutoCleanupBase@PAVCGenericProgressShim@@@RAII@@8;

	// Token: 0x04000053 RID: 83 RVA: 0x00011A8C File Offset: 0x00010E8C
	internal static $_s__RTTIBaseClassArray$_extraBytes_8 ??_R2?$CAutoRelease@PAUIFlashingDevice@FlashingPlatform@@@RAII@@8;

	// Token: 0x04000054 RID: 84 RVA: 0x00011A58 File Offset: 0x00010E58
	internal static $_s__RTTIBaseClassArray$_extraBytes_4 ??_R2?$CAutoCleanupBase@PAUIFlashingDevice@FlashingPlatform@@@RAII@@8;

	// Token: 0x04000055 RID: 85 RVA: 0x00011B0C File Offset: 0x00010F0C
	internal static $_s__RTTIBaseClassArray$_extraBytes_8 ??_R2?$CAutoDeleteArray@E@RAII@@8;

	// Token: 0x04000056 RID: 86 RVA: 0x00011AD8 File Offset: 0x00010ED8
	internal static $_s__RTTIBaseClassArray$_extraBytes_4 ??_R2?$CAutoCleanupBase@PAE@RAII@@8;

	// Token: 0x04000057 RID: 87 RVA: 0x00011B58 File Offset: 0x00010F58
	internal static $_s__RTTIBaseClassArray$_extraBytes_8 ??_R2?$CAutoDeleteArray@$$CBG@RAII@@8;

	// Token: 0x04000058 RID: 88 RVA: 0x00011BD8 File Offset: 0x00010FD8
	internal static $_s__RTTIBaseClassArray$_extraBytes_8 ??_R2?$CAutoRelease@PAUIConnectedDevice@FlashingPlatform@@@RAII@@8;

	// Token: 0x04000059 RID: 89 RVA: 0x00011BA4 File Offset: 0x00010FA4
	internal static $_s__RTTIBaseClassArray$_extraBytes_4 ??_R2?$CAutoCleanupBase@PAUIConnectedDevice@FlashingPlatform@@@RAII@@8;

	// Token: 0x0400005A RID: 90 RVA: 0x00011D0C File Offset: 0x0001110C
	internal static $_s__RTTIBaseClassArray$_extraBytes_8 ??_R2CGenericProgressShim@@8;

	// Token: 0x0400005B RID: 91 RVA: 0x00011CD8 File Offset: 0x000110D8
	internal static $_s__RTTIBaseClassArray$_extraBytes_4 ??_R2IGenericProgress@FlashingPlatform@@8;

	// Token: 0x0400005C RID: 92 RVA: 0x00011C8C File Offset: 0x0001108C
	internal static $_s__RTTIBaseClassArray$_extraBytes_8 ??_R2CDeviceNotificationCallbackShim@@8;

	// Token: 0x0400005D RID: 93 RVA: 0x00011C58 File Offset: 0x00011058
	internal static $_s__RTTIBaseClassArray$_extraBytes_4 ??_R2IDeviceNotificationCallback@FlashingPlatform@@8;

	// Token: 0x0400005E RID: 94 RVA: 0x00011D3C File Offset: 0x0001113C
	internal static _s__RTTIBaseClassDescriptor2 ??_R1A@?0A@EA@?$CAutoDelete@PAVCGenericProgressShim@@@RAII@@8;

	// Token: 0x0400005F RID: 95 RVA: 0x00011C08 File Offset: 0x00011008
	internal static _s__RTTIBaseClassDescriptor2 ??_R1A@?0A@EA@?$CAutoCleanupBase@PAVCGenericProgressShim@@@RAII@@8;

	// Token: 0x04000060 RID: 96 RVA: 0x00011A3C File Offset: 0x00010E3C
	internal static _s__RTTIBaseClassDescriptor2 ??_R1A@?0A@EA@?$CAutoRelease@PAUIFlashingDevice@FlashingPlatform@@@RAII@@8;

	// Token: 0x04000061 RID: 97 RVA: 0x00011A70 File Offset: 0x00010E70
	internal static _s__RTTIBaseClassDescriptor2 ??_R1A@?0A@EA@?$CAutoCleanupBase@PAUIFlashingDevice@FlashingPlatform@@@RAII@@8;

	// Token: 0x04000062 RID: 98 RVA: 0x00011ABC File Offset: 0x00010EBC
	internal static _s__RTTIBaseClassDescriptor2 ??_R1A@?0A@EA@?$CAutoDeleteArray@E@RAII@@8;

	// Token: 0x04000063 RID: 99 RVA: 0x00011AF0 File Offset: 0x00010EF0
	internal static _s__RTTIBaseClassDescriptor2 ??_R1A@?0A@EA@?$CAutoCleanupBase@PAE@RAII@@8;

	// Token: 0x04000064 RID: 100 RVA: 0x00011B3C File Offset: 0x00010F3C
	internal static _s__RTTIBaseClassDescriptor2 ??_R1A@?0A@EA@?$CAutoDeleteArray@$$CBG@RAII@@8;

	// Token: 0x04000065 RID: 101 RVA: 0x00011B88 File Offset: 0x00010F88
	internal static _s__RTTIBaseClassDescriptor2 ??_R1A@?0A@EA@?$CAutoRelease@PAUIConnectedDevice@FlashingPlatform@@@RAII@@8;

	// Token: 0x04000066 RID: 102 RVA: 0x00011BBC File Offset: 0x00010FBC
	internal static _s__RTTIBaseClassDescriptor2 ??_R1A@?0A@EA@?$CAutoCleanupBase@PAUIConnectedDevice@FlashingPlatform@@@RAII@@8;

	// Token: 0x04000067 RID: 103 RVA: 0x00011CBC File Offset: 0x000110BC
	internal static _s__RTTIBaseClassDescriptor2 ??_R1A@?0A@EA@CGenericProgressShim@@8;

	// Token: 0x04000068 RID: 104 RVA: 0x00011CF0 File Offset: 0x000110F0
	internal static _s__RTTIBaseClassDescriptor2 ??_R1A@?0A@EA@IGenericProgress@FlashingPlatform@@8;

	// Token: 0x04000069 RID: 105 RVA: 0x00011C3C File Offset: 0x0001103C
	internal static _s__RTTIBaseClassDescriptor2 ??_R1A@?0A@EA@CDeviceNotificationCallbackShim@@8;

	// Token: 0x0400006A RID: 106 RVA: 0x00011C70 File Offset: 0x00011070
	internal static _s__RTTIBaseClassDescriptor2 ??_R1A@?0A@EA@IDeviceNotificationCallback@FlashingPlatform@@8;

	// Token: 0x0400006B RID: 107 RVA: 0x00011C2C File Offset: 0x0001102C
	internal static _s__RTTIClassHierarchyDescriptor ??_R3?$CAutoCleanupBase@PAVCGenericProgressShim@@@RAII@@8;

	// Token: 0x0400006C RID: 108 RVA: 0x0001A6D0 File Offset: 0x000196D0
	internal static $_TypeDescriptor$_extraBytes_56 ??_R0?AV?$CAutoCleanupBase@PAVCGenericProgressShim@@@RAII@@@8;

	// Token: 0x0400006D RID: 109 RVA: 0x00011D64 File Offset: 0x00011164
	internal static _s__RTTIClassHierarchyDescriptor ??_R3?$CAutoDelete@PAVCGenericProgressShim@@@RAII@@8;

	// Token: 0x0400006E RID: 110 RVA: 0x0001A7D0 File Offset: 0x000197D0
	internal static $_TypeDescriptor$_extraBytes_51 ??_R0?AV?$CAutoDelete@PAVCGenericProgressShim@@@RAII@@@8;

	// Token: 0x0400006F RID: 111 RVA: 0x00011A60 File Offset: 0x00010E60
	internal static _s__RTTIClassHierarchyDescriptor ??_R3?$CAutoCleanupBase@PAUIFlashingDevice@FlashingPlatform@@@RAII@@8;

	// Token: 0x04000070 RID: 112 RVA: 0x0001A560 File Offset: 0x00019560
	internal static $_TypeDescriptor$_extraBytes_68 ??_R0?AV?$CAutoCleanupBase@PAUIFlashingDevice@FlashingPlatform@@@RAII@@@8;

	// Token: 0x04000071 RID: 113 RVA: 0x00011A98 File Offset: 0x00010E98
	internal static _s__RTTIClassHierarchyDescriptor ??_R3?$CAutoRelease@PAUIFlashingDevice@FlashingPlatform@@@RAII@@8;

	// Token: 0x04000072 RID: 114 RVA: 0x0001A518 File Offset: 0x00019518
	internal static $_TypeDescriptor$_extraBytes_64 ??_R0?AV?$CAutoRelease@PAUIFlashingDevice@FlashingPlatform@@@RAII@@@8;

	// Token: 0x04000073 RID: 115 RVA: 0x00011AE0 File Offset: 0x00010EE0
	internal static _s__RTTIClassHierarchyDescriptor ??_R3?$CAutoCleanupBase@PAE@RAII@@8;

	// Token: 0x04000074 RID: 116 RVA: 0x0001A5D4 File Offset: 0x000195D4
	internal static $_TypeDescriptor$_extraBytes_34 ??_R0?AV?$CAutoCleanupBase@PAE@RAII@@@8;

	// Token: 0x04000075 RID: 117 RVA: 0x00011B18 File Offset: 0x00010F18
	internal static _s__RTTIClassHierarchyDescriptor ??_R3?$CAutoDeleteArray@E@RAII@@8;

	// Token: 0x04000076 RID: 118 RVA: 0x0001A5AC File Offset: 0x000195AC
	internal static $_TypeDescriptor$_extraBytes_32 ??_R0?AV?$CAutoDeleteArray@E@RAII@@@8;

	// Token: 0x04000077 RID: 119 RVA: 0x00011B64 File Offset: 0x00010F64
	internal static _s__RTTIClassHierarchyDescriptor ??_R3?$CAutoDeleteArray@$$CBG@RAII@@8;

	// Token: 0x04000078 RID: 120 RVA: 0x0001A600 File Offset: 0x00019600
	internal static $_TypeDescriptor$_extraBytes_36 ??_R0?AV?$CAutoDeleteArray@$$CBG@RAII@@@8;

	// Token: 0x04000079 RID: 121 RVA: 0x00011BAC File Offset: 0x00010FAC
	internal static _s__RTTIClassHierarchyDescriptor ??_R3?$CAutoCleanupBase@PAUIConnectedDevice@FlashingPlatform@@@RAII@@8;

	// Token: 0x0400007A RID: 122 RVA: 0x0001A680 File Offset: 0x00019680
	internal static $_TypeDescriptor$_extraBytes_69 ??_R0?AV?$CAutoCleanupBase@PAUIConnectedDevice@FlashingPlatform@@@RAII@@@8;

	// Token: 0x0400007B RID: 123 RVA: 0x00011BE4 File Offset: 0x00010FE4
	internal static _s__RTTIClassHierarchyDescriptor ??_R3?$CAutoRelease@PAUIConnectedDevice@FlashingPlatform@@@RAII@@8;

	// Token: 0x0400007C RID: 124 RVA: 0x0001A630 File Offset: 0x00019630
	internal static $_TypeDescriptor$_extraBytes_65 ??_R0?AV?$CAutoRelease@PAUIConnectedDevice@FlashingPlatform@@@RAII@@@8;

	// Token: 0x0400007D RID: 125 RVA: 0x00011CE0 File Offset: 0x000110E0
	internal static _s__RTTIClassHierarchyDescriptor ??_R3IGenericProgress@FlashingPlatform@@8;

	// Token: 0x0400007E RID: 126 RVA: 0x0001A7A0 File Offset: 0x000197A0
	internal static $_TypeDescriptor$_extraBytes_40 ??_R0?AUIGenericProgress@FlashingPlatform@@@8;

	// Token: 0x0400007F RID: 127 RVA: 0x00011D18 File Offset: 0x00011118
	internal static _s__RTTIClassHierarchyDescriptor ??_R3CGenericProgressShim@@8;

	// Token: 0x04000080 RID: 128 RVA: 0x0001A77C File Offset: 0x0001977C
	internal static $_TypeDescriptor$_extraBytes_27 ??_R0?AVCGenericProgressShim@@@8;

	// Token: 0x04000081 RID: 129 RVA: 0x00011C60 File Offset: 0x00011060
	internal static _s__RTTIClassHierarchyDescriptor ??_R3IDeviceNotificationCallback@FlashingPlatform@@8;

	// Token: 0x04000082 RID: 130 RVA: 0x0001A740 File Offset: 0x00019740
	internal static $_TypeDescriptor$_extraBytes_51 ??_R0?AUIDeviceNotificationCallback@FlashingPlatform@@@8;

	// Token: 0x04000083 RID: 131 RVA: 0x00011C98 File Offset: 0x00011098
	internal static _s__RTTIClassHierarchyDescriptor ??_R3CDeviceNotificationCallbackShim@@8;

	// Token: 0x04000084 RID: 132 RVA: 0x0001A710 File Offset: 0x00019710
	internal static $_TypeDescriptor$_extraBytes_38 ??_R0?AVCDeviceNotificationCallbackShim@@@8;

	// Token: 0x04000085 RID: 133 RVA: 0x00011D74 File Offset: 0x00011174
	internal static _s__RTTICompleteObjectLocator ??_R4?$CAutoDelete@PAVCGenericProgressShim@@@RAII@@6B@;

	// Token: 0x04000086 RID: 134 RVA: 0x00011AA8 File Offset: 0x00010EA8
	internal static _s__RTTICompleteObjectLocator ??_R4?$CAutoRelease@PAUIFlashingDevice@FlashingPlatform@@@RAII@@6B@;

	// Token: 0x04000087 RID: 135 RVA: 0x00011B28 File Offset: 0x00010F28
	internal static _s__RTTICompleteObjectLocator ??_R4?$CAutoDeleteArray@E@RAII@@6B@;

	// Token: 0x04000088 RID: 136 RVA: 0x00011B74 File Offset: 0x00010F74
	internal static _s__RTTICompleteObjectLocator ??_R4?$CAutoDeleteArray@$$CBG@RAII@@6B@;

	// Token: 0x04000089 RID: 137 RVA: 0x00011BF4 File Offset: 0x00010FF4
	internal static _s__RTTICompleteObjectLocator ??_R4?$CAutoRelease@PAUIConnectedDevice@FlashingPlatform@@@RAII@@6B@;

	// Token: 0x0400008A RID: 138 RVA: 0x00011D28 File Offset: 0x00011128
	internal static _s__RTTICompleteObjectLocator ??_R4CGenericProgressShim@@6B@;

	// Token: 0x0400008B RID: 139 RVA: 0x00011CA8 File Offset: 0x000110A8
	internal static _s__RTTICompleteObjectLocator ??_R4CDeviceNotificationCallbackShim@@6B@;

	// Token: 0x0400008C RID: 140 RVA: 0x0001A150 File Offset: 0x00019150
	internal static $ArrayType$$$BY0P@Q6GXXZ ??_7?$CAutoRelease@PAUIConnectedDevice@FlashingPlatform@@@RAII@@6B@;

	// Token: 0x0400008D RID: 141 RVA: 0x0001A104 File Offset: 0x00019104
	internal static $ArrayType$$$BY0BD@Q6GXXZ ??_7?$CAutoDeleteArray@$$CBG@RAII@@6B@;

	// Token: 0x0400008E RID: 142 RVA: 0x0001A0B8 File Offset: 0x000190B8
	internal static $ArrayType$$$BY0BD@Q6GXXZ ??_7?$CAutoDeleteArray@E@RAII@@6B@;

	// Token: 0x0400008F RID: 143 RVA: 0x0001A07C File Offset: 0x0001907C
	internal static $ArrayType$$$BY0P@Q6GXXZ ??_7?$CAutoRelease@PAUIFlashingDevice@FlashingPlatform@@@RAII@@6B@;

	// Token: 0x04000090 RID: 144 RVA: 0x0001A1AC File Offset: 0x000191AC
	internal static $ArrayType$$$BY0P@Q6GXXZ ??_7?$CAutoDelete@PAVCGenericProgressShim@@@RAII@@6B@;

	// Token: 0x04000091 RID: 145 RVA: 0x0001A1A0 File Offset: 0x000191A0
	internal static $ArrayType$$$BY01Q6GXXZ ??_7CGenericProgressShim@@6B@;

	// Token: 0x04000092 RID: 146 RVA: 0x0001A18C File Offset: 0x0001918C
	internal static $ArrayType$$$BY02Q6GXXZ ??_7CDeviceNotificationCallbackShim@@6B@;

	// Token: 0x04000093 RID: 147 RVA: 0x00001408 File Offset: 0x00000808
	internal static $ArrayType$$$BY0CI@$$CBG ??_C@_1FA@DFPBFODB@?$AAF?$AAa?$AAi?$AAl?$AAe?$AAd?$AA?5?$AAt?$AAo?$AA?5?$AAg?$AAe?$AAt?$AA?5?$AAf?$AAl?$AAa?$AAs?$AAh?$AAi?$AAn?$AAg?$AA?5?$AAp?$AAl?$AAa?$AAt?$AAf?$AAo?$AAr?$AAm?$AA?5@;

	// Token: 0x04000094 RID: 148 RVA: 0x00001458 File Offset: 0x00000858
	internal static $ArrayType$$$BY0FC@$$CBG ??_C@_1KE@NICNNOJH@?$AAM?$AAi?$AAs?$AAm?$AAa?$AAt?$AAc?$AAh?$AAe?$AAd?$AA?5?$AAv?$AAe?$AAr?$AAs?$AAi?$AAo?$AAn?$AA?5?$AAo?$AAf?$AA?5?$AAn?$AAa?$AAt?$AAi?$AAv?$AAe?$AA?5?$AAf?$AAl?$AAa@;

	// Token: 0x04000095 RID: 149 RVA: 0x00001500 File Offset: 0x00000900
	internal static $ArrayType$$$BY0CD@$$CBG ??_C@_1EG@HDIJANGN@?$AAF?$AAa?$AAi?$AAl?$AAe?$AAd?$AA?5?$AAt?$AAo?$AA?5?$AAc?$AAr?$AAe?$AAa?$AAt?$AAe?$AA?5?$AAf?$AAl?$AAa?$AAs?$AAh?$AAi?$AAn?$AAg?$AA?5?$AAp?$AAl?$AAa?$AAt?$AAf?$AAo@;

	// Token: 0x04000096 RID: 150 RVA: 0x00001548 File Offset: 0x00000948
	internal static $ArrayType$$$BY0CC@$$CBG ??_C@_1EE@GEIPDOOK@?$AAF?$AAa?$AAi?$AAl?$AAe?$AAd?$AA?5?$AAt?$AAo?$AA?5?$AAc?$AAr?$AAe?$AAa?$AAt?$AAe?$AA?5?$AAc?$AAo?$AAn?$AAn?$AAe?$AAc?$AAt?$AAe?$AAd?$AA?5?$AAd?$AAe?$AAv?$AAi?$AAc@;

	// Token: 0x04000097 RID: 151 RVA: 0x00001590 File Offset: 0x00000990
	internal static $ArrayType$$$BY0CK@$$CBG ??_C@_1FE@BCHEDBFP@?$AAF?$AAa?$AAi?$AAl?$AAe?$AAd?$AA?5?$AAt?$AAo?$AA?5?$AAg?$AAe?$AAt?$AA?5?$AAc?$AAo?$AAn?$AAn?$AAe?$AAc?$AAt?$AAe?$AAd?$AA?5?$AAd?$AAe?$AAv?$AAi?$AAc?$AAe?$AA?5?$AAc@;

	// Token: 0x04000098 RID: 152 RVA: 0x000015E8 File Offset: 0x000009E8
	internal static $ArrayType$$$BY0DA@$$CBG ??_C@_1GA@LFCCFCBB@?$AAF?$AAa?$AAi?$AAl?$AAe?$AAd?$AA?5?$AAt?$AAo?$AA?5?$AAr?$AAe?$AAg?$AAi?$AAs?$AAt?$AAe?$AAr?$AA?5?$AAd?$AAe?$AAv?$AAi?$AAc?$AAe?$AA?5?$AAn?$AAo?$AAt?$AAi?$AAf?$AAi@;

	// Token: 0x04000099 RID: 153 RVA: 0x00011ED8 File Offset: 0x000112D8
	internal static $_s__RTTIBaseClassArray$_extraBytes_8 ??_R2?$CAutoDelete@PAVCDeviceNotificationCallbackShim@@@RAII@@8;

	// Token: 0x0400009A RID: 154 RVA: 0x00011EA4 File Offset: 0x000112A4
	internal static $_s__RTTIBaseClassArray$_extraBytes_4 ??_R2?$CAutoCleanupBase@PAVCDeviceNotificationCallbackShim@@@RAII@@8;

	// Token: 0x0400009B RID: 155 RVA: 0x00011DD8 File Offset: 0x000111D8
	internal static $_s__RTTIBaseClassArray$_extraBytes_8 ??_R2?$CAutoRelease@PAUIConnectedDeviceCollection@FlashingPlatform@@@RAII@@8;

	// Token: 0x0400009C RID: 156 RVA: 0x00011DA4 File Offset: 0x000111A4
	internal static $_s__RTTIBaseClassArray$_extraBytes_4 ??_R2?$CAutoCleanupBase@PAUIConnectedDeviceCollection@FlashingPlatform@@@RAII@@8;

	// Token: 0x0400009D RID: 157 RVA: 0x00011E58 File Offset: 0x00011258
	internal static $_s__RTTIBaseClassArray$_extraBytes_8 ??_R2?$CAutoRelease@PAUIFlashingPlatform@FlashingPlatform@@@RAII@@8;

	// Token: 0x0400009E RID: 158 RVA: 0x00011E24 File Offset: 0x00011224
	internal static $_s__RTTIBaseClassArray$_extraBytes_4 ??_R2?$CAutoCleanupBase@PAUIFlashingPlatform@FlashingPlatform@@@RAII@@8;

	// Token: 0x0400009F RID: 159 RVA: 0x00011EBC File Offset: 0x000112BC
	internal static _s__RTTIBaseClassDescriptor2 ??_R1A@?0A@EA@?$CAutoDelete@PAVCDeviceNotificationCallbackShim@@@RAII@@8;

	// Token: 0x040000A0 RID: 160 RVA: 0x00011E88 File Offset: 0x00011288
	internal static _s__RTTIBaseClassDescriptor2 ??_R1A@?0A@EA@?$CAutoCleanupBase@PAVCDeviceNotificationCallbackShim@@@RAII@@8;

	// Token: 0x040000A1 RID: 161 RVA: 0x00011D88 File Offset: 0x00011188
	internal static _s__RTTIBaseClassDescriptor2 ??_R1A@?0A@EA@?$CAutoRelease@PAUIConnectedDeviceCollection@FlashingPlatform@@@RAII@@8;

	// Token: 0x040000A2 RID: 162 RVA: 0x00011DBC File Offset: 0x000111BC
	internal static _s__RTTIBaseClassDescriptor2 ??_R1A@?0A@EA@?$CAutoCleanupBase@PAUIConnectedDeviceCollection@FlashingPlatform@@@RAII@@8;

	// Token: 0x040000A3 RID: 163 RVA: 0x00011E08 File Offset: 0x00011208
	internal static _s__RTTIBaseClassDescriptor2 ??_R1A@?0A@EA@?$CAutoRelease@PAUIFlashingPlatform@FlashingPlatform@@@RAII@@8;

	// Token: 0x040000A4 RID: 164 RVA: 0x00011E3C File Offset: 0x0001123C
	internal static _s__RTTIBaseClassDescriptor2 ??_R1A@?0A@EA@?$CAutoCleanupBase@PAUIFlashingPlatform@FlashingPlatform@@@RAII@@8;

	// Token: 0x040000A5 RID: 165 RVA: 0x00011EAC File Offset: 0x000112AC
	internal static _s__RTTIClassHierarchyDescriptor ??_R3?$CAutoCleanupBase@PAVCDeviceNotificationCallbackShim@@@RAII@@8;

	// Token: 0x040000A6 RID: 166 RVA: 0x0001A960 File Offset: 0x00019960
	internal static $_TypeDescriptor$_extraBytes_67 ??_R0?AV?$CAutoCleanupBase@PAVCDeviceNotificationCallbackShim@@@RAII@@@8;

	// Token: 0x040000A7 RID: 167 RVA: 0x00011EE4 File Offset: 0x000112E4
	internal static _s__RTTIClassHierarchyDescriptor ??_R3?$CAutoDelete@PAVCDeviceNotificationCallbackShim@@@RAII@@8;

	// Token: 0x040000A8 RID: 168 RVA: 0x0001A9B0 File Offset: 0x000199B0
	internal static $_TypeDescriptor$_extraBytes_62 ??_R0?AV?$CAutoDelete@PAVCDeviceNotificationCallbackShim@@@RAII@@@8;

	// Token: 0x040000A9 RID: 169 RVA: 0x00011DAC File Offset: 0x000111AC
	internal static _s__RTTIClassHierarchyDescriptor ??_R3?$CAutoCleanupBase@PAUIConnectedDeviceCollection@FlashingPlatform@@@RAII@@8;

	// Token: 0x040000AA RID: 170 RVA: 0x0001A868 File Offset: 0x00019868
	internal static $_TypeDescriptor$_extraBytes_79 ??_R0?AV?$CAutoCleanupBase@PAUIConnectedDeviceCollection@FlashingPlatform@@@RAII@@@8;

	// Token: 0x040000AB RID: 171 RVA: 0x00011DE4 File Offset: 0x000111E4
	internal static _s__RTTIClassHierarchyDescriptor ??_R3?$CAutoRelease@PAUIConnectedDeviceCollection@FlashingPlatform@@@RAII@@8;

	// Token: 0x040000AC RID: 172 RVA: 0x0001A810 File Offset: 0x00019810
	internal static $_TypeDescriptor$_extraBytes_75 ??_R0?AV?$CAutoRelease@PAUIConnectedDeviceCollection@FlashingPlatform@@@RAII@@@8;

	// Token: 0x040000AD RID: 173 RVA: 0x00011E2C File Offset: 0x0001122C
	internal static _s__RTTIClassHierarchyDescriptor ??_R3?$CAutoCleanupBase@PAUIFlashingPlatform@FlashingPlatform@@@RAII@@8;

	// Token: 0x040000AE RID: 174 RVA: 0x0001A910 File Offset: 0x00019910
	internal static $_TypeDescriptor$_extraBytes_70 ??_R0?AV?$CAutoCleanupBase@PAUIFlashingPlatform@FlashingPlatform@@@RAII@@@8;

	// Token: 0x040000AF RID: 175 RVA: 0x00011E64 File Offset: 0x00011264
	internal static _s__RTTIClassHierarchyDescriptor ??_R3?$CAutoRelease@PAUIFlashingPlatform@FlashingPlatform@@@RAII@@8;

	// Token: 0x040000B0 RID: 176 RVA: 0x0001A8C0 File Offset: 0x000198C0
	internal static $_TypeDescriptor$_extraBytes_66 ??_R0?AV?$CAutoRelease@PAUIFlashingPlatform@FlashingPlatform@@@RAII@@@8;

	// Token: 0x040000B1 RID: 177 RVA: 0x00011EF4 File Offset: 0x000112F4
	internal static _s__RTTICompleteObjectLocator ??_R4?$CAutoDelete@PAVCDeviceNotificationCallbackShim@@@RAII@@6B@;

	// Token: 0x040000B2 RID: 178 RVA: 0x00011DF4 File Offset: 0x000111F4
	internal static _s__RTTICompleteObjectLocator ??_R4?$CAutoRelease@PAUIConnectedDeviceCollection@FlashingPlatform@@@RAII@@6B@;

	// Token: 0x040000B3 RID: 179 RVA: 0x00011E74 File Offset: 0x00011274
	internal static _s__RTTICompleteObjectLocator ??_R4?$CAutoRelease@PAUIFlashingPlatform@FlashingPlatform@@@RAII@@6B@;

	// Token: 0x040000B4 RID: 180 RVA: 0x0001A330 File Offset: 0x00019330
	internal static $ArrayType$$$BY0P@Q6GXXZ ??_7?$CAutoRelease@PAUIFlashingPlatform@FlashingPlatform@@@RAII@@6B@;

	// Token: 0x040000B5 RID: 181 RVA: 0x0001A2F4 File Offset: 0x000192F4
	internal static $ArrayType$$$BY0P@Q6GXXZ ??_7?$CAutoRelease@PAUIConnectedDeviceCollection@FlashingPlatform@@@RAII@@6B@;

	// Token: 0x040000B6 RID: 182 RVA: 0x0001A36C File Offset: 0x0001936C
	internal static $ArrayType$$$BY0P@Q6GXXZ ??_7?$CAutoDelete@PAVCDeviceNotificationCallbackShim@@@RAII@@6B@;

	// Token: 0x040000B7 RID: 183
	[FixedAddressValueType]
	internal static Progress.State ?InitializedPerProcess@CurrentDomain@<CrtImplementationDetails>@@$$Q2W4State@Progress@2@A;

	// Token: 0x040000B8 RID: 184 RVA: 0x00001030 File Offset: 0x00000430
	internal static method ?InitializedPerProcess$initializer$@CurrentDomain@<CrtImplementationDetails>@@$$Q2P6MXXZA;

	// Token: 0x040000B9 RID: 185
	[FixedAddressValueType]
	internal static int ?Uninitialized@CurrentDomain@<CrtImplementationDetails>@@$$Q2HA;

	// Token: 0x040000BA RID: 186
	[FixedAddressValueType]
	internal static Progress.State ?InitializedVtables@CurrentDomain@<CrtImplementationDetails>@@$$Q2W4State@Progress@2@A;

	// Token: 0x040000BB RID: 187 RVA: 0x0001AA3C File Offset: 0x00019A3C
	internal static bool ?Entered@DefaultDomain@<CrtImplementationDetails>@@2_NA;

	// Token: 0x040000BC RID: 188 RVA: 0x0001AA3F File Offset: 0x00019A3F
	internal static bool ?InitializedPerProcess@DefaultDomain@<CrtImplementationDetails>@@2_NA;

	// Token: 0x040000BD RID: 189 RVA: 0x0001AA38 File Offset: 0x00019A38
	internal static int ?Count@AllDomains@<CrtImplementationDetails>@@2HA;

	// Token: 0x040000BE RID: 190 RVA: 0x0001A450 File Offset: 0x00019450
	internal static TriBool.State ?hasNative@DefaultDomain@<CrtImplementationDetails>@@0W4State@TriBool@2@A;

	// Token: 0x040000BF RID: 191
	[FixedAddressValueType]
	internal static int ?Initialized@CurrentDomain@<CrtImplementationDetails>@@$$Q2HA;

	// Token: 0x040000C0 RID: 192 RVA: 0x0001A44C File Offset: 0x0001944C
	internal static TriBool.State ?hasPerProcess@DefaultDomain@<CrtImplementationDetails>@@0W4State@TriBool@2@A;

	// Token: 0x040000C1 RID: 193 RVA: 0x0001AA3E File Offset: 0x00019A3E
	internal static bool ?InitializedNativeFromCCTOR@DefaultDomain@<CrtImplementationDetails>@@2_NA;

	// Token: 0x040000C2 RID: 194
	[FixedAddressValueType]
	internal static bool ?IsDefaultDomain@CurrentDomain@<CrtImplementationDetails>@@$$Q2_NA;

	// Token: 0x040000C3 RID: 195
	[FixedAddressValueType]
	internal static Progress.State ?InitializedNative@CurrentDomain@<CrtImplementationDetails>@@$$Q2W4State@Progress@2@A;

	// Token: 0x040000C4 RID: 196 RVA: 0x0001AA3D File Offset: 0x00019A3D
	internal static bool ?InitializedNative@DefaultDomain@<CrtImplementationDetails>@@2_NA;

	// Token: 0x040000C5 RID: 197
	[FixedAddressValueType]
	internal static Progress.State ?InitializedPerAppDomain@CurrentDomain@<CrtImplementationDetails>@@$$Q2W4State@Progress@2@A;

	// Token: 0x040000C6 RID: 198 RVA: 0x00001044 File Offset: 0x00000444
	internal static $ArrayType$$$BY00Q6MPBXXZ __xc_mp_z;

	// Token: 0x040000C7 RID: 199 RVA: 0x0000104C File Offset: 0x0000044C
	internal static $ArrayType$$$BY00Q6MPBXXZ __xi_vt_z;

	// Token: 0x040000C8 RID: 200 RVA: 0x00001024 File Offset: 0x00000424
	internal static method ?IsDefaultDomain$initializer$@CurrentDomain@<CrtImplementationDetails>@@$$Q2P6MXXZA;

	// Token: 0x040000C9 RID: 201 RVA: 0x00001018 File Offset: 0x00000418
	internal static $ArrayType$$$BY00Q6MPBXXZ __xc_ma_a;

	// Token: 0x040000CA RID: 202 RVA: 0x00001038 File Offset: 0x00000438
	internal static $ArrayType$$$BY00Q6MPBXXZ __xc_ma_z;

	// Token: 0x040000CB RID: 203 RVA: 0x0000101C File Offset: 0x0000041C
	internal static method ?Initialized$initializer$@CurrentDomain@<CrtImplementationDetails>@@$$Q2P6MXXZA;

	// Token: 0x040000CC RID: 204 RVA: 0x00001034 File Offset: 0x00000434
	internal static method ?InitializedPerAppDomain$initializer$@CurrentDomain@<CrtImplementationDetails>@@$$Q2P6MXXZA;

	// Token: 0x040000CD RID: 205 RVA: 0x00001048 File Offset: 0x00000448
	internal static $ArrayType$$$BY00Q6MPBXXZ __xi_vt_a;

	// Token: 0x040000CE RID: 206 RVA: 0x0000102C File Offset: 0x0000042C
	internal static method ?InitializedNative$initializer$@CurrentDomain@<CrtImplementationDetails>@@$$Q2P6MXXZA;

	// Token: 0x040000CF RID: 207 RVA: 0x0000103C File Offset: 0x0000043C
	internal static $ArrayType$$$BY00Q6MPBXXZ __xc_mp_a;

	// Token: 0x040000D0 RID: 208 RVA: 0x00001028 File Offset: 0x00000428
	internal static method ?InitializedVtables$initializer$@CurrentDomain@<CrtImplementationDetails>@@$$Q2P6MXXZA;

	// Token: 0x040000D1 RID: 209 RVA: 0x00001020 File Offset: 0x00000420
	internal static method ?Uninitialized$initializer$@CurrentDomain@<CrtImplementationDetails>@@$$Q2P6MXXZA;

	// Token: 0x040000D2 RID: 210 RVA: 0x00001648 File Offset: 0x00000A48
	public unsafe static int** __unep@?DoNothing@DefaultDomain@<CrtImplementationDetails>@@$$FCGJPAX@Z;

	// Token: 0x040000D3 RID: 211 RVA: 0x0000164C File Offset: 0x00000A4C
	public unsafe static int** __unep@?_UninitializeDefaultDomain@LanguageSupport@<CrtImplementationDetails>@@$$FCGJPAX@Z;

	// Token: 0x040000D4 RID: 212 RVA: 0x0001AAC4 File Offset: 0x00019AC4
	internal unsafe static method* __onexitbegin_m;

	// Token: 0x040000D5 RID: 213 RVA: 0x0001AACC File Offset: 0x00019ACC
	internal static uint __exit_list_size;

	// Token: 0x040000D6 RID: 214
	[FixedAddressValueType]
	internal unsafe static method* __onexitend_app_domain;

	// Token: 0x040000D7 RID: 215
	[FixedAddressValueType]
	internal unsafe static void* ?_lock@AtExitLock@<CrtImplementationDetails>@@$$Q0PAXA;

	// Token: 0x040000D8 RID: 216
	[FixedAddressValueType]
	internal static int ?_ref_count@AtExitLock@<CrtImplementationDetails>@@$$Q0HA;

	// Token: 0x040000D9 RID: 217 RVA: 0x0001AAC8 File Offset: 0x00019AC8
	internal unsafe static method* __onexitend_m;

	// Token: 0x040000DA RID: 218
	[FixedAddressValueType]
	internal static uint __exit_list_size_app_domain;

	// Token: 0x040000DB RID: 219
	[FixedAddressValueType]
	internal unsafe static method* __onexitbegin_app_domain;

	// Token: 0x040000DC RID: 220 RVA: 0x00001650 File Offset: 0x00000A50
	internal static $ArrayType$$$BY0P@$$CBD ??_C@_0P@GHFPNOJB@bad?5allocation?$AA@;

	// Token: 0x040000DD RID: 221 RVA: 0x00011F2C File Offset: 0x0001132C
	internal static _s__RTTIClassHierarchyDescriptor ??_R3exception@@8;

	// Token: 0x040000DE RID: 222 RVA: 0x0001AB00 File Offset: 0x00019B00
	internal static bad_alloc ?nomem@?6???_U@YAPAXI@Z@4Vbad_alloc@std@@B;

	// Token: 0x040000DF RID: 223 RVA: 0x0001A9F8 File Offset: 0x000199F8
	internal static $_TypeDescriptor$_extraBytes_20 ??_R0?AVbad_alloc@std@@@8;

	// Token: 0x040000E0 RID: 224 RVA: 0x00011F58 File Offset: 0x00011358
	internal static $_s__RTTIBaseClassArray$_extraBytes_8 ??_R2bad_alloc@std@@8;

	// Token: 0x040000E1 RID: 225 RVA: 0x00019AAC File Offset: 0x00018EAC
	internal static $_s__CatchableTypeArray$_extraBytes_8 _CTA2?AVbad_alloc@std@@;

	// Token: 0x040000E2 RID: 226 RVA: 0x00011F08 File Offset: 0x00011308
	internal static _s__RTTIBaseClassDescriptor2 ??_R1A@?0A@EA@bad_alloc@std@@8;

	// Token: 0x040000E3 RID: 227 RVA: 0x00019AB8 File Offset: 0x00018EB8
	internal static _s__ThrowInfo _TI2?AVbad_alloc@std@@;

	// Token: 0x040000E4 RID: 228 RVA: 0x0001A464 File Offset: 0x00019464
	internal static $ArrayType$$$BY02Q6AXXZ ??_7bad_alloc@std@@6B@;

	// Token: 0x040000E5 RID: 229 RVA: 0x00019A90 File Offset: 0x00018E90
	internal static _s__CatchableType _CT??_R0?AVexception@@@8??0exception@@$$FQAE@ABV0@@Z12;

	// Token: 0x040000E6 RID: 230 RVA: 0x0001AA14 File Offset: 0x00019A14
	internal static $_TypeDescriptor$_extraBytes_16 ??_R0?AVexception@@@8;

	// Token: 0x040000E7 RID: 231 RVA: 0x00011F24 File Offset: 0x00011324
	internal static $_s__RTTIBaseClassArray$_extraBytes_4 ??_R2exception@@8;

	// Token: 0x040000E8 RID: 232 RVA: 0x0001A45C File Offset: 0x0001945C
	internal unsafe static sbyte* _bad_alloc_Message;

	// Token: 0x040000E9 RID: 233 RVA: 0x00011F74 File Offset: 0x00011374
	internal static _s__RTTICompleteObjectLocator ??_R4bad_alloc@std@@6B@;

	// Token: 0x040000EA RID: 234 RVA: 0x00011F64 File Offset: 0x00011364
	internal static _s__RTTIClassHierarchyDescriptor ??_R3bad_alloc@std@@8;

	// Token: 0x040000EB RID: 235 RVA: 0x00011F3C File Offset: 0x0001133C
	internal static _s__RTTIBaseClassDescriptor2 ??_R1A@?0A@EA@exception@@8;

	// Token: 0x040000EC RID: 236 RVA: 0x00019A74 File Offset: 0x00018E74
	internal static _s__CatchableType _CT??_R0?AVbad_alloc@std@@@8??0bad_alloc@std@@$$FQAE@ABV01@@Z12;

	// Token: 0x040000ED RID: 237 RVA: 0x0001AAFC File Offset: 0x00019AFC
	internal static uint ?$S1@?6???_U@YAPAXI@Z@4IA;

	// Token: 0x040000EE RID: 238 RVA: 0x00001660 File Offset: 0x00000A60
	internal static __s_GUID _GUID_cb2f6723_ab3a_11d2_9c40_00c04fa30a3e;

	// Token: 0x040000EF RID: 239 RVA: 0x00001680 File Offset: 0x00000A80
	internal static __s_GUID _GUID_cb2f6722_ab3a_11d2_9c40_00c04fa30a3e;

	// Token: 0x040000F0 RID: 240 RVA: 0x000016A0 File Offset: 0x00000AA0
	internal static __s_GUID _GUID_90f1a06c_7712_4762_86b5_7a5eba6bdb02;

	// Token: 0x040000F1 RID: 241 RVA: 0x00001690 File Offset: 0x00000A90
	internal static __s_GUID _GUID_90f1a06e_7712_4762_86b5_7a5eba6bdb02;

	// Token: 0x040000F2 RID: 242 RVA: 0x00001670 File Offset: 0x00000A70
	internal static __s_GUID _GUID_00000000_0000_0000_c000_000000000046;

	// Token: 0x040000F3 RID: 243 RVA: 0x0001AB0C File Offset: 0x00019B0C
	internal unsafe static void* ?Begin@?1??IsPointerInMsvcrtDll@?A0x47ab7201@@YAHPAXPAH@Z@4PAXA;

	// Token: 0x040000F4 RID: 244 RVA: 0x0001AB10 File Offset: 0x00019B10
	internal unsafe static void* ?End@?1??IsPointerInMsvcrtDll@?A0x47ab7201@@YAHPAXPAH@Z@4PAXA;

	// Token: 0x040000F5 RID: 245 RVA: 0x000016B0 File Offset: 0x00000AB0
	unsafe static int** __unep@?_errno@@$$J0YAPAHXZ;

	// Token: 0x040000F6 RID: 246 RVA: 0x000016B4 File Offset: 0x00000AB4
	internal static $ArrayType$$$BY0P@$$CBD ??_C@_0P@OEFGOMJK@kernelbase?4dll?$AA@;

	// Token: 0x040000F7 RID: 247 RVA: 0x000016C4 File Offset: 0x00000AC4
	internal static $ArrayType$$$BY0N@$$CBD ??_C@_0N@MDJJJHMB@kernel32?4dll?$AA@;

	// Token: 0x040000F8 RID: 248 RVA: 0x000016D4 File Offset: 0x00000AD4
	internal static $ArrayType$$$BY0O@$$CBD ??_C@_0O@NLDKAIKN@EncodePointer?$AA@;

	// Token: 0x040000F9 RID: 249 RVA: 0x000016E4 File Offset: 0x00000AE4
	internal static $ArrayType$$$BY0O@$$CBD ??_C@_0O@KBPMFGHI@DecodePointer?$AA@;

	// Token: 0x040000FA RID: 250 RVA: 0x0001AB1C File Offset: 0x00019B1C
	internal static bool ?fInitialized@?1??_decode_pointer@@YAPAXPAX@Z@4_NA;

	// Token: 0x040000FB RID: 251 RVA: 0x0001AB14 File Offset: 0x00019B14
	internal static bool ?fInitialized@?1??_encode_pointer@@YAPAXPAX@Z@4_NA;

	// Token: 0x040000FC RID: 252 RVA: 0x0001AB20 File Offset: 0x00019B20
	internal static method ?pfnDecodePointer@?1??_decode_pointer@@YAPAXPAX@Z@4P6GPAX0@ZA;

	// Token: 0x040000FD RID: 253 RVA: 0x0001AB18 File Offset: 0x00019B18
	internal static method ?pfnEncodePointer@?1??_encode_pointer@@YAPAXPAX@Z@4P6GPAX0@ZA;

	// Token: 0x040000FE RID: 254 RVA: 0x000016F4 File Offset: 0x00000AF4
	internal static $ArrayType$$$BY06$$CBD ??_C@_06OJHGLDPL@?$CInull?$CJ?$AA@;

	// Token: 0x040000FF RID: 255 RVA: 0x000016FC File Offset: 0x00000AFC
	internal static $ArrayType$$$BY06$$CB_W ??_C@_1O@CEDCILHN@?$AA?$CI?$AAn?$AAu?$AAl?$AAl?$AA?$CJ?$AA?$AA@;

	// Token: 0x04000100 RID: 256 RVA: 0x00001710 File Offset: 0x00000B10
	internal static $ArrayType$$$BY0FJ@$$CBE __lookuptable_s;

	// Token: 0x04000101 RID: 257 RVA: 0x0000176C File Offset: 0x00000B6C
	internal static $ArrayType$$$BY01Q6GXXZ ??_7type_info@@6B@;

	// Token: 0x04000102 RID: 258 RVA: 0x0001AE60 File Offset: 0x00019E60
	internal static CAtlComModule _AtlComModule;

	// Token: 0x04000103 RID: 259 RVA: 0x00001014 File Offset: 0x00000414
	internal static $ArrayType$$$BY0A@P6AHXZ __xi_z;

	// Token: 0x04000104 RID: 260 RVA: 0x0001AE8C File Offset: 0x00019E8C
	internal static volatile __enative_startup_state __native_startup_state;

	// Token: 0x04000105 RID: 261 RVA: 0x00001000 File Offset: 0x00000400
	internal static $ArrayType$$$BY0A@P6AXXZ __xc_a;

	// Token: 0x04000106 RID: 262 RVA: 0x0001AE88 File Offset: 0x00019E88
	internal unsafe static volatile void* __native_startup_lock;

	// Token: 0x04000107 RID: 263 RVA: 0x0000100C File Offset: 0x0000040C
	internal static $ArrayType$$$BY0A@P6AHXZ __xi_a;

	// Token: 0x04000108 RID: 264 RVA: 0x0001A490 File Offset: 0x00019490
	internal static volatile uint __native_dllmain_reason;

	// Token: 0x04000109 RID: 265 RVA: 0x0001A494 File Offset: 0x00019494
	internal static volatile uint __native_vcclrit_reason;

	// Token: 0x0400010A RID: 266 RVA: 0x00001008 File Offset: 0x00000408
	internal static $ArrayType$$$BY0A@P6AXXZ __xc_z;

	// Token: 0x0400010B RID: 267 RVA: 0x0000176C File Offset: 0x00000B6C
	internal static $ArrayType$$$BY01Q6AXXZ ??_7type_info@@6B@;

	// Token: 0x0400010C RID: 268 RVA: 0x00001710 File Offset: 0x00000B10
	internal static $ArrayType$$$BY0A@$$CBE __lookuptable_s;

	// Token: 0x0400010D RID: 269 RVA: 0x0001B094 File Offset: 0x00019C94
	internal unsafe static int* __imp___mb_cur_max;

	// Token: 0x0400010E RID: 270 RVA: 0x0001B084 File Offset: 0x00019C84
	internal unsafe static ioinfo* __imp___badioinfo;

	// Token: 0x0400010F RID: 271 RVA: 0x0001B088 File Offset: 0x00019C88
	internal unsafe static $ArrayType$$$BY0A@PAUioinfo@@* __imp___pioinfo;

	// Token: 0x04000110 RID: 272 RVA: 0x0001B08C File Offset: 0x00019C8C
	internal unsafe static $ArrayType$$$BY0A@U_iobuf@@* __imp__iob;
}
