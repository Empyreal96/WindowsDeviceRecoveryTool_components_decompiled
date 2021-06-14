using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Core;
using Microsoft.WindowsAzure.Storage.Core.Auth;
using Microsoft.WindowsAzure.Storage.Core.Executor;
using Microsoft.WindowsAzure.Storage.Core.Util;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;
using Microsoft.WindowsAzure.Storage.Table.Protocol;
using Microsoft.WindowsAzure.Storage.Table.Queryable;

namespace Microsoft.WindowsAzure.Storage.Table
{
	// Token: 0x0200003A RID: 58
	public sealed class CloudTable
	{
		// Token: 0x06000AE5 RID: 2789 RVA: 0x00026A74 File Offset: 0x00024C74
		[DoesServiceRequest]
		public TableResult Execute(TableOperation operation, TableRequestOptions requestOptions = null, OperationContext operationContext = null)
		{
			CommonUtility.AssertNotNull("operation", operation);
			return operation.Execute(this.ServiceClient, this, requestOptions, operationContext);
		}

		// Token: 0x06000AE6 RID: 2790 RVA: 0x00026A90 File Offset: 0x00024C90
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginExecute(TableOperation operation, AsyncCallback callback, object state)
		{
			CommonUtility.AssertNotNull("batch", operation);
			return this.BeginExecute(operation, null, null, callback, state);
		}

		// Token: 0x06000AE7 RID: 2791 RVA: 0x00026AA8 File Offset: 0x00024CA8
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginExecute(TableOperation operation, TableRequestOptions requestOptions, OperationContext operationContext, AsyncCallback callback, object state)
		{
			CommonUtility.AssertNotNull("operation", operation);
			return operation.BeginExecute(this.ServiceClient, this, requestOptions, operationContext, callback, state);
		}

		// Token: 0x06000AE8 RID: 2792 RVA: 0x00026AC8 File Offset: 0x00024CC8
		public TableResult EndExecute(IAsyncResult asyncResult)
		{
			return Executor.EndExecuteAsync<TableResult>(asyncResult);
		}

		// Token: 0x06000AE9 RID: 2793 RVA: 0x00026AD0 File Offset: 0x00024CD0
		[DoesServiceRequest]
		public Task<TableResult> ExecuteAsync(TableOperation operation)
		{
			return this.ExecuteAsync(operation, CancellationToken.None);
		}

		// Token: 0x06000AEA RID: 2794 RVA: 0x00026ADE File Offset: 0x00024CDE
		[DoesServiceRequest]
		public Task<TableResult> ExecuteAsync(TableOperation operation, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<TableOperation, TableResult>(new Func<TableOperation, AsyncCallback, object, ICancellableAsyncResult>(this.BeginExecute), new Func<IAsyncResult, TableResult>(this.EndExecute), operation, cancellationToken);
		}

		// Token: 0x06000AEB RID: 2795 RVA: 0x00026AFF File Offset: 0x00024CFF
		[DoesServiceRequest]
		public Task<TableResult> ExecuteAsync(TableOperation operation, TableRequestOptions requestOptions, OperationContext operationContext)
		{
			return this.ExecuteAsync(operation, requestOptions, operationContext, CancellationToken.None);
		}

		// Token: 0x06000AEC RID: 2796 RVA: 0x00026B0F File Offset: 0x00024D0F
		[DoesServiceRequest]
		public Task<TableResult> ExecuteAsync(TableOperation operation, TableRequestOptions requestOptions, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<TableOperation, TableRequestOptions, OperationContext, TableResult>(new Func<TableOperation, TableRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginExecute), new Func<IAsyncResult, TableResult>(this.EndExecute), operation, requestOptions, operationContext, cancellationToken);
		}

		// Token: 0x06000AED RID: 2797 RVA: 0x00026B33 File Offset: 0x00024D33
		[DoesServiceRequest]
		public IList<TableResult> ExecuteBatch(TableBatchOperation batch, TableRequestOptions requestOptions = null, OperationContext operationContext = null)
		{
			CommonUtility.AssertNotNull("batch", batch);
			return batch.Execute(this.ServiceClient, this, requestOptions, operationContext);
		}

		// Token: 0x06000AEE RID: 2798 RVA: 0x00026B4F File Offset: 0x00024D4F
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginExecuteBatch(TableBatchOperation batch, AsyncCallback callback, object state)
		{
			CommonUtility.AssertNotNull("batch", batch);
			return this.BeginExecuteBatch(batch, null, null, callback, state);
		}

		// Token: 0x06000AEF RID: 2799 RVA: 0x00026B67 File Offset: 0x00024D67
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginExecuteBatch(TableBatchOperation batch, TableRequestOptions requestOptions, OperationContext operationContext, AsyncCallback callback, object state)
		{
			CommonUtility.AssertNotNull("batch", batch);
			return batch.BeginExecute(this.ServiceClient, this, requestOptions, operationContext, callback, state);
		}

		// Token: 0x06000AF0 RID: 2800 RVA: 0x00026B87 File Offset: 0x00024D87
		public IList<TableResult> EndExecuteBatch(IAsyncResult asyncResult)
		{
			return Executor.EndExecuteAsync<IList<TableResult>>(asyncResult);
		}

		// Token: 0x06000AF1 RID: 2801 RVA: 0x00026B8F File Offset: 0x00024D8F
		[DoesServiceRequest]
		public Task<IList<TableResult>> ExecuteBatchAsync(TableBatchOperation batch)
		{
			return this.ExecuteBatchAsync(batch, CancellationToken.None);
		}

		// Token: 0x06000AF2 RID: 2802 RVA: 0x00026B9D File Offset: 0x00024D9D
		[DoesServiceRequest]
		public Task<IList<TableResult>> ExecuteBatchAsync(TableBatchOperation batch, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<TableBatchOperation, IList<TableResult>>(new Func<TableBatchOperation, AsyncCallback, object, ICancellableAsyncResult>(this.BeginExecuteBatch), new Func<IAsyncResult, IList<TableResult>>(this.EndExecuteBatch), batch, cancellationToken);
		}

		// Token: 0x06000AF3 RID: 2803 RVA: 0x00026BBE File Offset: 0x00024DBE
		[DoesServiceRequest]
		public Task<IList<TableResult>> ExecuteBatchAsync(TableBatchOperation batch, TableRequestOptions requestOptions, OperationContext operationContext)
		{
			return this.ExecuteBatchAsync(batch, requestOptions, operationContext, CancellationToken.None);
		}

