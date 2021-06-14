using System;

namespace Microsoft.Data.Edm.Csdl.Internal.CsdlSemantics
{
	// Token: 0x020000A6 RID: 166
	internal interface IEdmAssociationSet : IEdmNamedElement, IEdmElement
	{
		// Token: 0x1700017D RID: 381
		// (get) Token: 0x060002BE RID: 702
		IEdmAssociation Association { get; }

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x060002BF RID: 703
		IEdmAssociationSetEnd End1 { get; }

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x060002C0 RID: 704
		IEdmAssociationSetEnd End2 { get; }

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x060002C1 RID: 705
		IEdmEntityContainer Container { get; }
	}
}
