using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Core;
using Microsoft.WindowsAzure.Storage.Core.Executor;
using Microsoft.WindowsAzure.Storage.Core.Util;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;
using Microsoft.WindowsAzure.Storage.Table.Protocol;
using Microsoft.WindowsAzure.Storage.Table.Queryable;

namespace Microsoft.WindowsAzure.Storage.Table
{
	// Token: 0x02000040 RID: 64
	public sealed class TableQuery<TElement> : IQueryable<TElement>, IEnumerable<TElement>, IQueryable, IEnumerable
	{
		// Token: 0x06000C14 RID: 3092 RVA: 0x0002AC84 File Offset: 0x00028E84
		public TableQuery()
		{
			if (typeof(TElement).GetInterface(typeof(ITableEntity).FullName, false) == null)
			{
				throw new NotSupportedException("TableQuery Generic Type must implement the ITableEntity Interface");
			}
			if (typeof(TElement).GetConstructor(Type.EmptyTypes) == null)
			{
				throw new NotSupportedException("TableQuery Generic Type must provide a default parameterless constructor.");
			}
		}

		// Token: 0x06000C15 RID: 3093 RVA: 0x0002ACFC File Offset: 0x00028EFC
		internal TableQuery(CloudTable table)
		{
			this.queryProvider = new TableQueryProvider(table);
			this.queryExpression = new ResourceSetExpression(typeof(IOrderedQueryable<TElement>), null, Expression.Constant("0"), typeof(TElement), null, CountOption.None, null, null);
		}

		// Token: 0x06000C16 RID: 3094 RVA: 0x0002AD55 File Offset: 0x00028F55
		internal TableQuery(Expression queryExpression, TableQueryProvider queryProvider)
		{
			this.queryProvider = queryProvider;
			this.queryExpression = queryExpression;
		}

		// Token: 0x06000C17 RID: 3095 RVA: 0x0002AD78 File Offset: 0x00028F78
		[DoesServiceRequest]
		public IEnumerable<TElement> Execute(TableRequestOptions requestOptions = null, OperationContext operationContext = null)
		{
			if (this.queryProvider == null)
			{
				throw new InvalidOperationException("Unknown Table. The TableQuery does not have an associated CloudTable Reference. Please execute the query via the CloudTable ExecuteQuery APIs.");
			}
			TableQuery<TElement>.ExecutionInfo executionInfo = this.Bind();
			executionInfo.RequestOptions = (requestOptions ?? executionInfo.RequestOptions);
			executionInfo.OperationContext = (operationContext ?? executionInfo.OperationContext);
			if (executionInfo.Resolver != null)
			{
				return this.ExecuteInternal<TElement>(this.queryProvider.Table.ServiceClient, this.queryProvider.Table, executionInfo.Resolver, executionInfo.RequestOptions, executionInfo.OperationContext);
			}
			return this.ExecuteInternal(this.queryProvider.Table.ServiceClient, this.queryProvider.Table, executionInfo.RequestOptions, executionInfo.OperationContext);
		}

		// Token: 0x06000C18 RID: 3096 RVA: 0x0002AE2A File Offset: 0x0002902A
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginExecuteSegmented(TableContinuationToken currentToken, AsyncCallback callback, object state)
		{
			return this.BeginExecuteSegmented(currentToken, null, null, callback, state);
		}

		// Token: 0x06000C19 RID: 3097 RVA: 0x0002AE38 File Offset: 0x00029038
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginExecuteSegmented(TableContinuationToken currentToken, TableRequestOptions requestOptions, OperationContext operationContext, AsyncCallback callback, object state)
		{
			if (this.queryProvider == null)
			{
				throw new InvalidOperationException("Unknown Table. The TableQuery does not have an associated CloudTable Reference. Please execute the query via the CloudTable ExecuteQuery APIs.");
			}
			TableQuery<TElement>.ExecutionInfo executionInfo = this.Bind();
			executionInfo.RequestOptions = ((requestOptions == null) ? executionInfo.RequestOptions : requestOptions);
			executionInfo.OperationContext = ((operationContext == null) ? executionInfo.OperationContext : operationContext);
			if (executionInfo.Resolver != null)
			{
				return this.BeginExecuteQuerySegmentedInternal<TElement>(currentToken, this.queryProvider.Table.ServiceClient, this.queryProvider.Table, executionInfo.Resolver, executionInfo.RequestOptions, executionInfo.OperationContext, callback, state);
			}
			return this.BeginExecuteQuerySegmentedInternal(currentToken, this.queryProvider.Table.ServiceClient, this.queryProvider.Table, executionInfo.RequestOptions, executionInfo.OperationContext, callback, state);
		}

