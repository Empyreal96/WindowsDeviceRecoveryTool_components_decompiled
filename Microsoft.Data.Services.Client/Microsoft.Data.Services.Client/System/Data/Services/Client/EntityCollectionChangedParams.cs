using System;
using System.Collections;
using System.Collections.Specialized;

namespace System.Data.Services.Client
{
	// Token: 0x020000F2 RID: 242
	public sealed class EntityCollectionChangedParams
	{
		// Token: 0x060007FC RID: 2044 RVA: 0x00022378 File Offset: 0x00020578
		internal EntityCollectionChangedParams(DataServiceContext context, object sourceEntity, string propertyName, string sourceEntitySet, ICollection collection, object targetEntity, string targetEntitySet, NotifyCollectionChangedAction action)
		{
			this.context = context;
			this.sourceEntity = sourceEntity;
			this.propertyName = propertyName;
			this.sourceEntitySet = sourceEntitySet;
			this.collection = collection;
			this.targetEntity = targetEntity;
			this.targetEntitySet = targetEntitySet;
			this.action = action;
		}

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x060007FD RID: 2045 RVA: 0x000223C8 File Offset: 0x000205C8
		public DataServiceContext Context
		{
			get
			{
				return this.context;
			}
		}

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x060007FE RID: 2046 RVA: 0x000223D0 File Offset: 0x000205D0
		public object SourceEntity
		{
			get
			{
				return this.sourceEntity;
			}
		}

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x060007FF RID: 2047 RVA: 0x000223D8 File Offset: 0x000205D8
		public string PropertyName
		{
			get
			{
				return this.propertyName;
			}
		}

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x06000800 RID: 2048 RVA: 0x000223E0 File Offset: 0x000205E0
		public string SourceEntitySet
		{
			get
			{
				return this.sourceEntitySet;
			}
		}

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x06000801 RID: 2049 RVA: 0x000223E8 File Offset: 0x000205E8
		public object TargetEntity
		{
			get
			{
				return this.targetEntity;
			}
		}

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x06000802 RID: 2050 RVA: 0x000223F0 File Offset: 0x000205F0
		public string TargetEntitySet
		{
			get
			{
				return this.targetEntitySet;
			}
		}

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x06000803 RID: 2051 RVA: 0x000223F8 File Offset: 0x000205F8
		public ICollection Collection
		{
			get
			{
				return this.collection;
			}
		}

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x06000804 RID: 2052 RVA: 0x00022400 File Offset: 0x00020600
		public NotifyCollectionChangedAction Action
		{
			get
			{
				return this.action;
			}
		}

		// Token: 0x040004CB RID: 1227
		private readonly DataServiceContext context;

		// Token: 0x040004CC RID: 1228
		private readonly object sourceEntity;

		// Token: 0x040004CD RID: 1229
		private readonly string propertyName;

		// Token: 0x040004CE RID: 1230
		private readonly string sourceEntitySet;

		// Token: 0x040004CF RID: 1231
		private readonly ICollection collection;

		// Token: 0x040004D0 RID: 1232
		private readonly object targetEntity;

		// Token: 0x040004D1 RID: 1233
		private readonly string targetEntitySet;

		// Token: 0x040004D2 RID: 1234
		private readonly NotifyCollectionChangedAction action;
	}
}
