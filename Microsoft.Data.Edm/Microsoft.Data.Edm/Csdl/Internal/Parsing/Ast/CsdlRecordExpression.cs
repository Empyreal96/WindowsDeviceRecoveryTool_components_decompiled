using System;
using System.Collections.Generic;
using Microsoft.Data.Edm.Expressions;

namespace Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast
{
	// Token: 0x0200001F RID: 31
	internal class CsdlRecordExpression : CsdlExpressionBase
	{
		// Token: 0x0600007C RID: 124 RVA: 0x00002CE5 File Offset: 0x00000EE5
		public CsdlRecordExpression(CsdlTypeReference type, IEnumerable<CsdlPropertyValue> propertyValues, CsdlLocation location) : base(location)
		{
			this.type = type;
			this.propertyValues = new List<CsdlPropertyValue>(propertyValues);
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x0600007D RID: 125 RVA: 0x00002D01 File Offset: 0x00000F01
		public override EdmExpressionKind ExpressionKind
		{
			get
			{
				return EdmExpressionKind.Record;
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x0600007E RID: 126 RVA: 0x00002D05 File Offset: 0x00000F05
		public CsdlTypeReference Type
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x0600007F RID: 127 RVA: 0x00002D0D File Offset: 0x00000F0D
		public IEnumerable<CsdlPropertyValue> PropertyValues
		{
			get
			{
				return this.propertyValues;
			}
		}

		// Token: 0x04000030 RID: 48
		private readonly CsdlTypeReference type;

		// Token: 0x04000031 RID: 49
		private readonly List<CsdlPropertyValue> propertyValues;
	}
}
