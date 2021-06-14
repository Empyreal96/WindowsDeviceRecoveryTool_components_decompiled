using System;

namespace MS.Internal.Data
{
	// Token: 0x0200073F RID: 1855
	internal struct RBFinger<T>
	{
		// Token: 0x17001C2D RID: 7213
		// (get) Token: 0x0600766A RID: 30314 RVA: 0x0021D7B0 File Offset: 0x0021B9B0
		// (set) Token: 0x0600766B RID: 30315 RVA: 0x0021D7B8 File Offset: 0x0021B9B8
		public RBNode<T> Node { get; set; }

		// Token: 0x17001C2E RID: 7214
		// (get) Token: 0x0600766C RID: 30316 RVA: 0x0021D7C1 File Offset: 0x0021B9C1
		// (set) Token: 0x0600766D RID: 30317 RVA: 0x0021D7C9 File Offset: 0x0021B9C9
		public int Offset { get; set; }

		// Token: 0x17001C2F RID: 7215
		// (get) Token: 0x0600766E RID: 30318 RVA: 0x0021D7D2 File Offset: 0x0021B9D2
		// (set) Token: 0x0600766F RID: 30319 RVA: 0x0021D7DA File Offset: 0x0021B9DA
		public int Index { get; set; }

		// Token: 0x17001C30 RID: 7216
		// (get) Token: 0x06007670 RID: 30320 RVA: 0x0021D7E3 File Offset: 0x0021B9E3
		// (set) Token: 0x06007671 RID: 30321 RVA: 0x0021D7EB File Offset: 0x0021B9EB
		public bool Found { get; set; }

		// Token: 0x17001C31 RID: 7217
		// (get) Token: 0x06007672 RID: 30322 RVA: 0x0021D7F4 File Offset: 0x0021B9F4
		public T Item
		{
			get
			{
				return this.Node.GetItemAt(this.Offset);
			}
		}

		// Token: 0x06007673 RID: 30323 RVA: 0x0021D807 File Offset: 0x0021BA07
		public void SetItem(T x)
		{
			this.Node.SetItemAt(this.Offset, x);
		}

		// Token: 0x17001C32 RID: 7218
		// (get) Token: 0x06007674 RID: 30324 RVA: 0x0021D81C File Offset: 0x0021BA1C
		public bool IsValid
		{
			get
			{
				return this.Node != null && this.Node.HasData;
			}
		}

		// Token: 0x06007675 RID: 30325 RVA: 0x0021D833 File Offset: 0x0021BA33
		public static RBFinger<T>operator +(RBFinger<T> finger, int delta)
		{
			if (delta >= 0)
			{
				while (delta > 0)
				{
					if (!finger.IsValid)
					{
						break;
					}
					finger = ++finger;
					delta--;
				}
			}
			else
			{
				while (delta < 0 && finger.IsValid)
				{
					finger = --finger;
					delta++;
				}
			}
			return finger;
		}

		// Token: 0x06007676 RID: 30326 RVA: 0x0021D872 File Offset: 0x0021BA72
		public static RBFinger<T>operator -(RBFinger<T> finger, int delta)
		{
			return finger + -delta;
		}

		// Token: 0x06007677 RID: 30327 RVA: 0x0021D87C File Offset: 0x0021BA7C
		public static int operator -(RBFinger<T> f1, RBFinger<T> f2)
		{
			return f1.Index - f2.Index;
		}

		// Token: 0x06007678 RID: 30328 RVA: 0x0021D890 File Offset: 0x0021BA90
		public static RBFinger<T>operator ++(RBFinger<T> finger)
		{
			finger.Offset++;
			finger.Index++;
			if (finger.Offset == finger.Node.Size)
			{
				finger.Node = finger.Node.GetSuccessor();
				finger.Offset = 0;
			}
			return finger;
		}

		// Token: 0x06007679 RID: 30329 RVA: 0x0021D8EC File Offset: 0x0021BAEC
		public static RBFinger<T>operator --(RBFinger<T> finger)
		{
			finger.Offset--;
			finger.Index--;
			if (finger.Offset < 0)
			{
				finger.Node = finger.Node.GetPredecessor();
				if (finger.Node != null)
				{
					finger.Offset = finger.Node.Size - 1;
				}
			}
			return finger;
		}

		// Token: 0x0600767A RID: 30330 RVA: 0x0021D953 File Offset: 0x0021BB53
		public static bool operator <(RBFinger<T> f1, RBFinger<T> f2)
		{
			return f1.Index < f2.Index;
		}

		// Token: 0x0600767B RID: 30331 RVA: 0x0021D965 File Offset: 0x0021BB65
		public static bool operator >(RBFinger<T> f1, RBFinger<T> f2)
		{
			return f1.Index > f2.Index;
		}
	}
}
