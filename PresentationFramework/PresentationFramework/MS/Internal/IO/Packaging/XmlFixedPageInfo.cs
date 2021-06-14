using System;
using System.Globalization;
using System.Windows;
using System.Xml;

namespace MS.Internal.IO.Packaging
{
	// Token: 0x02000669 RID: 1641
	internal class XmlFixedPageInfo : FixedPageInfo
	{
		// Token: 0x06006C97 RID: 27799 RVA: 0x001F3F9C File Offset: 0x001F219C
		internal XmlFixedPageInfo(XmlNode fixedPageNode)
		{
			this._pageNode = fixedPageNode;
			if (this._pageNode.LocalName != "FixedPage" || this._pageNode.NamespaceURI != ElementTableKey.FixedMarkupNamespace)
			{
				throw new ArgumentException(SR.Get("UnexpectedXmlNodeInXmlFixedPageInfoConstructor", new object[]
				{
					this._pageNode.NamespaceURI,
					this._pageNode.LocalName,
					ElementTableKey.FixedMarkupNamespace,
					"FixedPage"
				}));
			}
		}

		// Token: 0x06006C98 RID: 27800 RVA: 0x001F4026 File Offset: 0x001F2226
		internal override GlyphRunInfo GlyphRunAtPosition(int position)
		{
			if (position < 0 || position >= this.GlyphRunList.Length)
			{
				return null;
			}
			if (this.GlyphRunList[position] == null)
			{
				this.GlyphRunList[position] = new XmlGlyphRunInfo(this.NodeList[position]);
			}
			return this.GlyphRunList[position];
		}

		// Token: 0x170019F4 RID: 6644
		// (get) Token: 0x06006C99 RID: 27801 RVA: 0x001F4064 File Offset: 0x001F2264
		internal override int GlyphRunCount
		{
			get
			{
				return this.GlyphRunList.Length;
			}
		}

		// Token: 0x170019F5 RID: 6645
		// (get) Token: 0x06006C9A RID: 27802 RVA: 0x001F406E File Offset: 0x001F226E
		private XmlGlyphRunInfo[] GlyphRunList
		{
			get
			{
				if (this._glyphRunList == null)
				{
					this._glyphRunList = new XmlGlyphRunInfo[this.NodeList.Count];
				}
				return this._glyphRunList;
			}
		}

		// Token: 0x170019F6 RID: 6646
		// (get) Token: 0x06006C9B RID: 27803 RVA: 0x001F4094 File Offset: 0x001F2294
		private XmlNodeList NodeList
		{
			get
			{
				if (this._nodeList == null)
				{
					string xpath = string.Format(CultureInfo.InvariantCulture, ".//*[namespace-uri()='{0}' and local-name()='{1}']", new object[]
					{
						ElementTableKey.FixedMarkupNamespace,
						"Glyphs"
					});
					this._nodeList = this._pageNode.SelectNodes(xpath);
				}
				return this._nodeList;
			}
		}

		// Token: 0x0400354A RID: 13642
		private const string _fixedPageName = "FixedPage";

		// Token: 0x0400354B RID: 13643
		private const string _glyphRunName = "Glyphs";

		// Token: 0x0400354C RID: 13644
		private XmlNode _pageNode;

		// Token: 0x0400354D RID: 13645
		private XmlNodeList _nodeList;

		// Token: 0x0400354E RID: 13646
		private XmlGlyphRunInfo[] _glyphRunList;
	}
}
