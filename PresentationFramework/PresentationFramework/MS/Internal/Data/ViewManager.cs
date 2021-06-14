using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Data;

namespace MS.Internal.Data
{
	// Token: 0x02000749 RID: 1865
	internal class ViewManager : HybridDictionary
	{
		// Token: 0x17001C4A RID: 7242
		public CollectionRecord this[object o]
		{
			get
			{
				WeakRefKey weakRefKey = new WeakRefKey(o);
				return (CollectionRecord)base[weakRefKey];
			}
		}

		// Token: 0x060076FC RID: 30460 RVA: 0x0021FF04 File Offset: 0x0021E104
		internal void Add(object collection, CollectionRecord cr)
		{
			base.Add(new WeakRefKey(collection), cr);
			DataBindEngine.CurrentDataBindEngine.ScheduleCleanup();
		}

		// Token: 0x060076FD RID: 30461 RVA: 0x0021FF24 File Offset: 0x0021E124
		internal ViewRecord GetViewRecord(object collection, CollectionViewSource cvs, Type collectionViewType, bool createView, Func<object, object> GetSourceItem)
		{
			ViewRecord viewRecord = this.GetExistingView(collection, cvs, collectionViewType, GetSourceItem);
			if (viewRecord != null || !createView)
			{
				return viewRecord;
			}
			IListSource listSource = collection as IListSource;
			IList list = null;
			if (listSource != null)
			{
				list = listSource.GetList();
				viewRecord = this.GetExistingView(list, cvs, collectionViewType, GetSourceItem);
				if (viewRecord != null)
				{
					return this.CacheView(collection, cvs, (CollectionView)viewRecord.View, viewRecord);
				}
			}
			ICollectionView collectionView = collection as ICollectionView;
			if (collectionView != null)
			{
				collectionView = new CollectionViewProxy(collectionView);
			}
			else if (collectionViewType == null)
			{
				ICollectionViewFactory collectionViewFactory = collection as ICollectionViewFactory;
				if (collectionViewFactory != null)
				{
					collectionView = collectionViewFactory.CreateView();
				}
				else
				{
					IList list2 = (list != null) ? list : (collection as IList);
					if (list2 != null)
					{
						IBindingList bindingList = list2 as IBindingList;
						if (bindingList != null)
						{
							collectionView = new BindingListCollectionView(bindingList);
						}
						else
						{
							collectionView = new ListCollectionView(list2);
						}
					}
					else
					{
						IEnumerable enumerable = collection as IEnumerable;
						if (enumerable != null)
						{
							collectionView = new EnumerableCollectionView(enumerable);
						}
					}
				}
			}
			else
			{
				if (!typeof(ICollectionView).IsAssignableFrom(collectionViewType))
				{
					throw new ArgumentException(SR.Get("CollectionView_WrongType", new object[]
					{
						collectionViewType.Name
					}));
				}
				object obj = (list != null) ? list : collection;
				try
				{
					collectionView = (Activator.CreateInstance(collectionViewType, BindingFlags.CreateInstance, null, new object[]
					{
						obj
					}, null) as ICollectionView);
				}
				catch (MissingMethodException innerException)
				{
					throw new ArgumentException(SR.Get("CollectionView_ViewTypeInsufficient", new object[]
					{
						collectionViewType.Name,
						collection.GetType()
					}), innerException);
				}
			}
			if (collectionView != null)
			{
				CollectionView collectionView2 = collectionView as CollectionView;
				if (collectionView2 == null)
				{
					collectionView2 = new CollectionViewProxy(collectionView);
				}
				if (list != null)
				{
					viewRecord = this.CacheView(list, cvs, collectionView2, null);
				}
				viewRecord = this.CacheView(collection, cvs, collectionView2, viewRecord);
				BindingOperations.OnCollectionViewRegistering(collectionView2);
			}
			return viewRecord;
		}

		// Token: 0x060076FE RID: 30462 RVA: 0x002200DC File Offset: 0x0021E2DC
		private CollectionRecord EnsureCollectionRecord(object collection, Func<object, object> GetSourceItem = null)
		{
			CollectionRecord collectionRecord = this[collection];
			if (collectionRecord == null)
			{
				collectionRecord = new CollectionRecord();
				this.Add(collection, collectionRecord);
				object parent = (GetSourceItem != null) ? GetSourceItem(collection) : null;
				IEnumerable enumerable = collection as IEnumerable;
				if (enumerable != null)
				{
					BindingOperations.OnCollectionRegistering(enumerable, parent);
				}
			}
			return collectionRecord;
		}

		// Token: 0x060076FF RID: 30463 RVA: 0x00220124 File Offset: 0x0021E324
		internal void RegisterCollectionSynchronizationCallback(IEnumerable collection, object context, CollectionSynchronizationCallback synchronizationCallback)
		{
			CollectionRecord collectionRecord = this.EnsureCollectionRecord(collection, null);
			collectionRecord.SynchronizationInfo = new SynchronizationInfo(context, synchronizationCallback);
			ViewTable viewTable = collectionRecord.ViewTable;
			if (viewTable != null)
			{
				bool isSynchronized = collectionRecord.SynchronizationInfo.IsSynchronized;
				foreach (object obj in viewTable)
				{
					ViewRecord viewRecord = (ViewRecord)((DictionaryEntry)obj).Value;
					CollectionView collectionView = viewRecord.View as CollectionView;
					if (collectionView != null)
					{
						collectionView.SetAllowsCrossThreadChanges(isSynchronized);
					}
				}
			}
		}

