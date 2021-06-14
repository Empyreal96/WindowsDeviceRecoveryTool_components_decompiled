using System;
using System.Collections.ObjectModel;

namespace Microsoft.WindowsDeviceRecoveryTool.Framework
{
	// Token: 0x0200004A RID: 74
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class RegionAttribute : Attribute
	{
		// Token: 0x0600028F RID: 655 RVA: 0x0000F61E File Offset: 0x0000D81E
		public RegionAttribute(params string[] names)
		{
			this.Names = new ReadOnlyCollection<string>(names);
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000290 RID: 656 RVA: 0x0000F638 File Offset: 0x0000D838
		// (set) Token: 0x06000291 RID: 657 RVA: 0x0000F64F File Offset: 0x0000D84F
		public ReadOnlyCollection<string> Names { get; private set; }
	}
}
