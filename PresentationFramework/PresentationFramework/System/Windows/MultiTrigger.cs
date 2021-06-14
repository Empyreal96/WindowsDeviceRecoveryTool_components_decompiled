using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Markup;

namespace System.Windows
{
	/// <summary>Represents a trigger that applies property values or performs actions when a set of conditions are satisfied.</summary>
	// Token: 0x020000DE RID: 222
	[ContentProperty("Setters")]
	public sealed class MultiTrigger : TriggerBase, IAddChild
	{
		/// <summary>Gets a collection of <see cref="T:System.Windows.Condition" /> objects. Changes to property values are applied when all of the conditions in the collection are met.</summary>
		/// <returns>The default is an empty collection.</returns>
		// Token: 0x1700018C RID: 396
		// (get) Token: 0x06000793 RID: 1939 RVA: 0x00017A10 File Offset: 0x00015C10
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ConditionCollection Conditions
		{
			get
			{
				base.VerifyAccess();
				return this._conditions;
			}
		}

		/// <summary>Gets a collection of <see cref="T:System.Windows.Setter" /> objects, which describe the property values to apply when all of the conditions of the <see cref="T:System.Windows.MultiTrigger" /> are met.</summary>
		/// <returns>The default value is null.</returns>
		// Token: 0x1700018D RID: 397
		// (get) Token: 0x06000794 RID: 1940 RVA: 0x00017A1E File Offset: 0x00015C1E
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
		// Token: 0x06000795 RID: 1941 RVA: 0x00017A3F File Offset: 0x00015C3F
		void IAddChild.AddChild(object value)
		{
			base.VerifyAccess();
			this.Setters.Add(Trigger.CheckChildIsSetter(value));
		}

		/// <summary>Adds the text content of a node to the object. </summary>
		/// <param name="text">The text to add to the object.</param>
		// Token: 0x06000796 RID: 1942 RVA: 0x0000A446 File Offset: 0x00008646
		void IAddChild.AddText(string text)
		{
			base.VerifyAccess();
			XamlSerializerUtil.ThrowIfNonWhiteSpaceInAddText(text, this);
		}

		// Token: 0x06000797 RID: 1943 RVA: 0x00017A58 File Offset: 0x00015C58
		internal override void Seal()
		{
			if (base.IsSealed)
			{
				return;
			}
			base.ProcessSettersCollection(this._setters);
			if (this._conditions.Count > 0)
			{
				this._conditions.Seal(ValueLookupType.Trigger);
			}
			base.TriggerConditions = new TriggerCondition[this._conditions.Count];
			for (int i = 0; i < base.TriggerConditions.Length; i++)
			{
				base.TriggerConditions[i] = new TriggerCondition(this._conditions[i].Property, LogicalOp.Equals, this._conditions[i].Value, (this._conditions[i].SourceName != null) ? this._conditions[i].SourceName : "~Self");
			}
			for (int j = 0; j < this.PropertyValues.Count; j++)
			{
				PropertyValue value = this.PropertyValues[j];
				value.Conditions = base.TriggerConditions;
				this.PropertyValues[j] = value;
			}
			base.Seal();
		}

		// Token: 0x06000798 RID: 1944 RVA: 0x00017B60 File Offset: 0x00015D60
		internal override bool GetCurrentState(DependencyObject container, UncommonField<HybridDictionary[]> dataField)
		{
			bool flag = base.TriggerConditions.Length != 0;
			int num = 0;
			while (flag && num < base.TriggerConditions.Length)
			{
				flag = base.TriggerConditions[num].Match(container.GetValue(base.TriggerConditions[num].Property));
				num++;
			}
			return flag;
		}

		// Token: 0x04000762 RID: 1890
		private ConditionCollection _conditions = new ConditionCollection();

		// Token: 0x04000763 RID: 1891
		private SetterBaseCollection _setters;
	}
}
