using System;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Threading;
using Nokia.Lucid.Properties;

namespace Nokia.Lucid.DeviceDetection.Primitives
{
	// Token: 0x02000008 RID: 8
	internal static class CriticalRegion
	{
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600001A RID: 26 RVA: 0x00002D15 File Offset: 0x00000F15
		public static bool IsRegionCritical
		{
			get
			{
				return CriticalRegion.level > 0;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600001B RID: 27 RVA: 0x00002D1F File Offset: 0x00000F1F
		internal static int Level
		{
			get
			{
				return CriticalRegion.level;
			}
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002D28 File Offset: 0x00000F28
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public static void BeginCriticalRegion(ref bool success)
		{
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
			}
			finally
			{
				if (CriticalRegion.level == 0)
				{
					Thread.BeginCriticalRegion();
				}
				CriticalRegion.level++;
				success = true;
			}
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002D68 File Offset: 0x00000F68
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public static void EndCriticalRegion()
		{
			if (CriticalRegion.level <= 0)
			{
				throw new InvalidOperationException(Resources.InvalidOperationException_MessageText_CoundNotEndCriticalRegion);
			}
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
			}
			finally
			{
				if (CriticalRegion.level == 1)
				{
					Thread.EndCriticalRegion();
				}
				CriticalRegion.level--;
			}
		}

		// Token: 0x04000012 RID: 18
		[ThreadStatic]
		private static int level;
	}
}
