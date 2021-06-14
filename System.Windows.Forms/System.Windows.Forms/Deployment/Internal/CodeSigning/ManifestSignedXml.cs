using System;
using System.Security.Cryptography.Xml;
using System.Xml;

namespace System.Deployment.Internal.CodeSigning
{
	// Token: 0x0200000B RID: 11
	internal class ManifestSignedXml : SignedXml
	{
		// Token: 0x06000025 RID: 37 RVA: 0x00002706 File Offset: 0x00000906
		internal ManifestSignedXml()
		{
		}

		// Token: 0x06000026 RID: 38 RVA: 0x0000270E File Offset: 0x0000090E
		internal ManifestSignedXml(XmlElement elem) : base(elem)
		{
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002717 File Offset: 0x00000917
		internal ManifestSignedXml(XmlDocument document) : base(document)
		{
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002720 File Offset: 0x00000920
		internal ManifestSignedXml(XmlDocument document, bool verify) : base(document)
		{
			this.m_verify = verify;
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002730 File Offset: 0x00000930
		private static XmlElement FindIdElement(XmlElement context, string idValue)
		{
			if (context == null)
			{
				return null;
			}
			XmlElement xmlElement = context.SelectSingleNode("//*[@Id=\"" + idValue + "\"]") as XmlElement;
			if (xmlElement != null)
			{
				return xmlElement;
			}
			xmlElement = (context.SelectSingleNode("//*[@id=\"" + idValue + "\"]") as XmlElement);
			if (xmlElement != null)
			{
				return xmlElement;
			}
			return context.SelectSingleNode("//*[@ID=\"" + idValue + "\"]") as XmlElement;
		}

		// Token: 0x0600002A RID: 42 RVA: 0x000027A0 File Offset: 0x000009A0
		public override XmlElement GetIdElement(XmlDocument document, string idValue)
		{
			if (this.m_verify)
			{
				return base.GetIdElement(document, idValue);
			}
			KeyInfo keyInfo = base.KeyInfo;
			if (keyInfo.Id != idValue)
			{
				return null;
			}
			return keyInfo.GetXml();
		}

		// Token: 0x040000AE RID: 174
		private bool m_verify;
	}
}
