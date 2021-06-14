using System;
using System.Windows.Media;

namespace System.Windows.Documents
{
	// Token: 0x020003C6 RID: 966
	internal class ColorTableEntry
	{
		// Token: 0x060033FA RID: 13306 RVA: 0x000E79EA File Offset: 0x000E5BEA
		internal ColorTableEntry()
		{
			this._color = Color.FromArgb(byte.MaxValue, 0, 0, 0);
			this._bAuto = false;
		}

		// Token: 0x17000D56 RID: 3414
		// (get) Token: 0x060033FB RID: 13307 RVA: 0x000E7A0C File Offset: 0x000E5C0C
		// (set) Token: 0x060033FC RID: 13308 RVA: 0x000E7A14 File Offset: 0x000E5C14
		internal Color Color
		{
			get
			{
				return this._color;
			}
			set
			{
				this._color = value;
			}
		}

		// Token: 0x17000D57 RID: 3415
		// (get) Token: 0x060033FD RID: 13309 RVA: 0x000E7A1D File Offset: 0x000E5C1D
		// (set) Token: 0x060033FE RID: 13310 RVA: 0x000E7A25 File Offset: 0x000E5C25
		internal bool IsAuto
		{
			get
			{
				return this._bAuto;
			}
			set
			{
				this._bAuto = value;
			}
		}

		// Token: 0x17000D58 RID: 3416
		// (set) Token: 0x060033FF RID: 13311 RVA: 0x000E7A2E File Offset: 0x000E5C2E
		internal byte Red
		{
			set
			{
				this._color = Color.FromArgb(byte.MaxValue, value, this._color.G, this._color.B);
			}
		}

		// Token: 0x17000D59 RID: 3417
		// (set) Token: 0x06003400 RID: 13312 RVA: 0x000E7A57 File Offset: 0x000E5C57
		internal byte Green
		{
			set
			{
				this._color = Color.FromArgb(byte.MaxValue, this._color.R, value, this._color.B);
			}
		}

		// Token: 0x17000D5A RID: 3418
		// (set) Token: 0x06003401 RID: 13313 RVA: 0x000E7A80 File Offset: 0x000E5C80
		internal byte Blue
		{
			set
			{
				this._color = Color.FromArgb(byte.MaxValue, this._color.R, this._color.G, value);
			}
		}

		// Token: 0x040024B8 RID: 9400
		private Color _color;

		// Token: 0x040024B9 RID: 9401
		private bool _bAuto;
	}
}
