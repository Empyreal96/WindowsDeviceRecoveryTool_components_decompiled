using System;
using System.Net;
using System.Net.NetworkInformation;

namespace Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Helpers
{
	// Token: 0x02000009 RID: 9
	public interface IEnvironmentInfo
	{
		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600004C RID: 76
		string UserSiteLanguage { get; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600004D RID: 77
		PhysicalAddress[] PhysicalAddressList { get; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600004E RID: 78
		IPAddress[] IPAddressList { get; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600004F RID: 79
		string ApplicationName { get; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000050 RID: 80
		string ApplicationVersion { get; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000051 RID: 81
		string ApplicationVendor { get; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000052 RID: 82
		string NasVersion { get; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000053 RID: 83
		string PapiVersion { get; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000054 RID: 84
		string CapiVersion { get; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000055 RID: 85
		string FuseVersion { get; }

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000056 RID: 86
		string OSVersion { get; }
	}
}
