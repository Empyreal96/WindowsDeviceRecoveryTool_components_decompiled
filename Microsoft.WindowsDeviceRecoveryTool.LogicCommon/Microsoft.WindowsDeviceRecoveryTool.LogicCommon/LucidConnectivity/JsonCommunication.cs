using System;
using System.Text;
using System.Threading;
using Nokia.Lucid.UsbDeviceIo;

namespace Microsoft.WindowsDeviceRecoveryTool.LogicCommon.LucidConnectivity
{
	// Token: 0x02000015 RID: 21
	public class JsonCommunication : IDisposable
	{
		// Token: 0x060000A0 RID: 160 RVA: 0x00003A50 File Offset: 0x00001C50
		public JsonCommunication(UsbDeviceIo deviceIo)
		{
			this.LucidDeviceIo = deviceIo;
			this.messageIdCounter = 0;
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00003A78 File Offset: 0x00001C78
		~JsonCommunication()
		{
			this.Dispose();
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000A2 RID: 162 RVA: 0x00003AAC File Offset: 0x00001CAC
		// (set) Token: 0x060000A3 RID: 163 RVA: 0x00003AC4 File Offset: 0x00001CC4
		public UsbDeviceIo LucidDeviceIo
		{
			get
			{
				return this.lucidIo;
			}
			set
			{
				if (this.lucidIo != null)
				{
					this.lucidIo.OnReceived -= this.HandlReceivedData;
					this.lucidIo.Dispose();
				}
				this.lucidIo = value;
				if (this.lucidIo != null)
				{
					this.lucidIo.OnReceived += this.HandlReceivedData;
					this.receiveBuffer = null;
				}
			}
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00003B3C File Offset: 0x00001D3C
		public void Dispose()
		{
			this.messageIdCounter = 0;
			if (this.lucidIo != null)
			{
				this.lucidIo.OnReceived -= this.HandlReceivedData;
				this.lucidIo.Dispose();
				this.lucidIo = null;
			}
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00003B8B File Offset: 0x00001D8B
		public void Send(byte[] request)
		{
			this.LucidDeviceIo.Send(request, (uint)request.Length);
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00003BA0 File Offset: 0x00001DA0
		public byte[] Receive(TimeSpan timeSpan)
		{
			Thread.Sleep(timeSpan);
			if (this.receiveBuffer == null)
			{
				throw new TimeoutException("JsonComms: No message received");
			}
			return this.receiveBuffer;
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00003BDB File Offset: 0x00001DDB
		public void SetFilteringState(bool doFilter)
		{
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00003BE0 File Offset: 0x00001DE0
		public void Send(string message)
		{
			lock (this.syncObject)
			{
				this.messageIdCounter++;
				message = message.Replace("\"method\"", "\"id\":" + this.messageIdCounter + ",\"method\"");
				ASCIIEncoding asciiencoding = new ASCIIEncoding();
				byte[] bytes = asciiencoding.GetBytes(message);
				this.Send(bytes);
			}
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00003C74 File Offset: 0x00001E74
		public string ReceiveJson(TimeSpan timeSpan)
		{
			byte[] bytes = this.Receive(timeSpan);
			ASCIIEncoding asciiencoding = new ASCIIEncoding();
			return asciiencoding.GetString(bytes);
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00003CA0 File Offset: 0x00001EA0
		private void HandlReceivedData(object sender, OnReceivedEventArgs eventArgs)
		{
			byte[] data = eventArgs.Data;
			lock (this.syncObject)
			{
				this.receiveBuffer = new byte[data.Length];
				Buffer.BlockCopy(data, 0, this.receiveBuffer, 0, data.Length);
			}
		}

		// Token: 0x04000063 RID: 99
		private readonly object syncObject = new object();

		// Token: 0x04000064 RID: 100
		private UsbDeviceIo lucidIo;

		// Token: 0x04000065 RID: 101
		private byte[] receiveBuffer;

		// Token: 0x04000066 RID: 102
		private int messageIdCounter;
	}
}
