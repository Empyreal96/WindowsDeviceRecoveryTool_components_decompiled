using System;
using System.Collections;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;

namespace System.Windows.Controls
{
	/// <summary>Represents a <see cref="T:System.Windows.Controls.DataGrid" /> column that hosts <see cref="T:System.Windows.Controls.ComboBox" /> controls in its cells.</summary>
	// Token: 0x020004A6 RID: 1190
	public class DataGridComboBoxColumn : DataGridColumn
	{
		// Token: 0x06004898 RID: 18584 RVA: 0x0014A0A0 File Offset: 0x001482A0
		static DataGridComboBoxColumn()
		{
			DataGridColumn.SortMemberPathProperty.OverrideMetadata(typeof(DataGridComboBoxColumn), new FrameworkPropertyMetadata(null, new CoerceValueCallback(DataGridComboBoxColumn.OnCoerceSortMemberPath)));
		}

		/// <summary>Gets the resource key for the style to apply to a read-only combo box.</summary>
		/// <returns>The key for the style.</returns>
		// Token: 0x170011B7 RID: 4535
		// (get) Token: 0x06004899 RID: 18585 RVA: 0x0014A1A2 File Offset: 0x001483A2
		public static ComponentResourceKey TextBlockComboBoxStyleKey
		{
			get
			{
				return SystemResourceKey.DataGridComboBoxColumnTextBlockComboBoxStyleKey;
			}
		}

		// Token: 0x0600489A RID: 18586 RVA: 0x0014A1AC File Offset: 0x001483AC
		private static object OnCoerceSortMemberPath(DependencyObject d, object baseValue)
		{
			DataGridComboBoxColumn dataGridComboBoxColumn = (DataGridComboBoxColumn)d;
			string text = (string)baseValue;
			if (string.IsNullOrEmpty(text))
			{
				string pathFromBinding = DataGridHelper.GetPathFromBinding(dataGridComboBoxColumn.EffectiveBinding as Binding);
				if (!string.IsNullOrEmpty(pathFromBinding))
				{
					text = pathFromBinding;
				}
			}
			return text;
		}

		// Token: 0x170011B8 RID: 4536
		// (get) Token: 0x0600489B RID: 18587 RVA: 0x0014A1EB File Offset: 0x001483EB
		private BindingBase EffectiveBinding
		{
			get
			{
				if (this.SelectedItemBinding != null)
				{
					return this.SelectedItemBinding;
				}
				if (this.SelectedValueBinding != null)
				{
					return this.SelectedValueBinding;
				}
				return this.TextBinding;
			}
		}

		/// <summary>Gets or sets the value of the selected item, obtained by using <see cref="P:System.Windows.Controls.DataGridComboBoxColumn.SelectedValuePath" />.</summary>
		/// <returns>The binding for the selected value.</returns>
		// Token: 0x170011B9 RID: 4537
		// (get) Token: 0x0600489C RID: 18588 RVA: 0x0014A211 File Offset: 0x00148411
		// (set) Token: 0x0600489D RID: 18589 RVA: 0x0014A21C File Offset: 0x0014841C
		public virtual BindingBase SelectedValueBinding
		{
			get
			{
				return this._selectedValueBinding;
			}
			set
			{
				if (this._selectedValueBinding != value)
				{
					BindingBase selectedValueBinding = this._selectedValueBinding;
					this._selectedValueBinding = value;
					base.CoerceValue(DataGridColumn.IsReadOnlyProperty);
					base.CoerceValue(DataGridColumn.SortMemberPathProperty);
					this.OnSelectedValueBindingChanged(selectedValueBinding, this._selectedValueBinding);
				}
			}
		}

		/// <summary>Determines the value of the <see cref="P:System.Windows.Controls.DataGridColumn.IsReadOnly" /> property based on property rules from the <see cref="T:System.Windows.Controls.DataGrid" /> that contains this column.</summary>
		/// <param name="baseValue">The value that was passed to the delegate.</param>
		/// <returns>
		///     <see langword="true" /> if the combo boxes in the column cannot be edited; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600489E RID: 18590 RVA: 0x0014A263 File Offset: 0x00148463
		protected override bool OnCoerceIsReadOnly(bool baseValue)
		{
			return DataGridHelper.IsOneWay(this.EffectiveBinding) || base.OnCoerceIsReadOnly(baseValue);
		}

