using System;
using System.Collections.Generic;
using System.Data.Services.Common;
using System.Linq;
using Microsoft.Data.Edm;

namespace Microsoft.Data.OData.Metadata
{
	// Token: 0x02000203 RID: 515
	internal sealed class ODataEntityPropertyMappingCache
	{
		// Token: 0x06000FC3 RID: 4035 RVA: 0x00039454 File Offset: 0x00037654
		internal ODataEntityPropertyMappingCache(ODataEntityPropertyMappingCollection mappings, IEdmModel model, int totalMappingCount)
		{
			this.mappings = mappings;
			this.model = model;
			this.totalMappingCount = totalMappingCount;
			this.mappingsForInheritedProperties = new List<EntityPropertyMappingAttribute>();
			this.mappingsForDeclaredProperties = ((mappings == null) ? new List<EntityPropertyMappingAttribute>() : new List<EntityPropertyMappingAttribute>(mappings));
			this.epmTargetTree = new EpmTargetTree();
			this.epmSourceTree = new EpmSourceTree(this.epmTargetTree);
		}

		// Token: 0x17000347 RID: 839
		// (get) Token: 0x06000FC4 RID: 4036 RVA: 0x000394B9 File Offset: 0x000376B9
		internal List<EntityPropertyMappingAttribute> MappingsForInheritedProperties
		{
			get
			{
				return this.mappingsForInheritedProperties;
			}
		}

		// Token: 0x17000348 RID: 840
		// (get) Token: 0x06000FC5 RID: 4037 RVA: 0x000394C1 File Offset: 0x000376C1
		internal List<EntityPropertyMappingAttribute> MappingsForDeclaredProperties
		{
			get
			{
				return this.mappingsForDeclaredProperties;
			}
		}

		// Token: 0x17000349 RID: 841
		// (get) Token: 0x06000FC6 RID: 4038 RVA: 0x000394C9 File Offset: 0x000376C9
		internal EpmSourceTree EpmSourceTree
		{
			get
			{
				return this.epmSourceTree;
			}
		}

		// Token: 0x1700034A RID: 842
		// (get) Token: 0x06000FC7 RID: 4039 RVA: 0x000394D1 File Offset: 0x000376D1
		internal EpmTargetTree EpmTargetTree
		{
			get
			{
				return this.epmTargetTree;
			}
		}

		// Token: 0x1700034B RID: 843
		// (get) Token: 0x06000FC8 RID: 4040 RVA: 0x000394D9 File Offset: 0x000376D9
		internal IEnumerable<EntityPropertyMappingAttribute> AllMappings
		{
			get
			{
				return this.MappingsForDeclaredProperties.Concat(this.MappingsForInheritedProperties);
			}
		}

		// Token: 0x1700034C RID: 844
		// (get) Token: 0x06000FC9 RID: 4041 RVA: 0x000394EC File Offset: 0x000376EC
		internal int TotalMappingCount
		{
			get
			{
				return this.totalMappingCount;
			}
		}

		// Token: 0x06000FCA RID: 4042 RVA: 0x000394F4 File Offset: 0x000376F4
		internal void BuildEpmForType(IEdmEntityType definingEntityType, IEdmEntityType affectedEntityType)
		{
			if (definingEntityType.BaseType != null)
			{
				this.BuildEpmForType(definingEntityType.BaseEntityType(), affectedEntityType);
			}
			ODataEntityPropertyMappingCollection entityPropertyMappings = this.model.GetEntityPropertyMappings(definingEntityType);
			if (entityPropertyMappings == null)
			{
				return;
			}
			foreach (EntityPropertyMappingAttribute entityPropertyMappingAttribute in entityPropertyMappings)
			{
				this.epmSourceTree.Add(new EntityPropertyMappingInfo(entityPropertyMappingAttribute, definingEntityType, affectedEntityType));
				if (definingEntityType == affectedEntityType && !ODataEntityPropertyMappingCache.PropertyExistsOnType(affectedEntityType, entityPropertyMappingAttribute))
				{
					this.MappingsForInheritedProperties.Add(entityPropertyMappingAttribute);
					this.MappingsForDeclaredProperties.Remove(entityPropertyMappingAttribute);
				}
			}
		}

		// Token: 0x06000FCB RID: 4043 RVA: 0x00039594 File Offset: 0x00037794
		internal bool IsDirty(ODataEntityPropertyMappingCollection propertyMappings)
		{
			return (this.mappings != null || propertyMappings != null) && (!object.ReferenceEquals(this.mappings, propertyMappings) || this.mappings.Count != propertyMappings.Count);
		}

		// Token: 0x06000FCC RID: 4044 RVA: 0x000395E4 File Offset: 0x000377E4
		private static bool PropertyExistsOnType(IEdmStructuredType structuredType, EntityPropertyMappingAttribute epmAttribute)
		{
			int num = epmAttribute.SourcePath.IndexOf('/');
			string propertyToLookFor = (num == -1) ? epmAttribute.SourcePath : epmAttribute.SourcePath.Substring(0, num);
			return structuredType.DeclaredProperties.Any((IEdmProperty p) => p.Name == propertyToLookFor);
		}

		// Token: 0x040005B7 RID: 1463
		private readonly ODataEntityPropertyMappingCollection mappings;

		// Token: 0x040005B8 RID: 1464
		private readonly List<EntityPropertyMappingAttribute> mappingsForInheritedProperties;

		// Token: 0x040005B9 RID: 1465
		private readonly List<EntityPropertyMappingAttribute> mappingsForDeclaredProperties;

		// Token: 0x040005BA RID: 1466
		private readonly EpmSourceTree epmSourceTree;

		// Token: 0x040005BB RID: 1467
		private readonly EpmTargetTree epmTargetTree;

		// Token: 0x040005BC RID: 1468
		private readonly IEdmModel model;

		// Token: 0x040005BD RID: 1469
		private readonly int totalMappingCount;
	}
}
