using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	/// <summary>Specifies the appearance, text formatting, and behavior of a <see cref="T:System.Windows.Forms.DataGrid" /> control column. This class is abstract.</summary>
	// Token: 0x02000170 RID: 368
	[ToolboxItem(false)]
	[DesignTimeVisible(false)]
	[DefaultProperty("Header")]
	public abstract class DataGridColumnStyle : Component, IDataGridColumnStyleEditingNotificationService
	{
		/// <summary>In a derived class, initializes a new instance of the <see cref="T:System.Windows.Forms.DataGridColumnStyle" /> class.</summary>
		// Token: 0x06001305 RID: 4869 RVA: 0x0004858C File Offset: 0x0004678C
		public DataGridColumnStyle()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DataGridColumnStyle" /> class with the specified <see cref="T:System.ComponentModel.PropertyDescriptor" />.</summary>
		/// <param name="prop">A <see cref="T:System.ComponentModel.PropertyDescriptor" /> that provides the attributes for the column. </param>
		// Token: 0x06001306 RID: 4870 RVA: 0x000485C8 File Offset: 0x000467C8
		public DataGridColumnStyle(PropertyDescriptor prop) : this()
		{
			this.PropertyDescriptor = prop;
			if (prop != null)
			{
				this.readOnly = prop.IsReadOnly;
			}
		}

		// Token: 0x06001307 RID: 4871 RVA: 0x000485E6 File Offset: 0x000467E6
		internal DataGridColumnStyle(PropertyDescriptor prop, bool isDefault) : this(prop)
		{
			this.isDefault = isDefault;
			if (isDefault)
			{
				this.headerName = prop.Name;
				this.mappingName = prop.Name;
			}
		}

		/// <summary>Gets or sets the alignment of text in a column.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.HorizontalAlignment" /> values. The default is <see langword="Left" />. Valid options include <see langword="Left" />, <see langword="Center" />, and <see langword="Right" />.</returns>
		// Token: 0x1700049E RID: 1182
		// (get) Token: 0x06001308 RID: 4872 RVA: 0x00048611 File Offset: 0x00046811
		// (set) Token: 0x06001309 RID: 4873 RVA: 0x0004861C File Offset: 0x0004681C
		[SRCategory("CatDisplay")]
		[Localizable(true)]
		[DefaultValue(HorizontalAlignment.Left)]
		public virtual HorizontalAlignment Alignment
		{
			get
			{
				return this.alignment;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(DataGridLineStyle));
				}
				if (this.alignment != value)
				{
					this.alignment = value;
					this.OnAlignmentChanged(EventArgs.Empty);
					this.Invalidate();
				}
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.DataGridColumnStyle.Alignment" /> property value changes.</summary>
		// Token: 0x140000CC RID: 204
		// (add) Token: 0x0600130A RID: 4874 RVA: 0x00048670 File Offset: 0x00046870
		// (remove) Token: 0x0600130B RID: 4875 RVA: 0x00048683 File Offset: 0x00046883
		public event EventHandler AlignmentChanged
		{
			add
			{
				base.Events.AddHandler(DataGridColumnStyle.EventAlignment, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGridColumnStyle.EventAlignment, value);
			}
		}

		/// <summary>Updates the value of a specified row with the given text.</summary>
		/// <param name="source">The <see cref="T:System.Windows.Forms.CurrencyManager" /> associated with the <see cref="T:System.Windows.Forms.DataGridColumnStyle" />. </param>
		/// <param name="rowNum">The row to update. </param>
		/// <param name="displayText">The new value. </param>
		// Token: 0x0600130C RID: 4876 RVA: 0x0000701A File Offset: 0x0000521A
		protected internal virtual void UpdateUI(CurrencyManager source, int rowNum, string displayText)
		{
		}

		/// <summary>Gets the <see cref="T:System.Windows.Forms.AccessibleObject" /> for the column.</summary>
		/// <returns>An <see cref="T:System.Windows.Forms.AccessibleObject" /> for the column.</returns>
		// Token: 0x1700049F RID: 1183
		// (get) Token: 0x0600130D RID: 4877 RVA: 0x00048696 File Offset: 0x00046896
		[Browsable(false)]
		public AccessibleObject HeaderAccessibleObject
		{
			get
			{
				if (this.headerAccessibleObject == null)
				{
					this.headerAccessibleObject = this.CreateHeaderAccessibleObject();
				}
				return this.headerAccessibleObject;
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.ComponentModel.PropertyDescriptor" /> that determines the attributes of data displayed by the <see cref="T:System.Windows.Forms.DataGridColumnStyle" />.</summary>
		/// <returns>A <see cref="T:System.ComponentModel.PropertyDescriptor" /> that contains data about the attributes of the column.</returns>
		// Token: 0x170004A0 RID: 1184
		// (get) Token: 0x0600130E RID: 4878 RVA: 0x000486B2 File Offset: 0x000468B2
		// (set) Token: 0x0600130F RID: 4879 RVA: 0x000486BA File Offset: 0x000468BA
		[DefaultValue(null)]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public virtual PropertyDescriptor PropertyDescriptor
		{
			get
			{
				return this.propertyDescriptor;
			}
			set
			{
				if (this.propertyDescriptor != value)
				{
					this.propertyDescriptor = value;
					this.OnPropertyDescriptorChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.DataGridColumnStyle.PropertyDescriptor" /> property value changes.</summary>
		// Token: 0x140000CD RID: 205
		// (add) Token: 0x06001310 RID: 4880 RVA: 0x000486D7 File Offset: 0x000468D7
		// (remove) Token: 0x06001311 RID: 4881 RVA: 0x000486EA File Offset: 0x000468EA
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public event EventHandler PropertyDescriptorChanged
		{
			add
			{
				base.Events.AddHandler(DataGridColumnStyle.EventPropertyDescriptor, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGridColumnStyle.EventPropertyDescriptor, value);
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Forms.AccessibleObject" /> for the column.</summary>
		/// <returns>An <see cref="T:System.Windows.Forms.AccessibleObject" /> for the column.</returns>
		// Token: 0x06001312 RID: 4882 RVA: 0x000486FD File Offset: 0x000468FD
		protected virtual AccessibleObject CreateHeaderAccessibleObject()
		{
			return new DataGridColumnStyle.DataGridColumnHeaderAccessibleObject(this);
		}

		/// <summary>Sets the <see cref="T:System.Windows.Forms.DataGrid" /> control that this column belongs to.</summary>
		/// <param name="value">The <see cref="T:System.Windows.Forms.DataGrid" /> control that this column belongs to. </param>
		// Token: 0x06001313 RID: 4883 RVA: 0x00048705 File Offset: 0x00046905
		protected virtual void SetDataGrid(DataGrid value)
		{
			this.SetDataGridInColumn(value);
		}

		/// <summary>Sets the <see cref="T:System.Windows.Forms.DataGrid" /> for the column.</summary>
		/// <param name="value">A <see cref="T:System.Windows.Forms.DataGrid" />. </param>
		// Token: 0x06001314 RID: 4884 RVA: 0x00048710 File Offset: 0x00046910
		protected virtual void SetDataGridInColumn(DataGrid value)
		{
			if (this.PropertyDescriptor == null && value != null)
			{
				CurrencyManager listManager = value.ListManager;
				if (listManager == null)
				{
					return;
				}
				PropertyDescriptorCollection itemProperties = listManager.GetItemProperties();
				int count = itemProperties.Count;
				for (int i = 0; i < itemProperties.Count; i++)
				{
					PropertyDescriptor propertyDescriptor = itemProperties[i];
					if (!typeof(IList).IsAssignableFrom(propertyDescriptor.PropertyType) && propertyDescriptor.Name.Equals(this.HeaderText))
					{
						this.PropertyDescriptor = propertyDescriptor;
						return;
					}
				}
			}
		}

		// Token: 0x06001315 RID: 4885 RVA: 0x00048790 File Offset: 0x00046990
		internal void SetDataGridInternalInColumn(DataGrid value)
		{
			if (value == null || value.Initializing)
			{
				return;
			}
			this.SetDataGridInColumn(value);
		}

		/// <summary>Gets the <see cref="T:System.Windows.Forms.DataGridTableStyle" /> for the column.</summary>
		/// <returns>The <see cref="P:System.Windows.Forms.DataGridColumnStyle.DataGridTableStyle" /> that contains the current <see cref="T:System.Windows.Forms.DataGridColumnStyle" />.</returns>
		// Token: 0x170004A1 RID: 1185
		// (get) Token: 0x06001316 RID: 4886 RVA: 0x000487A5 File Offset: 0x000469A5
		[Browsable(false)]
		public virtual DataGridTableStyle DataGridTableStyle
		{
			get
			{
				return this.dataGridTableStyle;
			}
		}

		// Token: 0x06001317 RID: 4887 RVA: 0x000487B0 File Offset: 0x000469B0
		internal void SetDataGridTableInColumn(DataGridTableStyle value, bool force)
		{
			if (this.dataGridTableStyle != null && this.dataGridTableStyle.Equals(value) && !force)
			{
				return;
			}
			if (value != null && value.DataGrid != null && !value.DataGrid.Initializing)
			{
				this.SetDataGridInColumn(value.DataGrid);
			}
			this.dataGridTableStyle = value;
		}

		/// <summary>Gets the height of the column's font.</summary>
		/// <returns>The height of the font, in pixels. If no font height has been set, the property returns the <see cref="T:System.Windows.Forms.DataGrid" /> control's font height; if that property hasn't been set, the default font height value for the <see cref="T:System.Windows.Forms.DataGrid" /> control is returned.</returns>
		// Token: 0x170004A2 RID: 1186
		// (get) Token: 0x06001318 RID: 4888 RVA: 0x00048802 File Offset: 0x00046A02
		protected int FontHeight
		{
			get
			{
				if (this.fontHeight != -1)
				{
					return this.fontHeight;
				}
				if (this.DataGridTableStyle != null)
				{
					return this.DataGridTableStyle.DataGrid.FontHeight;
				}
				return DataGridTableStyle.defaultFontHeight;
			}
		}

		// Token: 0x06001319 RID: 4889 RVA: 0x00048832 File Offset: 0x00046A32
		private bool ShouldSerializeFont()
		{
			return this.font != null;
		}

		/// <summary>Occurs when the column's font changes.</summary>
		// Token: 0x140000CE RID: 206
		// (add) Token: 0x0600131A RID: 4890 RVA: 0x0000701A File Offset: 0x0000521A
		// (remove) Token: 0x0600131B RID: 4891 RVA: 0x0000701A File Offset: 0x0000521A
		public event EventHandler FontChanged
		{
			add
			{
			}
			remove
			{
			}
		}

		/// <summary>Gets or sets the text of the column header.</summary>
		/// <returns>A string that is displayed as the column header. If it is created by the <see cref="T:System.Windows.Forms.DataGrid" />, the default value is the name of the <see cref="T:System.ComponentModel.PropertyDescriptor" /> used to create the column. If it is created by the user, the default is an empty string ("").</returns>
		// Token: 0x170004A3 RID: 1187
		// (get) Token: 0x0600131C RID: 4892 RVA: 0x0004883D File Offset: 0x00046A3D
		// (set) Token: 0x0600131D RID: 4893 RVA: 0x00048845 File Offset: 0x00046A45
		[Localizable(true)]
		[SRCategory("CatDisplay")]
		public virtual string HeaderText
		{
			get
			{
				return this.headerName;
			}
			set
			{
				if (value == null)
				{
					value = "";
				}
				if (this.headerName.Equals(value))
				{
					return;
				}
				this.headerName = value;
				this.OnHeaderTextChanged(EventArgs.Empty);
				if (this.PropertyDescriptor != null)
				{
					this.Invalidate();
				}
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.DataGridColumnStyle.HeaderText" /> property value changes.</summary>
		// Token: 0x140000CF RID: 207
		// (add) Token: 0x0600131E RID: 4894 RVA: 0x00048880 File Offset: 0x00046A80
		// (remove) Token: 0x0600131F RID: 4895 RVA: 0x00048893 File Offset: 0x00046A93
		public event EventHandler HeaderTextChanged
		{
			add
			{
				base.Events.AddHandler(DataGridColumnStyle.EventHeaderText, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGridColumnStyle.EventHeaderText, value);
			}
		}

		/// <summary>Gets or sets the name of the data member to map the column style to.</summary>
		/// <returns>The name of the data member to map the column style to.</returns>
		// Token: 0x170004A4 RID: 1188
		// (get) Token: 0x06001320 RID: 4896 RVA: 0x000488A6 File Offset: 0x00046AA6
		// (set) Token: 0x06001321 RID: 4897 RVA: 0x000488B0 File Offset: 0x00046AB0
		[Editor("System.Windows.Forms.Design.DataGridColumnStyleMappingNameEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[Localizable(true)]
		[DefaultValue("")]
		public string MappingName
		{
			get
			{
				return this.mappingName;
			}
			set
			{
				if (value == null)
				{
					value = "";
				}
				if (this.mappingName.Equals(value))
				{
					return;
				}
				string text = this.mappingName;
				this.mappingName = value;
				try
				{
					if (this.dataGridTableStyle != null)
					{
						this.dataGridTableStyle.GridColumnStyles.CheckForMappingNameDuplicates(this);
					}
				}
				catch
				{
					this.mappingName = text;
					throw;
				}
				this.OnMappingNameChanged(EventArgs.Empty);
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.DataGridColumnStyle.MappingName" /> value changes.</summary>
		// Token: 0x140000D0 RID: 208
		// (add) Token: 0x06001322 RID: 4898 RVA: 0x00048924 File Offset: 0x00046B24
		// (remove) Token: 0x06001323 RID: 4899 RVA: 0x00048937 File Offset: 0x00046B37
		public event EventHandler MappingNameChanged
		{
			add
			{
				base.Events.AddHandler(DataGridColumnStyle.EventMappingName, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGridColumnStyle.EventMappingName, value);
			}
		}

		// Token: 0x06001324 RID: 4900 RVA: 0x0004894A File Offset: 0x00046B4A
		private bool ShouldSerializeHeaderText()
		{
			return this.headerName.Length != 0;
		}

		/// <summary>Resets the <see cref="P:System.Windows.Forms.DataGridColumnStyle.HeaderText" /> to its default value, <see langword="null" />.</summary>
		// Token: 0x06001325 RID: 4901 RVA: 0x0004895A File Offset: 0x00046B5A
		public void ResetHeaderText()
		{
			this.HeaderText = "";
		}

		/// <summary>Gets or sets the text that is displayed when the column contains <see langword="null" />.</summary>
		/// <returns>A string displayed in a column containing a <see cref="F:System.DBNull.Value" />.</returns>
		// Token: 0x170004A5 RID: 1189
		// (get) Token: 0x06001326 RID: 4902 RVA: 0x00048967 File Offset: 0x00046B67
		// (set) Token: 0x06001327 RID: 4903 RVA: 0x0004896F File Offset: 0x00046B6F
		[Localizable(true)]
		[SRCategory("CatDisplay")]
		public virtual string NullText
		{
			get
			{
				return this.nullText;
			}
			set
			{
				if (this.nullText != null && this.nullText.Equals(value))
				{
					return;
				}
				this.nullText = value;
				this.OnNullTextChanged(EventArgs.Empty);
				this.Invalidate();
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.DataGridColumnStyle.NullText" /> value changes.</summary>
		// Token: 0x140000D1 RID: 209
		// (add) Token: 0x06001328 RID: 4904 RVA: 0x000489A0 File Offset: 0x00046BA0
		// (remove) Token: 0x06001329 RID: 4905 RVA: 0x000489B3 File Offset: 0x00046BB3
		public event EventHandler NullTextChanged
		{
			add
			{
				base.Events.AddHandler(DataGridColumnStyle.EventNullText, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGridColumnStyle.EventNullText, value);
			}
		}

		/// <summary>Gets or sets a value indicating whether the data in the column can be edited.</summary>
		/// <returns>
		///     <see langword="true" />, if the data cannot be edited; otherwise, <see langword="false" />.</returns>
		// Token: 0x170004A6 RID: 1190
		// (get) Token: 0x0600132A RID: 4906 RVA: 0x000489C6 File Offset: 0x00046BC6
		// (set) Token: 0x0600132B RID: 4907 RVA: 0x000489CE File Offset: 0x00046BCE
		[DefaultValue(false)]
		public virtual bool ReadOnly
		{
			get
			{
				return this.readOnly;
			}
			set
			{
				if (this.readOnly != value)
				{
					this.readOnly = value;
					this.OnReadOnlyChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.DataGridColumnStyle.ReadOnly" /> property value changes.</summary>
		// Token: 0x140000D2 RID: 210
		// (add) Token: 0x0600132C RID: 4908 RVA: 0x000489EB File Offset: 0x00046BEB
		// (remove) Token: 0x0600132D RID: 4909 RVA: 0x000489FE File Offset: 0x00046BFE
		public event EventHandler ReadOnlyChanged
		{
			add
			{
				base.Events.AddHandler(DataGridColumnStyle.EventReadOnly, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGridColumnStyle.EventReadOnly, value);
			}
		}

		/// <summary>Gets or sets the width of the column.</summary>
		/// <returns>The width of the column, in pixels.</returns>
		// Token: 0x170004A7 RID: 1191
		// (get) Token: 0x0600132E RID: 4910 RVA: 0x00048A11 File Offset: 0x00046C11
		// (set) Token: 0x0600132F RID: 4911 RVA: 0x00048A1C File Offset: 0x00046C1C
		[SRCategory("CatLayout")]
		[Localizable(true)]
		[DefaultValue(100)]
		public virtual int Width
		{
			get
			{
				return this.width;
			}
			set
			{
				if (this.width != value)
				{
					this.width = value;
					DataGrid dataGrid = (this.DataGridTableStyle == null) ? null : this.DataGridTableStyle.DataGrid;
					if (dataGrid != null)
					{
						dataGrid.PerformLayout();
						dataGrid.InvalidateInside();
					}
					this.OnWidthChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.DataGridColumnStyle.Width" /> property value changes.</summary>
		// Token: 0x140000D3 RID: 211
		// (add) Token: 0x06001330 RID: 4912 RVA: 0x00048A6A File Offset: 0x00046C6A
		// (remove) Token: 0x06001331 RID: 4913 RVA: 0x00048A7D File Offset: 0x00046C7D
		public event EventHandler WidthChanged
		{
			add
			{
				base.Events.AddHandler(DataGridColumnStyle.EventWidth, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGridColumnStyle.EventWidth, value);
			}
		}

		/// <summary>Suspends the painting of the column until the <see cref="M:System.Windows.Forms.DataGridColumnStyle.EndUpdate" /> method is called.</summary>
		// Token: 0x06001332 RID: 4914 RVA: 0x00048A90 File Offset: 0x00046C90
		protected void BeginUpdate()
		{
			this.updating = true;
		}

		/// <summary>Resumes the painting of columns suspended by calling the <see cref="M:System.Windows.Forms.DataGridColumnStyle.BeginUpdate" /> method.</summary>
		// Token: 0x06001333 RID: 4915 RVA: 0x00048A99 File Offset: 0x00046C99
		protected void EndUpdate()
		{
			this.updating = false;
			if (this.invalid)
			{
				this.invalid = false;
				this.Invalidate();
			}
		}

		// Token: 0x170004A8 RID: 1192
		// (get) Token: 0x06001334 RID: 4916 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		internal virtual bool WantArrows
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06001335 RID: 4917 RVA: 0x00048AB7 File Offset: 0x00046CB7
		internal virtual string GetDisplayText(object value)
		{
			return value.ToString();
		}

		// Token: 0x06001336 RID: 4918 RVA: 0x00048ABF File Offset: 0x00046CBF
		private void ResetNullText()
		{
			this.NullText = SR.GetString("DataGridNullText");
		}

		// Token: 0x06001337 RID: 4919 RVA: 0x00048AD1 File Offset: 0x00046CD1
		private bool ShouldSerializeNullText()
		{
			return !SR.GetString("DataGridNullText").Equals(this.nullText);
		}

		/// <summary>When overridden in a derived class, gets the width and height of the specified value. The width and height are used when the user navigates to <see cref="T:System.Windows.Forms.DataGridTableStyle" /> using the <see cref="T:System.Windows.Forms.DataGridColumnStyle" />.</summary>
		/// <param name="g">A <see cref="T:System.Drawing.Graphics" /> object. </param>
		/// <param name="value">An object value for which you want to know the screen height and width. </param>
		/// <returns>A <see cref="T:System.Drawing.Size" /> that contains the dimensions of the cell.</returns>
		// Token: 0x06001338 RID: 4920
		protected internal abstract Size GetPreferredSize(Graphics g, object value);

		/// <summary>When overridden in a derived class, gets the minimum height of a row.</summary>
		/// <returns>The minimum height of a row.</returns>
		// Token: 0x06001339 RID: 4921
		protected internal abstract int GetMinimumHeight();

		/// <summary>When overridden in a derived class, gets the height used for automatically resizing columns.</summary>
		/// <param name="g">A <see cref="T:System.Drawing.Graphics" /> object. </param>
		/// <param name="value">An object value for which you want to know the screen height and width. </param>
		/// <returns>The height used for auto resizing a cell.</returns>
		// Token: 0x0600133A RID: 4922
		protected internal abstract int GetPreferredHeight(Graphics g, object value);

		/// <summary>Gets the value in the specified row from the specified <see cref="T:System.Windows.Forms.CurrencyManager" />.</summary>
		/// <param name="source">The <see cref="T:System.Windows.Forms.CurrencyManager" /> containing the data. </param>
		/// <param name="rowNum">The row number containing the data. </param>
		/// <returns>An <see cref="T:System.Object" /> containing the value.</returns>
		/// <exception cref="T:System.ApplicationException">The <see cref="T:System.Data.DataColumn" /> for this <see cref="T:System.Windows.Forms.DataGridColumnStyle" /> hasn't been set yet. </exception>
		// Token: 0x0600133B RID: 4923 RVA: 0x00048AEC File Offset: 0x00046CEC
		protected internal virtual object GetColumnValueAtRow(CurrencyManager source, int rowNum)
		{
			this.CheckValidDataSource(source);
			if (this.PropertyDescriptor == null)
			{
				throw new InvalidOperationException(SR.GetString("DataGridColumnNoPropertyDescriptor"));
			}
			return this.PropertyDescriptor.GetValue(source[rowNum]);
		}

		/// <summary>Redraws the column and causes a paint message to be sent to the control.</summary>
		// Token: 0x0600133C RID: 4924 RVA: 0x00048B2C File Offset: 0x00046D2C
		protected virtual void Invalidate()
		{
			if (this.updating)
			{
				this.invalid = true;
				return;
			}
			DataGridTableStyle dataGridTableStyle = this.DataGridTableStyle;
			if (dataGridTableStyle != null)
			{
				dataGridTableStyle.InvalidateColumn(this);
			}
		}

		/// <summary>Throws an exception if the <see cref="T:System.Windows.Forms.DataGrid" /> does not have a valid data source, or if this column is not mapped to a valid property in the data source.</summary>
		/// <param name="value">A <see cref="T:System.Windows.Forms.CurrencyManager" /> to check. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="value" /> is <see langword="null" />. </exception>
		/// <exception cref="T:System.ApplicationException">The <see cref="T:System.ComponentModel.PropertyDescriptor" /> for this column is <see langword="null" />. </exception>
		// Token: 0x0600133D RID: 4925 RVA: 0x00048B5C File Offset: 0x00046D5C
		protected void CheckValidDataSource(CurrencyManager value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value", "DataGridColumnStyle.CheckValidDataSource(DataSource value), value == null");
			}
			if (this.PropertyDescriptor == null)
			{
				throw new InvalidOperationException(SR.GetString("DataGridColumnUnbound", new object[]
				{
					this.HeaderText
				}));
			}
		}

		/// <summary>When overridden in a derived class, initiates a request to interrupt an edit procedure.</summary>
		/// <param name="rowNum">The row number upon which an operation is being interrupted. </param>
		// Token: 0x0600133E RID: 4926
		protected internal abstract void Abort(int rowNum);

		/// <summary>When overridden in a derived class, initiates a request to complete an editing procedure.</summary>
		/// <param name="dataSource">The <see cref="T:System.Windows.Forms.CurrencyManager" /> for the <see cref="T:System.Windows.Forms.DataGridColumnStyle" />. </param>
		/// <param name="rowNum">The number of the row being edited. </param>
		/// <returns>
		///     <see langword="true" /> if the editing procedure committed successfully; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600133F RID: 4927
		protected internal abstract bool Commit(CurrencyManager dataSource, int rowNum);

		/// <summary>Prepares a cell for editing.</summary>
		/// <param name="source">The <see cref="T:System.Windows.Forms.CurrencyManager" /> for the <see cref="T:System.Windows.Forms.DataGridColumnStyle" />. </param>
		/// <param name="rowNum">The row number to edit. </param>
		/// <param name="bounds">The bounding <see cref="T:System.Drawing.Rectangle" /> in which the control is to be sited. </param>
		/// <param name="readOnly">A value indicating whether the column is a read-only. <see langword="true" /> if the value is read-only; otherwise, <see langword="false" />. </param>
		// Token: 0x06001340 RID: 4928 RVA: 0x00048BA5 File Offset: 0x00046DA5
		protected internal virtual void Edit(CurrencyManager source, int rowNum, Rectangle bounds, bool readOnly)
		{
			this.Edit(source, rowNum, bounds, readOnly, null, true);
		}

		/// <summary>Prepares the cell for editing using the specified <see cref="T:System.Windows.Forms.CurrencyManager" />, row number, and <see cref="T:System.Drawing.Rectangle" /> parameters.</summary>
		/// <param name="source">The <see cref="T:System.Windows.Forms.CurrencyManager" /> for the <see cref="T:System.Windows.Forms.DataGridColumnStyle" />. </param>
		/// <param name="rowNum">The row number in this column which is being edited. </param>
		/// <param name="bounds">The <see cref="T:System.Drawing.Rectangle" /> in which the control is to be sited. </param>
		/// <param name="readOnly">A value indicating whether the column is a read-only. <see langword="true" /> if the value is read-only; otherwise, <see langword="false" />. </param>
		/// <param name="displayText">The text to display in the control. </param>
		// Token: 0x06001341 RID: 4929 RVA: 0x00048BB4 File Offset: 0x00046DB4
		protected internal virtual void Edit(CurrencyManager source, int rowNum, Rectangle bounds, bool readOnly, string displayText)
		{
			this.Edit(source, rowNum, bounds, readOnly, displayText, true);
		}

		/// <summary>When overridden in a deriving class, prepares a cell for editing.</summary>
		/// <param name="source">The <see cref="T:System.Windows.Forms.CurrencyManager" /> for the <see cref="T:System.Windows.Forms.DataGridColumnStyle" />. </param>
		/// <param name="rowNum">The row number in this column which is being edited. </param>
		/// <param name="bounds">The <see cref="T:System.Drawing.Rectangle" /> in which the control is to be sited. </param>
		/// <param name="readOnly">A value indicating whether the column is a read-only. <see langword="true" /> if the value is read-only; otherwise, <see langword="false" />. </param>
		/// <param name="displayText">The text to display in the control. </param>
		/// <param name="cellIsVisible">A value indicating whether the cell is visible. <see langword="true" /> if the cell is visible; otherwise, <see langword="false" />. </param>
		// Token: 0x06001342 RID: 4930
		protected internal abstract void Edit(CurrencyManager source, int rowNum, Rectangle bounds, bool readOnly, string displayText, bool cellIsVisible);

		// Token: 0x06001343 RID: 4931 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		internal virtual bool MouseDown(int rowNum, int x, int y)
		{
			return false;
		}

		/// <summary>Enters a <see cref="F:System.DBNull.Value" /> into the column.</summary>
		// Token: 0x06001344 RID: 4932 RVA: 0x0000701A File Offset: 0x0000521A
		protected internal virtual void EnterNullValue()
		{
		}

		// Token: 0x06001345 RID: 4933 RVA: 0x00048BC4 File Offset: 0x00046DC4
		internal virtual bool KeyPress(int rowNum, Keys keyData)
		{
			if (this.ReadOnly || (this.DataGridTableStyle != null && this.DataGridTableStyle.DataGrid != null && this.DataGridTableStyle.DataGrid.ReadOnly))
			{
				return false;
			}
			if (keyData == (Keys)131168 || keyData == (Keys.ShiftKey | Keys.Space | Keys.Control))
			{
				this.EnterNullValue();
				return true;
			}
			return false;
		}

		/// <summary>Notifies a column that it must relinquish the focus to the control it is hosting.</summary>
		// Token: 0x06001346 RID: 4934 RVA: 0x0000701A File Offset: 0x0000521A
		protected internal virtual void ConcedeFocus()
		{
		}

		/// <summary>Paints the <see cref="T:System.Windows.Forms.DataGridColumnStyle" /> with the specified <see cref="T:System.Drawing.Graphics" />, <see cref="T:System.Drawing.Rectangle" />, <see cref="T:System.Windows.Forms.CurrencyManager" />, and row number.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> to draw to. </param>
		/// <param name="bounds">The bounding <see cref="T:System.Drawing.Rectangle" /> to paint into. </param>
		/// <param name="source">The <see cref="T:System.Windows.Forms.CurrencyManager" /> of the <see cref="T:System.Windows.Forms.DataGrid" /> control the column belongs to. </param>
		/// <param name="rowNum">The number of the row in the underlying data being referred to. </param>
		// Token: 0x06001347 RID: 4935
		protected internal abstract void Paint(Graphics g, Rectangle bounds, CurrencyManager source, int rowNum);

		/// <summary>When overridden in a derived class, paints a <see cref="T:System.Windows.Forms.DataGridColumnStyle" /> with the specified <see cref="T:System.Drawing.Graphics" />, <see cref="T:System.Drawing.Rectangle" />, <see cref="T:System.Windows.Forms.CurrencyManager" />, row number, and alignment.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> to draw to. </param>
		/// <param name="bounds">The bounding <see cref="T:System.Drawing.Rectangle" /> to paint into. </param>
		/// <param name="source">The <see cref="T:System.Windows.Forms.CurrencyManager" /> of the <see cref="T:System.Windows.Forms.DataGrid" /> control the column belongs to. </param>
		/// <param name="rowNum">The number of the row in the underlying data being referred to. </param>
		/// <param name="alignToRight">A value indicating whether to align the column's content to the right. <see langword="true" /> if the content should be aligned to the right; otherwise <see langword="false" />. </param>
		// Token: 0x06001348 RID: 4936
		protected internal abstract void Paint(Graphics g, Rectangle bounds, CurrencyManager source, int rowNum, bool alignToRight);

		/// <summary>Paints a <see cref="T:System.Windows.Forms.DataGridColumnStyle" /> with the specified <see cref="T:System.Drawing.Graphics" />, <see cref="T:System.Drawing.Rectangle" />, <see cref="T:System.Windows.Forms.CurrencyManager" />, row number, background color, foreground color, and alignment.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> to draw to. </param>
		/// <param name="bounds">The bounding <see cref="T:System.Drawing.Rectangle" /> to paint into. </param>
		/// <param name="source">The <see cref="T:System.Windows.Forms.CurrencyManager" /> of the <see cref="T:System.Windows.Forms.DataGrid" /> control the column belongs to. </param>
		/// <param name="rowNum">The number of the row in the underlying data table being referred to. </param>
		/// <param name="backBrush">A <see cref="T:System.Drawing.Brush" /> used to paint the background color. </param>
		/// <param name="foreBrush">A <see cref="T:System.Drawing.Color" /> used to paint the foreground color. </param>
		/// <param name="alignToRight">A value indicating whether to align the content to the right. <see langword="true" /> if the content is aligned to the right, otherwise, <see langword="false" />. </param>
		// Token: 0x06001349 RID: 4937 RVA: 0x00048C1B File Offset: 0x00046E1B
		protected internal virtual void Paint(Graphics g, Rectangle bounds, CurrencyManager source, int rowNum, Brush backBrush, Brush foreBrush, bool alignToRight)
		{
			this.Paint(g, bounds, source, rowNum, alignToRight);
		}

		// Token: 0x0600134A RID: 4938 RVA: 0x00048C2C File Offset: 0x00046E2C
		private void OnPropertyDescriptorChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[DataGridColumnStyle.EventPropertyDescriptor] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x0600134B RID: 4939 RVA: 0x00048C5C File Offset: 0x00046E5C
		private void OnAlignmentChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[DataGridColumnStyle.EventAlignment] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x0600134C RID: 4940 RVA: 0x00048C8C File Offset: 0x00046E8C
		private void OnHeaderTextChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[DataGridColumnStyle.EventHeaderText] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x0600134D RID: 4941 RVA: 0x00048CBC File Offset: 0x00046EBC
		private void OnMappingNameChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[DataGridColumnStyle.EventMappingName] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x0600134E RID: 4942 RVA: 0x00048CEC File Offset: 0x00046EEC
		private void OnReadOnlyChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[DataGridColumnStyle.EventReadOnly] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x0600134F RID: 4943 RVA: 0x00048D1C File Offset: 0x00046F1C
		private void OnNullTextChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[DataGridColumnStyle.EventNullText] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06001350 RID: 4944 RVA: 0x00048D4C File Offset: 0x00046F4C
		private void OnWidthChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[DataGridColumnStyle.EventWidth] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Sets the value in a specified row with the value from a specified <see cref="T:System.Windows.Forms.CurrencyManager" />.</summary>
		/// <param name="source">A <see cref="T:System.Windows.Forms.CurrencyManager" /> associated with the <see cref="T:System.Windows.Forms.DataGridColumnStyle" />. </param>
		/// <param name="rowNum">The number of the row. </param>
		/// <param name="value">The value to set. </param>
		/// <exception cref="T:System.ArgumentException">The <see cref="T:System.Windows.Forms.CurrencyManager" /> object's <see cref="P:System.Windows.Forms.BindingManagerBase.Position" /> does not match <paramref name="rowNum" />. </exception>
		// Token: 0x06001351 RID: 4945 RVA: 0x00048D7C File Offset: 0x00046F7C
		protected internal virtual void SetColumnValueAtRow(CurrencyManager source, int rowNum, object value)
		{
			this.CheckValidDataSource(source);
			if (source.Position != rowNum)
			{
				throw new ArgumentException(SR.GetString("DataGridColumnListManagerPosition"), "rowNum");
			}
			if (source[rowNum] is IEditableObject)
			{
				((IEditableObject)source[rowNum]).BeginEdit();
			}
			this.PropertyDescriptor.SetValue(source[rowNum], value);
		}

		/// <summary>Informs the <see cref="T:System.Windows.Forms.DataGrid" /> that the user has begun editing the column.</summary>
		/// <param name="editingControl">The <see cref="T:System.Windows.Forms.Control" /> that hosted by the column. </param>
		// Token: 0x06001352 RID: 4946 RVA: 0x00048DE0 File Offset: 0x00046FE0
		protected internal virtual void ColumnStartedEditing(Control editingControl)
		{
			this.DataGridTableStyle.DataGrid.ColumnStartedEditing(editingControl);
		}

		/// <summary>Informs the <see cref="T:System.Windows.Forms.DataGrid" /> control that the user has begun editing the column.</summary>
		/// <param name="editingControl">The <see cref="T:System.Windows.Forms.Control" /> that is editing the column.</param>
		// Token: 0x06001353 RID: 4947 RVA: 0x00048DF3 File Offset: 0x00046FF3
		void IDataGridColumnStyleEditingNotificationService.ColumnStartedEditing(Control editingControl)
		{
			this.ColumnStartedEditing(editingControl);
		}

		/// <summary>Allows the column to free resources when the control it hosts is not needed.</summary>
		// Token: 0x06001354 RID: 4948 RVA: 0x0000701A File Offset: 0x0000521A
		protected internal virtual void ReleaseHostedControl()
		{
		}

		// Token: 0x04000986 RID: 2438
		private HorizontalAlignment alignment;

		// Token: 0x04000987 RID: 2439
		private PropertyDescriptor propertyDescriptor;

		// Token: 0x04000988 RID: 2440
		private DataGridTableStyle dataGridTableStyle;

		// Token: 0x04000989 RID: 2441
		private Font font;

		// Token: 0x0400098A RID: 2442
		internal int fontHeight = -1;

		// Token: 0x0400098B RID: 2443
		private string mappingName = "";

		// Token: 0x0400098C RID: 2444
		private string headerName = "";

		// Token: 0x0400098D RID: 2445
		private bool invalid;

		// Token: 0x0400098E RID: 2446
		private string nullText = SR.GetString("DataGridNullText");

		// Token: 0x0400098F RID: 2447
		private bool readOnly;

		// Token: 0x04000990 RID: 2448
		private bool updating;

		// Token: 0x04000991 RID: 2449
		internal int width = -1;

		// Token: 0x04000992 RID: 2450
		private bool isDefault;

		// Token: 0x04000993 RID: 2451
		private AccessibleObject headerAccessibleObject;

		// Token: 0x04000994 RID: 2452
		private static readonly object EventAlignment = new object();

		// Token: 0x04000995 RID: 2453
		private static readonly object EventPropertyDescriptor = new object();

		// Token: 0x04000996 RID: 2454
		private static readonly object EventHeaderText = new object();

		// Token: 0x04000997 RID: 2455
		private static readonly object EventMappingName = new object();

		// Token: 0x04000998 RID: 2456
		private static readonly object EventNullText = new object();

		// Token: 0x04000999 RID: 2457
		private static readonly object EventReadOnly = new object();

		// Token: 0x0400099A RID: 2458
		private static readonly object EventWidth = new object();

		/// <summary>Contains a <see cref="T:System.Diagnostics.TraceSwitch" /> that is used by the .NET Framework infrastructure.</summary>
		// Token: 0x02000591 RID: 1425
		protected class CompModSwitches
		{
			/// <summary>Gets a <see cref="T:System.Diagnostics.TraceSwitch" />.</summary>
			/// <returns>A <see cref="T:System.Diagnostics.TraceSwitch" /> used by the .NET Framework infrastructure.</returns>
			// Token: 0x17001500 RID: 5376
			// (get) Token: 0x060057F8 RID: 22520 RVA: 0x001724B1 File Offset: 0x001706B1
			public static TraceSwitch DGEditColumnEditing
			{
				get
				{
					if (DataGridColumnStyle.CompModSwitches.dgEditColumnEditing == null)
					{
						DataGridColumnStyle.CompModSwitches.dgEditColumnEditing = new TraceSwitch("DGEditColumnEditing", "Editing related tracing");
					}
					return DataGridColumnStyle.CompModSwitches.dgEditColumnEditing;
				}
			}

			// Token: 0x0400389F RID: 14495
			private static TraceSwitch dgEditColumnEditing;
		}

		/// <summary>Provides an implementation for an object that can be inspected by an accessibility application.</summary>
		// Token: 0x02000592 RID: 1426
		[ComVisible(true)]
		protected class DataGridColumnHeaderAccessibleObject : AccessibleObject
		{
			/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DataGridColumnStyle.DataGridColumnHeaderAccessibleObject" /> class and specifies the <see cref="T:System.Windows.Forms.DataGridColumnStyle" /> that hosts the object.</summary>
			/// <param name="owner">The <see cref="T:System.Windows.Forms.DataGridColumnStyle" /> that hosts the object. </param>
			// Token: 0x060057FA RID: 22522 RVA: 0x001724D3 File Offset: 0x001706D3
			public DataGridColumnHeaderAccessibleObject(DataGridColumnStyle owner) : this()
			{
				this.owner = owner;
			}

			/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DataGridColumnStyle.DataGridColumnHeaderAccessibleObject" /> class without specifying a <see cref="T:System.Windows.Forms.DataGridColumnStyle" /> host for the object. </summary>
			// Token: 0x060057FB RID: 22523 RVA: 0x001724E2 File Offset: 0x001706E2
			public DataGridColumnHeaderAccessibleObject()
			{
			}

			/// <summary>Gets the bounding rectangle of a column.</summary>
			/// <returns>A <see cref="T:System.Drawing.Rectangle" /> that contains the bounding values of the column.</returns>
			// Token: 0x17001501 RID: 5377
			// (get) Token: 0x060057FC RID: 22524 RVA: 0x001724EC File Offset: 0x001706EC
			public override Rectangle Bounds
			{
				get
				{
					if (this.owner.PropertyDescriptor == null)
					{
						return Rectangle.Empty;
					}
					DataGrid dataGrid = this.DataGrid;
					if (dataGrid.DataGridRowsLength == 0)
					{
						return Rectangle.Empty;
					}
					GridColumnStylesCollection gridColumnStyles = this.owner.dataGridTableStyle.GridColumnStyles;
					int col = -1;
					for (int i = 0; i < gridColumnStyles.Count; i++)
					{
						if (gridColumnStyles[i] == this.owner)
						{
							col = i;
							break;
						}
					}
					Rectangle cellBounds = dataGrid.GetCellBounds(0, col);
					cellBounds.Y = dataGrid.GetColumnHeadersRect().Y;
					return dataGrid.RectangleToScreen(cellBounds);
				}
			}

			/// <summary>Gets the name of the column that owns the accessibility object.</summary>
			/// <returns>The name of the column that owns the accessibility object.</returns>
			// Token: 0x17001502 RID: 5378
			// (get) Token: 0x060057FD RID: 22525 RVA: 0x00172584 File Offset: 0x00170784
			public override string Name
			{
				get
				{
					return this.Owner.headerName;
				}
			}

			/// <summary>Gets the column style object that owns the accessibility object.</summary>
			/// <returns>The <see cref="T:System.Windows.Forms.DataGridColumnStyle" /> that owns the accessibility object.</returns>
			// Token: 0x17001503 RID: 5379
			// (get) Token: 0x060057FE RID: 22526 RVA: 0x00172591 File Offset: 0x00170791
			protected DataGridColumnStyle Owner
			{
				get
				{
					return this.owner;
				}
			}

			/// <summary>Gets the parent accessibility object.</summary>
			/// <returns>The parent <see cref="T:System.Windows.Forms.AccessibleObject" /> of the column style object.</returns>
			// Token: 0x17001504 RID: 5380
			// (get) Token: 0x060057FF RID: 22527 RVA: 0x00172599 File Offset: 0x00170799
			public override AccessibleObject Parent
			{
				[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				get
				{
					return this.DataGrid.AccessibilityObject;
				}
			}

			// Token: 0x17001505 RID: 5381
			// (get) Token: 0x06005800 RID: 22528 RVA: 0x001725A6 File Offset: 0x001707A6
			private DataGrid DataGrid
			{
				get
				{
					return this.owner.dataGridTableStyle.dataGrid;
				}
			}

			/// <summary>Gets the role of the accessibility object.</summary>
			/// <returns>The <see langword="AccessibleRole" /> object of the accessibility object.</returns>
			// Token: 0x17001506 RID: 5382
			// (get) Token: 0x06005801 RID: 22529 RVA: 0x001725B8 File Offset: 0x001707B8
			public override AccessibleRole Role
			{
				get
				{
					return AccessibleRole.ColumnHeader;
				}
			}

			/// <summary>Enables navigation to another object.</summary>
			/// <param name="navdir">One of the <see cref="T:System.Windows.Forms.AccessibleNavigation" /> values. </param>
			/// <returns>An <see cref="T:System.Windows.Forms.AccessibleObject" /> specified by the <paramref name="navdir" /> parameter.</returns>
			// Token: 0x06005802 RID: 22530 RVA: 0x001725BC File Offset: 0x001707BC
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override AccessibleObject Navigate(AccessibleNavigation navdir)
			{
				switch (navdir)
				{
				case AccessibleNavigation.Up:
				case AccessibleNavigation.Left:
				case AccessibleNavigation.Previous:
					return this.Parent.GetChild(1 + this.Owner.dataGridTableStyle.GridColumnStyles.IndexOf(this.Owner) - 1);
				case AccessibleNavigation.Down:
				case AccessibleNavigation.Right:
				case AccessibleNavigation.Next:
					return this.Parent.GetChild(1 + this.Owner.dataGridTableStyle.GridColumnStyles.IndexOf(this.Owner) + 1);
				default:
					return null;
				}
			}

			// Token: 0x040038A0 RID: 14496
			private DataGridColumnStyle owner;
		}
	}
}
