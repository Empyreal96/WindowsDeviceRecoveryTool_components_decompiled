using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Edm.Validation;

namespace Microsoft.Data.Edm.Library.Internal
{
	// Token: 0x0200002C RID: 44
	internal class BadType : BadElement, IEdmType, IEdmElement
	{
		// Token: 0x06000092 RID: 146 RVA: 0x00002DCD File Offset: 0x00000FCD
		public BadType(IEnumerable<EdmError> errors) : base(errors)
		{
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000093 RID: 147 RVA: 0x00002DD6 File Offset: 0x00000FD6
		public virtual EdmTypeKind TypeKind
		{
			get
			{
				return EdmTypeKind.None;
			}
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00002DDC File Offset: 0x00000FDC
		public override string ToString()
		{
			EdmError edmError = base.Errors.FirstOrDefault<EdmError>();
			string str = (edmError != null) ? (edmError.ErrorCode.ToString() + ":") : "";
			return str + this.ToTraceString();
		}
	}
}
