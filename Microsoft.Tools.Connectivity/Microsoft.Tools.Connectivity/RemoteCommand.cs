using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace Microsoft.Tools.Connectivity
{
	// Token: 0x02000009 RID: 9
	[CLSCompliant(true)]
	public class RemoteCommand
	{
		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600005A RID: 90 RVA: 0x0000315A File Offset: 0x0000135A
		// (set) Token: 0x0600005B RID: 91 RVA: 0x00003162 File Offset: 0x00001362
		public string WorkingFolder { get; set; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600005C RID: 92 RVA: 0x0000316B File Offset: 0x0000136B
		// (set) Token: 0x0600005D RID: 93 RVA: 0x00003173 File Offset: 0x00001373
		public TimeSpan Timeout { get; set; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600005E RID: 94 RVA: 0x0000317C File Offset: 0x0000137C
		// (set) Token: 0x0600005F RID: 95 RVA: 0x00003184 File Offset: 0x00001384
		public bool CaptureOutput { get; set; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000060 RID: 96 RVA: 0x0000318D File Offset: 0x0000138D
		// (set) Token: 0x06000061 RID: 97 RVA: 0x00003195 File Offset: 0x00001395
		public RemoteCommand.OutputCallbackDelegate OutputCallback { get; set; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000062 RID: 98 RVA: 0x0000319E File Offset: 0x0000139E
		public bool IsRunAsLoggedOnSupported
		{
			get
			{
				return this.RemoteDevice.IsRunAsLoggedOnSupported;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000063 RID: 99 RVA: 0x000031AB File Offset: 0x000013AB
		// (set) Token: 0x06000064 RID: 100 RVA: 0x000031B3 File Offset: 0x000013B3
		public int ExitCode { get; private set; }

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000065 RID: 101 RVA: 0x000031BC File Offset: 0x000013BC
		public string Output
		{
			get
			{
				return this.outputString.ToString();
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000066 RID: 102 RVA: 0x000031C9 File Offset: 0x000013C9
		internal RemoteDevice RemoteDevice
		{
			get
			{
				return this.remoteDevice;
			}
		}

		// Token: 0x06000067 RID: 103 RVA: 0x000031D1 File Offset: 0x000013D1
		internal RemoteCommand(RemoteDevice remoteDevice, string command, string arguments)
		{
			this.remoteDevice = remoteDevice;
			this.commandText = command;
			this.argumentsText = arguments;
			this.outputString = new StringBuilder();
			this.Timeout = this.remoteDevice.Timeout;
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00003238 File Offset: 0x00001438
		public int Execute()
		{
			this.RemoteDevice.EnsureConnection();
			CallbackHandler outputCallback = new CallbackHandler(delegate(uint flags, string data)
			{
				if (this.OutputCallback != null)
				{
					this.OutputCallback(data);
				}
				if (this.CaptureOutput)
				{
					this.outputString.Append(data);
				}
			});
			string command = this.commandText;
			this.outputString.Clear();
			try
			{
				this.ExitCode = (int)this.RemoteDevice.SirepClient.LaunchWithOutput((uint)this.Timeout.TotalMilliseconds, command, this.argumentsText, this.WorkingFolder, 0U, outputCallback);
			}
			catch (COMException ex)
			{
				this.RemoteDevice.ExceptionHandler(ex);
			}
			return this.ExitCode;
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00003328 File Offset: 0x00001528
		public int Execute(AutoResetEvent cancelEvent)
		{
			AutoResetEvent completedEvent = new AutoResetEvent(false);
			Exception exception = null;
			ThreadPool.QueueUserWorkItem(delegate(object o)
			{
				try
				{
					this.Execute();
				}
				catch (Exception exception)
				{
					exception = exception;
				}
				finally
				{
					completedEvent.Set();
				}
			});
			switch (WaitHandle.WaitAny(new WaitHandle[]
			{
				completedEvent,
				cancelEvent
			}))
			{
			case 0:
				if (exception != null)
				{
					throw exception;
				}
				break;
			case 1:
				throw new OperationCanceledException();
			}
			return this.ExitCode;
		}

		// Token: 0x0600006A RID: 106 RVA: 0x000033B0 File Offset: 0x000015B0
		public int CreateProcess()
		{
			this.RemoteDevice.EnsureConnection();
			int result = 0;
			this.outputString.Clear();
			try
			{
				result = (int)this.RemoteDevice.SirepClient.CreateProcess(this.commandText, this.argumentsText, this.WorkingFolder, 0U);
			}
			catch (COMException ex)
			{
				this.RemoteDevice.ExceptionHandler(ex);
			}
			return result;
		}

		// Token: 0x0400008E RID: 142
		private RemoteDevice remoteDevice;

		// Token: 0x0400008F RID: 143
		private string commandText;

		// Token: 0x04000090 RID: 144
		private string argumentsText;

		// Token: 0x04000091 RID: 145
		private StringBuilder outputString;

		// Token: 0x0200000A RID: 10
		// (Invoke) Token: 0x0600006D RID: 109
		public delegate void OutputCallbackDelegate(string output);
	}
}
