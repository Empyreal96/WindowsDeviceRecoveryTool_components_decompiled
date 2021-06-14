using System;
using System.Windows.Data;

namespace System.Windows.Controls
{
	/// <summary>Represents a <see cref="T:System.Windows.Controls.DataGrid" /> column that hosts template-specified content in its cells.</summary>
	// Token: 0x020004BE RID: 1214
	public class DataGridTemplateColumn : DataGridColumn
	{
		// Token: 0x060049A4 RID: 18852 RVA: 0x0014D33C File Offset: 0x0014B53C
		static DataGridTemplateColumn()
		{
			DataGridColumn.CanUserSortProperty.OverrideMetadata(typeof(DataGridTemplateColumn), new FrameworkPropertyMetadata(null, new CoerceValueCallback(DataGridTemplateColumn.OnCoerceTemplateColumnCanUserSort)));
			DataGridColumn.SortMemberPathProperty.OverrideMetadata(typeof(DataGridTemplateColumn), new FrameworkPropertyMetadata(new PropertyChangedCallback(DataGridTemplateColumn.OnTemplateColumnSortMemberPathChanged)));
		}

		// Token: 0x060049A5 RID: 18853 RVA: 0x0014D468 File Offset: 0x0014B668
		private static void OnTemplateColumnSortMemberPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DataGridTemplateColumn dataGridTemplateColumn = (DataGridTemplateColumn)d;
			dataGridTemplateColumn.CoerceValue(DataGridColumn.CanUserSortProperty);
		}

		// Token: 0x060049A6 RID: 18854 RVA: 0x0014D488 File Offset: 0x0014B688
		private static object OnCoerceTemplateColumnCanUserSort(DependencyObject d, object baseValue)
		{
			DataGridTemplateColumn dataGridTemplateColumn = (DataGridTemplateColumn)d;
			if (string.IsNullOrEmpty(dataGridTemplateColumn.SortMemberPath))
			{
				return false;
			}
			return DataGridColumn.OnCoerceCanUserSort(d, baseValue);
		}

		/// <summary>Gets or sets the template to use to display the contents of a cell that is not in editing mode.</summary>
		/// <returns>The template to use to display the contents of a cell that is not in editing mode. The registered default is <see langword="null" />. For information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x170011FC RID: 4604
		// (get) Token: 0x060049A7 RID: 18855 RVA: 0x0014D4B7 File Offset: 0x0014B6B7
		// (set) Token: 0x060049A8 RID: 18856 RVA: 0x0014D4C9 File Offset: 0x0014B6C9
		public DataTemplate CellTemplate
		{
			get
			{
				return (DataTemplate)base.GetValue(DataGridTemplateColumn.CellTemplateProperty);
			}
			set
			{
				base.SetValue(DataGridTemplateColumn.CellTemplateProperty, value);
			}
		}

		/// <summary>Gets or sets the object that determines which template to use to display the contents of a cell that is not in editing mode. </summary>
		/// <returns>The object that determines which template to use. The registered default is <see langword="null" />. For information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x170011FD RID: 4605
		// (get) Token: 0x060049A9 RID: 18857 RVA: 0x0014D4D7 File Offset: 0x0014B6D7
		// (set) Token: 0x060049AA RID: 18858 RVA: 0x0014D4E9 File Offset: 0x0014B6E9
		public DataTemplateSelector CellTemplateSelector
		{
			get
			{
				return (DataTemplateSelector)base.GetValue(DataGridTemplateColumn.CellTemplateSelectorProperty);
			}
			set
			{
				base.SetValue(DataGridTemplateColumn.CellTemplateSelectorProperty, value);
			}
		}

		/// <summary>Gets or sets the template to use to display the contents of a cell that is in editing mode.</summary>
		/// <returns>The template that is used to display the contents of a cell that is in editing mode. The registered default is <see langword="null" />. For information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x170011FE RID: 4606
		// (get) Token: 0x060049AB RID: 18859 RVA: 0x0014D4F7 File Offset: 0x0014B6F7
		// (set) Token: 0x060049AC RID: 18860 RVA: 0x0014D509 File Offset: 0x0014B709
		public DataTemplate CellEditingTemplate
		{
			get
			{
				return (DataTemplate)base.GetValue(DataGridTemplateColumn.CellEditingTemplateProperty);
			}
			set
			{
				base.SetValue(DataGridTemplateColumn.CellEditingTemplateProperty, value);
			}
		}

		/// <summary>Gets or sets the object that determines which template to use to display the contents of a cell that is in editing mode.</summary>
		/// <returns>The object that determines which template to use to display the contents of a cell that is in editing mode. The registered default is <see langword="null" />. For information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x170011FF RID: 4607
		// (get) Token: 0x060049AD RID: 18861 RVA: 0x0014D517 File Offset: 0x0014B717
		// (set) Token: 0x060049AE RID: 18862 RVA: 0x0014D529 File Offset: 0x0014B729
		public DataTemplateSelector CellEditingTemplateSelector
		{
			get
			{
				return (DataTemplateSelector)base.GetValue(DataGridTemplateColumn.CellEditingTemplateSelectorProperty);
			}
			set
			{
				base.SetValue(DataGridTemplateColumn.CellEditingTemplateSelectorProperty, value);
			}
		}

		// Token: 0x060049AF RID: 18863 RVA: 0x0014D537 File Offset: 0x0014B737
		private void ChooseCellTemplateAndSelector(bool isEditing, out DataTemplate template, out DataTemplateSelector templateSelector)
		{
			template = null;
			templateSelector = null;
			if (isEditing)
			{
				template = this.CellEditingTemplate;
				templateSelector = this.CellEditingTemplateSelector;
			}
			if (template == null && templateSelector == null)
			{
				template = this.CellTemplate;
				templateSelector = this.CellTemplateSelector;
			}
		}

