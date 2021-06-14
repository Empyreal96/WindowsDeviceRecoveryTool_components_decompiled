using System;
using System.ComponentModel;
using System.Windows.Markup;
using MS.Internal;

namespace System.Windows
{
	// Token: 0x020000EE RID: 238
	[TypeConverter(typeof(ResourceReferenceExpressionConverter))]
	internal class ResourceReferenceExpression : Expression
	{
		// Token: 0x06000871 RID: 2161 RVA: 0x0001B84D File Offset: 0x00019A4D
		public ResourceReferenceExpression(object resourceKey)
		{
			this._resourceKey = resourceKey;
		}

		// Token: 0x06000872 RID: 2162 RVA: 0x0000C238 File Offset: 0x0000A438
		internal override DependencySource[] GetSources()
		{
			return null;
		}

		// Token: 0x06000873 RID: 2163 RVA: 0x0001B85C File Offset: 0x00019A5C
		internal override object GetValue(DependencyObject d, DependencyProperty dp)
		{
			if (d == null)
			{
				throw new ArgumentNullException("d");
			}
			if (dp == null)
			{
				throw new ArgumentNullException("dp");
			}
			if (this.ReadInternalState(ResourceReferenceExpression.InternalState.HasCachedResourceValue))
			{
				return this._cachedResourceValue;
			}
			object obj;
			return this.GetRawValue(d, out obj, dp);
		}

		// Token: 0x06000874 RID: 2164 RVA: 0x0001B89F File Offset: 0x00019A9F
		internal override Expression Copy(DependencyObject targetObject, DependencyProperty targetDP)
		{
			return new ResourceReferenceExpression(this.ResourceKey);
		}

		// Token: 0x06000875 RID: 2165 RVA: 0x0001B8AC File Offset: 0x00019AAC
		internal object GetRawValue(DependencyObject d, out object source, DependencyProperty dp)
		{
			if (!this.ReadInternalState(ResourceReferenceExpression.InternalState.IsMentorCacheValid))
			{
				this._mentorCache = Helper.FindMentor(d);
				this.WriteInternalState(ResourceReferenceExpression.InternalState.IsMentorCacheValid, true);
				if (this._mentorCache != null && this._mentorCache != this._targetObject)
				{
					FrameworkElement frameworkElement;
					FrameworkContentElement frameworkContentElement;
					Helper.DowncastToFEorFCE(this._mentorCache, out frameworkElement, out frameworkContentElement, true);
					if (frameworkElement != null)
					{
						frameworkElement.ResourcesChanged += this.InvalidateExpressionValue;
					}
					else
					{
						frameworkContentElement.ResourcesChanged += this.InvalidateExpressionValue;
					}
				}
			}
			object obj;
			if (this._mentorCache != null)
			{
				FrameworkElement fe;
				FrameworkContentElement fce;
				Helper.DowncastToFEorFCE(this._mentorCache, out fe, out fce, true);
				obj = FrameworkElement.FindResourceInternal(fe, fce, dp, this._resourceKey, null, true, false, null, false, out source);
			}
			else
			{
				obj = FrameworkElement.FindResourceFromAppOrSystem(this._resourceKey, out source, false, true, false);
			}
			if (obj == null)
			{
				obj = DependencyProperty.UnsetValue;
			}
			this._cachedResourceValue = obj;
			this.WriteInternalState(ResourceReferenceExpression.InternalState.HasCachedResourceValue, true);
			object resource = obj;
			DeferredResourceReference deferredResourceReference = obj as DeferredResourceReference;
			if (deferredResourceReference != null)
			{
				if (deferredResourceReference.IsInflated)
				{
					resource = (deferredResourceReference.Value as Freezable);
				}
				else if (!this.ReadInternalState(ResourceReferenceExpression.InternalState.IsListeningForInflated))
				{
					deferredResourceReference.AddInflatedListener(this);
					this.WriteInternalState(ResourceReferenceExpression.InternalState.IsListeningForInflated, true);
				}
			}
			this.ListenForFreezableChanges(resource);
			return obj;
		}

		// Token: 0x06000876 RID: 2166 RVA: 0x0000B02A File Offset: 0x0000922A
		internal override bool SetValue(DependencyObject d, DependencyProperty dp, object value)
		{
			return false;
		}

		// Token: 0x06000877 RID: 2167 RVA: 0x0001B9C4 File Offset: 0x00019BC4
		internal override void OnAttach(DependencyObject d, DependencyProperty dp)
		{
			this._targetObject = d;
			this._targetProperty = dp;
			FrameworkObject frameworkObject = new FrameworkObject(this._targetObject);
			frameworkObject.HasResourceReference = true;
			if (!frameworkObject.IsValid)
			{
				this._targetObject.InheritanceContextChanged += this.InvalidateExpressionValue;
			}
		}

