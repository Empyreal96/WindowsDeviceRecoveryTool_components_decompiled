using System;
using System.Collections;

namespace System.Windows
{
	// Token: 0x02000140 RID: 320
	internal class SingleChildEnumerator : IEnumerator
	{
		// Token: 0x06000E39 RID: 3641 RVA: 0x00036A6C File Offset: 0x00034C6C
		internal SingleChildEnumerator(object Child)
		{
			this._child = Child;
			this._count = ((Child == null) ? 0 : 1);
		}

		// Token: 0x17000455 RID: 1109
		// (get) Token: 0x06000E3A RID: 3642 RVA: 0x00036A8F File Offset: 0x00034C8F
		object IEnumerator.Current
		{
			get
			{
				if (this._index != 0)
				{
					return null;
				}
				return this._child;
			}
		}

		// Token: 0x06000E3B RID: 3643 RVA: 0x00036AA1 File Offset: 0x00034CA1
		bool IEnumerator.MoveNext()
		{
			this._index++;
			return this._index < this._count;
		}

		// Token: 0x06000E3C RID: 3644 RVA: 0x00036ABF File Offset: 0x00034CBF
		void IEnumerator.Reset()
		{
			this._index = -1;
		}

		// Token: 0x04000BA2 RID: 2978
		private int _index = -1;

		// Token: 0x04000BA3 RID: 2979
		private int _count;

		// Token: 0x04000BA4 RID: 2980
		private object _child;
	}
}
