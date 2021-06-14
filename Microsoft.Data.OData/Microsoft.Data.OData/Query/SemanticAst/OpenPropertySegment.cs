using System;
using Microsoft.Data.Edm;

namespace Microsoft.Data.OData.Query.SemanticAst
{
	// Token: 0x02000072 RID: 114
	public sealed class OpenPropertySegment : ODataPathSegment
	{
		// Token: 0x060002B3 RID: 691 RVA: 0x0000A704 File Offset: 0x00008904
		public OpenPropertySegment(string propertyName)
		{
			this.propertyName = propertyName;
			base.Identifier = propertyName;
			base.TargetEdmType = null;
			base.TargetKind = RequestTargetKind.OpenProperty;
			base.SingleResult = true;
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x060002B4 RID: 692 RVA: 0x0000A730 File Offset: 0x00008930
		public string PropertyName
		{
			get
			{
				return this.propertyName;
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x060002B5 RID: 693 RVA: 0x0000A738 File Offset: 0x00008938
		public override IEdmType EdmType
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x0000A73B File Offset: 0x0000893B
		public override T Translate<T>(PathSegmentTranslator<T> translator)
		{
			ExceptionUtils.CheckArgumentNotNull<PathSegmentTranslator<T>>(translator, "translator");
			return translator.Translate(this);
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x0000A74F File Offset: 0x0000894F
		public override void Handle(PathSegmentHandler handler)
		{
			ExceptionUtils.CheckArgumentNotNull<PathSegmentHandler>(handler, "translator");
			handler.Handle(this);
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x0000A764 File Offset: 0x00008964
		internal override bool Equals(ODataPathSegment other)
		{
			ExceptionUtils.CheckArgumentNotNull<ODataPathSegment>(other, "other");
			OpenPropertySegment openPropertySegment = other as OpenPropertySegment;
			return openPropertySegment != null && openPropertySegment.PropertyName == this.PropertyName;
		}

		// Token: 0x040000BB RID: 187
		private readonly string propertyName;
	}
}
