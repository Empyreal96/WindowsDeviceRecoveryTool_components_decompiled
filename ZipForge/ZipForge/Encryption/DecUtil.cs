using System;
using System.Collections;

namespace ComponentAce.Encryption
{
	// Token: 0x02000012 RID: 18
	internal class DecUtil
	{
		// Token: 0x06000051 RID: 81 RVA: 0x0000BA2B File Offset: 0x0000AA2B
		public static int TableFind(char Value, string Table, int Len)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000052 RID: 82 RVA: 0x0000BA32 File Offset: 0x0000AA32
		public static int ROL(uint Value, int Shift)
		{
			return (int)(Value << Shift | Value >> 32 - Shift);
		}

		// Token: 0x06000053 RID: 83 RVA: 0x0000BA44 File Offset: 0x0000AA44
		public static int ROLADD(int Value, int Add, int Shift)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000054 RID: 84 RVA: 0x0000BA4B File Offset: 0x0000AA4B
		public static int ROLSUB(int Value, int Sub, int Shift)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000055 RID: 85 RVA: 0x0000BA52 File Offset: 0x0000AA52
		public static int ROR(int Value, int Shift)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000056 RID: 86 RVA: 0x0000BA59 File Offset: 0x0000AA59
		public static int RORADD(int Value, int Add, int Shift)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000057 RID: 87 RVA: 0x0000BA60 File Offset: 0x0000AA60
		public static int RORSUB(int Value, int Sub, int Shift)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000058 RID: 88 RVA: 0x0000BA67 File Offset: 0x0000AA67
		public static void SwapIntBuf(object Source, object Dest, int Count)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000059 RID: 89 RVA: 0x0000BA6E File Offset: 0x0000AA6E
		public static void BSwapIntBuf(object Source, object Dest, int Count)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600005A RID: 90 RVA: 0x0000BA75 File Offset: 0x0000AA75
		public static int SwapBits(int Value)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600005B RID: 91 RVA: 0x0000BA7C File Offset: 0x0000AA7C
		public static int LSBit(int Value)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600005C RID: 92 RVA: 0x0000BA83 File Offset: 0x0000AA83
		public static int MSBit(int Value)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600005D RID: 93 RVA: 0x0000BA8A File Offset: 0x0000AA8A
		public static int OneBit(int Value)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600005E RID: 94 RVA: 0x0000BA91 File Offset: 0x0000AA91
		public static int MemCompare(object P1, object P2, int Size)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600005F RID: 95 RVA: 0x0000BA98 File Offset: 0x0000AA98
		public static void XORBuffers(byte[] Src1, int Src1Offset, byte[] Src2, int Src2Offset, int Size, byte[] Dest, int DestOffset)
		{
			while (Size > 0)
			{
				Dest[DestOffset++] = (Src1[Src1Offset++] ^ Src2[Src2Offset++]);
				Size--;
			}
		}

		// Token: 0x06000060 RID: 96 RVA: 0x0000BAC3 File Offset: 0x0000AAC3
		public static void DoProgress(object Sender, int Current, int Maximal)
		{
		}

		// Token: 0x06000061 RID: 97 RVA: 0x0000BAC8 File Offset: 0x0000AAC8
		public static int RndXORBuffer(int Seed, ref byte[] Buffer, int Size)
		{
			return 0;
		}

		// Token: 0x06000062 RID: 98 RVA: 0x0000BAD8 File Offset: 0x0000AAD8
		public static int RndTimeSeed()
		{
			return 0;
		}

		// Token: 0x06000063 RID: 99 RVA: 0x0000BAE8 File Offset: 0x0000AAE8
		public static ushort CRC16(ushort CRC, object Data, int DataSize)
		{
			return 0;
		}

		// Token: 0x06000064 RID: 100 RVA: 0x0000BAF8 File Offset: 0x0000AAF8
		public static int SwapInteger(int i)
		{
			byte b = (byte)(i >> 24 & 255);
			byte b2 = (byte)(i >> 16 & 255);
			byte b3 = (byte)(i >> 8 & 255);
			byte b4 = (byte)(i & 255);
			return (int)b + ((int)b2 << 8) + ((int)b3 << 16) + ((int)b4 << 24);
		}

		// Token: 0x06000065 RID: 101 RVA: 0x0000BB40 File Offset: 0x0000AB40
		public static uint SwapInteger(uint i)
		{
			byte b = (byte)(i >> 24 & 255U);
			byte b2 = (byte)(i >> 16 & 255U);
			byte b3 = (byte)(i >> 8 & 255U);
			byte b4 = (byte)(i & 255U);
			return (uint)((int)b + ((int)b2 << 8) + ((int)b3 << 16) + ((int)b4 << 24));
		}

		// Token: 0x06000066 RID: 102 RVA: 0x0000BB88 File Offset: 0x0000AB88
		public static byte[] GetTestVector()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000067 RID: 103 RVA: 0x0000BB90 File Offset: 0x0000AB90
		public static byte[] Copy2DArrayToFlatArray(uint[,] source)
		{
			byte[] array = new byte[(source.GetUpperBound(0) - source.GetLowerBound(0) + 1) * (source.GetUpperBound(1) - source.GetLowerBound(1) + 1) * 4];
			for (int i = source.GetLowerBound(0); i <= source.GetUpperBound(0); i++)
			{
				for (int j = source.GetLowerBound(1); j <= source.GetUpperBound(1); j++)
				{
					int index = ((source.GetUpperBound(1) - source.GetLowerBound(1) + 1) * i + j) * 4;
					BitConverter.GetBytes(source[i, j]).CopyTo(array, index);
				}
			}
			return array;
		}

		// Token: 0x06000068 RID: 104 RVA: 0x0000BC28 File Offset: 0x0000AC28
		public static byte[] IntArrayToByteArray(uint[] source)
		{
			byte[] array = new byte[source.Length * 4];
			for (int i = 0; i < source.Length; i++)
			{
				BitConverter.GetBytes(source[i]).CopyTo(array, i * 4);
			}
			return array;
		}

		// Token: 0x04000088 RID: 136
		public const int fmtDEFAULT = -1;

		// Token: 0x04000089 RID: 137
		public const int fmtNONE = 0;

		// Token: 0x0400008A RID: 138
		public const int fmtCOPY = 1;

		// Token: 0x0400008B RID: 139
		public const int fmtHEX = 16;

		// Token: 0x0400008C RID: 140
		public const int fmtHEXL = 1016;

		// Token: 0x0400008D RID: 141
		public const int fmtMIME64 = 4196;

		// Token: 0x0400008E RID: 142
		public const int fmtUU = 21845;

		// Token: 0x0400008F RID: 143
		public const int fmtXX = 22616;

		// Token: 0x04000090 RID: 144
		public const bool InitTestIsOk = true;

		// Token: 0x04000091 RID: 145
		public const ushort IdentityBase = 4660;

		// Token: 0x04000092 RID: 146
		public const object Progress = null;

		// Token: 0x04000093 RID: 147
		public const int FCPUType = 0;

		// Token: 0x04000094 RID: 148
		public const ArrayList FStrFMTs = null;

		// Token: 0x04000095 RID: 149
		public const int FStrFMT = 4196;
	}
}
