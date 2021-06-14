using System;
using Microsoft.Data.Edm.Library.Internal;
using Microsoft.Data.Edm.Validation;

namespace Microsoft.Data.Edm.Csdl.Internal.CsdlSemantics
{
	// Token: 0x020001E0 RID: 480
	internal class UnresolvedComplexType : BadComplexType, IUnresolvedElement
	{
		// Token: 0x06000B68 RID: 2920 RVA: 0x0002116C File Offset: 0x0001F36C
		public UnresolvedComplexType(string qualifiedName, EdmLocation location) : base(qualifiedName, new EdmError[]
		{
			new EdmError(location, EdmErrorCode.BadUnresolvedComplexType, Strings.Bad_UnresolvedComplexType(qualifiedName))
		})
		{
		}
	}
}
