using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.IO;
using System.Linq.Expressions;
using System.Net;
using System.Text;

namespace Microsoft.WindowsAzure.Storage.Table.Protocol
{
	// Token: 0x0200004F RID: 79
	internal static class TableUtilities
	{
		// Token: 0x06000CEA RID: 3306 RVA: 0x0002F8C4 File Offset: 0x0002DAC4
		internal static StorageException TranslateDataServiceException(Exception e, RequestResult reqResult, Func<Stream, IDictionary<string, string>, StorageExtendedErrorInformation> parseError)
		{
			StorageException result;
			try
			{
				DataServiceRequestException ex = TableUtilities.FindInnerExceptionOfType<DataServiceRequestException>(e);
				DataServiceQueryException ex2 = TableUtilities.FindInnerExceptionOfType<DataServiceQueryException>(e);
				if (ex == null && ex2 == null)
				{
					InvalidOperationException ex3 = TableUtilities.FindInnerExceptionOfType<InvalidOperationException>(e);
					if (ex3 != null && !(ex3 is WebException) && string.CompareOrdinal(ex3.Source, "Microsoft.Data.Services.Client") == 0 && ex3.Message.Contains("type is not compatible with the expected"))
					{
						result = new StorageException(reqResult, e.Message, e)
						{
							IsRetryable = false
						};
					}
					else
					{
						result = null;
					}
				}
				else if (ex != null)
				{
					DataServiceResponse response = ex.Response;
					reqResult.HttpStatusCode = response.BatchStatusCode;
					foreach (OperationResponse operationResponse in response)
					{
						reqResult.HttpStatusCode = operationResponse.StatusCode;
						if (reqResult.HttpStatusCode >= 300)
						{
							IDictionary<string, string> headers = operationResponse.Headers;
							string s = ex.InnerException.ToString().Replace("System.Data.Services.Client.DataServiceClientException: ", string.Empty);
							using (Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(s)))
							{
								reqResult.ExtendedErrorInformation = parseError(stream, headers);
								break;
							}
						}
					}
					result = new StorageException(reqResult, (reqResult.ExtendedErrorInformation != null) ? reqResult.ExtendedErrorInformation.ErrorCode : ex.Message, ex);
				}
				else
				{
					QueryOperationResponse response2 = ex2.Response;
					reqResult.HttpStatusCode = response2.StatusCode;
					string message = ex2.InnerException.Message;
					using (Stream stream2 = new MemoryStream(Encoding.UTF8.GetBytes(message)))
					{
						reqResult.ExtendedErrorInformation = parseError(stream2, response2.Headers);
					}
					result = new StorageException(reqResult, (reqResult.ExtendedErrorInformation != null) ? reqResult.ExtendedErrorInformation.ErrorCode : ex2.Message, ex2);
				}
			}
			catch (Exception)
			{
				result = new StorageException(reqResult, e.Message, e);
			}
			return result;
		}

		// Token: 0x06000CEB RID: 3307 RVA: 0x0002FB18 File Offset: 0x0002DD18
		internal static T FindInnerExceptionOfType<T>(Exception exception) where T : Exception
		{
			T t = default(T);
			while (exception != null)
			{
				t = (exception as T);
				if (t != null)
				{
					break;
				}
				exception = exception.InnerException;
			}
			return t;
		}

		// Token: 0x06000CEC RID: 3308 RVA: 0x0002FB50 File Offset: 0x0002DD50
		public static DataServiceQuery<TElement> ApplyContinuationToQuery<TElement>(TableContinuationToken continuationToken, DataServiceQuery<TElement> localQuery)
		{
			if (continuationToken != null)
			{
				if (continuationToken.NextPartitionKey != null)
				{
					localQuery = localQuery.AddQueryOption("NextPartitionKey", continuationToken.NextPartitionKey);
				}
				if (continuationToken.NextRowKey != null)
				{
					localQuery = localQuery.AddQueryOption("NextRowKey", continuationToken.NextRowKey);
				}
				if (continuationToken.NextTableName != null)
				{
					localQuery = localQuery.AddQueryOption("NextTableName", continuationToken.NextTableName);
				}
			}
			return localQuery;
		}

		// Token: 0x06000CED RID: 3309 RVA: 0x0002FBB4 File Offset: 0x0002DDB4
		public static long GetQueryTakeCount<TElement>(DataServiceQuery<TElement> query, long defaultValue)
		{
			MethodCallExpression methodCallExpression = query.Expression as MethodCallExpression;
			if (methodCallExpression != null && methodCallExpression.Method.Name == "Take")
			{
				ConstantExpression constantExpression = methodCallExpression.Arguments[1] as ConstantExpression;
				if (constantExpression != null)
				{
					return (long)((int)constantExpression.Value);
				}
			}
			return defaultValue;
		}

