using System;
using System.Globalization;
using System.IO;

namespace Microsoft.Data.OData
{
	// Token: 0x02000264 RID: 612
	internal static class ODataBatchWriterUtils
	{
		// Token: 0x06001434 RID: 5172 RVA: 0x0004B27C File Offset: 0x0004947C
		internal static string CreateBatchBoundary(bool isResponse)
		{
			string format = isResponse ? "batchresponse_{0}" : "batch_{0}";
			return string.Format(CultureInfo.InvariantCulture, format, new object[]
			{
				Guid.NewGuid().ToString()
			});
		}

		// Token: 0x06001435 RID: 5173 RVA: 0x0004B2C4 File Offset: 0x000494C4
		internal static string CreateChangeSetBoundary(bool isResponse)
		{
			string format = isResponse ? "changesetresponse_{0}" : "changeset_{0}";
			return string.Format(CultureInfo.InvariantCulture, format, new object[]
			{
				Guid.NewGuid().ToString()
			});
		}

		// Token: 0x06001436 RID: 5174 RVA: 0x0004B30C File Offset: 0x0004950C
		internal static string CreateMultipartMixedContentType(string boundary)
		{
			return string.Format(CultureInfo.InvariantCulture, "{0}; {1}={2}", new object[]
			{
				"multipart/mixed",
				"boundary",
				boundary
			});
		}

		// Token: 0x06001437 RID: 5175 RVA: 0x0004B344 File Offset: 0x00049544
		internal static void WriteStartBoundary(TextWriter writer, string boundary, bool firstBoundary)
		{
			if (!firstBoundary)
			{
				writer.WriteLine();
			}
			writer.WriteLine("--{0}", boundary);
		}

		// Token: 0x06001438 RID: 5176 RVA: 0x0004B35B File Offset: 0x0004955B
		internal static void WriteEndBoundary(TextWriter writer, string boundary, bool missingStartBoundary)
		{
			if (!missingStartBoundary)
			{
				writer.WriteLine();
			}
			writer.Write("--{0}--", boundary);
		}

		// Token: 0x06001439 RID: 5177 RVA: 0x0004B374 File Offset: 0x00049574
		internal static void WriteRequestPreamble(TextWriter writer, string httpMethod, Uri uri)
		{
			writer.WriteLine("{0}: {1}", "Content-Type", "application/http");
			writer.WriteLine("{0}: {1}", "Content-Transfer-Encoding", "binary");
			writer.WriteLine();
			writer.WriteLine("{0} {1} {2}", httpMethod, UriUtilsCommon.UriToString(uri), "HTTP/1.1");
		}

		// Token: 0x0600143A RID: 5178 RVA: 0x0004B3C8 File Offset: 0x000495C8
		internal static void WriteResponsePreamble(TextWriter writer)
		{
			writer.WriteLine("{0}: {1}", "Content-Type", "application/http");
			writer.WriteLine("{0}: {1}", "Content-Transfer-Encoding", "binary");
			writer.WriteLine();
		}

		// Token: 0x0600143B RID: 5179 RVA: 0x0004B3FC File Offset: 0x000495FC
		internal static void WriteChangeSetPreamble(TextWriter writer, string changeSetBoundary)
		{
			string arg = ODataBatchWriterUtils.CreateMultipartMixedContentType(changeSetBoundary);
			writer.WriteLine("{0}: {1}", "Content-Type", arg);
			writer.WriteLine();
		}
	}
}
