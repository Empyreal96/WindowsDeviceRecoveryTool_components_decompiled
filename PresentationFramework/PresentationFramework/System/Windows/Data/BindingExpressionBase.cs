using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Threading;
using MS.Internal;
using MS.Internal.Data;
using MS.Internal.Documents;

namespace System.Windows.Data
{
	/// <summary>Represents the base class for <see cref="T:System.Windows.Data.BindingExpression" />, <see cref="T:System.Windows.Data.PriorityBindingExpression" />, and <see cref="T:System.Windows.Data.MultiBindingExpression" />.</summary>
	// Token: 0x020001A0 RID: 416
	public abstract class BindingExpressionBase : Expression, IWeakEventListener
	{
		// Token: 0x060018A9 RID: 6313 RVA: 0x0007738C File Offset: 0x0007558C
		internal BindingExpressionBase(BindingBase binding, BindingExpressionBase parent) : base(ExpressionMode.SupportsUnboundSources)
		{
			if (binding == null)
			{
				throw new ArgumentNullException("binding");
			}
			this._binding = binding;
			this.SetValue(BindingExpressionBase.Feature.ParentBindingExpressionBase, parent, null);
			this._flags = (BindingExpressionBase.PrivateFlags)binding.Flags;
			if (parent != null)
			{
				this.ResolveNamesInTemplate = parent.ResolveNamesInTemplate;
				Type type = parent.GetType();
				if (type == typeof(MultiBindingExpression))
				{
					this.ChangeFlag(BindingExpressionBase.PrivateFlags.iInMultiBindingExpression, true);
				}
				else if (type == typeof(PriorityBindingExpression))
				{
					this.ChangeFlag(BindingExpressionBase.PrivateFlags.iInPriorityBindingExpression, true);
				}
			}
			PresentationTraceLevel traceLevel = PresentationTraceSources.GetTraceLevel(binding);
			if (traceLevel > PresentationTraceLevel.None)
			{
				PresentationTraceSources.SetTraceLevel(this, traceLevel);
			}
			if (TraceData.IsExtendedTraceEnabled(this, TraceDataLevel.CreateExpression))
			{
				if (parent == null)
				{
					TraceData.Trace(TraceEventType.Warning, TraceData.CreatedExpression(new object[]
					{
						TraceData.Identify(this),
						TraceData.Identify(binding)
					}));
				}
				else
				{
					TraceData.Trace(TraceEventType.Warning, TraceData.CreatedExpressionInParent(new object[]
					{
						TraceData.Identify(this),
						TraceData.Identify(binding),
						TraceData.Identify(parent)
					}));
				}
			}
			if (this.LookupValidationRule(typeof(ExceptionValidationRule)) != null)
			{
				this.ChangeFlag(BindingExpressionBase.PrivateFlags.iValidatesOnExceptions, true);
			}
			if (this.LookupValidationRule(typeof(DataErrorValidationRule)) != null)
			{
				this.ChangeFlag(BindingExpressionBase.PrivateFlags.iValidatesOnDataErrors, true);
			}
		}

		/// <summary>Gets the element that is the binding target object of this binding expression.</summary>
		/// <returns>The element that is the binding target object of this binding expression.</returns>
		// Token: 0x17000594 RID: 1428
		// (get) Token: 0x060018AA RID: 6314 RVA: 0x000774DF File Offset: 0x000756DF
		public DependencyObject Target
		{
			get
			{
				return this.TargetElement;
			}
		}

