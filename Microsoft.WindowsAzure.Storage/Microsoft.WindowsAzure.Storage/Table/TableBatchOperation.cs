using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Microsoft.WindowsAzure.Storage.Core;
using Microsoft.WindowsAzure.Storage.Core.Executor;
using Microsoft.WindowsAzure.Storage.Core.Util;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;
using Microsoft.WindowsAzure.Storage.Table.Protocol;

namespace Microsoft.WindowsAzure.Storage.Table
{
	// Token: 0x0200003D RID: 61
	public sealed class TableBatchOperation : IList<TableOperation>, ICollection<TableOperation>, IEnumerable<TableOperation>, IEnumerable
	{
		// Token: 0x06000BBE RID: 3006 RVA: 0x00028D9C File Offset: 0x00026F9C
		public void Retrieve<TElement>(string partitionKey, string rowKey) where TElement : ITableEntity
		{
			CommonUtility.AssertNotNull("partitionKey", partitionKey);
			CommonUtility.AssertNotNull("rowkey", rowKey);
			this.Add(new TableOperation(null, TableOperationType.Retrieve)
			{
				RetrievePartitionKey = partitionKey,
				RetrieveRowKey = rowKey,
				RetrieveResolver = ((string pk, string rk, DateTimeOffset ts, IDictionary<string, EntityProperty> prop, string etag) => EntityUtilities.ResolveEntityByType<TElement>(pk, rk, ts, prop, etag))
			});
		}

		// Token: 0x06000BBF RID: 3007 RVA: 0x00028E10 File Offset: 0x00027010
		public void Retrieve<TResult>(string partitionKey, string rowKey, EntityResolver<TResult> resolver)
		{
			CommonUtility.AssertNotNull("partitionKey", partitionKey);
			CommonUtility.AssertNotNull("rowkey", rowKey);
			this.Add(new TableOperation(null, TableOperationType.Retrieve)
			{
				RetrievePartitionKey = partitionKey,
				RetrieveRowKey = rowKey,
				RetrieveResolver = ((string pk, string rk, DateTimeOffset ts, IDictionary<string, EntityProperty> prop, string etag) => resolver(pk, rk, ts, prop, etag))
			});
		}

