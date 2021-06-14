using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Markup;
using MS.Internal;

namespace System.Windows.Data
{
	/// <summary>Defines the common characteristics of the <see cref="T:System.Windows.Data.Binding" />, <see cref="T:System.Windows.Data.PriorityBinding" />, and <see cref="T:System.Windows.Data.MultiBinding" /> classes. </summary>
	// Token: 0x0200019D RID: 413
	[MarkupExtensionReturnType(typeof(object))]
	[Localizability(LocalizationCategory.None, Modifiability = Modifiability.Unmodifiable, Readability = Readability.Unreadable)]
	public abstract class BindingBase : MarkupExtension
	{
		// Token: 0x06001817 RID: 6167 RVA: 0x00074881 File Offset: 0x00072A81
		internal BindingBase()
		{
		}

		/// <summary>Gets or sets the value to use when the binding is unable to return a value.</summary>
		/// <returns>The default value is <see cref="F:System.Windows.DependencyProperty.UnsetValue" />.</returns>
		// Token: 0x17000576 RID: 1398
		// (get) Token: 0x06001818 RID: 6168 RVA: 0x00074894 File Offset: 0x00072A94
		// (set) Token: 0x06001819 RID: 6169 RVA: 0x000748A2 File Offset: 0x00072AA2
		public object FallbackValue
		{
			get
			{
				return this.GetValue(BindingBase.Feature.FallbackValue, DependencyProperty.UnsetValue);
			}
			set
			{
				this.CheckSealed();
				this.SetValue(BindingBase.Feature.FallbackValue, value);
			}
		}

		/// <summary>Returns a value that indicates whether serialization processes should serialize the effective value of the <see cref="P:System.Windows.Data.BindingBase.FallbackValue" /> property on instances of this class.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="P:System.Windows.Data.BindingBase.FallbackValue" /> property value should be serialized; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600181A RID: 6170 RVA: 0x000748B2 File Offset: 0x00072AB2
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeFallbackValue()
		{
			return this.HasValue(BindingBase.Feature.FallbackValue);
		}

		/// <summary>Gets or sets a string that specifies how to format the binding if it displays the bound value as a string.</summary>
		/// <returns>A string that specifies how to format the binding if it displays the bound value as a string.</returns>
		// Token: 0x17000577 RID: 1399
		// (get) Token: 0x0600181B RID: 6171 RVA: 0x000748BB File Offset: 0x00072ABB
		// (set) Token: 0x0600181C RID: 6172 RVA: 0x000748CA File Offset: 0x00072ACA
		[DefaultValue(null)]
		public string StringFormat
		{
			get
			{
				return (string)this.GetValue(BindingBase.Feature.StringFormat, null);
			}
			set
			{
				this.CheckSealed();
				this.SetValue(BindingBase.Feature.StringFormat, value, null);
			}
		}

		/// <summary>Gets or sets the value that is used in the target when the value of the source is <see langword="null" />.</summary>
		/// <returns>The value that is used in the target when the value of the source is <see langword="null" />.</returns>
		// Token: 0x17000578 RID: 1400
		// (get) Token: 0x0600181D RID: 6173 RVA: 0x000748DB File Offset: 0x00072ADB
		// (set) Token: 0x0600181E RID: 6174 RVA: 0x000748E9 File Offset: 0x00072AE9
		public object TargetNullValue
		{
			get
			{
				return this.GetValue(BindingBase.Feature.TargetNullValue, DependencyProperty.UnsetValue);
			}
			set
			{
				this.CheckSealed();
				this.SetValue(BindingBase.Feature.TargetNullValue, value);
			}
		}

		/// <summary>Returns a value that indicates whether the <see cref="P:System.Windows.Data.BindingBase.TargetNullValue" /> property should be serialized.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="P:System.Windows.Data.BindingBase.TargetNullValue" /> property should be serialized; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600181F RID: 6175 RVA: 0x000748F9 File Offset: 0x00072AF9
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeTargetNullValue()
		{
			return this.HasValue(BindingBase.Feature.TargetNullValue);
		}

