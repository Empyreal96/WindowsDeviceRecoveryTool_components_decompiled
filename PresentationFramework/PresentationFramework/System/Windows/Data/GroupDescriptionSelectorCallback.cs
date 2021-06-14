using System;
using System.ComponentModel;

namespace System.Windows.Data
{
	/// <summary>Represents a method that is used to provide custom logic to select the <see cref="T:System.ComponentModel.GroupDescription" /> based on the parent group and its level.</summary>
	/// <param name="group">The parent group.</param>
	/// <param name="level">The level of <paramref name="group" />.</param>
	/// <returns>The <see cref="T:System.ComponentModel.GroupDescription" /> chosen based on the parent group and its level. </returns>
	// Token: 0x020001B3 RID: 435
	// (Invoke) Token: 0x06001BEB RID: 7147
	public delegate GroupDescription GroupDescriptionSelectorCallback(CollectionViewGroup group, int level);
}
