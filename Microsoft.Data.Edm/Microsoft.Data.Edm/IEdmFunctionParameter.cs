using System;

namespace Microsoft.Data.Edm
{
	// Token: 0x02000171 RID: 369
	public interface IEdmFunctionParameter : IEdmNamedElement, IEdmVocabularyAnnotatable, IEdmElement
	{
		// Token: 0x1700033F RID: 831
		// (get) Token: 0x060007FD RID: 2045
		IEdmTypeReference Type { get; }

		// Token: 0x17000340 RID: 832
		// (get) Token: 0x060007FE RID: 2046
		IEdmFunctionBase DeclaringFunction { get; }

		// Token: 0x17000341 RID: 833
		// (get) Token: 0x060007FF RID: 2047
		EdmFunctionParameterMode Mode { get; }
	}
}
