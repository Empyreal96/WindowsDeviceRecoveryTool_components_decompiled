using System;
using Microsoft.Data.Edm;

namespace Microsoft.Data.OData.Query.SemanticAst
{
	// Token: 0x02000049 RID: 73
	public sealed class NavigationPropertyLinkSegment : ODataPathSegment
	{
		// Token: 0x060001E3 RID: 483 RVA: 0x0000792C File Offset: 0x00005B2C
		public NavigationPropertyLinkSegment(IEdmNavigationProperty navigationProperty, IEdmEntitySet entitySet)
		{
			ExceptionUtils.CheckArgumentNotNull<IEdmNavigationProperty>(navigationProperty, "navigationProperty");
			this.navigationProperty = navigationProperty;
			base.TargetEdmEntitySet = entitySet;
			base.Identifier = navigationProperty.Name;
			base.TargetEdmType = navigationProperty.Type.Definition;
			base.SingleResult = !navigationProperty.Type.IsCollection();
			base.TargetKind = RequestTargetKind.Resource;
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060001E4 RID: 484 RVA: 0x00007990 File Offset: 0x00005B90
		public IEdmNavigationProperty NavigationProperty
		{
			get
			{
				return this.navigationProperty;
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060001E5 RID: 485 RVA: 0x00007998 File Offset: 0x00005B98
		public IEdmEntitySet EntitySet
		{
			get
			{
				return base.TargetEdmEntitySet;
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060001E6 RID: 486 RVA: 0x000079A0 File Offset: 0x00005BA0
		public override IEdmType EdmType
		{
			get
			{
				return this.navigationProperty.Type.Definition;
			}
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x000079B2 File Offset: 0x00005BB2
		public override T Translate<T>(PathSegmentTranslator<T> translator)
		{
			ExceptionUtils.CheckArgumentNotNull<PathSegmentTranslator<T>>(translator, "translator");
			return translator.Translate(this);
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x000079C6 File Offset: 0x00005BC6
		public override void Handle(PathSegmentHandler handler)
		{
			ExceptionUtils.CheckArgumentNotNull<PathSegmentHandler>(handler, "translator");
			handler.Handle(this);
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x000079DC File Offset: 0x00005BDC
		internal override bool Equals(ODataPathSegment other)
		{
			ExceptionUtils.CheckArgumentNotNull<ODataPathSegment>(other, "other");
			NavigationPropertyLinkSegment navigationPropertyLinkSegment = other as NavigationPropertyLinkSegment;
			return navigationPropertyLinkSegment != null && navigationPropertyLinkSegment.NavigationProperty == this.navigationProperty;
		}

		// Token: 0x04000080 RID: 128
		private readonly IEdmNavigationProperty navigationProperty;
	}
}
