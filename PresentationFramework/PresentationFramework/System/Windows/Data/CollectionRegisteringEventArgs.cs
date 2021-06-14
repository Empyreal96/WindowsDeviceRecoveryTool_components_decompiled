using System;
using System.Collections;

namespace System.Windows.Data
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Data.BindingOperations.CollectionRegistering" /> event.</summary>
	// Token: 0x020001A5 RID: 421
	public class CollectionRegisteringEventArgs : EventArgs
	{
		// Token: 0x06001A64 RID: 6756 RVA: 0x0007DAA7 File Offset: 0x0007BCA7
		internal CollectionRegisteringEventArgs(IEnumerable collection, object parent = null)
		{
			this._collection = collection;
			this._parent = parent;
		}

		/// <summary>Gets the collection to be registered for cross-thread access.</summary>
		/// <returns>The collection to be registered for cross-thread access.</returns>
		// Token: 0x17000615 RID: 1557
		// (get) Token: 0x06001A65 RID: 6757 RVA: 0x0007DABD File Offset: 0x0007BCBD
		public IEnumerable Collection
		{
			get
			{
				return this._collection;
			}
		}

		/// <summary>Gets the parent of the collection to register.</summary>
		/// <returns>The parent of the collection to register.</returns>
		// Token: 0x17000616 RID: 1558
		// (get) Token: 0x06001A66 RID: 6758 RVA: 0x0007DAC5 File Offset: 0x0007BCC5
		public object Parent
		{
			get
			{
				return this._parent;
			}
		}

		// Token: 0x04001351 RID: 4945
		private IEnumerable _collection;

		// Token: 0x04001352 RID: 4946
		private object _parent;
	}
}
