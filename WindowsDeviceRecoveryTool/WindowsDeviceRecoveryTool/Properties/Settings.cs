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
		// Token: 0x0600035E RID: 862 RVA: 0x0001089E File Offset: 0x0000EA9E
		public Settings()
		{
			base.SettingChanging += this.SettingChangingEventHandler;
		}

		// Token: 0x0600035F RID: 863 RVA: 0x000108BC File Offset: 0x0000EABC
		private void SettingChangingEventHandler(object sender, SettingChangingEventArgs e)
		{
			if (e.SettingName.Equals("Style"))
			{
				StyleLogic.CurrentStyle = StyleLogic.GetStyle((string)e.NewValue);
			}
		}

		// Token: 0x06000360 RID: 864 RVA: 0x000108FC File Offset: 0x0000EAFC
		public static string GetThemeFileName(string name)
		{
			if (name != null)
			{
				if (name == "ThemeDark")
				{
					return "DarkTheme.xaml";
				}
				if (name == "ThemeLight")
				{
					return "LightTheme.xaml";
				}
				if (name == "ThemeHighContrast")
				{
					return "HighContrastTheme.xaml";
				}
			}
			return "LightTheme.xaml";
		}

		// Token: 0x06000361 RID: 865 RVA: 0x0001095C File Offset: 0x0000EB5C
		public static string GetSelectedThemeFileName()
		{
			return Settings.GetThemeFileName(Settings.Default.Theme);
		}
	}
}