		/// <summary>Gets or sets the binding for the currently selected item.</summary>
		/// <returns>The binding for the selected item.</returns>
		// Token: 0x170011BA RID: 4538
		// (get) Token: 0x0600489F RID: 18591 RVA: 0x0014A27B File Offset: 0x0014847B
		// (set) Token: 0x060048A0 RID: 18592 RVA: 0x0014A284 File Offset: 0x00148484
		public virtual BindingBase SelectedItemBinding
		{
			get
			{
				return this._selectedItemBinding;
			}
			set
			{
				if (this._selectedItemBinding != value)
				{
					BindingBase selectedItemBinding = this._selectedItemBinding;
					this._selectedItemBinding = value;
					base.CoerceValue(DataGridColumn.IsReadOnlyProperty);
					base.CoerceValue(DataGridColumn.SortMemberPathProperty);
					this.OnSelectedItemBindingChanged(selectedItemBinding, this._selectedItemBinding);
				}
			}
		}

		/// <summary>Gets or sets the binding for the text in the text box portion of the <see cref="T:System.Windows.Controls.ComboBox" /> control.</summary>
		/// <returns>The binding for the text of the currently selected item.</returns>
		// Token: 0x170011BB RID: 4539
		// (get) Token: 0x060048A1 RID: 18593 RVA: 0x0014A2CB File Offset: 0x001484CB
		// (set) Token: 0x060048A2 RID: 18594 RVA: 0x0014A2D4 File Offset: 0x001484D4
		public virtual BindingBase TextBinding
		{
			get
			{
				return this._textBinding;
			}
			set
			{
				if (this._textBinding != value)
				{
					BindingBase textBinding = this._textBinding;
					this._textBinding = value;
					base.CoerceValue(DataGridColumn.IsReadOnlyProperty);
					base.CoerceValue(DataGridColumn.SortMemberPathProperty);
					this.OnTextBindingChanged(textBinding, this._textBinding);
				}
			}
		}

		/// <summary>Notifies the <see cref="T:System.Windows.Controls.DataGrid" /> when the <see cref="P:System.Windows.Controls.DataGridComboBoxColumn.SelectedValueBinding" /> property changes.</summary>
		/// <param name="oldBinding">The previous binding.</param>
		/// <param name="newBinding">The binding that the column has been changed to.</param>
		// Token: 0x060048A3 RID: 18595 RVA: 0x0014A31B File Offset: 0x0014851B
		protected virtual void OnSelectedValueBindingChanged(BindingBase oldBinding, BindingBase newBinding)
		{
			base.NotifyPropertyChanged("SelectedValueBinding");
		}

		/// <summary>Notifies the <see cref="T:System.Windows.Controls.DataGrid" /> when the <see cref="P:System.Windows.Controls.DataGridComboBoxColumn.SelectedItemBinding" /> property changes.</summary>
		/// <param name="oldBinding">The previous binding.</param>
		/// <param name="newBinding">The binding that the column has been changed to.</param>
		// Token: 0x060048A4 RID: 18596 RVA: 0x0014A328 File Offset: 0x00148528
		protected virtual void OnSelectedItemBindingChanged(BindingBase oldBinding, BindingBase newBinding)
		{
			base.NotifyPropertyChanged("SelectedItemBinding");
		}

		/// <summary>Notifies the <see cref="T:System.Windows.Controls.DataGrid" /> when the <see cref="P:System.Windows.Controls.DataGridComboBoxColumn.TextBinding" /> property changes.</summary>
		/// <param name="oldBinding">The previous binding.</param>
		/// <param name="newBinding">The binding that the column has been changed to.</param>
		// Token: 0x060048A5 RID: 18597 RVA: 0x0014A335 File Offset: 0x00148535
		protected virtual void OnTextBindingChanged(BindingBase oldBinding, BindingBase newBinding)
		{
			base.NotifyPropertyChanged("TextBinding");
		}

