using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Edm.Expressions;

namespace Microsoft.Data.Edm.Library.Internal
{
	// Token: 0x020000E0 RID: 224
	internal class AmbiguousFunctionImportBinding : AmbiguousBinding<IEdmFunctionImport>, IEdmFunctionImport, IEdmFunctionBase, IEdmEntityContainerElement, IEdmNamedElement, IEdmVocabularyAnnotatable, IEdmElement
	{
		// Token: 0x0600047A RID: 1146 RVA: 0x0000C021 File Offset: 0x0000A221
		public AmbiguousFunctionImportBinding(IEdmFunctionImport first, IEdmFunctionImport second) : base(first, second)
		{
		}

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x0600047B RID: 1147 RVA: 0x0000C02B File Offset: 0x0000A22B
		public IEdmTypeReference ReturnType
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x0600047C RID: 1148 RVA: 0x0000C030 File Offset: 0x0000A230
		public IEdmEntityContainer Container
		{
			get
			{
				IEdmFunctionImport edmFunctionImport = base.Bindings.FirstOrDefault<IEdmFunctionImport>();
				if (edmFunctionImport == null)
				{
					return null;
				}
				return edmFunctionImport.Container;
			}
		}

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x0600047D RID: 1149 RVA: 0x0000C054 File Offset: 0x0000A254
		public IEnumerable<IEdmFunctionParameter> Parameters
		{
			get
			{
				IEdmFunctionImport edmFunctionImport = base.Bindings.FirstOrDefault<IEdmFunctionImport>();
				if (edmFunctionImport == null)
				{
					return Enumerable.Empty<IEdmFunctionParameter>();
				}
				return edmFunctionImport.Parameters;
			}
		}

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x0600047E RID: 1150 RVA: 0x0000C07C File Offset: 0x0000A27C
		public EdmContainerElementKind ContainerElementKind
		{
			get
			{
				return EdmContainerElementKind.FunctionImport;
			}
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x0600047F RID: 1151 RVA: 0x0000C07F File Offset: 0x0000A27F
		public IEdmExpression EntitySet
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x06000480 RID: 1152 RVA: 0x0000C082 File Offset: 0x0000A282
		public bool IsSideEffecting
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x06000481 RID: 1153 RVA: 0x0000C085 File Offset: 0x0000A285
		public bool IsComposable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x06000482 RID: 1154 RVA: 0x0000C088 File Offset: 0x0000A288
		public bool IsBindable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x0000C08C File Offset: 0x0000A28C
		public IEdmFunctionParameter FindParameter(string name)
		{
			IEdmFunctionImport edmFunctionImport = base.Bindings.FirstOrDefault<IEdmFunctionImport>();
			if (edmFunctionImport == null)
			{
				return null;
			}
			return edmFunctionImport.FindParameter(name);
		}
	}
}
