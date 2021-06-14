using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Edm.Validation;

namespace Microsoft.Data.Edm.Library.Internal
{
	// Token: 0x02000101 RID: 257
	internal class BadPrimitiveTypeReference : EdmPrimitiveTypeReference, IEdmCheckable
	{
		// Token: 0x060004F6 RID: 1270 RVA: 0x0000C7C4 File Offset: 0x0000A9C4
		public BadPrimitiveTypeReference(string qualifiedName, bool isNullable, IEnumerable<EdmError> errors) : base(new BadPrimitiveType(qualifiedName, EdmPrimitiveTypeKind.None, errors), isNullable)
		{
			this.errors = errors;
		}

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x060004F7 RID: 1271 RVA: 0x0000C7DC File Offset: 0x0000A9DC
		public IEnumerable<EdmError> Errors
		{
			get
			{
				return this.errors;
			}
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x0000C7E4 File Offset: 0x0000A9E4
		public override string ToString()
		{
			EdmError edmError = this.Errors.FirstOrDefault<EdmError>();
			string str = (edmError != null) ? (edmError.ErrorCode.ToString() + ":") : "";
			return str + this.ToTraceString();
		}

		// Token: 0x040001D6 RID: 470
		private readonly IEnumerable<EdmError> errors;
	}
}
