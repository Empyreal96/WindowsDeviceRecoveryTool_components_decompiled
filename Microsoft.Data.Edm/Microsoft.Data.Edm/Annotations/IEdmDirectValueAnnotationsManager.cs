using System;
using System.Collections.Generic;

namespace Microsoft.Data.Edm.Annotations
{
	// Token: 0x02000048 RID: 72
	public interface IEdmDirectValueAnnotationsManager
	{
		// Token: 0x06000104 RID: 260
		IEnumerable<IEdmDirectValueAnnotation> GetDirectValueAnnotations(IEdmElement element);

		// Token: 0x06000105 RID: 261
		void SetAnnotationValue(IEdmElement element, string namespaceName, string localName, object value);

		// Token: 0x06000106 RID: 262
		void SetAnnotationValues(IEnumerable<IEdmDirectValueAnnotationBinding> annotations);

		// Token: 0x06000107 RID: 263
		object GetAnnotationValue(IEdmElement element, string namespaceName, string localName);

		// Token: 0x06000108 RID: 264
		object[] GetAnnotationValues(IEnumerable<IEdmDirectValueAnnotationBinding> annotations);
	}
}
