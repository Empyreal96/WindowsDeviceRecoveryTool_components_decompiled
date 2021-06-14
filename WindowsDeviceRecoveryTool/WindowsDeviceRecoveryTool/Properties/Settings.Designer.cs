using System;
using System.CodeDom.Compiler;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Microsoft.WindowsDeviceRecoveryTool.Styles.Assets;

namespace Microsoft.WindowsDeviceRecoveryTool.Properties
{
	// Token: 0x02000071 RID: 113
	[CompilerGenerated]
	[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "11.0.0.0")]
	public sealed partial class Settings : ApplicationSettingsBase
	{
		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x06000337 RID: 823 RVA: 0x00010498 File Offset: 0x0000E698
		public static Settings Default
		{
			get
			{
				return Settings.defaultInstance;
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06000338 RID: 824 RVA: 0x000104B0 File Offset: 0x0000E6B0
		// (set) Token: 0x06000339 RID: 825 RVA: 0x000104D2 File Offset: 0x0000E6D2
		[DebuggerNonUserCode]
		[UserScopedSetting]
		[DefaultSettingValue("AccentColorIndigo")]
		public string Style
		{
			get
			{
				return (string)this["Style"];
			}
			set
			{
				this["Style"] = value;
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x0600033A RID: 826 RVA: 0x000104E4 File Offset: 0x0000E6E4
		// (set) Token: 0x0600033B RID: 827 RVA: 0x00010506 File Offset: 0x0000E706
		[UserScopedSetting]
		[DefaultSettingValue("")]
		[DebuggerNonUserCode]
		public string Login
		{
			get
			{
				return (string)this["Login"];
			}
			set
			{
				this["Login"] = value;
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x0600033C RID: 828 RVA: 0x00010518 File Offset: 0x0000E718
		// (set) Token: 0x0600033D RID: 829 RVA: 0x0001053A File Offset: 0x0000E73A
		[DefaultSettingValue("")]
		[UserScopedSetting]
		[DebuggerNonUserCode]
		public string Password
		{
			get
			{
				return (string)this["Password"];
			}
			set
			{
				this["Password"] = value;
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x0600033E RID: 830 RVA: 0x0001054C File Offset: 0x0000E74C
		// (set) Token: 0x0600033F RID: 831 RVA: 0x0001056E File Offset: 0x0000E76E
		[DebuggerNonUserCode]
		[DefaultSettingValue("False")]
		[UserScopedSetting]
		public string SaveCredentials
		{
			get
			{
				return (string)this["SaveCredentials"];
			}
			set
			{
				this["SaveCredentials"] = value;
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000340 RID: 832 RVA: 0x00010580 File Offset: 0x0000E780
		// (set) Token: 0x06000341 RID: 833 RVA: 0x000105A2 File Offset: 0x0000E7A2
		[DebuggerNonUserCode]
		[DefaultSettingValue("")]
		[UserScopedSetting]
		public string UserGroup
		{
			get
			{
				return (string)this["UserGroup"];
			}
			set
			{
				this["UserGroup"] = value;
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000342 RID: 834 RVA: 0x000105B4 File Offset: 0x0000E7B4
		// (set) Token: 0x06000343 RID: 835 RVA: 0x000105D6 File Offset: 0x0000E7D6
		[UserScopedSetting]
		[DefaultSettingValue("")]
		[DebuggerNonUserCode]
		public string ProxyPassword
		{
			get
			{
				return (string)this["ProxyPassword"];
			}
			set
			{
				this["ProxyPassword"] = value;
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000344 RID: 836 RVA: 0x000105E8 File Offset: 0x0000E7E8
		// (set) Token: 0x06000345 RID: 837 RVA: 0x0001060A File Offset: 0x0000E80A
		[DebuggerNonUserCode]
		[DefaultSettingValue("True")]
		[UserScopedSetting]
		public bool TraceEnabled
		{
			get
			{
				return (bool)this["TraceEnabled"];
			}
			set
			{
				this["TraceEnabled"] = value;
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x06000346 RID: 838 RVA: 0x00010620 File Offset: 0x0000E820
		// (set) Token: 0x06000347 RID: 839 RVA: 0x00010642 File Offset: 0x0000E842
		[DebuggerNonUserCode]
		[UserScopedSetting]
		[DefaultSettingValue("%temp%")]
		public string ZipFilePath
		{
			get
			{
				return (string)this["ZipFilePath"];
			}
			set
			{
				this["ZipFilePath"] = value;
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x06000348 RID: 840 RVA: 0x00010654 File Offset: 0x0000E854
		// (set) Token: 0x06000349 RID: 841 RVA: 0x00010676 File Offset: 0x0000E876
		[DebuggerNonUserCode]
		[DefaultSettingValue("")]
		[UserScopedSetting]
		public string ProxyUsername
		{
			get
			{
				return (string)this["ProxyUsername"];
			}
			set
			{
				this["ProxyUsername"] = value;
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x0600034A RID: 842 RVA: 0x00010688 File Offset: 0x0000E888
		// (set) Token: 0x0600034B RID: 843 RVA: 0x000106AA File Offset: 0x0000E8AA
		[DebuggerNonUserCode]
		[UserScopedSetting]
		[DefaultSettingValue("")]
		public string ProxyAddress
		{
			get
			{
				return (string)this["ProxyAddress"];
			}
			set
			{
				this["ProxyAddress"] = value;
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x0600034C RID: 844 RVA: 0x000106BC File Offset: 0x0000E8BC
		// (set) Token: 0x0600034D RID: 845 RVA: 0x000106DE File Offset: 0x0000E8DE
		[UserScopedSetting]
		[DebuggerNonUserCode]
		[DefaultSettingValue("8080")]
		public int ProxyPort
		{
			get
			{
				return (int)this["ProxyPort"];
			}
			set
			{
				this["ProxyPort"] = value;
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x0600034E RID: 846 RVA: 0x000106F4 File Offset: 0x0000E8F4
		// (set) Token: 0x0600034F RID: 847 RVA: 0x00010716 File Offset: 0x0000E916
		[UserScopedSetting]
		[DebuggerNonUserCode]
		[DefaultSettingValue("False")]
		public bool UseManualProxy
		{
			get
			{
				return (bool)this["UseManualProxy"];
			}
			set
			{
				this["UseManualProxy"] = value;
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000350 RID: 848 RVA: 0x0001072C File Offset: 0x0000E92C
		// (set) Token: 0x06000351 RID: 849 RVA: 0x0001074E File Offset: 0x0000E94E
		[DebuggerNonUserCode]
		[DefaultSettingValue("1")]
		[UserScopedSetting]
		public string DaysToCollectLogFiles
		{
			get
			{
				return (string)this["DaysToCollectLogFiles"];
			}
			set
			{
				this["DaysToCollectLogFiles"] = value;
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000352 RID: 850 RVA: 0x00010760 File Offset: 0x0000E960
		// (set) Token: 0x06000353 RID: 851 RVA: 0x00010782 File Offset: 0x0000E982
		[DefaultSettingValue("")]
		[UserScopedSetting]
		[DebuggerNonUserCode]
		public string RememberedUsergroups
		{
			get
			{
				return (string)this["RememberedUsergroups"];
			}
			set
			{
				this["RememberedUsergroups"] = value;
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x06000354 RID: 852 RVA: 0x00010794 File Offset: 0x0000E994
		// (set) Token: 0x06000355 RID: 853 RVA: 0x000107B6 File Offset: 0x0000E9B6
		[DefaultSettingValue("True")]
		[DebuggerNonUserCode]
		[UserScopedSetting]
		public bool CallUpgrade
		{
			get
			{
				return (bool)this["CallUpgrade"];
			}
			set
			{
				this["CallUpgrade"] = value;
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x06000356 RID: 854 RVA: 0x000107CC File Offset: 0x0000E9CC
		// (set) Token: 0x06000357 RID: 855 RVA: 0x000107EE File Offset: 0x0000E9EE
		[UserScopedSetting]
		[DebuggerNonUserCode]
		[DefaultSettingValue("ThemeLight")]
		public string Theme
		{
			get
			{
				return (string)this["Theme"];
			}
			set
			{
				this["Theme"] = value;
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06000358 RID: 856 RVA: 0x00010800 File Offset: 0x0000EA00
		// (set) Token: 0x06000359 RID: 857 RVA: 0x00010822 File Offset: 0x0000EA22
		[DefaultSettingValue("False")]
		[UserScopedSetting]
		[DebuggerNonUserCode]
		public string ExtendedTraceEnabled
		{
			get
			{
				return (string)this["ExtendedTraceEnabled"];
			}
			set
			{
				this["ExtendedTraceEnabled"] = value;
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x0600035A RID: 858 RVA: 0x00010834 File Offset: 0x0000EA34
		// (set) Token: 0x0600035B RID: 859 RVA: 0x00010856 File Offset: 0x0000EA56
		[DefaultSettingValue("False")]
		[DebuggerNonUserCode]
		[UserScopedSetting]
		public bool CustomPackagesPathEnabled
		{
			get
			{
				return (bool)this["CustomPackagesPathEnabled"];
			}
			set
			{
				this["CustomPackagesPathEnabled"] = value;
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x0600035C RID: 860 RVA: 0x0001086C File Offset: 0x0000EA6C
		// (set) Token: 0x0600035D RID: 861 RVA: 0x0001088E File Offset: 0x0000EA8E
		[UserScopedSetting]
		[DebuggerNonUserCode]
		[DefaultSettingValue("")]
		public string PackagesPath
		{
			get
			{
				return (string)this["PackagesPath"];
			}
			set
			{
				this["PackagesPath"] = value;
			}
		}

		// Token: 0x0400017E RID: 382
		private static Settings defaultInstance = (Settings)SettingsBase.Synchronized(new Settings());
	}
}
