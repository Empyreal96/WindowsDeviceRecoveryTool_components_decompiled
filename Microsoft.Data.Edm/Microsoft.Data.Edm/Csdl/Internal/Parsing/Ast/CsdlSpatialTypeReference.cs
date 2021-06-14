using System;

namespace Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast
{
	// Token: 0x02000023 RID: 35
	internal class CsdlSpatialTypeReference : CsdlPrimitiveTypeReference
	{
		// Token: 0x06000086 RID: 134 RVA: 0x00002D61 File Offset: 0x00000F61
		public CsdlSpatialTypeReference(EdmPrimitiveTypeKind kind, int? srid, string typeName, bool isNullable, CsdlLocation location) : base(kind, typeName, isNullable, location)
		{
			this.srid = srid;
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000087 RID: 135 RVA: 0x00002D76 File Offset: 0x00000F76
		public int? Srid
		{
			get
			{
				return this.srid;
			}
		}

		// Token: 0x04000035 RID: 53
		private readonly int? srid;
	}
}
