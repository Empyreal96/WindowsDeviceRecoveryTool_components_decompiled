using System;
using System.Collections.Generic;
using Microsoft.WindowsDeviceRecoveryTool.Model;

namespace Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Services
{
	// Token: 0x0200003A RID: 58
	public interface IManufacturerDataProvider
	{
		// Token: 0x06000301 RID: 769
		List<ManufacturerInfo> GetAdaptationsData();
	}
}
