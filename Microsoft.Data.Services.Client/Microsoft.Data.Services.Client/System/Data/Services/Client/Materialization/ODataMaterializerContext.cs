using System;
using System.Data.Services.Client.Metadata;
using Microsoft.Data.Edm;

namespace System.Data.Services.Client.Materialization
{
	// Token: 0x02000054 RID: 84
	internal class ODataMaterializerContext : IODataMaterializerContext
	{
		// Token: 0x060002AE RID: 686 RVA: 0x0000CDDC File Offset: 0x0000AFDC
		internal ODataMaterializerContext(ResponseInfo responseInfo)
		{
			this.ResponseInfo = responseInfo;
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x060002AF RID: 687 RVA: 0x0000CDEB File Offset: 0x0000AFEB
		public bool IgnoreMissingProperties
		{
			get
			{
				return this.ResponseInfo.IgnoreMissingProperties;
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x060002B0 RID: 688 RVA: 0x0000CDF8 File Offset: 0x0000AFF8
		public ClientEdmModel Model
		{
			get
			{
				return this.ResponseInfo.Model;
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x060002B1 RID: 689 RVA: 0x0000CE05 File Offset: 0x0000B005
		public DataServiceClientResponsePipelineConfiguration ResponsePipeline
		{
			get
			{
				return this.ResponseInfo.ResponsePipeline;
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x060002B2 RID: 690 RVA: 0x0000CE12 File Offset: 0x0000B012
		// (set) Token: 0x060002B3 RID: 691 RVA: 0x0000CE1A File Offset: 0x0000B01A
		private protected ResponseInfo ResponseInfo { protected get; private set; }

		// Token: 0x060002B4 RID: 692 RVA: 0x0000CE23 File Offset: 0x0000B023
		public ClientTypeAnnotation ResolveTypeForMaterialization(Type expectedType, string wireTypeName)
		{
			return this.ResponseInfo.TypeResolver.ResolveTypeForMaterialization(expectedType, wireTypeName);
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x0000CE37 File Offset: 0x0000B037
		public IEdmType ResolveExpectedTypeForReading(Type expectedType)
		{
			return this.ResponseInfo.TypeResolver.ResolveExpectedTypeForReading(expectedType);
		}
	}
}
