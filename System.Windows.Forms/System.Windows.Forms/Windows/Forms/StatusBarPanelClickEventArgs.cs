using System;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.StatusBar.PanelClick" /> event.</summary>
	// Token: 0x02000369 RID: 873
	public class StatusBarPanelClickEventArgs : MouseEventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.StatusBarPanelClickEventArgs" /> class.</summary>
		/// <param name="statusBarPanel">The <see cref="T:System.Windows.Forms.StatusBarPanel" /> that represents the panel that was clicked. </param>
		/// <param name="button">One of the <see cref="T:System.Windows.Forms.MouseButtons" /> values that represents the mouse buttons that were clicked while over the <see cref="T:System.Windows.Forms.StatusBarPanel" />. </param>
		/// <param name="clicks">The number of times that the mouse button was clicked. </param>
		/// <param name="x">The x-coordinate of the mouse click. </param>
		/// <param name="y">The y-coordinate of the mouse click. </param>
		// Token: 0x060036E9 RID: 14057 RVA: 0x000F8D57 File Offset: 0x000F6F57
		public StatusBarPanelClickEventArgs(StatusBarPanel statusBarPanel, MouseButtons button, int clicks, int x, int y) : base(button, clicks, x, y, 0)
		{
			this.statusBarPanel = statusBarPanel;
		}

		/// <summary>Gets the <see cref="T:System.Windows.Forms.StatusBarPanel" /> to draw.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.StatusBarPanel" /> to draw.</returns>
		// Token: 0x17000D70 RID: 3440
		// (get) Token: 0x060036EA RID: 14058 RVA: 0x000F8D6D File Offset: 0x000F6F6D
		public StatusBarPanel StatusBarPanel
		{
			get
			{
				return this.statusBarPanel;
			}
		}

		// Token: 0x040021E0 RID: 8672
		private readonly StatusBarPanel statusBarPanel;
	}
}
