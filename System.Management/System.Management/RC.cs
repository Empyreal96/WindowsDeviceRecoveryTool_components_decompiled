using System;
using System.Globalization;
using System.Reflection;
using System.Resources;

namespace System.Management
{
	// Token: 0x02000046 RID: 70
	internal sealed class RC
	{
		// Token: 0x06000289 RID: 649 RVA: 0x000035AF File Offset: 0x000017AF
		private RC()
		{
		}

		// Token: 0x0600028A RID: 650 RVA: 0x0000DBA6 File Offset: 0x0000BDA6
		public static string GetString(string strToGet)
		{
			return RC.resMgr.GetString(strToGet, CultureInfo.CurrentCulture);
		}

		// Token: 0x040001C4 RID: 452
		private static readonly ResourceManager resMgr = new ResourceManager(Assembly.GetExecutingAssembly().GetName().Name, Assembly.GetExecutingAssembly(), null);
	}
}
