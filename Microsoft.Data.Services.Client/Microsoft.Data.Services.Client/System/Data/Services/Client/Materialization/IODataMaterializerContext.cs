using System;
using System.Data.Services.Client.Metadata;
using Microsoft.Data.Edm;

namespace System.Data.Services.Client.Materialization
{
	// Token: 0x02000053 RID: 83
	internal interface IODataMaterializerContext
	{
		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x060002A9 RID: 681
		bool IgnoreMissingProperties { get; }

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x060002AA RID: 682
		ClientEdmModel Model { get; }

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x060002AB RID: 683
		DataServiceClientResponsePipelineConfiguration ResponsePipeline { get; }

		// Token: 0x060002AC RID: 684
		ClientTypeAnnotation ResolveTypeForMaterialization(Type expectedType, string readerTypeName);

		// Token: 0x060002AD RID: 685
		IEdmType ResolveExpectedTypeForReading(Type clientClrType);
	}
}
