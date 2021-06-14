using System;

namespace Microsoft.WindowsDeviceRecoveryTool.FawkesAdaptation.Services
{
	// Token: 0x0200000B RID: 11
	internal class FawkesProgressData
	{
		// Token: 0x06000063 RID: 99 RVA: 0x0000377E File Offset: 0x0000197E
		internal FawkesProgressData(double? value, string message)
		{
			this.Value = value;
			this.Message = message;
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000064 RID: 100 RVA: 0x00003794 File Offset: 0x00001994
		// (set) Token: 0x06000065 RID: 101 RVA: 0x0000379C File Offset: 0x0000199C
		public double? Value { get; private set; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000066 RID: 102 RVA: 0x000037A5 File Offset: 0x000019A5
		// (set) Token: 0x06000067 RID: 103 RVA: 0x000037AD File Offset: 0x000019AD
		public string Message { get; private set; }
	}
}
