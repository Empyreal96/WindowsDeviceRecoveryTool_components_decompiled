using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Controls;
using MS.Internal;
using MS.Internal.Data;

namespace System.Windows.Data
{
	/// <summary>Contains instance information about a single instance of a <see cref="T:System.Windows.Data.PriorityBinding" />.</summary>
	// Token: 0x020001B8 RID: 440
	public sealed class PriorityBindingExpression : BindingExpressionBase
	{
		// Token: 0x06001C69 RID: 7273 RVA: 0x00085C1B File Offset: 0x00083E1B
		private PriorityBindingExpression(PriorityBinding binding, BindingExpressionBase owner) : base(binding, owner)
		{
		}

		/// <summary>Gets the <see cref="T:System.Windows.Data.PriorityBinding" /> object from which this <see cref="T:System.Windows.Data.PriorityBindingExpression" /> is created.</summary>
		/// <returns>The <see cref="T:System.Windows.Data.PriorityBinding" /> object from which this <see cref="T:System.Windows.Data.PriorityBindingExpression" /> is created.</returns>
		// Token: 0x170006AC RID: 1708
		// (get) Token: 0x06001C6A RID: 7274 RVA: 0x00085C38 File Offset: 0x00083E38
		public PriorityBinding ParentPriorityBinding
		{
			get
			{
				return (PriorityBinding)base.ParentBindingBase;
			}
		}

		/// <summary>Gets the collection of <see cref="T:System.Windows.Data.BindingExpression" /> objects inside this instance of <see cref="T:System.Windows.Data.PriorityBindingExpression" />.</summary>
		/// <returns>A read-only collection of the <see cref="T:System.Windows.Data.BindingExpression" /> objects. Although the return type is a collection of <see cref="T:System.Windows.Data.BindingExpressionBase" /> objects, the returned collection only contains <see cref="T:System.Windows.Data.BindingExpression" /> objects because the <see cref="T:System.Windows.Data.PriorityBinding" /> class currently supports only <see cref="T:System.Windows.Data.Binding" /> objects.</returns>
		// Token: 0x170006AD RID: 1709
		// (get) Token: 0x06001C6B RID: 7275 RVA: 0x00085C45 File Offset: 0x00083E45
		public ReadOnlyCollection<BindingExpressionBase> BindingExpressions
		{
			get
			{
				return new ReadOnlyCollection<BindingExpressionBase>(this.MutableBindingExpressions);
			}
		}

		/// <summary>Gets the active <see cref="T:System.Windows.Data.BindingExpression" /> object.</summary>
		/// <returns>The active <see cref="T:System.Windows.Data.BindingExpression" /> object; or <see langword="null" />, if no <see cref="T:System.Windows.Data.BindingExpression" /> object is active. Although the return type is <see cref="T:System.Windows.Data.BindingExpressionBase" />, the returned object is only a <see cref="T:System.Windows.Data.BindingExpression" /> object because the <see cref="T:System.Windows.Data.PriorityBinding" /> class currently supports only <see cref="T:System.Windows.Data.Binding" /> objects.</returns>
		// Token: 0x170006AE RID: 1710
		// (get) Token: 0x06001C6C RID: 7276 RVA: 0x00085C52 File Offset: 0x00083E52
		public BindingExpressionBase ActiveBindingExpression
		{
			get
			{
				if (this._activeIndex >= 0)
				{
					return this.MutableBindingExpressions[this._activeIndex];
				}
				return null;
			}
		}

		/// <summary>Gets a value that indicates whether the parent binding has a failed validation rule.</summary>
		/// <returns>
		///     <see langword="true" /> if the parent binding has a failed validation rule; otherwise, <see langword="false" />.</returns>
		// Token: 0x170006AF RID: 1711
		// (get) Token: 0x06001C6D RID: 7277 RVA: 0x00085C70 File Offset: 0x00083E70
		public override bool HasValidationError
		{
			get
			{
				return this._activeIndex >= 0 && this.MutableBindingExpressions[this._activeIndex].HasValidationError;
			}
		}

