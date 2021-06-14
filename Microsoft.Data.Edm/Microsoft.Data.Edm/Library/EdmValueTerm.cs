using System;

namespace Microsoft.Data.Edm.Library
{
	// Token: 0x020001A6 RID: 422
	public class EdmValueTerm : EdmNamedElement, IEdmValueTerm, IEdmTerm, IEdmSchemaElement, IEdmNamedElement, IEdmVocabularyAnnotatable, IEdmElement
	{
		// Token: 0x06000923 RID: 2339 RVA: 0x0001884D File Offset: 0x00016A4D
		public EdmValueTerm(string namespaceName, string name, EdmPrimitiveTypeKind type) : this(namespaceName, name, EdmCoreModel.Instance.GetPrimitive(type, true))
		{
		}

		// Token: 0x06000924 RID: 2340 RVA: 0x00018863 File Offset: 0x00016A63
		public EdmValueTerm(string namespaceName, string name, IEdmTypeReference type) : base(name)
		{
			EdmUtil.CheckArgumentNull<string>(namespaceName, "namespaceName");
			EdmUtil.CheckArgumentNull<IEdmTypeReference>(type, "type");
			this.namespaceName = namespaceName;
			this.type = type;
		}

		// Token: 0x170003E2 RID: 994
		// (get) Token: 0x06000925 RID: 2341 RVA: 0x00018892 File Offset: 0x00016A92
		public string Namespace
		{
			get
			{
				return this.namespaceName;
			}
		}

		// Token: 0x170003E3 RID: 995
		// (get) Token: 0x06000926 RID: 2342 RVA: 0x0001889A File Offset: 0x00016A9A
		public EdmTermKind TermKind
		{
			get
			{
				return EdmTermKind.Value;
			}
		}

		// Token: 0x170003E4 RID: 996
		// (get) Token: 0x06000927 RID: 2343 RVA: 0x0001889D File Offset: 0x00016A9D
		public IEdmTypeReference Type
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x170003E5 RID: 997
		// (get) Token: 0x06000928 RID: 2344 RVA: 0x000188A5 File Offset: 0x00016AA5
		public EdmSchemaElementKind SchemaElementKind
		{
			get
			{
				return EdmSchemaElementKind.ValueTerm;
			}
		}

		// Token: 0x04000474 RID: 1140
		private readonly string namespaceName;

		// Token: 0x04000475 RID: 1141
		private readonly IEdmTypeReference type;
	}
}
