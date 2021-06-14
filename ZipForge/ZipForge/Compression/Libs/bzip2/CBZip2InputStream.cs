using System;
using System.IO;

namespace ComponentAce.Compression.Libs.bzip2
{
	// Token: 0x02000025 RID: 37
	internal class CBZip2InputStream : Stream
	{
		// Token: 0x06000170 RID: 368 RVA: 0x000113BC File Offset: 0x000103BC
		private void initBlock3()
		{
			for (int i = 0; i < BZip2Constants_Fields.N_GROUPS; i++)
			{
				this.limit[i] = new int[BZip2Constants_Fields.MAX_ALPHA_SIZE];
			}
			for (int j = 0; j < BZip2Constants_Fields.N_GROUPS; j++)
			{
				this.base_Renamed[j] = new int[BZip2Constants_Fields.MAX_ALPHA_SIZE];
			}
			for (int k = 0; k < BZip2Constants_Fields.N_GROUPS; k++)
			{
				this.perm[k] = new int[BZip2Constants_Fields.MAX_ALPHA_SIZE];
			}
		}

		// Token: 0x1700002E RID: 46
		// (set) Token: 0x06000171 RID: 369 RVA: 0x00011430 File Offset: 0x00010430
		private int DecompressStructureSizes
		{
			set
			{
				if (0 <= value && value <= 9 && 0 <= this.blockSize100k)
				{
					int num = this.blockSize100k;
				}
				this.blockSize100k = value;
				if (value == 0)
				{
					return;
				}
				int num2 = BZip2Constants_Fields.baseBlockSize * value;
				this.ll8 = new char[num2];
				this.tt = new int[num2];
			}
		}

		// Token: 0x06000172 RID: 370 RVA: 0x00011484 File Offset: 0x00010484
		private static void cadvise()
		{
			Console.Out.WriteLine("Crc Error");
		}

		// Token: 0x06000173 RID: 371 RVA: 0x00011495 File Offset: 0x00010495
		private static void badBGLengths()
		{
			CBZip2InputStream.cadvise();
		}

		// Token: 0x06000174 RID: 372 RVA: 0x0001149C File Offset: 0x0001049C
		private static void bitStreamEOF()
		{
			CBZip2InputStream.cadvise();
		}

		// Token: 0x06000175 RID: 373 RVA: 0x000114A3 File Offset: 0x000104A3
		private static void compressedStreamEOF()
		{
			CBZip2InputStream.cadvise();
		}

		// Token: 0x06000176 RID: 374 RVA: 0x000114AC File Offset: 0x000104AC
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

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000177 RID: 375 RVA: 0x00011506 File Offset: 0x00010506
		public bool IsStreamEndReached
		{
			get
			{
				return this.bsStream == null;
			}
		}

		// Token: 0x06000178 RID: 376 RVA: 0x00011514 File Offset: 0x00010514
		public CBZip2InputStream(Stream zStream)
		{
			this.initBlock3();
			this.ll8 = null;
			this.tt = null;
			this.bsSetStream(zStream);
			this.initialize();
			this.initBlock();
			this.setupBlock();
		}

		// Token: 0x06000179 RID: 377 RVA: 0x00011610 File Offset: 0x00010610
		public override int ReadByte()
		{
			if (this.streamEnd)
			{
				return -1;
			}
			int result = this.currentChar;
			switch (this.currentState)
			{
			case 3:
				this.setupRandPartB();
				break;
			case 4:
				this.setupRandPartC();
				break;
			case 6:
				this.setupNoRandPartB();
				break;
			case 7:
				this.setupNoRandPartC();
				break;
			}
			return result;
		}

		// Token: 0x0600017A RID: 378 RVA: 0x0001167C File Offset: 0x0001067C
		private void initialize()
		{
			char c = this.bsGetUChar();
			char c2 = this.bsGetUChar();
			if (c != 'h' || c2 < '1' || c2 > '9')
			{
				this.bsFinishedWithStream();
				this.streamEnd = true;
				return;
			}
			this.DecompressStructureSizes = (int)(c2 - '0');
			this.computedCombinedCRC = 0;
		}

