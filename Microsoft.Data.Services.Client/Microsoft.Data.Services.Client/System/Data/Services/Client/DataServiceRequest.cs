using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Xml;
using Microsoft.Data.OData;

namespace System.Data.Services.Client
{
	// Token: 0x02000109 RID: 265
	public abstract class DataServiceRequest
	{
		// Token: 0x06000897 RID: 2199 RVA: 0x00023F54 File Offset: 0x00022154
		internal DataServiceRequest()
		{
			this.PayloadKind = ODataPayloadKind.Unsupported;
		}

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x06000898 RID: 2200
		public abstract Type ElementType { get; }

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x06000899 RID: 2201
		// (set) Token: 0x0600089A RID: 2202
		public abstract Uri RequestUri { get; internal set; }

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x0600089B RID: 2203
		internal abstract ProjectionPlan Plan { get; }

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x0600089C RID: 2204 RVA: 0x00023F67 File Offset: 0x00022167
		// (set) Token: 0x0600089D RID: 2205 RVA: 0x00023F6F File Offset: 0x0002216F
		internal ODataPayloadKind PayloadKind { get; set; }

		// Token: 0x0600089E RID: 2206 RVA: 0x00023F78 File Offset: 0x00022178
		internal static MaterializeAtom Materialize(ResponseInfo responseInfo, QueryComponents queryComponents, ProjectionPlan plan, string contentType, IODataResponseMessage message, ODataPayloadKind expectedPayloadKind)
		{
			if (message.StatusCode == 204 || string.IsNullOrEmpty(contentType))
			{
				return MaterializeAtom.EmptyResults;
			}
			return new MaterializeAtom(responseInfo, queryComponents, plan, message, expectedPayloadKind);
		}

		// Token: 0x0600089F RID: 2207 RVA: 0x00023FA4 File Offset: 0x000221A4
		internal static DataServiceRequest GetInstance(Type elementType, Uri requestUri)
		{
			Type type = typeof(DataServiceRequest<>).MakeGenericType(new Type[]
			{
				elementType
			});
			return (DataServiceRequest)Activator.CreateInstance(type, new object[]
			{
				requestUri
			});
		}

		// Token: 0x060008A0 RID: 2208 RVA: 0x00023FE4 File Offset: 0x000221E4
		internal static IEnumerable<TElement> EndExecute<TElement>(object source, DataServiceContext context, string method, IAsyncResult asyncResult)
		{
			IEnumerable<TElement> result;
			try
			{
				QueryResult queryResult = QueryResult.EndExecuteQuery<TElement>(source, method, asyncResult);
				result = queryResult.ProcessResult<TElement>(queryResult.ServiceRequest.Plan);
			}
			catch (DataServiceQueryException ex)
			{
				Exception ex2 = ex;
				while (ex2.InnerException != null)
				{
					ex2 = ex2.InnerException;
				}
				DataServiceClientException ex3 = ex2 as DataServiceClientException;
				if (!context.IgnoreResourceNotFoundException || ex3 == null || ex3.StatusCode != 404)
				{
					throw;
				}
				result = (IEnumerable<TElement>)new QueryOperationResponse<TElement>(ex.Response.HeaderCollection, ex.Response.Query, MaterializeAtom.EmptyResults)
				{
					StatusCode = 404
				};
			}
			return result;
		}

		// Token: 0x060008A1 RID: 2209
		internal abstract QueryComponents QueryComponents(ClientEdmModel model);

		// Token: 0x060008A2 RID: 2210 RVA: 0x00024094 File Offset: 0x00022294
		internal QueryOperationResponse<TElement> Execute<TElement>(DataServiceContext context, QueryComponents queryComponents)
		{
			QueryResult queryResult = null;
			QueryOperationResponse<TElement> result;
			try
			{
				Uri uri = queryComponents.Uri;
				DataServiceRequest<TElement> dataServiceRequest = new DataServiceRequest<TElement>(uri, queryComponents, this.Plan);
				queryResult = dataServiceRequest.CreateExecuteResult(this, context, null, null, "Execute");
				queryResult.ExecuteQuery();
				result = queryResult.ProcessResult<TElement>(this.Plan);
			}
			catch (InvalidOperationException ex)
			{
				if (queryResult != null)
				{
					QueryOperationResponse response = queryResult.GetResponse<TElement>(MaterializeAtom.EmptyResults);
					if (response != null)
					{
						if (context.IgnoreResourceNotFoundException)
						{
							DataServiceClientException ex2 = ex as DataServiceClientException;
							if (ex2 != null && ex2.StatusCode == 404)
							{
								return (QueryOperationResponse<TElement>)response;
							}
						}
						response.Error = ex;
						throw new DataServiceQueryException(Strings.DataServiceException_GeneralError, ex, response);
					}
				}
				throw;
			}
			return result;
		}

