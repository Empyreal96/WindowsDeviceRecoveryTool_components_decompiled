using System;
using Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast;
using Microsoft.Data.Edm.Internal;

namespace Microsoft.Data.Edm.Csdl.Internal.CsdlSemantics
{
	// Token: 0x0200007E RID: 126
	internal class CsdlSemanticsNamedTypeReference : CsdlSemanticsElement, IEdmTypeReference, IEdmElement
	{
		// Token: 0x06000207 RID: 519 RVA: 0x000059D5 File Offset: 0x00003BD5
		public CsdlSemanticsNamedTypeReference(CsdlSemanticsSchema schema, CsdlNamedTypeReference reference) : base(reference)
		{
			this.schema = schema;
			this.reference = reference;
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x06000208 RID: 520 RVA: 0x000059F7 File Offset: 0x00003BF7
		public IEdmType Definition
		{
			get
			{
				return this.definitionCache.GetValue(this, CsdlSemanticsNamedTypeReference.ComputeDefinitionFunc, null);
			}
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x06000209 RID: 521 RVA: 0x00005A0B File Offset: 0x00003C0B
		public bool IsNullable
		{
			get
			{
				return this.reference.IsNullable;
			}
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x0600020A RID: 522 RVA: 0x00005A18 File Offset: 0x00003C18
		public override CsdlSemanticsModel Model
		{
			get
			{
				return this.schema.Model;
			}
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x0600020B RID: 523 RVA: 0x00005A25 File Offset: 0x00003C25
		public override CsdlElement Element
		{
			get
			{
				return this.reference;
			}
		}

		// Token: 0x0600020C RID: 524 RVA: 0x00005A2D File Offset: 0x00003C2D
		public override string ToString()
		{
			return this.ToTraceString();
		}

		// Token: 0x0600020D RID: 525 RVA: 0x00005A38 File Offset: 0x00003C38
		private IEdmType ComputeDefinition()
		{
			IEdmType edmType = this.schema.FindType(this.reference.FullName);
			IEdmType result;
			if ((result = edmType) == null)
			{
				result = new UnresolvedType(this.schema.ReplaceAlias(this.reference.FullName) ?? this.reference.FullName, base.Location);
			}
			return result;
		}

		// Token: 0x040000E6 RID: 230
		private readonly CsdlSemanticsSchema schema;

		// Token: 0x040000E7 RID: 231
		private readonly CsdlNamedTypeReference reference;

		// Token: 0x040000E8 RID: 232
		private readonly Cache<CsdlSemanticsNamedTypeReference, IEdmType> definitionCache = new Cache<CsdlSemanticsNamedTypeReference, IEdmType>();

		// Token: 0x040000E9 RID: 233
		private static readonly Func<CsdlSemanticsNamedTypeReference, IEdmType> ComputeDefinitionFunc = (CsdlSemanticsNamedTypeReference me) => me.ComputeDefinition();
	}
}