		/// <summary>Gets or sets the name of the <see cref="T:System.Windows.Data.BindingGroup" /> to which this binding belongs.</summary>
		/// <returns>The name of the <see cref="T:System.Windows.Data.BindingGroup" /> to which this binding belongs.</returns>
		// Token: 0x17000579 RID: 1401
		// (get) Token: 0x06001820 RID: 6176 RVA: 0x00074902 File Offset: 0x00072B02
		// (set) Token: 0x06001821 RID: 6177 RVA: 0x00074915 File Offset: 0x00072B15
		[DefaultValue("")]
		public string BindingGroupName
		{
			get
			{
				return (string)this.GetValue(BindingBase.Feature.BindingGroupName, string.Empty);
			}
			set
			{
				this.CheckSealed();
				this.SetValue(BindingBase.Feature.BindingGroupName, value, string.Empty);
			}
		}

		/// <summary>Gets or sets the amount of time, in milliseconds, to wait before updating the binding source after the value on the target changes.</summary>
		/// <returns>The amount of time, in milliseconds, to wait before updating the binding source.</returns>
		// Token: 0x1700057A RID: 1402
		// (get) Token: 0x06001822 RID: 6178 RVA: 0x0007492A File Offset: 0x00072B2A
		// (set) Token: 0x06001823 RID: 6179 RVA: 0x0007493E File Offset: 0x00072B3E
		[DefaultValue(0)]
		public int Delay
		{
			get
			{
				return (int)this.GetValue(BindingBase.Feature.Delay, 0);
			}
			set
			{
				this.CheckSealed();
				this.SetValue(BindingBase.Feature.Delay, value, 0);
			}
		}

		/// <summary>Returns an object that should be set on the property where this binding and extension are applied.</summary>
		/// <param name="serviceProvider">The object that can provide services for the markup extension. May be <see langword="null" />; see the Remarks section for more information.</param>
		/// <returns>The value to set on the binding target property.</returns>
		// Token: 0x06001824 RID: 6180 RVA: 0x0007495C File Offset: 0x00072B5C
		public sealed override object ProvideValue(IServiceProvider serviceProvider)
		{
			if (serviceProvider == null)
			{
				return this;
			}
			DependencyObject dependencyObject;
			DependencyProperty dependencyProperty;
			Helper.CheckCanReceiveMarkupExtension(this, serviceProvider, out dependencyObject, out dependencyProperty);
			if (dependencyObject == null || dependencyProperty == null)
			{
				return this;
			}
			return this.CreateBindingExpression(dependencyObject, dependencyProperty);
		}

		// Token: 0x06001825 RID: 6181
		internal abstract BindingExpressionBase CreateBindingExpressionOverride(DependencyObject targetObject, DependencyProperty targetProperty, BindingExpressionBase owner);

		// Token: 0x06001826 RID: 6182 RVA: 0x00074989 File Offset: 0x00072B89
		internal bool TestFlag(BindingBase.BindingFlags flag)
		{
			return (this._flags & flag) > BindingBase.BindingFlags.OneTime;
		}

		// Token: 0x06001827 RID: 6183 RVA: 0x00074996 File Offset: 0x00072B96
		internal void SetFlag(BindingBase.BindingFlags flag)
		{
			this._flags |= flag;
		}

		// Token: 0x06001828 RID: 6184 RVA: 0x000749A6 File Offset: 0x00072BA6
		internal void ClearFlag(BindingBase.BindingFlags flag)
		{
			this._flags &= ~flag;
		}

		// Token: 0x06001829 RID: 6185 RVA: 0x000749B7 File Offset: 0x00072BB7
		internal void ChangeFlag(BindingBase.BindingFlags flag, bool value)
		{
			if (value)
			{
				this._flags |= flag;
				return;
			}
			this._flags &= ~flag;
		}

