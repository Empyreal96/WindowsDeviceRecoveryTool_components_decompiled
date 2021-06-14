using System;

namespace Microsoft.WindowsDeviceRecoveryTool.Model
{
	// Token: 0x02000049 RID: 73
	public interface ISalesNameProvider
	{
		// Token: 0x06000200 RID: 512
		string NameForVidPid(string vid, string pid);

		// Token: 0x06000201 RID: 513
		string NameForString(string text);

		// Token: 0x06000202 RID: 514
		string NameForTypeDesignator(string typeDesignator);
	}
}
