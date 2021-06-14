using System;

namespace ComponentAce.Compression.Libs.ZLib
{
	// Token: 0x020000B0 RID: 176
	internal sealed class InfCodes
	{
		// Token: 0x060007E4 RID: 2020 RVA: 0x000307C2 File Offset: 0x0002F7C2
		internal InfCodes(int bl, int bd, int[] tl, int tl_index, int[] td, int td_index, ZStream z)
		{
			this.mode = InflateCodesMode.START;
			this.lbits = (byte)bl;
			this.dbits = (byte)bd;
			this.ltree = tl;
			this.ltree_index = tl_index;
			this.dtree = td;
			this.dtree_index = td_index;
		}

		// Token: 0x060007E5 RID: 2021 RVA: 0x00030800 File Offset: 0x0002F800
		internal InfCodes(int bl, int bd, int[] tl, int[] td, ZStream z)
		{
			this.mode = InflateCodesMode.START;
			this.lbits = (byte)bl;
			this.dbits = (byte)bd;
			this.ltree = tl;
			this.ltree_index = 0;
			this.dtree = td;
			this.dtree_index = 0;
		}

		// Token: 0x060007E6 RID: 2022 RVA: 0x0003083C File Offset: 0x0002F83C
		internal int proc(InfBlocks s, ZStream z, int r)
		{
			int num = z.next_in_index;
			int num2 = z.avail_in;
			int num3 = s.BitB;
			int i = s.BitK;
			int num4 = s.WritePos;
			int num5 = (num4 < s.ReadPos) ? (s.ReadPos - num4 - 1) : (s.End - num4);
			for (;;)
			{
				int num6;
				switch (this.mode)
				{
				case InflateCodesMode.START:
					if (num5 >= 258 && num2 >= 10)
					{
						s.BitB = num3;
						s.BitK = i;
						z.avail_in = num2;
						z.total_in += (long)(num - z.next_in_index);
						z.next_in_index = num;
						s.WritePos = num4;
						r = this.inflate_fast((int)this.lbits, (int)this.dbits, this.ltree, this.ltree_index, this.dtree, this.dtree_index, s, z);
						num = z.next_in_index;
						num2 = z.avail_in;
						num3 = s.BitB;
						i = s.BitK;
						num4 = s.WritePos;
						num5 = ((num4 < s.ReadPos) ? (s.ReadPos - num4 - 1) : (s.End - num4));
						if (r != 0)
						{
							this.mode = ((r == 1) ? InflateCodesMode.WASH : InflateCodesMode.BADCODE);
							continue;
						}
					}
					this.need = (int)this.lbits;
					this.tree = this.ltree;
					this.tree_index = this.ltree_index;
					this.mode = InflateCodesMode.LEN;
					goto IL_199;
				case InflateCodesMode.LEN:
					goto IL_199;
				case InflateCodesMode.LENEXT:
					num6 = this.get_Renamed;
					while (i < num6)
					{
						if (num2 == 0)
						{
							goto IL_34F;
						}
						r = 0;
						num2--;
						num3 |= (int)(z.next_in[num++] & byte.MaxValue) << i;
						i += 8;
					}
					this.count += (num3 & ZLibUtil.inflate_mask[num6]);
					num3 >>= num6;
					i -= num6;
					this.need = (int)this.dbits;
					this.tree = this.dtree;
					this.tree_index = this.dtree_index;
					this.mode = InflateCodesMode.DIST;
					goto IL_412;
				case InflateCodesMode.DIST:
					goto IL_412;
				case InflateCodesMode.DISTEXT:
					num6 = this.get_Renamed;
					while (i < num6)
					{
						if (num2 == 0)
						{
							goto IL_596;
						}
						r = 0;
						num2--;
						num3 |= (int)(z.next_in[num++] & byte.MaxValue) << i;
						i += 8;
					}
					this.dist += (num3 & ZLibUtil.inflate_mask[num6]);
					num3 >>= num6;
					i -= num6;
					this.mode = InflateCodesMode.COPY;
					goto IL_635;
				case InflateCodesMode.COPY:
					goto IL_635;
				case InflateCodesMode.LIT:
					if (num5 == 0)
					{
						if (num4 == s.End && s.ReadPos != 0)
						{
							num4 = 0;
							num5 = ((num4 < s.ReadPos) ? (s.ReadPos - num4 - 1) : (s.End - num4));
						}
						if (num5 == 0)
						{
							s.WritePos = num4;
							r = s.inflate_flush(z, r);
							num4 = s.WritePos;
							num5 = ((num4 < s.ReadPos) ? (s.ReadPos - num4 - 1) : (s.End - num4));
							if (num4 == s.End && s.ReadPos != 0)
							{
								num4 = 0;
								num5 = ((num4 < s.ReadPos) ? (s.ReadPos - num4 - 1) : (s.End - num4));
							}
							if (num5 == 0)
							{
								goto Block_44;
							}
						}
					}
					r = 0;
					s.Window[num4++] = (byte)this.lit;
					num5--;
					this.mode = InflateCodesMode.START;
					continue;
				case InflateCodesMode.WASH:
					goto IL_8DB;
				case InflateCodesMode.END:
					goto IL_98A;
				case InflateCodesMode.BADCODE:
					goto IL_9D4;
				}
				break;
				IL_199:
				num6 = this.need;
				while (i < num6)
				{
					if (num2 == 0)
					{
						goto IL_1AB;
					}
					r = 0;
					num2--;
					num3 |= (int)(z.next_in[num++] & byte.MaxValue) << i;
					i += 8;
				}
				int num7 = (this.tree_index + (num3 & ZLibUtil.inflate_mask[num6])) * 3;
				num3 = ZLibUtil.URShift(num3, this.tree[num7 + 1]);
				i -= this.tree[num7 + 1];
				int num8 = this.tree[num7];
				if (num8 == 0)
				{
					this.lit = this.tree[num7 + 2];
					this.mode = InflateCodesMode.LIT;
					continue;
				}
				if ((num8 & 16) != 0)
				{
					this.get_Renamed = (num8 & 15);
					this.count = this.tree[num7 + 2];
					this.mode = InflateCodesMode.LENEXT;
					continue;
				}
				if ((num8 & 64) == 0)
				{
					this.need = num8;
					this.tree_index = num7 / 3 + this.tree[num7 + 2];
					continue;
				}
				if ((num8 & 32) != 0)
				{
					this.mode = InflateCodesMode.WASH;
					continue;
				}
				goto IL_2DF;
				IL_412:
				num6 = this.need;
				while (i < num6)
				{
					if (num2 == 0)
					{
						goto IL_424;
					}
					r = 0;
					num2--;
					num3 |= (int)(z.next_in[num++] & byte.MaxValue) << i;
					i += 8;
				}
				num7 = (this.tree_index + (num3 & ZLibUtil.inflate_mask[num6])) * 3;
				num3 >>= this.tree[num7 + 1];
				i -= this.tree[num7 + 1];
				num8 = this.tree[num7];
				if ((num8 & 16) != 0)
				{
					this.get_Renamed = (num8 & 15);
					this.dist = this.tree[num7 + 2];
					this.mode = InflateCodesMode.DISTEXT;
					continue;
				}
				if ((num8 & 64) == 0)
				{
					this.need = num8;
					this.tree_index = num7 / 3 + this.tree[num7 + 2];
					continue;
				}
				goto IL_526;
				IL_635:
				int j;
				for (j = num4 - this.dist; j < 0; j += s.End)
				{
				}
				while (this.count != 0)
				{
					if (num5 == 0)
					{
						if (num4 == s.End && s.ReadPos != 0)
						{
							num4 = 0;
							num5 = ((num4 < s.ReadPos) ? (s.ReadPos - num4 - 1) : (s.End - num4));
						}
						if (num5 == 0)
						{
							s.WritePos = num4;
							r = s.inflate_flush(z, r);
							num4 = s.WritePos;
							num5 = ((num4 < s.ReadPos) ? (s.ReadPos - num4 - 1) : (s.End - num4));
							if (num4 == s.End && s.ReadPos != 0)
							{
								num4 = 0;
								num5 = ((num4 < s.ReadPos) ? (s.ReadPos - num4 - 1) : (s.End - num4));
							}
							if (num5 == 0)
							{
								goto Block_32;
							}
						}
					}
					s.Window[num4++] = s.Window[j++];
					num5--;
					if (j == s.End)
					{
						j = 0;
					}
					this.count--;
				}
				this.mode = InflateCodesMode.START;
			}
			r = -2;
			s.BitB = num3;
			s.BitK = i;
			z.avail_in = num2;
			z.total_in += (long)(num - z.next_in_index);
			z.next_in_index = num;
			s.WritePos = num4;
			return s.inflate_flush(z, r);
			IL_1AB:
			s.BitB = num3;
			s.BitK = i;
			z.avail_in = num2;
			z.total_in += (long)(num - z.next_in_index);
			z.next_in_index = num;
			s.WritePos = num4;
			return s.inflate_flush(z, r);
			IL_2DF:
			this.mode = InflateCodesMode.BADCODE;
			z.msg = "invalid literal/length code";
			r = -3;
			s.BitB = num3;
			s.BitK = i;
			z.avail_in = num2;
			z.total_in += (long)(num - z.next_in_index);
			z.next_in_index = num;
			s.WritePos = num4;
			return s.inflate_flush(z, r);
			IL_34F:
			s.BitB = num3;
			s.BitK = i;
			z.avail_in = num2;
			z.total_in += (long)(num - z.next_in_index);
			z.next_in_index = num;
			s.WritePos = num4;
			return s.inflate_flush(z, r);
			IL_424:
			s.BitB = num3;
			s.BitK = i;
			z.avail_in = num2;
			z.total_in += (long)(num - z.next_in_index);
			z.next_in_index = num;
			s.WritePos = num4;
			return s.inflate_flush(z, r);
			IL_526:
			this.mode = InflateCodesMode.BADCODE;
			z.msg = "invalid distance code";
			r = -3;
			s.BitB = num3;
			s.BitK = i;
			z.avail_in = num2;
			z.total_in += (long)(num - z.next_in_index);
			z.next_in_index = num;
			s.WritePos = num4;
			return s.inflate_flush(z, r);
			IL_596:
			s.BitB = num3;
			s.BitK = i;
			z.avail_in = num2;
			z.total_in += (long)(num - z.next_in_index);
			z.next_in_index = num;
			s.WritePos = num4;
			return s.inflate_flush(z, r);
			Block_32:
			s.BitB = num3;
			s.BitK = i;
			z.avail_in = num2;
			z.total_in += (long)(num - z.next_in_index);
			z.next_in_index = num;
			s.WritePos = num4;
			return s.inflate_flush(z, r);
			Block_44:
			s.BitB = num3;
			s.BitK = i;
			z.avail_in = num2;
			z.total_in += (long)(num - z.next_in_index);
			z.next_in_index = num;
			s.WritePos = num4;
			return s.inflate_flush(z, r);
			IL_8DB:
			if (i > 7)
			{
				i -= 8;
				num2++;
				num--;
			}
			s.WritePos = num4;
			r = s.inflate_flush(z, r);
			num4 = s.WritePos;
			int num9 = (num4 < s.ReadPos) ? (s.ReadPos - num4 - 1) : (s.End - num4);
			if (s.ReadPos != s.WritePos)
			{
				s.BitB = num3;
				s.BitK = i;
				z.avail_in = num2;
				z.total_in += (long)(num - z.next_in_index);
				z.next_in_index = num;
				s.WritePos = num4;
				return s.inflate_flush(z, r);
			}
			this.mode = InflateCodesMode.END;
			IL_98A:
			r = 1;
			s.BitB = num3;
			s.BitK = i;
			z.avail_in = num2;
			z.total_in += (long)(num - z.next_in_index);
			z.next_in_index = num;
			s.WritePos = num4;
			return s.inflate_flush(z, r);
			IL_9D4:
			r = -3;
			s.BitB = num3;
			s.BitK = i;
			z.avail_in = num2;
			z.total_in += (long)(num - z.next_in_index);
			z.next_in_index = num;
			s.WritePos = num4;
			return s.inflate_flush(z, r);
		}

