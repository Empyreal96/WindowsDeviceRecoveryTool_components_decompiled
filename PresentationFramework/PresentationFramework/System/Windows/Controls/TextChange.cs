using System;

namespace System.Windows.Controls
{
	/// <summary>Contains information about the changes that occur in the <see cref="E:System.Windows.Controls.Primitives.TextBoxBase.TextChanged" /> event.</summary>
	// Token: 0x0200053E RID: 1342
	public class TextChange
	{
		// Token: 0x060057E2 RID: 22498 RVA: 0x0000326D File Offset: 0x0000146D
		internal TextChange()
		{
		}

		/// <summary>Gets or sets the position at which the change occurred.</summary>
		/// <returns>The position at which the change occurred.</returns>
		// Token: 0x17001568 RID: 5480
		// (get) Token: 0x060057E3 RID: 22499 RVA: 0x00185BFF File Offset: 0x00183DFF
		// (set) Token: 0x060057E4 RID: 22500 RVA: 0x00185C07 File Offset: 0x00183E07
		public int Offset
		{
			get
			{
				return this._offset;
			}
			internal set
			{
				this._offset = value;
			}
		}

		/// <summary>Gets or sets the number of symbols that have been added to the control.</summary>
		/// <returns>The number of symbols that have been added to the control.</returns>
		// Token: 0x17001569 RID: 5481
		// (get) Token: 0x060057E5 RID: 22501 RVA: 0x00185C10 File Offset: 0x00183E10
		// (set) Token: 0x060057E6 RID: 22502 RVA: 0x00185C18 File Offset: 0x00183E18
		public int AddedLength
		{
			get
			{
				return this._addedLength;
			}
			internal set
			{
				this._addedLength = value;
			}
		}

		/// <summary>Gets or sets the number of symbols that have been removed from the control.</summary>
		/// <returns>The number of symbols that have been removed from the control.</returns>
		// Token: 0x1700156A RID: 5482
		// (get) Token: 0x060057E7 RID: 22503 RVA: 0x00185C21 File Offset: 0x00183E21
		// (set) Token: 0x060057E8 RID: 22504 RVA: 0x00185C29 File Offset: 0x00183E29
		public int RemovedLength
		{
			get
			{
				return this._removedLength;
			}
			internal set
			{
				this._removedLength = value;
			}
		}

		// Token: 0x04002E90 RID: 11920
		private int _offset;

		// Token: 0x04002E91 RID: 11921
		private int _addedLength;

		// Token: 0x04002E92 RID: 11922
		private int _removedLength;
	}
}
