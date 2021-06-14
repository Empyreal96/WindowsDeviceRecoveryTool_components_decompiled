using System;
using System.Runtime.Remoting.Lifetime;
using System.Security;
using System.Security.Permissions;

namespace MS.Internal.Utility
{
	// Token: 0x020007EE RID: 2030
	[Serializable]
	internal class SponsorHelper : ISponsor
	{
		// Token: 0x06007D3C RID: 32060 RVA: 0x0023330F File Offset: 0x0023150F
		internal SponsorHelper(ILease lease, TimeSpan timespan)
		{
			this._lease = lease;
			this._timespan = timespan;
		}

		// Token: 0x06007D3D RID: 32061 RVA: 0x00233325 File Offset: 0x00231525
		TimeSpan ISponsor.Renewal(ILease lease)
		{
			if (lease == null)
			{
				throw new ArgumentNullException("lease");
			}
			return this._timespan;
		}

		// Token: 0x06007D3E RID: 32062 RVA: 0x0023333B File Offset: 0x0023153B
		[SecurityCritical]
		[SecurityTreatAsSafe]
		[SecurityPermission(SecurityAction.Assert, RemotingConfiguration = true)]
		internal void Register()
		{
			this._lease.Register(this);
		}

		// Token: 0x06007D3F RID: 32063 RVA: 0x00233349 File Offset: 0x00231549
		[SecurityCritical]
		[SecurityTreatAsSafe]
		[SecurityPermission(SecurityAction.Assert, RemotingConfiguration = true)]
		internal void Unregister()
		{
			this._lease.Unregister(this);
		}

		// Token: 0x04003AEC RID: 15084
		private ILease _lease;

		// Token: 0x04003AED RID: 15085
		private TimeSpan _timespan;
	}
}
