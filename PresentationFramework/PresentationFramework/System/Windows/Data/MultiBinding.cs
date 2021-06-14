using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Markup;
using MS.Internal.Controls;
using MS.Internal.Data;

namespace System.Windows.Data
{
	/// <summary>Describes a collection of <see cref="T:System.Windows.Data.Binding" /> objects attached to a single binding target property.</summary>
	// Token: 0x020001B4 RID: 436
	[ContentProperty("Bindings")]
	public class MultiBinding : BindingBase, IAddChild
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Data.MultiBinding" /> class.</summary>
		// Token: 0x06001BEE RID: 7150 RVA: 0x00083BB8 File Offset: 0x00081DB8
		public MultiBinding()
		{
			this._bindingCollection = new BindingCollection(this, new BindingCollectionChangedCallback(this.OnBindingCollectionChanged));
		}

		/// <summary>Adds a child object.</summary>
		/// <param name="value">The child object to add. </param>
		// Token: 0x06001BEF RID: 7151 RVA: 0x00083BD8 File Offset: 0x00081DD8
		void IAddChild.AddChild(object value)
		{
			BindingBase bindingBase = value as BindingBase;
			if (bindingBase != null)
			{
				this.Bindings.Add(bindingBase);
				return;
			}
			throw new ArgumentException(SR.Get("ChildHasWrongType", new object[]
			{
				base.GetType().Name,
				"BindingBase",
				value.GetType().FullName
			}), "value");
		}

		/// <summary>Adds the text content of a node to the object. </summary>
		/// <param name="text">The text to add to the object.</param>
		// Token: 0x06001BF0 RID: 7152 RVA: 0x0000B31C File Offset: 0x0000951C
		void IAddChild.AddText(string text)
		{
			XamlSerializerUtil.ThrowIfNonWhiteSpaceInAddText(text, this);
		}

		/// <summary>Gets the collection of <see cref="T:System.Windows.Data.Binding" /> objects within this <see cref="T:System.Windows.Data.MultiBinding" /> instance.</summary>
		/// <returns>A collection of <see cref="T:System.Windows.Data.Binding" /> objects. <see cref="T:System.Windows.Data.MultiBinding" /> currently supports only objects of type <see cref="T:System.Windows.Data.Binding" /> and not <see cref="T:System.Windows.Data.MultiBinding" /> or <see cref="T:System.Windows.Data.PriorityBinding" />. Adding a <see cref="T:System.Windows.Data.Binding" /> child to a <see cref="T:System.Windows.Data.MultiBinding" /> object implicitly adds the child to the <see cref="T:System.Windows.Data.BindingBase" /> collection for the <see cref="T:System.Windows.Data.MultiBinding" /> object.</returns>
		// Token: 0x1700068B RID: 1675
		// (get) Token: 0x06001BF1 RID: 7153 RVA: 0x00083C3A File Offset: 0x00081E3A
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public Collection<BindingBase> Bindings
		{
			get
			{
				return this._bindingCollection;
			}
		}

		/// <summary>Indicates whether the <see cref="P:System.Windows.Data.MultiBinding.Bindings" /> property should be persisted.</summary>
		/// <returns>
		///     <see langword="true" /> if the property value has changed from its default; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001BF2 RID: 7154 RVA: 0x00083C42 File Offset: 0x00081E42
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeBindings()
		{
			return this.Bindings != null && this.Bindings.Count > 0;
		}

