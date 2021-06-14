using System;
using System.Collections.Generic;

namespace Microsoft.Data.Edm.Annotations
{
	// Token: 0x0200009E RID: 158
	public interface IEdmTypeAnnotation : IEdmVocabularyAnnotation, IEdmElement
	{
		// Token: 0x17000169 RID: 361
		// (get) Token: 0x0600029B RID: 667
		IEnumerable<IEdmPropertyValueBinding> PropertyValueBindings { get; }
	}
}
