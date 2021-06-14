using System;

namespace Microsoft.Data.Edm.Library
{
	// Token: 0x02000106 RID: 262
	public class EdmSpatialTypeReference : EdmPrimitiveTypeReference, IEdmSpatialTypeReference, IEdmPrimitiveTypeReference, IEdmTypeReference, IEdmElement
	{
		// Token: 0x06000509 RID: 1289 RVA: 0x0000C93C File Offset: 0x0000AB3C
		public EdmSpatialTypeReference(IEdmPrimitiveType definition, bool isNullable) : this(definition, isNullable, null)
		{
			EdmUtil.CheckArgumentNull<IEdmPrimitiveType>(definition, "definition");
			switch (definition.PrimitiveKind)
			{
			case EdmPrimitiveTypeKind.Geography:
			case EdmPrimitiveTypeKind.GeographyPoint:
			case EdmPrimitiveTypeKind.GeographyLineString:
			case EdmPrimitiveTypeKind.GeographyPolygon:
			case EdmPrimitiveTypeKind.GeographyCollection:
			case EdmPrimitiveTypeKind.GeographyMultiPolygon:
			case EdmPrimitiveTypeKind.GeographyMultiLineString:
			case EdmPrimitiveTypeKind.GeographyMultiPoint:
				this.spatialReferenceIdentifier = new int?(4326);
				return;
			default:
				this.spatialReferenceIdentifier = new int?(0);
				return;
			}
		}

		// Token: 0x0600050A RID: 1290 RVA: 0x0000C9B5 File Offset: 0x0000ABB5
		public EdmSpatialTypeReference(IEdmPrimitiveType definition, bool isNullable, int? spatialReferenceIdentifier) : base(definition, isNullable)
		{
			this.spatialReferenceIdentifier = spatialReferenceIdentifier;
		}

		// Token: 0x1700022F RID: 559
		// (get) Token: 0x0600050B RID: 1291 RVA: 0x0000C9C6 File Offset: 0x0000ABC6
		public int? SpatialReferenceIdentifier
		{
			get
			{
				return this.spatialReferenceIdentifier;
			}
		}

		// Token: 0x040001DD RID: 477
		private readonly int? spatialReferenceIdentifier;
	}
}
