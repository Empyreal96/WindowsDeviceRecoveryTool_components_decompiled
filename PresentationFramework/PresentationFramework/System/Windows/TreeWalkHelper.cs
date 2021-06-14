using System;
using System.Security;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using MS.Internal;
using MS.Utility;

namespace System.Windows
{
	// Token: 0x02000130 RID: 304
	internal static class TreeWalkHelper
	{
		// Token: 0x06000C61 RID: 3169 RVA: 0x0002E1F8 File Offset: 0x0002C3F8
		internal static void InvalidateOnTreeChange(FrameworkElement fe, FrameworkContentElement fce, DependencyObject parent, bool isAddOperation)
		{
			FrameworkObject frameworkObject = new FrameworkObject(parent);
			if (!frameworkObject.IsValid)
			{
				parent = frameworkObject.FrameworkParent.DO;
			}
			FrameworkObject frameworkObject2 = new FrameworkObject(fe, fce);
			if (isAddOperation)
			{
				frameworkObject2.SetShouldLookupImplicitStyles();
			}
			frameworkObject2.Reset(frameworkObject2.TemplatedParent);
			frameworkObject2.HasTemplateChanged = false;
			DependencyObject dependencyObject = (fe != null) ? fe : fce;
			if (fe != null)
			{
				if (fe.IsInitialized && !fe.HasLocalStyle)
				{
					fe.HasStyleChanged = false;
					fe.HasStyleInvalidated = false;
					fe.HasTemplateChanged = false;
					fe.AncestorChangeInProgress = true;
					fe.UpdateStyleProperty();
					fe.AncestorChangeInProgress = false;
				}
			}
			else if (!fce.HasLocalStyle)
			{
				fce.HasStyleChanged = false;
				fce.HasStyleInvalidated = false;
				fce.AncestorChangeInProgress = true;
				fce.UpdateStyleProperty();
				fce.AncestorChangeInProgress = false;
			}
			if (TreeWalkHelper.HasChildren(fe, fce))
			{
				FrameworkContextData frameworkContextData = FrameworkContextData.From(dependencyObject.Dispatcher);
				if (frameworkContextData.WasNodeVisited(dependencyObject, TreeWalkHelper.TreeChangeDelegate))
				{
					return;
				}
				TreeChangeInfo data = new TreeChangeInfo(dependencyObject, parent, isAddOperation);
				PrePostDescendentsWalker<TreeChangeInfo> prePostDescendentsWalker = new PrePostDescendentsWalker<TreeChangeInfo>(TreeWalkPriority.LogicalTree, TreeWalkHelper.TreeChangeDelegate, TreeWalkHelper.TreeChangePostDelegate, data);
				frameworkContextData.AddWalker(TreeWalkHelper.TreeChangeDelegate, prePostDescendentsWalker);
				try
				{
					prePostDescendentsWalker.StartWalk(dependencyObject);
					return;
				}
				finally
				{
					frameworkContextData.RemoveWalker(TreeWalkHelper.TreeChangeDelegate, prePostDescendentsWalker);
				}
			}
			TreeChangeInfo info = new TreeChangeInfo(dependencyObject, parent, isAddOperation);
			TreeWalkHelper.OnAncestorChanged(fe, fce, info);
			bool visitedViaVisualTree = false;
			TreeWalkHelper.OnPostAncestorChanged(dependencyObject, info, visitedViaVisualTree);
		}

		// Token: 0x06000C62 RID: 3170 RVA: 0x0002E35C File Offset: 0x0002C55C
		private static bool OnAncestorChanged(DependencyObject d, TreeChangeInfo info, bool visitedViaVisualTree)
		{
			FrameworkObject frameworkObject = new FrameworkObject(d, true);
			TreeWalkHelper.OnAncestorChanged(frameworkObject.FE, frameworkObject.FCE, info);
			return true;
		}

		// Token: 0x06000C63 RID: 3171 RVA: 0x0002E387 File Offset: 0x0002C587
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private static void OnAncestorChanged(FrameworkElement fe, FrameworkContentElement fce, TreeChangeInfo info)
		{
			if (fe != null)
			{
				fe.OnAncestorChangedInternal(info);
				return;
			}
			fce.OnAncestorChangedInternal(info);
		}

