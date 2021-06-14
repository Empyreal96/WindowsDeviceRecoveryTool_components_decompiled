using System;
using System.Collections.Generic;
using Microsoft.Data.Edm.EdmToClrConversion;
using Microsoft.Data.Edm.Expressions;
using Microsoft.Data.Edm.Values;

namespace Microsoft.Data.Edm.Evaluation
{
	// Token: 0x020000CF RID: 207
	public class EdmToClrEvaluator : EdmExpressionEvaluator
	{
		// Token: 0x06000421 RID: 1057 RVA: 0x0000B426 File Offset: 0x00009626
		public EdmToClrEvaluator(IDictionary<IEdmFunction, Func<IEdmValue[], IEdmValue>> builtInFunctions) : base(builtInFunctions)
		{
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x0000B43A File Offset: 0x0000963A
		public EdmToClrEvaluator(IDictionary<IEdmFunction, Func<IEdmValue[], IEdmValue>> builtInFunctions, Func<string, IEdmValue[], IEdmValue> lastChanceFunctionApplier) : base(builtInFunctions, lastChanceFunctionApplier)
		{
		}

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x06000423 RID: 1059 RVA: 0x0000B44F File Offset: 0x0000964F
		// (set) Token: 0x06000424 RID: 1060 RVA: 0x0000B457 File Offset: 0x00009657
		public EdmToClrConverter EdmToClrConverter
		{
			get
			{
				return this.edmToClrConverter;
			}
			set
			{
				EdmUtil.CheckArgumentNull<EdmToClrConverter>(value, "value");
				this.edmToClrConverter = value;
			}
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x0000B46C File Offset: 0x0000966C
		public T EvaluateToClrValue<T>(IEdmExpression expression)
		{
			IEdmValue edmValue = base.Evaluate(expression);
			return this.edmToClrConverter.AsClrValue<T>(edmValue);
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x0000B490 File Offset: 0x00009690
		public T EvaluateToClrValue<T>(IEdmExpression expression, IEdmStructuredValue context)
		{
			IEdmValue edmValue = base.Evaluate(expression, context);
			return this.edmToClrConverter.AsClrValue<T>(edmValue);
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x0000B4B4 File Offset: 0x000096B4
		public T EvaluateToClrValue<T>(IEdmExpression expression, IEdmStructuredValue context, IEdmTypeReference targetType)
		{
			IEdmValue edmValue = base.Evaluate(expression, context, targetType);
			return this.edmToClrConverter.AsClrValue<T>(edmValue);
		}

		// Token: 0x0400019B RID: 411
		private EdmToClrConverter edmToClrConverter = new EdmToClrConverter();
	}
}
