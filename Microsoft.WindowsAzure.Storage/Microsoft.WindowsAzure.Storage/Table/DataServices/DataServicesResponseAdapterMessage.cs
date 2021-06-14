using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Data.OData;

namespace Microsoft.WindowsAzure.Storage.Table.DataServices
{
	// Token: 0x0200006D RID: 109
	internal class DataServicesResponseAdapterMessage : IODataResponseMessage
	{
		// Token: 0x06000E03 RID: 3587 RVA: 0x00036752 File Offset: 0x00034952
		public DataServicesResponseAdapterMessage(Dictionary<string, string> responseHeaders, Stream inputStream) : this(responseHeaders, inputStream, null)
		{
		}

		// Token: 0x06000E04 RID: 3588 RVA: 0x0003675D File Offset: 0x0003495D
		public DataServicesResponseAdapterMessage(IDictionary<string, string> responseHeaders, Stream inputStream, string responseContentType)
		{
			this.responseHeaders = responseHeaders;
			this.inputStream = inputStream;
			this.responseContentType = responseContentType;
		}

		// Token: 0x06000E05 RID: 3589 RVA: 0x00036782 File Offset: 0x00034982
		public Task<Stream> GetStreamAsync()
		{
			return Task.Factory.StartNew<Stream>(() => this.inputStream);
		}

		// Token: 0x06000E06 RID: 3590 RVA: 0x0003679C File Offset: 0x0003499C
		public string GetHeader(string headerName)
		{
			if (headerName == "Content-Type")
			{
				if (this.responseContentType != null)
				{
					return this.responseContentType;
				}
				return this.responseHeaders["Content-Type"];
			}
			else
			{
				if (headerName == "DataServiceVersion" || headerName == "Preference-Applied")
				{
					return null;
				}
				if (headerName == "Content-Encoding")
				{
					return this.responseHeaders["ContentEncoding"];
				}
				return this.responseHeaders[headerName];
			}
		}

		// Token: 0x06000E07 RID: 3591 RVA: 0x0003681E File Offset: 0x00034A1E
		public Stream GetStream()
		{
			return this.inputStream;
		}

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x06000E08 RID: 3592 RVA: 0x00036828 File Offset: 0x00034A28
		public IEnumerable<KeyValuePair<string, string>> Headers
		{
			get
			{
				List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
				foreach (string text in this.responseHeaders.Keys)
				{
					if (text == "Content-Type" && this.responseContentType != null)
					{
						list.Add(new KeyValuePair<string, string>(text, this.responseContentType));
					}
					else
					{
						list.Add(new KeyValuePair<string, string>(text, this.responseHeaders[text]));
					}
				}
				return list;
			}
		}

		// Token: 0x06000E09 RID: 3593 RVA: 0x000368BC File Offset: 0x00034ABC
		public void SetHeader(string headerName, string headerValue)
		{
			throw new NotSupportedException();
		}

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x06000E0A RID: 3594 RVA: 0x000368C3 File Offset: 0x00034AC3
		// (set) Token: 0x06000E0B RID: 3595 RVA: 0x000368CA File Offset: 0x00034ACA
		public int StatusCode
		{
			get
			{
				throw new NotSupportedException();
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x040001F5 RID: 501
		private IDictionary<string, string> responseHeaders;

		// Token: 0x040001F6 RID: 502
		private Stream inputStream;

		// Token: 0x040001F7 RID: 503
		private string responseContentType;
	}
}
