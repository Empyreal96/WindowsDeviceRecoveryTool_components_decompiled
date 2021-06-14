using System;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Threading;
using Nokia.Lucid.Properties;

namespace Nokia.Lucid.DeviceDetection.Primitives
{
	// Token: 0x02000009 RID: 9
	internal static class ThreadAffinity
	{
		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600001E RID: 30 RVA: 0x00002DBC File Offset: 0x00000FBC
		public static bool IsThreadAffine
		{
			get
			{
				return ThreadAffinity.level > 0;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600001F RID: 31 RVA: 0x00002DC6 File Offset: 0x00000FC6
		internal static int Level
		{
			get
			{
				return ThreadAffinity.level;
			}
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002DD0 File Offset: 0x00000FD0
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public static void BeginThreadAffinity(ref bool success)
		{
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
			}
			finally
			{
				if (ThreadAffinity.level == 0)
				{
					Thread.BeginThreadAffinity();
				}
				ThreadAffinity.level++;
				success = true;
			}
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002E10 File Offset: 0x00001010
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public static void EndThreadAffinity()
		{
			if (ThreadAffinity.level <= 0)
			{
				throw new InvalidOperationException(Resources.InvalidOperationException_MessageText_CouldNotEndThreadAffinity);
			}
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
			}
			finally
			{
				if (ThreadAffinity.level == 1)
				{
					Thread.EndThreadAffinity();
				}
				ThreadAffinity.level--;
			}
		}

		// Token: 0x04000013 RID: 19
		[ThreadStatic]
		private static int level;
	}
}
