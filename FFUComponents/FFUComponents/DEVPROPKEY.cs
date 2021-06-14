using System;
using System.Runtime.InteropServices;

namespace FFUComponents
{
	// Token: 0x02000043 RID: 67
	[StructLayout(LayoutKind.Sequential)]
	public class DEVPROPKEY
	{
		// Token: 0x060000C2 RID: 194 RVA: 0x0000431A File Offset: 0x0000251A
		public DEVPROPKEY(Guid a_fmtid, uint a_pid)
		{
			this.fmtid = a_fmtid;
			this.pid = a_pid;
		}

		// Token: 0x040000EB RID: 235
		public Guid fmtid;

		// Token: 0x040000EC RID: 236
		public uint pid;
	}
}