		/// <summary>Gets or sets a value that indicates the direction of the data flow of this binding.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Data.BindingMode" /> values. The default value is <see cref="F:System.Windows.Data.BindingMode.Default" />, which returns the default binding mode value of the target dependency property. However, the default value varies for each dependency property. In general, user-editable control properties, such as <see cref="P:System.Windows.Controls.TextBox.Text" />, default to two-way bindings, whereas most other properties default to one-way bindings.A programmatic way to determine whether a dependency property binds one-way or two-way by default is to get the property metadata of the property using <see cref="M:System.Windows.DependencyProperty.GetMetadata(System.Type)" /> and then check the Boolean value of the <see cref="P:System.Windows.FrameworkPropertyMetadata.BindsTwoWayByDefault" /> property.</returns>
		// Token: 0x1700068C RID: 1676
		// (get) Token: 0x06001BF3 RID: 7155 RVA: 0x00083C5C File Offset: 0x00081E5C
		// (set) Token: 0x06001BF4 RID: 7156 RVA: 0x00083C98 File Offset: 0x00081E98
		[DefaultValue(BindingMode.Default)]
		public BindingMode Mode
		{
			get
			{
				switch (base.GetFlagsWithinMask(BindingBase.BindingFlags.PropagationMask))
				{
				case BindingBase.BindingFlags.OneTime:
					return BindingMode.OneTime;
				case BindingBase.BindingFlags.OneWay:
					return BindingMode.OneWay;
				case BindingBase.BindingFlags.OneWayToSource:
					return BindingMode.OneWayToSource;
				case BindingBase.BindingFlags.TwoWay:
					return BindingMode.TwoWay;
				case BindingBase.BindingFlags.PropDefault:
					return BindingMode.Default;
				default:
					return BindingMode.TwoWay;
				}
			}
			set
			{
				base.CheckSealed();
				base.ChangeFlagsWithinMask(BindingBase.BindingFlags.PropagationMask, BindingBase.FlagsFrom(value));
			}
		}

		/// <summary>Gets or sets a value that determines the timing of binding source updates.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Data.UpdateSourceTrigger" /> values. The default value is <see cref="F:System.Windows.Data.UpdateSourceTrigger.Default" />, which returns the default <see cref="T:System.Windows.Data.UpdateSourceTrigger" /> value of the target dependency property. However, the default value for most dependency properties is <see cref="F:System.Windows.Data.UpdateSourceTrigger.PropertyChanged" />, while the <see cref="P:System.Windows.Controls.TextBox.Text" /> property has a default value of <see cref="F:System.Windows.Data.UpdateSourceTrigger.LostFocus" />.A programmatic way to determine the default <see cref="P:System.Windows.Data.Binding.UpdateSourceTrigger" /> value of a dependency property is to get the property metadata of the property using <see cref="M:System.Windows.DependencyProperty.GetMetadata(System.Type)" /> and then check the value of the <see cref="P:System.Windows.FrameworkPropertyMetadata.DefaultUpdateSourceTrigger" /> property.</returns>
		// Token: 0x1700068D RID: 1677
		// (get) Token: 0x06001BF5 RID: 7157 RVA: 0x00083CB0 File Offset: 0x00081EB0
		// (set) Token: 0x06001BF6 RID: 7158 RVA: 0x00083CF9 File Offset: 0x00081EF9
		[DefaultValue(UpdateSourceTrigger.PropertyChanged)]
		public UpdateSourceTrigger UpdateSourceTrigger
		{
			get
			{
				BindingBase.BindingFlags flagsWithinMask = base.GetFlagsWithinMask(BindingBase.BindingFlags.UpdateDefault);
				if (flagsWithinMask <= BindingBase.BindingFlags.UpdateOnLostFocus)
				{
					if (flagsWithinMask == BindingBase.BindingFlags.OneTime)
					{
						return UpdateSourceTrigger.PropertyChanged;
					}
					if (flagsWithinMask == BindingBase.BindingFlags.UpdateOnLostFocus)
					{
						return UpdateSourceTrigger.LostFocus;
					}
				}
				else
				{
					if (flagsWithinMask == BindingBase.BindingFlags.UpdateExplicitly)
					{
						return UpdateSourceTrigger.Explicit;
					}
					if (flagsWithinMask == BindingBase.BindingFlags.UpdateDefault)
					{
						return UpdateSourceTrigger.Default;
					}
				}
				return UpdateSourceTrigger.Default;
			}
			set
			{
				base.CheckSealed();
				base.ChangeFlagsWithinMask(BindingBase.BindingFlags.UpdateDefault, BindingBase.FlagsFrom(value));
			}
		}

