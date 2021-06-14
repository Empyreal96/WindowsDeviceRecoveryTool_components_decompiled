using System;

namespace MS.Internal.Documents
{
	// Token: 0x020006DC RID: 1756
	internal class PageCacheChange
	{
		// Token: 0x06007167 RID: 29031 RVA: 0x002073A1 File Offset: 0x002055A1
		public PageCacheChange(int start, int count, PageCacheChangeType type)
		{
			this._start = start;
			this._count = count;
			this._type = type;
		}

		// Token: 0x17001AE7 RID: 6887
		// (get) Token: 0x06007168 RID: 29032 RVA: 0x002073BE File Offset: 0x002055BE
		public int Start
		{
			get
			{
				return this._start;
			}
		}

		// Token: 0x17001AE8 RID: 6888
		// (get) Token: 0x06007169 RID: 29033 RVA: 0x002073C6 File Offset: 0x002055C6
		public int Count
		{
			get
			{
				return this._count;
			}
		}

		// Token: 0x17001AE9 RID: 6889
		// (get) Token: 0x0600716A RID: 29034 RVA: 0x002073CE File Offset: 0x002055CE
		public PageCacheChangeType Type
		{
			get
			{
				return this._type;
			}
		}

		// Token: 0x0400371A RID: 14106
		private readonly int _start;

		// Token: 0x0400371B RID: 14107
		private readonly int _count;

		// Token: 0x0400371C RID: 14108
		private readonly PageCacheChangeType _type;
	}
}
