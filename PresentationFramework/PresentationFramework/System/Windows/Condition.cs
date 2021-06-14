using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace System.Windows
{
	/// <summary>Represents a condition for the <see cref="T:System.Windows.MultiTrigger" /> and the <see cref="T:System.Windows.MultiDataTrigger" />, which apply changes to property values based on a set of conditions.</summary>
	// Token: 0x020000A3 RID: 163
	[XamlSetMarkupExtension("ReceiveMarkupExtension")]
	[XamlSetTypeConverter("ReceiveTypeConverter")]
	public sealed class Condition : ISupportInitialize
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Condition" /> class. </summary>
		// Token: 0x0600034B RID: 843 RVA: 0x0000957E File Offset: 0x0000777E
		public Condition()
		{
			this._property = null;
			this._binding = null;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Condition" /> class with the specified property and value. This constructor performs parameter validation. </summary>
		/// <param name="conditionProperty">The property of the condition.</param>
		/// <param name="conditionValue">The value of the condition.</param>
		// Token: 0x0600034C RID: 844 RVA: 0x0000959F File Offset: 0x0000779F
		public Condition(DependencyProperty conditionProperty, object conditionValue) : this(conditionProperty, conditionValue, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Condition" /> class with the specified property, value, and the name of the source object.</summary>
		/// <param name="conditionProperty">The property of the condition.</param>
		/// <param name="conditionValue">The value of the condition.</param>
		/// <param name="sourceName">
		///       x:Name of the object with the <paramref name="conditionProperty" />.</param>
		// Token: 0x0600034D RID: 845 RVA: 0x000095AC File Offset: 0x000077AC
		public Condition(DependencyProperty conditionProperty, object conditionValue, string sourceName)
		{
			if (conditionProperty == null)
			{
				throw new ArgumentNullException("conditionProperty");
			}
			if (!conditionProperty.IsValidValue(conditionValue))
			{
				throw new ArgumentException(SR.Get("InvalidPropertyValue", new object[]
				{
					conditionValue,
					conditionProperty.Name
				}));
			}
			this._property = conditionProperty;
			this.Value = conditionValue;
			this._sourceName = sourceName;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Condition" /> class. </summary>
		/// <param name="binding">The binding that specifies the property of the condition.</param>
		/// <param name="conditionValue">The value of the condition.</param>
		// Token: 0x0600034E RID: 846 RVA: 0x00009619 File Offset: 0x00007819
		public Condition(BindingBase binding, object conditionValue)
		{
			if (binding == null)
			{
				throw new ArgumentNullException("binding");
			}
			this.Binding = binding;
			this.Value = conditionValue;
		}

		/// <summary>Gets or sets the property of the condition. This is only applicable to <see cref="T:System.Windows.MultiTrigger" /> objects.</summary>
		/// <returns>A <see cref="T:System.Windows.DependencyProperty" /> that specifies the property of the condition. The default value is null.</returns>
		// Token: 0x1700006F RID: 111
		// (get) Token: 0x0600034F RID: 847 RVA: 0x00009648 File Offset: 0x00007848
		// (set) Token: 0x06000350 RID: 848 RVA: 0x00009650 File Offset: 0x00007850
		[Ambient]
		[DefaultValue(null)]
		public DependencyProperty Property
		{
			get
			{
				return this._property;
			}
			set
			{
				if (this._sealed)
				{
					throw new InvalidOperationException(SR.Get("CannotChangeAfterSealed", new object[]
					{
						"Condition"
					}));
				}
				if (this._binding != null)
				{
					throw new InvalidOperationException(SR.Get("ConditionCannotUseBothPropertyAndBinding"));
				}
				this._property = value;
			}
		}

		/// <summary>Gets or sets the binding that specifies the property of the condition. This is only applicable to <see cref="T:System.Windows.MultiDataTrigger" /> objects.</summary>
		/// <returns>The default value is null.</returns>
		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000351 RID: 849 RVA: 0x000096A2 File Offset: 0x000078A2
		// (set) Token: 0x06000352 RID: 850 RVA: 0x000096AC File Offset: 0x000078AC
		[DefaultValue(null)]
		public BindingBase Binding
		{
			get
			{
				return this._binding;
			}
			set
			{
				if (this._sealed)
				{
					throw new InvalidOperationException(SR.Get("CannotChangeAfterSealed", new object[]
					{
						"Condition"
					}));
				}
				if (this._property != null)
				{
					throw new InvalidOperationException(SR.Get("ConditionCannotUseBothPropertyAndBinding"));
				}
				this._binding = value;
			}
		}

		/// <summary>Gets or sets the value of the condition.</summary>
		/// <returns>The <see cref="P:System.Windows.Condition.Value" /> property cannot be null for a given <see cref="T:System.Windows.Condition" />.See also the Exceptions section. The default value is null.</returns>
		/// <exception cref="T:System.ArgumentException">
		///         <see cref="T:System.Windows.Markup.MarkupExtension" />s are not supported.</exception>
		/// <exception cref="T:System.ArgumentException">Expressions are not supported.</exception>
		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000353 RID: 851 RVA: 0x000096FE File Offset: 0x000078FE
		// (set) Token: 0x06000354 RID: 852 RVA: 0x00009708 File Offset: 0x00007908
		[TypeConverter(typeof(SetterTriggerConditionValueConverter))]
		public object Value
		{
			get
			{
				return this._value;
			}
			set
			{
				if (this._sealed)
				{
					throw new InvalidOperationException(SR.Get("CannotChangeAfterSealed", new object[]
					{
						"Condition"
					}));
				}
				if (value is MarkupExtension)
				{
					throw new ArgumentException(SR.Get("ConditionValueOfMarkupExtensionNotSupported", new object[]
					{
						value.GetType().Name
					}));
				}
				if (value is Expression)
				{
					throw new ArgumentException(SR.Get("ConditionValueOfExpressionNotSupported"));
				}
				this._value = value;
			}
		}

		/// <summary>Gets or sets the name of the object with the property that causes the associated setters to be applied. This is only applicable to <see cref="T:System.Windows.MultiTrigger" /> objects.</summary>
		/// <returns>The default property is <see langword="null" />. If this property is <see langword="null" />, then the property of the object being styled causes the associated setters to be applied.</returns>
		/// <exception cref="T:System.InvalidOperationException">After a <see cref="T:System.Windows.Condition" /> is in use, it cannot be modified.</exception>
		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000355 RID: 853 RVA: 0x00009786 File Offset: 0x00007986
		// (set) Token: 0x06000356 RID: 854 RVA: 0x0000978E File Offset: 0x0000798E
		[DefaultValue(null)]
		public string SourceName
		{
			get
			{
				return this._sourceName;
			}
			set
			{
				if (this._sealed)
				{
					throw new InvalidOperationException(SR.Get("CannotChangeAfterSealed", new object[]
					{
						"Condition"
					}));
				}
				this._sourceName = value;
			}
		}

		// Token: 0x06000357 RID: 855 RVA: 0x000097C0 File Offset: 0x000079C0
		internal void Seal(ValueLookupType type)
		{
			if (this._sealed)
			{
				return;
			}
			this._sealed = true;
			if (this._property != null && this._binding != null)
			{
				throw new InvalidOperationException(SR.Get("ConditionCannotUseBothPropertyAndBinding"));
			}
			if (type - ValueLookupType.Trigger > 1)
			{
				if (type - ValueLookupType.DataTrigger > 1)
				{
					throw new InvalidOperationException(SR.Get("UnexpectedValueTypeForCondition", new object[]
					{
						type
					}));
				}
				if (this._binding == null)
				{
					throw new InvalidOperationException(SR.Get("NullPropertyIllegal", new object[]
					{
						"Binding"
					}));
				}
			}
			else
			{
				if (this._property == null)
				{
					throw new InvalidOperationException(SR.Get("NullPropertyIllegal", new object[]
					{
						"Property"
					}));
				}
				if (!this._property.IsValidValue(this._value))
				{
					throw new InvalidOperationException(SR.Get("InvalidPropertyValue", new object[]
					{
						this._value,
						this._property.Name
					}));
				}
			}
			StyleHelper.SealIfSealable(this._value);
		}

		/// <summary>Signals the object that initialization is starting.</summary>
		// Token: 0x06000358 RID: 856 RVA: 0x00002137 File Offset: 0x00000337
		void ISupportInitialize.BeginInit()
		{
		}

		/// <summary>Signals the object that initialization is complete.</summary>
		// Token: 0x06000359 RID: 857 RVA: 0x000098C4 File Offset: 0x00007AC4
		void ISupportInitialize.EndInit()
		{
			if (this._unresolvedProperty != null)
			{
				try
				{
					this.Property = DependencyPropertyConverter.ResolveProperty(this._serviceProvider, this.SourceName, this._unresolvedProperty);
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

		/// <summary>Handles cases where a markup extension provides a value for a property of a <see cref="T:System.Windows.Condition" /> object</summary>
		/// <param name="targetObject">The object where the markup extension sets the value.</param>
		/// <param name="eventArgs">Data that is relevant for markup extension processing.</param>
		// Token: 0x0600035A RID: 858 RVA: 0x00009960 File Offset: 0x00007B60
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
			Condition condition = targetObject as Condition;
			if (condition != null && eventArgs.Member.Name == "Binding" && eventArgs.MarkupExtension is BindingBase)
			{
				condition.Binding = (eventArgs.MarkupExtension as BindingBase);
				eventArgs.Handled = true;
			}
		}

		/// <summary>Handles cases where a type converter provides a value for a property of on a<see cref="T:System.Windows.Condition" /> object.</summary>
		/// <param name="targetObject">The object where the type converter sets the value.</param>
		/// <param name="eventArgs">Data that is relevant for type converter processing.</param>
		// Token: 0x0600035B RID: 859 RVA: 0x000099D0 File Offset: 0x00007BD0
		public static void ReceiveTypeConverter(object targetObject, XamlSetTypeConverterEventArgs eventArgs)
		{
			Condition condition = targetObject as Condition;
			if (condition == null)
			{
				throw new ArgumentNullException("targetObject");
			}
			if (eventArgs == null)
			{
				throw new ArgumentNullException("eventArgs");
			}
			if (eventArgs.Member.Name == "Property")
			{
				condition._unresolvedProperty = eventArgs.Value;
				condition._serviceProvider = eventArgs.ServiceProvider;
				condition._cultureInfoForTypeConverter = eventArgs.CultureInfo;
				eventArgs.Handled = true;
				return;
			}
			if (eventArgs.Member.Name == "Value")
			{
				condition._unresolvedValue = eventArgs.Value;
				condition._serviceProvider = eventArgs.ServiceProvider;
				condition._cultureInfoForTypeConverter = eventArgs.CultureInfo;
				eventArgs.Handled = true;
			}
		}

		// Token: 0x040005DD RID: 1501
		private bool _sealed;

		// Token: 0x040005DE RID: 1502
		private DependencyProperty _property;

		// Token: 0x040005DF RID: 1503
		private BindingBase _binding;

		// Token: 0x040005E0 RID: 1504
		private object _value = DependencyProperty.UnsetValue;

		// Token: 0x040005E1 RID: 1505
		private string _sourceName;

		// Token: 0x040005E2 RID: 1506
		private object _unresolvedProperty;

		// Token: 0x040005E3 RID: 1507
		private object _unresolvedValue;

		// Token: 0x040005E4 RID: 1508
		private ITypeDescriptorContext _serviceProvider;

		// Token: 0x040005E5 RID: 1509
		private CultureInfo _cultureInfoForTypeConverter;
	}
}
