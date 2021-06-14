using System;
using System.Collections;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	/// <summary>A collection that stores <see cref="T:System.Windows.Forms.ColumnStyle" /> objects.</summary>
	// Token: 0x0200038A RID: 906
	public class TableLayoutColumnStyleCollection : TableLayoutStyleCollection
	{
		// Token: 0x060038E2 RID: 14562 RVA: 0x000FE814 File Offset: 0x000FCA14
		internal TableLayoutColumnStyleCollection(IArrangedElement Owner) : base(Owner)
		{
		}

		// Token: 0x060038E3 RID: 14563 RVA: 0x000FE81D File Offset: 0x000FCA1D
		internal TableLayoutColumnStyleCollection() : base(null)
		{
		}

		// Token: 0x17000E35 RID: 3637
		// (get) Token: 0x060038E4 RID: 14564 RVA: 0x000FE826 File Offset: 0x000FCA26
		internal override string PropertyName
		{
			get
			{
				return PropertyNames.ColumnStyles;
			}
		}

		/// <summary>Adds an item to the <see cref="T:System.Windows.Forms.TableLayoutColumnStyleCollection" />.</summary>
		/// <param name="columnStyle">The <see cref="T:System.Windows.Forms.ColumnStyle" /> to add to the <see cref="T:System.Windows.Forms.TableLayoutColumnStyleCollection" />.</param>
		/// <returns>The position into which the new element was inserted.</returns>
		// Token: 0x060038E5 RID: 14565 RVA: 0x000FE57A File Offset: 0x000FC77A
		public int Add(ColumnStyle columnStyle)
		{
			return ((IList)this).Add(columnStyle);
		}

		/// <summary>Inserts a <see cref="T:System.Windows.Forms.ColumnStyle" /> into the <see cref="T:System.Windows.Forms.TableLayoutColumnStyleCollection" /> at the specified position.</summary>
		/// <param name="index">The zero-based index at which <see cref="T:System.Windows.Forms.ColumnStyle" /> should be inserted.</param>
		/// <param name="columnStyle">The <see cref="T:System.Windows.Forms.ColumnStyle" /> to insert into the <see cref="T:System.Windows.Forms.TableLayoutColumnStyleCollection" />.</param>
		// Token: 0x060038E6 RID: 14566 RVA: 0x000FE82D File Offset: 0x000FCA2D
		public void Insert(int index, ColumnStyle columnStyle)
		{
			((IList)this).Insert(index, columnStyle);
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.Forms.ColumnStyle" /> at the specified index.</summary>
		/// <param name="index">The zero-based index of the <see cref="T:System.Windows.Forms.ColumnStyle" /> to get or set.</param>
		/// <returns>A <see cref="T:System.Windows.Forms.ColumnStyle" /> at the specified index.</returns>
		// Token: 0x17000E36 RID: 3638
		public ColumnStyle this[int index]
		{
			get
			{
				return (ColumnStyle)((IList)this)[index];
			}
			set
			{
				((IList)this)[index] = value;
			}
		}

		/// <summary>Removes the first occurrence of a specific <see cref="T:System.Windows.Forms.ColumnStyle" /> from the <see cref="T:System.Windows.Forms.TableLayoutColumnStyleCollection" />.</summary>
		/// <param name="columnStyle">The <see cref="T:System.Windows.Forms.ColumnStyle" /> to remove from the <see cref="T:System.Windows.Forms.TableLayoutColumnStyleCollection" />. The value can be <see langword="null" />.</param>
		// Token: 0x060038E9 RID: 14569 RVA: 0x000FE845 File Offset: 0x000FCA45
		public void Remove(ColumnStyle columnStyle)
		{
			((IList)this).Remove(columnStyle);
		}

		/// <summary>Determines whether the specified <see cref="T:System.Windows.Forms.ColumnStyle" /> is in the collection.</summary>
		/// <param name="columnStyle">The <see cref="T:System.Windows.Forms.ColumnStyle" /> to locate in the <see cref="T:System.Windows.Forms.TableLayoutColumnStyleCollection" />. The value can be <see langword="null" />.</param>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Forms.ColumnStyle" /> is found in the <see cref="T:System.Windows.Forms.TableLayoutColumnStyleCollection" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x060038EA RID: 14570 RVA: 0x000FE84E File Offset: 0x000FCA4E
		public bool Contains(ColumnStyle columnStyle)
		{
			return ((IList)this).Contains(columnStyle);
		}

		/// <summary>Determines the index of a specific item in the <see cref="T:System.Windows.Forms.TableLayoutColumnStyleCollection" />.</summary>
		/// <param name="columnStyle">The <see cref="T:System.Windows.Forms.ColumnStyle" /> to locate in the <see cref="T:System.Windows.Forms.TableLayoutColumnStyleCollection" />.</param>
		/// <returns>The index of <paramref name="columnStyle" /> if found in the <see cref="T:System.Windows.Forms.TableLayoutColumnStyleCollection" />; otherwise, -1.</returns>
		// Token: 0x060038EB RID: 14571 RVA: 0x000FE857 File Offset: 0x000FCA57
		public int IndexOf(ColumnStyle columnStyle)
		{
			return ((IList)this).IndexOf(columnStyle);
		}
	}
}
