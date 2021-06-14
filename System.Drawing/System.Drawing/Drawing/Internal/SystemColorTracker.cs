using System;
using Microsoft.Win32;

namespace System.Drawing.Internal
{
	// Token: 0x020000EB RID: 235
	internal class SystemColorTracker
	{
		// Token: 0x06000C51 RID: 3153 RVA: 0x00003800 File Offset: 0x00001A00
		private SystemColorTracker()
		{
		}

		// Token: 0x06000C52 RID: 3154 RVA: 0x0002B9EC File Offset: 0x00029BEC
		internal static void Add(ISystemColorTracker obj)
		{
			Type typeFromHandle = typeof(SystemColorTracker);
			lock (typeFromHandle)
			{
				if (SystemColorTracker.list.Length == SystemColorTracker.count)
				{
					SystemColorTracker.GarbageCollectList();
				}
				if (!SystemColorTracker.addedTracker)
				{
					SystemColorTracker.addedTracker = true;
					SystemEvents.UserPreferenceChanged += SystemColorTracker.OnUserPreferenceChanged;
				}
				int num = SystemColorTracker.count;
				SystemColorTracker.count++;
				if (SystemColorTracker.list[num] == null)
				{
					SystemColorTracker.list[num] = new WeakReference(obj);
				}
				else
				{
					SystemColorTracker.list[num].Target = obj;
				}
			}
		}

		// Token: 0x06000C53 RID: 3155 RVA: 0x0002BA94 File Offset: 0x00029C94
		private static void CleanOutBrokenLinks()
		{
			int num = SystemColorTracker.list.Length - 1;
			int num2 = 0;
			int num3 = SystemColorTracker.list.Length;
			for (;;)
			{
				if (num2 < num3)
				{
					if (SystemColorTracker.list[num2].Target != null)
					{
						num2++;
						continue;
					}
				}
				while (num >= 0 && SystemColorTracker.list[num].Target == null)
				{
					num--;
				}
				if (num2 >= num)
				{
					break;
				}
				WeakReference weakReference = SystemColorTracker.list[num2];
				SystemColorTracker.list[num2] = SystemColorTracker.list[num];
				SystemColorTracker.list[num] = weakReference;
				num2++;
				num--;
			}
			SystemColorTracker.count = num2;
		}

		// Token: 0x06000C54 RID: 3156 RVA: 0x0002BB18 File Offset: 0x00029D18
		private static void GarbageCollectList()
		{
			SystemColorTracker.CleanOutBrokenLinks();
			if ((float)SystemColorTracker.count / (float)SystemColorTracker.list.Length > SystemColorTracker.EXPAND_THRESHOLD)
			{
				WeakReference[] array = new WeakReference[SystemColorTracker.list.Length * SystemColorTracker.EXPAND_FACTOR];
				SystemColorTracker.list.CopyTo(array, 0);
				SystemColorTracker.list = array;
				int num = SystemColorTracker.list.Length;
				int warning_SIZE = SystemColorTracker.WARNING_SIZE;
			}
		}

		// Token: 0x06000C55 RID: 3157 RVA: 0x0002BB74 File Offset: 0x00029D74
		private static void OnUserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
		{
			if (e.Category == UserPreferenceCategory.Color)
			{
				for (int i = 0; i < SystemColorTracker.count; i++)
				{
					ISystemColorTracker systemColorTracker = (ISystemColorTracker)SystemColorTracker.list[i].Target;
					if (systemColorTracker != null)
					{
						systemColorTracker.OnSystemColorChanged();
					}
				}
			}
		}

		// Token: 0x04000AC9 RID: 2761
		private static int INITIAL_SIZE = 200;

		// Token: 0x04000ACA RID: 2762
		private static int WARNING_SIZE = 100000;

		// Token: 0x04000ACB RID: 2763
		private static float EXPAND_THRESHOLD = 0.75f;

		// Token: 0x04000ACC RID: 2764
		private static int EXPAND_FACTOR = 2;

		// Token: 0x04000ACD RID: 2765
		private static WeakReference[] list = new WeakReference[SystemColorTracker.INITIAL_SIZE];

		// Token: 0x04000ACE RID: 2766
		private static int count = 0;

		// Token: 0x04000ACF RID: 2767
		private static bool addedTracker;
	}
}
