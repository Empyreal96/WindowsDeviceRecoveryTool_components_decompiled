using System;
using System.IO;

namespace ComponentAce.Compression.Libs.bzip2
{
	// Token: 0x02000026 RID: 38
	internal class CBZip2OutputStream : Stream
	{
		// Token: 0x0600019D RID: 413 RVA: 0x00012792 File Offset: 0x00011792
		private static void panic()
		{
			Console.Out.WriteLine("panic");
		}

		// Token: 0x0600019E RID: 414 RVA: 0x000127A4 File Offset: 0x000117A4
		private void makeMaps()
		{
			this.nInUse = 0;
			for (int i = 0; i < 256; i++)
			{
				if (this.inUse[i])
				{
					this.seqToUnseq[this.nInUse] = (char)i;
					this.unseqToSeq[i] = (char)this.nInUse;
					this.nInUse++;
				}
			}
		}

		// Token: 0x0600019F RID: 415 RVA: 0x00012800 File Offset: 0x00011800
		protected internal static void hbMakeCodeLengths(char[] len, int[] freq, int alphaSize, int maxLen)
		{
			int[] array = new int[BZip2Constants_Fields.MAX_ALPHA_SIZE + 2];
			int[] array2 = new int[BZip2Constants_Fields.MAX_ALPHA_SIZE * 2];
			int[] array3 = new int[BZip2Constants_Fields.MAX_ALPHA_SIZE * 2];
			for (int i = 0; i < alphaSize; i++)
			{
				array2[i + 1] = ((freq[i] == 0) ? 1 : freq[i]) << 8;
			}
			for (;;)
			{
				int num = alphaSize;
				int j = 0;
				array[0] = 0;
				array2[0] = 0;
				array3[0] = -2;
				for (int i = 1; i <= alphaSize; i++)
				{
					array3[i] = -1;
					j++;
					array[j] = i;
					int num2 = j;
					int num3 = array[num2];
					while (array2[num3] < array2[array[num2 >> 1]])
					{
						array[num2] = array[num2 >> 1];
						num2 >>= 1;
					}
					array[num2] = num3;
				}
				if (j >= BZip2Constants_Fields.MAX_ALPHA_SIZE + 2)
				{
					CBZip2OutputStream.panic();
				}
				while (j > 1)
				{
					int num4 = array[1];
					array[1] = array[j];
					j--;
					int num5 = 1;
					int num6 = array[num5];
					for (;;)
					{
						int num7 = num5 << 1;
						if (num7 > j)
						{
							break;
						}
						if (num7 < j && array2[array[num7 + 1]] < array2[array[num7]])
						{
							num7++;
						}
						if (array2[num6] < array2[array[num7]])
						{
							break;
						}
						array[num5] = array[num7];
						num5 = num7;
					}
					array[num5] = num6;
					int num8 = array[1];
					array[1] = array[j];
					j--;
					int num9 = 1;
					int num10 = array[num9];
					for (;;)
					{
						int num11 = num9 << 1;
						if (num11 > j)
						{
							break;
						}
						if (num11 < j && array2[array[num11 + 1]] < array2[array[num11]])
						{
							num11++;
						}
						if (array2[num10] < array2[array[num11]])
						{
							break;
						}
						array[num9] = array[num11];
						num9 = num11;
					}
					array[num9] = num10;
					num++;
					array3[num4] = (array3[num8] = num);
					array2[num] = ((array2[num4] & -256) + (array2[num8] & -256) | 1 + (((array2[num4] & 255) > (array2[num8] & 255)) ? (array2[num4] & 255) : (array2[num8] & 255)));
					array3[num] = -1;
					j++;
					array[j] = num;
					int num12 = j;
					int num13 = array[num12];
					while (array2[num13] < array2[array[num12 >> 1]])
					{
						array[num12] = array[num12 >> 1];
						num12 >>= 1;
					}
					array[num12] = num13;
				}
				if (num >= BZip2Constants_Fields.MAX_ALPHA_SIZE * 2)
				{
					CBZip2OutputStream.panic();
				}
				bool flag = false;
				for (int i = 1; i <= alphaSize; i++)
				{
					int num14 = 0;
					int num15 = i;
					while (array3[num15] >= 0)
					{
						num15 = array3[num15];
						num14++;
					}
					len[i - 1] = (char)num14;
					if (num14 > maxLen)
					{
						flag = true;
					}
				}
				if (!flag)
				{
					break;
				}
				for (int i = 1; i < alphaSize; i++)
				{
					int num14 = array2[i] >> 8;
					num14 = 1 + num14 / 2;
					array2[i] = num14 << 8;
				}
			}
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x00012AF5 File Offset: 0x00011AF5
		public CBZip2OutputStream(Stream inStream) : this(inStream, 9)
		{
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x00012B38 File Offset: 0x00011B38
		public CBZip2OutputStream(Stream inStream, int inBlockSize)
		{
			this.block = null;
			this.quadrant = null;
			this.zptr = null;
			this.ftab = null;
			this.bsSetStream(inStream);
			this.workFactor = 50;
			if (inBlockSize > 9)
			{
				inBlockSize = 9;
			}
			if (inBlockSize < 1)
			{
				inBlockSize = 1;
			}
			this.blockSize100k = inBlockSize;
			this.allocateCompressStructures();
			this.initialize();
			this.initBlock();
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x00012C2C File Offset: 0x00011C2C
		public void WriteByte(int bv)
		{
			int num = (256 + bv) % 256;
			if (this.currentChar != -1)
			{
				if (this.currentChar != num)
				{
					this.writeRun();
					this.runLength = 1;
					this.currentChar = num;
					return;
				}
				this.runLength++;
				if (this.runLength > 254)
				{
					this.writeRun();
					this.currentChar = -1;
					this.runLength = 0;
					return;
				}
			}
			else
			{
				this.currentChar = num;
				this.runLength++;
			}
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x00012CB3 File Offset: 0x00011CB3
		public override void WriteByte(byte bv)
		{
			this.WriteByte((int)bv);
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x00012CBC File Offset: 0x00011CBC
		private void writeRun()
		{
			if (this.last >= this.allowableBlockSize)
			{
				this.endBlock();
				this.initBlock();
				this.writeRun();
				return;
			}
			this.inUse[this.currentChar] = true;
			for (int i = 0; i < this.runLength; i++)
			{
				this.mCrc.updateCRC((int)((ushort)this.currentChar));
			}
			switch (this.runLength)
			{
			case 1:
				this.last++;
				this.block[this.last + 1] = (char)this.currentChar;
				return;
			case 2:
				this.last++;
				this.block[this.last + 1] = (char)this.currentChar;
				this.last++;
				this.block[this.last + 1] = (char)this.currentChar;
				return;
			case 3:
				this.last++;
				this.block[this.last + 1] = (char)this.currentChar;
				this.last++;
				this.block[this.last + 1] = (char)this.currentChar;
				this.last++;
				this.block[this.last + 1] = (char)this.currentChar;
				return;
			default:
				this.inUse[this.runLength - 4] = true;
				this.last++;
				this.block[this.last + 1] = (char)this.currentChar;
				this.last++;
				this.block[this.last + 1] = (char)this.currentChar;
				this.last++;
				this.block[this.last + 1] = (char)this.currentChar;
				this.last++;
				this.block[this.last + 1] = (char)this.currentChar;
				this.last++;
				this.block[this.last + 1] = (char)(this.runLength - 4);
				return;
			}
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x00012EE0 File Offset: 0x00011EE0
		~CBZip2OutputStream()
		{
			this.Close();
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x00012F0C File Offset: 0x00011F0C
		public override void Close()
		{
			if (this.closed)
			{
				return;
			}
			if (this.runLength > 0)
			{
				this.writeRun();
			}
			this.currentChar = -1;
			this.endBlock();
			this.endCompression();
			this.closed = true;
			base.Close();
			this.bsStream.Close();
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x00012F5C File Offset: 0x00011F5C
		public override void Flush()
		{
			this.bsStream.Flush();
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x00012F69 File Offset: 0x00011F69
		private void initialize()
		{
			this.bytesOut = 0;
			this.nBlocksRandomised = 0;
			this.bsPutUChar(104);
			this.bsPutUChar(48 + this.blockSize100k);
			this.combinedCRC = 0;
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x00012F98 File Offset: 0x00011F98
		private void initBlock()
		{
			this.mCrc.initialiseCRC();
			this.last = -1;
			for (int i = 0; i < 256; i++)
			{
				this.inUse[i] = false;
			}
			this.allowableBlockSize = BZip2Constants_Fields.baseBlockSize * this.blockSize100k - 20;
		}

		// Token: 0x060001AA RID: 426 RVA: 0x00012FE8 File Offset: 0x00011FE8
		private void endBlock()
		{
			this.blockCRC = this.mCrc.FinalCRC;
			this.combinedCRC = (this.combinedCRC << 1 | SupportClass.URShift(this.combinedCRC, 31));
			this.combinedCRC ^= this.blockCRC;
			this.doReversibleTransformation();
			this.bsPutUChar(49);
			this.bsPutUChar(65);
			this.bsPutUChar(89);
			this.bsPutUChar(38);
			this.bsPutUChar(83);
			this.bsPutUChar(89);
			this.bsPutint(this.blockCRC);
			if (this.blockRandomised)
			{
				this.bsW(1, 1);
				this.nBlocksRandomised++;
			}
			else
			{
				this.bsW(1, 0);
			}
			this.moveToFrontCodeAndSend();
		}

		// Token: 0x060001AB RID: 427 RVA: 0x000130A8 File Offset: 0x000120A8
		private void endCompression()
		{
			this.bsPutUChar(23);
			this.bsPutUChar(114);
			this.bsPutUChar(69);
			this.bsPutUChar(56);
			this.bsPutUChar(80);
			this.bsPutUChar(144);
			this.bsPutint(this.combinedCRC);
			this.bsFinishedWithStream();
		}

		// Token: 0x060001AC RID: 428 RVA: 0x000130FC File Offset: 0x000120FC
		private void hbAssignCodes(int[] code, char[] length, int minLen, int maxLen, int alphaSize)
		{
			int num = 0;
			for (int i = minLen; i <= maxLen; i++)
			{
				for (int j = 0; j < alphaSize; j++)
				{
					if ((int)length[j] == i)
					{
						code[j] = num;
						num++;
					}
				}
				num <<= 1;
			}
		}

		// Token: 0x060001AD RID: 429 RVA: 0x00013137 File Offset: 0x00012137
		private void bsSetStream(Stream f)
		{
			this.bsStream = f;
			this.bsLive = 0;
			this.bsBuff = 0;
			this.bytesOut = 0;
		}

		// Token: 0x060001AE RID: 430 RVA: 0x00013158 File Offset: 0x00012158
		private void bsFinishedWithStream()
		{
			while (this.bsLive > 0)
			{
				int num = this.bsBuff >> 24;
				try
				{
					this.bsStream.WriteByte((byte)num);
				}
				catch (IOException ex)
				{
					throw ex;
				}
				this.bsBuff <<= 8;
				this.bsLive -= 8;
				this.bytesOut++;
			}
		}

		// Token: 0x060001AF RID: 431 RVA: 0x000131C8 File Offset: 0x000121C8
		private void bsW(int n, int v)
		{
			while (this.bsLive >= 8)
			{
				int num = this.bsBuff >> 24;
				try
				{
					this.bsStream.WriteByte((byte)num);
				}
				catch (IOException ex)
				{
					throw ex;
				}
				this.bsBuff <<= 8;
				this.bsLive -= 8;
				this.bytesOut++;
			}
			this.bsBuff |= v << 32 - this.bsLive - n;
			this.bsLive += n;
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x00013264 File Offset: 0x00012264
		private void bsPutUChar(int c)
		{
			this.bsW(8, c);
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x00013270 File Offset: 0x00012270
		private void bsPutint(int u)
		{
			this.bsW(8, u >> 24 & 255);
			this.bsW(8, u >> 16 & 255);
			this.bsW(8, u >> 8 & 255);
			this.bsW(8, u & 255);
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x000132BD File Offset: 0x000122BD
		private void bsPutIntVS(int numBits, int c)
		{
			this.bsW(numBits, c);
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x000132C8 File Offset: 0x000122C8
		private void sendMTFValues()
		{
			char[][] array = new char[BZip2Constants_Fields.N_GROUPS][];
			for (int i = 0; i < BZip2Constants_Fields.N_GROUPS; i++)
			{
				array[i] = new char[BZip2Constants_Fields.MAX_ALPHA_SIZE];
			}
			int num = 0;
			int num2 = this.nInUse + 2;
			for (int j = 0; j < BZip2Constants_Fields.N_GROUPS; j++)
			{
				for (int k = 0; k < num2; k++)
				{
					array[j][k] = (char)SupportClass.Identity(15L);
				}
			}
			if (this.nMTF <= 0)
			{
				CBZip2OutputStream.panic();
			}
			int num3;
			if (this.nMTF < 200)
			{
				num3 = 2;
			}
			else if (this.nMTF < 600)
			{
				num3 = 3;
			}
			else if (this.nMTF < 1200)
			{
				num3 = 4;
			}
			else if (this.nMTF < 2400)
			{
				num3 = 5;
			}
			else
			{
				num3 = 6;
			}
			int l = num3;
			int num4 = this.nMTF;
			int m = 0;
			while (l > 0)
			{
				int num5 = num4 / l;
				int num6 = m - 1;
				int num7 = 0;
				while (num7 < num5 && num6 < num2 - 1)
				{
					num6++;
					num7 += this.mtfFreq[num6];
				}
				if (num6 > m && l != num3 && l != 1 && (num3 - l) % 2 == 1)
				{
					num7 -= this.mtfFreq[num6];
					num6--;
				}
				for (int k = 0; k < num2; k++)
				{
					if (k >= m && k <= num6)
					{
						array[l - 1][k] = (char)SupportClass.Identity(0L);
					}
					else
					{
						array[l - 1][k] = (char)SupportClass.Identity(15L);
					}
				}
				l--;
				m = num6 + 1;
				num4 -= num7;
			}
			int[][] array2 = new int[BZip2Constants_Fields.N_GROUPS][];
			for (int n = 0; n < BZip2Constants_Fields.N_GROUPS; n++)
			{
				array2[n] = new int[BZip2Constants_Fields.MAX_ALPHA_SIZE];
			}
			int[] array3 = new int[BZip2Constants_Fields.N_GROUPS];
			short[] array4 = new short[BZip2Constants_Fields.N_GROUPS];
			for (int num8 = 0; num8 < BZip2Constants_Fields.N_ITERS; num8++)
			{
				for (int j = 0; j < num3; j++)
				{
					array3[j] = 0;
				}
				for (int j = 0; j < num3; j++)
				{
					for (int k = 0; k < num2; k++)
					{
						array2[j][k] = 0;
					}
				}
				num = 0;
				int num9 = 0;
				int num6;
				for (m = 0; m < this.nMTF; m = num6 + 1)
				{
					num6 = m + BZip2Constants_Fields.G_SIZE - 1;
					if (num6 >= this.nMTF)
					{
						num6 = this.nMTF - 1;
					}
					for (int j = 0; j < num3; j++)
					{
						array4[j] = 0;
					}
					if (num3 == 6)
					{
						short num15;
						short num14;
						short num13;
						short num12;
						short num11;
						short num10 = num11 = (num12 = (num13 = (num14 = (num15 = 0))));
						for (int num16 = m; num16 <= num6; num16++)
						{
							short num17 = this.szptr[num16];
							num11 += (short)array[0][(int)num17];
							num10 += (short)array[1][(int)num17];
							num12 += (short)array[2][(int)num17];
							num13 += (short)array[3][(int)num17];
							num14 += (short)array[4][(int)num17];
							num15 += (short)array[5][(int)num17];
						}
						array4[0] = num11;
						array4[1] = num10;
						array4[2] = num12;
						array4[3] = num13;
						array4[4] = num14;
						array4[5] = num15;
					}
					else
					{
						for (int num16 = m; num16 <= num6; num16++)
						{
							short num18 = this.szptr[num16];
							for (int j = 0; j < num3; j++)
							{
								short[] array5 = array4;
								int num19 = j;
								array5[num19] += (short)array[j][(int)num18];
							}
						}
					}
					int num20 = 999999999;
					int num21 = -1;
					for (int j = 0; j < num3; j++)
					{
						if ((int)array4[j] < num20)
						{
							num20 = (int)array4[j];
							num21 = j;
						}
					}
					num9 += num20;
					array3[num21]++;
					this.selector[num] = (char)num21;
					num++;
					for (int num16 = m; num16 <= num6; num16++)
					{
						array2[num21][(int)this.szptr[num16]]++;
					}
				}
				for (int j = 0; j < num3; j++)
				{
					CBZip2OutputStream.hbMakeCodeLengths(array[j], array2[j], num2, 20);
				}
			}
			if (num3 >= 8)
			{
				CBZip2OutputStream.panic();
			}
			if (num >= 32768 || num > 2 + 900000 / BZip2Constants_Fields.G_SIZE)
			{
				CBZip2OutputStream.panic();
			}
			char[] array6 = new char[BZip2Constants_Fields.N_GROUPS];
			for (int num16 = 0; num16 < num3; num16++)
			{
				array6[num16] = (char)num16;
			}
			for (int num16 = 0; num16 < num; num16++)
			{
				char c = this.selector[num16];
				int num22 = 0;
				char c2 = array6[num22];
				while (c != c2)
				{
					num22++;
					char c3 = c2;
					c2 = array6[num22];
					array6[num22] = c3;
				}
				array6[0] = c2;
				this.selectorMtf[num16] = (char)num22;
			}
			int[][] array7 = new int[BZip2Constants_Fields.N_GROUPS][];
			for (int num23 = 0; num23 < BZip2Constants_Fields.N_GROUPS; num23++)
			{
				array7[num23] = new int[BZip2Constants_Fields.MAX_ALPHA_SIZE];
			}
			for (int j = 0; j < num3; j++)
			{
				int num24 = 32;
				int num25 = 0;
				for (int num16 = 0; num16 < num2; num16++)
				{
					if ((int)array[j][num16] > num25)
					{
						num25 = (int)array[j][num16];
					}
					if ((int)array[j][num16] < num24)
					{
						num24 = (int)array[j][num16];
					}
				}
				if (num25 > 20)
				{
					CBZip2OutputStream.panic();
				}
				if (num24 < 1)
				{
					CBZip2OutputStream.panic();
				}
				this.hbAssignCodes(array7[j], array[j], num24, num25, num2);
			}
			bool[] array8 = new bool[16];
			for (int num16 = 0; num16 < 16; num16++)
			{
				array8[num16] = false;
				for (int num22 = 0; num22 < 16; num22++)
				{
					if (this.inUse[num16 * 16 + num22])
					{
						array8[num16] = true;
					}
				}
			}
			for (int num16 = 0; num16 < 16; num16++)
			{
				if (array8[num16])
				{
					this.bsW(1, 1);
				}
				else
				{
					this.bsW(1, 0);
				}
			}
			for (int num16 = 0; num16 < 16; num16++)
			{
				if (array8[num16])
				{
					for (int num22 = 0; num22 < 16; num22++)
					{
						if (this.inUse[num16 * 16 + num22])
						{
							this.bsW(1, 1);
						}
						else
						{
							this.bsW(1, 0);
						}
					}
				}
			}
			this.bsW(3, num3);
			this.bsW(15, num);
			for (int num16 = 0; num16 < num; num16++)
			{
				for (int num22 = 0; num22 < (int)this.selectorMtf[num16]; num22++)
				{
					this.bsW(1, 1);
				}
				this.bsW(1, 0);
			}
			for (int j = 0; j < num3; j++)
			{
				int num26 = (int)array[j][0];
				this.bsW(5, num26);
				for (int num16 = 0; num16 < num2; num16++)
				{
					while (num26 < (int)array[j][num16])
					{
						this.bsW(2, 2);
						num26++;
					}
					while (num26 > (int)array[j][num16])
					{
						this.bsW(2, 3);
						num26--;
					}
					this.bsW(1, 0);
				}
			}
			int num27 = 0;
			m = 0;
			while (m < this.nMTF)
			{
				int num6 = m + BZip2Constants_Fields.G_SIZE - 1;
				if (num6 >= this.nMTF)
				{
					num6 = this.nMTF - 1;
				}
				for (int num16 = m; num16 <= num6; num16++)
				{
					this.bsW((int)array[(int)this.selector[num27]][(int)this.szptr[num16]], array7[(int)this.selector[num27]][(int)this.szptr[num16]]);
				}
				m = num6 + 1;
				num27++;
			}
			if (num27 != num)
			{
				CBZip2OutputStream.panic();
			}
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x00013A25 File Offset: 0x00012A25
		private void moveToFrontCodeAndSend()
		{
			this.bsPutIntVS(24, this.origPtr);
			this.generateMTFValues();
			this.sendMTFValues();
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x00013A44 File Offset: 0x00012A44
		private void simpleSort(int lo, int hi, int d)
		{
			int num = hi - lo + 1;
			if (num < 2)
			{
				return;
			}
			int i = 0;
			while (this.incs[i] < num)
			{
				i++;
			}
			for (i--; i >= 0; i--)
			{
				int num2 = this.incs[i];
				int j = lo + num2;
				while (j <= hi)
				{
					int num3 = this.zptr[j];
					int num4 = j;
					while (this.fullGtU(this.zptr[num4 - num2] + d, num3 + d))
					{
						this.zptr[num4] = this.zptr[num4 - num2];
						num4 -= num2;
						if (num4 <= lo + num2 - 1)
						{
							break;
						}
					}
					this.zptr[num4] = num3;
					j++;
					if (j > hi)
					{
						break;
					}
					num3 = this.zptr[j];
					num4 = j;
					while (this.fullGtU(this.zptr[num4 - num2] + d, num3 + d))
					{
						this.zptr[num4] = this.zptr[num4 - num2];
						num4 -= num2;
						if (num4 <= lo + num2 - 1)
						{
							break;
						}
					}
					this.zptr[num4] = num3;
					j++;
					if (j > hi)
					{
						break;
					}
					num3 = this.zptr[j];
					num4 = j;
					while (this.fullGtU(this.zptr[num4 - num2] + d, num3 + d))
					{
						this.zptr[num4] = this.zptr[num4 - num2];
						num4 -= num2;
						if (num4 <= lo + num2 - 1)
						{
							break;
						}
					}
					this.zptr[num4] = num3;
					j++;
					if (this.workDone > this.workLimit && this.firstAttempt)
					{
						return;
					}
				}
			}
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x00013BC0 File Offset: 0x00012BC0
		private void vswap(int p1, int p2, int n)
		{
			while (n > 0)
			{
				int num = this.zptr[p1];
				this.zptr[p1] = this.zptr[p2];
				this.zptr[p2] = num;
				p1++;
				p2++;
				n--;
			}
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x00013C08 File Offset: 0x00012C08
		private char med3(char a, char b, char c)
		{
			if (a > b)
			{
				char c2 = a;
				a = b;
				b = c2;
			}
			if (b > c)
			{
				char c2 = b;
				b = c;
				c = c2;
			}
			if (a > b)
			{
				b = a;
			}
			return b;
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x00013C38 File Offset: 0x00012C38
		private void qSort3(int loSt, int hiSt, int dSt)
		{
			CBZip2OutputStream.StackElem[] array = new CBZip2OutputStream.StackElem[1000];
			for (int i = 0; i < 1000; i++)
			{
				array[i] = new CBZip2OutputStream.StackElem();
			}
			int j = 0;
			array[j].ll = loSt;
			array[j].hh = hiSt;
			array[j].dd = dSt;
			j++;
			while (j > 0)
			{
				if (j >= 1000)
				{
					CBZip2OutputStream.panic();
				}
				j--;
				int ll = array[j].ll;
				int hh = array[j].hh;
				int dd = array[j].dd;
				if (hh - ll < 20 || dd > 10)
				{
					this.simpleSort(ll, hh, dd);
					if (this.workDone > this.workLimit && this.firstAttempt)
					{
						return;
					}
				}
				else
				{
					int num = (int)this.med3(this.block[this.zptr[ll] + dd + 1], this.block[this.zptr[hh] + dd + 1], this.block[this.zptr[ll + hh >> 1] + dd + 1]);
					int k;
					int num2 = k = ll;
					int num4;
					int num3 = num4 = hh;
					for (;;)
					{
						if (k <= num4)
						{
							int num5 = (int)this.block[this.zptr[k] + dd + 1] - num;
							if (num5 == 0)
							{
								int num6 = this.zptr[k];
								this.zptr[k] = this.zptr[num2];
								this.zptr[num2] = num6;
								num2++;
								k++;
								continue;
							}
							if (num5 <= 0)
							{
								k++;
								continue;
							}
						}
						while (k <= num4)
						{
							int num5 = (int)this.block[this.zptr[num4] + dd + 1] - num;
							if (num5 == 0)
							{
								int num7 = this.zptr[num4];
								this.zptr[num4] = this.zptr[num3];
								this.zptr[num3] = num7;
								num3--;
								num4--;
							}
							else
							{
								if (num5 < 0)
								{
									break;
								}
								num4--;
							}
						}
						if (k > num4)
						{
							break;
						}
						int num8 = this.zptr[k];
						this.zptr[k] = this.zptr[num4];
						this.zptr[num4] = num8;
						k++;
						num4--;
					}
					if (num3 < num2)
					{
						array[j].ll = ll;
						array[j].hh = hh;
						array[j].dd = dd + 1;
						j++;
					}
					else
					{
						int num5 = (num2 - ll < k - num2) ? (num2 - ll) : (k - num2);
						this.vswap(ll, k - num5, num5);
						int num9 = (hh - num3 < num3 - num4) ? (hh - num3) : (num3 - num4);
						this.vswap(k, hh - num9 + 1, num9);
						num5 = ll + k - num2 - 1;
						num9 = hh - (num3 - num4) + 1;
						array[j].ll = ll;
						array[j].hh = num5;
						array[j].dd = dd;
						j++;
						array[j].ll = num5 + 1;
						array[j].hh = num9 - 1;
						array[j].dd = dd + 1;
						j++;
						array[j].ll = num9;
						array[j].hh = hh;
						array[j].dd = dd;
						j++;
					}
				}
			}
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x00013F74 File Offset: 0x00012F74
		private void mainSort()
		{
			int[] array = new int[256];
			int[] array2 = new int[256];
			bool[] array3 = new bool[256];
			for (int i = 0; i < BZip2Constants_Fields.NUM_OVERSHOOT_BYTES; i++)
			{
				this.block[this.last + i + 2] = this.block[i % (this.last + 1) + 1];
			}
			for (int i = 0; i <= this.last + BZip2Constants_Fields.NUM_OVERSHOOT_BYTES; i++)
			{
				this.quadrant[i] = 0;
			}
			this.block[0] = this.block[this.last + 1];
			if (this.last < 4000)
			{
				for (int i = 0; i <= this.last; i++)
				{
					this.zptr[i] = i;
				}
				this.firstAttempt = false;
				this.workDone = (this.workLimit = 0);
				this.simpleSort(0, this.last, 0);
				return;
			}
			int num = 0;
			for (int i = 0; i <= 255; i++)
			{
				array3[i] = false;
			}
			for (int i = 0; i <= 65536; i++)
			{
				this.ftab[i] = 0;
			}
			int num2 = (int)this.block[0];
			for (int i = 0; i <= this.last; i++)
			{
				int num3 = (int)this.block[i + 1];
				this.ftab[(num2 << 8) + num3]++;
				num2 = num3;
			}
			for (int i = 1; i <= 65536; i++)
			{
				this.ftab[i] += this.ftab[i - 1];
			}
			num2 = (int)this.block[1];
			int j;
			for (int i = 0; i < this.last; i++)
			{
				int num3 = (int)this.block[i + 2];
				j = (num2 << 8) + num3;
				num2 = num3;
				this.ftab[j]--;
				this.zptr[this.ftab[j]] = i;
			}
			j = (int)(((int)this.block[this.last + 1] << 8) + this.block[1]);
			this.ftab[j]--;
			this.zptr[this.ftab[j]] = this.last;
			for (int i = 0; i <= 255; i++)
			{
				array[i] = i;
			}
			int num4 = 1;
			do
			{
				num4 = 3 * num4 + 1;
			}
			while (num4 <= 256);
			do
			{
				num4 /= 3;
				for (int i = num4; i <= 255; i++)
				{
					int num5 = array[i];
					j = i;
					while (this.ftab[array[j - num4] + 1 << 8] - this.ftab[array[j - num4] << 8] > this.ftab[num5 + 1 << 8] - this.ftab[num5 << 8])
					{
						array[j] = array[j - num4];
						j -= num4;
						if (j <= num4 - 1)
						{
							break;
						}
					}
					array[j] = num5;
				}
			}
			while (num4 != 1);
			for (int i = 0; i <= 255; i++)
			{
				int num6 = array[i];
				for (j = 0; j <= 255; j++)
				{
					int num7 = (num6 << 8) + j;
					if ((this.ftab[num7] & 2097152) != 2097152)
					{
						int num8 = this.ftab[num7] & CBZip2OutputStream.CLEARMASK;
						int num9 = (this.ftab[num7 + 1] & CBZip2OutputStream.CLEARMASK) - 1;
						if (num9 > num8)
						{
							this.qSort3(num8, num9, 2);
							num += num9 - num8 + 1;
							if (this.workDone > this.workLimit && this.firstAttempt)
							{
								return;
							}
						}
						this.ftab[num7] |= 2097152;
					}
				}
				array3[num6] = true;
				if (i < 255)
				{
					int num10 = this.ftab[num6 << 8] & CBZip2OutputStream.CLEARMASK;
					int num11 = (this.ftab[num6 + 1 << 8] & CBZip2OutputStream.CLEARMASK) - num10;
					int num12 = 0;
					while (num11 >> num12 > 65534)
					{
						num12++;
					}
					for (j = 0; j < num11; j++)
					{
						int num13 = this.zptr[num10 + j];
						int num14 = j >> num12;
						this.quadrant[num13] = num14;
						if (num13 < BZip2Constants_Fields.NUM_OVERSHOOT_BYTES)
						{
							this.quadrant[num13 + this.last + 1] = num14;
						}
					}
					if (num11 - 1 >> num12 > 65535)
					{
						CBZip2OutputStream.panic();
					}
				}
				for (j = 0; j <= 255; j++)
				{
					array2[j] = (this.ftab[(j << 8) + num6] & CBZip2OutputStream.CLEARMASK);
				}
				for (j = (this.ftab[num6 << 8] & CBZip2OutputStream.CLEARMASK); j < (this.ftab[num6 + 1 << 8] & CBZip2OutputStream.CLEARMASK); j++)
				{
					num2 = (int)this.block[this.zptr[j]];
					if (!array3[num2])
					{
						this.zptr[array2[num2]] = ((this.zptr[j] == 0) ? this.last : (this.zptr[j] - 1));
						array2[num2]++;
					}
				}
				for (j = 0; j <= 255; j++)
				{
					this.ftab[(j << 8) + num6] |= 2097152;
				}
			}
		}

		// Token: 0x060001BA RID: 442 RVA: 0x000144AC File Offset: 0x000134AC
		private void randomiseBlock()
		{
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < 256; i++)
			{
				this.inUse[i] = false;
			}
			for (int i = 0; i <= this.last; i++)
			{
				if (num == 0)
				{
					num = (int)((ushort)BZip2Constants_Fields.rNums[num2]);
					num2++;
					if (num2 == 512)
					{
						num2 = 0;
					}
				}
				num--;
				char[] array = this.block;
				int num3 = i + 1;
				array[num3] ^= ((num == 1) ? '\u0001' : '\0');
				char[] array2 = this.block;
				int num4 = i + 1;
				array2[num4] &= 'ÿ';
				this.inUse[(int)this.block[i + 1]] = true;
			}
		}

		// Token: 0x060001BB RID: 443 RVA: 0x00014560 File Offset: 0x00013560
		private void doReversibleTransformation()
		{
			this.workLimit = this.workFactor * this.last;
			this.workDone = 0;
			this.blockRandomised = false;
			this.firstAttempt = true;
			this.mainSort();
			if (this.workDone > this.workLimit && this.firstAttempt)
			{
				this.randomiseBlock();
				this.workLimit = (this.workDone = 0);
				this.blockRandomised = true;
				this.firstAttempt = false;
				this.mainSort();
			}
			this.origPtr = -1;
			for (int i = 0; i <= this.last; i++)
			{
				if (this.zptr[i] == 0)
				{
					this.origPtr = i;
					break;
				}
			}
			if (this.origPtr == -1)
			{
				CBZip2OutputStream.panic();
			}
		}

		// Token: 0x060001BC RID: 444 RVA: 0x00014614 File Offset: 0x00013614
		private bool fullGtU(int i1, int i2)
		{
			char c = this.block[i1 + 1];
			char c2 = this.block[i2 + 1];
			if (c != c2)
			{
				return c > c2;
			}
			i1++;
			i2++;
			c = this.block[i1 + 1];
			c2 = this.block[i2 + 1];
			if (c != c2)
			{
				return c > c2;
			}
			i1++;
			i2++;
			c = this.block[i1 + 1];
			c2 = this.block[i2 + 1];
			if (c != c2)
			{
				return c > c2;
			}
			i1++;
			i2++;
			c = this.block[i1 + 1];
			c2 = this.block[i2 + 1];
			if (c != c2)
			{
				return c > c2;
			}
			i1++;
			i2++;
			c = this.block[i1 + 1];
			c2 = this.block[i2 + 1];
			if (c != c2)
			{
				return c > c2;
			}
			i1++;
			i2++;
			c = this.block[i1 + 1];
			c2 = this.block[i2 + 1];
			if (c != c2)
			{
				return c > c2;
			}
			i1++;
			i2++;
			int num = this.last + 1;
			int num2;
			int num3;
			for (;;)
			{
				c = this.block[i1 + 1];
				c2 = this.block[i2 + 1];
				if (c != c2)
				{
					break;
				}
				num2 = this.quadrant[i1];
				num3 = this.quadrant[i2];
				if (num2 != num3)
				{
					goto Block_8;
				}
				i1++;
				i2++;
				c = this.block[i1 + 1];
				c2 = this.block[i2 + 1];
				if (c != c2)
				{
					goto Block_9;
				}
				num2 = this.quadrant[i1];
				num3 = this.quadrant[i2];
				if (num2 != num3)
				{
					goto Block_10;
				}
				i1++;
				i2++;
				c = this.block[i1 + 1];
				c2 = this.block[i2 + 1];
				if (c != c2)
				{
					goto Block_11;
				}
				num2 = this.quadrant[i1];
				num3 = this.quadrant[i2];
				if (num2 != num3)
				{
					goto Block_12;
				}
				i1++;
				i2++;
				c = this.block[i1 + 1];
				c2 = this.block[i2 + 1];
				if (c != c2)
				{
					goto Block_13;
				}
				num2 = this.quadrant[i1];
				num3 = this.quadrant[i2];
				if (num2 != num3)
				{
					goto Block_14;
				}
				i1++;
				i2++;
				if (i1 > this.last)
				{
					i1 -= this.last;
					i1--;
				}
				if (i2 > this.last)
				{
					i2 -= this.last;
					i2--;
				}
				num -= 4;
				this.workDone++;
				if (num < 0)
				{
					return false;
				}
			}
			return c > c2;
			Block_8:
			return num2 > num3;
			Block_9:
			return c > c2;
			Block_10:
			return num2 > num3;
			Block_11:
			return c > c2;
			Block_12:
			return num2 > num3;
			Block_13:
			return c > c2;
			Block_14:
			return num2 > num3;
		}

		// Token: 0x060001BD RID: 445 RVA: 0x00014888 File Offset: 0x00013888
		private void allocateCompressStructures()
		{
			int num = BZip2Constants_Fields.baseBlockSize * this.blockSize100k;
			this.block = new char[num + 1 + BZip2Constants_Fields.NUM_OVERSHOOT_BYTES];
			this.quadrant = new int[num + BZip2Constants_Fields.NUM_OVERSHOOT_BYTES];
			this.zptr = new int[num];
			this.ftab = new int[65537];
			if (this.block != null && this.quadrant != null && this.zptr != null)
			{
				int[] array = this.ftab;
			}
			this.szptr = new short[2 * num];
		}

		// Token: 0x060001BE RID: 446 RVA: 0x00014914 File Offset: 0x00013914
		private void generateMTFValues()
		{
			char[] array = new char[256];
			this.makeMaps();
			int num = this.nInUse + 1;
			for (int i = 0; i <= num; i++)
			{
				this.mtfFreq[i] = 0;
			}
			int num2 = 0;
			int num3 = 0;
			for (int i = 0; i < this.nInUse; i++)
			{
				array[i] = (char)i;
			}
			for (int i = 0; i <= this.last; i++)
			{
				char c = this.unseqToSeq[(int)this.block[this.zptr[i]]];
				int num4 = 0;
				char c2 = array[num4];
				while (c != c2)
				{
					num4++;
					char c3 = c2;
					c2 = array[num4];
					array[num4] = c3;
				}
				array[0] = c2;
				if (num4 == 0)
				{
					num3++;
				}
				else
				{
					if (num3 > 0)
					{
						num3--;
						for (;;)
						{
							switch (num3 % 2)
							{
							case 0:
								this.szptr[num2] = (short)BZip2Constants_Fields.RUNA;
								num2++;
								this.mtfFreq[BZip2Constants_Fields.RUNA]++;
								break;
							case 1:
								this.szptr[num2] = (short)BZip2Constants_Fields.RUNB;
								num2++;
								this.mtfFreq[BZip2Constants_Fields.RUNB]++;
								break;
							}
							if (num3 < 2)
							{
								break;
							}
							num3 = (num3 - 2) / 2;
						}
						num3 = 0;
					}
					this.szptr[num2] = (short)(num4 + 1);
					num2++;
					this.mtfFreq[num4 + 1]++;
				}
			}
			if (num3 > 0)
			{
				num3--;
				for (;;)
				{
					switch (num3 % 2)
					{
					case 0:
						this.szptr[num2] = (short)BZip2Constants_Fields.RUNA;
						num2++;
						this.mtfFreq[BZip2Constants_Fields.RUNA]++;
						break;
					case 1:
						this.szptr[num2] = (short)BZip2Constants_Fields.RUNB;
						num2++;
						this.mtfFreq[BZip2Constants_Fields.RUNB]++;
						break;
					}
					if (num3 < 2)
					{
						break;
					}
					num3 = (num3 - 2) / 2;
				}
			}
			this.szptr[num2] = (short)num;
			num2++;
			this.mtfFreq[num]++;
			this.nMTF = num2;
		}

		// Token: 0x060001BF RID: 447 RVA: 0x00014B6B File Offset: 0x00013B6B
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x00014B72 File Offset: 0x00013B72
		public override void SetLength(long value)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x00014B79 File Offset: 0x00013B79
		public override int Read(byte[] buffer, int offset, int count)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x00014B80 File Offset: 0x00013B80
		public override void Write(byte[] buffer, int offset, int count)
		{
			for (int i = offset; i < count; i++)
			{
				this.WriteByte((int)buffer[i]);
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060001C3 RID: 451 RVA: 0x00014BA2 File Offset: 0x00013BA2
		public override bool CanRead
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060001C4 RID: 452 RVA: 0x00014BA5 File Offset: 0x00013BA5
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060001C5 RID: 453 RVA: 0x00014BA8 File Offset: 0x00013BA8
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060001C6 RID: 454 RVA: 0x00014BAB File Offset: 0x00013BAB
		public override long Length
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060001C7 RID: 455 RVA: 0x00014BB2 File Offset: 0x00013BB2
		// (set) Token: 0x060001C8 RID: 456 RVA: 0x00014BB9 File Offset: 0x00013BB9
		public override long Position
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
			}
		}

		// Token: 0x04000103 RID: 259
		protected internal const int SETMASK = 2097152;

		// Token: 0x04000104 RID: 260
		protected internal const int GREATER_ICOST = 15;

		// Token: 0x04000105 RID: 261
		protected internal const int LESSER_ICOST = 0;

		// Token: 0x04000106 RID: 262
		protected internal const int SMALL_THRESH = 20;

		// Token: 0x04000107 RID: 263
		protected internal const int DEPTH_THRESH = 10;

		// Token: 0x04000108 RID: 264
		protected internal const int QSORT_STACK_SIZE = 1000;

		// Token: 0x04000109 RID: 265
		protected internal static readonly int CLEARMASK = -2097153;

		// Token: 0x0400010A RID: 266
		internal int last;

		// Token: 0x0400010B RID: 267
		internal int origPtr;

		// Token: 0x0400010C RID: 268
		internal int blockSize100k;

		// Token: 0x0400010D RID: 269
		internal bool blockRandomised;

		// Token: 0x0400010E RID: 270
		internal int bytesOut;

		// Token: 0x0400010F RID: 271
		internal int bsBuff;

		// Token: 0x04000110 RID: 272
		internal int bsLive;

		// Token: 0x04000111 RID: 273
		internal CRC mCrc = new CRC();

		// Token: 0x04000112 RID: 274
		private bool[] inUse = new bool[256];

		// Token: 0x04000113 RID: 275
		private int nInUse;

		// Token: 0x04000114 RID: 276
		private char[] seqToUnseq = new char[256];

		// Token: 0x04000115 RID: 277
		private char[] unseqToSeq = new char[256];

		// Token: 0x04000116 RID: 278
		private char[] selector = new char[BZip2Constants_Fields.MAX_SELECTORS];

		// Token: 0x04000117 RID: 279
		private char[] selectorMtf = new char[BZip2Constants_Fields.MAX_SELECTORS];

		// Token: 0x04000118 RID: 280
		private char[] block;

		// Token: 0x04000119 RID: 281
		private int[] quadrant;

		// Token: 0x0400011A RID: 282
		private int[] zptr;

		// Token: 0x0400011B RID: 283
		private short[] szptr;

		// Token: 0x0400011C RID: 284
		private int[] ftab;

		// Token: 0x0400011D RID: 285
		private int nMTF;

		// Token: 0x0400011E RID: 286
		private int[] mtfFreq = new int[BZip2Constants_Fields.MAX_ALPHA_SIZE];

		// Token: 0x0400011F RID: 287
		private int workFactor;

		// Token: 0x04000120 RID: 288
		private int workDone;

		// Token: 0x04000121 RID: 289
		private int workLimit;

		// Token: 0x04000122 RID: 290
		private bool firstAttempt;

		// Token: 0x04000123 RID: 291
		private int nBlocksRandomised;

		// Token: 0x04000124 RID: 292
		private int currentChar = -1;

		// Token: 0x04000125 RID: 293
		private int runLength;

		// Token: 0x04000126 RID: 294
		internal bool closed;

		// Token: 0x04000127 RID: 295
		private int blockCRC;

		// Token: 0x04000128 RID: 296
		private int combinedCRC;

		// Token: 0x04000129 RID: 297
		private int allowableBlockSize;

		// Token: 0x0400012A RID: 298
		private Stream bsStream;

		// Token: 0x0400012B RID: 299
		private int[] incs = new int[]
		{
			1,
			4,
			13,
			40,
			121,
			364,
			1093,
			3280,
			9841,
			29524,
			88573,
			265720,
			797161,
			2391484
		};

		// Token: 0x02000027 RID: 39
		private class StackElem
		{
			// Token: 0x0400012C RID: 300
			internal int ll;

			// Token: 0x0400012D RID: 301
			internal int hh;

			// Token: 0x0400012E RID: 302
			internal int dd;
		}
	}
}
