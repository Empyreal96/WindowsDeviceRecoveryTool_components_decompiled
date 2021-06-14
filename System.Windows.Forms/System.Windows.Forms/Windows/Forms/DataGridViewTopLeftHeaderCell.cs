using System;
using System.Drawing;
using System.Security.Permissions;
using System.Windows.Forms.VisualStyles;

namespace System.Windows.Forms
{
	/// <summary>Represents the cell in the top left corner of the <see cref="T:System.Windows.Forms.DataGridView" /> that sits above the row headers and to the left of the column headers.</summary>
	// Token: 0x02000210 RID: 528
	public class DataGridViewTopLeftHeaderCell : DataGridViewColumnHeaderCell
	{
		/// <summary>Creates a new accessible object for the <see cref="T:System.Windows.Forms.DataGridViewTopLeftHeaderCell" />. </summary>
		/// <returns>A new <see cref="T:System.Windows.Forms.DataGridViewTopLeftHeaderCell.DataGridViewTopLeftHeaderCellAccessibleObject" /> for the <see cref="T:System.Windows.Forms.DataGridViewTopLeftHeaderCell" />. </returns>
		// Token: 0x06002019 RID: 8217 RVA: 0x000A05AD File Offset: 0x0009E7AD
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			return new DataGridViewTopLeftHeaderCell.DataGridViewTopLeftHeaderCellAccessibleObject(this);
		}

		/// <summary>Returns the bounding rectangle that encloses the cell's content area, which is calculated using the specified <see cref="T:System.Drawing.Graphics" /> object and cell style.</summary>
		/// <param name="graphics">The graphics context for the cell.</param>
		/// <param name="cellStyle">The <see cref="T:System.Windows.Forms.DataGridViewCellStyle" /> to be applied to the cell.</param>
		/// <param name="rowIndex">The index of the cell's parent row.</param>
		/// <returns>The <see cref="T:System.Drawing.Rectangle" /> that bounds the cell's contents.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="rowIndex" /> does not equal -1.</exception>
		// Token: 0x0600201A RID: 8218 RVA: 0x000A05B8 File Offset: 0x0009E7B8
		protected override Rectangle GetContentBounds(Graphics graphics, DataGridViewCellStyle cellStyle, int rowIndex)
		{
			if (cellStyle == null)
			{
				throw new ArgumentNullException("cellStyle");
			}
			if (rowIndex != -1)
			{
				throw new ArgumentOutOfRangeException("rowIndex");
			}
			if (base.DataGridView == null)
			{
				return Rectangle.Empty;
			}
			object value = this.GetValue(rowIndex);
			DataGridViewAdvancedBorderStyle advancedBorderStyle;
			DataGridViewElementStates cellState;
			Rectangle rectangle;
			base.ComputeBorderStyleCellStateAndCellBounds(rowIndex, out advancedBorderStyle, out cellState, out rectangle);
			return this.PaintPrivate(graphics, rectangle, rectangle, rowIndex, cellState, value, null, cellStyle, advancedBorderStyle, DataGridViewPaintParts.ContentForeground, true, false, false);
		}

		/// <summary>Returns the bounding rectangle that encloses the cell's error icon, if one is displayed.</summary>
		/// <param name="graphics">The graphics context for the cell.</param>
		/// <param name="cellStyle">The <see cref="T:System.Windows.Forms.DataGridViewCellStyle" /> to be applied to the cell.</param>
		/// <param name="rowIndex">The index of the cell's parent row.</param>
		/// <returns>The <see cref="T:System.Drawing.Rectangle" /> that bounds the cell's error icon, if one is displayed; otherwise, <see cref="F:System.Drawing.Rectangle.Empty" />.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="rowIndex" /> does not equal -1.</exception>
		// Token: 0x0600201B RID: 8219 RVA: 0x000A061C File Offset: 0x0009E81C
		protected override Rectangle GetErrorIconBounds(Graphics graphics, DataGridViewCellStyle cellStyle, int rowIndex)
		{
			if (rowIndex != -1)
			{
				throw new ArgumentOutOfRangeException("rowIndex");
			}
			if (base.DataGridView == null)
			{
				return Rectangle.Empty;
			}
			if (cellStyle == null)
			{
				throw new ArgumentNullException("cellStyle");
			}
			DataGridViewAdvancedBorderStyle advancedBorderStyle;
			DataGridViewElementStates cellState;
			Rectangle rectangle;
			base.ComputeBorderStyleCellStateAndCellBounds(rowIndex, out advancedBorderStyle, out cellState, out rectangle);
			return this.PaintPrivate(graphics, rectangle, rectangle, rowIndex, cellState, null, this.GetErrorText(rowIndex), cellStyle, advancedBorderStyle, DataGridViewPaintParts.ContentForeground, false, true, false);
		}

