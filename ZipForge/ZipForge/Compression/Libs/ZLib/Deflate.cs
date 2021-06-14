using System;

namespace ComponentAce.Compression.Libs.ZLib
{
	// Token: 0x020000AB RID: 171
	internal sealed class Deflate
	{
		// Token: 0x17000151 RID: 337
		// (get) Token: 0x0600079E RID: 1950 RVA: 0x0002D56C File Offset: 0x0002C56C
		// (set) Token: 0x0600079F RID: 1951 RVA: 0x0002D574 File Offset: 0x0002C574
		public int level
		{
			get
			{
				return this._level;
			}
			set
			{
				this._level = value;
			}
		}

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x060007A0 RID: 1952 RVA: 0x0002D57D File Offset: 0x0002C57D
		// (set) Token: 0x060007A1 RID: 1953 RVA: 0x0002D585 File Offset: 0x0002C585
		public int Pending
		{
			get
			{
				return this.pending;
			}
			set
			{
				this.pending = value;
			}
		}

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x060007A2 RID: 1954 RVA: 0x0002D58E File Offset: 0x0002C58E
		// (set) Token: 0x060007A3 RID: 1955 RVA: 0x0002D596 File Offset: 0x0002C596
		public byte[] Pending_buf
		{
			get
			{
				return this.pending_buf;
			}
			set
			{
				this.pending_buf = value;
			}
		}

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x060007A4 RID: 1956 RVA: 0x0002D59F File Offset: 0x0002C59F
		// (set) Token: 0x060007A5 RID: 1957 RVA: 0x0002D5A7 File Offset: 0x0002C5A7
		public int Pending_out
		{
			get
			{
				return this.pending_out;
			}
			set
			{
				this.pending_out = value;
			}
		}

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x060007A6 RID: 1958 RVA: 0x0002D5B0 File Offset: 0x0002C5B0
		// (set) Token: 0x060007A7 RID: 1959 RVA: 0x0002D5B8 File Offset: 0x0002C5B8
		public int NoHeader
		{
			get
			{
				return this.noheader;
			}
			set
			{
				this.noheader = value;
			}
		}

		// Token: 0x060007A8 RID: 1960 RVA: 0x0002D5C4 File Offset: 0x0002C5C4
		internal Deflate()
		{
			this.dyn_ltree = new short[1146];
			this.dyn_dtree = new short[122];
			this.bl_tree = new short[78];
		}

		// Token: 0x060007A9 RID: 1961 RVA: 0x0002D650 File Offset: 0x0002C650
		private void lm_init()
		{
			this.window_size = 2 * this.w_size;
			Array.Clear(this.head, 0, this.hash_size);
			this.max_lazy_match = Deflate.config_table[this.level].max_lazy;
			this.good_match = Deflate.config_table[this.level].good_length;
			this.nice_match = Deflate.config_table[this.level].nice_length;
			this.max_chain_length = Deflate.config_table[this.level].max_chain;
			this.strstart = 0;
			this.block_start = 0;
			this.lookahead = 0;
			this.match_length = (this.prev_length = 2);
			this.match_available = 0;
			this.ins_h = 0;
		}

		// Token: 0x060007AA RID: 1962 RVA: 0x0002D70C File Offset: 0x0002C70C
		private void tr_init()
		{
			this.l_desc.DynTree = this.dyn_ltree;
			this.l_desc.StatDesc = StaticTree.static_l_desc;
			this.d_desc.DynTree = this.dyn_dtree;
			this.d_desc.StatDesc = StaticTree.static_d_desc;
			this.bl_desc.DynTree = this.bl_tree;
			this.bl_desc.StatDesc = StaticTree.static_bl_desc;
			this.bi_buf = 0;
			this.bi_valid = 0;
			this.last_eob_len = 8;
			this.init_block();
		}

		// Token: 0x060007AB RID: 1963 RVA: 0x0002D798 File Offset: 0x0002C798
		private void init_block()
		{
			for (int i = 0; i < 286; i++)
			{
				this.dyn_ltree[i * 2] = 0;
			}
			for (int j = 0; j < 30; j++)
			{
				this.dyn_dtree[j * 2] = 0;
			}
			for (int k = 0; k < 19; k++)
			{
				this.bl_tree[k * 2] = 0;
			}
			this.dyn_ltree[512] = 1;
			this.opt_len = (this.static_len = 0);
			this.last_lit = (this.matches = 0);
		}

		// Token: 0x060007AC RID: 1964 RVA: 0x0002D820 File Offset: 0x0002C820
		internal void pqdownheap(short[] tree, int k)
		{
			int num = this.heap[k];
			for (int i = k << 1; i <= this.heap_len; i <<= 1)
			{
				if (i < this.heap_len && Deflate.smaller(tree, this.heap[i + 1], this.heap[i], this.depth))
				{
					i++;
				}
				if (Deflate.smaller(tree, num, this.heap[i], this.depth))
				{
					break;
				}
				this.heap[k] = this.heap[i];
				k = i;
			}
			this.heap[k] = num;
		}

		// Token: 0x060007AD RID: 1965 RVA: 0x0002D8A9 File Offset: 0x0002C8A9
		internal static bool smaller(short[] tree, int n, int m, byte[] depth)
		{
			return tree[n * 2] < tree[m * 2] || (tree[n * 2] == tree[m * 2] && depth[n] <= depth[m]);
		}

		// Token: 0x060007AE RID: 1966 RVA: 0x0002D8D4 File Offset: 0x0002C8D4
		private void scan_tree(short[] tree, int max_code)
		{
			int num = -1;
			int num2 = (int)tree[1];
			int num3 = 0;
			int num4 = 7;
			int num5 = 4;
			if (num2 == 0)
			{
				num4 = 138;
				num5 = 3;
			}
			tree[(max_code + 1) * 2 + 1] = (short)ZLibUtil.Identity(65535L);
			for (int i = 0; i <= max_code; i++)
			{
				int num6 = num2;
				num2 = (int)tree[(i + 1) * 2 + 1];
				if (++num3 >= num4 || num6 != num2)
				{
					if (num3 < num5)
					{
						this.bl_tree[num6 * 2] = (short)((int)this.bl_tree[num6 * 2] + num3);
					}
					else if (num6 != 0)
					{
						if (num6 != num)
						{
							short[] array = this.bl_tree;
							int num7 = num6 * 2;
							array[num7] += 1;
						}
						short[] array2 = this.bl_tree;
						int num8 = 32;
						array2[num8] += 1;
					}
					else if (num3 <= 10)
					{
						short[] array3 = this.bl_tree;
						int num9 = 34;
						array3[num9] += 1;
					}
					else
					{
						short[] array4 = this.bl_tree;
						int num10 = 36;
						array4[num10] += 1;
					}
					num3 = 0;
					num = num6;
					if (num2 == 0)
					{
						num4 = 138;
						num5 = 3;
					}
					else if (num6 == num2)
					{
						num4 = 6;
						num5 = 3;
					}
					else
					{
						num4 = 7;
						num5 = 4;
					}
				}
			}
		}

