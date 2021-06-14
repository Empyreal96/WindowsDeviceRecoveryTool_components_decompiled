using System;

namespace System.Data.Services.Client
{
	// Token: 0x020000F1 RID: 241
	public sealed class EntityChangedParams
	{
		// Token: 0x060007F5 RID: 2037 RVA: 0x00022310 File Offset: 0x00020510
		internal EntityChangedParams(DataServiceContext context, object entity, string propertyName, object propertyValue, string sourceEntitySet, string targetEntitySet)
		{
			this.context = context;
			this.entity = entity;
			this.propertyName = propertyName;
			this.propertyValue = propertyValue;
			this.sourceEntitySet = sourceEntitySet;
			this.targetEntitySet = targetEntitySet;
		}

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x060007F6 RID: 2038 RVA: 0x00022345 File Offset: 0x00020545
		public DataServiceContext Context
		{
			get
			{
				return this.context;
			}
		}

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x060007F7 RID: 2039 RVA: 0x0002234D File Offset: 0x0002054D
		public object Entity
		{
			get
			{
				return this.entity;
			}
		}

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x060007F8 RID: 2040 RVA: 0x00022355 File Offset: 0x00020555
		public string PropertyName
		{
			get
			{
				return this.propertyName;
			}
		}

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x060007F9 RID: 2041 RVA: 0x0002235D File Offset: 0x0002055D
		public object PropertyValue
		{
			get
			{
				return this.propertyValue;
			}
		}

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x060007FA RID: 2042 RVA: 0x00022365 File Offset: 0x00020565
		public string SourceEntitySet
		{
			get
			{
				return this.sourceEntitySet;
			}
		}

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x060007FB RID: 2043 RVA: 0x0002236D File Offset: 0x0002056D
		public string TargetEntitySet
		{
			get
			{
				return this.targetEntitySet;
			}
		}

		// Token: 0x040004C5 RID: 1221
		private readonly DataServiceContext context;

		// Token: 0x040004C6 RID: 1222
		private readonly object entity;

		// Token: 0x040004C7 RID: 1223
		private readonly string propertyName;

		// Token: 0x040004C8 RID: 1224
		private readonly object propertyValue;

		// Token: 0x040004C9 RID: 1225
		private readonly string sourceEntitySet;

		// Token: 0x040004CA RID: 1226
		private readonly string targetEntitySet;
	}
}
