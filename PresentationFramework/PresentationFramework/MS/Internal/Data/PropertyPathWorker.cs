using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Markup;

namespace MS.Internal.Data
{
	// Token: 0x0200073E RID: 1854
	internal sealed class PropertyPathWorker : IWeakEventListener
	{
		// Token: 0x06007630 RID: 30256 RVA: 0x0021B433 File Offset: 0x00219633
		internal PropertyPathWorker(PropertyPath path) : this(path, DataBindEngine.CurrentDataBindEngine)
		{
		}

		// Token: 0x06007631 RID: 30257 RVA: 0x0021B441 File Offset: 0x00219641
		internal PropertyPathWorker(PropertyPath path, ClrBindingWorker host, bool isDynamic, DataBindEngine engine) : this(path, engine)
		{
			this._host = host;
			this._isDynamic = isDynamic;
		}

		// Token: 0x06007632 RID: 30258 RVA: 0x0021B45C File Offset: 0x0021965C
		private PropertyPathWorker(PropertyPath path, DataBindEngine engine)
		{
			this._parent = path;
			this._arySVS = new PropertyPathWorker.SourceValueState[path.Length];
			this._engine = engine;
			for (int i = this._arySVS.Length - 1; i >= 0; i--)
			{
				this._arySVS[i].item = BindingExpressionBase.CreateReference(BindingExpression.NullDataItem);
			}
		}

		// Token: 0x17001C23 RID: 7203
		// (get) Token: 0x06007633 RID: 30259 RVA: 0x0021B4BE File Offset: 0x002196BE
		internal int Length
		{
			get
			{
				return this._parent.Length;
			}
		}

		// Token: 0x17001C24 RID: 7204
		// (get) Token: 0x06007634 RID: 30260 RVA: 0x0021B4CB File Offset: 0x002196CB
		internal PropertyPathStatus Status
		{
			get
			{
				return this._status;
			}
		}

		// Token: 0x17001C25 RID: 7205
		// (get) Token: 0x06007635 RID: 30261 RVA: 0x0021B4D3 File Offset: 0x002196D3
		// (set) Token: 0x06007636 RID: 30262 RVA: 0x0021B4E5 File Offset: 0x002196E5
		internal DependencyObject TreeContext
		{
			get
			{
				return BindingExpressionBase.GetReference(this._treeContext) as DependencyObject;
			}
			set
			{
				this._treeContext = BindingExpressionBase.CreateReference(value);
			}
		}

		// Token: 0x06007637 RID: 30263 RVA: 0x0021B4F3 File Offset: 0x002196F3
		internal void SetTreeContext(WeakReference wr)
		{
			this._treeContext = BindingExpressionBase.CreateReference(wr);
		}

		// Token: 0x17001C26 RID: 7206
		// (get) Token: 0x06007638 RID: 30264 RVA: 0x0021B501 File Offset: 0x00219701
		internal bool IsDBNullValidForUpdate
		{
			get
			{
				if (this._isDBNullValidForUpdate == null)
				{
					this.DetermineWhetherDBNullIsValid();
				}
				return this._isDBNullValidForUpdate.Value;
			}
		}

		// Token: 0x17001C27 RID: 7207
		// (get) Token: 0x06007639 RID: 30265 RVA: 0x0021B524 File Offset: 0x00219724
		internal object SourceItem
		{
			get
			{
				int num = this.Length - 1;
				object obj = (num >= 0) ? this.GetItem(num) : null;
				if (obj == BindingExpression.NullDataItem)
				{
					obj = null;
				}
				return obj;
			}
		}

		// Token: 0x17001C28 RID: 7208
		// (get) Token: 0x0600763A RID: 30266 RVA: 0x0021B554 File Offset: 0x00219754
		internal string SourcePropertyName
		{
			get
			{
				int num = this.Length - 1;
				if (num < 0)
				{
					return null;
				}
				SourceValueType type = this.SVI[num].type;
				if (type != SourceValueType.Property)
				{
					if (type != SourceValueType.Indexer)
					{
						return null;
					}
					string path = this._parent.Path;
					int startIndex = path.LastIndexOf('[');
					return path.Substring(startIndex);
				}
				else
				{
					PropertyInfo propertyInfo;
					PropertyDescriptor propertyDescriptor;
					DependencyProperty dependencyProperty;
					DynamicPropertyAccessor dynamicPropertyAccessor;
					this.SetPropertyInfo(this.GetAccessor(num), out propertyInfo, out propertyDescriptor, out dependencyProperty, out dynamicPropertyAccessor);
					if (dependencyProperty != null)
					{
						return dependencyProperty.Name;
					}
					if (propertyInfo != null)
					{
						return propertyInfo.Name;
					}
					if (propertyDescriptor != null)
					{
						return propertyDescriptor.Name;
					}
					if (dynamicPropertyAccessor == null)
					{
						return null;
					}
					return dynamicPropertyAccessor.PropertyName;
				}
			}
		}

		// Token: 0x17001C29 RID: 7209
		// (get) Token: 0x0600763B RID: 30267 RVA: 0x0021B5F7 File Offset: 0x002197F7
		// (set) Token: 0x0600763C RID: 30268 RVA: 0x0021B5FF File Offset: 0x002197FF
		internal bool NeedsDirectNotification
		{
			get
			{
				return this._needsDirectNotification;
			}
			private set
			{
				if (value)
				{
					this._dependencySourcesChanged = true;
				}
				this._needsDirectNotification = value;
			}
		}

		// Token: 0x0600763D RID: 30269 RVA: 0x0021B612 File Offset: 0x00219812
		internal object GetItem(int level)
		{
			return BindingExpressionBase.GetReference(this._arySVS[level].item);
		}

		// Token: 0x0600763E RID: 30270 RVA: 0x0021B62A File Offset: 0x0021982A
		internal object GetAccessor(int level)
		{
			return this._arySVS[level].info;
		}

		// Token: 0x0600763F RID: 30271 RVA: 0x0021B640 File Offset: 0x00219840
		internal object[] GetIndexerArguments(int level)
		{
			object[] args = this._arySVS[level].args;
			PropertyPathWorker.IListIndexerArg listIndexerArg;
			if (args != null && args.Length == 1 && (listIndexerArg = (args[0] as PropertyPathWorker.IListIndexerArg)) != null)
			{
				return new object[]
				{
					listIndexerArg.Value
				};
			}
			return args;
		}

		// Token: 0x06007640 RID: 30272 RVA: 0x0021B68A File Offset: 0x0021988A
		internal Type GetType(int level)
		{
			return this._arySVS[level].type;
		}

		// Token: 0x06007641 RID: 30273 RVA: 0x0021B69D File Offset: 0x0021989D
		internal IDisposable SetContext(object rootItem)
		{
			if (this._contextHelper == null)
			{
				this._contextHelper = new PropertyPathWorker.ContextHelper(this);
			}
			this._contextHelper.SetContext(rootItem);
			return this._contextHelper;
		}

		// Token: 0x06007642 RID: 30274 RVA: 0x0021B6C5 File Offset: 0x002198C5
		internal void AttachToRootItem(object rootItem)
		{
			this._rootItem = BindingExpressionBase.CreateReference(rootItem);
			this.UpdateSourceValueState(-1, null);
		}

