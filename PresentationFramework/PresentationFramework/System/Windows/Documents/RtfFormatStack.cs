using System;
using System.Collections;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x020003A9 RID: 937
	internal class RtfFormatStack : ArrayList
	{
		// Token: 0x0600329A RID: 12954 RVA: 0x000E3C42 File Offset: 0x000E1E42
		internal RtfFormatStack() : base(20)
		{
		}

		// Token: 0x0600329B RID: 12955 RVA: 0x000E3C4C File Offset: 0x000E1E4C
		internal void Push()
		{
			FormatState formatState = this.Top();
			FormatState value = (formatState != null) ? new FormatState(formatState) : new FormatState();
			this.Add(value);
		}

		// Token: 0x0600329C RID: 12956 RVA: 0x000E3C79 File Offset: 0x000E1E79
		internal void Pop()
		{
			Invariant.Assert(this.Count != 0);
			if (this.Count > 0)
			{
				this.RemoveAt(this.Count - 1);
			}
		}

		// Token: 0x0600329D RID: 12957 RVA: 0x000E3CA0 File Offset: 0x000E1EA0
		internal FormatState Top()
		{
			if (this.Count <= 0)
			{
				return null;
			}
			return this.EntryAt(this.Count - 1);
		}

		// Token: 0x0600329E RID: 12958 RVA: 0x000E3CBC File Offset: 0x000E1EBC
		internal FormatState PrevTop(int fromTop)
		{
			int num = this.Count - 1 - fromTop;
			if (num < 0 || num >= this.Count)
			{
				return null;
			}
			return this.EntryAt(num);
		}

		// Token: 0x0600329F RID: 12959 RVA: 0x000E3CEA File Offset: 0x000E1EEA
		internal FormatState EntryAt(int index)
		{
			return (FormatState)this[index];
		}
	}
}
