using System;
using Microsoft.Data.Edm;

namespace Microsoft.Data.OData.Query.SemanticAst
{
	// Token: 0x02000047 RID: 71
	internal sealed class IsCollectionTranslator : PathSegmentTranslator<bool>
	{
		// Token: 0x060001C6 RID: 454 RVA: 0x000077BC File Offset: 0x000059BC
		public override bool Translate(NavigationPropertySegment segment)
		{
			ExceptionUtils.CheckArgumentNotNull<NavigationPropertySegment>(segment, "segment");
			return segment.NavigationProperty.Type.IsCollection();
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x000077D9 File Offset: 0x000059D9
		public override bool Translate(EntitySetSegment segment)
		{
			ExceptionUtils.CheckArgumentNotNull<EntitySetSegment>(segment, "segment");
			return true;
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x000077E7 File Offset: 0x000059E7
		public override bool Translate(KeySegment segment)
		{
			ExceptionUtils.CheckArgumentNotNull<KeySegment>(segment, "segment");
			return false;
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x000077F5 File Offset: 0x000059F5
		public override bool Translate(PropertySegment segment)
		{
			ExceptionUtils.CheckArgumentNotNull<PropertySegment>(segment, "segment");
			return false;
		}

		// Token: 0x060001CA RID: 458 RVA: 0x00007803 File Offset: 0x00005A03
		public override bool Translate(OpenPropertySegment segment)
		{
			ExceptionUtils.CheckArgumentNotNull<OpenPropertySegment>(segment, "segment");
			return false;
		}

		// Token: 0x060001CB RID: 459 RVA: 0x00007811 File Offset: 0x00005A11
		public override bool Translate(CountSegment segment)
		{
			ExceptionUtils.CheckArgumentNotNull<CountSegment>(segment, "segment");
			return false;
		}

		// Token: 0x060001CC RID: 460 RVA: 0x0000781F File Offset: 0x00005A1F
		public override bool Translate(NavigationPropertyLinkSegment segment)
		{
			ExceptionUtils.CheckArgumentNotNull<NavigationPropertyLinkSegment>(segment, "segment");
			return false;
		}

		// Token: 0x060001CD RID: 461 RVA: 0x0000782D File Offset: 0x00005A2D
		public override bool Translate(BatchSegment segment)
		{
			ExceptionUtils.CheckArgumentNotNull<BatchSegment>(segment, "segment");
			return false;
		}

		// Token: 0x060001CE RID: 462 RVA: 0x0000783B File Offset: 0x00005A3B
		public override bool Translate(BatchReferenceSegment segment)
		{
			ExceptionUtils.CheckArgumentNotNull<BatchReferenceSegment>(segment, "segment");
			return false;
		}

		// Token: 0x060001CF RID: 463 RVA: 0x00007849 File Offset: 0x00005A49
		public override bool Translate(ValueSegment segment)
		{
			ExceptionUtils.CheckArgumentNotNull<ValueSegment>(segment, "segment");
			throw new NotImplementedException(segment.ToString());
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x00007861 File Offset: 0x00005A61
		public override bool Translate(MetadataSegment segment)
		{
			ExceptionUtils.CheckArgumentNotNull<MetadataSegment>(segment, "segment");
			return false;
		}
	}
}