		// Token: 0x06000878 RID: 2168 RVA: 0x0001BA14 File Offset: 0x00019C14
		internal override void OnDetach(DependencyObject d, DependencyProperty dp)
		{
			this.InvalidateMentorCache();
			if (!(this._targetObject is FrameworkElement) && !(this._targetObject is FrameworkContentElement))
			{
				this._targetObject.InheritanceContextChanged -= this.InvalidateExpressionValue;
			}
			this._targetObject = null;
			this._targetProperty = null;
			this._weakContainerRRE = null;
		}

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x06000879 RID: 2169 RVA: 0x0001BA6D File Offset: 0x00019C6D
		public object ResourceKey
		{
			get
			{
				return this._resourceKey;
			}
		}

		// Token: 0x0600087A RID: 2170 RVA: 0x0001BA78 File Offset: 0x00019C78
		private void InvalidateCacheValue()
		{
			object resource = this._cachedResourceValue;
			DeferredResourceReference deferredResourceReference = this._cachedResourceValue as DeferredResourceReference;
			if (deferredResourceReference != null)
			{
				if (deferredResourceReference.IsInflated)
				{
					resource = deferredResourceReference.Value;
				}
				else if (this.ReadInternalState(ResourceReferenceExpression.InternalState.IsListeningForInflated))
				{
					deferredResourceReference.RemoveInflatedListener(this);
					this.WriteInternalState(ResourceReferenceExpression.InternalState.IsListeningForInflated, false);
				}
				deferredResourceReference.RemoveFromDictionary();
			}
			this.StopListeningForFreezableChanges(resource);
			this._cachedResourceValue = null;
			this.WriteInternalState(ResourceReferenceExpression.InternalState.HasCachedResourceValue, false);
		}

		// Token: 0x0600087B RID: 2171 RVA: 0x0001BAE4 File Offset: 0x00019CE4
		private void InvalidateMentorCache()
		{
			if (this.ReadInternalState(ResourceReferenceExpression.InternalState.IsMentorCacheValid))
			{
				if (this._mentorCache != null)
				{
					if (this._mentorCache != this._targetObject)
					{
						FrameworkElement frameworkElement;
						FrameworkContentElement frameworkContentElement;
						Helper.DowncastToFEorFCE(this._mentorCache, out frameworkElement, out frameworkContentElement, true);
						if (frameworkElement != null)
						{
							frameworkElement.ResourcesChanged -= this.InvalidateExpressionValue;
						}
						else
						{
							frameworkContentElement.ResourcesChanged -= this.InvalidateExpressionValue;
						}
					}
					this._mentorCache = null;
				}
				this.WriteInternalState(ResourceReferenceExpression.InternalState.IsMentorCacheValid, false);
			}
			this.InvalidateCacheValue();
		}

		// Token: 0x0600087C RID: 2172 RVA: 0x0001BB60 File Offset: 0x00019D60
		internal void InvalidateExpressionValue(object sender, EventArgs e)
		{
			if (this._targetObject == null)
			{
				return;
			}
			ResourcesChangedEventArgs resourcesChangedEventArgs = e as ResourcesChangedEventArgs;
			if (resourcesChangedEventArgs != null)
			{
				if (!resourcesChangedEventArgs.Info.IsTreeChange)
				{
					this.InvalidateCacheValue();
				}
				else
				{
					this.InvalidateMentorCache();
				}
			}
			else
			{
				this.InvalidateMentorCache();
			}
			this.InvalidateTargetProperty(sender, e);
		}

		// Token: 0x0600087D RID: 2173 RVA: 0x0001BBAE File Offset: 0x00019DAE
		private void InvalidateTargetProperty(object sender, EventArgs e)
		{
			this._targetObject.InvalidateProperty(this._targetProperty);
		}

		// Token: 0x0600087E RID: 2174 RVA: 0x0001BBC1 File Offset: 0x00019DC1
		private void InvalidateTargetSubProperty(object sender, EventArgs e)
		{
			this._targetObject.NotifySubPropertyChange(this._targetProperty);
		}

		// Token: 0x0600087F RID: 2175 RVA: 0x0001BBD4 File Offset: 0x00019DD4
		private void ListenForFreezableChanges(object resource)
		{
			if (!this.ReadInternalState(ResourceReferenceExpression.InternalState.IsListeningForFreezableChanges))
			{
				Freezable freezable = resource as Freezable;
				if (freezable != null && !freezable.IsFrozen)
				{
					if (this._weakContainerRRE == null)
					{
						this._weakContainerRRE = new ResourceReferenceExpression.ResourceReferenceExpressionWeakContainer(this);
					}
					this._weakContainerRRE.AddChangedHandler(freezable);
					this.WriteInternalState(ResourceReferenceExpression.InternalState.IsListeningForFreezableChanges, true);
				}
			}
		}

