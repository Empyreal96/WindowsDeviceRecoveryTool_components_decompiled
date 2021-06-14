using System;
using System.Windows.Data;

namespace System.Windows.Controls
{
	/// <summary>Delegate used to select the group style as a function of the parent group and its level.</summary>
	/// <param name="group">Group whose style is to be selected.</param>
	/// <param name="level">Level of the group.</param>
	/// <returns>The appropriate group style.</returns>
	// Token: 0x020004E2 RID: 1250
	// (Invoke) Token: 0x06004E02 RID: 19970
	public delegate GroupStyle GroupStyleSelector(CollectionViewGroup group, int level);
}
