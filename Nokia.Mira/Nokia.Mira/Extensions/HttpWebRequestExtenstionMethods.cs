using System;
using System.Net;
using System.Threading;
using Nokia.Mira.Primitives;

namespace Nokia.Mira.Extensions
{
	// Token: 0x0200002C RID: 44
	internal static class HttpWebRequestExtenstionMethods
	{
		// Token: 0x060000B7 RID: 183 RVA: 0x000039A0 File Offset: 0x00001BA0
		public static IWebResponse GetResponse(this HttpWebRequest request, long rangeBegin, long rangeEnd, CancellationToken token)
		{
			request.AddRange(rangeBegin, rangeEnd);
			IDisposable disposable = null;
			IWebResponse result;
			try
			{
				disposable = token.Register(new Action(request.Abort));
				result = new WebResponseAdapter((HttpWebResponse)request.GetResponse());
			}
			catch (WebException ex)
			{
				if (ex.Status == WebExceptionStatus.ProtocolError)
				{
					HttpWebResponse httpWebResponse = ex.Response as HttpWebResponse;
					if (httpWebResponse != null && httpWebResponse.StatusCode == HttpStatusCode.RequestedRangeNotSatisfiable)
					{
						return new WebResponseAdapter((HttpWebResponse)ex.Response);
					}
				}
				if (ex.Status == WebExceptionStatus.RequestCanceled)
				{
					throw new OperationCanceledException(token);
				}
				throw;
			}
			finally
			{
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
			return result;
		}
	}
}