		// Token: 0x06000C64 RID: 3172 RVA: 0x0002E39B File Offset: 0x0002C59B
		private static bool OnPostAncestorChanged(DependencyObject d, TreeChangeInfo info, bool visitedViaVisualTree)
		{
			if (info.TopmostCollapsedParentNode == d)
			{
				info.TopmostCollapsedParentNode = null;
			}
			info.InheritablePropertiesStack.Pop();
			return true;
		}

		// Token: 0x06000C65 RID: 3173 RVA: 0x0002E3C0 File Offset: 0x0002C5C0
		internal static FrugalObjectList<DependencyProperty> InvalidateTreeDependentProperties(TreeChangeInfo info, FrameworkElement fe, FrameworkContentElement fce, Style selfStyle, Style selfThemeStyle, ref ChildRecord childRecord, bool isChildRecordValid, bool hasStyleChanged, bool isSelfInheritanceParent, bool wasSelfInheritanceParent)
		{
			DependencyObject dependencyObject = (fe != null) ? fe : fce;
			FrameworkObject frameworkObject = new FrameworkObject(fe, fce);
			FrugalObjectList<DependencyProperty> frugalObjectList = info.InheritablePropertiesStack.Peek();
			int num = (frugalObjectList != null) ? frugalObjectList.Count : 0;
			FrugalObjectList<DependencyProperty> frugalObjectList2 = null;
			if (TreeWalkHelper.HasChildren(fe, fce))
			{
				frugalObjectList2 = new FrugalObjectList<DependencyProperty>(num);
			}
			info.ResetInheritableValueIndexer();
			for (int i = 0; i < num; i++)
			{
				DependencyProperty dependencyProperty = frugalObjectList[i];
				PropertyMetadata metadata = dependencyProperty.GetMetadata(dependencyObject);
				if (metadata.IsInherited)
				{
					FrameworkPropertyMetadata frameworkPropertyMetadata = (FrameworkPropertyMetadata)metadata;
					bool flag = TreeWalkHelper.InvalidateTreeDependentProperty(info, dependencyObject, ref frameworkObject, dependencyProperty, frameworkPropertyMetadata, selfStyle, selfThemeStyle, ref childRecord, isChildRecordValid, hasStyleChanged, isSelfInheritanceParent, wasSelfInheritanceParent);
					if (flag && frugalObjectList2 != null && (!TreeWalkHelper.SkipNow(frameworkObject.InheritanceBehavior) || frameworkPropertyMetadata.OverridesInheritanceBehavior))
					{
						frugalObjectList2.Add(dependencyProperty);
					}
				}
			}
			return frugalObjectList2;
		}

