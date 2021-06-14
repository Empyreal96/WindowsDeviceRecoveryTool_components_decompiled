using System;
using System.Globalization;
using System.Windows;
using System.Windows.Markup;
using System.Xml;

namespace MS.Internal.IO.Packaging
{
	// Token: 0x0200066A RID: 1642
	internal class XmlGlyphRunInfo : GlyphRunInfo
	{
		// Token: 0x06006C9C RID: 27804 RVA: 0x001F40E7 File Offset: 0x001F22E7
		internal XmlGlyphRunInfo(XmlNode glyphsNode)
		{
			this._glyphsNode = (glyphsNode as XmlElement);
		}

		// Token: 0x170019F7 RID: 6647
		// (get) Token: 0x06006C9D RID: 27805 RVA: 0x001F40FB File Offset: 0x001F22FB
		internal override Point StartPosition
		{
			get
			{
				throw new NotSupportedException(SR.Get("XmlGlyphRunInfoIsNonGraphic"));
			}
		}

		// Token: 0x170019F8 RID: 6648
		// (get) Token: 0x06006C9E RID: 27806 RVA: 0x001F40FB File Offset: 0x001F22FB
		internal override Point EndPosition
		{
			get
			{
				throw new NotSupportedException(SR.Get("XmlGlyphRunInfoIsNonGraphic"));
			}
		}

		// Token: 0x170019F9 RID: 6649
		// (get) Token: 0x06006C9F RID: 27807 RVA: 0x001F40FB File Offset: 0x001F22FB
		internal override double WidthEmFontSize
		{
			get
			{
				throw new NotSupportedException(SR.Get("XmlGlyphRunInfoIsNonGraphic"));
			}
		}

		// Token: 0x170019FA RID: 6650
		// (get) Token: 0x06006CA0 RID: 27808 RVA: 0x001F40FB File Offset: 0x001F22FB
		internal override double HeightEmFontSize
		{
			get
			{
				throw new NotSupportedException(SR.Get("XmlGlyphRunInfoIsNonGraphic"));
			}
		}

		// Token: 0x170019FB RID: 6651
		// (get) Token: 0x06006CA1 RID: 27809 RVA: 0x001F40FB File Offset: 0x001F22FB
		internal override bool GlyphsHaveSidewaysOrientation
		{
			get
			{
				throw new NotSupportedException(SR.Get("XmlGlyphRunInfoIsNonGraphic"));
			}
		}

		// Token: 0x170019FC RID: 6652
		// (get) Token: 0x06006CA2 RID: 27810 RVA: 0x001F40FB File Offset: 0x001F22FB
		internal override int BidiLevel
		{
			get
			{
				throw new NotSupportedException(SR.Get("XmlGlyphRunInfoIsNonGraphic"));
			}
		}

		// Token: 0x170019FD RID: 6653
		// (get) Token: 0x06006CA3 RID: 27811 RVA: 0x001F410C File Offset: 0x001F230C
		internal override uint LanguageID
		{
			get
			{
				checked
				{
					if (this._languageID == null)
					{
						XmlElement xmlElement = this._glyphsNode;
						while (xmlElement != null && this._languageID == null)
						{
							string attribute = xmlElement.GetAttribute("xml:lang");
							if (attribute != null && attribute.Length > 0)
							{
								if (string.CompareOrdinal(attribute.ToUpperInvariant(), "UND") == 0)
								{
									this._languageID = new uint?(0U);
								}
								else
								{
									XmlLanguage language = XmlLanguage.GetLanguage(attribute);
									CultureInfo compatibleCulture = language.GetCompatibleCulture();
									this._languageID = new uint?((uint)compatibleCulture.LCID);
								}
							}
							xmlElement = (xmlElement.ParentNode as XmlElement);
						}
						if (this._languageID == null)
						{
							this._languageID = new uint?((uint)CultureInfo.InvariantCulture.LCID);
						}
					}
					return this._languageID.Value;
				}
			}
		}

		// Token: 0x170019FE RID: 6654
		// (get) Token: 0x06006CA4 RID: 27812 RVA: 0x001F41D4 File Offset: 0x001F23D4
		internal override string UnicodeString
		{
			get
			{
				if (this._unicodeString == null)
				{
					this._unicodeString = this._glyphsNode.GetAttribute("UnicodeString");
				}
				return this._unicodeString;
			}
		}

		// Token: 0x0400354F RID: 13647
		private const string _glyphRunName = "Glyphs";

		// Token: 0x04003550 RID: 13648
		private const string _xmlLangAttribute = "xml:lang";

		// Token: 0x04003551 RID: 13649
		private const string _unicodeStringAttribute = "UnicodeString";

		// Token: 0x04003552 RID: 13650
		private const string _undeterminedLanguageStringUpper = "UND";

		// Token: 0x04003553 RID: 13651
		private XmlElement _glyphsNode;

		// Token: 0x04003554 RID: 13652
		private string _unicodeString;

		// Token: 0x04003555 RID: 13653
		private uint? _languageID;
	}
}
