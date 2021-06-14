using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Reflection;
using System.Windows;

namespace MS.Internal.Data
{
	// Token: 0x02000743 RID: 1859
	internal class StaticPropertyChangedEventManager : WeakEventManager
	{
		// Token: 0x060076D6 RID: 30422 RVA: 0x0001737C File Offset: 0x0001557C
		private StaticPropertyChangedEventManager()
		{
		}

		// Token: 0x060076D7 RID: 30423 RVA: 0x0021F880 File Offset: 0x0021DA80
		public static void AddHandler(Type type, EventHandler<PropertyChangedEventArgs> handler, string propertyName)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (handler == null)
			{
				throw new ArgumentNullException("handler");
			}
			StaticPropertyChangedEventManager.CurrentManager.PrivateAddHandler(type, handler, propertyName);
		}

		// Token: 0x060076D8 RID: 30424 RVA: 0x0021F8B1 File Offset: 0x0021DAB1
		public static void RemoveHandler(Type type, EventHandler<PropertyChangedEventArgs> handler, string propertyName)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (handler == null)
			{
				throw new ArgumentNullException("handler");
			}
			StaticPropertyChangedEventManager.CurrentManager.PrivateRemoveHandler(type, handler, propertyName);
		}

		// Token: 0x060076D9 RID: 30425 RVA: 0x0021F8E2 File Offset: 0x0021DAE2
		protected override WeakEventManager.ListenerList NewListenerList()
		{
			return new WeakEventManager.ListenerList<PropertyChangedEventArgs>();
		}

		// Token: 0x060076DA RID: 30426 RVA: 0x00002137 File Offset: 0x00000337
		protected override void StartListening(object source)
		{
		}

		// Token: 0x060076DB RID: 30427 RVA: 0x00002137 File Offset: 0x00000337
		protected override void StopListening(object source)
		{
		}

		// Token: 0x060076DC RID: 30428 RVA: 0x0021F8EC File Offset: 0x0021DAEC
		protected override bool Purge(object source, object data, bool purgeAll)
		{
			StaticPropertyChangedEventManager.TypeRecord typeRecord = (StaticPropertyChangedEventManager.TypeRecord)data;
			bool result = typeRecord.Purge(purgeAll);
			if (!purgeAll && typeRecord.IsEmpty)
			{
				base.Remove(typeRecord.Type);
			}
			return result;
		}

		// Token: 0x17001C40 RID: 7232
		// (get) Token: 0x060076DD RID: 30429 RVA: 0x0021F920 File Offset: 0x0021DB20
		private static StaticPropertyChangedEventManager CurrentManager
		{
			get
			{
				Type typeFromHandle = typeof(StaticPropertyChangedEventManager);
				StaticPropertyChangedEventManager staticPropertyChangedEventManager = (StaticPropertyChangedEventManager)WeakEventManager.GetCurrentManager(typeFromHandle);
				if (staticPropertyChangedEventManager == null)
				{
					staticPropertyChangedEventManager = new StaticPropertyChangedEventManager();
					WeakEventManager.SetCurrentManager(typeFromHandle, staticPropertyChangedEventManager);
				}
				return staticPropertyChangedEventManager;
			}
		}

		// Token: 0x060076DE RID: 30430 RVA: 0x0021F958 File Offset: 0x0021DB58
		private void PrivateAddHandler(Type type, EventHandler<PropertyChangedEventArgs> handler, string propertyName)
		{
			using (base.WriteLock)
			{
				StaticPropertyChangedEventManager.TypeRecord typeRecord = (StaticPropertyChangedEventManager.TypeRecord)base[type];
				if (typeRecord == null)
				{
					typeRecord = new StaticPropertyChangedEventManager.TypeRecord(type, this);
					base[type] = typeRecord;
					typeRecord.StartListening();
				}
				typeRecord.AddHandler(handler, propertyName);
			}
		}

		// Token: 0x060076DF RID: 30431 RVA: 0x0021F9B8 File Offset: 0x0021DBB8
		private void PrivateRemoveHandler(Type type, EventHandler<PropertyChangedEventArgs> handler, string propertyName)
		{
			using (base.WriteLock)
			{
				StaticPropertyChangedEventManager.TypeRecord typeRecord = (StaticPropertyChangedEventManager.TypeRecord)base[type];
				if (typeRecord != null)
				{
					typeRecord.RemoveHandler(handler, propertyName);
					if (typeRecord.IsEmpty)
					{
						typeRecord.StopListening();
						base.Remove(typeRecord.Type);
					}
				}
			}
		}

		// Token: 0x060076E0 RID: 30432 RVA: 0x0021FA1C File Offset: 0x0021DC1C
		private void OnStaticPropertyChanged(StaticPropertyChangedEventManager.TypeRecord typeRecord, PropertyChangedEventArgs args)
		{
			WeakEventManager.ListenerList listenerList;
			using (base.ReadLock)
			{
				listenerList = typeRecord.GetListenerList(args.PropertyName);
				listenerList.BeginUse();
			}
			try
			{
				base.DeliverEventToList(null, args, listenerList);
			}
			finally
			{
				listenerList.EndUse();
			}
			if (listenerList == typeRecord.ProposedAllListenersList)
			{
				using (base.WriteLock)
				{
					typeRecord.StoreAllListenersList((WeakEventManager.ListenerList<PropertyChangedEventArgs>)listenerList);
				}
			}
		}

		// Token: 0x04003898 RID: 14488
		private static readonly string AllListenersKey = "<All Listeners>";

		// Token: 0x04003899 RID: 14489
		private static readonly string StaticPropertyChanged = "StaticPropertyChanged";

		// Token: 0x02000B62 RID: 2914
		private class TypeRecord
		{
			// Token: 0x06008DE8 RID: 36328 RVA: 0x0025A784 File Offset: 0x00258984
			public TypeRecord(Type type, StaticPropertyChangedEventManager manager)
			{
				this._type = type;
				this._manager = manager;
				this._dict = new HybridDictionary(true);
			}

			// Token: 0x17001F8B RID: 8075
			// (get) Token: 0x06008DE9 RID: 36329 RVA: 0x0025A7B1 File Offset: 0x002589B1
			public Type Type
			{
				get
				{
					return this._type;
				}
			}

			// Token: 0x17001F8C RID: 8076
			// (get) Token: 0x06008DEA RID: 36330 RVA: 0x0025A7B9 File Offset: 0x002589B9
			public bool IsEmpty
			{
				get
				{
					return this._dict.Count == 0;
				}
			}

			// Token: 0x17001F8D RID: 8077
			// (get) Token: 0x06008DEB RID: 36331 RVA: 0x0025A7C9 File Offset: 0x002589C9
			public WeakEventManager.ListenerList ProposedAllListenersList
			{
				get
				{
					return this._proposedAllListenersList;
				}
			}

			// Token: 0x17001F8E RID: 8078
			// (get) Token: 0x06008DEC RID: 36332 RVA: 0x0025A7D1 File Offset: 0x002589D1
			private static MethodInfo OnStaticPropertyChangedMethodInfo
			{
				get
				{
					return typeof(StaticPropertyChangedEventManager.TypeRecord).GetMethod("OnStaticPropertyChanged", BindingFlags.Instance | BindingFlags.NonPublic);
				}
			}

			// Token: 0x06008DED RID: 36333 RVA: 0x0025A7EC File Offset: 0x002589EC
			public void StartListening()
			{
				EventInfo @event = this._type.GetEvent(StaticPropertyChangedEventManager.StaticPropertyChanged, BindingFlags.Static | BindingFlags.Public);
				if (@event != null)
				{
					Delegate handler = Delegate.CreateDelegate(@event.EventHandlerType, this, StaticPropertyChangedEventManager.TypeRecord.OnStaticPropertyChangedMethodInfo);
					@event.AddEventHandler(null, handler);
				}
			}

			// Token: 0x06008DEE RID: 36334 RVA: 0x0025A830 File Offset: 0x00258A30
			public void StopListening()
			{
				EventInfo @event = this._type.GetEvent(StaticPropertyChangedEventManager.StaticPropertyChanged, BindingFlags.Static | BindingFlags.Public);
				if (@event != null)
				{
					Delegate handler = Delegate.CreateDelegate(@event.EventHandlerType, this, StaticPropertyChangedEventManager.TypeRecord.OnStaticPropertyChangedMethodInfo);
					@event.RemoveEventHandler(null, handler);
				}
			}

			// Token: 0x06008DEF RID: 36335 RVA: 0x0025A873 File Offset: 0x00258A73
			private void OnStaticPropertyChanged(object sender, PropertyChangedEventArgs e)
			{
				this.HandleStaticPropertyChanged(e);
			}

			// Token: 0x06008DF0 RID: 36336 RVA: 0x0025A87C File Offset: 0x00258A7C
			public void HandleStaticPropertyChanged(PropertyChangedEventArgs e)
			{
				this._manager.OnStaticPropertyChanged(this, e);
			}

			// Token: 0x06008DF1 RID: 36337 RVA: 0x0025A88C File Offset: 0x00258A8C
			public void AddHandler(EventHandler<PropertyChangedEventArgs> handler, string propertyName)
			{
				StaticPropertyChangedEventManager.PropertyRecord propertyRecord = (StaticPropertyChangedEventManager.PropertyRecord)this._dict[propertyName];
				if (propertyRecord == null)
				{
					propertyRecord = new StaticPropertyChangedEventManager.PropertyRecord(propertyName, this);
					this._dict[propertyName] = propertyRecord;
					propertyRecord.StartListening(this._type);
				}
				propertyRecord.AddHandler(handler);
				this._dict.Remove(StaticPropertyChangedEventManager.AllListenersKey);
				this._proposedAllListenersList = null;
				this._manager.ScheduleCleanup();
			}

			// Token: 0x06008DF2 RID: 36338 RVA: 0x0025A8F8 File Offset: 0x00258AF8
			public void RemoveHandler(EventHandler<PropertyChangedEventArgs> handler, string propertyName)
			{
				StaticPropertyChangedEventManager.PropertyRecord propertyRecord = (StaticPropertyChangedEventManager.PropertyRecord)this._dict[propertyName];
				if (propertyRecord != null)
				{
					propertyRecord.RemoveHandler(handler);
					if (propertyRecord.IsEmpty)
					{
						this._dict.Remove(propertyName);
					}
					this._dict.Remove(StaticPropertyChangedEventManager.AllListenersKey);
					this._proposedAllListenersList = null;
				}
			}

			// Token: 0x06008DF3 RID: 36339 RVA: 0x0025A94C File Offset: 0x00258B4C
			public WeakEventManager.ListenerList GetListenerList(string propertyName)
			{
				WeakEventManager.ListenerList listenerList3;
				if (!string.IsNullOrEmpty(propertyName))
				{
					StaticPropertyChangedEventManager.PropertyRecord propertyRecord = (StaticPropertyChangedEventManager.PropertyRecord)this._dict[propertyName];
					WeakEventManager.ListenerList<PropertyChangedEventArgs> listenerList = (propertyRecord == null) ? null : propertyRecord.List;
					StaticPropertyChangedEventManager.PropertyRecord propertyRecord2 = (StaticPropertyChangedEventManager.PropertyRecord)this._dict[string.Empty];
					WeakEventManager.ListenerList<PropertyChangedEventArgs> listenerList2 = (propertyRecord2 == null) ? null : propertyRecord2.List;
					if (listenerList2 == null)
					{
						if (listenerList != null)
						{
							listenerList3 = listenerList;
						}
						else
						{
							listenerList3 = WeakEventManager.ListenerList.Empty;
						}
					}
					else if (listenerList != null)
					{
						listenerList3 = new WeakEventManager.ListenerList<PropertyChangedEventArgs>(listenerList.Count + listenerList2.Count);
						int i = 0;
						int count = listenerList.Count;
						while (i < count)
						{
							listenerList3.Add(listenerList[i]);
							i++;
						}
						int j = 0;
						int count2 = listenerList2.Count;
						while (j < count2)
						{
							listenerList3.Add(listenerList2[j]);
							j++;
						}
					}
					else
					{
						listenerList3 = listenerList2;
					}
				}
				else
				{
					StaticPropertyChangedEventManager.PropertyRecord propertyRecord3 = (StaticPropertyChangedEventManager.PropertyRecord)this._dict[StaticPropertyChangedEventManager.AllListenersKey];
					WeakEventManager.ListenerList<PropertyChangedEventArgs> listenerList4 = (propertyRecord3 == null) ? null : propertyRecord3.List;
					if (listenerList4 == null)
					{
						int num = 0;
						foreach (object obj in this._dict)
						{
							num += ((StaticPropertyChangedEventManager.PropertyRecord)((DictionaryEntry)obj).Value).List.Count;
						}
						listenerList4 = new WeakEventManager.ListenerList<PropertyChangedEventArgs>(num);
						foreach (object obj2 in this._dict)
						{
							WeakEventManager.ListenerList list = ((StaticPropertyChangedEventManager.PropertyRecord)((DictionaryEntry)obj2).Value).List;
							int k = 0;
							int count3 = list.Count;
							while (k < count3)
							{
								listenerList4.Add(list.GetListener(k));
								k++;
							}
						}
						this._proposedAllListenersList = listenerList4;
					}
					listenerList3 = listenerList4;
				}
				return listenerList3;
			}

			// Token: 0x06008DF4 RID: 36340 RVA: 0x0025AB68 File Offset: 0x00258D68
			public void StoreAllListenersList(WeakEventManager.ListenerList<PropertyChangedEventArgs> list)
			{
				if (this._proposedAllListenersList == list)
				{
					this._dict[StaticPropertyChangedEventManager.AllListenersKey] = new StaticPropertyChangedEventManager.PropertyRecord(StaticPropertyChangedEventManager.AllListenersKey, this, list);
					this._proposedAllListenersList = null;
				}
			}

			// Token: 0x06008DF5 RID: 36341 RVA: 0x0025AB98 File Offset: 0x00258D98
			public bool Purge(bool purgeAll)
			{
				bool flag = false;
				if (!purgeAll)
				{
					if (!BaseAppContextSwitches.EnableWeakEventMemoryImprovements)
					{
						ICollection keys = this._dict.Keys;
						string[] array = new string[keys.Count];
						keys.CopyTo(array, 0);
						for (int i = array.Length - 1; i >= 0; i--)
						{
							if (!(array[i] == StaticPropertyChangedEventManager.AllListenersKey))
							{
								StaticPropertyChangedEventManager.PropertyRecord propertyRecord = (StaticPropertyChangedEventManager.PropertyRecord)this._dict[array[i]];
								if (propertyRecord.Purge())
								{
									flag = true;
								}
								if (propertyRecord.IsEmpty)
								{
									propertyRecord.StopListening(this._type);
									this._dict.Remove(array[i]);
								}
							}
						}
					}
					else
					{
						IDictionaryEnumerator enumerator = this._dict.GetEnumerator();
						while (enumerator.MoveNext())
						{
							string text = (string)enumerator.Key;
							if (!(text == StaticPropertyChangedEventManager.AllListenersKey))
							{
								StaticPropertyChangedEventManager.PropertyRecord propertyRecord2 = (StaticPropertyChangedEventManager.PropertyRecord)enumerator.Value;
								if (propertyRecord2.Purge())
								{
									flag = true;
								}
								if (propertyRecord2.IsEmpty)
								{
									propertyRecord2.StopListening(this._type);
									this._toRemove.Add(text);
								}
							}
						}
						if (this._toRemove.Count > 0)
						{
							foreach (string key in this._toRemove)
							{
								this._dict.Remove(key);
							}
							this._toRemove.Clear();
							this._toRemove.TrimExcess();
						}
					}
					if (flag)
					{
						this._dict.Remove(StaticPropertyChangedEventManager.AllListenersKey);
						this._proposedAllListenersList = null;
					}
					if (this.IsEmpty)
					{
						this.StopListening();
					}
				}
				else
				{
					flag = true;
					this.StopListening();
					foreach (object obj in this._dict)
					{
						StaticPropertyChangedEventManager.PropertyRecord propertyRecord3 = (StaticPropertyChangedEventManager.PropertyRecord)((DictionaryEntry)obj).Value;
						propertyRecord3.StopListening(this._type);
					}
				}
				return flag;
			}

			// Token: 0x04004B21 RID: 19233
			private Type _type;

			// Token: 0x04004B22 RID: 19234
			private HybridDictionary _dict;

			// Token: 0x04004B23 RID: 19235
			private StaticPropertyChangedEventManager _manager;

			// Token: 0x04004B24 RID: 19236
			private WeakEventManager.ListenerList<PropertyChangedEventArgs> _proposedAllListenersList;

			// Token: 0x04004B25 RID: 19237
			private List<string> _toRemove = new List<string>();
		}

		// Token: 0x02000B63 RID: 2915
		private class PropertyRecord
		{
			// Token: 0x06008DF6 RID: 36342 RVA: 0x0025ADB8 File Offset: 0x00258FB8
			public PropertyRecord(string propertyName, StaticPropertyChangedEventManager.TypeRecord owner) : this(propertyName, owner, new WeakEventManager.ListenerList<PropertyChangedEventArgs>())
			{
			}

			// Token: 0x06008DF7 RID: 36343 RVA: 0x0025ADC7 File Offset: 0x00258FC7
			public PropertyRecord(string propertyName, StaticPropertyChangedEventManager.TypeRecord owner, WeakEventManager.ListenerList<PropertyChangedEventArgs> list)
			{
				this._propertyName = propertyName;
				this._typeRecord = owner;
				this._list = list;
			}

			// Token: 0x17001F8F RID: 8079
			// (get) Token: 0x06008DF8 RID: 36344 RVA: 0x0025ADE4 File Offset: 0x00258FE4
			public bool IsEmpty
			{
				get
				{
					return this._list.IsEmpty;
				}
			}

			// Token: 0x17001F90 RID: 8080
			// (get) Token: 0x06008DF9 RID: 36345 RVA: 0x0025ADF1 File Offset: 0x00258FF1
			public WeakEventManager.ListenerList<PropertyChangedEventArgs> List
			{
				get
				{
					return this._list;
				}
			}

			// Token: 0x17001F91 RID: 8081
			// (get) Token: 0x06008DFA RID: 36346 RVA: 0x0025ADF9 File Offset: 0x00258FF9
			private static MethodInfo OnStaticPropertyChangedMethodInfo
			{
				get
				{
					return typeof(StaticPropertyChangedEventManager.PropertyRecord).GetMethod("OnStaticPropertyChanged", BindingFlags.Instance | BindingFlags.NonPublic);
				}
			}

			// Token: 0x06008DFB RID: 36347 RVA: 0x0025AE14 File Offset: 0x00259014
			public void StartListening(Type type)
			{
				string name = this._propertyName + "Changed";
				EventInfo @event = type.GetEvent(name, BindingFlags.Static | BindingFlags.Public);
				if (@event != null)
				{
					Delegate handler = Delegate.CreateDelegate(@event.EventHandlerType, this, StaticPropertyChangedEventManager.PropertyRecord.OnStaticPropertyChangedMethodInfo);
					@event.AddEventHandler(null, handler);
				}
			}

			// Token: 0x06008DFC RID: 36348 RVA: 0x0025AE60 File Offset: 0x00259060
			public void StopListening(Type type)
			{
				string name = this._propertyName + "Changed";
				EventInfo @event = type.GetEvent(name, BindingFlags.Static | BindingFlags.Public);
				if (@event != null)
				{
					Delegate handler = Delegate.CreateDelegate(@event.EventHandlerType, this, StaticPropertyChangedEventManager.PropertyRecord.OnStaticPropertyChangedMethodInfo);
					@event.RemoveEventHandler(null, handler);
				}
			}

			// Token: 0x06008DFD RID: 36349 RVA: 0x0025AEAB File Offset: 0x002590AB
			private void OnStaticPropertyChanged(object sender, EventArgs e)
			{
				this._typeRecord.HandleStaticPropertyChanged(new PropertyChangedEventArgs(this._propertyName));
			}

			// Token: 0x06008DFE RID: 36350 RVA: 0x0025AEC4 File Offset: 0x002590C4
			public void AddHandler(EventHandler<PropertyChangedEventArgs> handler)
			{
				WeakEventManager.ListenerList list = this._list;
				if (WeakEventManager.ListenerList.PrepareForWriting(ref list))
				{
					this._list = (WeakEventManager.ListenerList<PropertyChangedEventArgs>)list;
				}
				this._list.AddHandler(handler);
			}

			// Token: 0x06008DFF RID: 36351 RVA: 0x0025AEFC File Offset: 0x002590FC
			public void RemoveHandler(EventHandler<PropertyChangedEventArgs> handler)
			{
				WeakEventManager.ListenerList list = this._list;
				if (WeakEventManager.ListenerList.PrepareForWriting(ref list))
				{
					this._list = (WeakEventManager.ListenerList<PropertyChangedEventArgs>)list;
				}
				this._list.RemoveHandler(handler);
			}

			// Token: 0x06008E00 RID: 36352 RVA: 0x0025AF34 File Offset: 0x00259134
			public bool Purge()
			{
				WeakEventManager.ListenerList list = this._list;
				if (WeakEventManager.ListenerList.PrepareForWriting(ref list))
				{
					this._list = (WeakEventManager.ListenerList<PropertyChangedEventArgs>)list;
				}
				return this._list.Purge();
			}

			// Token: 0x04004B26 RID: 19238
			private string _propertyName;

			// Token: 0x04004B27 RID: 19239
			private WeakEventManager.ListenerList<PropertyChangedEventArgs> _list;

			// Token: 0x04004B28 RID: 19240
			private StaticPropertyChangedEventManager.TypeRecord _typeRecord;
		}
	}
}
