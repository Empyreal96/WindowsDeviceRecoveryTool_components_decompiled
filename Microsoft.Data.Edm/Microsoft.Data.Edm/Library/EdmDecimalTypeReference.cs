using System;

namespace Microsoft.Data.Edm.Library
{
	// Token: 0x020000F3 RID: 243
	public class EdmDecimalTypeReference : EdmPrimitiveTypeReference, IEdmDecimalTypeReference, IEdmPrimitiveTypeReference, IEdmTypeReference, IEdmElement
	{
		// Token: 0x060004C3 RID: 1219 RVA: 0x0000C490 File Offset: 0x0000A690
		public EdmDecimalTypeReference(IEdmPrimitiveType definition, bool isNullable) : this(definition, isNullable, null, null)
		{
		}

		// Token: 0x060004C4 RID: 1220 RVA: 0x0000C4B7 File Offset: 0x0000A6B7
		public EdmDecimalTypeReference(IEdmPrimitiveType definition, bool isNullable, int? precision, int? scale) : base(definition, isNullable)
		{
			this.precision = precision;
			this.scale = scale;
		}

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x060004C5 RID: 1221 RVA: 0x0000C4D0 File Offset: 0x0000A6D0
		public int? Precision
		{
			get
			{
				return this.precision;
			}
		}

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x060004C6 RID: 1222 RVA: 0x0000C4D8 File Offset: 0x0000A6D8
		public int? Scale
		{
			get
			{
				return this.scale;
			}
		}

		// Token: 0x040001C4 RID: 452
		private readonly int? precision;

		// Token: 0x040001C5 RID: 453
		private readonly int? scale;
	}
}