		/// <summary>Gets the binding target property of this binding expression.</summary>
		/// <returns>The binding target property of this binding expression.</returns>
		// Token: 0x17000595 RID: 1429
		// (get) Token: 0x060018AB RID: 6315 RVA: 0x000774E7 File Offset: 0x000756E7
		public DependencyProperty TargetProperty
		{
			get
			{
				return this._targetProperty;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Data.BindingBase" /> object from which this <see cref="T:System.Windows.Data.BindingExpressionBase" /> object is created.</summary>
		/// <returns>The <see cref="T:System.Windows.Data.BindingBase" /> object from which this <see cref="T:System.Windows.Data.BindingExpressionBase" /> object is created.</returns>
		// Token: 0x17000596 RID: 1430
		// (get) Token: 0x060018AC RID: 6316 RVA: 0x000774EF File Offset: 0x000756EF
		public BindingBase ParentBindingBase
		{
			get
			{
				return this._binding;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Data.BindingGroup" /> that this binding expression belongs to.</summary>
		/// <returns>The <see cref="T:System.Windows.Data.BindingGroup" /> that this binding expression belongs to. This property returns <see langword="null" /> if the <see cref="T:System.Windows.Data.BindingExpressionBase" /> is not part of a  <see cref="T:System.Windows.Data.BindingGroup" />.</returns>
		// Token: 0x17000597 RID: 1431
		// (get) Token: 0x060018AD RID: 6317 RVA: 0x000774F8 File Offset: 0x000756F8
		public BindingGroup BindingGroup
		{
			get
			{
				BindingExpressionBase rootBindingExpression = this.RootBindingExpression;
				WeakReference<BindingGroup> weakReference = (WeakReference<BindingGroup>)rootBindingExpression.GetValue(BindingExpressionBase.Feature.BindingGroup, null);
				if (weakReference == null)
				{
					return null;
				}
				BindingGroup result;
				if (!weakReference.TryGetTarget(out result))
				{
					return null;
				}
				return result;
			}
		}

		/// <summary>Gets the status of the binding expression.</summary>
		/// <returns>A <see cref="T:System.Windows.Data.BindingStatus" /> value that describes the status of the binding expression.</returns>
		// Token: 0x17000598 RID: 1432
		// (get) Token: 0x060018AE RID: 6318 RVA: 0x0007752C File Offset: 0x0007572C
		public BindingStatus Status
		{
			get
			{
				return (BindingStatus)this._status;
			}
		}

		// Token: 0x17000599 RID: 1433
		// (get) Token: 0x060018AF RID: 6319 RVA: 0x0007752C File Offset: 0x0007572C
		internal BindingStatusInternal StatusInternal
		{
			get
			{
				return this._status;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Controls.ValidationError" /> that caused this instance of <see cref="T:System.Windows.Data.BindingExpressionBase" /> to be invalid.</summary>
		/// <returns>The <see cref="T:System.Windows.Controls.ValidationError" /> that caused this instance of <see cref="T:System.Windows.Data.BindingExpressionBase" /> to be invalid.</returns>
		// Token: 0x1700059A RID: 1434
		// (get) Token: 0x060018B0 RID: 6320 RVA: 0x00077534 File Offset: 0x00075734
		public virtual ValidationError ValidationError
		{
			get
			{
				return this.BaseValidationError;
			}
		}

		// Token: 0x1700059B RID: 1435
		// (get) Token: 0x060018B1 RID: 6321 RVA: 0x0007753C File Offset: 0x0007573C
		internal ValidationError BaseValidationError
		{
			get
			{
				return (ValidationError)this.GetValue(BindingExpressionBase.Feature.ValidationError, null);
			}
		}

		// Token: 0x1700059C RID: 1436
		// (get) Token: 0x060018B2 RID: 6322 RVA: 0x0007754B File Offset: 0x0007574B
		internal List<ValidationError> NotifyDataErrors
		{
			get
			{
				return (List<ValidationError>)this.GetValue(BindingExpressionBase.Feature.NotifyDataErrors, null);
			}
		}

		/// <summary>Gets a value that indicates whether the parent binding has a failed validation rule.</summary>
		/// <returns>
		///     <see langword="true" /> if the parent binding has a failed validation rule; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700059D RID: 1437
		// (get) Token: 0x060018B3 RID: 6323 RVA: 0x0007755A File Offset: 0x0007575A
		public virtual bool HasError
		{
			get
			{
				return this.HasValidationError;
			}
		}

		/// <summary>Gets a value that indicates whether the parent binding has a failed validation rule.</summary>
		/// <returns>
		///     <see langword="true" /> if the parent binding has a failed validation rule; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700059E RID: 1438
		// (get) Token: 0x060018B4 RID: 6324 RVA: 0x00077562 File Offset: 0x00075762
		public virtual bool HasValidationError
		{
			get
			{
				return this.HasValue(BindingExpressionBase.Feature.ValidationError) || this.HasValue(BindingExpressionBase.Feature.NotifyDataErrors);
			}
		}

		/// <summary>Gets or sets a value that indicates whether the target of the binding has a value that has not been written to the source.</summary>
		/// <returns>
		///     <see langword="true" /> if the target has a value that has not been written to the source; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700059F RID: 1439
		// (get) Token: 0x060018B5 RID: 6325 RVA: 0x00077576 File Offset: 0x00075776
		public bool IsDirty
		{
			get
			{
				return this.NeedsUpdate;
			}
		}

		/// <summary>Gets a collection of <see cref="T:System.Windows.Controls.ValidationError" /> objects that caused this instance of <see cref="T:System.Windows.Data.BindingExpressionBase" /> to be invalid.</summary>
		/// <returns>A collection of <see cref="T:System.Windows.Controls.ValidationError" /> objects that caused this instance of <see cref="T:System.Windows.Data.BindingExpressionBase" /> to be invalid.  The value is <see langword="null" /> if there are no errors.</returns>
		// Token: 0x170005A0 RID: 1440
		// (get) Token: 0x060018B6 RID: 6326 RVA: 0x00077580 File Offset: 0x00075780
		public virtual ReadOnlyCollection<ValidationError> ValidationErrors
		{
			get
			{
				if (this.HasError)
				{
					List<ValidationError> list;
					if (!this.HasValue(BindingExpressionBase.Feature.ValidationError))
					{
						list = this.NotifyDataErrors;
					}
					else
					{
						if (this.NotifyDataErrors == null)
						{
							list = new List<ValidationError>();
						}
						else
						{
							list = new List<ValidationError>(this.NotifyDataErrors);
						}
						list.Insert(0, this.BaseValidationError);
					}
					return new ReadOnlyCollection<ValidationError>(list);
				}
				return null;
			}
		}

		/// <summary>Forces a data transfer from the binding source to the binding target.</summary>
		// Token: 0x060018B7 RID: 6327 RVA: 0x00002137 File Offset: 0x00000337
		public virtual void UpdateTarget()
		{
		}

		/// <summary>Sends the current binding target value to the binding source in <see cref="F:System.Windows.Data.BindingMode.TwoWay" /> or <see cref="F:System.Windows.Data.BindingMode.OneWayToSource" /> bindings.</summary>
		// Token: 0x060018B8 RID: 6328 RVA: 0x00002137 File Offset: 0x00000337
		public virtual void UpdateSource()
		{
		}

		/// <summary>Runs any <see cref="T:System.Windows.Controls.ValidationRule" /> objects on the associated <see cref="T:System.Windows.Data.Binding" /> that have the <see cref="P:System.Windows.Controls.ValidationRule.ValidationStep" /> property set to <see cref="F:System.Windows.Controls.ValidationStep.RawProposedValue" /> or <see cref="F:System.Windows.Controls.ValidationStep.ConvertedProposedValue" />. This method does not update the source.</summary>
		/// <returns>
		///     <see langword="true" /> if the validation rules succeed; otherwise, <see langword="false" />.</returns>
		// Token: 0x060018B9 RID: 6329 RVA: 0x000775D8 File Offset: 0x000757D8
		public bool ValidateWithoutUpdate()
		{
			Collection<BindingExpressionBase.ProposedValue> collection;
			return !this.NeedsValidation || this.ValidateAndConvertProposedValue(out collection);
		}

		// Token: 0x060018BA RID: 6330 RVA: 0x000775F7 File Offset: 0x000757F7
		internal sealed override void OnAttach(DependencyObject d, DependencyProperty dp)
		{
			if (d == null)
			{
				throw new ArgumentNullException("d");
			}
			if (dp == null)
			{
				throw new ArgumentNullException("dp");
			}
			this.Attach(d, dp);
		}

		// Token: 0x060018BB RID: 6331 RVA: 0x0007761D File Offset: 0x0007581D
		internal sealed override void OnDetach(DependencyObject d, DependencyProperty dp)
		{
			this.Detach();
		}

		// Token: 0x060018BC RID: 6332 RVA: 0x00077625 File Offset: 0x00075825
		internal override object GetValue(DependencyObject d, DependencyProperty dp)
		{
			return this.Value;
		}

		// Token: 0x060018BD RID: 6333 RVA: 0x0007762D File Offset: 0x0007582D
		internal override bool SetValue(DependencyObject d, DependencyProperty dp, object value)
		{
			if (this.IsReflective)
			{
				this.Value = value;
				return true;
			}
			return false;
		}

		// Token: 0x060018BE RID: 6334 RVA: 0x00077644 File Offset: 0x00075844
		internal override void OnPropertyInvalidation(DependencyObject d, DependencyPropertyChangedEventArgs args)
		{
			if (this.IsDetached)
			{
				return;
			}
			this.IsTransferPending = true;
			if (this.Dispatcher.Thread == Thread.CurrentThread)
			{
				this.HandlePropertyInvalidation(d, args);
				return;
			}
			this.Engine.Marshal(new DispatcherOperationCallback(this.HandlePropertyInvalidationOperation), new object[]
			{
				d,
				args
			}, 1);
		}

		// Token: 0x060018BF RID: 6335 RVA: 0x000776A8 File Offset: 0x000758A8
		internal override DependencySource[] GetSources()
		{
			int num = (this._sources != null) ? this._sources.Length : 0;
			if (num == 0)
			{
				return null;
			}
			DependencySource[] array = new DependencySource[num];
			int num2 = 0;
			for (int i = 0; i < num; i++)
			{
				DependencyObject dependencyObject = this._sources[i].DependencyObject;
				if (dependencyObject != null)
				{
					array[num2++] = new DependencySource(dependencyObject, this._sources[i].DependencyProperty);
				}
			}
			if (num2 < num)
			{
				DependencySource[] sourceArray = array;
				array = new DependencySource[num2];
				Array.Copy(sourceArray, 0, array, 0, num2);
			}
			return array;
		}

		// Token: 0x060018C0 RID: 6336 RVA: 0x0007772B File Offset: 0x0007592B
		internal override Expression Copy(DependencyObject targetObject, DependencyProperty targetDP)
		{
			return this.ParentBindingBase.CreateBindingExpression(targetObject, targetDP);
		}

		/// <summary>This member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <param name="managerType">The type of the <see cref="T:System.Windows.WeakEventManager" /> calling this method. This only recognizes manager objects of type <see cref="T:System.Collections.Specialized.CollectionChangedEventManager" />.</param>
		/// <param name="sender">Object that originated the event.</param>
		/// <param name="e">Event data.</param>
		/// <returns>
		///     <see langword="true" /> if the listener handled the event; otherwise, <see langword="false" />.</returns>
		// Token: 0x060018C1 RID: 6337 RVA: 0x0007773A File Offset: 0x0007593A
		bool IWeakEventListener.ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
		{
			return this.ReceiveWeakEvent(managerType, sender, e);
		}

		// Token: 0x060018C2 RID: 6338 RVA: 0x00077745 File Offset: 0x00075945
		internal static BindingExpressionBase CreateUntargetedBindingExpression(DependencyObject d, BindingBase binding)
		{
			return binding.CreateBindingExpression(d, BindingExpressionBase.NoTargetProperty);
		}

		// Token: 0x060018C3 RID: 6339 RVA: 0x00077753 File Offset: 0x00075953
		internal void Attach(DependencyObject d)
		{
			this.Attach(d, BindingExpressionBase.NoTargetProperty);
		}

		// Token: 0x14000045 RID: 69
		// (add) Token: 0x060018C4 RID: 6340 RVA: 0x00077764 File Offset: 0x00075964
		// (remove) Token: 0x060018C5 RID: 6341 RVA: 0x0007779C File Offset: 0x0007599C
		internal event EventHandler<BindingValueChangedEventArgs> ValueChanged;

		// Token: 0x170005A1 RID: 1441
		// (get) Token: 0x060018C6 RID: 6342 RVA: 0x000777D1 File Offset: 0x000759D1
		// (set) Token: 0x060018C7 RID: 6343 RVA: 0x000777DE File Offset: 0x000759DE
		internal bool IsAttaching
		{
			get
			{
				return this.TestFlag(BindingExpressionBase.PrivateFlags.iAttaching);
			}
			set
			{
				this.ChangeFlag(BindingExpressionBase.PrivateFlags.iAttaching, value);
			}
		}

		// Token: 0x170005A2 RID: 1442
		// (get) Token: 0x060018C8 RID: 6344 RVA: 0x000777EC File Offset: 0x000759EC
		// (set) Token: 0x060018C9 RID: 6345 RVA: 0x000777F9 File Offset: 0x000759F9
		internal bool IsDetaching
		{
			get
			{
				return this.TestFlag(BindingExpressionBase.PrivateFlags.iDetaching);
			}
			set
			{
				this.ChangeFlag(BindingExpressionBase.PrivateFlags.iDetaching, value);
			}
		}

		// Token: 0x170005A3 RID: 1443
		// (get) Token: 0x060018CA RID: 6346 RVA: 0x00077807 File Offset: 0x00075A07
		internal bool IsDetached
		{
			get
			{
				return this._status == BindingStatusInternal.Detached;
			}
		}

		// Token: 0x170005A4 RID: 1444
		// (get) Token: 0x060018CB RID: 6347 RVA: 0x00077812 File Offset: 0x00075A12
		private bool IsAttached
		{
			get
			{
				return this._status != BindingStatusInternal.Unattached && this._status != BindingStatusInternal.Detached && !this.IsDetaching;
			}
		}

		// Token: 0x170005A5 RID: 1445
		// (get) Token: 0x060018CC RID: 6348 RVA: 0x00077830 File Offset: 0x00075A30
		internal bool IsDynamic
		{
			get
			{
				return this.TestFlag(BindingExpressionBase.PrivateFlags.iSourceToTarget) && (!this.IsInMultiBindingExpression || this.ParentMultiBindingExpression.IsDynamic);
			}
		}

		// Token: 0x170005A6 RID: 1446
		// (get) Token: 0x060018CD RID: 6349 RVA: 0x00077852 File Offset: 0x00075A52
		// (set) Token: 0x060018CE RID: 6350 RVA: 0x00077874 File Offset: 0x00075A74
		internal bool IsReflective
		{
			get
			{
				return this.TestFlag(BindingExpressionBase.PrivateFlags.iTargetToSource) && (!this.IsInMultiBindingExpression || this.ParentMultiBindingExpression.IsReflective);
			}
			set
			{
				this.ChangeFlag(BindingExpressionBase.PrivateFlags.iTargetToSource, value);
			}
		}

		// Token: 0x170005A7 RID: 1447
		// (get) Token: 0x060018CF RID: 6351 RVA: 0x0007787E File Offset: 0x00075A7E
		// (set) Token: 0x060018D0 RID: 6352 RVA: 0x00077888 File Offset: 0x00075A88
		internal bool UseDefaultValueConverter
		{
			get
			{
				return this.TestFlag(BindingExpressionBase.PrivateFlags.iDefaultValueConverter);
			}
			set
			{
				this.ChangeFlag(BindingExpressionBase.PrivateFlags.iDefaultValueConverter, value);
			}
		}

		// Token: 0x170005A8 RID: 1448
		// (get) Token: 0x060018D1 RID: 6353 RVA: 0x00077893 File Offset: 0x00075A93
		internal bool IsOneWayToSource
		{
			get
			{
				return (this._flags & BindingExpressionBase.PrivateFlags.iPropagationMask) == BindingExpressionBase.PrivateFlags.iTargetToSource;
			}
		}

		// Token: 0x170005A9 RID: 1449
		// (get) Token: 0x060018D2 RID: 6354 RVA: 0x000778A0 File Offset: 0x00075AA0
		internal bool IsUpdateOnPropertyChanged
		{
			get
			{
				return (this._flags & BindingExpressionBase.PrivateFlags.iUpdateDefault) == ~(BindingExpressionBase.PrivateFlags.iSourceToTarget | BindingExpressionBase.PrivateFlags.iTargetToSource | BindingExpressionBase.PrivateFlags.iPropDefault | BindingExpressionBase.PrivateFlags.iNotifyOnTargetUpdated | BindingExpressionBase.PrivateFlags.iDefaultValueConverter | BindingExpressionBase.PrivateFlags.iInTransfer | BindingExpressionBase.PrivateFlags.iInUpdate | BindingExpressionBase.PrivateFlags.iTransferPending | BindingExpressionBase.PrivateFlags.iNeedDataTransfer | BindingExpressionBase.PrivateFlags.iTransferDeferred | BindingExpressionBase.PrivateFlags.iUpdateOnLostFocus | BindingExpressionBase.PrivateFlags.iUpdateExplicitly | BindingExpressionBase.PrivateFlags.iNeedsUpdate | BindingExpressionBase.PrivateFlags.iPathGeneratedInternally | BindingExpressionBase.PrivateFlags.iUsingMentor | BindingExpressionBase.PrivateFlags.iResolveNamesInTemplate | BindingExpressionBase.PrivateFlags.iDetaching | BindingExpressionBase.PrivateFlags.iNeedsCollectionView | BindingExpressionBase.PrivateFlags.iInPriorityBindingExpression | BindingExpressionBase.PrivateFlags.iInMultiBindingExpression | BindingExpressionBase.PrivateFlags.iUsingFallbackValue | BindingExpressionBase.PrivateFlags.iNotifyOnValidationError | BindingExpressionBase.PrivateFlags.iAttaching | BindingExpressionBase.PrivateFlags.iNotifyOnSourceUpdated | BindingExpressionBase.PrivateFlags.iValidatesOnExceptions | BindingExpressionBase.PrivateFlags.iValidatesOnDataErrors | BindingExpressionBase.PrivateFlags.iIllegalInput | BindingExpressionBase.PrivateFlags.iNeedsValidation | BindingExpressionBase.PrivateFlags.iTargetWantsXTNotification | BindingExpressionBase.PrivateFlags.iValidatesOnNotifyDataErrors | BindingExpressionBase.PrivateFlags.iDataErrorsChangedPending | BindingExpressionBase.PrivateFlags.iDeferUpdateForComposition);
			}
		}

		// Token: 0x170005AA RID: 1450
		// (get) Token: 0x060018D3 RID: 6355 RVA: 0x000778B1 File Offset: 0x00075AB1
		internal bool IsUpdateOnLostFocus
		{
			get
			{
				return this.TestFlag(BindingExpressionBase.PrivateFlags.iUpdateOnLostFocus);
			}
		}

		// Token: 0x170005AB RID: 1451
		// (get) Token: 0x060018D4 RID: 6356 RVA: 0x000778BE File Offset: 0x00075ABE
		// (set) Token: 0x060018D5 RID: 6357 RVA: 0x000778CB File Offset: 0x00075ACB
		internal bool IsTransferPending
		{
			get
			{
				return this.TestFlag(BindingExpressionBase.PrivateFlags.iTransferPending);
			}
			set
			{
				this.ChangeFlag(BindingExpressionBase.PrivateFlags.iTransferPending, value);
			}
		}

		// Token: 0x170005AC RID: 1452
		// (get) Token: 0x060018D6 RID: 6358 RVA: 0x000778D9 File Offset: 0x00075AD9
		// (set) Token: 0x060018D7 RID: 6359 RVA: 0x000778E6 File Offset: 0x00075AE6
		internal bool TransferIsDeferred
		{
			get
			{
				return this.TestFlag(BindingExpressionBase.PrivateFlags.iTransferDeferred);
			}
			set
			{
				this.ChangeFlag(BindingExpressionBase.PrivateFlags.iTransferDeferred, value);
			}
		}

		// Token: 0x170005AD RID: 1453
		// (get) Token: 0x060018D8 RID: 6360 RVA: 0x000778F4 File Offset: 0x00075AF4
		// (set) Token: 0x060018D9 RID: 6361 RVA: 0x000778FE File Offset: 0x00075AFE
		internal bool IsInTransfer
		{
			get
			{
				return this.TestFlag(BindingExpressionBase.PrivateFlags.iInTransfer);
			}
			set
			{
				this.ChangeFlag(BindingExpressionBase.PrivateFlags.iInTransfer, value);
			}
		}

		// Token: 0x170005AE RID: 1454
		// (get) Token: 0x060018DA RID: 6362 RVA: 0x00077909 File Offset: 0x00075B09
		// (set) Token: 0x060018DB RID: 6363 RVA: 0x00077913 File Offset: 0x00075B13
		internal bool IsInUpdate
		{
			get
			{
				return this.TestFlag(BindingExpressionBase.PrivateFlags.iInUpdate);
			}
			set
			{
				this.ChangeFlag(BindingExpressionBase.PrivateFlags.iInUpdate, value);
			}
		}

		// Token: 0x170005AF RID: 1455
		// (get) Token: 0x060018DC RID: 6364 RVA: 0x0007791E File Offset: 0x00075B1E
		// (set) Token: 0x060018DD RID: 6365 RVA: 0x0007792B File Offset: 0x00075B2B
		internal bool UsingFallbackValue
		{
			get
			{
				return this.TestFlag(BindingExpressionBase.PrivateFlags.iUsingFallbackValue);
			}
			set
			{
				this.ChangeFlag(BindingExpressionBase.PrivateFlags.iUsingFallbackValue, value);
			}
		}

		// Token: 0x170005B0 RID: 1456
		// (get) Token: 0x060018DE RID: 6366 RVA: 0x00077939 File Offset: 0x00075B39
		// (set) Token: 0x060018DF RID: 6367 RVA: 0x00077946 File Offset: 0x00075B46
		internal bool UsingMentor
		{
			get
			{
				return this.TestFlag(BindingExpressionBase.PrivateFlags.iUsingMentor);
			}
			set
			{
				this.ChangeFlag(BindingExpressionBase.PrivateFlags.iUsingMentor, value);
			}
		}

		// Token: 0x170005B1 RID: 1457
		// (get) Token: 0x060018E0 RID: 6368 RVA: 0x00077954 File Offset: 0x00075B54
		// (set) Token: 0x060018E1 RID: 6369 RVA: 0x00077961 File Offset: 0x00075B61
		internal bool ResolveNamesInTemplate
		{
			get
			{
				return this.TestFlag(BindingExpressionBase.PrivateFlags.iResolveNamesInTemplate);
			}
			set
			{
				this.ChangeFlag(BindingExpressionBase.PrivateFlags.iResolveNamesInTemplate, value);
			}
		}

		// Token: 0x170005B2 RID: 1458
		// (get) Token: 0x060018E2 RID: 6370 RVA: 0x0007796F File Offset: 0x00075B6F
		// (set) Token: 0x060018E3 RID: 6371 RVA: 0x0007797C File Offset: 0x00075B7C
		internal bool NeedsDataTransfer
		{
			get
			{
				return this.TestFlag(BindingExpressionBase.PrivateFlags.iNeedDataTransfer);
			}
			set
			{
				this.ChangeFlag(BindingExpressionBase.PrivateFlags.iNeedDataTransfer, value);
			}
		}

		// Token: 0x170005B3 RID: 1459
		// (get) Token: 0x060018E4 RID: 6372 RVA: 0x0007798A File Offset: 0x00075B8A
		// (set) Token: 0x060018E5 RID: 6373 RVA: 0x00077997 File Offset: 0x00075B97
		internal bool NeedsUpdate
		{
			get
			{
				return this.TestFlag(BindingExpressionBase.PrivateFlags.iNeedsUpdate);
			}
			set
			{
				this.ChangeFlag(BindingExpressionBase.PrivateFlags.iNeedsUpdate, value);
				if (value)
				{
					this.NeedsValidation = true;
				}
			}
		}

		// Token: 0x170005B4 RID: 1460
		// (get) Token: 0x060018E6 RID: 6374 RVA: 0x000779AF File Offset: 0x00075BAF
		// (set) Token: 0x060018E7 RID: 6375 RVA: 0x000779C7 File Offset: 0x00075BC7
		internal bool NeedsValidation
		{
			get
			{
				return this.TestFlag(BindingExpressionBase.PrivateFlags.iNeedsValidation) || this.HasValue(BindingExpressionBase.Feature.ValidationError);
			}
			set
			{
				this.ChangeFlag(BindingExpressionBase.PrivateFlags.iNeedsValidation, value);
			}
		}

		// Token: 0x170005B5 RID: 1461
		// (get) Token: 0x060018E8 RID: 6376 RVA: 0x000779D5 File Offset: 0x00075BD5
		// (set) Token: 0x060018E9 RID: 6377 RVA: 0x000779DE File Offset: 0x00075BDE
		internal bool NotifyOnTargetUpdated
		{
			get
			{
				return this.TestFlag(BindingExpressionBase.PrivateFlags.iNotifyOnTargetUpdated);
			}
			set
			{
				this.ChangeFlag(BindingExpressionBase.PrivateFlags.iNotifyOnTargetUpdated, value);
			}
		}

		// Token: 0x170005B6 RID: 1462
		// (get) Token: 0x060018EA RID: 6378 RVA: 0x000779E8 File Offset: 0x00075BE8
		// (set) Token: 0x060018EB RID: 6379 RVA: 0x000779F5 File Offset: 0x00075BF5
		internal bool NotifyOnSourceUpdated
		{
			get
			{
				return this.TestFlag(BindingExpressionBase.PrivateFlags.iNotifyOnSourceUpdated);
			}
			set
			{
				this.ChangeFlag(BindingExpressionBase.PrivateFlags.iNotifyOnSourceUpdated, value);
			}
		}

		// Token: 0x170005B7 RID: 1463
		// (get) Token: 0x060018EC RID: 6380 RVA: 0x00077A03 File Offset: 0x00075C03
		// (set) Token: 0x060018ED RID: 6381 RVA: 0x00077A10 File Offset: 0x00075C10
		internal bool NotifyOnValidationError
		{
			get
			{
				return this.TestFlag(BindingExpressionBase.PrivateFlags.iNotifyOnValidationError);
			}
			set
			{
				this.ChangeFlag(BindingExpressionBase.PrivateFlags.iNotifyOnValidationError, value);
			}
		}

		// Token: 0x170005B8 RID: 1464
		// (get) Token: 0x060018EE RID: 6382 RVA: 0x00077A1E File Offset: 0x00075C1E
		internal bool IsInPriorityBindingExpression
		{
			get
			{
				return this.TestFlag(BindingExpressionBase.PrivateFlags.iInPriorityBindingExpression);
			}
		}

		// Token: 0x170005B9 RID: 1465
		// (get) Token: 0x060018EF RID: 6383 RVA: 0x00077A2B File Offset: 0x00075C2B
		internal bool IsInMultiBindingExpression
		{
			get
			{
				return this.TestFlag(BindingExpressionBase.PrivateFlags.iInMultiBindingExpression);
			}
		}

		// Token: 0x170005BA RID: 1466
		// (get) Token: 0x060018F0 RID: 6384 RVA: 0x00077A38 File Offset: 0x00075C38
		internal bool IsInBindingExpressionCollection
		{
			get
			{
				return this.TestFlag(BindingExpressionBase.PrivateFlags.iInPriorityBindingExpression | BindingExpressionBase.PrivateFlags.iInMultiBindingExpression);
			}
		}

		// Token: 0x170005BB RID: 1467
		// (get) Token: 0x060018F1 RID: 6385 RVA: 0x00077A45 File Offset: 0x00075C45
		internal bool ValidatesOnExceptions
		{
			get
			{
				return this.TestFlag(BindingExpressionBase.PrivateFlags.iValidatesOnExceptions);
			}
		}

		// Token: 0x170005BC RID: 1468
		// (get) Token: 0x060018F2 RID: 6386 RVA: 0x00077A52 File Offset: 0x00075C52
		internal bool ValidatesOnDataErrors
		{
			get
			{
				return this.TestFlag(BindingExpressionBase.PrivateFlags.iValidatesOnDataErrors);
			}
		}

		// Token: 0x170005BD RID: 1469
		// (get) Token: 0x060018F3 RID: 6387 RVA: 0x00077A5F File Offset: 0x00075C5F
		// (set) Token: 0x060018F4 RID: 6388 RVA: 0x00077A6C File Offset: 0x00075C6C
		internal bool TargetWantsCrossThreadNotifications
		{
			get
			{
				return this.TestFlag(BindingExpressionBase.PrivateFlags.iTargetWantsXTNotification);
			}
			set
			{
				this.ChangeFlag(BindingExpressionBase.PrivateFlags.iTargetWantsXTNotification, value);
			}
		}

		// Token: 0x170005BE RID: 1470
		// (get) Token: 0x060018F5 RID: 6389 RVA: 0x00077A7A File Offset: 0x00075C7A
		// (set) Token: 0x060018F6 RID: 6390 RVA: 0x00077A87 File Offset: 0x00075C87
		internal bool IsDataErrorsChangedPending
		{
			get
			{
				return this.TestFlag(BindingExpressionBase.PrivateFlags.iDataErrorsChangedPending);
			}
			set
			{
				this.ChangeFlag(BindingExpressionBase.PrivateFlags.iDataErrorsChangedPending, value);
			}
		}

		// Token: 0x170005BF RID: 1471
		// (get) Token: 0x060018F7 RID: 6391 RVA: 0x00077A95 File Offset: 0x00075C95
		// (set) Token: 0x060018F8 RID: 6392 RVA: 0x00077AA2 File Offset: 0x00075CA2
		internal bool IsUpdateDeferredForComposition
		{
			get
			{
				return this.TestFlag(~(BindingExpressionBase.PrivateFlags.iSourceToTarget | BindingExpressionBase.PrivateFlags.iTargetToSource | BindingExpressionBase.PrivateFlags.iPropDefault | BindingExpressionBase.PrivateFlags.iNotifyOnTargetUpdated | BindingExpressionBase.PrivateFlags.iDefaultValueConverter | BindingExpressionBase.PrivateFlags.iInTransfer | BindingExpressionBase.PrivateFlags.iInUpdate | BindingExpressionBase.PrivateFlags.iTransferPending | BindingExpressionBase.PrivateFlags.iNeedDataTransfer | BindingExpressionBase.PrivateFlags.iTransferDeferred | BindingExpressionBase.PrivateFlags.iUpdateOnLostFocus | BindingExpressionBase.PrivateFlags.iUpdateExplicitly | BindingExpressionBase.PrivateFlags.iNeedsUpdate | BindingExpressionBase.PrivateFlags.iPathGeneratedInternally | BindingExpressionBase.PrivateFlags.iUsingMentor | BindingExpressionBase.PrivateFlags.iResolveNamesInTemplate | BindingExpressionBase.PrivateFlags.iDetaching | BindingExpressionBase.PrivateFlags.iNeedsCollectionView | BindingExpressionBase.PrivateFlags.iInPriorityBindingExpression | BindingExpressionBase.PrivateFlags.iInMultiBindingExpression | BindingExpressionBase.PrivateFlags.iUsingFallbackValue | BindingExpressionBase.PrivateFlags.iNotifyOnValidationError | BindingExpressionBase.PrivateFlags.iAttaching | BindingExpressionBase.PrivateFlags.iNotifyOnSourceUpdated | BindingExpressionBase.PrivateFlags.iValidatesOnExceptions | BindingExpressionBase.PrivateFlags.iValidatesOnDataErrors | BindingExpressionBase.PrivateFlags.iIllegalInput | BindingExpressionBase.PrivateFlags.iNeedsValidation | BindingExpressionBase.PrivateFlags.iTargetWantsXTNotification | BindingExpressionBase.PrivateFlags.iValidatesOnNotifyDataErrors | BindingExpressionBase.PrivateFlags.iDataErrorsChangedPending));
			}
			set
			{
				this.ChangeFlag(~(BindingExpressionBase.PrivateFlags.iSourceToTarget | BindingExpressionBase.PrivateFlags.iTargetToSource | BindingExpressionBase.PrivateFlags.iPropDefault | BindingExpressionBase.PrivateFlags.iNotifyOnTargetUpdated | BindingExpressionBase.PrivateFlags.iDefaultValueConverter | BindingExpressionBase.PrivateFlags.iInTransfer | BindingExpressionBase.PrivateFlags.iInUpdate | BindingExpressionBase.PrivateFlags.iTransferPending | BindingExpressionBase.PrivateFlags.iNeedDataTransfer | BindingExpressionBase.PrivateFlags.iTransferDeferred | BindingExpressionBase.PrivateFlags.iUpdateOnLostFocus | BindingExpressionBase.PrivateFlags.iUpdateExplicitly | BindingExpressionBase.PrivateFlags.iNeedsUpdate | BindingExpressionBase.PrivateFlags.iPathGeneratedInternally | BindingExpressionBase.PrivateFlags.iUsingMentor | BindingExpressionBase.PrivateFlags.iResolveNamesInTemplate | BindingExpressionBase.PrivateFlags.iDetaching | BindingExpressionBase.PrivateFlags.iNeedsCollectionView | BindingExpressionBase.PrivateFlags.iInPriorityBindingExpression | BindingExpressionBase.PrivateFlags.iInMultiBindingExpression | BindingExpressionBase.PrivateFlags.iUsingFallbackValue | BindingExpressionBase.PrivateFlags.iNotifyOnValidationError | BindingExpressionBase.PrivateFlags.iAttaching | BindingExpressionBase.PrivateFlags.iNotifyOnSourceUpdated | BindingExpressionBase.PrivateFlags.iValidatesOnExceptions | BindingExpressionBase.PrivateFlags.iValidatesOnDataErrors | BindingExpressionBase.PrivateFlags.iIllegalInput | BindingExpressionBase.PrivateFlags.iNeedsValidation | BindingExpressionBase.PrivateFlags.iTargetWantsXTNotification | BindingExpressionBase.PrivateFlags.iValidatesOnNotifyDataErrors | BindingExpressionBase.PrivateFlags.iDataErrorsChangedPending), value);
			}
		}

		// Token: 0x170005C0 RID: 1472
		// (get) Token: 0x060018F9 RID: 6393 RVA: 0x00077AB0 File Offset: 0x00075CB0
		internal bool ValidatesOnNotifyDataErrors
		{
			get
			{
				return this.TestFlag(BindingExpressionBase.PrivateFlags.iValidatesOnNotifyDataErrors);
			}
		}

		// Token: 0x170005C1 RID: 1473
		// (get) Token: 0x060018FA RID: 6394 RVA: 0x00077ABD File Offset: 0x00075CBD
		internal MultiBindingExpression ParentMultiBindingExpression
		{
			get
			{
				return this.GetValue(BindingExpressionBase.Feature.ParentBindingExpressionBase, null) as MultiBindingExpression;
			}
		}

		// Token: 0x170005C2 RID: 1474
		// (get) Token: 0x060018FB RID: 6395 RVA: 0x00077ACC File Offset: 0x00075CCC
		internal PriorityBindingExpression ParentPriorityBindingExpression
		{
			get
			{
				return this.GetValue(BindingExpressionBase.Feature.ParentBindingExpressionBase, null) as PriorityBindingExpression;
			}
		}

		// Token: 0x170005C3 RID: 1475
		// (get) Token: 0x060018FC RID: 6396 RVA: 0x00077ADB File Offset: 0x00075CDB
		internal BindingExpressionBase ParentBindingExpressionBase
		{
			get
			{
				return (BindingExpressionBase)this.GetValue(BindingExpressionBase.Feature.ParentBindingExpressionBase, null);
			}
		}

		// Token: 0x170005C4 RID: 1476
		// (get) Token: 0x060018FD RID: 6397 RVA: 0x00077AEA File Offset: 0x00075CEA
		internal object FallbackValue
		{
			get
			{
				return BindingExpressionBase.ConvertFallbackValue(this.ParentBindingBase.FallbackValue, this.TargetProperty, this);
			}
		}

		// Token: 0x170005C5 RID: 1477
		// (get) Token: 0x060018FE RID: 6398 RVA: 0x00077B04 File Offset: 0x00075D04
		internal object DefaultValue
		{
			get
			{
				DependencyObject targetElement = this.TargetElement;
				if (targetElement != null)
				{
					return this.TargetProperty.GetDefaultValue(targetElement.DependencyObjectType);
				}
				return DependencyProperty.UnsetValue;
			}
		}

		// Token: 0x170005C6 RID: 1478
		// (get) Token: 0x060018FF RID: 6399 RVA: 0x00077B32 File Offset: 0x00075D32
		internal string EffectiveStringFormat
		{
			get
			{
				return (string)this.GetValue(BindingExpressionBase.Feature.EffectiveStringFormat, null);
			}
		}

		// Token: 0x170005C7 RID: 1479
		// (get) Token: 0x06001900 RID: 6400 RVA: 0x00077B41 File Offset: 0x00075D41
		internal object EffectiveTargetNullValue
		{
			get
			{
				return this.GetValue(BindingExpressionBase.Feature.EffectiveTargetNullValue, DependencyProperty.UnsetValue);
			}
		}

		// Token: 0x170005C8 RID: 1480
		// (get) Token: 0x06001901 RID: 6401 RVA: 0x00077B50 File Offset: 0x00075D50
		internal BindingExpressionBase RootBindingExpression
		{
			get
			{
				BindingExpressionBase bindingExpressionBase = this;
				for (BindingExpressionBase parentBindingExpressionBase = this.ParentBindingExpressionBase; parentBindingExpressionBase != null; parentBindingExpressionBase = bindingExpressionBase.ParentBindingExpressionBase)
				{
					bindingExpressionBase = parentBindingExpressionBase;
				}
				return bindingExpressionBase;
			}
		}

		// Token: 0x170005C9 RID: 1481
		// (get) Token: 0x06001902 RID: 6402 RVA: 0x0000B02A File Offset: 0x0000922A
		internal virtual bool IsParentBindingUpdateTriggerDefault
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170005CA RID: 1482
		// (get) Token: 0x06001903 RID: 6403 RVA: 0x00077B75 File Offset: 0x00075D75
		internal bool UsesLanguage
		{
			get
			{
				return this.ParentBindingBase.ConverterCultureInternal == null;
			}
		}

		// Token: 0x170005CB RID: 1483
		// (get) Token: 0x06001904 RID: 6404 RVA: 0x00077B88 File Offset: 0x00075D88
		internal bool IsEligibleForCommit
		{
			get
			{
				if (this.IsDetaching)
				{
					return false;
				}
				switch (this.StatusInternal)
				{
				case BindingStatusInternal.Unattached:
				case BindingStatusInternal.Inactive:
				case BindingStatusInternal.Detached:
				case BindingStatusInternal.PathError:
					return false;
				}
				return true;
			}
		}

		// Token: 0x06001905 RID: 6405 RVA: 0x00077BCC File Offset: 0x00075DCC
		internal virtual bool AttachOverride(DependencyObject target, DependencyProperty dp)
		{
			this._targetElement = new WeakReference(target);
			this._targetProperty = dp;
			DataBindEngine currentDataBindEngine = DataBindEngine.CurrentDataBindEngine;
			if (currentDataBindEngine == null || currentDataBindEngine.IsShutDown)
			{
				return false;
			}
			this._engine = currentDataBindEngine;
			this.DetermineEffectiveStringFormat();
			this.DetermineEffectiveTargetNullValue();
			this.DetermineEffectiveUpdateBehavior();
			this.DetermineEffectiveValidatesOnNotifyDataErrors();
			if (dp == TextBox.TextProperty && this.IsReflective && !this.IsInBindingExpressionCollection)
			{
				TextBoxBase textBoxBase = target as TextBoxBase;
				if (textBoxBase != null)
				{
					textBoxBase.PreviewTextInput += this.OnPreviewTextInput;
				}
			}
			if (TraceData.IsExtendedTraceEnabled(this, TraceDataLevel.Attach))
			{
				TraceData.Trace(TraceEventType.Warning, TraceData.AttachExpression(new object[]
				{
					TraceData.Identify(this),
					target.GetType().FullName,
					dp.Name,
					AvTrace.GetHashCodeHelper(target)
				}));
			}
			return true;
		}

		// Token: 0x06001906 RID: 6406 RVA: 0x00077C9C File Offset: 0x00075E9C
		internal virtual void DetachOverride()
		{
			this.UpdateValidationError(null, false);
			this.UpdateNotifyDataErrorValidationErrors(null);
			if (this.TargetProperty == TextBox.TextProperty && this.IsReflective && !this.IsInBindingExpressionCollection)
			{
				TextBoxBase textBoxBase = this.TargetElement as TextBoxBase;
				if (textBoxBase != null)
				{
					textBoxBase.PreviewTextInput -= this.OnPreviewTextInput;
				}
			}
			this._engine = null;
			this._targetElement = null;
			this._targetProperty = null;
			this.SetStatus(BindingStatusInternal.Detached);
			if (TraceData.IsExtendedTraceEnabled(this, TraceDataLevel.Attach))
			{
				TraceData.Trace(TraceEventType.Warning, TraceData.DetachExpression(new object[]
				{
					TraceData.Identify(this)
				}));
			}
		}

		// Token: 0x06001907 RID: 6407
		internal abstract void InvalidateChild(BindingExpressionBase bindingExpression);

		// Token: 0x06001908 RID: 6408
		internal abstract void ChangeSourcesForChild(BindingExpressionBase bindingExpression, WeakDependencySource[] newSources);

		// Token: 0x06001909 RID: 6409
		internal abstract void ReplaceChild(BindingExpressionBase bindingExpression);

		// Token: 0x0600190A RID: 6410
		internal abstract void HandlePropertyInvalidation(DependencyObject d, DependencyPropertyChangedEventArgs args);

		// Token: 0x0600190B RID: 6411 RVA: 0x00077D38 File Offset: 0x00075F38
		private object HandlePropertyInvalidationOperation(object o)
		{
			object[] array = (object[])o;
			this.HandlePropertyInvalidation((DependencyObject)array[0], (DependencyPropertyChangedEventArgs)array[1]);
			return null;
		}

		// Token: 0x0600190C RID: 6412 RVA: 0x00077D64 File Offset: 0x00075F64
		internal void OnBindingGroupChanged(bool joining)
		{
			if (joining)
			{
				if (this.IsParentBindingUpdateTriggerDefault)
				{
					if (this.IsUpdateOnLostFocus)
					{
						LostFocusEventManager.RemoveHandler(this.TargetElement, new EventHandler<RoutedEventArgs>(this.OnLostFocus));
					}
					this.SetUpdateSourceTrigger(UpdateSourceTrigger.Explicit);
					return;
				}
			}
			else if (this.IsParentBindingUpdateTriggerDefault)
			{
				this.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new DispatcherOperationCallback(this.RestoreUpdateTriggerOperation), null);
			}
		}

		// Token: 0x0600190D RID: 6413 RVA: 0x00077DC8 File Offset: 0x00075FC8
		private object RestoreUpdateTriggerOperation(object arg)
		{
			DependencyObject targetElement = this.TargetElement;
			if (!this.IsDetached && targetElement != null)
			{
				FrameworkPropertyMetadata fwMetaData = this.TargetProperty.GetMetadata(targetElement.DependencyObjectType) as FrameworkPropertyMetadata;
				UpdateSourceTrigger defaultUpdateSourceTrigger = this.GetDefaultUpdateSourceTrigger(fwMetaData);
				this.SetUpdateSourceTrigger(defaultUpdateSourceTrigger);
				if (this.IsUpdateOnLostFocus)
				{
					LostFocusEventManager.AddHandler(targetElement, new EventHandler<RoutedEventArgs>(this.OnLostFocus));
				}
			}
			return null;
		}

		// Token: 0x0600190E RID: 6414
		internal abstract void UpdateBindingGroup(BindingGroup bg);

		// Token: 0x0600190F RID: 6415 RVA: 0x00077E2C File Offset: 0x0007602C
		internal bool UpdateValue()
		{
			ValidationError baseValidationError = this.BaseValidationError;
			if (this.StatusInternal == BindingStatusInternal.UpdateSourceError)
			{
				this.SetStatus(BindingStatusInternal.Active);
			}
			object obj = this.GetRawProposedValue();
			if (!this.Validate(obj, ValidationStep.RawProposedValue))
			{
				return false;
			}
			obj = this.ConvertProposedValue(obj);
			if (!this.Validate(obj, ValidationStep.ConvertedProposedValue))
			{
				return false;
			}
			obj = this.UpdateSource(obj);
			if (!this.Validate(obj, ValidationStep.UpdatedValue))
			{
				return false;
			}
			obj = this.CommitSource(obj);
			if (!this.Validate(obj, ValidationStep.CommittedValue))
			{
				return false;
			}
			if (this.BaseValidationError == baseValidationError)
			{
				this.UpdateValidationError(null, false);
			}
			this.EndSourceUpdate();
			this.NotifyCommitManager();
			return !this.HasValue(BindingExpressionBase.Feature.ValidationError);
		}

		// Token: 0x06001910 RID: 6416 RVA: 0x00077EC8 File Offset: 0x000760C8
		internal virtual object GetRawProposedValue()
		{
			object obj = this.Value;
			if (ItemsControl.EqualsEx(obj, this.EffectiveTargetNullValue))
			{
				obj = null;
			}
			return obj;
		}

		// Token: 0x06001911 RID: 6417
		internal abstract object ConvertProposedValue(object rawValue);

		// Token: 0x06001912 RID: 6418
		internal abstract bool ObtainConvertedProposedValue(BindingGroup bindingGroup);

		// Token: 0x06001913 RID: 6419
		internal abstract object UpdateSource(object convertedValue);

		// Token: 0x06001914 RID: 6420
		internal abstract bool UpdateSource(BindingGroup bindingGroup);

		// Token: 0x06001915 RID: 6421 RVA: 0x00012630 File Offset: 0x00010830
		internal virtual object CommitSource(object value)
		{
			return value;
		}

		// Token: 0x06001916 RID: 6422
		internal abstract void StoreValueInBindingGroup(object value, BindingGroup bindingGroup);

		// Token: 0x06001917 RID: 6423 RVA: 0x00077EF0 File Offset: 0x000760F0
		internal virtual bool Validate(object value, ValidationStep validationStep)
		{
			if (value == Binding.DoNothing)
			{
				return true;
			}
			if (value == DependencyProperty.UnsetValue)
			{
				this.SetStatus(BindingStatusInternal.UpdateSourceError);
				return false;
			}
			ValidationError validationError = this.GetValidationErrors(validationStep);
			if (validationError != null && validationError.RuleInError == DataErrorValidationRule.Instance)
			{
				validationError = null;
			}
			Collection<ValidationRule> validationRulesInternal = this.ParentBindingBase.ValidationRulesInternal;
			if (validationRulesInternal != null)
			{
				CultureInfo culture = this.GetCulture();
				foreach (ValidationRule validationRule in validationRulesInternal)
				{
					if (validationRule.ValidationStep == validationStep)
					{
						ValidationResult validationResult = validationRule.Validate(value, culture, this);
						if (!validationResult.IsValid)
						{
							if (TraceData.IsExtendedTraceEnabled(this, TraceDataLevel.Transfer))
							{
								TraceData.Trace(TraceEventType.Warning, TraceData.ValidationRuleFailed(new object[]
								{
									TraceData.Identify(this),
									TraceData.Identify(validationRule)
								}));
							}
							this.UpdateValidationError(new ValidationError(validationRule, this, validationResult.ErrorContent, null), false);
							return false;
						}
					}
				}
			}
			if (validationError != null && validationError == this.GetValidationErrors(validationStep))
			{
				this.UpdateValidationError(null, false);
			}
			return true;
		}

		// Token: 0x06001918 RID: 6424
		internal abstract bool CheckValidationRules(BindingGroup bindingGroup, ValidationStep validationStep);

		// Token: 0x06001919 RID: 6425
		internal abstract bool ValidateAndConvertProposedValue(out Collection<BindingExpressionBase.ProposedValue> values);

		// Token: 0x0600191A RID: 6426 RVA: 0x00078004 File Offset: 0x00076204
		internal CultureInfo GetCulture()
		{
			if (this._culture == BindingExpressionBase.DefaultValueObject)
			{
				this._culture = this.ParentBindingBase.ConverterCultureInternal;
				if (this._culture == null)
				{
					DependencyObject targetElement = this.TargetElement;
					if (targetElement != null)
					{
						if (this.IsInTransfer && this.TargetProperty == FrameworkElement.LanguageProperty)
						{
							if (TraceData.IsEnabled)
							{
								TraceData.Trace(TraceEventType.Critical, TraceData.RequiresExplicitCulture, this.TargetProperty.Name, this);
							}
							throw new InvalidOperationException(SR.Get("RequiresExplicitCulture", new object[]
							{
								this.TargetProperty.Name
							}));
						}
						this._culture = ((XmlLanguage)targetElement.GetValue(FrameworkElement.LanguageProperty)).GetSpecificCulture();
					}
				}
			}
			return (CultureInfo)this._culture;
		}

		// Token: 0x0600191B RID: 6427 RVA: 0x000780C1 File Offset: 0x000762C1
		internal void InvalidateCulture()
		{
			this._culture = BindingExpressionBase.DefaultValueObject;
		}

		// Token: 0x0600191C RID: 6428 RVA: 0x000780CE File Offset: 0x000762CE
		internal void BeginSourceUpdate()
		{
			this.ChangeFlag(BindingExpressionBase.PrivateFlags.iInUpdate, true);
		}

		// Token: 0x0600191D RID: 6429 RVA: 0x000780DC File Offset: 0x000762DC
		internal void EndSourceUpdate()
		{
			if (this.IsInUpdate && this.IsDynamic && this.StatusInternal == BindingStatusInternal.Active)
			{
				TextBoxBase textBoxBase = this.Target as TextBoxBase;
				UndoManager undoManager = (textBoxBase == null) ? null : textBoxBase.TextContainer.UndoManager;
				if (undoManager != null && undoManager.OpenedUnit != null && undoManager.OpenedUnit.GetType() != typeof(TextParentUndoUnit))
				{
					if (!this.HasValue(BindingExpressionBase.Feature.UpdateTargetOperation))
					{
						DispatcherOperation value = this.Dispatcher.BeginInvoke(DispatcherPriority.Send, new DispatcherOperationCallback(this.UpdateTargetCallback), null);
						this.SetValue(BindingExpressionBase.Feature.UpdateTargetOperation, value);
					}
				}
				else
				{
					this.UpdateTarget();
				}
			}
			this.ChangeFlag(BindingExpressionBase.PrivateFlags.iInUpdate | BindingExpressionBase.PrivateFlags.iNeedsUpdate, false);
		}

		// Token: 0x0600191E RID: 6430 RVA: 0x0007818D File Offset: 0x0007638D
		private object UpdateTargetCallback(object unused)
		{
			this.ClearValue(BindingExpressionBase.Feature.UpdateTargetOperation);
			this.IsInUpdate = true;
			this.UpdateTarget();
			this.IsInUpdate = false;
			return null;
		}

		// Token: 0x0600191F RID: 6431 RVA: 0x000781AC File Offset: 0x000763AC
		internal bool ShouldUpdateWithCurrentValue(DependencyObject target, out object currentValue)
		{
			if (this.IsReflective)
			{
				FrameworkObject frameworkObject = new FrameworkObject(target);
				if (!frameworkObject.IsInitialized)
				{
					DependencyProperty targetProperty = this.TargetProperty;
					EntryIndex entryIndex = target.LookupEntry(targetProperty.GlobalIndex);
					if (entryIndex.Found)
					{
						EffectiveValueEntry valueEntry = target.GetValueEntry(entryIndex, targetProperty, null, RequestFlags.RawEntry);
						if (valueEntry.IsCoercedWithCurrentValue)
						{
							currentValue = valueEntry.GetFlattenedEntry(RequestFlags.FullyResolved).Value;
							if (valueEntry.IsDeferredReference)
							{
								DeferredReference deferredReference = (DeferredReference)currentValue;
								currentValue = deferredReference.GetValue(valueEntry.BaseValueSourceInternal);
							}
							return true;
						}
					}
				}
			}
			currentValue = null;
			return false;
		}

		// Token: 0x06001920 RID: 6432 RVA: 0x00078240 File Offset: 0x00076440
		internal void ChangeValue(object newValue, bool notify)
		{
			object oldValue = (this._value != BindingExpressionBase.DefaultValueObject) ? this._value : DependencyProperty.UnsetValue;
			this._value = newValue;
			if (notify && this.ValueChanged != null)
			{
				this.ValueChanged(this, new BindingValueChangedEventArgs(oldValue, newValue));
			}
		}

		// Token: 0x06001921 RID: 6433 RVA: 0x0007828D File Offset: 0x0007648D
		internal void Clean()
		{
			if (this.NeedsUpdate)
			{
				this.NeedsUpdate = false;
			}
			if (!this.IsInUpdate)
			{
				this.NeedsValidation = false;
				this.NotifyCommitManager();
			}
		}

		// Token: 0x06001922 RID: 6434 RVA: 0x000782B4 File Offset: 0x000764B4
		internal void Dirty()
		{
			if (this.ShouldReactToDirty())
			{
				this.NeedsUpdate = true;
				if (!this.HasValue(BindingExpressionBase.Feature.Timer))
				{
					this.ProcessDirty();
				}
				else
				{
					DispatcherTimer dispatcherTimer = (DispatcherTimer)this.GetValue(BindingExpressionBase.Feature.Timer, null);
					dispatcherTimer.Stop();
					dispatcherTimer.Start();
				}
				this.NotifyCommitManager();
			}
		}

		// Token: 0x06001923 RID: 6435 RVA: 0x00078301 File Offset: 0x00076501
		private bool ShouldReactToDirty()
		{
			return !this.IsInTransfer && this.IsAttached && this.ShouldReactToDirtyOverride();
		}

		// Token: 0x06001924 RID: 6436 RVA: 0x00016748 File Offset: 0x00014948
		internal virtual bool ShouldReactToDirtyOverride()
		{
			return true;
		}

		// Token: 0x06001925 RID: 6437 RVA: 0x0007831B File Offset: 0x0007651B
		private void ProcessDirty()
		{
			if (this.IsUpdateOnPropertyChanged)
			{
				if (Helper.IsComposing(this.Target, this.TargetProperty))
				{
					this.IsUpdateDeferredForComposition = true;
					return;
				}
				this.Update();
			}
		}

		// Token: 0x06001926 RID: 6438 RVA: 0x00078347 File Offset: 0x00076547
		private void OnTimerTick(object sender, EventArgs e)
		{
			this.ProcessDirty();
		}

		// Token: 0x06001927 RID: 6439 RVA: 0x0007834F File Offset: 0x0007654F
		private void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			if (this.IsUpdateDeferredForComposition && e.TextComposition.Source == this.TargetElement && e.TextComposition.Stage == TextCompositionStage.Done)
			{
				this.IsUpdateDeferredForComposition = false;
				this.Dirty();
			}
		}

		// Token: 0x06001928 RID: 6440 RVA: 0x00078388 File Offset: 0x00076588
		internal void Invalidate(bool isASubPropertyChange)
		{
			if (this.IsAttaching)
			{
				return;
			}
			DependencyObject targetElement = this.TargetElement;
			if (targetElement != null)
			{
				if (this.IsInBindingExpressionCollection)
				{
					this.ParentBindingExpressionBase.InvalidateChild(this);
					return;
				}
				if (this.TargetProperty != BindingExpressionBase.NoTargetProperty)
				{
					if (!isASubPropertyChange)
					{
						targetElement.InvalidateProperty(this.TargetProperty);
						return;
					}
					targetElement.NotifySubPropertyChange(this.TargetProperty);
				}
			}
		}

		// Token: 0x06001929 RID: 6441 RVA: 0x000783E8 File Offset: 0x000765E8
		internal object UseFallbackValue()
		{
			object obj = this.FallbackValue;
			if (obj == BindingExpressionBase.DefaultValueObject)
			{
				obj = DependencyProperty.UnsetValue;
			}
			if (obj == DependencyProperty.UnsetValue && this.IsOneWayToSource)
			{
				obj = this.DefaultValue;
			}
			if (obj != DependencyProperty.UnsetValue)
			{
				this.UsingFallbackValue = true;
			}
			else
			{
				if (this.StatusInternal == BindingStatusInternal.Active)
				{
					this.SetStatus(BindingStatusInternal.UpdateTargetError);
				}
				if (!this.IsInBindingExpressionCollection)
				{
					obj = this.DefaultValue;
					if (TraceData.IsEnabled)
					{
						TraceData.Trace(TraceEventType.Information, TraceData.NoValueToTransfer, this);
					}
				}
			}
			return obj;
		}

		// Token: 0x0600192A RID: 6442 RVA: 0x00078465 File Offset: 0x00076665
		internal static bool IsNullValue(object value)
		{
			return value == null || Convert.IsDBNull(value) || SystemDataHelper.IsSqlNull(value);
		}

		// Token: 0x0600192B RID: 6443 RVA: 0x00078484 File Offset: 0x00076684
		internal object NullValueForType(Type type)
		{
			if (type == null)
			{
				return null;
			}
			if (SystemDataHelper.IsSqlNullableType(type))
			{
				return SystemDataHelper.NullValueForSqlNullableType(type);
			}
			if (!type.IsValueType)
			{
				return null;
			}
			if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
			{
				return null;
			}
			return DependencyProperty.UnsetValue;
		}

		// Token: 0x0600192C RID: 6444 RVA: 0x000784DC File Offset: 0x000766DC
		internal ValidationRule LookupValidationRule(Type type)
		{
			ValidationRule validationRule = this.ParentBindingBase.GetValidationRule(type);
			if (validationRule == null && this.HasValue(BindingExpressionBase.Feature.ParentBindingExpressionBase))
			{
				validationRule = this.ParentBindingExpressionBase.LookupValidationRule(type);
			}
			return validationRule;
		}

		// Token: 0x0600192D RID: 6445 RVA: 0x00078510 File Offset: 0x00076710
		internal void JoinBindingGroup(bool isReflective, DependencyObject contextElement)
		{
			BindingGroup bindingGroup = this.RootBindingExpression.FindBindingGroup(isReflective, contextElement);
			if (bindingGroup != null)
			{
				this.JoinBindingGroup(bindingGroup, false);
			}
		}

		// Token: 0x0600192E RID: 6446 RVA: 0x00078538 File Offset: 0x00076738
		internal void LeaveBindingGroup()
		{
			BindingExpressionBase rootBindingExpression = this.RootBindingExpression;
			BindingGroup bindingGroup = rootBindingExpression.BindingGroup;
			if (bindingGroup != null)
			{
				bindingGroup.BindingExpressions.Remove(rootBindingExpression);
				rootBindingExpression.ClearValue(BindingExpressionBase.Feature.BindingGroup);
			}
		}

		// Token: 0x0600192F RID: 6447 RVA: 0x0007856C File Offset: 0x0007676C
		internal void RejoinBindingGroup(bool isReflective, DependencyObject contextElement)
		{
			BindingExpressionBase rootBindingExpression = this.RootBindingExpression;
			BindingGroup bindingGroup = rootBindingExpression.BindingGroup;
			WeakReference<BindingGroup> weakReference = (WeakReference<BindingGroup>)rootBindingExpression.GetValue(BindingExpressionBase.Feature.BindingGroup, null);
			rootBindingExpression.SetValue(BindingExpressionBase.Feature.BindingGroup, null, weakReference);
			BindingGroup bindingGroup2;
			try
			{
				bindingGroup2 = rootBindingExpression.FindBindingGroup(isReflective, contextElement);
			}
			finally
			{
				rootBindingExpression.SetValue(BindingExpressionBase.Feature.BindingGroup, weakReference, null);
			}
			if (bindingGroup != bindingGroup2)
			{
				rootBindingExpression.LeaveBindingGroup();
				if (bindingGroup2 != null)
				{
					this.JoinBindingGroup(bindingGroup2, false);
				}
			}
		}

		// Token: 0x06001930 RID: 6448 RVA: 0x000785D8 File Offset: 0x000767D8
		internal BindingGroup FindBindingGroup(bool isReflective, DependencyObject contextElement)
		{
			if ((WeakReference<BindingGroup>)this.GetValue(BindingExpressionBase.Feature.BindingGroup, null) != null)
			{
				return this.BindingGroup;
			}
			string bindingGroupName = this.ParentBindingBase.BindingGroupName;
			if (bindingGroupName == null)
			{
				this.MarkAsNonGrouped();
				return null;
			}
			if (!string.IsNullOrEmpty(bindingGroupName))
			{
				DependencyProperty bindingGroupProperty = FrameworkElement.BindingGroupProperty;
				FrameworkObject frameworkParent = new FrameworkObject(Helper.FindMentor(this.TargetElement));
				while (frameworkParent.DO != null)
				{
					BindingGroup bindingGroup = (BindingGroup)frameworkParent.DO.GetValue(bindingGroupProperty);
					if (bindingGroup == null)
					{
						this.MarkAsNonGrouped();
						return null;
					}
					if (bindingGroup.Name == bindingGroupName)
					{
						if (bindingGroup.SharesProposedValues && TraceData.IsEnabled)
						{
							TraceData.Trace(TraceEventType.Warning, TraceData.SharesProposedValuesRequriesImplicitBindingGroup(new object[]
							{
								TraceData.Identify(this),
								bindingGroupName,
								TraceData.Identify(bindingGroup)
							}));
						}
						return bindingGroup;
					}
					frameworkParent = frameworkParent.FrameworkParent;
				}
				if (TraceData.IsEnabled)
				{
					TraceData.Trace(TraceEventType.Error, TraceData.BindingGroupNameMatchFailed(new object[]
					{
						bindingGroupName
					}), this);
				}
				this.MarkAsNonGrouped();
				return null;
			}
			if (!isReflective || contextElement == null)
			{
				return null;
			}
			BindingGroup bindingGroup2 = (BindingGroup)contextElement.GetValue(FrameworkElement.BindingGroupProperty);
			if (bindingGroup2 == null)
			{
				this.MarkAsNonGrouped();
				return null;
			}
			DependencyProperty dataContextProperty = FrameworkElement.DataContextProperty;
			DependencyObject inheritanceContext = bindingGroup2.InheritanceContext;
			if (inheritanceContext == null || !ItemsControl.EqualsEx(contextElement.GetValue(dataContextProperty), inheritanceContext.GetValue(dataContextProperty)))
			{
				this.MarkAsNonGrouped();
				return null;
			}
			return bindingGroup2;
		}

		// Token: 0x06001931 RID: 6449 RVA: 0x0007872C File Offset: 0x0007692C
		internal void JoinBindingGroup(BindingGroup bg, bool explicitJoin)
		{
			BindingExpressionBase bindingExpressionBase = null;
			for (BindingExpressionBase bindingExpressionBase2 = this; bindingExpressionBase2 != null; bindingExpressionBase2 = bindingExpressionBase2.ParentBindingExpressionBase)
			{
				bindingExpressionBase = bindingExpressionBase2;
				bindingExpressionBase2.OnBindingGroupChanged(true);
				bg.AddToValueTable(bindingExpressionBase2);
			}
			if (!bindingExpressionBase.HasValue(BindingExpressionBase.Feature.BindingGroup))
			{
				bindingExpressionBase.SetValue(BindingExpressionBase.Feature.BindingGroup, new WeakReference<BindingGroup>(bg));
				bool flag = !explicitJoin || !bg.BindingExpressions.Contains(bindingExpressionBase);
				if (flag)
				{
					bg.BindingExpressions.Add(bindingExpressionBase);
				}
				if (explicitJoin)
				{
					bindingExpressionBase.UpdateBindingGroup(bg);
					if (bg.SharesProposedValues && TraceData.IsEnabled)
					{
						TraceData.Trace(TraceEventType.Warning, TraceData.SharesProposedValuesRequriesImplicitBindingGroup(new object[]
						{
							TraceData.Identify(bindingExpressionBase),
							bindingExpressionBase.ParentBindingBase.BindingGroupName,
							TraceData.Identify(bg)
						}));
						return;
					}
				}
			}
			else if (bindingExpressionBase.BindingGroup != bg)
			{
				throw new InvalidOperationException(SR.Get("BindingGroup_CannotChangeGroups"));
			}
		}

		// Token: 0x06001932 RID: 6450 RVA: 0x000787F8 File Offset: 0x000769F8
		private void MarkAsNonGrouped()
		{
			if (!(this is BindingExpression))
			{
				this.SetValue(BindingExpressionBase.Feature.BindingGroup, BindingExpressionBase.NullBindingGroupReference);
			}
		}

		// Token: 0x06001933 RID: 6451 RVA: 0x00078810 File Offset: 0x00076A10
		internal void NotifyCommitManager()
		{
			if (this.IsReflective && !this.IsDetached && !this.Engine.IsShutDown)
			{
				bool flag = this.IsEligibleForCommit && (this.IsDirty || this.HasValidationError);
				BindingExpressionBase rootBindingExpression = this.RootBindingExpression;
				BindingGroup bindingGroup = rootBindingExpression.BindingGroup;
				rootBindingExpression.UpdateCommitState();
				if (bindingGroup == null)
				{
					if (rootBindingExpression != this && !flag)
					{
						flag = (rootBindingExpression.IsEligibleForCommit && (rootBindingExpression.IsDirty || rootBindingExpression.HasValidationError));
					}
					if (flag)
					{
						this.Engine.CommitManager.AddBinding(rootBindingExpression);
						return;
					}
					this.Engine.CommitManager.RemoveBinding(rootBindingExpression);
					return;
				}
				else
				{
					if (!flag)
					{
						flag = (bindingGroup.Owner != null && (bindingGroup.IsDirty || bindingGroup.HasValidationError));
					}
					if (flag)
					{
						this.Engine.CommitManager.AddBindingGroup(bindingGroup);
						return;
					}
					this.Engine.CommitManager.RemoveBindingGroup(bindingGroup);
				}
			}
		}

		// Token: 0x06001934 RID: 6452 RVA: 0x00002137 File Offset: 0x00000337
		internal virtual void UpdateCommitState()
		{
		}

		// Token: 0x06001935 RID: 6453 RVA: 0x00078908 File Offset: 0x00076B08
		internal void AdoptProperties(BindingExpressionBase bb)
		{
			BindingExpressionBase.PrivateFlags privateFlags = (bb != null) ? bb._flags : (~(BindingExpressionBase.PrivateFlags.iSourceToTarget | BindingExpressionBase.PrivateFlags.iTargetToSource | BindingExpressionBase.PrivateFlags.iPropDefault | BindingExpressionBase.PrivateFlags.iNotifyOnTargetUpdated | BindingExpressionBase.PrivateFlags.iDefaultValueConverter | BindingExpressionBase.PrivateFlags.iInTransfer | BindingExpressionBase.PrivateFlags.iInUpdate | BindingExpressionBase.PrivateFlags.iTransferPending | BindingExpressionBase.PrivateFlags.iNeedDataTransfer | BindingExpressionBase.PrivateFlags.iTransferDeferred | BindingExpressionBase.PrivateFlags.iUpdateOnLostFocus | BindingExpressionBase.PrivateFlags.iUpdateExplicitly | BindingExpressionBase.PrivateFlags.iNeedsUpdate | BindingExpressionBase.PrivateFlags.iPathGeneratedInternally | BindingExpressionBase.PrivateFlags.iUsingMentor | BindingExpressionBase.PrivateFlags.iResolveNamesInTemplate | BindingExpressionBase.PrivateFlags.iDetaching | BindingExpressionBase.PrivateFlags.iNeedsCollectionView | BindingExpressionBase.PrivateFlags.iInPriorityBindingExpression | BindingExpressionBase.PrivateFlags.iInMultiBindingExpression | BindingExpressionBase.PrivateFlags.iUsingFallbackValue | BindingExpressionBase.PrivateFlags.iNotifyOnValidationError | BindingExpressionBase.PrivateFlags.iAttaching | BindingExpressionBase.PrivateFlags.iNotifyOnSourceUpdated | BindingExpressionBase.PrivateFlags.iValidatesOnExceptions | BindingExpressionBase.PrivateFlags.iValidatesOnDataErrors | BindingExpressionBase.PrivateFlags.iIllegalInput | BindingExpressionBase.PrivateFlags.iNeedsValidation | BindingExpressionBase.PrivateFlags.iTargetWantsXTNotification | BindingExpressionBase.PrivateFlags.iValidatesOnNotifyDataErrors | BindingExpressionBase.PrivateFlags.iDataErrorsChangedPending | BindingExpressionBase.PrivateFlags.iDeferUpdateForComposition));
			this._flags = ((this._flags & ~(BindingExpressionBase.PrivateFlags.iSourceToTarget | BindingExpressionBase.PrivateFlags.iTargetToSource | BindingExpressionBase.PrivateFlags.iNeedsUpdate | BindingExpressionBase.PrivateFlags.iNeedsValidation)) | (privateFlags & BindingExpressionBase.PrivateFlags.iAdoptionMask));
		}

		// Token: 0x06001936 RID: 6454 RVA: 0x0000B02A File Offset: 0x0000922A
		internal virtual bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
		{
			return false;
		}

		// Token: 0x06001937 RID: 6455 RVA: 0x00002137 File Offset: 0x00000337
		internal virtual void OnLostFocus(object sender, RoutedEventArgs e)
		{
		}

		// Token: 0x06001938 RID: 6456
		internal abstract object GetSourceItem(object newValue);

		// Token: 0x06001939 RID: 6457 RVA: 0x0007893C File Offset: 0x00076B3C
		private bool TestFlag(BindingExpressionBase.PrivateFlags flag)
		{
			return (this._flags & flag) > ~(BindingExpressionBase.PrivateFlags.iSourceToTarget | BindingExpressionBase.PrivateFlags.iTargetToSource | BindingExpressionBase.PrivateFlags.iPropDefault | BindingExpressionBase.PrivateFlags.iNotifyOnTargetUpdated | BindingExpressionBase.PrivateFlags.iDefaultValueConverter | BindingExpressionBase.PrivateFlags.iInTransfer | BindingExpressionBase.PrivateFlags.iInUpdate | BindingExpressionBase.PrivateFlags.iTransferPending | BindingExpressionBase.PrivateFlags.iNeedDataTransfer | BindingExpressionBase.PrivateFlags.iTransferDeferred | BindingExpressionBase.PrivateFlags.iUpdateOnLostFocus | BindingExpressionBase.PrivateFlags.iUpdateExplicitly | BindingExpressionBase.PrivateFlags.iNeedsUpdate | BindingExpressionBase.PrivateFlags.iPathGeneratedInternally | BindingExpressionBase.PrivateFlags.iUsingMentor | BindingExpressionBase.PrivateFlags.iResolveNamesInTemplate | BindingExpressionBase.PrivateFlags.iDetaching | BindingExpressionBase.PrivateFlags.iNeedsCollectionView | BindingExpressionBase.PrivateFlags.iInPriorityBindingExpression | BindingExpressionBase.PrivateFlags.iInMultiBindingExpression | BindingExpressionBase.PrivateFlags.iUsingFallbackValue | BindingExpressionBase.PrivateFlags.iNotifyOnValidationError | BindingExpressionBase.PrivateFlags.iAttaching | BindingExpressionBase.PrivateFlags.iNotifyOnSourceUpdated | BindingExpressionBase.PrivateFlags.iValidatesOnExceptions | BindingExpressionBase.PrivateFlags.iValidatesOnDataErrors | BindingExpressionBase.PrivateFlags.iIllegalInput | BindingExpressionBase.PrivateFlags.iNeedsValidation | BindingExpressionBase.PrivateFlags.iTargetWantsXTNotification | BindingExpressionBase.PrivateFlags.iValidatesOnNotifyDataErrors | BindingExpressionBase.PrivateFlags.iDataErrorsChangedPending | BindingExpressionBase.PrivateFlags.iDeferUpdateForComposition);
		}

		// Token: 0x0600193A RID: 6458 RVA: 0x00078949 File Offset: 0x00076B49
		private void ChangeFlag(BindingExpressionBase.PrivateFlags flag, bool value)
		{
			if (value)
			{
				this._flags |= flag;
				return;
			}
			this._flags &= ~flag;
		}

		// Token: 0x170005CC RID: 1484
		// (get) Token: 0x0600193B RID: 6459 RVA: 0x0007896C File Offset: 0x00076B6C
		internal DependencyObject TargetElement
		{
			get
			{
				if (this._targetElement != null)
				{
					DependencyObject dependencyObject = this._targetElement.Target as DependencyObject;
					if (dependencyObject != null)
					{
						return dependencyObject;
					}
					this._targetElement = null;
					this.Detach();
				}
				return null;
			}
		}

		// Token: 0x170005CD RID: 1485
		// (get) Token: 0x0600193C RID: 6460 RVA: 0x000789A5 File Offset: 0x00076BA5
		internal WeakReference TargetElementReference
		{
			get
			{
				return this._targetElement;
			}
		}

		// Token: 0x170005CE RID: 1486
		// (get) Token: 0x0600193D RID: 6461 RVA: 0x000789AD File Offset: 0x00076BAD
		internal DataBindEngine Engine
		{
			get
			{
				return this._engine;
			}
		}

		// Token: 0x170005CF RID: 1487
		// (get) Token: 0x0600193E RID: 6462 RVA: 0x000789B5 File Offset: 0x00076BB5
		internal Dispatcher Dispatcher
		{
			get
			{
				if (this._engine == null)
				{
					return null;
				}
				return this._engine.Dispatcher;
			}
		}

		// Token: 0x170005D0 RID: 1488
		// (get) Token: 0x0600193F RID: 6463 RVA: 0x000789CC File Offset: 0x00076BCC
		// (set) Token: 0x06001940 RID: 6464 RVA: 0x000789EE File Offset: 0x00076BEE
		internal object Value
		{
			get
			{
				if (this._value == BindingExpressionBase.DefaultValueObject)
				{
					this.ChangeValue(this.UseFallbackValue(), false);
				}
				return this._value;
			}
			set
			{
				this.ChangeValue(value, true);
				this.Dirty();
			}
		}

		// Token: 0x170005D1 RID: 1489
		// (get) Token: 0x06001941 RID: 6465 RVA: 0x000789FE File Offset: 0x00076BFE
		internal WeakDependencySource[] WeakSources
		{
			get
			{
				return this._sources;
			}
		}

		// Token: 0x170005D2 RID: 1490
		// (get) Token: 0x06001942 RID: 6466 RVA: 0x0000B02A File Offset: 0x0000922A
		internal virtual bool IsDisconnected
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06001943 RID: 6467 RVA: 0x00078A06 File Offset: 0x00076C06
		internal void Attach(DependencyObject target, DependencyProperty dp)
		{
			if (target != null)
			{
				target.VerifyAccess();
			}
			this.IsAttaching = true;
			this.AttachOverride(target, dp);
			this.IsAttaching = false;
		}

		// Token: 0x06001944 RID: 6468 RVA: 0x00078A28 File Offset: 0x00076C28
		internal void Detach()
		{
			if (this.IsDetached || this.IsDetaching)
			{
				return;
			}
			this.IsDetaching = true;
			this.LeaveBindingGroup();
			this.NotifyCommitManager();
			this.DetachOverride();
			this.IsDetaching = false;
		}

		// Token: 0x06001945 RID: 6469 RVA: 0x00078A5C File Offset: 0x00076C5C
		internal virtual void Disconnect()
		{
			object obj = DependencyProperty.UnsetValue;
			DependencyProperty targetProperty = this.TargetProperty;
			if (targetProperty == ContentControl.ContentProperty || targetProperty == ContentPresenter.ContentProperty || targetProperty == HeaderedItemsControl.HeaderProperty || targetProperty == HeaderedContentControl.HeaderProperty)
			{
				obj = BindingExpressionBase.DisconnectedItem;
			}
			if (targetProperty.PropertyType == typeof(IEnumerable))
			{
				obj = null;
			}
			if (obj != DependencyProperty.UnsetValue)
			{
				this.ChangeValue(obj, false);
				this.Invalidate(false);
			}
		}

		// Token: 0x06001946 RID: 6470 RVA: 0x00078ACC File Offset: 0x00076CCC
		internal void SetStatus(BindingStatusInternal status)
		{
			if (this.IsDetached && status != this._status)
			{
				throw new InvalidOperationException(SR.Get("BindingExpressionStatusChanged", new object[]
				{
					this._status,
					status
				}));
			}
			this._status = status;
		}

		// Token: 0x06001947 RID: 6471 RVA: 0x00078B20 File Offset: 0x00076D20
		internal static object ConvertFallbackValue(object value, DependencyProperty dp, object sender)
		{
			Exception p;
			object obj = BindingExpressionBase.ConvertValue(value, dp, out p);
			if (obj == BindingExpressionBase.DefaultValueObject && TraceData.IsEnabled)
			{
				TraceData.Trace(TraceEventType.Error, TraceData.FallbackConversionFailed(new object[]
				{
					AvTrace.ToStringHelper(value),
					AvTrace.TypeName(value),
					dp.Name,
					dp.PropertyType.Name
				}), sender, p);
			}
			return obj;
		}

		// Token: 0x06001948 RID: 6472 RVA: 0x00078B84 File Offset: 0x00076D84
		internal static object ConvertTargetNullValue(object value, DependencyProperty dp, object sender)
		{
			Exception p;
			object obj = BindingExpressionBase.ConvertValue(value, dp, out p);
			if (obj == BindingExpressionBase.DefaultValueObject && TraceData.IsEnabled)
			{
				TraceData.Trace(TraceEventType.Error, TraceData.TargetNullValueConversionFailed(new object[]
				{
					AvTrace.ToStringHelper(value),
					AvTrace.TypeName(value),
					dp.Name,
					dp.PropertyType.Name
				}), sender, p);
			}
			return obj;
		}

		// Token: 0x06001949 RID: 6473 RVA: 0x00078BE8 File Offset: 0x00076DE8
		private static object ConvertValue(object value, DependencyProperty dp, out Exception e)
		{
			e = null;
			object obj;
			if (value == DependencyProperty.UnsetValue || dp.IsValidValue(value))
			{
				obj = value;
			}
			else
			{
				obj = null;
				bool flag = false;
				TypeConverter converter = DefaultValueConverter.GetConverter(dp.PropertyType);
				if (converter != null && converter.CanConvertFrom(value.GetType()))
				{
					try
					{
						obj = converter.ConvertFrom(null, CultureInfo.InvariantCulture, value);
						flag = dp.IsValidValue(obj);
					}
					catch (Exception ex)
					{
						e = ex;
					}
					catch
					{
					}
				}
				if (!flag)
				{
					obj = BindingExpressionBase.DefaultValueObject;
				}
			}
			return obj;
		}

		// Token: 0x170005D3 RID: 1491
		// (get) Token: 0x0600194A RID: 6474 RVA: 0x00078C78 File Offset: 0x00076E78
		internal TraceEventType TraceLevel
		{
			get
			{
				if (this.ParentBindingBase.FallbackValue != DependencyProperty.UnsetValue)
				{
					return TraceEventType.Warning;
				}
				if (this.IsInBindingExpressionCollection)
				{
					return TraceEventType.Warning;
				}
				return TraceEventType.Error;
			}
		}

		// Token: 0x0600194B RID: 6475 RVA: 0x00002137 File Offset: 0x00000337
		internal virtual void Activate()
		{
		}

		// Token: 0x0600194C RID: 6476 RVA: 0x00002137 File Offset: 0x00000337
		internal virtual void Deactivate()
		{
		}

		// Token: 0x0600194D RID: 6477 RVA: 0x00078C9C File Offset: 0x00076E9C
		internal bool Update()
		{
			if (this.HasValue(BindingExpressionBase.Feature.Timer))
			{
				DispatcherTimer dispatcherTimer = (DispatcherTimer)this.GetValue(BindingExpressionBase.Feature.Timer, null);
				dispatcherTimer.Stop();
			}
			return this.UpdateOverride();
		}

		// Token: 0x0600194E RID: 6478 RVA: 0x00016748 File Offset: 0x00014948
		internal virtual bool UpdateOverride()
		{
			return true;
		}

		// Token: 0x0600194F RID: 6479 RVA: 0x00078CCC File Offset: 0x00076ECC
		internal void UpdateValidationError(ValidationError validationError, bool skipBindingGroup = false)
		{
			ValidationError baseValidationError = this.BaseValidationError;
			this.SetValue(BindingExpressionBase.Feature.ValidationError, validationError, null);
			if (validationError != null)
			{
				this.AddValidationError(validationError, skipBindingGroup);
			}
			if (baseValidationError != null)
			{
				this.RemoveValidationError(baseValidationError, skipBindingGroup);
			}
		}

		// Token: 0x06001950 RID: 6480 RVA: 0x00078D00 File Offset: 0x00076F00
		internal void UpdateNotifyDataErrorValidationErrors(List<object> errors)
		{
			List<object> list;
			List<ValidationError> list2;
			BindingExpressionBase.GetValidationDelta(this.NotifyDataErrors, errors, out list, out list2);
			if (list != null && list.Count > 0)
			{
				ValidationRule instance = NotifyDataErrorValidationRule.Instance;
				List<ValidationError> list3 = this.NotifyDataErrors;
				if (list3 == null)
				{
					list3 = new List<ValidationError>();
					this.SetValue(BindingExpressionBase.Feature.NotifyDataErrors, list3);
				}
				foreach (object errorContent in list)
				{
					ValidationError validationError = new ValidationError(instance, this, errorContent, null);
					list3.Add(validationError);
					this.AddValidationError(validationError, false);
				}
			}
			if (list2 != null && list2.Count > 0)
			{
				List<ValidationError> notifyDataErrors = this.NotifyDataErrors;
				foreach (ValidationError validationError2 in list2)
				{
					notifyDataErrors.Remove(validationError2);
					this.RemoveValidationError(validationError2, false);
				}
				if (notifyDataErrors.Count == 0)
				{
					this.ClearValue(BindingExpressionBase.Feature.NotifyDataErrors);
				}
			}
		}

		// Token: 0x06001951 RID: 6481 RVA: 0x00078E10 File Offset: 0x00077010
		internal static void GetValidationDelta(List<ValidationError> previousErrors, List<object> errors, out List<object> toAdd, out List<ValidationError> toRemove)
		{
			if (previousErrors == null || previousErrors.Count == 0)
			{
				toAdd = errors;
				toRemove = null;
				return;
			}
			if (errors == null || errors.Count == 0)
			{
				toAdd = null;
				toRemove = new List<ValidationError>(previousErrors);
				return;
			}
			toAdd = new List<object>();
			toRemove = new List<ValidationError>(previousErrors);
			for (int i = errors.Count - 1; i >= 0; i--)
			{
				object obj = errors[i];
				int j;
				for (j = toRemove.Count - 1; j >= 0; j--)
				{
					if (ItemsControl.EqualsEx(toRemove[j].ErrorContent, obj))
					{
						toRemove.RemoveAt(j);
						break;
					}
				}
				if (j < 0)
				{
					toAdd.Add(obj);
				}
			}
		}

		// Token: 0x06001952 RID: 6482 RVA: 0x00078EB0 File Offset: 0x000770B0
		internal void AddValidationError(ValidationError validationError, bool skipBindingGroup = false)
		{
			Validation.AddValidationError(validationError, this.TargetElement, this.NotifyOnValidationError);
			if (!skipBindingGroup)
			{
				BindingGroup bindingGroup = this.BindingGroup;
				if (bindingGroup != null)
				{
					bindingGroup.AddValidationError(validationError);
				}
			}
		}

		// Token: 0x06001953 RID: 6483 RVA: 0x00078EE4 File Offset: 0x000770E4
		internal void RemoveValidationError(ValidationError validationError, bool skipBindingGroup = false)
		{
			Validation.RemoveValidationError(validationError, this.TargetElement, this.NotifyOnValidationError);
			if (!skipBindingGroup)
			{
				BindingGroup bindingGroup = this.BindingGroup;
				if (bindingGroup != null)
				{
					bindingGroup.RemoveValidationError(validationError);
				}
			}
		}

		// Token: 0x06001954 RID: 6484 RVA: 0x00078F18 File Offset: 0x00077118
		internal ValidationError GetValidationErrors(ValidationStep validationStep)
		{
			ValidationError baseValidationError = this.BaseValidationError;
			if (baseValidationError == null || baseValidationError.RuleInError.ValidationStep != validationStep)
			{
				return null;
			}
			return baseValidationError;
		}

		// Token: 0x06001955 RID: 6485 RVA: 0x00078F40 File Offset: 0x00077140
		internal void ChangeSources(WeakDependencySource[] newSources)
		{
			if (this.IsInBindingExpressionCollection)
			{
				this.ParentBindingExpressionBase.ChangeSourcesForChild(this, newSources);
			}
			else
			{
				this.ChangeSources(this.TargetElement, this.TargetProperty, newSources);
			}
			this._sources = newSources;
		}

		// Token: 0x06001956 RID: 6486 RVA: 0x00078F74 File Offset: 0x00077174
		internal static WeakDependencySource[] CombineSources(int index, Collection<BindingExpressionBase> bindingExpressions, int count, WeakDependencySource[] newSources, WeakDependencySource[] commonSources = null)
		{
			if (index == count)
			{
				count++;
			}
			Collection<WeakDependencySource> collection = new Collection<WeakDependencySource>();
			if (commonSources != null)
			{
				for (int i = 0; i < commonSources.Length; i++)
				{
					collection.Add(commonSources[i]);
				}
			}
			for (int j = 0; j < count; j++)
			{
				BindingExpressionBase bindingExpressionBase = bindingExpressions[j];
				WeakDependencySource[] array = (j == index) ? newSources : ((bindingExpressionBase != null) ? bindingExpressionBase.WeakSources : null);
				int num = (array == null) ? 0 : array.Length;
				for (int k = 0; k < num; k++)
				{
					WeakDependencySource weakDependencySource = array[k];
					for (int l = 0; l < collection.Count; l++)
					{
						WeakDependencySource weakDependencySource2 = collection[l];
						if (weakDependencySource.DependencyObject == weakDependencySource2.DependencyObject && weakDependencySource.DependencyProperty == weakDependencySource2.DependencyProperty)
						{
							weakDependencySource = null;
							break;
						}
					}
					if (weakDependencySource != null)
					{
						collection.Add(weakDependencySource);
					}
				}
			}
			WeakDependencySource[] array2;
			if (collection.Count > 0)
			{
				array2 = new WeakDependencySource[collection.Count];
				collection.CopyTo(array2, 0);
				collection.Clear();
			}
			else
			{
				array2 = null;
			}
			return array2;
		}

		// Token: 0x06001957 RID: 6487 RVA: 0x0007907C File Offset: 0x0007727C
		internal void ResolvePropertyDefaultSettings(BindingMode mode, UpdateSourceTrigger updateTrigger, FrameworkPropertyMetadata fwMetaData)
		{
			if (mode == BindingMode.Default)
			{
				BindingExpressionBase.BindingFlags bindingFlags = BindingExpressionBase.BindingFlags.OneWay;
				if (fwMetaData != null && fwMetaData.BindsTwoWayByDefault)
				{
					bindingFlags = BindingExpressionBase.BindingFlags.TwoWay;
				}
				this.ChangeFlag(BindingExpressionBase.PrivateFlags.iPropagationMask, false);
				this.ChangeFlag((BindingExpressionBase.PrivateFlags)bindingFlags, true);
				if (TraceData.IsExtendedTraceEnabled(this, TraceDataLevel.CreateExpression))
				{
					TraceData.Trace(TraceEventType.Warning, TraceData.ResolveDefaultMode(new object[]
					{
						TraceData.Identify(this),
						(bindingFlags == BindingExpressionBase.BindingFlags.OneWay) ? BindingMode.OneWay : BindingMode.TwoWay
					}));
				}
			}
			if (updateTrigger == UpdateSourceTrigger.Default)
			{
				UpdateSourceTrigger defaultUpdateSourceTrigger = this.GetDefaultUpdateSourceTrigger(fwMetaData);
				this.SetUpdateSourceTrigger(defaultUpdateSourceTrigger);
				if (TraceData.IsExtendedTraceEnabled(this, TraceDataLevel.CreateExpression))
				{
					TraceData.Trace(TraceEventType.Warning, TraceData.ResolveDefaultUpdate(new object[]
					{
						TraceData.Identify(this),
						defaultUpdateSourceTrigger
					}));
				}
			}
			Invariant.Assert((this._flags & BindingExpressionBase.PrivateFlags.iUpdateDefault) != BindingExpressionBase.PrivateFlags.iUpdateDefault, "BindingExpression should not have Default update trigger");
		}

		// Token: 0x06001958 RID: 6488 RVA: 0x00079140 File Offset: 0x00077340
		internal UpdateSourceTrigger GetDefaultUpdateSourceTrigger(FrameworkPropertyMetadata fwMetaData)
		{
			return this.IsInMultiBindingExpression ? UpdateSourceTrigger.Explicit : ((fwMetaData != null) ? fwMetaData.DefaultUpdateSourceTrigger : UpdateSourceTrigger.PropertyChanged);
		}

		// Token: 0x06001959 RID: 6489 RVA: 0x00079166 File Offset: 0x00077366
		internal void SetUpdateSourceTrigger(UpdateSourceTrigger ust)
		{
			this.ChangeFlag(BindingExpressionBase.PrivateFlags.iUpdateDefault, false);
			this.ChangeFlag((BindingExpressionBase.PrivateFlags)BindingBase.FlagsFrom(ust), true);
		}

		// Token: 0x0600195A RID: 6490 RVA: 0x00079184 File Offset: 0x00077384
		internal Type GetEffectiveTargetType()
		{
			Type result = this.TargetProperty.PropertyType;
			for (BindingExpressionBase parentBindingExpressionBase = this.ParentBindingExpressionBase; parentBindingExpressionBase != null; parentBindingExpressionBase = parentBindingExpressionBase.ParentBindingExpressionBase)
			{
				if (parentBindingExpressionBase is MultiBindingExpression)
				{
					result = typeof(object);
					break;
				}
			}
			return result;
		}

		// Token: 0x0600195B RID: 6491 RVA: 0x000791C8 File Offset: 0x000773C8
		internal void DetermineEffectiveStringFormat()
		{
			Type left = this.TargetProperty.PropertyType;
			if (left != typeof(string))
			{
				return;
			}
			string stringFormat = this.ParentBindingBase.StringFormat;
			for (BindingExpressionBase parentBindingExpressionBase = this.ParentBindingExpressionBase; parentBindingExpressionBase != null; parentBindingExpressionBase = parentBindingExpressionBase.ParentBindingExpressionBase)
			{
				if (parentBindingExpressionBase is MultiBindingExpression)
				{
					left = typeof(object);
					break;
				}
				if (stringFormat == null && parentBindingExpressionBase is PriorityBindingExpression)
				{
					stringFormat = parentBindingExpressionBase.ParentBindingBase.StringFormat;
				}
			}
			if (left == typeof(string) && !string.IsNullOrEmpty(stringFormat))
			{
				this.SetValue(BindingExpressionBase.Feature.EffectiveStringFormat, Helper.GetEffectiveStringFormat(stringFormat), null);
			}
		}

		// Token: 0x0600195C RID: 6492 RVA: 0x00079268 File Offset: 0x00077468
		internal void DetermineEffectiveTargetNullValue()
		{
			Type type = this.TargetProperty.PropertyType;
			object obj = this.ParentBindingBase.TargetNullValue;
			for (BindingExpressionBase parentBindingExpressionBase = this.ParentBindingExpressionBase; parentBindingExpressionBase != null; parentBindingExpressionBase = parentBindingExpressionBase.ParentBindingExpressionBase)
			{
				if (parentBindingExpressionBase is MultiBindingExpression)
				{
					type = typeof(object);
					break;
				}
				if (obj == DependencyProperty.UnsetValue && parentBindingExpressionBase is PriorityBindingExpression)
				{
					obj = parentBindingExpressionBase.ParentBindingBase.TargetNullValue;
				}
			}
			if (obj != DependencyProperty.UnsetValue)
			{
				obj = BindingExpressionBase.ConvertTargetNullValue(obj, this.TargetProperty, this);
				if (obj == BindingExpressionBase.DefaultValueObject)
				{
					obj = DependencyProperty.UnsetValue;
				}
			}
			this.SetValue(BindingExpressionBase.Feature.EffectiveTargetNullValue, obj, DependencyProperty.UnsetValue);
		}

		// Token: 0x0600195D RID: 6493 RVA: 0x00079304 File Offset: 0x00077504
		private void DetermineEffectiveUpdateBehavior()
		{
			if (!this.IsReflective)
			{
				return;
			}
			for (BindingExpressionBase parentBindingExpressionBase = this.ParentBindingExpressionBase; parentBindingExpressionBase != null; parentBindingExpressionBase = parentBindingExpressionBase.ParentBindingExpressionBase)
			{
				if (parentBindingExpressionBase is MultiBindingExpression)
				{
					return;
				}
			}
			int delay = this.ParentBindingBase.Delay;
			if (delay > 0 && this.IsUpdateOnPropertyChanged)
			{
				DispatcherTimer dispatcherTimer = new DispatcherTimer();
				this.SetValue(BindingExpressionBase.Feature.Timer, dispatcherTimer);
				dispatcherTimer.Interval = TimeSpan.FromMilliseconds((double)delay);
				dispatcherTimer.Tick += this.OnTimerTick;
			}
		}

		// Token: 0x0600195E RID: 6494 RVA: 0x0007937C File Offset: 0x0007757C
		internal void DetermineEffectiveValidatesOnNotifyDataErrors()
		{
			bool flag = this.ParentBindingBase.ValidatesOnNotifyDataErrorsInternal;
			BindingExpressionBase parentBindingExpressionBase = this.ParentBindingExpressionBase;
			while (flag && parentBindingExpressionBase != null)
			{
				flag = parentBindingExpressionBase.ValidatesOnNotifyDataErrors;
				parentBindingExpressionBase = parentBindingExpressionBase.ParentBindingExpressionBase;
			}
			this.ChangeFlag(BindingExpressionBase.PrivateFlags.iValidatesOnNotifyDataErrors, flag);
		}

		// Token: 0x0600195F RID: 6495 RVA: 0x000793BE File Offset: 0x000775BE
		internal static object CreateReference(object item)
		{
			if (item != null && !(item is BindingListCollectionView) && item != BindingExpression.NullDataItem && item != BindingExpressionBase.DisconnectedItem)
			{
				item = new WeakReference(item);
			}
			return item;
		}

		// Token: 0x06001960 RID: 6496 RVA: 0x000793E4 File Offset: 0x000775E4
		internal static object CreateReference(WeakReference item)
		{
			return item;
		}

		// Token: 0x06001961 RID: 6497 RVA: 0x000793F4 File Offset: 0x000775F4
		internal static object ReplaceReference(object oldReference, object item)
		{
			if (item != null && !(item is BindingListCollectionView) && item != BindingExpression.NullDataItem && item != BindingExpressionBase.DisconnectedItem)
			{
				WeakReference weakReference = oldReference as WeakReference;
				if (weakReference != null)
				{
					weakReference.Target = item;
					item = weakReference;
				}
				else
				{
					item = new WeakReference(item);
				}
			}
			return item;
		}

		// Token: 0x06001962 RID: 6498 RVA: 0x0007943C File Offset: 0x0007763C
		internal static object GetReference(object reference)
		{
			if (reference == null)
			{
				return null;
			}
			WeakReference weakReference = reference as WeakReference;
			if (weakReference != null)
			{
				return weakReference.Target;
			}
			return reference;
		}

		// Token: 0x06001963 RID: 6499 RVA: 0x00079460 File Offset: 0x00077660
		internal static void InitializeTracing(BindingExpressionBase expr, DependencyObject d, DependencyProperty dp)
		{
			BindingBase parentBindingBase = expr.ParentBindingBase;
		}

		// Token: 0x06001964 RID: 6500 RVA: 0x00079474 File Offset: 0x00077674
		private void ChangeSources(DependencyObject target, DependencyProperty dp, WeakDependencySource[] newSources)
		{
			DependencySource[] array;
			if (newSources != null)
			{
				array = new DependencySource[newSources.Length];
				int num = 0;
				for (int i = 0; i < newSources.Length; i++)
				{
					DependencyObject dependencyObject = newSources[i].DependencyObject;
					if (dependencyObject != null)
					{
						array[num++] = new DependencySource(dependencyObject, newSources[i].DependencyProperty);
					}
				}
				if (num < newSources.Length)
				{
					DependencySource[] array2;
					if (num > 0)
					{
						array2 = new DependencySource[num];
						Array.Copy(array, 0, array2, 0, num);
					}
					else
					{
						array2 = null;
					}
					array = array2;
				}
			}
			else
			{
				array = null;
			}
			base.ChangeSources(target, dp, array);
		}

		// Token: 0x06001965 RID: 6501 RVA: 0x000794F0 File Offset: 0x000776F0
		internal bool HasValue(BindingExpressionBase.Feature id)
		{
			return this._values.HasValue((int)id);
		}

		// Token: 0x06001966 RID: 6502 RVA: 0x000794FE File Offset: 0x000776FE
		internal object GetValue(BindingExpressionBase.Feature id, object defaultValue)
		{
			return this._values.GetValue((int)id, defaultValue);
		}

		// Token: 0x06001967 RID: 6503 RVA: 0x0007950D File Offset: 0x0007770D
		internal void SetValue(BindingExpressionBase.Feature id, object value)
		{
			this._values.SetValue((int)id, value);
		}

		// Token: 0x06001968 RID: 6504 RVA: 0x0007951C File Offset: 0x0007771C
		internal void SetValue(BindingExpressionBase.Feature id, object value, object defaultValue)
		{
			if (object.Equals(value, defaultValue))
			{
				this._values.ClearValue((int)id);
				return;
			}
			this._values.SetValue((int)id, value);
		}

		// Token: 0x06001969 RID: 6505 RVA: 0x00079541 File Offset: 0x00077741
		internal void ClearValue(BindingExpressionBase.Feature id)
		{
			this._values.ClearValue((int)id);
		}

		// Token: 0x0400130C RID: 4876
		internal static readonly DependencyProperty NoTargetProperty = DependencyProperty.RegisterAttached("NoTarget", typeof(object), typeof(BindingExpressionBase), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));

		// Token: 0x0400130D RID: 4877
		private BindingBase _binding;

		// Token: 0x0400130E RID: 4878
		private WeakReference _targetElement;

		// Token: 0x0400130F RID: 4879
		private DependencyProperty _targetProperty;

		// Token: 0x04001310 RID: 4880
		private DataBindEngine _engine;

		// Token: 0x04001311 RID: 4881
		private BindingExpressionBase.PrivateFlags _flags;

		// Token: 0x04001312 RID: 4882
		private object _value = BindingExpressionBase.DefaultValueObject;

		// Token: 0x04001313 RID: 4883
		private BindingStatusInternal _status;

		// Token: 0x04001314 RID: 4884
		private WeakDependencySource[] _sources;

		// Token: 0x04001315 RID: 4885
		private object _culture = BindingExpressionBase.DefaultValueObject;

		// Token: 0x04001316 RID: 4886
		internal static readonly object DefaultValueObject = new NamedObject("DefaultValue");

		// Token: 0x04001317 RID: 4887
		internal static readonly object DisconnectedItem = new NamedObject("DisconnectedItem");

		// Token: 0x04001318 RID: 4888
		private static readonly WeakReference<BindingGroup> NullBindingGroupReference = new WeakReference<BindingGroup>(null);

		// Token: 0x04001319 RID: 4889
		private UncommonValueTable _values;

		// Token: 0x02000866 RID: 2150
		[Flags]
		internal enum BindingFlags : uint
		{
			// Token: 0x040040CC RID: 16588
			OneWay = 1U,
			// Token: 0x040040CD RID: 16589
			TwoWay = 3U,
			// Token: 0x040040CE RID: 16590
			OneWayToSource = 2U,
			// Token: 0x040040CF RID: 16591
			OneTime = 0U,
			// Token: 0x040040D0 RID: 16592
			PropDefault = 4U,
			// Token: 0x040040D1 RID: 16593
			NotifyOnTargetUpdated = 8U,
			// Token: 0x040040D2 RID: 16594
			NotifyOnSourceUpdated = 8388608U,
			// Token: 0x040040D3 RID: 16595
			NotifyOnValidationError = 2097152U,
			// Token: 0x040040D4 RID: 16596
			UpdateOnPropertyChanged = 0U,
			// Token: 0x040040D5 RID: 16597
			UpdateOnLostFocus = 1024U,
			// Token: 0x040040D6 RID: 16598
			UpdateExplicitly = 2048U,
			// Token: 0x040040D7 RID: 16599
			UpdateDefault = 3072U,
			// Token: 0x040040D8 RID: 16600
			PathGeneratedInternally = 8192U,
			// Token: 0x040040D9 RID: 16601
			ValidatesOnExceptions = 16777216U,
			// Token: 0x040040DA RID: 16602
			ValidatesOnDataErrors = 33554432U,
			// Token: 0x040040DB RID: 16603
			ValidatesOnNotifyDataErrors = 536870912U,
			// Token: 0x040040DC RID: 16604
			Default = 3076U,
			// Token: 0x040040DD RID: 16605
			IllegalInput = 67108864U,
			// Token: 0x040040DE RID: 16606
			PropagationMask = 7U,
			// Token: 0x040040DF RID: 16607
			UpdateMask = 3072U
		}

		// Token: 0x02000867 RID: 2151
		[Flags]
		private enum PrivateFlags : uint
		{
			// Token: 0x040040E1 RID: 16609
			iSourceToTarget = 1U,
			// Token: 0x040040E2 RID: 16610
			iTargetToSource = 2U,
			// Token: 0x040040E3 RID: 16611
			iPropDefault = 4U,
			// Token: 0x040040E4 RID: 16612
			iNotifyOnTargetUpdated = 8U,
			// Token: 0x040040E5 RID: 16613
			iDefaultValueConverter = 16U,
			// Token: 0x040040E6 RID: 16614
			iInTransfer = 32U,
			// Token: 0x040040E7 RID: 16615
			iInUpdate = 64U,
			// Token: 0x040040E8 RID: 16616
			iTransferPending = 128U,
			// Token: 0x040040E9 RID: 16617
			iNeedDataTransfer = 256U,
			// Token: 0x040040EA RID: 16618
			iTransferDeferred = 512U,
			// Token: 0x040040EB RID: 16619
			iUpdateOnLostFocus = 1024U,
			// Token: 0x040040EC RID: 16620
			iUpdateExplicitly = 2048U,
			// Token: 0x040040ED RID: 16621
			iUpdateDefault = 3072U,
			// Token: 0x040040EE RID: 16622
			iNeedsUpdate = 4096U,
			// Token: 0x040040EF RID: 16623
			iPathGeneratedInternally = 8192U,
			// Token: 0x040040F0 RID: 16624
			iUsingMentor = 16384U,
			// Token: 0x040040F1 RID: 16625
			iResolveNamesInTemplate = 32768U,
			// Token: 0x040040F2 RID: 16626
			iDetaching = 65536U,
			// Token: 0x040040F3 RID: 16627
			iNeedsCollectionView = 131072U,
			// Token: 0x040040F4 RID: 16628
			iInPriorityBindingExpression = 262144U,
			// Token: 0x040040F5 RID: 16629
			iInMultiBindingExpression = 524288U,
			// Token: 0x040040F6 RID: 16630
			iUsingFallbackValue = 1048576U,
			// Token: 0x040040F7 RID: 16631
			iNotifyOnValidationError = 2097152U,
			// Token: 0x040040F8 RID: 16632
			iAttaching = 4194304U,
			// Token: 0x040040F9 RID: 16633
			iNotifyOnSourceUpdated = 8388608U,
			// Token: 0x040040FA RID: 16634
			iValidatesOnExceptions = 16777216U,
			// Token: 0x040040FB RID: 16635
			iValidatesOnDataErrors = 33554432U,
			// Token: 0x040040FC RID: 16636
			iIllegalInput = 67108864U,
			// Token: 0x040040FD RID: 16637
			iNeedsValidation = 134217728U,
			// Token: 0x040040FE RID: 16638
			iTargetWantsXTNotification = 268435456U,
			// Token: 0x040040FF RID: 16639
			iValidatesOnNotifyDataErrors = 536870912U,
			// Token: 0x04004100 RID: 16640
			iDataErrorsChangedPending = 1073741824U,
			// Token: 0x04004101 RID: 16641
			iDeferUpdateForComposition = 2147483648U,
			// Token: 0x04004102 RID: 16642
			iPropagationMask = 7U,
			// Token: 0x04004103 RID: 16643
			iUpdateMask = 3072U,
			// Token: 0x04004104 RID: 16644
			iAdoptionMask = 134221827U
		}

		// Token: 0x02000868 RID: 2152
		internal class ProposedValue
		{
			// Token: 0x060082D5 RID: 33493 RVA: 0x00243EF7 File Offset: 0x002420F7
			internal ProposedValue(BindingExpression bindingExpression, object rawValue, object convertedValue)
			{
				this._bindingExpression = bindingExpression;
				this._rawValue = rawValue;
				this._convertedValue = convertedValue;
			}

			// Token: 0x17001D9C RID: 7580
			// (get) Token: 0x060082D6 RID: 33494 RVA: 0x00243F14 File Offset: 0x00242114
			internal BindingExpression BindingExpression
			{
				get
				{
					return this._bindingExpression;
				}
			}

			// Token: 0x17001D9D RID: 7581
			// (get) Token: 0x060082D7 RID: 33495 RVA: 0x00243F1C File Offset: 0x0024211C
			internal object RawValue
			{
				get
				{
					return this._rawValue;
				}
			}

			// Token: 0x17001D9E RID: 7582
			// (get) Token: 0x060082D8 RID: 33496 RVA: 0x00243F24 File Offset: 0x00242124
			internal object ConvertedValue
			{
				get
				{
					return this._convertedValue;
				}
			}

			// Token: 0x04004105 RID: 16645
			private BindingExpression _bindingExpression;

			// Token: 0x04004106 RID: 16646
			private object _rawValue;

			// Token: 0x04004107 RID: 16647
			private object _convertedValue;
		}

		// Token: 0x02000869 RID: 2153
		internal enum Feature
		{
			// Token: 0x04004109 RID: 16649
			ParentBindingExpressionBase,
			// Token: 0x0400410A RID: 16650
			ValidationError,
			// Token: 0x0400410B RID: 16651
			NotifyDataErrors,
			// Token: 0x0400410C RID: 16652
			EffectiveStringFormat,
			// Token: 0x0400410D RID: 16653
			EffectiveTargetNullValue,
			// Token: 0x0400410E RID: 16654
			BindingGroup,
			// Token: 0x0400410F RID: 16655
			Timer,
			// Token: 0x04004110 RID: 16656
			UpdateTargetOperation,
			// Token: 0x04004111 RID: 16657
			Converter,
			// Token: 0x04004112 RID: 16658
			SourceType,
			// Token: 0x04004113 RID: 16659
			DataProvider,
			// Token: 0x04004114 RID: 16660
			CollectionViewSource,
			// Token: 0x04004115 RID: 16661
			DynamicConverter,
			// Token: 0x04004116 RID: 16662
			DataErrorValue,
			// Token: 0x04004117 RID: 16663
			LastFeatureId
		}
	}
}