		/// <summary>Gets or sets a value that indicates whether to raise the <see cref="E:System.Windows.FrameworkElement.SourceUpdated" /> event when a value is transferred from the binding target to the binding source.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="E:System.Windows.FrameworkElement.SourceUpdated" /> event will be raised when the binding source value is updated; otherwise, <see langword="false" />. The default value is <see langword="false" />.</returns>
		// Token: 0x1700068E RID: 1678
		// (get) Token: 0x06001BF7 RID: 7159 RVA: 0x0007427C File Offset: 0x0007247C
		// (set) Token: 0x06001BF8 RID: 7160 RVA: 0x00083D14 File Offset: 0x00081F14
		[DefaultValue(false)]
		public bool NotifyOnSourceUpdated
		{
			get
			{
				return base.TestFlag(BindingBase.BindingFlags.NotifyOnSourceUpdated);
			}
			set
			{
				bool flag = base.TestFlag(BindingBase.BindingFlags.NotifyOnSourceUpdated);
				if (flag != value)
				{
					base.CheckSealed();
					base.ChangeFlag(BindingBase.BindingFlags.NotifyOnSourceUpdated, value);
				}
			}
		}

		/// <summary>Gets or sets a value that indicates whether to raise the <see cref="E:System.Windows.FrameworkElement.TargetUpdated" /> event when a value is transferred from the binding source to the binding target.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="E:System.Windows.FrameworkElement.TargetUpdated" /> event will be raised when the binding target value is updated; otherwise, <see langword="false" />. The default value is <see langword="false" />.</returns>
		// Token: 0x1700068F RID: 1679
		// (get) Token: 0x06001BF9 RID: 7161 RVA: 0x000742BB File Offset: 0x000724BB
		// (set) Token: 0x06001BFA RID: 7162 RVA: 0x00083D44 File Offset: 0x00081F44
		[DefaultValue(false)]
		public bool NotifyOnTargetUpdated
		{
			get
			{
				return base.TestFlag(BindingBase.BindingFlags.NotifyOnTargetUpdated);
			}
			set
			{
				bool flag = base.TestFlag(BindingBase.BindingFlags.NotifyOnTargetUpdated);
				if (flag != value)
				{
					base.CheckSealed();
					base.ChangeFlag(BindingBase.BindingFlags.NotifyOnTargetUpdated, value);
				}
			}
		}

		/// <summary>Gets or sets a value that indicates whether to raise the <see cref="E:System.Windows.Controls.Validation.Error" /> attached event on the bound element.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="E:System.Windows.Controls.Validation.Error" /> attached event will be raised on the bound element when there is a validation error during source updates; otherwise, <see langword="false" />. The default value is <see langword="false" />.</returns>
		// Token: 0x17000690 RID: 1680
		// (get) Token: 0x06001BFB RID: 7163 RVA: 0x000742EB File Offset: 0x000724EB
		// (set) Token: 0x06001BFC RID: 7164 RVA: 0x00083D6C File Offset: 0x00081F6C
		[DefaultValue(false)]
		public bool NotifyOnValidationError
		{
			get
			{
				return base.TestFlag(BindingBase.BindingFlags.NotifyOnValidationError);
			}
			set
			{
				bool flag = base.TestFlag(BindingBase.BindingFlags.NotifyOnValidationError);
				if (flag != value)
				{
					base.CheckSealed();
					base.ChangeFlag(BindingBase.BindingFlags.NotifyOnValidationError, value);
				}
			}
		}

		/// <summary>Gets or sets the converter to use to convert the source values to or from the target value.</summary>
		/// <returns>A value of type <see cref="T:System.Windows.Data.IMultiValueConverter" /> that indicates the converter to use. The default value is <see langword="null" />.</returns>
		// Token: 0x17000691 RID: 1681
		// (get) Token: 0x06001BFD RID: 7165 RVA: 0x00083D9B File Offset: 0x00081F9B
		// (set) Token: 0x06001BFE RID: 7166 RVA: 0x00074337 File Offset: 0x00072537
		[DefaultValue(null)]
		public IMultiValueConverter Converter
		{
			get
			{
				return (IMultiValueConverter)base.GetValue(BindingBase.Feature.Converter, null);
			}
			set
			{
				base.CheckSealed();
				base.SetValue(BindingBase.Feature.Converter, value, null);
			}
		}

