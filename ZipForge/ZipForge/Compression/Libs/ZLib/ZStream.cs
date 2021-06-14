using System;

namespace ComponentAce.Compression.Libs.ZLib
{
	// Token: 0x020000C3 RID: 195
	public sealed class ZStream
	{
		// Token: 0x17000171 RID: 369
		// (get) Token: 0x0600084F RID: 2127 RVA: 0x00036172 File Offset: 0x00035172
		// (set) Token: 0x06000850 RID: 2128 RVA: 0x0003617A File Offset: 0x0003517A
		internal long adler
		{
			get
			{
				return this._adler;
			}
			set
			{
				this._adler = value;
			}
		}

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x06000851 RID: 2129 RVA: 0x00036183 File Offset: 0x00035183
		// (set) Token: 0x06000852 RID: 2130 RVA: 0x0003618B File Offset: 0x0003518B
		internal BlockType Data_type
		{
			get
			{
				return this.data_type;
			}
			set
			{
				this.data_type = value;
			}
		}

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x06000853 RID: 2131 RVA: 0x00036194 File Offset: 0x00035194
		// (set) Token: 0x06000854 RID: 2132 RVA: 0x0003619C File Offset: 0x0003519C
		public byte[] next_in
		{
			get
			{
				return this._next_in;
			}
			set
			{
				this._next_in = value;
			}
		}

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x06000855 RID: 2133 RVA: 0x000361A5 File Offset: 0x000351A5
		// (set) Token: 0x06000856 RID: 2134 RVA: 0x000361AD File Offset: 0x000351AD
		public int next_in_index
		{
			get
			{
				return this._next_in_index;
			}
			set
			{
				this._next_in_index = value;
			}
		}

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x06000857 RID: 2135 RVA: 0x000361B6 File Offset: 0x000351B6
		// (set) Token: 0x06000858 RID: 2136 RVA: 0x000361BE File Offset: 0x000351BE
		public int avail_in
		{
			get
			{
				return this._avail_in;
			}
			set
			{
				this._avail_in = value;
			}
		}

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x06000859 RID: 2137 RVA: 0x000361C7 File Offset: 0x000351C7
		// (set) Token: 0x0600085A RID: 2138 RVA: 0x000361CF File Offset: 0x000351CF
		public long total_in
		{
			get
			{
				return this._total_in;
			}
			set
			{
				this._total_in = value;
			}
		}

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x0600085B RID: 2139 RVA: 0x000361D8 File Offset: 0x000351D8
		// (set) Token: 0x0600085C RID: 2140 RVA: 0x000361E0 File Offset: 0x000351E0
		public byte[] next_out
		{
			get
			{
				return this._next_out;
			}
			set
			{
				this._next_out = value;
			}
		}

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x0600085D RID: 2141 RVA: 0x000361E9 File Offset: 0x000351E9
		// (set) Token: 0x0600085E RID: 2142 RVA: 0x000361F1 File Offset: 0x000351F1
		public int next_out_index
		{
			get
			{
				return this._next_out_index;
			}
			set
			{
				this._next_out_index = value;
			}
		}

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x0600085F RID: 2143 RVA: 0x000361FA File Offset: 0x000351FA
		// (set) Token: 0x06000860 RID: 2144 RVA: 0x00036202 File Offset: 0x00035202
		public int avail_out
		{
			get
			{
				return this._avail_out;
			}
			set
			{
				this._avail_out = value;
			}
		}

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x06000861 RID: 2145 RVA: 0x0003620B File Offset: 0x0003520B
		// (set) Token: 0x06000862 RID: 2146 RVA: 0x00036213 File Offset: 0x00035213
		public long total_out
		{
			get
			{
				return this._total_out;
			}
			set
			{
				this._total_out = value;
			}
		}

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x06000863 RID: 2147 RVA: 0x0003621C File Offset: 0x0003521C
		// (set) Token: 0x06000864 RID: 2148 RVA: 0x00036224 File Offset: 0x00035224
		public string msg
		{
			get
			{
				return this._msg;
			}
			set
			{
				this._msg = value;
			}
		}

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x06000865 RID: 2149 RVA: 0x0003622D File Offset: 0x0003522D
		// (set) Token: 0x06000866 RID: 2150 RVA: 0x00036235 File Offset: 0x00035235
		internal Deflate dstate
		{
			get
			{
				return this._dstate;
			}
			set
			{
				this._dstate = value;
			}
		}

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x06000867 RID: 2151 RVA: 0x0003623E File Offset: 0x0003523E
		// (set) Token: 0x06000868 RID: 2152 RVA: 0x00036246 File Offset: 0x00035246
		internal Inflate istate
		{
			get
			{
				return this._istate;
			}
			set
			{
				this._istate = value;
			}
		}

		// Token: 0x06000869 RID: 2153 RVA: 0x0003624F File Offset: 0x0003524F
		public int inflateInit()
		{
			return this.inflateInit(15);
		}

		// Token: 0x0600086A RID: 2154 RVA: 0x00036259 File Offset: 0x00035259
		public int inflateInit(int windowBits)
		{
			this._istate = new Inflate();
			return this._istate.inflateInit(this, windowBits);
		}

		// Token: 0x0600086B RID: 2155 RVA: 0x00036273 File Offset: 0x00035273
		public int inflate(FlushStrategy flush)
		{
			if (this._istate == null)
			{
				return -2;
			}
			return this._istate.inflate(this, flush);
		}

