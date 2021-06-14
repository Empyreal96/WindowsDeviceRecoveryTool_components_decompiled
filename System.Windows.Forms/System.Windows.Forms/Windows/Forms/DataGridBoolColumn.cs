using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace System.Windows.Forms
{
	/// <summary>Specifies a column in which each cell contains a check box for representing a Boolean value.</summary>
	// Token: 0x0200016C RID: 364
	public class DataGridBoolColumn : DataGridColumnStyle
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DataGridBoolColumn" /> class.</summary>
		// Token: 0x0600128D RID: 4749 RVA: 0x00046DC4 File Offset: 0x00044FC4
		public DataGridBoolColumn()
		{
		}

		/// <summary>Initializes a new instance of a <see cref="T:System.Windows.Forms.DataGridBoolColumn" /> with the specified <see cref="T:System.ComponentModel.PropertyDescriptor" />.</summary>
		/// <param name="prop">The <see cref="T:System.ComponentModel.PropertyDescriptor" /> associated with the column. </param>
		// Token: 0x0600128E RID: 4750 RVA: 0x00046E14 File Offset: 0x00045014
		public DataGridBoolColumn(PropertyDescriptor prop) : base(prop)
		{
		}

		/// <summary>Initializes a new instance of a <see cref="T:System.Windows.Forms.DataGridBoolColumn" /> with the specified <see cref="T:System.ComponentModel.PropertyDescriptor" />, and specifying whether the column style is a default column.</summary>
		/// <param name="prop">The <see cref="T:System.ComponentModel.PropertyDescriptor" /> associated with the column. </param>
		/// <param name="isDefault">
		///       <see langword="true" /> to specify the column as the default; otherwise, <see langword="false" />. </param>
		// Token: 0x0600128F RID: 4751 RVA: 0x00046E64 File Offset: 0x00045064
		public DataGridBoolColumn(PropertyDescriptor prop, bool isDefault) : base(prop, isDefault)
		{
		}

		/// <summary>Gets or sets the actual value used when setting the value of the column to <see langword="true" />.</summary>
		/// <returns>The value, typed as <see cref="T:System.Object" />.</returns>
		// Token: 0x17000489 RID: 1161
		// (get) Token: 0x06001290 RID: 4752 RVA: 0x00046EB5 File Offset: 0x000450B5
		// (set) Token: 0x06001291 RID: 4753 RVA: 0x00046EBD File Offset: 0x000450BD
		[TypeConverter(typeof(StringConverter))]
		[DefaultValue(true)]
		public object TrueValue
		{
			get
			{
				return this.trueValue;
			}
			set
			{
				if (!this.trueValue.Equals(value))
				{
					this.trueValue = value;
					this.OnTrueValueChanged(EventArgs.Empty);
					this.Invalidate();
				}
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.DataGridBoolColumn.TrueValue" /> property value is changed.</summary>
		// Token: 0x140000C6 RID: 198
		// (add) Token: 0x06001292 RID: 4754 RVA: 0x00046EE5 File Offset: 0x000450E5
		// (remove) Token: 0x06001293 RID: 4755 RVA: 0x00046EF8 File Offset: 0x000450F8
		public event EventHandler TrueValueChanged
		{
			add
			{
				base.Events.AddHandler(DataGridBoolColumn.EventTrueValue, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGridBoolColumn.EventTrueValue, value);
			}
		}

		/// <summary>Gets or sets the actual value used when setting the value of the column to <see langword="false" />.</summary>
		/// <returns>The value, typed as <see cref="T:System.Object" />.</returns>
		// Token: 0x1700048A RID: 1162
		// (get) Token: 0x06001294 RID: 4756 RVA: 0x00046F0B File Offset: 0x0004510B
		// (set) Token: 0x06001295 RID: 4757 RVA: 0x00046F13 File Offset: 0x00045113
		[TypeConverter(typeof(StringConverter))]
		[DefaultValue(false)]
		public object FalseValue
		{
			get
			{
				return this.falseValue;
			}
			set
			{
				if (!this.falseValue.Equals(value))
				{
					this.falseValue = value;
					this.OnFalseValueChanged(EventArgs.Empty);
					this.Invalidate();
				}
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.DataGridBoolColumn.FalseValue" /> property is changed.</summary>
		// Token: 0x140000C7 RID: 199
		// (add) Token: 0x06001296 RID: 4758 RVA: 0x00046F3B File Offset: 0x0004513B
		// (remove) Token: 0x06001297 RID: 4759 RVA: 0x00046F4E File Offset: 0x0004514E
		public event EventHandler FalseValueChanged
		{
			add
			{
				base.Events.AddHandler(DataGridBoolColumn.EventFalseValue, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGridBoolColumn.EventFalseValue, value);
			}
		}

		/// <summary>Gets or sets the actual value used when setting the value of the column to <see cref="F:System.DBNull.Value" />.</summary>
		/// <returns>The value, typed as <see cref="T:System.Object" />.</returns>
		// Token: 0x1700048B RID: 1163
		// (get) Token: 0x06001298 RID: 4760 RVA: 0x00046F61 File Offset: 0x00045161
		// (set) Token: 0x06001299 RID: 4761 RVA: 0x00046F69 File Offset: 0x00045169
		[TypeConverter(typeof(StringConverter))]
		public object NullValue
		{
			get
			{
				return this.nullValue;
			}
			set
			{
				if (!this.nullValue.Equals(value))
				{
					this.nullValue = value;
					this.OnFalseValueChanged(EventArgs.Empty);
					this.Invalidate();
				}
			}
		}

		/// <summary>Notifies a column that it must relinquish the focus to the control it is hosting.</summary>
		// Token: 0x0600129A RID: 4762 RVA: 0x00046F91 File Offset: 0x00045191
		protected internal override void ConcedeFocus()
		{
			base.ConcedeFocus();
			this.isSelected = false;
			this.isEditing = false;
		}

		// Token: 0x0600129B RID: 4763 RVA: 0x00046FA8 File Offset: 0x000451A8
		private Rectangle GetCheckBoxBounds(Rectangle bounds, bool alignToRight)
		{
			if (alignToRight)
			{
				return new Rectangle(bounds.X + (bounds.Width - DataGridBoolColumn.idealCheckSize) / 2, bounds.Y + (bounds.Height - DataGridBoolColumn.idealCheckSize) / 2, (bounds.Width < DataGridBoolColumn.idealCheckSize) ? bounds.Width : DataGridBoolColumn.idealCheckSize, DataGridBoolColumn.idealCheckSize);
			}
			return new Rectangle(Math.Max(0, bounds.X + (bounds.Width - DataGridBoolColumn.idealCheckSize) / 2), Math.Max(0, bounds.Y + (bounds.Height - DataGridBoolColumn.idealCheckSize) / 2), (bounds.Width < DataGridBoolColumn.idealCheckSize) ? bounds.Width : DataGridBoolColumn.idealCheckSize, DataGridBoolColumn.idealCheckSize);
		}

		/// <summary>Gets the value at the specified row.</summary>
		/// <param name="lm">The <see cref="T:System.Windows.Forms.CurrencyManager" /> for the column. </param>
		/// <param name="row">The row number. </param>
		/// <returns>The value, typed as <see cref="T:System.Object" />.</returns>
		// Token: 0x0600129C RID: 4764 RVA: 0x00047070 File Offset: 0x00045270
		protected internal override object GetColumnValueAtRow(CurrencyManager lm, int row)
		{
			object columnValueAtRow = base.GetColumnValueAtRow(lm, row);
			object result = Convert.DBNull;
			if (columnValueAtRow.Equals(this.trueValue))
			{
				result = true;
			}
			else if (columnValueAtRow.Equals(this.falseValue))
			{
				result = false;
			}
			return result;
		}

		// Token: 0x0600129D RID: 4765 RVA: 0x000470BC File Offset: 0x000452BC
		private bool IsReadOnly()
		{
			bool flag = this.ReadOnly;
			if (this.DataGridTableStyle != null)
			{
				flag = (flag || this.DataGridTableStyle.ReadOnly);
				if (this.DataGridTableStyle.DataGrid != null)
				{
					flag = (flag || this.DataGridTableStyle.DataGrid.ReadOnly);
				}
			}
			return flag;
		}

		/// <summary>Sets the value of a specified row.</summary>
		/// <param name="lm">The <see cref="T:System.Windows.Forms.CurrencyManager" /> for the column. </param>
		/// <param name="row">The row number. </param>
		/// <param name="value">The value to set, typed as <see cref="T:System.Object" />. </param>
		// Token: 0x0600129E RID: 4766 RVA: 0x00047110 File Offset: 0x00045310
		protected internal override void SetColumnValueAtRow(CurrencyManager lm, int row, object value)
		{
			object value2 = null;
			if (true.Equals(value))
			{
				value2 = this.TrueValue;
			}
			else if (false.Equals(value))
			{
				value2 = this.FalseValue;
			}
			else if (Convert.IsDBNull(value))
			{
				value2 = this.NullValue;
			}
			this.currentValue = value2;
			base.SetColumnValueAtRow(lm, row, value2);
		}

		/// <summary>Gets the optimum width and height of a cell given a specific value to contain.</summary>
		/// <param name="g">A <see cref="T:System.Drawing.Graphics" /> that draws the cell. </param>
		/// <param name="value">The value that must fit in the cell. </param>
		/// <returns>A <see cref="T:System.Drawing.Size" /> that contains the drawing information for the cell.</returns>
		// Token: 0x0600129F RID: 4767 RVA: 0x00047168 File Offset: 0x00045368
		protected internal override Size GetPreferredSize(Graphics g, object value)
		{
			return new Size(DataGridBoolColumn.idealCheckSize + 2, DataGridBoolColumn.idealCheckSize + 2);
		}

		/// <summary>Gets the height of a cell in a column.</summary>
		/// <returns>The height of the column. The default is 16.</returns>
		// Token: 0x060012A0 RID: 4768 RVA: 0x0004717D File Offset: 0x0004537D
		protected internal override int GetMinimumHeight()
		{
			return DataGridBoolColumn.idealCheckSize + 2;
		}

		/// <summary>Gets the height used when resizing columns.</summary>
		/// <param name="g">A <see cref="T:System.Drawing.Graphics" /> that draws on the screen. </param>
		/// <param name="value">An <see cref="T:System.Object" /> that contains the value to be drawn to the screen. </param>
		/// <returns>The height used to automatically resize cells in a column.</returns>
		// Token: 0x060012A1 RID: 4769 RVA: 0x0004717D File Offset: 0x0004537D
		protected internal override int GetPreferredHeight(Graphics g, object value)
		{
			return DataGridBoolColumn.idealCheckSize + 2;
		}

		/// <summary>Initiates a request to interrupt an edit procedure.</summary>
		/// <param name="rowNum">The number of the row in which an operation is being interrupted. </param>
		// Token: 0x060012A2 RID: 4770 RVA: 0x00047186 File Offset: 0x00045386
		protected internal override void Abort(int rowNum)
		{
			this.isSelected = false;
			this.isEditing = false;
			this.Invalidate();
		}

		/// <summary>Initiates a request to complete an editing procedure.</summary>
		/// <param name="dataSource">The <see cref="T:System.Data.DataView" /> of the edited column. </param>
		/// <param name="rowNum">The number of the edited row. </param>
		/// <returns>
		///     <see langword="true" /> if the editing procedure committed successfully; otherwise, <see langword="false" />.</returns>
		// Token: 0x060012A3 RID: 4771 RVA: 0x0004719C File Offset: 0x0004539C
		protected internal override bool Commit(CurrencyManager dataSource, int rowNum)
		{
			this.isSelected = false;
			this.Invalidate();
			if (!this.isEditing)
			{
				return true;
			}
			this.SetColumnValueAtRow(dataSource, rowNum, this.currentValue);
			this.isEditing = false;
			return true;
		}

		/// <summary>Prepares the cell for editing a value.</summary>
		/// <param name="source">The <see cref="T:System.Data.DataView" /> of the edited cell. </param>
		/// <param name="rowNum">The row number of the edited cell. </param>
		/// <param name="bounds">The <see cref="T:System.Drawing.Rectangle" /> in which the control is to be sited. </param>
		/// <param name="readOnly">
		///       <see langword="true" /> if the value is read only; otherwise, <see langword="false" />. </param>
		/// <param name="displayText">The text to display in the cell. </param>
		/// <param name="cellIsVisible">
		///       <see langword="true" /> to show the cell; otherwise, <see langword="false" />. </param>
		// Token: 0x060012A4 RID: 4772 RVA: 0x000471CC File Offset: 0x000453CC
		protected internal override void Edit(CurrencyManager source, int rowNum, Rectangle bounds, bool readOnly, string displayText, bool cellIsVisible)
		{
			this.isSelected = true;
			DataGrid dataGrid = this.DataGridTableStyle.DataGrid;
			if (!dataGrid.Focused)
			{
				dataGrid.FocusInternal();
			}
			if (!readOnly && !this.IsReadOnly())
			{
				this.editingRow = rowNum;
				this.currentValue = this.GetColumnValueAtRow(source, rowNum);
			}
			base.Invalidate();
		}

		// Token: 0x060012A5 RID: 4773 RVA: 0x00047222 File Offset: 0x00045422
		internal override bool KeyPress(int rowNum, Keys keyData)
		{
			if (this.isSelected && this.editingRow == rowNum && !this.IsReadOnly() && (keyData & Keys.KeyCode) == Keys.Space)
			{
				this.ToggleValue();
				this.Invalidate();
				return true;
			}
			return base.KeyPress(rowNum, keyData);
		}

		// Token: 0x060012A6 RID: 4774 RVA: 0x0004725E File Offset: 0x0004545E
		internal override bool MouseDown(int rowNum, int x, int y)
		{
			base.MouseDown(rowNum, x, y);
			if (this.isSelected && this.editingRow == rowNum && !this.IsReadOnly())
			{
				this.ToggleValue();
				this.Invalidate();
				return true;
			}
			return false;
		}

		// Token: 0x060012A7 RID: 4775 RVA: 0x00047294 File Offset: 0x00045494
		private void OnTrueValueChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[DataGridBoolColumn.EventTrueValue] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x060012A8 RID: 4776 RVA: 0x000472C4 File Offset: 0x000454C4
		private void OnFalseValueChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[DataGridBoolColumn.EventFalseValue] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x060012A9 RID: 4777 RVA: 0x000472F4 File Offset: 0x000454F4
		private void OnAllowNullChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[DataGridBoolColumn.EventAllowNull] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Draws the <see cref="T:System.Windows.Forms.DataGridBoolColumn" /> with the given <see cref="T:System.Drawing.Graphics" />, <see cref="T:System.Drawing.Rectangle" /> and row number.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> to draw to. </param>
		/// <param name="bounds">The bounding <see cref="T:System.Drawing.Rectangle" /> to paint into. </param>
		/// <param name="source">The <see cref="T:System.Windows.Forms.CurrencyManager" /> of the column. </param>
		/// <param name="rowNum">The number of the row referred to in the underlying data. </param>
		// Token: 0x060012AA RID: 4778 RVA: 0x00047322 File Offset: 0x00045522
		protected internal override void Paint(Graphics g, Rectangle bounds, CurrencyManager source, int rowNum)
		{
			this.Paint(g, bounds, source, rowNum, false);
		}

		/// <summary>Draws the <see cref="T:System.Windows.Forms.DataGridBoolColumn" /> with the given <see cref="T:System.Drawing.Graphics" />, <see cref="T:System.Drawing.Rectangle" />, row number, and alignment settings.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> to draw to. </param>
		/// <param name="bounds">The bounding <see cref="T:System.Drawing.Rectangle" /> to paint into. </param>
		/// <param name="source">The <see cref="T:System.Windows.Forms.CurrencyManager" /> of the column. </param>
		/// <param name="rowNum">The number of the row in the underlying data table being referred to. </param>
		/// <param name="alignToRight">A value indicating whether to align the content to the right. <see langword="true" /> if the content is aligned to the right, otherwise, <see langword="false" />. </param>
		// Token: 0x060012AB RID: 4779 RVA: 0x00047330 File Offset: 0x00045530
		protected internal override void Paint(Graphics g, Rectangle bounds, CurrencyManager source, int rowNum, bool alignToRight)
		{
			this.Paint(g, bounds, source, rowNum, this.DataGridTableStyle.BackBrush, this.DataGridTableStyle.ForeBrush, alignToRight);
		}

		/// <summary>Draws the <see cref="T:System.Windows.Forms.DataGridBoolColumn" /> with the given <see cref="T:System.Drawing.Graphics" />, <see cref="T:System.Drawing.Rectangle" />, row number, <see cref="T:System.Drawing.Brush" />, and <see cref="T:System.Drawing.Color" />.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> to draw to. </param>
		/// <param name="bounds">The bounding <see cref="T:System.Drawing.Rectangle" /> to paint into. </param>
		/// <param name="source">The <see cref="T:System.Windows.Forms.CurrencyManager" /> of the column. </param>
		/// <param name="rowNum">The number of the row in the underlying data table being referred to. </param>
		/// <param name="backBrush">A <see cref="T:System.Drawing.Brush" /> used to paint the background color. </param>
		/// <param name="foreBrush">A <see cref="T:System.Drawing.Color" /> used to paint the foreground color. </param>
		/// <param name="alignToRight">A value indicating whether to align the content to the right. <see langword="true" /> if the content is aligned to the right, otherwise, <see langword="false" />. </param>
		// Token: 0x060012AC RID: 4780 RVA: 0x00047358 File Offset: 0x00045558
		protected internal override void Paint(Graphics g, Rectangle bounds, CurrencyManager source, int rowNum, Brush backBrush, Brush foreBrush, bool alignToRight)
		{
			object obj = (this.isEditing && this.editingRow == rowNum) ? this.currentValue : this.GetColumnValueAtRow(source, rowNum);
			ButtonState buttonState = ButtonState.Inactive;
			if (!Convert.IsDBNull(obj))
			{
				buttonState = (((bool)obj) ? ButtonState.Checked : ButtonState.Normal);
			}
			Rectangle checkBoxBounds = this.GetCheckBoxBounds(bounds, alignToRight);
			Region clip = g.Clip;
			g.ExcludeClip(checkBoxBounds);
			Brush brush = this.DataGridTableStyle.IsDefault ? this.DataGridTableStyle.DataGrid.SelectionBackBrush : this.DataGridTableStyle.SelectionBackBrush;
			if (this.isSelected && this.editingRow == rowNum && !this.IsReadOnly())
			{
				g.FillRectangle(brush, bounds);
			}
			else
			{
				g.FillRectangle(backBrush, bounds);
			}
			g.Clip = clip;
			if (buttonState == ButtonState.Inactive)
			{
				ControlPaint.DrawMixedCheckBox(g, checkBoxBounds, ButtonState.Checked);
			}
			else
			{
				ControlPaint.DrawCheckBox(g, checkBoxBounds, buttonState);
			}
			if (this.IsReadOnly() && this.isSelected && source.Position == rowNum)
			{
				bounds.Inflate(-1, -1);
				Pen pen = new Pen(brush);
				pen.DashStyle = DashStyle.Dash;
				g.DrawRectangle(pen, bounds);
				pen.Dispose();
				bounds.Inflate(1, 1);
			}
		}

		/// <summary>Gets or sets a value indicating whether null values are allowed.</summary>
		/// <returns>
		///     <see langword="true" /> if null values are allowed, otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x1700048C RID: 1164
		// (get) Token: 0x060012AD RID: 4781 RVA: 0x0004748C File Offset: 0x0004568C
		// (set) Token: 0x060012AE RID: 4782 RVA: 0x00047494 File Offset: 0x00045694
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		[SRDescription("DataGridBoolColumnAllowNullValue")]
		public bool AllowNull
		{
			get
			{
				return this.allowNull;
			}
			set
			{
				if (this.allowNull != value)
				{
					this.allowNull = value;
					if (!value && Convert.IsDBNull(this.currentValue))
					{
						this.currentValue = false;
						this.Invalidate();
					}
					this.OnAllowNullChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.DataGridBoolColumn.AllowNull" /> property is changed.</summary>
		// Token: 0x140000C8 RID: 200
		// (add) Token: 0x060012AF RID: 4783 RVA: 0x000474D3 File Offset: 0x000456D3
		// (remove) Token: 0x060012B0 RID: 4784 RVA: 0x000474E6 File Offset: 0x000456E6
		public event EventHandler AllowNullChanged
		{
			add
			{
				base.Events.AddHandler(DataGridBoolColumn.EventAllowNull, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGridBoolColumn.EventAllowNull, value);
			}
		}

		/// <summary>Enters a <see cref="F:System.DBNull.Value" /> into the column.</summary>
		/// <exception cref="T:System.Exception">The <see cref="P:System.Windows.Forms.DataGridBoolColumn.AllowNull" /> property is set to <see langword="false" />. </exception>
		// Token: 0x060012B1 RID: 4785 RVA: 0x000474F9 File Offset: 0x000456F9
		protected internal override void EnterNullValue()
		{
			if (!this.AllowNull || this.IsReadOnly())
			{
				return;
			}
			if (this.currentValue != Convert.DBNull)
			{
				this.currentValue = Convert.DBNull;
				this.Invalidate();
			}
		}

		// Token: 0x060012B2 RID: 4786 RVA: 0x0004752A File Offset: 0x0004572A
		private void ResetNullValue()
		{
			this.NullValue = Convert.DBNull;
		}

		// Token: 0x060012B3 RID: 4787 RVA: 0x00047537 File Offset: 0x00045737
		private bool ShouldSerializeNullValue()
		{
			return this.nullValue != Convert.DBNull;
		}

		// Token: 0x060012B4 RID: 4788 RVA: 0x0004754C File Offset: 0x0004574C
		private void ToggleValue()
		{
			if (this.currentValue is bool && !(bool)this.currentValue)
			{
				this.currentValue = true;
			}
			else if (this.AllowNull)
			{
				if (Convert.IsDBNull(this.currentValue))
				{
					this.currentValue = false;
				}
				else
				{
					this.currentValue = Convert.DBNull;
				}
			}
			else
			{
				this.currentValue = false;
			}
			this.isEditing = true;
			this.DataGridTableStyle.DataGrid.ColumnStartedEditing(Rectangle.Empty);
		}

		// Token: 0x04000957 RID: 2391
		private static readonly int idealCheckSize = 14;

		// Token: 0x04000958 RID: 2392
		private bool isEditing;

		// Token: 0x04000959 RID: 2393
		private bool isSelected;

		// Token: 0x0400095A RID: 2394
		private bool allowNull = true;

		// Token: 0x0400095B RID: 2395
		private int editingRow = -1;

		// Token: 0x0400095C RID: 2396
		private object currentValue = Convert.DBNull;

		// Token: 0x0400095D RID: 2397
		private object trueValue = true;

		// Token: 0x0400095E RID: 2398
		private object falseValue = false;

		// Token: 0x0400095F RID: 2399
		private object nullValue = Convert.DBNull;

		// Token: 0x04000960 RID: 2400
		private static readonly object EventTrueValue = new object();

		// Token: 0x04000961 RID: 2401
		private static readonly object EventFalseValue = new object();

		// Token: 0x04000962 RID: 2402
		private static readonly object EventAllowNull = new object();
	}
}
