using System;

namespace Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast
{
	// Token: 0x0200000F RID: 15
	internal class CsdlEnumMember : CsdlNamedElement
	{
		// Token: 0x06000048 RID: 72 RVA: 0x00002A1F File Offset: 0x00000C1F
		public CsdlEnumMember(string name, long? value, CsdlDocumentation documentation, CsdlLocation location) : base(name, documentation, location)
		{
			this.Value = value;
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000049 RID: 73 RVA: 0x00002A32 File Offset: 0x00000C32
		// (set) Token: 0x0600004A RID: 74 RVA: 0x00002A3A File Offset: 0x00000C3A
		public long? Value { get; set; }
	}
}
