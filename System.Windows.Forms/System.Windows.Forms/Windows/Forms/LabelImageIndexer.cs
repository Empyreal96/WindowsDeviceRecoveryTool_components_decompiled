using System;

namespace System.Windows.Forms
{
	// Token: 0x020002AA RID: 682
	internal class LabelImageIndexer : ImageList.Indexer
	{
		// Token: 0x060027BE RID: 10174 RVA: 0x000B9CF8 File Offset: 0x000B7EF8
		public LabelImageIndexer(Label owner)
		{
			this.owner = owner;
		}

		// Token: 0x170009A2 RID: 2466
		// (get) Token: 0x060027BF RID: 10175 RVA: 0x000B9D0E File Offset: 0x000B7F0E
		// (set) Token: 0x060027C0 RID: 10176 RVA: 0x0000701A File Offset: 0x0000521A
		public override ImageList ImageList
		{
			get
			{
				if (this.owner != null)
				{
					return this.owner.ImageList;
				}
				return null;
			}
			set
			{
			}
		}

		// Token: 0x170009A3 RID: 2467
		// (get) Token: 0x060027C1 RID: 10177 RVA: 0x000B9D25 File Offset: 0x000B7F25
		// (set) Token: 0x060027C2 RID: 10178 RVA: 0x000B9D2D File Offset: 0x000B7F2D
		public override string Key
		{
			get
			{
				return base.Key;
			}
			set
			{
				base.Key = value;
				this.useIntegerIndex = false;
			}
		}

		// Token: 0x170009A4 RID: 2468
		// (get) Token: 0x060027C3 RID: 10179 RVA: 0x000B9D3D File Offset: 0x000B7F3D
		// (set) Token: 0x060027C4 RID: 10180 RVA: 0x000B9D45 File Offset: 0x000B7F45
		public override int Index
		{
			get
			{
				return base.Index;
			}
			set
			{
				base.Index = value;
				this.useIntegerIndex = true;
			}
		}

		// Token: 0x170009A5 RID: 2469
		// (get) Token: 0x060027C5 RID: 10181 RVA: 0x000B9D58 File Offset: 0x000B7F58
		public override int ActualIndex
		{
			get
			{
				if (this.useIntegerIndex)
				{
					if (this.Index >= this.ImageList.Images.Count)
					{
						return this.ImageList.Images.Count - 1;
					}
					return this.Index;
				}
				else
				{
					if (this.ImageList != null)
					{
						return this.ImageList.Images.IndexOfKey(this.Key);
					}
					return -1;
				}
			}
		}

		// Token: 0x04001169 RID: 4457
		private Label owner;

		// Token: 0x0400116A RID: 4458
		private bool useIntegerIndex = true;
	}
}
