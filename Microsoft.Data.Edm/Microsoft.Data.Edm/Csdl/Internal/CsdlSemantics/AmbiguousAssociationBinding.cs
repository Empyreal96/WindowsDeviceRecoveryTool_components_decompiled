using System;
using Microsoft.Data.Edm.Library.Internal;

namespace Microsoft.Data.Edm.Csdl.Internal.CsdlSemantics
{
	// Token: 0x0200017B RID: 379
	internal class AmbiguousAssociationBinding : AmbiguousBinding<IEdmAssociation>, IEdmAssociation, IEdmNamedElement, IEdmElement
	{
		// Token: 0x06000867 RID: 2151 RVA: 0x00017CE1 File Offset: 0x00015EE1
		public AmbiguousAssociationBinding(IEdmAssociation first, IEdmAssociation second) : base(first, second)
		{
			this.namespaceName = (first.Namespace ?? string.Empty);
		}

		// Token: 0x17000375 RID: 885
		// (get) Token: 0x06000868 RID: 2152 RVA: 0x00017D00 File Offset: 0x00015F00
		public string Namespace
		{
			get
			{
				return this.namespaceName;
			}
		}

		// Token: 0x17000376 RID: 886
		// (get) Token: 0x06000869 RID: 2153 RVA: 0x00017D08 File Offset: 0x00015F08
		public IEdmAssociationEnd End1
		{
			get
			{
				return new BadAssociationEnd(this, "End1", base.Errors);
			}
		}

		// Token: 0x17000377 RID: 887
		// (get) Token: 0x0600086A RID: 2154 RVA: 0x00017D1B File Offset: 0x00015F1B
		public IEdmAssociationEnd End2
		{
			get
			{
				return new BadAssociationEnd(this, "End2", base.Errors);
			}
		}

		// Token: 0x17000378 RID: 888
		// (get) Token: 0x0600086B RID: 2155 RVA: 0x00017D2E File Offset: 0x00015F2E
		public CsdlSemanticsReferentialConstraint ReferentialConstraint
		{
			get
			{
				return null;
			}
		}

		// Token: 0x04000431 RID: 1073
		private readonly string namespaceName;
	}
}
