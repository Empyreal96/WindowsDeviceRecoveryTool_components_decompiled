using System;
using System.Collections;

namespace System.Windows.Forms
{
	/// <summary>Contains a collection of <see cref="T:System.Windows.Forms.GridItem" /> objects.</summary>
	// Token: 0x0200025A RID: 602
	public class GridItemCollection : ICollection, IEnumerable
	{
		// Token: 0x06002451 RID: 9297 RVA: 0x000B0AB9 File Offset: 0x000AECB9
		internal GridItemCollection(GridItem[] entries)
		{
			if (entries == null)
			{
				this.entries = new GridItem[0];
				return;
			}
			this.entries = entries;
		}

		/// <summary>Gets the number of grid items in the collection.</summary>
		/// <returns>The number of grid items in the collection.</returns>
		// Token: 0x170008B9 RID: 2233
		// (get) Token: 0x06002452 RID: 9298 RVA: 0x000B0AD8 File Offset: 0x000AECD8
		public int Count
		{
			get
			{
				return this.entries.Length;
			}
		}

		/// <summary>For a description of this member, see <see cref="P:System.Collections.ICollection.SyncRoot" />.</summary>
		/// <returns>An object that can be used to synchronize access to the <see cref="T:System.Windows.Forms.GridItemCollection" />.</returns>
		// Token: 0x170008BA RID: 2234
		// (get) Token: 0x06002453 RID: 9299 RVA: 0x000069BD File Offset: 0x00004BBD
		object ICollection.SyncRoot
		{
			get
			{
				return this;
			}
		}

		/// <summary>For a description of this member, see <see cref="P:System.Collections.ICollection.IsSynchronized" />.</summary>
		/// <returns>
		///     <see langword="false" /> in all cases.</returns>
		// Token: 0x170008BB RID: 2235
		// (get) Token: 0x06002454 RID: 9300 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Forms.GridItem" /> at the specified index.</summary>
		/// <param name="index">The index of the grid item to return. </param>
		/// <returns>The <see cref="T:System.Windows.Forms.GridItem" /> at the specified index.</returns>
		// Token: 0x170008BC RID: 2236
		public GridItem this[int index]
		{
			get
			{
				return this.entries[index];
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Forms.GridItem" /> with the matching label.</summary>
		/// <param name="label">A string value to match to a grid item label </param>
		/// <returns>The grid item whose label matches the <paramref name="label" /> parameter.</returns>
		// Token: 0x170008BD RID: 2237
		public GridItem this[string label]
		{
			get
			{
				foreach (GridItem gridItem in this.entries)
				{
					if (gridItem.Label == label)
					{
						return gridItem;
					}
				}
				return null;
			}
		}

		/// <summary>For a description of this member, see <see cref="M:System.Collections.ICollection.CopyTo(System.Array,System.Int32)" />.</summary>
		/// <param name="dest">The one-dimensional array that is the destination of the elements copied from the collection. The array must have zero-based indexing.</param>
		/// <param name="index">The zero-based index in the array at which copying begins.</param>
		// Token: 0x06002457 RID: 9303 RVA: 0x000B0B23 File Offset: 0x000AED23
		void ICollection.CopyTo(Array dest, int index)
		{
			if (this.entries.Length != 0)
			{
				Array.Copy(this.entries, 0, dest, index, this.entries.Length);
			}
		}

		/// <summary>Returns an enumeration of all the grid items in the collection.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> for the <see cref="T:System.Windows.Forms.GridItemCollection" />.</returns>
		// Token: 0x06002458 RID: 9304 RVA: 0x000B0B44 File Offset: 0x000AED44
		public IEnumerator GetEnumerator()
		{
			return this.entries.GetEnumerator();
		}

		/// <summary>Specifies that the <see cref="T:System.Windows.Forms.GridItemCollection" /> has no entries. </summary>
		// Token: 0x04000F9D RID: 3997
		public static GridItemCollection Empty = new GridItemCollection(new GridItem[0]);

		// Token: 0x04000F9E RID: 3998
		internal GridItem[] entries;
	}
}
