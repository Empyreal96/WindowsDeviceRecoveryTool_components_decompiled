using System;

namespace Microsoft.Data.OData
{
	// Token: 0x0200011E RID: 286
	public sealed class ODataFeedAndEntrySerializationInfo
	{
		// Token: 0x170001EE RID: 494
		// (get) Token: 0x060007AD RID: 1965 RVA: 0x00019C9F File Offset: 0x00017E9F
		// (set) Token: 0x060007AE RID: 1966 RVA: 0x00019CA7 File Offset: 0x00017EA7
		public string EntitySetName
		{
			get
			{
				return this.entitySetName;
			}
			set
			{
				ExceptionUtils.CheckArgumentStringNotNullOrEmpty(value, "EntitySetName");
				this.entitySetName = value;
			}
		}

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x060007AF RID: 1967 RVA: 0x00019CBB File Offset: 0x00017EBB
		// (set) Token: 0x060007B0 RID: 1968 RVA: 0x00019CC3 File Offset: 0x00017EC3
		public string EntitySetElementTypeName
		{
			get
			{
				return this.entitySetElementTypeName;
			}
			set
			{
				ExceptionUtils.CheckArgumentStringNotNullOrEmpty(value, "EntitySetElementTypeName");
				this.entitySetElementTypeName = value;
			}
		}

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x060007B1 RID: 1969 RVA: 0x00019CD7 File Offset: 0x00017ED7
		// (set) Token: 0x060007B2 RID: 1970 RVA: 0x00019CE9 File Offset: 0x00017EE9
		public string ExpectedTypeName
		{
			get
			{
				return this.expectedTypeName ?? this.EntitySetElementTypeName;
			}
			set
			{
				ExceptionUtils.CheckArgumentStringNotEmpty(value, "ExpectedTypeName");
				this.expectedTypeName = value;
			}
		}

		// Token: 0x060007B3 RID: 1971 RVA: 0x00019CFD File Offset: 0x00017EFD
		internal static ODataFeedAndEntrySerializationInfo Validate(ODataFeedAndEntrySerializationInfo serializationInfo)
		{
			if (serializationInfo != null)
			{
				ExceptionUtils.CheckArgumentNotNull<string>(serializationInfo.EntitySetName, "serializationInfo.EntitySetName");
				ExceptionUtils.CheckArgumentNotNull<string>(serializationInfo.EntitySetElementTypeName, "serializationInfo.EntitySetElementTypeName");
			}
			return serializationInfo;
		}

		// Token: 0x040002E4 RID: 740
		private string entitySetName;

		// Token: 0x040002E5 RID: 741
		private string entitySetElementTypeName;

		// Token: 0x040002E6 RID: 742
		private string expectedTypeName;
	}
}
