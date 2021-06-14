using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using Microsoft.WindowsAzure.Storage.Core;
using Microsoft.WindowsAzure.Storage.Core.Executor;
using Microsoft.WindowsAzure.Storage.Core.Util;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;
using Microsoft.WindowsAzure.Storage.Table.Protocol;

namespace Microsoft.WindowsAzure.Storage.Table
{
	// Token: 0x02000042 RID: 66
	public sealed class TableQuery
	{
		// Token: 0x06000C42 RID: 3138 RVA: 0x0002BB9F File Offset: 0x00029D9F
		public static T Project<T>(T entity, params string[] columns)
		{
			return entity;
		}

		// Token: 0x06000C43 RID: 3139 RVA: 0x0002BBFC File Offset: 0x00029DFC
		internal IEnumerable<DynamicTableEntity> Execute(CloudTableClient client, CloudTable table, TableRequestOptions requestOptions, OperationContext operationContext)
		{
			CommonUtility.AssertNotNullOrEmpty("tableName", table.Name);
			TableRequestOptions modifiedOptions = TableRequestOptions.ApplyDefaults(requestOptions, client);
			operationContext = (operationContext ?? new OperationContext());
			return CommonUtility.LazyEnumerable<DynamicTableEntity>(delegate(IContinuationToken continuationToken)
			{
				TableQuerySegment<DynamicTableEntity> tableQuerySegment = this.ExecuteQuerySegmented((TableContinuationToken)continuationToken, client, table, modifiedOptions, operationContext);
				return new ResultSegment<DynamicTableEntity>(tableQuerySegment.Results)
				{
					ContinuationToken = tableQuerySegment.ContinuationToken
				};
			}, (this.takeCount != null) ? ((long)this.takeCount.Value) : long.MaxValue);
		}

		// Token: 0x06000C44 RID: 3140 RVA: 0x0002BD00 File Offset: 0x00029F00
		internal IEnumerable<TResult> Execute<TResult>(CloudTableClient client, CloudTable table, EntityResolver<TResult> resolver, TableRequestOptions requestOptions, OperationContext operationContext)
		{
			CommonUtility.AssertNotNullOrEmpty("tableName", table.Name);
			TableRequestOptions modifiedOptions = TableRequestOptions.ApplyDefaults(requestOptions, client);
			operationContext = (operationContext ?? new OperationContext());
			return CommonUtility.LazyEnumerable<TResult>(delegate(IContinuationToken continuationToken)
			{
				TableQuerySegment<TResult> tableQuerySegment = this.ExecuteQuerySegmented<TResult>((TableContinuationToken)continuationToken, client, table, resolver, modifiedOptions, operationContext);
				return new ResultSegment<TResult>(tableQuerySegment.Results)
				{
					ContinuationToken = tableQuerySegment.ContinuationToken
				};
			}, (this.takeCount != null) ? ((long)this.takeCount.Value) : long.MaxValue);
		}

