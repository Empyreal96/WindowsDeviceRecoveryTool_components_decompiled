using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using ClickerUtilityLibrary.Comm.USBDriver;
using ClickerUtilityLibrary.DataModel;
using ClickerUtilityLibrary.Misc;

namespace ClickerUtilityLibrary.Comm
{
	// Token: 0x02000024 RID: 36
	internal class UsbManager : IDisposable
	{
		// Token: 0x14000006 RID: 6
		// (add) Token: 0x06000111 RID: 273 RVA: 0x00007170 File Offset: 0x00005370
		// (remove) Token: 0x06000112 RID: 274 RVA: 0x000071A8 File Offset: 0x000053A8
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event EventHandler<FEvent> UsbManagerEvent;

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x06000113 RID: 275 RVA: 0x000071E0 File Offset: 0x000053E0
		// (remove) Token: 0x06000114 RID: 276 RVA: 0x00007218 File Offset: 0x00005418
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event EventHandler<byte[]> UsbDataReceived;

		// Token: 0x06000115 RID: 277 RVA: 0x00007250 File Offset: 0x00005450
		protected virtual void OnUsbDataReceived(byte[] data)
		{
			bool flag = this.UsbDataReceived != null;
			if (flag)
			{
				this.UsbDataReceived(this, data);
			}
		}