		// Token: 0x060007AF RID: 1967 RVA: 0x0002DA08 File Offset: 0x0002CA08
		private int build_bl_tree()
		{
			this.scan_tree(this.dyn_ltree, this.l_desc.MaxCode);
			this.scan_tree(this.dyn_dtree, this.d_desc.MaxCode);
			this.bl_desc.build_tree(this);
			int num = 18;
			while (num >= 3 && this.bl_tree[(int)(ZLibUtil.bl_order[num] * 2 + 1)] == 0)
			{
				num--;
			}
			this.opt_len += 3 * (num + 1) + 5 + 5 + 4;
			return num;
		}

		// Token: 0x060007B0 RID: 1968 RVA: 0x0002DA8C File Offset: 0x0002CA8C
		private void send_all_trees(int lcodes, int dcodes, int blcodes)
		{
			this.send_bits(lcodes - 257, 5);
			this.send_bits(dcodes - 1, 5);
			this.send_bits(blcodes - 4, 4);
			for (int i = 0; i < blcodes; i++)
			{
				this.send_bits((int)this.bl_tree[(int)(ZLibUtil.bl_order[i] * 2 + 1)], 3);
			}
			this.send_tree(this.dyn_ltree, lcodes - 1);
			this.send_tree(this.dyn_dtree, dcodes - 1);
		}

		// Token: 0x060007B1 RID: 1969 RVA: 0x0002DB00 File Offset: 0x0002CB00
		private void send_tree(short[] tree, int max_code)
		{
			int num = -1;
			int num2 = (int)tree[1];
			int num3 = 0;
			int num4 = 7;
			int num5 = 4;
			if (num2 == 0)
			{
				num4 = 138;
				num5 = 3;
			}
			for (int i = 0; i <= max_code; i++)
			{
				int num6 = num2;
				num2 = (int)tree[(i + 1) * 2 + 1];
				if (++num3 >= num4 || num6 != num2)
				{
					if (num3 < num5)
					{
						do
						{
							this.send_code(num6, this.bl_tree);
						}
						while (--num3 != 0);
					}
					else if (num6 != 0)
					{
						if (num6 != num)
						{
							this.send_code(num6, this.bl_tree);
							num3--;
						}
						this.send_code(16, this.bl_tree);
						this.send_bits(num3 - 3, 2);
					}
					else if (num3 <= 10)
					{
						this.send_code(17, this.bl_tree);
						this.send_bits(num3 - 3, 3);
					}
					else
					{
						this.send_code(18, this.bl_tree);
						this.send_bits(num3 - 11, 7);
					}
					num3 = 0;
					num = num6;
					if (num2 == 0)
					{
						num4 = 138;
						num5 = 3;
					}
					else if (num6 == num2)
					{
						num4 = 6;
						num5 = 3;
					}
					else
					{
						num4 = 7;
						num5 = 4;
					}
				}
			}
		}

		// Token: 0x060007B2 RID: 1970 RVA: 0x0002DC0D File Offset: 0x0002CC0D
		private void put_byte(byte[] p, int start, int len)
		{
			Array.Copy(p, start, this.Pending_buf, this.pending, len);
			this.pending += len;
		}

		// Token: 0x060007B3 RID: 1971 RVA: 0x0002DC34 File Offset: 0x0002CC34
		private void put_byte(byte c)
		{
			this.Pending_buf[this.pending++] = c;
		}

		// Token: 0x060007B4 RID: 1972 RVA: 0x0002DC5A File Offset: 0x0002CC5A
		private void put_short(int w)
		{
			this.put_byte((byte)w);
			this.put_byte((byte)ZLibUtil.URShift(w, 8));
		}

		// Token: 0x060007B5 RID: 1973 RVA: 0x0002DC72 File Offset: 0x0002CC72
		private void putShortMSB(int b)
		{
			this.put_byte((byte)(b >> 8));
			this.put_byte((byte)b);
		}

		// Token: 0x060007B6 RID: 1974 RVA: 0x0002DC86 File Offset: 0x0002CC86
		private void send_code(int c, short[] tree)
		{
			this.send_bits((int)tree[c * 2] & 65535, (int)tree[c * 2 + 1] & 65535);
		}

		// Token: 0x060007B7 RID: 1975 RVA: 0x0002DCA8 File Offset: 0x0002CCA8
		private void send_bits(int value_Renamed, int length)
		{
			if (this.bi_valid > 16 - length)
			{
				this.bi_buf = (short)((ushort)this.bi_buf | (ushort)(value_Renamed << this.bi_valid & 65535));
				this.put_short((int)this.bi_buf);
				this.bi_buf = (short)ZLibUtil.URShift(value_Renamed, 16 - this.bi_valid);
				this.bi_valid += length - 16;
				return;
			}
			this.bi_buf = (short)((ushort)this.bi_buf | (ushort)(value_Renamed << this.bi_valid & 65535));
			this.bi_valid += length;
		}

		// Token: 0x060007B8 RID: 1976 RVA: 0x0002DD4C File Offset: 0x0002CD4C
		private void _tr_align()
		{
			this.send_bits(2, 3);
			this.send_code(256, StaticTree.static_ltree);
			this.bi_flush();
			if (1 + this.last_eob_len + 10 - this.bi_valid < 9)
			{
				this.send_bits(2, 3);
				this.send_code(256, StaticTree.static_ltree);
				this.bi_flush();
			}
			this.last_eob_len = 7;
		}

