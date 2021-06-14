using System;
using Microsoft.Data.Edm.Expressions;

namespace Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast
{
	// Token: 0x02000014 RID: 20
	internal class CsdlLabeledExpression : CsdlExpressionBase
	{
		// Token: 0x0600005B RID: 91 RVA: 0x00002B05 File Offset: 0x00000D05
		public CsdlLabeledExpression(string label, CsdlExpressionBase element, CsdlLocation location) : base(location)
		{
			this.label = label;
			this.element = element;
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600005C RID: 92 RVA: 0x00002B1C File Offset: 0x00000D1C
		public override EdmExpressionKind ExpressionKind
		{
			get
			{
				return EdmExpressionKind.Labeled;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600005D RID: 93 RVA: 0x00002B20 File Offset: 0x00000D20
		public string Label
		{
			get
			{
				return this.label;
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x0600005E RID: 94 RVA: 0x00002B28 File Offset: 0x00000D28
		public CsdlExpressionBase Element
		{
			get
			{
				return this.element;
			}
		}

		// Token: 0x0400001F RID: 31
		private readonly string label;

		// Token: 0x04000020 RID: 32
		private readonly CsdlExpressionBase element;
	}
}
