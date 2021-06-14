using System;
using System.Collections.Generic;
using Microsoft.Data.Edm.Validation;

namespace Microsoft.Data.Edm
{
	// Token: 0x02000028 RID: 40
	public interface IEdmCheckable
	{
		// Token: 0x17000045 RID: 69
		// (get) Token: 0x0600008E RID: 142
		IEnumerable<EdmError> Errors { get; }
	}
}
