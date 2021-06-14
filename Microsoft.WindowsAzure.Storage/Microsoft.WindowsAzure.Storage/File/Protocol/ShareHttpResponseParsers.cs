using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Xml;
using System.Xml.Linq;
using Microsoft.WindowsAzure.Storage.Core.Util;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;

namespace Microsoft.WindowsAzure.Storage.File.Protocol
{
	// Token: 0x02000031 RID: 49
	public static class ShareHttpResponseParsers
	{
		// Token: 0x06000959 RID: 2393 RVA: 0x000225AE File Offset: 0x000207AE
		public static string GetRequestId(HttpWebResponse response)
		{
			return Response.GetRequestId(response);
		}

		// Token: 0x0600095A RID: 2394 RVA: 0x000225B8 File Offset: 0x000207B8
		public static FileShareProperties GetProperties(HttpWebResponse response)
		{
			CommonUtility.AssertNotNull("response", response);
			FileShareProperties fileShareProperties = new FileShareProperties();
			fileShareProperties.ETag = HttpResponseParsers.GetETag(response);
			fileShareProperties.LastModified = new DateTimeOffset?(response.LastModified.ToUniversalTime());
			string text = response.Headers["x-ms-share-quota"];
			if (!string.IsNullOrEmpty(text))
			{
				fileShareProperties.Quota = new int?(int.Parse(text, CultureInfo.InvariantCulture));
			}
			return fileShareProperties;
		}

		// Token: 0x0600095B RID: 2395 RVA: 0x00022630 File Offset: 0x00020830
		public static IDictionary<string, string> GetMetadata(HttpWebResponse response)
		{
			return HttpResponseParsers.GetMetadata(response);
		}

		// Token: 0x0600095C RID: 2396 RVA: 0x00022638 File Offset: 0x00020838
		public static void ReadSharedAccessIdentifiers(Stream inputStream, FileSharePermissions permissions)
		{
			CommonUtility.AssertNotNull("permissions", permissions);
			Response.ReadSharedAccessIdentifiers<SharedAccessFilePolicy>(permissions.SharedAccessPolicies, new FileAccessPolicyResponse(inputStream));
		}

		// Token: 0x0600095D RID: 2397 RVA: 0x00022658 File Offset: 0x00020858
		public static ShareStats ReadShareStats(Stream inputStream)
		{
			ShareStats result;
			using (XmlReader xmlReader = XmlReader.Create(inputStream))
			{
				XDocument shareStatsDocument = XDocument.Load(xmlReader);
				result = ShareStats.FromServiceXml(shareStatsDocument);
			}
			return result;
		}
	}
}
