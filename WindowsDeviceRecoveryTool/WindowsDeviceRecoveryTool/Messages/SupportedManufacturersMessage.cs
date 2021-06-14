using System;
using System.Collections.Generic;
using Microsoft.WindowsDeviceRecoveryTool.Model;

namespace Microsoft.WindowsDeviceRecoveryTool.Messages
{
	// Token: 0x02000023 RID: 35
	public class SupportedManufacturersMessage
	{
		// Token: 0x06000103 RID: 259 RVA: 0x00008490 File Offset: 0x00006690
		public SupportedManufacturersMessage(List<ManufacturerInfo> adaptationsData)
		{
			this.Manufacturers = adaptationsData;
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000104 RID: 260 RVA: 0x000084A4 File Offset: 0x000066A4
		// (set) Token: 0x06000105 RID: 261 RVA: 0x000084BB File Offset: 0x000066BB
		public List<ManufacturerInfo> Manufacturers { get; private set; }
	}
}
