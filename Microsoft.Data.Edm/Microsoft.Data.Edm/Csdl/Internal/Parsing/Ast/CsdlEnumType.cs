using System;
using System.Collections.Generic;

namespace Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast
{
	// Token: 0x02000011 RID: 17
	internal class CsdlEnumType : CsdlNamedElement
	{
		// Token: 0x0600004E RID: 78 RVA: 0x00002A5F File Offset: 0x00000C5F
		public CsdlEnumType(string name, string underlyingTypeName, bool isFlags, IEnumerable<CsdlEnumMember> members, CsdlDocumentation documentation, CsdlLocation location) : base(name, documentation, location)
		{
			this.underlyingTypeName = underlyingTypeName;
			this.isFlags = isFlags;
			this.members = new List<CsdlEnumMember>(members);
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600004F RID: 79 RVA: 0x00002A87 File Offset: 0x00000C87
		public string UnderlyingTypeName
		{
			get
			{
				return this.underlyingTypeName;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000050 RID: 80 RVA: 0x00002A8F File Offset: 0x00000C8F
		public bool IsFlags
		{
			get
			{
				return this.isFlags;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000051 RID: 81 RVA: 0x00002A97 File Offset: 0x00000C97
		public IEnumerable<CsdlEnumMember> Members
		{
			get
			{
				return this.members;
			}
		}

		// Token: 0x04000017 RID: 23
		private readonly string underlyingTypeName;

		// Token: 0x04000018 RID: 24
		private readonly bool isFlags;

		// Token: 0x04000019 RID: 25
		private readonly List<CsdlEnumMember> members;
	}
}
