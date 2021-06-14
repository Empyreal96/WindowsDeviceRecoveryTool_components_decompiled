using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace System.Windows.Forms
{
	/// <summary>Represents a sorted collection of <see cref="T:System.Windows.Forms.NumericUpDownAcceleration" /> objects in the <see cref="T:System.Windows.Forms.NumericUpDown" /> control.</summary>
	// Token: 0x020002FF RID: 767
	[ListBindable(false)]
	public class NumericUpDownAccelerationCollection : MarshalByRefObject, ICollection<NumericUpDownAcceleration>, IEnumerable<NumericUpDownAcceleration>, IEnumerable
	{
		/// <summary>Adds a new <see cref="T:System.Windows.Forms.NumericUpDownAcceleration" /> to the <see cref="T:System.Windows.Forms.NumericUpDownAccelerationCollection" />.</summary>
		/// <param name="acceleration">The <see cref="T:System.Windows.Forms.NumericUpDownAcceleration" /> to add to the <see cref="T:System.Windows.Forms.NumericUpDownAccelerationCollection" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="acceleration" /> is <see langword="null" />.</exception>
		// Token: 0x06002E95 RID: 11925 RVA: 0x000D8A18 File Offset: 0x000D6C18
		public void Add(NumericUpDownAcceleration acceleration)
		{
			if (acceleration == null)
			{
				throw new ArgumentNullException("acceleration");
			}
			int num = 0;
			while (num < this.items.Count && acceleration.Seconds >= this.items[num].Seconds)
			{
				num++;
			}
			this.items.Insert(num, acceleration);
		}

		/// <summary>Removes all elements from the <see cref="T:System.Windows.Forms.NumericUpDownAccelerationCollection" />.</summary>
		// Token: 0x06002E96 RID: 11926 RVA: 0x000D8A6F File Offset: 0x000D6C6F
		public void Clear()
		{
			this.items.Clear();
		}

		/// <summary>Determines whether the <see cref="T:System.Windows.Forms.NumericUpDownAccelerationCollection" /> contains a specific <see cref="T:System.Windows.Forms.NumericUpDownAcceleration" />.</summary>
		/// <param name="acceleration">The <see cref="T:System.Windows.Forms.NumericUpDownAcceleration" /> to locate in the <see cref="T:System.Windows.Forms.NumericUpDownAccelerationCollection" />.</param>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Forms.NumericUpDownAcceleration" /> is found in the <see cref="T:System.Windows.Forms.NumericUpDownAccelerationCollection" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x06002E97 RID: 11927 RVA: 0x000D8A7C File Offset: 0x000D6C7C
		public bool Contains(NumericUpDownAcceleration acceleration)
		{
			return this.items.Contains(acceleration);
		}

		/// <summary>Copies the <see cref="T:System.Windows.Forms.NumericUpDownAccelerationCollection" /> values to a one-dimensional <see cref="T:System.Windows.Forms.NumericUpDownAcceleration" /> instance at the specified index.</summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Windows.Forms.NumericUpDownAcceleration" /> that is the destination of the values copied from <see cref="T:System.Windows.Forms.NumericUpDownAccelerationCollection" />. </param>
		/// <param name="index">The index in <paramref name="array" /> where copying begins.</param>
		// Token: 0x06002E98 RID: 11928 RVA: 0x000D8A8A File Offset: 0x000D6C8A
		public void CopyTo(NumericUpDownAcceleration[] array, int index)
		{
			this.items.CopyTo(array, index);
		}

		/// <summary>Gets the number of objects in the <see cref="T:System.Windows.Forms.NumericUpDownAccelerationCollection" />.</summary>
		/// <returns>The number of objects in the collection.</returns>
		// Token: 0x17000B3A RID: 2874
		// (get) Token: 0x06002E99 RID: 11929 RVA: 0x000D8A99 File Offset: 0x000D6C99
		public int Count
		{
			get
			{
				return this.items.Count;
			}
		}

		/// <summary>Gets a value indicating whether the <see cref="T:System.Windows.Forms.NumericUpDownAccelerationCollection" /> is read-only.</summary>
		/// <returns>
		///     <see langword="true" /> if the collection is read-only; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000B3B RID: 2875
		// (get) Token: 0x06002E9A RID: 11930 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		/// <summary>Removes the first occurrence of the specified <see cref="T:System.Windows.Forms.NumericUpDownAcceleration" /> from the <see cref="T:System.Windows.Forms.NumericUpDownAccelerationCollection" />.</summary>
		/// <param name="acceleration">The <see cref="T:System.Windows.Forms.NumericUpDownAcceleration" /> to remove from the collection.</param>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Forms.NumericUpDownAcceleration" /> is removed from <see cref="T:System.Windows.Forms.NumericUpDownAccelerationCollection" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x06002E9B RID: 11931 RVA: 0x000D8AA6 File Offset: 0x000D6CA6
		public bool Remove(NumericUpDownAcceleration acceleration)
		{
			return this.items.Remove(acceleration);
		}

		// Token: 0x06002E9C RID: 11932 RVA: 0x000D8AB4 File Offset: 0x000D6CB4
		IEnumerator<NumericUpDownAcceleration> IEnumerable<NumericUpDownAcceleration>.GetEnumerator()
		{
			return this.items.GetEnumerator();
		}

		/// <summary>Gets the enumerator for the collection.</summary>
		/// <returns>An iteration over the collection.</returns>
		// Token: 0x06002E9D RID: 11933 RVA: 0x000D8AC6 File Offset: 0x000D6CC6
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)this.items).GetEnumerator();
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.NumericUpDownAccelerationCollection" /> class.</summary>
		// Token: 0x06002E9E RID: 11934 RVA: 0x000D8AD3 File Offset: 0x000D6CD3
		public NumericUpDownAccelerationCollection()
		{
			this.items = new List<NumericUpDownAcceleration>();
		}

		/// <summary>Adds the elements of the specified array to the <see cref="T:System.Windows.Forms.NumericUpDownAccelerationCollection" />, keeping the collection sorted.</summary>
		/// <param name="accelerations">An array of type <see cref="T:System.Windows.Forms.NumericUpDownAcceleration" />  containing the objects to add to the collection.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="accelerations" /> is <see langword="null" />, or one of the entries in the <paramref name="accelerations" /> array is <see langword="null" />.</exception>
		// Token: 0x06002E9F RID: 11935 RVA: 0x000D8AE8 File Offset: 0x000D6CE8
		public void AddRange(params NumericUpDownAcceleration[] accelerations)
		{
			if (accelerations == null)
			{
				throw new ArgumentNullException("accelerations");
			}
			for (int i = 0; i < accelerations.Length; i++)
			{
				if (accelerations[i] == null)
				{
					throw new ArgumentNullException(SR.GetString("NumericUpDownAccelerationCollectionAtLeastOneEntryIsNull"));
				}
			}
			foreach (NumericUpDownAcceleration acceleration in accelerations)
			{
				this.Add(acceleration);
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Forms.NumericUpDownAcceleration" /> at the specified index number.</summary>
		/// <param name="index">The zero-based index of the <see cref="T:System.Windows.Forms.NumericUpDownAcceleration" /> to get from the collection.</param>
		/// <returns>The <see cref="T:System.Windows.Forms.NumericUpDownAcceleration" /> with the specified index.</returns>
		// Token: 0x17000B3C RID: 2876
		public NumericUpDownAcceleration this[int index]
		{
			get
			{
				return this.items[index];
			}
		}

		// Token: 0x04001D3C RID: 7484
		private List<NumericUpDownAcceleration> items;
	}
}
