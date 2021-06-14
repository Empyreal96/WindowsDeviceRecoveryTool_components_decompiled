using System;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.PropertyGrid.PropertyValueChanged" /> event of a <see cref="T:System.Windows.Forms.PropertyGrid" />.</summary>
	// Token: 0x02000324 RID: 804
	[ComVisible(true)]
	public class PropertyValueChangedEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.PropertyValueChangedEventArgs" /> class.</summary>
		/// <param name="changedItem">The item in the grid that changed. </param>
		/// <param name="oldValue">The old property value. </param>
		// Token: 0x060031FD RID: 12797 RVA: 0x000E9EF1 File Offset: 0x000E80F1
		public PropertyValueChangedEventArgs(GridItem changedItem, object oldValue)
		{
			this.changedItem = changedItem;
			this.oldValue = oldValue;
		}

		/// <summary>Gets the <see cref="T:System.Windows.Forms.GridItem" /> that was changed.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.GridItem" /> in the <see cref="T:System.Windows.Forms.PropertyGrid" />.</returns>
		// Token: 0x17000C64 RID: 3172
		// (get) Token: 0x060031FE RID: 12798 RVA: 0x000E9F07 File Offset: 0x000E8107
		public GridItem ChangedItem
		{
			get
			{
				return this.changedItem;
			}
		}

		/// <summary>The value of the grid item before it was changed.</summary>
		/// <returns>A object representing the old value of the property.</returns>
		// Token: 0x17000C65 RID: 3173
		// (get) Token: 0x060031FF RID: 12799 RVA: 0x000E9F0F File Offset: 0x000E810F
		public object OldValue
		{
			get
			{
				return this.oldValue;
			}
		}

		// Token: 0x04001E2D RID: 7725
		private readonly GridItem changedItem;

		// Token: 0x04001E2E RID: 7726
		private object oldValue;
	}
}
