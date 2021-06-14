using System;
using System.Security;
using System.Threading;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000634 RID: 1588
	internal sealed class PageBreakRecord : IDisposable
	{
		// Token: 0x060068F0 RID: 26864 RVA: 0x001D9BBC File Offset: 0x001D7DBC
		internal PageBreakRecord(PtsContext ptsContext, SecurityCriticalDataForSet<IntPtr> br, int pageNumber)
		{
			Invariant.Assert(ptsContext != null, "Invalid PtsContext object.");
			Invariant.Assert(br.Value != IntPtr.Zero, "Invalid break record object.");
			this._br = br;
			this._pageNumber = pageNumber;
			this._ptsContext = new WeakReference(ptsContext);
			ptsContext.OnPageBreakRecordCreated(this._br);
		}

		// Token: 0x060068F1 RID: 26865 RVA: 0x001D9C20 File Offset: 0x001D7E20
		~PageBreakRecord()
		{
			this.Dispose(false);
		}

		// Token: 0x060068F2 RID: 26866 RVA: 0x001D9C50 File Offset: 0x001D7E50
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x17001964 RID: 6500
		// (get) Token: 0x060068F3 RID: 26867 RVA: 0x001D9C5F File Offset: 0x001D7E5F
		internal IntPtr BreakRecord
		{
			get
			{
				return this._br.Value;
			}
		}

		// Token: 0x17001965 RID: 6501
		// (get) Token: 0x060068F4 RID: 26868 RVA: 0x001D9C6C File Offset: 0x001D7E6C
		internal int PageNumber
		{
			get
			{
				return this._pageNumber;
			}
		}

		// Token: 0x060068F5 RID: 26869 RVA: 0x001D9C74 File Offset: 0x001D7E74
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void Dispose(bool disposing)
		{
			if (Interlocked.CompareExchange(ref this._disposed, 1, 0) == 0)
			{
				PtsContext ptsContext = this._ptsContext.Target as PtsContext;
				if (ptsContext != null && !ptsContext.Disposed)
				{
					ptsContext.OnPageBreakRecordDisposed(this._br, disposing);
				}
				this._br.Value = IntPtr.Zero;
				this._ptsContext = null;
			}
		}

		// Token: 0x040033FE RID: 13310
		private SecurityCriticalDataForSet<IntPtr> _br;

		// Token: 0x040033FF RID: 13311
		private readonly int _pageNumber;

		// Token: 0x04003400 RID: 13312
		private WeakReference _ptsContext;

		// Token: 0x04003401 RID: 13313
		private int _disposed;
	}
}
