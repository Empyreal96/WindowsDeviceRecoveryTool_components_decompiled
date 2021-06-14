using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Windows;

namespace MS.Internal
{
	// Token: 0x020005F0 RID: 1520
	internal static class SystemXmlHelper
	{
		// Token: 0x06006541 RID: 25921 RVA: 0x001C6B94 File Offset: 0x001C4D94
		internal static bool IsXmlNode(object item)
		{
			SystemXmlExtensionMethods systemXmlExtensionMethods = AssemblyHelper.ExtensionsForSystemXml(false);
			return systemXmlExtensionMethods != null && systemXmlExtensionMethods.IsXmlNode(item);
		}

		// Token: 0x06006542 RID: 25922 RVA: 0x001C6BB4 File Offset: 0x001C4DB4
		internal static bool IsXmlNamespaceManager(object item)
		{
			SystemXmlExtensionMethods systemXmlExtensionMethods = AssemblyHelper.ExtensionsForSystemXml(false);
			return systemXmlExtensionMethods != null && systemXmlExtensionMethods.IsXmlNamespaceManager(item);
		}

		// Token: 0x06006543 RID: 25923 RVA: 0x001C6BD4 File Offset: 0x001C4DD4
		internal static bool TryGetValueFromXmlNode(object item, string name, out object value)
		{
			SystemXmlExtensionMethods systemXmlExtensionMethods = AssemblyHelper.ExtensionsForSystemXml(false);
			if (systemXmlExtensionMethods != null)
			{
				return systemXmlExtensionMethods.TryGetValueFromXmlNode(item, name, out value);
			}
			value = null;
			return false;
		}

		// Token: 0x06006544 RID: 25924 RVA: 0x001C6BFC File Offset: 0x001C4DFC
		internal static IComparer PrepareXmlComparer(IEnumerable collection, SortDescriptionCollection sort, CultureInfo culture)
		{
			SystemXmlExtensionMethods systemXmlExtensionMethods = AssemblyHelper.ExtensionsForSystemXml(false);
			if (systemXmlExtensionMethods != null)
			{
				return systemXmlExtensionMethods.PrepareXmlComparer(collection, sort, culture);
			}
			return null;
		}

		// Token: 0x06006545 RID: 25925 RVA: 0x001C6C20 File Offset: 0x001C4E20
		internal static bool IsEmptyXmlDataCollection(object parent)
		{
			SystemXmlExtensionMethods systemXmlExtensionMethods = AssemblyHelper.ExtensionsForSystemXml(false);
			return systemXmlExtensionMethods != null && systemXmlExtensionMethods.IsEmptyXmlDataCollection(parent);
		}

		// Token: 0x06006546 RID: 25926 RVA: 0x001C6C40 File Offset: 0x001C4E40
		internal static string GetXmlTagName(object item, DependencyObject target)
		{
			SystemXmlExtensionMethods systemXmlExtensionMethods = AssemblyHelper.ExtensionsForSystemXml(false);
			if (systemXmlExtensionMethods == null)
			{
				return null;
			}
			return systemXmlExtensionMethods.GetXmlTagName(item, target);
		}

		// Token: 0x06006547 RID: 25927 RVA: 0x001C6C64 File Offset: 0x001C4E64
		internal static object FindXmlNodeWithInnerText(IEnumerable items, object innerText, out int index)
		{
			index = -1;
			SystemXmlExtensionMethods systemXmlExtensionMethods = AssemblyHelper.ExtensionsForSystemXml(false);
			if (systemXmlExtensionMethods == null)
			{
				return DependencyProperty.UnsetValue;
			}
			return systemXmlExtensionMethods.FindXmlNodeWithInnerText(items, innerText, out index);
		}

		// Token: 0x06006548 RID: 25928 RVA: 0x001C6C90 File Offset: 0x001C4E90
		internal static object GetInnerText(object item)
		{
			SystemXmlExtensionMethods systemXmlExtensionMethods = AssemblyHelper.ExtensionsForSystemXml(false);
			if (systemXmlExtensionMethods == null)
			{
				return null;
			}
			return systemXmlExtensionMethods.GetInnerText(item);
		}
	}
}
