using System;

namespace Microsoft.Data.Edm.Csdl.Internal.CsdlSemantics
{
	// Token: 0x020000A5 RID: 165
	internal interface IEdmAssociationEnd : IEdmNamedElement, IEdmElement
	{
		// Token: 0x17000179 RID: 377
		// (get) Token: 0x060002BA RID: 698
		IEdmAssociation DeclaringAssociation { get; }

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x060002BB RID: 699
		IEdmEntityType EntityType { get; }

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x060002BC RID: 700
		EdmMultiplicity Multiplicity { get; }

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x060002BD RID: 701
		EdmOnDeleteAction OnDelete { get; }
	}
}
