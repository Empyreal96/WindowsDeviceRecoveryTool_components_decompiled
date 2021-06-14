using System;

namespace System.Windows.Controls
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Controls.DataGrid.InitializingNewItem" /> event.</summary>
	// Token: 0x020004E8 RID: 1256
	public class InitializingNewItemEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.InitializingNewItemEventArgs" /> class. </summary>
		/// <param name="newItem">The new item added to the <see cref="T:System.Windows.Controls.DataGrid" />.</param>
		// Token: 0x06004E88 RID: 20104 RVA: 0x001614A5 File Offset: 0x0015F6A5
		public InitializingNewItemEventArgs(object newItem)
		{
			this._newItem = newItem;
		}

		/// <summary>Gets the new item added to the <see cref="T:System.Windows.Controls.DataGrid" />.</summary>
		/// <returns>The new item added to the grid.</returns>
		// Token: 0x17001320 RID: 4896
		// (get) Token: 0x06004E89 RID: 20105 RVA: 0x001614B4 File Offset: 0x0015F6B4
		public object NewItem
		{
			get
			{
				return this._newItem;
			}
		}

		// Token: 0x04002BCC RID: 11212
		private object _newItem;
	}
}
