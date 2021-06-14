using System;
using System.IO;

namespace Nokia.Mira.Cryptography
{
	// Token: 0x0200000C RID: 12
	internal sealed class CRC32
	{
		// Token: 0x06000024 RID: 36 RVA: 0x000025D3 File Offset: 0x000007D3
		static CRC32()
		{
			CRC32.Initialize();
		}

		// Token: 0x06000025 RID: 37 RVA: 0x000025EC File Offset: 0x000007EC
		public byte[] ComputeHash(string filePath)
		{
			if (string.IsNullOrEmpty(filePath))
			{
				throw new ArgumentException("filePath");
			}
			byte[] array = new byte[65536];
			byte[] result;
			using (Stream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				for (;;)
				{
					int num = stream.Read(array, 0, 65536);
					if (num == 0)
					{
						break;
					}
					this.HashCore(array, 0, num, array, 0);
				}
				result = this.HashFinal();
			}
			return result;
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002664 File Offset: 0x00000864
		private static void Initialize()
		{
			for (uint num = 0U; num < 256U; num += 1U)
			{
				uint num2 = num;
				for (int i = 8; i > 0; i--)
				{
					if ((num2 & 1U) == 1U)
					{
						num2 = (num2 >> 1 ^ 3988292384U);
					}
					else
					{
						num2 >>= 1;
					}
				}
				CRC32.Crc32Table[(int)((UIntPtr)num)] = num2;
			}
		}

		// Token: 0x06000027 RID: 39 RVA: 0x000026B0 File Offset: 0x000008B0
		private void HashCore(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
		{
			int num = inputOffset + inputCount;
			for (int i = inputOffset; i < num; i++)
			{
				this.crc32Result = (this.crc32Result >> 8 ^ CRC32.Crc32Table[(int)((UIntPtr)((uint)inputBuffer[i] ^ (this.crc32Result & 255U)))]);
			}
			if (outputBuffer != null && (inputBuffer != outputBuffer || inputOffset != outputOffset))
			{
				Buffer.BlockCopy(inputBuffer, inputOffset, outputBuffer, outputOffset, inputCount);
			}
		}

		// Token: 0x06000028 RID: 40 RVA: 0x0000270D File Offset: 0x0000090D
		private byte[] HashFinal()
		{
			this.crc32Result = ~this.crc32Result;
			return BitConverter.GetBytes(this.crc32Result);
		}

		// Token: 0x04000011 RID: 17
		private const uint Polynomial = 3988292384U;

		// Token: 0x04000012 RID: 18
		private const int BufferSize = 65536;

		// Token: 0x04000013 RID: 19
		private static readonly uint[] Crc32Table = new uint[256];

		// Token: 0x04000014 RID: 20
		private uint crc32Result = uint.MaxValue;
	}
}