		// Token: 0x06000BC0 RID: 3008 RVA: 0x00028E70 File Offset: 0x00027070
		[DoesServiceRequest]
		internal IList<TableResult> Execute(CloudTableClient client, CloudTable table, TableRequestOptions requestOptions, OperationContext operationContext)
		{
			TableRequestOptions tableRequestOptions = TableRequestOptions.ApplyDefaults(requestOptions, client);
			operationContext = (operationContext ?? new OperationContext());
			CommonUtility.AssertNotNullOrEmpty("tableName", table.Name);
			if (this.operations.Count == 0)
			{
				throw new InvalidOperationException("Cannot execute an empty batch operation");
			}
			return Executor.ExecuteSync<IList<TableResult>>(TableBatchOperation.BatchImpl(this, client, table, tableRequestOptions), tableRequestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x06000BC1 RID: 3009 RVA: 0x00028ED0 File Offset: 0x000270D0
		[DoesServiceRequest]
		internal ICancellableAsyncResult BeginExecute(CloudTableClient client, CloudTable table, TableRequestOptions requestOptions, OperationContext operationContext, AsyncCallback callback, object state)
		{
			TableRequestOptions tableRequestOptions = TableRequestOptions.ApplyDefaults(requestOptions, client);
			operationContext = (operationContext ?? new OperationContext());
			CommonUtility.AssertNotNullOrEmpty("tableName", table.Name);
			if (this.operations.Count == 0)
			{
				throw new InvalidOperationException("Cannot execute an empty batch operation");
			}
			return Executor.BeginExecuteAsync<IList<TableResult>>(TableBatchOperation.BatchImpl(this, client, table, tableRequestOptions), tableRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x06000BC2 RID: 3010 RVA: 0x00028F34 File Offset: 0x00027134
		internal static IList<TableResult> EndExecute(IAsyncResult asyncResult)
		{
			return Executor.EndExecuteAsync<IList<TableResult>>(asyncResult);
		}

		// Token: 0x06000BC3 RID: 3011 RVA: 0x00028FFC File Offset: 0x000271FC
		private static RESTCommand<IList<TableResult>> BatchImpl(TableBatchOperation batch, CloudTableClient client, CloudTable table, TableRequestOptions requestOptions)
		{
			RESTCommand<IList<TableResult>> batchCmd = new RESTCommand<IList<TableResult>>(client.Credentials, client.StorageUri);
			requestOptions.ApplyToStorageCommand<IList<TableResult>>(batchCmd);
			List<TableResult> results = new List<TableResult>();
			batchCmd.CommandLocationMode = (batch.ContainsWrites ? CommandLocationMode.PrimaryOnly : CommandLocationMode.PrimaryOrSecondary);
			batchCmd.RetrieveResponseStream = true;
			batchCmd.SignRequest = new Action<HttpWebRequest, OperationContext>(client.AuthenticationHandler.SignRequest);
			batchCmd.ParseError = new Func<Stream, HttpWebResponse, string, StorageExtendedErrorInformation>(StorageExtendedErrorInformation.ReadFromStreamUsingODataLib);
			batchCmd.BuildRequestDelegate = delegate(Uri uri, UriQueryBuilder builder, int? timeout, bool useVersionHeader, OperationContext ctx)
			{
				Tuple<HttpWebRequest, Stream> tuple = TableOperationHttpWebRequestFactory.BuildRequestForTableBatchOperation(uri, builder, client.BufferManager, timeout, table.Name, batch, useVersionHeader, ctx, requestOptions, client.AccountName);
				batchCmd.SendStream = tuple.Item2;
				return tuple.Item1;
			};
			batchCmd.PreProcessResponse = ((RESTCommand<IList<TableResult>> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx) => HttpResponseParsers.ProcessExpectedStatusCodeNoException<IList<TableResult>>(HttpStatusCode.Accepted, (resp != null) ? resp.StatusCode : HttpStatusCode.Unused, results, cmd, ex));
			batchCmd.PostProcessResponse = ((RESTCommand<IList<TableResult>> cmd, HttpWebResponse resp, OperationContext ctx) => TableOperationHttpResponseParsers.TableBatchOperationPostProcess(results, batch, cmd, resp, ctx, requestOptions, client.AccountName));
			batchCmd.RecoveryAction = delegate(StorageCommandBase<IList<TableResult>> cmd, Exception ex, OperationContext ctx)
			{
				results.Clear();
			};
			return batchCmd;
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x06000BC4 RID: 3012 RVA: 0x0002912B File Offset: 0x0002732B
		// (set) Token: 0x06000BC5 RID: 3013 RVA: 0x00029133 File Offset: 0x00027333
		internal bool ContainsWrites { get; private set; }

		// Token: 0x06000BC7 RID: 3015 RVA: 0x0002914F File Offset: 0x0002734F
		public void Delete(ITableEntity entity)
		{
			CommonUtility.AssertNotNull("entity", entity);
			if (string.IsNullOrEmpty(entity.ETag))
			{
				throw new ArgumentException("Delete requires an ETag (which may be the '*' wildcard).");
			}
			this.Add(new TableOperation(entity, TableOperationType.Delete));
		}

		// Token: 0x06000BC8 RID: 3016 RVA: 0x00029181 File Offset: 0x00027381
		public void Insert(ITableEntity entity)
		{
			this.Insert(entity, true);
		}

		// Token: 0x06000BC9 RID: 3017 RVA: 0x0002918B File Offset: 0x0002738B
		public void Insert(ITableEntity entity, bool echoContent)
		{
			CommonUtility.AssertNotNull("entity", entity);
			this.Add(new TableOperation(entity, TableOperationType.Insert, echoContent));
		}

		// Token: 0x06000BCA RID: 3018 RVA: 0x000291A6 File Offset: 0x000273A6
		public void InsertOrMerge(ITableEntity entity)
		{
			CommonUtility.AssertNotNull("entity", entity);
			this.Add(new TableOperation(entity, TableOperationType.InsertOrMerge));
		}

		// Token: 0x06000BCB RID: 3019 RVA: 0x000291C0 File Offset: 0x000273C0
		public void InsertOrReplace(ITableEntity entity)
		{
			CommonUtility.AssertNotNull("entity", entity);
			this.Add(new TableOperation(entity, TableOperationType.InsertOrReplace));
		}

		// Token: 0x06000BCC RID: 3020 RVA: 0x000291DA File Offset: 0x000273DA
		public void Merge(ITableEntity entity)
		{
			CommonUtility.AssertNotNull("entity", entity);
			if (string.IsNullOrEmpty(entity.ETag))
			{
				throw new ArgumentException("Merge requires an ETag (which may be the '*' wildcard).");
			}
			this.Add(new TableOperation(entity, TableOperationType.Merge));
		}

		// Token: 0x06000BCD RID: 3021 RVA: 0x0002920C File Offset: 0x0002740C
		public void Replace(ITableEntity entity)
		{
			CommonUtility.AssertNotNull("entity", entity);
			if (string.IsNullOrEmpty(entity.ETag))
			{
				throw new ArgumentException("Replace requires an ETag (which may be the '*' wildcard).");
			}
			this.Add(new TableOperation(entity, TableOperationType.Replace));
		}

		// Token: 0x06000BCE RID: 3022 RVA: 0x00029240 File Offset: 0x00027440
		public void Retrieve(string partitionKey, string rowKey)
		{
			CommonUtility.AssertNotNull("partitionKey", partitionKey);
			CommonUtility.AssertNotNull("rowkey", rowKey);
			this.Add(new TableOperation(null, TableOperationType.Retrieve)
			{
				RetrievePartitionKey = partitionKey,
				RetrieveRowKey = rowKey
			});
		}

		// Token: 0x06000BCF RID: 3023 RVA: 0x00029280 File Offset: 0x00027480
		public int IndexOf(TableOperation item)
		{
			return this.operations.IndexOf(item);
		}

		// Token: 0x06000BD0 RID: 3024 RVA: 0x00029290 File Offset: 0x00027490
		public void Insert(int index, TableOperation item)
		{
			CommonUtility.AssertNotNull("item", item);
			this.CheckSingleQueryPerBatch(item);
			this.LockToPartitionKey((item.OperationType == TableOperationType.Retrieve) ? item.RetrievePartitionKey : item.Entity.PartitionKey);
			this.operations.Insert(index, item);
		}

		// Token: 0x06000BD1 RID: 3025 RVA: 0x000292DE File Offset: 0x000274DE
		public void RemoveAt(int index)
		{
			this.operations.RemoveAt(index);
			if (this.operations.Count == 0)
			{
				this.batchPartitionKey = null;
				this.hasQuery = false;
			}
		}

		// Token: 0x17000123 RID: 291
		public TableOperation this[int index]
		{
			get
			{
				return this.operations[index];
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x06000BD4 RID: 3028 RVA: 0x0002931C File Offset: 0x0002751C
		public void Add(TableOperation item)
		{
			CommonUtility.AssertNotNull("item", item);
			this.CheckSingleQueryPerBatch(item);
			this.LockToPartitionKey((item.OperationType == TableOperationType.Retrieve) ? item.RetrievePartitionKey : item.Entity.PartitionKey);
			this.operations.Add(item);
		}

		// Token: 0x06000BD5 RID: 3029 RVA: 0x00029369 File Offset: 0x00027569
		public void Clear()
		{
			this.operations.Clear();
			this.batchPartitionKey = null;
			this.hasQuery = false;
		}

		// Token: 0x06000BD6 RID: 3030 RVA: 0x00029384 File Offset: 0x00027584
		public bool Contains(TableOperation item)
		{
			return this.operations.Contains(item);
		}

		// Token: 0x06000BD7 RID: 3031 RVA: 0x00029392 File Offset: 0x00027592
		public void CopyTo(TableOperation[] array, int arrayIndex)
		{
			this.operations.CopyTo(array, arrayIndex);
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x06000BD8 RID: 3032 RVA: 0x000293A1 File Offset: 0x000275A1
		public int Count
		{
			get
			{
				return this.operations.Count;
			}
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x06000BD9 RID: 3033 RVA: 0x000293AE File Offset: 0x000275AE
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000BDA RID: 3034 RVA: 0x000293B4 File Offset: 0x000275B4
		public bool Remove(TableOperation item)
		{
			CommonUtility.AssertNotNull("item", item);
			bool result = this.operations.Remove(item);
			if (this.operations.Count == 0)
			{
				this.batchPartitionKey = null;
				this.hasQuery = false;
			}
			return result;
		}

		// Token: 0x06000BDB RID: 3035 RVA: 0x000293F5 File Offset: 0x000275F5
		public IEnumerator<TableOperation> GetEnumerator()
		{
			return this.operations.GetEnumerator();
		}

		// Token: 0x06000BDC RID: 3036 RVA: 0x00029407 File Offset: 0x00027607
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.operations.GetEnumerator();
		}

		// Token: 0x06000BDD RID: 3037 RVA: 0x0002941C File Offset: 0x0002761C
		private void CheckSingleQueryPerBatch(TableOperation item)
		{
			if (this.hasQuery)
			{
				throw new ArgumentException("A batch transaction with a retrieve operation cannot contain any other operations.");
			}
			if (item.OperationType == TableOperationType.Retrieve)
			{
				if (this.operations.Count > 0)
				{
					throw new ArgumentException("A batch transaction with a retrieve operation cannot contain any other operations.");
				}
				this.hasQuery = true;
			}
			this.ContainsWrites = (item.OperationType != TableOperationType.Retrieve);
		}

		// Token: 0x06000BDE RID: 3038 RVA: 0x00029477 File Offset: 0x00027677
		private void LockToPartitionKey(string partitionKey)
		{
			if (this.batchPartitionKey == null)
			{
				this.batchPartitionKey = partitionKey;
				return;
			}
			if (partitionKey != this.batchPartitionKey)
			{
				throw new ArgumentException("All entities in a given batch must have the same partition key.");
			}
		}

		// Token: 0x0400015F RID: 351
		private bool hasQuery;

		// Token: 0x04000160 RID: 352
		private string batchPartitionKey;

		// Token: 0x04000161 RID: 353
		private List<TableOperation> operations = new List<TableOperation>();
	}
}
