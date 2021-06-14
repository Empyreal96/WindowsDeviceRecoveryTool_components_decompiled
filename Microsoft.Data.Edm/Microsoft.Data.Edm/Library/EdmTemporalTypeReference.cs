using System;

namespace Microsoft.Data.Edm.Library
{
	// Token: 0x0200010C RID: 268
	public class EdmTemporalTypeReference : EdmPrimitiveTypeReference, IEdmTemporalTypeReference, IEdmPrimitiveTypeReference, IEdmTypeReference, IEdmElement
	{
		// Token: 0x0600051F RID: 1311 RVA: 0x0000CB90 File Offset: 0x0000AD90
		public EdmTemporalTypeReference(IEdmPrimitiveType definition, bool isNullable) : this(definition, isNullable, null)
		{
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x0000CBAE File Offset: 0x0000ADAE
		public EdmTemporalTypeReference(IEdmPrimitiveType definition, bool isNullable, int? precision) : base(definition, isNullable)
		{
			this.precision = precision;
		}

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x06000521 RID: 1313 RVA: 0x0000CBBF File Offset: 0x0000ADBF
		public int? Precision
		{
			get
			{
				return this.precision;
			}
		}

		// Token: 0x040001E5 RID: 485
		private readonly int? precision;
	}
}
