using System;
using System.Runtime.InteropServices;
using System.Windows.Forms.Design;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.PropertyGrid.PropertyTabChanged" /> event of a <see cref="T:System.Windows.Forms.PropertyGrid" />.</summary>
	// Token: 0x02000322 RID: 802
	[ComVisible(true)]
	public class PropertyTabChangedEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.PropertyTabChangedEventArgs" /> class.</summary>
		/// <param name="oldTab">The Previously selected property tab. </param>
		/// <param name="newTab">The newly selected property tab. </param>
		// Token: 0x060031F6 RID: 12790 RVA: 0x000E9ECB File Offset: 0x000E80CB
		public PropertyTabChangedEventArgs(PropertyTab oldTab, PropertyTab newTab)
		{
			this.oldTab = oldTab;
			this.newTab = newTab;
		}

		/// <summary>Gets the old <see cref="T:System.Windows.Forms.Design.PropertyTab" /> selected.</summary>
		/// <returns>The old <see cref="T:System.Windows.Forms.Design.PropertyTab" /> that was selected.</returns>
		// Token: 0x17000C62 RID: 3170
		// (get) Token: 0x060031F7 RID: 12791 RVA: 0x000E9EE1 File Offset: 0x000E80E1
		public PropertyTab OldTab
		{
			get
			{
				return this.oldTab;
			}
		}

		/// <summary>Gets the new <see cref="T:System.Windows.Forms.Design.PropertyTab" /> selected.</summary>
		/// <returns>The newly selected <see cref="T:System.Windows.Forms.Design.PropertyTab" />.</returns>
		// Token: 0x17000C63 RID: 3171
		// (get) Token: 0x060031F8 RID: 12792 RVA: 0x000E9EE9 File Offset: 0x000E80E9
		public PropertyTab NewTab
		{
			get
			{
				return this.newTab;
			}
		}

		// Token: 0x04001E2B RID: 7723
		private PropertyTab oldTab;

		// Token: 0x04001E2C RID: 7724
		private PropertyTab newTab;
	}
}