		/// <summary>Calculates the preferred size, in pixels, of the cell.</summary>
		/// <param name="graphics">The <see cref="T:System.Drawing.Graphics" /> used to draw the cell.</param>
		/// <param name="cellStyle">A <see cref="T:System.Windows.Forms.DataGridViewCellStyle" /> that represents the style of the cell.</param>
		/// <param name="rowIndex">The zero-based row index of the cell.</param>
		/// <param name="constraintSize">The cell's maximum allowable size.</param>
		/// <returns>A <see cref="T:System.Drawing.Size" /> that represents the preferred size, in pixels, of the cell.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="rowIndex" /> does not equal -1.</exception>
		// Token: 0x0600201C RID: 8220 RVA: 0x000A067C File Offset: 0x0009E87C
		protected override Size GetPreferredSize(Graphics graphics, DataGridViewCellStyle cellStyle, int rowIndex, Size constraintSize)
		{
			if (rowIndex != -1)
			{
				throw new ArgumentOutOfRangeException("rowIndex");
			}
			if (base.DataGridView == null)
			{
				return new Size(-1, -1);
			}
			if (cellStyle == null)
			{
				throw new ArgumentNullException("cellStyle");
			}
			Rectangle rectangle = this.BorderWidths(base.DataGridView.AdjustedTopLeftHeaderBorderStyle);
			int borderAndPaddingWidths = rectangle.Left + rectangle.Width + cellStyle.Padding.Horizontal;
			int borderAndPaddingHeights = rectangle.Top + rectangle.Height + cellStyle.Padding.Vertical;
			TextFormatFlags flags = DataGridViewUtilities.ComputeTextFormatFlagsForCellStyleAlignment(base.DataGridView.RightToLeftInternal, cellStyle.Alignment, cellStyle.WrapMode);
			object obj = this.GetValue(rowIndex);
			if (!(obj is string))
			{
				obj = null;
			}
			return DataGridViewUtilities.GetPreferredRowHeaderSize(graphics, (string)obj, cellStyle, borderAndPaddingWidths, borderAndPaddingHeights, base.DataGridView.ShowCellErrors, false, constraintSize, flags);
		}

		/// <summary>Paints the current <see cref="T:System.Windows.Forms.DataGridViewTopLeftHeaderCell" />.</summary>
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
		// Token: 0x0600201D RID: 8221 RVA: 0x000A075C File Offset: 0x0009E95C
		protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
		{
			if (cellStyle == null)
			{
				throw new ArgumentNullException("cellStyle");
			}
			this.PaintPrivate(graphics, clipBounds, cellBounds, rowIndex, cellState, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts, false, false, true);
		}

