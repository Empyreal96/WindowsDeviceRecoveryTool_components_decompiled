using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Threading;

namespace MS.Internal.Data
{
	// Token: 0x02000711 RID: 1809
	internal class ClrBindingWorker : BindingWorker
	{
		// Token: 0x0600746C RID: 29804 RVA: 0x002152AC File Offset: 0x002134AC
		internal ClrBindingWorker(BindingExpression b, DataBindEngine engine) : base(b)
		{
			PropertyPath propertyPath = base.ParentBinding.Path;
			if (base.ParentBinding.XPath != null)
			{
				propertyPath = this.PrepareXmlBinding(propertyPath);
			}
			if (propertyPath == null)
			{
				propertyPath = new PropertyPath(string.Empty, new object[0]);
			}
			if (base.ParentBinding.Path == null)
			{
				base.ParentBinding.UsePath(propertyPath);
			}
			this._pathWorker = new PropertyPathWorker(propertyPath, this, base.IsDynamic, engine);
			this._pathWorker.SetTreeContext(base.ParentBindingExpression.TargetElementReference);
		}

		// Token: 0x0600746D RID: 29805 RVA: 0x00215338 File Offset: 0x00213538
		[MethodImpl(MethodImplOptions.NoInlining)]
		private PropertyPath PrepareXmlBinding(PropertyPath path)
		{
			if (path == null)
			{
				DependencyProperty targetProperty = base.TargetProperty;
				Type propertyType = targetProperty.PropertyType;
				string path2;
				if (propertyType == typeof(object))
				{
					if (targetProperty == BindingExpressionBase.NoTargetProperty || targetProperty == Selector.SelectedValueProperty || targetProperty.OwnerType == typeof(LiveShapingList))
					{
						path2 = "/InnerText";
					}
					else if (targetProperty == FrameworkElement.DataContextProperty || targetProperty == CollectionViewSource.SourceProperty)
					{
						path2 = string.Empty;
					}
					else
					{
						path2 = "/";
					}
				}
				else if (propertyType.IsAssignableFrom(typeof(XmlDataCollection)))
				{
					path2 = string.Empty;
				}
				else
				{
					path2 = "/InnerText";
				}
				path = new PropertyPath(path2, new object[0]);
			}
			if (path.SVI.Length != 0)
			{
				base.SetValue(BindingWorker.Feature.XmlWorker, new XmlBindingWorker(this, path.SVI[0].drillIn == DrillIn.Never));
			}
			return path;
		}

		// Token: 0x17001BB5 RID: 7093
		// (get) Token: 0x0600746E RID: 29806 RVA: 0x00215413 File Offset: 0x00213613
		internal override Type SourcePropertyType
		{
			get
			{
				return this.PW.GetType(this.PW.Length - 1);
			}
		}

		// Token: 0x17001BB6 RID: 7094
		// (get) Token: 0x0600746F RID: 29807 RVA: 0x0021542D File Offset: 0x0021362D
		internal override bool IsDBNullValidForUpdate
		{
			get
			{
				return this.PW.IsDBNullValidForUpdate;
			}
		}

		// Token: 0x17001BB7 RID: 7095
		// (get) Token: 0x06007470 RID: 29808 RVA: 0x0021543A File Offset: 0x0021363A
		internal override object SourceItem
		{
			get
			{
				return this.PW.SourceItem;
			}
		}

		// Token: 0x17001BB8 RID: 7096
		// (get) Token: 0x06007471 RID: 29809 RVA: 0x00215447 File Offset: 0x00213647
		internal override string SourcePropertyName
		{
			get
			{
				return this.PW.SourcePropertyName;
			}
		}

		// Token: 0x17001BB9 RID: 7097
		// (get) Token: 0x06007472 RID: 29810 RVA: 0x00215454 File Offset: 0x00213654
		internal override bool CanUpdate
		{
			get
			{
				PropertyPathWorker pw = this.PW;
				int num = this.PW.Length - 1;
				if (num < 0)
				{
					return false;
				}
				object item = pw.GetItem(num);
				if (item == null || item == BindingExpression.NullDataItem)
				{
					return false;
				}
				object accessor = pw.GetAccessor(num);
				return accessor != null && (accessor != DependencyProperty.UnsetValue || this.XmlWorker != null);
			}
		}

