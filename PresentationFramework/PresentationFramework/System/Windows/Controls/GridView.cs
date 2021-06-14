using System;
using System.ComponentModel;
using System.Windows.Automation.Peers;
using System.Windows.Markup;
using MS.Internal;
using MS.Internal.KnownBoxes;

namespace System.Windows.Controls
{
	/// <summary>Represents a view mode that displays data items in columns for a <see cref="T:System.Windows.Controls.ListView" /> control.</summary>
	// Token: 0x020004D5 RID: 1237
	[StyleTypedProperty(Property = "ColumnHeaderContainerStyle", StyleTargetType = typeof(GridViewColumnHeader))]
	[ContentProperty("Columns")]
	public class GridView : ViewBase, IAddChild
	{
		/// <summary>Adds a child object. </summary>
		/// <param name="column">The child object to add.</param>
		// Token: 0x06004CAA RID: 19626 RVA: 0x0015A89A File Offset: 0x00158A9A
		void IAddChild.AddChild(object column)
		{
			this.AddChild(column);
		}

		/// <summary>Adds a <see cref="T:System.Windows.Controls.GridViewColumn" /> object to a <see cref="T:System.Windows.Controls.GridView" />.</summary>
		/// <param name="column">The column to add </param>
		// Token: 0x06004CAB RID: 19627 RVA: 0x0015A8A4 File Offset: 0x00158AA4
		protected virtual void AddChild(object column)
		{
			GridViewColumn gridViewColumn = column as GridViewColumn;
			if (gridViewColumn != null)
			{
				this.Columns.Add(gridViewColumn);
				return;
			}
			throw new InvalidOperationException(SR.Get("ListView_IllegalChildrenType"));
		}

		/// <summary>Adds the text content of a node to the object. </summary>
		/// <param name="text">The text to add to the object.</param>
		// Token: 0x06004CAC RID: 19628 RVA: 0x0015A8D7 File Offset: 0x00158AD7
		void IAddChild.AddText(string text)
		{
			this.AddText(text);
		}

		/// <summary>Not supported.</summary>
		/// <param name="text">Text string</param>
		// Token: 0x06004CAD RID: 19629 RVA: 0x0015A89A File Offset: 0x00158A9A
		protected virtual void AddText(string text)
		{
			this.AddChild(text);
		}

		/// <summary>Returns the string representation of the <see cref="T:System.Windows.Controls.GridView" /> object.</summary>
		/// <returns>A string that indicates the number of columns in the <see cref="T:System.Windows.Controls.GridView" />.</returns>
		// Token: 0x06004CAE RID: 19630 RVA: 0x0015A8E0 File Offset: 0x00158AE0
		public override string ToString()
		{
			return SR.Get("ToStringFormatString_GridView", new object[]
			{
				base.GetType(),
				this.Columns.Count
			});
		}

		/// <summary>Gets the <see cref="T:System.Windows.Automation.Peers.AutomationPeer" /> implementation for this <see cref="T:System.Windows.Controls.GridView" /> object.</summary>
		/// <param name="parent">The <see cref="T:System.Windows.Controls.ListView" /> control that implements this <see cref="T:System.Windows.Controls.GridView" /> view.</param>
		/// <returns>A <see cref="T:System.Windows.Automation.Peers.GridViewAutomationPeer" /> for this <see cref="T:System.Windows.Controls.GridView" />.</returns>
		// Token: 0x06004CAF RID: 19631 RVA: 0x0015A90E File Offset: 0x00158B0E
		protected internal override IViewAutomationPeer GetAutomationPeer(ListView parent)
		{
			return new GridViewAutomationPeer(this, parent);
		}

