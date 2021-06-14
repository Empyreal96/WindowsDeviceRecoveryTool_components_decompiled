using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Edm.Validation.Internal;

namespace Microsoft.Data.Edm.Validation
{
	// Token: 0x02000239 RID: 569
	public static class ValidationExtensionMethods
	{
		// Token: 0x06000C99 RID: 3225 RVA: 0x00025305 File Offset: 0x00023505
		public static bool IsBad(this IEdmElement element)
		{
			EdmUtil.CheckArgumentNull<IEdmElement>(element, "element");
			return element.Errors().FirstOrDefault<EdmError>() != null;
		}

		// Token: 0x06000C9A RID: 3226 RVA: 0x00025324 File Offset: 0x00023524
		public static IEnumerable<EdmError> Errors(this IEdmElement element)
		{
			EdmUtil.CheckArgumentNull<IEdmElement>(element, "element");
			return InterfaceValidator.GetStructuralErrors(element);
		}

		// Token: 0x06000C9B RID: 3227 RVA: 0x00025338 File Offset: 0x00023538
		public static IEnumerable<EdmError> TypeErrors(this IEdmTypeReference type)
		{
			EdmUtil.CheckArgumentNull<IEdmTypeReference>(type, "type");
			return InterfaceValidator.GetStructuralErrors(type).Concat(InterfaceValidator.GetStructuralErrors(type.Definition));
		}
	}
}
