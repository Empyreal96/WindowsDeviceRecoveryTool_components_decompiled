using System;

namespace Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast
{
	// Token: 0x020000AE RID: 174
	internal class CsdlFunctionReturnType : CsdlElement
	{
		// Token: 0x0600030A RID: 778 RVA: 0x00007CC7 File Offset: 0x00005EC7
		public CsdlFunctionReturnType(CsdlTypeReference returnType, CsdlLocation location) : base(location)
		{
			this.returnType = returnType;
		}

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x0600030B RID: 779 RVA: 0x00007CD7 File Offset: 0x00005ED7
		public CsdlTypeReference ReturnType
		{
			get
			{
				return this.returnType;
			}
		}

		// Token: 0x04000157 RID: 343
		private readonly CsdlTypeReference returnType;
	}
}