		// Token: 0x06007473 RID: 29811 RVA: 0x002154B0 File Offset: 0x002136B0
		internal override void AttachDataItem()
		{
			object obj;
			if (this.XmlWorker == null)
			{
				obj = base.DataItem;
			}
			else
			{
				this.XmlWorker.AttachDataItem();
				obj = this.XmlWorker.RawValue();
			}
			this.PW.AttachToRootItem(obj);
			if (this.PW.Length == 0)
			{
				base.ParentBindingExpression.SetupDefaultValueConverter(obj.GetType());
			}
		}

		// Token: 0x06007474 RID: 29812 RVA: 0x00215510 File Offset: 0x00213710
		internal override void DetachDataItem()
		{
			this.PW.DetachFromRootItem();
			if (this.XmlWorker != null)
			{
				this.XmlWorker.DetachDataItem();
			}
			AsyncGetValueRequest asyncGetValueRequest = (AsyncGetValueRequest)base.GetValue(BindingWorker.Feature.PendingGetValueRequest, null);
			if (asyncGetValueRequest != null)
			{
				asyncGetValueRequest.Cancel();
				base.ClearValue(BindingWorker.Feature.PendingGetValueRequest);
			}
			AsyncSetValueRequest asyncSetValueRequest = (AsyncSetValueRequest)base.GetValue(BindingWorker.Feature.PendingSetValueRequest, null);
			if (asyncSetValueRequest != null)
			{
				asyncSetValueRequest.Cancel();
				base.ClearValue(BindingWorker.Feature.PendingSetValueRequest);
			}
		}

		// Token: 0x06007475 RID: 29813 RVA: 0x00215578 File Offset: 0x00213778
		internal override object RawValue()
		{
			object result = this.PW.RawValue();
			this.SetStatus(this.PW.Status);
			return result;
		}

		// Token: 0x06007476 RID: 29814 RVA: 0x002155A3 File Offset: 0x002137A3
		internal override void RefreshValue()
		{
			this.PW.RefreshValue();
		}

		// Token: 0x06007477 RID: 29815 RVA: 0x002155B0 File Offset: 0x002137B0
		internal override void UpdateValue(object value)
		{
			int level = this.PW.Length - 1;
			object item = this.PW.GetItem(level);
			if (item == null || item == BindingExpression.NullDataItem)
			{
				return;
			}
			if (base.ParentBinding.IsAsync && !(this.PW.GetAccessor(level) is DependencyProperty))
			{
				this.RequestAsyncSetValue(item, value);
				return;
			}
			this.PW.SetValue(item, value);
		}

		// Token: 0x06007478 RID: 29816 RVA: 0x0021561A File Offset: 0x0021381A
		internal override void OnCurrentChanged(ICollectionView collectionView, EventArgs args)
		{
			if (this.XmlWorker != null)
			{
				this.XmlWorker.OnCurrentChanged(collectionView, args);
			}
			this.PW.OnCurrentChanged(collectionView);
		}

		// Token: 0x06007479 RID: 29817 RVA: 0x0021563D File Offset: 0x0021383D
		internal override bool UsesDependencyProperty(DependencyObject d, DependencyProperty dp)
		{
			return this.PW.UsesDependencyProperty(d, dp);
		}

		// Token: 0x0600747A RID: 29818 RVA: 0x0021564C File Offset: 0x0021384C
		internal override void OnSourceInvalidation(DependencyObject d, DependencyProperty dp, bool isASubPropertyChange)
		{
			this.PW.OnDependencyPropertyChanged(d, dp, isASubPropertyChange);
		}

		// Token: 0x0600747B RID: 29819 RVA: 0x0021565C File Offset: 0x0021385C
		internal override bool IsPathCurrent()
		{
			object rootItem = (this.XmlWorker == null) ? base.DataItem : this.XmlWorker.RawValue();
			return this.PW.IsPathCurrent(rootItem);
		}

		// Token: 0x17001BBA RID: 7098
		// (get) Token: 0x0600747C RID: 29820 RVA: 0x00215691 File Offset: 0x00213891
		internal bool TransfersDefaultValue
		{
			get
			{
				return base.ParentBinding.TransfersDefaultValue;
			}
		}

		// Token: 0x17001BBB RID: 7099
		// (get) Token: 0x0600747D RID: 29821 RVA: 0x0021569E File Offset: 0x0021389E
		internal bool ValidatesOnNotifyDataErrors
		{
			get
			{
				return base.ParentBindingExpression.ValidatesOnNotifyDataErrors;
			}
		}

