using System;
using System.Collections;

namespace System.Windows.Forms
{
	// Token: 0x0200019C RID: 412
	internal class DataGridViewCellLinkedListEnumerator : IEnumerator
	{
		// Token: 0x06001AFA RID: 6906 RVA: 0x0008704B File Offset: 0x0008524B
		public DataGridViewCellLinkedListEnumerator(DataGridViewCellLinkedListElement headElement)
		{
			this.headElement = headElement;
			this.reset = true;
		}

		// Token: 0x17000621 RID: 1569
		// (get) Token: 0x06001AFB RID: 6907 RVA: 0x00087061 File Offset: 0x00085261
		object IEnumerator.Current
		{
			get
			{
				return this.current.DataGridViewCell;
			}
		}

		// Token: 0x06001AFC RID: 6908 RVA: 0x0008706E File Offset: 0x0008526E
		bool IEnumerator.MoveNext()
		{
			if (this.reset)
			{
				this.current = this.headElement;
				this.reset = false;
			}
			else
			{
				this.current = this.current.Next;
			}
			return this.current != null;
		}

		// Token: 0x06001AFD RID: 6909 RVA: 0x000870A7 File Offset: 0x000852A7
		void IEnumerator.Reset()
		{
			this.reset = true;
			this.current = null;
		}

		// Token: 0x04000C13 RID: 3091
		private DataGridViewCellLinkedListElement headElement;

		// Token: 0x04000C14 RID: 3092
		private DataGridViewCellLinkedListElement current;

		// Token: 0x04000C15 RID: 3093
		private bool reset;
	}
}
