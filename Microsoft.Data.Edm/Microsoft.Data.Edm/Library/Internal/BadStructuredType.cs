using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Edm.Validation;

namespace Microsoft.Data.Edm.Library.Internal
{
	// Token: 0x020000EB RID: 235
	internal abstract class BadStructuredType : BadType, IEdmStructuredType, IEdmType, IEdmElement, IEdmCheckable
	{
		// Token: 0x060004B0 RID: 1200 RVA: 0x0000C3B4 File Offset: 0x0000A5B4
		protected BadStructuredType(IEnumerable<EdmError> errors) : base(errors)
		{
		}

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x060004B1 RID: 1201 RVA: 0x0000C3BD File Offset: 0x0000A5BD
		public IEdmStructuredType BaseType
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x060004B2 RID: 1202 RVA: 0x0000C3C0 File Offset: 0x0000A5C0
		public IEnumerable<IEdmProperty> DeclaredProperties
		{
			get
			{
				return Enumerable.Empty<IEdmProperty>();
			}
		}

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x060004B3 RID: 1203 RVA: 0x0000C3C7 File Offset: 0x0000A5C7
		public bool IsAbstract
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x060004B4 RID: 1204 RVA: 0x0000C3CA File Offset: 0x0000A5CA
		public bool IsOpen
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x0000C3CD File Offset: 0x0000A5CD
		public IEdmProperty FindProperty(string name)
		{
			return null;
		}
	}
}
