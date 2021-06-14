using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using Microsoft.WindowsDeviceRecoveryTool.Common.Tracing;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Services;

namespace Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Msr
{
	// Token: 0x02000019 RID: 25
	[DataContract]
	public class MsrDownloadConfig
	{
		// Token: 0x060000E3 RID: 227 RVA: 0x00005F6C File Offset: 0x0000416C
		private MsrDownloadConfig()
		{
			this.DownloadProgressUpdateIntervalMillis = 300;
			this.MaxNumberOfParallelDownloads = 5;
			this.ChunkSizeBytes = 3145728;
			this.MinimalReportedDownloadedBytesIncrease = 1024;
			this.ReportingProgressIntervalMillis = 100;
			this.NumberOfProgressEventsToSkipInUI = 100;
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00005FC0 File Offset: 0x000041C0
		static MsrDownloadConfig()
		{
			try
			{
				FlowConditionService flowConditionService = new FlowConditionService();
				MsrDownloadConfig.IsTestConfigEnabled = flowConditionService.IsTestConfigFileAvailable;
			}
			catch (Exception)
			{
				Tracer<FlowConditionService>.WriteWarning("Could not initialize flow condition service.", new object[0]);
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000E5 RID: 229 RVA: 0x00006024 File Offset: 0x00004224
		public static MsrDownloadConfig Instance
		{
			get
			{
				return MsrDownloadConfig.configInstance.Value;
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000E6 RID: 230 RVA: 0x00006040 File Offset: 0x00004240
		// (set) Token: 0x060000E7 RID: 231 RVA: 0x00006057 File Offset: 0x00004257
		[DataMember]
		public int DownloadProgressUpdateIntervalMillis { get; private set; }

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000E8 RID: 232 RVA: 0x00006060 File Offset: 0x00004260
		// (set) Token: 0x060000E9 RID: 233 RVA: 0x00006077 File Offset: 0x00004277
		[DataMember]
		public int MaxNumberOfParallelDownloads { get; private set; }

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000EA RID: 234 RVA: 0x00006080 File Offset: 0x00004280
		// (set) Token: 0x060000EB RID: 235 RVA: 0x00006097 File Offset: 0x00004297
		[DataMember]
		public int ChunkSizeBytes { get; private set; }

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000EC RID: 236 RVA: 0x000060A0 File Offset: 0x000042A0
		// (set) Token: 0x060000ED RID: 237 RVA: 0x000060B7 File Offset: 0x000042B7
		[DataMember]
		public int MinimalReportedDownloadedBytesIncrease { get; private set; }

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000EE RID: 238 RVA: 0x000060C0 File Offset: 0x000042C0
		// (set) Token: 0x060000EF RID: 239 RVA: 0x000060D7 File Offset: 0x000042D7
		[DataMember]
		public int ReportingProgressIntervalMillis { get; private set; }

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000F0 RID: 240 RVA: 0x000060E0 File Offset: 0x000042E0
		// (set) Token: 0x060000F1 RID: 241 RVA: 0x000060F7 File Offset: 0x000042F7
		[DataMember]
		public int NumberOfProgressEventsToSkipInUI { get; private set; }

		// Token: 0x060000F2 RID: 242 RVA: 0x00006100 File Offset: 0x00004300
		private static MsrDownloadConfig ReadConfig()
		{
			MsrDownloadConfig result;
			if (!MsrDownloadConfig.IsTestConfigEnabled)
			{
				result = new MsrDownloadConfig();
			}
			else
			{
				MsrDownloadConfig msrDownloadConfig;
				if (!File.Exists("./MsrDownloadConfig.xml"))
				{
					msrDownloadConfig = new MsrDownloadConfig();
					using (FileStream fileStream = new FileStream("./MsrDownloadConfig.xml", FileMode.OpenOrCreate, FileAccess.Write))
					{
						DataContractSerializer dataContractSerializer = new DataContractSerializer(typeof(MsrDownloadConfig));
						dataContractSerializer.WriteObject(fileStream, msrDownloadConfig);
					}
				}
				else
				{
					using (XmlDictionaryReader xmlDictionaryReader = XmlDictionaryReader.CreateTextReader(new FileStream("./MsrDownloadConfig.xml", FileMode.Open, FileAccess.Read), new XmlDictionaryReaderQuotas()))
					{
						DataContractSerializer dataContractSerializer = new DataContractSerializer(typeof(MsrDownloadConfig));
						msrDownloadConfig = (MsrDownloadConfig)dataContractSerializer.ReadObject(xmlDictionaryReader, true);
					}
				}
				result = msrDownloadConfig;
			}
			return result;
		}

		// Token: 0x04000076 RID: 118
		private const string ConfigPathConst = "./MsrDownloadConfig.xml";

		// Token: 0x04000077 RID: 119
		private static readonly Lazy<MsrDownloadConfig> configInstance = new Lazy<MsrDownloadConfig>(new Func<MsrDownloadConfig>(MsrDownloadConfig.ReadConfig));

		// Token: 0x04000078 RID: 120
		private static readonly bool IsTestConfigEnabled;
	}
}
