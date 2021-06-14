using System;
using Microsoft.Data.Edm.Internal;
using Microsoft.Data.Edm.Library.Internal;
using Microsoft.Data.Edm.Library.Values;
using Microsoft.Data.Edm.Validation;
using Microsoft.Data.Edm.Values;

namespace Microsoft.Data.Edm.Csdl.Internal.CsdlSemantics
{
	// Token: 0x0200018A RID: 394
	internal class UnresolvedEnumMember : BadElement, IEdmEnumMember, IEdmNamedElement, IEdmElement
	{
		// Token: 0x060008AB RID: 2219 RVA: 0x0001814C File Offset: 0x0001634C
		public UnresolvedEnumMember(string name, IEdmEnumType declaringType, EdmLocation location) : base(new EdmError[]
		{
			new EdmError(location, EdmErrorCode.BadUnresolvedEnumMember, Strings.Bad_UnresolvedEnumMember(name))
		})
		{
			this.name = (name ?? string.Empty);
			this.declaringType = declaringType;
		}

		// Token: 0x1700039D RID: 925
		// (get) Token: 0x060008AC RID: 2220 RVA: 0x0001819D File Offset: 0x0001639D
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x1700039E RID: 926
		// (get) Token: 0x060008AD RID: 2221 RVA: 0x000181A5 File Offset: 0x000163A5
		public IEdmPrimitiveValue Value
		{
			get
			{
				return this.value.GetValue(this, UnresolvedEnumMember.ComputeValueFunc, null);
			}
		}

		// Token: 0x1700039F RID: 927
		// (get) Token: 0x060008AE RID: 2222 RVA: 0x000181B9 File Offset: 0x000163B9
		public IEdmEnumType DeclaringType
		{
			get
			{
				return this.declaringType;
			}
		}

		// Token: 0x060008AF RID: 2223 RVA: 0x000181C1 File Offset: 0x000163C1
		private IEdmPrimitiveValue ComputeValue()
		{
			return new EdmIntegerConstant(0L);
		}

		// Token: 0x04000449 RID: 1097
		private readonly string name;

		// Token: 0x0400044A RID: 1098
		private readonly IEdmEnumType declaringType;

		// Token: 0x0400044B RID: 1099
		private readonly Cache<UnresolvedEnumMember, IEdmPrimitiveValue> value = new Cache<UnresolvedEnumMember, IEdmPrimitiveValue>();

		// Token: 0x0400044C RID: 1100
		private static readonly Func<UnresolvedEnumMember, IEdmPrimitiveValue> ComputeValueFunc = (UnresolvedEnumMember me) => me.ComputeValue();
	}
}
