using System;
using System.Diagnostics;
using System.Globalization;

namespace Microsoft.WindowsAzure.Storage.Core
{
	// Token: 0x02000057 RID: 87
	internal static class Logger
	{
		// Token: 0x06000D4E RID: 3406 RVA: 0x000311C8 File Offset: 0x0002F3C8
		static Logger()
		{
			AppDomain.CurrentDomain.DomainUnload += Logger.AppDomainUnloadEvent;
			AppDomain.CurrentDomain.ProcessExit += Logger.ProcessExitEvent;
		}

		// Token: 0x06000D4F RID: 3407 RVA: 0x00031218 File Offset: 0x0002F418
		private static void Close()
		{
			Logger.isClosed = true;
			TraceSource traceSource = Logger.traceSource;
			if (traceSource != null)
			{
				Logger.traceSource = null;
				traceSource.Close();
			}
		}

		// Token: 0x06000D50 RID: 3408 RVA: 0x00031242 File Offset: 0x0002F442
		private static void ProcessExitEvent(object sender, EventArgs e)
		{
			Logger.Close();
		}

		// Token: 0x06000D51 RID: 3409 RVA: 0x00031249 File Offset: 0x0002F449
		private static void AppDomainUnloadEvent(object sender, EventArgs e)
		{
			Logger.Close();
		}

		// Token: 0x06000D52 RID: 3410 RVA: 0x00031250 File Offset: 0x0002F450
		internal static void LogError(OperationContext operationContext, string format, params object[] args)
		{
			if (!Logger.isClosed && Logger.traceSource.Switch.ShouldTrace(TraceEventType.Error) && Logger.ShouldLog(LogLevel.Error, operationContext))
			{
				Logger.traceSource.TraceEvent(TraceEventType.Error, 1, Logger.FormatLine(operationContext, format, args));
			}
		}

		// Token: 0x06000D53 RID: 3411 RVA: 0x0003128A File Offset: 0x0002F48A
		internal static void LogWarning(OperationContext operationContext, string format, params object[] args)
		{
			if (!Logger.isClosed && Logger.traceSource.Switch.ShouldTrace(TraceEventType.Warning) && Logger.ShouldLog(LogLevel.Warning, operationContext))
			{
				Logger.traceSource.TraceEvent(TraceEventType.Warning, 2, Logger.FormatLine(operationContext, format, args));
			}
		}

		// Token: 0x06000D54 RID: 3412 RVA: 0x000312C4 File Offset: 0x0002F4C4
		internal static void LogInformational(OperationContext operationContext, string format, params object[] args)
		{
			if (!Logger.isClosed && Logger.traceSource.Switch.ShouldTrace(TraceEventType.Information) && Logger.ShouldLog(LogLevel.Informational, operationContext))
			{
				Logger.traceSource.TraceEvent(TraceEventType.Information, 3, Logger.FormatLine(operationContext, format, args));
			}
		}

		// Token: 0x06000D55 RID: 3413 RVA: 0x000312FE File Offset: 0x0002F4FE
		internal static void LogVerbose(OperationContext operationContext, string format, params object[] args)
		{
			if (!Logger.isClosed && Logger.traceSource.Switch.ShouldTrace(TraceEventType.Verbose) && Logger.ShouldLog(LogLevel.Verbose, operationContext))
			{
				Logger.traceSource.TraceEvent(TraceEventType.Verbose, 4, Logger.FormatLine(operationContext, format, args));
			}
		}

		// Token: 0x06000D56 RID: 3414 RVA: 0x0003133C File Offset: 0x0002F53C
		private static string FormatLine(OperationContext operationContext, string format, object[] args)
		{
			return string.Format(CultureInfo.InvariantCulture, "{0}: {1}", new object[]
			{
				(operationContext == null) ? "*" : operationContext.ClientRequestID,
				(args == null) ? format : string.Format(CultureInfo.InvariantCulture, format, args).Replace('\n', '.')
			});
		}

		// Token: 0x06000D57 RID: 3415 RVA: 0x00031391 File Offset: 0x0002F591
		private static bool ShouldLog(LogLevel level, OperationContext operationContext)
		{
			return operationContext == null || level <= operationContext.LogLevel;
		}

		// Token: 0x040001AA RID: 426
		private const string TraceFormat = "{0}: {1}";

		// Token: 0x040001AB RID: 427
		private static TraceSource traceSource = new TraceSource("Microsoft.WindowsAzure.Storage");

		// Token: 0x040001AC RID: 428
		private static volatile bool isClosed = false;
	}
}