		// Token: 0x0600017B RID: 379 RVA: 0x000116C8 File Offset: 0x000106C8
		private void initBlock()
		{
			char c = this.bsGetUChar();
			char c2 = this.bsGetUChar();
			char c3 = this.bsGetUChar();
			char c4 = this.bsGetUChar();
			char c5 = this.bsGetUChar();
			char c6 = this.bsGetUChar();
			if (c == '\u0017' && c2 == 'r' && c3 == 'E' && c4 == '8' && c5 == 'P' && c6 == '\u0090')
			{
				this.complete();
				return;
			}
			if (c != '1' || c2 != 'A' || c3 != 'Y' || c4 != '&' || c5 != 'S' || c6 != 'Y')
			{
				CBZip2InputStream.badBlockHeader();
				this.streamEnd = true;
				return;
			}
			this.storedBlockCRC = this.bsGetInt32();
			if (this.bsR(1) == 1)
			{
				this.blockRandomised = true;
			}
			else
			{
				this.blockRandomised = false;
			}
			this.getAndMoveToFrontDecode();
			this.mCrc.initialiseCRC();
			this.currentState = 1;
		}

		// Token: 0x0600017C RID: 380 RVA: 0x00011798 File Offset: 0x00010798
		private void endBlock()
		{
			this.computedBlockCRC = this.mCrc.FinalCRC;
			if (this.storedBlockCRC != this.computedBlockCRC)
			{
				CBZip2InputStream.crcError();
			}
			this.computedCombinedCRC = (this.computedCombinedCRC << 1 | SupportClass.URShift(this.computedCombinedCRC, 31));
			this.computedCombinedCRC ^= this.computedBlockCRC;
		}

		// Token: 0x0600017D RID: 381 RVA: 0x000117F8 File Offset: 0x000107F8
		private void complete()
		{
			this.storedCombinedCRC = this.bsGetInt32();
			if (this.storedCombinedCRC != this.computedCombinedCRC)
			{
				CBZip2InputStream.crcError();
			}
			this.bsFinishedWithStream();
			this.streamEnd = true;
		}

		// Token: 0x0600017E RID: 382 RVA: 0x00011826 File Offset: 0x00010826
		private static void blockOverrun()
		{
			CBZip2InputStream.cadvise();
		}

		// Token: 0x0600017F RID: 383 RVA: 0x0001182D File Offset: 0x0001082D
		private static void badBlockHeader()
		{
			CBZip2InputStream.cadvise();
		}

		// Token: 0x06000180 RID: 384 RVA: 0x00011834 File Offset: 0x00010834
		private static void crcError()
		{
			CBZip2InputStream.cadvise();
		}

		// Token: 0x06000181 RID: 385 RVA: 0x0001183C File Offset: 0x0001083C
		private void bsFinishedWithStream()
		{
			try
			{
				if (this.bsStream != null)
				{
					if (this.bsStream.CanSeek)
					{
						this.position = this.bsStream.Position;
					}
					this.bsStream.Close();
					this.bsStream = null;
				}
			}
			catch (IOException)
			{
			}
		}

		// Token: 0x06000182 RID: 386 RVA: 0x00011898 File Offset: 0x00010898
		private void bsSetStream(Stream f)
		{
			this.bsStream = f;
			this.bsLive = 0;
			this.bsBuff = 0;
		}

		// Token: 0x06000183 RID: 387 RVA: 0x000118B0 File Offset: 0x000108B0
		private int bsR(int n)
		{
			while (this.bsLive < n)
			{
				char c = '\0';
				try
				{
					c = (char)this.bsStream.ReadByte();
				}
				catch (IOException)
				{
					CBZip2InputStream.compressedStreamEOF();
				}
				if ((short)c == -1)
				{
					CBZip2InputStream.compressedStreamEOF();
				}
				int num = (int)c;
				this.bsBuff = (this.bsBuff << 8 | (num & 255));
				this.bsLive += 8;
			}
			int result = this.bsBuff >> this.bsLive - n & (1 << n) - 1;
			this.bsLive -= n;
			return result;
		}

		// Token: 0x06000184 RID: 388 RVA: 0x0001194C File Offset: 0x0001094C
		private char bsGetUChar()
		{
			return (char)this.bsR(8);
		}

