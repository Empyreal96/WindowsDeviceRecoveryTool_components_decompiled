using System;
using System.IO;
using System.Net;
using Microsoft.WindowsAzure.Storage.Blob.Protocol;
using Microsoft.WindowsAzure.Storage.Core;
using Microsoft.WindowsAzure.Storage.Core.Executor;
using Microsoft.WindowsAzure.Storage.Core.Util;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;

namespace Microsoft.WindowsAzure.Storage.Blob
{
	// Token: 0x0200001C RID: 28
	internal static class CloudBlobSharedImpl
	{
		// Token: 0x06000521 RID: 1313 RVA: 0x0001152C File Offset: 0x0000F72C
		internal static void BlobOutputStreamCommitCallback(IAsyncResult result)
		{
			StorageAsyncResult<NullType> storageAsyncResult = (StorageAsyncResult<NullType>)result.AsyncState;
			CloudBlobStream cloudBlobStream = (CloudBlobStream)storageAsyncResult.OperationState;
			storageAsyncResult.UpdateCompletedSynchronously(result.CompletedSynchronously);
			try
			{
				cloudBlobStream.EndCommit(result);
				cloudBlobStream.Dispose();
				storageAsyncResult.OnComplete();
			}
			catch (Exception exception)
			{
				storageAsyncResult.OnComplete(exception);
			}
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x00011834 File Offset: 0x0000FA34
		internal static RESTCommand<NullType> GetBlobImpl(ICloudBlob blob, BlobAttributes attributes, Stream destStream, long? offset, long? length, AccessCondition accessCondition, BlobRequestOptions options)
		{
			string lockedETag = null;
			AccessCondition lockedAccessCondition = null;
			bool isRangeGet = offset != null;
			bool arePropertiesPopulated = false;
			string storedMD5 = null;
			long startingOffset = (offset != null) ? offset.Value : 0L;
			long? startingLength = length;
			long? validateLength = null;
			RESTCommand<NullType> getCmd = new RESTCommand<NullType>(blob.ServiceClient.Credentials, attributes.StorageUri);
			options.ApplyToStorageCommand<NullType>(getCmd);
			getCmd.CommandLocationMode = CommandLocationMode.PrimaryOrSecondary;
			getCmd.RetrieveResponseStream = true;
			getCmd.DestinationStream = destStream;
			getCmd.CalculateMd5ForResponseStream = !options.DisableContentMD5Validation.Value;
			getCmd.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => BlobHttpWebRequestFactory.Get(uri, serverTimeout, attributes.SnapshotTime, offset, length, options.UseTransactionalMD5.Value, accessCondition, useVersionHeader, ctx));
			getCmd.SignRequest = new Action<HttpWebRequest, OperationContext>(blob.ServiceClient.AuthenticationHandler.SignRequest);
			getCmd.RecoveryAction = delegate(StorageCommandBase<NullType> cmd, Exception ex, OperationContext ctx)
			{
				if (lockedAccessCondition == null && !string.IsNullOrEmpty(lockedETag))
				{
					lockedAccessCondition = AccessCondition.GenerateIfMatchCondition(lockedETag);
					if (accessCondition != null)
					{
						lockedAccessCondition.LeaseId = accessCondition.LeaseId;
					}
				}
				if (cmd.StreamCopyState != null)
				{
					offset = new long?(startingOffset + cmd.StreamCopyState.Length);
					if (startingLength != null)
					{
						length = new long?(startingLength.Value - cmd.StreamCopyState.Length);
					}
				}
				getCmd.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext context) => BlobHttpWebRequestFactory.Get(uri, serverTimeout, attributes.SnapshotTime, offset, length, options.UseTransactionalMD5.Value && !arePropertiesPopulated, lockedAccessCondition ?? accessCondition, useVersionHeader, context));
			};
			getCmd.PreProcessResponse = delegate(RESTCommand<NullType> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx)
			{
				HttpResponseParsers.ProcessExpectedStatusCodeNoException<NullType>((offset != null) ? HttpStatusCode.PartialContent : HttpStatusCode.OK, resp, NullType.Value, cmd, ex);
				if (!arePropertiesPopulated)
				{
					CloudBlobSharedImpl.UpdateAfterFetchAttributes(attributes, resp, isRangeGet);
					storedMD5 = resp.Headers[HttpResponseHeader.ContentMd5];
					if (!options.DisableContentMD5Validation.Value && options.UseTransactionalMD5.Value && string.IsNullOrEmpty(storedMD5))
					{
						throw new StorageException(cmd.CurrentResult, "MD5 does not exist. If you do not want to force validation, please disable UseTransactionalMD5.", null)
						{
							IsRetryable = false
						};
					}
					getCmd.CommandLocationMode = ((cmd.CurrentResult.TargetLocation == StorageLocation.Primary) ? CommandLocationMode.PrimaryOnly : CommandLocationMode.SecondaryOnly);
					lockedETag = attributes.Properties.ETag;
					if (resp.ContentLength >= 0L)
					{
						validateLength = new long?(resp.ContentLength);
					}
					arePropertiesPopulated = true;
				}
				return NullType.Value;
			};
			getCmd.PostProcessResponse = delegate(RESTCommand<NullType> cmd, HttpWebResponse resp, OperationContext ctx)
			{
				HttpResponseParsers.ValidateResponseStreamMd5AndLength<NullType>(validateLength, storedMD5, cmd);
				return NullType.Value;
			};
			return getCmd;
		}

		// Token: 0x06000523 RID: 1315 RVA: 0x00011A1C File Offset: 0x0000FC1C
		internal static RESTCommand<NullType> FetchAttributesImpl(ICloudBlob blob, BlobAttributes attributes, AccessCondition accessCondition, BlobRequestOptions options)
		{
			RESTCommand<NullType> restcommand = new RESTCommand<NullType>(blob.ServiceClient.Credentials, attributes.StorageUri);
			options.ApplyToStorageCommand<NullType>(restcommand);
			restcommand.CommandLocationMode = CommandLocationMode.PrimaryOrSecondary;
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => BlobHttpWebRequestFactory.GetProperties(uri, serverTimeout, attributes.SnapshotTime, accessCondition, useVersionHeader, ctx));
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(blob.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = delegate(RESTCommand<NullType> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx)
			{
				HttpResponseParsers.ProcessExpectedStatusCodeNoException<NullType>(HttpStatusCode.OK, resp, NullType.Value, cmd, ex);
				CloudBlobSharedImpl.UpdateAfterFetchAttributes(attributes, resp, false);
				return NullType.Value;
			};
			return restcommand;
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x00011AF8 File Offset: 0x0000FCF8
		internal static RESTCommand<bool> ExistsImpl(ICloudBlob blob, BlobAttributes attributes, BlobRequestOptions options, bool primaryOnly)
		{
			RESTCommand<bool> restcommand = new RESTCommand<bool>(blob.ServiceClient.Credentials, attributes.StorageUri);
			options.ApplyToStorageCommand<bool>(restcommand);
			restcommand.CommandLocationMode = (primaryOnly ? CommandLocationMode.PrimaryOnly : CommandLocationMode.PrimaryOrSecondary);
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => BlobHttpWebRequestFactory.GetProperties(uri, serverTimeout, attributes.SnapshotTime, null, useVersionHeader, ctx));
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(blob.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = delegate(RESTCommand<bool> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx)
			{
				if (resp.StatusCode == HttpStatusCode.NotFound)
				{
					return false;
				}
				HttpResponseParsers.ProcessExpectedStatusCodeNoException<bool>(HttpStatusCode.OK, resp, true, cmd, ex);
				CloudBlobSharedImpl.UpdateAfterFetchAttributes(attributes, resp, false);
				return true;
			};
			return restcommand;
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x00011BDC File Offset: 0x0000FDDC
		internal static RESTCommand<NullType> SetMetadataImpl(ICloudBlob blob, BlobAttributes attributes, AccessCondition accessCondition, BlobRequestOptions options)
		{
			RESTCommand<NullType> restcommand = new RESTCommand<NullType>(blob.ServiceClient.Credentials, attributes.StorageUri);
			options.ApplyToStorageCommand<NullType>(restcommand);
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => BlobHttpWebRequestFactory.SetMetadata(uri, serverTimeout, accessCondition, useVersionHeader, ctx));
			restcommand.SetHeaders = delegate(HttpWebRequest r, OperationContext ctx)
			{
				BlobHttpWebRequestFactory.AddMetadata(r, attributes.Metadata);
			};
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(blob.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = delegate(RESTCommand<NullType> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx)
			{
				HttpResponseParsers.ProcessExpectedStatusCodeNoException<NullType>(HttpStatusCode.OK, resp, NullType.Value, cmd, ex);
				CloudBlobSharedImpl.UpdateETagLMTLengthAndSequenceNumber(attributes, resp, false);
				return NullType.Value;
			};
			return restcommand;
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x00011CD4 File Offset: 0x0000FED4
		internal static RESTCommand<NullType> SetPropertiesImpl(ICloudBlob blob, BlobAttributes attributes, AccessCondition accessCondition, BlobRequestOptions options)
		{
			RESTCommand<NullType> restcommand = new RESTCommand<NullType>(blob.ServiceClient.Credentials, attributes.StorageUri);
			options.ApplyToStorageCommand<NullType>(restcommand);
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => BlobHttpWebRequestFactory.SetProperties(uri, serverTimeout, attributes.Properties, accessCondition, useVersionHeader, ctx));
			restcommand.SetHeaders = delegate(HttpWebRequest r, OperationContext ctx)
			{
				BlobHttpWebRequestFactory.AddMetadata(r, attributes.Metadata);
			};
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(blob.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = delegate(RESTCommand<NullType> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx)
			{
				HttpResponseParsers.ProcessExpectedStatusCodeNoException<NullType>(HttpStatusCode.OK, resp, NullType.Value, cmd, ex);
				CloudBlobSharedImpl.UpdateETagLMTLengthAndSequenceNumber(attributes, resp, false);
				return NullType.Value;
			};
			return restcommand;
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x00011DAC File Offset: 0x0000FFAC
		internal static RESTCommand<NullType> DeleteBlobImpl(ICloudBlob blob, BlobAttributes attributes, DeleteSnapshotsOption deleteSnapshotsOption, AccessCondition accessCondition, BlobRequestOptions options)
		{
			RESTCommand<NullType> restcommand = new RESTCommand<NullType>(blob.ServiceClient.Credentials, attributes.StorageUri);
			options.ApplyToStorageCommand<NullType>(restcommand);
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => BlobHttpWebRequestFactory.Delete(uri, serverTimeout, attributes.SnapshotTime, deleteSnapshotsOption, accessCondition, useVersionHeader, ctx));
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(blob.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = ((RESTCommand<NullType> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx) => HttpResponseParsers.ProcessExpectedStatusCodeNoException<NullType>(HttpStatusCode.Accepted, resp, NullType.Value, cmd, ex));
			return restcommand;
		}

		// Token: 0x06000528 RID: 1320 RVA: 0x00011EB4 File Offset: 0x000100B4
		internal static RESTCommand<string> AcquireLeaseImpl(ICloudBlob blob, BlobAttributes attributes, TimeSpan? leaseTime, string proposedLeaseId, AccessCondition accessCondition, BlobRequestOptions options)
		{
			int leaseDuration = -1;
			if (leaseTime != null)
			{
				CommonUtility.AssertInBounds<TimeSpan>("leaseTime", leaseTime.Value, TimeSpan.FromSeconds(1.0), TimeSpan.MaxValue);
				leaseDuration = (int)leaseTime.Value.TotalSeconds;
			}
			RESTCommand<string> restcommand = new RESTCommand<string>(blob.ServiceClient.Credentials, attributes.StorageUri);
			options.ApplyToStorageCommand<string>(restcommand);
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => BlobHttpWebRequestFactory.Lease(uri, serverTimeout, LeaseAction.Acquire, proposedLeaseId, new int?(leaseDuration), null, accessCondition, useVersionHeader, ctx));
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(blob.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = delegate(RESTCommand<string> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx)
			{
				HttpResponseParsers.ProcessExpectedStatusCodeNoException<string>(HttpStatusCode.Created, resp, null, cmd, ex);
				CloudBlobSharedImpl.UpdateETagLMTLengthAndSequenceNumber(attributes, resp, false);
				return BlobHttpResponseParsers.GetLeaseId(resp);
			};
			return restcommand;
		}

		// Token: 0x06000529 RID: 1321 RVA: 0x00011FF4 File Offset: 0x000101F4
		internal static RESTCommand<NullType> RenewLeaseImpl(ICloudBlob blob, BlobAttributes attributes, AccessCondition accessCondition, BlobRequestOptions options)
		{
			CommonUtility.AssertNotNull("accessCondition", accessCondition);
			if (accessCondition.LeaseId == null)
			{
				throw new ArgumentException("A lease ID must be specified when renewing a lease.", "accessCondition");
			}
			RESTCommand<NullType> restcommand = new RESTCommand<NullType>(blob.ServiceClient.Credentials, attributes.StorageUri);
			options.ApplyToStorageCommand<NullType>(restcommand);
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => BlobHttpWebRequestFactory.Lease(uri, serverTimeout, LeaseAction.Renew, null, null, null, accessCondition, useVersionHeader, ctx));
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(blob.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = delegate(RESTCommand<NullType> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx)
			{
				HttpResponseParsers.ProcessExpectedStatusCodeNoException<NullType>(HttpStatusCode.OK, resp, NullType.Value, cmd, ex);
				CloudBlobSharedImpl.UpdateETagLMTLengthAndSequenceNumber(attributes, resp, false);
				return NullType.Value;
			};
			return restcommand;
		}

		// Token: 0x0600052A RID: 1322 RVA: 0x0001210C File Offset: 0x0001030C
		internal static RESTCommand<string> ChangeLeaseImpl(ICloudBlob blob, BlobAttributes attributes, string proposedLeaseId, AccessCondition accessCondition, BlobRequestOptions options)
		{
			CommonUtility.AssertNotNull("accessCondition", accessCondition);
			CommonUtility.AssertNotNull("proposedLeaseId", proposedLeaseId);
			if (accessCondition.LeaseId == null)
			{
				throw new ArgumentException("A lease ID must be specified when changing a lease.", "accessCondition");
			}
			RESTCommand<string> restcommand = new RESTCommand<string>(blob.ServiceClient.Credentials, attributes.StorageUri);
			options.ApplyToStorageCommand<string>(restcommand);
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => BlobHttpWebRequestFactory.Lease(uri, serverTimeout, LeaseAction.Change, proposedLeaseId, null, null, accessCondition, useVersionHeader, ctx));
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(blob.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = delegate(RESTCommand<string> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx)
			{
				HttpResponseParsers.ProcessExpectedStatusCodeNoException<string>(HttpStatusCode.OK, resp, null, cmd, ex);
				CloudBlobSharedImpl.UpdateETagLMTLengthAndSequenceNumber(attributes, resp, false);
				return BlobHttpResponseParsers.GetLeaseId(resp);
			};
			return restcommand;
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x0001223C File Offset: 0x0001043C
		internal static RESTCommand<NullType> ReleaseLeaseImpl(ICloudBlob blob, BlobAttributes attributes, AccessCondition accessCondition, BlobRequestOptions options)
		{
			CommonUtility.AssertNotNull("accessCondition", accessCondition);
			if (accessCondition.LeaseId == null)
			{
				throw new ArgumentException("A lease ID must be specified when releasing a lease.", "accessCondition");
			}
			RESTCommand<NullType> restcommand = new RESTCommand<NullType>(blob.ServiceClient.Credentials, attributes.StorageUri);
			options.ApplyToStorageCommand<NullType>(restcommand);
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => BlobHttpWebRequestFactory.Lease(uri, serverTimeout, LeaseAction.Release, null, null, null, accessCondition, useVersionHeader, ctx));
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(blob.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = delegate(RESTCommand<NullType> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx)
			{
				HttpResponseParsers.ProcessExpectedStatusCodeNoException<NullType>(HttpStatusCode.OK, resp, NullType.Value, cmd, ex);
				CloudBlobSharedImpl.UpdateETagLMTLengthAndSequenceNumber(attributes, resp, false);
				return NullType.Value;
			};
			return restcommand;
		}

		// Token: 0x0600052C RID: 1324 RVA: 0x00012384 File Offset: 0x00010584
		internal static RESTCommand<TimeSpan> BreakLeaseImpl(ICloudBlob blob, BlobAttributes attributes, TimeSpan? breakPeriod, AccessCondition accessCondition, BlobRequestOptions options)
		{
			int? breakSeconds = null;
			if (breakPeriod != null)
			{
				CommonUtility.AssertInBounds<TimeSpan>("breakPeriod", breakPeriod.Value, TimeSpan.Zero, TimeSpan.MaxValue);
				breakSeconds = new int?((int)breakPeriod.Value.TotalSeconds);
			}
			RESTCommand<TimeSpan> restcommand = new RESTCommand<TimeSpan>(blob.ServiceClient.Credentials, attributes.StorageUri);
			options.ApplyToStorageCommand<TimeSpan>(restcommand);
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => BlobHttpWebRequestFactory.Lease(uri, serverTimeout, LeaseAction.Break, null, null, breakSeconds, accessCondition, useVersionHeader, ctx));
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(blob.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = delegate(RESTCommand<TimeSpan> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx)
			{
				HttpResponseParsers.ProcessExpectedStatusCodeNoException<TimeSpan>(HttpStatusCode.Accepted, resp, TimeSpan.Zero, cmd, ex);
				CloudBlobSharedImpl.UpdateETagLMTLengthAndSequenceNumber(attributes, resp, false);
				int? remainingLeaseTime = BlobHttpResponseParsers.GetRemainingLeaseTime(resp);
				if (remainingLeaseTime == null)
				{
					throw new StorageException(cmd.CurrentResult, "Valid lease time expected but not received from the service.", null);
				}
				return TimeSpan.FromSeconds((double)remainingLeaseTime.Value);
			};
			return restcommand;
		}

		// Token: 0x0600052D RID: 1325 RVA: 0x000124F0 File Offset: 0x000106F0
		internal static RESTCommand<string> StartCopyFromBlobImpl(ICloudBlob blob, BlobAttributes attributes, Uri source, AccessCondition sourceAccessCondition, AccessCondition destAccessCondition, BlobRequestOptions options)
		{
			if (sourceAccessCondition != null && !string.IsNullOrEmpty(sourceAccessCondition.LeaseId))
			{
				throw new ArgumentException("A lease condition cannot be specified on the source of a copy.", "sourceAccessCondition");
			}
			RESTCommand<string> restcommand = new RESTCommand<string>(blob.ServiceClient.Credentials, attributes.StorageUri);
			options.ApplyToStorageCommand<string>(restcommand);
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => BlobHttpWebRequestFactory.CopyFrom(uri, serverTimeout, source, sourceAccessCondition, destAccessCondition, useVersionHeader, ctx));
			restcommand.SetHeaders = delegate(HttpWebRequest r, OperationContext ctx)
			{
				BlobHttpWebRequestFactory.AddMetadata(r, attributes.Metadata);
			};
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(blob.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = delegate(RESTCommand<string> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx)
			{
				HttpResponseParsers.ProcessExpectedStatusCodeNoException<string>(HttpStatusCode.Accepted, resp, null, cmd, ex);
				CopyState copyAttributes = BlobHttpResponseParsers.GetCopyAttributes(resp);
				attributes.Properties = BlobHttpResponseParsers.GetProperties(resp);
				attributes.Metadata = BlobHttpResponseParsers.GetMetadata(resp);
				attributes.CopyState = copyAttributes;
				return copyAttributes.CopyId;
			};
			return restcommand;
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x000125F8 File Offset: 0x000107F8
		internal static RESTCommand<NullType> AbortCopyImpl(ICloudBlob blob, BlobAttributes attributes, string copyId, AccessCondition accessCondition, BlobRequestOptions options)
		{
			CommonUtility.AssertNotNull("copyId", copyId);
			RESTCommand<NullType> restcommand = new RESTCommand<NullType>(blob.ServiceClient.Credentials, attributes.StorageUri);
			options.ApplyToStorageCommand<NullType>(restcommand);
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => BlobHttpWebRequestFactory.AbortCopy(uri, serverTimeout, copyId, accessCondition, useVersionHeader, ctx));
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(blob.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = ((RESTCommand<NullType> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx) => HttpResponseParsers.ProcessExpectedStatusCodeNoException<NullType>(HttpStatusCode.NoContent, resp, NullType.Value, cmd, ex));
			return restcommand;
		}

		// Token: 0x0600052F RID: 1327 RVA: 0x0001269C File Offset: 0x0001089C
		internal static void UpdateAfterFetchAttributes(BlobAttributes attributes, HttpWebResponse response, bool ignoreMD5)
		{
			BlobProperties properties = BlobHttpResponseParsers.GetProperties(response);
			if (attributes.Properties.BlobType != BlobType.Unspecified && attributes.Properties.BlobType != properties.BlobType)
			{
				throw new InvalidOperationException("Blob type of the blob reference doesn't match blob type of the blob.");
			}
			if (ignoreMD5)
			{
				properties.ContentMD5 = attributes.Properties.ContentMD5;
			}
			attributes.Properties = properties;
			attributes.Metadata = BlobHttpResponseParsers.GetMetadata(response);
			attributes.CopyState = BlobHttpResponseParsers.GetCopyAttributes(response);
		}

		// Token: 0x06000530 RID: 1328 RVA: 0x00012710 File Offset: 0x00010910
		internal static void UpdateETagLMTLengthAndSequenceNumber(BlobAttributes attributes, HttpWebResponse response, bool updateLength)
		{
			BlobProperties properties = BlobHttpResponseParsers.GetProperties(response);
			attributes.Properties.ETag = (properties.ETag ?? attributes.Properties.ETag);
			BlobProperties properties2 = attributes.Properties;
			DateTimeOffset? lastModified = properties.LastModified;
			properties2.LastModified = ((lastModified != null) ? new DateTimeOffset?(lastModified.GetValueOrDefault()) : attributes.Properties.LastModified);
			BlobProperties properties3 = attributes.Properties;
			long? pageBlobSequenceNumber = properties.PageBlobSequenceNumber;
			properties3.PageBlobSequenceNumber = ((pageBlobSequenceNumber != null) ? new long?(pageBlobSequenceNumber.GetValueOrDefault()) : attributes.Properties.PageBlobSequenceNumber);
			if (updateLength)
			{
				attributes.Properties.Length = properties.Length;
			}
		}

		// Token: 0x06000531 RID: 1329 RVA: 0x000127C0 File Offset: 0x000109C0
		internal static Uri SourceBlobToUri(ICloudBlob source)
		{
			CommonUtility.AssertNotNull("source", source);
			return source.ServiceClient.Credentials.TransformUri(source.SnapshotQualifiedUri);
		}
	}
}
