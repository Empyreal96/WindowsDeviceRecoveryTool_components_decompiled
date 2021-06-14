using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Edm.Validation;

namespace Microsoft.Data.Edm.Library.Internal
{
	// Token: 0x020000E9 RID: 233
	internal class BadBinaryTypeReference : EdmBinaryTypeReference, IEdmCheckable
	{
		// Token: 0x060004AA RID: 1194 RVA: 0x0000C308 File Offset: 0x0000A508
		public BadBinaryTypeReference(string qualifiedName, bool isNullable, IEnumerable<EdmError> errors) : base(new BadPrimitiveType(qualifiedName, EdmPrimitiveTypeKind.Binary, errors), isNullable, false, null, new bool?(false))
		{
			this.errors = errors;
		}

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x060004AB RID: 1195 RVA: 0x0000C33B File Offset: 0x0000A53B
		public IEnumerable<EdmError> Errors
		{
			get
			{
				return this.errors;
			}
		}

		// Token: 0x060004AC RID: 1196 RVA: 0x0000C344 File Offset: 0x0000A544
		public override string ToString()
		{
			EdmError edmError = this.Errors.FirstOrDefault<EdmError>();
			string str = (edmError != null) ? (edmError.ErrorCode.ToString() + ":") : "";
			return str + this.ToTraceString();
		}

		// Token: 0x040001BF RID: 447
		private readonly IEnumerable<EdmError> errors;
	}
}
