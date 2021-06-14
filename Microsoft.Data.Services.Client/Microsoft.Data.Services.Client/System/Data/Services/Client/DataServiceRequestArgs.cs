using System;
using System.Collections.Generic;

namespace System.Data.Services.Client
{
	// Token: 0x020000FD RID: 253
	public class DataServiceRequestArgs
	{
		// Token: 0x0600083D RID: 2109 RVA: 0x00022FD4 File Offset: 0x000211D4
		public DataServiceRequestArgs()
		{
			this.HeaderCollection = new HeaderCollection();
		}

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x0600083E RID: 2110 RVA: 0x00022FE7 File Offset: 0x000211E7
		// (set) Token: 0x0600083F RID: 2111 RVA: 0x00022FF9 File Offset: 0x000211F9
		public string AcceptContentType
		{
			get
			{
				return this.HeaderCollection.GetHeader("Accept");
			}
			set
			{
				this.HeaderCollection.SetHeader("Accept", value);
			}
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x06000840 RID: 2112 RVA: 0x0002300C File Offset: 0x0002120C
		// (set) Token: 0x06000841 RID: 2113 RVA: 0x0002301E File Offset: 0x0002121E
		public string ContentType
		{
			get
			{
				return this.HeaderCollection.GetHeader("Content-Type");
			}
			set
			{
				this.HeaderCollection.SetHeader("Content-Type", value);
			}
		}

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x06000842 RID: 2114 RVA: 0x00023031 File Offset: 0x00021231
		// (set) Token: 0x06000843 RID: 2115 RVA: 0x00023043 File Offset: 0x00021243
		public string Slug
		{
			get
			{
				return this.HeaderCollection.GetHeader("Slug");
			}
			set
			{
				this.HeaderCollection.SetHeader("Slug", value);
			}
		}

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x06000844 RID: 2116 RVA: 0x00023056 File Offset: 0x00021256
		// (set) Token: 0x06000845 RID: 2117 RVA: 0x00023068 File Offset: 0x00021268
		public Dictionary<string, string> Headers
		{
			get
			{
				return (Dictionary<string, string>)this.HeaderCollection.UnderlyingDictionary;
			}
			internal set
			{
				this.HeaderCollection = new HeaderCollection(value);
			}
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x06000846 RID: 2118 RVA: 0x00023076 File Offset: 0x00021276
		// (set) Token: 0x06000847 RID: 2119 RVA: 0x0002307E File Offset: 0x0002127E
		internal HeaderCollection HeaderCollection { get; private set; }
	}
}
