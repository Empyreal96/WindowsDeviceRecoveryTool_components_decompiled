using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Nokia.Lucid.TcpServer
{
	// Token: 0x02000039 RID: 57
	public class TcpServerIo
	{
		// Token: 0x06000187 RID: 391 RVA: 0x0000B914 File Offset: 0x00009B14
		~TcpServerIo()
		{
			this.Dispose(false);
		}

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000188 RID: 392 RVA: 0x0000B944 File Offset: 0x00009B44
		// (remove) Token: 0x06000189 RID: 393 RVA: 0x0000B97C File Offset: 0x00009B7C
		public event EventHandler<MessageIoEventArgs> OnExternalSendRequest;

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x0600018A RID: 394 RVA: 0x0000B9B1 File Offset: 0x00009BB1
		// (set) Token: 0x0600018B RID: 395 RVA: 0x0000B9B9 File Offset: 0x00009BB9
		public bool PhonetProtocol { get; set; }

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x0600018C RID: 396 RVA: 0x0000B9C2 File Offset: 0x00009BC2
		// (set) Token: 0x0600018D RID: 397 RVA: 0x0000B9CA File Offset: 0x00009BCA
		public int Port { get; private set; }

		// Token: 0x0600018E RID: 398 RVA: 0x0000B9D3 File Offset: 0x00009BD3
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600018F RID: 399 RVA: 0x0000BA00 File Offset: 0x00009C00
		public void Start(int portNumber, int portRange)
		{
			this.CheckIfDisposed();
			if (portNumber <= 0)
			{
				throw new ArgumentOutOfRangeException("portNumber", "Port number cannot be 0 or negative");
			}
			if (portRange < 0)
			{
				throw new ArgumentOutOfRangeException("portRange", "Port range cannot be negative");
			}
			this.listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			int num = portNumber + portRange;
			try
			{
				IL_4D:
				IPEndPoint localEP = new IPEndPoint(IPAddress.Any, portNumber);
				this.listener.Bind(localEP);
			}
			catch (SocketException ex)
			{
				if (ex.ErrorCode == 10048 && portNumber == num)
				{
					throw new IndexOutOfRangeException("No ports were available in the specified ports range", ex);
				}
				if (ex.ErrorCode == 10048 && portNumber < num)
				{
					portNumber++;
					goto IL_4D;
				}
				throw;
			}
			this.Port = portNumber;
			this.listener.Listen(1);
			this.cancelServer = new CancellationTokenSource();
			CancellationToken token = this.cancelServer.Token;
			this.serverThread = new Task(delegate()
			{
				this.ServerExecution(token);
			});
			this.serverThread.Start();
		}

		// Token: 0x06000190 RID: 400 RVA: 0x0000BB10 File Offset: 0x00009D10
		public void Stop()
		{
			this.CheckIfDisposed();
			try
			{
				if (this.cancelServer != null)
				{
					this.cancelServer.Cancel();
				}
				if (this.listener != null)
				{
					this.listener.Close();
					this.listener = null;
				}
				lock (this.clientObjectLock)
				{
					if (this.client != null)
					{
						this.client.Shutdown(SocketShutdown.Both);
						this.client.Close();
						this.client = null;
					}
				}
				if (this.serverThread != null)
				{
					this.serverThread.Wait(5000);
					this.serverThread.Dispose();
					this.serverThread = null;
				}
				if (this.cancelServer != null)
				{
					this.cancelServer.Dispose();
					this.cancelServer = null;
				}
			}
			catch (AggregateException)
			{
			}
		}

		// Token: 0x06000191 RID: 401 RVA: 0x0000BBFC File Offset: 0x00009DFC
		public void Send(ref byte[] dataToSend, uint length)
		{
			this.CheckIfDisposed();
			if (this.PhonetProtocol)
			{
				dataToSend[0] = 29;
			}
			try
			{
				if (this.client != null && this.client.Connected)
				{
					this.client.BeginSend(dataToSend, 0, (int)length, SocketFlags.None, new AsyncCallback(this.AsyncSend), this.client);
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000192 RID: 402 RVA: 0x0000BC6C File Offset: 0x00009E6C
		private void AsyncSend(IAsyncResult ar)
		{
			try
			{
				Socket socket = (Socket)ar.AsyncState;
				socket.EndSend(ar);
			}
			catch (ObjectDisposedException)
			{
			}
		}

		// Token: 0x06000193 RID: 403 RVA: 0x0000BCA4 File Offset: 0x00009EA4
		private void Dispose(bool disposing)
		{
			if (this.disposed)
			{
				return;
			}
			this.Stop();
			this.disposed = true;
		}

		// Token: 0x06000194 RID: 404 RVA: 0x0000BCBE File Offset: 0x00009EBE
		private void SendToDevice(ref byte[] dataToSend)
		{
			if (this.OnExternalSendRequest != null)
			{
				this.OnExternalSendRequest(this, new MessageIoEventArgs(ref dataToSend));
			}
		}

		// Token: 0x06000195 RID: 405 RVA: 0x0000BCDA File Offset: 0x00009EDA
		private void CheckIfDisposed()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException("TcpServerIo object has been disposed.");
			}
		}

		// Token: 0x06000196 RID: 406 RVA: 0x0000BCF0 File Offset: 0x00009EF0
		private void ServerExecution(CancellationToken ct)
		{
			byte[] array = new byte[1048576];
			while (!ct.IsCancellationRequested)
			{
				try
				{
					if (this.client == null)
					{
						this.client = this.listener.Accept();
					}
					int num = this.client.Receive(array);
					if (num < 1)
					{
						lock (this.clientObjectLock)
						{
							this.client.Disconnect(false);
							this.client.Dispose();
							this.client = null;
						}
					}
					else
					{
						byte[] dst = new byte[num];
						Buffer.BlockCopy(array, 0, dst, 0, num);
						this.SendToDevice(ref dst);
					}
				}
				catch (SocketException)
				{
					lock (this.clientObjectLock)
					{
						this.client.Disconnect(false);
						this.client.Dispose();
						this.client = null;
					}
				}
			}
		}

		// Token: 0x040000D1 RID: 209
		private readonly object clientObjectLock = new object();

		// Token: 0x040000D2 RID: 210
		private CancellationTokenSource cancelServer;

		// Token: 0x040000D3 RID: 211
		private Task serverThread;

		// Token: 0x040000D4 RID: 212
		private Socket client;

		// Token: 0x040000D5 RID: 213
		private Socket listener;

		// Token: 0x040000D6 RID: 214
		private bool disposed;
	}
}
