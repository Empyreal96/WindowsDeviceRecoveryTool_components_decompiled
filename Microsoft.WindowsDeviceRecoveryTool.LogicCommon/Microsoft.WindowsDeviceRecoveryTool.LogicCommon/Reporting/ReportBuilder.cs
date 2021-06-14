using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Helpers;
using Microsoft.WindowsDeviceRecoveryTool.Model;

namespace Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Reporting
{
	// Token: 0x02000029 RID: 41
	public class ReportBuilder
	{
		// Token: 0x060001E9 RID: 489 RVA: 0x00009130 File Offset: 0x00007330
		public static MsrReport Build(ReportData reportData, bool isInternal)
		{
			return new MsrReport(reportData.SessionId)
			{
				SystemInfo = reportData.SystemInfo,
				OsPlatform = (Environment.Is64BitOperatingSystem ? "x64" : "x86"),
				OsVersion = Environment.OSVersion.Version.ToString(),
				CountryCode = RegionAndLanguage.CurrentLocation,
				ActionDescription = reportData.Description,
				Imei = reportData.PhoneInfo.Imei,
				Vid = reportData.PhoneInfo.Vid,
				Pid = reportData.PhoneInfo.Pid,
				Mid = reportData.PhoneInfo.Mid,
				Cid = reportData.PhoneInfo.Cid,
				SerialNumber = reportData.PhoneInfo.SerialNumber,
				SalesName = reportData.PhoneInfo.SalesName,
				PhoneType = reportData.PhoneInfo.Type,
				SwVersion = reportData.PhoneInfo.SoftwareVersion,
				AkVersion = reportData.PhoneInfo.AkVersion,
				NewAkVersion = reportData.PhoneInfo.NewAkVersion,
				NewSwVersion = reportData.PhoneInfo.NewSoftwareVersion,
				DownloadDuration = reportData.DownloadDuration,
				UpdateDuration = reportData.UpdateDuration,
				ApiError = string.Format("{0:X8}", ReportBuilder.ApiError(reportData.Exception)),
				ApiErrorMessage = ReportBuilder.GetApiErrorMessage(reportData.Exception, "S_OK"),
				Uri = reportData.UriData,
				ApplicationVersion = ReportBuilder.AppVersion(isInternal),
				FirmwareGrading = ReportBuilder.FirmwareGradingCheck(reportData.PhoneInfo.SoftwareVersion, reportData.PhoneInfo.NewSoftwareVersion).ToString(),
				LocalPath = reportData.LocalPath,
				PackageSizeOnServer = reportData.PackageSize.ToString(CultureInfo.InvariantCulture),
				DownloadedBytes = reportData.DownloadedBytes.ToString(CultureInfo.InvariantCulture),
				ResumeCounter = reportData.ResumeCounter.ToString(CultureInfo.InvariantCulture),
				ManufacturerName = reportData.PhoneInfo.ReportManufacturerName,
				ManufacturerProductLine = reportData.PhoneInfo.ReportManufacturerProductLine,
				ManufacturerHardwareModel = reportData.PhoneInfo.HardwareModel,
				ManufacturerHardwareVariant = reportData.PhoneInfo.HardwareVariant,
				TimeStamp = TimeStampUtility.CreateTimeStamp(),
				UserSiteLanguage = ApplicationInfo.CurrentLanguageInRegistry.IetfLanguageTag,
				ApplicationName = ReportBuilder.environmentInfo.ApplicationName,
				ApplicationVendor = ReportBuilder.environmentInfo.ApplicationVendor,
				LastErrorData = ReportBuilder.GetApiErrorMessage(reportData.LastError, string.Empty),
				DebugField = ReportBuilder.GetApiErrorDetails(reportData.Exception ?? reportData.LastError, (reportData.Exception == null && reportData.LastError != null) ? ("Last error:" + Environment.NewLine) : string.Empty)
			};
		}

		// Token: 0x060001EA RID: 490 RVA: 0x00009454 File Offset: 0x00007654
		private static string GetApiErrorDetails(Exception exception, string prefix = "")
		{
			string result;
			if (exception == null)
			{
				result = null;
			}
			else
			{
				result = ReportBuilder.Trim(string.Format(CultureInfo.InvariantCulture, "{0}{1}", new object[]
				{
					prefix,
					exception
				}), 1994);
			}
			return result;
		}

		// Token: 0x060001EB RID: 491 RVA: 0x000094A0 File Offset: 0x000076A0
		private static string Trim(string baseString, int maxNoOfChars)
		{
			string result;
			if (string.IsNullOrWhiteSpace(baseString))
			{
				result = baseString;
			}
			else
			{
				result = new string(baseString.Take(maxNoOfChars).ToArray<char>());
			}
			return result;
		}

		// Token: 0x060001EC RID: 492 RVA: 0x000094D8 File Offset: 0x000076D8
		private static int ApiError(Exception error)
		{
			COMException ex = error as COMException;
			return (ex == null) ? 0 : ex.ErrorCode;
		}

		// Token: 0x060001ED RID: 493 RVA: 0x00009500 File Offset: 0x00007700
		private static string GetApiErrorMessage(Exception error, string defaultEmptyValue = "S_OK")
		{
			string text;
			if (error == null)
			{
				text = defaultEmptyValue;
			}
			else
			{
				text = error.Message + "|" + error.Source;
				if (error.InnerException != null)
				{
					string text2 = text;
					text = string.Concat(new string[]
					{
						text2,
						"|",
						error.InnerException.Message,
						"|",
						error.InnerException.Source
					});
				}
			}
			return text;
		}

