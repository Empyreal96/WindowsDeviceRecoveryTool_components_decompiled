using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;

namespace SoftwareRepository.Reporting
{
	// Token: 0x0200001B RID: 27
	public class Reporter
	{
		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000B2 RID: 178 RVA: 0x000038E8 File Offset: 0x00001AE8
		// (set) Token: 0x060000B3 RID: 179 RVA: 0x000038F0 File Offset: 0x00001AF0
		[SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings")]
		public string SoftwareRepositoryAlternativeBaseUrl { get; set; }

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000B4 RID: 180 RVA: 0x000038F9 File Offset: 0x00001AF9
		// (set) Token: 0x060000B5 RID: 181 RVA: 0x00003901 File Offset: 0x00001B01
		public string SoftwareRepositoryAuthenticationToken { get; set; }

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000B6 RID: 182 RVA: 0x0000390A File Offset: 0x00001B0A
		// (set) Token: 0x060000B7 RID: 183 RVA: 0x00003912 File Offset: 0x00001B12
		public IWebProxy SoftwareRepositoryProxy { get; set; }

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000B8 RID: 184 RVA: 0x0000391B File Offset: 0x00001B1B
		// (set) Token: 0x060000B9 RID: 185 RVA: 0x00003923 File Offset: 0x00001B23
		public string SoftwareRepositoryUserAgent { get; set; }

		// Token: 0x060000BA RID: 186 RVA: 0x0000392C File Offset: 0x00001B2C
		public async Task<string> GetReportUploadLocationAsync(string manufacturerName, string manufacturerProductLine, string reportClassification, string fileName, CancellationToken cancellationToken)
		{
			string ret = string.Empty;
			ReportUploadLocationParameters graph = new ReportUploadLocationParameters
			{
				ManufacturerName = manufacturerName,
				ManufacturerProductLine = manufacturerProductLine,
				ReportClassification = reportClassification,
				FileName = fileName
			};
			try
			{
				XmlObjectSerializer xmlObjectSerializer = new DataContractJsonSerializer(typeof(ReportUploadLocationParameters));
				MemoryStream memoryStream = new MemoryStream();
				xmlObjectSerializer.WriteObject(memoryStream, graph);
				memoryStream.Seek(0L, SeekOrigin.Begin);
				string content = new StreamReader(memoryStream).ReadToEnd();
				string str = "https://api.swrepository.com";
				if (this.SoftwareRepositoryAlternativeBaseUrl != null)
				{
					str = this.SoftwareRepositoryAlternativeBaseUrl;
				}
				Uri requestUri = new Uri(str + "/rest-api/report/1/uploadlocation");
				string input = "SoftwareRepository";
				if (this.SoftwareRepositoryUserAgent != null)
				{
					input = this.SoftwareRepositoryUserAgent;
				}
				HttpClient httpClient = null;
				if (this.SoftwareRepositoryProxy != null)
				{
					httpClient = new HttpClient(new HttpClientHandler
					{
						Proxy = this.SoftwareRepositoryProxy,
						UseProxy = true
					});
				}
				else
				{
					httpClient = new HttpClient();
				}
				httpClient.DefaultRequestHeaders.UserAgent.TryParseAdd(input);
				httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				if (this.SoftwareRepositoryAuthenticationToken != null)
				{
					httpClient.DefaultRequestHeaders.Add("X-Authentication", this.SoftwareRepositoryAuthenticationToken);
					httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.SoftwareRepositoryAuthenticationToken);
				}
				HttpResponseMessage httpResponseMessage = await httpClient.PostAsync(requestUri, new StringContent(content, Encoding.UTF8, "application/json"), cancellationToken);
				httpResponseMessage.EnsureSuccessStatusCode();
				try
				{
					ret = httpResponseMessage.Headers.First((KeyValuePair<string, IEnumerable<string>> h) => h.Key.Equals("X-Upload-Location")).Value.First<string>();
				}
				catch (InvalidOperationException)
				{
					if (httpResponseMessage.Headers.Location != null)
					{
						ret = httpResponseMessage.Headers.Location.AbsoluteUri;
					}
				}
				httpClient.Dispose();
				httpClient = null;
			}
			catch (Exception innerException)
			{
				throw new ReportException("Report exception", innerException);
			}
			return ret;
		}

