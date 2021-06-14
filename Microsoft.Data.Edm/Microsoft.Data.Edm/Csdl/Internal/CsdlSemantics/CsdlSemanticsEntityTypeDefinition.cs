using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast;
using Microsoft.Data.Edm.Internal;
using Microsoft.Data.Edm.Library.Internal;

namespace Microsoft.Data.Edm.Csdl.Internal.CsdlSemantics
{
	// Token: 0x0200016F RID: 367
	internal class CsdlSemanticsEntityTypeDefinition : CsdlSemanticsStructuredTypeDefinition, IEdmEntityType, IEdmStructuredType, IEdmSchemaType, IEdmType, IEdmTerm, IEdmSchemaElement, IEdmNamedElement, IEdmVocabularyAnnotatable, IEdmElement
	{
		// Token: 0x060007E7 RID: 2023 RVA: 0x00015A11 File Offset: 0x00013C11
		public CsdlSemanticsEntityTypeDefinition(CsdlSemanticsSchema context, CsdlEntityType entity) : base(context, entity)
		{
			this.entity = entity;
		}

		// Token: 0x17000332 RID: 818
		// (get) Token: 0x060007E8 RID: 2024 RVA: 0x00015A38 File Offset: 0x00013C38
		public override IEdmStructuredType BaseType
		{
			get
			{
				return this.baseTypeCache.GetValue(this, CsdlSemanticsEntityTypeDefinition.ComputeBaseTypeFunc, CsdlSemanticsEntityTypeDefinition.OnCycleBaseTypeFunc);
			}
		}

		// Token: 0x17000333 RID: 819
		// (get) Token: 0x060007E9 RID: 2025 RVA: 0x00015A50 File Offset: 0x00013C50
		public override EdmTypeKind TypeKind
		{
			get
			{
				return EdmTypeKind.Entity;
			}
		}

		// Token: 0x17000334 RID: 820
		// (get) Token: 0x060007EA RID: 2026 RVA: 0x00015A53 File Offset: 0x00013C53
		public string Name
		{
			get
			{
				return this.entity.Name;
			}
		}

		// Token: 0x17000335 RID: 821
		// (get) Token: 0x060007EB RID: 2027 RVA: 0x00015A60 File Offset: 0x00013C60
		public override bool IsAbstract
		{
			get
			{
				return this.entity.IsAbstract;
			}
		}

		// Token: 0x17000336 RID: 822
		// (get) Token: 0x060007EC RID: 2028 RVA: 0x00015A6D File Offset: 0x00013C6D
		public override bool IsOpen
		{
			get
			{
				return this.entity.IsOpen;
			}
		}

		// Token: 0x17000337 RID: 823
		// (get) Token: 0x060007ED RID: 2029 RVA: 0x00015A7A File Offset: 0x00013C7A
		public IEnumerable<IEdmStructuralProperty> DeclaredKey
		{
			get
			{
				return this.declaredKeyCache.GetValue(this, CsdlSemanticsEntityTypeDefinition.ComputeDeclaredKeyFunc, null);
			}
		}

		// Token: 0x17000338 RID: 824
		// (get) Token: 0x060007EE RID: 2030 RVA: 0x00015A8E File Offset: 0x00013C8E
		public EdmTermKind TermKind
		{
			get
			{
				return EdmTermKind.Type;
			}
		}

		// Token: 0x17000339 RID: 825
		// (get) Token: 0x060007EF RID: 2031 RVA: 0x00015A91 File Offset: 0x00013C91
		protected override CsdlStructuredType MyStructured
		{
			get
			{
				return this.entity;
			}
		}

		// Token: 0x060007F0 RID: 2032 RVA: 0x00015A9C File Offset: 0x00013C9C
		protected override List<IEdmProperty> ComputeDeclaredProperties()
		{
			List<IEdmProperty> list = base.ComputeDeclaredProperties();
			foreach (CsdlNavigationProperty navigationProperty in this.entity.NavigationProperties)
			{
				list.Add(new CsdlSemanticsNavigationProperty(this, navigationProperty));
			}
			return list;
		}

		// Token: 0x060007F1 RID: 2033 RVA: 0x00015AFC File Offset: 0x00013CFC
		private IEdmEntityType ComputeBaseType()
		{
			if (this.entity.BaseTypeName != null)
			{
				IEdmEntityType edmEntityType = base.Context.FindType(this.entity.BaseTypeName) as IEdmEntityType;
				if (edmEntityType != null)
				{
					IEdmStructuredType baseType = edmEntityType.BaseType;
				}
				return edmEntityType ?? new UnresolvedEntityType(base.Context.UnresolvedName(this.entity.BaseTypeName), base.Location);
			}
			return null;
		}

		// Token: 0x060007F2 RID: 2034 RVA: 0x00015B84 File Offset: 0x00013D84
		private IEnumerable<IEdmStructuralProperty> ComputeDeclaredKey()
		{
			if (this.entity.Key != null)
			{
				List<IEdmStructuralProperty> list = new List<IEdmStructuralProperty>();
				using (IEnumerator<CsdlPropertyReference> enumerator = this.entity.Key.Properties.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						CsdlPropertyReference keyProperty = enumerator.Current;
						IEdmStructuralProperty edmStructuralProperty = base.FindProperty(keyProperty.PropertyName) as IEdmStructuralProperty;
						if (edmStructuralProperty != null)
						{
							list.Add(edmStructuralProperty);
						}
						else
						{
							edmStructuralProperty = (base.DeclaredProperties.SingleOrDefault((IEdmProperty p) => p.Name == keyProperty.PropertyName) as IEdmStructuralProperty);
							if (edmStructuralProperty != null)
							{
								list.Add(edmStructuralProperty);
							}
							else
							{
								list.Add(new UnresolvedProperty(this, keyProperty.PropertyName, base.Location));
							}
						}
					}
				}
				return list;
			}
			return null;
		}

		// Token: 0x040003FC RID: 1020
		private readonly CsdlEntityType entity;

		// Token: 0x040003FD RID: 1021
		private readonly Cache<CsdlSemanticsEntityTypeDefinition, IEdmEntityType> baseTypeCache = new Cache<CsdlSemanticsEntityTypeDefinition, IEdmEntityType>();

		// Token: 0x040003FE RID: 1022
		private static readonly Func<CsdlSemanticsEntityTypeDefinition, IEdmEntityType> ComputeBaseTypeFunc = (CsdlSemanticsEntityTypeDefinition me) => me.ComputeBaseType();

		// Token: 0x040003FF RID: 1023
		private static readonly Func<CsdlSemanticsEntityTypeDefinition, IEdmEntityType> OnCycleBaseTypeFunc = (CsdlSemanticsEntityTypeDefinition me) => new CyclicEntityType(me.GetCyclicBaseTypeName(me.entity.BaseTypeName), me.Location);

		// Token: 0x04000400 RID: 1024
		private readonly Cache<CsdlSemanticsEntityTypeDefinition, IEnumerable<IEdmStructuralProperty>> declaredKeyCache = new Cache<CsdlSemanticsEntityTypeDefinition, IEnumerable<IEdmStructuralProperty>>();

		// Token: 0x04000401 RID: 1025
		private static readonly Func<CsdlSemanticsEntityTypeDefinition, IEnumerable<IEdmStructuralProperty>> ComputeDeclaredKeyFunc = (CsdlSemanticsEntityTypeDefinition me) => me.ComputeDeclaredKey();
	}
}
