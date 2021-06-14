using System;

namespace ComponentAce.Compression.Libs.ZLib
{
	// Token: 0x020000AE RID: 174
	internal sealed class InfBlocks
	{
		// Token: 0x17000156 RID: 342
		// (get) Token: 0x060007D1 RID: 2001 RVA: 0x0002F709 File Offset: 0x0002E709
		// (set) Token: 0x060007D2 RID: 2002 RVA: 0x0002F711 File Offset: 0x0002E711
		public byte[] Window
		{
			get
			{
				return this.window;
			}
			set
			{
				this.window = value;
			}
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x060007D3 RID: 2003 RVA: 0x0002F71A File Offset: 0x0002E71A
		// (set) Token: 0x060007D4 RID: 2004 RVA: 0x0002F722 File Offset: 0x0002E722
		public int End
		{
			get
			{
				return this.end;
			}
			set
			{
				this.end = value;
			}
		}

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x060007D5 RID: 2005 RVA: 0x0002F72B File Offset: 0x0002E72B
		// (set) Token: 0x060007D6 RID: 2006 RVA: 0x0002F733 File Offset: 0x0002E733
		public int ReadPos
		{
			get
			{
				return this.read;
			}
			set
			{
				this.read = value;
			}
		}

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x060007D7 RID: 2007 RVA: 0x0002F73C File Offset: 0x0002E73C
		// (set) Token: 0x060007D8 RID: 2008 RVA: 0x0002F744 File Offset: 0x0002E744
		public int WritePos
		{
			get
			{
				return this.write;
			}
			set
			{
				this.write = value;
			}
		}

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x060007D9 RID: 2009 RVA: 0x0002F74D File Offset: 0x0002E74D
		// (set) Token: 0x060007DA RID: 2010 RVA: 0x0002F755 File Offset: 0x0002E755
		public int BitK
		{
			get
			{
				return this.bitk;
			}
			set
			{
				this.bitk = value;
			}
		}

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x060007DB RID: 2011 RVA: 0x0002F75E File Offset: 0x0002E75E
		// (set) Token: 0x060007DC RID: 2012 RVA: 0x0002F766 File Offset: 0x0002E766
		public int BitB
		{
			get
			{
				return this.bitb;
			}
			set
			{
				this.bitb = value;
			}
		}

		// Token: 0x060007DD RID: 2013 RVA: 0x0002F770 File Offset: 0x0002E770
		internal InfBlocks(ZStream z, bool needCheck, int w)
		{
			this.hufts = new int[4320];
			this.window = new byte[w];
			this.end = w;
			this.needCheck = needCheck;
			this.mode = InflateBlockMode.TYPE;
			this.reset(z, null);
		}

		// Token: 0x060007DE RID: 2014 RVA: 0x0002F7D4 File Offset: 0x0002E7D4
		internal void reset(ZStream z, long[] c)
		{
			if (c != null)
			{
				c[0] = this.check;
			}
			if (this.mode == InflateBlockMode.BTREE || this.mode == InflateBlockMode.DTREE)
			{
				this.blens = null;
			}
			if (this.mode == InflateBlockMode.CODES)
			{
				this.codes.free(z);
			}
			this.mode = InflateBlockMode.TYPE;
			this.BitK = 0;
			this.BitB = 0;
			this.ReadPos = (this.WritePos = 0);
			if (this.needCheck)
			{
				z.adler = (this.check = Adler32.GetAdler32Checksum(0L, null, 0, 0));
			}
		}

		// Token: 0x060007DF RID: 2015 RVA: 0x0002F864 File Offset: 0x0002E864
		internal int proc(ZStream z, int r)
		{
			int num = z.next_in_index;
			int num2 = z.avail_in;
			int num3 = this.BitB;
			int i = this.BitK;
			int num4 = this.WritePos;
			int num5 = (num4 < this.ReadPos) ? (this.ReadPos - num4 - 1) : (this.End - num4);
			int num6;
			for (;;)
			{
				switch (this.mode)
				{
				case InflateBlockMode.TYPE:
					while (i < 3)
					{
						if (num2 == 0)
						{
							goto IL_8C;
						}
						r = 0;
						num2--;
						num3 |= (int)(z.next_in[num++] & byte.MaxValue) << i;
						i += 8;
					}
					num6 = (num3 & 7);
					this.last = (num6 & 1);
					switch (ZLibUtil.URShift(num6, 1))
					{
					case 0:
						num3 = ZLibUtil.URShift(num3, 3);
						i -= 3;
						num6 = (i & 7);
						num3 = ZLibUtil.URShift(num3, num6);
						i -= num6;
						this.mode = InflateBlockMode.LENS;
						continue;
					case 1:
					{
						int[] array = new int[1];
						int[] array2 = new int[1];
						int[][] array3 = new int[1][];
						int[][] array4 = new int[1][];
						InfTree.inflate_trees_fixed(array, array2, array3, array4, z);
						this.codes = new InfCodes(array[0], array2[0], array3[0], array4[0], z);
						num3 = ZLibUtil.URShift(num3, 3);
						i -= 3;
						this.mode = InflateBlockMode.CODES;
						continue;
					}
					case 2:
						num3 = ZLibUtil.URShift(num3, 3);
						i -= 3;
						this.mode = InflateBlockMode.TABLE;
						continue;
					case 3:
						goto IL_1CD;
					default:
						continue;
					}
					break;
				case InflateBlockMode.LENS:
					while (i < 32)
					{
						if (num2 == 0)
						{
							goto IL_23D;
						}
						r = 0;
						num2--;
						num3 |= (int)(z.next_in[num++] & byte.MaxValue) << i;
						i += 8;
					}
					if ((ZLibUtil.URShift(~num3, 16) & 65535) != (num3 & 65535))
					{
						goto Block_8;
					}
					this.left = (num3 & 65535);
					i = (num3 = 0);
					this.mode = ((this.left != 0) ? InflateBlockMode.STORED : ((this.last != 0) ? InflateBlockMode.DRY : InflateBlockMode.TYPE));
					continue;
				case InflateBlockMode.STORED:
					if (num2 == 0)
					{
						goto Block_11;
					}
					if (num5 == 0)
					{
						if (num4 == this.End && this.ReadPos != 0)
						{
							num4 = 0;
							num5 = ((num4 < this.ReadPos) ? (this.ReadPos - num4 - 1) : (this.End - num4));
						}
						if (num5 == 0)
						{
							this.WritePos = num4;
							r = this.inflate_flush(z, r);
							num4 = this.WritePos;
							num5 = ((num4 < this.ReadPos) ? (this.ReadPos - num4 - 1) : (this.End - num4));
							if (num4 == this.End && this.ReadPos != 0)
							{
								num4 = 0;
								num5 = ((num4 < this.ReadPos) ? (this.ReadPos - num4 - 1) : (this.End - num4));
							}
							if (num5 == 0)
							{
								goto Block_21;
							}
						}
					}
					r = 0;
					num6 = this.left;
					if (num6 > num2)
					{
						num6 = num2;
					}
					if (num6 > num5)
					{
						num6 = num5;
					}
					Array.Copy(z.next_in, num, this.Window, num4, num6);
					num += num6;
					num2 -= num6;
					num4 += num6;
					num5 -= num6;
					if ((this.left -= num6) == 0)
					{
						this.mode = ((this.last != 0) ? InflateBlockMode.DRY : InflateBlockMode.TYPE);
						continue;
					}
					continue;
				case InflateBlockMode.TABLE:
					while (i < 14)
					{
						if (num2 == 0)
						{
							goto IL_515;
						}
						r = 0;
						num2--;
						num3 |= (int)(z.next_in[num++] & byte.MaxValue) << i;
						i += 8;
					}
					num6 = (this.table = (num3 & 16383));
					if ((num6 & 31) > 29 || (num6 >> 5 & 31) > 29)
					{
						goto IL_5A3;
					}
					num6 = 258 + (num6 & 31) + (num6 >> 5 & 31);
					this.blens = new int[num6];
					num3 = ZLibUtil.URShift(num3, 14);
					i -= 14;
					this.index = 0;
					this.mode = InflateBlockMode.BTREE;
					goto IL_6E1;
				case InflateBlockMode.BTREE:
					goto IL_6E1;
				case InflateBlockMode.DTREE:
					goto IL_7B9;
				case InflateBlockMode.CODES:
					goto IL_B58;
				case InflateBlockMode.DRY:
					goto IL_C21;
				case InflateBlockMode.DONE:
					goto IL_CB6;
				case InflateBlockMode.BAD:
					goto IL_CFD;
				}
				break;
				IL_6E1:
				while (this.index < 4 + ZLibUtil.URShift(this.table, 10))
				{
					while (i < 3)
					{
						if (num2 == 0)
						{
							goto IL_646;
						}
						r = 0;
						num2--;
						num3 |= (int)(z.next_in[num++] & byte.MaxValue) << i;
						i += 8;
					}
					this.blens[ZLibUtil.border[this.index++]] = (num3 & 7);
					num3 = ZLibUtil.URShift(num3, 3);
					i -= 3;
				}
				while (this.index < 19)
				{
					this.blens[ZLibUtil.border[this.index++]] = 0;
				}
				this.bb[0] = 7;
				num6 = InfTree.inflate_trees_bits(this.blens, this.bb, this.tb, this.hufts, z);
				if (num6 != 0)
				{
					goto Block_33;
				}
				this.index = 0;
				this.mode = InflateBlockMode.DTREE;
				for (;;)
				{
					IL_7B9:
					num6 = this.table;
					if (this.index >= 258 + (num6 & 31) + (num6 >> 5 & 31))
					{
						break;
					}
					num6 = this.bb[0];
					while (i < num6)
					{
						if (num2 == 0)
						{
							goto IL_7F0;
						}
						r = 0;
						num2--;
						num3 |= (int)(z.next_in[num++] & byte.MaxValue) << i;
						i += 8;
					}
					num6 = this.hufts[(this.tb[0] + (num3 & ZLibUtil.inflate_mask[num6])) * 3 + 1];
					int num7 = this.hufts[(this.tb[0] + (num3 & ZLibUtil.inflate_mask[num6])) * 3 + 2];
					if (num7 < 16)
					{
						num3 = ZLibUtil.URShift(num3, num6);
						i -= num6;
						this.blens[this.index++] = num7;
					}
					else
					{
						int num8 = (num7 == 18) ? 7 : (num7 - 14);
						int num9 = (num7 == 18) ? 11 : 3;
						while (i < num6 + num8)
						{
							if (num2 == 0)
							{
								goto IL_8F4;
							}
							r = 0;
							num2--;
							num3 |= (int)(z.next_in[num++] & byte.MaxValue) << i;
							i += 8;
						}
						num3 = ZLibUtil.URShift(num3, num6);
						i -= num6;
						num9 += (num3 & ZLibUtil.inflate_mask[num8]);
						num3 = ZLibUtil.URShift(num3, num8);
						i -= num8;
						num8 = this.index;
						num6 = this.table;
						if (num8 + num9 > 258 + (num6 & 31) + (num6 >> 5 & 31) || (num7 == 16 && num8 < 1))
						{
							goto IL_9BE;
						}
						num7 = ((num7 == 16) ? this.blens[num8 - 1] : 0);
						do
						{
							this.blens[num8++] = num7;
						}
						while (--num9 != 0);
						this.index = num8;
					}
				}
				this.tb[0] = -1;
				int[] array5 = new int[1];
				int[] array6 = new int[1];
				int[] array7 = new int[1];
				int[] array8 = new int[1];
				array5[0] = 9;
				array6[0] = 6;
				num6 = this.table;
				num6 = InfTree.inflate_trees_dynamic(257 + (num6 & 31), 1 + (num6 >> 5 & 31), this.blens, array5, array6, array7, array8, this.hufts, z);
				if (num6 != 0)
				{
					goto Block_47;
				}
				this.codes = new InfCodes(array5[0], array6[0], this.hufts, array7[0], this.hufts, array8[0], z);
				this.blens = null;
				this.mode = InflateBlockMode.CODES;
				IL_B58:
				this.BitB = num3;
				this.BitK = i;
				z.avail_in = num2;
				z.total_in += (long)(num - z.next_in_index);
				z.next_in_index = num;
				this.WritePos = num4;
				if ((r = this.codes.proc(this, z, r)) != 1)
				{
					goto Block_49;
				}
				r = 0;
				this.codes.free(z);
				num = z.next_in_index;
				num2 = z.avail_in;
				num3 = this.BitB;
				i = this.BitK;
				num4 = this.WritePos;
				num5 = ((num4 < this.ReadPos) ? (this.ReadPos - num4 - 1) : (this.End - num4));
				if (this.last != 0)
				{
					goto IL_C1A;
				}
				this.mode = InflateBlockMode.TYPE;
			}
			r = -2;
			this.BitB = num3;
			this.BitK = i;
			z.avail_in = num2;
			z.total_in += (long)(num - z.next_in_index);
			z.next_in_index = num;
			this.WritePos = num4;
			return this.inflate_flush(z, r);
			IL_8C:
			this.BitB = num3;
			this.BitK = i;
			z.avail_in = num2;
			z.total_in += (long)(num - z.next_in_index);
			z.next_in_index = num;
			this.WritePos = num4;
			return this.inflate_flush(z, r);
			IL_1CD:
			num3 = ZLibUtil.URShift(num3, 3);
			i -= 3;
			this.mode = InflateBlockMode.BAD;
			z.msg = "invalid block type";
			r = -3;
			this.BitB = num3;
			this.BitK = i;
			z.avail_in = num2;
			z.total_in += (long)(num - z.next_in_index);
			z.next_in_index = num;
			this.WritePos = num4;
			return this.inflate_flush(z, r);
			IL_23D:
			this.BitB = num3;
			this.BitK = i;
			z.avail_in = num2;
			z.total_in += (long)(num - z.next_in_index);
			z.next_in_index = num;
			this.WritePos = num4;
			return this.inflate_flush(z, r);
			Block_8:
			this.mode = InflateBlockMode.BAD;
			z.msg = "invalid stored block lengths";
			r = -3;
			this.BitB = num3;
			this.BitK = i;
			z.avail_in = num2;
			z.total_in += (long)(num - z.next_in_index);
			z.next_in_index = num;
			this.WritePos = num4;
			return this.inflate_flush(z, r);
			Block_11:
			this.BitB = num3;
			this.BitK = i;
			z.avail_in = num2;
			z.total_in += (long)(num - z.next_in_index);
			z.next_in_index = num;
			this.WritePos = num4;
			return this.inflate_flush(z, r);
			Block_21:
			this.BitB = num3;
			this.BitK = i;
			z.avail_in = num2;
			z.total_in += (long)(num - z.next_in_index);
			z.next_in_index = num;
			this.WritePos = num4;
			return this.inflate_flush(z, r);
			IL_515:
			this.BitB = num3;
			this.BitK = i;
			z.avail_in = num2;
			z.total_in += (long)(num - z.next_in_index);
			z.next_in_index = num;
			this.WritePos = num4;
			return this.inflate_flush(z, r);
			IL_5A3:
			this.mode = InflateBlockMode.BAD;
			z.msg = "too many length or distance symbols";
			r = -3;
			this.BitB = num3;
			this.BitK = i;
			z.avail_in = num2;
			z.total_in += (long)(num - z.next_in_index);
			z.next_in_index = num;
			this.WritePos = num4;
			return this.inflate_flush(z, r);
			IL_646:
			this.BitB = num3;
			this.BitK = i;
			z.avail_in = num2;
			z.total_in += (long)(num - z.next_in_index);
			z.next_in_index = num;
			this.WritePos = num4;
			return this.inflate_flush(z, r);
			Block_33:
			r = num6;
			if (r == -3)
			{
				this.blens = null;
				this.mode = InflateBlockMode.BAD;
			}
			this.BitB = num3;
			this.BitK = i;
			z.avail_in = num2;
			z.total_in += (long)(num - z.next_in_index);
			z.next_in_index = num;
			this.WritePos = num4;
			return this.inflate_flush(z, r);
			IL_7F0:
			this.BitB = num3;
			this.BitK = i;
			z.avail_in = num2;
			z.total_in += (long)(num - z.next_in_index);
			z.next_in_index = num;
			this.WritePos = num4;
			return this.inflate_flush(z, r);
			IL_8F4:
			this.BitB = num3;
			this.BitK = i;
			z.avail_in = num2;
			z.total_in += (long)(num - z.next_in_index);
			z.next_in_index = num;
			this.WritePos = num4;
			return this.inflate_flush(z, r);
			IL_9BE:
			this.blens = null;
			this.mode = InflateBlockMode.BAD;
			z.msg = "invalid bit length repeat";
			r = -3;
			this.BitB = num3;
			this.BitK = i;
			z.avail_in = num2;
			z.total_in += (long)(num - z.next_in_index);
			z.next_in_index = num;
			this.WritePos = num4;
			return this.inflate_flush(z, r);
			Block_47:
			if (num6 == -3)
			{
				this.blens = null;
				this.mode = InflateBlockMode.BAD;
			}
			r = num6;
			this.BitB = num3;
			this.BitK = i;
			z.avail_in = num2;
			z.total_in += (long)(num - z.next_in_index);
			z.next_in_index = num;
			this.WritePos = num4;
			return this.inflate_flush(z, r);
			Block_49:
			return this.inflate_flush(z, r);
			IL_C1A:
			this.mode = InflateBlockMode.DRY;
			IL_C21:
			this.WritePos = num4;
			r = this.inflate_flush(z, r);
			num4 = this.WritePos;
			int num10 = (num4 < this.ReadPos) ? (this.ReadPos - num4 - 1) : (this.End - num4);
			if (this.ReadPos != this.WritePos)
			{
				this.BitB = num3;
				this.BitK = i;
				z.avail_in = num2;
				z.total_in += (long)(num - z.next_in_index);
				z.next_in_index = num;
				this.WritePos = num4;
				return this.inflate_flush(z, r);
			}
			this.mode = InflateBlockMode.DONE;
			IL_CB6:
			r = 1;
			this.BitB = num3;
			this.BitK = i;
			z.avail_in = num2;
			z.total_in += (long)(num - z.next_in_index);
			z.next_in_index = num;
			this.WritePos = num4;
			return this.inflate_flush(z, r);
			IL_CFD:
			r = -3;
			this.BitB = num3;
			this.BitK = i;
			z.avail_in = num2;
			z.total_in += (long)(num - z.next_in_index);
			z.next_in_index = num;
			this.WritePos = num4;
			return this.inflate_flush(z, r);
		}

		// Token: 0x060007E0 RID: 2016 RVA: 0x000305FD File Offset: 0x0002F5FD
		internal void free(ZStream z)
		{
			this.reset(z, null);
			this.Window = null;
			this.hufts = null;
		}

		// Token: 0x060007E1 RID: 2017 RVA: 0x00030618 File Offset: 0x0002F618
		internal void set_dictionary(byte[] d, int start, int n)
		{
			Array.Copy(d, start, this.Window, 0, n);
			this.WritePos = n;
			this.ReadPos = n;
		}

		// Token: 0x060007E2 RID: 2018 RVA: 0x00030644 File Offset: 0x0002F644
		internal int sync_point()
		{
			if (this.mode != InflateBlockMode.LENS)
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x060007E3 RID: 2019 RVA: 0x00030654 File Offset: 0x0002F654
		internal int inflate_flush(ZStream z, int r)
		{
			int num = z.next_out_index;
			int num2 = this.ReadPos;
			int num3 = ((num2 <= this.WritePos) ? this.WritePos : this.End) - num2;
			if (num3 > z.avail_out)
			{
				num3 = z.avail_out;
			}
			if (num3 != 0 && r == -5)
			{
				r = 0;
			}
			z.avail_out -= num3;
			z.total_out += (long)num3;
			if (this.needCheck)
			{
				z.adler = (this.check = Adler32.GetAdler32Checksum(this.check, this.Window, num2, num3));
			}
			Array.Copy(this.Window, num2, z.next_out, num, num3);
			num += num3;
			num2 += num3;
			if (num2 == this.End)
			{
				num2 = 0;
				if (this.WritePos == this.End)
				{
					this.WritePos = 0;
				}
				num3 = this.WritePos - num2;
				if (num3 > z.avail_out)
				{
					num3 = z.avail_out;
				}
				if (num3 != 0 && r == -5)
				{
					r = 0;
				}
				z.avail_out -= num3;
				z.total_out += (long)num3;
				if (this.needCheck)
				{
					z.adler = (this.check = Adler32.GetAdler32Checksum(this.check, this.Window, num2, num3));
				}
				Array.Copy(this.Window, num2, z.next_out, num, num3);
				num += num3;
				num2 += num3;
			}
			z.next_out_index = num;
			this.ReadPos = num2;
			return r;
		}

		// Token: 0x0400049F RID: 1183
		private const int MANY = 1440;

		// Token: 0x040004A0 RID: 1184
		private InflateBlockMode mode;

		// Token: 0x040004A1 RID: 1185
		private int left;

		// Token: 0x040004A2 RID: 1186
		private int table;

		// Token: 0x040004A3 RID: 1187
		private int index;

		// Token: 0x040004A4 RID: 1188
		private int[] blens;

		// Token: 0x040004A5 RID: 1189
		private int[] bb = new int[1];

		// Token: 0x040004A6 RID: 1190
		private int[] tb = new int[1];

		// Token: 0x040004A7 RID: 1191
		private InfCodes codes;

		// Token: 0x040004A8 RID: 1192
		private int last;

		// Token: 0x040004A9 RID: 1193
		private int bitk;

		// Token: 0x040004AA RID: 1194
		private int bitb;

		// Token: 0x040004AB RID: 1195
		private int[] hufts;

		// Token: 0x040004AC RID: 1196
		private byte[] window;

		// Token: 0x040004AD RID: 1197
		private int end;

		// Token: 0x040004AE RID: 1198
		private int read;

		// Token: 0x040004AF RID: 1199
		private int write;

		// Token: 0x040004B0 RID: 1200
		private bool needCheck;

		// Token: 0x040004B1 RID: 1201
		private long check;
	}
}
