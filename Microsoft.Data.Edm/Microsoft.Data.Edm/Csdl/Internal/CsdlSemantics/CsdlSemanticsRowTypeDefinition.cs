using System;
using Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast;

namespace Microsoft.Data.Edm.Csdl.Internal.CsdlSemantics
{
	// Token: 0x020001AC RID: 428
	internal class CsdlSemanticsRowTypeDefinition : CsdlSemanticsStructuredTypeDefinition, IEdmRowType, IEdmStructuredType, IEdmType, IEdmElement
	{
		// Token: 0x06000951 RID: 2385 RVA: 0x0001929B File Offset: 0x0001749B
		public CsdlSemanticsRowTypeDefinition(CsdlSemanticsSchema context, CsdlRowType row) : base(context, row)
		{
			this.row = row;
		}

		// Token: 0x170003FA RID: 1018
		// (get) Token: 0x06000952 RID: 2386 RVA: 0x000192AC File Offset: 0x000174AC
		public override EdmTypeKind TypeKind
		{
			get
			{
				return EdmTypeKind.Row;
			}
		}

		// Token: 0x170003FB RID: 1019
		// (get) Token: 0x06000953 RID: 2387 RVA: 0x000192AF File Offset: 0x000174AF
		public override IEdmStructuredType BaseType
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170003FC RID: 1020
		// (get) Token: 0x06000954 RID: 2388 RVA: 0x000192B2 File Offset: 0x000174B2
		protected override CsdlStructuredType MyStructured
		{
			get
			{
				return this.row;
			}
		}

		// Token: 0x0400048E RID: 1166
		private readonly CsdlRowType row;
	}
}