		// Token: 0x0600747E RID: 29822 RVA: 0x002156AB File Offset: 0x002138AB
		internal void CancelPendingTasks()
		{
			base.ParentBindingExpression.CancelPendingTasks();
		}

		// Token: 0x0600747F RID: 29823 RVA: 0x002156B8 File Offset: 0x002138B8
		internal bool AsyncGet(object item, int level)
		{
			if (base.ParentBinding.IsAsync)
			{
				this.RequestAsyncGetValue(item, level);
				return true;
			}
			return false;
		}

		// Token: 0x06007480 RID: 29824 RVA: 0x002156D4 File Offset: 0x002138D4
		internal void ReplaceCurrentItem(ICollectionView oldCollectionView, ICollectionView newCollectionView)
		{
			if (oldCollectionView != null)
			{
				CurrentChangedEventManager.RemoveHandler(oldCollectionView, new EventHandler<EventArgs>(base.ParentBindingExpression.OnCurrentChanged));
				if (base.IsReflective)
				{
					CurrentChangingEventManager.RemoveHandler(oldCollectionView, new EventHandler<CurrentChangingEventArgs>(base.ParentBindingExpression.OnCurrentChanging));
				}
			}
			if (newCollectionView != null)
			{
				CurrentChangedEventManager.AddHandler(newCollectionView, new EventHandler<EventArgs>(base.ParentBindingExpression.OnCurrentChanged));
				if (base.IsReflective)
				{
					CurrentChangingEventManager.AddHandler(newCollectionView, new EventHandler<CurrentChangingEventArgs>(base.ParentBindingExpression.OnCurrentChanging));
				}
			}
		}

		// Token: 0x06007481 RID: 29825 RVA: 0x00215754 File Offset: 0x00213954
		internal void NewValueAvailable(bool dependencySourcesChanged, bool initialValue, bool isASubPropertyChange)
		{
			this.SetStatus(this.PW.Status);
			BindingExpression parentBindingExpression = base.ParentBindingExpression;
			BindingGroup bindingGroup = parentBindingExpression.BindingGroup;
			if (bindingGroup != null)
			{
				bindingGroup.UpdateTable(parentBindingExpression);
			}
			if (dependencySourcesChanged)
			{
				this.ReplaceDependencySources();
			}
			if (base.Status != BindingStatusInternal.AsyncRequestPending)
			{
				if (!initialValue)
				{
					parentBindingExpression.ScheduleTransfer(isASubPropertyChange);
					return;
				}
				base.SetTransferIsPending(false);
			}
		}

		// Token: 0x06007482 RID: 29826 RVA: 0x002157AE File Offset: 0x002139AE
		internal void SetupDefaultValueConverter(Type type)
		{
			base.ParentBindingExpression.SetupDefaultValueConverter(type);
		}

		// Token: 0x06007483 RID: 29827 RVA: 0x002157BC File Offset: 0x002139BC
		internal bool IsValidValue(object value)
		{
			return base.TargetProperty.IsValidValue(value);
		}

		// Token: 0x06007484 RID: 29828 RVA: 0x002157CC File Offset: 0x002139CC
		internal void OnSourcePropertyChanged(object o, string propName)
		{
			int level;
			if (!base.IgnoreSourcePropertyChange && (level = this.PW.LevelForPropertyChange(o, propName)) >= 0)
			{
				if (base.Dispatcher.Thread == Thread.CurrentThread)
				{
					this.PW.OnPropertyChangedAtLevel(level);
					return;
				}
				base.SetTransferIsPending(true);
				if (base.ParentBindingExpression.TargetWantsCrossThreadNotifications)
				{
					LiveShapingItem liveShapingItem = base.TargetElement as LiveShapingItem;
					if (liveShapingItem != null)
					{
						liveShapingItem.OnCrossThreadPropertyChange(base.TargetProperty);
					}
				}
				base.Engine.Marshal(new DispatcherOperationCallback(this.ScheduleTransferOperation), null, 1);
			}
		}

