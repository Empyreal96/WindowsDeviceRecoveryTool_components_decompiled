using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	/// <summary>Implements the basic functionality for a collection of table layout styles.</summary>
	// Token: 0x02000387 RID: 903
	[Editor("System.Windows.Forms.Design.StyleCollectionEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
	public abstract class TableLayoutStyleCollection : IList, ICollection, IEnumerable
	{
		// Token: 0x060038C1 RID: 14529 RVA: 0x000FE518 File Offset: 0x000FC718
		internal TableLayoutStyleCollection(IArrangedElement owner)
		{
			this._owner = owner;
		}

		// Token: 0x17000E29 RID: 3625
		// (get) Token: 0x060038C2 RID: 14530 RVA: 0x000FE532 File Offset: 0x000FC732
		internal IArrangedElement Owner
		{
			get
			{
				return this._owner;
			}
		}

		// Token: 0x17000E2A RID: 3626
		// (get) Token: 0x060038C3 RID: 14531 RVA: 0x0000DE5C File Offset: 0x0000C05C
		internal virtual string PropertyName
		{
			get
			{
				return null;
			}
		}

		/// <summary>For a description of this method, see the <see cref="M:System.Collections.IList.Add(System.Object)" /> method.</summary>
		/// <param name="style">The <see cref="T:System.Object" /> to add to the <see cref="T:System.Collections.IList" />.</param>
		/// <returns>The position into which <paramref name="style" /> was inserted.</returns>
		// Token: 0x060038C4 RID: 14532 RVA: 0x000FE53C File Offset: 0x000FC73C
		int IList.Add(object style)
		{
			this.EnsureNotOwned((TableLayoutStyle)style);
			((TableLayoutStyle)style).Owner = this.Owner;
			int result = this._innerList.Add(style);
			this.PerformLayoutIfOwned();
			return result;
		}

		/// <summary>Adds a new <see cref="T:System.Windows.Forms.TableLayoutStyle" /> to the end of the current collection.</summary>
		/// <param name="style">The <see cref="T:System.Windows.Forms.TableLayoutStyle" /> to add to the <see cref="T:System.Windows.Forms.TableLayoutStyleCollection" />.</param>
		/// <returns>The position into which the new element was inserted.</returns>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="style" /> is already assigned to another owner. You must first remove it from its current location or clone it.</exception>
		// Token: 0x060038C5 RID: 14533 RVA: 0x000FE57A File Offset: 0x000FC77A
		public int Add(TableLayoutStyle style)
		{
			return ((IList)this).Add(style);
		}

		/// <summary>For a description of this method, see the <see cref="M:System.Collections.IList.Insert(System.Int32,System.Object)" /> method.</summary>
		/// <param name="index">The zero-based index at which <paramref name="style" /> should be inserted.</param>
		/// <param name="style">The <see cref="T:System.Object" /> to insert into the <see cref="T:System.Collections.IList" />.</param>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="style" /> is already assigned to another owner. You must first remove it from its current location or clone it.</exception>
		// Token: 0x060038C6 RID: 14534 RVA: 0x000FE583 File Offset: 0x000FC783
		void IList.Insert(int index, object style)
		{
			this.EnsureNotOwned((TableLayoutStyle)style);
			((TableLayoutStyle)style).Owner = this.Owner;
			this._innerList.Insert(index, style);
			this.PerformLayoutIfOwned();
		}

		/// <summary>For a description of this method, see the <see cref="P:System.Collections.IList.Item(System.Int32)" /> property.</summary>
		/// <param name="index">The zero-based index of the element to get or set.</param>
		/// <returns>The element at the specified index.</returns>
		// Token: 0x17000E2B RID: 3627
		object IList.this[int index]
		{
			get
			{
				return this._innerList[index];
			}
			set
			{
				TableLayoutStyle tableLayoutStyle = (TableLayoutStyle)value;
				this.EnsureNotOwned(tableLayoutStyle);
				tableLayoutStyle.Owner = this.Owner;
				this._innerList[index] = tableLayoutStyle;
				this.PerformLayoutIfOwned();
			}
		}

		/// <summary>Gets or sets <see cref="T:System.Windows.Forms.TableLayoutStyle" /> at the specified index.</summary>
		/// <param name="index">The zero-based index of the <see cref="T:System.Windows.Forms.TableLayoutStyle" /> to get or set.</param>
		/// <returns>A <see cref="T:System.Windows.Forms.TableLayoutStyle" /> at the specified index.</returns>
		/// <exception cref="T:System.ArgumentException">The property value is already assigned to another owner. You must first remove it from its current location or clone it.</exception>
		// Token: 0x17000E2C RID: 3628
		public TableLayoutStyle this[int index]
		{
			get
			{
				return (TableLayoutStyle)((IList)this)[index];
			}
			set
			{
				((IList)this)[index] = value;
			}
		}

		/// <summary>For a description of this method, see the <see cref="M:System.Collections.IList.Remove(System.Object)" /> method.</summary>
		/// <param name="style">The <see cref="T:System.Object" /> to remove from the <see cref="T:System.Collections.IList" />.</param>
		// Token: 0x060038CB RID: 14539 RVA: 0x000FE616 File Offset: 0x000FC816
		void IList.Remove(object style)
		{
			((TableLayoutStyle)style).Owner = null;
			this._innerList.Remove(style);
			this.PerformLayoutIfOwned();
		}

		/// <summary>Disassociates the collection from its associated <see cref="T:System.Windows.Forms.TableLayoutPanel" /> and empties the collection.</summary>
		// Token: 0x060038CC RID: 14540 RVA: 0x000FE638 File Offset: 0x000FC838
		public void Clear()
		{
			foreach (object obj in this._innerList)
			{
				TableLayoutStyle tableLayoutStyle = (TableLayoutStyle)obj;
				tableLayoutStyle.Owner = null;
			}
			this._innerList.Clear();
			this.PerformLayoutIfOwned();
		}

		/// <summary>Removes the style at the specified index of the collection.</summary>
		/// <param name="index">The zero-based index of the <see cref="T:System.Windows.Forms.TableLayoutStyle" /> to be removed.</param>
		// Token: 0x060038CD RID: 14541 RVA: 0x000FE6A4 File Offset: 0x000FC8A4
		public void RemoveAt(int index)
		{
			TableLayoutStyle tableLayoutStyle = (TableLayoutStyle)this._innerList[index];
			tableLayoutStyle.Owner = null;
			this._innerList.RemoveAt(index);
			this.PerformLayoutIfOwned();
		}

		/// <summary>For a description of this method, see the <see cref="M:System.Collections.IList.Contains(System.Object)" /> method.</summary>
		/// <param name="style">The <see cref="T:System.Object" /> to locate in the <see cref="T:System.Collections.IList" />.</param>
		/// <returns>
		///     <see langword="true" /> if <paramref name="style" /> is found in the <see cref="T:System.Collections.IList" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x060038CE RID: 14542 RVA: 0x000FE6DC File Offset: 0x000FC8DC
		bool IList.Contains(object style)
		{
			return this._innerList.Contains(style);
		}

		/// <summary>For a description of this method, see the <see cref="M:System.Collections.IList.IndexOf(System.Object)" /> method.</summary>
		/// <param name="style">The <see cref="T:System.Object" /> to locate in the <see cref="T:System.Collections.IList" />.</param>
		/// <returns>The index of <paramref name="style" /> if found in the list; otherwise, -1.</returns>
		// Token: 0x060038CF RID: 14543 RVA: 0x000FE6EA File Offset: 0x000FC8EA
		int IList.IndexOf(object style)
		{
			return this._innerList.IndexOf(style);
		}

		/// <summary>For a description of this method, see the <see cref="P:System.Collections.IList.IsFixedSize" /> property.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Collections.IList" /> has a fixed size; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000E2D RID: 3629
		// (get) Token: 0x060038D0 RID: 14544 RVA: 0x000FE6F8 File Offset: 0x000FC8F8
		bool IList.IsFixedSize
		{
			get
			{
				return this._innerList.IsFixedSize;
			}
		}

		/// <summary>For a description of this method, see the <see cref="P:System.Collections.IList.IsReadOnly" /> property.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Collections.IList" /> is read-only; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000E2E RID: 3630
		// (get) Token: 0x060038D1 RID: 14545 RVA: 0x000FE705 File Offset: 0x000FC905
		bool IList.IsReadOnly
		{
			get
			{
				return this._innerList.IsReadOnly;
			}
		}

		/// <summary>For a description of this method, see the <see cref="M:System.Collections.ICollection.CopyTo(System.Array,System.Int32)" /> method.</summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.ICollection" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
		/// <param name="startIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
		// Token: 0x060038D2 RID: 14546 RVA: 0x000FE712 File Offset: 0x000FC912
		void ICollection.CopyTo(Array array, int startIndex)
		{
			this._innerList.CopyTo(array, startIndex);
		}

		/// <summary>Gets the number of styles actually contained in the <see cref="T:System.Windows.Forms.TableLayoutStyleCollection" />.</summary>
		/// <returns>The number of styles actually contained in the <see cref="T:System.Windows.Forms.TableLayoutStyleCollection" />.</returns>
		// Token: 0x17000E2F RID: 3631
		// (get) Token: 0x060038D3 RID: 14547 RVA: 0x000FE721 File Offset: 0x000FC921
		public int Count
		{
			get
			{
				return this._innerList.Count;
			}
		}

		/// <summary>For a description of this method, see the <see cref="P:System.Collections.ICollection.IsSynchronized" /> property.</summary>
		/// <returns>
		///     <see langword="true" /> if access to the <see cref="T:System.Collections.ICollection" /> is synchronized (thread safe); otherwise, <see langword="false" />.</returns>
		// Token: 0x17000E30 RID: 3632
		// (get) Token: 0x060038D4 RID: 14548 RVA: 0x000FE72E File Offset: 0x000FC92E
		bool ICollection.IsSynchronized
		{
			get
			{
				return this._innerList.IsSynchronized;
			}
		}

		/// <summary>For a description of this method, see the <see cref="P:System.Collections.ICollection.SyncRoot" /> property.</summary>
		/// <returns>An object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />.</returns>
		// Token: 0x17000E31 RID: 3633
		// (get) Token: 0x060038D5 RID: 14549 RVA: 0x000FE73B File Offset: 0x000FC93B
		object ICollection.SyncRoot
		{
			get
			{
				return this._innerList.SyncRoot;
			}
		}

		/// <summary>For a description of this method, see the <see cref="M:System.Collections.IEnumerable.GetEnumerator" /> method.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> that can be used to iterate through the collection.</returns>
		// Token: 0x060038D6 RID: 14550 RVA: 0x000FE748 File Offset: 0x000FC948
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this._innerList.GetEnumerator();
		}

		// Token: 0x060038D7 RID: 14551 RVA: 0x000FE755 File Offset: 0x000FC955
		private void EnsureNotOwned(TableLayoutStyle style)
		{
			if (style.Owner != null)
			{
				throw new ArgumentException(SR.GetString("OnlyOneControl", new object[]
				{
					style.GetType().Name
				}), "style");
			}
		}

		// Token: 0x060038D8 RID: 14552 RVA: 0x000FE788 File Offset: 0x000FC988
		internal void EnsureOwnership(IArrangedElement owner)
		{
			this._owner = owner;
			for (int i = 0; i < this.Count; i++)
			{
				this[i].Owner = owner;
			}
		}

		// Token: 0x060038D9 RID: 14553 RVA: 0x000FE7BA File Offset: 0x000FC9BA
		private void PerformLayoutIfOwned()
		{
			if (this.Owner != null)
			{
				LayoutTransaction.DoLayout(this.Owner, this.Owner, this.PropertyName);
			}
		}

		// Token: 0x04002277 RID: 8823
		private IArrangedElement _owner;

		// Token: 0x04002278 RID: 8824
		private ArrayList _innerList = new ArrayList();
	}
}
