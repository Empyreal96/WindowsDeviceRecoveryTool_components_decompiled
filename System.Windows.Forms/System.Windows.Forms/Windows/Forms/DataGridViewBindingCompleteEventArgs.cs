using System;
using System.ComponentModel;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.DataGridView.DataBindingComplete" /> event.</summary>
	// Token: 0x0200018F RID: 399
	public class DataGridViewBindingCompleteEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DataGridViewBindingCompleteEventArgs" /> class.</summary>
		/// <param name="listChangedType">One of the <see cref="T:System.ComponentModel.ListChangedType" /> values.</param>
		// Token: 0x060019A8 RID: 6568 RVA: 0x0007FA03 File Offset: 0x0007DC03
		public DataGridViewBindingCompleteEventArgs(ListChangedType listChangedType)
		{
			this.listChangedType = listChangedType;
		}

		/// <summary>Gets a value specifying how the list changed.</summary>
		/// <returns>One of the <see cref="T:System.ComponentModel.ListChangedType" /> values.</returns>
		// Token: 0x170005CC RID: 1484
		// (get) Token: 0x060019A9 RID: 6569 RVA: 0x0007FA12 File Offset: 0x0007DC12
		public ListChangedType ListChangedType
		{
			get
			{
				return this.listChangedType;
			}
		}

		// Token: 0x04000BC7 RID: 3015
		private ListChangedType listChangedType;
	}
}
