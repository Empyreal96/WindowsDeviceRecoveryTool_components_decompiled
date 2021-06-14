using System;

namespace System.Windows.Forms
{
	/// <summary>Allows a custom control to prevent the <see cref="E:System.Windows.Forms.Control.MouseWheel" /> event from being sent to its parent container.</summary>
	// Token: 0x0200025F RID: 607
	public class HandledMouseEventArgs : MouseEventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.HandledMouseEventArgs" /> class with the specified mouse button, number of mouse button clicks, horizontal and vertical screen coordinates, and the change of mouse pointer position.</summary>
		/// <param name="button">One of the <see cref="T:System.Windows.Forms.MouseButtons" /> values indicating which mouse button was pressed. </param>
		/// <param name="clicks">The number of times a mouse button was pressed. </param>
		/// <param name="x">The x-coordinate of a mouse click, in pixels. </param>
		/// <param name="y">The y-coordinate of a mouse click, in pixels. </param>
		/// <param name="delta">A signed count of the number of detents the wheel has rotated. </param>
		// Token: 0x060024A8 RID: 9384 RVA: 0x000B1E7E File Offset: 0x000B007E
		public HandledMouseEventArgs(MouseButtons button, int clicks, int x, int y, int delta) : this(button, clicks, x, y, delta, false)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.HandledMouseEventArgs" /> class with the specified mouse button, number of mouse button clicks, horizontal and vertical screen coordinates, the change of mouse pointer position, and the value indicating whether the event is handled.</summary>
		/// <param name="button">One of the <see cref="T:System.Windows.Forms.MouseButtons" /> values indicating which mouse button was pressed. </param>
		/// <param name="clicks">The number of times a mouse button was pressed. </param>
		/// <param name="x">The x-coordinate of a mouse click, in pixels. </param>
		/// <param name="y">The y-coordinate of a mouse click, in pixels. </param>
		/// <param name="delta">A signed count of the number of detents the wheel has rotated. </param>
		/// <param name="defaultHandledValue">
		///       <see langword="true" /> if the event is handled; otherwise, <see langword="false" />. </param>
		// Token: 0x060024A9 RID: 9385 RVA: 0x000B1E8E File Offset: 0x000B008E
		public HandledMouseEventArgs(MouseButtons button, int clicks, int x, int y, int delta, bool defaultHandledValue) : base(button, clicks, x, y, delta)
		{
			this.handled = defaultHandledValue;
		}

		/// <summary>Gets or sets whether this event should be forwarded to the control's parent container.</summary>
		/// <returns>
		///     <see langword="true" /> if the mouse event should go to the parent control; otherwise, <see langword="false" />.</returns>
		// Token: 0x170008CE RID: 2254
		// (get) Token: 0x060024AA RID: 9386 RVA: 0x000B1EA5 File Offset: 0x000B00A5
		// (set) Token: 0x060024AB RID: 9387 RVA: 0x000B1EAD File Offset: 0x000B00AD
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

		// Token: 0x04000FB1 RID: 4017
		private bool handled;
	}
}
