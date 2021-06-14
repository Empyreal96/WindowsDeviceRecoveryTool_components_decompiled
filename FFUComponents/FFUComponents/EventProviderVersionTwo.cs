using System;
using System.Diagnostics.Eventing;
using System.Runtime.InteropServices;

namespace FFUComponents
{
	// Token: 0x0200005C RID: 92
	internal class EventProviderVersionTwo : EventProvider
	{
		// Token: 0x060001FB RID: 507 RVA: 0x00009C5C File Offset: 0x00007E5C
		internal EventProviderVersionTwo(Guid id) : base(id)
		{
		}

		// Token: 0x060001FC RID: 508 RVA: 0x00009C68 File Offset: 0x00007E68
		internal unsafe bool TemplateDeviceSpecificEventWithString(ref EventDescriptor eventDescriptor, Guid DeviceId, string DeviceFriendlyName, string AssemblyFileVersion)
		{
			int num = 3;
			bool result = true;
			if (base.IsEnabled(eventDescriptor.Level, eventDescriptor.Keywords))
			{
				byte* ptr = stackalloc byte[(UIntPtr)(sizeof(EventProviderVersionTwo.EventData) * num)];
				EventProviderVersionTwo.EventData* ptr2 = (EventProviderVersionTwo.EventData*)ptr;
				ptr2->DataPointer = &DeviceId;
				ptr2->Size = (uint)sizeof(Guid);
				ptr2[1].Size = (uint)((DeviceFriendlyName.Length + 1) * 2);
				ptr2[2].Size = (uint)((AssemblyFileVersion.Length + 1) * 2);
				fixed (char* ptr3 = DeviceFriendlyName, ptr4 = AssemblyFileVersion)
				{
					ptr2[1].DataPointer = ptr3;
					ptr2[2].DataPointer = ptr4;
					result = base.WriteEvent(ref eventDescriptor, num, (IntPtr)((void*)ptr));
				}
			}
			return result;
		}

		// Token: 0x060001FD RID: 509 RVA: 0x00009D44 File Offset: 0x00007F44
		internal unsafe bool TemplateDeviceSpecificEvent(ref EventDescriptor eventDescriptor, Guid DeviceId, string DeviceFriendlyName)
		{
			int num = 2;
			bool result = true;
			if (base.IsEnabled(eventDescriptor.Level, eventDescriptor.Keywords))
			{
				byte* ptr = stackalloc byte[(UIntPtr)(sizeof(EventProviderVersionTwo.EventData) * num)];
				EventProviderVersionTwo.EventData* ptr2 = (EventProviderVersionTwo.EventData*)ptr;
				ptr2->DataPointer = &DeviceId;
				ptr2->Size = (uint)sizeof(Guid);
				ptr2[1].Size = (uint)((DeviceFriendlyName.Length + 1) * 2);
				fixed (char* ptr3 = DeviceFriendlyName)
				{
					ptr2[1].DataPointer = ptr3;
					result = base.WriteEvent(ref eventDescriptor, num, (IntPtr)((void*)ptr));
				}
			}
			return result;
		}

		// Token: 0x060001FE RID: 510 RVA: 0x00009DD8 File Offset: 0x00007FD8
		internal unsafe bool TemplateDeviceEventWithErrorCode(ref EventDescriptor eventDescriptor, Guid DeviceId, string DeviceFriendlyName, int ErrorCode)
		{
			int num = 3;
			bool result = true;
			if (base.IsEnabled(eventDescriptor.Level, eventDescriptor.Keywords))
			{
				byte* ptr = stackalloc byte[(UIntPtr)(sizeof(EventProviderVersionTwo.EventData) * num)];
				EventProviderVersionTwo.EventData* ptr2 = (EventProviderVersionTwo.EventData*)ptr;
				ptr2->DataPointer = &DeviceId;
				ptr2->Size = (uint)sizeof(Guid);
				ptr2[1].Size = (uint)((DeviceFriendlyName.Length + 1) * 2);
				ptr2[2].DataPointer = &ErrorCode;
				ptr2[2].Size = 4U;
				fixed (char* ptr3 = DeviceFriendlyName)
				{
					ptr2[1].DataPointer = ptr3;
					result = base.WriteEvent(ref eventDescriptor, num, (IntPtr)((void*)ptr));
				}
			}
			return result;
		}

