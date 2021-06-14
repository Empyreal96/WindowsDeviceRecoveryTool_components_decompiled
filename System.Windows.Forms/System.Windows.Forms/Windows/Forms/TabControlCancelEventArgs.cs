using System;
using System.ComponentModel;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.TabControl.Selecting" /> and <see cref="E:System.Windows.Forms.TabControl.Deselecting" /> events of a <see cref="T:System.Windows.Forms.TabControl" /> control. </summary>
	// Token: 0x02000377 RID: 887
	public class TabControlCancelEventArgs : CancelEventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.TabControlCancelEventArgs" /> class. </summary>
		/// <param name="tabPage">The <see cref="T:System.Windows.Forms.TabPage" /> the event is occurring for.</param>
		/// <param name="tabPageIndex">The zero-based index of <paramref name="tabPage" /> in the <see cref="P:System.Windows.Forms.TabControl.TabPages" /> collection.</param>
		/// <param name="cancel">
		///       <see langword="true" /> to cancel the tab change by default; otherwise, <see langword="false" />.</param>
		/// <param name="action">One of the <see cref="T:System.Windows.Forms.TabControlAction" /> values.</param>
		// Token: 0x0600383A RID: 14394 RVA: 0x000FCDD1 File Offset: 0x000FAFD1
		public TabControlCancelEventArgs(TabPage tabPage, int tabPageIndex, bool cancel, TabControlAction action) : base(cancel)
		{
			this.tabPage = tabPage;
			this.tabPageIndex = tabPageIndex;
			this.action = action;
		}

		/// <summary>Gets the <see cref="T:System.Windows.Forms.TabPage" /> the event is occurring for.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.TabPage" /> the event is occurring for.</returns>
		// Token: 0x17000E06 RID: 3590
		// (get) Token: 0x0600383B RID: 14395 RVA: 0x000FCDF0 File Offset: 0x000FAFF0
		public TabPage TabPage
		{
			get
			{
				return this.tabPage;
			}
		}

		/// <summary>Gets the zero-based index of the <see cref="P:System.Windows.Forms.TabControlCancelEventArgs.TabPage" /> in the <see cref="P:System.Windows.Forms.TabControl.TabPages" /> collection.</summary>
		/// <returns>The zero-based index of the <see cref="P:System.Windows.Forms.TabControlCancelEventArgs.TabPage" /> in the <see cref="P:System.Windows.Forms.TabControl.TabPages" /> collection.</returns>
		// Token: 0x17000E07 RID: 3591
		// (get) Token: 0x0600383C RID: 14396 RVA: 0x000FCDF8 File Offset: 0x000FAFF8
		public int TabPageIndex
		{
			get
			{
				return this.tabPageIndex;
			}
		}

		/// <summary>Gets a value indicating which event is occurring. </summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.TabControlAction" /> values.</returns>
		// Token: 0x17000E08 RID: 3592
		// (get) Token: 0x0600383D RID: 14397 RVA: 0x000FCE00 File Offset: 0x000FB000
		public TabControlAction Action
		{
			get
			{
				return this.action;
			}
		}

		// Token: 0x04002253 RID: 8787
		private TabPage tabPage;

		// Token: 0x04002254 RID: 8788
		private int tabPageIndex;

		// Token: 0x04002255 RID: 8789
		private TabControlAction action;
	}
}
