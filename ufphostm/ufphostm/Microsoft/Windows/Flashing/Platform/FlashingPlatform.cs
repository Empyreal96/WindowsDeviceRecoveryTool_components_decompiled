using System;
using System.ComponentModel;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using FlashingPlatform;
using RAII;

namespace Microsoft.Windows.Flashing.Platform
{
	// Token: 0x02000040 RID: 64
	public class FlashingPlatform : IDisposable
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600011A RID: 282 RVA: 0x00014EFC File Offset: 0x000142FC
		internal unsafe IFlashingPlatform* Native
		{
			get
			{
				return this.m_Platform;
			}
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00015044 File Offset: 0x00014444
		public unsafe FlashingPlatform(string LogFile)
		{
			uint num2;
			uint num3;
			int num = NativeFlashingPlatform.GetFlashingPlatformVersion(&num2, &num3);
			if (num < 0)
			{
				CAutoDeleteArray<unsigned\u0020short\u0020const\u0020> cautoDeleteArray<unsigned_u0020short_u0020const_u0020>;
				*(ref cautoDeleteArray<unsigned_u0020short_u0020const_u0020> + 4) = <Module>.UfphNativeStrFormat((ushort*)(&<Module>.??_C@_1FA@DFPBFODB@?$AAF?$AAa?$AAi?$AAl?$AAe?$AAd?$AA?5?$AAt?$AAo?$AA?5?$AAg?$AAe?$AAt?$AA?5?$AAf?$AAl?$AAa?$AAs?$AAh?$AAi?$AAn?$AAg?$AA?5?$AAp?$AAl?$AAa?$AAt?$AAf?$AAo?$AAr?$AAm?$AA?5@));
				cautoDeleteArray<unsigned_u0020short_u0020const_u0020> = ref <Module>.??_7?$CAutoDeleteArray@$$CBG@RAII@@6B@;
				try
				{
					IntPtr ptr = (IntPtr)calli(System.UInt16 modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), ref cautoDeleteArray<unsigned_u0020short_u0020const_u0020>, *(cautoDeleteArray<unsigned_u0020short_u0020const_u0020> + 16));
					throw new Win32Exception(num, Marshal.PtrToStringUni(ptr));
				}
				catch
				{
					<Module>.___CxxCallUnwindDtor(ldftn(RAII.CAutoDeleteArray<unsigned\u0020short\u0020const\u0020>.{dtor}), (void*)(&cautoDeleteArray<unsigned_u0020short_u0020const_u0020>));
					throw;
				}
			}
			if (num2 != FlashingPlatform.MajorVerion || num3 != FlashingPlatform.MinorVerion)
			{
				num = -2147019873;
			}
			if (num < 0)
			{
				CAutoDeleteArray<unsigned\u0020short\u0020const\u0020> cautoDeleteArray<unsigned_u0020short_u0020const_u0020>2;
				*(ref cautoDeleteArray<unsigned_u0020short_u0020const_u0020>2 + 4) = <Module>.UfphNativeStrFormat((ushort*)(&<Module>.??_C@_1KE@NICNNOJH@?$AAM?$AAi?$AAs?$AAm?$AAa?$AAt?$AAc?$AAh?$AAe?$AAd?$AA?5?$AAv?$AAe?$AAr?$AAs?$AAi?$AAo?$AAn?$AA?5?$AAo?$AAf?$AA?5?$AAn?$AAa?$AAt?$AAi?$AAv?$AAe?$AA?5?$AAf?$AAl?$AAa@), FlashingPlatform.MajorVerion, FlashingPlatform.MinorVerion, num2, num3);
				cautoDeleteArray<unsigned_u0020short_u0020const_u0020>2 = ref <Module>.??_7?$CAutoDeleteArray@$$CBG@RAII@@6B@;
				try
				{
					IntPtr ptr2 = (IntPtr)calli(System.UInt16 modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), ref cautoDeleteArray<unsigned_u0020short_u0020const_u0020>2, *(cautoDeleteArray<unsigned_u0020short_u0020const_u0020>2 + 16));
					throw new Win32Exception(num, Marshal.PtrToStringUni(ptr2));
				}
				catch
				{
					<Module>.___CxxCallUnwindDtor(ldftn(RAII.CAutoDeleteArray<unsigned\u0020short\u0020const\u0020>.{dtor}), (void*)(&cautoDeleteArray<unsigned_u0020short_u0020const_u0020>2));
					throw;
				}
			}
			ushort* ptr3;
			if (LogFile != null)
			{
				ptr3 = (ushort*)Marshal.StringToCoTaskMemUni(LogFile).ToPointer();
			}
			else
			{
				ptr3 = null;
			}
			CAutoComFree<unsigned\u0020short\u0020const\u0020*> cautoComFree<unsigned_u0020short_u0020const_u0020*>;
			*(ref cautoComFree<unsigned_u0020short_u0020const_u0020*> + 4) = ptr3;
			cautoComFree<unsigned_u0020short_u0020const_u0020*> = ref <Module>.??_7?$CAutoComFree@PBG@RAII@@6B@;
			CAutoRelease<FlashingPlatform::IFlashingPlatform\u0020*> cautoRelease<FlashingPlatform::IFlashingPlatform_u0020*>;
			CAutoDeleteArray<unsigned\u0020short\u0020const\u0020> cautoDeleteArray<unsigned_u0020short_u0020const_u0020>3;
			try
			{
				*(ref cautoRelease<FlashingPlatform::IFlashingPlatform_u0020*> + 4) = 0;
				cautoRelease<FlashingPlatform::IFlashingPlatform_u0020*> = ref <Module>.??_7?$CAutoRelease@PAUIFlashingPlatform@FlashingPlatform@@@RAII@@6B@;
				try
				{
					num = NativeFlashingPlatform.CreateFlashingPlatform(calli(System.UInt16 modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), ref cautoComFree<unsigned_u0020short_u0020const_u0020*>, *(cautoComFree<unsigned_u0020short_u0020const_u0020*> + 16)), calli(FlashingPlatform.IFlashingPlatform** modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), ref cautoRelease<FlashingPlatform::IFlashingPlatform_u0020*>, *(cautoRelease<FlashingPlatform::IFlashingPlatform_u0020*> + 4)));
					if (num < 0)
					{
						*(ref cautoDeleteArray<unsigned_u0020short_u0020const_u0020>3 + 4) = <Module>.UfphNativeStrFormat((ushort*)(&<Module>.??_C@_1EG@HDIJANGN@?$AAF?$AAa?$AAi?$AAl?$AAe?$AAd?$AA?5?$AAt?$AAo?$AA?5?$AAc?$AAr?$AAe?$AAa?$AAt?$AAe?$AA?5?$AAf?$AAl?$AAa?$AAs?$AAh?$AAi?$AAn?$AAg?$AA?5?$AAp?$AAl?$AAa?$AAt?$AAf?$AAo@));
						cautoDeleteArray<unsigned_u0020short_u0020const_u0020>3 = ref <Module>.??_7?$CAutoDeleteArray@$$CBG@RAII@@6B@;
						try
						{
							IntPtr ptr4 = (IntPtr)calli(System.UInt16 modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), ref cautoDeleteArray<unsigned_u0020short_u0020const_u0020>3, *(cautoDeleteArray<unsigned_u0020short_u0020const_u0020>3 + 16));
							throw new Win32Exception(num, Marshal.PtrToStringUni(ptr4));
						}
						catch
						{
							<Module>.___CxxCallUnwindDtor(ldftn(RAII.CAutoDeleteArray<unsigned\u0020short\u0020const\u0020>.{dtor}), (void*)(&cautoDeleteArray<unsigned_u0020short_u0020const_u0020>3));
							throw;
						}
					}
					this.m_Platform = calli(FlashingPlatform.IFlashingPlatform* modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), ref cautoRelease<FlashingPlatform::IFlashingPlatform_u0020*>, *(cautoRelease<FlashingPlatform::IFlashingPlatform_u0020*> + 40));
					this.m_DeviceNotificationCallbackShim = null;
					this.m_DeviceNotificationCallback = null;
				}
				catch
				{
					<Module>.___CxxCallUnwindDtor(ldftn(RAII.CAutoRelease<FlashingPlatform::IFlashingPlatform\u0020*>.{dtor}), (void*)(&cautoRelease<FlashingPlatform::IFlashingPlatform_u0020*>));
					throw;
				}
				cautoRelease<FlashingPlatform::IFlashingPlatform_u0020*> = ref <Module>.??_7?$CAutoRelease@PAUIFlashingPlatform@FlashingPlatform@@@RAII@@6B@;
				<Module>.RAII.CAutoRelease<FlashingPlatform::IFlashingPlatform\u0020*>.Release(ref cautoRelease<FlashingPlatform::IFlashingPlatform_u0020*>);
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
						<Module>.___CxxCallUnwindDtor(ldftn(RAII.CAutoDeleteArray<unsigned\u0020short\u0020const\u0020>.{dtor}), (void*)(&cautoDeleteArray<unsigned_u0020short_u0020const_u0020>3));
						throw;
					}
				}
				catch
				{
					<Module>.___CxxCallUnwindDtor(ldftn(RAII.CAutoRelease<FlashingPlatform::IFlashingPlatform\u0020*>.{dtor}), (void*)(&cautoRelease<FlashingPlatform::IFlashingPlatform_u0020*>));
					throw;
				}
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(RAII.CAutoComFree<unsigned\u0020short\u0020const\u0020*>.{dtor}), (void*)(&cautoComFree<unsigned_u0020short_u0020const_u0020*>));
				throw;
			}
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00015B7C File Offset: 0x00014F7C
		private void ~FlashingPlatform()
		{
			this.!FlashingPlatform();
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00015938 File Offset: 0x00014D38
		private unsafe void !FlashingPlatform()
		{
			CDeviceNotificationCallbackShim* deviceNotificationCallbackShim = this.m_DeviceNotificationCallbackShim;
			if (deviceNotificationCallbackShim != null)
			{
				CDeviceNotificationCallbackShim* ptr = deviceNotificationCallbackShim;
				<Module>.gcroot<Microsoft::Windows::Flashing::Platform::DeviceNotificationCallback\u0020^>.{dtor}(ptr + 4 / sizeof(CDeviceNotificationCallbackShim));
				<Module>.delete((void*)ptr);
				this.m_DeviceNotificationCallbackShim = null;
			}
			IFlashingPlatform* platform = this.m_Platform;
			if (platform != null)
			{
				IFlashingPlatform* ptr2 = platform;
				object obj = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), ptr2, *(*(int*)ptr2 + 28));
				this.m_Platform = null;
			}
			this.m_DeviceNotificationCallback = null;
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00014F14 File Offset: 0x00014314
		public void GetVersion(out uint Major, out uint Minor)
		{
			Major = FlashingPlatform.MajorVerion;
			Minor = FlashingPlatform.MinorVerion;
		}

		// Token: 0x0600011F RID: 287 RVA: 0x00014F34 File Offset: 0x00014334
		public Logger GetLogger()
		{
			return new Logger(this);
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00015318 File Offset: 0x00014718
		public unsafe ConnectedDevice CreateConnectedDevice([In] string DevicePath)
		{
			ushort* ptr;
			if (DevicePath != null)
			{
				ptr = (ushort*)Marshal.StringToCoTaskMemUni(DevicePath).ToPointer();
			}
			else
			{
				ptr = null;
			}
			CAutoComFree<unsigned\u0020short\u0020const\u0020*> cautoComFree<unsigned_u0020short_u0020const_u0020*>;
			*(ref cautoComFree<unsigned_u0020short_u0020const_u0020*> + 4) = ptr;
			cautoComFree<unsigned_u0020short_u0020const_u0020*> = ref <Module>.??_7?$CAutoComFree@PBG@RAII@@6B@;
			ConnectedDevice result;
			try
			{
				CAutoRelease<FlashingPlatform::IConnectedDevice\u0020*> cautoRelease<FlashingPlatform::IConnectedDevice_u0020*>;
				*(ref cautoRelease<FlashingPlatform::IConnectedDevice_u0020*> + 4) = 0;
				cautoRelease<FlashingPlatform::IConnectedDevice_u0020*> = ref <Module>.??_7?$CAutoRelease@PAUIConnectedDevice@FlashingPlatform@@@RAII@@6B@;
				try
				{
					IFlashingPlatform* platform = this.m_Platform;
					int num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr,System.UInt16 modopt(System.Runtime.CompilerServices.IsConst)*,FlashingPlatform.IConnectedDevice**), platform, calli(System.UInt16 modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), ref cautoComFree<unsigned_u0020short_u0020const_u0020*>, *(cautoComFree<unsigned_u0020short_u0020const_u0020*> + 16)), calli(FlashingPlatform.IConnectedDevice** modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), ref cautoRelease<FlashingPlatform::IConnectedDevice_u0020*>, *(cautoRelease<FlashingPlatform::IConnectedDevice_u0020*> + 4)), *(*(int*)platform + 4));
					if (num < 0)
					{
						CAutoDeleteArray<unsigned\u0020short\u0020const\u0020> cautoDeleteArray<unsigned_u0020short_u0020const_u0020>;
						*(ref cautoDeleteArray<unsigned_u0020short_u0020const_u0020> + 4) = <Module>.UfphNativeStrFormat((ushort*)(&<Module>.??_C@_1EE@GEIPDOOK@?$AAF?$AAa?$AAi?$AAl?$AAe?$AAd?$AA?5?$AAt?$AAo?$AA?5?$AAc?$AAr?$AAe?$AAa?$AAt?$AAe?$AA?5?$AAc?$AAo?$AAn?$AAn?$AAe?$AAc?$AAt?$AAe?$AAd?$AA?5?$AAd?$AAe?$AAv?$AAi?$AAc@));
						cautoDeleteArray<unsigned_u0020short_u0020const_u0020> = ref <Module>.??_7?$CAutoDeleteArray@$$CBG@RAII@@6B@;
						try
						{
							platform = this.m_Platform;
							IFlashingPlatform* ptr2 = platform;
							if (calli(FlashingPlatform.ILogger* modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), ptr2, *(*(int*)ptr2)))
							{
								platform = this.m_Platform;
								IFlashingPlatform* ptr3 = platform;
								int num2 = calli(FlashingPlatform.ILogger* modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), ptr3, *(*(int*)ptr3));
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
					result = new ConnectedDevice(calli(FlashingPlatform.IConnectedDevice* modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), ref cautoRelease<FlashingPlatform::IConnectedDevice_u0020*>, *(cautoRelease<FlashingPlatform::IConnectedDevice_u0020*> + 40)), this);
				}
				catch
				{
					<Module>.___CxxCallUnwindDtor(ldftn(RAII.CAutoRelease<FlashingPlatform::IConnectedDevice\u0020*>.{dtor}), (void*)(&cautoRelease<FlashingPlatform::IConnectedDevice_u0020*>));
					throw;
				}
				cautoRelease<FlashingPlatform::IConnectedDevice_u0020*> = ref <Module>.??_7?$CAutoRelease@PAUIConnectedDevice@FlashingPlatform@@@RAII@@6B@;
				<Module>.RAII.CAutoRelease<FlashingPlatform::IConnectedDevice\u0020*>.Release(ref cautoRelease<FlashingPlatform::IConnectedDevice_u0020*>);
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(RAII.CAutoComFree<unsigned\u0020short\u0020const\u0020*>.{dtor}), (void*)(&cautoComFree<unsigned_u0020short_u0020const_u0020*>));
				throw;
			}
			<Module>.RAII.CAutoComFree<unsigned\u0020short\u0020const\u0020*>.{dtor}(ref cautoComFree<unsigned_u0020short_u0020const_u0020*>);
			return result;
			try
			{
				try
				{
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
				catch
				{
					CAutoRelease<FlashingPlatform::IConnectedDevice\u0020*> cautoRelease<FlashingPlatform::IConnectedDevice_u0020*>;
					<Module>.___CxxCallUnwindDtor(ldftn(RAII.CAutoRelease<FlashingPlatform::IConnectedDevice\u0020*>.{dtor}), (void*)(&cautoRelease<FlashingPlatform::IConnectedDevice_u0020*>));
					throw;
				}
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(RAII.CAutoComFree<unsigned\u0020short\u0020const\u0020*>.{dtor}), (void*)(&cautoComFree<unsigned_u0020short_u0020const_u0020*>));
				throw;
			}
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00015534 File Offset: 0x00014934
		public unsafe FlashingDevice CreateFlashingDevice([In] string DevicePath)
		{
			ushort* ptr;
			if (DevicePath != null)
			{
				ptr = (ushort*)Marshal.StringToCoTaskMemUni(DevicePath).ToPointer();
			}
			else
			{
				ptr = null;
			}
			CAutoComFree<unsigned\u0020short\u0020const\u0020*> cautoComFree<unsigned_u0020short_u0020const_u0020*>;
			*(ref cautoComFree<unsigned_u0020short_u0020const_u0020*> + 4) = ptr;
			cautoComFree<unsigned_u0020short_u0020const_u0020*> = ref <Module>.??_7?$CAutoComFree@PBG@RAII@@6B@;
			FlashingDevice result;
			try
			{
				CAutoRelease<FlashingPlatform::IFlashingDevice\u0020*> cautoRelease<FlashingPlatform::IFlashingDevice_u0020*>;
				*(ref cautoRelease<FlashingPlatform::IFlashingDevice_u0020*> + 4) = 0;
				cautoRelease<FlashingPlatform::IFlashingDevice_u0020*> = ref <Module>.??_7?$CAutoRelease@PAUIFlashingDevice@FlashingPlatform@@@RAII@@6B@;
				try
				{
					IFlashingPlatform* platform = this.m_Platform;
					int num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr,System.UInt16 modopt(System.Runtime.CompilerServices.IsConst)*,FlashingPlatform.IFlashingDevice**), platform, calli(System.UInt16 modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), ref cautoComFree<unsigned_u0020short_u0020const_u0020*>, *(cautoComFree<unsigned_u0020short_u0020const_u0020*> + 16)), calli(FlashingPlatform.IFlashingDevice** modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), ref cautoRelease<FlashingPlatform::IFlashingDevice_u0020*>, *(cautoRelease<FlashingPlatform::IFlashingDevice_u0020*> + 4)), *(*(int*)platform + 8));
					if (num < 0)
					{
						CAutoDeleteArray<unsigned\u0020short\u0020const\u0020> cautoDeleteArray<unsigned_u0020short_u0020const_u0020>;
						*(ref cautoDeleteArray<unsigned_u0020short_u0020const_u0020> + 4) = <Module>.UfphNativeStrFormat((ushort*)(&<Module>.??_C@_1EC@LEPNEJDA@?$AAF?$AAa?$AAi?$AAl?$AAe?$AAd?$AA?5?$AAt?$AAo?$AA?5?$AAc?$AAr?$AAe?$AAa?$AAt?$AAe?$AA?5?$AAf?$AAl?$AAa?$AAs?$AAh?$AAi?$AAn?$AAg?$AA?5?$AAd?$AAe?$AAv?$AAi?$AAc?$AAe@));
						cautoDeleteArray<unsigned_u0020short_u0020const_u0020> = ref <Module>.??_7?$CAutoDeleteArray@$$CBG@RAII@@6B@;
						try
						{
							platform = this.m_Platform;
							IFlashingPlatform* ptr2 = platform;
							if (calli(FlashingPlatform.ILogger* modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), ptr2, *(*(int*)ptr2)))
							{
								platform = this.m_Platform;
								IFlashingPlatform* ptr3 = platform;
								int num2 = calli(FlashingPlatform.ILogger* modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), ptr3, *(*(int*)ptr3));
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
					result = new FlashingDevice(calli(FlashingPlatform.IFlashingDevice* modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), ref cautoRelease<FlashingPlatform::IFlashingDevice_u0020*>, *(cautoRelease<FlashingPlatform::IFlashingDevice_u0020*> + 40)), this);
				}
				catch
				{
					<Module>.___CxxCallUnwindDtor(ldftn(RAII.CAutoRelease<FlashingPlatform::IFlashingDevice\u0020*>.{dtor}), (void*)(&cautoRelease<FlashingPlatform::IFlashingDevice_u0020*>));
					throw;
				}
				cautoRelease<FlashingPlatform::IFlashingDevice_u0020*> = ref <Module>.??_7?$CAutoRelease@PAUIFlashingDevice@FlashingPlatform@@@RAII@@6B@;
				<Module>.RAII.CAutoRelease<FlashingPlatform::IFlashingDevice\u0020*>.Release(ref cautoRelease<FlashingPlatform::IFlashingDevice_u0020*>);
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(RAII.CAutoComFree<unsigned\u0020short\u0020const\u0020*>.{dtor}), (void*)(&cautoComFree<unsigned_u0020short_u0020const_u0020*>));
				throw;
			}
			<Module>.RAII.CAutoComFree<unsigned\u0020short\u0020const\u0020*>.{dtor}(ref cautoComFree<unsigned_u0020short_u0020const_u0020*>);
			return result;
			try
			{
				try
				{
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
				catch
				{
					CAutoRelease<FlashingPlatform::IFlashingDevice\u0020*> cautoRelease<FlashingPlatform::IFlashingDevice_u0020*>;
					<Module>.___CxxCallUnwindDtor(ldftn(RAII.CAutoRelease<FlashingPlatform::IFlashingDevice\u0020*>.{dtor}), (void*)(&cautoRelease<FlashingPlatform::IFlashingDevice_u0020*>));
					throw;
				}
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(RAII.CAutoComFree<unsigned\u0020short\u0020const\u0020*>.{dtor}), (void*)(&cautoComFree<unsigned_u0020short_u0020const_u0020*>));
				throw;
			}
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00015750 File Offset: 0x00014B50
		public unsafe ConnectedDeviceCollection GetConnectedDeviceCollection()
		{
			CAutoRelease<FlashingPlatform::IConnectedDeviceCollection\u0020*> cautoRelease<FlashingPlatform::IConnectedDeviceCollection_u0020*>;
			*(ref cautoRelease<FlashingPlatform::IConnectedDeviceCollection_u0020*> + 4) = 0;
			cautoRelease<FlashingPlatform::IConnectedDeviceCollection_u0020*> = ref <Module>.??_7?$CAutoRelease@PAUIConnectedDeviceCollection@FlashingPlatform@@@RAII@@6B@;
			ConnectedDeviceCollection result;
			try
			{
				IFlashingPlatform* platform = this.m_Platform;
				int num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr,FlashingPlatform.IConnectedDeviceCollection**), platform, calli(FlashingPlatform.IConnectedDeviceCollection** modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), ref cautoRelease<FlashingPlatform::IConnectedDeviceCollection_u0020*>, *(cautoRelease<FlashingPlatform::IConnectedDeviceCollection_u0020*> + 4)), *(*(int*)platform + 12));
				if (num < 0)
				{
					CAutoDeleteArray<unsigned\u0020short\u0020const\u0020> cautoDeleteArray<unsigned_u0020short_u0020const_u0020>;
					*(ref cautoDeleteArray<unsigned_u0020short_u0020const_u0020> + 4) = <Module>.UfphNativeStrFormat((ushort*)(&<Module>.??_C@_1FE@BCHEDBFP@?$AAF?$AAa?$AAi?$AAl?$AAe?$AAd?$AA?5?$AAt?$AAo?$AA?5?$AAg?$AAe?$AAt?$AA?5?$AAc?$AAo?$AAn?$AAn?$AAe?$AAc?$AAt?$AAe?$AAd?$AA?5?$AAd?$AAe?$AAv?$AAi?$AAc?$AAe?$AA?5?$AAc@));
					cautoDeleteArray<unsigned_u0020short_u0020const_u0020> = ref <Module>.??_7?$CAutoDeleteArray@$$CBG@RAII@@6B@;
					try
					{
						platform = this.m_Platform;
						IFlashingPlatform* ptr = platform;
						if (calli(FlashingPlatform.ILogger* modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), ptr, *(*(int*)ptr)))
						{
							platform = this.m_Platform;
							IFlashingPlatform* ptr2 = platform;
							int num2 = calli(FlashingPlatform.ILogger* modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), ptr2, *(*(int*)ptr2));
							calli(System.Void modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr,FlashingPlatform.LogLevel,System.UInt16 modopt(System.Runtime.CompilerServices.IsConst)*), num2, 2, calli(System.UInt16 modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), ref cautoDeleteArray<unsigned_u0020short_u0020const_u0020>, *(cautoDeleteArray<unsigned_u0020short_u0020const_u0020> + 16)), *(*num2 + 4));
						}
						IntPtr ptr3 = (IntPtr)calli(System.UInt16 modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), ref cautoDeleteArray<unsigned_u0020short_u0020const_u0020>, *(cautoDeleteArray<unsigned_u0020short_u0020const_u0020> + 16));
						throw new Win32Exception(num, Marshal.PtrToStringUni(ptr3));
					}
					catch
					{
						<Module>.___CxxCallUnwindDtor(ldftn(RAII.CAutoDeleteArray<unsigned\u0020short\u0020const\u0020>.{dtor}), (void*)(&cautoDeleteArray<unsigned_u0020short_u0020const_u0020>));
						throw;
					}
				}
				result = new ConnectedDeviceCollection(calli(FlashingPlatform.IConnectedDeviceCollection* modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), ref cautoRelease<FlashingPlatform::IConnectedDeviceCollection_u0020*>, *(cautoRelease<FlashingPlatform::IConnectedDeviceCollection_u0020*> + 40)), this);
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(RAII.CAutoRelease<FlashingPlatform::IConnectedDeviceCollection\u0020*>.{dtor}), (void*)(&cautoRelease<FlashingPlatform::IConnectedDeviceCollection_u0020*>));
				throw;
			}
			cautoRelease<FlashingPlatform::IConnectedDeviceCollection_u0020*> = ref <Module>.??_7?$CAutoRelease@PAUIConnectedDeviceCollection@FlashingPlatform@@@RAII@@6B@;
			<Module>.RAII.CAutoRelease<FlashingPlatform::IConnectedDeviceCollection\u0020*>.Release(ref cautoRelease<FlashingPlatform::IConnectedDeviceCollection_u0020*>);
			return result;
			try
			{
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
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(RAII.CAutoRelease<FlashingPlatform::IConnectedDeviceCollection\u0020*>.{dtor}), (void*)(&cautoRelease<FlashingPlatform::IConnectedDeviceCollection_u0020*>));
				throw;
			}
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00015994 File Offset: 0x00014D94
		public unsafe void RegisterDeviceNotificationCallback(DeviceNotificationCallback Callback, ref DeviceNotificationCallback OldCallback)
		{
			CDeviceNotificationCallbackShim* ptr3;
			if (Callback != null)
			{
				CDeviceNotificationCallbackShim* ptr = <Module>.@new(8U);
				CDeviceNotificationCallbackShim* ptr2;
				try
				{
					if (ptr != null)
					{
						ptr2 = <Module>.CDeviceNotificationCallbackShim.{ctor}(ptr, Callback);
					}
					else
					{
						ptr2 = 0;
					}
				}
				catch
				{
					<Module>.delete((void*)ptr);
					throw;
				}
				ptr3 = ptr2;
			}
			else
			{
				ptr3 = null;
			}
			CAutoDelete<CDeviceNotificationCallbackShim\u0020*> cautoDelete<CDeviceNotificationCallbackShim_u0020*>;
			*(ref cautoDelete<CDeviceNotificationCallbackShim_u0020*> + 4) = ptr3;
			cautoDelete<CDeviceNotificationCallbackShim_u0020*> = ref <Module>.??_7?$CAutoDelete@PAVCDeviceNotificationCallbackShim@@@RAII@@6B@;
			CAutoDeleteArray<unsigned\u0020short\u0020const\u0020> cautoDeleteArray<unsigned_u0020short_u0020const_u0020>;
			try
			{
				IFlashingPlatform* platform = this.m_Platform;
				int num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr,FlashingPlatform.IDeviceNotificationCallback*,FlashingPlatform.IDeviceNotificationCallback**), platform, calli(CDeviceNotificationCallbackShim* modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), ref cautoDelete<CDeviceNotificationCallbackShim_u0020*>, *(cautoDelete<CDeviceNotificationCallbackShim_u0020*> + 16)), 0, *(*(int*)platform + 16));
				if (num < 0)
				{
					*(ref cautoDeleteArray<unsigned_u0020short_u0020const_u0020> + 4) = <Module>.UfphNativeStrFormat((ushort*)(&<Module>.??_C@_1GA@LFCCFCBB@?$AAF?$AAa?$AAi?$AAl?$AAe?$AAd?$AA?5?$AAt?$AAo?$AA?5?$AAr?$AAe?$AAg?$AAi?$AAs?$AAt?$AAe?$AAr?$AA?5?$AAd?$AAe?$AAv?$AAi?$AAc?$AAe?$AA?5?$AAn?$AAo?$AAt?$AAi?$AAf?$AAi@));
					cautoDeleteArray<unsigned_u0020short_u0020const_u0020> = ref <Module>.??_7?$CAutoDeleteArray@$$CBG@RAII@@6B@;
					try
					{
						platform = this.m_Platform;
						IFlashingPlatform* ptr4 = platform;
						if (calli(FlashingPlatform.ILogger* modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), ptr4, *(*(int*)ptr4)))
						{
							platform = this.m_Platform;
							IFlashingPlatform* ptr5 = platform;
							int num2 = calli(FlashingPlatform.ILogger* modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), ptr5, *(*(int*)ptr5));
							calli(System.Void modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr,FlashingPlatform.LogLevel,System.UInt16 modopt(System.Runtime.CompilerServices.IsConst)*), num2, 2, calli(System.UInt16 modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), ref cautoDeleteArray<unsigned_u0020short_u0020const_u0020>, *(cautoDeleteArray<unsigned_u0020short_u0020const_u0020> + 16)), *(*num2 + 4));
						}
						IntPtr ptr6 = (IntPtr)calli(System.UInt16 modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), ref cautoDeleteArray<unsigned_u0020short_u0020const_u0020>, *(cautoDeleteArray<unsigned_u0020short_u0020const_u0020> + 16));
						throw new Win32Exception(num, Marshal.PtrToStringUni(ptr6));
					}
					catch
					{
						<Module>.___CxxCallUnwindDtor(ldftn(RAII.CAutoDeleteArray<unsigned\u0020short\u0020const\u0020>.{dtor}), (void*)(&cautoDeleteArray<unsigned_u0020short_u0020const_u0020>));
						throw;
					}
				}
				if (OldCallback != null)
				{
					OldCallback = this.m_DeviceNotificationCallback;
				}
				this.m_DeviceNotificationCallback = Callback;
				this.m_DeviceNotificationCallbackShim = calli(CDeviceNotificationCallbackShim* modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), ref cautoDelete<CDeviceNotificationCallbackShim_u0020*>, *(cautoDelete<CDeviceNotificationCallbackShim_u0020*> + 40));
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(RAII.CAutoDelete<CDeviceNotificationCallbackShim\u0020*>.{dtor}), (void*)(&cautoDelete<CDeviceNotificationCallbackShim_u0020*>));
				throw;
			}
			cautoDelete<CDeviceNotificationCallbackShim_u0020*> = ref <Module>.??_7?$CAutoDelete@PAVCDeviceNotificationCallbackShim@@@RAII@@6B@;
			<Module>.RAII.CAutoDelete<CDeviceNotificationCallbackShim\u0020*>.Release(ref cautoDelete<CDeviceNotificationCallbackShim_u0020*>);
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
				<Module>.___CxxCallUnwindDtor(ldftn(RAII.CAutoDelete<CDeviceNotificationCallbackShim\u0020*>.{dtor}), (void*)(&cautoDelete<CDeviceNotificationCallbackShim_u0020*>));
				throw;
			}
		}

		// Token: 0x06000124 RID: 292 RVA: 0x00014F4C File Offset: 0x0001434C
		public unsafe string GetErrorMessage(int HResult)
		{
			IFlashingPlatform* platform = this.m_Platform;
			ushort* ptr = calli(System.UInt16 modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr,System.Int32 modopt(System.Runtime.CompilerServices.IsLong)), platform, HResult, *(*(int*)platform + 20));
			return (ptr == null) ? null : Marshal.PtrToStringUni((IntPtr)((void*)ptr));
		}

		// Token: 0x06000125 RID: 293 RVA: 0x00014F84 File Offset: 0x00014384
		public unsafe int Thor2ResultFromHResult(int HResult)
		{
			IFlashingPlatform* platform = this.m_Platform;
			return calli(System.Int32 modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr,System.Int32 modopt(System.Runtime.CompilerServices.IsLong)), platform, HResult, *(*(int*)platform + 24));
		}

		// Token: 0x06000126 RID: 294 RVA: 0x000124D8 File Offset: 0x000118D8
		[HandleProcessCorruptedStateExceptions]
		protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool A_0)
		{
			if (A_0)
			{
				this.~FlashingPlatform();
			}
			else
			{
				try
				{
					this.!FlashingPlatform();
				}
				finally
				{
					base.Finalize();
				}
			}
		}

		// Token: 0x06000127 RID: 295 RVA: 0x000127D8 File Offset: 0x00011BD8
		public sealed void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00012524 File Offset: 0x00011924
		protected override void Finalize()
		{
			this.Dispose(false);
		}

		// Token: 0x04000114 RID: 276
		private unsafe IFlashingPlatform* m_Platform;

		// Token: 0x04000115 RID: 277
		private DeviceNotificationCallback m_DeviceNotificationCallback;

		// Token: 0x04000116 RID: 278
		private unsafe CDeviceNotificationCallbackShim* m_DeviceNotificationCallbackShim;

		// Token: 0x04000117 RID: 279
		public static uint MajorVerion = 0U;

		// Token: 0x04000118 RID: 280
		public static uint MinorVerion = 2U;
	}
}
