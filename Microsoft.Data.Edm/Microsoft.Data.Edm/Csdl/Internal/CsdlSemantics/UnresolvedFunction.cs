using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Edm.Library.Internal;
using Microsoft.Data.Edm.Validation;

namespace Microsoft.Data.Edm.Csdl.Internal.CsdlSemantics
{
	// Token: 0x02000188 RID: 392
	internal class UnresolvedFunction : BadElement, IEdmFunction, IEdmFunctionBase, IEdmSchemaElement, IEdmNamedElement, IEdmVocabularyAnnotatable, IEdmElement, IUnresolvedElement
	{
		// Token: 0x060008A2 RID: 2210 RVA: 0x00018094 File Offset: 0x00016294
		public UnresolvedFunction(string qualifiedName, string errorMessage, EdmLocation location) : base(new EdmError[]
		{
			new EdmError(location, EdmErrorCode.BadUnresolvedFunction, errorMessage)
		})
		{
			qualifiedName = (qualifiedName ?? string.Empty);
			EdmUtil.TryGetNamespaceNameFromQualifiedName(qualifiedName, out this.namespaceName, out this.name);
			this.returnType = new BadTypeReference(new BadType(base.Errors), true);
		}

		// Token: 0x17000397 RID: 919
		// (get) Token: 0x060008A3 RID: 2211 RVA: 0x000180F4 File Offset: 0x000162F4
		public EdmSchemaElementKind SchemaElementKind
		{
			get
			{
				return EdmSchemaElementKind.Function;
			}
		}

		// Token: 0x17000398 RID: 920
		// (get) Token: 0x060008A4 RID: 2212 RVA: 0x000180F7 File Offset: 0x000162F7
		public string Namespace
		{
			get
			{
				return this.namespaceName;
			}
		}

		// Token: 0x17000399 RID: 921
		// (get) Token: 0x060008A5 RID: 2213 RVA: 0x000180FF File Offset: 0x000162FF
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x1700039A RID: 922
		// (get) Token: 0x060008A6 RID: 2214 RVA: 0x00018107 File Offset: 0x00016307
		public string DefiningExpression
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700039B RID: 923
		// (get) Token: 0x060008A7 RID: 2215 RVA: 0x0001810A File Offset: 0x0001630A
		public IEdmTypeReference ReturnType
		{
			get
			{
				return this.returnType;
			}
		}

		// Token: 0x1700039C RID: 924
		// (get) Token: 0x060008A8 RID: 2216 RVA: 0x00018112 File Offset: 0x00016312
		public IEnumerable<IEdmFunctionParameter> Parameters
		{
			get
			{
				return Enumerable.Empty<IEdmFunctionParameter>();
			}
		}

		// Token: 0x060008A9 RID: 2217 RVA: 0x00018119 File Offset: 0x00016319
		public IEdmFunctionParameter FindParameter(string name)
		{
			return null;
		}

		// Token: 0x04000446 RID: 1094
		private readonly string namespaceName;

		// Token: 0x04000447 RID: 1095
		private readonly string name;

		// Token: 0x04000448 RID: 1096
		private readonly IEdmTypeReference returnType;
	}
}
