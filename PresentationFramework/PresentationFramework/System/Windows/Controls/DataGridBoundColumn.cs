using System;
using System.Windows.Data;

namespace System.Windows.Controls
{
	/// <summary>Serves as the base class for columns that can bind to a property in the data source of a <see cref="T:System.Windows.Controls.DataGrid" />.</summary>
	// Token: 0x02000495 RID: 1173
	public abstract class DataGridBoundColumn : DataGridColumn
	{
		// Token: 0x060046D1 RID: 18129 RVA: 0x001415C4 File Offset: 0x0013F7C4
		static DataGridBoundColumn()
		{
			DataGridColumn.SortMemberPathProperty.OverrideMetadata(typeof(DataGridBoundColumn), new FrameworkPropertyMetadata(null, new CoerceValueCallback(DataGridBoundColumn.OnCoerceSortMemberPath)));
		}

		// Token: 0x060046D2 RID: 18130 RVA: 0x00141664 File Offset: 0x0013F864
		private static object OnCoerceSortMemberPath(DependencyObject d, object baseValue)
		{
			DataGridBoundColumn dataGridBoundColumn = (DataGridBoundColumn)d;
			string text = (string)baseValue;
			if (string.IsNullOrEmpty(text))
			{
				string pathFromBinding = DataGridHelper.GetPathFromBinding(dataGridBoundColumn.Binding as Binding);
				if (!string.IsNullOrEmpty(pathFromBinding))
				{
					text = pathFromBinding;
				}
			}
			return text;
		}

		/// <summary>Gets or sets the binding that associates the column with a property in the data source.</summary>
		/// <returns>The object that represents the data binding for the column. The default is <see langword="null" />.</returns>
		// Token: 0x17001157 RID: 4439
		// (get) Token: 0x060046D3 RID: 18131 RVA: 0x001416A3 File Offset: 0x0013F8A3
		// (set) Token: 0x060046D4 RID: 18132 RVA: 0x001416AC File Offset: 0x0013F8AC
		public virtual BindingBase Binding
		{
			get
			{
				return this._binding;
			}
			set
			{
				if (this._binding != value)
				{
					BindingBase binding = this._binding;
					this._binding = value;
					base.CoerceValue(DataGridColumn.IsReadOnlyProperty);
					base.CoerceValue(DataGridColumn.SortMemberPathProperty);
					this.OnBindingChanged(binding, this._binding);
				}
			}
		}

		/// <summary>Determines the value of the <see cref="P:System.Windows.Controls.DataGridColumn.IsReadOnly" /> property based on property rules from the data grid that contains this column.</summary>
		/// <param name="baseValue">The value that was passed to the delegate.</param>
		/// <returns>
		///     <see langword="true" /> if cells in the column cannot be edited based on rules from the data grid; otherwise, <see langword="false" />.</returns>
		// Token: 0x060046D5 RID: 18133 RVA: 0x001416F3 File Offset: 0x0013F8F3
		protected override bool OnCoerceIsReadOnly(bool baseValue)
		{
			return DataGridHelper.IsOneWay(this._binding) || base.OnCoerceIsReadOnly(baseValue);
		}

		/// <summary>Notifies the <see cref="T:System.Windows.Controls.DataGrid" /> when the value of the <see cref="P:System.Windows.Controls.DataGridBoundColumn.Binding" /> property changes.</summary>
		/// <param name="oldBinding">The previous binding.</param>
		/// <param name="newBinding">The binding that the column has been changed to.</param>
		// Token: 0x060046D6 RID: 18134 RVA: 0x0014170B File Offset: 0x0013F90B
		protected virtual void OnBindingChanged(BindingBase oldBinding, BindingBase newBinding)
		{
			base.NotifyPropertyChanged("Binding");
		}

		// Token: 0x060046D7 RID: 18135 RVA: 0x00141718 File Offset: 0x0013F918
		internal void ApplyBinding(DependencyObject target, DependencyProperty property)
		{
			BindingBase binding = this.Binding;
			if (binding != null)
			{
				BindingOperations.SetBinding(target, property, binding);
				return;
			}
			BindingOperations.ClearBinding(target, property);
		}

