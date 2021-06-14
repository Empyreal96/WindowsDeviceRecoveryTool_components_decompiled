using System;

namespace ComponentAce.Encryption
{
	// Token: 0x02000008 RID: 8
	internal class RijndaelCipher : Cipher
	{
		// Token: 0x0600003C RID: 60 RVA: 0x00002DA6 File Offset: 0x00001DA6
		public RijndaelCipher(byte[] Key) : base(Key)
		{
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002DAF File Offset: 0x00001DAF
		public override void GetContext(ref int ABufSize, ref int AKeySize, ref int AUserSize)
		{
			ABufSize = 16;
			AKeySize = 32;
			AUserSize = 480;
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002DC0 File Offset: 0x00001DC0
		protected override void Encode(byte[] Data, int Offset)
		{
			int num = 0;
			byte[] array = new byte[4];
			int num2;
			int num3;
			int num4;
			int num5;
			int num6;
			int num7;
			uint value;
			for (int i = 2; i <= this.FRounds; i++)
			{
				num2 = BitConverter.ToInt32(Data, Offset);
				num3 = BitConverter.ToInt32(base.User, num);
				num4 = (num2 ^ num3);
				num += 4;
				Offset += 4;
				num2 = BitConverter.ToInt32(Data, Offset);
				num3 = BitConverter.ToInt32(base.User, num);
				num5 = (num2 ^ num3);
				num += 4;
				Offset += 4;
				num2 = BitConverter.ToInt32(Data, Offset);
				num3 = BitConverter.ToInt32(base.User, num);
				num6 = (num2 ^ num3);
				num += 4;
				Offset += 4;
				num2 = BitConverter.ToInt32(Data, Offset);
				num3 = BitConverter.ToInt32(base.User, num);
				num7 = (num2 ^ num3);
				num += 4;
				value = (CipherUtil.Rijndael_T[0, num7 & 255] ^ CipherUtil.Rijndael_T[1, num4 >> 8 & 255] ^ CipherUtil.Rijndael_T[2, num5 >> 16 & 255] ^ CipherUtil.Rijndael_T[3, num6 >> 24 & 255]);
				array = BitConverter.GetBytes(value);
				array.CopyTo(Data, Offset);
				Offset -= 4;
				value = (CipherUtil.Rijndael_T[0, num6 & 255] ^ CipherUtil.Rijndael_T[1, num7 >> 8 & 255] ^ CipherUtil.Rijndael_T[2, num4 >> 16 & 255] ^ CipherUtil.Rijndael_T[3, num5 >> 24 & 255]);
				array = BitConverter.GetBytes(value);
				array.CopyTo(Data, Offset);
				Offset -= 4;
				value = (CipherUtil.Rijndael_T[0, num5 & 255] ^ CipherUtil.Rijndael_T[1, num6 >> 8 & 255] ^ CipherUtil.Rijndael_T[2, num7 >> 16 & 255] ^ CipherUtil.Rijndael_T[3, num4 >> 24 & 255]);
				array = BitConverter.GetBytes(value);
				array.CopyTo(Data, Offset);
				Offset -= 4;
				value = (CipherUtil.Rijndael_T[0, num4 & 255] ^ CipherUtil.Rijndael_T[1, num5 >> 8 & 255] ^ CipherUtil.Rijndael_T[2, num6 >> 16 & 255] ^ CipherUtil.Rijndael_T[3, num7 >> 24 & 255]);
				array = BitConverter.GetBytes(value);
				array.CopyTo(Data, Offset);
			}
			num2 = BitConverter.ToInt32(Data, Offset);
			num3 = BitConverter.ToInt32(base.User, num);
			num4 = (num2 ^ num3);
			num += 4;
			Offset += 4;
			num2 = BitConverter.ToInt32(Data, Offset);
			num3 = BitConverter.ToInt32(base.User, num);
			num5 = (num2 ^ num3);
			num += 4;
			Offset += 4;
			num2 = BitConverter.ToInt32(Data, Offset);
			num3 = BitConverter.ToInt32(base.User, num);
			num6 = (num2 ^ num3);
			num += 4;
			Offset += 4;
			num2 = BitConverter.ToInt32(Data, Offset);
			num3 = BitConverter.ToInt32(base.User, num);
			num7 = (num2 ^ num3);
			num += 4;
			value = (uint)((int)CipherUtil.Rijndael_S[0, num7 & 255] | (int)CipherUtil.Rijndael_S[0, num4 >> 8 & 255] << 8 | (int)CipherUtil.Rijndael_S[0, num5 >> 16 & 255] << 16 | (int)CipherUtil.Rijndael_S[0, num6 >> 24 & 255] << 24);
			array = BitConverter.GetBytes(value);
			array.CopyTo(Data, Offset);
			Offset -= 4;
			value = (uint)((int)CipherUtil.Rijndael_S[0, num6 & 255] | (int)CipherUtil.Rijndael_S[0, num7 >> 8 & 255] << 8 | (int)CipherUtil.Rijndael_S[0, num4 >> 16 & 255] << 16 | (int)CipherUtil.Rijndael_S[0, num5 >> 24 & 255] << 24);
			array = BitConverter.GetBytes(value);
			array.CopyTo(Data, Offset);
			Offset -= 4;
			value = (uint)((int)CipherUtil.Rijndael_S[0, num5 & 255] | (int)CipherUtil.Rijndael_S[0, num6 >> 8 & 255] << 8 | (int)CipherUtil.Rijndael_S[0, num7 >> 16 & 255] << 16 | (int)CipherUtil.Rijndael_S[0, num4 >> 24 & 255] << 24);
			array = BitConverter.GetBytes(value);
			array.CopyTo(Data, Offset);
			Offset -= 4;
			value = (uint)((int)CipherUtil.Rijndael_S[0, num4 & 255] | (int)CipherUtil.Rijndael_S[0, num5 >> 8 & 255] << 8 | (int)CipherUtil.Rijndael_S[0, num6 >> 16 & 255] << 16 | (int)CipherUtil.Rijndael_S[0, num7 >> 24 & 255] << 24);
			array = BitConverter.GetBytes(value);
			array.CopyTo(Data, Offset);
			for (int j = 0; j < 4; j++)
			{
				num2 = BitConverter.ToInt32(Data, Offset);
				num3 = BitConverter.ToInt32(base.User, num);
				array = BitConverter.GetBytes(num2 ^ num3);
				array.CopyTo(Data, Offset);
				num += 4;
				Offset += 4;
			}
		}

		// Token: 0x0600003F RID: 63 RVA: 0x000032F8 File Offset: 0x000022F8
		protected override void Decode(byte[] Data, int Offset)
		{
			byte[] array = new byte[4];
			int num = base.UserSize >> 1;
			num += (this.FRounds * 4 + 3) * 4;
			Offset += 12;
			int num2;
			int num3;
			int num4;
			int num5;
			int num6;
			int num7;
			uint value;
			for (int i = 1; i < this.FRounds; i++)
			{
				num2 = BitConverter.ToInt32(Data, Offset);
				num3 = BitConverter.ToInt32(base.User, num);
				num4 = (num2 ^ num3);
				num -= 4;
				Offset -= 4;
				num2 = BitConverter.ToInt32(Data, Offset);
				num3 = BitConverter.ToInt32(base.User, num);
				num5 = (num2 ^ num3);
				num -= 4;
				Offset -= 4;
				num2 = BitConverter.ToInt32(Data, Offset);
				num3 = BitConverter.ToInt32(base.User, num);
				num6 = (num2 ^ num3);
				num -= 4;
				Offset -= 4;
				num2 = BitConverter.ToInt32(Data, Offset);
				num3 = BitConverter.ToInt32(base.User, num);
				num7 = (num2 ^ num3);
				num -= 4;
				value = (CipherUtil.Rijndael_T[4, num7 & 255] ^ CipherUtil.Rijndael_T[5, num4 >> 8 & 255] ^ CipherUtil.Rijndael_T[6, num5 >> 16 & 255] ^ CipherUtil.Rijndael_T[7, num6 >> 24 & 255]);
				array = BitConverter.GetBytes(value);
				array.CopyTo(Data, Offset);
				Offset += 4;
				value = (CipherUtil.Rijndael_T[4, num6 & 255] ^ CipherUtil.Rijndael_T[5, num7 >> 8 & 255] ^ CipherUtil.Rijndael_T[6, num4 >> 16 & 255] ^ CipherUtil.Rijndael_T[7, num5 >> 24 & 255]);
				array = BitConverter.GetBytes(value);
				array.CopyTo(Data, Offset);
				Offset += 4;
				value = (CipherUtil.Rijndael_T[4, num5 & 255] ^ CipherUtil.Rijndael_T[5, num6 >> 8 & 255] ^ CipherUtil.Rijndael_T[6, num7 >> 16 & 255] ^ CipherUtil.Rijndael_T[7, num4 >> 24 & 255]);
				array = BitConverter.GetBytes(value);
				array.CopyTo(Data, Offset);
				Offset += 4;
				value = (CipherUtil.Rijndael_T[4, num4 & 255] ^ CipherUtil.Rijndael_T[5, num5 >> 8 & 255] ^ CipherUtil.Rijndael_T[6, num6 >> 16 & 255] ^ CipherUtil.Rijndael_T[7, num7 >> 24 & 255]);
				array = BitConverter.GetBytes(value);
				array.CopyTo(Data, Offset);
			}
			num2 = BitConverter.ToInt32(Data, Offset);
			num3 = BitConverter.ToInt32(base.User, num);
			num4 = (num2 ^ num3);
			num -= 4;
			Offset -= 4;
			num2 = BitConverter.ToInt32(Data, Offset);
			num3 = BitConverter.ToInt32(base.User, num);
			num5 = (num2 ^ num3);
			num -= 4;
			Offset -= 4;
			num2 = BitConverter.ToInt32(Data, Offset);
			num3 = BitConverter.ToInt32(base.User, num);
			num6 = (num2 ^ num3);
			num -= 4;
			Offset -= 4;
			num2 = BitConverter.ToInt32(Data, Offset);
			num3 = BitConverter.ToInt32(base.User, num);
			num7 = (num2 ^ num3);
			num -= 4;
			value = (uint)((int)CipherUtil.Rijndael_S[1, num7 & 255] | (int)CipherUtil.Rijndael_S[1, num4 >> 8 & 255] << 8 | (int)CipherUtil.Rijndael_S[1, num5 >> 16 & 255] << 16 | (int)CipherUtil.Rijndael_S[1, num6 >> 24 & 255] << 24);
			array = BitConverter.GetBytes(value);
			array.CopyTo(Data, Offset);
			Offset += 4;
			value = (uint)((int)CipherUtil.Rijndael_S[1, num6 & 255] | (int)CipherUtil.Rijndael_S[1, num7 >> 8 & 255] << 8 | (int)CipherUtil.Rijndael_S[1, num4 >> 16 & 255] << 16 | (int)CipherUtil.Rijndael_S[1, num5 >> 24 & 255] << 24);
			array = BitConverter.GetBytes(value);
			array.CopyTo(Data, Offset);
			Offset += 4;
			value = (uint)((int)CipherUtil.Rijndael_S[1, num5 & 255] | (int)CipherUtil.Rijndael_S[1, num6 >> 8 & 255] << 8 | (int)CipherUtil.Rijndael_S[1, num7 >> 16 & 255] << 16 | (int)CipherUtil.Rijndael_S[1, num4 >> 24 & 255] << 24);
			array = BitConverter.GetBytes(value);
			array.CopyTo(Data, Offset);
			Offset += 4;
			value = (uint)((int)CipherUtil.Rijndael_S[1, num4 & 255] | (int)CipherUtil.Rijndael_S[1, num5 >> 8 & 255] << 8 | (int)CipherUtil.Rijndael_S[1, num6 >> 16 & 255] << 16 | (int)CipherUtil.Rijndael_S[1, num7 >> 24 & 255] << 24);
			array = BitConverter.GetBytes(value);
			array.CopyTo(Data, Offset);
			for (int i = 0; i < 4; i++)
			{
				num2 = BitConverter.ToInt32(Data, Offset);
				num3 = BitConverter.ToInt32(base.User, num);
				array = BitConverter.GetBytes(num2 ^ num3);
				array.CopyTo(Data, Offset);
				Offset -= 4;
				num -= 4;
			}
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00003838 File Offset: 0x00002838
		public void Init_BuildEncodeKey_NextRounds(byte[] K, ref int R, ref int T)
		{
			int num = 0;
			while (num < this.FRounds - 6 && R <= this.FRounds)
			{
				while (num < this.FRounds - 6 && T < 4)
				{
					Array.Copy(K, num * 4, base.User, (R * 4 + T) * 4, 4);
					num++;
					T++;
				}
				if (T == 4)
				{
					T = 0;
					R++;
				}
			}
		}

		// Token: 0x06000041 RID: 65 RVA: 0x000038C0 File Offset: 0x000028C0
		public void Init_BuildEncodeKey(byte[] K)
		{
			byte[] array = new byte[]
			{
				1,
				2,
				4,
				8,
				16,
				32,
				64,
				128,
				27,
				54,
				108,
				216,
				171,
				77,
				154,
				47,
				94,
				188,
				99,
				198,
				151,
				53,
				106,
				212,
				179,
				125,
				250,
				239,
				197,
				145
			};
			byte[] array2 = new byte[4];
			int i = 0;
			int num = 0;
			int num2 = 0;
			this.Init_BuildEncodeKey_NextRounds(K, ref i, ref num);
			while (i <= this.FRounds)
			{
				int num3 = 0;
				uint num4 = BitConverter.ToUInt32(K, (this.FRounds - 7) * 4);
				K[num3] = (K[num3] ^ CipherUtil.Rijndael_S[(int)((UIntPtr)0), (int)((UIntPtr)(num4 >> 8 & 255U))] ^ array[num2]);
				num3++;
				K[num3] ^= CipherUtil.Rijndael_S[(int)((UIntPtr)0), (int)((UIntPtr)(num4 >> 16 & 255U))];
				num3++;
				K[num3] ^= CipherUtil.Rijndael_S[(int)((UIntPtr)0), (int)((UIntPtr)(num4 >> 24))];
				num3++;
				K[num3] ^= CipherUtil.Rijndael_S[(int)((UIntPtr)0), (int)((UIntPtr)(num4 & 255U))];
				num2++;
				if (this.FRounds == 14)
				{
					for (int j = 1; j <= 3; j++)
					{
						uint num5 = BitConverter.ToUInt32(K, (j - 1) * 4);
						uint num6 = BitConverter.ToUInt32(K, j * 4);
						array2 = BitConverter.GetBytes(num6 ^ num5);
						array2.CopyTo(K, j * 4);
					}
					num3 = 16;
					num4 = BitConverter.ToUInt32(K, 12);
					K[num3] ^= CipherUtil.Rijndael_S[(int)((UIntPtr)0), (int)((UIntPtr)(num4 & 255U))];
					num3++;
					K[num3] ^= CipherUtil.Rijndael_S[(int)((UIntPtr)0), (int)((UIntPtr)(num4 >> 8 & 255U))];
					num3++;
					K[num3] ^= CipherUtil.Rijndael_S[(int)((UIntPtr)0), (int)((UIntPtr)(num4 >> 16 & 255U))];
					num3++;
					K[num3] ^= CipherUtil.Rijndael_S[(int)((UIntPtr)0), (int)((UIntPtr)(num4 >> 24))];
					for (int j = 5; j <= 7; j++)
					{
						uint num5 = BitConverter.ToUInt32(K, (j - 1) * 4);
						uint num6 = BitConverter.ToUInt32(K, j * 4);
						array2 = BitConverter.GetBytes(num6 ^ num5);
						array2.CopyTo(K, j * 4);
					}
				}
				else
				{
					for (int j = 1; j <= this.FRounds - 7; j++)
					{
						uint num5 = BitConverter.ToUInt32(K, (j - 1) * 4);
						uint num6 = BitConverter.ToUInt32(K, j * 4);
						array2 = BitConverter.GetBytes(num6 ^ num5);
						array2.CopyTo(K, j * 4);
					}
				}
				this.Init_BuildEncodeKey_NextRounds(K, ref i, ref num);
			}
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00003B3C File Offset: 0x00002B3C
		public void Init_BuildDecodeKey()
		{
			int num = base.UserSize >> 1;
			Array.Copy(base.User, 0, base.User, num, num);
			num += 16;
			for (int i = 0; i <= this.FRounds * 4 - 5; i++)
			{
				uint num2 = BitConverter.ToUInt32(base.User, num);
				uint value = CipherUtil.Rijndael_Key[(int)((UIntPtr)(num2 & 255U))] ^ (uint)DecUtil.ROL(CipherUtil.Rijndael_Key[(int)((UIntPtr)(num2 >> 8 & 255U))], 8) ^ (uint)DecUtil.ROL(CipherUtil.Rijndael_Key[(int)((UIntPtr)(num2 >> 16 & 255U))], 16) ^ (uint)DecUtil.ROL(CipherUtil.Rijndael_Key[(int)((UIntPtr)(num2 >> 24))], 24);
				BitConverter.GetBytes(value).CopyTo(base.User, num);
				num += 4;
			}
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00003BF8 File Offset: 0x00002BF8
		public override void Init(byte[] Key, int Size, byte[] IVector)
		{
			byte[] array = new byte[32];
			base.InitBegin(Size);
			if (Size <= 16)
			{
				this.FRounds = 10;
			}
			else if (Size <= 24)
			{
				this.FRounds = 12;
			}
			else
			{
				this.FRounds = 14;
			}
			Key.CopyTo(array, 0);
			this.Init_BuildEncodeKey(array);
			this.Init_BuildDecodeKey();
			array.Initialize();
			this.InitEnd(IVector);
		}

		// Token: 0x0400001F RID: 31
		private int FRounds;
	}
}