		// Token: 0x060000BB RID: 187 RVA: 0x0000399C File Offset: 0x00001B9C
		[SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
		public async Task<bool> UploadReportAsync(string manufacturerName, string manufacturerProductLine, string reportClassification, List<string> filePaths, CancellationToken cancellationToken)
		{
			try
			{
				foreach (string filePath in filePaths)
				{
					string fileName = Path.GetFileName(filePath);
					await new CloudBlockBlob(new Uri(await this.GetReportUploadLocationAsync(manufacturerName, manufacturerProductLine, reportClassification, fileName, cancellationToken))).UploadFromFileAsync(filePath, FileMode.Open);
					filePath = null;
				}
				List<string>.Enumerator enumerator = default(List<string>.Enumerator);
			}
			catch (Exception innerException)
			{
				throw new ReportException("Cannot upload report.", innerException);
			}
			return true;
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00003A0C File Offset: 0x00001C0C
		[SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "cancellationToken")]
		internal async Task SendDownloadReport(string id, string filename, List<string> url, int status, long time, long size, int connections, CancellationToken cancellationToken)
		{
			DownloadReport graph = new DownloadReport
			{
				ApiVersion = "1",
				Id = id,
				FileName = filename,
				Url = url,
				Status = status,
				Time = time,
				Size = size,
				Connections = connections
			};
			XmlObjectSerializer xmlObjectSerializer = new DataContractJsonSerializer(typeof(DownloadReport));
			MemoryStream memoryStream = new MemoryStream();
			xmlObjectSerializer.WriteObject(memoryStream, graph);
			memoryStream.Seek(0L, SeekOrigin.Begin);
			string content = new StreamReader(memoryStream).ReadToEnd();
			string str = "https://api.swrepository.com";
			if (this.SoftwareRepositoryAlternativeBaseUrl != null)
			{
				str = this.SoftwareRepositoryAlternativeBaseUrl;
			}
			string input = "SoftwareRepository";
			if (this.SoftwareRepositoryUserAgent != null)
			{
				input = this.SoftwareRepositoryUserAgent;
			}
			Uri requestUri = new Uri(str + "/rest-api/discovery/1/report");
			HttpClient httpClient = null;
			if (this.SoftwareRepositoryProxy != null)
			{
				httpClient = new HttpClient(new HttpClientHandler
				{
					Proxy = this.SoftwareRepositoryProxy,
					UseProxy = true
				});
			}
			else
			{
				httpClient = new HttpClient();
			}
			httpClient.DefaultRequestHeaders.UserAgent.TryParseAdd(input);
			HttpResponseMessage httpResponseMessage = await httpClient.PostAsync(requestUri, new StringContent(content, Encoding.UTF8, "application/json"));
			if (httpResponseMessage.StatusCode != HttpStatusCode.OK && httpResponseMessage.StatusCode != HttpStatusCode.BadRequest)
			{
				HttpStatusCode statusCode = httpResponseMessage.StatusCode;
			}
			httpClient.Dispose();
		}

		// Token: 0x0400006C RID: 108
		private const string DefaultSoftwareRepositoryBaseUrl = "https://api.swrepository.com";

		// Token: 0x0400006D RID: 109
		private const string DefaultSoftwareRepositoryDownloadReportUrl = "/rest-api/discovery/1/report";

		// Token: 0x0400006E RID: 110
		private const string DefaultSoftwareRepositoryUploadReport = "/rest-api/report";

		// Token: 0x0400006F RID: 111
		private const string DefaultSoftwareRepositoryReportUploadApiVersion = "/1";

		// Token: 0x04000070 RID: 112
		private const string DefaultSoftwareRepositoryReportUploadApi = "/uploadlocation";

		// Token: 0x04000071 RID: 113
		private const string DefaultSoftwareRepositoryUserAgent = "SoftwareRepository";
	}
}
