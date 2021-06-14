using System;
using System.Runtime.InteropServices;

namespace Standard
{
	// Token: 0x02000078 RID: 120
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	internal struct PKEY
	{
		// Token: 0x0600011B RID: 283 RVA: 0x00003DAF File Offset: 0x00001FAF
		public PKEY(Guid fmtid, uint pid)
		{
			this._fmtid = fmtid;
			this._pid = pid;
		}

		// Token: 0x0400056C RID: 1388
		private readonly Guid _fmtid;

		// Token: 0x0400056D RID: 1389
		private readonly uint _pid;

		// Token: 0x0400056E RID: 1390
		public static readonly PKEY Title = new PKEY(new Guid("F29F85E0-4FF9-1068-AB91-08002B27B3D9"), 2U);

		// Token: 0x0400056F RID: 1391
		public static readonly PKEY AppUserModel_ID = new PKEY(new Guid("9F4C2855-9F79-4B39-A8D0-E1D42DE1D5F3"), 5U);

		// Token: 0x04000570 RID: 1392
		public static readonly PKEY AppUserModel_IsDestListSeparator = new PKEY(new Guid("9F4C2855-9F79-4B39-A8D0-E1D42DE1D5F3"), 6U);

		// Token: 0x04000571 RID: 1393
		public static readonly PKEY AppUserModel_RelaunchCommand = new PKEY(new Guid("9F4C2855-9F79-4B39-A8D0-E1D42DE1D5F3"), 2U);

		// Token: 0x04000572 RID: 1394
		public static readonly PKEY AppUserModel_RelaunchDisplayNameResource = new PKEY(new Guid("9F4C2855-9F79-4B39-A8D0-E1D42DE1D5F3"), 4U);

		// Token: 0x04000573 RID: 1395
		public static readonly PKEY AppUserModel_RelaunchIconResource = new PKEY(new Guid("9F4C2855-9F79-4B39-A8D0-E1D42DE1D5F3"), 3U);
	}
}
