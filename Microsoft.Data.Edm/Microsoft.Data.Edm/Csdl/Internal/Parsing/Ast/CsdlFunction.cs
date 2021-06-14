using System;
using System.Collections.Generic;

namespace Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast
{
	// Token: 0x0200013F RID: 319
	internal class CsdlFunction : CsdlFunctionBase
	{
		// Token: 0x06000602 RID: 1538 RVA: 0x0000F608 File Offset: 0x0000D808
		public CsdlFunction(string name, IEnumerable<CsdlFunctionParameter> parameters, string definingExpression, CsdlTypeReference returnType, CsdlDocumentation documentation, CsdlLocation location) : base(name, parameters, returnType, documentation, location)
		{
			this.definingExpression = definingExpression;
		}

		// Token: 0x17000282 RID: 642
		// (get) Token: 0x06000603 RID: 1539 RVA: 0x0000F61F File Offset: 0x0000D81F
		public string DefiningExpression
		{
			get
			{
				return this.definingExpression;
			}
		}

		// Token: 0x04000331 RID: 817
		private readonly string definingExpression;
	}
}
