using System;
using System.IO;
using System.Text;

namespace ComponentAce.Compression.Libs.ZLib
{
	// Token: 0x020000BE RID: 190
	internal class ZLibUtil
	{
		// Token: 0x0600081B RID: 2075 RVA: 0x00035310 File Offset: 0x00034310
		public static long Identity(long literal)
		{
			return literal;
		}

		// Token: 0x0600081C RID: 2076 RVA: 0x00035313 File Offset: 0x00034313
		public static ulong Identity(ulong literal)
		{
			return literal;
		}

		// Token: 0x0600081D RID: 2077 RVA: 0x00035316 File Offset: 0x00034316
		internal static float Identity(float literal)
		{
			return literal;
		}

		// Token: 0x0600081E RID: 2078 RVA: 0x00035319 File Offset: 0x00034319
		internal static double Identity(double literal)
		{
			return literal;
		}

		// Token: 0x0600081F RID: 2079 RVA: 0x0003531C File Offset: 0x0003431C
		internal static int URShift(int number, int bits)
		{
			if (number >= 0)
			{
				return number >> bits;
			}
			return (number >> bits) + (2 << ~bits);
		}

		// Token: 0x06000820 RID: 2080 RVA: 0x00035337 File Offset: 0x00034337
		internal static int URShift(int number, long bits)
		{
			return ZLibUtil.URShift(number, (int)bits);
		}

		// Token: 0x06000821 RID: 2081 RVA: 0x00035341 File Offset: 0x00034341
		internal static long URShift(long number, int bits)
		{
			if (number >= 0L)
			{
				return number >> bits;
			}
			return (number >> bits) + (2L << ~bits);
		}

		// Token: 0x06000822 RID: 2082 RVA: 0x0003535E File Offset: 0x0003435E
		internal static long URShift(long number, long bits)
		{
			return ZLibUtil.URShift(number, (int)bits);
		}

		// Token: 0x06000823 RID: 2083 RVA: 0x00035368 File Offset: 0x00034368
		internal static int ReadInput(Stream sourceStream, byte[] target, int start, int count)
		{
			if (target.Length == 0)
			{
				return 0;
			}
			byte[] array = new byte[target.Length];
			int num = sourceStream.Read(array, start, count);
			if (num == 0)
			{
				return -1;
			}
			for (int i = start; i < start + num; i++)
			{
				target[i] = array[i];
			}
			return num;
		}

		// Token: 0x06000824 RID: 2084 RVA: 0x000353AC File Offset: 0x000343AC
		internal static int ReadInput(TextReader sourceTextReader, byte[] target, int start, int count)
		{
			if (target.Length == 0)
			{
				return 0;
			}
			char[] array = new char[target.Length];
			int num = sourceTextReader.Read(array, start, count);
			if (num == 0)
			{
				return -1;
			}
			for (int i = start; i < start + num; i++)
			{
				target[i] = (byte)array[i];
			}
			return num;
		}

		// Token: 0x06000825 RID: 2085 RVA: 0x000353EE File Offset: 0x000343EE
		internal static byte[] ToByteArray(string sourceString)
		{
			return Encoding.UTF8.GetBytes(sourceString);
		}

		// Token: 0x06000826 RID: 2086 RVA: 0x000353FB File Offset: 0x000343FB
		internal static char[] ToCharArray(byte[] byteArray)
		{
			return Encoding.UTF8.GetChars(byteArray);
		}

		// Token: 0x04000526 RID: 1318
		internal const int MAX_WBITS = 15;

		// Token: 0x04000527 RID: 1319
		internal const int PRESET_DICT = 32;

		// Token: 0x04000528 RID: 1320
		internal const int zLibBufSize = 1048576;

		// Token: 0x04000529 RID: 1321
		internal const int Z_DEFLATED = 8;

		// Token: 0x0400052A RID: 1322
		internal const int BL_CODES = 19;

		// Token: 0x0400052B RID: 1323
		internal const int D_CODES = 30;

		// Token: 0x0400052C RID: 1324
		internal const int LITERALS = 256;

		// Token: 0x0400052D RID: 1325
		internal const int LENGTH_CODES = 29;

		// Token: 0x0400052E RID: 1326
		internal const int L_CODES = 286;

		// Token: 0x0400052F RID: 1327
		internal const int HEAP_SIZE = 573;

		// Token: 0x04000530 RID: 1328
		internal const int MAX_BL_BITS = 7;

		// Token: 0x04000531 RID: 1329
		internal const int END_BLOCK = 256;

		// Token: 0x04000532 RID: 1330
		internal const int REP_3_6 = 16;

		// Token: 0x04000533 RID: 1331
		internal const int REPZ_3_10 = 17;

		// Token: 0x04000534 RID: 1332
		internal const int REPZ_11_138 = 18;

		// Token: 0x04000535 RID: 1333
		internal const int Buf_size = 16;

		// Token: 0x04000536 RID: 1334
		internal const int DIST_CODE_LEN = 512;

