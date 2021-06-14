using System;
using System.IO;

namespace ComponentAce.Compression.Libs.PPMd
{
	// Token: 0x0200004E RID: 78
	internal class Coder
	{
		// Token: 0x06000319 RID: 793 RVA: 0x00019FA2 File Offset: 0x00018FA2
		public static void RangeEncoderInitialize()
		{
			Coder.low = 0U;
			Coder.range = uint.MaxValue;
		}

		// Token: 0x0600031A RID: 794 RVA: 0x00019FB0 File Offset: 0x00018FB0
		public static void RangeEncoderNormalize(Stream stream)
		{
			for (;;)
			{
				if ((Coder.low ^ Coder.low + Coder.range) >= 16777216U)
				{
					if (Coder.range >= 32768U)
					{
						break;
					}
					Coder.range = ((uint)(-(uint)((ulong)Coder.low)) & 32767U);
				}
				stream.WriteByte((byte)(Coder.low >> 24));
				Coder.range <<= 8;
				Coder.low <<= 8;
			}
		}

		// Token: 0x0600031B RID: 795 RVA: 0x0001A01F File Offset: 0x0001901F
		public static void RangeEncodeSymbol()
		{
			Coder.low += Coder.LowCount * (Coder.range /= Coder.Scale);
			Coder.range *= Coder.HighCount - Coder.LowCount;
		}

		// Token: 0x0600031C RID: 796 RVA: 0x0001A059 File Offset: 0x00019059
		public static void RangeShiftEncodeSymbol(int rangeShift)
		{
			Coder.low += Coder.LowCount * (Coder.range >>= rangeShift);
			Coder.range *= Coder.HighCount - Coder.LowCount;
		}

		// Token: 0x0600031D RID: 797 RVA: 0x0001A094 File Offset: 0x00019094
		public static void RangeEncoderFlush(Stream stream)
		{
			for (uint num = 0U; num < 4U; num += 1U)
			{
				stream.WriteByte((byte)(Coder.low >> 24));
				Coder.low <<= 8;
			}
		}

		// Token: 0x0600031E RID: 798 RVA: 0x0001A0C8 File Offset: 0x000190C8
		public static void RangeDecoderInitialize(Stream stream)
		{
			Coder.low = 0U;
			Coder.code = 0U;
			Coder.range = uint.MaxValue;
			for (uint num = 0U; num < 4U; num += 1U)
			{
				Coder.code = (Coder.code << 8 | (uint)((byte)stream.ReadByte()));
			}
		}

		// Token: 0x0600031F RID: 799 RVA: 0x0001A108 File Offset: 0x00019108
		public static void RangeDecoderNormalize(Stream stream)
		{
			for (;;)
			{
				if ((Coder.low ^ Coder.low + Coder.range) >= 16777216U)
				{
					if (Coder.range >= 32768U)
					{
						break;
					}
					Coder.range = ((uint)(-(uint)((ulong)Coder.low)) & 32767U);
				}
				Coder.code = (Coder.code << 8 | (uint)((byte)stream.ReadByte()));
				Coder.range <<= 8;
				Coder.low <<= 8;
			}
		}

		// Token: 0x06000320 RID: 800 RVA: 0x0001A17C File Offset: 0x0001917C
		public static bool SafeRangeDecoderNormalize(Stream stream)
		{
			uint num = Coder.low;
			uint num2 = Coder.range;
			uint num3 = 16777216U;
			uint num4 = 32768U;
			uint num5 = Coder.code;
			long position = stream.Position;
			for (;;)
			{
				if ((num ^ num + num2) >= num3)
				{
					if (num2 >= num4)
					{
						goto IL_6B;
					}
					num2 = ((uint)(-(uint)((ulong)num)) & num4 - 1U);
				}
				int num6 = stream.ReadByte();
				if (num6 == -1)
				{
					break;
				}
				num5 = (num5 << 8 | (uint)((byte)num6));
				num2 <<= 8;
				num <<= 8;
			}
			stream.Seek(position, SeekOrigin.Begin);
			return false;
			IL_6B:
			Coder.low = num;
			Coder.range = num2;
			Coder.code = num5;
			return true;
		}

		// Token: 0x06000321 RID: 801 RVA: 0x0001A208 File Offset: 0x00019208
		public static uint RangeGetCurrentCount()
		{
			return (Coder.code - Coder.low) / (Coder.range /= Coder.Scale);
		}

		// Token: 0x06000322 RID: 802 RVA: 0x0001A227 File Offset: 0x00019227
		public static uint RangeGetCurrentShiftCount(int rangeShift)
		{
			return (Coder.code - Coder.low) / (Coder.range >>= rangeShift);
		}

		// Token: 0x06000323 RID: 803 RVA: 0x0001A245 File Offset: 0x00019245
		public static void RangeRemoveSubrange()
		{
			Coder.low += Coder.range * Coder.LowCount;
			Coder.range *= Coder.HighCount - Coder.LowCount;
		}

		// Token: 0x04000227 RID: 551
		private const uint RangeTop = 16777216U;

		// Token: 0x04000228 RID: 552
		private const uint RangeBottom = 32768U;

		// Token: 0x04000229 RID: 553
		private static uint low;

		// Token: 0x0400022A RID: 554
		private static uint code;

		// Token: 0x0400022B RID: 555
		private static uint range;

		// Token: 0x0400022C RID: 556
		public static uint LowCount;

		// Token: 0x0400022D RID: 557
		public static uint HighCount;

		// Token: 0x0400022E RID: 558
		public static uint Scale;
	}
}
