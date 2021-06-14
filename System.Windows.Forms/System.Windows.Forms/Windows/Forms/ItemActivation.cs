using System;

namespace System.Windows.Forms
{
	/// <summary>Specifies the user action that is required to activate items in a list view control and the feedback that is given as the user moves the mouse pointer over an item.</summary>
	// Token: 0x02000295 RID: 661
	public enum ItemActivation
	{
		/// <summary>The user must double-click to activate items. No feedback is given as the user moves the mouse pointer over an item.</summary>
		// Token: 0x04001069 RID: 4201
		Standard,
		/// <summary>The user must single-click to activate items. The cursor changes to a hand pointer cursor, and the item text changes color as the user moves the mouse pointer over the item.</summary>
		// Token: 0x0400106A RID: 4202
		OneClick,
		/// <summary>The user must click an item twice to activate it. This is different from the standard double-click because the two clicks can have any duration between them. The item text changes color as the user moves the mouse pointer over the item.</summary>
		// Token: 0x0400106B RID: 4203
		TwoClick
	}
}
