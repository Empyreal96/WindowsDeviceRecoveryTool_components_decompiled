using System;
using System.Globalization;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.DataGridView.RowHeightInfoNeeded" /> event of a <see cref="T:System.Windows.Forms.DataGridView" />. </summary>
	// Token: 0x02000200 RID: 512
	public class DataGridViewRowHeightInfoNeededEventArgs : EventArgs
	{
		// Token: 0x06001F47 RID: 8007 RVA: 0x0009E2DB File Offset: 0x0009C4DB
		internal DataGridViewRowHeightInfoNeededEventArgs()
		{
			this.rowIndex = -1;
			this.height = -1;
			this.minimumHeight = -1;
		}

		/// <summary>Gets or sets the height of the row the event occurred for.</summary>
		/// <returns>The row height. </returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The specified value when setting this property is greater than 65,536. </exception>
		// Token: 0x1700075B RID: 1883
		// (get) Token: 0x06001F48 RID: 8008 RVA: 0x0009E2F8 File Offset: 0x0009C4F8
		// (set) Token: 0x06001F49 RID: 8009 RVA: 0x0009E300 File Offset: 0x0009C500
		public int Height
		{
			get
			{
				return this.height;
			}
			set
			{
				if (value < this.minimumHeight)
				{
					value = this.minimumHeight;
				}
				if (value > 65536)
				{
					throw new ArgumentOutOfRangeException("Height", SR.GetString("InvalidHighBoundArgumentEx", new object[]
					{
						"Height",
						value.ToString(CultureInfo.CurrentCulture),
						65536.ToString(CultureInfo.CurrentCulture)
					}));
				}
				this.height = value;
			}
		}

		/// <summary>Gets or sets the minimum height of the row the event occurred for. </summary>
		/// <returns>The minimum row height.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The specified value when setting this property is less than 2.</exception>
		// Token: 0x1700075C RID: 1884
		// (get) Token: 0x06001F4A RID: 8010 RVA: 0x0009E374 File Offset: 0x0009C574
		// (set) Token: 0x06001F4B RID: 8011 RVA: 0x0009E37C File Offset: 0x0009C57C
		public int MinimumHeight
		{
			get
			{
				return this.minimumHeight;
			}
			set
			{
				if (value < 2)
				{
					throw new ArgumentOutOfRangeException("MinimumHeight", value, SR.GetString("DataGridViewBand_MinimumHeightSmallerThanOne", new object[]
					{
						2.ToString(CultureInfo.CurrentCulture)
					}));
				}
				if (this.height < value)
				{
					this.height = value;
				}
				this.minimumHeight = value;
			}
		}

		/// <summary>Gets the index of the row associated with this <see cref="T:System.Windows.Forms.DataGridViewRowHeightInfoNeededEventArgs" />.</summary>
		/// <returns>The zero-based index of the row the event occurred for.</returns>
		// Token: 0x1700075D RID: 1885
		// (get) Token: 0x06001F4C RID: 8012 RVA: 0x0009E3D6 File Offset: 0x0009C5D6
		public int RowIndex
		{
			get
			{
				return this.rowIndex;
			}
		}

		// Token: 0x06001F4D RID: 8013 RVA: 0x0009E3DE File Offset: 0x0009C5DE
		internal void SetProperties(int rowIndex, int height, int minimumHeight)
		{
			this.rowIndex = rowIndex;
			this.height = height;
			this.minimumHeight = minimumHeight;
		}

		// Token: 0x04000D8E RID: 3470
		private int rowIndex;

		// Token: 0x04000D8F RID: 3471
		private int height;

		// Token: 0x04000D90 RID: 3472
		private int minimumHeight;
	}
}
