using System;

namespace ComponentAce.Encryption
{
	// Token: 0x02000009 RID: 9
	internal class TCipher_Blowfish : Cipher
	{
		// Token: 0x06000044 RID: 68 RVA: 0x00003C5C File Offset: 0x00002C5C
		public TCipher_Blowfish(byte[] Key) : base(Key)
		{
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00003C65 File Offset: 0x00002C65
		public override void GetContext(ref int ABufSize, ref int AKeySize, ref int AUserSize)
		{
			ABufSize = 8;
			AKeySize = 56;
			AUserSize = CipherUtil.Blowfish_Key.Length * 4 + CipherUtil.Blowfish_Data.Length * 4;
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00003C88 File Offset: 0x00002C88
		protected override void Encode(byte[] Data, int Offset)
		{
			int num = CipherUtil.Blowfish_Data.Length * 4;
			uint num2 = DecUtil.SwapInteger(BitConverter.ToUInt32(Data, Offset)) ^ BitConverter.ToUInt32(base.User, num);
			num += 4;
			uint num3 = DecUtil.SwapInteger(BitConverter.ToUInt32(Data, Offset + 4));
			for (uint num4 = 0U; num4 <= 7U; num4 += 1U)
			{
				uint num5 = BitConverter.ToUInt32(base.User, (int)((byte)(num2 >> 24) * 4));
				uint num6 = BitConverter.ToUInt32(base.User, (256 + (int)((byte)(num2 >> 16 & 255U))) * 4);
				uint num7 = BitConverter.ToUInt32(base.User, (512 + (int)((byte)(num2 >> 8 & 255U))) * 4);
				uint num8 = BitConverter.ToUInt32(base.User, (768 + (int)((byte)(num2 & 255U))) * 4);
				num3 = (num3 ^ BitConverter.ToUInt32(base.User, num) ^ (num5 + num6 ^ num7) + num8);
				num5 = BitConverter.ToUInt32(base.User, (int)((byte)(num3 >> 24) * 4));
				num6 = BitConverter.ToUInt32(base.User, (256 + (int)((byte)(num3 >> 16 & 255U))) * 4);
				num7 = BitConverter.ToUInt32(base.User, (512 + (int)((byte)(num3 >> 8 & 255U))) * 4);
				num8 = BitConverter.ToUInt32(base.User, (768 + (int)((byte)(num3 & 255U))) * 4);
				num2 = (num2 ^ BitConverter.ToUInt32(base.User, num + 4) ^ (num5 + num6 ^ num7) + num8);
				num += 8;
			}
			BitConverter.GetBytes(DecUtil.SwapInteger(num3 ^ BitConverter.ToUInt32(base.User, num))).CopyTo(Data, Offset);
			BitConverter.GetBytes(DecUtil.SwapInteger(num2)).CopyTo(Data, Offset + 4);
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00003E30 File Offset: 0x00002E30
		protected override void Decode(byte[] Data, int Offset)
		{
			int num = CipherUtil.Blowfish_Data.Length * 4 + CipherUtil.Blowfish_Key.Length * 4 - 4;
			uint num2 = BitConverter.ToUInt32(Data, Offset);
			uint num3 = BitConverter.ToUInt32(base.User, num);
			uint num4 = DecUtil.SwapInteger(num2) ^ num3;
			num2 = BitConverter.ToUInt32(Data, Offset + 4);
			uint num5 = DecUtil.SwapInteger(num2);
			for (int i = 0; i <= 7; i++)
			{
				num -= 8;
				num2 = BitConverter.ToUInt32(base.User, (int)((byte)(num4 >> 24) * 4));
				num3 = BitConverter.ToUInt32(base.User, (256 + (int)((byte)(num4 >> 16 & 255U))) * 4);
				uint num6 = BitConverter.ToUInt32(base.User, (512 + (int)((byte)(num4 >> 8 & 255U))) * 4);
				uint num7 = BitConverter.ToUInt32(base.User, (768 + (int)((byte)(num4 & 255U))) * 4);
				num5 = (num5 ^ BitConverter.ToUInt32(base.User, num + 4) ^ (num2 + num3 ^ num6) + num7);
				num2 = BitConverter.ToUInt32(base.User, (int)((byte)(num5 >> 24) * 4));
				num3 = BitConverter.ToUInt32(base.User, (256 + (int)((byte)(num5 >> 16 & 255U))) * 4);
				num6 = BitConverter.ToUInt32(base.User, (512 + (int)((byte)(num5 >> 8 & 255U))) * 4);
				num7 = BitConverter.ToUInt32(base.User, (768 + (int)((byte)(num5 & 255U))) * 4);
				num4 = (num4 ^ BitConverter.ToUInt32(base.User, num) ^ (num2 + num3 ^ num6) + num7);
			}
			num -= 4;
			num2 = DecUtil.SwapInteger(num5 ^ BitConverter.ToUInt32(base.User, num));
			BitConverter.GetBytes(num2).CopyTo(Data, Offset);
			num3 = DecUtil.SwapInteger(num4);
			BitConverter.GetBytes(num3).CopyTo(Data, Offset + 4);
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00003FF8 File Offset: 0x00002FF8
		public override void Init(byte[] Key, int Size, byte[] IVector)
		{
			byte[] array = new byte[8];
			base.InitBegin(Size);
			byte[] array2 = DecUtil.Copy2DArrayToFlatArray(CipherUtil.Blowfish_Data);
			int length = CipherUtil.Blowfish_Data.Length;
			array2.CopyTo(base.User, 0);
			DecUtil.IntArrayToByteArray(CipherUtil.Blowfish_Key).CopyTo(base.User, length * 4);
			int i = 0;
			for (int j = 0; j <= 17; j++)
			{
				int num = ((int)Key[i % Size] << 24) + ((int)Key[(i + 1) % Size] << 16) + ((int)Key[(i + 2) % Size] << 8) + (int)Key[(i + 3) % Size];
				int value = BitConverter.ToInt32(base.User, length * 4 + j * 4) ^ num;
				BitConverter.GetBytes(value).CopyTo(base.User, length * 4 + j * 4);
				i = (i + 4) % Size;
			}
			for (int j = 0; j <= 8; j++)
			{
				this.Encode(array, 0);
				int value2 = DecUtil.SwapInteger(BitConverter.ToInt32(array, 0));
				BitConverter.GetBytes(value2).CopyTo(base.User, (length + j * 2) * 4);
				value2 = DecUtil.SwapInteger(BitConverter.ToInt32(array, 4));
				BitConverter.GetBytes(value2).CopyTo(base.User, (length + j * 2 + 1) * 4);
			}
			for (int j = 0; j <= 3; j++)
			{
				for (i = 0; i <= 127; i++)
				{
					this.Encode(array, 0);
					int value2 = DecUtil.SwapInteger(BitConverter.ToInt32(array, 0));
					BitConverter.GetBytes(value2).CopyTo(base.User, (j * 256 + i * 2) * 4);
					value2 = DecUtil.SwapInteger(BitConverter.ToInt32(array, 4));
					BitConverter.GetBytes(value2).CopyTo(base.User, (j * 256 + i * 2 + 1) * 4);
				}
			}
			array.Initialize();
			this.InitEnd(IVector);
		}
	}
}
