using System;
using System.Collections.Generic;

namespace System.Windows.Shell
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Shell.JumpList.JumpItemsRemovedByUser" /> event.</summary>
	// Token: 0x02000147 RID: 327
	public sealed class JumpItemsRemovedEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Shell.JumpItemsRemovedEventArgs" /> class.</summary>
		// Token: 0x06000E58 RID: 3672 RVA: 0x00036E83 File Offset: 0x00035083
		public JumpItemsRemovedEventArgs() : this(null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Shell.JumpItemsRemovedEventArgs" /> class that has the specified parameters.</summary>
		/// <param name="removedItems">The list of Jump List items that have been removed by the user since <see cref="M:System.Windows.Shell.JumpList.Apply" /> was last called.</param>
		// Token: 0x06000E59 RID: 3673 RVA: 0x00036E8C File Offset: 0x0003508C
		public JumpItemsRemovedEventArgs(IList<JumpItem> removedItems)
		{
			if (removedItems != null)
			{
				this.RemovedItems = new List<JumpItem>(removedItems).AsReadOnly();
				return;
			}
			this.RemovedItems = new List<JumpItem>().AsReadOnly();
		}

		/// <summary>Gets the list of Jump List items that have been removed by the user since the <see cref="M:System.Windows.Shell.JumpList.Apply" /> method was last called.</summary>
		/// <returns>The list of Jump List items that have been removed by the user since the <see cref="M:System.Windows.Shell.JumpList.Apply" /> method was last called.</returns>
		// Token: 0x1700045E RID: 1118
		// (get) Token: 0x06000E5A RID: 3674 RVA: 0x00036EB9 File Offset: 0x000350B9
		// (set) Token: 0x06000E5B RID: 3675 RVA: 0x00036EC1 File Offset: 0x000350C1
		public IList<JumpItem> RemovedItems { get; private set; }
	}
}
