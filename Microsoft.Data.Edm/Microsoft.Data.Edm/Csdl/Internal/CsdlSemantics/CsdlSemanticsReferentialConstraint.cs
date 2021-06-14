using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast;
using Microsoft.Data.Edm.Internal;
using Microsoft.Data.Edm.Library.Internal;
using Microsoft.Data.Edm.Validation;

namespace Microsoft.Data.Edm.Csdl.Internal.CsdlSemantics
{
	// Token: 0x020001AB RID: 427
	internal class CsdlSemanticsReferentialConstraint : CsdlSemanticsElement, IEdmCheckable
	{
		// Token: 0x0600093D RID: 2365 RVA: 0x000189D8 File Offset: 0x00016BD8
		public CsdlSemanticsReferentialConstraint(CsdlSemanticsAssociation context, CsdlReferentialConstraint constraint) : base(constraint)
		{
			this.context = context;
			this.constraint = constraint;
		}

		// Token: 0x170003F2 RID: 1010
		// (get) Token: 0x0600093E RID: 2366 RVA: 0x00018A31 File Offset: 0x00016C31
		public override CsdlSemanticsModel Model
		{
			get
			{
				return this.context.Model;
			}
		}

		// Token: 0x170003F3 RID: 1011
		// (get) Token: 0x0600093F RID: 2367 RVA: 0x00018A3E File Offset: 0x00016C3E
		public override CsdlElement Element
		{
			get
			{
				return this.constraint;
			}
		}

		// Token: 0x170003F4 RID: 1012
		// (get) Token: 0x06000940 RID: 2368 RVA: 0x00018A46 File Offset: 0x00016C46
		public IEdmAssociationEnd PrincipalEnd
		{
			get
			{
				return this.principalCache.GetValue(this, CsdlSemanticsReferentialConstraint.ComputePrincipalFunc, null);
			}
		}

		// Token: 0x170003F5 RID: 1013
		// (get) Token: 0x06000941 RID: 2369 RVA: 0x00018A5A File Offset: 0x00016C5A
		public IEnumerable<IEdmStructuralProperty> DependentProperties
		{
			get
			{
				return this.dependentPropertiesCache.GetValue(this, CsdlSemanticsReferentialConstraint.ComputeDependentPropertiesFunc, null);
			}
		}

		// Token: 0x170003F6 RID: 1014
		// (get) Token: 0x06000942 RID: 2370 RVA: 0x00018A6E File Offset: 0x00016C6E
		public IEnumerable<EdmError> Errors
		{
			get
			{
				return this.errorsCache.GetValue(this, CsdlSemanticsReferentialConstraint.ComputeErrorsFunc, null);
			}
		}

		// Token: 0x170003F7 RID: 1015
		// (get) Token: 0x06000943 RID: 2371 RVA: 0x00018A84 File Offset: 0x00016C84
		private IEdmAssociationEnd DependentEnd
		{
			get
			{
				IEdmAssociation declaringAssociation = this.PrincipalEnd.DeclaringAssociation;
				if (this.PrincipalEnd != declaringAssociation.End1)
				{
					return declaringAssociation.End1;
				}
				return declaringAssociation.End2;
			}
		}

		// Token: 0x170003F8 RID: 1016
		// (get) Token: 0x06000944 RID: 2372 RVA: 0x00018AB8 File Offset: 0x00016CB8
		private IEnumerable<string> PrincipalKeyPropertiesNotFoundInPrincipalProperties
		{
			get
			{
				return this.principalKeyPropertiesNotFoundInPrincipalPropertiesCache.GetValue(this, CsdlSemanticsReferentialConstraint.ComputePrincipalKeyPropertiesNotFoundInPrincipalPropertiesFunc, null);
			}
		}