		// Token: 0x0600182A RID: 6186 RVA: 0x000749DA File Offset: 0x00072BDA
		internal BindingBase.BindingFlags GetFlagsWithinMask(BindingBase.BindingFlags mask)
		{
			return this._flags & mask;
		}

		// Token: 0x0600182B RID: 6187 RVA: 0x000749E4 File Offset: 0x00072BE4
		internal void ChangeFlagsWithinMask(BindingBase.BindingFlags mask, BindingBase.BindingFlags flags)
		{
			this._flags = ((this._flags & ~mask) | (flags & mask));
		}

		// Token: 0x0600182C RID: 6188 RVA: 0x000749F9 File Offset: 0x00072BF9
		internal static BindingBase.BindingFlags FlagsFrom(BindingMode bindingMode)
		{
			switch (bindingMode)
			{
			case BindingMode.TwoWay:
				return BindingBase.BindingFlags.TwoWay;
			case BindingMode.OneWay:
				return BindingBase.BindingFlags.OneWay;
			case BindingMode.OneTime:
				return BindingBase.BindingFlags.OneTime;
			case BindingMode.OneWayToSource:
				return BindingBase.BindingFlags.OneWayToSource;
			case BindingMode.Default:
				return BindingBase.BindingFlags.PropDefault;
			default:
				return BindingBase.BindingFlags.IllegalInput;
			}
		}

		// Token: 0x0600182D RID: 6189 RVA: 0x00074A26 File Offset: 0x00072C26
		internal static BindingBase.BindingFlags FlagsFrom(UpdateSourceTrigger updateSourceTrigger)
		{
			switch (updateSourceTrigger)
			{
			case UpdateSourceTrigger.Default:
				return BindingBase.BindingFlags.UpdateDefault;
			case UpdateSourceTrigger.PropertyChanged:
				return BindingBase.BindingFlags.OneTime;
			case UpdateSourceTrigger.LostFocus:
				return BindingBase.BindingFlags.UpdateOnLostFocus;
			case UpdateSourceTrigger.Explicit:
				return BindingBase.BindingFlags.UpdateExplicitly;
			default:
				return BindingBase.BindingFlags.IllegalInput;
			}
		}

		// Token: 0x1700057B RID: 1403
		// (get) Token: 0x0600182E RID: 6190 RVA: 0x00074A59 File Offset: 0x00072C59
		internal BindingBase.BindingFlags Flags
		{
			get
			{
				return this._flags;
			}
		}

		// Token: 0x1700057C RID: 1404
		// (get) Token: 0x0600182F RID: 6191 RVA: 0x0000C238 File Offset: 0x0000A438
		internal virtual CultureInfo ConverterCultureInternal
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700057D RID: 1405
		// (get) Token: 0x06001830 RID: 6192 RVA: 0x0000C238 File Offset: 0x0000A438
		internal virtual Collection<ValidationRule> ValidationRulesInternal
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700057E RID: 1406
		// (get) Token: 0x06001831 RID: 6193 RVA: 0x0000B02A File Offset: 0x0000922A
		internal virtual bool ValidatesOnNotifyDataErrorsInternal
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06001832 RID: 6194 RVA: 0x00074A61 File Offset: 0x00072C61
		internal BindingExpressionBase CreateBindingExpression(DependencyObject targetObject, DependencyProperty targetProperty)
		{
			this._isSealed = true;
			return this.CreateBindingExpressionOverride(targetObject, targetProperty, null);
		}

		// Token: 0x06001833 RID: 6195 RVA: 0x00074A73 File Offset: 0x00072C73
		internal BindingExpressionBase CreateBindingExpression(DependencyObject targetObject, DependencyProperty targetProperty, BindingExpressionBase owner)
		{
			this._isSealed = true;
			return this.CreateBindingExpressionOverride(targetObject, targetProperty, owner);
		}

		// Token: 0x06001834 RID: 6196 RVA: 0x00074A85 File Offset: 0x00072C85
		internal void CheckSealed()
		{
			if (this._isSealed)
			{
				throw new InvalidOperationException(SR.Get("ChangeSealedBinding"));
			}
		}

