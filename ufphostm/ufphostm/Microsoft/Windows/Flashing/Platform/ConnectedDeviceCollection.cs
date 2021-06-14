using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using FlashingPlatform;
using RAII;

namespace Microsoft.Windows.Flashing.Platform
{
	// Token: 0x02000041 RID: 65
	public class ConnectedDeviceCollection : IEnumerable<ConnectedDevice>, IEnumerator<ConnectedDevice>
	{
		// Token: 0x0600012A RID: 298 RVA: 0x00012B9C File Offset: 0x00011F9C
		internal unsafe ConnectedDeviceCollection(IConnectedDeviceCollection* Collection, [In] FlashingPlatform Platform)
		{
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600012B RID: 299 RVA: 0x00012BFC File Offset: 0x00011FFC
		internal unsafe IConnectedDeviceCollection* Native
		{
			get
			{
				return this.m_Collection;
			}
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00013024 File Offset: 0x00012424
		private void ~ConnectedDeviceCollection()
		{
			this.!ConnectedDeviceCollection();
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00012BCC File Offset: 0x00011FCC
		private unsafe void !ConnectedDeviceCollection()
		{
			IConnectedDeviceCollection* collection = this.m_Collection;
			if (collection != null)
			{
				IConnectedDeviceCollection* ptr = collection;
				object obj = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), ptr, *(*(int*)ptr + 8));
				this.m_Collection = null;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600012E RID: 302 RVA: 0x00012C14 File Offset: 0x00012014
		public unsafe virtual int Count
		{
			get
			{
				IConnectedDeviceCollection* collection = this.m_Collection;
				return calli(System.UInt32 modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), collection, *(*(int*)collection));
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600012F RID: 303 RVA: 0x0001462C File Offset: 0x00013A2C
		public virtual object Current
		{
			get
			{
				return this.CurrentT;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000130 RID: 304 RVA: 0x00013F64 File Offset: 0x00013364
		public unsafe virtual ConnectedDevice CurrentT
		{
			get
			{
				if (this.m_Index < 0)
				{
					throw new InvalidOperationException(Marshal.PtrToStringUni((IntPtr)((void*)(&<Module>.??_C@_1DC@CMEHKPEC@?$AAY?$AAo?$AAu?$AA?5?$AAm?$AAu?$AAs?$AAt?$AA?5?$AAc?$AAa?$AAl?$AAl?$AA?5?$AAM?$AAo?$AAv?$AAe?$AAN?$AAe?$AAx?$AAt?$AA?$CI?$AA?$CJ?$AA?$AA@))));
				}
				int index = this.m_Index;
				if (index >= this.Count)
				{
					throw new InvalidOperationException(Marshal.PtrToStringUni((IntPtr)((void*)(&<Module>.??_C@_1CM@MEAAIPAK@?$AAE?$AAn?$AAu?$AAm?$AAe?$AAr?$AAa?$AAt?$AAi?$AAo?$AAn?$AA?5?$AAh?$AAa?$AAs?$AA?5?$AAe?$AAn?$AAd?$AAe?$AAd?$AA?$AA@))));
				}
				return this.GetConnectedDeviceAt((uint)index);
			}
		}

		// Token: 0x06000131 RID: 305 RVA: 0x0001303C File Offset: 0x0001243C
		public virtual IEnumerator GetEnumerator()
		{
			return this.GetEnumeratorT();
		}

		// Token: 0x06000132 RID: 306 RVA: 0x00012C34 File Offset: 0x00012034
		public virtual IEnumerator<ConnectedDevice> GetEnumeratorT()
		{
			return this;
		}

		// Token: 0x06000133 RID: 307 RVA: 0x00012C48 File Offset: 0x00012048
		[return: MarshalAs(UnmanagedType.U1)]
		public virtual bool MoveNext()
		{
			int index = this.m_Index;
			if (index < this.Count)
			{
				this.m_Index = index + 1;
			}
			return ((this.m_Index < this.Count) ? 1 : 0) != 0;
		}

		// Token: 0x06000134 RID: 308 RVA: 0x00012C84 File Offset: 0x00012084
		public virtual void Reset()
		{
			this.m_Index = -1;
		}

		// Token: 0x06000135 RID: 309 RVA: 0x00012C14 File Offset: 0x00012014
		public unsafe uint GetCount()
		{
			IConnectedDeviceCollection* collection = this.m_Collection;
			return calli(System.UInt32 modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), collection, *(*(int*)collection));
		}

		// Token: 0x06000136 RID: 310 RVA: 0x00013084 File Offset: 0x00012484
		public unsafe ConnectedDevice GetConnectedDeviceAt(uint Index)
		{
			CAutoRelease<FlashingPlatform::IConnectedDevice\u0020*> cautoRelease<FlashingPlatform::IConnectedDevice_u0020*>;
			*(ref cautoRelease<FlashingPlatform::IConnectedDevice_u0020*> + 4) = 0;
			cautoRelease<FlashingPlatform::IConnectedDevice_u0020*> = ref <Module>.??_7?$CAutoRelease@PAUIConnectedDevice@FlashingPlatform@@@RAII@@6B@;
			ConnectedDevice result;
			try
			{
				IConnectedDeviceCollection* collection = this.m_Collection;
				int num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr,System.UInt32,FlashingPlatform.IConnectedDevice**), collection, Index, calli(FlashingPlatform.IConnectedDevice** modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), ref cautoRelease<FlashingPlatform::IConnectedDevice_u0020*>, *(cautoRelease<FlashingPlatform::IConnectedDevice_u0020*> + 4)), *(*(int*)collection + 4));
				if (num < 0)
				{
					CAutoDeleteArray<unsigned\u0020short\u0020const\u0020> cautoDeleteArray<unsigned_u0020short_u0020const_u0020>;
					*(ref cautoDeleteArray<unsigned_u0020short_u0020const_u0020> + 4) = <Module>.UfphNativeStrFormat((ushort*)(&<Module>.??_C@_1FG@NPJPCCED@?$AAF?$AAa?$AAi?$AAl?$AAe?$AAd?$AA?5?$AAt?$AAo?$AA?5?$AAg?$AAe?$AAt?$AA?5?$AAc?$AAo?$AAn?$AAn?$AAe?$AAc?$AAt?$AAe?$AAd?$AA?5?$AAd?$AAe?$AAv?$AAi?$AAc?$AAe?$AA?5?$AAa@), Index);
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
				result = new ConnectedDevice(calli(FlashingPlatform.IConnectedDevice* modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), ref cautoRelease<FlashingPlatform::IConnectedDevice_u0020*>, *(cautoRelease<FlashingPlatform::IConnectedDevice_u0020*> + 40)), this.m_Platform);
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(RAII.CAutoRelease<FlashingPlatform::IConnectedDevice\u0020*>.{dtor}), (void*)(&cautoRelease<FlashingPlatform::IConnectedDevice_u0020*>));
				throw;
			}
			cautoRelease<FlashingPlatform::IConnectedDevice_u0020*> = ref <Module>.??_7?$CAutoRelease@PAUIConnectedDevice@FlashingPlatform@@@RAII@@6B@;
			<Module>.RAII.CAutoRelease<FlashingPlatform::IConnectedDevice\u0020*>.Release(ref cautoRelease<FlashingPlatform::IConnectedDevice_u0020*>);
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
				<Module>.___CxxCallUnwindDtor(ldftn(RAII.CAutoRelease<FlashingPlatform::IConnectedDevice\u0020*>.{dtor}), (void*)(&cautoRelease<FlashingPlatform::IConnectedDevice_u0020*>));
				throw;
			}
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00012560 File Offset: 0x00011960
		[HandleProcessCorruptedStateExceptions]
		protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool A_0)
		{
			if (A_0)
			{
				this.~ConnectedDeviceCollection();
			}
			else
			{
				try
				{
					this.!ConnectedDeviceCollection();
				}
				finally
				{
					base.Finalize();
				}
			}
		}

		// Token: 0x06000138 RID: 312 RVA: 0x000127F8 File Offset: 0x00011BF8
		public sealed void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000139 RID: 313 RVA: 0x000125AC File Offset: 0x000119AC
		protected override void Finalize()
		{
			this.Dispose(false);
		}

		// Token: 0x04000119 RID: 281
		private unsafe IConnectedDeviceCollection* m_Collection = Collection;

		// Token: 0x0400011A RID: 282
		private int m_Index = -1;

		// Token: 0x0400011B RID: 283
		internal FlashingPlatform m_Platform = Platform;
	}
}
