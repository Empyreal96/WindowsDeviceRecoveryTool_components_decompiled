using System;
using System.Collections.Generic;

namespace System.Windows.Shell
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Shell.JumpList.JumpItemsRejected" /> event.</summary>
	// Token: 0x02000146 RID: 326
	public sealed class JumpItemsRejectedEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Shell.JumpItemsRejectedEventArgs" /> class.</summary>
		// Token: 0x06000E52 RID: 3666 RVA: 0x00036DCB File Offset: 0x00034FCB
		public JumpItemsRejectedEventArgs() : this(null, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Shell.JumpItemsRejectedEventArgs" /> class that has the specified parameters.</summary>
		/// <param name="rejectedItems">The list of Jump List items that could not be added to the Jump List by the Windows shell.</param>
		/// <param name="reasons">The list of reasons why the rejected Jump List items could not be added to the Jump List.</param>
		/// <exception cref="T:System.ArgumentException">The count of <paramref name="rejectedItems " />does not equal the count of rejection <paramref name="reasons" />.</exception>
		// Token: 0x06000E53 RID: 3667 RVA: 0x00036DD8 File Offset: 0x00034FD8
		public JumpItemsRejectedEventArgs(IList<JumpItem> rejectedItems, IList<JumpItemRejectionReason> reasons)
		{
			if ((rejectedItems == null && reasons != null) || (reasons == null && rejectedItems != null) || (rejectedItems != null && reasons != null && rejectedItems.Count != reasons.Count))
			{
				throw new ArgumentException(SR.Get("JumpItemsRejectedEventArgs_CountMismatch"));
			}
			if (rejectedItems != null)
			{
				this.RejectedItems = new List<JumpItem>(rejectedItems).AsReadOnly();
				this.RejectionReasons = new List<JumpItemRejectionReason>(reasons).AsReadOnly();
				return;
			}
			this.RejectedItems = new List<JumpItem>().AsReadOnly();
			this.RejectionReasons = new List<JumpItemRejectionReason>().AsReadOnly();
		}

		/// <summary>Gets the list of Jump List items that could not be added to the Jump List by the Windows shell.</summary>
		/// <returns>The list of Jump List items that could not be added to the Jump List by the Windows shell.</returns>
		// Token: 0x1700045C RID: 1116
		// (get) Token: 0x06000E54 RID: 3668 RVA: 0x00036E61 File Offset: 0x00035061
		// (set) Token: 0x06000E55 RID: 3669 RVA: 0x00036E69 File Offset: 0x00035069
		public IList<JumpItem> RejectedItems { get; private set; }

		/// <summary>Gets the list of reasons why the rejected Jump List items could not be added to the Jump List.</summary>
		/// <returns>The list of reasons why the rejected Jump List items could not be added to the Jump List.</returns>
		// Token: 0x1700045D RID: 1117
		// (get) Token: 0x06000E56 RID: 3670 RVA: 0x00036E72 File Offset: 0x00035072
		// (set) Token: 0x06000E57 RID: 3671 RVA: 0x00036E7A File Offset: 0x0003507A
		public IList<JumpItemRejectionReason> RejectionReasons { get; private set; }
	}
}
