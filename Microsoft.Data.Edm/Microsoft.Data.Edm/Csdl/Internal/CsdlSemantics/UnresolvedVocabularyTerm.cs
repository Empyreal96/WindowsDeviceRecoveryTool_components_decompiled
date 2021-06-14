using System;
using Microsoft.Data.Edm.Library;

namespace Microsoft.Data.Edm.Csdl.Internal.CsdlSemantics
{
	// Token: 0x02000035 RID: 53
	internal abstract class UnresolvedVocabularyTerm : EdmElement, IEdmTerm, IEdmSchemaElement, IEdmNamedElement, IEdmVocabularyAnnotatable, IEdmElement, IUnresolvedElement
	{
		// Token: 0x060000A6 RID: 166 RVA: 0x00002EB4 File Offset: 0x000010B4
		protected UnresolvedVocabularyTerm(string qualifiedName)
		{
			qualifiedName = (qualifiedName ?? string.Empty);
			EdmUtil.TryGetNamespaceNameFromQualifiedName(qualifiedName, out this.namespaceName, out this.name);
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060000A7 RID: 167 RVA: 0x00002EDB File Offset: 0x000010DB
		public string Namespace
		{
			get
			{
				return this.namespaceName;
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060000A8 RID: 168 RVA: 0x00002EE3 File Offset: 0x000010E3
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060000A9 RID: 169
		public abstract EdmTermKind TermKind { get; }

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060000AA RID: 170
		public abstract EdmSchemaElementKind SchemaElementKind { get; }

		// Token: 0x0400003B RID: 59
		private readonly string namespaceName;

		// Token: 0x0400003C RID: 60
		private readonly string name;
	}
}
