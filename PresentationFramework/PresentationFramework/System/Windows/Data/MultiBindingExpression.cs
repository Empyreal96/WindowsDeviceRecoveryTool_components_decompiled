using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Controls;
using MS.Internal;
using MS.Internal.Data;

namespace System.Windows.Data
{
	/// <summary>Contains instance information about a single instance of a <see cref="T:System.Windows.Data.MultiBinding" />.</summary>
	// Token: 0x020001B5 RID: 437
	public sealed class MultiBindingExpression : BindingExpressionBase, IDataBindEngineClient
	{
		// Token: 0x06001C17 RID: 7191 RVA: 0x00083FBC File Offset: 0x000821BC
		private MultiBindingExpression(MultiBinding binding, BindingExpressionBase owner) : base(binding, owner)
		{
			int count = binding.Bindings.Count;
			this._tempValues = new object[count];
			this._tempTypes = new Type[count];
		}

		// Token: 0x06001C18 RID: 7192 RVA: 0x00084000 File Offset: 0x00082200
		void IDataBindEngineClient.TransferValue()
		{
			this.TransferValue();
		}

		// Token: 0x06001C19 RID: 7193 RVA: 0x00074CEE File Offset: 0x00072EEE
		void IDataBindEngineClient.UpdateValue()
		{
			base.UpdateValue();
		}

		// Token: 0x06001C1A RID: 7194 RVA: 0x00084008 File Offset: 0x00082208
		bool IDataBindEngineClient.AttachToContext(bool lastChance)
		{
			this.AttachToContext(lastChance);
			return !base.TransferIsDeferred;
		}

		// Token: 0x06001C1B RID: 7195 RVA: 0x00002137 File Offset: 0x00000337
		void IDataBindEngineClient.VerifySourceReference(bool lastChance)
		{
		}

		// Token: 0x06001C1C RID: 7196 RVA: 0x0008401A File Offset: 0x0008221A
		void IDataBindEngineClient.OnTargetUpdated()
		{
			this.OnTargetUpdated();
		}

