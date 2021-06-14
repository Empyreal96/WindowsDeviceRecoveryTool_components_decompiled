using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using Microsoft.WindowsAzure.Storage.Core.Util;
using Microsoft.WindowsAzure.Storage.Table.Protocol;

namespace Microsoft.WindowsAzure.Storage
{
	// Token: 0x0200007E RID: 126
	[Serializable]
	public class StorageException : Exception
	{
		// Token: 0x170001BE RID: 446
		// (get) Token: 0x06000EE8 RID: 3816 RVA: 0x0003901B File Offset: 0x0003721B
		// (set) Token: 0x06000EE9 RID: 3817 RVA: 0x00039023 File Offset: 0x00037223
		public RequestResult RequestInformation { get; private set; }

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x06000EEA RID: 3818 RVA: 0x0003902C File Offset: 0x0003722C
		// (set) Token: 0x06000EEB RID: 3819 RVA: 0x00039034 File Offset: 0x00037234
		internal bool IsRetryable { get; set; }

		// Token: 0x06000EEC RID: 3820 RVA: 0x0003903D File Offset: 0x0003723D
		public StorageException() : this(null, null, null)
		{
		}

		// Token: 0x06000EED RID: 3821 RVA: 0x00039048 File Offset: 0x00037248
		public StorageException(string message) : this(null, message, null)
		{
		}

		// Token: 0x06000EEE RID: 3822 RVA: 0x00039053 File Offset: 0x00037253
		public StorageException(string message, Exception innerException) : this(null, message, innerException)
		{
		}

		// Token: 0x06000EEF RID: 3823 RVA: 0x0003905E File Offset: 0x0003725E
		protected StorageException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			if (info != null)
			{
				this.IsRetryable = info.GetBoolean("IsRetryable");
				this.RequestInformation = (RequestResult)info.GetValue("RequestInformation", typeof(RequestResult));
			}
		}

