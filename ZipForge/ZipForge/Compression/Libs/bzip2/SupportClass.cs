using System;

namespace ComponentAce.Compression.Libs.bzip2
{
	// Token: 0x02000029 RID: 41
	internal class SupportClass
	{
		// Token: 0x060001D2 RID: 466 RVA: 0x0001505C File Offset: 0x0001405C
		public static int URShift(int number, int bits)
		{
			if (number >= 0)
			{
				return number >> bits;
			}
			return (number >> bits) + (2 << ~bits);
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x00015077 File Offset: 0x00014077
		public static int URShift(int number, long bits)
		{
			return SupportClass.URShift(number, (int)bits);
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x00015081 File Offset: 0x00014081
		public static long URShift(long number, int bits)
		{
			if (number >= 0L)
			{
				return number >> bits;
			}
			return (number >> bits) + (2L << ~bits);
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x0001509E File Offset: 0x0001409E
		public static long URShift(long number, long bits)
		{
			return SupportClass.URShift(number, (int)bits);
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x000150A8 File Offset: 0x000140A8
		public static long Identity(long literal)
		{
			return literal;
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x000150AB File Offset: 0x000140AB
		public static ulong Identity(ulong literal)
		{
			return literal;
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x000150AE File Offset: 0x000140AE
		public static float Identity(float literal)
		{
			return literal;
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x000150B1 File Offset: 0x000140B1
		public static double Identity(double literal)
		{
			return literal;
		}
	}
}
