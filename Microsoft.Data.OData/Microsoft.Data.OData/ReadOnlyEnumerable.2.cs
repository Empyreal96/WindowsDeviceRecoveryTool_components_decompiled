using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Microsoft.Data.OData
{
	// Token: 0x02000243 RID: 579
	internal sealed class ReadOnlyEnumerable<T> : ReadOnlyEnumerable, IEnumerable<!0>, IEnumerable
	{
		// Token: 0x0600128E RID: 4750 RVA: 0x00045B6D File Offset: 0x00043D6D
		internal ReadOnlyEnumerable() : this(new List<T>())
		{
		}

		// Token: 0x0600128F RID: 4751 RVA: 0x00045B7A File Offset: 0x00043D7A
		internal ReadOnlyEnumerable(IList<T> sourceList) : base(sourceList)
		{
			this.sourceList = sourceList;
		}

		// Token: 0x06001290 RID: 4752 RVA: 0x00045B8A File Offset: 0x00043D8A
		IEnumerator<T> IEnumerable<!0>.GetEnumerator()
		{
			return this.sourceList.GetEnumerator();
		}

		// Token: 0x06001291 RID: 4753 RVA: 0x00045B97 File Offset: 0x00043D97
		internal static ReadOnlyEnumerable<T> Empty()
		{
			return ReadOnlyEnumerable<T>.EmptyInstance.Value;
		}

		// Token: 0x06001292 RID: 4754 RVA: 0x00045BA3 File Offset: 0x00043DA3
		internal void AddToSourceList(T itemToAdd)
		{
			this.sourceList.Add(itemToAdd);
		}

		// Token: 0x040006AB RID: 1707
		private readonly IList<T> sourceList;

		// Token: 0x040006AC RID: 1708
		private static readonly SimpleLazy<ReadOnlyEnumerable<T>> EmptyInstance = new SimpleLazy<ReadOnlyEnumerable<T>>(() => new ReadOnlyEnumerable<T>(new ReadOnlyCollection<T>(new List<T>(0))), true);
	}
}