		// Token: 0x06000C1A RID: 3098 RVA: 0x0002AEF6 File Offset: 0x000290F6
		public TableQuerySegment<TElement> EndExecuteSegmented(IAsyncResult asyncResult)
		{
			return Executor.EndExecuteAsync<TableQuerySegment<TElement>>(asyncResult);
		}

		// Token: 0x06000C1B RID: 3099 RVA: 0x0002AEFE File Offset: 0x000290FE
		[DoesServiceRequest]
		public Task<TableQuerySegment<TElement>> ExecuteSegmentedAsync(TableContinuationToken currentToken)
		{
			return this.ExecuteSegmentedAsync(currentToken, CancellationToken.None);
		}

		// Token: 0x06000C1C RID: 3100 RVA: 0x0002AF0C File Offset: 0x0002910C
		[DoesServiceRequest]
		public Task<TableQuerySegment<TElement>> ExecuteSegmentedAsync(TableContinuationToken currentToken, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<TableContinuationToken, TableQuerySegment<TElement>>(new Func<TableContinuationToken, AsyncCallback, object, ICancellableAsyncResult>(this.BeginExecuteSegmented), new Func<IAsyncResult, TableQuerySegment<TElement>>(this.EndExecuteSegmented), currentToken, cancellationToken);
		}

		// Token: 0x06000C1D RID: 3101 RVA: 0x0002AF2D File Offset: 0x0002912D
		[DoesServiceRequest]
		public Task<TableQuerySegment<TElement>> ExecuteSegmentedAsync(TableContinuationToken currentToken, TableRequestOptions requestOptions, OperationContext operationContext)
		{
			return this.ExecuteSegmentedAsync(currentToken, requestOptions, operationContext, CancellationToken.None);
		}

		// Token: 0x06000C1E RID: 3102 RVA: 0x0002AF3D File Offset: 0x0002913D
		[DoesServiceRequest]
		public Task<TableQuerySegment<TElement>> ExecuteSegmentedAsync(TableContinuationToken currentToken, TableRequestOptions requestOptions, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<TableContinuationToken, TableRequestOptions, OperationContext, TableQuerySegment<TElement>>(new Func<TableContinuationToken, TableRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginExecuteSegmented), new Func<IAsyncResult, TableQuerySegment<TElement>>(this.EndExecuteSegmented), currentToken, requestOptions, operationContext, cancellationToken);
		}

		// Token: 0x06000C1F RID: 3103 RVA: 0x0002AF64 File Offset: 0x00029164
		[DoesServiceRequest]
		public TableQuerySegment<TElement> ExecuteSegmented(TableContinuationToken continuationToken, TableRequestOptions requestOptions = null, OperationContext operationContext = null)
		{
			if (this.queryProvider == null)
			{
				throw new InvalidOperationException("Unknown Table. The TableQuery does not have an associated CloudTable Reference. Please execute the query via the CloudTable ExecuteQuery APIs.");
			}
			TableQuery<TElement>.ExecutionInfo executionInfo = this.Bind();
			executionInfo.RequestOptions = ((requestOptions == null) ? executionInfo.RequestOptions : requestOptions);
			executionInfo.OperationContext = ((operationContext == null) ? executionInfo.OperationContext : operationContext);
			if (executionInfo.Resolver != null)
			{
				return this.ExecuteQuerySegmentedInternal<TElement>(continuationToken, this.queryProvider.Table.ServiceClient, this.queryProvider.Table, executionInfo.Resolver, executionInfo.RequestOptions, executionInfo.OperationContext);
			}
			return this.ExecuteQuerySegmentedInternal(continuationToken, this.queryProvider.Table.ServiceClient, this.queryProvider.Table, executionInfo.RequestOptions, executionInfo.OperationContext);
		}

