using System;
using System.Collections.Generic;
using MS.Internal;
using MS.Utility;

namespace System.Windows
{
	// Token: 0x0200012F RID: 303
	internal struct TreeChangeInfo
	{
		// Token: 0x06000C58 RID: 3160 RVA: 0x0002DF9B File Offset: 0x0002C19B
		public TreeChangeInfo(DependencyObject root, DependencyObject parent, bool isAddOperation)
		{
			this._rootOfChange = root;
			this._isAddOperation = isAddOperation;
			this._topmostCollapsedParentNode = null;
			this._rootInheritableValues = null;
			this._inheritablePropertiesStack = null;
			this._valueIndexer = 0;
			this.InheritablePropertiesStack.Push(this.CreateParentInheritableProperties(root, parent, isAddOperation));
		}

		// Token: 0x06000C59 RID: 3161 RVA: 0x0002DFDC File Offset: 0x0002C1DC
		internal FrugalObjectList<DependencyProperty> CreateParentInheritableProperties(DependencyObject d, DependencyObject parent, bool isAddOperation)
		{
			if (parent == null)
			{
				return new FrugalObjectList<DependencyProperty>(0);
			}
			DependencyObjectType dependencyObjectType = d.DependencyObjectType;
			EffectiveValueEntry[] array = null;
			uint num = 0U;
			uint num2 = 0U;
			if (!parent.IsSelfInheritanceParent)
			{
				DependencyObject inheritanceParent = parent.InheritanceParent;
				if (inheritanceParent != null)
				{
					array = inheritanceParent.EffectiveValues;
					num = inheritanceParent.EffectiveValuesCount;
					num2 = inheritanceParent.InheritableEffectiveValuesCount;
				}
			}
			else
			{
				array = parent.EffectiveValues;
				num = parent.EffectiveValuesCount;
				num2 = parent.InheritableEffectiveValuesCount;
			}
			FrugalObjectList<DependencyProperty> frugalObjectList = new FrugalObjectList<DependencyProperty>((int)num2);
			if (num2 == 0U)
			{
				return frugalObjectList;
			}
			this._rootInheritableValues = new InheritablePropertyChangeInfo[num2];
			int num3 = 0;
			FrameworkObject frameworkObject = new FrameworkObject(parent);
			for (uint num4 = 0U; num4 < num; num4 += 1U)
			{
				EffectiveValueEntry effectiveValueEntry = array[(int)num4];
				DependencyProperty dependencyProperty = DependencyProperty.RegisteredPropertyList.List[effectiveValueEntry.PropertyIndex];
				if (dependencyProperty != null && dependencyProperty.IsPotentiallyInherited)
				{
					PropertyMetadata metadata = dependencyProperty.GetMetadata(parent.DependencyObjectType);
					if (metadata != null && metadata.IsInherited)
					{
						FrameworkPropertyMetadata frameworkPropertyMetadata = (FrameworkPropertyMetadata)metadata;
						if (!TreeWalkHelper.SkipNow(frameworkObject.InheritanceBehavior) || frameworkPropertyMetadata.OverridesInheritanceBehavior)
						{
							frugalObjectList.Add(dependencyProperty);
							EffectiveValueEntry valueEntry = d.GetValueEntry(d.LookupEntry(dependencyProperty.GlobalIndex), dependencyProperty, dependencyProperty.GetMetadata(dependencyObjectType), RequestFlags.DeferredReferences);
							EffectiveValueEntry newEntry;
							if (isAddOperation)
							{
								newEntry = effectiveValueEntry;
								if (newEntry.BaseValueSourceInternal != BaseValueSourceInternal.Default || newEntry.HasModifiers)
								{
									newEntry = newEntry.GetFlattenedEntry(RequestFlags.FullyResolved);
									newEntry.BaseValueSourceInternal = BaseValueSourceInternal.Inherited;
								}
							}
							else
							{
								newEntry = default(EffectiveValueEntry);
							}
							this._rootInheritableValues[num3++] = new InheritablePropertyChangeInfo(d, dependencyProperty, valueEntry, newEntry);
							if ((ulong)num2 == (ulong)((long)num3))
							{
								break;
							}
						}
					}
				}
			}
			return frugalObjectList;
		}

		// Token: 0x06000C5A RID: 3162 RVA: 0x0002E17B File Offset: 0x0002C37B
		internal void ResetInheritableValueIndexer()
		{
			this._valueIndexer = 0;
		}

		// Token: 0x06000C5B RID: 3163 RVA: 0x0002E184 File Offset: 0x0002C384
		internal InheritablePropertyChangeInfo GetRootInheritableValue(DependencyProperty dp)
		{
			InheritablePropertyChangeInfo result;
			do
			{
				InheritablePropertyChangeInfo[] rootInheritableValues = this._rootInheritableValues;
				int valueIndexer = this._valueIndexer;
				this._valueIndexer = valueIndexer + 1;
				result = rootInheritableValues[valueIndexer];
			}
			while (result.Property != dp);
			return result;
		}

		// Token: 0x170003F5 RID: 1013
		// (get) Token: 0x06000C5C RID: 3164 RVA: 0x0002E1B9 File Offset: 0x0002C3B9
		internal Stack<FrugalObjectList<DependencyProperty>> InheritablePropertiesStack
		{
			get
			{
				if (this._inheritablePropertiesStack == null)
				{
					this._inheritablePropertiesStack = new Stack<FrugalObjectList<DependencyProperty>>(1);
				}
				return this._inheritablePropertiesStack;
			}
		}

		// Token: 0x170003F6 RID: 1014
		// (get) Token: 0x06000C5D RID: 3165 RVA: 0x0002E1D5 File Offset: 0x0002C3D5
		// (set) Token: 0x06000C5E RID: 3166 RVA: 0x0002E1DD File Offset: 0x0002C3DD
		internal object TopmostCollapsedParentNode
		{
			get
			{
				return this._topmostCollapsedParentNode;
			}
			set
			{
				this._topmostCollapsedParentNode = value;
			}
		}

		// Token: 0x170003F7 RID: 1015
		// (get) Token: 0x06000C5F RID: 3167 RVA: 0x0002E1E6 File Offset: 0x0002C3E6
		internal bool IsAddOperation
		{
			get
			{
				return this._isAddOperation;
			}
		}

		// Token: 0x170003F8 RID: 1016
		// (get) Token: 0x06000C60 RID: 3168 RVA: 0x0002E1EE File Offset: 0x0002C3EE
		internal DependencyObject Root
		{
			get
			{
				return this._rootOfChange;
			}
		}

		// Token: 0x04000B01 RID: 2817
		private Stack<FrugalObjectList<DependencyProperty>> _inheritablePropertiesStack;

		// Token: 0x04000B02 RID: 2818
		private object _topmostCollapsedParentNode;

		// Token: 0x04000B03 RID: 2819
		private bool _isAddOperation;

		// Token: 0x04000B04 RID: 2820
		private DependencyObject _rootOfChange;

		// Token: 0x04000B05 RID: 2821
		private InheritablePropertyChangeInfo[] _rootInheritableValues;

		// Token: 0x04000B06 RID: 2822
		private int _valueIndexer;
	}
}
