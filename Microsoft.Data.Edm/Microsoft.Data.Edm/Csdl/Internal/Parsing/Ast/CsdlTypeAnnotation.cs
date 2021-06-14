using System;
using System.Collections.Generic;

namespace Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast
{
	// Token: 0x02000065 RID: 101
	internal class CsdlTypeAnnotation : CsdlVocabularyAnnotationBase
	{
		// Token: 0x060001AB RID: 427 RVA: 0x000051ED File Offset: 0x000033ED
		public CsdlTypeAnnotation(string term, string qualifier, IEnumerable<CsdlPropertyValue> properties, CsdlLocation location) : base(term, qualifier, location)
		{
			this.properties = new List<CsdlPropertyValue>(properties);
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x060001AC RID: 428 RVA: 0x00005205 File Offset: 0x00003405
		public IEnumerable<CsdlPropertyValue> Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x040000B7 RID: 183
		private readonly List<CsdlPropertyValue> properties;
	}
}
