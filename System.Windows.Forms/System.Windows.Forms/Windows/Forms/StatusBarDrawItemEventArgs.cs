using System;
using System.Drawing;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.StatusBar.DrawItem" /> event.</summary>
	// Token: 0x02000364 RID: 868
	public class StatusBarDrawItemEventArgs : DrawItemEventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.StatusBarDrawItemEventArgs" /> class without specifying a background and foreground color for the <see cref="T:System.Windows.Forms.StatusBarPanel" />.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> to use to draw the <see cref="T:System.Windows.Forms.StatusBarPanel" />. </param>
		/// <param name="font">The <see cref="T:System.Drawing.Font" /> used to render text. </param>
		/// <param name="r">The <see cref="T:System.Drawing.Rectangle" /> that represents the client area of the <see cref="T:System.Windows.Forms.StatusBarPanel" />. </param>
		/// <param name="itemId">The zero-based index of the panel in the <see cref="T:System.Windows.Forms.StatusBar.StatusBarPanelCollection" /> of the <see cref="T:System.Windows.Forms.StatusBar" /> control. </param>
		/// <param name="itemState">One of the <see cref="T:System.Windows.Forms.DrawItemState" /> values that represents state information about the <see cref="T:System.Windows.Forms.StatusBarPanel" />. </param>
		/// <param name="panel">A <see cref="T:System.Windows.Forms.StatusBarPanel" /> that represents the panel to draw. </param>
		// Token: 0x060036BB RID: 14011 RVA: 0x000F84F3 File Offset: 0x000F66F3
		public StatusBarDrawItemEventArgs(Graphics g, Font font, Rectangle r, int itemId, DrawItemState itemState, StatusBarPanel panel) : base(g, font, r, itemId, itemState)
		{
			this.panel = panel;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.StatusBarDrawItemEventArgs" /> class with a specified background and foreground color for the <see cref="T:System.Windows.Forms.StatusBarPanel" />.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> to use to draw the <see cref="T:System.Windows.Forms.StatusBarPanel" />. </param>
		/// <param name="font">The <see cref="T:System.Drawing.Font" /> used to render text. </param>
		/// <param name="r">The <see cref="T:System.Drawing.Rectangle" /> that represents the client area of the <see cref="T:System.Windows.Forms.StatusBarPanel" />. </param>
		/// <param name="itemId">The zero-based index of the panel in the <see cref="T:System.Windows.Forms.StatusBar.StatusBarPanelCollection" /> of the <see cref="T:System.Windows.Forms.StatusBar" /> control. </param>
		/// <param name="itemState">One of the <see cref="T:System.Windows.Forms.DrawItemState" /> values that represents state information about the <see cref="T:System.Windows.Forms.StatusBarPanel" />. </param>
		/// <param name="panel">A <see cref="T:System.Windows.Forms.StatusBarPanel" /> that represents the panel to draw. </param>
		/// <param name="foreColor">One of the <see cref="T:System.Drawing.Color" /> values that represents the foreground color of the panel to draw. </param>
		/// <param name="backColor">One of the <see cref="T:System.Drawing.Color" /> values that represents the background color of the panel to draw. </param>
		// Token: 0x060036BC RID: 14012 RVA: 0x000F850A File Offset: 0x000F670A
		public StatusBarDrawItemEventArgs(Graphics g, Font font, Rectangle r, int itemId, DrawItemState itemState, StatusBarPanel panel, Color foreColor, Color backColor) : base(g, font, r, itemId, itemState, foreColor, backColor)
		{
			this.panel = panel;
		}

		/// <summary>Gets the <see cref="T:System.Windows.Forms.StatusBarPanel" /> to draw.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.StatusBarPanel" /> to draw.</returns>
		// Token: 0x17000D5F RID: 3423
		// (get) Token: 0x060036BD RID: 14013 RVA: 0x000F8525 File Offset: 0x000F6725
		public StatusBarPanel Panel
		{
			get
			{
				return this.panel;
			}
		}

		// Token: 0x040021C4 RID: 8644
		private readonly StatusBarPanel panel;
	}
}
