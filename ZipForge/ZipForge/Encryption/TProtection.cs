using System;

namespace ComponentAce.Encryption
{
	// Token: 0x02000005 RID: 5
	internal class TProtection
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000005 RID: 5 RVA: 0x0000235D File Offset: 0x0000135D
		// (set) Token: 0x06000006 RID: 6 RVA: 0x00002365 File Offset: 0x00001365
		public TProtection Protection
		{
			get
			{
				return this.FProtection;
			}
			set
			{
				this.FProtection = value;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000007 RID: 7 RVA: 0x0000236E File Offset: 0x0000136E
		// (set) Token: 0x06000008 RID: 8 RVA: 0x00002376 File Offset: 0x00001376
		public TPAction[] Actions
		{
			get
			{
				return this.FActions;
			}
			set
			{
				this.FActions = value;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000009 RID: 9 RVA: 0x0000237F File Offset: 0x0000137F
		public int RefCount
		{
			get
			{
				return this.FRefCount;
			}
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002388 File Offset: 0x00001388
		public bool SetProtection_CheckProtection(TProtection P)
		{
			return P != this && this.SetProtection_CheckProtection(P.FProtection);
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000023AD File Offset: 0x000013AD
		protected virtual void CodeInit(TPAction Action)
		{
			if (this.Protection != null)
			{
				this.Protection.CodeInit(Action);
			}
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000023C3 File Offset: 0x000013C3
		protected virtual void CodeDone(TPAction Action)
		{
			if (this.Protection != null)
			{
				this.Protection.CodeDone(Action);
			}
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000023D9 File Offset: 0x000013D9
		protected virtual void CodeBuf(byte[] Buffer, int BufferSize, TPAction Action)
		{
			if (this.Protection != null)
			{
				this.Protection.CodeBuf(Buffer, BufferSize, Action);
			}
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000023F4 File Offset: 0x000013F4
		public virtual int CodeBuffer(ref byte[] Buffer, int BufferSize, TPAction Action)
		{
			this.CodeInit(Action);
			try
			{
				this.CodeBuf(Buffer, BufferSize, Action);
			}
			finally
			{
				this.CodeDone(Action);
			}
			return BufferSize;
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002430 File Offset: 0x00001430
		~TProtection()
		{
			this.Protection = null;
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002460 File Offset: 0x00001460
		public TProtection()
		{
			TPAction[] factions = new TPAction[1];
			this.FActions = factions;
			base..ctor();
		}

		// Token: 0x04000010 RID: 16
		private int FRefCount;

		// Token: 0x04000011 RID: 17
		private TProtection FProtection;

		// Token: 0x04000012 RID: 18
		private TPAction[] FActions;
	}
}
