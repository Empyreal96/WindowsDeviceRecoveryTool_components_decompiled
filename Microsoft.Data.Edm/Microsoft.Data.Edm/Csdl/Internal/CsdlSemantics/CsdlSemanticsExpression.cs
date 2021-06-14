using System;
using Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast;
using Microsoft.Data.Edm.Expressions;

namespace Microsoft.Data.Edm.Csdl.Internal.CsdlSemantics
{
	// Token: 0x0200003C RID: 60
	internal abstract class CsdlSemanticsExpression : CsdlSemanticsElement, IEdmExpression, IEdmElement
	{
		// Token: 0x060000CA RID: 202 RVA: 0x0000310F File Offset: 0x0000130F
		protected CsdlSemanticsExpression(CsdlSemanticsSchema schema, CsdlExpressionBase element) : base(element)
		{
			this.schema = schema;
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x060000CB RID: 203
		public abstract EdmExpressionKind ExpressionKind { get; }

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x060000CC RID: 204 RVA: 0x0000311F File Offset: 0x0000131F
		public CsdlSemanticsSchema Schema
		{
			get
			{
				return this.schema;
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x060000CD RID: 205 RVA: 0x00003127 File Offset: 0x00001327
		public override CsdlSemanticsModel Model
		{
			get
			{
				return this.schema.Model;
			}
		}

		// Token: 0x04000044 RID: 68
		private readonly CsdlSemanticsSchema schema;
	}
}
