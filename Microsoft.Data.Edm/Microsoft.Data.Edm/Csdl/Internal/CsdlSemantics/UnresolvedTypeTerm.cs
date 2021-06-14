using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Data.Edm.Csdl.Internal.CsdlSemantics
{
	// Token: 0x02000038 RID: 56
	internal class UnresolvedTypeTerm : UnresolvedVocabularyTerm, IEdmEntityType, IEdmStructuredType, IEdmSchemaType, IEdmType, IEdmTerm, IEdmSchemaElement, IEdmNamedElement, IEdmVocabularyAnnotatable, IEdmElement
	{
		// Token: 0x060000B1 RID: 177 RVA: 0x00002EEB File Offset: 0x000010EB
		public UnresolvedTypeTerm(string qualifiedName) : base(qualifiedName)
		{
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060000B2 RID: 178 RVA: 0x00002EF4 File Offset: 0x000010F4
		public IEnumerable<IEdmStructuralProperty> DeclaredKey
		{
			get
			{
				return Enumerable.Empty<IEdmStructuralProperty>();
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060000B3 RID: 179 RVA: 0x00002EFB File Offset: 0x000010FB
		public bool IsAbstract
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060000B4 RID: 180 RVA: 0x00002EFE File Offset: 0x000010FE
		public bool IsOpen
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060000B5 RID: 181 RVA: 0x00002F01 File Offset: 0x00001101
		public IEdmStructuredType BaseType
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060000B6 RID: 182 RVA: 0x00002F04 File Offset: 0x00001104
		public IEnumerable<IEdmProperty> DeclaredProperties
		{
			get
			{
				return Enumerable.Empty<IEdmProperty>();
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060000B7 RID: 183 RVA: 0x00002F0B File Offset: 0x0000110B
		public EdmTypeKind TypeKind
		{
			get
			{
				return EdmTypeKind.Entity;
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060000B8 RID: 184 RVA: 0x00002F0E File Offset: 0x0000110E
		public override EdmSchemaElementKind SchemaElementKind
		{
			get
			{
				return EdmSchemaElementKind.TypeDefinition;
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x060000B9 RID: 185 RVA: 0x00002F11 File Offset: 0x00001111
		public override EdmTermKind TermKind
		{
			get
			{
				return EdmTermKind.Type;
			}
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00002F14 File Offset: 0x00001114
		public IEdmProperty FindProperty(string name)
		{
			return null;
		}
	}
}