		// Token: 0x0600201E RID: 8222 RVA: 0x000A0794 File Offset: 0x0009E994
		private Rectangle PaintPrivate(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts, bool computeContentBounds, bool computeErrorIconBounds, bool paint)
		{
			Rectangle result = Rectangle.Empty;
			Rectangle rectangle = cellBounds;
			Rectangle rectangle2 = this.BorderWidths(advancedBorderStyle);
			rectangle.Offset(rectangle2.X, rectangle2.Y);
			rectangle.Width -= rectangle2.Right;
			rectangle.Height -= rectangle2.Bottom;
			bool flag = (cellState & DataGridViewElementStates.Selected) > DataGridViewElementStates.None;
			if (paint && DataGridViewCell.PaintBackground(paintParts))
			{
				if (base.DataGridView.ApplyVisualStylesToHeaderCells)
				{
					int headerState = 1;
					if (base.ButtonState != ButtonState.Normal)
					{
						headerState = 3;
					}
					else if (base.DataGridView.MouseEnteredCellAddress.Y == rowIndex && base.DataGridView.MouseEnteredCellAddress.X == base.ColumnIndex)
					{
						headerState = 2;
					}
					rectangle.Inflate(16, 16);
					DataGridViewTopLeftHeaderCell.DataGridViewTopLeftHeaderCellRenderer.DrawHeader(graphics, rectangle, headerState);
					rectangle.Inflate(-16, -16);
				}
				else
				{
					SolidBrush cachedBrush = base.DataGridView.GetCachedBrush((DataGridViewCell.PaintSelectionBackground(paintParts) && flag) ? cellStyle.SelectionBackColor : cellStyle.BackColor);
					if (cachedBrush.Color.A == 255)
					{
						graphics.FillRectangle(cachedBrush, rectangle);
					}
				}
			}
			if (paint && DataGridViewCell.PaintBorder(paintParts))
			{
				this.PaintBorder(graphics, clipBounds, cellBounds, cellStyle, advancedBorderStyle);
			}
			if (cellStyle.Padding != Padding.Empty)
			{
				if (base.DataGridView.RightToLeftInternal)
				{
					rectangle.Offset(cellStyle.Padding.Right, cellStyle.Padding.Top);
				}
				else
				{
					rectangle.Offset(cellStyle.Padding.Left, cellStyle.Padding.Top);
				}
				rectangle.Width -= cellStyle.Padding.Horizontal;
				rectangle.Height -= cellStyle.Padding.Vertical;
			}
			Rectangle cellValueBounds = rectangle;
			string text = formattedValue as string;
			rectangle.Offset(1, 1);
			rectangle.Width -= 3;
			rectangle.Height -= 2;
			if (rectangle.Width > 0 && rectangle.Height > 0 && !string.IsNullOrEmpty(text) && (paint || computeContentBounds))
			{
				Color foreColor;
				if (base.DataGridView.ApplyVisualStylesToHeaderCells)
				{
					foreColor = DataGridViewTopLeftHeaderCell.DataGridViewTopLeftHeaderCellRenderer.VisualStyleRenderer.GetColor(ColorProperty.TextColor);
				}
				else
				{
					foreColor = (flag ? cellStyle.SelectionForeColor : cellStyle.ForeColor);
				}
				TextFormatFlags textFormatFlags = DataGridViewUtilities.ComputeTextFormatFlagsForCellStyleAlignment(base.DataGridView.RightToLeftInternal, cellStyle.Alignment, cellStyle.WrapMode);
				if (paint)
				{
					if (DataGridViewCell.PaintContentForeground(paintParts))
					{
						if ((textFormatFlags & TextFormatFlags.SingleLine) != TextFormatFlags.Default)
						{
							textFormatFlags |= TextFormatFlags.EndEllipsis;
						}
						TextRenderer.DrawText(graphics, text, cellStyle.Font, rectangle, foreColor, textFormatFlags);
					}
				}
				else
				{
					result = DataGridViewUtilities.GetTextBounds(rectangle, text, textFormatFlags, cellStyle);
				}
			}
			else if (computeErrorIconBounds && !string.IsNullOrEmpty(errorText))
			{
				result = base.ComputeErrorIconBounds(cellValueBounds);
			}
			if (base.DataGridView.ShowCellErrors && paint && DataGridViewCell.PaintErrorIcon(paintParts))
			{
				base.PaintErrorIcon(graphics, cellStyle, rowIndex, cellBounds, cellValueBounds, errorText);
			}
			return result;
		}

