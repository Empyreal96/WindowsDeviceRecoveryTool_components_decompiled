using System;
using Microsoft.Data.Edm.Library.Internal;
using Microsoft.Data.Edm.Validation;

namespace Microsoft.Data.Edm.Csdl.Internal.CsdlSemantics
{
	// Token: 0x020001E1 RID: 481
	internal class UnresolvedEntityContainer : BadEntityContainer, IUnresolvedElement
	{
		// Token: 0x06000B69 RID: 2921 RVA: 0x0002119C File Offset: 0x0001F39C
		public UnresolvedEntityContainer(string name, EdmLocation location) : base(name, new EdmError[]
		{
			new EdmError(location, EdmErrorCode.BadUnresolvedEntityContainer, Strings.Bad_UnresolvedEntityContainer(name))
		})
		{
		}
	}
}
