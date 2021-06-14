using System;

namespace MS.Internal.Xaml.Context
{
	// Token: 0x02000805 RID: 2053
	internal abstract class XamlFrame
	{
		// Token: 0x06007DFF RID: 32255 RVA: 0x00235281 File Offset: 0x00233481
		protected XamlFrame()
		{
			this._depth = -1;
		}

		// Token: 0x06007E00 RID: 32256 RVA: 0x00235290 File Offset: 0x00233490
		protected XamlFrame(XamlFrame source)
		{
			this._depth = source._depth;
		}

		// Token: 0x06007E01 RID: 32257 RVA: 0x0003E384 File Offset: 0x0003C584
		public virtual XamlFrame Clone()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06007E02 RID: 32258
		public abstract void Reset();

		// Token: 0x17001D47 RID: 7495
		// (get) Token: 0x06007E03 RID: 32259 RVA: 0x002352A4 File Offset: 0x002334A4
		public int Depth
		{
			get
			{
				return this._depth;
			}
		}

		// Token: 0x17001D48 RID: 7496
		// (get) Token: 0x06007E04 RID: 32260 RVA: 0x002352AC File Offset: 0x002334AC
		// (set) Token: 0x06007E05 RID: 32261 RVA: 0x002352B4 File Offset: 0x002334B4
		public XamlFrame Previous
		{
			get
			{
				return this._previous;
			}
			set
			{
				this._previous = value;
				this._depth = ((this._previous == null) ? 0 : (this._previous._depth + 1));
			}
		}

		// Token: 0x04003B7A RID: 15226
		private int _depth;

		// Token: 0x04003B7B RID: 15227
		private XamlFrame _previous;
	}
}
