using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Data.OData.Query.SemanticAst
{
	// Token: 0x02000077 RID: 119
	public sealed class SelectExpandClause
	{
		// Token: 0x060002C8 RID: 712 RVA: 0x0000A8EE File Offset: 0x00008AEE
		public SelectExpandClause(ICollection<SelectItem> selectedItems, bool allSelected)
		{
			this.usedInternalLegacyConsturctor = false;
			this.selectedItems = selectedItems;
			this.allSelected = new bool?(allSelected);
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x0000A910 File Offset: 0x00008B10
		internal SelectExpandClause(Selection selection, Expansion expansion)
		{
			this.usedInternalLegacyConsturctor = true;
			this.selection = selection;
			this.expansion = (expansion ?? new Expansion(new List<ExpandedNavigationSelectItem>()));
			this.selectedItems = null;
			this.allSelected = null;
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x060002CA RID: 714 RVA: 0x0000A94E File Offset: 0x00008B4E
		public IEnumerable<SelectItem> SelectedItems
		{
			get
			{
				return this.selectedItems;
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x060002CB RID: 715 RVA: 0x0000A956 File Offset: 0x00008B56
		public bool AllSelected
		{
			get
			{
				if (this.usedInternalLegacyConsturctor)
				{
					return this.Selection is AllSelection;
				}
				return this.allSelected.Value;
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x060002CC RID: 716 RVA: 0x0000A97A File Offset: 0x00008B7A
		internal Selection Selection
		{
			get
			{
				return this.selection;
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x060002CD RID: 717 RVA: 0x0000A982 File Offset: 0x00008B82
		internal Expansion Expansion
		{
			get
			{
				return this.expansion;
			}
		}

		// Token: 0x060002CE RID: 718 RVA: 0x0000A998 File Offset: 0x00008B98
		internal void AddSelectItem(SelectItem itemToAdd)
		{
			ExceptionUtils.CheckArgumentNotNull<SelectItem>(itemToAdd, "itemToAdd");
			if (this.selection is AllSelection)
			{
				return;
			}
			if (this.selection is ExpansionsOnly || this.selection is UnknownSelection)
			{
				this.selection = new PartialSelection(new List<SelectItem>
				{
					itemToAdd
				});
				return;
			}
			List<SelectItem> list = ((PartialSelection)this.selection).SelectedItems.ToList<SelectItem>();
			if (itemToAdd is WildcardSelectItem)
			{
				IEnumerable<SelectItem> enumerable = list.Where(new Func<SelectItem, bool>(UriUtils.IsStructuralOrNavigationPropertySelectionItem)).ToArray<SelectItem>();
				using (IEnumerator<SelectItem> enumerator = enumerable.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						SelectItem item2 = enumerator.Current;
						list.Remove(item2);
					}
					goto IL_E2;
				}
			}
			if (UriUtils.IsStructuralOrNavigationPropertySelectionItem(itemToAdd))
			{
				if (list.Any((SelectItem item) => item is WildcardSelectItem))
				{
					return;
				}
			}
			IL_E2:
			list.Add(itemToAdd);
			this.selection = new PartialSelection(list);
		}

		// Token: 0x060002CF RID: 719 RVA: 0x0000AAAC File Offset: 0x00008CAC
		internal void SetAllSelectionRecursively()
		{
			this.selection = AllSelection.Instance;
			foreach (ExpandedNavigationSelectItem expandedNavigationSelectItem in this.expansion.ExpandItems)
			{
				expandedNavigationSelectItem.SelectAndExpand.SetAllSelectionRecursively();
			}
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x0000AB10 File Offset: 0x00008D10
		internal void InitializeEmptySelection()
		{
			if (this.selection is UnknownSelection)
			{
				this.selection = ExpansionsOnly.Instance;
			}
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x0000AB2C File Offset: 0x00008D2C
		internal void ComputeFinalSelectedItems()
		{
			if (this.selectedItems != null)
			{
				throw new InvalidOperationException("This should only be called once at the end of processing.");
			}
			this.selectedItems = new List<SelectItem>();
			PartialSelection partialSelection = this.Selection as PartialSelection;
			if (partialSelection != null)
			{
				foreach (SelectItem item in partialSelection.SelectedItems)
				{
					this.selectedItems.Add(item);
				}
			}
			foreach (ExpandedNavigationSelectItem expandedNavigationSelectItem in this.Expansion.ExpandItems)
			{
				if (expandedNavigationSelectItem.SelectAndExpand != null)
				{
					expandedNavigationSelectItem.SelectAndExpand.ComputeFinalSelectedItems();
				}
				this.selectedItems.Add(expandedNavigationSelectItem);
			}
		}

		// Token: 0x040000C1 RID: 193
		private readonly Expansion expansion;

		// Token: 0x040000C2 RID: 194
		private readonly bool usedInternalLegacyConsturctor;

		// Token: 0x040000C3 RID: 195
		private Selection selection;

		// Token: 0x040000C4 RID: 196
		private ICollection<SelectItem> selectedItems;

		// Token: 0x040000C5 RID: 197
		private bool? allSelected;
	}
}
