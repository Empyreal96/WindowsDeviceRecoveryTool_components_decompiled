using System;

namespace Microsoft.Data.Edm.Library
{
	// Token: 0x020000E8 RID: 232
	public class EdmBinaryTypeReference : EdmPrimitiveTypeReference, IEdmBinaryTypeReference, IEdmPrimitiveTypeReference, IEdmTypeReference, IEdmElement
	{
		// Token: 0x060004A5 RID: 1189 RVA: 0x0000C290 File Offset: 0x0000A490
		public EdmBinaryTypeReference(IEdmPrimitiveType definition, bool isNullable) : this(definition, isNullable, false, null, null)
		{
		}

		// Token: 0x060004A6 RID: 1190 RVA: 0x0000C2B8 File Offset: 0x0000A4B8
		public EdmBinaryTypeReference(IEdmPrimitiveType definition, bool isNullable, bool isUnbounded, int? maxLength, bool? isFixedLength) : base(definition, isNullable)
		{
			if (isUnbounded && maxLength != null)
			{
				throw new InvalidOperationException(Strings.EdmModel_Validator_Semantic_IsUnboundedCannotBeTrueWhileMaxLengthIsNotNull);
			}
			this.isUnbounded = isUnbounded;
			this.maxLength = maxLength;
			this.isFixedLength = isFixedLength;
		}

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x060004A7 RID: 1191 RVA: 0x0000C2F0 File Offset: 0x0000A4F0
		public bool? IsFixedLength
		{
			get
			{
				return this.isFixedLength;
			}
		}

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x060004A8 RID: 1192 RVA: 0x0000C2F8 File Offset: 0x0000A4F8
		public bool IsUnbounded
		{
			get
			{
				return this.isUnbounded;
			}
		}

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x060004A9 RID: 1193 RVA: 0x0000C300 File Offset: 0x0000A500
		public int? MaxLength
		{
			get
			{
				return this.maxLength;
			}
		}

		// Token: 0x040001BC RID: 444
		private readonly bool isUnbounded;

		// Token: 0x040001BD RID: 445
		private readonly int? maxLength;

		// Token: 0x040001BE RID: 446
		private readonly bool? isFixedLength;
	}
}
