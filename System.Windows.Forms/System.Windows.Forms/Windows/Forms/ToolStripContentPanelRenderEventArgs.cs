using System;
using System.Drawing;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.ToolStripContentPanel.RendererChanged" /> event. </summary>
	// Token: 0x020003E1 RID: 993
	public class ToolStripContentPanelRenderEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripContentPanelRenderEventArgs" /> class. </summary>
		/// <param name="g">A <see cref="T:System.Drawing.Graphics" /> representing the GDI+ drawing surface.</param>
		/// <param name="contentPanel">The <see cref="T:System.Windows.Forms.ToolStripContentPanel" /> to render.</param>
		// Token: 0x0600426E RID: 17006 RVA: 0x0011C60B File Offset: 0x0011A80B
		public ToolStripContentPanelRenderEventArgs(Graphics g, ToolStripContentPanel contentPanel)
		{
			this.contentPanel = contentPanel;
			this.graphics = g;
		}

		/// <summary>Gets the object to use for drawing.</summary>
		/// <returns>The <see cref="T:System.Drawing.Graphics" /> to use for drawing.</returns>
		// Token: 0x170010AC RID: 4268
		// (get) Token: 0x0600426F RID: 17007 RVA: 0x0011C621 File Offset: 0x0011A821
		public Graphics Graphics
		{
			get
			{
				return this.graphics;
			}
		}

		/// <summary>Gets or sets a value indicating whether the event was handled.</summary>
		/// <returns>
		///     <see langword="true" /> if the event was handled; otherwise, <see langword="false" />. </returns>
		// Token: 0x170010AD RID: 4269
		// (get) Token: 0x06004270 RID: 17008 RVA: 0x0011C629 File Offset: 0x0011A829
		// (set) Token: 0x06004271 RID: 17009 RVA: 0x0011C631 File Offset: 0x0011A831
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

		/// <summary>Gets the <see cref="T:System.Windows.Forms.ToolStripContentPanel" /> affected by the click.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.ToolStripContentPanel" /> object affected by the click.</returns>
		// Token: 0x170010AE RID: 4270
		// (get) Token: 0x06004272 RID: 17010 RVA: 0x0011C63A File Offset: 0x0011A83A
		public ToolStripContentPanel ToolStripContentPanel
		{
			get
			{
				return this.contentPanel;
			}
		}

		// Token: 0x04002546 RID: 9542
		private ToolStripContentPanel contentPanel;

		// Token: 0x04002547 RID: 9543
		private Graphics graphics;

		// Token: 0x04002548 RID: 9544
		private bool handled;
	}
}
