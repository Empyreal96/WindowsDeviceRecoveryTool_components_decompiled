using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using MS.Internal.PtsHost;

namespace MS.Internal.Documents
{
	// Token: 0x020006E0 RID: 1760
	internal sealed class TextParagraphResult : ParagraphResult
	{
		// Token: 0x06007178 RID: 29048 RVA: 0x002074A7 File Offset: 0x002056A7
		internal TextParagraphResult(TextParaClient paraClient) : base(paraClient)
		{
		}

		// Token: 0x06007179 RID: 29049 RVA: 0x00207526 File Offset: 0x00205726
		internal Rect GetRectangleFromTextPosition(ITextPointer position)
		{
			return ((TextParaClient)this._paraClient).GetRectangleFromTextPosition(position);
		}

		// Token: 0x0600717A RID: 29050 RVA: 0x00207539 File Offset: 0x00205739
		internal Geometry GetTightBoundingGeometryFromTextPositions(ITextPointer startPosition, ITextPointer endPosition, double paragraphTopSpace, Rect visibleRect)
		{
			return ((TextParaClient)this._paraClient).GetTightBoundingGeometryFromTextPositions(startPosition, endPosition, paragraphTopSpace, visibleRect);
		}

		// Token: 0x0600717B RID: 29051 RVA: 0x00207550 File Offset: 0x00205750
		internal bool IsAtCaretUnitBoundary(ITextPointer position)
		{
			return ((TextParaClient)this._paraClient).IsAtCaretUnitBoundary(position);
		}

		// Token: 0x0600717C RID: 29052 RVA: 0x00207563 File Offset: 0x00205763
		internal ITextPointer GetNextCaretUnitPosition(ITextPointer position, LogicalDirection direction)
		{
			return ((TextParaClient)this._paraClient).GetNextCaretUnitPosition(position, direction);
		}

		// Token: 0x0600717D RID: 29053 RVA: 0x00207577 File Offset: 0x00205777
		internal ITextPointer GetBackspaceCaretUnitPosition(ITextPointer position)
		{
			return ((TextParaClient)this._paraClient).GetBackspaceCaretUnitPosition(position);
		}

		// Token: 0x0600717E RID: 29054 RVA: 0x0020758A File Offset: 0x0020578A
		internal void GetGlyphRuns(List<GlyphRun> glyphRuns, ITextPointer start, ITextPointer end)
		{
			((TextParaClient)this._paraClient).GetGlyphRuns(glyphRuns, start, end);
		}

		// Token: 0x0600717F RID: 29055 RVA: 0x002075A0 File Offset: 0x002057A0
		internal override bool Contains(ITextPointer position, bool strict)
		{
			bool flag = base.Contains(position, strict);
			if (!flag && strict)
			{
				flag = (position.CompareTo(base.EndPosition) == 0);
			}
			return flag;
		}

		// Token: 0x17001AF1 RID: 6897
		// (get) Token: 0x06007180 RID: 29056 RVA: 0x002075CF File Offset: 0x002057CF
		internal ReadOnlyCollection<LineResult> Lines
		{
			get
			{
				if (this._lines == null)
				{
					this._lines = ((TextParaClient)this._paraClient).GetLineResults();
				}
				Invariant.Assert(this._lines != null, "Lines collection is null");
				return this._lines;
			}
		}

		// Token: 0x17001AF2 RID: 6898
		// (get) Token: 0x06007181 RID: 29057 RVA: 0x00207608 File Offset: 0x00205808
		internal ReadOnlyCollection<ParagraphResult> Floaters
		{
			get
			{
				if (this._floaters == null)
				{
					this._floaters = ((TextParaClient)this._paraClient).GetFloaters();
				}
				return this._floaters;
			}
		}

		// Token: 0x17001AF3 RID: 6899
		// (get) Token: 0x06007182 RID: 29058 RVA: 0x0020762E File Offset: 0x0020582E
		internal ReadOnlyCollection<ParagraphResult> Figures
		{
			get
			{
				if (this._figures == null)
				{
					this._figures = ((TextParaClient)this._paraClient).GetFigures();
				}
				return this._figures;
			}
		}

		// Token: 0x17001AF4 RID: 6900
		// (get) Token: 0x06007183 RID: 29059 RVA: 0x00207654 File Offset: 0x00205854
		internal override bool HasTextContent
		{
			get
			{
				return this.Lines.Count > 0 && !this.ContainsOnlyFloatingElements;
			}
		}

		// Token: 0x17001AF5 RID: 6901
		// (get) Token: 0x06007184 RID: 29060 RVA: 0x00207670 File Offset: 0x00205870
		private bool ContainsOnlyFloatingElements
		{
			get
			{
				bool result = false;
				TextParagraph textParagraph = this._paraClient.Paragraph as TextParagraph;
				Invariant.Assert(textParagraph != null);
				if (textParagraph.HasFiguresOrFloaters())
				{
					if (this.Lines.Count == 0)
					{
						result = true;
					}
					else if (this.Lines.Count == 1)
					{
						int lastDcpAttachedObjectBeforeLine = textParagraph.GetLastDcpAttachedObjectBeforeLine(0);
						if (lastDcpAttachedObjectBeforeLine + textParagraph.ParagraphStartCharacterPosition == textParagraph.ParagraphEndCharacterPosition)
						{
							result = true;
						}
					}
				}
				return result;
			}
		}

		// Token: 0x04003727 RID: 14119
		private ReadOnlyCollection<LineResult> _lines;

		// Token: 0x04003728 RID: 14120
		private ReadOnlyCollection<ParagraphResult> _floaters;

		// Token: 0x04003729 RID: 14121
		private ReadOnlyCollection<ParagraphResult> _figures;
	}
}
