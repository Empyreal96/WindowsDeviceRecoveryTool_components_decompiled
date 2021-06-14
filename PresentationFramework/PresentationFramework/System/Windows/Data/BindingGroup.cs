using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Markup;
using MS.Internal;
using MS.Internal.Controls;
using MS.Internal.Data;

namespace System.Windows.Data
{
	/// <summary>Contains a collection of bindings and <see cref="T:System.Windows.Controls.ValidationRule" /> objects that are used to validate an object.</summary>
	// Token: 0x020001A1 RID: 417
	public class BindingGroup : DependencyObject
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Data.BindingGroup" /> class. </summary>
		// Token: 0x0600196A RID: 6506 RVA: 0x00079550 File Offset: 0x00077750
		public BindingGroup()
		{
			this._validationRules = new ValidationRuleCollection();
			this.Initialize();
		}

		// Token: 0x0600196B RID: 6507 RVA: 0x000795B0 File Offset: 0x000777B0
		internal BindingGroup(BindingGroup master)
		{
			this._validationRules = master._validationRules;
			this._name = master._name;
			this._notifyOnValidationError = master._notifyOnValidationError;
			this._sharesProposedValues = master._sharesProposedValues;
			this._validatesOnNotifyDataError = master._validatesOnNotifyDataError;
			this.Initialize();
		}

		// Token: 0x0600196C RID: 6508 RVA: 0x00079640 File Offset: 0x00077840
		private void Initialize()
		{
			this._engine = DataBindEngine.CurrentDataBindEngine;
			this._bindingExpressions = new BindingGroup.BindingExpressionCollection();
			((INotifyCollectionChanged)this._bindingExpressions).CollectionChanged += this.OnBindingsChanged;
			this._itemsRW = new Collection<WeakReference>();
			this._items = new WeakReadOnlyCollection<object>(this._itemsRW);
		}

		/// <summary>Gets the object that this <see cref="T:System.Windows.Data.BindingGroup" /> is assigned to.</summary>
		/// <returns>The object that this <see cref="T:System.Windows.Data.BindingGroup" /> is assigned to.</returns>
		// Token: 0x170005D4 RID: 1492
		// (get) Token: 0x0600196D RID: 6509 RVA: 0x00079696 File Offset: 0x00077896
		public DependencyObject Owner
		{
			get
			{
				return this.InheritanceContext;
			}
		}

		/// <summary>Gets a collection of <see cref="T:System.Windows.Controls.ValidationRule" /> objects that validate the source objects in the <see cref="T:System.Windows.Data.BindingGroup" />.</summary>
		/// <returns>A collection of <see cref="T:System.Windows.Controls.ValidationRule" /> objects that validate the source objects in the <see cref="T:System.Windows.Data.BindingGroup" />. </returns>
		// Token: 0x170005D5 RID: 1493
		// (get) Token: 0x0600196E RID: 6510 RVA: 0x0007969E File Offset: 0x0007789E
		public Collection<ValidationRule> ValidationRules
		{
			get
			{
				return this._validationRules;
			}
		}

		/// <summary>Gets a collection of <see cref="T:System.Windows.Data.BindingExpression" /> objects that contains information for each Binding in the <see cref="T:System.Windows.Data.BindingGroup" />.</summary>
		/// <returns>A collection of <see cref="T:System.Windows.Data.BindingExpression" /> objects that contains information for each binding in the <see cref="T:System.Windows.Data.BindingGroup" />.</returns>
		// Token: 0x170005D6 RID: 1494
		// (get) Token: 0x0600196F RID: 6511 RVA: 0x000796A6 File Offset: 0x000778A6
		public Collection<BindingExpressionBase> BindingExpressions
		{
			get
			{
				return this._bindingExpressions;
			}
		}