		// Token: 0x170003F9 RID: 1017
		// (get) Token: 0x06000945 RID: 2373 RVA: 0x00018ACC File Offset: 0x00016CCC
		private IEnumerable<string> DependentPropertiesNotFoundInDependentType
		{
			get
			{
				return this.dependentPropertiesNotFoundInDependentTypeCache.GetValue(this, CsdlSemanticsReferentialConstraint.ComputeDependentPropertiesNotFoundInDependentTypeFunc, null);
			}
		}

		// Token: 0x06000946 RID: 2374 RVA: 0x00018AE0 File Offset: 0x00016CE0
		private IEnumerable<EdmError> ComputeErrors()
		{
			List<EdmError> list = null;
			IEdmEntityType entityType = this.PrincipalEnd.EntityType;
			CsdlReferentialConstraintRole principal = this.constraint.Principal;
			CsdlReferentialConstraintRole dependent = this.constraint.Dependent;
			if (this.constraint.Principal.Role == this.constraint.Dependent.Role)
			{
				list = CsdlSemanticsElement.AllocateAndAdd<EdmError>(list, new EdmError(base.Location, EdmErrorCode.SameRoleReferredInReferentialConstraint, Strings.EdmModel_Validator_Semantic_SameRoleReferredInReferentialConstraint(this.constraint.Principal.Role)));
			}
			if (this.constraint.Dependent.Role != this.context.End1.Name && this.constraint.Dependent.Role != this.context.End2.Name)
			{
				list = CsdlSemanticsElement.AllocateAndAdd<EdmError>(list, new EdmError(base.Location, EdmErrorCode.InvalidRoleInRelationshipConstraint, Strings.CsdlParser_InvalidEndRoleInRelationshipConstraint(this.constraint.Dependent.Role, this.context.Name)));
			}
			if (this.constraint.Principal.Role != this.context.End1.Name && this.constraint.Principal.Role != this.context.End2.Name)
			{
				list = CsdlSemanticsElement.AllocateAndAdd<EdmError>(list, new EdmError(base.Location, EdmErrorCode.InvalidRoleInRelationshipConstraint, Strings.CsdlParser_InvalidEndRoleInRelationshipConstraint(this.constraint.Principal.Role, this.context.Name)));
			}
			if (list == null)
			{
				if (principal.Properties.Count<CsdlPropertyReference>() != dependent.Properties.Count<CsdlPropertyReference>())
				{
					list = CsdlSemanticsElement.AllocateAndAdd<EdmError>(list, new EdmError(base.Location, EdmErrorCode.MismatchNumberOfPropertiesInRelationshipConstraint, Strings.EdmModel_Validator_Semantic_MismatchNumberOfPropertiesinRelationshipConstraint));
				}
				if (entityType.Key().Count<IEdmStructuralProperty>() != principal.Properties.Count<CsdlPropertyReference>() || this.PrincipalKeyPropertiesNotFoundInPrincipalProperties.Count<string>() != 0)
				{
					string errorMessage = Strings.EdmModel_Validator_Semantic_InvalidPropertyInRelationshipConstraintPrimaryEnd(this.DependentEnd.DeclaringAssociation.Namespace + '.' + this.DependentEnd.DeclaringAssociation.Name, this.constraint.Principal.Role);
					list = CsdlSemanticsElement.AllocateAndAdd<EdmError>(list, new EdmError(base.Location, EdmErrorCode.BadPrincipalPropertiesInReferentialConstraint, errorMessage));
				}
				foreach (string p in this.DependentPropertiesNotFoundInDependentType)
				{
					string errorMessage2 = Strings.EdmModel_Validator_Semantic_InvalidPropertyInRelationshipConstraintDependentEnd(p, this.constraint.Dependent.Role);
					list = CsdlSemanticsElement.AllocateAndAdd<EdmError>(list, new EdmError(base.Location, EdmErrorCode.InvalidPropertyInRelationshipConstraint, errorMessage2));
				}
			}
			return list ?? Enumerable.Empty<EdmError>();
		}

