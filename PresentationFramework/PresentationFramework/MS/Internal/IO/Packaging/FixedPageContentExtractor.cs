using System;
using System.Xml;

namespace MS.Internal.IO.Packaging
{
	// Token: 0x0200065F RID: 1631
	internal class FixedPageContentExtractor
	{
		// Token: 0x06006C2C RID: 27692 RVA: 0x001F1E53 File Offset: 0x001F0053
		internal FixedPageContentExtractor(XmlNode fixedPage)
		{
			this._fixedPageInfo = new XmlFixedPageInfo(fixedPage);
			this._nextGlyphRun = 0;
		}

		// Token: 0x06006C2D RID: 27693 RVA: 0x001F1E70 File Offset: 0x001F0070
		internal string NextGlyphContent(out bool inline, out uint lcid)
		{
			inline = false;
			lcid = 0U;
			if (this._nextGlyphRun >= this._fixedPageInfo.GlyphRunCount)
			{
				return null;
			}
			GlyphRunInfo glyphRunInfo = this._fixedPageInfo.GlyphRunAtPosition(this._nextGlyphRun);
			lcid = glyphRunInfo.LanguageID;
			this._nextGlyphRun++;
			return glyphRunInfo.UnicodeString;
		}

		// Token: 0x170019DF RID: 6623
		// (get) Token: 0x06006C2E RID: 27694 RVA: 0x001F1EC6 File Offset: 0x001F00C6
		internal bool AtEndOfPage
		{
			get
			{
				return this._nextGlyphRun >= this._fixedPageInfo.GlyphRunCount;
			}
		}

		// Token: 0x04003517 RID: 13591
		private XmlFixedPageInfo _fixedPageInfo;

		// Token: 0x04003518 RID: 13592
		private int _nextGlyphRun;
	}
}
