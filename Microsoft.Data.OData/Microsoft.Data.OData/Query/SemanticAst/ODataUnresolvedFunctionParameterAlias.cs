using System;
using Microsoft.Data.Edm;

namespace Microsoft.Data.OData.Query.SemanticAst
{
	// Token: 0x0200008B RID: 139
	public class ODataUnresolvedFunctionParameterAlias : ODataValue
	{
		// Token: 0x06000343 RID: 835 RVA: 0x0000B649 File Offset: 0x00009849
		public ODataUnresolvedFunctionParameterAlias(string alias, IEdmTypeReference type)
		{
			ExceptionUtils.CheckArgumentStringNotNullOrEmpty(alias, "alias");
			this.Alias = alias;
			this.Type = type;
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x06000344 RID: 836 RVA: 0x0000B66A File Offset: 0x0000986A
		// (set) Token: 0x06000345 RID: 837 RVA: 0x0000B672 File Offset: 0x00009872
		public IEdmTypeReference Type { get; private set; }

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000346 RID: 838 RVA: 0x0000B67B File Offset: 0x0000987B
		// (set) Token: 0x06000347 RID: 839 RVA: 0x0000B683 File Offset: 0x00009883
		public string Alias { get; private set; }
	}
}
