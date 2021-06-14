using System;
using System.ComponentModel;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using FlashingPlatform;
using RAII;

namespace Microsoft.Windows.Flashing.Platform
{
	// Token: 0x02000043 RID: 67
	public class FlashingDevice : ConnectedDevice
	{
		// Token: 0x06000147 RID: 327 RVA: 0x00013678 File Offset: 0x00012A78
		internal unsafe FlashingDevice(IFlashingDevice* Device, [In] FlashingPlatform Platform) : base(null, Platform)
		{
			try
			{
				base.SetDevice((IConnectedDevice*)Device);
			}
			catch
			{
				base.Dispose(true);
				throw;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000148 RID: 328 RVA: 0x00012DA0 File Offset: 0x000121A0
		internal new unsafe IFlashingDevice* Native
		{
			get
			{
				return this.m_Device;
			}
		}

		// Token: 0x06000149 RID: 329 RVA: 0x0001306C File Offset: 0x0001246C
		private void ~FlashingDevice()
		{
			this.!FlashingDevice();
		}

		// Token: 0x0600014A RID: 330 RVA: 0x00012D70 File Offset: 0x00012170
		private unsafe void !FlashingDevice()
		{
			IFlashingDevice* device = this.m_Device;
			if (device != null)
			{
				IFlashingDevice* ptr = device;
				object obj = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), ptr, *(*(int*)ptr + 20));
				this.m_Device = null;
			}
		}