		// Token: 0x06001835 RID: 6197 RVA: 0x00074AA0 File Offset: 0x00072CA0
		internal ValidationRule GetValidationRule(Type type)
		{
			if (this.TestFlag(BindingBase.BindingFlags.ValidatesOnExceptions) && type == typeof(ExceptionValidationRule))
			{
				return ExceptionValidationRule.Instance;
			}
			if (this.TestFlag(BindingBase.BindingFlags.ValidatesOnDataErrors) && type == typeof(DataErrorValidationRule))
			{
				return DataErrorValidationRule.Instance;
			}
			if (this.TestFlag(BindingBase.BindingFlags.ValidatesOnNotifyDataErrors) && type == typeof(NotifyDataErrorValidationRule))
			{
				return NotifyDataErrorValidationRule.Instance;
			}
			return this.LookupValidationRule(type);
		}

		// Token: 0x06001836 RID: 6198 RVA: 0x0000C238 File Offset: 0x0000A438
		internal virtual ValidationRule LookupValidationRule(Type type)
		{
			return null;
		}

		// Token: 0x06001837 RID: 6199 RVA: 0x00074B24 File Offset: 0x00072D24
		internal static ValidationRule LookupValidationRule(Type type, Collection<ValidationRule> collection)
		{
			if (collection == null)
			{
				return null;
			}
			for (int i = 0; i < collection.Count; i++)
			{
				if (type.IsInstanceOfType(collection[i]))
				{
					return collection[i];
				}
			}
			return null;
		}

		// Token: 0x06001838 RID: 6200 RVA: 0x00074B60 File Offset: 0x00072D60
		internal BindingBase Clone(BindingMode mode)
		{
			BindingBase bindingBase = this.CreateClone();
			this.InitializeClone(bindingBase, mode);
			return bindingBase;
		}

		// Token: 0x06001839 RID: 6201 RVA: 0x00074B80 File Offset: 0x00072D80
		internal virtual void InitializeClone(BindingBase clone, BindingMode mode)
		{
			clone._flags = this._flags;
			this.CopyValue(BindingBase.Feature.FallbackValue, clone);
			clone._isSealed = this._isSealed;
			this.CopyValue(BindingBase.Feature.StringFormat, clone);
			this.CopyValue(BindingBase.Feature.TargetNullValue, clone);
			this.CopyValue(BindingBase.Feature.BindingGroupName, clone);
			clone.ChangeFlagsWithinMask(BindingBase.BindingFlags.PropagationMask, BindingBase.FlagsFrom(mode));
		}

		// Token: 0x0600183A RID: 6202
		internal abstract BindingBase CreateClone();

		// Token: 0x0600183B RID: 6203 RVA: 0x00074BD2 File Offset: 0x00072DD2
		internal bool HasValue(BindingBase.Feature id)
		{
			return this._values.HasValue((int)id);
		}

		// Token: 0x0600183C RID: 6204 RVA: 0x00074BE0 File Offset: 0x00072DE0
		internal object GetValue(BindingBase.Feature id, object defaultValue)
		{
			return this._values.GetValue((int)id, defaultValue);
		}

		// Token: 0x0600183D RID: 6205 RVA: 0x00074BEF File Offset: 0x00072DEF
		internal void SetValue(BindingBase.Feature id, object value)
		{
			this._values.SetValue((int)id, value);
		}

		// Token: 0x0600183E RID: 6206 RVA: 0x00074BFE File Offset: 0x00072DFE
		internal void SetValue(BindingBase.Feature id, object value, object defaultValue)
		{
			if (object.Equals(value, defaultValue))
			{
				this._values.ClearValue((int)id);
				return;
			}
			this._values.SetValue((int)id, value);
		}

		// Token: 0x0600183F RID: 6207 RVA: 0x00074C23 File Offset: 0x00072E23
		internal void ClearValue(BindingBase.Feature id)
		{
			this._values.ClearValue((int)id);
		}

