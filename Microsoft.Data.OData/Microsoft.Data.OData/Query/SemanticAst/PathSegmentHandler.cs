using System;

namespace Microsoft.Data.OData.Query.SemanticAst
{
	// Token: 0x02000044 RID: 68
	public abstract class PathSegmentHandler
	{
		// Token: 0x060001AA RID: 426 RVA: 0x00007654 File Offset: 0x00005854
		public virtual void Handle(TypeSegment segment)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060001AB RID: 427 RVA: 0x0000765B File Offset: 0x0000585B
		public virtual void Handle(NavigationPropertySegment segment)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060001AC RID: 428 RVA: 0x00007662 File Offset: 0x00005862
		public virtual void Handle(EntitySetSegment segment)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060001AD RID: 429 RVA: 0x00007669 File Offset: 0x00005869
		public virtual void Handle(KeySegment segment)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060001AE RID: 430 RVA: 0x00007670 File Offset: 0x00005870
		public virtual void Handle(PropertySegment segment)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060001AF RID: 431 RVA: 0x00007677 File Offset: 0x00005877
		public virtual void Handle(OperationSegment segment)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x0000767E File Offset: 0x0000587E
		public virtual void Handle(OpenPropertySegment segment)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x00007685 File Offset: 0x00005885
		public virtual void Handle(CountSegment segment)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x0000768C File Offset: 0x0000588C
		public virtual void Handle(NavigationPropertyLinkSegment segment)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x00007693 File Offset: 0x00005893
		public virtual void Handle(ValueSegment segment)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x0000769A File Offset: 0x0000589A
		public virtual void Handle(BatchSegment segment)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x000076A1 File Offset: 0x000058A1
		public virtual void Handle(BatchReferenceSegment segment)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x000076A8 File Offset: 0x000058A8
		public virtual void Handle(MetadataSegment segment)
		{
			throw new NotImplementedException();
		}
	}
}
