using System;
using Microsoft.Data.Edm.Library.Internal;
using Microsoft.Data.Edm.Validation;

namespace Microsoft.Data.Edm.Csdl.Internal.CsdlSemantics
{
	// Token: 0x020001E3 RID: 483
	internal class UnresolvedEntityType : BadEntityType, IUnresolvedElement
	{
		// Token: 0x06000B6B RID: 2923 RVA: 0x00021200 File Offset: 0x0001F400
		public UnresolvedEntityType(string qualifiedName, EdmLocation location) : base(qualifiedName, new EdmError[]
		{
			new EdmError(location, EdmErrorCode.BadUnresolvedEntityType, Strings.Bad_UnresolvedEntityType(qualifiedName))
		})
		{
		}
	}
}
