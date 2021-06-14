using System;
using System.Drawing;

namespace System.Windows.Forms
{
	// Token: 0x0200016B RID: 363
	internal class DataGridAddNewRow : DataGridRow
	{
		// Token: 0x06001283 RID: 4739 RVA: 0x00046CB0 File Offset: 0x00044EB0
		public DataGridAddNewRow(DataGrid dGrid, DataGridTableStyle gridTable, int rowNum) : base(dGrid, gridTable, rowNum)
		{
		}

		// Token: 0x17000488 RID: 1160
		// (get) Token: 0x06001284 RID: 4740 RVA: 0x00046CBB File Offset: 0x00044EBB
		// (set) Token: 0x06001285 RID: 4741 RVA: 0x00046CC3 File Offset: 0x00044EC3
		public bool DataBound
		{
			get
			{
				return this.dataBound;
			}
			set
			{
				this.dataBound = value;
			}
		}

		// Token: 0x06001286 RID: 4742 RVA: 0x00046CCC File Offset: 0x00044ECC
		public override void OnEdit()
		{
			if (!this.DataBound)
			{
				base.DataGrid.AddNewRow();
			}
		}

		// Token: 0x06001287 RID: 4743 RVA: 0x00046CE1 File Offset: 0x00044EE1
		public override void OnRowLeave()
		{
			if (this.DataBound)
			{
				this.DataBound = false;
			}
		}

		// Token: 0x06001288 RID: 4744 RVA: 0x0000701A File Offset: 0x0000521A
		internal override void LoseChildFocus(Rectangle rowHeader, bool alignToRight)
		{
		}

		// Token: 0x06001289 RID: 4745 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		internal override bool ProcessTabKey(Keys keyData, Rectangle rowHeaders, bool alignToRight)
		{
			return false;
		}

		// Token: 0x0600128A RID: 4746 RVA: 0x00046CF2 File Offset: 0x00044EF2
		public override int Paint(Graphics g, Rectangle bounds, Rectangle trueRowBounds, int firstVisibleColumn, int columnCount)
		{
			return this.Paint(g, bounds, trueRowBounds, firstVisibleColumn, columnCount, false);
		}

		// Token: 0x0600128B RID: 4747 RVA: 0x00046D04 File Offset: 0x00044F04
		public override int Paint(Graphics g, Rectangle bounds, Rectangle trueRowBounds, int firstVisibleColumn, int columnCount, bool alignToRight)
		{
			Rectangle bounds2 = bounds;
			DataGridLineStyle gridLineStyle;
			if (this.dgTable.IsDefault)
			{
				gridLineStyle = base.DataGrid.GridLineStyle;
			}
			else
			{
				gridLineStyle = this.dgTable.GridLineStyle;
			}
			int num = (base.DataGrid == null) ? 0 : ((gridLineStyle == DataGridLineStyle.Solid) ? 1 : 0);
			bounds2.Height -= num;
			int num2 = base.PaintData(g, bounds2, firstVisibleColumn, columnCount, alignToRight);
			if (num > 0)
			{
				this.PaintBottomBorder(g, bounds, num2, num, alignToRight);
			}
			return num2;
		}

		// Token: 0x0600128C RID: 4748 RVA: 0x00046D80 File Offset: 0x00044F80
		protected override void PaintCellContents(Graphics g, Rectangle cellBounds, DataGridColumnStyle column, Brush backBr, Brush foreBrush, bool alignToRight)
		{
			if (this.DataBound)
			{
				CurrencyManager listManager = base.DataGrid.ListManager;
				column.Paint(g, cellBounds, listManager, base.RowNumber, alignToRight);
				return;
			}
			base.PaintCellContents(g, cellBounds, column, backBr, foreBrush, alignToRight);
		}

		// Token: 0x04000956 RID: 2390
		private bool dataBound;
	}
}
