using System;
using Microsoft.Data.Edm.Validation;

namespace Microsoft.Data.Edm.Library.Internal
{
	// Token: 0x02000110 RID: 272
	internal class CyclicEntityContainer : BadEntityContainer
	{
		// Token: 0x06000529 RID: 1321 RVA: 0x0000CCE0 File Offset: 0x0000AEE0
		public CyclicEntityContainer(string name, EdmLocation location) : base(name, new EdmError[]
		{
			new EdmError(location, EdmErrorCode.BadCyclicEntityContainer, Strings.Bad_CyclicEntityContainer(name))
		})
		{
		}
	}
}
