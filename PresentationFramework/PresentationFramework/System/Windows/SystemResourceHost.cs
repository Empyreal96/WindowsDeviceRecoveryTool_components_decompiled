using System;

namespace System.Windows
{
	// Token: 0x02000112 RID: 274
	internal sealed class SystemResourceHost
	{
		// Token: 0x06000B57 RID: 2903 RVA: 0x0000326D File Offset: 0x0000146D
		private SystemResourceHost()
		{
		}

		// Token: 0x170003A7 RID: 935
		// (get) Token: 0x06000B58 RID: 2904 RVA: 0x0002800C File Offset: 0x0002620C
		internal static SystemResourceHost Instance
		{
			get
			{
				if (SystemResourceHost._instance == null)
				{
					SystemResourceHost._instance = new SystemResourceHost();
				}
				return SystemResourceHost._instance;
			}
		}

		// Token: 0x04000990 RID: 2448
		private static SystemResourceHost _instance;
	}
}
