using System;

namespace ComponentAce.Compression.Libs.PPMd
{
	// Token: 0x02000056 RID: 86
	internal class See2Context
	{
		// Token: 0x060003A1 RID: 929 RVA: 0x0001D9FB File Offset: 0x0001C9FB
		public void Initialize(uint initialValue)
		{
			this.Shift = 3;
			this.Summary = (ushort)(initialValue << (int)this.Shift);
			this.Count = 7;
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x0001DA20 File Offset: 0x0001CA20
		public uint Mean()
		{
			uint num = (uint)(this.Summary >> (int)this.Shift);
			this.Summary = (ushort)((uint)this.Summary - num);
			return (uint)((ulong)num + (ulong)((num == 0U) ? 1L : 0L));
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x0001DA5C File Offset: 0x0001CA5C
		public void Update()
		{
			if (this.Shift < 7 && (this.Count -= 1) == 0)
			{
				this.Summary += this.Summary;
				int num = 3;
				byte shift;
				this.Shift = (shift = this.Shift) + 1;
				this.Count = num << (int)(shift & 31);
			}
		}

		// Token: 0x0400026E RID: 622
		private const byte PeriodBitCount = 7;

		// Token: 0x0400026F RID: 623
		public ushort Summary;

		// Token: 0x04000270 RID: 624
		public byte Shift;

		// Token: 0x04000271 RID: 625
		public byte Count;
	}
}