		// Token: 0x06007643 RID: 30275 RVA: 0x0021B6DB File Offset: 0x002198DB
		internal void DetachFromRootItem()
		{
			this._rootItem = BindingExpression.NullDataItem;
			this.UpdateSourceValueState(-1, null);
			this._rootItem = null;
		}

		// Token: 0x06007644 RID: 30276 RVA: 0x0021B6F8 File Offset: 0x002198F8
		internal object GetValue(object item, int level)
		{
			bool flag = this.IsExtendedTraceEnabled(TraceDataLevel.CreateExpression);
			object obj = DependencyProperty.UnsetValue;
			PropertyInfo propertyInfo;
			PropertyDescriptor propertyDescriptor;
			DependencyProperty dependencyProperty;
			DynamicPropertyAccessor dynamicPropertyAccessor;
			this.SetPropertyInfo(this._arySVS[level].info, out propertyInfo, out propertyDescriptor, out dependencyProperty, out dynamicPropertyAccessor);
			switch (this.SVI[level].type)
			{
			case SourceValueType.Property:
				if (propertyInfo != null)
				{
					obj = propertyInfo.GetValue(item, null);
				}
				else if (propertyDescriptor != null)
				{
					bool indexerIsNext = level + 1 < this.SVI.Length && this.SVI[level + 1].type == SourceValueType.Indexer;
					obj = this.Engine.GetValue(item, propertyDescriptor, indexerIsNext);
				}
				else if (dependencyProperty != null)
				{
					DependencyObject dependencyObject = (DependencyObject)item;
					if (level != this.Length - 1 || this._host == null || this._host.TransfersDefaultValue)
					{
						obj = dependencyObject.GetValue(dependencyProperty);
					}
					else if (!Helper.HasDefaultValue(dependencyObject, dependencyProperty))
					{
						obj = dependencyObject.GetValue(dependencyProperty);
					}
					else
					{
						obj = BindingExpression.IgnoreDefaultValue;
					}
				}
				else if (dynamicPropertyAccessor != null)
				{
					obj = dynamicPropertyAccessor.GetValue(item);
				}
				break;
			case SourceValueType.Indexer:
				if (propertyInfo != null)
				{
					object[] args = this._arySVS[level].args;
					PropertyPathWorker.IListIndexerArg listIndexerArg;
					if (args != null && args.Length == 1 && (listIndexerArg = (args[0] as PropertyPathWorker.IListIndexerArg)) != null)
					{
						int value = listIndexerArg.Value;
						IList list = (IList)item;
						if (0 <= value && value < list.Count)
						{
							obj = list[value];
						}
						else
						{
							obj = PropertyPathWorker.IListIndexOutOfRange;
						}
					}
					else
					{
						obj = propertyInfo.GetValue(item, BindingFlags.GetProperty, null, args, CultureInfo.InvariantCulture);
					}
				}
				else
				{
					DynamicIndexerAccessor dynamicIndexerAccessor;
					if ((dynamicIndexerAccessor = (this._arySVS[level].info as DynamicIndexerAccessor)) == null)
					{
						throw new NotSupportedException(SR.Get("IndexedPropDescNotImplemented"));
					}
					obj = dynamicIndexerAccessor.GetValue(item, this._arySVS[level].args);
				}
				break;
			case SourceValueType.Direct:
				obj = item;
				break;
			}
			if (flag)
			{
				object obj2 = this._arySVS[level].info;
				if (obj2 == DependencyProperty.UnsetValue)
				{
					obj2 = null;
				}
				TraceData.Trace(TraceEventType.Warning, TraceData.GetValue(new object[]
				{
					TraceData.Identify(this._host.ParentBindingExpression),
					level,
					TraceData.Identify(item),
					TraceData.IdentifyAccessor(obj2),
					TraceData.Identify(obj)
				}));
			}
			return obj;
		}

		// Token: 0x06007645 RID: 30277 RVA: 0x0021B96C File Offset: 0x00219B6C
		internal void SetValue(object item, object value)
		{
			bool flag = this.IsExtendedTraceEnabled(TraceDataLevel.CreateExpression);
			int num = this._arySVS.Length - 1;
			PropertyInfo propertyInfo;
			PropertyDescriptor propertyDescriptor;
			DependencyProperty dependencyProperty;
			DynamicPropertyAccessor dynamicPropertyAccessor;
			this.SetPropertyInfo(this._arySVS[num].info, out propertyInfo, out propertyDescriptor, out dependencyProperty, out dynamicPropertyAccessor);
			if (flag)
			{
				TraceData.Trace(TraceEventType.Warning, TraceData.SetValue(new object[]
				{
					TraceData.Identify(this._host.ParentBindingExpression),
					num,
					TraceData.Identify(item),
					TraceData.IdentifyAccessor(this._arySVS[num].info),
					TraceData.Identify(value)
				}));
			}
			SourceValueType type = this.SVI[num].type;
			if (type != SourceValueType.Property)
			{
				if (type != SourceValueType.Indexer)
				{
					return;
				}
				if (propertyInfo != null)
				{
					propertyInfo.SetValue(item, value, BindingFlags.SetProperty, null, this.GetIndexerArguments(num), CultureInfo.InvariantCulture);
					return;
				}
				DynamicIndexerAccessor dynamicIndexerAccessor;
				if ((dynamicIndexerAccessor = (this._arySVS[num].info as DynamicIndexerAccessor)) != null)
				{
					dynamicIndexerAccessor.SetValue(item, this._arySVS[num].args, value);
					return;
				}
				throw new NotSupportedException(SR.Get("IndexedPropDescNotImplemented"));
			}
			else
			{
				if (propertyDescriptor != null)
				{
					propertyDescriptor.SetValue(item, value);
					return;
				}
				if (propertyInfo != null)
				{
					propertyInfo.SetValue(item, value, null);
					return;
				}
				if (dependencyProperty != null)
				{
					((DependencyObject)item).SetValue(dependencyProperty, value);
					return;
				}
				if (dynamicPropertyAccessor != null)
				{
					dynamicPropertyAccessor.SetValue(item, value);
					return;
				}
				return;
			}
		}

		// Token: 0x06007646 RID: 30278 RVA: 0x0021BAD4 File Offset: 0x00219CD4
		internal object RawValue()
		{
			object obj = this.RawValue(this.Length - 1);
			if (obj == PropertyPathWorker.AsyncRequestPending)
			{
				obj = DependencyProperty.UnsetValue;
			}
			return obj;
		}

		// Token: 0x06007647 RID: 30279 RVA: 0x0021BB00 File Offset: 0x00219D00
		internal void RefreshValue()
		{
			for (int i = 1; i < this._arySVS.Length; i++)
			{
				object reference = BindingExpressionBase.GetReference(this._arySVS[i].item);
				if (!ItemsControl.EqualsEx(reference, this.RawValue(i - 1)))
				{
					this.UpdateSourceValueState(i - 1, null);
					return;
				}
			}
			this.UpdateSourceValueState(this.Length - 1, null);
		}

