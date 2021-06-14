using System;
using System.Windows;
using System.Xml;

namespace MS.Internal.Data
{
	// Token: 0x02000750 RID: 1872
	internal class XmlNodeChangedEventManager : WeakEventManager
	{
		// Token: 0x0600774C RID: 30540 RVA: 0x0001737C File Offset: 0x0001557C
		private XmlNodeChangedEventManager()
		{
		}

		// Token: 0x0600774D RID: 30541 RVA: 0x00221963 File Offset: 0x0021FB63
		public static void AddListener(XmlDocument source, IWeakEventListener listener)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (listener == null)
			{
				throw new ArgumentNullException("listener");
			}
			XmlNodeChangedEventManager.CurrentManager.ProtectedAddListener(source, listener);
		}

		// Token: 0x0600774E RID: 30542 RVA: 0x0022198D File Offset: 0x0021FB8D
		public static void RemoveListener(XmlDocument source, IWeakEventListener listener)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (listener == null)
			{
				throw new ArgumentNullException("listener");
			}
			XmlNodeChangedEventManager.CurrentManager.ProtectedRemoveListener(source, listener);
		}

		// Token: 0x0600774F RID: 30543 RVA: 0x002219B7 File Offset: 0x0021FBB7
		public static void AddHandler(XmlDocument source, EventHandler<XmlNodeChangedEventArgs> handler)
		{
			if (handler == null)
			{
				throw new ArgumentNullException("handler");
			}
			XmlNodeChangedEventManager.CurrentManager.ProtectedAddHandler(source, handler);
		}

		// Token: 0x06007750 RID: 30544 RVA: 0x002219D3 File Offset: 0x0021FBD3
		public static void RemoveHandler(XmlDocument source, EventHandler<XmlNodeChangedEventArgs> handler)
		{
			if (handler == null)
			{
				throw new ArgumentNullException("handler");
			}
			XmlNodeChangedEventManager.CurrentManager.ProtectedRemoveHandler(source, handler);
		}

		// Token: 0x06007751 RID: 30545 RVA: 0x002219EF File Offset: 0x0021FBEF
		protected override WeakEventManager.ListenerList NewListenerList()
		{
			return new WeakEventManager.ListenerList<XmlNodeChangedEventArgs>();
		}

		// Token: 0x06007752 RID: 30546 RVA: 0x002219F8 File Offset: 0x0021FBF8
		protected override void StartListening(object source)
		{
			XmlNodeChangedEventHandler value = new XmlNodeChangedEventHandler(this.OnXmlNodeChanged);
			XmlDocument xmlDocument = (XmlDocument)source;
			xmlDocument.NodeInserted += value;
			xmlDocument.NodeRemoved += value;
			xmlDocument.NodeChanged += value;
		}

		// Token: 0x06007753 RID: 30547 RVA: 0x00221A30 File Offset: 0x0021FC30
		protected override void StopListening(object source)
		{
			XmlNodeChangedEventHandler value = new XmlNodeChangedEventHandler(this.OnXmlNodeChanged);
			XmlDocument xmlDocument = (XmlDocument)source;
			xmlDocument.NodeInserted -= value;
			xmlDocument.NodeRemoved -= value;
			xmlDocument.NodeChanged -= value;
		}

		// Token: 0x17001C58 RID: 7256
		// (get) Token: 0x06007754 RID: 30548 RVA: 0x00221A68 File Offset: 0x0021FC68
		private static XmlNodeChangedEventManager CurrentManager
		{
			get
			{
				Type typeFromHandle = typeof(XmlNodeChangedEventManager);
				XmlNodeChangedEventManager xmlNodeChangedEventManager = (XmlNodeChangedEventManager)WeakEventManager.GetCurrentManager(typeFromHandle);
				if (xmlNodeChangedEventManager == null)
				{
					xmlNodeChangedEventManager = new XmlNodeChangedEventManager();
					WeakEventManager.SetCurrentManager(typeFromHandle, xmlNodeChangedEventManager);
				}
				return xmlNodeChangedEventManager;
			}
		}

		// Token: 0x06007755 RID: 30549 RVA: 0x000174E5 File Offset: 0x000156E5
		private void OnXmlNodeChanged(object sender, XmlNodeChangedEventArgs args)
		{
			base.DeliverEvent(sender, args);
		}
	}
}