		// Token: 0x06000C66 RID: 3174 RVA: 0x0002E490 File Offset: 0x0002C690
		private static bool InvalidateTreeDependentProperty(TreeChangeInfo info, DependencyObject d, ref FrameworkObject fo, DependencyProperty dp, FrameworkPropertyMetadata fMetadata, Style selfStyle, Style selfThemeStyle, ref ChildRecord childRecord, bool isChildRecordValid, bool hasStyleChanged, bool isSelfInheritanceParent, bool wasSelfInheritanceParent)
		{
			if (!TreeWalkHelper.SkipNext(fo.InheritanceBehavior) || fMetadata.OverridesInheritanceBehavior)
			{
				InheritablePropertyChangeInfo rootInheritableValue = info.GetRootInheritableValue(dp);
				EffectiveValueEntry oldEntry = rootInheritableValue.OldEntry;
				EffectiveValueEntry effectiveValueEntry = info.IsAddOperation ? rootInheritableValue.NewEntry : new EffectiveValueEntry(dp, BaseValueSourceInternal.Inherited);
				bool flag = TreeWalkHelper.IsForceInheritedProperty(dp);
				if (d != info.Root)
				{
					if (wasSelfInheritanceParent)
					{
						oldEntry = d.GetValueEntry(d.LookupEntry(dp.GlobalIndex), dp, fMetadata, RequestFlags.DeferredReferences);
					}
					else if (isSelfInheritanceParent)
					{
						EffectiveValueEntry valueEntry = d.GetValueEntry(d.LookupEntry(dp.GlobalIndex), dp, fMetadata, RequestFlags.DeferredReferences);
						if (valueEntry.BaseValueSourceInternal <= BaseValueSourceInternal.Inherited)
						{
							oldEntry = oldEntry.GetFlattenedEntry(RequestFlags.FullyResolved);
							oldEntry.BaseValueSourceInternal = BaseValueSourceInternal.Inherited;
						}
						else
						{
							oldEntry = valueEntry;
						}
					}
					else
					{
						oldEntry = oldEntry.GetFlattenedEntry(RequestFlags.FullyResolved);
						oldEntry.BaseValueSourceInternal = BaseValueSourceInternal.Inherited;
					}
				}
				else if (info.IsAddOperation && (flag || oldEntry.BaseValueSourceInternal <= BaseValueSourceInternal.Inherited))
				{
					EffectiveValueEntry valueEntry2 = d.GetValueEntry(d.LookupEntry(dp.GlobalIndex), dp, fMetadata, RequestFlags.DeferredReferences);
					if (valueEntry2.BaseValueSourceInternal > BaseValueSourceInternal.Inherited)
					{
						oldEntry = valueEntry2;
					}
				}
				OperationType operationType = info.IsAddOperation ? OperationType.AddChild : OperationType.RemoveChild;
				if (BaseValueSourceInternal.Inherited >= oldEntry.BaseValueSourceInternal)
				{
					return (d.UpdateEffectiveValue(d.LookupEntry(dp.GlobalIndex), dp, fMetadata, oldEntry, ref effectiveValueEntry, false, false, operationType) & (UpdateResult)5) == UpdateResult.ValueChanged;
				}
				if (flag)
				{
					effectiveValueEntry = new EffectiveValueEntry(dp, FullValueSource.IsCoerced);
					return (d.UpdateEffectiveValue(d.LookupEntry(dp.GlobalIndex), dp, fMetadata, oldEntry, ref effectiveValueEntry, false, false, operationType) & (UpdateResult)5) == UpdateResult.ValueChanged;
				}
			}
			return false;
		}

		// Token: 0x06000C67 RID: 3175 RVA: 0x0002E60C File Offset: 0x0002C80C
		internal static void InvalidateOnResourcesChange(FrameworkElement fe, FrameworkContentElement fce, ResourcesChangeInfo info)
		{
			FrameworkObject frameworkObject = new FrameworkObject(fe, fce);
			frameworkObject.Reset(frameworkObject.TemplatedParent);
			frameworkObject.HasTemplateChanged = false;
			DependencyObject dependencyObject = (fe != null) ? fe : fce;
			if (TreeWalkHelper.HasChildren(fe, fce))
			{
				DescendentsWalker<ResourcesChangeInfo> descendentsWalker = new DescendentsWalker<ResourcesChangeInfo>(TreeWalkPriority.LogicalTree, TreeWalkHelper.ResourcesChangeDelegate, info);
				descendentsWalker.StartWalk(dependencyObject);
				return;
			}
			TreeWalkHelper.OnResourcesChanged(dependencyObject, info, true);
		}

		// Token: 0x06000C68 RID: 3176 RVA: 0x0002E666 File Offset: 0x0002C866
		private static bool OnResourcesChangedCallback(DependencyObject d, ResourcesChangeInfo info, bool visitedViaVisualTree)
		{
			TreeWalkHelper.OnResourcesChanged(d, info, true);
			return true;
		}