		/// <summary>Gets the default value of the <see cref="P:System.Windows.Controls.DataGridComboBoxColumn.ElementStyle" />.</summary>
		/// <returns>The default value of the <see cref="P:System.Windows.Controls.DataGridComboBoxColumn.ElementStyle" />.</returns>
		// Token: 0x170011BC RID: 4540
		// (get) Token: 0x060048A6 RID: 18598 RVA: 0x0014A344 File Offset: 0x00148544
		public static Style DefaultElementStyle
		{
			get
			{
				if (DataGridComboBoxColumn._defaultElementStyle == null)
				{
					Style style = new Style(typeof(ComboBox));
					style.Setters.Add(new Setter(Selector.IsSynchronizedWithCurrentItemProperty, false));
					style.Seal();
					DataGridComboBoxColumn._defaultElementStyle = style;
				}
				return DataGridComboBoxColumn._defaultElementStyle;
			}
		}

		/// <summary>Gets the default value of the <see cref="P:System.Windows.Controls.DataGridComboBoxColumn.EditingElementStyle" /> property.</summary>
		/// <returns>The default value of <see cref="P:System.Windows.Controls.DataGridComboBoxColumn.EditingElementStyle" />.</returns>
		// Token: 0x170011BD RID: 4541
		// (get) Token: 0x060048A7 RID: 18599 RVA: 0x0014A394 File Offset: 0x00148594
		public static Style DefaultEditingElementStyle
		{
			get
			{
				return DataGridComboBoxColumn.DefaultElementStyle;
			}
		}

		/// <summary>Gets or sets the style that is used when rendering the element that the column displays for a cell that is not in editing mode.</summary>
		/// <returns>The style that is used when rendering a display-only element. The registered default is <see langword="null" />. For information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x170011BE RID: 4542
		// (get) Token: 0x060048A8 RID: 18600 RVA: 0x0014A39B File Offset: 0x0014859B
		// (set) Token: 0x060048A9 RID: 18601 RVA: 0x0014A3AD File Offset: 0x001485AD
		public Style ElementStyle
		{
			get
			{
				return (Style)base.GetValue(DataGridComboBoxColumn.ElementStyleProperty);
			}
			set
			{
				base.SetValue(DataGridComboBoxColumn.ElementStyleProperty, value);
			}
		}

		/// <summary>Gets or sets the style that is used when rendering the element that the column displays for a cell in editing mode.</summary>
		/// <returns>The style that is used when rendering an editing element. The registered default is <see langword="null" />. For information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x170011BF RID: 4543
		// (get) Token: 0x060048AA RID: 18602 RVA: 0x0014A3BB File Offset: 0x001485BB
		// (set) Token: 0x060048AB RID: 18603 RVA: 0x0014A3CD File Offset: 0x001485CD
		public Style EditingElementStyle
		{
			get
			{
				return (Style)base.GetValue(DataGridComboBoxColumn.EditingElementStyleProperty);
			}
			set
			{
				base.SetValue(DataGridComboBoxColumn.EditingElementStyleProperty, value);
			}
		}

		// Token: 0x060048AC RID: 18604 RVA: 0x0014A3DC File Offset: 0x001485DC
		private void ApplyStyle(bool isEditing, bool defaultToElementStyle, FrameworkElement element)
		{
			Style style = this.PickStyle(isEditing, defaultToElementStyle);
			if (style != null)
			{
				element.Style = style;
			}
		}

		// Token: 0x060048AD RID: 18605 RVA: 0x0014A3FC File Offset: 0x001485FC
		internal void ApplyStyle(bool isEditing, bool defaultToElementStyle, FrameworkContentElement element)
		{
			Style style = this.PickStyle(isEditing, defaultToElementStyle);
			if (style != null)
			{
				element.Style = style;
			}
		}

		// Token: 0x060048AE RID: 18606 RVA: 0x0014A41C File Offset: 0x0014861C
		private Style PickStyle(bool isEditing, bool defaultToElementStyle)
		{
			Style style = isEditing ? this.EditingElementStyle : this.ElementStyle;
			if (isEditing && defaultToElementStyle && style == null)
			{
				style = this.ElementStyle;
			}
			return style;
		}

		// Token: 0x060048AF RID: 18607 RVA: 0x0014A44B File Offset: 0x0014864B
		private static void ApplyBinding(BindingBase binding, DependencyObject target, DependencyProperty property)
		{
			if (binding != null)
			{
				BindingOperations.SetBinding(target, property, binding);
				return;
			}
			BindingOperations.ClearBinding(target, property);
		}

