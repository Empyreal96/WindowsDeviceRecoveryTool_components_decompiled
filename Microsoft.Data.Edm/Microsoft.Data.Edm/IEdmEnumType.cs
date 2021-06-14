using System;
using System.Collections.Generic;

namespace Microsoft.Data.Edm
{
	// Token: 0x02000030 RID: 48
	public interface IEdmEnumType : IEdmSchemaType, IEdmSchemaElement, IEdmNamedElement, IEdmVocabularyAnnotatable, IEdmType, IEdmElement
	{
		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000098 RID: 152
		IEdmPrimitiveType UnderlyingType { get; }

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000099 RID: 153
		IEnumerable<IEdmEnumMember> Members { get; }

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x0600009A RID: 154
		bool IsFlags { get; }
	}
}
