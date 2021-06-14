using System;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.Metadata;

namespace Microsoft.Data.OData.Query.SemanticAst
{
	// Token: 0x020000AB RID: 171
	public sealed class ConstantNode : SingleValueNode
	{
		// Token: 0x060003FC RID: 1020 RVA: 0x0000D27F File Offset: 0x0000B47F
		public ConstantNode(object constantValue, string literalText) : this(constantValue)
		{
			ExceptionUtils.CheckArgumentStringNotNullOrEmpty(literalText, "literalText");
			this.LiteralText = literalText;
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x0000D29A File Offset: 0x0000B49A
		public ConstantNode(object constantValue)
		{
			this.constantValue = constantValue;
			this.typeReference = ((constantValue == null) ? null : EdmLibraryExtensions.GetPrimitiveTypeReference(constantValue.GetType()));
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x060003FE RID: 1022 RVA: 0x0000D2C0 File Offset: 0x0000B4C0
		public object Value
		{
			get
			{
				return this.constantValue;
			}
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x060003FF RID: 1023 RVA: 0x0000D2C8 File Offset: 0x0000B4C8
		// (set) Token: 0x06000400 RID: 1024 RVA: 0x0000D2D0 File Offset: 0x0000B4D0
		public string LiteralText { get; private set; }

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x06000401 RID: 1025 RVA: 0x0000D2D9 File Offset: 0x0000B4D9
		public override IEdmTypeReference TypeReference
		{
			get
			{
				return this.typeReference;
			}
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x06000402 RID: 1026 RVA: 0x0000D2E1 File Offset: 0x0000B4E1
		internal override InternalQueryNodeKind InternalKind
		{
			get
			{
				return InternalQueryNodeKind.Constant;
			}
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x0000D2E4 File Offset: 0x0000B4E4
		public override T Accept<T>(QueryNodeVisitor<T> visitor)
		{
			ExceptionUtils.CheckArgumentNotNull<QueryNodeVisitor<T>>(visitor, "visitor");
			return visitor.Visit(this);
		}

		// Token: 0x0400014F RID: 335
		private readonly object constantValue;

		// Token: 0x04000150 RID: 336
		private readonly IEdmTypeReference typeReference;
	}
}
