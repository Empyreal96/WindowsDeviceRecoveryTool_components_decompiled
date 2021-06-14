using System;
using MS.Internal.Documents;

namespace System.Windows.Documents
{
	// Token: 0x02000335 RID: 821
	internal class ColumnResizeUndoUnit : ParentUndoUnit
	{
		// Token: 0x06002B4B RID: 11083 RVA: 0x000C59F8 File Offset: 0x000C3BF8
		internal ColumnResizeUndoUnit(TextPointer textPointerTable, int columnIndex, double[] columnWidths, double resizeAmount) : base("ColumnResize")
		{
			this._textContainer = textPointerTable.TextContainer;
			this._cpTable = this._textContainer.Start.GetOffsetToPosition(textPointerTable);
			this._columnWidths = columnWidths;
			this._columnIndex = columnIndex;
			this._resizeAmount = resizeAmount;
		}

		// Token: 0x06002B4C RID: 11084 RVA: 0x000C5A4C File Offset: 0x000C3C4C
		public override void Do()
		{
			UndoManager undoManager = base.TopContainer as UndoManager;
			IParentUndoUnit parentUndoUnit = null;
			TextPointer textPointer = new TextPointer(this._textContainer.Start, this._cpTable, LogicalDirection.Forward);
			Table table = (Table)textPointer.Parent;
			this._columnWidths[this._columnIndex] -= this._resizeAmount;
			if (this._columnIndex < table.ColumnCount - 1)
			{
				this._columnWidths[this._columnIndex + 1] += this._resizeAmount;
			}
			if (undoManager != null && undoManager.IsEnabled)
			{
				parentUndoUnit = new ColumnResizeUndoUnit(textPointer, this._columnIndex, this._columnWidths, -this._resizeAmount);
				undoManager.Open(parentUndoUnit);
			}
			TextRangeEditTables.EnsureTableColumnsAreFixedSize(table, this._columnWidths);
			if (parentUndoUnit != null)
			{
				undoManager.Close(parentUndoUnit, UndoCloseAction.Commit);
			}
		}

		// Token: 0x04001C8C RID: 7308
		private TextContainer _textContainer;

		// Token: 0x04001C8D RID: 7309
		private double[] _columnWidths;

		// Token: 0x04001C8E RID: 7310
		private int _cpTable;

		// Token: 0x04001C8F RID: 7311
		private int _columnIndex;

		// Token: 0x04001C90 RID: 7312
		private double _resizeAmount;
	}
}
