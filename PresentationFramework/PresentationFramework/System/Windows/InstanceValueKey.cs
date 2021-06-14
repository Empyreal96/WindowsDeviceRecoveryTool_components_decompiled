using System;

namespace System.Windows
{
	// Token: 0x0200010B RID: 267
	internal class InstanceValueKey
	{
		// Token: 0x06000966 RID: 2406 RVA: 0x00020F91 File Offset: 0x0001F191
		internal InstanceValueKey(int childIndex, int dpIndex, int index)
		{
			this._childIndex = childIndex;
			this._dpIndex = dpIndex;
			this._index = index;
		}

		// Token: 0x06000967 RID: 2407 RVA: 0x00020FB0 File Offset: 0x0001F1B0
		public override bool Equals(object o)
		{
			InstanceValueKey instanceValueKey = o as InstanceValueKey;
			return instanceValueKey != null && (this._childIndex == instanceValueKey._childIndex && this._dpIndex == instanceValueKey._dpIndex) && this._index == instanceValueKey._index;
		}

		// Token: 0x06000968 RID: 2408 RVA: 0x00020FF5 File Offset: 0x0001F1F5
		public override int GetHashCode()
		{
			return 20000 * this._childIndex + 20 * this._dpIndex + this._index;
		}

		// Token: 0x0400081A RID: 2074
		private int _childIndex;

		// Token: 0x0400081B RID: 2075
		private int _dpIndex;

		// Token: 0x0400081C RID: 2076
		private int _index;
	}
}