		// Token: 0x060007B9 RID: 1977 RVA: 0x0002DDB4 File Offset: 0x0002CDB4
		private bool _tr_tally(int dist, int lc)
		{
			this.Pending_buf[this.d_buf + this.last_lit * 2] = (byte)ZLibUtil.URShift(dist, 8);
			this.Pending_buf[this.d_buf + this.last_lit * 2 + 1] = (byte)dist;
			this.Pending_buf[this.l_buf + this.last_lit] = (byte)lc;
			this.last_lit++;
			if (dist == 0)
			{
				short[] array = this.dyn_ltree;
				int num = lc * 2;
				array[num] += 1;
			}
			else
			{
				this.matches++;
				dist--;
				short[] array2 = this.dyn_ltree;
				int num2 = ((int)ZLibUtil._length_code[lc] + 256 + 1) * 2;
				array2[num2] += 1;
				short[] array3 = this.dyn_dtree;
				int num3 = Tree.d_code(dist) * 2;
				array3[num3] += 1;
			}
			if ((this.last_lit & 8191) == 0 && this.level > 2)
			{
				int num4 = this.last_lit * 8;
				int num5 = this.strstart - this.block_start;
				for (int i = 0; i < 30; i++)
				{
					num4 = (int)((long)num4 + (long)this.dyn_dtree[i * 2] * (5L + (long)ZLibUtil.extra_dbits[i]));
				}
				num4 = ZLibUtil.URShift(num4, 3);
				if (this.matches < this.last_lit / 2 && num4 < num5 / 2)
				{
					return true;
				}
			}
			return this.last_lit == this.lit_bufsize - 1;
		}

		// Token: 0x060007BA RID: 1978 RVA: 0x0002DF24 File Offset: 0x0002CF24
		private void compress_block(short[] ltree, short[] dtree)
		{
			int num = 0;
			if (this.last_lit != 0)
			{
				do
				{
					int num2 = ((int)this.Pending_buf[this.d_buf + num * 2] << 8 & 65280) | (int)(this.Pending_buf[this.d_buf + num * 2 + 1] & byte.MaxValue);
					int num3 = (int)(this.Pending_buf[this.l_buf + num] & byte.MaxValue);
					num++;
					if (num2 == 0)
					{
						this.send_code(num3, ltree);
					}
					else
					{
						int num4 = (int)ZLibUtil._length_code[num3];
						this.send_code(num4 + 256 + 1, ltree);
						int num5 = ZLibUtil.extra_lbits[num4];
						if (num5 != 0)
						{
							num3 -= ZLibUtil.base_length[num4];
							this.send_bits(num3, num5);
						}
						num2--;
						num4 = Tree.d_code(num2);
						this.send_code(num4, dtree);
						num5 = ZLibUtil.extra_dbits[num4];
						if (num5 != 0)
						{
							num2 -= ZLibUtil.base_dist[num4];
							this.send_bits(num2, num5);
						}
					}
				}
				while (num < this.last_lit);
			}
			this.send_code(256, ltree);
			this.last_eob_len = (int)ltree[513];
		}

		// Token: 0x060007BB RID: 1979 RVA: 0x0002E02C File Offset: 0x0002D02C
		private void set_data_type()
		{
			int i = 0;
			int num = 0;
			int num2 = 0;
			while (i < 7)
			{
				num2 += (int)this.dyn_ltree[i * 2];
				i++;
			}
			while (i < 128)
			{
				num += (int)this.dyn_ltree[i * 2];
				i++;
			}
			while (i < 256)
			{
				num2 += (int)this.dyn_ltree[i * 2];
				i++;
			}
			this.data_type = ((num2 > ZLibUtil.URShift(num, 2)) ? BlockType.Z_BINARY : BlockType.Z_ASCII);
		}

		// Token: 0x060007BC RID: 1980 RVA: 0x0002E0A0 File Offset: 0x0002D0A0
		private void bi_flush()
		{
			if (this.bi_valid == 16)
			{
				this.put_short((int)this.bi_buf);
				this.bi_buf = 0;
				this.bi_valid = 0;
				return;
			}
			if (this.bi_valid >= 8)
			{
				this.put_byte((byte)this.bi_buf);
				this.bi_buf = (short)ZLibUtil.URShift((int)this.bi_buf, 8);
				this.bi_valid -= 8;
			}
		}

		// Token: 0x060007BD RID: 1981 RVA: 0x0002E109 File Offset: 0x0002D109
		private void bi_windup()
		{
			if (this.bi_valid > 8)
			{
				this.put_short((int)this.bi_buf);
			}
			else if (this.bi_valid > 0)
			{
				this.put_byte((byte)this.bi_buf);
			}
			this.bi_buf = 0;
			this.bi_valid = 0;
		}

		// Token: 0x060007BE RID: 1982 RVA: 0x0002E146 File Offset: 0x0002D146
		private void copy_block(int buf, int len, bool header)
		{
			this.bi_windup();
			this.last_eob_len = 8;
			if (header)
			{
				this.put_short((int)((short)len));
				this.put_short((int)((short)(~(short)len)));
			}
			this.put_byte(this.window, buf, len);
		}

		// Token: 0x060007BF RID: 1983 RVA: 0x0002E177 File Offset: 0x0002D177
		private void flush_block_only(bool eof)
		{
			this._tr_flush_block((this.block_start >= 0) ? this.block_start : -1, this.strstart - this.block_start, eof);
			this.block_start = this.strstart;
			this.strm.flush_pending();
		}

		// Token: 0x060007C0 RID: 1984 RVA: 0x0002E1B8 File Offset: 0x0002D1B8
		private int deflate_stored(int flush)
		{
			int num = 65535;
			if (num > this.pending_buf_size - 5)
			{
				num = this.pending_buf_size - 5;
			}
			for (;;)
			{
				if (this.lookahead <= 1)
				{
					this.fill_window();
					if (this.lookahead == 0 && flush == 0)
					{
						break;
					}
					if (this.lookahead == 0)
					{
						goto IL_D7;
					}
				}
				this.strstart += this.lookahead;
				this.lookahead = 0;
				int num2 = this.block_start + num;
				if (this.strstart == 0 || this.strstart >= num2)
				{
					this.lookahead = this.strstart - num2;
					this.strstart = num2;
					this.flush_block_only(false);
					if (this.strm.avail_out == 0)
					{
						return 0;
					}
				}
				if (this.strstart - this.block_start >= this.w_size - 262)
				{
					this.flush_block_only(false);
					if (this.strm.avail_out == 0)
					{
						return 0;
					}
				}
			}
			return 0;
			IL_D7:
			this.flush_block_only(flush == 4);
			if (this.strm.avail_out == 0)
			{
				if (flush != 4)
				{
					return 0;
				}
				return 2;
			}
			else
			{
				if (flush != 4)
				{
					return 1;
				}
				return 3;
			}
		}

