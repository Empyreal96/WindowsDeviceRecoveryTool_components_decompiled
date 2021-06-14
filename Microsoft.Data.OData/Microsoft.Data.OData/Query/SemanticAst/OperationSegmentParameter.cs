using System;

namespace Microsoft.Data.OData.Query.SemanticAst
{
	// Token: 0x02000073 RID: 115
	public sealed class OperationSegmentParameter : ODataAnnotatable
	{
		// Token: 0x060002B9 RID: 697 RVA: 0x0000A799 File Offset: 0x00008999
		public OperationSegmentParameter(string name, object value)
		{
			ExceptionUtils.CheckArgumentStringNotNullOrEmpty(name, "name");
			this.Name = name;
			this.Value = value;
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x060002BA RID: 698 RVA: 0x0000A7BA File Offset: 0x000089BA
		// (set) Token: 0x060002BB RID: 699 RVA: 0x0000A7C2 File Offset: 0x000089C2
		public string Name { get; private set; }

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x060002BC RID: 700 RVA: 0x0000A7CB File Offset: 0x000089CB
		// (set) Token: 0x060002BD RID: 701 RVA: 0x0000A7D3 File Offset: 0x000089D3
		public object Value { get; private set; }
	}
}
