using System;
using System.Runtime.InteropServices;

namespace Microsoft.Windows.Flashing.Platform
{
	// Token: 0x0200003D RID: 61
	public abstract class GenericProgress
	{
		// Token: 0x06000112 RID: 274
		public abstract void RegisterProgress([In] uint Progress);

		// Token: 0x06000113 RID: 275 RVA: 0x00012490 File Offset: 0x00011890
		public GenericProgress()
		{
		}
	}
}
