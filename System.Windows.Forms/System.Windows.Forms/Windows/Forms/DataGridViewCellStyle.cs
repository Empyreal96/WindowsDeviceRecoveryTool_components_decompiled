using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Text;

namespace System.Windows.Forms
{
	/// <summary>Represents the formatting and style information applied to individual cells within a <see cref="T:System.Windows.Forms.DataGridView" /> control.</summary>
	// Token: 0x020001A2 RID: 418
	[TypeConverter(typeof(DataGridViewCellStyleConverter))]
	[Editor("System.Windows.Forms.Design.DataGridViewCellStyleEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
	public class DataGridViewCellStyle : ICloneable
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DataGridViewCellStyle" /> class using default property values.</summary>
		// Token: 0x06001B21 RID: 6945 RVA: 0x000875C8 File Offset: 0x000857C8
		public DataGridViewCellStyle()
		{
			this.propertyStore = new PropertyStore();
			this.scope = DataGridViewCellStyleScopes.None;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DataGridViewCellStyle" /> class using the property values of the specified <see cref="T:System.Windows.Forms.DataGridViewCellStyle" />.</summary>
		/// <param name="dataGridViewCellStyle">The <see cref="T:System.Windows.Forms.DataGridViewCellStyle" /> used as a template to provide initial property values. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="dataGridViewCellStyle" /> is <see langword="null" />.</exception>
		// Token: 0x06001B22 RID: 6946 RVA: 0x000875E4 File Offset: 0x000857E4
		public DataGridViewCellStyle(DataGridViewCellStyle dataGridViewCellStyle)
		{
			if (dataGridViewCellStyle == null)
			{
				throw new ArgumentNullException("dataGridViewCellStyle");
			}
			this.propertyStore = new PropertyStore();
			this.scope = DataGridViewCellStyleScopes.None;
			this.BackColor = dataGridViewCellStyle.BackColor;
			this.ForeColor = dataGridViewCellStyle.ForeColor;
			this.SelectionBackColor = dataGridViewCellStyle.SelectionBackColor;
			this.SelectionForeColor = dataGridViewCellStyle.SelectionForeColor;
			this.Font = dataGridViewCellStyle.Font;
			this.NullValue = dataGridViewCellStyle.NullValue;
			this.DataSourceNullValue = dataGridViewCellStyle.DataSourceNullValue;
			this.Format = dataGridViewCellStyle.Format;
			if (!dataGridViewCellStyle.IsFormatProviderDefault)
			{
				this.FormatProvider = dataGridViewCellStyle.FormatProvider;
			}
			this.AlignmentInternal = dataGridViewCellStyle.Alignment;
			this.WrapModeInternal = dataGridViewCellStyle.WrapMode;
			this.Tag = dataGridViewCellStyle.Tag;
			this.PaddingInternal = dataGridViewCellStyle.Padding;
		}

		/// <summary>Gets or sets a value indicating the position of the cell content within a <see cref="T:System.Windows.Forms.DataGridView" /> cell.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.DataGridViewContentAlignment" /> values. The default is <see cref="F:System.Windows.Forms.DataGridViewContentAlignment.NotSet" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The property value is not a valid <see cref="T:System.Windows.Forms.DataGridViewContentAlignment" /> value. </exception>
		// Token: 0x17000638 RID: 1592
		// (get) Token: 0x06001B23 RID: 6947 RVA: 0x000876BC File Offset: 0x000858BC
		// (set) Token: 0x06001B24 RID: 6948 RVA: 0x000876E4 File Offset: 0x000858E4
		[SRDescription("DataGridViewCellStyleAlignmentDescr")]
		[DefaultValue(DataGridViewContentAlignment.NotSet)]
		[SRCategory("CatLayout")]
		public DataGridViewContentAlignment Alignment
		{
			get
			{
				bool flag;
				int integer = this.Properties.GetInteger(DataGridViewCellStyle.PropAlignment, out flag);
				if (flag)
				{
					return (DataGridViewContentAlignment)integer;
				}
				return DataGridViewContentAlignment.NotSet;
			}
			set
			{
				if (value <= DataGridViewContentAlignment.MiddleCenter)
				{
					if (value <= DataGridViewContentAlignment.TopRight)
					{
						if (value <= DataGridViewContentAlignment.TopCenter || value == DataGridViewContentAlignment.TopRight)
						{
							goto IL_5C;
						}
					}
					else if (value == DataGridViewContentAlignment.MiddleLeft || value == DataGridViewContentAlignment.MiddleCenter)
					{
						goto IL_5C;
					}
				}
				else if (value <= DataGridViewContentAlignment.BottomLeft)
				{
					if (value == DataGridViewContentAlignment.MiddleRight || value == DataGridViewContentAlignment.BottomLeft)
					{
						goto IL_5C;
					}
				}
				else if (value == DataGridViewContentAlignment.BottomCenter || value == DataGridViewContentAlignment.BottomRight)
				{
					goto IL_5C;
				}
				throw new InvalidEnumArgumentException("value", (int)value, typeof(DataGridViewContentAlignment));
				IL_5C:
				this.AlignmentInternal = value;
			}
		}

		// Token: 0x17000639 RID: 1593
		// (set) Token: 0x06001B25 RID: 6949 RVA: 0x00087754 File Offset: 0x00085954
		internal DataGridViewContentAlignment AlignmentInternal
		{
			set
			{
				if (this.Alignment != value)
				{
					this.Properties.SetInteger(DataGridViewCellStyle.PropAlignment, (int)value);
					this.OnPropertyChanged(DataGridViewCellStyle.DataGridViewCellStylePropertyInternal.Other);
				}
			}
		}

		/// <summary>Gets or sets the background color of a <see cref="T:System.Windows.Forms.DataGridView" /> cell.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that represents the background color of a cell. The default is <see cref="F:System.Drawing.Color.Empty" />.</returns>
		// Token: 0x1700063A RID: 1594
		// (get) Token: 0x06001B26 RID: 6950 RVA: 0x00087777 File Offset: 0x00085977
		// (set) Token: 0x06001B27 RID: 6951 RVA: 0x0008778C File Offset: 0x0008598C
		[SRCategory("CatAppearance")]
		public Color BackColor
		{
			get
			{
				return this.Properties.GetColor(DataGridViewCellStyle.PropBackColor);
			}
			set
			{
				Color backColor = this.BackColor;
				if (!value.IsEmpty || this.Properties.ContainsObject(DataGridViewCellStyle.PropBackColor))
				{
					this.Properties.SetColor(DataGridViewCellStyle.PropBackColor, value);
				}
				if (!backColor.Equals(this.BackColor))
				{
					this.OnPropertyChanged(DataGridViewCellStyle.DataGridViewCellStylePropertyInternal.Color);
				}
			}
		}

		/// <summary>Gets or sets the value saved to the data source when the user enters a null value into a cell.</summary>
		/// <returns>The value saved to the data source when the user specifies a null cell value. The default is <see cref="F:System.DBNull.Value" />.</returns>
		// Token: 0x1700063B RID: 1595
		// (get) Token: 0x06001B28 RID: 6952 RVA: 0x000877ED File Offset: 0x000859ED
		// (set) Token: 0x06001B29 RID: 6953 RVA: 0x00087818 File Offset: 0x00085A18
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public object DataSourceNullValue
		{
			get
			{
				if (this.Properties.ContainsObject(DataGridViewCellStyle.PropDataSourceNullValue))
				{
					return this.Properties.GetObject(DataGridViewCellStyle.PropDataSourceNullValue);
				}
				return DBNull.Value;
			}
			set
			{
				object dataSourceNullValue = this.DataSourceNullValue;
				if (dataSourceNullValue == value || (dataSourceNullValue != null && dataSourceNullValue.Equals(value)))
				{
					return;
				}
				if (value == DBNull.Value && this.Properties.ContainsObject(DataGridViewCellStyle.PropDataSourceNullValue))
				{
					this.Properties.RemoveObject(DataGridViewCellStyle.PropDataSourceNullValue);
				}
				else
				{
					this.Properties.SetObject(DataGridViewCellStyle.PropDataSourceNullValue, value);
				}
				this.OnPropertyChanged(DataGridViewCellStyle.DataGridViewCellStylePropertyInternal.Other);
			}
		}

		/// <summary>Gets or sets the font applied to the textual content of a <see cref="T:System.Windows.Forms.DataGridView" /> cell.</summary>
		/// <returns>The <see cref="T:System.Drawing.Font" /> applied to the cell text. The default is <see langword="null" />.</returns>
		// Token: 0x1700063C RID: 1596
		// (get) Token: 0x06001B2A RID: 6954 RVA: 0x00087881 File Offset: 0x00085A81
		// (set) Token: 0x06001B2B RID: 6955 RVA: 0x00087898 File Offset: 0x00085A98
		[SRCategory("CatAppearance")]
		public Font Font
		{
			get
			{
				return (Font)this.Properties.GetObject(DataGridViewCellStyle.PropFont);
			}
			set
			{
				Font font = this.Font;
				if (value != null || this.Properties.ContainsObject(DataGridViewCellStyle.PropFont))
				{
					this.Properties.SetObject(DataGridViewCellStyle.PropFont, value);
				}
				if ((font == null && value != null) || (font != null && value == null) || (font != null && value != null && !font.Equals(this.Font)))
				{
					this.OnPropertyChanged(DataGridViewCellStyle.DataGridViewCellStylePropertyInternal.Font);
				}
			}
		}

		/// <summary>Gets or sets the foreground color of a <see cref="T:System.Windows.Forms.DataGridView" /> cell.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that represents the foreground color of a cell. The default is <see cref="F:System.Drawing.Color.Empty" />.</returns>
		// Token: 0x1700063D RID: 1597
		// (get) Token: 0x06001B2C RID: 6956 RVA: 0x000878F9 File Offset: 0x00085AF9
		// (set) Token: 0x06001B2D RID: 6957 RVA: 0x0008790C File Offset: 0x00085B0C
		[SRCategory("CatAppearance")]
		public Color ForeColor
		{
			get
			{
				return this.Properties.GetColor(DataGridViewCellStyle.PropForeColor);
			}
			set
			{
				Color foreColor = this.ForeColor;
				if (!value.IsEmpty || this.Properties.ContainsObject(DataGridViewCellStyle.PropForeColor))
				{
					this.Properties.SetColor(DataGridViewCellStyle.PropForeColor, value);
				}
				if (!foreColor.Equals(this.ForeColor))
				{
					this.OnPropertyChanged(DataGridViewCellStyle.DataGridViewCellStylePropertyInternal.ForeColor);
				}
			}
		}

		/// <summary>Gets or sets the format string applied to the textual content of a <see cref="T:System.Windows.Forms.DataGridView" /> cell.</summary>
		/// <returns>A string that indicates the format of the cell value. The default is <see cref="F:System.String.Empty" />.</returns>
		// Token: 0x1700063E RID: 1598
		// (get) Token: 0x06001B2E RID: 6958 RVA: 0x00087970 File Offset: 0x00085B70
		// (set) Token: 0x06001B2F RID: 6959 RVA: 0x000879A0 File Offset: 0x00085BA0
		[DefaultValue("")]
		[Editor("System.Windows.Forms.Design.FormatStringEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[SRCategory("CatBehavior")]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public string Format
		{
			get
			{
				object @object = this.Properties.GetObject(DataGridViewCellStyle.PropFormat);
				if (@object == null)
				{
					return string.Empty;
				}
				return (string)@object;
			}
			set
			{
				string format = this.Format;
				if ((value != null && value.Length > 0) || this.Properties.ContainsObject(DataGridViewCellStyle.PropFormat))
				{
					this.Properties.SetObject(DataGridViewCellStyle.PropFormat, value);
				}
				if (!format.Equals(this.Format))
				{
					this.OnPropertyChanged(DataGridViewCellStyle.DataGridViewCellStylePropertyInternal.Other);
				}
			}
		}

		/// <summary>Gets or sets the object used to provide culture-specific formatting of <see cref="T:System.Windows.Forms.DataGridView" /> cell values.</summary>
		/// <returns>An <see cref="T:System.IFormatProvider" /> used for cell formatting. The default is <see cref="P:System.Globalization.CultureInfo.CurrentUICulture" />.</returns>
		// Token: 0x1700063F RID: 1599
		// (get) Token: 0x06001B30 RID: 6960 RVA: 0x000879F8 File Offset: 0x00085BF8
		// (set) Token: 0x06001B31 RID: 6961 RVA: 0x00087A28 File Offset: 0x00085C28
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public IFormatProvider FormatProvider
		{
			get
			{
				object @object = this.Properties.GetObject(DataGridViewCellStyle.PropFormatProvider);
				if (@object == null)
				{
					return CultureInfo.CurrentCulture;
				}
				return (IFormatProvider)@object;
			}
			set
			{
				object @object = this.Properties.GetObject(DataGridViewCellStyle.PropFormatProvider);
				this.Properties.SetObject(DataGridViewCellStyle.PropFormatProvider, value);
				if (value != @object)
				{
					this.OnPropertyChanged(DataGridViewCellStyle.DataGridViewCellStylePropertyInternal.Other);
				}
			}
		}

		/// <summary>Gets a value indicating whether the <see cref="P:System.Windows.Forms.DataGridViewCellStyle.DataSourceNullValue" /> property has been set.</summary>
		/// <returns>
		///     <see langword="true" /> if the value of the <see cref="P:System.Windows.Forms.DataGridViewCellStyle.DataSourceNullValue" /> property is the default value; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000640 RID: 1600
		// (get) Token: 0x06001B32 RID: 6962 RVA: 0x00087A62 File Offset: 0x00085C62
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public bool IsDataSourceNullValueDefault
		{
			get
			{
				return !this.Properties.ContainsObject(DataGridViewCellStyle.PropDataSourceNullValue) || this.Properties.GetObject(DataGridViewCellStyle.PropDataSourceNullValue) == DBNull.Value;
			}
		}

		/// <summary>Gets a value that indicates whether the <see cref="P:System.Windows.Forms.DataGridViewCellStyle.FormatProvider" /> property has been set.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="P:System.Windows.Forms.DataGridViewCellStyle.FormatProvider" /> property is the default value; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000641 RID: 1601
		// (get) Token: 0x06001B33 RID: 6963 RVA: 0x00087A8F File Offset: 0x00085C8F
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public bool IsFormatProviderDefault
		{
			get
			{
				return this.Properties.GetObject(DataGridViewCellStyle.PropFormatProvider) == null;
			}
		}

		/// <summary>Gets a value indicating whether the <see cref="P:System.Windows.Forms.DataGridViewCellStyle.NullValue" /> property has been set.</summary>
		/// <returns>
		///     <see langword="true" /> if the value of the <see cref="P:System.Windows.Forms.DataGridViewCellStyle.NullValue" /> property is the default value; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000642 RID: 1602
		// (get) Token: 0x06001B34 RID: 6964 RVA: 0x00087AA4 File Offset: 0x00085CA4
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public bool IsNullValueDefault
		{
			get
			{
				if (!this.Properties.ContainsObject(DataGridViewCellStyle.PropNullValue))
				{
					return true;
				}
				object @object = this.Properties.GetObject(DataGridViewCellStyle.PropNullValue);
				return @object is string && @object.Equals("");
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.Forms.DataGridView" /> cell display value corresponding to a cell value of <see cref="F:System.DBNull.Value" /> or <see langword="null" />.</summary>
		/// <returns>The object used to indicate a null value in a cell. The default is <see cref="F:System.String.Empty" />.</returns>
		// Token: 0x17000643 RID: 1603
		// (get) Token: 0x06001B35 RID: 6965 RVA: 0x00087AEB File Offset: 0x00085CEB
		// (set) Token: 0x06001B36 RID: 6966 RVA: 0x00087B18 File Offset: 0x00085D18
		[DefaultValue("")]
		[TypeConverter(typeof(StringConverter))]
		[SRCategory("CatData")]
		public object NullValue
		{
			get
			{
				if (this.Properties.ContainsObject(DataGridViewCellStyle.PropNullValue))
				{
					return this.Properties.GetObject(DataGridViewCellStyle.PropNullValue);
				}
				return "";
			}
			set
			{
				object nullValue = this.NullValue;
				if (nullValue == value || (nullValue != null && nullValue.Equals(value)))
				{
					return;
				}
				if (value is string && value.Equals("") && this.Properties.ContainsObject(DataGridViewCellStyle.PropNullValue))
				{
					this.Properties.RemoveObject(DataGridViewCellStyle.PropNullValue);
				}
				else
				{
					this.Properties.SetObject(DataGridViewCellStyle.PropNullValue, value);
				}
				this.OnPropertyChanged(DataGridViewCellStyle.DataGridViewCellStylePropertyInternal.Other);
			}
		}

		/// <summary>Gets or sets the space between the edge of a <see cref="T:System.Windows.Forms.DataGridViewCell" /> and its content.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.Padding" /> that represents the space between the edge of a <see cref="T:System.Windows.Forms.DataGridViewCell" /> and its content.</returns>
		// Token: 0x17000644 RID: 1604
		// (get) Token: 0x06001B37 RID: 6967 RVA: 0x00087B8E File Offset: 0x00085D8E
		// (set) Token: 0x06001B38 RID: 6968 RVA: 0x00087BA0 File Offset: 0x00085DA0
		[SRCategory("CatLayout")]
		public Padding Padding
		{
			get
			{
				return this.Properties.GetPadding(DataGridViewCellStyle.PropPadding);
			}
			set
			{
				if (value.Left < 0 || value.Right < 0 || value.Top < 0 || value.Bottom < 0)
				{
					if (value.All != -1)
					{
						value.All = 0;
					}
					else
					{
						value.Left = Math.Max(0, value.Left);
						value.Right = Math.Max(0, value.Right);
						value.Top = Math.Max(0, value.Top);
						value.Bottom = Math.Max(0, value.Bottom);
					}
				}
				this.PaddingInternal = value;
			}
		}

		// Token: 0x17000645 RID: 1605
		// (set) Token: 0x06001B39 RID: 6969 RVA: 0x00087C40 File Offset: 0x00085E40
		internal Padding PaddingInternal
		{
			set
			{
				if (value != this.Padding)
				{
					this.Properties.SetPadding(DataGridViewCellStyle.PropPadding, value);
					this.OnPropertyChanged(DataGridViewCellStyle.DataGridViewCellStylePropertyInternal.Other);
				}
			}
		}

		// Token: 0x17000646 RID: 1606
		// (get) Token: 0x06001B3A RID: 6970 RVA: 0x00087C68 File Offset: 0x00085E68
		internal PropertyStore Properties
		{
			get
			{
				return this.propertyStore;
			}
		}

		// Token: 0x17000647 RID: 1607
		// (get) Token: 0x06001B3B RID: 6971 RVA: 0x00087C70 File Offset: 0x00085E70
		// (set) Token: 0x06001B3C RID: 6972 RVA: 0x00087C78 File Offset: 0x00085E78
		internal DataGridViewCellStyleScopes Scope
		{
			get
			{
				return this.scope;
			}
			set
			{
				this.scope = value;
			}
		}

		/// <summary>Gets or sets the background color used by a <see cref="T:System.Windows.Forms.DataGridView" /> cell when it is selected.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that represents the background color of a selected cell. The default is <see cref="F:System.Drawing.Color.Empty" />.</returns>
		// Token: 0x17000648 RID: 1608
		// (get) Token: 0x06001B3D RID: 6973 RVA: 0x00087C81 File Offset: 0x00085E81
		// (set) Token: 0x06001B3E RID: 6974 RVA: 0x00087C94 File Offset: 0x00085E94
		[SRCategory("CatAppearance")]
		public Color SelectionBackColor
		{
			get
			{
				return this.Properties.GetColor(DataGridViewCellStyle.PropSelectionBackColor);
			}
			set
			{
				Color selectionBackColor = this.SelectionBackColor;
				if (!value.IsEmpty || this.Properties.ContainsObject(DataGridViewCellStyle.PropSelectionBackColor))
				{
					this.Properties.SetColor(DataGridViewCellStyle.PropSelectionBackColor, value);
				}
				if (!selectionBackColor.Equals(this.SelectionBackColor))
				{
					this.OnPropertyChanged(DataGridViewCellStyle.DataGridViewCellStylePropertyInternal.Color);
				}
			}
		}

		/// <summary>Gets or sets the foreground color used by a <see cref="T:System.Windows.Forms.DataGridView" /> cell when it is selected.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that represents the foreground color of a selected cell. The default is <see cref="F:System.Drawing.Color.Empty" />.</returns>
		// Token: 0x17000649 RID: 1609
		// (get) Token: 0x06001B3F RID: 6975 RVA: 0x00087CF5 File Offset: 0x00085EF5
		// (set) Token: 0x06001B40 RID: 6976 RVA: 0x00087D08 File Offset: 0x00085F08
		[SRCategory("CatAppearance")]
		public Color SelectionForeColor
		{
			get
			{
				return this.Properties.GetColor(DataGridViewCellStyle.PropSelectionForeColor);
			}
			set
			{
				Color selectionForeColor = this.SelectionForeColor;
				if (!value.IsEmpty || this.Properties.ContainsObject(DataGridViewCellStyle.PropSelectionForeColor))
				{
					this.Properties.SetColor(DataGridViewCellStyle.PropSelectionForeColor, value);
				}
				if (!selectionForeColor.Equals(this.SelectionForeColor))
				{
					this.OnPropertyChanged(DataGridViewCellStyle.DataGridViewCellStylePropertyInternal.Color);
				}
			}
		}

		/// <summary>Gets or sets an object that contains additional data related to the <see cref="T:System.Windows.Forms.DataGridViewCellStyle" />.</summary>
		/// <returns>An object that contains additional data. The default is <see langword="null" />.</returns>
		// Token: 0x1700064A RID: 1610
		// (get) Token: 0x06001B41 RID: 6977 RVA: 0x00087D69 File Offset: 0x00085F69
		// (set) Token: 0x06001B42 RID: 6978 RVA: 0x00087D7B File Offset: 0x00085F7B
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public object Tag
		{
			get
			{
				return this.Properties.GetObject(DataGridViewCellStyle.PropTag);
			}
			set
			{
				if (value != null || this.Properties.ContainsObject(DataGridViewCellStyle.PropTag))
				{
					this.Properties.SetObject(DataGridViewCellStyle.PropTag, value);
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether textual content in a <see cref="T:System.Windows.Forms.DataGridView" /> cell is wrapped to subsequent lines or truncated when it is too long to fit on a single line.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.DataGridViewTriState" /> values. The default is <see cref="F:System.Windows.Forms.DataGridViewTriState.NotSet" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The property value is not a valid <see cref="T:System.Windows.Forms.DataGridViewTriState" /> value. </exception>
		// Token: 0x1700064B RID: 1611
		// (get) Token: 0x06001B43 RID: 6979 RVA: 0x00087DA4 File Offset: 0x00085FA4
		// (set) Token: 0x06001B44 RID: 6980 RVA: 0x00087DCA File Offset: 0x00085FCA
		[DefaultValue(DataGridViewTriState.NotSet)]
		[SRCategory("CatLayout")]
		public DataGridViewTriState WrapMode
		{
			get
			{
				bool flag;
				int integer = this.Properties.GetInteger(DataGridViewCellStyle.PropWrapMode, out flag);
				if (flag)
				{
					return (DataGridViewTriState)integer;
				}
				return DataGridViewTriState.NotSet;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(DataGridViewTriState));
				}
				this.WrapModeInternal = value;
			}
		}

		// Token: 0x1700064C RID: 1612
		// (set) Token: 0x06001B45 RID: 6981 RVA: 0x00087DF9 File Offset: 0x00085FF9
		internal DataGridViewTriState WrapModeInternal
		{
			set
			{
				if (this.WrapMode != value)
				{
					this.Properties.SetInteger(DataGridViewCellStyle.PropWrapMode, (int)value);
					this.OnPropertyChanged(DataGridViewCellStyle.DataGridViewCellStylePropertyInternal.Other);
				}
			}
		}

		// Token: 0x06001B46 RID: 6982 RVA: 0x00087E1C File Offset: 0x0008601C
		internal void AddScope(DataGridView dataGridView, DataGridViewCellStyleScopes scope)
		{
			this.scope |= scope;
			this.dataGridView = dataGridView;
		}

		/// <summary>Applies the specified <see cref="T:System.Windows.Forms.DataGridViewCellStyle" /> to the current <see cref="T:System.Windows.Forms.DataGridViewCellStyle" />.</summary>
		/// <param name="dataGridViewCellStyle">The <see cref="T:System.Windows.Forms.DataGridViewCellStyle" /> to apply to the current <see cref="T:System.Windows.Forms.DataGridViewCellStyle" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="dataGridViewCellStyle" /> is <see langword="null" />.</exception>
		// Token: 0x06001B47 RID: 6983 RVA: 0x00087E34 File Offset: 0x00086034
		public virtual void ApplyStyle(DataGridViewCellStyle dataGridViewCellStyle)
		{
			if (dataGridViewCellStyle == null)
			{
				throw new ArgumentNullException("dataGridViewCellStyle");
			}
			if (!dataGridViewCellStyle.BackColor.IsEmpty)
			{
				this.BackColor = dataGridViewCellStyle.BackColor;
			}
			if (!dataGridViewCellStyle.ForeColor.IsEmpty)
			{
				this.ForeColor = dataGridViewCellStyle.ForeColor;
			}
			if (!dataGridViewCellStyle.SelectionBackColor.IsEmpty)
			{
				this.SelectionBackColor = dataGridViewCellStyle.SelectionBackColor;
			}
			if (!dataGridViewCellStyle.SelectionForeColor.IsEmpty)
			{
				this.SelectionForeColor = dataGridViewCellStyle.SelectionForeColor;
			}
			if (dataGridViewCellStyle.Font != null)
			{
				this.Font = dataGridViewCellStyle.Font;
			}
			if (!dataGridViewCellStyle.IsNullValueDefault)
			{
				this.NullValue = dataGridViewCellStyle.NullValue;
			}
			if (!dataGridViewCellStyle.IsDataSourceNullValueDefault)
			{
				this.DataSourceNullValue = dataGridViewCellStyle.DataSourceNullValue;
			}
			if (dataGridViewCellStyle.Format.Length != 0)
			{
				this.Format = dataGridViewCellStyle.Format;
			}
			if (!dataGridViewCellStyle.IsFormatProviderDefault)
			{
				this.FormatProvider = dataGridViewCellStyle.FormatProvider;
			}
			if (dataGridViewCellStyle.Alignment != DataGridViewContentAlignment.NotSet)
			{
				this.AlignmentInternal = dataGridViewCellStyle.Alignment;
			}
			if (dataGridViewCellStyle.WrapMode != DataGridViewTriState.NotSet)
			{
				this.WrapModeInternal = dataGridViewCellStyle.WrapMode;
			}
			if (dataGridViewCellStyle.Tag != null)
			{
				this.Tag = dataGridViewCellStyle.Tag;
			}
			if (dataGridViewCellStyle.Padding != Padding.Empty)
			{
				this.PaddingInternal = dataGridViewCellStyle.Padding;
			}
		}

		/// <summary>Creates an exact copy of this <see cref="T:System.Windows.Forms.DataGridViewCellStyle" />.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.DataGridViewCellStyle" /> that represents an exact copy of this cell style.</returns>
		// Token: 0x06001B48 RID: 6984 RVA: 0x00087F82 File Offset: 0x00086182
		public virtual DataGridViewCellStyle Clone()
		{
			return new DataGridViewCellStyle(this);
		}

		/// <summary>Returns a value indicating whether this instance is equivalent to the specified object.</summary>
		/// <param name="o">An object to compare with this instance, or <see langword="null" />. </param>
		/// <returns>
		///     <see langword="true" /> if <paramref name="o" /> is a <see cref="T:System.Windows.Forms.DataGridViewCellStyle" /> and has the same property values as this instance; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001B49 RID: 6985 RVA: 0x00087F8C File Offset: 0x0008618C
		public override bool Equals(object o)
		{
			DataGridViewCellStyle dataGridViewCellStyle = o as DataGridViewCellStyle;
			return dataGridViewCellStyle != null && this.GetDifferencesFrom(dataGridViewCellStyle) == DataGridViewCellStyleDifferences.None;
		}

		// Token: 0x06001B4A RID: 6986 RVA: 0x00087FB0 File Offset: 0x000861B0
		internal DataGridViewCellStyleDifferences GetDifferencesFrom(DataGridViewCellStyle dgvcs)
		{
			bool flag = dgvcs.Alignment != this.Alignment || dgvcs.DataSourceNullValue != this.DataSourceNullValue || dgvcs.Font != this.Font || dgvcs.Format != this.Format || dgvcs.FormatProvider != this.FormatProvider || dgvcs.NullValue != this.NullValue || dgvcs.Padding != this.Padding || dgvcs.Tag != this.Tag || dgvcs.WrapMode != this.WrapMode;
			bool flag2 = dgvcs.BackColor != this.BackColor || dgvcs.ForeColor != this.ForeColor || dgvcs.SelectionBackColor != this.SelectionBackColor || dgvcs.SelectionForeColor != this.SelectionForeColor;
			if (flag)
			{
				return DataGridViewCellStyleDifferences.AffectPreferredSize;
			}
			if (flag2)
			{
				return DataGridViewCellStyleDifferences.DoNotAffectPreferredSize;
			}
			return DataGridViewCellStyleDifferences.None;
		}

		/// <summary>Serves as a hash function for a particular type.</summary>
		/// <returns>A hash code for the current <see cref="T:System.Object" />.</returns>
		// Token: 0x06001B4B RID: 6987 RVA: 0x000880A8 File Offset: 0x000862A8
		public override int GetHashCode()
		{
			return WindowsFormsUtils.GetCombinedHashCodes(new int[]
			{
				(int)this.Alignment,
				(int)this.WrapMode,
				this.Padding.GetHashCode(),
				this.Format.GetHashCode(),
				this.BackColor.GetHashCode(),
				this.ForeColor.GetHashCode(),
				this.SelectionBackColor.GetHashCode(),
				this.SelectionForeColor.GetHashCode(),
				(this.Font == null) ? 1 : this.Font.GetHashCode(),
				(this.NullValue == null) ? 1 : this.NullValue.GetHashCode(),
				(this.DataSourceNullValue == null) ? 1 : this.DataSourceNullValue.GetHashCode(),
				(this.Tag == null) ? 1 : this.Tag.GetHashCode()
			});
		}

		// Token: 0x06001B4C RID: 6988 RVA: 0x000881BB File Offset: 0x000863BB
		private void OnPropertyChanged(DataGridViewCellStyle.DataGridViewCellStylePropertyInternal property)
		{
			if (this.dataGridView != null && this.scope != DataGridViewCellStyleScopes.None)
			{
				this.dataGridView.OnCellStyleContentChanged(this, property);
			}
		}

		// Token: 0x06001B4D RID: 6989 RVA: 0x000881DA File Offset: 0x000863DA
		internal void RemoveScope(DataGridViewCellStyleScopes scope)
		{
			this.scope &= ~scope;
			if (this.scope == DataGridViewCellStyleScopes.None)
			{
				this.dataGridView = null;
			}
		}

		// Token: 0x06001B4E RID: 6990 RVA: 0x000881FC File Offset: 0x000863FC
		private bool ShouldSerializeBackColor()
		{
			bool result;
			this.Properties.GetColor(DataGridViewCellStyle.PropBackColor, out result);
			return result;
		}

		// Token: 0x06001B4F RID: 6991 RVA: 0x0008821D File Offset: 0x0008641D
		private bool ShouldSerializeFont()
		{
			return this.Properties.GetObject(DataGridViewCellStyle.PropFont) != null;
		}

		// Token: 0x06001B50 RID: 6992 RVA: 0x00088234 File Offset: 0x00086434
		private bool ShouldSerializeForeColor()
		{
			bool result;
			this.Properties.GetColor(DataGridViewCellStyle.PropForeColor, out result);
			return result;
		}

		// Token: 0x06001B51 RID: 6993 RVA: 0x00088255 File Offset: 0x00086455
		private bool ShouldSerializeFormatProvider()
		{
			return this.Properties.GetObject(DataGridViewCellStyle.PropFormatProvider) != null;
		}

		// Token: 0x06001B52 RID: 6994 RVA: 0x0008826A File Offset: 0x0008646A
		private bool ShouldSerializePadding()
		{
			return this.Padding != Padding.Empty;
		}

		// Token: 0x06001B53 RID: 6995 RVA: 0x0008827C File Offset: 0x0008647C
		private bool ShouldSerializeSelectionBackColor()
		{
			bool result;
			this.Properties.GetObject(DataGridViewCellStyle.PropSelectionBackColor, out result);
			return result;
		}

		// Token: 0x06001B54 RID: 6996 RVA: 0x000882A0 File Offset: 0x000864A0
		private bool ShouldSerializeSelectionForeColor()
		{
			bool result;
			this.Properties.GetColor(DataGridViewCellStyle.PropSelectionForeColor, out result);
			return result;
		}

		/// <summary>Returns a string indicating the current property settings of the <see cref="T:System.Windows.Forms.DataGridViewCellStyle" />.</summary>
		/// <returns>A string indicating the current property settings of the <see cref="T:System.Windows.Forms.DataGridViewCellStyle" />.</returns>
		// Token: 0x06001B55 RID: 6997 RVA: 0x000882C4 File Offset: 0x000864C4
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder(128);
			stringBuilder.Append("DataGridViewCellStyle {");
			bool flag = true;
			if (this.BackColor != Color.Empty)
			{
				stringBuilder.Append(" BackColor=" + this.BackColor.ToString());
				flag = false;
			}
			if (this.ForeColor != Color.Empty)
			{
				if (!flag)
				{
					stringBuilder.Append(",");
				}
				stringBuilder.Append(" ForeColor=" + this.ForeColor.ToString());
				flag = false;
			}
			if (this.SelectionBackColor != Color.Empty)
			{
				if (!flag)
				{
					stringBuilder.Append(",");
				}
				stringBuilder.Append(" SelectionBackColor=" + this.SelectionBackColor.ToString());
				flag = false;
			}
			if (this.SelectionForeColor != Color.Empty)
			{
				if (!flag)
				{
					stringBuilder.Append(",");
				}
				stringBuilder.Append(" SelectionForeColor=" + this.SelectionForeColor.ToString());
				flag = false;
			}
			if (this.Font != null)
			{
				if (!flag)
				{
					stringBuilder.Append(",");
				}
				stringBuilder.Append(" Font=" + this.Font.ToString());
				flag = false;
			}
			if (!this.IsNullValueDefault && this.NullValue != null)
			{
				if (!flag)
				{
					stringBuilder.Append(",");
				}
				stringBuilder.Append(" NullValue=" + this.NullValue.ToString());
				flag = false;
			}
			if (!this.IsDataSourceNullValueDefault && this.DataSourceNullValue != null)
			{
				if (!flag)
				{
					stringBuilder.Append(",");
				}
				stringBuilder.Append(" DataSourceNullValue=" + this.DataSourceNullValue.ToString());
				flag = false;
			}
			if (!string.IsNullOrEmpty(this.Format))
			{
				if (!flag)
				{
					stringBuilder.Append(",");
				}
				stringBuilder.Append(" Format=" + this.Format);
				flag = false;
			}
			if (this.WrapMode != DataGridViewTriState.NotSet)
			{
				if (!flag)
				{
					stringBuilder.Append(",");
				}
				stringBuilder.Append(" WrapMode=" + this.WrapMode.ToString());
				flag = false;
			}
			if (this.Alignment != DataGridViewContentAlignment.NotSet)
			{
				if (!flag)
				{
					stringBuilder.Append(",");
				}
				stringBuilder.Append(" Alignment=" + this.Alignment.ToString());
				flag = false;
			}
			if (this.Padding != Padding.Empty)
			{
				if (!flag)
				{
					stringBuilder.Append(",");
				}
				stringBuilder.Append(" Padding=" + this.Padding.ToString());
				flag = false;
			}
			if (this.Tag != null)
			{
				if (!flag)
				{
					stringBuilder.Append(",");
				}
				stringBuilder.Append(" Tag=" + this.Tag.ToString());
			}
			stringBuilder.Append(" }");
			return stringBuilder.ToString();
		}

		/// <summary>Creates an exact copy of this <see cref="T:System.Windows.Forms.DataGridViewCellStyle" />.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.DataGridViewCellStyle" /> that represents an exact copy of this cell style.</returns>
		// Token: 0x06001B56 RID: 6998 RVA: 0x000885EC File Offset: 0x000867EC
		object ICloneable.Clone()
		{
			return this.Clone();
		}

		// Token: 0x04000C2D RID: 3117
		private static readonly int PropAlignment = PropertyStore.CreateKey();

		// Token: 0x04000C2E RID: 3118
		private static readonly int PropBackColor = PropertyStore.CreateKey();

		// Token: 0x04000C2F RID: 3119
		private static readonly int PropDataSourceNullValue = PropertyStore.CreateKey();

		// Token: 0x04000C30 RID: 3120
		private static readonly int PropFont = PropertyStore.CreateKey();

		// Token: 0x04000C31 RID: 3121
		private static readonly int PropForeColor = PropertyStore.CreateKey();

		// Token: 0x04000C32 RID: 3122
		private static readonly int PropFormat = PropertyStore.CreateKey();

		// Token: 0x04000C33 RID: 3123
		private static readonly int PropFormatProvider = PropertyStore.CreateKey();

		// Token: 0x04000C34 RID: 3124
		private static readonly int PropNullValue = PropertyStore.CreateKey();

		// Token: 0x04000C35 RID: 3125
		private static readonly int PropPadding = PropertyStore.CreateKey();

		// Token: 0x04000C36 RID: 3126
		private static readonly int PropSelectionBackColor = PropertyStore.CreateKey();

		// Token: 0x04000C37 RID: 3127
		private static readonly int PropSelectionForeColor = PropertyStore.CreateKey();

		// Token: 0x04000C38 RID: 3128
		private static readonly int PropTag = PropertyStore.CreateKey();

		// Token: 0x04000C39 RID: 3129
		private static readonly int PropWrapMode = PropertyStore.CreateKey();

		// Token: 0x04000C3A RID: 3130
		private const string DATAGRIDVIEWCELLSTYLE_nullText = "";

		// Token: 0x04000C3B RID: 3131
		private DataGridViewCellStyleScopes scope;

		// Token: 0x04000C3C RID: 3132
		private PropertyStore propertyStore;

		// Token: 0x04000C3D RID: 3133
		private DataGridView dataGridView;

		// Token: 0x020005AC RID: 1452
		internal enum DataGridViewCellStylePropertyInternal
		{
			// Token: 0x04003929 RID: 14633
			Color,
			// Token: 0x0400392A RID: 14634
			Other,
			// Token: 0x0400392B RID: 14635
			Font,
			// Token: 0x0400392C RID: 14636
			ForeColor
		}
	}
}
