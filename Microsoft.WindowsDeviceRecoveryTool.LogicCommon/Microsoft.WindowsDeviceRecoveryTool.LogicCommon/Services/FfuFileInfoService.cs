using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using FfuFileReader;
using Microsoft.WindowsDeviceRecoveryTool.Common.Tracing;
using Microsoft.WindowsDeviceRecoveryTool.Model;
using Microsoft.WindowsDeviceRecoveryTool.Model.Exceptions;
using Microsoft.WindowsPhone.Imaging;

namespace Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Services
{
	// Token: 0x02000036 RID: 54
	[Export]
	public class FfuFileInfoService
	{
		// Token: 0x060002D7 RID: 727 RVA: 0x0000BE7F File Offset: 0x0000A07F
		[ImportingConstructor]
		public FfuFileInfoService()
		{
			this.ffuReaderManaged = new FfuReaderManaged();
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x060002D8 RID: 728 RVA: 0x0000BE98 File Offset: 0x0000A098
		// (set) Token: 0x060002D9 RID: 729 RVA: 0x0000BEAF File Offset: 0x0000A0AF
		public string RootKeyHash { get; private set; }

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x060002DA RID: 730 RVA: 0x0000BEB8 File Offset: 0x0000A0B8
		// (set) Token: 0x060002DB RID: 731 RVA: 0x0000BECF File Offset: 0x0000A0CF
		public string PlatformId { get; private set; }

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x060002DC RID: 732 RVA: 0x0000BED8 File Offset: 0x0000A0D8
		// (set) Token: 0x060002DD RID: 733 RVA: 0x0000BEEF File Offset: 0x0000A0EF
		public string FullName { get; private set; }

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x060002DE RID: 734 RVA: 0x0000BEF8 File Offset: 0x0000A0F8
		// (set) Token: 0x060002DF RID: 735 RVA: 0x0000BF0F File Offset: 0x0000A10F
		public string Name { get; private set; }

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x060002E0 RID: 736 RVA: 0x0000BF18 File Offset: 0x0000A118
		// (set) Token: 0x060002E1 RID: 737 RVA: 0x0000BF2F File Offset: 0x0000A12F
		public long Length { get; private set; }

		// Token: 0x060002E2 RID: 738 RVA: 0x0000BF38 File Offset: 0x0000A138
		public PlatformId ReadFfuFilePlatformId(string ffuFilePath)
		{
			PlatformId platformId = new PlatformId();
			platformId.SetPlatformId(this.ReadFfuPlatformId(ffuFilePath));
			return platformId;
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x0000BF60 File Offset: 0x0000A160
		public string ReadFfuPlatformId(string ffuFileName)
		{
			FileInfo fileInfo = new FileInfo(ffuFileName);
			int num = this.ffuReaderManaged.ReadPlatformId(fileInfo.FullName);
			if (num != 0)
			{
				throw new FfuFileInfoReadException(num, ffuFileName);
			}
			this.RootKeyHash = this.ffuReaderManaged.RootKeyHash;
			this.PlatformId = this.ffuReaderManaged.PlatformId;
			this.FullName = fileInfo.FullName;
			this.Name = fileInfo.Name;
			this.Length = fileInfo.Length;
			return this.ffuReaderManaged.PlatformId;
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x0000BFF4 File Offset: 0x0000A1F4
		public void ReadFfuFile(string ffuFileName)
		{
			FileInfo fileInfo = new FileInfo(ffuFileName);
			int num = this.ffuReaderManaged.Read(fileInfo.FullName);
			if (num != 0)
			{
				throw new FfuFileInfoReadException(num, ffuFileName);
			}
			this.RootKeyHash = this.ffuReaderManaged.RootKeyHash;
			this.PlatformId = this.ffuReaderManaged.PlatformId;
			this.FullName = fileInfo.FullName;
			this.Name = fileInfo.Name;
			this.Length = fileInfo.Length;
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x0000C078 File Offset: 0x0000A278
		public bool TryReadFfuSoftwareVersion(string ffuFilePath, out string version)
		{
			version = null;
			bool result;
			try
			{
				FullFlashUpdateImage orReadImageFromFile = FfuFileInfoService.GetOrReadImageFromFile(ffuFilePath);
				version = orReadImageFromFile.OSVersion;
				result = true;
			}
			catch (Exception error)
			{
				Tracer<FfuFileInfoService>.WriteWarning(error, "Could not read ffu image: {0}", new object[]
				{
					ffuFilePath
				});
				result = false;
			}
			return result;
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x0000C0D0 File Offset: 0x0000A2D0
		public bool TryReadAllFfuPlatformIds(string ffuFilePath, out IEnumerable<PlatformId> platformIds)
		{
			platformIds = null;
			bool result;
			try
			{
				FullFlashUpdateImage orReadImageFromFile = FfuFileInfoService.GetOrReadImageFromFile(ffuFilePath);
				List<PlatformId> list = new List<PlatformId>();
				foreach (string platformId in orReadImageFromFile.DevicePlatformIDs)
				{
					PlatformId platformId2 = new PlatformId();
					platformId2.SetPlatformId(platformId);
					list.Add(platformId2);
				}
				platformIds = list;
				result = true;
			}
			catch (Exception error)
			{
				Tracer<FfuFileInfoService>.WriteWarning(error, "Could not read ffu image: {0}", new object[]
				{
					ffuFilePath
				});
				result = false;
			}
			return result;
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x0000C170 File Offset: 0x0000A370
		private static FullFlashUpdateImage GetOrReadImageFromFile(string ffuFilePath)
		{
			if (!FfuFileInfoService.imagesDataCache.ContainsKey(ffuFilePath))
			{
				FullFlashUpdateImage fullFlashUpdateImage = new FullFlashUpdateImage();
				fullFlashUpdateImage.Initialize(ffuFilePath);
				FfuFileInfoService.imagesDataCache.Add(ffuFilePath, fullFlashUpdateImage);
			}
			return FfuFileInfoService.imagesDataCache[ffuFilePath];
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x0000C1BC File Offset: 0x0000A3BC
		public void ClearDataForFfuFile(string ffuFilePath)
		{
			if (FfuFileInfoService.imagesDataCache.ContainsKey(ffuFilePath))
			{
				FfuFileInfoService.imagesDataCache.Remove(ffuFilePath);
			}
		}

		// Token: 0x0400016D RID: 365
		private readonly FfuReaderManaged ffuReaderManaged;

		// Token: 0x0400016E RID: 366
		private static Dictionary<string, FullFlashUpdateImage> imagesDataCache = new Dictionary<string, FullFlashUpdateImage>();
	}
}
