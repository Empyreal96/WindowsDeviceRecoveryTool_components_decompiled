using System;
using System.Windows;
using System.Windows.Documents;
using MS.Internal.Text;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000612 RID: 1554
	internal class CellInfo
	{
		// Token: 0x0600677C RID: 26492 RVA: 0x001CF0F0 File Offset: 0x001CD2F0
		internal CellInfo(TableParaClient tpc, CellParaClient cpc)
		{
			this._rectTable = new Rect(TextDpi.FromTextDpi(tpc.Rect.u), TextDpi.FromTextDpi(tpc.Rect.v), TextDpi.FromTextDpi(tpc.Rect.du), TextDpi.FromTextDpi(tpc.Rect.dv));
			this._rectCell = new Rect(TextDpi.FromTextDpi(cpc.Rect.u), TextDpi.FromTextDpi(cpc.Rect.v), TextDpi.FromTextDpi(cpc.Rect.du), TextDpi.FromTextDpi(cpc.Rect.dv));
			this._autofitWidth = tpc.AutofitWidth;
			this._columnWidths = new double[tpc.CalculatedColumns.Length];
			for (int i = 0; i < tpc.CalculatedColumns.Length; i++)
			{
				this._columnWidths[i] = tpc.CalculatedColumns[i].DurWidth;
			}
			this._cell = cpc.Cell;
		}

		// Token: 0x0600677D RID: 26493 RVA: 0x001CF1F0 File Offset: 0x001CD3F0
		internal void Adjust(Point ptAdjust)
		{
			this._rectTable.X = this._rectTable.X + ptAdjust.X;
			this._rectTable.Y = this._rectTable.Y + ptAdjust.Y;
			this._rectCell.X = this._rectCell.X + ptAdjust.X;
			this._rectCell.Y = this._rectCell.Y + ptAdjust.Y;
		}

		// Token: 0x17001903 RID: 6403
		// (get) Token: 0x0600677E RID: 26494 RVA: 0x001CF261 File Offset: 0x001CD461
		internal TableCell Cell
		{
			get
			{
				return this._cell;
			}
		}

		// Token: 0x17001904 RID: 6404
		// (get) Token: 0x0600677F RID: 26495 RVA: 0x001CF269 File Offset: 0x001CD469
		internal double[] TableColumnWidths
		{
			get
			{
				return this._columnWidths;
			}
		}

		// Token: 0x17001905 RID: 6405
		// (get) Token: 0x06006780 RID: 26496 RVA: 0x001CF271 File Offset: 0x001CD471
		internal double TableAutofitWidth
		{
			get
			{
				return this._autofitWidth;
			}
		}

		// Token: 0x17001906 RID: 6406
		// (get) Token: 0x06006781 RID: 26497 RVA: 0x001CF279 File Offset: 0x001CD479
		internal Rect TableArea
		{
			get
			{
				return this._rectTable;
			}
		}

		// Token: 0x17001907 RID: 6407
		// (get) Token: 0x06006782 RID: 26498 RVA: 0x001CF281 File Offset: 0x001CD481
		internal Rect CellArea
		{
			get
			{
				return this._rectCell;
			}
		}

		// Token: 0x04003370 RID: 13168
		private Rect _rectCell;

		// Token: 0x04003371 RID: 13169
		private Rect _rectTable;

		// Token: 0x04003372 RID: 13170
		private TableCell _cell;

		// Token: 0x04003373 RID: 13171
		private double[] _columnWidths;

		// Token: 0x04003374 RID: 13172
		private double _autofitWidth;
	}
}
