using System;
using System.Collections;
using System.ComponentModel;

namespace System.Windows.Forms
{
	/// <summary>Provides the base functionality for creating data-related collections in the <see cref="N:System.Windows.Forms" /> namespace.</summary>
	// Token: 0x0200011E RID: 286
	public class BaseCollection : MarshalByRefObject, ICollection, IEnumerable
	{
		/// <summary>Gets the total number of elements in the collection.</summary>
		/// <returns>The total number of elements in the collection.</returns>
		// Token: 0x17000233 RID: 563
		// (get) Token: 0x06000762 RID: 1890 RVA: 0x000163E6 File Offset: 0x000145E6
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public virtual int Count
		{
			get
			{
				return this.List.Count;
			}
		}

		/// <summary>Copies all the elements of the current one-dimensional <see cref="T:System.Array" /> to the specified one-dimensional <see cref="T:System.Array" /> starting at the specified destination <see cref="T:System.Array" /> index.</summary>
		/// <param name="ar">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from the current <see langword="Array" />. </param>
		/// <param name="index">The zero-based relative index in <paramref name="ar" /> at which copying begins. </param>
		// Token: 0x06000763 RID: 1891 RVA: 0x000163F3 File Offset: 0x000145F3
		public void CopyTo(Array ar, int index)
		{
			this.List.CopyTo(ar, index);
		}

		/// <summary>Gets the object that enables iterating through the members of the collection.</summary>
		/// <returns>An object that implements the <see cref="T:System.Collections.IEnumerator" /> interface.</returns>
		// Token: 0x06000764 RID: 1892 RVA: 0x00016402 File Offset: 0x00014602
		public IEnumerator GetEnumerator()
		{
			return this.List.GetEnumerator();
		}

		/// <summary>Gets a value indicating whether the collection is read-only.</summary>
		/// <returns>This property is always <see langword="false" />.</returns>
		// Token: 0x17000234 RID: 564
		// (get) Token: 0x06000765 RID: 1893 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection" /> is synchronized.</summary>
		/// <returns>This property always returns <see langword="false" />.</returns>
		// Token: 0x17000235 RID: 565
		// (get) Token: 0x06000766 RID: 1894 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets an object that can be used to synchronize access to the <see cref="T:System.Windows.Forms.BaseCollection" />.</summary>
		/// <returns>An object that can be used to synchronize the <see cref="T:System.Windows.Forms.BaseCollection" />.</returns>
		// Token: 0x17000236 RID: 566
		// (get) Token: 0x06000767 RID: 1895 RVA: 0x000069BD File Offset: 0x00004BBD
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		/// <summary>Gets the list of elements contained in the <see cref="T:System.Windows.Forms.BaseCollection" /> instance.</summary>
		/// <returns>An <see cref="T:System.Collections.ArrayList" /> containing the elements of the collection. This property returns <see langword="null" /> unless overridden in a derived class.</returns>
		// Token: 0x17000237 RID: 567
		// (get) Token: 0x06000768 RID: 1896 RVA: 0x0000DE5C File Offset: 0x0000C05C
		protected virtual ArrayList List
		{
			get
			{
				return null;
			}
		}
	}
}
