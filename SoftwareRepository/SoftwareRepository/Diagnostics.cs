using System;
using System.Diagnostics.CodeAnalysis;

namespace SoftwareRepository
{
	// Token: 0x02000004 RID: 4
	internal static class Diagnostics
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		[SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "level")]
		[SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "args")]
		[SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "formatMessage")]
		internal static void Log(LogLevel level, string formatMessage, params object[] args)
		{
		}

		// Token: 0x0400000A RID: 10
		[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
		private static string Category = "SoftwareRepository";
	}
}