		// Token: 0x06000947 RID: 2375 RVA: 0x00018D98 File Offset: 0x00016F98
		private IEdmAssociationEnd ComputePrincipal()
		{
			IEdmAssociationEnd edmAssociationEnd = this.context.End1;
			if (edmAssociationEnd.Name != this.constraint.Principal.Role)
			{
				edmAssociationEnd = this.context.End2;
			}
			if (edmAssociationEnd.Name != this.constraint.Principal.Role)
			{
				edmAssociationEnd = new BadAssociationEnd(this.context, this.constraint.Principal.Role, new EdmError[]
				{
					new EdmError(base.Location, EdmErrorCode.BadNonComputableAssociationEnd, Strings.Bad_UncomputableAssociationEnd(this.constraint.Principal.Role))
				});
			}
			return edmAssociationEnd;
		}

		// Token: 0x06000948 RID: 2376 RVA: 0x00018E78 File Offset: 0x00017078
		private IEnumerable<IEdmStructuralProperty> ComputeDependentProperties()
		{
			List<IEdmStructuralProperty> list = new List<IEdmStructuralProperty>();
			IEdmEntityType entityType = this.DependentEnd.EntityType;
			IEdmEntityType principalRoleType = this.PrincipalEnd.EntityType;
			CsdlReferentialConstraintRole principal = this.constraint.Principal;
			CsdlReferentialConstraintRole dependent = this.constraint.Dependent;
			if (principalRoleType.Key().Count<IEdmStructuralProperty>() == principal.Properties.Count<CsdlPropertyReference>() && principal.Properties.Count<CsdlPropertyReference>() == dependent.Properties.Count<CsdlPropertyReference>() && this.PrincipalKeyPropertiesNotFoundInPrincipalProperties.Count<string>() == 0 && this.DependentPropertiesNotFoundInDependentType.Count<string>() == 0)
			{
				using (IEnumerator<IEdmStructuralProperty> enumerator = this.PrincipalEnd.EntityType.Key().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						IEdmStructuralProperty principalKeyProperty = enumerator.Current;
						CsdlPropertyReference reference2 = (from reference in principal.Properties
						where principalRoleType.FindProperty(reference.PropertyName).Equals(principalKeyProperty)
						select reference).FirstOrDefault<CsdlPropertyReference>();
						int index = principal.IndexOf(reference2);
						CsdlPropertyReference csdlPropertyReference = dependent.Properties.ElementAt(index);
						IEdmStructuralProperty item = entityType.FindProperty(csdlPropertyReference.PropertyName) as IEdmStructuralProperty;
						list.Add(item);
					}
					return list;
				}
			}
			list = new List<IEdmStructuralProperty>();
			foreach (CsdlPropertyReference csdlPropertyReference2 in dependent.Properties)
			{
				list.Add(new BadProperty(entityType, csdlPropertyReference2.PropertyName, new EdmError[]
				{
					new EdmError(base.Location, EdmErrorCode.TypeMismatchRelationshipConstraint, Strings.CsdlSemantics_ReferentialConstraintMismatch)
				}));
			}
			return list;
		}

