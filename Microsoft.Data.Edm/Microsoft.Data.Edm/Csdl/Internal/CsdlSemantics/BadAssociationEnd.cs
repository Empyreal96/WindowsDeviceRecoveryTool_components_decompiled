using System;
using System.Collections.Generic;
using Microsoft.Data.Edm.Internal;
using Microsoft.Data.Edm.Library.Internal;
using Microsoft.Data.Edm.Validation;

namespace Microsoft.Data.Edm.Csdl.Internal.CsdlSemantics
{
	// Token: 0x02000178 RID: 376
	internal class BadAssociationEnd : BadElement, IEdmAssociationEnd, IEdmNamedElement, IEdmElement
	{
		// Token: 0x06000850 RID: 2128 RVA: 0x00017A13 File Offset: 0x00015C13
		public BadAssociationEnd(IEdmAssociation declaringAssociation, string role, IEnumerable<EdmError> errors) : base(errors)
		{
			this.role = (role ?? string.Empty);
			this.declaringAssociation = declaringAssociation;
		}

		// Token: 0x17000367 RID: 871
		// (get) Token: 0x06000851 RID: 2129 RVA: 0x00017A3E File Offset: 0x00015C3E
		public IEdmAssociation DeclaringAssociation
		{
			get
			{
				return this.declaringAssociation;
			}
		}

		// Token: 0x17000368 RID: 872
		// (get) Token: 0x06000852 RID: 2130 RVA: 0x00017A46 File Offset: 0x00015C46
		public EdmMultiplicity Multiplicity
		{
			get
			{
				return EdmMultiplicity.Unknown;
			}
		}

		// Token: 0x17000369 RID: 873
		// (get) Token: 0x06000853 RID: 2131 RVA: 0x00017A49 File Offset: 0x00015C49
		public EdmOnDeleteAction OnDelete
		{
			get
			{
				return EdmOnDeleteAction.None;
			}
		}

		// Token: 0x1700036A RID: 874
		// (get) Token: 0x06000854 RID: 2132 RVA: 0x00017A4C File Offset: 0x00015C4C
		public IEdmEntityType EntityType
		{
			get
			{
				return this.type.GetValue(this, BadAssociationEnd.ComputeTypeFunc, null);
			}
		}

		// Token: 0x1700036B RID: 875
		// (get) Token: 0x06000855 RID: 2133 RVA: 0x00017A60 File Offset: 0x00015C60
		public string Name
		{
			get
			{
				return this.role;
			}
		}

		// Token: 0x06000856 RID: 2134 RVA: 0x00017A68 File Offset: 0x00015C68
		private BadEntityType ComputeType()
		{
			return new BadEntityType(this.declaringAssociation.Name + "." + this.role, base.Errors);
		}

		// Token: 0x04000428 RID: 1064
		private readonly string role;

		// Token: 0x04000429 RID: 1065
		private readonly IEdmAssociation declaringAssociation;

		// Token: 0x0400042A RID: 1066
		private readonly Cache<BadAssociationEnd, BadEntityType> type = new Cache<BadAssociationEnd, BadEntityType>();

		// Token: 0x0400042B RID: 1067
		private static readonly Func<BadAssociationEnd, BadEntityType> ComputeTypeFunc = (BadAssociationEnd me) => me.ComputeType();
	}
}
