using System;

namespace ComponentAce.Compression.Libs.ZLib
{
	// Token: 0x020000B2 RID: 178
	internal sealed class Inflate
	{
		// Token: 0x060007E9 RID: 2025 RVA: 0x00031930 File Offset: 0x00030930
		internal int inflateReset(ZStream z)
		{
			if (z == null || z.istate == null)
			{
				return -2;
			}
			z.total_in = (z.total_out = 0L);
			z.msg = null;
			z.istate.mode = ((z.istate.nowrap != 0) ? InflateMode.BLOCKS : InflateMode.METHOD);
			z.istate.blocks.reset(z, null);
			return 0;
		}

		// Token: 0x060007EA RID: 2026 RVA: 0x00031992 File Offset: 0x00030992
		internal int inflateEnd(ZStream z)
		{
			if (this.blocks != null)
			{
				this.blocks.free(z);
			}
			this.blocks = null;
			return 0;
		}

		// Token: 0x060007EB RID: 2027 RVA: 0x000319B0 File Offset: 0x000309B0
		internal int inflateInit(ZStream z, int windowBits)
		{
			z.msg = null;
			this.blocks = null;
			this.nowrap = 0;
			if (windowBits < 0)
			{
				windowBits = -windowBits;
				this.nowrap = 1;
			}
			if (windowBits < 8 || windowBits > 15)
			{
				this.inflateEnd(z);
				return -2;
			}
			this.wbits = windowBits;
			z.istate.blocks = new InfBlocks(z, z.istate.nowrap == 0, 1 << windowBits);
			this.inflateReset(z);
			return 0;
		}

		// Token: 0x060007EC RID: 2028 RVA: 0x00031A2C File Offset: 0x00030A2C
		internal int inflate(ZStream z, FlushStrategy flush)
		{
			if (z == null || z.istate == null || z.next_in == null)
			{
				return -2;
			}
			int num = (flush == FlushStrategy.Z_FINISH) ? -5 : 0;
			int num2 = -5;
			for (;;)
			{
				switch (z.istate.mode)
				{
				case InflateMode.METHOD:
					if (z.avail_in == 0)
					{
						return num2;
					}
					num2 = num;
					z.avail_in--;
					z.total_in += 1L;
					if (((z.istate.method = (int)z.next_in[z.next_in_index++]) & 15) != 8)
					{
						z.istate.mode = InflateMode.BAD;
						z.msg = "unknown compression method";
						z.istate.marker = 5;
						continue;
					}
					if ((z.istate.method >> 4) + 8 > z.istate.wbits)
					{
						z.istate.mode = InflateMode.BAD;
						z.msg = "invalid Window size";
						z.istate.marker = 5;
						continue;
					}
					z.istate.mode = InflateMode.FLAG;
					goto IL_149;
				case InflateMode.FLAG:
					goto IL_149;
				case InflateMode.DICT4:
					goto IL_1F3;
				case InflateMode.DICT3:
					goto IL_25E;
				case InflateMode.DICT2:
					goto IL_2D0;
				case InflateMode.DICT1:
					goto IL_341;
				case InflateMode.DICT0:
					goto IL_3BD;
				case InflateMode.BLOCKS:
					num2 = z.istate.blocks.proc(z, num2);
					if (num2 == -3)
					{
						z.istate.mode = InflateMode.BAD;
						z.istate.marker = 0;
						continue;
					}
					if (num2 == 0)
					{
						num2 = num;
					}
					if (num2 != 1)
					{
						return num2;
					}
					num2 = num;
					z.istate.blocks.reset(z, z.istate.was);
					if (z.istate.nowrap != 0)
					{
						z.istate.mode = InflateMode.DONE;
						continue;
					}
					z.istate.mode = InflateMode.CHECK4;
					goto IL_46E;
				case InflateMode.CHECK4:
					goto IL_46E;
				case InflateMode.CHECK3:
					goto IL_4D9;
				case InflateMode.CHECK2:
					goto IL_54C;
				case InflateMode.CHECK1:
					goto IL_5BE;
				case InflateMode.DONE:
					return 1;
				case InflateMode.BAD:
					return -3;
				}
				break;
				IL_149:
				if (z.avail_in == 0)
				{
					return num2;
				}
				num2 = num;
				z.avail_in--;
				z.total_in += 1L;
				int num3 = (int)(z.next_in[z.next_in_index++] & byte.MaxValue);
				if (((z.istate.method << 8) + num3) % 31 != 0)
				{
					z.istate.mode = InflateMode.BAD;
					z.msg = "incorrect header check";
					z.istate.marker = 5;
					continue;
				}
				if ((num3 & 32) == 0)
				{
					z.istate.mode = InflateMode.BLOCKS;
					continue;
				}
				goto IL_1E7;
				IL_5BE:
				if (z.avail_in == 0)
				{
					return num2;
				}
				num2 = num;
				z.avail_in--;
				z.total_in += 1L;
				z.istate.need += (long)((ulong)z.next_in[z.next_in_index++] & 255UL);
				if ((int)z.istate.was[0] != (int)z.istate.need)
				{
					z.istate.mode = InflateMode.BAD;
					z.msg = "incorrect data check";
					z.istate.marker = 5;
					continue;
				}
				goto IL_660;
				IL_54C:
				if (z.avail_in == 0)
				{
					return num2;
				}
				num2 = num;
				z.avail_in--;
				z.total_in += 1L;
				z.istate.need += ((long)((long)(z.next_in[z.next_in_index++] & byte.MaxValue) << 8) & 65280L);
				z.istate.mode = InflateMode.CHECK1;
				goto IL_5BE;
				IL_4D9:
				if (z.avail_in == 0)
				{
					return num2;
				}
				num2 = num;
				z.avail_in--;
				z.total_in += 1L;
				z.istate.need += ((long)((long)(z.next_in[z.next_in_index++] & byte.MaxValue) << 16) & 16711680L);
				z.istate.mode = InflateMode.CHECK2;
				goto IL_54C;
				IL_46E:
				if (z.avail_in == 0)
				{
					return num2;
				}
				num2 = num;
				z.avail_in--;
				z.total_in += 1L;
				z.istate.need = (long)((int)(z.next_in[z.next_in_index++] & byte.MaxValue) << 24 & -16777216);
				z.istate.mode = InflateMode.CHECK3;
				goto IL_4D9;
			}
			return -2;
			IL_1E7:
			z.istate.mode = InflateMode.DICT4;
			IL_1F3:
			if (z.avail_in == 0)
			{
				return num2;
			}
			num2 = num;
			z.avail_in--;
			z.total_in += 1L;
			z.istate.need = ((long)(z.next_in[z.next_in_index++] & byte.MaxValue) << 24 & -16777216L);
			z.istate.mode = InflateMode.DICT3;
			IL_25E:
			if (z.avail_in == 0)
			{
				return num2;
			}
			num2 = num;
			z.avail_in--;
			z.total_in += 1L;
			z.istate.need += ((long)(z.next_in[z.next_in_index++] & byte.MaxValue) << 16 & 16711680L);
			z.istate.mode = InflateMode.DICT2;
			IL_2D0:
			if (z.avail_in == 0)
			{
				return num2;
			}
			num2 = num;
			z.avail_in--;
			z.total_in += 1L;
			z.istate.need += ((long)(z.next_in[z.next_in_index++] & byte.MaxValue) << 8 & 65280L);
			z.istate.mode = InflateMode.DICT1;
			IL_341:
			if (z.avail_in == 0)
			{
				return num2;
			}
			z.avail_in--;
			z.total_in += 1L;
			z.istate.need += (long)((ulong)z.next_in[z.next_in_index++] & 255UL);
			z.adler = z.istate.need;
			z.istate.mode = InflateMode.DICT0;
			return 2;
			IL_3BD:
			z.istate.mode = InflateMode.BAD;
			z.msg = "need dictionary";
			z.istate.marker = 0;
			return -2;
			IL_660:
			z.istate.mode = InflateMode.DONE;
			return 1;
		}

