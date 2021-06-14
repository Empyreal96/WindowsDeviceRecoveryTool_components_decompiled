using System;
using System.Globalization;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000105 RID: 261
	internal static class ValidationUtils
	{
		// Token: 0x06000C25 RID: 3109 RVA: 0x00031732 File Offset: 0x0002F932
		public static void ArgumentNotNullOrEmpty(string value, string parameterName)
		{
			if (value == null)
			{
				throw new ArgumentNullException(parameterName);
			}
			if (value.Length == 0)
			{
				throw new ArgumentException("'{0}' cannot be empty.".FormatWith(CultureInfo.InvariantCulture, parameterName), parameterName);
			}
		}

		// Token: 0x06000C26 RID: 3110 RVA: 0x0003175D File Offset: 0x0002F95D
		public static void ArgumentTypeIsEnum(Type enumType, string parameterName)
		{
			ValidationUtils.ArgumentNotNull(enumType, "enumType");
			if (!enumType.IsEnum())
			{
				throw new ArgumentException("Type {0} is not an Enum.".FormatWith(CultureInfo.InvariantCulture, enumType), parameterName);
			}
		}

		// Token: 0x06000C27 RID: 3111 RVA: 0x00031789 File Offset: 0x0002F989
		public static void ArgumentNotNull(object value, string parameterName)
		{
			if (value == null)
			{
				throw new ArgumentNullException(parameterName);
			}
		}
	}
}
