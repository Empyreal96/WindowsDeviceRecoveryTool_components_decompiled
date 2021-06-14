using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;
using System.Xaml;
using MS.Internal.Controls;
using MS.Internal.Data;
using MS.Internal.Hashing.PresentationFramework;

namespace MS.Internal
{
	// Token: 0x020005E7 RID: 1511
	internal static class Helper
	{
		// Token: 0x060064BD RID: 25789 RVA: 0x001C4250 File Offset: 0x001C2450
		internal static object ResourceFailureThrow(object key)
		{
			Helper.FindResourceHelper findResourceHelper = new Helper.FindResourceHelper(key);
			return findResourceHelper.TryCatchWhen();
		}

		// Token: 0x060064BE RID: 25790 RVA: 0x001C426C File Offset: 0x001C246C
		internal static object FindTemplateResourceFromAppOrSystem(DependencyObject target, ArrayList keys, int exactMatch, ref int bestMatch)
		{
			object result = null;
			Application application = Application.Current;
			if (application != null)
			{
				for (int i = 0; i < bestMatch; i++)
				{
					object obj = Application.Current.FindResourceInternal(keys[i]);
					if (obj != null)
					{
						bestMatch = i;
						result = obj;
						if (bestMatch < exactMatch)
						{
							return result;
						}
					}
				}
			}
			if (bestMatch >= exactMatch)
			{
				for (int i = 0; i < bestMatch; i++)
				{
					object obj2 = SystemResources.FindResourceInternal(keys[i]);
					if (obj2 != null)
					{
						bestMatch = i;
						result = obj2;
						if (bestMatch < exactMatch)
						{
							return result;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x060064BF RID: 25791 RVA: 0x001C42E4 File Offset: 0x001C24E4
		internal static DependencyObject FindMentor(DependencyObject d)
		{
			while (d != null)
			{
				FrameworkElement frameworkElement;
				FrameworkContentElement frameworkContentElement;
				Helper.DowncastToFEorFCE(d, out frameworkElement, out frameworkContentElement, false);
				if (frameworkElement != null)
				{
					return frameworkElement;
				}
				if (frameworkContentElement != null)
				{
					return frameworkContentElement;
				}
				d = d.InheritanceContext;
			}
			return null;
		}

		// Token: 0x060064C0 RID: 25792 RVA: 0x001C4314 File Offset: 0x001C2514
		internal static bool HasDefaultValue(DependencyObject d, DependencyProperty dp)
		{
			return Helper.HasDefaultOrInheritedValueImpl(d, dp, false, true);
		}

		// Token: 0x060064C1 RID: 25793 RVA: 0x001C431F File Offset: 0x001C251F
		internal static bool HasDefaultOrInheritedValue(DependencyObject d, DependencyProperty dp)
		{
			return Helper.HasDefaultOrInheritedValueImpl(d, dp, true, true);
		}

		// Token: 0x060064C2 RID: 25794 RVA: 0x001C432A File Offset: 0x001C252A
		internal static bool HasUnmodifiedDefaultValue(DependencyObject d, DependencyProperty dp)
		{
			return Helper.HasDefaultOrInheritedValueImpl(d, dp, false, false);
		}

		// Token: 0x060064C3 RID: 25795 RVA: 0x001C4335 File Offset: 0x001C2535
		internal static bool HasUnmodifiedDefaultOrInheritedValue(DependencyObject d, DependencyProperty dp)
		{
			return Helper.HasDefaultOrInheritedValueImpl(d, dp, true, false);
		}

		// Token: 0x060064C4 RID: 25796 RVA: 0x001C4340 File Offset: 0x001C2540
		private static bool HasDefaultOrInheritedValueImpl(DependencyObject d, DependencyProperty dp, bool checkInherited, bool ignoreModifiers)
		{
			PropertyMetadata metadata = dp.GetMetadata(d);
			bool flag;
			BaseValueSourceInternal valueSource = d.GetValueSource(dp, metadata, out flag);
			if (valueSource == BaseValueSourceInternal.Default || (checkInherited && valueSource == BaseValueSourceInternal.Inherited))
			{
				if (ignoreModifiers && (d is FrameworkElement || d is FrameworkContentElement))
				{
					flag = false;
				}
				return !flag;
			}
			return false;
		}

		// Token: 0x060064C5 RID: 25797 RVA: 0x001C4388 File Offset: 0x001C2588
		internal static void DowncastToFEorFCE(DependencyObject d, out FrameworkElement fe, out FrameworkContentElement fce, bool throwIfNeither)
		{
			if (FrameworkElement.DType.IsInstanceOfType(d))
			{
				fe = (FrameworkElement)d;
				fce = null;
				return;
			}
			if (FrameworkContentElement.DType.IsInstanceOfType(d))
			{
				fe = null;
				fce = (FrameworkContentElement)d;
				return;
			}
			if (throwIfNeither)
			{
				throw new InvalidOperationException(SR.Get("MustBeFrameworkDerived", new object[]
				{
					d.GetType()
				}));
			}
			fe = null;
			fce = null;
		}

		// Token: 0x060064C6 RID: 25798 RVA: 0x001C43F0 File Offset: 0x001C25F0
		internal static void CheckStyleAndStyleSelector(string name, DependencyProperty styleProperty, DependencyProperty styleSelectorProperty, DependencyObject d)
		{
			if (TraceData.IsEnabled)
			{
				object obj = d.ReadLocalValue(styleSelectorProperty);
				if (obj != DependencyProperty.UnsetValue && (obj is StyleSelector || obj is ResourceReferenceExpression))
				{
					object obj2 = d.ReadLocalValue(styleProperty);
					if (obj2 != DependencyProperty.UnsetValue && (obj2 is Style || obj2 is ResourceReferenceExpression))
					{
						TraceData.Trace(TraceEventType.Error, TraceData.StyleAndStyleSelectorDefined(new object[]
						{
							name
						}), d);
					}
				}
			}
		}

		// Token: 0x060064C7 RID: 25799 RVA: 0x001C445A File Offset: 0x001C265A
		internal static void CheckTemplateAndTemplateSelector(string name, DependencyProperty templateProperty, DependencyProperty templateSelectorProperty, DependencyObject d)
		{
			if (TraceData.IsEnabled && Helper.IsTemplateSelectorDefined(templateSelectorProperty, d) && Helper.IsTemplateDefined(templateProperty, d))
			{
				TraceData.Trace(TraceEventType.Error, TraceData.TemplateAndTemplateSelectorDefined(new object[]
				{
					name
				}), d);
			}
		}

		// Token: 0x060064C8 RID: 25800 RVA: 0x001C448C File Offset: 0x001C268C
		internal static bool IsTemplateSelectorDefined(DependencyProperty templateSelectorProperty, DependencyObject d)
		{
			object obj = d.ReadLocalValue(templateSelectorProperty);
			return obj != DependencyProperty.UnsetValue && obj != null && (obj is DataTemplateSelector || obj is ResourceReferenceExpression);
		}

		// Token: 0x060064C9 RID: 25801 RVA: 0x001C44C4 File Offset: 0x001C26C4
		internal static bool IsTemplateDefined(DependencyProperty templateProperty, DependencyObject d)
		{
			object obj = d.ReadLocalValue(templateProperty);
			return obj != DependencyProperty.UnsetValue && obj != null && (obj is FrameworkTemplate || obj is ResourceReferenceExpression);
		}

		// Token: 0x060064CA RID: 25802 RVA: 0x001C44FC File Offset: 0x001C26FC
		internal static object FindNameInTemplate(string name, DependencyObject templatedParent)
		{
			FrameworkElement frameworkElement = templatedParent as FrameworkElement;
			return frameworkElement.TemplateInternal.FindName(name, frameworkElement);
		}

		// Token: 0x060064CB RID: 25803 RVA: 0x001C4520 File Offset: 0x001C2720
		internal static IGeneratorHost GeneratorHostForElement(DependencyObject element)
		{
			DependencyObject dependencyObject = null;
			DependencyObject dependencyObject2 = null;
			while (element != null)
			{
				while (element != null)
				{
					dependencyObject = element;
					element = Helper.GetTemplatedParent(element);
					if (dependencyObject is ContentPresenter)
					{
						ComboBox comboBox = element as ComboBox;
						if (comboBox != null)
						{
							return comboBox;
						}
					}
				}
				Visual visual = dependencyObject as Visual;
				if (visual != null)
				{
					dependencyObject2 = VisualTreeHelper.GetParent(visual);
					element = (dependencyObject2 as GridViewRowPresenterBase);
				}
				else
				{
					dependencyObject2 = null;
				}
			}
			if (dependencyObject2 != null)
			{
				ItemsControl itemsOwner = ItemsControl.GetItemsOwner(dependencyObject2);
				if (itemsOwner != null)
				{
					return itemsOwner;
				}
			}
			return null;
		}

		// Token: 0x060064CC RID: 25804 RVA: 0x001C4588 File Offset: 0x001C2788
		internal static DependencyObject GetTemplatedParent(DependencyObject d)
		{
			FrameworkElement frameworkElement;
			FrameworkContentElement frameworkContentElement;
			Helper.DowncastToFEorFCE(d, out frameworkElement, out frameworkContentElement, false);
			if (frameworkElement != null)
			{
				return frameworkElement.TemplatedParent;
			}
			if (frameworkContentElement != null)
			{
				return frameworkContentElement.TemplatedParent;
			}
			return null;
		}

		// Token: 0x060064CD RID: 25805 RVA: 0x001C45B8 File Offset: 0x001C27B8
		internal static XmlDataProvider XmlDataProviderForElement(DependencyObject d)
		{
			IGeneratorHost generatorHost = Helper.GeneratorHostForElement(d);
			ItemCollection itemCollection = (generatorHost != null) ? generatorHost.View : null;
			ICollectionView collectionView = (itemCollection != null) ? itemCollection.CollectionView : null;
			XmlDataCollection xmlDataCollection = (collectionView != null) ? (collectionView.SourceCollection as XmlDataCollection) : null;
			if (xmlDataCollection == null)
			{
				return null;
			}
			return xmlDataCollection.ParentXmlDataProvider;
		}

		// Token: 0x060064CE RID: 25806 RVA: 0x001C4604 File Offset: 0x001C2804
		internal static Size MeasureElementWithSingleChild(UIElement element, Size constraint)
		{
			UIElement uielement = (VisualTreeHelper.GetChildrenCount(element) > 0) ? (VisualTreeHelper.GetChild(element, 0) as UIElement) : null;
			if (uielement != null)
			{
				uielement.Measure(constraint);
				return uielement.DesiredSize;
			}
			return default(Size);
		}

		// Token: 0x060064CF RID: 25807 RVA: 0x001C4644 File Offset: 0x001C2844
		internal static Size ArrangeElementWithSingleChild(UIElement element, Size arrangeSize)
		{
			UIElement uielement = (VisualTreeHelper.GetChildrenCount(element) > 0) ? (VisualTreeHelper.GetChild(element, 0) as UIElement) : null;
			if (uielement != null)
			{
				uielement.Arrange(new Rect(arrangeSize));
			}
			return arrangeSize;
		}

		// Token: 0x060064D0 RID: 25808 RVA: 0x001C467A File Offset: 0x001C287A
		internal static bool IsDoubleValid(double value)
		{
			return !double.IsInfinity(value) && !double.IsNaN(value);
		}

		// Token: 0x060064D1 RID: 25809 RVA: 0x001C4690 File Offset: 0x001C2890
		internal static void CheckCanReceiveMarkupExtension(MarkupExtension markupExtension, IServiceProvider serviceProvider, out DependencyObject targetDependencyObject, out DependencyProperty targetDependencyProperty)
		{
			targetDependencyObject = null;
			targetDependencyProperty = null;
			IProvideValueTarget provideValueTarget = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
			if (provideValueTarget == null)
			{
				return;
			}
			object targetObject = provideValueTarget.TargetObject;
			if (targetObject == null)
			{
				return;
			}
			Type type = targetObject.GetType();
			object targetProperty = provideValueTarget.TargetProperty;
			if (targetProperty != null)
			{
				targetDependencyProperty = (targetProperty as DependencyProperty);
				if (targetDependencyProperty != null)
				{
					targetDependencyObject = (targetObject as DependencyObject);
					return;
				}
				MemberInfo memberInfo = targetProperty as MemberInfo;
				if (memberInfo != null)
				{
					PropertyInfo propertyInfo = memberInfo as PropertyInfo;
					EventHandler<XamlSetMarkupExtensionEventArgs> eventHandler = Helper.LookupSetMarkupExtensionHandler(type);
					if (eventHandler != null && propertyInfo != null)
					{
						IXamlSchemaContextProvider xamlSchemaContextProvider = serviceProvider.GetService(typeof(IXamlSchemaContextProvider)) as IXamlSchemaContextProvider;
						if (xamlSchemaContextProvider != null)
						{
							XamlSchemaContext schemaContext = xamlSchemaContextProvider.SchemaContext;
							XamlType xamlType = schemaContext.GetXamlType(type);
							if (xamlType != null)
							{
								XamlMember member = xamlType.GetMember(propertyInfo.Name);
								if (member != null)
								{
									XamlSetMarkupExtensionEventArgs xamlSetMarkupExtensionEventArgs = new XamlSetMarkupExtensionEventArgs(member, markupExtension, serviceProvider);
									eventHandler(targetObject, xamlSetMarkupExtensionEventArgs);
									if (xamlSetMarkupExtensionEventArgs.Handled)
									{
										return;
									}
								}
							}
						}
					}
					Type type2;
					if (propertyInfo != null)
					{
						type2 = propertyInfo.PropertyType;
					}
					else
					{
						MethodInfo methodInfo = (MethodInfo)memberInfo;
						ParameterInfo[] parameters = methodInfo.GetParameters();
						type2 = parameters[1].ParameterType;
					}
					if (!typeof(MarkupExtension).IsAssignableFrom(type2) || !type2.IsAssignableFrom(markupExtension.GetType()))
					{
						throw new System.Windows.Markup.XamlParseException(SR.Get("MarkupExtensionDynamicOrBindingOnClrProp", new object[]
						{
							markupExtension.GetType().Name,
							memberInfo.Name,
							type.Name
						}));
					}
				}
				else if (!typeof(BindingBase).IsAssignableFrom(markupExtension.GetType()) || !typeof(Collection<BindingBase>).IsAssignableFrom(targetProperty.GetType()))
				{
					throw new System.Windows.Markup.XamlParseException(SR.Get("MarkupExtensionDynamicOrBindingInCollection", new object[]
					{
						markupExtension.GetType().Name,
						targetProperty.GetType().Name
					}));
				}
			}
			else if (!typeof(BindingBase).IsAssignableFrom(markupExtension.GetType()) || !typeof(Collection<BindingBase>).IsAssignableFrom(type))
			{
				throw new System.Windows.Markup.XamlParseException(SR.Get("MarkupExtensionDynamicOrBindingInCollection", new object[]
				{
					markupExtension.GetType().Name,
					type.Name
				}));
			}
		}

		// Token: 0x060064D2 RID: 25810 RVA: 0x001C48E0 File Offset: 0x001C2AE0
		private static EventHandler<XamlSetMarkupExtensionEventArgs> LookupSetMarkupExtensionHandler(Type type)
		{
			if (typeof(Setter).IsAssignableFrom(type))
			{
				return new EventHandler<XamlSetMarkupExtensionEventArgs>(Setter.ReceiveMarkupExtension);
			}
			if (typeof(DataTrigger).IsAssignableFrom(type))
			{
				return new EventHandler<XamlSetMarkupExtensionEventArgs>(DataTrigger.ReceiveMarkupExtension);
			}
			if (typeof(Condition).IsAssignableFrom(type))
			{
				return new EventHandler<XamlSetMarkupExtensionEventArgs>(Condition.ReceiveMarkupExtension);
			}
			return null;
		}

		// Token: 0x060064D3 RID: 25811 RVA: 0x001C494B File Offset: 0x001C2B4B
		internal static string GetEffectiveStringFormat(string stringFormat)
		{
			if (stringFormat.IndexOf('{') < 0)
			{
				stringFormat = "{0:" + stringFormat + "}";
			}
			return stringFormat;
		}

		// Token: 0x060064D4 RID: 25812 RVA: 0x001C496C File Offset: 0x001C2B6C
		internal static object ReadItemValue(DependencyObject owner, object item, int dpIndex)
		{
			if (item != null)
			{
				List<KeyValuePair<int, object>> itemValues = Helper.GetItemValues(owner, item);
				if (itemValues != null)
				{
					for (int i = 0; i < itemValues.Count; i++)
					{
						if (itemValues[i].Key == dpIndex)
						{
							return itemValues[i].Value;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x060064D5 RID: 25813 RVA: 0x001C49BC File Offset: 0x001C2BBC
		internal static void StoreItemValue(DependencyObject owner, object item, int dpIndex, object value)
		{
			if (item != null)
			{
				List<KeyValuePair<int, object>> list = Helper.EnsureItemValues(owner, item);
				bool flag = false;
				KeyValuePair<int, object> keyValuePair = new KeyValuePair<int, object>(dpIndex, value);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].Key == dpIndex)
					{
						list[i] = keyValuePair;
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					list.Add(keyValuePair);
				}
			}
		}

		// Token: 0x060064D6 RID: 25814 RVA: 0x001C4A1C File Offset: 0x001C2C1C
		internal static void ClearItemValue(DependencyObject owner, object item, int dpIndex)
		{
			if (item != null)
			{
				List<KeyValuePair<int, object>> itemValues = Helper.GetItemValues(owner, item);
				if (itemValues != null)
				{
					for (int i = 0; i < itemValues.Count; i++)
					{
						if (itemValues[i].Key == dpIndex)
						{
							itemValues.RemoveAt(i);
							return;
						}
					}
				}
			}
		}

		// Token: 0x060064D7 RID: 25815 RVA: 0x001C4A62 File Offset: 0x001C2C62
		internal static List<KeyValuePair<int, object>> GetItemValues(DependencyObject owner, object item)
		{
			return Helper.GetItemValues(owner, item, Helper.ItemValueStorageField.GetValue(owner));
		}

		// Token: 0x060064D8 RID: 25816 RVA: 0x001C4A78 File Offset: 0x001C2C78
		internal static List<KeyValuePair<int, object>> GetItemValues(DependencyObject owner, object item, WeakDictionary<object, List<KeyValuePair<int, object>>> itemValueStorage)
		{
			List<KeyValuePair<int, object>> result = null;
			if (itemValueStorage != null)
			{
				itemValueStorage.TryGetValue(item, out result);
			}
			return result;
		}

		// Token: 0x060064D9 RID: 25817 RVA: 0x001C4A98 File Offset: 0x001C2C98
		internal static List<KeyValuePair<int, object>> EnsureItemValues(DependencyObject owner, object item)
		{
			WeakDictionary<object, List<KeyValuePair<int, object>>> weakDictionary = Helper.EnsureItemValueStorage(owner);
			List<KeyValuePair<int, object>> list = Helper.GetItemValues(owner, item, weakDictionary);
			if (list == null && HashHelper.HasReliableHashCode(item))
			{
				list = new List<KeyValuePair<int, object>>(3);
				weakDictionary[item] = list;
			}
			return list;
		}

		// Token: 0x060064DA RID: 25818 RVA: 0x001C4AD0 File Offset: 0x001C2CD0
		internal static WeakDictionary<object, List<KeyValuePair<int, object>>> EnsureItemValueStorage(DependencyObject owner)
		{
			WeakDictionary<object, List<KeyValuePair<int, object>>> weakDictionary = Helper.ItemValueStorageField.GetValue(owner);
			if (weakDictionary == null)
			{
				weakDictionary = new WeakDictionary<object, List<KeyValuePair<int, object>>>();
				Helper.ItemValueStorageField.SetValue(owner, weakDictionary);
			}
			return weakDictionary;
		}

		// Token: 0x060064DB RID: 25819 RVA: 0x001C4B00 File Offset: 0x001C2D00
		internal static void SetItemValuesOnContainer(DependencyObject owner, DependencyObject container, object item)
		{
			int[] itemValueStorageIndices = Helper.ItemValueStorageIndices;
			List<KeyValuePair<int, object>> list = Helper.GetItemValues(owner, item) ?? new List<KeyValuePair<int, object>>();
			foreach (int num in itemValueStorageIndices)
			{
				DependencyProperty dependencyProperty = DependencyProperty.RegisteredPropertyList.List[num];
				object obj = DependencyProperty.UnsetValue;
				for (int j = 0; j < list.Count; j++)
				{
					if (list[j].Key == num)
					{
						obj = list[j].Value;
						break;
					}
				}
				if (dependencyProperty != null)
				{
					if (obj != DependencyProperty.UnsetValue)
					{
						Helper.ModifiedItemValue modifiedItemValue = obj as Helper.ModifiedItemValue;
						if (modifiedItemValue == null)
						{
							container.SetValue(dependencyProperty, obj);
						}
						else if (modifiedItemValue.IsCoercedWithCurrentValue)
						{
							container.SetCurrentValue(dependencyProperty, modifiedItemValue.Value);
						}
					}
					else if (container != container.GetValue(ItemContainerGenerator.ItemForItemContainerProperty))
					{
						EntryIndex entryIndex = container.LookupEntry(num);
						EffectiveValueEntry effectiveValueEntry = new EffectiveValueEntry(dependencyProperty);
						if (entryIndex.Found)
						{
							effectiveValueEntry = container.EffectiveValues[(int)entryIndex.Index];
							if (effectiveValueEntry.IsCoercedWithCurrentValue)
							{
								container.InvalidateProperty(dependencyProperty, false);
								entryIndex = container.LookupEntry(num);
								if (entryIndex.Found)
								{
									effectiveValueEntry = container.EffectiveValues[(int)entryIndex.Index];
								}
							}
						}
						if (entryIndex.Found && (effectiveValueEntry.BaseValueSourceInternal == BaseValueSourceInternal.Local || effectiveValueEntry.BaseValueSourceInternal == BaseValueSourceInternal.ParentTemplate) && !effectiveValueEntry.HasModifiers)
						{
							container.ClearValue(dependencyProperty);
						}
					}
				}
				else if (obj != DependencyProperty.UnsetValue)
				{
					EntryIndex entryIndex2 = container.LookupEntry(num);
					container.SetEffectiveValue(entryIndex2, null, num, null, obj, BaseValueSourceInternal.Local);
				}
			}
		}

		// Token: 0x060064DC RID: 25820 RVA: 0x001C4CA8 File Offset: 0x001C2EA8
		internal static void StoreItemValues(IContainItemStorage owner, DependencyObject container, object item)
		{
			int[] itemValueStorageIndices = Helper.ItemValueStorageIndices;
			DependencyObject owner2 = (DependencyObject)owner;
			foreach (int num in itemValueStorageIndices)
			{
				EntryIndex entryIndex = container.LookupEntry(num);
				if (entryIndex.Found)
				{
					EffectiveValueEntry effectiveValueEntry = container.EffectiveValues[(int)entryIndex.Index];
					if ((effectiveValueEntry.BaseValueSourceInternal == BaseValueSourceInternal.Local || effectiveValueEntry.BaseValueSourceInternal == BaseValueSourceInternal.ParentTemplate) && !effectiveValueEntry.HasModifiers)
					{
						Helper.StoreItemValue(owner2, item, num, effectiveValueEntry.Value);
					}
					else if (effectiveValueEntry.IsCoercedWithCurrentValue)
					{
						Helper.StoreItemValue(owner2, item, num, new Helper.ModifiedItemValue(effectiveValueEntry.ModifiedValue.CoercedValue, FullValueSource.IsCoercedWithCurrentValue));
					}
					else
					{
						Helper.ClearItemValue(owner2, item, num);
					}
				}
			}
		}

		// Token: 0x060064DD RID: 25821 RVA: 0x001C4D61 File Offset: 0x001C2F61
		internal static void ClearItemValueStorage(DependencyObject owner)
		{
			Helper.ItemValueStorageField.ClearValue(owner);
		}

		// Token: 0x060064DE RID: 25822 RVA: 0x001C4D6E File Offset: 0x001C2F6E
		internal static void ClearItemValueStorage(DependencyObject owner, int[] dpIndices)
		{
			Helper.ClearItemValueStorageRecursive(Helper.ItemValueStorageField.GetValue(owner), dpIndices);
		}

		// Token: 0x060064DF RID: 25823 RVA: 0x001C4D84 File Offset: 0x001C2F84
		private static void ClearItemValueStorageRecursive(WeakDictionary<object, List<KeyValuePair<int, object>>> itemValueStorage, int[] dpIndices)
		{
			if (itemValueStorage != null)
			{
				foreach (List<KeyValuePair<int, object>> list in itemValueStorage.Values)
				{
					for (int i = 0; i < list.Count; i++)
					{
						KeyValuePair<int, object> keyValuePair = list[i];
						if (keyValuePair.Key == Helper.ItemValueStorageField.GlobalIndex)
						{
							Helper.ClearItemValueStorageRecursive((WeakDictionary<object, List<KeyValuePair<int, object>>>)keyValuePair.Value, dpIndices);
						}
						for (int j = 0; j < dpIndices.Length; j++)
						{
							if (keyValuePair.Key == dpIndices[j])
							{
								list.RemoveAt(i--);
								break;
							}
						}
					}
				}
			}
		}

		// Token: 0x060064E0 RID: 25824 RVA: 0x001C4E3C File Offset: 0x001C303C
		internal static void ApplyCorrectionFactorToPixelHeaderSize(ItemsControl scrollingItemsControl, FrameworkElement virtualizingElement, Panel itemsHost, ref Size headerSize)
		{
			if (!VirtualizingStackPanel.IsVSP45Compat)
			{
				return;
			}
			bool flag = itemsHost != null && itemsHost.IsVisible;
			if (flag)
			{
				headerSize.Height = Math.Max(GroupItem.DesiredPixelItemsSizeCorrectionFactorField.GetValue(virtualizingElement).Top, headerSize.Height);
			}
			else
			{
				headerSize.Height = Math.Max(virtualizingElement.DesiredSize.Height, headerSize.Height);
			}
			headerSize.Width = Math.Max(virtualizingElement.DesiredSize.Width, headerSize.Width);
		}

		// Token: 0x060064E1 RID: 25825 RVA: 0x001C4EC8 File Offset: 0x001C30C8
		internal static HierarchicalVirtualizationItemDesiredSizes ApplyCorrectionFactorToItemDesiredSizes(FrameworkElement virtualizingElement, Panel itemsHost)
		{
			HierarchicalVirtualizationItemDesiredSizes value = GroupItem.HierarchicalVirtualizationItemDesiredSizesField.GetValue(virtualizingElement);
			if (!VirtualizingStackPanel.IsVSP45Compat)
			{
				return value;
			}
			if (itemsHost != null && itemsHost.IsVisible)
			{
				Size pixelSize = value.PixelSize;
				Size pixelSizeInViewport = value.PixelSizeInViewport;
				Size pixelSizeBeforeViewport = value.PixelSizeBeforeViewport;
				Size pixelSizeAfterViewport = value.PixelSizeAfterViewport;
				bool flag = false;
				Thickness value2 = new Thickness(0.0);
				Size desiredSize = virtualizingElement.DesiredSize;
				if (DoubleUtil.GreaterThan(pixelSize.Height, 0.0))
				{
					value2 = GroupItem.DesiredPixelItemsSizeCorrectionFactorField.GetValue(virtualizingElement);
					pixelSize.Height += value2.Bottom;
					flag = true;
				}
				pixelSize.Width = Math.Max(desiredSize.Width, pixelSize.Width);
				if (DoubleUtil.AreClose(value.PixelSizeAfterViewport.Height, 0.0) && DoubleUtil.AreClose(value.PixelSizeInViewport.Height, 0.0) && DoubleUtil.GreaterThan(value.PixelSizeBeforeViewport.Height, 0.0))
				{
					if (!flag)
					{
						value2 = GroupItem.DesiredPixelItemsSizeCorrectionFactorField.GetValue(virtualizingElement);
					}
					pixelSizeBeforeViewport.Height += value2.Bottom;
					flag = true;
				}
				pixelSizeBeforeViewport.Width = Math.Max(desiredSize.Width, pixelSizeBeforeViewport.Width);
				if (DoubleUtil.AreClose(value.PixelSizeAfterViewport.Height, 0.0) && DoubleUtil.GreaterThan(value.PixelSizeInViewport.Height, 0.0))
				{
					if (!flag)
					{
						value2 = GroupItem.DesiredPixelItemsSizeCorrectionFactorField.GetValue(virtualizingElement);
					}
					pixelSizeInViewport.Height += value2.Bottom;
					flag = true;
				}
				pixelSizeInViewport.Width = Math.Max(desiredSize.Width, pixelSizeInViewport.Width);
				if (DoubleUtil.GreaterThan(value.PixelSizeAfterViewport.Height, 0.0))
				{
					if (!flag)
					{
						value2 = GroupItem.DesiredPixelItemsSizeCorrectionFactorField.GetValue(virtualizingElement);
					}
					pixelSizeAfterViewport.Height += value2.Bottom;
				}
				pixelSizeAfterViewport.Width = Math.Max(desiredSize.Width, pixelSizeAfterViewport.Width);
				value = new HierarchicalVirtualizationItemDesiredSizes(value.LogicalSize, value.LogicalSizeInViewport, value.LogicalSizeBeforeViewport, value.LogicalSizeAfterViewport, pixelSize, pixelSizeInViewport, pixelSizeBeforeViewport, pixelSizeAfterViewport);
			}
			return value;
		}

		// Token: 0x060064E2 RID: 25826 RVA: 0x001C5140 File Offset: 0x001C3340
		internal static void ComputeCorrectionFactor(ItemsControl scrollingItemsControl, FrameworkElement virtualizingElement, Panel itemsHost, FrameworkElement headerElement)
		{
			if (!VirtualizingStackPanel.IsVSP45Compat)
			{
				return;
			}
			Rect rect = new Rect(default(Point), virtualizingElement.DesiredSize);
			bool flag = false;
			if (itemsHost != null)
			{
				Thickness value = default(Thickness);
				if (itemsHost.IsVisible)
				{
					Rect rect2 = itemsHost.TransformToAncestor(virtualizingElement).TransformBounds(new Rect(default(Point), itemsHost.DesiredSize));
					value.Top = rect2.Top;
					value.Bottom = rect.Bottom - rect2.Bottom;
					if (value.Bottom < 0.0)
					{
						value.Bottom = 0.0;
					}
				}
				Thickness value2 = GroupItem.DesiredPixelItemsSizeCorrectionFactorField.GetValue(virtualizingElement);
				if (!DoubleUtil.AreClose(value.Top, value2.Top) || !DoubleUtil.AreClose(value.Bottom, value2.Bottom))
				{
					flag = true;
					GroupItem.DesiredPixelItemsSizeCorrectionFactorField.SetValue(virtualizingElement, value);
				}
			}
			if (flag && scrollingItemsControl != null)
			{
				itemsHost = scrollingItemsControl.ItemsHost;
				if (itemsHost != null)
				{
					VirtualizingStackPanel virtualizingStackPanel = itemsHost as VirtualizingStackPanel;
					if (virtualizingStackPanel != null)
					{
						virtualizingStackPanel.AnchoredInvalidateMeasure();
						return;
					}
					itemsHost.InvalidateMeasure();
				}
			}
		}

		// Token: 0x060064E3 RID: 25827 RVA: 0x001C525C File Offset: 0x001C345C
		internal static void ClearVirtualizingElement(IHierarchicalVirtualizationAndScrollInfo virtualizingElement)
		{
			virtualizingElement.ItemDesiredSizes = default(HierarchicalVirtualizationItemDesiredSizes);
			virtualizingElement.MustDisableVirtualization = false;
		}

		// Token: 0x060064E4 RID: 25828 RVA: 0x001C5280 File Offset: 0x001C3480
		internal static T FindTemplatedDescendant<T>(FrameworkElement searchStart, FrameworkElement templatedParent) where T : FrameworkElement
		{
			T t = default(T);
			int childrenCount = VisualTreeHelper.GetChildrenCount(searchStart);
			int num = 0;
			while (num < childrenCount && t == null)
			{
				FrameworkElement frameworkElement = VisualTreeHelper.GetChild(searchStart, num) as FrameworkElement;
				if (frameworkElement != null && frameworkElement.TemplatedParent == templatedParent)
				{
					T t2 = frameworkElement as T;
					if (t2 != null)
					{
						t = t2;
					}
					else
					{
						t = Helper.FindTemplatedDescendant<T>(frameworkElement, templatedParent);
					}
				}
				num++;
			}
			return t;
		}

		// Token: 0x060064E5 RID: 25829 RVA: 0x001C52F0 File Offset: 0x001C34F0
		internal static T FindVisualAncestor<T>(DependencyObject element, Func<DependencyObject, bool> shouldContinueFunc) where T : DependencyObject
		{
			while (element != null)
			{
				element = VisualTreeHelper.GetParent(element);
				T t = element as T;
				if (t != null)
				{
					return t;
				}
				if (!shouldContinueFunc(element))
				{
					break;
				}
			}
			return default(T);
		}

		// Token: 0x060064E6 RID: 25830 RVA: 0x001C5332 File Offset: 0x001C3532
		internal static void InvalidateMeasureOnPath(DependencyObject pathStartElement, DependencyObject pathEndElement, bool duringMeasure)
		{
			Helper.InvalidateMeasureOnPath(pathStartElement, pathEndElement, duringMeasure, false);
		}

		// Token: 0x060064E7 RID: 25831 RVA: 0x001C5340 File Offset: 0x001C3540
		internal static void InvalidateMeasureOnPath(DependencyObject pathStartElement, DependencyObject pathEndElement, bool duringMeasure, bool includePathEnd)
		{
			DependencyObject dependencyObject = pathStartElement;
			while (dependencyObject != null && (includePathEnd || dependencyObject != pathEndElement))
			{
				UIElement uielement = dependencyObject as UIElement;
				if (uielement != null)
				{
					if (duringMeasure)
					{
						uielement.InvalidateMeasureInternal();
					}
					else
					{
						uielement.InvalidateMeasure();
					}
				}
				if (dependencyObject == pathEndElement)
				{
					break;
				}
				dependencyObject = VisualTreeHelper.GetParent(dependencyObject);
			}
		}

		// Token: 0x060064E8 RID: 25832 RVA: 0x001C5384 File Offset: 0x001C3584
		internal static void InvalidateMeasureForSubtree(DependencyObject d)
		{
			UIElement uielement = d as UIElement;
			if (uielement != null)
			{
				if (uielement.MeasureDirty)
				{
					return;
				}
				uielement.InvalidateMeasureInternal();
			}
			int childrenCount = VisualTreeHelper.GetChildrenCount(d);
			for (int i = 0; i < childrenCount; i++)
			{
				DependencyObject child = VisualTreeHelper.GetChild(d, i);
				if (child != null)
				{
					Helper.InvalidateMeasureForSubtree(child);
				}
			}
		}

		// Token: 0x060064E9 RID: 25833 RVA: 0x001C53D0 File Offset: 0x001C35D0
		internal static bool IsAnyAncestorOf(DependencyObject ancestor, DependencyObject element)
		{
			return ancestor != null && element != null && Helper.FindAnyAncestor(element, (DependencyObject d) => d == ancestor) != null;
		}

		// Token: 0x060064EA RID: 25834 RVA: 0x001C540C File Offset: 0x001C360C
		internal static DependencyObject FindAnyAncestor(DependencyObject element, Predicate<DependencyObject> predicate)
		{
			while (element != null)
			{
				element = Helper.GetAnyParent(element);
				if (element != null && predicate(element))
				{
					return element;
				}
			}
			return null;
		}

		// Token: 0x060064EB RID: 25835 RVA: 0x001C542C File Offset: 0x001C362C
		internal static DependencyObject GetAnyParent(DependencyObject element)
		{
			DependencyObject dependencyObject = null;
			if (!(element is ContentElement))
			{
				dependencyObject = VisualTreeHelper.GetParent(element);
			}
			if (dependencyObject == null)
			{
				dependencyObject = LogicalTreeHelper.GetParent(element);
			}
			return dependencyObject;
		}

		// Token: 0x060064EC RID: 25836 RVA: 0x001C5458 File Offset: 0x001C3658
		internal static bool IsDefaultValue(DependencyProperty dp, DependencyObject element)
		{
			bool flag;
			return element.GetValueSource(dp, null, out flag) == BaseValueSourceInternal.Default;
		}

		// Token: 0x060064ED RID: 25837 RVA: 0x001C5472 File Offset: 0x001C3672
		internal static bool IsComposing(DependencyObject d, DependencyProperty dp)
		{
			return dp == TextBox.TextProperty && Helper.IsComposing(d as TextBoxBase);
		}

		// Token: 0x060064EE RID: 25838 RVA: 0x001C548C File Offset: 0x001C368C
		internal static bool IsComposing(TextBoxBase tbb)
		{
			if (tbb == null)
			{
				return false;
			}
			TextEditor textEditor = tbb.TextEditor;
			if (textEditor == null)
			{
				return false;
			}
			TextStore textStore = textEditor.TextStore;
			return textStore != null && textStore.IsEffectivelyComposing;
		}

		// Token: 0x040032B6 RID: 12982
		private static readonly Type NullableType = Type.GetType("System.Nullable`1");

		// Token: 0x040032B7 RID: 12983
		private static readonly UncommonField<WeakDictionary<object, List<KeyValuePair<int, object>>>> ItemValueStorageField = new UncommonField<WeakDictionary<object, List<KeyValuePair<int, object>>>>();

		// Token: 0x040032B8 RID: 12984
		private static readonly int[] ItemValueStorageIndices = new int[]
		{
			Helper.ItemValueStorageField.GlobalIndex,
			TreeViewItem.IsExpandedProperty.GlobalIndex,
			Expander.IsExpandedProperty.GlobalIndex,
			GroupItem.DesiredPixelItemsSizeCorrectionFactorField.GlobalIndex,
			VirtualizingStackPanel.ItemsHostInsetProperty.GlobalIndex
		};

		// Token: 0x02000A05 RID: 2565
		private class FindResourceHelper
		{
			// Token: 0x060089FA RID: 35322 RVA: 0x00256A9C File Offset: 0x00254C9C
			internal object TryCatchWhen()
			{
				Dispatcher.CurrentDispatcher.WrappedInvoke(new DispatcherOperationCallback(this.DoTryCatchWhen), null, 1, new DispatcherOperationCallback(this.CatchHandler));
				return this._resource;
			}

			// Token: 0x060089FB RID: 35323 RVA: 0x00256AC9 File Offset: 0x00254CC9
			private object DoTryCatchWhen(object arg)
			{
				throw new ResourceReferenceKeyNotFoundException(SR.Get("MarkupExtensionResourceNotFound", new object[]
				{
					this._name
				}), this._name);
			}

			// Token: 0x060089FC RID: 35324 RVA: 0x00256AEF File Offset: 0x00254CEF
			private object CatchHandler(object arg)
			{
				this._resource = DependencyProperty.UnsetValue;
				return null;
			}

			// Token: 0x060089FD RID: 35325 RVA: 0x00256AFD File Offset: 0x00254CFD
			public FindResourceHelper(object name)
			{
				this._name = name;
				this._resource = null;
			}

			// Token: 0x040046B4 RID: 18100
			private object _name;

			// Token: 0x040046B5 RID: 18101
			private object _resource;
		}

		// Token: 0x02000A06 RID: 2566
		private class ModifiedItemValue
		{
			// Token: 0x060089FE RID: 35326 RVA: 0x00256B13 File Offset: 0x00254D13
			public ModifiedItemValue(object value, FullValueSource valueSource)
			{
				this._value = value;
				this._valueSource = valueSource;
			}

			// Token: 0x17001F27 RID: 7975
			// (get) Token: 0x060089FF RID: 35327 RVA: 0x00256B29 File Offset: 0x00254D29
			public object Value
			{
				get
				{
					return this._value;
				}
			}

			// Token: 0x17001F28 RID: 7976
			// (get) Token: 0x06008A00 RID: 35328 RVA: 0x00256B31 File Offset: 0x00254D31
			public bool IsCoercedWithCurrentValue
			{
				get
				{
					return (this._valueSource & FullValueSource.IsCoercedWithCurrentValue) > (FullValueSource)0;
				}
			}

			// Token: 0x040046B6 RID: 18102
			private object _value;

			// Token: 0x040046B7 RID: 18103
			private FullValueSource _valueSource;
		}
	}
}
