using System;
using System.Collections.Generic;

namespace Microsoft.Data.Edm.Library
{
	// Token: 0x020001D0 RID: 464
	public class EdmEntityType : EdmStructuredType, IEdmEntityType, IEdmStructuredType, IEdmSchemaType, IEdmType, IEdmTerm, IEdmSchemaElement, IEdmNamedElement, IEdmVocabularyAnnotatable, IEdmElement
	{
		// Token: 0x06000AFA RID: 2810 RVA: 0x000202A0 File Offset: 0x0001E4A0
		public EdmEntityType(string namespaceName, string name) : this(namespaceName, name, null, false, false)
		{
		}

		// Token: 0x06000AFB RID: 2811 RVA: 0x000202AD File Offset: 0x0001E4AD
		public EdmEntityType(string namespaceName, string name, IEdmEntityType baseType) : this(namespaceName, name, baseType, false, false)
		{
		}

		// Token: 0x06000AFC RID: 2812 RVA: 0x000202BA File Offset: 0x0001E4BA
		public EdmEntityType(string namespaceName, string name, IEdmEntityType baseType, bool isAbstract, bool isOpen) : base(isAbstract, isOpen, baseType)
		{
			EdmUtil.CheckArgumentNull<string>(namespaceName, "namespaceName");
			EdmUtil.CheckArgumentNull<string>(name, "name");
			this.namespaceName = namespaceName;
			this.name = name;
		}

		// Token: 0x1700042D RID: 1069
		// (get) Token: 0x06000AFD RID: 2813 RVA: 0x000202ED File Offset: 0x0001E4ED
		public virtual IEnumerable<IEdmStructuralProperty> DeclaredKey
		{
			get
			{
				return this.declaredKey;
			}
		}

		// Token: 0x1700042E RID: 1070
		// (get) Token: 0x06000AFE RID: 2814 RVA: 0x000202F5 File Offset: 0x0001E4F5
		public EdmSchemaElementKind SchemaElementKind
		{
			get
			{
				return EdmSchemaElementKind.TypeDefinition;
			}
		}

		// Token: 0x1700042F RID: 1071
		// (get) Token: 0x06000AFF RID: 2815 RVA: 0x000202F8 File Offset: 0x0001E4F8
		public string Namespace
		{
			get
			{
				return this.namespaceName;
			}
		}

		// Token: 0x17000430 RID: 1072
		// (get) Token: 0x06000B00 RID: 2816 RVA: 0x00020300 File Offset: 0x0001E500
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x17000431 RID: 1073
		// (get) Token: 0x06000B01 RID: 2817 RVA: 0x00020308 File Offset: 0x0001E508
		public override EdmTypeKind TypeKind
		{
			get
			{
				return EdmTypeKind.Entity;
			}
		}

		// Token: 0x17000432 RID: 1074
		// (get) Token: 0x06000B02 RID: 2818 RVA: 0x0002030B File Offset: 0x0001E50B
		public EdmTermKind TermKind
		{
			get
			{
				return EdmTermKind.Type;
			}
		}

		// Token: 0x06000B03 RID: 2819 RVA: 0x0002030E File Offset: 0x0001E50E
		public void AddKeys(params IEdmStructuralProperty[] keyProperties)
		{
			this.AddKeys((IEnumerable<IEdmStructuralProperty>)keyProperties);
		}

		// Token: 0x06000B04 RID: 2820 RVA: 0x0002031C File Offset: 0x0001E51C
		public void AddKeys(IEnumerable<IEdmStructuralProperty> keyProperties)
		{
			EdmUtil.CheckArgumentNull<IEnumerable<IEdmStructuralProperty>>(keyProperties, "keyProperties");
			foreach (IEdmStructuralProperty item in keyProperties)
			{
				if (this.declaredKey == null)
				{
					this.declaredKey = new List<IEdmStructuralProperty>();
				}
				this.declaredKey.Add(item);
			}
		}

