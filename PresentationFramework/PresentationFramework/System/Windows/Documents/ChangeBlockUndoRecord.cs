using System;
using MS.Internal;
using MS.Internal.Documents;

namespace System.Windows.Documents
{
	// Token: 0x02000334 RID: 820
	internal class ChangeBlockUndoRecord
	{
		// Token: 0x06002B49 RID: 11081 RVA: 0x000C58B8 File Offset: 0x000C3AB8
		internal ChangeBlockUndoRecord(ITextContainer textContainer, string actionDescription)
		{
			if (textContainer.UndoManager != null)
			{
				this._undoManager = textContainer.UndoManager;
				if (this._undoManager.IsEnabled)
				{
					if (textContainer.TextView != null)
					{
						if (this._undoManager.OpenedUnit == null)
						{
							if (textContainer.TextSelection != null)
							{
								this._parentUndoUnit = new TextParentUndoUnit(textContainer.TextSelection);
							}
							else
							{
								this._parentUndoUnit = new ParentUndoUnit(actionDescription);
							}
							this._undoManager.Open(this._parentUndoUnit);
							return;
						}
					}
					else
					{
						this._undoManager.Clear();
					}
				}
			}
		}

		// Token: 0x06002B4A RID: 11082 RVA: 0x000C5948 File Offset: 0x000C3B48
		internal void OnEndChange()
		{
			if (this._parentUndoUnit != null)
			{
				IParentUndoUnit openedUnit;
				if (this._parentUndoUnit.Container is UndoManager)
				{
					openedUnit = ((UndoManager)this._parentUndoUnit.Container).OpenedUnit;
				}
				else
				{
					openedUnit = ((IParentUndoUnit)this._parentUndoUnit.Container).OpenedUnit;
				}
				if (openedUnit == this._parentUndoUnit)
				{
					if (this._parentUndoUnit is TextParentUndoUnit)
					{
						((TextParentUndoUnit)this._parentUndoUnit).RecordRedoSelectionState();
					}
					Invariant.Assert(this._undoManager != null);
					this._undoManager.Close(this._parentUndoUnit, (this._parentUndoUnit.LastUnit != null) ? UndoCloseAction.Commit : UndoCloseAction.Discard);
				}
			}
		}

		// Token: 0x04001C8A RID: 7306
		private readonly UndoManager _undoManager;

		// Token: 0x04001C8B RID: 7307
		private readonly IParentUndoUnit _parentUndoUnit;
	}
}
