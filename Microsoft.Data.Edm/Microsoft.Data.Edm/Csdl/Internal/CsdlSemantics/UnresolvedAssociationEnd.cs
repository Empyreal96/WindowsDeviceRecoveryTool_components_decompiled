using System;
using Microsoft.Data.Edm.Validation;

namespace Microsoft.Data.Edm.Csdl.Internal.CsdlSemantics
{
	// Token: 0x020001A3 RID: 419
	internal class UnresolvedAssociationEnd : BadAssociationEnd, IUnresolvedElement
	{
		// Token: 0x06000920 RID: 2336 RVA: 0x000187B8 File Offset: 0x000169B8
		public UnresolvedAssociationEnd(IEdmAssociation declaringAssociation, string role, EdmLocation location) : base(declaringAssociation, role, new EdmError[]
		{
			new EdmError(location, EdmErrorCode.BadNonComputableAssociationEnd, Strings.Bad_UncomputableAssociationEnd(role))
		})
		{
		}
	}
}