		// Token: 0x060007E7 RID: 2023 RVA: 0x000312B2 File Offset: 0x000302B2
		internal void free(ZStream z)
		{
		}

		// Token: 0x060007E8 RID: 2024 RVA: 0x000312B4 File Offset: 0x000302B4
		internal int inflate_fast(int bl, int bd, int[] tl, int tl_index, int[] td, int td_index, InfBlocks s, ZStream z)
		{
			int num = z.next_in_index;
			int num2 = z.avail_in;
			int num3 = s.BitB;
			int i = s.BitK;
			int num4 = s.WritePos;
			int num5 = (num4 < s.ReadPos) ? (s.ReadPos - num4 - 1) : (s.End - num4);
			int num6 = ZLibUtil.inflate_mask[bl];
			int num7 = ZLibUtil.inflate_mask[bd];
			int num9;
			int num10;
			for (;;)
			{
				if (i >= 20)
				{
					int num8 = num3 & num6;
					if ((num9 = tl[(tl_index + num8) * 3]) == 0)
					{
						num3 >>= tl[(tl_index + num8) * 3 + 1];
						i -= tl[(tl_index + num8) * 3 + 1];
						s.Window[num4++] = (byte)tl[(tl_index + num8) * 3 + 2];
						num5--;
					}
					else
					{
						for (;;)
						{
							num3 >>= tl[(tl_index + num8) * 3 + 1];
							i -= tl[(tl_index + num8) * 3 + 1];
							if ((num9 & 16) != 0)
							{
								break;
							}
							if ((num9 & 64) != 0)
							{
								goto IL_4D3;
							}
							num8 += tl[(tl_index + num8) * 3 + 2];
							num8 += (num3 & ZLibUtil.inflate_mask[num9]);
							if ((num9 = tl[(tl_index + num8) * 3]) == 0)
							{
								goto Block_20;
							}
						}
						num9 &= 15;
						num10 = tl[(tl_index + num8) * 3 + 2] + (num3 & ZLibUtil.inflate_mask[num9]);
						num3 >>= num9;
						for (i -= num9; i < 15; i += 8)
						{
							num2--;
							num3 |= (int)(z.next_in[num++] & byte.MaxValue) << i;
						}
						num8 = (num3 & num7);
						num9 = td[(td_index + num8) * 3];
						for (;;)
						{
							num3 >>= td[(td_index + num8) * 3 + 1];
							i -= td[(td_index + num8) * 3 + 1];
							if ((num9 & 16) != 0)
							{
								break;
							}
							if ((num9 & 64) != 0)
							{
								goto IL_3D9;
							}
							num8 += td[(td_index + num8) * 3 + 2];
							num8 += (num3 & ZLibUtil.inflate_mask[num9]);
							num9 = td[(td_index + num8) * 3];
						}
						num9 &= 15;
						while (i < num9)
						{
							num2--;
							num3 |= (int)(z.next_in[num++] & byte.MaxValue) << i;
							i += 8;
						}
						int num11 = td[(td_index + num8) * 3 + 2] + (num3 & ZLibUtil.inflate_mask[num9]);
						num3 >>= num9;
						i -= num9;
						num5 -= num10;
						int num12;
						if (num4 >= num11)
						{
							num12 = num4 - num11;
							if (num4 - num12 > 0 && 2 > num4 - num12)
							{
								s.Window[num4++] = s.Window[num12++];
								num10--;
								s.Window[num4++] = s.Window[num12++];
								num10--;
							}
							else
							{
								Array.Copy(s.Window, num12, s.Window, num4, 2);
								num4 += 2;
								num12 += 2;
								num10 -= 2;
							}
						}
						else
						{
							num12 = num4 - num11;
							do
							{
								num12 += s.End;
							}
							while (num12 < 0);
							num9 = s.End - num12;
							if (num10 > num9)
							{
								num10 -= num9;
								if (num4 - num12 > 0 && num9 > num4 - num12)
								{
									do
									{
										s.Window[num4++] = s.Window[num12++];
									}
									while (--num9 != 0);
								}
								else
								{
									Array.Copy(s.Window, num12, s.Window, num4, num9);
									num4 += num9;
									num12 += num9;
								}
								num12 = 0;
							}
						}
						if (num4 - num12 > 0 && num10 > num4 - num12)
						{
							do
							{
								s.Window[num4++] = s.Window[num12++];
							}
							while (--num10 != 0);
							goto IL_5E0;
						}
						Array.Copy(s.Window, num12, s.Window, num4, num10);
						num4 += num10;
						num12 += num10;
						goto IL_5E0;
						Block_20:
						num3 >>= tl[(tl_index + num8) * 3 + 1];
						i -= tl[(tl_index + num8) * 3 + 1];
						s.Window[num4++] = (byte)tl[(tl_index + num8) * 3 + 2];
						num5--;
					}
					IL_5E0:
					if (num5 < 258 || num2 < 10)
					{
						goto IL_5F2;
					}
				}
				else
				{
					num2--;
					num3 |= (int)(z.next_in[num++] & byte.MaxValue) << i;
					i += 8;
				}
			}
			IL_3D9:
			z.msg = "invalid distance code";
			num10 = z.avail_in - num2;
			num10 = ((i >> 3 < num10) ? (i >> 3) : num10);
			num2 += num10;
			num -= num10;
			i -= num10 << 3;
			s.BitB = num3;
			s.BitK = i;
			z.avail_in = num2;
			z.total_in += (long)(num - z.next_in_index);
			z.next_in_index = num;
			s.WritePos = num4;
			return -3;
			IL_4D3:
			if ((num9 & 32) != 0)
			{
				num10 = z.avail_in - num2;
				num10 = ((i >> 3 < num10) ? (i >> 3) : num10);
				num2 += num10;
				num -= num10;
				i -= num10 << 3;
				s.BitB = num3;
				s.BitK = i;
				z.avail_in = num2;
				z.total_in += (long)(num - z.next_in_index);
				z.next_in_index = num;
				s.WritePos = num4;
				return 1;
			}
			z.msg = "invalid literal/length code";
			num10 = z.avail_in - num2;
			num10 = ((i >> 3 < num10) ? (i >> 3) : num10);
			num2 += num10;
			num -= num10;
			i -= num10 << 3;
			s.BitB = num3;
			s.BitK = i;
			z.avail_in = num2;
			z.total_in += (long)(num - z.next_in_index);
			z.next_in_index = num;
			s.WritePos = num4;
			return -3;
			IL_5F2:
			num10 = z.avail_in - num2;
			num10 = ((i >> 3 < num10) ? (i >> 3) : num10);
			num2 += num10;
			num -= num10;
			i -= num10 << 3;
			s.BitB = num3;
			s.BitK = i;
			z.avail_in = num2;
			z.total_in += (long)(num - z.next_in_index);
			z.next_in_index = num;
			s.WritePos = num4;
			return 0;
		}

		// Token: 0x040004BD RID: 1213
		private InflateCodesMode mode;

		// Token: 0x040004BE RID: 1214
		private int count;

		// Token: 0x040004BF RID: 1215
		private int[] tree;

		// Token: 0x040004C0 RID: 1216
		internal int tree_index;

		// Token: 0x040004C1 RID: 1217
		internal int need;

		// Token: 0x040004C2 RID: 1218
		internal int lit;

		// Token: 0x040004C3 RID: 1219
		internal int get_Renamed;

		// Token: 0x040004C4 RID: 1220
		internal int dist;

		// Token: 0x040004C5 RID: 1221
		private byte lbits;

		// Token: 0x040004C6 RID: 1222
		private byte dbits;

		// Token: 0x040004C7 RID: 1223
		private int[] ltree;

		// Token: 0x040004C8 RID: 1224
		private int ltree_index;

		// Token: 0x040004C9 RID: 1225
		private int[] dtree;

		// Token: 0x040004CA RID: 1226
		private int dtree_index;
	}
}