		// Token: 0x1700069C RID: 1692
		// (get) Token: 0x06001C1D RID: 7197 RVA: 0x00074D81 File Offset: 0x00072F81
		DependencyObject IDataBindEngineClient.TargetElement
		{
			get
			{
				if (base.UsingMentor)
				{
					return Helper.FindMentor(base.TargetElement);
				}
				return base.TargetElement;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Data.MultiBinding" /> object from which this <see cref="T:System.Windows.Data.MultiBindingExpression" /> is created.</summary>
		/// <returns>The <see cref="T:System.Windows.Data.MultiBinding" /> object from which this <see cref="T:System.Windows.Data.MultiBindingExpression" /> is created.</returns>
		// Token: 0x1700069D RID: 1693
		// (get) Token: 0x06001C1E RID: 7198 RVA: 0x00084022 File Offset: 0x00082222
		public MultiBinding ParentMultiBinding
		{
			get
			{
				return (MultiBinding)base.ParentBindingBase;
			}
		}

		/// <summary>Gets the collection of <see cref="T:System.Windows.Data.BindingExpression" /> objects in this instance of <see cref="T:System.Windows.Data.MultiBindingExpression" />.</summary>
		/// <returns>A read-only collection of the <see cref="T:System.Windows.Data.BindingExpression" /> objects. Even though the return type is a collection of <see cref="T:System.Windows.Data.BindingExpressionBase" /> objects the returned collection would only contain <see cref="T:System.Windows.Data.BindingExpression" /> objects because the <see cref="T:System.Windows.Data.MultiBinding" /> class currently only supports <see cref="T:System.Windows.Data.Binding" /> objects.</returns>
		// Token: 0x1700069E RID: 1694
		// (get) Token: 0x06001C1F RID: 7199 RVA: 0x0008402F File Offset: 0x0008222F
		public ReadOnlyCollection<BindingExpressionBase> BindingExpressions
		{
			get
			{
				return new ReadOnlyCollection<BindingExpressionBase>(this.MutableBindingExpressions);
			}
		}

		/// <summary>Sends the current binding target value to the binding source properties in <see cref="F:System.Windows.Data.BindingMode.TwoWay" /> or <see cref="F:System.Windows.Data.BindingMode.OneWayToSource" /> bindings.</summary>
		// Token: 0x06001C20 RID: 7200 RVA: 0x0008403C File Offset: 0x0008223C
		public override void UpdateSource()
		{
			if (this.MutableBindingExpressions.Count == 0)
			{
				throw new InvalidOperationException(SR.Get("BindingExpressionIsDetached"));
			}
			base.NeedsUpdate = true;
			base.Update();
		}

		/// <summary>Forces a data transfer from the binding source properties to the binding target property.</summary>
		// Token: 0x06001C21 RID: 7201 RVA: 0x00084069 File Offset: 0x00082269
		public override void UpdateTarget()
		{
			if (this.MutableBindingExpressions.Count == 0)
			{
				throw new InvalidOperationException(SR.Get("BindingExpressionIsDetached"));
			}
			this.UpdateTarget(true);
		}

		// Token: 0x1700069F RID: 1695
		// (get) Token: 0x06001C22 RID: 7202 RVA: 0x0008408F File Offset: 0x0008228F
		internal override bool IsParentBindingUpdateTriggerDefault
		{
			get
			{
				return this.ParentMultiBinding.UpdateSourceTrigger == UpdateSourceTrigger.Default;
			}
		}

		// Token: 0x06001C23 RID: 7203 RVA: 0x000840A0 File Offset: 0x000822A0
		internal static MultiBindingExpression CreateBindingExpression(DependencyObject d, DependencyProperty dp, MultiBinding binding, BindingExpressionBase owner)
		{
			FrameworkPropertyMetadata frameworkPropertyMetadata = dp.GetMetadata(d.DependencyObjectType) as FrameworkPropertyMetadata;
			if ((frameworkPropertyMetadata != null && !frameworkPropertyMetadata.IsDataBindingAllowed) || dp.ReadOnly)
			{
				throw new ArgumentException(SR.Get("PropertyNotBindable", new object[]
				{
					dp.Name
				}), "dp");
			}
			MultiBindingExpression multiBindingExpression = new MultiBindingExpression(binding, owner);
			multiBindingExpression.ResolvePropertyDefaultSettings(binding.Mode, binding.UpdateSourceTrigger, frameworkPropertyMetadata);
			return multiBindingExpression;
		}

		// Token: 0x06001C24 RID: 7204 RVA: 0x00084114 File Offset: 0x00082314
		private void AttachToContext(bool lastChance)
		{
			DependencyObject targetElement = base.TargetElement;
			if (targetElement == null)
			{
				return;
			}
			bool flag = TraceData.IsExtendedTraceEnabled(this, TraceDataLevel.Attach);
			this._converter = this.ParentMultiBinding.Converter;
			if (this._converter == null && string.IsNullOrEmpty(base.EffectiveStringFormat))
			{
				TraceData.Trace(TraceEventType.Error, TraceData.MultiBindingHasNoConverter, this.ParentMultiBinding);
			}
			if (flag)
			{
				TraceData.Trace(TraceEventType.Warning, TraceData.AttachToContext(new object[]
				{
					TraceData.Identify(this),
					lastChance ? " (last chance)" : string.Empty
				}));
			}
			base.TransferIsDeferred = true;
			bool flag2 = true;
			int count = this.MutableBindingExpressions.Count;
			for (int i = 0; i < count; i++)
			{
				if (this.MutableBindingExpressions[i].StatusInternal == BindingStatusInternal.Unattached)
				{
					flag2 = false;
				}
			}
			if (!flag2 && !lastChance)
			{
				if (flag)
				{
					TraceData.Trace(TraceEventType.Warning, TraceData.ChildNotAttached(new object[]
					{
						TraceData.Identify(this)
					}));
				}
				return;
			}
			if (base.UsesLanguage)
			{
				WeakDependencySource[] commonSources = new WeakDependencySource[]
				{
					new WeakDependencySource(base.TargetElement, FrameworkElement.LanguageProperty)
				};
				WeakDependencySource[] newSources = BindingExpressionBase.CombineSources(-1, this.MutableBindingExpressions, this.MutableBindingExpressions.Count, null, commonSources);
				base.ChangeSources(newSources);
			}
			bool flag3 = base.IsOneWayToSource;
			object newValue;
			if (base.ShouldUpdateWithCurrentValue(targetElement, out newValue))
			{
				flag3 = true;
				base.ChangeValue(newValue, false);
				base.NeedsUpdate = true;
			}
			base.SetStatus(BindingStatusInternal.Active);
			if (!flag3)
			{
				this.UpdateTarget(false);
				return;
			}
			base.UpdateValue();
		}

		/// <summary>Gets the <see cref="T:System.Windows.Controls.ValidationError" /> object that caused this instance of <see cref="T:System.Windows.Data.MultiBindingExpression" /> to be invalid.</summary>
		/// <returns>The <see cref="T:System.Windows.Controls.ValidationError" /> object that caused this instance of <see cref="T:System.Windows.Data.MultiBindingExpression" /> to be invalid.</returns>
		// Token: 0x170006A0 RID: 1696
		// (get) Token: 0x06001C25 RID: 7205 RVA: 0x00084280 File Offset: 0x00082480
		public override ValidationError ValidationError
		{
			get
			{
				ValidationError validationError = base.ValidationError;
				if (validationError == null)
				{
					for (int i = 0; i < this.MutableBindingExpressions.Count; i++)
					{
						validationError = this.MutableBindingExpressions[i].ValidationError;
						if (validationError != null)
						{
							break;
						}
					}
				}
				return validationError;
			}
		}

		/// <summary>Returns a value that indicates whether any of the inner <see cref="T:System.Windows.Data.Binding" /> objects or the <see cref="T:System.Windows.Data.MultiBinding" /> itself has a failing validation rule.</summary>
		/// <returns>
		///     <see langword="true" /> if at least one of the inner <see cref="T:System.Windows.Data.Binding" /> objects or the <see cref="T:System.Windows.Data.MultiBinding" /> itself has a failing validation rule; otherwise, <see langword="false" />.</returns>
		// Token: 0x170006A1 RID: 1697
		// (get) Token: 0x06001C26 RID: 7206 RVA: 0x000842C4 File Offset: 0x000824C4
		public override bool HasError
		{
			get
			{
				bool hasError = base.HasError;
				if (!hasError)
				{
					for (int i = 0; i < this.MutableBindingExpressions.Count; i++)
					{
						if (this.MutableBindingExpressions[i].HasError)
						{
							return true;
						}
					}
				}
				return hasError;
			}
		}

		/// <summary>Gets a value that indicates whether the parent binding has a failed validation rule.</summary>
		/// <returns>
		///     <see langword="true" /> if the parent binding has a failed validation rule; otherwise, <see langword="false" />.</returns>
		// Token: 0x170006A2 RID: 1698
		// (get) Token: 0x06001C27 RID: 7207 RVA: 0x00084308 File Offset: 0x00082508
		public override bool HasValidationError
		{
			get
			{
				bool hasValidationError = base.HasValidationError;
				if (!hasValidationError)
				{
					for (int i = 0; i < this.MutableBindingExpressions.Count; i++)
					{
						if (this.MutableBindingExpressions[i].HasValidationError)
						{
							return true;
						}
					}
				}
				return hasValidationError;
			}
		}

		// Token: 0x06001C28 RID: 7208 RVA: 0x0008434C File Offset: 0x0008254C
		internal override bool AttachOverride(DependencyObject d, DependencyProperty dp)
		{
			if (!base.AttachOverride(d, dp))
			{
				return false;
			}
			DependencyObject targetElement = base.TargetElement;
			if (targetElement == null)
			{
				return false;
			}
			if (base.IsUpdateOnLostFocus)
			{
				LostFocusEventManager.AddHandler(targetElement, new EventHandler<RoutedEventArgs>(this.OnLostFocus));
			}
			base.TransferIsDeferred = true;
			int count = this.ParentMultiBinding.Bindings.Count;
			for (int i = 0; i < count; i++)
			{
				this.AttachBindingExpression(i, false);
			}
			this.AttachToContext(false);
			if (base.TransferIsDeferred)
			{
				base.Engine.AddTask(this, TaskOps.AttachToContext);
				if (TraceData.IsExtendedTraceEnabled(this, TraceDataLevel.Attach))
				{
					TraceData.Trace(TraceEventType.Warning, TraceData.DeferAttachToContext(new object[]
					{
						TraceData.Identify(this)
					}));
				}
			}
			return true;
		}

		// Token: 0x06001C29 RID: 7209 RVA: 0x000843FC File Offset: 0x000825FC
		internal override void DetachOverride()
		{
			DependencyObject targetElement = base.TargetElement;
			if (targetElement != null && base.IsUpdateOnLostFocus)
			{
				LostFocusEventManager.RemoveHandler(targetElement, new EventHandler<RoutedEventArgs>(this.OnLostFocus));
			}
			int count = this.MutableBindingExpressions.Count;
			for (int i = count - 1; i >= 0; i--)
			{
				BindingExpressionBase bindingExpressionBase = this.MutableBindingExpressions[i];
				if (bindingExpressionBase != null)
				{
					bindingExpressionBase.Detach();
					this.MutableBindingExpressions.RemoveAt(i);
				}
			}
			base.ChangeSources(null);
			base.DetachOverride();
		}

		// Token: 0x06001C2A RID: 7210 RVA: 0x00084478 File Offset: 0x00082678
		internal override void InvalidateChild(BindingExpressionBase bindingExpression)
		{
			int num = this.MutableBindingExpressions.IndexOf(bindingExpression);
			if (0 <= num && base.IsDynamic)
			{
				base.NeedsDataTransfer = true;
				this.Transfer();
			}
		}

		// Token: 0x06001C2B RID: 7211 RVA: 0x000844AC File Offset: 0x000826AC
		internal override void ChangeSourcesForChild(BindingExpressionBase bindingExpression, WeakDependencySource[] newSources)
		{
			int num = this.MutableBindingExpressions.IndexOf(bindingExpression);
			if (num >= 0)
			{
				WeakDependencySource[] commonSources = null;
				if (base.UsesLanguage)
				{
					commonSources = new WeakDependencySource[]
					{
						new WeakDependencySource(base.TargetElement, FrameworkElement.LanguageProperty)
					};
				}
				WeakDependencySource[] newSources2 = BindingExpressionBase.CombineSources(num, this.MutableBindingExpressions, this.MutableBindingExpressions.Count, newSources, commonSources);
				base.ChangeSources(newSources2);
			}
		}

		// Token: 0x06001C2C RID: 7212 RVA: 0x00084510 File Offset: 0x00082710
		internal override void ReplaceChild(BindingExpressionBase bindingExpression)
		{
			int num = this.MutableBindingExpressions.IndexOf(bindingExpression);
			DependencyObject targetElement = base.TargetElement;
			if (num >= 0 && targetElement != null)
			{
				bindingExpression.Detach();
				this.AttachBindingExpression(num, true);
			}
		}

		// Token: 0x06001C2D RID: 7213 RVA: 0x00084548 File Offset: 0x00082748
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

		// Token: 0x06001C2E RID: 7214 RVA: 0x00084584 File Offset: 0x00082784
		internal override object ConvertProposedValue(object value)
		{
			object unsetValue;
			if (!this.ConvertProposedValueImpl(value, out unsetValue))
			{
				unsetValue = DependencyProperty.UnsetValue;
				ValidationError validationError = new ValidationError(ConversionValidationRule.Instance, this, SR.Get("Validation_ConversionFailed", new object[]
				{
					value
				}), null);
				base.UpdateValidationError(validationError, false);
			}
			return unsetValue;
		}

		// Token: 0x06001C2F RID: 7215 RVA: 0x000845D0 File Offset: 0x000827D0
		private bool ConvertProposedValueImpl(object value, out object result)
		{
			DependencyObject targetElement = base.TargetElement;
			if (targetElement == null)
			{
				result = DependencyProperty.UnsetValue;
				return false;
			}
			result = this.GetValuesForChildBindings(value);
			if (base.IsDetached)
			{
				return false;
			}
			if (result == DependencyProperty.UnsetValue)
			{
				base.SetStatus(BindingStatusInternal.UpdateSourceError);
				return false;
			}
			object[] array = (object[])result;
			if (array == null)
			{
				if (TraceData.IsEnabled)
				{
					TraceData.Trace(TraceEventType.Error, TraceData.BadMultiConverterForUpdate(new object[]
					{
						this.Converter.GetType().Name,
						AvTrace.ToStringHelper(value),
						AvTrace.TypeName(value)
					}), this);
				}
				result = DependencyProperty.UnsetValue;
				return false;
			}
			if (TraceData.IsExtendedTraceEnabled(this, TraceDataLevel.Transfer))
			{
				for (int i = 0; i < array.Length; i++)
				{
					TraceData.Trace(TraceEventType.Warning, TraceData.UserConvertBackMulti(new object[]
					{
						TraceData.Identify(this),
						i,
						TraceData.Identify(array[i])
					}));
				}
			}
			int num = this.MutableBindingExpressions.Count;
			if (array.Length != num && TraceData.IsEnabled)
			{
				TraceData.Trace(TraceEventType.Information, TraceData.MultiValueConverterMismatch, new object[]
				{
					this.Converter.GetType().Name,
					num,
					array.Length,
					TraceData.DescribeTarget(targetElement, base.TargetProperty)
				});
			}
			if (array.Length < num)
			{
				num = array.Length;
			}
			bool result2 = true;
			for (int j = 0; j < num; j++)
			{
				value = array[j];
				if (value != Binding.DoNothing && value != DependencyProperty.UnsetValue)
				{
					BindingExpressionBase bindingExpressionBase = this.MutableBindingExpressions[j];
					bindingExpressionBase.SetValue(targetElement, base.TargetProperty, value);
					value = bindingExpressionBase.GetRawProposedValue();
					if (!bindingExpressionBase.Validate(value, ValidationStep.RawProposedValue))
					{
						value = DependencyProperty.UnsetValue;
					}
					value = bindingExpressionBase.ConvertProposedValue(value);
				}
				else if (value == DependencyProperty.UnsetValue && TraceData.IsEnabled)
				{
					TraceData.Trace(TraceEventType.Information, TraceData.UnsetValueInMultiBindingExpressionUpdate(new object[]
					{
						this.Converter.GetType().Name,
						AvTrace.ToStringHelper(value),
						j,
						this._tempTypes[j]
					}), this);
				}
				if (value == DependencyProperty.UnsetValue)
				{
					result2 = false;
				}
				array[j] = value;
			}
			Array.Clear(this._tempTypes, 0, this._tempTypes.Length);
			result = array;
			return result2;
		}

		// Token: 0x06001C30 RID: 7216 RVA: 0x0008480C File Offset: 0x00082A0C
		private object GetValuesForChildBindings(object rawValue)
		{
			if (this.Converter == null)
			{
				if (TraceData.IsEnabled)
				{
					TraceData.Trace(TraceEventType.Error, TraceData.MultiValueConverterMissingForUpdate, this);
				}
				return DependencyProperty.UnsetValue;
			}
			CultureInfo culture = base.GetCulture();
			int count = this.MutableBindingExpressions.Count;
			for (int i = 0; i < count; i++)
			{
				BindingExpressionBase bindingExpressionBase = this.MutableBindingExpressions[i];
				BindingExpression bindingExpression = bindingExpressionBase as BindingExpression;
				if (bindingExpression != null && bindingExpression.UseDefaultValueConverter)
				{
					this._tempTypes[i] = bindingExpression.ConverterSourceType;
				}
				else
				{
					this._tempTypes[i] = base.TargetProperty.PropertyType;
				}
			}
			return this.Converter.ConvertBack(rawValue, this._tempTypes, this.ParentMultiBinding.ConverterParameter, culture);
		}

		// Token: 0x06001C31 RID: 7217 RVA: 0x000848C0 File Offset: 0x00082AC0
		internal override bool ObtainConvertedProposedValue(BindingGroup bindingGroup)
		{
			bool result = true;
			if (base.NeedsUpdate)
			{
				object obj = bindingGroup.GetValue(this);
				if (obj != DependencyProperty.UnsetValue)
				{
					obj = this.ConvertProposedValue(obj);
					object[] array;
					if (obj == DependencyProperty.UnsetValue)
					{
						result = false;
					}
					else if ((array = (obj as object[])) != null)
					{
						for (int i = 0; i < array.Length; i++)
						{
							if (array[i] == DependencyProperty.UnsetValue)
							{
								result = false;
							}
						}
					}
				}
				this.StoreValueInBindingGroup(obj, bindingGroup);
			}
			else
			{
				bindingGroup.UseSourceValue(this);
			}
			return result;
		}

		// Token: 0x06001C32 RID: 7218 RVA: 0x00084934 File Offset: 0x00082B34
		internal override object UpdateSource(object convertedValue)
		{
			if (convertedValue == DependencyProperty.UnsetValue)
			{
				base.SetStatus(BindingStatusInternal.UpdateSourceError);
				return convertedValue;
			}
			object[] array = convertedValue as object[];
			int num = this.MutableBindingExpressions.Count;
			if (array.Length < num)
			{
				num = array.Length;
			}
			base.BeginSourceUpdate();
			bool flag = false;
			for (int i = 0; i < num; i++)
			{
				object obj = array[i];
				if (obj != Binding.DoNothing)
				{
					BindingExpressionBase bindingExpressionBase = this.MutableBindingExpressions[i];
					bindingExpressionBase.UpdateSource(obj);
					if (bindingExpressionBase.StatusInternal == BindingStatusInternal.UpdateSourceError)
					{
						base.SetStatus(BindingStatusInternal.UpdateSourceError);
					}
					flag = true;
				}
			}
			if (!flag)
			{
				base.IsInUpdate = false;
			}
			base.EndSourceUpdate();
			this.OnSourceUpdated();
			return convertedValue;
		}

		// Token: 0x06001C33 RID: 7219 RVA: 0x000849D4 File Offset: 0x00082BD4
		internal override bool UpdateSource(BindingGroup bindingGroup)
		{
			bool result = true;
			if (base.NeedsUpdate)
			{
				object value = bindingGroup.GetValue(this);
				this.UpdateSource(value);
				if (value == DependencyProperty.UnsetValue)
				{
					result = false;
				}
			}
			return result;
		}

		// Token: 0x06001C34 RID: 7220 RVA: 0x00084A08 File Offset: 0x00082C08
		internal override void StoreValueInBindingGroup(object value, BindingGroup bindingGroup)
		{
			bindingGroup.SetValue(this, value);
			object[] array = value as object[];
			if (array != null)
			{
				int num = this.MutableBindingExpressions.Count;
				if (array.Length < num)
				{
					num = array.Length;
				}
				for (int i = 0; i < num; i++)
				{
					this.MutableBindingExpressions[i].StoreValueInBindingGroup(array[i], bindingGroup);
				}
				return;
			}
			for (int j = this.MutableBindingExpressions.Count - 1; j >= 0; j--)
			{
				this.MutableBindingExpressions[j].StoreValueInBindingGroup(DependencyProperty.UnsetValue, bindingGroup);
			}
		}

		// Token: 0x06001C35 RID: 7221 RVA: 0x00084A90 File Offset: 0x00082C90
		internal override bool Validate(object value, ValidationStep validationStep)
		{
			if (value == Binding.DoNothing)
			{
				return true;
			}
			if (value == DependencyProperty.UnsetValue)
			{
				base.SetStatus(BindingStatusInternal.UpdateSourceError);
				return false;
			}
			bool result = base.Validate(value, validationStep);
			if (validationStep != ValidationStep.RawProposedValue)
			{
				object[] array = value as object[];
				int num = this.MutableBindingExpressions.Count;
				if (array.Length < num)
				{
					num = array.Length;
				}
				for (int i = 0; i < num; i++)
				{
					value = array[i];
					if (value != DependencyProperty.UnsetValue && value != Binding.DoNothing && !this.MutableBindingExpressions[i].Validate(value, validationStep))
					{
						array[i] = DependencyProperty.UnsetValue;
					}
				}
			}
			return result;
		}

		// Token: 0x06001C36 RID: 7222 RVA: 0x00084B20 File Offset: 0x00082D20
		internal override bool CheckValidationRules(BindingGroup bindingGroup, ValidationStep validationStep)
		{
			if (!base.NeedsValidation)
			{
				return true;
			}
			if (validationStep <= ValidationStep.CommittedValue)
			{
				object value = bindingGroup.GetValue(this);
				bool flag = this.Validate(value, validationStep);
				if (flag && validationStep == ValidationStep.CommittedValue)
				{
					base.NeedsValidation = false;
				}
				return flag;
			}
			throw new InvalidOperationException(SR.Get("ValidationRule_UnknownStep", new object[]
			{
				validationStep,
				bindingGroup
			}));
		}

		// Token: 0x06001C37 RID: 7223 RVA: 0x00084B80 File Offset: 0x00082D80
		internal override bool ValidateAndConvertProposedValue(out Collection<BindingExpressionBase.ProposedValue> values)
		{
			values = null;
			object rawProposedValue = this.GetRawProposedValue();
			if (!this.Validate(rawProposedValue, ValidationStep.RawProposedValue))
			{
				return false;
			}
			object valuesForChildBindings = this.GetValuesForChildBindings(rawProposedValue);
			if (base.IsDetached || valuesForChildBindings == DependencyProperty.UnsetValue || valuesForChildBindings == null)
			{
				return false;
			}
			int num = this.MutableBindingExpressions.Count;
			object[] array = (object[])valuesForChildBindings;
			if (array.Length < num)
			{
				num = array.Length;
			}
			values = new Collection<BindingExpressionBase.ProposedValue>();
			bool flag = true;
			for (int i = 0; i < num; i++)
			{
				object obj = array[i];
				if (obj != Binding.DoNothing)
				{
					if (obj == DependencyProperty.UnsetValue)
					{
						flag = false;
					}
					else
					{
						BindingExpressionBase bindingExpressionBase = this.MutableBindingExpressions[i];
						bindingExpressionBase.Value = obj;
						if (bindingExpressionBase.NeedsValidation)
						{
							Collection<BindingExpressionBase.ProposedValue> collection;
							bool flag2 = bindingExpressionBase.ValidateAndConvertProposedValue(out collection);
							if (collection != null)
							{
								int j = 0;
								int count = collection.Count;
								while (j < count)
								{
									values.Add(collection[j]);
									j++;
								}
							}
							flag = (flag && flag2);
						}
					}
				}
			}
			return flag;
		}

		// Token: 0x06001C38 RID: 7224 RVA: 0x00084C84 File Offset: 0x00082E84
		internal override object GetSourceItem(object newValue)
		{
			if (newValue == null)
			{
				return null;
			}
			int count = this.MutableBindingExpressions.Count;
			for (int i = 0; i < count; i++)
			{
				object value = this.MutableBindingExpressions[i].GetValue(null, null);
				if (ItemsControl.EqualsEx(value, newValue))
				{
					return this.MutableBindingExpressions[i].GetSourceItem(newValue);
				}
			}
			return null;
		}

		// Token: 0x170006A3 RID: 1699
		// (get) Token: 0x06001C39 RID: 7225 RVA: 0x00084CDF File Offset: 0x00082EDF
		private Collection<BindingExpressionBase> MutableBindingExpressions
		{
			get
			{
				return this._list;
			}
		}

		// Token: 0x170006A4 RID: 1700
		// (get) Token: 0x06001C3A RID: 7226 RVA: 0x00084CE7 File Offset: 0x00082EE7
		// (set) Token: 0x06001C3B RID: 7227 RVA: 0x00084CEF File Offset: 0x00082EEF
		private IMultiValueConverter Converter
		{
			get
			{
				return this._converter;
			}
			set
			{
				this._converter = value;
			}
		}

		// Token: 0x06001C3C RID: 7228 RVA: 0x00084CF8 File Offset: 0x00082EF8
		private BindingExpressionBase AttachBindingExpression(int i, bool replaceExisting)
		{
			DependencyObject targetElement = base.TargetElement;
			if (targetElement == null)
			{
				return null;
			}
			BindingBase bindingBase = this.ParentMultiBinding.Bindings[i];
			MultiBinding.CheckTrigger(bindingBase);
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

		// Token: 0x06001C3D RID: 7229 RVA: 0x00084D64 File Offset: 0x00082F64
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
			bool flag = true;
			base.TransferIsDeferred = true;
			if (base.UsesLanguage && d == base.TargetElement && property == FrameworkElement.LanguageProperty)
			{
				base.InvalidateCulture();
				base.NeedsDataTransfer = true;
			}
			if (base.IsDetached)
			{
				return;
			}
			int count = this.MutableBindingExpressions.Count;
			for (int i = 0; i < count; i++)
			{
				BindingExpressionBase bindingExpressionBase = this.MutableBindingExpressions[i];
				if (bindingExpressionBase != null)
				{
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
					if (bindingExpressionBase.IsDisconnected)
					{
						flag = false;
					}
				}
			}
			base.TransferIsDeferred = false;
			if (flag)
			{
				this.Transfer();
				return;
			}
			this.Disconnect();
		}

		// Token: 0x06001C3E RID: 7230 RVA: 0x0000B02A File Offset: 0x0000922A
		internal override bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
		{
			return false;
		}

		// Token: 0x06001C3F RID: 7231 RVA: 0x00076FF2 File Offset: 0x000751F2
		internal override void OnLostFocus(object sender, RoutedEventArgs e)
		{
			if (TraceData.IsExtendedTraceEnabled(this, TraceDataLevel.Transfer))
			{
				TraceData.Trace(TraceEventType.Warning, TraceData.GotEvent(new object[]
				{
					TraceData.Identify(this),
					"LostFocus",
					TraceData.Identify(sender)
				}));
			}
			base.Update();
		}

		// Token: 0x06001C40 RID: 7232 RVA: 0x00084E78 File Offset: 0x00083078
		private void UpdateTarget(bool includeInnerBindings)
		{
			base.TransferIsDeferred = true;
			if (includeInnerBindings)
			{
				foreach (BindingExpressionBase bindingExpressionBase in this.MutableBindingExpressions)
				{
					bindingExpressionBase.UpdateTarget();
				}
			}
			base.TransferIsDeferred = false;
			base.NeedsDataTransfer = true;
			this.Transfer();
			base.NeedsUpdate = false;
		}

		// Token: 0x06001C41 RID: 7233 RVA: 0x00084EEC File Offset: 0x000830EC
		private void Transfer()
		{
			if (base.NeedsDataTransfer && base.StatusInternal != BindingStatusInternal.Unattached && !base.TransferIsDeferred)
			{
				this.TransferValue();
			}
		}

		// Token: 0x06001C42 RID: 7234 RVA: 0x00084F0C File Offset: 0x0008310C
		private void TransferValue()
		{
			base.IsInTransfer = true;
			base.NeedsDataTransfer = false;
			DependencyObject targetElement = base.TargetElement;
			if (targetElement != null)
			{
				bool flag = TraceData.IsExtendedTraceEnabled(this, TraceDataLevel.Transfer);
				object obj = DependencyProperty.UnsetValue;
				object obj2 = this._tempValues;
				CultureInfo culture = base.GetCulture();
				int count = this.MutableBindingExpressions.Count;
				for (int i = 0; i < count; i++)
				{
					this._tempValues[i] = this.MutableBindingExpressions[i].GetValue(targetElement, base.TargetProperty);
					if (flag)
					{
						TraceData.Trace(TraceEventType.Warning, TraceData.GetRawValueMulti(new object[]
						{
							TraceData.Identify(this),
							i,
							TraceData.Identify(this._tempValues[i])
						}));
					}
				}
				if (this.Converter != null)
				{
					obj2 = this.Converter.Convert(this._tempValues, base.TargetProperty.PropertyType, this.ParentMultiBinding.ConverterParameter, culture);
					if (base.IsDetached)
					{
						return;
					}
					if (flag)
					{
						TraceData.Trace(TraceEventType.Warning, TraceData.UserConverter(new object[]
						{
							TraceData.Identify(this),
							TraceData.Identify(obj2)
						}));
					}
				}
				else if (base.EffectiveStringFormat != null)
				{
					for (int j = 0; j < this._tempValues.Length; j++)
					{
						if (this._tempValues[j] == DependencyProperty.UnsetValue)
						{
							obj2 = DependencyProperty.UnsetValue;
							break;
						}
					}
				}
				else
				{
					if (TraceData.IsEnabled)
					{
						TraceData.Trace(TraceEventType.Error, TraceData.MultiValueConverterMissingForTransfer, this);
						goto IL_371;
					}
					goto IL_371;
				}
				if (base.EffectiveStringFormat == null || obj2 == Binding.DoNothing || obj2 == DependencyProperty.UnsetValue)
				{
					obj = obj2;
				}
				else
				{
					try
					{
						if (obj2 == this._tempValues)
						{
							obj = string.Format(culture, base.EffectiveStringFormat, this._tempValues);
						}
						else
						{
							obj = string.Format(culture, base.EffectiveStringFormat, new object[]
							{
								obj2
							});
						}
						if (flag)
						{
							TraceData.Trace(TraceEventType.Warning, TraceData.FormattedValue(new object[]
							{
								TraceData.Identify(this),
								TraceData.Identify(obj)
							}));
						}
					}
					catch (FormatException)
					{
						obj = DependencyProperty.UnsetValue;
						if (flag)
						{
							TraceData.Trace(TraceEventType.Warning, TraceData.FormattingFailed(new object[]
							{
								TraceData.Identify(this),
								base.EffectiveStringFormat
							}));
						}
					}
				}
				Array.Clear(this._tempValues, 0, this._tempValues.Length);
				if (obj != Binding.DoNothing)
				{
					if (base.EffectiveTargetNullValue != DependencyProperty.UnsetValue && BindingExpressionBase.IsNullValue(obj))
					{
						obj = base.EffectiveTargetNullValue;
						if (flag)
						{
							TraceData.Trace(TraceEventType.Warning, TraceData.NullConverter(new object[]
							{
								TraceData.Identify(this),
								TraceData.Identify(obj)
							}));
						}
					}
					if (obj != DependencyProperty.UnsetValue && !base.TargetProperty.IsValidValue(obj))
					{
						if (TraceData.IsEnabled)
						{
							TraceData.Trace(base.TraceLevel, TraceData.BadValueAtTransfer, obj, this);
						}
						if (flag)
						{
							TraceData.Trace(TraceEventType.Warning, TraceData.BadValueAtTransferExtended(new object[]
							{
								TraceData.Identify(this),
								TraceData.Identify(obj)
							}));
						}
						obj = DependencyProperty.UnsetValue;
					}
					if (obj == DependencyProperty.UnsetValue)
					{
						obj = base.UseFallbackValue();
						if (flag)
						{
							TraceData.Trace(TraceEventType.Warning, TraceData.UseFallback(new object[]
							{
								TraceData.Identify(this),
								TraceData.Identify(obj)
							}));
						}
					}
					if (flag)
					{
						TraceData.Trace(TraceEventType.Warning, TraceData.TransferValue(new object[]
						{
							TraceData.Identify(this),
							TraceData.Identify(obj)
						}));
					}
					bool flag2 = !base.IsInUpdate || !ItemsControl.EqualsEx(obj, base.Value);
					if (flag2)
					{
						base.ChangeValue(obj, true);
						base.Invalidate(false);
						Validation.ClearInvalid(this);
					}
					base.Clean();
					if (flag2)
					{
						this.OnTargetUpdated();
					}
				}
			}
			IL_371:
			base.IsInTransfer = false;
		}

		// Token: 0x06001C43 RID: 7235 RVA: 0x000852A4 File Offset: 0x000834A4
		private void OnTargetUpdated()
		{
			if (base.NotifyOnTargetUpdated)
			{
				DependencyObject targetElement = base.TargetElement;
				if (targetElement != null)
				{
					if (base.IsAttaching && this == targetElement.ReadLocalValue(base.TargetProperty))
					{
						base.Engine.AddTask(this, TaskOps.RaiseTargetUpdatedEvent);
						return;
					}
					BindingExpression.OnTargetUpdated(targetElement, base.TargetProperty);
				}
			}
		}

		// Token: 0x06001C44 RID: 7236 RVA: 0x000852F4 File Offset: 0x000834F4
		private void OnSourceUpdated()
		{
			if (base.NotifyOnSourceUpdated)
			{
				DependencyObject targetElement = base.TargetElement;
				if (targetElement != null)
				{
					BindingExpression.OnSourceUpdated(targetElement, base.TargetProperty);
				}
			}
		}

		// Token: 0x06001C45 RID: 7237 RVA: 0x00085320 File Offset: 0x00083520
		internal override bool ShouldReactToDirtyOverride()
		{
			foreach (BindingExpressionBase bindingExpressionBase in this.MutableBindingExpressions)
			{
				if (!bindingExpressionBase.ShouldReactToDirtyOverride())
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001C46 RID: 7238 RVA: 0x00085378 File Offset: 0x00083578
		internal override bool UpdateOverride()
		{
			return !base.NeedsUpdate || !base.IsReflective || base.IsInTransfer || base.StatusInternal == BindingStatusInternal.Unattached || base.UpdateValue();
		}

		// Token: 0x040013C0 RID: 5056
		private Collection<BindingExpressionBase> _list = new Collection<BindingExpressionBase>();

		// Token: 0x040013C1 RID: 5057
		private IMultiValueConverter _converter;

		// Token: 0x040013C2 RID: 5058
		private object[] _tempValues;

		// Token: 0x040013C3 RID: 5059
		private Type[] _tempTypes;
	}
}
