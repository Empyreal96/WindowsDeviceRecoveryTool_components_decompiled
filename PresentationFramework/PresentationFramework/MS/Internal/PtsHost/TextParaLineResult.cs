using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using MS.Internal.Documents;

namespace MS.Internal.PtsHost
{
	// Token: 0x0200064F RID: 1615
	internal sealed class TextParaLineResult : LineResult
	{
		// Token: 0x06006B4C RID: 27468 RVA: 0x001F00B0 File Offset: 0x001EE2B0
		internal override ITextPointer GetTextPositionFromDistance(double distance)
		{
			return this._owner.GetTextPositionFromDistance(this._dcp, distance);
		}

		// Token: 0x06006B4D RID: 27469 RVA: 0x0000B02A File Offset: 0x0000922A
		internal override bool IsAtCaretUnitBoundary(ITextPointer position)
		{
			return false;
		}

		// Token: 0x06006B4E RID: 27470 RVA: 0x0000C238 File Offset: 0x0000A438
		internal override ITextPointer GetNextCaretUnitPosition(ITextPointer position, LogicalDirection direction)
		{
			return null;
		}

		// Token: 0x06006B4F RID: 27471 RVA: 0x0000C238 File Offset: 0x0000A438
		internal override ITextPointer GetBackspaceCaretUnitPosition(ITextPointer position)
		{
			return null;
		}

		// Token: 0x06006B50 RID: 27472 RVA: 0x0000C238 File Offset: 0x0000A438
		internal override ReadOnlyCollection<GlyphRun> GetGlyphRuns(ITextPointer start, ITextPointer end)
		{
			return null;
		}

		// Token: 0x06006B51 RID: 27473 RVA: 0x001F00C4 File Offset: 0x001EE2C4
		internal override ITextPointer GetContentEndPosition()
		{
			this.EnsureComplexData();
			return this._owner.GetTextPosition(this._dcp + this._cchContent, LogicalDirection.Backward);
		}

		// Token: 0x06006B52 RID: 27474 RVA: 0x001F00E5 File Offset: 0x001EE2E5
		internal override ITextPointer GetEllipsesPosition()
		{
			this.EnsureComplexData();
			if (this._cchEllipses != 0)
			{
				return this._owner.GetTextPosition(this._dcp + this._cch - this._cchEllipses, LogicalDirection.Forward);
			}
			return null;
		}

		// Token: 0x06006B53 RID: 27475 RVA: 0x001F0117 File Offset: 0x001EE317
		internal override int GetContentEndPositionCP()
		{
			this.EnsureComplexData();
			return this._dcp + this._cchContent;
		}

		// Token: 0x06006B54 RID: 27476 RVA: 0x001F012C File Offset: 0x001EE32C
		internal override int GetEllipsesPositionCP()
		{
			this.EnsureComplexData();
			return this._dcp + this._cch - this._cchEllipses;
		}

		// Token: 0x170019B5 RID: 6581
		// (get) Token: 0x06006B55 RID: 27477 RVA: 0x001F0148 File Offset: 0x001EE348
		internal override ITextPointer StartPosition
		{
			get
			{
				if (this._startPosition == null)
				{
					this._startPosition = this._owner.GetTextPosition(this._dcp, LogicalDirection.Forward);
				}
				return this._startPosition;
			}
		}

		// Token: 0x170019B6 RID: 6582
		// (get) Token: 0x06006B56 RID: 27478 RVA: 0x001F0170 File Offset: 0x001EE370
		internal override ITextPointer EndPosition
		{
			get
			{
				if (this._endPosition == null)
				{
					this._endPosition = this._owner.GetTextPosition(this._dcp + this._cch, LogicalDirection.Backward);
				}
				return this._endPosition;
			}
		}

		// Token: 0x170019B7 RID: 6583
		// (get) Token: 0x06006B57 RID: 27479 RVA: 0x001F019F File Offset: 0x001EE39F
		internal override int StartPositionCP
		{
			get
			{
				return this._dcp + this._owner.Paragraph.ParagraphStartCharacterPosition;
			}
		}

		// Token: 0x170019B8 RID: 6584
		// (get) Token: 0x06006B58 RID: 27480 RVA: 0x001F01B8 File Offset: 0x001EE3B8
		internal override int EndPositionCP
		{
			get
			{
				return this._dcp + this._cch + this._owner.Paragraph.ParagraphStartCharacterPosition;
			}
		}

		// Token: 0x170019B9 RID: 6585
		// (get) Token: 0x06006B59 RID: 27481 RVA: 0x001F01D8 File Offset: 0x001EE3D8
		internal override Rect LayoutBox
		{
			get
			{
				return this._layoutBox;
			}
		}

		// Token: 0x170019BA RID: 6586
		// (get) Token: 0x06006B5A RID: 27482 RVA: 0x001F01E0 File Offset: 0x001EE3E0
		internal override double Baseline
		{
			get
			{
				return this._baseline;
			}
		}

		// Token: 0x06006B5B RID: 27483 RVA: 0x001F01E8 File Offset: 0x001EE3E8
		internal TextParaLineResult(TextParaClient owner, int dcp, int cch, Rect layoutBox, double baseline)
		{
			this._owner = owner;
			this._dcp = dcp;
			this._cch = cch;
			this._layoutBox = layoutBox;
			this._baseline = baseline;
			this._cchContent = (this._cchEllipses = -1);
		}

		// Token: 0x170019BB RID: 6587
		// (get) Token: 0x06006B5C RID: 27484 RVA: 0x001F0230 File Offset: 0x001EE430
		// (set) Token: 0x06006B5D RID: 27485 RVA: 0x001F023F File Offset: 0x001EE43F
		internal int DcpLast
		{
			get
			{
				return this._dcp + this._cch;
			}
			set
			{
				this._cch = value - this._dcp;
			}
		}

		// Token: 0x06006B5E RID: 27486 RVA: 0x001F024F File Offset: 0x001EE44F
		private void EnsureComplexData()
		{
			if (this._cchContent == -1)
			{
				this._owner.GetLineDetails(this._dcp, out this._cchContent, out this._cchEllipses);
			}
		}

		// Token: 0x04003460 RID: 13408
		private readonly TextParaClient _owner;

		// Token: 0x04003461 RID: 13409
		private int _dcp;

		// Token: 0x04003462 RID: 13410
		private int _cch;

		// Token: 0x04003463 RID: 13411
		private readonly Rect _layoutBox;

		// Token: 0x04003464 RID: 13412
		private readonly double _baseline;

		// Token: 0x04003465 RID: 13413
		private ITextPointer _startPosition;

		// Token: 0x04003466 RID: 13414
		private ITextPointer _endPosition;

		// Token: 0x04003467 RID: 13415
		private int _cchContent;

		// Token: 0x04003468 RID: 13416
		private int _cchEllipses;
	}
}
