using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using Microsoft.WindowsAzure.Storage.Core;
using Microsoft.WindowsAzure.Storage.Core.Executor;
using Microsoft.WindowsAzure.Storage.Core.Util;
using Microsoft.WindowsAzure.Storage.Table.Protocol;

namespace Microsoft.WindowsAzure.Storage.Table
{
	// Token: 0x0200003F RID: 63
	public sealed class TableOperation
	{
		// Token: 0x06000BE8 RID: 3048 RVA: 0x00029DDC File Offset: 0x00027FDC
		internal TableResult Execute(CloudTableClient client, CloudTable table, TableRequestOptions requestOptions, OperationContext operationContext)
		{
			TableRequestOptions tableRequestOptions = TableRequestOptions.ApplyDefaults(requestOptions, client);
			operationContext = (operationContext ?? new OperationContext());
			CommonUtility.AssertNotNullOrEmpty("tableName", table.Name);
			return Executor.ExecuteSync<TableResult>(this.GenerateCMDForOperation(client, table, tableRequestOptions), tableRequestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x06000BE9 RID: 3049 RVA: 0x00029E24 File Offset: 0x00028024
		[DoesServiceRequest]
		internal ICancellableAsyncResult BeginExecute(CloudTableClient client, CloudTable table, TableRequestOptions requestOptions, OperationContext operationContext, AsyncCallback callback, object state)
		{
			TableRequestOptions tableRequestOptions = TableRequestOptions.ApplyDefaults(requestOptions, client);
			operationContext = (operationContext ?? new OperationContext());
			CommonUtility.AssertNotNullOrEmpty("tableName", table.Name);
			return Executor.BeginExecuteAsync<TableResult>(this.GenerateCMDForOperation(client, table, tableRequestOptions), tableRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x06000BEA RID: 3050 RVA: 0x00029E70 File Offset: 0x00028070
		internal static TableResult EndExecute(IAsyncResult asyncResult)
		{
			return Executor.EndExecuteAsync<TableResult>(asyncResult);
		}

		// Token: 0x06000BEB RID: 3051 RVA: 0x00029E78 File Offset: 0x00028078
		internal RESTCommand<TableResult> GenerateCMDForOperation(CloudTableClient client, CloudTable table, TableRequestOptions modifiedOptions)
		{
			if (this.OperationType == TableOperationType.Insert || this.OperationType == TableOperationType.InsertOrMerge || this.OperationType == TableOperationType.InsertOrReplace)
			{
				if (!this.isTableEntity && this.OperationType != TableOperationType.Insert)
				{
					CommonUtility.AssertNotNull("Upserts require a valid PartitionKey", this.Entity.PartitionKey);
					CommonUtility.AssertNotNull("Upserts require a valid RowKey", this.Entity.RowKey);
				}
				return TableOperation.InsertImpl(this, client, table, modifiedOptions);
			}
			if (this.OperationType == TableOperationType.Delete)
			{
				if (!this.isTableEntity)
				{
					CommonUtility.AssertNotNullOrEmpty("Delete requires a valid ETag", this.Entity.ETag);
					CommonUtility.AssertNotNull("Delete requires a valid PartitionKey", this.Entity.PartitionKey);
					CommonUtility.AssertNotNull("Delete requires a valid RowKey", this.Entity.RowKey);
				}
				return TableOperation.DeleteImpl(this, client, table, modifiedOptions);
			}
			if (this.OperationType == TableOperationType.Merge)
			{
				CommonUtility.AssertNotNullOrEmpty("Merge requires a valid ETag", this.Entity.ETag);
				CommonUtility.AssertNotNull("Merge requires a valid PartitionKey", this.Entity.PartitionKey);
				CommonUtility.AssertNotNull("Merge requires a valid RowKey", this.Entity.RowKey);
				return TableOperation.MergeImpl(this, client, table, modifiedOptions);
			}
			if (this.OperationType == TableOperationType.Replace)
			{
				CommonUtility.AssertNotNullOrEmpty("Replace requires a valid ETag", this.Entity.ETag);
				CommonUtility.AssertNotNull("Replace requires a valid PartitionKey", this.Entity.PartitionKey);
				CommonUtility.AssertNotNull("Replace requires a valid RowKey", this.Entity.RowKey);
				return TableOperation.ReplaceImpl(this, client, table, modifiedOptions);
			}
			if (this.OperationType == TableOperationType.Retrieve)
			{
				return TableOperation.RetrieveImpl(this, client, table, modifiedOptions);
			}
			throw new NotSupportedException();
		}

		// Token: 0x06000BEC RID: 3052 RVA: 0x0002A094 File Offset: 0x00028294
		private static RESTCommand<TableResult> InsertImpl(TableOperation operation, CloudTableClient client, CloudTable table, TableRequestOptions requestOptions)
		{
			RESTCommand<TableResult> insertCmd = new RESTCommand<TableResult>(client.Credentials, operation.GenerateRequestURI(client.StorageUri, table.Name));
			requestOptions.ApplyToStorageCommand<TableResult>(insertCmd);
			TableResult result = new TableResult
			{
				Result = operation.Entity
			};
			insertCmd.RetrieveResponseStream = true;
			insertCmd.SignRequest = new Action<HttpWebRequest, OperationContext>(client.AuthenticationHandler.SignRequest);
			insertCmd.ParseError = new Func<Stream, HttpWebResponse, string, StorageExtendedErrorInformation>(StorageExtendedErrorInformation.ReadFromStreamUsingODataLib);
			insertCmd.BuildRequestDelegate = delegate(Uri uri, UriQueryBuilder builder, int? timeout, bool useVersionHeader, OperationContext ctx)
			{
				Tuple<HttpWebRequest, Stream> tuple = TableOperationHttpWebRequestFactory.BuildRequestForTableOperation(uri, builder, client.BufferManager, timeout, operation, useVersionHeader, ctx, requestOptions, client.AccountName);
				insertCmd.SendStream = tuple.Item2;
				return tuple.Item1;
			};
			insertCmd.PreProcessResponse = ((RESTCommand<TableResult> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx) => TableOperationHttpResponseParsers.TableOperationPreProcess(result, operation, resp, ex));
			insertCmd.PostProcessResponse = ((RESTCommand<TableResult> cmd, HttpWebResponse resp, OperationContext ctx) => TableOperationHttpResponseParsers.TableOperationPostProcess(result, operation, cmd, resp, ctx, requestOptions, client.AccountName));
			return insertCmd;
		}

		// Token: 0x06000BED RID: 3053 RVA: 0x0002A210 File Offset: 0x00028410
		private static RESTCommand<TableResult> DeleteImpl(TableOperation operation, CloudTableClient client, CloudTable table, TableRequestOptions requestOptions)
		{
			RESTCommand<TableResult> restcommand = new RESTCommand<TableResult>(client.Credentials, operation.GenerateRequestURI(client.StorageUri, table.Name));
			requestOptions.ApplyToStorageCommand<TableResult>(restcommand);
			TableResult result = new TableResult
			{
				Result = operation.Entity
			};
			restcommand.RetrieveResponseStream = false;
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(client.AuthenticationHandler.SignRequest);
			restcommand.ParseError = new Func<Stream, HttpWebResponse, string, StorageExtendedErrorInformation>(StorageExtendedErrorInformation.ReadFromStreamUsingODataLib);
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? timeout, bool useVersionHeader, OperationContext ctx) => TableOperationHttpWebRequestFactory.BuildRequestForTableOperation(uri, builder, client.BufferManager, timeout, operation, useVersionHeader, ctx, requestOptions, client.AccountName).Item1);
			restcommand.PreProcessResponse = ((RESTCommand<TableResult> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx) => TableOperationHttpResponseParsers.TableOperationPreProcess(result, operation, resp, ex));
			return restcommand;
		}

		// Token: 0x06000BEE RID: 3054 RVA: 0x0002A35C File Offset: 0x0002855C
		private static RESTCommand<TableResult> MergeImpl(TableOperation operation, CloudTableClient client, CloudTable table, TableRequestOptions requestOptions)
		{
			RESTCommand<TableResult> mergeCmd = new RESTCommand<TableResult>(client.Credentials, operation.GenerateRequestURI(client.StorageUri, table.Name));
			requestOptions.ApplyToStorageCommand<TableResult>(mergeCmd);
			TableResult result = new TableResult
			{
				Result = operation.Entity
			};
			mergeCmd.RetrieveResponseStream = false;
			mergeCmd.SignRequest = new Action<HttpWebRequest, OperationContext>(client.AuthenticationHandler.SignRequest);
			mergeCmd.ParseError = new Func<Stream, HttpWebResponse, string, StorageExtendedErrorInformation>(StorageExtendedErrorInformation.ReadFromStreamUsingODataLib);
			mergeCmd.BuildRequestDelegate = delegate(Uri uri, UriQueryBuilder builder, int? timeout, bool useVersionHeader, OperationContext ctx)
			{
				Tuple<HttpWebRequest, Stream> tuple = TableOperationHttpWebRequestFactory.BuildRequestForTableOperation(uri, builder, client.BufferManager, timeout, operation, useVersionHeader, ctx, requestOptions, client.AccountName);
				mergeCmd.SendStream = tuple.Item2;
				return tuple.Item1;
			};
			mergeCmd.PreProcessResponse = ((RESTCommand<TableResult> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx) => TableOperationHttpResponseParsers.TableOperationPreProcess(result, operation, resp, ex));
			return mergeCmd;
		}

		// Token: 0x06000BEF RID: 3055 RVA: 0x0002A4D0 File Offset: 0x000286D0
		private static RESTCommand<TableResult> ReplaceImpl(TableOperation operation, CloudTableClient client, CloudTable table, TableRequestOptions requestOptions)
		{
			RESTCommand<TableResult> replaceCmd = new RESTCommand<TableResult>(client.Credentials, operation.GenerateRequestURI(client.StorageUri, table.Name));
			requestOptions.ApplyToStorageCommand<TableResult>(replaceCmd);
			TableResult result = new TableResult
			{
				Result = operation.Entity
			};
			replaceCmd.RetrieveResponseStream = false;
			replaceCmd.SignRequest = new Action<HttpWebRequest, OperationContext>(client.AuthenticationHandler.SignRequest);
			replaceCmd.ParseError = new Func<Stream, HttpWebResponse, string, StorageExtendedErrorInformation>(StorageExtendedErrorInformation.ReadFromStreamUsingODataLib);
			replaceCmd.BuildRequestDelegate = delegate(Uri uri, UriQueryBuilder builder, int? timeout, bool useVersionHeader, OperationContext ctx)
			{
				Tuple<HttpWebRequest, Stream> tuple = TableOperationHttpWebRequestFactory.BuildRequestForTableOperation(uri, builder, client.BufferManager, timeout, operation, useVersionHeader, ctx, requestOptions, client.AccountName);
				replaceCmd.SendStream = tuple.Item2;
				return tuple.Item1;
			};
			replaceCmd.PreProcessResponse = ((RESTCommand<TableResult> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx) => TableOperationHttpResponseParsers.TableOperationPreProcess(result, operation, resp, ex));
			return replaceCmd;
		}

		// Token: 0x06000BF0 RID: 3056 RVA: 0x0002A688 File Offset: 0x00028888
		private static RESTCommand<TableResult> RetrieveImpl(TableOperation operation, CloudTableClient client, CloudTable table, TableRequestOptions requestOptions)
		{
			requestOptions.AssertPolicyIfRequired();
			RESTCommand<TableResult> restcommand = new RESTCommand<TableResult>(client.Credentials, operation.GenerateRequestURI(client.StorageUri, table.Name));
			requestOptions.ApplyToStorageCommand<TableResult>(restcommand);
			TableResult result = new TableResult();
			restcommand.CommandLocationMode = (operation.isPrimaryOnlyRetrieve ? CommandLocationMode.PrimaryOnly : CommandLocationMode.PrimaryOrSecondary);
			restcommand.RetrieveResponseStream = true;
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(client.AuthenticationHandler.SignRequest);
			restcommand.ParseError = new Func<Stream, HttpWebResponse, string, StorageExtendedErrorInformation>(StorageExtendedErrorInformation.ReadFromStreamUsingODataLib);
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? timeout, bool useVersionHeader, OperationContext ctx) => TableOperationHttpWebRequestFactory.BuildRequestForTableOperation(uri, builder, client.BufferManager, timeout, operation, useVersionHeader, ctx, requestOptions, client.AccountName).Item1);
			restcommand.PreProcessResponse = ((RESTCommand<TableResult> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx) => TableOperationHttpResponseParsers.TableOperationPreProcess(result, operation, resp, ex));
			restcommand.PostProcessResponse = delegate(RESTCommand<TableResult> cmd, HttpWebResponse resp, OperationContext ctx)
			{
				if (resp.StatusCode == HttpStatusCode.NotFound)
				{
					return result;
				}
				result = TableOperationHttpResponseParsers.TableOperationPostProcess(result, operation, cmd, resp, ctx, requestOptions, client.AccountName);
				return result;
			};
			return restcommand;
		}

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x06000BF1 RID: 3057 RVA: 0x0002A784 File Offset: 0x00028984
		internal string HttpMethod
		{
			get
			{
				switch (this.OperationType)
				{
				case TableOperationType.Insert:
					return "POST";
				case TableOperationType.Delete:
					return "DELETE";
				case TableOperationType.Replace:
				case TableOperationType.InsertOrReplace:
					return "PUT";
				case TableOperationType.Merge:
				case TableOperationType.InsertOrMerge:
					return "POST";
				case TableOperationType.Retrieve:
					return "GET";
				default:
					throw new NotSupportedException();
				}
			}
		}

