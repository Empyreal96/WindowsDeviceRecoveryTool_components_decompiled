using System;
using Microsoft.Data.Edm;

namespace Microsoft.Data.OData.JsonLight
{
	// Token: 0x0200010C RID: 268
	internal abstract class JsonLightTypeNameOracle : TypeNameOracle
	{
		// Token: 0x06000746 RID: 1862
		internal abstract string GetEntryTypeNameForWriting(string expectedTypeName, ODataEntry entry);

		// Token: 0x06000747 RID: 1863
		internal abstract string GetValueTypeNameForWriting(ODataValue value, IEdmTypeReference typeReferenceFromMetadata, IEdmTypeReference typeReferenceFromValue, bool isOpenProperty);
	}
}
