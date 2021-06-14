using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;
using SoftwareRepository.Discovery;

namespace SoftwareRepository.Streaming
{
	// Token: 0x02000017 RID: 23
	[DataContract]
	internal class FileUrlResult
	{
		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000092 RID: 146 RVA: 0x00003711 File Offset: 0x00001911
		// (set) Token: 0x06000093 RID: 147 RVA: 0x00003719 File Offset: 0x00001919
		[DataMember(Name = "url")]
		internal string Url { get; set; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000094 RID: 148 RVA: 0x00003722 File Offset: 0x00001922
		// (set) Token: 0x06000095 RID: 149 RVA: 0x0000372A File Offset: 0x0000192A
		[DataMember(Name = "alternateUrl")]
		internal List<string> AlternateUrl { get; set; }

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000096 RID: 150 RVA: 0x00003733 File Offset: 0x00001933
		// (set) Token: 0x06000097 RID: 151 RVA: 0x0000373B File Offset: 0x0000193B
		[DataMember(Name = "fileSize")]
		internal long FileSize { get; set; }

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000098 RID: 152 RVA: 0x00003744 File Offset: 0x00001944
		// (set) Token: 0x06000099 RID: 153 RVA: 0x0000374C File Offset: 0x0000194C
		[DataMember(Name = "checksum")]
		internal List<SoftwareFileChecksum> Checksum { get; set; }

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x0600009A RID: 154 RVA: 0x00003755 File Offset: 0x00001955
		// (set) Token: 0x0600009B RID: 155 RVA: 0x0000375D File Offset: 0x0000195D
		internal HttpStatusCode StatusCode { get; set; }

		// Token: 0x0600009C RID: 156 RVA: 0x00003768 File Offset: 0x00001968
		internal List<string> GetFileUrls()
		{
			List<string> list = new List<string>();
			if (!string.IsNullOrEmpty(this.Url))
			{
				list.Add(this.Url);
			}
			if (this.AlternateUrl != null)
			{
				foreach (string text in this.AlternateUrl)
				{
					if (!string.IsNullOrEmpty(text))
					{
						list.Add(text);
					}
				}
			}
			return list;
		}
	}
}
