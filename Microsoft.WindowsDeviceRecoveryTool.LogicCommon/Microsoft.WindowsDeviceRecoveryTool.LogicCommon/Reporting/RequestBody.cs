using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Reporting
{
	// Token: 0x02000030 RID: 48
	[DataContract]
	internal class RequestBody
	{
		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x06000276 RID: 630 RVA: 0x0000A26C File Offset: 0x0000846C
		// (set) Token: 0x06000277 RID: 631 RVA: 0x0000A283 File Offset: 0x00008483
		[DataMember(Name = "manufacturerName")]
		internal string ManufacturerName { get; set; }

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x06000278 RID: 632 RVA: 0x0000A28C File Offset: 0x0000848C
		// (set) Token: 0x06000279 RID: 633 RVA: 0x0000A2A3 File Offset: 0x000084A3
		[DataMember(Name = "manufacturerProductLine")]
		internal string ManufacturerProductLine { get; set; }

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x0600027A RID: 634 RVA: 0x0000A2AC File Offset: 0x000084AC
		// (set) Token: 0x0600027B RID: 635 RVA: 0x0000A2C3 File Offset: 0x000084C3
		[DataMember(Name = "reportClassification")]
		internal string ReportClassification { get; set; }

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x0600027C RID: 636 RVA: 0x0000A2CC File Offset: 0x000084CC
		// (set) Token: 0x0600027D RID: 637 RVA: 0x0000A2E3 File Offset: 0x000084E3
		[DataMember(Name = "fileName")]
		internal string FileName { get; set; }

		// Token: 0x0600027E RID: 638 RVA: 0x0000A2EC File Offset: 0x000084EC
		public string ToJsonString()
		{
			string result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				RequestBody.Serializer.WriteObject(memoryStream, this);
				memoryStream.Flush();
				memoryStream.Position = 0L;
				using (StreamReader streamReader = new StreamReader(memoryStream))
				{
					result = streamReader.ReadToEnd();
				}
			}
			return result;
		}

		// Token: 0x04000142 RID: 322
		private static readonly DataContractJsonSerializer Serializer = new DataContractJsonSerializer(typeof(RequestBody));
	}
}
