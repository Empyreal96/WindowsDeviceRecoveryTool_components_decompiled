using System;
using System.Windows.Threading;

namespace MS.Internal.Data
{
	// Token: 0x02000716 RID: 1814
	internal class DataBindOperation
	{
		// Token: 0x060074CA RID: 29898 RVA: 0x002169EF File Offset: 0x00214BEF
		public DataBindOperation(DispatcherOperationCallback method, object arg, int cost = 1)
		{
			this._method = method;
			this._arg = arg;
			this._cost = cost;
		}

		// Token: 0x17001BCA RID: 7114
		// (get) Token: 0x060074CB RID: 29899 RVA: 0x00216A0C File Offset: 0x00214C0C
		// (set) Token: 0x060074CC RID: 29900 RVA: 0x00216A14 File Offset: 0x00214C14
		public int Cost
		{
			get
			{
				return this._cost;
			}
			set
			{
				this._cost = value;
			}
		}

		// Token: 0x060074CD RID: 29901 RVA: 0x00216A1D File Offset: 0x00214C1D
		public void Invoke()
		{
			this._method(this._arg);
		}

		// Token: 0x040037FA RID: 14330
		private DispatcherOperationCallback _method;

		// Token: 0x040037FB RID: 14331
		private object _arg;

		// Token: 0x040037FC RID: 14332
		private int _cost;
	}
}