		// Token: 0x06000EF0 RID: 3824 RVA: 0x0003909C File Offset: 0x0003729C
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info != null)
			{
				info.AddValue("IsRetryable", this.IsRetryable);
				info.AddValue("RequestInformation", this.RequestInformation, typeof(RequestResult));
			}
			base.GetObjectData(info, context);
		}

		// Token: 0x06000EF1 RID: 3825 RVA: 0x000390D5 File Offset: 0x000372D5
		public StorageException(RequestResult res, string message, Exception inner) : base(message, inner)
		{
			this.RequestInformation = res;
			this.IsRetryable = true;
		}

		// Token: 0x06000EF2 RID: 3826 RVA: 0x000390ED File Offset: 0x000372ED
		public static StorageException TranslateException(Exception ex, RequestResult reqResult)
		{
			return StorageException.TranslateException(ex, reqResult, null);
		}

		// Token: 0x06000EF3 RID: 3827 RVA: 0x000390F8 File Offset: 0x000372F8
		public static StorageException TranslateException(Exception ex, RequestResult reqResult, Func<Stream, StorageExtendedErrorInformation> parseError)
		{
			try
			{
				StorageException result;
				if ((result = StorageException.CoreTranslate(ex, reqResult, ref parseError)) != null)
				{
					return result;
				}
				WebException ex2 = ex as WebException;
				if (ex2 != null)
				{
					HttpWebResponse httpWebResponse = ex2.Response as HttpWebResponse;
					if (httpWebResponse != null)
					{
						StorageException.PopulateRequestResult(reqResult, httpWebResponse);
						reqResult.ExtendedErrorInformation = parseError(httpWebResponse.GetResponseStream());
					}
				}
			}
			catch (Exception)
			{
			}
			return new StorageException(reqResult, ex.Message, ex);
		}

		// Token: 0x06000EF4 RID: 3828 RVA: 0x0003916C File Offset: 0x0003736C
		internal static StorageException TranslateExceptionWithPreBufferedStream(Exception ex, RequestResult reqResult, Func<Stream, StorageExtendedErrorInformation> parseError, Stream responseStream)
		{
			try
			{
				StorageException result;
				if ((result = StorageException.CoreTranslate(ex, reqResult, ref parseError)) != null)
				{
					return result;
				}
				WebException ex2 = ex as WebException;
				if (ex2 != null)
				{
					HttpWebResponse httpWebResponse = ex2.Response as HttpWebResponse;
					if (httpWebResponse != null)
					{
						StorageException.PopulateRequestResult(reqResult, httpWebResponse);
						reqResult.ExtendedErrorInformation = parseError(responseStream);
					}
				}
			}
			catch (Exception)
			{
			}
			return new StorageException(reqResult, ex.Message, ex);
		}

		// Token: 0x06000EF5 RID: 3829 RVA: 0x000391DC File Offset: 0x000373DC
		private static StorageException CoreTranslate(Exception ex, RequestResult reqResult, ref Func<Stream, StorageExtendedErrorInformation> parseError)
		{
			CommonUtility.AssertNotNull("reqResult", reqResult);
			CommonUtility.AssertNotNull("ex", ex);
			if (parseError == null)
			{
				parseError = new Func<Stream, StorageExtendedErrorInformation>(StorageExtendedErrorInformation.ReadFromStream);
			}
			if (ex is StorageException)
			{
				return (StorageException)ex;
			}
			if (ex is TimeoutException)
			{
				reqResult.HttpStatusMessage = null;
				reqResult.HttpStatusCode = 408;
				reqResult.ExtendedErrorInformation = null;
				return new StorageException(reqResult, ex.Message, ex);
			}
			if (ex is ArgumentException)
			{
				reqResult.HttpStatusMessage = null;
				reqResult.HttpStatusCode = 306;
				reqResult.ExtendedErrorInformation = null;
				return new StorageException(reqResult, ex.Message, ex)
				{
					IsRetryable = false
				};
			}
			StorageException ex2 = TableUtilities.TranslateDataServiceException(ex, reqResult, null);
			if (ex2 != null)
			{
				return ex2;
			}
			return null;
		}

		// Token: 0x06000EF6 RID: 3830 RVA: 0x00039298 File Offset: 0x00037498
		internal static StorageException TranslateDataServiceException(Exception ex, RequestResult reqResult, Func<Stream, IDictionary<string, string>, StorageExtendedErrorInformation> parseError)
		{
			CommonUtility.AssertNotNull("reqResult", reqResult);
			CommonUtility.AssertNotNull("ex", ex);
			CommonUtility.AssertNotNull("parseError", parseError);
			if (ex is StorageException)
			{
				return (StorageException)ex;
			}
			if (ex is TimeoutException)
			{
				reqResult.HttpStatusMessage = null;
				reqResult.HttpStatusCode = 408;
				reqResult.ExtendedErrorInformation = null;
				return new StorageException(reqResult, ex.Message, ex);
			}
			if (ex is ArgumentException)
			{
				reqResult.HttpStatusMessage = null;
				reqResult.HttpStatusCode = 306;
				reqResult.ExtendedErrorInformation = null;
				return new StorageException(reqResult, ex.Message, ex)
				{
					IsRetryable = false
				};
			}
			StorageException ex2 = TableUtilities.TranslateDataServiceException(ex, reqResult, parseError);
			if (ex2 != null)
			{
				return ex2;
			}
			return new StorageException(reqResult, ex.Message, ex);
		}

		// Token: 0x06000EF7 RID: 3831 RVA: 0x00039358 File Offset: 0x00037558
		private static void PopulateRequestResult(RequestResult reqResult, HttpWebResponse response)
		{
			reqResult.HttpStatusMessage = response.StatusDescription;
			reqResult.HttpStatusCode = (int)response.StatusCode;
			if (response.Headers != null)
			{
				reqResult.ServiceRequestID = HttpWebUtility.TryGetHeader(response, "x-ms-request-id", null);
				reqResult.ContentMd5 = HttpWebUtility.TryGetHeader(response, "Content-MD5", null);
				string text = HttpWebUtility.TryGetHeader(response, "Date", null);
				reqResult.RequestDate = (string.IsNullOrEmpty(text) ? DateTime.Now.ToString("R", CultureInfo.InvariantCulture) : text);
				reqResult.Etag = response.Headers[HttpResponseHeader.ETag];
			}
			if (response.ContentLength > 0L)
			{
				reqResult.IngressBytes += response.ContentLength;
			}
		}

		// Token: 0x06000EF8 RID: 3832 RVA: 0x00039410 File Offset: 0x00037610
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(base.ToString());
			if (this.RequestInformation != null)
			{
				stringBuilder.AppendLine("Request Information");
				stringBuilder.AppendLine("RequestID:" + this.RequestInformation.ServiceRequestID);
				stringBuilder.AppendLine("RequestDate:" + this.RequestInformation.RequestDate);
				stringBuilder.AppendLine("StatusMessage:" + this.RequestInformation.HttpStatusMessage);
				if (this.RequestInformation.ExtendedErrorInformation != null)
				{
					stringBuilder.AppendLine("ErrorCode:" + this.RequestInformation.ExtendedErrorInformation.ErrorCode);
				}
			}
			return stringBuilder.ToString();
		}
	}
}