		// Token: 0x060001EE RID: 494 RVA: 0x00009594 File Offset: 0x00007794
		public static string AppVersion(bool isInternal)
		{
			string text = ReportBuilder.MainAppVersion();
			if (isInternal)
			{
				text = "[INT]" + text;
			}
			return text;
		}

		// Token: 0x060001EF RID: 495 RVA: 0x000095C4 File Offset: 0x000077C4
		private static string MainAppVersion()
		{
			Assembly entryAssembly = Assembly.GetEntryAssembly();
			string result;
			if (entryAssembly != null)
			{
				Version version = entryAssembly.GetName().Version;
				string text = version.Major.ToString(CultureInfo.InvariantCulture);
				string text2 = version.Minor.ToString(CultureInfo.InvariantCulture);
				string text3 = version.Build.ToString(CultureInfo.InvariantCulture);
				result = string.Concat(new object[]
				{
					text,
					'.',
					text2,
					'.',
					text3
				});
			}
			else
			{
				result = string.Empty;
			}
			return result;
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x0000967C File Offset: 0x0000787C
		[Conditional("DAILY")]
		private static void FormatVersionString(ref string version)
		{
			int num;
			if (int.TryParse(version, out num))
			{
				if (num > 0)
				{
					version = num.ToString("0000");
				}
			}
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x000096B8 File Offset: 0x000078B8
		private static ReportBuilder.FirmwareGrading FirmwareGradingCheck(string currentFwVersion, string newFirmwareVersion)
		{
			ReportBuilder.FirmwareGrading result;
			if (string.IsNullOrEmpty(currentFwVersion) || string.IsNullOrEmpty(newFirmwareVersion))
			{
				result = ReportBuilder.FirmwareGrading.None;
			}
			else if (currentFwVersion == newFirmwareVersion)
			{
				result = ReportBuilder.FirmwareGrading.SameVersion;
			}
			else
			{
				string text = Regex.Replace(currentFwVersion, "[^\\d\\.]", string.Empty);
				string text2 = Regex.Replace(newFirmwareVersion, "[^\\d\\.]", string.Empty);
				if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(text2))
				{
					result = ReportBuilder.FirmwareGrading.None;
				}
				else
				{
					ReportBuilder.VersionComparisonResult versionComparisonResult = ReportBuilder.VersionCompare(text, text2);
					if (versionComparisonResult == ReportBuilder.VersionComparisonResult.FirstIsOlder)
					{
						result = ReportBuilder.FirmwareGrading.Upgrade;
					}
					else if (versionComparisonResult == ReportBuilder.VersionComparisonResult.FirstIsNewer)
					{
						result = ReportBuilder.FirmwareGrading.Downgrade;
					}
					else
					{
						result = ReportBuilder.FirmwareGrading.None;
					}
				}
			}
			return result;
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x00009774 File Offset: 0x00007974
		private static ReportBuilder.VersionComparisonResult VersionCompare(string version1, string version2)
		{
			if (string.IsNullOrEmpty(version1) || string.IsNullOrEmpty(version2))
			{
				throw new ArgumentException("Version cannot be null or an empty string.");
			}
			version1 = version1.Trim();
			version2 = version2.Trim();
			string[] array = version1.Split(new char[]
			{
				'.'
			});
			string[] array2 = version2.Split(new char[]
			{
				'.'
			});
			int num = Math.Min(array.Length, array2.Length);
			int i = 0;
			while (i < num)
			{
				string value = array[i].Trim();
				string value2 = array2[i].Trim();
				long num2 = Convert.ToInt64(value);
				long num3 = Convert.ToInt64(value2);
				ReportBuilder.VersionComparisonResult result;
				if (num2 > num3)
				{
					result = ReportBuilder.VersionComparisonResult.FirstIsNewer;
				}
				else
				{
					if (num2 >= num3)
					{
						i++;
						continue;
					}
					result = ReportBuilder.VersionComparisonResult.FirstIsOlder;
				}
				return result;
			}
			if (array.Length > array2.Length)
			{
				return ReportBuilder.VersionComparisonResult.FirstIsNewer;
			}
			return (array.Length < array2.Length) ? ReportBuilder.VersionComparisonResult.FirstIsOlder : ReportBuilder.VersionComparisonResult.BothAreSame;
		}

		// Token: 0x040000EC RID: 236
		private const string InternalPrefix = "[INT]";

		// Token: 0x040000ED RID: 237
		private const string VersionPattern = "[^\\d\\.]";

		// Token: 0x040000EE RID: 238
		private const int DebugFieldLength = 2000;

		// Token: 0x040000EF RID: 239
		private const string EmptyErrorDefaultValue = "S_OK";

		// Token: 0x040000F0 RID: 240
		private static EnvironmentInfo environmentInfo = new EnvironmentInfo(new ApplicationInfo());

		// Token: 0x0200002A RID: 42
		private enum FirmwareGrading
		{
			// Token: 0x040000F2 RID: 242
			None,
			// Token: 0x040000F3 RID: 243
			Downgrade,
			// Token: 0x040000F4 RID: 244
			SameVersion,
			// Token: 0x040000F5 RID: 245
			Upgrade
		}

		// Token: 0x0200002B RID: 43
		private enum VersionComparisonResult
		{
			// Token: 0x040000F7 RID: 247
			FirstIsOlder = -1,
			// Token: 0x040000F8 RID: 248
			BothAreSame,
			// Token: 0x040000F9 RID: 249
			FirstIsNewer
		}
	}
}
