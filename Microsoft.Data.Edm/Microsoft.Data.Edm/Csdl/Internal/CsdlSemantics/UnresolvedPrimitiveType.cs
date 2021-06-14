using System;
using Microsoft.Data.Edm.Library.Internal;
using Microsoft.Data.Edm.Validation;

namespace Microsoft.Data.Edm.Csdl.Internal.CsdlSemantics
{
	// Token: 0x020001A5 RID: 421
	internal class UnresolvedPrimitiveType : BadPrimitiveType, IUnresolvedElement
	{
		// Token: 0x06000922 RID: 2338 RVA: 0x0001881C File Offset: 0x00016A1C
		public UnresolvedPrimitiveType(string qualifiedName, EdmLocation location) : base(qualifiedName, EdmPrimitiveTypeKind.None, new EdmError[]
		{
			new EdmError(location, EdmErrorCode.BadUnresolvedPrimitiveType, Strings.Bad_UnresolvedPrimitiveType(qualifiedName))
		})
		{
		}
	}
}
