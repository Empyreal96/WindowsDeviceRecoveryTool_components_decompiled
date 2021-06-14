using System;
using System.Globalization;
using System.Net;
using Microsoft.WindowsAzure.Storage.Core.Util;

namespace Microsoft.WindowsAzure.Storage.Shared.Protocol
{
	// Token: 0x02000053 RID: 83
	internal static class WebRequestExtensions
	{
		// Token: 0x06000D0A RID: 3338 RVA: 0x0003061C File Offset: 0x0002E81C
		internal static void AddLeaseId(this HttpWebRequest request, string leaseId)
		{
			if (leaseId != null)
			{
				request.AddOptionalHeader("x-ms-lease-id", leaseId);
			}
		}

		// Token: 0x06000D0B RID: 3339 RVA: 0x0003062D File Offset: 0x0002E82D
		internal static void AddOptionalHeader(this HttpWebRequest request, string name, string value)
		{
			if (!string.IsNullOrEmpty(value))
			{
				request.Headers.Add(name, value);
			}
		}

		// Token: 0x06000D0C RID: 3340 RVA: 0x00030644 File Offset: 0x0002E844
		internal static void AddOptionalHeader(this HttpWebRequest request, string name, int? value)
		{
			if (value != null)
			{
				request.Headers.Add(name, value.Value.ToString(CultureInfo.InvariantCulture));
			}
		}

		// Token: 0x06000D0D RID: 3341 RVA: 0x0003067C File Offset: 0x0002E87C
		internal static void AddOptionalHeader(this HttpWebRequest request, string name, long? value)
		{
			if (value != null)
			{
				request.Headers.Add(name, value.Value.ToString(CultureInfo.InvariantCulture));
			}
		}

		// Token: 0x06000D0E RID: 3342 RVA: 0x000306B2 File Offset: 0x0002E8B2
		internal static void ApplyLeaseId(this HttpWebRequest request, AccessCondition accessCondition)
		{
			if (accessCondition != null && !string.IsNullOrEmpty(accessCondition.LeaseId))
			{
				request.Headers["x-ms-lease-id"] = accessCondition.LeaseId;
			}
		}

		// Token: 0x06000D0F RID: 3343 RVA: 0x000306DA File Offset: 0x0002E8DA
		internal static void ApplySequenceNumberCondition(this HttpWebRequest request, AccessCondition accessCondition)
		{
			if (accessCondition != null)
			{
				request.AddOptionalHeader("x-ms-if-sequence-number-le", accessCondition.IfSequenceNumberLessThanOrEqual);
				request.AddOptionalHeader("x-ms-if-sequence-number-lt", accessCondition.IfSequenceNumberLessThan);
				request.AddOptionalHeader("x-ms-if-sequence-number-eq", accessCondition.IfSequenceNumberEqual);
			}
		}

		// Token: 0x06000D10 RID: 3344 RVA: 0x00030712 File Offset: 0x0002E912
		internal static void ApplyAppendCondition(this HttpWebRequest request, AccessCondition accessCondition)
		{
			if (accessCondition != null)
			{
				request.AddOptionalHeader("x-ms-blob-condition-maxsize", accessCondition.IfMaxSizeLessThanOrEqual);
				request.AddOptionalHeader("x-ms-blob-condition-appendpos", accessCondition.IfAppendPositionEqual);
			}
		}

		// Token: 0x06000D11 RID: 3345 RVA: 0x0003073C File Offset: 0x0002E93C
		internal static void ApplyAccessCondition(this HttpWebRequest request, AccessCondition accessCondition)
		{
			if (accessCondition != null)
			{
				if (!string.IsNullOrEmpty(accessCondition.IfMatchETag))
				{
					request.Headers[HttpRequestHeader.IfMatch] = accessCondition.IfMatchETag;
				}
				if (!string.IsNullOrEmpty(accessCondition.IfNoneMatchETag))
				{
					request.Headers[HttpRequestHeader.IfNoneMatch] = accessCondition.IfNoneMatchETag;
				}
				if (accessCondition.IfModifiedSinceTime != null)
				{
					request.IfModifiedSince = accessCondition.IfModifiedSinceTime.Value.UtcDateTime;
				}
				if (accessCondition.IfNotModifiedSinceTime != null)
				{
					request.Headers[HttpRequestHeader.IfUnmodifiedSince] = HttpWebUtility.ConvertDateTimeToHttpString(accessCondition.IfNotModifiedSinceTime.Value);
				}
				request.ApplyLeaseId(accessCondition);
			}
		}

		// Token: 0x06000D12 RID: 3346 RVA: 0x000307F4 File Offset: 0x0002E9F4
		internal static void ApplyAccessConditionToSource(this HttpWebRequest request, AccessCondition accessCondition)
		{
			if (accessCondition != null)
			{
				if (!string.IsNullOrEmpty(accessCondition.IfMatchETag))
				{
					request.Headers["x-ms-source-if-match"] = accessCondition.IfMatchETag;
				}
				if (!string.IsNullOrEmpty(accessCondition.IfNoneMatchETag))
				{
					request.Headers["x-ms-source-if-none-match"] = accessCondition.IfNoneMatchETag;
				}
				if (accessCondition.IfModifiedSinceTime != null)
				{
					request.Headers["x-ms-source-if-modified-since"] = HttpWebUtility.ConvertDateTimeToHttpString(accessCondition.IfModifiedSinceTime.Value);
				}
				if (accessCondition.IfNotModifiedSinceTime != null)
				{
					request.Headers["x-ms-source-if-unmodified-since"] = HttpWebUtility.ConvertDateTimeToHttpString(accessCondition.IfNotModifiedSinceTime.Value);
				}
				if (!string.IsNullOrEmpty(accessCondition.LeaseId))
				{
					throw new InvalidOperationException("A lease condition cannot be specified on the source of a copy.");
				}
			}
		}
	}
}
