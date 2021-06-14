using System;
using System.Windows.Controls;
using System.Windows.Data;

namespace System.Windows
{
	/// <summary>Represents a <see cref="T:System.Windows.DataTemplate" /> that supports <see cref="T:System.Windows.Controls.HeaderedItemsControl" />, such as <see cref="T:System.Windows.Controls.TreeViewItem" /> or <see cref="T:System.Windows.Controls.MenuItem" />.</summary>
	// Token: 0x020000CF RID: 207
	public class HierarchicalDataTemplate : DataTemplate
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.HierarchicalDataTemplate" /> class.</summary>
		// Token: 0x0600072D RID: 1837 RVA: 0x00016BBB File Offset: 0x00014DBB
		public HierarchicalDataTemplate()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.HierarchicalDataTemplate" /> class with the specified type for which the template is intended.</summary>
		/// <param name="dataType">The type for which this template is intended. </param>
		// Token: 0x0600072E RID: 1838 RVA: 0x00016BC3 File Offset: 0x00014DC3
		public HierarchicalDataTemplate(object dataType) : base(dataType)
		{
		}

		/// <summary>Gets or sets the binding for this data template, which indicates where to find the collection that represents the next level in the data hierarchy.</summary>
		/// <returns>The default is <see langword="null" />.</returns>
		// Token: 0x17000171 RID: 369
		// (get) Token: 0x0600072F RID: 1839 RVA: 0x00016BCC File Offset: 0x00014DCC
		// (set) Token: 0x06000730 RID: 1840 RVA: 0x00016BD4 File Offset: 0x00014DD4
		public BindingBase ItemsSource
		{
			get
			{
				return this._itemsSourceBinding;
			}
			set
			{
				base.CheckSealed();
				this._itemsSourceBinding = value;
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.DataTemplate" /> to apply to the <see cref="P:System.Windows.Controls.ItemsControl.ItemTemplate" /> property on a generated <see cref="T:System.Windows.Controls.HeaderedItemsControl" /> (such as a <see cref="T:System.Windows.Controls.MenuItem" /> or a <see cref="T:System.Windows.Controls.TreeViewItem" />), to indicate how to display items from the next level in the data hierarchy.</summary>
		/// <returns>The <see cref="T:System.Windows.DataTemplate" /> to apply to the <see cref="P:System.Windows.Controls.ItemsControl.ItemTemplate" /> property on a generated <see cref="T:System.Windows.Controls.HeaderedItemsControl" /> (such as a <see cref="T:System.Windows.Controls.MenuItem" /> or a <see cref="T:System.Windows.Controls.TreeViewItem" />), to indicate how to display items from the next level in the data hierarchy.</returns>
		// Token: 0x17000172 RID: 370
		// (get) Token: 0x06000731 RID: 1841 RVA: 0x00016BE3 File Offset: 0x00014DE3
		// (set) Token: 0x06000732 RID: 1842 RVA: 0x00016BEB File Offset: 0x00014DEB
		public DataTemplate ItemTemplate
		{
			get
			{
				return this._itemTemplate;
			}
			set
			{
				base.CheckSealed();
				this._itemTemplate = value;
				this._itemTemplateSet = true;
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.Controls.DataTemplateSelector" /> to apply to the <see cref="P:System.Windows.Controls.ItemsControl.ItemTemplateSelector" /> property on a generated <see cref="T:System.Windows.Controls.HeaderedItemsControl" /> (such as a <see cref="T:System.Windows.Controls.MenuItem" /> or a <see cref="T:System.Windows.Controls.TreeViewItem" />), to indicate how to select a template to display items from the next level in the data hierarchy.</summary>
		/// <returns>The <see cref="T:System.Windows.Controls.DataTemplateSelector" /> object to apply to the <see cref="P:System.Windows.Controls.ItemsControl.ItemTemplateSelector" /> property on a generated <see cref="T:System.Windows.Controls.HeaderedItemsControl" /> (such as a <see cref="T:System.Windows.Controls.MenuItem" /> or a <see cref="T:System.Windows.Controls.TreeViewItem" />), to indicate how to select a template to display items from the next level in the data hierarchy.</returns>
		// Token: 0x17000173 RID: 371
		// (get) Token: 0x06000733 RID: 1843 RVA: 0x00016C01 File Offset: 0x00014E01
		// (set) Token: 0x06000734 RID: 1844 RVA: 0x00016C09 File Offset: 0x00014E09
		public DataTemplateSelector ItemTemplateSelector
		{
			get
			{
				return this._itemTemplateSelector;
			}
			set
			{
				base.CheckSealed();
				this._itemTemplateSelector = value;
				this._itemTemplateSelectorSet = true;
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.Style" /> that is applied to the item container for each child item.</summary>
		/// <returns>The <see cref="T:System.Windows.Style" /> that is applied to the item container for each child item.</returns>
		// Token: 0x17000174 RID: 372
		// (get) Token: 0x06000735 RID: 1845 RVA: 0x00016C1F File Offset: 0x00014E1F
		// (set) Token: 0x06000736 RID: 1846 RVA: 0x00016C27 File Offset: 0x00014E27
		public Style ItemContainerStyle
		{
			get
			{
				return this._itemContainerStyle;
			}
			set
			{
				base.CheckSealed();
				this._itemContainerStyle = value;
				this._itemContainerStyleSet = true;
			}
		}

		/// <summary>Gets or sets custom style-selection logic for a style that can be applied to each item container. </summary>
		/// <returns>A <see cref="T:System.Windows.Controls.StyleSelector" /> that chooses which style to use as the <see cref="P:System.Windows.HierarchicalDataTemplate.ItemContainerStyle" />. The default is <see langword="null" />.</returns>
		// Token: 0x17000175 RID: 373
		// (get) Token: 0x06000737 RID: 1847 RVA: 0x00016C3D File Offset: 0x00014E3D
		// (set) Token: 0x06000738 RID: 1848 RVA: 0x00016C45 File Offset: 0x00014E45
		public StyleSelector ItemContainerStyleSelector
		{
			get
			{
				return this._itemContainerStyleSelector;
			}
			set
			{
				base.CheckSealed();
				this._itemContainerStyleSelector = value;
				this._itemContainerStyleSelectorSet = true;
			}
		}

		/// <summary>Gets or sets a composite string that specifies how to format the items in the next level in the data hierarchy if they are displayed as strings.</summary>
		/// <returns>A composite string that specifies how to format the items in the next level of the data hierarchy if they are displayed as strings.</returns>
		// Token: 0x17000176 RID: 374
		// (get) Token: 0x06000739 RID: 1849 RVA: 0x00016C5B File Offset: 0x00014E5B
		// (set) Token: 0x0600073A RID: 1850 RVA: 0x00016C63 File Offset: 0x00014E63
		public string ItemStringFormat
		{
			get
			{
				return this._itemStringFormat;
			}
			set
			{
				base.CheckSealed();
				this._itemStringFormat = value;
				this._itemStringFormatSet = true;
			}
		}

		/// <summary>Gets or sets the number of alternating item containers for the child items.</summary>
		/// <returns>The number of alternating item containers for the next level of items.</returns>
		// Token: 0x17000177 RID: 375
		// (get) Token: 0x0600073B RID: 1851 RVA: 0x00016C79 File Offset: 0x00014E79
		// (set) Token: 0x0600073C RID: 1852 RVA: 0x00016C81 File Offset: 0x00014E81
		public int AlternationCount
		{
			get
			{
				return this._alternationCount;
			}
			set
			{
				base.CheckSealed();
				this._alternationCount = value;
				this._alternationCountSet = true;
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.Data.BindingGroup" /> that is copied to each child item.</summary>
		/// <returns>The <see cref="T:System.Windows.Data.BindingGroup" /> that is copied to each child item.</returns>
		// Token: 0x17000178 RID: 376
		// (get) Token: 0x0600073D RID: 1853 RVA: 0x00016C97 File Offset: 0x00014E97
		// (set) Token: 0x0600073E RID: 1854 RVA: 0x00016C9F File Offset: 0x00014E9F
		public BindingGroup ItemBindingGroup
		{
			get
			{
				return this._itemBindingGroup;
			}
			set
			{
				base.CheckSealed();
				this._itemBindingGroup = value;
				this._itemBindingGroupSet = true;
			}
		}

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x0600073F RID: 1855 RVA: 0x00016CB5 File Offset: 0x00014EB5
		internal bool IsItemTemplateSet
		{
			get
			{
				return this._itemTemplateSet;
			}
		}

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x06000740 RID: 1856 RVA: 0x00016CBD File Offset: 0x00014EBD
		internal bool IsItemTemplateSelectorSet
		{
			get
			{
				return this._itemTemplateSelectorSet;
			}
		}

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x06000741 RID: 1857 RVA: 0x00016CC5 File Offset: 0x00014EC5
		internal bool IsItemContainerStyleSet
		{
			get
			{
				return this._itemContainerStyleSet;
			}
		}

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x06000742 RID: 1858 RVA: 0x00016CCD File Offset: 0x00014ECD
		internal bool IsItemContainerStyleSelectorSet
		{
			get
			{
				return this._itemContainerStyleSelectorSet;
			}
		}

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x06000743 RID: 1859 RVA: 0x00016CD5 File Offset: 0x00014ED5
		internal bool IsItemStringFormatSet
		{
			get
			{
				return this._itemStringFormatSet;
			}
		}

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x06000744 RID: 1860 RVA: 0x00016CDD File Offset: 0x00014EDD
		internal bool IsAlternationCountSet
		{
			get
			{
				return this._alternationCountSet;
			}
		}

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x06000745 RID: 1861 RVA: 0x00016CE5 File Offset: 0x00014EE5
		internal bool IsItemBindingGroupSet
		{
			get
			{
				return this._itemBindingGroupSet;
			}
		}

		// Token: 0x0400071A RID: 1818
		private BindingBase _itemsSourceBinding;

		// Token: 0x0400071B RID: 1819
		private DataTemplate _itemTemplate;

		// Token: 0x0400071C RID: 1820
		private DataTemplateSelector _itemTemplateSelector;

		// Token: 0x0400071D RID: 1821
		private Style _itemContainerStyle;

		// Token: 0x0400071E RID: 1822
		private StyleSelector _itemContainerStyleSelector;

		// Token: 0x0400071F RID: 1823
		private string _itemStringFormat;

		// Token: 0x04000720 RID: 1824
		private int _alternationCount;

		// Token: 0x04000721 RID: 1825
		private BindingGroup _itemBindingGroup;

		// Token: 0x04000722 RID: 1826
		private bool _itemTemplateSet;

		// Token: 0x04000723 RID: 1827
		private bool _itemTemplateSelectorSet;

		// Token: 0x04000724 RID: 1828
		private bool _itemContainerStyleSet;

		// Token: 0x04000725 RID: 1829
		private bool _itemContainerStyleSelectorSet;

		// Token: 0x04000726 RID: 1830
		private bool _itemStringFormatSet;

		// Token: 0x04000727 RID: 1831
		private bool _alternationCountSet;

		// Token: 0x04000728 RID: 1832
		private bool _itemBindingGroupSet;
	}
}