		// Token: 0x04000537 RID: 1335
		internal static readonly byte[] mark = new byte[]
		{
			0,
			0,
			(byte)ZLibUtil.Identity(255L),
			(byte)ZLibUtil.Identity(255L)
		};

		// Token: 0x04000538 RID: 1336
		internal static readonly string[] z_errmsg = new string[]
		{
			"need dictionary",
			"stream End",
			"",
			"file error",
			"stream error",
			"data error",
			"insufficient memory",
			"buffer error",
			"incompatible version",
			""
		};

		// Token: 0x04000539 RID: 1337
		internal static readonly int[] inflate_mask = new int[]
		{
			0,
			1,
			3,
			7,
			15,
			31,
			63,
			127,
			255,
			511,
			1023,
			2047,
			4095,
			8191,
			16383,
			32767,
			65535
		};

		// Token: 0x0400053A RID: 1338
		internal static readonly int[] border = new int[]
		{
			16,
			17,
			18,
			0,
			8,
			7,
			9,
			6,
			10,
			5,
			11,
			4,
			12,
			3,
			13,
			2,
			14,
			1,
			15
		};

		// Token: 0x0400053B RID: 1339
		internal static readonly int[] extra_lbits = new int[]
		{
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			1,
			1,
			1,
			1,
			2,
			2,
			2,
			2,
			3,
			3,
			3,
			3,
			4,
			4,
			4,
			4,
			5,
			5,
			5,
			5,
			0
		};

		// Token: 0x0400053C RID: 1340
		internal static readonly int[] extra_dbits = new int[]
		{
			0,
			0,
			0,
			0,
			1,
			1,
			2,
			2,
			3,
			3,
			4,
			4,
			5,
			5,
			6,
			6,
			7,
			7,
			8,
			8,
			9,
			9,
			10,
			10,
			11,
			11,
			12,
			12,
			13,
			13
		};

		// Token: 0x0400053D RID: 1341
		internal static readonly int[] extra_blbits = new int[]
		{
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			2,
			3,
			7
		};

		// Token: 0x0400053E RID: 1342
		internal static readonly byte[] bl_order = new byte[]
		{
			16,
			17,
			18,
			0,
			8,
			7,
			9,
			6,
			10,
			5,
			11,
			4,
			12,
			3,
			13,
			2,
			14,
			1,
			15
		};

		// Token: 0x0400053F RID: 1343
		internal static readonly byte[] _dist_code = new byte[]
		{
			0,
			1,
			2,
			3,
			4,
			4,
			5,
			5,
			6,
			6,
			6,
			6,
			7,
			7,
			7,
			7,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			9,
			9,
			9,
			9,
			9,
			9,
			9,
			9,
			10,
			10,
			10,
			10,
			10,
			10,
			10,
			10,
			10,
			10,
			10,
			10,
			10,
			10,
			10,
			10,
			11,
			11,
			11,
			11,
			11,
			11,
			11,
			11,
			11,
			11,
			11,
			11,
			11,
			11,
			11,
			11,
			12,
			12,
			12,
			12,
			12,
			12,
			12,
			12,
			12,
			12,
			12,
			12,
			12,
			12,
			12,
			12,
			12,
			12,
			12,
			12,
			12,
			12,
			12,
			12,
			12,
			12,
			12,
			12,
			12,
			12,
			12,
			12,
			13,
			13,
			13,
			13,
			13,
			13,
			13,
			13,
			13,
			13,
			13,
			13,
			13,
			13,
			13,
			13,
			13,
			13,
			13,
			13,
			13,
			13,
			13,
			13,
			13,
			13,
			13,
			13,
			13,
			13,
			13,
			13,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			0,
			0,
			16,
			17,
			18,
			18,
			19,
			19,
			20,
			20,
			20,
			20,
			21,
			21,
			21,
			21,
			22,
			22,
			22,
			22,
			22,
			22,
			22,
			22,
			23,
			23,
			23,
			23,
			23,
			23,
			23,
			23,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29
		};

		// Token: 0x04000540 RID: 1344
		internal static readonly byte[] _length_code = new byte[]
		{
			0,
			1,
			2,
			3,
			4,
			5,
			6,
			7,
			8,
			8,
			9,
			9,
			10,
			10,
			11,
			11,
			12,
			12,
			12,
			12,
			13,
			13,
			13,
			13,
			14,
			14,
			14,
			14,
			15,
			15,
			15,
			15,
			16,
			16,
			16,
			16,
			16,
			16,
			16,
			16,
			17,
			17,
			17,
			17,
			17,
			17,
			17,
			17,
			18,
			18,
			18,
			18,
			18,
			18,
			18,
			18,
			19,
			19,
			19,
			19,
			19,
			19,
			19,
			19,
			20,
			20,
			20,
			20,
			20,
			20,
			20,
			20,
			20,
			20,
			20,
			20,
			20,
			20,
			20,
			20,
			21,
			21,
			21,
			21,
			21,
			21,
			21,
			21,
			21,
			21,
			21,
			21,
			21,
			21,
			21,
			21,
			22,
			22,
			22,
			22,
			22,
			22,
			22,
			22,
			22,
			22,
			22,
			22,
			22,
			22,
			22,
			22,
			23,
			23,
			23,
			23,
			23,
			23,
			23,
			23,
			23,
			23,
			23,
			23,
			23,
			23,
			23,
			23,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			28
		};

