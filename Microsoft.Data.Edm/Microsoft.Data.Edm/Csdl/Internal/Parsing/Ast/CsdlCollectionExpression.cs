using System;
using System.Collections.Generic;
using Microsoft.Data.Edm.Expressions;

namespace Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast
{
	// Token: 0x0200000C RID: 12
	internal class CsdlCollectionExpression : CsdlExpressionBase
	{
		// Token: 0x0600003F RID: 63 RVA: 0x000029AC File Offset: 0x00000BAC
		public CsdlCollectionExpression(CsdlTypeReference type, IEnumerable<CsdlExpressionBase> elementValues, CsdlLocation location) : base(location)
		{
			this.type = type;
			this.elementValues = new List<CsdlExpressionBase>(elementValues);
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000040 RID: 64 RVA: 0x000029C8 File Offset: 0x00000BC8
		public override EdmExpressionKind ExpressionKind
		{
			get
			{
				return EdmExpressionKind.Collection;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000041 RID: 65 RVA: 0x000029CC File Offset: 0x00000BCC
		public CsdlTypeReference Type
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000042 RID: 66 RVA: 0x000029D4 File Offset: 0x00000BD4
		public IEnumerable<CsdlExpressionBase> ElementValues
		{
			get
			{
				return this.elementValues;
			}
		}

		// Token: 0x04000011 RID: 17
		private readonly CsdlTypeReference type;

		// Token: 0x04000012 RID: 18
		private readonly List<CsdlExpressionBase> elementValues;
	}
}
