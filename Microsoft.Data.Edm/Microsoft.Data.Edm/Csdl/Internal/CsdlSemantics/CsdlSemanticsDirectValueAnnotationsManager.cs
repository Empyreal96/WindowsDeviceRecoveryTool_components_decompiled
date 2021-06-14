using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Edm.Annotations;
using Microsoft.Data.Edm.Library.Annotations;

namespace Microsoft.Data.Edm.Csdl.Internal.CsdlSemantics
{
	// Token: 0x0200004A RID: 74
	internal class CsdlSemanticsDirectValueAnnotationsManager : EdmDirectValueAnnotationsManager
	{
		// Token: 0x06000119 RID: 281 RVA: 0x00004474 File Offset: 0x00002674
		protected override IEnumerable<IEdmDirectValueAnnotation> GetAttachedAnnotations(IEdmElement element)
		{
			CsdlSemanticsElement csdlSemanticsElement = element as CsdlSemanticsElement;
			if (csdlSemanticsElement != null)
			{
				return csdlSemanticsElement.DirectValueAnnotations;
			}
			return Enumerable.Empty<IEdmDirectValueAnnotation>();
		}
	}
}
