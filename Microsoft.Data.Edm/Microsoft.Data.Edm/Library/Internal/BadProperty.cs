using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Edm.Internal;
using Microsoft.Data.Edm.Validation;

namespace Microsoft.Data.Edm.Library.Internal
{
	// Token: 0x02000103 RID: 259
	internal class BadProperty : BadElement, IEdmStructuralProperty, IEdmProperty, IEdmNamedElement, IEdmVocabularyAnnotatable, IEdmElement
	{
		// Token: 0x060004FC RID: 1276 RVA: 0x0000C84A File Offset: 0x0000AA4A
		public BadProperty(IEdmStructuredType declaringType, string name, IEnumerable<EdmError> errors) : base(errors)
		{
			this.name = (name ?? string.Empty);
			this.declaringType = declaringType;
		}

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x060004FD RID: 1277 RVA: 0x0000C875 File Offset: 0x0000AA75
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x060004FE RID: 1278 RVA: 0x0000C87D File Offset: 0x0000AA7D
		public IEdmStructuredType DeclaringType
		{
			get
			{
				return this.declaringType;
			}
		}

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x060004FF RID: 1279 RVA: 0x0000C885 File Offset: 0x0000AA85
		public IEdmTypeReference Type
		{
			get
			{
				return this.type.GetValue(this, BadProperty.ComputeTypeFunc, null);
			}
		}

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x06000500 RID: 1280 RVA: 0x0000C899 File Offset: 0x0000AA99
		public string DefaultValueString
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x06000501 RID: 1281 RVA: 0x0000C89C File Offset: 0x0000AA9C
		public EdmConcurrencyMode ConcurrencyMode
		{
			get
			{
				return EdmConcurrencyMode.None;
			}
		}

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x06000502 RID: 1282 RVA: 0x0000C89F File Offset: 0x0000AA9F
		public EdmPropertyKind PropertyKind
		{
			get
			{
				return EdmPropertyKind.None;
			}
		}

		// Token: 0x06000503 RID: 1283 RVA: 0x0000C8A4 File Offset: 0x0000AAA4
		public override string ToString()
		{
			EdmError edmError = base.Errors.FirstOrDefault<EdmError>();
			string str = (edmError != null) ? (edmError.ErrorCode.ToString() + ":") : "";
			return str + this.ToTraceString();
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x0000C8EE File Offset: 0x0000AAEE
		private IEdmTypeReference ComputeType()
		{
			return new BadTypeReference(new BadType(base.Errors), true);
		}

		// Token: 0x040001D8 RID: 472
		private readonly string name;

		// Token: 0x040001D9 RID: 473
		private readonly IEdmStructuredType declaringType;

		// Token: 0x040001DA RID: 474
		private readonly Cache<BadProperty, IEdmTypeReference> type = new Cache<BadProperty, IEdmTypeReference>();

		// Token: 0x040001DB RID: 475
		private static readonly Func<BadProperty, IEdmTypeReference> ComputeTypeFunc = (BadProperty me) => me.ComputeType();
	}
}
