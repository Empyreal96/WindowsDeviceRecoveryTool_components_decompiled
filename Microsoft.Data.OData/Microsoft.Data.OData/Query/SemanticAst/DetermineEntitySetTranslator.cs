using System;
using Microsoft.Data.Edm;

namespace Microsoft.Data.OData.Query.SemanticAst
{
	// Token: 0x02000020 RID: 32
	internal sealed class DetermineEntitySetTranslator : PathSegmentTranslator<IEdmEntitySet>
	{
		// Token: 0x060000C9 RID: 201 RVA: 0x00004080 File Offset: 0x00002280
		public override IEdmEntitySet Translate(NavigationPropertyLinkSegment segment)
		{
			ExceptionUtils.CheckArgumentNotNull<NavigationPropertyLinkSegment>(segment, "segment");
			return segment.EntitySet;
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00004093 File Offset: 0x00002293
		public override IEdmEntitySet Translate(TypeSegment segment)
		{
			ExceptionUtils.CheckArgumentNotNull<TypeSegment>(segment, "segment");
			return segment.EntitySet;
		}

		// Token: 0x060000CB RID: 203 RVA: 0x000040A6 File Offset: 0x000022A6
		public override IEdmEntitySet Translate(NavigationPropertySegment segment)
		{
			ExceptionUtils.CheckArgumentNotNull<NavigationPropertySegment>(segment, "segment");
			return segment.EntitySet;
		}

		// Token: 0x060000CC RID: 204 RVA: 0x000040B9 File Offset: 0x000022B9
		public override IEdmEntitySet Translate(EntitySetSegment segment)
		{
			ExceptionUtils.CheckArgumentNotNull<EntitySetSegment>(segment, "segment");
			return segment.EntitySet;
		}

		// Token: 0x060000CD RID: 205 RVA: 0x000040CC File Offset: 0x000022CC
		public override IEdmEntitySet Translate(KeySegment segment)
		{
			ExceptionUtils.CheckArgumentNotNull<KeySegment>(segment, "segment");
			return segment.EntitySet;
		}

		// Token: 0x060000CE RID: 206 RVA: 0x000040DF File Offset: 0x000022DF
		public override IEdmEntitySet Translate(PropertySegment segment)
		{
			ExceptionUtils.CheckArgumentNotNull<PropertySegment>(segment, "segment");
			return null;
		}

		// Token: 0x060000CF RID: 207 RVA: 0x000040ED File Offset: 0x000022ED
		public override IEdmEntitySet Translate(OperationSegment segment)
		{
			ExceptionUtils.CheckArgumentNotNull<OperationSegment>(segment, "segment");
			return segment.EntitySet;
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00004100 File Offset: 0x00002300
		public override IEdmEntitySet Translate(CountSegment segment)
		{
			ExceptionUtils.CheckArgumentNotNull<CountSegment>(segment, "segment");
			return null;
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x0000410E File Offset: 0x0000230E
		public override IEdmEntitySet Translate(OpenPropertySegment segment)
		{
			ExceptionUtils.CheckArgumentNotNull<OpenPropertySegment>(segment, "segment");
			return null;
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x0000411C File Offset: 0x0000231C
		public override IEdmEntitySet Translate(ValueSegment segment)
		{
			ExceptionUtils.CheckArgumentNotNull<ValueSegment>(segment, "segment");
			return null;
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x0000412A File Offset: 0x0000232A
		public override IEdmEntitySet Translate(BatchSegment segment)
		{
			ExceptionUtils.CheckArgumentNotNull<BatchSegment>(segment, "segment");
			return null;
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00004138 File Offset: 0x00002338
		public override IEdmEntitySet Translate(BatchReferenceSegment segment)
		{
			ExceptionUtils.CheckArgumentNotNull<BatchReferenceSegment>(segment, "segment");
			return segment.EntitySet;
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x0000414B File Offset: 0x0000234B
		public override IEdmEntitySet Translate(MetadataSegment segment)
		{
			ExceptionUtils.CheckArgumentNotNull<MetadataSegment>(segment, "segment");
			return null;
		}
	}
}
