using System;

namespace ComponentAce.Compression.Libs.PPMd
{
	// Token: 0x0200004F RID: 79
	internal struct MemoryNode
	{
		// Token: 0x06000325 RID: 805 RVA: 0x0001A27B File Offset: 0x0001927B
		public MemoryNode(uint address)
		{
			this.Address = address;
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000326 RID: 806 RVA: 0x0001A284 File Offset: 0x00019284
		// (set) Token: 0x06000327 RID: 807 RVA: 0x0001A2D8 File Offset: 0x000192D8
		public uint Stamp
		{
			get
			{
				return (uint)((int)MemoryNode.Memory[(int)((UIntPtr)this.Address)] | (int)MemoryNode.Memory[(int)((UIntPtr)(this.Address + 1U))] << 8 | (int)MemoryNode.Memory[(int)((UIntPtr)(this.Address + 2U))] << 16 | (int)MemoryNode.Memory[(int)((UIntPtr)(this.Address + 3U))] << 24);
			}
			set
			{
				MemoryNode.Memory[(int)((UIntPtr)this.Address)] = (byte)value;
				MemoryNode.Memory[(int)((UIntPtr)(this.Address + 1U))] = (byte)(value >> 8);
				MemoryNode.Memory[(int)((UIntPtr)(this.Address + 2U))] = (byte)(value >> 16);
				MemoryNode.Memory[(int)((UIntPtr)(this.Address + 3U))] = (byte)(value >> 24);
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000328 RID: 808 RVA: 0x0001A330 File Offset: 0x00019330
		// (set) Token: 0x06000329 RID: 809 RVA: 0x0001A38C File Offset: 0x0001938C
		public MemoryNode Next
		{
			get
			{
				return new MemoryNode((uint)((int)MemoryNode.Memory[(int)((UIntPtr)(this.Address + 4U))] | (int)MemoryNode.Memory[(int)((UIntPtr)(this.Address + 5U))] << 8 | (int)MemoryNode.Memory[(int)((UIntPtr)(this.Address + 6U))] << 16 | (int)MemoryNode.Memory[(int)((UIntPtr)(this.Address + 7U))] << 24));
			}
			set
			{
				MemoryNode.Memory[(int)((UIntPtr)(this.Address + 4U))] = (byte)value.Address;
				MemoryNode.Memory[(int)((UIntPtr)(this.Address + 5U))] = (byte)(value.Address >> 8);
				MemoryNode.Memory[(int)((UIntPtr)(this.Address + 6U))] = (byte)(value.Address >> 16);
				MemoryNode.Memory[(int)((UIntPtr)(this.Address + 7U))] = (byte)(value.Address >> 24);
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x0600032A RID: 810 RVA: 0x0001A400 File Offset: 0x00019400
		// (set) Token: 0x0600032B RID: 811 RVA: 0x0001A458 File Offset: 0x00019458
		public uint UnitCount
		{
			get
			{
				return (uint)((int)MemoryNode.Memory[(int)((UIntPtr)(this.Address + 8U))] | (int)MemoryNode.Memory[(int)((UIntPtr)(this.Address + 9U))] << 8 | (int)MemoryNode.Memory[(int)((UIntPtr)(this.Address + 10U))] << 16 | (int)MemoryNode.Memory[(int)((UIntPtr)(this.Address + 11U))] << 24);
			}
			set
			{
				MemoryNode.Memory[(int)((UIntPtr)(this.Address + 8U))] = (byte)value;
				MemoryNode.Memory[(int)((UIntPtr)(this.Address + 9U))] = (byte)(value >> 8);
				MemoryNode.Memory[(int)((UIntPtr)(this.Address + 10U))] = (byte)(value >> 16);
				MemoryNode.Memory[(int)((UIntPtr)(this.Address + 11U))] = (byte)(value >> 24);
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x0600032C RID: 812 RVA: 0x0001A4B4 File Offset: 0x000194B4
		public bool Available
		{
			get
			{
				return this.Next.Address != 0U;
			}
		}

		// Token: 0x0600032D RID: 813 RVA: 0x0001A4C7 File Offset: 0x000194C7
		public void Link(MemoryNode memoryNode)
		{
			memoryNode.Next = this.Next;
			this.Next = memoryNode;
		}

		// Token: 0x0600032E RID: 814 RVA: 0x0001A4E0 File Offset: 0x000194E0
		public void Unlink()
		{
			this.Next = this.Next.Next;
		}

		// Token: 0x0600032F RID: 815 RVA: 0x0001A501 File Offset: 0x00019501
		public void Insert(MemoryNode memoryNode, uint unitCount)
		{
			this.Link(memoryNode);
			memoryNode.Stamp = uint.MaxValue;
			memoryNode.UnitCount = unitCount;
			this.Stamp += 1U;
		}

		// Token: 0x06000330 RID: 816 RVA: 0x0001A528 File Offset: 0x00019528
		public MemoryNode Remove()
		{
			MemoryNode next = this.Next;
			this.Unlink();
			this.Stamp -= 1U;
			return next;
		}

		// Token: 0x06000331 RID: 817 RVA: 0x0001A551 File Offset: 0x00019551
		public static implicit operator MemoryNode(Pointer pointer)
		{
			return new MemoryNode(pointer.Address);
		}

		// Token: 0x06000332 RID: 818 RVA: 0x0001A55F File Offset: 0x0001955F
		public static MemoryNode operator +(MemoryNode memoryNode, int offset)
		{
			memoryNode.Address = (uint)((ulong)memoryNode.Address + (ulong)((long)(offset * 12)));
			return memoryNode;
		}

		// Token: 0x06000333 RID: 819 RVA: 0x0001A578 File Offset: 0x00019578
		public static MemoryNode operator +(MemoryNode memoryNode, uint offset)
		{
			memoryNode.Address += offset * 12U;
			return memoryNode;
		}

		// Token: 0x06000334 RID: 820 RVA: 0x0001A58D File Offset: 0x0001958D
		public static MemoryNode operator -(MemoryNode memoryNode, int offset)
		{
			memoryNode.Address = (uint)((ulong)memoryNode.Address - (ulong)((long)(offset * 12)));
			return memoryNode;
		}

		// Token: 0x06000335 RID: 821 RVA: 0x0001A5A6 File Offset: 0x000195A6
		public static MemoryNode operator -(MemoryNode memoryNode, uint offset)
		{
			memoryNode.Address -= offset * 12U;
			return memoryNode;
		}

		// Token: 0x06000336 RID: 822 RVA: 0x0001A5BB File Offset: 0x000195BB
		public static bool operator ==(MemoryNode memoryNode1, MemoryNode memoryNode2)
		{
			return memoryNode1.Address == memoryNode2.Address;
		}

		// Token: 0x06000337 RID: 823 RVA: 0x0001A5CD File Offset: 0x000195CD
		public static bool operator !=(MemoryNode memoryNode1, MemoryNode memoryNode2)
		{
			return memoryNode1.Address != memoryNode2.Address;
		}

		// Token: 0x06000338 RID: 824 RVA: 0x0001A5E4 File Offset: 0x000195E4
		public override bool Equals(object obj)
		{
			if (obj is MemoryNode)
			{
				return ((MemoryNode)obj).Address == this.Address;
			}
			return base.Equals(obj);
		}

		// Token: 0x06000339 RID: 825 RVA: 0x0001A621 File Offset: 0x00019621
		public override int GetHashCode()
		{
			return this.Address.GetHashCode();
		}

		// Token: 0x0400022F RID: 559
		public const int Size = 12;

		// Token: 0x04000230 RID: 560
		public uint Address;

		// Token: 0x04000231 RID: 561
		public static byte[] Memory;

		// Token: 0x04000232 RID: 562
		public static readonly MemoryNode Zero = new MemoryNode(0U);
	}
}