		/// <summary>Gets the key that references the style that is defined for the <see cref="T:System.Windows.Controls.ScrollViewer" /> control that encloses the content that is displayed by a <see cref="T:System.Windows.Controls.GridView" />.</summary>
		/// <returns>A <see cref="T:System.Windows.ResourceKey" /> that references the <see cref="T:System.Windows.Style" /> that is applied to the <see cref="T:System.Windows.Controls.ScrollViewer" /> control for a <see cref="T:System.Windows.Controls.GridView" />. The default value is the style for the <see cref="T:System.Windows.Controls.ScrollViewer" /> object of a <see cref="T:System.Windows.Controls.ListView" /> in the current theme.</returns>
		// Token: 0x170012AE RID: 4782
		// (get) Token: 0x06004CB0 RID: 19632 RVA: 0x0015A917 File Offset: 0x00158B17
		public static ResourceKey GridViewScrollViewerStyleKey
		{
			get
			{
				return SystemResourceKey.GridViewScrollViewerStyleKey;
			}
		}

		/// <summary>Gets the key that references the style that is defined for the <see cref="T:System.Windows.Controls.GridView" />.</summary>
		/// <returns>A <see cref="T:System.Windows.ResourceKey" /> that references the <see cref="T:System.Windows.Style" /> that is applied to the <see cref="T:System.Windows.Controls.GridView" />. The default value is the style for the <see cref="T:System.Windows.Controls.ListView" /> in the current theme.</returns>
		// Token: 0x170012AF RID: 4783
		// (get) Token: 0x06004CB1 RID: 19633 RVA: 0x0015A91E File Offset: 0x00158B1E
		public static ResourceKey GridViewStyleKey
		{
			get
			{
				return SystemResourceKey.GridViewStyleKey;
			}
		}

		/// <summary>Gets the key that references the style that is defined for each <see cref="T:System.Windows.Controls.ListViewItem" /> in a <see cref="T:System.Windows.Controls.GridView" />.</summary>
		/// <returns>A <see cref="T:System.Windows.ResourceKey" /> that references the style for each <see cref="T:System.Windows.Controls.ListViewItem" />. The default value references the default style for a <see cref="T:System.Windows.Controls.ListViewItem" /> control in the current theme.</returns>
		// Token: 0x170012B0 RID: 4784
		// (get) Token: 0x06004CB2 RID: 19634 RVA: 0x0015A925 File Offset: 0x00158B25
		public static ResourceKey GridViewItemContainerStyleKey
		{
			get
			{
				return SystemResourceKey.GridViewItemContainerStyleKey;
			}
		}

		/// <summary>Gets the contents of the <see cref="P:System.Windows.Controls.GridView.ColumnCollection" /> attached property.</summary>
		/// <param name="element">The <see cref="T:System.Windows.DependencyObject" /> that is associated with the collection.</param>
		/// <returns>The <see cref="P:System.Windows.Controls.GridView.ColumnCollection" /> of the specified <see cref="T:System.Windows.DependencyObject" />.</returns>
		// Token: 0x06004CB3 RID: 19635 RVA: 0x0015A92C File Offset: 0x00158B2C
		public static GridViewColumnCollection GetColumnCollection(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (GridViewColumnCollection)element.GetValue(GridView.ColumnCollectionProperty);
		}

		/// <summary>Sets the contents of the <see cref="P:System.Windows.Controls.GridView.ColumnCollection" /> attached property.</summary>
		/// <param name="element">The <see cref="T:System.Windows.Controls.GridView" /> object.</param>
		/// <param name="collection">The <see cref="T:System.Windows.Controls.GridViewColumnCollection" /> object to assign.</param>
		// Token: 0x06004CB4 RID: 19636 RVA: 0x0015A94C File Offset: 0x00158B4C
		public static void SetColumnCollection(DependencyObject element, GridViewColumnCollection collection)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(GridView.ColumnCollectionProperty, collection);
		}

