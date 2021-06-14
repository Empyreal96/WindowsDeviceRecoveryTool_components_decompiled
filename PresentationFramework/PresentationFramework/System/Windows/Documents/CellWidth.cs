using System;

namespace System.Windows.Documents
{
	// Token: 0x020003BF RID: 959
	internal class CellWidth
	{
		// Token: 0x0600337F RID: 13183 RVA: 0x000E6540 File Offset: 0x000E4740
		internal CellWidth()
		{
			this.Type = WidthType.WidthAuto;
			this.Value = 0L;
		}

		// Token: 0x06003380 RID: 13184 RVA: 0x000E6557 File Offset: 0x000E4757
		internal CellWidth(CellWidth cw)
		{
			this.Type = cw.Type;
			this.Value = cw.Value;
		}

		// Token: 0x17000D21 RID: 3361
		// (get) Token: 0x06003381 RID: 13185 RVA: 0x000E6577 File Offset: 0x000E4777
		// (set) Token: 0x06003382 RID: 13186 RVA: 0x000E657F File Offset: 0x000E477F
		internal WidthType Type
		{
			get
			{
				return this._type;
			}
			set
			{
				this._type = value;
			}
		}

		// Token: 0x17000D22 RID: 3362
		// (get) Token: 0x06003383 RID: 13187 RVA: 0x000E6588 File Offset: 0x000E4788
		// (set) Token: 0x06003384 RID: 13188 RVA: 0x000E6590 File Offset: 0x000E4790
		internal long Value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = value;
			}
		}

		// Token: 0x06003385 RID: 13189 RVA: 0x000E6599 File Offset: 0x000E4799
		internal void SetDefaults()
		{
			this.Type = WidthType.WidthAuto;
			this.Value = 0L;
		}

		// Token: 0x0400248A RID: 9354
		private WidthType _type;

		// Token: 0x0400248B RID: 9355
		private long _value;
	}
}