		/// <summary>Gets or sets the binding object to use when getting or setting cell content for the clipboard.</summary>
		/// <returns>An object that represents the binding.</returns>
		// Token: 0x170011C0 RID: 4544
		// (get) Token: 0x060048B0 RID: 18608 RVA: 0x0014A461 File Offset: 0x00148661
		// (set) Token: 0x060048B1 RID: 18609 RVA: 0x001417E1 File Offset: 0x0013F9E1
		public override BindingBase ClipboardContentBinding
		{
			get
			{
				return base.ClipboardContentBinding ?? this.EffectiveBinding;
			}
			set
			{
				base.ClipboardContentBinding = value;
			}
		}

		/// <summary>Gets or sets a collection that is used to generate the content of the combo box control.</summary>
		/// <returns>A collection that is used to generate the content of the combo box control. The registered default is <see langword="null" />. For information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x170011C1 RID: 4545
		// (get) Token: 0x060048B2 RID: 18610 RVA: 0x0014A473 File Offset: 0x00148673
		// (set) Token: 0x060048B3 RID: 18611 RVA: 0x0014A485 File Offset: 0x00148685
		public IEnumerable ItemsSource
		{
			get
			{
				return (IEnumerable)base.GetValue(DataGridComboBoxColumn.ItemsSourceProperty);
			}
			set
			{
				base.SetValue(DataGridComboBoxColumn.ItemsSourceProperty, value);
			}
		}

		/// <summary>Gets or sets a path to a value on the source object to provide the visual representation of the object.</summary>
		/// <returns>The path to a value on the source object. The registered default is an empty string (""). For information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x170011C2 RID: 4546
		// (get) Token: 0x060048B4 RID: 18612 RVA: 0x0014A493 File Offset: 0x00148693
		// (set) Token: 0x060048B5 RID: 18613 RVA: 0x0014A4A5 File Offset: 0x001486A5
		public string DisplayMemberPath
		{
			get
			{
				return (string)base.GetValue(DataGridComboBoxColumn.DisplayMemberPathProperty);
			}
			set
			{
				base.SetValue(DataGridComboBoxColumn.DisplayMemberPathProperty, value);
			}
		}

		/// <summary>Gets or sets the path that is used to get the <see cref="P:System.Windows.Controls.Primitives.Selector.SelectedValue" /> from the <see cref="P:System.Windows.Controls.Primitives.Selector.SelectedItem" />.</summary>
		/// <returns>The path to get the selected value. The registered default is an empty string (""). For information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x170011C3 RID: 4547
		// (get) Token: 0x060048B6 RID: 18614 RVA: 0x0014A4B3 File Offset: 0x001486B3
		// (set) Token: 0x060048B7 RID: 18615 RVA: 0x0014A4C5 File Offset: 0x001486C5
		public string SelectedValuePath
		{
			get
			{
				return (string)base.GetValue(DataGridComboBoxColumn.SelectedValuePathProperty);
			}
			set
			{
				base.SetValue(DataGridComboBoxColumn.SelectedValuePathProperty, value);
			}
		}

		/// <summary>Refreshes the contents of a cell in the column in response to a binding change.</summary>
		/// <param name="element">The cell to update.</param>
		/// <param name="propertyName">The name of the column property that has changed.</param>
		// Token: 0x060048B8 RID: 18616 RVA: 0x0014A4D4 File Offset: 0x001486D4
		protected internal override void RefreshCellContent(FrameworkElement element, string propertyName)
		{
			DataGridCell dataGridCell = element as DataGridCell;
			if (dataGridCell == null)
			{
				base.RefreshCellContent(element, propertyName);
				return;
			}
			bool isEditing = dataGridCell.IsEditing;
			if ((string.Compare(propertyName, "ElementStyle", StringComparison.Ordinal) == 0 && !isEditing) || (string.Compare(propertyName, "EditingElementStyle", StringComparison.Ordinal) == 0 && isEditing))
			{
				dataGridCell.BuildVisualTree();
				return;
			}
			ComboBox comboBox = dataGridCell.Content as ComboBox;
			if (propertyName == "SelectedItemBinding")
			{
				DataGridComboBoxColumn.ApplyBinding(this.SelectedItemBinding, comboBox, Selector.SelectedItemProperty);
				return;
			}
			if (propertyName == "SelectedValueBinding")
			{
				DataGridComboBoxColumn.ApplyBinding(this.SelectedValueBinding, comboBox, Selector.SelectedValueProperty);
				return;
			}
			if (propertyName == "TextBinding")
			{
				DataGridComboBoxColumn.ApplyBinding(this.TextBinding, comboBox, ComboBox.TextProperty);
				return;
			}
			if (propertyName == "SelectedValuePath")
			{
				DataGridHelper.SyncColumnProperty(this, comboBox, Selector.SelectedValuePathProperty, DataGridComboBoxColumn.SelectedValuePathProperty);
				return;
			}
			if (propertyName == "DisplayMemberPath")
			{
				DataGridHelper.SyncColumnProperty(this, comboBox, ItemsControl.DisplayMemberPathProperty, DataGridComboBoxColumn.DisplayMemberPathProperty);
				return;
			}
			if (!(propertyName == "ItemsSource"))
			{
				base.RefreshCellContent(element, propertyName);
				return;
			}
			DataGridHelper.SyncColumnProperty(this, comboBox, ItemsControl.ItemsSourceProperty, DataGridComboBoxColumn.ItemsSourceProperty);
		}

