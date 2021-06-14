using System;
using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Text;
using System.Windows;
using System.Windows.Markup;
using System.Xml;

namespace MS.Internal.Ink
{
	// Token: 0x02000696 RID: 1686
	internal class XamlClipboardData : ElementsClipboardData
	{
		// Token: 0x06006E07 RID: 28167 RVA: 0x001FA694 File Offset: 0x001F8894
		internal XamlClipboardData()
		{
		}

		// Token: 0x06006E08 RID: 28168 RVA: 0x001FA69C File Offset: 0x001F889C
		internal XamlClipboardData(UIElement[] elements) : base(elements)
		{
		}

		// Token: 0x06006E09 RID: 28169 RVA: 0x001FA6A8 File Offset: 0x001F88A8
		internal override bool CanPaste(IDataObject dataObject)
		{
			bool result = false;
			try
			{
				result = dataObject.GetDataPresent(DataFormats.Xaml, false);
			}
			catch (SecurityException)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06006E0A RID: 28170 RVA: 0x001FA6DC File Offset: 0x001F88DC
		protected override bool CanCopy()
		{
			return base.Elements != null && base.Elements.Count != 0;
		}

		// Token: 0x06006E0B RID: 28171 RVA: 0x001FA6F8 File Offset: 0x001F88F8
		[SecurityCritical]
		protected override void DoCopy(IDataObject dataObject)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (UIElement obj in base.Elements)
			{
				string value = XamlWriter.Save(obj);
				stringBuilder.Append(value);
			}
			dataObject.SetData(DataFormats.Xaml, stringBuilder.ToString());
			PermissionSet permissionSet = SecurityHelper.ExtractAppDomainPermissionSetMinusSiteOfOrigin();
			string data = permissionSet.ToString();
			dataObject.SetData(DataFormats.ApplicationTrust, data);
		}

		// Token: 0x06006E0C RID: 28172 RVA: 0x001FA788 File Offset: 0x001F8988
		protected override void DoPaste(IDataObject dataObject)
		{
			base.ElementList = new List<UIElement>();
			string text = dataObject.GetData(DataFormats.Xaml) as string;
			if (!string.IsNullOrEmpty(text))
			{
				bool useRestrictiveXamlReader = !Clipboard.UseLegacyDangerousClipboardDeserializationMode();
				UIElement uielement = XamlReader.Load(new XmlTextReader(new StringReader(text)), useRestrictiveXamlReader) as UIElement;
				if (uielement != null)
				{
					base.ElementList.Add(uielement);
				}
			}
		}
	}
}
