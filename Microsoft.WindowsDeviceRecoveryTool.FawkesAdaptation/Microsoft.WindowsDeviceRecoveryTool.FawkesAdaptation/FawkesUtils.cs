using System;
using ClickerUtilityLibrary.Misc;
using Microsoft.WindowsDeviceRecoveryTool.Model;

namespace Microsoft.WindowsDeviceRecoveryTool.FawkesAdaptation
{
	// Token: 0x0200000D RID: 13
	public static class FawkesUtils
	{
		// Token: 0x0600006C RID: 108 RVA: 0x000037DC File Offset: 0x000019DC
		public static bool TryParseImageVersion(string versionString, out ImageVersion imageVersion)
		{
			imageVersion = null;
			if (string.IsNullOrWhiteSpace(versionString))
			{
				return false;
			}
			string[] array = versionString.Split(new char[]
			{
				'.',
				'-'
			});
			if (array.Length == 0)
			{
				imageVersion = new ImageVersion(0);
				return true;
			}
			byte minor = 0;
			short buildNumber = 0;
			byte major;
			if (!byte.TryParse(array[0], out major))
			{
				return false;
			}
			if (array.Length > 1)
			{
				if (!byte.TryParse(array[1], out minor))
				{
					return false;
				}
				if (array.Length > 2 && !short.TryParse(array[2], out buildNumber))
				{
					return false;
				}
			}
			imageVersion = new ImageVersion(major, minor, buildNumber);
			return true;
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00003867 File Offset: 0x00001A67
		internal static void WriteFawkesDeviceInfo(this Phone phone, FawkesDeviceInfo deviceInfo)
		{
			phone.HardwareId = deviceInfo.HardwareId;
			phone.SalesName = deviceInfo.DeviceFriendlyName;
			phone.SoftwareVersion = deviceInfo.FirmwareVersion.ToString();
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00003892 File Offset: 0x00001A92
		internal static FawkesDeviceInfo ReadFawkesDeviceInfo(this Phone phone)
		{
			return new FawkesDeviceInfo(phone.SoftwareVersion, phone.HardwareId, phone.SalesName);
		}
	}
}
