using System;
using Microsoft.Data.Edm;

namespace Microsoft.Data.OData.Query.SemanticAst
{
	// Token: 0x02000070 RID: 112
	public sealed class NavigationPropertySegment : ODataPathSegment
	{
		// Token: 0x060002A9 RID: 681 RVA: 0x0000A534 File Offset: 0x00008734
		public NavigationPropertySegment(IEdmNavigationProperty navigationProperty, IEdmEntitySet entitySet)
		{
			ExceptionUtils.CheckArgumentNotNull<IEdmNavigationProperty>(navigationProperty, "navigationProperty");
			this.navigationProperty = navigationProperty;
			base.TargetEdmEntitySet = entitySet;
			base.Identifier = navigationProperty.Name;
			base.TargetEdmType = navigationProperty.Type.Definition;
			base.SingleResult = !navigationProperty.Type.IsCollection();
			base.TargetKind = RequestTargetKind.Resource;
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x060002AA RID: 682 RVA: 0x0000A598 File Offset: 0x00008798
		public IEdmNavigationProperty NavigationProperty
		{
			get
			{
				return this.navigationProperty;
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x060002AB RID: 683 RVA: 0x0000A5A0 File Offset: 0x000087A0
		public IEdmEntitySet EntitySet
		{
			get
			{
				return base.TargetEdmEntitySet;
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x060002AC RID: 684 RVA: 0x0000A5A8 File Offset: 0x000087A8
		public override IEdmType EdmType
		{
			get
			{
				return this.navigationProperty.Type.Definition;
			}
		}

		// Token: 0x060002AD RID: 685 RVA: 0x0000A5BA File Offset: 0x000087BA
		public override T Translate<T>(PathSegmentTranslator<T> translator)
		{
			ExceptionUtils.CheckArgumentNotNull<PathSegmentTranslator<T>>(translator, "translator");
			return translator.Translate(this);
		}

		// Token: 0x060002AE RID: 686 RVA: 0x0000A5CE File Offset: 0x000087CE
		public override void Handle(PathSegmentHandler handler)
		{
			ExceptionUtils.CheckArgumentNotNull<PathSegmentHandler>(handler, "translator");
			handler.Handle(this);
		}

		// Token: 0x060002AF RID: 687 RVA: 0x0000A5E4 File Offset: 0x000087E4
		internal override bool Equals(ODataPathSegment other)
		{
			ExceptionUtils.CheckArgumentNotNull<ODataPathSegment>(other, "other");
			NavigationPropertySegment navigationPropertySegment = other as NavigationPropertySegment;
			return navigationPropertySegment != null && navigationPropertySegment.NavigationProperty == this.NavigationProperty;
		}

		// Token: 0x040000BA RID: 186
		private readonly IEdmNavigationProperty navigationProperty;
	}
}
