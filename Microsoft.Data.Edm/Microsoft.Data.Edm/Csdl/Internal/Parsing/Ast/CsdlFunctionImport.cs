using System;
using System.Collections.Generic;

namespace Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast
{
	// Token: 0x02000140 RID: 320
	internal class CsdlFunctionImport : CsdlFunctionBase
	{
		// Token: 0x06000604 RID: 1540 RVA: 0x0000F627 File Offset: 0x0000D827
		public CsdlFunctionImport(string name, bool sideEffecting, bool composable, bool bindable, string entitySet, string entitySetPath, IEnumerable<CsdlFunctionParameter> parameters, CsdlTypeReference returnType, CsdlDocumentation documentation, CsdlLocation location) : base(name, parameters, returnType, documentation, location)
		{
			this.sideEffecting = sideEffecting;
			this.composable = composable;
			this.bindable = bindable;
			this.entitySet = entitySet;
			this.entitySetPath = entitySetPath;
		}

		// Token: 0x17000283 RID: 643
		// (get) Token: 0x06000605 RID: 1541 RVA: 0x0000F65E File Offset: 0x0000D85E
		public bool SideEffecting
		{
			get
			{
				return this.sideEffecting;
			}
		}

		// Token: 0x17000284 RID: 644
		// (get) Token: 0x06000606 RID: 1542 RVA: 0x0000F666 File Offset: 0x0000D866
		public bool Composable
		{
			get
			{
				return this.composable;
			}
		}

		// Token: 0x17000285 RID: 645
		// (get) Token: 0x06000607 RID: 1543 RVA: 0x0000F66E File Offset: 0x0000D86E
		public bool Bindable
		{
			get
			{
				return this.bindable;
			}
		}

		// Token: 0x17000286 RID: 646
		// (get) Token: 0x06000608 RID: 1544 RVA: 0x0000F676 File Offset: 0x0000D876
		public string EntitySet
		{
			get
			{
				return this.entitySet;
			}
		}

		// Token: 0x17000287 RID: 647
		// (get) Token: 0x06000609 RID: 1545 RVA: 0x0000F67E File Offset: 0x0000D87E
		public string EntitySetPath
		{
			get
			{
				return this.entitySetPath;
			}
		}

		// Token: 0x04000332 RID: 818
		private readonly bool sideEffecting;

		// Token: 0x04000333 RID: 819
		private readonly bool composable;

		// Token: 0x04000334 RID: 820
		private readonly bool bindable;

		// Token: 0x04000335 RID: 821
		private readonly string entitySet;

		// Token: 0x04000336 RID: 822
		private readonly string entitySetPath;
	}
}
