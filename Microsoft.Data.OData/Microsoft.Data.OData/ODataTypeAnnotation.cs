using System;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.Metadata;

namespace Microsoft.Data.OData
{
	// Token: 0x0200017A RID: 378
	internal sealed class ODataTypeAnnotation
	{
		// Token: 0x06000AAA RID: 2730 RVA: 0x000237E5 File Offset: 0x000219E5
		public ODataTypeAnnotation(IEdmEntitySet entitySet, IEdmEntityType entityType)
		{
			ExceptionUtils.CheckArgumentNotNull<IEdmEntitySet>(entitySet, "entitySet");
			ExceptionUtils.CheckArgumentNotNull<IEdmEntityType>(entityType, "entityType");
			this.entitySet = entitySet;
			this.type = entityType.ToTypeReference(true);
		}

		// Token: 0x06000AAB RID: 2731 RVA: 0x00023817 File Offset: 0x00021A17
		public ODataTypeAnnotation(IEdmComplexTypeReference complexType)
		{
			ExceptionUtils.CheckArgumentNotNull<IEdmComplexTypeReference>(complexType, "complexType");
			this.type = complexType;
		}

		// Token: 0x06000AAC RID: 2732 RVA: 0x00023831 File Offset: 0x00021A31
		public ODataTypeAnnotation(IEdmCollectionTypeReference collectionType)
		{
			ExceptionUtils.CheckArgumentNotNull<IEdmCollectionTypeReference>(collectionType, "collectionType");
			this.type = collectionType;
		}

		// Token: 0x17000280 RID: 640
		// (get) Token: 0x06000AAD RID: 2733 RVA: 0x0002384B File Offset: 0x00021A4B
		public IEdmTypeReference Type
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x17000281 RID: 641
		// (get) Token: 0x06000AAE RID: 2734 RVA: 0x00023853 File Offset: 0x00021A53
		public IEdmEntitySet EntitySet
		{
			get
			{
				return this.entitySet;
			}
		}

		// Token: 0x040003F7 RID: 1015
		private readonly IEdmTypeReference type;

		// Token: 0x040003F8 RID: 1016
		private readonly IEdmEntitySet entitySet;
	}
}