		// Token: 0x06001840 RID: 6208 RVA: 0x00074C31 File Offset: 0x00072E31
		internal void CopyValue(BindingBase.Feature id, BindingBase clone)
		{
			if (this.HasValue(id))
			{
				clone.SetValue(id, this.GetValue(id, null));
			}
		}

		// Token: 0x04001301 RID: 4865
		private BindingBase.BindingFlags _flags = BindingBase.BindingFlags.Default;

		// Token: 0x04001302 RID: 4866
		private bool _isSealed;

		// Token: 0x04001303 RID: 4867
		private UncommonValueTable _values;

		// Token: 0x02000861 RID: 2145
		[Flags]
		internal enum BindingFlags : uint
		{
			// Token: 0x0400409C RID: 16540
			OneWay = 1U,
			// Token: 0x0400409D RID: 16541
			TwoWay = 3U,
			// Token: 0x0400409E RID: 16542
			OneWayToSource = 2U,
			// Token: 0x0400409F RID: 16543
			OneTime = 0U,
			// Token: 0x040040A0 RID: 16544
			PropDefault = 4U,
			// Token: 0x040040A1 RID: 16545
			NotifyOnTargetUpdated = 8U,
			// Token: 0x040040A2 RID: 16546
			NotifyOnSourceUpdated = 8388608U,
			// Token: 0x040040A3 RID: 16547
			NotifyOnValidationError = 2097152U,
			// Token: 0x040040A4 RID: 16548
			UpdateDefault = 3072U,
			// Token: 0x040040A5 RID: 16549
			UpdateOnPropertyChanged = 0U,
			// Token: 0x040040A6 RID: 16550
			UpdateOnLostFocus = 1024U,
			// Token: 0x040040A7 RID: 16551
			UpdateExplicitly = 2048U,
			// Token: 0x040040A8 RID: 16552
			PathGeneratedInternally = 8192U,
			// Token: 0x040040A9 RID: 16553
			ValidatesOnExceptions = 16777216U,
			// Token: 0x040040AA RID: 16554
			ValidatesOnDataErrors = 33554432U,
			// Token: 0x040040AB RID: 16555
			ValidatesOnNotifyDataErrors = 536870912U,
			// Token: 0x040040AC RID: 16556
			PropagationMask = 7U,
			// Token: 0x040040AD RID: 16557
			UpdateMask = 3072U,
			// Token: 0x040040AE RID: 16558
			Default = 536873988U,
			// Token: 0x040040AF RID: 16559
			IllegalInput = 67108864U
		}

		// Token: 0x02000862 RID: 2146
		internal enum Feature
		{
			// Token: 0x040040B1 RID: 16561
			FallbackValue,
			// Token: 0x040040B2 RID: 16562
			StringFormat,
			// Token: 0x040040B3 RID: 16563
			TargetNullValue,
			// Token: 0x040040B4 RID: 16564
			BindingGroupName,
			// Token: 0x040040B5 RID: 16565
			Delay,
			// Token: 0x040040B6 RID: 16566
			XPath,
			// Token: 0x040040B7 RID: 16567
			Culture,
			// Token: 0x040040B8 RID: 16568
			AsyncState,
			// Token: 0x040040B9 RID: 16569
			ObjectSource,
			// Token: 0x040040BA RID: 16570
			RelativeSource,
			// Token: 0x040040BB RID: 16571
			ElementSource,
			// Token: 0x040040BC RID: 16572
			Converter,
			// Token: 0x040040BD RID: 16573
			ConverterParameter,
			// Token: 0x040040BE RID: 16574
			ValidationRules,
			// Token: 0x040040BF RID: 16575
			ExceptionFilterCallback,
			// Token: 0x040040C0 RID: 16576
			AttachedPropertiesInPath,
			// Token: 0x040040C1 RID: 16577
			LastFeatureId
		}
	}
}
