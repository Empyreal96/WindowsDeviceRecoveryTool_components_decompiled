using System;
using Microsoft.Data.Edm.Validation;

namespace Microsoft.Data.Edm.Library.Internal
{
	// Token: 0x0200010F RID: 271
	internal class CyclicComplexType : BadComplexType
	{
		// Token: 0x06000528 RID: 1320 RVA: 0x0000CCB0 File Offset: 0x0000AEB0
		public CyclicComplexType(string qualifiedName, EdmLocation location) : base(qualifiedName, new EdmError[]
		{
			new EdmError(location, EdmErrorCode.BadCyclicComplex, Strings.Bad_CyclicComplex(qualifiedName))
		})
		{
		}
	}
}
