using System;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000101 RID: 257
	internal struct StringReference
	{
		// Token: 0x1700026E RID: 622
		// (get) Token: 0x06000BFD RID: 3069 RVA: 0x000311D1 File Offset: 0x0002F3D1
		public char[] Chars
		{
			get
			{
				return this._chars;
			}
		}

		// Token: 0x1700026F RID: 623
		// (get) Token: 0x06000BFE RID: 3070 RVA: 0x000311D9 File Offset: 0x0002F3D9
		public int StartIndex
		{
			get
			{
				return this._startIndex;
			}
		}

		// Token: 0x17000270 RID: 624
		// (get) Token: 0x06000BFF RID: 3071 RVA: 0x000311E1 File Offset: 0x0002F3E1
		public int Length
		{
			get
			{
				return this._length;
			}
		}

		// Token: 0x06000C00 RID: 3072 RVA: 0x000311E9 File Offset: 0x0002F3E9
		public StringReference(char[] chars, int startIndex, int length)
		{
			this._chars = chars;
			this._startIndex = startIndex;
			this._length = length;
		}

		// Token: 0x06000C01 RID: 3073 RVA: 0x00031200 File Offset: 0x0002F400
		public override string ToString()
		{
			return new string(this._chars, this._startIndex, this._length);
		}

		// Token: 0x04000458 RID: 1112
		private readonly char[] _chars;

		// Token: 0x04000459 RID: 1113
		private readonly int _startIndex;

		// Token: 0x0400045A RID: 1114
		private readonly int _length;
	}
}
