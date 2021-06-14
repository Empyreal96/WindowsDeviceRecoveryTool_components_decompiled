using System;
using System.Collections;

namespace ComponentAce.Encryption
{
	// Token: 0x02000007 RID: 7
	internal class Cipher : TProtection
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600001E RID: 30 RVA: 0x000025DB File Offset: 0x000015DB
		protected byte[] User
		{
			get
			{
				return this.FUser;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600001F RID: 31 RVA: 0x000025E3 File Offset: 0x000015E3
		protected byte[] Buffer
		{
			get
			{
				return this.FBuffer;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000020 RID: 32 RVA: 0x000025EB File Offset: 0x000015EB
		protected int UserSize
		{
			get
			{
				return this.FUserSize;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000021 RID: 33 RVA: 0x000025F3 File Offset: 0x000015F3
		// (set) Token: 0x06000022 RID: 34 RVA: 0x000025FB File Offset: 0x000015FB
		public CipherBlockMode Mode
		{
			get
			{
				return this.FMode;
			}
			set
			{
				this.FMode = value;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000023 RID: 35 RVA: 0x00002604 File Offset: 0x00001604
		public int KeySize
		{
			get
			{
				return this.FKeySize;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000024 RID: 36 RVA: 0x0000260C File Offset: 0x0000160C
		public int BufSize
		{
			get
			{
				return this.FBufSize;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000025 RID: 37 RVA: 0x00002614 File Offset: 0x00001614
		// (set) Token: 0x06000026 RID: 38 RVA: 0x0000261C File Offset: 0x0000161C
		public bool Initialized
		{
			get
			{
				return this.FInitialized;
			}
			set
			{
				this.FInitialized = value;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000027 RID: 39 RVA: 0x00002625 File Offset: 0x00001625
		public byte[] Vector
		{
			get
			{
				return this.FVector;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000028 RID: 40 RVA: 0x0000262D File Offset: 0x0000162D
		public byte[] Feedback
		{
			get
			{
				return this.FFeedback;
			}
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002635 File Offset: 0x00001635
		protected void InitBegin(int Size)
		{
			this.FInitialized = false;
			this.Protect();
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002644 File Offset: 0x00001644
		protected virtual void InitEnd(byte[] IVector)
		{
			if (IVector == null)
			{
				this.Encode(this.Vector, 0);
			}
			else
			{
				IVector.CopyTo(this.Vector, 0);
			}
			this.Vector.CopyTo(this.Feedback, 0);
			this.Initialized = true;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x0000267E File Offset: 0x0000167E
		public virtual void GetContext(ref int ABufSize, ref int AKeySize, ref int AUserSize)
		{
			ABufSize = 0;
			AKeySize = 0;
			AUserSize = 0;
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002689 File Offset: 0x00001689
		public virtual byte[] TestVector()
		{
			return DecUtil.GetTestVector();
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002690 File Offset: 0x00001690
		protected virtual void Encode(byte[] Data, int Offset)
		{
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002692 File Offset: 0x00001692
		protected virtual void Decode(byte[] Data, int Offset)
		{
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002694 File Offset: 0x00001694
		public Cipher(byte[] key)
		{
			this.GetContext(ref this.FBufSize, ref this.FKeySize, ref this.FUserSize);
			this.FVector = new byte[this.FBufSize];
			this.FFeedback = new byte[this.FBufSize];
			this.FBuffer = new byte[this.FBufSize];
			this.FUser = new byte[this.FUserSize];
			this.Protect();
			if (key != null)
			{
				this.InitKey(key, null);
			}
			this.ctrMode_EncryptionBlock = new byte[this.FBufSize];
			this.ctrMode_Nonce = new byte[this.FBufSize];
			this.ctrMode_Position = this.FBufSize;
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002744 File Offset: 0x00001744
		~Cipher()
		{
			this.Protect();
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002770 File Offset: 0x00001770
		public int MaxKeySize()
		{
			int result = 0;
			int num = 0;
			this.GetContext(ref num, ref result, ref num);
			return result;
		}

		// Token: 0x06000032 RID: 50 RVA: 0x0000278E File Offset: 0x0000178E
		public static bool SelfTest()
		{
			return true;
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00002791 File Offset: 0x00001791
		public virtual void Init(byte[] Key, int Size, byte[] IVector)
		{
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002794 File Offset: 0x00001794
		public void InitKey(byte[] Key, byte[] IVector)
		{
			int num = Key.Length;
			if (num > this.FKeySize)
			{
				num = this.FKeySize;
			}
			this.Init(Key, num, IVector);
			this.EncodeBuffer(Key, Key, Key.Length);
			this.Done();
			this.FInitialized = true;
		}

		// Token: 0x06000035 RID: 53 RVA: 0x000027D6 File Offset: 0x000017D6
		public virtual void Done()
		{
			this.FFeedback.CopyTo(this.FBuffer, 0);
			this.FVector.CopyTo(this.FFeedback, 0);
		}

		// Token: 0x06000036 RID: 54 RVA: 0x000027FC File Offset: 0x000017FC
		public virtual void Protect()
		{
			this.FInitialized = false;
			for (int i = 0; i < this.FBufSize; i++)
			{
				this.FVector[i] = byte.MaxValue;
			}
			for (int j = 0; j < this.FBufSize; j++)
			{
				this.FFeedback[j] = byte.MaxValue;
			}
		}

		// Token: 0x06000037 RID: 55 RVA: 0x0000284C File Offset: 0x0000184C
		public void EncodeBuffer(byte[] Source, byte[] Dest, int DataSize)
		{
			int num = 0;
			int num2 = 0;
			byte[] array = new byte[this.FBufSize];
			if (!this.Initialized)
			{
				throw new NotImplementedException();
			}
			switch (this.FMode)
			{
			case CipherBlockMode.CTS:
				while (DataSize >= this.FBufSize)
				{
					DecUtil.XORBuffers(Source, num, this.FFeedback, 0, this.FBufSize, Dest, num2);
					this.Encode(Dest, num2);
					DecUtil.XORBuffers(Dest, num2, this.FFeedback, 0, this.FBufSize, this.FFeedback, 0);
					num += this.FBufSize;
					num2 += this.FBufSize;
					DataSize -= this.FBufSize;
				}
				if (DataSize > 0)
				{
					this.FFeedback.CopyTo(this.FBuffer, 0);
					this.Encode(this.FBuffer, 0);
					DecUtil.XORBuffers(Source, num, this.FBuffer, 0, DataSize, Dest, num2);
					DecUtil.XORBuffers(this.FBuffer, 0, this.FFeedback, 0, this.FBufSize, this.FFeedback, 0);
					return;
				}
				break;
			case CipherBlockMode.CTR:
				Source.CopyTo(Dest, num);
				for (int i = 0; i < DataSize; i++)
				{
					if (this.ctrMode_Position == this.FBufSize)
					{
						for (int j = 0; j < 8; j++)
						{
							if (this.ctrMode_Nonce[j] < 255)
							{
								this.ctrMode_Nonce[j] = this.ctrMode_Nonce[j] + 1;
								break;
							}
							this.ctrMode_Nonce[j] = 0;
						}
						this.ctrMode_Nonce.CopyTo(this.ctrMode_EncryptionBlock, 0);
						this.Encode(this.ctrMode_EncryptionBlock, 0);
						this.ctrMode_Position = 0;
					}
					int num3 = this.ctrMode_Position;
					this.ctrMode_Position++;
					Dest[i] ^= this.ctrMode_EncryptionBlock[num3];
				}
				break;
			case CipherBlockMode.CBC:
			case CipherBlockMode.CFB:
			case CipherBlockMode.OFB:
			case CipherBlockMode.CTSMAC:
			case CipherBlockMode.CBCMAC:
			case CipherBlockMode.CFBMAC:
				break;
			case CipherBlockMode.ECB:
				if (Source != Dest)
				{
					Source.CopyTo(Dest, num);
				}
				while (DataSize >= this.FBufSize)
				{
					Array.Copy(Dest, num, array, 0, this.FBufSize);
					this.Encode(array, 0);
					Array.Copy(array, 0, Dest, num, this.FBufSize);
					num += this.FBufSize;
					DataSize -= this.FBufSize;
				}
				if (DataSize > 0)
				{
					array = new byte[this.Buffer.Length];
					Dest.CopyTo(array, num);
					this.Encode(array, 0);
					Array.Copy(array, 0, Dest, num, DataSize);
					return;
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002A98 File Offset: 0x00001A98
		public void DecodeBuffer(byte[] Source, byte[] Dest, int DataSize)
		{
			int num = 0;
			byte[] array = new byte[this.Buffer.Length];
			byte[] array2 = new byte[this.Buffer.Length];
			int num2 = this.Buffer.Length;
			if (!this.Initialized)
			{
				throw new Exception("not initialized");
			}
			switch (this.FMode)
			{
			case CipherBlockMode.CTS:
				Source.CopyTo(Dest, 0);
				this.FFeedback.CopyTo(array, 0);
				this.Buffer.CopyTo(array2, 0);
				while (DataSize >= this.FBufSize)
				{
					DecUtil.XORBuffers(Dest, num, array, 0, this.FBufSize, this.FBuffer, 0);
					this.Decode(Dest, num);
					DecUtil.XORBuffers(Dest, num, array, 0, this.FBufSize, Dest, num);
					array.CopyTo(array2, 0);
					this.FBuffer.CopyTo(array, 0);
					num += this.FBufSize;
					DataSize -= this.FBufSize;
				}
				array.CopyTo(this.FFeedback, 0);
				if (DataSize > 0)
				{
					this.FFeedback.CopyTo(this.FBuffer, 0);
					this.Encode(this.FBuffer, 0);
					DecUtil.XORBuffers(this.FBuffer, 0, Dest, num, DataSize, Dest, num);
					DecUtil.XORBuffers(this.FBuffer, 0, this.FFeedback, 0, this.FBufSize, this.FFeedback, 0);
					return;
				}
				break;
			case CipherBlockMode.CTR:
				Source.CopyTo(Dest, num);
				for (int i = 0; i < DataSize; i++)
				{
					if (this.ctrMode_Position == this.FBufSize)
					{
						for (int j = 0; j < 8; j++)
						{
							if (this.ctrMode_Nonce[j] < 255)
							{
								this.ctrMode_Nonce[j] = this.ctrMode_Nonce[j] + 1;
								break;
							}
							this.ctrMode_Nonce[j] = 0;
						}
						this.ctrMode_Nonce.CopyTo(this.ctrMode_EncryptionBlock, 0);
						this.Encode(this.ctrMode_EncryptionBlock, 0);
						this.ctrMode_Position = 0;
					}
					int num3 = this.ctrMode_Position;
					this.ctrMode_Position++;
					Dest[i] ^= this.ctrMode_EncryptionBlock[num3];
				}
				return;
			case CipherBlockMode.CBC:
			case CipherBlockMode.CFB:
			case CipherBlockMode.OFB:
				break;
			case CipherBlockMode.ECB:
				if (Source != Dest)
				{
					Source.CopyTo(Dest, num);
				}
				while (DataSize >= this.FBufSize)
				{
					Array.Copy(Dest, num, array, 0, this.FBufSize);
					this.Decode(array, 0);
					Array.Copy(array, 0, Dest, num, this.FBufSize);
					num += this.FBufSize;
					DataSize -= this.FBufSize;
				}
				if (DataSize > 0)
				{
					array = new byte[this.Buffer.Length];
					Dest.CopyTo(array, num);
					this.Decode(array, 0);
					Array.Copy(array, 0, Dest, num, DataSize);
					return;
				}
				break;
			case CipherBlockMode.CTSMAC:
			case CipherBlockMode.CBCMAC:
			case CipherBlockMode.CFBMAC:
				this.EncodeBuffer(Source, Dest, DataSize);
				break;
			default:
				return;
			}
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002D3C File Offset: 0x00001D3C
		protected override void CodeInit(TPAction Action)
		{
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002D3E File Offset: 0x00001D3E
		protected override void CodeDone(TPAction Action)
		{
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00002D40 File Offset: 0x00001D40
		protected override void CodeBuf(byte[] Buffer, int BufferSize, TPAction Action)
		{
			if (Action == TPAction.paDecode)
			{
				if (new ArrayList(base.Actions).Contains(Action))
				{
					this.DecodeBuffer(Buffer, Buffer, BufferSize);
				}
				base.CodeBuf(Buffer, BufferSize, Action);
				return;
			}
			base.CodeBuf(Buffer, BufferSize, Action);
			if (new ArrayList(base.Actions).Contains(Action))
			{
				this.EncodeBuffer(Buffer, Buffer, BufferSize);
			}
		}

		// Token: 0x04000013 RID: 19
		private CipherBlockMode FMode;

		// Token: 0x04000014 RID: 20
		private int FKeySize;

		// Token: 0x04000015 RID: 21
		private int FBufSize;

		// Token: 0x04000016 RID: 22
		private int FUserSize;

		// Token: 0x04000017 RID: 23
		private byte[] FBuffer;

		// Token: 0x04000018 RID: 24
		private byte[] FVector;

		// Token: 0x04000019 RID: 25
		private byte[] FFeedback;

		// Token: 0x0400001A RID: 26
		private byte[] FUser;

		// Token: 0x0400001B RID: 27
		private bool FInitialized;

		// Token: 0x0400001C RID: 28
		private int ctrMode_Position;

		// Token: 0x0400001D RID: 29
		private byte[] ctrMode_EncryptionBlock;

		// Token: 0x0400001E RID: 30
		private byte[] ctrMode_Nonce;
	}
}
