using System;

namespace FFUComponents
{
	// Token: 0x0200001F RID: 31
	public interface IFFUDevice : IDisposable
	{
		// Token: 0x17000012 RID: 18
		// (get) Token: 0x060000A3 RID: 163
		string DeviceFriendlyName { get; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x060000A4 RID: 164
		Guid DeviceUniqueID { get; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x060000A5 RID: 165
		Guid SerialNumber { get; }

		// Token: 0x060000A6 RID: 166
		void FlashFFUFile(string ffuFilePath);

		// Token: 0x060000A7 RID: 167
		void FlashFFUFile(string ffuFilePath, bool optimize);

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x060000A8 RID: 168
		// (remove) Token: 0x060000A9 RID: 169
		event EventHandler<ProgressEventArgs> ProgressEvent;

		// Token: 0x060000AA RID: 170
		bool WriteWim(string wimPath);

		// Token: 0x060000AB RID: 171
		bool EndTransfer();

		// Token: 0x060000AC RID: 172
		bool SkipTransfer();

		// Token: 0x060000AD RID: 173
		bool Reboot();

		// Token: 0x060000AE RID: 174
		bool EnterMassStorage();

		// Token: 0x060000AF RID: 175
		bool ClearIdOverride();

		// Token: 0x060000B0 RID: 176
		bool GetDiskInfo(out uint blockSize, out ulong lastBlock);

		// Token: 0x060000B1 RID: 177
		void ReadDisk(ulong diskOffset, byte[] buffer, int offset, int count);

		// Token: 0x060000B2 RID: 178
		void WriteDisk(ulong diskOffset, byte[] buffer, int offset, int count);

		// Token: 0x060000B3 RID: 179
		uint SetBootMode(uint bootMode, string profileName);

		// Token: 0x060000B4 RID: 180
		void QueryDeviceUnlockId(out byte[] unlockId, out byte[] oemId, out byte[] platformId);

		// Token: 0x060000B5 RID: 181
		void RelockDeviceUnlockId();

		// Token: 0x060000B6 RID: 182
		uint[] QueryUnlockTokenFiles();

		// Token: 0x060000B7 RID: 183
		void WriteUnlockTokenFile(uint unlockTokenId, byte[] fileData);

		// Token: 0x060000B8 RID: 184
		bool QueryBitlockerState();

		// Token: 0x060000B9 RID: 185
		string GetServicingLogs(string logFolderPath);

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x060000BA RID: 186
		string DeviceType { get; }
	}
}
