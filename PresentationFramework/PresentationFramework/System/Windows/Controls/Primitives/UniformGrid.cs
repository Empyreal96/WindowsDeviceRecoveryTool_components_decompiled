using System;

namespace System.Windows.Controls.Primitives
{
	/// <summary>Provides a way to arrange content in a grid where all the cells in the grid have the same size.</summary>
	// Token: 0x020005B5 RID: 1461
	public class UniformGrid : Panel
	{
		/// <summary>Gets or sets the number of leading blank cells in the first row of the grid.  </summary>
		/// <returns>The number of empty cells that are in the first row of the grid. The default is 0.</returns>
		// Token: 0x17001769 RID: 5993
		// (get) Token: 0x06006145 RID: 24901 RVA: 0x001B4E71 File Offset: 0x001B3071
		// (set) Token: 0x06006146 RID: 24902 RVA: 0x001B4E83 File Offset: 0x001B3083
		public int FirstColumn
		{
			get
			{
				return (int)base.GetValue(UniformGrid.FirstColumnProperty);
			}
			set
			{
				base.SetValue(UniformGrid.FirstColumnProperty, value);
			}
		}

		// Token: 0x06006147 RID: 24903 RVA: 0x0015A58B File Offset: 0x0015878B
		private static bool ValidateFirstColumn(object o)
		{
			return (int)o >= 0;
		}

		/// <summary>Gets or sets the number of columns that are in the grid.  </summary>
		/// <returns>The number of columns that are in the grid. The default is 0. </returns>
		// Token: 0x1700176A RID: 5994
		// (get) Token: 0x06006148 RID: 24904 RVA: 0x001B4E96 File Offset: 0x001B3096
		// (set) Token: 0x06006149 RID: 24905 RVA: 0x001B4EA8 File Offset: 0x001B30A8
		public int Columns
		{
			get
			{
				return (int)base.GetValue(UniformGrid.ColumnsProperty);
			}
			set
			{
				base.SetValue(UniformGrid.ColumnsProperty, value);
			}
		}

		// Token: 0x0600614A RID: 24906 RVA: 0x0015A58B File Offset: 0x0015878B
		private static bool ValidateColumns(object o)
		{
			return (int)o >= 0;
		}

		/// <summary>Gets or sets the number of rows that are in the grid.  </summary>
		/// <returns>The number of rows that are in the grid. The default is 0.</returns>
		// Token: 0x1700176B RID: 5995
		// (get) Token: 0x0600614B RID: 24907 RVA: 0x001B4EBB File Offset: 0x001B30BB
		// (set) Token: 0x0600614C RID: 24908 RVA: 0x001B4ECD File Offset: 0x001B30CD
		public int Rows
		{
			get
			{
				return (int)base.GetValue(UniformGrid.RowsProperty);
			}
			set
			{
				base.SetValue(UniformGrid.RowsProperty, value);
			}
		}

		// Token: 0x0600614D RID: 24909 RVA: 0x0015A58B File Offset: 0x0015878B
		private static bool ValidateRows(object o)
		{
			return (int)o >= 0;
		}

		/// <summary>Computes the desired size of the <see cref="T:System.Windows.Controls.Primitives.UniformGrid" /> by measuring all of the child elements.</summary>
		/// <param name="constraint">The <see cref="T:System.Windows.Size" /> of the available area for the grid.</param>
		/// <returns>The desired <see cref="T:System.Windows.Size" /> based on the child content of the grid and the <paramref name="constraint" /> parameter.</returns>
		// Token: 0x0600614E RID: 24910 RVA: 0x001B4EE0 File Offset: 0x001B30E0
		protected override Size MeasureOverride(Size constraint)
		{
			this.UpdateComputedValues();
			Size availableSize = new Size(constraint.Width / (double)this._columns, constraint.Height / (double)this._rows);
			double num = 0.0;
			double num2 = 0.0;
			int i = 0;
			int count = base.InternalChildren.Count;
			while (i < count)
			{
				UIElement uielement = base.InternalChildren[i];
				uielement.Measure(availableSize);
				Size desiredSize = uielement.DesiredSize;
				if (num < desiredSize.Width)
				{
					num = desiredSize.Width;
				}
				if (num2 < desiredSize.Height)
				{
					num2 = desiredSize.Height;
				}
				i++;
			}
			return new Size(num * (double)this._columns, num2 * (double)this._rows);
		}

