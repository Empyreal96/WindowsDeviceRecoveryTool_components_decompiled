using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Edm.Validation;

namespace Microsoft.Data.Edm.Library.Internal
{
	// Token: 0x0200010E RID: 270
	internal class BadTypeReference : EdmTypeReference, IEdmCheckable
	{
		// Token: 0x06000525 RID: 1317 RVA: 0x0000CC46 File Offset: 0x0000AE46
		public BadTypeReference(BadType definition, bool isNullable) : base(definition, isNullable)
		{
			this.errors = definition.Errors;
		}

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x06000526 RID: 1318 RVA: 0x0000CC5C File Offset: 0x0000AE5C
		public IEnumerable<EdmError> Errors
		{
			get
			{
				return this.errors;
			}
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x0000CC64 File Offset: 0x0000AE64
		public override string ToString()
		{
			EdmError edmError = this.Errors.FirstOrDefault<EdmError>();
			string str = (edmError != null) ? (edmError.ErrorCode.ToString() + ":") : "";
			return str + this.ToTraceString();
		}

		// Token: 0x040001E7 RID: 487
		private readonly IEnumerable<EdmError> errors;
	}
}
