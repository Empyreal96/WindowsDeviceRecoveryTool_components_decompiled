using System;
using System.Drawing;

namespace System.Windows.Forms
{
	/// <summary>Provides data for <see cref="T:System.Windows.Forms.ToolStripPanel" /> drawing.</summary>
	// Token: 0x020003DF RID: 991
	public class ToolStripPanelRenderEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripPanelRenderEventArgs" /> class for the specified <see cref="T:System.Windows.Forms.ToolStripPanel" /> that uses the specified graphics for drawing. </summary>
		/// <param name="g">The graphics used to paint the <see cref="T:System.Windows.Forms.ToolStripPanel" />.</param>
		/// <param name="toolStripPanel">The <see cref="T:System.Windows.Forms.ToolStripPanel" /> to draw.</param>
		// Token: 0x06004265 RID: 16997 RVA: 0x0011C5D4 File Offset: 0x0011A7D4
		public ToolStripPanelRenderEventArgs(Graphics g, ToolStripPanel toolStripPanel)
		{
			this.toolStripPanel = toolStripPanel;
			this.graphics = g;
		}

		/// <summary>Gets or sets the graphics used to paint the <see cref="T:System.Windows.Forms.ToolStripPanel" />.</summary>
		/// <returns>The <see cref="T:System.Drawing.Graphics" /> used to paint.</returns>
		// Token: 0x170010A9 RID: 4265
		// (get) Token: 0x06004266 RID: 16998 RVA: 0x0011C5EA File Offset: 0x0011A7EA
		public Graphics Graphics
		{
			get
			{
				return this.graphics;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Forms.ToolStripPanel" /> to paint.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.ToolStripPanel" /> to paint.</returns>
		// Token: 0x170010AA RID: 4266
		// (get) Token: 0x06004267 RID: 16999 RVA: 0x0011C5F2 File Offset: 0x0011A7F2
		public ToolStripPanel ToolStripPanel
		{
			get
			{
				return this.toolStripPanel;
			}
		}

		/// <summary>Gets or sets a value indicating whether the event was handled.</summary>
		/// <returns>
		///     <see langword="true" /> if the event was handled; otherwise, <see langword="false" />. </returns>
		// Token: 0x170010AB RID: 4267
		// (get) Token: 0x06004268 RID: 17000 RVA: 0x0011C5FA File Offset: 0x0011A7FA
		// (set) Token: 0x06004269 RID: 17001 RVA: 0x0011C602 File Offset: 0x0011A802
		public bool Handled
		{
			get
			{
				return this.handled;
			}
			set
			{
				this.handled = value;
			}
		}

		// Token: 0x04002543 RID: 9539
		private ToolStripPanel toolStripPanel;

		// Token: 0x04002544 RID: 9540
		private Graphics graphics;

		// Token: 0x04002545 RID: 9541
		private bool handled;
	}
}
