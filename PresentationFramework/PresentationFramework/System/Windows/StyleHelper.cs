using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;
using MS.Internal;
using MS.Internal.Data;
using MS.Utility;

namespace System.Windows
{
	// Token: 0x020000FB RID: 251
	internal static class StyleHelper
	{
		// Token: 0x060008EF RID: 2287 RVA: 0x0001D0F8 File Offset: 0x0001B2F8
		internal static void UpdateStyleCache(FrameworkElement fe, FrameworkContentElement fce, Style oldStyle, Style newStyle, ref Style styleCache)
		{
			if (newStyle != null)
			{
				DependencyObject dependencyObject = fe;
				if (dependencyObject == null)
				{
					dependencyObject = fce;
				}
				newStyle.CheckTargetType(dependencyObject);
				newStyle.Seal();
			}
			styleCache = newStyle;
			StyleHelper.DoStyleInvalidations(fe, fce, oldStyle, newStyle);
			StyleHelper.ExecuteOnApplyEnterExitActions(fe, fce, newStyle, StyleHelper.StyleDataField);
		}

		// Token: 0x060008F0 RID: 2288 RVA: 0x0001D138 File Offset: 0x0001B338
		internal static void UpdateThemeStyleCache(FrameworkElement fe, FrameworkContentElement fce, Style oldThemeStyle, Style newThemeStyle, ref Style themeStyleCache)
		{
			if (newThemeStyle != null)
			{
				DependencyObject dependencyObject = fe;
				if (dependencyObject == null)
				{
					dependencyObject = fce;
				}
				newThemeStyle.CheckTargetType(dependencyObject);
				newThemeStyle.Seal();
				if (StyleHelper.IsSetOnContainer(FrameworkElement.OverridesDefaultStyleProperty, ref newThemeStyle.ContainerDependents, true))
				{
					throw new InvalidOperationException(SR.Get("CannotHaveOverridesDefaultStyleInThemeStyle"));
				}
				if (newThemeStyle.HasEventSetters)
				{
					throw new InvalidOperationException(SR.Get("CannotHaveEventHandlersInThemeStyle"));
				}
			}
			themeStyleCache = newThemeStyle;
			Style style = null;
			if (fe != null)
			{
				if (StyleHelper.ShouldGetValueFromStyle(FrameworkElement.DefaultStyleKeyProperty))
				{
					style = fe.Style;
				}
			}
			else if (StyleHelper.ShouldGetValueFromStyle(FrameworkContentElement.DefaultStyleKeyProperty))
			{
				style = fce.Style;
			}
			StyleHelper.DoThemeStyleInvalidations(fe, fce, oldThemeStyle, newThemeStyle, style);
			StyleHelper.ExecuteOnApplyEnterExitActions(fe, fce, newThemeStyle, StyleHelper.ThemeStyleDataField);
		}

		// Token: 0x060008F1 RID: 2289 RVA: 0x0001D1E0 File Offset: 0x0001B3E0
		internal static Style GetThemeStyle(FrameworkElement fe, FrameworkContentElement fce)
		{
			Style style = null;
			object defaultStyleKey;
			bool overridesDefaultStyle;
			Style themeStyle;
			if (fe != null)
			{
				fe.HasThemeStyleEverBeenFetched = true;
				defaultStyleKey = fe.DefaultStyleKey;
				overridesDefaultStyle = fe.OverridesDefaultStyle;
				if (StyleHelper.ShouldGetValueFromStyle(FrameworkElement.DefaultStyleKeyProperty))
				{
					Style style2 = fe.Style;
				}
				themeStyle = fe.ThemeStyle;
			}
			else
			{
				fce.HasThemeStyleEverBeenFetched = true;
				defaultStyleKey = fce.DefaultStyleKey;
				overridesDefaultStyle = fce.OverridesDefaultStyle;
				if (StyleHelper.ShouldGetValueFromStyle(FrameworkContentElement.DefaultStyleKeyProperty))
				{
					Style style2 = fce.Style;
				}
				themeStyle = fce.ThemeStyle;
			}
			if (defaultStyleKey != null && !overridesDefaultStyle)
			{
				DependencyObjectType dtypeThemeStyleKey;
				if (fe != null)
				{
					dtypeThemeStyleKey = fe.DTypeThemeStyleKey;
				}
				else
				{
					dtypeThemeStyleKey = fce.DTypeThemeStyleKey;
				}
				object obj;
				if (dtypeThemeStyleKey != null && dtypeThemeStyleKey.SystemType != null && dtypeThemeStyleKey.SystemType.Equals(defaultStyleKey))
				{
					obj = SystemResources.FindThemeStyle(dtypeThemeStyleKey);
				}
				else
				{
					obj = SystemResources.FindResourceInternal(defaultStyleKey);
				}
				if (obj != null)
				{
					if (!(obj is Style))
					{
						throw new InvalidOperationException(SR.Get("SystemResourceForTypeIsNotStyle", new object[]
						{
							defaultStyleKey
						}));
					}
					style = (Style)obj;
				}
				if (style == null)
				{
					Type type = defaultStyleKey as Type;
					if (type != null)
					{
						PropertyMetadata metadata = FrameworkElement.StyleProperty.GetMetadata(type);
						if (metadata != null)
						{
							style = (metadata.DefaultValue as Style);
						}
					}
				}
			}
			if (themeStyle != style)
			{
				if (fe != null)
				{
					FrameworkElement.OnThemeStyleChanged(fe, themeStyle, style);
				}
				else
				{
					FrameworkContentElement.OnThemeStyleChanged(fce, themeStyle, style);
				}
			}
			return style;
		}

		// Token: 0x060008F2 RID: 2290 RVA: 0x0001D330 File Offset: 0x0001B530
		internal static void UpdateTemplateCache(FrameworkElement fe, FrameworkTemplate oldTemplate, FrameworkTemplate newTemplate, DependencyProperty templateProperty)
		{
			if (newTemplate != null)
			{
				newTemplate.Seal();
			}
			fe.TemplateCache = newTemplate;
			StyleHelper.DoTemplateInvalidations(fe, oldTemplate);
			StyleHelper.ExecuteOnApplyEnterExitActions(fe, null, newTemplate);
		}

		// Token: 0x060008F3 RID: 2291 RVA: 0x0001D360 File Offset: 0x0001B560
		internal static void SealTemplate(FrameworkTemplate frameworkTemplate, ref bool isSealed, FrameworkElementFactory templateRoot, TriggerCollection triggers, ResourceDictionary resources, HybridDictionary childIndexFromChildID, ref FrugalStructList<ChildRecord> childRecordFromChildIndex, ref FrugalStructList<ItemStructMap<TriggerSourceRecord>> triggerSourceRecordFromChildIndex, ref FrugalStructList<ContainerDependent> containerDependents, ref FrugalStructList<ChildPropertyDependent> resourceDependents, ref ItemStructList<ChildEventDependent> eventDependents, ref HybridDictionary triggerActions, ref HybridDictionary dataTriggerRecordFromBinding, ref bool hasInstanceValues, ref EventHandlersStore eventHandlersStore)
		{
			if (isSealed)
			{
				return;
			}
			if (frameworkTemplate != null)
			{
				frameworkTemplate.ProcessTemplateBeforeSeal();
			}
			if (templateRoot != null)
			{
				templateRoot.Seal(frameworkTemplate);
			}
			if (triggers != null)
			{
				triggers.Seal();
			}
			if (resources != null)
			{
				resources.IsReadOnly = true;
			}
			if (templateRoot != null)
			{
				StyleHelper.ProcessTemplateContentFromFEF(templateRoot, ref childRecordFromChildIndex, ref triggerSourceRecordFromChildIndex, ref resourceDependents, ref eventDependents, ref dataTriggerRecordFromBinding, childIndexFromChildID, ref hasInstanceValues);
			}
			bool hasLoadedChangeHandler = false;
			StyleHelper.ProcessTemplateTriggers(triggers, frameworkTemplate, ref childRecordFromChildIndex, ref triggerSourceRecordFromChildIndex, ref containerDependents, ref resourceDependents, ref eventDependents, ref dataTriggerRecordFromBinding, childIndexFromChildID, ref hasInstanceValues, ref triggerActions, templateRoot, ref eventHandlersStore, ref frameworkTemplate.PropertyTriggersWithActions, ref frameworkTemplate.DataTriggersWithActions, ref hasLoadedChangeHandler);
			frameworkTemplate.HasLoadedChangeHandler = hasLoadedChangeHandler;
			frameworkTemplate.SetResourceReferenceState();
			isSealed = true;
			frameworkTemplate.DetachFromDispatcher();
			if (StyleHelper.IsSetOnContainer(Control.TemplateProperty, ref containerDependents, true) || StyleHelper.IsSetOnContainer(ContentPresenter.TemplateProperty, ref containerDependents, true))
			{
				throw new InvalidOperationException(SR.Get("CannotHavePropertyInTemplate", new object[]
				{
					Control.TemplateProperty.Name
				}));
			}
			if (StyleHelper.IsSetOnContainer(FrameworkElement.StyleProperty, ref containerDependents, true))
			{
				throw new InvalidOperationException(SR.Get("CannotHavePropertyInTemplate", new object[]
				{
					FrameworkElement.StyleProperty.Name
				}));
			}
			if (StyleHelper.IsSetOnContainer(FrameworkElement.DefaultStyleKeyProperty, ref containerDependents, true))
			{
				throw new InvalidOperationException(SR.Get("CannotHavePropertyInTemplate", new object[]
				{
					FrameworkElement.DefaultStyleKeyProperty.Name
				}));
			}
			if (StyleHelper.IsSetOnContainer(FrameworkElement.OverridesDefaultStyleProperty, ref containerDependents, true))
			{
				throw new InvalidOperationException(SR.Get("CannotHavePropertyInTemplate", new object[]
				{
					FrameworkElement.OverridesDefaultStyleProperty.Name
				}));
			}
			if (StyleHelper.IsSetOnContainer(FrameworkElement.NameProperty, ref containerDependents, true))
			{
				throw new InvalidOperationException(SR.Get("CannotHavePropertyInTemplate", new object[]
				{
					FrameworkElement.NameProperty.Name
				}));
			}
		}

		// Token: 0x060008F4 RID: 2292 RVA: 0x0001D4FC File Offset: 0x0001B6FC
		internal static void UpdateTables(ref PropertyValue propertyValue, ref FrugalStructList<ChildRecord> childRecordFromChildIndex, ref FrugalStructList<ItemStructMap<TriggerSourceRecord>> triggerSourceRecordFromChildIndex, ref FrugalStructList<ChildPropertyDependent> resourceDependents, ref HybridDictionary dataTriggerRecordFromBinding, HybridDictionary childIndexFromChildName, ref bool hasInstanceValues)
		{
			int num = StyleHelper.QueryChildIndexFromChildName(propertyValue.ChildName, childIndexFromChildName);
			if (num == -1)
			{
				throw new InvalidOperationException(SR.Get("NameNotFound", new object[]
				{
					propertyValue.ChildName
				}));
			}
			object valueInternal = propertyValue.ValueInternal;
			bool flag = StyleHelper.RequiresInstanceStorage(ref valueInternal);
			propertyValue.ValueInternal = valueInternal;
			childRecordFromChildIndex.EnsureIndex(num);
			ChildRecord childRecord = childRecordFromChildIndex[num];
			int num2 = childRecord.ValueLookupListFromProperty.EnsureEntry(propertyValue.Property.GlobalIndex);
			ChildValueLookup childValueLookup = default(ChildValueLookup);
			childValueLookup.LookupType = (ValueLookupType)propertyValue.ValueType;
			childValueLookup.Conditions = propertyValue.Conditions;
			childValueLookup.Property = propertyValue.Property;
			childValueLookup.Value = propertyValue.ValueInternal;
			childRecord.ValueLookupListFromProperty.Entries[num2].Value.Add(ref childValueLookup);
			childRecordFromChildIndex[num] = childRecord;
			switch (propertyValue.ValueType)
			{
			case PropertyValueType.Set:
				hasInstanceValues = (hasInstanceValues || flag);
				return;
			case PropertyValueType.Trigger:
			case PropertyValueType.PropertyTriggerResource:
				if (propertyValue.Conditions != null)
				{
					for (int i = 0; i < propertyValue.Conditions.Length; i++)
					{
						int sourceChildIndex = propertyValue.Conditions[i].SourceChildIndex;
						triggerSourceRecordFromChildIndex.EnsureIndex(sourceChildIndex);
						ItemStructMap<TriggerSourceRecord> itemStructMap = triggerSourceRecordFromChildIndex[sourceChildIndex];
						if (propertyValue.Conditions[i].Property == null)
						{
							throw new InvalidOperationException(SR.Get("MissingTriggerProperty"));
						}
						int num3 = itemStructMap.EnsureEntry(propertyValue.Conditions[i].Property.GlobalIndex);
						StyleHelper.AddPropertyDependent(num, propertyValue.Property, ref itemStructMap.Entries[num3].Value.ChildPropertyDependents);
						triggerSourceRecordFromChildIndex[sourceChildIndex] = itemStructMap;
					}
					if (propertyValue.ValueType == PropertyValueType.PropertyTriggerResource)
					{
						StyleHelper.AddResourceDependent(num, propertyValue.Property, propertyValue.ValueInternal, ref resourceDependents);
					}
				}
				if (propertyValue.ValueType != PropertyValueType.PropertyTriggerResource)
				{
					hasInstanceValues = (hasInstanceValues || flag);
					return;
				}
				break;
			case PropertyValueType.DataTrigger:
			case PropertyValueType.DataTriggerResource:
				if (propertyValue.Conditions != null)
				{
					if (dataTriggerRecordFromBinding == null)
					{
						dataTriggerRecordFromBinding = new HybridDictionary();
					}
					for (int j = 0; j < propertyValue.Conditions.Length; j++)
					{
						DataTriggerRecord dataTriggerRecord = (DataTriggerRecord)dataTriggerRecordFromBinding[propertyValue.Conditions[j].Binding];
						if (dataTriggerRecord == null)
						{
							dataTriggerRecord = new DataTriggerRecord();
							dataTriggerRecordFromBinding[propertyValue.Conditions[j].Binding] = dataTriggerRecord;
						}
						StyleHelper.AddPropertyDependent(num, propertyValue.Property, ref dataTriggerRecord.Dependents);
					}
					if (propertyValue.ValueType == PropertyValueType.DataTriggerResource)
					{
						StyleHelper.AddResourceDependent(num, propertyValue.Property, propertyValue.ValueInternal, ref resourceDependents);
					}
				}
				if (propertyValue.ValueType != PropertyValueType.DataTriggerResource)
				{
					hasInstanceValues = (hasInstanceValues || flag);
					return;
				}
				break;
			case PropertyValueType.TemplateBinding:
			{
				TemplateBindingExtension templateBindingExtension = (TemplateBindingExtension)propertyValue.ValueInternal;
				DependencyProperty property = propertyValue.Property;
				DependencyProperty property2 = templateBindingExtension.Property;
				int index = 0;
				triggerSourceRecordFromChildIndex.EnsureIndex(index);
				ItemStructMap<TriggerSourceRecord> itemStructMap2 = triggerSourceRecordFromChildIndex[index];
				int num4 = itemStructMap2.EnsureEntry(property2.GlobalIndex);
				StyleHelper.AddPropertyDependent(num, property, ref itemStructMap2.Entries[num4].Value.ChildPropertyDependents);
				triggerSourceRecordFromChildIndex[index] = itemStructMap2;
				return;
			}
			case PropertyValueType.Resource:
				StyleHelper.AddResourceDependent(num, propertyValue.Property, propertyValue.ValueInternal, ref resourceDependents);
				break;
			default:
				return;
			}
		}

		// Token: 0x060008F5 RID: 2293 RVA: 0x0001D83C File Offset: 0x0001BA3C
		private static bool RequiresInstanceStorage(ref object value)
		{
			MarkupExtension markupExtension = null;
			Freezable freezable = null;
			DeferredReference deferredReference;
			if ((deferredReference = (value as DeferredReference)) != null)
			{
				Type valueType = deferredReference.GetValueType();
				if (valueType != null)
				{
					if (typeof(MarkupExtension).IsAssignableFrom(valueType))
					{
						value = deferredReference.GetValue(BaseValueSourceInternal.Style);
						if ((markupExtension = (value as MarkupExtension)) == null)
						{
							freezable = (value as Freezable);
						}
					}
					else if (typeof(Freezable).IsAssignableFrom(valueType))
					{
						freezable = (Freezable)deferredReference.GetValue(BaseValueSourceInternal.Style);
					}
				}
			}
			else if ((markupExtension = (value as MarkupExtension)) == null)
			{
				freezable = (value as Freezable);
			}
			bool result = false;
			if (markupExtension != null)
			{
				value = markupExtension;
				result = true;
			}
			else if (freezable != null)
			{
				value = freezable;
				if (!freezable.CanFreeze)
				{
					result = true;
				}
			}
			return result;
		}

