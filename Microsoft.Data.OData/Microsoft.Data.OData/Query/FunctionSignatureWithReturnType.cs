using System;
using Microsoft.Data.Edm;

namespace Microsoft.Data.OData.Query
{
	// Token: 0x020000BA RID: 186
	internal sealed class FunctionSignatureWithReturnType : FunctionSignature
	{
		// Token: 0x06000484 RID: 1156 RVA: 0x0000EB99 File Offset: 0x0000CD99
		internal FunctionSignatureWithReturnType(IEdmTypeReference returnType, params IEdmTypeReference[] argumentTypes) : base(argumentTypes)
		{
			this.returnType = returnType;
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x06000485 RID: 1157 RVA: 0x0000EBA9 File Offset: 0x0000CDA9
		internal IEdmTypeReference ReturnType
		{
			get
			{
				return this.returnType;
			}
		}

		// Token: 0x04000187 RID: 391
		private readonly IEdmTypeReference returnType;
	}
}
