using System;
using System.Collections.Specialized;

namespace System.Windows.Controls.Primitives
{
	// Token: 0x0200058C RID: 1420
	internal class InternalCollectionChangedEventManager : WeakEventManager
	{
		// Token: 0x06005E15 RID: 24085 RVA: 0x0001737C File Offset: 0x0001557C
		private InternalCollectionChangedEventManager()
		{
		}

		// Token: 0x06005E16 RID: 24086 RVA: 0x001A743C File Offset: 0x001A563C
		public static void AddListener(GridViewColumnCollection source, IWeakEventListener listener)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (listener == null)
			{
				throw new ArgumentNullException("listener");
			}
			InternalCollectionChangedEventManager.CurrentManager.ProtectedAddListener(source, listener);
		}

		// Token: 0x06005E17 RID: 24087 RVA: 0x001A7466 File Offset: 0x001A5666
		public static void RemoveListener(GridViewColumnCollection source, IWeakEventListener listener)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (listener == null)
			{
				throw new ArgumentNullException("listener");
			}
			InternalCollectionChangedEventManager.CurrentManager.ProtectedRemoveListener(source, listener);
		}

		// Token: 0x06005E18 RID: 24088 RVA: 0x001A7490 File Offset: 0x001A5690
		public static void AddHandler(GridViewColumnCollection source, EventHandler<NotifyCollectionChangedEventArgs> handler)
		{
			if (handler == null)
			{
				throw new ArgumentNullException("handler");
			}
			InternalCollectionChangedEventManager.CurrentManager.ProtectedAddHandler(source, handler);
		}

		// Token: 0x06005E19 RID: 24089 RVA: 0x001A74AC File Offset: 0x001A56AC
		public static void RemoveHandler(GridViewColumnCollection source, EventHandler<NotifyCollectionChangedEventArgs> handler)
		{
			if (handler == null)
			{
				throw new ArgumentNullException("handler");
			}
			InternalCollectionChangedEventManager.CurrentManager.ProtectedRemoveHandler(source, handler);
		}

		// Token: 0x06005E1A RID: 24090 RVA: 0x001A74C8 File Offset: 0x001A56C8
		protected override WeakEventManager.ListenerList NewListenerList()
		{
			return new WeakEventManager.ListenerList<NotifyCollectionChangedEventArgs>();
		}

		// Token: 0x06005E1B RID: 24091 RVA: 0x001A74D0 File Offset: 0x001A56D0
		protected override void StartListening(object source)
		{
			GridViewColumnCollection gridViewColumnCollection = (GridViewColumnCollection)source;
			gridViewColumnCollection.InternalCollectionChanged += this.OnCollectionChanged;
		}

		// Token: 0x06005E1C RID: 24092 RVA: 0x001A74F8 File Offset: 0x001A56F8
		protected override void StopListening(object source)
		{
			GridViewColumnCollection gridViewColumnCollection = (GridViewColumnCollection)source;
			gridViewColumnCollection.InternalCollectionChanged -= this.OnCollectionChanged;
		}

		// Token: 0x170016B8 RID: 5816
		// (get) Token: 0x06005E1D RID: 24093 RVA: 0x001A7520 File Offset: 0x001A5720
		private static InternalCollectionChangedEventManager CurrentManager
		{
			get
			{
				Type typeFromHandle = typeof(InternalCollectionChangedEventManager);
				InternalCollectionChangedEventManager internalCollectionChangedEventManager = (InternalCollectionChangedEventManager)WeakEventManager.GetCurrentManager(typeFromHandle);
				if (internalCollectionChangedEventManager == null)
				{
					internalCollectionChangedEventManager = new InternalCollectionChangedEventManager();
					WeakEventManager.SetCurrentManager(typeFromHandle, internalCollectionChangedEventManager);
				}
				return internalCollectionChangedEventManager;
			}
		}

		// Token: 0x06005E1E RID: 24094 RVA: 0x000174E5 File Offset: 0x000156E5
		private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
		{
			base.DeliverEvent(sender, args);
		}
	}
}
