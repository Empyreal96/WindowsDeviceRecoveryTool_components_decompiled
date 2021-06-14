using System;
using System.Collections;

namespace System.Windows.Documents
{
	/// <summary>Provides a collection of all of the <see cref="T:System.Windows.Documents.LinkTarget" /> elements in a <see cref="T:System.IO.Packaging.Package" />.</summary>
	// Token: 0x02000393 RID: 915
	public sealed class LinkTargetCollection : CollectionBase
	{
		/// <summary>Gets or sets the <see cref="T:System.Windows.Documents.LinkTarget" /> at the specified index.</summary>
		/// <param name="index">The index of the target being written or read.</param>
		/// <returns>The <see cref="T:System.Windows.Documents.LinkTarget" /> at the specified index.</returns>
		// Token: 0x17000C7D RID: 3197
		public LinkTarget this[int index]
		{
			get
			{
				return (LinkTarget)((IList)this)[index];
			}
			set
			{
				((IList)this)[index] = value;
			}
		}

		/// <summary>Adds a specified <see cref="T:System.Windows.Documents.LinkTarget" /> to the collection.</summary>
		/// <param name="value">The link target that is added.</param>
		/// <returns>The zero-based index in the collection of the <paramref name="value" /> added.</returns>
		// Token: 0x060031A9 RID: 12713 RVA: 0x000DBAB9 File Offset: 0x000D9CB9
		public int Add(LinkTarget value)
		{
			return ((IList)this).Add(value);
		}

		/// <summary>Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1" />.</summary>
		/// <param name="value">The link target to remove.</param>
		// Token: 0x060031AA RID: 12714 RVA: 0x000DBAC2 File Offset: 0x000D9CC2
		public void Remove(LinkTarget value)
		{
			((IList)this).Remove(value);
		}

		/// <summary>Specifies a value that indicates whether a particular <see cref="T:System.Windows.Documents.LinkTarget" /> is in the collection.</summary>
		/// <param name="value">The link to test for.</param>
		/// <returns>
		///     <see langword="true" /> if <paramref name="value" /> is present; otherwise, <see langword="false" />.</returns>
		// Token: 0x060031AB RID: 12715 RVA: 0x000DBACB File Offset: 0x000D9CCB
		public bool Contains(LinkTarget value)
		{
			return ((IList)this).Contains(value);
		}

		/// <summary>Copies the items in the collection to the specified array beginning at the specified index.</summary>
		/// <param name="array">The target array.</param>
		/// <param name="index">The zero-based index of the array position where the first item is copied. </param>
		// Token: 0x060031AC RID: 12716 RVA: 0x000DBAD4 File Offset: 0x000D9CD4
		public void CopyTo(LinkTarget[] array, int index)
		{
			((ICollection)this).CopyTo(array, index);
		}

		/// <summary>Gets the index of the specified item.</summary>
		/// <param name="value">The object to locate in the collection.</param>
		/// <returns>The index of <paramref name="value" /> if found in the collection; otherwise, -1.</returns>
		// Token: 0x060031AD RID: 12717 RVA: 0x000DBADE File Offset: 0x000D9CDE
		public int IndexOf(LinkTarget value)
		{
			return ((IList)this).IndexOf(value);
		}

		/// <summary>Inserts the specified item into the collection at the specified index.</summary>
		/// <param name="index">The point where the link target is inserted.</param>
		/// <param name="value">The target to insert.</param>
		// Token: 0x060031AE RID: 12718 RVA: 0x000DBAE7 File Offset: 0x000D9CE7
		public void Insert(int index, LinkTarget value)
		{
			((IList)this).Insert(index, value);
		}
	}
}
