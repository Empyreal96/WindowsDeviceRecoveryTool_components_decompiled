using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Microsoft.WindowsPhone.ImageUpdate.Tools.Common
{
	// Token: 0x02000034 RID: 52
	public static class LogUtil
	{
		// Token: 0x0600015F RID: 351
		[DllImport("UpdateDLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto, SetLastError = true)]
		private static extern void IU_LogTo([MarshalAs(UnmanagedType.FunctionPtr)] LogUtil.InteropLogString ErrorMsgHandler, [MarshalAs(UnmanagedType.FunctionPtr)] LogUtil.InteropLogString WarningMsgHandler, [MarshalAs(UnmanagedType.FunctionPtr)] LogUtil.InteropLogString InfoMsgHandler, [MarshalAs(UnmanagedType.FunctionPtr)] LogUtil.InteropLogString DebugMsgHandler);

		// Token: 0x06000160 RID: 352 RVA: 0x000087A0 File Offset: 0x000069A0
		static LogUtil()
		{
			LogUtil._msgPrefix.Add(LogUtil.MessageLevel.Debug, "diagnostic");
			LogUtil._msgPrefix.Add(LogUtil.MessageLevel.Message, "info");
			LogUtil._msgPrefix.Add(LogUtil.MessageLevel.Warning, "warning ");
			LogUtil._msgPrefix.Add(LogUtil.MessageLevel.Error, "fatal error ");
			LogUtil._msgColor.Add(LogUtil.MessageLevel.Debug, ConsoleColor.DarkGray);
			LogUtil._msgColor.Add(LogUtil.MessageLevel.Message, ConsoleColor.Gray);
			LogUtil._msgColor.Add(LogUtil.MessageLevel.Warning, ConsoleColor.Yellow);
			LogUtil._msgColor.Add(LogUtil.MessageLevel.Error, ConsoleColor.Red);
			LogUtil.IULogTo(new LogUtil.InteropLogString(LogUtil.Error), new LogUtil.InteropLogString(LogUtil.Warning), new LogUtil.InteropLogString(LogUtil.Message), new LogUtil.InteropLogString(LogUtil.Diagnostic));
		}

		// Token: 0x06000161 RID: 353 RVA: 0x00008888 File Offset: 0x00006A88
		private static void LogMessage(LogUtil.MessageLevel level, string message)
		{
			if (level != LogUtil.MessageLevel.Debug || LogUtil._verbose)
			{
				string[] array = message.Split(new char[]
				{
					'\r',
					'\n'
				}, StringSplitOptions.RemoveEmptyEntries);
				foreach (string arg in array)
				{
					ConsoleColor foregroundColor = Console.ForegroundColor;
					Console.ForegroundColor = LogUtil._msgColor[level];
					Console.WriteLine("{0}: {1}", LogUtil._msgPrefix[level], arg);
					Console.ForegroundColor = foregroundColor;
				}
			}
		}

		// Token: 0x06000162 RID: 354 RVA: 0x00008908 File Offset: 0x00006B08
		private static void LogMessage(LogUtil.MessageLevel level, string format, params object[] args)
		{
			LogUtil.LogMessage(level, string.Format(format, args));
		}

		// Token: 0x06000163 RID: 355 RVA: 0x00008917 File Offset: 0x00006B17
		public static void SetVerbosity(bool enabled)
		{
			LogUtil._verbose = enabled;
		}

		// Token: 0x06000164 RID: 356 RVA: 0x0000891F File Offset: 0x00006B1F
		public static void Error(string message)
		{
			LogUtil.LogMessage(LogUtil.MessageLevel.Error, message);
		}

		// Token: 0x06000165 RID: 357 RVA: 0x00008928 File Offset: 0x00006B28
		public static void Error(string format, params object[] args)
		{
			LogUtil.LogMessage(LogUtil.MessageLevel.Error, format, args);
		}

		// Token: 0x06000166 RID: 358 RVA: 0x00008932 File Offset: 0x00006B32
		public static void Warning(string message)
		{
			LogUtil.LogMessage(LogUtil.MessageLevel.Warning, message);
		}

		// Token: 0x06000167 RID: 359 RVA: 0x0000893B File Offset: 0x00006B3B
		public static void Warning(string format, params object[] args)
		{
			LogUtil.LogMessage(LogUtil.MessageLevel.Warning, format, args);
		}

		// Token: 0x06000168 RID: 360 RVA: 0x00008945 File Offset: 0x00006B45
		public static void Message(string message)
		{
			LogUtil.LogMessage(LogUtil.MessageLevel.Message, message);
		}

		// Token: 0x06000169 RID: 361 RVA: 0x0000894E File Offset: 0x00006B4E
		public static void Message(string format, params object[] args)
		{
			LogUtil.LogMessage(LogUtil.MessageLevel.Message, format, args);
		}

		// Token: 0x0600016A RID: 362 RVA: 0x00008958 File Offset: 0x00006B58
		public static void Diagnostic(string message)
		{
			LogUtil.LogMessage(LogUtil.MessageLevel.Debug, message);
		}

		// Token: 0x0600016B RID: 363 RVA: 0x00008961 File Offset: 0x00006B61
		public static void Diagnostic(string format, params object[] args)
		{
			LogUtil.LogMessage(LogUtil.MessageLevel.Debug, format, args);
		}

		// Token: 0x0600016C RID: 364 RVA: 0x0000896B File Offset: 0x00006B6B
		public static void LogCopyright()
		{
			Console.WriteLine(CommonUtils.GetCopyrightString());
		}

		// Token: 0x0600016D RID: 365 RVA: 0x00008977 File Offset: 0x00006B77
		public static void IULogTo(LogUtil.InteropLogString errorLogger, LogUtil.InteropLogString warningLogger, LogUtil.InteropLogString msgLogger, LogUtil.InteropLogString debugLogger)
		{
			LogUtil._iuErrorLogger = errorLogger;
			LogUtil._iuWarningLogger = warningLogger;
			LogUtil._iuMsgLogger = msgLogger;
			LogUtil._iuDebugLogger = debugLogger;
			LogUtil.IU_LogTo(errorLogger, warningLogger, msgLogger, debugLogger);
		}

		// Token: 0x040000D5 RID: 213
		private static bool _verbose = false;

		// Token: 0x040000D6 RID: 214
		private static Dictionary<LogUtil.MessageLevel, string> _msgPrefix = new Dictionary<LogUtil.MessageLevel, string>();

		// Token: 0x040000D7 RID: 215
		private static Dictionary<LogUtil.MessageLevel, ConsoleColor> _msgColor = new Dictionary<LogUtil.MessageLevel, ConsoleColor>();

		// Token: 0x040000D8 RID: 216
		private static LogUtil.InteropLogString _iuErrorLogger = null;

		// Token: 0x040000D9 RID: 217
		private static LogUtil.InteropLogString _iuWarningLogger = null;

		// Token: 0x040000DA RID: 218
		private static LogUtil.InteropLogString _iuMsgLogger = null;

		// Token: 0x040000DB RID: 219
		private static LogUtil.InteropLogString _iuDebugLogger = null;

		// Token: 0x02000035 RID: 53
		private enum MessageLevel
		{
			// Token: 0x040000DD RID: 221
			Error,
			// Token: 0x040000DE RID: 222
			Warning,
			// Token: 0x040000DF RID: 223
			Message,
			// Token: 0x040000E0 RID: 224
			Debug
		}

		// Token: 0x02000036 RID: 54
		// (Invoke) Token: 0x0600016F RID: 367
		public delegate void InteropLogString([MarshalAs(UnmanagedType.LPWStr)] string outputStr);
	}
}