		// Token: 0x06000185 RID: 389 RVA: 0x00011958 File Offset: 0x00010958
		private int bsGetint()
		{
			int num = 0;
			num = (num << 8 | this.bsR(8));
			num = (num << 8 | this.bsR(8));
			num = (num << 8 | this.bsR(8));
			return num << 8 | this.bsR(8);
		}

		// Token: 0x06000186 RID: 390 RVA: 0x00011998 File Offset: 0x00010998
		private int bsGetIntVS(int numBits)
		{
			return this.bsR(numBits);
		}

		// Token: 0x06000187 RID: 391 RVA: 0x000119A1 File Offset: 0x000109A1
		private int bsGetInt32()
		{
			return this.bsGetint();
		}

		// Token: 0x06000188 RID: 392 RVA: 0x000119AC File Offset: 0x000109AC
		private void hbCreateDecodeTables(int[] limit, int[] base_Renamed, int[] perm, char[] length, int minLen, int maxLen, int alphaSize)
		{
			int num = 0;
			for (int i = minLen; i <= maxLen; i++)
			{
				for (int j = 0; j < alphaSize; j++)
				{
					if ((int)length[j] == i)
					{
						perm[num] = j;
						num++;
					}
				}
			}
			for (int i = 0; i < BZip2Constants_Fields.MAX_CODE_LEN; i++)
			{
				base_Renamed[i] = 0;
			}
			for (int i = 0; i < alphaSize; i++)
			{
				base_Renamed[(int)(length[i] + '\u0001')]++;
			}
			for (int i = 1; i < BZip2Constants_Fields.MAX_CODE_LEN; i++)
			{
				base_Renamed[i] += base_Renamed[i - 1];
			}
			for (int i = 0; i < BZip2Constants_Fields.MAX_CODE_LEN; i++)
			{
				limit[i] = 0;
			}
			int num2 = 0;
			for (int i = minLen; i <= maxLen; i++)
			{
				num2 += base_Renamed[i + 1] - base_Renamed[i];
				limit[i] = num2 - 1;
				num2 <<= 1;
			}
			for (int i = minLen + 1; i <= maxLen; i++)
			{
				base_Renamed[i] = (limit[i - 1] + 1 << 1) - base_Renamed[i];
			}
		}

		// Token: 0x06000189 RID: 393 RVA: 0x00011AA4 File Offset: 0x00010AA4
		private void recvDecodingTables()
		{
			char[][] array = new char[BZip2Constants_Fields.N_GROUPS][];
			for (int i = 0; i < BZip2Constants_Fields.N_GROUPS; i++)
			{
				array[i] = new char[BZip2Constants_Fields.MAX_ALPHA_SIZE];
			}
			bool[] array2 = new bool[16];
			for (int j = 0; j < 16; j++)
			{
				if (this.bsR(1) == 1)
				{
					array2[j] = true;
				}
				else
				{
					array2[j] = false;
				}
			}
			for (int j = 0; j < 256; j++)
			{
				this.inUse[j] = false;
			}
			for (int j = 0; j < 16; j++)
			{
				if (array2[j])
				{
					for (int k = 0; k < 16; k++)
					{
						if (this.bsR(1) == 1)
						{
							this.inUse[j * 16 + k] = true;
						}
					}
				}
			}
			this.makeMaps();
			int num = this.nInUse + 2;
			int num2 = this.bsR(3);
			int num3 = this.bsR(15);
			for (int j = 0; j < num3; j++)
			{
				int k = 0;
				while (this.bsR(1) == 1)
				{
					k++;
				}
				this.selectorMtf[j] = (char)k;
			}
			char[] array3 = new char[BZip2Constants_Fields.N_GROUPS];
			char c = '\0';
			while ((int)c < num2)
			{
				array3[(int)c] = c;
				c += '\u0001';
			}
			for (int j = 0; j < num3; j++)
			{
				c = this.selectorMtf[j];
				char c2 = array3[(int)c];
				while (c > '\0')
				{
					array3[(int)c] = array3[(int)(c - '\u0001')];
					c -= '\u0001';
				}
				array3[0] = c2;
				this.selector[j] = c2;
			}
			for (int l = 0; l < num2; l++)
			{
				int num4 = this.bsR(5);
				for (int j = 0; j < num; j++)
				{
					while (this.bsR(1) == 1)
					{
						if (this.bsR(1) == 0)
						{
							num4++;
						}
						else
						{
							num4--;
						}
					}
					array[l][j] = (char)num4;
				}
			}
			for (int l = 0; l < num2; l++)
			{
				int num5 = 32;
				int num6 = 0;
				for (int j = 0; j < num; j++)
				{
					if ((int)array[l][j] > num6)
					{
						num6 = (int)array[l][j];
					}
					if ((int)array[l][j] < num5)
					{
						num5 = (int)array[l][j];
					}
				}
				this.hbCreateDecodeTables(this.limit[l], this.base_Renamed[l], this.perm[l], array[l], num5, num6, num);
				this.minLens[l] = num5;
			}
		}

