using System;

namespace Microsoft.Tools.DeviceUpdate.DeviceUtils
{
	// Token: 0x02000003 RID: 3
	public class DevicePropertyAttribute : Attribute
	{
		// Token: 0x06000003 RID: 3 RVA: 0x00002063 File Offset: 0x00000263
		public DevicePropertyAttribute(string name)
		{
			this.Name = name;
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000004 RID: 4 RVA: 0x00002072 File Offset: 0x00000272
		// (set) Token: 0x06000005 RID: 5 RVA: 0x0000207A File Offset: 0x0000027A
		public string Name { get; private set; }
	}
}
