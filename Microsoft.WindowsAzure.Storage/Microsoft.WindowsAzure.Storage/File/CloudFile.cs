using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Core;
using Microsoft.WindowsAzure.Storage.Core.Auth;
using Microsoft.WindowsAzure.Storage.Core.Executor;
using Microsoft.WindowsAzure.Storage.Core.Util;
using Microsoft.WindowsAzure.Storage.File.Protocol;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;

namespace Microsoft.WindowsAzure.Storage.File
{
	// Token: 0x02000024 RID: 36
	public sealed class CloudFile : IListFileItem
	{
		// Token: 0x060006AB RID: 1707 RVA: 0x00018CFC File Offset: 0x00016EFC
		[DoesServiceRequest]
		public Stream OpenRead(AccessCondition accessCondition = null, FileRequestOptions options = null, OperationContext operationContext = null)
		{
			operationContext = (operationContext ?? new OperationContext());
			this.FetchAttributes(accessCondition, options, operationContext);
			AccessCondition accessCondition2 = AccessCondition.CloneConditionWithETag(accessCondition, this.Properties.ETag);
			FileRequestOptions options2 = FileRequestOptions.ApplyDefaults(options, this.ServiceClient, false);
			return new FileReadStream(this, accessCondition2, options2, operationContext);
		}

		// Token: 0x060006AC RID: 1708 RVA: 0x00018D47 File Offset: 0x00016F47
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginOpenRead(AsyncCallback callback, object state)
		{
			return this.BeginOpenRead(null, null, null, callback, state);
		}

		// Token: 0x060006AD RID: 1709 RVA: 0x00018E04 File Offset: 0x00017004
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginOpenRead(AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			StorageAsyncResult<Stream> storageAsyncResult = new StorageAsyncResult<Stream>(callback, state);
			operationContext = (operationContext ?? new OperationContext());
			ICancellableAsyncResult @object = this.BeginFetchAttributes(accessCondition, options, operationContext, delegate(IAsyncResult ar)
			{
				try
				{
					this.EndFetchAttributes(ar);
					storageAsyncResult.UpdateCompletedSynchronously(ar.CompletedSynchronously);
					AccessCondition accessCondition2 = AccessCondition.CloneConditionWithETag(accessCondition, this.Properties.ETag);
					FileRequestOptions options2 = FileRequestOptions.ApplyDefaults(options, this.ServiceClient, false);
					storageAsyncResult.Result = new FileReadStream(this, accessCondition2, options2, operationContext);
					storageAsyncResult.OnComplete();
				}
				catch (Exception exception)
				{
					storageAsyncResult.OnComplete(exception);
				}
			}, null);
			storageAsyncResult.CancelDelegate = new Action(@object.Cancel);
			return storageAsyncResult;
		}

		// Token: 0x060006AE RID: 1710 RVA: 0x00018E9C File Offset: 0x0001709C
		public Stream EndOpenRead(IAsyncResult asyncResult)
		{
			StorageAsyncResult<Stream> storageAsyncResult = (StorageAsyncResult<Stream>)asyncResult;
			storageAsyncResult.End();
			return storageAsyncResult.Result;
		}

		// Token: 0x060006AF RID: 1711 RVA: 0x00018EBC File Offset: 0x000170BC
		[DoesServiceRequest]
		public Task<Stream> OpenReadAsync()
		{
			return this.OpenReadAsync(CancellationToken.None);
		}

		// Token: 0x060006B0 RID: 1712 RVA: 0x00018EC9 File Offset: 0x000170C9
		[DoesServiceRequest]
		public Task<Stream> OpenReadAsync(CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<Stream>(new Func<AsyncCallback, object, ICancellableAsyncResult>(this.BeginOpenRead), new Func<IAsyncResult, Stream>(this.EndOpenRead), cancellationToken);
		}

