using System;
using Microsoft.Data.Edm.Library.Internal;
using Microsoft.Data.Edm.Validation;

namespace Microsoft.Data.Edm.Csdl.Internal.CsdlSemantics
{
	// Token: 0x02000189 RID: 393
	internal class UnresolvedLabeledElement : BadLabeledExpression, IUnresolvedElement
	{
		// Token: 0x060008AA RID: 2218 RVA: 0x0001811C File Offset: 0x0001631C
		public UnresolvedLabeledElement(string label, EdmLocation location) : base(label, new EdmError[]
		{
			new EdmError(location, EdmErrorCode.BadUnresolvedLabeledElement, Strings.Bad_UnresolvedLabeledElement(label))
		})
		{
		}
	}
}
