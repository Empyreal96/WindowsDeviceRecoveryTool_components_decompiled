using System;
using Microsoft.Data.Edm.Expressions;
using Microsoft.Data.Edm.Values;

namespace Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast
{
	// Token: 0x0200001D RID: 29
	internal class CsdlConstantExpression : CsdlExpressionBase
	{
		// Token: 0x06000075 RID: 117 RVA: 0x00002C28 File Offset: 0x00000E28
		public CsdlConstantExpression(EdmValueKind kind, string value, CsdlLocation location) : base(location)
		{
			this.kind = kind;
			this.value = value;
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000076 RID: 118 RVA: 0x00002C40 File Offset: 0x00000E40
		public override EdmExpressionKind ExpressionKind
		{
			get
			{
				switch (this.kind)
				{
				case EdmValueKind.Binary:
					return EdmExpressionKind.BinaryConstant;
				case EdmValueKind.Boolean:
					return EdmExpressionKind.BooleanConstant;
				case EdmValueKind.DateTimeOffset:
					return EdmExpressionKind.DateTimeOffsetConstant;
				case EdmValueKind.DateTime:
					return EdmExpressionKind.DateTimeConstant;
				case EdmValueKind.Decimal:
					return EdmExpressionKind.DecimalConstant;
				case EdmValueKind.Floating:
					return EdmExpressionKind.FloatingConstant;
				case EdmValueKind.Guid:
					return EdmExpressionKind.GuidConstant;
				case EdmValueKind.Integer:
					return EdmExpressionKind.IntegerConstant;
				case EdmValueKind.Null:
					return EdmExpressionKind.Null;
				case EdmValueKind.String:
					return EdmExpressionKind.StringConstant;
				case EdmValueKind.Time:
					return EdmExpressionKind.TimeConstant;
				}
				return EdmExpressionKind.None;
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000077 RID: 119 RVA: 0x00002CAE File Offset: 0x00000EAE
		public EdmValueKind ValueKind
		{
			get
			{
				return this.kind;
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000078 RID: 120 RVA: 0x00002CB6 File Offset: 0x00000EB6
		public string Value
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x0400002C RID: 44
		private readonly EdmValueKind kind;

		// Token: 0x0400002D RID: 45
		private readonly string value;
	}
}
