using System;
using System.Collections.Generic;

namespace Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast
{
	// Token: 0x0200013E RID: 318
	internal abstract class CsdlFunctionBase : CsdlNamedElement
	{
		// Token: 0x060005FF RID: 1535 RVA: 0x0000F5D8 File Offset: 0x0000D7D8
		protected CsdlFunctionBase(string name, IEnumerable<CsdlFunctionParameter> parameters, CsdlTypeReference returnType, CsdlDocumentation documentation, CsdlLocation location) : base(name, documentation, location)
		{
			this.parameters = new List<CsdlFunctionParameter>(parameters);
			this.returnType = returnType;
		}

		// Token: 0x17000280 RID: 640
		// (get) Token: 0x06000600 RID: 1536 RVA: 0x0000F5F8 File Offset: 0x0000D7F8
		public IEnumerable<CsdlFunctionParameter> Parameters
		{
			get
			{
				return this.parameters;
			}
		}

		// Token: 0x17000281 RID: 641
		// (get) Token: 0x06000601 RID: 1537 RVA: 0x0000F600 File Offset: 0x0000D800
		public CsdlTypeReference ReturnType
		{
			get
			{
				return this.returnType;
			}
		}

		// Token: 0x0400032F RID: 815
		private readonly List<CsdlFunctionParameter> parameters;

		// Token: 0x04000330 RID: 816
		private readonly CsdlTypeReference returnType;
	}
}
