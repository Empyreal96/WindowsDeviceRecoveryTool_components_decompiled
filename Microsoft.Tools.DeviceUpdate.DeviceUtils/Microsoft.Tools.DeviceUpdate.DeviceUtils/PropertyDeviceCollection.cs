using System;
using System.Collections.Generic;
using System.Reflection;

namespace Microsoft.Tools.DeviceUpdate.DeviceUtils
{
	// Token: 0x02000024 RID: 36
	public class PropertyDeviceCollection
	{
		// Token: 0x060000DD RID: 221 RVA: 0x0000F4A8 File Offset: 0x0000D6A8
		public static void GetProperties(object host, ref SortedDictionary<string, string> properties)
		{
			foreach (Type type in host.GetType().GetInterfaces())
			{
				foreach (PropertyInfo propertyInfo in type.GetProperties())
				{
					object[] customAttributes = propertyInfo.GetCustomAttributes(typeof(DevicePropertyAttribute), true);
					foreach (object obj in customAttributes)
					{
						DevicePropertyAttribute devicePropertyAttribute = (DevicePropertyAttribute)obj;
						string name = devicePropertyAttribute.Name;
						try
						{
							string value = propertyInfo.GetValue(host, null).ToString();
							if (!properties.ContainsKey(name) || string.IsNullOrEmpty(properties[name]))
							{
								properties.Add(name, value);
							}
						}
						catch
						{
						}
					}
				}
			}
		}

		// Token: 0x060000DE RID: 222 RVA: 0x0000F594 File Offset: 0x0000D794
		public static void GetPropertyNames(object host, ref SortedDictionary<string, string> properties)
		{
			foreach (Type type in host.GetType().GetInterfaces())
			{
				foreach (PropertyInfo propertyInfo in type.GetProperties())
				{
					object[] customAttributes = propertyInfo.GetCustomAttributes(typeof(DevicePropertyAttribute), true);
					foreach (object obj in customAttributes)
					{
						DevicePropertyAttribute devicePropertyAttribute = (DevicePropertyAttribute)obj;
						string name = devicePropertyAttribute.Name;
						properties[devicePropertyAttribute.Name] = propertyInfo.Name;
					}
				}
			}
		}

		// Token: 0x060000DF RID: 223 RVA: 0x0000F640 File Offset: 0x0000D840
		public static object GetProperty(object host, string name)
		{
			foreach (Type type in host.GetType().GetInterfaces())
			{
				PropertyInfo deviceProperty = PropertyDeviceCollection.GetDeviceProperty(host, type, name);
				if (null != deviceProperty)
				{
					object value = deviceProperty.GetValue(host, null);
					if (value != null)
					{
						return value;
					}
				}
			}
			return null;
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x0000F698 File Offset: 0x0000D898
		public static string GetPropertyString(object host, string name)
		{
			object property = PropertyDeviceCollection.GetProperty(host, name);
			if (property == null)
			{
				return null;
			}
			return property.ToString();
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x0000F6B8 File Offset: 0x0000D8B8
		public static void SetProperty(object host, string name, object value)
		{
			PropertyInfo property = host.GetType().GetProperty(name);
			if (null != property.GetSetMethod())
			{
				property.SetValue(host, value, null);
			}
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x0000F6EC File Offset: 0x0000D8EC
		private static PropertyInfo GetDeviceProperty(object host, Type type, string name)
		{
			foreach (PropertyInfo propertyInfo in type.GetProperties())
			{
				object[] customAttributes = propertyInfo.GetCustomAttributes(typeof(DevicePropertyAttribute), true);
				foreach (object obj in customAttributes)
				{
					DevicePropertyAttribute devicePropertyAttribute = (DevicePropertyAttribute)obj;
					if (string.Compare(name, devicePropertyAttribute.Name, true) == 0)
					{
						return propertyInfo;
					}
				}
			}
			return null;
		}
	}
}
