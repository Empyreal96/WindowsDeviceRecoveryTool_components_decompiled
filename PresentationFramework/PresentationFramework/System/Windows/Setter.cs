using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace System.Windows
{
	/// <summary>Represents a setter that applies a property value.</summary>
	/// <exception cref="T:System.ArgumentNullException">The <see cref="P:System.Windows.Setter.Property" /> property cannot be null.</exception>
	/// <exception cref="T:System.ArgumentException">If the specified <see cref="P:System.Windows.Setter.Property" /> is a read-only property.</exception>
	/// <exception cref="T:System.ArgumentException">If the specified <see cref="P:System.Windows.Setter.Value" /> is set to <see cref="F:System.Windows.DependencyProperty.UnsetValue" />.</exception>
	// Token: 0x020000F2 RID: 242
	[XamlSetMarkupExtension("ReceiveMarkupExtension")]
	[XamlSetTypeConverter("ReceiveTypeConverter")]
	public class Setter : SetterBase, ISupportInitialize
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Setter" /> class.</summary>
		// Token: 0x0600088F RID: 2191 RVA: 0x0001BD34 File Offset: 0x00019F34
		public Setter()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Setter" /> class with the specified property and value.</summary>
		/// <param name="property">The <see cref="T:System.Windows.DependencyProperty" /> to apply the <see cref="P:System.Windows.Setter.Value" /> to.</param>
		/// <param name="value">The value to apply to the property.</param>
		// Token: 0x06000890 RID: 2192 RVA: 0x0001BD47 File Offset: 0x00019F47
		public Setter(DependencyProperty property, object value)
		{
			this.Initialize(property, value, null);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Setter" /> class with the specified property, value, and target name.</summary>
		/// <param name="property">The <see cref="T:System.Windows.DependencyProperty" /> to apply the <see cref="P:System.Windows.Setter.Value" /> to.</param>
		/// <param name="value">The value to apply to the property.</param>
		/// <param name="targetName">The name of the child node this <see cref="T:System.Windows.Setter" /> is intended for.</param>
		// Token: 0x06000891 RID: 2193 RVA: 0x0001BD63 File Offset: 0x00019F63
		public Setter(DependencyProperty property, object value, string targetName)
		{
			this.Initialize(property, value, targetName);
		}

		// Token: 0x06000892 RID: 2194 RVA: 0x0001BD7F File Offset: 0x00019F7F
		private void Initialize(DependencyProperty property, object value, string target)
		{
			if (value == DependencyProperty.UnsetValue)
			{
				throw new ArgumentException(SR.Get("SetterValueCannotBeUnset"));
			}
			this.CheckValidProperty(property);
			this._property = property;
			this._value = value;
			this._target = target;
		}

		// Token: 0x06000893 RID: 2195 RVA: 0x0001BDB8 File Offset: 0x00019FB8
		private void CheckValidProperty(DependencyProperty property)
		{
			if (property == null)
			{
				throw new ArgumentNullException("property");
			}
			if (property.ReadOnly)
			{
				throw new ArgumentException(SR.Get("ReadOnlyPropertyNotAllowed", new object[]
				{
					property.Name,
					base.GetType().Name
				}));
			}
			if (property == FrameworkElement.NameProperty)
			{
				throw new InvalidOperationException(SR.Get("CannotHavePropertyInStyle", new object[]
				{
					FrameworkElement.NameProperty.Name
				}));
			}
		}

		// Token: 0x06000894 RID: 2196 RVA: 0x0001BE34 File Offset: 0x0001A034
		internal override void Seal()
		{
			DependencyProperty property = this.Property;
			object valueInternal = this.ValueInternal;
			if (property == null)
			{
				throw new ArgumentException(SR.Get("NullPropertyIllegal", new object[]
				{
					"Setter.Property"
				}));
			}
			if (string.IsNullOrEmpty(this.TargetName) && property == FrameworkElement.StyleProperty)
			{
				throw new ArgumentException(SR.Get("StylePropertyInStyleNotAllowed"));
			}
			if (!property.IsValidValue(valueInternal))
			{
				if (valueInternal is MarkupExtension)
				{
					if (!(valueInternal is DynamicResourceExtension) && !(valueInternal is BindingBase))
					{
						throw new ArgumentException(SR.Get("SetterValueOfMarkupExtensionNotSupported", new object[]
						{
							valueInternal.GetType().Name
						}));
					}
				}
				else if (!(valueInternal is DeferredReference))
				{
					throw new ArgumentException(SR.Get("InvalidSetterValue", new object[]
					{
						valueInternal,
						property.OwnerType,
						property.Name
					}));
				}
			}
			StyleHelper.SealIfSealable(this._value);
			base.Seal();
		}

		/// <summary>Gets or sets the property to which the <see cref="P:System.Windows.Setter.Value" /> will be applied.</summary>
		/// <returns>A <see cref="T:System.Windows.DependencyProperty" /> to which the <see cref="P:System.Windows.Setter.Value" /> will be applied. The default value is null.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <see cref="P:System.Windows.Setter.Property" /> property cannot be <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">The specified <see cref="P:System.Windows.Setter.Property" /> property cannot be read-only.</exception>
		/// <exception cref="T:System.InvalidOperationException">If the specified <see cref="P:System.Windows.Setter.Value" /> is not valid for the type of the specified <see cref="P:System.Windows.Setter.Property" />.</exception>
		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x06000895 RID: 2197 RVA: 0x0001BF1F File Offset: 0x0001A11F
		// (set) Token: 0x06000896 RID: 2198 RVA: 0x0001BF27 File Offset: 0x0001A127
		[Ambient]
		[DefaultValue(null)]
		[Localizability(LocalizationCategory.None, Modifiability = Modifiability.Unmodifiable, Readability = Readability.Unreadable)]
		public DependencyProperty Property
		{
			get
			{
				return this._property;
			}
			set
			{
				this.CheckValidProperty(value);
				base.CheckSealed();
				this._property = value;
			}
		}

		/// <summary>Gets or sets the value to apply to the property that is specified by this <see cref="T:System.Windows.Setter" />.</summary>
		/// <returns>The default value is <see cref="F:System.Windows.DependencyProperty.UnsetValue" />.</returns>
		/// <exception cref="T:System.ArgumentException">If the specified <see cref="P:System.Windows.Setter.Value" /> is set to <see cref="F:System.Windows.DependencyProperty.UnsetValue" />.</exception>
		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x06000897 RID: 2199 RVA: 0x0001BF40 File Offset: 0x0001A140
		// (set) Token: 0x06000898 RID: 2200 RVA: 0x0001BF6F File Offset: 0x0001A16F
		[DependsOn("Property")]
		[DependsOn("TargetName")]
		[Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
		[TypeConverter(typeof(SetterTriggerConditionValueConverter))]
		public object Value
		{
			get
			{
				DeferredReference deferredReference = this._value as DeferredReference;
				if (deferredReference != null)
				{
					this._value = deferredReference.GetValue(BaseValueSourceInternal.Unknown);
				}
				return this._value;
			}
			set
			{
				if (value == DependencyProperty.UnsetValue)
				{
					throw new ArgumentException(SR.Get("SetterValueCannotBeUnset"));
				}
				base.CheckSealed();
				if (value is Expression)
				{
					throw new ArgumentException(SR.Get("StyleValueOfExpressionNotSupported"));
				}
				this._value = value;
			}
		}

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x06000899 RID: 2201 RVA: 0x0001BFAE File Offset: 0x0001A1AE
		internal object ValueInternal
		{
			get
			{
				return this._value;
			}
		}

		/// <summary>Gets or sets the name of the object this <see cref="T:System.Windows.Setter" /> is intended for.</summary>
		/// <returns>The default value is <see langword="null" />.</returns>
		// Token: 0x170001CB RID: 459
		// (get) Token: 0x0600089A RID: 2202 RVA: 0x0001BFB6 File Offset: 0x0001A1B6
		// (set) Token: 0x0600089B RID: 2203 RVA: 0x0001BFBE File Offset: 0x0001A1BE
		[DefaultValue(null)]
		[Ambient]
		public string TargetName
		{
			get
			{
				return this._target;
			}
			set
			{
				base.CheckSealed();
				this._target = value;
			}
		}

		/// <summary>Handles cases where a markup extension provides a value for a property of <see cref="T:System.Windows.Setter" /> object.</summary>
		/// <param name="targetObject">The object where the markup extension sets the value.</param>
		/// <param name="eventArgs">Data that is relevant for markup extension processing.</param>
		// Token: 0x0600089C RID: 2204 RVA: 0x0001BFD0 File Offset: 0x0001A1D0
		public static void ReceiveMarkupExtension(object targetObject, XamlSetMarkupExtensionEventArgs eventArgs)
		{
			if (targetObject == null)
			{
				throw new ArgumentNullException("targetObject");
			}
			if (eventArgs == null)
			{
				throw new ArgumentNullException("eventArgs");
			}
			Setter setter = targetObject as Setter;
			if (setter == null || eventArgs.Member.Name != "Value")
			{
				return;
			}
			MarkupExtension markupExtension = eventArgs.MarkupExtension;
			if (markupExtension is StaticResourceExtension)
			{
				StaticResourceExtension staticResourceExtension = markupExtension as StaticResourceExtension;
				setter.Value = staticResourceExtension.ProvideValueInternal(eventArgs.ServiceProvider, true);
				eventArgs.Handled = true;
				return;
			}
			if (markupExtension is DynamicResourceExtension || markupExtension is BindingBase)
			{
				setter.Value = markupExtension;
				eventArgs.Handled = true;
			}
		}

		/// <summary>Handles cases where a type converter provides a value for a property of a <see cref="T:System.Windows.Setter" /> object.</summary>
		/// <param name="targetObject">The object where the type converter sets the value.</param>
		/// <param name="eventArgs">Data that is relevant for type converter processing.</param>
		// Token: 0x0600089D RID: 2205 RVA: 0x0001C06C File Offset: 0x0001A26C
		public static void ReceiveTypeConverter(object targetObject, XamlSetTypeConverterEventArgs eventArgs)
		{
			Setter setter = targetObject as Setter;
			if (setter == null)
			{
				throw new ArgumentNullException("targetObject");
			}
			if (eventArgs == null)
			{
				throw new ArgumentNullException("eventArgs");
			}
			if (eventArgs.Member.Name == "Property")
			{
				setter._unresolvedProperty = eventArgs.Value;
				setter._serviceProvider = eventArgs.ServiceProvider;
				setter._cultureInfoForTypeConverter = eventArgs.CultureInfo;
				eventArgs.Handled = true;
				return;
			}
			if (eventArgs.Member.Name == "Value")
			{
				setter._unresolvedValue = eventArgs.Value;
				setter._serviceProvider = eventArgs.ServiceProvider;
				setter._cultureInfoForTypeConverter = eventArgs.CultureInfo;
				eventArgs.Handled = true;
			}
		}

		/// <summary>Signals the object that initialization is starting. </summary>
		// Token: 0x0600089E RID: 2206 RVA: 0x00002137 File Offset: 0x00000337
		void ISupportInitialize.BeginInit()
		{
		}

		/// <summary>Signals the object that initialization is complete. </summary>
		// Token: 0x0600089F RID: 2207 RVA: 0x0001C124 File Offset: 0x0001A324
		void ISupportInitialize.EndInit()
		{
			if (this._unresolvedProperty != null)
			{
				try
				{
					this.Property = DependencyPropertyConverter.ResolveProperty(this._serviceProvider, this.TargetName, this._unresolvedProperty);
				}
				finally
				{
					this._unresolvedProperty = null;
				}
			}
			if (this._unresolvedValue != null)
			{
				try
				{
					this.Value = SetterTriggerConditionValueConverter.ResolveValue(this._serviceProvider, this.Property, this._cultureInfoForTypeConverter, this._unresolvedValue);
				}
				finally
				{
					this._unresolvedValue = null;
				}
			}
			this._serviceProvider = null;
			this._cultureInfoForTypeConverter = null;
		}

		// Token: 0x040007A9 RID: 1961
		private DependencyProperty _property;

		// Token: 0x040007AA RID: 1962
		private object _value = DependencyProperty.UnsetValue;

		// Token: 0x040007AB RID: 1963
		private string _target;

		// Token: 0x040007AC RID: 1964
		private object _unresolvedProperty;

		// Token: 0x040007AD RID: 1965
		private object _unresolvedValue;

		// Token: 0x040007AE RID: 1966
		private ITypeDescriptorContext _serviceProvider;

		// Token: 0x040007AF RID: 1967
		private CultureInfo _cultureInfoForTypeConverter;
	}
}
