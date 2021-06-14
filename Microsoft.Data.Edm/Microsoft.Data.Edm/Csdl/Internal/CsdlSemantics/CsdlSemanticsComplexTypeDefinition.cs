using System;
using Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast;
using Microsoft.Data.Edm.Internal;
using Microsoft.Data.Edm.Library.Internal;

namespace Microsoft.Data.Edm.Csdl.Internal.CsdlSemantics
{
	// Token: 0x02000168 RID: 360
	internal class CsdlSemanticsComplexTypeDefinition : CsdlSemanticsStructuredTypeDefinition, IEdmComplexType, IEdmStructuredType, IEdmSchemaType, IEdmType, IEdmTerm, IEdmSchemaElement, IEdmNamedElement, IEdmVocabularyAnnotatable, IEdmElement
	{
		// Token: 0x06000795 RID: 1941 RVA: 0x00014A89 File Offset: 0x00012C89
		public CsdlSemanticsComplexTypeDefinition(CsdlSemanticsSchema context, CsdlComplexType complex) : base(context, complex)
		{
			this.complex = complex;
		}

		// Token: 0x17000309 RID: 777
		// (get) Token: 0x06000796 RID: 1942 RVA: 0x00014AA5 File Offset: 0x00012CA5
		public override IEdmStructuredType BaseType
		{
			get
			{
				return this.baseTypeCache.GetValue(this, CsdlSemanticsComplexTypeDefinition.ComputeBaseTypeFunc, CsdlSemanticsComplexTypeDefinition.OnCycleBaseTypeFunc);
			}
		}

		// Token: 0x1700030A RID: 778
		// (get) Token: 0x06000797 RID: 1943 RVA: 0x00014ABD File Offset: 0x00012CBD
		public override EdmTypeKind TypeKind
		{
			get
			{
				return EdmTypeKind.Complex;
			}
		}

		// Token: 0x1700030B RID: 779
		// (get) Token: 0x06000798 RID: 1944 RVA: 0x00014AC0 File Offset: 0x00012CC0
		public EdmTermKind TermKind
		{
			get
			{
				return EdmTermKind.Type;
			}
		}

		// Token: 0x1700030C RID: 780
		// (get) Token: 0x06000799 RID: 1945 RVA: 0x00014AC3 File Offset: 0x00012CC3
		public override bool IsAbstract
		{
			get
			{
				return this.complex.IsAbstract;
			}
		}

		// Token: 0x1700030D RID: 781
		// (get) Token: 0x0600079A RID: 1946 RVA: 0x00014AD0 File Offset: 0x00012CD0
		public string Name
		{
			get
			{
				return this.complex.Name;
			}
		}

		// Token: 0x1700030E RID: 782
		// (get) Token: 0x0600079B RID: 1947 RVA: 0x00014ADD File Offset: 0x00012CDD
		protected override CsdlStructuredType MyStructured
		{
			get
			{
				return this.complex;
			}
		}

		// Token: 0x0600079C RID: 1948 RVA: 0x00014AE8 File Offset: 0x00012CE8
		private IEdmComplexType ComputeBaseType()
		{
			if (this.complex.BaseTypeName != null)
			{
				IEdmComplexType edmComplexType = base.Context.FindType(this.complex.BaseTypeName) as IEdmComplexType;
				if (edmComplexType != null)
				{
					IEdmStructuredType baseType = edmComplexType.BaseType;
				}
				return edmComplexType ?? new UnresolvedComplexType(base.Context.UnresolvedName(this.complex.BaseTypeName), base.Location);
			}
			return null;
		}

		// Token: 0x040003CE RID: 974
		private readonly CsdlComplexType complex;

		// Token: 0x040003CF RID: 975
		private readonly Cache<CsdlSemanticsComplexTypeDefinition, IEdmComplexType> baseTypeCache = new Cache<CsdlSemanticsComplexTypeDefinition, IEdmComplexType>();

		// Token: 0x040003D0 RID: 976
		private static readonly Func<CsdlSemanticsComplexTypeDefinition, IEdmComplexType> ComputeBaseTypeFunc = (CsdlSemanticsComplexTypeDefinition me) => me.ComputeBaseType();

		// Token: 0x040003D1 RID: 977
		private static readonly Func<CsdlSemanticsComplexTypeDefinition, IEdmComplexType> OnCycleBaseTypeFunc = (CsdlSemanticsComplexTypeDefinition me) => new CyclicComplexType(me.GetCyclicBaseTypeName(me.complex.BaseTypeName), me.Location);
	}
}