		// Token: 0x060008A3 RID: 2211 RVA: 0x0002414C File Offset: 0x0002234C
		internal long GetQuerySetCount(DataServiceContext context)
		{
			Version version = this.QueryComponents(context.Model).Version;
			if (version == null || version.Major < 2)
			{
				version = Util.DataServiceVersion2;
			}
			QueryResult queryResult = null;
			QueryComponents queryComponents = this.QueryComponents(context.Model);
			Uri uri = queryComponents.Uri;
			DataServiceRequest<long> serviceRequest = new DataServiceRequest<long>(uri, queryComponents, null);
			HeaderCollection headerCollection = new HeaderCollection();
			headerCollection.SetRequestVersion(version, context.MaxProtocolVersionAsVersion);
			context.Format.SetRequestAcceptHeaderForCount(headerCollection);
			string method = "GET";
			ODataRequestMessageWrapper request = context.CreateODataRequestMessage(context.CreateRequestArgsAndFireBuildingRequest(method, uri, headerCollection, context.HttpStack, null), new string[]
			{
				"Accept"
			}, null);
			queryResult = new QueryResult(this, "Execute", serviceRequest, request, new RequestInfo(context), null, null);
			long result;
			try
			{
				queryResult.ExecuteQuery();
				if (HttpStatusCode.NoContent == queryResult.StatusCode)
				{
					throw new DataServiceQueryException(Strings.DataServiceRequest_FailGetCount, queryResult.Failure);
				}
				StreamReader streamReader = new StreamReader(queryResult.GetResponseStream());
				long num = -1L;
				try
				{
					num = XmlConvert.ToInt64(streamReader.ReadToEnd());
				}
				finally
				{
					streamReader.Close();
				}
				result = num;
			}
			catch (InvalidOperationException ex)
			{
				QueryOperationResponse response = queryResult.GetResponse<long>(MaterializeAtom.EmptyResults);
				if (response != null)
				{
					response.Error = ex;
					throw new DataServiceQueryException(Strings.DataServiceException_GeneralError, ex, response);
				}
				throw;
			}
			return result;
		}

		// Token: 0x060008A4 RID: 2212 RVA: 0x000242B4 File Offset: 0x000224B4
		internal IAsyncResult BeginExecute(object source, DataServiceContext context, AsyncCallback callback, object state, string method)
		{
			QueryResult queryResult = this.CreateExecuteResult(source, context, callback, state, method);
			queryResult.BeginExecuteQuery();
			return queryResult;
		}

		// Token: 0x060008A5 RID: 2213 RVA: 0x000242D8 File Offset: 0x000224D8
		private QueryResult CreateExecuteResult(object source, DataServiceContext context, AsyncCallback callback, object state, string method)
		{
			QueryComponents queryComponents = this.QueryComponents(context.Model);
			RequestInfo requestInfo = new RequestInfo(context);
			if (queryComponents.UriOperationParameters != null)
			{
				Serializer serializer = new Serializer(requestInfo);
				this.RequestUri = serializer.WriteUriOperationParametersToUri(this.RequestUri, queryComponents.UriOperationParameters);
			}
			HeaderCollection headerCollection = new HeaderCollection();
			if (string.CompareOrdinal("POST", queryComponents.HttpMethod) == 0)
			{
				if (queryComponents.BodyOperationParameters == null)
				{
					headerCollection.SetHeader("Content-Length", "0");
				}
				else
				{
					context.Format.SetRequestContentTypeForOperationParameters(headerCollection);
				}
			}
			headerCollection.SetRequestVersion(queryComponents.Version, requestInfo.MaxProtocolVersionAsVersion);
			requestInfo.Format.SetRequestAcceptHeaderForQuery(headerCollection, queryComponents);
			ODataRequestMessageWrapper odataRequestMessageWrapper = new RequestInfo(context).WriteHelper.CreateRequestMessage(context.CreateRequestArgsAndFireBuildingRequest(queryComponents.HttpMethod, this.RequestUri, headerCollection, context.HttpStack, null));
			odataRequestMessageWrapper.FireSendingRequest2(null);
			QueryResult result;
			if (queryComponents.BodyOperationParameters != null)
			{
				Serializer serializer2 = new Serializer(requestInfo);
				serializer2.WriteBodyOperationParameters(queryComponents.BodyOperationParameters, odataRequestMessageWrapper);
				result = new QueryResult(source, method, this, odataRequestMessageWrapper, requestInfo, callback, state, odataRequestMessageWrapper.CachedRequestStream);
			}
			else
			{
				result = new QueryResult(source, method, this, odataRequestMessageWrapper, requestInfo, callback, state);
			}
			return result;
		}
	}
}
