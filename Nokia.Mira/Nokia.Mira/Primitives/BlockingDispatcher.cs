using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Nokia.Mira.Primitives
{
	// Token: 0x0200001D RID: 29
	internal class BlockingDispatcher<T> : IDisposable
	{
		// Token: 0x06000074 RID: 116 RVA: 0x00002D27 File Offset: 0x00000F27
		public BlockingDispatcher(Action<T> onDispatch)
		{
			this.onDispatch = onDispatch;
			this.dispatchTask = Task.Factory.StartNew(new Action(this.DispatchThread));
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00002D60 File Offset: 0x00000F60
		public void Dispatch(T item)
		{
			try
			{
				this.blockingCollection.Add(item);
			}
			catch (ObjectDisposedException)
			{
			}
			catch (InvalidOperationException)
			{
			}
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00002DA0 File Offset: 0x00000FA0
		public void Dispose()
		{
			this.blockingCollection.CompleteAdding();
			this.dispatchTask.Wait();
			this.blockingCollection.Dispose();
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00002DC4 File Offset: 0x00000FC4
		private void DispatchThread()
		{
			for (;;)
			{
				T obj;
				try
				{
					obj = this.blockingCollection.Take();
				}
				catch (InvalidOperationException)
				{
					break;
				}
				this.onDispatch(obj);
			}
		}

		// Token: 0x04000038 RID: 56
		private readonly Action<T> onDispatch;

		// Token: 0x04000039 RID: 57
		private readonly BlockingCollection<T> blockingCollection = new BlockingCollection<T>();

		// Token: 0x0400003A RID: 58
		private readonly Task dispatchTask;
	}
}