		/// <summary>Determines whether to serialize the <see cref="P:System.Windows.Controls.GridView.ColumnCollection" /> attached property.</summary>
		/// <param name="obj">The object on which the <see cref="P:System.Windows.Controls.GridView.ColumnCollection" /> is set.</param>
		/// <returns>
		///     <see langword="true" /> if the <see cref="P:System.Windows.Controls.GridView.ColumnCollection" /> must be serialized; otherwise, <see langword="false" />.</returns>
		// Token: 0x06004CB5 RID: 19637 RVA: 0x0015A968 File Offset: 0x00158B68
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static bool ShouldSerializeColumnCollection(DependencyObject obj)
		{
			ListViewItem listViewItem = obj as ListViewItem;
			if (listViewItem != null)
			{
				ListView listView = listViewItem.ParentSelector as ListView;
				if (listView != null)
				{
					GridView gridView = listView.View as GridView;
					if (gridView != null)
					{
						GridViewColumnCollection gridViewColumnCollection = listViewItem.ReadLocalValue(GridView.ColumnCollectionProperty) as GridViewColumnCollection;
						return gridViewColumnCollection != gridView.Columns;
					}
				}
			}
			return true;
		}

		/// <summary>Gets the collection of <see cref="T:System.Windows.Controls.GridViewColumn" /> objects that is defined for this <see cref="T:System.Windows.Controls.GridView" />.</summary>
		/// <returns>The collection of columns in the <see cref="T:System.Windows.Controls.GridView" />. The default value is <see langword="null" />.</returns>
		// Token: 0x170012B1 RID: 4785
		// (get) Token: 0x06004CB6 RID: 19638 RVA: 0x0015A9BC File Offset: 0x00158BBC
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public GridViewColumnCollection Columns
		{
			get
			{
				if (this._columns == null)
				{
					this._columns = new GridViewColumnCollection();
					this._columns.Owner = this;
					this._columns.InViewMode = true;
				}
				return this._columns;
			}
		}

		/// <summary>Gets or sets the style to apply to column headers. </summary>
		/// <returns>The <see cref="T:System.Windows.Style" /> that is used to define the display properties for column headers. The default value is <see langword="null" />.</returns>
		// Token: 0x170012B2 RID: 4786
		// (get) Token: 0x06004CB7 RID: 19639 RVA: 0x0015A9EF File Offset: 0x00158BEF
		// (set) Token: 0x06004CB8 RID: 19640 RVA: 0x0015AA01 File Offset: 0x00158C01
		public Style ColumnHeaderContainerStyle
		{
			get
			{
				return (Style)base.GetValue(GridView.ColumnHeaderContainerStyleProperty);
			}
			set
			{
				base.SetValue(GridView.ColumnHeaderContainerStyleProperty, value);
			}
		}

		/// <summary>Gets or sets a template to use to display the column headers. </summary>
		/// <returns>The <see cref="T:System.Windows.DataTemplate" /> to use to display the column headers as part of the <see cref="T:System.Windows.Controls.GridView" />. The default value is <see langword="null" />.</returns>
		// Token: 0x170012B3 RID: 4787
		// (get) Token: 0x06004CB9 RID: 19641 RVA: 0x0015AA0F File Offset: 0x00158C0F
		// (set) Token: 0x06004CBA RID: 19642 RVA: 0x0015AA21 File Offset: 0x00158C21
		public DataTemplate ColumnHeaderTemplate
		{
			get
			{
				return (DataTemplate)base.GetValue(GridView.ColumnHeaderTemplateProperty);
			}
			set
			{
				base.SetValue(GridView.ColumnHeaderTemplateProperty, value);
			}
		}

