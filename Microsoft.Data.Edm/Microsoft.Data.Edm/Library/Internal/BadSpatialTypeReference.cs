using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Edm.Validation;

namespace Microsoft.Data.Edm.Library.Internal
{
	// Token: 0x02000107 RID: 263
	internal class BadSpatialTypeReference : EdmSpatialTypeReference, IEdmCheckable
	{
		// Token: 0x0600050C RID: 1292 RVA: 0x0000C9D0 File Offset: 0x0000ABD0
		public BadSpatialTypeReference(string qualifiedName, bool isNullable, IEnumerable<EdmError> errors) : base(new BadPrimitiveType(qualifiedName, EdmPrimitiveTypeKind.None, errors), isNullable, null)
		{
			this.errors = errors;
		}

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x0600050D RID: 1293 RVA: 0x0000C9FC File Offset: 0x0000ABFC
		public IEnumerable<EdmError> Errors
		{
			get
			{
				return this.errors;
			}
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x0000CA04 File Offset: 0x0000AC04
		public override string ToString()
		{
			EdmError edmError = this.Errors.FirstOrDefault<EdmError>();
			string str = (edmError != null) ? (edmError.ErrorCode.ToString() + ":") : "";
			return str + this.ToTraceString();
		}

		// Token: 0x040001DE RID: 478
		private readonly IEnumerable<EdmError> errors;
	}
}
