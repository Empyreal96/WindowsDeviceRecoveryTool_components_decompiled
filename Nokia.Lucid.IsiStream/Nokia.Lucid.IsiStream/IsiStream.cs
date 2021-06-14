using System;
using System.Collections.Concurrent;

namespace Nokia.Lucid.IsiStream
{
	// Token: 0x02000006 RID: 6
	public class IsiStream : IDisposable
	{
		// Token: 0x06000031 RID: 49 RVA: 0x000024C1 File Offset: 0x000006C1
		public IsiStream() : this(10000)
		{
		}

		// Token: 0x06000032 RID: 50 RVA: 0x000024CE File Offset: 0x000006CE
		public IsiStream(int maxItemCount)
		{
			this.ObjectId = PhonetObjectIdManager.LeaseObjectId();
			this.ObjectIdFilterEnabled = true;
			this.MaxItemCount = maxItemCount;
			this.messageQueue = new BlockingCollection<IsiMessage>(this.MaxItemCount);
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00002500 File Offset: 0x00000700
		~IsiStream()
		{
			this.Dispose(false);
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000034 RID: 52 RVA: 0x00002530 File Offset: 0x00000730
		// (remove) Token: 0x06000035 RID: 53 RVA: 0x00002568 File Offset: 0x00000768
		public event EventHandler<IsiSendEventArgs> OnSendRequest;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000036 RID: 54 RVA: 0x000025A0 File Offset: 0x000007A0
		// (remove) Token: 0x06000037 RID: 55 RVA: 0x000025D8 File Offset: 0x000007D8
		public event EventHandler<IsiReceiveEventArgs> OnReceived;

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000038 RID: 56 RVA: 0x0000260D File Offset: 0x0000080D
		// (set) Token: 0x06000039 RID: 57 RVA: 0x00002615 File Offset: 0x00000815
		public int MaxItemCount { get; private set; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600003A RID: 58 RVA: 0x0000261E File Offset: 0x0000081E
		public int ItemCount
		{
			get
			{
				return this.messageQueue.Count;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600003B RID: 59 RVA: 0x0000262B File Offset: 0x0000082B
		// (set) Token: 0x0600003C RID: 60 RVA: 0x00002633 File Offset: 0x00000833
		public byte ObjectId { get; private set; }

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600003D RID: 61 RVA: 0x0000263C File Offset: 0x0000083C
		// (set) Token: 0x0600003E RID: 62 RVA: 0x00002644 File Offset: 0x00000844
		public bool ObjectIdFilterEnabled { get; set; }

		// Token: 0x0600003F RID: 63 RVA: 0x0000264D File Offset: 0x0000084D
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000040 RID: 64 RVA: 0x0000265C File Offset: 0x0000085C
		public void ClearReceiveBuffer()
		{
			this.messageQueue = new BlockingCollection<IsiMessage>(10000);
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00002670 File Offset: 0x00000870
		public void Add(byte[] data)
		{
			if ((long)data.Length < (long)((ulong)IsiMessage.FirstDataByteIndex))
			{
				return;
			}
			IsiMessage isiMessage = this.Encode(data);
			if (isiMessage == null)
			{
				return;
			}
			if (this.ObjectIdFilterEnabled && isiMessage.ReceiverObject != this.ObjectId)
			{
				return;
			}
			if (this.OnReceived != null && this.OnReceived != null)
			{
				IsiReceiveEventArgs e = new IsiReceiveEventArgs(isiMessage);
				this.HandleOnReceived(e);
				return;
			}
			if (!this.messageQueue.TryAdd(isiMessage, 500))
			{
				Console.WriteLine("IsiStream: Add Logging/Error handler, messages will be lost until there is free space in buffer");
			}
		}

		// Token: 0x06000042 RID: 66 RVA: 0x000026E9 File Offset: 0x000008E9
		public void Send(IsiMessage request)
		{
			this.Decode(request);
			this.OnSendRequest(this, new IsiSendEventArgs(request));
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00002704 File Offset: 0x00000904
		public void Receive(out IsiMessage resp, TimeSpan receiveTimeout)
		{
			if (!this.messageQueue.TryTake(out resp, receiveTimeout))
			{
				throw new TimeoutException("receive operation timed out");
			}
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00002720 File Offset: 0x00000920
		public void SendAndReceive(IsiMessage req, out IsiMessage resp, TimeSpan receiveTimeout)
		{
			this.ClearReceiveBuffer();
			this.Send(req);
			this.Receive(out resp, receiveTimeout);
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00002737 File Offset: 0x00000937
		private void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					PhonetObjectIdManager.ReleaseObjectId(this.ObjectId);
					this.messageQueue.Dispose();
					this.messageQueue = null;
				}
				this.disposed = true;
			}
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00002768 File Offset: 0x00000968
		private IsiMessage Encode(byte[] data)
		{
			return new IsiMessage(data);
		}

		// Token: 0x06000047 RID: 71 RVA: 0x0000277D File Offset: 0x0000097D
		private void Decode(IsiMessage message)
		{
			message[7U] = this.ObjectId;
			message[8U] = IsiUtidManager.GetUtid();
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00002798 File Offset: 0x00000998
		private void HandleOnReceived(IsiReceiveEventArgs e)
		{
			EventHandler<IsiReceiveEventArgs> onReceived = this.OnReceived;
			if (onReceived != null)
			{
				onReceived(this, e);
			}
		}

		// Token: 0x04000012 RID: 18
		private const int DefaultMaxItemCount = 10000;

		// Token: 0x04000013 RID: 19
		private BlockingCollection<IsiMessage> messageQueue;

		// Token: 0x04000014 RID: 20
		private bool disposed;
	}
}
