using System;

namespace Microsoft.Data.OData.Atom
{
	// Token: 0x02000283 RID: 643
	public static class ExtensionMethods
	{
		// Token: 0x0600155F RID: 5471 RVA: 0x0004DEC8 File Offset: 0x0004C0C8
		public static AtomEntryMetadata Atom(this ODataEntry entry)
		{
			ExceptionUtils.CheckArgumentNotNull<ODataEntry>(entry, "entry");
			AtomEntryMetadata atomEntryMetadata = entry.GetAnnotation<AtomEntryMetadata>();
			if (atomEntryMetadata == null)
			{
				atomEntryMetadata = new AtomEntryMetadata();
				entry.SetAnnotation<AtomEntryMetadata>(atomEntryMetadata);
			}
			return atomEntryMetadata;
		}

		// Token: 0x06001560 RID: 5472 RVA: 0x0004DEF8 File Offset: 0x0004C0F8
		public static AtomFeedMetadata Atom(this ODataFeed feed)
		{
			ExceptionUtils.CheckArgumentNotNull<ODataFeed>(feed, "feed");
			AtomFeedMetadata atomFeedMetadata = feed.GetAnnotation<AtomFeedMetadata>();
			if (atomFeedMetadata == null)
			{
				atomFeedMetadata = new AtomFeedMetadata();
				feed.SetAnnotation<AtomFeedMetadata>(atomFeedMetadata);
			}
			return atomFeedMetadata;
		}

		// Token: 0x06001561 RID: 5473 RVA: 0x0004DF28 File Offset: 0x0004C128
		public static AtomLinkMetadata Atom(this ODataNavigationLink navigationLink)
		{
			ExceptionUtils.CheckArgumentNotNull<ODataNavigationLink>(navigationLink, "navigationLink");
			AtomLinkMetadata atomLinkMetadata = navigationLink.GetAnnotation<AtomLinkMetadata>();
			if (atomLinkMetadata == null)
			{
				atomLinkMetadata = new AtomLinkMetadata();
				navigationLink.SetAnnotation<AtomLinkMetadata>(atomLinkMetadata);
			}
			return atomLinkMetadata;
		}

		// Token: 0x06001562 RID: 5474 RVA: 0x0004DF58 File Offset: 0x0004C158
		public static AtomWorkspaceMetadata Atom(this ODataWorkspace workspace)
		{
			ExceptionUtils.CheckArgumentNotNull<ODataWorkspace>(workspace, "workspace");
			AtomWorkspaceMetadata atomWorkspaceMetadata = workspace.GetAnnotation<AtomWorkspaceMetadata>();
			if (atomWorkspaceMetadata == null)
			{
				atomWorkspaceMetadata = new AtomWorkspaceMetadata();
				workspace.SetAnnotation<AtomWorkspaceMetadata>(atomWorkspaceMetadata);
			}
			return atomWorkspaceMetadata;
		}

		// Token: 0x06001563 RID: 5475 RVA: 0x0004DF88 File Offset: 0x0004C188
		public static AtomResourceCollectionMetadata Atom(this ODataResourceCollectionInfo collection)
		{
			ExceptionUtils.CheckArgumentNotNull<ODataResourceCollectionInfo>(collection, "collection");
			AtomResourceCollectionMetadata atomResourceCollectionMetadata = collection.GetAnnotation<AtomResourceCollectionMetadata>();
			if (atomResourceCollectionMetadata == null)
			{
				atomResourceCollectionMetadata = new AtomResourceCollectionMetadata();
				collection.SetAnnotation<AtomResourceCollectionMetadata>(atomResourceCollectionMetadata);
			}
			return atomResourceCollectionMetadata;
		}

		// Token: 0x06001564 RID: 5476 RVA: 0x0004DFB8 File Offset: 0x0004C1B8
		public static AtomLinkMetadata Atom(this ODataAssociationLink associationLink)
		{
			ExceptionUtils.CheckArgumentNotNull<ODataAssociationLink>(associationLink, "associationLink");
			AtomLinkMetadata atomLinkMetadata = associationLink.GetAnnotation<AtomLinkMetadata>();
			if (atomLinkMetadata == null)
			{
				atomLinkMetadata = new AtomLinkMetadata();
				associationLink.SetAnnotation<AtomLinkMetadata>(atomLinkMetadata);
			}
			return atomLinkMetadata;
		}
	}
}