		// Token: 0x06007485 RID: 29829 RVA: 0x0021585C File Offset: 0x00213A5C
		internal void OnDataErrorsChanged(INotifyDataErrorInfo indei, string propName)
		{
			if (base.Dispatcher.Thread == Thread.CurrentThread)
			{
				base.ParentBindingExpression.UpdateNotifyDataErrors(indei, propName, DependencyProperty.UnsetValue);
				return;
			}
			if (!base.ParentBindingExpression.IsDataErrorsChangedPending)
			{
				base.ParentBindingExpression.IsDataErrorsChangedPending = true;
				base.Engine.Marshal(delegate(object arg)
				{
					object[] array = (object[])arg;
					base.ParentBindingExpression.UpdateNotifyDataErrors((INotifyDataErrorInfo)array[0], (string)array[1], DependencyProperty.UnsetValue);
					return null;
				}, new object[]
				{
					indei,
					propName
				}, 1);
			}
		}

		// Token: 0x06007486 RID: 29830 RVA: 0x002158D0 File Offset: 0x00213AD0
		internal void OnXmlValueChanged()
		{
			object item = this.PW.GetItem(0);
			this.OnSourcePropertyChanged(item, null);
		}

		// Token: 0x06007487 RID: 29831 RVA: 0x002158F2 File Offset: 0x00213AF2
		internal void UseNewXmlItem(object item)
		{
			this.PW.DetachFromRootItem();
			this.PW.AttachToRootItem(item);
			if (base.Status != BindingStatusInternal.AsyncRequestPending)
			{
				base.ParentBindingExpression.ScheduleTransfer(false);
			}
		}

		// Token: 0x06007488 RID: 29832 RVA: 0x00215920 File Offset: 0x00213B20
		internal object GetResultNode()
		{
			return this.PW.GetItem(0);
		}

		// Token: 0x06007489 RID: 29833 RVA: 0x0021592E File Offset: 0x00213B2E
		internal DependencyObject CheckTarget()
		{
			return base.TargetElement;
		}

		// Token: 0x0600748A RID: 29834 RVA: 0x00215938 File Offset: 0x00213B38
		internal void ReportGetValueError(int k, object item, Exception ex)
		{
			if (TraceData.IsEnabled)
			{
				SourceValueInfo sourceValueInfo = this.PW.GetSourceValueInfo(k);
				Type type = this.PW.GetType(k);
				string text = (k > 0) ? this.PW.GetSourceValueInfo(k - 1).name : string.Empty;
				TraceData.Trace(base.ParentBindingExpression.TraceLevel, TraceData.CannotGetClrRawValue(new object[]
				{
					sourceValueInfo.propertyName,
					type.Name,
					text,
					AvTrace.TypeName(item)
				}), base.ParentBindingExpression, ex);
			}
		}

		// Token: 0x0600748B RID: 29835 RVA: 0x002159C8 File Offset: 0x00213BC8
		internal void ReportSetValueError(int k, object item, object value, Exception ex)
		{
			if (TraceData.IsEnabled)
			{
				SourceValueInfo sourceValueInfo = this.PW.GetSourceValueInfo(k);
				Type type = this.PW.GetType(k);
				TraceData.Trace(TraceEventType.Error, TraceData.CannotSetClrRawValue(new object[]
				{
					sourceValueInfo.propertyName,
					type.Name,
					AvTrace.TypeName(item),
					AvTrace.ToStringHelper(value),
					AvTrace.TypeName(value)
				}), base.ParentBindingExpression, ex);
			}
		}

		// Token: 0x0600748C RID: 29836 RVA: 0x00215A3C File Offset: 0x00213C3C
		internal void ReportRawValueErrors(int k, object item, object info)
		{
			if (TraceData.IsEnabled)
			{
				if (item == null)
				{
					TraceData.Trace(TraceEventType.Information, TraceData.MissingDataItem, base.ParentBindingExpression);
				}
				if (info == null)
				{
					TraceData.Trace(TraceEventType.Information, TraceData.MissingInfo, base.ParentBindingExpression);
				}
				if (item == BindingExpression.NullDataItem)
				{
					TraceData.Trace(TraceEventType.Information, TraceData.NullDataItem, base.ParentBindingExpression);
				}
			}
		}

		// Token: 0x0600748D RID: 29837 RVA: 0x00215A94 File Offset: 0x00213C94
		internal void ReportBadXPath(TraceEventType traceType)
		{
			XmlBindingWorker xmlWorker = this.XmlWorker;
			if (xmlWorker != null)
			{
				xmlWorker.ReportBadXPath(traceType);
			}
		}