		// Token: 0x0600018A RID: 394 RVA: 0x00011CE0 File Offset: 0x00010CE0
		private void getAndMoveToFrontDecode()
		{
			char[] array = new char[256];
			int num = BZip2Constants_Fields.baseBlockSize * this.blockSize100k;
			this.origPtr = this.bsGetIntVS(24);
			this.recvDecodingTables();
			int num2 = this.nInUse + 1;
			int num3 = -1;
			int num4 = 0;
			for (int i = 0; i <= 255; i++)
			{
				this.unzftab[i] = 0;
			}
			for (int i = 0; i <= 255; i++)
			{
				array[i] = (char)i;
			}
			this.last = -1;
			if (num4 == 0)
			{
				num3++;
				num4 = BZip2Constants_Fields.G_SIZE;
			}
			num4--;
			int num5 = (int)this.selector[num3];
			int num6 = this.minLens[num5];
			int j;
			int num8;
			for (j = this.bsR(num6); j > this.limit[num5][num6]; j = (j << 1 | num8))
			{
				num6++;
				while (this.bsLive < 1)
				{
					char c = '\0';
					try
					{
						c = (char)this.bsStream.ReadByte();
					}
					catch (IOException)
					{
						CBZip2InputStream.compressedStreamEOF();
					}
					if ((short)c == -1)
					{
						CBZip2InputStream.compressedStreamEOF();
					}
					int num7 = (int)c;
					this.bsBuff = (this.bsBuff << 8 | (num7 & 255));
					this.bsLive += 8;
				}
				num8 = (this.bsBuff >> this.bsLive - 1 & 1);
				this.bsLive--;
			}
			int num9 = this.perm[num5][j - this.base_Renamed[num5][num6]];
			while (num9 != num2)
			{
				if (num9 == BZip2Constants_Fields.RUNA || num9 == BZip2Constants_Fields.RUNB)
				{
					int k = -1;
					int num10 = 1;
					do
					{
						if (num9 == BZip2Constants_Fields.RUNA)
						{
							k += num10;
						}
						else if (num9 == BZip2Constants_Fields.RUNB)
						{
							k += 2 * num10;
						}
						num10 *= 2;
						if (num4 == 0)
						{
							num3++;
							num4 = BZip2Constants_Fields.G_SIZE;
						}
						num4--;
						int num11 = (int)this.selector[num3];
						int num12 = this.minLens[num11];
						int l;
						int num14;
						for (l = this.bsR(num12); l > this.limit[num11][num12]; l = (l << 1 | num14))
						{
							num12++;
							while (this.bsLive < 1)
							{
								char c2 = '\0';
								try
								{
									c2 = (char)this.bsStream.ReadByte();
								}
								catch (IOException)
								{
									CBZip2InputStream.compressedStreamEOF();
								}
								if ((short)c2 == -1)
								{
									CBZip2InputStream.compressedStreamEOF();
								}
								int num13 = (int)c2;
								this.bsBuff = (this.bsBuff << 8 | (num13 & 255));
								this.bsLive += 8;
							}
							num14 = (this.bsBuff >> this.bsLive - 1 & 1);
							this.bsLive--;
						}
						num9 = this.perm[num11][l - this.base_Renamed[num11][num12]];
					}
					while (num9 == BZip2Constants_Fields.RUNA || num9 == BZip2Constants_Fields.RUNB);
					k++;
					char c3 = this.seqToUnseq[(int)array[0]];
					this.unzftab[(int)c3] += k;
					while (k > 0)
					{
						this.last++;
						this.ll8[this.last] = c3;
						k--;
					}
					if (this.last >= num)
					{
						CBZip2InputStream.blockOverrun();
					}
				}
				else
				{
					this.last++;
					if (this.last >= num)
					{
						CBZip2InputStream.blockOverrun();
					}
					char c4 = array[num9 - 1];
					this.unzftab[(int)this.seqToUnseq[(int)c4]]++;
					this.ll8[this.last] = this.seqToUnseq[(int)c4];
					int m;
					for (m = num9 - 1; m > 3; m -= 4)
					{
						array[m] = array[m - 1];
						array[m - 1] = array[m - 2];
						array[m - 2] = array[m - 3];
						array[m - 3] = array[m - 4];
					}
					while (m > 0)
					{
						array[m] = array[m - 1];
						m--;
					}
					array[0] = c4;
					if (num4 == 0)
					{
						num3++;
						num4 = BZip2Constants_Fields.G_SIZE;
					}
					num4--;
					int num15 = (int)this.selector[num3];
					int num16 = this.minLens[num15];
					int n;
					int num18;
					for (n = this.bsR(num16); n > this.limit[num15][num16]; n = (n << 1 | num18))
					{
						num16++;
						while (this.bsLive < 1)
						{
							char c5 = '\0';
							try
							{
								c5 = (char)this.bsStream.ReadByte();
							}
							catch (IOException)
							{
								CBZip2InputStream.compressedStreamEOF();
							}
							int num17 = (int)c5;
							this.bsBuff = (this.bsBuff << 8 | (num17 & 255));
							this.bsLive += 8;
						}
						num18 = (this.bsBuff >> this.bsLive - 1 & 1);
						this.bsLive--;
					}
					num9 = this.perm[num15][n - this.base_Renamed[num15][num16]];
				}
			}
		}