		// Token: 0x06000B05 RID: 2821 RVA: 0x00020388 File Offset: 0x0001E588
		public EdmNavigationProperty AddUnidirectionalNavigation(EdmNavigationPropertyInfo propertyInfo)
		{
			return this.AddUnidirectionalNavigation(propertyInfo, this.FixUpDefaultPartnerInfo(propertyInfo, null));
		}

		// Token: 0x06000B06 RID: 2822 RVA: 0x0002039C File Offset: 0x0001E59C
		public EdmNavigationProperty AddUnidirectionalNavigation(EdmNavigationPropertyInfo propertyInfo, EdmNavigationPropertyInfo partnerInfo)
		{
			EdmUtil.CheckArgumentNull<EdmNavigationPropertyInfo>(propertyInfo, "propertyInfo");
			EdmNavigationProperty edmNavigationProperty = EdmNavigationProperty.CreateNavigationPropertyWithPartner(propertyInfo, this.FixUpDefaultPartnerInfo(propertyInfo, partnerInfo));
			base.AddProperty(edmNavigationProperty);
			return edmNavigationProperty;
		}

		// Token: 0x06000B07 RID: 2823 RVA: 0x000203CC File Offset: 0x0001E5CC
		public EdmNavigationProperty AddBidirectionalNavigation(EdmNavigationPropertyInfo propertyInfo, EdmNavigationPropertyInfo partnerInfo)
		{
			EdmUtil.CheckArgumentNull<EdmNavigationPropertyInfo>(propertyInfo, "propertyInfo");
			EdmUtil.CheckArgumentNull<IEdmEntityType>(propertyInfo.Target, "propertyInfo.Target");
			EdmEntityType edmEntityType = propertyInfo.Target as EdmEntityType;
			if (edmEntityType == null)
			{
				throw new ArgumentException("propertyInfo.Target", Strings.Constructable_TargetMustBeStock(typeof(EdmEntityType).FullName));
			}
			EdmNavigationProperty edmNavigationProperty = EdmNavigationProperty.CreateNavigationPropertyWithPartner(propertyInfo, this.FixUpDefaultPartnerInfo(propertyInfo, partnerInfo));
			base.AddProperty(edmNavigationProperty);
			edmEntityType.AddProperty(edmNavigationProperty.Partner);
			return edmNavigationProperty;
		}

		// Token: 0x06000B08 RID: 2824 RVA: 0x00020448 File Offset: 0x0001E648
		private EdmNavigationPropertyInfo FixUpDefaultPartnerInfo(EdmNavigationPropertyInfo propertyInfo, EdmNavigationPropertyInfo partnerInfo)
		{
			EdmNavigationPropertyInfo edmNavigationPropertyInfo = null;
			if (partnerInfo == null)
			{
				edmNavigationPropertyInfo = (partnerInfo = new EdmNavigationPropertyInfo());
			}
			if (partnerInfo.Name == null)
			{
				if (edmNavigationPropertyInfo == null)
				{
					edmNavigationPropertyInfo = partnerInfo.Clone();
				}
				edmNavigationPropertyInfo.Name = (propertyInfo.Name ?? string.Empty) + "Partner";
			}
			if (partnerInfo.Target == null)
			{
				if (edmNavigationPropertyInfo == null)
				{
					edmNavigationPropertyInfo = partnerInfo.Clone();
				}
				edmNavigationPropertyInfo.Target = this;
			}
			if (partnerInfo.TargetMultiplicity == EdmMultiplicity.Unknown)
			{
				if (edmNavigationPropertyInfo == null)
				{
					edmNavigationPropertyInfo = partnerInfo.Clone();
				}
				edmNavigationPropertyInfo.TargetMultiplicity = EdmMultiplicity.ZeroOrOne;
			}
			return edmNavigationPropertyInfo ?? partnerInfo;
		}

		// Token: 0x0400052D RID: 1325
		private readonly string namespaceName;

		// Token: 0x0400052E RID: 1326
		private readonly string name;

		// Token: 0x0400052F RID: 1327
		private List<IEdmStructuralProperty> declaredKey;
	}
}
