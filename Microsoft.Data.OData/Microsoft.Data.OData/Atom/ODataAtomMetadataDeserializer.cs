using System;
using System.Globalization;
using System.Linq;
using System.Xml;
using Microsoft.Data.OData.Metadata;

namespace Microsoft.Data.OData.Atom
{
	// Token: 0x020001DF RID: 479
	internal abstract class ODataAtomMetadataDeserializer : ODataAtomDeserializer
	{
		// Token: 0x06000ECB RID: 3787 RVA: 0x00034170 File Offset: 0x00032370
		internal ODataAtomMetadataDeserializer(ODataAtomInputContext atomInputContext) : base(atomInputContext)
		{
			XmlNameTable nameTable = base.XmlReader.NameTable;
			this.EmptyNamespace = nameTable.Add(string.Empty);
			this.AtomNamespace = nameTable.Add("http://www.w3.org/2005/Atom");
		}

		// Token: 0x1700031A RID: 794
		// (get) Token: 0x06000ECC RID: 3788 RVA: 0x000341B2 File Offset: 0x000323B2
		protected bool ReadAtomMetadata
		{
			get
			{
				return base.AtomInputContext.MessageReaderSettings.EnableAtomMetadataReading;
			}
		}

		// Token: 0x06000ECD RID: 3789 RVA: 0x000341C4 File Offset: 0x000323C4
		protected AtomPersonMetadata ReadAtomPersonConstruct(EpmTargetPathSegment epmTargetPathSegment)
		{
			AtomPersonMetadata atomPersonMetadata = new AtomPersonMetadata();
			if (!base.XmlReader.IsEmptyElement)
			{
				base.XmlReader.Read();
				for (;;)
				{
					XmlNodeType nodeType = base.XmlReader.NodeType;
					if (nodeType != XmlNodeType.Element)
					{
						if (nodeType != XmlNodeType.EndElement)
						{
							goto IL_FE;
						}
					}
					else
					{
						EpmTargetPathSegment epmTargetPathSegment2;
						string localName;
						if (!base.XmlReader.NamespaceEquals(this.AtomNamespace) || !this.ShouldReadElement(epmTargetPathSegment, base.XmlReader.LocalName, out epmTargetPathSegment2) || (localName = base.XmlReader.LocalName) == null)
						{
							goto IL_FE;
						}
						if (!(localName == "name"))
						{
							if (!(localName == "uri"))
							{
								if (!(localName == "email"))
								{
									goto IL_FE;
								}
								atomPersonMetadata.Email = this.ReadElementStringValue();
							}
							else
							{
								Uri xmlBaseUri = base.XmlReader.XmlBaseUri;
								string text = this.ReadElementStringValue();
								if (epmTargetPathSegment2 != null)
								{
									atomPersonMetadata.UriFromEpm = text;
								}
								if (this.ReadAtomMetadata)
								{
									atomPersonMetadata.Uri = base.ProcessUriFromPayload(text, xmlBaseUri);
								}
							}
						}
						else
						{
							atomPersonMetadata.Name = this.ReadElementStringValue();
						}
					}
					IL_109:
					if (base.XmlReader.NodeType == XmlNodeType.EndElement)
					{
						break;
					}
					continue;
					IL_FE:
					base.XmlReader.Skip();
					goto IL_109;
				}
			}
			base.XmlReader.Read();
			return atomPersonMetadata;
		}

