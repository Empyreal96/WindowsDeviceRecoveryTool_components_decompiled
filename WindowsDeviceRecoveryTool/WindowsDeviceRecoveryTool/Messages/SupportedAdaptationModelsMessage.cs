using System;
using System.Collections.Generic;
using Microsoft.WindowsDeviceRecoveryTool.Model;

namespace Microsoft.WindowsDeviceRecoveryTool.Messages
{
	// Token: 0x02000022 RID: 34
	public class SupportedAdaptationModelsMessage
	{
		// Token: 0x06000100 RID: 256 RVA: 0x0000845C File Offset: 0x0000665C
		public SupportedAdaptationModelsMessage(List<Phone> adaptationsModels)
		{
			this.Models = adaptationsModels;
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000101 RID: 257 RVA: 0x00008470 File Offset: 0x00006670
		// (set) Token: 0x06000102 RID: 258 RVA: 0x00008487 File Offset: 0x00006687
		public List<Phone> Models { get; private set; }
	}
}
