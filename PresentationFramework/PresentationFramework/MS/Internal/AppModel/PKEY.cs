using System;
using System.Runtime.InteropServices;

namespace MS.Internal.AppModel
{
	// Token: 0x020007A5 RID: 1957
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	internal struct PKEY
	{
		// Token: 0x06007A6C RID: 31340 RVA: 0x0022B0CD File Offset: 0x002292CD
		private PKEY(Guid fmtid, uint pid)
		{
			this._fmtid = fmtid;
			this._pid = pid;
		}

		// Token: 0x04003A06 RID: 14854
		private readonly Guid _fmtid;

		// Token: 0x04003A07 RID: 14855
		private readonly uint _pid;

		// Token: 0x04003A08 RID: 14856
		public static readonly PKEY Title = new PKEY(new Guid("F29F85E0-4FF9-1068-AB91-08002B27B3D9"), 2U);

		// Token: 0x04003A09 RID: 14857
		public static readonly PKEY AppUserModel_ID = new PKEY(new Guid("9F4C2855-9F79-4B39-A8D0-E1D42DE1D5F3"), 5U);

		// Token: 0x04003A0A RID: 14858
		public static readonly PKEY AppUserModel_IsDestListSeparator = new PKEY(new Guid("9F4C2855-9F79-4B39-A8D0-E1D42DE1D5F3"), 6U);

		// Token: 0x04003A0B RID: 14859
		public static readonly PKEY AppUserModel_RelaunchCommand = new PKEY(new Guid("9F4C2855-9F79-4B39-A8D0-E1D42DE1D5F3"), 2U);

		// Token: 0x04003A0C RID: 14860
		public static readonly PKEY AppUserModel_RelaunchDisplayNameResource = new PKEY(new Guid("9F4C2855-9F79-4B39-A8D0-E1D42DE1D5F3"), 4U);

		// Token: 0x04003A0D RID: 14861
		public static readonly PKEY AppUserModel_RelaunchIconResource = new PKEY(new Guid("9F4C2855-9F79-4B39-A8D0-E1D42DE1D5F3"), 3U);
	}
}