		// Token: 0x060048B9 RID: 18617 RVA: 0x0014A5F9 File Offset: 0x001487F9
		private object GetComboBoxSelectionValue(ComboBox comboBox)
		{
			if (this.SelectedItemBinding != null)
			{
				return comboBox.SelectedItem;
			}
			if (this.SelectedValueBinding != null)
			{
				return comboBox.SelectedValue;
			}
			return comboBox.Text;
		}

		/// <summary>Gets a read-only combo box control that is bound to the column's <see cref="P:System.Windows.Controls.DataGridComboBoxColumn.SelectedItemBinding" />, <see cref="P:System.Windows.Controls.DataGridComboBoxColumn.SelectedValueBinding" />, and <see cref="P:System.Windows.Controls.DataGridComboBoxColumn.TextBinding" /> values.</summary>
		/// <param name="cell">The cell that will contain the generated element.</param>
		/// <param name="dataItem">The data item represented by the row that contains the intended cell.</param>
		/// <returns>A new, read-only combo box control that is bound to the column's <see cref="P:System.Windows.Controls.DataGridComboBoxColumn.SelectedItemBinding" />, <see cref="P:System.Windows.Controls.DataGridComboBoxColumn.SelectedValueBinding" />, and <see cref="P:System.Windows.Controls.DataGridComboBoxColumn.TextBinding" /> values.</returns>
		// Token: 0x060048BA RID: 18618 RVA: 0x0014A620 File Offset: 0x00148820
		protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
		{
			DataGridComboBoxColumn.TextBlockComboBox textBlockComboBox = new DataGridComboBoxColumn.TextBlockComboBox();
			this.ApplyStyle(false, false, textBlockComboBox);
			this.ApplyColumnProperties(textBlockComboBox);
			DataGridHelper.RestoreFlowDirection(textBlockComboBox, cell);
			return textBlockComboBox;
		}

		/// <summary>Gets a combo box control that is bound to the column's <see cref="P:System.Windows.Controls.DataGridComboBoxColumn.SelectedItemBinding" />, <see cref="P:System.Windows.Controls.DataGridComboBoxColumn.SelectedValueBinding" />, and <see cref="P:System.Windows.Controls.DataGridComboBoxColumn.TextBinding" /> values.</summary>
		/// <param name="cell">The cell that will contain the generated element.</param>
		/// <param name="dataItem">The data item represented by the row that contains the intended cell.</param>
		/// <returns>A new combo box control that is bound to the column's <see cref="P:System.Windows.Controls.DataGridComboBoxColumn.SelectedItemBinding" />, <see cref="P:System.Windows.Controls.DataGridComboBoxColumn.SelectedValueBinding" />, and <see cref="P:System.Windows.Controls.DataGridComboBoxColumn.TextBinding" /> values.</returns>
		// Token: 0x060048BB RID: 18619 RVA: 0x0014A64C File Offset: 0x0014884C
		protected override FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem)
		{
			ComboBox comboBox = new ComboBox();
			this.ApplyStyle(true, false, comboBox);
			this.ApplyColumnProperties(comboBox);
			DataGridHelper.RestoreFlowDirection(comboBox, cell);
			return comboBox;
		}

