using System;

namespace Microsoft.Data.OData.Query.SemanticAst
{
	// Token: 0x0200001F RID: 31
	public abstract class PathSegmentTranslator<T>
	{
		// Token: 0x060000BB RID: 187 RVA: 0x0000401D File Offset: 0x0000221D
		public virtual T Translate(TypeSegment segment)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00004024 File Offset: 0x00002224
		public virtual T Translate(NavigationPropertySegment segment)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060000BD RID: 189 RVA: 0x0000402B File Offset: 0x0000222B
		public virtual T Translate(EntitySetSegment segment)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060000BE RID: 190 RVA: 0x00004032 File Offset: 0x00002232
		public virtual T Translate(KeySegment segment)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00004039 File Offset: 0x00002239
		public virtual T Translate(PropertySegment segment)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00004040 File Offset: 0x00002240
		public virtual T Translate(OperationSegment segment)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00004047 File Offset: 0x00002247
		public virtual T Translate(OpenPropertySegment segment)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x0000404E File Offset: 0x0000224E
		public virtual T Translate(CountSegment segment)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00004055 File Offset: 0x00002255
		public virtual T Translate(NavigationPropertyLinkSegment segment)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x0000405C File Offset: 0x0000225C
		public virtual T Translate(ValueSegment segment)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00004063 File Offset: 0x00002263
		public virtual T Translate(BatchSegment segment)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x0000406A File Offset: 0x0000226A
		public virtual T Translate(BatchReferenceSegment segment)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00004071 File Offset: 0x00002271
		public virtual T Translate(MetadataSegment segment)
		{
			throw new NotImplementedException();
		}
	}
}
