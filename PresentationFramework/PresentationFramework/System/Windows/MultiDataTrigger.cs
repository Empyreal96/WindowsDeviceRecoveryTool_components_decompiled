using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Markup;

namespace System.Windows
{
	/// <summary>Represents a trigger that applies property values or performs actions when the bound data meet a set of conditions. </summary>
	// Token: 0x020000DD RID: 221
	[ContentProperty("Setters")]
	public sealed class MultiDataTrigger : TriggerBase, IAddChild
	{
		/// <summary>Gets a collection of <see cref="T:System.Windows.Condition" /> objects. Changes to property values are applied when all the conditions in the collection are met.</summary>
		/// <returns>A collection of <see cref="T:System.Windows.Condition" /> objects. The default is an empty collection.</returns>
		// Token: 0x1700018A RID: 394
		// (get) Token: 0x0600078C RID: 1932 RVA: 0x000177F7 File Offset: 0x000159F7
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ConditionCollection Conditions
		{
			get
			{
				base.VerifyAccess();
				return this._conditions;
			}
		}

		/// <summary>Gets a collection of <see cref="T:System.Windows.Setter" /> objects that describe the property values to apply when all the conditions of the <see cref="T:System.Windows.MultiDataTrigger" /> are met.</summary>
		/// <returns>A collection of <see cref="T:System.Windows.Setter" /> objects. The default value is an empty collection.</returns>
		// Token: 0x1700018B RID: 395
		// (get) Token: 0x0600078D RID: 1933 RVA: 0x00017805 File Offset: 0x00015A05
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
		// Token: 0x0600078E RID: 1934 RVA: 0x00017826 File Offset: 0x00015A26
		void IAddChild.AddChild(object value)
		{
			base.VerifyAccess();
			this.Setters.Add(Trigger.CheckChildIsSetter(value));
		}

		/// <summary>Adds the text content of a node to the object. </summary>
		/// <param name="text">The text to add to the object.</param>
		// Token: 0x0600078F RID: 1935 RVA: 0x0000A446 File Offset: 0x00008646
		void IAddChild.AddText(string text)
		{
			base.VerifyAccess();
			XamlSerializerUtil.ThrowIfNonWhiteSpaceInAddText(text, this);
		}

		// Token: 0x06000790 RID: 1936 RVA: 0x00017840 File Offset: 0x00015A40
		internal override void Seal()
		{
			if (base.IsSealed)
			{
				return;
			}
			base.ProcessSettersCollection(this._setters);
			if (this._conditions.Count > 0)
			{
				this._conditions.Seal(ValueLookupType.DataTrigger);
			}
			base.TriggerConditions = new TriggerCondition[this._conditions.Count];
			for (int i = 0; i < base.TriggerConditions.Length; i++)
			{
				if (this._conditions[i].SourceName != null && this._conditions[i].SourceName.Length > 0)
				{
					throw new InvalidOperationException(SR.Get("SourceNameNotSupportedForDataTriggers"));
				}
				base.TriggerConditions[i] = new TriggerCondition(this._conditions[i].Binding, LogicalOp.Equals, this._conditions[i].Value);
			}
			for (int j = 0; j < this.PropertyValues.Count; j++)
			{
				PropertyValue propertyValue = this.PropertyValues[j];
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
				this.PropertyValues[j] = propertyValue;
			}
			base.Seal();
		}

		// Token: 0x06000791 RID: 1937 RVA: 0x000179A4 File Offset: 0x00015BA4
		internal override bool GetCurrentState(DependencyObject container, UncommonField<HybridDictionary[]> dataField)
		{
			bool flag = base.TriggerConditions.Length != 0;
			int num = 0;
			while (flag && num < base.TriggerConditions.Length)
			{
				flag = base.TriggerConditions[num].ConvertAndMatch(StyleHelper.GetDataTriggerValue(dataField, container, base.TriggerConditions[num].Binding));
				num++;
			}
			return flag;
		}

		// Token: 0x04000760 RID: 1888
		private ConditionCollection _conditions = new ConditionCollection();

		// Token: 0x04000761 RID: 1889
		private SetterBaseCollection _setters;
	}
}
