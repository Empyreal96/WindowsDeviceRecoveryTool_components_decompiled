using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Markup;

namespace System.Windows
{
	/// <summary>Represents a trigger that applies property values or performs actions conditionally.</summary>
	// Token: 0x02000131 RID: 305
	[ContentProperty("Setters")]
	[XamlSetTypeConverter("ReceiveTypeConverter")]
	public class Trigger : TriggerBase, IAddChild, ISupportInitialize
	{
		/// <summary>Gets or sets the property that returns the value that is compared with the <see cref="P:System.Windows.Trigger.Value" /> property of the trigger. The comparison is a reference equality check.</summary>
		/// <returns>A <see cref="T:System.Windows.DependencyProperty" /> that returns the property value of the element. The default value is <see langword="null" />.</returns>
		/// <exception cref="T:System.ArgumentException">A <see cref="T:System.Windows.Style" /> cannot contain a <see cref="T:System.Windows.Trigger" /> that refers to the <see cref="T:System.Windows.Style" /> property.</exception>
		/// <exception cref="T:System.InvalidOperationException">After a <see cref="T:System.Windows.Trigger" /> is in use, it cannot be modified.</exception>
		// Token: 0x170003F9 RID: 1017
		// (get) Token: 0x06000C77 RID: 3191 RVA: 0x0002EE9D File Offset: 0x0002D09D
		// (set) Token: 0x06000C78 RID: 3192 RVA: 0x0002EEAB File Offset: 0x0002D0AB
		[Ambient]
		[Localizability(LocalizationCategory.None, Modifiability = Modifiability.Unmodifiable, Readability = Readability.Unreadable)]
		public DependencyProperty Property
		{
			get
			{
				base.VerifyAccess();
				return this._property;
			}
			set
			{
				base.VerifyAccess();
				if (base.IsSealed)
				{
					throw new InvalidOperationException(SR.Get("CannotChangeAfterSealed", new object[]
					{
						"Trigger"
					}));
				}
				this._property = value;
			}
		}

