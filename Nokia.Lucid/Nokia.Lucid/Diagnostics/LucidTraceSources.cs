using System;
using System.Diagnostics;

namespace Nokia.Lucid.Diagnostics
{
	// Token: 0x02000022 RID: 34
	public static class LucidTraceSources
	{
		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000100 RID: 256 RVA: 0x0000A3F1 File Offset: 0x000085F1
		public static TraceSource MessageTraceSource
		{
			get
			{
				return Nokia.Lucid.Diagnostics.MessageTraceSource.Instance;
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000101 RID: 257 RVA: 0x0000A3F8 File Offset: 0x000085F8
		public static TraceSource DeviceDetectionTraceSource
		{
			get
			{
				return Nokia.Lucid.Diagnostics.DeviceDetectionTraceSource.Instance;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000102 RID: 258 RVA: 0x0000A3FF File Offset: 0x000085FF
		public static TraceSource UsbDeviceIoTraceSource
		{
			get
			{
				return Nokia.Lucid.Diagnostics.UsbDeviceIoTraceSource.Instance;
			}
		}
	}
}
