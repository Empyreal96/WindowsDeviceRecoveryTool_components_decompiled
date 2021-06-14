using System;
using System.Collections;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	/// <summary>A collection that stores <see cref="T:System.Windows.Forms.RowStyle" /> objects.</summary>
	// Token: 0x0200038B RID: 907
	public class TableLayoutRowStyleCollection : TableLayoutStyleCollection
	{
		// Token: 0x060038EC RID: 14572 RVA: 0x000FE814 File Offset: 0x000FCA14
		internal TableLayoutRowStyleCollection(IArrangedElement Owner) : base(Owner)
		{
		}

		// Token: 0x060038ED RID: 14573 RVA: 0x000FE81D File Offset: 0x000FCA1D
		internal TableLayoutRowStyleCollection() : base(null)
		{
		}

		// Token: 0x17000E37 RID: 3639
		// (get) Token: 0x060038EE RID: 14574 RVA: 0x000FE860 File Offset: 0x000FCA60
		internal override string PropertyName
		{
			get
			{
				return PropertyNames.RowStyles;
			}
		}

		/// <summary>Adds a new <see cref="T:System.Windows.Forms.RowStyle" /> to the <see cref="T:System.Windows.Forms.TableLayoutRowStyleCollection" />.</summary>
		/// <param name="rowStyle">The <see cref="T:System.Windows.Forms.RowStyle" /> to add to the <see cref="T:System.Windows.Forms.TableLayoutRowStyleCollection" />.</param>
		/// <returns>The position into which the new element was inserted.</returns>
		// Token: 0x060038EF RID: 14575 RVA: 0x000FE57A File Offset: 0x000FC77A
		public int Add(RowStyle rowStyle)
		{
			return ((IList)this).Add(rowStyle);
		}

		/// <summary>Inserts a <see cref="T:System.Windows.Forms.RowStyle" /> into the <see cref="T:System.Windows.Forms.TableLayoutRowStyleCollection" /> at the specified position.</summary>
		/// <param name="index">The zero-based index at which the <see cref="T:System.Windows.Forms.RowStyle" /> should be inserted.</param>
		/// <param name="rowStyle">The <see cref="T:System.Windows.Forms.RowStyle" /> to insert into the <see cref="T:System.Windows.Forms.TableLayoutRowStyleCollection" />. The value can be <see langword="null" />.</param>
		// Token: 0x060038F0 RID: 14576 RVA: 0x000FE82D File Offset: 0x000FCA2D
		public void Insert(int index, RowStyle rowStyle)
		{
			((IList)this).Insert(index, rowStyle);
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.Forms.RowStyle" /> at the specified index.</summary>
		/// <param name="index">The zero-based index of the <see cref="T:System.Windows.Forms.RowStyle" /> to get or set.</param>
		/// <returns>A <see cref="T:System.Windows.Forms.RowStyle" /> at the specified index.</returns>
		// Token: 0x17000E38 RID: 3640
		public RowStyle this[int index]
		{
			get
			{
				return (RowStyle)((IList)this)[index];
			}
			set
			{
				((IList)this)[index] = value;
			}
		}

		/// <summary>Removes the first occurrence of a specific object from the <see cref="T:System.Windows.Forms.TableLayoutRowStyleCollection" />.</summary>
		/// <param name="rowStyle">The <see cref="T:System.Windows.Forms.RowStyle" /> to remove from the <see cref="T:System.Windows.Forms.TableLayoutRowStyleCollection" />. The value can be <see langword="null" />.</param>
		// Token: 0x060038F3 RID: 14579 RVA: 0x000FE845 File Offset: 0x000FCA45
		public void Remove(RowStyle rowStyle)
		{
			((IList)this).Remove(rowStyle);
		}

		/// <summary>Determines whether the <see cref="T:System.Windows.Forms.TableLayoutRowStyleCollection" /> contains a specific style.</summary>
		/// <param name="rowStyle">The <see cref="T:System.Windows.Forms.RowStyle" /> to locate in the <see cref="T:System.Windows.Forms.TableLayoutRowStyleCollection" />.</param>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Forms.RowStyle" /> is found in the <see cref="T:System.Windows.Forms.TableLayoutRowStyleCollection" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x060038F4 RID: 14580 RVA: 0x000FE84E File Offset: 0x000FCA4E
		public bool Contains(RowStyle rowStyle)
		{
			return ((IList)this).Contains(rowStyle);
		}

		/// <summary>Determines the index of a specific item in the <see cref="T:System.Windows.Forms.TableLayoutRowStyleCollection" />.</summary>
		/// <param name="rowStyle">The <see cref="T:System.Windows.Forms.RowStyle" /> to locate in the <see cref="T:System.Windows.Forms.TableLayoutRowStyleCollection" />.</param>
		/// <returns>The index of <paramref name="rowStyle" /> if found in the <see cref="T:System.Windows.Forms.TableLayoutRowStyleCollection" />; otherwise, -1.</returns>
		// Token: 0x060038F5 RID: 14581 RVA: 0x000FE857 File Offset: 0x000FCA57
		public int IndexOf(RowStyle rowStyle)
		{
			return ((IList)this).IndexOf(rowStyle);
		}
	}
}
