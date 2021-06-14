using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;

namespace MS.Internal.Data
{
	// Token: 0x0200074A RID: 1866
	internal class ValueChangedEventManager : WeakEventManager
	{
		// Token: 0x06007709 RID: 30473 RVA: 0x0022053C File Offset: 0x0021E73C
		private ValueChangedEventManager()
		{
		}

		// Token: 0x0600770A RID: 30474 RVA: 0x0022054F File Offset: 0x0021E74F
		public static void AddListener(object source, IWeakEventListener listener, PropertyDescriptor pd)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (listener == null)
			{
				throw new ArgumentNullException("listener");
			}
			ValueChangedEventManager.CurrentManager.PrivateAddListener(source, listener, pd);
		}

		// Token: 0x0600770B RID: 30475 RVA: 0x0022057A File Offset: 0x0021E77A
		public static void RemoveListener(object source, IWeakEventListener listener, PropertyDescriptor pd)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (listener == null)
			{
				throw new ArgumentNullException("listener");
			}
			ValueChangedEventManager.CurrentManager.PrivateRemoveListener(source, listener, pd);
		}

		// Token: 0x0600770C RID: 30476 RVA: 0x002205A5 File Offset: 0x0021E7A5
		public static void AddHandler(object source, EventHandler<ValueChangedEventArgs> handler, PropertyDescriptor pd)
		{
			if (handler == null)
			{
				throw new ArgumentNullException("handler");
			}
			if (handler.GetInvocationList().Length != 1)
			{
				throw new NotSupportedException(SR.Get("NoMulticastHandlers"));
			}
			ValueChangedEventManager.CurrentManager.PrivateAddHandler(source, handler, pd);
		}

		// Token: 0x0600770D RID: 30477 RVA: 0x002205DD File Offset: 0x0021E7DD
		public static void RemoveHandler(object source, EventHandler<ValueChangedEventArgs> handler, PropertyDescriptor pd)
		{
			if (handler == null)
			{
				throw new ArgumentNullException("handler");
			}
			if (handler.GetInvocationList().Length != 1)
			{
				throw new NotSupportedException(SR.Get("NoMulticastHandlers"));
			}
			ValueChangedEventManager.CurrentManager.PrivateRemoveHandler(source, handler, pd);
		}

		// Token: 0x0600770E RID: 30478 RVA: 0x00220615 File Offset: 0x0021E815
		protected override WeakEventManager.ListenerList NewListenerList()
		{
			return new WeakEventManager.ListenerList<ValueChangedEventArgs>();
		}

		// Token: 0x0600770F RID: 30479 RVA: 0x00002137 File Offset: 0x00000337
		protected override void StartListening(object source)
		{
		}

		// Token: 0x06007710 RID: 30480 RVA: 0x00002137 File Offset: 0x00000337
		protected override void StopListening(object source)
		{
		}

		// Token: 0x06007711 RID: 30481 RVA: 0x0022061C File Offset: 0x0021E81C
		protected override bool Purge(object source, object data, bool purgeAll)
		{
			bool result = false;
			HybridDictionary hybridDictionary = (HybridDictionary)data;
			if (!BaseAppContextSwitches.EnableWeakEventMemoryImprovements)
			{
				ICollection keys = hybridDictionary.Keys;
				PropertyDescriptor[] array = new PropertyDescriptor[keys.Count];
				keys.CopyTo(array, 0);
				for (int i = array.Length - 1; i >= 0; i--)
				{
					bool flag = purgeAll || source == null;
					ValueChangedEventManager.ValueChangedRecord valueChangedRecord = (ValueChangedEventManager.ValueChangedRecord)hybridDictionary[array[i]];
					if (!flag)
					{
						if (valueChangedRecord.Purge())
						{
							result = true;
						}
						flag = valueChangedRecord.IsEmpty;
					}
					if (flag)
					{
						valueChangedRecord.StopListening();
						if (!purgeAll)
						{
							hybridDictionary.Remove(array[i]);
						}
					}
				}
			}
			else
			{
				IDictionaryEnumerator enumerator = hybridDictionary.GetEnumerator();
				while (enumerator.MoveNext())
				{
					bool flag2 = purgeAll || source == null;
					ValueChangedEventManager.ValueChangedRecord valueChangedRecord2 = (ValueChangedEventManager.ValueChangedRecord)enumerator.Value;
					if (!flag2)
					{
						if (valueChangedRecord2.Purge())
						{
							result = true;
						}
						flag2 = valueChangedRecord2.IsEmpty;
					}
					if (flag2)
					{
						valueChangedRecord2.StopListening();
						if (!purgeAll)
						{
							this._toRemove.Add((PropertyDescriptor)enumerator.Key);
						}
					}
				}
				if (this._toRemove.Count > 0)
				{
					foreach (PropertyDescriptor key in this._toRemove)
					{
						hybridDictionary.Remove(key);
					}
					this._toRemove.Clear();
					this._toRemove.TrimExcess();
				}
			}
			if (hybridDictionary.Count == 0)
			{
				result = true;
				if (source != null)
				{
					base.Remove(source);
				}
			}
			return result;
		}

		// Token: 0x17001C4C RID: 7244
		// (get) Token: 0x06007712 RID: 30482 RVA: 0x002207AC File Offset: 0x0021E9AC
		private static ValueChangedEventManager CurrentManager
		{
			get
			{
				Type typeFromHandle = typeof(ValueChangedEventManager);
				ValueChangedEventManager valueChangedEventManager = (ValueChangedEventManager)WeakEventManager.GetCurrentManager(typeFromHandle);
				if (valueChangedEventManager == null)
				{
					valueChangedEventManager = new ValueChangedEventManager();
					WeakEventManager.SetCurrentManager(typeFromHandle, valueChangedEventManager);
				}
				return valueChangedEventManager;
			}
		}

		// Token: 0x06007713 RID: 30483 RVA: 0x002207E1 File Offset: 0x0021E9E1
		private void PrivateAddListener(object source, IWeakEventListener listener, PropertyDescriptor pd)
		{
			this.AddListener(source, pd, listener, null);
		}

		// Token: 0x06007714 RID: 30484 RVA: 0x002207ED File Offset: 0x0021E9ED
		private void PrivateRemoveListener(object source, IWeakEventListener listener, PropertyDescriptor pd)
		{
			this.RemoveListener(source, pd, listener, null);
		}

		// Token: 0x06007715 RID: 30485 RVA: 0x002207F9 File Offset: 0x0021E9F9
		private void PrivateAddHandler(object source, EventHandler<ValueChangedEventArgs> handler, PropertyDescriptor pd)
		{
			this.AddListener(source, pd, null, handler);
		}

		// Token: 0x06007716 RID: 30486 RVA: 0x00220805 File Offset: 0x0021EA05
		private void PrivateRemoveHandler(object source, EventHandler<ValueChangedEventArgs> handler, PropertyDescriptor pd)
		{
			this.RemoveListener(source, pd, null, handler);
		}

		// Token: 0x06007717 RID: 30487 RVA: 0x00220814 File Offset: 0x0021EA14
		private void AddListener(object source, PropertyDescriptor pd, IWeakEventListener listener, EventHandler<ValueChangedEventArgs> handler)
		{
			using (base.WriteLock)
			{
				HybridDictionary hybridDictionary = (HybridDictionary)base[source];
				if (hybridDictionary == null)
				{
					hybridDictionary = new HybridDictionary();
					base[source] = hybridDictionary;
				}
				ValueChangedEventManager.ValueChangedRecord valueChangedRecord = (ValueChangedEventManager.ValueChangedRecord)hybridDictionary[pd];
				if (valueChangedRecord == null)
				{
					valueChangedRecord = new ValueChangedEventManager.ValueChangedRecord(this, source, pd);
					hybridDictionary[pd] = valueChangedRecord;
				}
				valueChangedRecord.Add(listener, handler);
				base.ScheduleCleanup();
			}
		}

		// Token: 0x06007718 RID: 30488 RVA: 0x00220894 File Offset: 0x0021EA94
		private void RemoveListener(object source, PropertyDescriptor pd, IWeakEventListener listener, EventHandler<ValueChangedEventArgs> handler)
		{
			using (base.WriteLock)
			{
				HybridDictionary hybridDictionary = (HybridDictionary)base[source];
				if (hybridDictionary != null)
				{
					ValueChangedEventManager.ValueChangedRecord valueChangedRecord = (ValueChangedEventManager.ValueChangedRecord)hybridDictionary[pd];
					if (valueChangedRecord != null)
					{
						valueChangedRecord.Remove(listener, handler);
						if (valueChangedRecord.IsEmpty)
						{
							hybridDictionary.Remove(pd);
						}
					}
					if (hybridDictionary.Count == 0)
					{
						base.Remove(source);
					}
				}
			}
		}

		// Token: 0x040038AA RID: 14506
		private List<PropertyDescriptor> _toRemove = new List<PropertyDescriptor>();

		// Token: 0x02000B64 RID: 2916
		private class ValueChangedRecord
		{
			// Token: 0x06008E01 RID: 36353 RVA: 0x0025AF68 File Offset: 0x00259168
			public ValueChangedRecord(ValueChangedEventManager manager, object source, PropertyDescriptor pd)
			{
				this._manager = manager;
				this._source = source;
				this._pd = pd;
				this._eventArgs = new ValueChangedEventArgs(pd);
				pd.AddValueChanged(source, new EventHandler(this.OnValueChanged));
			}

			// Token: 0x17001F92 RID: 8082
			// (get) Token: 0x06008E02 RID: 36354 RVA: 0x0025AFBC File Offset: 0x002591BC
			public bool IsEmpty
			{
				get
				{
					bool flag = this._listeners.IsEmpty;
					if (!flag && this.HasIgnorableListeners)
					{
						flag = true;
						int i = 0;
						int count = this._listeners.Count;
						while (i < count)
						{
							if (!this.IsIgnorable(this._listeners.GetListener(i).Target))
							{
								flag = false;
								break;
							}
							i++;
						}
					}
					return flag;
				}
			}

			// Token: 0x06008E03 RID: 36355 RVA: 0x0025B01C File Offset: 0x0025921C
			public void Add(IWeakEventListener listener, EventHandler<ValueChangedEventArgs> handler)
			{
				WeakEventManager.ListenerList listeners = this._listeners;
				if (WeakEventManager.ListenerList.PrepareForWriting(ref listeners))
				{
					this._listeners = (WeakEventManager.ListenerList<ValueChangedEventArgs>)listeners;
				}
				if (handler != null)
				{
					this._listeners.AddHandler(handler);
					if (!this.HasIgnorableListeners && this.IsIgnorable(handler.Target))
					{
						this.HasIgnorableListeners = true;
						return;
					}
				}
				else
				{
					this._listeners.Add(listener);
				}
			}

			// Token: 0x06008E04 RID: 36356 RVA: 0x0025B080 File Offset: 0x00259280
			public void Remove(IWeakEventListener listener, EventHandler<ValueChangedEventArgs> handler)
			{
				WeakEventManager.ListenerList listeners = this._listeners;
				if (WeakEventManager.ListenerList.PrepareForWriting(ref listeners))
				{
					this._listeners = (WeakEventManager.ListenerList<ValueChangedEventArgs>)listeners;
				}
				if (handler != null)
				{
					this._listeners.RemoveHandler(handler);
				}
				else
				{
					this._listeners.Remove(listener);
				}
				if (this.IsEmpty)
				{
					this.StopListening();
				}
			}

			// Token: 0x06008E05 RID: 36357 RVA: 0x0025B0D4 File Offset: 0x002592D4
			public bool Purge()
			{
				WeakEventManager.ListenerList listeners = this._listeners;
				if (WeakEventManager.ListenerList.PrepareForWriting(ref listeners))
				{
					this._listeners = (WeakEventManager.ListenerList<ValueChangedEventArgs>)listeners;
				}
				return this._listeners.Purge();
			}

			// Token: 0x06008E06 RID: 36358 RVA: 0x0025B108 File Offset: 0x00259308
			public void StopListening()
			{
				if (this._source != null)
				{
					this._pd.RemoveValueChanged(this._source, new EventHandler(this.OnValueChanged));
					this._source = null;
				}
			}

			// Token: 0x06008E07 RID: 36359 RVA: 0x0025B138 File Offset: 0x00259338
			private void OnValueChanged(object sender, EventArgs e)
			{
				using (this._manager.ReadLock)
				{
					this._listeners.BeginUse();
				}
				try
				{
					this._manager.DeliverEventToList(sender, this._eventArgs, this._listeners);
				}
				finally
				{
					this._listeners.EndUse();
				}
			}

			// Token: 0x17001F93 RID: 8083
			// (get) Token: 0x06008E08 RID: 36360 RVA: 0x0025B1AC File Offset: 0x002593AC
			// (set) Token: 0x06008E09 RID: 36361 RVA: 0x0025B1B4 File Offset: 0x002593B4
			private bool HasIgnorableListeners { get; set; }

			// Token: 0x06008E0A RID: 36362 RVA: 0x0025B1BD File Offset: 0x002593BD
			private bool IsIgnorable(object target)
			{
				return target is ValueTable;
			}

			// Token: 0x04004B2A RID: 19242
			private PropertyDescriptor _pd;

			// Token: 0x04004B2B RID: 19243
			private ValueChangedEventManager _manager;

			// Token: 0x04004B2C RID: 19244
			private object _source;

			// Token: 0x04004B2D RID: 19245
			private WeakEventManager.ListenerList<ValueChangedEventArgs> _listeners = new WeakEventManager.ListenerList<ValueChangedEventArgs>();

			// Token: 0x04004B2E RID: 19246
			private ValueChangedEventArgs _eventArgs;
		}
	}
}