		// Token: 0x06000CEE RID: 3310 RVA: 0x0002FC0C File Offset: 0x0002DE0C
		public static TableContinuationToken ContinuationFromResponse(QueryOperationResponse response)
		{
			string text;
			response.Headers.TryGetValue("x-ms-continuation-NextPartitionKey", out text);
			string text2;
			response.Headers.TryGetValue("x-ms-continuation-NextRowKey", out text2);
			string text3;
			response.Headers.TryGetValue("x-ms-continuation-NextTableName", out text3);
			if (string.IsNullOrEmpty(text) && string.IsNullOrEmpty(text2) && string.IsNullOrEmpty(text3))
			{
				return null;
			}
			return new TableContinuationToken
			{
				NextPartitionKey = text,
				NextRowKey = text2,
				NextTableName = text3
			};
		}

		// Token: 0x06000CEF RID: 3311 RVA: 0x0002FC90 File Offset: 0x0002DE90
		internal static void CopyRequestData(HttpWebRequest destinationRequest, HttpWebRequest sourceRequest)
		{
			destinationRequest.AllowAutoRedirect = sourceRequest.AllowAutoRedirect;
			destinationRequest.AllowWriteStreamBuffering = sourceRequest.AllowWriteStreamBuffering;
			destinationRequest.AuthenticationLevel = sourceRequest.AuthenticationLevel;
			destinationRequest.AutomaticDecompression = sourceRequest.AutomaticDecompression;
			destinationRequest.CachePolicy = sourceRequest.CachePolicy;
			destinationRequest.ClientCertificates = sourceRequest.ClientCertificates;
			destinationRequest.ConnectionGroupName = sourceRequest.ConnectionGroupName;
			destinationRequest.ContinueDelegate = sourceRequest.ContinueDelegate;
			destinationRequest.CookieContainer = sourceRequest.CookieContainer;
			destinationRequest.Credentials = sourceRequest.Credentials;
			destinationRequest.ImpersonationLevel = sourceRequest.ImpersonationLevel;
			destinationRequest.KeepAlive = sourceRequest.KeepAlive;
			destinationRequest.MaximumAutomaticRedirections = sourceRequest.MaximumAutomaticRedirections;
			destinationRequest.MaximumResponseHeadersLength = sourceRequest.MaximumResponseHeadersLength;
			destinationRequest.MediaType = sourceRequest.MediaType;
			destinationRequest.Method = sourceRequest.Method;
			destinationRequest.Pipelined = sourceRequest.Pipelined;
			destinationRequest.PreAuthenticate = sourceRequest.PreAuthenticate;
			destinationRequest.ProtocolVersion = sourceRequest.ProtocolVersion;
			destinationRequest.Proxy = sourceRequest.Proxy;
			destinationRequest.ReadWriteTimeout = sourceRequest.ReadWriteTimeout;
			destinationRequest.SendChunked = sourceRequest.SendChunked;
			destinationRequest.Timeout = sourceRequest.Timeout;
			destinationRequest.UnsafeAuthenticatedConnectionSharing = sourceRequest.UnsafeAuthenticatedConnectionSharing;
			destinationRequest.UseDefaultCredentials = sourceRequest.UseDefaultCredentials;
			foreach (object obj in sourceRequest.Headers)
			{
				string text = (string)obj;
				string key;
				switch (key = text)
				{
				case "Accept":
					destinationRequest.Accept = sourceRequest.Accept;
					continue;
				case "Connection":
					destinationRequest.Connection = sourceRequest.Connection;
					continue;
				case "Content-Length":
					destinationRequest.ContentLength = sourceRequest.ContentLength;
					continue;
				case "Content-Type":
					destinationRequest.ContentType = sourceRequest.ContentType;
					continue;
				case "Expect":
					destinationRequest.Expect = sourceRequest.Expect;
					continue;
				case "If-Modified-Since":
					destinationRequest.IfModifiedSince = sourceRequest.IfModifiedSince;
					continue;
				case "Referer":
					destinationRequest.Referer = sourceRequest.Referer;
					continue;
				case "Transfer-Encoding":
					destinationRequest.TransferEncoding = sourceRequest.TransferEncoding;
					continue;
				case "User-Agent":
					destinationRequest.UserAgent = sourceRequest.UserAgent;
					continue;
				}
				destinationRequest.Headers.Add(text, sourceRequest.Headers[text]);
			}
		}
	}
}
