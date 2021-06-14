using System;
using Microsoft.Data.Edm;

namespace Microsoft.Data.OData.Query.SemanticAst
{
	// Token: 0x02000065 RID: 101
	public sealed class BatchSegment : ODataPathSegment
	{
		// Token: 0x06000278 RID: 632 RVA: 0x0000A0DA File Offset: 0x000082DA
		private BatchSegment()
		{
			base.Identifier = "$batch";
			base.TargetKind = RequestTargetKind.Batch;
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000279 RID: 633 RVA: 0x0000A0F4 File Offset: 0x000082F4
		public override IEdmType EdmType
		{
			get
			{
				return null;
			}
		}

		// Token: 0x0600027A RID: 634 RVA: 0x0000A0F7 File Offset: 0x000082F7
		public override T Translate<T>(PathSegmentTranslator<T> translator)
		{
			ExceptionUtils.CheckArgumentNotNull<PathSegmentTranslator<T>>(translator, "translator");
			return translator.Translate(this);
		}

		// Token: 0x0600027B RID: 635 RVA: 0x0000A10B File Offset: 0x0000830B
		public override void Handle(PathSegmentHandler handler)
		{
			ExceptionUtils.CheckArgumentNotNull<PathSegmentHandler>(handler, "translator");
			handler.Handle(this);
		}

		// Token: 0x0600027C RID: 636 RVA: 0x0000A11F File Offset: 0x0000831F
		internal override bool Equals(ODataPathSegment other)
		{
			ExceptionUtils.CheckArgumentNotNull<ODataPathSegment>(other, "other");
			return other is BatchSegment;
		}

		// Token: 0x040000A7 RID: 167
		public static readonly BatchSegment Instance = new BatchSegment();
	}
}