		/// <summary>Updates the target on the active binding. </summary>
		// Token: 0x06001C6E RID: 7278 RVA: 0x00085C94 File Offset: 0x00083E94
		public override void UpdateTarget()
		{
			BindingExpressionBase activeBindingExpression = this.ActiveBindingExpression;
			if (activeBindingExpression != null)
			{
				activeBindingExpression.UpdateTarget();
			}
		}

		/// <summary>Updates the source on the active binding.</summary>
		// Token: 0x06001C6F RID: 7279 RVA: 0x00085CB4 File Offset: 0x00083EB4
		public override void UpdateSource()
		{
			BindingExpressionBase activeBindingExpression = this.ActiveBindingExpression;
			if (activeBindingExpression != null)
			{
				activeBindingExpression.UpdateSource();
			}
		}

		// Token: 0x06001C70 RID: 7280 RVA: 0x00085CD4 File Offset: 0x00083ED4
		internal override bool SetValue(DependencyObject d, DependencyProperty dp, object value)
		{
			BindingExpressionBase activeBindingExpression = this.ActiveBindingExpression;
			bool flag;
			if (activeBindingExpression != null)
			{
				flag = activeBindingExpression.SetValue(d, dp, value);
				if (flag)
				{
					base.Value = activeBindingExpression.Value;
					base.AdoptProperties(activeBindingExpression);
					base.NotifyCommitManager();
				}
			}
			else
			{
				flag = true;
			}
			return flag;
		}

		// Token: 0x06001C71 RID: 7281 RVA: 0x00085D18 File Offset: 0x00083F18
		internal static PriorityBindingExpression CreateBindingExpression(DependencyObject d, DependencyProperty dp, PriorityBinding binding, BindingExpressionBase owner)
		{
			FrameworkPropertyMetadata frameworkPropertyMetadata = dp.GetMetadata(d.DependencyObjectType) as FrameworkPropertyMetadata;
			if ((frameworkPropertyMetadata != null && !frameworkPropertyMetadata.IsDataBindingAllowed) || dp.ReadOnly)
			{
				throw new ArgumentException(SR.Get("PropertyNotBindable", new object[]
				{
					dp.Name
				}), "dp");
			}
			return new PriorityBindingExpression(binding, owner);
		}

		// Token: 0x170006B0 RID: 1712
		// (get) Token: 0x06001C72 RID: 7282 RVA: 0x00085D77 File Offset: 0x00083F77
		internal int AttentiveBindingExpressions
		{
			get
			{
				if (this._activeIndex != -1)
				{
					return this._activeIndex + 1;
				}
				return this.MutableBindingExpressions.Count;
			}
		}

		// Token: 0x06001C73 RID: 7283 RVA: 0x00085D98 File Offset: 0x00083F98
		internal override bool AttachOverride(DependencyObject d, DependencyProperty dp)
		{
			if (!base.AttachOverride(d, dp))
			{
				return false;
			}
			if (base.TargetElement == null)
			{
				return false;
			}
			base.SetStatus(BindingStatusInternal.Active);
			int count = this.ParentPriorityBinding.Bindings.Count;
			this._activeIndex = -1;
			for (int i = 0; i < count; i++)
			{
				this.AttachBindingExpression(i, false);
			}
			return true;
		}

		// Token: 0x06001C74 RID: 7284 RVA: 0x00085DF4 File Offset: 0x00083FF4
		internal override void DetachOverride()
		{
			int count = this.MutableBindingExpressions.Count;
			for (int i = 0; i < count; i++)
			{
				BindingExpressionBase bindingExpressionBase = this.MutableBindingExpressions[i];
				if (bindingExpressionBase != null)
				{
					bindingExpressionBase.Detach();
				}
			}
			base.ChangeSources(null);
			base.DetachOverride();
		}

