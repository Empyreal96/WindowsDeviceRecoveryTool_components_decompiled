using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Data.Edm.Library.Internal
{
	// Token: 0x020000DF RID: 223
	internal class AmbiguousFunctionBinding : AmbiguousBinding<IEdmFunction>, IEdmFunction, IEdmFunctionBase, IEdmSchemaElement, IEdmNamedElement, IEdmVocabularyAnnotatable, IEdmElement
	{
		// Token: 0x06000473 RID: 1139 RVA: 0x0000BF96 File Offset: 0x0000A196
		public AmbiguousFunctionBinding(IEdmFunction first, IEdmFunction second) : base(first, second)
		{
		}

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x06000474 RID: 1140 RVA: 0x0000BFA0 File Offset: 0x0000A1A0
		public IEdmTypeReference ReturnType
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x06000475 RID: 1141 RVA: 0x0000BFA3 File Offset: 0x0000A1A3
		public EdmSchemaElementKind SchemaElementKind
		{
			get
			{
				return EdmSchemaElementKind.Function;
			}
		}

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x06000476 RID: 1142 RVA: 0x0000BFA8 File Offset: 0x0000A1A8
		public string Namespace
		{
			get
			{
				IEdmFunction edmFunction = base.Bindings.FirstOrDefault<IEdmFunction>();
				if (edmFunction == null)
				{
					return string.Empty;
				}
				return edmFunction.Namespace;
			}
		}

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x06000477 RID: 1143 RVA: 0x0000BFD0 File Offset: 0x0000A1D0
		public string DefiningExpression
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x06000478 RID: 1144 RVA: 0x0000BFD4 File Offset: 0x0000A1D4
		public IEnumerable<IEdmFunctionParameter> Parameters
		{
			get
			{
				IEdmFunction edmFunction = base.Bindings.FirstOrDefault<IEdmFunction>();
				if (edmFunction == null)
				{
					return Enumerable.Empty<IEdmFunctionParameter>();
				}
				return edmFunction.Parameters;
			}
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x0000BFFC File Offset: 0x0000A1FC
		public IEdmFunctionParameter FindParameter(string name)
		{
			IEdmFunction edmFunction = base.Bindings.FirstOrDefault<IEdmFunction>();
			if (edmFunction == null)
			{
				return null;
			}
			return edmFunction.FindParameter(name);
		}
	}
}
