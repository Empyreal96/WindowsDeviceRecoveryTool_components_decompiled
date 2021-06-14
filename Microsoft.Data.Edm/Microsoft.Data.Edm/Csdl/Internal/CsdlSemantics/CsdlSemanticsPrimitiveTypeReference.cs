using System;
using Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast;
using Microsoft.Data.Edm.Library;

namespace Microsoft.Data.Edm.Csdl.Internal.CsdlSemantics
{
	// Token: 0x02000093 RID: 147
	internal class CsdlSemanticsPrimitiveTypeReference : CsdlSemanticsElement, IEdmPrimitiveTypeReference, IEdmTypeReference, IEdmElement
	{
		// Token: 0x06000266 RID: 614 RVA: 0x00006129 File Offset: 0x00004329
		public CsdlSemanticsPrimitiveTypeReference(CsdlSemanticsSchema schema, CsdlPrimitiveTypeReference reference) : base(reference)
		{
			this.schema = schema;
			this.Reference = reference;
			this.definition = EdmCoreModel.Instance.GetPrimitiveType(this.Reference.Kind);
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x06000267 RID: 615 RVA: 0x0000615B File Offset: 0x0000435B
		public bool IsNullable
		{
			get
			{
				return this.Reference.IsNullable;
			}
		}

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x06000268 RID: 616 RVA: 0x00006168 File Offset: 0x00004368
		public IEdmType Definition
		{
			get
			{
				return this.definition;
			}
		}

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x06000269 RID: 617 RVA: 0x00006170 File Offset: 0x00004370
		public override CsdlSemanticsModel Model
		{
			get
			{
				return this.schema.Model;
			}
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x0600026A RID: 618 RVA: 0x0000617D File Offset: 0x0000437D
		public override CsdlElement Element
		{
			get
			{
				return this.Reference;
			}
		}

		// Token: 0x0600026B RID: 619 RVA: 0x00006185 File Offset: 0x00004385
		public override string ToString()
		{
			return this.ToTraceString();
		}

		// Token: 0x04000116 RID: 278
		internal readonly CsdlPrimitiveTypeReference Reference;

		// Token: 0x04000117 RID: 279
		private readonly CsdlSemanticsSchema schema;

		// Token: 0x04000118 RID: 280
		private readonly IEdmPrimitiveType definition;
	}
}