		// Token: 0x06001C75 RID: 7285 RVA: 0x00085E3C File Offset: 0x0008403C
		internal override void InvalidateChild(BindingExpressionBase bindingExpression)
		{
			if (this._isInInvalidateBinding)
			{
				return;
			}
			this._isInInvalidateBinding = true;
			int num = this.MutableBindingExpressions.IndexOf(bindingExpression);
			DependencyObject targetElement = base.TargetElement;
			if (targetElement != null && 0 <= num && num < this.AttentiveBindingExpressions)
			{
				if (num != this._activeIndex || (bindingExpression.StatusInternal != BindingStatusInternal.Active && !bindingExpression.UsingFallbackValue))
				{
					this.ChooseActiveBindingExpression(targetElement);
				}
				base.UsingFallbackValue = false;
				BindingExpressionBase activeBindingExpression = this.ActiveBindingExpression;
				object obj = (activeBindingExpression != null) ? activeBindingExpression.GetValue(targetElement, base.TargetProperty) : base.UseFallbackValue();
				base.ChangeValue(obj, true);
				if (TraceData.IsExtendedTraceEnabled(this, TraceDataLevel.Transfer))
				{
					TraceData.Trace(TraceEventType.Warning, TraceData.PriorityTransfer(new object[]
					{
						TraceData.Identify(this),
						TraceData.Identify(obj),
						this._activeIndex,
						TraceData.Identify(activeBindingExpression)
					}));
				}
				if (!base.IsAttaching)
				{
					targetElement.InvalidateProperty(base.TargetProperty);
				}
			}
			this._isInInvalidateBinding = false;
		}

		// Token: 0x06001C76 RID: 7286 RVA: 0x00085F34 File Offset: 0x00084134
		internal override void ChangeSourcesForChild(BindingExpressionBase bindingExpression, WeakDependencySource[] newSources)
		{
			int num = this.MutableBindingExpressions.IndexOf(bindingExpression);
			if (num >= 0)
			{
				WeakDependencySource[] newSources2 = BindingExpressionBase.CombineSources(num, this.MutableBindingExpressions, this.AttentiveBindingExpressions, newSources, null);
				base.ChangeSources(newSources2);
			}
		}

		// Token: 0x06001C77 RID: 7287 RVA: 0x00085F70 File Offset: 0x00084170
		internal override void ReplaceChild(BindingExpressionBase bindingExpression)
		{
			int num = this.MutableBindingExpressions.IndexOf(bindingExpression);
			DependencyObject targetElement = base.TargetElement;
			if (num >= 0 && targetElement != null)
			{
				bindingExpression.Detach();
				bindingExpression = this.AttachBindingExpression(num, true);
			}
		}

		// Token: 0x06001C78 RID: 7288 RVA: 0x00085FA8 File Offset: 0x000841A8
		internal override void UpdateBindingGroup(BindingGroup bg)
		{
			int i = 0;
			int num = this.MutableBindingExpressions.Count - 1;
			while (i < num)
			{
				this.MutableBindingExpressions[i].UpdateBindingGroup(bg);
				i++;
			}
		}

		// Token: 0x06001C79 RID: 7289 RVA: 0x00085FE4 File Offset: 0x000841E4
		internal override bool ShouldReactToDirtyOverride()
		{
			BindingExpressionBase activeBindingExpression = this.ActiveBindingExpression;
			return activeBindingExpression != null && activeBindingExpression.ShouldReactToDirtyOverride();
		}

		// Token: 0x06001C7A RID: 7290 RVA: 0x00086004 File Offset: 0x00084204
		internal override object GetRawProposedValue()
		{
			BindingExpressionBase activeBindingExpression = this.ActiveBindingExpression;
			if (activeBindingExpression != null)
			{
				return activeBindingExpression.GetRawProposedValue();
			}
			return DependencyProperty.UnsetValue;
		}

		// Token: 0x06001C7B RID: 7291 RVA: 0x00086028 File Offset: 0x00084228
		internal override object ConvertProposedValue(object rawValue)
		{
			BindingExpressionBase activeBindingExpression = this.ActiveBindingExpression;
			if (activeBindingExpression != null)
			{
				return activeBindingExpression.ConvertProposedValue(rawValue);
			}
			return DependencyProperty.UnsetValue;
		}