		// Token: 0x06000AF4 RID: 2804 RVA: 0x00026BCE File Offset: 0x00024DCE
		[DoesServiceRequest]
		public Task<IList<TableResult>> ExecuteBatchAsync(TableBatchOperation batch, TableRequestOptions requestOptions, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<TableBatchOperation, TableRequestOptions, OperationContext, IList<TableResult>>(new Func<TableBatchOperation, TableRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginExecuteBatch), new Func<IAsyncResult, IList<TableResult>>(this.EndExecuteBatch), batch, requestOptions, operationContext, cancellationToken);
		}

		// Token: 0x06000AF5 RID: 2805 RVA: 0x00026BF2 File Offset: 0x00024DF2
		[DoesServiceRequest]
		public IEnumerable<DynamicTableEntity> ExecuteQuery(TableQuery query, TableRequestOptions requestOptions = null, OperationContext operationContext = null)
		{
			CommonUtility.AssertNotNull("query", query);
			return query.Execute(this.ServiceClient, this, requestOptions, operationContext);
		}

		// Token: 0x06000AF6 RID: 2806 RVA: 0x00026C0E File Offset: 0x00024E0E
		[DoesServiceRequest]
		public TableQuerySegment<DynamicTableEntity> ExecuteQuerySegmented(TableQuery query, TableContinuationToken token, TableRequestOptions requestOptions = null, OperationContext operationContext = null)
		{
			CommonUtility.AssertNotNull("query", query);
			return query.ExecuteQuerySegmented(token, this.ServiceClient, this, requestOptions, operationContext);
		}

		// Token: 0x06000AF7 RID: 2807 RVA: 0x00026C2C File Offset: 0x00024E2C
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginExecuteQuerySegmented(TableQuery query, TableContinuationToken token, AsyncCallback callback, object state)
		{
			CommonUtility.AssertNotNull("query", query);
			return this.BeginExecuteQuerySegmented(query, token, null, null, callback, state);
		}

		// Token: 0x06000AF8 RID: 2808 RVA: 0x00026C46 File Offset: 0x00024E46
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginExecuteQuerySegmented(TableQuery query, TableContinuationToken token, TableRequestOptions requestOptions, OperationContext operationContext, AsyncCallback callback, object state)
		{
			CommonUtility.AssertNotNull("query", query);
			return query.BeginExecuteQuerySegmented(token, this.ServiceClient, this, requestOptions, operationContext, callback, state);
		}

		// Token: 0x06000AF9 RID: 2809 RVA: 0x00026C68 File Offset: 0x00024E68
		public TableQuerySegment<DynamicTableEntity> EndExecuteQuerySegmented(IAsyncResult asyncResult)
		{
			return Executor.EndExecuteAsync<TableQuerySegment<DynamicTableEntity>>(asyncResult);
		}

		// Token: 0x06000AFA RID: 2810 RVA: 0x00026C70 File Offset: 0x00024E70
		[DoesServiceRequest]
		public Task<TableQuerySegment<DynamicTableEntity>> ExecuteQuerySegmentedAsync(TableQuery query, TableContinuationToken token)
		{
			return this.ExecuteQuerySegmentedAsync(query, token, CancellationToken.None);
		}

		// Token: 0x06000AFB RID: 2811 RVA: 0x00026C7F File Offset: 0x00024E7F
		[DoesServiceRequest]
		public Task<TableQuerySegment<DynamicTableEntity>> ExecuteQuerySegmentedAsync(TableQuery query, TableContinuationToken token, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<TableQuery, TableContinuationToken, TableQuerySegment<DynamicTableEntity>>(new Func<TableQuery, TableContinuationToken, AsyncCallback, object, ICancellableAsyncResult>(this.BeginExecuteQuerySegmented), new Func<IAsyncResult, TableQuerySegment<DynamicTableEntity>>(this.EndExecuteQuerySegmented), query, token, cancellationToken);
		}

		// Token: 0x06000AFC RID: 2812 RVA: 0x00026CA1 File Offset: 0x00024EA1
		[DoesServiceRequest]
		public Task<TableQuerySegment<DynamicTableEntity>> ExecuteQuerySegmentedAsync(TableQuery query, TableContinuationToken token, TableRequestOptions requestOptions, OperationContext operationContext)
		{
			return this.ExecuteQuerySegmentedAsync(query, token, requestOptions, operationContext, CancellationToken.None);
		}

		// Token: 0x06000AFD RID: 2813 RVA: 0x00026CB3 File Offset: 0x00024EB3
		[DoesServiceRequest]
		public Task<TableQuerySegment<DynamicTableEntity>> ExecuteQuerySegmentedAsync(TableQuery query, TableContinuationToken token, TableRequestOptions requestOptions, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<TableQuery, TableContinuationToken, TableRequestOptions, OperationContext, TableQuerySegment<DynamicTableEntity>>(new Func<TableQuery, TableContinuationToken, TableRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginExecuteQuerySegmented), new Func<IAsyncResult, TableQuerySegment<DynamicTableEntity>>(this.EndExecuteQuerySegmented), query, token, requestOptions, operationContext, cancellationToken);
		}

		// Token: 0x06000AFE RID: 2814 RVA: 0x00026CD9 File Offset: 0x00024ED9
		[DoesServiceRequest]
		public IEnumerable<TResult> ExecuteQuery<TResult>(TableQuery query, EntityResolver<TResult> resolver, TableRequestOptions requestOptions = null, OperationContext operationContext = null)
		{
			CommonUtility.AssertNotNull("query", query);
			CommonUtility.AssertNotNull("resolver", resolver);
			return query.Execute<TResult>(this.ServiceClient, this, resolver, requestOptions, operationContext);
		}

		// Token: 0x06000AFF RID: 2815 RVA: 0x00026D02 File Offset: 0x00024F02
		[DoesServiceRequest]
		public TableQuerySegment<TResult> ExecuteQuerySegmented<TResult>(TableQuery query, EntityResolver<TResult> resolver, TableContinuationToken token, TableRequestOptions requestOptions = null, OperationContext operationContext = null)
		{
			CommonUtility.AssertNotNull("query", query);
			return query.ExecuteQuerySegmented<TResult>(token, this.ServiceClient, this, resolver, requestOptions, operationContext);
		}

		// Token: 0x06000B00 RID: 2816 RVA: 0x00026D22 File Offset: 0x00024F22
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginExecuteQuerySegmented<TResult>(TableQuery query, EntityResolver<TResult> resolver, TableContinuationToken token, AsyncCallback callback, object state)
		{
			return this.BeginExecuteQuerySegmented<TResult>(query, resolver, token, null, null, callback, state);
		}

		// Token: 0x06000B01 RID: 2817 RVA: 0x00026D34 File Offset: 0x00024F34
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginExecuteQuerySegmented<TResult>(TableQuery query, EntityResolver<TResult> resolver, TableContinuationToken token, TableRequestOptions requestOptions, OperationContext operationContext, AsyncCallback callback, object state)
		{
			CommonUtility.AssertNotNull("query", query);
			CommonUtility.AssertNotNull("resolver", resolver);
			return query.BeginExecuteQuerySegmented<TResult>(token, this.ServiceClient, this, resolver, requestOptions, operationContext, callback, state);
		}

		// Token: 0x06000B02 RID: 2818 RVA: 0x00026D6E File Offset: 0x00024F6E
		[DoesServiceRequest]
		public Task<TableQuerySegment<TResult>> ExecuteQuerySegmentedAsync<TResult>(TableQuery query, EntityResolver<TResult> resolver, TableContinuationToken token)
		{
			return this.ExecuteQuerySegmentedAsync<TResult>(query, resolver, token, CancellationToken.None);
		}

		// Token: 0x06000B03 RID: 2819 RVA: 0x00026D7E File Offset: 0x00024F7E
		[DoesServiceRequest]
		public Task<TableQuerySegment<TResult>> ExecuteQuerySegmentedAsync<TResult>(TableQuery query, EntityResolver<TResult> resolver, TableContinuationToken token, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<TableQuery, EntityResolver<TResult>, TableContinuationToken, TableQuerySegment<TResult>>(new Func<TableQuery, EntityResolver<TResult>, TableContinuationToken, AsyncCallback, object, ICancellableAsyncResult>(this.BeginExecuteQuerySegmented<TResult>), new Func<IAsyncResult, TableQuerySegment<TResult>>(this.EndExecuteQuerySegmented<TResult>), query, resolver, token, cancellationToken);
		}

		// Token: 0x06000B04 RID: 2820 RVA: 0x00026DA2 File Offset: 0x00024FA2
		[DoesServiceRequest]
		public Task<TableQuerySegment<TResult>> ExecuteQuerySegmentedAsync<TResult>(TableQuery query, EntityResolver<TResult> resolver, TableContinuationToken token, TableRequestOptions requestOptions, OperationContext operationContext)
		{
			return this.ExecuteQuerySegmentedAsync<TResult>(query, resolver, token, requestOptions, operationContext, CancellationToken.None);
		}

		// Token: 0x06000B05 RID: 2821 RVA: 0x00026DB6 File Offset: 0x00024FB6
		[DoesServiceRequest]
		public Task<TableQuerySegment<TResult>> ExecuteQuerySegmentedAsync<TResult>(TableQuery query, EntityResolver<TResult> resolver, TableContinuationToken token, TableRequestOptions requestOptions, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<TableQuery, EntityResolver<TResult>, TableContinuationToken, TableRequestOptions, OperationContext, TableQuerySegment<TResult>>(new Func<TableQuery, EntityResolver<TResult>, TableContinuationToken, TableRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginExecuteQuerySegmented<TResult>), new Func<IAsyncResult, TableQuerySegment<TResult>>(this.EndExecuteQuerySegmented<TResult>), query, resolver, token, requestOptions, operationContext, cancellationToken);
		}

		// Token: 0x06000B06 RID: 2822 RVA: 0x00026DDE File Offset: 0x00024FDE
		public TableQuery<TElement> CreateQuery<TElement>() where TElement : ITableEntity, new()
		{
			return new TableQuery<TElement>(this);
		}

		// Token: 0x06000B07 RID: 2823 RVA: 0x00026DE6 File Offset: 0x00024FE6
		[DoesServiceRequest]
		public IEnumerable<TElement> ExecuteQuery<TElement>(TableQuery<TElement> query, TableRequestOptions requestOptions = null, OperationContext operationContext = null) where TElement : ITableEntity, new()
		{
			CommonUtility.AssertNotNull("query", query);
			if (query.Provider != null)
			{
				return query.Execute(requestOptions, operationContext);
			}
			return query.ExecuteInternal(this.ServiceClient, this, requestOptions, operationContext);
		}

		// Token: 0x06000B08 RID: 2824 RVA: 0x00026E13 File Offset: 0x00025013
		[DoesServiceRequest]
		public TableQuerySegment<TElement> ExecuteQuerySegmented<TElement>(TableQuery<TElement> query, TableContinuationToken token, TableRequestOptions requestOptions = null, OperationContext operationContext = null) where TElement : ITableEntity, new()
		{
			CommonUtility.AssertNotNull("query", query);
			if (query.Provider != null)
			{
				return query.ExecuteSegmented(token, requestOptions, operationContext);
			}
			return query.ExecuteQuerySegmentedInternal(token, this.ServiceClient, this, requestOptions, operationContext);
		}

		// Token: 0x06000B09 RID: 2825 RVA: 0x00026E44 File Offset: 0x00025044
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginExecuteQuerySegmented<TElement>(TableQuery<TElement> query, TableContinuationToken token, AsyncCallback callback, object state) where TElement : ITableEntity, new()
		{
			CommonUtility.AssertNotNull("query", query);
			return this.BeginExecuteQuerySegmented<TElement>(query, token, null, null, callback, state);
		}

		// Token: 0x06000B0A RID: 2826 RVA: 0x00026E5E File Offset: 0x0002505E
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginExecuteQuerySegmented<TElement>(TableQuery<TElement> query, TableContinuationToken token, TableRequestOptions requestOptions, OperationContext operationContext, AsyncCallback callback, object state) where TElement : ITableEntity, new()
		{
			CommonUtility.AssertNotNull("query", query);
			if (query.Provider != null)
			{
				return query.BeginExecuteSegmented(token, requestOptions, operationContext, callback, state);
			}
			return query.BeginExecuteQuerySegmentedInternal(token, this.ServiceClient, this, requestOptions, operationContext, callback, state);
		}

		// Token: 0x06000B0B RID: 2827 RVA: 0x00026E97 File Offset: 0x00025097
		public TableQuerySegment<TResult> EndExecuteQuerySegmented<TResult>(IAsyncResult asyncResult)
		{
			return Executor.EndExecuteAsync<TableQuerySegment<TResult>>(asyncResult);
		}

		// Token: 0x06000B0C RID: 2828 RVA: 0x00026E9F File Offset: 0x0002509F
		[DoesServiceRequest]
		public Task<TableQuerySegment<TElement>> ExecuteQuerySegmentedAsync<TElement>(TableQuery<TElement> query, TableContinuationToken token) where TElement : ITableEntity, new()
		{
			return this.ExecuteQuerySegmentedAsync<TElement>(query, token, CancellationToken.None);
		}

		// Token: 0x06000B0D RID: 2829 RVA: 0x00026EAE File Offset: 0x000250AE
		[DoesServiceRequest]
		public Task<TableQuerySegment<TElement>> ExecuteQuerySegmentedAsync<TElement>(TableQuery<TElement> query, TableContinuationToken token, CancellationToken cancellationToken) where TElement : ITableEntity, new()
		{
			return AsyncExtensions.TaskFromApm<TableQuery<TElement>, TableContinuationToken, TableQuerySegment<TElement>>(new Func<TableQuery<TElement>, TableContinuationToken, AsyncCallback, object, ICancellableAsyncResult>(this.BeginExecuteQuerySegmented<TElement>), new Func<IAsyncResult, TableQuerySegment<TElement>>(this.EndExecuteQuerySegmented<TElement>), query, token, cancellationToken);
		}

		// Token: 0x06000B0E RID: 2830 RVA: 0x00026ED0 File Offset: 0x000250D0
		[DoesServiceRequest]
		public Task<TableQuerySegment<TElement>> ExecuteQuerySegmentedAsync<TElement>(TableQuery<TElement> query, TableContinuationToken token, TableRequestOptions requestOptions, OperationContext operationContext) where TElement : ITableEntity, new()
		{
			return this.ExecuteQuerySegmentedAsync<TElement>(query, token, requestOptions, operationContext, CancellationToken.None);
		}

		// Token: 0x06000B0F RID: 2831 RVA: 0x00026EE2 File Offset: 0x000250E2
		[DoesServiceRequest]
		public Task<TableQuerySegment<TElement>> ExecuteQuerySegmentedAsync<TElement>(TableQuery<TElement> query, TableContinuationToken token, TableRequestOptions requestOptions, OperationContext operationContext, CancellationToken cancellationToken) where TElement : ITableEntity, new()
		{
			return AsyncExtensions.TaskFromApm<TableQuery<TElement>, TableContinuationToken, TableRequestOptions, OperationContext, TableQuerySegment<TElement>>(new Func<TableQuery<TElement>, TableContinuationToken, TableRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginExecuteQuerySegmented<TElement>), new Func<IAsyncResult, TableQuerySegment<TElement>>(this.EndExecuteQuerySegmented<TElement>), query, token, requestOptions, operationContext, cancellationToken);
		}

		// Token: 0x06000B10 RID: 2832 RVA: 0x00026F08 File Offset: 0x00025108
		[DoesServiceRequest]
		public IEnumerable<TResult> ExecuteQuery<TElement, TResult>(TableQuery<TElement> query, EntityResolver<TResult> resolver, TableRequestOptions requestOptions = null, OperationContext operationContext = null) where TElement : ITableEntity, new()
		{
			CommonUtility.AssertNotNull("query", query);
			CommonUtility.AssertNotNull("resolver", resolver);
			if (query.Provider != null)
			{
				return query.Resolve(resolver).Execute(requestOptions, operationContext);
			}
			return query.ExecuteInternal<TResult>(this.ServiceClient, this, resolver, requestOptions, operationContext);
		}

		// Token: 0x06000B11 RID: 2833 RVA: 0x00026F54 File Offset: 0x00025154
		[DoesServiceRequest]
		public TableQuerySegment<TResult> ExecuteQuerySegmented<TElement, TResult>(TableQuery<TElement> query, EntityResolver<TResult> resolver, TableContinuationToken token, TableRequestOptions requestOptions = null, OperationContext operationContext = null) where TElement : ITableEntity, new()
		{
			CommonUtility.AssertNotNull("query", query);
			CommonUtility.AssertNotNull("resolver", resolver);
			if (query.Provider != null)
			{
				return query.Resolve(resolver).ExecuteSegmented(token, requestOptions, operationContext);
			}
			return query.ExecuteQuerySegmentedInternal<TResult>(token, this.ServiceClient, this, resolver, requestOptions, operationContext);
		}

		// Token: 0x06000B12 RID: 2834 RVA: 0x00026FA4 File Offset: 0x000251A4
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginExecuteQuerySegmented<TElement, TResult>(TableQuery<TElement> query, EntityResolver<TResult> resolver, TableContinuationToken token, AsyncCallback callback, object state) where TElement : ITableEntity, new()
		{
			return this.BeginExecuteQuerySegmented<TElement, TResult>(query, resolver, token, null, null, callback, state);
		}

		// Token: 0x06000B13 RID: 2835 RVA: 0x00026FB8 File Offset: 0x000251B8
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginExecuteQuerySegmented<TElement, TResult>(TableQuery<TElement> query, EntityResolver<TResult> resolver, TableContinuationToken token, TableRequestOptions requestOptions, OperationContext operationContext, AsyncCallback callback, object state) where TElement : ITableEntity, new()
		{
			CommonUtility.AssertNotNull("query", query);
			CommonUtility.AssertNotNull("resolver", resolver);
			if (query.Provider != null)
			{
				return query.Resolve(resolver).BeginExecuteSegmented(token, requestOptions, operationContext, callback, state);
			}
			return query.BeginExecuteQuerySegmentedInternal<TResult>(token, this.ServiceClient, this, resolver, requestOptions, operationContext, callback, state);
		}

		// Token: 0x06000B14 RID: 2836 RVA: 0x00027010 File Offset: 0x00025210
		public TableQuerySegment<TResult> EndExecuteQuerySegmented<TElement, TResult>(IAsyncResult asyncResult) where TElement : ITableEntity, new()
		{
			return Executor.EndExecuteAsync<TableQuerySegment<TResult>>(asyncResult);
		}

		// Token: 0x06000B15 RID: 2837 RVA: 0x00027018 File Offset: 0x00025218
		[DoesServiceRequest]
		public Task<TableQuerySegment<TResult>> ExecuteQuerySegmentedAsync<TElement, TResult>(TableQuery<TElement> query, EntityResolver<TResult> resolver, TableContinuationToken token) where TElement : ITableEntity, new()
		{
			return this.ExecuteQuerySegmentedAsync<TElement, TResult>(query, resolver, token, CancellationToken.None);
		}

		// Token: 0x06000B16 RID: 2838 RVA: 0x00027028 File Offset: 0x00025228
		[DoesServiceRequest]
		public Task<TableQuerySegment<TResult>> ExecuteQuerySegmentedAsync<TElement, TResult>(TableQuery<TElement> query, EntityResolver<TResult> resolver, TableContinuationToken token, CancellationToken cancellationToken) where TElement : ITableEntity, new()
		{
			return AsyncExtensions.TaskFromApm<TableQuery<TElement>, EntityResolver<TResult>, TableContinuationToken, TableQuerySegment<TResult>>(new Func<TableQuery<TElement>, EntityResolver<TResult>, TableContinuationToken, AsyncCallback, object, ICancellableAsyncResult>(this.BeginExecuteQuerySegmented<TElement, TResult>), new Func<IAsyncResult, TableQuerySegment<TResult>>(this.EndExecuteQuerySegmented<TElement, TResult>), query, resolver, token, cancellationToken);
		}

		// Token: 0x06000B17 RID: 2839 RVA: 0x0002704C File Offset: 0x0002524C
		[DoesServiceRequest]
		public Task<TableQuerySegment<TResult>> ExecuteQuerySegmentedAsync<TElement, TResult>(TableQuery<TElement> query, EntityResolver<TResult> resolver, TableContinuationToken token, TableRequestOptions requestOptions, OperationContext operationContext) where TElement : ITableEntity, new()
		{
			return this.ExecuteQuerySegmentedAsync<TElement, TResult>(query, resolver, token, requestOptions, operationContext, CancellationToken.None);
		}

		// Token: 0x06000B18 RID: 2840 RVA: 0x00027060 File Offset: 0x00025260
		[DoesServiceRequest]
		public Task<TableQuerySegment<TResult>> ExecuteQuerySegmentedAsync<TElement, TResult>(TableQuery<TElement> query, EntityResolver<TResult> resolver, TableContinuationToken token, TableRequestOptions requestOptions, OperationContext operationContext, CancellationToken cancellationToken) where TElement : ITableEntity, new()
		{
			return AsyncExtensions.TaskFromApm<TableQuery<TElement>, EntityResolver<TResult>, TableContinuationToken, TableRequestOptions, OperationContext, TableQuerySegment<TResult>>(new Func<TableQuery<TElement>, EntityResolver<TResult>, TableContinuationToken, TableRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginExecuteQuerySegmented<TElement, TResult>), new Func<IAsyncResult, TableQuerySegment<TResult>>(this.EndExecuteQuerySegmented<TElement, TResult>), query, resolver, token, requestOptions, operationContext, cancellationToken);
		}

		// Token: 0x06000B19 RID: 2841 RVA: 0x00027088 File Offset: 0x00025288
		[DoesServiceRequest]
		public void Create(TableRequestOptions requestOptions = null, OperationContext operationContext = null)
		{
			requestOptions = TableRequestOptions.ApplyDefaults(requestOptions, this.ServiceClient);
			operationContext = (operationContext ?? new OperationContext());
			TableOperation tableOperation = new TableOperation(new DynamicTableEntity
			{
				Properties = 
				{
					{
						"TableName",
						new EntityProperty(this.Name)
					}
				}
			}, TableOperationType.Insert);
			tableOperation.IsTableEntity = true;
			CloudTable tableReference = this.ServiceClient.GetTableReference("Tables");
			tableOperation.Execute(this.ServiceClient, tableReference, requestOptions, operationContext);
		}

		// Token: 0x06000B1A RID: 2842 RVA: 0x00027100 File Offset: 0x00025300
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginCreate(AsyncCallback callback, object state)
		{
			return this.BeginCreate(null, null, callback, state);
		}

		// Token: 0x06000B1B RID: 2843 RVA: 0x0002710C File Offset: 0x0002530C
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginCreate(TableRequestOptions requestOptions, OperationContext operationContext, AsyncCallback callback, object state)
		{
			requestOptions = TableRequestOptions.ApplyDefaults(requestOptions, this.ServiceClient);
			operationContext = (operationContext ?? new OperationContext());
			TableOperation tableOperation = new TableOperation(new DynamicTableEntity
			{
				Properties = 
				{
					{
						"TableName",
						new EntityProperty(this.Name)
					}
				}
			}, TableOperationType.Insert);
			tableOperation.IsTableEntity = true;
			CloudTable tableReference = this.ServiceClient.GetTableReference("Tables");
			return tableOperation.BeginExecute(this.ServiceClient, tableReference, requestOptions, operationContext, callback, state);
		}

		// Token: 0x06000B1C RID: 2844 RVA: 0x00027186 File Offset: 0x00025386
		public void EndCreate(IAsyncResult asyncResult)
		{
			Executor.EndExecuteAsync<TableResult>(asyncResult);
		}

		// Token: 0x06000B1D RID: 2845 RVA: 0x0002718F File Offset: 0x0002538F
		[DoesServiceRequest]
		public Task CreateAsync()
		{
			return this.CreateAsync(CancellationToken.None);
		}

		// Token: 0x06000B1E RID: 2846 RVA: 0x0002719C File Offset: 0x0002539C
		[DoesServiceRequest]
		public Task CreateAsync(CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm(new Func<AsyncCallback, object, ICancellableAsyncResult>(this.BeginCreate), new Action<IAsyncResult>(this.EndCreate), cancellationToken);
		}

		// Token: 0x06000B1F RID: 2847 RVA: 0x000271BC File Offset: 0x000253BC
		[DoesServiceRequest]
		public Task CreateAsync(TableRequestOptions requestOptions, OperationContext operationContext)
		{
			return this.CreateAsync(requestOptions, operationContext, CancellationToken.None);
		}

		// Token: 0x06000B20 RID: 2848 RVA: 0x000271CB File Offset: 0x000253CB
		[DoesServiceRequest]
		public Task CreateAsync(TableRequestOptions requestOptions, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<TableRequestOptions, OperationContext>(new Func<TableRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginCreate), new Action<IAsyncResult>(this.EndCreate), requestOptions, operationContext, cancellationToken);
		}

		// Token: 0x06000B21 RID: 2849 RVA: 0x000271F0 File Offset: 0x000253F0
		[DoesServiceRequest]
		public bool CreateIfNotExists(TableRequestOptions requestOptions = null, OperationContext operationContext = null)
		{
			TableRequestOptions requestOptions2 = TableRequestOptions.ApplyDefaults(requestOptions, this.ServiceClient);
			operationContext = (operationContext ?? new OperationContext());
			if (this.Exists(true, requestOptions2, operationContext))
			{
				return false;
			}
			bool result;
			try
			{
				this.Create(requestOptions2, operationContext);
				result = true;
			}
			catch (StorageException ex)
			{
				if (ex.RequestInformation.HttpStatusCode != 409)
				{
					throw;
				}
				if (ex.RequestInformation.ExtendedErrorInformation != null && !(ex.RequestInformation.ExtendedErrorInformation.ErrorCode == TableErrorCodeStrings.TableAlreadyExists))
				{
					throw;
				}
				result = false;
			}
			return result;
		}

		// Token: 0x06000B22 RID: 2850 RVA: 0x00027284 File Offset: 0x00025484
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginCreateIfNotExists(AsyncCallback callback, object state)
		{
			return this.BeginCreateIfNotExists(null, null, callback, state);
		}

		// Token: 0x06000B23 RID: 2851 RVA: 0x00027290 File Offset: 0x00025490
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginCreateIfNotExists(TableRequestOptions requestOptions, OperationContext operationContext, AsyncCallback callback, object state)
		{
			TableRequestOptions requestOptions2 = TableRequestOptions.ApplyDefaults(requestOptions, this.ServiceClient);
			operationContext = (operationContext ?? new OperationContext());
			StorageAsyncResult<bool> storageAsyncResult = new StorageAsyncResult<bool>(callback, state)
			{
				RequestOptions = requestOptions2,
				OperationContext = operationContext
			};
			ICancellableAsyncResult @object = this.BeginExists(true, requestOptions2, operationContext, new AsyncCallback(this.CreateIfNotExistHandler), storageAsyncResult);
			storageAsyncResult.CancelDelegate = new Action(@object.Cancel);
			return storageAsyncResult;
		}

		// Token: 0x06000B24 RID: 2852 RVA: 0x000273F8 File Offset: 0x000255F8
		private void CreateIfNotExistHandler(IAsyncResult asyncResult)
		{
			StorageAsyncResult<bool> storageAsyncResult = asyncResult.AsyncState as StorageAsyncResult<bool>;
			lock (storageAsyncResult.CancellationLockerObject)
			{
				storageAsyncResult.CancelDelegate = null;
				storageAsyncResult.UpdateCompletedSynchronously(asyncResult.CompletedSynchronously);
				try
				{
					bool flag2 = this.EndExists(asyncResult);
					if (flag2)
					{
						storageAsyncResult.Result = false;
						storageAsyncResult.OnComplete();
					}
					else
					{
						ICancellableAsyncResult @object = this.BeginCreate((TableRequestOptions)storageAsyncResult.RequestOptions, storageAsyncResult.OperationContext, delegate(IAsyncResult createRes)
						{
							storageAsyncResult.CancelDelegate = null;
							storageAsyncResult.UpdateCompletedSynchronously(storageAsyncResult.CompletedSynchronously);
							try
							{
								this.EndCreate(createRes);
								storageAsyncResult.Result = true;
								storageAsyncResult.OnComplete();
							}
							catch (StorageException ex)
							{
								if (ex.RequestInformation.HttpStatusCode == 409)
								{
									if (ex.RequestInformation.ExtendedErrorInformation == null || ex.RequestInformation.ExtendedErrorInformation.ErrorCode == TableErrorCodeStrings.TableAlreadyExists)
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
						storageAsyncResult.CancelDelegate = new Action(@object.Cancel);
					}
				}
				catch (Exception exception)
				{
					storageAsyncResult.OnComplete(exception);
				}
			}
		}

		// Token: 0x06000B25 RID: 2853 RVA: 0x00027518 File Offset: 0x00025718
		public bool EndCreateIfNotExists(IAsyncResult asyncResult)
		{
			StorageAsyncResult<bool> storageAsyncResult = asyncResult as StorageAsyncResult<bool>;
			CommonUtility.AssertNotNull("AsyncResult", storageAsyncResult);
			storageAsyncResult.End();
			return storageAsyncResult.Result;
		}

		// Token: 0x06000B26 RID: 2854 RVA: 0x00027543 File Offset: 0x00025743
		[DoesServiceRequest]
		public Task<bool> CreateIfNotExistsAsync()
		{
			return this.CreateIfNotExistsAsync(CancellationToken.None);
		}

		// Token: 0x06000B27 RID: 2855 RVA: 0x00027550 File Offset: 0x00025750
		[DoesServiceRequest]
		public Task<bool> CreateIfNotExistsAsync(CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<bool>(new Func<AsyncCallback, object, ICancellableAsyncResult>(this.BeginCreateIfNotExists), new Func<IAsyncResult, bool>(this.EndCreateIfNotExists), cancellationToken);
		}

		// Token: 0x06000B28 RID: 2856 RVA: 0x00027570 File Offset: 0x00025770
		[DoesServiceRequest]
		public Task<bool> CreateIfNotExistsAsync(TableRequestOptions requestOptions, OperationContext operationContext)
		{
			return this.CreateIfNotExistsAsync(requestOptions, operationContext, CancellationToken.None);
		}

		// Token: 0x06000B29 RID: 2857 RVA: 0x0002757F File Offset: 0x0002577F
		[DoesServiceRequest]
		public Task<bool> CreateIfNotExistsAsync(TableRequestOptions requestOptions, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<TableRequestOptions, OperationContext, bool>(new Func<TableRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginCreateIfNotExists), new Func<IAsyncResult, bool>(this.EndCreateIfNotExists), requestOptions, operationContext, cancellationToken);
		}

		// Token: 0x06000B2A RID: 2858 RVA: 0x000275A4 File Offset: 0x000257A4
		[DoesServiceRequest]
		public void Delete(TableRequestOptions requestOptions = null, OperationContext operationContext = null)
		{
			requestOptions = TableRequestOptions.ApplyDefaults(requestOptions, this.ServiceClient);
			operationContext = (operationContext ?? new OperationContext());
			TableOperation tableOperation = new TableOperation(new DynamicTableEntity
			{
				Properties = 
				{
					{
						"TableName",
						new EntityProperty(this.Name)
					}
				}
			}, TableOperationType.Delete);
			tableOperation.IsTableEntity = true;
			CloudTable tableReference = this.ServiceClient.GetTableReference("Tables");
			tableOperation.Execute(this.ServiceClient, tableReference, requestOptions, operationContext);
		}

		// Token: 0x06000B2B RID: 2859 RVA: 0x0002761C File Offset: 0x0002581C
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginDelete(AsyncCallback callback, object state)
		{
			return this.BeginDelete(null, null, callback, state);
		}

		// Token: 0x06000B2C RID: 2860 RVA: 0x00027628 File Offset: 0x00025828
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginDelete(TableRequestOptions requestOptions, OperationContext operationContext, AsyncCallback callback, object state)
		{
			requestOptions = TableRequestOptions.ApplyDefaults(requestOptions, this.ServiceClient);
			operationContext = (operationContext ?? new OperationContext());
			TableOperation tableOperation = new TableOperation(new DynamicTableEntity
			{
				Properties = 
				{
					{
						"TableName",
						new EntityProperty(this.Name)
					}
				}
			}, TableOperationType.Delete);
			tableOperation.IsTableEntity = true;
			CloudTable tableReference = this.ServiceClient.GetTableReference("Tables");
			return tableOperation.BeginExecute(this.ServiceClient, tableReference, requestOptions, operationContext, callback, state);
		}

		// Token: 0x06000B2D RID: 2861 RVA: 0x000276A2 File Offset: 0x000258A2
		public void EndDelete(IAsyncResult asyncResult)
		{
			Executor.EndExecuteAsync<TableResult>(asyncResult);
		}

		// Token: 0x06000B2E RID: 2862 RVA: 0x000276AB File Offset: 0x000258AB
		[DoesServiceRequest]
		public Task DeleteAsync()
		{
			return this.DeleteAsync(CancellationToken.None);
		}

		// Token: 0x06000B2F RID: 2863 RVA: 0x000276B8 File Offset: 0x000258B8
		[DoesServiceRequest]
		public Task DeleteAsync(CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm(new Func<AsyncCallback, object, ICancellableAsyncResult>(this.BeginDelete), new Action<IAsyncResult>(this.EndDelete), cancellationToken);
		}

		// Token: 0x06000B30 RID: 2864 RVA: 0x000276D8 File Offset: 0x000258D8
		[DoesServiceRequest]
		public Task DeleteAsync(TableRequestOptions requestOptions, OperationContext operationContext)
		{
			return this.DeleteAsync(requestOptions, operationContext, CancellationToken.None);
		}

		// Token: 0x06000B31 RID: 2865 RVA: 0x000276E7 File Offset: 0x000258E7
		[DoesServiceRequest]
		public Task DeleteAsync(TableRequestOptions requestOptions, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<TableRequestOptions, OperationContext>(new Func<TableRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginDelete), new Action<IAsyncResult>(this.EndDelete), requestOptions, operationContext, cancellationToken);
		}

		// Token: 0x06000B32 RID: 2866 RVA: 0x0002770C File Offset: 0x0002590C
		[DoesServiceRequest]
		public bool DeleteIfExists(TableRequestOptions requestOptions = null, OperationContext operationContext = null)
		{
			TableRequestOptions requestOptions2 = TableRequestOptions.ApplyDefaults(requestOptions, this.ServiceClient);
			operationContext = (operationContext ?? new OperationContext());
			if (!this.Exists(true, requestOptions2, operationContext))
			{
				return false;
			}
			bool result;
			try
			{
				this.Delete(requestOptions2, operationContext);
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

		// Token: 0x06000B33 RID: 2867 RVA: 0x000277A0 File Offset: 0x000259A0
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginDeleteIfExists(AsyncCallback callback, object state)
		{
			return this.BeginDeleteIfExists(null, null, callback, state);
		}

		// Token: 0x06000B34 RID: 2868 RVA: 0x000277AC File Offset: 0x000259AC
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginDeleteIfExists(TableRequestOptions requestOptions, OperationContext operationContext, AsyncCallback callback, object state)
		{
			TableRequestOptions requestOptions2 = TableRequestOptions.ApplyDefaults(requestOptions, this.ServiceClient);
			operationContext = (operationContext ?? new OperationContext());
			StorageAsyncResult<bool> storageAsyncResult = new StorageAsyncResult<bool>(callback, state)
			{
				RequestOptions = requestOptions2,
				OperationContext = operationContext
			};
			ICancellableAsyncResult @object = this.BeginExists(true, requestOptions2, operationContext, new AsyncCallback(this.DeleteIfExistsHandler), storageAsyncResult);
			storageAsyncResult.CancelDelegate = new Action(@object.Cancel);
			return storageAsyncResult;
		}

		// Token: 0x06000B35 RID: 2869 RVA: 0x00027910 File Offset: 0x00025B10
		private void DeleteIfExistsHandler(IAsyncResult asyncResult)
		{
			StorageAsyncResult<bool> storageAsyncResult = asyncResult.AsyncState as StorageAsyncResult<bool>;
			lock (storageAsyncResult.CancellationLockerObject)
			{
				storageAsyncResult.CancelDelegate = null;
				storageAsyncResult.UpdateCompletedSynchronously(asyncResult.CompletedSynchronously);
				try
				{
					if (!this.EndExists(asyncResult))
					{
						storageAsyncResult.Result = false;
						storageAsyncResult.OnComplete();
					}
					else
					{
						ICancellableAsyncResult @object = this.BeginDelete((TableRequestOptions)storageAsyncResult.RequestOptions, storageAsyncResult.OperationContext, delegate(IAsyncResult deleteRes)
						{
							storageAsyncResult.CancelDelegate = null;
							storageAsyncResult.UpdateCompletedSynchronously(deleteRes.CompletedSynchronously);
							try
							{
								this.EndDelete(deleteRes);
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
						storageAsyncResult.CancelDelegate = new Action(@object.Cancel);
					}
				}
				catch (Exception exception)
				{
					storageAsyncResult.OnComplete(exception);
				}
			}
		}

		// Token: 0x06000B36 RID: 2870 RVA: 0x00027A30 File Offset: 0x00025C30
		public bool EndDeleteIfExists(IAsyncResult asyncResult)
		{
			StorageAsyncResult<bool> storageAsyncResult = asyncResult as StorageAsyncResult<bool>;
			CommonUtility.AssertNotNull("AsyncResult", storageAsyncResult);
			storageAsyncResult.End();
			return storageAsyncResult.Result;
		}

		// Token: 0x06000B37 RID: 2871 RVA: 0x00027A5B File Offset: 0x00025C5B
		[DoesServiceRequest]
		public Task<bool> DeleteIfExistsAsync()
		{
			return this.DeleteIfExistsAsync(CancellationToken.None);
		}

		// Token: 0x06000B38 RID: 2872 RVA: 0x00027A68 File Offset: 0x00025C68
		[DoesServiceRequest]
		public Task<bool> DeleteIfExistsAsync(CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<bool>(new Func<AsyncCallback, object, ICancellableAsyncResult>(this.BeginDeleteIfExists), new Func<IAsyncResult, bool>(this.EndDeleteIfExists), cancellationToken);
		}

		// Token: 0x06000B39 RID: 2873 RVA: 0x00027A88 File Offset: 0x00025C88
		[DoesServiceRequest]
		public Task<bool> DeleteIfExistsAsync(TableRequestOptions requestOptions, OperationContext operationContext)
		{
			return this.DeleteIfExistsAsync(requestOptions, operationContext, CancellationToken.None);
		}

		// Token: 0x06000B3A RID: 2874 RVA: 0x00027A97 File Offset: 0x00025C97
		[DoesServiceRequest]
		public Task<bool> DeleteIfExistsAsync(TableRequestOptions requestOptions, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<TableRequestOptions, OperationContext, bool>(new Func<TableRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginDeleteIfExists), new Func<IAsyncResult, bool>(this.EndDeleteIfExists), requestOptions, operationContext, cancellationToken);
		}

		// Token: 0x06000B3B RID: 2875 RVA: 0x00027AB9 File Offset: 0x00025CB9
		[DoesServiceRequest]
		public bool Exists(TableRequestOptions requestOptions = null, OperationContext operationContext = null)
		{
			return this.Exists(false, requestOptions, operationContext);
		}

		// Token: 0x06000B3C RID: 2876 RVA: 0x00027AC4 File Offset: 0x00025CC4
		private bool Exists(bool primaryOnly, TableRequestOptions requestOptions, OperationContext operationContext)
		{
			requestOptions = TableRequestOptions.ApplyDefaults(requestOptions, this.ServiceClient);
			operationContext = (operationContext ?? new OperationContext());
			TableOperation tableOperation = new TableOperation(new DynamicTableEntity
			{
				Properties = 
				{
					{
						"TableName",
						new EntityProperty(this.Name)
					}
				}
			}, TableOperationType.Retrieve);
			tableOperation.IsTableEntity = true;
			tableOperation.IsPrimaryOnlyRetrieve = primaryOnly;
			CloudTable tableReference = this.ServiceClient.GetTableReference("Tables");
			TableResult tableResult = tableOperation.Execute(this.ServiceClient, tableReference, requestOptions, operationContext);
			return tableResult.HttpStatusCode == 200;
		}

		// Token: 0x06000B3D RID: 2877 RVA: 0x00027B50 File Offset: 0x00025D50
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginExists(AsyncCallback callback, object state)
		{
			return this.BeginExists(null, null, callback, state);
		}

		// Token: 0x06000B3E RID: 2878 RVA: 0x00027B5C File Offset: 0x00025D5C
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginExists(TableRequestOptions requestOptions, OperationContext operationContext, AsyncCallback callback, object state)
		{
			return this.BeginExists(false, requestOptions, operationContext, callback, state);
		}

		// Token: 0x06000B3F RID: 2879 RVA: 0x00027B6C File Offset: 0x00025D6C
		private ICancellableAsyncResult BeginExists(bool primaryOnly, TableRequestOptions requestOptions, OperationContext operationContext, AsyncCallback callback, object state)
		{
			requestOptions = TableRequestOptions.ApplyDefaults(requestOptions, this.ServiceClient);
			operationContext = (operationContext ?? new OperationContext());
			TableOperation tableOperation = new TableOperation(new DynamicTableEntity
			{
				Properties = 
				{
					{
						"TableName",
						new EntityProperty(this.Name)
					}
				}
			}, TableOperationType.Retrieve);
			tableOperation.IsTableEntity = true;
			tableOperation.IsPrimaryOnlyRetrieve = primaryOnly;
			CloudTable tableReference = this.ServiceClient.GetTableReference("Tables");
			return tableOperation.BeginExecute(this.ServiceClient, tableReference, requestOptions, operationContext, callback, state);
		}

		// Token: 0x06000B40 RID: 2880 RVA: 0x00027BF0 File Offset: 0x00025DF0
		public bool EndExists(IAsyncResult asyncResult)
		{
			TableResult tableResult = Executor.EndExecuteAsync<TableResult>(asyncResult);
			return tableResult.HttpStatusCode == 200;
		}

		// Token: 0x06000B41 RID: 2881 RVA: 0x00027C11 File Offset: 0x00025E11
		[DoesServiceRequest]
		public Task<bool> ExistsAsync()
		{
			return this.ExistsAsync(CancellationToken.None);
		}

		// Token: 0x06000B42 RID: 2882 RVA: 0x00027C1E File Offset: 0x00025E1E
		[DoesServiceRequest]
		public Task<bool> ExistsAsync(CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<bool>(new Func<AsyncCallback, object, ICancellableAsyncResult>(this.BeginExists), new Func<IAsyncResult, bool>(this.EndExists), cancellationToken);
		}

		// Token: 0x06000B43 RID: 2883 RVA: 0x00027C3E File Offset: 0x00025E3E
		[DoesServiceRequest]
		public Task<bool> ExistsAsync(TableRequestOptions requestOptions, OperationContext operationContext)
		{
			return this.ExistsAsync(requestOptions, operationContext, CancellationToken.None);
		}

		// Token: 0x06000B44 RID: 2884 RVA: 0x00027C4D File Offset: 0x00025E4D
		[DoesServiceRequest]
		public Task<bool> ExistsAsync(TableRequestOptions requestOptions, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<TableRequestOptions, OperationContext, bool>(new Func<TableRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginExists), new Func<IAsyncResult, bool>(this.EndExists), requestOptions, operationContext, cancellationToken);
		}

		// Token: 0x06000B45 RID: 2885 RVA: 0x00027C6F File Offset: 0x00025E6F
		[DoesServiceRequest]
		public TablePermissions GetPermissions(TableRequestOptions requestOptions = null, OperationContext operationContext = null)
		{
			requestOptions = TableRequestOptions.ApplyDefaults(requestOptions, this.ServiceClient);
			operationContext = (operationContext ?? new OperationContext());
			return Executor.ExecuteSync<TablePermissions>(this.GetAclImpl(requestOptions), requestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x06000B46 RID: 2886 RVA: 0x00027C9E File Offset: 0x00025E9E
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginGetPermissions(AsyncCallback callback, object state)
		{
			return this.BeginGetPermissions(null, null, callback, state);
		}

		// Token: 0x06000B47 RID: 2887 RVA: 0x00027CAA File Offset: 0x00025EAA
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginGetPermissions(TableRequestOptions requestOptions, OperationContext operationContext, AsyncCallback callback, object state)
		{
			requestOptions = TableRequestOptions.ApplyDefaults(requestOptions, this.ServiceClient);
			operationContext = (operationContext ?? new OperationContext());
			return Executor.BeginExecuteAsync<TablePermissions>(this.GetAclImpl(requestOptions), requestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x06000B48 RID: 2888 RVA: 0x00027CDC File Offset: 0x00025EDC
		public TablePermissions EndGetPermissions(IAsyncResult asyncResult)
		{
			return Executor.EndExecuteAsync<TablePermissions>(asyncResult);
		}

		// Token: 0x06000B49 RID: 2889 RVA: 0x00027CE4 File Offset: 0x00025EE4
		[DoesServiceRequest]
		public Task<TablePermissions> GetPermissionsAsync()
		{
			return this.GetPermissionsAsync(CancellationToken.None);
		}

		// Token: 0x06000B4A RID: 2890 RVA: 0x00027CF1 File Offset: 0x00025EF1
		[DoesServiceRequest]
		public Task<TablePermissions> GetPermissionsAsync(CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<TablePermissions>(new Func<AsyncCallback, object, ICancellableAsyncResult>(this.BeginGetPermissions), new Func<IAsyncResult, TablePermissions>(this.EndGetPermissions), cancellationToken);
		}

		// Token: 0x06000B4B RID: 2891 RVA: 0x00027D11 File Offset: 0x00025F11
		[DoesServiceRequest]
		public Task<TablePermissions> GetPermissionsAsync(TableRequestOptions requestOptions, OperationContext operationContext)
		{
			return this.GetPermissionsAsync(requestOptions, operationContext, CancellationToken.None);
		}

		// Token: 0x06000B4C RID: 2892 RVA: 0x00027D20 File Offset: 0x00025F20
		[DoesServiceRequest]
		public Task<TablePermissions> GetPermissionsAsync(TableRequestOptions requestOptions, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<TableRequestOptions, OperationContext, TablePermissions>(new Func<TableRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginGetPermissions), new Func<IAsyncResult, TablePermissions>(this.EndGetPermissions), requestOptions, operationContext, cancellationToken);
		}

		// Token: 0x06000B4D RID: 2893 RVA: 0x00027D54 File Offset: 0x00025F54
		private RESTCommand<TablePermissions> GetAclImpl(TableRequestOptions requestOptions)
		{
			RESTCommand<TablePermissions> restcommand = new RESTCommand<TablePermissions>(this.ServiceClient.Credentials, this.StorageUri);
			restcommand.CommandLocationMode = CommandLocationMode.PrimaryOrSecondary;
			restcommand.BuildRequestDelegate = new Func<Uri, UriQueryBuilder, int?, bool, OperationContext, HttpWebRequest>(TableHttpWebRequestFactory.GetAcl);
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(this.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.ParseError = new Func<Stream, HttpWebResponse, string, StorageExtendedErrorInformation>(StorageExtendedErrorInformation.ReadFromStreamUsingODataLib);
			restcommand.RetrieveResponseStream = true;
			restcommand.PreProcessResponse = ((RESTCommand<TablePermissions> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx) => HttpResponseParsers.ProcessExpectedStatusCodeNoException<TablePermissions>(HttpStatusCode.OK, resp, null, cmd, ex));
			restcommand.PostProcessResponse = new Func<RESTCommand<TablePermissions>, HttpWebResponse, OperationContext, TablePermissions>(this.ParseGetAcl);
			requestOptions.ApplyToStorageCommand<TablePermissions>(restcommand);
			return restcommand;
		}

		// Token: 0x06000B4E RID: 2894 RVA: 0x00027E04 File Offset: 0x00026004
		private TablePermissions ParseGetAcl(RESTCommand<TablePermissions> cmd, HttpWebResponse resp, OperationContext ctx)
		{
			TablePermissions tablePermissions = new TablePermissions();
			TableHttpWebResponseParsers.ReadSharedAccessIdentifiers(cmd.ResponseStream, tablePermissions);
			return tablePermissions;
		}

		// Token: 0x06000B4F RID: 2895 RVA: 0x00027E24 File Offset: 0x00026024
		[DoesServiceRequest]
		public void SetPermissions(TablePermissions permissions, TableRequestOptions requestOptions = null, OperationContext operationContext = null)
		{
			requestOptions = TableRequestOptions.ApplyDefaults(requestOptions, this.ServiceClient);
			operationContext = (operationContext ?? new OperationContext());
			Executor.ExecuteSync<NullType>(this.SetAclImpl(permissions, requestOptions), requestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x06000B50 RID: 2896 RVA: 0x00027E55 File Offset: 0x00026055
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginSetPermissions(TablePermissions permissions, AsyncCallback callback, object state)
		{
			return this.BeginSetPermissions(permissions, null, null, callback, state);
		}

		// Token: 0x06000B51 RID: 2897 RVA: 0x00027E62 File Offset: 0x00026062
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginSetPermissions(TablePermissions permissions, TableRequestOptions requestOptions, OperationContext operationContext, AsyncCallback callback, object state)
		{
			requestOptions = TableRequestOptions.ApplyDefaults(requestOptions, this.ServiceClient);
			operationContext = (operationContext ?? new OperationContext());
			return Executor.BeginExecuteAsync<NullType>(this.SetAclImpl(permissions, requestOptions), requestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x06000B52 RID: 2898 RVA: 0x00027E96 File Offset: 0x00026096
		public void EndSetPermissions(IAsyncResult asyncResult)
		{
			Executor.EndExecuteAsync<NullType>(asyncResult);
		}

		// Token: 0x06000B53 RID: 2899 RVA: 0x00027E9F File Offset: 0x0002609F
		[DoesServiceRequest]
		public Task SetPermissionsAsync(TablePermissions permissions)
		{
			return this.SetPermissionsAsync(permissions, CancellationToken.None);
		}

		// Token: 0x06000B54 RID: 2900 RVA: 0x00027EAD File Offset: 0x000260AD
		[DoesServiceRequest]
		public Task SetPermissionsAsync(TablePermissions permissions, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<TablePermissions>(new Func<TablePermissions, AsyncCallback, object, ICancellableAsyncResult>(this.BeginSetPermissions), new Action<IAsyncResult>(this.EndSetPermissions), permissions, cancellationToken);
		}

		// Token: 0x06000B55 RID: 2901 RVA: 0x00027ECE File Offset: 0x000260CE
		[DoesServiceRequest]
		public Task SetPermissionsAsync(TablePermissions permissions, TableRequestOptions requestOptions, OperationContext operationContext)
		{
			return this.SetPermissionsAsync(permissions, requestOptions, operationContext, CancellationToken.None);
		}

		// Token: 0x06000B56 RID: 2902 RVA: 0x00027EDE File Offset: 0x000260DE
		[DoesServiceRequest]
		public Task SetPermissionsAsync(TablePermissions permissions, TableRequestOptions requestOptions, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<TablePermissions, TableRequestOptions, OperationContext>(new Func<TablePermissions, TableRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginSetPermissions), new Action<IAsyncResult>(this.EndSetPermissions), permissions, requestOptions, operationContext, cancellationToken);
		}

		// Token: 0x06000B57 RID: 2903 RVA: 0x00027F18 File Offset: 0x00026118
		private RESTCommand<NullType> SetAclImpl(TablePermissions permissions, TableRequestOptions requestOptions)
		{
			MultiBufferMemoryStream multiBufferMemoryStream = new MultiBufferMemoryStream(null, 1024);
			TableRequest.WriteSharedAccessIdentifiers(permissions.SharedAccessPolicies, multiBufferMemoryStream);
			multiBufferMemoryStream.Seek(0L, SeekOrigin.Begin);
			RESTCommand<NullType> restcommand = new RESTCommand<NullType>(this.ServiceClient.Credentials, this.StorageUri);
			restcommand.BuildRequestDelegate = new Func<Uri, UriQueryBuilder, int?, bool, OperationContext, HttpWebRequest>(TableHttpWebRequestFactory.SetAcl);
			restcommand.SendStream = multiBufferMemoryStream;
			restcommand.StreamToDispose = multiBufferMemoryStream;
			restcommand.RecoveryAction = new Action<StorageCommandBase<NullType>, Exception, OperationContext>(RecoveryActions.RewindStream<NullType>);
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(this.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.ParseError = new Func<Stream, HttpWebResponse, string, StorageExtendedErrorInformation>(StorageExtendedErrorInformation.ReadFromStreamUsingODataLib);
			restcommand.PreProcessResponse = ((RESTCommand<NullType> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx) => HttpResponseParsers.ProcessExpectedStatusCodeNoException<NullType>(HttpStatusCode.NoContent, resp, NullType.Value, cmd, ex));
			requestOptions.ApplyToStorageCommand<NullType>(restcommand);
			return restcommand;
		}

		// Token: 0x06000B58 RID: 2904 RVA: 0x00027FEA File Offset: 0x000261EA
		public CloudTable(Uri tableAddress) : this(tableAddress, null)
		{
		}

		// Token: 0x06000B59 RID: 2905 RVA: 0x00027FF4 File Offset: 0x000261F4
		public CloudTable(Uri tableAbsoluteUri, StorageCredentials credentials) : this(new StorageUri(tableAbsoluteUri), credentials)
		{
		}

		// Token: 0x06000B5A RID: 2906 RVA: 0x00028003 File Offset: 0x00026203
		public CloudTable(StorageUri tableAddress, StorageCredentials credentials)
		{
			this.ParseQueryAndVerify(tableAddress, credentials);
		}

		// Token: 0x06000B5B RID: 2907 RVA: 0x00028013 File Offset: 0x00026213
		internal CloudTable(string tableName, CloudTableClient client)
		{
			CommonUtility.AssertNotNull("tableName", tableName);
			CommonUtility.AssertNotNull("client", client);
			this.Name = tableName;
			this.StorageUri = NavigationHelper.AppendPathToUri(client.StorageUri, tableName);
			this.ServiceClient = client;
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x06000B5C RID: 2908 RVA: 0x00028051 File Offset: 0x00026251
		// (set) Token: 0x06000B5D RID: 2909 RVA: 0x00028059 File Offset: 0x00026259
		public CloudTableClient ServiceClient { get; private set; }

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x06000B5E RID: 2910 RVA: 0x00028062 File Offset: 0x00026262
		// (set) Token: 0x06000B5F RID: 2911 RVA: 0x0002806A File Offset: 0x0002626A
		public string Name { get; private set; }

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x06000B60 RID: 2912 RVA: 0x00028073 File Offset: 0x00026273
		public Uri Uri
		{
			get
			{
				return this.StorageUri.PrimaryUri;
			}
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x06000B61 RID: 2913 RVA: 0x00028080 File Offset: 0x00026280
		// (set) Token: 0x06000B62 RID: 2914 RVA: 0x00028088 File Offset: 0x00026288
		public StorageUri StorageUri { get; private set; }

		// Token: 0x06000B63 RID: 2915 RVA: 0x00028091 File Offset: 0x00026291
		public string GetSharedAccessSignature(SharedAccessTablePolicy policy)
		{
			return this.GetSharedAccessSignature(policy, null, null, null, null, null);
		}

		// Token: 0x06000B64 RID: 2916 RVA: 0x0002809F File Offset: 0x0002629F
		public string GetSharedAccessSignature(SharedAccessTablePolicy policy, string accessPolicyIdentifier)
		{
			return this.GetSharedAccessSignature(policy, accessPolicyIdentifier, null, null, null, null);
		}

		// Token: 0x06000B65 RID: 2917 RVA: 0x000280B0 File Offset: 0x000262B0
		public string GetSharedAccessSignature(SharedAccessTablePolicy policy, string accessPolicyIdentifier, string startPartitionKey, string startRowKey, string endPartitionKey, string endRowKey)
		{
			if (!this.ServiceClient.Credentials.IsSharedKey)
			{
				string message = string.Format(CultureInfo.CurrentCulture, "Cannot create Shared Access Signature unless Account Key credentials are used.", new object[0]);
				throw new InvalidOperationException(message);
			}
			string canonicalName = this.GetCanonicalName("2015-02-21");
			StorageAccountKey key = this.ServiceClient.Credentials.Key;
			string hash = SharedAccessSignatureHelper.GetHash(policy, accessPolicyIdentifier, startPartitionKey, startRowKey, endPartitionKey, endRowKey, canonicalName, "2015-02-21", key.KeyValue);
			UriQueryBuilder signature = SharedAccessSignatureHelper.GetSignature(policy, this.Name, accessPolicyIdentifier, startPartitionKey, startRowKey, endPartitionKey, endRowKey, hash, key.KeyName, "2015-02-21");
			return signature.ToString();
		}

		// Token: 0x06000B66 RID: 2918 RVA: 0x00028150 File Offset: 0x00026350
		[Obsolete("This overload has been deprecated because the SAS tokens generated using the current version work fine with old libraries. Please use the other overloads.")]
		public string GetSharedAccessSignature(SharedAccessTablePolicy policy, string accessPolicyIdentifier, string startPartitionKey, string startRowKey, string endPartitionKey, string endRowKey, string sasVersion)
		{
			if (!this.ServiceClient.Credentials.IsSharedKey)
			{
				string message = string.Format(CultureInfo.CurrentCulture, "Cannot create Shared Access Signature unless Account Key credentials are used.", new object[0]);
				throw new InvalidOperationException(message);
			}
			string sasVersion2 = SharedAccessSignatureHelper.ValidateSASVersionString(sasVersion);
			string canonicalName = this.GetCanonicalName(sasVersion2);
			StorageAccountKey key = this.ServiceClient.Credentials.Key;
			string hash = SharedAccessSignatureHelper.GetHash(policy, accessPolicyIdentifier, startPartitionKey, startRowKey, endPartitionKey, endRowKey, canonicalName, sasVersion2, key.KeyValue);
			UriQueryBuilder signature = SharedAccessSignatureHelper.GetSignature(policy, this.Name, accessPolicyIdentifier, startPartitionKey, startRowKey, endPartitionKey, endRowKey, hash, key.KeyName, sasVersion2);
			return signature.ToString();
		}

		// Token: 0x06000B67 RID: 2919 RVA: 0x000281ED File Offset: 0x000263ED
		public override string ToString()
		{
			return this.Name;
		}

		// Token: 0x06000B68 RID: 2920 RVA: 0x000281F8 File Offset: 0x000263F8
		private void ParseQueryAndVerify(StorageUri address, StorageCredentials credentials)
		{
			StorageCredentials storageCredentials;
			this.StorageUri = NavigationHelper.ParseQueueTableQueryAndVerify(address, out storageCredentials);
			if (storageCredentials != null && credentials != null)
			{
				string message = string.Format(CultureInfo.CurrentCulture, "Cannot provide credentials as part of the address and as constructor parameter. Either pass in the address or use a different constructor.", new object[0]);
				throw new ArgumentException(message);
			}
			this.ServiceClient = new CloudTableClient(NavigationHelper.GetServiceClientBaseAddress(this.StorageUri, null), credentials ?? storageCredentials);
			this.Name = NavigationHelper.GetTableNameFromUri(this.Uri, new bool?(this.ServiceClient.UsePathStyleUris));
		}

		// Token: 0x06000B69 RID: 2921 RVA: 0x0002827C File Offset: 0x0002647C
		private string GetCanonicalName(string sasVersion)
		{
			string accountName = this.ServiceClient.Credentials.AccountName;
			string text = this.Name.ToLower();
			string format = "/{0}/{1}/{2}";
			if (sasVersion == "2012-02-12" || sasVersion == "2013-08-15")
			{
				format = "/{1}/{2}";
			}
			return string.Format(CultureInfo.InvariantCulture, format, new object[]
			{
				"table",
				accountName,
				text
			});
		}
	}
}
