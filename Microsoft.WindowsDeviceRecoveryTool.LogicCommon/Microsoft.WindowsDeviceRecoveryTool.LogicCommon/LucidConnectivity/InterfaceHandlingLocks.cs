using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.WindowsDeviceRecoveryTool.Common.Tracing;

namespace Microsoft.WindowsDeviceRecoveryTool.LogicCommon.LucidConnectivity
{
	// Token: 0x02000014 RID: 20
	internal class InterfaceHandlingLocks
	{
		// Token: 0x06000099 RID: 153 RVA: 0x00003631 File Offset: 0x00001831
		public InterfaceHandlingLocks()
		{
			this.syncObject = new object();
			this.locks = new Dictionary<string, ManualResetEventSlim>();
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00003654 File Offset: 0x00001854
		public void CreateLock(string id)
		{
			lock (this.syncObject)
			{
				id = this.ConvertId(id);
				if (this.locks.ContainsKey(id))
				{
					this.locks[id] = new ManualResetEventSlim(true);
				}
				else
				{
					this.locks.Add(id, new ManualResetEventSlim(true));
				}
				Tracer<InterfaceHandlingLocks>.WriteInformation("*** CREATE_LOCK: Thread {0} created interface lock for '{1}' ***", new object[]
				{
					Thread.CurrentThread.ManagedThreadId.ToString("X4"),
					id
				});
			}
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00003714 File Offset: 0x00001914
		public bool Lock(string id)
		{
			bool result;
			lock (this.syncObject)
			{
				id = this.ConvertId(id);
				Tracer<InterfaceHandlingLocks>.WriteInformation("*** LOCK: Interface handling for '{0}' locked by thread {1} ***", new object[]
				{
					id,
					Thread.CurrentThread.ManagedThreadId.ToString("X4")
				});
				if (this.locks.ContainsKey(id))
				{
					this.locks[id].Reset();
					result = true;
				}
				else
				{
					result = false;
				}
			}
			return result;
		}

		// Token: 0x0600009C RID: 156 RVA: 0x000037C4 File Offset: 0x000019C4
		public bool Unlock(string id)
		{
			bool result;
			lock (this.syncObject)
			{
				id = this.ConvertId(id);
				Tracer<InterfaceHandlingLocks>.WriteInformation("*** UNLOCK: Interface handling for '{0}' unlocked by thread {1} ***", new object[]
				{
					id,
					Thread.CurrentThread.ManagedThreadId.ToString("X4")
				});
				if (this.locks.ContainsKey(id))
				{
					this.locks[id].Set();
					result = true;
				}
				else
				{
					result = false;
				}
			}
			return result;
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00003874 File Offset: 0x00001A74
		public void Discard(string id)
		{
			lock (this.syncObject)
			{
				id = this.ConvertId(id);
				if (this.locks.ContainsKey(id))
				{
					this.locks.Remove(id);
					Tracer<InterfaceHandlingLocks>.WriteInformation("*** DISCARD_LOCK: Lock '{0}' discarded by thread {1} ***", new object[]
					{
						id,
						Thread.CurrentThread.ManagedThreadId.ToString("X4")
					});
				}
				else
				{
					Tracer<InterfaceHandlingLocks>.WriteWarning("*** DISCARD_LOCK: Lock '{0}' could not be discarded by thread {1} ***", new object[]
					{
						id,
						Thread.CurrentThread.ManagedThreadId.ToString("X4")
					});
				}
			}
		}

		// Token: 0x0600009E RID: 158 RVA: 0x0000394C File Offset: 0x00001B4C
		public bool Wait(string id, int timeoutMs)
		{
			id = this.ConvertId(id);
			Tracer<InterfaceHandlingLocks>.WriteInformation("*** WAIT: Thread {0} is waiting for unlocking of '{1}' ***", new object[]
			{
				Thread.CurrentThread.ManagedThreadId.ToString("X4"),
				id
			});
			bool result;
			if (this.locks.ContainsKey(id))
			{
				bool flag = this.locks[id].Wait(timeoutMs);
				if (flag)
				{
					Tracer<InterfaceHandlingLocks>.WriteInformation("*** SIGNAL: Thread {0} is allowed to continue handling interface(s) for '{1}' ***", new object[]
					{
						Thread.CurrentThread.ManagedThreadId.ToString("X4"),
						id
					});
				}
				else
				{
					Tracer<InterfaceHandlingLocks>.WriteWarning("*** NO_SIGNAL: Waiting for unlocking of '{0}' timed out ***", new object[]
					{
						id
					});
				}
				result = flag;
			}
			else
			{
				Tracer<InterfaceHandlingLocks>.WriteWarning("*** NO_LOCK: No interface lock found for '{0}' ***", new object[]
				{
					id
				});
				result = false;
			}
			return result;
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00003A38 File Offset: 0x00001C38
		private string ConvertId(string id)
		{
			return id.ToUpperInvariant();
		}

		// Token: 0x04000061 RID: 97
		private readonly object syncObject;

		// Token: 0x04000062 RID: 98
		private readonly Dictionary<string, ManualResetEventSlim> locks;
	}
}
