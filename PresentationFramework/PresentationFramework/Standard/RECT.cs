using System;

namespace Standard
{
	// Token: 0x02000058 RID: 88
	internal struct RECT
	{
		// Token: 0x0600006D RID: 109 RVA: 0x000032EF File Offset: 0x000014EF
		public void Offset(int dx, int dy)
		{
			this._left += dx;
			this._top += dy;
			this._right += dx;
			this._bottom += dy;
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600006E RID: 110 RVA: 0x00003329 File Offset: 0x00001529
		// (set) Token: 0x0600006F RID: 111 RVA: 0x00003331 File Offset: 0x00001531
		public int Left
		{
			get
			{
				return this._left;
			}
			set
			{
				this._left = value;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000070 RID: 112 RVA: 0x0000333A File Offset: 0x0000153A
		// (set) Token: 0x06000071 RID: 113 RVA: 0x00003342 File Offset: 0x00001542
		public int Right
		{
			get
			{
				return this._right;
			}
			set
			{
				this._right = value;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000072 RID: 114 RVA: 0x0000334B File Offset: 0x0000154B
		// (set) Token: 0x06000073 RID: 115 RVA: 0x00003353 File Offset: 0x00001553
		public int Top
		{
			get
			{
				return this._top;
			}
			set
			{
				this._top = value;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000074 RID: 116 RVA: 0x0000335C File Offset: 0x0000155C
		// (set) Token: 0x06000075 RID: 117 RVA: 0x00003364 File Offset: 0x00001564
		public int Bottom
		{
			get
			{
				return this._bottom;
			}
			set
			{
				this._bottom = value;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000076 RID: 118 RVA: 0x0000336D File Offset: 0x0000156D
		public int Width
		{
			get
			{
				return this._right - this._left;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000077 RID: 119 RVA: 0x0000337C File Offset: 0x0000157C
		public int Height
		{
			get
			{
				return this._bottom - this._top;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000078 RID: 120 RVA: 0x0000338C File Offset: 0x0000158C
		public POINT Position
		{
			get
			{
				return new POINT
				{
					x = this._left,
					y = this._top
				};
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000079 RID: 121 RVA: 0x000033BC File Offset: 0x000015BC
		public SIZE Size
		{
			get
			{
				return new SIZE
				{
					cx = this.Width,
					cy = this.Height
				};
			}
		}

		// Token: 0x0600007A RID: 122 RVA: 0x000033EC File Offset: 0x000015EC
		public static RECT Union(RECT rect1, RECT rect2)
		{
			return new RECT
			{
				Left = Math.Min(rect1.Left, rect2.Left),
				Top = Math.Min(rect1.Top, rect2.Top),
				Right = Math.Max(rect1.Right, rect2.Right),
				Bottom = Math.Max(rect1.Bottom, rect2.Bottom)
			};
		}

		// Token: 0x0600007B RID: 123 RVA: 0x0000346C File Offset: 0x0000166C
		public override bool Equals(object obj)
		{
			bool result;
			try
			{
				RECT rect = (RECT)obj;
				result = (rect._bottom == this._bottom && rect._left == this._left && rect._right == this._right && rect._top == this._top);
			}
			catch (InvalidCastException)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x0600007C RID: 124 RVA: 0x000034D4 File Offset: 0x000016D4
		public override int GetHashCode()
		{
			return (this._left << 16 | Utility.LOWORD(this._right)) ^ (this._top << 16 | Utility.LOWORD(this._bottom));
		}

		// Token: 0x04000488 RID: 1160
		private int _left;

		// Token: 0x04000489 RID: 1161
		private int _top;

		// Token: 0x0400048A RID: 1162
		private int _right;

		// Token: 0x0400048B RID: 1163
		private int _bottom;
	}
}
