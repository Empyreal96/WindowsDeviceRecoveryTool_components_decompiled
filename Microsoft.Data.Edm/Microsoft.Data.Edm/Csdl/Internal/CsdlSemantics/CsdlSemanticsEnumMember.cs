using System;
using Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast;
using Microsoft.Data.Edm.Internal;
using Microsoft.Data.Edm.Library;
using Microsoft.Data.Edm.Library.Values;
using Microsoft.Data.Edm.Values;

namespace Microsoft.Data.Edm.Csdl.Internal.CsdlSemantics
{
	// Token: 0x02000062 RID: 98
	internal class CsdlSemanticsEnumMember : CsdlSemanticsElement, IEdmEnumMember, IEdmNamedElement, IEdmElement
	{
		// Token: 0x0600018E RID: 398 RVA: 0x00004E77 File Offset: 0x00003077
		public CsdlSemanticsEnumMember(CsdlSemanticsEnumTypeDefinition declaringType, CsdlEnumMember member) : base(member)
		{
			this.member = member;
			this.declaringType = declaringType;
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x0600018F RID: 399 RVA: 0x00004E99 File Offset: 0x00003099
		public string Name
		{
			get
			{
				return this.member.Name;
			}
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x06000190 RID: 400 RVA: 0x00004EA6 File Offset: 0x000030A6
		public IEdmEnumType DeclaringType
		{
			get
			{
				return this.declaringType;
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x06000191 RID: 401 RVA: 0x00004EAE File Offset: 0x000030AE
		public IEdmPrimitiveValue Value
		{
			get
			{
				return this.valueCache.GetValue(this, CsdlSemanticsEnumMember.ComputeValueFunc, null);
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x06000192 RID: 402 RVA: 0x00004EC2 File Offset: 0x000030C2
		public override CsdlSemanticsModel Model
		{
			get
			{
				return this.declaringType.Model;
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x06000193 RID: 403 RVA: 0x00004ECF File Offset: 0x000030CF
		public override CsdlElement Element
		{
			get
			{
				return this.member;
			}
		}

		// Token: 0x06000194 RID: 404 RVA: 0x00004ED8 File Offset: 0x000030D8
		private IEdmPrimitiveValue ComputeValue()
		{
			return new EdmIntegerConstant(new EdmPrimitiveTypeReference(this.DeclaringType.UnderlyingType, false), this.member.Value.Value);
		}

		// Token: 0x040000AA RID: 170
		private readonly CsdlEnumMember member;

		// Token: 0x040000AB RID: 171
		private readonly CsdlSemanticsEnumTypeDefinition declaringType;

		// Token: 0x040000AC RID: 172
		private readonly Cache<CsdlSemanticsEnumMember, IEdmPrimitiveValue> valueCache = new Cache<CsdlSemanticsEnumMember, IEdmPrimitiveValue>();

		// Token: 0x040000AD RID: 173
		private static readonly Func<CsdlSemanticsEnumMember, IEdmPrimitiveValue> ComputeValueFunc = (CsdlSemanticsEnumMember me) => me.ComputeValue();
	}
}