		// Token: 0x060048BC RID: 18620 RVA: 0x0014A678 File Offset: 0x00148878
		private void ApplyColumnProperties(ComboBox comboBox)
		{
			DataGridComboBoxColumn.ApplyBinding(this.SelectedItemBinding, comboBox, Selector.SelectedItemProperty);
			DataGridComboBoxColumn.ApplyBinding(this.SelectedValueBinding, comboBox, Selector.SelectedValueProperty);
			DataGridComboBoxColumn.ApplyBinding(this.TextBinding, comboBox, ComboBox.TextProperty);
			DataGridHelper.SyncColumnProperty(this, comboBox, Selector.SelectedValuePathProperty, DataGridComboBoxColumn.SelectedValuePathProperty);
			DataGridHelper.SyncColumnProperty(this, comboBox, ItemsControl.DisplayMemberPathProperty, DataGridComboBoxColumn.DisplayMemberPathProperty);
			DataGridHelper.SyncColumnProperty(this, comboBox, ItemsControl.ItemsSourceProperty, DataGridComboBoxColumn.ItemsSourceProperty);
		}

		/// <summary>Called when a cell in the column enters editing mode.</summary>
		/// <param name="editingElement">The element that the column displays for a cell in editing mode.</param>
		/// <param name="editingEventArgs">Information about the user gesture that is causing a cell to enter editing mode.</param>
		/// <returns>The unedited value.</returns>
		// Token: 0x060048BD RID: 18621 RVA: 0x0014A6EC File Offset: 0x001488EC
		protected override object PrepareCellForEdit(FrameworkElement editingElement, RoutedEventArgs editingEventArgs)
		{
			ComboBox comboBox = editingElement as ComboBox;
			if (comboBox != null)
			{
				comboBox.Focus();
				object comboBoxSelectionValue = this.GetComboBoxSelectionValue(comboBox);
				if (DataGridComboBoxColumn.IsComboBoxOpeningInputEvent(editingEventArgs))
				{
					comboBox.IsDropDownOpen = true;
				}
				return comboBoxSelectionValue;
			}
			return null;
		}

		/// <summary>Causes the column cell being edited to revert to the specified value.</summary>
		/// <param name="editingElement">The element that the column displays for a cell in editing mode.</param>
		/// <param name="uneditedValue">The previous, unedited value in the cell being edited.</param>
		// Token: 0x060048BE RID: 18622 RVA: 0x0014A724 File Offset: 0x00148924
		protected override void CancelCellEdit(FrameworkElement editingElement, object uneditedValue)
		{
			ComboBox comboBox = editingElement as ComboBox;
			if (comboBox != null && comboBox.EditableTextBoxSite != null)
			{
				DataGridHelper.CacheFlowDirection(comboBox.EditableTextBoxSite, comboBox.Parent as DataGridCell);
				DataGridHelper.CacheFlowDirection(comboBox, comboBox.Parent as DataGridCell);
			}
			base.CancelCellEdit(editingElement, uneditedValue);
		}

		/// <summary>Performs any required validation before exiting the editing mode.</summary>
		/// <param name="editingElement">The element that the column displays for a cell in editing mode.</param>
		/// <returns>
		///     <see langword="false" /> if validation fails; otherwise, <see langword="true" />.</returns>
		// Token: 0x060048BF RID: 18623 RVA: 0x0014A774 File Offset: 0x00148974
		protected override bool CommitCellEdit(FrameworkElement editingElement)
		{
			ComboBox comboBox = editingElement as ComboBox;
			if (comboBox != null && comboBox.EditableTextBoxSite != null)
			{
				DataGridHelper.CacheFlowDirection(comboBox.EditableTextBoxSite, comboBox.Parent as DataGridCell);
				DataGridHelper.CacheFlowDirection(comboBox, comboBox.Parent as DataGridCell);
			}
			return base.CommitCellEdit(editingElement);
		}

		// Token: 0x060048C0 RID: 18624 RVA: 0x0014A7C1 File Offset: 0x001489C1
		internal override void OnInput(InputEventArgs e)
		{
			if (DataGridComboBoxColumn.IsComboBoxOpeningInputEvent(e))
			{
				base.BeginEdit(e, true);
			}
		}

