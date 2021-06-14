using System;
using System.Collections.ObjectModel;

namespace Microsoft.Data.OData.Query.SemanticAst
{
	// Token: 0x0200009A RID: 154
	public abstract class LambdaNode : SingleValueNode
	{
		// Token: 0x060003A1 RID: 929 RVA: 0x0000BCB0 File Offset: 0x00009EB0
		protected LambdaNode(Collection<RangeVariable> rangeVariables) : this(rangeVariables, null)
		{
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x0000BCBA File Offset: 0x00009EBA
		protected LambdaNode(Collection<RangeVariable> rangeVariables, RangeVariable currentRangeVariable)
		{
			this.rangeVariables = rangeVariables;
			this.currentRangeVariable = currentRangeVariable;
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x060003A3 RID: 931 RVA: 0x0000BCD0 File Offset: 0x00009ED0
		public Collection<RangeVariable> RangeVariables
		{
			get
			{
				return this.rangeVariables;
			}
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x060003A4 RID: 932 RVA: 0x0000BCD8 File Offset: 0x00009ED8
		public RangeVariable CurrentRangeVariable
		{
			get
			{
				return this.currentRangeVariable;
			}
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x060003A5 RID: 933 RVA: 0x0000BCE0 File Offset: 0x00009EE0
		// (set) Token: 0x060003A6 RID: 934 RVA: 0x0000BCE8 File Offset: 0x00009EE8
		public SingleValueNode Body { get; set; }

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x060003A7 RID: 935 RVA: 0x0000BCF1 File Offset: 0x00009EF1
		// (set) Token: 0x060003A8 RID: 936 RVA: 0x0000BCF9 File Offset: 0x00009EF9
		public CollectionNode Source { get; set; }

		// Token: 0x04000118 RID: 280
		private readonly Collection<RangeVariable> rangeVariables;

		// Token: 0x04000119 RID: 281
		private readonly RangeVariable currentRangeVariable;
	}
}
