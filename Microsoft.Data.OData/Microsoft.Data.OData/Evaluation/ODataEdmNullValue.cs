using System;
using Microsoft.Data.Edm;
using Microsoft.Data.Edm.Library.Values;
using Microsoft.Data.Edm.Values;

namespace Microsoft.Data.OData.Evaluation
{
	// Token: 0x0200017C RID: 380
	internal sealed class ODataEdmNullValue : EdmValue, IEdmNullValue, IEdmValue, IEdmElement
	{
		// Token: 0x06000AB2 RID: 2738 RVA: 0x00023A84 File Offset: 0x00021C84
		internal ODataEdmNullValue(IEdmTypeReference type) : base(type)
		{
		}

		// Token: 0x17000284 RID: 644
		// (get) Token: 0x06000AB3 RID: 2739 RVA: 0x00023A8D File Offset: 0x00021C8D
		public override EdmValueKind ValueKind
		{
			get
			{
				return EdmValueKind.Null;
			}
		}

		// Token: 0x040003FA RID: 1018
		internal static ODataEdmNullValue UntypedInstance = new ODataEdmNullValue(null);
	}
}
