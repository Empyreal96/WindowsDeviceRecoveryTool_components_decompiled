using System;

namespace Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast
{
	// Token: 0x02000019 RID: 25
	internal abstract class CsdlVocabularyAnnotationBase : CsdlElement
	{
		// Token: 0x0600006C RID: 108 RVA: 0x00002BAF File Offset: 0x00000DAF
		protected CsdlVocabularyAnnotationBase(string term, string qualifier, CsdlLocation location) : base(location)
		{
			this.qualifier = qualifier;
			this.term = term;
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600006D RID: 109 RVA: 0x00002BC6 File Offset: 0x00000DC6
		public string Qualifier
		{
			get
			{
				return this.qualifier;
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600006E RID: 110 RVA: 0x00002BCE File Offset: 0x00000DCE
		public string Term
		{
			get
			{
				return this.term;
			}
		}

		// Token: 0x04000026 RID: 38
		private readonly string qualifier;

		// Token: 0x04000027 RID: 39
		private readonly string term;
	}
}