		// Token: 0x060007C1 RID: 1985 RVA: 0x0002E2C2 File Offset: 0x0002D2C2
		private void _tr_stored_block(int buf, int stored_len, bool eof)
		{
			this.send_bits(eof ? 1 : 0, 3);
			this.copy_block(buf, stored_len, true);
		}

		// Token: 0x060007C2 RID: 1986 RVA: 0x0002E2DC File Offset: 0x0002D2DC
		private void _tr_flush_block(int buf, int stored_len, bool eof)
		{
			int num = 0;
			int num2;
			int num3;
			if (this.level > 0)
			{
				if (this.data_type == BlockType.Z_UNKNOWN)
				{
					this.set_data_type();
				}
				this.l_desc.build_tree(this);
				this.d_desc.build_tree(this);
				num = this.build_bl_tree();
				num2 = ZLibUtil.URShift(this.opt_len + 3 + 7, 3);
				num3 = ZLibUtil.URShift(this.static_len + 3 + 7, 3);
				if (num3 <= num2)
				{
					num2 = num3;
				}
			}
			else
			{
				num3 = (num2 = stored_len + 5);
			}
			if (stored_len + 4 <= num2 && buf != -1)
			{
				this._tr_stored_block(buf, stored_len, eof);
			}
			else if (num3 == num2)
			{
				this.send_bits(2 + (eof ? 1 : 0), 3);
				this.compress_block(StaticTree.static_ltree, StaticTree.static_dtree);
			}
			else
			{
				this.send_bits(4 + (eof ? 1 : 0), 3);
				this.send_all_trees(this.l_desc.MaxCode + 1, this.d_desc.MaxCode + 1, num + 1);
				this.compress_block(this.dyn_ltree, this.dyn_dtree);
			}
			this.init_block();
			if (eof)
			{
				this.bi_windup();
			}
		}

		// Token: 0x060007C3 RID: 1987 RVA: 0x0002E3E4 File Offset: 0x0002D3E4
		private void fill_window()
		{
			for (;;)
			{
				int num = this.window_size - this.lookahead - this.strstart;
				int num2;
				if (num == 0 && this.strstart == 0 && this.lookahead == 0)
				{
					num = this.w_size;
				}
				else if (num == -1)
				{
					num--;
				}
				else if (this.strstart >= this.w_size + this.w_size - 262)
				{
					Array.Copy(this.window, this.w_size, this.window, 0, this.w_size);
					this.match_start -= this.w_size;
					this.strstart -= this.w_size;
					this.block_start -= this.w_size;
					num2 = this.hash_size;
					int num3 = num2;
					do
					{
						int num4 = (int)this.head[--num3] & 65535;
						this.head[num3] = (short)((num4 >= this.w_size) ? (num4 - this.w_size) : 0);
					}
					while (--num2 != 0);
					num2 = this.w_size;
					num3 = num2;
					do
					{
						int num4 = (int)this.prev[--num3] & 65535;
						this.prev[num3] = (short)((num4 >= this.w_size) ? (num4 - this.w_size) : 0);
					}
					while (--num2 != 0);
					num += this.w_size;
				}
				if (this.strm.avail_in == 0)
				{
					break;
				}
				num2 = this.strm.read_buf(this.window, this.strstart + this.lookahead, num);
				this.lookahead += num2;
				if (this.lookahead >= 3)
				{
					this.ins_h = (int)(this.window[this.strstart] & byte.MaxValue);
					this.ins_h = ((this.ins_h << this.hash_shift ^ (int)(this.window[this.strstart + 1] & byte.MaxValue)) & this.hash_mask);
				}
				if (this.lookahead >= 262 || this.strm.avail_in == 0)
				{
					return;
				}
			}
		}

		// Token: 0x060007C4 RID: 1988 RVA: 0x0002E5E0 File Offset: 0x0002D5E0
		private int deflate_fast(int flush)
		{
			int num = 0;
			for (;;)
			{
				if (this.lookahead < 262)
				{
					this.fill_window();
					if (this.lookahead < 262 && flush == 0)
					{
						break;
					}
					if (this.lookahead == 0)
					{
						goto IL_2C6;
					}
				}
				if (this.lookahead >= 3)
				{
					this.ins_h = ((this.ins_h << this.hash_shift ^ (int)(this.window[this.strstart + 2] & byte.MaxValue)) & this.hash_mask);
					num = ((int)this.head[this.ins_h] & 65535);
					this.prev[this.strstart & this.w_mask] = this.head[this.ins_h];
					this.head[this.ins_h] = (short)this.strstart;
				}
				if ((long)num != 0L && (this.strstart - num & 65535) <= this.w_size - 262 && this.strategy != CompressionStrategy.Z_HUFFMAN_ONLY)
				{
					this.match_length = this.longest_match(num);
				}
				bool flag;
				if (this.match_length >= 3)
				{
					flag = this._tr_tally(this.strstart - this.match_start, this.match_length - 3);
					this.lookahead -= this.match_length;
					if (this.match_length <= this.max_lazy_match && this.lookahead >= 3)
					{
						this.match_length--;
						do
						{
							this.strstart++;
							this.ins_h = ((this.ins_h << this.hash_shift ^ (int)(this.window[this.strstart + 2] & byte.MaxValue)) & this.hash_mask);
							num = ((int)this.head[this.ins_h] & 65535);
							this.prev[this.strstart & this.w_mask] = this.head[this.ins_h];
							this.head[this.ins_h] = (short)this.strstart;
						}
						while (--this.match_length != 0);
						this.strstart++;
					}
					else
					{
						this.strstart += this.match_length;
						this.match_length = 0;
						this.ins_h = (int)(this.window[this.strstart] & byte.MaxValue);
						this.ins_h = ((this.ins_h << this.hash_shift ^ (int)(this.window[this.strstart + 1] & byte.MaxValue)) & this.hash_mask);
					}
				}
				else
				{
					flag = this._tr_tally(0, (int)(this.window[this.strstart] & byte.MaxValue));
					this.lookahead--;
					this.strstart++;
				}
				if (flag)
				{
					this.flush_block_only(false);
					if (this.strm.avail_out == 0)
					{
						return 0;
					}
				}
			}
			return 0;
			IL_2C6:
			this.flush_block_only(flush == 4);
			if (this.strm.avail_out == 0)
			{
				if (flush == 4)
				{
					return 2;
				}
				return 0;
			}
			else
			{
				if (flush != 4)
				{
					return 1;
				}
				return 3;
			}
		}