		// Token: 0x060008F6 RID: 2294 RVA: 0x0001D8F0 File Offset: 0x0001BAF0
		internal static void AddContainerDependent(DependencyProperty dp, bool fromVisualTrigger, ref FrugalStructList<ContainerDependent> containerDependents)
		{
			ContainerDependent containerDependent;
			for (int i = 0; i < containerDependents.Count; i++)
			{
				containerDependent = containerDependents[i];
				if (dp == containerDependent.Property)
				{
					containerDependent.FromVisualTrigger = (containerDependent.FromVisualTrigger || fromVisualTrigger);
					return;
				}
			}
			containerDependent = new ContainerDependent
			{
				Property = dp,
				FromVisualTrigger = fromVisualTrigger
			};
			containerDependents.Add(containerDependent);
		}

		// Token: 0x060008F7 RID: 2295 RVA: 0x0001D94C File Offset: 0x0001BB4C
		internal static void AddEventDependent(int childIndex, EventHandlersStore eventHandlersStore, ref ItemStructList<ChildEventDependent> eventDependents)
		{
			if (eventHandlersStore != null)
			{
				ChildEventDependent childEventDependent = default(ChildEventDependent);
				childEventDependent.ChildIndex = childIndex;
				childEventDependent.EventHandlersStore = eventHandlersStore;
				eventDependents.Add(ref childEventDependent);
			}
		}

		// Token: 0x060008F8 RID: 2296 RVA: 0x0001D97C File Offset: 0x0001BB7C
		private static void AddPropertyDependent(int childIndex, DependencyProperty dp, ref FrugalStructList<ChildPropertyDependent> propertyDependents)
		{
			propertyDependents.Add(new ChildPropertyDependent
			{
				ChildIndex = childIndex,
				Property = dp
			});
		}

		// Token: 0x060008F9 RID: 2297 RVA: 0x0001D9AC File Offset: 0x0001BBAC
		private static void AddResourceDependent(int childIndex, DependencyProperty dp, object name, ref FrugalStructList<ChildPropertyDependent> resourceDependents)
		{
			bool flag = true;
			for (int i = 0; i < resourceDependents.Count; i++)
			{
				ChildPropertyDependent childPropertyDependent = resourceDependents[i];
				if (childPropertyDependent.ChildIndex == childIndex && childPropertyDependent.Property == dp && childPropertyDependent.Name == name)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				resourceDependents.Add(new ChildPropertyDependent
				{
					ChildIndex = childIndex,
					Property = dp,
					Name = name
				});
			}
		}

		// Token: 0x060008FA RID: 2298 RVA: 0x0001DA20 File Offset: 0x0001BC20
		internal static void ProcessTemplateContentFromFEF(FrameworkElementFactory factory, ref FrugalStructList<ChildRecord> childRecordFromChildIndex, ref FrugalStructList<ItemStructMap<TriggerSourceRecord>> triggerSourceRecordFromChildIndex, ref FrugalStructList<ChildPropertyDependent> resourceDependents, ref ItemStructList<ChildEventDependent> eventDependents, ref HybridDictionary dataTriggerRecordFromBinding, HybridDictionary childIndexFromChildID, ref bool hasInstanceValues)
		{
			for (int i = 0; i < factory.PropertyValues.Count; i++)
			{
				PropertyValue propertyValue = factory.PropertyValues[i];
				StyleHelper.UpdateTables(ref propertyValue, ref childRecordFromChildIndex, ref triggerSourceRecordFromChildIndex, ref resourceDependents, ref dataTriggerRecordFromBinding, childIndexFromChildID, ref hasInstanceValues);
			}
			StyleHelper.AddEventDependent(factory._childIndex, factory.EventHandlersStore, ref eventDependents);
			for (factory = factory.FirstChild; factory != null; factory = factory.NextSibling)
			{
				StyleHelper.ProcessTemplateContentFromFEF(factory, ref childRecordFromChildIndex, ref triggerSourceRecordFromChildIndex, ref resourceDependents, ref eventDependents, ref dataTriggerRecordFromBinding, childIndexFromChildID, ref hasInstanceValues);
			}
		}

		// Token: 0x060008FB RID: 2299 RVA: 0x0001DA9C File Offset: 0x0001BC9C
		private static void ProcessTemplateTriggers(TriggerCollection triggers, FrameworkTemplate frameworkTemplate, ref FrugalStructList<ChildRecord> childRecordFromChildIndex, ref FrugalStructList<ItemStructMap<TriggerSourceRecord>> triggerSourceRecordFromChildIndex, ref FrugalStructList<ContainerDependent> containerDependents, ref FrugalStructList<ChildPropertyDependent> resourceDependents, ref ItemStructList<ChildEventDependent> eventDependents, ref HybridDictionary dataTriggerRecordFromBinding, HybridDictionary childIndexFromChildID, ref bool hasInstanceValues, ref HybridDictionary triggerActions, FrameworkElementFactory templateRoot, ref EventHandlersStore eventHandlersStore, ref FrugalMap propertyTriggersWithActions, ref HybridDictionary dataTriggersWithActions, ref bool hasLoadedChangeHandler)
		{
			if (triggers != null)
			{
				int count = triggers.Count;
				for (int i = 0; i < count; i++)
				{
					TriggerBase triggerBase = triggers[i];
					Trigger trigger;
					MultiTrigger multiTrigger;
					DataTrigger dataTrigger;
					MultiDataTrigger multiDataTrigger;
					EventTrigger eventTrigger;
					StyleHelper.DetermineTriggerType(triggerBase, out trigger, out multiTrigger, out dataTrigger, out multiDataTrigger, out eventTrigger);
					if (trigger != null || multiTrigger != null || dataTrigger != null || multiDataTrigger != null)
					{
						TriggerCondition[] triggerConditions = triggerBase.TriggerConditions;
						for (int j = 0; j < triggerConditions.Length; j++)
						{
							triggerConditions[j].SourceChildIndex = StyleHelper.QueryChildIndexFromChildName(triggerConditions[j].SourceName, childIndexFromChildID);
						}
						for (int k = 0; k < triggerBase.PropertyValues.Count; k++)
						{
							PropertyValue propertyValue = triggerBase.PropertyValues[k];
							if (propertyValue.ChildName == "~Self")
							{
								StyleHelper.AddContainerDependent(propertyValue.Property, true, ref containerDependents);
							}
							StyleHelper.UpdateTables(ref propertyValue, ref childRecordFromChildIndex, ref triggerSourceRecordFromChildIndex, ref resourceDependents, ref dataTriggerRecordFromBinding, childIndexFromChildID, ref hasInstanceValues);
						}
						if (triggerBase.HasEnterActions || triggerBase.HasExitActions)
						{
							if (trigger != null)
							{
								StyleHelper.AddPropertyTriggerWithAction(triggerBase, trigger.Property, ref propertyTriggersWithActions);
							}
							else if (multiTrigger != null)
							{
								for (int l = 0; l < multiTrigger.Conditions.Count; l++)
								{
									Condition condition = multiTrigger.Conditions[l];
									StyleHelper.AddPropertyTriggerWithAction(triggerBase, condition.Property, ref propertyTriggersWithActions);
								}
							}
							else if (dataTrigger != null)
							{
								StyleHelper.AddDataTriggerWithAction(triggerBase, dataTrigger.Binding, ref dataTriggersWithActions);
							}
							else
							{
								if (multiDataTrigger == null)
								{
									throw new InvalidOperationException(SR.Get("UnsupportedTriggerInTemplate", new object[]
									{
										triggerBase.GetType().Name
									}));
								}
								for (int m = 0; m < multiDataTrigger.Conditions.Count; m++)
								{
									Condition condition2 = multiDataTrigger.Conditions[m];
									StyleHelper.AddDataTriggerWithAction(triggerBase, condition2.Binding, ref dataTriggersWithActions);
								}
							}
						}
					}
					else
					{
						if (eventTrigger == null)
						{
							throw new InvalidOperationException(SR.Get("UnsupportedTriggerInTemplate", new object[]
							{
								triggerBase.GetType().Name
							}));
						}
						StyleHelper.ProcessEventTrigger(eventTrigger, childIndexFromChildID, ref triggerActions, ref eventDependents, templateRoot, frameworkTemplate, ref eventHandlersStore, ref hasLoadedChangeHandler);
					}
				}
			}
		}

		// Token: 0x060008FC RID: 2300 RVA: 0x0001DCB4 File Offset: 0x0001BEB4
		private static void DetermineTriggerType(TriggerBase triggerBase, out Trigger trigger, out MultiTrigger multiTrigger, out DataTrigger dataTrigger, out MultiDataTrigger multiDataTrigger, out EventTrigger eventTrigger)
		{
			Trigger trigger2;
			trigger = (trigger2 = (triggerBase as Trigger));
			if (trigger2 != null)
			{
				multiTrigger = null;
				dataTrigger = null;
				multiDataTrigger = null;
				eventTrigger = null;
				return;
			}
			MultiTrigger multiTrigger2;
			multiTrigger = (multiTrigger2 = (triggerBase as MultiTrigger));
			if (multiTrigger2 != null)
			{
				dataTrigger = null;
				multiDataTrigger = null;
				eventTrigger = null;
				return;
			}
			DataTrigger dataTrigger2;
			dataTrigger = (dataTrigger2 = (triggerBase as DataTrigger));
			if (dataTrigger2 != null)
			{
				multiDataTrigger = null;
				eventTrigger = null;
				return;
			}
			MultiDataTrigger multiDataTrigger2;
			multiDataTrigger = (multiDataTrigger2 = (triggerBase as MultiDataTrigger));
			if (multiDataTrigger2 != null)
			{
				eventTrigger = null;
				return;
			}
			EventTrigger eventTrigger2;
			eventTrigger = (eventTrigger2 = (triggerBase as EventTrigger));
		}

		// Token: 0x060008FD RID: 2301 RVA: 0x0001DD30 File Offset: 0x0001BF30
		internal static void ProcessEventTrigger(EventTrigger eventTrigger, HybridDictionary childIndexFromChildName, ref HybridDictionary triggerActions, ref ItemStructList<ChildEventDependent> eventDependents, FrameworkElementFactory templateRoot, FrameworkTemplate frameworkTemplate, ref EventHandlersStore eventHandlersStore, ref bool hasLoadedChangeHandler)
		{
			if (eventTrigger == null)
			{
				return;
			}
			List<TriggerAction> list = null;
			bool flag = true;
			bool flag2 = false;
			FrameworkElementFactory frameworkElementFactory = null;
			if (eventTrigger.SourceName == null)
			{
				eventTrigger.TriggerChildIndex = 0;
			}
			else
			{
				int num = StyleHelper.QueryChildIndexFromChildName(eventTrigger.SourceName, childIndexFromChildName);
				if (num == -1)
				{
					throw new InvalidOperationException(SR.Get("EventTriggerTargetNameUnresolvable", new object[]
					{
						eventTrigger.SourceName
					}));
				}
				eventTrigger.TriggerChildIndex = num;
			}
			if (triggerActions == null)
			{
				triggerActions = new HybridDictionary();
			}
			else
			{
				list = (triggerActions[eventTrigger.RoutedEvent] as List<TriggerAction>);
			}
			if (list == null)
			{
				flag = false;
				list = new List<TriggerAction>();
			}
			for (int i = 0; i < eventTrigger.Actions.Count; i++)
			{
				TriggerAction item = eventTrigger.Actions[i];
				list.Add(item);
				flag2 = true;
			}
			if (flag2 && !flag)
			{
				triggerActions[eventTrigger.RoutedEvent] = list;
			}
			if (templateRoot != null || eventTrigger.TriggerChildIndex == 0)
			{
				if (eventTrigger.TriggerChildIndex != 0)
				{
					frameworkElementFactory = StyleHelper.FindFEF(templateRoot, eventTrigger.TriggerChildIndex);
				}
				if (eventTrigger.RoutedEvent == FrameworkElement.LoadedEvent || eventTrigger.RoutedEvent == FrameworkElement.UnloadedEvent)
				{
					if (eventTrigger.TriggerChildIndex == 0)
					{
						hasLoadedChangeHandler = true;
					}
					else
					{
						frameworkElementFactory.HasLoadedChangeHandler = true;
					}
				}
				StyleHelper.AddDelegateToFireTrigger(eventTrigger.RoutedEvent, eventTrigger.TriggerChildIndex, templateRoot, frameworkElementFactory, ref eventDependents, ref eventHandlersStore);
				return;
			}
			if (eventTrigger.RoutedEvent == FrameworkElement.LoadedEvent || eventTrigger.RoutedEvent == FrameworkElement.UnloadedEvent)
			{
				FrameworkTemplate.TemplateChildLoadedFlags templateChildLoadedFlags = frameworkTemplate._TemplateChildLoadedDictionary[eventTrigger.TriggerChildIndex] as FrameworkTemplate.TemplateChildLoadedFlags;
				if (templateChildLoadedFlags == null)
				{
					templateChildLoadedFlags = new FrameworkTemplate.TemplateChildLoadedFlags();
					frameworkTemplate._TemplateChildLoadedDictionary[eventTrigger.TriggerChildIndex] = templateChildLoadedFlags;
				}
				if (eventTrigger.RoutedEvent == FrameworkElement.LoadedEvent)
				{
					templateChildLoadedFlags.HasLoadedChangedHandler = true;
				}
				else
				{
					templateChildLoadedFlags.HasUnloadedChangedHandler = true;
				}
			}
			StyleHelper.AddDelegateToFireTrigger(eventTrigger.RoutedEvent, eventTrigger.TriggerChildIndex, ref eventDependents, ref eventHandlersStore);
		}

		// Token: 0x060008FE RID: 2302 RVA: 0x0001DF04 File Offset: 0x0001C104
		private static void AddDelegateToFireTrigger(RoutedEvent routedEvent, int childIndex, FrameworkElementFactory templateRoot, FrameworkElementFactory childFef, ref ItemStructList<ChildEventDependent> eventDependents, ref EventHandlersStore eventHandlersStore)
		{
			if (childIndex == 0)
			{
				if (eventHandlersStore == null)
				{
					eventHandlersStore = new EventHandlersStore();
					StyleHelper.AddEventDependent(0, eventHandlersStore, ref eventDependents);
				}
				eventHandlersStore.AddRoutedEventHandler(routedEvent, StyleHelper.EventTriggerHandlerOnContainer, false);
				return;
			}
			if (childFef.EventHandlersStore == null)
			{
				childFef.EventHandlersStore = new EventHandlersStore();
				StyleHelper.AddEventDependent(childIndex, childFef.EventHandlersStore, ref eventDependents);
			}
			childFef.EventHandlersStore.AddRoutedEventHandler(routedEvent, StyleHelper.EventTriggerHandlerOnChild, false);
		}

		// Token: 0x060008FF RID: 2303 RVA: 0x0001DF6F File Offset: 0x0001C16F
		private static void AddDelegateToFireTrigger(RoutedEvent routedEvent, int childIndex, ref ItemStructList<ChildEventDependent> eventDependents, ref EventHandlersStore eventHandlersStore)
		{
			if (eventHandlersStore == null)
			{
				eventHandlersStore = new EventHandlersStore();
			}
			StyleHelper.AddEventDependent(childIndex, eventHandlersStore, ref eventDependents);
			eventHandlersStore.AddRoutedEventHandler(routedEvent, StyleHelper.EventTriggerHandlerOnChild, false);
		}

		// Token: 0x06000900 RID: 2304 RVA: 0x0001DF94 File Offset: 0x0001C194
		internal static void SealIfSealable(object value)
		{
			ISealable sealable = value as ISealable;
			if (sealable != null && !sealable.IsSealed && sealable.CanSeal)
			{
				sealable.Seal();
			}
		}

