using System;

namespace ComponentAce.Compression.Libs.ZLib
{
	// Token: 0x020000B4 RID: 180
	internal sealed class InfTree
	{
		// Token: 0x060007F3 RID: 2035 RVA: 0x00033EAC File Offset: 0x00032EAC
		private static int huft_build(int[] b, int bindex, int n, int s, int[] d, int[] e, int[] t, int[] m, int[] hp, int[] hn, int[] v)
		{
			int[] array = new int[16];
			int[] array2 = new int[3];
			int[] array3 = new int[15];
			int[] array4 = new int[16];
			int num = 0;
			int num2 = n;
			do
			{
				array[b[bindex + num]]++;
				num++;
				num2--;
			}
			while (num2 != 0);
			if (array[0] == n)
			{
				t[0] = -1;
				m[0] = 0;
				return 0;
			}
			int num3 = m[0];
			int i = 1;
			while (i <= 15 && array[i] == 0)
			{
				i++;
			}
			int j = i;
			if (num3 < i)
			{
				num3 = i;
			}
			num2 = 15;
			while (num2 != 0 && array[num2] == 0)
			{
				num2--;
			}
			int num4 = num2;
			if (num3 > num2)
			{
				num3 = num2;
			}
			m[0] = num3;
			int num5 = 1 << i;
			while (i < num2)
			{
				if ((num5 -= array[i]) < 0)
				{
					return -3;
				}
				i++;
				num5 <<= 1;
			}
			if ((num5 -= array[num2]) < 0)
			{
				return -3;
			}
			array[num2] += num5;
			i = (array4[1] = 0);
			num = 1;
			int num6 = 2;
			while (--num2 != 0)
			{
				i = (array4[num6] = i + array[num]);
				num6++;
				num++;
			}
			num2 = 0;
			num = 0;
			do
			{
				if ((i = b[bindex + num]) != 0)
				{
					v[array4[i]++] = num2;
				}
				num++;
			}
			while (++num2 < n);
			n = array4[num4];
			num2 = (array4[0] = 0);
			num = 0;
			int num7 = -1;
			int num8 = -num3;
			array3[0] = 0;
			int num9 = 0;
			int num10 = 0;
			while (j <= num4)
			{
				int num11 = array[j];
				while (num11-- != 0)
				{
					int num12;
					while (j > num8 + num3)
					{
						num7++;
						num8 += num3;
						num10 = num4 - num8;
						num10 = ((num10 > num3) ? num3 : num10);
						if ((num12 = 1 << ((i = j - num8) & 31)) > num11 + 1)
						{
							num12 -= num11 + 1;
							num6 = j;
							if (i < num10)
							{
								while (++i < num10 && (num12 <<= 1) > array[++num6])
								{
									num12 -= array[num6];
								}
							}
						}
						num10 = 1 << i;
						if (hn[0] + num10 > 1440)
						{
							return -3;
						}
						num9 = (array3[num7] = hn[0]);
						hn[0] += num10;
						if (num7 != 0)
						{
							array4[num7] = num2;
							array2[0] = (int)((byte)i);
							array2[1] = (int)((byte)num3);
							i = ZLibUtil.URShift(num2, num8 - num3);
							array2[2] = num9 - array3[num7 - 1] - i;
							Array.Copy(array2, 0, hp, (array3[num7 - 1] + i) * 3, 3);
						}
						else
						{
							t[0] = num9;
						}
					}
					array2[1] = (int)((byte)(j - num8));
					if (num >= n)
					{
						array2[0] = 192;
					}
					else if (v[num] < s)
					{
						array2[0] = (int)((v[num] < 256) ? 0 : 96);
						array2[2] = v[num++];
					}
					else
					{
						array2[0] = (int)((byte)(e[v[num] - s] + 16 + 64));
						array2[2] = d[v[num++] - s];
					}
					num12 = 1 << j - num8;
					for (i = ZLibUtil.URShift(num2, num8); i < num10; i += num12)
					{
						Array.Copy(array2, 0, hp, (num9 + i) * 3, 3);
					}
					i = 1 << j - 1;
					while ((num2 & i) != 0)
					{
						num2 ^= i;
						i = ZLibUtil.URShift(i, 1);
					}
					num2 ^= i;
					int num13 = (1 << num8) - 1;
					while ((num2 & num13) != array4[num7])
					{
						num7--;
						num8 -= num3;
						num13 = (1 << num8) - 1;
					}
				}
				j++;
			}
			if (num5 == 0 || num4 == 1)
			{
				return 0;
			}
			return -5;
		}

		// Token: 0x060007F4 RID: 2036 RVA: 0x0003429C File Offset: 0x0003329C
		internal static int inflate_trees_bits(int[] c, int[] bb, int[] tb, int[] hp, ZStream z)
		{
			int[] hn = new int[1];
			int[] v = new int[19];
			int num = InfTree.huft_build(c, 0, 19, 19, null, null, tb, bb, hp, hn, v);
			if (num == -3)
			{
				z.msg = "oversubscribed dynamic bit lengths tree";
			}
			else if (num == -3 || bb[0] == 0)
			{
				z.msg = "incomplete dynamic bit lengths tree";
				num = -3;
			}
			return num;
		}

		// Token: 0x060007F5 RID: 2037 RVA: 0x000342F8 File Offset: 0x000332F8
		internal static int inflate_trees_dynamic(int nl, int nd, int[] c, int[] bl, int[] bd, int[] tl, int[] td, int[] hp, ZStream z)
		{
			int[] hn = new int[1];
			int[] v = new int[288];
			int num = InfTree.huft_build(c, 0, nl, 257, InfTreeUtil.cplens, InfTreeUtil.cplext, tl, bl, hp, hn, v);
			if (num != 0 || bl[0] == 0)
			{
				if (num == -3)
				{
					z.msg = "oversubscribed literal/length tree";
				}
				else if (num != -3)
				{
					z.msg = "incomplete literal/length tree";
					num = -3;
				}
				return num;
			}
			num = InfTree.huft_build(c, nl, nd, 0, InfTreeUtil.cpdist, InfTreeUtil.cpdext, td, bd, hp, hn, v);
			if (num != 0 || (bd[0] == 0 && nl > 257))
			{
				if (num == -3)
				{
					z.msg = "oversubscribed distance tree";
				}
				else if (num == -3)
				{
					z.msg = "incomplete distance tree";
					num = -3;
				}
				else if (num != -3)
				{
					z.msg = "empty distance tree with lengths";
					num = -3;
				}
				return num;
			}
			return 0;
		}

		// Token: 0x060007F6 RID: 2038 RVA: 0x000343D4 File Offset: 0x000333D4
		internal static int inflate_trees_fixed(int[] bl, int[] bd, int[][] tl, int[][] td, ZStream z)
		{
			bl[0] = 9;
			bd[0] = 5;
			tl[0] = InfTreeUtil.fixed_tl;
			td[0] = InfTreeUtil.fixed_td;
			return 0;
		}
	}
}
