using System;

namespace ComponentAce.Compression.Libs.ZLib
{
	// Token: 0x020000B6 RID: 182
	internal sealed class Tree
	{
		// Token: 0x1700015C RID: 348
		// (get) Token: 0x060007FA RID: 2042 RVA: 0x000349AB File Offset: 0x000339AB
		// (set) Token: 0x060007FB RID: 2043 RVA: 0x000349B3 File Offset: 0x000339B3
		public short[] DynTree
		{
			get
			{
				return this.dyn_tree;
			}
			set
			{
				this.dyn_tree = value;
			}
		}

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x060007FC RID: 2044 RVA: 0x000349BC File Offset: 0x000339BC
		// (set) Token: 0x060007FD RID: 2045 RVA: 0x000349C4 File Offset: 0x000339C4
		public int MaxCode
		{
			get
			{
				return this.max_code;
			}
			set
			{
				this.max_code = value;
			}
		}

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x060007FE RID: 2046 RVA: 0x000349CD File Offset: 0x000339CD
		// (set) Token: 0x060007FF RID: 2047 RVA: 0x000349D5 File Offset: 0x000339D5
		internal StaticTree StatDesc
		{
			get
			{
				return this.stat_desc;
			}
			set
			{
				this.stat_desc = value;
			}
		}

		// Token: 0x06000800 RID: 2048 RVA: 0x000349DE File Offset: 0x000339DE
		internal static int d_code(int dist)
		{
			if (dist >= 256)
			{
				return (int)ZLibUtil._dist_code[256 + ZLibUtil.URShift(dist, 7)];
			}
			return (int)ZLibUtil._dist_code[dist];
		}

		// Token: 0x06000801 RID: 2049 RVA: 0x00034A04 File Offset: 0x00033A04
		private void gen_bitlen(Deflate s)
		{
			short[] array = this.dyn_tree;
			short[] static_tree = this.stat_desc.static_tree;
			int[] extra_bits = this.stat_desc.extra_bits;
			int extra_base = this.stat_desc.extra_base;
			int max_length = this.stat_desc.max_length;
			int num = 0;
			for (int i = 0; i <= 15; i++)
			{
				s.bl_count[i] = 0;
			}
			array[s.heap[s.heap_max] * 2 + 1] = 0;
			int j;
			for (j = s.heap_max + 1; j < 573; j++)
			{
				int num2 = s.heap[j];
				int i = (int)(array[(int)(array[num2 * 2 + 1] * 2 + 1)] + 1);
				if (i > max_length)
				{
					i = max_length;
					num++;
				}
				array[num2 * 2 + 1] = (short)i;
				if (num2 <= this.max_code)
				{
					short[] bl_count = s.bl_count;
					int num3 = i;
					bl_count[num3] += 1;
					int num4 = 0;
					if (num2 >= extra_base)
					{
						num4 = extra_bits[num2 - extra_base];
					}
					short num5 = array[num2 * 2];
					s.opt_len += (int)num5 * (i + num4);
					if (static_tree != null)
					{
						s.static_len += (int)num5 * ((int)static_tree[num2 * 2 + 1] + num4);
					}
				}
			}
			if (num == 0)
			{
				return;
			}
			do
			{
				int i = max_length - 1;
				while (s.bl_count[i] == 0)
				{
					i--;
				}
				short[] bl_count2 = s.bl_count;
				int num6 = i;
				bl_count2[num6] -= 1;
				s.bl_count[i + 1] = s.bl_count[i + 1] + 2;
				short[] bl_count3 = s.bl_count;
				int num7 = max_length;
				bl_count3[num7] -= 1;
				num -= 2;
			}
			while (num > 0);
			for (int i = max_length; i != 0; i--)
			{
				int num2 = (int)s.bl_count[i];
				while (num2 != 0)
				{
					int num8 = s.heap[--j];
					if (num8 <= this.max_code)
					{
						if ((int)array[num8 * 2 + 1] != i)
						{
							s.opt_len = (int)((long)s.opt_len + ((long)i - (long)array[num8 * 2 + 1]) * (long)array[num8 * 2]);
							array[num8 * 2 + 1] = (short)i;
						}
						num2--;
					}
				}
			}
		}

		// Token: 0x06000802 RID: 2050 RVA: 0x00034C38 File Offset: 0x00033C38
		internal void build_tree(Deflate s)
		{
			short[] array = this.dyn_tree;
			short[] static_tree = this.stat_desc.static_tree;
			int elems = this.stat_desc.elems;
			int num = -1;
			s.heap_len = 0;
			s.heap_max = 573;
			for (int i = 0; i < elems; i++)
			{
				if (array[i * 2] != 0)
				{
					num = (s.heap[++s.heap_len] = i);
					s.depth[i] = 0;
				}
				else
				{
					array[i * 2 + 1] = 0;
				}
			}
			int num2;
			while (s.heap_len < 2)
			{
				num2 = (s.heap[++s.heap_len] = ((num < 2) ? (++num) : 0));
				array[num2 * 2] = 1;
				s.depth[num2] = 0;
				s.opt_len--;
				if (static_tree != null)
				{
					s.static_len -= (int)static_tree[num2 * 2 + 1];
				}
			}
			this.max_code = num;
			for (int i = s.heap_len / 2; i >= 1; i--)
			{
				s.pqdownheap(array, i);
			}
			num2 = elems;
			do
			{
				int i = s.heap[1];
				s.heap[1] = s.heap[s.heap_len--];
				s.pqdownheap(array, 1);
				int num3 = s.heap[1];
				s.heap[--s.heap_max] = i;
				s.heap[--s.heap_max] = num3;
				array[num2 * 2] = array[i * 2] + array[num3 * 2];
				s.depth[num2] = Math.Max(s.depth[i], s.depth[num3]) + 1;
				array[i * 2 + 1] = (array[num3 * 2 + 1] = (short)num2);
				s.heap[1] = num2++;
				s.pqdownheap(array, 1);
			}
			while (s.heap_len >= 2);
			s.heap[--s.heap_max] = s.heap[1];
			this.gen_bitlen(s);
			Tree.gen_codes(array, num, s.bl_count);
		}

		// Token: 0x06000803 RID: 2051 RVA: 0x00034E70 File Offset: 0x00033E70
		private static void gen_codes(short[] tree, int max_code, short[] bl_count)
		{
			short[] array = new short[16];
			short num = 0;
			for (int i = 1; i <= 15; i++)
			{
				num = (array[i] = (short)(num + bl_count[i - 1] << 1));
			}
			for (int j = 0; j <= max_code; j++)
			{
				int num2 = (int)tree[j * 2 + 1];
				if (num2 != 0)
				{
					int num3 = j * 2;
					short[] array2 = array;
					int num4 = num2;
					short code;
					array2[num4] = (code = array2[num4]) + 1;
					tree[num3] = (short)Tree.bi_reverse((int)code, num2);
				}
			}
		}

		// Token: 0x06000804 RID: 2052 RVA: 0x00034EE4 File Offset: 0x00033EE4
		private static int bi_reverse(int code, int len)
		{
			int num = 0;
			do
			{
				num |= (code & 1);
				code = ZLibUtil.URShift(code, 1);
				num <<= 1;
			}
			while (--len > 0);
			return ZLibUtil.URShift(num, 1);
		}

		// Token: 0x040004FC RID: 1276
		private short[] dyn_tree;

		// Token: 0x040004FD RID: 1277
		private int max_code;

		// Token: 0x040004FE RID: 1278
		private StaticTree stat_desc;
	}
}