		// Token: 0x06000C20 RID: 3104 RVA: 0x0002B01C File Offset: 0x0002921C
		public IEnumerator<TElement> GetEnumerator()
		{
			if (this.Expression == null)
			{
				TableRequestOptions requestOptions = TableRequestOptions.ApplyDefaults(null, this.queryProvider.Table.ServiceClient);
				return this.ExecuteInternal(this.queryProvider.Table.ServiceClient, this.queryProvider.Table, requestOptions, null).GetEnumerator();
			}
			TableQuery<TElement>.ExecutionInfo executionInfo = this.Bind();
			if (executionInfo.Resolver != null)
			{
				return this.ExecuteInternal<TElement>(this.queryProvider.Table.ServiceClient, this.queryProvider.Table, executionInfo.Resolver, executionInfo.RequestOptions, executionInfo.OperationContext).GetEnumerator();
			}
			return this.ExecuteInternal(this.queryProvider.Table.ServiceClient, this.queryProvider.Table, executionInfo.RequestOptions, executionInfo.OperationContext).GetEnumerator();
		}

		// Token: 0x06000C21 RID: 3105 RVA: 0x0002B0EB File Offset: 0x000292EB
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x06000C22 RID: 3106 RVA: 0x0002B0F3 File Offset: 0x000292F3
		public Type ElementType
		{
			get
			{
				return typeof(TElement);
			}
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x06000C23 RID: 3107 RVA: 0x0002B0FF File Offset: 0x000292FF
		public Expression Expression
		{
			get
			{
				return this.queryExpression;
			}
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x06000C24 RID: 3108 RVA: 0x0002B107 File Offset: 0x00029307
		public IQueryProvider Provider
		{
			get
			{
				return this.queryProvider;
			}
		}

		// Token: 0x06000C25 RID: 3109 RVA: 0x0002B180 File Offset: 0x00029380
		internal TableQuery<TElement>.ExecutionInfo Bind()
		{
			TableQuery<TElement>.ExecutionInfo executionInfo = new TableQuery<TElement>.ExecutionInfo();
			if (this.Expression != null)
			{
				Dictionary<Expression, Expression> rewrites = new Dictionary<Expression, Expression>(ReferenceEqualityComparer<Expression>.Instance);
				Expression expression = Evaluator.PartialEval(this.Expression);
				Expression e = ExpressionNormalizer.Normalize(expression, rewrites);
				Expression e2 = ResourceBinder.Bind(e);
				ExpressionParser parser = new ExpressionParser();
				parser.Translate(e2);
				this.TakeCount = parser.TakeCount;
				this.FilterString = parser.FilterString;
				this.SelectColumns = parser.SelectColumns;
				executionInfo.RequestOptions = parser.RequestOptions;
				executionInfo.OperationContext = parser.OperationContext;
				if (parser.Resolver == null)
				{
					if (parser.Projection != null && parser.Projection.Selector != ProjectionQueryOptionExpression.DefaultLambda)
					{
						Type intermediateType = parser.Projection.Selector.Parameters[0].Type;
						ParameterExpression parameterExpression = Expression.Parameter(typeof(object));
						Func<object, TElement> projectorFunc = Expression.Lambda<Func<object, TElement>>(Expression.Invoke(parser.Projection.Selector, new Expression[]
						{
							Expression.Convert(parameterExpression, intermediateType)
						}), new ParameterExpression[]
						{
							parameterExpression
						}).Compile();
						executionInfo.Resolver = delegate(string pk, string rk, DateTimeOffset ts, IDictionary<string, EntityProperty> props, string etag)
						{
							ITableEntity tableEntity = (ITableEntity)EntityUtilities.InstantiateEntityFromType(intermediateType);
							tableEntity.PartitionKey = pk;
							tableEntity.RowKey = rk;
							tableEntity.Timestamp = ts;
							tableEntity.ReadEntity(props, parser.OperationContext);
							tableEntity.ETag = etag;
							return projectorFunc(tableEntity);
						};
					}
				}
				else
				{
					executionInfo.Resolver = (EntityResolver<TElement>)parser.Resolver.Value;
				}
			}
			executionInfo.RequestOptions = TableRequestOptions.ApplyDefaults(executionInfo.RequestOptions, this.queryProvider.Table.ServiceClient);
			executionInfo.OperationContext = (executionInfo.OperationContext ?? new OperationContext());
			return executionInfo;
		}

		// Token: 0x06000C26 RID: 3110 RVA: 0x0002B3DC File Offset: 0x000295DC
		internal IEnumerable<TElement> ExecuteInternal(CloudTableClient client, CloudTable table, TableRequestOptions requestOptions, OperationContext operationContext)
		{
			CommonUtility.AssertNotNullOrEmpty("tableName", table.Name);
			TableRequestOptions modifiedOptions = TableRequestOptions.ApplyDefaults(requestOptions, client);
			operationContext = (operationContext ?? new OperationContext());
			return CommonUtility.LazyEnumerable<TElement>(delegate(IContinuationToken continuationToken)
			{
				TableQuerySegment<TElement> tableQuerySegment = this.ExecuteQuerySegmentedInternal((TableContinuationToken)continuationToken, client, table, modifiedOptions, operationContext);
				return new ResultSegment<TElement>(tableQuerySegment.Results)
				{
					ContinuationToken = tableQuerySegment.ContinuationToken
				};
			}, (this.TakeCount != null) ? ((long)this.TakeCount.Value) : long.MaxValue);
		}

		// Token: 0x06000C27 RID: 3111 RVA: 0x0002B488 File Offset: 0x00029688
		internal TableQuerySegment<TElement> ExecuteQuerySegmentedInternal(TableContinuationToken token, CloudTableClient client, CloudTable table, TableRequestOptions requestOptions, OperationContext operationContext)
		{
			CommonUtility.AssertNotNullOrEmpty("tableName", table.Name);
			TableRequestOptions tableRequestOptions = TableRequestOptions.ApplyDefaults(requestOptions, client);
			operationContext = (operationContext ?? new OperationContext());
			RESTCommand<TableQuerySegment<TElement>> cmd = TableQuery<TElement>.QueryImpl<TElement, TElement>(this, token, client, table, new EntityResolver<TElement>(EntityUtilities.ResolveEntityByType<TElement>), tableRequestOptions);
			return Executor.ExecuteSync<TableQuerySegment<TElement>>(cmd, tableRequestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x06000C28 RID: 3112 RVA: 0x0002B4E0 File Offset: 0x000296E0
		internal ICancellableAsyncResult BeginExecuteQuerySegmentedInternal(TableContinuationToken token, CloudTableClient client, CloudTable table, TableRequestOptions requestOptions, OperationContext operationContext, AsyncCallback callback, object state)
		{
			CommonUtility.AssertNotNullOrEmpty("tableName", table.Name);
			TableRequestOptions tableRequestOptions = TableRequestOptions.ApplyDefaults(requestOptions, client);
			operationContext = (operationContext ?? new OperationContext());
			return Executor.BeginExecuteAsync<TableQuerySegment<TElement>>(TableQuery<TElement>.QueryImpl<TElement, TElement>(this, token, client, table, new EntityResolver<TElement>(EntityUtilities.ResolveEntityByType<TElement>), tableRequestOptions), tableRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x06000C29 RID: 3113 RVA: 0x0002B53A File Offset: 0x0002973A
		internal TableQuerySegment<TElement> EndExecuteQuerySegmentedInternal(IAsyncResult asyncResult)
		{
			return Executor.EndExecuteAsync<TableQuerySegment<TElement>>(asyncResult);
		}

		// Token: 0x06000C2A RID: 3114 RVA: 0x0002B5A4 File Offset: 0x000297A4
		internal IEnumerable<TResult> ExecuteInternal<TResult>(CloudTableClient client, CloudTable table, EntityResolver<TResult> resolver, TableRequestOptions requestOptions, OperationContext operationContext)
		{
			CommonUtility.AssertNotNullOrEmpty("tableName", table.Name);
			CommonUtility.AssertNotNull("resolver", resolver);
			TableRequestOptions modifiedOptions = TableRequestOptions.ApplyDefaults(requestOptions, client);
			operationContext = (operationContext ?? new OperationContext());
			return CommonUtility.LazyEnumerable<TResult>(delegate(IContinuationToken continuationToken)
			{
				TableQuerySegment<TResult> tableQuerySegment = this.ExecuteQuerySegmentedInternal<TResult>((TableContinuationToken)continuationToken, client, table, resolver, modifiedOptions, operationContext);
				return new ResultSegment<TResult>(tableQuerySegment.Results)
				{
					ContinuationToken = tableQuerySegment.ContinuationToken
				};
			}, (this.takeCount != null) ? ((long)this.takeCount.Value) : long.MaxValue);
		}

		// Token: 0x06000C2B RID: 3115 RVA: 0x0002B660 File Offset: 0x00029860
		internal TableQuerySegment<TResult> ExecuteQuerySegmentedInternal<TResult>(TableContinuationToken token, CloudTableClient client, CloudTable table, EntityResolver<TResult> resolver, TableRequestOptions requestOptions, OperationContext operationContext)
		{
			CommonUtility.AssertNotNullOrEmpty("tableName", table.Name);
			CommonUtility.AssertNotNull("resolver", resolver);
			TableRequestOptions tableRequestOptions = TableRequestOptions.ApplyDefaults(requestOptions, client);
			operationContext = (operationContext ?? new OperationContext());
			RESTCommand<TableQuerySegment<TResult>> cmd = TableQuery<TElement>.QueryImpl<TElement, TResult>(this, token, client, table, resolver, tableRequestOptions);
			return Executor.ExecuteSync<TableQuerySegment<TResult>>(cmd, tableRequestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x06000C2C RID: 3116 RVA: 0x0002B6BC File Offset: 0x000298BC
		internal ICancellableAsyncResult BeginExecuteQuerySegmentedInternal<TResult>(TableContinuationToken token, CloudTableClient client, CloudTable table, EntityResolver<TResult> resolver, TableRequestOptions requestOptions, OperationContext operationContext, AsyncCallback callback, object state)
		{
			CommonUtility.AssertNotNullOrEmpty("tableName", table.Name);
			CommonUtility.AssertNotNull("resolver", resolver);
			TableRequestOptions tableRequestOptions = TableRequestOptions.ApplyDefaults(requestOptions, client);
			operationContext = (operationContext ?? new OperationContext());
			return Executor.BeginExecuteAsync<TableQuerySegment<TResult>>(TableQuery<TElement>.QueryImpl<TElement, TResult>(this, token, client, table, resolver, tableRequestOptions), tableRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x06000C2D RID: 3117 RVA: 0x0002B718 File Offset: 0x00029918
		internal TableQuerySegment<TResult> EndExecuteQuerySegmentedInternal<TResult>(IAsyncResult asyncResult)
		{
			return Executor.EndExecuteAsync<TableQuerySegment<TResult>>(asyncResult);
		}

		// Token: 0x06000C2E RID: 3118 RVA: 0x0002B7DC File Offset: 0x000299DC
		private static RESTCommand<TableQuerySegment<RESULT_TYPE>> QueryImpl<T, RESULT_TYPE>(TableQuery<T> query, TableContinuationToken token, CloudTableClient client, CloudTable table, EntityResolver<RESULT_TYPE> resolver, TableRequestOptions requestOptions)
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
				ResultSegment<RESULT_TYPE> resultSegment = TableOperationHttpResponseParsers.TableQueryPostProcessGeneric<RESULT_TYPE, T>(cmd.ResponseStream, new Func<string, string, DateTimeOffset, IDictionary<string, EntityProperty>, string, RESULT_TYPE>(resolver.Invoke), resp, requestOptions, ctx, client.AccountName);
				if (resultSegment.ContinuationToken != null)
				{
					resultSegment.ContinuationToken.TargetLocation = new StorageLocation?(cmd.CurrentResult.TargetLocation);
				}
				return new TableQuerySegment<RESULT_TYPE>(resultSegment);
			};
			return restcommand;
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x06000C2F RID: 3119 RVA: 0x0002B90C File Offset: 0x00029B0C
		// (set) Token: 0x06000C30 RID: 3120 RVA: 0x0002B914 File Offset: 0x00029B14
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

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x06000C31 RID: 3121 RVA: 0x0002B93B File Offset: 0x00029B3B
		// (set) Token: 0x06000C32 RID: 3122 RVA: 0x0002B943 File Offset: 0x00029B43
		public string FilterString { get; set; }

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x06000C33 RID: 3123 RVA: 0x0002B94C File Offset: 0x00029B4C
		// (set) Token: 0x06000C34 RID: 3124 RVA: 0x0002B954 File Offset: 0x00029B54
		public IList<string> SelectColumns { get; set; }

		// Token: 0x06000C35 RID: 3125 RVA: 0x0002B95D File Offset: 0x00029B5D
		public TableQuery<TElement> Select(IList<string> columns)
		{
			if (this.Expression != null)
			{
				throw new NotSupportedException("Fluent methods may not be invoked on a Query created via CloudTable.CreateQuery<T>()");
			}
			this.SelectColumns = columns;
			return this;
		}

		// Token: 0x06000C36 RID: 3126 RVA: 0x0002B97A File Offset: 0x00029B7A
		public TableQuery<TElement> Take(int? take)
		{
			if (this.Expression != null)
			{
				throw new NotSupportedException("Fluent methods may not be invoked on a Query created via CloudTable.CreateQuery<T>()");
			}
			this.TakeCount = take;
			return this;
		}

		// Token: 0x06000C37 RID: 3127 RVA: 0x0002B997 File Offset: 0x00029B97
		public TableQuery<TElement> Where(string filter)
		{
			if (this.Expression != null)
			{
				throw new NotSupportedException("Fluent methods may not be invoked on a Query created via CloudTable.CreateQuery<T>()");
			}
			this.FilterString = filter;
			return this;
		}

		// Token: 0x06000C38 RID: 3128 RVA: 0x0002B9B4 File Offset: 0x00029BB4
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

		// Token: 0x0400016E RID: 366
		private readonly Expression queryExpression;

		// Token: 0x0400016F RID: 367
		private readonly TableQueryProvider queryProvider;

		// Token: 0x04000170 RID: 368
		private int? takeCount = null;

		// Token: 0x02000041 RID: 65
		internal class ExecutionInfo
		{
			// Token: 0x17000138 RID: 312
			// (get) Token: 0x06000C3A RID: 3130 RVA: 0x0002BB50 File Offset: 0x00029D50
			// (set) Token: 0x06000C3B RID: 3131 RVA: 0x0002BB58 File Offset: 0x00029D58
			public OperationContext OperationContext { get; set; }

			// Token: 0x17000139 RID: 313
			// (get) Token: 0x06000C3C RID: 3132 RVA: 0x0002BB61 File Offset: 0x00029D61
			// (set) Token: 0x06000C3D RID: 3133 RVA: 0x0002BB69 File Offset: 0x00029D69
			public TableRequestOptions RequestOptions { get; set; }

			// Token: 0x1700013A RID: 314
			// (get) Token: 0x06000C3E RID: 3134 RVA: 0x0002BB72 File Offset: 0x00029D72
			// (set) Token: 0x06000C3F RID: 3135 RVA: 0x0002BB7A File Offset: 0x00029D7A
			public EntityResolver<TElement> Resolver { get; set; }
		}
	}
}
