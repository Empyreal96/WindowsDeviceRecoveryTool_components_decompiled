using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ClickerUtilityLibrary.Comm.USBDriver
{
	// Token: 0x02000025 RID: 37
	internal class CfgMgr
	{
		// Token: 0x0600012B RID: 299
		[DllImport("CfgMgr32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.U4)]
		internal static extern uint CM_Register_Notification([MarshalAs(UnmanagedType.LPStruct)] [In] CfgMgr.CM_NOTIFY_FILTER pFilter, IntPtr pContext, CfgMgr.CM_NOTIFY_CALLBACK pCallback, out UIntPtr pNotifyContext);

		// Token: 0x0600012C RID: 300
		[DllImport("CfgMgr32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.U4)]
		internal static extern uint CM_Unregister_Notification(UIntPtr NotifyContext);

		// Token: 0x040000F3 RID: 243
		internal const int MAX_DEVICE_ID_LEN = 200;

		// Token: 0x040000F4 RID: 244
		internal const int ANYSIZE_ARRAY = 1;

		// Token: 0x0200004B RID: 75
		internal enum WinError : uint
		{
			// Token: 0x040001B1 RID: 433
			Success,
			// Token: 0x040001B2 RID: 434
			Cancelled = 1223U
		}

		// Token: 0x0200004C RID: 76
		[Flags]
		internal enum CM_NOTIFY_FILTER_FLAGS : uint
		{
			// Token: 0x040001B4 RID: 436
			CM_NOTIFY_FILTER_FLAG_ALL_INTERFACE_CLASSES = 1U,
			// Token: 0x040001B5 RID: 437
			CM_NOTIFY_FILTER_FLAG_ALL_DEVICE_INSTANCES = 2U
		}

		// Token: 0x0200004D RID: 77
		internal enum CM_NOTIFY_FILTER_TYPE
		{
			// Token: 0x040001B7 RID: 439
			CM_NOTIFY_FILTER_TYPE_DEVICEINTERFACE,
			// Token: 0x040001B8 RID: 440
			CM_NOTIFY_FILTER_TYPE_DEVICEHANDLE,
			// Token: 0x040001B9 RID: 441
			CM_NOTIFY_FILTER_TYPE_DEVICEINSTANCE,
			// Token: 0x040001BA RID: 442
			CM_NOTIFY_FILTER_TYPE_MAX
		}

		// Token: 0x0200004E RID: 78
		internal struct CM_NOTIFY_FILTER_DEVICE_INTERFACE
		{
			// Token: 0x040001BB RID: 443
			public Guid ClassGuid;
		}

		// Token: 0x0200004F RID: 79
		internal struct CM_NOTIFY_FILTER_DEVICE_HANDLE
		{
			// Token: 0x040001BC RID: 444
			public IntPtr hTarget;
		}

		// Token: 0x02000050 RID: 80
		internal struct CM_NOTIFY_FILTER_DEVICE_INSTANCE
		{
			// Token: 0x040001BD RID: 445
			[FixedBuffer(typeof(ushort), 200)]
			public CfgMgr.CM_NOTIFY_FILTER_DEVICE_INSTANCE.<InstanceId>e__FixedBuffer InstanceId;

			// Token: 0x0200005C RID: 92
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 400)]
			public struct <InstanceId>e__FixedBuffer
			{
				// Token: 0x040001E7 RID: 487
				public ushort FixedElementField;
			}
		}

		// Token: 0x02000051 RID: 81
		[StructLayout(LayoutKind.Explicit)]
		internal struct CM_NOTIFY_FILTER_UNION
		{
			// Token: 0x040001BE RID: 446
			[FieldOffset(0)]
			public CfgMgr.CM_NOTIFY_FILTER_DEVICE_INTERFACE DeviceInterface;

			// Token: 0x040001BF RID: 447
			[FieldOffset(0)]
			public CfgMgr.CM_NOTIFY_FILTER_DEVICE_HANDLE DeviceHandle;

			// Token: 0x040001C0 RID: 448
			[FieldOffset(0)]
			public CfgMgr.CM_NOTIFY_FILTER_DEVICE_INSTANCE DeviceInstance;
		}

		// Token: 0x02000052 RID: 82
		[StructLayout(LayoutKind.Sequential)]
		internal class CM_NOTIFY_FILTER
		{
			// Token: 0x040001C1 RID: 449
			public uint cbSize;

			// Token: 0x040001C2 RID: 450
			public CfgMgr.CM_NOTIFY_FILTER_FLAGS Flags;

			// Token: 0x040001C3 RID: 451
			public CfgMgr.CM_NOTIFY_FILTER_TYPE FilterType;

			// Token: 0x040001C4 RID: 452
			public uint Reserved;

			// Token: 0x040001C5 RID: 453
			public CfgMgr.CM_NOTIFY_FILTER_UNION u;
		}

		// Token: 0x02000053 RID: 83
		internal enum CM_NOTIFY_ACTION
		{
			// Token: 0x040001C7 RID: 455
			CM_NOTIFY_ACTION_DEVICEINTERFACEARRIVAL,
			// Token: 0x040001C8 RID: 456
			CM_NOTIFY_ACTION_DEVICEINTERFACEREMOVAL,
			// Token: 0x040001C9 RID: 457
			CM_NOTIFY_ACTION_DEVICEQUERYREMOVE,
			// Token: 0x040001CA RID: 458
			CM_NOTIFY_ACTION_DEVICEQUERYREMOVEFAILED,
			// Token: 0x040001CB RID: 459
			CM_NOTIFY_ACTION_DEVICEREMOVEPENDING,
			// Token: 0x040001CC RID: 460
			CM_NOTIFY_ACTION_DEVICEREMOVECOMPLETE,
			// Token: 0x040001CD RID: 461
			CM_NOTIFY_ACTION_DEVICECUSTOMEVENT,
			// Token: 0x040001CE RID: 462
			CM_NOTIFY_ACTION_DEVICEINSTANCEENUMERATED,
			// Token: 0x040001CF RID: 463
			CM_NOTIFY_ACTION_DEVICEINSTANCESTARTED,
			// Token: 0x040001D0 RID: 464
			CM_NOTIFY_ACTION_DEVICEINSTANCEREMOVED,
			// Token: 0x040001D1 RID: 465
			CM_NOTIFY_ACTION_MAX
		}

		// Token: 0x02000054 RID: 84
		internal struct CM_NOTIFY_EVENT_DATA_DEVICE_INTERFACE
		{
			// Token: 0x040001D2 RID: 466
			public Guid ClassGuid;

			// Token: 0x040001D3 RID: 467
			[FixedBuffer(typeof(ushort), 1)]
			public CfgMgr.CM_NOTIFY_EVENT_DATA_DEVICE_INTERFACE.<SymbolicLink>e__FixedBuffer SymbolicLink;

			// Token: 0x0200005D RID: 93
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 2)]
			public struct <SymbolicLink>e__FixedBuffer
			{
				// Token: 0x040001E8 RID: 488
				public ushort FixedElementField;
			}
		}

		// Token: 0x02000055 RID: 85
		internal struct CM_NOTIFY_EVENT_DATA_DEVICE_HANDLE
		{
			// Token: 0x040001D4 RID: 468
			public Guid EventGuid;

			// Token: 0x040001D5 RID: 469
			public int NameOffset;

			// Token: 0x040001D6 RID: 470
			public uint DataSize;

			// Token: 0x040001D7 RID: 471
			[FixedBuffer(typeof(byte), 1)]
			public CfgMgr.CM_NOTIFY_EVENT_DATA_DEVICE_HANDLE.<Data>e__FixedBuffer Data;

			// Token: 0x0200005E RID: 94
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 1)]
			public struct <Data>e__FixedBuffer
			{
				// Token: 0x040001E9 RID: 489
				public byte FixedElementField;
			}
		}

		// Token: 0x02000056 RID: 86
		internal struct CM_NOTIFY_EVENT_DATA_DEVICE_INSTANCE
		{
			// Token: 0x040001D8 RID: 472
			[FixedBuffer(typeof(ushort), 1)]
			public CfgMgr.CM_NOTIFY_EVENT_DATA_DEVICE_INSTANCE.<InstanceId>e__FixedBuffer InstanceId;

			// Token: 0x0200005F RID: 95
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 2)]
			public struct <InstanceId>e__FixedBuffer
			{
				// Token: 0x040001EA RID: 490
				public ushort FixedElementField;
			}
		}

		// Token: 0x02000057 RID: 87
		[StructLayout(LayoutKind.Explicit)]
		internal struct CM_NOTIFY_EVENT_DATA_UNION
		{
			// Token: 0x040001D9 RID: 473
			[FieldOffset(0)]
			public CfgMgr.CM_NOTIFY_EVENT_DATA_DEVICE_INTERFACE DeviceInterface;

			// Token: 0x040001DA RID: 474
			[FieldOffset(0)]
			public CfgMgr.CM_NOTIFY_EVENT_DATA_DEVICE_HANDLE DeviceHandle;

			// Token: 0x040001DB RID: 475
			[FieldOffset(0)]
			public CfgMgr.CM_NOTIFY_EVENT_DATA_DEVICE_INSTANCE DeviceInstance;
		}

		// Token: 0x02000058 RID: 88
		internal struct CM_NOTIFY_EVENT_DATA
		{
			// Token: 0x040001DC RID: 476
			public CfgMgr.CM_NOTIFY_FILTER_TYPE FilterType;

			// Token: 0x040001DD RID: 477
			public uint Reserved;

			// Token: 0x040001DE RID: 478
			public CfgMgr.CM_NOTIFY_EVENT_DATA_UNION u;
		}

		// Token: 0x02000059 RID: 89
		// (Invoke) Token: 0x06000177 RID: 375
		internal delegate uint CM_NOTIFY_CALLBACK(IntPtr hNotify, IntPtr Context, CfgMgr.CM_NOTIFY_ACTION Action, ref CfgMgr.CM_NOTIFY_EVENT_DATA EventData, uint EventDataSize);
	}
}