		// Token: 0x06000949 RID: 2377 RVA: 0x0001907C File Offset: 0x0001727C
		private IEnumerable<string> ComputePrincipalKeyPropertiesNotFoundInPrincipalProperties()
		{
			List<string> list = null;
			using (IEnumerator<IEdmStructuralProperty> enumerator = this.PrincipalEnd.EntityType.Key().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					IEdmStructuralProperty principalKeyProperty = enumerator.Current;
					CsdlReferentialConstraintRole principal = this.constraint.Principal;
					if ((from reference in principal.Properties
					where reference.PropertyName == principalKeyProperty.Name
					select reference).FirstOrDefault<CsdlPropertyReference>() == null)
					{
						list = CsdlSemanticsElement.AllocateAndAdd<string>(list, principalKeyProperty.Name);
					}
				}
			}
			return list ?? Enumerable.Empty<string>();
		}

		// Token: 0x0600094A RID: 2378 RVA: 0x00019134 File Offset: 0x00017334
		private IEnumerable<string> ComputeDependentPropertiesNotFoundInDependentType()
		{
			List<string> list = new List<string>();
			IEdmEntityType entityType = this.DependentEnd.EntityType;
			foreach (CsdlPropertyReference csdlPropertyReference in this.constraint.Dependent.Properties)
			{
				if (entityType.FindProperty(csdlPropertyReference.PropertyName) == null)
				{
					list = CsdlSemanticsElement.AllocateAndAdd<string>(list, csdlPropertyReference.PropertyName);
				}
			}
			return list ?? Enumerable.Empty<string>();
		}

		// Token: 0x0400047D RID: 1149
		private readonly CsdlSemanticsAssociation context;

		// Token: 0x0400047E RID: 1150
		private readonly CsdlReferentialConstraint constraint;

		// Token: 0x0400047F RID: 1151
		private readonly Cache<CsdlSemanticsReferentialConstraint, IEdmAssociationEnd> principalCache = new Cache<CsdlSemanticsReferentialConstraint, IEdmAssociationEnd>();

		// Token: 0x04000480 RID: 1152
		private static readonly Func<CsdlSemanticsReferentialConstraint, IEdmAssociationEnd> ComputePrincipalFunc = (CsdlSemanticsReferentialConstraint me) => me.ComputePrincipal();

		// Token: 0x04000481 RID: 1153
		private readonly Cache<CsdlSemanticsReferentialConstraint, IEnumerable<IEdmStructuralProperty>> dependentPropertiesCache = new Cache<CsdlSemanticsReferentialConstraint, IEnumerable<IEdmStructuralProperty>>();

		// Token: 0x04000482 RID: 1154
		private static readonly Func<CsdlSemanticsReferentialConstraint, IEnumerable<IEdmStructuralProperty>> ComputeDependentPropertiesFunc = (CsdlSemanticsReferentialConstraint me) => me.ComputeDependentProperties();

		// Token: 0x04000483 RID: 1155
		private readonly Cache<CsdlSemanticsReferentialConstraint, IEnumerable<EdmError>> errorsCache = new Cache<CsdlSemanticsReferentialConstraint, IEnumerable<EdmError>>();

		// Token: 0x04000484 RID: 1156
		private static readonly Func<CsdlSemanticsReferentialConstraint, IEnumerable<EdmError>> ComputeErrorsFunc = (CsdlSemanticsReferentialConstraint me) => me.ComputeErrors();

		// Token: 0x04000485 RID: 1157
		private readonly Cache<CsdlSemanticsReferentialConstraint, IEnumerable<string>> principalKeyPropertiesNotFoundInPrincipalPropertiesCache = new Cache<CsdlSemanticsReferentialConstraint, IEnumerable<string>>();

		// Token: 0x04000486 RID: 1158
		private static readonly Func<CsdlSemanticsReferentialConstraint, IEnumerable<string>> ComputePrincipalKeyPropertiesNotFoundInPrincipalPropertiesFunc = (CsdlSemanticsReferentialConstraint me) => me.ComputePrincipalKeyPropertiesNotFoundInPrincipalProperties();

		// Token: 0x04000487 RID: 1159
		private readonly Cache<CsdlSemanticsReferentialConstraint, IEnumerable<string>> dependentPropertiesNotFoundInDependentTypeCache = new Cache<CsdlSemanticsReferentialConstraint, IEnumerable<string>>();

		// Token: 0x04000488 RID: 1160
		private static readonly Func<CsdlSemanticsReferentialConstraint, IEnumerable<string>> ComputeDependentPropertiesNotFoundInDependentTypeFunc = (CsdlSemanticsReferentialConstraint me) => me.ComputeDependentPropertiesNotFoundInDependentType();
	}
}