		// Token: 0x06004CBB RID: 19643 RVA: 0x0015AA30 File Offset: 0x00158C30
		private static void OnColumnHeaderTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			GridView d2 = (GridView)d;
			Helper.CheckTemplateAndTemplateSelector("GridViewColumnHeader", GridView.ColumnHeaderTemplateProperty, GridView.ColumnHeaderTemplateSelectorProperty, d2);
		}

		/// <summary>Gets or sets the selector object that provides logic for selecting a template to use for each column header. </summary>
		/// <returns>The <see cref="T:System.Windows.Controls.DataTemplateSelector" /> object that determines the data template to use for each column header. The default value is <see langword="null" />. </returns>
		// Token: 0x170012B4 RID: 4788
		// (get) Token: 0x06004CBC RID: 19644 RVA: 0x0015AA59 File Offset: 0x00158C59
		// (set) Token: 0x06004CBD RID: 19645 RVA: 0x0015AA6B File Offset: 0x00158C6B
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public DataTemplateSelector ColumnHeaderTemplateSelector
		{
			get
			{
				return (DataTemplateSelector)base.GetValue(GridView.ColumnHeaderTemplateSelectorProperty);
			}
			set
			{
				base.SetValue(GridView.ColumnHeaderTemplateSelectorProperty, value);
			}
		}

		// Token: 0x06004CBE RID: 19646 RVA: 0x0015AA7C File Offset: 0x00158C7C
		private static void OnColumnHeaderTemplateSelectorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			GridView d2 = (GridView)d;
			Helper.CheckTemplateAndTemplateSelector("GridViewColumnHeader", GridView.ColumnHeaderTemplateProperty, GridView.ColumnHeaderTemplateSelectorProperty, d2);
		}

		/// <summary>Gets or sets a composite string that specifies how to format the column headers of the <see cref="T:System.Windows.Controls.GridView" /> if they are displayed as strings.</summary>
		/// <returns>A composite string that specifies how to format the column headers of the <see cref="T:System.Windows.Controls.GridView" /> if they are displayed as strings. The default is <see langword="null" />.</returns>
		// Token: 0x170012B5 RID: 4789
		// (get) Token: 0x06004CBF RID: 19647 RVA: 0x0015AAA5 File Offset: 0x00158CA5
		// (set) Token: 0x06004CC0 RID: 19648 RVA: 0x0015AAB7 File Offset: 0x00158CB7
		public string ColumnHeaderStringFormat
		{
			get
			{
				return (string)base.GetValue(GridView.ColumnHeaderStringFormatProperty);
			}
			set
			{
				base.SetValue(GridView.ColumnHeaderStringFormatProperty, value);
			}
		}

		/// <summary>Gets or sets whether columns in a <see cref="T:System.Windows.Controls.GridView" /> can be reordered by a drag-and-drop operation. </summary>
		/// <returns>
		///     <see langword="true" /> if columns can be reordered; otherwise, <see langword="false" />. The default value is <see langword="true" />.</returns>
		// Token: 0x170012B6 RID: 4790
		// (get) Token: 0x06004CC1 RID: 19649 RVA: 0x0015AAC5 File Offset: 0x00158CC5
		// (set) Token: 0x06004CC2 RID: 19650 RVA: 0x0015AAD7 File Offset: 0x00158CD7
		public bool AllowsColumnReorder
		{
			get
			{
				return (bool)base.GetValue(GridView.AllowsColumnReorderProperty);
			}
			set
			{
				base.SetValue(GridView.AllowsColumnReorderProperty, value);
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Windows.Controls.ContextMenu" /> for the <see cref="T:System.Windows.Controls.GridView" />. </summary>
		/// <returns>The <see cref="T:System.Windows.Controls.ContextMenu" /> for the column headers in a <see cref="T:System.Windows.Controls.GridView" />. The default value is <see langword="null" />.</returns>
		// Token: 0x170012B7 RID: 4791
		// (get) Token: 0x06004CC3 RID: 19651 RVA: 0x0015AAE5 File Offset: 0x00158CE5
		// (set) Token: 0x06004CC4 RID: 19652 RVA: 0x0015AAF7 File Offset: 0x00158CF7
		public ContextMenu ColumnHeaderContextMenu
		{
			get
			{
				return (ContextMenu)base.GetValue(GridView.ColumnHeaderContextMenuProperty);
			}
			set
			{
				base.SetValue(GridView.ColumnHeaderContextMenuProperty, value);
			}
		}

		/// <summary>Gets or sets the content of a tooltip that appears when the mouse pointer pauses over one of the column headers. </summary>
		/// <returns>An object that represents the content that appears as a tooltip when the mouse pointer is paused over one of the column headers. The default value is not defined.</returns>
		// Token: 0x170012B8 RID: 4792
		// (get) Token: 0x06004CC5 RID: 19653 RVA: 0x0015AB05 File Offset: 0x00158D05
		// (set) Token: 0x06004CC6 RID: 19654 RVA: 0x0015AB12 File Offset: 0x00158D12
		public object ColumnHeaderToolTip
		{
			get
			{
				return base.GetValue(GridView.ColumnHeaderToolTipProperty);
			}
			set
			{
				base.SetValue(GridView.ColumnHeaderToolTipProperty, value);
			}
		}

		/// <summary>Prepares a <see cref="T:System.Windows.Controls.ListViewItem" /> for display according to the definition of this <see cref="T:System.Windows.Controls.GridView" /> object.</summary>
		/// <param name="item">The <see cref="T:System.Windows.Controls.ListViewItem" /> to display.</param>
		// Token: 0x06004CC7 RID: 19655 RVA: 0x0015AB20 File Offset: 0x00158D20
		protected internal override void PrepareItem(ListViewItem item)
		{
			base.PrepareItem(item);
			GridView.SetColumnCollection(item, this._columns);
		}

		/// <summary>Removes all settings, bindings, and styling from a <see cref="T:System.Windows.Controls.ListViewItem" />.</summary>
		/// <param name="item">The <see cref="T:System.Windows.Controls.ListViewItem" /> to clear.</param>
		// Token: 0x06004CC8 RID: 19656 RVA: 0x0015AB35 File Offset: 0x00158D35
		protected internal override void ClearItem(ListViewItem item)
		{
			item.ClearValue(GridView.ColumnCollectionProperty);
			base.ClearItem(item);
		}

		/// <summary>Gets the reference for the default style for the <see cref="T:System.Windows.Controls.GridView" />.</summary>
		/// <returns>The <see cref="P:System.Windows.Controls.GridView.GridViewStyleKey" />. The default value is the <see cref="P:System.Windows.Controls.GridView.GridViewStyleKey" /> in the current theme.</returns>
		// Token: 0x170012B9 RID: 4793
		// (get) Token: 0x06004CC9 RID: 19657 RVA: 0x0015AB49 File Offset: 0x00158D49
		protected internal override object DefaultStyleKey
		{
			get
			{
				return GridView.GridViewStyleKey;
			}
		}

		/// <summary>Gets the reference to the default style for the container of the data items in the <see cref="T:System.Windows.Controls.GridView" />.</summary>
		/// <returns>The <see cref="P:System.Windows.Controls.GridView.GridViewItemContainerStyleKey" />. The default value is the <see cref="P:System.Windows.Controls.GridView.GridViewItemContainerStyleKey" /> in the current theme.</returns>
		// Token: 0x170012BA RID: 4794
		// (get) Token: 0x06004CCA RID: 19658 RVA: 0x0015AB50 File Offset: 0x00158D50
		protected internal override object ItemContainerDefaultStyleKey
		{
			get
			{
				return GridView.GridViewItemContainerStyleKey;
			}
		}

		// Token: 0x06004CCB RID: 19659 RVA: 0x0015AB58 File Offset: 0x00158D58
		internal override void OnInheritanceContextChangedCore(EventArgs args)
		{
			base.OnInheritanceContextChangedCore(args);
			if (this._columns != null)
			{
				foreach (GridViewColumn gridViewColumn in this._columns)
				{
					gridViewColumn.OnInheritanceContextChanged(args);
				}
			}
		}

		// Token: 0x06004CCC RID: 19660 RVA: 0x0015ABB4 File Offset: 0x00158DB4
		internal override void OnThemeChanged()
		{
			if (this._columns != null)
			{
				for (int i = 0; i < this._columns.Count; i++)
				{
					this._columns[i].OnThemeChanged();
				}
			}
		}

		// Token: 0x170012BB RID: 4795
		// (get) Token: 0x06004CCD RID: 19661 RVA: 0x0015ABF0 File Offset: 0x00158DF0
		// (set) Token: 0x06004CCE RID: 19662 RVA: 0x0015ABF8 File Offset: 0x00158DF8
		internal GridViewHeaderRowPresenter HeaderRowPresenter
		{
			get
			{
				return this._gvheaderRP;
			}
			set
			{
				this._gvheaderRP = value;
			}
		}

		/// <summary>Identifies the <see cref="F:System.Windows.Controls.GridView.ColumnCollectionProperty" /> attachedproperty. </summary>
		// Token: 0x04002B2A RID: 11050
		public static readonly DependencyProperty ColumnCollectionProperty = DependencyProperty.RegisterAttached("ColumnCollection", typeof(GridViewColumnCollection), typeof(GridView));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.GridView.ColumnHeaderContainerStyle" /> dependency property. </summary>
		// Token: 0x04002B2B RID: 11051
		public static readonly DependencyProperty ColumnHeaderContainerStyleProperty = DependencyProperty.Register("ColumnHeaderContainerStyle", typeof(Style), typeof(GridView));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.GridView.ColumnHeaderTemplate" /> dependency property. </summary>
		// Token: 0x04002B2C RID: 11052
		public static readonly DependencyProperty ColumnHeaderTemplateProperty = DependencyProperty.Register("ColumnHeaderTemplate", typeof(DataTemplate), typeof(GridView), new FrameworkPropertyMetadata(new PropertyChangedCallback(GridView.OnColumnHeaderTemplateChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.GridView.ColumnHeaderTemplateSelector" /> dependency property. </summary>
		// Token: 0x04002B2D RID: 11053
		public static readonly DependencyProperty ColumnHeaderTemplateSelectorProperty = DependencyProperty.Register("ColumnHeaderTemplateSelector", typeof(DataTemplateSelector), typeof(GridView), new FrameworkPropertyMetadata(new PropertyChangedCallback(GridView.OnColumnHeaderTemplateSelectorChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.GridView.ColumnHeaderStringFormat" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.GridView.ColumnHeaderStringFormat" /> dependency property.</returns>
		// Token: 0x04002B2E RID: 11054
		public static readonly DependencyProperty ColumnHeaderStringFormatProperty = DependencyProperty.Register("ColumnHeaderStringFormat", typeof(string), typeof(GridView));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.GridView.AllowsColumnReorder" /> dependency property. </summary>
		// Token: 0x04002B2F RID: 11055
		public static readonly DependencyProperty AllowsColumnReorderProperty = DependencyProperty.Register("AllowsColumnReorder", typeof(bool), typeof(GridView), new FrameworkPropertyMetadata(BooleanBoxes.TrueBox));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.GridView.ColumnHeaderContextMenu" /> dependency property. </summary>
		// Token: 0x04002B30 RID: 11056
		public static readonly DependencyProperty ColumnHeaderContextMenuProperty = DependencyProperty.Register("ColumnHeaderContextMenu", typeof(ContextMenu), typeof(GridView));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.GridView.ColumnHeaderToolTip" /> dependency property. </summary>
		// Token: 0x04002B31 RID: 11057
		public static readonly DependencyProperty ColumnHeaderToolTipProperty = DependencyProperty.Register("ColumnHeaderToolTip", typeof(object), typeof(GridView));

		// Token: 0x04002B32 RID: 11058
		private GridViewColumnCollection _columns;

		// Token: 0x04002B33 RID: 11059
		private GridViewHeaderRowPresenter _gvheaderRP;
	}
}