		// Token: 0x17001BBC RID: 7100
		// (get) Token: 0x0600748E RID: 29838 RVA: 0x00215AB2 File Offset: 0x00213CB2
		private PropertyPathWorker PW
		{
			get
			{
				return this._pathWorker;
			}
		}

		// Token: 0x17001BBD RID: 7101
		// (get) Token: 0x0600748F RID: 29839 RVA: 0x00215ABA File Offset: 0x00213CBA
		private XmlBindingWorker XmlWorker
		{
			get
			{
				return (XmlBindingWorker)base.GetValue(BindingWorker.Feature.XmlWorker, null);
			}
		}

		// Token: 0x06007490 RID: 29840 RVA: 0x00215AC9 File Offset: 0x00213CC9
		private void SetStatus(PropertyPathStatus status)
		{
			switch (status)
			{
			case PropertyPathStatus.Inactive:
				base.Status = BindingStatusInternal.Inactive;
				return;
			case PropertyPathStatus.Active:
				base.Status = BindingStatusInternal.Active;
				return;
			case PropertyPathStatus.PathError:
				base.Status = BindingStatusInternal.PathError;
				return;
			case PropertyPathStatus.AsyncRequestPending:
				base.Status = BindingStatusInternal.AsyncRequestPending;
				return;
			default:
				return;
			}
		}

		// Token: 0x06007491 RID: 29841 RVA: 0x00215B04 File Offset: 0x00213D04
		private void ReplaceDependencySources()
		{
			if (!base.ParentBindingExpression.IsDetaching)
			{
				int num = this.PW.Length;
				if (this.PW.NeedsDirectNotification)
				{
					num++;
				}
				WeakDependencySource[] array = new WeakDependencySource[num];
				int n = 0;
				if (base.IsDynamic)
				{
					for (int i = 0; i < this.PW.Length; i++)
					{
						DependencyProperty dependencyProperty = this.PW.GetAccessor(i) as DependencyProperty;
						if (dependencyProperty != null)
						{
							DependencyObject dependencyObject = this.PW.GetItem(i) as DependencyObject;
							if (dependencyObject != null)
							{
								array[n++] = new WeakDependencySource(dependencyObject, dependencyProperty);
							}
						}
					}
					if (this.PW.NeedsDirectNotification)
					{
						DependencyObject dependencyObject2 = this.PW.RawValue() as Freezable;
						if (dependencyObject2 != null)
						{
							array[n++] = new WeakDependencySource(dependencyObject2, DependencyObject.DirectDependencyProperty);
						}
					}
				}
				base.ParentBindingExpression.ChangeWorkerSources(array, n);
			}
		}

		// Token: 0x06007492 RID: 29842 RVA: 0x00215BEC File Offset: 0x00213DEC
		private void RequestAsyncGetValue(object item, int level)
		{
			string nameFromInfo = this.GetNameFromInfo(this.PW.GetAccessor(level));
			Invariant.Assert(nameFromInfo != null, "Async GetValue expects a name");
			AsyncGetValueRequest asyncGetValueRequest = (AsyncGetValueRequest)base.GetValue(BindingWorker.Feature.PendingGetValueRequest, null);
			if (asyncGetValueRequest != null)
			{
				asyncGetValueRequest.Cancel();
			}
			asyncGetValueRequest = new AsyncGetValueRequest(item, nameFromInfo, base.ParentBinding.AsyncState, ClrBindingWorker.DoGetValueCallback, ClrBindingWorker.CompleteGetValueCallback, new object[]
			{
				this,
				level
			});
			base.SetValue(BindingWorker.Feature.PendingGetValueRequest, asyncGetValueRequest);
			base.Engine.AddAsyncRequest(base.TargetElement, asyncGetValueRequest);
		}

		// Token: 0x06007493 RID: 29843 RVA: 0x00215C7C File Offset: 0x00213E7C
		private static object OnGetValueCallback(AsyncDataRequest adr)
		{
			AsyncGetValueRequest asyncGetValueRequest = (AsyncGetValueRequest)adr;
			ClrBindingWorker clrBindingWorker = (ClrBindingWorker)asyncGetValueRequest.Args[0];
			object value = clrBindingWorker.PW.GetValue(asyncGetValueRequest.SourceItem, (int)asyncGetValueRequest.Args[1]);
			if (value == PropertyPathWorker.IListIndexOutOfRange)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			return value;
		}

