using System;
using System.Globalization;

namespace Microsoft.WindowsDeviceRecoveryTool.Common
{
	// Token: 0x02000003 RID: 3
	public static class ComputerUnitsConverter
	{
		// Token: 0x06000006 RID: 6 RVA: 0x00002138 File Offset: 0x00000338
		public static string SpeedToString(double bytesPerSecond)
		{
			CultureInfo currentUICulture = CultureInfo.CurrentUICulture;
			return string.Format(currentUICulture, "{0:0.00} {1}", new object[]
			{
				bytesPerSecond / 1024.0,
				"kB/s"
			});
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002180 File Offset: 0x00000380
		public static string SizeToString(long size)
		{
			return ComputerUnitsConverter.SizeToString((float)size, 0);
		}

		// Token: 0x06000008 RID: 8 RVA: 0x0000219C File Offset: 0x0000039C
		private static string SizeToString(float size, int unit)
		{
			int num = unit + 1;
			string result;
			if (size >= 1024f && num < ComputerUnitsConverter.Units.Length)
			{
				result = ComputerUnitsConverter.SizeToString(size / 1024f, num);
			}
			else
			{
				string format = ComputerUnitsConverter.Units[unit].Replace("{0}", "{0:0.00}");
				CultureInfo currentUICulture = CultureInfo.CurrentUICulture;
				result = string.Format(currentUICulture, format, new object[]
				{
					size
				});
			}
			return result;
		}

		// Token: 0x04000003 RID: 3
		private const float Kilo = 1024f;

		// Token: 0x04000004 RID: 4
		private static readonly string[] Units = new string[]
		{
			"{0} B",
			"{0} KB",
			"{0} MB",
			"{0} GB",
			"{0} TB"
		};
	}
}