		// Token: 0x0600018B RID: 395 RVA: 0x000121D4 File Offset: 0x000111D4
		private void setupBlock()
		{
			int[] array = new int[257];
			array[0] = 0;
			this.i = 1;
			while (this.i <= 256)
			{
				array[this.i] = this.unzftab[this.i - 1];
				this.i++;
			}
			this.i = 1;
			while (this.i <= 256)
			{
				array[this.i] += array[this.i - 1];
				this.i++;
			}
			this.i = 0;
			while (this.i <= this.last)
			{
				char c = this.ll8[this.i];
				this.tt[array[(int)c]] = this.i;
				array[(int)c]++;
				this.i++;
			}
			this.tPos = this.tt[this.origPtr];
			this.count = 0;
			this.i2 = 0;
			this.ch2 = 256;
			if (this.blockRandomised)
			{
				this.rNToGo = 0;
				this.rTPos = 0;
				this.setupRandPartA();
				return;
			}
			this.setupNoRandPartA();
		}

		// Token: 0x0600018C RID: 396 RVA: 0x0001231C File Offset: 0x0001131C
		private void setupRandPartA()
		{
			if (this.i2 <= this.last)
			{
				this.chPrev = this.ch2;
				this.ch2 = (int)this.ll8[this.tPos];
				this.tPos = this.tt[this.tPos];
				if (this.rNToGo == 0)
				{
					this.rNToGo = BZip2Constants_Fields.rNums[this.rTPos];
					this.rTPos++;
					if (this.rTPos == 512)
					{
						this.rTPos = 0;
					}
				}
				this.rNToGo--;
				this.ch2 ^= ((this.rNToGo == 1) ? 1 : 0);
				this.i2++;
				this.currentChar = this.ch2;
				this.currentState = 3;
				this.mCrc.updateCRC(this.ch2);
				return;
			}
			this.endBlock();
			this.initBlock();
			this.setupBlock();
		}

		// Token: 0x0600018D RID: 397 RVA: 0x00012418 File Offset: 0x00011418
		private void setupNoRandPartA()
		{
			if (this.i2 <= this.last)
			{
				this.chPrev = this.ch2;
				this.ch2 = (int)this.ll8[this.tPos];
				this.tPos = this.tt[this.tPos];
				this.i2++;
				this.currentChar = this.ch2;
				this.currentState = 6;
				this.mCrc.updateCRC(this.ch2);
				return;
			}
			this.endBlock();
			this.initBlock();
			this.setupBlock();
		}