		// Token: 0x06000ECE RID: 3790 RVA: 0x000342FC File Offset: 0x000324FC
		protected DateTimeOffset? ReadAtomDateConstruct()
		{
			string text = this.ReadElementStringValue();
			text = text.Trim();
			if (text.Length >= 20)
			{
				if (text[19] == '.')
				{
					int num = 20;
					while (text.Length > num && char.IsDigit(text[num]))
					{
						num++;
					}
					text = text.Substring(0, 19) + text.Substring(num);
				}
				DateTimeOffset value;
				if (DateTimeOffset.TryParseExact(text, "yyyy-MM-ddTHH:mm:sszzz", CultureInfo.InvariantCulture.DateTimeFormat, DateTimeStyles.None, out value))
				{
					return new DateTimeOffset?(value);
				}
				if (DateTimeOffset.TryParseExact(text, "yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture.DateTimeFormat, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal, out value))
				{
					return new DateTimeOffset?(value);
				}
			}
			return new DateTimeOffset?(PlatformHelper.ConvertStringToDateTimeOffset(text));
		}

		// Token: 0x06000ECF RID: 3791 RVA: 0x000343B4 File Offset: 0x000325B4
		protected string ReadAtomDateConstructAsString()
		{
			return this.ReadElementStringValue();
		}

		// Token: 0x06000ED0 RID: 3792 RVA: 0x000343CC File Offset: 0x000325CC
		protected AtomTextConstruct ReadAtomTextConstruct()
		{
			AtomTextConstruct atomTextConstruct = new AtomTextConstruct();
			string text = null;
			while (base.XmlReader.MoveToNextAttribute())
			{
				if (base.XmlReader.NamespaceEquals(this.EmptyNamespace) && string.CompareOrdinal(base.XmlReader.LocalName, "type") == 0)
				{
					text = base.XmlReader.Value;
				}
			}
			base.XmlReader.MoveToElement();
			if (text != null)
			{
				string a;
				if ((a = text) != null)
				{
					if (a == "text")
					{
						atomTextConstruct.Kind = AtomTextConstructKind.Text;
						goto IL_C5;
					}
					if (a == "html")
					{
						atomTextConstruct.Kind = AtomTextConstructKind.Html;
						goto IL_C5;
					}
					if (a == "xhtml")
					{
						atomTextConstruct.Kind = AtomTextConstructKind.Xhtml;
						goto IL_C5;
					}
				}
				throw new ODataException(Strings.ODataAtomEntryMetadataDeserializer_InvalidTextConstructKind(text, base.XmlReader.LocalName));
			}
			atomTextConstruct.Kind = AtomTextConstructKind.Text;
			IL_C5:
			if (atomTextConstruct.Kind == AtomTextConstructKind.Xhtml)
			{
				atomTextConstruct.Text = base.XmlReader.ReadInnerXml();
			}
			else
			{
				atomTextConstruct.Text = this.ReadElementStringValue();
			}
			return atomTextConstruct;
		}

		// Token: 0x06000ED1 RID: 3793 RVA: 0x000344C7 File Offset: 0x000326C7
		protected string ReadElementStringValue()
		{
			if (base.UseClientFormatBehavior)
			{
				return base.XmlReader.ReadFirstTextNodeValue();
			}
			return base.XmlReader.ReadElementValue();
		}

		// Token: 0x06000ED2 RID: 3794 RVA: 0x000344E8 File Offset: 0x000326E8
		protected AtomTextConstruct ReadTitleElement()
		{
			return this.ReadAtomTextConstruct();
		}

		// Token: 0x06000ED3 RID: 3795 RVA: 0x00034510 File Offset: 0x00032710
		protected bool ShouldReadElement(EpmTargetPathSegment parentSegment, string segmentName, out EpmTargetPathSegment subSegment)
		{
			subSegment = null;
			if (parentSegment != null)
			{
				subSegment = parentSegment.SubSegments.FirstOrDefault((EpmTargetPathSegment segment) => string.CompareOrdinal(segment.SegmentName, segmentName) == 0);
				if (subSegment != null && subSegment.EpmInfo != null && subSegment.EpmInfo.Attribute.KeepInContent)
				{
					return this.ReadAtomMetadata;
				}
			}
			return subSegment != null || this.ReadAtomMetadata;
		}

		// Token: 0x0400051C RID: 1308
		private readonly string EmptyNamespace;

		// Token: 0x0400051D RID: 1309
		private readonly string AtomNamespace;
	}
}
