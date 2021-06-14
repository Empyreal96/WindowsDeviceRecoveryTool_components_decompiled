using System;
using System.Globalization;
using System.Text;

namespace System.Windows.Documents
{
	// Token: 0x020003BD RID: 957
	internal class ParaBorder
	{
		// Token: 0x0600336C RID: 13164 RVA: 0x000E6038 File Offset: 0x000E4238
		internal ParaBorder()
		{
			this.BorderLeft = new BorderFormat();
			this.BorderTop = new BorderFormat();
			this.BorderRight = new BorderFormat();
			this.BorderBottom = new BorderFormat();
			this.BorderAll = new BorderFormat();
			this.Spacing = 0L;
		}

		// Token: 0x0600336D RID: 13165 RVA: 0x000E608C File Offset: 0x000E428C
		internal ParaBorder(ParaBorder pb)
		{
			this.BorderLeft = new BorderFormat(pb.BorderLeft);
			this.BorderTop = new BorderFormat(pb.BorderTop);
			this.BorderRight = new BorderFormat(pb.BorderRight);
			this.BorderBottom = new BorderFormat(pb.BorderBottom);
			this.BorderAll = new BorderFormat(pb.BorderAll);
			this.Spacing = pb.Spacing;
		}

		// Token: 0x17000D18 RID: 3352
		// (get) Token: 0x0600336E RID: 13166 RVA: 0x000E6100 File Offset: 0x000E4300
		// (set) Token: 0x0600336F RID: 13167 RVA: 0x000E6108 File Offset: 0x000E4308
		internal BorderFormat BorderLeft
		{
			get
			{
				return this._bfLeft;
			}
			set
			{
				this._bfLeft = value;
			}
		}

		// Token: 0x17000D19 RID: 3353
		// (get) Token: 0x06003370 RID: 13168 RVA: 0x000E6111 File Offset: 0x000E4311
		// (set) Token: 0x06003371 RID: 13169 RVA: 0x000E6119 File Offset: 0x000E4319
		internal BorderFormat BorderTop
		{
			get
			{
				return this._bfTop;
			}
			set
			{
				this._bfTop = value;
			}
		}

		// Token: 0x17000D1A RID: 3354
		// (get) Token: 0x06003372 RID: 13170 RVA: 0x000E6122 File Offset: 0x000E4322
		// (set) Token: 0x06003373 RID: 13171 RVA: 0x000E612A File Offset: 0x000E432A
		internal BorderFormat BorderRight
		{
			get
			{
				return this._bfRight;
			}
			set
			{
				this._bfRight = value;
			}
		}

		// Token: 0x17000D1B RID: 3355
		// (get) Token: 0x06003374 RID: 13172 RVA: 0x000E6133 File Offset: 0x000E4333
		// (set) Token: 0x06003375 RID: 13173 RVA: 0x000E613B File Offset: 0x000E433B
		internal BorderFormat BorderBottom
		{
			get
			{
				return this._bfBottom;
			}
			set
			{
				this._bfBottom = value;
			}
		}

		// Token: 0x17000D1C RID: 3356
		// (get) Token: 0x06003376 RID: 13174 RVA: 0x000E6144 File Offset: 0x000E4344
		// (set) Token: 0x06003377 RID: 13175 RVA: 0x000E614C File Offset: 0x000E434C
		internal BorderFormat BorderAll
		{
			get
			{
				return this._bfAll;
			}
			set
			{
				this._bfAll = value;
			}
		}

		// Token: 0x17000D1D RID: 3357
		// (get) Token: 0x06003378 RID: 13176 RVA: 0x000E6155 File Offset: 0x000E4355
		// (set) Token: 0x06003379 RID: 13177 RVA: 0x000E615D File Offset: 0x000E435D
		internal long Spacing
		{
			get
			{
				return this._nSpacing;
			}
			set
			{
				this._nSpacing = value;
			}
		}

		// Token: 0x17000D1E RID: 3358
		// (get) Token: 0x0600337A RID: 13178 RVA: 0x000E6166 File Offset: 0x000E4366
		// (set) Token: 0x0600337B RID: 13179 RVA: 0x000E6173 File Offset: 0x000E4373
		internal long CF
		{
			get
			{
				return this.BorderLeft.CF;
			}
			set
			{
				this.BorderLeft.CF = value;
				this.BorderTop.CF = value;
				this.BorderRight.CF = value;
				this.BorderBottom.CF = value;
				this.BorderAll.CF = value;
			}
		}

