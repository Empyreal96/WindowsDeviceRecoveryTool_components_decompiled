using System;
using System.Collections.Generic;
using Microsoft.Data.Edm.Validation;
using Microsoft.Data.Edm.Values;

namespace Microsoft.Data.Edm.Library.Internal
{
	// Token: 0x02000102 RID: 258
	internal class BadPrimitiveValue : BadElement, IEdmPrimitiveValue, IEdmValue, IEdmElement
	{
		// Token: 0x060004F9 RID: 1273 RVA: 0x0000C82E File Offset: 0x0000AA2E
		public BadPrimitiveValue(IEdmPrimitiveTypeReference type, IEnumerable<EdmError> errors) : base(errors)
		{
			this.type = type;
		}

		// Token: 0x17000226 RID: 550
		// (get) Token: 0x060004FA RID: 1274 RVA: 0x0000C83E File Offset: 0x0000AA3E
		public IEdmTypeReference Type
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x060004FB RID: 1275 RVA: 0x0000C846 File Offset: 0x0000AA46
		public EdmValueKind ValueKind
		{
			get
			{
				return EdmValueKind.None;
			}
		}

		// Token: 0x040001D7 RID: 471
		private readonly IEdmPrimitiveTypeReference type;
	}
}
