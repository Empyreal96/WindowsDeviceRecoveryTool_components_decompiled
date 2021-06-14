using System;
using System.Deployment.Application;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Interop;

namespace MS.Internal
{
	// Token: 0x020005E3 RID: 1507
	internal static class DeploymentExceptionMapper
	{
		// Token: 0x06006476 RID: 25718 RVA: 0x001C2ED4 File Offset: 0x001C10D4
		internal static MissingDependencyType GetWinFXRequirement(Exception e, InPlaceHostingManager hostingManager, out string version, out Uri fwlinkUri)
		{
			version = string.Empty;
			fwlinkUri = null;
			if (e is DependentPlatformMissingException)
			{
				Regex regex = new Regex(".NET Framework (v\\.?)?(?<version>\\d{1,2}(\\.\\d{1,2})?)", RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);
				string message = e.Message;
				Match match = regex.Match(message);
				if (match.Success)
				{
					version = match.Groups[1].Value;
					DeploymentExceptionMapper.ConstructFwlinkUrl(version, out fwlinkUri);
					return MissingDependencyType.WinFX;
				}
			}
			Assembly assembly = Assembly.GetAssembly(hostingManager.GetType());
			if (assembly == null)
			{
				return MissingDependencyType.Others;
			}
			ResourceManager resourceManager = new ResourceManager("System.Deployment", assembly);
			if (resourceManager == null)
			{
				return MissingDependencyType.Others;
			}
			string text = resourceManager.GetString("PlatformMicrosoftCommonLanguageRuntime", CultureInfo.CurrentUICulture);
			string text2 = resourceManager.GetString("PlatformDependentAssemblyVersion", CultureInfo.CurrentUICulture);
			if (text == null || text2 == null)
			{
				return MissingDependencyType.Others;
			}
			text = text.Replace("{0}", "");
			text2 = text2.Replace("{0}", "");
			text2 = text2.Replace("{1}", "");
			string[] array = new string[]
			{
				"WindowsBase",
				"System.Core",
				"Sentinel.v3.5Client",
				"System.Data.Entity"
			};
			string message2 = e.Message;
			int num = message2.IndexOf(text2, StringComparison.Ordinal);
			if (num != -1)
			{
				version = string.Copy(message2.Substring(num + text2.Length));
				int num2 = version.IndexOf(".", StringComparison.Ordinal);
				int num3 = version.IndexOf(".", num2 + 1, StringComparison.Ordinal);
				if (message2.IndexOf(text, StringComparison.Ordinal) != -1)
				{
					if (OperatingSystemVersionCheck.IsVersionOrLater(OperatingSystemVersion.Windows8))
					{
						int num4 = version.IndexOf(".", num3 + 1, StringComparison.Ordinal);
						if (num4 != -1)
						{
							version = version.Substring(0, num4);
						}
					}
					else if (num3 != -1)
					{
						version = version.Substring(0, num3);
					}
					string version2 = "CLR" + version;
					if (!DeploymentExceptionMapper.ConstructFwlinkUrl(version2, out fwlinkUri))
					{
						return MissingDependencyType.Others;
					}
					return MissingDependencyType.CLR;
				}
				else
				{
					if (num3 != -1)
					{
						version = version.Substring(0, num3);
					}
					bool flag = false;
					foreach (string value in array)
					{
						if (message2.IndexOf(value, StringComparison.OrdinalIgnoreCase) > 0)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						version = string.Empty;
					}
				}
			}
			if (!DeploymentExceptionMapper.ConstructFwlinkUrl(version, out fwlinkUri))
			{
				return MissingDependencyType.Others;
			}
			return MissingDependencyType.WinFX;
		}

		// Token: 0x06006477 RID: 25719 RVA: 0x001C3100 File Offset: 0x001C1300
		internal static void GetErrorTextFromException(Exception e, out string errorTitle, out string errorMessage)
		{
			errorTitle = string.Empty;
			errorMessage = string.Empty;
			if (e == null)
			{
				errorTitle = SR.Get("CancelledTitle");
				errorMessage = SR.Get("CancelledText");
				return;
			}
			if (e is DependentPlatformMissingException)
			{
				errorTitle = SR.Get("PlatformRequirementTitle");
				errorMessage = e.Message;
				return;
			}
			if (e is InvalidDeploymentException)
			{
				errorTitle = SR.Get("InvalidDeployTitle");
				errorMessage = SR.Get("InvalidDeployText");
				return;
			}
			if (e is TrustNotGrantedException)
			{
				errorTitle = SR.Get("TrustNotGrantedTitle");
				errorMessage = SR.Get("TrustNotGrantedText");
				return;
			}
			if (e is DeploymentDownloadException)
			{
				errorTitle = SR.Get("DownloadTitle");
				errorMessage = SR.Get("DownloadText");
				return;
			}
			if (e is DeploymentException)
			{
				errorTitle = SR.Get("DeployTitle");
				errorMessage = SR.Get("DeployText");
				return;
			}
			errorTitle = SR.Get("UnknownErrorTitle");
			errorMessage = SR.Get("UnknownErrorText") + "\n\n" + e.Message;
		}

		// Token: 0x06006478 RID: 25720 RVA: 0x001C3200 File Offset: 0x001C1400
		internal static bool ConstructFwlinkUrl(string version, out Uri fwlinkUri)
		{
			string text = string.Empty;
			fwlinkUri = null;
			if (version != string.Empty)
			{
				text = string.Copy("http://go.microsoft.com/fwlink?prd=11953&sbp=Bootwinfx&pver=");
				text += version;
				text += "&plcid=0x409&clcid=0x409&";
				DateTime today = DateTime.Today;
				text += today.Year.ToString();
				text += today.Month.ToString();
				text += today.Day.ToString();
				fwlinkUri = new Uri(text);
				return true;
			}
			return false;
		}

		// Token: 0x040032AA RID: 12970
		private const string fwlinkPrefix = "http://go.microsoft.com/fwlink?prd=11953&sbp=Bootwinfx&pver=";

		// Token: 0x040032AB RID: 12971
		private const string fwlinkSuffix = "&plcid=0x409&clcid=0x409&";
	}
}
