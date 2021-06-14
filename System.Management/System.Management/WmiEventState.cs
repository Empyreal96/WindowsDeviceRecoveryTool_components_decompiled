using System;
using System.Threading;

namespace System.Management
{
	// Token: 0x02000026 RID: 38
	internal class WmiEventState
	{
		// Token: 0x06000131 RID: 305 RVA: 0x00007EA8 File Offset: 0x000060A8
		internal WmiEventState(Delegate d, ManagementEventArgs args, AutoResetEvent h)
		{
			this.d = d;
			this.args = args;
			this.h = h;
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000132 RID: 306 RVA: 0x00007EC5 File Offset: 0x000060C5
		public Delegate Delegate
		{
			get
			{
				return this.d;
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000133 RID: 307 RVA: 0x00007ECD File Offset: 0x000060CD
		public ManagementEventArgs Args
		{
			get
			{
				return this.args;
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000134 RID: 308 RVA: 0x00007ED5 File Offset: 0x000060D5
		public AutoResetEvent AutoResetEvent
		{
			get
			{
				return this.h;
			}
		}

		// Token: 0x0400011D RID: 285
		private Delegate d;

		// Token: 0x0400011E RID: 286
		private ManagementEventArgs args;

		// Token: 0x0400011F RID: 287
		private AutoResetEvent h;
	}
}
