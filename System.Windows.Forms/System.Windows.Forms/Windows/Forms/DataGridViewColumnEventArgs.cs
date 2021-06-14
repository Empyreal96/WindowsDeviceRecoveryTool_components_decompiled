using System;

namespace System.Windows.Forms
{
	/// <summary>Provides data for column-related events of a <see cref="T:System.Windows.Forms.DataGridView" />. </summary>
	// Token: 0x020001B3 RID: 435
	public class DataGridViewColumnEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DataGridViewColumnEventArgs" /> class. </summary>
		/// <param name="dataGridViewColumn">The column that the event occurs for.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="dataGridViewColumn" /> is <see langword="null" />.</exception>
		// Token: 0x06001C5B RID: 7259 RVA: 0x0008D7FD File Offset: 0x0008B9FD
		public DataGridViewColumnEventArgs(DataGridViewColumn dataGridViewColumn)
		{
			if (dataGridViewColumn == null)
			{
				throw new ArgumentNullException("dataGridViewColumn");
			}
			this.dataGridViewColumn = dataGridViewColumn;
		}

		/// <summary>Gets the column that the event occurs for.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.DataGridViewColumn" /> that the event occurs for.</returns>
		// Token: 0x170006A0 RID: 1696
		// (get) Token: 0x06001C5C RID: 7260 RVA: 0x0008D81A File Offset: 0x0008BA1A
		public DataGridViewColumn Column
		{
			get
			{
				return this.dataGridViewColumn;
			}
		}

		// Token: 0x04000C97 RID: 3223
		private DataGridViewColumn dataGridViewColumn;
	}
}
