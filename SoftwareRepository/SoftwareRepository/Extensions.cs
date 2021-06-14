using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace SoftwareRepository
{
	// Token: 0x02000005 RID: 5
	public static class Extensions
	{
		// Token: 0x06000003 RID: 3 RVA: 0x0000205E File Offset: 0x0000025E
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "o")]
		public static string ToJson(this object o)
		{
			return JsonConvert.SerializeObject(o);
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002066 File Offset: 0x00000266
		public static string ToSpeedFormat(this double speed)
		{
			return string.Format("{0}/s", Extensions.ByteSizeConverter((long)Math.Round(speed)));
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002080 File Offset: 0x00000280
		private static string ByteSizeConverter(long size)
		{
			string text;
			if (size < 1024L)
			{
				text = string.Format("{0} B", size);
			}
			else if (size < 1048576L)
			{
				text = string.Format("{0:F2} KiB", 1.0 * (double)size / 1024.0);
			}
			else if (size < 1073741824L)
			{
				text = string.Format("{0:F2} MiB", 1.0 * (double)size / 1048576.0);
			}
			else if (size < 1099511627776L)
			{
				text = string.Format("{0:F2} GiB", 1.0 * (double)size / 1073741824.0);
			}
			else
			{
				text = string.Format("{0:F2} TiB", 1.0 * (double)size / 1099511627776.0);
			}
			if (size >= 1024L)
			{
				return string.Format("{0} ({1} B)", text, size);
			}
			return text;
		}
	}
}
