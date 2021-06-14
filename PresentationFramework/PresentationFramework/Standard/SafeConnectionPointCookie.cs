using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices.ComTypes;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace Standard
{
	// Token: 0x02000041 RID: 65
	[SecurityCritical]
	internal sealed class SafeConnectionPointCookie : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06000062 RID: 98 RVA: 0x0000312C File Offset: 0x0000132C
		[SecurityCritical]
		public SafeConnectionPointCookie(IConnectionPointContainer target, object sink, Guid eventId) : base(true)
		{
			Verify.IsNotNull<IConnectionPointContainer>(target, "target");
			Verify.IsNotNull<object>(sink, "sink");
			Verify.IsNotDefault<Guid>(eventId, "eventId");
			this.handle = IntPtr.Zero;
			IConnectionPoint connectionPoint = null;
			try
			{
				target.FindConnectionPoint(ref eventId, out connectionPoint);
				int num;
				connectionPoint.Advise(sink, out num);
				if (num == 0)
				{
					throw new InvalidOperationException("IConnectionPoint::Advise returned an invalid cookie.");
				}
				this.handle = new IntPtr(num);
				this._cp = connectionPoint;
				connectionPoint = null;
			}
			finally
			{
				Utility.SafeRelease<IConnectionPoint>(ref connectionPoint);
			}
		}

		// Token: 0x06000063 RID: 99 RVA: 0x000031C0 File Offset: 0x000013C0
		[SecurityCritical]
		public void Disconnect()
		{
			this.ReleaseHandle();
		}

		// Token: 0x06000064 RID: 100 RVA: 0x000031CC File Offset: 0x000013CC
		[SecurityCritical]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		protected override bool ReleaseHandle()
		{
			bool result;
			try
			{
				if (!this.IsInvalid)
				{
					int dwCookie = this.handle.ToInt32();
					this.handle = IntPtr.Zero;
					try
					{
						this._cp.Unadvise(dwCookie);
					}
					finally
					{
						Utility.SafeRelease<IConnectionPoint>(ref this._cp);
					}
				}
				result = true;
			}
			catch
			{
				result = false;
			}
			return result;
		}

		// Token: 0x040003FC RID: 1020
		private IConnectionPoint _cp;
	}
}