		/// <summary>Paints the border of the current <see cref="T:System.Windows.Forms.DataGridViewCell" />.</summary>
		/// <param name="graphics">The <see cref="T:System.Drawing.Graphics" /> used to paint the border.</param>
		/// <param name="clipBounds">A <see cref="T:System.Drawing.Rectangle" /> that represents the area of the <see cref="T:System.Windows.Forms.DataGridView" /> that needs to be repainted.</param>
		/// <param name="bounds">A <see cref="T:System.Drawing.Rectangle" /> that contains the area of the border that is being painted.</param>
		/// <param name="cellStyle">A <see cref="T:System.Windows.Forms.DataGridViewCellStyle" /> that contains formatting and style information about the cell.</param>
		/// <param name="advancedBorderStyle">A <see cref="T:System.Windows.Forms.DataGridViewAdvancedBorderStyle" /> that contains border styles of the border that is being painted.</param>
		// Token: 0x0600201F RID: 8223 RVA: 0x000A0AD0 File Offset: 0x0009ECD0
		protected override void PaintBorder(Graphics graphics, Rectangle clipBounds, Rectangle bounds, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle)
		{
			if (base.DataGridView == null)
			{
				return;
			}
			base.PaintBorder(graphics, clipBounds, bounds, cellStyle, advancedBorderStyle);
			if (!base.DataGridView.RightToLeftInternal && base.DataGridView.ApplyVisualStylesToHeaderCells)
			{
				if (base.DataGridView.AdvancedColumnHeadersBorderStyle.All == DataGridViewAdvancedCellBorderStyle.Inset)
				{
					Pen pen = null;
					Pen pen2 = null;
					base.GetContrastedPens(cellStyle.BackColor, ref pen, ref pen2);
					graphics.DrawLine(pen, bounds.X, bounds.Y, bounds.X, bounds.Bottom - 1);
					graphics.DrawLine(pen, bounds.X, bounds.Y, bounds.Right - 1, bounds.Y);
					return;
				}
				if (base.DataGridView.AdvancedColumnHeadersBorderStyle.All == DataGridViewAdvancedCellBorderStyle.Outset)
				{
					Pen pen3 = null;
					Pen pen4 = null;
					base.GetContrastedPens(cellStyle.BackColor, ref pen3, ref pen4);
					graphics.DrawLine(pen4, bounds.X, bounds.Y, bounds.X, bounds.Bottom - 1);
					graphics.DrawLine(pen4, bounds.X, bounds.Y, bounds.Right - 1, bounds.Y);
					return;
				}
				if (base.DataGridView.AdvancedColumnHeadersBorderStyle.All == DataGridViewAdvancedCellBorderStyle.InsetDouble)
				{
					Pen pen5 = null;
					Pen pen6 = null;
					base.GetContrastedPens(cellStyle.BackColor, ref pen5, ref pen6);
					graphics.DrawLine(pen5, bounds.X + 1, bounds.Y + 1, bounds.X + 1, bounds.Bottom - 1);
					graphics.DrawLine(pen5, bounds.X + 1, bounds.Y + 1, bounds.Right - 1, bounds.Y + 1);
				}
			}
		}

		/// <summary>Returns the string representation of the cell.</summary>
		/// <returns>A string that represents the current cell.</returns>
		// Token: 0x06002020 RID: 8224 RVA: 0x000A0C7B File Offset: 0x0009EE7B
		public override string ToString()
		{
			return "DataGridViewTopLeftHeaderCell";
		}

		// Token: 0x04000DD9 RID: 3545
		private static readonly VisualStyleElement HeaderElement = VisualStyleElement.Header.Item.Normal;

		// Token: 0x04000DDA RID: 3546
		private const byte DATAGRIDVIEWTOPLEFTHEADERCELL_horizontalTextMarginLeft = 1;

		// Token: 0x04000DDB RID: 3547
		private const byte DATAGRIDVIEWTOPLEFTHEADERCELL_horizontalTextMarginRight = 2;

		// Token: 0x04000DDC RID: 3548
		private const byte DATAGRIDVIEWTOPLEFTHEADERCELL_verticalTextMargin = 1;

		// Token: 0x020005C1 RID: 1473
		private class DataGridViewTopLeftHeaderCellRenderer
		{
			// Token: 0x060059DB RID: 23003 RVA: 0x000027DB File Offset: 0x000009DB
			private DataGridViewTopLeftHeaderCellRenderer()
			{
			}

