using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	/// <summary>Collects the characteristics associated with table layouts.</summary>
	// Token: 0x02000382 RID: 898
	[TypeConverter(typeof(TableLayoutSettingsTypeConverter))]
	[Serializable]
	public sealed class TableLayoutSettings : LayoutSettings, ISerializable
	{
		// Token: 0x0600388C RID: 14476 RVA: 0x000FDA8E File Offset: 0x000FBC8E
		internal TableLayoutSettings() : base(null)
		{
			this._stub = new TableLayoutSettings.TableLayoutSettingsStub();
		}

		// Token: 0x0600388D RID: 14477 RVA: 0x000A7773 File Offset: 0x000A5973
		internal TableLayoutSettings(IArrangedElement owner) : base(owner)
		{
		}

		// Token: 0x0600388E RID: 14478 RVA: 0x000FDAA4 File Offset: 0x000FBCA4
		internal TableLayoutSettings(SerializationInfo serializationInfo, StreamingContext context) : this()
		{
			TypeConverter converter = TypeDescriptor.GetConverter(this);
			string @string = serializationInfo.GetString("SerializedString");
			if (!string.IsNullOrEmpty(@string) && converter != null)
			{
				TableLayoutSettings tableLayoutSettings = converter.ConvertFromInvariantString(@string) as TableLayoutSettings;
				if (tableLayoutSettings != null)
				{
					this.ApplySettings(tableLayoutSettings);
				}
			}
		}

		/// <summary>Gets the current table layout engine.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.Layout.LayoutEngine" /> currently being used. </returns>
		// Token: 0x17000E1A RID: 3610
		// (get) Token: 0x0600388F RID: 14479 RVA: 0x000FCE51 File Offset: 0x000FB051
		public override LayoutEngine LayoutEngine
		{
			get
			{
				return TableLayout.Instance;
			}
		}

		// Token: 0x17000E1B RID: 3611
		// (get) Token: 0x06003890 RID: 14480 RVA: 0x000FDAEC File Offset: 0x000FBCEC
		private TableLayout TableLayout
		{
			get
			{
				return (TableLayout)this.LayoutEngine;
			}
		}

		// Token: 0x17000E1C RID: 3612
		// (get) Token: 0x06003891 RID: 14481 RVA: 0x000FDAF9 File Offset: 0x000FBCF9
		// (set) Token: 0x06003892 RID: 14482 RVA: 0x000FDB04 File Offset: 0x000FBD04
		[DefaultValue(TableLayoutPanelCellBorderStyle.None)]
		[SRCategory("CatAppearance")]
		[SRDescription("TableLayoutPanelCellBorderStyleDescr")]
		internal TableLayoutPanelCellBorderStyle CellBorderStyle
		{
			get
			{
				return this._borderStyle;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 6))
				{
					throw new ArgumentException(SR.GetString("InvalidArgument", new object[]
					{
						"CellBorderStyle",
						value.ToString()
					}));
				}
				this._borderStyle = value;
				TableLayout.ContainerInfo containerInfo = TableLayout.GetContainerInfo(base.Owner);
				containerInfo.CellBorderWidth = TableLayoutSettings.borderStyleToOffset[(int)value];
				LayoutTransaction.DoLayout(base.Owner, base.Owner, PropertyNames.CellBorderStyle);
			}
		}

		// Token: 0x17000E1D RID: 3613
		// (get) Token: 0x06003893 RID: 14483 RVA: 0x000FDB85 File Offset: 0x000FBD85
		[DefaultValue(0)]
		internal int CellBorderWidth
		{
			get
			{
				return TableLayout.GetContainerInfo(base.Owner).CellBorderWidth;
			}
		}

		/// <summary>Gets or sets the maximum number of columns allowed in the table layout.</summary>
		/// <returns>The maximum number of columns allowed in the table layout. The default is 0.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The property value is less than 0.</exception>
		// Token: 0x17000E1E RID: 3614
		// (get) Token: 0x06003894 RID: 14484 RVA: 0x000FDB98 File Offset: 0x000FBD98
		// (set) Token: 0x06003895 RID: 14485 RVA: 0x000FDBB8 File Offset: 0x000FBDB8
		[SRDescription("GridPanelColumnsDescr")]
		[SRCategory("CatLayout")]
		[DefaultValue(0)]
		public int ColumnCount
		{
			get
			{
				TableLayout.ContainerInfo containerInfo = TableLayout.GetContainerInfo(base.Owner);
				return containerInfo.MaxColumns;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("ColumnCount", value, SR.GetString("InvalidLowBoundArgumentEx", new object[]
					{
						"ColumnCount",
						value.ToString(CultureInfo.CurrentCulture),
						0.ToString(CultureInfo.CurrentCulture)
					}));
				}
				TableLayout.ContainerInfo containerInfo = TableLayout.GetContainerInfo(base.Owner);
				containerInfo.MaxColumns = value;
				LayoutTransaction.DoLayout(base.Owner, base.Owner, PropertyNames.Columns);
			}
		}

		/// <summary>Gets or sets the maximum number of rows allowed in the table layout.</summary>
		/// <returns>The maximum number of rows allowed in the table layout. The default is 0.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The property value is less than 0.</exception>
		// Token: 0x17000E1F RID: 3615
		// (get) Token: 0x06003896 RID: 14486 RVA: 0x000FDC3C File Offset: 0x000FBE3C
		// (set) Token: 0x06003897 RID: 14487 RVA: 0x000FDC5C File Offset: 0x000FBE5C
		[SRDescription("GridPanelRowsDescr")]
		[SRCategory("CatLayout")]
		[DefaultValue(0)]
		public int RowCount
		{
			get
			{
				TableLayout.ContainerInfo containerInfo = TableLayout.GetContainerInfo(base.Owner);
				return containerInfo.MaxRows;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("RowCount", value, SR.GetString("InvalidLowBoundArgumentEx", new object[]
					{
						"RowCount",
						value.ToString(CultureInfo.CurrentCulture),
						0.ToString(CultureInfo.CurrentCulture)
					}));
				}
				TableLayout.ContainerInfo containerInfo = TableLayout.GetContainerInfo(base.Owner);
				containerInfo.MaxRows = value;
				LayoutTransaction.DoLayout(base.Owner, base.Owner, PropertyNames.Rows);
			}
		}

		/// <summary>Gets the collection of styles used to determine the look and feel of the table layout rows.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.TableLayoutRowStyleCollection" /> that contains the row styles for the layout table.</returns>
		// Token: 0x17000E20 RID: 3616
		// (get) Token: 0x06003898 RID: 14488 RVA: 0x000FDCE0 File Offset: 0x000FBEE0
		[SRDescription("GridPanelRowStylesDescr")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[SRCategory("CatLayout")]
		public TableLayoutRowStyleCollection RowStyles
		{
			get
			{
				if (this.IsStub)
				{
					return this._stub.RowStyles;
				}
				TableLayout.ContainerInfo containerInfo = TableLayout.GetContainerInfo(base.Owner);
				return containerInfo.RowStyles;
			}
		}

		/// <summary>Gets the collection of styles used to determine the look and feel of the table layout columns. </summary>
		/// <returns>A <see cref="T:System.Windows.Forms.TableLayoutColumnStyleCollection" /> that contains the column styles for the layout table. </returns>
		// Token: 0x17000E21 RID: 3617
		// (get) Token: 0x06003899 RID: 14489 RVA: 0x000FDD14 File Offset: 0x000FBF14
		[SRDescription("GridPanelColumnStylesDescr")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[SRCategory("CatLayout")]
		public TableLayoutColumnStyleCollection ColumnStyles
		{
			get
			{
				if (this.IsStub)
				{
					return this._stub.ColumnStyles;
				}
				TableLayout.ContainerInfo containerInfo = TableLayout.GetContainerInfo(base.Owner);
				return containerInfo.ColumnStyles;
			}
		}

		/// <summary>Gets or sets a value indicating how the table layout should expand to accommodate new cells when all existing cells are occupied.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.TableLayoutPanelGrowStyle" /> values. The default is <see cref="F:System.Windows.Forms.TableLayoutPanelGrowStyle.AddRows" />.</returns>
		/// <exception cref="T:System.ArgumentException">The property value is not valid for the enumeration type.</exception>
		// Token: 0x17000E22 RID: 3618
		// (get) Token: 0x0600389A RID: 14490 RVA: 0x000FDD47 File Offset: 0x000FBF47
		// (set) Token: 0x0600389B RID: 14491 RVA: 0x000FDD5C File Offset: 0x000FBF5C
		[SRDescription("TableLayoutPanelGrowStyleDescr")]
		[SRCategory("CatLayout")]
		[DefaultValue(TableLayoutPanelGrowStyle.AddRows)]
		public TableLayoutPanelGrowStyle GrowStyle
		{
			get
			{
				return TableLayout.GetContainerInfo(base.Owner).GrowStyle;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
				{
					throw new ArgumentException(SR.GetString("InvalidArgument", new object[]
					{
						"GrowStyle",
						value.ToString()
					}));
				}
				TableLayout.ContainerInfo containerInfo = TableLayout.GetContainerInfo(base.Owner);
				if (containerInfo.GrowStyle != value)
				{
					containerInfo.GrowStyle = value;
					LayoutTransaction.DoLayout(base.Owner, base.Owner, PropertyNames.GrowStyle);
				}
			}
		}

		// Token: 0x17000E23 RID: 3619
		// (get) Token: 0x0600389C RID: 14492 RVA: 0x000FDDD9 File Offset: 0x000FBFD9
		internal bool IsStub
		{
			get
			{
				return this._stub != null;
			}
		}

		// Token: 0x0600389D RID: 14493 RVA: 0x000FDDE6 File Offset: 0x000FBFE6
		internal void ApplySettings(TableLayoutSettings settings)
		{
			if (settings.IsStub)
			{
				if (!this.IsStub)
				{
					settings._stub.ApplySettings(this);
					return;
				}
				this._stub = settings._stub;
			}
		}

		/// <summary>Gets the number of columns that the cell containing the child control spans.</summary>
		/// <param name="control">A control contained within a cell.</param>
		/// <returns>The number of columns that the cell containing the child control spans.</returns>
		// Token: 0x0600389E RID: 14494 RVA: 0x000FDE14 File Offset: 0x000FC014
		public int GetColumnSpan(object control)
		{
			if (control == null)
			{
				throw new ArgumentNullException("control");
			}
			if (this.IsStub)
			{
				return this._stub.GetColumnSpan(control);
			}
			IArrangedElement element = this.LayoutEngine.CastToArrangedElement(control);
			return TableLayout.GetLayoutInfo(element).ColumnSpan;
		}

		/// <summary>Sets the number of columns that the cell containing the child control spans.</summary>
		/// <param name="control">A control contained within a cell.</param>
		/// <param name="value">The number of columns that the cell containing the child control spans.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="control" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="value" /> is less than 1.</exception>
		// Token: 0x0600389F RID: 14495 RVA: 0x000FDE5C File Offset: 0x000FC05C
		public void SetColumnSpan(object control, int value)
		{
			if (value < 1)
			{
				throw new ArgumentOutOfRangeException("ColumnSpan", SR.GetString("InvalidArgument", new object[]
				{
					"ColumnSpan",
					value.ToString(CultureInfo.CurrentCulture)
				}));
			}
			if (this.IsStub)
			{
				this._stub.SetColumnSpan(control, value);
				return;
			}
			IArrangedElement arrangedElement = this.LayoutEngine.CastToArrangedElement(control);
			if (arrangedElement.Container != null)
			{
				TableLayout.ClearCachedAssignments(TableLayout.GetContainerInfo(arrangedElement.Container));
			}
			TableLayout.GetLayoutInfo(arrangedElement).ColumnSpan = value;
			LayoutTransaction.DoLayout(arrangedElement.Container, arrangedElement, PropertyNames.ColumnSpan);
		}

		/// <summary>Gets the number of rows that the cell containing the child control spans.</summary>
		/// <param name="control">A control contained within a cell.</param>
		/// <returns>The number of rows that the cell containing the child control spans.</returns>
		// Token: 0x060038A0 RID: 14496 RVA: 0x000FDEF8 File Offset: 0x000FC0F8
		public int GetRowSpan(object control)
		{
			if (this.IsStub)
			{
				return this._stub.GetRowSpan(control);
			}
			IArrangedElement element = this.LayoutEngine.CastToArrangedElement(control);
			return TableLayout.GetLayoutInfo(element).RowSpan;
		}

		/// <summary>Sets the number of rows that the cell containing the child control spans.</summary>
		/// <param name="control">A control contained within a cell.</param>
		/// <param name="value">The number of rows that the cell containing the child control spans.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="control" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="value" /> is less than 1.</exception>
		// Token: 0x060038A1 RID: 14497 RVA: 0x000FDF34 File Offset: 0x000FC134
		public void SetRowSpan(object control, int value)
		{
			if (value < 1)
			{
				throw new ArgumentOutOfRangeException("RowSpan", SR.GetString("InvalidArgument", new object[]
				{
					"RowSpan",
					value.ToString(CultureInfo.CurrentCulture)
				}));
			}
			if (control == null)
			{
				throw new ArgumentNullException("control");
			}
			if (this.IsStub)
			{
				this._stub.SetRowSpan(control, value);
				return;
			}
			IArrangedElement arrangedElement = this.LayoutEngine.CastToArrangedElement(control);
			if (arrangedElement.Container != null)
			{
				TableLayout.ClearCachedAssignments(TableLayout.GetContainerInfo(arrangedElement.Container));
			}
			TableLayout.GetLayoutInfo(arrangedElement).RowSpan = value;
			LayoutTransaction.DoLayout(arrangedElement.Container, arrangedElement, PropertyNames.RowSpan);
		}

		/// <summary>Gets the row position of the specified child control.</summary>
		/// <param name="control">A control contained within a cell.</param>
		/// <returns>The row position of the specified child control.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="control" /> is <see langword="null" />.</exception>
		// Token: 0x060038A2 RID: 14498 RVA: 0x000FDFE0 File Offset: 0x000FC1E0
		[SRDescription("GridPanelRowDescr")]
		[SRCategory("CatLayout")]
		[DefaultValue(-1)]
		public int GetRow(object control)
		{
			if (control == null)
			{
				throw new ArgumentNullException("control");
			}
			if (this.IsStub)
			{
				return this._stub.GetRow(control);
			}
			IArrangedElement element = this.LayoutEngine.CastToArrangedElement(control);
			TableLayout.LayoutInfo layoutInfo = TableLayout.GetLayoutInfo(element);
			return layoutInfo.RowPosition;
		}

		/// <summary>Sets the row position of the specified child control.</summary>
		/// <param name="control">A control contained within a cell.</param>
		/// <param name="row">The row position of the specified child control.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="control" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="row" /> is less than -1.</exception>
		// Token: 0x060038A3 RID: 14499 RVA: 0x000FE02C File Offset: 0x000FC22C
		public void SetRow(object control, int row)
		{
			if (control == null)
			{
				throw new ArgumentNullException("control");
			}
			if (row < -1)
			{
				throw new ArgumentOutOfRangeException("Row", SR.GetString("InvalidArgument", new object[]
				{
					"Row",
					row.ToString(CultureInfo.CurrentCulture)
				}));
			}
			this.SetCellPosition(control, row, -1, true, false);
		}

		/// <summary>Gets the <see cref="T:System.Windows.Forms.TableLayoutPanelCellPosition" /> that represents the row and the column of the cell.</summary>
		/// <param name="control">A control contained within a cell.</param>
		/// <returns>A <see cref="T:System.Windows.Forms.TableLayoutPanelCellPosition" /> that represents the cell position.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="control" /> is <see langword="null" />.</exception>
		// Token: 0x060038A4 RID: 14500 RVA: 0x000FE088 File Offset: 0x000FC288
		[SRDescription("TableLayoutSettingsGetCellPositionDescr")]
		[SRCategory("CatLayout")]
		[DefaultValue(-1)]
		public TableLayoutPanelCellPosition GetCellPosition(object control)
		{
			if (control == null)
			{
				throw new ArgumentNullException("control");
			}
			return new TableLayoutPanelCellPosition(this.GetColumn(control), this.GetRow(control));
		}

		/// <summary>Sets the <see cref="T:System.Windows.Forms.TableLayoutPanelCellPosition" /> that represents the row and the column of the cell.</summary>
		/// <param name="control">A control contained within a cell.</param>
		/// <param name="cellPosition">A <see cref="T:System.Windows.Forms.TableLayoutPanelCellPosition" />  that represents the cell position.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="control" /> is <see langword="null" />.</exception>
		// Token: 0x060038A5 RID: 14501 RVA: 0x000FE0AB File Offset: 0x000FC2AB
		[SRDescription("TableLayoutSettingsSetCellPositionDescr")]
		[SRCategory("CatLayout")]
		[DefaultValue(-1)]
		public void SetCellPosition(object control, TableLayoutPanelCellPosition cellPosition)
		{
			if (control == null)
			{
				throw new ArgumentNullException("control");
			}
			this.SetCellPosition(control, cellPosition.Row, cellPosition.Column, true, true);
		}

		/// <summary>Gets the column position of the specified child control.</summary>
		/// <param name="control">A control contained within a cell.</param>
		/// <returns>The column position of the specified child control.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="control" /> is <see langword="null" />.</exception>
		// Token: 0x060038A6 RID: 14502 RVA: 0x000FE0D4 File Offset: 0x000FC2D4
		[SRDescription("GridPanelColumnDescr")]
		[SRCategory("CatLayout")]
		[DefaultValue(-1)]
		public int GetColumn(object control)
		{
			if (control == null)
			{
				throw new ArgumentNullException("control");
			}
			if (this.IsStub)
			{
				return this._stub.GetColumn(control);
			}
			IArrangedElement element = this.LayoutEngine.CastToArrangedElement(control);
			TableLayout.LayoutInfo layoutInfo = TableLayout.GetLayoutInfo(element);
			return layoutInfo.ColumnPosition;
		}

		/// <summary>Sets the column position for the specified child control.</summary>
		/// <param name="control">A control contained within a cell.</param>
		/// <param name="column">The column position for the specified child control.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="control" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="column" /> is less than -1.</exception>
		// Token: 0x060038A7 RID: 14503 RVA: 0x000FE120 File Offset: 0x000FC320
		public void SetColumn(object control, int column)
		{
			if (column < -1)
			{
				throw new ArgumentException(SR.GetString("InvalidArgument", new object[]
				{
					"Column",
					column.ToString(CultureInfo.CurrentCulture)
				}));
			}
			if (this.IsStub)
			{
				this._stub.SetColumn(control, column);
				return;
			}
			this.SetCellPosition(control, -1, column, false, true);
		}

		// Token: 0x060038A8 RID: 14504 RVA: 0x000FE180 File Offset: 0x000FC380
		private void SetCellPosition(object control, int row, int column, bool rowSpecified, bool colSpecified)
		{
			if (this.IsStub)
			{
				if (colSpecified)
				{
					this._stub.SetColumn(control, column);
				}
				if (rowSpecified)
				{
					this._stub.SetRow(control, row);
					return;
				}
			}
			else
			{
				IArrangedElement arrangedElement = this.LayoutEngine.CastToArrangedElement(control);
				if (arrangedElement.Container != null)
				{
					TableLayout.ClearCachedAssignments(TableLayout.GetContainerInfo(arrangedElement.Container));
				}
				TableLayout.LayoutInfo layoutInfo = TableLayout.GetLayoutInfo(arrangedElement);
				if (colSpecified)
				{
					layoutInfo.ColumnPosition = column;
				}
				if (rowSpecified)
				{
					layoutInfo.RowPosition = row;
				}
				LayoutTransaction.DoLayout(arrangedElement.Container, arrangedElement, PropertyNames.TableIndex);
			}
		}

		// Token: 0x060038A9 RID: 14505 RVA: 0x000FE20B File Offset: 0x000FC40B
		internal IArrangedElement GetControlFromPosition(int column, int row)
		{
			return this.TableLayout.GetControlFromPosition(base.Owner, column, row);
		}

		// Token: 0x060038AA RID: 14506 RVA: 0x000FE220 File Offset: 0x000FC420
		internal TableLayoutPanelCellPosition GetPositionFromControl(IArrangedElement element)
		{
			return this.TableLayout.GetPositionFromControl(base.Owner, element);
		}

		/// <summary>For a description of this member, see <see cref="M:System.Runtime.Serialization.ISerializable.GetObjectData(System.Runtime.Serialization.SerializationInfo,System.Runtime.Serialization.StreamingContext)" />.</summary>
		/// <param name="si">The object to be populated with serialization information. </param>
		/// <param name="context">The destination context of the serialization.</param>
		// Token: 0x060038AB RID: 14507 RVA: 0x000FE234 File Offset: 0x000FC434
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		void ISerializable.GetObjectData(SerializationInfo si, StreamingContext context)
		{
			TypeConverter converter = TypeDescriptor.GetConverter(this);
			string value = (converter != null) ? converter.ConvertToInvariantString(this) : null;
			if (!string.IsNullOrEmpty(value))
			{
				si.AddValue("SerializedString", value);
			}
		}

		// Token: 0x060038AC RID: 14508 RVA: 0x000FE26C File Offset: 0x000FC46C
		internal List<TableLayoutSettings.ControlInformation> GetControlsInformation()
		{
			if (this.IsStub)
			{
				return this._stub.GetControlsInformation();
			}
			List<TableLayoutSettings.ControlInformation> list = new List<TableLayoutSettings.ControlInformation>(base.Owner.Children.Count);
			foreach (object obj in base.Owner.Children)
			{
				IArrangedElement arrangedElement = (IArrangedElement)obj;
				Control control = arrangedElement as Control;
				if (control != null)
				{
					TableLayoutSettings.ControlInformation item = default(TableLayoutSettings.ControlInformation);
					PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(control)["Name"];
					if (propertyDescriptor != null && propertyDescriptor.PropertyType == typeof(string))
					{
						item.Name = propertyDescriptor.GetValue(control);
					}
					item.Row = this.GetRow(control);
					item.RowSpan = this.GetRowSpan(control);
					item.Column = this.GetColumn(control);
					item.ColumnSpan = this.GetColumnSpan(control);
					list.Add(item);
				}
			}
			return list;
		}

		// Token: 0x0400226D RID: 8813
		private static int[] borderStyleToOffset = new int[]
		{
			0,
			1,
			2,
			3,
			2,
			3,
			3
		};

		// Token: 0x0400226E RID: 8814
		private TableLayoutPanelCellBorderStyle _borderStyle;

		// Token: 0x0400226F RID: 8815
		private TableLayoutSettings.TableLayoutSettingsStub _stub;

		// Token: 0x02000724 RID: 1828
		internal struct ControlInformation
		{
			// Token: 0x06006088 RID: 24712 RVA: 0x0018B6B3 File Offset: 0x001898B3
			internal ControlInformation(object name, int row, int column, int rowSpan, int columnSpan)
			{
				this.Name = name;
				this.Row = row;
				this.Column = column;
				this.RowSpan = rowSpan;
				this.ColumnSpan = columnSpan;
			}

			// Token: 0x04004154 RID: 16724
			internal object Name;

			// Token: 0x04004155 RID: 16725
			internal int Row;

			// Token: 0x04004156 RID: 16726
			internal int Column;

			// Token: 0x04004157 RID: 16727
			internal int RowSpan;

			// Token: 0x04004158 RID: 16728
			internal int ColumnSpan;
		}

		// Token: 0x02000725 RID: 1829
		private class TableLayoutSettingsStub
		{
			// Token: 0x0600608A RID: 24714 RVA: 0x0018B6EC File Offset: 0x001898EC
			internal void ApplySettings(TableLayoutSettings settings)
			{
				TableLayout.ContainerInfo containerInfo = TableLayout.GetContainerInfo(settings.Owner);
				Control control = containerInfo.Container as Control;
				if (control != null && this.controlsInfo != null)
				{
					foreach (object obj in this.controlsInfo.Keys)
					{
						TableLayoutSettings.ControlInformation controlInformation = this.controlsInfo[obj];
						foreach (object obj2 in control.Controls)
						{
							Control control2 = (Control)obj2;
							if (control2 != null)
							{
								string @string = null;
								PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(control2)["Name"];
								if (propertyDescriptor != null && propertyDescriptor.PropertyType == typeof(string))
								{
									@string = (propertyDescriptor.GetValue(control2) as string);
								}
								if (WindowsFormsUtils.SafeCompareStrings(@string, obj as string, false))
								{
									settings.SetRow(control2, controlInformation.Row);
									settings.SetColumn(control2, controlInformation.Column);
									settings.SetRowSpan(control2, controlInformation.RowSpan);
									settings.SetColumnSpan(control2, controlInformation.ColumnSpan);
									break;
								}
							}
						}
					}
				}
				containerInfo.RowStyles = this.rowStyles;
				containerInfo.ColumnStyles = this.columnStyles;
				this.columnStyles = null;
				this.rowStyles = null;
				this.isValid = false;
			}

			// Token: 0x1700170E RID: 5902
			// (get) Token: 0x0600608B RID: 24715 RVA: 0x0018B8A8 File Offset: 0x00189AA8
			public TableLayoutColumnStyleCollection ColumnStyles
			{
				get
				{
					if (this.columnStyles == null)
					{
						this.columnStyles = new TableLayoutColumnStyleCollection();
					}
					return this.columnStyles;
				}
			}

			// Token: 0x1700170F RID: 5903
			// (get) Token: 0x0600608C RID: 24716 RVA: 0x0018B8C3 File Offset: 0x00189AC3
			public bool IsValid
			{
				get
				{
					return this.isValid;
				}
			}

			// Token: 0x17001710 RID: 5904
			// (get) Token: 0x0600608D RID: 24717 RVA: 0x0018B8CB File Offset: 0x00189ACB
			public TableLayoutRowStyleCollection RowStyles
			{
				get
				{
					if (this.rowStyles == null)
					{
						this.rowStyles = new TableLayoutRowStyleCollection();
					}
					return this.rowStyles;
				}
			}

			// Token: 0x0600608E RID: 24718 RVA: 0x0018B8E8 File Offset: 0x00189AE8
			internal List<TableLayoutSettings.ControlInformation> GetControlsInformation()
			{
				if (this.controlsInfo == null)
				{
					return new List<TableLayoutSettings.ControlInformation>();
				}
				List<TableLayoutSettings.ControlInformation> list = new List<TableLayoutSettings.ControlInformation>(this.controlsInfo.Count);
				foreach (object obj in this.controlsInfo.Keys)
				{
					TableLayoutSettings.ControlInformation item = this.controlsInfo[obj];
					item.Name = obj;
					list.Add(item);
				}
				return list;
			}

			// Token: 0x0600608F RID: 24719 RVA: 0x0018B978 File Offset: 0x00189B78
			private TableLayoutSettings.ControlInformation GetControlInformation(object controlName)
			{
				if (this.controlsInfo == null)
				{
					return TableLayoutSettings.TableLayoutSettingsStub.DefaultControlInfo;
				}
				if (!this.controlsInfo.ContainsKey(controlName))
				{
					return TableLayoutSettings.TableLayoutSettingsStub.DefaultControlInfo;
				}
				return this.controlsInfo[controlName];
			}

			// Token: 0x06006090 RID: 24720 RVA: 0x0018B9A8 File Offset: 0x00189BA8
			public int GetColumn(object controlName)
			{
				return this.GetControlInformation(controlName).Column;
			}

			// Token: 0x06006091 RID: 24721 RVA: 0x0018B9B6 File Offset: 0x00189BB6
			public int GetColumnSpan(object controlName)
			{
				return this.GetControlInformation(controlName).ColumnSpan;
			}

			// Token: 0x06006092 RID: 24722 RVA: 0x0018B9C4 File Offset: 0x00189BC4
			public int GetRow(object controlName)
			{
				return this.GetControlInformation(controlName).Row;
			}

			// Token: 0x06006093 RID: 24723 RVA: 0x0018B9D2 File Offset: 0x00189BD2
			public int GetRowSpan(object controlName)
			{
				return this.GetControlInformation(controlName).RowSpan;
			}

			// Token: 0x06006094 RID: 24724 RVA: 0x0018B9E0 File Offset: 0x00189BE0
			private void SetControlInformation(object controlName, TableLayoutSettings.ControlInformation info)
			{
				if (this.controlsInfo == null)
				{
					this.controlsInfo = new Dictionary<object, TableLayoutSettings.ControlInformation>();
				}
				this.controlsInfo[controlName] = info;
			}

			// Token: 0x06006095 RID: 24725 RVA: 0x0018BA04 File Offset: 0x00189C04
			public void SetColumn(object controlName, int column)
			{
				if (this.GetColumn(controlName) != column)
				{
					TableLayoutSettings.ControlInformation controlInformation = this.GetControlInformation(controlName);
					controlInformation.Column = column;
					this.SetControlInformation(controlName, controlInformation);
				}
			}

			// Token: 0x06006096 RID: 24726 RVA: 0x0018BA34 File Offset: 0x00189C34
			public void SetColumnSpan(object controlName, int value)
			{
				if (this.GetColumnSpan(controlName) != value)
				{
					TableLayoutSettings.ControlInformation controlInformation = this.GetControlInformation(controlName);
					controlInformation.ColumnSpan = value;
					this.SetControlInformation(controlName, controlInformation);
				}
			}

			// Token: 0x06006097 RID: 24727 RVA: 0x0018BA64 File Offset: 0x00189C64
			public void SetRow(object controlName, int row)
			{
				if (this.GetRow(controlName) != row)
				{
					TableLayoutSettings.ControlInformation controlInformation = this.GetControlInformation(controlName);
					controlInformation.Row = row;
					this.SetControlInformation(controlName, controlInformation);
				}
			}

			// Token: 0x06006098 RID: 24728 RVA: 0x0018BA94 File Offset: 0x00189C94
			public void SetRowSpan(object controlName, int value)
			{
				if (this.GetRowSpan(controlName) != value)
				{
					TableLayoutSettings.ControlInformation controlInformation = this.GetControlInformation(controlName);
					controlInformation.RowSpan = value;
					this.SetControlInformation(controlName, controlInformation);
				}
			}

			// Token: 0x04004159 RID: 16729
			private static TableLayoutSettings.ControlInformation DefaultControlInfo = new TableLayoutSettings.ControlInformation(null, -1, -1, 1, 1);

			// Token: 0x0400415A RID: 16730
			private TableLayoutColumnStyleCollection columnStyles;

			// Token: 0x0400415B RID: 16731
			private TableLayoutRowStyleCollection rowStyles;

			// Token: 0x0400415C RID: 16732
			private Dictionary<object, TableLayoutSettings.ControlInformation> controlsInfo;

			// Token: 0x0400415D RID: 16733
			private bool isValid = true;
		}

		// Token: 0x02000726 RID: 1830
		internal class StyleConverter : TypeConverter
		{
			// Token: 0x0600609A RID: 24730 RVA: 0x0001F8F0 File Offset: 0x0001DAF0
			public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
			{
				return destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
			}

			// Token: 0x0600609B RID: 24731 RVA: 0x0018BAD4 File Offset: 0x00189CD4
			public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
			{
				if (destinationType == null)
				{
					throw new ArgumentNullException("destinationType");
				}
				if (destinationType == typeof(InstanceDescriptor) && value is TableLayoutStyle)
				{
					TableLayoutStyle tableLayoutStyle = (TableLayoutStyle)value;
					SizeType sizeType = tableLayoutStyle.SizeType;
					if (sizeType == SizeType.AutoSize)
					{
						return new InstanceDescriptor(tableLayoutStyle.GetType().GetConstructor(new Type[0]), new object[0]);
					}
					if (sizeType - SizeType.Absolute <= 1)
					{
						return new InstanceDescriptor(tableLayoutStyle.GetType().GetConstructor(new Type[]
						{
							typeof(SizeType),
							typeof(int)
						}), new object[]
						{
							tableLayoutStyle.SizeType,
							tableLayoutStyle.Size
						});
					}
				}
				return base.ConvertTo(context, culture, value, destinationType);
			}
		}
	}
}
