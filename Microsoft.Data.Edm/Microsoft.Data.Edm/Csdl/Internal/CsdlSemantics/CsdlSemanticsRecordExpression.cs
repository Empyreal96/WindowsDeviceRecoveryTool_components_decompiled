using System;
using System.Collections.Generic;
using Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast;
using Microsoft.Data.Edm.Expressions;
using Microsoft.Data.Edm.Internal;

namespace Microsoft.Data.Edm.Csdl.Internal.CsdlSemantics
{
	// Token: 0x02000091 RID: 145
	internal class CsdlSemanticsRecordExpression : CsdlSemanticsExpression, IEdmRecordExpression, IEdmExpression, IEdmElement
	{
		// Token: 0x0600025B RID: 603 RVA: 0x00005FD0 File Offset: 0x000041D0
		public CsdlSemanticsRecordExpression(CsdlRecordExpression expression, IEdmEntityType bindingContext, CsdlSemanticsSchema schema) : base(schema, expression)
		{
			this.expression = expression;
			this.bindingContext = bindingContext;
		}

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x0600025C RID: 604 RVA: 0x00005FFE File Offset: 0x000041FE
		public override CsdlElement Element
		{
			get
			{
				return this.expression;
			}
		}

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x0600025D RID: 605 RVA: 0x00006006 File Offset: 0x00004206
		public override EdmExpressionKind ExpressionKind
		{
			get
			{
				return EdmExpressionKind.Record;
			}
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x0600025E RID: 606 RVA: 0x0000600A File Offset: 0x0000420A
		public IEdmStructuredTypeReference DeclaredType
		{
			get
			{
				return this.declaredTypeCache.GetValue(this, CsdlSemanticsRecordExpression.ComputeDeclaredTypeFunc, null);
			}
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x0600025F RID: 607 RVA: 0x0000601E File Offset: 0x0000421E
		public IEnumerable<IEdmPropertyConstructor> Properties
		{
			get
			{
				return this.propertiesCache.GetValue(this, CsdlSemanticsRecordExpression.ComputePropertiesFunc, null);
			}
		}

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x06000260 RID: 608 RVA: 0x00006032 File Offset: 0x00004232
		public IEdmEntityType BindingContext
		{
			get
			{
				return this.bindingContext;
			}
		}

		// Token: 0x06000261 RID: 609 RVA: 0x0000603C File Offset: 0x0000423C
		private IEnumerable<IEdmPropertyConstructor> ComputeProperties()
		{
			List<IEdmPropertyConstructor> list = new List<IEdmPropertyConstructor>();
			foreach (CsdlPropertyValue property in this.expression.PropertyValues)
			{
				list.Add(new CsdlSemanticsPropertyConstructor(property, this));
			}
			return list;
		}

		// Token: 0x06000262 RID: 610 RVA: 0x0000609C File Offset: 0x0000429C
		private IEdmStructuredTypeReference ComputeDeclaredType()
		{
			if (this.expression.Type == null)
			{
				return null;
			}
			return CsdlSemanticsModel.WrapTypeReference(base.Schema, this.expression.Type).AsStructured();
		}

		// Token: 0x0400010E RID: 270
		private readonly CsdlRecordExpression expression;

		// Token: 0x0400010F RID: 271
		private readonly IEdmEntityType bindingContext;

		// Token: 0x04000110 RID: 272
		private readonly Cache<CsdlSemanticsRecordExpression, IEdmStructuredTypeReference> declaredTypeCache = new Cache<CsdlSemanticsRecordExpression, IEdmStructuredTypeReference>();

		// Token: 0x04000111 RID: 273
		private static readonly Func<CsdlSemanticsRecordExpression, IEdmStructuredTypeReference> ComputeDeclaredTypeFunc = (CsdlSemanticsRecordExpression me) => me.ComputeDeclaredType();

		// Token: 0x04000112 RID: 274
		private readonly Cache<CsdlSemanticsRecordExpression, IEnumerable<IEdmPropertyConstructor>> propertiesCache = new Cache<CsdlSemanticsRecordExpression, IEnumerable<IEdmPropertyConstructor>>();

		// Token: 0x04000113 RID: 275
		private static readonly Func<CsdlSemanticsRecordExpression, IEnumerable<IEdmPropertyConstructor>> ComputePropertiesFunc = (CsdlSemanticsRecordExpression me) => me.ComputeProperties();
	}
}
