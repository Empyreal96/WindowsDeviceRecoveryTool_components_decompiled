using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Edm.Validation;

namespace Microsoft.Data.Edm.Library.Internal
{
	// Token: 0x020000FC RID: 252
	internal class BadEntityTypeReference : EdmEntityTypeReference, IEdmCheckable
	{
		// Token: 0x060004E1 RID: 1249 RVA: 0x0000C648 File Offset: 0x0000A848
		public BadEntityTypeReference(string qualifiedName, bool isNullable, IEnumerable<EdmError> errors) : base(new BadEntityType(qualifiedName, errors), isNullable)
		{
			this.errors = errors;
		}

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x060004E2 RID: 1250 RVA: 0x0000C65F File Offset: 0x0000A85F
		public IEnumerable<EdmError> Errors
		{
			get
			{
				return this.errors;
			}
		}

		// Token: 0x060004E3 RID: 1251 RVA: 0x0000C668 File Offset: 0x0000A868
		public override string ToString()
		{
			EdmError edmError = this.Errors.FirstOrDefault<EdmError>();
			string str = (edmError != null) ? (edmError.ErrorCode.ToString() + ":") : "";
			return str + this.ToTraceString();
		}

		// Token: 0x040001CC RID: 460
		private readonly IEnumerable<EdmError> errors;
	}
}
