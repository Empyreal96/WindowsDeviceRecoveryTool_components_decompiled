using System;

namespace Microsoft.Data.Edm.Values
{
	// Token: 0x02000043 RID: 67
	public interface IEdmValue : IEdmElement
	{
		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060000F5 RID: 245
		IEdmTypeReference Type { get; }

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060000F6 RID: 246
		EdmValueKind ValueKind { get; }
	}
}