		// Token: 0x06000901 RID: 2305 RVA: 0x0001DFC4 File Offset: 0x0001C1C4
		internal static void UpdateInstanceData(UncommonField<HybridDictionary[]> dataField, FrameworkElement fe, FrameworkContentElement fce, Style oldStyle, Style newStyle, FrameworkTemplate oldFrameworkTemplate, FrameworkTemplate newFrameworkTemplate, InternalFlags hasGeneratedSubTreeFlag)
		{
			DependencyObject dependencyObject = (fe != null) ? fe : fce;
			if (oldStyle != null || oldFrameworkTemplate != null)
			{
				StyleHelper.ReleaseInstanceData(dataField, dependencyObject, fe, fce, oldStyle, oldFrameworkTemplate, hasGeneratedSubTreeFlag);
			}
			if (newStyle != null || newFrameworkTemplate != null)
			{
				StyleHelper.CreateInstanceData(dataField, dependencyObject, fe, fce, newStyle, newFrameworkTemplate);
				return;
			}
			dataField.ClearValue(dependencyObject);
		}

		// Token: 0x06000902 RID: 2306 RVA: 0x0001E00C File Offset: 0x0001C20C
		internal static void CreateInstanceData(UncommonField<HybridDictionary[]> dataField, DependencyObject container, FrameworkElement fe, FrameworkContentElement fce, Style newStyle, FrameworkTemplate newFrameworkTemplate)
		{
			if (newStyle != null)
			{
				if (newStyle.HasInstanceValues)
				{
					HybridDictionary instanceValues = StyleHelper.EnsureInstanceData(dataField, container, InstanceStyleData.InstanceValues);
					StyleHelper.ProcessInstanceValuesForChild(container, container, 0, instanceValues, true, ref newStyle.ChildRecordFromChildIndex);
					return;
				}
			}
			else if (newFrameworkTemplate != null && newFrameworkTemplate.HasInstanceValues)
			{
				HybridDictionary instanceValues2 = StyleHelper.EnsureInstanceData(dataField, container, InstanceStyleData.InstanceValues);
				StyleHelper.ProcessInstanceValuesForChild(container, container, 0, instanceValues2, true, ref newFrameworkTemplate.ChildRecordFromChildIndex);
			}
		}

		// Token: 0x06000903 RID: 2307 RVA: 0x0001E068 File Offset: 0x0001C268
		internal static void CreateInstanceDataForChild(UncommonField<HybridDictionary[]> dataField, DependencyObject container, DependencyObject child, int childIndex, bool hasInstanceValues, ref FrugalStructList<ChildRecord> childRecordFromChildIndex)
		{
			if (hasInstanceValues)
			{
				HybridDictionary instanceValues = StyleHelper.EnsureInstanceData(dataField, container, InstanceStyleData.InstanceValues);
				StyleHelper.ProcessInstanceValuesForChild(container, child, childIndex, instanceValues, true, ref childRecordFromChildIndex);
			}
		}

		// Token: 0x06000904 RID: 2308 RVA: 0x0001E090 File Offset: 0x0001C290
		internal static void ReleaseInstanceData(UncommonField<HybridDictionary[]> dataField, DependencyObject container, FrameworkElement fe, FrameworkContentElement fce, Style oldStyle, FrameworkTemplate oldFrameworkTemplate, InternalFlags hasGeneratedSubTreeFlag)
		{
			HybridDictionary[] value = dataField.GetValue(container);
			if (oldStyle != null)
			{
				HybridDictionary instanceValues = (value != null) ? value[0] : null;
				StyleHelper.ReleaseInstanceDataForDataTriggers(dataField, instanceValues, oldStyle, oldFrameworkTemplate);
				if (oldStyle.HasInstanceValues)
				{
					StyleHelper.ProcessInstanceValuesForChild(container, container, 0, instanceValues, false, ref oldStyle.ChildRecordFromChildIndex);
					return;
				}
			}
			else if (oldFrameworkTemplate != null)
			{
				HybridDictionary instanceValues2 = (value != null) ? value[0] : null;
				StyleHelper.ReleaseInstanceDataForDataTriggers(dataField, instanceValues2, oldStyle, oldFrameworkTemplate);
				if (oldFrameworkTemplate.HasInstanceValues)
				{
					StyleHelper.ProcessInstanceValuesForChild(container, container, 0, instanceValues2, false, ref oldFrameworkTemplate.ChildRecordFromChildIndex);
					return;
				}
			}
			else
			{
				HybridDictionary instanceValues3 = (value != null) ? value[0] : null;
				StyleHelper.ReleaseInstanceDataForDataTriggers(dataField, instanceValues3, oldStyle, oldFrameworkTemplate);
			}
		}

		// Token: 0x06000905 RID: 2309 RVA: 0x0001E122 File Offset: 0x0001C322
		internal static HybridDictionary EnsureInstanceData(UncommonField<HybridDictionary[]> dataField, DependencyObject container, InstanceStyleData dataType)
		{
			return StyleHelper.EnsureInstanceData(dataField, container, dataType, -1);
		}

		// Token: 0x06000906 RID: 2310 RVA: 0x0001E130 File Offset: 0x0001C330
		internal static HybridDictionary EnsureInstanceData(UncommonField<HybridDictionary[]> dataField, DependencyObject container, InstanceStyleData dataType, int initialSize)
		{
			HybridDictionary[] array = dataField.GetValue(container);
			if (array == null)
			{
				array = new HybridDictionary[1];
				dataField.SetValue(container, array);
			}
			if (array[(int)dataType] == null)
			{
				if (initialSize < 0)
				{
					array[(int)dataType] = new HybridDictionary();
				}
				else
				{
					array[(int)dataType] = new HybridDictionary(initialSize);
				}
			}
			return array[(int)dataType];
		}

		// Token: 0x06000907 RID: 2311 RVA: 0x0001E178 File Offset: 0x0001C378
		private static void ProcessInstanceValuesForChild(DependencyObject container, DependencyObject child, int childIndex, HybridDictionary instanceValues, bool apply, ref FrugalStructList<ChildRecord> childRecordFromChildIndex)
		{
			if (childIndex == -1)
			{
				FrameworkElement frameworkElement;
				FrameworkContentElement frameworkContentElement;
				Helper.DowncastToFEorFCE(child, out frameworkElement, out frameworkContentElement, false);
				childIndex = ((frameworkElement != null) ? frameworkElement.TemplateChildIndex : ((frameworkContentElement != null) ? frameworkContentElement.TemplateChildIndex : -1));
			}
			if (0 <= childIndex && childIndex < childRecordFromChildIndex.Count)
			{
				int count = childRecordFromChildIndex[childIndex].ValueLookupListFromProperty.Count;
				for (int i = 0; i < count; i++)
				{
					StyleHelper.ProcessInstanceValuesHelper(ref childRecordFromChildIndex[childIndex].ValueLookupListFromProperty.Entries[i].Value, child, childIndex, instanceValues, apply);
				}
			}
		}

		// Token: 0x06000908 RID: 2312 RVA: 0x0001E204 File Offset: 0x0001C404
		private static void ProcessInstanceValuesHelper(ref ItemStructList<ChildValueLookup> valueLookupList, DependencyObject target, int childIndex, HybridDictionary instanceValues, bool apply)
		{
			for (int i = valueLookupList.Count - 1; i >= 0; i--)
			{
				ValueLookupType lookupType = valueLookupList.List[i].LookupType;
				if (lookupType <= ValueLookupType.Trigger || lookupType == ValueLookupType.DataTrigger)
				{
					DependencyProperty property = valueLookupList.List[i].Property;
					object value = valueLookupList.List[i].Value;
					Freezable freezable;
					if (value is MarkupExtension)
					{
						StyleHelper.ProcessInstanceValue(target, childIndex, instanceValues, property, i, apply);
					}
					else if ((freezable = (value as Freezable)) != null)
					{
						if (!freezable.CheckAccess())
						{
							throw new InvalidOperationException(SR.Get("CrossThreadAccessOfUnshareableFreezable", new object[]
							{
								freezable.GetType().FullName
							}));
						}
						if (!freezable.IsFrozen)
						{
							StyleHelper.ProcessInstanceValue(target, childIndex, instanceValues, property, i, apply);
						}
					}
				}
			}
		}

		// Token: 0x06000909 RID: 2313 RVA: 0x0001E2D0 File Offset: 0x0001C4D0
		internal static void ProcessInstanceValue(DependencyObject target, int childIndex, HybridDictionary instanceValues, DependencyProperty dp, int i, bool apply)
		{
			InstanceValueKey key = new InstanceValueKey(childIndex, dp.GlobalIndex, i);
			if (apply)
			{
				instanceValues[key] = StyleHelper.NotYetApplied;
				return;
			}
			object obj = instanceValues[key];
			instanceValues.Remove(key);
			Expression expression;
			if ((expression = (obj as Expression)) != null)
			{
				expression.OnDetach(target, dp);
				return;
			}
			Freezable doValue;
			if ((doValue = (obj as Freezable)) != null)
			{
				target.RemoveSelfAsInheritanceContext(doValue, dp);
			}
		}

		// Token: 0x0600090A RID: 2314 RVA: 0x0001E334 File Offset: 0x0001C534
		private static void ReleaseInstanceDataForDataTriggers(UncommonField<HybridDictionary[]> dataField, HybridDictionary instanceValues, Style oldStyle, FrameworkTemplate oldFrameworkTemplate)
		{
			if (instanceValues == null)
			{
				return;
			}
			EventHandler<BindingValueChangedEventArgs> handler;
			if (dataField == StyleHelper.StyleDataField)
			{
				handler = new EventHandler<BindingValueChangedEventArgs>(StyleHelper.OnBindingValueInStyleChanged);
			}
			else if (dataField == StyleHelper.TemplateDataField)
			{
				handler = new EventHandler<BindingValueChangedEventArgs>(StyleHelper.OnBindingValueInTemplateChanged);
			}
			else
			{
				handler = new EventHandler<BindingValueChangedEventArgs>(StyleHelper.OnBindingValueInThemeStyleChanged);
			}
			HybridDictionary hybridDictionary = null;
			if (oldStyle != null)
			{
				hybridDictionary = oldStyle._dataTriggerRecordFromBinding;
			}
			else if (oldFrameworkTemplate != null)
			{
				hybridDictionary = oldFrameworkTemplate._dataTriggerRecordFromBinding;
			}
			if (hybridDictionary != null)
			{
				foreach (object obj in hybridDictionary.Keys)
				{
					BindingBase binding = (BindingBase)obj;
					StyleHelper.ReleaseInstanceDataForTriggerBinding(binding, instanceValues, handler);
				}
			}
			HybridDictionary hybridDictionary2 = null;
			if (oldStyle != null)
			{
				hybridDictionary2 = oldStyle.DataTriggersWithActions;
			}
			else if (oldFrameworkTemplate != null)
			{
				hybridDictionary2 = oldFrameworkTemplate.DataTriggersWithActions;
			}
			if (hybridDictionary2 != null)
			{
				foreach (object obj2 in hybridDictionary2.Keys)
				{
					BindingBase binding2 = (BindingBase)obj2;
					StyleHelper.ReleaseInstanceDataForTriggerBinding(binding2, instanceValues, handler);
				}
			}
		}

		// Token: 0x0600090B RID: 2315 RVA: 0x0001E464 File Offset: 0x0001C664
		private static void ReleaseInstanceDataForTriggerBinding(BindingBase binding, HybridDictionary instanceValues, EventHandler<BindingValueChangedEventArgs> handler)
		{
			BindingExpressionBase bindingExpressionBase = (BindingExpressionBase)instanceValues[binding];
			if (bindingExpressionBase != null)
			{
				bindingExpressionBase.ValueChanged -= handler;
				bindingExpressionBase.Detach();
				instanceValues.Remove(binding);
			}
		}

