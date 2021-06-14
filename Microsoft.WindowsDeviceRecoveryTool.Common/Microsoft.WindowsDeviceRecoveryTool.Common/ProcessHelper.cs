using System;
using System.Diagnostics;
using System.IO;
using Microsoft.WindowsDeviceRecoveryTool.Common.Tracing;

namespace Microsoft.WindowsDeviceRecoveryTool.Common
{
	// Token: 0x02000009 RID: 9
	public class ProcessHelper
	{
		// Token: 0x06000022 RID: 34 RVA: 0x000029F0 File Offset: 0x00000BF0
		public ProcessHelper()
		{
			this.process = new Process();
			this.process.OutputDataReceived += this.ProcessOnOutputDataReceived;
			this.process.ErrorDataReceived += this.ProcessOnErrorDataReceived;
			this.process.Exited += this.ProcessOnExited;
		}

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000023 RID: 35 RVA: 0x00002A5C File Offset: 0x00000C5C
		// (remove) Token: 0x06000024 RID: 36 RVA: 0x00002A98 File Offset: 0x00000C98
		public event DataReceivedEventHandler OutputDataReceived;

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x06000025 RID: 37 RVA: 0x00002AD4 File Offset: 0x00000CD4
		// (remove) Token: 0x06000026 RID: 38 RVA: 0x00002B10 File Offset: 0x00000D10
		public event DataReceivedEventHandler ErrorDataReceived;

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000027 RID: 39 RVA: 0x00002B4C File Offset: 0x00000D4C
		// (set) Token: 0x06000028 RID: 40 RVA: 0x00002B69 File Offset: 0x00000D69
		public bool EnableRaisingEvents
		{
			get
			{
				return this.process.EnableRaisingEvents;
			}
			set
			{
				this.process.EnableRaisingEvents = value;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000029 RID: 41 RVA: 0x00002B7C File Offset: 0x00000D7C
		// (set) Token: 0x0600002A RID: 42 RVA: 0x00002B99 File Offset: 0x00000D99
		public ProcessStartInfo StartInfo
		{
			get
			{
				return this.process.StartInfo;
			}
			set
			{
				this.process.StartInfo = value;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600002B RID: 43 RVA: 0x00002BAC File Offset: 0x00000DAC
		public int Id
		{
			get
			{
				return this.process.Id;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600002C RID: 44 RVA: 0x00002BCC File Offset: 0x00000DCC
		public bool HasExited
		{
			get
			{
				return this.process.HasExited;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600002D RID: 45 RVA: 0x00002BEC File Offset: 0x00000DEC
		public int ExitCode
		{
			get
			{
				return this.process.ExitCode;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600002E RID: 46 RVA: 0x00002C0C File Offset: 0x00000E0C
		public StreamWriter StandardInput
		{
			get
			{
				return this.process.StandardInput;
			}
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002C2C File Offset: 0x00000E2C
		public static Process GetProcessById(int processId)
		{
			return Process.GetProcessById(processId);
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002C44 File Offset: 0x00000E44
		public static Process[] GetProcessesByName(string processName)
		{
			return Process.GetProcessesByName(processName);
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002C5C File Offset: 0x00000E5C
		public bool Start()
		{
			Tracer<ProcessHelper>.WriteInformation("ProcessHelper start called {0}", new object[]
			{
				this.process.StartInfo.FileName
			});
			return this.process.Start();
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002C9F File Offset: 0x00000E9F
		public void Dispose()
		{
			this.process.Dispose();
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00002CAE File Offset: 0x00000EAE
		public void WaitForExit()
		{
			this.process.WaitForExit();
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002CBD File Offset: 0x00000EBD
		public void WaitForExit(int milliseconds)
		{
			this.process.WaitForExit(milliseconds);
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002CCD File Offset: 0x00000ECD
		public void BeginOutputReadLine()
		{
			this.process.BeginOutputReadLine();
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002CDC File Offset: 0x00000EDC
		public void Kill()
		{
			this.process.Kill();
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002CEC File Offset: 0x00000EEC
		private void ProcessOnOutputDataReceived(object sender, DataReceivedEventArgs dataReceivedEventArgs)
		{
			if (this.OutputDataReceived != null)
			{
				this.OutputDataReceived(sender, dataReceivedEventArgs);
			}
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002D18 File Offset: 0x00000F18
		private void ProcessOnErrorDataReceived(object sender, DataReceivedEventArgs dataReceivedEventArgs)
		{
			if (this.ErrorDataReceived != null)
			{
				this.ErrorDataReceived(sender, dataReceivedEventArgs);
			}
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002D43 File Offset: 0x00000F43
		private void ProcessOnExited(object sender, EventArgs eventArgs)
		{
			Tracer<ProcessHelper>.WriteInformation("Process exited");
		}

		// Token: 0x0400000B RID: 11
		private readonly Process process;
	}
}
