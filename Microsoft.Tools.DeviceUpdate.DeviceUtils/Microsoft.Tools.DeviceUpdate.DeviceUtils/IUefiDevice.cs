using System;
using FFUComponents;

namespace Microsoft.Tools.DeviceUpdate.DeviceUtils
{
	// Token: 0x02000011 RID: 17
	public interface IUefiDevice : IDevicePropertyCollection, IDisposable
	{
		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000A7 RID: 167
		Guid DeviceUniqueId { get; }

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000A8 RID: 168
		[DeviceProperty("uniqueID")]
		string UniqueID { get; }

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000A9 RID: 169
		[DeviceProperty("uefiName")]
		string UefiName { get; }

		// Token: 0x060000AA RID: 170
		void FlashFFU(string path, bool optimize, EventHandler<ProgressEventArgs> progressEventHandler);

		// Token: 0x060000AB RID: 171
		void WriteWim(string path, EventHandler<ProgressEventArgs> progressEventHandler);

		// Token: 0x060000AC RID: 172
		void SkipFFU();

		// Token: 0x060000AD RID: 173
		void ReadPartition(string partition, out byte[] data);

		// Token: 0x060000AE RID: 174
		void WritePartition(string partition, byte[] data);

		// Token: 0x060000AF RID: 175
		void ClearPlatformIDOverride();
	}
}