		/// <summary>Gets or sets the value to be compared with the property value of the element. The comparison is a reference equality check.</summary>
		/// <returns>The default value is <see langword="null" />. See also the Exceptions section.</returns>
		/// <exception cref="T:System.ArgumentException">Only load-time <see cref="T:System.Windows.Markup.MarkupExtension" />s are supported.</exception>
		/// <exception cref="T:System.ArgumentException">Expressions such as bindings are not supported.</exception>
		/// <exception cref="T:System.InvalidOperationException">After a <see cref="T:System.Windows.Trigger" /> is in use, it cannot be modified.</exception>
		// Token: 0x170003FA RID: 1018
		// (get) Token: 0x06000C79 RID: 3193 RVA: 0x0002EEE0 File Offset: 0x0002D0E0
		// (set) Token: 0x06000C7A RID: 3194 RVA: 0x0002EEF0 File Offset: 0x0002D0F0
		[DependsOn("Property")]
		[DependsOn("SourceName")]
		[Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
		[TypeConverter(typeof(SetterTriggerConditionValueConverter))]
		public object Value
		{
			get
			{
				base.VerifyAccess();
				return this._value;
			}
			set
			{
				base.VerifyAccess();
				if (base.IsSealed)
				{
					throw new InvalidOperationException(SR.Get("CannotChangeAfterSealed", new object[]
					{
						"Trigger"
					}));
				}
				if (value is NullExtension)
				{
					value = null;
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

		/// <summary>Gets or sets the name of the object with the property that causes the associated setters to be applied.</summary>
		/// <returns>The default property is <see langword="null" />. If this property is <see langword="null" />, then the <see cref="P:System.Windows.Trigger.Property" /> property is evaluated with respect to the element this style or template is being applied to (the styled parent or the templated parent).</returns>
		/// <exception cref="T:System.InvalidOperationException">After a <see cref="T:System.Windows.Trigger" /> is in use, it cannot be modified.</exception>
		// Token: 0x170003FB RID: 1019
		// (get) Token: 0x06000C7B RID: 3195 RVA: 0x0002EF7F File Offset: 0x0002D17F
		// (set) Token: 0x06000C7C RID: 3196 RVA: 0x0002EF8D File Offset: 0x0002D18D
		[DefaultValue(null)]
		[Ambient]
		public string SourceName
		{
			get
			{
				base.VerifyAccess();
				return this._sourceName;
			}
			set
			{
				base.VerifyAccess();
				if (base.IsSealed)
				{
					throw new InvalidOperationException(SR.Get("CannotChangeAfterSealed", new object[]
					{
						"Trigger"
					}));
				}
				this._sourceName = value;
			}
		}

		/// <summary>Gets a collection of <see cref="T:System.Windows.Setter" /> objects, which describe the property values to apply when the specified condition has been met.</summary>
		/// <returns>The default value is <see langword="null" />.</returns>
		// Token: 0x170003FC RID: 1020
		// (get) Token: 0x06000C7D RID: 3197 RVA: 0x0002EFC2 File Offset: 0x0002D1C2
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public SetterBaseCollection Setters
		{
			get
			{
				base.VerifyAccess();
				if (this._setters == null)
				{
					this._setters = new SetterBaseCollection();
				}
				return this._setters;
			}
		}

		/// <summary>Adds a child object. </summary>
		/// <param name="value">The child object to add.</param>
		// Token: 0x06000C7E RID: 3198 RVA: 0x0002EFE3 File Offset: 0x0002D1E3
		void IAddChild.AddChild(object value)
		{
			base.VerifyAccess();
			this.Setters.Add(Trigger.CheckChildIsSetter(value));
		}

		/// <summary>Adds the text content of a node to the object. </summary>
		/// <param name="text">The text to add to the object.</param>
		// Token: 0x06000C7F RID: 3199 RVA: 0x0000A446 File Offset: 0x00008646
		void IAddChild.AddText(string text)
		{
			base.VerifyAccess();
			XamlSerializerUtil.ThrowIfNonWhiteSpaceInAddText(text, this);
		}

		// Token: 0x06000C80 RID: 3200 RVA: 0x0002EFFC File Offset: 0x0002D1FC
		internal static Setter CheckChildIsSetter(object o)
		{
			if (o == null)
			{
				throw new ArgumentNullException("o");
			}
			Setter setter = o as Setter;
			if (setter == null)
			{
				throw new ArgumentException(SR.Get("UnexpectedParameterType", new object[]
				{
					o.GetType(),
					typeof(Setter)
				}), "o");
			}
			return setter;
		}

		// Token: 0x06000C81 RID: 3201 RVA: 0x0002F054 File Offset: 0x0002D254
		internal sealed override void Seal()
		{
			if (base.IsSealed)
			{
				return;
			}
			if (this._property != null && !this._property.IsValidValue(this._value))
			{
				throw new InvalidOperationException(SR.Get("InvalidPropertyValue", new object[]
				{
					this._value,
					this._property.Name
				}));
			}
			StyleHelper.SealIfSealable(this._value);
			base.ProcessSettersCollection(this._setters);
			base.TriggerConditions = new TriggerCondition[]
			{
				new TriggerCondition(this._property, LogicalOp.Equals, this._value, (this._sourceName != null) ? this._sourceName : "~Self")
			};
			for (int i = 0; i < this.PropertyValues.Count; i++)
			{
				PropertyValue value = this.PropertyValues[i];
				value.Conditions = base.TriggerConditions;
				this.PropertyValues[i] = value;
			}
			base.Seal();
		}

		// Token: 0x06000C82 RID: 3202 RVA: 0x0002F146 File Offset: 0x0002D346
		internal override bool GetCurrentState(DependencyObject container, UncommonField<HybridDictionary[]> dataField)
		{
			return base.TriggerConditions[0].Match(container.GetValue(base.TriggerConditions[0].Property));
		}

		/// <summary>Signals the object that initialization is starting.</summary>
		// Token: 0x06000C83 RID: 3203 RVA: 0x00002137 File Offset: 0x00000337
		void ISupportInitialize.BeginInit()
		{
		}

		/// <summary>Signals the object that initialization is complete.</summary>
		// Token: 0x06000C84 RID: 3204 RVA: 0x0002F170 File Offset: 0x0002D370
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

		/// <summary>Handles cases where a type converter provides a value for a property of a <see cref="T:System.Windows.Trigger" /> object.</summary>
		/// <param name="targetObject">The object where the type converter sets the value.</param>
		/// <param name="eventArgs">Data that is relevant for type converter processing.</param>
		// Token: 0x06000C85 RID: 3205 RVA: 0x0002F20C File Offset: 0x0002D40C
		public static void ReceiveTypeConverter(object targetObject, XamlSetTypeConverterEventArgs eventArgs)
		{
			Trigger trigger = targetObject as Trigger;
			if (trigger == null)
			{
				throw new ArgumentNullException("targetObject");
			}
			if (eventArgs == null)
			{
				throw new ArgumentNullException("eventArgs");
			}
			if (eventArgs.Member.Name == "Property")
			{
				trigger._unresolvedProperty = eventArgs.Value;
				trigger._serviceProvider = eventArgs.ServiceProvider;
				trigger._cultureInfoForTypeConverter = eventArgs.CultureInfo;
				eventArgs.Handled = true;
				return;
			}
			if (eventArgs.Member.Name == "Value")
			{
				trigger._unresolvedValue = eventArgs.Value;
				trigger._serviceProvider = eventArgs.ServiceProvider;
				trigger._cultureInfoForTypeConverter = eventArgs.CultureInfo;
				eventArgs.Handled = true;
			}
		}

		// Token: 0x04000B0B RID: 2827
		private DependencyProperty _property;

		// Token: 0x04000B0C RID: 2828
		private object _value = DependencyProperty.UnsetValue;

		// Token: 0x04000B0D RID: 2829
		private string _sourceName;

		// Token: 0x04000B0E RID: 2830
		private SetterBaseCollection _setters;

		// Token: 0x04000B0F RID: 2831
		private object _unresolvedProperty;

		// Token: 0x04000B10 RID: 2832
		private object _unresolvedValue;

		// Token: 0x04000B11 RID: 2833
		private ITypeDescriptorContext _serviceProvider;

		// Token: 0x04000B12 RID: 2834
		private CultureInfo _cultureInfoForTypeConverter;
	}
}
