using System;

namespace Microsoft.Data.Edm.Csdl.Internal.CsdlSemantics
{
	// Token: 0x020000A7 RID: 167
	internal interface IEdmAssociationSetEnd : IEdmElement
	{
		// Token: 0x17000181 RID: 385
		// (get) Token: 0x060002C2 RID: 706
		IEdmAssociationEnd Role { get; }

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x060002C3 RID: 707
		IEdmEntitySet EntitySet { get; }
	}
}
