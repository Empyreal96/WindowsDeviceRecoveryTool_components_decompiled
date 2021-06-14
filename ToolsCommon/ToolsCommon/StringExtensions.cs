using System;

namespace Microsoft.WindowsPhone.ImageUpdate.Tools.Common
{
	// Token: 0x02000038 RID: 56
	public static class StringExtensions
	{
		// Token: 0x0600017E RID: 382 RVA: 0x00008B60 File Offset: 0x00006D60
		public static string Replace(this string originalString, string oldValue, string newValue, StringComparison comparisonType)
		{
			int num = 0;
			for (;;)
			{
				num = originalString.IndexOf(oldValue, num, comparisonType);
				if (num == -1)
				{
					break;
				}
				originalString = originalString.Substring(0, num) + newValue + originalString.Substring(num + oldValue.Length);
				num += newValue.Length;
			}
			return originalString;
		}
	}
}
