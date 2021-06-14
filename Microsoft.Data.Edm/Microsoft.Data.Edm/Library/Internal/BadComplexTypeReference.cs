using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Edm.Validation;

namespace Microsoft.Data.Edm.Library.Internal
{
	// Token: 0x020000F1 RID: 241
	internal class BadComplexTypeReference : EdmComplexTypeReference, IEdmCheckable
	{
		// Token: 0x060004BE RID: 1214 RVA: 0x0000C425 File Offset: 0x0000A625
		public BadComplexTypeReference(string qualifiedName, bool isNullable, IEnumerable<EdmError> errors) : base(new BadComplexType(qualifiedName, errors), isNullable)
		{
			this.errors = errors;
		}

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x060004BF RID: 1215 RVA: 0x0000C43C File Offset: 0x0000A63C
		public IEnumerable<EdmError> Errors
		{
			get
			{
				return this.errors;
			}
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x0000C444 File Offset: 0x0000A644
		public override string ToString()
		{
			EdmError edmError = this.Errors.FirstOrDefault<EdmError>();
			string str = (edmError != null) ? (edmError.ErrorCode.ToString() + ":") : "";
			return str + this.ToTraceString();
		}

		// Token: 0x040001C3 RID: 451
		private readonly IEnumerable<EdmError> errors;
	}
}
