using System;
using Microsoft.WindowsDeviceRecoveryTool.Model;

namespace Microsoft.WindowsDeviceRecoveryTool.LogicCommon
{
	// Token: 0x02000021 RID: 33
	public class BaseSalesNameProvider : ISalesNameProvider
	{
		// Token: 0x06000124 RID: 292 RVA: 0x00007634 File Offset: 0x00005834
		public virtual string NameForVidPid(string vid, string pid)
		{
			return string.Empty;
		}

		// Token: 0x06000125 RID: 293 RVA: 0x0000764C File Offset: 0x0000584C
		public virtual string NameForString(string text)
		{
			return string.Empty;
		}

		// Token: 0x06000126 RID: 294 RVA: 0x00007664 File Offset: 0x00005864
		public virtual string NameForTypeDesignator(string typeDesignator)
		{
			return string.Empty;
		}
	}
}