			// Token: 0x170015C2 RID: 5570
			// (get) Token: 0x060059DC RID: 23004 RVA: 0x0017A797 File Offset: 0x00178997
			public static VisualStyleRenderer VisualStyleRenderer
			{
				get
				{
					if (DataGridViewTopLeftHeaderCell.DataGridViewTopLeftHeaderCellRenderer.visualStyleRenderer == null)
					{
						DataGridViewTopLeftHeaderCell.DataGridViewTopLeftHeaderCellRenderer.visualStyleRenderer = new VisualStyleRenderer(DataGridViewTopLeftHeaderCell.HeaderElement);
					}
					return DataGridViewTopLeftHeaderCell.DataGridViewTopLeftHeaderCellRenderer.visualStyleRenderer;
				}
			}

			// Token: 0x060059DD RID: 23005 RVA: 0x0017A7B4 File Offset: 0x001789B4
			public static void DrawHeader(Graphics g, Rectangle bounds, int headerState)
			{
				DataGridViewTopLeftHeaderCell.DataGridViewTopLeftHeaderCellRenderer.VisualStyleRenderer.SetParameters(DataGridViewTopLeftHeaderCell.HeaderElement.ClassName, DataGridViewTopLeftHeaderCell.HeaderElement.Part, headerState);
				DataGridViewTopLeftHeaderCell.DataGridViewTopLeftHeaderCellRenderer.VisualStyleRenderer.DrawBackground(g, bounds, Rectangle.Truncate(g.ClipBounds));
			}

			// Token: 0x0400394A RID: 14666
			private static VisualStyleRenderer visualStyleRenderer;
		}

		/// <summary>Provides information about a <see cref="T:System.Windows.Forms.DataGridViewTopLeftHeaderCell" /> to accessibility client applications.</summary>
		// Token: 0x020005C2 RID: 1474
		protected class DataGridViewTopLeftHeaderCellAccessibleObject : DataGridViewColumnHeaderCell.DataGridViewColumnHeaderCellAccessibleObject
		{
			/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DataGridViewTopLeftHeaderCell.DataGridViewTopLeftHeaderCellAccessibleObject" /> class. </summary>
			/// <param name="owner">The <see cref="T:System.Windows.Forms.DataGridViewTopLeftHeaderCell" /> that owns the <see cref="T:System.Windows.Forms.DataGridViewTopLeftHeaderCell.DataGridViewTopLeftHeaderCellAccessibleObject" />.</param>
			// Token: 0x060059DE RID: 23006 RVA: 0x0017A7EC File Offset: 0x001789EC
			public DataGridViewTopLeftHeaderCellAccessibleObject(DataGridViewTopLeftHeaderCell owner) : base(owner)
			{
			}

			/// <summary>Gets the location and size of the accessible object.</summary>
			/// <returns>A <see cref="T:System.Drawing.Rectangle" /> that represents the bounds of the accessible object.</returns>
			// Token: 0x170015C3 RID: 5571
			// (get) Token: 0x060059DF RID: 23007 RVA: 0x0017A7F8 File Offset: 0x001789F8
			public override Rectangle Bounds
			{
				get
				{
					Rectangle cellDisplayRectangle = base.Owner.DataGridView.GetCellDisplayRectangle(-1, -1, false);
					return base.Owner.DataGridView.RectangleToScreen(cellDisplayRectangle);
				}
			}

			/// <summary>Gets a description of the default action of the <see cref="T:System.Windows.Forms.DataGridViewTopLeftHeaderCell.DataGridViewTopLeftHeaderCellAccessibleObject" />.</summary>
			/// <returns>The string "Press to Select All" if the <see cref="P:System.Windows.Forms.DataGridView.MultiSelect" /> property is <see langword="true" />; otherwise, an empty string ("").</returns>
			// Token: 0x170015C4 RID: 5572
			// (get) Token: 0x060059E0 RID: 23008 RVA: 0x0017A82A File Offset: 0x00178A2A
			public override string DefaultAction
			{
				get
				{
					if (base.Owner.DataGridView.MultiSelect)
					{
						return SR.GetString("DataGridView_AccTopLeftColumnHeaderCellDefaultAction");
					}
					return string.Empty;
				}
			}

