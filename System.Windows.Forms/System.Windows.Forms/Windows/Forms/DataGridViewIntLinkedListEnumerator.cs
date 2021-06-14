using System;
using System.Collections;

namespace System.Windows.Forms
{
	// Token: 0x020001F2 RID: 498
	internal class DataGridViewIntLinkedListEnumerator : IEnumerator
	{
		// Token: 0x06001E26 RID: 7718 RVA: 0x00095C84 File Offset: 0x00093E84
		public DataGridViewIntLinkedListEnumerator(DataGridViewIntLinkedListElement headElement)
		{
			this.headElement = headElement;
			this.reset = true;
		}

		// Token: 0x1700070F RID: 1807
		// (get) Token: 0x06001E27 RID: 7719 RVA: 0x00095C9A File Offset: 0x00093E9A
		object IEnumerator.Current
		{
			get
			{
				return this.current.Int;
			}
		}

		// Token: 0x06001E28 RID: 7720 RVA: 0x00095CAC File Offset: 0x00093EAC
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

		// Token: 0x06001E29 RID: 7721 RVA: 0x00095CE5 File Offset: 0x00093EE5
		void IEnumerator.Reset()
		{
			this.reset = true;
			this.current = null;
		}

		// Token: 0x04000D43 RID: 3395
		private DataGridViewIntLinkedListElement headElement;

		// Token: 0x04000D44 RID: 3396
		private DataGridViewIntLinkedListElement current;

		// Token: 0x04000D45 RID: 3397
		private bool reset;
	}
}
