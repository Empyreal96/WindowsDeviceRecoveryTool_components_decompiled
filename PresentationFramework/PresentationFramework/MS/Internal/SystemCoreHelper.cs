using System;

namespace MS.Internal
{
	// Token: 0x020005EE RID: 1518
	internal static class SystemCoreHelper
	{
		// Token: 0x06006536 RID: 25910 RVA: 0x001C69F8 File Offset: 0x001C4BF8
		internal static bool IsIDynamicMetaObjectProvider(object item)
		{
			SystemCoreExtensionMethods systemCoreExtensionMethods = AssemblyHelper.ExtensionsForSystemCore(false);
			return systemCoreExtensionMethods != null && systemCoreExtensionMethods.IsIDynamicMetaObjectProvider(item);
		}

		// Token: 0x06006537 RID: 25911 RVA: 0x001C6A18 File Offset: 0x001C4C18
		internal static object NewDynamicPropertyAccessor(Type ownerType, string propertyName)
		{
			SystemCoreExtensionMethods systemCoreExtensionMethods = AssemblyHelper.ExtensionsForSystemCore(false);
			if (systemCoreExtensionMethods == null)
			{
				return null;
			}
			return systemCoreExtensionMethods.NewDynamicPropertyAccessor(ownerType, propertyName);
		}

		// Token: 0x06006538 RID: 25912 RVA: 0x001C6A3C File Offset: 0x001C4C3C
		internal static object GetIndexerAccessor(int rank)
		{
			SystemCoreExtensionMethods systemCoreExtensionMethods = AssemblyHelper.ExtensionsForSystemCore(false);
			if (systemCoreExtensionMethods == null)
			{
				return null;
			}
			return systemCoreExtensionMethods.GetIndexerAccessor(rank);
		}
	}
}
