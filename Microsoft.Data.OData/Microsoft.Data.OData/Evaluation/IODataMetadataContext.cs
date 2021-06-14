using System;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.JsonLight;

namespace Microsoft.Data.OData.Evaluation
{
	// Token: 0x02000106 RID: 262
	internal interface IODataMetadataContext
	{
		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x06000722 RID: 1826
		IEdmModel Model { get; }

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x06000723 RID: 1827
		Uri ServiceBaseUri { get; }

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x06000724 RID: 1828
		Uri MetadataDocumentUri { get; }

		// Token: 0x06000725 RID: 1829
		ODataEntityMetadataBuilder GetEntityMetadataBuilderForReader(IODataJsonLightReaderEntryState entryState);

		// Token: 0x06000726 RID: 1830
		IEdmFunctionImport[] GetAlwaysBindableOperationsForType(IEdmType bindingType);

		// Token: 0x06000727 RID: 1831
		bool OperationsBoundToEntityTypeMustBeContainerQualified(IEdmEntityType entityType);
	}
}
