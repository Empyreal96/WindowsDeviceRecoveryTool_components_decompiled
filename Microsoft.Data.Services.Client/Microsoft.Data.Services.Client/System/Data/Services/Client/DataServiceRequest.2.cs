using System;

namespace System.Data.Services.Client
{
	// Token: 0x0200010B RID: 267
	public sealed class DataServiceRequest<TElement> : DataServiceRequest
	{
		// Token: 0x060008B0 RID: 2224 RVA: 0x0002442A File Offset: 0x0002262A
		public DataServiceRequest(Uri requestUri)
		{
			Util.CheckArgumentNull<Uri>(requestUri, "requestUri");
			this.requestUri = requestUri;
		}

		// Token: 0x060008B1 RID: 2225 RVA: 0x00024445 File Offset: 0x00022645
		internal DataServiceRequest(Uri requestUri, QueryComponents queryComponents, ProjectionPlan plan) : this(requestUri)
		{
			this.queryComponents = queryComponents;
			this.plan = plan;
		}

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x060008B2 RID: 2226 RVA: 0x0002445C File Offset: 0x0002265C
		public override Type ElementType
		{
			get
			{
				return typeof(TElement);
			}
		}

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x060008B3 RID: 2227 RVA: 0x00024468 File Offset: 0x00022668
		// (set) Token: 0x060008B4 RID: 2228 RVA: 0x00024470 File Offset: 0x00022670
		public override Uri RequestUri
		{
			get
			{
				return this.requestUri;
			}
			internal set
			{
				this.requestUri = value;
			}
		}

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x060008B5 RID: 2229 RVA: 0x00024479 File Offset: 0x00022679
		internal override ProjectionPlan Plan
		{
			get
			{
				return this.plan;
			}
		}

		// Token: 0x060008B6 RID: 2230 RVA: 0x00024481 File Offset: 0x00022681
		public override string ToString()
		{
			return this.requestUri.ToString();
		}

		// Token: 0x060008B7 RID: 2231 RVA: 0x00024490 File Offset: 0x00022690
		internal override QueryComponents QueryComponents(ClientEdmModel model)
		{
			if (this.queryComponents == null)
			{
				Type type = typeof(TElement);
				type = ((PrimitiveType.IsKnownType(type) || WebUtil.IsCLRTypeCollection(type, model)) ? type : TypeSystem.GetElementType(type));
				this.queryComponents = new QueryComponents(this.requestUri, Util.DataServiceVersionEmpty, type, null, null);
			}
			return this.queryComponents;
		}

		// Token: 0x0400050C RID: 1292
		private readonly ProjectionPlan plan;

		// Token: 0x0400050D RID: 1293
		private Uri requestUri;

		// Token: 0x0400050E RID: 1294
		private QueryComponents queryComponents;
	}
}