		// Token: 0x060007ED RID: 2029 RVA: 0x000320B0 File Offset: 0x000310B0
		internal int inflateSetDictionary(ZStream z, byte[] dictionary, int dictLength)
		{
			int start = 0;
			int num = dictLength;
			if (z == null || z.istate == null || z.istate.mode != InflateMode.DICT0)
			{
				return -2;
			}
			if (Adler32.GetAdler32Checksum(1L, dictionary, 0, dictLength) != z.adler)
			{
				return -3;
			}
			z.adler = Adler32.GetAdler32Checksum(0L, null, 0, 0);
			if (num >= 1 << z.istate.wbits)
			{
				num = (1 << z.istate.wbits) - 1;
				start = dictLength - num;
			}
			z.istate.blocks.set_dictionary(dictionary, start, num);
			z.istate.mode = InflateMode.BLOCKS;
			return 0;
		}

		// Token: 0x060007EE RID: 2030 RVA: 0x0003214C File Offset: 0x0003114C
		internal int inflateSync(ZStream z)
		{
			if (z == null || z.istate == null)
			{
				return -2;
			}
			if (z.istate.mode != InflateMode.BAD)
			{
				z.istate.mode = InflateMode.BAD;
				z.istate.marker = 0;
			}
			int num;
			if ((num = z.avail_in) == 0)
			{
				return -5;
			}
			int num2 = z.next_in_index;
			int num3 = z.istate.marker;
			while (num != 0 && num3 < 4)
			{
				if (z.next_in[num2] == ZLibUtil.mark[num3])
				{
					num3++;
				}
				else if (z.next_in[num2] != 0)
				{
					num3 = 0;
				}
				else
				{
					num3 = 4 - num3;
				}
				num2++;
				num--;
			}
			z.total_in += (long)(num2 - z.next_in_index);
			z.next_in_index = num2;
			z.avail_in = num;
			z.istate.marker = num3;
			if (num3 != 4)
			{
				return -3;
			}
			long total_in = z.total_in;
			long total_out = z.total_out;
			this.inflateReset(z);
			z.total_in = total_in;
			z.total_out = total_out;
			z.istate.mode = InflateMode.BLOCKS;
			return 0;
		}

		// Token: 0x060007EF RID: 2031 RVA: 0x00032253 File Offset: 0x00031253
		internal int inflateSyncPoint(ZStream z)
		{
			if (z == null || z.istate == null || z.istate.blocks == null)
			{
				return -2;
			}
			return z.istate.blocks.sync_point();
		}

		// Token: 0x040004DA RID: 1242
		private InflateMode mode;

		// Token: 0x040004DB RID: 1243
		private int method;

		// Token: 0x040004DC RID: 1244
		private long[] was = new long[1];

		// Token: 0x040004DD RID: 1245
		private long need;

		// Token: 0x040004DE RID: 1246
		private int marker;

		// Token: 0x040004DF RID: 1247
		private int nowrap;

		// Token: 0x040004E0 RID: 1248
		private int wbits;

		// Token: 0x040004E1 RID: 1249
		private InfBlocks blocks;
	}
}
