using System;
using Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast;
using Microsoft.Data.Edm.Expressions;
using Microsoft.Data.Edm.Internal;
using Microsoft.Data.Edm.Library.Expressions;

namespace Microsoft.Data.Edm.Csdl.Internal.CsdlSemantics
{
	// Token: 0x020000B7 RID: 183
	internal class CsdlSemanticsFunctionImport : CsdlSemanticsFunctionBase, IEdmFunctionImport, IEdmFunctionBase, IEdmEntityContainerElement, IEdmNamedElement, IEdmVocabularyAnnotatable, IEdmElement
	{
		// Token: 0x06000346 RID: 838 RVA: 0x00008901 File Offset: 0x00006B01
		public CsdlSemanticsFunctionImport(CsdlSemanticsEntityContainer container, CsdlFunctionImport functionImport) : base(container.Context, functionImport)
		{
			this.container = container;
			this.functionImport = functionImport;
		}

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x06000347 RID: 839 RVA: 0x00008929 File Offset: 0x00006B29
		public bool IsSideEffecting
		{
			get
			{
				return this.functionImport.SideEffecting;
			}
		}

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x06000348 RID: 840 RVA: 0x00008936 File Offset: 0x00006B36
		public bool IsComposable
		{
			get
			{
				return this.functionImport.Composable;
			}
		}

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x06000349 RID: 841 RVA: 0x00008943 File Offset: 0x00006B43
		public bool IsBindable
		{
			get
			{
				return this.functionImport.Bindable;
			}
		}

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x0600034A RID: 842 RVA: 0x00008950 File Offset: 0x00006B50
		public IEdmEntityContainer Container
		{
			get
			{
				return this.container;
			}
		}

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x0600034B RID: 843 RVA: 0x00008958 File Offset: 0x00006B58
		public IEdmExpression EntitySet
		{
			get
			{
				return this.entitySetCache.GetValue(this, CsdlSemanticsFunctionImport.ComputeEntitySetFunc, null);
			}
		}

		// Token: 0x0600034C RID: 844 RVA: 0x0000896C File Offset: 0x00006B6C
		private IEdmExpression ComputeEntitySet()
		{
			if (this.functionImport.EntitySet != null)
			{
				IEdmEntitySet referencedEntitySet = this.container.FindEntitySet(this.functionImport.EntitySet) ?? new UnresolvedEntitySet(this.functionImport.EntitySet, this.Container, base.Location);
				return new CsdlSemanticsFunctionImport.FunctionImportEntitySetReferenceExpression(referencedEntitySet)
				{
					Location = base.Location
				};
			}
			if (this.functionImport.EntitySetPath != null)
			{
				return new CsdlSemanticsFunctionImport.FunctionImportPathExpression(this.functionImport.EntitySetPath)
				{
					Location = base.Location
				};
			}
			return null;
		}

		// Token: 0x04000171 RID: 369
		private readonly CsdlFunctionImport functionImport;

		// Token: 0x04000172 RID: 370
		private readonly CsdlSemanticsEntityContainer container;

		// Token: 0x04000173 RID: 371
		private readonly Cache<CsdlSemanticsFunctionImport, IEdmExpression> entitySetCache = new Cache<CsdlSemanticsFunctionImport, IEdmExpression>();

		// Token: 0x04000174 RID: 372
		private static readonly Func<CsdlSemanticsFunctionImport, IEdmExpression> ComputeEntitySetFunc = (CsdlSemanticsFunctionImport me) => me.ComputeEntitySet();

		// Token: 0x020000B9 RID: 185
		private sealed class FunctionImportEntitySetReferenceExpression : EdmEntitySetReferenceExpression, IEdmLocatable
		{
			// Token: 0x06000352 RID: 850 RVA: 0x00008A52 File Offset: 0x00006C52
			internal FunctionImportEntitySetReferenceExpression(IEdmEntitySet referencedEntitySet) : base(referencedEntitySet)
			{
			}

			// Token: 0x170001A2 RID: 418
			// (get) Token: 0x06000353 RID: 851 RVA: 0x00008A5B File Offset: 0x00006C5B
			// (set) Token: 0x06000354 RID: 852 RVA: 0x00008A63 File Offset: 0x00006C63
			public EdmLocation Location { get; set; }
		}

		// Token: 0x020000BB RID: 187
		private sealed class FunctionImportPathExpression : EdmPathExpression, IEdmLocatable
		{
			// Token: 0x0600035A RID: 858 RVA: 0x00008B2C File Offset: 0x00006D2C
			internal FunctionImportPathExpression(string path) : base(path)
			{
			}

			// Token: 0x170001A5 RID: 421
			// (get) Token: 0x0600035B RID: 859 RVA: 0x00008B35 File Offset: 0x00006D35
			// (set) Token: 0x0600035C RID: 860 RVA: 0x00008B3D File Offset: 0x00006D3D
			public EdmLocation Location { get; set; }
		}
	}
}
