using System;
using System.Collections;
using System.ComponentModel;

namespace System.Windows.Controls.Primitives
{
	/// <summary>Provides an abstract class for controls that allow multiple items to be selected.</summary>
	// Token: 0x02000599 RID: 1433
	public abstract class MultiSelector : Selector
	{
		/// <summary>Gets or sets a value that indicates whether the multiple items in the <see cref="T:System.Windows.Controls.Primitives.MultiSelector" /> can be selected at a time. </summary>
		/// <returns>
		///     <see langword="true" /> if multiple items in the <see cref="T:System.Windows.Controls.Primitives.MultiSelector" /> can be selected at a time; otherwise, <see langword="false" />.</returns>
		// Token: 0x170016D9 RID: 5849
		// (get) Token: 0x06005E9B RID: 24219 RVA: 0x001A829F File Offset: 0x001A649F
		// (set) Token: 0x06005E9C RID: 24220 RVA: 0x001A82A7 File Offset: 0x001A64A7
		protected bool CanSelectMultipleItems
		{
			get
			{
				return base.CanSelectMultiple;
			}
			set
			{
				base.CanSelectMultiple = value;
			}
		}

		/// <summary>Gets the items in the <see cref="T:System.Windows.Controls.Primitives.MultiSelector" /> that are selected.</summary>
		/// <returns>The items in the <see cref="T:System.Windows.Controls.Primitives.MultiSelector" /> that are selected.</returns>
		// Token: 0x170016DA RID: 5850
		// (get) Token: 0x06005E9D RID: 24221 RVA: 0x0016C4A1 File Offset: 0x0016A6A1
		[Bindable(true)]
		[Category("Appearance")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public IList SelectedItems
		{
			get
			{
				return base.SelectedItemsImpl;
			}
		}

		/// <summary>Starts a new selection transaction.</summary>
		/// <exception cref="T:System.InvalidOperationException">
		///         <see cref="P:System.Windows.Controls.Primitives.MultiSelector.IsUpdatingSelectedItems" /> is <see langword="true" /> when this method is called.</exception>
		// Token: 0x06005E9E RID: 24222 RVA: 0x001A82B0 File Offset: 0x001A64B0
		protected void BeginUpdateSelectedItems()
		{
			((SelectedItemCollection)this.SelectedItems).BeginUpdateSelectedItems();
		}

		/// <summary>Commits the selected items to the <see cref="T:System.Windows.Controls.Primitives.MultiSelector" />.</summary>
		/// <exception cref="T:System.InvalidOperationException">
		///         <see cref="P:System.Windows.Controls.Primitives.MultiSelector.IsUpdatingSelectedItems" /> is <see langword="false" /> when this method is called.</exception>
		// Token: 0x06005E9F RID: 24223 RVA: 0x001A82C2 File Offset: 0x001A64C2
		protected void EndUpdateSelectedItems()
		{
			((SelectedItemCollection)this.SelectedItems).EndUpdateSelectedItems();
		}

		/// <summary>Gets a value that indicates whether the <see cref="T:System.Windows.Controls.Primitives.MultiSelector" /> is currently performing a bulk update to the <see cref="P:System.Windows.Controls.Primitives.MultiSelector.SelectedItems" /> collection.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Controls.Primitives.MultiSelector" /> is currently performing a bulk update to the <see cref="P:System.Windows.Controls.Primitives.MultiSelector.SelectedItems" /> collection; otherwise, <see langword="false" />.</returns>
		// Token: 0x170016DB RID: 5851
		// (get) Token: 0x06005EA0 RID: 24224 RVA: 0x001A82D4 File Offset: 0x001A64D4
		protected bool IsUpdatingSelectedItems
		{
			get
			{
				return ((SelectedItemCollection)this.SelectedItems).IsUpdatingSelectedItems;
			}
		}

		/// <summary>Selects all of the items in the <see cref="T:System.Windows.Controls.Primitives.MultiSelector" />.</summary>
		/// <exception cref="T:System.InvalidOperationException">
		///         <see cref="P:System.Windows.Controls.Primitives.MultiSelector.CanSelectMultipleItems" /> is <see langword="false" />.</exception>
		// Token: 0x06005EA1 RID: 24225 RVA: 0x001A82E6 File Offset: 0x001A64E6
		public void SelectAll()
		{
			if (this.CanSelectMultipleItems)
			{
				this.SelectAllImpl();
				return;
			}
			throw new NotSupportedException(SR.Get("MultiSelectorSelectAll"));
		}

		/// <summary>Unselects all of the items in the <see cref="T:System.Windows.Controls.Primitives.MultiSelector" />.</summary>
		// Token: 0x06005EA2 RID: 24226 RVA: 0x0016C3DF File Offset: 0x0016A5DF
		public void UnselectAll()
		{
			this.UnselectAllImpl();
		}
	}
}