		// Token: 0x060048C1 RID: 18625 RVA: 0x0014A7D4 File Offset: 0x001489D4
		private static bool IsComboBoxOpeningInputEvent(RoutedEventArgs e)
		{
			KeyEventArgs keyEventArgs = e as KeyEventArgs;
			if (keyEventArgs != null && keyEventArgs.RoutedEvent == Keyboard.KeyDownEvent && (keyEventArgs.KeyStates & KeyStates.Down) == KeyStates.Down)
			{
				bool flag = (keyEventArgs.KeyboardDevice.Modifiers & ModifierKeys.Alt) == ModifierKeys.Alt;
				Key key = keyEventArgs.Key;
				if (key == Key.System)
				{
					key = keyEventArgs.SystemKey;
				}
				return (key == Key.F4 && !flag) || ((key == Key.Up || key == Key.Down) && flag);
			}
			return false;
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGridComboBoxColumn.ElementStyle" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGridComboBoxColumn.ElementStyle" /> dependency property.</returns>
		// Token: 0x040029A3 RID: 10659
		public static readonly DependencyProperty ElementStyleProperty = DataGridBoundColumn.ElementStyleProperty.AddOwner(typeof(DataGridComboBoxColumn), new FrameworkPropertyMetadata(DataGridComboBoxColumn.DefaultElementStyle));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGridComboBoxColumn.EditingElementStyle" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGridComboBoxColumn.EditingElementStyle" /> dependency property.</returns>
		// Token: 0x040029A4 RID: 10660
		public static readonly DependencyProperty EditingElementStyleProperty = DataGridBoundColumn.EditingElementStyleProperty.AddOwner(typeof(DataGridComboBoxColumn), new FrameworkPropertyMetadata(DataGridComboBoxColumn.DefaultEditingElementStyle));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGridComboBoxColumn.ItemsSource" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGridComboBoxColumn.ItemsSource" /> dependency property.</returns>
		// Token: 0x040029A5 RID: 10661
		public static readonly DependencyProperty ItemsSourceProperty = ItemsControl.ItemsSourceProperty.AddOwner(typeof(DataGridComboBoxColumn), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGridColumn.NotifyPropertyChangeForRefreshContent)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGridComboBoxColumn.DisplayMemberPath" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGridComboBoxColumn.DisplayMemberPath" /> dependency property.</returns>
		// Token: 0x040029A6 RID: 10662
		public static readonly DependencyProperty DisplayMemberPathProperty = ItemsControl.DisplayMemberPathProperty.AddOwner(typeof(DataGridComboBoxColumn), new FrameworkPropertyMetadata(string.Empty, new PropertyChangedCallback(DataGridColumn.NotifyPropertyChangeForRefreshContent)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGridComboBoxColumn.SelectedValuePath" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGridComboBoxColumn.SelectedValuePath" /> dependency property.</returns>
		// Token: 0x040029A7 RID: 10663
		public static readonly DependencyProperty SelectedValuePathProperty = Selector.SelectedValuePathProperty.AddOwner(typeof(DataGridComboBoxColumn), new FrameworkPropertyMetadata(string.Empty, new PropertyChangedCallback(DataGridColumn.NotifyPropertyChangeForRefreshContent)));

		// Token: 0x040029A8 RID: 10664
		private static Style _defaultElementStyle;

		// Token: 0x040029A9 RID: 10665
		private BindingBase _selectedValueBinding;

		// Token: 0x040029AA RID: 10666
		private BindingBase _selectedItemBinding;

		// Token: 0x040029AB RID: 10667
		private BindingBase _textBinding;

		// Token: 0x0200096A RID: 2410
		internal class TextBlockComboBox : ComboBox
		{
			// Token: 0x06008765 RID: 34661 RVA: 0x0024FBB2 File Offset: 0x0024DDB2
			static TextBlockComboBox()
			{
				FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(DataGridComboBoxColumn.TextBlockComboBox), new FrameworkPropertyMetadata(DataGridComboBoxColumn.TextBlockComboBoxStyleKey));
				KeyboardNavigation.IsTabStopProperty.OverrideMetadata(typeof(DataGridComboBoxColumn.TextBlockComboBox), new FrameworkPropertyMetadata(false));
			}
		}
	}
}
