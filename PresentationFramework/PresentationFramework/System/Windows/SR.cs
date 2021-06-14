using System;
using System.Globalization;
using System.Resources;

namespace System.Windows
{
	// Token: 0x02000143 RID: 323
	internal static class SR
	{
		// Token: 0x06000E4B RID: 3659 RVA: 0x00036D20 File Offset: 0x00034F20
		internal static string Get(string id)
		{
			string @string = SR._resourceManager.GetString(id);
			if (@string == null)
			{
				@string = SR._resourceManager.GetString("Unavailable");
			}
			return @string;
		}

		// Token: 0x06000E4C RID: 3660 RVA: 0x00036D50 File Offset: 0x00034F50
		internal static string Get(string id, params object[] args)
		{
			string text = SR._resourceManager.GetString(id);
			if (text == null)
			{
				text = SR._resourceManager.GetString("Unavailable");
			}
			else if (args != null && args.Length != 0)
			{
				text = string.Format(CultureInfo.CurrentCulture, text, args);
			}
			return text;
		}

		// Token: 0x1700045A RID: 1114
		// (get) Token: 0x06000E4D RID: 3661 RVA: 0x00036D93 File Offset: 0x00034F93
		internal static ResourceManager ResourceManager
		{
			get
			{
				return SR._resourceManager;
			}
		}

		// Token: 0x0400111B RID: 4379
		private static ResourceManager _resourceManager = new ResourceManager("ExceptionStringTable", typeof(SR).Assembly);
	}
}