		// Token: 0x060049B0 RID: 18864 RVA: 0x0014D56C File Offset: 0x0014B76C
		private FrameworkElement LoadTemplateContent(bool isEditing, object dataItem, DataGridCell cell)
		{
			DataTemplate dataTemplate;
			DataTemplateSelector dataTemplateSelector;
			this.ChooseCellTemplateAndSelector(isEditing, out dataTemplate, out dataTemplateSelector);
			if (dataTemplate != null || dataTemplateSelector != null)
			{
				ContentPresenter contentPresenter = new ContentPresenter();
				BindingOperations.SetBinding(contentPresenter, ContentPresenter.ContentProperty, new Binding());
				contentPresenter.ContentTemplate = dataTemplate;
				contentPresenter.ContentTemplateSelector = dataTemplateSelector;
				return contentPresenter;
			}
			return null;
		}

		/// <summary>Gets an element defined by the <see cref="P:System.Windows.Controls.DataGridTemplateColumn.CellTemplate" /> that is bound to the column's <see cref="P:System.Windows.Controls.DataGridBoundColumn.Binding" /> property value.</summary>
		/// <param name="cell">The cell that will contain the generated element.</param>
		/// <param name="dataItem">The data item represented by the row that contains the intended cell.</param>
		/// <returns>A new, read-only element that is bound to the column's <see cref="P:System.Windows.Controls.DataGridBoundColumn.Binding" /> property value.</returns>
		// Token: 0x060049B1 RID: 18865 RVA: 0x0014D5B2 File Offset: 0x0014B7B2
		protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
		{
			return this.LoadTemplateContent(false, dataItem, cell);
		}

		/// <summary>Gets an element defined by the <see cref="P:System.Windows.Controls.DataGridTemplateColumn.CellEditingTemplate" /> that is bound to the column's <see cref="P:System.Windows.Controls.DataGridBoundColumn.Binding" /> property value.</summary>
		/// <param name="cell">The cell that will contain the generated element.</param>
		/// <param name="dataItem">The data item represented by the row that contains the intended cell.</param>
		/// <returns>A new editing element that is bound to the column's <see cref="P:System.Windows.Controls.DataGridBoundColumn.Binding" /> property value.</returns>
		// Token: 0x060049B2 RID: 18866 RVA: 0x0014D5BD File Offset: 0x0014B7BD
		protected override FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem)
		{
			return this.LoadTemplateContent(true, dataItem, cell);
		}

		/// <summary>Refreshes the contents of a cell in the column in response to a template property value change.</summary>
		/// <param name="element">The cell to update.</param>
		/// <param name="propertyName">The name of the column property that has changed.</param>
		// Token: 0x060049B3 RID: 18867 RVA: 0x0014D5C8 File Offset: 0x0014B7C8
		protected internal override void RefreshCellContent(FrameworkElement element, string propertyName)
		{
			DataGridCell dataGridCell = element as DataGridCell;
			if (dataGridCell != null)
			{
				bool isEditing = dataGridCell.IsEditing;
				if ((!isEditing && (string.Compare(propertyName, "CellTemplate", StringComparison.Ordinal) == 0 || string.Compare(propertyName, "CellTemplateSelector", StringComparison.Ordinal) == 0)) || (isEditing && (string.Compare(propertyName, "CellEditingTemplate", StringComparison.Ordinal) == 0 || string.Compare(propertyName, "CellEditingTemplateSelector", StringComparison.Ordinal) == 0)))
				{
					dataGridCell.BuildVisualTree();
					return;
				}
			}
			base.RefreshCellContent(element, propertyName);
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGridTemplateColumn.CellTemplate" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGridTemplateColumn.CellTemplate" /> dependency property.</returns>
		// Token: 0x04002A25 RID: 10789
		public static readonly DependencyProperty CellTemplateProperty = DependencyProperty.Register("CellTemplate", typeof(DataTemplate), typeof(DataGridTemplateColumn), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGridColumn.NotifyPropertyChangeForRefreshContent)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGridTemplateColumn.CellEditingTemplateSelector" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGridTemplateColumn.CellEditingTemplateSelector" /> dependency property.</returns>
		// Token: 0x04002A26 RID: 10790
		public static readonly DependencyProperty CellTemplateSelectorProperty = DependencyProperty.Register("CellTemplateSelector", typeof(DataTemplateSelector), typeof(DataGridTemplateColumn), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGridColumn.NotifyPropertyChangeForRefreshContent)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGridTemplateColumn.CellEditingTemplate" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGridTemplateColumn.CellEditingTemplate" /> dependency property.</returns>
		// Token: 0x04002A27 RID: 10791
		public static readonly DependencyProperty CellEditingTemplateProperty = DependencyProperty.Register("CellEditingTemplate", typeof(DataTemplate), typeof(DataGridTemplateColumn), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGridColumn.NotifyPropertyChangeForRefreshContent)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGridTemplateColumn.CellEditingTemplateSelector" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGridTemplateColumn.CellEditingTemplateSelector" /> dependency property.</returns>
		// Token: 0x04002A28 RID: 10792
		public static readonly DependencyProperty CellEditingTemplateSelectorProperty = DependencyProperty.Register("CellEditingTemplateSelector", typeof(DataTemplateSelector), typeof(DataGridTemplateColumn), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGridColumn.NotifyPropertyChangeForRefreshContent)));
	}
}
