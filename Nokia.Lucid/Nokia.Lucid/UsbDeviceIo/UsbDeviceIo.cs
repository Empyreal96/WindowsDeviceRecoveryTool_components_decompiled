using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Nokia.Lucid.Diagnostics;

namespace Nokia.Lucid.UsbDeviceIo
{
	// Token: 0x0200003A RID: 58
	public class UsbDeviceIo : IDisposable
	{
		// Token: 0x06000198 RID: 408 RVA: 0x0000BE1B File Offset: 0x0000A01B
		public UsbDeviceIo(string devicePath) : this(devicePath, 1000)
		{
		}

		// Token: 0x06000199 RID: 409 RVA: 0x0000BE44 File Offset: 0x0000A044
		public UsbDeviceIo(string devicePath, int maxItemCount)
		{
			using (EntryExitLogger.Log("UsbDeviceIo.UsbDeviceIo(string devicePath)", UsbDeviceIoTraceSource.Instance))
			{
				try
				{
					this.winUsbIo = new WinUsbIo(devicePath);
					this.winUsbIo.Open();
					this.winUsbIo.SetPipePolicy(WinUsbIo.PIPE_TYPE.PipeTypeBulkOut, WinUsbIo.POLICY_TYPE.SHORT_PACKET_TERMINATE, 1U);
					this.MaxItemCount = maxItemCount;
					this.receiveQueue = new BlockingCollection<byte[]>(this.MaxItemCount);
					this.cancelReceiver = new CancellationTokenSource();
					CancellationToken token = this.cancelReceiver.Token;
					this.receiverTask = Task.Factory.StartNew(delegate()
					{
						this.Receiver(token);
					}, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
				}
				catch (Exception arg)
				{
					RobustTrace.Trace<Exception>(new Action<Exception>(UsbDeviceIoTraceSource.Instance.DeviceIoError), arg);
					throw;
				}
			}
		}

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x0600019A RID: 410 RVA: 0x0000BF38 File Offset: 0x0000A138
		// (remove) Token: 0x0600019B RID: 411 RVA: 0x0000BF70 File Offset: 0x0000A170
		public event EventHandler<OnReceivedEventArgs> OnReceived;

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x0600019C RID: 412 RVA: 0x0000BFA8 File Offset: 0x0000A1A8
		// (remove) Token: 0x0600019D RID: 413 RVA: 0x0000BFE0 File Offset: 0x0000A1E0
		public event EventHandler<OnSendingEventArgs> OnSending;

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x0600019E RID: 414 RVA: 0x0000C015 File Offset: 0x0000A215
		// (set) Token: 0x0600019F RID: 415 RVA: 0x0000C01D File Offset: 0x0000A21D
		public int MaxItemCount { get; private set; }

		// Token: 0x060001A0 RID: 416 RVA: 0x0000C028 File Offset: 0x0000A228
		public void Dispose()
		{
			using (EntryExitLogger.Log("UsbDeviceIo.Dispose()", UsbDeviceIoTraceSource.Instance))
			{
				this.Dispose(true);
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x0000C070 File Offset: 0x0000A270
		[CLSCompliant(false)]
		public void Send(byte[] dataToSend, uint length)
		{
			using (EntryExitLogger.Log("UsbDeviceIo.Send(byte[] dataToSend, uint length)", UsbDeviceIoTraceSource.Instance))
			{
				try
				{
					OnSendingEventArgs e = new OnSendingEventArgs(dataToSend);
					this.HandleOnSending(e);
					this.winUsbIo.Write(dataToSend, length);
					RobustTrace.Trace<byte[]>(new Action<byte[]>(UsbDeviceIoTraceSource.Instance.DeviceIoMessageOut), dataToSend);
				}
				catch (Exception arg)
				{
					RobustTrace.Trace<Exception>(new Action<Exception>(UsbDeviceIoTraceSource.Instance.DeviceIoError), arg);
					throw;
				}
			}
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x0000C104 File Offset: 0x0000A304
		[CLSCompliant(false)]
		public uint Receive(out byte[] receivedData, TimeSpan receiveTimeout)
		{
			uint result;
			using (EntryExitLogger.Log("UsbDeviceIo.Receive(ref byte[] receivedData, TimeSpan receiveTimeout)", UsbDeviceIoTraceSource.Instance))
			{
				try
				{
					if (!this.receiveQueue.TryTake(out receivedData, receiveTimeout))
					{
						throw new TimeoutException("receive operation timed out");
					}
					result = (uint)receivedData.Length;
				}
				catch (Exception arg)
				{
					RobustTrace.Trace<Exception>(new Action<Exception>(UsbDeviceIoTraceSource.Instance.DeviceIoError), arg);
					throw;
				}
			}
			return result;
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x0000C184 File Offset: 0x0000A384
		[CLSCompliant(false)]
		public void GetUsbDeviceDescriptor(out NativeMethods.USB_DEVICE_DESCRIPTOR deviceDescriptor)
		{
			using (EntryExitLogger.Log("UsbDeviceIo.GetUsbDeviceDescriptor(deviceDescriptor)", UsbDeviceIoTraceSource.Instance))
			{
				try
				{
					this.winUsbIo.GetDeviceDescriptor(out deviceDescriptor);
				}
				catch (Exception arg)
				{
					RobustTrace.Trace<Exception>(new Action<Exception>(UsbDeviceIoTraceSource.Instance.DeviceIoError), arg);
					throw;
				}
			}
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x0000C1F0 File Offset: 0x0000A3F0
		private static byte[] GetNewBuffer(uint length)
		{
			return new byte[length];
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x0000C1FC File Offset: 0x0000A3FC
		private void Receiver(CancellationToken ct)
		{
			using (EntryExitLogger.Log("UsbDeviceIo.Receiver(CancellationToken ct)", UsbDeviceIoTraceSource.Instance))
			{
				byte[] array = new byte[1048576];
				while (!ct.IsCancellationRequested)
				{
					try
					{
						uint num = this.winUsbIo.Read(array, 1048576U);
						if (num > 0U)
						{
							byte[] newBuffer = UsbDeviceIo.GetNewBuffer(num);
							Buffer.BlockCopy(array, 0, newBuffer, 0, (int)num);
							if (this.OnReceived != null)
							{
								OnReceivedEventArgs e = new OnReceivedEventArgs(newBuffer);
								this.HandleOnReceived(e);
							}
							else if (!this.receiveQueue.TryAdd(newBuffer, 500))
							{
								RobustTrace.Trace<Exception>(new Action<Exception>(UsbDeviceIoTraceSource.Instance.DeviceIoError), new Exception("Messages will be lost until there is free space in buffer"));
								continue;
							}
							RobustTrace.Trace<byte[]>(new Action<byte[]>(UsbDeviceIoTraceSource.Instance.DeviceIoMessageIn), newBuffer);
						}
					}
					catch (Win32Exception ex)
					{
						if (ex.NativeErrorCode == 31)
						{
							RobustTrace.Trace<string>(new Action<string>(UsbDeviceIoTraceSource.Instance.DeviceIoInformation), ex.Message);
							break;
						}
						if (ex.NativeErrorCode == 995)
						{
							RobustTrace.Trace<string>(new Action<string>(UsbDeviceIoTraceSource.Instance.DeviceIoInformation), ex.Message);
						}
						else
						{
							RobustTrace.Trace<Win32Exception>(new Action<Win32Exception>(UsbDeviceIoTraceSource.Instance.DeviceIoErrorWin32), ex);
						}
					}
					catch (Exception arg)
					{
						RobustTrace.Trace<Exception>(new Action<Exception>(UsbDeviceIoTraceSource.Instance.DeviceIoError), arg);
					}
				}
			}
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x0000C3B4 File Offset: 0x0000A5B4
		private void Dispose(bool disposing)
		{
			using (EntryExitLogger.Log("UsbDeviceIo.Dispose(bool disposing)", UsbDeviceIoTraceSource.Instance))
			{
				try
				{
					if (disposing)
					{
						this.cancelReceiver.Cancel();
						if (!this.receiverTask.Wait(10))
						{
							try
							{
								this.winUsbIo.CancelIo();
							}
							catch (Win32Exception arg)
							{
								RobustTrace.Trace<Win32Exception>(new Action<Win32Exception>(UsbDeviceIoTraceSource.Instance.DeviceIoErrorWin32), arg);
							}
							this.receiverTask.Wait(1000);
						}
						if (this.receiverTask.IsCompleted)
						{
							this.receiverTask.Dispose();
						}
						this.cancelReceiver.Dispose();
						this.receiveQueue.Dispose();
					}
				}
				catch (AggregateException ex)
				{
					foreach (Exception ex2 in ex.InnerExceptions)
					{
						Console.WriteLine("msg: " + ex2.Message);
					}
				}
				catch (Exception arg2)
				{
					RobustTrace.Trace<Exception>(new Action<Exception>(UsbDeviceIoTraceSource.Instance.DeviceIoError), arg2);
				}
				this.winUsbIo.Dispose();
			}
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x0000C510 File Offset: 0x0000A710
		private void HandleOnReceived(OnReceivedEventArgs e)
		{
			using (EntryExitLogger.Log("UsbDeviceIo.HandleOnReceived(OnReceivedEventArgs e)", UsbDeviceIoTraceSource.Instance))
			{
				EventHandler<OnReceivedEventArgs> onReceived = this.OnReceived;
				if (onReceived != null)
				{
					onReceived(this, e);
				}
			}
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x0000C55C File Offset: 0x0000A75C
		private void HandleOnSending(OnSendingEventArgs e)
		{
			using (EntryExitLogger.Log("UsbDeviceIo.HandleOnSending(OnSendingEventArgs e)", UsbDeviceIoTraceSource.Instance))
			{
				EventHandler<OnSendingEventArgs> onSending = this.OnSending;
				if (onSending != null)
				{
					onSending(this, e);
				}
			}
		}

		// Token: 0x040000DA RID: 218
		private const int DefaultMaxItemCount = 1000;

		// Token: 0x040000DB RID: 219
		private readonly WinUsbIo winUsbIo;

		// Token: 0x040000DC RID: 220
		private readonly BlockingCollection<byte[]> receiveQueue;

		// Token: 0x040000DD RID: 221
		private readonly Task receiverTask;

		// Token: 0x040000DE RID: 222
		private readonly CancellationTokenSource cancelReceiver;
	}
}
