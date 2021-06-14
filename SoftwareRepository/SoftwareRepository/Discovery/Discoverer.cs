using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SoftwareRepository.Discovery
{
	// Token: 0x0200001E RID: 30
	public class Discoverer
	{
		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000CF RID: 207 RVA: 0x00003B15 File Offset: 0x00001D15
		// (set) Token: 0x060000D0 RID: 208 RVA: 0x00003B1D File Offset: 0x00001D1D
		public DiscoveryCondition DiscoveryCondition { get; set; }

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000D1 RID: 209 RVA: 0x00003B26 File Offset: 0x00001D26
		// (set) Token: 0x060000D2 RID: 210 RVA: 0x00003B2E File Offset: 0x00001D2E
		[SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings")]
		public string SoftwareRepositoryAlternativeBaseUrl { get; set; }

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000D3 RID: 211 RVA: 0x00003B37 File Offset: 0x00001D37
		// (set) Token: 0x060000D4 RID: 212 RVA: 0x00003B3F File Offset: 0x00001D3F
		public string SoftwareRepositoryAuthenticationToken { get; set; }

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000D5 RID: 213 RVA: 0x00003B48 File Offset: 0x00001D48
		// (set) Token: 0x060000D6 RID: 214 RVA: 0x00003B50 File Offset: 0x00001D50
		public IWebProxy SoftwareRepositoryProxy { get; set; }

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000D7 RID: 215 RVA: 0x00003B59 File Offset: 0x00001D59
		// (set) Token: 0x060000D8 RID: 216 RVA: 0x00003B61 File Offset: 0x00001D61
		public string SoftwareRepositoryUserAgent { get; set; }

		// Token: 0x060000D9 RID: 217 RVA: 0x00003B6C File Offset: 0x00001D6C
		public async Task<DiscoveryResult> DiscoverAsync(string descriptor)
		{
			return await this.DiscoverAsync(descriptor, CancellationToken.None);
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00003BBC File Offset: 0x00001DBC
		public async Task<DiscoveryJsonResult> DiscoverJsonAsync(string descriptor)
		{
			return await this.DiscoverJsonAsync(descriptor, CancellationToken.None);
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00003C0C File Offset: 0x00001E0C
		public async Task<DiscoveryResult> DiscoverAsync(string descriptor, CancellationToken cancellationToken)
		{
			DiscoveryResult discoveryResult = new DiscoveryResult();
			DiscoveryJsonResult discoveryJsonResult = await this.DiscoverJsonAsync(descriptor, cancellationToken);
			discoveryResult.StatusCode = discoveryJsonResult.StatusCode;
			if (discoveryResult.StatusCode == HttpStatusCode.OK)
			{
				discoveryResult.Result = (SoftwarePackages)new DataContractJsonSerializer(typeof(SoftwarePackages)).ReadObject(new MemoryStream(Encoding.UTF8.GetBytes(discoveryJsonResult.Result)));
			}
			return discoveryResult;
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00003C64 File Offset: 0x00001E64
		public async Task<DiscoveryJsonResult> DiscoverJsonAsync(string descriptor, CancellationToken cancellationToken)
		{
			DiscoveryJsonResult discoveryResult = new DiscoveryJsonResult();
			try
			{
				XmlObjectSerializer xmlObjectSerializer = new DataContractJsonSerializer(typeof(DiscoveryParameters));
				MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(descriptor));
				DiscoveryParameters discoveryParameters = (DiscoveryParameters)xmlObjectSerializer.ReadObject(stream);
				if (discoveryParameters.APIVersion == null)
				{
					discoveryParameters.APIVersion = "1";
				}
				if (discoveryParameters.Condition == null)
				{
					discoveryParameters.Condition = new List<string>();
					discoveryParameters.Condition.Add("default");
				}
				if (discoveryParameters.Response == null || discoveryParameters.Response.Count == 0)
				{
					discoveryParameters.Response = new List<string>();
					discoveryParameters.Response.Add("default");
				}
				DiscoveryJsonResult discoveryJsonResult = await this.DiscoverJsonAsync(discoveryParameters, cancellationToken);
				discoveryResult = discoveryJsonResult;
			}
			catch (Exception innerException)
			{
				throw new DiscoveryException("Discovery exception", innerException);
			}
			return discoveryResult;
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00003CBC File Offset: 0x00001EBC
		[SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
		public async Task<DiscoveryResult> DiscoverAsync(string manufacturerName, string manufacturerProductLine, string packageType, string packageClass, [Optional] string packageTitle, [Optional] string packageSubtitle, [Optional] string packageRevision, [Optional] string packageSubRevision, [Optional] string packageState, [Optional] string manufacturerPackageId, [Optional] string manufacturerModelName, [Optional] string manufacturerVariantName, [Optional] string manufacturerPlatformId, [Optional] string manufacturerHardwareModel, [Optional] string manufacturerHardwareVariant, [Optional] string operatorName, [Optional] string customerName, [Optional] Dictionary<string, string> extendedAttributes, [Optional] List<string> responseFilter, [Optional] CancellationToken cancellationToken)
		{
			ExtendedAttributes extendedAttributes2 = null;
			if (extendedAttributes != null && extendedAttributes.Count > 0)
			{
				extendedAttributes2 = new ExtendedAttributes
				{
					Dictionary = extendedAttributes
				};
			}
			DiscoveryQueryParameters query = new DiscoveryQueryParameters
			{
				ManufacturerName = manufacturerName,
				ManufacturerProductLine = manufacturerProductLine,
				PackageType = packageType,
				PackageClass = packageClass,
				PackageTitle = packageTitle,
				PackageSubtitle = packageSubtitle,
				PackageRevision = packageRevision,
				PackageSubRevision = packageSubRevision,
				PackageState = packageState,
				ManufacturerPackageId = manufacturerPackageId,
				ManufacturerModelName = manufacturerModelName,
				ManufacturerVariantName = manufacturerVariantName,
				ManufacturerPlatformId = manufacturerPlatformId,
				ManufacturerHardwareModel = manufacturerHardwareModel,
				ManufacturerHardwareVariant = manufacturerHardwareVariant,
				OperatorName = operatorName,
				CustomerName = customerName,
				ExtendedAttributes = extendedAttributes2
			};
			DiscoveryParameters discoveryParameters = new DiscoveryParameters(this.DiscoveryCondition)
			{
				Query = query
			};
			if (responseFilter != null && responseFilter.Count > 0)
			{
				discoveryParameters.Response = responseFilter;
			}
			return await this.DiscoverAsync(discoveryParameters, cancellationToken);
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00003DB4 File Offset: 0x00001FB4
		public async Task<DiscoveryResult> DiscoverAsync(DiscoveryParameters discoveryParameters)
		{
			if (discoveryParameters.Response == null || discoveryParameters.Response.Count == 0)
			{
				discoveryParameters.Response = new List<string>();
				discoveryParameters.Response.Add("default");
			}
			return await this.DiscoverAsync(discoveryParameters, CancellationToken.None);
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00003E04 File Offset: 0x00002004
		public async Task<DiscoveryResult> DiscoverAsync(DiscoveryParameters discoveryParameters, CancellationToken cancellationToken)
		{
			DiscoveryResult discoveryResult = new DiscoveryResult();
			DiscoveryJsonResult discoveryJsonResult = await this.DiscoverJsonAsync(discoveryParameters, cancellationToken);
			discoveryResult.StatusCode = discoveryJsonResult.StatusCode;
			if (discoveryResult.StatusCode == HttpStatusCode.OK)
			{
				discoveryResult.Result = (SoftwarePackages)new DataContractJsonSerializer(typeof(SoftwarePackages)).ReadObject(new MemoryStream(Encoding.UTF8.GetBytes(discoveryJsonResult.Result)));
			}
			return discoveryResult;
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00003E5C File Offset: 0x0000205C
		[SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
		public async Task<DiscoveryJsonResult> DiscoverJsonAsync(string manufacturerName, string manufacturerProductLine, string packageType, string packageClass, [Optional] string packageTitle, [Optional] string packageSubtitle, [Optional] string packageRevision, [Optional] string packageSubRevision, [Optional] string packageState, [Optional] string manufacturerPackageId, [Optional] string manufacturerModelName, [Optional] string manufacturerVariantName, [Optional] string manufacturerPlatformId, [Optional] string manufacturerHardwareModel, [Optional] string manufacturerHardwareVariant, [Optional] string operatorName, [Optional] string customerName, [Optional] Dictionary<string, string> extendedAttributes, [Optional] List<string> responseFilter, [Optional] CancellationToken cancellationToken)
		{
			ExtendedAttributes extendedAttributes2 = null;
			if (extendedAttributes != null && extendedAttributes.Count > 0)
			{
				extendedAttributes2 = new ExtendedAttributes
				{
					Dictionary = extendedAttributes
				};
			}
			DiscoveryQueryParameters query = new DiscoveryQueryParameters
			{
				ManufacturerName = manufacturerName,
				ManufacturerProductLine = manufacturerProductLine,
				PackageType = packageType,
				PackageClass = packageClass,
				PackageTitle = packageTitle,
				PackageSubtitle = packageSubtitle,
				PackageRevision = packageRevision,
				PackageSubRevision = packageSubRevision,
				PackageState = packageState,
				ManufacturerPackageId = manufacturerPackageId,
				ManufacturerModelName = manufacturerModelName,
				ManufacturerVariantName = manufacturerVariantName,
				ManufacturerPlatformId = manufacturerPlatformId,
				ManufacturerHardwareModel = manufacturerHardwareModel,
				ManufacturerHardwareVariant = manufacturerHardwareVariant,
				OperatorName = operatorName,
				CustomerName = customerName,
				ExtendedAttributes = extendedAttributes2
			};
			DiscoveryParameters discoveryParameters = new DiscoveryParameters(this.DiscoveryCondition)
			{
				Query = query
			};
			if (responseFilter != null && responseFilter.Count > 0)
			{
				discoveryParameters.Response = responseFilter;
			}
			return await this.DiscoverJsonAsync(discoveryParameters, cancellationToken);
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00003F54 File Offset: 0x00002154
		public async Task<DiscoveryJsonResult> DiscoverJsonAsync(DiscoveryParameters discoveryParameters)
		{
			return await this.DiscoverJsonAsync(discoveryParameters, CancellationToken.None);
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00003FA4 File Offset: 0x000021A4
		public async Task<DiscoveryJsonResult> DiscoverJsonAsync(DiscoveryParameters discoveryParameters, CancellationToken cancellationToken)
		{
			DiscoveryJsonResult discoveryJsonResult = new DiscoveryJsonResult();
			try
			{
				XmlObjectSerializer xmlObjectSerializer = new DataContractJsonSerializer(typeof(DiscoveryParameters));
				MemoryStream memoryStream = new MemoryStream();
				xmlObjectSerializer.WriteObject(memoryStream, discoveryParameters);
				memoryStream.Seek(0L, SeekOrigin.Begin);
				string content = new StreamReader(memoryStream).ReadToEnd();
				string str = "https://api.swrepository.com";
				if (this.SoftwareRepositoryAlternativeBaseUrl != null)
				{
					str = this.SoftwareRepositoryAlternativeBaseUrl;
				}
				Uri requestUri = new Uri(str + "/rest-api/discovery/1/package");
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
				discoveryJsonResult.StatusCode = httpResponseMessage.StatusCode;
				if (discoveryJsonResult.StatusCode == HttpStatusCode.OK)
				{
					HttpContent content2 = httpResponseMessage.Content;
					DiscoveryJsonResult discoveryJsonResult2 = discoveryJsonResult;
					discoveryJsonResult2.Result = await content2.ReadAsStringAsync();
					discoveryJsonResult2 = null;
				}
				else
				{
					discoveryJsonResult.Result = string.Empty;
				}
				httpClient.Dispose();
				httpClient = null;
			}
			catch (Exception innerException)
			{
				throw new DiscoveryException("Discovery exception", innerException);
			}
			return discoveryJsonResult;
		}

		// Token: 0x04000082 RID: 130
		private const string DefaultSoftwareRepositoryBaseUrl = "https://api.swrepository.com";

		// Token: 0x04000083 RID: 131
		private const string DefaultSoftwareRepositoryDiscovery = "/rest-api/discovery/1/package";

		// Token: 0x04000084 RID: 132
		private const string DefaultSoftwareRepositoryUserAgent = "SoftwareRepository";
	}
}
