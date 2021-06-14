using System;
using System.Windows.Media.TextFormatting;

namespace MS.Internal.Text
{
	// Token: 0x02000601 RID: 1537
	internal struct LineMetrics
	{
		// Token: 0x06006650 RID: 26192 RVA: 0x001CC19C File Offset: 0x001CA39C
		internal LineMetrics(int length, double width, double height, double baseline, bool hasInlineObjects, TextLineBreak textLineBreak)
		{
			this._start = 0.0;
			this._width = width;
			this._height = height;
			this._baseline = baseline;
			this._textLineBreak = textLineBreak;
			this._packedData = (uint)((length & (int)LineMetrics.LengthMask) | (int)(hasInlineObjects ? LineMetrics.HasInlineObjectsMask : 0U));
		}

		// Token: 0x06006651 RID: 26193 RVA: 0x001CC1F0 File Offset: 0x001CA3F0
		internal LineMetrics(LineMetrics source, double start, double width)
		{
			this._start = start;
			this._width = width;
			this._height = source.Height;
			this._baseline = source.Baseline;
			this._textLineBreak = source.TextLineBreak;
			this._packedData = (source._packedData | LineMetrics.HasBeenUpdatedMask);
		}

		// Token: 0x06006652 RID: 26194 RVA: 0x001CC244 File Offset: 0x001CA444
		internal LineMetrics Dispose(bool returnUpdatedMetrics)
		{
			if (this._textLineBreak != null)
			{
				this._textLineBreak.Dispose();
				if (returnUpdatedMetrics)
				{
					return new LineMetrics(this.Length, this._width, this._height, this._baseline, this.HasInlineObjects, null);
				}
			}
			return this;
		}

		// Token: 0x17001887 RID: 6279
		// (get) Token: 0x06006653 RID: 26195 RVA: 0x001CC292 File Offset: 0x001CA492
		internal int Length
		{
			get
			{
				return (int)(this._packedData & LineMetrics.LengthMask);
			}
		}

		// Token: 0x17001888 RID: 6280
		// (get) Token: 0x06006654 RID: 26196 RVA: 0x001CC2A0 File Offset: 0x001CA4A0
		internal double Width
		{
			get
			{
				return this._width;
			}
		}

		// Token: 0x17001889 RID: 6281
		// (get) Token: 0x06006655 RID: 26197 RVA: 0x001CC2A8 File Offset: 0x001CA4A8
		internal double Height
		{
			get
			{
				return this._height;
			}
		}

		// Token: 0x1700188A RID: 6282
		// (get) Token: 0x06006656 RID: 26198 RVA: 0x001CC2B0 File Offset: 0x001CA4B0
		internal double Start
		{
			get
			{
				return this._start;
			}
		}

		// Token: 0x1700188B RID: 6283
		// (get) Token: 0x06006657 RID: 26199 RVA: 0x001CC2B8 File Offset: 0x001CA4B8
		internal double Baseline
		{
			get
			{
				return this._baseline;
			}
		}

		// Token: 0x1700188C RID: 6284
		// (get) Token: 0x06006658 RID: 26200 RVA: 0x001CC2C0 File Offset: 0x001CA4C0
		internal bool HasInlineObjects
		{
			get
			{
				return (this._packedData & LineMetrics.HasInlineObjectsMask) > 0U;
			}
		}

		// Token: 0x1700188D RID: 6285
		// (get) Token: 0x06006659 RID: 26201 RVA: 0x001CC2D1 File Offset: 0x001CA4D1
		internal TextLineBreak TextLineBreak
		{
			get
			{
				return this._textLineBreak;
			}
		}

		// Token: 0x04003301 RID: 13057
		private uint _packedData;

		// Token: 0x04003302 RID: 13058
		private double _width;

		// Token: 0x04003303 RID: 13059
		private double _height;

		// Token: 0x04003304 RID: 13060
		private double _start;

		// Token: 0x04003305 RID: 13061
		private double _baseline;

		// Token: 0x04003306 RID: 13062
		private TextLineBreak _textLineBreak;

		// Token: 0x04003307 RID: 13063
		private static readonly uint HasBeenUpdatedMask = 1073741824U;

		// Token: 0x04003308 RID: 13064
		private static readonly uint LengthMask = 1073741823U;

		// Token: 0x04003309 RID: 13065
		private static readonly uint HasInlineObjectsMask = 2147483648U;
	}
}
