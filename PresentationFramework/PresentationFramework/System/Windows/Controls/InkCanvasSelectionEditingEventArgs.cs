using System;
using System.ComponentModel;

namespace System.Windows.Controls
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Controls.InkCanvas.SelectionMoving" /> and <see cref="E:System.Windows.Controls.InkCanvas.SelectionResizing" /> events. </summary>
	// Token: 0x0200056B RID: 1387
	public class InkCanvasSelectionEditingEventArgs : CancelEventArgs
	{
		// Token: 0x06005B7E RID: 23422 RVA: 0x0019C5A8 File Offset: 0x0019A7A8
		internal InkCanvasSelectionEditingEventArgs(Rect oldRectangle, Rect newRectangle)
		{
			this._oldRectangle = oldRectangle;
			this._newRectangle = newRectangle;
		}

		/// <summary>Gets the bounds of the selection before the user moved or resized it.</summary>
		/// <returns>The bounds of the selection before the user moved or resized it.</returns>
		// Token: 0x17001628 RID: 5672
		// (get) Token: 0x06005B7F RID: 23423 RVA: 0x0019C5BE File Offset: 0x0019A7BE
		public Rect OldRectangle
		{
			get
			{
				return this._oldRectangle;
			}
		}

		/// <summary>Gets or sets the bounds of the selection after it is moved or resized.</summary>
		/// <returns>The bounds of the selection after it is moved or resized.</returns>
		// Token: 0x17001629 RID: 5673
		// (get) Token: 0x06005B80 RID: 23424 RVA: 0x0019C5C6 File Offset: 0x0019A7C6
		// (set) Token: 0x06005B81 RID: 23425 RVA: 0x0019C5CE File Offset: 0x0019A7CE
		public Rect NewRectangle
		{
			get
			{
				return this._newRectangle;
			}
			set
			{
				this._newRectangle = value;
			}
		}

		// Token: 0x04002F87 RID: 12167
		private Rect _oldRectangle;

		// Token: 0x04002F88 RID: 12168
		private Rect _newRectangle;
	}
}