		// Token: 0x060006B1 RID: 1713 RVA: 0x00018EE9 File Offset: 0x000170E9
		[DoesServiceRequest]
		public Task<Stream> OpenReadAsync(AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext)
		{
			return this.OpenReadAsync(accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x060006B2 RID: 1714 RVA: 0x00018EF9 File Offset: 0x000170F9
		[DoesServiceRequest]
		public Task<Stream> OpenReadAsync(AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<AccessCondition, FileRequestOptions, OperationContext, Stream>(new Func<AccessCondition, FileRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginOpenRead), new Func<IAsyncResult, Stream>(this.EndOpenRead), accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x060006B3 RID: 1715 RVA: 0x00018F20 File Offset: 0x00017120
		[DoesServiceRequest]
		public CloudFileStream OpenWrite(long? size, AccessCondition accessCondition = null, FileRequestOptions options = null, OperationContext operationContext = null)
		{
			FileRequestOptions fileRequestOptions = FileRequestOptions.ApplyDefaults(options, this.ServiceClient, false);
			operationContext = (operationContext ?? new OperationContext());
			bool flag = size != null;
			if (flag)
			{
				this.Create(size.Value, accessCondition, options, operationContext);
			}
			else
			{
				if (fileRequestOptions.StoreFileContentMD5.Value)
				{
					throw new ArgumentException("MD5 cannot be calculated for an existing blob because it would require reading the existing data. Please disable StoreBlobContentMD5.");
				}
				this.FetchAttributes(accessCondition, options, operationContext);
				size = new long?(this.Properties.Length);
			}
			if (accessCondition != null)
			{
				accessCondition = AccessCondition.GenerateLeaseCondition(accessCondition.LeaseId);
			}
			return new FileWriteStream(this, size.Value, flag, accessCondition, fileRequestOptions, operationContext);
		}

		// Token: 0x060006B4 RID: 1716 RVA: 0x00018FBF File Offset: 0x000171BF
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginOpenWrite(long? size, AsyncCallback callback, object state)
		{
			return this.BeginOpenWrite(size, null, null, null, callback, state);
		}

		// Token: 0x060006B5 RID: 1717 RVA: 0x0001913C File Offset: 0x0001733C
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginOpenWrite(long? size, AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			FileRequestOptions modifiedOptions = FileRequestOptions.ApplyDefaults(options, this.ServiceClient, false);
			operationContext = (operationContext ?? new OperationContext());
			bool createNew = size != null;
			StorageAsyncResult<CloudFileStream> storageAsyncResult = new StorageAsyncResult<CloudFileStream>(callback, state);
			ICancellableAsyncResult @object;
			if (createNew)
			{
				@object = this.BeginCreate(size.Value, accessCondition, options, operationContext, delegate(IAsyncResult ar)
				{
					storageAsyncResult.UpdateCompletedSynchronously(ar.CompletedSynchronously);
					try
					{
						this.EndCreate(ar);
						if (accessCondition != null)
						{
							accessCondition = AccessCondition.GenerateLeaseCondition(accessCondition.LeaseId);
						}
						storageAsyncResult.Result = new FileWriteStream(this, size.Value, createNew, accessCondition, modifiedOptions, operationContext);
						storageAsyncResult.OnComplete();
					}
					catch (Exception exception)
					{
						storageAsyncResult.OnComplete(exception);
					}
				}, null);
			}
			else
			{
				if (modifiedOptions.StoreFileContentMD5.Value)
				{
					throw new ArgumentException("MD5 cannot be calculated for an existing blob because it would require reading the existing data. Please disable StoreBlobContentMD5.");
				}
				@object = this.BeginFetchAttributes(accessCondition, options, operationContext, delegate(IAsyncResult ar)
				{
					storageAsyncResult.UpdateCompletedSynchronously(ar.CompletedSynchronously);
					try
					{
						this.EndFetchAttributes(ar);
						if (accessCondition != null)
						{
							accessCondition = AccessCondition.GenerateLeaseCondition(accessCondition.LeaseId);
						}
						storageAsyncResult.Result = new FileWriteStream(this, this.Properties.Length, createNew, accessCondition, modifiedOptions, operationContext);
						storageAsyncResult.OnComplete();
					}
					catch (Exception exception)
					{
						storageAsyncResult.OnComplete(exception);
					}
				}, null);
			}
			storageAsyncResult.CancelDelegate = new Action(@object.Cancel);
			return storageAsyncResult;
		}

		// Token: 0x060006B6 RID: 1718 RVA: 0x00019258 File Offset: 0x00017458
		public CloudFileStream EndOpenWrite(IAsyncResult asyncResult)
		{
			StorageAsyncResult<CloudFileStream> storageAsyncResult = (StorageAsyncResult<CloudFileStream>)asyncResult;
			storageAsyncResult.End();
			return storageAsyncResult.Result;
		}

		// Token: 0x060006B7 RID: 1719 RVA: 0x00019278 File Offset: 0x00017478
		[DoesServiceRequest]
		public Task<CloudFileStream> OpenWriteAsync(long? size)
		{
			return this.OpenWriteAsync(size, CancellationToken.None);
		}

		// Token: 0x060006B8 RID: 1720 RVA: 0x00019286 File Offset: 0x00017486
		[DoesServiceRequest]
		public Task<CloudFileStream> OpenWriteAsync(long? size, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<long?, CloudFileStream>(new Func<long?, AsyncCallback, object, ICancellableAsyncResult>(this.BeginOpenWrite), new Func<IAsyncResult, CloudFileStream>(this.EndOpenWrite), size, cancellationToken);
		}

		// Token: 0x060006B9 RID: 1721 RVA: 0x000192A7 File Offset: 0x000174A7
		[DoesServiceRequest]
		public Task<CloudFileStream> OpenWriteAsync(long? size, AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext)
		{
			return this.OpenWriteAsync(size, accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x060006BA RID: 1722 RVA: 0x000192B9 File Offset: 0x000174B9
		[DoesServiceRequest]
		public Task<CloudFileStream> OpenWriteAsync(long? size, AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<long?, AccessCondition, FileRequestOptions, OperationContext, CloudFileStream>(new Func<long?, AccessCondition, FileRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginOpenWrite), new Func<IAsyncResult, CloudFileStream>(this.EndOpenWrite), size, accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x060006BB RID: 1723 RVA: 0x000192E0 File Offset: 0x000174E0
		[DoesServiceRequest]
		public void DownloadToStream(Stream target, AccessCondition accessCondition = null, FileRequestOptions options = null, OperationContext operationContext = null)
		{
			this.DownloadRangeToStream(target, null, null, accessCondition, options, operationContext);
		}

		// Token: 0x060006BC RID: 1724 RVA: 0x0001930A File Offset: 0x0001750A
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginDownloadToStream(Stream target, AsyncCallback callback, object state)
		{
			return this.BeginDownloadToStream(target, null, null, null, callback, state);
		}

		// Token: 0x060006BD RID: 1725 RVA: 0x00019318 File Offset: 0x00017518
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginDownloadToStream(Stream target, AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			return this.BeginDownloadRangeToStream(target, null, null, accessCondition, options, operationContext, callback, state);
		}

		// Token: 0x060006BE RID: 1726 RVA: 0x00019346 File Offset: 0x00017546
		public void EndDownloadToStream(IAsyncResult asyncResult)
		{
			this.EndDownloadRangeToStream(asyncResult);
		}

		// Token: 0x060006BF RID: 1727 RVA: 0x0001934F File Offset: 0x0001754F
		[DoesServiceRequest]
		public Task DownloadToStreamAsync(Stream target)
		{
			return this.DownloadToStreamAsync(target, CancellationToken.None);
		}

		// Token: 0x060006C0 RID: 1728 RVA: 0x0001935D File Offset: 0x0001755D
		[DoesServiceRequest]
		public Task DownloadToStreamAsync(Stream target, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<Stream>(new Func<Stream, AsyncCallback, object, ICancellableAsyncResult>(this.BeginDownloadToStream), new Action<IAsyncResult>(this.EndDownloadToStream), target, cancellationToken);
		}

		// Token: 0x060006C1 RID: 1729 RVA: 0x0001937E File Offset: 0x0001757E
		[DoesServiceRequest]
		public Task DownloadToStreamAsync(Stream target, AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext)
		{
			return this.DownloadToStreamAsync(target, accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x060006C2 RID: 1730 RVA: 0x00019390 File Offset: 0x00017590
		[DoesServiceRequest]
		public Task DownloadToStreamAsync(Stream target, AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<Stream, AccessCondition, FileRequestOptions, OperationContext>(new Func<Stream, AccessCondition, FileRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginDownloadToStream), new Action<IAsyncResult>(this.EndDownloadToStream), target, accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x060006C3 RID: 1731 RVA: 0x000193B8 File Offset: 0x000175B8
		[DoesServiceRequest]
		public void DownloadToFile(string path, FileMode mode, AccessCondition accessCondition = null, FileRequestOptions options = null, OperationContext operationContext = null)
		{
			CommonUtility.AssertNotNull("path", path);
			try
			{
				using (FileStream fileStream = new FileStream(path, mode, FileAccess.Write))
				{
					this.DownloadToStream(fileStream, accessCondition, options, operationContext);
				}
			}
			catch (Exception)
			{
				if (mode != FileMode.Create)
				{
					if (mode != FileMode.CreateNew)
					{
						goto IL_42;
					}
				}
				try
				{
					File.Delete(path);
				}
				catch (Exception)
				{
				}
				IL_42:
				throw;
			}
		}

		// Token: 0x060006C4 RID: 1732 RVA: 0x00019434 File Offset: 0x00017634
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginDownloadToFile(string path, FileMode mode, AsyncCallback callback, object state)
		{
			return this.BeginDownloadToFile(path, mode, null, null, null, callback, state);
		}

		// Token: 0x060006C5 RID: 1733 RVA: 0x00019444 File Offset: 0x00017644
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginDownloadToFile(string path, FileMode mode, AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			CommonUtility.AssertNotNull("path", path);
			FileStream fileStream = new FileStream(path, mode, FileAccess.Write);
			StorageAsyncResult<NullType> storageAsyncResult = new StorageAsyncResult<NullType>(callback, state)
			{
				OperationState = Tuple.Create<FileStream, FileMode>(fileStream, mode)
			};
			ICancellableAsyncResult result;
			try
			{
				ICancellableAsyncResult @object = this.BeginDownloadToStream(fileStream, accessCondition, options, operationContext, new AsyncCallback(this.DownloadToFileCallback), storageAsyncResult);
				storageAsyncResult.CancelDelegate = new Action(@object.Cancel);
				result = storageAsyncResult;
			}
			catch (Exception)
			{
				fileStream.Dispose();
				throw;
			}
			return result;
		}

		// Token: 0x060006C6 RID: 1734 RVA: 0x000194CC File Offset: 0x000176CC
		private void DownloadToFileCallback(IAsyncResult asyncResult)
		{
			StorageAsyncResult<NullType> storageAsyncResult = (StorageAsyncResult<NullType>)asyncResult.AsyncState;
			Exception exception = null;
			bool flag = false;
			string path = null;
			try
			{
				this.EndDownloadToStream(asyncResult);
			}
			catch (Exception ex)
			{
				exception = ex;
				flag = true;
			}
			try
			{
				FileStream item = ((Tuple<FileStream, FileMode>)storageAsyncResult.OperationState).Item1;
				path = item.Name;
				item.Dispose();
			}
			catch (Exception ex2)
			{
				exception = ex2;
			}
			if (flag)
			{
				try
				{
					FileMode item2 = ((Tuple<FileStream, FileMode>)storageAsyncResult.OperationState).Item2;
					if (item2 == FileMode.Create || item2 == FileMode.CreateNew)
					{
						File.Delete(path);
					}
				}
				catch (Exception)
				{
				}
			}
			storageAsyncResult.OnComplete(exception);
		}

		// Token: 0x060006C7 RID: 1735 RVA: 0x00019580 File Offset: 0x00017780
		public void EndDownloadToFile(IAsyncResult asyncResult)
		{
			StorageAsyncResult<NullType> storageAsyncResult = (StorageAsyncResult<NullType>)asyncResult;
			storageAsyncResult.End();
		}

		// Token: 0x060006C8 RID: 1736 RVA: 0x0001959A File Offset: 0x0001779A
		[DoesServiceRequest]
		public Task DownloadToFileAsync(string path, FileMode mode)
		{
			return this.DownloadToFileAsync(path, mode, CancellationToken.None);
		}

		// Token: 0x060006C9 RID: 1737 RVA: 0x000195A9 File Offset: 0x000177A9
		[DoesServiceRequest]
		public Task DownloadToFileAsync(string path, FileMode mode, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<string, FileMode>(new Func<string, FileMode, AsyncCallback, object, ICancellableAsyncResult>(this.BeginDownloadToFile), new Action<IAsyncResult>(this.EndDownloadToFile), path, mode, cancellationToken);
		}

		// Token: 0x060006CA RID: 1738 RVA: 0x000195CB File Offset: 0x000177CB
		[DoesServiceRequest]
		public Task DownloadToFileAsync(string path, FileMode mode, AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext)
		{
			return this.DownloadToFileAsync(path, mode, accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x060006CB RID: 1739 RVA: 0x000195DF File Offset: 0x000177DF
		[DoesServiceRequest]
		public Task DownloadToFileAsync(string path, FileMode mode, AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<string, FileMode, AccessCondition, FileRequestOptions, OperationContext>(new Func<string, FileMode, AccessCondition, FileRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginDownloadToFile), new Action<IAsyncResult>(this.EndDownloadToFile), path, mode, accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x060006CC RID: 1740 RVA: 0x00019608 File Offset: 0x00017808
		[DoesServiceRequest]
		public int DownloadToByteArray(byte[] target, int index, AccessCondition accessCondition = null, FileRequestOptions options = null, OperationContext operationContext = null)
		{
			return this.DownloadRangeToByteArray(target, index, null, null, accessCondition, options, operationContext);
		}

		// Token: 0x060006CD RID: 1741 RVA: 0x00019634 File Offset: 0x00017834
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginDownloadToByteArray(byte[] target, int index, AsyncCallback callback, object state)
		{
			return this.BeginDownloadToByteArray(target, index, null, null, null, callback, state);
		}

		// Token: 0x060006CE RID: 1742 RVA: 0x00019644 File Offset: 0x00017844
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginDownloadToByteArray(byte[] target, int index, AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			return this.BeginDownloadRangeToByteArray(target, index, null, null, accessCondition, options, operationContext, callback, state);
		}

		// Token: 0x060006CF RID: 1743 RVA: 0x00019674 File Offset: 0x00017874
		public int EndDownloadToByteArray(IAsyncResult asyncResult)
		{
			return this.EndDownloadRangeToByteArray(asyncResult);
		}

		// Token: 0x060006D0 RID: 1744 RVA: 0x0001967D File Offset: 0x0001787D
		[DoesServiceRequest]
		public Task<int> DownloadToByteArrayAsync(byte[] target, int index)
		{
			return this.DownloadToByteArrayAsync(target, index, CancellationToken.None);
		}

		// Token: 0x060006D1 RID: 1745 RVA: 0x0001968C File Offset: 0x0001788C
		[DoesServiceRequest]
		public Task<int> DownloadToByteArrayAsync(byte[] target, int index, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<byte[], int, int>(new Func<byte[], int, AsyncCallback, object, ICancellableAsyncResult>(this.BeginDownloadToByteArray), new Func<IAsyncResult, int>(this.EndDownloadToByteArray), target, index, cancellationToken);
		}

		// Token: 0x060006D2 RID: 1746 RVA: 0x000196AE File Offset: 0x000178AE
		[DoesServiceRequest]
		public Task<int> DownloadToByteArrayAsync(byte[] target, int index, AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext)
		{
			return this.DownloadToByteArrayAsync(target, index, accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x060006D3 RID: 1747 RVA: 0x000196C2 File Offset: 0x000178C2
		[DoesServiceRequest]
		public Task<int> DownloadToByteArrayAsync(byte[] target, int index, AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<byte[], int, AccessCondition, FileRequestOptions, OperationContext, int>(new Func<byte[], int, AccessCondition, FileRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginDownloadToByteArray), new Func<IAsyncResult, int>(this.EndDownloadToByteArray), target, index, accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x000196EC File Offset: 0x000178EC
		public string DownloadText(Encoding encoding = null, AccessCondition accessCondition = null, FileRequestOptions options = null, OperationContext operationContext = null)
		{
			string @string;
			using (SyncMemoryStream syncMemoryStream = new SyncMemoryStream())
			{
				this.DownloadToStream(syncMemoryStream, accessCondition, options, operationContext);
				byte[] buffer = syncMemoryStream.GetBuffer();
				@string = (encoding ?? Encoding.UTF8).GetString(buffer, 0, (int)syncMemoryStream.Length);
			}
			return @string;
		}

		// Token: 0x060006D5 RID: 1749 RVA: 0x00019748 File Offset: 0x00017948
		public ICancellableAsyncResult BeginDownloadText(AsyncCallback callback, object state)
		{
			return this.BeginDownloadText(null, null, null, null, callback, state);
		}

		// Token: 0x060006D6 RID: 1750 RVA: 0x00019758 File Offset: 0x00017958
		public ICancellableAsyncResult BeginDownloadText(Encoding encoding, AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			SyncMemoryStream syncMemoryStream = new SyncMemoryStream();
			StorageAsyncResult<string> storageAsyncResult = new StorageAsyncResult<string>(callback, state)
			{
				OperationState = Tuple.Create<SyncMemoryStream, Encoding>(syncMemoryStream, encoding)
			};
			ICancellableAsyncResult @object = this.BeginDownloadToStream(syncMemoryStream, accessCondition, options, operationContext, new AsyncCallback(this.DownloadTextCallback), storageAsyncResult);
			storageAsyncResult.CancelDelegate = new Action(@object.Cancel);
			return storageAsyncResult;
		}

		// Token: 0x060006D7 RID: 1751 RVA: 0x000197B4 File Offset: 0x000179B4
		private void DownloadTextCallback(IAsyncResult asyncResult)
		{
			StorageAsyncResult<string> storageAsyncResult = (StorageAsyncResult<string>)asyncResult.AsyncState;
			try
			{
				this.EndDownloadToStream(asyncResult);
				Tuple<SyncMemoryStream, Encoding> tuple = (Tuple<SyncMemoryStream, Encoding>)storageAsyncResult.OperationState;
				byte[] buffer = tuple.Item1.GetBuffer();
				storageAsyncResult.Result = (tuple.Item2 ?? Encoding.UTF8).GetString(buffer, 0, (int)tuple.Item1.Length);
				storageAsyncResult.OnComplete();
			}
			catch (Exception exception)
			{
				storageAsyncResult.OnComplete(exception);
			}
		}

		// Token: 0x060006D8 RID: 1752 RVA: 0x00019838 File Offset: 0x00017A38
		public string EndDownloadText(IAsyncResult asyncResult)
		{
			StorageAsyncResult<string> storageAsyncResult = (StorageAsyncResult<string>)asyncResult;
			storageAsyncResult.End();
			return storageAsyncResult.Result;
		}

		// Token: 0x060006D9 RID: 1753 RVA: 0x00019858 File Offset: 0x00017A58
		[DoesServiceRequest]
		public Task<string> DownloadTextAsync()
		{
			return this.DownloadTextAsync(CancellationToken.None);
		}

		// Token: 0x060006DA RID: 1754 RVA: 0x00019865 File Offset: 0x00017A65
		[DoesServiceRequest]
		public Task<string> DownloadTextAsync(CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<string>(new Func<AsyncCallback, object, ICancellableAsyncResult>(this.BeginDownloadText), new Func<IAsyncResult, string>(this.EndDownloadText), cancellationToken);
		}

		// Token: 0x060006DB RID: 1755 RVA: 0x00019885 File Offset: 0x00017A85
		[DoesServiceRequest]
		public Task<string> DownloadTextAsync(Encoding encoding, AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext)
		{
			return this.DownloadTextAsync(encoding, accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x060006DC RID: 1756 RVA: 0x00019897 File Offset: 0x00017A97
		[DoesServiceRequest]
		public Task<string> DownloadTextAsync(Encoding encoding, AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<Encoding, AccessCondition, FileRequestOptions, OperationContext, string>(new Func<Encoding, AccessCondition, FileRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginDownloadText), new Func<IAsyncResult, string>(this.EndDownloadText), encoding, accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x060006DD RID: 1757 RVA: 0x000198C0 File Offset: 0x00017AC0
		[DoesServiceRequest]
		public void DownloadRangeToStream(Stream target, long? offset, long? length, AccessCondition accessCondition = null, FileRequestOptions options = null, OperationContext operationContext = null)
		{
			CommonUtility.AssertNotNull("target", target);
			FileRequestOptions fileRequestOptions = FileRequestOptions.ApplyDefaults(options, this.ServiceClient, true);
			Executor.ExecuteSync<NullType>(this.GetFileImpl(target, offset, length, accessCondition, fileRequestOptions), fileRequestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x060006DE RID: 1758 RVA: 0x00019904 File Offset: 0x00017B04
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginDownloadRangeToStream(Stream target, long? offset, long? length, AsyncCallback callback, object state)
		{
			return this.BeginDownloadRangeToStream(target, offset, length, null, null, null, callback, state);
		}

		// Token: 0x060006DF RID: 1759 RVA: 0x00019924 File Offset: 0x00017B24
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginDownloadRangeToStream(Stream target, long? offset, long? length, AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			CommonUtility.AssertNotNull("target", target);
			FileRequestOptions fileRequestOptions = FileRequestOptions.ApplyDefaults(options, this.ServiceClient, true);
			return Executor.BeginExecuteAsync<NullType>(this.GetFileImpl(target, offset, length, accessCondition, fileRequestOptions), fileRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x060006E0 RID: 1760 RVA: 0x00019968 File Offset: 0x00017B68
		public void EndDownloadRangeToStream(IAsyncResult asyncResult)
		{
			Executor.EndExecuteAsync<NullType>(asyncResult);
		}

		// Token: 0x060006E1 RID: 1761 RVA: 0x00019971 File Offset: 0x00017B71
		[DoesServiceRequest]
		public Task DownloadRangeToStreamAsync(Stream target, long? offset, long? length)
		{
			return this.DownloadRangeToStreamAsync(target, offset, length, CancellationToken.None);
		}

		// Token: 0x060006E2 RID: 1762 RVA: 0x00019981 File Offset: 0x00017B81
		[DoesServiceRequest]
		public Task DownloadRangeToStreamAsync(Stream target, long? offset, long? length, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<Stream, long?, long?>(new Func<Stream, long?, long?, AsyncCallback, object, ICancellableAsyncResult>(this.BeginDownloadRangeToStream), new Action<IAsyncResult>(this.EndDownloadRangeToStream), target, offset, length, cancellationToken);
		}

		// Token: 0x060006E3 RID: 1763 RVA: 0x000199A5 File Offset: 0x00017BA5
		[DoesServiceRequest]
		public Task DownloadRangeToStreamAsync(Stream target, long? offset, long? length, AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext)
		{
			return this.DownloadRangeToStreamAsync(target, offset, length, accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x060006E4 RID: 1764 RVA: 0x000199BC File Offset: 0x00017BBC
		[DoesServiceRequest]
		public Task DownloadRangeToStreamAsync(Stream target, long? offset, long? length, AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<Stream, long?, long?, AccessCondition, FileRequestOptions, OperationContext>(new Func<Stream, long?, long?, AccessCondition, FileRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginDownloadRangeToStream), new Action<IAsyncResult>(this.EndDownloadRangeToStream), target, offset, length, accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x060006E5 RID: 1765 RVA: 0x000199F4 File Offset: 0x00017BF4
		[DoesServiceRequest]
		public int DownloadRangeToByteArray(byte[] target, int index, long? fileOffset, long? length, AccessCondition accessCondition = null, FileRequestOptions options = null, OperationContext operationContext = null)
		{
			int result;
			using (SyncMemoryStream syncMemoryStream = new SyncMemoryStream(target, index))
			{
				this.DownloadRangeToStream(syncMemoryStream, fileOffset, length, accessCondition, options, operationContext);
				result = (int)syncMemoryStream.Position;
			}
			return result;
		}

		// Token: 0x060006E6 RID: 1766 RVA: 0x00019A40 File Offset: 0x00017C40
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginDownloadRangeToByteArray(byte[] target, int index, long? fileOffset, long? length, AsyncCallback callback, object state)
		{
			return this.BeginDownloadRangeToByteArray(target, index, fileOffset, length, null, null, null, callback, state);
		}

		// Token: 0x060006E7 RID: 1767 RVA: 0x00019A60 File Offset: 0x00017C60
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginDownloadRangeToByteArray(byte[] target, int index, long? fileOffset, long? length, AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			SyncMemoryStream syncMemoryStream = new SyncMemoryStream(target, index);
			StorageAsyncResult<int> storageAsyncResult = new StorageAsyncResult<int>(callback, state)
			{
				OperationState = syncMemoryStream
			};
			ICancellableAsyncResult @object = this.BeginDownloadRangeToStream(syncMemoryStream, fileOffset, length, accessCondition, options, operationContext, new AsyncCallback(this.DownloadRangeToByteArrayCallback), storageAsyncResult);
			storageAsyncResult.CancelDelegate = new Action(@object.Cancel);
			return storageAsyncResult;
		}

		// Token: 0x060006E8 RID: 1768 RVA: 0x00019ABC File Offset: 0x00017CBC
		private void DownloadRangeToByteArrayCallback(IAsyncResult asyncResult)
		{
			StorageAsyncResult<int> storageAsyncResult = (StorageAsyncResult<int>)asyncResult.AsyncState;
			try
			{
				this.EndDownloadRangeToStream(asyncResult);
				SyncMemoryStream syncMemoryStream = (SyncMemoryStream)storageAsyncResult.OperationState;
				storageAsyncResult.Result = (int)syncMemoryStream.Position;
				storageAsyncResult.OnComplete();
			}
			catch (Exception exception)
			{
				storageAsyncResult.OnComplete(exception);
			}
		}

		// Token: 0x060006E9 RID: 1769 RVA: 0x00019B18 File Offset: 0x00017D18
		public int EndDownloadRangeToByteArray(IAsyncResult asyncResult)
		{
			StorageAsyncResult<int> storageAsyncResult = (StorageAsyncResult<int>)asyncResult;
			storageAsyncResult.End();
			return storageAsyncResult.Result;
		}

		// Token: 0x060006EA RID: 1770 RVA: 0x00019B38 File Offset: 0x00017D38
		[DoesServiceRequest]
		public Task<int> DownloadRangeToByteArrayAsync(byte[] target, int index, long? fileOffset, long? length)
		{
			return this.DownloadRangeToByteArrayAsync(target, index, fileOffset, length, CancellationToken.None);
		}

		// Token: 0x060006EB RID: 1771 RVA: 0x00019B4A File Offset: 0x00017D4A
		[DoesServiceRequest]
		public Task<int> DownloadRangeToByteArrayAsync(byte[] target, int index, long? fileOffset, long? length, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<byte[], int, long?, long?, int>(new Func<byte[], int, long?, long?, AsyncCallback, object, ICancellableAsyncResult>(this.BeginDownloadRangeToByteArray), new Func<IAsyncResult, int>(this.EndDownloadRangeToByteArray), target, index, fileOffset, length, cancellationToken);
		}

		// Token: 0x060006EC RID: 1772 RVA: 0x00019B70 File Offset: 0x00017D70
		[DoesServiceRequest]
		public Task<int> DownloadRangeToByteArrayAsync(byte[] target, int index, long? fileOffset, long? length, AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext)
		{
			return this.DownloadRangeToByteArrayAsync(target, index, fileOffset, length, accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x060006ED RID: 1773 RVA: 0x00019B94 File Offset: 0x00017D94
		[DoesServiceRequest]
		public Task<int> DownloadRangeToByteArrayAsync(byte[] target, int index, long? fileOffset, long? length, AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<byte[], int, long?, long?, AccessCondition, FileRequestOptions, OperationContext, int>(new Func<byte[], int, long?, long?, AccessCondition, FileRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginDownloadRangeToByteArray), new Func<IAsyncResult, int>(this.EndDownloadRangeToByteArray), target, index, fileOffset, length, accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x060006EE RID: 1774 RVA: 0x00019BCC File Offset: 0x00017DCC
		[DoesServiceRequest]
		public void UploadFromStream(Stream source, AccessCondition accessCondition = null, FileRequestOptions options = null, OperationContext operationContext = null)
		{
			this.UploadFromStreamHelper(source, null, accessCondition, options, operationContext);
		}

		// Token: 0x060006EF RID: 1775 RVA: 0x00019BED File Offset: 0x00017DED
		[DoesServiceRequest]
		public void UploadFromStream(Stream source, long length, AccessCondition accessCondition = null, FileRequestOptions options = null, OperationContext operationContext = null)
		{
			this.UploadFromStreamHelper(source, new long?(length), accessCondition, options, operationContext);
		}

		// Token: 0x060006F0 RID: 1776 RVA: 0x00019C04 File Offset: 0x00017E04
		internal void UploadFromStreamHelper(Stream source, long? length, AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext)
		{
			CommonUtility.AssertNotNull("source", source);
			if (!source.CanSeek)
			{
				throw new InvalidOperationException();
			}
			if (length != null)
			{
				CommonUtility.AssertInBounds<long>("length", length.Value, 1L, source.Length - source.Position);
			}
			else
			{
				length = new long?(source.Length - source.Position);
			}
			FileRequestOptions options2 = FileRequestOptions.ApplyDefaults(options, this.ServiceClient, true);
			operationContext = (operationContext ?? new OperationContext());
			using (CloudFileStream cloudFileStream = this.OpenWrite(length, accessCondition, options2, operationContext))
			{
				using (ExecutionState<NullType> executionState = CommonUtility.CreateTemporaryExecutionState(options2))
				{
					source.WriteToSync(cloudFileStream, length, null, false, true, executionState, null);
					cloudFileStream.Commit();
				}
			}
		}

		// Token: 0x060006F1 RID: 1777 RVA: 0x00019CE8 File Offset: 0x00017EE8
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginUploadFromStream(Stream source, AsyncCallback callback, object state)
		{
			return this.BeginUploadFromStreamHelper(source, null, null, null, null, callback, state);
		}

		// Token: 0x060006F2 RID: 1778 RVA: 0x00019D0C File Offset: 0x00017F0C
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginUploadFromStream(Stream source, AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			return this.BeginUploadFromStreamHelper(source, null, accessCondition, options, operationContext, callback, state);
		}

		// Token: 0x060006F3 RID: 1779 RVA: 0x00019D31 File Offset: 0x00017F31
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginUploadFromStream(Stream source, long length, AsyncCallback callback, object state)
		{
			return this.BeginUploadFromStreamHelper(source, new long?(length), null, null, null, callback, state);
		}

		// Token: 0x060006F4 RID: 1780 RVA: 0x00019D46 File Offset: 0x00017F46
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginUploadFromStream(Stream source, long length, AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			return this.BeginUploadFromStreamHelper(source, new long?(length), accessCondition, options, operationContext, callback, state);
		}

		// Token: 0x060006F5 RID: 1781 RVA: 0x00019F94 File Offset: 0x00018194
		[DoesServiceRequest]
		internal ICancellableAsyncResult BeginUploadFromStreamHelper(Stream source, long? length, AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			CommonUtility.AssertNotNull("source", source);
			if (!source.CanSeek)
			{
				throw new InvalidOperationException();
			}
			if (length != null)
			{
				CommonUtility.AssertInBounds<long>("length", length.Value, 1L, source.Length - source.Position);
			}
			else
			{
				length = new long?(source.Length - source.Position);
			}
			FileRequestOptions options2 = FileRequestOptions.ApplyDefaults(options, this.ServiceClient, true);
			ExecutionState<NullType> tempExecutionState = CommonUtility.CreateTemporaryExecutionState(options2);
			StorageAsyncResult<NullType> storageAsyncResult = new StorageAsyncResult<NullType>(callback, state);
			ICancellableAsyncResult @object = this.BeginOpenWrite(length, accessCondition, options2, operationContext, delegate(IAsyncResult ar)
			{
				storageAsyncResult.UpdateCompletedSynchronously(ar.CompletedSynchronously);
				lock (storageAsyncResult.CancellationLockerObject)
				{
					storageAsyncResult.CancelDelegate = null;
					try
					{
						CloudFileStream fileStream = this.EndOpenWrite(ar);
						storageAsyncResult.OperationState = fileStream;
						source.WriteToAsync(fileStream, length, null, false, tempExecutionState, null, delegate(ExecutionState<NullType> completedState)
						{
							storageAsyncResult.UpdateCompletedSynchronously(completedState.CompletedSynchronously);
							if (completedState.ExceptionRef != null)
							{
								storageAsyncResult.OnComplete(completedState.ExceptionRef);
								return;
							}
							try
							{
								lock (storageAsyncResult.CancellationLockerObject)
								{
									storageAsyncResult.CancelDelegate = null;
									ICancellableAsyncResult object2 = fileStream.BeginCommit(new AsyncCallback(CloudFile.FileStreamCommitCallback), storageAsyncResult);
									storageAsyncResult.CancelDelegate = new Action(object2.Cancel);
									if (storageAsyncResult.CancelRequested)
									{
										storageAsyncResult.Cancel();
									}
								}
							}
							catch (Exception exception2)
							{
								storageAsyncResult.OnComplete(exception2);
							}
						});
						storageAsyncResult.CancelDelegate = new Action(tempExecutionState.Cancel);
						if (storageAsyncResult.CancelRequested)
						{
							storageAsyncResult.Cancel();
						}
					}
					catch (Exception exception)
					{
						storageAsyncResult.OnComplete(exception);
					}
				}
			}, null);
			storageAsyncResult.CancelDelegate = new Action(@object.Cancel);
			return storageAsyncResult;
		}

		// Token: 0x060006F6 RID: 1782 RVA: 0x0001A0AC File Offset: 0x000182AC
		public void EndUploadFromStream(IAsyncResult asyncResult)
		{
			StorageAsyncResult<NullType> storageAsyncResult = (StorageAsyncResult<NullType>)asyncResult;
			storageAsyncResult.End();
		}

		// Token: 0x060006F7 RID: 1783 RVA: 0x0001A0C6 File Offset: 0x000182C6
		[DoesServiceRequest]
		public Task UploadFromStreamAsync(Stream source)
		{
			return this.UploadFromStreamAsync(source, CancellationToken.None);
		}

		// Token: 0x060006F8 RID: 1784 RVA: 0x0001A0D4 File Offset: 0x000182D4
		[DoesServiceRequest]
		public Task UploadFromStreamAsync(Stream source, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<Stream>(new Func<Stream, AsyncCallback, object, ICancellableAsyncResult>(this.BeginUploadFromStream), new Action<IAsyncResult>(this.EndUploadFromStream), source, cancellationToken);
		}

		// Token: 0x060006F9 RID: 1785 RVA: 0x0001A0F5 File Offset: 0x000182F5
		[DoesServiceRequest]
		public Task UploadFromStreamAsync(Stream source, AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext)
		{
			return this.UploadFromStreamAsync(source, accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x060006FA RID: 1786 RVA: 0x0001A107 File Offset: 0x00018307
		[DoesServiceRequest]
		public Task UploadFromStreamAsync(Stream source, AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<Stream, AccessCondition, FileRequestOptions, OperationContext>(new Func<Stream, AccessCondition, FileRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginUploadFromStream), new Action<IAsyncResult>(this.EndUploadFromStream), source, accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x060006FB RID: 1787 RVA: 0x0001A12D File Offset: 0x0001832D
		[DoesServiceRequest]
		public Task UploadFromStreamAsync(Stream source, long length)
		{
			return this.UploadFromStreamAsync(source, length, CancellationToken.None);
		}

		// Token: 0x060006FC RID: 1788 RVA: 0x0001A13C File Offset: 0x0001833C
		[DoesServiceRequest]
		public Task UploadFromStreamAsync(Stream source, long length, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<Stream, long>(new Func<Stream, long, AsyncCallback, object, ICancellableAsyncResult>(this.BeginUploadFromStream), new Action<IAsyncResult>(this.EndUploadFromStream), source, length, cancellationToken);
		}

		// Token: 0x060006FD RID: 1789 RVA: 0x0001A15E File Offset: 0x0001835E
		[DoesServiceRequest]
		public Task UploadFromStreamAsync(Stream source, long length, AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext)
		{
			return this.UploadFromStreamAsync(source, length, accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x060006FE RID: 1790 RVA: 0x0001A172 File Offset: 0x00018372
		[DoesServiceRequest]
		public Task UploadFromStreamAsync(Stream source, long length, AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<Stream, long, AccessCondition, FileRequestOptions, OperationContext>(new Func<Stream, long, AccessCondition, FileRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginUploadFromStream), new Action<IAsyncResult>(this.EndUploadFromStream), source, length, accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x060006FF RID: 1791 RVA: 0x0001A19C File Offset: 0x0001839C
		[DoesServiceRequest]
		public void UploadFromFile(string path, FileMode mode, AccessCondition accessCondition = null, FileRequestOptions options = null, OperationContext operationContext = null)
		{
			CommonUtility.AssertNotNull("path", path);
			using (FileStream fileStream = new FileStream(path, mode, FileAccess.Read))
			{
				this.UploadFromStream(fileStream, accessCondition, options, operationContext);
			}
		}

		// Token: 0x06000700 RID: 1792 RVA: 0x0001A1E8 File Offset: 0x000183E8
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginUploadFromFile(string path, FileMode mode, AsyncCallback callback, object state)
		{
			return this.BeginUploadFromFile(path, mode, null, null, null, callback, state);
		}

		// Token: 0x06000701 RID: 1793 RVA: 0x0001A1F8 File Offset: 0x000183F8
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginUploadFromFile(string path, FileMode mode, AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			CommonUtility.AssertNotNull("path", path);
			FileStream fileStream = new FileStream(path, mode, FileAccess.Read);
			StorageAsyncResult<NullType> storageAsyncResult = new StorageAsyncResult<NullType>(callback, state)
			{
				OperationState = fileStream
			};
			ICancellableAsyncResult result;
			try
			{
				ICancellableAsyncResult @object = this.BeginUploadFromStream(fileStream, accessCondition, options, operationContext, new AsyncCallback(this.UploadFromFileCallback), storageAsyncResult);
				storageAsyncResult.CancelDelegate = new Action(@object.Cancel);
				result = storageAsyncResult;
			}
			catch (Exception)
			{
				fileStream.Dispose();
				throw;
			}
			return result;
		}

		// Token: 0x06000702 RID: 1794 RVA: 0x0001A27C File Offset: 0x0001847C
		private void UploadFromFileCallback(IAsyncResult asyncResult)
		{
			StorageAsyncResult<NullType> storageAsyncResult = (StorageAsyncResult<NullType>)asyncResult.AsyncState;
			Exception exception = null;
			try
			{
				this.EndUploadFromStream(asyncResult);
			}
			catch (Exception ex)
			{
				exception = ex;
			}
			try
			{
				FileStream fileStream = (FileStream)storageAsyncResult.OperationState;
				fileStream.Dispose();
			}
			catch (Exception ex2)
			{
				exception = ex2;
			}
			storageAsyncResult.OnComplete(exception);
		}

		// Token: 0x06000703 RID: 1795 RVA: 0x0001A2E4 File Offset: 0x000184E4
		public void EndUploadFromFile(IAsyncResult asyncResult)
		{
			StorageAsyncResult<NullType> storageAsyncResult = (StorageAsyncResult<NullType>)asyncResult;
			storageAsyncResult.End();
		}

		// Token: 0x06000704 RID: 1796 RVA: 0x0001A2FE File Offset: 0x000184FE
		[DoesServiceRequest]
		public Task UploadFromFileAsync(string path, FileMode mode)
		{
			return this.UploadFromFileAsync(path, mode, CancellationToken.None);
		}

		// Token: 0x06000705 RID: 1797 RVA: 0x0001A30D File Offset: 0x0001850D
		[DoesServiceRequest]
		public Task UploadFromFileAsync(string path, FileMode mode, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<string, FileMode>(new Func<string, FileMode, AsyncCallback, object, ICancellableAsyncResult>(this.BeginUploadFromFile), new Action<IAsyncResult>(this.EndUploadFromFile), path, mode, cancellationToken);
		}

		// Token: 0x06000706 RID: 1798 RVA: 0x0001A32F File Offset: 0x0001852F
		[DoesServiceRequest]
		public Task UploadFromFileAsync(string path, FileMode mode, AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext)
		{
			return this.UploadFromFileAsync(path, mode, accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x06000707 RID: 1799 RVA: 0x0001A343 File Offset: 0x00018543
		[DoesServiceRequest]
		public Task UploadFromFileAsync(string path, FileMode mode, AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<string, FileMode, AccessCondition, FileRequestOptions, OperationContext>(new Func<string, FileMode, AccessCondition, FileRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginUploadFromFile), new Action<IAsyncResult>(this.EndUploadFromFile), path, mode, accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x06000708 RID: 1800 RVA: 0x0001A36C File Offset: 0x0001856C
		[DoesServiceRequest]
		public void UploadFromByteArray(byte[] buffer, int index, int count, AccessCondition accessCondition = null, FileRequestOptions options = null, OperationContext operationContext = null)
		{
			CommonUtility.AssertNotNull("buffer", buffer);
			using (SyncMemoryStream syncMemoryStream = new SyncMemoryStream(buffer, index, count))
			{
				this.UploadFromStream(syncMemoryStream, accessCondition, options, operationContext);
			}
		}

		// Token: 0x06000709 RID: 1801 RVA: 0x0001A3B8 File Offset: 0x000185B8
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginUploadFromByteArray(byte[] buffer, int index, int count, AsyncCallback callback, object state)
		{
			return this.BeginUploadFromByteArray(buffer, index, count, null, null, null, callback, state);
		}

		// Token: 0x0600070A RID: 1802 RVA: 0x0001A3D8 File Offset: 0x000185D8
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginUploadFromByteArray(byte[] buffer, int index, int count, AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			CommonUtility.AssertNotNull("buffer", buffer);
			SyncMemoryStream source = new SyncMemoryStream(buffer, index, count);
			return this.BeginUploadFromStream(source, accessCondition, options, operationContext, callback, state);
		}

		// Token: 0x0600070B RID: 1803 RVA: 0x0001A40A File Offset: 0x0001860A
		public void EndUploadFromByteArray(IAsyncResult asyncResult)
		{
			this.EndUploadFromStream(asyncResult);
		}

		// Token: 0x0600070C RID: 1804 RVA: 0x0001A413 File Offset: 0x00018613
		[DoesServiceRequest]
		public Task UploadFromByteArrayAsync(byte[] buffer, int index, int count)
		{
			return this.UploadFromByteArrayAsync(buffer, index, count, CancellationToken.None);
		}

		// Token: 0x0600070D RID: 1805 RVA: 0x0001A423 File Offset: 0x00018623
		[DoesServiceRequest]
		public Task UploadFromByteArrayAsync(byte[] buffer, int index, int count, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<byte[], int, int>(new Func<byte[], int, int, AsyncCallback, object, ICancellableAsyncResult>(this.BeginUploadFromByteArray), new Action<IAsyncResult>(this.EndUploadFromByteArray), buffer, index, count, cancellationToken);
		}

		// Token: 0x0600070E RID: 1806 RVA: 0x0001A447 File Offset: 0x00018647
		[DoesServiceRequest]
		public Task UploadFromByteArrayAsync(byte[] buffer, int index, int count, AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext)
		{
			return this.UploadFromByteArrayAsync(buffer, index, count, accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x0600070F RID: 1807 RVA: 0x0001A460 File Offset: 0x00018660
		[DoesServiceRequest]
		public Task UploadFromByteArrayAsync(byte[] buffer, int index, int count, AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<byte[], int, int, AccessCondition, FileRequestOptions, OperationContext>(new Func<byte[], int, int, AccessCondition, FileRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginUploadFromByteArray), new Action<IAsyncResult>(this.EndUploadFromByteArray), buffer, index, count, accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x06000710 RID: 1808 RVA: 0x0001A498 File Offset: 0x00018698
		[DoesServiceRequest]
		public void UploadText(string content, Encoding encoding = null, AccessCondition accessCondition = null, FileRequestOptions options = null, OperationContext operationContext = null)
		{
			CommonUtility.AssertNotNull("content", content);
			byte[] bytes = (encoding ?? Encoding.UTF8).GetBytes(content);
			this.UploadFromByteArray(bytes, 0, bytes.Length, accessCondition, options, operationContext);
		}

		// Token: 0x06000711 RID: 1809 RVA: 0x0001A4D1 File Offset: 0x000186D1
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginUploadText(string content, AsyncCallback callback, object state)
		{
			return this.BeginUploadText(content, null, null, null, null, callback, state);
		}

		// Token: 0x06000712 RID: 1810 RVA: 0x0001A4E0 File Offset: 0x000186E0
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginUploadText(string content, Encoding encoding, AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			CommonUtility.AssertNotNull("content", content);
			byte[] bytes = (encoding ?? Encoding.UTF8).GetBytes(content);
			return this.BeginUploadFromByteArray(bytes, 0, bytes.Length, accessCondition, options, operationContext, callback, state);
		}

		// Token: 0x06000713 RID: 1811 RVA: 0x0001A51D File Offset: 0x0001871D
		public void EndUploadText(IAsyncResult asyncResult)
		{
			this.EndUploadFromByteArray(asyncResult);
		}

		// Token: 0x06000714 RID: 1812 RVA: 0x0001A526 File Offset: 0x00018726
		[DoesServiceRequest]
		public Task UploadTextAsync(string content)
		{
			return this.UploadTextAsync(content, CancellationToken.None);
		}

		// Token: 0x06000715 RID: 1813 RVA: 0x0001A534 File Offset: 0x00018734
		[DoesServiceRequest]
		public Task UploadTextAsync(string content, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<string>(new Func<string, AsyncCallback, object, ICancellableAsyncResult>(this.BeginUploadText), new Action<IAsyncResult>(this.EndUploadText), content, cancellationToken);
		}

		// Token: 0x06000716 RID: 1814 RVA: 0x0001A555 File Offset: 0x00018755
		[DoesServiceRequest]
		public Task UploadTextAsync(string content, Encoding encoding, AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext)
		{
			return this.UploadTextAsync(content, encoding, accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x06000717 RID: 1815 RVA: 0x0001A569 File Offset: 0x00018769
		[DoesServiceRequest]
		public Task UploadTextAsync(string content, Encoding encoding, AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<string, Encoding, AccessCondition, FileRequestOptions, OperationContext>(new Func<string, Encoding, AccessCondition, FileRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginUploadText), new Action<IAsyncResult>(this.EndUploadText), content, encoding, accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x06000718 RID: 1816 RVA: 0x0001A594 File Offset: 0x00018794
		[DoesServiceRequest]
		public void Create(long size, AccessCondition accessCondition = null, FileRequestOptions options = null, OperationContext operationContext = null)
		{
			FileRequestOptions fileRequestOptions = FileRequestOptions.ApplyDefaults(options, this.ServiceClient, true);
			Executor.ExecuteSync<NullType>(this.CreateImpl(size, accessCondition, fileRequestOptions), fileRequestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x06000719 RID: 1817 RVA: 0x0001A5C6 File Offset: 0x000187C6
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginCreate(long size, AsyncCallback callback, object state)
		{
			return this.BeginCreate(size, null, null, null, callback, state);
		}

		// Token: 0x0600071A RID: 1818 RVA: 0x0001A5D4 File Offset: 0x000187D4
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginCreate(long size, AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			FileRequestOptions fileRequestOptions = FileRequestOptions.ApplyDefaults(options, this.ServiceClient, true);
			return Executor.BeginExecuteAsync<NullType>(this.CreateImpl(size, accessCondition, fileRequestOptions), fileRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x0600071B RID: 1819 RVA: 0x0001A609 File Offset: 0x00018809
		public void EndCreate(IAsyncResult asyncResult)
		{
			Executor.EndExecuteAsync<NullType>(asyncResult);
		}

		// Token: 0x0600071C RID: 1820 RVA: 0x0001A612 File Offset: 0x00018812
		[DoesServiceRequest]
		public Task CreateAsync(long size)
		{
			return this.CreateAsync(size, CancellationToken.None);
		}

		// Token: 0x0600071D RID: 1821 RVA: 0x0001A620 File Offset: 0x00018820
		[DoesServiceRequest]
		public Task CreateAsync(long size, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<long>(new Func<long, AsyncCallback, object, ICancellableAsyncResult>(this.BeginCreate), new Action<IAsyncResult>(this.EndCreate), size, cancellationToken);
		}

		// Token: 0x0600071E RID: 1822 RVA: 0x0001A641 File Offset: 0x00018841
		[DoesServiceRequest]
		public Task CreateAsync(long size, AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext)
		{
			return this.CreateAsync(size, accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x0600071F RID: 1823 RVA: 0x0001A653 File Offset: 0x00018853
		[DoesServiceRequest]
		public Task CreateAsync(long size, AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<long, AccessCondition, FileRequestOptions, OperationContext>(new Func<long, AccessCondition, FileRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginCreate), new Action<IAsyncResult>(this.EndCreate), size, accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x06000720 RID: 1824 RVA: 0x0001A67C File Offset: 0x0001887C
		[DoesServiceRequest]
		public bool Exists(FileRequestOptions options = null, OperationContext operationContext = null)
		{
			FileRequestOptions fileRequestOptions = FileRequestOptions.ApplyDefaults(options, this.ServiceClient, true);
			return Executor.ExecuteSync<bool>(this.ExistsImpl(fileRequestOptions), fileRequestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x06000721 RID: 1825 RVA: 0x0001A6AA File Offset: 0x000188AA
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginExists(AsyncCallback callback, object state)
		{
			return this.BeginExists(null, null, callback, state);
		}

		// Token: 0x06000722 RID: 1826 RVA: 0x0001A6B8 File Offset: 0x000188B8
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginExists(FileRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			FileRequestOptions fileRequestOptions = FileRequestOptions.ApplyDefaults(options, this.ServiceClient, true);
			return Executor.BeginExecuteAsync<bool>(this.ExistsImpl(fileRequestOptions), fileRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x06000723 RID: 1827 RVA: 0x0001A6E9 File Offset: 0x000188E9
		public bool EndExists(IAsyncResult asyncResult)
		{
			return Executor.EndExecuteAsync<bool>(asyncResult);
		}

		// Token: 0x06000724 RID: 1828 RVA: 0x0001A6F1 File Offset: 0x000188F1
		[DoesServiceRequest]
		public Task<bool> ExistsAsync()
		{
			return this.ExistsAsync(CancellationToken.None);
		}

		// Token: 0x06000725 RID: 1829 RVA: 0x0001A6FE File Offset: 0x000188FE
		[DoesServiceRequest]
		public Task<bool> ExistsAsync(CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<bool>(new Func<AsyncCallback, object, ICancellableAsyncResult>(this.BeginExists), new Func<IAsyncResult, bool>(this.EndExists), cancellationToken);
		}

		// Token: 0x06000726 RID: 1830 RVA: 0x0001A71E File Offset: 0x0001891E
		[DoesServiceRequest]
		public Task<bool> ExistsAsync(FileRequestOptions options, OperationContext operationContext)
		{
			return this.ExistsAsync(options, operationContext, CancellationToken.None);
		}

		// Token: 0x06000727 RID: 1831 RVA: 0x0001A72D File Offset: 0x0001892D
		[DoesServiceRequest]
		public Task<bool> ExistsAsync(FileRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<FileRequestOptions, OperationContext, bool>(new Func<FileRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginExists), new Func<IAsyncResult, bool>(this.EndExists), options, operationContext, cancellationToken);
		}

		// Token: 0x06000728 RID: 1832 RVA: 0x0001A750 File Offset: 0x00018950
		[DoesServiceRequest]
		public void FetchAttributes(AccessCondition accessCondition = null, FileRequestOptions options = null, OperationContext operationContext = null)
		{
			FileRequestOptions fileRequestOptions = FileRequestOptions.ApplyDefaults(options, this.ServiceClient, true);
			Executor.ExecuteSync<NullType>(this.FetchAttributesImpl(accessCondition, fileRequestOptions), fileRequestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x06000729 RID: 1833 RVA: 0x0001A780 File Offset: 0x00018980
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginFetchAttributes(AsyncCallback callback, object state)
		{
			return this.BeginFetchAttributes(null, null, null, callback, state);
		}

		// Token: 0x0600072A RID: 1834 RVA: 0x0001A790 File Offset: 0x00018990
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginFetchAttributes(AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			FileRequestOptions fileRequestOptions = FileRequestOptions.ApplyDefaults(options, this.ServiceClient, true);
			return Executor.BeginExecuteAsync<NullType>(this.FetchAttributesImpl(accessCondition, fileRequestOptions), fileRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x0600072B RID: 1835 RVA: 0x0001A7C3 File Offset: 0x000189C3
		public void EndFetchAttributes(IAsyncResult asyncResult)
		{
			Executor.EndExecuteAsync<NullType>(asyncResult);
		}

		// Token: 0x0600072C RID: 1836 RVA: 0x0001A7CC File Offset: 0x000189CC
		[DoesServiceRequest]
		public Task FetchAttributesAsync()
		{
			return this.FetchAttributesAsync(CancellationToken.None);
		}

		// Token: 0x0600072D RID: 1837 RVA: 0x0001A7D9 File Offset: 0x000189D9
		[DoesServiceRequest]
		public Task FetchAttributesAsync(CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm(new Func<AsyncCallback, object, ICancellableAsyncResult>(this.BeginFetchAttributes), new Action<IAsyncResult>(this.EndFetchAttributes), cancellationToken);
		}

		// Token: 0x0600072E RID: 1838 RVA: 0x0001A7F9 File Offset: 0x000189F9
		[DoesServiceRequest]
		public Task FetchAttributesAsync(AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext)
		{
			return this.FetchAttributesAsync(accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x0600072F RID: 1839 RVA: 0x0001A809 File Offset: 0x00018A09
		[DoesServiceRequest]
		public Task FetchAttributesAsync(AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<AccessCondition, FileRequestOptions, OperationContext>(new Func<AccessCondition, FileRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginFetchAttributes), new Action<IAsyncResult>(this.EndFetchAttributes), accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x06000730 RID: 1840 RVA: 0x0001A830 File Offset: 0x00018A30
		[DoesServiceRequest]
		public void Delete(AccessCondition accessCondition = null, FileRequestOptions options = null, OperationContext operationContext = null)
		{
			FileRequestOptions fileRequestOptions = FileRequestOptions.ApplyDefaults(options, this.ServiceClient, true);
			Executor.ExecuteSync<NullType>(this.DeleteFileImpl(accessCondition, fileRequestOptions), fileRequestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x06000731 RID: 1841 RVA: 0x0001A860 File Offset: 0x00018A60
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginDelete(AsyncCallback callback, object state)
		{
			return this.BeginDelete(null, null, null, callback, state);
		}

		// Token: 0x06000732 RID: 1842 RVA: 0x0001A870 File Offset: 0x00018A70
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginDelete(AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			FileRequestOptions fileRequestOptions = FileRequestOptions.ApplyDefaults(options, this.ServiceClient, true);
			return Executor.BeginExecuteAsync<NullType>(this.DeleteFileImpl(accessCondition, fileRequestOptions), fileRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x06000733 RID: 1843 RVA: 0x0001A8A3 File Offset: 0x00018AA3
		public void EndDelete(IAsyncResult asyncResult)
		{
			Executor.EndExecuteAsync<NullType>(asyncResult);
		}

		// Token: 0x06000734 RID: 1844 RVA: 0x0001A8AC File Offset: 0x00018AAC
		[DoesServiceRequest]
		public Task DeleteAsync()
		{
			return this.DeleteAsync(CancellationToken.None);
		}

		// Token: 0x06000735 RID: 1845 RVA: 0x0001A8B9 File Offset: 0x00018AB9
		[DoesServiceRequest]
		public Task DeleteAsync(CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm(new Func<AsyncCallback, object, ICancellableAsyncResult>(this.BeginDelete), new Action<IAsyncResult>(this.EndDelete), cancellationToken);
		}

		// Token: 0x06000736 RID: 1846 RVA: 0x0001A8D9 File Offset: 0x00018AD9
		[DoesServiceRequest]
		public Task DeleteAsync(AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext)
		{
			return this.DeleteAsync(accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x06000737 RID: 1847 RVA: 0x0001A8E9 File Offset: 0x00018AE9
		[DoesServiceRequest]
		public Task DeleteAsync(AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<AccessCondition, FileRequestOptions, OperationContext>(new Func<AccessCondition, FileRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginDelete), new Action<IAsyncResult>(this.EndDelete), accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x06000738 RID: 1848 RVA: 0x0001A910 File Offset: 0x00018B10
		[DoesServiceRequest]
		public bool DeleteIfExists(AccessCondition accessCondition = null, FileRequestOptions options = null, OperationContext operationContext = null)
		{
			FileRequestOptions options2 = FileRequestOptions.ApplyDefaults(options, this.ServiceClient, true);
			operationContext = (operationContext ?? new OperationContext());
			if (!this.Exists(options2, operationContext))
			{
				return false;
			}
			bool result;
			try
			{
				this.Delete(accessCondition, options2, operationContext);
				result = true;
			}
			catch (StorageException ex)
			{
				if (ex.RequestInformation.HttpStatusCode != 404)
				{
					throw;
				}
				if (ex.RequestInformation.ExtendedErrorInformation != null && !(ex.RequestInformation.ExtendedErrorInformation.ErrorCode == StorageErrorCodeStrings.ResourceNotFound))
				{
					throw;
				}
				result = false;
			}
			return result;
		}

		// Token: 0x06000739 RID: 1849 RVA: 0x0001A9A8 File Offset: 0x00018BA8
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginDeleteIfExists(AsyncCallback callback, object state)
		{
			return this.BeginDeleteIfExists(null, null, null, callback, state);
		}

		// Token: 0x0600073A RID: 1850 RVA: 0x0001A9B8 File Offset: 0x00018BB8
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginDeleteIfExists(AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			FileRequestOptions requestOptions = FileRequestOptions.ApplyDefaults(options, this.ServiceClient, true);
			operationContext = (operationContext ?? new OperationContext());
			StorageAsyncResult<bool> storageAsyncResult = new StorageAsyncResult<bool>(callback, state)
			{
				RequestOptions = requestOptions,
				OperationContext = operationContext
			};
			this.DeleteIfExistsHandler(accessCondition, options, operationContext, storageAsyncResult);
			return storageAsyncResult;
		}

		// Token: 0x0600073B RID: 1851 RVA: 0x0001AC0C File Offset: 0x00018E0C
		private void DeleteIfExistsHandler(AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext, StorageAsyncResult<bool> storageAsyncResult)
		{
			ICancellableAsyncResult @object = this.BeginExists(options, operationContext, delegate(IAsyncResult existsResult)
			{
				storageAsyncResult.UpdateCompletedSynchronously(existsResult.CompletedSynchronously);
				lock (storageAsyncResult.CancellationLockerObject)
				{
					storageAsyncResult.CancelDelegate = null;
					try
					{
						if (!this.EndExists(existsResult))
						{
							storageAsyncResult.Result = false;
							storageAsyncResult.OnComplete();
							return;
						}
					}
					catch (Exception exception)
					{
						storageAsyncResult.OnComplete(exception);
						return;
					}
					ICancellableAsyncResult object2 = this.BeginDelete(accessCondition, options, operationContext, delegate(IAsyncResult deleteResult)
					{
						storageAsyncResult.UpdateCompletedSynchronously(deleteResult.CompletedSynchronously);
						storageAsyncResult.CancelDelegate = null;
						try
						{
							this.EndDelete(deleteResult);
							storageAsyncResult.Result = true;
							storageAsyncResult.OnComplete();
						}
						catch (StorageException ex)
						{
							if (ex.RequestInformation.HttpStatusCode == 404)
							{
								if (ex.RequestInformation.ExtendedErrorInformation == null || ex.RequestInformation.ExtendedErrorInformation.ErrorCode == StorageErrorCodeStrings.ResourceNotFound)
								{
									storageAsyncResult.Result = false;
									storageAsyncResult.OnComplete();
								}
								else
								{
									storageAsyncResult.OnComplete(ex);
								}
							}
							else
							{
								storageAsyncResult.OnComplete(ex);
							}
						}
						catch (Exception exception2)
						{
							storageAsyncResult.OnComplete(exception2);
						}
					}, null);
					storageAsyncResult.CancelDelegate = new Action(object2.Cancel);
					if (storageAsyncResult.CancelRequested)
					{
						storageAsyncResult.Cancel();
					}
				}
			}, null);
			storageAsyncResult.CancelDelegate = new Action(@object.Cancel);
		}

		// Token: 0x0600073C RID: 1852 RVA: 0x0001AC7C File Offset: 0x00018E7C
		public bool EndDeleteIfExists(IAsyncResult asyncResult)
		{
			StorageAsyncResult<bool> storageAsyncResult = (StorageAsyncResult<bool>)asyncResult;
			storageAsyncResult.End();
			return storageAsyncResult.Result;
		}

		// Token: 0x0600073D RID: 1853 RVA: 0x0001AC9C File Offset: 0x00018E9C
		[DoesServiceRequest]
		public Task<bool> DeleteIfExistsAsync()
		{
			return this.DeleteIfExistsAsync(CancellationToken.None);
		}

		// Token: 0x0600073E RID: 1854 RVA: 0x0001ACA9 File Offset: 0x00018EA9
		[DoesServiceRequest]
		public Task<bool> DeleteIfExistsAsync(CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<bool>(new Func<AsyncCallback, object, ICancellableAsyncResult>(this.BeginDeleteIfExists), new Func<IAsyncResult, bool>(this.EndDeleteIfExists), cancellationToken);
		}

		// Token: 0x0600073F RID: 1855 RVA: 0x0001ACC9 File Offset: 0x00018EC9
		[DoesServiceRequest]
		public Task<bool> DeleteIfExistsAsync(AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext)
		{
			return this.DeleteIfExistsAsync(accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x06000740 RID: 1856 RVA: 0x0001ACD9 File Offset: 0x00018ED9
		[DoesServiceRequest]
		public Task<bool> DeleteIfExistsAsync(AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<AccessCondition, FileRequestOptions, OperationContext, bool>(new Func<AccessCondition, FileRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginDeleteIfExists), new Func<IAsyncResult, bool>(this.EndDeleteIfExists), accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x06000741 RID: 1857 RVA: 0x0001AD00 File Offset: 0x00018F00
		[DoesServiceRequest]
		public IEnumerable<FileRange> ListRanges(long? offset = null, long? length = null, AccessCondition accessCondition = null, FileRequestOptions options = null, OperationContext operationContext = null)
		{
			FileRequestOptions fileRequestOptions = FileRequestOptions.ApplyDefaults(options, this.ServiceClient, true);
			return Executor.ExecuteSync<IEnumerable<FileRange>>(this.ListRangesImpl(offset, length, accessCondition, fileRequestOptions), fileRequestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x06000742 RID: 1858 RVA: 0x0001AD34 File Offset: 0x00018F34
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginListRanges(AsyncCallback callback, object state)
		{
			return this.BeginListRanges(null, null, null, null, null, callback, state);
		}

		// Token: 0x06000743 RID: 1859 RVA: 0x0001AD60 File Offset: 0x00018F60
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginListRanges(long? offset, long? length, AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			FileRequestOptions fileRequestOptions = FileRequestOptions.ApplyDefaults(options, this.ServiceClient, true);
			return Executor.BeginExecuteAsync<IEnumerable<FileRange>>(this.ListRangesImpl(offset, length, accessCondition, fileRequestOptions), fileRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x06000744 RID: 1860 RVA: 0x0001AD97 File Offset: 0x00018F97
		public IEnumerable<FileRange> EndListRanges(IAsyncResult asyncResult)
		{
			return Executor.EndExecuteAsync<IEnumerable<FileRange>>(asyncResult);
		}

		// Token: 0x06000745 RID: 1861 RVA: 0x0001AD9F File Offset: 0x00018F9F
		[DoesServiceRequest]
		public Task<IEnumerable<FileRange>> ListRangesAsync()
		{
			return this.ListRangesAsync(CancellationToken.None);
		}

		// Token: 0x06000746 RID: 1862 RVA: 0x0001ADAC File Offset: 0x00018FAC
		[DoesServiceRequest]
		public Task<IEnumerable<FileRange>> ListRangesAsync(CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<IEnumerable<FileRange>>(new Func<AsyncCallback, object, ICancellableAsyncResult>(this.BeginListRanges), new Func<IAsyncResult, IEnumerable<FileRange>>(this.EndListRanges), cancellationToken);
		}

		// Token: 0x06000747 RID: 1863 RVA: 0x0001ADCC File Offset: 0x00018FCC
		[DoesServiceRequest]
		public Task<IEnumerable<FileRange>> ListRangesAsync(long? offset, long? length, AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext)
		{
			return this.ListRangesAsync(offset, length, accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x06000748 RID: 1864 RVA: 0x0001ADE0 File Offset: 0x00018FE0
		[DoesServiceRequest]
		public Task<IEnumerable<FileRange>> ListRangesAsync(long? offset, long? length, AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<long?, long?, AccessCondition, FileRequestOptions, OperationContext, IEnumerable<FileRange>>(new Func<long?, long?, AccessCondition, FileRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginListRanges), new Func<IAsyncResult, IEnumerable<FileRange>>(this.EndListRanges), offset, length, accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x06000749 RID: 1865 RVA: 0x0001AE08 File Offset: 0x00019008
		[DoesServiceRequest]
		public void SetProperties(AccessCondition accessCondition = null, FileRequestOptions options = null, OperationContext operationContext = null)
		{
			FileRequestOptions fileRequestOptions = FileRequestOptions.ApplyDefaults(options, this.ServiceClient, true);
			Executor.ExecuteSync<NullType>(this.SetPropertiesImpl(accessCondition, fileRequestOptions), fileRequestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x0600074A RID: 1866 RVA: 0x0001AE38 File Offset: 0x00019038
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginSetProperties(AsyncCallback callback, object state)
		{
			return this.BeginSetProperties(null, null, null, callback, state);
		}

		// Token: 0x0600074B RID: 1867 RVA: 0x0001AE48 File Offset: 0x00019048
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginSetProperties(AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			FileRequestOptions fileRequestOptions = FileRequestOptions.ApplyDefaults(options, this.ServiceClient, true);
			return Executor.BeginExecuteAsync<NullType>(this.SetPropertiesImpl(accessCondition, fileRequestOptions), fileRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x0600074C RID: 1868 RVA: 0x0001AE7B File Offset: 0x0001907B
		public void EndSetProperties(IAsyncResult asyncResult)
		{
			Executor.EndExecuteAsync<NullType>(asyncResult);
		}

		// Token: 0x0600074D RID: 1869 RVA: 0x0001AE84 File Offset: 0x00019084
		[DoesServiceRequest]
		public Task SetPropertiesAsync()
		{
			return this.SetPropertiesAsync(CancellationToken.None);
		}

		// Token: 0x0600074E RID: 1870 RVA: 0x0001AE91 File Offset: 0x00019091
		[DoesServiceRequest]
		public Task SetPropertiesAsync(CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm(new Func<AsyncCallback, object, ICancellableAsyncResult>(this.BeginSetProperties), new Action<IAsyncResult>(this.EndSetProperties), cancellationToken);
		}

		// Token: 0x0600074F RID: 1871 RVA: 0x0001AEB1 File Offset: 0x000190B1
		[DoesServiceRequest]
		public Task SetPropertiesAsync(AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext)
		{
			return this.SetPropertiesAsync(accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x06000750 RID: 1872 RVA: 0x0001AEC1 File Offset: 0x000190C1
		[DoesServiceRequest]
		public Task SetPropertiesAsync(AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<AccessCondition, FileRequestOptions, OperationContext>(new Func<AccessCondition, FileRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginSetProperties), new Action<IAsyncResult>(this.EndSetProperties), accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x06000751 RID: 1873 RVA: 0x0001AEE8 File Offset: 0x000190E8
		[DoesServiceRequest]
		public void Resize(long size, AccessCondition accessCondition = null, FileRequestOptions options = null, OperationContext operationContext = null)
		{
			FileRequestOptions fileRequestOptions = FileRequestOptions.ApplyDefaults(options, this.ServiceClient, true);
			Executor.ExecuteSync<NullType>(this.ResizeImpl(size, accessCondition, fileRequestOptions), fileRequestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x06000752 RID: 1874 RVA: 0x0001AF1A File Offset: 0x0001911A
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginResize(long size, AsyncCallback callback, object state)
		{
			return this.BeginResize(size, null, null, null, callback, state);
		}

		// Token: 0x06000753 RID: 1875 RVA: 0x0001AF28 File Offset: 0x00019128
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginResize(long size, AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			FileRequestOptions fileRequestOptions = FileRequestOptions.ApplyDefaults(options, this.ServiceClient, true);
			return Executor.BeginExecuteAsync<NullType>(this.ResizeImpl(size, accessCondition, fileRequestOptions), fileRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x06000754 RID: 1876 RVA: 0x0001AF5D File Offset: 0x0001915D
		public void EndResize(IAsyncResult asyncResult)
		{
			Executor.EndExecuteAsync<NullType>(asyncResult);
		}

		// Token: 0x06000755 RID: 1877 RVA: 0x0001AF66 File Offset: 0x00019166
		[DoesServiceRequest]
		public Task ResizeAsync(long size)
		{
			return this.ResizeAsync(size, CancellationToken.None);
		}

		// Token: 0x06000756 RID: 1878 RVA: 0x0001AF74 File Offset: 0x00019174
		[DoesServiceRequest]
		public Task ResizeAsync(long size, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<long>(new Func<long, AsyncCallback, object, ICancellableAsyncResult>(this.BeginResize), new Action<IAsyncResult>(this.EndResize), size, cancellationToken);
		}

		// Token: 0x06000757 RID: 1879 RVA: 0x0001AF95 File Offset: 0x00019195
		[DoesServiceRequest]
		public Task ResizeAsync(long size, AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext)
		{
			return this.ResizeAsync(size, accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x06000758 RID: 1880 RVA: 0x0001AFA7 File Offset: 0x000191A7
		[DoesServiceRequest]
		public Task ResizeAsync(long size, AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<long, AccessCondition, FileRequestOptions, OperationContext>(new Func<long, AccessCondition, FileRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginResize), new Action<IAsyncResult>(this.EndResize), size, accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x06000759 RID: 1881 RVA: 0x0001AFD0 File Offset: 0x000191D0
		[DoesServiceRequest]
		public void SetMetadata(AccessCondition accessCondition = null, FileRequestOptions options = null, OperationContext operationContext = null)
		{
			FileRequestOptions fileRequestOptions = FileRequestOptions.ApplyDefaults(options, this.ServiceClient, true);
			Executor.ExecuteSync<NullType>(this.SetMetadataImpl(accessCondition, fileRequestOptions), fileRequestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x0600075A RID: 1882 RVA: 0x0001B000 File Offset: 0x00019200
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginSetMetadata(AsyncCallback callback, object state)
		{
			return this.BeginSetMetadata(null, null, null, callback, state);
		}

		// Token: 0x0600075B RID: 1883 RVA: 0x0001B010 File Offset: 0x00019210
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginSetMetadata(AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			FileRequestOptions fileRequestOptions = FileRequestOptions.ApplyDefaults(options, this.ServiceClient, true);
			return Executor.BeginExecuteAsync<NullType>(this.SetMetadataImpl(accessCondition, fileRequestOptions), fileRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x0600075C RID: 1884 RVA: 0x0001B043 File Offset: 0x00019243
		public void EndSetMetadata(IAsyncResult asyncResult)
		{
			Executor.EndExecuteAsync<NullType>(asyncResult);
		}

		// Token: 0x0600075D RID: 1885 RVA: 0x0001B04C File Offset: 0x0001924C
		[DoesServiceRequest]
		public Task SetMetadataAsync()
		{
			return this.SetMetadataAsync(CancellationToken.None);
		}

		// Token: 0x0600075E RID: 1886 RVA: 0x0001B059 File Offset: 0x00019259
		[DoesServiceRequest]
		public Task SetMetadataAsync(CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm(new Func<AsyncCallback, object, ICancellableAsyncResult>(this.BeginSetMetadata), new Action<IAsyncResult>(this.EndSetMetadata), cancellationToken);
		}

		// Token: 0x0600075F RID: 1887 RVA: 0x0001B079 File Offset: 0x00019279
		[DoesServiceRequest]
		public Task SetMetadataAsync(AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext)
		{
			return this.SetMetadataAsync(accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x06000760 RID: 1888 RVA: 0x0001B089 File Offset: 0x00019289
		[DoesServiceRequest]
		public Task SetMetadataAsync(AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<AccessCondition, FileRequestOptions, OperationContext>(new Func<AccessCondition, FileRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginSetMetadata), new Action<IAsyncResult>(this.EndSetMetadata), accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x06000761 RID: 1889 RVA: 0x0001B0B0 File Offset: 0x000192B0
		[DoesServiceRequest]
		public void WriteRange(Stream rangeData, long startOffset, string contentMD5 = null, AccessCondition accessCondition = null, FileRequestOptions options = null, OperationContext operationContext = null)
		{
			CommonUtility.AssertNotNull("rangeData", rangeData);
			FileRequestOptions fileRequestOptions = FileRequestOptions.ApplyDefaults(options, this.ServiceClient, true);
			bool flag = contentMD5 == null && fileRequestOptions.UseTransactionalMD5.Value;
			operationContext = (operationContext ?? new OperationContext());
			Stream stream = rangeData;
			bool flag2 = false;
			try
			{
				if (!rangeData.CanSeek || flag)
				{
					ExecutionState<NullType> executionState = CommonUtility.CreateTemporaryExecutionState(fileRequestOptions);
					Stream toStream;
					if (rangeData.CanSeek)
					{
						toStream = Stream.Null;
					}
					else
					{
						stream = new MultiBufferMemoryStream(this.ServiceClient.BufferManager, 65536);
						flag2 = true;
						toStream = stream;
					}
					long position = stream.Position;
					StreamDescriptor streamDescriptor = new StreamDescriptor();
					rangeData.WriteToSync(toStream, null, new long?(4194304L), flag, true, executionState, streamDescriptor);
					stream.Position = position;
					if (flag)
					{
						contentMD5 = streamDescriptor.Md5;
					}
				}
				Executor.ExecuteSync<NullType>(this.PutRangeImpl(stream, startOffset, contentMD5, accessCondition, fileRequestOptions), fileRequestOptions.RetryPolicy, operationContext);
			}
			finally
			{
				if (flag2)
				{
					stream.Dispose();
				}
			}
		}

		// Token: 0x06000762 RID: 1890 RVA: 0x0001B1BC File Offset: 0x000193BC
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginWriteRange(Stream rangeData, long startOffset, string contentMD5, AsyncCallback callback, object state)
		{
			return this.BeginWriteRange(rangeData, startOffset, contentMD5, null, null, null, callback, state);
		}

		// Token: 0x06000763 RID: 1891 RVA: 0x0001B2BC File Offset: 0x000194BC
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginWriteRange(Stream rangeData, long startOffset, string contentMD5, AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			CommonUtility.AssertNotNull("rangeData", rangeData);
			FileRequestOptions modifiedOptions = FileRequestOptions.ApplyDefaults(options, this.ServiceClient, true);
			bool requiresContentMD5 = contentMD5 == null && modifiedOptions.UseTransactionalMD5.Value;
			operationContext = (operationContext ?? new OperationContext());
			StorageAsyncResult<NullType> storageAsyncResult = new StorageAsyncResult<NullType>(callback, state);
			if (rangeData.CanSeek && !requiresContentMD5)
			{
				this.WriteRangeHandler(rangeData, startOffset, contentMD5, accessCondition, modifiedOptions, operationContext, storageAsyncResult);
			}
			else
			{
				ExecutionState<NullType> executionState = CommonUtility.CreateTemporaryExecutionState(modifiedOptions);
				storageAsyncResult.CancelDelegate = new Action(executionState.Cancel);
				Stream toStream;
				Stream seekableStream;
				if (rangeData.CanSeek)
				{
					seekableStream = rangeData;
					toStream = Stream.Null;
				}
				else
				{
					seekableStream = new MultiBufferMemoryStream(this.ServiceClient.BufferManager, 65536);
					storageAsyncResult.OperationState = seekableStream;
					toStream = seekableStream;
				}
				long startPosition = seekableStream.Position;
				StreamDescriptor streamCopyState = new StreamDescriptor();
				rangeData.WriteToAsync(toStream, null, new long?(4194304L), requiresContentMD5, executionState, streamCopyState, delegate(ExecutionState<NullType> completedState)
				{
					storageAsyncResult.UpdateCompletedSynchronously(completedState.CompletedSynchronously);
					if (completedState.ExceptionRef != null)
					{
						storageAsyncResult.OnComplete(completedState.ExceptionRef);
						return;
					}
					if (requiresContentMD5)
					{
						contentMD5 = streamCopyState.Md5;
					}
					seekableStream.Position = startPosition;
					this.WriteRangeHandler(seekableStream, startOffset, contentMD5, accessCondition, modifiedOptions, operationContext, storageAsyncResult);
				});
			}
			return storageAsyncResult;
		}

		// Token: 0x06000764 RID: 1892 RVA: 0x0001B4CC File Offset: 0x000196CC
		private void WriteRangeHandler(Stream rangeData, long startOffset, string contentMD5, AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext, StorageAsyncResult<NullType> storageAsyncResult)
		{
			lock (storageAsyncResult.CancellationLockerObject)
			{
				ICancellableAsyncResult @object = Executor.BeginExecuteAsync<NullType>(this.PutRangeImpl(rangeData, startOffset, contentMD5, accessCondition, options), options.RetryPolicy, operationContext, delegate(IAsyncResult ar)
				{
					storageAsyncResult.UpdateCompletedSynchronously(ar.CompletedSynchronously);
					try
					{
						Executor.EndExecuteAsync<NullType>(ar);
						storageAsyncResult.OnComplete();
					}
					catch (Exception exception)
					{
						storageAsyncResult.OnComplete(exception);
					}
				}, null);
				storageAsyncResult.CancelDelegate = new Action(@object.Cancel);
				if (storageAsyncResult.CancelRequested)
				{
					storageAsyncResult.Cancel();
				}
			}
		}

		// Token: 0x06000765 RID: 1893 RVA: 0x0001B57C File Offset: 0x0001977C
		public void EndWriteRange(IAsyncResult asyncResult)
		{
			StorageAsyncResult<NullType> storageAsyncResult = (StorageAsyncResult<NullType>)asyncResult;
			try
			{
				storageAsyncResult.End();
			}
			finally
			{
				if (storageAsyncResult.OperationState != null)
				{
					MultiBufferMemoryStream multiBufferMemoryStream = (MultiBufferMemoryStream)storageAsyncResult.OperationState;
					multiBufferMemoryStream.Dispose();
				}
			}
		}

		// Token: 0x06000766 RID: 1894 RVA: 0x0001B5C4 File Offset: 0x000197C4
		[DoesServiceRequest]
		public Task WriteRangeAsync(Stream rangeData, long startOffset, string contentMD5)
		{
			return this.WriteRangeAsync(rangeData, startOffset, contentMD5, CancellationToken.None);
		}

		// Token: 0x06000767 RID: 1895 RVA: 0x0001B5D4 File Offset: 0x000197D4
		[DoesServiceRequest]
		public Task WriteRangeAsync(Stream rangeData, long startOffset, string contentMD5, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<Stream, long, string>(new Func<Stream, long, string, AsyncCallback, object, ICancellableAsyncResult>(this.BeginWriteRange), new Action<IAsyncResult>(this.EndWriteRange), rangeData, startOffset, contentMD5, cancellationToken);
		}

		// Token: 0x06000768 RID: 1896 RVA: 0x0001B5F8 File Offset: 0x000197F8
		[DoesServiceRequest]
		public Task WriteRangeAsync(Stream rangeData, long startOffset, string contentMD5, AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext)
		{
			return this.WriteRangeAsync(rangeData, startOffset, contentMD5, accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x06000769 RID: 1897 RVA: 0x0001B610 File Offset: 0x00019810
		[DoesServiceRequest]
		public Task WriteRangeAsync(Stream rangeData, long startOffset, string contentMD5, AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<Stream, long, string, AccessCondition, FileRequestOptions, OperationContext>(new Func<Stream, long, string, AccessCondition, FileRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginWriteRange), new Action<IAsyncResult>(this.EndWriteRange), rangeData, startOffset, contentMD5, accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x0600076A RID: 1898 RVA: 0x0001B648 File Offset: 0x00019848
		[DoesServiceRequest]
		public void ClearRange(long startOffset, long length, AccessCondition accessCondition = null, FileRequestOptions options = null, OperationContext operationContext = null)
		{
			FileRequestOptions fileRequestOptions = FileRequestOptions.ApplyDefaults(options, this.ServiceClient, true);
			Executor.ExecuteSync<NullType>(this.ClearRangeImpl(startOffset, length, accessCondition, fileRequestOptions), fileRequestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x0600076B RID: 1899 RVA: 0x0001B67C File Offset: 0x0001987C
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginClearRange(long startOffset, long length, AsyncCallback callback, object state)
		{
			return this.BeginClearRange(startOffset, length, null, null, null, callback, state);
		}

		// Token: 0x0600076C RID: 1900 RVA: 0x0001B68C File Offset: 0x0001988C
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginClearRange(long startOffset, long length, AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			FileRequestOptions fileRequestOptions = FileRequestOptions.ApplyDefaults(options, this.ServiceClient, true);
			return Executor.BeginExecuteAsync<NullType>(this.ClearRangeImpl(startOffset, length, accessCondition, fileRequestOptions), fileRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x0600076D RID: 1901 RVA: 0x0001B6C3 File Offset: 0x000198C3
		public void EndClearRange(IAsyncResult asyncResult)
		{
			Executor.EndExecuteAsync<NullType>(asyncResult);
		}

		// Token: 0x0600076E RID: 1902 RVA: 0x0001B6CC File Offset: 0x000198CC
		[DoesServiceRequest]
		public Task ClearRangeAsync(long startOffset, long length)
		{
			return this.ClearRangeAsync(startOffset, length, CancellationToken.None);
		}

		// Token: 0x0600076F RID: 1903 RVA: 0x0001B6DB File Offset: 0x000198DB
		[DoesServiceRequest]
		public Task ClearRangeAsync(long startOffset, long length, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<long, long>(new Func<long, long, AsyncCallback, object, ICancellableAsyncResult>(this.BeginClearRange), new Action<IAsyncResult>(this.EndClearRange), startOffset, length, cancellationToken);
		}

		// Token: 0x06000770 RID: 1904 RVA: 0x0001B6FD File Offset: 0x000198FD
		[DoesServiceRequest]
		public Task ClearRangeAsync(long startOffset, long length, AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext)
		{
			return this.ClearRangeAsync(startOffset, length, accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x06000771 RID: 1905 RVA: 0x0001B711 File Offset: 0x00019911
		[DoesServiceRequest]
		public Task ClearRangeAsync(long startOffset, long length, AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<long, long, AccessCondition, FileRequestOptions, OperationContext>(new Func<long, long, AccessCondition, FileRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginClearRange), new Action<IAsyncResult>(this.EndClearRange), startOffset, length, accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x06000772 RID: 1906 RVA: 0x0001B73C File Offset: 0x0001993C
		[DoesServiceRequest]
		public string StartCopy(Uri source, AccessCondition sourceAccessCondition = null, AccessCondition destAccessCondition = null, FileRequestOptions options = null, OperationContext operationContext = null)
		{
			CommonUtility.AssertNotNull("source", source);
			FileRequestOptions fileRequestOptions = FileRequestOptions.ApplyDefaults(options, this.ServiceClient, true);
			return Executor.ExecuteSync<string>(this.StartCopyImpl(source, sourceAccessCondition, destAccessCondition, fileRequestOptions), fileRequestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x06000773 RID: 1907 RVA: 0x0001B77A File Offset: 0x0001997A
		[DoesServiceRequest]
		public string StartCopy(CloudFile source, AccessCondition sourceAccessCondition = null, AccessCondition destAccessCondition = null, FileRequestOptions options = null, OperationContext operationContext = null)
		{
			return this.StartCopy(CloudFile.SourceFileToUri(source), sourceAccessCondition, destAccessCondition, options, operationContext);
		}

		// Token: 0x06000774 RID: 1908 RVA: 0x0001B78E File Offset: 0x0001998E
		[DoesServiceRequest]
		public string StartCopy(CloudBlob source, AccessCondition sourceAccessCondition = null, AccessCondition destAccessCondition = null, FileRequestOptions options = null, OperationContext operationContext = null)
		{
			return this.StartCopy(CloudBlob.SourceBlobToUri(source), sourceAccessCondition, destAccessCondition, options, operationContext);
		}

		// Token: 0x06000775 RID: 1909 RVA: 0x0001B7A2 File Offset: 0x000199A2
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginStartCopy(Uri source, AsyncCallback callback, object state)
		{
			return this.BeginStartCopy(source, null, null, null, null, callback, state);
		}

		// Token: 0x06000776 RID: 1910 RVA: 0x0001B7B1 File Offset: 0x000199B1
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginStartCopy(CloudFile source, AsyncCallback callback, object state)
		{
			return this.BeginStartCopy(CloudFile.SourceFileToUri(source), callback, state);
		}

		// Token: 0x06000777 RID: 1911 RVA: 0x0001B7C1 File Offset: 0x000199C1
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginStartCopy(CloudBlob source, AsyncCallback callback, object state)
		{
			return this.BeginStartCopy(CloudBlob.SourceBlobToUri(source), callback, state);
		}

		// Token: 0x06000778 RID: 1912 RVA: 0x0001B7D4 File Offset: 0x000199D4
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginStartCopy(Uri source, AccessCondition sourceAccessCondition, AccessCondition destAccessCondition, FileRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			CommonUtility.AssertNotNull("source", source);
			FileRequestOptions fileRequestOptions = FileRequestOptions.ApplyDefaults(options, this.ServiceClient, true);
			return Executor.BeginExecuteAsync<string>(this.StartCopyImpl(source, sourceAccessCondition, destAccessCondition, fileRequestOptions), fileRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x06000779 RID: 1913 RVA: 0x0001B816 File Offset: 0x00019A16
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginStartCopy(CloudFile source, AccessCondition sourceAccessCondition, AccessCondition destAccessCondition, FileRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			return this.BeginStartCopy(CloudFile.SourceFileToUri(source), sourceAccessCondition, destAccessCondition, options, operationContext, callback, state);
		}

		// Token: 0x0600077A RID: 1914 RVA: 0x0001B82E File Offset: 0x00019A2E
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginStartCopy(CloudBlob source, AccessCondition sourceAccessCondition, AccessCondition destAccessCondition, FileRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			return this.BeginStartCopy(CloudBlob.SourceBlobToUri(source), sourceAccessCondition, destAccessCondition, options, operationContext, callback, state);
		}

		// Token: 0x0600077B RID: 1915 RVA: 0x0001B846 File Offset: 0x00019A46
		public string EndStartCopy(IAsyncResult asyncResult)
		{
			return Executor.EndExecuteAsync<string>(asyncResult);
		}

		// Token: 0x0600077C RID: 1916 RVA: 0x0001B84E File Offset: 0x00019A4E
		[DoesServiceRequest]
		public Task<string> StartCopyAsync(Uri source)
		{
			return this.StartCopyAsync(source, CancellationToken.None);
		}

		// Token: 0x0600077D RID: 1917 RVA: 0x0001B85C File Offset: 0x00019A5C
		[DoesServiceRequest]
		public Task<string> StartCopyAsync(Uri source, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<Uri, string>(new Func<Uri, AsyncCallback, object, ICancellableAsyncResult>(this.BeginStartCopy), new Func<IAsyncResult, string>(this.EndStartCopy), source, cancellationToken);
		}

		// Token: 0x0600077E RID: 1918 RVA: 0x0001B87D File Offset: 0x00019A7D
		[DoesServiceRequest]
		public Task<string> StartCopyAsync(CloudFile source)
		{
			return this.StartCopyAsync(source, CancellationToken.None);
		}

		// Token: 0x0600077F RID: 1919 RVA: 0x0001B88B File Offset: 0x00019A8B
		[DoesServiceRequest]
		public Task<string> StartCopyAsync(CloudBlob source)
		{
			return this.StartCopyAsync(source, CancellationToken.None);
		}

		// Token: 0x06000780 RID: 1920 RVA: 0x0001B899 File Offset: 0x00019A99
		[DoesServiceRequest]
		public Task<string> StartCopyAsync(CloudFile source, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<CloudFile, string>(new Func<CloudFile, AsyncCallback, object, ICancellableAsyncResult>(this.BeginStartCopy), new Func<IAsyncResult, string>(this.EndStartCopy), source, cancellationToken);
		}

		// Token: 0x06000781 RID: 1921 RVA: 0x0001B8BA File Offset: 0x00019ABA
		[DoesServiceRequest]
		public Task<string> StartCopyAsync(CloudBlob source, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<CloudBlob, string>(new Func<CloudBlob, AsyncCallback, object, ICancellableAsyncResult>(this.BeginStartCopy), new Func<IAsyncResult, string>(this.EndStartCopy), source, cancellationToken);
		}

		// Token: 0x06000782 RID: 1922 RVA: 0x0001B8DB File Offset: 0x00019ADB
		[DoesServiceRequest]
		public Task<string> StartCopyAsync(Uri source, AccessCondition sourceAccessCondition, AccessCondition destAccessCondition, FileRequestOptions options, OperationContext operationContext)
		{
			return this.StartCopyAsync(source, sourceAccessCondition, destAccessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x06000783 RID: 1923 RVA: 0x0001B8EF File Offset: 0x00019AEF
		[DoesServiceRequest]
		public Task<string> StartCopyAsync(Uri source, AccessCondition sourceAccessCondition, AccessCondition destAccessCondition, FileRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<Uri, AccessCondition, AccessCondition, FileRequestOptions, OperationContext, string>(new Func<Uri, AccessCondition, AccessCondition, FileRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginStartCopy), new Func<IAsyncResult, string>(this.EndStartCopy), source, sourceAccessCondition, destAccessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x06000784 RID: 1924 RVA: 0x0001B917 File Offset: 0x00019B17
		[DoesServiceRequest]
		public Task<string> StartCopyAsync(CloudFile source, AccessCondition sourceAccessCondition, AccessCondition destAccessCondition, FileRequestOptions options, OperationContext operationContext)
		{
			return this.StartCopyAsync(source, sourceAccessCondition, destAccessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x06000785 RID: 1925 RVA: 0x0001B92B File Offset: 0x00019B2B
		[DoesServiceRequest]
		public Task<string> StartCopyAsync(CloudBlob source, AccessCondition sourceAccessCondition, AccessCondition destAccessCondition, FileRequestOptions options, OperationContext operationContext)
		{
			return this.StartCopyAsync(source, sourceAccessCondition, destAccessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x06000786 RID: 1926 RVA: 0x0001B93F File Offset: 0x00019B3F
		[DoesServiceRequest]
		public Task<string> StartCopyAsync(CloudFile source, AccessCondition sourceAccessCondition, AccessCondition destAccessCondition, FileRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<CloudFile, AccessCondition, AccessCondition, FileRequestOptions, OperationContext, string>(new Func<CloudFile, AccessCondition, AccessCondition, FileRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginStartCopy), new Func<IAsyncResult, string>(this.EndStartCopy), source, sourceAccessCondition, destAccessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x06000787 RID: 1927 RVA: 0x0001B967 File Offset: 0x00019B67
		[DoesServiceRequest]
		public Task<string> StartCopyAsync(CloudBlob source, AccessCondition sourceAccessCondition, AccessCondition destAccessCondition, FileRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<CloudBlob, AccessCondition, AccessCondition, FileRequestOptions, OperationContext, string>(new Func<CloudBlob, AccessCondition, AccessCondition, FileRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginStartCopy), new Func<IAsyncResult, string>(this.EndStartCopy), source, sourceAccessCondition, destAccessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x06000788 RID: 1928 RVA: 0x0001B990 File Offset: 0x00019B90
		[DoesServiceRequest]
		public void AbortCopy(string copyId, AccessCondition accessCondition = null, FileRequestOptions options = null, OperationContext operationContext = null)
		{
			FileRequestOptions fileRequestOptions = FileRequestOptions.ApplyDefaults(options, this.ServiceClient, true);
			Executor.ExecuteSync<NullType>(this.AbortCopyImpl(copyId, accessCondition, fileRequestOptions), fileRequestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x06000789 RID: 1929 RVA: 0x0001B9C2 File Offset: 0x00019BC2
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginAbortCopy(string copyId, AsyncCallback callback, object state)
		{
			return this.BeginAbortCopy(copyId, null, null, null, callback, state);
		}

		// Token: 0x0600078A RID: 1930 RVA: 0x0001B9D0 File Offset: 0x00019BD0
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginAbortCopy(string copyId, AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			FileRequestOptions fileRequestOptions = FileRequestOptions.ApplyDefaults(options, this.ServiceClient, true);
			return Executor.BeginExecuteAsync<NullType>(this.AbortCopyImpl(copyId, accessCondition, fileRequestOptions), fileRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x0600078B RID: 1931 RVA: 0x0001BA05 File Offset: 0x00019C05
		public void EndAbortCopy(IAsyncResult asyncResult)
		{
			Executor.EndExecuteAsync<NullType>(asyncResult);
		}

		// Token: 0x0600078C RID: 1932 RVA: 0x0001BA0E File Offset: 0x00019C0E
		[DoesServiceRequest]
		public Task AbortCopyAsync(string copyId)
		{
			return this.AbortCopyAsync(copyId, CancellationToken.None);
		}

		// Token: 0x0600078D RID: 1933 RVA: 0x0001BA1C File Offset: 0x00019C1C
		[DoesServiceRequest]
		public Task AbortCopyAsync(string copyId, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<string>(new Func<string, AsyncCallback, object, ICancellableAsyncResult>(this.BeginAbortCopy), new Action<IAsyncResult>(this.EndAbortCopy), copyId, cancellationToken);
		}

		// Token: 0x0600078E RID: 1934 RVA: 0x0001BA3D File Offset: 0x00019C3D
		[DoesServiceRequest]
		public Task AbortCopyAsync(string copyId, AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext)
		{
			return this.AbortCopyAsync(copyId, accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x0600078F RID: 1935 RVA: 0x0001BA4F File Offset: 0x00019C4F
		[DoesServiceRequest]
		public Task AbortCopyAsync(string copyId, AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<string, AccessCondition, FileRequestOptions, OperationContext>(new Func<string, AccessCondition, FileRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginAbortCopy), new Action<IAsyncResult>(this.EndAbortCopy), copyId, accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x06000790 RID: 1936 RVA: 0x0001BD48 File Offset: 0x00019F48
		private RESTCommand<NullType> GetFileImpl(Stream destStream, long? offset, long? length, AccessCondition accessCondition, FileRequestOptions options)
		{
			string lockedETag = null;
			AccessCondition lockedAccessCondition = null;
			bool isRangeGet = offset != null;
			bool arePropertiesPopulated = false;
			string storedMD5 = null;
			long startingOffset = (offset != null) ? offset.Value : 0L;
			long? startingLength = length;
			long? validateLength = null;
			RESTCommand<NullType> getCmd = new RESTCommand<NullType>(this.ServiceClient.Credentials, this.StorageUri);
			options.ApplyToStorageCommand<NullType>(getCmd);
			getCmd.CommandLocationMode = CommandLocationMode.PrimaryOrSecondary;
			getCmd.RetrieveResponseStream = true;
			getCmd.DestinationStream = destStream;
			getCmd.CalculateMd5ForResponseStream = !options.DisableContentMD5Validation.Value;
			getCmd.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => FileHttpWebRequestFactory.Get(uri, serverTimeout, offset, length, options.UseTransactionalMD5.Value, accessCondition, useVersionHeader, ctx));
			getCmd.SignRequest = new Action<HttpWebRequest, OperationContext>(this.ServiceClient.AuthenticationHandler.SignRequest);
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
				getCmd.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext context) => FileHttpWebRequestFactory.Get(uri, serverTimeout, offset, length, options.UseTransactionalMD5.Value && !arePropertiesPopulated, accessCondition, useVersionHeader, context));
			};
			getCmd.PreProcessResponse = delegate(RESTCommand<NullType> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx)
			{
				HttpResponseParsers.ProcessExpectedStatusCodeNoException<NullType>((offset != null) ? HttpStatusCode.PartialContent : HttpStatusCode.OK, resp, NullType.Value, cmd, ex);
				if (!arePropertiesPopulated)
				{
					this.UpdateAfterFetchAttributes(resp, isRangeGet);
					storedMD5 = resp.Headers[HttpResponseHeader.ContentMd5];
					if (!options.DisableContentMD5Validation.Value && options.UseTransactionalMD5.Value && string.IsNullOrEmpty(storedMD5))
					{
						throw new StorageException(cmd.CurrentResult, "MD5 does not exist. If you do not want to force validation, please disable UseTransactionalMD5.", null)
						{
							IsRetryable = false
						};
					}
					getCmd.CommandLocationMode = ((cmd.CurrentResult.TargetLocation == StorageLocation.Primary) ? CommandLocationMode.PrimaryOnly : CommandLocationMode.SecondaryOnly);
					lockedETag = this.attributes.Properties.ETag;
					if (resp.ContentLength >= 0L)
					{
						validateLength = new long?(resp.ContentLength);
					}
					arePropertiesPopulated = true;
				}
				else if (!HttpResponseParsers.GetETag(resp).Equals(lockedETag, StringComparison.Ordinal))
				{
					throw new StorageException(new RequestResult
					{
						HttpStatusMessage = null,
						HttpStatusCode = 412,
						ExtendedErrorInformation = null
					}, "The condition specified using HTTP conditional header(s) is not met.", null);
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

		// Token: 0x06000791 RID: 1937 RVA: 0x0001BF54 File Offset: 0x0001A154
		private RESTCommand<NullType> CreateImpl(long sizeInBytes, AccessCondition accessCondition, FileRequestOptions options)
		{
			RESTCommand<NullType> restcommand = new RESTCommand<NullType>(this.ServiceClient.Credentials, this.StorageUri);
			options.ApplyToStorageCommand<NullType>(restcommand);
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => FileHttpWebRequestFactory.Create(uri, serverTimeout, this.Properties, sizeInBytes, accessCondition, useVersionHeader, ctx));
			restcommand.SetHeaders = delegate(HttpWebRequest r, OperationContext ctx)
			{
				FileHttpWebRequestFactory.AddMetadata(r, this.Metadata);
			};
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(this.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = delegate(RESTCommand<NullType> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx)
			{
				HttpResponseParsers.ProcessExpectedStatusCodeNoException<NullType>(HttpStatusCode.Created, resp, NullType.Value, cmd, ex);
				this.UpdateETagLMTAndLength(resp, false);
				this.Properties.Length = sizeInBytes;
				return NullType.Value;
			};
			return restcommand;
		}

		// Token: 0x06000792 RID: 1938 RVA: 0x0001C02C File Offset: 0x0001A22C
		private RESTCommand<NullType> FetchAttributesImpl(AccessCondition accessCondition, FileRequestOptions options)
		{
			RESTCommand<NullType> restcommand = new RESTCommand<NullType>(this.ServiceClient.Credentials, this.StorageUri);
			options.ApplyToStorageCommand<NullType>(restcommand);
			restcommand.CommandLocationMode = CommandLocationMode.PrimaryOrSecondary;
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => FileHttpWebRequestFactory.GetProperties(uri, serverTimeout, accessCondition, useVersionHeader, ctx));
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(this.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = delegate(RESTCommand<NullType> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx)
			{
				HttpResponseParsers.ProcessExpectedStatusCodeNoException<NullType>(HttpStatusCode.OK, resp, NullType.Value, cmd, ex);
				this.UpdateAfterFetchAttributes(resp, false);
				return NullType.Value;
			};
			return restcommand;
		}

		// Token: 0x06000793 RID: 1939 RVA: 0x0001C0EC File Offset: 0x0001A2EC
		private RESTCommand<bool> ExistsImpl(FileRequestOptions options)
		{
			RESTCommand<bool> restcommand = new RESTCommand<bool>(this.ServiceClient.Credentials, this.StorageUri);
			options.ApplyToStorageCommand<bool>(restcommand);
			restcommand.CommandLocationMode = CommandLocationMode.PrimaryOrSecondary;
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => FileHttpWebRequestFactory.GetProperties(uri, serverTimeout, null, useVersionHeader, ctx));
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(this.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = delegate(RESTCommand<bool> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx)
			{
				if (resp.StatusCode == HttpStatusCode.NotFound)
				{
					return false;
				}
				HttpResponseParsers.ProcessExpectedStatusCodeNoException<bool>(HttpStatusCode.OK, resp, true, cmd, ex);
				this.UpdateAfterFetchAttributes(resp, false);
				return true;
			};
			return restcommand;
		}

		// Token: 0x06000794 RID: 1940 RVA: 0x0001C1A0 File Offset: 0x0001A3A0
		private RESTCommand<NullType> DeleteFileImpl(AccessCondition accessCondition, FileRequestOptions options)
		{
			RESTCommand<NullType> restcommand = new RESTCommand<NullType>(this.ServiceClient.Credentials, this.StorageUri);
			options.ApplyToStorageCommand<NullType>(restcommand);
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => FileHttpWebRequestFactory.Delete(uri, serverTimeout, accessCondition, useVersionHeader, ctx));
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(this.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = ((RESTCommand<NullType> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx) => HttpResponseParsers.ProcessExpectedStatusCodeNoException<NullType>(HttpStatusCode.Accepted, resp, NullType.Value, cmd, ex));
			return restcommand;
		}

		// Token: 0x06000795 RID: 1941 RVA: 0x0001C2A0 File Offset: 0x0001A4A0
		private RESTCommand<IEnumerable<FileRange>> ListRangesImpl(long? offset, long? length, AccessCondition accessCondition, FileRequestOptions options)
		{
			RESTCommand<IEnumerable<FileRange>> restcommand = new RESTCommand<IEnumerable<FileRange>>(this.ServiceClient.Credentials, this.StorageUri);
			options.ApplyToStorageCommand<IEnumerable<FileRange>>(restcommand);
			restcommand.CommandLocationMode = CommandLocationMode.PrimaryOrSecondary;
			restcommand.RetrieveResponseStream = true;
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => FileHttpWebRequestFactory.ListRanges(uri, serverTimeout, offset, length, accessCondition, useVersionHeader, ctx));
			restcommand.SetHeaders = delegate(HttpWebRequest r, OperationContext ctx)
			{
				FileHttpWebRequestFactory.AddMetadata(r, this.Metadata);
			};
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(this.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = ((RESTCommand<IEnumerable<FileRange>> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx) => HttpResponseParsers.ProcessExpectedStatusCodeNoException<IEnumerable<FileRange>>(HttpStatusCode.OK, resp, null, cmd, ex));
			restcommand.PostProcessResponse = delegate(RESTCommand<IEnumerable<FileRange>> cmd, HttpWebResponse resp, OperationContext ctx)
			{
				this.UpdateETagLMTAndLength(resp, true);
				ListRangesResponse listRangesResponse = new ListRangesResponse(cmd.ResponseStream);
				return listRangesResponse.Ranges.ToList<FileRange>();
			};
			return restcommand;
		}

		// Token: 0x06000796 RID: 1942 RVA: 0x0001C3CC File Offset: 0x0001A5CC
		private RESTCommand<NullType> SetPropertiesImpl(AccessCondition accessCondition, FileRequestOptions options)
		{
			RESTCommand<NullType> restcommand = new RESTCommand<NullType>(this.ServiceClient.Credentials, this.StorageUri);
			options.ApplyToStorageCommand<NullType>(restcommand);
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => FileHttpWebRequestFactory.SetProperties(uri, serverTimeout, this.Properties, accessCondition, useVersionHeader, ctx));
			restcommand.SetHeaders = delegate(HttpWebRequest r, OperationContext ctx)
			{
				FileHttpWebRequestFactory.AddMetadata(r, this.Metadata);
			};
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(this.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = delegate(RESTCommand<NullType> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx)
			{
				HttpResponseParsers.ProcessExpectedStatusCodeNoException<NullType>(HttpStatusCode.OK, resp, NullType.Value, cmd, ex);
				this.UpdateETagLMTAndLength(resp, false);
				return NullType.Value;
			};
			return restcommand;
		}

		// Token: 0x06000797 RID: 1943 RVA: 0x0001C4C0 File Offset: 0x0001A6C0
		private RESTCommand<NullType> ResizeImpl(long sizeInBytes, AccessCondition accessCondition, FileRequestOptions options)
		{
			RESTCommand<NullType> restcommand = new RESTCommand<NullType>(this.ServiceClient.Credentials, this.StorageUri);
			options.ApplyToStorageCommand<NullType>(restcommand);
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => FileHttpWebRequestFactory.Resize(uri, serverTimeout, sizeInBytes, accessCondition, useVersionHeader, ctx));
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(this.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = delegate(RESTCommand<NullType> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx)
			{
				HttpResponseParsers.ProcessExpectedStatusCodeNoException<NullType>(HttpStatusCode.OK, resp, NullType.Value, cmd, ex);
				this.UpdateETagLMTAndLength(resp, false);
				this.Properties.Length = sizeInBytes;
				return NullType.Value;
			};
			return restcommand;
		}

		// Token: 0x06000798 RID: 1944 RVA: 0x0001C594 File Offset: 0x0001A794
		private RESTCommand<NullType> SetMetadataImpl(AccessCondition accessCondition, FileRequestOptions options)
		{
			RESTCommand<NullType> restcommand = new RESTCommand<NullType>(this.ServiceClient.Credentials, this.StorageUri);
			options.ApplyToStorageCommand<NullType>(restcommand);
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => FileHttpWebRequestFactory.SetMetadata(uri, serverTimeout, accessCondition, useVersionHeader, ctx));
			restcommand.SetHeaders = delegate(HttpWebRequest r, OperationContext ctx)
			{
				FileHttpWebRequestFactory.AddMetadata(r, this.Metadata);
			};
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(this.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = delegate(RESTCommand<NullType> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx)
			{
				HttpResponseParsers.ProcessExpectedStatusCodeNoException<NullType>(HttpStatusCode.OK, resp, NullType.Value, cmd, ex);
				this.UpdateETagLMTAndLength(resp, false);
				return NullType.Value;
			};
			return restcommand;
		}

		// Token: 0x06000799 RID: 1945 RVA: 0x0001C6A0 File Offset: 0x0001A8A0
		private RESTCommand<NullType> PutRangeImpl(Stream rangeData, long startOffset, string contentMD5, AccessCondition accessCondition, FileRequestOptions options)
		{
			long offset = rangeData.Position;
			long num = rangeData.Length - offset;
			FileRange fileRange = new FileRange(startOffset, startOffset + num - 1L);
			FileRangeWrite fileRangeWrite = FileRangeWrite.Update;
			if (1L + fileRange.EndOffset - fileRange.StartOffset == 0L)
			{
				CommonUtility.ArgumentOutOfRange("rangeData", rangeData);
			}
			RESTCommand<NullType> restcommand = new RESTCommand<NullType>(this.ServiceClient.Credentials, this.StorageUri);
			options.ApplyToStorageCommand<NullType>(restcommand);
			restcommand.SendStream = rangeData;
			restcommand.RecoveryAction = delegate(StorageCommandBase<NullType> cmd, Exception ex, OperationContext ctx)
			{
				RecoveryActions.SeekStream<NullType>(cmd, offset);
			};
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => FileHttpWebRequestFactory.PutRange(uri, serverTimeout, fileRange, fileRangeWrite, accessCondition, useVersionHeader, ctx));
			restcommand.SetHeaders = delegate(HttpWebRequest r, OperationContext ctx)
			{
				if (!string.IsNullOrEmpty(contentMD5))
				{
					r.Headers[HttpRequestHeader.ContentMd5] = contentMD5;
				}
			};
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(this.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = delegate(RESTCommand<NullType> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx)
			{
				HttpResponseParsers.ProcessExpectedStatusCodeNoException<NullType>(HttpStatusCode.Created, resp, NullType.Value, cmd, ex);
				this.UpdateETagLMTAndLength(resp, false);
				return NullType.Value;
			};
			return restcommand;
		}

		// Token: 0x0600079A RID: 1946 RVA: 0x0001C7FC File Offset: 0x0001A9FC
		private RESTCommand<NullType> ClearRangeImpl(long startOffset, long length, AccessCondition accessCondition, FileRequestOptions options)
		{
			CommonUtility.AssertNotNull("options", options);
			if (startOffset < 0L)
			{
				CommonUtility.ArgumentOutOfRange("startOffset", startOffset);
			}
			if (length <= 0L)
			{
				CommonUtility.ArgumentOutOfRange("length", length);
			}
			FileRange fileRange = new FileRange(startOffset, startOffset + length - 1L);
			FileRangeWrite fileWrite = FileRangeWrite.Clear;
			RESTCommand<NullType> restcommand = new RESTCommand<NullType>(this.ServiceClient.Credentials, this.StorageUri);
			options.ApplyToStorageCommand<NullType>(restcommand);
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => FileHttpWebRequestFactory.PutRange(uri, serverTimeout, fileRange, fileWrite, accessCondition, useVersionHeader, ctx));
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(this.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = delegate(RESTCommand<NullType> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx)
			{
				HttpResponseParsers.ProcessExpectedStatusCodeNoException<NullType>(HttpStatusCode.Created, resp, NullType.Value, cmd, ex);
				this.UpdateETagLMTAndLength(resp, false);
				return NullType.Value;
			};
			return restcommand;
		}

		// Token: 0x0600079B RID: 1947 RVA: 0x0001C960 File Offset: 0x0001AB60
		private RESTCommand<string> StartCopyImpl(Uri source, AccessCondition sourceAccessCondition, AccessCondition destAccessCondition, FileRequestOptions options)
		{
			if (sourceAccessCondition != null && !string.IsNullOrEmpty(sourceAccessCondition.LeaseId))
			{
				throw new ArgumentException("A lease condition cannot be specified on the source of a copy.", "sourceAccessCondition");
			}
			RESTCommand<string> restcommand = new RESTCommand<string>(this.ServiceClient.Credentials, this.attributes.StorageUri);
			options.ApplyToStorageCommand<string>(restcommand);
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => FileHttpWebRequestFactory.CopyFrom(uri, serverTimeout, source, sourceAccessCondition, destAccessCondition, useVersionHeader, ctx));
			restcommand.SetHeaders = delegate(HttpWebRequest r, OperationContext ctx)
			{
				FileHttpWebRequestFactory.AddMetadata(r, this.attributes.Metadata);
			};
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(this.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = delegate(RESTCommand<string> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx)
			{
				HttpResponseParsers.ProcessExpectedStatusCodeNoException<string>(HttpStatusCode.Accepted, resp, null, cmd, ex);
				CopyState copyAttributes = FileHttpResponseParsers.GetCopyAttributes(resp);
				this.attributes.Properties = FileHttpResponseParsers.GetProperties(resp);
				this.attributes.Metadata = FileHttpResponseParsers.GetMetadata(resp);
				this.attributes.CopyState = copyAttributes;
				return copyAttributes.CopyId;
			};
			return restcommand;
		}

		// Token: 0x0600079C RID: 1948 RVA: 0x0001CA68 File Offset: 0x0001AC68
		private RESTCommand<NullType> AbortCopyImpl(string copyId, AccessCondition accessCondition, FileRequestOptions options)
		{
			CommonUtility.AssertNotNull("copyId", copyId);
			RESTCommand<NullType> restcommand = new RESTCommand<NullType>(this.ServiceClient.Credentials, this.attributes.StorageUri);
			options.ApplyToStorageCommand<NullType>(restcommand);
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => FileHttpWebRequestFactory.AbortCopy(uri, serverTimeout, copyId, accessCondition, useVersionHeader, ctx));
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(this.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = ((RESTCommand<NullType> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx) => HttpResponseParsers.ProcessExpectedStatusCodeNoException<NullType>(HttpStatusCode.NoContent, resp, NullType.Value, cmd, ex));
			return restcommand;
		}

		// Token: 0x0600079D RID: 1949 RVA: 0x0001CB10 File Offset: 0x0001AD10
		private static void FileStreamCommitCallback(IAsyncResult result)
		{
			StorageAsyncResult<NullType> storageAsyncResult = (StorageAsyncResult<NullType>)result.AsyncState;
			CloudFileStream cloudFileStream = (CloudFileStream)storageAsyncResult.OperationState;
			storageAsyncResult.UpdateCompletedSynchronously(result.CompletedSynchronously);
			try
			{
				cloudFileStream.EndCommit(result);
				cloudFileStream.Dispose();
				storageAsyncResult.OnComplete();
			}
			catch (Exception exception)
			{
				storageAsyncResult.OnComplete(exception);
			}
		}

		// Token: 0x0600079E RID: 1950 RVA: 0x0001CB70 File Offset: 0x0001AD70
		internal static Uri SourceFileToUri(CloudFile source)
		{
			CommonUtility.AssertNotNull("source", source);
			return source.ServiceClient.Credentials.TransformUri(source.Uri);
		}

		// Token: 0x0600079F RID: 1951 RVA: 0x0001CB94 File Offset: 0x0001AD94
		private void UpdateAfterFetchAttributes(HttpWebResponse response, bool ignoreMD5)
		{
			FileProperties properties = FileHttpResponseParsers.GetProperties(response);
			CopyState copyAttributes = FileHttpResponseParsers.GetCopyAttributes(response);
			if (ignoreMD5)
			{
				properties.ContentMD5 = this.attributes.Properties.ContentMD5;
			}
			this.attributes.Properties = properties;
			this.attributes.Metadata = FileHttpResponseParsers.GetMetadata(response);
			this.attributes.CopyState = copyAttributes;
		}

		// Token: 0x060007A0 RID: 1952 RVA: 0x0001CBF4 File Offset: 0x0001ADF4
		private void UpdateETagLMTAndLength(HttpWebResponse response, bool updateLength)
		{
			FileProperties properties = FileHttpResponseParsers.GetProperties(response);
			this.Properties.ETag = (properties.ETag ?? this.Properties.ETag);
			FileProperties properties2 = this.Properties;
			DateTimeOffset? lastModified = properties.LastModified;
			properties2.LastModified = ((lastModified != null) ? new DateTimeOffset?(lastModified.GetValueOrDefault()) : this.Properties.LastModified);
			if (updateLength)
			{
				this.Properties.Length = properties.Length;
			}
		}

		// Token: 0x060007A1 RID: 1953 RVA: 0x0001CC70 File Offset: 0x0001AE70
		public CloudFile(Uri fileAbsoluteUri) : this(fileAbsoluteUri, null)
		{
		}

		// Token: 0x060007A2 RID: 1954 RVA: 0x0001CC7A File Offset: 0x0001AE7A
		public CloudFile(Uri fileAbsoluteUri, StorageCredentials credentials) : this(new StorageUri(fileAbsoluteUri), credentials)
		{
		}

		// Token: 0x060007A3 RID: 1955 RVA: 0x0001CC89 File Offset: 0x0001AE89
		public CloudFile(StorageUri fileAbsoluteUri, StorageCredentials credentials)
		{
			this.streamWriteSizeInBytes = 4194304;
			this.streamMinimumReadSizeInBytes = 4194304;
			base..ctor();
			this.attributes = new CloudFileAttributes();
			this.ParseQueryAndVerify(fileAbsoluteUri, credentials);
		}

		// Token: 0x060007A4 RID: 1956 RVA: 0x0001CCBC File Offset: 0x0001AEBC
		internal CloudFile(StorageUri uri, string fileName, CloudFileShare share)
		{
			this.streamWriteSizeInBytes = 4194304;
			this.streamMinimumReadSizeInBytes = 4194304;
			base..ctor();
			CommonUtility.AssertNotNull("uri", uri);
			CommonUtility.AssertNotNullOrEmpty("fileName", fileName);
			CommonUtility.AssertNotNull("share", share);
			this.attributes = new CloudFileAttributes();
			this.attributes.StorageUri = uri;
			this.ServiceClient = share.ServiceClient;
			this.share = share;
			this.Name = fileName;
		}

		// Token: 0x060007A5 RID: 1957 RVA: 0x0001CD38 File Offset: 0x0001AF38
		internal CloudFile(CloudFileAttributes attributes, CloudFileClient serviceClient)
		{
			this.streamWriteSizeInBytes = 4194304;
			this.streamMinimumReadSizeInBytes = 4194304;
			base..ctor();
			this.attributes = attributes;
			this.ServiceClient = serviceClient;
			this.ParseQueryAndVerify(this.StorageUri, this.ServiceClient.Credentials);
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x060007A6 RID: 1958 RVA: 0x0001CD86 File Offset: 0x0001AF86
		// (set) Token: 0x060007A7 RID: 1959 RVA: 0x0001CD8E File Offset: 0x0001AF8E
		public CloudFileClient ServiceClient { get; private set; }

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x060007A8 RID: 1960 RVA: 0x0001CD97 File Offset: 0x0001AF97
		// (set) Token: 0x060007A9 RID: 1961 RVA: 0x0001CD9F File Offset: 0x0001AF9F
		public int StreamWriteSizeInBytes
		{
			get
			{
				return this.streamWriteSizeInBytes;
			}
			set
			{
				CommonUtility.AssertInBounds<int>("StreamWriteSizeInBytes", value, 512, 4194304);
				this.streamWriteSizeInBytes = value;
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x060007AA RID: 1962 RVA: 0x0001CDBD File Offset: 0x0001AFBD
		// (set) Token: 0x060007AB RID: 1963 RVA: 0x0001CDC5 File Offset: 0x0001AFC5
		public int StreamMinimumReadSizeInBytes
		{
			get
			{
				return this.streamMinimumReadSizeInBytes;
			}
			set
			{
				CommonUtility.AssertInBounds<long>("StreamMinimumReadSizeInBytes", (long)value, 16384L);
				this.streamMinimumReadSizeInBytes = value;
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x060007AC RID: 1964 RVA: 0x0001CDE0 File Offset: 0x0001AFE0
		public FileProperties Properties
		{
			get
			{
				return this.attributes.Properties;
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x060007AD RID: 1965 RVA: 0x0001CDED File Offset: 0x0001AFED
		public IDictionary<string, string> Metadata
		{
			get
			{
				return this.attributes.Metadata;
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x060007AE RID: 1966 RVA: 0x0001CDFA File Offset: 0x0001AFFA
		public Uri Uri
		{
			get
			{
				return this.attributes.Uri;
			}
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x060007AF RID: 1967 RVA: 0x0001CE07 File Offset: 0x0001B007
		public StorageUri StorageUri
		{
			get
			{
				return this.attributes.StorageUri;
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x060007B0 RID: 1968 RVA: 0x0001CE14 File Offset: 0x0001B014
		public CopyState CopyState
		{
			get
			{
				return this.attributes.CopyState;
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x060007B1 RID: 1969 RVA: 0x0001CE21 File Offset: 0x0001B021
		// (set) Token: 0x060007B2 RID: 1970 RVA: 0x0001CE29 File Offset: 0x0001B029
		public string Name { get; private set; }

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x060007B3 RID: 1971 RVA: 0x0001CE32 File Offset: 0x0001B032
		public CloudFileShare Share
		{
			get
			{
				if (this.share == null)
				{
					this.share = this.ServiceClient.GetShareReference(NavigationHelper.GetShareName(this.Uri, new bool?(this.ServiceClient.UsePathStyleUris)));
				}
				return this.share;
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x060007B4 RID: 1972 RVA: 0x0001CE70 File Offset: 0x0001B070
		public CloudFileDirectory Parent
		{
			get
			{
				string directoryName;
				StorageUri uri;
				if (this.parent == null && NavigationHelper.GetFileParentNameAndAddress(this.StorageUri, new bool?(this.ServiceClient.UsePathStyleUris), out directoryName, out uri))
				{
					this.parent = new CloudFileDirectory(uri, directoryName, this.Share);
				}
				return this.parent;
			}
		}

		// Token: 0x060007B5 RID: 1973 RVA: 0x0001CEBF File Offset: 0x0001B0BF
		public string GetSharedAccessSignature(SharedAccessFilePolicy policy)
		{
			return this.GetSharedAccessSignature(policy, null, null);
		}

		// Token: 0x060007B6 RID: 1974 RVA: 0x0001CECA File Offset: 0x0001B0CA
		public string GetSharedAccessSignature(SharedAccessFilePolicy policy, string groupPolicyIdentifier)
		{
			return this.GetSharedAccessSignature(policy, null, groupPolicyIdentifier);
		}

		// Token: 0x060007B7 RID: 1975 RVA: 0x0001CED5 File Offset: 0x0001B0D5
		public string GetSharedAccessSignature(SharedAccessFilePolicy policy, SharedAccessFileHeaders headers)
		{
			return this.GetSharedAccessSignature(policy, headers, null);
		}

		// Token: 0x060007B8 RID: 1976 RVA: 0x0001CEE0 File Offset: 0x0001B0E0
		public string GetSharedAccessSignature(SharedAccessFilePolicy policy, SharedAccessFileHeaders headers, string groupPolicyIdentifier)
		{
			if (!this.ServiceClient.Credentials.IsSharedKey)
			{
				string message = string.Format(CultureInfo.InvariantCulture, "Cannot create Shared Access Signature unless Account Key credentials are used.", new object[0]);
				throw new InvalidOperationException(message);
			}
			string canonicalName = this.GetCanonicalName();
			StorageAccountKey key = this.ServiceClient.Credentials.Key;
			string hash = SharedAccessSignatureHelper.GetHash(policy, headers, groupPolicyIdentifier, canonicalName, "2015-02-21", key.KeyValue);
			UriQueryBuilder signature = SharedAccessSignatureHelper.GetSignature(policy, headers, groupPolicyIdentifier, "f", hash, key.KeyName, "2015-02-21");
			return signature.ToString();
		}

		// Token: 0x060007B9 RID: 1977 RVA: 0x0001CF70 File Offset: 0x0001B170
		private string GetCanonicalName()
		{
			string accountName = this.ServiceClient.Credentials.AccountName;
			string name = this.Share.Name;
			string text = NavigationHelper.GetFileAndDirectoryName(this.Uri, new bool?(this.ServiceClient.UsePathStyleUris)).Replace('\\', '/');
			return string.Format(CultureInfo.InvariantCulture, "/{0}/{1}/{2}/{3}", new object[]
			{
				"file",
				accountName,
				name,
				text
			});
		}

		// Token: 0x060007BA RID: 1978 RVA: 0x0001CFEC File Offset: 0x0001B1EC
		private void ParseQueryAndVerify(StorageUri address, StorageCredentials credentials)
		{
			StorageCredentials storageCredentials;
			this.attributes.StorageUri = NavigationHelper.ParseFileQueryAndVerify(address, out storageCredentials);
			if (storageCredentials != null && credentials != null)
			{
				string message = string.Format(CultureInfo.CurrentCulture, "Cannot provide credentials as part of the address and as constructor parameter. Either pass in the address or use a different constructor.", new object[0]);
				throw new ArgumentException(message);
			}
			if (this.ServiceClient == null)
			{
				this.ServiceClient = new CloudFileClient(NavigationHelper.GetServiceClientBaseAddress(this.StorageUri, null), credentials ?? storageCredentials);
			}
			this.Name = NavigationHelper.GetFileName(this.Uri, new bool?(this.ServiceClient.UsePathStyleUris));
		}

		// Token: 0x040000CC RID: 204
		private int streamWriteSizeInBytes;

		// Token: 0x040000CD RID: 205
		private int streamMinimumReadSizeInBytes;

		// Token: 0x040000CE RID: 206
		private CloudFileShare share;

		// Token: 0x040000CF RID: 207
		private CloudFileDirectory parent;

		// Token: 0x040000D0 RID: 208
		private readonly CloudFileAttributes attributes;
	}
}
