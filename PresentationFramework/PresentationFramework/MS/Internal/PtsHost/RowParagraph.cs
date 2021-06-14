using System;
using System.Security;
using System.Windows;
using System.Windows.Documents;
using MS.Internal.PtsHost.UnsafeNativeMethods;
using MS.Internal.Text;

namespace MS.Internal.PtsHost
{
	// Token: 0x0200063E RID: 1598
	internal sealed class RowParagraph : BaseParagraph
	{
		// Token: 0x06006A20 RID: 27168 RVA: 0x001CF9E3 File Offset: 0x001CDBE3
		internal RowParagraph(DependencyObject element, StructuralCache structuralCache) : base(element, structuralCache)
		{
		}

		// Token: 0x06006A21 RID: 27169 RVA: 0x001E3830 File Offset: 0x001E1A30
		public override void Dispose()
		{
			if (this._cellParagraphs != null)
			{
				for (int i = 0; i < this._cellParagraphs.Length; i++)
				{
					this._cellParagraphs[i].Dispose();
				}
			}
			this._cellParagraphs = null;
			base.Dispose();
		}

		// Token: 0x06006A22 RID: 27170 RVA: 0x0012B8F2 File Offset: 0x00129AF2
		internal override void GetParaProperties(ref PTS.FSPAP fspap)
		{
			Invariant.Assert(false);
		}

		// Token: 0x06006A23 RID: 27171 RVA: 0x001E3872 File Offset: 0x001E1A72
		internal override void CreateParaclient(out IntPtr paraClientHandle)
		{
			Invariant.Assert(false);
			paraClientHandle = IntPtr.Zero;
		}

		// Token: 0x06006A24 RID: 27172 RVA: 0x001E3881 File Offset: 0x001E1A81
		internal override void UpdGetParaChange(out PTS.FSKCHANGE fskch, out int fNoFurtherChanges)
		{
			base.UpdGetParaChange(out fskch, out fNoFurtherChanges);
			fskch = PTS.FSKCHANGE.fskchNew;
		}

		// Token: 0x06006A25 RID: 27173 RVA: 0x001E3890 File Offset: 0x001E1A90
		internal void GetRowProperties(uint fswdirTable, out PTS.FSTABLEROWPROPS rowprops)
		{
			bool flag = this.Row.Index == this.Row.RowGroup.Rows.Count - 1;
			PTS.FSKROWHEIGHTRESTRICTION fskrowheight;
			int num;
			this.GetRowHeight(out fskrowheight, out num);
			rowprops = default(PTS.FSTABLEROWPROPS);
			rowprops.fskrowbreak = PTS.FSKROWBREAKRESTRICTION.fskrowbreakAnywhere;
			rowprops.fskrowheight = fskrowheight;
			rowprops.dvrRowHeightRestriction = 0;
			rowprops.dvrAboveRow = num;
			rowprops.dvrBelowRow = num;
			int num2 = TextDpi.ToTextDpi(this.Table.InternalCellSpacing);
			MbpInfo mbpInfo = MbpInfo.FromElement(this.Table, base.StructuralCache.TextFormatterHost.PixelsPerDip);
			if (this.Row.Index == 0 && this.Table.IsFirstNonEmptyRowGroup(this.Row.RowGroup.Index))
			{
				rowprops.dvrAboveTopRow = mbpInfo.BPTop + num2 / 2;
			}
			else
			{
				rowprops.dvrAboveTopRow = num;
			}
			if (flag && this.Table.IsLastNonEmptyRowGroup(this.Row.RowGroup.Index))
			{
				rowprops.dvrBelowBottomRow = mbpInfo.BPBottom + num2 / 2;
			}
			else
			{
				rowprops.dvrBelowBottomRow = num;
			}
			rowprops.dvrAboveRowBreak = num2 / 2;
			rowprops.dvrBelowRowBreak = num2 / 2;
			rowprops.cCells = this.Row.FormatCellCount;
		}

		// Token: 0x06006A26 RID: 27174 RVA: 0x001E39C5 File Offset: 0x001E1BC5
		internal void FInterruptFormattingTable(int dvr, out int fInterrupt)
		{
			fInterrupt = 0;
		}

		// Token: 0x06006A27 RID: 27175 RVA: 0x001E39CC File Offset: 0x001E1BCC
		[SecurityCritical]
		internal unsafe void CalcHorizontalBBoxOfRow(int cCells, IntPtr* rgnmCell, IntPtr* rgpfsCell, out int urBBox, out int durBBox)
		{
			urBBox = 0;
			durBBox = 0;
			for (int i = 0; i < cCells; i++)
			{
				if (rgpfsCell[(IntPtr)i * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] != IntPtr.Zero)
				{
					CellParaClient cellParaClient = base.PtsContext.HandleToObject(rgpfsCell[(IntPtr)i * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)]) as CellParaClient;
					PTS.ValidateHandle(cellParaClient);
					durBBox = TextDpi.ToTextDpi(cellParaClient.TableParaClient.TableDesiredWidth);
					return;
				}
			}
		}

