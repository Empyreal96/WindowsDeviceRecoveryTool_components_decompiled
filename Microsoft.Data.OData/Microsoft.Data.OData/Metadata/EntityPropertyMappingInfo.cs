using System;
using System.Data.Services.Common;
using System.Diagnostics;
using Microsoft.Data.Edm;

namespace Microsoft.Data.OData.Metadata
{
	// Token: 0x02000265 RID: 613
	[DebuggerDisplay("EntityPropertyMappingInfo {DefiningType}")]
	internal sealed class EntityPropertyMappingInfo
	{
		// Token: 0x0600143C RID: 5180 RVA: 0x0004B427 File Offset: 0x00049627
		internal EntityPropertyMappingInfo(EntityPropertyMappingAttribute attribute, IEdmEntityType definingType, IEdmEntityType actualTypeDeclaringProperty)
		{
			this.attribute = attribute;
			this.definingType = definingType;
			this.actualPropertyType = actualTypeDeclaringProperty;
			this.isSyndicationMapping = (this.attribute.TargetSyndicationItem != SyndicationItemProperty.CustomProperty);
		}

		// Token: 0x1700041B RID: 1051
		// (get) Token: 0x0600143D RID: 5181 RVA: 0x0004B45B File Offset: 0x0004965B
		internal EntityPropertyMappingAttribute Attribute
		{
			get
			{
				return this.attribute;
			}
		}

		// Token: 0x1700041C RID: 1052
		// (get) Token: 0x0600143E RID: 5182 RVA: 0x0004B463 File Offset: 0x00049663
		internal IEdmEntityType DefiningType
		{
			get
			{
				return this.definingType;
			}
		}

		// Token: 0x1700041D RID: 1053
		// (get) Token: 0x0600143F RID: 5183 RVA: 0x0004B46B File Offset: 0x0004966B
		internal IEdmEntityType ActualPropertyType
		{
			get
			{
				return this.actualPropertyType;
			}
		}

		// Token: 0x1700041E RID: 1054
		// (get) Token: 0x06001440 RID: 5184 RVA: 0x0004B473 File Offset: 0x00049673
		internal EpmSourcePathSegment[] PropertyValuePath
		{
			get
			{
				return this.propertyValuePath;
			}
		}

		// Token: 0x1700041F RID: 1055
		// (get) Token: 0x06001441 RID: 5185 RVA: 0x0004B47B File Offset: 0x0004967B
		internal bool IsSyndicationMapping
		{
			get
			{
				return this.isSyndicationMapping;
			}
		}

		// Token: 0x06001442 RID: 5186 RVA: 0x0004B483 File Offset: 0x00049683
		internal void SetPropertyValuePath(EpmSourcePathSegment[] path)
		{
			this.propertyValuePath = path;
		}

		// Token: 0x06001443 RID: 5187 RVA: 0x0004B48C File Offset: 0x0004968C
		internal bool DefiningTypesAreEqual(EntityPropertyMappingInfo other)
		{
			return this.DefiningType.IsEquivalentTo(other.DefiningType);
		}

		// Token: 0x04000728 RID: 1832
		private readonly EntityPropertyMappingAttribute attribute;

		// Token: 0x04000729 RID: 1833
		private readonly IEdmEntityType definingType;

		// Token: 0x0400072A RID: 1834
		private readonly IEdmEntityType actualPropertyType;

		// Token: 0x0400072B RID: 1835
		private EpmSourcePathSegment[] propertyValuePath;

		// Token: 0x0400072C RID: 1836
		private bool isSyndicationMapping;
	}
}