		// Token: 0x06007494 RID: 29844 RVA: 0x00215CD4 File Offset: 0x00213ED4
		private static object OnCompleteGetValueCallback(AsyncDataRequest adr)
		{
			AsyncGetValueRequest asyncGetValueRequest = (AsyncGetValueRequest)adr;
			ClrBindingWorker clrBindingWorker = (ClrBindingWorker)asyncGetValueRequest.Args[0];
			DataBindEngine engine = clrBindingWorker.Engine;
			if (engine != null)
			{
				engine.Marshal(ClrBindingWorker.CompleteGetValueLocalCallback, asyncGetValueRequest, 1);
			}
			return null;
		}

		// Token: 0x06007495 RID: 29845 RVA: 0x00215D10 File Offset: 0x00213F10
		private static object OnCompleteGetValueOperation(object arg)
		{
			AsyncGetValueRequest asyncGetValueRequest = (AsyncGetValueRequest)arg;
			ClrBindingWorker clrBindingWorker = (ClrBindingWorker)asyncGetValueRequest.Args[0];
			clrBindingWorker.CompleteGetValue(asyncGetValueRequest);
			return null;
		}

		// Token: 0x06007496 RID: 29846 RVA: 0x00215D3C File Offset: 0x00213F3C
		private void CompleteGetValue(AsyncGetValueRequest request)
		{
			AsyncGetValueRequest asyncGetValueRequest = (AsyncGetValueRequest)base.GetValue(BindingWorker.Feature.PendingGetValueRequest, null);
			if (asyncGetValueRequest == request)
			{
				base.ClearValue(BindingWorker.Feature.PendingGetValueRequest);
				int num = (int)request.Args[1];
				if (this.CheckTarget() == null)
				{
					return;
				}
				AsyncRequestStatus status = request.Status;
				if (status != AsyncRequestStatus.Completed)
				{
					if (status != AsyncRequestStatus.Failed)
					{
						return;
					}
					this.ReportGetValueError(num, request.SourceItem, request.Exception);
					this.PW.OnNewValue(num, DependencyProperty.UnsetValue);
				}
				else
				{
					this.PW.OnNewValue(num, request.Result);
					this.SetStatus(this.PW.Status);
					if (num == this.PW.Length - 1)
					{
						base.ParentBindingExpression.TransferValue(request.Result, false);
						return;
					}
				}
			}
		}

		// Token: 0x06007497 RID: 29847 RVA: 0x00215DF8 File Offset: 0x00213FF8
		private void RequestAsyncSetValue(object item, object value)
		{
			string nameFromInfo = this.GetNameFromInfo(this.PW.GetAccessor(this.PW.Length - 1));
			Invariant.Assert(nameFromInfo != null, "Async SetValue expects a name");
			AsyncSetValueRequest asyncSetValueRequest = (AsyncSetValueRequest)base.GetValue(BindingWorker.Feature.PendingSetValueRequest, null);
			if (asyncSetValueRequest != null)
			{
				asyncSetValueRequest.Cancel();
			}
			asyncSetValueRequest = new AsyncSetValueRequest(item, nameFromInfo, value, base.ParentBinding.AsyncState, ClrBindingWorker.DoSetValueCallback, ClrBindingWorker.CompleteSetValueCallback, new object[]
			{
				this
			});
			base.SetValue(BindingWorker.Feature.PendingSetValueRequest, asyncSetValueRequest);
			base.Engine.AddAsyncRequest(base.TargetElement, asyncSetValueRequest);
		}

		// Token: 0x06007498 RID: 29848 RVA: 0x00215E8C File Offset: 0x0021408C
		private static object OnSetValueCallback(AsyncDataRequest adr)
		{
			AsyncSetValueRequest asyncSetValueRequest = (AsyncSetValueRequest)adr;
			ClrBindingWorker clrBindingWorker = (ClrBindingWorker)asyncSetValueRequest.Args[0];
			clrBindingWorker.PW.SetValue(asyncSetValueRequest.TargetItem, asyncSetValueRequest.Value);
			return null;
		}

		// Token: 0x06007499 RID: 29849 RVA: 0x00215EC8 File Offset: 0x002140C8
		private static object OnCompleteSetValueCallback(AsyncDataRequest adr)
		{
			AsyncSetValueRequest asyncSetValueRequest = (AsyncSetValueRequest)adr;
			ClrBindingWorker clrBindingWorker = (ClrBindingWorker)asyncSetValueRequest.Args[0];
			DataBindEngine engine = clrBindingWorker.Engine;
			if (engine != null)
			{
				engine.Marshal(ClrBindingWorker.CompleteSetValueLocalCallback, asyncSetValueRequest, 1);
			}
			return null;
		}

