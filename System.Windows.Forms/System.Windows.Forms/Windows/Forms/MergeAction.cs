using System;

namespace System.Windows.Forms
{
	/// <summary>Specifies the kind of action to take if a match is found when combining menu items on a <see cref="T:System.Windows.Forms.ToolStrip" />.</summary>
	// Token: 0x020002E7 RID: 743
	public enum MergeAction
	{
		/// <summary>Appends the item to the end of the collection, ignoring match results.</summary>
		// Token: 0x04001322 RID: 4898
		Append,
		/// <summary>Inserts the item to the target's collection immediately preceding the matched item. A match of the end of the list results in the item being appended to the list. If there is no match or the match is at the beginning of the list, the item is inserted at the beginning of the collection.</summary>
		// Token: 0x04001323 RID: 4899
		Insert,
		/// <summary>Replaces the matched item with the source item. The original item's drop-down items do not become children of the incoming item.</summary>
		// Token: 0x04001324 RID: 4900
		Replace,
		/// <summary>Removes the matched item.</summary>
		// Token: 0x04001325 RID: 4901
		Remove,
		/// <summary>A match is required, but no action is taken. Use this for tree creation and successful access to nested layouts.</summary>
		// Token: 0x04001326 RID: 4902
		MatchOnly
	}
}
