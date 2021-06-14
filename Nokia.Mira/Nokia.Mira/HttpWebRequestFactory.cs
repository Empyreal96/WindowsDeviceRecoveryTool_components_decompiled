using System;
using System.Net;

namespace Nokia.Mira
{
	// Token: 0x0200001B RID: 27
	public sealed class HttpWebRequestFactory : IHttpWebRequestFactory
	{
		// Token: 0x06000054 RID: 84 RVA: 0x00002B78 File Offset: 0x00000D78
		public HttpWebRequestFactory(Uri uri)
		{
			if (uri == null)
			{
				throw new ArgumentNullException("uri");
			}
			this.uri = uri;
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000055 RID: 85 RVA: 0x00002BDD File Offset: 0x00000DDD
		// (set) Token: 0x06000056 RID: 86 RVA: 0x00002BE5 File Offset: 0x00000DE5
		public DecompressionMethods AutomaticDecompression
		{
			get
			{
				return this.automaticDecompression;
			}
			set
			{
				this.automaticDecompression = value;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000057 RID: 87 RVA: 0x00002BEE File Offset: 0x00000DEE
		// (set) Token: 0x06000058 RID: 88 RVA: 0x00002BF6 File Offset: 0x00000DF6
		public CookieContainer CookieContainer
		{
			get
			{
				return this.cookieContainer;
			}
			set
			{
				this.cookieContainer = value;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000059 RID: 89 RVA: 0x00002BFF File Offset: 0x00000DFF
		// (set) Token: 0x0600005A RID: 90 RVA: 0x00002C07 File Offset: 0x00000E07
		public ICredentials Credentials
		{
			get
			{
				return this.credentials;
			}
			set
			{
				this.credentials = value;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600005B RID: 91 RVA: 0x00002C10 File Offset: 0x00000E10
		// (set) Token: 0x0600005C RID: 92 RVA: 0x00002C18 File Offset: 0x00000E18
		public string Method
		{
			get
			{
				return this.method;
			}
			set
			{
				this.method = value;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600005D RID: 93 RVA: 0x00002C21 File Offset: 0x00000E21
		// (set) Token: 0x0600005E RID: 94 RVA: 0x00002C29 File Offset: 0x00000E29
		public Version ProtocolVersion
		{
			get
			{
				return this.protocolVersion;
			}
			set
			{
				this.protocolVersion = value;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600005F RID: 95 RVA: 0x00002C32 File Offset: 0x00000E32
		// (set) Token: 0x06000060 RID: 96 RVA: 0x00002C3A File Offset: 0x00000E3A
		public bool PreAuthenticate
		{
			get
			{
				return this.preAuthenticate;
			}
			set
			{
				this.preAuthenticate = value;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000061 RID: 97 RVA: 0x00002C43 File Offset: 0x00000E43
		// (set) Token: 0x06000062 RID: 98 RVA: 0x00002C4B File Offset: 0x00000E4B
		public IWebProxy Proxy
		{
			get
			{
				return this.proxy;
			}
			set
			{
				this.proxy = value;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000063 RID: 99 RVA: 0x00002C54 File Offset: 0x00000E54
		// (set) Token: 0x06000064 RID: 100 RVA: 0x00002C5C File Offset: 0x00000E5C
		public int ReadWriteTimeout
		{
			get
			{
				return this.readWriteTimeout;
			}
			set
			{
				this.readWriteTimeout = value;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000065 RID: 101 RVA: 0x00002C65 File Offset: 0x00000E65
		public Uri RequestUri
		{
			get
			{
				return this.uri;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000066 RID: 102 RVA: 0x00002C6D File Offset: 0x00000E6D
		// (set) Token: 0x06000067 RID: 103 RVA: 0x00002C75 File Offset: 0x00000E75
		public int Timeout
		{
			get
			{
				return this.timeout;
			}
			set
			{
				this.timeout = value;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000068 RID: 104 RVA: 0x00002C7E File Offset: 0x00000E7E
		// (set) Token: 0x06000069 RID: 105 RVA: 0x00002C86 File Offset: 0x00000E86
		public bool UseDefaultCredentials
		{
			get
			{
				return this.useDefaultCredentials;
			}
			set
			{
				this.useDefaultCredentials = value;
			}
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00002C90 File Offset: 0x00000E90
		public HttpWebRequest Create()
		{
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(this.uri);
			httpWebRequest.AutomaticDecompression = this.automaticDecompression;
			httpWebRequest.CookieContainer = this.cookieContainer;
			httpWebRequest.Credentials = this.credentials;
			httpWebRequest.Method = this.method;
			httpWebRequest.ProtocolVersion = this.protocolVersion;
			httpWebRequest.PreAuthenticate = this.preAuthenticate;
			httpWebRequest.Proxy = this.proxy;
			httpWebRequest.ReadWriteTimeout = this.ReadWriteTimeout;
			httpWebRequest.Timeout = this.timeout;
			httpWebRequest.UseDefaultCredentials = this.useDefaultCredentials;
			return httpWebRequest;
		}

		// Token: 0x0400002D RID: 45
		private readonly Uri uri;

		// Token: 0x0400002E RID: 46
		private IWebProxy proxy = WebRequest.DefaultWebProxy;

		// Token: 0x0400002F RID: 47
		private ICredentials credentials;

		// Token: 0x04000030 RID: 48
		private DecompressionMethods automaticDecompression;

		// Token: 0x04000031 RID: 49
		private CookieContainer cookieContainer;

		// Token: 0x04000032 RID: 50
		private Version protocolVersion = HttpVersion.Version11;

		// Token: 0x04000033 RID: 51
		private string method = "GET";

		// Token: 0x04000034 RID: 52
		private bool preAuthenticate;

		// Token: 0x04000035 RID: 53
		private bool useDefaultCredentials;

		// Token: 0x04000036 RID: 54
		private int timeout = 10000;

		// Token: 0x04000037 RID: 55
		private int readWriteTimeout = 60000;
	}
}
