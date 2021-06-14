using System;
using System.Collections.Generic;
using Microsoft.Data.Edm.Annotations;
using Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast;
using Microsoft.Data.Edm.Internal;
using Microsoft.Data.Edm.Library;

namespace Microsoft.Data.Edm.Csdl.Internal.CsdlSemantics
{
	// Token: 0x0200016E RID: 366
	internal class CsdlSemanticsEntitySet : CsdlSemanticsElement, IEdmEntitySet, IEdmEntityContainerElement, IEdmNamedElement, IEdmVocabularyAnnotatable, IEdmElement
	{
		// Token: 0x060007D7 RID: 2007 RVA: 0x0001557D File Offset: 0x0001377D
		public CsdlSemanticsEntitySet(CsdlSemanticsEntityContainer container, CsdlEntitySet entitySet) : base(entitySet)
		{
			this.container = container;
			this.entitySet = entitySet;
		}

		// Token: 0x1700032B RID: 811
		// (get) Token: 0x060007D8 RID: 2008 RVA: 0x000155AA File Offset: 0x000137AA
		public override CsdlSemanticsModel Model
		{
			get
			{
				return this.container.Model;
			}
		}

		// Token: 0x1700032C RID: 812
		// (get) Token: 0x060007D9 RID: 2009 RVA: 0x000155B7 File Offset: 0x000137B7
		public IEdmEntityContainer Container
		{
			get
			{
				return this.container;
			}
		}

		// Token: 0x1700032D RID: 813
		// (get) Token: 0x060007DA RID: 2010 RVA: 0x000155BF File Offset: 0x000137BF
		public override CsdlElement Element
		{
			get
			{
				return this.entitySet;
			}
		}

		// Token: 0x1700032E RID: 814
		// (get) Token: 0x060007DB RID: 2011 RVA: 0x000155C7 File Offset: 0x000137C7
		public string Name
		{
			get
			{
				return this.entitySet.Name;
			}
		}

		// Token: 0x1700032F RID: 815
		// (get) Token: 0x060007DC RID: 2012 RVA: 0x000155D4 File Offset: 0x000137D4
		public IEdmEntityType ElementType
		{
			get
			{
				return this.elementTypeCache.GetValue(this, CsdlSemanticsEntitySet.ComputeElementTypeFunc, null);
			}
		}

		// Token: 0x17000330 RID: 816
		// (get) Token: 0x060007DD RID: 2013 RVA: 0x000155E8 File Offset: 0x000137E8
		public EdmContainerElementKind ContainerElementKind
		{
			get
			{
				return EdmContainerElementKind.EntitySet;
			}
		}

		// Token: 0x17000331 RID: 817
		// (get) Token: 0x060007DE RID: 2014 RVA: 0x000155EB File Offset: 0x000137EB
		public IEnumerable<IEdmNavigationTargetMapping> NavigationTargets
		{
			get
			{
				return this.navigationTargetsCache.GetValue(this, CsdlSemanticsEntitySet.ComputeNavigationTargetsFunc, null);
			}
		}

		// Token: 0x060007DF RID: 2015 RVA: 0x00015600 File Offset: 0x00013800
		public IEdmEntitySet FindNavigationTarget(IEdmNavigationProperty property)
		{
			foreach (IEdmNavigationTargetMapping edmNavigationTargetMapping in this.NavigationTargets)
			{
				if (edmNavigationTargetMapping.NavigationProperty == property)
				{
					return edmNavigationTargetMapping.TargetEntitySet;
				}
			}
			return null;
		}

		// Token: 0x060007E0 RID: 2016 RVA: 0x0001565C File Offset: 0x0001385C
		protected override IEnumerable<IEdmVocabularyAnnotation> ComputeInlineVocabularyAnnotations()
		{
			return this.Model.WrapInlineVocabularyAnnotations(this, this.container.Context);
		}

		// Token: 0x060007E1 RID: 2017 RVA: 0x00015675 File Offset: 0x00013875
		private IEdmEntityType ComputeElementType()
		{
			return (this.container.Context.FindType(this.entitySet.EntityType) as IEdmEntityType) ?? new UnresolvedEntityType(this.entitySet.EntityType, base.Location);
		}

		// Token: 0x060007E2 RID: 2018 RVA: 0x000156B4 File Offset: 0x000138B4
		private IEnumerable<IEdmNavigationTargetMapping> ComputeNavigationTargets()
		{
			List<IEdmNavigationTargetMapping> list = new List<IEdmNavigationTargetMapping>();
			foreach (IEdmNavigationProperty edmNavigationProperty in this.ElementType.NavigationProperties())
			{
				IEdmEntitySet edmEntitySet = this.FindNavigationTargetHelper(edmNavigationProperty);
				if (edmEntitySet != null)
				{
					list.Add(new EdmNavigationTargetMapping(edmNavigationProperty, edmEntitySet));
				}
			}
			foreach (IEdmStructuredType edmStructuredType in this.Model.FindAllDerivedTypes(this.ElementType))
			{
				IEdmEntityType type = (IEdmEntityType)edmStructuredType;
				foreach (IEdmNavigationProperty edmNavigationProperty2 in type.DeclaredNavigationProperties())
				{
					IEdmEntitySet edmEntitySet2 = this.FindNavigationTargetHelper(edmNavigationProperty2);
					if (edmEntitySet2 != null)
					{
						list.Add(new EdmNavigationTargetMapping(edmNavigationProperty2, edmEntitySet2));
					}
				}
			}
			return list;
		}

