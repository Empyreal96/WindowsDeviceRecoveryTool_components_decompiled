using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using Microsoft.WindowsAzure.Storage.Core.Util;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;

namespace Microsoft.WindowsAzure.Storage.Blob.Protocol
{
	// Token: 0x0200001F RID: 31
	public static class BlobHttpResponseParsers
	{
		// Token: 0x06000641 RID: 1601 RVA: 0x00017451 File Offset: 0x00015651
		public static string GetRequestId(HttpWebResponse response)
		{
			return Response.GetRequestId(response);
		}

		// Token: 0x06000642 RID: 1602 RVA: 0x0001745C File Offset: 0x0001565C
		public static BlobProperties GetProperties(HttpWebResponse response)
		{
			CommonUtility.AssertNotNull("response", response);
			BlobProperties blobProperties = new BlobProperties();
			blobProperties.ETag = HttpResponseParsers.GetETag(response);
			blobProperties.LastModified = new DateTimeOffset?(response.LastModified.ToUniversalTime());
			blobProperties.ContentLanguage = response.Headers[HttpResponseHeader.ContentLanguage];
			blobProperties.ContentDisposition = response.Headers["Content-Disposition"];
			blobProperties.ContentEncoding = response.Headers[HttpResponseHeader.ContentEncoding];
			blobProperties.ContentMD5 = response.Headers[HttpResponseHeader.ContentMd5];
			blobProperties.ContentType = response.Headers[HttpResponseHeader.ContentType];
			blobProperties.CacheControl = response.Headers[HttpResponseHeader.CacheControl];
			string value = response.Headers["x-ms-blob-type"];
			if (!string.IsNullOrEmpty(value))
			{
				blobProperties.BlobType = (BlobType)Enum.Parse(typeof(BlobType), value, true);
			}
			blobProperties.LeaseStatus = BlobHttpResponseParsers.GetLeaseStatus(response);
			blobProperties.LeaseState = BlobHttpResponseParsers.GetLeaseState(response);
			blobProperties.LeaseDuration = BlobHttpResponseParsers.GetLeaseDuration(response);
			string text = response.Headers[HttpResponseHeader.ContentRange];
			string text2 = response.Headers["Content-Length"];
			string text3 = response.Headers["x-ms-blob-content-length"];
			if (!string.IsNullOrEmpty(text))
			{
				blobProperties.Length = long.Parse(text.Split(new char[]
				{
					'/'
				})[1], CultureInfo.InvariantCulture);
			}
			else if (!string.IsNullOrEmpty(text3))
			{
				blobProperties.Length = long.Parse(text3, CultureInfo.InvariantCulture);
			}
			else if (!string.IsNullOrEmpty(text2))
			{
				blobProperties.Length = long.Parse(text2, CultureInfo.InvariantCulture);
			}
			else
			{
				blobProperties.Length = response.ContentLength;
			}
			string text4 = response.Headers["x-ms-blob-sequence-number"];
			if (!string.IsNullOrEmpty(text4))
			{
				blobProperties.PageBlobSequenceNumber = new long?(long.Parse(text4, CultureInfo.InvariantCulture));
			}
			string text5 = response.Headers["x-ms-blob-committed-block-count"];
			if (!string.IsNullOrEmpty(text5))
			{
				blobProperties.AppendBlobCommittedBlockCount = new int?(int.Parse(text5, CultureInfo.InvariantCulture));
			}
			return blobProperties;
		}

		// Token: 0x06000643 RID: 1603 RVA: 0x00017680 File Offset: 0x00015880
		public static LeaseStatus GetLeaseStatus(HttpWebResponse response)
		{
			CommonUtility.AssertNotNull("response", response);
			string leaseStatus = response.Headers["x-ms-lease-status"];
			return BlobHttpResponseParsers.GetLeaseStatus(leaseStatus);
		}

		// Token: 0x06000644 RID: 1604 RVA: 0x000176B0 File Offset: 0x000158B0
		public static LeaseState GetLeaseState(HttpWebResponse response)
		{
			CommonUtility.AssertNotNull("response", response);
			string leaseState = response.Headers["x-ms-lease-state"];
			return BlobHttpResponseParsers.GetLeaseState(leaseState);
		}

		// Token: 0x06000645 RID: 1605 RVA: 0x000176E0 File Offset: 0x000158E0
		public static LeaseDuration GetLeaseDuration(HttpWebResponse response)
		{
			CommonUtility.AssertNotNull("response", response);
			string leaseDuration = response.Headers["x-ms-lease-duration"];
			return BlobHttpResponseParsers.GetLeaseDuration(leaseDuration);
		}

		// Token: 0x06000646 RID: 1606 RVA: 0x0001770F File Offset: 0x0001590F
		public static string GetLeaseId(HttpWebResponse response)
		{
			CommonUtility.AssertNotNull("response", response);
			return response.Headers["x-ms-lease-id"];
		}

		// Token: 0x06000647 RID: 1607 RVA: 0x0001772C File Offset: 0x0001592C
		public static int? GetRemainingLeaseTime(HttpWebResponse response)
		{
			CommonUtility.AssertNotNull("response", response);
			int value;
			if (int.TryParse(response.Headers["x-ms-lease-time"], out value))
			{
				return new int?(value);
			}
			return null;
		}

		// Token: 0x06000648 RID: 1608 RVA: 0x0001776D File Offset: 0x0001596D
		public static IDictionary<string, string> GetMetadata(HttpWebResponse response)
		{
			return HttpResponseParsers.GetMetadata(response);
		}

