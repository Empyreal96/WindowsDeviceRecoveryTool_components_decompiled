using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	/// <summary>Represents a panel that dynamically lays out its contents in a grid composed of rows and columns.</summary>
	// Token: 0x0200037C RID: 892
	[ProvideProperty("ColumnSpan", typeof(Control))]
	[ProvideProperty("RowSpan", typeof(Control))]
	[ProvideProperty("Row", typeof(Control))]
	[ProvideProperty("Column", typeof(Control))]
	[ProvideProperty("CellPosition", typeof(Control))]
	[DefaultProperty("ColumnCount")]
	[DesignerSerializer("System.Windows.Forms.Design.TableLayoutPanelCodeDomSerializer, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.ComponentModel.Design.Serialization.CodeDomSerializer, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[Docking(DockingBehavior.Never)]
	[Designer("System.Windows.Forms.Design.TableLayoutPanelDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[SRDescription("DescriptionTableLayoutPanel")]
	public class TableLayoutPanel : Panel, IExtenderProvider
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.TableLayoutPanel" /> class.</summary>
		// Token: 0x0600384A RID: 14410 RVA: 0x000FCE3D File Offset: 0x000FB03D
		public TableLayoutPanel()
		{
			this._tableLayoutSettings = TableLayout.CreateSettings(this);
		}

		/// <summary>Gets a cached instance of the panel's layout engine.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.Layout.LayoutEngine" /> for the panel's contents.</returns>
		// Token: 0x17000E0C RID: 3596
		// (get) Token: 0x0600384B RID: 14411 RVA: 0x000FCE51 File Offset: 0x000FB051
		public override LayoutEngine LayoutEngine
		{
			get
			{
				return TableLayout.Instance;
			}
		}

		/// <summary>Gets or sets a value representing the table layout settings.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.TableLayoutSettings" /> containing the table layout settings.</returns>
		/// <exception cref="T:System.NotSupportedException">The property value is <see langword="null" />, or an attempt was made to set <see cref="T:System.Windows.Forms.TableLayoutSettings" />  directly, which is not supported; instead, set individual properties.</exception>
		// Token: 0x17000E0D RID: 3597
		// (get) Token: 0x0600384C RID: 14412 RVA: 0x000FCE58 File Offset: 0x000FB058
		// (set) Token: 0x0600384D RID: 14413 RVA: 0x000FCE60 File Offset: 0x000FB060
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public TableLayoutSettings LayoutSettings
		{
			get
			{
				return this._tableLayoutSettings;
			}
			set
			{
				if (value != null && value.IsStub)
				{
					using (new LayoutTransaction(this, this, PropertyNames.LayoutSettings))
					{
						this._tableLayoutSettings.ApplySettings(value);
						return;
					}
				}
				throw new NotSupportedException(SR.GetString("TableLayoutSettingSettingsIsNotSupported"));
			}
		}

		/// <summary>Gets or sets the border style for the panel.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.BorderStyle" /> values describing the style of the border of the panel. The default is <see cref="F:System.Windows.Forms.BorderStyle.None" />.</returns>
		// Token: 0x17000E0E RID: 3598
		// (get) Token: 0x0600384E RID: 14414 RVA: 0x000F7565 File Offset: 0x000F5765
		// (set) Token: 0x0600384F RID: 14415 RVA: 0x000F756D File Offset: 0x000F576D
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Localizable(true)]
		public new BorderStyle BorderStyle
		{
			get
			{
				return base.BorderStyle;
			}
			set
			{
				base.BorderStyle = value;
			}
		}

		/// <summary>Gets or sets the style of the cell borders.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.TableLayoutPanelCellBorderStyle" /> values describing the style of all the cell borders in the table. The default is <see cref="F:System.Windows.Forms.TableLayoutPanelCellBorderStyle.None" />.</returns>
		// Token: 0x17000E0F RID: 3599
		// (get) Token: 0x06003850 RID: 14416 RVA: 0x000FCEC0 File Offset: 0x000FB0C0
		// (set) Token: 0x06003851 RID: 14417 RVA: 0x000FCECD File Offset: 0x000FB0CD
		[DefaultValue(TableLayoutPanelCellBorderStyle.None)]
		[SRCategory("CatAppearance")]
		[SRDescription("TableLayoutPanelCellBorderStyleDescr")]
		[Localizable(true)]
		public TableLayoutPanelCellBorderStyle CellBorderStyle
		{
			get
			{
				return this._tableLayoutSettings.CellBorderStyle;
			}
			set
			{
				this._tableLayoutSettings.CellBorderStyle = value;
				if (value != TableLayoutPanelCellBorderStyle.None)
				{
					base.SetStyle(ControlStyles.ResizeRedraw, true);
				}
				base.Invalidate();
			}
		}

		// Token: 0x17000E10 RID: 3600
		// (get) Token: 0x06003852 RID: 14418 RVA: 0x000FCEED File Offset: 0x000FB0ED
		private int CellBorderWidth
		{
			get
			{
				return this._tableLayoutSettings.CellBorderWidth;
			}
		}

		/// <summary>Gets the collection of controls contained within the <see cref="T:System.Windows.Forms.TableLayoutPanel" />.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.TableLayoutControlCollection" /> containing the controls associated with the current <see cref="T:System.Windows.Forms.TableLayoutPanel" />.</returns>
		// Token: 0x17000E11 RID: 3601
		// (get) Token: 0x06003853 RID: 14419 RVA: 0x000FCEFA File Offset: 0x000FB0FA
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[SRDescription("ControlControlsDescr")]
		public new TableLayoutControlCollection Controls
		{
			get
			{
				return (TableLayoutControlCollection)base.Controls;
			}
		}

		/// <summary>Gets or sets the number of columns in the table.</summary>
		/// <returns>The number of columns in the <see cref="T:System.Windows.Forms.TableLayoutPanel" /> control. The default is 0.</returns>
		// Token: 0x17000E12 RID: 3602
		// (get) Token: 0x06003854 RID: 14420 RVA: 0x000FCF07 File Offset: 0x000FB107
		// (set) Token: 0x06003855 RID: 14421 RVA: 0x000FCF14 File Offset: 0x000FB114
		[SRDescription("GridPanelColumnsDescr")]
		[SRCategory("CatLayout")]
		[DefaultValue(0)]
		[Localizable(true)]
		public int ColumnCount
		{
			get
			{
				return this._tableLayoutSettings.ColumnCount;
			}
			set
			{
				this._tableLayoutSettings.ColumnCount = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the <see cref="T:System.Windows.Forms.TableLayoutPanel" /> control should expand to accommodate new cells when all existing cells are occupied.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.TableLayoutPanelGrowStyle" /> indicating the growth scheme. The default is <see cref="F:System.Windows.Forms.TableLayoutPanelGrowStyle.AddRows" />.</returns>
		/// <exception cref="T:System.ArgumentException">The property value is invalid for the <see cref="T:System.Windows.Forms.TableLayoutPanelGrowStyle" /> enumeration.</exception>
		// Token: 0x17000E13 RID: 3603
		// (get) Token: 0x06003856 RID: 14422 RVA: 0x000FCF22 File Offset: 0x000FB122
		// (set) Token: 0x06003857 RID: 14423 RVA: 0x000FCF2F File Offset: 0x000FB12F
		[SRDescription("TableLayoutPanelGrowStyleDescr")]
		[SRCategory("CatLayout")]
		[DefaultValue(TableLayoutPanelGrowStyle.AddRows)]
		public TableLayoutPanelGrowStyle GrowStyle
		{
			get
			{
				return this._tableLayoutSettings.GrowStyle;
			}
			set
			{
				this._tableLayoutSettings.GrowStyle = value;
			}
		}

		/// <summary>Gets or sets the number of rows in the table.</summary>
		/// <returns>The number of rows in the <see cref="T:System.Windows.Forms.TableLayoutPanel" /> control. The default is 0.</returns>
		// Token: 0x17000E14 RID: 3604
		// (get) Token: 0x06003858 RID: 14424 RVA: 0x000FCF3D File Offset: 0x000FB13D
		// (set) Token: 0x06003859 RID: 14425 RVA: 0x000FCF4A File Offset: 0x000FB14A
		[SRDescription("GridPanelRowsDescr")]
		[SRCategory("CatLayout")]
		[DefaultValue(0)]
		[Localizable(true)]
		public int RowCount
		{
			get
			{
				return this._tableLayoutSettings.RowCount;
			}
			set
			{
				this._tableLayoutSettings.RowCount = value;
			}
		}

		/// <summary>Gets a collection of row styles for the <see cref="T:System.Windows.Forms.TableLayoutPanel" />.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.TableLayoutRowStyleCollection" /> containing a <see cref="T:System.Windows.Forms.RowStyle" /> for each row in the <see cref="T:System.Windows.Forms.TableLayoutPanel" /> control.</returns>
		// Token: 0x17000E15 RID: 3605
		// (get) Token: 0x0600385A RID: 14426 RVA: 0x000FCF58 File Offset: 0x000FB158
		[SRDescription("GridPanelRowStylesDescr")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[SRCategory("CatLayout")]
		[DisplayName("Rows")]
		[MergableProperty(false)]
		[Browsable(false)]
		public TableLayoutRowStyleCollection RowStyles
		{
			get
			{
				return this._tableLayoutSettings.RowStyles;
			}
		}

		/// <summary>Gets a collection of column styles for the <see cref="T:System.Windows.Forms.TableLayoutPanel" />.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.TableLayoutColumnStyleCollection" /> containing a <see cref="T:System.Windows.Forms.ColumnStyle" /> for each column in the <see cref="T:System.Windows.Forms.TableLayoutPanel" /> control.</returns>
		// Token: 0x17000E16 RID: 3606
		// (get) Token: 0x0600385B RID: 14427 RVA: 0x000FCF65 File Offset: 0x000FB165
		[SRDescription("GridPanelColumnStylesDescr")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[SRCategory("CatLayout")]
		[DisplayName("Columns")]
		[Browsable(false)]
		[MergableProperty(false)]
		public TableLayoutColumnStyleCollection ColumnStyles
		{
			get
			{
				return this._tableLayoutSettings.ColumnStyles;
			}
		}

		/// <summary>Creates a new instance of the control collection for the control.</summary>
		/// <returns>A new instance of <see cref="T:System.Windows.Forms.Control.ControlCollection" /> assigned to the control.</returns>
		// Token: 0x0600385C RID: 14428 RVA: 0x000FCF72 File Offset: 0x000FB172
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override Control.ControlCollection CreateControlsInstance()
		{
			return new TableLayoutControlCollection(this);
		}

		// Token: 0x0600385D RID: 14429 RVA: 0x000FCF7C File Offset: 0x000FB17C
		private bool ShouldSerializeControls()
		{
			TableLayoutControlCollection controls = this.Controls;
			return controls != null && controls.Count > 0;
		}

		/// <summary>For a description of this member, see <see cref="M:System.ComponentModel.IExtenderProvider.CanExtend(System.Object)" />.</summary>
		/// <param name="obj">The <see cref="T:System.Object" /> to receive the extender properties.</param>
		/// <returns>
		///     <see langword="true" /> if this object can provide extender properties to the specified object; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600385E RID: 14430 RVA: 0x000FCFA0 File Offset: 0x000FB1A0
		bool IExtenderProvider.CanExtend(object obj)
		{
			Control control = obj as Control;
			return control != null && control.Parent == this;
		}

		/// <summary>Returns the number of columns spanned by the specified child control.</summary>
		/// <param name="control">A child control of the <see cref="T:System.Windows.Forms.TableLayoutPanel" />.</param>
		/// <returns>The number of columns spanned by the child control. The default is 1.</returns>
		// Token: 0x0600385F RID: 14431 RVA: 0x000FCFC2 File Offset: 0x000FB1C2
		[SRDescription("GridPanelGetColumnSpanDescr")]
		[DefaultValue(1)]
		[SRCategory("CatLayout")]
		[DisplayName("ColumnSpan")]
		public int GetColumnSpan(Control control)
		{
			return this._tableLayoutSettings.GetColumnSpan(control);
		}

		/// <summary>Sets the number of columns spanned by the child control.</summary>
		/// <param name="control">A child control of the <see cref="T:System.Windows.Forms.TableLayoutPanel" />.</param>
		/// <param name="value">The number of columns to span.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="value" /> is less than 1.</exception>
		// Token: 0x06003860 RID: 14432 RVA: 0x000FCFD0 File Offset: 0x000FB1D0
		public void SetColumnSpan(Control control, int value)
		{
			this._tableLayoutSettings.SetColumnSpan(control, value);
		}

		/// <summary>Returns the number of rows spanned by the specified child control.</summary>
		/// <param name="control">A child control of the <see cref="T:System.Windows.Forms.TableLayoutPanel" />.</param>
		/// <returns>The number of rows spanned by the child control. The default is 1.</returns>
		// Token: 0x06003861 RID: 14433 RVA: 0x000FCFDF File Offset: 0x000FB1DF
		[SRDescription("GridPanelGetRowSpanDescr")]
		[DefaultValue(1)]
		[SRCategory("CatLayout")]
		[DisplayName("RowSpan")]
		public int GetRowSpan(Control control)
		{
			return this._tableLayoutSettings.GetRowSpan(control);
		}

		/// <summary>Sets the number of rows spanned by the child control.</summary>
		/// <param name="control">A child control of the <see cref="T:System.Windows.Forms.TableLayoutPanel" />.</param>
		/// <param name="value">The number of rows to span.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="value" /> is less than 1.</exception>
		// Token: 0x06003862 RID: 14434 RVA: 0x000FCFED File Offset: 0x000FB1ED
		public void SetRowSpan(Control control, int value)
		{
			this._tableLayoutSettings.SetRowSpan(control, value);
		}

		/// <summary>Returns the row position of the specified child control.</summary>
		/// <param name="control">A child control of the <see cref="T:System.Windows.Forms.TableLayoutPanel" />.</param>
		/// <returns>The row position of <paramref name="control" />, or -1 if the position of <paramref name="control" /> is determined by <see cref="P:System.Windows.Forms.TableLayoutPanel.LayoutEngine" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="control" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///         <paramref name="control" /> is not a type that can be arranged by this <see cref="T:System.Windows.Forms.Layout.LayoutEngine" />.</exception>
		// Token: 0x06003863 RID: 14435 RVA: 0x000FCFFC File Offset: 0x000FB1FC
		[DefaultValue(-1)]
		[SRDescription("GridPanelRowDescr")]
		[SRCategory("CatLayout")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DisplayName("Row")]
		public int GetRow(Control control)
		{
			return this._tableLayoutSettings.GetRow(control);
		}

		/// <summary>Sets the row position of the specified child control.</summary>
		/// <param name="control">The control to move to another row.</param>
		/// <param name="row">The row to which <paramref name="control" /> will be moved.</param>
		// Token: 0x06003864 RID: 14436 RVA: 0x000FD00A File Offset: 0x000FB20A
		public void SetRow(Control control, int row)
		{
			this._tableLayoutSettings.SetRow(control, row);
		}

		/// <summary>Gets the <see cref="T:System.Windows.Forms.TableLayoutPanelCellPosition" /> that represents the row and the column of the cell.</summary>
		/// <param name="control">A control contained within a cell.</param>
		/// <returns>A <see cref="T:System.Windows.Forms.TableLayoutPanelCellPosition" /> that represents the cell position.</returns>
		// Token: 0x06003865 RID: 14437 RVA: 0x000FD019 File Offset: 0x000FB219
		[DefaultValue(typeof(TableLayoutPanelCellPosition), "-1,-1")]
		[SRDescription("GridPanelCellPositionDescr")]
		[SRCategory("CatLayout")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DisplayName("Cell")]
		public TableLayoutPanelCellPosition GetCellPosition(Control control)
		{
			return this._tableLayoutSettings.GetCellPosition(control);
		}

		/// <summary>Sets the <see cref="T:System.Windows.Forms.TableLayoutPanelCellPosition" /> that represents the row and the column of the cell.</summary>
		/// <param name="control">A control contained within a cell.</param>
		/// <param name="position">A <see cref="T:System.Windows.Forms.TableLayoutPanelCellPosition" /> that represents the row and the column of the cell.</param>
		// Token: 0x06003866 RID: 14438 RVA: 0x000FD027 File Offset: 0x000FB227
		public void SetCellPosition(Control control, TableLayoutPanelCellPosition position)
		{
			this._tableLayoutSettings.SetCellPosition(control, position);
		}

		/// <summary>Returns the column position of the specified child control.</summary>
		/// <param name="control">A child control of the <see cref="T:System.Windows.Forms.TableLayoutPanel" />.</param>
		/// <returns>The column position of the specified child control, or -1 if the position of <paramref name="control" /> is determined by <see cref="P:System.Windows.Forms.TableLayoutPanel.LayoutEngine" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="control" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///         <paramref name="control" /> is not a type that can be arranged by this <see cref="T:System.Windows.Forms.Layout.LayoutEngine" />.</exception>
		// Token: 0x06003867 RID: 14439 RVA: 0x000FD036 File Offset: 0x000FB236
		[DefaultValue(-1)]
		[SRDescription("GridPanelColumnDescr")]
		[SRCategory("CatLayout")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DisplayName("Column")]
		public int GetColumn(Control control)
		{
			return this._tableLayoutSettings.GetColumn(control);
		}

		/// <summary>Sets the column position of the specified child control.</summary>
		/// <param name="control">The control to move to another column.</param>
		/// <param name="column">The column to which <paramref name="control" /> will be moved.</param>
		// Token: 0x06003868 RID: 14440 RVA: 0x000FD044 File Offset: 0x000FB244
		public void SetColumn(Control control, int column)
		{
			this._tableLayoutSettings.SetColumn(control, column);
		}

		/// <summary>Returns the child control occupying the specified position.</summary>
		/// <param name="column">The column position of the control to retrieve.</param>
		/// <param name="row">The row position of the control to retrieve.</param>
		/// <returns>The child control occupying the specified cell; otherwise, <see langword="null" /> if no control exists at the specified column and row, or if the control has its <see cref="P:System.Windows.Forms.Control.Visible" /> property set to <see langword="false" />.</returns>
		/// <exception cref="T:System.ArgumentException">Either <paramref name="column" /> or <paramref name="row" /> (or both) is less than 0.</exception>
		// Token: 0x06003869 RID: 14441 RVA: 0x000FD053 File Offset: 0x000FB253
		public Control GetControlFromPosition(int column, int row)
		{
			return (Control)this._tableLayoutSettings.GetControlFromPosition(column, row);
		}

		/// <summary>Gets the <see cref="T:System.Windows.Forms.TableLayoutPanelCellPosition" /> that represents the row and the column of the cell that contains the control.</summary>
		/// <param name="control">A control contained within a cell.</param>
		/// <returns>A <see cref="T:System.Windows.Forms.TableLayoutPanelCellPosition" /> that represents the cell position.</returns>
		// Token: 0x0600386A RID: 14442 RVA: 0x000FD067 File Offset: 0x000FB267
		public TableLayoutPanelCellPosition GetPositionFromControl(Control control)
		{
			return this._tableLayoutSettings.GetPositionFromControl(control);
		}

		/// <summary>Returns an array representing the widths, in pixels, of the columns in the <see cref="T:System.Windows.Forms.TableLayoutPanel" />.</summary>
		/// <returns>An array of type <see cref="T:System.Int32" /> that contains the widths, in pixels, of the columns in the <see cref="T:System.Windows.Forms.TableLayoutPanel" />.</returns>
		// Token: 0x0600386B RID: 14443 RVA: 0x000FD078 File Offset: 0x000FB278
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int[] GetColumnWidths()
		{
			TableLayout.ContainerInfo containerInfo = TableLayout.GetContainerInfo(this);
			if (containerInfo.Columns == null)
			{
				return new int[0];
			}
			int[] array = new int[containerInfo.Columns.Length];
			for (int i = 0; i < containerInfo.Columns.Length; i++)
			{
				array[i] = containerInfo.Columns[i].MinSize;
			}
			return array;
		}

		/// <summary>Returns an array representing the heights, in pixels, of the rows in the <see cref="T:System.Windows.Forms.TableLayoutPanel" />.</summary>
		/// <returns>An array of type <see cref="T:System.Int32" /> that contains the heights, in pixels, of the rows in the <see cref="T:System.Windows.Forms.TableLayoutPanel" />.</returns>
		// Token: 0x0600386C RID: 14444 RVA: 0x000FD0D4 File Offset: 0x000FB2D4
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int[] GetRowHeights()
		{
			TableLayout.ContainerInfo containerInfo = TableLayout.GetContainerInfo(this);
			if (containerInfo.Rows == null)
			{
				return new int[0];
			}
			int[] array = new int[containerInfo.Rows.Length];
			for (int i = 0; i < containerInfo.Rows.Length; i++)
			{
				array[i] = containerInfo.Rows[i].MinSize;
			}
			return array;
		}

		/// <summary>Occurs when the cell is redrawn.</summary>
		// Token: 0x140002C1 RID: 705
		// (add) Token: 0x0600386D RID: 14445 RVA: 0x000FD12D File Offset: 0x000FB32D
		// (remove) Token: 0x0600386E RID: 14446 RVA: 0x000FD140 File Offset: 0x000FB340
		[SRCategory("CatAppearance")]
		[SRDescription("TableLayoutPanelOnPaintCellDescr")]
		public event TableLayoutCellPaintEventHandler CellPaint
		{
			add
			{
				base.Events.AddHandler(TableLayoutPanel.EventCellPaint, value);
			}
			remove
			{
				base.Events.RemoveHandler(TableLayoutPanel.EventCellPaint, value);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.Layout" /> event.</summary>
		/// <param name="levent">A <see cref="T:System.Windows.Forms.LayoutEventArgs" /> that contains the event data.</param>
		// Token: 0x0600386F RID: 14447 RVA: 0x000FD153 File Offset: 0x000FB353
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void OnLayout(LayoutEventArgs levent)
		{
			base.OnLayout(levent);
			base.Invalidate();
		}

		/// <summary>Receives a call when the cell should be refreshed.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.TableLayoutCellPaintEventArgs" /> that provides data for the event.</param>
		// Token: 0x06003870 RID: 14448 RVA: 0x000FD164 File Offset: 0x000FB364
		protected virtual void OnCellPaint(TableLayoutCellPaintEventArgs e)
		{
			TableLayoutCellPaintEventHandler tableLayoutCellPaintEventHandler = (TableLayoutCellPaintEventHandler)base.Events[TableLayoutPanel.EventCellPaint];
			if (tableLayoutCellPaintEventHandler != null)
			{
				tableLayoutCellPaintEventHandler(this, e);
			}
		}

		/// <summary>Paints the background of the panel.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs" />  that contains information about the panel to paint.</param>
		// Token: 0x06003871 RID: 14449 RVA: 0x000FD194 File Offset: 0x000FB394
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			base.OnPaintBackground(e);
			int cellBorderWidth = this.CellBorderWidth;
			TableLayout.ContainerInfo containerInfo = TableLayout.GetContainerInfo(this);
			TableLayout.Strip[] columns = containerInfo.Columns;
			TableLayout.Strip[] rows = containerInfo.Rows;
			TableLayoutPanelCellBorderStyle cellBorderStyle = this.CellBorderStyle;
			if (columns == null || rows == null)
			{
				return;
			}
			int num = columns.Length;
			int num2 = rows.Length;
			int num3 = 0;
			int num4 = 0;
			Graphics graphics = e.Graphics;
			Rectangle displayRectangle = this.DisplayRectangle;
			Rectangle clipRectangle = e.ClipRectangle;
			bool flag = this.RightToLeft == RightToLeft.Yes;
			int num5;
			if (flag)
			{
				num5 = displayRectangle.Right - cellBorderWidth / 2;
			}
			else
			{
				num5 = displayRectangle.X + cellBorderWidth / 2;
			}
			for (int i = 0; i < num; i++)
			{
				int num6 = displayRectangle.Y + cellBorderWidth / 2;
				if (flag)
				{
					num5 -= columns[i].MinSize;
				}
				for (int j = 0; j < num2; j++)
				{
					int x = num5;
					int y = num6;
					TableLayout.Strip strip = columns[i];
					int minSize = strip.MinSize;
					strip = rows[j];
					Rectangle bound = new Rectangle(x, y, minSize, strip.MinSize);
					Rectangle rectangle = new Rectangle(bound.X + (cellBorderWidth + 1) / 2, bound.Y + (cellBorderWidth + 1) / 2, bound.Width - (cellBorderWidth + 1) / 2, bound.Height - (cellBorderWidth + 1) / 2);
					if (clipRectangle.IntersectsWith(rectangle))
					{
						using (TableLayoutCellPaintEventArgs tableLayoutCellPaintEventArgs = new TableLayoutCellPaintEventArgs(graphics, clipRectangle, rectangle, i, j))
						{
							this.OnCellPaint(tableLayoutCellPaintEventArgs);
						}
						ControlPaint.PaintTableCellBorder(cellBorderStyle, graphics, bound);
					}
					num6 += rows[j].MinSize;
					if (i == 0)
					{
						num4 += rows[j].MinSize;
					}
				}
				if (!flag)
				{
					num5 += columns[i].MinSize;
				}
				num3 += columns[i].MinSize;
			}
			if (!base.HScroll && !base.VScroll && cellBorderStyle != TableLayoutPanelCellBorderStyle.None)
			{
				Rectangle bound2 = new Rectangle(cellBorderWidth / 2 + displayRectangle.X, cellBorderWidth / 2 + displayRectangle.Y, displayRectangle.Width - cellBorderWidth, displayRectangle.Height - cellBorderWidth);
				if (cellBorderStyle == TableLayoutPanelCellBorderStyle.Inset)
				{
					graphics.DrawLine(SystemPens.ControlDark, bound2.Right, bound2.Y, bound2.Right, bound2.Bottom);
					graphics.DrawLine(SystemPens.ControlDark, bound2.X, bound2.Y + bound2.Height - 1, bound2.X + bound2.Width - 1, bound2.Y + bound2.Height - 1);
				}
				else
				{
					if (cellBorderStyle == TableLayoutPanelCellBorderStyle.Outset)
					{
						using (Pen pen = new Pen(SystemColors.Window))
						{
							graphics.DrawLine(pen, bound2.X + bound2.Width - 1, bound2.Y, bound2.X + bound2.Width - 1, bound2.Y + bound2.Height - 1);
							graphics.DrawLine(pen, bound2.X, bound2.Y + bound2.Height - 1, bound2.X + bound2.Width - 1, bound2.Y + bound2.Height - 1);
							goto IL_342;
						}
					}
					ControlPaint.PaintTableCellBorder(cellBorderStyle, graphics, bound2);
				}
				IL_342:
				ControlPaint.PaintTableControlBorder(cellBorderStyle, graphics, displayRectangle);
				return;
			}
			ControlPaint.PaintTableControlBorder(cellBorderStyle, graphics, displayRectangle);
		}

		/// <summary>Performs the work of scaling the entire panel and any child controls.</summary>
		/// <param name="dx">The ratio by which to scale the control horizontally.</param>
		/// <param name="dy">The ratio by which to scale the control vertically</param>
		// Token: 0x06003872 RID: 14450 RVA: 0x000FD518 File Offset: 0x000FB718
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void ScaleCore(float dx, float dy)
		{
			base.ScaleCore(dx, dy);
			this.ScaleAbsoluteStyles(new SizeF(dx, dy));
		}

		/// <summary>Scales a control's location, size, padding and margin.</summary>
		/// <param name="factor">The height and width of the control's bounds.</param>
		/// <param name="specified">One of the values of <see cref="T:System.Windows.Forms.BoundsSpecified" />  that specifies the bounds of the control to use when defining its size and position.</param>
		// Token: 0x06003873 RID: 14451 RVA: 0x000FD52F File Offset: 0x000FB72F
		protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
		{
			base.ScaleControl(factor, specified);
			this.ScaleAbsoluteStyles(factor);
		}

		// Token: 0x06003874 RID: 14452 RVA: 0x000FD540 File Offset: 0x000FB740
		private void ScaleAbsoluteStyles(SizeF factor)
		{
			TableLayout.ContainerInfo containerInfo = TableLayout.GetContainerInfo(this);
			int num = 0;
			int num2 = -1;
			int num3 = containerInfo.Rows.Length - 1;
			if (containerInfo.Rows.Length != 0)
			{
				num2 = containerInfo.Rows[num3].MinSize;
			}
			int num4 = -1;
			int num5 = containerInfo.Columns.Length - 1;
			if (containerInfo.Columns.Length != 0)
			{
				num4 = containerInfo.Columns[containerInfo.Columns.Length - 1].MinSize;
			}
			foreach (object obj in ((IEnumerable)this.ColumnStyles))
			{
				ColumnStyle columnStyle = (ColumnStyle)obj;
				if (columnStyle.SizeType == SizeType.Absolute)
				{
					if (num == num5 && num4 > 0)
					{
						columnStyle.Width = (float)Math.Round((double)((float)num4 * factor.Width));
					}
					else
					{
						columnStyle.Width = (float)Math.Round((double)(columnStyle.Width * factor.Width));
					}
				}
				num++;
			}
			num = 0;
			foreach (object obj2 in ((IEnumerable)this.RowStyles))
			{
				RowStyle rowStyle = (RowStyle)obj2;
				if (rowStyle.SizeType == SizeType.Absolute)
				{
					if (num == num3 && num2 > 0)
					{
						rowStyle.Height = (float)Math.Round((double)((float)num2 * factor.Height));
					}
					else
					{
						rowStyle.Height = (float)Math.Round((double)(rowStyle.Height * factor.Height));
					}
				}
			}
		}

		// Token: 0x0400225C RID: 8796
		private TableLayoutSettings _tableLayoutSettings;

		// Token: 0x0400225D RID: 8797
		private static readonly object EventCellPaint = new object();
	}
}