		// Token: 0x17000D1F RID: 3359
		// (get) Token: 0x0600337C RID: 13180 RVA: 0x000E61B4 File Offset: 0x000E43B4
		internal bool IsNone
		{
			get
			{
				return this.BorderLeft.IsNone && this.BorderTop.IsNone && this.BorderRight.IsNone && this.BorderBottom.IsNone && this.BorderAll.IsNone;
			}
		}

		// Token: 0x0600337D RID: 13181 RVA: 0x000E6204 File Offset: 0x000E4404
		internal string GetBorderAttributeString(ConverterState converterState)
		{
			if (this.IsNone)
			{
				return string.Empty;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" BorderThickness=\"");
			if (!this.BorderAll.IsNone)
			{
				stringBuilder.Append(Converters.TwipToPositiveVisiblePxString((double)this.BorderAll.EffectiveWidth));
			}
			else
			{
				stringBuilder.Append(Converters.TwipToPositiveVisiblePxString((double)this.BorderLeft.EffectiveWidth));
				stringBuilder.Append(",");
				stringBuilder.Append(Converters.TwipToPositiveVisiblePxString((double)this.BorderTop.EffectiveWidth));
				stringBuilder.Append(",");
				stringBuilder.Append(Converters.TwipToPositiveVisiblePxString((double)this.BorderRight.EffectiveWidth));
				stringBuilder.Append(",");
				stringBuilder.Append(Converters.TwipToPositiveVisiblePxString((double)this.BorderBottom.EffectiveWidth));
			}
			stringBuilder.Append("\"");
			ColorTableEntry colorTableEntry = null;
			if (this.CF >= 0L)
			{
				colorTableEntry = converterState.ColorTable.EntryAt((int)this.CF);
			}
			if (colorTableEntry != null)
			{
				stringBuilder.Append(" BorderBrush=\"");
				stringBuilder.Append(colorTableEntry.Color.ToString());
				stringBuilder.Append("\"");
			}
			else
			{
				stringBuilder.Append(" BorderBrush=\"#FF000000\"");
			}
			if (this.Spacing != 0L)
			{
				stringBuilder.Append(" Padding=\"");
				stringBuilder.Append(Converters.TwipToPositivePxString((double)this.Spacing));
				stringBuilder.Append("\"");
			}
			return stringBuilder.ToString();
		}

		// Token: 0x17000D20 RID: 3360
		// (get) Token: 0x0600337E RID: 13182 RVA: 0x000E6388 File Offset: 0x000E4588
		internal string RTFEncoding
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				if (this.IsNone)
				{
					stringBuilder.Append("\\brdrnil");
				}
				else
				{
					stringBuilder.Append("\\brdrl");
					stringBuilder.Append(this.BorderLeft.RTFEncoding);
					if (this.BorderLeft.CF >= 0L)
					{
						stringBuilder.Append("\\brdrcf");
						stringBuilder.Append(this.BorderLeft.CF.ToString(CultureInfo.InvariantCulture));
					}
					stringBuilder.Append("\\brdrt");
					stringBuilder.Append(this.BorderTop.RTFEncoding);
					if (this.BorderTop.CF >= 0L)
					{
						stringBuilder.Append("\\brdrcf");
						stringBuilder.Append(this.BorderTop.CF.ToString(CultureInfo.InvariantCulture));
					}
					stringBuilder.Append("\\brdrr");
					stringBuilder.Append(this.BorderRight.RTFEncoding);
					if (this.BorderRight.CF >= 0L)
					{
						stringBuilder.Append("\\brdrcf");
						stringBuilder.Append(this.BorderRight.CF.ToString(CultureInfo.InvariantCulture));
					}
					stringBuilder.Append("\\brdrb");
					stringBuilder.Append(this.BorderBottom.RTFEncoding);
					if (this.BorderBottom.CF >= 0L)
					{
						stringBuilder.Append("\\brdrcf");
						stringBuilder.Append(this.BorderBottom.CF.ToString(CultureInfo.InvariantCulture));
					}
					stringBuilder.Append("\\brsp");
					stringBuilder.Append(this.Spacing.ToString(CultureInfo.InvariantCulture));
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x0400247F RID: 9343
		private BorderFormat _bfLeft;

		// Token: 0x04002480 RID: 9344
		private BorderFormat _bfTop;

		// Token: 0x04002481 RID: 9345
		private BorderFormat _bfRight;

		// Token: 0x04002482 RID: 9346
		private BorderFormat _bfBottom;

		// Token: 0x04002483 RID: 9347
		private BorderFormat _bfAll;

		// Token: 0x04002484 RID: 9348
		private long _nSpacing;
	}
}
