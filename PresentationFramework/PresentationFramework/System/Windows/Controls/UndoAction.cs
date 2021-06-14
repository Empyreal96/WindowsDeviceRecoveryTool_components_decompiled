using System;

namespace System.Windows.Controls
{
	/// <summary>
	///
	///     How the undo stack caused or is affected by a text change. </summary>
	// Token: 0x0200053F RID: 1343
	public enum UndoAction
	{
		/// <summary>
		///
		///     This change will not affect the undo stack at all </summary>
		// Token: 0x04002E94 RID: 11924
		None,
		/// <summary>
		///
		///     This change will merge into the previous undo unit </summary>
		// Token: 0x04002E95 RID: 11925
		Merge,
		/// <summary>
		///
		///     This change is the result of a call to Undo() </summary>
		// Token: 0x04002E96 RID: 11926
		Undo,
		/// <summary>
		///
		///     This change is the result of a call to Redo() </summary>
		// Token: 0x04002E97 RID: 11927
		Redo,
		/// <summary>
		///
		///     This change will clear the undo stack </summary>
		// Token: 0x04002E98 RID: 11928
		Clear,
		/// <summary>
		///
		///     This change will create a new undo unit </summary>
		// Token: 0x04002E99 RID: 11929
		Create
	}
}
