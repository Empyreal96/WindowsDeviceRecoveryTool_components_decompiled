using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace MS.Internal.Ink
{
	// Token: 0x02000689 RID: 1673
	internal static class HighContrastHelper
	{
		// Token: 0x06006D77 RID: 28023 RVA: 0x001F6DFC File Offset: 0x001F4FFC
		internal static void RegisterHighContrastCallback(HighContrastCallback highContrastCallback)
		{
			object _lock = HighContrastHelper.__lock;
			lock (_lock)
			{
				int count = HighContrastHelper.__highContrastCallbackList.Count;
				int i = 0;
				int num = 0;
				if (HighContrastHelper.__increaseCount > 100)
				{
					while (i < count)
					{
						WeakReference weakReference = HighContrastHelper.__highContrastCallbackList[num];
						if (weakReference.IsAlive)
						{
							num++;
						}
						else
						{
							HighContrastHelper.__highContrastCallbackList.RemoveAt(num);
						}
						i++;
					}
					HighContrastHelper.__increaseCount = 0;
				}
				HighContrastHelper.__highContrastCallbackList.Add(new WeakReference(highContrastCallback));
				HighContrastHelper.__increaseCount++;
			}
		}

		// Token: 0x06006D78 RID: 28024 RVA: 0x001F6EA8 File Offset: 0x001F50A8
		internal static void OnSettingChanged()
		{
			HighContrastHelper.UpdateHighContrast();
		}

		// Token: 0x06006D79 RID: 28025 RVA: 0x001F6EB0 File Offset: 0x001F50B0
		private static void UpdateHighContrast()
		{
			object _lock = HighContrastHelper.__lock;
			lock (_lock)
			{
				int count = HighContrastHelper.__highContrastCallbackList.Count;
				int i = 0;
				int num = 0;
				while (i < count)
				{
					WeakReference weakReference = HighContrastHelper.__highContrastCallbackList[num];
					if (weakReference.IsAlive)
					{
						HighContrastCallback highContrastCallback = weakReference.Target as HighContrastCallback;
						if (highContrastCallback.Dispatcher != null)
						{
							highContrastCallback.Dispatcher.BeginInvoke(DispatcherPriority.Background, new HighContrastHelper.UpdateHighContrastCallback(HighContrastHelper.OnUpdateHighContrast), highContrastCallback);
						}
						else
						{
							HighContrastHelper.OnUpdateHighContrast(highContrastCallback);
						}
						num++;
					}
					else
					{
						HighContrastHelper.__highContrastCallbackList.RemoveAt(num);
					}
					i++;
				}
				HighContrastHelper.__increaseCount = 0;
			}
		}

		// Token: 0x06006D7A RID: 28026 RVA: 0x001F6F70 File Offset: 0x001F5170
		private static void OnUpdateHighContrast(HighContrastCallback highContrastCallback)
		{
			bool highContrast = SystemParameters.HighContrast;
			Color windowTextColor = SystemColors.WindowTextColor;
			if (highContrast)
			{
				highContrastCallback.TurnHighContrastOn(windowTextColor);
				return;
			}
			highContrastCallback.TurnHighContrastOff();
		}

		// Token: 0x040035EB RID: 13803
		private static object __lock = new object();

		// Token: 0x040035EC RID: 13804
		private static List<WeakReference> __highContrastCallbackList = new List<WeakReference>();

		// Token: 0x040035ED RID: 13805
		private static int __increaseCount = 0;

		// Token: 0x040035EE RID: 13806
		private const int CleanTolerance = 100;

		// Token: 0x02000B24 RID: 2852
		// (Invoke) Token: 0x06008D3B RID: 36155
		private delegate void UpdateHighContrastCallback(HighContrastCallback highContrastCallback);
	}
}
