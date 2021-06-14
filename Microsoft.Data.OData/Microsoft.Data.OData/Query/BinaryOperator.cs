using System;

namespace Microsoft.Data.OData.Query
{
	// Token: 0x020000A6 RID: 166
	internal sealed class BinaryOperator
	{
		// Token: 0x060003E0 RID: 992 RVA: 0x0000C335 File Offset: 0x0000A535
		private BinaryOperator(string text, short precedence, bool needParenEvenWhenTheSame)
		{
			this.text = text;
			this.precedence = precedence;
			this.needParenEvenWhenTheSame = needParenEvenWhenTheSame;
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x060003E1 RID: 993 RVA: 0x0000C352 File Offset: 0x0000A552
		public bool NeedParenEvenWhenTheSame
		{
			get
			{
				return this.needParenEvenWhenTheSame;
			}
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x060003E2 RID: 994 RVA: 0x0000C35A File Offset: 0x0000A55A
		public short Precedence
		{
			get
			{
				return this.precedence;
			}
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x060003E3 RID: 995 RVA: 0x0000C362 File Offset: 0x0000A562
		public string Text
		{
			get
			{
				return this.text;
			}
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x0000C36C File Offset: 0x0000A56C
		public static BinaryOperator GetOperator(BinaryOperatorKind operatorKind)
		{
			switch (operatorKind)
			{
			case BinaryOperatorKind.Or:
				return BinaryOperator.Or;
			case BinaryOperatorKind.And:
				return BinaryOperator.And;
			case BinaryOperatorKind.Equal:
				return BinaryOperator.Equal;
			case BinaryOperatorKind.NotEqual:
				return BinaryOperator.NotEqual;
			case BinaryOperatorKind.GreaterThan:
				return BinaryOperator.GreaterThan;
			case BinaryOperatorKind.GreaterThanOrEqual:
				return BinaryOperator.GreaterThanOrEqual;
			case BinaryOperatorKind.LessThan:
				return BinaryOperator.LessThan;
			case BinaryOperatorKind.LessThanOrEqual:
				return BinaryOperator.LessThanOrEqual;
			case BinaryOperatorKind.Add:
				return BinaryOperator.Add;
			case BinaryOperatorKind.Subtract:
				return BinaryOperator.Subtract;
			case BinaryOperatorKind.Multiply:
				return BinaryOperator.Multiply;
			case BinaryOperatorKind.Divide:
				return BinaryOperator.Divide;
			case BinaryOperatorKind.Modulo:
				return BinaryOperator.Modulo;
			default:
				throw new ODataException(Strings.General_InternalError(InternalErrorCodes.BinaryOperator_GetOperator_UnreachableCodePath));
			}
		}

		// Token: 0x0400012C RID: 300
		private static readonly BinaryOperator Add = new BinaryOperator("add", 4, false);

		// Token: 0x0400012D RID: 301
		private static readonly BinaryOperator And = new BinaryOperator("and", 1, false);

		// Token: 0x0400012E RID: 302
		private static readonly BinaryOperator Divide = new BinaryOperator("div", 5, true);

		// Token: 0x0400012F RID: 303
		private static readonly BinaryOperator Equal = new BinaryOperator("eq", 2, true);

		// Token: 0x04000130 RID: 304
		private static readonly BinaryOperator GreaterThanOrEqual = new BinaryOperator("ge", 3, true);

		// Token: 0x04000131 RID: 305
		private static readonly BinaryOperator GreaterThan = new BinaryOperator("gt", 3, true);

		// Token: 0x04000132 RID: 306
		private static readonly BinaryOperator LessThanOrEqual = new BinaryOperator("le", 3, true);

		// Token: 0x04000133 RID: 307
		private static readonly BinaryOperator LessThan = new BinaryOperator("lt", 3, true);

		// Token: 0x04000134 RID: 308
		private static readonly BinaryOperator Modulo = new BinaryOperator("mod", 5, true);

		// Token: 0x04000135 RID: 309
		private static readonly BinaryOperator Multiply = new BinaryOperator("mul", 5, false);

		// Token: 0x04000136 RID: 310
		private static readonly BinaryOperator NotEqual = new BinaryOperator("ne", 2, true);

		// Token: 0x04000137 RID: 311
		private static readonly BinaryOperator Or = new BinaryOperator("or", 0, false);

		// Token: 0x04000138 RID: 312
		private static readonly BinaryOperator Subtract = new BinaryOperator("sub", 4, true);

		// Token: 0x04000139 RID: 313
		private readonly string text;

		// Token: 0x0400013A RID: 314
		private readonly short precedence;

		// Token: 0x0400013B RID: 315
		private readonly bool needParenEvenWhenTheSame;
	}
}
