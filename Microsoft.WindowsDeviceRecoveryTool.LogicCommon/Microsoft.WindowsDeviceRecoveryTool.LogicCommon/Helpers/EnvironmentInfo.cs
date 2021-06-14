using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.NetworkInformation;
using Microsoft.WindowsDeviceRecoveryTool.Model;

namespace Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Helpers
{
	// Token: 0x0200000A RID: 10
	internal sealed class EnvironmentInfo : IEnvironmentInfo
	{
		// Token: 0x06000057 RID: 87 RVA: 0x000028A0 File Offset: 0x00000AA0
		public EnvironmentInfo(ApplicationInfo applicationInfo)
		{
			if (applicationInfo == null)
			{
				throw new ArgumentNullException("applicationInfo");
			}
			this.applicationInfo = applicationInfo;
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000058 RID: 88 RVA: 0x000028D4 File Offset: 0x00000AD4
		public string UserSiteLanguage
		{
			get
			{
				return CultureInfo.CurrentUICulture.Name;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000059 RID: 89 RVA: 0x000028F0 File Offset: 0x00000AF0
		public PhysicalAddress[] PhysicalAddressList
		{
			get
			{
				NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
				List<PhysicalAddress> list = new List<PhysicalAddress>();
				foreach (NetworkInterface networkInterface in allNetworkInterfaces)
				{
					list.Add(networkInterface.GetPhysicalAddress());
				}
				return list.ToArray();
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600005A RID: 90 RVA: 0x00002948 File Offset: 0x00000B48
		public IPAddress[] IPAddressList
		{
			get
			{
				string hostName = Dns.GetHostName();
				IPHostEntry hostEntry = Dns.GetHostEntry(hostName);
				return hostEntry.AddressList;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600005B RID: 91 RVA: 0x00002970 File Offset: 0x00000B70
		public string ApplicationName
		{
			get
			{
				return this.applicationInfo.ApplicationDisplayName;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600005C RID: 92 RVA: 0x00002990 File Offset: 0x00000B90
		public string ApplicationVersion
		{
			get
			{
				return this.applicationInfo.ApplicationVersion;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600005D RID: 93 RVA: 0x000029B0 File Offset: 0x00000BB0
		public string ApplicationVendor
		{
			get
			{
				return this.applicationInfo.CompanyName;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600005E RID: 94 RVA: 0x000029D0 File Offset: 0x00000BD0
		public string NasVersion
		{
			get
			{
				return "Unknown";
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600005F RID: 95 RVA: 0x000029E8 File Offset: 0x00000BE8
		public string PapiVersion
		{
			get
			{
				return "Unknown";
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000060 RID: 96 RVA: 0x00002A00 File Offset: 0x00000C00
		public string CapiVersion
		{
			get
			{
				return "Unknown";
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000061 RID: 97 RVA: 0x00002A18 File Offset: 0x00000C18
		public string FuseVersion
		{
			get
			{
				return "Unknown";
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000062 RID: 98 RVA: 0x00002A30 File Offset: 0x00000C30
		public string OSVersion
		{
			get
			{
				return Environment.OSVersion.VersionString;
			}
		}

		// Token: 0x04000019 RID: 25
		private readonly ApplicationInfo applicationInfo;
	}
}
