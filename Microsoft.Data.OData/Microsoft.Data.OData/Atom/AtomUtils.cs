using System;

namespace Microsoft.Data.OData.Atom
{
	// Token: 0x02000280 RID: 640
	internal static class AtomUtils
	{
		// Token: 0x06001546 RID: 5446 RVA: 0x0004DAF0 File Offset: 0x0004BCF0
		internal static string ComputeODataNavigationLinkRelation(ODataNavigationLink navigationLink)
		{
			return string.Join("/", new string[]
			{
				"http://schemas.microsoft.com/ado/2007/08/dataservices",
				"related",
				navigationLink.Name
			});
		}

		// Token: 0x06001547 RID: 5447 RVA: 0x0004DB28 File Offset: 0x0004BD28
		internal static string ComputeODataNavigationLinkType(ODataNavigationLink navigationLink)
		{
			if (!navigationLink.IsCollection.Value)
			{
				return "application/atom+xml;type=entry";
			}
			return "application/atom+xml;type=feed";
		}

		// Token: 0x06001548 RID: 5448 RVA: 0x0004DB50 File Offset: 0x0004BD50
		internal static string ComputeODataAssociationLinkRelation(ODataAssociationLink associationLink)
		{
			return string.Join("/", new string[]
			{
				"http://schemas.microsoft.com/ado/2007/08/dataservices",
				"relatedlinks",
				associationLink.Name
			});
		}

		// Token: 0x06001549 RID: 5449 RVA: 0x0004DB88 File Offset: 0x0004BD88
		internal static string ComputeStreamPropertyRelation(ODataProperty streamProperty, bool forEditLink)
		{
			string text = forEditLink ? "edit-media" : "mediaresource";
			return string.Join("/", new string[]
			{
				"http://schemas.microsoft.com/ado/2007/08/dataservices",
				text,
				streamProperty.Name
			});
		}

		// Token: 0x0600154A RID: 5450 RVA: 0x0004DBCC File Offset: 0x0004BDCC
		internal static string UnescapeAtomLinkRelationAttribute(string relation)
		{
			Uri uri;
			if (!string.IsNullOrEmpty(relation) && Uri.TryCreate(relation, UriKind.RelativeOrAbsolute, out uri) && uri.IsAbsoluteUri)
			{
				return uri.GetComponents(UriComponents.AbsoluteUri, UriFormat.SafeUnescaped);
			}
			return null;
		}

		// Token: 0x0600154B RID: 5451 RVA: 0x0004DBFF File Offset: 0x0004BDFF
		internal static string GetNameFromAtomLinkRelationAttribute(string relation, string namespacePrefix)
		{
			if (relation != null && relation.StartsWith(namespacePrefix, StringComparison.Ordinal))
			{
				return relation.Substring(namespacePrefix.Length);
			}
			return null;
		}

		// Token: 0x0600154C RID: 5452 RVA: 0x0004DC1C File Offset: 0x0004BE1C
		internal static bool IsExactNavigationLinkTypeMatch(string navigationLinkType, out bool hasEntryType, out bool hasFeedType)
		{
			hasEntryType = false;
			hasFeedType = false;
			if (!navigationLinkType.StartsWith("application/atom+xml", StringComparison.Ordinal))
			{
				return false;
			}
			int length = navigationLinkType.Length;
			int num = length;
			switch (num)
			{
			case 20:
				return true;
			case 21:
				return navigationLinkType[length - 1] == ';';
			default:
				switch (num)
				{
				case 30:
					hasFeedType = (string.Compare(";type=feed", 0, navigationLinkType, 20, ";type=feed".Length, StringComparison.Ordinal) == 0);
					return hasFeedType;
				case 31:
					hasEntryType = (string.Compare(";type=entry", 0, navigationLinkType, 20, ";type=entry".Length, StringComparison.Ordinal) == 0);
					return hasEntryType;
				default:
					return false;
				}
				break;
			}
		}

		// Token: 0x040007C4 RID: 1988
		private const int MimeApplicationAtomXmlLength = 20;

		// Token: 0x040007C5 RID: 1989
		private const int MimeApplicationAtomXmlLengthWithSemicolon = 21;

		// Token: 0x040007C6 RID: 1990
		private const int MimeApplicationAtomXmlTypeEntryLength = 31;

		// Token: 0x040007C7 RID: 1991
		private const int MimeApplicationAtomXmlTypeFeedLength = 30;

		// Token: 0x040007C8 RID: 1992
		private const string MimeApplicationAtomXmlTypeEntryParameter = ";type=entry";

		// Token: 0x040007C9 RID: 1993
		private const string MimeApplicationAtomXmlTypeFeedParameter = ";type=feed";
	}
}
