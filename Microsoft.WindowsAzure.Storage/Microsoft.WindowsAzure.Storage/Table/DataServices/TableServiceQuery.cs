using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Core.Executor;
using Microsoft.WindowsAzure.Storage.Core.Util;
using Microsoft.WindowsAzure.Storage.Table.Protocol;

namespace Microsoft.WindowsAzure.Storage.Table.DataServices
{
	// Token: 0x02000048 RID: 72
	[Obsolete("Support for accessing Windows Azure Tables via WCF Data Services is now obsolete. It's recommended that you use the Microsoft.WindowsAzure.Storage.Table namespace for working with tables.")]
	public class TableServiceQuery<TElement> : IQueryable<TElement>, IEnumerable<TElement>, IQueryable, IEnumerable
	{
		// Token: 0x06000C90 RID: 3216 RVA: 0x0002CFB5 File Offset: 0x0002B1B5
		[Obsolete("Support for accessing Windows Azure Tables via WCF Data Services is now obsolete. It's recommended that you use the Microsoft.WindowsAzure.Storage.Table namespace for working with tables.")]
		public TableServiceQuery(IQueryable<TElement> query, TableServiceContext context)
		{
			this.Query = (query as DataServiceQuery<TElement>);
			this.Context = context;
			this.IgnoreResourceNotFoundException = false;
		}

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x06000C91 RID: 3217 RVA: 0x0002CFD7 File Offset: 0x0002B1D7
		// (set) Token: 0x06000C92 RID: 3218 RVA: 0x0002CFDF File Offset: 0x0002B1DF
		[Obsolete("Support for accessing Windows Azure Tables via WCF Data Services is now obsolete. It's recommended that you use the Microsoft.WindowsAzure.Storage.Table namespace for working with tables.")]
		public TableServiceContext Context { get; private set; }

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x06000C93 RID: 3219 RVA: 0x0002CFE8 File Offset: 0x0002B1E8
		// (set) Token: 0x06000C94 RID: 3220 RVA: 0x0002CFF0 File Offset: 0x0002B1F0
		internal DataServiceQuery<TElement> Query { get; set; }

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x06000C95 RID: 3221 RVA: 0x0002CFF9 File Offset: 0x0002B1F9
		// (set) Token: 0x06000C96 RID: 3222 RVA: 0x0002D001 File Offset: 0x0002B201
		internal bool IgnoreResourceNotFoundException { get; set; }

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x06000C97 RID: 3223 RVA: 0x0002D00A File Offset: 0x0002B20A
		[Obsolete("Support for accessing Windows Azure Tables via WCF Data Services is now obsolete. It's recommended that you use the Microsoft.WindowsAzure.Storage.Table namespace for working with tables.")]
		public Type ElementType
		{
			get
			{
				return this.Query.ElementType;
			}
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x06000C98 RID: 3224 RVA: 0x0002D017 File Offset: 0x0002B217
		[Obsolete("Support for accessing Windows Azure Tables via WCF Data Services is now obsolete. It's recommended that you use the Microsoft.WindowsAzure.Storage.Table namespace for working with tables.")]
		public Expression Expression
		{
			get
			{
				return this.Query.Expression;
			}
		}

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x06000C99 RID: 3225 RVA: 0x0002D024 File Offset: 0x0002B224
		[Obsolete("Support for accessing Windows Azure Tables via WCF Data Services is now obsolete. It's recommended that you use the Microsoft.WindowsAzure.Storage.Table namespace for working with tables.")]
		public IQueryProvider Provider
		{
			get
			{
				return this.Query.Provider;
			}
		}

		// Token: 0x06000C9A RID: 3226 RVA: 0x0002D031 File Offset: 0x0002B231
		[Obsolete("Support for accessing Windows Azure Tables via WCF Data Services is now obsolete. It's recommended that you use the Microsoft.WindowsAzure.Storage.Table namespace for working with tables.")]
		public TableServiceQuery<TElement> Expand(string path)
		{
			return new TableServiceQuery<TElement>(this.Query.Expand(path), this.Context);
		}

		// Token: 0x06000C9B RID: 3227 RVA: 0x0002D04A File Offset: 0x0002B24A
		[Obsolete("Support for accessing Windows Azure Tables via WCF Data Services is now obsolete. It's recommended that you use the Microsoft.WindowsAzure.Storage.Table namespace for working with tables.")]
		public IEnumerator<TElement> GetEnumerator()
		{
			return this.Execute(null, null).GetEnumerator();
		}

		// Token: 0x06000C9C RID: 3228 RVA: 0x0002D059 File Offset: 0x0002B259
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06000C9D RID: 3229 RVA: 0x0002D088 File Offset: 0x0002B288
		[Obsolete("Support for accessing Windows Azure Tables via WCF Data Services is now obsolete. It's recommended that you use the Microsoft.WindowsAzure.Storage.Table namespace for working with tables.")]
		[DoesServiceRequest]
		public IEnumerable<TElement> Execute(TableRequestOptions requestOptions = null, OperationContext operationContext = null)
		{
			requestOptions = TableRequestOptions.ApplyDefaults(requestOptions, this.Context.ServiceClient);
			operationContext = (operationContext ?? new OperationContext());
			long queryTakeCount = TableUtilities.GetQueryTakeCount<TElement>(this.Query, long.MaxValue);
			return CommonUtility.LazyEnumerable<TElement>((IContinuationToken continuationToken) => this.ExecuteSegmentedCore((TableContinuationToken)continuationToken, requestOptions, operationContext), queryTakeCount);
		}

		// Token: 0x06000C9E RID: 3230 RVA: 0x0002D108 File Offset: 0x0002B308
		[DoesServiceRequest]
		[Obsolete("Support for accessing Windows Azure Tables via WCF Data Services is now obsolete. It's recommended that you use the Microsoft.WindowsAzure.Storage.Table namespace for working with tables.")]
		public TableQuerySegment<TElement> ExecuteSegmented(TableContinuationToken continuationToken, TableRequestOptions requestOptions = null, OperationContext operationContext = null)
		{
			requestOptions = TableRequestOptions.ApplyDefaults(requestOptions, this.Context.ServiceClient);
			operationContext = (operationContext ?? new OperationContext());
			return new TableQuerySegment<TElement>(this.ExecuteSegmentedCore(continuationToken, requestOptions, operationContext));
		}

		// Token: 0x06000C9F RID: 3231 RVA: 0x0002D137 File Offset: 0x0002B337
		[DoesServiceRequest]
		[Obsolete("Support for accessing Windows Azure Tables via WCF Data Services is now obsolete. It's recommended that you use the Microsoft.WindowsAzure.Storage.Table namespace for working with tables.")]
		public ICancellableAsyncResult BeginExecuteSegmented(TableContinuationToken currentToken, AsyncCallback callback, object state)
		{
			return this.BeginExecuteSegmented(currentToken, null, null, callback, state);
		}

		// Token: 0x06000CA0 RID: 3232 RVA: 0x0002D144 File Offset: 0x0002B344
		[DoesServiceRequest]
		[Obsolete("Support for accessing Windows Azure Tables via WCF Data Services is now obsolete. It's recommended that you use the Microsoft.WindowsAzure.Storage.Table namespace for working with tables.")]
		public ICancellableAsyncResult BeginExecuteSegmented(TableContinuationToken currentToken, TableRequestOptions requestOptions, OperationContext operationContext, AsyncCallback callback, object state)
		{
			requestOptions = TableRequestOptions.ApplyDefaults(requestOptions, this.Context.ServiceClient);
			operationContext = (operationContext ?? new OperationContext());
			TableCommand<ResultSegment<TElement>, IEnumerable<TElement>> cmd = this.GenerateExecuteCommand(currentToken, requestOptions);
			return TableExecutor.BeginExecuteAsync<ResultSegment<TElement>, IEnumerable<TElement>>(cmd, requestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x06000CA1 RID: 3233 RVA: 0x0002D18A File Offset: 0x0002B38A
		[Obsolete("Support for accessing Windows Azure Tables via WCF Data Services is now obsolete. It's recommended that you use the Microsoft.WindowsAzure.Storage.Table namespace for working with tables.")]
		public TableQuerySegment<TElement> EndExecuteSegmented(IAsyncResult asyncResult)
		{
			return new TableQuerySegment<TElement>(TableExecutor.EndExecuteAsync<ResultSegment<TElement>, IEnumerable<TElement>>(asyncResult));
		}

		// Token: 0x06000CA2 RID: 3234 RVA: 0x0002D197 File Offset: 0x0002B397
		[Obsolete("Support for accessing Windows Azure Tables via WCF Data Services is now obsolete. It's recommended that you use the Microsoft.WindowsAzure.Storage.Table namespace for working with tables.")]
		[DoesServiceRequest]
		public Task<TableQuerySegment<TElement>> ExecuteSegmentedAsync(TableContinuationToken currentToken)
		{
			return this.ExecuteSegmentedAsync(currentToken, CancellationToken.None);
		}

		// Token: 0x06000CA3 RID: 3235 RVA: 0x0002D1A5 File Offset: 0x0002B3A5
		[DoesServiceRequest]
		[Obsolete("Support for accessing Windows Azure Tables via WCF Data Services is now obsolete. It's recommended that you use the Microsoft.WindowsAzure.Storage.Table namespace for working with tables.")]
		public Task<TableQuerySegment<TElement>> ExecuteSegmentedAsync(TableContinuationToken currentToken, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<TableContinuationToken, TableQuerySegment<TElement>>(new Func<TableContinuationToken, AsyncCallback, object, ICancellableAsyncResult>(this.BeginExecuteSegmented), new Func<IAsyncResult, TableQuerySegment<TElement>>(this.EndExecuteSegmented), currentToken, cancellationToken);
		}

		// Token: 0x06000CA4 RID: 3236 RVA: 0x0002D1C6 File Offset: 0x0002B3C6
		[Obsolete("Support for accessing Windows Azure Tables via WCF Data Services is now obsolete. It's recommended that you use the Microsoft.WindowsAzure.Storage.Table namespace for working with tables.")]
		[DoesServiceRequest]
		public Task<TableQuerySegment<TElement>> ExecuteSegmentedAsync(TableContinuationToken currentToken, TableRequestOptions requestOptions, OperationContext operationContext)
		{
			return this.ExecuteSegmentedAsync(currentToken, requestOptions, operationContext, CancellationToken.None);
		}

		// Token: 0x06000CA5 RID: 3237 RVA: 0x0002D1D6 File Offset: 0x0002B3D6
		[Obsolete("Support for accessing Windows Azure Tables via WCF Data Services is now obsolete. It's recommended that you use the Microsoft.WindowsAzure.Storage.Table namespace for working with tables.")]
		[DoesServiceRequest]
		public Task<TableQuerySegment<TElement>> ExecuteSegmentedAsync(TableContinuationToken currentToken, TableRequestOptions requestOptions, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<TableContinuationToken, TableRequestOptions, OperationContext, TableQuerySegment<TElement>>(new Func<TableContinuationToken, TableRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginExecuteSegmented), new Func<IAsyncResult, TableQuerySegment<TElement>>(this.EndExecuteSegmented), currentToken, requestOptions, operationContext, cancellationToken);
		}

		// Token: 0x06000CA6 RID: 3238 RVA: 0x0002D1FC File Offset: 0x0002B3FC
		internal ResultSegment<TElement> ExecuteSegmentedCore(TableContinuationToken continuationToken, TableRequestOptions requestOptions, OperationContext operationContext)
		{
			TableCommand<ResultSegment<TElement>, IEnumerable<TElement>> cmd = this.GenerateExecuteCommand(continuationToken, requestOptions);
			return TableExecutor.ExecuteSync<ResultSegment<TElement>, IEnumerable<TElement>>(cmd, requestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x06000CA7 RID: 3239 RVA: 0x0002D238 File Offset: 0x0002B438
		private TableCommand<ResultSegment<TElement>, IEnumerable<TElement>> GenerateExecuteCommand(TableContinuationToken continuationToken, TableRequestOptions requestOptions)
		{
			DataServiceQuery<TElement> localQuery = this.Query;
			localQuery = TableUtilities.ApplyContinuationToQuery<TElement>(continuationToken, localQuery);
			if (requestOptions.ServerTimeout != null)
			{
				localQuery = localQuery.AddQueryOption("timeout", Convert.ToString(requestOptions.ServerTimeout.Value.TotalSeconds, CultureInfo.InvariantCulture));
			}
			TableCommand<ResultSegment<TElement>, IEnumerable<TElement>> tableCommand = new TableCommand<ResultSegment<TElement>, IEnumerable<TElement>>();
			tableCommand.ExecuteFunc = new Func<IEnumerable<TElement>>(localQuery.Execute);
			tableCommand.Begin = ((AsyncCallback callback, object state) => localQuery.BeginExecute(callback, state));
			tableCommand.End = new Func<IAsyncResult, IEnumerable<TElement>>(localQuery.EndExecute);
			tableCommand.ParseResponse = new Func<IEnumerable<TElement>, RequestResult, TableCommand<ResultSegment<TElement>, IEnumerable<TElement>>, ResultSegment<TElement>>(this.ParseTableQueryResponse);
			tableCommand.ParseDataServiceError = new Func<Stream, IDictionary<string, string>, string, StorageExtendedErrorInformation>(StorageExtendedErrorInformation.ReadDataServiceResponseFromStream);
			tableCommand.Context = this.Context;
			requestOptions.ApplyToStorageCommand<ResultSegment<TElement>, IEnumerable<TElement>>(tableCommand);
			return tableCommand;
		}

		// Token: 0x06000CA8 RID: 3240 RVA: 0x0002D330 File Offset: 0x0002B530
		private ResultSegment<TElement> ParseTableQueryResponse(IEnumerable<TElement> dataServiceQueryResponse, RequestResult reqResult, TableCommand<ResultSegment<TElement>, IEnumerable<TElement>> cmd)
		{
			if (reqResult.Exception == null)
			{
				QueryOperationResponse response = dataServiceQueryResponse as QueryOperationResponse;
				ResultSegment<TElement> resultSegment = new ResultSegment<TElement>(dataServiceQueryResponse.ToList<TElement>());
				resultSegment.ContinuationToken = TableUtilities.ContinuationFromResponse(response);
				if (resultSegment.ContinuationToken != null)
				{
					resultSegment.ContinuationToken.TargetLocation = new StorageLocation?(reqResult.TargetLocation);
				}
				return resultSegment;
			}
			DataServiceClientException ex = TableUtilities.FindInnerExceptionOfType<DataServiceClientException>(reqResult.Exception);
			if (this.IgnoreResourceNotFoundException && ex != null && ex.StatusCode == 404)
			{
				return new ResultSegment<TElement>(new List<TElement>());
			}
			throw reqResult.Exception;
		}
	}
}