		// Token: 0x06000BF2 RID: 3058 RVA: 0x0002A7DF File Offset: 0x000289DF
		internal TableOperation(ITableEntity entity, TableOperationType operationType) : this(entity, operationType, true)
		{
		}

		// Token: 0x06000BF3 RID: 3059 RVA: 0x0002A7EA File Offset: 0x000289EA
		internal TableOperation(ITableEntity entity, TableOperationType operationType, bool echoContent)
		{
			if (entity == null && operationType != TableOperationType.Retrieve)
			{
				throw new ArgumentNullException("entity");
			}
			this.Entity = entity;
			this.OperationType = operationType;
			this.EchoContent = echoContent;
		}

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x06000BF4 RID: 3060 RVA: 0x0002A819 File Offset: 0x00028A19
		// (set) Token: 0x06000BF5 RID: 3061 RVA: 0x0002A821 File Offset: 0x00028A21
		internal bool IsTableEntity
		{
			get
			{
				return this.isTableEntity;
			}
			set
			{
				this.isTableEntity = value;
			}
		}

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x06000BF6 RID: 3062 RVA: 0x0002A82A File Offset: 0x00028A2A
		// (set) Token: 0x06000BF7 RID: 3063 RVA: 0x0002A832 File Offset: 0x00028A32
		internal bool IsPrimaryOnlyRetrieve
		{
			get
			{
				return this.isPrimaryOnlyRetrieve;
			}
			set
			{
				this.isPrimaryOnlyRetrieve = value;
			}
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x06000BF8 RID: 3064 RVA: 0x0002A83B File Offset: 0x00028A3B
		// (set) Token: 0x06000BF9 RID: 3065 RVA: 0x0002A843 File Offset: 0x00028A43
		internal string RetrievePartitionKey { get; set; }

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x06000BFA RID: 3066 RVA: 0x0002A84C File Offset: 0x00028A4C
		// (set) Token: 0x06000BFB RID: 3067 RVA: 0x0002A854 File Offset: 0x00028A54
		internal string RetrieveRowKey { get; set; }

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x06000BFC RID: 3068 RVA: 0x0002A85D File Offset: 0x00028A5D
		// (set) Token: 0x06000BFD RID: 3069 RVA: 0x0002A87F File Offset: 0x00028A7F
		internal Func<string, string, DateTimeOffset, IDictionary<string, EntityProperty>, string, object> RetrieveResolver
		{
			get
			{
				if (this.retrieveResolver == null)
				{
					this.retrieveResolver = new Func<string, string, DateTimeOffset, IDictionary<string, EntityProperty>, string, object>(TableOperation.DynamicEntityResolver);
				}
				return this.retrieveResolver;
			}
			set
			{
				this.retrieveResolver = value;
			}
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x06000BFE RID: 3070 RVA: 0x0002A888 File Offset: 0x00028A88
		// (set) Token: 0x06000BFF RID: 3071 RVA: 0x0002A890 File Offset: 0x00028A90
		internal Type PropertyResolverType { get; set; }

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x06000C00 RID: 3072 RVA: 0x0002A899 File Offset: 0x00028A99
		// (set) Token: 0x06000C01 RID: 3073 RVA: 0x0002A8A1 File Offset: 0x00028AA1
		internal ITableEntity Entity { get; private set; }

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x06000C02 RID: 3074 RVA: 0x0002A8AA File Offset: 0x00028AAA
		// (set) Token: 0x06000C03 RID: 3075 RVA: 0x0002A8B2 File Offset: 0x00028AB2
		internal TableOperationType OperationType { get; private set; }

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x06000C04 RID: 3076 RVA: 0x0002A8BB File Offset: 0x00028ABB
		// (set) Token: 0x06000C05 RID: 3077 RVA: 0x0002A8C3 File Offset: 0x00028AC3
		internal bool EchoContent { get; set; }

		// Token: 0x06000C06 RID: 3078 RVA: 0x0002A8CC File Offset: 0x00028ACC
		public static TableOperation Delete(ITableEntity entity)
		{
			CommonUtility.AssertNotNull("entity", entity);
			if (string.IsNullOrEmpty(entity.ETag))
			{
				throw new ArgumentException("Delete requires an ETag (which may be the '*' wildcard).");
			}
			return new TableOperation(entity, TableOperationType.Delete);
		}

		// Token: 0x06000C07 RID: 3079 RVA: 0x0002A8F8 File Offset: 0x00028AF8
		public static TableOperation Insert(ITableEntity entity)
		{
			return TableOperation.Insert(entity, false);
		}

		// Token: 0x06000C08 RID: 3080 RVA: 0x0002A901 File Offset: 0x00028B01
		public static TableOperation Insert(ITableEntity entity, bool echoContent)
		{
			CommonUtility.AssertNotNull("entity", entity);
			return new TableOperation(entity, TableOperationType.Insert, echoContent);
		}

		// Token: 0x06000C09 RID: 3081 RVA: 0x0002A916 File Offset: 0x00028B16
		public static TableOperation InsertOrMerge(ITableEntity entity)
		{
			CommonUtility.AssertNotNull("entity", entity);
			return new TableOperation(entity, TableOperationType.InsertOrMerge);
		}

		// Token: 0x06000C0A RID: 3082 RVA: 0x0002A92A File Offset: 0x00028B2A
		public static TableOperation InsertOrReplace(ITableEntity entity)
		{
			CommonUtility.AssertNotNull("entity", entity);
			return new TableOperation(entity, TableOperationType.InsertOrReplace);
		}

		// Token: 0x06000C0B RID: 3083 RVA: 0x0002A93E File Offset: 0x00028B3E
		public static TableOperation Merge(ITableEntity entity)
		{
			CommonUtility.AssertNotNull("entity", entity);
			if (string.IsNullOrEmpty(entity.ETag))
			{
				throw new ArgumentException("Merge requires an ETag (which may be the '*' wildcard).");
			}
			return new TableOperation(entity, TableOperationType.Merge);
		}

		// Token: 0x06000C0C RID: 3084 RVA: 0x0002A96A File Offset: 0x00028B6A
		public static TableOperation Replace(ITableEntity entity)
		{
			CommonUtility.AssertNotNull("entity", entity);
			if (string.IsNullOrEmpty(entity.ETag))
			{
				throw new ArgumentException("Replace requires an ETag (which may be the '*' wildcard).");
			}
			return new TableOperation(entity, TableOperationType.Replace);
		}

		// Token: 0x06000C0D RID: 3085 RVA: 0x0002A9A8 File Offset: 0x00028BA8
		public static TableOperation Retrieve<TElement>(string partitionKey, string rowkey) where TElement : ITableEntity
		{
			CommonUtility.AssertNotNull("partitionKey", partitionKey);
			CommonUtility.AssertNotNull("rowkey", rowkey);
			return new TableOperation(null, TableOperationType.Retrieve)
			{
				RetrievePartitionKey = partitionKey,
				RetrieveRowKey = rowkey,
				RetrieveResolver = ((string pk, string rk, DateTimeOffset ts, IDictionary<string, EntityProperty> prop, string etag) => EntityUtilities.ResolveEntityByType<TElement>(pk, rk, ts, prop, etag)),
				PropertyResolverType = typeof(TElement)
			};
		}

		// Token: 0x06000C0E RID: 3086 RVA: 0x0002AA28 File Offset: 0x00028C28
		public static TableOperation Retrieve<TResult>(string partitionKey, string rowkey, EntityResolver<TResult> resolver)
		{
			CommonUtility.AssertNotNull("partitionKey", partitionKey);
			CommonUtility.AssertNotNull("rowkey", rowkey);
			return new TableOperation(null, TableOperationType.Retrieve)
			{
				RetrievePartitionKey = partitionKey,
				RetrieveRowKey = rowkey,
				RetrieveResolver = ((string pk, string rk, DateTimeOffset ts, IDictionary<string, EntityProperty> prop, string etag) => resolver(pk, rk, ts, prop, etag))
			};
		}

		// Token: 0x06000C0F RID: 3087 RVA: 0x0002AA84 File Offset: 0x00028C84
		public static TableOperation Retrieve(string partitionKey, string rowkey)
		{
			CommonUtility.AssertNotNull("partitionKey", partitionKey);
			CommonUtility.AssertNotNull("rowkey", rowkey);
			return new TableOperation(null, TableOperationType.Retrieve)
			{
				RetrievePartitionKey = partitionKey,
				RetrieveRowKey = rowkey
			};
		}

		// Token: 0x06000C10 RID: 3088 RVA: 0x0002AAC0 File Offset: 0x00028CC0
		private static object DynamicEntityResolver(string partitionKey, string rowKey, DateTimeOffset timestamp, IDictionary<string, EntityProperty> properties, string etag)
		{
			ITableEntity tableEntity = new DynamicTableEntity();
			tableEntity.PartitionKey = partitionKey;
			tableEntity.RowKey = rowKey;
			tableEntity.Timestamp = timestamp;
			tableEntity.ReadEntity(properties, null);
			tableEntity.ETag = etag;
			return tableEntity;
		}

		// Token: 0x06000C11 RID: 3089 RVA: 0x0002AAF9 File Offset: 0x00028CF9
		internal StorageUri GenerateRequestURI(StorageUri uriList, string tableName)
		{
			return new StorageUri(this.GenerateRequestURI(uriList.PrimaryUri, tableName), this.GenerateRequestURI(uriList.SecondaryUri, tableName));
		}

		// Token: 0x06000C12 RID: 3090 RVA: 0x0002AB1C File Offset: 0x00028D1C
		internal Uri GenerateRequestURI(Uri uri, string tableName)
		{
			if (uri == null)
			{
				return null;
			}
			if (this.OperationType == TableOperationType.Insert)
			{
				return NavigationHelper.AppendPathToSingleUri(uri, tableName + "()");
			}
			string text;
			if (this.isTableEntity)
			{
				text = string.Format(CultureInfo.InvariantCulture, "'{0}'", new object[]
				{
					this.Entity.WriteEntity(null)["TableName"].StringValue
				});
			}
			else if (this.OperationType == TableOperationType.Retrieve)
			{
				text = string.Format(CultureInfo.InvariantCulture, "{0}='{1}',{2}='{3}'", new object[]
				{
					"PartitionKey",
					this.RetrievePartitionKey.Replace("'", "''"),
					"RowKey",
					this.RetrieveRowKey.Replace("'", "''")
				});
			}
			else
			{
				text = string.Format(CultureInfo.InvariantCulture, "{0}='{1}',{2}='{3}'", new object[]
				{
					"PartitionKey",
					this.Entity.PartitionKey.Replace("'", "''"),
					"RowKey",
					this.Entity.RowKey.Replace("'", "''")
				});
			}
			return NavigationHelper.AppendPathToSingleUri(uri, string.Format(CultureInfo.InvariantCulture, "{0}({1})", new object[]
			{
				tableName,
				text
			}));
		}

		// Token: 0x04000165 RID: 357
		private bool isTableEntity;

		// Token: 0x04000166 RID: 358
		private bool isPrimaryOnlyRetrieve;

		// Token: 0x04000167 RID: 359
		private Func<string, string, DateTimeOffset, IDictionary<string, EntityProperty>, string, object> retrieveResolver;
	}
}