		// Token: 0x0600018E RID: 398 RVA: 0x000124AC File Offset: 0x000114AC
		private void setupRandPartB()
		{
			if (this.ch2 != this.chPrev)
			{
				this.currentState = 2;
				this.count = 1;
				this.setupRandPartA();
				return;
			}
			this.count++;
			if (this.count >= 4)
			{
				this.z = this.ll8[this.tPos];
				this.tPos = this.tt[this.tPos];
				if (this.rNToGo == 0)
				{
					this.rNToGo = BZip2Constants_Fields.rNums[this.rTPos];
					this.rTPos++;
					if (this.rTPos == 512)
					{
						this.rTPos = 0;
					}
				}
				this.rNToGo--;
				this.z ^= ((this.rNToGo == 1) ? '\u0001' : '\0');
				this.j2 = 0;
				this.currentState = 4;
				this.setupRandPartC();
				return;
			}
			this.currentState = 2;
			this.setupRandPartA();
		}

		// Token: 0x0600018F RID: 399 RVA: 0x000125A4 File Offset: 0x000115A4
		private void setupRandPartC()
		{
			if (this.j2 < (int)this.z)
			{
				this.currentChar = this.ch2;
				this.mCrc.updateCRC(this.ch2);
				this.j2++;
				return;
			}
			this.currentState = 2;
			this.i2++;
			this.count = 0;
			this.setupRandPartA();
		}

		// Token: 0x06000190 RID: 400 RVA: 0x00012610 File Offset: 0x00011610
		private void setupNoRandPartB()
		{
			if (this.ch2 != this.chPrev)
			{
				this.currentState = 5;
				this.count = 1;
				this.setupNoRandPartA();
				return;
			}
			this.count++;
			if (this.count >= 4)
			{
				this.z = this.ll8[this.tPos];
				this.tPos = this.tt[this.tPos];
				this.currentState = 7;
				this.j2 = 0;
				this.setupNoRandPartC();
				return;
			}
			this.currentState = 5;
			this.setupNoRandPartA();
		}

		// Token: 0x06000191 RID: 401 RVA: 0x000126A0 File Offset: 0x000116A0
		private void setupNoRandPartC()
		{
			if (this.j2 < (int)this.z)
			{
				this.currentChar = this.ch2;
				this.mCrc.updateCRC(this.ch2);
				this.j2++;
				return;
			}
			this.currentState = 5;
			this.i2++;
			this.count = 0;
			this.setupNoRandPartA();
		}

		// Token: 0x06000192 RID: 402 RVA: 0x00012709 File Offset: 0x00011709
		public override void Flush()
		{
		}

		// Token: 0x06000193 RID: 403 RVA: 0x0001270B File Offset: 0x0001170B
		public override long Seek(long offset, SeekOrigin origin)
		{
			return 0L;
		}

		// Token: 0x06000194 RID: 404 RVA: 0x0001270F File Offset: 0x0001170F
		public override void SetLength(long value)
		{
		}

		// Token: 0x06000195 RID: 405 RVA: 0x00012714 File Offset: 0x00011714
		public override int Read(byte[] buffer, int offset, int count)
		{
			int i;
			for (i = offset; i < count; i++)
			{
				int num = this.ReadByte();
				if (num == -1)
				{
					return i;
				}
				buffer[i] = (byte)num;
			}
			return i;
		}