		/// <summary>Gets or sets the name that identifies the <see cref="T:System.Windows.Data.BindingGroup" />, which can be used to include and exclude Binding objects in the <see cref="T:System.Windows.Data.BindingGroup" />.</summary>
		/// <returns>The name that identifies the <see cref="T:System.Windows.Data.BindingGroup" />.</returns>
		// Token: 0x170005D7 RID: 1495
		// (get) Token: 0x06001970 RID: 6512 RVA: 0x000796AE File Offset: 0x000778AE
		// (set) Token: 0x06001971 RID: 6513 RVA: 0x000796B6 File Offset: 0x000778B6
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				this._name = value;
			}
		}

		/// <summary>Gets or sets whether the <see cref="E:System.Windows.Controls.Validation.Error" /> event occurs when the state of a <see cref="T:System.Windows.Controls.ValidationRule" /> changes.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="E:System.Windows.Controls.Validation.Error" /> event occurs when the state of a <see cref="T:System.Windows.Controls.ValidationRule" /> changes; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x170005D8 RID: 1496
		// (get) Token: 0x06001972 RID: 6514 RVA: 0x000796BF File Offset: 0x000778BF
		// (set) Token: 0x06001973 RID: 6515 RVA: 0x000796C7 File Offset: 0x000778C7
		public bool NotifyOnValidationError
		{
			get
			{
				return this._notifyOnValidationError;
			}
			set
			{
				this._notifyOnValidationError = value;
			}
		}

		/// <summary>Gets or sets a value that indicates whether to include the <see cref="T:System.Windows.Controls.NotifyDataErrorValidationRule" />.</summary>
		/// <returns>
		///     <see langword="true" /> to include the <see cref="T:System.Windows.Controls.NotifyDataErrorValidationRule" />; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x170005D9 RID: 1497
		// (get) Token: 0x06001974 RID: 6516 RVA: 0x000796D0 File Offset: 0x000778D0
		// (set) Token: 0x06001975 RID: 6517 RVA: 0x000796D8 File Offset: 0x000778D8
		public bool ValidatesOnNotifyDataError
		{
			get
			{
				return this._validatesOnNotifyDataError;
			}
			set
			{
				this._validatesOnNotifyDataError = value;
			}
		}

		/// <summary>Gets or sets a value that indicates whether the <see cref="T:System.Windows.Data.BindingGroup" /> reuses target values that have not been committed to the source.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Data.BindingGroup" /> reuses target values that have not been committed to the source; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x170005DA RID: 1498
		// (get) Token: 0x06001976 RID: 6518 RVA: 0x000796E1 File Offset: 0x000778E1
		// (set) Token: 0x06001977 RID: 6519 RVA: 0x000796E9 File Offset: 0x000778E9
		public bool SharesProposedValues
		{
			get
			{
				return this._sharesProposedValues;
			}
			set
			{
				if (this._sharesProposedValues != value)
				{
					this._proposedValueTable.Clear();
					this._sharesProposedValues = value;
				}
			}
		}

		/// <summary>Gets whether each source in the binding can discard pending changes and restore the original values.</summary>
		/// <returns>
		///     <see langword="true" /> if each source in the binding can discard pending changes and restore the original values; otherwise, <see langword="false" />.</returns>
		// Token: 0x170005DB RID: 1499
		// (get) Token: 0x06001978 RID: 6520 RVA: 0x00079708 File Offset: 0x00077908
		public bool CanRestoreValues
		{
			get
			{
				IList items = this.Items;
				for (int i = items.Count - 1; i >= 0; i--)
				{
					if (!(items[i] is IEditableObject))
					{
						return false;
					}
				}
				return true;
			}
		}

		/// <summary>Gets the sources that are used by the Binding objects in the <see cref="T:System.Windows.Data.BindingGroup" />.</summary>
		/// <returns>The sources that are used by the Binding objects in the <see cref="T:System.Windows.Data.BindingGroup" />.</returns>
		// Token: 0x170005DC RID: 1500
		// (get) Token: 0x06001979 RID: 6521 RVA: 0x00079740 File Offset: 0x00077940
		public IList Items
		{
			get
			{
				this.EnsureItems();
				return this._items;
			}
		}

		/// <summary>Gets or sets a value that indicates whether the <see cref="T:System.Windows.Data.BindingGroup" /> contains a proposed value that has not been written to the source.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Data.BindingGroup" /> contains a proposed value that has not been written to the source; otherwise, <see langword="false" />.</returns>
		// Token: 0x170005DD RID: 1501
		// (get) Token: 0x0600197A RID: 6522 RVA: 0x00079750 File Offset: 0x00077950
		public bool IsDirty
		{
			get
			{
				if (this._proposedValueTable.Count > 0)
				{
					return true;
				}
				foreach (BindingExpressionBase bindingExpressionBase in this._bindingExpressions)
				{
					if (bindingExpressionBase.IsDirty)
					{
						return true;
					}
				}
				return false;
			}
		}

		/// <summary>Gets a value that indicates whether the <see cref="T:System.Windows.Data.BindingGroup" /> has a failed validation rule.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Data.BindingGroup" /> has a failed validation rule; otherwise, <see langword="false" />.</returns>
		// Token: 0x170005DE RID: 1502
		// (get) Token: 0x0600197B RID: 6523 RVA: 0x000797B8 File Offset: 0x000779B8
		public bool HasValidationError
		{
			get
			{
				ValidationErrorCollection validationErrorCollection;
				bool flag;
				return this.GetValidationErrors(out validationErrorCollection, out flag);
			}
		}

		/// <summary>Gets a collection of <see cref="T:System.Windows.Controls.ValidationError" /> objects that caused the <see cref="T:System.Windows.Data.BindingGroup" /> to be invalid.</summary>
		/// <returns>A collection of <see cref="T:System.Windows.Controls.ValidationError" /> objects that caused <see cref="T:System.Windows.Data.BindingGroup" /> to be invalid.  The value is <see langword="null" /> if there are no errors.</returns>
		// Token: 0x170005DF RID: 1503
		// (get) Token: 0x0600197C RID: 6524 RVA: 0x000797D0 File Offset: 0x000779D0
		public ReadOnlyCollection<ValidationError> ValidationErrors
		{
			get
			{
				ValidationErrorCollection validationErrorCollection;
				bool flag;
				if (!this.GetValidationErrors(out validationErrorCollection, out flag))
				{
					return null;
				}
				if (flag)
				{
					return new ReadOnlyCollection<ValidationError>(validationErrorCollection);
				}
				List<ValidationError> list = new List<ValidationError>();
				foreach (ValidationError validationError in validationErrorCollection)
				{
					if (this.Belongs(validationError))
					{
						list.Add(validationError);
					}
				}
				return new ReadOnlyCollection<ValidationError>(list);
			}
		}

		// Token: 0x0600197D RID: 6525 RVA: 0x00079848 File Offset: 0x00077A48
		private bool GetValidationErrors(out ValidationErrorCollection superset, out bool isPure)
		{
			superset = null;
			isPure = true;
			DependencyObject dependencyObject = Helper.FindMentor(this);
			if (dependencyObject == null)
			{
				return false;
			}
			superset = Validation.GetErrorsInternal(dependencyObject);
			if (superset == null || superset.Count == 0)
			{
				return false;
			}
			for (int i = superset.Count - 1; i >= 0; i--)
			{
				ValidationError error = superset[i];
				if (!this.Belongs(error))
				{
					isPure = false;
					break;
				}
			}
			return true;
		}

		// Token: 0x0600197E RID: 6526 RVA: 0x000798AC File Offset: 0x00077AAC
		private bool Belongs(ValidationError error)
		{
			BindingExpressionBase bindingExpressionBase;
			return error.BindingInError == this || this._proposedValueTable.HasValidationError(error) || ((bindingExpressionBase = (error.BindingInError as BindingExpressionBase)) != null && bindingExpressionBase.BindingGroup == this);
		}

		// Token: 0x170005E0 RID: 1504
		// (get) Token: 0x0600197F RID: 6527 RVA: 0x000798EC File Offset: 0x00077AEC
		private DataBindEngine Engine
		{
			get
			{
				return this._engine;
			}
		}

		/// <summary>Begins an edit transaction on the sources in the <see cref="T:System.Windows.Data.BindingGroup" />.</summary>
		// Token: 0x06001980 RID: 6528 RVA: 0x000798F4 File Offset: 0x00077AF4
		public void BeginEdit()
		{
			if (!this.IsEditing)
			{
				IList items = this.Items;
				for (int i = items.Count - 1; i >= 0; i--)
				{
					IEditableObject editableObject = items[i] as IEditableObject;
					if (editableObject != null)
					{
						editableObject.BeginEdit();
					}
				}
				this.IsEditing = true;
			}
		}

		/// <summary>Runs all the <see cref="T:System.Windows.Controls.ValidationRule" /> objects and updates the binding sources if all validation rules succeed.</summary>
		/// <returns>
		///     <see langword="true" /> if every <see cref="T:System.Windows.Controls.ValidationRule" /> succeeds and the values are committed to the sources; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001981 RID: 6529 RVA: 0x00079940 File Offset: 0x00077B40
		public bool CommitEdit()
		{
			bool flag = this.UpdateAndValidate(ValidationStep.CommittedValue);
			this.IsEditing = (this.IsEditing && !flag);
			return flag;
		}

		/// <summary>Ends the edit transaction and discards the pending changes.</summary>
		// Token: 0x06001982 RID: 6530 RVA: 0x0007996C File Offset: 0x00077B6C
		public void CancelEdit()
		{
			this.ClearValidationErrors();
			IList items = this.Items;
			for (int i = items.Count - 1; i >= 0; i--)
			{
				IEditableObject editableObject = items[i] as IEditableObject;
				if (editableObject != null)
				{
					editableObject.CancelEdit();
				}
			}
			for (int j = this._bindingExpressions.Count - 1; j >= 0; j--)
			{
				this._bindingExpressions[j].UpdateTarget();
			}
			this._proposedValueTable.UpdateDependents();
			this._proposedValueTable.Clear();
			this.IsEditing = false;
		}

		/// <summary>Runs the converter on the binding and the <see cref="T:System.Windows.Controls.ValidationRule" /> objects that have the <see cref="P:System.Windows.Controls.ValidationRule.ValidationStep" /> property set to <see cref="F:System.Windows.Controls.ValidationStep.RawProposedValue" /> or <see cref="F:System.Windows.Controls.ValidationStep.ConvertedProposedValue" />.</summary>
		/// <returns>
		///     <see langword="true" /> if the validation rules succeed; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001983 RID: 6531 RVA: 0x000799F5 File Offset: 0x00077BF5
		public bool ValidateWithoutUpdate()
		{
			return this.UpdateAndValidate(ValidationStep.ConvertedProposedValue);
		}

		/// <summary>Runs the converter on the binding and the <see cref="T:System.Windows.Controls.ValidationRule" /> objects that have the <see cref="P:System.Windows.Controls.ValidationRule.ValidationStep" /> property set to <see cref="F:System.Windows.Controls.ValidationStep.RawProposedValue" />, <see cref="F:System.Windows.Controls.ValidationStep.ConvertedProposedValue" />, or <see cref="F:System.Windows.Controls.ValidationStep.UpdatedValue" /> and saves the values of the targets to the source objects if all the validation rules succeed.</summary>
		/// <returns>
		///     <see langword="true" /> if all validation rules succeed; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001984 RID: 6532 RVA: 0x000799FE File Offset: 0x00077BFE
		public bool UpdateSources()
		{
			return this.UpdateAndValidate(ValidationStep.UpdatedValue);
		}

		/// <summary>Returns the proposed value for the specified property and item.</summary>
		/// <param name="item">The object that contains the specified property.</param>
		/// <param name="propertyName">The property whose proposed value to get.</param>
		/// <returns>The proposed property value. </returns>
		/// <exception cref="T:System.InvalidOperationException">There is not a binding for the specified item and property.</exception>
		/// <exception cref="T:System.Windows.Data.ValueUnavailableException">The value of the specified property is not available, due to a conversion error or because an earlier validation rule failed.</exception>
		// Token: 0x06001985 RID: 6533 RVA: 0x00079A08 File Offset: 0x00077C08
		public object GetValue(object item, string propertyName)
		{
			object obj;
			if (this.TryGetValueImpl(item, propertyName, out obj))
			{
				return obj;
			}
			if (obj == Binding.DoNothing)
			{
				throw new ValueUnavailableException(SR.Get("BindingGroup_NoEntry", new object[]
				{
					item,
					propertyName
				}));
			}
			throw new ValueUnavailableException(SR.Get("BindingGroup_ValueUnavailable", new object[]
			{
				item,
				propertyName
			}));
		}

		/// <summary>Attempts to get the proposed value for the specified property and item.</summary>
		/// <param name="item">The object that contains the specified property.</param>
		/// <param name="propertyName">The property whose proposed value to get.</param>
		/// <param name="value">When this method returns, contains an object that represents the proposed property value. This parameter is passed uninitialized. </param>
		/// <returns>
		///     <see langword="true" /> if value is the proposed value for the specified property; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001986 RID: 6534 RVA: 0x00079A68 File Offset: 0x00077C68
		public bool TryGetValue(object item, string propertyName, out object value)
		{
			bool result = this.TryGetValueImpl(item, propertyName, out value);
			if (value == Binding.DoNothing)
			{
				value = DependencyProperty.UnsetValue;
			}
			return result;
		}

		// Token: 0x06001987 RID: 6535 RVA: 0x00079A90 File Offset: 0x00077C90
		private bool TryGetValueImpl(object item, string propertyName, out object value)
		{
			BindingGroup.GetValueTableEntry getValueTableEntry = this._getValueTable[item, propertyName];
			ValidationStep validationStep;
			if (getValueTableEntry == null)
			{
				BindingGroup.ProposedValueEntry proposedValueEntry = this._proposedValueTable[item, propertyName];
				if (proposedValueEntry != null)
				{
					validationStep = this._validationStep;
					if (validationStep == ValidationStep.RawProposedValue)
					{
						value = proposedValueEntry.RawValue;
						return true;
					}
					if (validationStep - ValidationStep.ConvertedProposedValue <= 2)
					{
						value = proposedValueEntry.ConvertedValue;
						return value != DependencyProperty.UnsetValue;
					}
				}
				value = Binding.DoNothing;
				return false;
			}
			validationStep = this._validationStep;
			if (validationStep <= ValidationStep.CommittedValue)
			{
				value = getValueTableEntry.Value;
			}
			else
			{
				value = getValueTableEntry.BindingExpressionBase.RootBindingExpression.GetRawProposedValue();
			}
			if (value == Binding.DoNothing)
			{
				BindingExpression bindingExpression = (BindingExpression)getValueTableEntry.BindingExpressionBase;
				value = bindingExpression.SourceValue;
			}
			return value != DependencyProperty.UnsetValue;
		}

		// Token: 0x170005E1 RID: 1505
		// (get) Token: 0x06001988 RID: 6536 RVA: 0x00079B4C File Offset: 0x00077D4C
		internal override DependencyObject InheritanceContext
		{
			get
			{
				DependencyObject dependencyObject;
				if (!this._inheritanceContext.TryGetTarget(out dependencyObject))
				{
					this.CheckDetach(dependencyObject);
				}
				return dependencyObject;
			}
		}

		// Token: 0x06001989 RID: 6537 RVA: 0x00079B70 File Offset: 0x00077D70
		internal override void AddInheritanceContext(DependencyObject context, DependencyProperty property)
		{
			if (property != null && property.PropertyType != typeof(BindingGroup) && TraceData.IsEnabled)
			{
				string text = (property != null) ? property.Name : "(null)";
				TraceData.Trace(TraceEventType.Warning, TraceData.BindingGroupWrongProperty(new object[]
				{
					text,
					context.GetType().FullName
				}));
			}
			DependencyObject dependencyObject;
			this._inheritanceContext.TryGetTarget(out dependencyObject);
			InheritanceContextHelper.AddInheritanceContext(context, this, ref this._hasMultipleInheritanceContexts, ref dependencyObject);
			this.CheckDetach(dependencyObject);
			this._inheritanceContext = ((dependencyObject == null) ? BindingGroup.NullInheritanceContext : new WeakReference<DependencyObject>(dependencyObject));
			if (property == FrameworkElement.BindingGroupProperty && !this._hasMultipleInheritanceContexts && (this.ValidatesOnDataTransfer || this.ValidatesOnNotifyDataError))
			{
				UIElement uielement = Helper.FindMentor(this) as UIElement;
				if (uielement != null)
				{
					uielement.LayoutUpdated += this.OnLayoutUpdated;
				}
			}
			if (this._hasMultipleInheritanceContexts && property != ItemsControl.ItemBindingGroupProperty && TraceData.IsEnabled)
			{
				TraceData.Trace(TraceEventType.Warning, TraceData.BindingGroupMultipleInheritance);
			}
		}

		// Token: 0x0600198A RID: 6538 RVA: 0x00079C70 File Offset: 0x00077E70
		internal override void RemoveInheritanceContext(DependencyObject context, DependencyProperty property)
		{
			DependencyObject dependencyObject;
			this._inheritanceContext.TryGetTarget(out dependencyObject);
			InheritanceContextHelper.RemoveInheritanceContext(context, this, ref this._hasMultipleInheritanceContexts, ref dependencyObject);
			this.CheckDetach(dependencyObject);
			this._inheritanceContext = ((dependencyObject == null) ? BindingGroup.NullInheritanceContext : new WeakReference<DependencyObject>(dependencyObject));
		}

		// Token: 0x170005E2 RID: 1506
		// (get) Token: 0x0600198B RID: 6539 RVA: 0x00079CB7 File Offset: 0x00077EB7
		internal override bool HasMultipleInheritanceContexts
		{
			get
			{
				return this._hasMultipleInheritanceContexts;
			}
		}

		// Token: 0x0600198C RID: 6540 RVA: 0x00079CBF File Offset: 0x00077EBF
		private void CheckDetach(DependencyObject newOwner)
		{
			if (newOwner != null || this._inheritanceContext == BindingGroup.NullInheritanceContext)
			{
				return;
			}
			this.Engine.CommitManager.RemoveBindingGroup(this);
		}

		// Token: 0x170005E3 RID: 1507
		// (get) Token: 0x0600198D RID: 6541 RVA: 0x00079CE3 File Offset: 0x00077EE3
		// (set) Token: 0x0600198E RID: 6542 RVA: 0x00079CEB File Offset: 0x00077EEB
		private bool IsEditing { get; set; }

		// Token: 0x170005E4 RID: 1508
		// (get) Token: 0x0600198F RID: 6543 RVA: 0x00079CF4 File Offset: 0x00077EF4
		// (set) Token: 0x06001990 RID: 6544 RVA: 0x00079CFC File Offset: 0x00077EFC
		private bool IsItemsValid
		{
			get
			{
				return this._isItemsValid;
			}
			set
			{
				this._isItemsValid = value;
				if (!value && (this.IsEditing || this.ValidatesOnNotifyDataError))
				{
					this.EnsureItems();
				}
			}
		}

		// Token: 0x06001991 RID: 6545 RVA: 0x00079D20 File Offset: 0x00077F20
		internal void UpdateTable(BindingExpression bindingExpression)
		{
			bool flag = this._getValueTable.Update(bindingExpression);
			if (flag)
			{
				this._proposedValueTable.Remove(bindingExpression);
			}
			this.IsItemsValid = false;
		}

		// Token: 0x06001992 RID: 6546 RVA: 0x00079D50 File Offset: 0x00077F50
		internal void AddToValueTable(BindingExpressionBase bindingExpressionBase)
		{
			this._getValueTable.EnsureEntry(bindingExpressionBase);
		}

		// Token: 0x06001993 RID: 6547 RVA: 0x00079D5E File Offset: 0x00077F5E
		internal object GetValue(BindingExpressionBase bindingExpressionBase)
		{
			return this._getValueTable.GetValue(bindingExpressionBase);
		}

		// Token: 0x06001994 RID: 6548 RVA: 0x00079D6C File Offset: 0x00077F6C
		internal void SetValue(BindingExpressionBase bindingExpressionBase, object value)
		{
			this._getValueTable.SetValue(bindingExpressionBase, value);
		}

		// Token: 0x06001995 RID: 6549 RVA: 0x00079D7B File Offset: 0x00077F7B
		internal void UseSourceValue(BindingExpressionBase bindingExpressionBase)
		{
			this._getValueTable.UseSourceValue(bindingExpressionBase);
		}

		// Token: 0x06001996 RID: 6550 RVA: 0x00079D89 File Offset: 0x00077F89
		internal BindingGroup.ProposedValueEntry GetProposedValueEntry(object item, string propertyName)
		{
			return this._proposedValueTable[item, propertyName];
		}

		// Token: 0x06001997 RID: 6551 RVA: 0x00079D98 File Offset: 0x00077F98
		internal void RemoveProposedValueEntry(BindingGroup.ProposedValueEntry entry)
		{
			this._proposedValueTable.Remove(entry);
		}

		// Token: 0x06001998 RID: 6552 RVA: 0x00079DA8 File Offset: 0x00077FA8
		internal void AddBindingForProposedValue(BindingExpressionBase dependent, object item, string propertyName)
		{
			BindingGroup.ProposedValueEntry proposedValueEntry = this._proposedValueTable[item, propertyName];
			if (proposedValueEntry != null)
			{
				proposedValueEntry.AddDependent(dependent);
			}
		}

		// Token: 0x06001999 RID: 6553 RVA: 0x00079DD0 File Offset: 0x00077FD0
		internal void AddValidationError(ValidationError validationError)
		{
			DependencyObject dependencyObject = Helper.FindMentor(this);
			if (dependencyObject == null)
			{
				return;
			}
			Validation.AddValidationError(validationError, dependencyObject, this.NotifyOnValidationError);
		}

		// Token: 0x0600199A RID: 6554 RVA: 0x00079DF8 File Offset: 0x00077FF8
		internal void RemoveValidationError(ValidationError validationError)
		{
			DependencyObject dependencyObject = Helper.FindMentor(this);
			if (dependencyObject == null)
			{
				return;
			}
			Validation.RemoveValidationError(validationError, dependencyObject, this.NotifyOnValidationError);
		}

		// Token: 0x0600199B RID: 6555 RVA: 0x00079E1D File Offset: 0x0007801D
		private void ClearValidationErrors(ValidationStep validationStep)
		{
			this.ClearValidationErrorsImpl(validationStep, false);
		}

		// Token: 0x0600199C RID: 6556 RVA: 0x00079E27 File Offset: 0x00078027
		private void ClearValidationErrors()
		{
			this.ClearValidationErrorsImpl(ValidationStep.RawProposedValue, true);
		}

		// Token: 0x0600199D RID: 6557 RVA: 0x00079E34 File Offset: 0x00078034
		private void ClearValidationErrorsImpl(ValidationStep validationStep, bool allSteps)
		{
			DependencyObject dependencyObject = Helper.FindMentor(this);
			if (dependencyObject == null)
			{
				return;
			}
			ValidationErrorCollection errorsInternal = Validation.GetErrorsInternal(dependencyObject);
			if (errorsInternal == null)
			{
				return;
			}
			for (int i = errorsInternal.Count - 1; i >= 0; i--)
			{
				ValidationError validationError = errorsInternal[i];
				if ((allSteps || validationError.RuleInError.ValidationStep == validationStep) && (validationError.BindingInError == this || this._proposedValueTable.HasValidationError(validationError)))
				{
					this.RemoveValidationError(validationError);
				}
			}
		}

		// Token: 0x0600199E RID: 6558 RVA: 0x00079EA4 File Offset: 0x000780A4
		private void EnsureItems()
		{
			if (this.IsItemsValid)
			{
				return;
			}
			IList<WeakReference> list = new Collection<WeakReference>();
			DependencyObject dependencyObject = Helper.FindMentor(this);
			if (dependencyObject != null)
			{
				object value = dependencyObject.GetValue(FrameworkElement.DataContextProperty);
				if (value != null && value != CollectionView.NewItemPlaceholder && value != BindingExpressionBase.DisconnectedItem)
				{
					WeakReference weakReference = (this._itemsRW.Count > 0) ? this._itemsRW[0] : null;
					if (weakReference == null || !ItemsControl.EqualsEx(value, weakReference.Target))
					{
						weakReference = new WeakReference(value);
					}
					list.Add(weakReference);
				}
			}
			this._getValueTable.AddUniqueItems(list);
			this._proposedValueTable.AddUniqueItems(list);
			for (int i = this._itemsRW.Count - 1; i >= 0; i--)
			{
				int num = BindingGroup.FindIndexOf(this._itemsRW[i], list);
				if (num >= 0)
				{
					list.RemoveAt(num);
				}
				else
				{
					if (this.ValidatesOnNotifyDataError)
					{
						INotifyDataErrorInfo notifyDataErrorInfo = this._itemsRW[i].Target as INotifyDataErrorInfo;
						if (notifyDataErrorInfo != null)
						{
							ErrorsChangedEventManager.RemoveHandler(notifyDataErrorInfo, new EventHandler<DataErrorsChangedEventArgs>(this.OnErrorsChanged));
						}
					}
					this._itemsRW.RemoveAt(i);
				}
			}
			for (int j = list.Count - 1; j >= 0; j--)
			{
				this._itemsRW.Add(list[j]);
				if (this.IsEditing)
				{
					IEditableObject editableObject = list[j].Target as IEditableObject;
					if (editableObject != null)
					{
						editableObject.BeginEdit();
					}
				}
				if (this.ValidatesOnNotifyDataError)
				{
					INotifyDataErrorInfo notifyDataErrorInfo = list[j].Target as INotifyDataErrorInfo;
					if (notifyDataErrorInfo != null)
					{
						ErrorsChangedEventManager.AddHandler(notifyDataErrorInfo, new EventHandler<DataErrorsChangedEventArgs>(this.OnErrorsChanged));
						this.UpdateNotifyDataErrors(notifyDataErrorInfo, list[j]);
					}
				}
			}
			this.IsItemsValid = true;
		}

		// Token: 0x170005E5 RID: 1509
		// (get) Token: 0x0600199F RID: 6559 RVA: 0x0007A060 File Offset: 0x00078260
		private bool ValidatesOnDataTransfer
		{
			get
			{
				if (this.ValidationRules != null)
				{
					for (int i = this.ValidationRules.Count - 1; i >= 0; i--)
					{
						if (this.ValidationRules[i].ValidatesOnTargetUpdated)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x060019A0 RID: 6560 RVA: 0x0007A0A4 File Offset: 0x000782A4
		private void OnLayoutUpdated(object sender, EventArgs e)
		{
			DependencyObject dependencyObject = Helper.FindMentor(this);
			UIElement uielement = dependencyObject as UIElement;
			if (uielement != null)
			{
				uielement.LayoutUpdated -= this.OnLayoutUpdated;
			}
			FrameworkElement frameworkElement;
			FrameworkContentElement frameworkContentElement;
			Helper.DowncastToFEorFCE(dependencyObject, out frameworkElement, out frameworkContentElement, false);
			if (frameworkElement != null)
			{
				frameworkElement.DataContextChanged += this.OnDataContextChanged;
			}
			else if (frameworkContentElement != null)
			{
				frameworkContentElement.DataContextChanged += this.OnDataContextChanged;
			}
			this.ValidateOnDataTransfer();
		}

		// Token: 0x060019A1 RID: 6561 RVA: 0x0007A111 File Offset: 0x00078311
		private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if (e.NewValue == BindingExpressionBase.DisconnectedItem)
			{
				return;
			}
			this.IsItemsValid = false;
			this.ValidateOnDataTransfer();
		}

		// Token: 0x060019A2 RID: 6562 RVA: 0x0007A130 File Offset: 0x00078330
		private void ValidateOnDataTransfer()
		{
			if (!this.ValidatesOnDataTransfer)
			{
				return;
			}
			DependencyObject dependencyObject = Helper.FindMentor(this);
			if (dependencyObject == null || this.ValidationRules.Count == 0)
			{
				return;
			}
			Collection<ValidationError> collection;
			if (!Validation.GetHasError(dependencyObject))
			{
				collection = null;
			}
			else
			{
				collection = new Collection<ValidationError>();
				ReadOnlyCollection<ValidationError> errors = Validation.GetErrors(dependencyObject);
				int i = 0;
				int count = errors.Count;
				while (i < count)
				{
					ValidationError validationError = errors[i];
					if (validationError.RuleInError.ValidatesOnTargetUpdated && validationError.BindingInError == this)
					{
						collection.Add(validationError);
					}
					i++;
				}
			}
			CultureInfo culture = this.GetCulture();
			int j = 0;
			int count2 = this.ValidationRules.Count;
			while (j < count2)
			{
				ValidationRule validationRule = this.ValidationRules[j];
				if (validationRule.ValidatesOnTargetUpdated)
				{
					try
					{
						ValidationResult validationResult = validationRule.Validate(DependencyProperty.UnsetValue, culture, this);
						if (!validationResult.IsValid)
						{
							this.AddValidationError(new ValidationError(validationRule, this, validationResult.ErrorContent, null));
						}
					}
					catch (ValueUnavailableException ex)
					{
						this.AddValidationError(new ValidationError(validationRule, this, ex.Message, ex));
					}
				}
				j++;
			}
			if (collection != null)
			{
				int k = 0;
				int count3 = collection.Count;
				while (k < count3)
				{
					this.RemoveValidationError(collection[k]);
					k++;
				}
			}
		}

		// Token: 0x060019A3 RID: 6563 RVA: 0x0007A280 File Offset: 0x00078480
		private bool UpdateAndValidate(ValidationStep validationStep)
		{
			DependencyObject dependencyObject = Helper.FindMentor(this);
			if (dependencyObject != null && dependencyObject.GetValue(FrameworkElement.DataContextProperty) == CollectionView.NewItemPlaceholder)
			{
				return true;
			}
			this.PrepareProposedValuesForUpdate(dependencyObject, validationStep >= ValidationStep.UpdatedValue);
			bool flag = true;
			this._validationStep = ValidationStep.RawProposedValue;
			while (this._validationStep <= validationStep && flag)
			{
				switch (this._validationStep)
				{
				case ValidationStep.RawProposedValue:
					this._getValueTable.ResetValues();
					break;
				case ValidationStep.ConvertedProposedValue:
					flag = this.ObtainConvertedProposedValues();
					break;
				case ValidationStep.UpdatedValue:
					flag = this.UpdateValues();
					break;
				case ValidationStep.CommittedValue:
					flag = this.CommitValues();
					break;
				}
				if (!this.CheckValidationRules())
				{
					flag = false;
				}
				this._validationStep++;
			}
			this.ResetProposedValuesAfterUpdate(dependencyObject, flag && validationStep == ValidationStep.CommittedValue);
			this._validationStep = (ValidationStep)(-1);
			this._getValueTable.ResetValues();
			this.NotifyCommitManager();
			return flag;
		}

		// Token: 0x060019A4 RID: 6564 RVA: 0x0007A35C File Offset: 0x0007855C
		private void UpdateNotifyDataErrors(INotifyDataErrorInfo indei, WeakReference itemWR)
		{
			if (itemWR == null)
			{
				int num = BindingGroup.FindIndexOf(indei, this._itemsRW);
				if (num < 0)
				{
					return;
				}
				itemWR = this._itemsRW[num];
			}
			List<object> dataErrors;
			try
			{
				dataErrors = BindingExpression.GetDataErrors(indei, string.Empty);
			}
			catch (Exception ex)
			{
				if (CriticalExceptions.IsCriticalApplicationException(ex))
				{
					throw;
				}
				return;
			}
			this.UpdateNotifyDataErrorValidationErrors(itemWR, dataErrors);
		}

		// Token: 0x060019A5 RID: 6565 RVA: 0x0007A3C0 File Offset: 0x000785C0
		private void UpdateNotifyDataErrorValidationErrors(WeakReference itemWR, List<object> errors)
		{
			List<ValidationError> list;
			if (!this._notifyDataErrors.TryGetValue(itemWR, out list))
			{
				list = null;
			}
			List<object> list2;
			List<ValidationError> list3;
			BindingExpressionBase.GetValidationDelta(list, errors, out list2, out list3);
			if (list2 != null && list2.Count > 0)
			{
				ValidationRule instance = NotifyDataErrorValidationRule.Instance;
				if (list == null)
				{
					list = new List<ValidationError>();
				}
				foreach (object errorContent in list2)
				{
					ValidationError validationError = new ValidationError(instance, this, errorContent, null);
					list.Add(validationError);
					this.AddValidationError(validationError);
				}
			}
			if (list3 != null && list3.Count > 0)
			{
				foreach (ValidationError validationError2 in list3)
				{
					list.Remove(validationError2);
					this.RemoveValidationError(validationError2);
				}
				if (list.Count == 0)
				{
					list = null;
				}
			}
			if (list == null)
			{
				this._notifyDataErrors.Remove(itemWR);
				return;
			}
			this._notifyDataErrors[itemWR] = list;
		}

		// Token: 0x060019A6 RID: 6566 RVA: 0x0007A4DC File Offset: 0x000786DC
		private bool ObtainConvertedProposedValues()
		{
			bool flag = true;
			for (int i = this._bindingExpressions.Count - 1; i >= 0; i--)
			{
				flag = (this._bindingExpressions[i].ObtainConvertedProposedValue(this) && flag);
			}
			return flag;
		}

		// Token: 0x060019A7 RID: 6567 RVA: 0x0007A51C File Offset: 0x0007871C
		private bool UpdateValues()
		{
			bool flag = true;
			for (int i = this._bindingExpressions.Count - 1; i >= 0; i--)
			{
				flag = (this._bindingExpressions[i].UpdateSource(this) && flag);
			}
			if (this._proposedValueBindingExpressions != null)
			{
				for (int j = this._proposedValueBindingExpressions.Length - 1; j >= 0; j--)
				{
					BindingExpression bindingExpression = this._proposedValueBindingExpressions[j];
					BindingGroup.ProposedValueEntry proposedValueEntry = this._proposedValueTable[bindingExpression];
					flag = (bindingExpression.UpdateSource(proposedValueEntry.ConvertedValue) != DependencyProperty.UnsetValue && flag);
				}
			}
			return flag;
		}

		// Token: 0x060019A8 RID: 6568 RVA: 0x0007A5A8 File Offset: 0x000787A8
		private bool CheckValidationRules()
		{
			bool result = true;
			this.ClearValidationErrors(this._validationStep);
			for (int i = this._bindingExpressions.Count - 1; i >= 0; i--)
			{
				if (!this._bindingExpressions[i].CheckValidationRules(this, this._validationStep))
				{
					result = false;
				}
			}
			if (this._validationStep >= ValidationStep.UpdatedValue && this._proposedValueBindingExpressions != null)
			{
				for (int j = this._proposedValueBindingExpressions.Length - 1; j >= 0; j--)
				{
					if (!this._proposedValueBindingExpressions[j].CheckValidationRules(this, this._validationStep))
					{
						result = false;
					}
				}
			}
			CultureInfo culture = this.GetCulture();
			int k = 0;
			int count = this._validationRules.Count;
			while (k < count)
			{
				ValidationRule validationRule = this._validationRules[k];
				if (validationRule.ValidationStep == this._validationStep)
				{
					try
					{
						ValidationResult validationResult = validationRule.Validate(DependencyProperty.UnsetValue, culture, this);
						if (!validationResult.IsValid)
						{
							this.AddValidationError(new ValidationError(validationRule, this, validationResult.ErrorContent, null));
							result = false;
						}
					}
					catch (ValueUnavailableException ex)
					{
						this.AddValidationError(new ValidationError(validationRule, this, ex.Message, ex));
						result = false;
					}
				}
				k++;
			}
			return result;
		}

		// Token: 0x060019A9 RID: 6569 RVA: 0x0007A6DC File Offset: 0x000788DC
		private bool CommitValues()
		{
			bool result = true;
			IList items = this.Items;
			for (int i = items.Count - 1; i >= 0; i--)
			{
				IEditableObject editableObject = items[i] as IEditableObject;
				if (editableObject != null)
				{
					try
					{
						editableObject.EndEdit();
					}
					catch (Exception ex)
					{
						if (CriticalExceptions.IsCriticalApplicationException(ex))
						{
							throw;
						}
						ValidationError validationError = new ValidationError(ExceptionValidationRule.Instance, this, ex.Message, ex);
						this.AddValidationError(validationError);
						result = false;
					}
				}
			}
			return result;
		}

		// Token: 0x060019AA RID: 6570 RVA: 0x0007A760 File Offset: 0x00078960
		private static int FindIndexOf(WeakReference wr, IList<WeakReference> list)
		{
			object target = wr.Target;
			if (target == null)
			{
				return -1;
			}
			return BindingGroup.FindIndexOf(target, list);
		}

		// Token: 0x060019AB RID: 6571 RVA: 0x0007A780 File Offset: 0x00078980
		private static int FindIndexOf(object item, IList<WeakReference> list)
		{
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				if (ItemsControl.EqualsEx(item, list[i].Target))
				{
					return i;
				}
				i++;
			}
			return -1;
		}

		// Token: 0x060019AC RID: 6572 RVA: 0x0007A7B8 File Offset: 0x000789B8
		private CultureInfo GetCulture()
		{
			if (this._culture == null)
			{
				DependencyObject dependencyObject = Helper.FindMentor(this);
				if (dependencyObject != null)
				{
					this._culture = ((XmlLanguage)dependencyObject.GetValue(FrameworkElement.LanguageProperty)).GetSpecificCulture();
				}
			}
			return this._culture;
		}

		// Token: 0x060019AD RID: 6573 RVA: 0x0007A7F8 File Offset: 0x000789F8
		private void OnBindingsChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
			case NotifyCollectionChangedAction.Add:
			{
				BindingExpressionBase bindingExpressionBase = e.NewItems[0] as BindingExpressionBase;
				bindingExpressionBase.JoinBindingGroup(this, true);
				break;
			}
			case NotifyCollectionChangedAction.Remove:
			{
				BindingExpressionBase bindingExpressionBase = e.OldItems[0] as BindingExpressionBase;
				this.RemoveBindingExpression(bindingExpressionBase);
				break;
			}
			case NotifyCollectionChangedAction.Replace:
			{
				BindingExpressionBase bindingExpressionBase = e.OldItems[0] as BindingExpressionBase;
				this.RemoveBindingExpression(bindingExpressionBase);
				bindingExpressionBase = (e.NewItems[0] as BindingExpressionBase);
				bindingExpressionBase.JoinBindingGroup(this, true);
				break;
			}
			case NotifyCollectionChangedAction.Reset:
				this.RemoveAllBindingExpressions();
				break;
			}
			this.IsItemsValid = false;
		}

		// Token: 0x060019AE RID: 6574 RVA: 0x0007A8A4 File Offset: 0x00078AA4
		private void RemoveBindingExpression(BindingExpressionBase exprBase)
		{
			BindingExpressionBase rootBindingExpression = exprBase.RootBindingExpression;
			if (this.SharesProposedValues && rootBindingExpression.NeedsValidation)
			{
				Collection<BindingExpressionBase.ProposedValue> proposedValues;
				rootBindingExpression.ValidateAndConvertProposedValue(out proposedValues);
				this.PreserveProposedValues(proposedValues);
			}
			List<BindingExpressionBase> list = this._getValueTable.RemoveRootBinding(rootBindingExpression);
			foreach (BindingExpressionBase bindingExpressionBase in list)
			{
				bindingExpressionBase.OnBindingGroupChanged(false);
				this._bindingExpressions.Remove(bindingExpressionBase);
			}
			rootBindingExpression.LeaveBindingGroup();
		}

		// Token: 0x060019AF RID: 6575 RVA: 0x0007A93C File Offset: 0x00078B3C
		private void RemoveAllBindingExpressions()
		{
			BindingGroup.GetValueTableEntry firstEntry;
			while ((firstEntry = this._getValueTable.GetFirstEntry()) != null)
			{
				this.RemoveBindingExpression(firstEntry.BindingExpressionBase);
			}
		}

		// Token: 0x060019B0 RID: 6576 RVA: 0x0007A968 File Offset: 0x00078B68
		private void PreserveProposedValues(Collection<BindingExpressionBase.ProposedValue> proposedValues)
		{
			if (proposedValues == null)
			{
				return;
			}
			int i = 0;
			int count = proposedValues.Count;
			while (i < count)
			{
				this._proposedValueTable.Add(proposedValues[i]);
				i++;
			}
		}

		// Token: 0x060019B1 RID: 6577 RVA: 0x0007A9A0 File Offset: 0x00078BA0
		private void PrepareProposedValuesForUpdate(DependencyObject mentor, bool isUpdating)
		{
			int count = this._proposedValueTable.Count;
			if (count == 0)
			{
				return;
			}
			if (isUpdating)
			{
				this._proposedValueBindingExpressions = new BindingExpression[count];
				for (int i = 0; i < count; i++)
				{
					BindingGroup.ProposedValueEntry proposedValueEntry = this._proposedValueTable[i];
					Binding binding = proposedValueEntry.Binding;
					Binding binding2 = new Binding();
					binding2.Source = proposedValueEntry.Item;
					binding2.Mode = BindingMode.TwoWay;
					binding2.Path = new PropertyPath(proposedValueEntry.PropertyName, new object[]
					{
						binding.Path.PathParameters
					});
					binding2.ValidatesOnDataErrors = binding.ValidatesOnDataErrors;
					binding2.ValidatesOnNotifyDataErrors = binding.ValidatesOnNotifyDataErrors;
					binding2.ValidatesOnExceptions = binding.ValidatesOnExceptions;
					Collection<ValidationRule> validationRulesInternal = binding.ValidationRulesInternal;
					if (validationRulesInternal != null)
					{
						int j = 0;
						int count2 = validationRulesInternal.Count;
						while (j < count2)
						{
							binding2.ValidationRules.Add(validationRulesInternal[j]);
							j++;
						}
					}
					BindingExpression bindingExpression = (BindingExpression)BindingExpressionBase.CreateUntargetedBindingExpression(mentor, binding2);
					bindingExpression.Attach(mentor);
					bindingExpression.NeedsUpdate = true;
					this._proposedValueBindingExpressions[i] = bindingExpression;
				}
			}
		}

		// Token: 0x060019B2 RID: 6578 RVA: 0x0007AAC4 File Offset: 0x00078CC4
		private void ResetProposedValuesAfterUpdate(DependencyObject mentor, bool isFullUpdate)
		{
			if (this._proposedValueBindingExpressions != null)
			{
				int i = 0;
				int num = this._proposedValueBindingExpressions.Length;
				while (i < num)
				{
					BindingExpression bindingExpression = this._proposedValueBindingExpressions[i];
					ValidationError validationError = bindingExpression.ValidationError;
					bindingExpression.Detach();
					if (validationError != null)
					{
						ValidationError validationError2 = new ValidationError(validationError.RuleInError, this, validationError.ErrorContent, validationError.Exception);
						this.AddValidationError(validationError2);
					}
					i++;
				}
				this._proposedValueBindingExpressions = null;
			}
			if (isFullUpdate)
			{
				this._proposedValueTable.UpdateDependents();
				this._proposedValueTable.Clear();
			}
		}

		// Token: 0x060019B3 RID: 6579 RVA: 0x0007AB4C File Offset: 0x00078D4C
		private void NotifyCommitManager()
		{
			if (this.Engine.IsShutDown)
			{
				return;
			}
			bool flag = this.Owner != null && (this.IsDirty || this.HasValidationError);
			if (flag)
			{
				this.Engine.CommitManager.AddBindingGroup(this);
				return;
			}
			this.Engine.CommitManager.RemoveBindingGroup(this);
		}

		// Token: 0x060019B4 RID: 6580 RVA: 0x0007ABAA File Offset: 0x00078DAA
		private void OnErrorsChanged(object sender, DataErrorsChangedEventArgs e)
		{
			if (base.Dispatcher.Thread == Thread.CurrentThread)
			{
				this.UpdateNotifyDataErrors((INotifyDataErrorInfo)sender, null);
				return;
			}
			this.Engine.Marshal(delegate(object arg)
			{
				this.UpdateNotifyDataErrors((INotifyDataErrorInfo)arg, null);
				return null;
			}, sender, 1);
		}

		// Token: 0x0400131B RID: 4891
		private ValidationRuleCollection _validationRules;

		// Token: 0x0400131C RID: 4892
		private string _name;

		// Token: 0x0400131D RID: 4893
		private bool _notifyOnValidationError;

		// Token: 0x0400131E RID: 4894
		private bool _sharesProposedValues;

		// Token: 0x0400131F RID: 4895
		private bool _validatesOnNotifyDataError = true;

		// Token: 0x04001320 RID: 4896
		private DataBindEngine _engine;

		// Token: 0x04001321 RID: 4897
		private BindingGroup.BindingExpressionCollection _bindingExpressions;

		// Token: 0x04001322 RID: 4898
		private bool _isItemsValid;

		// Token: 0x04001323 RID: 4899
		private ValidationStep _validationStep = (ValidationStep)(-1);

		// Token: 0x04001324 RID: 4900
		private BindingGroup.GetValueTable _getValueTable = new BindingGroup.GetValueTable();

		// Token: 0x04001325 RID: 4901
		private BindingGroup.ProposedValueTable _proposedValueTable = new BindingGroup.ProposedValueTable();

		// Token: 0x04001326 RID: 4902
		private BindingExpression[] _proposedValueBindingExpressions;

		// Token: 0x04001327 RID: 4903
		private Collection<WeakReference> _itemsRW;

		// Token: 0x04001328 RID: 4904
		private WeakReadOnlyCollection<object> _items;

		// Token: 0x04001329 RID: 4905
		private CultureInfo _culture;

		// Token: 0x0400132A RID: 4906
		private Dictionary<WeakReference, List<ValidationError>> _notifyDataErrors = new Dictionary<WeakReference, List<ValidationError>>();

		// Token: 0x0400132B RID: 4907
		internal static readonly object DeferredTargetValue = new NamedObject("DeferredTargetValue");

		// Token: 0x0400132C RID: 4908
		internal static readonly object DeferredSourceValue = new NamedObject("DeferredSourceValue");

		// Token: 0x0400132D RID: 4909
		private static WeakReference<DependencyObject> NullInheritanceContext = new WeakReference<DependencyObject>(null);

		// Token: 0x0400132E RID: 4910
		private WeakReference<DependencyObject> _inheritanceContext = BindingGroup.NullInheritanceContext;

		// Token: 0x0400132F RID: 4911
		private bool _hasMultipleInheritanceContexts;

		// Token: 0x0200086A RID: 2154
		private class GetValueTable
		{
			// Token: 0x17001D9F RID: 7583
			public BindingGroup.GetValueTableEntry this[object item, string propertyName]
			{
				get
				{
					for (int i = this._table.Count - 1; i >= 0; i--)
					{
						BindingGroup.GetValueTableEntry getValueTableEntry = this._table[i];
						if (propertyName == getValueTableEntry.PropertyName && ItemsControl.EqualsEx(item, getValueTableEntry.Item))
						{
							return getValueTableEntry;
						}
					}
					return null;
				}
			}

			// Token: 0x17001DA0 RID: 7584
			public BindingGroup.GetValueTableEntry this[BindingExpressionBase bindingExpressionBase]
			{
				get
				{
					for (int i = this._table.Count - 1; i >= 0; i--)
					{
						BindingGroup.GetValueTableEntry getValueTableEntry = this._table[i];
						if (bindingExpressionBase == getValueTableEntry.BindingExpressionBase)
						{
							return getValueTableEntry;
						}
					}
					return null;
				}
			}

			// Token: 0x060082DB RID: 33499 RVA: 0x00243FC0 File Offset: 0x002421C0
			public void EnsureEntry(BindingExpressionBase bindingExpressionBase)
			{
				if (this[bindingExpressionBase] == null)
				{
					this._table.Add(new BindingGroup.GetValueTableEntry(bindingExpressionBase));
				}
			}

			// Token: 0x060082DC RID: 33500 RVA: 0x00243FEC File Offset: 0x002421EC
			public bool Update(BindingExpression bindingExpression)
			{
				BindingGroup.GetValueTableEntry getValueTableEntry = this[bindingExpression];
				bool flag = getValueTableEntry == null;
				if (flag)
				{
					this._table.Add(new BindingGroup.GetValueTableEntry(bindingExpression));
				}
				else
				{
					getValueTableEntry.Update(bindingExpression);
				}
				return flag;
			}

			// Token: 0x060082DD RID: 33501 RVA: 0x00244024 File Offset: 0x00242224
			public List<BindingExpressionBase> RemoveRootBinding(BindingExpressionBase rootBindingExpression)
			{
				List<BindingExpressionBase> list = new List<BindingExpressionBase>();
				for (int i = this._table.Count - 1; i >= 0; i--)
				{
					BindingExpressionBase bindingExpressionBase = this._table[i].BindingExpressionBase;
					if (bindingExpressionBase.RootBindingExpression == rootBindingExpression)
					{
						list.Add(bindingExpressionBase);
						this._table.RemoveAt(i);
					}
				}
				return list;
			}

			// Token: 0x060082DE RID: 33502 RVA: 0x00244080 File Offset: 0x00242280
			public void AddUniqueItems(IList<WeakReference> list)
			{
				for (int i = this._table.Count - 1; i >= 0; i--)
				{
					if (this._table[i].BindingExpressionBase.StatusInternal != BindingStatusInternal.PathError)
					{
						WeakReference itemReference = this._table[i].ItemReference;
						if (itemReference != null && BindingGroup.FindIndexOf(itemReference, list) < 0)
						{
							list.Add(itemReference);
						}
					}
				}
			}

			// Token: 0x060082DF RID: 33503 RVA: 0x002440E4 File Offset: 0x002422E4
			public object GetValue(BindingExpressionBase bindingExpressionBase)
			{
				BindingGroup.GetValueTableEntry getValueTableEntry = this[bindingExpressionBase];
				if (getValueTableEntry == null)
				{
					return DependencyProperty.UnsetValue;
				}
				return getValueTableEntry.Value;
			}

			// Token: 0x060082E0 RID: 33504 RVA: 0x00244108 File Offset: 0x00242308
			public void SetValue(BindingExpressionBase bindingExpressionBase, object value)
			{
				BindingGroup.GetValueTableEntry getValueTableEntry = this[bindingExpressionBase];
				if (getValueTableEntry != null)
				{
					getValueTableEntry.Value = value;
				}
			}

			// Token: 0x060082E1 RID: 33505 RVA: 0x00244128 File Offset: 0x00242328
			public void ResetValues()
			{
				for (int i = this._table.Count - 1; i >= 0; i--)
				{
					this._table[i].Value = BindingGroup.DeferredTargetValue;
				}
			}

			// Token: 0x060082E2 RID: 33506 RVA: 0x00244164 File Offset: 0x00242364
			public void UseSourceValue(BindingExpressionBase rootBindingExpression)
			{
				for (int i = this._table.Count - 1; i >= 0; i--)
				{
					if (this._table[i].BindingExpressionBase.RootBindingExpression == rootBindingExpression)
					{
						this._table[i].Value = BindingGroup.DeferredSourceValue;
					}
				}
			}

			// Token: 0x060082E3 RID: 33507 RVA: 0x002441B8 File Offset: 0x002423B8
			public BindingGroup.GetValueTableEntry GetFirstEntry()
			{
				if (this._table.Count <= 0)
				{
					return null;
				}
				return this._table[0];
			}

			// Token: 0x04004118 RID: 16664
			private Collection<BindingGroup.GetValueTableEntry> _table = new Collection<BindingGroup.GetValueTableEntry>();
		}

		// Token: 0x0200086B RID: 2155
		private class GetValueTableEntry
		{
			// Token: 0x060082E5 RID: 33509 RVA: 0x002441E9 File Offset: 0x002423E9
			public GetValueTableEntry(BindingExpressionBase bindingExpressionBase)
			{
				this._bindingExpressionBase = bindingExpressionBase;
			}

			// Token: 0x060082E6 RID: 33510 RVA: 0x00244204 File Offset: 0x00242404
			public void Update(BindingExpression bindingExpression)
			{
				object sourceItem = bindingExpression.SourceItem;
				if (sourceItem == null)
				{
					this._itemWR = null;
				}
				else if (this._itemWR == null)
				{
					this._itemWR = new WeakReference(sourceItem);
				}
				else
				{
					this._itemWR.Target = bindingExpression.SourceItem;
				}
				this._propertyName = bindingExpression.SourcePropertyName;
			}

			// Token: 0x17001DA1 RID: 7585
			// (get) Token: 0x060082E7 RID: 33511 RVA: 0x00244257 File Offset: 0x00242457
			public object Item
			{
				get
				{
					return this._itemWR.Target;
				}
			}

			// Token: 0x17001DA2 RID: 7586
			// (get) Token: 0x060082E8 RID: 33512 RVA: 0x00244264 File Offset: 0x00242464
			public WeakReference ItemReference
			{
				get
				{
					return this._itemWR;
				}
			}

			// Token: 0x17001DA3 RID: 7587
			// (get) Token: 0x060082E9 RID: 33513 RVA: 0x0024426C File Offset: 0x0024246C
			public string PropertyName
			{
				get
				{
					return this._propertyName;
				}
			}

			// Token: 0x17001DA4 RID: 7588
			// (get) Token: 0x060082EA RID: 33514 RVA: 0x00244274 File Offset: 0x00242474
			public BindingExpressionBase BindingExpressionBase
			{
				get
				{
					return this._bindingExpressionBase;
				}
			}

			// Token: 0x17001DA5 RID: 7589
			// (get) Token: 0x060082EB RID: 33515 RVA: 0x0024427C File Offset: 0x0024247C
			// (set) Token: 0x060082EC RID: 33516 RVA: 0x002442E3 File Offset: 0x002424E3
			public object Value
			{
				get
				{
					if (this._value == BindingGroup.DeferredTargetValue)
					{
						this._value = this._bindingExpressionBase.RootBindingExpression.GetRawProposedValue();
					}
					else if (this._value == BindingGroup.DeferredSourceValue)
					{
						BindingExpression bindingExpression = this._bindingExpressionBase as BindingExpression;
						this._value = ((bindingExpression != null) ? bindingExpression.SourceValue : DependencyProperty.UnsetValue);
					}
					return this._value;
				}
				set
				{
					this._value = value;
				}
			}

			// Token: 0x04004119 RID: 16665
			private BindingExpressionBase _bindingExpressionBase;

			// Token: 0x0400411A RID: 16666
			private WeakReference _itemWR;

			// Token: 0x0400411B RID: 16667
			private string _propertyName;

			// Token: 0x0400411C RID: 16668
			private object _value = BindingGroup.DeferredTargetValue;
		}

		// Token: 0x0200086C RID: 2156
		private class ProposedValueTable
		{
			// Token: 0x060082ED RID: 33517 RVA: 0x002442EC File Offset: 0x002424EC
			public void Add(BindingExpressionBase.ProposedValue proposedValue)
			{
				BindingExpression bindingExpression = proposedValue.BindingExpression;
				object sourceItem = bindingExpression.SourceItem;
				string sourcePropertyName = bindingExpression.SourcePropertyName;
				object rawValue = proposedValue.RawValue;
				object convertedValue = proposedValue.ConvertedValue;
				this.Remove(sourceItem, sourcePropertyName);
				this._table.Add(new BindingGroup.ProposedValueEntry(sourceItem, sourcePropertyName, rawValue, convertedValue, bindingExpression));
			}

			// Token: 0x060082EE RID: 33518 RVA: 0x0024433C File Offset: 0x0024253C
			public void Remove(object item, string propertyName)
			{
				int num = this.IndexOf(item, propertyName);
				if (num >= 0)
				{
					this._table.RemoveAt(num);
				}
			}

			// Token: 0x060082EF RID: 33519 RVA: 0x00244362 File Offset: 0x00242562
			public void Remove(BindingExpression bindExpr)
			{
				if (this._table.Count > 0)
				{
					this.Remove(bindExpr.SourceItem, bindExpr.SourcePropertyName);
				}
			}

			// Token: 0x060082F0 RID: 33520 RVA: 0x00244384 File Offset: 0x00242584
			public void Remove(BindingGroup.ProposedValueEntry entry)
			{
				this._table.Remove(entry);
			}

			// Token: 0x060082F1 RID: 33521 RVA: 0x00244393 File Offset: 0x00242593
			public void Clear()
			{
				this._table.Clear();
			}

			// Token: 0x17001DA6 RID: 7590
			// (get) Token: 0x060082F2 RID: 33522 RVA: 0x002443A0 File Offset: 0x002425A0
			public int Count
			{
				get
				{
					return this._table.Count;
				}
			}

			// Token: 0x17001DA7 RID: 7591
			public BindingGroup.ProposedValueEntry this[object item, string propertyName]
			{
				get
				{
					int num = this.IndexOf(item, propertyName);
					if (num >= 0)
					{
						return this._table[num];
					}
					return null;
				}
			}

			// Token: 0x17001DA8 RID: 7592
			public BindingGroup.ProposedValueEntry this[int index]
			{
				get
				{
					return this._table[index];
				}
			}

			// Token: 0x17001DA9 RID: 7593
			public BindingGroup.ProposedValueEntry this[BindingExpression bindExpr]
			{
				get
				{
					return this[bindExpr.SourceItem, bindExpr.SourcePropertyName];
				}
			}

			// Token: 0x060082F6 RID: 33526 RVA: 0x002443FC File Offset: 0x002425FC
			public void AddUniqueItems(IList<WeakReference> list)
			{
				for (int i = this._table.Count - 1; i >= 0; i--)
				{
					WeakReference itemReference = this._table[i].ItemReference;
					if (itemReference != null && BindingGroup.FindIndexOf(itemReference, list) < 0)
					{
						list.Add(itemReference);
					}
				}
			}

			// Token: 0x060082F7 RID: 33527 RVA: 0x00244448 File Offset: 0x00242648
			public void UpdateDependents()
			{
				for (int i = this._table.Count - 1; i >= 0; i--)
				{
					Collection<BindingExpressionBase> dependents = this._table[i].Dependents;
					if (dependents != null)
					{
						for (int j = dependents.Count - 1; j >= 0; j--)
						{
							BindingExpressionBase bindingExpressionBase = dependents[j];
							if (!bindingExpressionBase.IsDetached)
							{
								dependents[j].UpdateTarget();
							}
						}
					}
				}
			}

			// Token: 0x060082F8 RID: 33528 RVA: 0x002444B4 File Offset: 0x002426B4
			public bool HasValidationError(ValidationError validationError)
			{
				for (int i = this._table.Count - 1; i >= 0; i--)
				{
					if (validationError == this._table[i].ValidationError)
					{
						return true;
					}
				}
				return false;
			}

			// Token: 0x060082F9 RID: 33529 RVA: 0x002444F0 File Offset: 0x002426F0
			private int IndexOf(object item, string propertyName)
			{
				for (int i = this._table.Count - 1; i >= 0; i--)
				{
					BindingGroup.ProposedValueEntry proposedValueEntry = this._table[i];
					if (propertyName == proposedValueEntry.PropertyName && ItemsControl.EqualsEx(item, proposedValueEntry.Item))
					{
						return i;
					}
				}
				return -1;
			}

			// Token: 0x0400411D RID: 16669
			private Collection<BindingGroup.ProposedValueEntry> _table = new Collection<BindingGroup.ProposedValueEntry>();
		}

		// Token: 0x0200086D RID: 2157
		internal class ProposedValueEntry
		{
			// Token: 0x060082FB RID: 33531 RVA: 0x00244554 File Offset: 0x00242754
			public ProposedValueEntry(object item, string propertyName, object rawValue, object convertedValue, BindingExpression bindExpr)
			{
				this._itemReference = new WeakReference(item);
				this._propertyName = propertyName;
				this._rawValue = rawValue;
				this._convertedValue = convertedValue;
				this._error = bindExpr.ValidationError;
				this._binding = bindExpr.ParentBinding;
			}

			// Token: 0x17001DAA RID: 7594
			// (get) Token: 0x060082FC RID: 33532 RVA: 0x002445A3 File Offset: 0x002427A3
			public object Item
			{
				get
				{
					return this._itemReference.Target;
				}
			}

			// Token: 0x17001DAB RID: 7595
			// (get) Token: 0x060082FD RID: 33533 RVA: 0x002445B0 File Offset: 0x002427B0
			public string PropertyName
			{
				get
				{
					return this._propertyName;
				}
			}

			// Token: 0x17001DAC RID: 7596
			// (get) Token: 0x060082FE RID: 33534 RVA: 0x002445B8 File Offset: 0x002427B8
			public object RawValue
			{
				get
				{
					return this._rawValue;
				}
			}

			// Token: 0x17001DAD RID: 7597
			// (get) Token: 0x060082FF RID: 33535 RVA: 0x002445C0 File Offset: 0x002427C0
			public object ConvertedValue
			{
				get
				{
					return this._convertedValue;
				}
			}

			// Token: 0x17001DAE RID: 7598
			// (get) Token: 0x06008300 RID: 33536 RVA: 0x002445C8 File Offset: 0x002427C8
			public ValidationError ValidationError
			{
				get
				{
					return this._error;
				}
			}

			// Token: 0x17001DAF RID: 7599
			// (get) Token: 0x06008301 RID: 33537 RVA: 0x002445D0 File Offset: 0x002427D0
			public Binding Binding
			{
				get
				{
					return this._binding;
				}
			}

			// Token: 0x17001DB0 RID: 7600
			// (get) Token: 0x06008302 RID: 33538 RVA: 0x002445D8 File Offset: 0x002427D8
			public WeakReference ItemReference
			{
				get
				{
					return this._itemReference;
				}
			}

			// Token: 0x17001DB1 RID: 7601
			// (get) Token: 0x06008303 RID: 33539 RVA: 0x002445E0 File Offset: 0x002427E0
			public Collection<BindingExpressionBase> Dependents
			{
				get
				{
					return this._dependents;
				}
			}

			// Token: 0x06008304 RID: 33540 RVA: 0x002445E8 File Offset: 0x002427E8
			public void AddDependent(BindingExpressionBase dependent)
			{
				if (this._dependents == null)
				{
					this._dependents = new Collection<BindingExpressionBase>();
				}
				this._dependents.Add(dependent);
			}

			// Token: 0x0400411E RID: 16670
			private WeakReference _itemReference;

			// Token: 0x0400411F RID: 16671
			private string _propertyName;

			// Token: 0x04004120 RID: 16672
			private object _rawValue;

			// Token: 0x04004121 RID: 16673
			private object _convertedValue;

			// Token: 0x04004122 RID: 16674
			private ValidationError _error;

			// Token: 0x04004123 RID: 16675
			private Binding _binding;

			// Token: 0x04004124 RID: 16676
			private Collection<BindingExpressionBase> _dependents;
		}

		// Token: 0x0200086E RID: 2158
		private class BindingExpressionCollection : ObservableCollection<BindingExpressionBase>
		{
			// Token: 0x06008305 RID: 33541 RVA: 0x00244609 File Offset: 0x00242809
			protected override void InsertItem(int index, BindingExpressionBase item)
			{
				if (item == null)
				{
					throw new ArgumentNullException("item");
				}
				base.InsertItem(index, item);
			}

			// Token: 0x06008306 RID: 33542 RVA: 0x00244621 File Offset: 0x00242821
			protected override void SetItem(int index, BindingExpressionBase item)
			{
				if (item == null)
				{
					throw new ArgumentNullException("item");
				}
				base.SetItem(index, item);
			}
		}
	}
}
