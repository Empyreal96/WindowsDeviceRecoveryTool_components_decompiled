using System;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.Metadata;

namespace Microsoft.Data.OData
{
	// Token: 0x0200028F RID: 655
	internal static class ODataVersionChecker
	{
		// Token: 0x06001615 RID: 5653 RVA: 0x00050837 File Offset: 0x0004EA37
		internal static void CheckCount(ODataVersion version)
		{
			if (version < ODataVersion.V2)
			{
				throw new ODataException(Strings.ODataVersionChecker_InlineCountNotSupported(ODataUtils.ODataVersionToString(version)));
			}
		}

		// Token: 0x06001616 RID: 5654 RVA: 0x0005084E File Offset: 0x0004EA4E
		internal static void CheckCollectionValueProperties(ODataVersion version, string propertyName)
		{
			if (version < ODataVersion.V3)
			{
				throw new ODataException(Strings.ODataVersionChecker_CollectionPropertiesNotSupported(propertyName, ODataUtils.ODataVersionToString(version)));
			}
		}

		// Token: 0x06001617 RID: 5655 RVA: 0x00050866 File Offset: 0x0004EA66
		internal static void CheckCollectionValue(ODataVersion version)
		{
			if (version < ODataVersion.V3)
			{
				throw new ODataException(Strings.ODataVersionChecker_CollectionNotSupported(ODataUtils.ODataVersionToString(version)));
			}
		}

		// Token: 0x06001618 RID: 5656 RVA: 0x0005087D File Offset: 0x0004EA7D
		internal static void CheckNextLink(ODataVersion version)
		{
			if (version < ODataVersion.V2)
			{
				throw new ODataException(Strings.ODataVersionChecker_NextLinkNotSupported(ODataUtils.ODataVersionToString(version)));
			}
		}

		// Token: 0x06001619 RID: 5657 RVA: 0x00050894 File Offset: 0x0004EA94
		internal static void CheckDeltaLink(ODataVersion version)
		{
			if (version < ODataVersion.V3)
			{
				throw new ODataException(Strings.ODataVersionChecker_DeltaLinkNotSupported(ODataUtils.ODataVersionToString(version)));
			}
		}

		// Token: 0x0600161A RID: 5658 RVA: 0x000508AB File Offset: 0x0004EAAB
		internal static void CheckStreamReferenceProperty(ODataVersion version)
		{
			if (version < ODataVersion.V3)
			{
				throw new ODataException(Strings.ODataVersionChecker_StreamPropertiesNotSupported(ODataUtils.ODataVersionToString(version)));
			}
		}

		// Token: 0x0600161B RID: 5659 RVA: 0x000508C2 File Offset: 0x0004EAC2
		internal static void CheckAssociationLinks(ODataVersion version)
		{
			if (version < ODataVersion.V3)
			{
				throw new ODataException(Strings.ODataVersionChecker_AssociationLinksNotSupported(ODataUtils.ODataVersionToString(version)));
			}
		}

		// Token: 0x0600161C RID: 5660 RVA: 0x000508D9 File Offset: 0x0004EAD9
		internal static void CheckCustomTypeScheme(ODataVersion version)
		{
			if (version > ODataVersion.V2)
			{
				throw new ODataException(Strings.ODataVersionChecker_PropertyNotSupportedForODataVersionGreaterThanX("TypeScheme", ODataUtils.ODataVersionToString(ODataVersion.V2)));
			}
		}

		// Token: 0x0600161D RID: 5661 RVA: 0x000508F5 File Offset: 0x0004EAF5
		internal static void CheckCustomDataNamespace(ODataVersion version)
		{
			if (version > ODataVersion.V2)
			{
				throw new ODataException(Strings.ODataVersionChecker_PropertyNotSupportedForODataVersionGreaterThanX("DataNamespace", ODataUtils.ODataVersionToString(ODataVersion.V2)));
			}
		}

		// Token: 0x0600161E RID: 5662 RVA: 0x00050911 File Offset: 0x0004EB11
		internal static void CheckParameterPayload(ODataVersion version)
		{
			if (version < ODataVersion.V3)
			{
				throw new ODataException(Strings.ODataVersionChecker_ParameterPayloadNotSupported(ODataUtils.ODataVersionToString(version)));
			}
		}

		// Token: 0x0600161F RID: 5663 RVA: 0x00050928 File Offset: 0x0004EB28
		internal static void CheckEntityPropertyMapping(ODataVersion version, IEdmEntityType entityType, IEdmModel model)
		{
			ODataEntityPropertyMappingCache epmCache = model.GetEpmCache(entityType);
			if (epmCache != null && version < epmCache.EpmTargetTree.MinimumODataProtocolVersion)
			{
				throw new ODataException(Strings.ODataVersionChecker_EpmVersionNotSupported(entityType.ODataFullName(), ODataUtils.ODataVersionToString(epmCache.EpmTargetTree.MinimumODataProtocolVersion), ODataUtils.ODataVersionToString(version)));
			}
		}

		// Token: 0x06001620 RID: 5664 RVA: 0x00050975 File Offset: 0x0004EB75
		internal static void CheckSpatialValue(ODataVersion version)
		{
			if (version < ODataVersion.V3)
			{
				throw new ODataException(Strings.ODataVersionChecker_GeographyAndGeometryNotSupported(ODataUtils.ODataVersionToString(version)));
			}
		}

		// Token: 0x06001621 RID: 5665 RVA: 0x0005098C File Offset: 0x0004EB8C
		internal static void CheckVersionSupported(ODataVersion version, ODataMessageReaderSettings messageReaderSettings)
		{
			if (version > messageReaderSettings.MaxProtocolVersion)
			{
				throw new ODataException(Strings.ODataVersionChecker_MaxProtocolVersionExceeded(ODataUtils.ODataVersionToString(version), ODataUtils.ODataVersionToString(messageReaderSettings.MaxProtocolVersion)));
			}
		}
	}
}