			/// <summary>Gets the name of the <see cref="T:System.Windows.Forms.DataGridViewTopLeftHeaderCell.DataGridViewTopLeftHeaderCellAccessibleObject" />.</summary>
			/// <returns>The name of the <see cref="T:System.Windows.Forms.DataGridViewTopLeftHeaderCell.DataGridViewTopLeftHeaderCellAccessibleObject" />.</returns>
			// Token: 0x170015C5 RID: 5573
			// (get) Token: 0x060059E1 RID: 23009 RVA: 0x0017A850 File Offset: 0x00178A50
			public override string Name
			{
				get
				{
					object value = base.Owner.Value;
					if (value != null && !(value is string))
					{
						return string.Empty;
					}
					string value2 = value as string;
					if (!string.IsNullOrEmpty(value2))
					{
						return string.Empty;
					}
					if (base.Owner.DataGridView == null)
					{
						return string.Empty;
					}
					if (base.Owner.DataGridView.RightToLeft == RightToLeft.No)
					{
						return SR.GetString("DataGridView_AccTopLeftColumnHeaderCellName");
					}
					return SR.GetString("DataGridView_AccTopLeftColumnHeaderCellNameRTL");
				}
			}

			/// <summary>Gets the state of the <see cref="T:System.Windows.Forms.DataGridViewTopLeftHeaderCell.DataGridViewTopLeftHeaderCellAccessibleObject" />.</summary>
			/// <returns>A bitwise combination of <see cref="T:System.Windows.Forms.AccessibleStates" /> values. The default is <see cref="F:System.Windows.Forms.AccessibleStates.Selectable" />.</returns>
			// Token: 0x170015C6 RID: 5574
			// (get) Token: 0x060059E2 RID: 23010 RVA: 0x0017A8CC File Offset: 0x00178ACC
			public override AccessibleStates State
			{
				get
				{
					AccessibleStates accessibleStates = AccessibleStates.Selectable;
					AccessibleStates state = base.State;
					if ((state & AccessibleStates.Offscreen) == AccessibleStates.Offscreen)
					{
						accessibleStates |= AccessibleStates.Offscreen;
					}
					if (base.Owner.DataGridView.AreAllCellsSelected(false))
					{
						accessibleStates |= AccessibleStates.Selected;
					}
					return accessibleStates;
				}
			}

