using System;
using System.Collections.Generic;
using Microsoft.Data.Edm.Validation;

namespace Microsoft.Data.Edm.Library.Internal
{
	// Token: 0x0200002A RID: 42
	internal class BadElement : IEdmCheckable, IEdmVocabularyAnnotatable, IEdmElement
	{
		// Token: 0x0600008F RID: 143 RVA: 0x00002DB6 File Offset: 0x00000FB6
		public BadElement(IEnumerable<EdmError> errors)
		{
			this.errors = errors;
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000090 RID: 144 RVA: 0x00002DC5 File Offset: 0x00000FC5
		public IEnumerable<EdmError> Errors
		{
			get
			{
				return this.errors;
			}
		}

		// Token: 0x04000038 RID: 56
		private readonly IEnumerable<EdmError> errors;
	}
}
