using System;
using System.Collections;

namespace ComponentAce.Encryption
{
	// Token: 0x02000006 RID: 6
	internal class THash : TProtection
	{
		// Token: 0x06000011 RID: 17 RVA: 0x00002481 File Offset: 0x00001481
		public virtual void Init()
		{
			this.Protect(true);
		}

		// Token: 0x06000012 RID: 18 RVA: 0x0000248A File Offset: 0x0000148A
		public virtual void Calc(byte[] Data, int Offset, int DataSize)
		{
		}

		// Token: 0x06000013 RID: 19 RVA: 0x0000248C File Offset: 0x0000148C
		protected void Protect(bool IsInit)
		{
		}

		// Token: 0x06000014 RID: 20 RVA: 0x0000248E File Offset: 0x0000148E
		public virtual void Done()
		{
			this.Protect(false);
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002497 File Offset: 0x00001497
		public virtual byte[] DigestKey()
		{
			return DecUtil.GetTestVector();
		}

		// Token: 0x06000016 RID: 22 RVA: 0x000024A0 File Offset: 0x000014A0
		public virtual int DigestKeySize()
		{
			return 0;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000024B0 File Offset: 0x000014B0
		public virtual byte[] TestVector()
		{
			return DecUtil.GetTestVector();
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000024B7 File Offset: 0x000014B7
		public static string CalcBuffer(byte[] Buffer, int BufferSize, TProtection Protection, int Format)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000019 RID: 25 RVA: 0x000024BE File Offset: 0x000014BE
		public static bool SelfTest()
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600001A RID: 26 RVA: 0x000024C5 File Offset: 0x000014C5
		protected override void CodeInit(TPAction Action)
		{
			this.Init();
			if (Action == TPAction.paWipe)
			{
				throw new NotImplementedException();
			}
			base.CodeInit(Action);
		}

		// Token: 0x0600001B RID: 27 RVA: 0x000024DE File Offset: 0x000014DE
		protected override void CodeDone(TPAction Action)
		{
			base.CodeDone(Action);
			if (Action != TPAction.paCalc)
			{
				this.Init();
				return;
			}
			this.Done();
		}

		// Token: 0x0600001C RID: 28 RVA: 0x000024F8 File Offset: 0x000014F8
		protected override void CodeBuf(byte[] Buffer, int BufferSize, TPAction Action)
		{
			if (Action != TPAction.paDecode)
			{
				base.CodeBuf(Buffer, BufferSize, Action);
			}
			if (new ArrayList(base.Actions).Contains(Action))
			{
				int num = 0;
				int num2 = this.DigestKeySize();
				int i = BufferSize;
				if (Action == TPAction.paCalc)
				{
					this.Calc(Buffer, 0, BufferSize);
				}
				else if (new ArrayList(new object[]
				{
					TPAction.paScramble,
					TPAction.paWipe
				}).Contains(Action))
				{
					while (i > 0)
					{
						int num3 = i;
						if (num3 > num2)
						{
							num3 = num2;
						}
						this.Calc(Buffer, num, num3);
						this.Done();
						i -= num3;
						num += num3;
					}
				}
				else
				{
					while (i > 0)
					{
						int num3 = num2;
						if (num3 > i)
						{
							num3 = i;
						}
						this.Calc(this.DigestKey(), 0, num2);
						this.Done();
						i -= num3;
						num += num3;
					}
				}
			}
			if (Action == TPAction.paDecode)
			{
				base.CodeBuf(Buffer, BufferSize, Action);
			}
		}
	}
}
