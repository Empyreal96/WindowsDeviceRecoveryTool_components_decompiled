using System;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.TabControl.Selected" /> and <see cref="E:System.Windows.Forms.TabControl.Deselected" /> events of a <see cref="T:System.Windows.Forms.TabControl" /> control. </summary>
	// Token: 0x02000379 RID: 889
	public class TabControlEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.TabControlEventArgs" /> class. </summary>
		/// <param name="tabPage">The <see cref="T:System.Windows.Forms.TabPage" /> the event is occurring for.</param>
		/// <param name="tabPageIndex">The zero-based index of <paramref name="tabPage" /> in the <see cref="P:System.Windows.Forms.TabControl.TabPages" /> collection.</param>
		/// <param name="action">One of the <see cref="T:System.Windows.Forms.TabControlAction" /> values.</param>
		// Token: 0x06003842 RID: 14402 RVA: 0x000FCE08 File Offset: 0x000FB008
		public TabControlEventArgs(TabPage tabPage, int tabPageIndex, TabControlAction action)
		{
			this.tabPage = tabPage;
			this.tabPageIndex = tabPageIndex;
			this.action = action;
		}

		/// <summary>Gets the <see cref="T:System.Windows.Forms.TabPage" /> the event is occurring for.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.TabPage" /> the event is occurring for.</returns>
		// Token: 0x17000E09 RID: 3593
		// (get) Token: 0x06003843 RID: 14403 RVA: 0x000FCE25 File Offset: 0x000FB025
		public TabPage TabPage
		{
			get
			{
				return this.tabPage;
			}
		}

		/// <summary>Gets the zero-based index of the <see cref="P:System.Windows.Forms.TabControlEventArgs.TabPage" /> in the <see cref="P:System.Windows.Forms.TabControl.TabPages" /> collection.</summary>
		/// <returns>The zero-based index of the <see cref="P:System.Windows.Forms.TabControlEventArgs.TabPage" /> in the <see cref="P:System.Windows.Forms.TabControl.TabPages" /> collection.</returns>
		// Token: 0x17000E0A RID: 3594
		// (get) Token: 0x06003844 RID: 14404 RVA: 0x000FCE2D File Offset: 0x000FB02D
		public int TabPageIndex
		{
			get
			{
				return this.tabPageIndex;
			}
		}

		/// <summary>Gets a value indicating which event is occurring. </summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.TabControlAction" /> values.</returns>
		// Token: 0x17000E0B RID: 3595
		// (get) Token: 0x06003845 RID: 14405 RVA: 0x000FCE35 File Offset: 0x000FB035
		public TabControlAction Action
		{
			get
			{
				return this.action;
			}
		}

		// Token: 0x04002256 RID: 8790
		private TabPage tabPage;

		// Token: 0x04002257 RID: 8791
		private int tabPageIndex;

		// Token: 0x04002258 RID: 8792
		private TabControlAction action;
	}
}