		// Token: 0x06000116 RID: 278 RVA: 0x0000727C File Offset: 0x0000547C
		protected virtual void OnUsbManagerEvent(FEvent eventArgs)
		{
			bool flag = this.UsbManagerEvent != null;
			if (flag)
			{
				this.UsbManagerEvent(this, eventArgs);
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000117 RID: 279 RVA: 0x000072A8 File Offset: 0x000054A8
		public static UsbManager Instance
		{
			get
			{
				bool flag = UsbManager.USBinstance == null;
				if (flag)
				{
					object obj = UsbManager.usbspHandler;
					lock (obj)
					{
						UsbManager.USBinstance = new UsbManager();
					}
				}
				return UsbManager.USBinstance;
			}
		}

		// Token: 0x06000118 RID: 280 RVA: 0x00007310 File Offset: 0x00005510
		private UsbManager()
		{
			this.mReadList = new List<byte>();
			this.mIsUSBReceiveThreadRun = System.Threading.ThreadState.Stopped;
			this.mReceiveStateMachine = PacketReceiveStateMachine.Idle;
			this.mTimeoutTimer = new Stopwatch();
			this.mReadBufferLock = new object();
			this.mExceptionMessage = new FEvent(EventType.ExceptionMessage);
			this.mUsbDeviceConnectedEvent = new FEvent(EventType.UsbDeviceConnected);
			this.mUsbDeviceDisconnectedEvent = new FEvent(EventType.UsbDeviceDisconnected);
			this.mSuspendThread = new AutoResetEvent(false);
			this.ConnectedDeviceFriendlyName = null;
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000119 RID: 281 RVA: 0x0000738F File Offset: 0x0000558F
		// (set) Token: 0x0600011A RID: 282 RVA: 0x00007397 File Offset: 0x00005597
		public string ConnectedDeviceFriendlyName { get; set; }

		// Token: 0x0600011B RID: 283 RVA: 0x000073A0 File Offset: 0x000055A0
		public void StartConnectionManager()
		{
			this.connectionManager.Start();
		}

		// Token: 0x0600011C RID: 284 RVA: 0x000073B0 File Offset: 0x000055B0
		private void Dispose(bool Disposing)
		{
			if (Disposing)
			{
				this.StopUSBReceiver();
				bool flag = this.usbStream != null;
				if (flag)
				{
					this.usbStream.Dispose();
					this.usbStream = null;
				}
				bool flag2 = this.connectionManager != null;
				if (flag2)
				{
					this.connectionManager.Dispose();
					this.connectionManager = null;
				}
				bool flag3 = this.mSuspendThread != null;
				if (flag3)
				{
					this.mSuspendThread.Dispose();
				}
			}
		}

		// Token: 0x0600011D RID: 285 RVA: 0x0000742B File Offset: 0x0000562B
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00007440 File Offset: 0x00005640
		~UsbManager()
		{
			this.Dispose(false);
		}

		// Token: 0x0600011F RID: 287 RVA: 0x00007474 File Offset: 0x00005674
		internal void Init(IProtocol protocol, UsbDevice device = null)
		{
			bool flag = this.mIsUSBReceiveThreadRun != System.Threading.ThreadState.Stopped;
			if (flag)
			{
				this.PauseUsbReceiver();
			}
			bool flag2 = this.usbStream != null;
			if (flag2)
			{
				this.usbStream.Dispose();
				this.usbStream = null;
			}
			bool flag3 = this.connectionManager != null;
			if (flag3)
			{
				this.connectionManager.Dispose();
				this.connectionManager = null;
			}
			this.mCurrentProtocol = protocol;
			this.mCurrentReceivingPacket = protocol.CreateNewPacket();
			this.connectionManager = new UsbConnectionManager(delegate(string connectDeviceId)
			{
				try
				{
					this.usbStream = new DTSFUsbStream(connectDeviceId);
				}
				catch (Exception ex)
				{
					this.mExceptionMessage.StringParameter = ex.Message;
					this.OnUsbManagerEvent(this.mExceptionMessage);
					bool flag4 = this.usbStream != null;
					if (flag4)
					{
						this.usbStream.Dispose();
						this.usbStream = null;
					}
				}
				this.StartUsbReceiver();
				this.mUsbDeviceConnectedEvent = new FEvent(EventType.UsbDeviceConnected)
				{
					StringParameter = this.connectionManager.Device.DeviceInstanceId,
					ObjectParameter = this.connectionManager.Device.FriendlyName
				};
				this.OnUsbManagerEvent(this.mUsbDeviceConnectedEvent);
				this.ConnectedDeviceFriendlyName = this.connectionManager.Device.FriendlyName;
			}, delegate(string disconnectDeviceId)
			{
				this.StopUSBReceiver();
				this.mUsbDeviceDisconnectedEvent = new FEvent(EventType.UsbDeviceDisconnected)
				{
					StringParameter = this.connectionManager.Device.DeviceInstanceId
				};
				this.OnUsbManagerEvent(this.mUsbDeviceDisconnectedEvent);
				this.ConnectedDeviceFriendlyName = null;
				bool flag4 = this.usbStream != null;
				if (flag4)
				{
					this.usbStream.Dispose();
					this.usbStream = null;
				}
			}, device);
			this.StartUsbReceiver();
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00007520 File Offset: 0x00005720
		internal void ChangeProtocol(IProtocol protocol)
		{
			bool flag = this.mIsUSBReceiveThreadRun != System.Threading.ThreadState.Stopped;
			if (flag)
			{
				this.PauseUsbReceiver();
			}
			this.mCurrentProtocol = protocol;
			this.mCurrentReceivingPacket = protocol.CreateNewPacket();
			this.StartUsbReceiver();
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00007564 File Offset: 0x00005764
		internal void ConfigRingbuffer(PacketRingBuffer rb)
		{
			this.mRingBuffer = rb;
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00007570 File Offset: 0x00005770
		private void ResetReceivingStateMachine(bool isError)
		{
			this.mTimeoutTimer.Stop();
			this.mReceiveStateMachine = PacketReceiveStateMachine.Idle;
			object obj = this.mReadBufferLock;
			lock (obj)
			{
				if (isError)
				{
					this.mReadList.RemoveRange(0, 2);
				}
				else
				{
					this.mReadList.RemoveRange(0, this.mCurrentReceivingPacket.Length);
				}
			}
		}

		// Token: 0x06000123 RID: 291 RVA: 0x000075F4 File Offset: 0x000057F4
		private void StartUsbReceiver()
		{
			try
			{
				bool flag = this.mIsUSBReceiveThreadRun == System.Threading.ThreadState.Stopped;
				if (flag)
				{
					this.mUSBReceiveThread = new Thread(new ThreadStart(this.UsbReceiveThread))
					{
						IsBackground = true,
						Priority = ThreadPriority.Normal
					};
					this.mIsUSBReceiveThreadRun = System.Threading.ThreadState.Running;
					this.mUSBReceiveThread.Start();
				}
				else
				{
					bool flag2 = this.mIsUSBReceiveThreadRun == System.Threading.ThreadState.Suspended;
					if (flag2)
					{
						this.mIsUSBReceiveThreadRun = System.Threading.ThreadState.Running;
						this.mSuspendThread.Set();
					}
				}
			}
			catch (Exception ex)
			{
				this.mExceptionMessage.StringParameter = ex.Message;
				this.OnUsbManagerEvent(this.mExceptionMessage);
				this.mIsUSBReceiveThreadRun = System.Threading.ThreadState.Stopped;
			}
		}

		// Token: 0x06000124 RID: 292 RVA: 0x000076C0 File Offset: 0x000058C0
		private void PauseUsbReceiver()
		{
			this.mIsUSBReceiveThreadRun = System.Threading.ThreadState.Suspended;
		}

		// Token: 0x06000125 RID: 293 RVA: 0x000076D0 File Offset: 0x000058D0
		private void StopUSBReceiver()
		{
			this.mIsUSBReceiveThreadRun = System.Threading.ThreadState.Stopped;
			bool flag = this.mUSBReceiveThread != null;
			if (flag)
			{
				this.mUSBReceiveThread.Join();
			}
			this.mUSBReceiveThread = null;
		}

		// Token: 0x06000126 RID: 294 RVA: 0x0000770C File Offset: 0x0000590C
		private void UsbReceiveThread()
		{
			try
			{
				Thread.Sleep(1000);
				byte[] array = new byte[8192];
				while (this.mIsUSBReceiveThreadRun != System.Threading.ThreadState.Stopped)
				{
					try
					{
						bool flag = this.usbStream == null;
						if (flag)
						{
							Thread.Sleep(500);
						}
						else
						{
							bool flag2 = this.mIsUSBReceiveThreadRun == System.Threading.ThreadState.Suspended;
							if (flag2)
							{
								this.mSuspendThread.WaitOne();
								bool flag3 = this.usbStream == null;
								if (flag3)
								{
									continue;
								}
							}
							int num = this.usbStream.ReadWithTimeout(array, 0, array.Length, 500U);
							Thread.Sleep(10);
							int num2;
							for (int i = 0; i < num; i = num2 + 1)
							{
								object obj = this.mReadBufferLock;
								lock (obj)
								{
									this.mReadList.Add(array[i]);
								}
								num2 = i;
							}
							bool flag5 = num > 0;
							if (flag5)
							{
								object obj2 = this.mReadBufferLock;
								lock (obj2)
								{
									this.OnUsbDataReceived(this.mReadList.ToArray());
								}
							}
							while (this.mReadList.Count > 0)
							{
								switch (this.mReceiveStateMachine)
								{
								case PacketReceiveStateMachine.Idle:
									do
									{
										object obj3 = this.mReadBufferLock;
										lock (obj3)
										{
											int startOfPacketNumBytes = this.mCurrentProtocol.StartOfPacketNumBytes;
											byte[] array2 = new byte[startOfPacketNumBytes];
											for (int j = 0; j < startOfPacketNumBytes; j = num2)
											{
												array2[j] = this.mReadList[j];
												num2 = j + 1;
											}
											bool flag8 = this.mCurrentProtocol.IsStartOfPacket(array2);
											if (flag8)
											{
												this.mReceiveStateMachine = PacketReceiveStateMachine.ParseHeader;
												this.mCurrentReceivingPacket.Reset();
												break;
											}
											this.mReadList.RemoveAt(0);
										}
									}
									while (this.mReadList.Count > 1);
									break;
								case PacketReceiveStateMachine.ParseHeader:
								{
									bool flag9 = this.mReadList.Count > this.mCurrentReceivingPacket.HeaderSize;
									if (flag9)
									{
										for (int k = 0; k < this.mCurrentReceivingPacket.HeaderSize; k = num2 + 1)
										{
											this.mCurrentReceivingPacket.RawPacket[k] = this.mReadList[k];
											num2 = k;
										}
										bool flag10 = !this.mCurrentProtocol.IsHeaderValid(this.mCurrentReceivingPacket);
										if (flag10)
										{
											this.mRingBuffer.EnqueuePacket(this.mCurrentReceivingPacket);
											this.ResetReceivingStateMachine(true);
										}
										else
										{
											this.mReceiveStateMachine = PacketReceiveStateMachine.ValidateBody;
										}
									}
									break;
								}
								case PacketReceiveStateMachine.ValidateBody:
								{
									bool flag11 = this.mReadList.Count >= this.mCurrentReceivingPacket.Length;
									if (flag11)
									{
										for (int l = this.mCurrentReceivingPacket.HeaderSize; l < this.mCurrentReceivingPacket.Length; l = num2 + 1)
										{
											this.mCurrentReceivingPacket.RawPacket[l] = this.mReadList[l];
											num2 = l;
										}
										bool flag12 = !this.mCurrentProtocol.IsPacketValid(this.mCurrentReceivingPacket);
										if (flag12)
										{
											this.mRingBuffer.EnqueuePacket(this.mCurrentReceivingPacket);
											this.ResetReceivingStateMachine(true);
										}
										else
										{
											this.mRingBuffer.EnqueuePacket(this.mCurrentReceivingPacket);
											this.ResetReceivingStateMachine(false);
										}
									}
									break;
								}
								}
							}
						}
					}
					catch (Win32Exception ex)
					{
						bool flag13 = ex.Message != "A device attached to the system is not functioning";
						if (flag13)
						{
							throw;
						}
					}
					catch (IOException ex2)
					{
						bool flag14 = ex2.Message != "WinUsb SetPipe Policy failed.";
						if (flag14)
						{
							throw;
						}
					}
					catch (Exception ex3)
					{
						this.mExceptionMessage.StringParameter = ex3.Message;
						this.OnUsbManagerEvent(this.mExceptionMessage);
					}
				}
			}
			catch (Exception ex4)
			{
				bool flag15 = ex4.Message != "The system cannot find the file specified";
				if (flag15)
				{
					this.mExceptionMessage.StringParameter = ex4.Message;
					this.OnUsbManagerEvent(this.mExceptionMessage);
				}
				this.mIsUSBReceiveThreadRun = System.Threading.ThreadState.Stopped;
			}
		}

		// Token: 0x06000127 RID: 295 RVA: 0x00007C24 File Offset: 0x00005E24
		public void UsbSendByteArray(byte[] data, int length)
		{
			bool flag = length == -1;
			if (flag)
			{
				length = data.Count<byte>();
			}
			bool flag2 = this.usbStream != null;
			if (flag2)
			{
				this.usbStream.WriteWithTimeout(data, 0, length, 1000U);
			}
		}

		// Token: 0x040000DF RID: 223
		private static volatile UsbManager USBinstance;

		// Token: 0x040000E0 RID: 224
		private static readonly object usbspHandler = new object();

		// Token: 0x040000E1 RID: 225
		private DTSFUsbStream usbStream;

		// Token: 0x040000E2 RID: 226
		private UsbConnectionManager connectionManager;

		// Token: 0x040000E3 RID: 227
		private IPacket mCurrentReceivingPacket;

		// Token: 0x040000E4 RID: 228
		private Thread mUSBReceiveThread;

		// Token: 0x040000E5 RID: 229
		private volatile System.Threading.ThreadState mIsUSBReceiveThreadRun;

		// Token: 0x040000E6 RID: 230
		private PacketRingBuffer mRingBuffer;

		// Token: 0x040000E7 RID: 231
		private readonly object mReadBufferLock;

		// Token: 0x040000E8 RID: 232
		private readonly List<byte> mReadList;

		// Token: 0x040000E9 RID: 233
		private readonly Stopwatch mTimeoutTimer;

		// Token: 0x040000EA RID: 234
		private PacketReceiveStateMachine mReceiveStateMachine;

		// Token: 0x040000EB RID: 235
		private FEvent mUsbDeviceConnectedEvent;

		// Token: 0x040000EC RID: 236
		private FEvent mUsbDeviceDisconnectedEvent;

		// Token: 0x040000ED RID: 237
		private readonly FEvent mExceptionMessage;

		// Token: 0x040000EE RID: 238
		private readonly AutoResetEvent mSuspendThread;

		// Token: 0x040000EF RID: 239
		private IProtocol mCurrentProtocol;
	}
}
