using System;
using System.Diagnostics;
using ClickerUtilityLibrary.Comm.USBDriver;
using ClickerUtilityLibrary.DataModel;
using ClickerUtilityLibrary.Misc;

namespace ClickerUtilityLibrary.Comm
{
	// Token: 0x0200001F RID: 31
	public class CommandEngine : IDisposable
	{
		// Token: 0x14000005 RID: 5
		// (add) Token: 0x060000E2 RID: 226 RVA: 0x00006664 File Offset: 0x00004864
		// (remove) Token: 0x060000E3 RID: 227 RVA: 0x0000669C File Offset: 0x0000489C
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event EventHandler<FEvent> CommandEngineEvent;

		// Token: 0x060000E4 RID: 228 RVA: 0x000066D4 File Offset: 0x000048D4
		private void OnCommandEngineEvent(FEvent ev)
		{
			bool flag = this.CommandEngineEvent != null;
			if (flag)
			{
				this.CommandEngineEvent(this, ev);
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000E5 RID: 229 RVA: 0x00006700 File Offset: 0x00004900
		public static CommandEngine Instance
		{
			get
			{
				bool flag = CommandEngine.instance == null;
				if (flag)
				{
					object obj = CommandEngine.sHandler;
					lock (obj)
					{
						CommandEngine.instance = new CommandEngine();
					}
				}
				return CommandEngine.instance;
			}
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00006760 File Offset: 0x00004960
		private CommandEngine()
		{
			UsbManager.Instance.UsbManagerEvent += this.UsbManagerEventHandler;
			this.mReceivingRingBuffer = new PacketRingBuffer(10);
			UsbManager.Instance.ConfigRingbuffer(this.mReceivingRingBuffer);
			this.mExceptionMessage = new FEvent(EventType.ExceptionMessage);
			this.mNotifyNewMessage = new FEvent(EventType.PacketReceived);
			this.mReceivingRingBuffer.PacketRingBufferEvent += this.PacketRingBufferEventHandler;
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x000067DA File Offset: 0x000049DA
		private void UsbManagerEventHandler(object sender, FEvent fEvent)
		{
			this.OnCommandEngineEvent(fEvent);
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x000067E8 File Offset: 0x000049E8
		private void PacketRingBufferEventHandler(object sender, PacketRingBufferEventArgs eventArgs)
		{
			bool flag = eventArgs == null;
			if (!flag)
			{
				bool flag2 = eventArgs.Type == PacketRingBufferEventType.PacketEnqueued;
				if (flag2)
				{
					IPacket packet = this.mReceivingRingBuffer.DequeuePacket();
					bool flag3 = packet != null;
					if (flag3)
					{
						this.mProtocol.ParseCommandResponse(packet);
						this.mNotifyNewMessage.ObjectParameter = packet;
						this.OnCommandEngineEvent(this.mNotifyNewMessage);
					}
				}
			}
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00006850 File Offset: 0x00004A50
		public void StartCommandEngine(IProtocol protocol, UsbDevice device)
		{
			try
			{
				this.mProtocol = protocol;
				UsbManager.Instance.Init(protocol, device);
				UsbManager.Instance.StartConnectionManager();
				this.Device = device;
				this.IsStarted = true;
			}
			catch (Exception ex)
			{
				this.mExceptionMessage.StringParameter = ex.Message;
				this.OnCommandEngineEvent(this.mExceptionMessage);
			}
		}

		// Token: 0x060000EA RID: 234 RVA: 0x000068C8 File Offset: 0x00004AC8
		public void StartCommandEngine(IProtocol protocol)
		{
			this.StartCommandEngine(protocol, null);
		}

		// Token: 0x060000EB RID: 235 RVA: 0x000068D4 File Offset: 0x00004AD4
		public void SendRawData(byte[] data, int length)
		{
			UsbManager.Instance.UsbSendByteArray(data, length);
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000EC RID: 236 RVA: 0x000068E4 File Offset: 0x00004AE4
		public string ConnectedDeviceFriendlyName
		{
			get
			{
				return UsbManager.Instance.ConnectedDeviceFriendlyName;
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000ED RID: 237 RVA: 0x00006900 File Offset: 0x00004B00
		// (set) Token: 0x060000EE RID: 238 RVA: 0x00006908 File Offset: 0x00004B08
		public UsbDevice Device { get; private set; }

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000EF RID: 239 RVA: 0x00006911 File Offset: 0x00004B11
		// (set) Token: 0x060000F0 RID: 240 RVA: 0x00006919 File Offset: 0x00004B19
		public bool IsStarted { get; private set; }

		// Token: 0x060000F1 RID: 241 RVA: 0x00006922 File Offset: 0x00004B22
		internal void ChangeProtocol(IProtocol protocol)
		{
			this.mProtocol = protocol;
			UsbManager.Instance.ChangeProtocol(protocol);
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00006938 File Offset: 0x00004B38
		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				UsbManager.Instance.Dispose();
			}
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00006958 File Offset: 0x00004B58
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x040000CD RID: 205
		private const int RING_BUFFER_SIZE = 10;

		// Token: 0x040000CE RID: 206
		private readonly FEvent mExceptionMessage;

		// Token: 0x040000CF RID: 207
		private readonly FEvent mNotifyNewMessage;

		// Token: 0x040000D1 RID: 209
		private readonly PacketRingBuffer mReceivingRingBuffer;

		// Token: 0x040000D2 RID: 210
		private static CommandEngine instance;

		// Token: 0x040000D3 RID: 211
		private static readonly object sHandler = new object();

		// Token: 0x040000D4 RID: 212
		private IProtocol mProtocol;
	}
}
