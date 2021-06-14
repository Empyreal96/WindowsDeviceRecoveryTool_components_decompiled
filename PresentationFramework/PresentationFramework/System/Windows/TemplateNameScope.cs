using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows.Markup;
using MS.Internal;

namespace System.Windows
{
	// Token: 0x02000123 RID: 291
	internal class TemplateNameScope : INameScope
	{
		// Token: 0x06000C1A RID: 3098 RVA: 0x0002D27A File Offset: 0x0002B47A
		internal TemplateNameScope(DependencyObject templatedParent) : this(templatedParent, null, null)
		{
		}

		// Token: 0x06000C1B RID: 3099 RVA: 0x0002D285 File Offset: 0x0002B485
		internal TemplateNameScope(DependencyObject templatedParent, List<DependencyObject> affectedChildren, FrameworkTemplate frameworkTemplate)
		{
			this._affectedChildren = affectedChildren;
			this._frameworkTemplate = frameworkTemplate;
			this._templatedParent = templatedParent;
			this._isTemplatedParentAnFE = true;
		}

		// Token: 0x06000C1C RID: 3100 RVA: 0x0002D2A9 File Offset: 0x0002B4A9
		void INameScope.RegisterName(string name, object scopedElement)
		{
			if (!(scopedElement is FrameworkContentElement) && !(scopedElement is FrameworkElement))
			{
				this.RegisterNameInternal(name, scopedElement);
			}
		}

		// Token: 0x06000C1D RID: 3101 RVA: 0x0002D2C4 File Offset: 0x0002B4C4
		internal void RegisterNameInternal(string name, object scopedElement)
		{
			FrameworkElement frameworkElement;
			FrameworkContentElement frameworkContentElement;
			Helper.DowncastToFEorFCE(scopedElement as DependencyObject, out frameworkElement, out frameworkContentElement, false);
			if (this._templatedParent == null)
			{
				if (this._nameMap == null)
				{
					this._nameMap = new HybridDictionary();
				}
				this._nameMap[name] = scopedElement;
				if (frameworkElement != null || frameworkContentElement != null)
				{
					this.SetTemplateParentValues(name, scopedElement);
					return;
				}
			}
			else
			{
				if (frameworkElement == null && frameworkContentElement == null)
				{
					Hashtable hashtable = TemplateNameScope._templatedNonFeChildrenField.GetValue(this._templatedParent);
					if (hashtable == null)
					{
						hashtable = new Hashtable(1);
						TemplateNameScope._templatedNonFeChildrenField.SetValue(this._templatedParent, hashtable);
					}
					hashtable[name] = scopedElement;
					return;
				}
				this._affectedChildren.Add(scopedElement as DependencyObject);
				int num;
				if (frameworkElement != null)
				{
					frameworkElement._templatedParent = this._templatedParent;
					frameworkElement.IsTemplatedParentAnFE = this._isTemplatedParentAnFE;
					num = (frameworkElement.TemplateChildIndex = (int)this._frameworkTemplate.ChildIndexFromChildName[name]);
				}
				else
				{
					frameworkContentElement._templatedParent = this._templatedParent;
					frameworkContentElement.IsTemplatedParentAnFE = this._isTemplatedParentAnFE;
					num = (frameworkContentElement.TemplateChildIndex = (int)this._frameworkTemplate.ChildIndexFromChildName[name]);
				}
				HybridDictionary templateChildLoadedDictionary = this._frameworkTemplate._TemplateChildLoadedDictionary;
				FrameworkTemplate.TemplateChildLoadedFlags templateChildLoadedFlags = templateChildLoadedDictionary[num] as FrameworkTemplate.TemplateChildLoadedFlags;
				if (templateChildLoadedFlags != null && (templateChildLoadedFlags.HasLoadedChangedHandler || templateChildLoadedFlags.HasUnloadedChangedHandler))
				{
					BroadcastEventHelper.AddHasLoadedChangeHandlerFlagInAncestry((frameworkElement != null) ? frameworkElement : frameworkContentElement);
				}
				StyleHelper.CreateInstanceDataForChild(StyleHelper.TemplateDataField, this._templatedParent, (frameworkElement != null) ? frameworkElement : frameworkContentElement, num, this._frameworkTemplate.HasInstanceValues, ref this._frameworkTemplate.ChildRecordFromChildIndex);
			}
		}

		// Token: 0x06000C1E RID: 3102 RVA: 0x00002137 File Offset: 0x00000337
		void INameScope.UnregisterName(string name)
		{
		}

		// Token: 0x06000C1F RID: 3103 RVA: 0x0002D454 File Offset: 0x0002B654
		object INameScope.FindName(string name)
		{
			if (this._templatedParent != null)
			{
				FrameworkObject frameworkObject = new FrameworkObject(this._templatedParent);
				if (frameworkObject.IsFE)
				{
					return StyleHelper.FindNameInTemplateContent(frameworkObject.FE, name, frameworkObject.FE.TemplateInternal);
				}
				return null;
			}
			else
			{
				if (this._nameMap == null || name == null || name == string.Empty)
				{
					return null;
				}
				return this._nameMap[name];
			}
		}

		// Token: 0x06000C20 RID: 3104 RVA: 0x0002D4C1 File Offset: 0x0002B6C1
		private void SetTemplateParentValues(string name, object element)
		{
			FrameworkTemplate.SetTemplateParentValues(name, element, this._frameworkTemplate, ref this._provideValueServiceProvider);
		}

		// Token: 0x04000ACF RID: 2767
		private List<DependencyObject> _affectedChildren;

		// Token: 0x04000AD0 RID: 2768
		private static UncommonField<Hashtable> _templatedNonFeChildrenField = StyleHelper.TemplatedNonFeChildrenField;

		// Token: 0x04000AD1 RID: 2769
		private DependencyObject _templatedParent;

		// Token: 0x04000AD2 RID: 2770
		private FrameworkTemplate _frameworkTemplate;

		// Token: 0x04000AD3 RID: 2771
		private bool _isTemplatedParentAnFE;

		// Token: 0x04000AD4 RID: 2772
		private ProvideValueServiceProvider _provideValueServiceProvider;

		// Token: 0x04000AD5 RID: 2773
		private HybridDictionary _nameMap;
	}
}
