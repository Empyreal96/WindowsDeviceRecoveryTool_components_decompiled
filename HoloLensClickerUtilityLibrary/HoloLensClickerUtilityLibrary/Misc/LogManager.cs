using System;
using System.Globalization;
using System.IO;

namespace ClickerUtilityLibrary.Misc
{
	// Token: 0x0200000F RID: 15
	public class LogManager : IDisposable
	{
		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000056 RID: 86 RVA: 0x000049C8 File Offset: 0x00002BC8
		public static LogManager Instance
		{
			get
			{
				bool flag = LogManager.instance == null;
				if (flag)
				{
					object obj = LogManager.lmHandler;
					lock (obj)
					{
						LogManager.instance = new LogManager();
					}
				}
				return LogManager.instance;
			}
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00004A30 File Offset: 0x00002C30
		private LogManager()
		{
			string text = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
			text += "\\Microsoft\\HoloLensClickerUtility";
			text = text + "\\" + typeof(LogManager).Assembly.GetName().Version;
			text += ".\\Log\\";
			bool flag = !Directory.Exists(text);
			if (flag)
			{
				Directory.CreateDirectory(text);
			}
			string str = "Sys_" + DateTime.Now.ToString("yyyy_MM_dd", CultureInfo.InvariantCulture) + ".log";
			this.mSysLogWriter = new StreamWriter(text + str, true);
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00004AD8 File Offset: 0x00002CD8
		public void Dispose()
		{
			bool flag = this.mSysLogWriter != null;
			if (flag)
			{
				this.mSysLogWriter.Close();
			}
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00004B08 File Offset: 0x00002D08
		public void Log(string msg)
		{
			bool flag = this.mSysLogWriter != null;
			if (flag)
			{
				this.mSysLogWriter.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss: ", CultureInfo.InvariantCulture) + msg);
				this.mSysLogWriter.Flush();
			}
		}

		// Token: 0x04000070 RID: 112
		private static volatile LogManager instance;

		// Token: 0x04000071 RID: 113
		private static readonly object lmHandler = new object();

		// Token: 0x04000072 RID: 114
		private readonly StreamWriter mSysLogWriter;
	}
}
