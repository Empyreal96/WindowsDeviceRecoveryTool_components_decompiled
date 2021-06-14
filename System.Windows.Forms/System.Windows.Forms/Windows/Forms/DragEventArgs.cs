using System;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.Control.DragDrop" />, <see cref="E:System.Windows.Forms.Control.DragEnter" />, or <see cref="E:System.Windows.Forms.Control.DragOver" /> event.</summary>
	// Token: 0x02000227 RID: 551
	[ComVisible(true)]
	public class DragEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DragEventArgs" /> class.</summary>
		/// <param name="data">The data associated with this event. </param>
		/// <param name="keyState">The current state of the SHIFT, CTRL, and ALT keys. </param>
		/// <param name="x">The x-coordinate of the mouse cursor in pixels. </param>
		/// <param name="y">The y-coordinate of the mouse cursor in pixels. </param>
		/// <param name="allowedEffect">One of the <see cref="T:System.Windows.Forms.DragDropEffects" /> values. </param>
		/// <param name="effect">One of the <see cref="T:System.Windows.Forms.DragDropEffects" /> values. </param>
		// Token: 0x0600214C RID: 8524 RVA: 0x000A4660 File Offset: 0x000A2860
		public DragEventArgs(IDataObject data, int keyState, int x, int y, DragDropEffects allowedEffect, DragDropEffects effect)
		{
			this.data = data;
			this.keyState = keyState;
			this.x = x;
			this.y = y;
			this.allowedEffect = allowedEffect;
			this.effect = effect;
		}

		/// <summary>Gets the <see cref="T:System.Windows.Forms.IDataObject" /> that contains the data associated with this event.</summary>
		/// <returns>The data associated with this event.</returns>
		// Token: 0x170007E2 RID: 2018
		// (get) Token: 0x0600214D RID: 8525 RVA: 0x000A4695 File Offset: 0x000A2895
		public IDataObject Data
		{
			get
			{
				return this.data;
			}
		}

		/// <summary>Gets the current state of the SHIFT, CTRL, and ALT keys, as well as the state of the mouse buttons.</summary>
		/// <returns>The current state of the SHIFT, CTRL, and ALT keys and of the mouse buttons.</returns>
		// Token: 0x170007E3 RID: 2019
		// (get) Token: 0x0600214E RID: 8526 RVA: 0x000A469D File Offset: 0x000A289D
		public int KeyState
		{
			get
			{
				return this.keyState;
			}
		}

		/// <summary>Gets the x-coordinate of the mouse pointer, in screen coordinates.</summary>
		/// <returns>The x-coordinate of the mouse pointer in pixels.</returns>
		// Token: 0x170007E4 RID: 2020
		// (get) Token: 0x0600214F RID: 8527 RVA: 0x000A46A5 File Offset: 0x000A28A5
		public int X
		{
			get
			{
				return this.x;
			}
		}

		/// <summary>Gets the y-coordinate of the mouse pointer, in screen coordinates.</summary>
		/// <returns>The y-coordinate of the mouse pointer in pixels.</returns>
		// Token: 0x170007E5 RID: 2021
		// (get) Token: 0x06002150 RID: 8528 RVA: 0x000A46AD File Offset: 0x000A28AD
		public int Y
		{
			get
			{
				return this.y;
			}
		}

		/// <summary>Gets which drag-and-drop operations are allowed by the originator (or source) of the drag event.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.DragDropEffects" /> values.</returns>
		// Token: 0x170007E6 RID: 2022
		// (get) Token: 0x06002151 RID: 8529 RVA: 0x000A46B5 File Offset: 0x000A28B5
		public DragDropEffects AllowedEffect
		{
			get
			{
				return this.allowedEffect;
			}
		}

		/// <summary>Gets or sets the target drop effect in a drag-and-drop operation.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.DragDropEffects" /> values.</returns>
		// Token: 0x170007E7 RID: 2023
		// (get) Token: 0x06002152 RID: 8530 RVA: 0x000A46BD File Offset: 0x000A28BD
		// (set) Token: 0x06002153 RID: 8531 RVA: 0x000A46C5 File Offset: 0x000A28C5
		public DragDropEffects Effect
		{
			get
			{
				return this.effect;
			}
			set
			{
				this.effect = value;
			}
		}

		// Token: 0x04000E66 RID: 3686
		private readonly IDataObject data;

		// Token: 0x04000E67 RID: 3687
		private readonly int keyState;

		// Token: 0x04000E68 RID: 3688
		private readonly int x;

		// Token: 0x04000E69 RID: 3689
		private readonly int y;

		// Token: 0x04000E6A RID: 3690
		private readonly DragDropEffects allowedEffect;

		// Token: 0x04000E6B RID: 3691
		private DragDropEffects effect;
	}
}
