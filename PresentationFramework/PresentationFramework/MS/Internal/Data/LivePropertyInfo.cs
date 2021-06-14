using System;
using System.Windows;

namespace MS.Internal.Data
{
	// Token: 0x0200072E RID: 1838
	internal struct LivePropertyInfo
	{
		// Token: 0x060075AE RID: 30126 RVA: 0x0021991B File Offset: 0x00217B1B
		public LivePropertyInfo(string path, DependencyProperty dp)
		{
			this._path = path;
			this._dp = dp;
		}

		// Token: 0x17001C06 RID: 7174
		// (get) Token: 0x060075AF RID: 30127 RVA: 0x0021992B File Offset: 0x00217B2B
		public string Path
		{
			get
			{
				return this._path;
			}
		}

		// Token: 0x17001C07 RID: 7175
		// (get) Token: 0x060075B0 RID: 30128 RVA: 0x00219933 File Offset: 0x00217B33
		public DependencyProperty Property
		{
			get
			{
				return this._dp;
			}
		}

		// Token: 0x04003837 RID: 14391
		private string _path;

		// Token: 0x04003838 RID: 14392
		private DependencyProperty _dp;
	}
}
