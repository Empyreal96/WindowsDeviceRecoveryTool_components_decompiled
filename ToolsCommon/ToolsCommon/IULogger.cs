using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Microsoft.WindowsPhone.ImageUpdate.Tools.Common
{
	// Token: 0x02000064 RID: 100
	public class IULogger : IDeploymentLogger
	{
		// Token: 0x0600026A RID: 618 RVA: 0x0000B7A0 File Offset: 0x000099A0
		public IULogger()
		{
			this.MinLogLevel = LoggingLevel.Debug;
			this.LoggingMessage.Add(LoggingLevel.Debug, "DEBUG");
			this.LoggingMessage.Add(LoggingLevel.Info, "INFO");
			this.LoggingMessage.Add(LoggingLevel.Warning, "WARNING");
			this.LoggingMessage.Add(LoggingLevel.Error, "ERROR");
			this.LoggingFunctions.Add(LoggingLevel.Debug, new LogString(IULogger.LogToConsole));
			this.LoggingFunctions.Add(LoggingLevel.Info, new LogString(IULogger.LogToConsole));
			this.LoggingFunctions.Add(LoggingLevel.Warning, new LogString(IULogger.LogToError));
			this.LoggingFunctions.Add(LoggingLevel.Error, new LogString(IULogger.LogToError));
			this.LoggingColors.Add(LoggingLevel.Debug, ConsoleColor.DarkGray);
			this.LoggingColors.Add(LoggingLevel.Info, ConsoleColor.Gray);
			this.LoggingColors.Add(LoggingLevel.Warning, ConsoleColor.Yellow);
			this.LoggingColors.Add(LoggingLevel.Error, ConsoleColor.Red);
		}

		// Token: 0x0600026B RID: 619 RVA: 0x0000B8B5 File Offset: 0x00009AB5
		public static void LogToConsole(string format, params object[] list)
		{
			if (list.Length != 0)
			{
				Console.WriteLine(string.Format(CultureInfo.CurrentCulture, format, list));
				return;
			}
			Console.WriteLine(format);
		}

		// Token: 0x0600026C RID: 620 RVA: 0x0000B8D4 File Offset: 0x00009AD4
		public static void LogToError(string format, params object[] list)
		{
			if (list.Length != 0)
			{
				Console.Error.WriteLine(string.Format(CultureInfo.CurrentCulture, format, list));
				return;
			}
			Console.Error.WriteLine(format);
		}

		// Token: 0x0600026D RID: 621 RVA: 0x0000B8FD File Offset: 0x00009AFD
		public static void LogToNull(string format, params object[] list)
		{
		}

		// Token: 0x0600026E RID: 622 RVA: 0x0000B8FF File Offset: 0x00009AFF
		public void SetLoggingLevel(LoggingLevel level)
		{
			this.MinLogLevel = level;
		}

		// Token: 0x0600026F RID: 623 RVA: 0x0000B908 File Offset: 0x00009B08
		public void SetLogFunction(LoggingLevel level, LogString logFunc)
		{
			if (logFunc == null)
			{
				this.LoggingFunctions[level] = new LogString(IULogger.LogToNull);
				return;
			}
			this.LoggingFunctions[level] = logFunc;
		}

		// Token: 0x06000270 RID: 624 RVA: 0x0000B934 File Offset: 0x00009B34
		public void Log(LoggingLevel level, string format, params object[] list)
		{
			if (level >= this.MinLogLevel)
			{
				ConsoleColor foregroundColor = Console.ForegroundColor;
				Console.ForegroundColor = this.LoggingColors[level];
				this.LoggingFunctions[level](format, list);
				Console.ForegroundColor = foregroundColor;
			}
		}

		// Token: 0x06000271 RID: 625 RVA: 0x0000B97A File Offset: 0x00009B7A
		public void LogException(Exception exp)
		{
			this.LogException(exp, LoggingLevel.Error);
		}

		// Token: 0x06000272 RID: 626 RVA: 0x0000B984 File Offset: 0x00009B84
		public void LogException(Exception exp, LoggingLevel level)
		{
			StringBuilder stringBuilder = new StringBuilder();
			StackTrace stackTrace = new StackTrace(exp, true);
			if (stackTrace.FrameCount > 0)
			{
				StackFrame frame = stackTrace.GetFrame(stackTrace.FrameCount - 1);
				if (frame != null)
				{
					string arg = string.Format("{0}({1},{2}):", frame.GetFileName(), frame.GetFileLineNumber(), frame.GetFileColumnNumber());
					stringBuilder.Append(string.Format("{0}{1}", arg, Environment.NewLine));
				}
			}
			stringBuilder.Append(string.Format("{0}: {1}{2}", this.LoggingMessage[level], "0x" + Marshal.GetHRForException(exp).ToString("X"), Environment.NewLine));
			stringBuilder.Append(string.Format("{0}:{1}", Path.GetFileNameWithoutExtension(Process.GetCurrentProcess().ProcessName), Environment.NewLine));
			stringBuilder.Append(string.Format("EXCEPTION: {0}{1}", exp.Message, Environment.NewLine));
			stringBuilder.Append(string.Format("STACKTRACE:{1}{0}{1}", exp.StackTrace, Environment.NewLine));
			this.Log(level, stringBuilder.ToString(), new object[0]);
		}

		// Token: 0x06000273 RID: 627 RVA: 0x0000BAAA File Offset: 0x00009CAA
		public void LogError(string format, params object[] list)
		{
			this.Log(LoggingLevel.Error, format, list);
		}

		// Token: 0x06000274 RID: 628 RVA: 0x0000BAB5 File Offset: 0x00009CB5
		public void LogWarning(string format, params object[] list)
		{
			this.Log(LoggingLevel.Warning, format, list);
		}

		// Token: 0x06000275 RID: 629 RVA: 0x0000BAC0 File Offset: 0x00009CC0
		public void LogInfo(string format, params object[] list)
		{
			this.Log(LoggingLevel.Info, format, list);
		}

		// Token: 0x06000276 RID: 630 RVA: 0x0000BACB File Offset: 0x00009CCB
		public void LogDebug(string format, params object[] list)
		{
			this.Log(LoggingLevel.Debug, format, list);
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000277 RID: 631 RVA: 0x0000BAD6 File Offset: 0x00009CD6
		// (set) Token: 0x06000278 RID: 632 RVA: 0x0000BAE4 File Offset: 0x00009CE4
		public LogString ErrorLogger
		{
			get
			{
				return this.LoggingFunctions[LoggingLevel.Error];
			}
			set
			{
				this.SetLogFunction(LoggingLevel.Error, value);
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000279 RID: 633 RVA: 0x0000BAEE File Offset: 0x00009CEE
		// (set) Token: 0x0600027A RID: 634 RVA: 0x0000BAFC File Offset: 0x00009CFC
		public LogString WarningLogger
		{
			get
			{
				return this.LoggingFunctions[LoggingLevel.Warning];
			}
			set
			{
				this.SetLogFunction(LoggingLevel.Warning, value);
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x0600027B RID: 635 RVA: 0x0000BB06 File Offset: 0x00009D06
		// (set) Token: 0x0600027C RID: 636 RVA: 0x0000BB14 File Offset: 0x00009D14
		public LogString InformationLogger
		{
			get
			{
				return this.LoggingFunctions[LoggingLevel.Info];
			}
			set
			{
				this.SetLogFunction(LoggingLevel.Info, value);
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x0600027D RID: 637 RVA: 0x0000BB1E File Offset: 0x00009D1E
		// (set) Token: 0x0600027E RID: 638 RVA: 0x0000BB2C File Offset: 0x00009D2C
		public LogString DebugLogger
		{
			get
			{
				return this.LoggingFunctions[LoggingLevel.Debug];
			}
			set
			{
				this.SetLogFunction(LoggingLevel.Debug, value);
			}
		}

		// Token: 0x04000179 RID: 377
		private LoggingLevel MinLogLevel;

		// Token: 0x0400017A RID: 378
		private Dictionary<LoggingLevel, string> LoggingMessage = new Dictionary<LoggingLevel, string>();

		// Token: 0x0400017B RID: 379
		private Dictionary<LoggingLevel, LogString> LoggingFunctions = new Dictionary<LoggingLevel, LogString>();

		// Token: 0x0400017C RID: 380
		private Dictionary<LoggingLevel, ConsoleColor> LoggingColors = new Dictionary<LoggingLevel, ConsoleColor>();
	}
}
