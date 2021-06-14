using System;
using Nokia.Lucid.DeviceDetection;

namespace Microsoft.WindowsDeviceRecoveryTool.Detection
{
	// Token: 0x0200001B RID: 27
	public sealed class UsbDeviceChangeEvent
	{
		// Token: 0x060000DD RID: 221 RVA: 0x00005FC8 File Offset: 0x000041C8
		public UsbDeviceChangeEvent(DeviceChangedEventArgs data, bool isEnumerated = false)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			this.Data = data;
			this.IsEnumerated = isEnumerated;
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000DE RID: 222 RVA: 0x00006008 File Offset: 0x00004208
		// (set) Token: 0x060000DF RID: 223 RVA: 0x0000601F File Offset: 0x0000421F
		public DeviceChangedEventArgs Data { get; private set; }

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000E0 RID: 224 RVA: 0x00006028 File Offset: 0x00004228
		// (set) Token: 0x060000E1 RID: 225 RVA: 0x0000603F File Offset: 0x0000423F
		public bool IsEnumerated { get; private set; }
	}
}