		// Token: 0x06000C45 RID: 3141 RVA: 0x0002BDAC File Offset: 0x00029FAC
		internal TableQuerySegment<DynamicTableEntity> ExecuteQuerySegmented(TableContinuationToken token, CloudTableClient client, CloudTable table, TableRequestOptions requestOptions, OperationContext operationContext)
		{
			CommonUtility.AssertNotNullOrEmpty("tableName", table.Name);
			TableRequestOptions tableRequestOptions = TableRequestOptions.ApplyDefaults(requestOptions, client);
			operationContext = (operationContext ?? new OperationContext());
			RESTCommand<TableQuerySegment<DynamicTableEntity>> cmd = TableQuery.QueryImpl(this, token, client, table, tableRequestOptions);
			return Executor.ExecuteSync<TableQuerySegment<DynamicTableEntity>>(cmd, tableRequestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x06000C46 RID: 3142 RVA: 0x0002BDF8 File Offset: 0x00029FF8
		internal TableQuerySegment<TResult> ExecuteQuerySegmented<TResult>(TableContinuationToken token, CloudTableClient client, CloudTable table, EntityResolver<TResult> resolver, TableRequestOptions requestOptions, OperationContext operationContext)
		{
			CommonUtility.AssertNotNullOrEmpty("tableName", table.Name);
			CommonUtility.AssertNotNull("resolver", resolver);
			TableRequestOptions tableRequestOptions = TableRequestOptions.ApplyDefaults(requestOptions, client);
			operationContext = (operationContext ?? new OperationContext());
			RESTCommand<TableQuerySegment<TResult>> cmd = TableQuery.QueryImpl<TResult>(this, token, client, table, resolver, tableRequestOptions);
			return Executor.ExecuteSync<TableQuerySegment<TResult>>(cmd, tableRequestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x06000C47 RID: 3143 RVA: 0x0002BE54 File Offset: 0x0002A054
		[DoesServiceRequest]
		internal ICancellableAsyncResult BeginExecuteQuerySegmented(TableContinuationToken token, CloudTableClient client, CloudTable table, TableRequestOptions requestOptions, OperationContext operationContext, AsyncCallback callback, object state)
		{
			CommonUtility.AssertNotNullOrEmpty("tableName", table.Name);
			TableRequestOptions tableRequestOptions = TableRequestOptions.ApplyDefaults(requestOptions, client);
			operationContext = (operationContext ?? new OperationContext());
			return Executor.BeginExecuteAsync<TableQuerySegment<DynamicTableEntity>>(TableQuery.QueryImpl(this, token, client, table, tableRequestOptions), tableRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x06000C48 RID: 3144 RVA: 0x0002BEA4 File Offset: 0x0002A0A4
		[DoesServiceRequest]
		internal ICancellableAsyncResult BeginExecuteQuerySegmented<TResult>(TableContinuationToken token, CloudTableClient client, CloudTable table, EntityResolver<TResult> resolver, TableRequestOptions requestOptions, OperationContext operationContext, AsyncCallback callback, object state)
		{
			CommonUtility.AssertNotNullOrEmpty("tableName", table.Name);
			TableRequestOptions tableRequestOptions = TableRequestOptions.ApplyDefaults(requestOptions, client);
			operationContext = (operationContext ?? new OperationContext());
			return Executor.BeginExecuteAsync<TableQuerySegment<TResult>>(TableQuery.QueryImpl<TResult>(this, token, client, table, resolver, tableRequestOptions), tableRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x06000C49 RID: 3145 RVA: 0x0002BEF4 File Offset: 0x0002A0F4
		internal TableQuerySegment<DynamicTableEntity> EndExecuteQuerySegmented(IAsyncResult asyncResult)
		{
			return Executor.EndExecuteAsync<TableQuerySegment<DynamicTableEntity>>(asyncResult);
		}

		// Token: 0x06000C4A RID: 3146 RVA: 0x0002BFB4 File Offset: 0x0002A1B4
		private static RESTCommand<TableQuerySegment<DynamicTableEntity>> QueryImpl(TableQuery query, TableContinuationToken token, CloudTableClient client, CloudTable table, TableRequestOptions requestOptions)
		{
			requestOptions.AssertPolicyIfRequired();
			if (requestOptions.EncryptionPolicy != null && query.SelectColumns != null)
			{
				query.SelectColumns.Add("_ClientEncryptionMetadata1");
				query.SelectColumns.Add("_ClientEncryptionMetadata2");
			}
			UriQueryBuilder builder = query.GenerateQueryBuilder();
			if (token != null)
			{
				token.ApplyToUriQueryBuilder(builder);
			}
			StorageUri storageUri = NavigationHelper.AppendPathToUri(client.StorageUri, table.Name);
			RESTCommand<TableQuerySegment<DynamicTableEntity>> restcommand = new RESTCommand<TableQuerySegment<DynamicTableEntity>>(client.Credentials, storageUri);
			requestOptions.ApplyToStorageCommand<TableQuerySegment<DynamicTableEntity>>(restcommand);
			restcommand.CommandLocationMode = CommonUtility.GetListingLocationMode(token);
			restcommand.RetrieveResponseStream = true;
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(client.AuthenticationHandler.SignRequest);
			restcommand.Builder = builder;
			restcommand.ParseError = new Func<Stream, HttpWebResponse, string, StorageExtendedErrorInformation>(StorageExtendedErrorInformation.ReadFromStreamUsingODataLib);
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder queryBuilder, int? timeout, bool useVersionHeader, OperationContext ctx) => TableOperationHttpWebRequestFactory.BuildRequestForTableQuery(uri, queryBuilder, timeout, useVersionHeader, ctx, requestOptions.PayloadFormat.Value));
			restcommand.PreProcessResponse = ((RESTCommand<TableQuerySegment<DynamicTableEntity>> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx) => HttpResponseParsers.ProcessExpectedStatusCodeNoException<TableQuerySegment<DynamicTableEntity>>(HttpStatusCode.OK, (resp != null) ? resp.StatusCode : HttpStatusCode.Unused, null, cmd, ex));
			restcommand.PostProcessResponse = delegate(RESTCommand<TableQuerySegment<DynamicTableEntity>> cmd, HttpWebResponse resp, OperationContext ctx)
			{
				ResultSegment<DynamicTableEntity> resultSegment = TableOperationHttpResponseParsers.TableQueryPostProcessGeneric<DynamicTableEntity, DynamicTableEntity>(cmd.ResponseStream, new Func<string, string, DateTimeOffset, IDictionary<string, EntityProperty>, string, DynamicTableEntity>(EntityUtilities.ResolveDynamicEntity), resp, requestOptions, ctx, client.AccountName);
				if (resultSegment.ContinuationToken != null)
				{
					resultSegment.ContinuationToken.TargetLocation = new StorageLocation?(cmd.CurrentResult.TargetLocation);
				}
				return new TableQuerySegment<DynamicTableEntity>(resultSegment);
			};
			return restcommand;
		}

		// Token: 0x06000C4B RID: 3147 RVA: 0x0002C1AC File Offset: 0x0002A3AC
		private static RESTCommand<TableQuerySegment<RESULT_TYPE>> QueryImpl<RESULT_TYPE>(TableQuery query, TableContinuationToken token, CloudTableClient client, CloudTable table, EntityResolver<RESULT_TYPE> resolver, TableRequestOptions requestOptions)
		{
			UriQueryBuilder builder = query.GenerateQueryBuilder();
			if (token != null)
			{
				token.ApplyToUriQueryBuilder(builder);
			}
			StorageUri storageUri = NavigationHelper.AppendPathToUri(client.StorageUri, table.Name);
			RESTCommand<TableQuerySegment<RESULT_TYPE>> restcommand = new RESTCommand<TableQuerySegment<RESULT_TYPE>>(client.Credentials, storageUri);
			requestOptions.ApplyToStorageCommand<TableQuerySegment<RESULT_TYPE>>(restcommand);
			restcommand.CommandLocationMode = CommonUtility.GetListingLocationMode(token);
			restcommand.RetrieveResponseStream = true;
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(client.AuthenticationHandler.SignRequest);
			restcommand.Builder = builder;
			restcommand.ParseError = new Func<Stream, HttpWebResponse, string, StorageExtendedErrorInformation>(StorageExtendedErrorInformation.ReadFromStreamUsingODataLib);
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder queryBuilder, int? timeout, bool useVersionHeader, OperationContext ctx) => TableOperationHttpWebRequestFactory.BuildRequestForTableQuery(uri, queryBuilder, timeout, useVersionHeader, ctx, requestOptions.PayloadFormat.Value));
			restcommand.PreProcessResponse = ((RESTCommand<TableQuerySegment<RESULT_TYPE>> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx) => HttpResponseParsers.ProcessExpectedStatusCodeNoException<TableQuerySegment<RESULT_TYPE>>(HttpStatusCode.OK, (resp != null) ? resp.StatusCode : HttpStatusCode.Unused, null, cmd, ex));
			restcommand.PostProcessResponse = delegate(RESTCommand<TableQuerySegment<RESULT_TYPE>> cmd, HttpWebResponse resp, OperationContext ctx)
			{
				ResultSegment<RESULT_TYPE> resultSegment = TableOperationHttpResponseParsers.TableQueryPostProcessGeneric<RESULT_TYPE, DynamicTableEntity>(cmd.ResponseStream, new Func<string, string, DateTimeOffset, IDictionary<string, EntityProperty>, string, RESULT_TYPE>(resolver.Invoke), resp, requestOptions, ctx, client.AccountName);
				if (resultSegment.ContinuationToken != null)
				{
					resultSegment.ContinuationToken.TargetLocation = new StorageLocation?(cmd.CurrentResult.TargetLocation);
				}
				return new TableQuerySegment<RESULT_TYPE>(resultSegment);
			};
			return restcommand;
		}

		// Token: 0x06000C4C RID: 3148 RVA: 0x0002C29C File Offset: 0x0002A49C
		public static string GenerateFilterCondition(string propertyName, string operation, string givenValue)
		{
			givenValue = (givenValue ?? string.Empty);
			return TableQuery.GenerateFilterCondition(propertyName, operation, givenValue, EdmType.String);
		}

		// Token: 0x06000C4D RID: 3149 RVA: 0x0002C2B3 File Offset: 0x0002A4B3
		public static string GenerateFilterConditionForBool(string propertyName, string operation, bool givenValue)
		{
			return TableQuery.GenerateFilterCondition(propertyName, operation, givenValue ? "true" : "false", EdmType.Boolean);
		}

		// Token: 0x06000C4E RID: 3150 RVA: 0x0002C2CC File Offset: 0x0002A4CC
		public static string GenerateFilterConditionForBinary(string propertyName, string operation, byte[] givenValue)
		{
			CommonUtility.AssertNotNull("value", givenValue);
			StringBuilder stringBuilder = new StringBuilder();
			foreach (byte b in givenValue)
			{
				stringBuilder.AppendFormat("{0:x2}", b);
			}
			return TableQuery.GenerateFilterCondition(propertyName, operation, stringBuilder.ToString(), EdmType.Binary);
		}

		// Token: 0x06000C4F RID: 3151 RVA: 0x0002C320 File Offset: 0x0002A520
		public static string GenerateFilterConditionForDate(string propertyName, string operation, DateTimeOffset givenValue)
		{
			return TableQuery.GenerateFilterCondition(propertyName, operation, givenValue.UtcDateTime.ToString("o", CultureInfo.InvariantCulture), EdmType.DateTime);
		}

		// Token: 0x06000C50 RID: 3152 RVA: 0x0002C34E File Offset: 0x0002A54E
		public static string GenerateFilterConditionForDouble(string propertyName, string operation, double givenValue)
		{
			return TableQuery.GenerateFilterCondition(propertyName, operation, Convert.ToString(givenValue, CultureInfo.InvariantCulture), EdmType.Double);
		}

		// Token: 0x06000C51 RID: 3153 RVA: 0x0002C363 File Offset: 0x0002A563
		public static string GenerateFilterConditionForInt(string propertyName, string operation, int givenValue)
		{
			return TableQuery.GenerateFilterCondition(propertyName, operation, Convert.ToString(givenValue, CultureInfo.InvariantCulture), EdmType.Int32);
		}

		// Token: 0x06000C52 RID: 3154 RVA: 0x0002C378 File Offset: 0x0002A578
		public static string GenerateFilterConditionForLong(string propertyName, string operation, long givenValue)
		{
			return TableQuery.GenerateFilterCondition(propertyName, operation, Convert.ToString(givenValue, CultureInfo.InvariantCulture), EdmType.Int64);
		}

		// Token: 0x06000C53 RID: 3155 RVA: 0x0002C38D File Offset: 0x0002A58D
		public static string GenerateFilterConditionForGuid(string propertyName, string operation, Guid givenValue)
		{
			CommonUtility.AssertNotNull("value", givenValue);
			return TableQuery.GenerateFilterCondition(propertyName, operation, givenValue.ToString(), EdmType.Guid);
		}

		// Token: 0x06000C54 RID: 3156 RVA: 0x0002C3B4 File Offset: 0x0002A5B4
		private static string GenerateFilterCondition(string propertyName, string operation, string givenValue, EdmType edmType)
		{
			string text;
			if (edmType == EdmType.Boolean || edmType == EdmType.Double || edmType == EdmType.Int32)
			{
				text = givenValue;
			}
			else if (edmType == EdmType.Int64)
			{
				text = string.Format(CultureInfo.InvariantCulture, "{0}L", new object[]
				{
					givenValue
				});
			}
			else if (edmType == EdmType.DateTime)
			{
				text = string.Format(CultureInfo.InvariantCulture, "datetime'{0}'", new object[]
				{
					givenValue
				});
			}
			else if (edmType == EdmType.Guid)
			{
				text = string.Format(CultureInfo.InvariantCulture, "guid'{0}'", new object[]
				{
					givenValue
				});
			}
			else if (edmType == EdmType.Binary)
			{
				text = string.Format(CultureInfo.InvariantCulture, "X'{0}'", new object[]
				{
					givenValue
				});
			}
			else
			{
				text = string.Format(CultureInfo.InvariantCulture, "'{0}'", new object[]
				{
					givenValue.Replace("'", "''")
				});
			}
			return string.Format(CultureInfo.InvariantCulture, "{0} {1} {2}", new object[]
			{
				propertyName,
				operation,
				text
			});
		}

		// Token: 0x06000C55 RID: 3157 RVA: 0x0002C4BC File Offset: 0x0002A6BC
		public static string CombineFilters(string filterA, string operatorString, string filterB)
		{
			return string.Format(CultureInfo.InvariantCulture, "({0}) {1} ({2})", new object[]
			{
				filterA,
				operatorString,
				filterB
			});
		}

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x06000C56 RID: 3158 RVA: 0x0002C4EC File Offset: 0x0002A6EC
		// (set) Token: 0x06000C57 RID: 3159 RVA: 0x0002C4F4 File Offset: 0x0002A6F4
		public int? TakeCount
		{
			get
			{
				return this.takeCount;
			}
			set
			{
				if (value != null && value.Value <= 0)
				{
					throw new ArgumentException("Take count must be positive and greater than 0.");
				}
				this.takeCount = value;
			}
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x06000C58 RID: 3160 RVA: 0x0002C51B File Offset: 0x0002A71B
		// (set) Token: 0x06000C59 RID: 3161 RVA: 0x0002C523 File Offset: 0x0002A723
		public string FilterString { get; set; }

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x06000C5A RID: 3162 RVA: 0x0002C52C File Offset: 0x0002A72C
		// (set) Token: 0x06000C5B RID: 3163 RVA: 0x0002C534 File Offset: 0x0002A734
		public IList<string> SelectColumns { get; set; }

		// Token: 0x06000C5C RID: 3164 RVA: 0x0002C53D File Offset: 0x0002A73D
		public TableQuery Select(IList<string> columns)
		{
			this.SelectColumns = columns;
			return this;
		}

		// Token: 0x06000C5D RID: 3165 RVA: 0x0002C547 File Offset: 0x0002A747
		public TableQuery Take(int? take)
		{
			this.TakeCount = take;
			return this;
		}

		// Token: 0x06000C5E RID: 3166 RVA: 0x0002C551 File Offset: 0x0002A751
		public TableQuery Where(string filter)
		{
			this.FilterString = filter;
			return this;
		}

		// Token: 0x06000C5F RID: 3167 RVA: 0x0002C55C File Offset: 0x0002A75C
		internal UriQueryBuilder GenerateQueryBuilder()
		{
			UriQueryBuilder uriQueryBuilder = new UriQueryBuilder();
			if (!string.IsNullOrEmpty(this.FilterString))
			{
				uriQueryBuilder.Add("$filter", this.FilterString);
			}
			if (this.takeCount != null)
			{
				uriQueryBuilder.Add("$top", Convert.ToString(Math.Min(this.takeCount.Value, 1000), CultureInfo.InvariantCulture));
			}
			if (this.SelectColumns != null && this.SelectColumns.Count > 0)
			{
				StringBuilder stringBuilder = new StringBuilder();
				bool flag = false;
				bool flag2 = false;
				bool flag3 = false;
				for (int i = 0; i < this.SelectColumns.Count; i++)
				{
					if (this.SelectColumns[i] == "PartitionKey")
					{
						flag2 = true;
					}
					else if (this.SelectColumns[i] == "RowKey")
					{
						flag = true;
					}
					else if (this.SelectColumns[i] == "Timestamp")
					{
						flag3 = true;
					}
					stringBuilder.Append(this.SelectColumns[i]);
					if (i < this.SelectColumns.Count - 1)
					{
						stringBuilder.Append(",");
					}
				}
				if (!flag2)
				{
					stringBuilder.Append(",");
					stringBuilder.Append("PartitionKey");
				}
				if (!flag)
				{
					stringBuilder.Append(",");
					stringBuilder.Append("RowKey");
				}
				if (!flag3)
				{
					stringBuilder.Append(",");
					stringBuilder.Append("Timestamp");
				}
				uriQueryBuilder.Add("$select", stringBuilder.ToString());
			}
			return uriQueryBuilder;
		}

		// Token: 0x04000176 RID: 374
		private int? takeCount = null;
	}
}