		// Token: 0x06001C7C RID: 7292 RVA: 0x0008604C File Offset: 0x0008424C
		internal override bool ObtainConvertedProposedValue(BindingGroup bindingGroup)
		{
			BindingExpressionBase activeBindingExpression = this.ActiveBindingExpression;
			return activeBindingExpression == null || activeBindingExpression.ObtainConvertedProposedValue(bindingGroup);
		}

		// Token: 0x06001C7D RID: 7293 RVA: 0x0008606C File Offset: 0x0008426C
		internal override object UpdateSource(object convertedValue)
		{
			BindingExpressionBase activeBindingExpression = this.ActiveBindingExpression;
			object result;
			if (activeBindingExpression != null)
			{
				result = activeBindingExpression.UpdateSource(convertedValue);
				if (activeBindingExpression.StatusInternal == BindingStatusInternal.UpdateSourceError)
				{
					base.SetStatus(BindingStatusInternal.UpdateSourceError);
				}
			}
			else
			{
				result = DependencyProperty.UnsetValue;
			}
			return result;
		}

		// Token: 0x06001C7E RID: 7294 RVA: 0x000860A4 File Offset: 0x000842A4
		internal override bool UpdateSource(BindingGroup bindingGroup)
		{
			bool result = true;
			BindingExpressionBase activeBindingExpression = this.ActiveBindingExpression;
			if (activeBindingExpression != null)
			{
				result = activeBindingExpression.UpdateSource(bindingGroup);
				if (activeBindingExpression.StatusInternal == BindingStatusInternal.UpdateSourceError)
				{
					base.SetStatus(BindingStatusInternal.UpdateSourceError);
				}
			}
			return result;
		}

		// Token: 0x06001C7F RID: 7295 RVA: 0x000860D8 File Offset: 0x000842D8
		internal override void StoreValueInBindingGroup(object value, BindingGroup bindingGroup)
		{
			BindingExpressionBase activeBindingExpression = this.ActiveBindingExpression;
			if (activeBindingExpression != null)
			{
				activeBindingExpression.StoreValueInBindingGroup(value, bindingGroup);
			}
		}

		// Token: 0x06001C80 RID: 7296 RVA: 0x000860F8 File Offset: 0x000842F8
		internal override bool Validate(object value, ValidationStep validationStep)
		{
			BindingExpressionBase activeBindingExpression = this.ActiveBindingExpression;
			return activeBindingExpression == null || activeBindingExpression.Validate(value, validationStep);
		}

		// Token: 0x06001C81 RID: 7297 RVA: 0x0008611C File Offset: 0x0008431C
		internal override bool CheckValidationRules(BindingGroup bindingGroup, ValidationStep validationStep)
		{
			BindingExpressionBase activeBindingExpression = this.ActiveBindingExpression;
			return activeBindingExpression == null || activeBindingExpression.CheckValidationRules(bindingGroup, validationStep);
		}

		// Token: 0x06001C82 RID: 7298 RVA: 0x00086140 File Offset: 0x00084340
		internal override bool ValidateAndConvertProposedValue(out Collection<BindingExpressionBase.ProposedValue> values)
		{
			BindingExpressionBase activeBindingExpression = this.ActiveBindingExpression;
			if (activeBindingExpression != null)
			{
				return activeBindingExpression.ValidateAndConvertProposedValue(out values);
			}
			values = null;
			return true;
		}

		// Token: 0x06001C83 RID: 7299 RVA: 0x00086164 File Offset: 0x00084364
		internal override object GetSourceItem(object newValue)
		{
			BindingExpressionBase activeBindingExpression = this.ActiveBindingExpression;
			if (activeBindingExpression != null)
			{
				return activeBindingExpression.GetSourceItem(newValue);
			}
			return true;
		}

		// Token: 0x06001C84 RID: 7300 RVA: 0x0008618C File Offset: 0x0008438C
		internal override void UpdateCommitState()
		{
			BindingExpressionBase activeBindingExpression = this.ActiveBindingExpression;
			if (activeBindingExpression != null)
			{
				base.AdoptProperties(activeBindingExpression);
			}
		}

		// Token: 0x170006B1 RID: 1713
		// (get) Token: 0x06001C85 RID: 7301 RVA: 0x000861AA File Offset: 0x000843AA
		private Collection<BindingExpressionBase> MutableBindingExpressions
		{
			get
			{
				return this._list;
			}
		}