		/// <summary>Gets or sets the style that is used when rendering the element that the column displays for a cell that is not in editing mode.</summary>
		/// <returns>The style that is used when rendering a display-only element. The registered default is <see langword="null" />. For information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x17001158 RID: 4440
		// (get) Token: 0x060046D8 RID: 18136 RVA: 0x00141740 File Offset: 0x0013F940
		// (set) Token: 0x060046D9 RID: 18137 RVA: 0x00141752 File Offset: 0x0013F952
		public Style ElementStyle
		{
			get
			{
				return (Style)base.GetValue(DataGridBoundColumn.ElementStyleProperty);
			}
			set
			{
				base.SetValue(DataGridBoundColumn.ElementStyleProperty, value);
			}
		}

		/// <summary>Gets or sets the style that is used when rendering the element that the column displays for a cell in editing mode.</summary>
		/// <returns>The style that is used when rendering an editing element. The registered default is <see langword="null" />. For information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x17001159 RID: 4441
		// (get) Token: 0x060046DA RID: 18138 RVA: 0x00141760 File Offset: 0x0013F960
		// (set) Token: 0x060046DB RID: 18139 RVA: 0x00141772 File Offset: 0x0013F972
		public Style EditingElementStyle
		{
			get
			{
				return (Style)base.GetValue(DataGridBoundColumn.EditingElementStyleProperty);
			}
			set
			{
				base.SetValue(DataGridBoundColumn.EditingElementStyleProperty, value);
			}
		}

		// Token: 0x060046DC RID: 18140 RVA: 0x00141780 File Offset: 0x0013F980
		internal void ApplyStyle(bool isEditing, bool defaultToElementStyle, FrameworkElement element)
		{
			Style style = this.PickStyle(isEditing, defaultToElementStyle);
			if (style != null)
			{
				element.Style = style;
			}
		}

		// Token: 0x060046DD RID: 18141 RVA: 0x001417A0 File Offset: 0x0013F9A0
		private Style PickStyle(bool isEditing, bool defaultToElementStyle)
		{
			Style style = isEditing ? this.EditingElementStyle : this.ElementStyle;
			if (isEditing && defaultToElementStyle && style == null)
			{
				style = this.ElementStyle;
			}
			return style;
		}

		/// <summary>Gets or sets the binding object to use when getting or setting cell content for the clipboard.</summary>
		/// <returns>An object that represents the binding.</returns>
		// Token: 0x1700115A RID: 4442
		// (get) Token: 0x060046DE RID: 18142 RVA: 0x001417CF File Offset: 0x0013F9CF
		// (set) Token: 0x060046DF RID: 18143 RVA: 0x001417E1 File Offset: 0x0013F9E1
		public override BindingBase ClipboardContentBinding
		{
			get
			{
				return base.ClipboardContentBinding ?? this.Binding;
			}
			set
			{
				base.ClipboardContentBinding = value;
			}
		}

		/// <summary>Rebuilds the contents of a cell in the column in response to a binding change.</summary>
		/// <param name="element">The cell to update.</param>
		/// <param name="propertyName">The name of the column property that has changed.</param>
		// Token: 0x060046E0 RID: 18144 RVA: 0x001417EC File Offset: 0x0013F9EC
		protected internal override void RefreshCellContent(FrameworkElement element, string propertyName)
		{
			DataGridCell dataGridCell = element as DataGridCell;
			if (dataGridCell != null)
			{
				bool isEditing = dataGridCell.IsEditing;
				if (string.Compare(propertyName, "Binding", StringComparison.Ordinal) == 0 || (string.Compare(propertyName, "ElementStyle", StringComparison.Ordinal) == 0 && !isEditing) || (string.Compare(propertyName, "EditingElementStyle", StringComparison.Ordinal) == 0 && isEditing))
				{
					dataGridCell.BuildVisualTree();
					return;
				}
			}
			base.RefreshCellContent(element, propertyName);
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGridBoundColumn.ElementStyle" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGridBoundColumn.ElementStyle" /> dependency property.</returns>
		// Token: 0x0400293D RID: 10557
		public static readonly DependencyProperty ElementStyleProperty = DependencyProperty.Register("ElementStyle", typeof(Style), typeof(DataGridBoundColumn), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGridColumn.NotifyPropertyChangeForRefreshContent)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGridBoundColumn.EditingElementStyle" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGridBoundColumn.EditingElementStyle" /> dependency property.</returns>
		// Token: 0x0400293E RID: 10558
		public static readonly DependencyProperty EditingElementStyleProperty = DependencyProperty.Register("EditingElementStyle", typeof(Style), typeof(DataGridBoundColumn), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGridColumn.NotifyPropertyChangeForRefreshContent)));

		// Token: 0x0400293F RID: 10559
		private BindingBase _binding;
	}
}
