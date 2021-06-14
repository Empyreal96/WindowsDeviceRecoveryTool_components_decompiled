using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace System.Windows.Forms
{
	// Token: 0x02000273 RID: 627
	internal abstract class HtmlShim : IDisposable
	{
		// Token: 0x060025D6 RID: 9686 RVA: 0x000B4B04 File Offset: 0x000B2D04
		~HtmlShim()
		{
			this.Dispose(false);
		}

		// Token: 0x1700092E RID: 2350
		// (get) Token: 0x060025D7 RID: 9687 RVA: 0x000B4B34 File Offset: 0x000B2D34
		private EventHandlerList Events
		{
			get
			{
				if (this.events == null)
				{
					this.events = new EventHandlerList();
				}
				return this.events;
			}
		}

		// Token: 0x060025D8 RID: 9688
		public abstract void AttachEventHandler(string eventName, EventHandler eventHandler);

		// Token: 0x060025D9 RID: 9689 RVA: 0x000B4B4F File Offset: 0x000B2D4F
		public void AddHandler(object key, Delegate value)
		{
			this.eventCount++;
			this.Events.AddHandler(key, value);
			this.OnEventHandlerAdded();
		}

		// Token: 0x060025DA RID: 9690 RVA: 0x000B4B74 File Offset: 0x000B2D74
		protected HtmlToClrEventProxy AddEventProxy(string eventName, EventHandler eventHandler)
		{
			if (this.attachedEventList == null)
			{
				this.attachedEventList = new Dictionary<EventHandler, HtmlToClrEventProxy>();
			}
			HtmlToClrEventProxy htmlToClrEventProxy = new HtmlToClrEventProxy(this, eventName, eventHandler);
			this.attachedEventList[eventHandler] = htmlToClrEventProxy;
			return htmlToClrEventProxy;
		}

		// Token: 0x1700092F RID: 2351
		// (get) Token: 0x060025DB RID: 9691
		public abstract UnsafeNativeMethods.IHTMLWindow2 AssociatedWindow { get; }

		// Token: 0x060025DC RID: 9692
		public abstract void ConnectToEvents();

		// Token: 0x060025DD RID: 9693
		public abstract void DetachEventHandler(string eventName, EventHandler eventHandler);

		// Token: 0x060025DE RID: 9694 RVA: 0x000B4BAC File Offset: 0x000B2DAC
		public virtual void DisconnectFromEvents()
		{
			if (this.attachedEventList != null)
			{
				EventHandler[] array = new EventHandler[this.attachedEventList.Count];
				this.attachedEventList.Keys.CopyTo(array, 0);
				foreach (EventHandler eventHandler in array)
				{
					HtmlToClrEventProxy htmlToClrEventProxy = this.attachedEventList[eventHandler];
					this.DetachEventHandler(htmlToClrEventProxy.EventName, eventHandler);
				}
			}
		}

		// Token: 0x060025DF RID: 9695
		protected abstract object GetEventSender();

		// Token: 0x060025E0 RID: 9696 RVA: 0x000B4C14 File Offset: 0x000B2E14
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060025E1 RID: 9697 RVA: 0x000B4C23 File Offset: 0x000B2E23
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.DisconnectFromEvents();
				if (this.events != null)
				{
					this.events.Dispose();
					this.events = null;
				}
			}
		}

		// Token: 0x060025E2 RID: 9698 RVA: 0x000B4C48 File Offset: 0x000B2E48
		public void FireEvent(object key, EventArgs e)
		{
			Delegate @delegate = this.Events[key];
			if (@delegate != null)
			{
				try
				{
					@delegate.DynamicInvoke(new object[]
					{
						this.GetEventSender(),
						e
					});
				}
				catch (Exception t)
				{
					if (NativeWindow.WndProcShouldBeDebuggable)
					{
						throw;
					}
					Application.OnThreadException(t);
				}
			}
		}

		// Token: 0x060025E3 RID: 9699 RVA: 0x000B4CA4 File Offset: 0x000B2EA4
		protected virtual void OnEventHandlerAdded()
		{
			this.ConnectToEvents();
		}

		// Token: 0x060025E4 RID: 9700 RVA: 0x000B4CAC File Offset: 0x000B2EAC
		protected virtual void OnEventHandlerRemoved()
		{
			if (this.eventCount <= 0)
			{
				this.DisconnectFromEvents();
				this.eventCount = 0;
			}
		}

		// Token: 0x060025E5 RID: 9701 RVA: 0x000B4CC4 File Offset: 0x000B2EC4
		public void RemoveHandler(object key, Delegate value)
		{
			this.eventCount--;
			this.Events.RemoveHandler(key, value);
			this.OnEventHandlerRemoved();
		}

		// Token: 0x060025E6 RID: 9702 RVA: 0x000B4CE8 File Offset: 0x000B2EE8
		protected HtmlToClrEventProxy RemoveEventProxy(EventHandler eventHandler)
		{
			if (this.attachedEventList == null)
			{
				return null;
			}
			if (this.attachedEventList.ContainsKey(eventHandler))
			{
				HtmlToClrEventProxy result = this.attachedEventList[eventHandler];
				this.attachedEventList.Remove(eventHandler);
				return result;
			}
			return null;
		}

		// Token: 0x04001022 RID: 4130
		private EventHandlerList events;

		// Token: 0x04001023 RID: 4131
		private int eventCount;

		// Token: 0x04001024 RID: 4132
		private Dictionary<EventHandler, HtmlToClrEventProxy> attachedEventList;
	}
}
