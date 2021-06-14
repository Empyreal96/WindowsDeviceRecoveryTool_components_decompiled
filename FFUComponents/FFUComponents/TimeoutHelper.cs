using System;
using System.Diagnostics;

namespace FFUComponents
{
	// Token: 0x02000050 RID: 80
	internal class TimeoutHelper
	{
		// Token: 0x06000154 RID: 340 RVA: 0x000077EE File Offset: 0x000059EE
		public TimeoutHelper(int timeoutMilliseconds) : this(TimeSpan.FromMilliseconds((double)timeoutMilliseconds))
		{
		}

		// Token: 0x06000155 RID: 341 RVA: 0x000077FD File Offset: 0x000059FD
		public TimeoutHelper(TimeSpan timeout)
		{
			this.timeout = timeout;
			this.stopWatch = Stopwatch.StartNew();
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000156 RID: 342 RVA: 0x00007817 File Offset: 0x00005A17
		public bool HasExpired
		{
			get
			{
				return this.stopWatch.Elapsed > this.timeout;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000157 RID: 343 RVA: 0x0000782F File Offset: 0x00005A2F
		public TimeSpan Remaining
		{
			get
			{
				return this.timeout - this.stopWatch.Elapsed;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000158 RID: 344 RVA: 0x00007847 File Offset: 0x00005A47
		public TimeSpan Elapsed
		{
			get
			{
				return this.stopWatch.Elapsed;
			}
		}

		// Token: 0x0400015F RID: 351
		private TimeSpan timeout;

		// Token: 0x04000160 RID: 352
		private Stopwatch stopWatch;
	}
}
