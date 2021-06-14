using System;
using System.ComponentModel;
using System.Globalization;

namespace System.Windows.Forms
{
	/// <summary>Represents a cell in a <see cref="T:System.Windows.Forms.TableLayoutPanel" />.</summary>
	// Token: 0x0200037F RID: 895
	[TypeConverter(typeof(TableLayoutPanelCellPositionTypeConverter))]
	public struct TableLayoutPanelCellPosition
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.TableLayoutPanelCellPosition" /> class.</summary>
		/// <param name="column">The column position of the cell.</param>
		/// <param name="row">The row position of the cell.</param>
		// Token: 0x06003879 RID: 14457 RVA: 0x000FD72C File Offset: 0x000FB92C
		public TableLayoutPanelCellPosition(int column, int row)
		{
			if (row < -1)
			{
				throw new ArgumentOutOfRangeException("row", SR.GetString("InvalidArgument", new object[]
				{
					"row",
					row.ToString(CultureInfo.CurrentCulture)
				}));
			}
			if (column < -1)
			{
				throw new ArgumentOutOfRangeException("column", SR.GetString("InvalidArgument", new object[]
				{
					"column",
					column.ToString(CultureInfo.CurrentCulture)
				}));
			}
			this.row = row;
			this.column = column;
		}

		/// <summary>Gets or sets the row number of the current <see cref="T:System.Windows.Forms.TableLayoutPanelCellPosition" />.</summary>
		/// <returns>The row number of the current <see cref="T:System.Windows.Forms.TableLayoutPanelCellPosition" />.</returns>
		// Token: 0x17000E18 RID: 3608
		// (get) Token: 0x0600387A RID: 14458 RVA: 0x000FD7B3 File Offset: 0x000FB9B3
		// (set) Token: 0x0600387B RID: 14459 RVA: 0x000FD7BB File Offset: 0x000FB9BB
		public int Row
		{
			get
			{
				return this.row;
			}
			set
			{
				this.row = value;
			}
		}

		/// <summary>Gets or sets the column number of the current <see cref="T:System.Windows.Forms.TableLayoutPanelCellPosition" />.</summary>
		/// <returns>The column number of the current <see cref="T:System.Windows.Forms.TableLayoutPanelCellPosition" />.</returns>
		// Token: 0x17000E19 RID: 3609
		// (get) Token: 0x0600387C RID: 14460 RVA: 0x000FD7C4 File Offset: 0x000FB9C4
		// (set) Token: 0x0600387D RID: 14461 RVA: 0x000FD7CC File Offset: 0x000FB9CC
		public int Column
		{
			get
			{
				return this.column;
			}
			set
			{
				this.column = value;
			}
		}

		/// <summary>Specifies whether this <see cref="T:System.Windows.Forms.TableLayoutPanelCellPosition" /> contains the same row and column as the specified <see cref="T:System.Windows.Forms.TableLayoutPanelCellPosition" />.</summary>
		/// <param name="other">The <see cref="T:System.Windows.Forms.TableLayoutPanelCellPosition" /> to test.</param>
		/// <returns>
		///     <see langword="true" /> if <paramref name="other" /> is a <see cref="T:System.Windows.Forms.TableLayoutPanelCellPosition" /> and has the same row and column as the specified <see cref="T:System.Windows.Forms.TableLayoutPanelCellPosition" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600387E RID: 14462 RVA: 0x000FD7D8 File Offset: 0x000FB9D8
		public override bool Equals(object other)
		{
			if (other is TableLayoutPanelCellPosition)
			{
				TableLayoutPanelCellPosition tableLayoutPanelCellPosition = (TableLayoutPanelCellPosition)other;
				return tableLayoutPanelCellPosition.row == this.row && tableLayoutPanelCellPosition.column == this.column;
			}
			return false;
		}

		/// <summary>Compares two <see cref="T:System.Windows.Forms.TableLayoutPanelCellPosition" /> objects. The result specifies whether the values of the <see cref="P:System.Windows.Forms.TableLayoutPanelCellPosition.Row" /> and <see cref="P:System.Windows.Forms.TableLayoutPanelCellPosition.Column" /> properties of the two <see cref="T:System.Windows.Forms.TableLayoutPanelCellPosition" /> objects are equal.</summary>
		/// <param name="p1">A <see cref="T:System.Windows.Forms.TableLayoutPanelCellPosition" /> to compare.</param>
		/// <param name="p2">A <see cref="T:System.Windows.Forms.TableLayoutPanelCellPosition" /> to compare.</param>
		/// <returns>
		///     <see langword="true" /> if <paramref name="p1" /> and <paramref name="p2" /> are equal; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600387F RID: 14463 RVA: 0x000FD814 File Offset: 0x000FBA14
		public static bool operator ==(TableLayoutPanelCellPosition p1, TableLayoutPanelCellPosition p2)
		{
			return p1.Row == p2.Row && p1.Column == p2.Column;
		}

		/// <summary>Compares two <see cref="T:System.Windows.Forms.TableLayoutPanelCellPosition" /> objects. The result specifies whether the values of the <see cref="P:System.Windows.Forms.TableLayoutPanelCellPosition.Row" /> and <see cref="P:System.Windows.Forms.TableLayoutPanelCellPosition.Column" /> properties of the two <see cref="T:System.Windows.Forms.TableLayoutPanelCellPosition" /> objects are unequal.</summary>
		/// <param name="p1">A <see cref="T:System.Windows.Forms.TableLayoutPanelCellPosition" /> to compare.</param>
		/// <param name="p2">A <see cref="T:System.Windows.Forms.TableLayoutPanelCellPosition" /> to compare.</param>
		/// <returns>
		///     <see langword="true" /> if <paramref name="p1" /> and <paramref name="p2" /> differ; otherwise, <see langword="false" />.</returns>
		// Token: 0x06003880 RID: 14464 RVA: 0x000FD838 File Offset: 0x000FBA38
		public static bool operator !=(TableLayoutPanelCellPosition p1, TableLayoutPanelCellPosition p2)
		{
			return !(p1 == p2);
		}

		/// <summary>Converts this <see cref="T:System.Windows.Forms.TableLayoutPanelCellPosition" /> to a human readable string.</summary>
		/// <returns>A string that represents this <see cref="T:System.Windows.Forms.TableLayoutPanelCellPosition" />.</returns>
		// Token: 0x06003881 RID: 14465 RVA: 0x000FD844 File Offset: 0x000FBA44
		public override string ToString()
		{
			return this.Column.ToString(CultureInfo.CurrentCulture) + "," + this.Row.ToString(CultureInfo.CurrentCulture);
		}

		/// <summary>Returns a hash code for this <see cref="T:System.Windows.Forms.TableLayoutPanelCellPosition" />.</summary>
		/// <returns>An integer value that specifies a hash value for this <see cref="T:System.Windows.Forms.TableLayoutPanelCellPosition" />.</returns>
		// Token: 0x06003882 RID: 14466 RVA: 0x000FD881 File Offset: 0x000FBA81
		public override int GetHashCode()
		{
			return WindowsFormsUtils.GetCombinedHashCodes(new int[]
			{
				this.row,
				this.column
			});
		}

		// Token: 0x04002267 RID: 8807
		private int row;

		// Token: 0x04002268 RID: 8808
		private int column;
	}
}
