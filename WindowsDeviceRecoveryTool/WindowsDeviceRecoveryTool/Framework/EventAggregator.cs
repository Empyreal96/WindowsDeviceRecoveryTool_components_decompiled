using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.WindowsDeviceRecoveryTool.Common;

namespace Microsoft.WindowsDeviceRecoveryTool.Framework
{
	// Token: 0x020000E9 RID: 233
	[Export]
	public sealed class EventAggregator
	{
		// Token: 0x06000781 RID: 1921 RVA: 0x0002794C File Offset: 0x00025B4C
		public void Subscribe(ICanHandle instance)
		{
			lock (this.subscribers)
			{
				if (!this.subscribers.Any((WeakReference reference) => reference.Target == instance))
				{
					this.subscribers.Add(new WeakReference(instance));
				}
			}
		}

		// Token: 0x06000782 RID: 1922 RVA: 0x00027A10 File Offset: 0x00025C10
		public void Unsubscribe(ICanHandle instance)
		{
			lock (this.subscribers)
			{
				WeakReference weakReference = this.subscribers.FirstOrDefault((WeakReference reference) => reference.Target == instance);
				if (weakReference != null)
				{
					this.subscribers.Remove(weakReference);
				}
			}
		}

		// Token: 0x06000783 RID: 1923 RVA: 0x00027AA4 File Offset: 0x00025CA4
		public void Publish<TMessage>(TMessage message)
		{
			this.Publish<TMessage>(message, new Action<Action>(this.Execute));
		}

		// Token: 0x06000784 RID: 1924 RVA: 0x00027ABB File Offset: 0x00025CBB
		private void Execute(Action action)
		{
			AppDispatcher.Execute(action, false);
		}

		// Token: 0x06000785 RID: 1925 RVA: 0x00027BF4 File Offset: 0x00025DF4
		private void Publish<TMessage>(TMessage message, Action<Action> executor)
		{
			WeakReference[] objectsToNotify;
			lock (this.subscribers)
			{
				objectsToNotify = this.subscribers.ToArray();
			}
			executor(delegate
			{
				List<WeakReference> list = new List<WeakReference>();
				WeakReference[] objectsToNotify;
				foreach (WeakReference weakReference in objectsToNotify)
				{
					ICanHandle<TMessage> canHandle = weakReference.Target as ICanHandle<TMessage>;
					if (canHandle != null)
					{
						canHandle.Handle(message);
					}
					else if (!weakReference.IsAlive)
					{
						list.Add(weakReference);
					}
				}
				if (list.Count > 0)
				{
					lock (this.subscribers)
					{
						foreach (WeakReference item in list)
						{
							this.subscribers.Remove(item);
						}
					}
				}
			});
		}

		// Token: 0x0400035B RID: 859
		private readonly List<WeakReference> subscribers = new List<WeakReference>();
	}
}