		// Token: 0x06000C69 RID: 3177 RVA: 0x0002E674 File Offset: 0x0002C874
		internal static void OnResourcesChanged(DependencyObject d, ResourcesChangeInfo info, bool raiseResourceChangedEvent)
		{
			bool flag = info.Contains(d.DependencyObjectType.SystemType, true);
			bool isThemeChange = info.IsThemeChange;
			bool isStyleResourcesChange = info.IsStyleResourcesChange;
			bool isTemplateResourcesChange = info.IsTemplateResourcesChange;
			bool flag2 = info.Container == d;
			FrameworkObject frameworkObject = new FrameworkObject(d);
			if (info.IsResourceAddOperation || info.IsCatastrophicDictionaryChange)
			{
				frameworkObject.SetShouldLookupImplicitStyles();
			}
			if (frameworkObject.IsFE)
			{
				FrameworkElement fe = frameworkObject.FE;
				fe.HasStyleChanged = false;
				fe.HasStyleInvalidated = false;
				fe.HasTemplateChanged = false;
				if (info.IsImplicitDataTemplateChange)
				{
					ContentPresenter contentPresenter = fe as ContentPresenter;
					if (contentPresenter != null)
					{
						contentPresenter.ReevaluateTemplate();
					}
				}
				if (fe.HasResourceReference)
				{
					TreeWalkHelper.InvalidateResourceReferences(fe, info);
					if ((!isStyleResourcesChange && !isTemplateResourcesChange) || !flag2)
					{
						TreeWalkHelper.InvalidateStyleAndReferences(d, info, flag);
					}
				}
				else if (flag && (fe.HasImplicitStyleFromResources || fe.Style == FrameworkElement.StyleProperty.GetMetadata(fe.DependencyObjectType).DefaultValue) && (!isStyleResourcesChange || !flag2))
				{
					fe.UpdateStyleProperty();
				}
				if (isThemeChange)
				{
					fe.UpdateThemeStyleProperty();
				}
				if (raiseResourceChangedEvent && fe.PotentiallyHasMentees)
				{
					fe.RaiseClrEvent(FrameworkElement.ResourcesChangedKey, new ResourcesChangedEventArgs(info));
					return;
				}
			}
			else
			{
				FrameworkContentElement fce = frameworkObject.FCE;
				fce.HasStyleChanged = false;
				fce.HasStyleInvalidated = false;
				if (fce.HasResourceReference)
				{
					TreeWalkHelper.InvalidateResourceReferences(fce, info);
					if ((!isStyleResourcesChange && !isTemplateResourcesChange) || !flag2)
					{
						TreeWalkHelper.InvalidateStyleAndReferences(d, info, flag);
					}
				}
				else if (flag && (fce.HasImplicitStyleFromResources || fce.Style == FrameworkContentElement.StyleProperty.GetMetadata(fce.DependencyObjectType).DefaultValue) && (!isStyleResourcesChange || !flag2))
				{
					fce.UpdateStyleProperty();
				}
				if (isThemeChange)
				{
					fce.UpdateThemeStyleProperty();
				}
				if (raiseResourceChangedEvent && fce.PotentiallyHasMentees)
				{
					fce.RaiseClrEvent(FrameworkElement.ResourcesChangedKey, new ResourcesChangedEventArgs(info));
				}
			}
		}

		// Token: 0x06000C6A RID: 3178 RVA: 0x0002E850 File Offset: 0x0002CA50
		private static void InvalidateResourceReferences(DependencyObject d, ResourcesChangeInfo info)
		{
			LocalValueEnumerator localValueEnumerator = d.GetLocalValueEnumerator();
			int count = localValueEnumerator.Count;
			if (count > 0)
			{
				ResourceReferenceExpression[] array = new ResourceReferenceExpression[count];
				int num = 0;
				while (localValueEnumerator.MoveNext())
				{
					LocalValueEntry localValueEntry = localValueEnumerator.Current;
					ResourceReferenceExpression resourceReferenceExpression = localValueEntry.Value as ResourceReferenceExpression;
					if (resourceReferenceExpression != null && info.Contains(resourceReferenceExpression.ResourceKey, false))
					{
						array[num] = resourceReferenceExpression;
						num++;
					}
				}
				ResourcesChangedEventArgs e = new ResourcesChangedEventArgs(info);
				for (int i = 0; i < num; i++)
				{
					array[i].InvalidateExpressionValue(d, e);
				}
			}
		}

