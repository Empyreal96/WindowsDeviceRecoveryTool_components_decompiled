using System;

namespace Microsoft.Data.Edm.Expressions
{
	// Token: 0x0200004D RID: 77
	public interface IEdmEnumMemberReferenceExpression : IEdmExpression, IEdmElement
	{
		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000124 RID: 292
		IEdmEnumMember ReferencedEnumMember { get; }
	}
}
