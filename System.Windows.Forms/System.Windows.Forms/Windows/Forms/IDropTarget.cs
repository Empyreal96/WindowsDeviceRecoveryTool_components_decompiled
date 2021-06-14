using System;

namespace System.Windows.Forms
{
	/// <summary>Defines mouse events.</summary>
	// Token: 0x02000293 RID: 659
	public interface IDropTarget
	{
		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.DragEnter" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.DragEventArgs" /> that contains the event data.</param>
		// Token: 0x060026DC RID: 9948
		void OnDragEnter(DragEventArgs e);

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.DragLeave" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x060026DD RID: 9949
		void OnDragLeave(EventArgs e);

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.DragDrop" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.DragEventArgs" /> that contains the event data.</param>
		// Token: 0x060026DE RID: 9950
		void OnDragDrop(DragEventArgs e);

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.DragOver" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.DragEventArgs" /> that contains the event data.</param>
		// Token: 0x060026DF RID: 9951
		void OnDragOver(DragEventArgs e);
	}
}
