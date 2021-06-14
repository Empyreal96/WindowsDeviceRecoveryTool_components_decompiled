using System;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.Query.SemanticAst;

namespace Microsoft.Data.OData.Query
{
	// Token: 0x0200000C RID: 12
	internal sealed class ODataUriParserConfiguration
	{
		// Token: 0x06000037 RID: 55 RVA: 0x00002970 File Offset: 0x00000B70
		public ODataUriParserConfiguration(IEdmModel model, Uri serviceRoot)
		{
			ExceptionUtils.CheckArgumentNotNull<IEdmModel>(model, "model");
			if (serviceRoot != null && !serviceRoot.IsAbsoluteUri)
			{
				throw new ArgumentException(Strings.UriParser_UriMustBeAbsolute(serviceRoot), "serviceRoot");
			}
			this.model = model;
			this.serviceRoot = serviceRoot;
			this.Settings = new ODataUriParserSettings();
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000038 RID: 56 RVA: 0x000029D4 File Offset: 0x00000BD4
		// (set) Token: 0x06000039 RID: 57 RVA: 0x000029DC File Offset: 0x00000BDC
		public ODataUriParserSettings Settings { get; private set; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600003A RID: 58 RVA: 0x000029E5 File Offset: 0x00000BE5
		public IEdmModel Model
		{
			get
			{
				return this.model;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600003B RID: 59 RVA: 0x000029ED File Offset: 0x00000BED
		public Uri ServiceRoot
		{
			get
			{
				return this.serviceRoot;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600003C RID: 60 RVA: 0x000029F5 File Offset: 0x00000BF5
		// (set) Token: 0x0600003D RID: 61 RVA: 0x000029FD File Offset: 0x00000BFD
		public ODataUrlConventions UrlConventions
		{
			get
			{
				return this.urlConventions;
			}
			set
			{
				ExceptionUtils.CheckArgumentNotNull<ODataUrlConventions>(value, "UrlConventions");
				this.urlConventions = value;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600003E RID: 62 RVA: 0x00002A11 File Offset: 0x00000C11
		// (set) Token: 0x0600003F RID: 63 RVA: 0x00002A19 File Offset: 0x00000C19
		public Func<string, BatchReferenceSegment> BatchReferenceCallback { get; set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000040 RID: 64 RVA: 0x00002A22 File Offset: 0x00000C22
		// (set) Token: 0x06000041 RID: 65 RVA: 0x00002A2A File Offset: 0x00000C2A
		public Func<string, string> FunctionParameterAliasCallback { get; set; }

		// Token: 0x04000012 RID: 18
		private readonly IEdmModel model;

		// Token: 0x04000013 RID: 19
		private readonly Uri serviceRoot;

		// Token: 0x04000014 RID: 20
		private ODataUrlConventions urlConventions = ODataUrlConventions.Default;
	}
}