		/// <summary>Gets or sets an optional parameter to pass to a converter as additional information.</summary>
		/// <returns>A parameter to pass to a converter. The default value is <see langword="null" />.</returns>
		// Token: 0x17000692 RID: 1682
		// (get) Token: 0x06001BFF RID: 7167 RVA: 0x00074349 File Offset: 0x00072549
		// (set) Token: 0x06001C00 RID: 7168 RVA: 0x00074354 File Offset: 0x00072554
		[DefaultValue(null)]
		public object ConverterParameter
		{
			get
			{
				return base.GetValue(BindingBase.Feature.ConverterParameter, null);
			}
			set
			{
				base.CheckSealed();
				base.SetValue(BindingBase.Feature.ConverterParameter, value, null);
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Globalization.CultureInfo" /> object that applies to any converter assigned to bindings wrapped by the <see cref="T:System.Windows.Data.MultiBinding" /> or on the <see cref="T:System.Windows.Data.MultiBinding" /> itself.</summary>
		/// <returns>A valid <see cref="T:System.Globalization.CultureInfo" />.</returns>
		// Token: 0x17000693 RID: 1683
		// (get) Token: 0x06001C01 RID: 7169 RVA: 0x00074366 File Offset: 0x00072566
		// (set) Token: 0x06001C02 RID: 7170 RVA: 0x00074375 File Offset: 0x00072575
		[DefaultValue(null)]
		[TypeConverter(typeof(CultureInfoIetfLanguageTagConverter))]
		public CultureInfo ConverterCulture
		{
			get
			{
				return (CultureInfo)base.GetValue(BindingBase.Feature.Culture, null);
			}
			set
			{
				base.CheckSealed();
				base.SetValue(BindingBase.Feature.Culture, value, null);
			}
		}

		/// <summary>Gets the collection of <see cref="T:System.Windows.Controls.ValidationRule" /> objects for this instance of <see cref="T:System.Windows.Data.MultiBinding" />.</summary>
		/// <returns>The collection of <see cref="T:System.Windows.Controls.ValidationRule" /> objects for this instance of <see cref="T:System.Windows.Data.MultiBinding" />.</returns>
		// Token: 0x17000694 RID: 1684
		// (get) Token: 0x06001C03 RID: 7171 RVA: 0x00073F8D File Offset: 0x0007218D
		public Collection<ValidationRule> ValidationRules
		{
			get
			{
				if (!base.HasValue(BindingBase.Feature.ValidationRules))
				{
					base.SetValue(BindingBase.Feature.ValidationRules, new ValidationRuleCollection());
				}
				return (ValidationRuleCollection)base.GetValue(BindingBase.Feature.ValidationRules, null);
			}
		}

		/// <summary>Indicates whether the <see cref="P:System.Windows.Data.MultiBinding.ValidationRules" /> property should be persisted.</summary>
		/// <returns>
		///     <see langword="true" /> if the property value has changed from its default; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001C04 RID: 7172 RVA: 0x00083DAB File Offset: 0x00081FAB
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeValidationRules()
		{
			return base.HasValue(BindingBase.Feature.ValidationRules) && this.ValidationRules.Count > 0;
		}

		/// <summary>Gets or sets a handler you can use to provide custom logic for handling exceptions that the binding engine encounters during the update of the binding source value. This is only applicable if you have associated the <see cref="T:System.Windows.Controls.ExceptionValidationRule" /> with your <see cref="T:System.Windows.Data.MultiBinding" /> object.</summary>
		/// <returns>A method that provides custom logic for handling exceptions that the binding engine encounters during the update of the binding source value.</returns>
		// Token: 0x17000695 RID: 1685
		// (get) Token: 0x06001C05 RID: 7173 RVA: 0x00074582 File Offset: 0x00072782
		// (set) Token: 0x06001C06 RID: 7174 RVA: 0x00074592 File Offset: 0x00072792
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public UpdateSourceExceptionFilterCallback UpdateSourceExceptionFilter
		{
			get
			{
				return (UpdateSourceExceptionFilterCallback)base.GetValue(BindingBase.Feature.ExceptionFilterCallback, null);
			}
			set
			{
				base.SetValue(BindingBase.Feature.ExceptionFilterCallback, value, null);
			}
		}

		/// <summary>Gets or sets a value that indicates whether to include the <see cref="T:System.Windows.Controls.ExceptionValidationRule" />.</summary>
		/// <returns>
		///     <see langword="true" /> to include the <see cref="T:System.Windows.Controls.ExceptionValidationRule" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000696 RID: 1686
		// (get) Token: 0x06001C07 RID: 7175 RVA: 0x00073FD0 File Offset: 0x000721D0
		// (set) Token: 0x06001C08 RID: 7176 RVA: 0x00083DC8 File Offset: 0x00081FC8
		[DefaultValue(false)]
		public bool ValidatesOnExceptions
		{
			get
			{
				return base.TestFlag(BindingBase.BindingFlags.ValidatesOnExceptions);
			}
			set
			{
				bool flag = base.TestFlag(BindingBase.BindingFlags.ValidatesOnExceptions);
				if (flag != value)
				{
					base.CheckSealed();
					base.ChangeFlag(BindingBase.BindingFlags.ValidatesOnExceptions, value);
				}
			}
		}

		/// <summary>Gets or sets a value that indicates whether to include the <see cref="T:System.Windows.Controls.DataErrorValidationRule" />.</summary>
		/// <returns>
		///     <see langword="true" /> to include the <see cref="T:System.Windows.Controls.DataErrorValidationRule" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000697 RID: 1687
		// (get) Token: 0x06001C09 RID: 7177 RVA: 0x0007400F File Offset: 0x0007220F
		// (set) Token: 0x06001C0A RID: 7178 RVA: 0x00083DF8 File Offset: 0x00081FF8
		[DefaultValue(false)]
		public bool ValidatesOnDataErrors
		{
			get
			{
				return base.TestFlag(BindingBase.BindingFlags.ValidatesOnDataErrors);
			}
			set
			{
				bool flag = base.TestFlag(BindingBase.BindingFlags.ValidatesOnDataErrors);
				if (flag != value)
				{
					base.CheckSealed();
					base.ChangeFlag(BindingBase.BindingFlags.ValidatesOnDataErrors, value);
				}
			}
		}

		/// <summary>Gets or sets a value that indicates whether to include the <see cref="T:System.Windows.Controls.NotifyDataErrorValidationRule" />.</summary>
		/// <returns>
		///     <see langword="true" /> to include the <see cref="T:System.Windows.Controls.NotifyDataErrorValidationRule" />; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17000698 RID: 1688
		// (get) Token: 0x06001C0B RID: 7179 RVA: 0x0007404B File Offset: 0x0007224B
		// (set) Token: 0x06001C0C RID: 7180 RVA: 0x00083E28 File Offset: 0x00082028
		[DefaultValue(true)]
		public bool ValidatesOnNotifyDataErrors
		{
			get
			{
				return base.TestFlag(BindingBase.BindingFlags.ValidatesOnNotifyDataErrors);
			}
			set
			{
				bool flag = base.TestFlag(BindingBase.BindingFlags.ValidatesOnNotifyDataErrors);
				if (flag != value)
				{
					base.CheckSealed();
					base.ChangeFlag(BindingBase.BindingFlags.ValidatesOnNotifyDataErrors, value);
				}
			}
		}

		// Token: 0x06001C0D RID: 7181 RVA: 0x00083E58 File Offset: 0x00082058
		internal override BindingExpressionBase CreateBindingExpressionOverride(DependencyObject target, DependencyProperty dp, BindingExpressionBase owner)
		{
			if (this.Converter == null && string.IsNullOrEmpty(base.StringFormat))
			{
				throw new InvalidOperationException(SR.Get("MultiBindingHasNoConverter"));
			}
			for (int i = 0; i < this.Bindings.Count; i++)
			{
				MultiBinding.CheckTrigger(this.Bindings[i]);
			}
			return MultiBindingExpression.CreateBindingExpression(target, dp, this, owner);
		}

		// Token: 0x06001C0E RID: 7182 RVA: 0x000745A9 File Offset: 0x000727A9
		internal override ValidationRule LookupValidationRule(Type type)
		{
			return BindingBase.LookupValidationRule(type, this.ValidationRulesInternal);
		}

		// Token: 0x06001C0F RID: 7183 RVA: 0x00083EBC File Offset: 0x000820BC
		internal object DoFilterException(object bindExpr, Exception exception)
		{
			UpdateSourceExceptionFilterCallback updateSourceExceptionFilterCallback = (UpdateSourceExceptionFilterCallback)base.GetValue(BindingBase.Feature.ExceptionFilterCallback, null);
			if (updateSourceExceptionFilterCallback != null)
			{
				return updateSourceExceptionFilterCallback(bindExpr, exception);
			}
			return exception;
		}

		// Token: 0x06001C10 RID: 7184 RVA: 0x00083EE8 File Offset: 0x000820E8
		internal static void CheckTrigger(BindingBase bb)
		{
			Binding binding = bb as Binding;
			if (binding != null && binding.UpdateSourceTrigger != UpdateSourceTrigger.PropertyChanged && binding.UpdateSourceTrigger != UpdateSourceTrigger.Default)
			{
				throw new InvalidOperationException(SR.Get("NoUpdateSourceTriggerForInnerBindingOfMultiBinding"));
			}
		}

		// Token: 0x06001C11 RID: 7185 RVA: 0x00083F20 File Offset: 0x00082120
		internal override BindingBase CreateClone()
		{
			return new MultiBinding();
		}

		// Token: 0x06001C12 RID: 7186 RVA: 0x00083F28 File Offset: 0x00082128
		internal override void InitializeClone(BindingBase baseClone, BindingMode mode)
		{
			MultiBinding multiBinding = (MultiBinding)baseClone;
			base.CopyValue(BindingBase.Feature.Converter, multiBinding);
			base.CopyValue(BindingBase.Feature.ConverterParameter, multiBinding);
			base.CopyValue(BindingBase.Feature.Culture, multiBinding);
			base.CopyValue(BindingBase.Feature.ValidationRules, multiBinding);
			base.CopyValue(BindingBase.Feature.ExceptionFilterCallback, multiBinding);
			for (int i = 0; i <= this._bindingCollection.Count; i++)
			{
				multiBinding._bindingCollection.Add(this._bindingCollection[i].Clone(mode));
			}
			base.InitializeClone(baseClone, mode);
		}

		// Token: 0x17000699 RID: 1689
		// (get) Token: 0x06001C13 RID: 7187 RVA: 0x00074739 File Offset: 0x00072939
		internal override Collection<ValidationRule> ValidationRulesInternal
		{
			get
			{
				return (ValidationRuleCollection)base.GetValue(BindingBase.Feature.ValidationRules, null);
			}
		}

		// Token: 0x1700069A RID: 1690
		// (get) Token: 0x06001C14 RID: 7188 RVA: 0x00083FA3 File Offset: 0x000821A3
		internal override CultureInfo ConverterCultureInternal
		{
			get
			{
				return this.ConverterCulture;
			}
		}

		// Token: 0x1700069B RID: 1691
		// (get) Token: 0x06001C15 RID: 7189 RVA: 0x00083FAB File Offset: 0x000821AB
		internal override bool ValidatesOnNotifyDataErrorsInternal
		{
			get
			{
				return this.ValidatesOnNotifyDataErrors;
			}
		}

		// Token: 0x06001C16 RID: 7190 RVA: 0x00083FB3 File Offset: 0x000821B3
		private void OnBindingCollectionChanged()
		{
			base.CheckSealed();
		}

		// Token: 0x040013BF RID: 5055
		private BindingCollection _bindingCollection;
	}
}
