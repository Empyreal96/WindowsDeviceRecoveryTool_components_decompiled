using System;

namespace ComponentAce.Compression.Libs.PPMd
{
	// Token: 0x02000054 RID: 84
	internal struct Pointer
	{
		// Token: 0x06000377 RID: 887 RVA: 0x0001D618 File Offset: 0x0001C618
		public Pointer(uint address)
		{
			this.Address = address;
		}

		// Token: 0x17000080 RID: 128
		public byte this[int offset]
		{
			get
			{
				return Pointer.Memory[(int)(checked((IntPtr)(unchecked((ulong)this.Address + (ulong)((long)offset)))))];
			}
			set
			{
				Pointer.Memory[(int)(checked((IntPtr)(unchecked((ulong)this.Address + (ulong)((long)offset)))))] = value;
			}
		}

		// Token: 0x0600037A RID: 890 RVA: 0x0001D648 File Offset: 0x0001C648
		public static implicit operator Pointer(MemoryNode memoryNode)
		{
			return new Pointer(memoryNode.Address);
		}

		// Token: 0x0600037B RID: 891 RVA: 0x0001D656 File Offset: 0x0001C656
		public static implicit operator Pointer(Model.PpmContext context)
		{
			return new Pointer(context.Address);
		}

		// Token: 0x0600037C RID: 892 RVA: 0x0001D664 File Offset: 0x0001C664
		public static implicit operator Pointer(PpmState state)
		{
			return new Pointer(state.Address);
		}

		// Token: 0x0600037D RID: 893 RVA: 0x0001D672 File Offset: 0x0001C672
		public static Pointer operator +(Pointer pointer, int offset)
		{
			pointer.Address = (uint)((ulong)pointer.Address + (ulong)((long)offset));
			return pointer;
		}

		// Token: 0x0600037E RID: 894 RVA: 0x0001D688 File Offset: 0x0001C688
		public static Pointer operator +(Pointer pointer, uint offset)
		{
			pointer.Address += offset;
			return pointer;
		}

		// Token: 0x0600037F RID: 895 RVA: 0x0001D69A File Offset: 0x0001C69A
		public static Pointer operator ++(Pointer pointer)
		{
			pointer.Address += 1U;
			return pointer;
		}

		// Token: 0x06000380 RID: 896 RVA: 0x0001D6AC File Offset: 0x0001C6AC
		public static Pointer operator -(Pointer pointer, int offset)
		{
			pointer.Address = (uint)((ulong)pointer.Address - (ulong)((long)offset));
			return pointer;
		}

		// Token: 0x06000381 RID: 897 RVA: 0x0001D6C2 File Offset: 0x0001C6C2
		public static Pointer operator -(Pointer pointer, uint offset)
		{
			pointer.Address -= offset;
			return pointer;
		}

		// Token: 0x06000382 RID: 898 RVA: 0x0001D6D4 File Offset: 0x0001C6D4
		public static Pointer operator --(Pointer pointer)
		{
			pointer.Address -= 1U;
			return pointer;
		}

		// Token: 0x06000383 RID: 899 RVA: 0x0001D6E6 File Offset: 0x0001C6E6
		public static uint operator -(Pointer pointer1, Pointer pointer2)
		{
			return pointer1.Address - pointer2.Address;
		}

		// Token: 0x06000384 RID: 900 RVA: 0x0001D6F7 File Offset: 0x0001C6F7
		public static bool operator <(Pointer pointer1, Pointer pointer2)
		{
			return pointer1.Address < pointer2.Address;
		}

		// Token: 0x06000385 RID: 901 RVA: 0x0001D709 File Offset: 0x0001C709
		public static bool operator <=(Pointer pointer1, Pointer pointer2)
		{
			return pointer1.Address <= pointer2.Address;
		}

		// Token: 0x06000386 RID: 902 RVA: 0x0001D71E File Offset: 0x0001C71E
		public static bool operator >(Pointer pointer1, Pointer pointer2)
		{
			return pointer1.Address > pointer2.Address;
		}

		// Token: 0x06000387 RID: 903 RVA: 0x0001D730 File Offset: 0x0001C730
		public static bool operator >=(Pointer pointer1, Pointer pointer2)
		{
			return pointer1.Address >= pointer2.Address;
		}

		// Token: 0x06000388 RID: 904 RVA: 0x0001D745 File Offset: 0x0001C745
		public static bool operator ==(Pointer pointer1, Pointer pointer2)
		{
			return pointer1.Address == pointer2.Address;
		}

		// Token: 0x06000389 RID: 905 RVA: 0x0001D757 File Offset: 0x0001C757
		public static bool operator !=(Pointer pointer1, Pointer pointer2)
		{
			return pointer1.Address != pointer2.Address;
		}

		// Token: 0x0600038A RID: 906 RVA: 0x0001D76C File Offset: 0x0001C76C
		public override bool Equals(object obj)
		{
			if (obj is Pointer)
			{
				return ((Pointer)obj).Address == this.Address;
			}
			return base.Equals(obj);
		}

		// Token: 0x0600038B RID: 907 RVA: 0x0001D7A9 File Offset: 0x0001C7A9
		public override int GetHashCode()
		{
			return this.Address.GetHashCode();
		}

		// Token: 0x04000266 RID: 614
		public const int Size = 1;

		// Token: 0x04000267 RID: 615
		public uint Address;

		// Token: 0x04000268 RID: 616
		public static byte[] Memory;

		// Token: 0x04000269 RID: 617
		public static readonly Pointer Zero = new Pointer(0U);
	}
}
