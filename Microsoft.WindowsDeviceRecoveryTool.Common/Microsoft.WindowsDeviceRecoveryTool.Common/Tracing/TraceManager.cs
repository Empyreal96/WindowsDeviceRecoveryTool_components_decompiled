using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Microsoft.WindowsDeviceRecoveryTool.Common.Tracing
{
	// Token: 0x02000010 RID: 16
	public class TraceManager
	{
		// Token: 0x06000061 RID: 97 RVA: 0x0000375C File Offset: 0x0000195C
		internal TraceManager()
		{
			this.defaultLogFolder = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Microsoft\\Care Suite\\Windows Device Recovery Tool\\Traces\\";
			this.currentTracingLevel = SourceLevels.All;
			this.Tracers = new List<IThreadSafeTracer>();
			AppDomain.CurrentDomain.ProcessExit += this.OnCurrentDomainProcessExit;
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000062 RID: 98 RVA: 0x000037C0 File Offset: 0x000019C0
		public static TraceManager Instance
		{
			get
			{
				if (TraceManager.instance == null)
				{
					lock (TraceManager.StaticSyncRoot)
					{
						if (TraceManager.instance == null)
						{
							TraceManager.instance = new TraceManager();
						}
					}
				}
				return TraceManager.instance;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000063 RID: 99 RVA: 0x0000383C File Offset: 0x00001A3C
		// (set) Token: 0x06000064 RID: 100 RVA: 0x00003853 File Offset: 0x00001A53
		internal ITraceWriter MainTraceWriter { get; private set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000065 RID: 101 RVA: 0x0000385C File Offset: 0x00001A5C
		// (set) Token: 0x06000066 RID: 102 RVA: 0x00003873 File Offset: 0x00001A73
		internal List<IThreadSafeTracer> Tracers { get; private set; }

		// Token: 0x06000067 RID: 103 RVA: 0x0000387C File Offset: 0x00001A7C
		private void RegisterDiagnosticTraceWriter(string logPath, string logNamePrefix)
		{
			ITraceWriter traceWriter = new DiagnosticLogTextWriter(logPath, logNamePrefix);
			lock (this.syncRoot)
			{
				try
				{
					if (this.MainTraceWriter != null)
					{
						this.MainTraceWriter.Close();
					}
					TraceListener traceListener = traceWriter as TraceListener;
					foreach (IThreadSafeTracer threadSafeTracer in this.Tracers)
					{
						threadSafeTracer.AddTraceListener(traceListener);
					}
					this.MainTraceWriter = traceWriter;
					Tracer<TraceManager>.WriteInformation("New diagnostic trace writer registered.");
				}
				catch (Exception ex)
				{
					Tracer<TraceManager>.WriteError(ex, "Could not register diagnostic trace writer.", new object[0]);
					throw new InvalidOperationException("Could not register diagnostic trace writer.", ex);
				}
			}
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00003984 File Offset: 0x00001B84
		public void EnableDiagnosticLogs(string logPath, string logNamePrefix)
		{
			if (this.MainTraceWriter == null)
			{
				this.RegisterDiagnosticTraceWriter(logPath, logNamePrefix);
			}
			lock (this.syncRoot)
			{
				if (this.MainTraceWriter == null)
				{
					throw new InvalidOperationException("RegisterDiagnosticTraceWriter must be called before using this method.");
				}
				try
				{
					if (string.IsNullOrEmpty(this.MainTraceWriter.LogFilePath))
					{
						this.MainTraceWriter.ChangeLogFolder(this.defaultLogFolder);
					}
					foreach (IThreadSafeTracer threadSafeTracer in this.Tracers)
					{
						threadSafeTracer.EnableTracing();
					}
					this.currentTracingLevel = SourceLevels.All;
					Tracer<TraceManager>.WriteInformation("Diagnostic logs enabled.");
				}
				catch (Exception ex)
				{
					Tracer<TraceManager>.WriteError(ex, "Could not enable diagnostic logs.", new object[0]);
					throw new InvalidOperationException("Could not enable diagnostic logs.", ex);
				}
			}
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00003ABC File Offset: 0x00001CBC
		public void DisableDiagnosticLogs(bool removeCurrentLogFile)
		{
			lock (this.syncRoot)
			{
				if (this.MainTraceWriter == null)
				{
					throw new InvalidOperationException("RegisterDiagnosticTraceWriter must be called before using this method.");
				}
				try
				{
					if (removeCurrentLogFile && !string.IsNullOrEmpty(this.MainTraceWriter.LogFilePath))
					{
						string logFilePath = this.MainTraceWriter.LogFilePath;
						this.MainTraceWriter.Close();
						File.Delete(logFilePath);
						this.MainTraceWriter = null;
					}
					foreach (IThreadSafeTracer threadSafeTracer in this.Tracers)
					{
						threadSafeTracer.DisableTracing();
					}
					this.currentTracingLevel = SourceLevels.Off;
					Tracer<TraceManager>.WriteInformation("Diagnostic logs disabled.");
				}
				catch (Exception ex)
				{
					Tracer<TraceManager>.WriteError(ex, "Could not disable diagnostic logs.", new object[0]);
					throw new InvalidOperationException("Could not disable diagnostic logs.", ex);
				}
			}
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00003BF8 File Offset: 0x00001DF8
		public void ChangeDiagnosticLogFolder(string newPath)
		{
			lock (this.syncRoot)
			{
				if (this.MainTraceWriter == null)
				{
					throw new InvalidOperationException("RegisterDiagnosticTraceWriter must be called before using this method.");
				}
				try
				{
					this.MainTraceWriter.ChangeLogFolder(newPath);
					Tracer<TraceManager>.WriteInformation("Diagnostic logs folder changed.");
				}
				catch (Exception ex)
				{
					Tracer<TraceManager>.WriteError(ex, "Could not change diagnostic logs folder.", new object[0]);
					throw new InvalidOperationException("Could not change diagnostic logs folder.", ex);
				}
			}
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00003CA4 File Offset: 0x00001EA4
		public void RemoveDiagnosticLogs(string directoryPath, string appNamePrefix, bool traceEnabled)
		{
			Tracer<TraceManager>.WriteInformation("Remove diagnostic logs.");
			lock (this.syncRoot)
			{
				string[] files = Directory.GetFiles(directoryPath);
				foreach (string text in files)
				{
					try
					{
						File.Delete(text);
						Tracer<TraceManager>.WriteInformation("Succesfully removed file: {0}.", new object[]
						{
							text
						});
					}
					catch (Exception error)
					{
						Tracer<TraceManager>.WriteError(error, "Following file could not be deleted: {0}.", new object[]
						{
							text
						});
					}
				}
			}
			if (!traceEnabled && this.MainTraceWriter != null)
			{
				this.DisableDiagnosticLogs(true);
			}
			Tracer<TraceManager>.WriteInformation("Finished removing diagnostic logs.");
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00003DA4 File Offset: 0x00001FA4
		public void MoveDiagnosticLogFile(string newPath)
		{
			lock (this.syncRoot)
			{
				if (this.MainTraceWriter == null)
				{
					throw new InvalidOperationException("RegisterDiagnosticTraceWriter must be called before using this method.");
				}
				if (string.IsNullOrEmpty(this.MainTraceWriter.LogFilePath))
				{
					throw new InvalidOperationException("Current diagnostic log file does not exist. There is nothing to be moved.");
				}
				try
				{
					string logFilePath = this.MainTraceWriter.LogFilePath;
					this.MainTraceWriter.Close();
					Directory.CreateDirectory(newPath);
					File.Move(logFilePath, Path.Combine(newPath, Path.GetFileName(logFilePath)));
					this.MainTraceWriter.ChangeLogFolder(newPath);
					Tracer<TraceManager>.WriteInformation("Diagnostic logs folder changed.");
				}
				catch (Exception ex)
				{
					Tracer<TraceManager>.WriteError(ex, "Could not move diagnostic logs file", new object[0]);
					throw new InvalidOperationException("Could not move diagnostic logs file.", ex);
				}
			}
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00003EAC File Offset: 0x000020AC
		internal IThreadSafeTracer CreateTraceSource(string sourceName)
		{
			IThreadSafeTracer result;
			try
			{
				lock (this.syncRoot)
				{
					ThreadSafeTracer threadSafeTracer = new ThreadSafeTracer(sourceName, this.currentTracingLevel);
					if (this.MainTraceWriter != null)
					{
						TraceListener traceListener = this.MainTraceWriter as TraceListener;
						if (traceListener == null)
						{
							traceListener = new TraceListenerAdapter(this.MainTraceWriter);
						}
						threadSafeTracer.AddTraceListener(traceListener);
					}
					this.Tracers.Add(threadSafeTracer);
					result = threadSafeTracer;
				}
			}
			catch (Exception ex)
			{
				Trace.WriteLine("Could not create new tracer. Error: " + ex.Message);
				throw new InvalidOperationException("Could not create new tracer.", ex);
			}
			return result;
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00003F84 File Offset: 0x00002184
		private void OnCurrentDomainProcessExit(object sender, EventArgs e)
		{
			try
			{
				lock (this.syncRoot)
				{
					if (this.MainTraceWriter != null)
					{
						this.MainTraceWriter.Close();
					}
					foreach (IThreadSafeTracer threadSafeTracer in this.Tracers)
					{
						ThreadSafeTracer threadSafeTracer2 = (ThreadSafeTracer)threadSafeTracer;
						threadSafeTracer2.Close();
					}
					this.Tracers.Clear();
				}
			}
			catch (Exception ex)
			{
				Trace.WriteLine("TraceManager OnCurrentDomainProcessExit catches:" + ex.Message);
				throw;
			}
		}

		// Token: 0x04000014 RID: 20
		private static readonly object StaticSyncRoot = new object();

		// Token: 0x04000015 RID: 21
		private static TraceManager instance;

		// Token: 0x04000016 RID: 22
		private readonly object syncRoot = new object();

		// Token: 0x04000017 RID: 23
		private readonly string defaultLogFolder;

		// Token: 0x04000018 RID: 24
		private SourceLevels currentTracingLevel;
	}
}
