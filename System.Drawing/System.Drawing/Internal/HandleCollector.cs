using System;
using System.Threading;

namespace System.Internal
{
	// Token: 0x020000EE RID: 238
	internal sealed class HandleCollector
	{
		// Token: 0x14000009 RID: 9
		// (add) Token: 0x06000C5A RID: 3162 RVA: 0x0002BC58 File Offset: 0x00029E58
		// (remove) Token: 0x06000C5B RID: 3163 RVA: 0x0002BC8C File Offset: 0x00029E8C
		internal static event HandleChangeEventHandler HandleAdded;

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x06000C5C RID: 3164 RVA: 0x0002BCC0 File Offset: 0x00029EC0
		// (remove) Token: 0x06000C5D RID: 3165 RVA: 0x0002BCF4 File Offset: 0x00029EF4
		internal static event HandleChangeEventHandler HandleRemoved;

		// Token: 0x06000C5E RID: 3166 RVA: 0x0002BD27 File Offset: 0x00029F27
		internal static IntPtr Add(IntPtr handle, int type)
		{
			HandleCollector.handleTypes[type - 1].Add(handle);
			return handle;
		}

		// Token: 0x06000C5F RID: 3167 RVA: 0x0002BD3C File Offset: 0x00029F3C
		internal static void SuspendCollect()
		{
			object obj = HandleCollector.internalSyncObject;
			lock (obj)
			{
				HandleCollector.suspendCount++;
			}
		}

		// Token: 0x06000C60 RID: 3168 RVA: 0x0002BD84 File Offset: 0x00029F84
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

		// Token: 0x06000C61 RID: 3169 RVA: 0x0002BE34 File Offset: 0x0002A034
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

		// Token: 0x06000C62 RID: 3170 RVA: 0x0002BED4 File Offset: 0x0002A0D4
		internal static IntPtr Remove(IntPtr handle, int type)
		{
			return HandleCollector.handleTypes[type - 1].Remove(handle);
		}

		// Token: 0x04000AD3 RID: 2771
		private static HandleCollector.HandleType[] handleTypes;

		// Token: 0x04000AD4 RID: 2772
		private static int handleTypeCount;

		// Token: 0x04000AD5 RID: 2773
		private static int suspendCount;

		// Token: 0x04000AD8 RID: 2776
		private static object internalSyncObject = new object();

		// Token: 0x02000131 RID: 305
		private class HandleType
		{
			// Token: 0x06000FA4 RID: 4004 RVA: 0x0002E168 File Offset: 0x0002C368
			internal HandleType(string name, int expense, int initialThreshHold)
			{
				this.name = name;
				this.initialThreshHold = initialThreshHold;
				this.threshHold = initialThreshHold;
				this.deltaPercent = 100 - expense;
			}

			// Token: 0x06000FA5 RID: 4005 RVA: 0x0002E190 File Offset: 0x0002C390
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

			// Token: 0x06000FA6 RID: 4006 RVA: 0x0002E25C File Offset: 0x0002C45C
			internal int GetHandleCount()
			{
				int result;
				lock (this)
				{
					result = this.handleCount;
				}
				return result;
			}

			// Token: 0x06000FA7 RID: 4007 RVA: 0x0002E29C File Offset: 0x0002C49C
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

			// Token: 0x06000FA8 RID: 4008 RVA: 0x0002E318 File Offset: 0x0002C518
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

			// Token: 0x04000CD8 RID: 3288
			internal readonly string name;

			// Token: 0x04000CD9 RID: 3289
			private int initialThreshHold;

			// Token: 0x04000CDA RID: 3290
			private int threshHold;

			// Token: 0x04000CDB RID: 3291
			private int handleCount;

			// Token: 0x04000CDC RID: 3292
			private readonly int deltaPercent;
		}
	}
}
