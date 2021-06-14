using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml;
using System.Xml.Linq;
using Microsoft.WindowsAzure.Storage.Core.Executor;

namespace Microsoft.WindowsAzure.Storage.Shared.Protocol
{
	// Token: 0x02000050 RID: 80
	internal static class HttpResponseParsers
	{
		// Token: 0x06000CF0 RID: 3312 RVA: 0x0002FF8C File Offset: 0x0002E18C
		internal static T ProcessExpectedStatusCodeNoException<T>(HttpStatusCode expectedStatusCode, HttpWebResponse resp, T retVal, StorageCommandBase<T> cmd, Exception ex)
		{
			return HttpResponseParsers.ProcessExpectedStatusCodeNoException<T>(expectedStatusCode, (resp != null) ? resp.StatusCode : HttpStatusCode.Unused, retVal, cmd, ex);
		}

		// Token: 0x06000CF1 RID: 3313 RVA: 0x0002FFA8 File Offset: 0x0002E1A8
		internal static T ProcessExpectedStatusCodeNoException<T>(HttpStatusCode[] expectedStatusCodes, HttpWebResponse resp, T retVal, StorageCommandBase<T> cmd, Exception ex)
		{
			return HttpResponseParsers.ProcessExpectedStatusCodeNoException<T>(expectedStatusCodes, (resp != null) ? resp.StatusCode : HttpStatusCode.Unused, retVal, cmd, ex);
		}

		// Token: 0x06000CF2 RID: 3314 RVA: 0x0002FFC4 File Offset: 0x0002E1C4
		internal static string GetETag(HttpWebResponse response)
		{
			return response.Headers[HttpResponseHeader.ETag];
		}

		// Token: 0x06000CF3 RID: 3315 RVA: 0x0002FFD3 File Offset: 0x0002E1D3
		internal static IDictionary<string, string> GetMetadata(HttpWebResponse response)
		{
			return HttpResponseParsers.GetMetadataOrProperties(response, "x-ms-meta-");
		}

		// Token: 0x06000CF4 RID: 3316 RVA: 0x0002FFE0 File Offset: 0x0002E1E0
		private static IDictionary<string, string> GetMetadataOrProperties(HttpWebResponse response, string prefix)
		{
			IDictionary<string, string> dictionary = new Dictionary<string, string>();
			int length = prefix.Length;
			foreach (string text in response.Headers.AllKeys)
			{
				if (text.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
				{
					dictionary[text.Substring(length)] = response.Headers[text];
				}
			}
			return dictionary;
		}

		// Token: 0x06000CF5 RID: 3317 RVA: 0x00030041 File Offset: 0x0002E241
		internal static DateTime ToUTCTime(this string str)
		{
			return DateTime.Parse(str, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.AdjustToUniversal);
		}

		// Token: 0x06000CF6 RID: 3318 RVA: 0x00030050 File Offset: 0x0002E250
		internal static T ProcessExpectedStatusCodeNoException<T>(HttpStatusCode expectedStatusCode, HttpStatusCode actualStatusCode, T retVal, StorageCommandBase<T> cmd, Exception ex)
		{
			if (ex != null)
			{
				throw ex;
			}
			if (actualStatusCode != expectedStatusCode)
			{
				throw new StorageException(cmd.CurrentResult, string.Format(CultureInfo.InvariantCulture, "Unexpected response code, Expected:{0}, Received:{1}", new object[]
				{
					expectedStatusCode,
					actualStatusCode
				}), null);
			}
			return retVal;
		}

		// Token: 0x06000CF7 RID: 3319 RVA: 0x000300A0 File Offset: 0x0002E2A0
		internal static T ProcessExpectedStatusCodeNoException<T>(HttpStatusCode[] expectedStatusCodes, HttpStatusCode actualStatusCode, T retVal, StorageCommandBase<T> cmd, Exception ex)
		{
			if (ex != null)
			{
				throw ex;
			}
			if (!expectedStatusCodes.Contains(actualStatusCode))
			{
				string text = string.Join<HttpStatusCode>(",", expectedStatusCodes);
				throw new StorageException(cmd.CurrentResult, string.Format(CultureInfo.InvariantCulture, "Unexpected response code, Expected:{0}, Received:{1}", new object[]
				{
					text,
					actualStatusCode.ToString()
				}), null);
			}
			return retVal;
		}

		// Token: 0x06000CF8 RID: 3320 RVA: 0x00030100 File Offset: 0x0002E300
		internal static void ValidateResponseStreamMd5AndLength<T>(long? length, string md5, StorageCommandBase<T> cmd)
		{
			if (cmd.StreamCopyState == null)
			{
				throw new StorageException(cmd.CurrentResult, "The operation requires a response body but no data was copied to the destination buffer.", null)
				{
					IsRetryable = false
				};
			}
			if (length != null && cmd.StreamCopyState.Length != length.Value)
			{
				throw new StorageException(cmd.CurrentResult, string.Format(CultureInfo.InvariantCulture, "Incorrect number of bytes received. Expected '{0}', received '{1}'", new object[]
				{
					length,
					cmd.StreamCopyState.Length
				}), null)
				{
					IsRetryable = false
				};
			}
			if (md5 != null && cmd.StreamCopyState.Md5 != null && cmd.StreamCopyState.Md5 != md5)
			{
				throw new StorageException(cmd.CurrentResult, "Calculated MD5 does not match existing property", null)
				{
					IsRetryable = false
				};
			}
		}

		// Token: 0x06000CF9 RID: 3321 RVA: 0x000301D4 File Offset: 0x0002E3D4
		internal static ServiceProperties ReadServiceProperties(Stream inputStream)
		{
			ServiceProperties result;
			using (XmlReader xmlReader = XmlReader.Create(inputStream))
			{
				XDocument servicePropertiesDocument = XDocument.Load(xmlReader);
				result = ServiceProperties.FromServiceXml(servicePropertiesDocument);
			}
			return result;
		}

		// Token: 0x06000CFA RID: 3322 RVA: 0x00030214 File Offset: 0x0002E414
		internal static ServiceStats ReadServiceStats(Stream inputStream)
		{
			ServiceStats result;
			using (XmlReader xmlReader = XmlReader.Create(inputStream))
			{
				XDocument serviceStatsDocument = XDocument.Load(xmlReader);
				result = ServiceStats.FromServiceXml(serviceStatsDocument);
			}
			return result;
		}

		// Token: 0x06000CFB RID: 3323 RVA: 0x00030254 File Offset: 0x0002E454
		internal static void ReadSharedAccessIdentifiers<T>(IDictionary<string, T> sharedAccessPolicies, AccessPolicyResponseBase<T> policyResponse) where T : new()
		{
			foreach (KeyValuePair<string, T> keyValuePair in policyResponse.AccessIdentifiers)
			{
				sharedAccessPolicies.Add(keyValuePair.Key, keyValuePair.Value);
			}
		}
	}
}