		// Token: 0x06000880 RID: 2176 RVA: 0x0001BC24 File Offset: 0x00019E24
		private void StopListeningForFreezableChanges(object resource)
		{
			if (this.ReadInternalState(ResourceReferenceExpression.InternalState.IsListeningForFreezableChanges))
			{
				Freezable freezable = resource as Freezable;
				if (freezable != null && this._weakContainerRRE != null)
				{
					if (!freezable.IsFrozen)
					{
						this._weakContainerRRE.RemoveChangedHandler();
					}
					else
					{
						this._weakContainerRRE = null;
					}
				}
				this.WriteInternalState(ResourceReferenceExpression.InternalState.IsListeningForFreezableChanges, false);
			}
		}

		// Token: 0x06000881 RID: 2177 RVA: 0x0001BC70 File Offset: 0x00019E70
		internal void OnDeferredResourceInflated(DeferredResourceReference deferredResourceReference)
		{
			if (this.ReadInternalState(ResourceReferenceExpression.InternalState.IsListeningForInflated))
			{
				deferredResourceReference.RemoveInflatedListener(this);
				this.WriteInternalState(ResourceReferenceExpression.InternalState.IsListeningForInflated, false);
			}
			this.ListenForFreezableChanges(deferredResourceReference.Value);
		}

		// Token: 0x06000882 RID: 2178 RVA: 0x0001BC98 File Offset: 0x00019E98
		private bool ReadInternalState(ResourceReferenceExpression.InternalState reqFlag)
		{
			return (this._state & reqFlag) > ResourceReferenceExpression.InternalState.Default;
		}

		// Token: 0x06000883 RID: 2179 RVA: 0x0001BCA5 File Offset: 0x00019EA5
		private void WriteInternalState(ResourceReferenceExpression.InternalState reqFlag, bool set)
		{
			if (set)
			{
				this._state |= reqFlag;
				return;
			}
			this._state &= ~reqFlag;
		}

		// Token: 0x0400079F RID: 1951
		private object _resourceKey;

		// Token: 0x040007A0 RID: 1952
		private object _cachedResourceValue;

		// Token: 0x040007A1 RID: 1953
		private DependencyObject _mentorCache;

		// Token: 0x040007A2 RID: 1954
		private DependencyObject _targetObject;

		// Token: 0x040007A3 RID: 1955
		private DependencyProperty _targetProperty;

		// Token: 0x040007A4 RID: 1956
		private ResourceReferenceExpression.InternalState _state;

		// Token: 0x040007A5 RID: 1957
		private ResourceReferenceExpression.ResourceReferenceExpressionWeakContainer _weakContainerRRE;

		// Token: 0x02000827 RID: 2087
		[Flags]
		private enum InternalState : byte
		{
			// Token: 0x04003BEC RID: 15340
			Default = 0,
			// Token: 0x04003BED RID: 15341
			HasCachedResourceValue = 1,
			// Token: 0x04003BEE RID: 15342
			IsMentorCacheValid = 2,
			// Token: 0x04003BEF RID: 15343
			DisableThrowOnResourceFailure = 4,
			// Token: 0x04003BF0 RID: 15344
			IsListeningForFreezableChanges = 8,
			// Token: 0x04003BF1 RID: 15345
			IsListeningForInflated = 16
		}

		// Token: 0x02000828 RID: 2088
		private class ResourceReferenceExpressionWeakContainer : WeakReference
		{
			// Token: 0x06007E6E RID: 32366 RVA: 0x00235BB8 File Offset: 0x00233DB8
			public ResourceReferenceExpressionWeakContainer(ResourceReferenceExpression target) : base(target)
			{
			}

			// Token: 0x06007E6F RID: 32367 RVA: 0x00235BC4 File Offset: 0x00233DC4
			private void InvalidateTargetSubProperty(object sender, EventArgs args)
			{
				ResourceReferenceExpression resourceReferenceExpression = (ResourceReferenceExpression)this.Target;
				if (resourceReferenceExpression != null)
				{
					resourceReferenceExpression.InvalidateTargetSubProperty(sender, args);
					return;
				}
				this.RemoveChangedHandler();
			}

			// Token: 0x06007E70 RID: 32368 RVA: 0x00235BEF File Offset: 0x00233DEF
			public void AddChangedHandler(Freezable resource)
			{
				if (this._resource != null)
				{
					this.RemoveChangedHandler();
				}
				this._resource = resource;
				this._resource.Changed += this.InvalidateTargetSubProperty;
			}

			// Token: 0x06007E71 RID: 32369 RVA: 0x00235C1D File Offset: 0x00233E1D
			public void RemoveChangedHandler()
			{
				if (!this._resource.IsFrozen)
				{
					this._resource.Changed -= this.InvalidateTargetSubProperty;
					this._resource = null;
				}
			}

			// Token: 0x04003BF2 RID: 15346
			private Freezable _resource;
		}
	}
}
