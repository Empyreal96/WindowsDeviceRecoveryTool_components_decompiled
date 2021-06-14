using System;

namespace Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast
{
	// Token: 0x0200014C RID: 332
	internal class CsdlStringTypeReference : CsdlPrimitiveTypeReference
	{
		// Token: 0x06000633 RID: 1587 RVA: 0x0000F90C File Offset: 0x0000DB0C
		public CsdlStringTypeReference(bool? isFixedLength, bool isUnbounded, int? maxLength, bool? isUnicode, string collation, string typeName, bool isNullable, CsdlLocation location) : base(EdmPrimitiveTypeKind.String, typeName, isNullable, location)
		{
			this.isFixedLength = isFixedLength;
			this.isUnbounded = isUnbounded;
			this.maxLength = maxLength;
			this.isUnicode = isUnicode;
			this.collation = collation;
		}

		// Token: 0x170002A4 RID: 676
		// (get) Token: 0x06000634 RID: 1588 RVA: 0x0000F941 File Offset: 0x0000DB41
		public bool? IsFixedLength
		{
			get
			{
				return this.isFixedLength;
			}
		}

		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x06000635 RID: 1589 RVA: 0x0000F949 File Offset: 0x0000DB49
		public bool IsUnbounded
		{
			get
			{
				return this.isUnbounded;
			}
		}

		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x06000636 RID: 1590 RVA: 0x0000F951 File Offset: 0x0000DB51
		public int? MaxLength
		{
			get
			{
				return this.maxLength;
			}
		}

		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x06000637 RID: 1591 RVA: 0x0000F959 File Offset: 0x0000DB59
		public bool? IsUnicode
		{
			get
			{
				return this.isUnicode;
			}
		}

		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x06000638 RID: 1592 RVA: 0x0000F961 File Offset: 0x0000DB61
		public string Collation
		{
			get
			{
				return this.collation;
			}
		}

		// Token: 0x04000353 RID: 851
		private readonly bool? isFixedLength;

		// Token: 0x04000354 RID: 852
		private readonly bool isUnbounded;

		// Token: 0x04000355 RID: 853
		private readonly int? maxLength;

		// Token: 0x04000356 RID: 854
		private readonly bool? isUnicode;

		// Token: 0x04000357 RID: 855
		private readonly string collation;
	}
}
