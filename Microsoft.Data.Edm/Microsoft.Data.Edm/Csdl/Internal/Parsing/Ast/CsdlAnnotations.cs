using System;
using System.Collections.Generic;

namespace Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast
{
	// Token: 0x0200001A RID: 26
	internal class CsdlAnnotations
	{
		// Token: 0x0600006F RID: 111 RVA: 0x00002BD6 File Offset: 0x00000DD6
		public CsdlAnnotations(IEnumerable<CsdlVocabularyAnnotationBase> annotations, string target, string qualifier)
		{
			this.annotations = new List<CsdlVocabularyAnnotationBase>(annotations);
			this.target = target;
			this.qualifier = qualifier;
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000070 RID: 112 RVA: 0x00002BF8 File Offset: 0x00000DF8
		public IEnumerable<CsdlVocabularyAnnotationBase> Annotations
		{
			get
			{
				return this.annotations;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000071 RID: 113 RVA: 0x00002C00 File Offset: 0x00000E00
		public string Qualifier
		{
			get
			{
				return this.qualifier;
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000072 RID: 114 RVA: 0x00002C08 File Offset: 0x00000E08
		public string Target
		{
			get
			{
				return this.target;
			}
		}

		// Token: 0x04000028 RID: 40
		private readonly List<CsdlVocabularyAnnotationBase> annotations;

		// Token: 0x04000029 RID: 41
		private readonly string target;

		// Token: 0x0400002A RID: 42
		private readonly string qualifier;
	}
}
