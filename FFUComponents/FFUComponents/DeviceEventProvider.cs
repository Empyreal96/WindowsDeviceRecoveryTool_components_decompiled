using System;
using System.Diagnostics.Eventing;
using System.Runtime.InteropServices;

namespace FFUComponents
{
	// Token: 0x0200001D RID: 29
	internal class DeviceEventProvider : EventProvider
	{
		// Token: 0x060000A1 RID: 161 RVA: 0x00004212 File Offset: 0x00002412
		internal DeviceEventProvider(Guid id) : base(id)
		{
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x0000421C File Offset: 0x0000241C
		internal unsafe bool TemplateDeviceEvent(ref EventDescriptor eventDescriptor, Guid DeviceUniqueId, string DeviceFriendlyName, string AdditionalInfoString)
		{
			int num = 3;
			bool result = true;
			if (base.IsEnabled(eventDescriptor.Level, eventDescriptor.Keywords))
			{
				byte* ptr = stackalloc byte[(UIntPtr)(sizeof(DeviceEventProvider.EventData) * num)];
				DeviceEventProvider.EventData* ptr2 = (DeviceEventProvider.EventData*)ptr;
				ptr2->DataPointer = &DeviceUniqueId;
				ptr2->Size = (uint)sizeof(Guid);
				ptr2[1].Size = (uint)((DeviceFriendlyName.Length + 1) * 2);
				ptr2[2].Size = (uint)((AdditionalInfoString.Length + 1) * 2);
				fixed (char* ptr3 = DeviceFriendlyName, ptr4 = AdditionalInfoString)
				{
					ptr2[1].DataPointer = ptr3;
					ptr2[2].DataPointer = ptr4;
					result = base.WriteEvent(ref eventDescriptor, num, (IntPtr)((void*)ptr));
				}
			}
			return result;
		}

		// Token: 0x0200001E RID: 30
		[StructLayout(LayoutKind.Explicit, Size = 16)]
		private struct EventData
		{
			// Token: 0x04000045 RID: 69
			[FieldOffset(0)]
			internal ulong DataPointer;

			// Token: 0x04000046 RID: 70
			[FieldOffset(8)]
			internal uint Size;

			// Token: 0x04000047 RID: 71
			[FieldOffset(12)]
			internal int Reserved;
		}
	}
}
