using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Markup;

namespace System.Windows
{
	/// <summary>Represents a trigger that applies property values or performs actions when the bound data meets a specified condition.</summary>
	// Token: 0x020000AB RID: 171
	[ContentProperty("Setters")]
	[XamlSetMarkupExtension("ReceiveMarkupExtension")]
	public class DataTrigger : TriggerBase, IAddChild
	{
		/// <summary>Gets or sets the binding that produces the property value of the data object.</summary>
		/// <returns>The default value is <see langword="null" />.</returns>
		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000391 RID: 913 RVA: 0x0000A336 File Offset: 0x00008536
		// (set) Token: 0x06000392 RID: 914 RVA: 0x0000A344 File Offset: 0x00008544
		[Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
		public BindingBase Binding
		{
			get
			{
				base.VerifyAccess();
				return this._binding;
			}
			set
			{
				base.VerifyAccess();
				if (base.IsSealed)
				{
					throw new InvalidOperationException(SR.Get("CannotChangeAfterSealed", new object[]
					{
						"DataTrigger"
					}));
				}
				this._binding = value;
			}
		}

		/// <summary>Gets or sets the value to be compared with the property value of the data object.</summary>
		/// <returns>The default value is <see langword="null" />. See also the Exceptions section.</returns>
		/// <exception cref="T:System.ArgumentException">Only load-time <see cref="T:System.Windows.Markup.MarkupExtension" />s are supported.</exception>
		/// <exception cref="T:System.ArgumentException">Expressions are not supported. Bindings are not supported.</exception>
		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000393 RID: 915 RVA: 0x0000A379 File Offset: 0x00008579
		// (set) Token: 0x06000394 RID: 916 RVA: 0x0000A388 File Offset: 0x00008588
		[DependsOn("Binding")]
		[Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
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
						"DataTrigger"
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

		/// <summary>Gets a collection of <see cref="T:System.Windows.Setter" /> objects, which describe the property values to apply when the data item meets the specified condition.</summary>
		/// <returns>The default value is <see langword="null" />.</returns>
		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000395 RID: 917 RVA: 0x0000A40C File Offset: 0x0000860C
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
		// Token: 0x06000396 RID: 918 RVA: 0x0000A42D File Offset: 0x0000862D
		void IAddChild.AddChild(object value)
		{
			base.VerifyAccess();
			this.Setters.Add(Trigger.CheckChildIsSetter(value));
		}

		/// <summary>Adds the text content of a node to the object. </summary>
		/// <param name="text">The text to add to the object.</param>
		// Token: 0x06000397 RID: 919 RVA: 0x0000A446 File Offset: 0x00008646
		void IAddChild.AddText(string text)
		{
			base.VerifyAccess();
			XamlSerializerUtil.ThrowIfNonWhiteSpaceInAddText(text, this);
		}

		// Token: 0x06000398 RID: 920 RVA: 0x0000A458 File Offset: 0x00008658
		internal sealed override void Seal()
		{
			if (base.IsSealed)
			{
				return;
			}
			base.ProcessSettersCollection(this._setters);
			StyleHelper.SealIfSealable(this._value);
			base.TriggerConditions = new TriggerCondition[]
			{
				new TriggerCondition(this._binding, LogicalOp.Equals, this._value)
			};
			for (int i = 0; i < this.PropertyValues.Count; i++)
			{
				PropertyValue propertyValue = this.PropertyValues[i];
				propertyValue.Conditions = base.TriggerConditions;
				PropertyValueType valueType = propertyValue.ValueType;
				if (valueType != PropertyValueType.Trigger)
				{
					if (valueType != PropertyValueType.PropertyTriggerResource)
					{
						throw new InvalidOperationException(SR.Get("UnexpectedValueTypeForDataTrigger", new object[]
						{
							propertyValue.ValueType
						}));
					}
					propertyValue.ValueType = PropertyValueType.DataTriggerResource;
				}
				else
				{
					propertyValue.ValueType = PropertyValueType.DataTrigger;
				}
				this.PropertyValues[i] = propertyValue;
			}
			base.Seal();
		}

		// Token: 0x06000399 RID: 921 RVA: 0x0000A539 File Offset: 0x00008739
		internal override bool GetCurrentState(DependencyObject container, UncommonField<HybridDictionary[]> dataField)
		{
			return base.TriggerConditions[0].ConvertAndMatch(StyleHelper.GetDataTriggerValue(dataField, container, base.TriggerConditions[0].Binding));
		}

		/// <summary>Handles cases where a markup extension provides a value for a property of a <see cref="T:System.Windows.DataTrigger" /> object.</summary>
		/// <param name="targetObject">The object where the markup extension sets the value.</param>
		/// <param name="eventArgs">Data that is relevant for markup extension processing.</param>
		// Token: 0x0600039A RID: 922 RVA: 0x0000A564 File Offset: 0x00008764
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
			DataTrigger dataTrigger = targetObject as DataTrigger;
			if (dataTrigger != null && eventArgs.Member.Name == "Binding" && eventArgs.MarkupExtension is BindingBase)
			{
				dataTrigger.Binding = (eventArgs.MarkupExtension as BindingBase);
				eventArgs.Handled = true;
				return;
			}
			eventArgs.CallBase();
		}

		// Token: 0x040005F0 RID: 1520
		private BindingBase _binding;

		// Token: 0x040005F1 RID: 1521
		private object _value = DependencyProperty.UnsetValue;

		// Token: 0x040005F2 RID: 1522
		private SetterBaseCollection _setters;
	}
}
