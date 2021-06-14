using System;
using System.Collections.Generic;

namespace Microsoft.Data.Edm.Expressions
{
	// Token: 0x02000084 RID: 132
	public interface IEdmPathExpression : IEdmExpression, IEdmElement
	{
		// Token: 0x1700011F RID: 287
		// (get) Token: 0x0600021E RID: 542
		IEnumerable<string> Path { get; }
	}
}
