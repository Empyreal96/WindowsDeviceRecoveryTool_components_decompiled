using System;

namespace Microsoft.Data.OData
{
	// Token: 0x0200011A RID: 282
	public sealed class ODataEntityReferenceLinksSerializationInfo
	{
		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x06000798 RID: 1944 RVA: 0x00019B71 File Offset: 0x00017D71
		// (set) Token: 0x06000799 RID: 1945 RVA: 0x00019B79 File Offset: 0x00017D79
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

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x0600079A RID: 1946 RVA: 0x00019B8D File Offset: 0x00017D8D
		// (set) Token: 0x0600079B RID: 1947 RVA: 0x00019B95 File Offset: 0x00017D95
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

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x0600079C RID: 1948 RVA: 0x00019BA9 File Offset: 0x00017DA9
		// (set) Token: 0x0600079D RID: 1949 RVA: 0x00019BB1 File Offset: 0x00017DB1
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

		// Token: 0x0600079E RID: 1950 RVA: 0x00019BC5 File Offset: 0x00017DC5
		internal static ODataEntityReferenceLinksSerializationInfo Validate(ODataEntityReferenceLinksSerializationInfo serializationInfo)
		{
			if (serializationInfo != null)
			{
				ExceptionUtils.CheckArgumentNotNull<string>(serializationInfo.SourceEntitySetName, "serializationInfo.SourceEntitySetName");
				ExceptionUtils.CheckArgumentNotNull<string>(serializationInfo.NavigationPropertyName, "serializationInfo.NavigationPropertyName");
			}
			return serializationInfo;
		}

		// Token: 0x040002D7 RID: 727
		private string sourceEntitySetName;

		// Token: 0x040002D8 RID: 728
		private string typecast;

		// Token: 0x040002D9 RID: 729
		private string navigationPropertyName;
	}
}
