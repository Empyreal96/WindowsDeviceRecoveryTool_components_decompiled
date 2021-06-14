using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Data.OData
{
	// Token: 0x0200029A RID: 666
	internal static class ODataUtilsInternal
	{
		// Token: 0x06001674 RID: 5748 RVA: 0x00051860 File Offset: 0x0004FA60
		internal static Version ToDataServiceVersion(this ODataVersion version)
		{
			switch (version)
			{
			case ODataVersion.V1:
				return new Version(1, 0);
			case ODataVersion.V2:
				return new Version(2, 0);
			case ODataVersion.V3:
				return new Version(3, 0);
			default:
			{
				string message = Strings.General_InternalError(InternalErrorCodes.ODataUtilsInternal_ToDataServiceVersion_UnreachableCodePath);
				throw new ODataException(message);
			}
			}
		}

		// Token: 0x06001675 RID: 5749 RVA: 0x000518B0 File Offset: 0x0004FAB0
		internal static void SetDataServiceVersion(ODataMessage message, ODataMessageWriterSettings settings)
		{
			string headerValue = ODataUtils.ODataVersionToString(settings.Version.Value) + ";";
			message.SetHeader("DataServiceVersion", headerValue);
		}

		// Token: 0x06001676 RID: 5750 RVA: 0x000518E8 File Offset: 0x0004FAE8
		internal static ODataVersion GetDataServiceVersion(ODataMessage message, ODataVersion defaultVersion)
		{
			string header = message.GetHeader("DataServiceVersion");
			string text = header;
			if (!string.IsNullOrEmpty(text))
			{
				return ODataUtils.StringToODataVersion(text);
			}
			return defaultVersion;
		}

		// Token: 0x06001677 RID: 5751 RVA: 0x00051914 File Offset: 0x0004FB14
		internal static bool IsPayloadKindSupported(ODataPayloadKind payloadKind, bool inRequest)
		{
			switch (payloadKind)
			{
			case ODataPayloadKind.Feed:
			case ODataPayloadKind.EntityReferenceLinks:
			case ODataPayloadKind.Collection:
			case ODataPayloadKind.ServiceDocument:
			case ODataPayloadKind.MetadataDocument:
			case ODataPayloadKind.Error:
				return !inRequest;
			case ODataPayloadKind.Entry:
			case ODataPayloadKind.Property:
			case ODataPayloadKind.EntityReferenceLink:
			case ODataPayloadKind.Value:
			case ODataPayloadKind.BinaryValue:
			case ODataPayloadKind.Batch:
				return true;
			case ODataPayloadKind.Parameter:
				return inRequest;
			default:
				throw new ODataException(Strings.General_InternalError(InternalErrorCodes.ODataUtilsInternal_IsPayloadKindSupported_UnreachableCodePath));
			}
		}

		// Token: 0x06001678 RID: 5752 RVA: 0x00051978 File Offset: 0x0004FB78
		internal static IEnumerable<T> ConcatEnumerables<T>(IEnumerable<T> enumerable1, IEnumerable<T> enumerable2)
		{
			if (enumerable1 == null)
			{
				return enumerable2;
			}
			if (enumerable2 == null)
			{
				return enumerable1;
			}
			return enumerable1.Concat(enumerable2);
		}

		// Token: 0x06001679 RID: 5753 RVA: 0x0005198B File Offset: 0x0004FB8B
		internal static SelectedPropertiesNode SelectedProperties(this ODataMetadataDocumentUri metadataDocumentUri)
		{
			if (metadataDocumentUri == null)
			{
				return SelectedPropertiesNode.Create(null);
			}
			return SelectedPropertiesNode.Create(metadataDocumentUri.SelectClause);
		}
	}
}
