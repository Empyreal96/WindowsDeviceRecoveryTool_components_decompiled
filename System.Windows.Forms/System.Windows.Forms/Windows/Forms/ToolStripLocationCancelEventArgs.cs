using System;
using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms
{
	// Token: 0x020003FA RID: 1018
	internal class ToolStripLocationCancelEventArgs : CancelEventArgs
	{
		// Token: 0x060044EE RID: 17646 RVA: 0x00125C7C File Offset: 0x00123E7C
		public ToolStripLocationCancelEventArgs(Point newLocation, bool value) : base(value)
		{
			this.newLocation = newLocation;
		}

		// Token: 0x17001146 RID: 4422
		// (get) Token: 0x060044EF RID: 17647 RVA: 0x00125C8C File Offset: 0x00123E8C
		public Point NewLocation
		{
			get
			{
				return this.newLocation;
			}
		}

		// Token: 0x040025EB RID: 9707
		private Point newLocation;
	}
}