		// Token: 0x0600749A RID: 29850 RVA: 0x00215F04 File Offset: 0x00214104
		private static object OnCompleteSetValueOperation(object arg)
		{
			AsyncSetValueRequest asyncSetValueRequest = (AsyncSetValueRequest)arg;
			ClrBindingWorker clrBindingWorker = (ClrBindingWorker)asyncSetValueRequest.Args[0];
			clrBindingWorker.CompleteSetValue(asyncSetValueRequest);
			return null;
		}

		// Token: 0x0600749B RID: 29851 RVA: 0x00215F30 File Offset: 0x00214130
		private void CompleteSetValue(AsyncSetValueRequest request)
		{
			AsyncSetValueRequest asyncSetValueRequest = (AsyncSetValueRequest)base.GetValue(BindingWorker.Feature.PendingSetValueRequest, null);
			if (asyncSetValueRequest == request)
			{
				base.ClearValue(BindingWorker.Feature.PendingSetValueRequest);
				if (this.CheckTarget() == null)
				{
					return;
				}
				AsyncRequestStatus status = request.Status;
				if (status != AsyncRequestStatus.Completed && status == AsyncRequestStatus.Failed)
				{
					object obj = base.ParentBinding.DoFilterException(base.ParentBindingExpression, request.Exception);
					Exception ex = obj as Exception;
					ValidationError validationError;
					if (ex != null)
					{
						if (TraceData.IsEnabled)
						{
							int k = this.PW.Length - 1;
							object value = request.Value;
							this.ReportSetValueError(k, request.TargetItem, request.Value, ex);
							return;
						}
					}
					else if ((validationError = (obj as ValidationError)) != null)
					{
						Validation.MarkInvalid(base.ParentBindingExpression, validationError);
					}
				}
			}
		}

		// Token: 0x0600749C RID: 29852 RVA: 0x00215FE0 File Offset: 0x002141E0
		private string GetNameFromInfo(object info)
		{
			MemberInfo memberInfo;
			if ((memberInfo = (info as MemberInfo)) != null)
			{
				return memberInfo.Name;
			}
			PropertyDescriptor propertyDescriptor;
			if ((propertyDescriptor = (info as PropertyDescriptor)) != null)
			{
				return propertyDescriptor.Name;
			}
			DynamicObjectAccessor dynamicObjectAccessor;
			if ((dynamicObjectAccessor = (info as DynamicObjectAccessor)) != null)
			{
				return dynamicObjectAccessor.PropertyName;
			}
			return null;
		}

		// Token: 0x0600749D RID: 29853 RVA: 0x00216027 File Offset: 0x00214227
		private object ScheduleTransferOperation(object arg)
		{
			this.PW.RefreshValue();
			return null;
		}

		// Token: 0x040037D5 RID: 14293
		private static readonly AsyncRequestCallback DoGetValueCallback = new AsyncRequestCallback(ClrBindingWorker.OnGetValueCallback);

		// Token: 0x040037D6 RID: 14294
		private static readonly AsyncRequestCallback CompleteGetValueCallback = new AsyncRequestCallback(ClrBindingWorker.OnCompleteGetValueCallback);

		// Token: 0x040037D7 RID: 14295
		private static readonly DispatcherOperationCallback CompleteGetValueLocalCallback = new DispatcherOperationCallback(ClrBindingWorker.OnCompleteGetValueOperation);

		// Token: 0x040037D8 RID: 14296
		private static readonly AsyncRequestCallback DoSetValueCallback = new AsyncRequestCallback(ClrBindingWorker.OnSetValueCallback);

		// Token: 0x040037D9 RID: 14297
		private static readonly AsyncRequestCallback CompleteSetValueCallback = new AsyncRequestCallback(ClrBindingWorker.OnCompleteSetValueCallback);

		// Token: 0x040037DA RID: 14298
		private static readonly DispatcherOperationCallback CompleteSetValueLocalCallback = new DispatcherOperationCallback(ClrBindingWorker.OnCompleteSetValueOperation);

		// Token: 0x040037DB RID: 14299
		private PropertyPathWorker _pathWorker;
	}
}
