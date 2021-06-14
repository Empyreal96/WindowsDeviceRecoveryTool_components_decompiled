using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Automation.Peers;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using MS.Internal;
using MS.Internal.Controls;
using MS.Internal.Data;
using MS.Internal.KnownBoxes;

namespace System.Windows.Controls.Primitives
{
	/// <summary>Represents a control that allows a user to select items from among its child elements. </summary>
	// Token: 0x020005A8 RID: 1448
	[DefaultEvent("SelectionChanged")]
	[DefaultProperty("SelectedIndex")]
	[Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
	public abstract class Selector : ItemsControl
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.Primitives.Selector" /> class. </summary>
		// Token: 0x06005FBF RID: 24511 RVA: 0x001AD4AC File Offset: 0x001AB6AC
		protected Selector()
		{
			base.Items.CurrentChanged += this.OnCurrentChanged;
			base.ItemContainerGenerator.StatusChanged += this.OnGeneratorStatusChanged;
			this._focusEnterMainFocusScopeEventHandler = new EventHandler(this.OnFocusEnterMainFocusScope);
			KeyboardNavigation.Current.FocusEnterMainFocusScope += this._focusEnterMainFocusScopeEventHandler;
			ObservableCollection<object> observableCollection = new SelectedItemCollection(this);
			base.SetValue(Selector.SelectedItemsPropertyKey, observableCollection);
			observableCollection.CollectionChanged += this.OnSelectedItemsCollectionChanged;
			base.SetValue(Selector.IsSelectionActivePropertyKey, BooleanBoxes.FalseBox);
		}

		// Token: 0x06005FC0 RID: 24512 RVA: 0x001AD564 File Offset: 0x001AB764
		static Selector()
		{
			Selector.SelectionChangedEvent = EventManager.RegisterRoutedEvent("SelectionChanged", RoutingStrategy.Bubble, typeof(SelectionChangedEventHandler), typeof(Selector));
			Selector.SelectedEvent = EventManager.RegisterRoutedEvent("Selected", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Selector));
			Selector.UnselectedEvent = EventManager.RegisterRoutedEvent("Unselected", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Selector));
			Selector.IsSelectionActivePropertyKey = DependencyProperty.RegisterAttachedReadOnly("IsSelectionActive", typeof(bool), typeof(Selector), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.Inherits));
			Selector.IsSelectionActiveProperty = Selector.IsSelectionActivePropertyKey.DependencyProperty;
			Selector.IsSelectedProperty = DependencyProperty.RegisterAttached("IsSelected", typeof(bool), typeof(Selector), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
			Selector.IsSynchronizedWithCurrentItemProperty = DependencyProperty.Register("IsSynchronizedWithCurrentItem", typeof(bool?), typeof(Selector), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(Selector.OnIsSynchronizedWithCurrentItemChanged)));
			Selector.SelectedIndexProperty = DependencyProperty.Register("SelectedIndex", typeof(int), typeof(Selector), new FrameworkPropertyMetadata(-1, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Journal, new PropertyChangedCallback(Selector.OnSelectedIndexChanged), new CoerceValueCallback(Selector.CoerceSelectedIndex)), new ValidateValueCallback(Selector.ValidateSelectedIndex));
			Selector.SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(object), typeof(Selector), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(Selector.OnSelectedItemChanged), new CoerceValueCallback(Selector.CoerceSelectedItem)));
			Selector.SelectedValueProperty = DependencyProperty.Register("SelectedValue", typeof(object), typeof(Selector), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(Selector.OnSelectedValueChanged), new CoerceValueCallback(Selector.CoerceSelectedValue)));
			Selector.SelectedValuePathProperty = DependencyProperty.Register("SelectedValuePath", typeof(string), typeof(Selector), new FrameworkPropertyMetadata(string.Empty, new PropertyChangedCallback(Selector.OnSelectedValuePathChanged)));
			Selector.SelectedItemsPropertyKey = DependencyProperty.RegisterReadOnly("SelectedItems", typeof(IList), typeof(Selector), new FrameworkPropertyMetadata(null));
			Selector.SelectedItemsImplProperty = Selector.SelectedItemsPropertyKey.DependencyProperty;
			Selector.ItemValueBindingExpression = new BindingExpressionUncommonField();
			Selector.PendingSelectionByValueField = new UncommonField<ItemsControl.ItemInfo>();
			Selector.MatchExplicitEqualityComparer = new Selector.ItemInfoEqualityComparer(false);
			Selector.MatchUnresolvedEqualityComparer = new Selector.ItemInfoEqualityComparer(true);
			Selector.ChangeInfoField = new UncommonField<Selector.ChangeInfo>();
			EventManager.RegisterClassHandler(typeof(Selector), Selector.SelectedEvent, new RoutedEventHandler(Selector.OnSelected));
			EventManager.RegisterClassHandler(typeof(Selector), Selector.UnselectedEvent, new RoutedEventHandler(Selector.OnUnselected));
		}

		/// <summary>Occurs when the selection of a <see cref="T:System.Windows.Controls.Primitives.Selector" /> changes.</summary>
		// Token: 0x1400011C RID: 284
		// (add) Token: 0x06005FC1 RID: 24513 RVA: 0x001AD84A File Offset: 0x001ABA4A
		// (remove) Token: 0x06005FC2 RID: 24514 RVA: 0x001AD858 File Offset: 0x001ABA58
		[Category("Behavior")]
		public event SelectionChangedEventHandler SelectionChanged
		{
			add
			{
				base.AddHandler(Selector.SelectionChangedEvent, value);
			}
			remove
			{
				base.RemoveHandler(Selector.SelectionChangedEvent, value);
			}
		}

		/// <summary>Adds a handler for the <see cref="E:System.Windows.Controls.Primitives.Selector.Selected" /> attached event. </summary>
		/// <param name="element">Element that listens to this event.</param>
		/// <param name="handler">Event handler to add.</param>
		// Token: 0x06005FC3 RID: 24515 RVA: 0x001AD866 File Offset: 0x001ABA66
		public static void AddSelectedHandler(DependencyObject element, RoutedEventHandler handler)
		{
			UIElement.AddHandler(element, Selector.SelectedEvent, handler);
		}

		/// <summary>Removes a handler for the <see cref="E:System.Windows.Controls.Primitives.Selector.Selected" /> attached event. </summary>
		/// <param name="element">Element that listens to this event.</param>
		/// <param name="handler">Event handler to remove.</param>
		// Token: 0x06005FC4 RID: 24516 RVA: 0x001AD874 File Offset: 0x001ABA74
		public static void RemoveSelectedHandler(DependencyObject element, RoutedEventHandler handler)
		{
			UIElement.RemoveHandler(element, Selector.SelectedEvent, handler);
		}

		/// <summary>Adds a handler for the <see cref="E:System.Windows.Controls.Primitives.Selector.Unselected" /> attached event. </summary>
		/// <param name="element">Element that listens to this event.</param>
		/// <param name="handler">Event handler to add.</param>
		// Token: 0x06005FC5 RID: 24517 RVA: 0x001AD882 File Offset: 0x001ABA82
		public static void AddUnselectedHandler(DependencyObject element, RoutedEventHandler handler)
		{
			UIElement.AddHandler(element, Selector.UnselectedEvent, handler);
		}

		/// <summary>Removes a handler for the <see cref="E:System.Windows.Controls.Primitives.Selector.Unselected" /> attached event. </summary>
		/// <param name="element">Element that listens to this event.</param>
		/// <param name="handler">Event handler to remove.</param>
		// Token: 0x06005FC6 RID: 24518 RVA: 0x001AD890 File Offset: 0x001ABA90
		public static void RemoveUnselectedHandler(DependencyObject element, RoutedEventHandler handler)
		{
			UIElement.RemoveHandler(element, Selector.UnselectedEvent, handler);
		}

		/// <summary>Gets a value that indicates whether the keyboard focus is within the <see cref="T:System.Windows.Controls.Primitives.Selector" /></summary>
		/// <param name="element">The element from which to read the attached property.</param>
		/// <returns>Value of the property, true if the keyboard focus is within the <see cref="T:System.Windows.Controls.Primitives.Selector" />.</returns>
		// Token: 0x06005FC7 RID: 24519 RVA: 0x001AD89E File Offset: 0x001ABA9E
		public static bool GetIsSelectionActive(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(Selector.IsSelectionActiveProperty);
		}

		/// <summary>Gets the value of the <see cref="P:System.Windows.Controls.Primitives.Selector.IsSelected" /> attached property that indicates whether an item is selected. </summary>
		/// <param name="element">Object to query concerning the <see cref="P:System.Windows.Controls.Primitives.Selector.IsSelected" /> property.</param>
		/// <returns>Boolean value, true if the <see cref="P:System.Windows.Controls.Primitives.Selector.IsSelected" /> property is true.</returns>
		// Token: 0x06005FC8 RID: 24520 RVA: 0x001AD8BE File Offset: 0x001ABABE
		[AttachedPropertyBrowsableForChildren]
		public static bool GetIsSelected(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(Selector.IsSelectedProperty);
		}

		/// <summary>Sets a property value that indicates whether an item in a <see cref="T:System.Windows.Controls.Primitives.Selector" /> is selected. </summary>
		/// <param name="element">Object on which to set the property.</param>
		/// <param name="isSelected">Value to set.</param>
		// Token: 0x06005FC9 RID: 24521 RVA: 0x001AD8DE File Offset: 0x001ABADE
		public static void SetIsSelected(DependencyObject element, bool isSelected)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Selector.IsSelectedProperty, BooleanBoxes.Box(isSelected));
		}

		/// <summary>Gets or sets a value that indicates whether a <see cref="T:System.Windows.Controls.Primitives.Selector" /> should keep the <see cref="P:System.Windows.Controls.Primitives.Selector.SelectedItem" /> synchronized with the current item in the <see cref="P:System.Windows.Controls.ItemsControl.Items" /> property.  </summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="P:System.Windows.Controls.Primitives.Selector.SelectedItem" /> is always synchronized with the current item in the <see cref="T:System.Windows.Controls.ItemCollection" />; <see langword="false" /> if the <see cref="P:System.Windows.Controls.Primitives.Selector.SelectedItem" /> is never synchronized with the current item; <see langword="null" /> if the <see cref="P:System.Windows.Controls.Primitives.Selector.SelectedItem" /> is synchronized with the current item only if the <see cref="T:System.Windows.Controls.Primitives.Selector" /> uses a <see cref="T:System.Windows.Data.CollectionView" />.  The default value is <see langword="null" />.</returns>
		// Token: 0x1700170F RID: 5903
		// (get) Token: 0x06005FCA RID: 24522 RVA: 0x001AD8FF File Offset: 0x001ABAFF
		// (set) Token: 0x06005FCB RID: 24523 RVA: 0x001AD911 File Offset: 0x001ABB11
		[Bindable(true)]
		[Category("Behavior")]
		[TypeConverter("System.Windows.NullableBoolConverter, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, Custom=null")]
		[Localizability(LocalizationCategory.NeverLocalize)]
		public bool? IsSynchronizedWithCurrentItem
		{
			get
			{
				return (bool?)base.GetValue(Selector.IsSynchronizedWithCurrentItemProperty);
			}
			set
			{
				base.SetValue(Selector.IsSynchronizedWithCurrentItemProperty, value);
			}
		}

		// Token: 0x06005FCC RID: 24524 RVA: 0x001AD924 File Offset: 0x001ABB24
		private static void OnIsSynchronizedWithCurrentItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Selector selector = (Selector)d;
			selector.SetSynchronizationWithCurrentItem();
		}

		// Token: 0x06005FCD RID: 24525 RVA: 0x001AD940 File Offset: 0x001ABB40
		private void SetSynchronizationWithCurrentItem()
		{
			bool? isSynchronizedWithCurrentItem = this.IsSynchronizedWithCurrentItem;
			bool isSynchronizedWithCurrentItemPrivate = this.IsSynchronizedWithCurrentItemPrivate;
			bool flag;
			if (isSynchronizedWithCurrentItem != null)
			{
				flag = isSynchronizedWithCurrentItem.Value;
			}
			else
			{
				if (!base.IsInitialized)
				{
					return;
				}
				flag = ((SelectionMode)base.GetValue(ListBox.SelectionModeProperty) == SelectionMode.Single && !CollectionViewSource.IsDefaultView(base.Items.CollectionView));
			}
			this.IsSynchronizedWithCurrentItemPrivate = flag;
			if (!isSynchronizedWithCurrentItemPrivate && flag)
			{
				if (this.SelectedItem != null)
				{
					this.SetCurrentToSelected();
					return;
				}
				this.SetSelectedToCurrent();
			}
		}

		/// <summary>Gets or sets the index of the first item in the current selection or returns negative one (-1) if the selection is empty.  </summary>
		/// <returns>The index of first item in the current selection. The default value is negative one (-1).</returns>
		// Token: 0x17001710 RID: 5904
		// (get) Token: 0x06005FCE RID: 24526 RVA: 0x001AD9C6 File Offset: 0x001ABBC6
		// (set) Token: 0x06005FCF RID: 24527 RVA: 0x001AD9D8 File Offset: 0x001ABBD8
		[Bindable(true)]
		[Category("Appearance")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Localizability(LocalizationCategory.NeverLocalize)]
		public int SelectedIndex
		{
			get
			{
				return (int)base.GetValue(Selector.SelectedIndexProperty);
			}
			set
			{
				base.SetValue(Selector.SelectedIndexProperty, value);
			}
		}

		// Token: 0x06005FD0 RID: 24528 RVA: 0x001AD9EC File Offset: 0x001ABBEC
		private static void OnSelectedIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Selector selector = (Selector)d;
			if (!selector.SelectionChange.IsActive)
			{
				int index = (int)e.NewValue;
				selector.SelectionChange.SelectJustThisItem(selector.ItemInfoFromIndex(index), true);
			}
		}

		// Token: 0x06005FD1 RID: 24529 RVA: 0x001ADA2D File Offset: 0x001ABC2D
		private static bool ValidateSelectedIndex(object o)
		{
			return (int)o >= -1;
		}

		// Token: 0x06005FD2 RID: 24530 RVA: 0x001ADA3C File Offset: 0x001ABC3C
		private static object CoerceSelectedIndex(DependencyObject d, object value)
		{
			Selector selector = (Selector)d;
			if (value is int && (int)value >= selector.Items.Count)
			{
				return DependencyProperty.UnsetValue;
			}
			return value;
		}

		/// <summary>Gets or sets the first item in the current selection or returns null if the selection is empty  </summary>
		/// <returns>The first item in the current selection or <see langword="null" /> if the selection is empty.</returns>
		// Token: 0x17001711 RID: 5905
		// (get) Token: 0x06005FD3 RID: 24531 RVA: 0x001ADA72 File Offset: 0x001ABC72
		// (set) Token: 0x06005FD4 RID: 24532 RVA: 0x001ADA7F File Offset: 0x001ABC7F
		[Bindable(true)]
		[Category("Appearance")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public object SelectedItem
		{
			get
			{
				return base.GetValue(Selector.SelectedItemProperty);
			}
			set
			{
				base.SetValue(Selector.SelectedItemProperty, value);
			}
		}

		// Token: 0x06005FD5 RID: 24533 RVA: 0x001ADA90 File Offset: 0x001ABC90
		private static void OnSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Selector selector = (Selector)d;
			if (!selector.SelectionChange.IsActive)
			{
				selector.SelectionChange.SelectJustThisItem(selector.NewItemInfo(e.NewValue, null, -1), false);
			}
		}

		// Token: 0x06005FD6 RID: 24534 RVA: 0x001ADACC File Offset: 0x001ABCCC
		private static object CoerceSelectedItem(DependencyObject d, object value)
		{
			Selector selector = (Selector)d;
			if (value == null || selector.SkipCoerceSelectedItemCheck)
			{
				return value;
			}
			int selectedIndex = selector.SelectedIndex;
			if ((selectedIndex > -1 && selectedIndex < selector.Items.Count && selector.Items[selectedIndex] == value) || selector.Items.Contains(value))
			{
				return value;
			}
			return DependencyProperty.UnsetValue;
		}

		/// <summary>Gets or sets the value of the <see cref="P:System.Windows.Controls.Primitives.Selector.SelectedItem" />, obtained by using <see cref="P:System.Windows.Controls.Primitives.Selector.SelectedValuePath" />.  </summary>
		/// <returns>The value of the selected item.</returns>
		// Token: 0x17001712 RID: 5906
		// (get) Token: 0x06005FD7 RID: 24535 RVA: 0x001ADB2A File Offset: 0x001ABD2A
		// (set) Token: 0x06005FD8 RID: 24536 RVA: 0x001ADB37 File Offset: 0x001ABD37
		[Bindable(true)]
		[Category("Appearance")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Localizability(LocalizationCategory.NeverLocalize)]
		public object SelectedValue
		{
			get
			{
				return base.GetValue(Selector.SelectedValueProperty);
			}
			set
			{
				base.SetValue(Selector.SelectedValueProperty, value);
			}
		}

		// Token: 0x06005FD9 RID: 24537 RVA: 0x001ADB48 File Offset: 0x001ABD48
		private static void OnSelectedValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (!FrameworkAppContextSwitches.SelectionPropertiesCanLagBehindSelectionChangedEvent)
			{
				Selector selector = (Selector)d;
				ItemsControl.ItemInfo value = Selector.PendingSelectionByValueField.GetValue(selector);
				if (value != null)
				{
					try
					{
						if (!selector.SelectionChange.IsActive)
						{
							selector._cacheValid[16] = true;
							selector.SelectionChange.SelectJustThisItem(value, true);
						}
					}
					finally
					{
						selector._cacheValid[16] = false;
						Selector.PendingSelectionByValueField.ClearValue(selector);
					}
				}
			}
		}

		// Token: 0x06005FDA RID: 24538 RVA: 0x001ADBCC File Offset: 0x001ABDCC
		private object SelectItemWithValue(object value, bool selectNow)
		{
			object obj;
			if (FrameworkAppContextSwitches.SelectionPropertiesCanLagBehindSelectionChangedEvent)
			{
				this._cacheValid[16] = true;
				if (base.HasItems)
				{
					int index;
					obj = this.FindItemWithValue(value, out index);
					this.SelectionChange.SelectJustThisItem(base.NewItemInfo(obj, null, index), true);
				}
				else
				{
					obj = DependencyProperty.UnsetValue;
					this._cacheValid[32] = true;
				}
				this._cacheValid[16] = false;
			}
			else if (base.HasItems)
			{
				int index2;
				obj = this.FindItemWithValue(value, out index2);
				ItemsControl.ItemInfo itemInfo = base.NewItemInfo(obj, null, index2);
				if (selectNow)
				{
					try
					{
						this._cacheValid[16] = true;
						this.SelectionChange.SelectJustThisItem(itemInfo, true);
						return obj;
					}
					finally
					{
						this._cacheValid[16] = false;
					}
				}
				Selector.PendingSelectionByValueField.SetValue(this, itemInfo);
			}
			else
			{
				obj = DependencyProperty.UnsetValue;
				this._cacheValid[32] = true;
			}
			return obj;
		}

		// Token: 0x06005FDB RID: 24539 RVA: 0x001ADCBC File Offset: 0x001ABEBC
		private object FindItemWithValue(object value, out int index)
		{
			index = -1;
			if (!base.HasItems)
			{
				return DependencyProperty.UnsetValue;
			}
			BindingExpression bindingExpression = this.PrepareItemValueBinding(base.Items.GetRepresentativeItem());
			if (bindingExpression == null)
			{
				return DependencyProperty.UnsetValue;
			}
			if (!string.IsNullOrEmpty(this.SelectedValuePath))
			{
				Type knownType = (value != null) ? value.GetType() : null;
				DynamicValueConverter converter = new DynamicValueConverter(false);
				index = 0;
				foreach (object obj in ((IEnumerable)base.Items))
				{
					bindingExpression.Activate(obj);
					object value2 = bindingExpression.Value;
					if (this.VerifyEqual(value, knownType, value2, converter))
					{
						bindingExpression.Deactivate();
						return obj;
					}
					index++;
				}
				bindingExpression.Deactivate();
				index = -1;
				return DependencyProperty.UnsetValue;
			}
			string path = bindingExpression.ParentBinding.Path.Path;
			if (!string.IsNullOrEmpty(path))
			{
				return SystemXmlHelper.FindXmlNodeWithInnerText(base.Items, value, out index);
			}
			index = base.Items.IndexOf(value);
			if (index >= 0)
			{
				return value;
			}
			return DependencyProperty.UnsetValue;
		}

		// Token: 0x06005FDC RID: 24540 RVA: 0x001ADDE8 File Offset: 0x001ABFE8
		private bool VerifyEqual(object knownValue, Type knownType, object itemValue, DynamicValueConverter converter)
		{
			object obj = knownValue;
			if (knownType != null && itemValue != null)
			{
				Type type = itemValue.GetType();
				if (!knownType.IsAssignableFrom(type))
				{
					obj = converter.Convert(knownValue, type);
					if (obj == DependencyProperty.UnsetValue)
					{
						obj = knownValue;
					}
				}
			}
			return object.Equals(obj, itemValue);
		}

		// Token: 0x06005FDD RID: 24541 RVA: 0x001ADE30 File Offset: 0x001AC030
		private static object CoerceSelectedValue(DependencyObject d, object value)
		{
			Selector selector = (Selector)d;
			if (selector.SelectionChange.IsActive)
			{
				selector._cacheValid[16] = false;
			}
			else
			{
				object obj = selector.SelectItemWithValue(value, false);
				if (obj == DependencyProperty.UnsetValue && selector.HasItems)
				{
					value = null;
				}
			}
			return value;
		}

		/// <summary>Gets or sets the path that is used to get the <see cref="P:System.Windows.Controls.Primitives.Selector.SelectedValue" /> from the <see cref="P:System.Windows.Controls.Primitives.Selector.SelectedItem" />.  </summary>
		/// <returns>The path used to get the <see cref="P:System.Windows.Controls.Primitives.Selector.SelectedValue" />. The default is an empty string.</returns>
		// Token: 0x17001713 RID: 5907
		// (get) Token: 0x06005FDE RID: 24542 RVA: 0x001ADE7E File Offset: 0x001AC07E
		// (set) Token: 0x06005FDF RID: 24543 RVA: 0x001ADE90 File Offset: 0x001AC090
		[Bindable(true)]
		[Category("Appearance")]
		[Localizability(LocalizationCategory.NeverLocalize)]
		public string SelectedValuePath
		{
			get
			{
				return (string)base.GetValue(Selector.SelectedValuePathProperty);
			}
			set
			{
				base.SetValue(Selector.SelectedValuePathProperty, value);
			}
		}

		// Token: 0x06005FE0 RID: 24544 RVA: 0x001ADEA0 File Offset: 0x001AC0A0
		private static void OnSelectedValuePathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Selector selector = (Selector)d;
			Selector.ItemValueBindingExpression.ClearValue(selector);
			if (selector.GetValueEntry(selector.LookupEntry(Selector.SelectedValueProperty.GlobalIndex), Selector.SelectedValueProperty, null, RequestFlags.RawEntry).IsCoerced || selector.SelectedValue != null)
			{
				selector.CoerceValue(Selector.SelectedValueProperty);
			}
		}

		// Token: 0x06005FE1 RID: 24545 RVA: 0x001ADEFC File Offset: 0x001AC0FC
		private BindingExpression PrepareItemValueBinding(object item)
		{
			if (item == null)
			{
				return null;
			}
			bool flag = SystemXmlHelper.IsXmlNode(item);
			BindingExpression bindingExpression = Selector.ItemValueBindingExpression.GetValue(this);
			if (bindingExpression != null)
			{
				Binding binding = bindingExpression.ParentBinding;
				bool flag2 = binding.XPath != null;
				if ((!flag2 && flag) || (flag2 && !flag))
				{
					Selector.ItemValueBindingExpression.ClearValue(this);
					bindingExpression = null;
				}
			}
			if (bindingExpression == null)
			{
				Binding binding = new Binding();
				binding.Source = null;
				if (flag)
				{
					binding.XPath = this.SelectedValuePath;
					binding.Path = new PropertyPath("/InnerText", new object[0]);
				}
				else
				{
					binding.Path = new PropertyPath(this.SelectedValuePath, new object[0]);
				}
				bindingExpression = (BindingExpression)BindingExpressionBase.CreateUntargetedBindingExpression(this, binding);
				Selector.ItemValueBindingExpression.SetValue(this, bindingExpression);
			}
			return bindingExpression;
		}

		// Token: 0x17001714 RID: 5908
		// (get) Token: 0x06005FE2 RID: 24546 RVA: 0x001ADFB8 File Offset: 0x001AC1B8
		internal IList SelectedItemsImpl
		{
			get
			{
				return (IList)base.GetValue(Selector.SelectedItemsImplProperty);
			}
		}

		// Token: 0x06005FE3 RID: 24547 RVA: 0x001ADFCC File Offset: 0x001AC1CC
		internal bool SetSelectedItemsImpl(IEnumerable selectedItems)
		{
			bool flag = false;
			if (!this.SelectionChange.IsActive)
			{
				this.SelectionChange.Begin();
				this.SelectionChange.CleanupDeferSelection();
				ObservableCollection<object> observableCollection = (ObservableCollection<object>)base.GetValue(Selector.SelectedItemsImplProperty);
				try
				{
					if (observableCollection != null)
					{
						foreach (object item in observableCollection)
						{
							this.SelectionChange.Unselect(base.NewUnresolvedItemInfo(item));
						}
					}
					if (selectedItems != null)
					{
						foreach (object item2 in selectedItems)
						{
							if (!this.SelectionChange.Select(base.NewUnresolvedItemInfo(item2), false))
							{
								this.SelectionChange.Cancel();
								return false;
							}
						}
					}
					this.SelectionChange.End();
					flag = true;
				}
				finally
				{
					if (!flag)
					{
						this.SelectionChange.Cancel();
					}
				}
				return flag;
			}
			return flag;
		}

		// Token: 0x06005FE4 RID: 24548 RVA: 0x001AE0F4 File Offset: 0x001AC2F4
		private void OnSelectedItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (this.SelectionChange.IsActive)
			{
				return;
			}
			if (!this.CanSelectMultiple)
			{
				throw new InvalidOperationException(SR.Get("ChangingCollectionNotSupported"));
			}
			this.SelectionChange.Begin();
			bool flag = false;
			try
			{
				switch (e.Action)
				{
				case NotifyCollectionChangedAction.Add:
					if (e.NewItems.Count != 1)
					{
						throw new NotSupportedException(SR.Get("RangeActionsNotSupported"));
					}
					this.SelectionChange.Select(base.NewUnresolvedItemInfo(e.NewItems[0]), false);
					break;
				case NotifyCollectionChangedAction.Remove:
					if (e.OldItems.Count != 1)
					{
						throw new NotSupportedException(SR.Get("RangeActionsNotSupported"));
					}
					this.SelectionChange.Unselect(base.NewUnresolvedItemInfo(e.OldItems[0]));
					break;
				case NotifyCollectionChangedAction.Replace:
					if (e.NewItems.Count != 1 || e.OldItems.Count != 1)
					{
						throw new NotSupportedException(SR.Get("RangeActionsNotSupported"));
					}
					this.SelectionChange.Unselect(base.NewUnresolvedItemInfo(e.OldItems[0]));
					this.SelectionChange.Select(base.NewUnresolvedItemInfo(e.NewItems[0]), false);
					break;
				case NotifyCollectionChangedAction.Move:
					break;
				case NotifyCollectionChangedAction.Reset:
				{
					this.SelectionChange.CleanupDeferSelection();
					for (int i = 0; i < this._selectedItems.Count; i++)
					{
						this.SelectionChange.Unselect(this._selectedItems[i]);
					}
					ObservableCollection<object> observableCollection = (ObservableCollection<object>)sender;
					for (int j = 0; j < observableCollection.Count; j++)
					{
						this.SelectionChange.Select(base.NewUnresolvedItemInfo(observableCollection[j]), false);
					}
					break;
				}
				default:
					throw new NotSupportedException(SR.Get("UnexpectedCollectionChangeAction", new object[]
					{
						e.Action
					}));
				}
				this.SelectionChange.End();
				flag = true;
			}
			finally
			{
				if (!flag)
				{
					this.SelectionChange.Cancel();
				}
			}
		}

		// Token: 0x17001715 RID: 5909
		// (get) Token: 0x06005FE5 RID: 24549 RVA: 0x001AE31C File Offset: 0x001AC51C
		// (set) Token: 0x06005FE6 RID: 24550 RVA: 0x001AE32A File Offset: 0x001AC52A
		internal bool CanSelectMultiple
		{
			get
			{
				return this._cacheValid[2];
			}
			set
			{
				if (this._cacheValid[2] != value)
				{
					this._cacheValid[2] = value;
					if (!value && this._selectedItems.Count > 1)
					{
						this.SelectionChange.Validate();
					}
				}
			}
		}

		/// <summary>Returns an item container to the state it was in before <see cref="M:System.Windows.Controls.ItemsControl.PrepareContainerForItemOverride(System.Windows.DependencyObject,System.Object)" />.</summary>
		/// <param name="element">The item container element.</param>
		/// <param name="item">The data item.</param>
		// Token: 0x06005FE7 RID: 24551 RVA: 0x001AE364 File Offset: 0x001AC564
		protected override void ClearContainerForItemOverride(DependencyObject element, object item)
		{
			base.ClearContainerForItemOverride(element, item);
			if (!((IGeneratorHost)this).IsItemItsOwnContainer(item))
			{
				try
				{
					this._clearingContainer = element;
					element.ClearValue(Selector.IsSelectedProperty);
				}
				finally
				{
					this._clearingContainer = null;
				}
			}
		}

		// Token: 0x06005FE8 RID: 24552 RVA: 0x001AE3B0 File Offset: 0x001AC5B0
		internal void RaiseIsSelectedChangedAutomationEvent(DependencyObject container, bool isSelected)
		{
			SelectorAutomationPeer selectorAutomationPeer = UIElementAutomationPeer.FromElement(this) as SelectorAutomationPeer;
			if (selectorAutomationPeer != null && selectorAutomationPeer.ItemPeers != null)
			{
				object itemOrContainerFromContainer = base.GetItemOrContainerFromContainer(container);
				if (itemOrContainerFromContainer != null)
				{
					SelectorItemAutomationPeer selectorItemAutomationPeer = selectorAutomationPeer.ItemPeers[itemOrContainerFromContainer] as SelectorItemAutomationPeer;
					if (selectorItemAutomationPeer != null)
					{
						selectorItemAutomationPeer.RaiseAutomationIsSelectedChanged(isSelected);
					}
				}
			}
		}

		// Token: 0x06005FE9 RID: 24553 RVA: 0x001AE3FB File Offset: 0x001AC5FB
		internal void SetInitialMousePosition()
		{
			this._lastMousePosition = Mouse.GetPosition(this);
		}

		// Token: 0x06005FEA RID: 24554 RVA: 0x001AE40C File Offset: 0x001AC60C
		internal bool DidMouseMove()
		{
			Point position = Mouse.GetPosition(this);
			if (position != this._lastMousePosition)
			{
				this._lastMousePosition = position;
				return true;
			}
			return false;
		}

		// Token: 0x06005FEB RID: 24555 RVA: 0x001AE438 File Offset: 0x001AC638
		internal void ResetLastMousePosition()
		{
			this._lastMousePosition = default(Point);
		}

		// Token: 0x06005FEC RID: 24556 RVA: 0x001AE448 File Offset: 0x001AC648
		internal virtual void SelectAllImpl()
		{
			this.SelectionChange.Begin();
			this.SelectionChange.CleanupDeferSelection();
			try
			{
				int num = 0;
				foreach (object item in ((IEnumerable)base.Items))
				{
					ItemsControl.ItemInfo info = base.NewItemInfo(item, null, num++);
					this.SelectionChange.Select(info, true);
				}
			}
			finally
			{
				this.SelectionChange.End();
			}
		}

		// Token: 0x06005FED RID: 24557 RVA: 0x001AE4E4 File Offset: 0x001AC6E4
		internal virtual void UnselectAllImpl()
		{
			this.SelectionChange.Begin();
			this.SelectionChange.CleanupDeferSelection();
			try
			{
				object internalSelectedItem = this.InternalSelectedItem;
				foreach (ItemsControl.ItemInfo info in ((IEnumerable<ItemsControl.ItemInfo>)this._selectedItems))
				{
					this.SelectionChange.Unselect(info);
				}
			}
			finally
			{
				this.SelectionChange.End();
			}
		}

		/// <summary>Updates the current selection when an item in the <see cref="T:System.Windows.Controls.Primitives.Selector" /> has changed</summary>
		/// <param name="e">The event data.</param>
		// Token: 0x06005FEE RID: 24558 RVA: 0x001AE56C File Offset: 0x001AC76C
		protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
		{
			base.OnItemsChanged(e);
			if (base.DataContext == BindingExpressionBase.DisconnectedItem && (!FrameworkAppContextSwitches.SelectorUpdatesSelectionPropertiesWhenDisconnected || (!FrameworkAppContextSwitches.SelectorInDataGridUpdatesSelectionPropertiesWhenDisconnected && this.IsInDataGrid())))
			{
				return;
			}
			if (e.Action == NotifyCollectionChangedAction.Reset || (e.Action == NotifyCollectionChangedAction.Add && e.NewStartingIndex == 0))
			{
				this.ResetSelectedItemsAlgorithm();
			}
			EffectiveValueEntry valueEntry = base.GetValueEntry(base.LookupEntry(Selector.SelectedIndexProperty.GlobalIndex), Selector.SelectedIndexProperty, null, RequestFlags.DeferredReferences);
			if (!valueEntry.IsDeferredReference || !(valueEntry.Value is DeferredSelectedIndexReference))
			{
				base.CoerceValue(Selector.SelectedIndexProperty);
			}
			base.CoerceValue(Selector.SelectedItemProperty);
			if (this._cacheValid[32] && !object.Equals(this.SelectedValue, this.InternalSelectedValue))
			{
				this.SelectItemWithValue(this.SelectedValue, true);
			}
			switch (e.Action)
			{
			case NotifyCollectionChangedAction.Add:
				this.SelectionChange.Begin();
				try
				{
					ItemsControl.ItemInfo info = base.NewItemInfo(e.NewItems[0], null, e.NewStartingIndex);
					if (this.InfoGetIsSelected(info))
					{
						this.SelectionChange.Select(info, true);
					}
					return;
				}
				finally
				{
					this.SelectionChange.End();
				}
				break;
			case NotifyCollectionChangedAction.Remove:
				this.RemoveFromSelection(e);
				return;
			case NotifyCollectionChangedAction.Replace:
				break;
			case NotifyCollectionChangedAction.Move:
				this.AdjustNewContainers();
				this.SelectionChange.Validate();
				return;
			case NotifyCollectionChangedAction.Reset:
				if (base.Items.IsEmpty)
				{
					this.SelectionChange.CleanupDeferSelection();
				}
				if (base.Items.CurrentItem != null && this.IsSynchronizedWithCurrentItemPrivate)
				{
					this.SetSelectedToCurrent();
					return;
				}
				this.SelectionChange.Begin();
				try
				{
					this.LocateSelectedItems(null, true);
					if (base.ItemsSource == null)
					{
						for (int i = 0; i < base.Items.Count; i++)
						{
							ItemsControl.ItemInfo itemInfo = base.ItemInfoFromIndex(i);
							if (this.InfoGetIsSelected(itemInfo) && !this._selectedItems.Contains(itemInfo))
							{
								this.SelectionChange.Select(itemInfo, true);
							}
						}
					}
					return;
				}
				finally
				{
					this.SelectionChange.End();
				}
				goto IL_211;
			default:
				goto IL_211;
			}
			this.ItemSetIsSelected(base.ItemInfoFromIndex(e.NewStartingIndex), false);
			this.RemoveFromSelection(e);
			return;
			IL_211:
			throw new NotSupportedException(SR.Get("UnexpectedCollectionChangeAction", new object[]
			{
				e.Action
			}));
		}

		// Token: 0x06005FEF RID: 24559 RVA: 0x001AE7CC File Offset: 0x001AC9CC
		internal override void AdjustItemInfoOverride(NotifyCollectionChangedEventArgs e)
		{
			base.AdjustItemInfos(e, this._selectedItems);
			base.AdjustItemInfoOverride(e);
		}

		// Token: 0x06005FF0 RID: 24560 RVA: 0x001AE7E4 File Offset: 0x001AC9E4
		private void RemoveFromSelection(NotifyCollectionChangedEventArgs e)
		{
			this.SelectionChange.Begin();
			try
			{
				ItemsControl.ItemInfo itemInfo = base.NewItemInfo(e.OldItems[0], ItemsControl.ItemInfo.SentinelContainer, e.OldStartingIndex);
				itemInfo.Container = null;
				if (this._selectedItems.Contains(itemInfo))
				{
					this.SelectionChange.Unselect(itemInfo);
				}
			}
			finally
			{
				this.SelectionChange.End();
			}
		}

		/// <summary>Called when the selection changes.</summary>
		/// <param name="e">The event data.</param>
		// Token: 0x06005FF1 RID: 24561 RVA: 0x00012CF1 File Offset: 0x00010EF1
		protected virtual void OnSelectionChanged(SelectionChangedEventArgs e)
		{
			base.RaiseEvent(e);
		}

		/// <summary>Called when the <see cref="P:System.Windows.UIElement.IsKeyboardFocusWithin" /> property has changed. </summary>
		/// <param name="e">The event data.</param>
		// Token: 0x06005FF2 RID: 24562 RVA: 0x001AE85C File Offset: 0x001ACA5C
		protected override void OnIsKeyboardFocusWithinChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnIsKeyboardFocusWithinChanged(e);
			bool flag = false;
			if ((bool)e.NewValue)
			{
				flag = true;
			}
			else
			{
				DependencyObject dependencyObject = Keyboard.FocusedElement as DependencyObject;
				if (dependencyObject != null)
				{
					UIElement uielement = KeyboardNavigation.GetVisualRoot(this) as UIElement;
					if (uielement != null && uielement.IsKeyboardFocusWithin && FocusManager.GetFocusScope(dependencyObject) != FocusManager.GetFocusScope(this))
					{
						flag = true;
					}
				}
			}
			if (flag)
			{
				base.SetValue(Selector.IsSelectionActivePropertyKey, BooleanBoxes.TrueBox);
				return;
			}
			base.SetValue(Selector.IsSelectionActivePropertyKey, BooleanBoxes.FalseBox);
		}

		// Token: 0x06005FF3 RID: 24563 RVA: 0x00189BE6 File Offset: 0x00187DE6
		private void OnFocusEnterMainFocusScope(object sender, EventArgs e)
		{
			if (!base.IsKeyboardFocusWithin)
			{
				base.ClearValue(Selector.IsSelectionActivePropertyKey);
			}
		}

		/// <summary>Called when the source of an item in a selector changes.</summary>
		/// <param name="oldValue">Old value of the source.</param>
		/// <param name="newValue">New value of the source.</param>
		// Token: 0x06005FF4 RID: 24564 RVA: 0x001AE8DD File Offset: 0x001ACADD
		protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
		{
			this.SetSynchronizationWithCurrentItem();
		}

		/// <summary>Prepares the specified element to display the specified item. </summary>
		/// <param name="element">The element that is used to display the specified item.</param>
		/// <param name="item">The specified item to display.</param>
		// Token: 0x06005FF5 RID: 24565 RVA: 0x001AE8E5 File Offset: 0x001ACAE5
		protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
		{
			base.PrepareContainerForItemOverride(element, item);
			if (item == this.SelectedItem)
			{
				KeyboardNavigation.Current.UpdateActiveElement(this, element);
			}
			this.OnNewContainer();
		}

		/// <summary>Raises the <see cref="E:System.Windows.FrameworkElement.Initialized" /> event. This method is invoked whenever <see cref="P:System.Windows.FrameworkElement.IsInitialized" /> is set to <see langword="true" /> internally.</summary>
		/// <param name="e">The <see cref="T:System.Windows.RoutedEventArgs" /> that contains the event data.</param>
		// Token: 0x06005FF6 RID: 24566 RVA: 0x001AE90A File Offset: 0x001ACB0A
		protected override void OnInitialized(EventArgs e)
		{
			base.OnInitialized(e);
			this.SetSynchronizationWithCurrentItem();
		}

		// Token: 0x17001716 RID: 5910
		// (get) Token: 0x06005FF7 RID: 24567 RVA: 0x001AE919 File Offset: 0x001ACB19
		// (set) Token: 0x06005FF8 RID: 24568 RVA: 0x001AE927 File Offset: 0x001ACB27
		private bool IsSynchronizedWithCurrentItemPrivate
		{
			get
			{
				return this._cacheValid[4];
			}
			set
			{
				this._cacheValid[4] = value;
			}
		}

		// Token: 0x17001717 RID: 5911
		// (get) Token: 0x06005FF9 RID: 24569 RVA: 0x001AE936 File Offset: 0x001ACB36
		// (set) Token: 0x06005FFA RID: 24570 RVA: 0x001AE944 File Offset: 0x001ACB44
		private bool SkipCoerceSelectedItemCheck
		{
			get
			{
				return this._cacheValid[8];
			}
			set
			{
				this._cacheValid[8] = value;
			}
		}

		// Token: 0x06005FFB RID: 24571 RVA: 0x001AE954 File Offset: 0x001ACB54
		private void SetSelectedHelper(object item, FrameworkElement UI, bool selected)
		{
			bool flag = Selector.ItemGetIsSelectable(item);
			if (!flag && selected)
			{
				throw new InvalidOperationException(SR.Get("CannotSelectNotSelectableItem"));
			}
			this.SelectionChange.Begin();
			try
			{
				ItemsControl.ItemInfo info = base.NewItemInfo(item, UI, -1);
				if (selected)
				{
					this.SelectionChange.Select(info, true);
				}
				else
				{
					this.SelectionChange.Unselect(info);
				}
			}
			finally
			{
				this.SelectionChange.End();
			}
		}

		// Token: 0x06005FFC RID: 24572 RVA: 0x001AE9D4 File Offset: 0x001ACBD4
		private void OnCurrentChanged(object sender, EventArgs e)
		{
			if (this.IsSynchronizedWithCurrentItemPrivate)
			{
				this.SetSelectedToCurrent();
			}
		}

		// Token: 0x06005FFD RID: 24573 RVA: 0x001AE9E4 File Offset: 0x001ACBE4
		private void OnNewContainer()
		{
			if (this._cacheValid[64])
			{
				return;
			}
			this._cacheValid[64] = true;
			base.LayoutUpdated += this.OnLayoutUpdated;
		}

		// Token: 0x06005FFE RID: 24574 RVA: 0x001AEA16 File Offset: 0x001ACC16
		private void OnLayoutUpdated(object sender, EventArgs e)
		{
			this.AdjustNewContainers();
		}

		// Token: 0x06005FFF RID: 24575 RVA: 0x001AEA1E File Offset: 0x001ACC1E
		private void OnGeneratorStatusChanged(object sender, EventArgs e)
		{
			if (base.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
			{
				this.AdjustNewContainers();
			}
		}

		// Token: 0x06006000 RID: 24576 RVA: 0x001AEA34 File Offset: 0x001ACC34
		private void AdjustNewContainers()
		{
			if (this._cacheValid[64])
			{
				base.LayoutUpdated -= this.OnLayoutUpdated;
				this._cacheValid[64] = false;
			}
			this.AdjustItemInfosAfterGeneratorChangeOverride();
			if (base.HasItems)
			{
				this.SelectionChange.Begin();
				try
				{
					for (int i = 0; i < this._selectedItems.Count; i++)
					{
						this.ItemSetIsSelected(this._selectedItems[i], true);
					}
				}
				finally
				{
					this.SelectionChange.Cancel();
				}
			}
		}

		// Token: 0x06006001 RID: 24577 RVA: 0x001AEAD0 File Offset: 0x001ACCD0
		internal virtual void AdjustItemInfosAfterGeneratorChangeOverride()
		{
			base.AdjustItemInfosAfterGeneratorChange(this._selectedItems, true);
		}

		// Token: 0x06006002 RID: 24578 RVA: 0x001AEAE0 File Offset: 0x001ACCE0
		private void SetSelectedToCurrent()
		{
			if (!this._cacheValid[1])
			{
				this._cacheValid[1] = true;
				try
				{
					object currentItem = base.Items.CurrentItem;
					if (currentItem != null && Selector.ItemGetIsSelectable(currentItem))
					{
						this.SelectionChange.SelectJustThisItem(base.NewItemInfo(currentItem, null, base.Items.CurrentPosition), true);
					}
					else
					{
						this.SelectionChange.SelectJustThisItem(null, false);
					}
				}
				finally
				{
					this._cacheValid[1] = false;
				}
			}
		}

		// Token: 0x06006003 RID: 24579 RVA: 0x001AEB70 File Offset: 0x001ACD70
		private void SetCurrentToSelected()
		{
			if (!this._cacheValid[1])
			{
				this._cacheValid[1] = true;
				try
				{
					if (this._selectedItems.Count == 0)
					{
						base.Items.MoveCurrentToPosition(-1);
					}
					else
					{
						int index = this._selectedItems[0].Index;
						if (index >= 0)
						{
							base.Items.MoveCurrentToPosition(index);
						}
						else
						{
							base.Items.MoveCurrentTo(this.InternalSelectedItem);
						}
					}
				}
				finally
				{
					this._cacheValid[1] = false;
				}
			}
		}

		// Token: 0x06006004 RID: 24580 RVA: 0x001AEC0C File Offset: 0x001ACE0C
		private void UpdateSelectedItems()
		{
			SelectedItemCollection selectedItemCollection = (SelectedItemCollection)this.SelectedItemsImpl;
			if (selectedItemCollection != null)
			{
				Selector.InternalSelectedItemsStorage internalSelectedItemsStorage = new Selector.InternalSelectedItemsStorage(0, Selector.MatchExplicitEqualityComparer);
				Selector.InternalSelectedItemsStorage internalSelectedItemsStorage2 = new Selector.InternalSelectedItemsStorage(selectedItemCollection.Count, Selector.MatchExplicitEqualityComparer);
				internalSelectedItemsStorage.UsesItemHashCodes = this._selectedItems.UsesItemHashCodes;
				internalSelectedItemsStorage2.UsesItemHashCodes = this._selectedItems.UsesItemHashCodes;
				for (int i = 0; i < selectedItemCollection.Count; i++)
				{
					internalSelectedItemsStorage2.Add(selectedItemCollection[i], ItemsControl.ItemInfo.SentinelContainer, ~i);
				}
				using (internalSelectedItemsStorage2.DeferRemove())
				{
					ItemsControl.ItemInfo itemInfo = new ItemsControl.ItemInfo(null, null, -1);
					foreach (ItemsControl.ItemInfo itemInfo2 in ((IEnumerable<ItemsControl.ItemInfo>)this._selectedItems))
					{
						itemInfo.Reset(itemInfo2.Item);
						if (internalSelectedItemsStorage2.Contains(itemInfo))
						{
							internalSelectedItemsStorage2.Remove(itemInfo);
						}
						else
						{
							internalSelectedItemsStorage.Add(itemInfo2);
						}
					}
				}
				if (internalSelectedItemsStorage.Count > 0 || internalSelectedItemsStorage2.Count > 0)
				{
					if (selectedItemCollection.IsChanging)
					{
						Selector.ChangeInfoField.SetValue(this, new Selector.ChangeInfo(internalSelectedItemsStorage, internalSelectedItemsStorage2));
						return;
					}
					this.UpdateSelectedItems(internalSelectedItemsStorage, internalSelectedItemsStorage2);
				}
			}
		}

		// Token: 0x06006005 RID: 24581 RVA: 0x001AED58 File Offset: 0x001ACF58
		internal void FinishSelectedItemsChange()
		{
			Selector.ChangeInfo value = Selector.ChangeInfoField.GetValue(this);
			if (value != null)
			{
				bool isActive = this.SelectionChange.IsActive;
				if (!isActive)
				{
					this.SelectionChange.Begin();
				}
				this.UpdateSelectedItems(value.ToAdd, value.ToRemove);
				if (!isActive)
				{
					this.SelectionChange.End();
				}
			}
		}

		// Token: 0x06006006 RID: 24582 RVA: 0x001AEDB0 File Offset: 0x001ACFB0
		private void UpdateSelectedItems(Selector.InternalSelectedItemsStorage toAdd, Selector.InternalSelectedItemsStorage toRemove)
		{
			IList selectedItemsImpl = this.SelectedItemsImpl;
			Selector.ChangeInfoField.ClearValue(this);
			for (int i = 0; i < toAdd.Count; i++)
			{
				selectedItemsImpl.Add(toAdd[i].Item);
			}
			for (int j = toRemove.Count - 1; j >= 0; j--)
			{
				selectedItemsImpl.RemoveAt(~toRemove[j].Index);
			}
		}

		// Token: 0x06006007 RID: 24583 RVA: 0x001AEE1C File Offset: 0x001AD01C
		internal void UpdatePublicSelectionProperties()
		{
			EffectiveValueEntry valueEntry = base.GetValueEntry(base.LookupEntry(Selector.SelectedIndexProperty.GlobalIndex), Selector.SelectedIndexProperty, null, RequestFlags.DeferredReferences);
			if (!valueEntry.IsDeferredReference)
			{
				int num = (int)valueEntry.Value;
				if (num > base.Items.Count - 1 || (num == -1 && this._selectedItems.Count > 0) || (num > -1 && (this._selectedItems.Count == 0 || num != this._selectedItems[0].Index)))
				{
					base.SetCurrentDeferredValue(Selector.SelectedIndexProperty, new DeferredSelectedIndexReference(this));
				}
			}
			if (this.SelectedItem != this.InternalSelectedItem)
			{
				try
				{
					this.SkipCoerceSelectedItemCheck = true;
					base.SetCurrentValueInternal(Selector.SelectedItemProperty, this.InternalSelectedItem);
				}
				finally
				{
					this.SkipCoerceSelectedItemCheck = false;
				}
			}
			if (this._selectedItems.Count > 0)
			{
				this._cacheValid[32] = false;
			}
			if (!this._cacheValid[16] && !this._cacheValid[32])
			{
				object obj = this.InternalSelectedValue;
				if (obj == DependencyProperty.UnsetValue)
				{
					obj = null;
				}
				if (!object.Equals(this.SelectedValue, obj))
				{
					base.SetCurrentValueInternal(Selector.SelectedValueProperty, obj);
				}
			}
			this.UpdateSelectedItems();
		}

		// Token: 0x06006008 RID: 24584 RVA: 0x001AEF60 File Offset: 0x001AD160
		private void InvokeSelectionChanged(List<ItemsControl.ItemInfo> unselectedInfos, List<ItemsControl.ItemInfo> selectedInfos)
		{
			this.OnSelectionChanged(new SelectionChangedEventArgs(unselectedInfos, selectedInfos)
			{
				Source = this
			});
		}

		// Token: 0x06006009 RID: 24585 RVA: 0x001AEF84 File Offset: 0x001AD184
		private bool InfoGetIsSelected(ItemsControl.ItemInfo info)
		{
			DependencyObject container = info.Container;
			if (container != null)
			{
				return (bool)container.GetValue(Selector.IsSelectedProperty);
			}
			if (this.IsItemItsOwnContainerOverride(info.Item))
			{
				DependencyObject dependencyObject = info.Item as DependencyObject;
				if (dependencyObject != null)
				{
					return (bool)dependencyObject.GetValue(Selector.IsSelectedProperty);
				}
			}
			return false;
		}

		// Token: 0x0600600A RID: 24586 RVA: 0x001AEFDC File Offset: 0x001AD1DC
		private void ItemSetIsSelected(ItemsControl.ItemInfo info, bool value)
		{
			if (info == null)
			{
				return;
			}
			DependencyObject container = info.Container;
			if (container != null && container != ItemsControl.ItemInfo.RemovedContainer)
			{
				if (Selector.GetIsSelected(container) != value)
				{
					container.SetCurrentValueInternal(Selector.IsSelectedProperty, BooleanBoxes.Box(value));
					return;
				}
			}
			else
			{
				object item = info.Item;
				if (this.IsItemItsOwnContainerOverride(item))
				{
					DependencyObject dependencyObject = item as DependencyObject;
					if (dependencyObject != null && Selector.GetIsSelected(dependencyObject) != value)
					{
						dependencyObject.SetCurrentValueInternal(Selector.IsSelectedProperty, BooleanBoxes.Box(value));
					}
				}
			}
		}

		// Token: 0x0600600B RID: 24587 RVA: 0x001AF054 File Offset: 0x001AD254
		internal static bool ItemGetIsSelectable(object item)
		{
			return item != null && !(item is Separator);
		}

		// Token: 0x0600600C RID: 24588 RVA: 0x001AF068 File Offset: 0x001AD268
		internal static bool UiGetIsSelectable(DependencyObject o)
		{
			if (o != null)
			{
				if (!Selector.ItemGetIsSelectable(o))
				{
					return false;
				}
				ItemsControl itemsControl = ItemsControl.ItemsControlFromItemContainer(o);
				if (itemsControl != null)
				{
					object obj = itemsControl.ItemContainerGenerator.ItemFromContainer(o);
					return obj == o || Selector.ItemGetIsSelectable(obj);
				}
			}
			return false;
		}

		// Token: 0x0600600D RID: 24589 RVA: 0x001AF0A7 File Offset: 0x001AD2A7
		private static void OnSelected(object sender, RoutedEventArgs e)
		{
			((Selector)sender).NotifyIsSelectedChanged(e.OriginalSource as FrameworkElement, true, e);
		}

		// Token: 0x0600600E RID: 24590 RVA: 0x001AF0C1 File Offset: 0x001AD2C1
		private static void OnUnselected(object sender, RoutedEventArgs e)
		{
			((Selector)sender).NotifyIsSelectedChanged(e.OriginalSource as FrameworkElement, false, e);
		}

		// Token: 0x0600600F RID: 24591 RVA: 0x001AF0DC File Offset: 0x001AD2DC
		private bool IsInDataGrid()
		{
			FrameworkElement frameworkElement = this;
			ItemsControl itemsControl = null;
			while (frameworkElement != null)
			{
				itemsControl = ItemsControl.ItemsControlFromItemContainer(frameworkElement);
				if (itemsControl != null)
				{
					break;
				}
				frameworkElement = (VisualTreeHelper.GetParent(frameworkElement) as FrameworkElement);
			}
			return itemsControl is DataGrid || itemsControl is DataGridCellsPresenter;
		}

		// Token: 0x06006010 RID: 24592 RVA: 0x001AF11C File Offset: 0x001AD31C
		internal void NotifyIsSelectedChanged(FrameworkElement container, bool selected, RoutedEventArgs e)
		{
			if (this.SelectionChange.IsActive || container == this._clearingContainer)
			{
				e.Handled = true;
				return;
			}
			if (container != null)
			{
				object itemOrContainerFromContainer = base.GetItemOrContainerFromContainer(container);
				if (itemOrContainerFromContainer != DependencyProperty.UnsetValue)
				{
					this.SetSelectedHelper(itemOrContainerFromContainer, container, selected);
					e.Handled = true;
				}
			}
		}

		// Token: 0x17001718 RID: 5912
		// (get) Token: 0x06006011 RID: 24593 RVA: 0x001AF16A File Offset: 0x001AD36A
		internal Selector.SelectionChanger SelectionChange
		{
			get
			{
				if (this._selectionChangeInstance == null)
				{
					this._selectionChangeInstance = new Selector.SelectionChanger(this);
				}
				return this._selectionChangeInstance;
			}
		}

		// Token: 0x06006012 RID: 24594 RVA: 0x001AF186 File Offset: 0x001AD386
		private void ResetSelectedItemsAlgorithm()
		{
			if (!base.Items.IsEmpty)
			{
				this._selectedItems.UsesItemHashCodes = base.Items.CollectionView.HasReliableHashCodes();
			}
		}

		// Token: 0x06006013 RID: 24595 RVA: 0x001AF1B0 File Offset: 0x001AD3B0
		internal void LocateSelectedItems(List<Tuple<int, int>> ranges = null, bool deselectMissingItems = false)
		{
			List<int> list = new List<int>(this._selectedItems.Count);
			int num = 0;
			foreach (ItemsControl.ItemInfo itemInfo in ((IEnumerable<ItemsControl.ItemInfo>)this._selectedItems))
			{
				if (itemInfo.Index < 0)
				{
					num++;
				}
				else
				{
					list.Add(itemInfo.Index);
				}
			}
			int count = list.Count;
			list.Sort();
			ItemsControl.ItemInfo itemInfo2 = new ItemsControl.ItemInfo(null, ItemsControl.ItemInfo.KeyContainer, -1);
			int num2 = 0;
			while (num > 0 && num2 < base.Items.Count)
			{
				if (list.BinarySearch(0, count, num2, null) < 0)
				{
					itemInfo2.Reset(base.Items[num2]);
					itemInfo2.Index = num2;
					ItemsControl.ItemInfo itemInfo3 = this._selectedItems.FindMatch(itemInfo2);
					if (itemInfo3 != null)
					{
						itemInfo3.Index = num2;
						list.Add(num2);
						num--;
					}
				}
				num2++;
			}
			if (ranges != null)
			{
				ranges.Clear();
				list.Sort();
				list.Add(-1);
				int num3 = -1;
				int num4 = -2;
				foreach (int num5 in list)
				{
					if (num5 == num4 + 1)
					{
						num4 = num5;
					}
					else
					{
						if (num3 >= 0)
						{
							ranges.Add(new Tuple<int, int>(num3, num4 - num3 + 1));
						}
						num4 = (num3 = num5);
					}
				}
			}
			if (deselectMissingItems)
			{
				foreach (ItemsControl.ItemInfo itemInfo4 in ((IEnumerable<ItemsControl.ItemInfo>)this._selectedItems))
				{
					if (itemInfo4.Index < 0)
					{
						itemInfo4.Container = ItemsControl.ItemInfo.RemovedContainer;
						this.SelectionChange.Unselect(itemInfo4);
					}
				}
			}
		}

		// Token: 0x17001719 RID: 5913
		// (get) Token: 0x06006014 RID: 24596 RVA: 0x001AF3A0 File Offset: 0x001AD5A0
		internal object InternalSelectedItem
		{
			get
			{
				if (this._selectedItems.Count != 0)
				{
					return this._selectedItems[0].Item;
				}
				return null;
			}
		}

		// Token: 0x1700171A RID: 5914
		// (get) Token: 0x06006015 RID: 24597 RVA: 0x001AF3C2 File Offset: 0x001AD5C2
		internal ItemsControl.ItemInfo InternalSelectedInfo
		{
			get
			{
				if (this._selectedItems.Count != 0)
				{
					return this._selectedItems[0];
				}
				return null;
			}
		}

		// Token: 0x1700171B RID: 5915
		// (get) Token: 0x06006016 RID: 24598 RVA: 0x001AF3E0 File Offset: 0x001AD5E0
		internal int InternalSelectedIndex
		{
			get
			{
				if (this._selectedItems.Count == 0)
				{
					return -1;
				}
				int num = this._selectedItems[0].Index;
				if (num < 0)
				{
					num = base.Items.IndexOf(this._selectedItems[0].Item);
					this._selectedItems[0].Index = num;
				}
				return num;
			}
		}

		// Token: 0x1700171C RID: 5916
		// (get) Token: 0x06006017 RID: 24599 RVA: 0x001AF444 File Offset: 0x001AD644
		private object InternalSelectedValue
		{
			get
			{
				object internalSelectedItem = this.InternalSelectedItem;
				object result;
				if (internalSelectedItem != null)
				{
					BindingExpression bindingExpression = this.PrepareItemValueBinding(internalSelectedItem);
					if (string.IsNullOrEmpty(this.SelectedValuePath))
					{
						string path = bindingExpression.ParentBinding.Path.Path;
						if (string.IsNullOrEmpty(path))
						{
							result = internalSelectedItem;
						}
						else
						{
							result = SystemXmlHelper.GetInnerText(internalSelectedItem);
						}
					}
					else
					{
						bindingExpression.Activate(internalSelectedItem);
						result = bindingExpression.Value;
						bindingExpression.Deactivate();
					}
				}
				else
				{
					result = DependencyProperty.UnsetValue;
				}
				return result;
			}
		}

		/// <summary>Identifies the <see cref="E:System.Windows.Controls.Primitives.Selector.Selected" /> routed event.</summary>
		/// <returns>The identifier for the <see cref="E:System.Windows.Controls.Primitives.Selector.Selected" /> routed event.</returns>
		// Token: 0x040030D5 RID: 12501
		public static readonly RoutedEvent SelectedEvent;

		/// <summary>Identifies the <see cref="E:System.Windows.Controls.Primitives.Selector.Unselected" /> routed event. </summary>
		/// <returns>The identifier for the <see cref="E:System.Windows.Controls.Primitives.Selector.Unselected" /> routed event.</returns>
		// Token: 0x040030D6 RID: 12502
		public static readonly RoutedEvent UnselectedEvent;

		// Token: 0x040030D7 RID: 12503
		internal static readonly DependencyPropertyKey IsSelectionActivePropertyKey;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.Selector.IsSelectionActive" /> attached property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.Selector.IsSelectionActive" /> attached property.</returns>
		// Token: 0x040030D8 RID: 12504
		public static readonly DependencyProperty IsSelectionActiveProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.Selector.IsSelected" /> attached property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.Selector.IsSelected" /> attached property.</returns>
		// Token: 0x040030D9 RID: 12505
		public static readonly DependencyProperty IsSelectedProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.Selector.IsSynchronizedWithCurrentItem" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.Selector.IsSynchronizedWithCurrentItem" /> dependency property.</returns>
		// Token: 0x040030DA RID: 12506
		public static readonly DependencyProperty IsSynchronizedWithCurrentItemProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.Selector.SelectedIndex" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.Selector.SelectedIndex" /> dependency property.</returns>
		// Token: 0x040030DB RID: 12507
		public static readonly DependencyProperty SelectedIndexProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.Selector.SelectedItem" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.Selector.SelectedItem" /> dependency property.</returns>
		// Token: 0x040030DC RID: 12508
		public static readonly DependencyProperty SelectedItemProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.Selector.SelectedValue" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.Selector.SelectedValue" /> dependency property.</returns>
		// Token: 0x040030DD RID: 12509
		public static readonly DependencyProperty SelectedValueProperty;

		/// <summary> Identifies the <see cref="P:System.Windows.Controls.Primitives.Selector.SelectedValuePath" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.Selector.SelectedValuePath" /> dependency property.</returns>
		// Token: 0x040030DE RID: 12510
		public static readonly DependencyProperty SelectedValuePathProperty;

		// Token: 0x040030DF RID: 12511
		private static readonly DependencyPropertyKey SelectedItemsPropertyKey;

		// Token: 0x040030E0 RID: 12512
		internal static readonly DependencyProperty SelectedItemsImplProperty;

		// Token: 0x040030E1 RID: 12513
		private static readonly BindingExpressionUncommonField ItemValueBindingExpression;

		// Token: 0x040030E2 RID: 12514
		internal Selector.InternalSelectedItemsStorage _selectedItems = new Selector.InternalSelectedItemsStorage(1, Selector.MatchExplicitEqualityComparer);

		// Token: 0x040030E3 RID: 12515
		private Point _lastMousePosition;

		// Token: 0x040030E4 RID: 12516
		private Selector.SelectionChanger _selectionChangeInstance;

		// Token: 0x040030E5 RID: 12517
		private BitVector32 _cacheValid = new BitVector32(2);

		// Token: 0x040030E6 RID: 12518
		private EventHandler _focusEnterMainFocusScopeEventHandler;

		// Token: 0x040030E7 RID: 12519
		private DependencyObject _clearingContainer;

		// Token: 0x040030E8 RID: 12520
		private static readonly UncommonField<ItemsControl.ItemInfo> PendingSelectionByValueField;

		// Token: 0x040030E9 RID: 12521
		private static readonly Selector.ItemInfoEqualityComparer MatchExplicitEqualityComparer;

		// Token: 0x040030EA RID: 12522
		private static readonly Selector.ItemInfoEqualityComparer MatchUnresolvedEqualityComparer;

		// Token: 0x040030EB RID: 12523
		private static readonly UncommonField<Selector.ChangeInfo> ChangeInfoField;

		// Token: 0x020009F3 RID: 2547
		[Flags]
		private enum CacheBits
		{
			// Token: 0x04004691 RID: 18065
			SyncingSelectionAndCurrency = 1,
			// Token: 0x04004692 RID: 18066
			CanSelectMultiple = 2,
			// Token: 0x04004693 RID: 18067
			IsSynchronizedWithCurrentItem = 4,
			// Token: 0x04004694 RID: 18068
			SkipCoerceSelectedItemCheck = 8,
			// Token: 0x04004695 RID: 18069
			SelectedValueDrivesSelection = 16,
			// Token: 0x04004696 RID: 18070
			SelectedValueWaitsForItems = 32,
			// Token: 0x04004697 RID: 18071
			NewContainersArePending = 64
		}

		// Token: 0x020009F4 RID: 2548
		internal class SelectionChanger
		{
			// Token: 0x0600899C RID: 35228 RVA: 0x00255B40 File Offset: 0x00253D40
			internal SelectionChanger(Selector s)
			{
				this._owner = s;
				this._active = false;
				this._toSelect = new Selector.InternalSelectedItemsStorage(1, Selector.MatchUnresolvedEqualityComparer);
				this._toUnselect = new Selector.InternalSelectedItemsStorage(1, Selector.MatchUnresolvedEqualityComparer);
				this._toDeferSelect = new Selector.InternalSelectedItemsStorage(1, Selector.MatchUnresolvedEqualityComparer);
			}

			// Token: 0x17001F1A RID: 7962
			// (get) Token: 0x0600899D RID: 35229 RVA: 0x00255B94 File Offset: 0x00253D94
			internal bool IsActive
			{
				get
				{
					return this._active;
				}
			}

			// Token: 0x0600899E RID: 35230 RVA: 0x00255B9C File Offset: 0x00253D9C
			internal void Begin()
			{
				this._active = true;
				this._toSelect.Clear();
				this._toUnselect.Clear();
			}

			// Token: 0x0600899F RID: 35231 RVA: 0x00255BBC File Offset: 0x00253DBC
			internal void End()
			{
				List<ItemsControl.ItemInfo> list = new List<ItemsControl.ItemInfo>();
				List<ItemsControl.ItemInfo> list2 = new List<ItemsControl.ItemInfo>();
				try
				{
					this.ApplyCanSelectMultiple();
					this.CreateDeltaSelectionChange(list, list2);
					this._owner.UpdatePublicSelectionProperties();
				}
				finally
				{
					this.Cleanup();
				}
				if (list.Count > 0 || list2.Count > 0)
				{
					if (this._owner.IsSynchronizedWithCurrentItemPrivate)
					{
						this._owner.SetCurrentToSelected();
					}
					this._owner.InvokeSelectionChanged(list, list2);
				}
			}

			// Token: 0x060089A0 RID: 35232 RVA: 0x00255C40 File Offset: 0x00253E40
			private void ApplyCanSelectMultiple()
			{
				if (!this._owner.CanSelectMultiple)
				{
					if (this._toSelect.Count == 1)
					{
						this._toUnselect = new Selector.InternalSelectedItemsStorage(this._owner._selectedItems, null);
						return;
					}
					if (this._owner._selectedItems.Count > 1 && this._owner._selectedItems.Count != this._toUnselect.Count + 1)
					{
						ItemsControl.ItemInfo info = this._owner._selectedItems[0];
						this._toUnselect.Clear();
						foreach (ItemsControl.ItemInfo itemInfo in ((IEnumerable<ItemsControl.ItemInfo>)this._owner._selectedItems))
						{
							if (itemInfo != info)
							{
								this._toUnselect.Add(itemInfo);
							}
						}
					}
				}
			}

			// Token: 0x060089A1 RID: 35233 RVA: 0x00255D28 File Offset: 0x00253F28
			private void CreateDeltaSelectionChange(List<ItemsControl.ItemInfo> unselectedItems, List<ItemsControl.ItemInfo> selectedItems)
			{
				for (int i = 0; i < this._toDeferSelect.Count; i++)
				{
					ItemsControl.ItemInfo itemInfo = this._toDeferSelect[i];
					if (this._owner.Items.Contains(itemInfo.Item))
					{
						this._toSelect.Add(itemInfo);
						this._toDeferSelect.Remove(itemInfo);
						i--;
					}
				}
				if (this._toUnselect.Count > 0 || this._toSelect.Count > 0)
				{
					using (this._owner._selectedItems.DeferRemove())
					{
						if (this._toUnselect.ResolvedCount > 0)
						{
							foreach (ItemsControl.ItemInfo itemInfo2 in ((IEnumerable<ItemsControl.ItemInfo>)this._toUnselect))
							{
								if (itemInfo2.IsResolved)
								{
									this._owner.ItemSetIsSelected(itemInfo2, false);
									if (this._owner._selectedItems.Remove(itemInfo2))
									{
										unselectedItems.Add(itemInfo2);
									}
								}
							}
						}
						if (this._toUnselect.UnresolvedCount > 0)
						{
							foreach (ItemsControl.ItemInfo itemInfo3 in ((IEnumerable<ItemsControl.ItemInfo>)this._toUnselect))
							{
								if (!itemInfo3.IsResolved)
								{
									ItemsControl.ItemInfo itemInfo4 = this._owner._selectedItems.FindMatch(ItemsControl.ItemInfo.Key(itemInfo3));
									if (itemInfo4 != null)
									{
										this._owner.ItemSetIsSelected(itemInfo4, false);
										this._owner._selectedItems.Remove(itemInfo4);
										unselectedItems.Add(itemInfo4);
									}
								}
							}
						}
					}
					using (this._toSelect.DeferRemove())
					{
						if (this._toSelect.ResolvedCount > 0)
						{
							List<ItemsControl.ItemInfo> list = (this._toSelect.UnresolvedCount > 0) ? new List<ItemsControl.ItemInfo>() : null;
							foreach (ItemsControl.ItemInfo itemInfo5 in ((IEnumerable<ItemsControl.ItemInfo>)this._toSelect))
							{
								if (itemInfo5.IsResolved)
								{
									this._owner.ItemSetIsSelected(itemInfo5, true);
									if (!this._owner._selectedItems.Contains(itemInfo5))
									{
										this._owner._selectedItems.Add(itemInfo5);
										selectedItems.Add(itemInfo5);
									}
									if (list != null)
									{
										list.Add(itemInfo5);
									}
								}
							}
							if (list != null)
							{
								foreach (ItemsControl.ItemInfo e in list)
								{
									this._toSelect.Remove(e);
								}
							}
						}
						int num = 0;
						while (this._toSelect.UnresolvedCount > 0 && num < this._owner.Items.Count)
						{
							ItemsControl.ItemInfo itemInfo6 = this._owner.NewItemInfo(this._owner.Items[num], null, num);
							ItemsControl.ItemInfo e2 = new ItemsControl.ItemInfo(itemInfo6.Item, ItemsControl.ItemInfo.KeyContainer, -1);
							if (this._toSelect.Contains(e2) && !this._owner._selectedItems.Contains(itemInfo6))
							{
								this._owner.ItemSetIsSelected(itemInfo6, true);
								this._owner._selectedItems.Add(itemInfo6);
								selectedItems.Add(itemInfo6);
								this._toSelect.Remove(e2);
							}
							num++;
						}
					}
				}
			}

			// Token: 0x060089A2 RID: 35234 RVA: 0x00256124 File Offset: 0x00254324
			internal bool Select(ItemsControl.ItemInfo info, bool assumeInItemsCollection)
			{
				if (!Selector.ItemGetIsSelectable(info))
				{
					return false;
				}
				if (!assumeInItemsCollection && !this._owner.Items.Contains(info.Item))
				{
					if (!this._toDeferSelect.Contains(info))
					{
						this._toDeferSelect.Add(info);
					}
					return false;
				}
				ItemsControl.ItemInfo itemInfo = ItemsControl.ItemInfo.Key(info);
				if (this._toUnselect.Remove(itemInfo))
				{
					return true;
				}
				if (this._owner._selectedItems.Contains(info))
				{
					return false;
				}
				if (!itemInfo.IsKey && this._toSelect.Contains(itemInfo))
				{
					return false;
				}
				if (!this._owner.CanSelectMultiple && this._toSelect.Count > 0)
				{
					foreach (ItemsControl.ItemInfo info2 in ((IEnumerable<ItemsControl.ItemInfo>)this._toSelect))
					{
						this._owner.ItemSetIsSelected(info2, false);
					}
					this._toSelect.Clear();
				}
				this._toSelect.Add(info);
				return true;
			}

			// Token: 0x060089A3 RID: 35235 RVA: 0x00256230 File Offset: 0x00254430
			internal bool Unselect(ItemsControl.ItemInfo info)
			{
				ItemsControl.ItemInfo e = ItemsControl.ItemInfo.Key(info);
				this._toDeferSelect.Remove(info);
				if (this._toSelect.Remove(e))
				{
					return true;
				}
				if (!this._owner._selectedItems.Contains(e))
				{
					return false;
				}
				if (this._toUnselect.Contains(info))
				{
					return false;
				}
				this._toUnselect.Add(info);
				return true;
			}

			// Token: 0x060089A4 RID: 35236 RVA: 0x00256293 File Offset: 0x00254493
			internal void Validate()
			{
				this.Begin();
				this.End();
			}

			// Token: 0x060089A5 RID: 35237 RVA: 0x002562A1 File Offset: 0x002544A1
			internal void Cancel()
			{
				this.Cleanup();
			}

			// Token: 0x060089A6 RID: 35238 RVA: 0x002562A9 File Offset: 0x002544A9
			internal void CleanupDeferSelection()
			{
				if (this._toDeferSelect.Count > 0)
				{
					this._toDeferSelect.Clear();
				}
			}

			// Token: 0x060089A7 RID: 35239 RVA: 0x002562C4 File Offset: 0x002544C4
			internal void Cleanup()
			{
				this._active = false;
				if (this._toSelect.Count > 0)
				{
					this._toSelect.Clear();
				}
				if (this._toUnselect.Count > 0)
				{
					this._toUnselect.Clear();
				}
			}

			// Token: 0x060089A8 RID: 35240 RVA: 0x00256300 File Offset: 0x00254500
			internal void SelectJustThisItem(ItemsControl.ItemInfo info, bool assumeInItemsCollection)
			{
				this.Begin();
				this.CleanupDeferSelection();
				try
				{
					bool flag = false;
					for (int i = this._owner._selectedItems.Count - 1; i >= 0; i--)
					{
						if (info != this._owner._selectedItems[i])
						{
							this.Unselect(this._owner._selectedItems[i]);
						}
						else
						{
							flag = true;
						}
					}
					if (!flag && info != null && info.Item != DependencyProperty.UnsetValue)
					{
						this.Select(info, assumeInItemsCollection);
					}
				}
				finally
				{
					this.End();
				}
			}

			// Token: 0x04004698 RID: 18072
			private Selector _owner;

			// Token: 0x04004699 RID: 18073
			private Selector.InternalSelectedItemsStorage _toSelect;

			// Token: 0x0400469A RID: 18074
			private Selector.InternalSelectedItemsStorage _toUnselect;

			// Token: 0x0400469B RID: 18075
			private Selector.InternalSelectedItemsStorage _toDeferSelect;

			// Token: 0x0400469C RID: 18076
			private bool _active;
		}

		// Token: 0x020009F5 RID: 2549
		internal class InternalSelectedItemsStorage : IEnumerable<ItemsControl.ItemInfo>, IEnumerable
		{
			// Token: 0x060089A9 RID: 35241 RVA: 0x002563A8 File Offset: 0x002545A8
			internal InternalSelectedItemsStorage(int capacity, IEqualityComparer<ItemsControl.ItemInfo> equalityComparer)
			{
				this._equalityComparer = equalityComparer;
				this._list = new List<ItemsControl.ItemInfo>(capacity);
				this._set = new Dictionary<ItemsControl.ItemInfo, ItemsControl.ItemInfo>(capacity, equalityComparer);
			}

			// Token: 0x060089AA RID: 35242 RVA: 0x002563D0 File Offset: 0x002545D0
			internal InternalSelectedItemsStorage(Selector.InternalSelectedItemsStorage collection, IEqualityComparer<ItemsControl.ItemInfo> equalityComparer = null)
			{
				this._equalityComparer = (equalityComparer ?? collection._equalityComparer);
				this._list = new List<ItemsControl.ItemInfo>(collection._list);
				if (collection.UsesItemHashCodes)
				{
					this._set = new Dictionary<ItemsControl.ItemInfo, ItemsControl.ItemInfo>(collection._set, this._equalityComparer);
				}
				this._resolvedCount = collection._resolvedCount;
				this._unresolvedCount = collection._unresolvedCount;
			}

			// Token: 0x060089AB RID: 35243 RVA: 0x0025643C File Offset: 0x0025463C
			public void Add(object item, DependencyObject container, int index)
			{
				this.Add(new ItemsControl.ItemInfo(item, container, index));
			}

			// Token: 0x060089AC RID: 35244 RVA: 0x0025644C File Offset: 0x0025464C
			public void Add(ItemsControl.ItemInfo info)
			{
				if (this._set != null)
				{
					this._set.Add(info, info);
				}
				this._list.Add(info);
				if (info.IsResolved)
				{
					this._resolvedCount++;
					return;
				}
				this._unresolvedCount++;
			}

			// Token: 0x060089AD RID: 35245 RVA: 0x002564A0 File Offset: 0x002546A0
			public bool Remove(ItemsControl.ItemInfo e)
			{
				bool flag = false;
				bool flag2 = false;
				if (this._set != null)
				{
					ItemsControl.ItemInfo itemInfo;
					if (this._set.TryGetValue(e, out itemInfo))
					{
						flag = true;
						flag2 = itemInfo.IsResolved;
						this._set.Remove(e);
						if (this.RemoveIsDeferred)
						{
							itemInfo.Container = ItemsControl.ItemInfo.RemovedContainer;
							Selector.InternalSelectedItemsStorage.BatchRemoveHelper batchRemove = this._batchRemove;
							int removedCount = batchRemove.RemovedCount + 1;
							batchRemove.RemovedCount = removedCount;
						}
						else
						{
							this.RemoveFromList(e);
						}
					}
				}
				else
				{
					flag = this.RemoveFromList(e);
				}
				if (flag)
				{
					if (flag2)
					{
						this._resolvedCount--;
					}
					else
					{
						this._unresolvedCount--;
					}
				}
				return flag;
			}

			// Token: 0x060089AE RID: 35246 RVA: 0x00256540 File Offset: 0x00254740
			private bool RemoveFromList(ItemsControl.ItemInfo e)
			{
				bool result = false;
				int num = this.LastIndexInList(e);
				if (num >= 0)
				{
					this._list.RemoveAt(num);
					result = true;
				}
				return result;
			}

			// Token: 0x060089AF RID: 35247 RVA: 0x0025656A File Offset: 0x0025476A
			public bool Contains(ItemsControl.ItemInfo e)
			{
				if (this._set != null)
				{
					return this._set.ContainsKey(e);
				}
				return this.IndexInList(e) >= 0;
			}

			// Token: 0x17001F1B RID: 7963
			public ItemsControl.ItemInfo this[int index]
			{
				get
				{
					return this._list[index];
				}
			}

			// Token: 0x060089B1 RID: 35249 RVA: 0x0025659C File Offset: 0x0025479C
			public void Clear()
			{
				this._list.Clear();
				if (this._set != null)
				{
					this._set.Clear();
				}
				this._resolvedCount = (this._unresolvedCount = 0);
			}

			// Token: 0x17001F1C RID: 7964
			// (get) Token: 0x060089B2 RID: 35250 RVA: 0x002565D7 File Offset: 0x002547D7
			public int Count
			{
				get
				{
					return this._list.Count;
				}
			}

			// Token: 0x17001F1D RID: 7965
			// (get) Token: 0x060089B3 RID: 35251 RVA: 0x002565E4 File Offset: 0x002547E4
			public bool RemoveIsDeferred
			{
				get
				{
					return this._batchRemove != null && this._batchRemove.IsActive;
				}
			}

			// Token: 0x060089B4 RID: 35252 RVA: 0x002565FB File Offset: 0x002547FB
			public IDisposable DeferRemove()
			{
				if (this._batchRemove == null)
				{
					this._batchRemove = new Selector.InternalSelectedItemsStorage.BatchRemoveHelper(this);
				}
				this._batchRemove.Enter();
				return this._batchRemove;
			}

			// Token: 0x060089B5 RID: 35253 RVA: 0x00256624 File Offset: 0x00254824
			private void DoBatchRemove()
			{
				int num = 0;
				int count = this._list.Count;
				for (int i = 0; i < count; i++)
				{
					ItemsControl.ItemInfo itemInfo = this._list[i];
					if (!itemInfo.IsRemoved)
					{
						if (num < i)
						{
							this._list[num] = this._list[i];
						}
						num++;
					}
				}
				this._list.RemoveRange(num, count - num);
			}

			// Token: 0x17001F1E RID: 7966
			// (get) Token: 0x060089B6 RID: 35254 RVA: 0x0025668F File Offset: 0x0025488F
			public int ResolvedCount
			{
				get
				{
					return this._resolvedCount;
				}
			}

			// Token: 0x17001F1F RID: 7967
			// (get) Token: 0x060089B7 RID: 35255 RVA: 0x00256697 File Offset: 0x00254897
			public int UnresolvedCount
			{
				get
				{
					return this._unresolvedCount;
				}
			}

			// Token: 0x060089B8 RID: 35256 RVA: 0x0025669F File Offset: 0x0025489F
			IEnumerator<ItemsControl.ItemInfo> IEnumerable<ItemsControl.ItemInfo>.GetEnumerator()
			{
				return this._list.GetEnumerator();
			}

			// Token: 0x060089B9 RID: 35257 RVA: 0x0025669F File Offset: 0x0025489F
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this._list.GetEnumerator();
			}

			// Token: 0x17001F20 RID: 7968
			// (get) Token: 0x060089BA RID: 35258 RVA: 0x002566B1 File Offset: 0x002548B1
			// (set) Token: 0x060089BB RID: 35259 RVA: 0x002566BC File Offset: 0x002548BC
			public bool UsesItemHashCodes
			{
				get
				{
					return this._set != null;
				}
				set
				{
					if (value && this._set == null)
					{
						this._set = new Dictionary<ItemsControl.ItemInfo, ItemsControl.ItemInfo>(this._list.Count);
						for (int i = 0; i < this._list.Count; i++)
						{
							this._set.Add(this._list[i], this._list[i]);
						}
						return;
					}
					if (!value)
					{
						this._set = null;
					}
				}
			}

			// Token: 0x060089BC RID: 35260 RVA: 0x00256730 File Offset: 0x00254930
			public ItemsControl.ItemInfo FindMatch(ItemsControl.ItemInfo info)
			{
				ItemsControl.ItemInfo result;
				if (this._set != null)
				{
					if (!this._set.TryGetValue(info, out result))
					{
						result = null;
					}
				}
				else
				{
					int num = this.IndexInList(info);
					result = ((num < 0) ? null : this._list[num]);
				}
				return result;
			}

			// Token: 0x060089BD RID: 35261 RVA: 0x00256778 File Offset: 0x00254978
			private int IndexInList(ItemsControl.ItemInfo info)
			{
				return this._list.FindIndex((ItemsControl.ItemInfo x) => this._equalityComparer.Equals(info, x));
			}

			// Token: 0x060089BE RID: 35262 RVA: 0x002567B0 File Offset: 0x002549B0
			private int LastIndexInList(ItemsControl.ItemInfo info)
			{
				return this._list.FindLastIndex((ItemsControl.ItemInfo x) => this._equalityComparer.Equals(info, x));
			}

			// Token: 0x0400469D RID: 18077
			private List<ItemsControl.ItemInfo> _list;

			// Token: 0x0400469E RID: 18078
			private Dictionary<ItemsControl.ItemInfo, ItemsControl.ItemInfo> _set;

			// Token: 0x0400469F RID: 18079
			private IEqualityComparer<ItemsControl.ItemInfo> _equalityComparer;

			// Token: 0x040046A0 RID: 18080
			private int _resolvedCount;

			// Token: 0x040046A1 RID: 18081
			private int _unresolvedCount;

			// Token: 0x040046A2 RID: 18082
			private Selector.InternalSelectedItemsStorage.BatchRemoveHelper _batchRemove;

			// Token: 0x02000BAF RID: 2991
			private class BatchRemoveHelper : IDisposable
			{
				// Token: 0x060091E8 RID: 37352 RVA: 0x0025F613 File Offset: 0x0025D813
				public BatchRemoveHelper(Selector.InternalSelectedItemsStorage owner)
				{
					this._owner = owner;
				}

				// Token: 0x17001FD9 RID: 8153
				// (get) Token: 0x060091E9 RID: 37353 RVA: 0x0025F622 File Offset: 0x0025D822
				public bool IsActive
				{
					get
					{
						return this._level > 0;
					}
				}

				// Token: 0x17001FDA RID: 8154
				// (get) Token: 0x060091EA RID: 37354 RVA: 0x0025F62D File Offset: 0x0025D82D
				// (set) Token: 0x060091EB RID: 37355 RVA: 0x0025F635 File Offset: 0x0025D835
				public int RemovedCount { get; set; }

				// Token: 0x060091EC RID: 37356 RVA: 0x0025F63E File Offset: 0x0025D83E
				public void Enter()
				{
					this._level++;
				}

				// Token: 0x060091ED RID: 37357 RVA: 0x0025F650 File Offset: 0x0025D850
				public void Leave()
				{
					if (this._level > 0)
					{
						int num = this._level - 1;
						this._level = num;
						if (num == 0 && this.RemovedCount > 0)
						{
							this._owner.DoBatchRemove();
							this.RemovedCount = 0;
						}
					}
				}

				// Token: 0x060091EE RID: 37358 RVA: 0x0025F694 File Offset: 0x0025D894
				public void Dispose()
				{
					this.Leave();
				}

				// Token: 0x04004EE3 RID: 20195
				private Selector.InternalSelectedItemsStorage _owner;

				// Token: 0x04004EE4 RID: 20196
				private int _level;
			}
		}

		// Token: 0x020009F6 RID: 2550
		private class ItemInfoEqualityComparer : IEqualityComparer<ItemsControl.ItemInfo>
		{
			// Token: 0x060089BF RID: 35263 RVA: 0x002567E8 File Offset: 0x002549E8
			public ItemInfoEqualityComparer(bool matchUnresolved)
			{
				this._matchUnresolved = matchUnresolved;
			}

			// Token: 0x060089C0 RID: 35264 RVA: 0x002567F7 File Offset: 0x002549F7
			bool IEqualityComparer<ItemsControl.ItemInfo>.Equals(ItemsControl.ItemInfo x, ItemsControl.ItemInfo y)
			{
				if (x == y)
				{
					return true;
				}
				if (!(x == null))
				{
					return x.Equals(y, this._matchUnresolved);
				}
				return y == null;
			}

			// Token: 0x060089C1 RID: 35265 RVA: 0x0025681D File Offset: 0x00254A1D
			int IEqualityComparer<ItemsControl.ItemInfo>.GetHashCode(ItemsControl.ItemInfo x)
			{
				return x.GetHashCode();
			}

			// Token: 0x040046A3 RID: 18083
			private bool _matchUnresolved;
		}

		// Token: 0x020009F7 RID: 2551
		private class ChangeInfo
		{
			// Token: 0x060089C2 RID: 35266 RVA: 0x00256825 File Offset: 0x00254A25
			public ChangeInfo(Selector.InternalSelectedItemsStorage toAdd, Selector.InternalSelectedItemsStorage toRemove)
			{
				this.ToAdd = toAdd;
				this.ToRemove = toRemove;
			}

			// Token: 0x17001F21 RID: 7969
			// (get) Token: 0x060089C3 RID: 35267 RVA: 0x0025683B File Offset: 0x00254A3B
			// (set) Token: 0x060089C4 RID: 35268 RVA: 0x00256843 File Offset: 0x00254A43
			public Selector.InternalSelectedItemsStorage ToAdd { get; private set; }

			// Token: 0x17001F22 RID: 7970
			// (get) Token: 0x060089C5 RID: 35269 RVA: 0x0025684C File Offset: 0x00254A4C
			// (set) Token: 0x060089C6 RID: 35270 RVA: 0x00256854 File Offset: 0x00254A54
			public Selector.InternalSelectedItemsStorage ToRemove { get; private set; }
		}
	}
}
