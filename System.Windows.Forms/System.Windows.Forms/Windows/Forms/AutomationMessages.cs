using System;
using System.IO;

namespace System.Windows.Forms
{
	// Token: 0x0200031B RID: 795
	internal static class AutomationMessages
	{
		// Token: 0x060031AE RID: 12718 RVA: 0x000E8B2C File Offset: 0x000E6D2C
		public static IntPtr WriteAutomationText(string text)
		{
			IntPtr zero = IntPtr.Zero;
			string text2 = AutomationMessages.GenerateLogFileName(ref zero);
			if (text2 != null)
			{
				try
				{
					FileStream fileStream = new FileStream(text2, FileMode.Create, FileAccess.Write);
					StreamWriter streamWriter = new StreamWriter(fileStream);
					streamWriter.WriteLine(text);
					streamWriter.Dispose();
					fileStream.Dispose();
				}
				catch
				{
					zero = IntPtr.Zero;
				}
			}
			return zero;
		}

		// Token: 0x060031AF RID: 12719 RVA: 0x000E8B8C File Offset: 0x000E6D8C
		public static string ReadAutomationText(IntPtr fileId)
		{
			string result = null;
			if (fileId != IntPtr.Zero)
			{
				string path = AutomationMessages.GenerateLogFileName(ref fileId);
				if (File.Exists(path))
				{
					try
					{
						FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
						StreamReader streamReader = new StreamReader(fileStream);
						result = streamReader.ReadToEnd();
						streamReader.Dispose();
						fileStream.Dispose();
					}
					catch
					{
						result = null;
					}
				}
			}
			return result;
		}

		// Token: 0x060031B0 RID: 12720 RVA: 0x000E8BF4 File Offset: 0x000E6DF4
		private static string GenerateLogFileName(ref IntPtr fileId)
		{
			string result = null;
			string environmentVariable = Environment.GetEnvironmentVariable("TEMP");
			if (environmentVariable != null)
			{
				if (fileId == IntPtr.Zero)
				{
					Random random = new Random(DateTime.Now.Millisecond);
					fileId = new IntPtr(random.Next());
				}
				result = environmentVariable + "\\Maui" + fileId.ToString() + ".log";
			}
			return result;
		}

		// Token: 0x04001E11 RID: 7697
		private const int WM_USER = 1024;

		// Token: 0x04001E12 RID: 7698
		internal const int PGM_GETBUTTONCOUNT = 1104;

		// Token: 0x04001E13 RID: 7699
		internal const int PGM_GETBUTTONSTATE = 1106;

		// Token: 0x04001E14 RID: 7700
		internal const int PGM_SETBUTTONSTATE = 1105;

		// Token: 0x04001E15 RID: 7701
		internal const int PGM_GETBUTTONTEXT = 1107;

		// Token: 0x04001E16 RID: 7702
		internal const int PGM_GETBUTTONTOOLTIPTEXT = 1108;

		// Token: 0x04001E17 RID: 7703
		internal const int PGM_GETROWCOORDS = 1109;

		// Token: 0x04001E18 RID: 7704
		internal const int PGM_GETVISIBLEROWCOUNT = 1110;

		// Token: 0x04001E19 RID: 7705
		internal const int PGM_GETSELECTEDROW = 1111;

		// Token: 0x04001E1A RID: 7706
		internal const int PGM_SETSELECTEDTAB = 1112;

		// Token: 0x04001E1B RID: 7707
		internal const int PGM_GETTESTINGINFO = 1113;
	}
}
