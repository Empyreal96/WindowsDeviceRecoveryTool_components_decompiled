using System;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.Json;

namespace Microsoft.Data.OData.JsonLight
{
	// Token: 0x0200013C RID: 316
	internal interface IODataJsonLightValueSerializer
	{
		// Token: 0x1700020E RID: 526
		// (get) Token: 0x06000872 RID: 2162
		IJsonWriter JsonWriter { get; }

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x06000873 RID: 2163
		ODataVersion Version { get; }

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x06000874 RID: 2164
		IEdmModel Model { get; }

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x06000875 RID: 2165
		ODataMessageWriterSettings Settings { get; }

		// Token: 0x06000876 RID: 2166
		void WriteNullValue();

		// Token: 0x06000877 RID: 2167
		void WriteComplexValue(ODataComplexValue complexValue, IEdmTypeReference metadataTypeReference, bool isTopLevel, bool isOpenPropertyType, DuplicatePropertyNamesChecker duplicatePropertyNamesChecker);

		// Token: 0x06000878 RID: 2168
		void WriteCollectionValue(ODataCollectionValue collectionValue, IEdmTypeReference metadataTypeReference, bool isTopLevelProperty, bool isInUri, bool isOpenPropertyType);

		// Token: 0x06000879 RID: 2169
		void WritePrimitiveValue(object value, IEdmTypeReference expectedTypeReference);

		// Token: 0x0600087A RID: 2170
		DuplicatePropertyNamesChecker CreateDuplicatePropertyNamesChecker();
	}
}
