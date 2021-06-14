using System;
using System.Collections.Generic;
using Microsoft.Data.Edm.Library.Internal;
using Microsoft.Data.Edm.Validation;

namespace Microsoft.Data.Edm.Csdl.Internal.CsdlSemantics
{
	// Token: 0x02000180 RID: 384
	internal class BadAssociation : BadElement, IEdmAssociation, IEdmNamedElement, IEdmElement
	{
		// Token: 0x0600087A RID: 2170 RVA: 0x00017E36 File Offset: 0x00016036
		public BadAssociation(string qualifiedName, IEnumerable<EdmError> errors) : base(errors)
		{
			qualifiedName = (qualifiedName ?? string.Empty);
			EdmUtil.TryGetNamespaceNameFromQualifiedName(qualifiedName, out this.namespaceName, out this.name);
		}

		// Token: 0x17000380 RID: 896
		// (get) Token: 0x0600087B RID: 2171 RVA: 0x00017E5E File Offset: 0x0001605E
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x17000381 RID: 897
		// (get) Token: 0x0600087C RID: 2172 RVA: 0x00017E66 File Offset: 0x00016066
		public string Namespace
		{
			get
			{
				return this.namespaceName;
			}
		}

		// Token: 0x17000382 RID: 898
		// (get) Token: 0x0600087D RID: 2173 RVA: 0x00017E6E File Offset: 0x0001606E
		public IEdmAssociationEnd End1
		{
			get
			{
				return new BadAssociationEnd(this, "End1", base.Errors);
			}
		}

		// Token: 0x17000383 RID: 899
		// (get) Token: 0x0600087E RID: 2174 RVA: 0x00017E81 File Offset: 0x00016081
		public IEdmAssociationEnd End2
		{
			get
			{
				return new BadAssociationEnd(this, "End2", base.Errors);
			}
		}

		// Token: 0x17000384 RID: 900
		// (get) Token: 0x0600087F RID: 2175 RVA: 0x00017E94 File Offset: 0x00016094
		public CsdlSemanticsReferentialConstraint ReferentialConstraint
		{
			get
			{
				return null;
			}
		}

		// Token: 0x04000439 RID: 1081
		private string namespaceName;

		// Token: 0x0400043A RID: 1082
		private string name;
	}
}