		// Token: 0x0600086C RID: 2156 RVA: 0x00036290 File Offset: 0x00035290
		public int inflateEnd()
		{
			if (this._istate == null)
			{
				return -2;
			}
			int result = this._istate.inflateEnd(this);
			this._istate = null;
			return result;
		}

		// Token: 0x0600086D RID: 2157 RVA: 0x000362BD File Offset: 0x000352BD
		public int inflateSync()
		{
			if (this._istate == null)
			{
				return -2;
			}
			return this._istate.inflateSync(this);
		}

		// Token: 0x0600086E RID: 2158 RVA: 0x000362D6 File Offset: 0x000352D6
		public int inflateSetDictionary(byte[] dictionary, int dictLength)
		{
			if (this._istate == null)
			{
				return -2;
			}
			return this._istate.inflateSetDictionary(this, dictionary, dictLength);
		}

		// Token: 0x0600086F RID: 2159 RVA: 0x000362F1 File Offset: 0x000352F1
		public int deflateInit(int level)
		{
			return this.deflateInit(level, 15);
		}

		// Token: 0x06000870 RID: 2160 RVA: 0x000362FC File Offset: 0x000352FC
		public int deflateInit(int level, int bits)
		{
			this._dstate = new Deflate();
			return this._dstate.deflateInit(this, level, bits);
		}

		// Token: 0x06000871 RID: 2161 RVA: 0x00036317 File Offset: 0x00035317
		public int deflate(FlushStrategy flush)
		{
			if (this._dstate == null)
			{
				return -2;
			}
			return this._dstate.deflate(this, flush);
		}

		// Token: 0x06000872 RID: 2162 RVA: 0x00036334 File Offset: 0x00035334
		public int deflateEnd()
		{
			if (this._dstate == null)
			{
				return -2;
			}
			int result = this._dstate.deflateEnd();
			this._dstate = null;
			return result;
		}

		// Token: 0x06000873 RID: 2163 RVA: 0x00036360 File Offset: 0x00035360
		public int deflateParams(int level, CompressionStrategy strategy)
		{
			if (this._dstate == null)
			{
				return -2;
			}
			return this._dstate.deflateParams(this, level, strategy);
		}

		// Token: 0x06000874 RID: 2164 RVA: 0x0003637B File Offset: 0x0003537B
		public int deflateSetDictionary(byte[] dictionary, int dictLength)
		{
			if (this._dstate == null)
			{
				return -2;
			}
			return this._dstate.deflateSetDictionary(this, dictionary, dictLength);
		}

		// Token: 0x06000875 RID: 2165 RVA: 0x00036398 File Offset: 0x00035398
		public void flush_pending()
		{
			int num = this._dstate.Pending;
			if (num > this._avail_out)
			{
				num = this._avail_out;
			}
			if (num == 0)
			{
				return;
			}
			Array.Copy(this._dstate.Pending_buf, this._dstate.Pending_out, this._next_out, this._next_out_index, num);
			this._next_out_index += num;
			this._dstate.Pending_out += num;
			this._total_out += (long)num;
			this._avail_out -= num;
			this._dstate.Pending -= num;
			if (this._dstate.Pending == 0)
			{
				this._dstate.Pending_out = 0;
			}
		}

		// Token: 0x06000876 RID: 2166 RVA: 0x00036458 File Offset: 0x00035458
		public int read_buf(byte[] buf, int start, int size)
		{
			int num = this._avail_in;
			if (num > size)
			{
				num = size;
			}
			if (num == 0)
			{
				return 0;
			}
			this._avail_in -= num;
			if (this._dstate.NoHeader == 0)
			{
				this.adler = Adler32.GetAdler32Checksum(this.adler, this._next_in, this._next_in_index, num);
			}
			Array.Copy(this._next_in, this._next_in_index, buf, start, num);
			this._next_in_index += num;
			this._total_in += (long)num;
			return num;
		}

		// Token: 0x06000877 RID: 2167 RVA: 0x000364E2 File Offset: 0x000354E2
		public void free()
		{
			this._next_in = null;
			this._next_out = null;
			this._msg = null;
		}

		// Token: 0x04000558 RID: 1368
		private const int DEF_WBITS = 15;

		// Token: 0x04000559 RID: 1369
		private const int MAX_MEM_LEVEL = 9;

		// Token: 0x0400055A RID: 1370
		private byte[] _next_in;

		// Token: 0x0400055B RID: 1371
		private int _next_in_index;

		// Token: 0x0400055C RID: 1372
		private int _avail_in;

		// Token: 0x0400055D RID: 1373
		private long _total_in;

		// Token: 0x0400055E RID: 1374
		private byte[] _next_out;

		// Token: 0x0400055F RID: 1375
		private int _next_out_index;

		// Token: 0x04000560 RID: 1376
		private int _avail_out;

		// Token: 0x04000561 RID: 1377
		private long _total_out;

		// Token: 0x04000562 RID: 1378
		private string _msg;

		// Token: 0x04000563 RID: 1379
		private Deflate _dstate;

		// Token: 0x04000564 RID: 1380
		private Inflate _istate;

		// Token: 0x04000565 RID: 1381
		private BlockType data_type;

		// Token: 0x04000566 RID: 1382
		private long _adler;
	}
}
