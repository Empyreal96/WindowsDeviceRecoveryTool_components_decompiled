using System;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.Control.QueryContinueDrag" /> event.</summary>
	// Token: 0x02000328 RID: 808
	[ComVisible(true)]
	public class QueryContinueDragEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.QueryContinueDragEventArgs" /> class.</summary>
		/// <param name="keyState">The current state of the SHIFT, CTRL, and ALT keys. </param>
		/// <param name="escapePressed">
		///       <see langword="true" /> if the ESC key was pressed; otherwise, <see langword="false" />. </param>
		/// <param name="action">A <see cref="T:System.Windows.Forms.DragAction" /> value. </param>
		// Token: 0x06003210 RID: 12816 RVA: 0x000E9F67 File Offset: 0x000E8167
		public QueryContinueDragEventArgs(int keyState, bool escapePressed, DragAction action)
		{
			this.keyState = keyState;
			this.escapePressed = escapePressed;
			this.action = action;
		}

		/// <summary>Gets the current state of the SHIFT, CTRL, and ALT keys.</summary>
		/// <returns>The current state of the SHIFT, CTRL, and ALT keys.</returns>
		// Token: 0x17000C69 RID: 3177
		// (get) Token: 0x06003211 RID: 12817 RVA: 0x000E9F84 File Offset: 0x000E8184
		public int KeyState
		{
			get
			{
				return this.keyState;
			}
		}

		/// <summary>Gets whether the user pressed the ESC key.</summary>
		/// <returns>
		///     <see langword="true" /> if the ESC key was pressed; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000C6A RID: 3178
		// (get) Token: 0x06003212 RID: 12818 RVA: 0x000E9F8C File Offset: 0x000E818C
		public bool EscapePressed
		{
			get
			{
				return this.escapePressed;
			}
		}

		/// <summary>Gets or sets the status of a drag-and-drop operation.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.DragAction" /> value.</returns>
		// Token: 0x17000C6B RID: 3179
		// (get) Token: 0x06003213 RID: 12819 RVA: 0x000E9F94 File Offset: 0x000E8194
		// (set) Token: 0x06003214 RID: 12820 RVA: 0x000E9F9C File Offset: 0x000E819C
		public DragAction Action
		{
			get
			{
				return this.action;
			}
			set
			{
				this.action = value;
			}
		}

		// Token: 0x04001E32 RID: 7730
		private readonly int keyState;

		// Token: 0x04001E33 RID: 7731
		private readonly bool escapePressed;

		// Token: 0x04001E34 RID: 7732
		private DragAction action;
	}
}
