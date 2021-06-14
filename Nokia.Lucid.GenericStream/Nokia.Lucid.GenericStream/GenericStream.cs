using System;
using System.Collections.Concurrent;

namespace Nokia.Lucid.GenericStream
{
	// Token: 0x02000005 RID: 5
	public class GenericStream : IDisposable
	{
		// Token: 0x06000011 RID: 17 RVA: 0x00002284 File Offset: 0x00000484
		public GenericStream() : this(10000)
		{
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002291 File Offset: 0x00000491
		public GenericStream(int maxItemCount)
		{
			this.MaxItemCount = maxItemCount;
			this.messageQueue = new BlockingCollection<GenericMessage>(this.MaxItemCount);
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000022B4 File Offset: 0x000004B4
		~GenericStream()
		{
			this.Dispose(false);
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000014 RID: 20 RVA: 0x000022E4 File Offset: 0x000004E4
		// (remove) Token: 0x06000015 RID: 21 RVA: 0x0000231C File Offset: 0x0000051C
		public event EventHandler<GenericStreamSendEventArgs> OnSendRequest;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000016 RID: 22 RVA: 0x00002354 File Offset: 0x00000554
		// (remove) Token: 0x06000017 RID: 23 RVA: 0x0000238C File Offset: 0x0000058C
		public event EventHandler<GenericStreamReceiveEventArgs> OnReceived;

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000018 RID: 24 RVA: 0x000023C1 File Offset: 0x000005C1
		// (set) Token: 0x06000019 RID: 25 RVA: 0x000023C9 File Offset: 0x000005C9
		public int MaxItemCount { get; private set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600001A RID: 26 RVA: 0x000023D2 File Offset: 0x000005D2
		public int ItemCount
		{
			get
			{
				return this.messageQueue.Count;
			}
		}

		// Token: 0x0600001B RID: 27 RVA: 0x000023DF File Offset: 0x000005DF
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600001C RID: 28 RVA: 0x000023EE File Offset: 0x000005EE
		public void ClearReceiveBuffer()
		{
			this.messageQueue = new BlockingCollection<GenericMessage>(10000);
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002400 File Offset: 0x00000600
		public void Add(byte[] data)
		{
			GenericMessage genericMessage = this.Encode(data);
			if (genericMessage == null)
			{
				return;
			}
			if (this.OnReceived != null && this.OnReceived != null)
			{
				GenericStreamReceiveEventArgs e = new GenericStreamReceiveEventArgs(genericMessage);
				this.HandleOnReceived(e);
				return;
			}
			if (!this.messageQueue.TryAdd(genericMessage, 500))
			{
				Console.WriteLine("GenericStream: Add Logging/Error handler, messages will be lost until there is free space in buffer");
			}
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002455 File Offset: 0x00000655
		public void Send(GenericMessage request)
		{
			this.OnSendRequest(this, new GenericStreamSendEventArgs(request));
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002469 File Offset: 0x00000669
		public void Receive(out GenericMessage resp, TimeSpan receiveTimeout)
		{
			if (!this.messageQueue.TryTake(out resp, receiveTimeout))
			{
				throw new TimeoutException("receive operation timed out");
			}
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002485 File Offset: 0x00000685
		public void SendAndReceive(GenericMessage req, out GenericMessage resp, TimeSpan receiveTimeout)
		{
			this.ClearReceiveBuffer();
			this.Send(req);
			this.Receive(out resp, receiveTimeout);
		}

		// Token: 0x06000021 RID: 33 RVA: 0x0000249C File Offset: 0x0000069C
		private void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					this.messageQueue.Dispose();
					this.messageQueue = null;
				}
				this.disposed = true;
			}
		}

		// Token: 0x06000022 RID: 34 RVA: 0x000024C2 File Offset: 0x000006C2
		private GenericMessage Encode(byte[] data)
		{
			return new GenericMessage(data);
		}

		// Token: 0x06000023 RID: 35 RVA: 0x000024CC File Offset: 0x000006CC
		private void HandleOnReceived(GenericStreamReceiveEventArgs e)
		{
			EventHandler<GenericStreamReceiveEventArgs> onReceived = this.OnReceived;
			if (onReceived != null)
			{
				onReceived(this, e);
			}
		}

		// Token: 0x04000006 RID: 6
		private const int DefaultMaxItemCount = 10000;

		// Token: 0x04000007 RID: 7
		private BlockingCollection<GenericMessage> messageQueue;

		// Token: 0x04000008 RID: 8
		private bool disposed;
	}
}