		// Token: 0x06006A28 RID: 27176 RVA: 0x001E3A3C File Offset: 0x001E1C3C
		[SecurityCritical]
		internal unsafe void GetCells(int cCells, IntPtr* rgnmCell, PTS.FSTABLEKCELLMERGE* rgkcellmerge)
		{
			Invariant.Assert(cCells == this.Row.FormatCellCount);
			Invariant.Assert(cCells >= this.Row.Cells.Count);
			int num = 0;
			for (int i = 0; i < this.Row.Cells.Count; i++)
			{
				TableCell tableCell = this.Row.Cells[i];
				if (tableCell.RowSpan == 1)
				{
					rgnmCell[(IntPtr)num * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = this._cellParagraphs[i].Handle;
					rgkcellmerge[num] = PTS.FSTABLEKCELLMERGE.fskcellmergeNo;
					num++;
				}
			}
			Invariant.Assert(cCells == num + this._spannedCells.Length);
			if (this._spannedCells.Length != 0)
			{
				bool flag = this.Row.Index == this.Row.RowGroup.Rows.Count - 1;
				for (int j = 0; j < this._spannedCells.Length; j++)
				{
					TableCell cell = this._spannedCells[j].Cell;
					rgnmCell[(IntPtr)num * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = this._spannedCells[j].Handle;
					if (cell.RowIndex == this.Row.Index)
					{
						rgkcellmerge[num] = (flag ? PTS.FSTABLEKCELLMERGE.fskcellmergeNo : PTS.FSTABLEKCELLMERGE.fskcellmergeFirst);
					}
					else if (this.Row.Index - cell.RowIndex + 1 < cell.RowSpan)
					{
						rgkcellmerge[num] = (flag ? PTS.FSTABLEKCELLMERGE.fskcellmergeLast : PTS.FSTABLEKCELLMERGE.fskcellmergeMiddle);
					}
					else
					{
						rgkcellmerge[num] = PTS.FSTABLEKCELLMERGE.fskcellmergeLast;
					}
					num++;
				}
			}
		}

		// Token: 0x06006A29 RID: 27177 RVA: 0x001E3BBC File Offset: 0x001E1DBC
		internal void CalculateRowSpans()
		{
			RowParagraph previousRow = null;
			if (this.Row.Index != 0 && this.Previous != null)
			{
				previousRow = (RowParagraph)this.Previous;
			}
			Invariant.Assert(this._cellParagraphs == null);
			this._cellParagraphs = new CellParagraph[this.Row.Cells.Count];
			for (int i = 0; i < this.Row.Cells.Count; i++)
			{
				this._cellParagraphs[i] = new CellParagraph(this.Row.Cells[i], base.StructuralCache);
			}
			Invariant.Assert(this._spannedCells == null);
			if (this.Row.SpannedCells != null)
			{
				this._spannedCells = new CellParagraph[this.Row.SpannedCells.Length];
			}
			else
			{
				this._spannedCells = new CellParagraph[0];
			}
			for (int j = 0; j < this._spannedCells.Length; j++)
			{
				this._spannedCells[j] = this.FindCellParagraphForCell(previousRow, this.Row.SpannedCells[j]);
			}
		}

		// Token: 0x06006A2A RID: 27178 RVA: 0x001E3CC4 File Offset: 0x001E1EC4
		internal void GetRowHeight(out PTS.FSKROWHEIGHTRESTRICTION fskrowheight, out int dvrAboveBelow)
		{
			bool flag = this.Row.Index == this.Row.RowGroup.Rows.Count - 1;
			if (this.Row.HasRealCells || (flag && this._spannedCells.Length != 0))
			{
				fskrowheight = PTS.FSKROWHEIGHTRESTRICTION.fskrowheightNatural;
				dvrAboveBelow = TextDpi.ToTextDpi(this.Table.InternalCellSpacing / 2.0);
				return;
			}
			fskrowheight = PTS.FSKROWHEIGHTRESTRICTION.fskrowheightExactNoBreak;
			dvrAboveBelow = 0;
		}

		// Token: 0x1700197F RID: 6527
		// (get) Token: 0x06006A2B RID: 27179 RVA: 0x001E3D35 File Offset: 0x001E1F35
		internal TableRow Row
		{
			get
			{
				return (TableRow)base.Element;
			}
		}

		// Token: 0x17001980 RID: 6528
		// (get) Token: 0x06006A2C RID: 27180 RVA: 0x001E3D42 File Offset: 0x001E1F42
		internal Table Table
		{
			get
			{
				return this.Row.Table;
			}
		}

		// Token: 0x17001981 RID: 6529
		// (get) Token: 0x06006A2D RID: 27181 RVA: 0x001E3D4F File Offset: 0x001E1F4F
		internal CellParagraph[] Cells
		{
			get
			{
				return this._cellParagraphs;
			}
		}

		// Token: 0x06006A2E RID: 27182 RVA: 0x001E3D58 File Offset: 0x001E1F58
		private CellParagraph FindCellParagraphForCell(RowParagraph previousRow, TableCell cell)
		{
			for (int i = 0; i < this._cellParagraphs.Length; i++)
			{
				if (this._cellParagraphs[i].Cell == cell)
				{
					return this._cellParagraphs[i];
				}
			}
			if (previousRow != null)
			{
				for (int j = 0; j < previousRow._spannedCells.Length; j++)
				{
					if (previousRow._spannedCells[j].Cell == cell)
					{
						return previousRow._spannedCells[j];
					}
				}
			}
			Invariant.Assert(false, "Structural integrity for table not correct.");
			return null;
		}

		// Token: 0x0400342A RID: 13354
		private CellParagraph[] _cellParagraphs;

		// Token: 0x0400342B RID: 13355
		private CellParagraph[] _spannedCells;
	}
}
