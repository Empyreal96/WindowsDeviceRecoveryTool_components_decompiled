using System;

namespace Microsoft.Data.OData
{
	// Token: 0x02000119 RID: 281
	public sealed class ODataCollectionStartSerializationInfo
	{
		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x06000794 RID: 1940 RVA: 0x00019B30 File Offset: 0x00017D30
		// (set) Token: 0x06000795 RID: 1941 RVA: 0x00019B38 File Offset: 0x00017D38
		public string CollectionTypeName
		{
			get
			{
				return this.collectionTypeName;
			}
			set
			{
				ExceptionUtils.CheckArgumentStringNotNullOrEmpty(value, "CollectionTypeName");
				ValidationUtils.ValidateCollectionTypeName(value);
				this.collectionTypeName = value;
			}
		}

		// Token: 0x06000796 RID: 1942 RVA: 0x00019B53 File Offset: 0x00017D53
		internal static ODataCollectionStartSerializationInfo Validate(ODataCollectionStartSerializationInfo serializationInfo)
		{
			if (serializationInfo != null)
			{
				ExceptionUtils.CheckArgumentNotNull<string>(serializationInfo.CollectionTypeName, "serializationInfo.CollectionTypeName");
			}
			return serializationInfo;
		}

		// Token: 0x040002D6 RID: 726
		private string collectionTypeName;
	}
}
