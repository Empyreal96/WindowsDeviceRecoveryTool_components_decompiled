using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.Control.HelpRequested" /> event.</summary>
	// Token: 0x02000261 RID: 609
	[ComVisible(true)]
	public class HelpEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.HelpEventArgs" /> class.</summary>
		/// <param name="mousePos">The coordinates of the mouse pointer. </param>
		// Token: 0x060024B8 RID: 9400 RVA: 0x000B24D4 File Offset: 0x000B06D4
		public HelpEventArgs(Point mousePos)
		{
			this.mousePos = mousePos;
		}

		/// <summary>Gets the screen coordinates of the mouse pointer.</summary>
		/// <returns>A <see cref="T:System.Drawing.Point" /> representing the screen coordinates of the mouse pointer.</returns>
		// Token: 0x170008CF RID: 2255
		// (get) Token: 0x060024B9 RID: 9401 RVA: 0x000B24E3 File Offset: 0x000B06E3
		public Point MousePos
		{
			get
			{
				return this.mousePos;
			}
		}

		/// <summary>Gets or sets a value indicating whether the help event was handled.</summary>
		/// <returns>
		///     <see langword="true" /> if the event is handled; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x170008D0 RID: 2256
		// (get) Token: 0x060024BA RID: 9402 RVA: 0x000B24EB File Offset: 0x000B06EB
		// (set) Token: 0x060024BB RID: 9403 RVA: 0x000B24F3 File Offset: 0x000B06F3
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

		// Token: 0x04000FD1 RID: 4049
		private readonly Point mousePos;

		// Token: 0x04000FD2 RID: 4050
		private bool handled;
	}
}
