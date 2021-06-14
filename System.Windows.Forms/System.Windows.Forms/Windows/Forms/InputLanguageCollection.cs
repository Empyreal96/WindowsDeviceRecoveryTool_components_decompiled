using System;
using System.Collections;

namespace System.Windows.Forms
{
	/// <summary>Stores <see cref="T:System.Windows.Forms.InputLanguage" /> objects.</summary>
	// Token: 0x0200028F RID: 655
	public class InputLanguageCollection : ReadOnlyCollectionBase
	{
		// Token: 0x060026CF RID: 9935 RVA: 0x000B7635 File Offset: 0x000B5835
		internal InputLanguageCollection(InputLanguage[] value)
		{
			base.InnerList.AddRange(value);
		}

		/// <summary>Gets the entry at the specified index of the <see cref="T:System.Windows.Forms.InputLanguageCollection" />.</summary>
		/// <param name="index">The zero-based index of the entry to locate in the collection. </param>
		/// <returns>The <see cref="T:System.Windows.Forms.InputLanguage" /> at the specified index of the collection.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="index" /> is outside the valid range of indexes for the collection. </exception>
		// Token: 0x17000964 RID: 2404
		public InputLanguage this[int index]
		{
			get
			{
				return (InputLanguage)base.InnerList[index];
			}
		}

		/// <summary>Gets a value indicating whether the <see cref="T:System.Windows.Forms.InputLanguageCollection" /> contains the specified <see cref="T:System.Windows.Forms.InputLanguage" />.</summary>
		/// <param name="value">The <see cref="T:System.Windows.Forms.InputLanguage" /> to locate. </param>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Forms.InputLanguage" /> is contained in the collection; otherwise, <see langword="false" />.</returns>
		// Token: 0x060026D1 RID: 9937 RVA: 0x000B765C File Offset: 0x000B585C
		public bool Contains(InputLanguage value)
		{
			return base.InnerList.Contains(value);
		}

		/// <summary>Copies the <see cref="T:System.Windows.Forms.InputLanguageCollection" /> values to a one-dimensional <see cref="T:System.Array" /> at the specified index.</summary>
		/// <param name="array">The one-dimensional array that is the destination of the values copied from <see cref="T:System.Windows.Forms.InputLanguageCollection" />. </param>
		/// <param name="index">The index in <paramref name="array" /> where copying begins. </param>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="array" /> specifies a multidimensional array.-or- The number of elements in the <see cref="T:System.Windows.Forms.InputLanguageCollection" /> is greater than the available space between the <paramref name="index" /> and the end of <paramref name="array" />. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="array" /> is <see langword="null" />. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="index" /> is less than the lower bound of <paramref name="array" />. </exception>
		// Token: 0x060026D2 RID: 9938 RVA: 0x000B766A File Offset: 0x000B586A
		public void CopyTo(InputLanguage[] array, int index)
		{
			base.InnerList.CopyTo(array, index);
		}

		/// <summary>Returns the index of an <see cref="T:System.Windows.Forms.InputLanguage" /> in the <see cref="T:System.Windows.Forms.InputLanguageCollection" />.</summary>
		/// <param name="value">The <see cref="T:System.Windows.Forms.InputLanguage" /> to locate. </param>
		/// <returns>The index of the <see cref="T:System.Windows.Forms.InputLanguage" /> in the <see cref="T:System.Windows.Forms.InputLanguageCollection" />, if found; otherwise, -1.</returns>
		// Token: 0x060026D3 RID: 9939 RVA: 0x000B7679 File Offset: 0x000B5879
		public int IndexOf(InputLanguage value)
		{
			return base.InnerList.IndexOf(value);
		}
	}
}
