using System;
using System.ComponentModel;
using System.Reflection;

namespace MS.Internal
{
	// Token: 0x020005EF RID: 1519
	internal static class SystemDataHelper
	{
		// Token: 0x06006539 RID: 25913 RVA: 0x001C6A5C File Offset: 0x001C4C5C
		internal static bool IsDataView(IBindingList list)
		{
			SystemDataExtensionMethods systemDataExtensionMethods = AssemblyHelper.ExtensionsForSystemData(false);
			return systemDataExtensionMethods != null && systemDataExtensionMethods.IsDataView(list);
		}

		// Token: 0x0600653A RID: 25914 RVA: 0x001C6A7C File Offset: 0x001C4C7C
		internal static bool IsDataRowView(object item)
		{
			SystemDataExtensionMethods systemDataExtensionMethods = AssemblyHelper.ExtensionsForSystemData(false);
			return systemDataExtensionMethods != null && systemDataExtensionMethods.IsDataRowView(item);
		}

		// Token: 0x0600653B RID: 25915 RVA: 0x001C6A9C File Offset: 0x001C4C9C
		internal static bool IsSqlNull(object value)
		{
			SystemDataExtensionMethods systemDataExtensionMethods = AssemblyHelper.ExtensionsForSystemData(false);
			return systemDataExtensionMethods != null && systemDataExtensionMethods.IsSqlNull(value);
		}

		// Token: 0x0600653C RID: 25916 RVA: 0x001C6ABC File Offset: 0x001C4CBC
		internal static bool IsSqlNullableType(Type type)
		{
			SystemDataExtensionMethods systemDataExtensionMethods = AssemblyHelper.ExtensionsForSystemData(false);
			return systemDataExtensionMethods != null && systemDataExtensionMethods.IsSqlNullableType(type);
		}

		// Token: 0x0600653D RID: 25917 RVA: 0x001C6ADC File Offset: 0x001C4CDC
		internal static bool IsDataSetCollectionProperty(PropertyDescriptor pd)
		{
			SystemDataExtensionMethods systemDataExtensionMethods = AssemblyHelper.ExtensionsForSystemData(false);
			return systemDataExtensionMethods != null && systemDataExtensionMethods.IsDataSetCollectionProperty(pd);
		}

		// Token: 0x0600653E RID: 25918 RVA: 0x001C6AFC File Offset: 0x001C4CFC
		internal static object GetValue(object item, PropertyDescriptor pd, bool useFollowParent)
		{
			SystemDataExtensionMethods systemDataExtensionMethods = AssemblyHelper.ExtensionsForSystemData(false);
			if (systemDataExtensionMethods == null)
			{
				return null;
			}
			return systemDataExtensionMethods.GetValue(item, pd, useFollowParent);
		}

		// Token: 0x0600653F RID: 25919 RVA: 0x001C6B20 File Offset: 0x001C4D20
		internal static bool DetermineWhetherDBNullIsValid(object item, string columnName, object arg)
		{
			SystemDataExtensionMethods systemDataExtensionMethods = AssemblyHelper.ExtensionsForSystemData(false);
			return systemDataExtensionMethods != null && systemDataExtensionMethods.DetermineWhetherDBNullIsValid(item, columnName, arg);
		}

		// Token: 0x06006540 RID: 25920 RVA: 0x001C6B44 File Offset: 0x001C4D44
		internal static object NullValueForSqlNullableType(Type type)
		{
			FieldInfo field = type.GetField("Null", BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
			if (field != null)
			{
				return field.GetValue(null);
			}
			PropertyInfo property = type.GetProperty("Null", BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
			if (property != null)
			{
				return property.GetValue(null, null);
			}
			return null;
		}
	}
}
