using System;
using Microsoft.Data.Edm.Library.Internal;
using Microsoft.Data.Edm.Validation;

namespace Microsoft.Data.Edm.Csdl.Internal.CsdlSemantics
{
	// Token: 0x020001E4 RID: 484
	internal class UnresolvedProperty : BadProperty, IUnresolvedElement
	{
		// Token: 0x06000B6C RID: 2924 RVA: 0x00021230 File Offset: 0x0001F430
		public UnresolvedProperty(IEdmStructuredType declaringType, string name, EdmLocation location) : base(declaringType, name, new EdmError[]
		{
			new EdmError(location, EdmErrorCode.BadUnresolvedProperty, Strings.Bad_UnresolvedProperty(name))
		})
		{
		}
	}
}