			/// <summary>The value of the containing <see cref="T:System.Windows.Forms.DataGridViewTopLeftHeaderCell" />.</summary>
			/// <returns>Always returns <see cref="F:System.String.Empty" />.</returns>
			// Token: 0x170015C7 RID: 5575
			// (get) Token: 0x060059E3 RID: 23011 RVA: 0x00179039 File Offset: 0x00177239
			public override string Value
			{
				[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				get
				{
					return string.Empty;
				}
			}

			/// <summary>Performs the default action of the <see cref="T:System.Windows.Forms.DataGridViewTopLeftHeaderCell" />.</summary>
			// Token: 0x060059E4 RID: 23012 RVA: 0x0017A914 File Offset: 0x00178B14
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override void DoDefaultAction()
			{
				base.Owner.DataGridView.SelectAll();
			}

			/// <summary>Navigates to another accessible object.</summary>
			/// <param name="navigationDirection">One of the <see cref="T:System.Windows.Forms.AccessibleNavigation" /> values.</param>
			/// <returns>An <see cref="T:System.Windows.Forms.AccessibleObject" /> that represents an object in the specified direction.</returns>
			// Token: 0x060059E5 RID: 23013 RVA: 0x0017A928 File Offset: 0x00178B28
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override AccessibleObject Navigate(AccessibleNavigation navigationDirection)
			{
				switch (navigationDirection)
				{
				case AccessibleNavigation.Left:
					if (base.Owner.DataGridView.RightToLeft == RightToLeft.No)
					{
						return null;
					}
					return this.NavigateForward();
				case AccessibleNavigation.Right:
					if (base.Owner.DataGridView.RightToLeft == RightToLeft.No)
					{
						return this.NavigateForward();
					}
					return null;
				case AccessibleNavigation.Next:
					return this.NavigateForward();
				case AccessibleNavigation.Previous:
					return null;
				default:
					return null;
				}
			}

			// Token: 0x060059E6 RID: 23014 RVA: 0x0017A98F File Offset: 0x00178B8F
			private AccessibleObject NavigateForward()
			{
				if (base.Owner.DataGridView.Columns.GetColumnCount(DataGridViewElementStates.Visible) == 0)
				{
					return null;
				}
				return base.Owner.DataGridView.AccessibilityObject.GetChild(0).GetChild(1);
			}

			/// <summary>Modifies the selection in the <see cref="T:System.Windows.Forms.DataGridView" /> control or sets input focus to the control. </summary>
			/// <param name="flags">A bitwise combination of the <see cref="T:System.Windows.Forms.AccessibleSelection" /> values. </param>
			/// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Windows.Forms.DataGridViewCell.DataGridViewCellAccessibleObject.Owner" /> property value is <see langword="null" />.</exception>
			// Token: 0x060059E7 RID: 23015 RVA: 0x0017A9C8 File Offset: 0x00178BC8
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override void Select(AccessibleSelection flags)
			{
				if (base.Owner == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewCellAccessibleObject_OwnerNotSet"));
				}
				if ((flags & AccessibleSelection.TakeFocus) == AccessibleSelection.TakeFocus)
				{
					base.Owner.DataGridView.FocusInternal();
					if (base.Owner.DataGridView.Columns.GetColumnCount(DataGridViewElementStates.Visible) > 0 && base.Owner.DataGridView.Rows.GetRowCount(DataGridViewElementStates.Visible) > 0)
					{
						DataGridViewRow dataGridViewRow = base.Owner.DataGridView.Rows[base.Owner.DataGridView.Rows.GetFirstRow(DataGridViewElementStates.Visible)];
						DataGridViewColumn firstColumn = base.Owner.DataGridView.Columns.GetFirstColumn(DataGridViewElementStates.Visible);
						base.Owner.DataGridView.SetCurrentCellAddressCoreInternal(firstColumn.Index, dataGridViewRow.Index, false, true, false);
					}
				}
				if ((flags & AccessibleSelection.AddSelection) == AccessibleSelection.AddSelection && base.Owner.DataGridView.MultiSelect)
				{
					base.Owner.DataGridView.SelectAll();
				}
				if ((flags & AccessibleSelection.RemoveSelection) == AccessibleSelection.RemoveSelection && (flags & AccessibleSelection.AddSelection) == AccessibleSelection.None)
				{
					base.Owner.DataGridView.ClearSelection();
				}
			}

			// Token: 0x060059E8 RID: 23016 RVA: 0x0017AAE4 File Offset: 0x00178CE4
			internal override UnsafeNativeMethods.IRawElementProviderFragment FragmentNavigate(UnsafeNativeMethods.NavigateDirection direction)
			{
				DataGridView dataGridView = base.Owner.DataGridView;
				switch (direction)
				{
				case UnsafeNativeMethods.NavigateDirection.Parent:
					return dataGridView.AccessibilityObject.GetChild(0);
				case UnsafeNativeMethods.NavigateDirection.NextSibling:
					if (dataGridView.Columns.GetColumnCount(DataGridViewElementStates.Visible) == 0)
					{
						return null;
					}
					return this.NavigateForward();
				case UnsafeNativeMethods.NavigateDirection.PreviousSibling:
					return null;
				default:
					return null;
				}
			}

			// Token: 0x060059E9 RID: 23017 RVA: 0x0017AB3C File Offset: 0x00178D3C
			internal override object GetPropertyValue(int propertyId)
			{
				if (AccessibilityImprovements.Level3)
				{
					switch (propertyId)
					{
					case 30003:
						return 50034;
					case 30004:
					case 30006:
					case 30008:
					case 30011:
					case 30012:
						goto IL_99;
					case 30005:
						return this.Name;
					case 30007:
						return string.Empty;
					case 30009:
						break;
					case 30010:
						return base.Owner.DataGridView.Enabled;
					case 30013:
						return this.Help ?? string.Empty;
					default:
						if (propertyId != 30019 && propertyId != 30022)
						{
							goto IL_99;
						}
						break;
					}
					return false;
				}
				IL_99:
				return base.GetPropertyValue(propertyId);
			}
		}
	}
}
