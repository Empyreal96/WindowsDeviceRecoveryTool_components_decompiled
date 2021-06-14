using System;
using System.Security.Cryptography;
using System.Text;

namespace ComponentAce.Compression.ZipForge.Encryption
{
	// Token: 0x0200008A RID: 138
	internal class PBKDF2
	{
		// Token: 0x0600066F RID: 1647 RVA: 0x00029C6C File Offset: 0x00028C6C
		public PBKDF2(string password, int saltSize, int iterations)
		{
			if (saltSize < 0)
			{
				throw new ArgumentOutOfRangeException("saltSize");
			}
			this.rndGenerator = new RNGCryptoServiceProvider();
			byte[] array = new byte[saltSize];
			this.rndGenerator.GetBytes(array);
			this.Salt = array;
			this.IterationCount = iterations;
			this.hmacSHA1 = new HMACSHA1(new UTF8Encoding(false).GetBytes(password));
			this.Initialize();
		}

		// Token: 0x06000670 RID: 1648 RVA: 0x00029CD7 File Offset: 0x00028CD7
		public PBKDF2(string password, byte[] salt, int iterations)
		{
			this.Salt = salt;
			this.IterationCount = iterations;
			this.hmacSHA1 = new HMACSHA1(new UTF8Encoding(false).GetBytes(password));
			this.Initialize();
		}

		// Token: 0x06000671 RID: 1649 RVA: 0x00029D0C File Offset: 0x00028D0C
		private byte[] Int(uint i)
		{
			byte[] bytes = BitConverter.GetBytes(i);
			byte[] result = new byte[]
			{
				bytes[3],
				bytes[2],
				bytes[1],
				bytes[0]
			};
			if (!BitConverter.IsLittleEndian)
			{
				return bytes;
			}
			return result;
		}

		// Token: 0x06000672 RID: 1650 RVA: 0x00029D4C File Offset: 0x00028D4C
		public byte[] GetKeyBytes(int cb)
		{
			if (cb <= 0)
			{
				throw new ArgumentOutOfRangeException("cb");
			}
			byte[] array = new byte[cb];
			int i = 0;
			int num = this.endIndex - this.startIndex;
			if (num > 0)
			{
				if (cb < num)
				{
					Buffer.BlockCopy(this.buffer, this.startIndex, array, 0, cb);
					this.startIndex += cb;
					return array;
				}
				Buffer.BlockCopy(this.buffer, this.startIndex, array, 0, num);
				this.startIndex = (this.endIndex = 0);
				i += num;
			}
			while (i < cb)
			{
				byte[] array2 = this.Int(this.block);
				this.hmacSHA1.TransformBlock(this.saltValue, 0, this.saltValue.Length, this.saltValue, 0);
				this.hmacSHA1.TransformFinalBlock(array2, 0, array2.Length);
				byte[] array3 = this.hmacSHA1.Hash;
				this.hmacSHA1.Initialize();
				byte[] array4 = array3;
				for (int j = 2; j <= this.iterationNumber; j++)
				{
					array3 = this.hmacSHA1.ComputeHash(array3);
					for (int k = 0; k < 20; k++)
					{
						array4[k] ^= array3[k];
					}
				}
				this.block += 1U;
				byte[] src = array4;
				int num2 = cb - i;
				if (num2 <= 20)
				{
					Buffer.BlockCopy(src, 0, array, i, num2);
					i += num2;
					Buffer.BlockCopy(src, num2, this.buffer, this.startIndex, 20 - num2);
					this.endIndex += 20 - num2;
					return array;
				}
				Buffer.BlockCopy(src, 0, array, i, 20);
				i += 20;
			}
			return array;
		}

		// Token: 0x06000673 RID: 1651 RVA: 0x00029EF8 File Offset: 0x00028EF8
		private void Initialize()
		{
			if (this.buffer != null)
			{
				Array.Clear(this.buffer, 0, this.buffer.Length);
			}
			this.buffer = new byte[20];
			this.block = 1U;
			this.startIndex = (this.endIndex = 0);
		}

		// Token: 0x06000674 RID: 1652 RVA: 0x00029F45 File Offset: 0x00028F45
		public void Reset()
		{
			this.Initialize();
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x06000675 RID: 1653 RVA: 0x00029F4D File Offset: 0x00028F4D
		// (set) Token: 0x06000676 RID: 1654 RVA: 0x00029F55 File Offset: 0x00028F55
		public int IterationCount
		{
			get
			{
				return this.iterationNumber;
			}
			set
			{
				if (value <= 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.iterationNumber = value;
				this.Initialize();
			}
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x06000677 RID: 1655 RVA: 0x00029F73 File Offset: 0x00028F73
		// (set) Token: 0x06000678 RID: 1656 RVA: 0x00029F85 File Offset: 0x00028F85
		public byte[] Salt
		{
			get
			{
				return (byte[])this.saltValue.Clone();
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.saltValue = (byte[])value.Clone();
				this.Initialize();
			}
		}

		// Token: 0x04000367 RID: 871
		private const int BlockSize = 20;

		// Token: 0x04000368 RID: 872
		private HMACSHA1 hmacSHA1;

		// Token: 0x04000369 RID: 873
		private int iterationNumber;

		// Token: 0x0400036A RID: 874
		private byte[] saltValue;

		// Token: 0x0400036B RID: 875
		private uint block;

		// Token: 0x0400036C RID: 876
		private byte[] buffer;

		// Token: 0x0400036D RID: 877
		private int endIndex;

		// Token: 0x0400036E RID: 878
		private int startIndex;

		// Token: 0x0400036F RID: 879
		private RNGCryptoServiceProvider rndGenerator;
	}
}
