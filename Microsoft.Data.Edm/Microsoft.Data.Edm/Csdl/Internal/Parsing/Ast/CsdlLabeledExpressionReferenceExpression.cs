using System;
using Microsoft.Data.Edm.Expressions;

namespace Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast
{
	// Token: 0x02000015 RID: 21
	internal class CsdlLabeledExpressionReferenceExpression : CsdlExpressionBase
	{
		// Token: 0x0600005F RID: 95 RVA: 0x00002B30 File Offset: 0x00000D30
		public CsdlLabeledExpressionReferenceExpression(string label, CsdlLocation location) : base(location)
		{
			this.label = label;
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000060 RID: 96 RVA: 0x00002B40 File Offset: 0x00000D40
		public override EdmExpressionKind ExpressionKind
		{
			get
			{
				return EdmExpressionKind.LabeledExpressionReference;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000061 RID: 97 RVA: 0x00002B44 File Offset: 0x00000D44
		public string Label
		{
			get
			{
				return this.label;
			}
		}

		// Token: 0x04000021 RID: 33
		private readonly string label;
	}
}
