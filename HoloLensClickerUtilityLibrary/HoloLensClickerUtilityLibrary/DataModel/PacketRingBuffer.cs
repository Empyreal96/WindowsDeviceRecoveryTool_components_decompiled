using System;
using System.Diagnostics;
using ClickerUtilityLibrary.Comm;

namespace ClickerUtilityLibrary.DataModel
{
	// Token: 0x0200001C RID: 28
	internal class PacketRingBuffer
	{
		// Token: 0x14000004 RID: 4
		// (add) Token: 0x060000BC RID: 188 RVA: 0x00005B28 File Offset: 0x00003D28
		// (remove) Token: 0x060000BD RID: 189 RVA: 0x00005B60 File Offset: 0x00003D60
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal event EventHandler<PacketRingBufferEventArgs> PacketRingBufferEvent;

		// Token: 0x060000BE RID: 190 RVA: 0x00005B98 File Offset: 0x00003D98
		protected virtual void OnPacketRingBufferEvent(PacketRingBufferEventArgs eventArgs)
		{
			bool flag = this.PacketRingBufferEvent != null;
			if (flag)
			{
				this.PacketRingBufferEvent(this, eventArgs);
			}
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00005BC4 File Offset: 0x00003DC4
		public PacketRingBuffer(int size)
		{
			this.mSize = size;
			this.mPacketPool = new IPacket[this.mSize];
			int num;
			for (int i = 0; i < this.mSize; i = num + 1)
			{
				this.mPacketPool[i] = BootLoaderProtocol.Instance.CreateNewPacket();
				num = i;
			}
			this.mPoolLock = new object();
			this.mHead = 0;
			this.mTail = 0;
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000C0 RID: 192 RVA: 0x00005C38 File Offset: 0x00003E38
		public int Count
		{
			get
			{
				int num = this.mHead - this.mTail;
				bool flag = num < 0;
				int result;
				if (flag)
				{
					result = num + this.mSize;
				}
				else
				{
					result = num;
				}
				return result;
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000C1 RID: 193 RVA: 0x00005C70 File Offset: 0x00003E70
		public bool IsFull
		{
			get
			{
				return (this.mHead + 1) % this.mSize == this.mTail;
			}
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00005CA4 File Offset: 0x00003EA4
		public bool EnqueuePacket(IPacket packet)
		{
			bool flag = !this.IsFull;
			bool result;
			if (flag)
			{
				object obj = this.mPoolLock;
				lock (obj)
				{
					this.mPacketPool[this.mHead].Copy(packet);
					int num = this.mHead + 1;
					this.mHead = num;
					bool flag3 = this.mHead == this.mSize;
					if (flag3)
					{
						this.mHead = 0;
					}
				}
				PacketRingBufferEventArgs eventArgs = new PacketRingBufferEventArgs
				{
					Type = PacketRingBufferEventType.PacketEnqueued
				};
				this.OnPacketRingBufferEvent(eventArgs);
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00005D60 File Offset: 0x00003F60
		public IPacket DequeuePacket()
		{
			IPacket result = null;
			bool flag = this.Count > 0;
			if (flag)
			{
				object obj = this.mPoolLock;
				lock (obj)
				{
					result = this.mPacketPool[this.mTail];
					int num = this.mTail + 1;
					this.mTail = num;
					bool flag3 = this.mTail == this.mSize;
					if (flag3)
					{
						this.mTail = 0;
					}
				}
				PacketRingBufferEventArgs eventArgs = new PacketRingBufferEventArgs
				{
					Type = PacketRingBufferEventType.PacketDequeued
				};
				this.OnPacketRingBufferEvent(eventArgs);
			}
			return result;
		}

		// Token: 0x040000BE RID: 190
		private readonly int mSize;

		// Token: 0x040000BF RID: 191
		private readonly IPacket[] mPacketPool;

		// Token: 0x040000C0 RID: 192
		private int mHead;

		// Token: 0x040000C1 RID: 193
		private int mTail;

		// Token: 0x040000C2 RID: 194
		private readonly object mPoolLock;
	}
}