		// Token: 0x060001FF RID: 511 RVA: 0x00009E94 File Offset: 0x00008094
		internal unsafe bool TemplateNotifyException(ref EventDescriptor eventDescriptor, string DevicePath, string Exception)
		{
			int num = 2;
			bool result = true;
			if (base.IsEnabled(eventDescriptor.Level, eventDescriptor.Keywords))
			{
				byte* ptr = stackalloc byte[(UIntPtr)(sizeof(EventProviderVersionTwo.EventData) * num)];
				EventProviderVersionTwo.EventData* ptr2 = (EventProviderVersionTwo.EventData*)ptr;
				ptr2->Size = (uint)((DevicePath.Length + 1) * 2);
				ptr2[1].Size = (uint)((Exception.Length + 1) * 2);
				fixed (char* ptr3 = DevicePath, ptr4 = Exception)
				{
					ptr2->DataPointer = ptr3;
					ptr2[1].DataPointer = ptr4;
					result = base.WriteEvent(ref eventDescriptor, num, (IntPtr)((void*)ptr));
				}
			}
			return result;
		}

		// Token: 0x06000200 RID: 512 RVA: 0x00009F44 File Offset: 0x00008144
		internal unsafe bool TemplateDeviceSpecificEventWithSize(ref EventDescriptor eventDescriptor, Guid DeviceId, string DeviceFriendlyName, int TransferSize)
		{
			int num = 3;
			bool result = true;
			if (base.IsEnabled(eventDescriptor.Level, eventDescriptor.Keywords))
			{
				byte* ptr = stackalloc byte[(UIntPtr)(sizeof(EventProviderVersionTwo.EventData) * num)];
				EventProviderVersionTwo.EventData* ptr2 = (EventProviderVersionTwo.EventData*)ptr;
				ptr2->DataPointer = &DeviceId;
				ptr2->Size = (uint)sizeof(Guid);
				ptr2[1].Size = (uint)((DeviceFriendlyName.Length + 1) * 2);
				ptr2[2].DataPointer = &TransferSize;
				ptr2[2].Size = 4U;
				fixed (char* ptr3 = DeviceFriendlyName)
				{
					ptr2[1].DataPointer = ptr3;
					result = base.WriteEvent(ref eventDescriptor, num, (IntPtr)((void*)ptr));
				}
			}
			return result;
		}

		// Token: 0x06000201 RID: 513 RVA: 0x0000A000 File Offset: 0x00008200
		internal unsafe bool TemplateDeviceFlashParameters(ref EventDescriptor eventDescriptor, int USBTransactionSize, int PacketDataSize)
		{
			int num = 2;
			bool result = true;
			if (base.IsEnabled(eventDescriptor.Level, eventDescriptor.Keywords))
			{
				byte* ptr = stackalloc byte[(UIntPtr)(sizeof(EventProviderVersionTwo.EventData) * num)];
				EventProviderVersionTwo.EventData* ptr2 = (EventProviderVersionTwo.EventData*)ptr;
				ptr2->DataPointer = &USBTransactionSize;
				ptr2->Size = 4U;
				ptr2[1].DataPointer = &PacketDataSize;
				ptr2[1].Size = 4U;
				result = base.WriteEvent(ref eventDescriptor, num, (IntPtr)((void*)ptr));
			}
			return result;
		}

		// Token: 0x0200005D RID: 93
		[StructLayout(LayoutKind.Explicit, Size = 16)]
		private struct EventData
		{
			// Token: 0x040001CF RID: 463
			[FieldOffset(0)]
			internal ulong DataPointer;

			// Token: 0x040001D0 RID: 464
			[FieldOffset(8)]
			internal uint Size;

			// Token: 0x040001D1 RID: 465
			[FieldOffset(12)]
			internal int Reserved;
		}
	}
}
