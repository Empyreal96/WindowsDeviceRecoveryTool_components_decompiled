using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Xml;
using System.Xml.Linq;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Blob.Protocol;
using Microsoft.WindowsAzure.Storage.Core.Util;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;

namespace Microsoft.WindowsAzure.Storage.File.Protocol
{
	// Token: 0x0200002F RID: 47
	public static class FileHttpResponseParsers
	{
		// Token: 0x06000940 RID: 2368 RVA: 0x00021D9C File Offset: 0x0001FF9C
		public static string GetRequestId(HttpWebResponse response)
		{
			return Response.GetRequestId(response);
		}

		// Token: 0x06000941 RID: 2369 RVA: 0x00021DA4 File Offset: 0x0001FFA4
		public static FileProperties GetProperties(HttpWebResponse response)
		{
			CommonUtility.AssertNotNull("response", response);
			FileProperties fileProperties = new FileProperties();
			fileProperties.ETag = HttpResponseParsers.GetETag(response);
			fileProperties.LastModified = new DateTimeOffset?(response.LastModified.ToUniversalTime());
			fileProperties.ContentLanguage = response.Headers[HttpResponseHeader.ContentLanguage];
			fileProperties.ContentDisposition = response.Headers["Content-Disposition"];
			fileProperties.ContentEncoding = response.Headers[HttpResponseHeader.ContentEncoding];
			fileProperties.ContentMD5 = response.Headers[HttpResponseHeader.ContentMd5];
			fileProperties.ContentType = response.Headers[HttpResponseHeader.ContentType];
			fileProperties.CacheControl = response.Headers[HttpResponseHeader.CacheControl];
			string text = response.Headers[HttpResponseHeader.ContentRange];
			string text2 = response.Headers["Content-Length"];
			string text3 = response.Headers["x-ms-content-length"];
			if (!string.IsNullOrEmpty(text))
			{
				fileProperties.Length = long.Parse(text.Split(new char[]
				{
					'/'
				})[1], CultureInfo.InvariantCulture);
			}
			else if (!string.IsNullOrEmpty(text3))
			{
				fileProperties.Length = long.Parse(text3, CultureInfo.InvariantCulture);
			}
			else if (!string.IsNullOrEmpty(text2))
			{
				fileProperties.Length = long.Parse(text2, CultureInfo.InvariantCulture);
			}
			else
			{
				fileProperties.Length = response.ContentLength;
			}
			return fileProperties;
		}

		// Token: 0x06000942 RID: 2370 RVA: 0x00021F06 File Offset: 0x00020106
		public static IDictionary<string, string> GetMetadata(HttpWebResponse response)
		{
			return HttpResponseParsers.GetMetadata(response);
		}

		// Token: 0x06000943 RID: 2371 RVA: 0x00021F0E File Offset: 0x0002010E
		public static CopyState GetCopyAttributes(HttpWebResponse response)
		{
			return BlobHttpResponseParsers.GetCopyAttributes(response);
		}

		// Token: 0x06000944 RID: 2372 RVA: 0x00021F18 File Offset: 0x00020118
		public static FileServiceProperties ReadServiceProperties(Stream inputStream)
		{
			FileServiceProperties result;
			using (XmlReader xmlReader = XmlReader.Create(inputStream))
			{
				XDocument servicePropertiesDocument = XDocument.Load(xmlReader);
				result = FileServiceProperties.FromServiceXml(servicePropertiesDocument);
			}
			return result;
		}

		// Token: 0x06000945 RID: 2373 RVA: 0x00021F58 File Offset: 0x00020158
		public static ServiceStats ReadServiceStats(Stream inputStream)
		{
			return HttpResponseParsers.ReadServiceStats(inputStream);
		}
	}
}