		// Token: 0x060007E3 RID: 2019 RVA: 0x000157C8 File Offset: 0x000139C8
		private IEdmEntitySet FindNavigationTargetHelper(IEdmNavigationProperty property)
		{
			CsdlSemanticsNavigationProperty csdlSemanticsNavigationProperty = property as CsdlSemanticsNavigationProperty;
			if (csdlSemanticsNavigationProperty != null)
			{
				foreach (IEdmEntityContainer edmEntityContainer in this.Model.EntityContainers())
				{
					CsdlSemanticsEntityContainer csdlSemanticsEntityContainer = (CsdlSemanticsEntityContainer)edmEntityContainer;
					IEnumerable<CsdlSemanticsAssociationSet> enumerable = csdlSemanticsEntityContainer.FindAssociationSets(csdlSemanticsNavigationProperty.Association);
					if (enumerable != null)
					{
						foreach (CsdlSemanticsAssociationSet csdlSemanticsAssociationSet in enumerable)
						{
							CsdlSemanticsAssociationSetEnd csdlSemanticsAssociationSetEnd = csdlSemanticsAssociationSet.End1 as CsdlSemanticsAssociationSetEnd;
							CsdlSemanticsAssociationSetEnd csdlSemanticsAssociationSetEnd2 = csdlSemanticsAssociationSet.End2 as CsdlSemanticsAssociationSetEnd;
							if (csdlSemanticsAssociationSet.End1.EntitySet == this && csdlSemanticsNavigationProperty.To == csdlSemanticsAssociationSet.End2.Role)
							{
								this.Model.SetAssociationSetName(csdlSemanticsAssociationSet.End1.EntitySet, property, csdlSemanticsAssociationSet.Name);
								if (csdlSemanticsAssociationSetEnd != null && csdlSemanticsAssociationSetEnd2 != null)
								{
									this.Model.SetAssociationSetAnnotations(csdlSemanticsAssociationSetEnd.EntitySet, property, csdlSemanticsAssociationSet.DirectValueAnnotations, csdlSemanticsAssociationSetEnd.DirectValueAnnotations, csdlSemanticsAssociationSetEnd2.DirectValueAnnotations);
								}
								return csdlSemanticsAssociationSet.End2.EntitySet;
							}
							if (csdlSemanticsAssociationSet.End2.EntitySet == this && csdlSemanticsNavigationProperty.To == csdlSemanticsAssociationSet.End1.Role)
							{
								this.Model.SetAssociationSetName(csdlSemanticsAssociationSet.End2.EntitySet, property, csdlSemanticsAssociationSet.Name);
								if (csdlSemanticsAssociationSetEnd != null && csdlSemanticsAssociationSetEnd2 != null)
								{
									this.Model.SetAssociationSetAnnotations(csdlSemanticsAssociationSetEnd2.EntitySet, property, csdlSemanticsAssociationSet.DirectValueAnnotations, csdlSemanticsAssociationSetEnd2.DirectValueAnnotations, csdlSemanticsAssociationSetEnd.DirectValueAnnotations);
								}
								return csdlSemanticsAssociationSet.End1.EntitySet;
							}
						}
					}
				}
			}
			return null;
		}

		// Token: 0x040003F4 RID: 1012
		private readonly CsdlEntitySet entitySet;

		// Token: 0x040003F5 RID: 1013
		private readonly CsdlSemanticsEntityContainer container;

		// Token: 0x040003F6 RID: 1014
		private readonly Cache<CsdlSemanticsEntitySet, IEdmEntityType> elementTypeCache = new Cache<CsdlSemanticsEntitySet, IEdmEntityType>();

		// Token: 0x040003F7 RID: 1015
		private static readonly Func<CsdlSemanticsEntitySet, IEdmEntityType> ComputeElementTypeFunc = (CsdlSemanticsEntitySet me) => me.ComputeElementType();

		// Token: 0x040003F8 RID: 1016
		private readonly Cache<CsdlSemanticsEntitySet, IEnumerable<IEdmNavigationTargetMapping>> navigationTargetsCache = new Cache<CsdlSemanticsEntitySet, IEnumerable<IEdmNavigationTargetMapping>>();

		// Token: 0x040003F9 RID: 1017
		private static readonly Func<CsdlSemanticsEntitySet, IEnumerable<IEdmNavigationTargetMapping>> ComputeNavigationTargetsFunc = (CsdlSemanticsEntitySet me) => me.ComputeNavigationTargets();
	}
}
