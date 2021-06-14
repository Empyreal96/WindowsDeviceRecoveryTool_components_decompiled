using System;
using Microsoft.WindowsPhone.ImageUpdate.Tools.Common;

namespace Microsoft.WindowsPhone.Imaging
{
	// Token: 0x0200000C RID: 12
	public class ImageGenerator
	{
		// Token: 0x060000B0 RID: 176 RVA: 0x000048AF File Offset: 0x00002AAF
		public void Initialize(ImageGeneratorParameters parameters, IULogger logger)
		{
			this.Initialize(parameters, logger, false);
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x000048BA File Offset: 0x00002ABA
		public void Initialize(ImageGeneratorParameters parameters, IULogger logger, bool isDesktopImage)
		{
			this._logger = logger;
			if (logger == null)
			{
				this._logger = new IULogger();
			}
			this._parameters = parameters;
			this._isDesktopImage = isDesktopImage;
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x000048E0 File Offset: 0x00002AE0
		public FullFlashUpdateImage CreateFFU()
		{
			FullFlashUpdateImage fullFlashUpdateImage = new FullFlashUpdateImage();
			if (this._parameters == null)
			{
				throw new ImageCommonException("ImageCommon!ImageGenerator::CreateFFU: ImageGenerator has not been initialized.");
			}
			try
			{
				this._parameters.VerifyInputParameters();
				fullFlashUpdateImage.Initialize();
				fullFlashUpdateImage.Description = this._parameters.Description;
				fullFlashUpdateImage.DevicePlatformIDs = this._parameters.DevicePlatformIDs;
				fullFlashUpdateImage.ChunkSize = this._parameters.ChunkSize;
				fullFlashUpdateImage.HashAlgorithmID = this._parameters.Algid;
				fullFlashUpdateImage.DefaultPartitionAlignmentInBytes = this._parameters.DefaultPartitionByteAlignment;
			}
			catch (Exception innerException)
			{
				throw new ImageCommonException("ImageCommon!ImageGenerator::CreateFFU: Failed to Initialize FFU: ", innerException);
			}
			try
			{
				foreach (InputStore inputStore in this._parameters.Stores)
				{
					FullFlashUpdateImage.FullFlashUpdateStore fullFlashUpdateStore = new FullFlashUpdateImage.FullFlashUpdateStore();
					uint minSectorCount = this._parameters.MinSectorCount;
					if (!inputStore.IsMainOSStore())
					{
						minSectorCount = inputStore.SizeInSectors;
					}
					fullFlashUpdateStore.Initialize(fullFlashUpdateImage, inputStore.Id, inputStore.IsMainOSStore(), inputStore.DevicePath, inputStore.OnlyAllocateDefinedGptEntries, minSectorCount, this._parameters.SectorSize);
					foreach (InputPartition inputPartition in inputStore.Partitions)
					{
						FullFlashUpdateImage.FullFlashUpdatePartition fullFlashUpdatePartition = new FullFlashUpdateImage.FullFlashUpdatePartition();
						fullFlashUpdatePartition.Initialize(0U, inputPartition.TotalSectors, inputPartition.Type, inputPartition.Id, inputPartition.Name, fullFlashUpdateStore, inputPartition.UseAllSpace, this._isDesktopImage);
						fullFlashUpdatePartition.FileSystem = inputPartition.FileSystem;
						fullFlashUpdatePartition.Bootable = inputPartition.Bootable;
						fullFlashUpdatePartition.ReadOnly = inputPartition.ReadOnly;
						fullFlashUpdatePartition.Hidden = inputPartition.Hidden;
						fullFlashUpdatePartition.AttachDriveLetter = inputPartition.AttachDriveLetter;
						fullFlashUpdatePartition.PrimaryPartition = inputPartition.PrimaryPartition;
						fullFlashUpdatePartition.RequiredToFlash = inputPartition.RequiredToFlash;
						fullFlashUpdatePartition.ByteAlignment = inputPartition.ByteAlignment;
						fullFlashUpdatePartition.ClusterSize = inputPartition.ClusterSize;
						fullFlashUpdateStore.AddPartition(fullFlashUpdatePartition);
						if (!inputStore.IsMainOSStore() && inputPartition.ByteAlignment == 0U)
						{
							fullFlashUpdatePartition.ByteAlignment = fullFlashUpdateImage.ChunkSize * 1024U;
						}
					}
					fullFlashUpdateImage.AddStore(fullFlashUpdateStore);
				}
			}
			catch (Exception innerException2)
			{
				throw new ImageCommonException("ImageCommon!ImageGenerator::CreateFFU: Failed to add partitions to FFU: ", innerException2);
			}
			return fullFlashUpdateImage;
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00004B78 File Offset: 0x00002D78
		private uint AlignSectors(uint sectorCount, uint alignmentInSectors)
		{
			if (alignmentInSectors != 0U && sectorCount % alignmentInSectors != 0U)
			{
				return sectorCount + (alignmentInSectors - sectorCount % alignmentInSectors);
			}
			return sectorCount;
		}

		// Token: 0x0400003A RID: 58
		private IULogger _logger;

		// Token: 0x0400003B RID: 59
		private ImageGeneratorParameters _parameters;

		// Token: 0x0400003C RID: 60
		private bool _isDesktopImage;
	}
}
