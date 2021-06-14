using System;
using Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast;

namespace Microsoft.Data.Edm.Csdl.Internal.CsdlSemantics
{
	// Token: 0x0200006F RID: 111
	internal abstract class CsdlSemanticsTypeExpression : CsdlSemanticsElement, IEdmTypeReference, IEdmElement
	{
		// Token: 0x060001CC RID: 460 RVA: 0x00005443 File Offset: 0x00003643
		protected CsdlSemanticsTypeExpression(CsdlExpressionTypeReference expressionUsage, CsdlSemanticsTypeDefinition type) : base(expressionUsage)
		{
			this.expressionUsage = expressionUsage;
			this.type = type;
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x060001CD RID: 461 RVA: 0x0000545A File Offset: 0x0000365A
		public IEdmType Definition
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x060001CE RID: 462 RVA: 0x00005462 File Offset: 0x00003662
		public bool IsNullable
		{
			get
			{
				return this.expressionUsage.IsNullable;
			}
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x060001CF RID: 463 RVA: 0x0000546F File Offset: 0x0000366F
		public override CsdlSemanticsModel Model
		{
			get
			{
				return this.type.Model;
			}
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x060001D0 RID: 464 RVA: 0x0000547C File Offset: 0x0000367C
		public override CsdlElement Element
		{
			get
			{
				return this.expressionUsage;
			}
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x00005484 File Offset: 0x00003684
		public override string ToString()
		{
			return this.ToTraceString();
		}

		// Token: 0x040000C8 RID: 200
		private readonly CsdlExpressionTypeReference expressionUsage;

		// Token: 0x040000C9 RID: 201
		private readonly CsdlSemanticsTypeDefinition type;
	}
}