		// Token: 0x06000C6B RID: 3179 RVA: 0x0002E8E0 File Offset: 0x0002CAE0
		private static void InvalidateStyleAndReferences(DependencyObject d, ResourcesChangeInfo info, bool containsTypeOfKey)
		{
			FrameworkObject frameworkObject = new FrameworkObject(d);
			if (frameworkObject.IsFE)
			{
				FrameworkElement fe = frameworkObject.FE;
				if (containsTypeOfKey && !info.IsThemeChange && (fe.HasImplicitStyleFromResources || fe.Style == FrameworkElement.StyleProperty.GetMetadata(fe.DependencyObjectType).DefaultValue))
				{
					fe.UpdateStyleProperty();
				}
				if (fe.Style != null && fe.Style.HasResourceReferences && !fe.HasStyleChanged)
				{
					StyleHelper.InvalidateResourceDependents(d, info, ref fe.Style.ResourceDependents, false);
				}
				if (fe.TemplateInternal != null && fe.TemplateInternal.HasContainerResourceReferences)
				{
					StyleHelper.InvalidateResourceDependents(d, info, ref fe.TemplateInternal.ResourceDependents, false);
				}
				if (fe.TemplateChildIndex > 0)
				{
					FrameworkElement frameworkElement = (FrameworkElement)fe.TemplatedParent;
					FrameworkTemplate templateInternal = frameworkElement.TemplateInternal;
					if (!frameworkElement.HasTemplateChanged && templateInternal.HasChildResourceReferences)
					{
						StyleHelper.InvalidateResourceDependentsForChild(frameworkElement, fe, fe.TemplateChildIndex, info, templateInternal);
					}
				}
				if (!info.IsThemeChange)
				{
					Style themeStyle = fe.ThemeStyle;
					if (themeStyle != null && themeStyle.HasResourceReferences && themeStyle != fe.Style)
					{
						StyleHelper.InvalidateResourceDependents(d, info, ref themeStyle.ResourceDependents, false);
						return;
					}
				}
			}
			else if (frameworkObject.IsFCE)
			{
				FrameworkContentElement fce = frameworkObject.FCE;
				if (containsTypeOfKey && !info.IsThemeChange && (fce.HasImplicitStyleFromResources || fce.Style == FrameworkContentElement.StyleProperty.GetMetadata(fce.DependencyObjectType).DefaultValue))
				{
					fce.UpdateStyleProperty();
				}
				if (fce.Style != null && fce.Style.HasResourceReferences && !fce.HasStyleChanged)
				{
					StyleHelper.InvalidateResourceDependents(d, info, ref fce.Style.ResourceDependents, true);
				}
				if (fce.TemplateChildIndex > 0)
				{
					FrameworkElement frameworkElement2 = (FrameworkElement)fce.TemplatedParent;
					FrameworkTemplate templateInternal2 = frameworkElement2.TemplateInternal;
					if (!frameworkElement2.HasTemplateChanged && templateInternal2.HasChildResourceReferences)
					{
						StyleHelper.InvalidateResourceDependentsForChild(frameworkElement2, fce, fce.TemplateChildIndex, info, templateInternal2);
					}
				}
				if (!info.IsThemeChange)
				{
					Style themeStyle2 = fce.ThemeStyle;
					if (themeStyle2 != null && themeStyle2.HasResourceReferences && themeStyle2 != fce.Style)
					{
						StyleHelper.InvalidateResourceDependents(d, info, ref themeStyle2.ResourceDependents, false);
					}
				}
			}
		}

		// Token: 0x06000C6C RID: 3180 RVA: 0x0002EB1C File Offset: 0x0002CD1C
		internal static void InvalidateOnInheritablePropertyChange(FrameworkElement fe, FrameworkContentElement fce, InheritablePropertyChangeInfo info, bool skipStartNode)
		{
			DependencyProperty property = info.Property;
			FrameworkObject frameworkObject = new FrameworkObject(fe, fce);
			if (TreeWalkHelper.HasChildren(fe, fce))
			{
				DependencyObject @do = frameworkObject.DO;
				DescendentsWalker<InheritablePropertyChangeInfo> descendentsWalker = new DescendentsWalker<InheritablePropertyChangeInfo>(TreeWalkPriority.LogicalTree, TreeWalkHelper.InheritablePropertyChangeDelegate, info);
				descendentsWalker.StartWalk(@do, skipStartNode);
				return;
			}
			if (!skipStartNode)
			{
				bool visitedViaVisualTree = false;
				TreeWalkHelper.OnInheritablePropertyChanged(frameworkObject.DO, info, visitedViaVisualTree);
			}
		}