		// Token: 0x06000649 RID: 1609 RVA: 0x00017778 File Offset: 0x00015978
		public static CopyState GetCopyAttributes(HttpWebResponse response)
		{
			CommonUtility.AssertNotNull("response", response);
			string text = response.Headers["x-ms-copy-status"];
			if (!string.IsNullOrEmpty(text))
			{
				return BlobHttpResponseParsers.GetCopyAttributes(text, response.Headers["x-ms-copy-id"], response.Headers["x-ms-copy-source"], response.Headers["x-ms-copy-progress"], response.Headers["x-ms-copy-completion-time"], response.Headers["x-ms-copy-status-description"]);
			}
			return null;
		}

		// Token: 0x0600064A RID: 1610 RVA: 0x00017801 File Offset: 0x00015A01
		public static string GetSnapshotTime(HttpWebResponse response)
		{
			CommonUtility.AssertNotNull("response", response);
			return response.Headers["x-ms-snapshot"];
		}

		// Token: 0x0600064B RID: 1611 RVA: 0x0001781E File Offset: 0x00015A1E
		public static ServiceProperties ReadServiceProperties(Stream inputStream)
		{
			return HttpResponseParsers.ReadServiceProperties(inputStream);
		}

		// Token: 0x0600064C RID: 1612 RVA: 0x00017826 File Offset: 0x00015A26
		public static ServiceStats ReadServiceStats(Stream inputStream)
		{
			return HttpResponseParsers.ReadServiceStats(inputStream);
		}

		// Token: 0x0600064D RID: 1613 RVA: 0x00017830 File Offset: 0x00015A30
		internal static LeaseStatus GetLeaseStatus(string leaseStatus)
		{
			if (!string.IsNullOrEmpty(leaseStatus))
			{
				if (leaseStatus != null)
				{
					if (leaseStatus == "locked")
					{
						return LeaseStatus.Locked;
					}
					if (leaseStatus == "unlocked")
					{
						return LeaseStatus.Unlocked;
					}
				}
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Invalid lease status in response: '{0}'", new object[]
				{
					leaseStatus
				}), "leaseStatus");
			}
			return LeaseStatus.Unspecified;
		}

		// Token: 0x0600064E RID: 1614 RVA: 0x00017894 File Offset: 0x00015A94
		internal static LeaseState GetLeaseState(string leaseState)
		{
			if (!string.IsNullOrEmpty(leaseState))
			{
				if (leaseState != null)
				{
					if (leaseState == "available")
					{
						return LeaseState.Available;
					}
					if (leaseState == "leased")
					{
						return LeaseState.Leased;
					}
					if (leaseState == "expired")
					{
						return LeaseState.Expired;
					}
					if (leaseState == "breaking")
					{
						return LeaseState.Breaking;
					}
					if (leaseState == "broken")
					{
						return LeaseState.Broken;
					}
				}
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Invalid lease state in response: '{0}'", new object[]
				{
					leaseState
				}), "leaseState");
			}
			return LeaseState.Unspecified;
		}

		// Token: 0x0600064F RID: 1615 RVA: 0x00017924 File Offset: 0x00015B24
		internal static LeaseDuration GetLeaseDuration(string leaseDuration)
		{
			if (!string.IsNullOrEmpty(leaseDuration))
			{
				if (leaseDuration != null)
				{
					if (leaseDuration == "fixed")
					{
						return LeaseDuration.Fixed;
					}
					if (leaseDuration == "infinite")
					{
						return LeaseDuration.Infinite;
					}
				}
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Invalid lease duration in response: '{0}'", new object[]
				{
					leaseDuration
				}), "leaseDuration");
			}
			return LeaseDuration.Unspecified;
		}

		// Token: 0x06000650 RID: 1616 RVA: 0x00017988 File Offset: 0x00015B88
		internal static CopyState GetCopyAttributes(string copyStatusString, string copyId, string copySourceString, string copyProgressString, string copyCompletionTimeString, string copyStatusDescription)
		{
			CopyState copyState = new CopyState
			{
				CopyId = copyId,
				StatusDescription = copyStatusDescription
			};
			if (copyStatusString != null)
			{
				if (copyStatusString == "success")
				{
					copyState.Status = CopyStatus.Success;
					goto IL_7D;
				}
				if (copyStatusString == "pending")
				{
					copyState.Status = CopyStatus.Pending;
					goto IL_7D;
				}
				if (copyStatusString == "aborted")
				{
					copyState.Status = CopyStatus.Aborted;
					goto IL_7D;
				}
				if (copyStatusString == "failed")
				{
					copyState.Status = CopyStatus.Failed;
					goto IL_7D;
				}
			}
			copyState.Status = CopyStatus.Invalid;
			IL_7D:
			if (!string.IsNullOrEmpty(copyProgressString))
			{
				string[] array = copyProgressString.Split(new char[]
				{
					'/'
				});
				copyState.BytesCopied = new long?(long.Parse(array[0], CultureInfo.InvariantCulture));
				copyState.TotalBytes = new long?(long.Parse(array[1], CultureInfo.InvariantCulture));
			}
			if (!string.IsNullOrEmpty(copySourceString))
			{
				copyState.Source = new Uri(copySourceString);
			}
			if (!string.IsNullOrEmpty(copyCompletionTimeString))
			{
				copyState.CompletionTime = new DateTimeOffset?(copyCompletionTimeString.ToUTCTime());
			}
			return copyState;
		}
	}
}
