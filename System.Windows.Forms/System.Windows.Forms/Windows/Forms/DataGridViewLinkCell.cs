using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	/// <summary>Represents a cell that contains a link. </summary>
	// Token: 0x020001F4 RID: 500
	public class DataGridViewLinkCell : DataGridViewCell
	{
		/// <summary>Gets or sets the color used to display an active link.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that represents the color used to display a link that is being selected. The default value is the user's Internet Explorer setting for the color of links in the hover state. </returns>
		// Token: 0x17000712 RID: 1810
		// (get) Token: 0x06001E30 RID: 7728 RVA: 0x00095D28 File Offset: 0x00093F28
		// (set) Token: 0x06001E31 RID: 7729 RVA: 0x00095D78 File Offset: 0x00093F78
		public Color ActiveLinkColor
		{
			get
			{
				if (base.Properties.ContainsObject(DataGridViewLinkCell.PropLinkCellActiveLinkColor))
				{
					return (Color)base.Properties.GetObject(DataGridViewLinkCell.PropLinkCellActiveLinkColor);
				}
				if (SystemInformation.HighContrast && AccessibilityImprovements.Level2)
				{
					return this.HighContrastLinkColor;
				}
				return LinkUtilities.IEActiveLinkColor;
			}
			set
			{
				if (!value.Equals(this.ActiveLinkColor))
				{
					base.Properties.SetObject(DataGridViewLinkCell.PropLinkCellActiveLinkColor, value);
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

		// Token: 0x17000713 RID: 1811
		// (set) Token: 0x06001E32 RID: 7730 RVA: 0x00095DE4 File Offset: 0x00093FE4
		internal Color ActiveLinkColorInternal
		{
			set
			{
				if (!value.Equals(this.ActiveLinkColor))
				{
					base.Properties.SetObject(DataGridViewLinkCell.PropLinkCellActiveLinkColor, value);
				}
			}
		}

		// Token: 0x06001E33 RID: 7731 RVA: 0x00095E18 File Offset: 0x00094018
		private bool ShouldSerializeActiveLinkColor()
		{
			if (SystemInformation.HighContrast && AccessibilityImprovements.Level2)
			{
				return !this.ActiveLinkColor.Equals(SystemColors.HotTrack);
			}
			return !this.ActiveLinkColor.Equals(LinkUtilities.IEActiveLinkColor);
		}

		/// <summary>Gets the type of the cell's hosted editing control.</summary>
		/// <returns>Always <see langword="null" />. </returns>
		// Token: 0x17000714 RID: 1812
		// (get) Token: 0x06001E34 RID: 7732 RVA: 0x0000DE5C File Offset: 0x0000C05C
		public override Type EditType
		{
			get
			{
				return null;
			}
		}

		/// <summary>Gets the display <see cref="T:System.Type" /> of the cell value.</summary>
		/// <returns>The display <see cref="T:System.Type" /> of the cell value.</returns>
		// Token: 0x17000715 RID: 1813
		// (get) Token: 0x06001E35 RID: 7733 RVA: 0x00095E76 File Offset: 0x00094076
		public override Type FormattedValueType
		{
			get
			{
				return DataGridViewLinkCell.defaultFormattedValueType;
			}
		}

		/// <summary>Gets or sets a value that represents the behavior of a link.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.LinkBehavior" /> values. The default is <see cref="F:System.Windows.Forms.LinkBehavior.SystemDefault" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The specified value when setting this property is not a valid <see cref="T:System.Windows.Forms.LinkBehavior" /> value.</exception>
		// Token: 0x17000716 RID: 1814
		// (get) Token: 0x06001E36 RID: 7734 RVA: 0x00095E80 File Offset: 0x00094080
		// (set) Token: 0x06001E37 RID: 7735 RVA: 0x00095EA8 File Offset: 0x000940A8
		[DefaultValue(LinkBehavior.SystemDefault)]
		public LinkBehavior LinkBehavior
		{
			get
			{
				bool flag;
				int integer = base.Properties.GetInteger(DataGridViewLinkCell.PropLinkCellLinkBehavior, out flag);
				if (flag)
				{
					return (LinkBehavior)integer;
				}
				return LinkBehavior.SystemDefault;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 3))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(LinkBehavior));
				}
				if (value != this.LinkBehavior)
				{
					base.Properties.SetInteger(DataGridViewLinkCell.PropLinkCellLinkBehavior, (int)value);
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

		// Token: 0x17000717 RID: 1815
		// (set) Token: 0x06001E38 RID: 7736 RVA: 0x00095F24 File Offset: 0x00094124
		internal LinkBehavior LinkBehaviorInternal
		{
			set
			{
				if (value != this.LinkBehavior)
				{
					base.Properties.SetInteger(DataGridViewLinkCell.PropLinkCellLinkBehavior, (int)value);
				}
			}
		}

		/// <summary>Gets or sets the color used to display an inactive and unvisited link.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that represents the color used to initially display a link. The default value is the user's Internet Explorer setting for the link color.</returns>
		// Token: 0x17000718 RID: 1816
		// (get) Token: 0x06001E39 RID: 7737 RVA: 0x00095F40 File Offset: 0x00094140
		// (set) Token: 0x06001E3A RID: 7738 RVA: 0x00095F90 File Offset: 0x00094190
		public Color LinkColor
		{
			get
			{
				if (base.Properties.ContainsObject(DataGridViewLinkCell.PropLinkCellLinkColor))
				{
					return (Color)base.Properties.GetObject(DataGridViewLinkCell.PropLinkCellLinkColor);
				}
				if (SystemInformation.HighContrast && AccessibilityImprovements.Level2)
				{
					return this.HighContrastLinkColor;
				}
				return LinkUtilities.IELinkColor;
			}
			set
			{
				if (!value.Equals(this.LinkColor))
				{
					base.Properties.SetObject(DataGridViewLinkCell.PropLinkCellLinkColor, value);
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

		// Token: 0x17000719 RID: 1817
		// (set) Token: 0x06001E3B RID: 7739 RVA: 0x00095FFC File Offset: 0x000941FC
		internal Color LinkColorInternal
		{
			set
			{
				if (!value.Equals(this.LinkColor))
				{
					base.Properties.SetObject(DataGridViewLinkCell.PropLinkCellLinkColor, value);
				}
			}
		}

		// Token: 0x06001E3C RID: 7740 RVA: 0x00096030 File Offset: 0x00094230
		private bool ShouldSerializeLinkColor()
		{
			if (SystemInformation.HighContrast && AccessibilityImprovements.Level2)
			{
				return !this.LinkColor.Equals(SystemColors.HotTrack);
			}
			return !this.LinkColor.Equals(LinkUtilities.IELinkColor);
		}

		// Token: 0x1700071A RID: 1818
		// (get) Token: 0x06001E3D RID: 7741 RVA: 0x00096090 File Offset: 0x00094290
		// (set) Token: 0x06001E3E RID: 7742 RVA: 0x000960B6 File Offset: 0x000942B6
		private LinkState LinkState
		{
			get
			{
				bool flag;
				int integer = base.Properties.GetInteger(DataGridViewLinkCell.PropLinkCellLinkState, out flag);
				if (flag)
				{
					return (LinkState)integer;
				}
				return LinkState.Normal;
			}
			set
			{
				if (this.LinkState != value)
				{
					base.Properties.SetInteger(DataGridViewLinkCell.PropLinkCellLinkState, (int)value);
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether the link was visited.</summary>
		/// <returns>
		///     <see langword="true" /> if the link has been visited; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x1700071B RID: 1819
		// (get) Token: 0x06001E3F RID: 7743 RVA: 0x000960D2 File Offset: 0x000942D2
		// (set) Token: 0x06001E40 RID: 7744 RVA: 0x000960E4 File Offset: 0x000942E4
		public bool LinkVisited
		{
			get
			{
				return this.linkVisitedSet && this.linkVisited;
			}
			set
			{
				this.linkVisitedSet = true;
				if (value != this.LinkVisited)
				{
					this.linkVisited = value;
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

		// Token: 0x06001E41 RID: 7745 RVA: 0x00096138 File Offset: 0x00094338
		private bool ShouldSerializeLinkVisited()
		{
			return this.linkVisitedSet = true;
		}

		/// <summary>Gets or sets a value indicating whether the link changes color when it is visited.</summary>
		/// <returns>
		///     <see langword="true" /> if the link changes color when it is selected; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x1700071C RID: 1820
		// (get) Token: 0x06001E42 RID: 7746 RVA: 0x00096150 File Offset: 0x00094350
		// (set) Token: 0x06001E43 RID: 7747 RVA: 0x0009617C File Offset: 0x0009437C
		[DefaultValue(true)]
		public bool TrackVisitedState
		{
			get
			{
				bool flag;
				int integer = base.Properties.GetInteger(DataGridViewLinkCell.PropLinkCellTrackVisitedState, out flag);
				return !flag || integer != 0;
			}
			set
			{
				if (value != this.TrackVisitedState)
				{
					base.Properties.SetInteger(DataGridViewLinkCell.PropLinkCellTrackVisitedState, value ? 1 : 0);
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

		// Token: 0x1700071D RID: 1821
		// (set) Token: 0x06001E44 RID: 7748 RVA: 0x000961D8 File Offset: 0x000943D8
		internal bool TrackVisitedStateInternal
		{
			set
			{
				if (value != this.TrackVisitedState)
				{
					base.Properties.SetInteger(DataGridViewLinkCell.PropLinkCellTrackVisitedState, value ? 1 : 0);
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether the column <see cref="P:System.Windows.Forms.DataGridViewLinkColumn.Text" /> property value is displayed as the link text.</summary>
		/// <returns>
		///     <see langword="true" /> if the column <see cref="P:System.Windows.Forms.DataGridViewLinkColumn.Text" /> property value is displayed as the link text; <see langword="false" /> if the cell <see cref="P:System.Windows.Forms.DataGridViewCell.FormattedValue" /> property value is displayed as the link text. The default is <see langword="false" />.</returns>
		// Token: 0x1700071E RID: 1822
		// (get) Token: 0x06001E45 RID: 7749 RVA: 0x000961FC File Offset: 0x000943FC
		// (set) Token: 0x06001E46 RID: 7750 RVA: 0x00096227 File Offset: 0x00094427
		[DefaultValue(false)]
		public bool UseColumnTextForLinkValue
		{
			get
			{
				bool flag;
				int integer = base.Properties.GetInteger(DataGridViewLinkCell.PropLinkCellUseColumnTextForLinkValue, out flag);
				return flag && integer != 0;
			}
			set
			{
				if (value != this.UseColumnTextForLinkValue)
				{
					base.Properties.SetInteger(DataGridViewLinkCell.PropLinkCellUseColumnTextForLinkValue, value ? 1 : 0);
					base.OnCommonChange();
				}
			}
		}

		// Token: 0x1700071F RID: 1823
		// (set) Token: 0x06001E47 RID: 7751 RVA: 0x0009624F File Offset: 0x0009444F
		internal bool UseColumnTextForLinkValueInternal
		{
			set
			{
				if (value != this.UseColumnTextForLinkValue)
				{
					base.Properties.SetInteger(DataGridViewLinkCell.PropLinkCellUseColumnTextForLinkValue, value ? 1 : 0);
				}
			}
		}

		/// <summary>Gets or sets the color used to display a link that has been previously visited.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that represents the color used to display a link that has been visited. The default value is the user's Internet Explorer setting for the visited link color.</returns>
		// Token: 0x17000720 RID: 1824
		// (get) Token: 0x06001E48 RID: 7752 RVA: 0x00096274 File Offset: 0x00094474
		// (set) Token: 0x06001E49 RID: 7753 RVA: 0x000962D0 File Offset: 0x000944D0
		public Color VisitedLinkColor
		{
			get
			{
				if (base.Properties.ContainsObject(DataGridViewLinkCell.PropLinkCellVisitedLinkColor))
				{
					return (Color)base.Properties.GetObject(DataGridViewLinkCell.PropLinkCellVisitedLinkColor);
				}
				if (!SystemInformation.HighContrast || !AccessibilityImprovements.Level2)
				{
					return LinkUtilities.IEVisitedLinkColor;
				}
				if (!this.Selected)
				{
					return LinkUtilities.GetVisitedLinkColor();
				}
				return SystemColors.HighlightText;
			}
			set
			{
				if (!value.Equals(this.VisitedLinkColor))
				{
					base.Properties.SetObject(DataGridViewLinkCell.PropLinkCellVisitedLinkColor, value);
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

		// Token: 0x17000721 RID: 1825
		// (set) Token: 0x06001E4A RID: 7754 RVA: 0x0009633C File Offset: 0x0009453C
		internal Color VisitedLinkColorInternal
		{
			set
			{
				if (!value.Equals(this.VisitedLinkColor))
				{
					base.Properties.SetObject(DataGridViewLinkCell.PropLinkCellVisitedLinkColor, value);
				}
			}
		}

		// Token: 0x06001E4B RID: 7755 RVA: 0x00096370 File Offset: 0x00094570
		private bool ShouldSerializeVisitedLinkColor()
		{
			if (SystemInformation.HighContrast && AccessibilityImprovements.Level2)
			{
				return !this.VisitedLinkColor.Equals(SystemColors.HotTrack);
			}
			return !this.VisitedLinkColor.Equals(LinkUtilities.IEVisitedLinkColor);
		}

		// Token: 0x17000722 RID: 1826
		// (get) Token: 0x06001E4C RID: 7756 RVA: 0x000963CE File Offset: 0x000945CE
		private Color HighContrastLinkColor
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				if (!this.Selected)
				{
					return SystemColors.HotTrack;
				}
				return SystemColors.HighlightText;
			}
		}

		/// <summary>Gets or sets the data type of the values in the cell. </summary>
		/// <returns>A <see cref="T:System.Type" /> representing the data type of the value in the cell.</returns>
		// Token: 0x17000723 RID: 1827
		// (get) Token: 0x06001E4D RID: 7757 RVA: 0x000963E4 File Offset: 0x000945E4
		public override Type ValueType
		{
			get
			{
				Type valueType = base.ValueType;
				if (valueType != null)
				{
					return valueType;
				}
				return DataGridViewLinkCell.defaultValueType;
			}
		}

		/// <summary>Creates an exact copy of this cell.</summary>
		/// <returns>An <see cref="T:System.Object" /> that represents the cloned <see cref="T:System.Windows.Forms.DataGridViewLinkCell" />.</returns>
		// Token: 0x06001E4E RID: 7758 RVA: 0x00096408 File Offset: 0x00094608
		public override object Clone()
		{
			Type type = base.GetType();
			DataGridViewLinkCell dataGridViewLinkCell;
			if (type == DataGridViewLinkCell.cellType)
			{
				dataGridViewLinkCell = new DataGridViewLinkCell();
			}
			else
			{
				dataGridViewLinkCell = (DataGridViewLinkCell)Activator.CreateInstance(type);
			}
			base.CloneInternal(dataGridViewLinkCell);
			if (base.Properties.ContainsObject(DataGridViewLinkCell.PropLinkCellActiveLinkColor))
			{
				dataGridViewLinkCell.ActiveLinkColorInternal = this.ActiveLinkColor;
			}
			if (base.Properties.ContainsInteger(DataGridViewLinkCell.PropLinkCellUseColumnTextForLinkValue))
			{
				dataGridViewLinkCell.UseColumnTextForLinkValueInternal = this.UseColumnTextForLinkValue;
			}
			if (base.Properties.ContainsInteger(DataGridViewLinkCell.PropLinkCellLinkBehavior))
			{
				dataGridViewLinkCell.LinkBehaviorInternal = this.LinkBehavior;
			}
			if (base.Properties.ContainsObject(DataGridViewLinkCell.PropLinkCellLinkColor))
			{
				dataGridViewLinkCell.LinkColorInternal = this.LinkColor;
			}
			if (base.Properties.ContainsInteger(DataGridViewLinkCell.PropLinkCellTrackVisitedState))
			{
				dataGridViewLinkCell.TrackVisitedStateInternal = this.TrackVisitedState;
			}
			if (base.Properties.ContainsObject(DataGridViewLinkCell.PropLinkCellVisitedLinkColor))
			{
				dataGridViewLinkCell.VisitedLinkColorInternal = this.VisitedLinkColor;
			}
			if (this.linkVisitedSet)
			{
				dataGridViewLinkCell.LinkVisited = this.LinkVisited;
			}
			return dataGridViewLinkCell;
		}

		// Token: 0x06001E4F RID: 7759 RVA: 0x00096510 File Offset: 0x00094710
		private bool LinkBoundsContainPoint(int x, int y, int rowIndex)
		{
			return base.GetContentBounds(rowIndex).Contains(x, y);
		}

		/// <summary>Creates a new accessible object for the <see cref="T:System.Windows.Forms.DataGridViewLinkCell" />. </summary>
		/// <returns>A new <see cref="T:System.Windows.Forms.DataGridViewLinkCell.DataGridViewLinkCellAccessibleObject" /> for the <see cref="T:System.Windows.Forms.DataGridViewLinkCell" />. </returns>
		// Token: 0x06001E50 RID: 7760 RVA: 0x0009652E File Offset: 0x0009472E
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			return new DataGridViewLinkCell.DataGridViewLinkCellAccessibleObject(this);
		}

		/// <summary>Returns the bounding rectangle that encloses the cell's content area, which is calculated using the specified <see cref="T:System.Drawing.Graphics" /> and cell style.</summary>
		/// <param name="graphics">The graphics context for the cell.</param>
		/// <param name="cellStyle">The <see cref="T:System.Windows.Forms.DataGridViewCellStyle" /> to be applied to the cell.</param>
		/// <param name="rowIndex">The index of the cell's parent row.</param>
		/// <returns>The <see cref="T:System.Drawing.Rectangle" /> that bounds the cell's contents.</returns>
		// Token: 0x06001E51 RID: 7761 RVA: 0x00096538 File Offset: 0x00094738
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
			DataGridViewElementStates cellState;
			Rectangle rectangle;
			base.ComputeBorderStyleCellStateAndCellBounds(rowIndex, out advancedBorderStyle, out cellState, out rectangle);
			return this.PaintPrivate(graphics, rectangle, rectangle, rowIndex, cellState, formattedValue, null, cellStyle, advancedBorderStyle, DataGridViewPaintParts.ContentForeground, true, false, false);
		}

		/// <summary>Returns the bounding rectangle that encloses the cell's error icon, if one is displayed.</summary>
		/// <param name="graphics">The graphics context for the cell.</param>
		/// <param name="cellStyle">The <see cref="T:System.Windows.Forms.DataGridViewCellStyle" /> to be applied to the cell.</param>
		/// <param name="rowIndex">The index of the cell's parent row.</param>
		/// <returns>The <see cref="T:System.Drawing.Rectangle" /> that bounds the cell's error icon, if one is displayed; otherwise, <see cref="F:System.Drawing.Rectangle.Empty" />.</returns>
		// Token: 0x06001E52 RID: 7762 RVA: 0x000965AC File Offset: 0x000947AC
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
			DataGridViewElementStates cellState;
			Rectangle rectangle;
			base.ComputeBorderStyleCellStateAndCellBounds(rowIndex, out advancedBorderStyle, out cellState, out rectangle);
			return this.PaintPrivate(graphics, rectangle, rectangle, rowIndex, cellState, formattedValue, this.GetErrorText(rowIndex), cellStyle, advancedBorderStyle, DataGridViewPaintParts.ContentForeground, false, true, false);
		}

		/// <summary>Calculates the preferred size, in pixels, of the cell.</summary>
		/// <param name="graphics">The <see cref="T:System.Drawing.Graphics" /> used to draw the cell.</param>
		/// <param name="cellStyle">A <see cref="T:System.Windows.Forms.DataGridViewCellStyle" /> that represents the style of the cell.</param>
		/// <param name="rowIndex">The zero-based row index of the cell.</param>
		/// <param name="constraintSize">The cell's maximum allowable size.</param>
		/// <returns>A <see cref="T:System.Drawing.Size" /> that represents the preferred size, in pixels, of the cell.</returns>
		// Token: 0x06001E53 RID: 7763 RVA: 0x00096640 File Offset: 0x00094840
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
			string text = formattedValue as string;
			if (string.IsNullOrEmpty(text))
			{
				text = " ";
			}
			TextFormatFlags flags = DataGridViewUtilities.ComputeTextFormatFlagsForCellStyleAlignment(base.DataGridView.RightToLeftInternal, cellStyle.Alignment, cellStyle.WrapMode);
			Size result;
			if (cellStyle.WrapMode == DataGridViewTriState.True && text.Length > 1)
			{
				if (freeDimensionFromConstraint != DataGridViewFreeDimension.Height)
				{
					if (freeDimensionFromConstraint == DataGridViewFreeDimension.Width)
					{
						int num3 = constraintSize.Height - num2 - 1 - 1;
						if ((cellStyle.Alignment & DataGridViewLinkCell.anyBottom) != DataGridViewContentAlignment.NotSet)
						{
							num3--;
						}
						result = new Size(DataGridViewCell.MeasureTextWidth(graphics, text, cellStyle.Font, Math.Max(1, num3), flags), 0);
					}
					else
					{
						result = DataGridViewCell.MeasureTextPreferredSize(graphics, text, cellStyle.Font, 5f, flags);
					}
				}
				else
				{
					result = new Size(0, DataGridViewCell.MeasureTextHeight(graphics, text, cellStyle.Font, Math.Max(1, constraintSize.Width - num - 1 - 2), flags));
				}
			}
			else if (freeDimensionFromConstraint != DataGridViewFreeDimension.Height)
			{
				if (freeDimensionFromConstraint == DataGridViewFreeDimension.Width)
				{
					result = new Size(DataGridViewCell.MeasureTextSize(graphics, text, cellStyle.Font, flags).Width, 0);
				}
				else
				{
					result = DataGridViewCell.MeasureTextSize(graphics, text, cellStyle.Font, flags);
				}
			}
			else
			{
				result = new Size(0, DataGridViewCell.MeasureTextSize(graphics, text, cellStyle.Font, flags).Height);
			}
			if (freeDimensionFromConstraint != DataGridViewFreeDimension.Height)
			{
				result.Width += 3 + num;
				if (base.DataGridView.ShowCellErrors)
				{
					result.Width = Math.Max(result.Width, num + 8 + (int)DataGridViewCell.iconsWidth);
				}
			}
			if (freeDimensionFromConstraint != DataGridViewFreeDimension.Width)
			{
				result.Height += 2 + num2;
				if ((cellStyle.Alignment & DataGridViewLinkCell.anyBottom) != DataGridViewContentAlignment.NotSet)
				{
					result.Height++;
				}
				if (base.DataGridView.ShowCellErrors)
				{
					result.Height = Math.Max(result.Height, num2 + 8 + (int)DataGridViewCell.iconsHeight);
				}
			}
			return result;
		}

		/// <summary>Gets the value of the cell.</summary>
		/// <param name="rowIndex">The index of the cell's parent row.</param>
		/// <returns>The value contained in the <see cref="T:System.Windows.Forms.DataGridViewCell" />.</returns>
		// Token: 0x06001E54 RID: 7764 RVA: 0x000968AC File Offset: 0x00094AAC
		protected override object GetValue(int rowIndex)
		{
			if (this.UseColumnTextForLinkValue && base.DataGridView != null && base.DataGridView.NewRowIndex != rowIndex && base.OwningColumn != null && base.OwningColumn is DataGridViewLinkColumn)
			{
				return ((DataGridViewLinkColumn)base.OwningColumn).Text;
			}
			return base.GetValue(rowIndex);
		}

		/// <summary>Indicates whether the row containing the cell will be unshared when a key is released and the cell has focus.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.KeyEventArgs" /> that contains data about the key press.</param>
		/// <param name="rowIndex">The index of the row containing the cell.</param>
		/// <returns>
		///     <see langword="true" /> if the SPACE key was released, the <see cref="P:System.Windows.Forms.DataGridViewLinkCell.TrackVisitedState" /> property is <see langword="true" />, the <see cref="P:System.Windows.Forms.DataGridViewLinkCell.LinkVisited" /> property is <see langword="false" />, and the CTRL, ALT, and SHIFT keys are not pressed; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001E55 RID: 7765 RVA: 0x00096904 File Offset: 0x00094B04
		protected override bool KeyUpUnsharesRow(KeyEventArgs e, int rowIndex)
		{
			return e.KeyCode != Keys.Space || e.Alt || e.Control || e.Shift || (this.TrackVisitedState && !this.LinkVisited);
		}

		/// <summary>Indicates whether the row containing the cell will be unshared when the mouse button is pressed while the pointer is over the cell.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.DataGridViewCellMouseEventArgs" /> that contains data about the mouse click.</param>
		/// <returns>
		///     <see langword="true" /> if the mouse pointer is over the link; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001E56 RID: 7766 RVA: 0x0009693D File Offset: 0x00094B3D
		protected override bool MouseDownUnsharesRow(DataGridViewCellMouseEventArgs e)
		{
			return this.LinkBoundsContainPoint(e.X, e.Y, e.RowIndex);
		}

		/// <summary>Indicates whether the row containing the cell will be unshared when the mouse pointer leaves the cell.</summary>
		/// <param name="rowIndex">The index of the row containing the cell.</param>
		/// <returns>
		///     <see langword="true" /> if the link displayed by the cell is not in the normal state; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001E57 RID: 7767 RVA: 0x00096957 File Offset: 0x00094B57
		protected override bool MouseLeaveUnsharesRow(int rowIndex)
		{
			return this.LinkState > LinkState.Normal;
		}

		/// <summary>Indicates whether the row containing the cell will be unshared when the mouse pointer moves over the cell.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.DataGridViewCellMouseEventArgs" /> that contains data about the mouse click.</param>
		/// <returns>
		///     <see langword="true" /> if the mouse pointer is over the link and the link is has not yet changed color to reflect the hover state; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001E58 RID: 7768 RVA: 0x00096962 File Offset: 0x00094B62
		protected override bool MouseMoveUnsharesRow(DataGridViewCellMouseEventArgs e)
		{
			if (this.LinkBoundsContainPoint(e.X, e.Y, e.RowIndex))
			{
				if ((this.LinkState & LinkState.Hover) == LinkState.Normal)
				{
					return true;
				}
			}
			else if ((this.LinkState & LinkState.Hover) != LinkState.Normal)
			{
				return true;
			}
			return false;
		}

		/// <summary>Indicates whether the row containing the cell will be unshared when the mouse button is released while the pointer is over the cell. </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.DataGridViewCellMouseEventArgs" /> that contains data about the mouse click.</param>
		/// <returns>
		///     <see langword="true" /> if the mouse pointer is over the link; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001E59 RID: 7769 RVA: 0x00096997 File Offset: 0x00094B97
		protected override bool MouseUpUnsharesRow(DataGridViewCellMouseEventArgs e)
		{
			return this.TrackVisitedState && this.LinkBoundsContainPoint(e.X, e.Y, e.RowIndex);
		}

		/// <summary>Called when a character key is released while the focus is on a cell.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.KeyEventArgs" /> that contains the event data. </param>
		/// <param name="rowIndex">The index of the cell's parent row.</param>
		// Token: 0x06001E5A RID: 7770 RVA: 0x000969BC File Offset: 0x00094BBC
		protected override void OnKeyUp(KeyEventArgs e, int rowIndex)
		{
			if (base.DataGridView == null)
			{
				return;
			}
			if (e.KeyCode == Keys.Space && !e.Alt && !e.Control && !e.Shift)
			{
				base.RaiseCellClick(new DataGridViewCellEventArgs(base.ColumnIndex, rowIndex));
				if (base.DataGridView != null && base.ColumnIndex < base.DataGridView.Columns.Count && rowIndex < base.DataGridView.Rows.Count)
				{
					base.RaiseCellContentClick(new DataGridViewCellEventArgs(base.ColumnIndex, rowIndex));
					if (this.TrackVisitedState)
					{
						this.LinkVisited = true;
					}
				}
				e.Handled = true;
			}
		}

		/// <summary>Called when the user holds down a mouse button while the pointer is on a cell.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.DataGridViewCellMouseEventArgs" /> that contains the event data.</param>
		// Token: 0x06001E5B RID: 7771 RVA: 0x00096A64 File Offset: 0x00094C64
		protected override void OnMouseDown(DataGridViewCellMouseEventArgs e)
		{
			if (base.DataGridView == null)
			{
				return;
			}
			if (this.LinkBoundsContainPoint(e.X, e.Y, e.RowIndex))
			{
				this.LinkState |= LinkState.Active;
				base.DataGridView.InvalidateCell(base.ColumnIndex, e.RowIndex);
			}
			base.OnMouseDown(e);
		}

		/// <summary>Called when the mouse pointer leaves the cell.</summary>
		/// <param name="rowIndex">The index of the cell's parent row. </param>
		// Token: 0x06001E5C RID: 7772 RVA: 0x00096AC0 File Offset: 0x00094CC0
		protected override void OnMouseLeave(int rowIndex)
		{
			if (base.DataGridView == null)
			{
				return;
			}
			if (DataGridViewLinkCell.dataGridViewCursor != null)
			{
				base.DataGridView.Cursor = DataGridViewLinkCell.dataGridViewCursor;
				DataGridViewLinkCell.dataGridViewCursor = null;
			}
			if (this.LinkState != LinkState.Normal)
			{
				this.LinkState = LinkState.Normal;
				base.DataGridView.InvalidateCell(base.ColumnIndex, rowIndex);
			}
			base.OnMouseLeave(rowIndex);
		}

		/// <summary>Called when the mouse pointer moves within a cell.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.DataGridViewCellMouseEventArgs" /> that contains the event data.</param>
		// Token: 0x06001E5D RID: 7773 RVA: 0x00096B24 File Offset: 0x00094D24
		protected override void OnMouseMove(DataGridViewCellMouseEventArgs e)
		{
			if (base.DataGridView == null)
			{
				return;
			}
			if (this.LinkBoundsContainPoint(e.X, e.Y, e.RowIndex))
			{
				if ((this.LinkState & LinkState.Hover) == LinkState.Normal)
				{
					this.LinkState |= LinkState.Hover;
					base.DataGridView.InvalidateCell(base.ColumnIndex, e.RowIndex);
				}
				if (DataGridViewLinkCell.dataGridViewCursor == null)
				{
					DataGridViewLinkCell.dataGridViewCursor = base.DataGridView.UserSetCursor;
				}
				if (base.DataGridView.Cursor != Cursors.Hand)
				{
					base.DataGridView.Cursor = Cursors.Hand;
				}
			}
			else if ((this.LinkState & LinkState.Hover) != LinkState.Normal)
			{
				this.LinkState &= (LinkState)(-2);
				base.DataGridView.Cursor = DataGridViewLinkCell.dataGridViewCursor;
				base.DataGridView.InvalidateCell(base.ColumnIndex, e.RowIndex);
			}
			base.OnMouseMove(e);
		}

		/// <summary>Called when the user releases a mouse button while the pointer is on a cell.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.DataGridViewCellMouseEventArgs" /> that contains the event data.</param>
		// Token: 0x06001E5E RID: 7774 RVA: 0x00096C10 File Offset: 0x00094E10
		protected override void OnMouseUp(DataGridViewCellMouseEventArgs e)
		{
			if (base.DataGridView == null)
			{
				return;
			}
			if (this.LinkBoundsContainPoint(e.X, e.Y, e.RowIndex) && this.TrackVisitedState)
			{
				this.LinkVisited = true;
			}
		}

		/// <summary>Paints the current <see cref="T:System.Windows.Forms.DataGridViewCell" />.</summary>
		/// <param name="graphics">The <see cref="T:System.Drawing.Graphics" /> used to paint the <see cref="T:System.Windows.Forms.DataGridViewCell" />.</param>
		/// <param name="clipBounds">A <see cref="T:System.Drawing.Rectangle" /> that represents the area of the <see cref="T:System.Windows.Forms.DataGridView" /> that needs to be repainted.</param>
		/// <param name="cellBounds">A <see cref="T:System.Drawing.Rectangle" /> that contains the bounds of the <see cref="T:System.Windows.Forms.DataGridViewCell" /> that is being painted.</param>
		/// <param name="rowIndex">The row index of the cell that is being painted.</param>
		/// <param name="cellState">A bitwise combination of <see cref="T:System.Windows.Forms.DataGridViewElementStates" /> values that specifies the state of the cell.</param>
		/// <param name="value">The data of the <see cref="T:System.Windows.Forms.DataGridViewCell" /> that is being painted.</param>
		/// <param name="formattedValue">The formatted data of the <see cref="T:System.Windows.Forms.DataGridViewCell" /> that is being painted.</param>
		/// <param name="errorText">An error message that is associated with the cell.</param>
		/// <param name="cellStyle">A <see cref="T:System.Windows.Forms.DataGridViewCellStyle" /> that contains formatting and style information about the cell.</param>
		/// <param name="advancedBorderStyle">A <see cref="T:System.Windows.Forms.DataGridViewAdvancedBorderStyle" /> that contains border styles for the cell that is being painted.</param>
		/// <param name="paintParts">A bitwise combination of the <see cref="T:System.Windows.Forms.DataGridViewPaintParts" /> values that specifies which parts of the cell need to be painted.</param>
		// Token: 0x06001E5F RID: 7775 RVA: 0x00096C44 File Offset: 0x00094E44
		protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
		{
			if (cellStyle == null)
			{
				throw new ArgumentNullException("cellStyle");
			}
			this.PaintPrivate(graphics, clipBounds, cellBounds, rowIndex, cellState, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts, false, false, true);
		}

		// Token: 0x06001E60 RID: 7776 RVA: 0x00096C7C File Offset: 0x00094E7C
		private Rectangle PaintPrivate(Graphics g, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts, bool computeContentBounds, bool computeErrorIconBounds, bool paint)
		{
			if (paint && DataGridViewCell.PaintBorder(paintParts))
			{
				this.PaintBorder(g, clipBounds, cellBounds, cellStyle, advancedBorderStyle);
			}
			Rectangle result = Rectangle.Empty;
			Rectangle rectangle = this.BorderWidths(advancedBorderStyle);
			Rectangle rectangle2 = cellBounds;
			rectangle2.Offset(rectangle.X, rectangle.Y);
			rectangle2.Width -= rectangle.Right;
			rectangle2.Height -= rectangle.Bottom;
			Point currentCellAddress = base.DataGridView.CurrentCellAddress;
			bool flag = currentCellAddress.X == base.ColumnIndex && currentCellAddress.Y == rowIndex;
			bool flag2 = (cellState & DataGridViewElementStates.Selected) > DataGridViewElementStates.None;
			SolidBrush cachedBrush = base.DataGridView.GetCachedBrush((DataGridViewCell.PaintSelectionBackground(paintParts) && flag2) ? cellStyle.SelectionBackColor : cellStyle.BackColor);
			if (paint && DataGridViewCell.PaintBackground(paintParts) && cachedBrush.Color.A == 255)
			{
				g.FillRectangle(cachedBrush, rectangle2);
			}
			if (cellStyle.Padding != Padding.Empty)
			{
				if (base.DataGridView.RightToLeftInternal)
				{
					rectangle2.Offset(cellStyle.Padding.Right, cellStyle.Padding.Top);
				}
				else
				{
					rectangle2.Offset(cellStyle.Padding.Left, cellStyle.Padding.Top);
				}
				rectangle2.Width -= cellStyle.Padding.Horizontal;
				rectangle2.Height -= cellStyle.Padding.Vertical;
			}
			Rectangle rectangle3 = rectangle2;
			string text = formattedValue as string;
			if (text != null && (paint || computeContentBounds))
			{
				rectangle2.Offset(1, 1);
				rectangle2.Width -= 3;
				rectangle2.Height -= 2;
				if ((cellStyle.Alignment & DataGridViewLinkCell.anyBottom) != DataGridViewContentAlignment.NotSet)
				{
					rectangle2.Height--;
				}
				Font font = null;
				Font font2 = null;
				LinkUtilities.EnsureLinkFonts(cellStyle.Font, this.LinkBehavior, ref font, ref font2);
				TextFormatFlags textFormatFlags = DataGridViewUtilities.ComputeTextFormatFlagsForCellStyleAlignment(base.DataGridView.RightToLeftInternal, cellStyle.Alignment, cellStyle.WrapMode);
				if (paint)
				{
					if (rectangle2.Width > 0 && rectangle2.Height > 0)
					{
						if (flag && base.DataGridView.ShowFocusCues && base.DataGridView.Focused && DataGridViewCell.PaintFocus(paintParts))
						{
							Rectangle textBounds = DataGridViewUtilities.GetTextBounds(rectangle2, text, textFormatFlags, cellStyle, (this.LinkState == LinkState.Hover) ? font2 : font);
							if ((cellStyle.Alignment & DataGridViewLinkCell.anyLeft) != DataGridViewContentAlignment.NotSet)
							{
								int num = textBounds.X;
								textBounds.X = num - 1;
								num = textBounds.Width;
								textBounds.Width = num + 1;
							}
							else if ((cellStyle.Alignment & DataGridViewLinkCell.anyRight) != DataGridViewContentAlignment.NotSet)
							{
								int num = textBounds.X;
								textBounds.X = num + 1;
								num = textBounds.Width;
								textBounds.Width = num + 1;
							}
							textBounds.Height += 2;
							ControlPaint.DrawFocusRectangle(g, textBounds, Color.Empty, cachedBrush.Color);
						}
						Color foreColor;
						if ((this.LinkState & LinkState.Active) == LinkState.Active)
						{
							foreColor = this.ActiveLinkColor;
						}
						else if (this.LinkVisited)
						{
							foreColor = this.VisitedLinkColor;
						}
						else
						{
							foreColor = this.LinkColor;
						}
						if (DataGridViewCell.PaintContentForeground(paintParts))
						{
							if ((textFormatFlags & TextFormatFlags.SingleLine) != TextFormatFlags.Default)
							{
								textFormatFlags |= TextFormatFlags.EndEllipsis;
							}
							TextRenderer.DrawText(g, text, (this.LinkState == LinkState.Hover) ? font2 : font, rectangle2, foreColor, textFormatFlags);
						}
					}
					else if (flag && base.DataGridView.ShowFocusCues && base.DataGridView.Focused && DataGridViewCell.PaintFocus(paintParts) && rectangle3.Width > 0 && rectangle3.Height > 0)
					{
						ControlPaint.DrawFocusRectangle(g, rectangle3, Color.Empty, cachedBrush.Color);
					}
				}
				else
				{
					result = DataGridViewUtilities.GetTextBounds(rectangle2, text, textFormatFlags, cellStyle, (this.LinkState == LinkState.Hover) ? font2 : font);
				}
				font.Dispose();
				font2.Dispose();
			}
			else if (paint || computeContentBounds)
			{
				if (flag && base.DataGridView.ShowFocusCues && base.DataGridView.Focused && DataGridViewCell.PaintFocus(paintParts) && paint && rectangle2.Width > 0 && rectangle2.Height > 0)
				{
					ControlPaint.DrawFocusRectangle(g, rectangle2, Color.Empty, cachedBrush.Color);
				}
			}
			else if (computeErrorIconBounds && !string.IsNullOrEmpty(errorText))
			{
				result = base.ComputeErrorIconBounds(rectangle3);
			}
			if (base.DataGridView.ShowCellErrors && paint && DataGridViewCell.PaintErrorIcon(paintParts))
			{
				base.PaintErrorIcon(g, cellStyle, rowIndex, cellBounds, rectangle3, errorText);
			}
			return result;
		}

		/// <summary>Returns a string that describes the current object.</summary>
		/// <returns>A string that represents the current object.</returns>
		// Token: 0x06001E61 RID: 7777 RVA: 0x00097150 File Offset: 0x00095350
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"DataGridViewLinkCell { ColumnIndex=",
				base.ColumnIndex.ToString(CultureInfo.CurrentCulture),
				", RowIndex=",
				base.RowIndex.ToString(CultureInfo.CurrentCulture),
				" }"
			});
		}

		// Token: 0x04000D48 RID: 3400
		private static readonly DataGridViewContentAlignment anyLeft = (DataGridViewContentAlignment)273;

		// Token: 0x04000D49 RID: 3401
		private static readonly DataGridViewContentAlignment anyRight = (DataGridViewContentAlignment)1092;

		// Token: 0x04000D4A RID: 3402
		private static readonly DataGridViewContentAlignment anyBottom = (DataGridViewContentAlignment)1792;

		// Token: 0x04000D4B RID: 3403
		private static Type defaultFormattedValueType = typeof(string);

		// Token: 0x04000D4C RID: 3404
		private static Type defaultValueType = typeof(object);

		// Token: 0x04000D4D RID: 3405
		private static Type cellType = typeof(DataGridViewLinkCell);

		// Token: 0x04000D4E RID: 3406
		private static readonly int PropLinkCellActiveLinkColor = PropertyStore.CreateKey();

		// Token: 0x04000D4F RID: 3407
		private static readonly int PropLinkCellLinkBehavior = PropertyStore.CreateKey();

		// Token: 0x04000D50 RID: 3408
		private static readonly int PropLinkCellLinkColor = PropertyStore.CreateKey();

		// Token: 0x04000D51 RID: 3409
		private static readonly int PropLinkCellLinkState = PropertyStore.CreateKey();

		// Token: 0x04000D52 RID: 3410
		private static readonly int PropLinkCellTrackVisitedState = PropertyStore.CreateKey();

		// Token: 0x04000D53 RID: 3411
		private static readonly int PropLinkCellUseColumnTextForLinkValue = PropertyStore.CreateKey();

		// Token: 0x04000D54 RID: 3412
		private static readonly int PropLinkCellVisitedLinkColor = PropertyStore.CreateKey();

		// Token: 0x04000D55 RID: 3413
		private const byte DATAGRIDVIEWLINKCELL_horizontalTextMarginLeft = 1;

		// Token: 0x04000D56 RID: 3414
		private const byte DATAGRIDVIEWLINKCELL_horizontalTextMarginRight = 2;

		// Token: 0x04000D57 RID: 3415
		private const byte DATAGRIDVIEWLINKCELL_verticalTextMarginTop = 1;

		// Token: 0x04000D58 RID: 3416
		private const byte DATAGRIDVIEWLINKCELL_verticalTextMarginBottom = 1;

		// Token: 0x04000D59 RID: 3417
		private bool linkVisited;

		// Token: 0x04000D5A RID: 3418
		private bool linkVisitedSet;

		// Token: 0x04000D5B RID: 3419
		private static Cursor dataGridViewCursor = null;

		/// <summary>Provides information about a <see cref="T:System.Windows.Forms.DataGridViewLinkCell" /> control to accessibility client applications.</summary>
		// Token: 0x020005B8 RID: 1464
		protected class DataGridViewLinkCellAccessibleObject : DataGridViewCell.DataGridViewCellAccessibleObject
		{
			/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DataGridViewLinkCell.DataGridViewLinkCellAccessibleObject" /> class. </summary>
			/// <param name="owner">The <see cref="T:System.Windows.Forms.DataGridViewCell" /> that owns the <see cref="T:System.Windows.Forms.DataGridViewLinkCell.DataGridViewLinkCellAccessibleObject" />.</param>
			// Token: 0x06005993 RID: 22931 RVA: 0x00176D3A File Offset: 0x00174F3A
			public DataGridViewLinkCellAccessibleObject(DataGridViewCell owner) : base(owner)
			{
			}

			/// <summary>Gets a string that represents the default action of the <see cref="T:System.Windows.Forms.DataGridViewLinkCell.DataGridViewLinkCellAccessibleObject" />.</summary>
			/// <returns>The string "Click".</returns>
			// Token: 0x170015A7 RID: 5543
			// (get) Token: 0x06005994 RID: 22932 RVA: 0x00179112 File Offset: 0x00177312
			public override string DefaultAction
			{
				get
				{
					return SR.GetString("DataGridView_AccLinkCellDefaultAction");
				}
			}

			/// <summary>Performs the default action of the <see cref="T:System.Windows.Forms.DataGridViewLinkCell.DataGridViewLinkCellAccessibleObject" />.</summary>
			/// <exception cref="T:System.InvalidOperationException">The cell returned by the <see cref="P:System.Windows.Forms.DataGridViewCell.DataGridViewCellAccessibleObject.Owner" /> property has a <see cref="P:System.Windows.Forms.DataGridViewElement.DataGridView" /> property value that is not <see langword="null" /> and a <see cref="P:System.Windows.Forms.DataGridViewCell.RowIndex" /> property value of -1, indicating that the cell is in a shared row.</exception>
			// Token: 0x06005995 RID: 22933 RVA: 0x00179120 File Offset: 0x00177320
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override void DoDefaultAction()
			{
				DataGridViewLinkCell dataGridViewLinkCell = (DataGridViewLinkCell)base.Owner;
				DataGridView dataGridView = dataGridViewLinkCell.DataGridView;
				if (dataGridView != null && dataGridViewLinkCell.RowIndex == -1)
				{
					throw new InvalidOperationException(SR.GetString("DataGridView_InvalidOperationOnSharedCell"));
				}
				if (dataGridViewLinkCell.OwningColumn != null && dataGridViewLinkCell.OwningRow != null)
				{
					dataGridView.OnCellContentClickInternal(new DataGridViewCellEventArgs(dataGridViewLinkCell.ColumnIndex, dataGridViewLinkCell.RowIndex));
				}
			}

			/// <summary>Gets the number of child accessible objects that belong to the <see cref="T:System.Windows.Forms.DataGridViewLinkCell.DataGridViewLinkCellAccessibleObject" />.</summary>
			/// <returns>The value –1.</returns>
			// Token: 0x06005996 RID: 22934 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
			public override int GetChildCount()
			{
				return 0;
			}

			// Token: 0x06005997 RID: 22935 RVA: 0x00176DCA File Offset: 0x00174FCA
			internal override bool IsIAccessibleExSupported()
			{
				return AccessibilityImprovements.Level2 || base.IsIAccessibleExSupported();
			}

			// Token: 0x06005998 RID: 22936 RVA: 0x00179183 File Offset: 0x00177383
			internal override object GetPropertyValue(int propertyID)
			{
				if (propertyID == 30003)
				{
					return 50005;
				}
				return base.GetPropertyValue(propertyID);
			}
		}
	}
}
