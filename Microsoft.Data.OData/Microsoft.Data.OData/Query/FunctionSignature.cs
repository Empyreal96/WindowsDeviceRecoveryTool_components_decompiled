using System;
using Microsoft.Data.Edm;

namespace Microsoft.Data.OData.Query
{
	// Token: 0x020000B9 RID: 185
	internal class FunctionSignature
	{
		// Token: 0x06000482 RID: 1154 RVA: 0x0000EB82 File Offset: 0x0000CD82
		internal FunctionSignature(params IEdmTypeReference[] argumentTypes)
		{
			this.argumentTypes = argumentTypes;
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x06000483 RID: 1155 RVA: 0x0000EB91 File Offset: 0x0000CD91
		internal IEdmTypeReference[] ArgumentTypes
		{
			get
			{
				return this.argumentTypes;
			}
		}

		// Token: 0x04000186 RID: 390
		private readonly IEdmTypeReference[] argumentTypes;
	}
}