		// Token: 0x04000541 RID: 1345
		internal static readonly int[] base_length = new int[]
		{
			0,
			1,
			2,
			3,
			4,
			5,
			6,
			7,
			8,
			10,
			12,
			14,
			16,
			20,
			24,
			28,
			32,
			40,
			48,
			56,
			64,
			80,
			96,
			112,
			128,
			160,
			192,
			224,
			0
		};

		// Token: 0x04000542 RID: 1346
		internal static readonly int[] base_dist = new int[]
		{
			0,
			1,
			2,
			3,
			4,
			6,
			8,
			12,
			16,
			24,
			32,
			48,
			64,
			96,
			128,
			192,
			256,
			384,
			512,
			768,
			1024,
			1536,
			2048,
			3072,
			4096,
			6144,
			8192,
			12288,
			16384,
			24576
		};

		// Token: 0x020000BF RID: 191
		internal class CopyLargeArrayToSmall
		{
			// Token: 0x06000829 RID: 2089 RVA: 0x00035B73 File Offset: 0x00034B73
			public static void Initialize(byte[] srcBuf, int srcOff, int srcDataLen, byte[] destBuff, int destOff, int destLen)
			{
				ZLibUtil.CopyLargeArrayToSmall.srcBuf = srcBuf;
				ZLibUtil.CopyLargeArrayToSmall.srcOff = srcOff;
				ZLibUtil.CopyLargeArrayToSmall.srcDataLen = srcDataLen;
				ZLibUtil.CopyLargeArrayToSmall.destBuff = destBuff;
				ZLibUtil.CopyLargeArrayToSmall.destOff = destOff;
				ZLibUtil.CopyLargeArrayToSmall.destLen = destLen;
				ZLibUtil.CopyLargeArrayToSmall.nWritten = 0;
			}

			// Token: 0x0600082A RID: 2090 RVA: 0x00035BA1 File Offset: 0x00034BA1
			public static int GetRemainingDataSize()
			{
				return ZLibUtil.CopyLargeArrayToSmall.srcDataLen;
			}

			// Token: 0x0600082B RID: 2091 RVA: 0x00035BA8 File Offset: 0x00034BA8
			public static int CopyData()
			{
				if (ZLibUtil.CopyLargeArrayToSmall.srcDataLen > ZLibUtil.CopyLargeArrayToSmall.destLen)
				{
					Array.Copy(ZLibUtil.CopyLargeArrayToSmall.srcBuf, ZLibUtil.CopyLargeArrayToSmall.srcOff, ZLibUtil.CopyLargeArrayToSmall.destBuff, ZLibUtil.CopyLargeArrayToSmall.destOff, ZLibUtil.CopyLargeArrayToSmall.destLen);
					ZLibUtil.CopyLargeArrayToSmall.srcDataLen -= ZLibUtil.CopyLargeArrayToSmall.destLen;
					ZLibUtil.CopyLargeArrayToSmall.srcOff += ZLibUtil.CopyLargeArrayToSmall.destLen;
					ZLibUtil.CopyLargeArrayToSmall.nWritten = ZLibUtil.CopyLargeArrayToSmall.destLen;
					return ZLibUtil.CopyLargeArrayToSmall.nWritten;
				}
				Array.Copy(ZLibUtil.CopyLargeArrayToSmall.srcBuf, ZLibUtil.CopyLargeArrayToSmall.srcOff, ZLibUtil.CopyLargeArrayToSmall.destBuff, ZLibUtil.CopyLargeArrayToSmall.destOff, ZLibUtil.CopyLargeArrayToSmall.srcDataLen);
				ZLibUtil.CopyLargeArrayToSmall.nWritten = ZLibUtil.CopyLargeArrayToSmall.srcDataLen;
				ZLibUtil.CopyLargeArrayToSmall.srcDataLen = 0;
				return ZLibUtil.CopyLargeArrayToSmall.nWritten;
			}

			// Token: 0x04000543 RID: 1347
			private static byte[] srcBuf;

			// Token: 0x04000544 RID: 1348
			private static int srcOff;

			// Token: 0x04000545 RID: 1349
			private static int srcDataLen;

			// Token: 0x04000546 RID: 1350
			private static byte[] destBuff;

			// Token: 0x04000547 RID: 1351
			private static int destOff;

			// Token: 0x04000548 RID: 1352
			private static int destLen;

			// Token: 0x04000549 RID: 1353
			private static int nWritten;
		}
	}
}
