using System;
using Microsoft.Data.Edm.Validation;

namespace Microsoft.Data.Edm.Library.Internal
{
	// Token: 0x02000111 RID: 273
	internal class CyclicEntityType : BadEntityType
	{
		// Token: 0x0600052A RID: 1322 RVA: 0x0000CD10 File Offset: 0x0000AF10
		public CyclicEntityType(string qualifiedName, EdmLocation location) : base(qualifiedName, new EdmError[]
		{
			new EdmError(location, EdmErrorCode.BadCyclicEntity, Strings.Bad_CyclicEntity(qualifiedName))
		})
		{
		}
	}
}
