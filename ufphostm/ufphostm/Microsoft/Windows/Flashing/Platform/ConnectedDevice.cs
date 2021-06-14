using System;
using System.ComponentModel;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using FlashingPlatform;
using RAII;

namespace Microsoft.Windows.Flashing.Platform
{
	// Token: 0x02000042 RID: 66
	public class ConnectedDevice : IDisposable
	{
		// Token: 0x0600013A RID: 314 RVA: 0x00012CA0 File Offset: 0x000120A0
		internal unsafe ConnectedDevice(IConnectedDevice* Device, [In] FlashingPlatform Platform)
		{
			bool ownDevice = Device != null;
			this.m_OwnDevice = ownDevice;
			this.m_Platform = Platform;
			base..ctor();
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600013B RID: 315 RVA: 0x00012D2C File Offset: 0x0001212C
		internal unsafe IConnectedDevice* Native
		{
			get
			{
				return this.m_Device;
			}
		}

		// Token: 0x0600013C RID: 316 RVA: 0x00012CD8 File Offset: 0x000120D8
		internal unsafe void SetDevice(IConnectedDevice* Device)
		{
			this.m_Device = Device;
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00013054 File Offset: 0x00012454
		private void ~ConnectedDevice()
		{
			this.!ConnectedDevice();
		}

		// Token: 0x0600013E RID: 318 RVA: 0x00012CF4 File Offset: 0x000120F4
		private unsafe void !ConnectedDevice()
		{
			IConnectedDevice* device = this.m_Device;
			if (device != null)
			{
				if (this.m_OwnDevice)
				{
					IConnectedDevice* ptr = device;
					object obj = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), ptr, *(*(int*)ptr + 20));
				}
				this.m_Device = null;
			}
		}

