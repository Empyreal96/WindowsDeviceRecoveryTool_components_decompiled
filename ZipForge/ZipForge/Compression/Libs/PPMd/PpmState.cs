using System;

namespace ComponentAce.Compression.Libs.PPMd
{
	// Token: 0x02000055 RID: 85
	internal struct PpmState
	{
		// Token: 0x0600038D RID: 909 RVA: 0x0001D7C3 File Offset: 0x0001C7C3
		public PpmState(uint address)
		{
			this.Address = address;
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x0600038E RID: 910 RVA: 0x0001D7CC File Offset: 0x0001C7CC
		// (set) Token: 0x0600038F RID: 911 RVA: 0x0001D7DB File Offset: 0x0001C7DB
		public byte Symbol
		{
			get
			{
				return PpmState.Memory[(int)((UIntPtr)this.Address)];
			}
			set
			{
				PpmState.Memory[(int)((UIntPtr)this.Address)] = value;
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000390 RID: 912 RVA: 0x0001D7EB File Offset: 0x0001C7EB
		// (set) Token: 0x06000391 RID: 913 RVA: 0x0001D7FC File Offset: 0x0001C7FC
		public byte Frequency
		{
			get
			{
				return PpmState.Memory[(int)((UIntPtr)(this.Address + 1U))];
			}
			set
			{
				PpmState.Memory[(int)((UIntPtr)(this.Address + 1U))] = value;
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000392 RID: 914 RVA: 0x0001D810 File Offset: 0x0001C810
		// (set) Token: 0x06000393 RID: 915 RVA: 0x0001D86C File Offset: 0x0001C86C
		public Model.PpmContext Successor
		{
			get
			{
				return new Model.PpmContext((uint)((int)PpmState.Memory[(int)((UIntPtr)(this.Address + 2U))] | (int)PpmState.Memory[(int)((UIntPtr)(this.Address + 3U))] << 8 | (int)PpmState.Memory[(int)((UIntPtr)(this.Address + 4U))] << 16 | (int)PpmState.Memory[(int)((UIntPtr)(this.Address + 5U))] << 24));
			}
			set
			{
				PpmState.Memory[(int)((UIntPtr)(this.Address + 2U))] = (byte)value.Address;
				PpmState.Memory[(int)((UIntPtr)(this.Address + 3U))] = (byte)(value.Address >> 8);
				PpmState.Memory[(int)((UIntPtr)(this.Address + 4U))] = (byte)(value.Address >> 16);
				PpmState.Memory[(int)((UIntPtr)(this.Address + 5U))] = (byte)(value.Address >> 24);
			}
		}

		// Token: 0x17000084 RID: 132
		public PpmState this[int offset]
		{
			get
			{
				return new PpmState((uint)((ulong)this.Address + (ulong)((long)(offset * 6))));
			}
		}

		// Token: 0x06000395 RID: 917 RVA: 0x0001D8F1 File Offset: 0x0001C8F1
		public static implicit operator PpmState(Pointer pointer)
		{
			return new PpmState(pointer.Address);
		}

		// Token: 0x06000396 RID: 918 RVA: 0x0001D8FF File Offset: 0x0001C8FF
		public static PpmState operator +(PpmState state, int offset)
		{
			state.Address = (uint)((ulong)state.Address + (ulong)((long)(offset * 6)));
			return state;
		}

		// Token: 0x06000397 RID: 919 RVA: 0x0001D917 File Offset: 0x0001C917
		public static PpmState operator ++(PpmState state)
		{
			state.Address += 6U;
			return state;
		}

		// Token: 0x06000398 RID: 920 RVA: 0x0001D929 File Offset: 0x0001C929
		public static PpmState operator -(PpmState state, int offset)
		{
			state.Address = (uint)((ulong)state.Address - (ulong)((long)(offset * 6)));
			return state;
		}

		// Token: 0x06000399 RID: 921 RVA: 0x0001D941 File Offset: 0x0001C941
		public static PpmState operator --(PpmState state)
		{
			state.Address -= 6U;
			return state;
		}

		// Token: 0x0600039A RID: 922 RVA: 0x0001D953 File Offset: 0x0001C953
		public static bool operator <=(PpmState state1, PpmState state2)
		{
			return state1.Address <= state2.Address;
		}

		// Token: 0x0600039B RID: 923 RVA: 0x0001D968 File Offset: 0x0001C968
		public static bool operator >=(PpmState state1, PpmState state2)
		{
			return state1.Address >= state2.Address;
		}

		// Token: 0x0600039C RID: 924 RVA: 0x0001D97D File Offset: 0x0001C97D
		public static bool operator ==(PpmState state1, PpmState state2)
		{
			return state1.Address == state2.Address;
		}

		// Token: 0x0600039D RID: 925 RVA: 0x0001D98F File Offset: 0x0001C98F
		public static bool operator !=(PpmState state1, PpmState state2)
		{
			return state1.Address != state2.Address;
		}

		// Token: 0x0600039E RID: 926 RVA: 0x0001D9A4 File Offset: 0x0001C9A4
		public override bool Equals(object obj)
		{
			if (obj is PpmState)
			{
				return ((PpmState)obj).Address == this.Address;
			}
			return base.Equals(obj);
		}

		// Token: 0x0600039F RID: 927 RVA: 0x0001D9E1 File Offset: 0x0001C9E1
		public override int GetHashCode()
		{
			return this.Address.GetHashCode();
		}

		// Token: 0x0400026A RID: 618
		public const int Size = 6;

		// Token: 0x0400026B RID: 619
		public uint Address;

		// Token: 0x0400026C RID: 620
		public static byte[] Memory;

		// Token: 0x0400026D RID: 621
		public static readonly PpmState Zero = new PpmState(0U);
	}
}