		// Token: 0x06000C6D RID: 3181 RVA: 0x0002EB78 File Offset: 0x0002CD78
		private static bool OnInheritablePropertyChanged(DependencyObject d, InheritablePropertyChangeInfo info, bool visitedViaVisualTree)
		{
			DependencyProperty property = info.Property;
			EffectiveValueEntry oldEntry = info.OldEntry;
			EffectiveValueEntry newEntry = info.NewEntry;
			InheritanceBehavior inheritanceBehavior;
			bool flag = TreeWalkHelper.IsInheritanceNode(d, property, out inheritanceBehavior);
			bool flag2 = TreeWalkHelper.IsForceInheritedProperty(property);
			if (!flag || (TreeWalkHelper.SkipNext(inheritanceBehavior) && !flag2))
			{
				return inheritanceBehavior == InheritanceBehavior.Default || flag2;
			}
			PropertyMetadata metadata = property.GetMetadata(d);
			EntryIndex entryIndex = d.LookupEntry(property.GlobalIndex);
			if (!d.IsSelfInheritanceParent)
			{
				DependencyObject frameworkParent = FrameworkElement.GetFrameworkParent(d);
				InheritanceBehavior inheritanceBehavior2 = InheritanceBehavior.Default;
				if (frameworkParent != null)
				{
					FrameworkObject frameworkObject = new FrameworkObject(frameworkParent, true);
					inheritanceBehavior2 = frameworkObject.InheritanceBehavior;
				}
				if (!TreeWalkHelper.SkipNext(inheritanceBehavior) && !TreeWalkHelper.SkipNow(inheritanceBehavior2))
				{
					d.SynchronizeInheritanceParent(frameworkParent);
				}
				if (oldEntry.BaseValueSourceInternal == BaseValueSourceInternal.Unknown)
				{
					oldEntry = EffectiveValueEntry.CreateDefaultValueEntry(property, metadata.GetDefaultValue(d, property));
				}
			}
			else
			{
				oldEntry = d.GetValueEntry(entryIndex, property, metadata, RequestFlags.RawEntry);
			}
			if (BaseValueSourceInternal.Inherited >= oldEntry.BaseValueSourceInternal)
			{
				if (visitedViaVisualTree && FrameworkElement.DType.IsInstanceOfType(d))
				{
					DependencyObject parent = LogicalTreeHelper.GetParent(d);
					if (parent != null)
					{
						DependencyObject parent2 = VisualTreeHelper.GetParent(d);
						if (parent2 != null && parent2 != parent)
						{
							return false;
						}
					}
				}
				return (d.UpdateEffectiveValue(entryIndex, property, metadata, oldEntry, ref newEntry, false, false, OperationType.Inherit) & (UpdateResult)5) == UpdateResult.ValueChanged;
			}
			if (flag2)
			{
				newEntry = new EffectiveValueEntry(property, FullValueSource.IsCoerced);
				return (d.UpdateEffectiveValue(d.LookupEntry(property.GlobalIndex), property, metadata, oldEntry, ref newEntry, false, false, OperationType.Inherit) & (UpdateResult)5) == UpdateResult.ValueChanged;
			}
			return false;
		}

		// Token: 0x06000C6E RID: 3182 RVA: 0x0002ECD4 File Offset: 0x0002CED4
		internal static void OnInheritedPropertyChanged(DependencyObject d, ref InheritablePropertyChangeInfo info, InheritanceBehavior inheritanceBehavior)
		{
			if (inheritanceBehavior == InheritanceBehavior.Default || TreeWalkHelper.IsForceInheritedProperty(info.Property))
			{
				FrameworkObject frameworkObject = new FrameworkObject(d);
				frameworkObject.OnInheritedPropertyChanged(ref info);
			}
		}