		// Token: 0x0600013F RID: 319 RVA: 0x00013224 File Offset: 0x00012624
		public unsafe virtual string GetDevicePath()
		{
			IConnectedDevice* device = this.m_Device;
			ushort* value;
			int num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr,System.UInt16 modopt(System.Runtime.CompilerServices.IsConst)**), device, ref value, *(*(int*)device));
			if (num < 0)
			{
				CAutoDeleteArray<unsigned\u0020short\u0020const\u0020> cautoDeleteArray<unsigned_u0020short_u0020const_u0020>;
				*(ref cautoDeleteArray<unsigned_u0020short_u0020const_u0020> + 4) = <Module>.UfphNativeStrFormat((ushort*)(&<Module>.??_C@_1DE@GCEKIMOP@?$AAF?$AAa?$AAi?$AAl?$AAe?$AAd?$AA?5?$AAt?$AAo?$AA?5?$AAg?$AAe?$AAt?$AA?5?$AAd?$AAe?$AAv?$AAi?$AAc?$AAe?$AA?5?$AAp?$AAa?$AAt?$AAh?$AA?$AA@));
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

		// Token: 0x06000140 RID: 320 RVA: 0x00013334 File Offset: 0x00012734
		public unsafe virtual void SendRawData([In] byte[] Message, uint MessageLength, uint Timeout)
		{
			CAutoDeleteArray<unsigned\u0020char> cautoDeleteArray<unsigned_u0020char>;
			*(ref cautoDeleteArray<unsigned_u0020char> + 4) = <Module>.new[](MessageLength);
			cautoDeleteArray<unsigned_u0020char> = ref <Module>.??_7?$CAutoDeleteArray@E@RAII@@6B@;
			CAutoDeleteArray<unsigned\u0020short\u0020const\u0020> cautoDeleteArray<unsigned_u0020short_u0020const_u0020>;
			try
			{
				IntPtr destination = (IntPtr)calli(System.Byte* modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), ref cautoDeleteArray<unsigned_u0020char>, *(cautoDeleteArray<unsigned_u0020char> + 16));
				Marshal.Copy(Message, 0, destination, (int)MessageLength);
				IConnectedDevice* device = this.m_Device;
				int num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr,System.Byte* modopt(System.Runtime.CompilerServices.IsConst) modopt(System.Runtime.CompilerServices.IsConst),System.UInt32 modopt(System.Runtime.CompilerServices.IsLong),System.UInt32 modopt(System.Runtime.CompilerServices.IsLong)), device, calli(System.Byte* modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), ref cautoDeleteArray<unsigned_u0020char>, *(cautoDeleteArray<unsigned_u0020char> + 16)), MessageLength, Timeout, *(*(int*)device + 4));
				if (num < 0)
				{
					*(ref cautoDeleteArray<unsigned_u0020short_u0020const_u0020> + 4) = <Module>.UfphNativeStrFormat((ushort*)(&<Module>.??_C@_1DA@DIJJDJIP@?$AAF?$AAa?$AAi?$AAl?$AAe?$AAd?$AA?5?$AAt?$AAo?$AA?5?$AAs?$AAe?$AAn?$AAd?$AA?5?$AAr?$AAa?$AAw?$AA?5?$AAd?$AAa?$AAt?$AAa?$AA?$AA@));
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
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(RAII.CAutoDeleteArray<unsigned\u0020char>.{dtor}), (void*)(&cautoDeleteArray<unsigned_u0020char>));
				throw;
			}
			<Module>.RAII.CAutoDeleteArray<unsigned\u0020char>.{dtor}(ref cautoDeleteArray<unsigned_u0020char>);
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
				<Module>.___CxxCallUnwindDtor(ldftn(RAII.CAutoDeleteArray<unsigned\u0020char>.{dtor}), (void*)(&cautoDeleteArray<unsigned_u0020char>));
				throw;
			}
		}

		// Token: 0x06000141 RID: 321 RVA: 0x000134D0 File Offset: 0x000128D0
		public unsafe virtual void ReceiveRawData(out byte[] Message, ref uint MessageLength, uint Timeout)
		{
			uint num = MessageLength;
			CAutoDeleteArray<unsigned\u0020char> cautoDeleteArray<unsigned_u0020char>;
			*(ref cautoDeleteArray<unsigned_u0020char> + 4) = <Module>.new[](MessageLength);
			cautoDeleteArray<unsigned_u0020char> = ref <Module>.??_7?$CAutoDeleteArray@E@RAII@@6B@;
			CAutoDeleteArray<unsigned\u0020short\u0020const\u0020> cautoDeleteArray<unsigned_u0020short_u0020const_u0020>;
			try
			{
				IConnectedDevice* device = this.m_Device;
				int num2 = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr,System.Byte* modopt(System.Runtime.CompilerServices.IsConst) modopt(System.Runtime.CompilerServices.IsConst),System.UInt32 modopt(System.Runtime.CompilerServices.IsLong)*,System.UInt32 modopt(System.Runtime.CompilerServices.IsLong)), device, calli(System.Byte* modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), ref cautoDeleteArray<unsigned_u0020char>, *(cautoDeleteArray<unsigned_u0020char> + 16)), ref num, Timeout, *(*(int*)device + 8));
				MessageLength = num;
				if (num2 < 0)
				{
					*(ref cautoDeleteArray<unsigned_u0020short_u0020const_u0020> + 4) = <Module>.UfphNativeStrFormat((ushort*)(&<Module>.??_C@_1DG@KNJJMPJN@?$AAF?$AAa?$AAi?$AAl?$AAe?$AAd?$AA?5?$AAt?$AAo?$AA?5?$AAr?$AAe?$AAc?$AAe?$AAi?$AAv?$AAe?$AA?5?$AAr?$AAa?$AAw?$AA?5?$AAd?$AAa?$AAt?$AAa?$AA?$AA@));
					cautoDeleteArray<unsigned_u0020short_u0020const_u0020> = ref <Module>.??_7?$CAutoDeleteArray@$$CBG@RAII@@6B@;
					try
					{
						IFlashingPlatform* native = this.m_Platform.Native;
						if (calli(FlashingPlatform.ILogger* modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), native, *(*(int*)native)))
						{
							IFlashingPlatform* native2 = this.m_Platform.Native;
							int num3 = calli(FlashingPlatform.ILogger* modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), native2, *(*(int*)native2));
							calli(System.Void modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr,FlashingPlatform.LogLevel,System.UInt16 modopt(System.Runtime.CompilerServices.IsConst)*), num3, 2, calli(System.UInt16 modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), ref cautoDeleteArray<unsigned_u0020short_u0020const_u0020>, *(cautoDeleteArray<unsigned_u0020short_u0020const_u0020> + 16)), *(*num3 + 4));
						}
						IntPtr ptr = (IntPtr)calli(System.UInt16 modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), ref cautoDeleteArray<unsigned_u0020short_u0020const_u0020>, *(cautoDeleteArray<unsigned_u0020short_u0020const_u0020> + 16));
						throw new Win32Exception(num2, Marshal.PtrToStringUni(ptr));
					}
					catch
					{
						<Module>.___CxxCallUnwindDtor(ldftn(RAII.CAutoDeleteArray<unsigned\u0020short\u0020const\u0020>.{dtor}), (void*)(&cautoDeleteArray<unsigned_u0020short_u0020const_u0020>));
						throw;
					}
				}
				Message = new byte[num];
				Marshal.Copy((IntPtr)calli(System.Byte* modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), ref cautoDeleteArray<unsigned_u0020char>, *(cautoDeleteArray<unsigned_u0020char> + 16)), Message, 0, (int)MessageLength);
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(RAII.CAutoDeleteArray<unsigned\u0020char>.{dtor}), (void*)(&cautoDeleteArray<unsigned_u0020char>));
				throw;
			}
			<Module>.RAII.CAutoDeleteArray<unsigned\u0020char>.{dtor}(ref cautoDeleteArray<unsigned_u0020char>);
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
				<Module>.___CxxCallUnwindDtor(ldftn(RAII.CAutoDeleteArray<unsigned\u0020char>.{dtor}), (void*)(&cautoDeleteArray<unsigned_u0020char>));
				throw;
			}
		}

		// Token: 0x06000142 RID: 322 RVA: 0x00012D44 File Offset: 0x00012144
		[return: MarshalAs(UnmanagedType.U1)]
		public unsafe virtual bool CanFlash()
		{
			IConnectedDevice* device = this.m_Device;
			return ((calli(System.Int32 modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), device, *(*(int*)device + 12)) != 0) ? 1 : 0) != 0;
		}

		// Token: 0x06000143 RID: 323 RVA: 0x00013FC0 File Offset: 0x000133C0
		public unsafe virtual FlashingDevice CreateFlashingDevice()
		{
			CAutoRelease<FlashingPlatform::IFlashingDevice\u0020*> cautoRelease<FlashingPlatform::IFlashingDevice_u0020*>;
			*(ref cautoRelease<FlashingPlatform::IFlashingDevice_u0020*> + 4) = 0;
			cautoRelease<FlashingPlatform::IFlashingDevice_u0020*> = ref <Module>.??_7?$CAutoRelease@PAUIFlashingDevice@FlashingPlatform@@@RAII@@6B@;
			FlashingDevice result;
			try
			{
				IConnectedDevice* device = this.m_Device;
				int num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr,FlashingPlatform.IFlashingDevice**), device, calli(FlashingPlatform.IFlashingDevice** modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), ref cautoRelease<FlashingPlatform::IFlashingDevice_u0020*>, *(cautoRelease<FlashingPlatform::IFlashingDevice_u0020*> + 4)), *(*(int*)device + 16));
				if (num < 0)
				{
					CAutoDeleteArray<unsigned\u0020short\u0020const\u0020> cautoDeleteArray<unsigned_u0020short_u0020const_u0020>;
					*(ref cautoDeleteArray<unsigned_u0020short_u0020const_u0020> + 4) = <Module>.UfphNativeStrFormat((ushort*)(&<Module>.??_C@_1EC@LEPNEJDA@?$AAF?$AAa?$AAi?$AAl?$AAe?$AAd?$AA?5?$AAt?$AAo?$AA?5?$AAc?$AAr?$AAe?$AAa?$AAt?$AAe?$AA?5?$AAf?$AAl?$AAa?$AAs?$AAh?$AAi?$AAn?$AAg?$AA?5?$AAd?$AAe?$AAv?$AAi?$AAc?$AAe@));
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
				result = new FlashingDevice(calli(FlashingPlatform.IFlashingDevice* modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), ref cautoRelease<FlashingPlatform::IFlashingDevice_u0020*>, *(cautoRelease<FlashingPlatform::IFlashingDevice_u0020*> + 40)), this.m_Platform);
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(RAII.CAutoRelease<FlashingPlatform::IFlashingDevice\u0020*>.{dtor}), (void*)(&cautoRelease<FlashingPlatform::IFlashingDevice_u0020*>));
				throw;
			}
			cautoRelease<FlashingPlatform::IFlashingDevice_u0020*> = ref <Module>.??_7?$CAutoRelease@PAUIFlashingDevice@FlashingPlatform@@@RAII@@6B@;
			<Module>.RAII.CAutoRelease<FlashingPlatform::IFlashingDevice\u0020*>.Release(ref cautoRelease<FlashingPlatform::IFlashingDevice_u0020*>);
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
				<Module>.___CxxCallUnwindDtor(ldftn(RAII.CAutoRelease<FlashingPlatform::IFlashingDevice\u0020*>.{dtor}), (void*)(&cautoRelease<FlashingPlatform::IFlashingDevice_u0020*>));
				throw;
			}
		}

		// Token: 0x06000144 RID: 324 RVA: 0x000125C8 File Offset: 0x000119C8
		[HandleProcessCorruptedStateExceptions]
		protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool A_0)
		{
			if (A_0)
			{
				this.~ConnectedDevice();
			}
			else
			{
				try
				{
					this.!ConnectedDevice();
				}
				finally
				{
					base.Finalize();
				}
			}
		}

		// Token: 0x06000145 RID: 325 RVA: 0x00012818 File Offset: 0x00011C18
		public sealed void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000146 RID: 326 RVA: 0x00012614 File Offset: 0x00011A14
		protected override void Finalize()
		{
			this.Dispose(false);
		}

		// Token: 0x0400011C RID: 284
		private unsafe IConnectedDevice* m_Device = Device;

		// Token: 0x0400011D RID: 285
		private bool m_OwnDevice;

		// Token: 0x0400011E RID: 286
		internal FlashingPlatform m_Platform;
	}
}
