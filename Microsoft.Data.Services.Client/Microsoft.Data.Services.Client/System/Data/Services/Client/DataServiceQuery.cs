using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;

namespace System.Data.Services.Client
{
	// Token: 0x0200010A RID: 266
	public abstract class DataServiceQuery : DataServiceRequest, IQueryable, IEnumerable
	{
		// Token: 0x060008A6 RID: 2214 RVA: 0x00024400 File Offset: 0x00022600
		internal DataServiceQuery()
		{
		}

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x060008A7 RID: 2215
		public abstract Expression Expression { get; }

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x060008A8 RID: 2216
		public abstract IQueryProvider Provider { get; }

		// Token: 0x060008A9 RID: 2217 RVA: 0x00024408 File Offset: 0x00022608
		IEnumerator IEnumerable.GetEnumerator()
		{
			throw Error.NotImplemented();
		}

		// Token: 0x060008AA RID: 2218 RVA: 0x0002440F File Offset: 0x0002260F
		public IEnumerable Execute()
		{
			return this.ExecuteInternal();
		}

		// Token: 0x060008AB RID: 2219 RVA: 0x00024417 File Offset: 0x00022617
		public IAsyncResult BeginExecute(AsyncCallback callback, object state)
		{
			return this.BeginExecuteInternal(callback, state);
		}

		// Token: 0x060008AC RID: 2220 RVA: 0x00024421 File Offset: 0x00022621
		public IEnumerable EndExecute(IAsyncResult asyncResult)
		{
			return this.EndExecuteInternal(asyncResult);
		}

		// Token: 0x060008AD RID: 2221
		internal abstract IEnumerable ExecuteInternal();

		// Token: 0x060008AE RID: 2222
		internal abstract IAsyncResult BeginExecuteInternal(AsyncCallback callback, object state);

		// Token: 0x060008AF RID: 2223
		internal abstract IEnumerable EndExecuteInternal(IAsyncResult asyncResult);
	}
}
