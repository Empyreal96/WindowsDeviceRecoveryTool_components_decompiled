using System;
using System.Collections.Generic;
using Microsoft.Data.Edm.Validation;
using Microsoft.Data.Edm.Values;

namespace Microsoft.Data.Edm.Library.Internal
{
	// Token: 0x020000FD RID: 253
	internal class BadEnumMember : BadElement, IEdmEnumMember, IEdmNamedElement, IEdmElement
	{
		// Token: 0x060004E4 RID: 1252 RVA: 0x0000C6B2 File Offset: 0x0000A8B2
		public BadEnumMember(IEdmEnumType declaringType, string name, IEnumerable<EdmError> errors) : base(errors)
		{
			this.name = (name ?? string.Empty);
			this.declaringType = declaringType;
		}

		// Token: 0x17000219 RID: 537
		// (get) Token: 0x060004E5 RID: 1253 RVA: 0x0000C6D2 File Offset: 0x0000A8D2
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x1700021A RID: 538
		// (get) Token: 0x060004E6 RID: 1254 RVA: 0x0000C6DA File Offset: 0x0000A8DA
		public IEdmEnumType DeclaringType
		{
			get
			{
				return this.declaringType;
			}
		}

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x060004E7 RID: 1255 RVA: 0x0000C6E2 File Offset: 0x0000A8E2
		public IEdmPrimitiveValue Value
		{
			get
			{
				return new BadPrimitiveValue(new EdmPrimitiveTypeReference(this.declaringType.UnderlyingType, false), base.Errors);
			}
		}

		// Token: 0x040001CD RID: 461
		private readonly string name;

		// Token: 0x040001CE RID: 462
		private readonly IEdmEnumType declaringType;
	}
}
