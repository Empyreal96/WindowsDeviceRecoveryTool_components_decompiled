using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	/// <summary>Displays a graphic in a <see cref="T:System.Windows.Forms.DataGridView" /> control. </summary>
	// Token: 0x020001EE RID: 494
	public class DataGridViewImageCell : DataGridViewCell
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DataGridViewImageCell" /> class, configuring it for use with cell values other than <see cref="T:System.Drawing.Icon" /> objects.</summary>
		// Token: 0x06001DE7 RID: 7655 RVA: 0x0009436E File Offset: 0x0009256E
		public DataGridViewImageCell() : this(false)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DataGridViewImageCell" /> class, optionally configuring it for use with <see cref="T:System.Drawing.Icon" /> cell values.</summary>
		/// <param name="valueIsIcon">The cell will display an <see cref="T:System.Drawing.Icon" /> value.</param>
		// Token: 0x06001DE8 RID: 7656 RVA: 0x00094377 File Offset: 0x00092577
		public DataGridViewImageCell(bool valueIsIcon)
		{
			if (valueIsIcon)
			{
				this.flags = 1;
			}
		}

		/// <summary>Gets the default value that is used when creating a new row.</summary>
		/// <returns>An object containing a default image placeholder, or <see langword="null" /> to display an empty cell.</returns>
		// Token: 0x170006F9 RID: 1785
		// (get) Token: 0x06001DE9 RID: 7657 RVA: 0x00094389 File Offset: 0x00092589
		public override object DefaultNewRowValue
		{
			get
			{
				if (DataGridViewImageCell.defaultTypeImage.IsAssignableFrom(this.ValueType))
				{
					return DataGridViewImageCell.ErrorBitmap;
				}
				if (DataGridViewImageCell.defaultTypeIcon.IsAssignableFrom(this.ValueType))
				{
					return DataGridViewImageCell.ErrorIcon;
				}
				return null;
			}
		}

		/// <summary>Gets or sets the text associated with the image.</summary>
		/// <returns>The text associated with the image displayed in the cell.</returns>
		// Token: 0x170006FA RID: 1786
		// (get) Token: 0x06001DEA RID: 7658 RVA: 0x000943BC File Offset: 0x000925BC
		// (set) Token: 0x06001DEB RID: 7659 RVA: 0x000943E9 File Offset: 0x000925E9
		[DefaultValue("")]
		public string Description
		{
			get
			{
				object @object = base.Properties.GetObject(DataGridViewImageCell.PropImageCellDescription);
				if (@object != null)
				{
					return (string)@object;
				}
				return string.Empty;
			}
			set
			{
				if (!string.IsNullOrEmpty(value) || base.Properties.ContainsObject(DataGridViewImageCell.PropImageCellDescription))
				{
					base.Properties.SetObject(DataGridViewImageCell.PropImageCellDescription, value);
				}
			}
		}

		/// <summary>Gets the type of the cell's hosted editing control. </summary>
		/// <returns>The <see cref="T:System.Type" /> of the underlying editing control. As implemented in this class, this property is always <see langword="null" />.</returns>
		// Token: 0x170006FB RID: 1787
		// (get) Token: 0x06001DEC RID: 7660 RVA: 0x0000DE5C File Offset: 0x0000C05C
		public override Type EditType
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170006FC RID: 1788
		// (get) Token: 0x06001DED RID: 7661 RVA: 0x00094416 File Offset: 0x00092616
		internal static Bitmap ErrorBitmap
		{
			get
			{
				if (DataGridViewImageCell.errorBmp == null)
				{
					DataGridViewImageCell.errorBmp = new Bitmap(typeof(DataGridView), "ImageInError.bmp");
				}
				return DataGridViewImageCell.errorBmp;
			}
		}

		// Token: 0x170006FD RID: 1789
		// (get) Token: 0x06001DEE RID: 7662 RVA: 0x0009443D File Offset: 0x0009263D
		internal static Icon ErrorIcon
		{
			get
			{
				if (DataGridViewImageCell.errorIco == null)
				{
					DataGridViewImageCell.errorIco = new Icon(typeof(DataGridView), "IconInError.ico");
				}
				return DataGridViewImageCell.errorIco;
			}
		}

		/// <summary>Gets the type of the formatted value associated with the cell.</summary>
		/// <returns>A <see cref="T:System.Type" /> object representing display value type of the cell, which is the <see cref="T:System.Drawing.Image" /> type if the <see cref="P:System.Windows.Forms.DataGridViewImageCell.ValueIsIcon" /> property is set to <see langword="false" /> or the <see cref="T:System.Drawing.Icon" /> type otherwise.</returns>
		// Token: 0x170006FE RID: 1790
		// (get) Token: 0x06001DEF RID: 7663 RVA: 0x00094464 File Offset: 0x00092664
		public override Type FormattedValueType
		{
			get
			{
				if (this.ValueIsIcon)
				{
					return DataGridViewImageCell.defaultTypeIcon;
				}
				return DataGridViewImageCell.defaultTypeImage;
			}
		}

		/// <summary>Gets or sets the graphics layout for the cell. </summary>
		/// <returns>A <see cref="T:System.Windows.Forms.DataGridViewImageCellLayout" /> for this cell. The default is <see cref="F:System.Windows.Forms.DataGridViewImageCellLayout.NotSet" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The supplied <see cref="T:System.Windows.Forms.DataGridViewImageCellLayout" /> value is invalid. </exception>
		// Token: 0x170006FF RID: 1791
		// (get) Token: 0x06001DF0 RID: 7664 RVA: 0x0009447C File Offset: 0x0009267C
		// (set) Token: 0x06001DF1 RID: 7665 RVA: 0x000944A4 File Offset: 0x000926A4
		[DefaultValue(DataGridViewImageCellLayout.NotSet)]
		public DataGridViewImageCellLayout ImageLayout
		{
			get
			{
				bool flag;
				int integer = base.Properties.GetInteger(DataGridViewImageCell.PropImageCellLayout, out flag);
				if (flag)
				{
					return (DataGridViewImageCellLayout)integer;
				}
				return DataGridViewImageCellLayout.Normal;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 3))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(DataGridViewImageCellLayout));
				}
				if (this.ImageLayout != value)
				{
					base.Properties.SetInteger(DataGridViewImageCell.PropImageCellLayout, (int)value);
					base.OnCommonChange();
				}
			}
		}

		// Token: 0x17000700 RID: 1792
		// (set) Token: 0x06001DF2 RID: 7666 RVA: 0x000944F7 File Offset: 0x000926F7
		internal DataGridViewImageCellLayout ImageLayoutInternal
		{
			set
			{
				if (this.ImageLayout != value)
				{
					base.Properties.SetInteger(DataGridViewImageCell.PropImageCellLayout, (int)value);
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether this cell displays an <see cref="T:System.Drawing.Icon" /> value.</summary>
		/// <returns>
		///     <see langword="true" /> if this cell displays an <see cref="T:System.Drawing.Icon" /> value; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000701 RID: 1793
		// (get) Token: 0x06001DF3 RID: 7667 RVA: 0x00094513 File Offset: 0x00092713
		// (set) Token: 0x06001DF4 RID: 7668 RVA: 0x00094520 File Offset: 0x00092720
		[DefaultValue(false)]
		public bool ValueIsIcon
		{
			get
			{
				return (this.flags & 1) > 0;
			}
			set
			{
				if (this.ValueIsIcon != value)
				{
					this.ValueIsIconInternal = value;
					if (base.DataGridView != null)
					{
						if (base.RowIndex != -1)
						{
							base.DataGridView.InvalidateCell(this);
							return;
						}
						base.DataGridView.InvalidateColumnInternal(base.ColumnIndex);
					}
				}
			}
		}

		// Token: 0x17000702 RID: 1794
		// (set) Token: 0x06001DF5 RID: 7669 RVA: 0x0009456C File Offset: 0x0009276C
		internal bool ValueIsIconInternal
		{
			set
			{
				if (this.ValueIsIcon != value)
				{
					if (value)
					{
						this.flags |= 1;
					}
					else
					{
						this.flags = (byte)((int)this.flags & -2);
					}
					if (base.DataGridView != null && base.RowIndex != -1 && base.DataGridView.NewRowIndex == base.RowIndex && !base.DataGridView.VirtualMode && ((value && base.Value == DataGridViewImageCell.ErrorBitmap) || (!value && base.Value == DataGridViewImageCell.ErrorIcon)))
					{
						base.Value = this.DefaultNewRowValue;
					}
				}
			}
		}

		/// <summary>Gets or sets the data type of the values in the cell. </summary>
		/// <returns>The <see cref="T:System.Type" /> of the cell's value.</returns>
		// Token: 0x17000703 RID: 1795
		// (get) Token: 0x06001DF6 RID: 7670 RVA: 0x00094608 File Offset: 0x00092808
		// (set) Token: 0x06001DF7 RID: 7671 RVA: 0x0009463A File Offset: 0x0009283A
		public override Type ValueType
		{
			get
			{
				Type valueType = base.ValueType;
				if (valueType != null)
				{
					return valueType;
				}
				if (this.ValueIsIcon)
				{
					return DataGridViewImageCell.defaultTypeIcon;
				}
				return DataGridViewImageCell.defaultTypeImage;
			}
			set
			{
				base.ValueType = value;
				this.ValueIsIcon = (value != null && DataGridViewImageCell.defaultTypeIcon.IsAssignableFrom(value));
			}
		}

		/// <summary>Creates an exact copy of this cell.</summary>
		/// <returns>An <see cref="T:System.Object" /> that represents the cloned <see cref="T:System.Windows.Forms.DataGridViewImageCell" />.</returns>
		// Token: 0x06001DF8 RID: 7672 RVA: 0x00094660 File Offset: 0x00092860
		public override object Clone()
		{
			Type type = base.GetType();
			DataGridViewImageCell dataGridViewImageCell;
			if (type == DataGridViewImageCell.cellType)
			{
				dataGridViewImageCell = new DataGridViewImageCell();
			}
			else
			{
				dataGridViewImageCell = (DataGridViewImageCell)Activator.CreateInstance(type);
			}
			base.CloneInternal(dataGridViewImageCell);
			dataGridViewImageCell.ValueIsIconInternal = this.ValueIsIcon;
			dataGridViewImageCell.Description = this.Description;
			dataGridViewImageCell.ImageLayoutInternal = this.ImageLayout;
			return dataGridViewImageCell;
		}

		/// <summary>Creates a new accessible object for the <see cref="T:System.Windows.Forms.DataGridViewImageCell" />. </summary>
		/// <returns>A new <see cref="T:System.Windows.Forms.DataGridViewImageCell.DataGridViewImageCellAccessibleObject" /> for the <see cref="T:System.Windows.Forms.DataGridViewImageCell" />. </returns>
		// Token: 0x06001DF9 RID: 7673 RVA: 0x000946C1 File Offset: 0x000928C1
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			return new DataGridViewImageCell.DataGridViewImageCellAccessibleObject(this);
		}

		/// <summary>Returns the bounding rectangle that encloses the cell's content area, which is calculated using the specified <see cref="T:System.Drawing.Graphics" /> and cell style.</summary>
		/// <param name="graphics">The graphics context for the cell.</param>
		/// <param name="cellStyle">The <see cref="T:System.Windows.Forms.DataGridViewCellStyle" /> to be applied to the cell.</param>
		/// <param name="rowIndex">The index of the cell's parent row.</param>
		/// <returns>The <see cref="T:System.Drawing.Rectangle" /> that bounds the cell's contents.</returns>
		// Token: 0x06001DFA RID: 7674 RVA: 0x000946CC File Offset: 0x000928CC
		protected override Rectangle GetContentBounds(Graphics graphics, DataGridViewCellStyle cellStyle, int rowIndex)
		{
			if (cellStyle == null)
			{
				throw new ArgumentNullException("cellStyle");
			}
			if (base.DataGridView == null || rowIndex < 0 || base.OwningColumn == null)
			{
				return Rectangle.Empty;
			}
			object value = this.GetValue(rowIndex);
			object formattedValue = this.GetFormattedValue(value, rowIndex, ref cellStyle, null, null, DataGridViewDataErrorContexts.Formatting);
			DataGridViewAdvancedBorderStyle advancedBorderStyle;
			DataGridViewElementStates elementState;
			Rectangle rectangle;
			base.ComputeBorderStyleCellStateAndCellBounds(rowIndex, out advancedBorderStyle, out elementState, out rectangle);
			return this.PaintPrivate(graphics, rectangle, rectangle, rowIndex, elementState, formattedValue, null, cellStyle, advancedBorderStyle, DataGridViewPaintParts.ContentForeground, true, false, false);
		}

		/// <summary>Returns the bounding rectangle that encloses the cell's error icon, if one is displayed.</summary>
		/// <param name="graphics">The graphics context for the cell.</param>
		/// <param name="cellStyle">The <see cref="T:System.Windows.Forms.DataGridViewCellStyle" /> to be applied to the cell.</param>
		/// <param name="rowIndex">The index of the cell's parent row.</param>
		/// <returns>The <see cref="T:System.Drawing.Rectangle" /> that bounds the cell's error icon, if one is displayed; otherwise, <see cref="F:System.Drawing.Rectangle.Empty" />.</returns>
		// Token: 0x06001DFB RID: 7675 RVA: 0x00094740 File Offset: 0x00092940
		protected override Rectangle GetErrorIconBounds(Graphics graphics, DataGridViewCellStyle cellStyle, int rowIndex)
		{
			if (cellStyle == null)
			{
				throw new ArgumentNullException("cellStyle");
			}
			if (base.DataGridView == null || rowIndex < 0 || base.OwningColumn == null || !base.DataGridView.ShowCellErrors || string.IsNullOrEmpty(this.GetErrorText(rowIndex)))
			{
				return Rectangle.Empty;
			}
			object value = this.GetValue(rowIndex);
			object formattedValue = this.GetFormattedValue(value, rowIndex, ref cellStyle, null, null, DataGridViewDataErrorContexts.Formatting);
			DataGridViewAdvancedBorderStyle advancedBorderStyle;
			DataGridViewElementStates elementState;
			Rectangle rectangle;
			base.ComputeBorderStyleCellStateAndCellBounds(rowIndex, out advancedBorderStyle, out elementState, out rectangle);
			return this.PaintPrivate(graphics, rectangle, rectangle, rowIndex, elementState, formattedValue, this.GetErrorText(rowIndex), cellStyle, advancedBorderStyle, DataGridViewPaintParts.ContentForeground, false, true, false);
		}

		/// <summary>Returns a graphic as it would be displayed in the cell.</summary>
		/// <param name="value">The value to be formatted. </param>
		/// <param name="rowIndex">The index of the cell's parent row. </param>
		/// <param name="cellStyle">The <see cref="T:System.Windows.Forms.DataGridViewCellStyle" /> in effect for the cell. </param>
		/// <param name="valueTypeConverter">A <see cref="T:System.ComponentModel.TypeConverter" /> associated with the value type that provides custom conversion to the formatted value type, or <see langword="null" /> if no such custom conversion is needed.</param>
		/// <param name="formattedValueTypeConverter">A <see cref="T:System.ComponentModel.TypeConverter" /> associated with the formatted value type that provides custom conversion from the value type, or <see langword="null" /> if no such custom conversion is needed.</param>
		/// <param name="context">A bitwise combination of <see cref="T:System.Windows.Forms.DataGridViewDataErrorContexts" /> values describing the context in which the formatted value is needed. </param>
		/// <returns>An object that represents the formatted image.</returns>
		// Token: 0x06001DFC RID: 7676 RVA: 0x000947D4 File Offset: 0x000929D4
		protected override object GetFormattedValue(object value, int rowIndex, ref DataGridViewCellStyle cellStyle, TypeConverter valueTypeConverter, TypeConverter formattedValueTypeConverter, DataGridViewDataErrorContexts context)
		{
			if ((context & DataGridViewDataErrorContexts.ClipboardContent) != (DataGridViewDataErrorContexts)0)
			{
				return this.Description;
			}
			object formattedValue = base.GetFormattedValue(value, rowIndex, ref cellStyle, valueTypeConverter, formattedValueTypeConverter, context);
			if (formattedValue == null && cellStyle.NullValue == null)
			{
				return null;
			}
			if (this.ValueIsIcon)
			{
				Icon icon = formattedValue as Icon;
				if (icon == null)
				{
					icon = DataGridViewImageCell.ErrorIcon;
				}
				return icon;
			}
			Image image = formattedValue as Image;
			if (image == null)
			{
				image = DataGridViewImageCell.ErrorBitmap;
			}
			return image;
		}

		/// <summary>Calculates the preferred size, in pixels, of the cell.</summary>
		/// <param name="graphics">The <see cref="T:System.Drawing.Graphics" /> used to draw the cell.</param>
		/// <param name="cellStyle">A <see cref="T:System.Windows.Forms.DataGridViewCellStyle" /> that represents the style of the cell.</param>
		/// <param name="rowIndex">The zero-based row index of the cell.</param>
		/// <param name="constraintSize">The cell's maximum allowable size.</param>
		/// <returns>A <see cref="T:System.Drawing.Size" /> that represents the preferred size, in pixels, of the cell.</returns>
		// Token: 0x06001DFD RID: 7677 RVA: 0x0009483C File Offset: 0x00092A3C
		protected override Size GetPreferredSize(Graphics graphics, DataGridViewCellStyle cellStyle, int rowIndex, Size constraintSize)
		{
			if (base.DataGridView == null)
			{
				return new Size(-1, -1);
			}
			if (cellStyle == null)
			{
				throw new ArgumentNullException("cellStyle");
			}
			Rectangle stdBorderWidths = base.StdBorderWidths;
			int num = stdBorderWidths.Left + stdBorderWidths.Width + cellStyle.Padding.Horizontal;
			int num2 = stdBorderWidths.Top + stdBorderWidths.Height + cellStyle.Padding.Vertical;
			DataGridViewFreeDimension freeDimensionFromConstraint = DataGridViewCell.GetFreeDimensionFromConstraint(constraintSize);
			object formattedValue = base.GetFormattedValue(rowIndex, ref cellStyle, DataGridViewDataErrorContexts.Formatting | DataGridViewDataErrorContexts.PreferredSize);
			Image image = formattedValue as Image;
			Icon icon = null;
			if (image == null)
			{
				icon = (formattedValue as Icon);
			}
			Size empty;
			if (freeDimensionFromConstraint == DataGridViewFreeDimension.Height && this.ImageLayout == DataGridViewImageCellLayout.Zoom)
			{
				if (image != null || icon != null)
				{
					if (image != null)
					{
						int num3 = constraintSize.Width - num;
						if (num3 <= 0 || image.Width == 0)
						{
							empty = Size.Empty;
						}
						else
						{
							empty = new Size(0, Math.Min(image.Height, decimal.ToInt32(image.Height * num3 / image.Width)));
						}
					}
					else
					{
						int num4 = constraintSize.Width - num;
						if (num4 <= 0 || icon.Width == 0)
						{
							empty = Size.Empty;
						}
						else
						{
							empty = new Size(0, Math.Min(icon.Height, decimal.ToInt32(icon.Height * num4 / icon.Width)));
						}
					}
				}
				else
				{
					empty = new Size(0, 1);
				}
			}
			else if (freeDimensionFromConstraint == DataGridViewFreeDimension.Width && this.ImageLayout == DataGridViewImageCellLayout.Zoom)
			{
				if (image != null || icon != null)
				{
					if (image != null)
					{
						int num5 = constraintSize.Height - num2;
						if (num5 <= 0 || image.Height == 0)
						{
							empty = Size.Empty;
						}
						else
						{
							empty = new Size(Math.Min(image.Width, decimal.ToInt32(image.Width * num5 / image.Height)), 0);
						}
					}
					else
					{
						int num6 = constraintSize.Height - num2;
						if (num6 <= 0 || icon.Height == 0)
						{
							empty = Size.Empty;
						}
						else
						{
							empty = new Size(Math.Min(icon.Width, decimal.ToInt32(icon.Width * num6 / icon.Height)), 0);
						}
					}
				}
				else
				{
					empty = new Size(1, 0);
				}
			}
			else
			{
				if (image != null)
				{
					empty = new Size(image.Width, image.Height);
				}
				else if (icon != null)
				{
					empty = new Size(icon.Width, icon.Height);
				}
				else
				{
					empty = new Size(1, 1);
				}
				if (freeDimensionFromConstraint == DataGridViewFreeDimension.Height)
				{
					empty.Width = 0;
				}
				else if (freeDimensionFromConstraint == DataGridViewFreeDimension.Width)
				{
					empty.Height = 0;
				}
			}
			if (freeDimensionFromConstraint != DataGridViewFreeDimension.Height)
			{
				empty.Width += num;
				if (base.DataGridView.ShowCellErrors)
				{
					empty.Width = Math.Max(empty.Width, num + 8 + (int)DataGridViewCell.iconsWidth);
				}
			}
			if (freeDimensionFromConstraint != DataGridViewFreeDimension.Width)
			{
				empty.Height += num2;
				if (base.DataGridView.ShowCellErrors)
				{
					empty.Height = Math.Max(empty.Height, num2 + 8 + (int)DataGridViewCell.iconsHeight);
				}
			}
			return empty;
		}

		/// <summary>Gets the value of the cell.</summary>
		/// <param name="rowIndex">The index of the cell's parent row.</param>
		/// <returns>The value contained in the <see cref="T:System.Windows.Forms.DataGridViewCell" />.</returns>
		// Token: 0x06001DFE RID: 7678 RVA: 0x00094BBC File Offset: 0x00092DBC
		protected override object GetValue(int rowIndex)
		{
			object value = base.GetValue(rowIndex);
			if (value == null)
			{
				DataGridViewImageColumn dataGridViewImageColumn = base.OwningColumn as DataGridViewImageColumn;
				if (dataGridViewImageColumn != null)
				{
					if (DataGridViewImageCell.defaultTypeImage.IsAssignableFrom(this.ValueType))
					{
						Image image = dataGridViewImageColumn.Image;
						if (image != null)
						{
							return image;
						}
					}
					else if (DataGridViewImageCell.defaultTypeIcon.IsAssignableFrom(this.ValueType))
					{
						Icon icon = dataGridViewImageColumn.Icon;
						if (icon != null)
						{
							return icon;
						}
					}
				}
			}
			return value;
		}

		// Token: 0x06001DFF RID: 7679 RVA: 0x00094C20 File Offset: 0x00092E20
		private Rectangle ImgBounds(Rectangle bounds, int imgWidth, int imgHeight, DataGridViewImageCellLayout imageLayout, DataGridViewCellStyle cellStyle)
		{
			Rectangle empty = Rectangle.Empty;
			if (imageLayout > DataGridViewImageCellLayout.Normal)
			{
				if (imageLayout == DataGridViewImageCellLayout.Zoom)
				{
					if (imgWidth * bounds.Height < imgHeight * bounds.Width)
					{
						empty = new Rectangle(bounds.X, bounds.Y, decimal.ToInt32(imgWidth * bounds.Height / imgHeight), bounds.Height);
					}
					else
					{
						empty = new Rectangle(bounds.X, bounds.Y, bounds.Width, decimal.ToInt32(imgHeight * bounds.Width / imgWidth));
					}
				}
			}
			else
			{
				empty = new Rectangle(bounds.X, bounds.Y, imgWidth, imgHeight);
			}
			DataGridViewContentAlignment alignment;
			if (base.DataGridView.RightToLeftInternal)
			{
				alignment = cellStyle.Alignment;
				if (alignment <= DataGridViewContentAlignment.MiddleLeft)
				{
					if (alignment != DataGridViewContentAlignment.TopLeft)
					{
						if (alignment != DataGridViewContentAlignment.TopRight)
						{
							if (alignment == DataGridViewContentAlignment.MiddleLeft)
							{
								empty.X = bounds.Right - empty.Width;
							}
						}
						else
						{
							empty.X = bounds.X;
						}
					}
					else
					{
						empty.X = bounds.Right - empty.Width;
					}
				}
				else if (alignment != DataGridViewContentAlignment.MiddleRight)
				{
					if (alignment != DataGridViewContentAlignment.BottomLeft)
					{
						if (alignment == DataGridViewContentAlignment.BottomRight)
						{
							empty.X = bounds.X;
						}
					}
					else
					{
						empty.X = bounds.Right - empty.Width;
					}
				}
				else
				{
					empty.X = bounds.X;
				}
			}
			else
			{
				alignment = cellStyle.Alignment;
				if (alignment <= DataGridViewContentAlignment.MiddleLeft)
				{
					if (alignment != DataGridViewContentAlignment.TopLeft)
					{
						if (alignment != DataGridViewContentAlignment.TopRight)
						{
							if (alignment == DataGridViewContentAlignment.MiddleLeft)
							{
								empty.X = bounds.X;
							}
						}
						else
						{
							empty.X = bounds.Right - empty.Width;
						}
					}
					else
					{
						empty.X = bounds.X;
					}
				}
				else if (alignment != DataGridViewContentAlignment.MiddleRight)
				{
					if (alignment != DataGridViewContentAlignment.BottomLeft)
					{
						if (alignment == DataGridViewContentAlignment.BottomRight)
						{
							empty.X = bounds.Right - empty.Width;
						}
					}
					else
					{
						empty.X = bounds.X;
					}
				}
				else
				{
					empty.X = bounds.Right - empty.Width;
				}
			}
			alignment = cellStyle.Alignment;
			if (alignment == DataGridViewContentAlignment.TopCenter || alignment == DataGridViewContentAlignment.MiddleCenter || alignment == DataGridViewContentAlignment.BottomCenter)
			{
				empty.X = bounds.X + (bounds.Width - empty.Width) / 2;
			}
			alignment = cellStyle.Alignment;
			if (alignment > DataGridViewContentAlignment.MiddleCenter)
			{
				if (alignment <= DataGridViewContentAlignment.BottomLeft)
				{
					if (alignment == DataGridViewContentAlignment.MiddleRight)
					{
						goto IL_2E7;
					}
					if (alignment != DataGridViewContentAlignment.BottomLeft)
					{
						return empty;
					}
				}
				else if (alignment != DataGridViewContentAlignment.BottomCenter && alignment != DataGridViewContentAlignment.BottomRight)
				{
					return empty;
				}
				empty.Y = bounds.Bottom - empty.Height;
				return empty;
			}
			if (alignment <= DataGridViewContentAlignment.TopRight)
			{
				if (alignment - DataGridViewContentAlignment.TopLeft > 1 && alignment != DataGridViewContentAlignment.TopRight)
				{
					return empty;
				}
				empty.Y = bounds.Y;
				return empty;
			}
			else if (alignment != DataGridViewContentAlignment.MiddleLeft && alignment != DataGridViewContentAlignment.MiddleCenter)
			{
				return empty;
			}
			IL_2E7:
			empty.Y = bounds.Y + (bounds.Height - empty.Height) / 2;
			return empty;
		}

		/// <summary>Paints the current <see cref="T:System.Windows.Forms.DataGridViewImageCell" />.</summary>
		/// <param name="graphics">The <see cref="T:System.Drawing.Graphics" /> used to paint the cell.</param>
		/// <param name="clipBounds">A <see cref="T:System.Drawing.Rectangle" /> that represents the area of the <see cref="T:System.Windows.Forms.DataGridView" /> that needs to be repainted.</param>
		/// <param name="cellBounds">A <see cref="T:System.Drawing.Rectangle" /> that contains the bounds of the cell that is being painted.</param>
		/// <param name="rowIndex">The row index of the cell that is being painted.</param>
		/// <param name="elementState">A bitwise combination of <see cref="T:System.Windows.Forms.DataGridViewElementStates" /> values that specifies the state of the cell.</param>
		/// <param name="value">The data of the cell that is being painted.</param>
		/// <param name="formattedValue">The formatted data of the cell that is being painted.</param>
		/// <param name="errorText">An error message that is associated with the cell.</param>
		/// <param name="cellStyle">A <see cref="T:System.Windows.Forms.DataGridViewCellStyle" /> that contains formatting and style information about the cell.</param>
		/// <param name="advancedBorderStyle">A <see cref="T:System.Windows.Forms.DataGridViewAdvancedBorderStyle" /> that contains border styles for the cell that is being painted.</param>
		/// <param name="paintParts">A bitwise combination of the <see cref="T:System.Windows.Forms.DataGridViewPaintParts" /> values that specifies which parts of the cell need to be painted.</param>
		// Token: 0x06001E00 RID: 7680 RVA: 0x00094F50 File Offset: 0x00093150
		protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates elementState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
		{
			if (cellStyle == null)
			{
				throw new ArgumentNullException("cellStyle");
			}
			this.PaintPrivate(graphics, clipBounds, cellBounds, rowIndex, elementState, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts, false, false, true);
		}

		// Token: 0x06001E01 RID: 7681 RVA: 0x00094F88 File Offset: 0x00093188
		private Rectangle PaintPrivate(Graphics g, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates elementState, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts, bool computeContentBounds, bool computeErrorIconBounds, bool paint)
		{
			if (paint && DataGridViewCell.PaintBorder(paintParts))
			{
				this.PaintBorder(g, clipBounds, cellBounds, cellStyle, advancedBorderStyle);
			}
			Rectangle rectangle = cellBounds;
			Rectangle rectangle2 = this.BorderWidths(advancedBorderStyle);
			rectangle.Offset(rectangle2.X, rectangle2.Y);
			rectangle.Width -= rectangle2.Right;
			rectangle.Height -= rectangle2.Bottom;
			Rectangle result;
			if (rectangle.Width > 0 && rectangle.Height > 0 && (paint || computeContentBounds))
			{
				Rectangle rectangle3 = rectangle;
				if (cellStyle.Padding != Padding.Empty)
				{
					if (base.DataGridView.RightToLeftInternal)
					{
						rectangle3.Offset(cellStyle.Padding.Right, cellStyle.Padding.Top);
					}
					else
					{
						rectangle3.Offset(cellStyle.Padding.Left, cellStyle.Padding.Top);
					}
					rectangle3.Width -= cellStyle.Padding.Horizontal;
					rectangle3.Height -= cellStyle.Padding.Vertical;
				}
				bool flag = (elementState & DataGridViewElementStates.Selected) > DataGridViewElementStates.None;
				SolidBrush cachedBrush = base.DataGridView.GetCachedBrush((DataGridViewCell.PaintSelectionBackground(paintParts) && flag) ? cellStyle.SelectionBackColor : cellStyle.BackColor);
				if (rectangle3.Width > 0 && rectangle3.Height > 0)
				{
					Image image = formattedValue as Image;
					Icon icon = null;
					if (image == null)
					{
						icon = (formattedValue as Icon);
					}
					if (icon != null || image != null)
					{
						DataGridViewImageCellLayout dataGridViewImageCellLayout = this.ImageLayout;
						if (dataGridViewImageCellLayout == DataGridViewImageCellLayout.NotSet)
						{
							if (base.OwningColumn is DataGridViewImageColumn)
							{
								dataGridViewImageCellLayout = ((DataGridViewImageColumn)base.OwningColumn).ImageLayout;
							}
							else
							{
								dataGridViewImageCellLayout = DataGridViewImageCellLayout.Normal;
							}
						}
						if (dataGridViewImageCellLayout == DataGridViewImageCellLayout.Stretch)
						{
							if (paint)
							{
								if (DataGridViewCell.PaintBackground(paintParts))
								{
									DataGridViewCell.PaintPadding(g, rectangle, cellStyle, cachedBrush, base.DataGridView.RightToLeftInternal);
								}
								if (DataGridViewCell.PaintContentForeground(paintParts))
								{
									if (image != null)
									{
										ImageAttributes imageAttributes = new ImageAttributes();
										imageAttributes.SetWrapMode(WrapMode.TileFlipXY);
										g.DrawImage(image, rectangle3, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, imageAttributes);
										imageAttributes.Dispose();
									}
									else
									{
										g.DrawIcon(icon, rectangle3);
									}
								}
							}
							result = rectangle3;
						}
						else
						{
							Rectangle rectangle4 = this.ImgBounds(rectangle3, (image == null) ? icon.Width : image.Width, (image == null) ? icon.Height : image.Height, dataGridViewImageCellLayout, cellStyle);
							result = rectangle4;
							if (paint)
							{
								if (DataGridViewCell.PaintBackground(paintParts) && cachedBrush.Color.A == 255)
								{
									g.FillRectangle(cachedBrush, rectangle);
								}
								if (DataGridViewCell.PaintContentForeground(paintParts))
								{
									Region clip = g.Clip;
									g.SetClip(Rectangle.Intersect(Rectangle.Intersect(rectangle4, rectangle3), Rectangle.Truncate(g.VisibleClipBounds)));
									if (image != null)
									{
										g.DrawImage(image, rectangle4);
									}
									else
									{
										g.DrawIconUnstretched(icon, rectangle4);
									}
									g.Clip = clip;
								}
							}
						}
					}
					else
					{
						if (paint && DataGridViewCell.PaintBackground(paintParts) && cachedBrush.Color.A == 255)
						{
							g.FillRectangle(cachedBrush, rectangle);
						}
						result = Rectangle.Empty;
					}
				}
				else
				{
					if (paint && DataGridViewCell.PaintBackground(paintParts) && cachedBrush.Color.A == 255)
					{
						g.FillRectangle(cachedBrush, rectangle);
					}
					result = Rectangle.Empty;
				}
				Point currentCellAddress = base.DataGridView.CurrentCellAddress;
				if (paint && DataGridViewCell.PaintFocus(paintParts) && currentCellAddress.X == base.ColumnIndex && currentCellAddress.Y == rowIndex && base.DataGridView.ShowFocusCues && base.DataGridView.Focused)
				{
					ControlPaint.DrawFocusRectangle(g, rectangle, Color.Empty, cachedBrush.Color);
				}
				if (base.DataGridView.ShowCellErrors && paint && DataGridViewCell.PaintErrorIcon(paintParts))
				{
					base.PaintErrorIcon(g, cellStyle, rowIndex, cellBounds, rectangle, errorText);
				}
			}
			else if (computeErrorIconBounds)
			{
				if (!string.IsNullOrEmpty(errorText))
				{
					result = base.ComputeErrorIconBounds(rectangle);
				}
				else
				{
					result = Rectangle.Empty;
				}
			}
			else
			{
				result = Rectangle.Empty;
			}
			return result;
		}

		/// <summary>Returns a string that describes the current object. </summary>
		/// <returns>A string that represents the current object.
		/// </returns>
		// Token: 0x06001E02 RID: 7682 RVA: 0x000953C0 File Offset: 0x000935C0
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"DataGridViewImageCell { ColumnIndex=",
				base.ColumnIndex.ToString(CultureInfo.CurrentCulture),
				", RowIndex=",
				base.RowIndex.ToString(CultureInfo.CurrentCulture),
				" }"
			});
		}

		// Token: 0x04000D2D RID: 3373
		private static ColorMap[] colorMap = new ColorMap[]
		{
			new ColorMap()
		};

		// Token: 0x04000D2E RID: 3374
		private static readonly int PropImageCellDescription = PropertyStore.CreateKey();

		// Token: 0x04000D2F RID: 3375
		private static readonly int PropImageCellLayout = PropertyStore.CreateKey();

		// Token: 0x04000D30 RID: 3376
		private static Type defaultTypeImage = typeof(Image);

		// Token: 0x04000D31 RID: 3377
		private static Type defaultTypeIcon = typeof(Icon);

		// Token: 0x04000D32 RID: 3378
		private static Type cellType = typeof(DataGridViewImageCell);

		// Token: 0x04000D33 RID: 3379
		private static Bitmap errorBmp = null;

		// Token: 0x04000D34 RID: 3380
		private static Icon errorIco = null;

		// Token: 0x04000D35 RID: 3381
		private const byte DATAGRIDVIEWIMAGECELL_valueIsIcon = 1;

		// Token: 0x04000D36 RID: 3382
		private byte flags;

		/// <summary>Provides information about a <see cref="T:System.Windows.Forms.DataGridViewImageCell" /> to accessibility client applications.</summary>
		// Token: 0x020005B7 RID: 1463
		protected class DataGridViewImageCellAccessibleObject : DataGridViewCell.DataGridViewCellAccessibleObject
		{
			/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DataGridViewImageCell.DataGridViewImageCellAccessibleObject" /> class. </summary>
			/// <param name="owner">The <see cref="T:System.Windows.Forms.DataGridViewCell" /> that owns the <see cref="T:System.Windows.Forms.DataGridViewImageCell.DataGridViewImageCellAccessibleObject" />.</param>
			// Token: 0x06005989 RID: 22921 RVA: 0x00176D3A File Offset: 0x00174F3A
			public DataGridViewImageCellAccessibleObject(DataGridViewCell owner) : base(owner)
			{
			}

			/// <summary>Gets a string that represents the default action of the <see cref="T:System.Windows.Forms.DataGridViewImageCell" />.</summary>
			/// <returns>An empty string ("").</returns>
			// Token: 0x170015A4 RID: 5540
			// (get) Token: 0x0600598A RID: 22922 RVA: 0x00179039 File Offset: 0x00177239
			public override string DefaultAction
			{
				get
				{
					return string.Empty;
				}
			}

			/// <summary>Gets the text associated with the image in the image cell.</summary>
			/// <returns>The text associated with the image in the image cell.</returns>
			// Token: 0x170015A5 RID: 5541
			// (get) Token: 0x0600598B RID: 22923 RVA: 0x00179040 File Offset: 0x00177240
			public override string Description
			{
				get
				{
					DataGridViewImageCell dataGridViewImageCell = base.Owner as DataGridViewImageCell;
					if (dataGridViewImageCell != null)
					{
						return dataGridViewImageCell.Description;
					}
					return null;
				}
			}

			/// <summary>Gets a string representing the formatted value of the owning cell. </summary>
			/// <returns>A <see cref="T:System.String" /> representation of the cell value.</returns>
			/// <exception cref="T:System.InvalidOperationException">The value of the <see cref="P:System.Windows.Forms.DataGridViewCell.DataGridViewCellAccessibleObject.Owner" /> property is <see langword="null" />.</exception>
			// Token: 0x170015A6 RID: 5542
			// (get) Token: 0x0600598C RID: 22924 RVA: 0x00179064 File Offset: 0x00177264
			// (set) Token: 0x0600598D RID: 22925 RVA: 0x0000701A File Offset: 0x0000521A
			public override string Value
			{
				[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				get
				{
					return base.Value;
				}
				[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				set
				{
				}
			}

			/// <summary>Performs the default action of the <see cref="T:System.Windows.Forms.DataGridViewImageCell.DataGridViewImageCellAccessibleObject" />.</summary>
			// Token: 0x0600598E RID: 22926 RVA: 0x0017906C File Offset: 0x0017726C
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override void DoDefaultAction()
			{
				if (AccessibilityImprovements.Level3)
				{
					DataGridViewImageCell dataGridViewImageCell = (DataGridViewImageCell)base.Owner;
					DataGridView dataGridView = dataGridViewImageCell.DataGridView;
					if (dataGridView != null && dataGridViewImageCell.RowIndex != -1 && dataGridViewImageCell.OwningColumn != null && dataGridViewImageCell.OwningRow != null)
					{
						dataGridView.OnCellContentClickInternal(new DataGridViewCellEventArgs(dataGridViewImageCell.ColumnIndex, dataGridViewImageCell.RowIndex));
					}
				}
			}

			/// <summary>Gets the number of child accessible objects that belong to the <see cref="T:System.Windows.Forms.DataGridViewImageCell.DataGridViewImageCellAccessibleObject" />.</summary>
			/// <returns>The value –1.</returns>
			// Token: 0x0600598F RID: 22927 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
			public override int GetChildCount()
			{
				return 0;
			}

			// Token: 0x06005990 RID: 22928 RVA: 0x00176DCA File Offset: 0x00174FCA
			internal override bool IsIAccessibleExSupported()
			{
				return AccessibilityImprovements.Level2 || base.IsIAccessibleExSupported();
			}

			// Token: 0x06005991 RID: 22929 RVA: 0x001790C6 File Offset: 0x001772C6
			internal override object GetPropertyValue(int propertyID)
			{
				if (propertyID == 30003)
				{
					return 50006;
				}
				if (AccessibilityImprovements.Level3 && propertyID == 30031)
				{
					return true;
				}
				return base.GetPropertyValue(propertyID);
			}

			// Token: 0x06005992 RID: 22930 RVA: 0x001790F8 File Offset: 0x001772F8
			internal override bool IsPatternSupported(int patternId)
			{
				return (AccessibilityImprovements.Level3 && patternId == 10000) || base.IsPatternSupported(patternId);
			}
		}
	}
}
