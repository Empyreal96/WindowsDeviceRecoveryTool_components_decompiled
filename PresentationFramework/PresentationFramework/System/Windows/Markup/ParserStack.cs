using System;
using System.Collections;

namespace System.Windows.Markup
{
	// Token: 0x02000226 RID: 550
	internal class ParserStack : ArrayList
	{
		// Token: 0x06002205 RID: 8709 RVA: 0x000AA120 File Offset: 0x000A8320
		internal ParserStack()
		{
		}

		// Token: 0x06002206 RID: 8710 RVA: 0x000AA128 File Offset: 0x000A8328
		private ParserStack(ICollection collection) : base(collection)
		{
		}

		// Token: 0x06002207 RID: 8711 RVA: 0x000AA131 File Offset: 0x000A8331
		public void Push(object o)
		{
			this.Add(o);
		}

		// Token: 0x06002208 RID: 8712 RVA: 0x000AA13C File Offset: 0x000A833C
		public object Pop()
		{
			object result = this[this.Count - 1];
			this.RemoveAt(this.Count - 1);
			return result;
		}

		// Token: 0x06002209 RID: 8713 RVA: 0x000AA167 File Offset: 0x000A8367
		public object Peek()
		{
			return this[this.Count - 1];
		}

		// Token: 0x0600220A RID: 8714 RVA: 0x000AA177 File Offset: 0x000A8377
		public override object Clone()
		{
			return new ParserStack(this);
		}

		// Token: 0x17000824 RID: 2084
		// (get) Token: 0x0600220B RID: 8715 RVA: 0x000AA17F File Offset: 0x000A837F
		internal object CurrentContext
		{
			get
			{
				if (this.Count <= 0)
				{
					return null;
				}
				return this[this.Count - 1];
			}
		}

		// Token: 0x17000825 RID: 2085
		// (get) Token: 0x0600220C RID: 8716 RVA: 0x000AA19A File Offset: 0x000A839A
		internal object ParentContext
		{
			get
			{
				if (this.Count <= 1)
				{
					return null;
				}
				return this[this.Count - 2];
			}
		}

		// Token: 0x17000826 RID: 2086
		// (get) Token: 0x0600220D RID: 8717 RVA: 0x000AA1B5 File Offset: 0x000A83B5
		internal object GrandParentContext
		{
			get
			{
				if (this.Count <= 2)
				{
					return null;
				}
				return this[this.Count - 3];
			}
		}

		// Token: 0x17000827 RID: 2087
		// (get) Token: 0x0600220E RID: 8718 RVA: 0x000AA1D0 File Offset: 0x000A83D0
		internal object GreatGrandParentContext
		{
			get
			{
				if (this.Count <= 3)
				{
					return null;
				}
				return this[this.Count - 4];
			}
		}
	}
}
