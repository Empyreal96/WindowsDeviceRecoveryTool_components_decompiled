using System;
using Microsoft.Data.Edm.Library.Internal;
using Microsoft.Data.Edm.Validation;

namespace Microsoft.Data.Edm.Csdl.Internal.CsdlSemantics
{
	// Token: 0x020001E2 RID: 482
	internal class UnresolvedEntitySet : BadEntitySet, IUnresolvedElement
	{
		// Token: 0x06000B6A RID: 2922 RVA: 0x000211CC File Offset: 0x0001F3CC
		public UnresolvedEntitySet(string name, IEdmEntityContainer container, EdmLocation location) : base(name, container, new EdmError[]
		{
			new EdmError(location, EdmErrorCode.BadUnresolvedEntitySet, Strings.Bad_UnresolvedEntitySet(name))
		})
		{
		}
	}
}