		// Token: 0x06007700 RID: 30464 RVA: 0x002201CC File Offset: 0x0021E3CC
		internal SynchronizationInfo GetSynchronizationInfo(IEnumerable collection)
		{
			CollectionRecord collectionRecord = this[collection];
			if (collectionRecord == null)
			{
				return SynchronizationInfo.None;
			}
			return collectionRecord.SynchronizationInfo;
		}

		// Token: 0x06007701 RID: 30465 RVA: 0x002201F0 File Offset: 0x0021E3F0
		public void AccessCollection(IEnumerable collection, Action accessMethod, bool writeAccess)
		{
			this.GetSynchronizationInfo(collection).AccessCollection(collection, accessMethod, writeAccess);
		}

		// Token: 0x17001C4B RID: 7243
		// (get) Token: 0x06007702 RID: 30466 RVA: 0x0022020F File Offset: 0x0021E40F
		internal static ViewManager Current
		{
			get
			{
				return DataBindEngine.CurrentDataBindEngine.ViewManager;
			}
		}

		// Token: 0x06007703 RID: 30467 RVA: 0x0022021C File Offset: 0x0021E41C
		private ViewRecord GetExistingView(object collection, CollectionViewSource cvs, Type collectionViewType, Func<object, object> GetSourceItem)
		{
			CollectionView collectionView = collection as CollectionView;
			ViewRecord result;
			if (collectionView == null)
			{
				CollectionRecord collectionRecord = this.EnsureCollectionRecord(collection, GetSourceItem);
				ViewTable viewTable = collectionRecord.ViewTable;
				if (viewTable != null)
				{
					ViewRecord viewRecord = viewTable[cvs];
					if (viewRecord != null)
					{
						collectionView = (CollectionView)viewRecord.View;
					}
					result = viewRecord;
					if (this._inactiveViewTables.Contains(viewTable))
					{
						this._inactiveViewTables[viewTable] = 2;
					}
				}
				else
				{
					result = null;
				}
			}
			else
			{
				result = new ViewRecord(collectionView);
			}
			if (collectionView != null)
			{
				this.ValidateViewType(collectionView, collectionViewType);
			}
			return result;
		}

		// Token: 0x06007704 RID: 30468 RVA: 0x002202A0 File Offset: 0x0021E4A0
		private ViewRecord CacheView(object collection, CollectionViewSource cvs, CollectionView cv, ViewRecord vr)
		{
			CollectionRecord collectionRecord = this[collection];
			ViewTable viewTable = collectionRecord.ViewTable;
			if (viewTable == null)
			{
				viewTable = new ViewTable();
				collectionRecord.ViewTable = viewTable;
				if (!(collection is INotifyCollectionChanged))
				{
					this._inactiveViewTables.Add(viewTable, 2);
				}
			}
			if (vr == null)
			{
				vr = new ViewRecord(cv);
			}
			else if (cv == null)
			{
				cv = (CollectionView)vr.View;
			}
			cv.SetViewManagerData(viewTable);
			viewTable[cvs] = vr;
			return vr;
		}

		// Token: 0x06007705 RID: 30469 RVA: 0x00220318 File Offset: 0x0021E518
		internal bool Purge()
		{
			int count = this._inactiveViewTables.Count;
			if (count > 0)
			{
				ViewTable[] array = new ViewTable[count];
				this._inactiveViewTables.Keys.CopyTo(array, 0);
				for (int i = 0; i < count; i++)
				{
					ViewTable key = array[i];
					int num = (int)this._inactiveViewTables[key];
					if (--num > 0)
					{
						this._inactiveViewTables[key] = num;
					}
					else
					{
						this._inactiveViewTables.Remove(key);
					}
				}
			}
			ArrayList arrayList = new ArrayList();
			bool flag = false;
			foreach (object obj in this)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				WeakRefKey weakRefKey = (WeakRefKey)dictionaryEntry.Key;
				CollectionRecord collectionRecord = (CollectionRecord)dictionaryEntry.Value;
				if (weakRefKey.Target == null || !collectionRecord.IsAlive)
				{
					arrayList.Add(weakRefKey);
				}
				else
				{
					ViewTable viewTable = collectionRecord.ViewTable;
					if (viewTable != null && viewTable.Purge())
					{
						flag = true;
						if (viewTable.Count == 0)
						{
							collectionRecord.ViewTable = null;
							if (!collectionRecord.IsAlive)
							{
								arrayList.Add(weakRefKey);
							}
						}
					}
				}
			}
			for (int j = 0; j < arrayList.Count; j++)
			{
				base.Remove(arrayList[j]);
			}
			return arrayList.Count > 0 || flag;
		}

		// Token: 0x06007706 RID: 30470 RVA: 0x002204A8 File Offset: 0x0021E6A8
		private void ValidateViewType(CollectionView cv, Type collectionViewType)
		{
			if (collectionViewType != null)
			{
				CollectionViewProxy collectionViewProxy = cv as CollectionViewProxy;
				Type type = (collectionViewProxy == null) ? cv.GetType() : collectionViewProxy.ProxiedView.GetType();
				if (type != collectionViewType)
				{
					throw new ArgumentException(SR.Get("CollectionView_NameTypeDuplicity", new object[]
					{
						collectionViewType,
						type
					}));
				}
			}
		}

		// Token: 0x040038A5 RID: 14501
		private const int InactivityThreshold = 2;

		// Token: 0x040038A6 RID: 14502
		private HybridDictionary _inactiveViewTables = new HybridDictionary();

		// Token: 0x040038A7 RID: 14503
		private static object StaticObject = new object();

		// Token: 0x040038A8 RID: 14504
		internal static WeakReference StaticWeakRef = new WeakReference(ViewManager.StaticObject);

		// Token: 0x040038A9 RID: 14505
		internal static WeakReference NullWeakRef = new WeakReference(null);
	}
}
