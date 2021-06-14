using System;
using System.Collections.Generic;
using ClickerUtilityLibrary.Misc;
using Microsoft.WindowsDeviceRecoveryTool.Common.Tracing;

namespace Microsoft.WindowsDeviceRecoveryTool.FawkesAdaptation.Services
{
	// Token: 0x02000009 RID: 9
	internal class FawkesLogger : ILogger
	{
		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000054 RID: 84 RVA: 0x000035A7 File Offset: 0x000017A7
		// (set) Token: 0x06000055 RID: 85 RVA: 0x000035AF File Offset: 0x000017AF
		internal List<string> LoggedErrorMessages { get; private set; }

		// Token: 0x06000056 RID: 86 RVA: 0x000035B8 File Offset: 0x000017B8
		public FawkesLogger()
		{
			this.LoggedErrorMessages = new List<string>();
		}

		// Token: 0x06000057 RID: 87 RVA: 0x000035CB File Offset: 0x000017CB
		public void LogDebug(string message)
		{
			if (message.Contains("Error"))
			{
				this.LoggedErrorMessages.Add(message);
				Tracer<FawkesLogger>.WriteError(message, new object[0]);
				return;
			}
			Tracer<FawkesLogger>.WriteInformation(message);
		}

		// Token: 0x06000058 RID: 88 RVA: 0x000035F9 File Offset: 0x000017F9
		public void LogInfo(string message)
		{
			Tracer<FawkesLogger>.WriteInformation(message);
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00003601 File Offset: 0x00001801
		public void LogError(string message)
		{
			this.LoggedErrorMessages.Add(message);
			Tracer<FawkesLogger>.WriteError(message, new object[0]);
		}
	}
}
