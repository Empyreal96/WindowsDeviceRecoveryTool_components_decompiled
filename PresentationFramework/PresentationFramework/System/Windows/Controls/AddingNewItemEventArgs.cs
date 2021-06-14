using System;

namespace System.Windows.Controls
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Controls.DataGrid.AddingNewItem" /> event.</summary>
	// Token: 0x0200046B RID: 1131
	public class AddingNewItemEventArgs : EventArgs
	{
		/// <summary>Gets or sets the item that will be added.</summary>
		/// <returns>The item that will be added.</returns>
		// Token: 0x17001042 RID: 4162
		// (get) Token: 0x06004221 RID: 16929 RVA: 0x0012EAFA File Offset: 0x0012CCFA
		// (set) Token: 0x06004222 RID: 16930 RVA: 0x0012EB02 File Offset: 0x0012CD02
		public object NewItem
		{
			get
			{
				return this._newItem;
			}
			set
			{
				this._newItem = value;
			}
		}

		// Token: 0x040027D5 RID: 10197
		private object _newItem;
	}
}
