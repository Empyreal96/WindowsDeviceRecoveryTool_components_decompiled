using System;
using System.Globalization;
using System.IO;

namespace Microsoft.Data.OData
{
	// Token: 0x020001D6 RID: 470
	internal static class ODataBatchUtils
	{
		// Token: 0x06000E92 RID: 3730 RVA: 0x00032D68 File Offset: 0x00030F68
		internal static Uri CreateOperationRequestUri(Uri uri, Uri baseUri, IODataUrlResolver urlResolver)
		{
			Uri uri2;
			if (urlResolver != null)
			{
				uri2 = urlResolver.ResolveUrl(baseUri, uri);
				if (uri2 != null)
				{
					return uri2;
				}
			}
			if (uri.IsAbsoluteUri)
			{
				uri2 = uri;
			}
			else
			{
				if (baseUri == null)
				{
					string message = UriUtilsCommon.UriToString(uri).StartsWith("$", StringComparison.Ordinal) ? Strings.ODataBatchUtils_RelativeUriStartingWithDollarUsedWithoutBaseUriSpecified(UriUtilsCommon.UriToString(uri)) : Strings.ODataBatchUtils_RelativeUriUsedWithoutBaseUriSpecified(UriUtilsCommon.UriToString(uri));
					throw new ODataException(message);
				}
				uri2 = UriUtils.UriToAbsoluteUri(baseUri, uri);
			}
			return uri2;
		}

		// Token: 0x06000E93 RID: 3731 RVA: 0x00032DE0 File Offset: 0x00030FE0
		internal static ODataBatchOperationReadStream CreateBatchOperationReadStream(ODataBatchReaderStream batchReaderStream, ODataBatchOperationHeaders headers, IODataBatchOperationListener operationListener)
		{
			string text;
			if (!headers.TryGetValue("Content-Length", out text))
			{
				return ODataBatchOperationReadStream.Create(batchReaderStream, operationListener);
			}
			int num = int.Parse(text, CultureInfo.InvariantCulture);
			if (num < 0)
			{
				throw new ODataException(Strings.ODataBatchReaderStream_InvalidContentLengthSpecified(text));
			}
			return ODataBatchOperationReadStream.Create(batchReaderStream, operationListener, num);
		}

		// Token: 0x06000E94 RID: 3732 RVA: 0x00032E28 File Offset: 0x00031028
		internal static ODataBatchOperationWriteStream CreateBatchOperationWriteStream(Stream outputStream, IODataBatchOperationListener operationListener)
		{
			return new ODataBatchOperationWriteStream(outputStream, operationListener);
		}

		// Token: 0x06000E95 RID: 3733 RVA: 0x00032E34 File Offset: 0x00031034
		internal static void EnsureArraySize(ref byte[] buffer, int numberOfBytesInBuffer, int requiredByteCount)
		{
			int num = buffer.Length - numberOfBytesInBuffer;
			if (requiredByteCount <= num)
			{
				return;
			}
			int num2 = requiredByteCount - num;
			byte[] src = buffer;
			buffer = new byte[buffer.Length + num2];
			Buffer.BlockCopy(src, 0, buffer, 0, numberOfBytesInBuffer);
		}
	}
}
