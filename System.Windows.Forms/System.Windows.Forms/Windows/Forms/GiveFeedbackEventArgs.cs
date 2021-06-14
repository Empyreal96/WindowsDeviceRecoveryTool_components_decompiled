using System;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.Control.GiveFeedback" /> event, which occurs during a drag operation.</summary>
	// Token: 0x02000257 RID: 599
	[ComVisible(true)]
	public class GiveFeedbackEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.GiveFeedbackEventArgs" /> class.</summary>
		/// <param name="effect">The type of drag-and-drop operation. Possible values are obtained by applying the bitwise OR (|) operation to the constants defined in the <see cref="T:System.Windows.Forms.DragDropEffects" />. </param>
		/// <param name="useDefaultCursors">
		///       <see langword="true" /> if default pointers are used; otherwise, <see langword="false" />. </param>
		// Token: 0x0600243C RID: 9276 RVA: 0x000B0A68 File Offset: 0x000AEC68
		public GiveFeedbackEventArgs(DragDropEffects effect, bool useDefaultCursors)
		{
			this.effect = effect;
			this.useDefaultCursors = useDefaultCursors;
		}

		/// <summary>Gets the drag-and-drop operation feedback that is displayed.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.DragDropEffects" /> values.</returns>
		// Token: 0x170008AE RID: 2222
		// (get) Token: 0x0600243D RID: 9277 RVA: 0x000B0A7E File Offset: 0x000AEC7E
		public DragDropEffects Effect
		{
			get
			{
				return this.effect;
			}
		}

		/// <summary>Gets or sets whether drag operation should use the default cursors that are associated with drag-drop effects.</summary>
		/// <returns>
		///     <see langword="true" /> if the default pointers are used; otherwise, <see langword="false" />.</returns>
		// Token: 0x170008AF RID: 2223
		// (get) Token: 0x0600243E RID: 9278 RVA: 0x000B0A86 File Offset: 0x000AEC86
		// (set) Token: 0x0600243F RID: 9279 RVA: 0x000B0A8E File Offset: 0x000AEC8E
		public bool UseDefaultCursors
		{
			get
			{
				return this.useDefaultCursors;
			}
			set
			{
				this.useDefaultCursors = value;
			}
		}

		// Token: 0x04000F9A RID: 3994
		private readonly DragDropEffects effect;

		// Token: 0x04000F9B RID: 3995
		private bool useDefaultCursors;
	}
}
