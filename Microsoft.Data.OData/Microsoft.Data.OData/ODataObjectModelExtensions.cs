using System;

namespace Microsoft.Data.OData
{
	// Token: 0x02000118 RID: 280
	public static class ODataObjectModelExtensions
	{
		// Token: 0x0600078E RID: 1934 RVA: 0x00019AB8 File Offset: 0x00017CB8
		public static void SetSerializationInfo(this ODataFeed feed, ODataFeedAndEntrySerializationInfo serializationInfo)
		{
			ExceptionUtils.CheckArgumentNotNull<ODataFeed>(feed, "feed");
			feed.SerializationInfo = serializationInfo;
		}

		// Token: 0x0600078F RID: 1935 RVA: 0x00019ACC File Offset: 0x00017CCC
		public static void SetSerializationInfo(this ODataEntry entry, ODataFeedAndEntrySerializationInfo serializationInfo)
		{
			ExceptionUtils.CheckArgumentNotNull<ODataEntry>(entry, "entry");
			entry.SerializationInfo = serializationInfo;
		}

		// Token: 0x06000790 RID: 1936 RVA: 0x00019AE0 File Offset: 0x00017CE0
		public static void SetSerializationInfo(this ODataProperty property, ODataPropertySerializationInfo serializationInfo)
		{
			ExceptionUtils.CheckArgumentNotNull<ODataProperty>(property, "property");
			property.SerializationInfo = serializationInfo;
		}

		// Token: 0x06000791 RID: 1937 RVA: 0x00019AF4 File Offset: 0x00017CF4
		public static void SetSerializationInfo(this ODataCollectionStart collectionStart, ODataCollectionStartSerializationInfo serializationInfo)
		{
			ExceptionUtils.CheckArgumentNotNull<ODataCollectionStart>(collectionStart, "collectionStart");
			collectionStart.SerializationInfo = serializationInfo;
		}

		// Token: 0x06000792 RID: 1938 RVA: 0x00019B08 File Offset: 0x00017D08
		public static void SetSerializationInfo(this ODataEntityReferenceLink entityReferenceLink, ODataEntityReferenceLinkSerializationInfo serializationInfo)
		{
			ExceptionUtils.CheckArgumentNotNull<ODataEntityReferenceLink>(entityReferenceLink, "entityReferenceLink");
			entityReferenceLink.SerializationInfo = serializationInfo;
		}

		// Token: 0x06000793 RID: 1939 RVA: 0x00019B1C File Offset: 0x00017D1C
		public static void SetSerializationInfo(this ODataEntityReferenceLinks entityReferenceLinks, ODataEntityReferenceLinksSerializationInfo serializationInfo)
		{
			ExceptionUtils.CheckArgumentNotNull<ODataEntityReferenceLinks>(entityReferenceLinks, "entityReferenceLinks");
			entityReferenceLinks.SerializationInfo = serializationInfo;
		}
	}
}