		// Token: 0x060007C5 RID: 1989 RVA: 0x0002E8DC File Offset: 0x0002D8DC
		private int deflate_slow(int flush)
		{
			int num = 0;
			for (;;)
			{
				if (this.lookahead < 262)
				{
					this.fill_window();
					if (this.lookahead < 262 && flush == 0)
					{
						break;
					}
					if (this.lookahead == 0)
					{
						goto IL_325;
					}
				}
				if (this.lookahead >= 3)
				{
					this.ins_h = ((this.ins_h << this.hash_shift ^ (int)(this.window[this.strstart + 2] & byte.MaxValue)) & this.hash_mask);
					num = ((int)this.head[this.ins_h] & 65535);
					this.prev[this.strstart & this.w_mask] = this.head[this.ins_h];
					this.head[this.ins_h] = (short)this.strstart;
				}
				this.prev_length = this.match_length;
				this.prev_match = this.match_start;
				this.match_length = 2;
				if (num != 0 && this.prev_length < this.max_lazy_match && (this.strstart - num & 65535) <= this.w_size - 262)
				{
					if (this.strategy != CompressionStrategy.Z_HUFFMAN_ONLY)
					{
						this.match_length = this.longest_match(num);
					}
					if (this.match_length <= 5 && (this.strategy == CompressionStrategy.Z_FILTERED || (this.match_length == 3 && this.strstart - this.match_start > 4096)))
					{
						this.match_length = 2;
					}
				}
				if (this.prev_length >= 3 && this.match_length <= this.prev_length)
				{
					int num2 = this.strstart + this.lookahead - 3;
					bool flag = this._tr_tally(this.strstart - 1 - this.prev_match, this.prev_length - 3);
					this.lookahead -= this.prev_length - 1;
					this.prev_length -= 2;
					do
					{
						if (++this.strstart <= num2)
						{
							this.ins_h = ((this.ins_h << this.hash_shift ^ (int)(this.window[this.strstart + 2] & byte.MaxValue)) & this.hash_mask);
							num = ((int)this.head[this.ins_h] & 65535);
							this.prev[this.strstart & this.w_mask] = this.head[this.ins_h];
							this.head[this.ins_h] = (short)this.strstart;
						}
					}
					while (--this.prev_length != 0);
					this.match_available = 0;
					this.match_length = 2;
					this.strstart++;
					if (flag)
					{
						this.flush_block_only(false);
						if (this.strm.avail_out == 0)
						{
							return 0;
						}
					}
				}
				else if (this.match_available != 0)
				{
					bool flag = this._tr_tally(0, (int)(this.window[this.strstart - 1] & byte.MaxValue));
					if (flag)
					{
						this.flush_block_only(false);
					}
					this.strstart++;
					this.lookahead--;
					if (this.strm.avail_out == 0)
					{
						return 0;
					}
				}
				else
				{
					this.match_available = 1;
					this.strstart++;
					this.lookahead--;
				}
			}
			return 0;
			IL_325:
			if (this.match_available != 0)
			{
				bool flag = this._tr_tally(0, (int)(this.window[this.strstart - 1] & byte.MaxValue));
				this.match_available = 0;
			}
			this.flush_block_only(flush == 4);
			if (this.strm.avail_out == 0)
			{
				if (flush == 4)
				{
					return 2;
				}
				return 0;
			}
			else
			{
				if (flush != 4)
				{
					return 1;
				}
				return 3;
			}
		}

		// Token: 0x060007C6 RID: 1990 RVA: 0x0002EC60 File Offset: 0x0002DC60
		private int longest_match(int cur_match)
		{
			int num = this.max_chain_length;
			int num2 = this.strstart;
			int num3 = this.prev_length;
			int num4 = (this.strstart > this.w_size - 262) ? (this.strstart - (this.w_size - 262)) : 0;
			int num5 = this.nice_match;
			int num6 = this.w_mask;
			int num7 = this.strstart + 258;
			byte b = this.window[num2 + num3 - 1];
			byte b2 = this.window[num2 + num3];
			if (this.prev_length >= this.good_match)
			{
				num >>= 2;
			}
			if (num5 > this.lookahead)
			{
				num5 = this.lookahead;
			}
			do
			{
				int num8 = cur_match;
				if (this.window[num8 + num3] == b2 && this.window[num8 + num3 - 1] == b && this.window[num8] == this.window[num2] && this.window[++num8] == this.window[num2 + 1])
				{
					num2 += 2;
					num8++;
					while (this.window[++num2] == this.window[++num8] && this.window[++num2] == this.window[++num8] && this.window[++num2] == this.window[++num8] && this.window[++num2] == this.window[++num8] && this.window[++num2] == this.window[++num8] && this.window[++num2] == this.window[++num8] && this.window[++num2] == this.window[++num8] && this.window[++num2] == this.window[++num8] && num2 < num7)
					{
					}
					int num9 = 258 - (num7 - num2);
					num2 = num7 - 258;
					if (num9 > num3)
					{
						this.match_start = cur_match;
						num3 = num9;
						if (num9 >= num5)
						{
							break;
						}
						b = this.window[num2 + num3 - 1];
						b2 = this.window[num2 + num3];
					}
				}
			}
			while ((cur_match = ((int)this.prev[cur_match & num6] & 65535)) > num4 && --num != 0);
			if (num3 <= this.lookahead)
			{
				return num3;
			}
			return this.lookahead;
		}

