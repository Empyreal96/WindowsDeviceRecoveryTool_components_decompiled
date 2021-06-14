using System;
using Microsoft.Data.Edm;

namespace Microsoft.Data.OData.JsonLight
{
	// Token: 0x02000111 RID: 273
	internal sealed class JsonNoMetadataTypeNameOracle : JsonLightTypeNameOracle
	{
		// Token: 0x06000759 RID: 1881 RVA: 0x000190EE File Offset: 0x000172EE
		internal override string GetEntryTypeNameForWriting(string expectedTypeName, ODataEntry entry)
		{
			return null;
		}

		// Token: 0x0600075A RID: 1882 RVA: 0x000190F1 File Offset: 0x000172F1
		internal override string GetValueTypeNameForWriting(ODataValue value, IEdmTypeReference typeReferenceFromMetadata, IEdmTypeReference typeReferenceFromValue, bool isOpenProperty)
		{
			return null;
		}
	}
}