		// Token: 0x06000C6F RID: 3183 RVA: 0x0002ED04 File Offset: 0x0002CF04
		internal static bool IsInheritanceNode(DependencyObject d, DependencyProperty dp, out InheritanceBehavior inheritanceBehavior)
		{
			inheritanceBehavior = InheritanceBehavior.Default;
			FrameworkPropertyMetadata frameworkPropertyMetadata = dp.GetMetadata(d.DependencyObjectType) as FrameworkPropertyMetadata;
			if (frameworkPropertyMetadata != null)
			{
				FrameworkObject frameworkObject = new FrameworkObject(d);
				if (!frameworkObject.IsValid)
				{
					return false;
				}
				if (frameworkObject.InheritanceBehavior != InheritanceBehavior.Default && !frameworkPropertyMetadata.OverridesInheritanceBehavior)
				{
					inheritanceBehavior = frameworkObject.InheritanceBehavior;
				}
				if (frameworkPropertyMetadata.Inherits)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000C70 RID: 3184 RVA: 0x0002ED64 File Offset: 0x0002CF64
		internal static bool IsInheritanceNode(FrameworkElement fe, DependencyProperty dp, out InheritanceBehavior inheritanceBehavior)
		{
			inheritanceBehavior = InheritanceBehavior.Default;
			FrameworkPropertyMetadata frameworkPropertyMetadata = dp.GetMetadata(fe.DependencyObjectType) as FrameworkPropertyMetadata;
			if (frameworkPropertyMetadata != null)
			{
				if (fe.InheritanceBehavior != InheritanceBehavior.Default && !frameworkPropertyMetadata.OverridesInheritanceBehavior)
				{
					inheritanceBehavior = fe.InheritanceBehavior;
				}
				return frameworkPropertyMetadata.Inherits;
			}
			return false;
		}

		// Token: 0x06000C71 RID: 3185 RVA: 0x0002EDAC File Offset: 0x0002CFAC
		internal static bool IsInheritanceNode(FrameworkContentElement fce, DependencyProperty dp, out InheritanceBehavior inheritanceBehavior)
		{
			inheritanceBehavior = InheritanceBehavior.Default;
			FrameworkPropertyMetadata frameworkPropertyMetadata = dp.GetMetadata(fce.DependencyObjectType) as FrameworkPropertyMetadata;
			if (frameworkPropertyMetadata != null)
			{
				if (fce.InheritanceBehavior != InheritanceBehavior.Default && !frameworkPropertyMetadata.OverridesInheritanceBehavior)
				{
					inheritanceBehavior = fce.InheritanceBehavior;
				}
				return frameworkPropertyMetadata.Inherits;
			}
			return false;
		}

		// Token: 0x06000C72 RID: 3186 RVA: 0x0002EDF1 File Offset: 0x0002CFF1
		internal static bool SkipNow(InheritanceBehavior inheritanceBehavior)
		{
			return inheritanceBehavior == InheritanceBehavior.SkipToAppNow || inheritanceBehavior == InheritanceBehavior.SkipToThemeNow || inheritanceBehavior == InheritanceBehavior.SkipAllNow;
		}

		// Token: 0x06000C73 RID: 3187 RVA: 0x0002EE02 File Offset: 0x0002D002
		internal static bool SkipNext(InheritanceBehavior inheritanceBehavior)
		{
			return inheritanceBehavior == InheritanceBehavior.SkipToAppNext || inheritanceBehavior == InheritanceBehavior.SkipToThemeNext || inheritanceBehavior == InheritanceBehavior.SkipAllNext;
		}

		// Token: 0x06000C74 RID: 3188 RVA: 0x0002EE13 File Offset: 0x0002D013
		internal static bool HasChildren(FrameworkElement fe, FrameworkContentElement fce)
		{
			return (fe != null && (fe.HasLogicalChildren || fe.HasVisualChildren || Popup.RegisteredPopupsField.GetValue(fe) != null)) || (fce != null && fce.HasLogicalChildren);
		}

		// Token: 0x06000C75 RID: 3189 RVA: 0x0002EE42 File Offset: 0x0002D042
		private static bool IsForceInheritedProperty(DependencyProperty dp)
		{
			return dp == FrameworkElement.FlowDirectionProperty;
		}

		// Token: 0x04000B07 RID: 2823
		private static VisitedCallback<TreeChangeInfo> TreeChangeDelegate = new VisitedCallback<TreeChangeInfo>(TreeWalkHelper.OnAncestorChanged);

		// Token: 0x04000B08 RID: 2824
		private static VisitedCallback<TreeChangeInfo> TreeChangePostDelegate = new VisitedCallback<TreeChangeInfo>(TreeWalkHelper.OnPostAncestorChanged);

		// Token: 0x04000B09 RID: 2825
		private static VisitedCallback<ResourcesChangeInfo> ResourcesChangeDelegate = new VisitedCallback<ResourcesChangeInfo>(TreeWalkHelper.OnResourcesChangedCallback);

		// Token: 0x04000B0A RID: 2826
		private static VisitedCallback<InheritablePropertyChangeInfo> InheritablePropertyChangeDelegate = new VisitedCallback<InheritablePropertyChangeInfo>(TreeWalkHelper.OnInheritablePropertyChanged);
	}
}
