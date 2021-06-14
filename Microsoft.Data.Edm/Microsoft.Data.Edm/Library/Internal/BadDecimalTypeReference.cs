using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Edm.Validation;

namespace Microsoft.Data.Edm.Library.Internal
{
	// Token: 0x020000F4 RID: 244
	internal class BadDecimalTypeReference : EdmDecimalTypeReference, IEdmCheckable
	{
		// Token: 0x060004C7 RID: 1223 RVA: 0x0000C4E0 File Offset: 0x0000A6E0
		public BadDecimalTypeReference(string qualifiedName, bool isNullable, IEnumerable<EdmError> errors) : base(new BadPrimitiveType(qualifiedName, EdmPrimitiveTypeKind.Decimal, errors), isNullable, null, null)
		{
			this.errors = errors;
		}

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x060004C8 RID: 1224 RVA: 0x0000C515 File Offset: 0x0000A715
		public IEnumerable<EdmError> Errors
		{
			get
			{
				return this.errors;
			}
		}

		// Token: 0x060004C9 RID: 1225 RVA: 0x0000C520 File Offset: 0x0000A720
		public override string ToString()
		{
			EdmError edmError = this.Errors.FirstOrDefault<EdmError>();
			string str = (edmError != null) ? (edmError.ErrorCode.ToString() + ":") : "";
			return str + this.ToTraceString();
		}

		// Token: 0x040001C6 RID: 454
		private readonly IEnumerable<EdmError> errors;
	}
}
