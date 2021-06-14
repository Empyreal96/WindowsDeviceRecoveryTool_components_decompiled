using System;

namespace System.Windows.Controls
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Controls.DataGrid.ColumnReordering" /> event.</summary>
	// Token: 0x020004A5 RID: 1189
	public class DataGridColumnReorderingEventArgs : DataGridColumnEventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.DataGridColumnReorderingEventArgs" /> class.</summary>
		/// <param name="dataGridColumn">The column that is being moved.</param>
		// Token: 0x06004891 RID: 18577 RVA: 0x0014A062 File Offset: 0x00148262
		public DataGridColumnReorderingEventArgs(DataGridColumn dataGridColumn) : base(dataGridColumn)
		{
		}

		/// <summary>Gets or sets a value that indicates whether the reordering operation is stopped before completion.</summary>
		/// <returns>
		///     <see langword="true" /> if the reordering operation is stopped before completion; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x170011B4 RID: 4532
		// (get) Token: 0x06004892 RID: 18578 RVA: 0x0014A06B File Offset: 0x0014826B
		// (set) Token: 0x06004893 RID: 18579 RVA: 0x0014A073 File Offset: 0x00148273
		public bool Cancel
		{
			get
			{
				return this._cancel;
			}
			set
			{
				this._cancel = value;
			}
		}

		/// <summary>Gets or sets the control that is used to display the visual indicator of the current drop location during a column drag operation.</summary>
		/// <returns>The control that is used to display the drop location indicator during a column drag operation.</returns>
		// Token: 0x170011B5 RID: 4533
		// (get) Token: 0x06004894 RID: 18580 RVA: 0x0014A07C File Offset: 0x0014827C
		// (set) Token: 0x06004895 RID: 18581 RVA: 0x0014A084 File Offset: 0x00148284
		public Control DropLocationIndicator
		{
			get
			{
				return this._dropLocationIndicator;
			}
			set
			{
				this._dropLocationIndicator = value;
			}
		}

		/// <summary>Gets or sets the control that is used to display the visual indicator of the header for the column that is being dragged.</summary>
		/// <returns>The control that is used to display a dragged column header.</returns>
		// Token: 0x170011B6 RID: 4534
		// (get) Token: 0x06004896 RID: 18582 RVA: 0x0014A08D File Offset: 0x0014828D
		// (set) Token: 0x06004897 RID: 18583 RVA: 0x0014A095 File Offset: 0x00148295
		public Control DragIndicator
		{
			get
			{
				return this._dragIndicator;
			}
			set
			{
				this._dragIndicator = value;
			}
		}

		// Token: 0x040029A0 RID: 10656
		private bool _cancel;

		// Token: 0x040029A1 RID: 10657
		private Control _dropLocationIndicator;

		// Token: 0x040029A2 RID: 10658
		private Control _dragIndicator;
	}
}
