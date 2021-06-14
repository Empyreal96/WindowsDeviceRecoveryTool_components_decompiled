using System;
using System.Reflection;

namespace Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Helpers
{
	// Token: 0x02000010 RID: 16
	public static class UriDataArgument
	{
		// Token: 0x0600008D RID: 141 RVA: 0x00003280 File Offset: 0x00001480
		public static string Description(UriData value)
		{
			string text = string.Empty;
			Type type = value.GetType();
			FieldInfo field = type.GetField(value.ToString());
			if (field != null)
			{
				UriDescriptionAttribute[] array = field.GetCustomAttributes(typeof(UriDescriptionAttribute), false) as UriDescriptionAttribute[];
				if (array != null && array.Length > 0)
				{
					text = array[0].Value;
				}
			}
			if (!string.IsNullOrEmpty(text))
			{
				object obj = text;
				text = string.Concat(new object[]
				{
					obj,
					" (",
					(int)value,
					")"
				});
			}
			return text;
		}
	}
}
