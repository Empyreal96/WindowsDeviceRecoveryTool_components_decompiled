using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.WindowsDeviceRecoveryTool.Common;
using Microsoft.WindowsDeviceRecoveryTool.Common.Tracing;
using Microsoft.WindowsDeviceRecoveryTool.Model.Enums;

namespace Microsoft.WindowsDeviceRecoveryTool.LumiaAdaptation.Services
{
	// Token: 0x02000008 RID: 8
	[Export]
	public class ProcessManager : IDisposable
	{
		// Token: 0x06000077 RID: 119 RVA: 0x0000579C File Offset: 0x0000399C
		[ImportingConstructor]
		public ProcessManager()
		{
			this.timeoutTimer = new System.Timers.Timer(90000.0);
			this.timeoutTimer.Elapsed += this.Thor2TimeoutOccured;
		}

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x06000078 RID: 120 RVA: 0x000057EC File Offset: 0x000039EC
		// (remove) Token: 0x06000079 RID: 121 RVA: 0x00005828 File Offset: 0x00003A28
		public event Action<DataReceivedEventArgs> OnOutputDataReceived;

		// Token: 0x0600007A RID: 122 RVA: 0x00005864 File Offset: 0x00003A64
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00005878 File Offset: 0x00003A78
		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				this.disposed = true;
			}
		}

		// Token: 0x0600007C RID: 124 RVA: 0x000058A0 File Offset: 0x00003AA0
		private void Thor2TimeoutOccured(object sender, ElapsedEventArgs e)
		{
			Tracer<ProcessManager>.WriteInformation("Thor2.exe timeout occured");
			lock (this.timeoutTimer)
			{
				this.ReleaseManagedObjects();
				this.timeoutOccured = true;
			}
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00005900 File Offset: 0x00003B00
		private void RestartTimeoutTimer()
		{
			lock (this.timeoutTimer)
			{
				if (!this.timeoutOccured)
				{
					this.timeoutTimer.Stop();
					this.timeoutTimer.Start();
				}
			}
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00005994 File Offset: 0x00003B94
		public Thor2ExitCode RunThor2ProcessWithArguments(string processArguments, CancellationToken cancellationToken)
		{
			ProcessHelper thor2ProcessProcess = this.PrepareThorProcess(processArguments);
			this.timeoutOccured = false;
			this.RestartTimeoutTimer();
			thor2ProcessProcess.OutputDataReceived += this.OutputDataReceived;
			thor2ProcessProcess.Start();
			int id = thor2ProcessProcess.Id;
			this.actualProcessIds.Add(id);
			thor2ProcessProcess.BeginOutputReadLine();
			Task task = new Task(delegate()
			{
				this.CancellationMonitor(cancellationToken, thor2ProcessProcess);
			});
			task.Start();
			thor2ProcessProcess.WaitForExit();
			thor2ProcessProcess.OutputDataReceived -= this.OutputDataReceived;
			this.timeoutTimer.Stop();
			lock (this.actualProcessIds)
			{
				if (this.actualProcessIds.Contains(id))
				{
					this.actualProcessIds.Remove(id);
				}
			}
			Thor2ExitCode result;
			if (this.timeoutOccured)
			{
				result = Thor2ExitCode.Thor2NotResponding;
			}
			else
			{
				result = (Thor2ExitCode)thor2ProcessProcess.ExitCode;
			}
			return result;
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00005AEC File Offset: 0x00003CEC
		private void OutputDataReceived(object sender, DataReceivedEventArgs dataReceivedEventArgs)
		{
			this.RestartTimeoutTimer();
			Action<DataReceivedEventArgs> onOutputDataReceived = this.OnOutputDataReceived;
			if (onOutputDataReceived != null)
			{
				onOutputDataReceived(dataReceivedEventArgs);
			}
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00005B1C File Offset: 0x00003D1C
		private void CancellationMonitor(CancellationToken token, ProcessHelper helper)
		{
			while (!helper.HasExited)
			{
				Thread.Sleep(500);
				if (token.IsCancellationRequested)
				{
					if (!helper.HasExited)
					{
						Tracer<ProcessManager>.WriteInformation("Cancellation requested. Process still running. Need to manually kill process.");
						helper.Kill();
					}
				}
			}
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00005B74 File Offset: 0x00003D74
		private ProcessHelper PrepareThorProcess(string processArguments)
		{
			string workingDirectoryPath = this.GetWorkingDirectoryPath();
			Tracer<ProcessManager>.WriteInformation("Creating process start information.");
			ProcessStartInfo startInfo = new ProcessStartInfo
			{
				FileName = string.Format("\"{0}\"", Path.Combine(workingDirectoryPath, "thor2.exe")),
				Arguments = processArguments,
				UseShellExecute = false,
				RedirectStandardError = true,
				RedirectStandardOutput = true,
				CreateNoWindow = true,
				WorkingDirectory = workingDirectoryPath
			};
			Tracer<ProcessManager>.WriteInformation("Creating process helper.");
			return new ProcessHelper
			{
				EnableRaisingEvents = true,
				StartInfo = startInfo
			};
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00005C10 File Offset: 0x00003E10
		private string GetWorkingDirectoryPath()
		{
			string directoryName = Path.GetDirectoryName(Uri.UnescapeDataString(new UriBuilder(Assembly.GetExecutingAssembly().CodeBase).Path));
			if (string.IsNullOrWhiteSpace(directoryName))
			{
				Tracer<ProcessManager>.WriteError("Could not find working directory path", new object[0]);
				throw new Exception("Could not find working directory path");
			}
			return directoryName;
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00005C6C File Offset: 0x00003E6C
		internal void ReleaseManagedObjects()
		{
			lock (this.actualProcessIds)
			{
				for (int i = this.actualProcessIds.Count - 1; i >= 0; i--)
				{
					this.AbortProcess(this.actualProcessIds[i]);
					this.actualProcessIds.RemoveAt(i);
				}
			}
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00005CF4 File Offset: 0x00003EF4
		private void AbortProcess(int processId)
		{
			try
			{
				Process processById = ProcessHelper.GetProcessById(processId);
				if (processById.ProcessName.Equals("thor2", StringComparison.OrdinalIgnoreCase))
				{
					Tracer<ProcessManager>.WriteInformation("Killing process {0}", new object[]
					{
						processId
					});
					processById.Kill();
					Tracer<ProcessManager>.WriteInformation("Process {0} killed", new object[]
					{
						processId
					});
				}
			}
			catch (Exception error)
			{
				Tracer<ProcessManager>.WriteError(error, "Aborting device update process {0} failed", new object[]
				{
					processId
				});
				throw;
			}
		}

		// Token: 0x04000034 RID: 52
		private readonly List<int> actualProcessIds = new List<int>();

		// Token: 0x04000035 RID: 53
		private readonly System.Timers.Timer timeoutTimer;

		// Token: 0x04000036 RID: 54
		private bool disposed;

		// Token: 0x04000037 RID: 55
		private bool timeoutOccured;
	}
}