		// Token: 0x0600090C RID: 2316 RVA: 0x0001E498 File Offset: 0x0001C698
		internal static bool ApplyTemplateContent(UncommonField<HybridDictionary[]> dataField, DependencyObject container, FrameworkElementFactory templateRoot, int lastChildIndex, HybridDictionary childIndexFromChildID, FrameworkTemplate frameworkTemplate)
		{
			bool result = false;
			FrameworkElement frameworkElement = container as FrameworkElement;
			if (templateRoot != null)
			{
				EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordXamlBaml, EventTrace.Level.Verbose, EventTrace.Event.WClientParseInstVisTreeBegin);
				StyleHelper.CheckForCircularReferencesInTemplateTree(container, frameworkTemplate);
				List<DependencyObject> list = new List<DependencyObject>(lastChildIndex);
				StyleHelper.TemplatedFeChildrenField.SetValue(container, list);
				List<DependencyObject> list2 = null;
				DependencyObject dependencyObject = templateRoot.InstantiateTree(dataField, container, container, list, ref list2, ref frameworkTemplate.ResourceDependents);
				if (list2 != null)
				{
					list.AddRange(list2);
				}
				result = true;
				if (frameworkElement != null && EventTrace.IsEnabled(EventTrace.Keyword.KeywordXamlBaml, EventTrace.Level.Verbose))
				{
					string text = frameworkElement.Name;
					if (text == null || text.Length == 0)
					{
						text = container.GetHashCode().ToString(CultureInfo.InvariantCulture);
					}
					EventTrace.EventProvider.TraceEvent(EventTrace.Event.WClientParseInstVisTreeEnd, EventTrace.Keyword.KeywordXamlBaml, EventTrace.Level.Verbose, string.Format(CultureInfo.InvariantCulture, "Style.InstantiateSubTree for {0} {1}", new object[]
					{
						container.GetType().Name,
						text
					}));
				}
			}
			else if (frameworkTemplate != null && frameworkTemplate.HasXamlNodeContent)
			{
				EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordXamlBaml, EventTrace.Level.Verbose, EventTrace.Event.WClientParseInstVisTreeBegin);
				StyleHelper.CheckForCircularReferencesInTemplateTree(container, frameworkTemplate);
				List<DependencyObject> list3 = new List<DependencyObject>(lastChildIndex);
				StyleHelper.TemplatedFeChildrenField.SetValue(container, list3);
				DependencyObject dependencyObject2 = frameworkTemplate.LoadContent(container, list3);
				result = true;
				if (frameworkElement != null && EventTrace.IsEnabled(EventTrace.Keyword.KeywordXamlBaml, EventTrace.Level.Verbose))
				{
					string text2 = frameworkElement.Name;
					if (text2 == null || text2.Length == 0)
					{
						text2 = container.GetHashCode().ToString(CultureInfo.InvariantCulture);
					}
					EventTrace.EventProvider.TraceEvent(EventTrace.Event.WClientParseInstVisTreeEnd, EventTrace.Keyword.KeywordXamlBaml, EventTrace.Level.Verbose, string.Format(CultureInfo.InvariantCulture, "Style.InstantiateSubTree for {0} {1}", new object[]
					{
						container.GetType().Name,
						text2
					}));
				}
			}
			else if (frameworkElement != null)
			{
				result = frameworkTemplate.BuildVisualTree(frameworkElement);
			}
			return result;
		}

		// Token: 0x0600090D RID: 2317 RVA: 0x0001E64A File Offset: 0x0001C84A
		internal static void AddCustomTemplateRoot(FrameworkElement container, UIElement child)
		{
			StyleHelper.AddCustomTemplateRoot(container, child, true, false);
		}

		// Token: 0x0600090E RID: 2318 RVA: 0x0001E658 File Offset: 0x0001C858
		internal static void AddCustomTemplateRoot(FrameworkElement container, UIElement child, bool checkVisualParent, bool mustCacheTreeStateOnChild)
		{
			if (child != null && checkVisualParent)
			{
				FrameworkElement frameworkElement = VisualTreeHelper.GetParent(child) as FrameworkElement;
				if (frameworkElement != null)
				{
					frameworkElement.TemplateChild = null;
					frameworkElement.InvalidateMeasure();
				}
			}
			container.TemplateChild = child;
		}

		// Token: 0x0600090F RID: 2319 RVA: 0x0001E690 File Offset: 0x0001C890
		private static void CheckForCircularReferencesInTemplateTree(DependencyObject container, FrameworkTemplate frameworkTemplate)
		{
			DependencyObject templatedParent;
			for (DependencyObject dependencyObject = container; dependencyObject != null; dependencyObject = ((dependencyObject is ContentPresenter) ? null : templatedParent))
			{
				FrameworkElement frameworkElement;
				FrameworkContentElement frameworkContentElement;
				Helper.DowncastToFEorFCE(dependencyObject, out frameworkElement, out frameworkContentElement, false);
				bool flag = frameworkElement != null;
				if (flag)
				{
					templatedParent = frameworkElement.TemplatedParent;
				}
				else
				{
					templatedParent = frameworkContentElement.TemplatedParent;
				}
				if (dependencyObject != container && templatedParent != null && (frameworkTemplate != null && flag) && frameworkElement.TemplateInternal == frameworkTemplate && dependencyObject.GetType() == container.GetType())
				{
					string text = flag ? frameworkElement.Name : frameworkContentElement.Name;
					throw new InvalidOperationException(SR.Get("TemplateCircularReferenceFound", new object[]
					{
						text,
						dependencyObject.GetType()
					}));
				}
			}
		}

		// Token: 0x06000910 RID: 2320 RVA: 0x0001E744 File Offset: 0x0001C944
		internal static void ClearGeneratedSubTree(HybridDictionary[] instanceData, FrameworkElement feContainer, FrameworkContentElement fceContainer, FrameworkTemplate oldFrameworkTemplate)
		{
			List<DependencyObject> value;
			if (feContainer != null)
			{
				value = StyleHelper.TemplatedFeChildrenField.GetValue(feContainer);
				StyleHelper.TemplatedFeChildrenField.ClearValue(feContainer);
			}
			else
			{
				value = StyleHelper.TemplatedFeChildrenField.GetValue(fceContainer);
				StyleHelper.TemplatedFeChildrenField.ClearValue(fceContainer);
			}
			DependencyObject dependencyObject = null;
			if (value != null)
			{
				dependencyObject = value[0];
				if (oldFrameworkTemplate != null)
				{
					StyleHelper.ClearTemplateChain(instanceData, feContainer, fceContainer, value, oldFrameworkTemplate);
				}
			}
			if (dependencyObject != null)
			{
				dependencyObject.ClearValue(NameScope.NameScopeProperty);
			}
			StyleHelper.DetachGeneratedSubTree(feContainer, fceContainer);
		}

		// Token: 0x06000911 RID: 2321 RVA: 0x0001E7B3 File Offset: 0x0001C9B3
		private static void DetachGeneratedSubTree(FrameworkElement feContainer, FrameworkContentElement fceContainer)
		{
			if (feContainer != null)
			{
				feContainer.TemplateChild = null;
				feContainer.HasTemplateGeneratedSubTree = false;
				return;
			}
			fceContainer.HasTemplateGeneratedSubTree = false;
		}

		// Token: 0x06000912 RID: 2322 RVA: 0x0001E7D0 File Offset: 0x0001C9D0
		private static void ClearTemplateChain(HybridDictionary[] instanceData, FrameworkElement feContainer, FrameworkContentElement fceContainer, List<DependencyObject> templateChain, FrameworkTemplate oldFrameworkTemplate)
		{
			FrameworkObject frameworkObject = new FrameworkObject(feContainer, fceContainer);
			HybridDictionary instanceValues = (instanceData != null) ? instanceData[0] : null;
			int[] array = new int[templateChain.Count];
			for (int i = 0; i < templateChain.Count; i++)
			{
				DependencyObject d = templateChain[i];
				FrameworkElement frameworkElement;
				FrameworkContentElement frameworkContentElement;
				StyleHelper.SpecialDowncastToFEorFCE(d, out frameworkElement, out frameworkContentElement, true);
				if (frameworkElement != null)
				{
					array[i] = frameworkElement.TemplateChildIndex;
					frameworkElement._templatedParent = null;
					frameworkElement.TemplateChildIndex = -1;
				}
				else if (frameworkContentElement != null)
				{
					array[i] = frameworkContentElement.TemplateChildIndex;
					frameworkContentElement._templatedParent = null;
					frameworkContentElement.TemplateChildIndex = -1;
				}
			}
			for (int j = 0; j < templateChain.Count; j++)
			{
				DependencyObject dependencyObject = templateChain[j];
				FrameworkObject child = new FrameworkObject(dependencyObject);
				int childIndex = array[j];
				StyleHelper.ProcessInstanceValuesForChild(feContainer, dependencyObject, array[j], instanceValues, false, ref oldFrameworkTemplate.ChildRecordFromChildIndex);
				StyleHelper.InvalidatePropertiesOnTemplateNode(frameworkObject.DO, child, array[j], ref oldFrameworkTemplate.ChildRecordFromChildIndex, true, oldFrameworkTemplate.VisualTree);
				if (child.StoresParentTemplateValues)
				{
					HybridDictionary value = StyleHelper.ParentTemplateValuesField.GetValue(dependencyObject);
					StyleHelper.ParentTemplateValuesField.ClearValue(dependencyObject);
					child.StoresParentTemplateValues = false;
					foreach (object obj in value)
					{
						DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
						DependencyProperty dp = (DependencyProperty)dictionaryEntry.Key;
						if (dictionaryEntry.Value is MarkupExtension)
						{
							StyleHelper.ProcessInstanceValue(dependencyObject, childIndex, instanceValues, dp, -1, false);
						}
						dependencyObject.InvalidateProperty(dp);
					}
				}
			}
		}

		// Token: 0x06000913 RID: 2323 RVA: 0x0001E974 File Offset: 0x0001CB74
		internal static void SpecialDowncastToFEorFCE(DependencyObject d, out FrameworkElement fe, out FrameworkContentElement fce, bool throwIfNeither)
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
			if (throwIfNeither && !(d is Visual3D))
			{
				throw new InvalidOperationException(SR.Get("MustBeFrameworkDerived", new object[]
				{
					d.GetType()
				}));
			}
			fe = null;
			fce = null;
		}

		// Token: 0x06000914 RID: 2324 RVA: 0x0001E9E4 File Offset: 0x0001CBE4
		internal static FrameworkElementFactory FindFEF(FrameworkElementFactory root, int childIndex)
		{
			if (root._childIndex == childIndex)
			{
				return root;
			}
			for (FrameworkElementFactory frameworkElementFactory = root.FirstChild; frameworkElementFactory != null; frameworkElementFactory = frameworkElementFactory.NextSibling)
			{
				FrameworkElementFactory frameworkElementFactory2 = StyleHelper.FindFEF(frameworkElementFactory, childIndex);
				if (frameworkElementFactory2 != null)
				{
					return frameworkElementFactory2;
				}
			}
			return null;
		}

		// Token: 0x06000915 RID: 2325 RVA: 0x0001EA20 File Offset: 0x0001CC20
		private static void ExecuteEventTriggerActionsOnContainer(object sender, RoutedEventArgs e)
		{
			FrameworkElement frameworkElement;
			FrameworkContentElement frameworkContentElement;
			Helper.DowncastToFEorFCE((DependencyObject)sender, out frameworkElement, out frameworkContentElement, false);
			FrameworkTemplate frameworkTemplate = null;
			Style style;
			Style themeStyle;
			if (frameworkElement != null)
			{
				style = frameworkElement.Style;
				themeStyle = frameworkElement.ThemeStyle;
				frameworkTemplate = frameworkElement.TemplateInternal;
			}
			else
			{
				style = frameworkContentElement.Style;
				themeStyle = frameworkContentElement.ThemeStyle;
			}
			if (style != null && style.EventHandlersStore != null)
			{
				StyleHelper.InvokeEventTriggerActions(frameworkElement, frameworkContentElement, style, null, 0, e.RoutedEvent);
			}
			if (themeStyle != null && themeStyle.EventHandlersStore != null)
			{
				StyleHelper.InvokeEventTriggerActions(frameworkElement, frameworkContentElement, themeStyle, null, 0, e.RoutedEvent);
			}
			if (frameworkTemplate != null && frameworkTemplate.EventHandlersStore != null)
			{
				StyleHelper.InvokeEventTriggerActions(frameworkElement, frameworkContentElement, null, frameworkTemplate, 0, e.RoutedEvent);
			}
		}

		// Token: 0x06000916 RID: 2326 RVA: 0x0001EAC0 File Offset: 0x0001CCC0
		private static void ExecuteEventTriggerActionsOnChild(object sender, RoutedEventArgs e)
		{
			FrameworkElement frameworkElement;
			FrameworkContentElement frameworkContentElement;
			Helper.DowncastToFEorFCE((DependencyObject)sender, out frameworkElement, out frameworkContentElement, false);
			DependencyObject templatedParent;
			int templateChildIndex;
			if (frameworkElement != null)
			{
				templatedParent = frameworkElement.TemplatedParent;
				templateChildIndex = frameworkElement.TemplateChildIndex;
			}
			else
			{
				templatedParent = frameworkContentElement.TemplatedParent;
				templateChildIndex = frameworkContentElement.TemplateChildIndex;
			}
			if (templatedParent != null)
			{
				FrameworkElement frameworkElement2;
				FrameworkContentElement fce;
				Helper.DowncastToFEorFCE(templatedParent, out frameworkElement2, out fce, false);
				FrameworkTemplate templateInternal = frameworkElement2.TemplateInternal;
				StyleHelper.InvokeEventTriggerActions(frameworkElement2, fce, null, templateInternal, templateChildIndex, e.RoutedEvent);
			}
		}

		// Token: 0x06000917 RID: 2327 RVA: 0x0001EB2C File Offset: 0x0001CD2C
		private static void InvokeEventTriggerActions(FrameworkElement fe, FrameworkContentElement fce, Style ownerStyle, FrameworkTemplate frameworkTemplate, int childIndex, RoutedEvent Event)
		{
			List<TriggerAction> list;
			if (ownerStyle != null)
			{
				list = ((ownerStyle._triggerActions != null) ? (ownerStyle._triggerActions[Event] as List<TriggerAction>) : null);
			}
			else
			{
				list = ((frameworkTemplate._triggerActions != null) ? (frameworkTemplate._triggerActions[Event] as List<TriggerAction>) : null);
			}
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					TriggerAction triggerAction = list[i];
					int triggerChildIndex = ((EventTrigger)triggerAction.ContainingTrigger).TriggerChildIndex;
					if (childIndex == triggerChildIndex)
					{
						triggerAction.Invoke(fe, fce, ownerStyle, frameworkTemplate, Storyboard.Layers.StyleOrTemplateEventTrigger);
					}
				}
			}
		}

		// Token: 0x06000918 RID: 2328 RVA: 0x0001EBBC File Offset: 0x0001CDBC
		internal static object GetChildValue(UncommonField<HybridDictionary[]> dataField, DependencyObject container, int childIndex, FrameworkObject child, DependencyProperty dp, ref FrugalStructList<ChildRecord> childRecordFromChildIndex, ref EffectiveValueEntry entry, out ValueLookupType sourceType, FrameworkElementFactory templateRoot)
		{
			object result = DependencyProperty.UnsetValue;
			sourceType = ValueLookupType.Simple;
			if (0 <= childIndex && childIndex < childRecordFromChildIndex.Count)
			{
				ChildRecord childRecord = childRecordFromChildIndex[childIndex];
				int num = childRecord.ValueLookupListFromProperty.Search(dp.GlobalIndex);
				if (num >= 0 && childRecord.ValueLookupListFromProperty.Entries[num].Value.Count > 0)
				{
					result = StyleHelper.GetChildValueHelper(dataField, ref childRecord.ValueLookupListFromProperty.Entries[num].Value, dp, container, child, childIndex, true, ref entry, out sourceType, templateRoot);
				}
			}
			return result;
		}

		// Token: 0x06000919 RID: 2329 RVA: 0x0001EC4C File Offset: 0x0001CE4C
		private static object GetChildValueHelper(UncommonField<HybridDictionary[]> dataField, ref ItemStructList<ChildValueLookup> valueLookupList, DependencyProperty dp, DependencyObject container, FrameworkObject child, int childIndex, bool styleLookup, ref EffectiveValueEntry entry, out ValueLookupType sourceType, FrameworkElementFactory templateRoot)
		{
			object obj = DependencyProperty.UnsetValue;
			sourceType = ValueLookupType.Simple;
			for (int i = valueLookupList.Count - 1; i >= 0; i--)
			{
				sourceType = valueLookupList.List[i].LookupType;
				switch (valueLookupList.List[i].LookupType)
				{
				case ValueLookupType.Simple:
					obj = valueLookupList.List[i].Value;
					break;
				case ValueLookupType.Trigger:
				case ValueLookupType.PropertyTriggerResource:
				case ValueLookupType.DataTrigger:
				case ValueLookupType.DataTriggerResource:
				{
					bool flag = true;
					if (valueLookupList.List[i].Conditions != null)
					{
						int num = 0;
						while (flag && num < valueLookupList.List[i].Conditions.Length)
						{
							ValueLookupType lookupType = valueLookupList.List[i].LookupType;
							if (lookupType - ValueLookupType.Trigger > 1)
							{
								if (lookupType - ValueLookupType.DataTrigger > 1)
								{
								}
								object state = StyleHelper.GetDataTriggerValue(dataField, container, valueLookupList.List[i].Conditions[num].Binding);
								flag = valueLookupList.List[i].Conditions[num].ConvertAndMatch(state);
							}
							else
							{
								int sourceChildIndex = valueLookupList.List[i].Conditions[num].SourceChildIndex;
								DependencyObject dependencyObject;
								if (sourceChildIndex == 0)
								{
									dependencyObject = container;
								}
								else
								{
									dependencyObject = StyleHelper.GetChild(container, sourceChildIndex);
								}
								DependencyProperty property = valueLookupList.List[i].Conditions[num].Property;
								object state;
								if (dependencyObject != null)
								{
									state = dependencyObject.GetValue(property);
								}
								else
								{
									Type forType;
									if (templateRoot != null)
									{
										forType = StyleHelper.FindFEF(templateRoot, sourceChildIndex).Type;
									}
									else
									{
										forType = (container as FrameworkElement).TemplateInternal.ChildTypeFromChildIndex[sourceChildIndex];
									}
									state = property.GetDefaultValue(forType);
								}
								flag = valueLookupList.List[i].Conditions[num].Match(state);
							}
							num++;
						}
					}
					if (flag)
					{
						if (valueLookupList.List[i].LookupType == ValueLookupType.PropertyTriggerResource || valueLookupList.List[i].LookupType == ValueLookupType.DataTriggerResource)
						{
							object obj2;
							obj = FrameworkElement.FindResourceInternal(child.FE, child.FCE, dp, valueLookupList.List[i].Value, null, true, false, null, false, out obj2);
							StyleHelper.SealIfSealable(obj);
						}
						else
						{
							obj = valueLookupList.List[i].Value;
						}
					}
					break;
				}
				case ValueLookupType.TemplateBinding:
				{
					TemplateBindingExtension templateBindingExtension = (TemplateBindingExtension)valueLookupList.List[i].Value;
					DependencyProperty property2 = templateBindingExtension.Property;
					obj = container.GetValue(property2);
					if (templateBindingExtension.Converter != null)
					{
						DependencyProperty property3 = valueLookupList.List[i].Property;
						CultureInfo compatibleCulture = child.Language.GetCompatibleCulture();
						obj = templateBindingExtension.Converter.Convert(obj, property3.PropertyType, templateBindingExtension.ConverterParameter, compatibleCulture);
					}
					if (obj != DependencyProperty.UnsetValue && !dp.IsValidValue(obj))
					{
						obj = DependencyProperty.UnsetValue;
					}
					break;
				}
				case ValueLookupType.Resource:
				{
					object obj3;
					obj = FrameworkElement.FindResourceInternal(child.FE, child.FCE, dp, valueLookupList.List[i].Value, null, true, false, null, false, out obj3);
					StyleHelper.SealIfSealable(obj);
					break;
				}
				}
				if (obj != DependencyProperty.UnsetValue)
				{
					entry.Value = obj;
					ValueLookupType lookupType = valueLookupList.List[i].LookupType;
					if (lookupType <= ValueLookupType.Trigger || lookupType == ValueLookupType.DataTrigger)
					{
						Freezable freezable;
						if (obj is MarkupExtension)
						{
							obj = StyleHelper.GetInstanceValue(dataField, container, child.FE, child.FCE, childIndex, valueLookupList.List[i].Property, i, ref entry);
						}
						else if ((freezable = (obj as Freezable)) != null && !freezable.IsFrozen)
						{
							obj = StyleHelper.GetInstanceValue(dataField, container, child.FE, child.FCE, childIndex, valueLookupList.List[i].Property, i, ref entry);
						}
					}
				}
				if (obj != DependencyProperty.UnsetValue)
				{
					break;
				}
			}
			return obj;
		}

		// Token: 0x0600091A RID: 2330 RVA: 0x0001F038 File Offset: 0x0001D238
		internal static object GetDataTriggerValue(UncommonField<HybridDictionary[]> dataField, DependencyObject container, BindingBase binding)
		{
			HybridDictionary[] value = dataField.GetValue(container);
			HybridDictionary hybridDictionary = StyleHelper.EnsureInstanceData(dataField, container, InstanceStyleData.InstanceValues);
			BindingExpressionBase bindingExpressionBase = (BindingExpressionBase)hybridDictionary[binding];
			if (bindingExpressionBase == null)
			{
				bindingExpressionBase = BindingExpressionBase.CreateUntargetedBindingExpression(container, binding);
				hybridDictionary[binding] = bindingExpressionBase;
				if (dataField == StyleHelper.StyleDataField)
				{
					bindingExpressionBase.ValueChanged += StyleHelper.OnBindingValueInStyleChanged;
				}
				else if (dataField == StyleHelper.TemplateDataField)
				{
					bindingExpressionBase.ResolveNamesInTemplate = true;
					bindingExpressionBase.ValueChanged += StyleHelper.OnBindingValueInTemplateChanged;
				}
				else
				{
					bindingExpressionBase.ValueChanged += StyleHelper.OnBindingValueInThemeStyleChanged;
				}
				bindingExpressionBase.Attach(container);
			}
			return bindingExpressionBase.Value;
		}

		// Token: 0x0600091B RID: 2331 RVA: 0x0001F0D4 File Offset: 0x0001D2D4
		internal static object GetInstanceValue(UncommonField<HybridDictionary[]> dataField, DependencyObject container, FrameworkElement feChild, FrameworkContentElement fceChild, int childIndex, DependencyProperty dp, int i, ref EffectiveValueEntry entry)
		{
			object value = entry.Value;
			DependencyObject dependencyObject = null;
			FrameworkElement frameworkElement;
			FrameworkContentElement frameworkContentElement;
			Helper.DowncastToFEorFCE(container, out frameworkElement, out frameworkContentElement, true);
			HybridDictionary[] array = (dataField != null) ? dataField.GetValue(container) : null;
			HybridDictionary hybridDictionary = (array != null) ? array[0] : null;
			InstanceValueKey key = new InstanceValueKey(childIndex, dp.GlobalIndex, i);
			object obj = (hybridDictionary != null) ? hybridDictionary[key] : null;
			bool flag = (feChild != null) ? feChild.IsRequestingExpression : fceChild.IsRequestingExpression;
			if (obj == null)
			{
				obj = StyleHelper.NotYetApplied;
			}
			Expression expression = obj as Expression;
			if (expression != null && expression.HasBeenDetached)
			{
				obj = StyleHelper.NotYetApplied;
			}
			if (obj == StyleHelper.NotYetApplied)
			{
				dependencyObject = feChild;
				if (dependencyObject == null)
				{
					dependencyObject = fceChild;
				}
				MarkupExtension markupExtension;
				Freezable freezable;
				if ((markupExtension = (value as MarkupExtension)) != null)
				{
					if (flag && !((feChild != null) ? feChild.IsInitialized : fceChild.IsInitialized))
					{
						return DependencyProperty.UnsetValue;
					}
					ProvideValueServiceProvider provideValueServiceProvider = new ProvideValueServiceProvider();
					provideValueServiceProvider.SetData(dependencyObject, dp);
					obj = markupExtension.ProvideValue(provideValueServiceProvider);
				}
				else if ((freezable = (value as Freezable)) != null)
				{
					obj = freezable.Clone();
					dependencyObject.ProvideSelfAsInheritanceContext(obj, dp);
				}
				hybridDictionary[key] = obj;
				if (obj != DependencyProperty.UnsetValue)
				{
					expression = (obj as Expression);
					if (expression != null)
					{
						expression.OnAttach(dependencyObject, dp);
					}
				}
			}
			if (expression != null)
			{
				if (!flag)
				{
					if (dependencyObject == null)
					{
						dependencyObject = feChild;
						if (dependencyObject == null)
						{
							dependencyObject = fceChild;
						}
					}
					entry.ResetValue(DependencyObject.ExpressionInAlternativeStore, true);
					entry.SetExpressionValue(expression.GetValue(dependencyObject, dp), DependencyObject.ExpressionInAlternativeStore);
				}
				else
				{
					entry.Value = obj;
				}
			}
			else
			{
				entry.Value = obj;
			}
			return obj;
		}

		// Token: 0x0600091C RID: 2332 RVA: 0x0001F265 File Offset: 0x0001D465
		internal static bool ShouldGetValueFromStyle(DependencyProperty dp)
		{
			return dp != FrameworkElement.StyleProperty;
		}

		// Token: 0x0600091D RID: 2333 RVA: 0x0001F272 File Offset: 0x0001D472
		internal static bool ShouldGetValueFromThemeStyle(DependencyProperty dp)
		{
			return dp != FrameworkElement.StyleProperty && dp != FrameworkElement.DefaultStyleKeyProperty && dp != FrameworkElement.OverridesDefaultStyleProperty;
		}

		// Token: 0x0600091E RID: 2334 RVA: 0x0001F291 File Offset: 0x0001D491
		internal static bool ShouldGetValueFromTemplate(DependencyProperty dp)
		{
			return dp != FrameworkElement.StyleProperty && dp != FrameworkElement.DefaultStyleKeyProperty && dp != FrameworkElement.OverridesDefaultStyleProperty && dp != Control.TemplateProperty && dp != ContentPresenter.TemplateProperty;
		}

		// Token: 0x0600091F RID: 2335 RVA: 0x0001F2C0 File Offset: 0x0001D4C0
		internal static void DoStyleInvalidations(FrameworkElement fe, FrameworkContentElement fce, Style oldStyle, Style newStyle)
		{
			if (oldStyle != newStyle)
			{
				DependencyObject dependencyObject = (fe != null) ? fe : fce;
				StyleHelper.UpdateLoadedFlag(dependencyObject, oldStyle, newStyle);
				StyleHelper.UpdateInstanceData(StyleHelper.StyleDataField, fe, fce, oldStyle, newStyle, null, null, (InternalFlags)0U);
				if (newStyle != null && newStyle.HasResourceReferences)
				{
					if (fe != null)
					{
						fe.HasResourceReference = true;
					}
					else
					{
						fce.HasResourceReference = true;
					}
				}
				FrugalStructList<ContainerDependent> frugalStructList = (oldStyle != null) ? oldStyle.ContainerDependents : StyleHelper.EmptyContainerDependents;
				FrugalStructList<ContainerDependent> frugalStructList2 = (newStyle != null) ? newStyle.ContainerDependents : StyleHelper.EmptyContainerDependents;
				FrugalStructList<ContainerDependent> frugalStructList3 = default(FrugalStructList<ContainerDependent>);
				StyleHelper.InvalidateContainerDependents(dependencyObject, ref frugalStructList3, ref frugalStructList, ref frugalStructList2);
				StyleHelper.DoStyleResourcesInvalidations(dependencyObject, fe, fce, oldStyle, newStyle);
				if (fe != null)
				{
					fe.OnStyleChanged(oldStyle, newStyle);
					return;
				}
				fce.OnStyleChanged(oldStyle, newStyle);
			}
		}

		// Token: 0x06000920 RID: 2336 RVA: 0x0001F368 File Offset: 0x0001D568
		internal static void DoThemeStyleInvalidations(FrameworkElement fe, FrameworkContentElement fce, Style oldThemeStyle, Style newThemeStyle, Style style)
		{
			if (oldThemeStyle != newThemeStyle && newThemeStyle != style)
			{
				DependencyObject dependencyObject = (fe != null) ? fe : fce;
				StyleHelper.UpdateLoadedFlag(dependencyObject, oldThemeStyle, newThemeStyle);
				StyleHelper.UpdateInstanceData(StyleHelper.ThemeStyleDataField, fe, fce, oldThemeStyle, newThemeStyle, null, null, (InternalFlags)0U);
				if (newThemeStyle != null && newThemeStyle.HasResourceReferences)
				{
					if (fe != null)
					{
						fe.HasResourceReference = true;
					}
					else
					{
						fce.HasResourceReference = true;
					}
				}
				FrugalStructList<ContainerDependent> frugalStructList = (oldThemeStyle != null) ? oldThemeStyle.ContainerDependents : StyleHelper.EmptyContainerDependents;
				FrugalStructList<ContainerDependent> frugalStructList2 = (newThemeStyle != null) ? newThemeStyle.ContainerDependents : StyleHelper.EmptyContainerDependents;
				FrugalStructList<ContainerDependent> frugalStructList3 = (style != null) ? style.ContainerDependents : default(FrugalStructList<ContainerDependent>);
				StyleHelper.InvalidateContainerDependents(dependencyObject, ref frugalStructList3, ref frugalStructList, ref frugalStructList2);
				StyleHelper.DoStyleResourcesInvalidations(dependencyObject, fe, fce, oldThemeStyle, newThemeStyle);
			}
		}

		// Token: 0x06000921 RID: 2337 RVA: 0x0001F414 File Offset: 0x0001D614
		internal static void DoTemplateInvalidations(FrameworkElement feContainer, FrameworkTemplate oldFrameworkTemplate)
		{
			HybridDictionary[] value = StyleHelper.TemplateDataField.GetValue(feContainer);
			FrameworkTemplate templateInternal = feContainer.TemplateInternal;
			object obj = templateInternal;
			bool flag = templateInternal != null && templateInternal.HasResourceReferences;
			StyleHelper.UpdateLoadedFlag(feContainer, oldFrameworkTemplate, templateInternal);
			if (oldFrameworkTemplate != obj)
			{
				StyleHelper.UpdateInstanceData(StyleHelper.TemplateDataField, feContainer, null, null, null, oldFrameworkTemplate, templateInternal, InternalFlags.HasTemplateGeneratedSubTree);
				if (obj != null && flag)
				{
					feContainer.HasResourceReference = true;
				}
				StyleHelper.UpdateLoadedFlag(feContainer, oldFrameworkTemplate, templateInternal);
				FrameworkElementFactory frameworkElementFactory = (oldFrameworkTemplate != null) ? oldFrameworkTemplate.VisualTree : null;
				FrameworkElementFactory frameworkElementFactory2 = (templateInternal != null) ? templateInternal.VisualTree : null;
				bool flag2 = oldFrameworkTemplate != null && oldFrameworkTemplate.CanBuildVisualTree;
				bool hasTemplateGeneratedSubTree = feContainer.HasTemplateGeneratedSubTree;
				FrugalStructList<ContainerDependent> frugalStructList = (oldFrameworkTemplate != null) ? oldFrameworkTemplate.ContainerDependents : StyleHelper.EmptyContainerDependents;
				FrugalStructList<ContainerDependent> frugalStructList2 = (templateInternal != null) ? templateInternal.ContainerDependents : StyleHelper.EmptyContainerDependents;
				if (hasTemplateGeneratedSubTree)
				{
					StyleHelper.ClearGeneratedSubTree(value, feContainer, null, oldFrameworkTemplate);
				}
				FrugalStructList<ContainerDependent> frugalStructList3 = default(FrugalStructList<ContainerDependent>);
				StyleHelper.InvalidateContainerDependents(feContainer, ref frugalStructList3, ref frugalStructList, ref frugalStructList2);
				StyleHelper.DoTemplateResourcesInvalidations(feContainer, feContainer, null, oldFrameworkTemplate, obj);
				feContainer.OnTemplateChangedInternal(oldFrameworkTemplate, templateInternal);
				return;
			}
			if (templateInternal != null && feContainer.HasTemplateGeneratedSubTree && templateInternal.VisualTree == null && !templateInternal.HasXamlNodeContent)
			{
				StyleHelper.ClearGeneratedSubTree(value, feContainer, null, oldFrameworkTemplate);
				feContainer.InvalidateMeasure();
			}
		}

		// Token: 0x06000922 RID: 2338 RVA: 0x0001F540 File Offset: 0x0001D740
		internal static void DoStyleResourcesInvalidations(DependencyObject container, FrameworkElement fe, FrameworkContentElement fce, Style oldStyle, Style newStyle)
		{
			if (!((fe != null) ? fe.AncestorChangeInProgress : fce.AncestorChangeInProgress))
			{
				List<ResourceDictionary> resourceDictionariesFromStyle = StyleHelper.GetResourceDictionariesFromStyle(oldStyle);
				List<ResourceDictionary> resourceDictionariesFromStyle2 = StyleHelper.GetResourceDictionariesFromStyle(newStyle);
				if ((resourceDictionariesFromStyle != null && resourceDictionariesFromStyle.Count > 0) || (resourceDictionariesFromStyle2 != null && resourceDictionariesFromStyle2.Count > 0))
				{
					StyleHelper.SetShouldLookupImplicitStyles(new FrameworkObject(fe, fce), resourceDictionariesFromStyle2);
					TreeWalkHelper.InvalidateOnResourcesChange(fe, fce, new ResourcesChangeInfo(resourceDictionariesFromStyle, resourceDictionariesFromStyle2, true, false, container));
				}
			}
		}

		// Token: 0x06000923 RID: 2339 RVA: 0x0001F5A8 File Offset: 0x0001D7A8
		internal static void DoTemplateResourcesInvalidations(DependencyObject container, FrameworkElement fe, FrameworkContentElement fce, object oldTemplate, object newTemplate)
		{
			if (!((fe != null) ? fe.AncestorChangeInProgress : fce.AncestorChangeInProgress))
			{
				List<ResourceDictionary> resourceDictionaryFromTemplate = StyleHelper.GetResourceDictionaryFromTemplate(oldTemplate);
				List<ResourceDictionary> resourceDictionaryFromTemplate2 = StyleHelper.GetResourceDictionaryFromTemplate(newTemplate);
				if (resourceDictionaryFromTemplate != resourceDictionaryFromTemplate2)
				{
					StyleHelper.SetShouldLookupImplicitStyles(new FrameworkObject(fe, fce), resourceDictionaryFromTemplate2);
					TreeWalkHelper.InvalidateOnResourcesChange(fe, fce, new ResourcesChangeInfo(resourceDictionaryFromTemplate, resourceDictionaryFromTemplate2, false, true, container));
				}
			}
		}

		// Token: 0x06000924 RID: 2340 RVA: 0x0001F5FC File Offset: 0x0001D7FC
		private static void SetShouldLookupImplicitStyles(FrameworkObject fo, List<ResourceDictionary> dictionaries)
		{
			if (dictionaries != null && dictionaries.Count > 0 && !fo.ShouldLookupImplicitStyles)
			{
				for (int i = 0; i < dictionaries.Count; i++)
				{
					if (dictionaries[i].HasImplicitStyles)
					{
						fo.ShouldLookupImplicitStyles = true;
						return;
					}
				}
			}
		}

		// Token: 0x06000925 RID: 2341 RVA: 0x0001F648 File Offset: 0x0001D848
		private static List<ResourceDictionary> GetResourceDictionariesFromStyle(Style style)
		{
			List<ResourceDictionary> list = null;
			while (style != null)
			{
				if (style._resources != null)
				{
					if (list == null)
					{
						list = new List<ResourceDictionary>(1);
					}
					list.Add(style._resources);
				}
				style = style.BasedOn;
			}
			return list;
		}

		// Token: 0x06000926 RID: 2342 RVA: 0x0001F684 File Offset: 0x0001D884
		private static List<ResourceDictionary> GetResourceDictionaryFromTemplate(object template)
		{
			ResourceDictionary resourceDictionary = null;
			if (template is FrameworkTemplate)
			{
				resourceDictionary = ((FrameworkTemplate)template)._resources;
			}
			if (resourceDictionary != null)
			{
				return new List<ResourceDictionary>(1)
				{
					resourceDictionary
				};
			}
			return null;
		}

		// Token: 0x06000927 RID: 2343 RVA: 0x0001F6BC File Offset: 0x0001D8BC
		internal static void UpdateLoadedFlag(DependencyObject d, Style oldStyle, Style newStyle)
		{
			Invariant.Assert(oldStyle != null || newStyle != null);
			if ((oldStyle == null || !oldStyle.HasLoadedChangeHandler) && newStyle != null && newStyle.HasLoadedChangeHandler)
			{
				BroadcastEventHelper.AddHasLoadedChangeHandlerFlagInAncestry(d);
				return;
			}
			if (oldStyle != null && oldStyle.HasLoadedChangeHandler && (newStyle == null || !newStyle.HasLoadedChangeHandler))
			{
				BroadcastEventHelper.RemoveHasLoadedChangeHandlerFlagInAncestry(d);
			}
		}

		// Token: 0x06000928 RID: 2344 RVA: 0x0001F711 File Offset: 0x0001D911
		internal static void UpdateLoadedFlag(DependencyObject d, FrameworkTemplate oldFrameworkTemplate, FrameworkTemplate newFrameworkTemplate)
		{
			if ((oldFrameworkTemplate == null || !oldFrameworkTemplate.HasLoadedChangeHandler) && newFrameworkTemplate != null && newFrameworkTemplate.HasLoadedChangeHandler)
			{
				BroadcastEventHelper.AddHasLoadedChangeHandlerFlagInAncestry(d);
				return;
			}
			if (oldFrameworkTemplate != null && oldFrameworkTemplate.HasLoadedChangeHandler && (newFrameworkTemplate == null || !newFrameworkTemplate.HasLoadedChangeHandler))
			{
				BroadcastEventHelper.RemoveHasLoadedChangeHandlerFlagInAncestry(d);
			}
		}

		// Token: 0x06000929 RID: 2345 RVA: 0x0001F74C File Offset: 0x0001D94C
		internal static void InvalidateContainerDependents(DependencyObject container, ref FrugalStructList<ContainerDependent> exclusionContainerDependents, ref FrugalStructList<ContainerDependent> oldContainerDependents, ref FrugalStructList<ContainerDependent> newContainerDependents)
		{
			int count = oldContainerDependents.Count;
			for (int i = 0; i < count; i++)
			{
				DependencyProperty property = oldContainerDependents[i].Property;
				if (!StyleHelper.IsSetOnContainer(property, ref exclusionContainerDependents, false))
				{
					container.InvalidateProperty(property);
				}
			}
			count = newContainerDependents.Count;
			if (count > 0)
			{
				FrameworkObject fo = new FrameworkObject(container);
				for (int j = 0; j < count; j++)
				{
					DependencyProperty property2 = newContainerDependents[j].Property;
					if (!StyleHelper.IsSetOnContainer(property2, ref exclusionContainerDependents, false) && !StyleHelper.IsSetOnContainer(property2, ref oldContainerDependents, false))
					{
						StyleHelper.ApplyStyleOrTemplateValue(fo, property2);
					}
				}
			}
		}

		// Token: 0x0600092A RID: 2346 RVA: 0x0001F7DC File Offset: 0x0001D9DC
		internal static void ApplyTemplatedParentValue(DependencyObject container, FrameworkObject child, int childIndex, ref FrugalStructList<ChildRecord> childRecordFromChildIndex, DependencyProperty dp, FrameworkElementFactory templateRoot)
		{
			EffectiveValueEntry effectiveValueEntry = new EffectiveValueEntry(dp);
			effectiveValueEntry.Value = DependencyProperty.UnsetValue;
			if (StyleHelper.GetValueFromTemplatedParent(container, childIndex, child, dp, ref childRecordFromChildIndex, templateRoot, ref effectiveValueEntry))
			{
				DependencyObject @do = child.DO;
				@do.UpdateEffectiveValue(@do.LookupEntry(dp.GlobalIndex), dp, dp.GetMetadata(@do.DependencyObjectType), default(EffectiveValueEntry), ref effectiveValueEntry, false, false, OperationType.Unknown);
			}
		}

		// Token: 0x0600092B RID: 2347 RVA: 0x0001F848 File Offset: 0x0001DA48
		internal static bool IsValueDynamic(DependencyObject container, int childIndex, DependencyProperty dp)
		{
			bool result = false;
			FrameworkObject frameworkObject = new FrameworkObject(container);
			FrameworkTemplate templateInternal = frameworkObject.TemplateInternal;
			if (templateInternal != null)
			{
				FrugalStructList<ChildRecord> childRecordFromChildIndex = templateInternal.ChildRecordFromChildIndex;
				if (0 <= childIndex && childIndex < childRecordFromChildIndex.Count)
				{
					ChildRecord childRecord = childRecordFromChildIndex[childIndex];
					int num = childRecord.ValueLookupListFromProperty.Search(dp.GlobalIndex);
					if (num >= 0 && childRecord.ValueLookupListFromProperty.Entries[num].Value.Count > 0)
					{
						ChildValueLookup childValueLookup = childRecord.ValueLookupListFromProperty.Entries[num].Value.List[0];
						result = (childValueLookup.LookupType == ValueLookupType.Resource || childValueLookup.LookupType == ValueLookupType.TemplateBinding || (childValueLookup.LookupType == ValueLookupType.Simple && childValueLookup.Value is BindingBase));
					}
				}
			}
			return result;
		}

		// Token: 0x0600092C RID: 2348 RVA: 0x0001F924 File Offset: 0x0001DB24
		internal static bool GetValueFromTemplatedParent(DependencyObject container, int childIndex, FrameworkObject child, DependencyProperty dp, ref FrugalStructList<ChildRecord> childRecordFromChildIndex, FrameworkElementFactory templateRoot, ref EffectiveValueEntry entry)
		{
			ValueLookupType valueLookupType = ValueLookupType.Simple;
			object obj = StyleHelper.GetChildValue(StyleHelper.TemplateDataField, container, childIndex, child, dp, ref childRecordFromChildIndex, ref entry, out valueLookupType, templateRoot);
			if (obj != DependencyProperty.UnsetValue)
			{
				if (valueLookupType == ValueLookupType.Trigger || valueLookupType == ValueLookupType.PropertyTriggerResource || valueLookupType == ValueLookupType.DataTrigger || valueLookupType == ValueLookupType.DataTriggerResource)
				{
					entry.BaseValueSourceInternal = BaseValueSourceInternal.ParentTemplateTrigger;
				}
				else
				{
					entry.BaseValueSourceInternal = BaseValueSourceInternal.ParentTemplate;
				}
				return true;
			}
			if (child.StoresParentTemplateValues)
			{
				HybridDictionary value = StyleHelper.ParentTemplateValuesField.GetValue(child.DO);
				if (value.Contains(dp))
				{
					entry.BaseValueSourceInternal = BaseValueSourceInternal.ParentTemplate;
					obj = value[dp];
					entry.Value = obj;
					if (obj is MarkupExtension)
					{
						StyleHelper.GetInstanceValue(StyleHelper.TemplateDataField, container, child.FE, child.FCE, childIndex, dp, -1, ref entry);
					}
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600092D RID: 2349 RVA: 0x0001F9E0 File Offset: 0x0001DBE0
		internal static void ApplyStyleOrTemplateValue(FrameworkObject fo, DependencyProperty dp)
		{
			EffectiveValueEntry effectiveValueEntry = new EffectiveValueEntry(dp);
			effectiveValueEntry.Value = DependencyProperty.UnsetValue;
			if (StyleHelper.GetValueFromStyleOrTemplate(fo, dp, ref effectiveValueEntry))
			{
				DependencyObject @do = fo.DO;
				@do.UpdateEffectiveValue(@do.LookupEntry(dp.GlobalIndex), dp, dp.GetMetadata(@do.DependencyObjectType), default(EffectiveValueEntry), ref effectiveValueEntry, false, false, OperationType.Unknown);
			}
		}

		// Token: 0x0600092E RID: 2350 RVA: 0x0001FA44 File Offset: 0x0001DC44
		internal static bool GetValueFromStyleOrTemplate(FrameworkObject fo, DependencyProperty dp, ref EffectiveValueEntry entry)
		{
			ValueLookupType valueLookupType = ValueLookupType.Simple;
			object obj = DependencyProperty.UnsetValue;
			EffectiveValueEntry effectiveValueEntry = entry;
			Style style = fo.Style;
			if (style != null && StyleHelper.ShouldGetValueFromStyle(dp))
			{
				object childValue = StyleHelper.GetChildValue(StyleHelper.StyleDataField, fo.DO, 0, fo, dp, ref style.ChildRecordFromChildIndex, ref effectiveValueEntry, out valueLookupType, null);
				if (childValue != DependencyProperty.UnsetValue)
				{
					if (valueLookupType == ValueLookupType.Trigger || valueLookupType == ValueLookupType.PropertyTriggerResource || valueLookupType == ValueLookupType.DataTrigger || valueLookupType == ValueLookupType.DataTriggerResource)
					{
						entry = effectiveValueEntry;
						entry.BaseValueSourceInternal = BaseValueSourceInternal.StyleTrigger;
						return true;
					}
					obj = childValue;
				}
			}
			if (StyleHelper.ShouldGetValueFromTemplate(dp))
			{
				FrameworkTemplate templateInternal = fo.TemplateInternal;
				if (templateInternal != null)
				{
					object childValue = StyleHelper.GetChildValue(StyleHelper.TemplateDataField, fo.DO, 0, fo, dp, ref templateInternal.ChildRecordFromChildIndex, ref entry, out valueLookupType, templateInternal.VisualTree);
					if (childValue != DependencyProperty.UnsetValue)
					{
						entry.BaseValueSourceInternal = BaseValueSourceInternal.TemplateTrigger;
						return true;
					}
				}
			}
			if (obj != DependencyProperty.UnsetValue)
			{
				entry = effectiveValueEntry;
				entry.BaseValueSourceInternal = BaseValueSourceInternal.Style;
				return true;
			}
			if (StyleHelper.ShouldGetValueFromThemeStyle(dp))
			{
				Style themeStyle = fo.ThemeStyle;
				if (themeStyle != null)
				{
					object childValue = StyleHelper.GetChildValue(StyleHelper.ThemeStyleDataField, fo.DO, 0, fo, dp, ref themeStyle.ChildRecordFromChildIndex, ref entry, out valueLookupType, null);
					if (childValue != DependencyProperty.UnsetValue)
					{
						if (valueLookupType == ValueLookupType.Trigger || valueLookupType == ValueLookupType.PropertyTriggerResource || valueLookupType == ValueLookupType.DataTrigger || valueLookupType == ValueLookupType.DataTriggerResource)
						{
							entry.BaseValueSourceInternal = BaseValueSourceInternal.ThemeStyleTrigger;
						}
						else
						{
							entry.BaseValueSourceInternal = BaseValueSourceInternal.ThemeStyle;
						}
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600092F RID: 2351 RVA: 0x0001FB84 File Offset: 0x0001DD84
		internal static void SortResourceDependents(ref FrugalStructList<ChildPropertyDependent> resourceDependents)
		{
			int count = resourceDependents.Count;
			for (int i = 1; i < count; i++)
			{
				ChildPropertyDependent childPropertyDependent = resourceDependents[i];
				int childIndex = childPropertyDependent.ChildIndex;
				int globalIndex = childPropertyDependent.Property.GlobalIndex;
				int num = i - 1;
				while (num >= 0 && (childIndex < resourceDependents[num].ChildIndex || (childIndex == resourceDependents[num].ChildIndex && globalIndex < resourceDependents[num].Property.GlobalIndex)))
				{
					resourceDependents[num + 1] = resourceDependents[num];
					num--;
				}
				if (num < i - 1)
				{
					resourceDependents[num + 1] = childPropertyDependent;
				}
			}
		}

		// Token: 0x06000930 RID: 2352 RVA: 0x0001FC34 File Offset: 0x0001DE34
		internal static void InvalidateResourceDependents(DependencyObject container, ResourcesChangeInfo info, ref FrugalStructList<ChildPropertyDependent> resourceDependents, bool invalidateVisualTreeToo)
		{
			List<DependencyObject> value = StyleHelper.TemplatedFeChildrenField.GetValue(container);
			for (int i = 0; i < resourceDependents.Count; i++)
			{
				if (info.Contains(resourceDependents[i].Name, false))
				{
					DependencyObject dependencyObject = null;
					DependencyProperty property = resourceDependents[i].Property;
					int childIndex = resourceDependents[i].ChildIndex;
					if (childIndex == 0)
					{
						dependencyObject = container;
					}
					else if (invalidateVisualTreeToo)
					{
						dependencyObject = StyleHelper.GetChild(value, childIndex);
						if (dependencyObject == null)
						{
							throw new InvalidOperationException(SR.Get("ChildTemplateInstanceDoesNotExist"));
						}
					}
					if (dependencyObject != null)
					{
						dependencyObject.InvalidateProperty(property);
						int globalIndex = property.GlobalIndex;
						while (++i < resourceDependents.Count && resourceDependents[i].ChildIndex == childIndex && resourceDependents[i].Property.GlobalIndex == globalIndex)
						{
						}
						i--;
					}
				}
			}
		}

		// Token: 0x06000931 RID: 2353 RVA: 0x0001FD0C File Offset: 0x0001DF0C
		internal static void InvalidateResourceDependentsForChild(DependencyObject container, DependencyObject child, int childIndex, ResourcesChangeInfo info, FrameworkTemplate parentTemplate)
		{
			FrugalStructList<ChildPropertyDependent> resourceDependents = parentTemplate.ResourceDependents;
			int count = resourceDependents.Count;
			for (int i = 0; i < count; i++)
			{
				if (resourceDependents[i].ChildIndex == childIndex && info.Contains(resourceDependents[i].Name, false))
				{
					DependencyProperty property = resourceDependents[i].Property;
					child.InvalidateProperty(property);
					int globalIndex = property.GlobalIndex;
					while (++i < resourceDependents.Count && resourceDependents[i].ChildIndex == childIndex && resourceDependents[i].Property.GlobalIndex == globalIndex)
					{
					}
					i--;
				}
			}
		}

		// Token: 0x06000932 RID: 2354 RVA: 0x0001FDBC File Offset: 0x0001DFBC
		internal static bool HasResourceDependentsForChild(int childIndex, ref FrugalStructList<ChildPropertyDependent> resourceDependents)
		{
			for (int i = 0; i < resourceDependents.Count; i++)
			{
				if (resourceDependents[i].ChildIndex == childIndex)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000933 RID: 2355 RVA: 0x0001FDEC File Offset: 0x0001DFEC
		internal static void InvalidatePropertiesOnTemplateNode(DependencyObject container, FrameworkObject child, int childIndex, ref FrugalStructList<ChildRecord> childRecordFromChildIndex, bool isDetach, FrameworkElementFactory templateRoot)
		{
			if (0 <= childIndex && childIndex < childRecordFromChildIndex.Count)
			{
				ChildRecord childRecord = childRecordFromChildIndex[childIndex];
				int count = childRecord.ValueLookupListFromProperty.Count;
				if (count > 0)
				{
					for (int i = 0; i < count; i++)
					{
						DependencyProperty property = childRecord.ValueLookupListFromProperty.Entries[i].Value.List[0].Property;
						if (!isDetach)
						{
							StyleHelper.ApplyTemplatedParentValue(container, child, childIndex, ref childRecordFromChildIndex, property, templateRoot);
						}
						else if (property != FrameworkElement.StyleProperty)
						{
							bool flag = true;
							if (property.IsPotentiallyInherited)
							{
								PropertyMetadata metadata = property.GetMetadata(child.DO.DependencyObjectType);
								if (metadata != null && metadata.IsInherited)
								{
									flag = false;
								}
							}
							if (flag)
							{
								child.DO.InvalidateProperty(property);
							}
						}
					}
				}
			}
		}

		// Token: 0x06000934 RID: 2356 RVA: 0x0001FEBC File Offset: 0x0001E0BC
		internal static bool IsSetOnContainer(DependencyProperty dp, ref FrugalStructList<ContainerDependent> containerDependents, bool alsoFromTriggers)
		{
			for (int i = 0; i < containerDependents.Count; i++)
			{
				if (dp == containerDependents[i].Property)
				{
					return alsoFromTriggers || !containerDependents[i].FromVisualTrigger;
				}
			}
			return false;
		}

		// Token: 0x06000935 RID: 2357 RVA: 0x0001FF00 File Offset: 0x0001E100
		internal static void OnTriggerSourcePropertyInvalidated(Style ownerStyle, FrameworkTemplate frameworkTemplate, DependencyObject container, DependencyProperty dp, DependencyPropertyChangedEventArgs changedArgs, bool invalidateOnlyContainer, ref FrugalStructList<ItemStructMap<TriggerSourceRecord>> triggerSourceRecordFromChildIndex, ref FrugalMap propertyTriggersWithActions, int sourceChildIndex)
		{
			if (0 <= sourceChildIndex && sourceChildIndex < triggerSourceRecordFromChildIndex.Count)
			{
				ItemStructMap<TriggerSourceRecord> itemStructMap = triggerSourceRecordFromChildIndex[sourceChildIndex];
				int num = itemStructMap.Search(dp.GlobalIndex);
				if (num >= 0)
				{
					TriggerSourceRecord value = itemStructMap.Entries[num].Value;
					StyleHelper.InvalidateDependents(ownerStyle, frameworkTemplate, container, dp, ref value.ChildPropertyDependents, invalidateOnlyContainer);
				}
			}
			object obj = propertyTriggersWithActions[dp.GlobalIndex];
			if (obj != DependencyProperty.UnsetValue)
			{
				TriggerBase triggerBase = obj as TriggerBase;
				if (triggerBase != null)
				{
					StyleHelper.InvokePropertyTriggerActions(triggerBase, container, dp, changedArgs, sourceChildIndex, ownerStyle, frameworkTemplate);
					return;
				}
				List<TriggerBase> list = (List<TriggerBase>)obj;
				for (int i = 0; i < list.Count; i++)
				{
					StyleHelper.InvokePropertyTriggerActions(list[i], container, dp, changedArgs, sourceChildIndex, ownerStyle, frameworkTemplate);
				}
			}
		}

		// Token: 0x06000936 RID: 2358 RVA: 0x0001FFC4 File Offset: 0x0001E1C4
		private static void InvalidateDependents(Style ownerStyle, FrameworkTemplate frameworkTemplate, DependencyObject container, DependencyProperty dp, ref FrugalStructList<ChildPropertyDependent> dependents, bool invalidateOnlyContainer)
		{
			for (int i = 0; i < dependents.Count; i++)
			{
				DependencyObject dependencyObject = null;
				int childIndex = dependents[i].ChildIndex;
				if (childIndex == 0)
				{
					dependencyObject = container;
				}
				else if (!invalidateOnlyContainer)
				{
					List<DependencyObject> value = StyleHelper.TemplatedFeChildrenField.GetValue(container);
					if (value != null && childIndex <= value.Count)
					{
						dependencyObject = StyleHelper.GetChild(value, childIndex);
					}
				}
				DependencyProperty property = dependents[i].Property;
				bool flag;
				if (dependencyObject != null && dependencyObject.GetValueSource(property, null, out flag) != BaseValueSourceInternal.Local)
				{
					dependencyObject.InvalidateProperty(property, true);
				}
			}
		}

		// Token: 0x06000937 RID: 2359 RVA: 0x0002004C File Offset: 0x0001E24C
		private static void InvokeDataTriggerActions(TriggerBase triggerBase, DependencyObject triggerContainer, BindingBase binding, BindingValueChangedEventArgs bindingChangedArgs, Style style, FrameworkTemplate frameworkTemplate, UncommonField<HybridDictionary[]> dataField)
		{
			DataTrigger dataTrigger = triggerBase as DataTrigger;
			bool oldState;
			bool newState;
			if (dataTrigger != null)
			{
				StyleHelper.EvaluateOldNewStates(dataTrigger, triggerContainer, binding, bindingChangedArgs, dataField, style, frameworkTemplate, out oldState, out newState);
			}
			else
			{
				StyleHelper.EvaluateOldNewStates((MultiDataTrigger)triggerBase, triggerContainer, binding, bindingChangedArgs, dataField, style, frameworkTemplate, out oldState, out newState);
			}
			StyleHelper.InvokeEnterOrExitActions(triggerBase, oldState, newState, triggerContainer, style, frameworkTemplate);
		}

		// Token: 0x06000938 RID: 2360 RVA: 0x000200A0 File Offset: 0x0001E2A0
		private static void InvokePropertyTriggerActions(TriggerBase triggerBase, DependencyObject triggerContainer, DependencyProperty changedProperty, DependencyPropertyChangedEventArgs changedArgs, int sourceChildIndex, Style style, FrameworkTemplate frameworkTemplate)
		{
			Trigger trigger = triggerBase as Trigger;
			bool oldState;
			bool newState;
			if (trigger != null)
			{
				StyleHelper.EvaluateOldNewStates(trigger, triggerContainer, changedProperty, changedArgs, sourceChildIndex, style, frameworkTemplate, out oldState, out newState);
			}
			else
			{
				StyleHelper.EvaluateOldNewStates((MultiTrigger)triggerBase, triggerContainer, changedProperty, changedArgs, sourceChildIndex, style, frameworkTemplate, out oldState, out newState);
			}
			StyleHelper.InvokeEnterOrExitActions(triggerBase, oldState, newState, triggerContainer, style, frameworkTemplate);
		}

		// Token: 0x06000939 RID: 2361 RVA: 0x000200F4 File Offset: 0x0001E2F4
		private static void ExecuteOnApplyEnterExitActions(FrameworkElement fe, FrameworkContentElement fce, Style style, UncommonField<HybridDictionary[]> dataField)
		{
			if (style == null)
			{
				return;
			}
			if (style.PropertyTriggersWithActions.Count == 0 && style.DataTriggersWithActions == null)
			{
				return;
			}
			TriggerCollection triggers = style.Triggers;
			DependencyObject triggerContainer = (fe != null) ? fe : fce;
			StyleHelper.ExecuteOnApplyEnterExitActionsLoop(triggerContainer, triggers, style, null, dataField);
		}

		// Token: 0x0600093A RID: 2362 RVA: 0x00020134 File Offset: 0x0001E334
		private static void ExecuteOnApplyEnterExitActions(FrameworkElement fe, FrameworkContentElement fce, FrameworkTemplate ft)
		{
			if (ft == null)
			{
				return;
			}
			if (ft != null && ft.PropertyTriggersWithActions.Count == 0 && ft.DataTriggersWithActions == null)
			{
				return;
			}
			TriggerCollection triggersInternal = ft.TriggersInternal;
			DependencyObject triggerContainer = (fe != null) ? fe : fce;
			StyleHelper.ExecuteOnApplyEnterExitActionsLoop(triggerContainer, triggersInternal, null, ft, StyleHelper.TemplateDataField);
		}

		// Token: 0x0600093B RID: 2363 RVA: 0x0002017C File Offset: 0x0001E37C
		private static void ExecuteOnApplyEnterExitActionsLoop(DependencyObject triggerContainer, TriggerCollection triggers, Style style, FrameworkTemplate ft, UncommonField<HybridDictionary[]> dataField)
		{
			for (int i = 0; i < triggers.Count; i++)
			{
				TriggerBase triggerBase = triggers[i];
				if ((triggerBase.HasEnterActions || triggerBase.HasExitActions) && (triggerBase.ExecuteEnterActionsOnApply || triggerBase.ExecuteExitActionsOnApply) && StyleHelper.NoSourceNameInTrigger(triggerBase))
				{
					bool currentState = triggerBase.GetCurrentState(triggerContainer, dataField);
					if (currentState && triggerBase.ExecuteEnterActionsOnApply)
					{
						StyleHelper.InvokeActions(triggerBase.EnterActions, triggerBase, triggerContainer, style, ft);
					}
					else if (!currentState && triggerBase.ExecuteExitActionsOnApply)
					{
						StyleHelper.InvokeActions(triggerBase.ExitActions, triggerBase, triggerContainer, style, ft);
					}
				}
			}
		}

		// Token: 0x0600093C RID: 2364 RVA: 0x0002020C File Offset: 0x0001E40C
		private static bool NoSourceNameInTrigger(TriggerBase triggerBase)
		{
			Trigger trigger = triggerBase as Trigger;
			if (trigger != null)
			{
				return trigger.SourceName == null;
			}
			MultiTrigger multiTrigger = triggerBase as MultiTrigger;
			if (multiTrigger != null)
			{
				for (int i = 0; i < multiTrigger.Conditions.Count; i++)
				{
					if (multiTrigger.Conditions[i].SourceName != null)
					{
						return false;
					}
				}
				return true;
			}
			return true;
		}

		// Token: 0x0600093D RID: 2365 RVA: 0x00020268 File Offset: 0x0001E468
		private static void InvokeEnterOrExitActions(TriggerBase triggerBase, bool oldState, bool newState, DependencyObject triggerContainer, Style style, FrameworkTemplate frameworkTemplate)
		{
			TriggerActionCollection actions;
			if (!oldState && newState)
			{
				actions = triggerBase.EnterActions;
			}
			else if (oldState && !newState)
			{
				actions = triggerBase.ExitActions;
			}
			else
			{
				actions = null;
			}
			StyleHelper.InvokeActions(actions, triggerBase, triggerContainer, style, frameworkTemplate);
		}

		// Token: 0x0600093E RID: 2366 RVA: 0x000202A3 File Offset: 0x0001E4A3
		private static void InvokeActions(TriggerActionCollection actions, TriggerBase triggerBase, DependencyObject triggerContainer, Style style, FrameworkTemplate frameworkTemplate)
		{
			if (actions != null)
			{
				if (StyleHelper.CanInvokeActionsNow(triggerContainer, frameworkTemplate))
				{
					StyleHelper.InvokeActions(triggerBase, triggerContainer, actions, style, frameworkTemplate);
					return;
				}
				StyleHelper.DeferActions(triggerBase, triggerContainer, actions, style, frameworkTemplate);
			}
		}

		// Token: 0x0600093F RID: 2367 RVA: 0x000202CC File Offset: 0x0001E4CC
		private static bool CanInvokeActionsNow(DependencyObject container, FrameworkTemplate frameworkTemplate)
		{
			bool result;
			if (frameworkTemplate != null)
			{
				FrameworkElement frameworkElement = (FrameworkElement)container;
				if (frameworkElement.HasTemplateGeneratedSubTree)
				{
					ContentPresenter contentPresenter = container as ContentPresenter;
					result = (contentPresenter == null || contentPresenter.TemplateIsCurrent);
				}
				else
				{
					result = false;
				}
			}
			else
			{
				result = true;
			}
			return result;
		}

		// Token: 0x06000940 RID: 2368 RVA: 0x0002030C File Offset: 0x0001E50C
		private static void DeferActions(TriggerBase triggerBase, DependencyObject triggerContainer, TriggerActionCollection actions, Style style, FrameworkTemplate frameworkTemplate)
		{
			DeferredAction item;
			item.TriggerBase = triggerBase;
			item.TriggerActionCollection = actions;
			ConditionalWeakTable<DependencyObject, List<DeferredAction>> conditionalWeakTable;
			if (frameworkTemplate != null)
			{
				conditionalWeakTable = frameworkTemplate.DeferredActions;
				if (conditionalWeakTable == null)
				{
					conditionalWeakTable = new ConditionalWeakTable<DependencyObject, List<DeferredAction>>();
					frameworkTemplate.DeferredActions = conditionalWeakTable;
				}
			}
			else
			{
				conditionalWeakTable = null;
			}
			if (conditionalWeakTable != null)
			{
				List<DeferredAction> list;
				if (!conditionalWeakTable.TryGetValue(triggerContainer, out list))
				{
					list = new List<DeferredAction>();
					conditionalWeakTable.Add(triggerContainer, list);
				}
				list.Add(item);
			}
		}

		// Token: 0x06000941 RID: 2369 RVA: 0x00020370 File Offset: 0x0001E570
		internal static void InvokeDeferredActions(DependencyObject triggerContainer, FrameworkTemplate frameworkTemplate)
		{
			List<DeferredAction> list;
			if (frameworkTemplate != null && frameworkTemplate.DeferredActions != null && frameworkTemplate.DeferredActions.TryGetValue(triggerContainer, out list))
			{
				for (int i = 0; i < list.Count; i++)
				{
					StyleHelper.InvokeActions(list[i].TriggerBase, triggerContainer, list[i].TriggerActionCollection, null, frameworkTemplate);
				}
				frameworkTemplate.DeferredActions.Remove(triggerContainer);
			}
		}

		// Token: 0x06000942 RID: 2370 RVA: 0x000203D8 File Offset: 0x0001E5D8
		internal static void InvokeActions(TriggerBase triggerBase, DependencyObject triggerContainer, TriggerActionCollection actions, Style style, FrameworkTemplate frameworkTemplate)
		{
			for (int i = 0; i < actions.Count; i++)
			{
				TriggerAction triggerAction = actions[i];
				triggerAction.Invoke(triggerContainer as FrameworkElement, triggerContainer as FrameworkContentElement, style, frameworkTemplate, triggerBase.Layer);
			}
		}

		// Token: 0x06000943 RID: 2371 RVA: 0x0002041C File Offset: 0x0001E61C
		private static void EvaluateOldNewStates(Trigger trigger, DependencyObject triggerContainer, DependencyProperty changedProperty, DependencyPropertyChangedEventArgs changedArgs, int sourceChildIndex, Style style, FrameworkTemplate frameworkTemplate, out bool oldState, out bool newState)
		{
			int num = 0;
			if (trigger.SourceName != null)
			{
				num = StyleHelper.QueryChildIndexFromChildName(trigger.SourceName, frameworkTemplate.ChildIndexFromChildName);
			}
			if (num == sourceChildIndex)
			{
				TriggerCondition[] triggerConditions = trigger.TriggerConditions;
				oldState = triggerConditions[0].Match(changedArgs.OldValue);
				newState = triggerConditions[0].Match(changedArgs.NewValue);
				return;
			}
			oldState = false;
			newState = false;
		}

		// Token: 0x06000944 RID: 2372 RVA: 0x00020488 File Offset: 0x0001E688
		private static void EvaluateOldNewStates(DataTrigger dataTrigger, DependencyObject triggerContainer, BindingBase binding, BindingValueChangedEventArgs bindingChangedArgs, UncommonField<HybridDictionary[]> dataField, Style style, FrameworkTemplate frameworkTemplate, out bool oldState, out bool newState)
		{
			TriggerCondition[] triggerConditions = dataTrigger.TriggerConditions;
			oldState = triggerConditions[0].ConvertAndMatch(bindingChangedArgs.OldValue);
			newState = triggerConditions[0].ConvertAndMatch(bindingChangedArgs.NewValue);
		}

		// Token: 0x06000945 RID: 2373 RVA: 0x000204C8 File Offset: 0x0001E6C8
		private static void EvaluateOldNewStates(MultiTrigger multiTrigger, DependencyObject triggerContainer, DependencyProperty changedProperty, DependencyPropertyChangedEventArgs changedArgs, int sourceChildIndex, Style style, FrameworkTemplate frameworkTemplate, out bool oldState, out bool newState)
		{
			TriggerCondition[] triggerConditions = multiTrigger.TriggerConditions;
			oldState = false;
			newState = false;
			for (int i = 0; i < triggerConditions.Length; i++)
			{
				int num;
				DependencyObject dependencyObject;
				if (triggerConditions[i].SourceChildIndex != 0)
				{
					num = triggerConditions[i].SourceChildIndex;
					dependencyObject = StyleHelper.GetChild(triggerContainer, num);
				}
				else
				{
					num = 0;
					dependencyObject = triggerContainer;
				}
				if (triggerConditions[i].Property == changedProperty && num == sourceChildIndex)
				{
					oldState = triggerConditions[i].Match(changedArgs.OldValue);
					newState = triggerConditions[i].Match(changedArgs.NewValue);
					if (oldState == newState)
					{
						return;
					}
				}
				else
				{
					object value = dependencyObject.GetValue(triggerConditions[i].Property);
					if (!triggerConditions[i].Match(value))
					{
						oldState = false;
						newState = false;
						return;
					}
				}
			}
		}

		// Token: 0x06000946 RID: 2374 RVA: 0x0002059C File Offset: 0x0001E79C
		private static void EvaluateOldNewStates(MultiDataTrigger multiDataTrigger, DependencyObject triggerContainer, BindingBase binding, BindingValueChangedEventArgs changedArgs, UncommonField<HybridDictionary[]> dataField, Style style, FrameworkTemplate frameworkTemplate, out bool oldState, out bool newState)
		{
			TriggerCondition[] triggerConditions = multiDataTrigger.TriggerConditions;
			oldState = false;
			newState = false;
			for (int i = 0; i < multiDataTrigger.Conditions.Count; i++)
			{
				BindingBase binding2 = triggerConditions[i].Binding;
				if (binding2 == binding)
				{
					oldState = triggerConditions[i].ConvertAndMatch(changedArgs.OldValue);
					newState = triggerConditions[i].ConvertAndMatch(changedArgs.NewValue);
					if (oldState == newState)
					{
						return;
					}
				}
				else
				{
					object dataTriggerValue = StyleHelper.GetDataTriggerValue(dataField, triggerContainer, binding2);
					if (!triggerConditions[i].ConvertAndMatch(dataTriggerValue))
					{
						oldState = false;
						newState = false;
						return;
					}
				}
			}
		}

		// Token: 0x06000947 RID: 2375 RVA: 0x00020638 File Offset: 0x0001E838
		internal static void AddPropertyTriggerWithAction(TriggerBase triggerBase, DependencyProperty property, ref FrugalMap triggersWithActions)
		{
			object obj = triggersWithActions[property.GlobalIndex];
			if (obj == DependencyProperty.UnsetValue)
			{
				triggersWithActions[property.GlobalIndex] = triggerBase;
			}
			else
			{
				TriggerBase triggerBase2 = obj as TriggerBase;
				if (triggerBase2 != null)
				{
					List<TriggerBase> list = new List<TriggerBase>();
					list.Add(triggerBase2);
					list.Add(triggerBase);
					triggersWithActions[property.GlobalIndex] = list;
				}
				else
				{
					List<TriggerBase> list2 = (List<TriggerBase>)obj;
					list2.Add(triggerBase);
				}
			}
			triggerBase.EstablishLayer();
		}

		// Token: 0x06000948 RID: 2376 RVA: 0x000206AC File Offset: 0x0001E8AC
		internal static void AddDataTriggerWithAction(TriggerBase triggerBase, BindingBase binding, ref HybridDictionary dataTriggersWithActions)
		{
			if (dataTriggersWithActions == null)
			{
				dataTriggersWithActions = new HybridDictionary();
			}
			object obj = dataTriggersWithActions[binding];
			if (obj == null)
			{
				dataTriggersWithActions[binding] = triggerBase;
			}
			else
			{
				TriggerBase triggerBase2 = obj as TriggerBase;
				if (triggerBase2 != null)
				{
					List<TriggerBase> list = new List<TriggerBase>();
					list.Add(triggerBase2);
					list.Add(triggerBase);
					dataTriggersWithActions[binding] = list;
				}
				else
				{
					List<TriggerBase> list2 = (List<TriggerBase>)obj;
					list2.Add(triggerBase);
				}
			}
			triggerBase.EstablishLayer();
		}

		// Token: 0x06000949 RID: 2377 RVA: 0x00020718 File Offset: 0x0001E918
		private static void OnBindingValueInStyleChanged(object sender, BindingValueChangedEventArgs e)
		{
			BindingExpressionBase bindingExpressionBase = (BindingExpressionBase)sender;
			BindingBase parentBindingBase = bindingExpressionBase.ParentBindingBase;
			DependencyObject targetElement = bindingExpressionBase.TargetElement;
			if (targetElement == null)
			{
				return;
			}
			FrameworkElement frameworkElement;
			FrameworkContentElement frameworkContentElement;
			Helper.DowncastToFEorFCE(targetElement, out frameworkElement, out frameworkContentElement, false);
			Style style = (frameworkElement != null) ? frameworkElement.Style : frameworkContentElement.Style;
			HybridDictionary dataTriggerRecordFromBinding = style._dataTriggerRecordFromBinding;
			if (dataTriggerRecordFromBinding != null && !bindingExpressionBase.IsAttaching)
			{
				DataTriggerRecord dataTriggerRecord = (DataTriggerRecord)dataTriggerRecordFromBinding[parentBindingBase];
				if (dataTriggerRecord != null)
				{
					StyleHelper.InvalidateDependents(style, null, targetElement, null, ref dataTriggerRecord.Dependents, false);
				}
			}
			StyleHelper.InvokeApplicableDataTriggerActions(style, null, targetElement, parentBindingBase, e, StyleHelper.StyleDataField);
		}

		// Token: 0x0600094A RID: 2378 RVA: 0x000207A8 File Offset: 0x0001E9A8
		private static void OnBindingValueInTemplateChanged(object sender, BindingValueChangedEventArgs e)
		{
			BindingExpressionBase bindingExpressionBase = (BindingExpressionBase)sender;
			BindingBase parentBindingBase = bindingExpressionBase.ParentBindingBase;
			DependencyObject targetElement = bindingExpressionBase.TargetElement;
			if (targetElement == null)
			{
				return;
			}
			FrameworkElement frameworkElement;
			FrameworkContentElement frameworkContentElement;
			Helper.DowncastToFEorFCE(targetElement, out frameworkElement, out frameworkContentElement, false);
			FrameworkTemplate templateInternal = frameworkElement.TemplateInternal;
			HybridDictionary hybridDictionary = null;
			if (templateInternal != null)
			{
				hybridDictionary = templateInternal._dataTriggerRecordFromBinding;
			}
			if (hybridDictionary != null && !bindingExpressionBase.IsAttaching)
			{
				DataTriggerRecord dataTriggerRecord = (DataTriggerRecord)hybridDictionary[parentBindingBase];
				if (dataTriggerRecord != null)
				{
					StyleHelper.InvalidateDependents(null, templateInternal, targetElement, null, ref dataTriggerRecord.Dependents, false);
				}
			}
			StyleHelper.InvokeApplicableDataTriggerActions(null, templateInternal, targetElement, parentBindingBase, e, StyleHelper.TemplateDataField);
		}

		// Token: 0x0600094B RID: 2379 RVA: 0x00020834 File Offset: 0x0001EA34
		private static void OnBindingValueInThemeStyleChanged(object sender, BindingValueChangedEventArgs e)
		{
			BindingExpressionBase bindingExpressionBase = (BindingExpressionBase)sender;
			BindingBase parentBindingBase = bindingExpressionBase.ParentBindingBase;
			DependencyObject targetElement = bindingExpressionBase.TargetElement;
			if (targetElement == null)
			{
				return;
			}
			FrameworkElement frameworkElement;
			FrameworkContentElement frameworkContentElement;
			Helper.DowncastToFEorFCE(targetElement, out frameworkElement, out frameworkContentElement, false);
			Style style = (frameworkElement != null) ? frameworkElement.ThemeStyle : frameworkContentElement.ThemeStyle;
			HybridDictionary dataTriggerRecordFromBinding = style._dataTriggerRecordFromBinding;
			if (dataTriggerRecordFromBinding != null && !bindingExpressionBase.IsAttaching)
			{
				DataTriggerRecord dataTriggerRecord = (DataTriggerRecord)dataTriggerRecordFromBinding[parentBindingBase];
				if (dataTriggerRecord != null)
				{
					StyleHelper.InvalidateDependents(style, null, targetElement, null, ref dataTriggerRecord.Dependents, false);
				}
			}
			StyleHelper.InvokeApplicableDataTriggerActions(style, null, targetElement, parentBindingBase, e, StyleHelper.ThemeStyleDataField);
		}

		// Token: 0x0600094C RID: 2380 RVA: 0x000208C4 File Offset: 0x0001EAC4
		private static void InvokeApplicableDataTriggerActions(Style style, FrameworkTemplate frameworkTemplate, DependencyObject container, BindingBase binding, BindingValueChangedEventArgs e, UncommonField<HybridDictionary[]> dataField)
		{
			HybridDictionary hybridDictionary;
			if (style != null)
			{
				hybridDictionary = style.DataTriggersWithActions;
			}
			else if (frameworkTemplate != null)
			{
				hybridDictionary = frameworkTemplate.DataTriggersWithActions;
			}
			else
			{
				hybridDictionary = null;
			}
			if (hybridDictionary != null)
			{
				object obj = hybridDictionary[binding];
				if (obj != null)
				{
					TriggerBase triggerBase = obj as TriggerBase;
					if (triggerBase != null)
					{
						StyleHelper.InvokeDataTriggerActions(triggerBase, container, binding, e, style, frameworkTemplate, dataField);
						return;
					}
					List<TriggerBase> list = (List<TriggerBase>)obj;
					for (int i = 0; i < list.Count; i++)
					{
						StyleHelper.InvokeDataTriggerActions(list[i], container, binding, e, style, frameworkTemplate, dataField);
					}
				}
			}
		}

		// Token: 0x0600094D RID: 2381 RVA: 0x00020944 File Offset: 0x0001EB44
		internal static int CreateChildIndexFromChildName(string childName, FrameworkTemplate frameworkTemplate)
		{
			HybridDictionary childIndexFromChildName = frameworkTemplate.ChildIndexFromChildName;
			int lastChildIndex = frameworkTemplate.LastChildIndex;
			if (childIndexFromChildName.Contains(childName))
			{
				throw new ArgumentException(SR.Get("NameScopeDuplicateNamesNotAllowed", new object[]
				{
					childName
				}));
			}
			if (lastChildIndex >= 65535)
			{
				throw new InvalidOperationException(SR.Get("StyleHasTooManyElements"));
			}
			object obj = lastChildIndex;
			childIndexFromChildName[childName] = obj;
			Interlocked.Increment(ref lastChildIndex);
			frameworkTemplate.LastChildIndex = lastChildIndex;
			return (int)obj;
		}

		// Token: 0x0600094E RID: 2382 RVA: 0x000209C0 File Offset: 0x0001EBC0
		internal static int QueryChildIndexFromChildName(string childName, HybridDictionary childIndexFromChildName)
		{
			if (childName == "~Self")
			{
				return 0;
			}
			object obj = childIndexFromChildName[childName];
			if (obj == null)
			{
				return -1;
			}
			return (int)obj;
		}

		// Token: 0x0600094F RID: 2383 RVA: 0x000209F0 File Offset: 0x0001EBF0
		internal static object FindNameInTemplateContent(DependencyObject container, string childName, FrameworkTemplate frameworkTemplate)
		{
			int num = StyleHelper.QueryChildIndexFromChildName(childName, frameworkTemplate.ChildIndexFromChildName);
			if (num != -1)
			{
				return StyleHelper.GetChild(container, num);
			}
			Hashtable value = StyleHelper.TemplatedNonFeChildrenField.GetValue(container);
			if (value != null)
			{
				return value[childName];
			}
			return null;
		}

		// Token: 0x06000950 RID: 2384 RVA: 0x00020A2E File Offset: 0x0001EC2E
		internal static DependencyObject GetChild(DependencyObject container, int childIndex)
		{
			return StyleHelper.GetChild(StyleHelper.TemplatedFeChildrenField.GetValue(container), childIndex);
		}

		// Token: 0x06000951 RID: 2385 RVA: 0x00020A44 File Offset: 0x0001EC44
		internal static DependencyObject GetChild(List<DependencyObject> styledChildren, int childIndex)
		{
			if (styledChildren == null || childIndex > styledChildren.Count)
			{
				return null;
			}
			if (childIndex < 0)
			{
				throw new ArgumentOutOfRangeException("childIndex");
			}
			return styledChildren[childIndex - 1];
		}

		// Token: 0x06000952 RID: 2386 RVA: 0x00020A79 File Offset: 0x0001EC79
		internal static void RegisterAlternateExpressionStorage()
		{
			DependencyObject.RegisterForAlternativeExpressionStorage(new AlternativeExpressionStorageCallback(StyleHelper.GetExpressionCore), out StyleHelper._getExpression);
		}

		// Token: 0x06000953 RID: 2387 RVA: 0x00020A94 File Offset: 0x0001EC94
		private static Expression GetExpressionCore(DependencyObject d, DependencyProperty dp, PropertyMetadata metadata)
		{
			FrameworkElement frameworkElement;
			FrameworkContentElement frameworkContentElement;
			Helper.DowncastToFEorFCE(d, out frameworkElement, out frameworkContentElement, false);
			if (frameworkElement != null)
			{
				return frameworkElement.GetExpressionCore(dp, metadata);
			}
			if (frameworkContentElement != null)
			{
				return frameworkContentElement.GetExpressionCore(dp, metadata);
			}
			return null;
		}

		// Token: 0x06000954 RID: 2388 RVA: 0x00020AC8 File Offset: 0x0001ECC8
		internal static Expression GetExpression(DependencyObject d, DependencyProperty dp)
		{
			FrameworkElement frameworkElement;
			FrameworkContentElement frameworkContentElement;
			Helper.DowncastToFEorFCE(d, out frameworkElement, out frameworkContentElement, false);
			bool flag = (frameworkElement != null) ? frameworkElement.IsInitialized : (frameworkContentElement == null || frameworkContentElement.IsInitialized);
			if (!flag)
			{
				if (frameworkElement != null)
				{
					frameworkElement.WriteInternalFlag(InternalFlags.IsInitialized, true);
				}
				else if (frameworkContentElement != null)
				{
					frameworkContentElement.WriteInternalFlag(InternalFlags.IsInitialized, true);
				}
			}
			Expression result = StyleHelper._getExpression(d, dp, dp.GetMetadata(d.DependencyObjectType));
			if (!flag)
			{
				if (frameworkElement != null)
				{
					frameworkElement.WriteInternalFlag(InternalFlags.IsInitialized, false);
				}
				else if (frameworkContentElement != null)
				{
					frameworkContentElement.WriteInternalFlag(InternalFlags.IsInitialized, false);
				}
			}
			return result;
		}

		// Token: 0x06000955 RID: 2389 RVA: 0x00020B58 File Offset: 0x0001ED58
		internal static RoutedEventHandlerInfo[] GetChildRoutedEventHandlers(int childIndex, RoutedEvent routedEvent, ref ItemStructList<ChildEventDependent> eventDependents)
		{
			if (childIndex > 0)
			{
				EventHandlersStore eventHandlersStore = null;
				for (int i = 0; i < eventDependents.Count; i++)
				{
					if (eventDependents.List[i].ChildIndex == childIndex)
					{
						eventHandlersStore = eventDependents.List[i].EventHandlersStore;
						break;
					}
				}
				if (eventHandlersStore != null)
				{
					return eventHandlersStore.GetRoutedEventHandlers(routedEvent);
				}
			}
			return null;
		}

		// Token: 0x06000956 RID: 2390 RVA: 0x00020BB0 File Offset: 0x0001EDB0
		internal static bool IsStylingLogicalTree(DependencyProperty dp, object value)
		{
			return dp != ItemsControl.ItemsPanelProperty && dp != FrameworkElement.ContextMenuProperty && dp != FrameworkElement.ToolTipProperty && (value is Visual || value is ContentElement);
		}

		// Token: 0x040007D9 RID: 2009
		internal static readonly UncommonField<HybridDictionary[]> StyleDataField = new UncommonField<HybridDictionary[]>();

		// Token: 0x040007DA RID: 2010
		internal static readonly UncommonField<HybridDictionary[]> TemplateDataField = new UncommonField<HybridDictionary[]>();

		// Token: 0x040007DB RID: 2011
		internal static readonly UncommonField<HybridDictionary> ParentTemplateValuesField = new UncommonField<HybridDictionary>();

		// Token: 0x040007DC RID: 2012
		internal static readonly UncommonField<HybridDictionary[]> ThemeStyleDataField = new UncommonField<HybridDictionary[]>();

		// Token: 0x040007DD RID: 2013
		internal static readonly UncommonField<List<DependencyObject>> TemplatedFeChildrenField = new UncommonField<List<DependencyObject>>();

		// Token: 0x040007DE RID: 2014
		internal static readonly UncommonField<Hashtable> TemplatedNonFeChildrenField = new UncommonField<Hashtable>();

		// Token: 0x040007DF RID: 2015
		internal const string SelfName = "~Self";

		// Token: 0x040007E0 RID: 2016
		internal static FrugalStructList<ContainerDependent> EmptyContainerDependents;

		// Token: 0x040007E1 RID: 2017
		internal static readonly object NotYetApplied = new NamedObject("NotYetApplied");

		// Token: 0x040007E2 RID: 2018
		private static AlternativeExpressionStorageCallback _getExpression;

		// Token: 0x040007E3 RID: 2019
		internal static RoutedEventHandler EventTriggerHandlerOnContainer = new RoutedEventHandler(StyleHelper.ExecuteEventTriggerActionsOnContainer);

		// Token: 0x040007E4 RID: 2020
		internal static RoutedEventHandler EventTriggerHandlerOnChild = new RoutedEventHandler(StyleHelper.ExecuteEventTriggerActionsOnChild);

		// Token: 0x040007E5 RID: 2021
		internal const int UnsharedTemplateContentPropertyIndex = -1;
	}
}
