using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Microsoft.WindowsDeviceRecoveryTool.Common.Tracing
{
	// Token: 0x02000011 RID: 17
	[CompilerGenerated]
	public static class Tracer<T>
	{
		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000071 RID: 113 RVA: 0x000040A0 File Offset: 0x000022A0
		// (set) Token: 0x06000072 RID: 114 RVA: 0x000040B6 File Offset: 0x000022B6
		internal static IThreadSafeTracer InternalTracer { get; set; } = TraceManager.Instance.CreateTraceSource(typeof(T).FullName);

		// Token: 0x06000073 RID: 115 RVA: 0x000040BE File Offset: 0x000022BE
		public static void LogEntry([CallerMemberName] string callerMemberName = "")
		{
			Tracer<T>.WriteEvent(TraceEventType.Information, null, string.Format("{0}()\tBEGIN", callerMemberName));
		}

		// Token: 0x06000074 RID: 116 RVA: 0x000040D4 File Offset: 0x000022D4
		public static void LogExit([CallerMemberName] string callerMemberName = "")
		{
			Tracer<T>.WriteEvent(TraceEventType.Information, null, string.Format("{0}()\tEND", callerMemberName));
		}

		// Token: 0x06000075 RID: 117 RVA: 0x000040EA File Offset: 0x000022EA
		public static void WriteError(string format, params object[] args)
		{
			Tracer<T>.WriteEvent(TraceEventType.Error, null, string.Format(CultureInfo.CurrentCulture, format, args));
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00004101 File Offset: 0x00002301
		public static void WriteError(IFormatProvider formatProvider, string format, params object[] args)
		{
			Tracer<T>.WriteEvent(TraceEventType.Error, null, string.Format(formatProvider, format, args));
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00004114 File Offset: 0x00002314
		public static void WriteError(Exception error)
		{
			Tracer<T>.WriteEvent(TraceEventType.Error, error.ToString(), null);
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00004125 File Offset: 0x00002325
		public static void WriteError(Exception error, string format, params object[] args)
		{
			Tracer<T>.WriteEvent(TraceEventType.Error, error.ToString(), string.Format(CultureInfo.CurrentCulture, format, args));
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00004141 File Offset: 0x00002341
		public static void WriteError(Exception error, IFormatProvider formatProvider, string format, params object[] args)
		{
			Tracer<T>.WriteEvent(TraceEventType.Error, error.ToString(), string.Format(formatProvider, format, args));
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00004159 File Offset: 0x00002359
		public static void WriteInformation(string text)
		{
			Tracer<T>.WriteEvent(TraceEventType.Information, null, text);
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00004165 File Offset: 0x00002365
		public static void WriteInformation(string format, params object[] args)
		{
			Tracer<T>.WriteEvent(TraceEventType.Information, null, string.Format(CultureInfo.CurrentCulture, format, args));
		}

		// Token: 0x0600007C RID: 124 RVA: 0x0000417C File Offset: 0x0000237C
		public static void WriteInformation(IFormatProvider formatProvider, string format, params object[] args)
		{
			Tracer<T>.WriteEvent(TraceEventType.Information, null, string.Format(formatProvider, format, args));
		}

		// Token: 0x0600007D RID: 125 RVA: 0x0000418F File Offset: 0x0000238F
		public static void WriteInformation(Exception error, string format, params object[] args)
		{
			Tracer<T>.WriteEvent(TraceEventType.Information, error.ToString(), string.Format(CultureInfo.CurrentCulture, format, args));
		}

		// Token: 0x0600007E RID: 126 RVA: 0x000041AB File Offset: 0x000023AB
		public static void WriteInformation(Exception error, IFormatProvider formatProvider, string format, params object[] args)
		{
			Tracer<T>.WriteEvent(TraceEventType.Information, error.ToString(), string.Format(formatProvider, format, args));
		}

		// Token: 0x0600007F RID: 127 RVA: 0x000041C3 File Offset: 0x000023C3
		public static void WriteVerbose(string format, params object[] args)
		{
			Tracer<T>.WriteEvent(TraceEventType.Verbose, null, string.Format(CultureInfo.CurrentCulture, format, args));
		}

		// Token: 0x06000080 RID: 128 RVA: 0x000041DB File Offset: 0x000023DB
		public static void WriteVerbose(IFormatProvider formatProvider, string format, params object[] args)
		{
			Tracer<T>.WriteEvent(TraceEventType.Verbose, null, string.Format(formatProvider, format, args));
		}

		// Token: 0x06000081 RID: 129 RVA: 0x000041EF File Offset: 0x000023EF
		public static void WriteVerbose(Exception error)
		{
			Tracer<T>.WriteEvent(TraceEventType.Verbose, error.ToString(), null);
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00004201 File Offset: 0x00002401
		public static void WriteVerbose(Exception error, string format, params object[] args)
		{
			Tracer<T>.WriteEvent(TraceEventType.Verbose, error.ToString(), string.Format(CultureInfo.CurrentCulture, format, args));
		}

		// Token: 0x06000083 RID: 131 RVA: 0x0000421E File Offset: 0x0000241E
		public static void WriteVerbose(Exception error, IFormatProvider formatProvider, string format, params object[] args)
		{
			Tracer<T>.WriteEvent(TraceEventType.Verbose, error.ToString(), string.Format(formatProvider, format, args));
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00004237 File Offset: 0x00002437
		public static void WriteWarning(string format, params object[] args)
		{
			Tracer<T>.WriteEvent(TraceEventType.Warning, null, string.Format(CultureInfo.CurrentCulture, format, args));
		}

		// Token: 0x06000085 RID: 133 RVA: 0x0000424E File Offset: 0x0000244E
		public static void WriteWarning(IFormatProvider formatProvider, string format, params object[] args)
		{
			Tracer<T>.WriteEvent(TraceEventType.Warning, null, string.Format(formatProvider, format, args));
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00004261 File Offset: 0x00002461
		public static void WriteWarning(Exception error)
		{
			Tracer<T>.WriteEvent(TraceEventType.Warning, error.ToString(), null);
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00004272 File Offset: 0x00002472
		public static void WriteWarning(Exception error, string format, params object[] args)
		{
			Tracer<T>.WriteEvent(TraceEventType.Warning, error.ToString(), string.Format(CultureInfo.CurrentCulture, format, args));
		}

		// Token: 0x06000088 RID: 136 RVA: 0x0000428E File Offset: 0x0000248E
		public static void WriteWarning(Exception error, IFormatProvider formatProvider, string format, params object[] args)
		{
			Tracer<T>.WriteEvent(TraceEventType.Warning, error.ToString(), string.Format(formatProvider, format, args));
		}

		// Token: 0x06000089 RID: 137 RVA: 0x000042A8 File Offset: 0x000024A8
		private static void WriteEvent(TraceEventType eventType, string errorText, string messageText)
		{
			string fileName = Path.GetFileName(typeof(T).Assembly.Location);
			Thread currentThread = Thread.CurrentThread;
			Tracer<T>.CurrentProcessInfo instance = Tracer<T>.CurrentProcessInfo.Instance;
			Tracer<T>.InternalTracer.TraceData(eventType, new object[]
			{
				instance.Id,
				instance.Name,
				currentThread.ManagedThreadId,
				currentThread.Name,
				fileName,
				messageText,
				errorText
			});
		}

		// Token: 0x02000012 RID: 18
		private sealed class CurrentProcessInfo
		{
			// Token: 0x1700000F RID: 15
			// (get) Token: 0x0600008A RID: 138 RVA: 0x0000432C File Offset: 0x0000252C
			public static Tracer<T>.CurrentProcessInfo Instance
			{
				get
				{
					if (Tracer<T>.CurrentProcessInfo.instance == null)
					{
						Process currentProcess = Process.GetCurrentProcess();
						Tracer<T>.CurrentProcessInfo currentProcessInfo = new Tracer<T>.CurrentProcessInfo();
						currentProcessInfo.Id = currentProcess.Id;
						currentProcessInfo.Name = currentProcess.ProcessName;
						Tracer<T>.CurrentProcessInfo value = currentProcessInfo;
						Interlocked.CompareExchange<Tracer<T>.CurrentProcessInfo>(ref Tracer<T>.CurrentProcessInfo.instance, value, null);
					}
					return Tracer<T>.CurrentProcessInfo.instance;
				}
			}

			// Token: 0x0400001C RID: 28
			public int Id;

			// Token: 0x0400001D RID: 29
			public string Name;

			// Token: 0x0400001E RID: 30
			private static Tracer<T>.CurrentProcessInfo instance;
		}
	}
}
