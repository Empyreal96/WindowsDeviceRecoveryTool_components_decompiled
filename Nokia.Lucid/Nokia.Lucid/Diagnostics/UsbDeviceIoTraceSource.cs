using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace Nokia.Lucid.Diagnostics
{
	// Token: 0x02000027 RID: 39
	internal sealed class UsbDeviceIoTraceSource : TraceSource
	{
		// Token: 0x0600012E RID: 302 RVA: 0x0000B154 File Offset: 0x00009354
		private UsbDeviceIoTraceSource() : base("Nokia.Lucid.UsbDeviceIo")
		{
		}

		// Token: 0x0600012F RID: 303 RVA: 0x0000B164 File Offset: 0x00009364
		public void DeviceIoInformation(string infoString)
		{
			string messageText = string.Format(CultureInfo.InvariantCulture, "Message: {0}", new object[]
			{
				infoString
			});
			this.TraceEvent(TraceEventType.Verbose, UsbDeviceIoTraceEventId.GenericTrace, messageText);
		}

		// Token: 0x06000130 RID: 304 RVA: 0x0000B198 File Offset: 0x00009398
		public void DeviceIoError(Exception exception)
		{
			string messageText = string.Format(CultureInfo.InvariantCulture, "Exception:\r\n{0}", new object[]
			{
				exception
			});
			this.TraceEvent(TraceEventType.Error, UsbDeviceIoTraceEventId.GenericTrace, messageText);
		}

		// Token: 0x06000131 RID: 305 RVA: 0x0000B1CC File Offset: 0x000093CC
		public void DeviceIoErrorWin32(Win32Exception exception)
		{
			string messageText = string.Format(CultureInfo.InvariantCulture, "StatusCode: {0}\r\nWin32Exception:\r\n{1}", new object[]
			{
				exception.NativeErrorCode,
				exception
			});
			this.TraceEvent(TraceEventType.Error, UsbDeviceIoTraceEventId.GenericTrace, messageText);
		}

		// Token: 0x06000132 RID: 306 RVA: 0x0000B20C File Offset: 0x0000940C
		public void DeviceIoMessageOut(byte[] message)
		{
			if (base.Switch.Level == SourceLevels.Information)
			{
				string messageText = string.Format(CultureInfo.InvariantCulture, ">> {0}", new object[]
				{
					this.FormatMessageTruncated(message)
				});
				this.TraceEvent(TraceEventType.Information, UsbDeviceIoTraceEventId.GenericTrace, messageText);
			}
			if (base.Switch.Level >= SourceLevels.Verbose)
			{
				string messageText2 = string.Format(CultureInfo.InvariantCulture, ">> {0}", new object[]
				{
					this.FormatMessage(message)
				});
				this.TraceEvent(TraceEventType.Information, UsbDeviceIoTraceEventId.GenericTrace, messageText2);
			}
		}

		// Token: 0x06000133 RID: 307 RVA: 0x0000B290 File Offset: 0x00009490
		public void DeviceIoMessageIn(byte[] message)
		{
			if (base.Switch.Level == SourceLevels.Information)
			{
				string messageText = string.Format(CultureInfo.InvariantCulture, "<< {0}", new object[]
				{
					this.FormatMessageTruncated(message)
				});
				this.TraceEvent(TraceEventType.Information, UsbDeviceIoTraceEventId.GenericTrace, messageText);
			}
			if (base.Switch.Level >= SourceLevels.Verbose)
			{
				string messageText2 = string.Format(CultureInfo.InvariantCulture, "<< {0}", new object[]
				{
					this.FormatMessage(message)
				});
				this.TraceEvent(TraceEventType.Information, UsbDeviceIoTraceEventId.GenericTrace, messageText2);
			}
		}

		// Token: 0x06000134 RID: 308 RVA: 0x0000B311 File Offset: 0x00009511
		private void TraceEvent(TraceEventType eventType, UsbDeviceIoTraceEventId id, string messageText)
		{
			base.TraceEvent(eventType, (int)id, messageText);
		}

		// Token: 0x06000135 RID: 309 RVA: 0x0000B31C File Offset: 0x0000951C
		private string FormatMessage(byte[] message)
		{
			StringBuilder stringBuilder = new StringBuilder(message.Length * 4);
			foreach (byte b in message)
			{
				stringBuilder.Append(b.ToString("x2"));
				stringBuilder.Append(", ");
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000136 RID: 310 RVA: 0x0000B370 File Offset: 0x00009570
		private string FormatMessageTruncated(byte[] message)
		{
			if (message.Length > 48)
			{
				StringBuilder stringBuilder = new StringBuilder(250);
				for (int i = 0; i < message.Length; i++)
				{
					stringBuilder.Append(message[i].ToString("x2"));
					stringBuilder.Append(", ");
					if (i == 31)
					{
						stringBuilder.Append("... ");
						i = message.Length - 17;
					}
				}
				stringBuilder.Append("(" + message.Length.ToString() + " bytes)");
				return stringBuilder.ToString();
			}
			return this.FormatMessage(message);
		}

		// Token: 0x0400009C RID: 156
		private const string TraceSourceName = "Nokia.Lucid.UsbDeviceIo";

		// Token: 0x0400009D RID: 157
		public static readonly UsbDeviceIoTraceSource Instance = new UsbDeviceIoTraceSource();
	}
}
