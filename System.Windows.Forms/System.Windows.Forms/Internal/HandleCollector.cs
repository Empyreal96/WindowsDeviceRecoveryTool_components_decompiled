using System;
using System.Threading;

namespace System.Internal
{
	// Token: 0x020000F9 RID: 249
	internal sealed class HandleCollector
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x060003E4 RID: 996 RVA: 0x0000C430 File Offset: 0x0000A630
		// (remove) Token: 0x060003E5 RID: 997 RVA: 0x0000C464 File Offset: 0x0000A664
		internal static event HandleChangeEventHandler HandleAdded;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x060003E6 RID: 998 RVA: 0x0000C498 File Offset: 0x0000A698
		// (remove) Token: 0x060003E7 RID: 999 RVA: 0x0000C4CC File Offset: 0x0000A6CC
		internal static event HandleChangeEventHandler HandleRemoved;

		// Token: 0x060003E8 RID: 1000 RVA: 0x0000C4FF File Offset: 0x0000A6FF
		internal static IntPtr Add(IntPtr handle, int type)
		{
			HandleCollector.handleTypes[type - 1].Add(handle);
			return handle;
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x0000C514 File Offset: 0x0000A714
		internal static void SuspendCollect()
		{
			object obj = HandleCollector.internalSyncObject;
			lock (obj)
			{
				HandleCollector.suspendCount++;
			}
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x0000C55C File Offset: 0x0000A75C
		internal static void ResumeCollect()
		{
			bool flag = false;
			object obj = HandleCollector.internalSyncObject;
			lock (obj)
			{
				if (HandleCollector.suspendCount > 0)
				{
					HandleCollector.suspendCount--;
				}
				if (HandleCollector.suspendCount == 0)
				{
					for (int i = 0; i < HandleCollector.handleTypeCount; i++)
					{
						HandleCollector.HandleType obj2 = HandleCollector.handleTypes[i];
						lock (obj2)
						{
							if (HandleCollector.handleTypes[i].NeedCollection())
							{
								flag = true;
							}
						}
					}
				}
			}
			if (flag)
			{
				GC.Collect();
			}
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x0000C60C File Offset: 0x0000A80C
		internal static int RegisterType(string typeName, int expense, int initialThreshold)
		{
			object obj = HandleCollector.internalSyncObject;
			int result;
			lock (obj)
			{
				if (HandleCollector.handleTypeCount == 0 || HandleCollector.handleTypeCount == HandleCollector.handleTypes.Length)
				{
					HandleCollector.HandleType[] destinationArray = new HandleCollector.HandleType[HandleCollector.handleTypeCount + 10];
					if (HandleCollector.handleTypes != null)
					{
						Array.Copy(HandleCollector.handleTypes, 0, destinationArray, 0, HandleCollector.handleTypeCount);
					}
					HandleCollector.handleTypes = destinationArray;
				}
				HandleCollector.handleTypes[HandleCollector.handleTypeCount++] = new HandleCollector.HandleType(typeName, expense, initialThreshold);
				result = HandleCollector.handleTypeCount;
			}
			return result;
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x0000C6AC File Offset: 0x0000A8AC
		internal static IntPtr Remove(IntPtr handle, int type)
		{
			return HandleCollector.handleTypes[type - 1].Remove(handle);
		}

		// Token: 0x04000425 RID: 1061
		private static HandleCollector.HandleType[] handleTypes;

		// Token: 0x04000426 RID: 1062
		private static int handleTypeCount;

		// Token: 0x04000427 RID: 1063
		private static int suspendCount;

		// Token: 0x0400042A RID: 1066
		private static object internalSyncObject = new object();

		// Token: 0x0200053D RID: 1341
		private class HandleType
		{
			// Token: 0x060054BF RID: 21695 RVA: 0x00163DCC File Offset: 0x00161FCC
			internal HandleType(string name, int expense, int initialThreshHold)
			{
				this.name = name;
				this.initialThreshHold = initialThreshHold;
				this.threshHold = initialThreshHold;
				this.deltaPercent = 100 - expense;
			}

			// Token: 0x060054C0 RID: 21696 RVA: 0x00163DF4 File Offset: 0x00161FF4
			internal void Add(IntPtr handle)
			{
				if (handle == IntPtr.Zero)
				{
					return;
				}
				bool flag = false;
				int currentHandleCount = 0;
				lock (this)
				{
					this.handleCount++;
					flag = this.NeedCollection();
					currentHandleCount = this.handleCount;
				}
				object internalSyncObject = HandleCollector.internalSyncObject;
				lock (internalSyncObject)
				{
					if (HandleCollector.HandleAdded != null)
					{
						HandleCollector.HandleAdded(this.name, handle, currentHandleCount);
					}
				}
				if (!flag)
				{
					return;
				}
				if (flag)
				{
					GC.Collect();
					int millisecondsTimeout = (100 - this.deltaPercent) / 4;
					Thread.Sleep(millisecondsTimeout);
				}
			}

			// Token: 0x060054C1 RID: 21697 RVA: 0x00163EC0 File Offset: 0x001620C0
			internal int GetHandleCount()
			{
				int result;
				lock (this)
				{
					result = this.handleCount;
				}
				return result;
			}

			// Token: 0x060054C2 RID: 21698 RVA: 0x00163F00 File Offset: 0x00162100
			internal bool NeedCollection()
			{
				if (HandleCollector.suspendCount > 0)
				{
					return false;
				}
				if (this.handleCount > this.threshHold)
				{
					this.threshHold = this.handleCount + this.handleCount * this.deltaPercent / 100;
					return true;
				}
				int num = 100 * this.threshHold / (100 + this.deltaPercent);
				if (num >= this.initialThreshHold && this.handleCount < (int)((float)num * 0.9f))
				{
					this.threshHold = num;
				}
				return false;
			}

			// Token: 0x060054C3 RID: 21699 RVA: 0x00163F7C File Offset: 0x0016217C
			internal IntPtr Remove(IntPtr handle)
			{
				if (handle == IntPtr.Zero)
				{
					return handle;
				}
				int currentHandleCount = 0;
				lock (this)
				{
					this.handleCount--;
					if (this.handleCount < 0)
					{
						this.handleCount = 0;
					}
					currentHandleCount = this.handleCount;
				}
				object internalSyncObject = HandleCollector.internalSyncObject;
				lock (internalSyncObject)
				{
					if (HandleCollector.HandleRemoved != null)
					{
						HandleCollector.HandleRemoved(this.name, handle, currentHandleCount);
					}
				}
				return handle;
			}

			// Token: 0x04003756 RID: 14166
			internal readonly string name;

			// Token: 0x04003757 RID: 14167
			private int initialThreshHold;

			// Token: 0x04003758 RID: 14168
			private int threshHold;

			// Token: 0x04003759 RID: 14169
			private int handleCount;

			// Token: 0x0400375A RID: 14170
			private readonly int deltaPercent;
		}
	}
}
