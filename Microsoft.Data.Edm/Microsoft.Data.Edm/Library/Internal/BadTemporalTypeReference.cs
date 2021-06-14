using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Edm.Validation;

namespace Microsoft.Data.Edm.Library.Internal
{
	// Token: 0x0200010D RID: 269
	internal class BadTemporalTypeReference : EdmTemporalTypeReference, IEdmCheckable
	{
		// Token: 0x06000522 RID: 1314 RVA: 0x0000CBC8 File Offset: 0x0000ADC8
		public BadTemporalTypeReference(string qualifiedName, bool isNullable, IEnumerable<EdmError> errors) : base(new BadPrimitiveType(qualifiedName, EdmPrimitiveTypeKind.None, errors), isNullable, null)
		{
			this.errors = errors;
		}

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x06000523 RID: 1315 RVA: 0x0000CBF4 File Offset: 0x0000ADF4
		public IEnumerable<EdmError> Errors
		{
			get
			{
				return this.errors;
			}
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x0000CBFC File Offset: 0x0000ADFC
		public override string ToString()
		{
			EdmError edmError = this.Errors.FirstOrDefault<EdmError>();
			string str = (edmError != null) ? (edmError.ErrorCode.ToString() + ":") : "";
			return str + this.ToTraceString();
		}

		// Token: 0x040001E6 RID: 486
		private readonly IEnumerable<EdmError> errors;
	}
}
