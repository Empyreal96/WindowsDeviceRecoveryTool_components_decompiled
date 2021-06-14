using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsDeviceRecoveryTool.Core;
using Microsoft.WindowsDeviceRecoveryTool.OemAdaptation.Primitives;

namespace Microsoft.WindowsDeviceRecoveryTool.OemAdaptation
{
	// Token: 0x02000007 RID: 7
	public sealed class ManufacturerModelsCatalog
	{
		// Token: 0x06000016 RID: 22 RVA: 0x00002258 File Offset: 0x00000458
		public ManufacturerModelsCatalog(ManufacturerInfo manufacturerInfo, IEnumerable<ModelInfo> modelInfos)
		{
			if (manufacturerInfo == null)
			{
				throw new ArgumentNullException("manufacturerInfo");
			}
			if (modelInfos == null)
			{
				throw new ArgumentNullException("modelInfos");
			}
			ModelInfo[] array = modelInfos.ToArray<ModelInfo>();
			if (array.Length == 0)
			{
				throw new ArgumentException("modelInfos should have at least one element");
			}
			this.ManufacturerInfo = manufacturerInfo;
			this.Models = array;
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000017 RID: 23 RVA: 0x000022AC File Offset: 0x000004AC
		// (set) Token: 0x06000018 RID: 24 RVA: 0x000022B4 File Offset: 0x000004B4
		public ManufacturerInfo ManufacturerInfo { get; private set; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000019 RID: 25 RVA: 0x000022BD File Offset: 0x000004BD
		// (set) Token: 0x0600001A RID: 26 RVA: 0x000022C5 File Offset: 0x000004C5
		public ModelInfo[] Models { get; private set; }

		// Token: 0x0600001B RID: 27 RVA: 0x000022DB File Offset: 0x000004DB
		public DeviceDetectionInformation[] GetDeviceDetectionInformations()
		{
			return this.Models.SelectMany((ModelInfo m) => m.DetectionInfo.DeviceDetectionInformations).ToArray<DeviceDetectionInformation>();
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002360 File Offset: 0x00000560
		public bool TryGetModelInfo(string deviceReturnedVariantName, out ModelInfo modelInfo)
		{
			if (string.IsNullOrEmpty(deviceReturnedVariantName))
			{
				modelInfo = null;
				return false;
			}
			ModelInfo modelInfo2 = this.Models.FirstOrDefault((ModelInfo m) => m.Variants.Any((VariantInfo v) => v.IdentificationInfo.DeviceReturnedValues.Any((string dv) => deviceReturnedVariantName.IndexOf(dv, StringComparison.OrdinalIgnoreCase) >= 0)));
			if (modelInfo2 == null)
			{
				modelInfo = null;
				return false;
			}
			modelInfo = modelInfo2;
			return true;
		}
	}
}