		// Token: 0x06000196 RID: 406 RVA: 0x00012740 File Offset: 0x00011740
		public override void Write(byte[] buffer, int offset, int count)
		{
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000197 RID: 407 RVA: 0x00012742 File Offset: 0x00011742
		public override bool CanRead
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000198 RID: 408 RVA: 0x00012745 File Offset: 0x00011745
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000199 RID: 409 RVA: 0x00012748 File Offset: 0x00011748
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x0600019A RID: 410 RVA: 0x0001274B File Offset: 0x0001174B
		public override long Length
		{
			get
			{
				return (long)this.tt.Length;
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x0600019B RID: 411 RVA: 0x00012758 File Offset: 0x00011758
		// (set) Token: 0x0600019C RID: 412 RVA: 0x00012790 File Offset: 0x00011790
		public override long Position
		{
			get
			{
				long result;
				if (this.bsStream != null && this.bsStream.CanSeek)
				{
					result = this.bsStream.Position;
				}
				else
				{
					result = this.position;
				}
				return result;
			}
			set
			{
			}
		}

		// Token: 0x040000D5 RID: 213
		private const int START_BLOCK_STATE = 1;

		// Token: 0x040000D6 RID: 214
		private const int RAND_PART_A_STATE = 2;

		// Token: 0x040000D7 RID: 215
		private const int RAND_PART_B_STATE = 3;

		// Token: 0x040000D8 RID: 216
		private const int RAND_PART_C_STATE = 4;

		// Token: 0x040000D9 RID: 217
		private const int NO_RAND_PART_A_STATE = 5;

		// Token: 0x040000DA RID: 218
		private const int NO_RAND_PART_B_STATE = 6;

		// Token: 0x040000DB RID: 219
		private const int NO_RAND_PART_C_STATE = 7;

		// Token: 0x040000DC RID: 220
		private int last;

		// Token: 0x040000DD RID: 221
		private int origPtr;

		// Token: 0x040000DE RID: 222
		private int blockSize100k;

		// Token: 0x040000DF RID: 223
		private bool blockRandomised;

		// Token: 0x040000E0 RID: 224
		private int bsBuff;

		// Token: 0x040000E1 RID: 225
		private int bsLive;

		// Token: 0x040000E2 RID: 226
		private CRC mCrc = new CRC();

		// Token: 0x040000E3 RID: 227
		private bool[] inUse = new bool[256];

		// Token: 0x040000E4 RID: 228
		private int nInUse;

		// Token: 0x040000E5 RID: 229
		private char[] seqToUnseq = new char[256];

		// Token: 0x040000E6 RID: 230
		private char[] unseqToSeq = new char[256];

		// Token: 0x040000E7 RID: 231
		private char[] selector = new char[BZip2Constants_Fields.MAX_SELECTORS];

		// Token: 0x040000E8 RID: 232
		private char[] selectorMtf = new char[BZip2Constants_Fields.MAX_SELECTORS];

		// Token: 0x040000E9 RID: 233
		private int[] tt;

		// Token: 0x040000EA RID: 234
		private char[] ll8;

		// Token: 0x040000EB RID: 235
		private int[] unzftab = new int[256];

		// Token: 0x040000EC RID: 236
		private int[][] limit = new int[BZip2Constants_Fields.N_GROUPS][];

		// Token: 0x040000ED RID: 237
		private int[][] base_Renamed = new int[BZip2Constants_Fields.N_GROUPS][];

		// Token: 0x040000EE RID: 238
		private int[][] perm = new int[BZip2Constants_Fields.N_GROUPS][];

		// Token: 0x040000EF RID: 239
		private int[] minLens = new int[BZip2Constants_Fields.N_GROUPS];

		// Token: 0x040000F0 RID: 240
		private Stream bsStream;

		// Token: 0x040000F1 RID: 241
		private bool streamEnd;

		// Token: 0x040000F2 RID: 242
		private int currentChar = -1;

		// Token: 0x040000F3 RID: 243
		private int currentState = 1;

		// Token: 0x040000F4 RID: 244
		private int storedBlockCRC;

		// Token: 0x040000F5 RID: 245
		private int storedCombinedCRC;

		// Token: 0x040000F6 RID: 246
		private int computedBlockCRC;

		// Token: 0x040000F7 RID: 247
		private int computedCombinedCRC;

		// Token: 0x040000F8 RID: 248
		internal int i2;

		// Token: 0x040000F9 RID: 249
		internal int count;

		// Token: 0x040000FA RID: 250
		internal int chPrev;

		// Token: 0x040000FB RID: 251
		internal int ch2;

		// Token: 0x040000FC RID: 252
		internal int i;

		// Token: 0x040000FD RID: 253
		internal int tPos;

		// Token: 0x040000FE RID: 254
		internal int rNToGo;

		// Token: 0x040000FF RID: 255
		internal int rTPos;

		// Token: 0x04000100 RID: 256
		internal int j2;

		// Token: 0x04000101 RID: 257
		internal char z;

		// Token: 0x04000102 RID: 258
		private long position;
	}
}
