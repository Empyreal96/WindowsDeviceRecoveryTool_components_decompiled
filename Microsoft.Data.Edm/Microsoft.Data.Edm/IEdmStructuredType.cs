using System;
using System.Collections.Generic;

namespace Microsoft.Data.Edm
{
	// Token: 0x02000036 RID: 54
	public interface IEdmStructuredType : IEdmType, IEdmElement
	{
		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060000AB RID: 171
		bool IsAbstract { get; }

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060000AC RID: 172
		bool IsOpen { get; }

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060000AD RID: 173
		IEdmStructuredType BaseType { get; }

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060000AE RID: 174
		IEnumerable<IEdmProperty> DeclaredProperties { get; }

		// Token: 0x060000AF RID: 175
		IEdmProperty FindProperty(string name);
	}
}
