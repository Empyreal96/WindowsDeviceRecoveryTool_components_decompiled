using System;
using System.ComponentModel;

namespace MS.Internal
{
	// Token: 0x020005F1 RID: 1521
	internal static class SystemXmlLinqHelper
	{
		// Token: 0x06006549 RID: 25929 RVA: 0x001C6CB0 File Offset: 0x001C4EB0
		internal static bool IsXElement(object item)
		{
			SystemXmlLinqExtensionMethods systemXmlLinqExtensionMethods = AssemblyHelper.ExtensionsForSystemXmlLinq(false);
			return systemXmlLinqExtensionMethods != null && systemXmlLinqExtensionMethods.IsXElement(item);
		}

		// Token: 0x0600654A RID: 25930 RVA: 0x001C6CD0 File Offset: 0x001C4ED0
		internal static string GetXElementTagName(object item)
		{
			SystemXmlLinqExtensionMethods systemXmlLinqExtensionMethods = AssemblyHelper.ExtensionsForSystemXmlLinq(false);
			if (systemXmlLinqExtensionMethods == null)
			{
				return null;
			}
			return systemXmlLinqExtensionMethods.GetXElementTagName(item);
		}

		// Token: 0x0600654B RID: 25931 RVA: 0x001C6CF0 File Offset: 0x001C4EF0
		internal static bool IsXLinqCollectionProperty(PropertyDescriptor pd)
		{
			SystemXmlLinqExtensionMethods systemXmlLinqExtensionMethods = AssemblyHelper.ExtensionsForSystemXmlLinq(false);
			return systemXmlLinqExtensionMethods != null && systemXmlLinqExtensionMethods.IsXLinqCollectionProperty(pd);
		}

		// Token: 0x0600654C RID: 25932 RVA: 0x001C6D10 File Offset: 0x001C4F10
		internal static bool IsXLinqNonIdempotentProperty(PropertyDescriptor pd)
		{
			SystemXmlLinqExtensionMethods systemXmlLinqExtensionMethods = AssemblyHelper.ExtensionsForSystemXmlLinq(false);
			return systemXmlLinqExtensionMethods != null && systemXmlLinqExtensionMethods.IsXLinqNonIdempotentProperty(pd);
		}
	}
}
