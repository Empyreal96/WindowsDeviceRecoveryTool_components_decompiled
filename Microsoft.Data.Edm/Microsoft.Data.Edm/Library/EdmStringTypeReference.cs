using System;

namespace Microsoft.Data.Edm.Library
{
	// Token: 0x02000109 RID: 265
	public class EdmStringTypeReference : EdmPrimitiveTypeReference, IEdmStringTypeReference, IEdmPrimitiveTypeReference, IEdmTypeReference, IEdmElement
	{
		// Token: 0x06000514 RID: 1300 RVA: 0x0000CA50 File Offset: 0x0000AC50
		public EdmStringTypeReference(IEdmPrimitiveType definition, bool isNullable) : this(definition, isNullable, false, null, null, null, null)
		{
		}

		// Token: 0x06000515 RID: 1301 RVA: 0x0000CA84 File Offset: 0x0000AC84
		public EdmStringTypeReference(IEdmPrimitiveType definition, bool isNullable, bool isUnbounded, int? maxLength, bool? isFixedLength, bool? isUnicode, string collation) : base(definition, isNullable)
		{
			if (isUnbounded && maxLength != null)
			{
				throw new InvalidOperationException(Strings.EdmModel_Validator_Semantic_IsUnboundedCannotBeTrueWhileMaxLengthIsNotNull);
			}
			this.isUnbounded = isUnbounded;
			this.maxLength = maxLength;
			this.isFixedLength = isFixedLength;
			this.isUnicode = isUnicode;
			this.collation = collation;
		}

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x06000516 RID: 1302 RVA: 0x0000CAD7 File Offset: 0x0000ACD7
		public bool? IsFixedLength
		{
			get
			{
				return this.isFixedLength;
			}
		}

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x06000517 RID: 1303 RVA: 0x0000CADF File Offset: 0x0000ACDF
		public bool IsUnbounded
		{
			get
			{
				return this.isUnbounded;
			}
		}

		// Token: 0x17000238 RID: 568
		// (get) Token: 0x06000518 RID: 1304 RVA: 0x0000CAE7 File Offset: 0x0000ACE7
		public int? MaxLength
		{
			get
			{
				return this.maxLength;
			}
		}

		// Token: 0x17000239 RID: 569
		// (get) Token: 0x06000519 RID: 1305 RVA: 0x0000CAEF File Offset: 0x0000ACEF
		public bool? IsUnicode
		{
			get
			{
				return this.isUnicode;
			}
		}

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x0600051A RID: 1306 RVA: 0x0000CAF7 File Offset: 0x0000ACF7
		public string Collation
		{
			get
			{
				return this.collation;
			}
		}

		// Token: 0x040001DF RID: 479
		private readonly bool isUnbounded;

		// Token: 0x040001E0 RID: 480
		private readonly int? maxLength;

		// Token: 0x040001E1 RID: 481
		private readonly bool? isFixedLength;

		// Token: 0x040001E2 RID: 482
		private readonly bool? isUnicode;

		// Token: 0x040001E3 RID: 483
		private readonly string collation;
	}
}