		// Token: 0x0600014B RID: 331 RVA: 0x000136CC File Offset: 0x00012ACC
		public unsafe string GetDeviceFriendlyName()
		{
			IFlashingDevice* device = this.m_Device;
			ushort* value;
			int num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr,System.UInt16 modopt(System.Runtime.CompilerServices.IsConst)**), device, ref value, *(*(int*)device + 24));
			if (num < 0)
			{
				CAutoDeleteArray<unsigned\u0020short\u0020const\u0020> cautoDeleteArray<unsigned_u0020short_u0020const_u0020>;
				*(ref cautoDeleteArray<unsigned_u0020short_u0020const_u0020> + 4) = <Module>.UfphNativeStrFormat((ushort*)(&<Module>.??_C@_1EG@CCFOHDEM@?$AAF?$AAa?$AAi?$AAl?$AAe?$AAd?$AA?5?$AAt?$AAo?$AA?5?$AAg?$AAe?$AAt?$AA?5?$AAd?$AAe?$AAv?$AAi?$AAc?$AAe?$AA?5?$AAf?$AAr?$AAi?$AAe?$AAn?$AAd?$AAl?$AAy?$AA?5?$AAn?$AAa@));
				cautoDeleteArray<unsigned_u0020short_u0020const_u0020> = ref <Module>.??_7?$CAutoDeleteArray@$$CBG@RAII@@6B@;
				try
				{
					IFlashingPlatform* native = this.m_Platform.Native;
					if (calli(FlashingPlatform.ILogger* modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), native, *(*(int*)native)))
					{
						IFlashingPlatform* native2 = this.m_Platform.Native;
						int num2 = calli(FlashingPlatform.ILogger* modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), native2, *(*(int*)native2));
						calli(System.Void modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr,FlashingPlatform.LogLevel,System.UInt16 modopt(System.Runtime.CompilerServices.IsConst)*), num2, 2, calli(System.UInt16 modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), ref cautoDeleteArray<unsigned_u0020short_u0020const_u0020>, *(cautoDeleteArray<unsigned_u0020short_u0020const_u0020> + 16)), *(*num2 + 4));
					}
					IntPtr ptr = (IntPtr)calli(System.UInt16 modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), ref cautoDeleteArray<unsigned_u0020short_u0020const_u0020>, *(cautoDeleteArray<unsigned_u0020short_u0020const_u0020> + 16));
					throw new Win32Exception(num, Marshal.PtrToStringUni(ptr));
				}
				catch
				{
					<Module>.___CxxCallUnwindDtor(ldftn(RAII.CAutoDeleteArray<unsigned\u0020short\u0020const\u0020>.{dtor}), (void*)(&cautoDeleteArray<unsigned_u0020short_u0020const_u0020>));
					throw;
				}
			}
			return Marshal.PtrToStringUni((IntPtr)((void*)value));
			try
			{
			}
			catch
			{
				CAutoDeleteArray<unsigned\u0020short\u0020const\u0020> cautoDeleteArray<unsigned_u0020short_u0020const_u0020>;
				<Module>.___CxxCallUnwindDtor(ldftn(RAII.CAutoDeleteArray<unsigned\u0020short\u0020const\u0020>.{dtor}), (void*)(&cautoDeleteArray<unsigned_u0020short_u0020const_u0020>));
				throw;
			}
		}

		// Token: 0x0600014C RID: 332 RVA: 0x000137E0 File Offset: 0x00012BE0
		public unsafe Guid GetDeviceUniqueID()
		{
			IFlashingDevice* device = this.m_Device;
			_GUID a;
			int num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr,_GUID*), device, ref a, *(*(int*)device + 28));
			if (num < 0)
			{
				CAutoDeleteArray<unsigned\u0020short\u0020const\u0020> cautoDeleteArray<unsigned_u0020short_u0020const_u0020>;
				*(ref cautoDeleteArray<unsigned_u0020short_u0020const_u0020> + 4) = <Module>.UfphNativeStrFormat((ushort*)(&<Module>.??_C@_1DO@FGPACCJK@?$AAF?$AAa?$AAi?$AAl?$AAe?$AAd?$AA?5?$AAt?$AAo?$AA?5?$AAg?$AAe?$AAt?$AA?5?$AAd?$AAe?$AAv?$AAi?$AAc?$AAe?$AA?5?$AAu?$AAn?$AAi?$AAq?$AAu?$AAe?$AA?5?$AAI?$AAD?$AA?$AA@));
				cautoDeleteArray<unsigned_u0020short_u0020const_u0020> = ref <Module>.??_7?$CAutoDeleteArray@$$CBG@RAII@@6B@;
				try
				{
					IFlashingPlatform* native = this.m_Platform.Native;
					if (calli(FlashingPlatform.ILogger* modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), native, *(*(int*)native)))
					{
						IFlashingPlatform* native2 = this.m_Platform.Native;
						int num2 = calli(FlashingPlatform.ILogger* modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), native2, *(*(int*)native2));
						calli(System.Void modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr,FlashingPlatform.LogLevel,System.UInt16 modopt(System.Runtime.CompilerServices.IsConst)*), num2, 2, calli(System.UInt16 modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), ref cautoDeleteArray<unsigned_u0020short_u0020const_u0020>, *(cautoDeleteArray<unsigned_u0020short_u0020const_u0020> + 16)), *(*num2 + 4));
					}
					IntPtr ptr = (IntPtr)calli(System.UInt16 modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), ref cautoDeleteArray<unsigned_u0020short_u0020const_u0020>, *(cautoDeleteArray<unsigned_u0020short_u0020const_u0020> + 16));
					throw new Win32Exception(num, Marshal.PtrToStringUni(ptr));
				}
				catch
				{
					<Module>.___CxxCallUnwindDtor(ldftn(RAII.CAutoDeleteArray<unsigned\u0020short\u0020const\u0020>.{dtor}), (void*)(&cautoDeleteArray<unsigned_u0020short_u0020const_u0020>));
					throw;
				}
			}
			Guid result = new Guid(a, *(ref a + 4), *(ref a + 6), *(ref a + 8), *(ref a + 9), *(ref a + 10), *(ref a + 11), *(ref a + 12), *(ref a + 13), *(ref a + 14), *(ref a + 15));
			return result;
			try
			{
			}
			catch
			{
				CAutoDeleteArray<unsigned\u0020short\u0020const\u0020> cautoDeleteArray<unsigned_u0020short_u0020const_u0020>;
				<Module>.___CxxCallUnwindDtor(ldftn(RAII.CAutoDeleteArray<unsigned\u0020short\u0020const\u0020>.{dtor}), (void*)(&cautoDeleteArray<unsigned_u0020short_u0020const_u0020>));
				throw;
			}
		}

		// Token: 0x0600014D RID: 333 RVA: 0x00013930 File Offset: 0x00012D30
		public unsafe Guid GetDeviceSerialNumber()
		{
			IFlashingDevice* device = this.m_Device;
			_GUID a;
			int num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr,_GUID*), device, ref a, *(*(int*)device + 32));
			if (num < 0)
			{
				CAutoDeleteArray<unsigned\u0020short\u0020const\u0020> cautoDeleteArray<unsigned_u0020short_u0020const_u0020>;
				*(ref cautoDeleteArray<unsigned_u0020short_u0020const_u0020> + 4) = <Module>.UfphNativeStrFormat((ushort*)(&<Module>.??_C@_1EG@FNHIMFAE@?$AAF?$AAa?$AAi?$AAl?$AAe?$AAd?$AA?5?$AAt?$AAo?$AA?5?$AAg?$AAe?$AAt?$AA?5?$AAd?$AAe?$AAv?$AAi?$AAc?$AAe?$AA?5?$AAs?$AAe?$AAr?$AAi?$AAa?$AAl?$AA?5?$AAn?$AAu?$AAm?$AAb@));
				cautoDeleteArray<unsigned_u0020short_u0020const_u0020> = ref <Module>.??_7?$CAutoDeleteArray@$$CBG@RAII@@6B@;
				try
				{
					IFlashingPlatform* native = this.m_Platform.Native;
					if (calli(FlashingPlatform.ILogger* modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), native, *(*(int*)native)))
					{
						IFlashingPlatform* native2 = this.m_Platform.Native;
						int num2 = calli(FlashingPlatform.ILogger* modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), native2, *(*(int*)native2));
						calli(System.Void modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr,FlashingPlatform.LogLevel,System.UInt16 modopt(System.Runtime.CompilerServices.IsConst)*), num2, 2, calli(System.UInt16 modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), ref cautoDeleteArray<unsigned_u0020short_u0020const_u0020>, *(cautoDeleteArray<unsigned_u0020short_u0020const_u0020> + 16)), *(*num2 + 4));
					}
					IntPtr ptr = (IntPtr)calli(System.UInt16 modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), ref cautoDeleteArray<unsigned_u0020short_u0020const_u0020>, *(cautoDeleteArray<unsigned_u0020short_u0020const_u0020> + 16));
					throw new Win32Exception(num, Marshal.PtrToStringUni(ptr));
				}
				catch
				{
					<Module>.___CxxCallUnwindDtor(ldftn(RAII.CAutoDeleteArray<unsigned\u0020short\u0020const\u0020>.{dtor}), (void*)(&cautoDeleteArray<unsigned_u0020short_u0020const_u0020>));
					throw;
				}
			}
			Guid result = new Guid(a, *(ref a + 4), *(ref a + 6), *(ref a + 8), *(ref a + 9), *(ref a + 10), *(ref a + 11), *(ref a + 12), *(ref a + 13), *(ref a + 14), *(ref a + 15));
			return result;
			try
			{
			}
			catch
			{
				CAutoDeleteArray<unsigned\u0020short\u0020const\u0020> cautoDeleteArray<unsigned_u0020short_u0020const_u0020>;
				<Module>.___CxxCallUnwindDtor(ldftn(RAII.CAutoDeleteArray<unsigned\u0020short\u0020const\u0020>.{dtor}), (void*)(&cautoDeleteArray<unsigned_u0020short_u0020const_u0020>));
				throw;
			}
		}

		// Token: 0x0600014E RID: 334 RVA: 0x0001415C File Offset: 0x0001355C
		public unsafe void WriteWim([In] string WimPath, GenericProgress Progress)
		{
			ushort* ptr;
			if (WimPath != null)
			{
				ptr = (ushort*)Marshal.StringToCoTaskMemUni(WimPath).ToPointer();
			}
			else
			{
				ptr = null;
			}
			CAutoComFree<unsigned\u0020short\u0020const\u0020*> cautoComFree<unsigned_u0020short_u0020const_u0020*>;
			*(ref cautoComFree<unsigned_u0020short_u0020const_u0020*> + 4) = ptr;
			cautoComFree<unsigned_u0020short_u0020const_u0020*> = ref <Module>.??_7?$CAutoComFree@PBG@RAII@@6B@;
			CAutoDelete<CGenericProgressShim\u0020*> cautoDelete<CGenericProgressShim_u0020*>;
			CAutoDeleteArray<unsigned\u0020short\u0020const\u0020> cautoDeleteArray<unsigned_u0020short_u0020const_u0020>;
			try
			{
				CGenericProgressShim* ptr2 = <Module>.@new(8U);
				CGenericProgressShim* ptr3;
				try
				{
					if (ptr2 != null)
					{
						*(int*)ptr2 = ref <Module>.??_7CGenericProgressShim@@6B@;
						*(int*)(ptr2 + 4 / sizeof(CGenericProgressShim)) = ((IntPtr)GCHandle.Alloc(Progress)).ToPointer();
						ptr3 = ptr2;
					}
					else
					{
						ptr3 = 0;
					}
				}
				catch
				{
					<Module>.delete((void*)ptr2);
					throw;
				}
				*(ref cautoDelete<CGenericProgressShim_u0020*> + 4) = ptr3;
				cautoDelete<CGenericProgressShim_u0020*> = ref <Module>.??_7?$CAutoDelete@PAVCGenericProgressShim@@@RAII@@6B@;
				try
				{
					IFlashingDevice* device = this.m_Device;
					int num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr,System.UInt16 modopt(System.Runtime.CompilerServices.IsConst)*,FlashingPlatform.IGenericProgress*), device, calli(System.UInt16 modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), ref cautoComFree<unsigned_u0020short_u0020const_u0020*>, *(cautoComFree<unsigned_u0020short_u0020const_u0020*> + 16)), calli(CGenericProgressShim* modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), ref cautoDelete<CGenericProgressShim_u0020*>, *(cautoDelete<CGenericProgressShim_u0020*> + 16)), *(*(int*)device + 36));
					if (num < 0)
					{
						*(ref cautoDeleteArray<unsigned_u0020short_u0020const_u0020> + 4) = <Module>.UfphNativeStrFormat((ushort*)(&<Module>.??_C@_1CI@ECIEBNIH@?$AAF?$AAa?$AAi?$AAl?$AAe?$AAd?$AA?5?$AAt?$AAo?$AA?5?$AAw?$AAr?$AAi?$AAt?$AAe?$AA?5?$AAW?$AAI?$AAM?$AA?$AA@));
						cautoDeleteArray<unsigned_u0020short_u0020const_u0020> = ref <Module>.??_7?$CAutoDeleteArray@$$CBG@RAII@@6B@;
						try
						{
							IFlashingPlatform* native = this.m_Platform.Native;
							if (calli(FlashingPlatform.ILogger* modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), native, *(*(int*)native)))
							{
								IFlashingPlatform* native2 = this.m_Platform.Native;
								int num2 = calli(FlashingPlatform.ILogger* modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), native2, *(*(int*)native2));
								calli(System.Void modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr,FlashingPlatform.LogLevel,System.UInt16 modopt(System.Runtime.CompilerServices.IsConst)*), num2, 2, calli(System.UInt16 modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), ref cautoDeleteArray<unsigned_u0020short_u0020const_u0020>, *(cautoDeleteArray<unsigned_u0020short_u0020const_u0020> + 16)), *(*num2 + 4));
							}
							IntPtr ptr4 = (IntPtr)calli(System.UInt16 modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), ref cautoDeleteArray<unsigned_u0020short_u0020const_u0020>, *(cautoDeleteArray<unsigned_u0020short_u0020const_u0020> + 16));
							throw new Win32Exception(num, Marshal.PtrToStringUni(ptr4));
						}
						catch
						{
							<Module>.___CxxCallUnwindDtor(ldftn(RAII.CAutoDeleteArray<unsigned\u0020short\u0020const\u0020>.{dtor}), (void*)(&cautoDeleteArray<unsigned_u0020short_u0020const_u0020>));
							throw;
						}
					}
				}
				catch
				{
					<Module>.___CxxCallUnwindDtor(ldftn(RAII.CAutoDelete<CGenericProgressShim\u0020*>.{dtor}), (void*)(&cautoDelete<CGenericProgressShim_u0020*>));
					throw;
				}
				cautoDelete<CGenericProgressShim_u0020*> = ref <Module>.??_7?$CAutoDelete@PAVCGenericProgressShim@@@RAII@@6B@;
				<Module>.RAII.CAutoDelete<CGenericProgressShim\u0020*>.Release(ref cautoDelete<CGenericProgressShim_u0020*>);
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(RAII.CAutoComFree<unsigned\u0020short\u0020const\u0020*>.{dtor}), (void*)(&cautoComFree<unsigned_u0020short_u0020const_u0020*>));
				throw;
			}
			<Module>.RAII.CAutoComFree<unsigned\u0020short\u0020const\u0020*>.{dtor}(ref cautoComFree<unsigned_u0020short_u0020const_u0020*>);
			try
			{
				try
				{
					try
					{
					}
					catch
					{
						<Module>.___CxxCallUnwindDtor(ldftn(RAII.CAutoDeleteArray<unsigned\u0020short\u0020const\u0020>.{dtor}), (void*)(&cautoDeleteArray<unsigned_u0020short_u0020const_u0020>));
						throw;
					}
				}
				catch
				{
					<Module>.___CxxCallUnwindDtor(ldftn(RAII.CAutoDelete<CGenericProgressShim\u0020*>.{dtor}), (void*)(&cautoDelete<CGenericProgressShim_u0020*>));
					throw;
				}
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(RAII.CAutoComFree<unsigned\u0020short\u0020const\u0020*>.{dtor}), (void*)(&cautoComFree<unsigned_u0020short_u0020const_u0020*>));
				throw;
			}
		}

		// Token: 0x0600014F RID: 335 RVA: 0x00013A80 File Offset: 0x00012E80
		public unsafe void SkipTransfer()
		{
			IFlashingDevice* device = this.m_Device;
			int num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), device, *(*(int*)device + 40));
			CAutoDeleteArray<unsigned\u0020short\u0020const\u0020> cautoDeleteArray<unsigned_u0020short_u0020const_u0020>;
			if (num < 0)
			{
				*(ref cautoDeleteArray<unsigned_u0020short_u0020const_u0020> + 4) = <Module>.UfphNativeStrFormat((ushort*)(&<Module>.??_C@_1DA@FFAALIKO@?$AAF?$AAa?$AAi?$AAl?$AAe?$AAd?$AA?5?$AAt?$AAo?$AA?5?$AAs?$AAk?$AAi?$AAp?$AA?5?$AAt?$AAr?$AAa?$AAn?$AAs?$AAf?$AAe?$AAr?$AA?$AA@));
				cautoDeleteArray<unsigned_u0020short_u0020const_u0020> = ref <Module>.??_7?$CAutoDeleteArray@$$CBG@RAII@@6B@;
				try
				{
					IFlashingPlatform* native = this.m_Platform.Native;
					if (calli(FlashingPlatform.ILogger* modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), native, *(*(int*)native)))
					{
						IFlashingPlatform* native2 = this.m_Platform.Native;
						int num2 = calli(FlashingPlatform.ILogger* modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), native2, *(*(int*)native2));
						calli(System.Void modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr,FlashingPlatform.LogLevel,System.UInt16 modopt(System.Runtime.CompilerServices.IsConst)*), num2, 2, calli(System.UInt16 modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), ref cautoDeleteArray<unsigned_u0020short_u0020const_u0020>, *(cautoDeleteArray<unsigned_u0020short_u0020const_u0020> + 16)), *(*num2 + 4));
					}
					IntPtr ptr = (IntPtr)calli(System.UInt16 modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), ref cautoDeleteArray<unsigned_u0020short_u0020const_u0020>, *(cautoDeleteArray<unsigned_u0020short_u0020const_u0020> + 16));
					throw new Win32Exception(num, Marshal.PtrToStringUni(ptr));
				}
				catch
				{
					<Module>.___CxxCallUnwindDtor(ldftn(RAII.CAutoDeleteArray<unsigned\u0020short\u0020const\u0020>.{dtor}), (void*)(&cautoDeleteArray<unsigned_u0020short_u0020const_u0020>));
					throw;
				}
			}
			try
			{
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(RAII.CAutoDeleteArray<unsigned\u0020short\u0020const\u0020>.{dtor}), (void*)(&cautoDeleteArray<unsigned_u0020short_u0020const_u0020>));
				throw;
			}
		}

		// Token: 0x06000150 RID: 336 RVA: 0x00013B80 File Offset: 0x00012F80
		public unsafe void Reboot()
		{
			IFlashingDevice* device = this.m_Device;
			int num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), device, *(*(int*)device + 44));
			CAutoDeleteArray<unsigned\u0020short\u0020const\u0020> cautoDeleteArray<unsigned_u0020short_u0020const_u0020>;
			if (num < 0)
			{
				*(ref cautoDeleteArray<unsigned_u0020short_u0020const_u0020> + 4) = <Module>.UfphNativeStrFormat((ushort*)(&<Module>.??_C@_1CC@KCIGDDEL@?$AAF?$AAa?$AAi?$AAl?$AAe?$AAd?$AA?5?$AAt?$AAo?$AA?5?$AAr?$AAe?$AAb?$AAo?$AAo?$AAt?$AA?$AA@));
				cautoDeleteArray<unsigned_u0020short_u0020const_u0020> = ref <Module>.??_7?$CAutoDeleteArray@$$CBG@RAII@@6B@;
				try
				{
					IFlashingPlatform* native = this.m_Platform.Native;
					if (calli(FlashingPlatform.ILogger* modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), native, *(*(int*)native)))
					{
						IFlashingPlatform* native2 = this.m_Platform.Native;
						int num2 = calli(FlashingPlatform.ILogger* modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), native2, *(*(int*)native2));
						calli(System.Void modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr,FlashingPlatform.LogLevel,System.UInt16 modopt(System.Runtime.CompilerServices.IsConst)*), num2, 2, calli(System.UInt16 modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), ref cautoDeleteArray<unsigned_u0020short_u0020const_u0020>, *(cautoDeleteArray<unsigned_u0020short_u0020const_u0020> + 16)), *(*num2 + 4));
					}
					IntPtr ptr = (IntPtr)calli(System.UInt16 modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), ref cautoDeleteArray<unsigned_u0020short_u0020const_u0020>, *(cautoDeleteArray<unsigned_u0020short_u0020const_u0020> + 16));
					throw new Win32Exception(num, Marshal.PtrToStringUni(ptr));
				}
				catch
				{
					<Module>.___CxxCallUnwindDtor(ldftn(RAII.CAutoDeleteArray<unsigned\u0020short\u0020const\u0020>.{dtor}), (void*)(&cautoDeleteArray<unsigned_u0020short_u0020const_u0020>));
					throw;
				}
			}
			try
			{
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(RAII.CAutoDeleteArray<unsigned\u0020short\u0020const\u0020>.{dtor}), (void*)(&cautoDeleteArray<unsigned_u0020short_u0020const_u0020>));
				throw;
			}
		}

		// Token: 0x06000151 RID: 337 RVA: 0x00013C80 File Offset: 0x00013080
		public unsafe void EnterMassStorageMode()
		{
			IFlashingDevice* device = this.m_Device;
			int num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), device, *(*(int*)device + 48));
			CAutoDeleteArray<unsigned\u0020short\u0020const\u0020> cautoDeleteArray<unsigned_u0020short_u0020const_u0020>;
			if (num < 0)
			{
				*(ref cautoDeleteArray<unsigned_u0020short_u0020const_u0020> + 4) = <Module>.UfphNativeStrFormat((ushort*)(&<Module>.??_C@_1EE@BNENPODH@?$AAF?$AAa?$AAi?$AAl?$AAe?$AAd?$AA?5?$AAt?$AAo?$AA?5?$AAe?$AAn?$AAt?$AAe?$AAr?$AA?5?$AAm?$AAa?$AAs?$AAs?$AA?5?$AAs?$AAt?$AAo?$AAr?$AAa?$AAg?$AAe?$AA?5?$AAm?$AAo?$AAd@));
				cautoDeleteArray<unsigned_u0020short_u0020const_u0020> = ref <Module>.??_7?$CAutoDeleteArray@$$CBG@RAII@@6B@;
				try
				{
					IFlashingPlatform* native = this.m_Platform.Native;
					if (calli(FlashingPlatform.ILogger* modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), native, *(*(int*)native)))
					{
						IFlashingPlatform* native2 = this.m_Platform.Native;
						int num2 = calli(FlashingPlatform.ILogger* modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), native2, *(*(int*)native2));
						calli(System.Void modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr,FlashingPlatform.LogLevel,System.UInt16 modopt(System.Runtime.CompilerServices.IsConst)*), num2, 2, calli(System.UInt16 modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), ref cautoDeleteArray<unsigned_u0020short_u0020const_u0020>, *(cautoDeleteArray<unsigned_u0020short_u0020const_u0020> + 16)), *(*num2 + 4));
					}
					IntPtr ptr = (IntPtr)calli(System.UInt16 modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), ref cautoDeleteArray<unsigned_u0020short_u0020const_u0020>, *(cautoDeleteArray<unsigned_u0020short_u0020const_u0020> + 16));
					throw new Win32Exception(num, Marshal.PtrToStringUni(ptr));
				}
				catch
				{
					<Module>.___CxxCallUnwindDtor(ldftn(RAII.CAutoDeleteArray<unsigned\u0020short\u0020const\u0020>.{dtor}), (void*)(&cautoDeleteArray<unsigned_u0020short_u0020const_u0020>));
					throw;
				}
			}
			try
			{
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(RAII.CAutoDeleteArray<unsigned\u0020short\u0020const\u0020>.{dtor}), (void*)(&cautoDeleteArray<unsigned_u0020short_u0020const_u0020>));
				throw;
			}
		}

		// Token: 0x06000152 RID: 338 RVA: 0x00013D80 File Offset: 0x00013180
		public unsafe void SetBootMode(uint BootMode, string ProfileName)
		{
			ushort* ptr;
			if (ProfileName != null)
			{
				ptr = (ushort*)Marshal.StringToCoTaskMemUni(ProfileName).ToPointer();
			}
			else
			{
				ptr = null;
			}
			CAutoComFree<unsigned\u0020short\u0020const\u0020*> cautoComFree<unsigned_u0020short_u0020const_u0020*>;
			*(ref cautoComFree<unsigned_u0020short_u0020const_u0020*> + 4) = ptr;
			cautoComFree<unsigned_u0020short_u0020const_u0020*> = ref <Module>.??_7?$CAutoComFree@PBG@RAII@@6B@;
			CAutoDeleteArray<unsigned\u0020short\u0020const\u0020> cautoDeleteArray<unsigned_u0020short_u0020const_u0020>;
			try
			{
				IFlashingDevice* device = this.m_Device;
				int num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr,System.UInt32,System.UInt16 modopt(System.Runtime.CompilerServices.IsConst)*), device, BootMode, calli(System.UInt16 modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), ref cautoComFree<unsigned_u0020short_u0020const_u0020*>, *(cautoComFree<unsigned_u0020short_u0020const_u0020*> + 16)), *(*(int*)device + 52));
				if (num < 0)
				{
					*(ref cautoDeleteArray<unsigned_u0020short_u0020const_u0020> + 4) = <Module>.UfphNativeStrFormat((ushort*)(&<Module>.??_C@_1DA@MBNNDMOJ@?$AAF?$AAa?$AAi?$AAl?$AAe?$AAd?$AA?5?$AAt?$AAo?$AA?5?$AAs?$AAe?$AAt?$AA?5?$AAb?$AAo?$AAo?$AAt?$AA?5?$AAm?$AAo?$AAd?$AAe?$AA?$AA@));
					cautoDeleteArray<unsigned_u0020short_u0020const_u0020> = ref <Module>.??_7?$CAutoDeleteArray@$$CBG@RAII@@6B@;
					try
					{
						IFlashingPlatform* native = this.m_Platform.Native;
						if (calli(FlashingPlatform.ILogger* modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), native, *(*(int*)native)))
						{
							IFlashingPlatform* native2 = this.m_Platform.Native;
							int num2 = calli(FlashingPlatform.ILogger* modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), native2, *(*(int*)native2));
							calli(System.Void modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr,FlashingPlatform.LogLevel,System.UInt16 modopt(System.Runtime.CompilerServices.IsConst)*), num2, 2, calli(System.UInt16 modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), ref cautoDeleteArray<unsigned_u0020short_u0020const_u0020>, *(cautoDeleteArray<unsigned_u0020short_u0020const_u0020> + 16)), *(*num2 + 4));
						}
						IntPtr ptr2 = (IntPtr)calli(System.UInt16 modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), ref cautoDeleteArray<unsigned_u0020short_u0020const_u0020>, *(cautoDeleteArray<unsigned_u0020short_u0020const_u0020> + 16));
						throw new Win32Exception(num, Marshal.PtrToStringUni(ptr2));
					}
					catch
					{
						<Module>.___CxxCallUnwindDtor(ldftn(RAII.CAutoDeleteArray<unsigned\u0020short\u0020const\u0020>.{dtor}), (void*)(&cautoDeleteArray<unsigned_u0020short_u0020const_u0020>));
						throw;
					}
				}
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(RAII.CAutoComFree<unsigned\u0020short\u0020const\u0020*>.{dtor}), (void*)(&cautoComFree<unsigned_u0020short_u0020const_u0020*>));
				throw;
			}
			<Module>.RAII.CAutoComFree<unsigned\u0020short\u0020const\u0020*>.{dtor}(ref cautoComFree<unsigned_u0020short_u0020const_u0020*>);
			try
			{
				try
				{
				}
				catch
				{
					<Module>.___CxxCallUnwindDtor(ldftn(RAII.CAutoDeleteArray<unsigned\u0020short\u0020const\u0020>.{dtor}), (void*)(&cautoDeleteArray<unsigned_u0020short_u0020const_u0020>));
					throw;
				}
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(RAII.CAutoComFree<unsigned\u0020short\u0020const\u0020*>.{dtor}), (void*)(&cautoComFree<unsigned_u0020short_u0020const_u0020*>));
				throw;
			}
		}

		// Token: 0x06000153 RID: 339 RVA: 0x000143BC File Offset: 0x000137BC
		public unsafe void FlashFFUFile([In] string FFUFilePath, FlashFlags Flags, GenericProgress Progress, HandleRef CancelEvent)
		{
			ushort* ptr;
			if (FFUFilePath != null)
			{
				ptr = (ushort*)Marshal.StringToCoTaskMemUni(FFUFilePath).ToPointer();
			}
			else
			{
				ptr = null;
			}
			CAutoComFree<unsigned\u0020short\u0020const\u0020*> cautoComFree<unsigned_u0020short_u0020const_u0020*>;
			*(ref cautoComFree<unsigned_u0020short_u0020const_u0020*> + 4) = ptr;
			cautoComFree<unsigned_u0020short_u0020const_u0020*> = ref <Module>.??_7?$CAutoComFree@PBG@RAII@@6B@;
			CAutoDelete<CGenericProgressShim\u0020*> cautoDelete<CGenericProgressShim_u0020*>;
			CAutoDeleteArray<unsigned\u0020short\u0020const\u0020> cautoDeleteArray<unsigned_u0020short_u0020const_u0020>;
			try
			{
				CGenericProgressShim* ptr2 = <Module>.@new(8U);
				CGenericProgressShim* ptr3;
				try
				{
					if (ptr2 != null)
					{
						*(int*)ptr2 = ref <Module>.??_7CGenericProgressShim@@6B@;
						*(int*)(ptr2 + 4 / sizeof(CGenericProgressShim)) = ((IntPtr)GCHandle.Alloc(Progress)).ToPointer();
						ptr3 = ptr2;
					}
					else
					{
						ptr3 = 0;
					}
				}
				catch
				{
					<Module>.delete((void*)ptr2);
					throw;
				}
				*(ref cautoDelete<CGenericProgressShim_u0020*> + 4) = ptr3;
				cautoDelete<CGenericProgressShim_u0020*> = ref <Module>.??_7?$CAutoDelete@PAVCGenericProgressShim@@@RAII@@6B@;
				try
				{
					IntPtr handle = CancelEvent.Handle;
					IFlashingDevice* device = this.m_Device;
					int num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr,System.UInt16 modopt(System.Runtime.CompilerServices.IsConst)*,System.UInt32 modopt(System.Runtime.CompilerServices.IsLong),FlashingPlatform.IGenericProgress*,System.Void*), device, calli(System.UInt16 modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), ref cautoComFree<unsigned_u0020short_u0020const_u0020*>, *(cautoComFree<unsigned_u0020short_u0020const_u0020*> + 16)), Flags, calli(CGenericProgressShim* modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), ref cautoDelete<CGenericProgressShim_u0020*>, *(cautoDelete<CGenericProgressShim_u0020*> + 16)), (void*)handle, *(*(int*)device + 56));
					if (num < 0)
					{
						*(ref cautoDeleteArray<unsigned_u0020short_u0020const_u0020> + 4) = <Module>.UfphNativeStrFormat((ushort*)(&<Module>.??_C@_1DC@PGNKAIIP@?$AAF?$AAa?$AAi?$AAl?$AAe?$AAd?$AA?5?$AAt?$AAo?$AA?5?$AAf?$AAl?$AAa?$AAs?$AAh?$AA?5?$AAF?$AAF?$AAU?$AA?5?$AAf?$AAi?$AAl?$AAe?$AA?$AA@));
						cautoDeleteArray<unsigned_u0020short_u0020const_u0020> = ref <Module>.??_7?$CAutoDeleteArray@$$CBG@RAII@@6B@;
						try
						{
							IFlashingPlatform* native = this.m_Platform.Native;
							if (calli(FlashingPlatform.ILogger* modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), native, *(*(int*)native)))
							{
								IFlashingPlatform* native2 = this.m_Platform.Native;
								int num2 = calli(FlashingPlatform.ILogger* modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), native2, *(*(int*)native2));
								calli(System.Void modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr,FlashingPlatform.LogLevel,System.UInt16 modopt(System.Runtime.CompilerServices.IsConst)*), num2, 2, calli(System.UInt16 modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), ref cautoDeleteArray<unsigned_u0020short_u0020const_u0020>, *(cautoDeleteArray<unsigned_u0020short_u0020const_u0020> + 16)), *(*num2 + 4));
							}
							IntPtr ptr4 = (IntPtr)calli(System.UInt16 modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), ref cautoDeleteArray<unsigned_u0020short_u0020const_u0020>, *(cautoDeleteArray<unsigned_u0020short_u0020const_u0020> + 16));
							throw new Win32Exception(num, Marshal.PtrToStringUni(ptr4));
						}
						catch
						{
							<Module>.___CxxCallUnwindDtor(ldftn(RAII.CAutoDeleteArray<unsigned\u0020short\u0020const\u0020>.{dtor}), (void*)(&cautoDeleteArray<unsigned_u0020short_u0020const_u0020>));
							throw;
						}
					}
				}
				catch
				{
					<Module>.___CxxCallUnwindDtor(ldftn(RAII.CAutoDelete<CGenericProgressShim\u0020*>.{dtor}), (void*)(&cautoDelete<CGenericProgressShim_u0020*>));
					throw;
				}
				cautoDelete<CGenericProgressShim_u0020*> = ref <Module>.??_7?$CAutoDelete@PAVCGenericProgressShim@@@RAII@@6B@;
				<Module>.RAII.CAutoDelete<CGenericProgressShim\u0020*>.Release(ref cautoDelete<CGenericProgressShim_u0020*>);
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(RAII.CAutoComFree<unsigned\u0020short\u0020const\u0020*>.{dtor}), (void*)(&cautoComFree<unsigned_u0020short_u0020const_u0020*>));
				throw;
			}
			<Module>.RAII.CAutoComFree<unsigned\u0020short\u0020const\u0020*>.{dtor}(ref cautoComFree<unsigned_u0020short_u0020const_u0020*>);
			try
			{
				try
				{
					try
					{
					}
					catch
					{
						<Module>.___CxxCallUnwindDtor(ldftn(RAII.CAutoDeleteArray<unsigned\u0020short\u0020const\u0020>.{dtor}), (void*)(&cautoDeleteArray<unsigned_u0020short_u0020const_u0020>));
						throw;
					}
				}
				catch
				{
					<Module>.___CxxCallUnwindDtor(ldftn(RAII.CAutoDelete<CGenericProgressShim\u0020*>.{dtor}), (void*)(&cautoDelete<CGenericProgressShim_u0020*>));
					throw;
				}
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(RAII.CAutoComFree<unsigned\u0020short\u0020const\u0020*>.{dtor}), (void*)(&cautoComFree<unsigned_u0020short_u0020const_u0020*>));
				throw;
			}
		}

		// Token: 0x06000154 RID: 340 RVA: 0x00012630 File Offset: 0x00011A30
		[HandleProcessCorruptedStateExceptions]
		protected override void Dispose([MarshalAs(UnmanagedType.U1)] bool A_0)
		{
			if (A_0)
			{
				try
				{
					this.~FlashingDevice();
					return;
				}
				finally
				{
					base.Dispose(true);
				}
			}
			try
			{
				this.!FlashingDevice();
			}
			finally
			{
				base.Dispose(false);
			}
		}

		// Token: 0x0400011F RID: 287
		private unsafe IFlashingDevice* m_Device = Device;
	}
}
