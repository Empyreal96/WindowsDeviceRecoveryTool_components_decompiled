using System;
using System.Collections.Generic;
using Microsoft.Data.Edm.Annotations;
using Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast;
using Microsoft.Data.Edm.Internal;
using Microsoft.Data.Edm.Library;
using Microsoft.Data.Edm.Library.Internal;
using Microsoft.Data.Edm.Validation;

namespace Microsoft.Data.Edm.Csdl.Internal.CsdlSemantics
{
	// Token: 0x02000064 RID: 100
	internal class CsdlSemanticsEnumTypeDefinition : CsdlSemanticsTypeDefinition, IEdmEnumType, IEdmSchemaType, IEdmSchemaElement, IEdmNamedElement, IEdmVocabularyAnnotatable, IEdmType, IEdmElement
	{
		// Token: 0x0600019B RID: 411 RVA: 0x00004F4E File Offset: 0x0000314E
		public CsdlSemanticsEnumTypeDefinition(CsdlSemanticsSchema context, CsdlEnumType enumeration) : base(enumeration)
		{
			this.context = context;
			this.enumeration = enumeration;
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x0600019C RID: 412 RVA: 0x00004F7B File Offset: 0x0000317B
		IEdmPrimitiveType IEdmEnumType.UnderlyingType
		{
			get
			{
				return this.underlyingTypeCache.GetValue(this, CsdlSemanticsEnumTypeDefinition.ComputeUnderlyingTypeFunc, null);
			}
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x0600019D RID: 413 RVA: 0x00004F8F File Offset: 0x0000318F
		IEnumerable<IEdmEnumMember> IEdmEnumType.Members
		{
			get
			{
				return this.membersCache.GetValue(this, CsdlSemanticsEnumTypeDefinition.ComputeMembersFunc, null);
			}
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x0600019E RID: 414 RVA: 0x00004FA3 File Offset: 0x000031A3
		bool IEdmEnumType.IsFlags
		{
			get
			{
				return this.enumeration.IsFlags;
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x0600019F RID: 415 RVA: 0x00004FB0 File Offset: 0x000031B0
		EdmSchemaElementKind IEdmSchemaElement.SchemaElementKind
		{
			get
			{
				return EdmSchemaElementKind.TypeDefinition;
			}
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x060001A0 RID: 416 RVA: 0x00004FB3 File Offset: 0x000031B3
		public string Namespace
		{
			get
			{
				return this.context.Namespace;
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x060001A1 RID: 417 RVA: 0x00004FC0 File Offset: 0x000031C0
		string IEdmNamedElement.Name
		{
			get
			{
				return this.enumeration.Name;
			}
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x060001A2 RID: 418 RVA: 0x00004FCD File Offset: 0x000031CD
		public override EdmTypeKind TypeKind
		{
			get
			{
				return EdmTypeKind.Enum;
			}
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x060001A3 RID: 419 RVA: 0x00004FD0 File Offset: 0x000031D0
		public override CsdlSemanticsModel Model
		{
			get
			{
				return this.context.Model;
			}
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x060001A4 RID: 420 RVA: 0x00004FDD File Offset: 0x000031DD
		public override CsdlElement Element
		{
			get
			{
				return this.enumeration;
			}
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x00004FE5 File Offset: 0x000031E5
		protected override IEnumerable<IEdmVocabularyAnnotation> ComputeInlineVocabularyAnnotations()
		{
			return this.Model.WrapInlineVocabularyAnnotations(this, this.context);
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x00004FFC File Offset: 0x000031FC
		private IEdmPrimitiveType ComputeUnderlyingType()
		{
			if (this.enumeration.UnderlyingTypeName == null)
			{
				return EdmCoreModel.Instance.GetPrimitiveType(EdmPrimitiveTypeKind.Int32);
			}
			EdmPrimitiveTypeKind primitiveTypeKind = EdmCoreModel.Instance.GetPrimitiveTypeKind(this.enumeration.UnderlyingTypeName);
			if (primitiveTypeKind == EdmPrimitiveTypeKind.None)
			{
				return new UnresolvedPrimitiveType(this.enumeration.UnderlyingTypeName, base.Location);
			}
			return EdmCoreModel.Instance.GetPrimitiveType(primitiveTypeKind);
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x00005060 File Offset: 0x00003260
		private IEnumerable<IEdmEnumMember> ComputeMembers()
		{
			List<IEdmEnumMember> list = new List<IEdmEnumMember>();
			long num = -1L;
			foreach (CsdlEnumMember csdlEnumMember in this.enumeration.Members)
			{
				long? value = null;
				IEdmEnumMember edmEnumMember;
				if (csdlEnumMember.Value == null)
				{
					if (num < 9223372036854775807L)
					{
						value = new long?(num + 1L);
						num = value.Value;
						csdlEnumMember.Value = value;
						edmEnumMember = new CsdlSemanticsEnumMember(this, csdlEnumMember);
					}
					else
					{
						edmEnumMember = new BadEnumMember(this, csdlEnumMember.Name, new EdmError[]
						{
							new EdmError(csdlEnumMember.Location ?? base.Location, EdmErrorCode.EnumMemberValueOutOfRange, Strings.CsdlSemantics_EnumMemberValueOutOfRange)
						});
					}
					edmEnumMember.SetIsValueExplicit(this.Model, new bool?(false));
				}
				else
				{
					num = csdlEnumMember.Value.Value;
					edmEnumMember = new CsdlSemanticsEnumMember(this, csdlEnumMember);
					edmEnumMember.SetIsValueExplicit(this.Model, new bool?(true));
				}
				list.Add(edmEnumMember);
			}
			return list;
		}

		// Token: 0x040000AF RID: 175
		private readonly CsdlSemanticsSchema context;

		// Token: 0x040000B0 RID: 176
		private readonly CsdlEnumType enumeration;

		// Token: 0x040000B1 RID: 177
		private readonly Cache<CsdlSemanticsEnumTypeDefinition, IEdmPrimitiveType> underlyingTypeCache = new Cache<CsdlSemanticsEnumTypeDefinition, IEdmPrimitiveType>();

		// Token: 0x040000B2 RID: 178
		private static readonly Func<CsdlSemanticsEnumTypeDefinition, IEdmPrimitiveType> ComputeUnderlyingTypeFunc = (CsdlSemanticsEnumTypeDefinition me) => me.ComputeUnderlyingType();

		// Token: 0x040000B3 RID: 179
		private readonly Cache<CsdlSemanticsEnumTypeDefinition, IEnumerable<IEdmEnumMember>> membersCache = new Cache<CsdlSemanticsEnumTypeDefinition, IEnumerable<IEdmEnumMember>>();

		// Token: 0x040000B4 RID: 180
		private static readonly Func<CsdlSemanticsEnumTypeDefinition, IEnumerable<IEdmEnumMember>> ComputeMembersFunc = (CsdlSemanticsEnumTypeDefinition me) => me.ComputeMembers();
	}
}
