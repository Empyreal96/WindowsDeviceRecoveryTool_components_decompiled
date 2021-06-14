using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast;
using Microsoft.Data.Edm.Expressions;
using Microsoft.Data.Edm.Internal;
using Microsoft.Data.Edm.Library.Expressions;
using Microsoft.Data.Edm.Validation;

namespace Microsoft.Data.Edm.Csdl.Internal.CsdlSemantics
{
	// Token: 0x0200003E RID: 62
	internal class CsdlSemanticsApplyExpression : CsdlSemanticsExpression, IEdmApplyExpression, IEdmExpression, IEdmElement, IEdmCheckable
	{
		// Token: 0x060000D0 RID: 208 RVA: 0x00003134 File Offset: 0x00001334
		public CsdlSemanticsApplyExpression(CsdlApplyExpression expression, IEdmEntityType bindingContext, CsdlSemanticsSchema schema) : base(schema, expression)
		{
			this.expression = expression;
			this.bindingContext = bindingContext;
			this.schema = schema;
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x060000D1 RID: 209 RVA: 0x00003169 File Offset: 0x00001369
		public override CsdlElement Element
		{
			get
			{
				return this.expression;
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x060000D2 RID: 210 RVA: 0x00003171 File Offset: 0x00001371
		public override EdmExpressionKind ExpressionKind
		{
			get
			{
				return EdmExpressionKind.FunctionApplication;
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x060000D3 RID: 211 RVA: 0x00003175 File Offset: 0x00001375
		public IEdmExpression AppliedFunction
		{
			get
			{
				return this.appliedFunctionCache.GetValue(this, CsdlSemanticsApplyExpression.ComputeAppliedFunctionFunc, null);
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x060000D4 RID: 212 RVA: 0x00003189 File Offset: 0x00001389
		public IEnumerable<IEdmExpression> Arguments
		{
			get
			{
				return this.argumentsCache.GetValue(this, CsdlSemanticsApplyExpression.ComputeArgumentsFunc, null);
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x060000D5 RID: 213 RVA: 0x0000319D File Offset: 0x0000139D
		public IEnumerable<EdmError> Errors
		{
			get
			{
				if (this.AppliedFunction is IUnresolvedElement)
				{
					return this.AppliedFunction.Errors();
				}
				return Enumerable.Empty<EdmError>();
			}
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x000031C0 File Offset: 0x000013C0
		private IEdmExpression ComputeAppliedFunction()
		{
			if (this.expression.Function == null)
			{
				return CsdlSemanticsModel.WrapExpression(this.expression.Arguments.FirstOrDefault(null), this.bindingContext, this.schema);
			}
			IEnumerable<IEdmFunction> source = this.schema.FindFunctions(this.expression.Function);
			IEdmFunction referencedFunction;
			if (source.Count<IEdmFunction>() == 0)
			{
				referencedFunction = new UnresolvedFunction(this.expression.Function, Strings.Bad_UnresolvedFunction(this.expression.Function), base.Location);
			}
			else
			{
				source = source.Where(new Func<IEdmFunction, bool>(this.IsMatchingFunction));
				int num = source.Count<IEdmFunction>();
				if (num > 1)
				{
					source = source.Where(new Func<IEdmFunction, bool>(this.IsExactMatch));
					num = source.Count<IEdmFunction>();
					if (num != 1)
					{
						referencedFunction = new UnresolvedFunction(this.expression.Function, Strings.Bad_AmbiguousFunction(this.expression.Function), base.Location);
					}
					else
					{
						referencedFunction = source.Single<IEdmFunction>();
					}
				}
				else if (num == 0)
				{
					referencedFunction = new UnresolvedFunction(this.expression.Function, Strings.Bad_FunctionParametersDontMatch(this.expression.Function), base.Location);
				}
				else
				{
					referencedFunction = source.Single<IEdmFunction>();
				}
			}
			return new EdmFunctionReferenceExpression(referencedFunction);
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x000032F4 File Offset: 0x000014F4
		private IEnumerable<IEdmExpression> ComputeArguments()
		{
			bool flag = this.expression.Function == null;
			List<IEdmExpression> list = new List<IEdmExpression>();
			foreach (CsdlExpressionBase csdlExpressionBase in this.expression.Arguments)
			{
				if (flag)
				{
					flag = false;
				}
				else
				{
					list.Add(CsdlSemanticsModel.WrapExpression(csdlExpressionBase, this.bindingContext, this.schema));
				}
			}
			return list;
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00003374 File Offset: 0x00001574
		private bool IsMatchingFunction(IEdmFunction function)
		{
			if (function.Parameters.Count<IEdmFunctionParameter>() != this.Arguments.Count<IEdmExpression>())
			{
				return false;
			}
			IEnumerator<IEdmExpression> enumerator = this.Arguments.GetEnumerator();
			foreach (IEdmFunctionParameter edmFunctionParameter in function.Parameters)
			{
				enumerator.MoveNext();
				IEnumerable<EdmError> enumerable;
				if (!enumerator.Current.TryAssertType(edmFunctionParameter.Type, this.bindingContext, false, out enumerable))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00003410 File Offset: 0x00001610
		private bool IsExactMatch(IEdmFunction function)
		{
			IEnumerator<IEdmExpression> enumerator = this.Arguments.GetEnumerator();
			foreach (IEdmFunctionParameter edmFunctionParameter in function.Parameters)
			{
				enumerator.MoveNext();
				IEnumerable<EdmError> enumerable;
				if (!enumerator.Current.TryAssertType(edmFunctionParameter.Type, this.bindingContext, true, out enumerable))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x04000045 RID: 69
		private readonly CsdlApplyExpression expression;

		// Token: 0x04000046 RID: 70
		private readonly CsdlSemanticsSchema schema;

		// Token: 0x04000047 RID: 71
		private readonly IEdmEntityType bindingContext;

		// Token: 0x04000048 RID: 72
		private readonly Cache<CsdlSemanticsApplyExpression, IEdmExpression> appliedFunctionCache = new Cache<CsdlSemanticsApplyExpression, IEdmExpression>();

		// Token: 0x04000049 RID: 73
		private static readonly Func<CsdlSemanticsApplyExpression, IEdmExpression> ComputeAppliedFunctionFunc = (CsdlSemanticsApplyExpression me) => me.ComputeAppliedFunction();

		// Token: 0x0400004A RID: 74
		private readonly Cache<CsdlSemanticsApplyExpression, IEnumerable<IEdmExpression>> argumentsCache = new Cache<CsdlSemanticsApplyExpression, IEnumerable<IEdmExpression>>();

		// Token: 0x0400004B RID: 75
		private static readonly Func<CsdlSemanticsApplyExpression, IEnumerable<IEdmExpression>> ComputeArgumentsFunc = (CsdlSemanticsApplyExpression me) => me.ComputeArguments();
	}
}
