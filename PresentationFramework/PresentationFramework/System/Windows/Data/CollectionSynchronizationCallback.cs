using System;
using System.Collections;

namespace System.Windows.Data
{
	/// <summary>Represent the method that synchronizes a collection for cross-thread access.</summary>
	/// <param name="collection">The collection to access on a thread other than the one that created it.</param>
	/// <param name="context">An object used to synchronize the collection.</param>
	/// <param name="accessMethod">A delegate to the method that performs the operation on the collection.</param>
	/// <param name="writeAccess">
	///       <see langword="true" /> if <paramref name="accessMethod" /> writes to the collection; otherwise, <see langword="false" />.</param>
	// Token: 0x020001A6 RID: 422
	// (Invoke) Token: 0x06001A68 RID: 6760
	public delegate void CollectionSynchronizationCallback(IEnumerable collection, object context, Action accessMethod, bool writeAccess);
}
