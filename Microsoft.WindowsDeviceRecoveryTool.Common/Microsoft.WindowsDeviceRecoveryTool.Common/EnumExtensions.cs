using System;
using System.ComponentModel;
using System.Reflection;

namespace Microsoft.WindowsDeviceRecoveryTool.Common
{
	// Token: 0x02000004 RID: 4
	public static class EnumExtensions
	{
		// Token: 0x0600000A RID: 10 RVA: 0x00002260 File Offset: 0x00000460
		public static string GetDescription(this Enum enumType)
		{
			FieldInfo field = enumType.GetType().GetField(enumType.ToString());
			DescriptionAttribute descriptionAttribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
			return (descriptionAttribute == null) ? null : descriptionAttribute.Description;
		}
	}
}