		// Token: 0x06001C86 RID: 7302 RVA: 0x000861B4 File Offset: 0x000843B4
		private BindingExpressionBase AttachBindingExpression(int i, bool replaceExisting)
		{
			DependencyObject targetElement = base.TargetElement;
			if (targetElement == null)
			{
				return null;
			}
			BindingBase bindingBase = this.ParentPriorityBinding.Bindings[i];
			BindingExpressionBase bindingExpressionBase = bindingBase.CreateBindingExpression(targetElement, base.TargetProperty, this);
			if (replaceExisting)
			{
				this.MutableBindingExpressions[i] = bindingExpressionBase;
			}
			else
			{
				this.MutableBindingExpressions.Add(bindingExpressionBase);
			}
			bindingExpressionBase.Attach(targetElement, base.TargetProperty);
			return bindingExpressionBase;
		}

		// Token: 0x06001C87 RID: 7303 RVA: 0x0008621C File Offset: 0x0008441C
		private void ChooseActiveBindingExpression(DependencyObject target)
		{
			int count = this.MutableBindingExpressions.Count;
			int i;
			for (i = 0; i < count; i++)
			{
				BindingExpressionBase bindingExpressionBase = this.MutableBindingExpressions[i];
				if (bindingExpressionBase.StatusInternal == BindingStatusInternal.Inactive)
				{
					bindingExpressionBase.Activate();
				}
				if (bindingExpressionBase.StatusInternal == BindingStatusInternal.Active || bindingExpressionBase.UsingFallbackValue)
				{
					break;
				}
			}
			int num = (i < count) ? i : -1;
			if (num != this._activeIndex)
			{
				int activeIndex = this._activeIndex;
				this._activeIndex = num;
				base.AdoptProperties(this.ActiveBindingExpression);
				WeakDependencySource[] newSources = BindingExpressionBase.CombineSources(-1, this.MutableBindingExpressions, this.AttentiveBindingExpressions, null, null);
				base.ChangeSources(newSources);
				if (num != -1)
				{
					for (i = activeIndex; i > num; i--)
					{
						this.MutableBindingExpressions[i].Deactivate();
					}
				}
			}
		}

		// Token: 0x06001C88 RID: 7304 RVA: 0x00002137 File Offset: 0x00000337
		private void ChangeValue()
		{
		}

		// Token: 0x06001C89 RID: 7305 RVA: 0x000862DC File Offset: 0x000844DC
		internal override void HandlePropertyInvalidation(DependencyObject d, DependencyPropertyChangedEventArgs args)
		{
			DependencyProperty property = args.Property;
			if (TraceData.IsExtendedTraceEnabled(this, TraceDataLevel.Transfer))
			{
				TraceData.Trace(TraceEventType.Warning, TraceData.GotPropertyChanged(new object[]
				{
					TraceData.Identify(this),
					TraceData.Identify(d),
					property.Name
				}));
			}
			for (int i = 0; i < this.AttentiveBindingExpressions; i++)
			{
				BindingExpressionBase bindingExpressionBase = this.MutableBindingExpressions[i];
				DependencySource[] sources = bindingExpressionBase.GetSources();
				if (sources != null)
				{
					foreach (DependencySource dependencySource in sources)
					{
						if (dependencySource.DependencyObject == d && dependencySource.DependencyProperty == property)
						{
							bindingExpressionBase.OnPropertyInvalidation(d, args);
							break;
						}
					}
				}
			}
		}

		// Token: 0x040013D4 RID: 5076
		private const int NoActiveBindingExpressions = -1;

		// Token: 0x040013D5 RID: 5077
		private const int UnknownActiveBindingExpression = -2;

		// Token: 0x040013D6 RID: 5078
		private Collection<BindingExpressionBase> _list = new Collection<BindingExpressionBase>();

		// Token: 0x040013D7 RID: 5079
		private int _activeIndex = -2;

		// Token: 0x040013D8 RID: 5080
		private bool _isInInvalidateBinding;
	}
}
