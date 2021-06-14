using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.WindowsPhone.ImageUpdate.Tools.Common;

namespace Microsoft.WindowsPhone.Imaging
{
	// Token: 0x02000004 RID: 4
	public class FullFlashUpdateImage
	{
		// Token: 0x0600000C RID: 12 RVA: 0x000021AC File Offset: 0x000003AC
		public void Initialize(string imagePath)
		{
			if (!File.Exists(imagePath))
			{
				throw new ImageCommonException("ImageCommon!FullFlashUpdateImage::Initialize: The FFU file '" + imagePath + "' does not exist.");
			}
			this._imagePath = Path.GetFullPath(imagePath);
			using (FileStream imageStream = this.GetImageStream())
			{
				using (BinaryReader binaryReader = new BinaryReader(imageStream))
				{
					uint num = binaryReader.ReadUInt32();
					byte[] signature = binaryReader.ReadBytes(12);
					if (num != FullFlashUpdateHeaders.SecurityHeaderSize || !FullFlashUpdateImage.SecurityHeader.ValidateSignature(signature))
					{
						throw new ImageCommonException("ImageCommon!FullFlashUpdateImage::Initialize: Unable to load image because the security header is invalid.");
					}
					this._securityHeader.ByteCount = num;
					this._securityHeader.ChunkSize = binaryReader.ReadUInt32();
					this._securityHeader.HashAlgorithmID = binaryReader.ReadUInt32();
					this._securityHeader.CatalogSize = binaryReader.ReadUInt32();
					this._securityHeader.HashTableSize = binaryReader.ReadUInt32();
					this._catalogData = binaryReader.ReadBytes((int)this._securityHeader.CatalogSize);
					this._hashTableData = binaryReader.ReadBytes((int)this._securityHeader.HashTableSize);
					binaryReader.ReadBytes((int)this.SecurityPadding);
					num = binaryReader.ReadUInt32();
					signature = binaryReader.ReadBytes(12);
					if (num != FullFlashUpdateHeaders.ImageHeaderSize || !FullFlashUpdateImage.ImageHeader.ValidateSignature(signature))
					{
						throw new ImageCommonException("ImageCommon!FullFlashUpdateImage::Initialize: Unable to load image because the image header is invalid.");
					}
					this._imageHeader.ByteCount = num;
					this._imageHeader.ManifestLength = binaryReader.ReadUInt32();
					this._imageHeader.ChunkSize = binaryReader.ReadUInt32();
					MemoryStream stream = new MemoryStream(binaryReader.ReadBytes((int)this._imageHeader.ManifestLength));
					StreamReader streamReader = new StreamReader(stream, Encoding.ASCII);
					try
					{
						this._manifest = new FullFlashUpdateImage.FullFlashUpdateManifest(this, streamReader);
					}
					finally
					{
						streamReader.Close();
						streamReader = null;
					}
					this.ValidateManifest();
					if (this._imageHeader.ChunkSize > 0U)
					{
						binaryReader.ReadBytes((int)this.CalculateAlignment((uint)imageStream.Position, this._imageHeader.ChunkSize * 1024U));
					}
					this._payloadOffset = imageStream.Position;
				}
			}
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000023F4 File Offset: 0x000005F4
		public FileStream GetImageStream()
		{
			FileStream fileStream = File.OpenRead(this._imagePath);
			fileStream.Position = this._payloadOffset;
			return fileStream;
		}

		// Token: 0x0600000E RID: 14 RVA: 0x0000241A File Offset: 0x0000061A
		[CLSCompliant(false)]
		public void AddStore(FullFlashUpdateImage.FullFlashUpdateStore store)
		{
			if (store == null)
			{
				throw new ArgumentNullException("ImageCommon!FullFlashUpdateImage::AddStore: store is null");
			}
			this._stores.Add(store);
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002438 File Offset: 0x00000638
		private void AddStore(FullFlashUpdateImage.ManifestCategory category)
		{
			uint sectorSize = uint.Parse(category["SectorSize"], CultureInfo.InvariantCulture);
			uint minSectorCount = 0U;
			if (category["MinSectorCount"] != null)
			{
				minSectorCount = uint.Parse(category["MinSectorCount"], CultureInfo.InvariantCulture);
			}
			string storeId = null;
			if (category["StoreId"] != null)
			{
				storeId = category["StoreId"];
			}
			bool isMainOSStore = true;
			if (category["IsMainOSStore"] != null)
			{
				isMainOSStore = bool.Parse(category["IsMainOSStore"]);
			}
			string devicePath = null;
			if (category["DevicePath"] != null)
			{
				devicePath = category["DevicePath"];
			}
			bool onlyAllocateDefinedGptEntries = false;
			if (category["OnlyAllocateDefinedGptEntries"] != null)
			{
				onlyAllocateDefinedGptEntries = bool.Parse(category["OnlyAllocateDefinedGptEntries"]);
			}
			FullFlashUpdateImage.FullFlashUpdateStore fullFlashUpdateStore = new FullFlashUpdateImage.FullFlashUpdateStore();
			fullFlashUpdateStore.Initialize(this, storeId, isMainOSStore, devicePath, onlyAllocateDefinedGptEntries, minSectorCount, sectorSize);
			this._stores.Add(fullFlashUpdateStore);
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000010 RID: 16 RVA: 0x0000251E File Offset: 0x0000071E
		public FullFlashUpdateImage.SecurityHeader GetSecureHeader
		{
			get
			{
				return this._securityHeader;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000011 RID: 17 RVA: 0x00002528 File Offset: 0x00000728
		public static int SecureHeaderSize
		{
			get
			{
				return Marshal.SizeOf<FullFlashUpdateImage.SecurityHeader>(default(FullFlashUpdateImage.SecurityHeader));
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000012 RID: 18 RVA: 0x00002543 File Offset: 0x00000743
		// (set) Token: 0x06000013 RID: 19 RVA: 0x0000254B File Offset: 0x0000074B
		public byte[] CatalogData
		{
			get
			{
				return this._catalogData;
			}
			set
			{
				this._catalogData = value;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000014 RID: 20 RVA: 0x00002554 File Offset: 0x00000754
		// (set) Token: 0x06000015 RID: 21 RVA: 0x0000255C File Offset: 0x0000075C
		public byte[] HashTableData
		{
			get
			{
				return this._hashTableData;
			}
			set
			{
				this._hashTableData = value;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000016 RID: 22 RVA: 0x00002565 File Offset: 0x00000765
		public FullFlashUpdateImage.ImageHeader GetImageHeader
		{
			get
			{
				return this._imageHeader;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000017 RID: 23 RVA: 0x00002570 File Offset: 0x00000770
		public static int ImageHeaderSize
		{
			get
			{
				return Marshal.SizeOf<FullFlashUpdateImage.ImageHeader>(default(FullFlashUpdateImage.ImageHeader));
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000018 RID: 24 RVA: 0x0000258B File Offset: 0x0000078B
		// (set) Token: 0x06000019 RID: 25 RVA: 0x00002598 File Offset: 0x00000798
		[CLSCompliant(false)]
		public uint ChunkSize
		{
			get
			{
				return this._imageHeader.ChunkSize;
			}
			set
			{
				this._imageHeader.ChunkSize = value;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600001A RID: 26 RVA: 0x000025A6 File Offset: 0x000007A6
		public uint ChunkSizeInBytes
		{
			get
			{
				return this.ChunkSize * 1024U;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600001B RID: 27 RVA: 0x000025B4 File Offset: 0x000007B4
		// (set) Token: 0x0600001C RID: 28 RVA: 0x000025C1 File Offset: 0x000007C1
		[CLSCompliant(false)]
		public uint HashAlgorithmID
		{
			get
			{
				return this._securityHeader.HashAlgorithmID;
			}
			set
			{
				this._securityHeader.HashAlgorithmID = value;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600001D RID: 29 RVA: 0x000025CF File Offset: 0x000007CF
		// (set) Token: 0x0600001E RID: 30 RVA: 0x000025DC File Offset: 0x000007DC
		[CLSCompliant(false)]
		public uint ManifestLength
		{
			get
			{
				return this._imageHeader.ManifestLength;
			}
			set
			{
				this._imageHeader.ManifestLength = value;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600001F RID: 31 RVA: 0x000025EA File Offset: 0x000007EA
		public int StoreCount
		{
			get
			{
				return this._stores.Count;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000020 RID: 32 RVA: 0x000025F7 File Offset: 0x000007F7
		public List<FullFlashUpdateImage.FullFlashUpdateStore> Stores
		{
			get
			{
				return this._stores;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000021 RID: 33 RVA: 0x00002600 File Offset: 0x00000800
		public uint ImageStyle
		{
			get
			{
				bool flag = true;
				if (this.Stores[0].Partitions != null && this.Stores[0].Partitions.Count<FullFlashUpdateImage.FullFlashUpdatePartition>() > 0)
				{
					flag = FullFlashUpdateImage.IsGPTPartitionType(this.Stores[0].Partitions[0].PartitionType);
				}
				if (!flag)
				{
					return FullFlashUpdateImage.PartitionTypeMbr;
				}
				return FullFlashUpdateImage.PartitionTypeGpt;
			}
		}

		// Token: 0x06000022 RID: 34 RVA: 0x0000266C File Offset: 0x0000086C
		public static bool IsGPTPartitionType(string partitionType)
		{
			Guid guid;
			return Guid.TryParse(partitionType, out guid);
		}

		// Token: 0x17000015 RID: 21
		public FullFlashUpdateImage.FullFlashUpdatePartition this[string name]
		{
			get
			{
				foreach (FullFlashUpdateImage.FullFlashUpdateStore fullFlashUpdateStore in this.Stores)
				{
					foreach (FullFlashUpdateImage.FullFlashUpdatePartition fullFlashUpdatePartition in fullFlashUpdateStore.Partitions)
					{
						if (string.CompareOrdinal(fullFlashUpdatePartition.Name, name) == 0)
						{
							return fullFlashUpdatePartition;
						}
					}
				}
				return null;
			}
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002724 File Offset: 0x00000924
		public void DisplayImageInformation(IULogger logger)
		{
			foreach (string text in this.DevicePlatformIDs)
			{
				logger.LogInfo("\tDevice Platform ID: {0}", new object[]
				{
					text
				});
			}
			logger.LogInfo("\tChunk Size: 0x{0:X}", new object[]
			{
				this.ChunkSize
			});
			logger.LogInfo(" ", new object[0]);
			foreach (FullFlashUpdateImage.FullFlashUpdateStore fullFlashUpdateStore in this.Stores)
			{
				logger.LogInfo("Store", new object[0]);
				logger.LogInfo("\tSector Size: 0x{0:X}", new object[]
				{
					fullFlashUpdateStore.SectorSize
				});
				logger.LogInfo("\tID: {0}", new object[]
				{
					fullFlashUpdateStore.Id
				});
				logger.LogInfo("\tDevice Path: {0}", new object[]
				{
					fullFlashUpdateStore.DevicePath
				});
				logger.LogInfo("\tContains MainOS: {0}", new object[]
				{
					fullFlashUpdateStore.IsMainOSStore
				});
				if (fullFlashUpdateStore.IsMainOSStore)
				{
					logger.LogInfo("\tMinimum Sector Count: 0x{0:X}", new object[]
					{
						fullFlashUpdateStore.SectorCount
					});
				}
				logger.LogInfo(" ", new object[0]);
				logger.LogInfo("There are {0} partitions in the store.", new object[]
				{
					fullFlashUpdateStore.Partitions.Count
				});
				logger.LogInfo(" ", new object[0]);
				foreach (FullFlashUpdateImage.FullFlashUpdatePartition fullFlashUpdatePartition in fullFlashUpdateStore.Partitions)
				{
					logger.LogInfo("\tPartition", new object[0]);
					logger.LogInfo("\t\tName: {0}", new object[]
					{
						fullFlashUpdatePartition.Name
					});
					logger.LogInfo("\t\tPartiton Type: {0}", new object[]
					{
						fullFlashUpdatePartition.PartitionType
					});
					logger.LogInfo("\t\tTotal Sectors: 0x{0:X}", new object[]
					{
						fullFlashUpdatePartition.TotalSectors
					});
					logger.LogInfo("\t\tSectors In Use: 0x{0:X}", new object[]
					{
						fullFlashUpdatePartition.SectorsInUse
					});
					logger.LogInfo(" ", new object[0]);
				}
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000025 RID: 37 RVA: 0x000029F4 File Offset: 0x00000BF4
		public uint StartOfImageHeader
		{
			get
			{
				uint num = 0U;
				if (this._manifest != null)
				{
					num += FullFlashUpdateHeaders.SecurityHeaderSize;
					num += this._securityHeader.CatalogSize;
					num += this._securityHeader.HashTableSize;
				}
				return num;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000026 RID: 38 RVA: 0x00002A30 File Offset: 0x00000C30
		private uint StartOfManifest
		{
			get
			{
				uint num = 0U;
				if (this._manifest != null)
				{
					num += this.StartOfImageHeader;
					num += FullFlashUpdateHeaders.ImageHeaderSize;
				}
				return num;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000027 RID: 39 RVA: 0x00002A59 File Offset: 0x00000C59
		public FullFlashUpdateImage.FullFlashUpdateManifest GetManifest
		{
			get
			{
				return this._manifest;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000028 RID: 40 RVA: 0x00002A61 File Offset: 0x00000C61
		// (set) Token: 0x06000029 RID: 41 RVA: 0x00002A69 File Offset: 0x00000C69
		[CLSCompliant(false)]
		public uint DefaultPartitionAlignmentInBytes
		{
			get
			{
				return this._defaultPartititionByteAlignment;
			}
			set
			{
				if (InputHelpers.IsPowerOfTwo(value))
				{
					this._defaultPartititionByteAlignment = value;
				}
			}
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002A7C File Offset: 0x00000C7C
		private uint CalculateAlignment(uint currentPosition, uint blockSize)
		{
			uint result = 0U;
			uint num = currentPosition % blockSize;
			if (num > 0U)
			{
				result = blockSize - num;
			}
			return result;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002A98 File Offset: 0x00000C98
		private void WritePadding(Stream outputStream, uint paddingSize)
		{
			for (uint num = 0U; num < paddingSize; num += 1U)
			{
				outputStream.WriteByte(Encoding.ASCII.GetBytes(" ")[0]);
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600002C RID: 44 RVA: 0x00002AC8 File Offset: 0x00000CC8
		[CLSCompliant(false)]
		public uint SecurityPadding
		{
			get
			{
				uint num = 1024U;
				if (this._imageHeader.ChunkSize != 0U)
				{
					num *= this._imageHeader.ChunkSize;
				}
				else
				{
					if (this._securityHeader.ChunkSize == 0U)
					{
						throw new ImageCommonException("ImageCommon!FullFlashUpdateImage::SecurityPadding: Neither the of the headers have been initialized with a chunk size.");
					}
					num *= this._securityHeader.ChunkSize;
				}
				return this.CalculateAlignment(FullFlashUpdateHeaders.SecurityHeaderSize + (uint)((this.CatalogData != null) ? this.CatalogData.Length : 0) + (uint)((this.HashTableData != null) ? this.HashTableData.Length : 0), num);
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600002D RID: 45 RVA: 0x00002B54 File Offset: 0x00000D54
		[CLSCompliant(false)]
		protected uint ManifestPadding
		{
			get
			{
				return this.CalculateAlignment(FullFlashUpdateHeaders.ImageHeaderSize + this._manifest.Length, this._imageHeader.ChunkSize * 1024U);
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600002E RID: 46 RVA: 0x00002B80 File Offset: 0x00000D80
		private uint EndOfManifest
		{
			get
			{
				uint num = 0U;
				if (this._manifest != null)
				{
					num = this.StartOfManifest;
					num += this._manifest.Length;
					num += this.ManifestPadding;
				}
				return num;
			}
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002BB8 File Offset: 0x00000DB8
		public byte[] GetSecurityHeader(byte[] catalogData, byte[] hashData)
		{
			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
			binaryWriter.Write(FullFlashUpdateHeaders.SecurityHeaderSize);
			binaryWriter.Write(FullFlashUpdateHeaders.GetSecuritySignature());
			binaryWriter.Write(this.ChunkSize);
			binaryWriter.Write(this.HashAlgorithmID);
			binaryWriter.Write(catalogData.Length);
			binaryWriter.Write(hashData.Length);
			binaryWriter.Write(catalogData);
			binaryWriter.Write(hashData);
			binaryWriter.Flush();
			if (memoryStream.Length % (long)((ulong)this.ChunkSizeInBytes) != 0L)
			{
				long num = (long)((ulong)this.ChunkSizeInBytes - (ulong)(memoryStream.Length % (long)((ulong)this.ChunkSizeInBytes)));
				memoryStream.SetLength(memoryStream.Length + num);
			}
			return memoryStream.ToArray();
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002C64 File Offset: 0x00000E64
		public byte[] GetManifestRegion()
		{
			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
			binaryWriter.Write(FullFlashUpdateHeaders.ImageHeaderSize);
			binaryWriter.Write(FullFlashUpdateHeaders.GetImageSignature());
			binaryWriter.Write(this._manifest.Length);
			binaryWriter.Write(this.ChunkSize);
			binaryWriter.Flush();
			this._manifest.WriteToStream(memoryStream);
			if (memoryStream.Length % (long)((ulong)this.ChunkSizeInBytes) != 0L)
			{
				long num = (long)((ulong)this.ChunkSizeInBytes - (ulong)(memoryStream.Length % (long)((ulong)this.ChunkSizeInBytes)));
				memoryStream.SetLength(memoryStream.Length + num);
			}
			return memoryStream.ToArray();
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000031 RID: 49 RVA: 0x00002D00 File Offset: 0x00000F00
		// (set) Token: 0x06000032 RID: 50 RVA: 0x00002D64 File Offset: 0x00000F64
		public string Description
		{
			get
			{
				if (this._manifest != null && this._manifest["FullFlash"] != null && this._manifest["FullFlash"]["Description"] != null)
				{
					return this._manifest["FullFlash"]["Description"];
				}
				return "";
			}
			set
			{
				if (this._manifest != null)
				{
					if (this._manifest["FullFlash"] == null)
					{
						this._manifest.AddCategory("FullFlash", "FullFlash");
					}
					this._manifest["FullFlash"]["Description"] = value;
				}
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000033 RID: 51 RVA: 0x00002DBC File Offset: 0x00000FBC
		// (set) Token: 0x06000034 RID: 52 RVA: 0x00002E6C File Offset: 0x0000106C
		public string[] DevicePlatformIDs
		{
			get
			{
				if (this._manifest == null || this._manifest["FullFlash"] == null)
				{
					return new string[]
					{
						""
					};
				}
				int num = 0;
				for (;;)
				{
					string name = string.Format("DevicePlatformId{0}", num);
					if (this._manifest["FullFlash"][name] == null)
					{
						break;
					}
					num++;
				}
				string[] array = new string[num];
				for (int i = 0; i < num; i++)
				{
					string name2 = string.Format("DevicePlatformId{0}", i);
					array[i] = this._manifest["FullFlash"][name2];
				}
				return array;
			}
			set
			{
				if (this._manifest != null)
				{
					if (this._manifest["FullFlash"] == null)
					{
						this._manifest.AddCategory("FullFlash", "FullFlash");
					}
					for (int i = 0; i < value.Length; i++)
					{
						string name = string.Format("DevicePlatformId{0}", i);
						this._manifest["FullFlash"][name] = value[i];
					}
				}
			}
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002EE1 File Offset: 0x000010E1
		public void WriteManifest(Stream stream)
		{
			this._manifest.WriteToStream(stream);
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002EF0 File Offset: 0x000010F0
		private void ValidateManifest()
		{
			if (this._manifest["FullFlash"] == null)
			{
				throw new ImageCommonException("ImageCommon!FullFlashUpdateImage::ValidateManifest: Missing 'FullFlash' or 'Image' category in the manifest");
			}
			string text = this._manifest["FullFlash"]["Version"];
			if (text == null)
			{
				throw new ImageCommonException("ImageCommon!FullFlashUpdateImage::ValidateManifest: Missing 'Version' name/value pair in the 'FullFlash' category.");
			}
			if (text.CompareTo("2.0") != 0)
			{
				throw new ImageCommonException("ImageCommon!FullFlashUpdateImage::ValidateManifest: 'Version' value (" + text + ") does not match current version of 2.0.");
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000037 RID: 55 RVA: 0x00002F68 File Offset: 0x00001168
		private uint PaddedManifestLength
		{
			get
			{
				return this._manifest.Length + this.ManifestPadding;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000038 RID: 56 RVA: 0x00002F7C File Offset: 0x0000117C
		// (set) Token: 0x06000039 RID: 57 RVA: 0x00002FE0 File Offset: 0x000011E0
		public string Version
		{
			get
			{
				if (this._manifest != null && this._manifest["FullFlash"] != null && this._manifest["FullFlash"]["Version"] != null)
				{
					return this._manifest["FullFlash"]["Version"];
				}
				return string.Empty;
			}
			private set
			{
				if (this._manifest != null)
				{
					if (this._manifest["FullFlash"] == null)
					{
						this._manifest.AddCategory("FullFlash", "FullFlash");
					}
					this._manifest["FullFlash"]["Version"] = value;
				}
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600003A RID: 58 RVA: 0x00003038 File Offset: 0x00001238
		// (set) Token: 0x0600003B RID: 59 RVA: 0x0000309C File Offset: 0x0000129C
		public string OSVersion
		{
			get
			{
				if (this._manifest != null && this._manifest["FullFlash"] != null && this._manifest["FullFlash"]["OSVersion"] != null)
				{
					return this._manifest["FullFlash"]["OSVersion"];
				}
				return string.Empty;
			}
			set
			{
				if (this._manifest != null)
				{
					if (this._manifest["FullFlash"] == null)
					{
						this._manifest.AddCategory("FullFlash", "FullFlash");
					}
					this._manifest["FullFlash"]["OSVersion"] = value;
				}
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600003C RID: 60 RVA: 0x000030F4 File Offset: 0x000012F4
		// (set) Token: 0x0600003D RID: 61 RVA: 0x00003158 File Offset: 0x00001358
		public string CanFlashToRemovableMedia
		{
			get
			{
				if (this._manifest != null && this._manifest["FullFlash"] != null && this._manifest["FullFlash"]["CanFlashToRemovableMedia"] != null)
				{
					return this._manifest["FullFlash"]["CanFlashToRemovableMedia"];
				}
				return string.Empty;
			}
			set
			{
				if (this._manifest != null)
				{
					if (this._manifest["FullFlash"] == null)
					{
						this._manifest.AddCategory("FullFlash", "FullFlash");
					}
					this._manifest["FullFlash"]["CanFlashToRemovableMedia"] = value;
				}
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600003E RID: 62 RVA: 0x000031B0 File Offset: 0x000013B0
		// (set) Token: 0x0600003F RID: 63 RVA: 0x00003214 File Offset: 0x00001414
		public string AntiTheftVersion
		{
			get
			{
				if (this._manifest != null && this._manifest["FullFlash"] != null && this._manifest["FullFlash"]["AntiTheftVersion"] != null)
				{
					return this._manifest["FullFlash"]["AntiTheftVersion"];
				}
				return string.Empty;
			}
			set
			{
				if (this._manifest != null)
				{
					if (this._manifest["FullFlash"] == null)
					{
						this._manifest.AddCategory("FullFlash", "FullFlash");
					}
					this._manifest["FullFlash"]["AntiTheftVersion"] = value;
				}
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000040 RID: 64 RVA: 0x0000326C File Offset: 0x0000146C
		// (set) Token: 0x06000041 RID: 65 RVA: 0x000032D8 File Offset: 0x000014D8
		private uint TotalSectors
		{
			get
			{
				if (this._manifest != null && this._manifest["Image"] != null && this._manifest["Image"]["TotalSectors"] != null)
				{
					return uint.Parse(this._manifest["Image"]["TotalSectors"], CultureInfo.InvariantCulture);
				}
				return 0U;
			}
			set
			{
				if (this._manifest != null)
				{
					if (this._manifest["Image"] == null)
					{
						this._manifest.AddCategory("Image", "Image");
					}
					this._manifest["Image"]["TotalSectors"] = value.ToString(CultureInfo.InvariantCulture);
				}
			}
		}

		// Token: 0x06000042 RID: 66 RVA: 0x0000333B File Offset: 0x0000153B
		[CLSCompliant(false)]
		public void Initialize()
		{
			this._manifest = new FullFlashUpdateImage.FullFlashUpdateManifest(this);
			this.Version = "2.0";
		}

		// Token: 0x04000001 RID: 1
		private const uint _OneKiloByte = 1024U;

		// Token: 0x04000002 RID: 2
		private const string _version = "2.0";

		// Token: 0x04000003 RID: 3
		private const uint _DefaultPartitionByteAlignment = 65536U;

		// Token: 0x04000004 RID: 4
		private FullFlashUpdateImage.FullFlashUpdateManifest _manifest;

		// Token: 0x04000005 RID: 5
		private List<FullFlashUpdateImage.FullFlashUpdateStore> _stores = new List<FullFlashUpdateImage.FullFlashUpdateStore>();

		// Token: 0x04000006 RID: 6
		private string _imagePath;

		// Token: 0x04000007 RID: 7
		private long _payloadOffset;

		// Token: 0x04000008 RID: 8
		private FullFlashUpdateImage.ImageHeader _imageHeader;

		// Token: 0x04000009 RID: 9
		private FullFlashUpdateImage.SecurityHeader _securityHeader;

		// Token: 0x0400000A RID: 10
		private byte[] _catalogData;

		// Token: 0x0400000B RID: 11
		private byte[] _hashTableData;

		// Token: 0x0400000C RID: 12
		private uint _defaultPartititionByteAlignment = 65536U;

		// Token: 0x0400000D RID: 13
		public static readonly uint PartitionTypeMbr = 0U;

		// Token: 0x0400000E RID: 14
		public static readonly uint PartitionTypeGpt = 1U;

		// Token: 0x02000005 RID: 5
		public struct SecurityHeader
		{
			// Token: 0x06000045 RID: 69 RVA: 0x00003380 File Offset: 0x00001580
			public static bool ValidateSignature(byte[] signature)
			{
				byte[] securitySignature = FullFlashUpdateHeaders.GetSecuritySignature();
				for (int i = 0; i < securitySignature.Length; i++)
				{
					if (signature[i] != securitySignature[i])
					{
						return false;
					}
				}
				return true;
			}

			// Token: 0x17000025 RID: 37
			// (get) Token: 0x06000046 RID: 70 RVA: 0x000033AC File Offset: 0x000015AC
			// (set) Token: 0x06000047 RID: 71 RVA: 0x000033B4 File Offset: 0x000015B4
			[CLSCompliant(false)]
			public uint ByteCount
			{
				get
				{
					return this._byteCount;
				}
				set
				{
					this._byteCount = value;
				}
			}

			// Token: 0x17000026 RID: 38
			// (get) Token: 0x06000048 RID: 72 RVA: 0x000033BD File Offset: 0x000015BD
			// (set) Token: 0x06000049 RID: 73 RVA: 0x000033C5 File Offset: 0x000015C5
			[CLSCompliant(false)]
			public uint ChunkSize
			{
				get
				{
					return this._chunkSize;
				}
				set
				{
					this._chunkSize = value;
				}
			}

			// Token: 0x17000027 RID: 39
			// (get) Token: 0x0600004A RID: 74 RVA: 0x000033CE File Offset: 0x000015CE
			// (set) Token: 0x0600004B RID: 75 RVA: 0x000033D6 File Offset: 0x000015D6
			[CLSCompliant(false)]
			public uint HashAlgorithmID
			{
				get
				{
					return this._algid;
				}
				set
				{
					this._algid = value;
				}
			}

			// Token: 0x17000028 RID: 40
			// (get) Token: 0x0600004C RID: 76 RVA: 0x000033DF File Offset: 0x000015DF
			// (set) Token: 0x0600004D RID: 77 RVA: 0x000033E7 File Offset: 0x000015E7
			[CLSCompliant(false)]
			public uint CatalogSize
			{
				get
				{
					return this._catalogSize;
				}
				set
				{
					this._catalogSize = value;
				}
			}

			// Token: 0x17000029 RID: 41
			// (get) Token: 0x0600004E RID: 78 RVA: 0x000033F0 File Offset: 0x000015F0
			// (set) Token: 0x0600004F RID: 79 RVA: 0x000033F8 File Offset: 0x000015F8
			[CLSCompliant(false)]
			public uint HashTableSize
			{
				get
				{
					return this._hashTableSize;
				}
				set
				{
					this._hashTableSize = value;
				}
			}

			// Token: 0x0400000F RID: 15
			private uint _byteCount;

			// Token: 0x04000010 RID: 16
			private uint _chunkSize;

			// Token: 0x04000011 RID: 17
			private uint _algid;

			// Token: 0x04000012 RID: 18
			private uint _catalogSize;

			// Token: 0x04000013 RID: 19
			private uint _hashTableSize;
		}

		// Token: 0x02000006 RID: 6
		public struct ImageHeader
		{
			// Token: 0x06000050 RID: 80 RVA: 0x00003404 File Offset: 0x00001604
			public static bool ValidateSignature(byte[] signature)
			{
				byte[] imageSignature = FullFlashUpdateHeaders.GetImageSignature();
				for (int i = 0; i < imageSignature.Length; i++)
				{
					if (signature[i] != imageSignature[i])
					{
						return false;
					}
				}
				return true;
			}

			// Token: 0x1700002A RID: 42
			// (get) Token: 0x06000051 RID: 81 RVA: 0x00003430 File Offset: 0x00001630
			// (set) Token: 0x06000052 RID: 82 RVA: 0x00003438 File Offset: 0x00001638
			[CLSCompliant(false)]
			public uint ByteCount
			{
				get
				{
					return this._byteCount;
				}
				set
				{
					this._byteCount = value;
				}
			}

			// Token: 0x1700002B RID: 43
			// (get) Token: 0x06000053 RID: 83 RVA: 0x00003441 File Offset: 0x00001641
			// (set) Token: 0x06000054 RID: 84 RVA: 0x00003449 File Offset: 0x00001649
			[CLSCompliant(false)]
			public uint ManifestLength
			{
				get
				{
					return this._manifestLength;
				}
				set
				{
					this._manifestLength = value;
				}
			}

			// Token: 0x1700002C RID: 44
			// (get) Token: 0x06000055 RID: 85 RVA: 0x00003452 File Offset: 0x00001652
			// (set) Token: 0x06000056 RID: 86 RVA: 0x0000345A File Offset: 0x0000165A
			[CLSCompliant(false)]
			public uint ChunkSize
			{
				get
				{
					return this._chunkSize;
				}
				set
				{
					this._chunkSize = value;
				}
			}

			// Token: 0x04000014 RID: 20
			private uint _byteCount;

			// Token: 0x04000015 RID: 21
			private uint _manifestLength;

			// Token: 0x04000016 RID: 22
			private uint _chunkSize;
		}

		// Token: 0x02000007 RID: 7
		public class FullFlashUpdatePartition
		{
			// Token: 0x06000057 RID: 87 RVA: 0x00003464 File Offset: 0x00001664
			[CLSCompliant(false)]
			public void Initialize(uint usedSectors, uint totalSectors, string partitionType, string partitionId, string name, FullFlashUpdateImage.FullFlashUpdateStore store, bool useAllSpace)
			{
				this.Initialize(usedSectors, totalSectors, partitionType, partitionId, name, store, useAllSpace, false);
			}

			// Token: 0x06000058 RID: 88 RVA: 0x00003484 File Offset: 0x00001684
			[CLSCompliant(false)]
			public void Initialize(uint usedSectors, uint totalSectors, string partitionType, string partitionId, string name, FullFlashUpdateImage.FullFlashUpdateStore store, bool useAllSpace, bool isDesktopImage)
			{
				this._sectorsInUse = usedSectors;
				this._totalSectors = totalSectors;
				this._type = partitionType;
				this._id = partitionId;
				this._name = name;
				this._store = store;
				this._useAllSpace = useAllSpace;
				if (!isDesktopImage && this._useAllSpace && !name.Equals("Data", StringComparison.InvariantCultureIgnoreCase))
				{
					throw new ImageCommonException(string.Format("ImageCommon!FullFlashUpdatePartition::Initialize: Partition {0} cannot specify UseAllSpace.", this._name));
				}
				if (this._totalSectors == 4294967295U)
				{
					throw new ImageCommonException(string.Concat(new object[]
					{
						"ImageCommon!FullFlashUpdatePartition::Initialize: Partition ",
						name,
						" is too large (",
						this._totalSectors,
						" sectors)"
					}));
				}
				this.ReadOnly = false;
				this.Bootable = false;
				this.Hidden = false;
				this.AttachDriveLetter = false;
				this.RequiredToFlash = false;
				this.SectorAlignment = 0U;
				this._fileSystem = string.Empty;
				this._byteAlignment = 0U;
				this._clusterSize = 0U;
			}

			// Token: 0x06000059 RID: 89 RVA: 0x00003584 File Offset: 0x00001784
			public void ToCategory(FullFlashUpdateImage.ManifestCategory category)
			{
				category.Clean();
				category["Name"] = this._name;
				category["Type"] = this._type;
				if (!string.IsNullOrEmpty(this._id))
				{
					category["Id"] = this._id;
				}
				category["Primary"] = this.PrimaryPartition;
				if (!string.IsNullOrEmpty(this._fileSystem))
				{
					category["FileSystem"] = this._fileSystem;
				}
				if (this.ReadOnly)
				{
					category["ReadOnly"] = this.ReadOnly.ToString(CultureInfo.InvariantCulture);
				}
				if (this.Hidden)
				{
					category["Hidden"] = this.Hidden.ToString(CultureInfo.InvariantCulture);
				}
				if (this.AttachDriveLetter)
				{
					category["AttachDriveLetter"] = this.AttachDriveLetter.ToString(CultureInfo.InvariantCulture);
				}
				if (this.Bootable)
				{
					category["Bootable"] = this.Bootable.ToString(CultureInfo.InvariantCulture);
				}
				if (this._useAllSpace)
				{
					category["UseAllSpace"] = "true";
				}
				else
				{
					category["TotalSectors"] = this._totalSectors.ToString(CultureInfo.InvariantCulture);
					category["UsedSectors"] = this._sectorsInUse.ToString(CultureInfo.InvariantCulture);
				}
				if (this._byteAlignment != 0U)
				{
					category["ByteAlignment"] = this._byteAlignment.ToString(CultureInfo.InvariantCulture);
				}
				if (this._clusterSize != 0U)
				{
					category["ClusterSize"] = this._clusterSize.ToString(CultureInfo.InvariantCulture);
				}
				if (this.SectorAlignment != 0U)
				{
					category["SectorAlignment"] = this.SectorAlignment.ToString(CultureInfo.InvariantCulture);
				}
				if (this.RequiredToFlash)
				{
					category["RequiredToFlash"] = this.RequiredToFlash.ToString(CultureInfo.InvariantCulture);
				}
			}

			// Token: 0x0600005A RID: 90 RVA: 0x00003782 File Offset: 0x00001982
			public override string ToString()
			{
				return this.Name;
			}

			// Token: 0x1700002D RID: 45
			// (get) Token: 0x0600005B RID: 91 RVA: 0x0000378A File Offset: 0x0000198A
			// (set) Token: 0x0600005C RID: 92 RVA: 0x00003792 File Offset: 0x00001992
			public string Name
			{
				get
				{
					return this._name;
				}
				set
				{
					this._name = value;
				}
			}

			// Token: 0x1700002E RID: 46
			// (get) Token: 0x0600005D RID: 93 RVA: 0x0000379B File Offset: 0x0000199B
			// (set) Token: 0x0600005E RID: 94 RVA: 0x000037A3 File Offset: 0x000019A3
			[CLSCompliant(false)]
			public uint TotalSectors
			{
				get
				{
					return this._totalSectors;
				}
				set
				{
					this._totalSectors = value;
				}
			}

			// Token: 0x1700002F RID: 47
			// (get) Token: 0x0600005F RID: 95 RVA: 0x000037AC File Offset: 0x000019AC
			// (set) Token: 0x06000060 RID: 96 RVA: 0x000037B4 File Offset: 0x000019B4
			public string PartitionType
			{
				get
				{
					return this._type;
				}
				set
				{
					this._type = value;
				}
			}

			// Token: 0x17000030 RID: 48
			// (get) Token: 0x06000061 RID: 97 RVA: 0x000037BD File Offset: 0x000019BD
			// (set) Token: 0x06000062 RID: 98 RVA: 0x000037C5 File Offset: 0x000019C5
			public string PartitionId
			{
				get
				{
					return this._id;
				}
				set
				{
					this._id = value;
				}
			}

			// Token: 0x17000031 RID: 49
			// (get) Token: 0x06000063 RID: 99 RVA: 0x000037CE File Offset: 0x000019CE
			// (set) Token: 0x06000064 RID: 100 RVA: 0x000037D6 File Offset: 0x000019D6
			public bool Bootable { get; set; }

			// Token: 0x17000032 RID: 50
			// (get) Token: 0x06000065 RID: 101 RVA: 0x000037DF File Offset: 0x000019DF
			// (set) Token: 0x06000066 RID: 102 RVA: 0x000037E7 File Offset: 0x000019E7
			public bool ReadOnly { get; set; }

			// Token: 0x17000033 RID: 51
			// (get) Token: 0x06000067 RID: 103 RVA: 0x000037F0 File Offset: 0x000019F0
			// (set) Token: 0x06000068 RID: 104 RVA: 0x000037F8 File Offset: 0x000019F8
			public bool Hidden { get; set; }

			// Token: 0x17000034 RID: 52
			// (get) Token: 0x06000069 RID: 105 RVA: 0x00003801 File Offset: 0x00001A01
			// (set) Token: 0x0600006A RID: 106 RVA: 0x00003809 File Offset: 0x00001A09
			public bool AttachDriveLetter { get; set; }

			// Token: 0x17000035 RID: 53
			// (get) Token: 0x0600006B RID: 107 RVA: 0x00003812 File Offset: 0x00001A12
			// (set) Token: 0x0600006C RID: 108 RVA: 0x0000381A File Offset: 0x00001A1A
			public string PrimaryPartition { get; set; }

			// Token: 0x17000036 RID: 54
			// (get) Token: 0x0600006D RID: 109 RVA: 0x00003823 File Offset: 0x00001A23
			public bool Contiguous
			{
				get
				{
					return true;
				}
			}

			// Token: 0x17000037 RID: 55
			// (get) Token: 0x0600006E RID: 110 RVA: 0x00003826 File Offset: 0x00001A26
			// (set) Token: 0x0600006F RID: 111 RVA: 0x0000382E File Offset: 0x00001A2E
			public string FileSystem
			{
				get
				{
					return this._fileSystem;
				}
				set
				{
					this._fileSystem = value;
				}
			}

			// Token: 0x17000038 RID: 56
			// (get) Token: 0x06000070 RID: 112 RVA: 0x00003837 File Offset: 0x00001A37
			// (set) Token: 0x06000071 RID: 113 RVA: 0x0000383F File Offset: 0x00001A3F
			[CLSCompliant(false)]
			public uint ByteAlignment
			{
				get
				{
					return this._byteAlignment;
				}
				set
				{
					if (InputHelpers.IsPowerOfTwo(value))
					{
						this._byteAlignment = value;
					}
				}
			}

			// Token: 0x17000039 RID: 57
			// (get) Token: 0x06000072 RID: 114 RVA: 0x00003850 File Offset: 0x00001A50
			// (set) Token: 0x06000073 RID: 115 RVA: 0x00003858 File Offset: 0x00001A58
			[CLSCompliant(false)]
			public uint ClusterSize
			{
				get
				{
					return this._clusterSize;
				}
				set
				{
					if (InputHelpers.IsPowerOfTwo(value))
					{
						this._clusterSize = value;
					}
				}
			}

			// Token: 0x1700003A RID: 58
			// (get) Token: 0x06000074 RID: 116 RVA: 0x00003869 File Offset: 0x00001A69
			[CLSCompliant(false)]
			public uint LastUsedSector
			{
				get
				{
					if (this._sectorsInUse > 0U)
					{
						return this._sectorsInUse - 1U;
					}
					return 0U;
				}
			}

			// Token: 0x1700003B RID: 59
			// (get) Token: 0x06000075 RID: 117 RVA: 0x0000387E File Offset: 0x00001A7E
			// (set) Token: 0x06000076 RID: 118 RVA: 0x00003886 File Offset: 0x00001A86
			[CLSCompliant(false)]
			public uint SectorsInUse
			{
				get
				{
					return this._sectorsInUse;
				}
				set
				{
					this._sectorsInUse = value;
				}
			}

			// Token: 0x1700003C RID: 60
			// (get) Token: 0x06000077 RID: 119 RVA: 0x0000388F File Offset: 0x00001A8F
			// (set) Token: 0x06000078 RID: 120 RVA: 0x00003897 File Offset: 0x00001A97
			public bool UseAllSpace
			{
				get
				{
					return this._useAllSpace;
				}
				set
				{
					this._useAllSpace = value;
				}
			}

			// Token: 0x1700003D RID: 61
			// (get) Token: 0x06000079 RID: 121 RVA: 0x000038A0 File Offset: 0x00001AA0
			// (set) Token: 0x0600007A RID: 122 RVA: 0x000038A8 File Offset: 0x00001AA8
			public bool RequiredToFlash { get; set; }

			// Token: 0x1700003E RID: 62
			// (get) Token: 0x0600007B RID: 123 RVA: 0x000038B1 File Offset: 0x00001AB1
			// (set) Token: 0x0600007C RID: 124 RVA: 0x000038B9 File Offset: 0x00001AB9
			[CLSCompliant(false)]
			public uint SectorAlignment { get; set; }

			// Token: 0x04000017 RID: 23
			private uint _sectorsInUse;

			// Token: 0x04000018 RID: 24
			private uint _totalSectors;

			// Token: 0x04000019 RID: 25
			private string _type;

			// Token: 0x0400001A RID: 26
			private string _id;

			// Token: 0x0400001B RID: 27
			private string _name;

			// Token: 0x0400001C RID: 28
			private FullFlashUpdateImage.FullFlashUpdateStore _store;

			// Token: 0x0400001D RID: 29
			private bool _useAllSpace;

			// Token: 0x0400001E RID: 30
			private string _fileSystem;

			// Token: 0x0400001F RID: 31
			private uint _byteAlignment;

			// Token: 0x04000020 RID: 32
			private uint _clusterSize;
		}

		// Token: 0x02000008 RID: 8
		public class FullFlashUpdateStore : IDisposable
		{
			// Token: 0x0600007E RID: 126 RVA: 0x000038CC File Offset: 0x00001ACC
			~FullFlashUpdateStore()
			{
				this.Dispose(false);
			}

			// Token: 0x0600007F RID: 127 RVA: 0x000038FC File Offset: 0x00001AFC
			public void Dispose()
			{
				this.Dispose(true);
				GC.SuppressFinalize(this);
			}

			// Token: 0x06000080 RID: 128 RVA: 0x0000390C File Offset: 0x00001B0C
			protected virtual void Dispose(bool isDisposing)
			{
				if (this._alreadyDisposed)
				{
					return;
				}
				if (isDisposing)
				{
					this._partitions = null;
				}
				if (File.Exists(this._tempBackingStoreFile))
				{
					try
					{
						File.Delete(this._tempBackingStoreFile);
					}
					catch (Exception ex)
					{
						Console.WriteLine("Warning: ImageCommon!Dispose: Failed to delete temporary backing store '" + this._tempBackingStoreFile + "' with exception: " + ex.Message);
					}
				}
				if (Directory.Exists(this._tempBackingStorePath))
				{
					try
					{
						Directory.Delete(this._tempBackingStorePath, true);
					}
					catch (Exception ex2)
					{
						Console.WriteLine("Warning: ImageCommon!Dispose: Failed to delete temporary backing store directory '" + this._tempBackingStorePath + "' with exception: " + ex2.Message);
					}
				}
				this._alreadyDisposed = true;
			}

			// Token: 0x1700003F RID: 63
			// (get) Token: 0x06000081 RID: 129 RVA: 0x000039CC File Offset: 0x00001BCC
			public FullFlashUpdateImage Image
			{
				get
				{
					return this._image;
				}
			}

			// Token: 0x17000040 RID: 64
			// (get) Token: 0x06000082 RID: 130 RVA: 0x000039D4 File Offset: 0x00001BD4
			public string Id
			{
				get
				{
					return this._storeId;
				}
			}

			// Token: 0x17000041 RID: 65
			// (get) Token: 0x06000083 RID: 131 RVA: 0x000039DC File Offset: 0x00001BDC
			public bool IsMainOSStore
			{
				get
				{
					return this._isMainOSStore;
				}
			}

			// Token: 0x17000042 RID: 66
			// (get) Token: 0x06000084 RID: 132 RVA: 0x000039E4 File Offset: 0x00001BE4
			public string DevicePath
			{
				get
				{
					return this._devicePath;
				}
			}

			// Token: 0x17000043 RID: 67
			// (get) Token: 0x06000085 RID: 133 RVA: 0x000039EC File Offset: 0x00001BEC
			public bool OnlyAllocateDefinedGptEntries
			{
				get
				{
					return this._onlyAllocateDefinedGptEntries;
				}
			}

			// Token: 0x17000044 RID: 68
			// (get) Token: 0x06000086 RID: 134 RVA: 0x000039F4 File Offset: 0x00001BF4
			// (set) Token: 0x06000087 RID: 135 RVA: 0x000039FC File Offset: 0x00001BFC
			[CLSCompliant(false)]
			public uint SectorCount
			{
				get
				{
					return this._minSectorCount;
				}
				set
				{
					this._minSectorCount = value;
				}
			}

			// Token: 0x17000045 RID: 69
			// (get) Token: 0x06000088 RID: 136 RVA: 0x00003A05 File Offset: 0x00001C05
			// (set) Token: 0x06000089 RID: 137 RVA: 0x00003A0D File Offset: 0x00001C0D
			[CLSCompliant(false)]
			public uint MinSectorCount
			{
				get
				{
					return this._minSectorCount;
				}
				set
				{
					this._minSectorCount = value;
				}
			}

			// Token: 0x17000046 RID: 70
			// (get) Token: 0x0600008A RID: 138 RVA: 0x00003A16 File Offset: 0x00001C16
			// (set) Token: 0x0600008B RID: 139 RVA: 0x00003A1E File Offset: 0x00001C1E
			[CLSCompliant(false)]
			public uint SectorSize
			{
				get
				{
					return this._sectorSize;
				}
				set
				{
					this._sectorSize = value;
				}
			}

			// Token: 0x17000047 RID: 71
			// (get) Token: 0x0600008C RID: 140 RVA: 0x00003A27 File Offset: 0x00001C27
			public int PartitionCount
			{
				get
				{
					return this._partitions.Count;
				}
			}

			// Token: 0x17000048 RID: 72
			// (get) Token: 0x0600008D RID: 141 RVA: 0x00003A34 File Offset: 0x00001C34
			public List<FullFlashUpdateImage.FullFlashUpdatePartition> Partitions
			{
				get
				{
					return this._partitions;
				}
			}

			// Token: 0x17000049 RID: 73
			public FullFlashUpdateImage.FullFlashUpdatePartition this[string name]
			{
				get
				{
					foreach (FullFlashUpdateImage.FullFlashUpdatePartition fullFlashUpdatePartition in this._partitions)
					{
						if (string.CompareOrdinal(fullFlashUpdatePartition.Name, name) == 0)
						{
							return fullFlashUpdatePartition;
						}
					}
					return null;
				}
			}

			// Token: 0x0600008F RID: 143 RVA: 0x00003AA0 File Offset: 0x00001CA0
			[CLSCompliant(false)]
			public void Initialize(FullFlashUpdateImage image, string storeId, bool isMainOSStore, string devicePath, bool onlyAllocateDefinedGptEntries, uint minSectorCount, uint sectorSize)
			{
				this._tempBackingStorePath = BuildPaths.GetImagingTempPath(Directory.GetCurrentDirectory());
				Directory.CreateDirectory(this._tempBackingStorePath);
				this._tempBackingStoreFile = FileUtils.GetTempFile(this._tempBackingStorePath) + "FFUBackingStore";
				this._image = image;
				this._storeId = storeId;
				this._isMainOSStore = isMainOSStore;
				this._devicePath = devicePath;
				this._onlyAllocateDefinedGptEntries = onlyAllocateDefinedGptEntries;
				this._minSectorCount = minSectorCount;
				this._sectorSize = sectorSize;
				this._sectorsUsed = 0U;
			}

			// Token: 0x06000090 RID: 144 RVA: 0x00003B20 File Offset: 0x00001D20
			public void AddPartition(FullFlashUpdateImage.FullFlashUpdatePartition partition)
			{
				if (this[partition.Name] != null)
				{
					throw new ImageCommonException("ImageCommon!FullFlashUpdateStore::AddPartition: Two partitions in a store have the same name (" + partition.Name + ").");
				}
				if (this._isMainOSStore)
				{
					if (this._minSectorCount != 0U && partition.TotalSectors > this._minSectorCount)
					{
						throw new ImageCommonException("ImageCommon!FullFlashUpdateStore::AddPartition: The partition " + partition.Name + " is too large for the store.");
					}
					if (partition.UseAllSpace)
					{
						using (List<FullFlashUpdateImage.FullFlashUpdatePartition>.Enumerator enumerator = this._partitions.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								FullFlashUpdateImage.FullFlashUpdatePartition fullFlashUpdatePartition = enumerator.Current;
								if (fullFlashUpdatePartition.UseAllSpace)
								{
									throw new ImageCommonException("ImageCommon!FullFlashUpdateStore::AddPartition: Two partitions in the same store have the UseAllSpace flag set.");
								}
							}
							goto IL_10D;
						}
					}
					if (partition.SectorsInUse > partition.TotalSectors)
					{
						throw new ImageCommonException(string.Concat(new object[]
						{
							"ImageCommon!FullFlashUpdateStore::AddPartition: The partition data is invalid.  There are more used sectors (",
							partition.SectorsInUse,
							") than total sectors (",
							partition.TotalSectors,
							") for partition:",
							partition.Name
						}));
					}
					IL_10D:
					if (this._minSectorCount != 0U)
					{
						if (partition.UseAllSpace)
						{
							this._sectorsUsed += 1U;
						}
						else
						{
							this._sectorsUsed += partition.TotalSectors;
						}
						if (this._sectorsUsed > this._minSectorCount)
						{
							throw new ImageCommonException(string.Concat(new object[]
							{
								"ImageCommon!FullFlashUpdateStore::AddPartition: Partition (",
								partition.Name,
								") on the Store does not fit. SectorsUsed = ",
								this._sectorsUsed,
								" > MinSectorCount = ",
								this._minSectorCount
							}));
						}
					}
				}
				this._partitions.Add(partition);
			}

			// Token: 0x06000091 RID: 145 RVA: 0x00003CEC File Offset: 0x00001EEC
			internal void AddPartition(FullFlashUpdateImage.ManifestCategory category)
			{
				uint usedSectors = 0U;
				uint num = 0U;
				string partitionType = category["Type"];
				string text = category["Name"];
				string partitionId = category["Id"];
				bool flag = false;
				if (this._isMainOSStore)
				{
					if (category["UsedSectors"] != null)
					{
						usedSectors = uint.Parse(category["UsedSectors"], CultureInfo.InvariantCulture);
					}
					if (category["TotalSectors"] != null)
					{
						num = uint.Parse(category["TotalSectors"], CultureInfo.InvariantCulture);
					}
					if (category["UseAllSpace"] != null)
					{
						flag = bool.Parse(category["UseAllSpace"]);
					}
					if (!flag && num == 0U)
					{
						throw new ImageCommonException(string.Format("ImageCommon!FullFlashUpdateImage::AddPartition: The partition category for partition {0} must contain either a 'TotalSectors' or 'UseAllSpace' key/value pair.", text));
					}
					if (flag && num > 0U)
					{
						throw new ImageCommonException(string.Format("ImageCommon!FullFlashUpdateImage::AddPartition: The partition category for partition {0} cannot contain both a 'TotalSectors' and a 'UseAllSpace' key/value pair.", text));
					}
				}
				FullFlashUpdateImage.FullFlashUpdatePartition fullFlashUpdatePartition = new FullFlashUpdateImage.FullFlashUpdatePartition();
				fullFlashUpdatePartition.Initialize(usedSectors, num, partitionType, partitionId, text, this, flag);
				if (category["Hidden"] != null)
				{
					fullFlashUpdatePartition.Hidden = bool.Parse(category["Hidden"]);
				}
				if (category["AttachDriveLetter"] != null)
				{
					fullFlashUpdatePartition.AttachDriveLetter = bool.Parse(category["AttachDriveLetter"]);
				}
				if (category["ReadOnly"] != null)
				{
					fullFlashUpdatePartition.ReadOnly = bool.Parse(category["ReadOnly"]);
				}
				if (category["Bootable"] != null)
				{
					fullFlashUpdatePartition.Bootable = bool.Parse(category["Bootable"]);
				}
				if (category["FileSystem"] != null)
				{
					fullFlashUpdatePartition.FileSystem = category["FileSystem"];
				}
				fullFlashUpdatePartition.PrimaryPartition = category["Primary"];
				if (category["ByteAlignment"] != null)
				{
					fullFlashUpdatePartition.ByteAlignment = uint.Parse(category["ByteAlignment"], CultureInfo.InvariantCulture);
				}
				if (category["ClusterSize"] != null)
				{
					fullFlashUpdatePartition.ClusterSize = uint.Parse(category["ClusterSize"], CultureInfo.InvariantCulture);
				}
				if (category["SectorAlignment"] != null)
				{
					fullFlashUpdatePartition.SectorAlignment = uint.Parse(category["SectorAlignment"], CultureInfo.InvariantCulture);
				}
				if (category["RequiredToFlash"] != null)
				{
					fullFlashUpdatePartition.RequiredToFlash = bool.Parse(category["RequiredToFlash"]);
				}
				this.AddPartition(fullFlashUpdatePartition);
			}

			// Token: 0x06000092 RID: 146 RVA: 0x00003F48 File Offset: 0x00002148
			public void TransferLocation(Stream sourceStream, Stream destinationStream)
			{
				byte[] array = new byte[1048576];
				int num2;
				for (long num = sourceStream.Length - sourceStream.Position; num > 0L; num -= (long)num2)
				{
					num2 = (int)Math.Min(num, (long)array.Length);
					sourceStream.Read(array, 0, num2);
					destinationStream.Write(array, 0, num2);
				}
			}

			// Token: 0x06000093 RID: 147 RVA: 0x00003F9C File Offset: 0x0000219C
			public void MoveToTempBackingStore(FileStream imageStream)
			{
				FileStream fileStream = null;
				if (File.Exists(this._tempBackingStoreFile))
				{
					return;
				}
				try
				{
					fileStream = new FileStream(this._tempBackingStoreFile, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
					this.TransferLocation(imageStream, fileStream);
				}
				finally
				{
					if (fileStream != null)
					{
						fileStream.Close();
						fileStream = null;
					}
				}
			}

			// Token: 0x06000094 RID: 148 RVA: 0x00003FF0 File Offset: 0x000021F0
			public void ToCategory(FullFlashUpdateImage.ManifestCategory category)
			{
				category["SectorSize"] = this._sectorSize.ToString(CultureInfo.InvariantCulture);
				if (this._minSectorCount != 0U)
				{
					category["MinSectorCount"] = this._minSectorCount.ToString(CultureInfo.InvariantCulture);
				}
				if (!string.IsNullOrEmpty(this._storeId))
				{
					category["StoreId"] = this._storeId;
				}
				category["IsMainOSStore"] = this._isMainOSStore.ToString(CultureInfo.InvariantCulture);
				if (!string.IsNullOrEmpty(this._devicePath))
				{
					category["DevicePath"] = this._devicePath;
				}
				category["OnlyAllocateDefinedGptEntries"] = this._onlyAllocateDefinedGptEntries.ToString(CultureInfo.InvariantCulture);
			}

			// Token: 0x1700004A RID: 74
			// (get) Token: 0x06000095 RID: 149 RVA: 0x000040AD File Offset: 0x000022AD
			public string BackingFile
			{
				get
				{
					return this._tempBackingStoreFile;
				}
			}

			// Token: 0x04000028 RID: 40
			private List<FullFlashUpdateImage.FullFlashUpdatePartition> _partitions = new List<FullFlashUpdateImage.FullFlashUpdatePartition>();

			// Token: 0x04000029 RID: 41
			private FullFlashUpdateImage _image;

			// Token: 0x0400002A RID: 42
			private string _storeId;

			// Token: 0x0400002B RID: 43
			private bool _isMainOSStore;

			// Token: 0x0400002C RID: 44
			private string _devicePath;

			// Token: 0x0400002D RID: 45
			private bool _onlyAllocateDefinedGptEntries;

			// Token: 0x0400002E RID: 46
			private uint _minSectorCount;

			// Token: 0x0400002F RID: 47
			private uint _sectorSize;

			// Token: 0x04000030 RID: 48
			private uint _sectorsUsed;

			// Token: 0x04000031 RID: 49
			private string _tempBackingStoreFile = string.Empty;

			// Token: 0x04000032 RID: 50
			private string _tempBackingStorePath = string.Empty;

			// Token: 0x04000033 RID: 51
			private bool _alreadyDisposed;
		}

		// Token: 0x02000009 RID: 9
		public class ManifestCategory
		{
			// Token: 0x1700004B RID: 75
			public string this[string name]
			{
				get
				{
					return (string)this._keyValues[name];
				}
				set
				{
					if (this._keyValues.ContainsKey(name))
					{
						this._keyValues[name] = value;
						return;
					}
					if (name.Length > this._maxKeySize)
					{
						this._maxKeySize = name.Length;
					}
					this._keyValues.Add(name, value);
				}
			}

			// Token: 0x1700004C RID: 76
			// (get) Token: 0x06000099 RID: 153 RVA: 0x00004144 File Offset: 0x00002344
			// (set) Token: 0x0600009A RID: 154 RVA: 0x0000414C File Offset: 0x0000234C
			public string Category
			{
				get
				{
					return this._category;
				}
				set
				{
					this._category = value;
				}
			}

			// Token: 0x1700004D RID: 77
			// (get) Token: 0x0600009B RID: 155 RVA: 0x00004155 File Offset: 0x00002355
			public string Name
			{
				get
				{
					return this._name;
				}
			}

			// Token: 0x0600009C RID: 156 RVA: 0x0000415D File Offset: 0x0000235D
			public void RemoveNameValue(string name)
			{
				if (this._keyValues.ContainsKey(name))
				{
					this._keyValues.Remove(name);
				}
			}

			// Token: 0x0600009D RID: 157 RVA: 0x00004179 File Offset: 0x00002379
			public ManifestCategory(string name)
			{
				this._name = name;
			}

			// Token: 0x0600009E RID: 158 RVA: 0x000041A9 File Offset: 0x000023A9
			public ManifestCategory(string name, string categoryValue)
			{
				this._name = name;
				this._category = categoryValue;
			}

			// Token: 0x0600009F RID: 159 RVA: 0x000041E0 File Offset: 0x000023E0
			public void WriteToStream(Stream targetStream)
			{
				ASCIIEncoding asciiencoding = new ASCIIEncoding();
				byte[] bytes = asciiencoding.GetBytes("[" + this._category + "]\r\n");
				targetStream.Write(bytes, 0, bytes.Count<byte>());
				foreach (object obj in this._keyValues)
				{
					string text = ((DictionaryEntry)obj).Key as string;
					bytes = asciiencoding.GetBytes(text);
					targetStream.Write(bytes, 0, bytes.Count<byte>());
					for (int i = 0; i < this._maxKeySize + 1 - text.Length; i++)
					{
						targetStream.Write(asciiencoding.GetBytes(" "), 0, 1);
					}
					bytes = asciiencoding.GetBytes("= " + this._keyValues[text] + "\r\n");
					targetStream.Write(bytes, 0, bytes.Count<byte>());
				}
				targetStream.Write(asciiencoding.GetBytes("\r\n"), 0, 2);
			}

			// Token: 0x060000A0 RID: 160 RVA: 0x00004308 File Offset: 0x00002508
			public void WriteToFile(TextWriter targetStream)
			{
				targetStream.WriteLine("[{0}]", this._category);
				foreach (object obj in this._keyValues)
				{
					string text = ((DictionaryEntry)obj).Key as string;
					targetStream.Write("{0}", text);
					for (int i = 0; i < this._maxKeySize + 1 - text.Length; i++)
					{
						targetStream.Write(" ");
					}
					targetStream.WriteLine("= {0}", this._keyValues[text]);
				}
				targetStream.WriteLine("");
			}

			// Token: 0x060000A1 RID: 161 RVA: 0x000043D0 File Offset: 0x000025D0
			public void Clean()
			{
				this._keyValues.Clear();
			}

			// Token: 0x04000034 RID: 52
			private string _name = string.Empty;

			// Token: 0x04000035 RID: 53
			private string _category = string.Empty;

			// Token: 0x04000036 RID: 54
			private int _maxKeySize;

			// Token: 0x04000037 RID: 55
			private Hashtable _keyValues = new Hashtable();
		}

		// Token: 0x0200000A RID: 10
		public class FullFlashUpdateManifest
		{
			// Token: 0x060000A2 RID: 162 RVA: 0x000043DD File Offset: 0x000025DD
			public FullFlashUpdateManifest(FullFlashUpdateImage image)
			{
				this._image = image;
			}

			// Token: 0x060000A3 RID: 163 RVA: 0x000043FC File Offset: 0x000025FC
			public FullFlashUpdateManifest(FullFlashUpdateImage image, StreamReader manifestStream)
			{
				Regex regex = new Regex("^\\s*\\[(?<category>[^\\]]+)\\]\\s*$");
				Regex regex2 = new Regex("^\\s*(?<key>[^=\\s]+)\\s*=\\s*(?<value>.*)(\\s*$)");
				Match match = null;
				this._image = image;
				FullFlashUpdateImage.ManifestCategory manifestCategory = null;
				while (!manifestStream.EndOfStream)
				{
					string text = manifestStream.ReadLine();
					if (regex.IsMatch(text))
					{
						match = null;
						Match match2 = regex.Match(text);
						string value = match2.Groups["category"].Value;
						this.ProcessCategory(manifestCategory);
						if (string.Compare(value, "Store", StringComparison.Ordinal) == 0)
						{
							manifestCategory = new FullFlashUpdateImage.ManifestCategory("Store", "Store");
						}
						else if (string.Compare(value, "Partition", StringComparison.Ordinal) == 0)
						{
							manifestCategory = new FullFlashUpdateImage.ManifestCategory("Partition", "Partition");
						}
						else
						{
							manifestCategory = this.AddCategory(value, value);
						}
					}
					else if (manifestCategory != null && regex2.IsMatch(text))
					{
						match = null;
						Match match3 = regex2.Match(text);
						manifestCategory[match3.Groups["key"].Value] = match3.Groups["value"].Value;
						if (match3.Groups["key"].ToString() == "Description")
						{
							match = match3;
						}
					}
					else if (match != null)
					{
						FullFlashUpdateImage.ManifestCategory manifestCategory2;
						string value2;
						(manifestCategory2 = manifestCategory)[value2 = match.Groups["key"].Value] = manifestCategory2[value2] + Environment.NewLine + text;
					}
				}
				this.ProcessCategory(manifestCategory);
			}

			// Token: 0x060000A4 RID: 164 RVA: 0x0000459C File Offset: 0x0000279C
			private void ProcessCategory(FullFlashUpdateImage.ManifestCategory category)
			{
				if (category != null)
				{
					if (string.CompareOrdinal(category.Name, "Store") == 0)
					{
						this._image.AddStore(category);
						category = null;
						return;
					}
					if (string.CompareOrdinal(category.Name, "Partition") == 0)
					{
						this._image.Stores.Last<FullFlashUpdateImage.FullFlashUpdateStore>().AddPartition(category);
						category = null;
					}
				}
			}

			// Token: 0x060000A5 RID: 165 RVA: 0x000045FC File Offset: 0x000027FC
			public FullFlashUpdateImage.ManifestCategory AddCategory(string name, string categoryValue)
			{
				if (this[name] != null)
				{
					throw new ImageCommonException("ImageCommon!FullFlashUpdateManifest::AddCategory: Cannot add duplicate categories to a manifest.");
				}
				FullFlashUpdateImage.ManifestCategory manifestCategory = new FullFlashUpdateImage.ManifestCategory(name, categoryValue);
				this._categories.Add(manifestCategory);
				return manifestCategory;
			}

			// Token: 0x060000A6 RID: 166 RVA: 0x00004634 File Offset: 0x00002834
			public void RemoveCategory(string name)
			{
				if (this[name] == null)
				{
					return;
				}
				FullFlashUpdateImage.ManifestCategory obj = this[name];
				this._categories.Remove(obj);
			}

			// Token: 0x1700004E RID: 78
			public FullFlashUpdateImage.ManifestCategory this[string categoryName]
			{
				get
				{
					foreach (object obj in this._categories)
					{
						FullFlashUpdateImage.ManifestCategory manifestCategory = (FullFlashUpdateImage.ManifestCategory)obj;
						if (string.Compare(manifestCategory.Name, categoryName, StringComparison.Ordinal) == 0)
						{
							return manifestCategory;
						}
					}
					return null;
				}
			}

			// Token: 0x060000A8 RID: 168 RVA: 0x000046C8 File Offset: 0x000028C8
			public void WriteToStream(Stream targetStream)
			{
				foreach (object obj in this._categories)
				{
					FullFlashUpdateImage.ManifestCategory manifestCategory = (FullFlashUpdateImage.ManifestCategory)obj;
					manifestCategory.WriteToStream(targetStream);
				}
				foreach (FullFlashUpdateImage.FullFlashUpdateStore fullFlashUpdateStore in this._image.Stores)
				{
					FullFlashUpdateImage.ManifestCategory manifestCategory2 = new FullFlashUpdateImage.ManifestCategory("Store", "Store");
					fullFlashUpdateStore.ToCategory(manifestCategory2);
					manifestCategory2.WriteToStream(targetStream);
					foreach (FullFlashUpdateImage.FullFlashUpdatePartition fullFlashUpdatePartition in fullFlashUpdateStore.Partitions)
					{
						FullFlashUpdateImage.ManifestCategory manifestCategory3 = new FullFlashUpdateImage.ManifestCategory("Partition", "Partition");
						fullFlashUpdatePartition.ToCategory(manifestCategory3);
						manifestCategory3.WriteToStream(targetStream);
					}
				}
			}

			// Token: 0x060000A9 RID: 169 RVA: 0x000047E4 File Offset: 0x000029E4
			public void WriteToFile(string fileName)
			{
				try
				{
					if (File.Exists(fileName))
					{
						File.Delete(fileName);
					}
				}
				catch (Exception innerException)
				{
					throw new ImageCommonException("ImageCommon!FullFlashUpdateManifest::WriteToFile: Unable to delete the existing image file", innerException);
				}
				StreamWriter streamWriter = File.CreateText(fileName);
				this.WriteToStream(streamWriter.BaseStream);
				streamWriter.Close();
			}

			// Token: 0x1700004F RID: 79
			// (get) Token: 0x060000AA RID: 170 RVA: 0x00004838 File Offset: 0x00002A38
			[CLSCompliant(false)]
			public uint Length
			{
				get
				{
					MemoryStream memoryStream = new MemoryStream();
					this.WriteToStream(memoryStream);
					return (uint)memoryStream.Position;
				}
			}

			// Token: 0x04000038 RID: 56
			private ArrayList _categories = new ArrayList(20);

			// Token: 0x04000039 RID: 57
			private FullFlashUpdateImage _image;
		}
	}
}