		// Token: 0x06007648 RID: 30280 RVA: 0x0021BB64 File Offset: 0x00219D64
		internal int LevelForPropertyChange(object item, string propertyName)
		{
			bool flag = propertyName == "Item[]";
			for (int i = 0; i < this._arySVS.Length; i++)
			{
				object obj = BindingExpressionBase.GetReference(this._arySVS[i].item);
				if (obj == BindingExpression.StaticSource)
				{
					obj = null;
				}
				if (obj == item && (string.IsNullOrEmpty(propertyName) || (flag && this.SVI[i].type == SourceValueType.Indexer) || string.Equals(this.SVI[i].propertyName, propertyName, StringComparison.OrdinalIgnoreCase)))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06007649 RID: 30281 RVA: 0x0021BBF1 File Offset: 0x00219DF1
		internal void OnPropertyChangedAtLevel(int level)
		{
			this.UpdateSourceValueState(level, null);
		}

		// Token: 0x0600764A RID: 30282 RVA: 0x0021BBFC File Offset: 0x00219DFC
		internal void OnCurrentChanged(ICollectionView collectionView)
		{
			for (int i = 0; i < this.Length; i++)
			{
				if (this._arySVS[i].collectionView == collectionView)
				{
					this._host.CancelPendingTasks();
					this.UpdateSourceValueState(i, collectionView);
					return;
				}
			}
		}

		// Token: 0x0600764B RID: 30283 RVA: 0x0021BC44 File Offset: 0x00219E44
		internal bool UsesDependencyProperty(DependencyObject d, DependencyProperty dp)
		{
			if (dp == DependencyObject.DirectDependencyProperty)
			{
				return true;
			}
			for (int i = 0; i < this._arySVS.Length; i++)
			{
				if (this._arySVS[i].info == dp && BindingExpressionBase.GetReference(this._arySVS[i].item) == d)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600764C RID: 30284 RVA: 0x0021BCA0 File Offset: 0x00219EA0
		internal void OnDependencyPropertyChanged(DependencyObject d, DependencyProperty dp, bool isASubPropertyChange)
		{
			if (dp == DependencyObject.DirectDependencyProperty)
			{
				this.UpdateSourceValueState(this._arySVS.Length, null, BindingExpression.NullDataItem, isASubPropertyChange);
				return;
			}
			for (int i = 0; i < this._arySVS.Length; i++)
			{
				if (this._arySVS[i].info == dp && BindingExpressionBase.GetReference(this._arySVS[i].item) == d)
				{
					this.UpdateSourceValueState(i, null, BindingExpression.NullDataItem, isASubPropertyChange);
					return;
				}
			}
		}

		// Token: 0x0600764D RID: 30285 RVA: 0x0021BD1A File Offset: 0x00219F1A
		internal void OnNewValue(int level, object value)
		{
			this._status = PropertyPathStatus.Active;
			if (level < this.Length - 1)
			{
				this.UpdateSourceValueState(level, null, value, false);
			}
		}

		// Token: 0x0600764E RID: 30286 RVA: 0x0021BD38 File Offset: 0x00219F38
		internal SourceValueInfo GetSourceValueInfo(int level)
		{
			return this.SVI[level];
		}

		// Token: 0x0600764F RID: 30287 RVA: 0x0021BD48 File Offset: 0x00219F48
		internal static bool IsIndexedProperty(PropertyInfo pi)
		{
			bool result = false;
			try
			{
				result = (pi != null && pi.GetIndexParameters().Length != 0);
			}
			catch (Exception ex)
			{
				if (CriticalExceptions.IsCriticalApplicationException(ex))
				{
					throw;
				}
			}
			return result;
		}

		// Token: 0x17001C2A RID: 7210
		// (get) Token: 0x06007650 RID: 30288 RVA: 0x0021BD90 File Offset: 0x00219F90
		private bool IsDynamic
		{
			get
			{
				return this._isDynamic;
			}
		}

		// Token: 0x17001C2B RID: 7211
		// (get) Token: 0x06007651 RID: 30289 RVA: 0x0021BD98 File Offset: 0x00219F98
		private SourceValueInfo[] SVI
		{
			get
			{
				return this._parent.SVI;
			}
		}

		// Token: 0x17001C2C RID: 7212
		// (get) Token: 0x06007652 RID: 30290 RVA: 0x0021BDA5 File Offset: 0x00219FA5
		private DataBindEngine Engine
		{
			get
			{
				return this._engine;
			}
		}

		// Token: 0x06007653 RID: 30291 RVA: 0x0021BDAD File Offset: 0x00219FAD
		private void UpdateSourceValueState(int k, ICollectionView collectionView)
		{
			this.UpdateSourceValueState(k, collectionView, BindingExpression.NullDataItem, false);
		}

		// Token: 0x06007654 RID: 30292 RVA: 0x0021BDC0 File Offset: 0x00219FC0
		private void UpdateSourceValueState(int k, ICollectionView collectionView, object newValue, bool isASubPropertyChange)
		{
			DependencyObject dependencyObject = null;
			if (this._host != null)
			{
				dependencyObject = this._host.CheckTarget();
				if (this._rootItem != BindingExpression.NullDataItem && dependencyObject == null)
				{
					return;
				}
			}
			int num = k;
			bool flag = this._host == null || k < 0;
			this._status = PropertyPathStatus.Active;
			this._dependencySourcesChanged = false;
			if (collectionView != null)
			{
				this.ReplaceItem(k, collectionView.CurrentItem, PropertyPathWorker.NoParent);
			}
			for (k++; k < this._arySVS.Length; k++)
			{
				isASubPropertyChange = false;
				ICollectionView collectionView2 = this._arySVS[k].collectionView;
				object obj = (newValue == BindingExpression.NullDataItem) ? this.RawValue(k - 1) : newValue;
				newValue = BindingExpression.NullDataItem;
				if (obj == PropertyPathWorker.AsyncRequestPending)
				{
					this._status = PropertyPathStatus.AsyncRequestPending;
					break;
				}
				if (!flag && obj == BindingExpressionBase.DisconnectedItem && this._arySVS[k - 1].info == FrameworkElement.DataContextProperty)
				{
					flag = true;
				}
				this.ReplaceItem(k, BindingExpression.NullDataItem, obj);
				ICollectionView collectionView3 = this._arySVS[k].collectionView;
				if (collectionView2 != collectionView3 && this._host != null)
				{
					this._host.ReplaceCurrentItem(collectionView2, collectionView3);
				}
			}
			if (this._host != null)
			{
				if (num < this._arySVS.Length)
				{
					this.NeedsDirectNotification = (this._status == PropertyPathStatus.Active && this._arySVS.Length != 0 && this.SVI[this._arySVS.Length - 1].type != SourceValueType.Direct && !(this._arySVS[this._arySVS.Length - 1].info is DependencyProperty) && typeof(DependencyObject).IsAssignableFrom(this._arySVS[this._arySVS.Length - 1].type));
				}
				if (!flag && this._arySVS.Length != 0 && this._arySVS[this._arySVS.Length - 1].info == FrameworkElement.DataContextProperty && this.RawValue() == BindingExpressionBase.DisconnectedItem)
				{
					flag = true;
				}
				this._host.NewValueAvailable(this._dependencySourcesChanged, flag, isASubPropertyChange);
			}
			GC.KeepAlive(dependencyObject);
		}

		// Token: 0x06007655 RID: 30293 RVA: 0x0021BFE0 File Offset: 0x0021A1E0
		private void ReplaceItem(int k, object newO, object parent)
		{
			bool flag = this.IsExtendedTraceEnabled(TraceDataLevel.Transfer);
			PropertyPathWorker.SourceValueState sourceValueState = default(PropertyPathWorker.SourceValueState);
			object reference = BindingExpressionBase.GetReference(this._arySVS[k].item);
			if (this.IsDynamic && this.SVI[k].type != SourceValueType.Direct)
			{
				DependencyProperty dependencyProperty;
				PropertyInfo propertyInfo;
				PropertyDescriptor propertyDescriptor;
				DynamicObjectAccessor dynamicObjectAccessor;
				PropertyPath.DowncastAccessor(this._arySVS[k].info, out dependencyProperty, out propertyInfo, out propertyDescriptor, out dynamicObjectAccessor);
				INotifyPropertyChanged source;
				if (reference == BindingExpression.StaticSource)
				{
					Type type = (propertyInfo != null) ? propertyInfo.DeclaringType : ((propertyDescriptor != null) ? propertyDescriptor.ComponentType : null);
					if (type != null)
					{
						StaticPropertyChangedEventManager.RemoveHandler(type, new EventHandler<PropertyChangedEventArgs>(this.OnStaticPropertyChanged), this.SVI[k].propertyName);
					}
				}
				else if (dependencyProperty != null)
				{
					this._dependencySourcesChanged = true;
				}
				else if ((source = (reference as INotifyPropertyChanged)) != null)
				{
					PropertyChangedEventManager.RemoveHandler(source, new EventHandler<PropertyChangedEventArgs>(this.OnPropertyChanged), this.SVI[k].propertyName);
				}
				else if (propertyDescriptor != null && reference != null)
				{
					ValueChangedEventManager.RemoveHandler(reference, new EventHandler<ValueChangedEventArgs>(this.OnValueChanged), propertyDescriptor);
				}
			}
			if (this._host != null && k == this.Length - 1 && this.IsDynamic && this._host.ValidatesOnNotifyDataErrors)
			{
				INotifyDataErrorInfo notifyDataErrorInfo = reference as INotifyDataErrorInfo;
				if (notifyDataErrorInfo != null)
				{
					ErrorsChangedEventManager.RemoveHandler(notifyDataErrorInfo, new EventHandler<DataErrorsChangedEventArgs>(this.OnErrorsChanged));
				}
			}
			this._isDBNullValidForUpdate = null;
			if (newO == null || parent == DependencyProperty.UnsetValue || parent == BindingExpression.NullDataItem || parent == BindingExpressionBase.DisconnectedItem)
			{
				this._arySVS[k].item = BindingExpressionBase.ReplaceReference(this._arySVS[k].item, newO);
				if (parent == DependencyProperty.UnsetValue || parent == BindingExpression.NullDataItem || parent == BindingExpressionBase.DisconnectedItem)
				{
					this._arySVS[k].collectionView = null;
				}
				if (flag)
				{
					TraceData.Trace(TraceEventType.Warning, TraceData.ReplaceItemShort(new object[]
					{
						TraceData.Identify(this._host.ParentBindingExpression),
						k,
						TraceData.Identify(newO)
					}));
				}
				return;
			}
			if (newO != BindingExpression.NullDataItem)
			{
				parent = newO;
				this.GetInfo(k, newO, ref sourceValueState);
				sourceValueState.collectionView = this._arySVS[k].collectionView;
			}
			else
			{
				DrillIn drillIn = this.SVI[k].drillIn;
				ICollectionView collectionView = null;
				if (drillIn != DrillIn.Always)
				{
					this.GetInfo(k, parent, ref sourceValueState);
				}
				if (sourceValueState.info == null)
				{
					collectionView = CollectionViewSource.GetDefaultCollectionView(parent, this.TreeContext, (object x) => BindingExpressionBase.GetReference((k == 0) ? this._rootItem : this._arySVS[k - 1].item));
					if (collectionView != null && drillIn != DrillIn.Always && collectionView != parent)
					{
						this.GetInfo(k, collectionView, ref sourceValueState);
					}
				}
				if (sourceValueState.info == null && drillIn != DrillIn.Never && collectionView != null)
				{
					newO = collectionView.CurrentItem;
					if (newO != null)
					{
						this.GetInfo(k, newO, ref sourceValueState);
						sourceValueState.collectionView = collectionView;
					}
					else
					{
						sourceValueState = this._arySVS[k];
						sourceValueState.collectionView = collectionView;
						if (!SystemXmlHelper.IsEmptyXmlDataCollection(parent))
						{
							sourceValueState.item = BindingExpressionBase.ReplaceReference(sourceValueState.item, BindingExpression.NullDataItem);
							if (sourceValueState.info == null)
							{
								sourceValueState.info = DependencyProperty.UnsetValue;
							}
						}
					}
				}
			}
			if (sourceValueState.info == null)
			{
				sourceValueState.item = BindingExpressionBase.ReplaceReference(sourceValueState.item, BindingExpression.NullDataItem);
				this._arySVS[k] = sourceValueState;
				this._status = PropertyPathStatus.PathError;
				this.ReportNoInfoError(k, parent);
				return;
			}
			this._arySVS[k] = sourceValueState;
			newO = BindingExpressionBase.GetReference(sourceValueState.item);
			if (flag)
			{
				TraceData.Trace(TraceEventType.Warning, TraceData.ReplaceItemLong(new object[]
				{
					TraceData.Identify(this._host.ParentBindingExpression),
					k,
					TraceData.Identify(newO),
					TraceData.IdentifyAccessor(sourceValueState.info)
				}));
			}
			if (this.IsDynamic && this.SVI[k].type != SourceValueType.Direct)
			{
				this.Engine.RegisterForCacheChanges(newO, sourceValueState.info);
				DependencyProperty dependencyProperty2;
				PropertyInfo propertyInfo2;
				PropertyDescriptor propertyDescriptor2;
				DynamicObjectAccessor dynamicObjectAccessor2;
				PropertyPath.DowncastAccessor(sourceValueState.info, out dependencyProperty2, out propertyInfo2, out propertyDescriptor2, out dynamicObjectAccessor2);
				INotifyPropertyChanged source2;
				if (newO == BindingExpression.StaticSource)
				{
					Type type2 = (propertyInfo2 != null) ? propertyInfo2.DeclaringType : ((propertyDescriptor2 != null) ? propertyDescriptor2.ComponentType : null);
					if (type2 != null)
					{
						StaticPropertyChangedEventManager.AddHandler(type2, new EventHandler<PropertyChangedEventArgs>(this.OnStaticPropertyChanged), this.SVI[k].propertyName);
					}
				}
				else if (dependencyProperty2 != null)
				{
					this._dependencySourcesChanged = true;
				}
				else if ((source2 = (newO as INotifyPropertyChanged)) != null)
				{
					PropertyChangedEventManager.AddHandler(source2, new EventHandler<PropertyChangedEventArgs>(this.OnPropertyChanged), this.SVI[k].propertyName);
				}
				else if (propertyDescriptor2 != null && newO != null)
				{
					ValueChangedEventManager.AddHandler(newO, new EventHandler<ValueChangedEventArgs>(this.OnValueChanged), propertyDescriptor2);
				}
			}
			if (this._host != null && k == this.Length - 1)
			{
				this._host.SetupDefaultValueConverter(sourceValueState.type);
				if (this._host.IsReflective)
				{
					this.CheckReadOnly(newO, sourceValueState.info);
				}
				if (this._host.ValidatesOnNotifyDataErrors)
				{
					INotifyDataErrorInfo notifyDataErrorInfo2 = newO as INotifyDataErrorInfo;
					if (notifyDataErrorInfo2 != null)
					{
						if (this.IsDynamic)
						{
							ErrorsChangedEventManager.AddHandler(notifyDataErrorInfo2, new EventHandler<DataErrorsChangedEventArgs>(this.OnErrorsChanged));
						}
						this._host.OnDataErrorsChanged(notifyDataErrorInfo2, this.SourcePropertyName);
					}
				}
			}
		}

		// Token: 0x06007656 RID: 30294 RVA: 0x0021C5C8 File Offset: 0x0021A7C8
		private void ReportNoInfoError(int k, object parent)
		{
			if (TraceData.IsEnabled)
			{
				BindingExpression bindingExpression = (this._host != null) ? this._host.ParentBindingExpression : null;
				if (bindingExpression == null || !bindingExpression.IsInPriorityBindingExpression)
				{
					if (!SystemXmlHelper.IsEmptyXmlDataCollection(parent))
					{
						SourceValueInfo sourceValueInfo = this.SVI[k];
						string text = (sourceValueInfo.type != SourceValueType.Indexer) ? sourceValueInfo.name : ("[" + sourceValueInfo.name + "]");
						string text2 = TraceData.DescribeSourceObject(parent);
						string text3 = (sourceValueInfo.drillIn == DrillIn.Always) ? "current item of collection" : "object";
						if (parent == null)
						{
							TraceData.Trace(TraceEventType.Information, TraceData.NullItem(new object[]
							{
								text,
								text3
							}), bindingExpression);
							return;
						}
						if (parent == CollectionView.NewItemPlaceholder || parent == DataGrid.NewItemPlaceholder)
						{
							TraceData.Trace(TraceEventType.Information, TraceData.PlaceholderItem(new object[]
							{
								text,
								text3
							}), bindingExpression);
							return;
						}
						TraceEventType type = (bindingExpression != null) ? bindingExpression.TraceLevel : TraceEventType.Error;
						TraceData.Trace(type, TraceData.ClrReplaceItem(new object[]
						{
							text,
							text2,
							text3
						}), bindingExpression);
						return;
					}
					else
					{
						TraceEventType traceType = (bindingExpression != null) ? bindingExpression.TraceLevel : TraceEventType.Error;
						this._host.ReportBadXPath(traceType);
					}
				}
			}
		}

		// Token: 0x06007657 RID: 30295 RVA: 0x0021C6F8 File Offset: 0x0021A8F8
		internal bool IsPathCurrent(object rootItem)
		{
			if (this.Status != PropertyPathStatus.Active)
			{
				return false;
			}
			object obj = rootItem;
			int i = 0;
			int length = this.Length;
			while (i < length)
			{
				ICollectionView collectionView = this._arySVS[i].collectionView;
				if (collectionView != null)
				{
					obj = collectionView.CurrentItem;
				}
				if (PropertyPath.IsStaticProperty(this._arySVS[i].info))
				{
					obj = BindingExpression.StaticSource;
				}
				if (!ItemsControl.EqualsEx(obj, BindingExpressionBase.GetReference(this._arySVS[i].item)) && !this.IsNonIdempotentProperty(i - 1))
				{
					return false;
				}
				if (i < length - 1)
				{
					obj = this.GetValue(obj, i);
				}
				i++;
			}
			return true;
		}

		// Token: 0x06007658 RID: 30296 RVA: 0x0021C79C File Offset: 0x0021A99C
		private bool IsNonIdempotentProperty(int level)
		{
			PropertyDescriptor pd;
			return level >= 0 && (pd = (this._arySVS[level].info as PropertyDescriptor)) != null && SystemXmlLinqHelper.IsXLinqNonIdempotentProperty(pd);
		}

		// Token: 0x06007659 RID: 30297 RVA: 0x0021C7D0 File Offset: 0x0021A9D0
		private void GetInfo(int k, object item, ref PropertyPathWorker.SourceValueState svs)
		{
			object reference = BindingExpressionBase.GetReference(this._arySVS[k].item);
			bool flag = this.IsExtendedTraceEnabled(TraceDataLevel.Transfer);
			Type reflectionType = ReflectionHelper.GetReflectionType(reference);
			Type reflectionType2 = ReflectionHelper.GetReflectionType(item);
			Type type = null;
			if (reflectionType2 == reflectionType && reference != BindingExpression.NullDataItem && !(this._arySVS[k].info is PropertyDescriptor))
			{
				svs = this._arySVS[k];
				svs.item = BindingExpressionBase.ReplaceReference(svs.item, item);
				if (flag)
				{
					TraceData.Trace(TraceEventType.Warning, TraceData.GetInfo_Reuse(new object[]
					{
						TraceData.Identify(this._host.ParentBindingExpression),
						k,
						TraceData.IdentifyAccessor(svs.info)
					}));
				}
				return;
			}
			if (reflectionType2 == null && this.SVI[k].type != SourceValueType.Direct)
			{
				svs.info = null;
				svs.args = null;
				svs.type = null;
				svs.item = BindingExpressionBase.ReplaceReference(svs.item, item);
				if (flag)
				{
					TraceData.Trace(TraceEventType.Warning, TraceData.GetInfo_Null(new object[]
					{
						TraceData.Identify(this._host.ParentBindingExpression),
						k
					}));
				}
				return;
			}
			int num;
			bool flag2 = !PropertyPath.IsParameterIndex(this.SVI[k].name, out num);
			if (flag2)
			{
				AccessorInfo accessorInfo = this.Engine.AccessorTable[this.SVI[k].type, reflectionType2, this.SVI[k].name];
				if (accessorInfo != null)
				{
					svs.info = accessorInfo.Accessor;
					svs.type = accessorInfo.PropertyType;
					svs.args = accessorInfo.Args;
					if (PropertyPath.IsStaticProperty(svs.info))
					{
						item = BindingExpression.StaticSource;
					}
					svs.item = BindingExpressionBase.ReplaceReference(svs.item, item);
					if (this.IsDynamic && this.SVI[k].type == SourceValueType.Property && svs.info is DependencyProperty)
					{
						this._dependencySourcesChanged = true;
					}
					if (flag)
					{
						TraceData.Trace(TraceEventType.Warning, TraceData.GetInfo_Cache(new object[]
						{
							TraceData.Identify(this._host.ParentBindingExpression),
							k,
							reflectionType2.Name,
							this.SVI[k].name,
							TraceData.IdentifyAccessor(svs.info)
						}));
					}
					return;
				}
			}
			object obj = null;
			object[] array = null;
			switch (this.SVI[k].type)
			{
			case SourceValueType.Property:
				break;
			case SourceValueType.Indexer:
			{
				IndexerParameterInfo[] array2 = this._parent.ResolveIndexerParams(k, this.TreeContext);
				if (array2.Length == 1 && (array2[0].type == null || array2[0].type == typeof(string)))
				{
					string name = (string)array2[0].value;
					if (this.ShouldConvertIndexerToProperty(item, ref name))
					{
						this._parent.ReplaceIndexerByProperty(k, name);
						break;
					}
				}
				array = new object[array2.Length];
				MemberInfo[][] array3 = new MemberInfo[2][];
				array3[0] = this.GetIndexers(reflectionType2, k);
				MemberInfo[][] array4 = array3;
				bool flag3 = item is IList;
				if (flag3)
				{
					array4[1] = typeof(IList).GetDefaultMembers();
				}
				int num2 = 0;
				while (obj == null && num2 < array4.Length)
				{
					if (array4[num2] != null)
					{
						MemberInfo[] array5 = array4[num2];
						int i = 0;
						while (i < array5.Length)
						{
							PropertyInfo propertyInfo = array5[i] as PropertyInfo;
							if (propertyInfo != null && this.MatchIndexerParameters(propertyInfo, array2, array, flag3))
							{
								obj = propertyInfo;
								type = reflectionType2.GetElementType();
								if (type == null)
								{
									type = propertyInfo.PropertyType;
									break;
								}
								break;
							}
							else
							{
								i++;
							}
						}
					}
					num2++;
				}
				if (obj == null && SystemCoreHelper.IsIDynamicMetaObjectProvider(item) && this.MatchIndexerParameters(null, array2, array, false))
				{
					obj = SystemCoreHelper.GetIndexerAccessor(array.Length);
					type = typeof(object);
				}
				if (flag)
				{
					TraceData.Trace(TraceEventType.Warning, TraceData.GetInfo_Indexer(new object[]
					{
						TraceData.Identify(this._host.ParentBindingExpression),
						k,
						reflectionType2.Name,
						this.SVI[k].name,
						TraceData.IdentifyAccessor(obj)
					}));
					goto IL_598;
				}
				goto IL_598;
			}
			case SourceValueType.Direct:
				if (item is ICollectionView && this._host != null && !this._host.IsValidValue(item))
				{
					goto IL_598;
				}
				obj = DependencyProperty.UnsetValue;
				type = reflectionType2;
				if (this.Length == 1 && item is Freezable && item != this.TreeContext)
				{
					obj = DependencyObject.DirectDependencyProperty;
					this._dependencySourcesChanged = true;
					goto IL_598;
				}
				goto IL_598;
			default:
				goto IL_598;
			}
			obj = this._parent.ResolvePropertyName(k, item, reflectionType2, this.TreeContext);
			if (flag)
			{
				TraceData.Trace(TraceEventType.Warning, TraceData.GetInfo_Property(new object[]
				{
					TraceData.Identify(this._host.ParentBindingExpression),
					k,
					reflectionType2.Name,
					this.SVI[k].name,
					TraceData.IdentifyAccessor(obj)
				}));
			}
			DependencyProperty dependencyProperty;
			PropertyInfo propertyInfo2;
			PropertyDescriptor propertyDescriptor;
			DynamicObjectAccessor dynamicObjectAccessor;
			PropertyPath.DowncastAccessor(obj, out dependencyProperty, out propertyInfo2, out propertyDescriptor, out dynamicObjectAccessor);
			if (dependencyProperty != null)
			{
				type = dependencyProperty.PropertyType;
				if (this.IsDynamic)
				{
					this._dependencySourcesChanged = true;
				}
			}
			else if (propertyInfo2 != null)
			{
				type = propertyInfo2.PropertyType;
			}
			else if (propertyDescriptor != null)
			{
				type = propertyDescriptor.PropertyType;
			}
			else if (dynamicObjectAccessor != null)
			{
				type = dynamicObjectAccessor.PropertyType;
			}
			IL_598:
			if (PropertyPath.IsStaticProperty(obj))
			{
				item = BindingExpression.StaticSource;
			}
			svs.info = obj;
			svs.args = array;
			svs.type = type;
			svs.item = BindingExpressionBase.ReplaceReference(svs.item, item);
			if (flag2 && obj != null && !(obj is PropertyDescriptor))
			{
				this.Engine.AccessorTable[this.SVI[k].type, reflectionType2, this.SVI[k].name] = new AccessorInfo(obj, type, array);
			}
		}

		// Token: 0x0600765A RID: 30298 RVA: 0x0021CE00 File Offset: 0x0021B000
		private MemberInfo[] GetIndexers(Type type, int k)
		{
			if (k > 0 && this._arySVS[k - 1].info == IndexerPropertyInfo.Instance)
			{
				List<MemberInfo> list = new List<MemberInfo>();
				string name = this.SVI[k - 1].name;
				PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);
				foreach (PropertyInfo propertyInfo in properties)
				{
					if (propertyInfo.Name == name && PropertyPathWorker.IsIndexedProperty(propertyInfo))
					{
						list.Add(propertyInfo);
					}
				}
				return list.ToArray();
			}
			return type.GetDefaultMembers();
		}

		// Token: 0x0600765B RID: 30299 RVA: 0x0021CE98 File Offset: 0x0021B098
		private bool MatchIndexerParameters(PropertyInfo pi, IndexerParameterInfo[] aryInfo, object[] args, bool isIList)
		{
			ParameterInfo[] array = (pi != null) ? pi.GetIndexParameters() : null;
			if (array != null && array.Length != aryInfo.Length)
			{
				return false;
			}
			for (int i = 0; i < args.Length; i++)
			{
				IndexerParameterInfo indexerParameterInfo = aryInfo[i];
				Type type = (array != null) ? array[i].ParameterType : typeof(object);
				if (indexerParameterInfo.type != null)
				{
					if (!type.IsAssignableFrom(indexerParameterInfo.type))
					{
						return false;
					}
					args.SetValue(indexerParameterInfo.value, i);
				}
				else
				{
					try
					{
						object obj = null;
						if (type == typeof(int))
						{
							int num;
							if (int.TryParse((string)indexerParameterInfo.value, NumberStyles.Integer, TypeConverterHelper.InvariantEnglishUS.NumberFormat, out num))
							{
								obj = num;
							}
						}
						else
						{
							TypeConverter converter = TypeDescriptor.GetConverter(type);
							if (converter != null && converter.CanConvertFrom(typeof(string)))
							{
								obj = converter.ConvertFromString(null, TypeConverterHelper.InvariantEnglishUS, (string)indexerParameterInfo.value);
							}
						}
						if (obj == null && type.IsAssignableFrom(typeof(string)))
						{
							obj = indexerParameterInfo.value;
						}
						if (obj == null)
						{
							return false;
						}
						args.SetValue(obj, i);
					}
					catch (Exception ex)
					{
						if (CriticalExceptions.IsCriticalApplicationException(ex))
						{
							throw;
						}
						return false;
					}
					catch
					{
						return false;
					}
				}
			}
			if (isIList && array.Length == 1 && array[0].ParameterType == typeof(int))
			{
				bool flag = true;
				if (!FrameworkAppContextSwitches.IListIndexerHidesCustomIndexer)
				{
					Type type2 = pi.DeclaringType;
					if (type2.IsGenericType)
					{
						type2 = type2.GetGenericTypeDefinition();
					}
					flag = PropertyPathWorker.IListIndexerWhitelist.Contains(type2);
				}
				if (flag)
				{
					args[0] = new PropertyPathWorker.IListIndexerArg((int)args[0]);
				}
			}
			return true;
		}

		// Token: 0x0600765C RID: 30300 RVA: 0x0021D074 File Offset: 0x0021B274
		private bool ShouldConvertIndexerToProperty(object item, ref string name)
		{
			if (SystemDataHelper.IsDataRowView(item))
			{
				PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(item);
				if (properties[name] != null)
				{
					return true;
				}
				int num;
				if (int.TryParse(name, NumberStyles.Integer, TypeConverterHelper.InvariantEnglishUS.NumberFormat, out num) && 0 <= num && num < properties.Count)
				{
					name = properties[num].Name;
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600765D RID: 30301 RVA: 0x0021D0D0 File Offset: 0x0021B2D0
		private object RawValue(int k)
		{
			if (k < 0)
			{
				return BindingExpressionBase.GetReference(this._rootItem);
			}
			if (k >= this._arySVS.Length)
			{
				return DependencyProperty.UnsetValue;
			}
			object reference = BindingExpressionBase.GetReference(this._arySVS[k].item);
			object info = this._arySVS[k].info;
			if (reference == BindingExpression.NullDataItem || info == null || (reference == null && info != DependencyProperty.UnsetValue))
			{
				if (this._host != null)
				{
					this._host.ReportRawValueErrors(k, reference, info);
				}
				return DependencyProperty.UnsetValue;
			}
			object obj = DependencyProperty.UnsetValue;
			if (!(info is DependencyProperty) && this.SVI[k].type != SourceValueType.Direct && this._host != null && this._host.AsyncGet(reference, k))
			{
				this._status = PropertyPathStatus.AsyncRequestPending;
				return PropertyPathWorker.AsyncRequestPending;
			}
			try
			{
				obj = this.GetValue(reference, k);
			}
			catch (Exception ex)
			{
				if (CriticalExceptions.IsCriticalApplicationException(ex))
				{
					throw;
				}
				BindingOperations.LogException(ex);
				if (this._host != null)
				{
					this._host.ReportGetValueError(k, reference, ex);
				}
			}
			catch
			{
				if (this._host != null)
				{
					this._host.ReportGetValueError(k, reference, new InvalidOperationException(SR.Get("NonCLSException", new object[]
					{
						"GetValue"
					})));
				}
			}
			if (obj == PropertyPathWorker.IListIndexOutOfRange)
			{
				obj = DependencyProperty.UnsetValue;
				if (this._host != null)
				{
					this._host.ReportGetValueError(k, reference, new ArgumentOutOfRangeException("index"));
				}
			}
			return obj;
		}

		// Token: 0x0600765E RID: 30302 RVA: 0x0021D264 File Offset: 0x0021B464
		private void SetPropertyInfo(object info, out PropertyInfo pi, out PropertyDescriptor pd, out DependencyProperty dp, out DynamicPropertyAccessor dpa)
		{
			pi = null;
			pd = null;
			dpa = null;
			dp = (info as DependencyProperty);
			if (dp == null)
			{
				pi = (info as PropertyInfo);
				if (pi == null)
				{
					pd = (info as PropertyDescriptor);
					if (pd == null)
					{
						dpa = (info as DynamicPropertyAccessor);
					}
				}
			}
		}

		// Token: 0x0600765F RID: 30303 RVA: 0x0021D2B0 File Offset: 0x0021B4B0
		private void CheckReadOnly(object item, object info)
		{
			PropertyInfo propertyInfo;
			PropertyDescriptor propertyDescriptor;
			DependencyProperty dependencyProperty;
			DynamicPropertyAccessor dynamicPropertyAccessor;
			this.SetPropertyInfo(info, out propertyInfo, out propertyDescriptor, out dependencyProperty, out dynamicPropertyAccessor);
			if (propertyInfo != null)
			{
				if (this.IsPropertyReadOnly(item, propertyInfo))
				{
					throw new InvalidOperationException(SR.Get("CannotWriteToReadOnly", new object[]
					{
						item.GetType(),
						propertyInfo.Name
					}));
				}
			}
			else if (propertyDescriptor != null)
			{
				if (propertyDescriptor.IsReadOnly)
				{
					throw new InvalidOperationException(SR.Get("CannotWriteToReadOnly", new object[]
					{
						item.GetType(),
						propertyDescriptor.Name
					}));
				}
			}
			else if (dependencyProperty != null)
			{
				if (dependencyProperty.ReadOnly)
				{
					throw new InvalidOperationException(SR.Get("CannotWriteToReadOnly", new object[]
					{
						item.GetType(),
						dependencyProperty.Name
					}));
				}
			}
			else if (dynamicPropertyAccessor != null && dynamicPropertyAccessor.IsReadOnly)
			{
				throw new InvalidOperationException(SR.Get("CannotWriteToReadOnly", new object[]
				{
					item.GetType(),
					dynamicPropertyAccessor.PropertyName
				}));
			}
		}

		// Token: 0x06007660 RID: 30304 RVA: 0x0021D3A8 File Offset: 0x0021B5A8
		private bool IsPropertyReadOnly(object item, PropertyInfo pi)
		{
			if (!pi.CanWrite)
			{
				return true;
			}
			MethodInfo methodInfo = null;
			try
			{
				methodInfo = pi.GetSetMethod(true);
			}
			catch (SecurityException)
			{
				return true;
			}
			catch (Exception ex)
			{
				if (CriticalExceptions.IsCriticalApplicationException(ex))
				{
					throw;
				}
			}
			if (methodInfo == null || methodInfo.IsPublic)
			{
				return false;
			}
			switch (FrameworkCompatibilityPreferences.GetHandleTwoWayBindingToPropertyWithNonPublicSetter())
			{
			case FrameworkCompatibilityPreferences.HandleBindingOptions.Disallow:
				if (this._host != null)
				{
					string text = "(unknown)";
					string text2 = "(unknown)";
					try
					{
						text = pi.Name;
						text2 = pi.DeclaringType.Name;
					}
					catch (Exception ex2)
					{
						if (CriticalExceptions.IsCriticalApplicationException(ex2))
						{
							throw;
						}
					}
					TraceData.Trace(TraceEventType.Warning, TraceData.DisallowTwoWay(new object[]
					{
						text2,
						text
					}), this._host.ParentBindingExpression);
					this._host.ParentBindingExpression.IsReflective = false;
				}
				return false;
			case FrameworkCompatibilityPreferences.HandleBindingOptions.Allow:
				return false;
			}
			return true;
		}

		// Token: 0x06007661 RID: 30305 RVA: 0x0021D4B0 File Offset: 0x0021B6B0
		private void DetermineWhetherDBNullIsValid()
		{
			bool value = false;
			object item = this.GetItem(this.Length - 1);
			if (item != null && AssemblyHelper.IsLoaded(UncommonAssembly.System_Data))
			{
				value = this.DetermineWhetherDBNullIsValid(item);
			}
			this._isDBNullValidForUpdate = new bool?(value);
		}

		// Token: 0x06007662 RID: 30306 RVA: 0x0021D4F0 File Offset: 0x0021B6F0
		private bool DetermineWhetherDBNullIsValid(object item)
		{
			PropertyInfo propertyInfo;
			PropertyDescriptor propertyDescriptor;
			DependencyProperty dependencyProperty;
			DynamicPropertyAccessor dynamicPropertyAccessor;
			this.SetPropertyInfo(this._arySVS[this.Length - 1].info, out propertyInfo, out propertyDescriptor, out dependencyProperty, out dynamicPropertyAccessor);
			string text = (propertyDescriptor != null) ? propertyDescriptor.Name : ((propertyInfo != null) ? propertyInfo.Name : null);
			object arg = (text == "Item" && propertyInfo != null) ? this._arySVS[this.Length - 1].args[0] : null;
			return SystemDataHelper.DetermineWhetherDBNullIsValid(item, text, arg);
		}

		// Token: 0x06007663 RID: 30307 RVA: 0x0000B02A File Offset: 0x0000922A
		bool IWeakEventListener.ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
		{
			return false;
		}

		// Token: 0x06007664 RID: 30308 RVA: 0x0021D584 File Offset: 0x0021B784
		private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (this.IsExtendedTraceEnabled(TraceDataLevel.Transfer))
			{
				TraceData.Trace(TraceEventType.Warning, TraceData.GotEvent(new object[]
				{
					TraceData.Identify(this._host.ParentBindingExpression),
					"PropertyChanged",
					TraceData.Identify(sender)
				}));
			}
			this._host.OnSourcePropertyChanged(sender, e.PropertyName);
		}

		// Token: 0x06007665 RID: 30309 RVA: 0x0021D5E4 File Offset: 0x0021B7E4
		private void OnValueChanged(object sender, ValueChangedEventArgs e)
		{
			if (this.IsExtendedTraceEnabled(TraceDataLevel.Transfer))
			{
				TraceData.Trace(TraceEventType.Warning, TraceData.GotEvent(new object[]
				{
					TraceData.Identify(this._host.ParentBindingExpression),
					"ValueChanged",
					TraceData.Identify(sender)
				}));
			}
			this._host.OnSourcePropertyChanged(sender, e.PropertyDescriptor.Name);
		}

		// Token: 0x06007666 RID: 30310 RVA: 0x0021D646 File Offset: 0x0021B846
		private void OnErrorsChanged(object sender, DataErrorsChangedEventArgs e)
		{
			if (e.PropertyName == this.SourcePropertyName)
			{
				this._host.OnDataErrorsChanged((INotifyDataErrorInfo)sender, e.PropertyName);
			}
		}

		// Token: 0x06007667 RID: 30311 RVA: 0x0021D674 File Offset: 0x0021B874
		private void OnStaticPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (this.IsExtendedTraceEnabled(TraceDataLevel.Transfer))
			{
				TraceData.Trace(TraceEventType.Warning, TraceData.GotEvent(new object[]
				{
					TraceData.Identify(this._host.ParentBindingExpression),
					"PropertyChanged",
					"(static)"
				}));
			}
			this._host.OnSourcePropertyChanged(sender, e.PropertyName);
		}

		// Token: 0x06007668 RID: 30312 RVA: 0x0021D6D0 File Offset: 0x0021B8D0
		private bool IsExtendedTraceEnabled(TraceDataLevel level)
		{
			return this._host != null && TraceData.IsExtendedTraceEnabled(this._host.ParentBindingExpression, level);
		}

		// Token: 0x04003873 RID: 14451
		private static readonly char[] s_comma = new char[]
		{
			','
		};

		// Token: 0x04003874 RID: 14452
		private static readonly char[] s_dot = new char[]
		{
			'.'
		};

		// Token: 0x04003875 RID: 14453
		private static readonly object NoParent = new NamedObject("NoParent");

		// Token: 0x04003876 RID: 14454
		private static readonly object AsyncRequestPending = new NamedObject("AsyncRequestPending");

		// Token: 0x04003877 RID: 14455
		internal static readonly object IListIndexOutOfRange = new NamedObject("IListIndexOutOfRange");

		// Token: 0x04003878 RID: 14456
		private static readonly IList<Type> IListIndexerWhitelist = new Type[]
		{
			typeof(ArrayList),
			typeof(IList),
			typeof(List<>),
			typeof(Collection<>),
			typeof(ReadOnlyCollection<>),
			typeof(StringCollection),
			typeof(LinkTargetCollection)
		};

		// Token: 0x04003879 RID: 14457
		private PropertyPath _parent;

		// Token: 0x0400387A RID: 14458
		private PropertyPathStatus _status;

		// Token: 0x0400387B RID: 14459
		private object _treeContext;

		// Token: 0x0400387C RID: 14460
		private object _rootItem;

		// Token: 0x0400387D RID: 14461
		private PropertyPathWorker.SourceValueState[] _arySVS;

		// Token: 0x0400387E RID: 14462
		private PropertyPathWorker.ContextHelper _contextHelper;

		// Token: 0x0400387F RID: 14463
		private ClrBindingWorker _host;

		// Token: 0x04003880 RID: 14464
		private DataBindEngine _engine;

		// Token: 0x04003881 RID: 14465
		private bool _dependencySourcesChanged;

		// Token: 0x04003882 RID: 14466
		private bool _isDynamic;

		// Token: 0x04003883 RID: 14467
		private bool _needsDirectNotification;

		// Token: 0x04003884 RID: 14468
		private bool? _isDBNullValidForUpdate;

		// Token: 0x02000B57 RID: 2903
		private class ContextHelper : IDisposable
		{
			// Token: 0x06008DC8 RID: 36296 RVA: 0x0025A407 File Offset: 0x00258607
			public ContextHelper(PropertyPathWorker owner)
			{
				this._owner = owner;
			}

			// Token: 0x06008DC9 RID: 36297 RVA: 0x0025A416 File Offset: 0x00258616
			public void SetContext(object rootItem)
			{
				this._owner.TreeContext = (rootItem as DependencyObject);
				this._owner.AttachToRootItem(rootItem);
			}

			// Token: 0x06008DCA RID: 36298 RVA: 0x0025A435 File Offset: 0x00258635
			void IDisposable.Dispose()
			{
				this._owner.DetachFromRootItem();
				this._owner.TreeContext = null;
				GC.SuppressFinalize(this);
			}

			// Token: 0x04004B05 RID: 19205
			private PropertyPathWorker _owner;
		}

		// Token: 0x02000B58 RID: 2904
		private class IListIndexerArg
		{
			// Token: 0x06008DCB RID: 36299 RVA: 0x0025A454 File Offset: 0x00258654
			public IListIndexerArg(int arg)
			{
				this._arg = arg;
			}

			// Token: 0x17001F84 RID: 8068
			// (get) Token: 0x06008DCC RID: 36300 RVA: 0x0025A463 File Offset: 0x00258663
			public int Value
			{
				get
				{
					return this._arg;
				}
			}

			// Token: 0x04004B06 RID: 19206
			private int _arg;
		}

		// Token: 0x02000B59 RID: 2905
		private struct SourceValueState
		{
			// Token: 0x04004B07 RID: 19207
			public ICollectionView collectionView;

			// Token: 0x04004B08 RID: 19208
			public object item;

			// Token: 0x04004B09 RID: 19209
			public object info;

			// Token: 0x04004B0A RID: 19210
			public Type type;

			// Token: 0x04004B0B RID: 19211
			public object[] args;
		}
	}
}