		/// <summary>Defines the layout of the <see cref="T:System.Windows.Controls.Primitives.UniformGrid" /> by distributing space evenly among all of the child elements.</summary>
		/// <param name="arrangeSize">The <see cref="T:System.Windows.Size" /> of the area for the grid to use.</param>
		/// <returns>The actual <see cref="T:System.Windows.Size" /> of the grid that is rendered to display the child elements that are visible.</returns>
		// Token: 0x0600614F RID: 24911 RVA: 0x001B4FA0 File Offset: 0x001B31A0
		protected override Size ArrangeOverride(Size arrangeSize)
		{
			Rect finalRect = new Rect(0.0, 0.0, arrangeSize.Width / (double)this._columns, arrangeSize.Height / (double)this._rows);
			double width = finalRect.Width;
			double num = arrangeSize.Width - 1.0;
			finalRect.X += finalRect.Width * (double)this.FirstColumn;
			foreach (object obj in base.InternalChildren)
			{
				UIElement uielement = (UIElement)obj;
				uielement.Arrange(finalRect);
				if (uielement.Visibility != Visibility.Collapsed)
				{
					finalRect.X += width;
					if (finalRect.X >= num)
					{
						finalRect.Y += finalRect.Height;
						finalRect.X = 0.0;
					}
				}
			}
			return arrangeSize;
		}

		// Token: 0x06006150 RID: 24912 RVA: 0x001B50B8 File Offset: 0x001B32B8
		private void UpdateComputedValues()
		{
			this._columns = this.Columns;
			this._rows = this.Rows;
			if (this.FirstColumn >= this._columns)
			{
				this.FirstColumn = 0;
			}
			if (this._rows == 0 || this._columns == 0)
			{
				int num = 0;
				int i = 0;
				int count = base.InternalChildren.Count;
				while (i < count)
				{
					UIElement uielement = base.InternalChildren[i];
					if (uielement.Visibility != Visibility.Collapsed)
					{
						num++;
					}
					i++;
				}
				if (num == 0)
				{
					num = 1;
				}
				if (this._rows == 0)
				{
					if (this._columns > 0)
					{
						this._rows = (num + this.FirstColumn + (this._columns - 1)) / this._columns;
						return;
					}
					this._rows = (int)Math.Sqrt((double)num);
					if (this._rows * this._rows < num)
					{
						this._rows++;
					}
					this._columns = this._rows;
					return;
				}
				else if (this._columns == 0)
				{
					this._columns = (num + (this._rows - 1)) / this._rows;
				}
			}
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.UniformGrid.FirstColumn" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.UniformGrid.FirstColumn" /> dependency property.</returns>
		// Token: 0x04003147 RID: 12615
		public static readonly DependencyProperty FirstColumnProperty = DependencyProperty.Register("FirstColumn", typeof(int), typeof(UniformGrid), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsMeasure), new ValidateValueCallback(UniformGrid.ValidateFirstColumn));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.UniformGrid.Columns" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.UniformGrid.Columns" /> dependency property.</returns>
		// Token: 0x04003148 RID: 12616
		public static readonly DependencyProperty ColumnsProperty = DependencyProperty.Register("Columns", typeof(int), typeof(UniformGrid), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsMeasure), new ValidateValueCallback(UniformGrid.ValidateColumns));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.UniformGrid.Rows" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.UniformGrid.Rows" /> dependency property.</returns>
		// Token: 0x04003149 RID: 12617
		public static readonly DependencyProperty RowsProperty = DependencyProperty.Register("Rows", typeof(int), typeof(UniformGrid), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsMeasure), new ValidateValueCallback(UniformGrid.ValidateRows));

		// Token: 0x0400314A RID: 12618
		private int _rows;

		// Token: 0x0400314B RID: 12619
		private int _columns;
	}
}
