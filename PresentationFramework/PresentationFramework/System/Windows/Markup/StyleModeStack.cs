using System;
using System.Collections.Generic;

namespace System.Windows.Markup
{
	// Token: 0x0200022E RID: 558
	internal class StyleModeStack
	{
		// Token: 0x06002251 RID: 8785 RVA: 0x000AAD45 File Offset: 0x000A8F45
		internal StyleModeStack()
		{
			this.Push(StyleMode.Base);
		}

		// Token: 0x17000838 RID: 2104
		// (get) Token: 0x06002252 RID: 8786 RVA: 0x000AAD61 File Offset: 0x000A8F61
		internal int Depth
		{
			get
			{
				return this._stack.Count - 1;
			}
		}

		// Token: 0x17000839 RID: 2105
		// (get) Token: 0x06002253 RID: 8787 RVA: 0x000AAD70 File Offset: 0x000A8F70
		internal StyleMode Mode
		{
			get
			{
				return this._stack.Peek();
			}
		}

		// Token: 0x06002254 RID: 8788 RVA: 0x000AAD7D File Offset: 0x000A8F7D
		internal void Push(StyleMode mode)
		{
			this._stack.Push(mode);
		}

		// Token: 0x06002255 RID: 8789 RVA: 0x000AAD8B File Offset: 0x000A8F8B
		internal void Push()
		{
			this.Push(this.Mode);
		}

		// Token: 0x06002256 RID: 8790 RVA: 0x000AAD99 File Offset: 0x000A8F99
		internal StyleMode Pop()
		{
			return this._stack.Pop();
		}

		// Token: 0x040019EB RID: 6635
		private Stack<StyleMode> _stack = new Stack<StyleMode>(64);
	}
}
