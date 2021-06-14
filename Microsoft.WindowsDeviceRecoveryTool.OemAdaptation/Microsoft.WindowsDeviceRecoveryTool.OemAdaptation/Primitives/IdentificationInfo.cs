using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.WindowsDeviceRecoveryTool.OemAdaptation.Primitives
{
	// Token: 0x02000004 RID: 4
	public sealed class IdentificationInfo
	{
		// Token: 0x06000005 RID: 5 RVA: 0x000020F0 File Offset: 0x000002F0
		public IdentificationInfo(IEnumerable<string> deviceReturnedValues)
		{
			if (deviceReturnedValues == null)
			{
				throw new ArgumentNullException("deviceReturnedValues");
			}
			string[] array = deviceReturnedValues.ToArray<string>();
			if (array.Length == 0)
			{
				throw new ArgumentException("deviceReturnedValues should have at least one element");
			}
			this.DeviceReturnedValues = array;
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000006 RID: 6 RVA: 0x0000212F File Offset: 0x0000032F
		// (set) Token: 0x06000007 RID: 7 RVA: 0x00002137 File Offset: 0x00000337
		public string[] DeviceReturnedValues { get; private set; }
	}
}
