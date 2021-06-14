using System;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000652 RID: 1618
	internal class UnmanagedHandle : IDisposable
	{
		// Token: 0x06006B78 RID: 27512 RVA: 0x001F0E10 File Offset: 0x001EF010
		protected UnmanagedHandle(PtsContext ptsContext)
		{
			this._ptsContext = ptsContext;
			this._handle = ptsContext.CreateHandle(this);
		}

		// Token: 0x06006B79 RID: 27513 RVA: 0x001F0E2C File Offset: 0x001EF02C
		public virtual void Dispose()
		{
			try
			{
				this._ptsContext.ReleaseHandle(this._handle);
			}
			finally
			{
				this._handle = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}

		// Token: 0x170019BF RID: 6591
		// (get) Token: 0x06006B7A RID: 27514 RVA: 0x001F0E70 File Offset: 0x001EF070
		internal IntPtr Handle
		{
			get
			{
				return this._handle;
			}
		}

		// Token: 0x170019C0 RID: 6592
		// (get) Token: 0x06006B7B RID: 27515 RVA: 0x001F0E78 File Offset: 0x001EF078
		internal PtsContext PtsContext
		{
			get
			{
				return this._ptsContext;
			}
		}

		// Token: 0x0400346A RID: 13418
		private IntPtr _handle;

		// Token: 0x0400346B RID: 13419
		private readonly PtsContext _ptsContext;
	}
}