		// Token: 0x060007C7 RID: 1991 RVA: 0x0002EEC7 File Offset: 0x0002DEC7
		internal int deflateInit(ZStream strm, int level, int bits)
		{
			return this.deflateInit2(strm, level, 8, bits, 8, CompressionStrategy.Z_DEFAULT_STRATEGY);
		}

		// Token: 0x060007C8 RID: 1992 RVA: 0x0002EED5 File Offset: 0x0002DED5
		internal int deflateInit(ZStream strm, int level)
		{
			return this.deflateInit(strm, level, 15);
		}

		// Token: 0x060007C9 RID: 1993 RVA: 0x0002EEE4 File Offset: 0x0002DEE4
		internal int deflateInit2(ZStream strm, int level, int method, int windowBits, int memLevel, CompressionStrategy strategy)
		{
			int noHeader = 0;
			strm.msg = null;
			if (level == -1)
			{
				level = 6;
			}
			if (windowBits < 0)
			{
				noHeader = 1;
				windowBits = -windowBits;
			}
			if (memLevel < 1 || memLevel > 9 || method != 8 || windowBits < 9 || windowBits > 15 || level < 0 || level > 9 || strategy < CompressionStrategy.Z_DEFAULT_STRATEGY || strategy > CompressionStrategy.Z_HUFFMAN_ONLY)
			{
				return -2;
			}
			strm.dstate = this;
			this.NoHeader = noHeader;
			this.w_bits = windowBits;
			this.w_size = 1 << this.w_bits;
			this.w_mask = this.w_size - 1;
			this.hash_bits = memLevel + 7;
			this.hash_size = 1 << this.hash_bits;
			this.hash_mask = this.hash_size - 1;
			this.hash_shift = (this.hash_bits + 3 - 1) / 3;
			this.window = new byte[this.w_size * 2];
			this.prev = new short[this.w_size];
			this.head = new short[this.hash_size];
			this.lit_bufsize = 1 << memLevel + 6;
			this.Pending_buf = new byte[this.lit_bufsize * 4];
			this.pending_buf_size = this.lit_bufsize * 4;
			this.d_buf = this.lit_bufsize;
			this.l_buf = 3 * this.lit_bufsize;
			this.level = level;
			this.strategy = strategy;
			this.method = (byte)method;
			return this.deflateReset(strm);
		}

		// Token: 0x060007CA RID: 1994 RVA: 0x0002F04C File Offset: 0x0002E04C
		internal int deflateReset(ZStream strm)
		{
			strm.total_in = (strm.total_out = 0L);
			strm.msg = null;
			strm.Data_type = BlockType.Z_UNKNOWN;
			this.pending = 0;
			this.Pending_out = 0;
			if (this.NoHeader < 0)
			{
				this.NoHeader = 0;
			}
			this.status = ((this.NoHeader != 0) ? DeflateState.BUSY_STATE : DeflateState.INIT_STATE);
			strm.adler = Adler32.GetAdler32Checksum(0L, null, 0, 0);
			this.last_flush = 0;
			this.tr_init();
			this.lm_init();
			return 0;
		}

		// Token: 0x060007CB RID: 1995 RVA: 0x0002F0D0 File Offset: 0x0002E0D0
		internal int deflateEnd()
		{
			if (this.status != DeflateState.INIT_STATE && this.status != DeflateState.BUSY_STATE && this.status != DeflateState.FINISH_STATE)
			{
				return -2;
			}
			this.Pending_buf = null;
			this.head = null;
			this.prev = null;
			this.window = null;
			if (this.status != DeflateState.BUSY_STATE)
			{
				return 0;
			}
			return -3;
		}

		// Token: 0x060007CC RID: 1996 RVA: 0x0002F12C File Offset: 0x0002E12C
		internal int deflateParams(ZStream strm, int level, CompressionStrategy strategy)
		{
			int result = 0;
			if (level == -1)
			{
				level = 6;
			}
			if (level < 0 || level > 9 || strategy < CompressionStrategy.Z_DEFAULT_STRATEGY || strategy > CompressionStrategy.Z_HUFFMAN_ONLY)
			{
				return -2;
			}
			if (Deflate.config_table[this._level].func != Deflate.config_table[level].func && strm.total_in != 0L)
			{
				result = strm.deflate(FlushStrategy.Z_PARTIAL_FLUSH);
			}
			if (this._level != level)
			{
				this._level = level;
				this.max_lazy_match = Deflate.config_table[this._level].max_lazy;
				this.good_match = Deflate.config_table[this._level].good_length;
				this.nice_match = Deflate.config_table[this._level].nice_length;
				this.max_chain_length = Deflate.config_table[this._level].max_chain;
			}
			this.strategy = strategy;
			return result;
		}

		// Token: 0x060007CD RID: 1997 RVA: 0x0002F1FC File Offset: 0x0002E1FC
		internal int deflateSetDictionary(ZStream strm, byte[] dictionary, int dictLength)
		{
			int num = dictLength;
			int sourceIndex = 0;
			if (dictionary == null || this.status != DeflateState.INIT_STATE)
			{
				return -2;
			}
			strm.adler = Adler32.GetAdler32Checksum(strm.adler, dictionary, 0, dictLength);
			if (num < 3)
			{
				return 0;
			}
			if (num > this.w_size - 262)
			{
				num = this.w_size - 262;
				sourceIndex = dictLength - num;
			}
			Array.Copy(dictionary, sourceIndex, this.window, 0, num);
			this.strstart = num;
			this.block_start = num;
			this.ins_h = (int)(this.window[0] & byte.MaxValue);
			this.ins_h = ((this.ins_h << this.hash_shift ^ (int)(this.window[1] & byte.MaxValue)) & this.hash_mask);
			for (int i = 0; i <= num - 3; i++)
			{
				this.ins_h = ((this.ins_h << this.hash_shift ^ (int)(this.window[i + 2] & byte.MaxValue)) & this.hash_mask);
				this.prev[i & this.w_mask] = this.head[this.ins_h];
				this.head[this.ins_h] = (short)i;
			}
			return 0;
		}

