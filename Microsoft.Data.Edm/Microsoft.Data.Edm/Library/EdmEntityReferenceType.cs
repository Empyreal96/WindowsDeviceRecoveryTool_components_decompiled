using System;

namespace Microsoft.Data.Edm.Library
{
	// Token: 0x020001CE RID: 462
	public class EdmEntityReferenceType : EdmType, IEdmEntityReferenceType, IEdmType, IEdmElement
	{
		// Token: 0x06000AED RID: 2797 RVA: 0x00020124 File Offset: 0x0001E324
		public EdmEntityReferenceType(IEdmEntityType entityType)
		{
			EdmUtil.CheckArgumentNull<IEdmEntityType>(entityType, "entityType");
			this.entityType = entityType;
		}

		// Token: 0x17000427 RID: 1063
		// (get) Token: 0x06000AEE RID: 2798 RVA: 0x0002013F File Offset: 0x0001E33F
		public override EdmTypeKind TypeKind
		{
			get
			{
				return EdmTypeKind.EntityReference;
			}
		}

		// Token: 0x17000428 RID: 1064
		// (get) Token: 0x06000AEF RID: 2799 RVA: 0x00020142 File Offset: 0x0001E342
		public IEdmEntityType EntityType
		{
			get
			{
				return this.entityType;
			}
		}

		// Token: 0x04000526 RID: 1318
		private readonly IEdmEntityType entityType;
	}
}
