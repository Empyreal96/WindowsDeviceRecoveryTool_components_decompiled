using System;
using System.IO;

namespace Microsoft.Tools.DeviceUpdate.DeviceUtils
{
	// Token: 0x02000008 RID: 8
	public interface IUpdateableDevice : IDevicePropertyCollection, IDisposable
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000021 RID: 33
		// (remove) Token: 0x06000022 RID: 34
		event MessageHandler NormalMessageEvent;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000023 RID: 35
		// (remove) Token: 0x06000024 RID: 36
		event MessageHandler WarningMessageEvent;

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000025 RID: 37
		// (remove) Token: 0x06000026 RID: 38
		event MessageHandler ProgressEvent;

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000027 RID: 39
		InstalledPackageInfo[] InstalledPackages { get; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000028 RID: 40
		[DeviceProperty("buildString")]
		string BuildString { get; }

		// Token: 0x06000029 RID: 41
		void RebootToUefi();

		// Token: 0x0600002A RID: 42
		void RebootToTarget(uint Target);

		// Token: 0x0600002B RID: 43
		void StartDeviceUpdateScan(uint throttle);

		// Token: 0x0600002C RID: 44
		void InitiateDuInstall();

		// Token: 0x0600002D RID: 45
		void ClearDuStagingDirectory();

		// Token: 0x0600002E RID: 46
		void GetDuDiagnostics(string path);

		// Token: 0x0600002F RID: 47
		void GetPackageInfo(string path);

		// Token: 0x06000030 RID: 48
		void SendIuPackage(string path);

		// Token: 0x06000031 RID: 49
		void SendIuPackage(Stream stream);
	}
}
