using System;

namespace Microsoft.Data.Edm.Library
{
	// Token: 0x020001D2 RID: 466
	public class EdmFunction : EdmFunctionBase, IEdmFunction, IEdmFunctionBase, IEdmSchemaElement, IEdmNamedElement, IEdmVocabularyAnnotatable, IEdmElement
	{
		// Token: 0x06000B10 RID: 2832 RVA: 0x000205BE File Offset: 0x0001E7BE
		public EdmFunction(string namespaceName, string name, IEdmTypeReference returnType) : this(namespaceName, name, returnType, null)
		{
		}

		// Token: 0x06000B11 RID: 2833 RVA: 0x000205CA File Offset: 0x0001E7CA
		public EdmFunction(string namespaceName, string name, IEdmTypeReference returnType, string definingExpression) : base(name, returnType)
		{
			EdmUtil.CheckArgumentNull<string>(namespaceName, "namespaceName");
			EdmUtil.CheckArgumentNull<IEdmTypeReference>(returnType, "returnType");
			this.namespaceName = namespaceName;
			this.definingExpression = definingExpression;
		}

		// Token: 0x17000435 RID: 1077
		// (get) Token: 0x06000B12 RID: 2834 RVA: 0x000205FB File Offset: 0x0001E7FB
		public string DefiningExpression
		{
			get
			{
				return this.definingExpression;
			}
		}

		// Token: 0x17000436 RID: 1078
		// (get) Token: 0x06000B13 RID: 2835 RVA: 0x00020603 File Offset: 0x0001E803
		public EdmSchemaElementKind SchemaElementKind
		{
			get
			{
				return EdmSchemaElementKind.Function;
			}
		}

		// Token: 0x17000437 RID: 1079
		// (get) Token: 0x06000B14 RID: 2836 RVA: 0x00020606 File Offset: 0x0001E806
		public string Namespace
		{
			get
			{
				return this.namespaceName;
			}
		}

		// Token: 0x04000532 RID: 1330
		private readonly string namespaceName;

		// Token: 0x04000533 RID: 1331
		private readonly string definingExpression;
	}
}
