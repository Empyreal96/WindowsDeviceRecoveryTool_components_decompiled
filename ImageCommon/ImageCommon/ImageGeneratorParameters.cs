using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using Microsoft.WindowsPhone.ImageUpdate.Tools.Common;

namespace Microsoft.WindowsPhone.Imaging
{
	// Token: 0x02000013 RID: 19
	public class ImageGeneratorParameters
	{
		// Token: 0x06000111 RID: 273 RVA: 0x000050E2 File Offset: 0x000032E2
		public ImageGeneratorParameters()
		{
			this.Stores = new List<InputStore>();
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00005112 File Offset: 0x00003312
		public void Initialize(IULogger logger)
		{
			if (logger == null)
			{
				this._logger = new IULogger();
				return;
			}
			this._logger = logger;
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000113 RID: 275 RVA: 0x0000512A File Offset: 0x0000332A
		// (set) Token: 0x06000114 RID: 276 RVA: 0x00005132 File Offset: 0x00003332
		public string Description { get; set; }

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000115 RID: 277 RVA: 0x0000513B File Offset: 0x0000333B
		// (set) Token: 0x06000116 RID: 278 RVA: 0x00005143 File Offset: 0x00003343
		public List<InputStore> Stores { get; set; }

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000117 RID: 279 RVA: 0x00005154 File Offset: 0x00003354
		public InputStore MainOSStore
		{
			get
			{
				return this.Stores.FirstOrDefault((InputStore x) => x.IsMainOSStore());
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000118 RID: 280 RVA: 0x0000517E File Offset: 0x0000337E
		// (set) Token: 0x06000119 RID: 281 RVA: 0x00005186 File Offset: 0x00003386
		[CLSCompliant(false)]
		public uint ChunkSize
		{
			get
			{
				return this._chunkSize;
			}
			set
			{
				this._chunkSize = ((value != 0U) ? value : 256U);
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x0600011A RID: 282 RVA: 0x00005199 File Offset: 0x00003399
		// (set) Token: 0x0600011B RID: 283 RVA: 0x000051A1 File Offset: 0x000033A1
		[CLSCompliant(false)]
		public uint DefaultPartitionByteAlignment { get; set; }

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x0600011C RID: 284 RVA: 0x000051AA File Offset: 0x000033AA
		// (set) Token: 0x0600011D RID: 285 RVA: 0x000051B2 File Offset: 0x000033B2
		[CLSCompliant(false)]
		public uint VirtualHardDiskSectorSize { get; set; }

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x0600011E RID: 286 RVA: 0x000051BB File Offset: 0x000033BB
		// (set) Token: 0x0600011F RID: 287 RVA: 0x000051C3 File Offset: 0x000033C3
		[CLSCompliant(false)]
		public uint SectorSize { get; set; }

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000120 RID: 288 RVA: 0x000051CC File Offset: 0x000033CC
		// (set) Token: 0x06000121 RID: 289 RVA: 0x000051D4 File Offset: 0x000033D4
		[CLSCompliant(false)]
		public uint MinSectorCount { get; set; }

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000122 RID: 290 RVA: 0x000051DD File Offset: 0x000033DD
		// (set) Token: 0x06000123 RID: 291 RVA: 0x000051E5 File Offset: 0x000033E5
		[CLSCompliant(false)]
		public uint Algid
		{
			get
			{
				return this._algid;
			}
			set
			{
				this._algid = ((value != 0U) ? value : 32780U);
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000124 RID: 292 RVA: 0x000051F8 File Offset: 0x000033F8
		public uint DeviceLayoutVersion
		{
			get
			{
				return this._deviceLayoutVersion;
			}
		}

		// Token: 0x06000125 RID: 293 RVA: 0x00005200 File Offset: 0x00003400
		private bool VerifyPartitionSizes()
		{
			uint num = 0U;
			if (this.Stores == null)
			{
				return true;
			}
			foreach (InputPartition inputPartition in this.MainOSStore.Partitions)
			{
				if (inputPartition.UseAllSpace)
				{
					num += 1U;
				}
				else
				{
					num += inputPartition.TotalSectors;
				}
			}
			if (num > this.MinSectorCount)
			{
				ulong num2 = (ulong)num * (ulong)this.SectorSize / 1024UL / 1024UL;
				ulong num3 = (ulong)this.MinSectorCount * (ulong)this.SectorSize / 1024UL / 1024UL;
				this._logger.LogError(string.Format("ImageCommon!ImageGeneratorParameters::VerifyPartitionSizes: The total sectors used by all the partitions ({0} sectors/{1} MB) is larger than the MinSectorCount ({2} sectors/{3} MB). This means the image would not flash to a device with only {4} sectors/{5} MB. Either remove image content, or increase MinSectorCount.", new object[]
				{
					num,
					num2,
					this.MinSectorCount,
					num3,
					this.MinSectorCount,
					num3
				}), new object[0]);
				return false;
			}
			return true;
		}

		// Token: 0x06000126 RID: 294 RVA: 0x00005310 File Offset: 0x00003510
		public bool VerifyInputParameters()
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			if (4294967295U / this.ChunkSize < 1024U)
			{
				this._logger.LogError("ImageCommon!ImageGeneratorParameters::VerifyInputParameters: The chunk size is specified in Kilobytes and the total size must be under 4GB.", new object[0]);
				return false;
			}
			if (this.SectorSize < 512U)
			{
				this._logger.LogError("ImageCommon!ImageGeneratorParameters::VerifyInputParameters: The sector size must be at least 512 bytes: {0} bytes.", new object[]
				{
					this.SectorSize
				});
				return false;
			}
			if (!InputHelpers.IsPowerOfTwo(this.SectorSize))
			{
				this._logger.LogError("ImageCommon!ImageGeneratorParameters::VerifyInputParameters: The sector size must be a multiple of 2: {0} bytes.", new object[]
				{
					this.SectorSize
				});
				return false;
			}
			if (this.ChunkSize * 1024U < this.SectorSize)
			{
				this._logger.LogError("ImageCommon!ImageGeneratorParameters::VerifyInputParameters: The chunk size is specified in Kilobytes and the total size must be under larger the sector size: {0} bytes.", new object[]
				{
					this.SectorSize
				});
				return false;
			}
			if (this.ChunkSize * 1024U % this.SectorSize != 0U)
			{
				this._logger.LogError("ImageCommon!ImageGeneratorParameters::VerifyInputParameters: The chunk size is specified in Kilobytes and must be divisible by the sector size: {0}.", new object[]
				{
					this.SectorSize
				});
				return false;
			}
			if (this.DefaultPartitionByteAlignment != 0U && !InputHelpers.IsPowerOfTwo(this.DefaultPartitionByteAlignment))
			{
				this._logger.LogError("ImageCommon!ImageGeneratorParameters::VerifyInputParameters: The default partition byte alignment must be a multiple of 2: {0} bytes.", new object[]
				{
					this.DefaultPartitionByteAlignment
				});
				return false;
			}
			if (this.Stores == null || this.Stores.Count == 0)
			{
				this._logger.LogError("ImageCommon!ImageGeneratorParameters::VerifyInputParameters: For Generating a FFU image, at least one store must be specified.", new object[0]);
				return false;
			}
			if (this.Stores.Count((InputStore x) => x.IsMainOSStore()) != 1)
			{
				this._logger.LogError("ImageCommon!ImageGeneratorParameters::VerifyInputParameters: For Generating a FFU image, one and only one of the stores must be MainOS.", new object[0]);
				return false;
			}
			if (this.MainOSStore.Partitions == null || this.MainOSStore.Partitions.Count<InputPartition>() == 0)
			{
				this._logger.LogError("ImageCommon!ImageGeneratorParameters::VerifyInputParameters: For Generating a FFU image, at least one partition must be specified.", new object[0]);
				return false;
			}
			if (this.SectorSize == 0U)
			{
				this._logger.LogError("ImageCommon!ImageGeneratorParameters::VerifyInputParameters: The SectorSize cannot be 0. Please provide a valid SectorSize.", new object[0]);
				return false;
			}
			if (this.ChunkSize == 0U)
			{
				this._logger.LogError("ImageCommon!ImageGeneratorParameters::VerifyInputParameters: The ChunkSize cannot be 0. Please provide a valid ChunkSize between 1-1024.", new object[0]);
				return false;
			}
			if (this.ChunkSize < 1U || this.ChunkSize > 1024U)
			{
				this._logger.LogError("ImageCommon!ImageGeneratorParameters::VerifyInputParameters: The ChunkSize must between 1-1024.", new object[0]);
				return false;
			}
			int num = 0;
			if (this.DevicePlatformIDs != null)
			{
				foreach (string text in this.DevicePlatformIDs)
				{
					num += text.Length + 1;
				}
			}
			if ((long)num > 191L)
			{
				this._logger.LogError("ImageCommon!ImageGeneratorParameters::VerifyInputParameters: parameter DevicePlatformID larger than {0}.", new object[]
				{
					192U.ToString()
				});
				return false;
			}
			foreach (InputStore inputStore in this.Stores)
			{
				foreach (InputPartition inputPartition in inputStore.Partitions)
				{
					if (dictionary.ContainsKey(inputPartition.Name))
					{
						this._logger.LogError("ImageCommon!ImageGeneratorParameters::VerifyInputParameters: A partition '" + inputPartition.Name + "' is defined twice in the DeviceLayout.", new object[0]);
						return false;
					}
					dictionary.Add(inputPartition.Name, "Partitions");
				}
			}
			InputPartition inputPartition2 = null;
			foreach (InputPartition inputPartition3 in this.MainOSStore.Partitions)
			{
				if (inputPartition2 != null)
				{
					this._logger.LogError("ImageCommon!ImageGeneratorParameters::VerifyInputParameters: Partitions that specify UseAllSpace must be at the end.  See partition '{0}' and '{1}' for conflict.", new object[]
					{
						inputPartition2.Name,
						inputPartition3.Name
					});
					return false;
				}
				if (inputPartition3.UseAllSpace)
				{
					inputPartition2 = inputPartition3;
					if (inputPartition3.TotalSectors != 0U)
					{
						this._logger.LogError("ImageCommon!ImageGeneratorParameters::VerifyInputParameters: A partition cannot use all available space and have total sectors set.  See partition " + inputPartition3.Name, new object[0]);
						return false;
					}
				}
				if (inputPartition3.ByteAlignment != 0U)
				{
					if (inputPartition3.SingleSectorAlignment && inputPartition3.ByteAlignment != this.SectorSize)
					{
						this._logger.LogError("ImageCommon!ImageGeneratorParameters::VerifyInputParameters: Partition '{0}' has both a byte alignment and SingleSectorAlignment set.", new object[]
						{
							inputPartition3.Name
						});
						return false;
					}
					if (!InputHelpers.IsPowerOfTwo(inputPartition3.ByteAlignment))
					{
						this._logger.LogError("ImageCommon!ImageGeneratorParameters::VerifyInputParameters: The byte alignment for partition '{0}' must be a multiple of 2: {1} bytes.", new object[]
						{
							inputPartition3.Name,
							inputPartition3.ByteAlignment
						});
						return false;
					}
				}
				if (inputPartition3.SingleSectorAlignment)
				{
					inputPartition3.ByteAlignment = this.SectorSize;
				}
				if (!string.IsNullOrEmpty(inputPartition3.PrimaryPartition) && this.FindPartition(inputPartition3.PrimaryPartition) == null)
				{
					this._logger.LogError(string.Format("ImageCommon!ImageGeneratorParameters::VerifyInputParameters: The primary partition for partition '{0}' is not found Primary: '{1}'.", inputPartition3.Name, inputPartition3.PrimaryPartition), new object[0]);
					return false;
				}
			}
			return this.VerifyPartitionSizes();
		}

		// Token: 0x06000127 RID: 295 RVA: 0x00005888 File Offset: 0x00003A88
		private InputPartition FindPartition(string PartitionName)
		{
			foreach (InputStore inputStore in this.Stores)
			{
				IEnumerable<InputPartition> source = from x in inputStore.Partitions
				where x.Name.Equals(PartitionName, StringComparison.OrdinalIgnoreCase)
				select x;
				if (source.ToArray<InputPartition>().Length != 0)
				{
					return source.First<InputPartition>();
				}
			}
			return null;
		}

		// Token: 0x06000128 RID: 296 RVA: 0x0000592C File Offset: 0x00003B2C
		public static bool IsDeviceLayoutV2(string DeviceLayoutXMLFile)
		{
			XPathNavigator xpathNavigator = new XPathDocument(DeviceLayoutXMLFile).CreateNavigator();
			xpathNavigator.MoveToFollowing(XPathNodeType.Element);
			IDictionary<string, string> namespacesInScope = xpathNavigator.GetNamespacesInScope(XmlNamespaceScope.All);
			return namespacesInScope.Values.Any((string x) => string.CompareOrdinal(x, "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate/v2") == 0);
		}

		// Token: 0x06000129 RID: 297 RVA: 0x0000597D File Offset: 0x00003B7D
		public static Stream GetDeviceLayoutXSD(string deviceLayoutXMLFile)
		{
			if (ImageGeneratorParameters.IsDeviceLayoutV2(deviceLayoutXMLFile))
			{
				return ImageGeneratorParameters.GetXSDStream(DevicePaths.DeviceLayoutSchema2);
			}
			return ImageGeneratorParameters.GetXSDStream(DevicePaths.DeviceLayoutSchema);
		}

		// Token: 0x0600012A RID: 298 RVA: 0x0000599C File Offset: 0x00003B9C
		public static Stream GetOEMDevicePlatformXSD()
		{
			return ImageGeneratorParameters.GetXSDStream(DevicePaths.OEMDevicePlatformSchema);
		}

		// Token: 0x0600012B RID: 299 RVA: 0x000059A8 File Offset: 0x00003BA8
		public static Stream GetXSDStream(string xsdID)
		{
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			string[] manifestResourceNames = executingAssembly.GetManifestResourceNames();
			string text = string.Empty;
			foreach (string text2 in manifestResourceNames)
			{
				if (text2.Contains(xsdID))
				{
					text = text2;
					break;
				}
			}
			if (string.IsNullOrEmpty(text))
			{
				throw new ImageCommonException("ImageCommon!ImageGeneratorParameters::GetXSDStream: XSD resource was not found: " + xsdID);
			}
			return executingAssembly.GetManifestResourceStream(text);
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00005A34 File Offset: 0x00003C34
		public void ProcessInputXML(string deviceLayoutXMLFile, string oemDevicePlatformXMLFile)
		{
			OEMDevicePlatformInput oemdevicePlatformInput = null;
			XsdValidator xsdValidator = new XsdValidator();
			try
			{
				using (Stream deviceLayoutXSD = ImageGeneratorParameters.GetDeviceLayoutXSD(deviceLayoutXMLFile))
				{
					xsdValidator.ValidateXsd(deviceLayoutXSD, deviceLayoutXMLFile, this._logger);
				}
			}
			catch (XsdValidatorException innerException)
			{
				throw new ImageCommonException("ImageCommon!ImageGeneratorParameters::ProcessInputXML: Unable to validate Device Layout XSD.", innerException);
			}
			this._logger.LogInfo("ImageCommon: Successfully validated the Device Layout XML", new object[0]);
			if (ImageGeneratorParameters.IsDeviceLayoutV2(deviceLayoutXMLFile))
			{
				this.InitializeV2DeviceLayout(deviceLayoutXMLFile);
			}
			else
			{
				this.InitializeV1DeviceLayout(deviceLayoutXMLFile);
			}
			xsdValidator = new XsdValidator();
			try
			{
				using (Stream oemdevicePlatformXSD = ImageGeneratorParameters.GetOEMDevicePlatformXSD())
				{
					xsdValidator.ValidateXsd(oemdevicePlatformXSD, oemDevicePlatformXMLFile, this._logger);
				}
			}
			catch (XsdValidatorException innerException2)
			{
				throw new ImageCommonException("ImageCommon!ImageGeneratorParameters::ProcessInputXML: Unable to validate OEM Device Platform XSD.", innerException2);
			}
			this._logger.LogInfo("ImageCommon: Successfully validated the OEM Device Platform XML", new object[0]);
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(OEMDevicePlatformInput));
			using (StreamReader streamReader = new StreamReader(oemDevicePlatformXMLFile))
			{
				try
				{
					oemdevicePlatformInput = (OEMDevicePlatformInput)xmlSerializer.Deserialize(streamReader);
				}
				catch (Exception innerException3)
				{
					throw new ImageCommonException("ImageCommon!ImageGeneratorParameters::ProcessInputXML: Unable to parse OEM Device Platform XML.", innerException3);
				}
			}
			try
			{
				this.DevicePlatformIDs = oemdevicePlatformInput.DevicePlatformIDs;
				this.MinSectorCount = oemdevicePlatformInput.MinSectorCount;
				foreach (InputStore inputStore in this.Stores)
				{
					foreach (InputPartition inputPartition in from x in inputStore.Partitions
					where !string.IsNullOrEmpty(x.FileSystem) && x.FileSystem.Equals("NTFS", StringComparison.OrdinalIgnoreCase)
					select x)
					{
						inputPartition.Compressed = true;
					}
				}
				foreach (string text in oemdevicePlatformInput.UncompressedPartitions ?? new string[0])
				{
					InputPartition inputPartition2 = this.FindPartition(text);
					if (inputPartition2 == null)
					{
						throw new ImageCommonException("Partition " + text + " was marked in the OEMDevicePlatform as uncompressed, but the partition doesn't exist in the device layout. Please ensure the spelling of the partition is correct in OEMDevicePlatform and that the partition is defined in the OEMDeviceLayout.");
					}
					this._logger.LogInfo("ImageCommon: Marking partition " + text + " uncompressed as requested by device plaform.", new object[0]);
					inputPartition2.Compressed = false;
				}
				this.AddSectorsToMainOs(oemdevicePlatformInput.AdditionalMainOSFreeSectorsRequest, oemdevicePlatformInput.MainOSRTCDataReservedSectors);
				if (oemdevicePlatformInput.MMOSPartitionTotalSectorsOverride != 0U)
				{
					InputPartition inputPartition3 = this.FindPartition("MMOS");
					if (inputPartition3 == null)
					{
						throw new ImageCommonException("ImageCommon!ImageGeneratorParameters::ProcessInputXML: The OEM Device Platform XML specifies that the MMOS should have total sectors set but no MMOS partition was found.");
					}
					inputPartition3.TotalSectors = oemdevicePlatformInput.MMOSPartitionTotalSectorsOverride;
				}
			}
			catch (ImageCommonException)
			{
				throw;
			}
			catch (Exception innerException4)
			{
				throw new ImageCommonException("ImageCommon!ImageGeneratorParameters::ProcessInputXML: There was a problem parsing the OEM Device Platform input", innerException4);
			}
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00005DA8 File Offset: 0x00003FA8
		private void InitializeV1DeviceLayout(string DeviceLayoutXMLFile)
		{
			XmlDocument xmlDocument = new XmlDocument();
			try
			{
				xmlDocument.Load(DeviceLayoutXMLFile);
			}
			catch (Exception innerException)
			{
				throw new ImageCommonException("ImageCommon!ImageGeneratorParameters::InitializeV1DeviceLayout: Unable to validate Device Layout XSD.", innerException);
			}
			DeviceLayoutInput deviceLayoutInput = null;
			using (StreamReader streamReader = new StreamReader(DeviceLayoutXMLFile))
			{
				try
				{
					XmlSerializer xmlSerializer = new XmlSerializer(typeof(DeviceLayoutInput));
					deviceLayoutInput = (DeviceLayoutInput)xmlSerializer.Deserialize(streamReader);
				}
				catch (Exception innerException2)
				{
					throw new ImageCommonException("ImageCommon!ImageGeneratorParameters::InitializeV1DeviceLayout: Unable to parse Device Layout XML.", innerException2);
				}
			}
			try
			{
				InputStore inputStore = new InputStore("MainOSStore");
				if (deviceLayoutInput.Partitions != null)
				{
					inputStore.Partitions = deviceLayoutInput.Partitions;
				}
				this.SectorSize = deviceLayoutInput.SectorSize;
				this.ChunkSize = deviceLayoutInput.ChunkSize;
				this.VirtualHardDiskSectorSize = deviceLayoutInput.SectorSize;
				this.DefaultPartitionByteAlignment = deviceLayoutInput.DefaultPartitionByteAlignment;
				foreach (InputPartition inputPartition in inputStore.Partitions)
				{
					if (inputPartition.MinFreeSectors != 0U)
					{
						if (inputPartition.TotalSectors != 0U || inputPartition.UseAllSpace)
						{
							throw new ImageCommonException("ImageCommon!ImageGeneratorParameters::InitializeV1DeviceLayout: MinFreeSectors cannot be set for partition '" + inputPartition.Name + "' when either TotalSectors or UseAllSpace is set.");
						}
						if (inputPartition.MinFreeSectors < 8192U)
						{
							throw new ImageCommonException(string.Concat(new object[]
							{
								"ImageCommon!ImageGeneratorParameters::InitializeV1DeviceLayout: MinFreeSectors cannot be set for partition '",
								inputPartition.Name,
								"' less than ",
								8192U,
								" sectors."
							}));
						}
					}
					if (inputPartition.GeneratedFileOverheadSectors != 0U && inputPartition.MinFreeSectors == 0U)
					{
						throw new ImageCommonException("ImageCommon!ImageGeneratorParameters::InitializeV1DeviceLayout: GeneratedFileOverheadSectors cannot be set for partition '" + inputPartition.Name + "' without MinFreeSectors being set.");
					}
				}
				this.Stores.Add(inputStore);
			}
			catch (ImageCommonException)
			{
				throw;
			}
			catch (Exception innerException3)
			{
				throw new ImageCommonException("ImageCommon!ImageGeneratorParameters::InitializeV1DeviceLayout: There was a problem parsing the Device Layout input", innerException3);
			}
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00005FEC File Offset: 0x000041EC
		private void InitializeV2DeviceLayout(string DeviceLayoutXMLFile)
		{
			XmlDocument xmlDocument = new XmlDocument();
			try
			{
				xmlDocument.Load(DeviceLayoutXMLFile);
			}
			catch (Exception innerException)
			{
				throw new ImageCommonException("ImageCommon!ImageGeneratorParameters::InitializeV2DeviceLayout: Unable to validate Device Layout XSD.", innerException);
			}
			DeviceLayoutInputv2 deviceLayoutInputv = null;
			using (StreamReader streamReader = new StreamReader(DeviceLayoutXMLFile))
			{
				try
				{
					XmlSerializer xmlSerializer = new XmlSerializer(typeof(DeviceLayoutInputv2));
					deviceLayoutInputv = (DeviceLayoutInputv2)xmlSerializer.Deserialize(streamReader);
				}
				catch (Exception innerException2)
				{
					throw new ImageCommonException("ImageCommon!ImageGeneratorParameters::InitializeV2DeviceLayout: Unable to parse Device Layout XML.", innerException2);
				}
			}
			try
			{
				if (deviceLayoutInputv.Stores != null)
				{
					this.Stores = new List<InputStore>(deviceLayoutInputv.Stores);
				}
				this.SectorSize = deviceLayoutInputv.SectorSize;
				this.ChunkSize = deviceLayoutInputv.ChunkSize;
				this.VirtualHardDiskSectorSize = deviceLayoutInputv.SectorSize;
				this.DefaultPartitionByteAlignment = deviceLayoutInputv.DefaultPartitionByteAlignment;
				foreach (InputStore inputStore in this.Stores)
				{
					if (inputStore.IsMainOSStore())
					{
						if (inputStore.SizeInSectors != 0U)
						{
							throw new ImageCommonException("ImageCommon!ImageGeneratorParameters::InitializeV2DeviceLayout: SizeInSector cannot be set for MainOS store.'");
						}
					}
					else
					{
						if (string.IsNullOrEmpty(inputStore.Id))
						{
							throw new ImageCommonException("ImageCommon!ImageGeneratorParameters::InitializeV2DeviceLayout: Id needs to be set for individual stores.'");
						}
						if (inputStore.SizeInSectors == 0U)
						{
							throw new ImageCommonException("ImageCommon!ImageGeneratorParameters::InitializeV2DeviceLayout: SizeInSector needs to be set for non-MainOS store '" + inputStore.Id + "'.");
						}
						ulong num = (ulong)inputStore.SizeInSectors * (ulong)this.SectorSize;
						if (num < 3145728UL)
						{
							throw new ImageCommonException("ImageCommon!ImageGeneratorParameters::InitializeV2DeviceLayout: Minimum size of a store '" + inputStore.Id + "' must be 3MB or larger.");
						}
					}
					if (string.IsNullOrEmpty(inputStore.StoreType))
					{
						throw new ImageCommonException("ImageCommon!ImageGeneratorParameters::InitializeV2DeviceLayout: StoreType needs to be set for store '" + inputStore.Id + "'.");
					}
					if (string.IsNullOrEmpty(inputStore.DevicePath))
					{
						throw new ImageCommonException("ImageCommon!ImageGeneratorParameters::InitializeV2DeviceLayout: DevicePath needs to be set for store '" + inputStore.Id + "'.");
					}
					if (inputStore.OnlyAllocateDefinedGptEntries && inputStore.Partitions.Count<InputPartition>() > 32)
					{
						throw new ImageCommonException("ImageCommon!ImageGeneratorParameters::InitializeV2DeviceLayout: Cannot use shortened GPT as it has more than 32 partitions for store '" + inputStore.Id + "'.");
					}
					foreach (InputPartition inputPartition in inputStore.Partitions)
					{
						if (inputPartition.MinFreeSectors != 0U)
						{
							if (inputPartition.TotalSectors != 0U || inputPartition.UseAllSpace)
							{
								throw new ImageCommonException("ImageCommon!ImageGeneratorParameters::InitializeV2DeviceLayout: MinFreeSectors cannot be set for partition '" + inputPartition.Name + "' when either TotalSectors or UseAllSpace is set.");
							}
							if (inputPartition.MinFreeSectors < 8192U)
							{
								throw new ImageCommonException(string.Concat(new object[]
								{
									"ImageCommon!ImageGeneratorParameters::InitializeV2DeviceLayout: MinFreeSectors cannot be set for partition '",
									inputPartition.Name,
									"' less than ",
									8192U,
									" sectors."
								}));
							}
						}
						if (inputPartition.GeneratedFileOverheadSectors != 0U && inputPartition.MinFreeSectors == 0U)
						{
							throw new ImageCommonException("ImageCommon!ImageGeneratorParameters::InitializeV2DeviceLayout: GeneratedFileOverheadSectors cannot be set for partition '" + inputPartition.Name + "' without MinFreeSectors being set.");
						}
					}
				}
			}
			catch (ImageCommonException)
			{
				throw;
			}
			catch (Exception innerException3)
			{
				throw new ImageCommonException("ImageCommon!ImageGeneratorParameters::InitializeV2DeviceLayout: There was a problem parsing the Device Layout input", innerException3);
			}
			this._deviceLayoutVersion = 2U;
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00006390 File Offset: 0x00004590
		private void AddSectorsToMainOs(uint additionalFreeSectors, uint runtimeConfigurationDataSectors)
		{
			InputPartition inputPartition = this.FindPartition("MainOS");
			if (inputPartition == null)
			{
				throw new ImageCommonException("ImageCommon!ImageGeneratorParameters::AddSectorsToMainOs: No MainOS partition found for additional free sectors.");
			}
			if ((additionalFreeSectors != 0U || runtimeConfigurationDataSectors != 0U) && inputPartition.MinFreeSectors == 0U)
			{
				throw new ImageCommonException("ImageCommon!ImageGeneratorParameters::AddSectorsToMainOs: The OEM Device Platform XML specifies that the MainOS should have additional free sectors but the MainOS partition is not using MinFreeSectors.");
			}
			if (runtimeConfigurationDataSectors > 104857600U / this.SectorSize)
			{
				throw new ImageCommonException("ImageCommon!ImageGeneratorParameters::AddSectorsToMainOs: Runtime configuration data reservation is limited to 100MB. Please reduce the number of sectors requested in 'MainOSMVDataReservedSectors' in the OEM device platform input.");
			}
			if (additionalFreeSectors != 0U)
			{
				this._logger.LogInfo("OEM device platform input requested {0} additional free sectors in the MainOS partition.", new object[]
				{
					additionalFreeSectors
				});
			}
			if (runtimeConfigurationDataSectors != 0U)
			{
				this._logger.LogInfo("OEM device platform input requested {0} additional sectors for runtime configuration data be reserved in the MainOS partition.", new object[]
				{
					runtimeConfigurationDataSectors
				});
			}
			inputPartition.MinFreeSectors += additionalFreeSectors;
			inputPartition.MinFreeSectors += runtimeConfigurationDataSectors;
		}

		// Token: 0x0400006A RID: 106
		[CLSCompliant(false)]
		public const uint DefaultChunkSize = 256U;

		// Token: 0x0400006B RID: 107
		[CLSCompliant(false)]
		public const uint DevicePlatformIDSize = 192U;

		// Token: 0x0400006C RID: 108
		private const uint _OneKiloBtye = 1024U;

		// Token: 0x0400006D RID: 109
		private const uint _MinimumSectorSize = 512U;

		// Token: 0x0400006E RID: 110
		private const uint _MinimumSectorFreeCount = 8192U;

		// Token: 0x0400006F RID: 111
		private const uint ALG_CLASS_HASH = 32768U;

		// Token: 0x04000070 RID: 112
		private const uint ALG_TYPE_ANY = 0U;

		// Token: 0x04000071 RID: 113
		private const uint ALG_SID_SHA_256 = 12U;

		// Token: 0x04000072 RID: 114
		private const uint CALG_SHA_256 = 32780U;

		// Token: 0x04000073 RID: 115
		private IULogger _logger;

		// Token: 0x04000074 RID: 116
		public string[] DevicePlatformIDs;

		// Token: 0x04000075 RID: 117
		private uint _chunkSize = 256U;

		// Token: 0x04000076 RID: 118
		private uint _algid = 32780U;

		// Token: 0x04000077 RID: 119
		private uint _deviceLayoutVersion = 1U;

		// Token: 0x02000014 RID: 20
		[CLSCompliant(false)]
		public enum FFUHashAlgorithm : uint
		{
			// Token: 0x04000083 RID: 131
			Ffuhsha256 = 32780U
		}
	}
}
