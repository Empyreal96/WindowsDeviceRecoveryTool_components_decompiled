using System;
using Microsoft.Data.Edm.Expressions;

namespace Microsoft.Data.Edm.Annotations
{
	// Token: 0x0200008A RID: 138
	public interface IEdmPropertyValueBinding : IEdmElement
	{
		// Token: 0x17000130 RID: 304
		// (get) Token: 0x0600023D RID: 573
		IEdmProperty BoundProperty { get; }

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x0600023E RID: 574
		IEdmExpression Value { get; }
	}
}
