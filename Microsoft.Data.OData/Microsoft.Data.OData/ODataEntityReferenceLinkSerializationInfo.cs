using System;

namespace Microsoft.Data.OData
{
	// Token: 0x0200011B RID: 283
	public sealed class ODataEntityReferenceLinkSerializationInfo
	{
		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x060007A0 RID: 1952 RVA: 0x00019BF3 File Offset: 0x00017DF3
		// (set) Token: 0x060007A1 RID: 1953 RVA: 0x00019BFB File Offset: 0x00017DFB
		public string SourceEntitySetName
		{
			get
			{
				return this.sourceEntitySetName;
			}
			set
			{
				ExceptionUtils.CheckArgumentStringNotNullOrEmpty(value, "SourceEntitySetName");
				this.sourceEntitySetName = value;
			}
		}

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x060007A2 RID: 1954 RVA: 0x00019C0F File Offset: 0x00017E0F
		// (set) Token: 0x060007A3 RID: 1955 RVA: 0x00019C17 File Offset: 0x00017E17
		public string Typecast
		{
			get
			{
				return this.typecast;
			}
			set
			{
				ExceptionUtils.CheckArgumentStringNotEmpty(value, "Typecast");
				this.typecast = value;
			}
		}

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x060007A4 RID: 1956 RVA: 0x00019C2B File Offset: 0x00017E2B
		// (set) Token: 0x060007A5 RID: 1957 RVA: 0x00019C33 File Offset: 0x00017E33
		public string NavigationPropertyName
		{
			get
			{
				return this.navigationPropertyName;
			}
			set
			{
				ExceptionUtils.CheckArgumentStringNotNullOrEmpty(value, "NavigationPropertyName");
				this.navigationPropertyName = value;
			}
		}

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x060007A6 RID: 1958 RVA: 0x00019C47 File Offset: 0x00017E47
		// (set) Token: 0x060007A7 RID: 1959 RVA: 0x00019C4F File Offset: 0x00017E4F
		public bool IsCollectionNavigationProperty { get; set; }

		// Token: 0x060007A8 RID: 1960 RVA: 0x00019C58 File Offset: 0x00017E58
		internal static ODataEntityReferenceLinkSerializationInfo Validate(ODataEntityReferenceLinkSerializationInfo serializationInfo)
		{
			if (serializationInfo != null)
			{
				ExceptionUtils.CheckArgumentNotNull<string>(serializationInfo.SourceEntitySetName, "serializationInfo.SourceEntitySetName");
				ExceptionUtils.CheckArgumentNotNull<string>(serializationInfo.NavigationPropertyName, "serializationInfo.NavigationPropertyName");
			}
			return serializationInfo;
		}

		// Token: 0x040002DA RID: 730
		private string sourceEntitySetName;

		// Token: 0x040002DB RID: 731
		private string typecast;

		// Token: 0x040002DC RID: 732
		private string navigationPropertyName;
	}
}
