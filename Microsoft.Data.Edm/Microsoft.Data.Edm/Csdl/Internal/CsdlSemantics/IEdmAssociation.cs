using System;

namespace Microsoft.Data.Edm.Csdl.Internal.CsdlSemantics
{
	// Token: 0x020000A4 RID: 164
	internal interface IEdmAssociation : IEdmNamedElement, IEdmElement
	{
		// Token: 0x17000175 RID: 373
		// (get) Token: 0x060002B6 RID: 694
		string Namespace { get; }

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x060002B7 RID: 695
		IEdmAssociationEnd End1 { get; }

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x060002B8 RID: 696
		IEdmAssociationEnd End2 { get; }

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x060002B9 RID: 697
		CsdlSemanticsReferentialConstraint ReferentialConstraint { get; }
	}
}
