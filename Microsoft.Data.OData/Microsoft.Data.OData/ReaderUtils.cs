using System;
using System.Linq;
using Microsoft.Data.Edm;

namespace Microsoft.Data.OData
{
	// Token: 0x02000240 RID: 576
	internal static class ReaderUtils
	{
		// Token: 0x06001265 RID: 4709 RVA: 0x00044F2C File Offset: 0x0004312C
		internal static ODataEntry CreateNewEntry()
		{
			return new ODataEntry
			{
				Properties = new ReadOnlyEnumerable<ODataProperty>(),
				AssociationLinks = ReadOnlyEnumerable<ODataAssociationLink>.Empty(),
				Actions = ReadOnlyEnumerable<ODataAction>.Empty(),
				Functions = ReadOnlyEnumerable<ODataFunction>.Empty()
			};
		}

		// Token: 0x06001266 RID: 4710 RVA: 0x00044F6C File Offset: 0x0004316C
		internal static void CheckForDuplicateNavigationLinkNameAndSetAssociationLink(DuplicatePropertyNamesChecker duplicatePropertyNamesChecker, ODataNavigationLink navigationLink, bool isExpanded, bool? isCollection)
		{
			ODataAssociationLink odataAssociationLink = duplicatePropertyNamesChecker.CheckForDuplicatePropertyNames(navigationLink, isExpanded, isCollection);
			if (odataAssociationLink != null && odataAssociationLink.Url != null && navigationLink.AssociationLinkUrl == null)
			{
				navigationLink.AssociationLinkUrl = odataAssociationLink.Url;
			}
		}

		// Token: 0x06001267 RID: 4711 RVA: 0x00044FB0 File Offset: 0x000431B0
		internal static void CheckForDuplicateAssociationLinkAndUpdateNavigationLink(DuplicatePropertyNamesChecker duplicatePropertyNamesChecker, ODataAssociationLink associationLink)
		{
			ODataNavigationLink odataNavigationLink = duplicatePropertyNamesChecker.CheckForDuplicateAssociationLinkNames(associationLink);
			if (odataNavigationLink != null && odataNavigationLink.AssociationLinkUrl == null && associationLink.Url != null)
			{
				odataNavigationLink.AssociationLinkUrl = associationLink.Url;
			}
		}

		// Token: 0x06001268 RID: 4712 RVA: 0x00045010 File Offset: 0x00043210
		internal static ODataAssociationLink GetOrCreateAssociationLinkForNavigationProperty(ODataEntry entry, IEdmNavigationProperty navigationProperty)
		{
			ODataAssociationLink odataAssociationLink = entry.AssociationLinks.FirstOrDefault((ODataAssociationLink al) => al.Name == navigationProperty.Name);
			if (odataAssociationLink == null)
			{
				odataAssociationLink = new ODataAssociationLink
				{
					Name = navigationProperty.Name
				};
				entry.AddAssociationLink(odataAssociationLink);
			}
			return odataAssociationLink;
		}

		// Token: 0x06001269 RID: 4713 RVA: 0x00045066 File Offset: 0x00043266
		internal static bool HasFlag(this ODataUndeclaredPropertyBehaviorKinds undeclaredPropertyBehaviorKinds, ODataUndeclaredPropertyBehaviorKinds flag)
		{
			return (undeclaredPropertyBehaviorKinds & flag) == flag;
		}

		// Token: 0x0600126A RID: 4714 RVA: 0x0004506E File Offset: 0x0004326E
		internal static string GetExpectedPropertyName(IEdmStructuralProperty expectedProperty)
		{
			if (expectedProperty == null)
			{
				return null;
			}
			return expectedProperty.Name;
		}
	}
}