		// Token: 0x060007CE RID: 1998 RVA: 0x0002F31C File Offset: 0x0002E31C
		internal int deflate(ZStream strm, FlushStrategy f)
		{
			if (f > FlushStrategy.Z_FINISH || f < FlushStrategy.Z_NO_FLUSH)
			{
				return -2;
			}
			if (strm.next_out == null || (strm.next_in == null && strm.avail_in != 0) || (this.status == DeflateState.FINISH_STATE && f != FlushStrategy.Z_FINISH))
			{
				strm.msg = ZLibUtil.z_errmsg[4];
				return -2;
			}
			if (strm.avail_out == 0)
			{
				strm.msg = ZLibUtil.z_errmsg[7];
				return -3;
			}
			this.strm = strm;
			int num = this.last_flush;
			this.last_flush = (int)f;
			if (this.status == DeflateState.INIT_STATE)
			{
				int num2 = 8 + (this.w_bits - 8 << 4) << 8;
				int num3 = (this.level - 1 & 255) >> 1;
				if (num3 > 3)
				{
					num3 = 3;
				}
				num2 |= num3 << 6;
				if (this.strstart != 0)
				{
					num2 |= 32;
				}
				num2 += 31 - num2 % 31;
				this.status = DeflateState.BUSY_STATE;
				this.putShortMSB(num2);
				if (this.strstart != 0)
				{
					this.putShortMSB((int)ZLibUtil.URShift(strm.adler, 16));
					this.putShortMSB((int)(strm.adler & 65535L));
				}
				strm.adler = Adler32.GetAdler32Checksum(0L, null, 0, 0);
			}
			if (this.pending != 0)
			{
				strm.flush_pending();
				if (strm.avail_out == 0)
				{
					this.last_flush = -1;
					return 0;
				}
			}
			else if (strm.avail_in == 0 && f <= (FlushStrategy)num && f != FlushStrategy.Z_FINISH)
			{
				strm.msg = ZLibUtil.z_errmsg[7];
				return -3;
			}
			if (this.status == DeflateState.FINISH_STATE && strm.avail_in != 0)
			{
				strm.msg = ZLibUtil.z_errmsg[7];
				return -3;
			}
			if (strm.avail_in != 0 || this.lookahead != 0 || (f != FlushStrategy.Z_NO_FLUSH && this.status != DeflateState.FINISH_STATE))
			{
				int num4 = -1;
				switch (Deflate.config_table[this.level].func)
				{
				case 0:
					num4 = this.deflate_stored((int)f);
					break;
				case 1:
					num4 = this.deflate_fast((int)f);
					break;
				case 2:
					num4 = this.deflate_slow((int)f);
					break;
				}
				if (num4 == 2 || num4 == 3)
				{
					this.status = DeflateState.FINISH_STATE;
				}
				if (num4 == 0 || num4 == 2)
				{
					if (strm.avail_out == 0)
					{
						this.last_flush = -1;
					}
					return 0;
				}
				if (num4 == 1)
				{
					if (f == FlushStrategy.Z_PARTIAL_FLUSH)
					{
						this._tr_align();
					}
					else
					{
						this._tr_stored_block(0, 0, false);
						if (f == FlushStrategy.Z_FULL_FLUSH)
						{
							for (int i = 0; i < this.hash_size; i++)
							{
								this.head[i] = 0;
							}
						}
					}
					strm.flush_pending();
					if (strm.avail_out == 0)
					{
						this.last_flush = -1;
						return 0;
					}
				}
			}
			if (f != FlushStrategy.Z_FINISH)
			{
				return 0;
			}
			if (this.NoHeader != 0)
			{
				return 1;
			}
			this.putShortMSB((int)ZLibUtil.URShift(strm.adler, 16));
			this.putShortMSB((int)(strm.adler & 65535L));
			strm.flush_pending();
			this.NoHeader = -1;
			if (this.pending == 0)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x060007CF RID: 1999 RVA: 0x0002F5E4 File Offset: 0x0002E5E4
		static Deflate()
		{
			Deflate.config_table[0] = new Deflate.Config(0, 0, 0, 0, 0);
			Deflate.config_table[1] = new Deflate.Config(4, 4, 8, 4, 1);
			Deflate.config_table[2] = new Deflate.Config(4, 5, 16, 8, 1);
			Deflate.config_table[3] = new Deflate.Config(4, 6, 32, 32, 1);
			Deflate.config_table[4] = new Deflate.Config(4, 4, 16, 16, 2);
			Deflate.config_table[5] = new Deflate.Config(8, 16, 32, 32, 2);
			Deflate.config_table[6] = new Deflate.Config(8, 16, 128, 128, 2);
			Deflate.config_table[7] = new Deflate.Config(8, 32, 128, 256, 2);
			Deflate.config_table[8] = new Deflate.Config(32, 128, 258, 1024, 2);
			Deflate.config_table[9] = new Deflate.Config(32, 258, 258, 4096, 2);
		}

		// Token: 0x04000437 RID: 1079
		private const int MAX_MEM_LEVEL = 9;

		// Token: 0x04000438 RID: 1080
		private const int Z_DEFAULT_COMPRESSION = -1;

		// Token: 0x04000439 RID: 1081
		private const int DEF_MEM_LEVEL = 8;

		// Token: 0x0400043A RID: 1082
		private const int STORED = 0;

		// Token: 0x0400043B RID: 1083
		private const int FAST = 1;

		// Token: 0x0400043C RID: 1084
		private const int SLOW = 2;

		// Token: 0x0400043D RID: 1085
		private const int NeedMore = 0;

		// Token: 0x0400043E RID: 1086
		private const int BlockDone = 1;

		// Token: 0x0400043F RID: 1087
		private const int FinishStarted = 2;

		// Token: 0x04000440 RID: 1088
		private const int FinishDone = 3;

		// Token: 0x04000441 RID: 1089
		private const int PRESET_DICT = 32;

		// Token: 0x04000442 RID: 1090
		private const int Z_DEFLATED = 8;

		// Token: 0x04000443 RID: 1091
		private const int STORED_BLOCK = 0;

		// Token: 0x04000444 RID: 1092
		private const int STATIC_TREES = 1;

		// Token: 0x04000445 RID: 1093
		private const int DYN_TREES = 2;

		// Token: 0x04000446 RID: 1094
		private const int Buf_size = 16;

		// Token: 0x04000447 RID: 1095
		private const int REP_3_6 = 16;

		// Token: 0x04000448 RID: 1096
		private const int REPZ_3_10 = 17;

		// Token: 0x04000449 RID: 1097
		private const int REPZ_11_138 = 18;

		// Token: 0x0400044A RID: 1098
		private const int MIN_MATCH = 3;

		// Token: 0x0400044B RID: 1099
		private const int MAX_MATCH = 258;

		// Token: 0x0400044C RID: 1100
		private const int MIN_LOOKAHEAD = 262;

		// Token: 0x0400044D RID: 1101
		private const int MAX_BITS = 15;

		// Token: 0x0400044E RID: 1102
		private const int D_CODES = 30;

		// Token: 0x0400044F RID: 1103
		private const int BL_CODES = 19;

		// Token: 0x04000450 RID: 1104
		private const int LENGTH_CODES = 29;

		// Token: 0x04000451 RID: 1105
		private const int LITERALS = 256;

		// Token: 0x04000452 RID: 1106
		private const int L_CODES = 286;

		// Token: 0x04000453 RID: 1107
		private const int HEAP_SIZE = 573;

		// Token: 0x04000454 RID: 1108
		private const int END_BLOCK = 256;

		// Token: 0x04000455 RID: 1109
		private static Deflate.Config[] config_table = new Deflate.Config[10];

		// Token: 0x04000456 RID: 1110
		private ZStream strm;

		// Token: 0x04000457 RID: 1111
		private DeflateState status;

		// Token: 0x04000458 RID: 1112
		private byte[] pending_buf;

		// Token: 0x04000459 RID: 1113
		private int pending_buf_size;

		// Token: 0x0400045A RID: 1114
		private int pending_out;

		// Token: 0x0400045B RID: 1115
		private int pending;

		// Token: 0x0400045C RID: 1116
		private int noheader;

		// Token: 0x0400045D RID: 1117
		private BlockType data_type;

		// Token: 0x0400045E RID: 1118
		private byte method;

		// Token: 0x0400045F RID: 1119
		private int last_flush;

		// Token: 0x04000460 RID: 1120
		private int w_size;

		// Token: 0x04000461 RID: 1121
		private int w_bits;

		// Token: 0x04000462 RID: 1122
		private int w_mask;

		// Token: 0x04000463 RID: 1123
		private byte[] window;

		// Token: 0x04000464 RID: 1124
		private int window_size;

		// Token: 0x04000465 RID: 1125
		private short[] prev;

		// Token: 0x04000466 RID: 1126
		private short[] head;

		// Token: 0x04000467 RID: 1127
		private int ins_h;

		// Token: 0x04000468 RID: 1128
		private int hash_size;

		// Token: 0x04000469 RID: 1129
		private int hash_bits;

		// Token: 0x0400046A RID: 1130
		private int hash_mask;

		// Token: 0x0400046B RID: 1131
		private int hash_shift;

		// Token: 0x0400046C RID: 1132
		private int block_start;

		// Token: 0x0400046D RID: 1133
		private int match_length;

		// Token: 0x0400046E RID: 1134
		private int prev_match;

		// Token: 0x0400046F RID: 1135
		private int match_available;

		// Token: 0x04000470 RID: 1136
		private int strstart;

		// Token: 0x04000471 RID: 1137
		private int match_start;

		// Token: 0x04000472 RID: 1138
		private int lookahead;

		// Token: 0x04000473 RID: 1139
		private int prev_length;

		// Token: 0x04000474 RID: 1140
		private int max_chain_length;

		// Token: 0x04000475 RID: 1141
		private int max_lazy_match;

		// Token: 0x04000476 RID: 1142
		private int _level;

		// Token: 0x04000477 RID: 1143
		private CompressionStrategy strategy;

		// Token: 0x04000478 RID: 1144
		private int good_match;

		// Token: 0x04000479 RID: 1145
		private int nice_match;

		// Token: 0x0400047A RID: 1146
		private short[] dyn_ltree;

		// Token: 0x0400047B RID: 1147
		private short[] dyn_dtree;

		// Token: 0x0400047C RID: 1148
		private short[] bl_tree;

		// Token: 0x0400047D RID: 1149
		private Tree l_desc = new Tree();

		// Token: 0x0400047E RID: 1150
		private Tree d_desc = new Tree();

		// Token: 0x0400047F RID: 1151
		private Tree bl_desc = new Tree();

		// Token: 0x04000480 RID: 1152
		internal short[] bl_count = new short[16];

		// Token: 0x04000481 RID: 1153
		internal int[] heap = new int[573];

		// Token: 0x04000482 RID: 1154
		internal int heap_len;

		// Token: 0x04000483 RID: 1155
		internal int heap_max;

		// Token: 0x04000484 RID: 1156
		internal byte[] depth = new byte[573];

		// Token: 0x04000485 RID: 1157
		internal int l_buf;

		// Token: 0x04000486 RID: 1158
		private int lit_bufsize;

		// Token: 0x04000487 RID: 1159
		private int last_lit;

		// Token: 0x04000488 RID: 1160
		private int d_buf;

		// Token: 0x04000489 RID: 1161
		internal int opt_len;

		// Token: 0x0400048A RID: 1162
		internal int static_len;

		// Token: 0x0400048B RID: 1163
		internal int matches;

		// Token: 0x0400048C RID: 1164
		internal int last_eob_len;

		// Token: 0x0400048D RID: 1165
		private short bi_buf;

		// Token: 0x0400048E RID: 1166
		private int bi_valid;

		// Token: 0x020000AC RID: 172
		internal class Config
		{
			// Token: 0x060007D0 RID: 2000 RVA: 0x0002F6DC File Offset: 0x0002E6DC
			internal Config(int good_length, int max_lazy, int nice_length, int max_chain, int func)
			{
				this.good_length = good_length;
				this.max_lazy = max_lazy;
				this.nice_length = nice_length;
				this.max_chain = max_chain;
				this.func = func;
			}

			// Token: 0x0400048F RID: 1167
			internal int good_length;

			// Token: 0x04000490 RID: 1168
			internal int max_lazy;

			// Token: 0x04000491 RID: 1169
			internal int nice_length;

			// Token: 0x04000492 RID: 1170
			internal int max_chain;

			// Token: 0x04000493 RID: 1171
			internal int func;
		}
	}
}
