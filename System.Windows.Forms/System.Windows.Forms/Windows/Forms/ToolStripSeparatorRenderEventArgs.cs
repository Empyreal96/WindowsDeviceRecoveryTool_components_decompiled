using System;
using System.Drawing;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.ToolStripRenderer.RenderGrip" /> event. </summary>
	// Token: 0x020003EF RID: 1007
	public class ToolStripSeparatorRenderEventArgs : ToolStripItemRenderEventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripSeparatorRenderEventArgs" /> class. </summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> to paint with.</param>
		/// <param name="separator">The <see cref="T:System.Windows.Forms.ToolStripSeparator" /> to be painted.</param>
		/// <param name="vertical">A value indicating whether or not the <see cref="T:System.Windows.Forms.ToolStripSeparator" /> is to be drawn vertically.</param>
		// Token: 0x060043E0 RID: 17376 RVA: 0x001221D9 File Offset: 0x001203D9
		public ToolStripSeparatorRenderEventArgs(Graphics g, ToolStripSeparator separator, bool vertical) : base(g, separator)
		{
			this.vertical = vertical;
		}

		/// <summary>Gets a value indicating whether the display style for the grip is vertical. </summary>
		/// <returns>
		///     <see langword="true" /> if the display style for the <see cref="T:System.Windows.Forms.ToolStripSeparator" /> is vertical; otherwise, <see langword="false" />.</returns>
		// Token: 0x17001104 RID: 4356
		// (get) Token: 0x060043E1 RID: 17377 RVA: 0x001221EA File Offset: 0x001203EA
		public bool Vertical
		{
			get
			{
				return this.vertical;
			}
		}

		// Token: 0x040025B0 RID: 9648
		private bool vertical;
	}
}
